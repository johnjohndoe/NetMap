
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.Visualization
{
//*****************************************************************************
//  Class: VertexDrawerBase
//
/// <summary>
///	Base class for vertex drawers.
/// </summary>
///
/// <remarks>
/// This abstract class can serve as a base class for <see
/// cref="IVertexDrawer" /> implementations.  Its implementations of the <see
/// cref="IVertexDrawer" /> public methods provide error checking but defer the
/// actual work to protected abstract methods.
///
/// <para>
/// The visibility and hit-testing behavior of the vertex can be controlled
/// with the <see cref="ReservedMetadataKeys.Visibility" /> key.
/// </para>
///
/// </remarks>
//*****************************************************************************

public abstract class VertexDrawerBase : DrawerBase, IVertexDrawer
{
    //*************************************************************************
    //  Constructor: VertexDrawerBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexDrawerBase" />
	/// class.
    /// </summary>
    //*************************************************************************

    public VertexDrawerBase()
    {
		// (Do nothing.)
    }

    //*************************************************************************
    //  Method: PreDrawVertex()
    //
    /// <summary>
    /// Prepares to draw a vertex.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex that will eventually be drawn.
    /// </param>
    ///
    /// <param name="drawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
    ///
    /// <remarks>
	/// After a graph is laid out but before its edges or vertices are drawn,
	/// this method gets called repeatedly, once for each of the graph's
	/// vertices.  The implementation can use this method to perform any
	/// pre-drawing calculations it needs.  It can also change the <see
	/// cref="IVertex.Location" /> of <paramref name="vertex" /> if the layout
	/// has located the vertex in a place where it would get clipped by the
	/// graph rectangle if it weren't moved.
    /// </remarks>
	///
	/// <seealso cref="DrawVertex" />
    //*************************************************************************

    public void
    PreDrawVertex
    (
        IVertex vertex,
		DrawContext drawContext
    )
	{
		AssertValid();

		const String MethodName = "PreDrawVertex";

		CheckDrawOrPreDrawVertexArguments(vertex, drawContext, MethodName);

		PreDrawVertexCore(vertex, drawContext);
	}

    //*************************************************************************
    //  Method: DrawVertex()
    //
    /// <summary>
    /// Draws a vertex.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex to draw.
    /// </param>
    ///
    /// <param name="drawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
    ///
    /// <remarks>
    /// This method gets called repeatedly while a graph is being drawn, once
	/// for each of the graph's vertices.  The <see cref="IVertex.Location" />
	///	property on all of the graph's vertices is set by ILayout.<see
	///	cref="ILayout.LayOutGraph" /> before this method is called.
    /// </remarks>
	///
	/// <seealso cref="PreDrawVertex" />
    //*************************************************************************

    public void
    DrawVertex
    (
        IVertex vertex,
		DrawContext drawContext
    )
	{
		AssertValid();

		const String MethodName = "DrawVertex";

		CheckDrawOrPreDrawVertexArguments(vertex, drawContext, MethodName);

		// If the vertex isn't hidden, draw it.

		if (GetVisibility(vertex) != VisibilityKeyValue.Hidden)
		{
			DrawVertexCore(vertex, drawContext);
		}
	}

    //*************************************************************************
    //  Method: VertexContainsPoint()
    //
    /// <summary>
    /// Determines whether a vertex contains a point.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex to check.
    /// </param>
    ///
    /// <param name="point">
	///	The point to check.
    /// </param>
    ///
    /// <returns>
	///	true if <paramref name="vertex" /> contains <paramref name="point" />.
    /// </returns>
	///
    /// <remarks>
	/// Because the vertex drawer knows the shape and size of a vertex, it's
	///	the vertex drawer's responsibility to determine whether a vertex
	///	contains a point.
	///
	/// <para>
	/// The <see cref="IVertex.Location" /> property of <paramref
	///	name="vertex" /> must be set before this method is called.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    public Boolean
    VertexContainsPoint
    (
        IVertex vertex,
		Point point
    )
	{
		AssertValid();

		const String MethodName = "VertexContainsPoint";

		this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "vertex", vertex);

		return ( VertexContainsPointCore(vertex, point) );
	}

    //*************************************************************************
    //  Method: VertexIntersectsWithRectangle()
    //
    /// <summary>
    /// Determines whether a vertex intersects a specified rectangle.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex to check.
    /// </param>
	///
    /// <param name="rectangle">
    /// The rectangle to check.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="vertex" /> intersects <paramref
	/// name="rectangle" />.
    /// </returns>
	///
    /// <remarks>
	/// Because the vertex drawer knows the shape and size of a vertex, it's
	///	the vertex drawer's responsibility to determine whether a vertex
	///	intersects a rectangle.
	///
	/// <para>
	/// The <see cref="IVertex.Location" /> property of <paramref
	///	name="vertex" /> must be set before this method is called.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    public Boolean
    VertexIntersectsWithRectangle
    (
        IVertex vertex,
		Rectangle rectangle
    )
	{
		AssertValid();

		const String MethodName = "VertexIntersectsWithRectangle";

		this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "vertex", vertex);

		return ( VertexIntersectsWithRectangleCore(vertex, rectangle) );
	}

    //*************************************************************************
    //  Method: DrawVertexCore()
    //
    /// <summary>
    /// Draws a vertex.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex to draw.
    /// </param>
    ///
    /// <param name="drawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
    ///
    /// <returns>
	/// The vertex's bounding rectangle.
    /// </returns>
    ///
    /// <remarks>
    /// This method gets called repeatedly while a graph is being drawn, once
	/// for each of the graph's vertices.  The <see cref="IVertex.Location" />
	///	property on all of the graph's vertices is set by ILayout.<see
	///	cref="ILayout.LayOutGraph" /> before this method is called.
	///
	/// <para>
	/// The arguments have already been checked for validity.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    protected abstract Rectangle
    DrawVertexCore
    (
        IVertex vertex,
		DrawContext drawContext
    );

    //*************************************************************************
    //  Method: PreDrawVertexCore()
    //
    /// <summary>
    /// Prepares to draw a vertex.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex that will eventually be drawn.
    /// </param>
    ///
    /// <param name="drawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
    ///
    /// <remarks>
	/// After a graph is laid out but before its edges or vertices are drawn,
	/// this method gets called repeatedly, once for each of the graph's
	/// vertices.  The implementation can use this method to perform any
	/// pre-drawing calculations it needs.  It can also change the <see
	/// cref="IVertex.Location" /> of <paramref name="vertex" /> if the layout
	/// has located the vertex in a place where it would get clipped by the
	/// graph rectangle if it weren't moved.
	///
	/// <para>
	/// The arguments have already been checked for validity.
	/// </para>
	///
    /// </remarks>
	///
	/// <seealso cref="DrawVertex" />
    //*************************************************************************

    protected virtual void
    PreDrawVertexCore
    (
        IVertex vertex,
		DrawContext drawContext
    )
	{
		Debug.Assert(vertex != null);
		Debug.Assert(drawContext != null);
		AssertValid();

		// (Do nothing.)
	}

    //*************************************************************************
    //  Method: VertexContainsPointCore()
    //
    /// <summary>
    /// Determines whether a vertex contains a point.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex to check.
    /// </param>
    ///
    /// <param name="point">
	///	The point to check.
    /// </param>
    ///
    /// <returns>
	///	true if <paramref name="vertex" /> contains <paramref name="point" />.
    /// </returns>
	///
    /// <remarks>
	/// Because the vertex drawer knows the shape and size of a vertex, it's
	///	the vertex drawer's responsibility to determine whether a vertex
	///	contains a point.
	///
	/// <para>
	/// The <see cref="IVertex.Location" /> property of <paramref
	///	name="vertex" /> must be set before this method is called.
	/// </para>
	///
	/// <para>
	/// The arguments have already been checked for validity.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    protected abstract Boolean
    VertexContainsPointCore
    (
        IVertex vertex,
		Point point
    );

    //*************************************************************************
    //  Method: VertexIntersectsWithRectangleCore()
    //
    /// <summary>
    /// Determines whether a vertex intersects a specified rectangle.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex to check.
    /// </param>
	///
    /// <param name="rectangle">
    /// The rectangle to check.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="vertex" /> intersects <paramref
	/// name="rectangle" />.
    /// </returns>
	///
    /// <remarks>
	/// Because the vertex drawer knows the shape and size of a vertex, it's
	///	the vertex drawer's responsibility to determine whether a vertex
	///	intersects a rectangle.
	///
	/// <para>
	/// The <see cref="IVertex.Location" /> property of <paramref
	///	name="vertex" /> must be set before this method is called.
	/// </para>
	///
	/// <para>
	/// The arguments have already been checked for validity.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    protected abstract Boolean
    VertexIntersectsWithRectangleCore
    (
        IVertex vertex,
		Rectangle rectangle
    );

    //*************************************************************************
    //  Method: CheckDrawOrPreDrawVertexArguments()
    //
    /// <summary>
    /// Checks the arguments to <see cref="DrawVertex" /> or <see
	/// cref="PreDrawVertex" />.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex that will eventually be drawn.
    /// </param>
    ///
    /// <param name="oDrawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
    ///
    /// <param name="sMethodName">
	/// Name of the method calling this method.
    /// </param>
    ///
    /// <remarks>
	/// An exception is thrown if one of the arguments is invalid.
	/// </remarks>
    //*************************************************************************

    protected void
    CheckDrawOrPreDrawVertexArguments
    (
        IVertex oVertex,
		DrawContext oDrawContext,
		String sMethodName
    )
	{
		Debug.Assert( !String.IsNullOrEmpty(sMethodName) );
		AssertValid();

		const String VertexArgumentName = "vertex";

		ArgumentChecker oArgumentChecker = this.ArgumentChecker;

		oArgumentChecker.CheckArgumentNotNull(sMethodName, VertexArgumentName,
			oVertex);

		oArgumentChecker.CheckArgumentNotNull(sMethodName, "drawContext",
			oDrawContext);

		Graphics oGraphics = oDrawContext.Graphics;

		oArgumentChecker.CheckArgumentNotNull(sMethodName,
			"drawContext.Graphics", oGraphics);

		if (oVertex.ParentGraph == null)
		{
            oArgumentChecker.ThrowArgumentException(
				sMethodName, VertexArgumentName,
				"The vertex doesn't belong to a graph.  It can't be drawn."
				);
		}
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
