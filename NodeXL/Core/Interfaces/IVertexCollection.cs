
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.NodeXL.Core
{
//*****************************************************************************
//  Interface: IVertexCollection
//
/// <summary>
/// Represents a collection of vertices.
/// </summary>
///
/// <remarks>
/// This is a collection of objects that implement the <see cref="IVertex" />
/// interface.  You can add vertices to the collection, remove them, access
/// a vertex, and enumerate all vertices.
/// </remarks>
///
/// <seealso cref="VertexCollection" />
/// <seealso cref="IVertex" />
//*****************************************************************************

public interface IVertexCollection : ICollection<IVertex>
{
    //*************************************************************************
    //  Method: Add()
    //
    /// <summary>
    /// Creates a vertex and adds it to the collection.
    /// </summary>
    ///
    /// <returns>
    /// The added vertex.
    /// </returns>
    //*************************************************************************

    IVertex
    Add();

    //*************************************************************************
    //  Method: Contains()
    //
    /// <summary>
    /// Determines whether the collection contains a vertex specified by <see
    /// cref="IIdentityProvider.ID" />
    /// </summary>
    ///
    /// <param name="id">
    /// The ID to search for.
    /// </param>
    ///
    /// <returns>
    /// true if the collection contains a vertex with the <see
    /// cref="IIdentityProvider.ID" /> <paramref name="id" />.
    /// </returns>
    ///
    /// <remarks>
    /// IDs are unique among all vertices, so there can be only one vertex with
    /// the specified ID.
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
    /// Determines whether the collection contains a vertex specified by <see
    /// cref="IIdentityProvider.Name" />
    /// </summary>
    ///
    /// <param name="name">
    /// The name to search for.  Can't be null or empty.
    /// </param>
    ///
    /// <returns>
    /// true if the collection contains a vertex with the <see
    /// cref="IIdentityProvider.Name" /> <paramref name="name" />.
    /// </returns>
    ///
    /// <remarks>
    /// Names do not have to be unique, so there could be more than one vertex
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
    /// Searches for a specified vertex.
    /// </overloads>
    ///
    /// <summary>
    /// Searches for the vertex with the specified <see
    /// cref="IIdentityProvider.ID" />.
    /// </summary>
    ///
    /// <param name="id">
    /// <see cref="IIdentityProvider.ID" /> of the vertex to search for.
    /// </param>
    ///
    /// <param name="vertex">
    /// Gets set to the specified <see cref="IVertex" /> if true is returned,
    /// or to null if false is returned.
    /// </param>
    ///
    /// <returns>
    /// true if a vertex with an <see cref="IIdentityProvider.ID" /> of
    /// <paramref name="id" /> is found, false if not.
    /// </returns>
    ///
    /// <remarks>
    /// This method searches the collection for the vertex with the <see
    /// cref="IIdentityProvider.ID" /> <paramref name="id" />.  If such a
    /// vertex is found, it gets stored at <paramref name="vertex" /> and true
    /// is returned.  Otherwise, <paramref name="vertex" /> gets set to null
    /// and false is returned.
    ///
    /// <para>
    /// IDs are unique among all vertices, so there can be only one vertex
    /// with the specified ID.
    /// </para>
    ///
    /// <para>
    /// Use <see cref="Contains(Int32)" /> if you want to determine whether
    /// such a vertex exists in the collection but you don't need the actual
    /// vertex.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    Boolean
    Find
    (
        Int32 id,
        out IVertex vertex
    );

    //*************************************************************************
    //  Method: Find()
    //
    /// <summary>
    /// Searches for the first vertex with the specified <see
    /// cref="IIdentityProvider.Name" />.
    /// </summary>
    ///
    /// <param name="name">
    /// The <see cref="IIdentityProvider.Name" /> of the vertex to search for.
    /// Can't be null or empty.
    /// </param>
    ///
    /// <param name="vertex">
    /// Gets set to the specified <see cref="IVertex" /> if true is returned,
    /// or to null if false is returned.
    /// </param>
    ///
    /// <returns>
    /// true if a vertex with a <see cref="IIdentityProvider.Name" /> of
    /// <paramref name="name" /> is found, false if not.
    /// </returns>
    ///
    /// <remarks>
    /// This method searches the collection for the first vertex with the <see
    /// cref="IIdentityProvider.Name" /> <paramref name="name" />.  If such
    /// a vertex is found, it gets stored at <paramref name="vertex" /> and
    /// true is returned.  Otherwise, <paramref name="vertex" /> gets set to
    /// null and false is returned.
    ///
    /// <para>
    /// Names do not have to be unique, so there could be more than one vertex
    /// with the same name.
    /// </para>
    ///
    /// <para>
    /// Use <see cref="Contains(String)" /> if you want to determine whether
    /// such a vertex exists in the collection but you don't need the actual
    /// vertex.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    Boolean
    Find
    (
        String name,
        out IVertex vertex
    );

    //*************************************************************************
    //  Method: Remove()
    //
    /// <summary>
    /// Removes a vertex specified by <see cref="IIdentityProvider.ID" /> from
    /// the collection.
    /// </summary>
    ///
    /// <param name="id">
    /// The ID of the vertex to remove.
    /// </param>
    ///
    /// <returns>
    /// true if the vertex was removed, false if the vertex wasn't found in the
    /// collection.
    /// </returns>
    ///
    /// <remarks>
    /// This method searches the collection for the vertex with the <see
    /// cref="IIdentityProvider.ID" /> <paramref name="id" />.  If found, it
    /// is removed from the collection, any edges connected to it are removed
    /// from the graph that owns this vertex collection, and true is returned.
    /// false is returned otherwise.
    ///
    /// <para>
    /// IDs are unique among all vertices, so there can be only one vertex
    /// with the specified ID.
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
    /// Removes a vertex specified by <see cref="IIdentityProvider.Name" />
    /// from the collection.
    /// </summary>
    ///
    /// <param name="name">
    /// Name of the vertex to remove.  Can't be null or empty.
    /// </param>
    ///
    /// <returns>
    /// true if the vertex was removed, false if the vertex wasn't found in the
    /// collection.
    /// </returns>
    ///
    /// <remarks>
    /// This method searches the collection for the first vertex with the <see
    /// cref="IIdentityProvider.Name" /> <paramref name="name" />.  If
    /// found, it is removed from the collection, any edges connected to it are
    /// removed from the graph that owns this vertex collection, and true is
    /// returned.  false is returned otherwise.
    ///
    /// <para>
    /// Names do not have to be unique, so there could be more than one vertex
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
    //  Method: GetReverseEnumerable()
    //
    /// <summary>
    /// Returns an IEnumerable that can be used to iterate backwards through
    /// the collection. 
    /// </summary>
    ///
    /// <returns>
    /// An IEnumerable that can be used to iterate backwards through the
    /// collection. 
    /// </returns>
    //*************************************************************************

    IEnumerable<IVertex>
    GetReverseEnumerable();


    //*************************************************************************
    //  Event: VertexAdded
    //
    /// <summary>
    /// Occurs when a vertex is added to the collection.
    /// </summary>
    //*************************************************************************

    event VertexEventHandler
    VertexAdded;


    //*************************************************************************
    //  Event: VertexRemoved
    //
    /// <summary>
    /// Occurs when a vertex is removed from the collection.
    /// </summary>
    //*************************************************************************

    event VertexEventHandler
    VertexRemoved;
}

}
