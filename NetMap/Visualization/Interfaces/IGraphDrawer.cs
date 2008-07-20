
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Diagnostics;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.Visualization
{
//*****************************************************************************
//  Interface: IGraphDrawer
//
/// <summary>
/// Supports graph drawing.
/// </summary>
///
/// <remarks>
/// A class that implements this interface is responsible for drawing a graph.
///
/// <para>
///	The implementation will typically draw a graph by laying it out via the
///	<see cref="ILayout.LayOutGraph" /> method on the <see cref="Layout" />
/// object, drawing the edges via repeated calls to the <see
/// cref="IEdgeDrawer.DrawEdge" /> method on the <see cref="EdgeDrawer" />
/// object, and drawing the vertices via repeated calls to the <see
/// cref="IVertexDrawer.DrawVertex" /> method on the <see
/// cref="VertexDrawer" /> object.  The order is important: The edges and
/// vertices can't be drawn before <see cref="ILayout.LayOutGraph" /> sets the
/// <see cref="IVertex.Location" /> property on the vertices, and the vertices
/// are drawn after the edges so that the vertices cover the ends of the edges.
/// </para>
///
/// <para>
/// The interface does not define any draw methods, because the arguments
/// passed to such methods vary with the implementation.  The implementation
/// might draw on a <see cref="Bitmap" /> or a <see cref="Graphics" /> object,
/// for example.  It's conceivable that the graph drawer might not even draw
/// directly on a visible surface; an implementation might "draw" to a VML
/// file, for example.
/// </para>
///
/// <para>
/// If your graph drawer uses an asynchronous layout, you should consider
/// implementing the <see cref="IAsyncGraphDrawer" /> interface instead.
/// </para>
///
/// </remarks>
//*****************************************************************************

public interface IGraphDrawer
{
    //*************************************************************************
    //  Property: Graph
    //
    /// <summary>
    /// Gets or sets the graph to draw.
    /// </summary>
    ///
    /// <value>
    /// The graph to draw, as an <see cref="IGraph" />.
    /// </value>
	///
	/// <remarks>
	///	An exception is thrown if this property is set to a graph that is
	/// already owned by another graph drawer.  If you want to simultaneously
	/// draw the same graph with two different graph drawers, make a copy of
	/// the graph using IGraph.<see cref="IGraph.Clone(Boolean, Boolean)" />.
	/// </remarks>
    //*************************************************************************

    IGraph
    Graph
    {
        get;
        set;
    }

    //*************************************************************************
    //  Property: Layout
    //
    /// <summary>
    /// Gets or sets the object to use to lay out the graph.
    /// </summary>
    ///
    /// <value>
    /// The object to use to lay out the graph, as an <see cref="ILayout" />.
    /// </value>
    //*************************************************************************

    ILayout
    Layout
    {
        get;
        set;
    }

    //*************************************************************************
    //  Property: VertexDrawer
    //
    /// <summary>
    /// Gets or sets the object to use to draw the graph's vertices.
    /// </summary>
    ///
    /// <value>
    /// The object to use to draw the graph's vertices, as an <see
	///	cref="IVertexDrawer" />.
    /// </value>
    //*************************************************************************

    IVertexDrawer
    VertexDrawer
    {
        get;
        set;
    }

    //*************************************************************************
    //  Property: EdgeDrawer
    //
    /// <summary>
    /// Gets or sets the object to use to draw the graph's edges.
    /// </summary>
    ///
    /// <value>
    /// The object to use to draw the graph's edges, as an <see
	///	cref="IEdgeDrawer" />.
    /// </value>
    //*************************************************************************

    IEdgeDrawer
    EdgeDrawer
    {
        get;
        set;
    }

	//*************************************************************************
	//	Method: GetVertexFromPoint()
	//
	/// <overloads>
	/// Gets the vertex containing a specified point.
	/// </overloads>
	///
	/// <summary>
	/// Gets the vertex containing a specified <see cref="Point" />.
	/// </summary>
	///
	/// <param name="point">
	/// Point to get a vertex for.
	/// </param>
	///
	/// <param name="vertex">
	/// Where the <see cref="IVertex" /> object gets stored.
	/// </param>
	///
	/// <returns>
	///	true if a vertex containing the point was found, false if not.
	/// </returns>
	///
	/// <remarks>
	/// This method looks for a vertex that contains <paramref name="point" />.
	/// If there is such a vertex, the vertex is stored at <paramref
	///	name="vertex" /> and true is returned.  Otherwise, <paramref
	/// name="vertex" /> is set to null and false is returned.
	///
	///	<para>
	/// The graph should be drawn before this method is used.  If the graph
	/// hasn't been drawn, the return value is unpredictable.
	///	</para>
	///
	/// <para>
	///	The <see cref="IVertexDrawer.VertexContainsPoint" /> method on the <see
	///	cref="VertexDrawer" /> object is used to determine which vertex, if
	///	any, contains the point.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	Boolean
	GetVertexFromPoint
	(
		Point point,
		out IVertex vertex
	);

	//*************************************************************************
	//	Method: GetVertexFromPoint()
	//
	/// <summary>
	/// Gets the vertex containing a specified coordinate pair.
	/// </summary>
	///
	/// <param name="x">
	/// X-coordinate of the point to get a vertex for.
	/// </param>
	///
	/// <param name="y">
	/// Y-coordinate of the point to get a vertex for.
	/// </param>
	///
	/// <param name="vertex">
	/// Where the <see cref="IVertex" /> object gets stored.
	/// </param>
	///
	/// <returns>
	///	true if a vertex containing the point was found, false if not.
	/// </returns>
	///
	/// <remarks>
	/// This method looks for a vertex that contains the point
	/// (<paramref name="x" />, <paramref name="y" />).  If there is such a
	/// vertex, the vertex is stored at <paramref name="vertex" /> and true is
	///	returned.  Otherwise, <paramref name="vertex" /> is set to null and
	///	false is returned.
	///
	///	<para>
	/// The graph should be drawn before this method is used.  If the graph
	/// hasn't been drawn, the return value is unpredictable.
	///	</para>
	///
	/// <para>
	///	The <see cref="IVertexDrawer.VertexContainsPoint" /> method on the <see
	///	cref="VertexDrawer" /> object is used to determine which vertex, if
	///	any, contains the point.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	Boolean
	GetVertexFromPoint
	(
		Int32 x,
		Int32 y,
		out IVertex vertex
	);

	//*************************************************************************
	//	Event: LayoutRequired
	//
	/// <summary>
	///	Occurs when a change occurs that requires the graph to be laid out
	/// again.
	/// </summary>
	///
	/// <remarks>
	///	The implementation must fire this event when any change is made to the
	/// object that might affect the layout of the graph, such as a vertex
	/// being added to the graph.
	///
	/// <para>
	/// The owner should lay out the graph and redraw it in response to the
	/// event.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	event EventHandler LayoutRequired;


	//*************************************************************************
	//	Event: RedrawRequired
	//
	/// <summary>
	///	Occurs when a change occurs that requires a graph redraw.
	/// </summary>
	///
	/// <remarks>
	///	The implementation fires this event when any change is made to the
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
}

}
