
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Diagnostics;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.Visualization
{
//*****************************************************************************
//  Interface: IEdgeDrawer
//
/// <summary>
/// Supports edge drawing.
/// </summary>
///
/// <remarks>
/// A class that implements this interface is responsible for drawing the edges
///	of a graph.
///
/// <para>
/// An implementation is typically meant to be used with a corresponding
/// implementation of the <see cref="IVertexDrawer" /> interface, although this
/// isn't a requirement.  An edge drawer can determine the type of the vertex
/// drawer in use by looking at the <see cref="DrawContext.GraphDrawer" />
/// property on the <see cref="DrawContext" /> object passed to <see
/// cref="DrawEdge" />.  If the vertex drawer is not of the expected
/// corresponding type, the edge drawer should revert to some sensible, if not
/// ideal, default behavior.
/// </para>
///
/// </remarks>
//*****************************************************************************

public interface IEdgeDrawer
{
    //*************************************************************************
    //  Method: DrawEdge()
    //
    /// <summary>
    /// Draws an edge.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge to draw.
    /// </param>
    ///
    /// <param name="drawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
    ///
    /// <remarks>
    /// This method gets called repeatedly while a graph is being drawn, once
	/// for each of the graph's edges.  The <see cref="IVertex.Location" />
	///	property on all of the graph's vertices is set by ILayout.<see
	///	cref="ILayout.LayOutGraph" /> before this method is called.
    /// </remarks>
    //*************************************************************************

    void
    DrawEdge
    (
        IEdge edge,
		DrawContext drawContext
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
