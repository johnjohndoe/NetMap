
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: SubgraphCalculator
//
/// <summary>
/// Gets a subgraph for a specified vertex.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class SubgraphCalculator
{
	//*************************************************************************
	//	Method: GetSubgraph()
	//
	/// <summary>
	/// Gets a subgraph for a specified vertex.
	/// </summary>
	///
	/// <param name="vertex">
	/// Vertex to get a subgraph for.
	/// </param>
	///
	/// <param name="levels">
	/// The number of levels to go out from <paramref name="vertex" />.
	/// Must be a multiple of 0.5.  If 0, the subgraph includes just the
	/// vertex; if 1, it includes the vertex and its adjacent vertices; if 2,
	/// it includes the vertex, its adjacent vertices, and their adjacent
	/// vertices; and so on.  The difference between N.5 and N.0 is that N.5
	/// includes any edges connecting the outermost vertices to each other,
	/// whereas N.0 does not.  1.5, for example, includes any edges that
	/// connect the vertex's adjacent vertices to each other, whereas 1.0
	/// includes only those edges that connect the adjacent vertices to the
	/// vertex.
	/// </param>
	///
	/// <param name="getSubgraphEdges">
	/// true to get the subgraph's vertices and edges, false to get the
	/// subgraph's vertices only.
	/// 
	/// </param>
	///
	/// <param name="subgraphVertices">
	/// Where a dictionary of subgraph vertices gets stored.  The key is the
	/// IVertex and the value is the vertex's level, which is the distance of
	/// the subgraph vertex from <paramref name="vertex" />.
	/// </param>
	///
	/// <param name="subgraphEdges">
	/// Where a dictionary of subgraph edges gets stored.  The key is the IEdge
	/// and the value isn't used.  If <paramref name="getSubgraphEdges" /> is
	/// false, the dictionary is always empty.
	/// </param>
	//*************************************************************************

	public static void
	GetSubgraph
	(
		IVertex vertex,
		Decimal levels,
		Boolean getSubgraphEdges,
		out Dictionary<IVertex, Int32> subgraphVertices,
		out Dictionary<IEdge, Char> subgraphEdges
	)
	{
		Debug.Assert(vertex != null);
		Debug.Assert(levels >= 0);
		Debug.Assert(Decimal.Remainder(levels, 0.5M) == 0M);

		subgraphVertices = new Dictionary<IVertex, Int32>();
		subgraphEdges = new Dictionary<IEdge, Char>();

		// This algorithm was suggested by Jure Leskovec at Carnegie Mellon
		// University.

		// Create a queue of vertices that need to be visited.  A queue is used
		// instead of recursion.

		Queue<IVertex> oVerticesToVisit = new Queue<IVertex>();

		// Get the level of the outermost vertices.

        Int32 iOuterLevel = (Int32)Math.Truncate(levels);

        subgraphVertices.Add(vertex, 0);
		oVerticesToVisit.Enqueue(vertex);

		while (oVerticesToVisit.Count > 0)
		{
			IVertex oVertexToVisit = oVerticesToVisit.Dequeue();

			Int32 iLevel = subgraphVertices[oVertexToVisit];

			if (iLevel == iOuterLevel)
			{
				continue;
			}

			foreach (IEdge oIncidentEdge in oVertexToVisit.IncidentEdges)
			{
				if (getSubgraphEdges)
				{
					// Use the Item property, not the Add() method, because the
					// same edge may be added twice.

					subgraphEdges[oIncidentEdge] = ' ';
				}

				IVertex oAdjacentVertex =
					oIncidentEdge.GetAdjacentVertex(oVertexToVisit);

				if ( !subgraphVertices.ContainsKey(oAdjacentVertex) )
				{
                    subgraphVertices.Add(oAdjacentVertex, iLevel + 1);
					oVerticesToVisit.Enqueue(oAdjacentVertex);
				}
			}
		}

		if ( getSubgraphEdges && ( (levels / 0.5M) % 2M ) == 1 )
		{
			// levels is 0.5, 1.5, 2.5, etc.  Any edges connecting the
			// outermost vertices need to be added to the edge dictionary.

			AddOuterEdgesToSubgraph(iOuterLevel, subgraphVertices,
				subgraphEdges);
		}
	}

	//*************************************************************************
	//	Method: AddOuterEdgesToSubgraph()
	//
	/// <summary>
	/// Adds any subgraph edges connecting the outermost vertices.
	/// </summary>
	///
	/// <param name="iOuterLevel">
	/// Level of the outermost vertices.
	/// </param>
	///
	/// <param name="oSubgraphVertices">
	/// Dictionary of subgraph vertices.  The key is the IVertex and the value
	/// is the vertex's level.
	/// </param>
	///
	/// <param name="oSubgraphEdges">
	/// Dictionary of subgraph edges.  The key is the IEdge and the value isn't
	/// used.  This method adds any outer edges to this dictionary.
	/// </param>
	//*************************************************************************

	private static void
	AddOuterEdgesToSubgraph
	(
		Int32 iOuterLevel,
		Dictionary<IVertex, Int32> oSubgraphVertices,
		Dictionary<IEdge, Char> oSubgraphEdges
	)
	{
		Debug.Assert(iOuterLevel > 0);
		Debug.Assert(oSubgraphVertices != null);
		Debug.Assert(oSubgraphEdges != null);

		// Loop through the subgraph's vertices.

		foreach (KeyValuePair<IVertex, Int32> oKeyValuePair in
			oSubgraphVertices)
		{
			if (oKeyValuePair.Value != iOuterLevel)
			{
				continue;
			}

			IVertex oVertex = oKeyValuePair.Key;

			// This is an outermost vertex.  Loop through its incident edges.

			foreach (IEdge oIncidentEdge in oVertex.IncidentEdges)
			{
				IVertex oAdjacentVertex =
					oIncidentEdge.GetAdjacentVertex(oVertex);

				Int32 iAdjacentVertexLevel;

				// If the incident edge connects the outermost vertex to
				// another outermost vertex in the subgraph, add the edge.

				if (
					oSubgraphVertices.TryGetValue(
						oAdjacentVertex, out iAdjacentVertexLevel)
					&&
					iAdjacentVertexLevel == iOuterLevel
					)
				{
					oSubgraphEdges[oIncidentEdge] = ' ';
				}
			}
		}
	}
}

}
