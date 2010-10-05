
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Algorithms
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
/// The edges (A,B) and (A,B) are always considered duplicates.  The edges
/// (A,B) and (B,A) are considered duplicates if the graph is undirected, but
/// not if it is directed.
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
        m_iTotalEdgesAfterMergingDuplicatesNoSelfLoops = Int32.MinValue;

        AssertValid();
    }

    //*************************************************************************
    //  Property: GraphContainsDuplicateEdges()
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
    //  Property: EdgesWithDuplicates()
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
    //  Property: UniqueEdges()
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
    //  Property: TotalEdgesAfterMergingDuplicatesNoSelfLoops()
    //
    /// <summary>
    /// Gets the total number of edges the graph would have if its duplicate
    /// edges were merged and all self-loops were removed.
    /// </summary>
    ///
    /// <value>
    /// The total number of edges the graph would have if its duplicate edges
    /// were merged and all self-loops were removed.
    /// </value>
    ///
    /// <remarks>
    /// This class does not actually merge duplicate edges or remove
    /// self-loops.
    /// </remarks>
    //*************************************************************************

    public Int32
    TotalEdgesAfterMergingDuplicatesNoSelfLoops
    {
        get
        {
            AssertValid();

            // Count the edges and cache the results if they haven't already
            // been counted.

            CountEdges();

            return (m_iTotalEdgesAfterMergingDuplicatesNoSelfLoops);
        }
    }

    //*************************************************************************
    //  Method: CountEdges()
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

            String sVertexNamePair = Edge.GetVertexNamePair(
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

        m_iTotalEdgesAfterMergingDuplicatesNoSelfLoops = 0;

        foreach (String sVertexNamePair in oVertexNamePairs.Keys)
        {
            String [] asVertexNames = sVertexNamePair.Split(
                Edge.VertexNamePairSeparator);

            Debug.Assert(asVertexNames.Length == 2);

            if ( asVertexNames[0] != asVertexNames[1] )
            {
                m_iTotalEdgesAfterMergingDuplicatesNoSelfLoops++;
            }
        }

        m_bEdgesCounted = true;

        AssertValid();
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
            Debug.Assert(m_iTotalEdgesAfterMergingDuplicatesNoSelfLoops >= 0);
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

    /// If m_bEdgesCounted is true, this is the number of edges that would be
    /// in m_oGraph if its duplicate edges were merged and all self-loops were
    /// removed.

    protected Int32 m_iTotalEdgesAfterMergingDuplicatesNoSelfLoops;
}

}
