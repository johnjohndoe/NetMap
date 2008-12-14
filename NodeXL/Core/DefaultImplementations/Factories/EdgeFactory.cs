
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.Core
{
//*****************************************************************************
//  Class: EdgeFactory
//
/// <summary>
/// Class that knows how to create <see cref="Edge" /> objects.
/// </summary>
///
/// <remarks>
///	This class implements <see cref="IEdgeFactory" />, which allows the core
/// NodeXL system to create edge objects without knowing their type.
/// </remarks>
///
/// <seealso cref="Edge" />
//*****************************************************************************

public class EdgeFactory : EdgeFactoryBase, IEdgeFactory
{
    //*************************************************************************
    //  Constructor: EdgeFactory()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EdgeFactory" /> class.
    /// </summary>
    //*************************************************************************

    public EdgeFactory()
    {
		// (Do nothing.)

		AssertValid();
    }

    //*************************************************************************
    //  Method: CreateEdgeCore()
    //
    /// <summary>
    /// Creates an edge object with a specified directedness.
    /// </summary>
	///
    /// <param name="vertex1">
	///	The edge's first vertex.  The vertex must have already been added to
	/// the graph to which the new edge will be added.
    /// </param>
    ///
    /// <param name="vertex2">
	///	The edge's second vertex.  The vertex must have already been added to
	/// the graph to which the new edge will be added.
    /// </param>
    ///
    /// <param name="isDirected">
	///	If true, <paramref name="vertex1" /> is the edge's back vertex and
	///	<paramref name="vertex2" /> is the edge's front vertex.  If false, the
	/// edge is undirected.
    /// </param>
    ///
    /// <returns>
	///	The <see cref="IEdge" /> interface on a newly created edge object.
    /// </returns>
	///
	/// <remarks>
	/// The arguments have already been checked for validity.
	/// </remarks>
    //*************************************************************************

	protected override IEdge
	CreateEdgeCore
	(
		IVertex vertex1,
		IVertex vertex2,
		Boolean isDirected
	)
	{
		Debug.Assert(vertex1 != null);
		Debug.Assert(vertex2 != null);
		AssertValid();

		return ( new Edge(vertex1, vertex2, isDirected) );
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

		// (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
