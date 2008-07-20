
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NetMap.Core;
using Microsoft.NetMap.Visualization;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: NetMapControlUtil
//
/// <summary>
/// Utility methods for working with the <see cref="NetMapControl" />.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class NetMapControlUtil
{
	//*************************************************************************
	//	Method: GetSelectedEdgeIDs()
	//
	/// <summary>
	/// Gets the IDs of the selected edges.
	/// </summary>
	///
	/// <param name="netMapControl">
	/// Control to get the selected edges from.
	/// </param>
	///
	/// <returns>
	/// A list of the IDs of the selected edges.  The IDs are those that came
	/// from the edge worksheet's ID column.  They are NOT IEdge.ID values.
	/// </returns>
	//*************************************************************************

    public static List<Int32>
	GetSelectedEdgeIDsAsList
	(
		NetMapControl netMapControl
	)
    {
		Debug.Assert(netMapControl != null);

		List<Int32> oSelectedEdgeIDs = new List<Int32>();

		foreach (IEdge oSelectedEdge in netMapControl.SelectedEdges)
		{
			// The IDs from the worksheet are stored in the edge Tags.

			if ( !(oSelectedEdge.Tag is Int32) )
			{
				// The ID column is optional, so edges may not have their Tag
				// set.

				continue;
			}

			oSelectedEdgeIDs.Add( (Int32)oSelectedEdge.Tag );
		}

		return (oSelectedEdgeIDs);
    }

	//*************************************************************************
	//	Method: GetSelectedVertexIDsAsList()
	//
	/// <summary>
	/// Gets the IDs of the selected vertices as a list.
	/// </summary>
	///
	/// <param name="netMapControl">
	/// Control to get the selected vertices from.
	/// </param>
	///
	/// <returns>
	/// A list of the IDs of the selected vertices.  The IDs are those that
	/// came from the vertex worksheet's ID column.  They are NOT IVertex.ID
	/// values.
	/// </returns>
	//*************************************************************************

    public static List<Int32>
	GetSelectedVertexIDsAsList
	(
		NetMapControl netMapControl
	)
    {
		Debug.Assert(netMapControl != null);

		List<Int32> oSelectedVertexIDs = new List<Int32>();

		foreach (IVertex oSelectedVertex in netMapControl.SelectedVertices)
		{
			// The IDs from the worksheet are stored in the vertex Tags.

			if ( !(oSelectedVertex.Tag is Int32) )
			{
				// The ID column is optional, so vertices may not have their
				// Tag set.

				continue;
			}

			oSelectedVertexIDs.Add( (Int32)oSelectedVertex.Tag );
		}

		return (oSelectedVertexIDs);
    }

    //*************************************************************************
    //  Method: GetSelectedVerticesAsDictionary()
    //
    /// <summary>
	/// Gets the selected vertices as a dictionary.
    /// </summary>
	///
	/// <param name="netMapControl">
	/// Control to get the selected vertices from.
	/// </param>
	///
	/// <returns>
	/// A dictionary of selected vertices.  The key is the IVertex and the
	/// value isn't used.
	/// </returns>
    //*************************************************************************

	public static Dictionary<IVertex, Char>
	GetSelectedVerticesAsDictionary
	(
		NetMapControl netMapControl
	)
	{
		Debug.Assert(netMapControl != null);

		Dictionary<IVertex, Char> oSelectedVertices =
			new Dictionary<IVertex, Char>();

		foreach (IVertex oSelectedVertex in netMapControl.SelectedVertices)
		{
			oSelectedVertices[oSelectedVertex] = ' ';
		}

		return (oSelectedVertices);
	}

    //*************************************************************************
    //  Method: GetSelectedVerticesAsDictionary2()
    //
    /// <summary>
	/// Gets the selected vertices as a dictionary.
    /// </summary>
	///
	/// <param name="netMapControl">
	/// Control to get the selected vertices from.
	/// </param>
	///
	/// <returns>
	/// A dictionary of selected vertices.  The key is the IVertex.ID and the
	/// value is the IVertex.
	/// </returns>
    //*************************************************************************

	public static Dictionary<Int32, IVertex>
	GetSelectedVerticesAsDictionary2
	(
		NetMapControl netMapControl
	)
	{
		Debug.Assert(netMapControl != null);

		Dictionary<Int32, IVertex> oSelectedVertices =
			new Dictionary<Int32, IVertex>();

		foreach (IVertex oSelectedVertex in netMapControl.SelectedVertices)
		{
			oSelectedVertices[oSelectedVertex.ID] = oSelectedVertex;
		}

		return (oSelectedVertices);
	}

    //*************************************************************************
    //  Method: GetSelectedEdgesAsDictionary()
    //
    /// <summary>
	/// Gets the selected edges as a dictionary.
    /// </summary>
	///
	/// <param name="netMapControl">
	/// Control to get the selected edges from.
	/// </param>
	///
	/// <returns>
	/// A dictionary of selected edges.  The key is the IEdge and the value
	/// isn't used.
	/// </returns>
    //*************************************************************************

	public static Dictionary<IEdge, Char>
	GetSelectedEdgesAsDictionary
	(
		NetMapControl netMapControl
	)
	{
		Debug.Assert(netMapControl != null);

		Dictionary<IEdge, Char> oSelectedEdges = new Dictionary<IEdge, Char>();

		foreach (IEdge oSelectedEdge in netMapControl.SelectedEdges)
		{
			oSelectedEdges[oSelectedEdge] = ' ';
		}

		return (oSelectedEdges);
	}

    //*************************************************************************
    //  Method: GetSelectedEdgesAsDictionary2()
    //
    /// <summary>
	/// Gets the selected edges as a dictionary.
    /// </summary>
	///
	/// <param name="netMapControl">
	/// Control to get the selected edges from.
	/// </param>
	///
	/// <returns>
	/// A dictionary of selected edges.  The key is the IEdge.ID and the value
	/// is the IEdge.
	/// </returns>
    //*************************************************************************

	public static Dictionary<Int32, IEdge>
	GetSelectedEdgesAsDictionary2
	(
		NetMapControl netMapControl
	)
	{
		Debug.Assert(netMapControl != null);

		Dictionary<Int32, IEdge> oSelectedEdges =
			new Dictionary<Int32, IEdge>();

		foreach (IEdge oSelectedEdge in netMapControl.SelectedEdges)
		{
			oSelectedEdges[oSelectedEdge.ID] = oSelectedEdge;
		}

		return (oSelectedEdges);
	}

    //*************************************************************************
    //  Method: GetVerticesAsArray()
    //
    /// <summary>
	/// Gets the entire collection of vertices as an array.
    /// </summary>
	///
	/// <param name="netMapControl">
	/// Control to get the vertices from.
	/// </param>
	///
	/// <returns>
	/// An array of vertices.
	/// </returns>
    //*************************************************************************

	public static IVertex []
	GetVerticesAsArray
	(
		NetMapControl netMapControl
	)
	{
		Debug.Assert(netMapControl != null);

		IVertexCollection oVertices = netMapControl.Graph.Vertices;
		Int32 iVertices = oVertices.Count;
		IVertex [] aoVertices = new IVertex[iVertices];
		Int32 i = 0;

		foreach (IVertex oVertex in oVertices)
		{
			aoVertices[i] = oVertex;

			i++;
		}

		return (aoVertices);
	}

    //*************************************************************************
    //  Method: GetEdgesAsArray()
    //
    /// <summary>
	/// Gets the entire collection of edges as an array.
    /// </summary>
	///
	/// <param name="netMapControl">
	/// Control to get the edges from.
	/// </param>
	///
	/// <returns>
	/// An array of edges.
	/// </returns>
    //*************************************************************************

	public static IEdge []
	GetEdgesAsArray
	(
		NetMapControl netMapControl
	)
	{
		Debug.Assert(netMapControl != null);

		IEdgeCollection oEdges = netMapControl.Graph.Edges;
		Int32 iEdges = oEdges.Count;
		IEdge [] aoEdges = new IEdge[iEdges];
		Int32 i = 0;

		foreach (IEdge oEdge in oEdges)
		{
			aoEdges[i] = oEdge;

			i++;
		}

		return (aoEdges);
	}

	//*************************************************************************
	//	Method: SelectSubgraphs()
	//
	/// <summary>
	///	Selects the subgraphs for one or more vertices.
	/// </summary>
	///
	/// <param name="netMapControl">
	/// The NetMapControl whose subgraphs should be selected.
	/// </param>
	///
	/// <param name="verticesToSelectSubgraphsFor">
	/// Zero ore more vertices whose subgraphs should be selected.
	/// </param>
	///
	/// <param name="levels">
    /// The number of levels of adjacent vertices to select in each subgraph.
	/// Must be a multiple of 0.5.  If 0, a subgraph includes just the vertex;
	/// if 1, it includes the vertex and its adjacent vertices; if 2, it
	/// includes the vertex, its adjacent vertices, and their adjacent
	/// vertices; and so on.  The difference between N.5 and N.0 is that N.5
	/// includes any edges connecting the outermost vertices to each other,
	/// whereas N.0 does not.  1.5, for example, includes any edges that
	/// connect the vertex's adjacent vertices to each other, whereas 1.0
	/// includes only those edges that connect the adjacent vertices to the
	/// vertex.
	/// </param>
	///
	/// <param name="selectConnectingEdges">
	/// true to select the subgraphs' connecting edges.
	/// </param>
	//*************************************************************************

    public static void
	SelectSubgraphs
	(
		NetMapControl netMapControl,
		IVertex [] verticesToSelectSubgraphsFor,
		Decimal levels,
		Boolean selectConnectingEdges
	)
    {
		Debug.Assert(netMapControl != null);
		Debug.Assert(verticesToSelectSubgraphsFor != null);
		Debug.Assert(levels >= 0);
		Debug.Assert(Decimal.Remainder(levels, 0.5M) == 0M);

		// Create dictionaries for all of the vertices and edges that will be
		// selected.  The key is the IVertex or IEdge and the value isn't used.
		// Dictionaries are used to prevent the same vertex or edge from being
		// selected twice.

		Dictionary<IVertex, Char> oAllSelectedVertices =
			new Dictionary<IVertex, Char>();

		Dictionary<IEdge, Char> oAllSelectedEdges =
			new Dictionary<IEdge, Char>();

		// Loop through the specified vertices.

		foreach (IVertex oVertexToSelectSubgraphFor in
			verticesToSelectSubgraphsFor)
		{
			// Create similar dictionaries for the vertices and edges that will
			// be selected for this subgraph only.

			Dictionary<IVertex, Char> oThisSubgraphSelectedVertices =
				new Dictionary<IVertex, Char>();

			Dictionary<IEdge, Char> oThisSubgraphSelectedEdges =
				new Dictionary<IEdge, Char>();

			SelectSubgraphRecursive(oVertexToSelectSubgraphFor, levels,
				selectConnectingEdges, oThisSubgraphSelectedVertices,
				oThisSubgraphSelectedEdges);

			if ( selectConnectingEdges && ( (levels / 0.5M) % 2M ) == 1 )
			{
				// decLevels is 0.5, 1.5, 2.5, etc.  Any edges connecting the
				// outermost vertices need to be selected.

				SelectOutermostSubgraphEdgesRecursive(
					oVertexToSelectSubgraphFor, levels,
					oThisSubgraphSelectedVertices, oThisSubgraphSelectedEdges);
			}

			// Consolidate the subgraph's selected vertices and edges into the
			// "all" dictionaries.

			foreach (IVertex oVertex in oThisSubgraphSelectedVertices.Keys)
			{
				oAllSelectedVertices[oVertex] = ' ';
			}

			foreach (IEdge oEdge in oThisSubgraphSelectedEdges.Keys)
			{
				oAllSelectedEdges[oEdge] = ' ';
			}
		}

		// Replace the selection.

		netMapControl.SetSelected(

			CollectionUtil.DictionaryKeysToArray<IVertex, Char>(
				oAllSelectedVertices),

			CollectionUtil.DictionaryKeysToArray<IEdge, Char>(
				oAllSelectedEdges)
			);
    }

	//*************************************************************************
	//	Method: SelectSubgraphRecursive()
	//
	/// <summary>
	///	Recursively selects a subgraph's vertices and edges.
	/// </summary>
	///
	/// <param name="oVertex">
	/// Vertex whose adjacent vertices should be recursively selected.
	/// </param>
	///
	/// <param name="decLevels">
	/// The number of levels to go out.
	/// </param>
	///
	/// <param name="bSelectConnectingEdges">
	/// true to select the subgraph's connecting edges.
	/// </param>
	///
	/// <param name="oSelectedVertices">
	/// Dictionary of selected vertices.  The key is the IVertex and the value
	/// isn't used.
	/// </param>
	///
	/// <param name="oSelectedEdges">
	/// Dictionary of selected edges.  The key is the IEdge and the value isn't
	/// used.
	/// </param>
	//*************************************************************************

	private static void
	SelectSubgraphRecursive
	(
		IVertex oVertex,
		Decimal decLevels,
		Boolean bSelectConnectingEdges,
		Dictionary<IVertex, Char> oSelectedVertices,
		Dictionary<IEdge, Char> oSelectedEdges
	)
	{
		Debug.Assert(oVertex != null);
		Debug.Assert(decLevels >= 0);
		Debug.Assert(oSelectedVertices != null);
		Debug.Assert(oSelectedEdges != null);

		// Note: This algorithm is inefficent in that it can repeatedly recurse
		// through the same vertices.  How to improve it?

		oSelectedVertices[oVertex] = ' ';

		if (decLevels >= 1M)
		{
			foreach (IVertex oAdjacentVertex in oVertex.AdjacentVertices)
			{
				SelectSubgraphRecursive(oAdjacentVertex, decLevels - 1M,
					bSelectConnectingEdges, oSelectedVertices, oSelectedEdges);
			}

			if (bSelectConnectingEdges)
			{
				foreach (IEdge oIncidentEdge in oVertex.IncidentEdges)
				{
					oSelectedEdges[oIncidentEdge] = ' ';
				}
			}
		}
	}

	//*************************************************************************
	//	Method: SelectOutermostSubgraphEdgesRecursive()
	//
	/// <summary>
	/// Recursively selects any edges connecting the outermost vertices in a
	/// subgraph.
	/// </summary>
	///
	/// <param name="oVertex">
	/// Vertex whose adjacent vertices were recursively selected.
	/// </param>
	///
	/// <param name="decLevels">
	/// The number of levels to go out.
	/// </param>
	///
	/// <param name="oSelectedVertices">
	/// Dictionary of selected vertices.  The key is the IVertex and the value
	/// isn't used.
	/// </param>
	///
	/// <param name="oSelectedEdges">
	/// Dictionary of selected edges.  The key is the IEdge and the value isn't
	/// used.
	/// </param>
	//*************************************************************************

	private static void
	SelectOutermostSubgraphEdgesRecursive
	(
		IVertex oVertex,
		Decimal decLevels,
		Dictionary<IVertex, Char> oSelectedVertices,
		Dictionary<IEdge, Char> oSelectedEdges
	)
	{
		Debug.Assert(oVertex != null);
		Debug.Assert(decLevels >= 0);
		Debug.Assert(oSelectedVertices != null);
		Debug.Assert(oSelectedEdges != null);

		if (decLevels == 0.5M)
		{
			// oVertex is an outermost vertex.  Loop through its incident
			// edges.

			foreach (IEdge oIncidentEdge in oVertex.IncidentEdges)
			{
				if ( oSelectedEdges.ContainsKey(oIncidentEdge) )
				{
					// The edge is already selected.

					continue;
				}

				// Figure out which of the edge's vertices is the one adjacent
				// to oVertex.

				IVertex [] aoIncidentEdgeVertices = oIncidentEdge.Vertices;
				IVertex oVertex0 = aoIncidentEdgeVertices[0];
				IVertex oVertex1 = aoIncidentEdgeVertices[1];

				Boolean bVertexIsVertex0 = (oVertex.ID == oVertex0.ID);

				IVertex oAdjacentVertex = bVertexIsVertex0 ?
					oVertex1 : oVertex0;

				if ( oSelectedVertices.ContainsKey(oAdjacentVertex) )
				{
					// The edge's adjacent vertex is in the subgraph, so select
					// the edge.

					oSelectedEdges[oIncidentEdge] = ' ';
				}
			}

			return;
		}

		// Recurse into the adjacent vertices.

		foreach (IVertex oAdjacentVertex in oVertex.AdjacentVertices)
		{
			SelectOutermostSubgraphEdgesRecursive(oAdjacentVertex,
				decLevels - 1M, oSelectedVertices, oSelectedEdges);
		}
	}
}

}
