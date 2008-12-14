
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.Core
{
//*****************************************************************************
//  Interface: IEdgeFactory
//
/// <summary>
///	Represents an object that knows how to create an edge.
/// </summary>
///
/// <remarks>
/// If you implement a custom edge class from scratch, you may also want to
///	implement <see cref="IEdgeFactory" />.  Several methods in the NodeXL
///	system create an edge and allow the type of the new edge to be specified
///	via an object that implements <see cref="IEdgeFactory" />.  For example,
///	the IGraphTransformer.Transform method takes an optional <see
///	cref="IEdgeFactory" /> argument that specifies the type of the edges in the
///	transformed graph.
/// </remarks>
///
/// <seealso cref="IEdge" />
//*****************************************************************************

public interface IEdgeFactory
{
    //*************************************************************************
    //  Method: CreateEdge()
    //
    /// <summary>
    /// Creates an edge object with a specified directedness.
    /// </summary>
	///
    /// <param name="vertex1">
	///	The edge's first vertex.  The vertex must have already been added to
	/// the graph to which the new edge will be added.
    /// </param>
    ///
    /// <param name="vertex2">
	///	The edge's second vertex.  The vertex must have already been added to
	/// the graph to which the new edge will be added.
    /// </param>
    ///
    /// <param name="isDirected">
	///	If true, <paramref name="vertex1" /> is the edge's back vertex and
	///	<paramref name="vertex2" /> is the edge's front vertex.  If false, the
	/// edge is undirected.
    /// </param>
    ///
    /// <returns>
	///	The <see cref="IEdge" /> interface on a newly created edge object.
    /// </returns>
	///
	/// <seealso cref="CreateUndirectedEdge" />
	/// <seealso cref="CreateDirectedEdge" />
    //*************************************************************************

	IEdge
	CreateEdge
	(
		IVertex vertex1,
		IVertex vertex2,
		Boolean isDirected
	);

    //*************************************************************************
    //  Method: CreateUndirectedEdge()
    //
    /// <summary>
    /// Creates an undirected edge object.
    /// </summary>
	///
    /// <param name="vertex1">
	///	The edge's first vertex.  The vertex must have already been added to
	/// the graph to which the new edge will be added.
    /// </param>
    ///
    /// <param name="vertex2">
	///	The edge's second vertex.  The vertex must have already been added to
	/// the graph to which the new edge will be added.
    /// </param>
    ///
    /// <returns>
	///	The <see cref="IEdge" /> interface on a newly created edge object.
    /// </returns>
	///
	/// <seealso cref="CreateDirectedEdge" />
	/// <seealso cref="CreateEdge" />
    //*************************************************************************

	IEdge
	CreateUndirectedEdge
	(
		IVertex vertex1,
		IVertex vertex2
	);

    //*************************************************************************
    //  Method: CreateDirectedEdge()
    //
    /// <summary>
    /// Creates a directed edge object.
    /// </summary>
	///
    /// <param name="backVertex">
	///	The edge's back vertex.  The vertex must have already been added to
	/// the graph to which the new edge will be added.
    /// </param>
    ///
    /// <param name="frontVertex">
	///	The edge's front vertex.  The vertex must have already been added to
	/// the graph to which the new edge will be added.
    /// </param>
    ///
    /// <returns>
	///	The <see cref="IEdge" /> interface on a newly created edge object.
    /// </returns>
	///
	/// <seealso cref="CreateUndirectedEdge" />
	/// <seealso cref="CreateEdge" />
    //*************************************************************************

	IEdge
	CreateDirectedEdge
	(
		IVertex backVertex,
		IVertex frontVertex
	);
}

}
