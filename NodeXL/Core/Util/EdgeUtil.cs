
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.Core
{
//*****************************************************************************
//  Class: EdgeUtil
//
/// <summary>
/// Utility methods for dealing with <see cref="IEdge" /> objects.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class EdgeUtil
{
    //*************************************************************************
    //  Method: EdgeToVertices()
    //
    /// <summary>
    /// Obtains an edge's two vertices.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge connecting the two vertices.  Can't be null.
    /// </param>
    ///
    /// <param name="className">
    /// Name of the class calling this method.
    /// </param>
    ///
    /// <param name="methodOrPropertyName">
    /// Name of the method or property calling this method.
    /// </param>
    ///
    /// <param name="vertex1">
    /// Where the edge's first vertex gets stored.
    /// </param>
    ///
    /// <param name="vertex2">
    /// Where the edge's second vertex gets stored.
    /// </param>
    ///
    /// <remarks>
    /// This method obtains an edge's two vertices and stores them at
    /// <paramref name="vertex1" /> and <paramref name="vertex2" />.  An
    /// <see cref="ApplicationException" /> is thrown if the vertices can't be
    /// obtained.
    /// </remarks>
    //*************************************************************************

    public static void
    EdgeToVertices
    (
        IEdge edge,
        String className,
        String methodOrPropertyName,
        out IVertex vertex1,
        out IVertex vertex2
    )
    {
        Debug.Assert(edge != null);
        Debug.Assert( !String.IsNullOrEmpty(className) );
        Debug.Assert( !String.IsNullOrEmpty(methodOrPropertyName) );

        String sErrorMessage = null;

        IVertex [] aoVertices = edge.Vertices;

        if (aoVertices == null)
        {
            sErrorMessage = "The edge's Vertices property is null.";
        }
        else if (aoVertices.Length != 2)
        {
            sErrorMessage = "The edge does not connect two vertices.";
        }
        else if (aoVertices[0] == null)
        {
            sErrorMessage = "The edge's first vertex is null.";
        }
        else if (aoVertices[1] == null)
        {
            sErrorMessage = "The edge's second vertex is null.";
        }
        else if (aoVertices[0].ParentGraph == null)
        {
            sErrorMessage =
                "The edge's first vertex does not belong to a graph.";
        }
        else if (aoVertices[1].ParentGraph == null)
        {
            sErrorMessage =
                "The edge's second vertex does not belong to a graph.";
        }
        else if ( aoVertices[0].ParentGraph != aoVertices[1].ParentGraph )
        {
            sErrorMessage =
                "The edge connects vertices not in the same graph.";
        }

        if (sErrorMessage != null)
        {
            Debug.Assert(false);

            throw new ApplicationException( String.Format(

                "{0}.{1}: {2}"
                ,
                className,
                methodOrPropertyName,
                sErrorMessage
                ) );
        }

        vertex1 = aoVertices[0];
        vertex2 = aoVertices[1];
    }

    //*************************************************************************
    //  Method: GetEdgeWeight()
    //
    /// <summary>
    /// Gets the edge weight between two vertices.
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
    /// The edge weight between the two vertices.
    /// </returns>
    ///
    /// <remarks>
    /// It's assumed that duplicate edges have been merged, and that the
    /// <see cref="ReservedMetadataKeys.EdgeWeight" /> key has been set on
    /// each of the graph's edges.
    /// </remarks>
    //*************************************************************************

    public static Double
    GetEdgeWeight
    (
        IVertex vertex1,
        IVertex vertex2
    )
    {
        Debug.Assert(vertex1 != null);
        Debug.Assert(vertex2 != null);

        Double dEdgeWeight = 0;

        // Get the edges that connect the two vertices.  This includes all
        // connecting edges and does not take directedness into account.

        IEdge [] aoConnectingEdges = vertex1.GetConnectingEdges(vertex2);
        Int32 iConnectingEdges = aoConnectingEdges.Length;
        IEdge oConnectingEdgeWithEdgeWeight = null;

        switch (vertex1.ParentGraph.Directedness)
        {
            case GraphDirectedness.Directed:

                // There can be 0, 1, or 2 edges between the vertices.  Only
                // one of them can originate at vertex1.

                Debug.Assert(iConnectingEdges <= 2);

                foreach (IEdge oConnectingEdge in aoConnectingEdges)
                {
                    if (oConnectingEdge.BackVertex == vertex1)
                    {
                        oConnectingEdgeWithEdgeWeight = oConnectingEdge;
                        break;
                    }
                }

                break;

            case GraphDirectedness.Undirected:

                // There can be 0 or 1 edges between the vertices.  There can't
                // be 2 edges, because the duplicate edges (A,B) and (B,A) have
                // been merged.

                Debug.Assert(iConnectingEdges <= 1);

                if (iConnectingEdges == 1)
                {
                    oConnectingEdgeWithEdgeWeight = aoConnectingEdges[0];
                }

                break;

            default:

                Debug.Assert(false);
                break;
        }

        if (oConnectingEdgeWithEdgeWeight != null)
        {
            dEdgeWeight = (Double)
                oConnectingEdgeWithEdgeWeight.GetRequiredValue(
                    ReservedMetadataKeys.EdgeWeight, typeof(Double) );
        }

        return (dEdgeWeight);
    }
}

}
