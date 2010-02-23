
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Algorithms;
using Microsoft.NodeXL.Visualization.Wpf;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: NodeXLControlUtil
//
/// <summary>
/// Utility methods for working with the <see cref="NodeXLControl" />.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class NodeXLControlUtil
{
    //*************************************************************************
    //  Method: GetSelectedEdgeIDs()
    //
    /// <summary>
    /// Gets the IDs of the selected edges.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
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
        NodeXLControl nodeXLControl
    )
    {
        Debug.Assert(nodeXLControl != null);

        List<Int32> oSelectedEdgeIDs = new List<Int32>();

        foreach (IEdge oSelectedEdge in nodeXLControl.SelectedEdges)
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
    //  Method: GetSelectedVertexIDsAsList()
    //
    /// <summary>
    /// Gets the IDs of the selected vertices as a list.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
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
        NodeXLControl nodeXLControl
    )
    {
        Debug.Assert(nodeXLControl != null);

        List<Int32> oSelectedVertexIDs = new List<Int32>();

        foreach (IVertex oSelectedVertex in nodeXLControl.SelectedVertices)
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
    /// <param name="nodeXLControl">
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
        NodeXLControl nodeXLControl
    )
    {
        Debug.Assert(nodeXLControl != null);

        Dictionary<IVertex, Char> oSelectedVertices =
            new Dictionary<IVertex, Char>();

        foreach (IVertex oSelectedVertex in nodeXLControl.SelectedVertices)
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
    /// <param name="nodeXLControl">
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
        NodeXLControl nodeXLControl
    )
    {
        Debug.Assert(nodeXLControl != null);

        Dictionary<Int32, IVertex> oSelectedVertices =
            new Dictionary<Int32, IVertex>();

        foreach (IVertex oSelectedVertex in nodeXLControl.SelectedVertices)
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
    /// <param name="nodeXLControl">
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
        NodeXLControl nodeXLControl
    )
    {
        Debug.Assert(nodeXLControl != null);

        Dictionary<IEdge, Char> oSelectedEdges = new Dictionary<IEdge, Char>();

        foreach (IEdge oSelectedEdge in nodeXLControl.SelectedEdges)
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
    /// <param name="nodeXLControl">
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
        NodeXLControl nodeXLControl
    )
    {
        Debug.Assert(nodeXLControl != null);

        Dictionary<Int32, IEdge> oSelectedEdges =
            new Dictionary<Int32, IEdge>();

        foreach (IEdge oSelectedEdge in nodeXLControl.SelectedEdges)
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
    /// <param name="nodeXLControl">
    /// Control to get the vertices from.
    /// </param>
    ///
    /// <returns>
    /// An array of vertices.  The array may be empty.
    /// </returns>
    //*************************************************************************

    public static IVertex []
    GetVerticesAsArray
    (
        NodeXLControl nodeXLControl
    )
    {
        Debug.Assert(nodeXLControl != null);

        // Note: This method is inefficient.  A future refactoring should
        // replace most IVertex[] parameters with ICollection<IVertex>,
        // eliminating the need to convert collections to arrays.

        IVertexCollection oVertices = nodeXLControl.Graph.Vertices;
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
    //  Method: GetVisibleVerticesAsArray()
    //
    /// <summary>
    /// Gets the collection of visible vertices as an array.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
    /// Control to get the vertices from.
    /// </param>
    ///
    /// <returns>
    /// An array of visible vertices.  The array may be empty.
    /// </returns>
    //*************************************************************************

    public static IVertex []
    GetVisibleVerticesAsArray
    (
        NodeXLControl nodeXLControl
    )
    {
        Debug.Assert(nodeXLControl != null);

        // Note: This method is inefficient.  A future refactoring should
        // replace most IVertex[] parameters with ICollection<IVertex>,
        // eliminating the need to convert collections to arrays.

        List<IVertex> oVisibleVertices = new List<IVertex>();

        foreach (IVertex oVertex in nodeXLControl.Graph.Vertices)
        {
            Object oVisibilityKeyValue;

            if (
                !oVertex.TryGetValue(ReservedMetadataKeys.Visibility,
                    typeof(VisibilityKeyValue), out oVisibilityKeyValue)
                ||
                (VisibilityKeyValue)oVisibilityKeyValue ==
                    VisibilityKeyValue.Visible
                )
            {
                oVisibleVertices.Add(oVertex);
            }
        }

        return ( oVisibleVertices.ToArray() );
    }

    //*************************************************************************
    //  Method: GetEdgesAsArray()
    //
    /// <summary>
    /// Gets the entire collection of edges as an array.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
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
        NodeXLControl nodeXLControl
    )
    {
        Debug.Assert(nodeXLControl != null);

        IEdgeCollection oEdges = nodeXLControl.Graph.Edges;
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
    //  Method: SelectSubgraphs()
    //
    /// <summary>
    /// Selects the subgraphs for one or more vertices.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
    /// The NodeXLControl whose subgraphs should be selected.
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
        NodeXLControl nodeXLControl,
        IVertex [] verticesToSelectSubgraphsFor,
        Decimal levels,
        Boolean selectConnectingEdges
    )
    {
        Debug.Assert(nodeXLControl != null);
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
            // These are similar dictionaries for the vertices and edges that
            // will be selected for this subgraph only.

            Dictionary<IVertex, Int32> oThisSubgraphSelectedVertices;
            Dictionary<IEdge, Char> oThisSubgraphSelectedEdges;

            SubgraphCalculator.GetSubgraph(oVertexToSelectSubgraphFor, levels,
                selectConnectingEdges, out oThisSubgraphSelectedVertices,
                out oThisSubgraphSelectedEdges);

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

        nodeXLControl.SetSelected(

            CollectionUtil.DictionaryKeysToArray<IVertex, Char>(
                oAllSelectedVertices),

            CollectionUtil.DictionaryKeysToArray<IEdge, Char>(
                oAllSelectedEdges)
            );
    }
}

}
