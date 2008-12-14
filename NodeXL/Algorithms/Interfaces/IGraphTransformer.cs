
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Algorithms
{
//*****************************************************************************
//  Interface: IGraphTransformer
//
/// <summary>
///	Supports the transformation of one graph into a new graph.
/// </summary>
//*****************************************************************************

public interface IGraphTransformer
{
    //*************************************************************************
    //  Method: Transform()
    //
    /// <overloads>
    /// Transforms an existing graph into a new graph.
    /// </overloads>
    ///
    /// <summary>
    /// Transforms an existing graph into a new graph using specified graph,
	///	vertex, and edge creators.
    /// </summary>
    ///
    /// <param name="graph">
    /// Existing graph to transform.
    /// </param>
    ///
    /// <param name="newGraphFactory">
    /// Object that can create a graph.
    /// </param>
    ///
    /// <param name="newVertexFactory">
    /// Object that can create vertices.
    /// </param>
	///
    /// <param name="newEdgeFactory">
    /// Object that can create edges.
    /// </param>
	///
    /// <returns>
    /// The new graph, as an <see cref="IGraph" />.  The graph is created using
	/// <paramref name="newGraphFactory" />.
    /// </returns>
    ///
    /// <remarks>
	///	This method transforms <paramref name="graph" /> into a new graph.
	///	<paramref name="graph" /> is not modified.
	///
	/// <para>
	///	To create a graph using default graph, vertex, and edge creators, use
	///	the other version of this method.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    IGraph
    Transform
    (
        IGraph graph,
		IGraphFactory newGraphFactory,
		IVertexFactory newVertexFactory,
		IEdgeFactory newEdgeFactory
    );

    //*************************************************************************
    //  Method: Transform()
    //
    /// <summary>
    /// Transforms an existing graph into a new graph using default graph,
	///	vertex, and edge creators.
    /// </summary>
    ///
    /// <param name="graph">
    /// Existing graph to transform.
    /// </param>
    ///
    /// <returns>
    /// The new <see cref="Graph" /> object.
    /// </returns>
    ///
    /// <remarks>
	///	This method transforms <paramref name="graph" /> into a new graph.
	///	<paramref name="graph" /> is not modified.
	///
	/// <para>
	///	The new graph is of type <see cref="Graph" />.  The new graph's
	///	vertices and edges are of type <see cref="Vertex" /> and <see
	///	cref="Edge" />.  To create a graph using specified graph, vertex, and
	///	edge creators, use the other version of this method.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    Graph
    Transform
    (
        IGraph graph
    );
}

}
