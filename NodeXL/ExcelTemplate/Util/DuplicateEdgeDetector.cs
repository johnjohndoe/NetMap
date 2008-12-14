
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: DuplicateEdgeDetector
//
/// <summary>
/// Counts duplicate and unique edges in a graph.
/// </summary>
///
/// <remarks>
/// The <see cref="IIdentityProvider.Name" /> property on each of an
/// edge's vertices is used to test for duplicate edges.
///
/// <para>
/// The edges A,B and A,B are always considered duplicates.  The edges A,B
/// and B,A are considered duplicates if the graph is undirected, but not if it
/// is directed.
/// </para>
///
/// <para>
/// Edges with null or empty names are ignored.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class DuplicateEdgeDetector
{
    //*************************************************************************
    //  Constructor: DuplicateEdgeDetector()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateEdgeDetector" />
	/// class.
    /// </summary>
	///
	/// <param name="graph">
	/// Graph to check.
	/// </param>
    //*************************************************************************

    public DuplicateEdgeDetector
	(
		IGraph graph
	)
    {
		m_oGraph = graph;

		m_bEdgesCounted = false;
		m_iUniqueEdges = Int32.MinValue;
		m_iEdgesWithDuplicates = Int32.MinValue;
		m_iEdges = Int32.MinValue;

		AssertValid();
    }

	//*************************************************************************
	//	Property: GraphContainsDuplicateEdges()
	//
	/// <summary>
	/// Gets a flag indicating whether the graph contains at least one pair
	/// of duplicate edges.
	/// </summary>
	///
	/// <value>
	/// true if <paramref name="graph" /> contains at least one pair of
	/// duplicate edges.
	/// </value>
	//*************************************************************************

    public Boolean
	GraphContainsDuplicateEdges
    {
		get
		{
			AssertValid();

			// Count the edges and cache the results if they haven't already
			// been counted.

			CountEdges();

			return (m_iEdgesWithDuplicates > 0);
		}
    }

	//*************************************************************************
	//	Property: EdgesWithDuplicates()
	//
	/// <summary>
	/// Gets the number of edges in the graph that have duplicates.
	/// </summary>
	///
	/// <value>
	/// The number of edges in the graph that have duplicates.
	/// </value>
	//*************************************************************************

    public Int32
	EdgesWithDuplicates
    {
		get
		{
			AssertValid();

			// Count the edges and cache the results if they haven't already
			// been counted.

			CountEdges();

			return (m_iEdgesWithDuplicates);
		}
    }

	//*************************************************************************
	//	Property: UniqueEdges()
	//
	/// <summary>
	/// Gets the number of unique edges in the graph.
	/// </summary>
	///
	/// <value>
	/// The number of unique edges in the graph.
	/// </value>
	//*************************************************************************

    public Int32
	UniqueEdges
    {
		get
		{
			AssertValid();

			// Count the edges and cache the results if they haven't already
			// been counted.

			CountEdges();

			return (m_iUniqueEdges);
		}
    }

	//*************************************************************************
	//	Property: Edges()
	//
	/// <summary>
	/// Gets the number of edges in the graph.
	/// </summary>
	///
	/// <value>
	/// The number of edges in the graph.
	/// </value>
	///
	/// <remarks>
	/// The number of edges in the graph is the sum of <see
	/// cref="UniqueEdges" /> and <see cref="EdgesWithDuplicates" />.
	/// </remarks>
	//*************************************************************************

    public Int32
	Edges
    {
		get
		{
			AssertValid();

			// Count the edges and cache the results if they haven't already
			// been counted.

			CountEdges();

			return (m_iEdges);
		}
    }

	//*************************************************************************
	//	Method: CountEdges()
	//
	/// <summary>
	/// Counts the edges and cache the results if they haven't already been
	/// counted.
	/// </summary>
	//*************************************************************************

    protected void
	CountEdges()
    {
		AssertValid();

		if (m_bEdgesCounted)
		{
			return;
		}

		m_iUniqueEdges = 0;
		m_iEdgesWithDuplicates = 0;
		m_iEdges = 0;

		IEdgeCollection oEdges = m_oGraph.Edges;

		Boolean bGraphIsDirected =
			(m_oGraph.Directedness == GraphDirectedness.Directed);

		// Create a dictionary of vertex name pairs.  The key is the vertex
		// name pair and the value is true if the edge has duplicates or false
		// if it doesn't.

		Dictionary <String, Boolean> oVertexNamePairs =
			new Dictionary<String, Boolean>(oEdges.Count);

		foreach (IEdge oEdge in oEdges)
		{
			IVertex [] aoVertices = oEdge.Vertices;
			String sVertex0Name = aoVertices[0].Name;
			String sVertex1Name = aoVertices[1].Name;

			if ( String.IsNullOrEmpty(sVertex0Name) ||
			     String.IsNullOrEmpty(sVertex1Name) )
			{
				continue;
			}

			m_iEdges++;

			String sVertexNamePair = GetVertexNamePair(
				sVertex0Name, sVertex1Name, bGraphIsDirected);

			Boolean bEdgeHasDuplicate;

			if ( oVertexNamePairs.TryGetValue(sVertexNamePair,
				out bEdgeHasDuplicate) )
			{
				if (!bEdgeHasDuplicate)
				{
					// This is the edge's first duplicate.

					m_iUniqueEdges--;
					m_iEdgesWithDuplicates++;

					oVertexNamePairs[sVertexNamePair] = true;
				}

				m_iEdgesWithDuplicates++;
			}
			else
			{
				m_iUniqueEdges++;

				oVertexNamePairs.Add(sVertexNamePair, false);
			}
		}

		m_bEdgesCounted = true;

		AssertValid();
	}

	//*************************************************************************
	//	Method: GetVertexNamePair()
	//
	/// <summary>
	/// Combines the names of an edge's vertices into a name pair suitable for
	/// use as a dictionary key.
	/// </summary>
	///
	/// <param name="vertex0Name">
	/// Name of the edge's first vertex.  Can't be null or empty.
	/// </param>
	///
	/// <param name="vertex1Name">
	/// Name of the edge's second vertex.  Can't be null or empty.
	/// </param>
	///
	/// <param name="graphIsDirected">
	/// true if the graph is directed, false if it is undirected.
	/// </param>
	///
	/// <returns>
	/// A name pair suitable for use as a dictionary key.
	/// </returns>
	///
	/// <remarks>
	/// The returned string can be used as a key in a dictionary used to find
	/// duplicate edges.
	/// </remarks>
	//*************************************************************************

    public static String
	GetVertexNamePair
	(
		String vertex0Name,
		String vertex1Name,
		Boolean graphIsDirected
	)
    {
		Debug.Assert( !String.IsNullOrEmpty(vertex0Name) );
		Debug.Assert( !String.IsNullOrEmpty(vertex1Name) );

		// This is a vertical tab, which is highly unlikely to be used in a
		// vertex name.

		const Char Separator = '\v';

		String sVertexNamePair;

		// In the undirected case, guarantee that A,B and B,A are considered
		// duplicates by always pairing them in the same order.

		if (graphIsDirected || vertex0Name.CompareTo(vertex1Name) < 0)
		{
			sVertexNamePair = vertex0Name + Separator + vertex1Name;
		}
		else
		{
			sVertexNamePair = vertex1Name + Separator + vertex0Name;
		}

		return (sVertexNamePair);
	}


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
		Debug.Assert(m_oGraph != null);

		if (m_bEdgesCounted)
		{
			Debug.Assert(m_iUniqueEdges >= 0);
			Debug.Assert(m_iEdgesWithDuplicates >= 0);
			Debug.Assert(m_iEdges >= 0);

			Debug.Assert(m_iEdges == m_iUniqueEdges + m_iEdgesWithDuplicates);
		}
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Graph to check.

	protected IGraph m_oGraph;

	/// true if the edges have already been counted.

	protected Boolean m_bEdgesCounted;

	/// If m_bEdgesCounted is true, this is the number of unique edges in
	/// m_oGraph.

	protected Int32 m_iUniqueEdges;

	/// If m_bEdgesCounted is true, this is the number of edges in m_oGraph
	/// that have duplicates.

	protected Int32 m_iEdgesWithDuplicates;

	/// If m_bEdgesCounted is true, this is the total number of edges in
	/// m_oGraph.

	protected Int32 m_iEdges;
}

}
