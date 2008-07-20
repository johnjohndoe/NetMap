
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.Algorithms
{
//*****************************************************************************
//  Class: KNeighborhoodGraphTransformer
//
/// <summary>
/// Transforms a graph into a new graph containing a subset of the original
/// graph's vertices.
/// </summary>
///
/// <remarks>
/// Given a graph X, a set of seed vertices V contained in X, and a distance K,
/// this class creates a new graph Y that contains only those vertices in X
///	that are either in V or within distance K of V.
///
/// <para>
///	TODO: Which edges are created in the new graph?
/// </para>
///
/// </remarks>
///
///	<example>
///	The following code transforms the existing graph oGraph into a new graph
///	using a set of seed vertices and a distance.
///
///	<code>
/// // Get the vertices to use as the seeds.
///
/// IVertexCollection oVertices = oGraph.Vertices;
/// IVertex oVertex1 = oVertices[0];
/// IVertex oVertex2 = oVertices[5];
/// IVertex [] aoSeedVertices = new IVertex [] {oVertex1, oVertex2};
///
/// Int32 iDistance = 12;
///
/// // Create the transformer object.
///
/// KNeighborhoodGraphTransformer oKNeighborhoodGraphTransformer
///		= new KNeighborhoodGraphTransformer(aoSeedVertices, iDistance);
///
/// // Use it to create a new graph that contains oVertex1, oVertex2, and those
/// // vertices that are within a distance 12 of oVertex1 or oVertex2.
///
/// Graph oNewGraph = oKNeighborhoodGraphTransformer.Transform(oGraph);
///	</code>
///
///	</example>
//*****************************************************************************

public class KNeighborhoodGraphTransformer : NetMapBase, IGraphTransformer
{
    //*************************************************************************
    //  Constructor: KNeighborhoodGraphTransformer()
    //
    /// <summary>
    /// Initializes a new instance of the KNeighborhoodGraphTransformer class.
    /// </summary>
	///
    /// <param name="seedVertices">
	///	Array of seed vertices to use when determining which vertices in the
	///	original graph should be copied to the new graph.
    /// </param>
    ///
    /// <param name="distance">
	///	Distance to use when determining which vertices in the original graph
	///	should be copied to the new graph.
    /// </param>
    //*************************************************************************

    public KNeighborhoodGraphTransformer
	(
		IVertex [] seedVertices,
		Int32 distance
	)
    {
		// TODO

		throw new NotImplementedException();
    }

    //*************************************************************************
    //  Property: SeedVertices
    //
    /// <summary>
    /// Gets or sets the array of seed vertices to use.
    /// </summary>
    ///
    /// <value>
	///	Array of seed vertices to use when determining which vertices in the
	///	original graph should be copied to the new graph, as an array of <see
	/// cref="IVertex" /> objects.
    /// </value>
    //*************************************************************************

    public IVertex []
	SeedVertices
    {
        get
        {
            AssertValid();

			// TODO

			throw new NotImplementedException();
        }

        set
        {
			// TODO

			throw new NotImplementedException();
        }
    }

    //*************************************************************************
    //  Property: Distance
    //
    /// <summary>
    /// Gets or sets the distance to use.
    /// </summary>
    ///
    /// <value>
	///	Distance to use when determining which vertices in the original graph
	///	should be copied to the new graph, as an <see cref="Int32" />.
    /// </value>
    //*************************************************************************

    public Int32
	Distance
    {
        get
        {
            AssertValid();

			// TODO

			throw new NotImplementedException();
        }

        set
        {
			// TODO

			throw new NotImplementedException();
        }
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
	///	vertex, and edge creators.
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
	///	This method transforms <paramref name="graph" /> into a new graph.
	///	<paramref name="graph" /> is not modified.
	///
	/// <para>
	///	To create a graph using default graph, vertex, and edge creators, use
	///	the other version of this method.
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
	///	vertex, and edge creators.
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
	///	This method transforms <paramref name="graph" /> into a new graph.
	///	<paramref name="graph" /> is not modified.
	///
	/// <para>
	///	The new graph is of type <see cref="Graph" />.  The new graph's
	///	vertices and edges are of type <see cref="Vertex" /> and <see
	///	cref="Edge" />.  To create a graph using specified graph, vertex, and
	///	edge creators, use the other version of this method.
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
