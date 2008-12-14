
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.Core
{
//*****************************************************************************
//  Class: EdgeFactoryBase
//
/// <summary>
///	Base class for edge factories.
/// </summary>
///
/// <remarks>
/// This abstract class can serve as a base class for <see
/// cref="IEdgeFactory" /> implementations.  Its implementations of the <see
/// cref="IEdgeFactory" /> public methods provide error checking but defer the
/// actual work to protected abstract methods.
/// </remarks>
///
/// <seealso cref="Edge" />
//*****************************************************************************

public abstract class EdgeFactoryBase : GraphVertexEdgeFactoryBase, IEdgeFactory
{
    //*************************************************************************
    //  Constructor: EdgeFactoryBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EdgeFactoryBase" /> class.
    /// </summary>
    //*************************************************************************

    public EdgeFactoryBase()
    {
		// (Do nothing.)

		AssertValid();
    }

    //*************************************************************************
    //  Method: CreateEdge()
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
	/// <seealso cref="CreateUndirectedEdge" />
	/// <seealso cref="CreateDirectedEdge" />
    //*************************************************************************

	public IEdge
	CreateEdge
	(
		IVertex vertex1,
		IVertex vertex2,
		Boolean isDirected
	)
	{
		AssertValid();

		const String MethodName = "CreateEdge";

		ArgumentChecker oArgumentChecker = this.ArgumentChecker;

		oArgumentChecker.CheckArgumentNotNull(MethodName, "vertex1", vertex1);
		oArgumentChecker.CheckArgumentNotNull(MethodName, "vertex2", vertex2);

		return ( CreateEdgeCore(vertex1, vertex2, isDirected) );
	}

    //*************************************************************************
    //  Method: CreateUndirectedEdge()
    //
    /// <summary>
    /// Creates an undirected edge object.
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
    /// <returns>
	///	The <see cref="IEdge" /> interface on a newly created edge object.
    /// </returns>
	///
	/// <seealso cref="CreateDirectedEdge" />
	/// <seealso cref="CreateEdge" />
    //*************************************************************************

	public IEdge
	CreateUndirectedEdge
	(
		IVertex vertex1,
		IVertex vertex2
	)
	{
		AssertValid();

		const String MethodName = "CreateUndirectedEdge";

		ArgumentChecker oArgumentChecker = this.ArgumentChecker;

		oArgumentChecker.CheckArgumentNotNull(MethodName, "vertex1", vertex1);
		oArgumentChecker.CheckArgumentNotNull(MethodName, "vertex2", vertex2);

		return ( CreateEdgeCore(vertex1, vertex2, false) );
	}

    //*************************************************************************
    //  Method: CreateDirectedEdge()
    //
    /// <summary>
    /// Creates a directed edge object.
    /// </summary>
	///
    /// <param name="backVertex">
	///	The edge's back vertex.  The vertex must have already been added to
	/// the graph to which the new edge will be added.
    /// </param>
    ///
    /// <param name="frontVertex">
	///	The edge's front vertex.  The vertex must have already been added to
	/// the graph to which the new edge will be added.
    /// </param>
    ///
    /// <returns>
	///	The <see cref="IEdge" /> interface on a newly created edge object.
    /// </returns>
	///
	/// <seealso cref="CreateUndirectedEdge" />
	/// <seealso cref="CreateEdge" />
    //*************************************************************************

	public IEdge
	CreateDirectedEdge
	(
		IVertex backVertex,
		IVertex frontVertex
	)
	{
		AssertValid();

		const String MethodName = "CreateDirectedEdge";

		ArgumentChecker oArgumentChecker = this.ArgumentChecker;

		oArgumentChecker.CheckArgumentNotNull(
			MethodName, "backVertex", backVertex);

		oArgumentChecker.CheckArgumentNotNull(
			MethodName, "frontVertex", frontVertex);

		return ( CreateEdgeCore(backVertex, frontVertex, true) );
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

	protected abstract IEdge
	CreateEdgeCore
	(
		IVertex vertex1,
		IVertex vertex2,
		Boolean isDirected
	);


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
