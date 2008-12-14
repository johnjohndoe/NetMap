
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: GraphFilter
//
/// <summary>
/// Transforms a graph by filtering its vertices or edges.
/// </summary>
//*****************************************************************************

public class GraphFilter : NodeXLBase, IGraphTransformer
{
    //*************************************************************************
    //  Constructor: GraphFilter()
    //
    /// <overloads>
    /// Initializes a new instance of the GraphFilter class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the GraphFilter class that filters a
    /// graph by its vertices.
    /// </summary>
    ///
    /// <param name="vertexDelegate">
    /// Method to call for each vertex in the graph being filtered.
    /// </param>
    ///
    /// <remarks>
    /// The <see cref="Transform(IGraph)" /> method calls <paramref
	///	name="vertexDelegate" /> for each vertex in the graph being filtered.
    /// The new graph contains each vertex for which <paramref
    /// name="vertexDelegate" /> returns true, and all edges that connect those
    /// vertices.
    /// </remarks>
    ///
    /// <example>
    /// The following code filters the existing graph oGraph using the delegate
    /// FilterVertex().
    ///
    /// <code>
    /// public Boolean
    /// FilterVertex
    /// (
    ///     IVertex vertex
    /// )
    /// {
    ///     if ( [some vertex test] )
    ///     {
    ///         // Copy this vertex to the new graph.
    ///
    ///         return (true);
    ///     }
    ///
    ///     // Do not copy this vertex to the new graph.
    ///
    ///     return (false);
    /// }
    ///
    /// public void
    /// FilterGraph()
    /// {
    ///     GraphFilter oGraphFilter =
    ///         new GraphFilter( new VertexDelegate(FilterVertex) );
    ///
    ///     // Create a new graph that contains those vertices for which
    ///     // FilterVertex() returns true.
    ///
    ///     Graph oNewGraph = oGraphFilter.Transform(oGraph);
    /// }
    ///
    /// </code>
    ///
    /// </example>
    //*************************************************************************

    public GraphFilter
    (
        VertexDelegate vertexDelegate
    )
    {
		// TODO

		throw new NotImplementedException();
    }

    //*************************************************************************
    //  Constructor: GraphFilter()
    //
    /// <summary>
    /// Initializes a new instance of the GraphFilter class that filters a
    /// graph by its edges.
    /// </summary>
    ///
    /// <param name="edgeDelegate">
    /// Method to call for each edge in the graph being filtered.
    /// </param>
    ///
    /// <remarks>
    /// The <see cref="Transform(IGraph)" /> method calls <paramref
	///	name="edgeDelegate" /> for each edge in the graph being filtered.
    /// The new graph contains all vertices, along with each edge for which
	///	<paramref name="edgeDelegate" /> returns true.
    /// </remarks>
    ///
    /// <example>
    /// The following code filters the existing graph oGraph using the delegate
    /// FilterEdge().
    ///
    /// <code>
    /// public Boolean
    /// FilterEdge
    /// (
    ///     IEdge edge
    /// )
    /// {
    ///     if ( [some edge test] )
    ///     {
    ///         // Copy this edge to the new graph.
    ///
    ///         return (true);
    ///     }
    ///
    ///     // Do not copy this edge to the new graph.
    ///
    ///     return (false);
    /// }
    ///
    /// public void
    /// FilterGraph()
    /// {
    ///     GraphFilter oGraphFilter =
    ///         new GraphFilter( new EdgeDelegate(FilterEdge) );
	///
	///     // Create a new graph that contains those edges for which
	///		// FilterEdge() returns true.
    ///
    ///     Graph oNewGraph = oGraphFilter.Transform(oGraph);
    /// }
	///
    /// </code>
    ///
    /// </example>
    //*************************************************************************

    public GraphFilter
    (
        EdgeDelegate edgeDelegate
    )
    {
		// TODO

		throw new NotImplementedException();
    }

    //*************************************************************************
    //  Method: Transform()
    //
    /// <overloads>
    /// Transforms an existing graph into a new graph.
    /// </overloads>
    ///
    /// <summary>
    /// Transforms an existing graph into a new graph using specified graph,
    /// vertex, and edge creators.
    /// </summary>
    ///
    /// <param name="graph">
    /// Existing graph to transform.
    /// </param>
    ///
    /// <param name="newGraphFactory">
    /// Object that can create a graph.
    /// </param>
    ///
    /// <param name="newVertexFactory">
    /// Object that can create vertices.
    /// </param>
    ///
    /// <param name="newEdgeFactory">
    /// Object that can create edges.
    /// </param>
    ///
    /// <returns>
    /// The new graph, as an <see cref="IGraph" />.  The graph is created using
    /// <paramref name="newGraphFactory" />.
    /// </returns>
    ///
    /// <remarks>
    /// This method transforms <paramref name="graph" /> into a new graph.
    /// <paramref name="graph" /> is not modified.
    ///
    /// <para>
    /// To create a graph using default graph, vertex, and edge creators, use
    /// the other version of this method.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public IGraph
    Transform
    (
        IGraph graph,
        IGraphFactory newGraphFactory,
        IVertexFactory newVertexFactory,
        IEdgeFactory newEdgeFactory
    )
    {
        AssertValid();

		// TODO

		throw new NotImplementedException();
    }

    //*************************************************************************
    //  Method: Transform()
    //
    /// <summary>
    /// Transforms an existing graph into a new graph using default graph,
    /// vertex, and edge creators.
    /// </summary>
    ///
    /// <param name="graph">
    /// Existing graph to transform.
    /// </param>
    ///
    /// <returns>
    /// The new <see cref="Graph" /> object.
    /// </returns>
    ///
    /// <remarks>
    /// This method transforms <paramref name="graph" /> into a new graph.
    /// <paramref name="graph" /> is not modified.
    ///
    /// <para>
    /// The new graph is of type <see cref="Graph" />.  The new graph's
    /// vertices and edges are of type <see cref="Vertex" /> and <see
    /// cref="Edge" />.  To create a graph using specified graph, vertex, and
    /// edge creators, use the other version of this method.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Graph
    Transform
    (
        IGraph graph
    )
    {
        AssertValid();

		// TODO

		throw new NotImplementedException();
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

        // TODO
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // TODO
}

}
