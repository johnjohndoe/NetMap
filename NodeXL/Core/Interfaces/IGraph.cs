
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.Core
{
//*****************************************************************************
//  Enum: GraphDirectedness
//
/// <summary>
/// Specifies the type of edges a graph can contain.
/// </summary>
//*****************************************************************************

public enum
GraphDirectedness
{
    /// <summary>
    /// Only directed edges can be added to the graph.  A directed edge has
    /// an <see cref="IEdge.IsDirected" /> value of true.
    /// </summary>

    Directed = 0,

    /// <summary>
    /// Only undirected edges can be added to the graph.  An undirected edge
    /// has an <see cref="IEdge.IsDirected" /> value of false.
    /// </summary>

    Undirected = 1,

    /// <summary>
    /// Both directed and undirected edges can be added to the graph.
    /// </summary>

    Mixed = 2,
}

//*****************************************************************************
//  Interface: IGraph
//
/// <summary>
/// Represents a graph.
/// </summary>
///
/// <remarks>
/// A graph has a collection of <see cref="Vertices" /> and a collection of
/// <see cref="Edges" /> that connect the <see cref="Vertices" />.  The <see
/// cref="Directedness" /> property specifies the type of edges that can be
/// added to the graph.
/// </remarks>
///
/// <seealso cref="Graph" />
//*****************************************************************************

public interface IGraph : IIdentityProvider, IMetadataProvider
{
    //*************************************************************************
    //  Property: Vertices
    //
    /// <summary>
    /// Gets the graph's collection of vertices.
    /// </summary>
    ///
    /// <value>
    /// A collection of vertices, as an <see cref="IVertexCollection" />.  The
    /// collection contains zero or more objects that implement <see
    /// cref="IVertex" />.
    /// </value>
    //*************************************************************************

    IVertexCollection
    Vertices
    {
        get;
    }

    //*************************************************************************
    //  Property: Edges
    //
    /// <summary>
    /// Gets the graph's collection of edges.
    /// </summary>
    ///
    /// <value>
    /// A collection of edges, as an <see cref="IEdgeCollection" />.  The
    /// collection contains zero or more objects that implement <see
    /// cref="IEdge" /> and that connect vertices in this graph.
    /// </value>
    //*************************************************************************

    IEdgeCollection
    Edges
    {
        get;
    }

    //*************************************************************************
    //  Property: Directedness
    //
    /// <summary>
    /// Gets a value that indicates the type of edges that can be added to the
    /// graph.
    /// </summary>
    ///
    /// <value>
    /// A <see cref="GraphDirectedness" /> value.
    /// </value>
    ///
    /// <remarks>
    /// The directedness of a graph is specified when the graph is created and
    /// cannot be changed.
    /// </remarks>
    //*************************************************************************

    GraphDirectedness
    Directedness
    {
        get;
    }

    //*************************************************************************
    //  Method: Clone()
    //
    /// <summary>
    /// Creates a copy of the graph.
    /// </summary>
    ///
    /// <param name="copyMetadataValues">
    /// If true, the key/value pairs that were set with <see
    /// cref="IMetadataProvider.SetValue" /> are copied to the new graph,
    /// vertices, and edges.  (This is a shallow copy.  The objects pointed to
    /// by the original values are NOT cloned.)  If false, the key/value pairs
    /// are not copied.
    /// </param>
    ///
    /// <param name="copyTag">
    /// If true, the <see cref="IMetadataProvider.Tag" /> properties on the new
    /// graph, vertices, and edges are set to the same value as in the original
    /// objects.  (This is a shallow copy.  The objects pointed to by the
    /// original <see cref="IMetadataProvider.Tag" /> properties are NOT
    /// cloned.)  If false, the <see cref="IMetadataProvider.Tag "/>
    /// properties on the new graph, vertices, and edges are set to null.
    /// </param>
    ///
    /// <returns>
    /// The copy of the graph, as an <see cref="IGraph" />.
    /// </returns>
    ///
    /// <remarks>
    /// The new graph, vertices, and edges have the same <see
    /// cref="IIdentityProvider.Name" /> values as the originals, but they are
    /// assigned new <see cref="IIdentityProvider.ID" />s.
    /// </remarks>
    //*************************************************************************

    IGraph
    Clone
    (
        Boolean copyMetadataValues,
        Boolean copyTag
    );
}

}
