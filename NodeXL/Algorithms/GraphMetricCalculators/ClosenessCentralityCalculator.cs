
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: ClosenessCentralityCalculator
//
/// <summary>
/// Calculates the closeness centrality for each of the graph's vertices.
/// </summary>
///
/// <remarks>
/// The closeness centralities are provided as a
/// Dictionary&lt;Int32, Double&gt;.  There is one key/value pair for each
/// vertex in the graph.  The key is the IVertex.ID and the value is the
/// vertex's closeness centrality, as a Double.
///
/// <para>
/// The closeness centrality of a vertex is calculated as the mean geodesic
/// distance (the shortest path) between the vertex and all other vertices
/// reachable from it.  If the vertex is isolated, its closeness centrality is
/// zero.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class ClosenessCentralityCalculator : GraphMetricCalculatorBase
{
    //*************************************************************************
    //  Constructor: ClosenessCentralityCalculator()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ClosenessCentralityCalculator" /> class.
    /// </summary>
    //*************************************************************************

    public ClosenessCentralityCalculator()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: GraphMetricDescription
    //
    /// <summary>
    /// Gets a description of the graph metrics calculated by the
    /// implementation.
    /// </summary>
    ///
    /// <value>
    /// A description suitable for use within the sentence "Calculating
    /// [GraphMetricDescription]."
    /// </value>
    //*************************************************************************

    public override String
    GraphMetricDescription
    {
        get
        {
            AssertValid();

            return ("closeness centralities");
        }
    }

    //*************************************************************************
    //  Method: TryCalculateGraphMetrics()
    //
    /// <summary>
    /// Attempts to calculate a set of one or more related metrics while
    /// optionally running on a background thread, and provides the metrics in
    /// a type-safe manner.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <param name="backgroundWorker">
    /// The BackgroundWorker whose thread is calling this method, or null if
    /// the method is being called by some other thread.
    /// </param>
    ///
    /// <param name="closenessCentralities">
    /// Where the graph metrics get stored if true is returned.  See the class
    /// notes for details on the type.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated, false if the user wants to
    /// cancel.
    /// </returns>
    //*************************************************************************

    public Boolean
    TryCalculateGraphMetrics
    (
        IGraph graph,
        BackgroundWorker backgroundWorker,
        out Dictionary<Int32, Double> closenessCentralities
    )
    {
        Debug.Assert(graph != null);

        Object oGraphMetricsAsObject;

        Boolean bReturn = TryCalculateGraphMetricsCore(graph, backgroundWorker,
            out oGraphMetricsAsObject);

        closenessCentralities =
            ( Dictionary<Int32, Double> )oGraphMetricsAsObject;

        return (bReturn);
    }

    //*************************************************************************
    //  Method: TryCalculateGraphMetricsCore()
    //
    /// <summary>
    /// Attempts to calculate a set of one or more related metrics while
    /// optionally running on a background thread.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// The BackgroundWorker whose thread is calling this method, or null if
    /// the method is being called by some other thread.
    /// </param>
    ///
    /// <param name="oGraphMetrics">
    /// Where the graph metrics get stored if true is returned.  See the class
    /// notes for details on the type.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated, false if the user wants to
    /// cancel.
    /// </returns>
    //*************************************************************************

    protected override Boolean
    TryCalculateGraphMetricsCore
    (
        IGraph oGraph,
        BackgroundWorker oBackgroundWorker,
        out Object oGraphMetrics
    )
    {
        Debug.Assert(oGraph != null);
        AssertValid();

        IVertexCollection oVertices = oGraph.Vertices;

        Dictionary<Int32, Double> oClosenessCentralities =
            new Dictionary<Int32, Double>(oVertices.Count);

        oGraphMetrics = oClosenessCentralities;

        // Calculate the length of the shortest path between each pair of
        // vertices.

        UInt16 [,] aui16AllPairsShortestPathLengths;

        if ( !( new AllPathsShortestPathCalculator(
            this.GraphMetricDescription) ).TryCalculateGraphMetrics(
                oGraph, oBackgroundWorker,
                out aui16AllPairsShortestPathLengths) )
        {
            // The user cancelled.

            return (false);
        }

        // Calculate the closeness centralities.

        Int32 i = 0;

        foreach (IVertex oVertexI in oVertices)
        {
            Int32 j = 0;
            Int32 iPathLengthSum = 0;
            Int32 iOtherVerticesInConnectedComponent = 0;

            foreach (IVertex oVertexJ in oVertices)
            {
                if (i != j)
                {
                    UInt16 ui16PathLength =
                        aui16AllPairsShortestPathLengths[i, j];

                    if (ui16PathLength !=
                        AllPathsShortestPathCalculator.NoPath)
                    {
                        iPathLengthSum += ui16PathLength;
                        iOtherVerticesInConnectedComponent++;
                    }
                }

                j++;
            }

            Double dClosenessCentrality;

            if (iOtherVerticesInConnectedComponent == 0)
            {
                dClosenessCentrality = 0;
            }
            else
            {
                dClosenessCentrality = (Double)iPathLengthSum /
                    (Double)iOtherVerticesInConnectedComponent;
            }

            oClosenessCentralities.Add(oVertexI.ID, dClosenessCentrality);

            i++;
        }

        return (true);
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
        base.AssertValid();

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
