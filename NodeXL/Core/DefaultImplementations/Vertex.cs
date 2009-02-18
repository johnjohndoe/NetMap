
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.Core
{
//*****************************************************************************
//  Class: Vertex
//
/// <summary>
/// Provides a default vertex implementation.
/// </summary>
///
/// <remarks>
/// A vertex, also known as a node, is a point in a graph that can be connected
/// to other vertices in the same graph.  The connections are called edges.
///
/// <para>
/// A <see cref="Vertex" /> can be created via its constructor and then added
/// to a graph via IGraph.Vertices.<see
/// cref="IVertexCollection.Add(IVertex)" />, or created and added to a graph
/// at the same time via IGraph.Vertices.<see
/// cref="IVertexCollection.Add()" />.
/// </para>
///
/// <para>
/// A vertex can be added to one graph only.  It cannot be added to a second
/// graph unless it is first removed from the first graph.
/// </para>
///
/// <para>
/// This class can be used as-is in many graphing applications.  You can also
/// customize it via inheritance, or implement your own vertex class from
/// scratch.  All vertex classes must implement the <see cref="IVertex" />
/// interface.  If you implement a vertex class, you may also want to implement
/// a corresponding <see cref="IVertexFactory" /> class.
/// </para>
///
/// </remarks>
///
/// <example>
/// The following code creates a <see cref="Vertex" /> object and adds it to a
/// graph.
///
/// <code>
/// oGraph.Vertices.Add();
/// </code>
///
/// </example>
///
/// <seealso cref="IVertex" />
/// <seealso cref="IVertexCollection" />
//*****************************************************************************

public class Vertex : GraphVertexEdgeBase, IVertex
{
    //*************************************************************************
    //  Constructor: Vertex()
    //
    /// <overloads>
    /// Static constructor for the <see cref="Vertex" /> class.
    /// </overloads>
    //*************************************************************************

    static Vertex()
    {
        m_oIDGenerator = new IDGenerator();
    }

    //*************************************************************************
    //  Constructor: Vertex()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="Vertex" /> class.
    /// </summary>
    //*************************************************************************

    public Vertex()
    :
    base( m_oIDGenerator.GetNextID() )
    {
        m_oParentGraph = null;

        m_oFirstIncidentEdgeNode = null;

        m_oLocation = PointF.Empty;

        AssertValid();
    }

    //*************************************************************************
    //  Property: ParentGraph
    //
    /// <summary>
    /// Gets the graph that owns the vertex.
    /// </summary>
    ///
    /// <value>
    /// The graph that owns the vertex, as an <see cref="IGraph" />, or null if
    /// the vertex does not belong to a graph.
    /// </value>
    ///
    /// <remarks>
    /// This is a read-only property.  When the vertex is added to a graph,
    /// this property is automatically set to that graph.  If the vertex is
    /// removed from the graph, this property is set to null.
    /// </remarks>
    //*************************************************************************

    public IGraph
    ParentGraph
    {
        get
        {
            AssertValid();

            return (m_oParentGraph);
        }
    }

    //*************************************************************************
    //  Property: IncomingEdges
    //
    /// <summary>
    /// Gets an array of the vertex's incoming edges.
    /// </summary>
    ///
    /// <value>
    /// An array of the vertex's zero or more incoming edges, as an array of
    /// <see cref="IEdge" /> objects.
    /// </value>
    ///
    /// <remarks>
    /// An incoming edge is either a directed edge that has this vertex at its
    /// front, or an undirected edge connected to this vertex.
    ///
    /// <para>
    /// A self-loop (an edge that connects a vertex to itself) is considered
    /// one incoming edge.
    /// </para>
    ///
    /// <para>
    /// If there are no incoming edges, the returned array is empty.  The
    /// returned value is never null.
    /// </para>
    ///
    /// <para>
    /// This property is an O(n) operation, where n is the number of edges
    /// incident to this vertex.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="OutgoingEdges" />
    /// <seealso cref="IncidentEdges" />
    //*************************************************************************

    [ DebuggerBrowsable(DebuggerBrowsableState.Never) ]

    public IEdge []
    IncomingEdges
    {
        get
        {
            AssertValid();

            return ( GetIncomingOrOutgoingEdges(true, false) );
        }
    }

    //*************************************************************************
    //  Property: OutgoingEdges
    //
    /// <summary>
    /// Gets an array of the vertex's outgoing edges.
    /// </summary>
    ///
    /// <value>
    /// An array of the vertex's zero or more outgoing edges, as an array of
    /// <see cref="IEdge" /> objects.
    /// </value>
    ///
    /// <remarks>
    /// An outgoing edge is either a directed edge that has this vertex at its
    /// back, or an undirected edge connected to this vertex.
    ///
    /// <para>
    /// A self-loop (an edge that connects a vertex to itself) is considered
    /// one outgoing edge.
    /// </para>
    ///
    /// <para>
    /// If there are no outgoing edges, the returned array is empty.  The
    /// returned value is never null.
    /// </para>
    ///
    /// <para>
    /// This property is an O(n) operation, where n is the number of edges
    /// incident to this vertex.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="IncomingEdges" />
    /// <seealso cref="IncidentEdges" />
    //*************************************************************************

    [ DebuggerBrowsable(DebuggerBrowsableState.Never) ]

    public IEdge []
    OutgoingEdges
    {
        get
        {
            AssertValid();

            return ( GetIncomingOrOutgoingEdges(false, true) );
        }
    }

    //*************************************************************************
    //  Property: IncidentEdges
    //
    /// <summary>
    /// Gets an array of the vertex's incident edges.
    /// </summary>
    ///
    /// <value>
    /// An array of the vertex's zero or more incident edges, as an array of
    /// <see cref="IEdge" /> objects.
    /// </value>
    ///
    /// <remarks>
    /// An incident edge is an edge that is connected to the vertex.
    ///
    /// <para>
    /// The returned array is the union of the <see cref="IncomingEdges" /> and
    /// <see cref="OutgoingEdges" /> arrays.
    /// </para>
    ///
    /// <para>
    /// A self-loop (an edge that connects a vertex to itself) is considered
    /// one incident edge.
    /// </para>
    ///
    /// <para>
    /// If there are no incident edges, the returned array is empty.  The
    /// returned value is never null.
    /// </para>
    ///
    /// <para>
    /// This property is an O(n) operation, where n is the number of edges
    /// incident to this vertex.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="IncomingEdges" />
    /// <seealso cref="IncidentEdges" />
    //*************************************************************************

    [ DebuggerBrowsable(DebuggerBrowsableState.Never) ]

    public IEdge []
    IncidentEdges
    {
        get
        {
            AssertValid();

            return ( GetIncomingOrOutgoingEdges(true, true) );
        }
    }

    //*************************************************************************
    //  Property: Location
    //
    /// <summary>
    /// Gets or sets the vertex's location.
    /// </summary>
    ///
    /// <value>
    /// The vertex's location as a <see cref="PointF" />.  The default value is
    /// cref="PointF.Empty" />.
    /// </value>
    ///
    /// <remarks>
    /// This property is set when the graph is laid out by
    /// ILayout.LayOutGraph and is read when the graph is drawn.
    /// </remarks>
    //*************************************************************************

    public PointF
    Location
    {
        get
        {
            AssertValid();

            return (m_oLocation);
        }

        set
        {
            m_oLocation = value;

            AssertValid();
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
    /// The vertex's degree, as an Int32.
    /// </value>
    ///
    /// <remarks>
    /// The degree of a vertex is the number of edges that are incident to it.
    /// (An incident edge is an edge that is connected to this vertex.)
    ///
    /// <para>
    /// A self-loop (an edge that connects a vertex to itself) is considered
    /// one incident edge.
    /// </para>
    ///
    /// <para>
    /// This property returns the same value as <see
    /// cref="IncidentEdges" />.Length.
    /// </para>
    ///
    /// <para>
    /// This property is an O(n) operation, where n is the number of edges
    /// incident to this vertex.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="IncomingEdges" />
    /// <seealso cref="OutgoingEdges" />
    /// <seealso cref="IncidentEdges" />
    //*************************************************************************

    public Int32
    Degree
    {
        get
        {
            AssertValid();

            EdgeCollection oEdgeCollection;

            if ( !GetEdgeCollection(out oEdgeCollection) )
            {
                // The vertex does not belong to a graph.

                return (0);
            }

            return ( oEdgeCollection.GetDegree(this) );
        }
    }

    //*************************************************************************
    //  Property: PredecessorVertices
    //
    /// <summary>
    /// Gets an array of the vertex's predecessor vertices.
    /// </summary>
    ///
    /// <value>
    /// An array of the vertex's zero or more predecessor vertices, as an array
    /// of <see cref="IVertex" /> objects.
    /// </value>
    ///
    /// <remarks>
    /// A predecessor vertex is a vertex at the other side of an incoming edge.
    /// (An incoming edge is either a directed edge that has this vertex at its
    /// front, or an undirected edge connected to this vertex.)
    ///
    /// <para>
    /// A self-loop (an edge that connects a vertex to itself) is always
    /// considered an incoming edge.  Therefore, if there is an edge that
    /// connects this vertex to itself, then this vertex is included in the
    /// returned array.
    /// </para>
    ///
    /// <para>
    /// The predecessor vertices in the returned array are unique.  If two or
    /// more incoming edges connect this vertex with another vertex, the other
    /// vertex is included once only.
    /// </para>
    ///
    /// <para>
    /// If there are no predecessor vertices, the returned array is empty.  The
    /// returned value is never null.
    /// </para>
    ///
    /// <para>
    /// This property is an O(n) operation, where n is the number of edges
    /// incident to this vertex.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="SuccessorVertices" />
    /// <seealso cref="AdjacentVertices" />
    //*************************************************************************

    [ DebuggerBrowsable(DebuggerBrowsableState.Never) ]

    public IVertex []
    PredecessorVertices
    {
        get
        {
            AssertValid();

            return ( GetPredecessorOrSuccessorVertices(true, false) );
        }
    }

    //*************************************************************************
    //  Property: SuccessorVertices
    //
    /// <summary>
    /// Gets an array of the vertex's successor vertices.
    /// </summary>
    ///
    /// <value>
    /// An array of the vertex's zero or more successor vertices, as an array
    /// of <see cref="IVertex" /> objects.
    /// </value>
    ///
    /// <remarks>
    /// A successor vertex is a vertex at the other side of an outgoing edge.
    /// (An outgoing edge is either a directed edge that has this vertex at its
    /// back, or an undirected edge connected to this vertex.)
    ///
    /// <para>
    /// A self-loop (an edge that connects a vertex to itself) is always
    /// considered an outgoing edge.  Therefore, if there is an edge that
    /// connects this vertex to itself, then this vertex is included in the
    /// returned array.
    /// </para>
    ///
    /// <para>
    /// The successor vertices in the returned array are unique.  If two or
    /// more outgoing edges connect this vertex with another vertex, the other
    /// vertex is included once only.
    /// </para>
    ///
    /// <para>
    /// If there are no successor vertices, the returned array is empty.  The
    /// returned value is never null.
    /// </para>
    ///
    /// <para>
    /// This property is an O(n) operation, where n is the number of edges
    /// incident to this vertex.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="PredecessorVertices" />
    /// <seealso cref="AdjacentVertices" />
    //*************************************************************************

    [ DebuggerBrowsable(DebuggerBrowsableState.Never) ]

    public IVertex []
    SuccessorVertices
    {
        get
        {
            AssertValid();

            return ( GetPredecessorOrSuccessorVertices(false, true) );
        }
    }

    //*************************************************************************
    //  Property: AdjacentVertices
    //
    /// <summary>
    /// Gets an array of the vertex's adjacent vertices.
    /// </summary>
    ///
    /// <value>
    /// An array of the vertex's zero or more adjacent vertices, as an array of
    /// <see cref="IVertex" /> objects.
    /// </value>
    ///
    /// <remarks>
    /// An adjacent vertex is a vertex at the other side of an incident edge.
    /// (An incident edge is an edge that is connected to the vertex.)
    ///
    /// <para>
    /// The returned collection is the union of the <see
    /// cref="PredecessorVertices" /> and <see cref="SuccessorVertices" />
    /// collections.
    /// </para>
    ///
    /// <para>
    /// A self-loop (an edge that connects a vertex to itself) is always
    /// considered an incident edge.  Therefore, if there is an edge that
    /// connects this vertex to itself, then this vertex is included in the
    /// returned array.
    /// </para>
    ///
    /// <para>
    /// The adjacent vertices in the returned array are unique.  If two or
    /// more edges connect this vertex with another vertex, the other vertex is
    /// included once only.
    /// </para>
    ///
    /// <para>
    /// If there are no adjacent vertices, the returned array is empty.  The
    /// returned value is never null.
    /// </para>
    ///
    /// <para>
    /// This property is an O(n) operation, where n is the number of edges
    /// incident to this vertex.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="PredecessorVertices" />
    /// <seealso cref="SuccessorVertices" />
    //*************************************************************************

    [ DebuggerBrowsable(DebuggerBrowsableState.Never) ]

    public IVertex []
    AdjacentVertices
    {
        get
        {
            AssertValid();

            return ( GetPredecessorOrSuccessorVertices(true, true) );
        }
    }

    //*************************************************************************
    //  Method: Clone()
    //
    /// <overloads>
    /// Creates a copy of the vertex.
    /// </overloads>
    ///
    /// <summary>
    /// Creates a copy of the vertex, making the copy the same type as the
    /// original.
    /// </summary>
    ///
    /// <param name="copyMetadataValues">
    /// If true, the key/value pairs that were set with <see
    /// cref="IMetadataProvider.SetValue" /> are copied to the new vertex.
    /// (This is a shallow copy.  The objects pointed to by the original values
    /// are NOT cloned.)  If false, the key/value pairs are not copied.
    /// </param>
    ///
    /// <param name="copyTag">
    /// If true, the <see cref="IMetadataProvider.Tag" /> property on the new
    /// vertex is set to the same value as in the original vertex.  (This is a
    /// shallow copy.  The object pointed to by the original <see
    /// cref="IMetadataProvider.Tag" /> is NOT cloned.)  If false, the <see
    /// cref="IMetadataProvider.Tag "/> property on the new vertex is set to
    /// null.
    /// </param>
    ///
    /// <returns>
    /// The copy of the vertex, as an <see cref="IVertex" />.
    /// </returns>
    ///
    /// <remarks>
    /// The new vertex is of the same type as the original.  It has no edges
    /// connected to it.  Its <see cref="IIdentityProvider.Name" /> is set to
    /// the same value as the original's, but it is assigned a new <see
    /// cref="IIdentityProvider.ID" />.  Its <see cref="ParentGraph" />
    /// is null and its <see cref="Location" /> is the default value of <see
    /// cref="Point.Empty" />.
    ///
    /// <para>
    /// The new vertex can be added to the same graph or to a different graph.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public IVertex
    Clone
    (
        Boolean copyMetadataValues,
        Boolean copyTag
    )
    {
        AssertValid();

        return ( Clone( copyMetadataValues, copyTag, new VertexFactory() ) );
    }

    //*************************************************************************
    //  Method: Clone()
    //
    /// <summary>
    /// Creates a copy of the vertex, making the copy a specified type.
    /// </summary>
    ///
    /// <param name="copyMetadataValues">
    /// If true, the key/value pairs that were set with <see
    /// cref="IMetadataProvider.SetValue" /> are copied to the new vertex.
    /// (This is a shallow copy.  The objects pointed to by the original values
    /// are NOT cloned.)  If false, the key/value pairs are not copied.
    /// </param>
    ///
    /// <param name="copyTag">
    /// If true, the <see cref="IMetadataProvider.Tag" /> property on the new
    /// vertex is set to the same value as in the original vertex.  (This is a
    /// shallow copy.  The object pointed to by the original <see
    /// cref="IMetadataProvider.Tag" /> is NOT cloned.)  If false, the <see
    /// cref="IMetadataProvider.Tag "/> property on the new vertex is set to
    /// null.
    /// </param>
    ///
    /// <param name="newVertexFactory">
    /// Object that can create a vertex.
    /// </param>
    ///
    /// <returns>
    /// The copy of the vertex, as an <see cref="IVertex" />.
    /// </returns>
    ///
    /// <remarks>
    /// The new vertex is created using <paramref name="newVertexFactory" />.
    /// It has no edges connected to it.  Its <see
    /// cref="IIdentityProvider.Name" /> is set to the same value as the
    /// original's, but it is assigned a new <see
    /// cref="IIdentityProvider.ID" />.  Its <see cref="ParentGraph" /> is null
    /// and its <see cref="Location" /> is the default value of <see
    /// cref="Point.Empty" />.
    ///
    /// <para>
    /// The new vertex can be added to the same graph or to a different graph.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public IVertex
    Clone
    (
        Boolean copyMetadataValues,
        Boolean copyTag,
        IVertexFactory newVertexFactory
    )
    {
        AssertValid();

        const String MethodName = "Clone";
        const String ArgumentName = "newVertexFactory";

        this.ArgumentChecker.CheckArgumentNotNull(
            MethodName, ArgumentName, newVertexFactory);

        IVertex oNewVertex = newVertexFactory.CreateVertex();

        // Copy the base-class fields to the new vertex.

        this.CopyTo(oNewVertex, copyMetadataValues, copyTag);

        return (oNewVertex);
    }

    //*************************************************************************
    //  Method: GetConnectingEdges()
    //
    /// <summary>
    /// Gets an array of edges that connect this vertex to a specified vertex.
    /// </summary>
    ///
    /// <param name="otherVertex">
    /// Other vertex.
    /// </param>
    ///
    /// <returns>
    /// An array of zero or more edges that connect this vertex to <paramref
    /// name="otherVertex" />, as an array of <see cref="IEdge" /> objects.
    /// </returns>
    ///
    /// <remarks>
    /// If there are no such edges, the returned array is empty.  The returned
    /// value is never null.
    ///
    /// <para>
    /// A self-loop (an edge that connects a vertex to itself) is returned in
    /// the array only if <paramref name="otherVertex" /> is this vertex.
    /// </para>
    ///
    /// <para>
    /// This method is an O(n) operation, where n is the number of edges
    /// incident to this vertex.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public IEdge []
    GetConnectingEdges
    (
        IVertex otherVertex
    )
    {
        AssertValid();

        const String MethodName = "GetConnectingEdges";
        const String ArgumentName = "otherVertex";

        this.ArgumentChecker.CheckArgumentNotNull(
            MethodName, ArgumentName, otherVertex);

        EdgeCollection oEdgeCollection;

        if ( !GetEdgeCollection(out oEdgeCollection) )
        {
            // The vertex does not belong to a graph.

            return ( new IEdge [0] );
        }

        return ( oEdgeCollection.GetConnectingEdges(this, otherVertex) );
    }

    //*************************************************************************
    //  Method: IsIncidentEdge()
    //
    /// <summary>
    /// Determines whether an edge is incident to the vertex.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge to test.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="edge" /> is incident to the vertex, false if
    /// not.
    /// </returns>
    ///
    /// <remarks>
    /// An incident edge is an edge that is connected to the vertex.
    ///
    /// <para>
    /// This method is an O(1) operation.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Boolean
    IsIncidentEdge
    (
        IEdge edge
    )
    {
        AssertValid();

        const String MethodName = "IsIncidentEdge";
        const String ArgumentName = "edge";

        this.ArgumentChecker.CheckArgumentNotNull(
            MethodName, ArgumentName, edge);

        IVertex oVertex1, oVertex2;

        EdgeUtil.EdgeToVertices(edge, this.ClassName, MethodName,
            out oVertex1, out oVertex2);

        return (oVertex1 == this || oVertex2 == this);
    }

    //*************************************************************************
    //  Method: IsOutgoingEdge()
    //
    /// <summary>
    /// Determines whether an edge is one of the vertex's outgoing edges.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge to test.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="edge" /> is one of the vertex's outgoing edges,
    /// false if not.
    /// </returns>
    ///
    /// <remarks>
    /// An outgoing edge is either a directed edge that has the vertex at its
    /// back, or an undirected edge connected to the vertex.
    ///
    /// <para>
    /// This method is an O(1) operation.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Boolean
    IsOutgoingEdge
    (
        IEdge edge
    )
    {
        AssertValid();

        const String MethodName = "IsOutgoingEdge";
        const String ArgumentName = "edge";

        this.ArgumentChecker.CheckArgumentNotNull(
            MethodName, ArgumentName, edge);

        if (edge.IsDirected)
        {
            return (edge.BackVertex == this);
        }

        return ( IsIncidentEdge(edge) );
    }

    //*************************************************************************
    //  Method: IsIncomingEdge()
    //
    /// <summary>
    /// Determines whether an edge is one of the vertex's incoming edges.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge to test.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="edge" /> is one of the vertex's incoming edges,
    /// false if not.
    /// </returns>
    ///
    /// <remarks>
    /// An incoming edge is either a directed edge that has the vertex at its
    /// front, or an undirected edge connected to the vertex.
    ///
    /// <para>
    /// This method is an O(1) operation.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Boolean
    IsIncomingEdge
    (
        IEdge edge
    )
    {
        AssertValid();

        const String MethodName = "IsIncomingEdge";
        const String ArgumentName = "edge";

        this.ArgumentChecker.CheckArgumentNotNull(
            MethodName, ArgumentName, edge);

        if (edge.IsDirected)
        {
            return (edge.FrontVertex == this);
        }

        return ( IsIncidentEdge(edge) );
    }

    //*************************************************************************
    //  Property: FirstIncidentEdge
    //
    /// <summary>
    /// Gets or sets the first node in the vertex's group of incident edges.
    /// </summary>
    ///
    /// <value>
    /// First node in the vertex's group of incident edges, or null if the
    /// vertex has no incident edges.
    /// </value>
    ///
    /// <remarks>
    /// This is used by <see cref="EdgeCollection" />, which maintains the
    /// incident edge groups for all vertices.
    /// </remarks>
    //*************************************************************************

    protected internal LinkedListNode<IEdge>
    FirstIncidentEdgeNode
    {
        get
        {
            AssertValid();

            return (m_oFirstIncidentEdgeNode);
        }

        set
        {
            m_oFirstIncidentEdgeNode = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: SetParentGraph()
    //
    /// <summary>
    /// Sets the graph that owns the vertex.
    /// </summary>
    ///
    /// <param name="oParentGraph">
    /// The graph that owns the vertex, as an <see cref="IGraph" />, or null if
    /// the vertex does not belong to a graph.
    /// </param>
    /// 
    /// <remarks>
    /// This is the implementation-specific way that the <see
    /// cref="ParentGraph" /> property gets set.  When the vertex is added to a
    /// graph, it must be set to that graph.  If the vertex is removed from a
    /// graph, it must be set to null.
    /// </remarks>
    //*************************************************************************

    protected internal void
    SetParentGraph
    (
        IGraph oParentGraph
    )
    {
        AssertValid();

        const String MethodName = "SetParentGraph";

        String sErrorMessage = null;

        if (oParentGraph == null)
        {
            // Remove all edges incident to this vertex.

            RemoveIncidentEdges();
        }
        else if (oParentGraph == m_oParentGraph)
        {
            sErrorMessage = 
                "The vertex has already been added to this graph."
                ;
        }
        else if (m_oParentGraph != null)
        {
            sErrorMessage = 

                "The vertex has already been added to a graph.  It must be"
                + " removed from the graph before it can be added to a"
                + " different graph."
                ;
        }

        if (sErrorMessage != null)
        {
            throw new ApplicationException( String.Format(
                "{0}.{1}: {2}"
                ,
                this.ClassName,
                MethodName,
                sErrorMessage
                ) );
        }

        m_oParentGraph = oParentGraph;

        AssertValid();
    }

    //*************************************************************************
    //  Method: IVertexToVertex
    //
    /// <summary>
    /// Casts an <see cref="IVertex" /> to a <see cref="Vertex" /> object.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The <see cref="IVertex" /> to cast to a <see cref="Vertex" /> object.
    /// </param>
    ///
    /// <param name="sClassName">
    /// Name of the class calling this method.
    /// </param>
    ///
    /// <param name="sMethodOrPropertyName">
    /// Name of the method or property calling this method.
    /// </param>
    ///
    /// <returns>
    /// The <see cref="Vertex" /> object.
    /// </returns>
    ///
    /// <remarks>
    /// An <see cref="ApplicationException" /> is thrown if <paramref
    /// name="oVertex" /> is null or not of type <see cref="Vertex" />.
    /// </remarks>
    //*************************************************************************

    protected internal static Vertex
    IVertexToVertex
    (
        IVertex oVertex,
        String sClassName,
        String sMethodOrPropertyName
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sClassName) );
        Debug.Assert( !String.IsNullOrEmpty(sMethodOrPropertyName) );

        if (oVertex == null)
        {
            throw new ApplicationException(String.Format(

                "{0}.{1}: A vertex is null."
                ,
                sClassName,
                sMethodOrPropertyName
                ) );
        }

        if ( !(oVertex is Vertex) )
        {
            throw new ApplicationException(String.Format(

                "{0}.{1}: A vertex is not of type Vertex.  The type is {2}."
                ,
                sClassName,
                sMethodOrPropertyName,
                oVertex.GetType().FullName
                ) );
        }

        return ( (Vertex)oVertex );
    }

    //*************************************************************************
    //  Method: GetIncomingOrOutgoingEdges()
    //
    /// <summary>
    /// Gets an array of the vertex's incoming or outgoing edges.
    /// </summary>
    ///
    /// <param name="bIncludeIncoming">
    /// true to include the vertex's incoming edges.
    /// </param>
    ///
    /// <param name="bIncludeOutgoing">
    /// true to include the vertex's outgoing edges.
    /// </param>
    ///
    /// <returns>
    /// An array of the vertex's zero or more incoming or outgoing edges (or
    /// both), as an array of <see cref="IEdge" /> objects.
    /// </returns>
    //*************************************************************************

    protected IEdge []
    GetIncomingOrOutgoingEdges
    (
        Boolean bIncludeIncoming,
        Boolean bIncludeOutgoing
    )
    {
        AssertValid();

        EdgeCollection oEdgeCollection;

        if ( !GetEdgeCollection(out oEdgeCollection) )
        {
            // The vertex does not belong to a graph.

            return ( new IEdge [0] );
        }

        return ( oEdgeCollection.GetIncomingOrOutgoingEdges(
            this, bIncludeIncoming, bIncludeOutgoing) );
    }

    //*************************************************************************
    //  Method: GetPredecessorOrSuccessorVertices()
    //
    /// <summary>
    /// Gets an array of the vertex's predecessor or successor vertices, or
    /// both.
    /// </summary>
    ///
    /// <param name="bIncludePredecessor">
    /// true to include the vertex's predecessor vertices.
    /// </param>
    ///
    /// <param name="bIncludeSuccessor">
    /// true to include the vertex's successor vertices.
    /// </param>
    ///
    /// <returns>
    /// An array of the vertex's zero or more predecessor or successor
    /// vertices, as an array of <see cref="IVertex" /> objects.
    /// </returns>
    //*************************************************************************

    protected IVertex []
    GetPredecessorOrSuccessorVertices
    (
        Boolean bIncludePredecessor,
        Boolean bIncludeSuccessor
    )
    {
        AssertValid();

        EdgeCollection oEdgeCollection;

        if ( !GetEdgeCollection(out oEdgeCollection) )
        {
            // The vertex does not belong to a graph.

            return ( new IVertex [0] );
        }

        return ( oEdgeCollection.GetPredecessorOrSuccessorVertices(
            this, bIncludePredecessor, bIncludeSuccessor) );
    }

    //*************************************************************************
    //  Method: RemoveIncidentEdges()
    //
    /// <summary>
    /// Removes all edges incident to this vertex.
    /// </summary>
    //*************************************************************************

    protected internal void
    RemoveIncidentEdges()
    {
        AssertValid();

        EdgeCollection oEdgeCollection;

        if ( !GetEdgeCollection(out oEdgeCollection) )
        {
            // The vertex does not belong to a graph.

            return;
        }

        oEdgeCollection.RemoveAllFromGroup(this);
    }

    //*************************************************************************
    //  Method: AppendPropertiesToString()
    //
    /// <summary>
    /// Appends the derived class's public property values to a String.
    /// </summary>
    ///
    /// <param name="oStringBuilder">
    /// Object to append to.
    /// </param>
    ///
    /// <param name="iIndentationLevel">
    /// Current indentation level.  Level 0 is "no indentation."
    /// </param>
    ///
    /// <param name="sFormat">
    /// The format to use, either "G", "P", or "D".  See <see
    /// cref="NodeXLBase.ToString()" /> for details.
    /// </param>
    ///
    /// <remarks>
    /// This method calls <see cref="ToStringUtil.AppendPropertyToString(
    /// StringBuilder, Int32, String, Object, Boolean)" /> for each of the
    /// derived class's public properties.  It is used in the implementation of
    /// <see cref="NodeXLBase.ToString()" />.
    /// </remarks>
    //*************************************************************************

    protected override void
    AppendPropertiesToString
    (
        StringBuilder oStringBuilder,
        Int32 iIndentationLevel,
        String sFormat
    )
    {
        AssertValid();
        Debug.Assert(oStringBuilder != null);
        Debug.Assert(iIndentationLevel >= 0);
        Debug.Assert( !String.IsNullOrEmpty(sFormat) );
        Debug.Assert(sFormat == "G" || sFormat == "P" || sFormat == "D");

        base.AppendPropertiesToString(
            oStringBuilder, iIndentationLevel, sFormat);

        if (sFormat == "G")
        {
            return;
        }

        ToStringUtil.AppendPropertyToString(oStringBuilder, iIndentationLevel,
            "AdjacentVertices", String.Empty, false);

        ToStringUtil.AppendVerticesToString(oStringBuilder, iIndentationLevel,
            sFormat, this.AdjacentVertices);

        ToStringUtil.AppendPropertyToString(oStringBuilder, iIndentationLevel,
            "Degree", this.Degree);

        ToStringUtil.AppendPropertyToString(oStringBuilder, iIndentationLevel,
            "IncidentEdges", String.Empty, false);

        ToStringUtil.AppendEdgesToString(oStringBuilder, iIndentationLevel,
            sFormat, this.IncidentEdges);

        ToStringUtil.AppendPropertyToString(oStringBuilder, iIndentationLevel,
            "IncomingEdges", String.Empty, false);

        ToStringUtil.AppendEdgesToString(oStringBuilder, iIndentationLevel,
            sFormat, this.IncomingEdges);

        ToStringUtil.AppendPropertyToString(oStringBuilder, iIndentationLevel,
            "Location", this.Location);

        ToStringUtil.AppendPropertyToString(oStringBuilder, iIndentationLevel,
            "OutgoingEdges", String.Empty, false);

        ToStringUtil.AppendEdgesToString(oStringBuilder, iIndentationLevel,
            sFormat, this.OutgoingEdges);

        ToStringUtil.AppendPropertyToString(oStringBuilder, iIndentationLevel,
            "ParentGraph", this.ParentGraph);

        ToStringUtil.AppendPropertyToString(oStringBuilder, iIndentationLevel,
            "PredecessorVertices", String.Empty, false);

        ToStringUtil.AppendVerticesToString(oStringBuilder, iIndentationLevel,
            sFormat, this.PredecessorVertices);

        ToStringUtil.AppendPropertyToString(oStringBuilder, iIndentationLevel,
            "SuccessorVertices", String.Empty, false);

        ToStringUtil.AppendVerticesToString(oStringBuilder, iIndentationLevel,
            sFormat, this.SuccessorVertices);
    }

    //*************************************************************************
    //  Method: GetEdgeCollection()
    //
    /// <summary>
    /// Gets the <see cref="EdgeCollection" /> owned by the parent graph.
    /// </summary>
    ///
    /// <param name="oEdgeCollection">
    /// Where the <see cref="EdgeCollection" /> gets stored if true is
    /// returned.
    /// </param>
    ///
    /// <returns>
    /// true if the <see cref="EdgeCollection" /> was stored at <paramref
    /// name="oEdgeCollection" />, false if the vertex does not belong to a
    /// graph.
    /// </returns>
    //*************************************************************************

    protected Boolean
    GetEdgeCollection
    (
        out EdgeCollection oEdgeCollection
    )
    {
        AssertValid();

        const String MethodName = "GetEdgeCollection";

        oEdgeCollection = null;

        if (m_oParentGraph == null)
        {
            // The vertex does not belong to a graph.

            return (false);
        }

        if ( !(m_oParentGraph.Edges is EdgeCollection) )
        {
            Debug.Assert(false);

            throw new ApplicationException( String.Format(

                "{0}.{1}: The Edges property of the graph that owns this"
                + " vertex is not of type EdgeCollection."
                ,
                this.ClassName,
                MethodName
                ) );
        }

        oEdgeCollection = (EdgeCollection)(m_oParentGraph.Edges);

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

        // m_oParentGraph
        // m_oFirstIncidentEdgeNode
        // m_oLocation
    }


    //*************************************************************************
    //  Private fields
    //*************************************************************************

    /// Generates unique IDs.

    private static IDGenerator m_oIDGenerator;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Graph to which this vertex has been added, or null if does not belong
    /// to a graph.

    protected IGraph m_oParentGraph;

    /// First node in the vertex's group of incident edges, or null if the
    /// vertex has no incident edges.  The edges are maintained by
    /// EdgeCollection.

    protected LinkedListNode<IEdge> m_oFirstIncidentEdgeNode;

    /// The vertex's location, or Point.Empty if Location hasn't been set yet.

    protected PointF m_oLocation;
}
}
