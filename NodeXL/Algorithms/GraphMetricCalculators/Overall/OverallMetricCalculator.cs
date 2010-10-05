
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: OverallMetricCalculator
//
/// <summary>
/// Calculates the overall metrics for the graph.
/// </summary>
///
/// <remarks>
/// The calculations for <see cref="OverallMetrics.GraphDensity" /> skip all
/// self-loops and duplicate edges, which would render the density invalid.
/// </remarks>
//*****************************************************************************

public class OverallMetricCalculator : GraphMetricCalculatorBase
{
    //*************************************************************************
    //  Constructor: OverallMetricCalculator()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="OverallMetricCalculator" /> class.
    /// </summary>
    //*************************************************************************

    public OverallMetricCalculator()
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

            return ("overall metrics");
        }
    }

    //*************************************************************************
    //  Method: CalculateGraphMetrics()
    //
    /// <summary>
    /// Calculate the graph metrics.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <returns>
    /// The graph metrics.
    /// </returns>
    //*************************************************************************

    public OverallMetrics
    CalculateGraphMetrics
    (
        IGraph graph
    )
    {
        Debug.Assert(graph != null);
        AssertValid();

        OverallMetrics oGraphMetrics;

        TryCalculateGraphMetrics(graph, null, out oGraphMetrics);

        return (oGraphMetrics);
    }

    //*************************************************************************
    //  Method: TryCalculateGraphMetrics()
    //
    /// <summary>
    /// Attempts to calculate the graph metrics while optionally running on a
    /// background thread.
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
    /// <param name="graphMetrics">
    /// Where the graph metrics get stored if true is returned.  
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
        out OverallMetrics graphMetrics
    )
    {
        Debug.Assert(graph != null);
        AssertValid();

        graphMetrics = null;

        if (backgroundWorker != null)
        {
            if (backgroundWorker.CancellationPending)
            {
                return (false);
            }

            ReportProgress(1, 3, backgroundWorker);
        }

        DuplicateEdgeDetector oDuplicateEdgeDetector =
            new DuplicateEdgeDetector(graph);

        Int32 iVertices = graph.Vertices.Count;
        Int32 iEdges = graph.Edges.Count;
        Int32 iSelfLoops = CountSelfLoops(graph);

        Int32 iConnectedComponents, iSingleVertexConnectedComponents,
            iMaximumConnectedComponentVertices,
            iMaximumConnectedComponentEdges;

        CalculateConnectedComponentMetrics(graph, out iConnectedComponents,
            out iSingleVertexConnectedComponents,
            out iMaximumConnectedComponentVertices,
            out iMaximumConnectedComponentEdges);

        Nullable<Int32> iMaximumGeodesicDistance;
        Nullable<Double> dAverageGeodesicDistance;

        if (backgroundWorker != null)
        {
            ReportProgress(2, 3, backgroundWorker);
        }

        CalculateGeodesicDistances(graph, out iMaximumGeodesicDistance,
            out dAverageGeodesicDistance);

        OverallMetrics oOverallMetrics = new OverallMetrics(
            graph.Directedness,
            oDuplicateEdgeDetector.UniqueEdges,
            oDuplicateEdgeDetector.EdgesWithDuplicates,
            iSelfLoops,
            iVertices,

            CalculateGraphDensity(graph, iVertices,
                oDuplicateEdgeDetector.
                    TotalEdgesAfterMergingDuplicatesNoSelfLoops),

            iConnectedComponents,
            iSingleVertexConnectedComponents,
            iMaximumConnectedComponentVertices,
            iMaximumConnectedComponentEdges,
            iMaximumGeodesicDistance,
            dAverageGeodesicDistance
            );

        graphMetrics = oOverallMetrics;

        return (true);
    }

    //*************************************************************************
    //  Method: CountSelfLoops()
    //
    /// <summary>
    /// Counts the number of self-loops in the graph.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <returns>
    /// The number of self-loops in the graph.
    /// </returns>
    //*************************************************************************

    protected Int32
    CountSelfLoops
    (
        IGraph oGraph
    )
    {
        Debug.Assert(oGraph != null);
        AssertValid();

        Int32 iSelfLoops = 0;

        foreach (IEdge oEdge in oGraph.Edges)
        {
            if (oEdge.IsSelfLoop)
            {
                iSelfLoops++;
            }
        }

        return (iSelfLoops);
    }

    //*************************************************************************
    //  Method: CalculateGraphDensity()
    //
    /// <summary>
    /// Calculates the graph's density.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <param name="iVertices">
    /// The number of vertices in the graph.
    /// </param>
    ///
    /// <param name="iTotalEdgesAfterMergingDuplicatesNoSelfLoops">
    /// The total number of edges the graph would have if its duplicate edges
    /// were merged and all self-loops were removed.
    /// </param>
    ///
    /// <returns>
    /// The graph density, or null if it couldn't be calculated.
    /// </returns>
    //*************************************************************************

    protected Nullable<Double>
    CalculateGraphDensity
    (
        IGraph oGraph,
        Int32 iVertices,
        Int32 iTotalEdgesAfterMergingDuplicatesNoSelfLoops
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(iVertices >= 0);
        Debug.Assert(iTotalEdgesAfterMergingDuplicatesNoSelfLoops >= 0);
        AssertValid();

        // The definition of graph density for an undirected graph is given
        // here:
        //
        // http://en.wikipedia.org/wiki/Dense_graph
        //
        // For directed graphs, the equation must be divided by 2 to account
        // for the doubled number of possible edges.
        //
        // The equation doesn't take self-loops or duplicates into account.
        // They should not be included.

        Nullable<Double> dGraphDensity = null;

        if (iVertices > 1)
        {
            Double dVertices = (Double)iVertices;

            dGraphDensity =
                (2 * (Double)iTotalEdgesAfterMergingDuplicatesNoSelfLoops) /
                ( dVertices * (dVertices - 1) );

            if (oGraph.Directedness == GraphDirectedness.Directed)
            {
                dGraphDensity /= 2.0;
            }

            // Don't allow rounding errors to create a very small negative
            // number.

            dGraphDensity = Math.Max(0, dGraphDensity.Value);
        }

        return (dGraphDensity);
    }

    //*************************************************************************
    //  Method: CalculateConnectedComponentMetrics()
    //
    /// <summary>
    /// Calculates the graph's connected component metrics.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <param name="iConnectedComponents">
    /// Where the number of connected components in the graph gets stored.
    /// </param>
    ///
    /// <param name="iSingleVertexConnectedComponents">
    /// Where the number of connected components in the graph that have one
    /// vertex gets stored.
    /// </param>
    ///
    /// <param name="iMaximumConnectedComponentVertices">
    /// Where the maximum number of vertices in a connected component gets
    /// stored.
    /// </param>
    ///
    /// <param name="iMaximumConnectedComponentEdges">
    /// Where the maximum number of edges in a connected component gets stored.
    /// </param>
    //*************************************************************************

    protected void
    CalculateConnectedComponentMetrics
    (
        IGraph oGraph,
        out Int32 iConnectedComponents,
        out Int32 iSingleVertexConnectedComponents,
        out Int32 iMaximumConnectedComponentVertices,
        out Int32 iMaximumConnectedComponentEdges
    )
    {
        Debug.Assert(oGraph != null);
        AssertValid();

        ConnectedComponentCalculator oConnectedComponentCalculator =
            new ConnectedComponentCalculator();

        IList< LinkedList<IVertex> > oConnectedComponents =
            oConnectedComponentCalculator.CalculateStronglyConnectedComponents(
                oGraph, true);

        iConnectedComponents = oConnectedComponents.Count;
        iSingleVertexConnectedComponents = 0;
        iMaximumConnectedComponentVertices = 0;
        iMaximumConnectedComponentEdges = 0;

        foreach (LinkedList<IVertex> oConnectedComponent in
            oConnectedComponents)
        {
            Int32 iVertices = oConnectedComponent.Count;

            if (iVertices == 1)
            {
                iSingleVertexConnectedComponents++;
            }

            iMaximumConnectedComponentVertices = Math.Max(
                iMaximumConnectedComponentVertices, iVertices);

            iMaximumConnectedComponentEdges = Math.Max(
                iMaximumConnectedComponentEdges,
                CountUniqueEdges(oConnectedComponent) );
        }
    }

    //*************************************************************************
    //  Method: CalculateGeodesicDistances()
    //
    /// <summary>
    /// Calculates the graph's maximum and average geodesic distances.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <param name="iMaximumGeodesicDistance">
    /// Where the maximum geodesic distance in the graph gets stored.  Gets set
    /// to null if the graph is empty.
    /// </param>
    ///
    /// <param name="dAverageGeodesicDistance">
    /// Where the average geodesic distance in the graph gets stored.  Gets set
    /// to null if the graph is empty.
    /// </param>
    //*************************************************************************

    protected void
    CalculateGeodesicDistances
    (
        IGraph oGraph,
        out Nullable<Int32> iMaximumGeodesicDistance,
        out Nullable<Double> dAverageGeodesicDistance
    )
    {
        Debug.Assert(oGraph != null);
        AssertValid();

        iMaximumGeodesicDistance = new Nullable<Int32>();
        dAverageGeodesicDistance = new Nullable<Double>();

        if (oGraph.Vertices.Count == 0 || oGraph.Edges.Count == 0)
        {
            return;
        }

        String sOutputFilePath = CalculateSnapGraphMetrics(oGraph,
            SnapGraphMetrics.GeodesicDistances);

        using ( StreamReader oStreamReader = new StreamReader(
            sOutputFilePath) ) 
        {
            // The first line is a header.

            String sLine = oStreamReader.ReadLine();

            Debug.Assert(sLine ==
                "Maximum Geodesic Distance\tAverage Geodesic Distance");

            // The second line contains the metric values.

            Debug.Assert(oStreamReader.Peek() >= 0);

            sLine = oStreamReader.ReadLine();
            String [] asFields = sLine.Split('\t');
            Debug.Assert(asFields.Length == 2);

            iMaximumGeodesicDistance =
                ParseSnapInt32GraphMetricValue(asFields, 0);

            dAverageGeodesicDistance =
                ParseSnapDoubleGraphMetricValue(asFields, 1);
        }

        File.Delete(sOutputFilePath);
    }

    //*************************************************************************
    //  Method: CountUniqueEdges()
    //
    /// <summary>
    /// Counts the unique edges in a connected component.
    /// </summary>
    ///
    /// <param name="oConnectedComponent">
    /// The connected component to count edges for.
    /// </param>
    ///
    /// <returns>
    /// The number of unique edges in <paramref name="oConnectedComponent" />.
    /// </returns>
    //*************************************************************************

    protected Int32
    CountUniqueEdges
    (
        LinkedList<IVertex> oConnectedComponent
    )
    {
        Debug.Assert(oConnectedComponent != null);
        AssertValid();

        HashSet<Int32> oUniqueEdgeIDs = new HashSet<Int32>();

        foreach (IVertex oVertex in oConnectedComponent)
        {
            foreach (IEdge oEdge in oVertex.IncidentEdges)
            {
                oUniqueEdgeIDs.Add(oEdge.ID);
            }
        }

        return (oUniqueEdgeIDs.Count);
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
