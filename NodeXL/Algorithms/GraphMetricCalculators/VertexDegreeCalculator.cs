
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: VertexDegreeCalculator
//
/// <summary>
/// Calculates the in-degree, out-degree, and degree for each of the graph's
/// vertices.
/// </summary>
///
/// <remarks>
/// The vertex degrees are provided as a
/// Dictionary&lt;Int32, VertexDegrees&gt;.  There is one key/value pair for
/// each vertex in the graph.  The key is the IVertex.ID and the value is a
/// <see cref="VertexDegrees" /> object containing the degree metrics for the
/// vertex.
///
/// <para>
/// This calculator includes all self-loops in its calculations.  It also
/// includes all parallel edges, which may not be expected by the user.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class VertexDegreeCalculator : GraphMetricCalculatorBase
{
    //*************************************************************************
    //  Constructor: VertexDegreeCalculator()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="VertexDegreeCalculator" /> class.
    /// </summary>
    //*************************************************************************

    public VertexDegreeCalculator()
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

            return ("vertex degrees");
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
    /// <param name="vertexDegrees">
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
        out Dictionary<Int32, VertexDegrees> vertexDegrees
    )
    {
        Debug.Assert(graph != null);

        Object oGraphMetricsAsObject;

        Boolean bReturn = TryCalculateGraphMetricsCore(graph, backgroundWorker,
            out oGraphMetricsAsObject);

        vertexDegrees =
            ( Dictionary<Int32, VertexDegrees> )oGraphMetricsAsObject;

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
        Int32 iCalculations = 0;

        Dictionary<Int32, VertexDegrees> oVertexIDDictionary =
            new Dictionary<Int32, VertexDegrees>(iVertices);

        oGraphMetrics = oVertexIDDictionary;

        foreach (IVertex oVertex in oVertices)
        {
            // Check for cancellation and report progress every
            // VerticesPerProgressReport calculations.

            if ( oBackgroundWorker != null &&
                (iCalculations % VerticesPerProgressReport == 0) )
            {
                if (oBackgroundWorker.CancellationPending)
                {
                    return (false);
                }

                ReportProgress(iCalculations, iVertices, oBackgroundWorker);
            }

            Int32 iInDegree, iOutDegree;

            CalculateVertexDegrees(oVertex, out iInDegree, out iOutDegree);

            oVertexIDDictionary.Add( oVertex.ID,
                new VertexDegrees(iInDegree, iOutDegree) );

            iCalculations++;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: CalculateVertexDegrees()
    //
    /// <summary>
    /// Calculates a vertex's in-degree and out-degree.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to calculate the in-degree and out-degree for.
    /// </param>
    ///
    /// <param name="iInDegree">
    /// Where the vertex's in-degree gets stored.
    /// </param>
    ///
    /// <param name="iOutDegree">
    /// Where the vertex's out-degree gets stored.
    /// </param>
    //*************************************************************************

    protected void
    CalculateVertexDegrees
    (
        IVertex oVertex,
        out Int32 iInDegree,
        out Int32 iOutDegree
    )
    {
        Debug.Assert(oVertex != null);
        AssertValid();

        iInDegree = 0;
        iOutDegree = 0;

        foreach (IEdge oIncidentEdge in oVertex.IncidentEdges)
        {
            IVertex [] aoVertices = oIncidentEdge.Vertices;

            // Test both of the edge's vertices so that a self-loop is properly
            // handled.

            if (aoVertices[0] == oVertex)
            {
                iOutDegree++;
            }

            if (aoVertices[1] == oVertex)
            {
                iInDegree++;
            }
        }
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


//*****************************************************************************
//  Class: VertexDegrees
//
/// <summary>
/// Stores the degree metrics for one vertex.
/// </summary>
///
/// <remarks>
/// Technically, in-degree and out-degree are valid only for directed graphs
/// and degree is valid only for undirected graphs, but all three metrics are
/// provided by this class anyway.  Degree is the sum of in-degree and
/// out-degree.
/// </remarks>
//*****************************************************************************

public class VertexDegrees : Object
{
    //*************************************************************************
    //  Constructor: VertexDegrees()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexDegrees" /> class.
    /// </summary>
    ///
    /// <param name="inDegree">
    /// The vertex's in-degree.
    /// </param>
    ///
    /// <param name="outDegree">
    /// The vertex's out-degree.
    /// </param>
    //*************************************************************************

    public VertexDegrees
    (
        Int32 inDegree,
        Int32 outDegree
    )
    {
        m_iInDegree = inDegree;
        m_iOutDegree = outDegree;

        AssertValid();
    }

    //*************************************************************************
    //  Property: InDegree
    //
    /// <summary>
    /// Gets the vertex's in-degree.
    /// </summary>
    ///
    /// <value>
    /// The vertex's in-degree.
    /// </value>
    //*************************************************************************

    public Int32
    InDegree
    {
        get
        {
            AssertValid();

            return (m_iInDegree);
        }
    }

    //*************************************************************************
    //  Property: OutDegree
    //
    /// <summary>
    /// Gets the vertex's out-degree.
    /// </summary>
    ///
    /// <value>
    /// The vertex's out-degree.
    /// </value>
    //*************************************************************************

    public Int32
    OutDegree
    {
        get
        {
            AssertValid();

            return (m_iOutDegree);
        }
    }

    //*************************************************************************
    //  Property: Degree
    //
    /// <summary>
    /// Gets the vertex's degree.
    /// </summary>
    ///
    /// <value>
    /// The vertex's degree.
    /// </value>
    //*************************************************************************

    public Int32
    Degree
    {
        get
        {
            AssertValid();

            return (m_iInDegree + m_iOutDegree);
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
        Debug.Assert(m_iInDegree >= 0);
        Debug.Assert(m_iOutDegree >= 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// In-degree.

    protected Int32 m_iInDegree;

    /// Out-degree.

    protected Int32 m_iOutDegree;
}

}
