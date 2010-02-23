
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
//  Enum: GraphRestrictions
//
/// <summary>
/// Specifies restrictions imposed by a graph.
/// </summary>
///
/// <remarks>
/// These are bit fields that can be ORed together.
/// </remarks>
//*****************************************************************************

[System.FlagsAttribute]

public enum
GraphRestrictions
{
    /// <summary>
    /// The graph does not impose restrictions.
    /// </summary>

    None = 0,

    /// <summary>
    /// Self-loops cannot be added to the graph.  A self-loop is an edge that
    /// connects a vertex to itself.
    /// </summary>

    NoSelfLoops = 1,

    /// <summary>
    /// Parallel edges cannot be added to the graph.  Parallel edges are
    /// defined at <see cref="IEdge.IsParallelTo" />.
    ///
    /// <para>
    /// Specifiying this restriction might slow down the addition of edges to
    /// a graph, since parallel edges need to be searched for with each edge
    /// addition.
    /// </para>
    ///
    /// </summary>

    NoParallelEdges = 2,

    /// <summary>
    /// All of the above restrictions are applied.
    /// </summary>

    All = NoSelfLoops | NoParallelEdges,
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
    //  Property: Restrictions
    //
    /// <summary>
    /// Gets an ORed set of flags that specify restrictions imposed by the
    /// graph.
    /// </summary>
    ///
    /// <value>
    /// An ORed combination of <see cref="GraphRestrictions" /> flags.
    /// </value>
    ///
    /// <remarks>
    /// The graph's restrictions are specified when the graph is created and
    /// cannot be changed.
    /// </remarks>
    //*************************************************************************

    GraphRestrictions
    Restrictions
    {
        get;
    }

    //*************************************************************************
    //  Property: PerformExtraValidations
    //
    /// <summary>
    /// Gets or sets a flag specifying whether extra but possibly slow
    /// validations are performed.
    /// </summary>
    ///
    /// <value>
    /// true to perform extra validations, false otherwise.  The default value
    /// is false.
    /// </value>
    ///
    /// <remarks>
    /// When this property is set to true, the graph should perform extra
    /// validations during certain operations.  For example, when a vertex is
    /// added to the <see cref="Vertices" /> collection, the graph might check
    /// whether the vertex already exists in the collection and throw an
    /// exception if it does.
    ///
    /// <para>
    /// Important note: The extra validations might be very slow, and therefore
    /// this property should be set to true only during development or after an
    /// unexpected problem occurs and the problem needs to be diagnosed.
    /// Depending on the implementation, checking whether a vertex already
    /// exists in the <see cref="Vertices" /> collection might be an O(n)
    /// operation, for example.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    Boolean
    PerformExtraValidations
    {
        get;
        set;
    }

    //*************************************************************************
    //  Method: HasRestrictions
    //
    /// <summary>
    /// Gets a flag that indicates whether the graph imposes specified
    /// restrictions.
    /// </summary>
    ///
    /// <param name="restrictions">
    /// An ORed combination of one or more <see cref="GraphRestrictions" />
    /// flags.
    /// </param>
    ///
    /// <returns>
    /// true if the graph imposes all of the restrictions specified by
    /// <paramref name="restrictions" />.
    /// </returns>
    ///
    /// <remarks>
    /// The graph's restrictions are specified when the graph is created and
    /// cannot be changed.
    ///
    /// <para>
    /// Use <see cref="Restrictions" /> to return all of the graph's
    /// restrictions.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <example>
    /// The following code determines whether a graph prohibits parallel edges.
    ///
    /// <code>
    /// Boolean bNoParallelEdges =
    ///     oGraph.HasRestrictions(GraphRestrictions.NoParallelEdges);
    /// </code>
    ///
    /// </example>
    ///
    /// <seealso cref="Restrictions" />
    //*************************************************************************

    Boolean
    HasRestrictions
    (
        GraphRestrictions restrictions
    );

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


    //*************************************************************************
    //  Event: EdgeAdded
    //
    /// <summary>
    /// Occurs when an edge is added to the <see cref="Edges" /> collection.
    /// </summary>
    //*************************************************************************

    event EdgeEventHandler
    EdgeAdded;


    //*************************************************************************
    //  Event: VertexAdded
    //
    /// <summary>
    /// Occurs when a vertex is added to the <see cref="Vertices" />
    /// collection.
    /// </summary>
    //*************************************************************************

    event VertexEventHandler
    VertexAdded;
}

}
