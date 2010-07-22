
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.NodeXL.Core
{
//*****************************************************************************
//  Interface: IVertex
//
/// <summary>
/// Represents a vertex.
/// </summary>
///
/// <remarks>
/// A vertex, also known as a node, is a point in a graph that can be connected
/// to other vertices in the same graph.  The connections are called edges.
///
/// <para>
/// A vertex can be created via the vertex constructor and then added to a
/// graph via IGraph.Vertices.<see cref="VertexCollection.Add(IVertex)" />, or
/// created and added to a graph at the same time via IGraph.Vertices.<see
/// cref="IVertexCollection.Add()" />.
/// </para>
///
/// <para>
/// A vertex can be added to one graph only.  It cannot be added to a second
/// graph unless it is first removed from the first graph.
/// </para>
///
/// </remarks>
///
/// <seealso cref="Vertex" />
//*****************************************************************************

public interface IVertex : IIdentityProvider, IMetadataProvider
{
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
    /// This is a read-only property.  The implementation determines how this
    /// property gets set.  When the vertex is added to a graph, it must be set
    /// to that graph.  If the vertex is removed from a graph, it must be set
    /// to null.
    /// </remarks>
    //*************************************************************************

    IGraph
    ParentGraph
    {
        get;
    }

    //*************************************************************************
    //  Property: IncomingEdges
    //
    /// <summary>
    /// Gets a collection of the vertex's incoming edges.
    /// </summary>
    ///
    /// <value>
    /// A collection of the vertex's zero or more incoming edges, as a
    /// collection of <see cref="IEdge" /> objects.
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
    /// If there are no incoming edges, the returned collection is empty.  The
    /// returned value is never null.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="OutgoingEdges" />
    /// <seealso cref="IncidentEdges" />
    //*************************************************************************

    [ DebuggerBrowsable(DebuggerBrowsableState.Never) ]

    ICollection<IEdge>
    IncomingEdges
    {
        get;
    }

    //*************************************************************************
    //  Property: OutgoingEdges
    //
    /// <summary>
    /// Gets a collection of the vertex's outgoing edges.
    /// </summary>
    ///
    /// <value>
    /// A collection of the vertex's zero or more outgoing edges, as a
    /// collection of <see cref="IEdge" /> objects.
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
    /// If there are no outgoing edges, the returned collection is empty.  The
    /// returned value is never null.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="IncomingEdges" />
    /// <seealso cref="IncidentEdges" />
    //*************************************************************************

    [ DebuggerBrowsable(DebuggerBrowsableState.Never) ]

    ICollection<IEdge>
    OutgoingEdges
    {
        get;
    }

    //*************************************************************************
    //  Property: IncidentEdges
    //
    /// <summary>
    /// Gets a collection of the vertex's incident edges.
    /// </summary>
    ///
    /// <value>
    /// A collection of the vertex's zero or more incident edges, as a
    /// collection of <see cref="IEdge" /> objects.
    /// </value>
    ///
    /// <remarks>
    /// An incident edge is an edge that is connected to the vertex.
    ///
    /// <para>
    /// The returned collection is the union of the <see
    /// cref="IncomingEdges" /> and <see cref="OutgoingEdges" /> collections.
    /// </para>
    ///
    /// <para>
    /// A self-loop (an edge that connects a vertex to itself) is considered
    /// one incident edge.
    /// </para>
    ///
    /// <para>
    /// If there are no incident edges, the returned collection is empty.  The
    /// returned value is never null.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="IncomingEdges" />
    /// <seealso cref="IncidentEdges" />
    //*************************************************************************

    [ DebuggerBrowsable(DebuggerBrowsableState.Never) ]

    ICollection<IEdge>
    IncidentEdges
    {
        get;
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
    /// </remarks>
    ///
    /// <seealso cref="IncomingEdges" />
    /// <seealso cref="OutgoingEdges" />
    /// <seealso cref="IncidentEdges" />
    //*************************************************************************

    Int32
    Degree
    {
        get;
    }

    //*************************************************************************
    //  Property: PredecessorVertices
    //
    /// <summary>
    /// Gets a collection of the vertex's predecessor vertices.
    /// </summary>
    ///
    /// <value>
    /// A collection of the vertex's zero or more predecessor vertices, as a
    /// collection of <see cref="IVertex" /> objects.
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
    /// returned collection.
    /// </para>
    ///
    /// <para>
    /// The predecessor vertices in the returned collection are unique.  If two
    /// or more edges connect this vertex with another vertex, the other vertex
    /// is included once only.
    /// </para>
    ///
    /// <para>
    /// If there are no predecessor vertices, the returned collection is empty.
    /// The returned value is never null.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="SuccessorVertices" />
    /// <seealso cref="AdjacentVertices" />
    //*************************************************************************

    [ DebuggerBrowsable(DebuggerBrowsableState.Never) ]

    ICollection<IVertex>
    PredecessorVertices
    {
        get;
    }

    //*************************************************************************
    //  Property: SuccessorVertices
    //
    /// <summary>
    /// Gets a collection of the vertex's successor vertices.
    /// </summary>
    ///
    /// <value>
    /// A collection of the vertex's zero or more successor vertices, as a
    /// collection of <see cref="IVertex" /> objects.
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
    /// returned collection.
    /// </para>
    ///
    /// <para>
    /// The successor vertices in the returned collection are unique.  If two
    /// or more edges connect this vertex with another vertex, the other vertex
    /// is included once only.
    /// </para>
    ///
    /// <para>
    /// If there are no successor vertices, the returned collection is empty.
    /// The returned value is never null.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="PredecessorVertices" />
    /// <seealso cref="AdjacentVertices" />
    //*************************************************************************

    [ DebuggerBrowsable(DebuggerBrowsableState.Never) ]

    ICollection<IVertex>
    SuccessorVertices
    {
        get;
    }

    //*************************************************************************
    //  Property: AdjacentVertices
    //
    /// <summary>
    /// Gets a collection of the vertex's adjacent vertices.
    /// </summary>
    ///
    /// <value>
    /// A collection of the vertex's zero or more adjacent vertices, as a
    /// collection of <see cref="IVertex" /> objects.
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
    /// returned collection.
    /// </para>
    ///
    /// <para>
    /// The adjacent vertices in the returned collection are unique.  If two or
    /// more edges connect this vertex with another vertex, the other vertex is
    /// included once only.
    /// </para>
    ///
    /// <para>
    /// If there are no adjacent vertices, the returned collection is empty.
    /// The returned value is never null.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="PredecessorVertices" />
    /// <seealso cref="SuccessorVertices" />
    //*************************************************************************

    [ DebuggerBrowsable(DebuggerBrowsableState.Never) ]

    ICollection<IVertex>
    AdjacentVertices
    {
        get;
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
    /// <see cref="PointF.Empty" />.
    /// </value>
    ///
    /// <remarks>
    /// Typically, this property is set when the graph is laid out by
    /// ILayout.LayOutGraph and is read when the graph is drawn.  It's also
    /// possible to explicitly set all of the graph's vertex locations using
    /// this property and then bypass the layout stage.  You might do this
    /// when you want to restore vertex locations that have been saved from a
    /// previous layout, for example.
    /// </remarks>
    //*************************************************************************

    PointF
    Location
    {
        get;
        set;
    }

    //*************************************************************************
    //  Method: Clone()
    //
    /// <summary>
    /// Creates a copy of the vertex.
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
    /// The new vertex has no edges connected to it.  Its <see
    /// cref="IIdentityProvider.Name" /> is set to the same value as the
    /// original's, but it is assigned a new <see
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

    IVertex
    Clone
    (
        Boolean copyMetadataValues,
        Boolean copyTag
    );

    //*************************************************************************
    //  Method: GetConnectingEdges()
    //
    /// <summary>
    /// Gets a collection of edges that connect this vertex to a specified
    /// vertex.
    /// </summary>
    ///
    /// <param name="otherVertex">
    /// Other vertex.
    /// </param>
    ///
    /// <returns>
    /// A collection of zero or more edges that connect this vertex to
    /// <paramref name="otherVertex" />, as a collection of <see
    /// cref="IEdge" /> objects.
    /// </returns>
    ///
    /// <remarks>
    /// If there are no such edges, the returned collection is empty.  The
    /// returned value is never null.
    ///
    /// <para>
    /// A self-loop (an edge that connects a vertex to itself) is returned in
    /// the collection only if <paramref name="otherVertex" /> is this vertex.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    ICollection<IEdge>
    GetConnectingEdges
    (
        IVertex otherVertex
    );

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

    Boolean
    IsIncidentEdge
    (
        IEdge edge
    );

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

    Boolean
    IsOutgoingEdge
    (
        IEdge edge
    );

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

    Boolean
    IsIncomingEdge
    (
        IEdge edge
    );
}

}
