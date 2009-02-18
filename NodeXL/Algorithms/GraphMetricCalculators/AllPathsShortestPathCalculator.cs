
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: AllPathsShortestPathCalculator
//
/// <summary>
/// Calculates the length of the shortest path between each pair of vertices in
/// a graph.
/// </summary>
///
/// <remarks>
/// The shortest paths are provided as a two-dimensional array of UInt16s.  The
/// [i,j] element is the length of the shortest path between vertices i and j
/// if there is such a path, or the constant <see cref="NoPath" /> if there is
/// no such path.  The i and j indexes assume that the graph's vertex
/// collection is being enumerated via a forward enumerator.  (The vertex
/// collection cannot be indexed and must be enumerated.)
///
/// <para>
/// The Floyd-Warshall algorithm is used to compute the shortest path lengths.
/// The algorithm is outlined here:
/// </para>
///
/// <para>
/// http://en.wikipedia.org/wiki/Floyd-Warshall_algorithm
/// </para>
///
/// </remarks>
//*****************************************************************************

public class AllPathsShortestPathCalculator : GraphMetricCalculatorBase
{
    //*************************************************************************
    //  Constructor: AllPathsShortestPathCalculator()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
    /// cref="AllPathsShortestPathCalculator" /> class with a default graph
    /// metric description.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AllPathsShortestPathCalculator" /> class.
    /// </summary>
    //*************************************************************************

    public AllPathsShortestPathCalculator()
    : this("shortest paths")
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: AllPathsShortestPathCalculator()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AllPathsShortestPathCalculator" /> class with a specified graph
    /// metric description.
    /// </summary>
    ///
    /// <param name="graphMetricDescription">
    /// A description suitable for use within the sentence "Calculating
    /// [GraphMetricDescription]."
    /// </param>
    ///
    /// <remarks>
    /// This overload is provided to allow another graph metric calculator to
    /// use this class while having its own graph metric description used in
    /// progress reports.
    /// </remarks>
    //*************************************************************************

    public AllPathsShortestPathCalculator
    (
        String graphMetricDescription
    )
    {
        m_sGraphMetricDescription = graphMetricDescription;

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

            return (m_sGraphMetricDescription);
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
    /// <param name="allPairsPathLengths">
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
        out UInt16 [,] allPairsPathLengths
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(backgroundWorker != null);

        Object oGraphMetricsAsObject;

        Boolean bReturn = TryCalculateGraphMetricsCore(graph, backgroundWorker,
            out oGraphMetricsAsObject);

        allPairsPathLengths = ( UInt16 [,] )oGraphMetricsAsObject;

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
    /// notes for the details on the type.
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
        Int32 iVertices = oVertices.Count;

        // Create an object that determines whether two vertices are connected
        // by an edge.

        SimpleGraphMatrix oSimpleGraphMatrix = new SimpleGraphMatrix(oGraph);

        UInt16 [,] aui16AllPairsPathLengths = null;

        try
        {
            aui16AllPairsPathLengths = new UInt16[iVertices, iVertices];
        }
        catch (OutOfMemoryException)
        {
            ReportCannotCalculateGraphMetrics( String.Format(

                "Calculating {0} requires a large amount of computer memory,"
                + " and there is not enough memory available."
                ,
                this.GraphMetricDescription
                ) );
        }

        oGraphMetrics = aui16AllPairsPathLengths;

        Int32 i = 0;
        Int32 j = 0;
        Int32 k = 0;
        UInt16 ui16IJPathLength;

        foreach (IVertex oVertexI in oVertices)
        {
            j = 0;

            foreach (IVertex oVertexJ in oVertices)
            {
                if ( i != j && oSimpleGraphMatrix.Aij(oVertexI, oVertexJ) )
                {
                    ui16IJPathLength = 1;
                }
                else
                {
                    ui16IJPathLength = NoPath;
                }

                aui16AllPairsPathLengths[i, j] = ui16IJPathLength;

                j++;
            }

            i++;
        }

        oSimpleGraphMatrix = null;

        for (k = 0; k < iVertices; k++)
        {
            for (i = 0; i < iVertices; i++)
            {
                for (j = 0; j < iVertices; j++)
                {
                    if (aui16AllPairsPathLengths[i, k] == NoPath ||
                        aui16AllPairsPathLengths[k, j] == NoPath)
                    {
                        ui16IJPathLength = aui16AllPairsPathLengths[i, j];
                    }
                    else
                    {
                        ui16IJPathLength = Math.Min(

                            aui16AllPairsPathLengths[i, j],

                            (UInt16)(aui16AllPairsPathLengths[i, k] +
                                aui16AllPairsPathLengths[k, j] )
                            );
                    }

                    aui16AllPairsPathLengths[i, j] = ui16IJPathLength;
                }
            }

            if (oBackgroundWorker != null)
            {
                if (oBackgroundWorker.CancellationPending)
                {
                    return (false);
                }

                ReportProgress(k, iVertices, oBackgroundWorker);
            }
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

        Debug.Assert( !String.IsNullOrEmpty(m_sGraphMetricDescription) );
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// Value used to indicate that there is no path between two vertices.

    public const UInt16 NoPath = UInt16.MaxValue;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Graph metric description.

    protected String m_sGraphMetricDescription;
}

}
