
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.NodeXL.Core
{
//*****************************************************************************
//  Class: SimpleGraphMatrix
//
/// <summary>
/// Simulates a matrix that can be used to determine whether two vertices are
/// connected by an edge.
/// </summary>
///
/// <remarks>
/// This class is meant for use in algorithms that require that a graph be
/// represented in a square matrix format, where Aij is 1 if there is an edge
/// between vertices i and j, and 0 otherwise.  It doesn't actually create a
/// matrix; instead, it uses an internal sparse dictionary to implement a quick
/// <see cref="Aij" /> lookup.
/// </remarks>
//*****************************************************************************

public class SimpleGraphMatrix : NodeXLBase
{
    //*************************************************************************
    //  Constructor: SimpleGraphMatrix()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleGraphMatrix" />
    /// class.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph that needs to be represented in square matrix format.  The
    /// graph can be of any <see cref="GraphDirectedness" />.
    /// </param>
    //*************************************************************************

    public SimpleGraphMatrix
    (
        IGraph graph
    )
    {
        Debug.Assert(graph != null);

        IEdgeCollection oEdges = graph.Edges;
        m_oEdgeDictionary = new Dictionary<Int64, Byte>(oEdges.Count);

        foreach (IEdge oEdge in oEdges)
        {
            IVertex [] aoVertices = oEdge.Vertices;

            m_oEdgeDictionary[ GetKey( aoVertices[0], aoVertices[1] ) ] = 0;
        }

        AssertValid();
    }

    //*************************************************************************
    //  Method: Aij()
    //
    /// <summary>
    /// Determines whether an edge exists between two of the graph's vertices.
    /// </summary>
    ///
    /// <param name="vertex1">
    /// The first vertex.
    /// </param>
    ///
    /// <param name="vertex2">
    /// The second vertex.
    /// </param>
    ///
    /// <returns>
    /// true if an edge exists between <paramref name="vertex1" /> and
    /// <paramref name="vertex2" />.
    /// </returns>
    ///
    /// <remarks>
    /// The direction of the edge (if the graph is directed) is irrelevant;
    /// swapping the two parameters yields the same return value.  The
    /// existence of multiple edges between the vertices is also irrelevant.
    /// As long as there is at least one connecting edge, true is returned.
    /// </remarks>
    //*************************************************************************

    public Boolean
    Aij
    (
        IVertex vertex1,
        IVertex vertex2
    )
    {
        AssertValid();

        return ( m_oEdgeDictionary.ContainsKey( GetKey(vertex1, vertex2) ) );
    }

    //*************************************************************************
    //  Method: GetKey()
    //
    /// <summary>
    /// Gets the dictionary key for two vertices.
    /// </summary>
    ///
    /// <param name="oVertex1">
    /// The first vertex.
    /// </param>
    ///
    /// <param name="oVertex2">
    /// The second vertex.
    /// </param>
    ///
    /// <returns>
    /// The dictionary key to use.
    /// </returns>
    //*************************************************************************

    protected Int64
    GetKey
    (
        IVertex oVertex1,
        IVertex oVertex2
    )
    {
        Debug.Assert(oVertex1 != null);
        Debug.Assert(oVertex2 != null);
        // AssertValid();

        Int32 iSmallerID = Math.Min(oVertex1.ID, oVertex2.ID);
        Int32 iLargerID = Math.Max(oVertex1.ID, oVertex2.ID);

        // Create an Int64 out of the two Int32 vertex IDs.

        return ( ( ( (Int64)iLargerID ) << 32) + ( (Int64)iSmallerID ) );
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

        Debug.Assert(m_oEdgeDictionary != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The key is what gets returned by GetKey() and the value isn't used.

    protected Dictionary<Int64, Byte> m_oEdgeDictionary;
}

}
