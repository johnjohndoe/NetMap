
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.Visualization
{
//*****************************************************************************
//  Interface: IAsyncGraphDrawer
//
/// <summary>
/// Supports graph drawing using an event-based asynchronous pattern.
/// </summary>
///
/// <remarks>
/// A class that implements this interface is responsible for drawing a graph
///	in either a synchronous or asynchronous manner.
///
/// <para>
/// Neither this interface nor its base <see cref="IGraphDrawer" /> interface
/// define any Draw methods, because the arguments passed to such methods vary
/// with the implementation.  The implementation might draw on a <see
/// cref="Bitmap" /> or a <see cref="Graphics" /> object, for example.  It's
/// conceivable that the graph drawer might not even draw directly on a visible
/// surface; an implementation might "draw" to a VML file, for example.
/// </para>
///
/// <para>
/// The implemention must define at least one synchronouos Draw method, along
/// with an asynchronous DrawAsync counterpart that follows the guidelines
/// outlined in the article "Multithreaded Programming with the Event-based
/// Asynchronous Pattern" in the .NET Framework Developer's Guide.  The
/// DrawAsync method starts the drawing on a worker thread and returns
/// immediately.  The <see cref="DrawAsyncIterationCompleted" /> event may fire
/// repeatedly while the drawing is occurring.  The <see
/// cref="DrawAsyncCompleted" /> event fires when the drawing is complete, an
/// error occurs, or the drawing is cancelled.  <see cref="DrawAsyncCancel" />
/// cancels the drawing.
/// </para>
///
/// </remarks>
//*****************************************************************************

public interface IAsyncGraphDrawer : IGraphDrawer
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

    new IGraph
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
	///
	/// <remarks>
	/// The object must implement <see cref="IAsyncLayout" /> as well as <see
	/// cref="ILayout" />.  An exception is thrown if the object does not
	/// implement <see cref="IAsyncLayout" />.
	/// </remarks>
    //*************************************************************************

    new ILayout
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

    new IVertexDrawer
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

    new IEdgeDrawer
    EdgeDrawer
    {
        get;
        set;
    }

    //*************************************************************************
    //  Property: IsBusy
    //
    /// <summary>
	/// Gets a value indicating whether an asynchronous operation is in
	/// progress.
    /// </summary>
    ///
    /// <value>
    /// true if an asynchronous operation is in progress.
    /// </value>
    //*************************************************************************

    Boolean
    IsBusy
    {
        get;
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

	new Boolean
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

	new Boolean
	GetVertexFromPoint
	(
		Int32 x,
		Int32 y,
		out IVertex vertex
	);

    //*************************************************************************
    //  Method: DrawAsyncCancel()
    //
    /// <summary>
    /// Cancels the drawing started by a DrawAsync method.
    /// </summary>
    ///
    /// <remarks>
	/// The drawing may or may not cancel, but the <see
	/// cref="DrawAsyncCompleted" /> event is guaranteed to fire.  The <see
	/// cref="AsyncCompletedEventArgs" /> object passed to the event handler
	/// contains a <see cref="AsyncCompletedEventArgs.Cancelled" /> property
	/// that indicates whether the cancellation occurred.
	///
	/// <para>
	/// If a drawing is not in progress, this method does nothing.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    void
    DrawAsyncCancel();


	//*************************************************************************
	//	Event: DrawAsyncIterationCompleted
	//
	/// <summary>
	///	Occurs when a drawing started by a DrawAsync method completes one
	/// iteration.
	/// </summary>
	///
	/// <remarks>
	/// If the <see cref="Layout" /> uses an iterative algorithm in which the
	/// graph is laid out and drawn multiple times before it reaches its final
	/// layout, this event fires after each iteration is completed.
	///
	/// <para>
	/// The event fires on the thread on which the DrawAsync method was called.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	event EventHandler DrawAsyncIterationCompleted;


	//*************************************************************************
	//	Event: DrawAsyncCompleted
	//
	/// <summary>
	///	Occurs when a drawing started by a DrawAsync method completes,
	/// is cancelled, or ends with an error.
	/// </summary>
	///
	/// <remarks>
	/// The event fires on the thread on which the DrawAsync method was called.
	/// </remarks>
	//*************************************************************************

	event AsyncCompletedEventHandler DrawAsyncCompleted;


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

	new event EventHandler LayoutRequired;


	//*************************************************************************
	//	Event: RedrawRequired
	//
	/// <summary>
	///	Occurs when a change occurs that requires a graph redraw.
	/// </summary>
	///
	/// <remarks>
	///	The implementation fires this event when any change is made to the
	/// object that might affect the appearance of the graph.  It does not fire
	/// when a graph is being drawn asynchronously and the drawing completes
	/// one iteration.
	///
	/// <para>
	/// The object owner should handle the event by redrawing the graph.  The
	/// graph does not need to be laid out again.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	new event EventHandler RedrawRequired;
}

}
