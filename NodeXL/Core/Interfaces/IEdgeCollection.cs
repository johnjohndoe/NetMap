
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.NodeXL.Core
{
//*****************************************************************************
//  Interface: IEdgeCollection
//
/// <summary>
/// Represents a collection of edges.
/// </summary>
///
/// <remarks>
/// This is a collection of objects that implement the <see cref="IEdge" />
/// interface.  You can add edges to the collection, remove them, access an
/// edge, and enumerate all edges.
/// </remarks>
///
/// <seealso cref="EdgeCollection" />
/// <seealso cref="IEdge" />
//*****************************************************************************

public interface IEdgeCollection : ICollection<IEdge>
{
    //*************************************************************************
    //  Method: Add()
    //
    /// <summary>
    /// Creates an edge and adds it to the collection.
    /// </summary>
    ///
    /// <param name="vertex1">
    /// The edge's first vertex.  The vertex must be contained in the graph
    /// that owns this edge collection.
    /// </param>
    ///
    /// <param name="vertex2">
    /// The edge's second vertex.  The vertex must be contained in the graph
    /// that owns this edge collection.
    /// </param>
    ///
    /// <param name="isDirected">
    /// If true, <paramref name="vertex1" /> is the edge's back vertex and
    /// <paramref name="vertex2" /> is the edge's front vertex.  If false, the
    /// edge is undirected.
    /// </param>
    ///
    /// <returns>
    /// The new edge, as an <see cref="IEdge" />.
    /// </returns>
    ///
    /// <remarks>
    /// This method creates an edge, connects it to the specified vertices, and
    /// adds the edge to the collection.
    ///
    /// <para>
    /// An exception is thrown if <paramref name="isDirected" /> is
    /// incompatible with the <see cref="IGraph.Directedness" /> property on
    /// the graph that owns this edge collection.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    IEdge
    Add
    (
        IVertex vertex1,
        IVertex vertex2,
        Boolean isDirected
    );

    //*************************************************************************
    //  Method: Add()
    //
    /// <summary>
    /// Creates an undirected edge and adds it to the collection.
    /// </summary>
    ///
    /// <param name="vertex1">
    /// The edge's first vertex.  The vertex must be contained in the graph
    /// that owns this edge collection.
    /// </param>
    ///
    /// <param name="vertex2">
    /// The edge's second vertex.  The vertex must be contained in the graph
    /// that owns this edge collection.
    /// </param>
    ///
    /// <returns>
    /// The new undirected edge, as an <see cref="IEdge" />.
    /// </returns>
    ///
    /// <remarks>
    /// This method creates an undirected edge, connects it to the specified
    /// vertices, and adds the edge to the collection.
    ///
    /// <para>
    /// An exception is thrown if the graph that owns this edge collection has
    /// a <see cref="IGraph.Directedness" /> value of <see
    /// cref="GraphDirectedness.Directed" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    IEdge
    Add
    (
        IVertex vertex1,
        IVertex vertex2
    );

    //*************************************************************************
    //  Method: Contains()
    //
    /// <summary>
    /// Determines whether the collection contains an edge specified by <see
    /// cref="IIdentityProvider.ID" />
    /// </summary>
    ///
    /// <param name="id">
    /// The ID to search for.
    /// </param>
    ///
    /// <returns>
    /// true if the collection contains an edge with the <see
    /// cref="IIdentityProvider.ID" /> <paramref name="id" />.
    /// </returns>
    ///
    /// <remarks>
    /// IDs are unique among all edges, so there can be only one edge with the
    /// specified ID.
    /// </remarks>
    //*************************************************************************

    Boolean
    Contains
    (
        Int32 id
    );

    //*************************************************************************
    //  Method: Contains()
    //
    /// <summary>
    /// Determines whether the collection contains an edge specified by <see
    /// cref="IIdentityProvider.Name" />
    /// </summary>
    ///
    /// <param name="name">
    /// The name to search for.  Can't be null or empty.
    /// </param>
    ///
    /// <returns>
    /// true if the collection contains an edge with the <see
    /// cref="IIdentityProvider.Name" /> <paramref name="name" />.
    /// </returns>
    ///
    /// <remarks>
    /// Names do not have to be unique, so there could be more than one edge
    /// with the same name.
    /// </remarks>
    //*************************************************************************

    Boolean
    Contains
    (
        String name
    );

    //*************************************************************************
    //  Method: Find()
    //
    /// <overloads>
    /// Searches for a specified edge.
    /// </overloads>
    ///
    /// <summary>
    /// Searches for an edge with the specified <see
    /// cref="IIdentityProvider.ID" />.
    /// </summary>
    ///
    /// <param name="id">
    /// The <see cref="IIdentityProvider.ID" /> of the edge to search for.
    /// </param>
    ///
    /// <param name="edge">
    /// Gets set to the specified <see cref="IEdge" /> if true is returned,
    /// or to null if false is returned.
    /// </param>
    ///
    /// <returns>
    /// true if an edge with the <see cref="IIdentityProvider.ID" /> <paramref
    /// name="id" /> is found, false if not.
    /// </returns>
    ///
    /// <remarks>
    /// This method searches the collection for an edge with the <see
    /// cref="IIdentityProvider.ID" /> <paramref name="id" />.  If such an
    /// edge is found, it gets stored at <paramref name="edge" /> and true is
    /// returned.  Otherwise, <paramref name="edge" /> gets set to null and
    /// false is returned.
    ///
    /// <para>
    /// IDs are unique among all edges, so there can be only one edge with the
    /// specified ID.
    /// </para>
    ///
    /// <para>
    /// Use <see cref="Contains(Int32)" /> if you want to determine whether
    /// such an edge exists in the collection but you don't need the actual
    /// edge.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    Boolean
    Find
    (
        Int32 id,
        out IEdge edge
    );

    //*************************************************************************
    //  Method: Find()
    //
    /// <summary>
    /// Searches for the first edge with the specified <see
    /// cref="IIdentityProvider.Name" />.
    /// </summary>
    ///
    /// <param name="name">
    /// The <see cref="IIdentityProvider.Name" /> of the edge to search for.
    /// Can't be null or empty.
    /// </param>
    ///
    /// <param name="edge">
    /// Gets set to the specified <see cref="IEdge" /> if true is returned, or
    /// to null if false is returned.
    /// </param>
    ///
    /// <returns>
    /// true if an edge with the <see cref="IIdentityProvider.Name" />
    /// <paramref name="name" /> is found, false if not.
    /// </returns>
    ///
    /// <remarks>
    /// This method searches the collection for the first edge with the <see
    /// cref="IIdentityProvider.Name" /> <paramref name="name" />.  If such
    /// an edge is found, it gets stored at <paramref name="edge" /> and true
    /// is returned.  Otherwise, <paramref name="edge" /> gets set to null and
    /// false is returned.
    ///
    /// <para>
    /// Names do not have to be unique, so there could be more than one edge
    /// with the same name.
    /// </para>
    ///
    /// <para>
    /// Use <see cref="Contains(String)" /> if you want to determine whether
    /// such an edge exists in the collection but you don't need the actual
    /// edge.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    Boolean
    Find
    (
        String name,
        out IEdge edge
    );

    //*************************************************************************
    //  Method: GetConnectingEdges()
    //
    /// <summary>
    /// Gets a collection of edges that connect two specified vertices.
    /// </summary>
    ///
    /// <param name="vertex1">
    /// First vertex.  Must belong to the parent graph.
    /// </param>
    ///
    /// <param name="vertex2">
    /// Second vertex.  Must belong to the parent graph.
    /// </param>
    ///
    /// <returns>
    /// A collection of zero or more edges that connect <paramref
    /// name="vertex1" /> to <paramref name="vertex2" />, as a collection of
    /// <see cref="IEdge" /> objects.
    /// </returns>
    ///
    /// <remarks>
    /// This method returns a collection of all edges that connect <paramref
    /// name="vertex1" /> to <paramref name="vertex2" />.  The directedness of
    /// the edges is not considered.
    ///
    /// <para>
    /// If there are no such edges, the returned collection is empty.  The
    /// returned value is never null.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    ICollection<IEdge>
    GetConnectingEdges
    (
        IVertex vertex1,
        IVertex vertex2
    );

    //*************************************************************************
    //  Method: Remove()
    //
    /// <summary>
    /// Removes an edge specified by <see cref="IIdentityProvider.ID" /> from
    /// the collection.
    /// </summary>
    ///
    /// <param name="id">
    /// The ID of the edge to remove.
    /// </param>
    ///
    /// <returns>
    /// true if the edge was removed, false if the edge wasn't found in the
    /// collection.
    /// </returns>
    ///
    /// <remarks>
    /// This method searches the collection for an edge with the <see
    /// cref="IIdentityProvider.ID" /> <paramref name="id" />.  If found, it
    /// is removed from the collection and true is returned.  false is returned
    /// otherwise.
    ///
    /// <para>
    /// The edge is unusable once it is removed from the collection.
    /// Attempting to access the edge's properties or methods will lead to
    /// unpredictable results.
    /// </para>
    ///
    /// <para>
    /// IDs are unique among all edges, so there can be only one vertex with
    /// the specified ID.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    Boolean
    Remove
    (
        Int32 id
    );

    //*************************************************************************
    //  Method: Remove()
    //
    /// <summary>
    /// Removes an edge specified by <see cref="IIdentityProvider.Name" />
    /// from the collection.
    /// </summary>
    ///
    /// <param name="name">
    /// The name of the edge to remove.  Can't be null or empty.
    /// </param>
    ///
    /// <returns>
    /// true if the edge was removed, false if the edge wasn't found in the
    /// collection.
    /// </returns>
    ///
    /// <remarks>
    /// This method searches the collection for the first edge with the <see
    /// cref="IIdentityProvider.Name" /> <paramref name="name" />.  If
    /// found, it is removed from the collection and true is returned.  false
    /// is returned otherwise.
    ///
    /// <para>
    /// The edge is unusable once it is removed from the collection.
    /// Attempting to access the edge's properties or methods will lead to
    /// unpredictable results.
    /// </para>
    ///
    /// <para>
    /// Names do not have to be unique, so there could be more than one edge
    /// with the same name.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    Boolean
    Remove
    (
        String name
    );


    //*************************************************************************
    //  Event: EdgeAdded
    //
    /// <summary>
    /// Occurs when an edge is added to the collection.
    /// </summary>
    //*************************************************************************

    event EdgeEventHandler
    EdgeAdded;


    //*************************************************************************
    //  Event: EdgeRemoved
    //
    /// <summary>
    /// Occurs when an edge is removed from the collection.
    /// </summary>
    //*************************************************************************

    event EdgeEventHandler
    EdgeRemoved;
}

}
