
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
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
/// The overall metrics are provided as an <see cref="OverallMetrics" />
/// object.
///
/// <para>
/// The calculations for graph density skip all self-loops, which would render
/// the density invalid.  The graph density is rendered invalid if the graph
/// has duplicate edges, however.
/// </para>
///
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
    /// <param name="overallMetrics">
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
        out OverallMetrics overallMetrics
    )
    {
        Debug.Assert(graph != null);

        Object oGraphMetricsAsObject;

        Boolean bReturn = TryCalculateGraphMetricsCore(graph, backgroundWorker,
            out oGraphMetricsAsObject);

        overallMetrics = (OverallMetrics)oGraphMetricsAsObject;

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

        oGraphMetrics = null;

        if (oBackgroundWorker != null)
        {
            if (oBackgroundWorker.CancellationPending)
            {
                return (false);
            }

            ReportProgress(5, 100, oBackgroundWorker);
        }

        Int32 iVertices = oGraph.Vertices.Count;
        Int32 iEdges = oGraph.Edges.Count;
        Int32 iSelfLoops = CountSelfLoops(oGraph);

        // The definition of graph density for an undirected graph is given
        // here:
        //
        // http://en.wikipedia.org/wiki/Dense_graph
        //
        // For directed graphs, the equation must be divided by 2 to account
        // for the doubled number of possible edges.
        //
        // The equation doesn't take self-loops into account.  They should not
        // be included.

        Int32 iNonSelfLoops = iEdges - iSelfLoops;

        Double dGraphDensity = OverallMetrics.NoGraphDensity;

        if (iVertices > 1)
        {
            Double dVertices = (Double)iVertices;

            dGraphDensity = (2 * (Double)iNonSelfLoops) /
                ( dVertices * (dVertices - 1) );

            if (oGraph.Directedness == GraphDirectedness.Directed)
            {
                dGraphDensity /= 2.0;
            }

            // Don't allow rounding errors to create a very small negative
            // number.

            dGraphDensity = Math.Max(0, dGraphDensity);
        }

        DuplicateEdgeDetector oDuplicateEdgeDetector =
            new DuplicateEdgeDetector(oGraph);

        OverallMetrics oOverallMetrics = new OverallMetrics(
            oGraph.Directedness,
            oDuplicateEdgeDetector.UniqueEdges,
            oDuplicateEdgeDetector.EdgesWithDuplicates,
            iEdges,
            iSelfLoops,
            iVertices,
            dGraphDensity
            );

        oGraphMetrics = oOverallMetrics;

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


//*****************************************************************************
//  Class: OverallMetrics
//
/// <summary>
/// Stores the overall metrics for a graph.
/// </summary>
//*****************************************************************************

public class OverallMetrics : Object
{
    //*************************************************************************
    //  Constructor: OverallMetrics()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="OverallMetrics" /> class.
    /// </summary>
    ///
    /// <param name="directedness">
    /// The graph's directedness.
    /// </param>
    ///
    /// <param name="uniqueEdges">
    /// The number of unique edges.
    /// </param>
    ///
    /// <param name="edgesWithDuplicates">
    /// The number of edges that have duplicates.
    /// </param>
    ///
    /// <param name="totalEdges">
    /// The total number of edges.
    /// </param>
    ///
    /// <param name="selfLoops">
    /// The number of self-loops.
    /// </param>
    ///
    /// <param name="vertices">
    /// The number of vertices.
    /// </param>
    ///
    /// <param name="graphDensity">
    /// The graph's density, or <see cref="NoGraphDensity" /> if the graph
    /// density can't be calculated.
    /// </param>
    //*************************************************************************

    public OverallMetrics
    (
        GraphDirectedness directedness,
        Int32 uniqueEdges,
        Int32 edgesWithDuplicates,
        Int32 totalEdges,
        Int32 selfLoops,
        Int32 vertices,
        Double graphDensity
    )
    {
        m_eDirectedness = directedness;
        m_iUniqueEdges = uniqueEdges;
        m_iEdgesWithDuplicates = edgesWithDuplicates;
        m_iTotalEdges = totalEdges;
        m_iSelfLoops = selfLoops;
        m_iVertices = vertices;
        m_dGraphDensity = graphDensity;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Directedness
    //
    /// <summary>
    /// Gets the graph's directedness.
    /// </summary>
    ///
    /// <value>
    /// The graph's directedness.
    /// </value>
    //*************************************************************************

    public GraphDirectedness
    Directedness
    {
        get
        {
            AssertValid();

            return (m_eDirectedness);
        }
    }

    //*************************************************************************
    //  Property: UniqueEdges
    //
    /// <summary>
    /// Gets the number of unique edges.
    /// </summary>
    ///
    /// <value>
    /// The number of unique edges.
    /// </value>
    //*************************************************************************

    public Int32
    UniqueEdges
    {
        get
        {
            AssertValid();

            return (m_iUniqueEdges);
        }
    }

    //*************************************************************************
    //  Property: EdgesWithDuplicates
    //
    /// <summary>
    /// Gets the number of edges that have duplicates.
    /// </summary>
    ///
    /// <value>
    /// The number of edges that have duplicates.
    /// </value>
    //*************************************************************************

    public Int32
    EdgesWithDuplicates
    {
        get
        {
            AssertValid();

            return (m_iEdgesWithDuplicates);
        }
    }

    //*************************************************************************
    //  Property: TotalEdges
    //
    /// <summary>
    /// Gets the total number of edges.
    /// </summary>
    ///
    /// <value>
    /// The total number of edges.
    /// </value>
    //*************************************************************************

    public Int32
    TotalEdges
    {
        get
        {
            AssertValid();

            return (m_iTotalEdges);
        }
    }

    //*************************************************************************
    //  Property: SelfLoops
    //
    /// <summary>
    /// Gets the number of self-loops.
    /// </summary>
    ///
    /// <value>
    /// The number of self-loops.
    /// </value>
    //*************************************************************************

    public Int32
    SelfLoops
    {
        get
        {
            AssertValid();

            return (m_iSelfLoops);
        }
    }

    //*************************************************************************
    //  Property: Vertices
    //
    /// <summary>
    /// Gets the number of vertices.
    /// </summary>
    ///
    /// <value>
    /// The number of vertices.
    /// </value>
    //*************************************************************************

    public Int32
    Vertices
    {
        get
        {
            AssertValid();

            return (m_iVertices);
        }
    }

    //*************************************************************************
    //  Property: GraphDensity
    //
    /// <summary>
    /// Gets the graph's density.
    /// </summary>
    ///
    /// <value>
    /// The graph's density, or <see cref="NoGraphDensity" /> if the graph
    /// density can't be calculated.
    /// </value>
    //*************************************************************************

    public Double
    GraphDensity
    {
        get
        {
            AssertValid();

            return (m_dGraphDensity);
        }
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        // m_eDirectedness
        Debug.Assert(m_iUniqueEdges >= 0);
        Debug.Assert(m_iEdgesWithDuplicates >= 0);
        Debug.Assert(m_iTotalEdges >= 0);
        Debug.Assert(m_iSelfLoops >= 0);
        Debug.Assert(m_iVertices >= 0);

        Debug.Assert(m_dGraphDensity == NoGraphDensity ||
            m_dGraphDensity >= 0);
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// <summary>
    /// Value of the <see cref="GraphDensity" /> property if the graph density
    /// can't be calculated.
    /// </summary>

    public const Double NoGraphDensity = Double.MinValue;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The graph's directedness.

    protected GraphDirectedness m_eDirectedness;

    /// The number of unique edges.

    protected Int32 m_iUniqueEdges;

    /// The number of edges that have duplicates.

    protected Int32 m_iEdgesWithDuplicates;

    /// The total number of edges.

    protected Int32 m_iTotalEdges;

    /// The number of self-loops.

    protected Int32 m_iSelfLoops;

    /// The number of vertices.

    protected Int32 m_iVertices;

    /// The graph's density, or NoGraphDensity.

    protected Double m_dGraphDensity;
}

}
