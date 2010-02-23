
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: ConnectedComponentCalculator
//
/// <summary>
/// Gets the strongly connected components for a specified graph.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class ConnectedComponentCalculator
{
    //*************************************************************************
    //  Method: GetStronglyConnectedComponents()
    //
    /// <overloads>
    /// Gets a set of strongly connected components.
    /// </overloads>
    ///
    /// <summary>
    /// Gets the strongly connected components for a specified graph.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to get the strongly connected components for.
    /// </param>
    ///
    /// <returns>
    /// A List of LinkedLists of vertices.  Each LinkedList of vertices
    /// comprises a strongly connected component of the graph.  The components
    /// are sorted by their vertex count, in increasing order.
    /// </returns>
    ///
    /// <remarks>
    /// This method uses Tarjan's strongly connected components algorithm,
    /// outlined here:
    ///
    /// <para>
    /// http://en.wikipedia.org/wiki/
    /// Tarjan%27s_strongly_connected_components_algorithm
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static List< LinkedList<IVertex> >
    GetStronglyConnectedComponents
    (
        IGraph graph
    )
    {
        Debug.Assert(graph != null);

        return ( GetStronglyConnectedComponents(graph.Vertices) );
    }

    //*************************************************************************
    //  Method: GetStronglyConnectedComponents()
    //
    /// <summary>
    /// Gets the strongly connected components for a specified set of vertices.
    /// </summary>
    ///
    /// <param name="vertices">
    /// Vertices to get the strongly connected components for.
    /// </param>
    ///
    /// <returns>
    /// A List of LinkedLists of vertices.  Each LinkedList of vertices
    /// comprises a strongly connected component of the graph.  The components
    /// are sorted by their vertex count, in increasing order.
    /// </returns>
    ///
    /// <remarks>
    /// This method uses Tarjan's strongly connected components algorithm,
    /// outlined here:
    ///
    /// <para>
    /// http://en.wikipedia.org/wiki/
    /// Tarjan%27s_strongly_connected_components_algorithm
    /// </para>
    ///
    /// <para>
    /// If a vertex specified in <paramref name="vertices" /> is strongly
    /// connected to one or more vertices, those vertices are included in the
    /// returned List even if they are not included in the <paramref
    /// name="vertices" /> collection.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static List< LinkedList<IVertex> >
    GetStronglyConnectedComponents
    (
        ICollection<IVertex> vertices
    )
    {
        Debug.Assert(vertices != null);

        Int32 iNextIndex = 0;
        Stack<IVertex> oStack = new Stack<IVertex>();

        // Note: A List is used instead of a LinkedList for the strongly
        // connected components only because LinkedList does not have a Sort()
        // method.

        List< LinkedList<IVertex> > oStronglyConnectedComponents =
            new List< LinkedList<IVertex> >();

        foreach (IVertex oVertex in vertices)
        {
            if ( !oVertex.ContainsKey(
                ReservedMetadataKeys.ConnectedComponentCalculatorIndex) )
            {
                RunTarjanAlgorithm(oVertex, oStack,
                    oStronglyConnectedComponents, ref iNextIndex);
            }
        }

        // Sort the components by increasing vertex count.

        oStronglyConnectedComponents.Sort(
            delegate
            (
                LinkedList<IVertex> oStronglyConnectedComponent1,
                LinkedList<IVertex> oStronglyConnectedComponent2
            )
            {
                return (oStronglyConnectedComponent1.Count.CompareTo(
                    oStronglyConnectedComponent2.Count)
                    );
            }
            );

        // Remove the metadata that was added to each vertex.

        foreach (IVertex oVertex in vertices)
        {
            oVertex.RemoveKey(
                ReservedMetadataKeys.ConnectedComponentCalculatorIndex);

            oVertex.RemoveKey(
                ReservedMetadataKeys.ConnectedComponentCalculatorLowLink);
        }

        return (oStronglyConnectedComponents);
    }

    //*************************************************************************
    //  Method: RunTarjanAlgorithm()
    //
    /// <summary>
    /// Runs the Tarjan algorithm on one vertex.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to run the algorithm on.
    /// </param>
    ///
    /// <param name="oStack">
    /// Stack used by the algorithm.
    /// </param>
    ///
    /// <param name="oStronglyConnectedComponents">
    /// List to add the strongly connected components to.
    /// </param>
    ///
    /// <param name="iNextIndex">
    /// Index to assign to <paramref name="oVertex" />.  Gets incremented.
    /// </param>
    //*************************************************************************

    private static void
    RunTarjanAlgorithm
    (
        IVertex oVertex,
        Stack<IVertex> oStack,
        List< LinkedList<IVertex> > oStronglyConnectedComponents,
        ref Int32 iNextIndex
    )
    {
        Debug.Assert(oVertex != null);

        Debug.Assert( !oVertex.ContainsKey(
            ReservedMetadataKeys.ConnectedComponentCalculatorIndex) );

        Debug.Assert(oStack != null);
        Debug.Assert(oStronglyConnectedComponents != null);
        Debug.Assert(iNextIndex >= 0);

        SetIndex(oVertex, iNextIndex);
        SetLowLink(oVertex, iNextIndex);
        iNextIndex++;

        oStack.Push(oVertex);

        foreach (IEdge oIncidentEdge in oVertex.IncidentEdges)
        {
            IVertex oAdjacentVertex = oIncidentEdge.GetAdjacentVertex(oVertex);

            if ( !oAdjacentVertex.ContainsKey(
                ReservedMetadataKeys.ConnectedComponentCalculatorIndex) )
            {
                RunTarjanAlgorithm(oAdjacentVertex, oStack,
                    oStronglyConnectedComponents, ref iNextIndex);

                SetLowLink(oVertex,
                    Math.Min(GetLowLink(oVertex), GetLowLink(oAdjacentVertex) )
                    );
            }
            else if ( oStack.Contains(oAdjacentVertex) )
            {
                SetLowLink(oVertex,
                    Math.Min( GetLowLink(oVertex), GetIndex(oAdjacentVertex) )
                    );
            }
        }

        if ( GetLowLink(oVertex) == GetIndex(oVertex) )
        {
            LinkedList<IVertex> oStronglyConnectedComponent =
                new LinkedList<IVertex>();

            IVertex oVertexInComponent;

            do
            {
                oVertexInComponent = oStack.Pop();
                oStronglyConnectedComponent.AddLast(oVertexInComponent);

            } while (oVertexInComponent.ID != oVertex.ID);

            oStronglyConnectedComponents.Add(oStronglyConnectedComponent);
        }
    }

    //*************************************************************************
    //  Method: SetIndex()
    //
    /// <summary>
    /// Sets the Index value for a vertex.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to set the Index value for.
    /// </param>
    ///
    /// <param name="iIndex">
    /// The Index value.
    /// </param>
    //*************************************************************************

    private static void
    SetIndex
    (
        IVertex oVertex,
        Int32 iIndex
    )
    {
        Debug.Assert(oVertex != null);

        oVertex.SetValue(
            ReservedMetadataKeys.ConnectedComponentCalculatorIndex,
            iIndex);
    }

    //*************************************************************************
    //  Method: GetIndex()
    //
    /// <summary>
    /// Gets the Index value for a vertex.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to get the Index value for.  The vertex must contain the
    /// value; an exception is thrown if the value doesn't exist in the
    /// vertex's metadata.
    /// </param>
    ///
    /// <returns>
    /// The vertex's Index value.
    /// </returns>
    //*************************************************************************

    private static Int32
    GetIndex
    (
        IVertex oVertex
    )
    {
        Debug.Assert(oVertex != null);

        return ( (Int32)oVertex.GetRequiredValue(
            ReservedMetadataKeys.ConnectedComponentCalculatorIndex,
            typeof(Int32) ) );
    }

    //*************************************************************************
    //  Method: SetLowLink()
    //
    /// <summary>
    /// Sets the LowLink value for a vertex.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to set the LowLink value for.
    /// </param>
    ///
    /// <param name="iLowLink">
    /// The LowLink value.
    /// </param>
    //*************************************************************************

    private static void
    SetLowLink
    (
        IVertex oVertex,
        Int32 iLowLink
    )
    {
        Debug.Assert(oVertex != null);

        oVertex.SetValue(
            ReservedMetadataKeys.ConnectedComponentCalculatorLowLink,
            iLowLink);
    }

    //*************************************************************************
    //  Method: GetLowLink()
    //
    /// <summary>
    /// Gets the LowLink value for a vertex.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to get the LowLink value for.  The vertex must contain the
    /// value; an exception is thrown if the value doesn't exist in the
    /// vertex's metadata.
    /// </param>
    ///
    /// <returns>
    /// The vertex's LowLink value.
    /// </returns>
    //*************************************************************************

    private static Int32
    GetLowLink
    (
        IVertex oVertex
    )
    {
        Debug.Assert(oVertex != null);

        return ( (Int32)oVertex.GetRequiredValue(
            ReservedMetadataKeys.ConnectedComponentCalculatorLowLink,
            typeof(Int32) ) );
    }
}

}
