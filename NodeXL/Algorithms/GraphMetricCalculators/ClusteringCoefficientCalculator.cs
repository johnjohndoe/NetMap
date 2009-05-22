
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: ClusteringCoefficientCalculator
//
/// <summary>
/// Calculates the clustering coefficient for each of the graph's vertices.
/// </summary>
///
/// <remarks>
/// The clustering coefficients are provided as a
/// Dictionary&lt;Int32, Double&gt;.  There is one key/value pair for each
/// vertex in the graph.  The key is the IVertex.ID and the value is the
/// vertex's clustering coefficient, as a Double.
///
/// <para>
/// See this article for a definition of clustering coefficient:
/// </para>
///
/// <para>
/// http://en.wikipedia.org/wiki/Clustering_coefficient
/// </para>
///
/// <para>
/// This calculator skips all self-loops, which would render the calculations
/// invalid.  The calculations are rendered invalid if the graph has duplicate
/// edges, however.  You can check for duplicate edges with <see
/// cref="DuplicateEdgeDetector" />.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class ClusteringCoefficientCalculator : GraphMetricCalculatorBase
{
    //*************************************************************************
    //  Constructor: ClusteringCoefficientCalculator()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ClusteringCoefficientCalculator" /> class.
    /// </summary>
    //*************************************************************************

    public ClusteringCoefficientCalculator()
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

            return ("clustering coefficients");
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
    /// <param name="clusteringCoefficients">
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
        out Dictionary<Int32, Double> clusteringCoefficients
    )
    {
        Debug.Assert(graph != null);

        Object oGraphMetricsAsObject;

        Boolean bReturn = TryCalculateGraphMetricsCore(graph, backgroundWorker,
            out oGraphMetricsAsObject);

        clusteringCoefficients =
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
        Int32 iVertices = oVertices.Count;

        Dictionary<Int32, Double> oClusteringCoefficients =
            new Dictionary<Int32, Double>(iVertices);

        oGraphMetrics = oClusteringCoefficients;

        Boolean bGraphIsDirected =
            (oGraph.Directedness == GraphDirectedness.Directed);

        Int32 iCalculations = 0;

        foreach (IVertex oVertex in oVertices)
        {
            // Check for cancellation and report progress every
            // VerticesPerProgressReport calculations.

            if (oBackgroundWorker != null &&
                iCalculations % VerticesPerProgressReport == 0)
            {
                if (oBackgroundWorker.CancellationPending)
                {
                    return (false);
                }

                ReportProgress(iCalculations, iVertices, oBackgroundWorker);
            }

            oClusteringCoefficients.Add(oVertex.ID,
                CalculateClusteringCoefficient(oVertex, bGraphIsDirected) );

            iCalculations++;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: CalculateClusteringCoefficient()
    //
    /// <summary>
    /// Calculates a vertex's clustering coefficient.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to calculate the clustering coefficient for.
    /// </param>
    ///
    /// <param name="bGraphIsDirected">
    /// true if the graph is directed, false if it's undirected.
    /// </param>
    ///
    /// <returns>
    /// The vertex's clustering coefficient.
    /// </returns>
    //*************************************************************************

    protected Double
    CalculateClusteringCoefficient
    (
        IVertex oVertex,
        Boolean bGraphIsDirected
    )
    {
        Debug.Assert(oVertex != null);
        AssertValid();

        IVertex [] aoAdjacentVertices = oVertex.AdjacentVertices;
        Int32 iAdjacentVertices = 0;
        Int32 iVertexID = oVertex.ID;

        // Create a dictionary of the vertex's adjacent vertices.  The key is
        // the IVertex.ID and the value isn't used.

        Dictionary<Int32, Char> oAdjacentVertexIDDictionary =
            new Dictionary<Int32, Char>();

        foreach (IVertex oAdjacentVertex in aoAdjacentVertices)
        {
            Int32 iAdjacentVertexID = oAdjacentVertex.ID;

            // Skip self-loops.

            if (iAdjacentVertexID == iVertexID)
            {
                continue;
            }

            oAdjacentVertexIDDictionary.Add(iAdjacentVertexID, ' ');

            iAdjacentVertices++;
        }

        if (iAdjacentVertices == 0)
        {
            return (0);
        }

        // Create a dictionary of the unique edges in the vertex's
        // neighborhood.  These are the edges that connect adjacent vertices to
        // each other but not to the vertex.  The key is the IEdge.ID and the
        // value isn't used.

        Dictionary<Int32, Char> oEdgesInNeighborhoodIDDictionary =
            new Dictionary<Int32, Char>();

        // Loop through the vertex's adjacent vertices.

        foreach (IVertex oAdjacentVertex in aoAdjacentVertices)
        {
            // Skip self-loops.

            if (oAdjacentVertex.ID == iVertexID)
            {
                continue;
            }

            // Loop through the adjacent vertex's incident edges.

            foreach (IEdge oIncidentEdge in oAdjacentVertex.IncidentEdges)
            {
                if (oIncidentEdge.IsSelfLoop)
                {
                    continue;
                }

                // If this incident edge connects the adjacent vertex to
                // another adjacent vertex, add it to the dictionary if it
                // isn't already there.

                if ( oAdjacentVertexIDDictionary.ContainsKey(
                    oIncidentEdge.GetAdjacentVertex(oAdjacentVertex).ID) )
                {
                    oEdgesInNeighborhoodIDDictionary[oIncidentEdge.ID] = ' ';
                }
            }
        }

        Double dNumerator = (Double)oEdgesInNeighborhoodIDDictionary.Count;

        Debug.Assert(iAdjacentVertices > 0);

        Double dDenominator = CalculateEdgesInFullyConnectedNeighborhood(
            iAdjacentVertices, bGraphIsDirected);

        if (dDenominator == 0)
        {
            return (0);
        }

        return (dNumerator / dDenominator);
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
    //  Protected constants
    //*************************************************************************

    /// Number of vertices that are processed before progress is reported and
    /// the cancellation flag is checked.

    protected const Int32 VerticesPerProgressReport = 100;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
