
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Algorithms
{
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
    /// The graph's density, or null if the graph density can't be calculated.
    /// </param>
    ///
    /// <param name="connectedComponents">
    /// The number of connected components in the graph.
    /// </param>
    ///
    /// <param name="singleVertexConnectedComponents">
    /// The number of connected components in the graph that have one vertex.
    /// </param>
    ///
    /// <param name="maximumConnectedComponentVertices">
    /// The maximum number of vertices in a connected component.
    /// </param>
    ///
    /// <param name="maximumConnectedComponentEdges">
    /// The maximum number of edges in a connected component.
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
        Nullable<Double> graphDensity,
        Int32 connectedComponents,
        Int32 singleVertexConnectedComponents,
        Int32 maximumConnectedComponentVertices,
        Int32 maximumConnectedComponentEdges
    )
    {
        m_eDirectedness = directedness;
        m_iUniqueEdges = uniqueEdges;
        m_iEdgesWithDuplicates = edgesWithDuplicates;
        m_iTotalEdges = totalEdges;
        m_iSelfLoops = selfLoops;
        m_iVertices = vertices;
        m_dGraphDensity = graphDensity;
        m_iConnectedComponents = connectedComponents;
        m_iSingleVertexConnectedComponents = singleVertexConnectedComponents;

        m_iMaximumConnectedComponentVertices =
            maximumConnectedComponentVertices;

        m_iMaximumConnectedComponentEdges = maximumConnectedComponentEdges;

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
    /// The graph's density, or null if the graph density can't be calculated.
    /// </value>
    //*************************************************************************

    public Nullable<Double>
    GraphDensity
    {
        get
        {
            AssertValid();

            return (m_dGraphDensity);
        }
    }

    //*************************************************************************
    //  Property: ConnectedComponents
    //
    /// <summary>
    /// Gets the number of connected components in the graph.
    /// </summary>
    ///
    /// <value>
    /// The number of connected components in the graph.
    /// </value>
    //*************************************************************************

    public Int32
    ConnectedComponents
    {
        get
        {
            AssertValid();

            return (m_iConnectedComponents);
        }
    }

    //*************************************************************************
    //  Property: SingleVertexConnectedComponents
    //
    /// <summary>
    /// Gets the number of connected components in the graph that have one
    /// vertex.
    /// </summary>
    ///
    /// <value>
    /// The number of connected components in the graph that have one vertex.
    /// </value>
    //*************************************************************************

    public Int32
    SingleVertexConnectedComponents
    {
        get
        {
            AssertValid();

            return (m_iSingleVertexConnectedComponents);
        }
    }

    //*************************************************************************
    //  Property: MaximumConnectedComponentVertices
    //
    /// <summary>
    /// Gets the maximum number of vertices in a connected component.
    /// vertex.
    /// </summary>
    ///
    /// <value>
    /// The maximum number of vertices in a connected component.
    /// </value>
    //*************************************************************************

    public Int32
    MaximumConnectedComponentVertices
    {
        get
        {
            AssertValid();

            return (m_iMaximumConnectedComponentVertices);
        }
    }

    //*************************************************************************
    //  Property: MaximumConnectedComponentEdges
    //
    /// <summary>
    /// Gets the maximum number of edges in a connected component.
    /// vertex.
    /// </summary>
    ///
    /// <value>
    /// The maximum number of edges in a connected component.
    /// </value>
    //*************************************************************************

    public Int32
    MaximumConnectedComponentEdges
    {
        get
        {
            AssertValid();

            return (m_iMaximumConnectedComponentEdges);
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
        Debug.Assert(!m_dGraphDensity.HasValue || m_dGraphDensity.Value >= 0);
        Debug.Assert(m_iConnectedComponents >= 0);
        Debug.Assert(m_iSingleVertexConnectedComponents >= 0);
        Debug.Assert(m_iMaximumConnectedComponentVertices >= 0);
        Debug.Assert(m_iMaximumConnectedComponentEdges >= 0);
    }


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

    protected Nullable<Double> m_dGraphDensity;

    /// The number of connected components in the graph.
    
    protected Int32 m_iConnectedComponents;

    /// The number of connected components in the graph that have one vertex.

    protected Int32 m_iSingleVertexConnectedComponents;

    /// The maximum number of vertices in a connected component.

    protected Int32 m_iMaximumConnectedComponentVertices;

    /// The maximum number of edges in a connected component.

    protected Int32 m_iMaximumConnectedComponentEdges;
}

}
