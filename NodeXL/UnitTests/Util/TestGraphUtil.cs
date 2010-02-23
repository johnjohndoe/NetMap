
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: TestGraphUtil
//
/// <summary>
/// Utility methods for testing graph components.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class TestGraphUtil
{
    //*************************************************************************
    //  Method: AddVertices()
    //
    /// <summary>
    /// Adds a specified number of <see cref="Vertex" /> objects to a graph
    /// using the <see cref="IVertexCollection.Add(IVertex)" /> method.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// Graph to add vertices to.
    /// </param>
    ///
    /// <param name="iVerticesToAdd">
    /// Number of vertices to add.
    /// </param>
    ///
    /// <returns>
    /// An array of the added vertices.
    /// </returns>
    //*************************************************************************

    public static IVertex[]
    AddVertices
    (
        IGraph oGraph,
        Int32 iVerticesToAdd
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(iVerticesToAdd >= 0);

        IVertex[] aoVertices = new IVertex[iVerticesToAdd];

        IVertexCollection oVertexCollection = oGraph.Vertices;

        // Add the vertices.

        for (Int32 i = 0; i < iVerticesToAdd; i++)
        {
            IVertex oVertex = aoVertices[i] = new Vertex();

            oVertex.Name = oVertex.ID.ToString();

            oVertexCollection.Add(oVertex);
        }

        return (aoVertices);
    }

    //*************************************************************************
    //  Method: MakeGraphComplete()
    //
    /// <summary>
    /// Adds an edge between each pair of vertices in a graph.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// Graph to add edges to.
    /// </param>
    ///
    /// <param name="aoVertices">
    /// Array of vertices in <paramref name="oGraph" />.
    /// </param>
    ///
    /// <param name="bDirected">
    /// true if the edges should be directed.
    /// </param>
    ///
    /// <returns>
    /// Array of added edges.
    /// </returns>
    //*************************************************************************

    public static IEdge []
    MakeGraphComplete
    (
        IGraph oGraph,
        IVertex [] aoVertices,
        Boolean bDirected
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(aoVertices != null);

        Int32 iVertices = aoVertices.Length;

        Int32 iEdges = GetEdgeCountForCompleteGraph(iVertices);

        IEdge[] aoAddedEdges = new IEdge[iEdges];

        IEdgeCollection oEdgeCollection = oGraph.Edges;

        Int32 iEdge = 0;

        for (Int32 i = 0; i < iVertices; i++)
        {
            for (Int32 j = i + 1; j < iVertices; j++)
            {
                IEdge oEdge = oEdgeCollection.Add(
                    aoVertices[i], aoVertices[j], bDirected);

                oEdge.Name = oEdge.ID.ToString();

                aoAddedEdges[iEdge] = oEdge;

                iEdge++;
            }
        }

        Debug.Assert(oEdgeCollection.Count == iEdges);

        return (aoAddedEdges);
    }

    //*************************************************************************
    //  Method: GetEdgeCountForCompleteGraph()
    //
    /// <summary>
    /// Returns the number of edges in a complete graph.
    /// </summary>
    ///
    /// <param name="iVertices">
    /// Number of vertices in the graph.
    /// </param>
    ///
    /// <returns>
    /// Number of edges in a complete graph with <paramref name="iVertices" />
    /// vertices.
    /// </returns>
    //*************************************************************************

    public static Int32
    GetEdgeCountForCompleteGraph
    (
        Int32 iVertices
    )
    {
        Debug.Assert(iVertices >= 0);

        // The number of edges in a complete graph is (1/2)v(v-1).

        if (iVertices == 0)
        {
            return (0);
        }

        return( ( iVertices * (iVertices - 1) ) / 2 );
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// <summary>
    /// Array of all possible Boolean values.
    /// </summary>

    public static Boolean [] AllBoolean = new Boolean []
    {
        true,
        false
    };

    /// <summary>
    /// Array of all possible <see cref="GraphDirectedness" /> values.
    /// </summary>

    public static GraphDirectedness [] AllGraphDirectedness =
        new GraphDirectedness []
    {
        GraphDirectedness.Directed,
        GraphDirectedness.Undirected,
        GraphDirectedness.Mixed
    };

    /// <summary>
    /// Array of all possible <see cref="GraphRestrictions" /> values.
    /// </summary>

    public static GraphRestrictions [] AllGraphRestrictions =
        new GraphRestrictions []
    {
        GraphRestrictions.None,
        GraphRestrictions.NoSelfLoops,
        GraphRestrictions.NoParallelEdges,
        GraphRestrictions.NoSelfLoops | GraphRestrictions.NoParallelEdges,
        GraphRestrictions.All
    };
}

}
