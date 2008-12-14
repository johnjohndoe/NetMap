
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: UndirectedToDirectedGraphTransformer
//
/// <summary>
/// Transforms an undirected graph into a new directed graph.
/// </summary>
///
/// <remarks>
///	Given an undirected graph X (a graph for which IGraph.<see
///	cref="IGraph.Directedness" /> is <see
///	cref="GraphDirectedness.Undirected" />), this class creates a new directed
///	graph Y such that if an undirected edge (A,B) exists in X, new directed
///	edges (A,B) and (B,A) are created in Y.
/// </remarks>
///
///	<example>
///	The following code transforms the existing undirected graph
///	oUndirectedGraph into a new directed graph of type <see cref="Graph" />.
///
///	<code>
/// UndirectedToDirectedGraphTransformer oUndirectedToDirectedGraphTransformer
///		= new UndirectedToDirectedGraphTransformer();
///
/// Graph oDirectedGraph =
///		oUndirectedToDirectedGraphTransformer.Transform(oUndirectedGraph);
///	</code>
///
///	</example>
//*****************************************************************************

public class UndirectedToDirectedGraphTransformer :
	NodeXLBase, IGraphTransformer
{
    //*************************************************************************
    //  Constructor: UndirectedToDirectedGraphTransformer()
    //
    /// <summary>
    /// Initializes a new instance of the UndirectedToDirectedGraphTransformer
	///	class.
    /// </summary>
    //*************************************************************************

    public UndirectedToDirectedGraphTransformer()
    {
		// TODO

		throw new NotImplementedException();
    }

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

    public IGraph
    Transform
    (
        IGraph graph,
		IGraphFactory newGraphFactory,
		IVertexFactory newVertexFactory,
		IEdgeFactory newEdgeFactory
    )
	{
		AssertValid();

		// TODO

		throw new NotImplementedException();
	}

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

    public Graph
    Transform
    (
        IGraph graph
    )
	{
		AssertValid();

		// TODO

		throw new NotImplementedException();
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

        // TODO
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // TODO
}

}
