
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NetMap.Core
{
//*****************************************************************************
//  Interface: IGraphFactory
//
/// <summary>
///	Represents an object that knows how to create a graph.
/// </summary>
///
/// <remarks>
/// If you implement a custom graph class from scratch, you may also want to
///	implement <see cref="IGraphFactory" />.  Several methods in the NetMap
///	system create a graph and allow the type of the new graph to be specified
///	via an object that implements <see cref="IGraphFactory" />.  For example,
///	the IGraphTransformer.Transform method takes an optional <see
///	cref="IGraphFactory" /> argument that specifies the type of the transformed
///	graph.
/// </remarks>
///
/// <seealso cref="IGraph" />
//*****************************************************************************

public interface IGraphFactory
{
    //*************************************************************************
    //  Method: CreateGraph()
    //
    /// <summary>
    /// Creates a graph object with a specified directedness and restrictions.
    /// </summary>
    ///
    /// <param name="directedness">
	///	Specifies the type of edges that can be added to the graph.
    /// </param>
	///
    /// <param name="restrictions">
	///	Specifies restrictions imposed by the graph.
    /// </param>
	///
    /// <returns>
	///	The <see cref="IGraph" /> interface on a newly created graph object.
    /// </returns>
    //*************************************************************************

	IGraph
	CreateGraph
	(
		GraphDirectedness directedness,
		GraphRestrictions restrictions
	);
}

}
