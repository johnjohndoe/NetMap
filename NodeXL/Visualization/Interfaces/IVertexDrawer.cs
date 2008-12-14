
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Visualization
{
//*****************************************************************************
//  Interface: IVertexDrawer
//
/// <summary>
/// Supports vertex drawing.
/// </summary>
///
/// <remarks>
/// A class that implements this interface is responsible for drawing the
///	vertices of a graph.
///
/// <para>
/// An implementation is typically meant to be used with a corresponding
/// implementation of the <see cref="IEdgeDrawer" /> interface, although this
/// isn't a requirement.  A vertex drawer can determine the type of the edge
/// drawer in use by looking at the <see cref="DrawContext.GraphDrawer" />
/// property on the <see cref="DrawContext" /> object passed to <see
/// cref="DrawVertex" />.  If the edge drawer is not of the expected
/// corresponding type, the vertex drawer should revert to some sensible, if
/// not ideal, default behavior.
/// </para>
///
/// </remarks>
//*****************************************************************************

public interface IVertexDrawer
{
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

    void
    PreDrawVertex
    (
        IVertex vertex,
		DrawContext drawContext
    );


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

    void
    DrawVertex
    (
        IVertex vertex,
		DrawContext drawContext
    );


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

    Boolean
    VertexContainsPoint
    (
        IVertex vertex,
		Point point
    );


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

    Boolean
    VertexIntersectsWithRectangle
    (
        IVertex vertex,
		Rectangle rectangle
    );


	//*************************************************************************
	//	Event: RedrawRequired
	//
	/// <summary>
	///	Occurs when a change occurs that requires a graph redraw.
	/// </summary>
	///
	/// <remarks>
	///	The implementation must fire this event when a change is made to the
	/// object that might affect the appearance of the graph.
	///
	/// <para>
	/// The object owner should handle the event by redrawing the graph.  The
	/// graph does not need to be laid out again.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	event EventHandler RedrawRequired;


	//*************************************************************************
	//	Event: LayoutRequired
	//
	/// <summary>
	///	Occurs when a change occurs that requires the graph to be laid out
	/// again.
	/// </summary>
	///
	/// <remarks>
	///	The implementation must fire this event when a change is made to the
	/// object that might affect the layout of the graph.
	///
	/// <para>
	///	The object owner should lay out the graph and redraw it in response to
	/// the event.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	event EventHandler LayoutRequired;
}

}
