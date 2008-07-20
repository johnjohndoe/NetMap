
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.Visualization
{
//*****************************************************************************
//	Class: AsyncGraphDrawer
//
/// <summary>
///	Draws a graph onto a <see cref="Bitmap" /> or <see cref="Graphics" />
/// object in either a synchronous or asynchronous manner.
/// </summary>
///
/// <remarks>
///	<see cref="AsyncGraphDrawer" /> is one of several classes provided with the
/// NetMap system that draw a graph, which is a set of vertices connected by
/// edges.
///
/// <para>
///	The following table summarizes the graph-drawing classes:
/// </para>
///
///	<list type="table">
///
///	<listheader>
/// <term>Class</term>
/// <term>For Use In</term>
/// <term>Features</term>
/// <term>Required NetMap Assemblies</term>
///	</listheader>
///
///	<item>
///	<term><see cref="GraphDrawer" /></term>
///	<term>
///	Any application that wants to draw a graph onto a <see cref="Bitmap" /> or
/// a <see cref="Graphics" /> object in a synchronous manner.
///	</term>
///	<term>
/// Can use custom layouts, vertex drawers, and edge drawers.
///	</term>
///	<term>
///	Core.dll, Visualization.dll
///	</term>
///	</item>
///
///	<item>
///	<term><see cref="AsyncGraphDrawer" /></term>
///	<term>
///	Any application that wants to draw a graph onto a <see cref="Bitmap" /> or
/// a <see cref="Graphics" /> object in a synchronous or asynchronous manner.
///	</term>
///	<term>
/// Can use custom layouts, vertex drawers, and edge drawers.
///	</term>
///	<term>
///	Core.dll, Visualization.dll
///	</term>
///	</item>
///
///	<item>
///	<term><see cref="MultiSelectionGraphDrawer" /></term>
///	<term>
///	Any application that wants to draw a graph onto a <see cref="Bitmap" /> or
/// a <see cref="Graphics" /> object in a synchronous or asynchronous manner.
///	</term>
///	<term>
/// Same as <see cref="AsyncGraphDrawer" />, plus vertices and edges can be
/// drawn as selected.
///	</term>
///	<term>
///	Core.dll, Visualization.dll
///	</term>
///	</item>
///
///	<item>
///	<term>NetMapControl</term>
///	<term>
///	Windows Forms applications
///	</term>
///	<term>
/// Wraps a <see cref="MultiSelectionGraphDrawer" /> in a Windows Forms
/// control.
///	</term>
///	<term>
///	Core.dll, Visualization.dll, Control.dll
///	</term>
///	</item>
///
///	</list>
///
/// <para>
/// <see cref="AsyncGraphDrawer" /> draws a graph onto a <see cref="Bitmap" />
/// or <see cref="Graphics" /> object using <see
/// cref="FruchtermanReingoldLayout" />, <see cref="VertexDrawer" />, and <see
/// cref="EdgeDrawer" /> objects by default.  The graph can be
/// customized by setting the <see cref="GraphDrawerBase.Layout" />, <see
/// cref="VertexDrawer" />, and <see cref="EdgeDrawer" /> properties to
/// alterative objects, or by deriving a class from this class and overriding
/// any of its methods.
/// </para>
///
/// </remarks>
///
///	<example>
///	Here is sample C# code that populates a graph with several vertices and
/// edges, adds metadata to the vertices and edges, sets the graph colors, and
/// draws the graph onto a bitmap in an asynchronous manner.
///
/// <code>
/// TODO: Add asynchronous code.
/// </code>
///
/// </example>
//*****************************************************************************

public class AsyncGraphDrawer : GraphDrawer, IAsyncGraphDrawer
{
    //*************************************************************************
    //  Constructor: AsyncGraphDrawer()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncGraphDrawer" />
	/// class.
    /// </summary>
    //*************************************************************************

    public AsyncGraphDrawer()
    {
		m_bIsBusy = false;
		m_oDrawContext = null;
		m_oGraphicsState = null;
		m_bDisposeGraphics = false;

		AssertValid();
    }

    //*************************************************************************
    //  Property: Graph
    //
    /// <summary>
    /// Gets or sets the graph to draw.
    /// </summary>
    ///
    /// <value>
    /// The graph to draw, as an <see cref="IGraph" />.  The default value is a
	/// <see cref="Graph" /> with mixed <see cref="IGraph.Directedness" />, no
	/// <see cref="IGraph.Restrictions" />, no vertices, and no edges.
    /// </value>
	///
	/// <remarks>
	///	An exception is thrown if this property is set to a graph that is
	/// already owned by another graph drawer.  If you want to simultaneously
	/// draw the same graph with two different graph drawers, make a copy of
	/// the graph using IGraph.<see cref="IGraph.Clone(Boolean, Boolean)" />.
	///
	/// <para>
	/// An exception is thrown if this property is set while an asynchronous
	/// drawing is in progress.
	/// </para>
	///
	/// </remarks>
    //*************************************************************************

    public new IGraph
    Graph
    {
        get
		{
			AssertValid();

			return (base.Graph);
		}

        set
		{
			const String PropertyName = "Graph";

			CheckBusy(PropertyName);

			base.Graph = value;
		}
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
	///
	/// <para>
	/// An exception is thrown if this property is set while an asynchronous
	/// drawing is in progress.
	/// </para>
	///
	/// </remarks>
    //*************************************************************************

    public new ILayout
    Layout
    {
        get
		{
			AssertValid();

			return (base.Layout);
		}

        set
		{
			AssertValid();

			const String PropertyName = "Layout";

			CheckBusy(PropertyName);

			// (The base class will check for null.)

			if ( value != null && !(value is IAsyncLayout) )
			{
				this.ArgumentChecker.ThrowPropertyException(PropertyName,

					"The Layout must implement the IAsyncLayout interface."
					);
			}

			CheckBusy(PropertyName);

			base.Layout = value;
		}
    }

    //*************************************************************************
    //  Property: IsBusy
    //
    /// <summary>
	/// Gets a value indicating whether an asynchronous drawing is in
	/// progress.
    /// </summary>
    ///
    /// <value>
    /// true if an asynchronous drawing is in progress.
    /// </value>
    //*************************************************************************

    public Boolean
    IsBusy
    {
        get
        {
            AssertValid();

			return (m_bIsBusy);
        }
    }

	//*************************************************************************
	//	Method: Draw()
	//
	/// <overloads>
	/// Lays out and draws the graph.
	/// </overloads>
	///
	/// <summary>
	/// Draws the graph within the entire rectangle of a <see cref="Bitmap" />.
	/// </summary>
	///
	/// <param name="bitmap">
	/// <see cref="Bitmap" /> to draw onto.
	/// </param>
	///
	/// <remarks>
	/// An exception is thrown if an asynchronous drawing is in progress.
	/// </remarks>
	//*************************************************************************

	public new void
	Draw
	(
		Bitmap bitmap
	)
	{
		AssertValid();

		const String MethodName = "Draw";

		CheckBusy(MethodName);

		base.Draw(bitmap);
	}

	//*************************************************************************
	//	Method: Draw()
	//
	/// <summary>
	/// Lays out and draws the graph within a specified rectangle of a <see
	/// cref="Bitmap" />.
	/// </summary>
	///
	/// <param name="bitmap">
	/// <see cref="Bitmap" /> to draw onto.
	/// </param>
	///
	/// <param name="graphRectangle">
	///	<see cref="Rectangle" /> to draw within.
	/// </param>
	///
	/// <remarks>
	/// An exception is thrown if an asynchronous drawing is in progress.
	/// </remarks>
	//*************************************************************************

	public new void
	Draw
	(
		Bitmap bitmap,
		Rectangle graphRectangle
	)
	{
		AssertValid();

		const String MethodName = "Draw";

		CheckBusy(MethodName);

		base.Draw(bitmap, graphRectangle);
	}

	//*************************************************************************
	//	Method: Draw()
	//
	/// <summary>
	/// Lays out and draws the graph within a specified rectangle of a <see
	/// cref="Graphics" /> object.
	/// </summary>
	///
	/// <param name="graphics">
	/// <see cref="Graphics" /> object to draw onto.
	/// </param>
	///
	/// <param name="graphRectangle">
	/// <see cref="Rectangle" /> to draw within.
	/// </param>
	///
	/// <remarks>
	/// An exception is thrown if an asynchronous drawing is in progress.
	/// </remarks>
	//*************************************************************************

	public new void
	Draw
	(
		Graphics graphics,
		Rectangle graphRectangle
	)
	{
		AssertValid();

		const String MethodName = "Draw";

		CheckBusy(MethodName);

		base.Draw(graphics, graphRectangle);
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
	///
	/// <para>
	/// An exception is thrown if an asynchronous drawing is in progress.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public new Boolean
	GetVertexFromPoint
	(
		Point point,
		out IVertex vertex
	)
	{
		AssertValid();

		String MethodName = "GetVertexFromPoint";

		CheckBusy(MethodName);

		return ( base.GetVertexFromPoint(point, out vertex) );
	}

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
	/// <para>
	/// An exception is thrown if an asynchronous drawing is in progress.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public new Boolean
	GetVertexFromPoint
	(
		Int32 x,
		Int32 y,
		out IVertex vertex
	)
	{
		AssertValid();

		String MethodName = "GetVertexFromPoint";

		CheckBusy(MethodName);

		return ( base.GetVertexFromPoint(x, y, out vertex) );
	}

	//*************************************************************************
	//	Method: DrawAsync()
	//
	/// <overloads>
	/// Asynchronously lays out and draws the graph.
	/// </overloads>
	///
	/// <summary>
	/// Lays out and draws the graph within the entire rectangle of a <see
	/// cref="Bitmap" />.
	/// </summary>
	///
	/// <param name="bitmap">
	/// <see cref="Bitmap" /> to draw onto.
	/// </param>
	///
	/// <remarks>
	/// This method starts laying out and drawing the graph on a worker thread
	/// and returns immediately.  The <see
	/// cref="DrawAsyncIterationCompleted" /> event may fire repeatedly while
	/// the drawing is occurring.  The <see cref="DrawAsyncCompleted" /> event
	/// fires when the drawing is complete, an error occurs, or the drawing is
	/// cancelled.  <see cref="DrawAsyncCancel" /> cancels the drawing.
	///
	/// <para>
	/// An exception is thrown if an asynchronous drawing is already in
	/// progress.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public void
	DrawAsync
	(
		Bitmap bitmap
	)
	{
		AssertValid();

		const String MethodName = "DrawAsync";

		this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "bitmap", bitmap);

		Rectangle oGraphRectangle = new Rectangle(Point.Empty, bitmap.Size);

		DrawAsync(bitmap, oGraphRectangle);
	}

	//*************************************************************************
	//	Method: DrawAsync()
	//
	/// <summary>
	/// Asynchronously lays out and draws the graph within a specified
	/// rectangle of a <see cref="Bitmap" />.
	/// </summary>
	///
	/// <param name="bitmap">
	/// <see cref="Bitmap" /> to draw onto.
	/// </param>
	///
	/// <param name="graphRectangle">
	///	<see cref="Rectangle" /> to draw within.
	/// </param>
	///
	/// <remarks>
	/// This method starts laying out and drawing the graph on a worker thread
	/// and returns immediately.  The <see
	/// cref="DrawAsyncIterationCompleted" /> event may fire repeatedly while
	/// the drawing is occurring.  The <see cref="DrawAsyncCompleted" /> event
	/// fires when the drawing is complete, an error occurs, or the drawing is
	/// cancelled.  <see cref="DrawAsyncCancel" /> cancels the drawing.
	///
	/// <para>
	/// An exception is thrown if an asynchronous drawing is already in
	/// progress.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public void
	DrawAsync
	(
		Bitmap bitmap,
		Rectangle graphRectangle
	)
	{
		AssertValid();

		const String MethodName = "DrawAsync";

		this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "bitmap", bitmap);

		// Get a Graphics object to use.  This will get disposed by
    	// Layout_LayOutGraphCompleted().

		Graphics oGraphics = Graphics.FromImage(bitmap);

		m_bDisposeGraphics = true;

		// Draw the graph onto the Graphics object.

		DrawAsync(oGraphics, graphRectangle);
	}

	//*************************************************************************
	//	Method: DrawAsync()
	//
	/// <summary>
	/// Asynchronously lays out and draws the graph within a specified
	/// rectangle of a <see cref="Graphics" /> object.
	/// </summary>
	///
	/// <param name="graphics">
	/// <see cref="Graphics" /> object to draw onto.
	/// </param>
	///
	/// <param name="graphRectangle">
	/// <see cref="Rectangle" /> to draw within.
	/// </param>
	///
	/// <remarks>
	/// This method starts laying out and drawing the graph on a worker thread
	/// and returns immediately.  The <see
	/// cref="DrawAsyncIterationCompleted" /> event may fire repeatedly while
	/// the drawing is occurring.  The <see cref="DrawAsyncCompleted" /> event
	/// fires when the drawing is complete, an error occurs, or the drawing is
	/// cancelled.  <see cref="DrawAsyncCancel" /> cancels the drawing.
	///
	/// <para>
	/// An exception is thrown if an asynchronous drawing is already in
	/// progress.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public void
	DrawAsync
	(
		Graphics graphics,
		Rectangle graphRectangle
	)
	{
		AssertValid();

		const String MethodName = "DrawAsync";

		this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "graphics", graphics);

		CheckBusy(MethodName);

		m_bIsBusy = true;

		// Save the Graphics state.  It will get restored by
		// Layout_LayOutGraphCompleted().

		Debug.Assert(m_oGraphicsState == null);

		m_oGraphicsState = graphics.Save();

		graphics.Clip = new Region(graphRectangle);

		// Create a DrawContext object that will be used within the
		// IAsyncLayout event handlers.
		
		Debug.Assert(m_oDrawContext == null);

		m_oDrawContext = new DrawContext(
			this, graphics, graphRectangle, this.Layout.Margin);

		// Lay out the graph asynchronously.  The graph will be drawn within
		// the IAsyncLayout event handlers.

		LayoutContext oLayoutContext = new LayoutContext(graphRectangle, this);

		this.AsyncLayout.LayOutGraphAsync(m_oGraph, oLayoutContext);
	}

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

    public void
    DrawAsyncCancel()
	{
		AssertValid();

		IAsyncLayout oAsyncLayout = this.AsyncLayout;

		if (!oAsyncLayout.IsBusy)
		{
			return;
		}

		oAsyncLayout.LayOutGraphAsyncCancel();
	}

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

	public event EventHandler DrawAsyncIterationCompleted;


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
	///
	/// <para>
	/// If the drawing was cancelled by <see cref="DrawAsyncCancel" />, the
	/// AsyncCompletedEventArgs.<see
	/// cref="AsyncCompletedEventArgs.Cancelled" /> property is set to true.
	/// If an exception is thrown during drawing, the
	/// AsyncCompletedEventArgs.<see cref="AsyncCompletedEventArgs.Error" />
	/// property is set to the exception.  If you want to determine whether the
	/// drawing successfully completed, you must check both of these
	/// properties.
	/// </para>
	///
	/// <para>
	/// Do not check for errors by putting a try/catch block around the
	/// DrawAsync call.  Check the AsyncCompletedEventArgs.<see
	/// cref="AsyncCompletedEventArgs.Error" /> property instead.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public event AsyncCompletedEventHandler DrawAsyncCompleted;


    //*************************************************************************
    //  Property: AsyncLayout
    //
    /// <summary>
    /// Gets the object to use to lay out the graph asynchronously.
    /// </summary>
    ///
    /// <value>
    /// The object to use to lay out the graph asynchronously, as an <see
	/// cref="IAsyncLayout" />.
    /// </value>
    //*************************************************************************

    protected IAsyncLayout
    AsyncLayout
    {
        get
		{
			AssertValid();

			const String PropertyName = "AsyncLayout";

			if ( !(m_oLayout is IAsyncLayout) )
			{
				// This should never occur.

				Debug.Assert(false);

				throw new ApplicationException(String.Format(

					"{0}.{1}: Layout is not an IAsyncLayout."
					,
					this.ClassName,
					PropertyName
					) );
			}

			return ( (IAsyncLayout)m_oLayout );
		}
    }

    //*************************************************************************
    //  Method: ConnectLayoutEvents()
    //
    /// <summary>
    /// Connects event handlers to an <see cref="ILayout" /> object's events.
    /// </summary>
	///
    /// <param name="oLayout">
	/// Object whose events need to be handled.
    /// </param>
    //*************************************************************************

	protected override void
	ConnectLayoutEvents
	(
		ILayout oLayout
	)
	{
		Debug.Assert(oLayout != null);
		Debug.Assert(oLayout is IAsyncLayout);

		IAsyncLayout oAsyncLayout = (IAsyncLayout)oLayout;

		base.ConnectLayoutEvents(oLayout);

		oAsyncLayout.LayOutGraphIterationCompleted +=
			new EventHandler(this.Layout_LayOutGraphIterationCompleted);

		oAsyncLayout.LayOutGraphCompleted +=
			new AsyncCompletedEventHandler(this.Layout_LayOutGraphCompleted);
	}

	//*************************************************************************
	//	Method: CheckBusy()
	//
	/// <summary>
	/// Throws an exception if a drawing is in progress.
	/// </summary>
	///
	/// <param name="sMethodOrPropertyName">
	/// Name of the method or property calling this method.
	/// </param>
	//*************************************************************************

	protected void
	CheckBusy
	(
		String sMethodOrPropertyName
	)
	{
		Debug.Assert( !String.IsNullOrEmpty(sMethodOrPropertyName) );
		AssertValid();

		if (m_bIsBusy)
		{
			throw new InvalidOperationException(String.Format(

				"{0}.{1}: An asynchronous drawing is in progress."
				,
				this.ClassName,
				sMethodOrPropertyName
				) );
		}
	}

	//*************************************************************************
	//	Method: FireDrawAsyncIterationCompleted()
	//
	/// <summary>
	///	Fires the <see cref="DrawAsyncIterationCompleted" /> event if
	/// appropriate.
	/// </summary>
	//*************************************************************************

	protected void
	FireDrawAsyncIterationCompleted()
	{
		AssertValid();

		EventUtil.FireEvent(this, this.DrawAsyncIterationCompleted);
	}

	//*************************************************************************
	//	Method: FireDrawAsyncCompleted()
	//
	/// <summary>
	///	Fires the <see cref="DrawAsyncCompleted" /> event if appropriate.
	/// </summary>
	///
	/// <param name="oAsyncCompletedEventArgs">
	/// An <see cref="AsyncCompletedEventArgs" /> that contains the event data.
	/// </param>
	//*************************************************************************

	protected void
	FireDrawAsyncCompleted
	(
		AsyncCompletedEventArgs oAsyncCompletedEventArgs
	)
	{
		AssertValid();

		AsyncCompletedEventHandler oEventHandler = this.DrawAsyncCompleted;

		if (oEventHandler != null)
		{
			oEventHandler(this, oAsyncCompletedEventArgs);
		}
	}

    //*************************************************************************
    //  Method: Layout_LayOutGraphIterationCompleted()
    //
    /// <summary>
	/// Handles the <see cref="IAsyncLayout.LayOutGraphIterationCompleted" />
	/// event.
    /// </summary>
	///
	/// <param name="oSender">
	/// Standard event argument.
	/// </param>
	///
	/// <param name="oEventArgs">
	/// Standard event argument.
	/// </param>
    //*************************************************************************

	protected void
    Layout_LayOutGraphIterationCompleted
	(
		Object oSender,
		EventArgs oEventArgs
	)
	{
		AssertValid();

		// The asynchronous layout has completed one iteration of setting the
		// Location property on all the graph's vertices, and the next
		// iteration won't begin until this event handler returns.  Draw the
		// graph in its current state.

		Debug.Assert(m_oDrawContext != null);

		DrawNoLayout(m_oDrawContext);

		FireDrawAsyncIterationCompleted();
	}

    //*************************************************************************
    //  Method: Layout_LayOutGraphCompleted()
    //
    /// <summary>
	/// Handles the <see cref="IAsyncLayout.LayOutGraphCompleted" /> event.
    /// </summary>
	///
	/// <param name="oSender">
	/// Standard event argument.
	/// </param>
	///
	/// <param name="oAsyncCompletedEventArgs">
	/// Standard event argument.
	/// </param>
    //*************************************************************************

	protected void
    Layout_LayOutGraphCompleted
	(
		Object oSender,
		AsyncCompletedEventArgs oAsyncCompletedEventArgs
	)
	{
		AssertValid();

		if (oAsyncCompletedEventArgs.Error != null)
		{
			// (Do nothing.)
		}
		else if (oAsyncCompletedEventArgs.Cancelled)
		{
			// (Do nothing.)
		}
		else
		{
			// The asynchronous layout has completed setting the Location
			// property on all the graph's vertices.  Draw the graph in its
			// final state.

			Debug.Assert(m_oDrawContext != null);

			DrawNoLayout(m_oDrawContext);
		}

		// Clean up.

		if (m_oDrawContext != null && m_oDrawContext.Graphics != null)
		{
			Graphics oGraphics = m_oDrawContext.Graphics;

			if (m_bDisposeGraphics)
			{
				GraphicsUtil.DisposeGraphics(ref oGraphics);
			}
			else if (m_oGraphicsState != null)
			{
				oGraphics.Restore(m_oGraphicsState);
			}
		}

		m_bIsBusy = false;
		m_oDrawContext = null;
		m_oGraphicsState = null;
		m_bDisposeGraphics = false;

		FireDrawAsyncCompleted(oAsyncCompletedEventArgs);
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

		// m_bIsBusy
		// m_oDrawContext
		// m_oGraphicsState
		// m_bDisposeGraphics
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// Multithreading Note
	//
	// Although this class performs most of its work on a worker thread, that
	// worker thread belongs to the class's IAsyncLayout object, which marshals
	// its events to the thread on which DrawAsync() was called.  Therefore,
	// all of the methods in this class run in the thread on which DrawAsync()
	// was called, and none of the following protected fields need to be
	// protected from multithreaded access.

	/// true if an asynchronous drawing is in progress.

	protected Boolean m_bIsBusy;

	/// Provides access to objects needed for asynchronous drawing, or null if
	/// no asynchronous drawing is in progress.

	protected DrawContext m_oDrawContext;

	/// Original state of the Graphics object in m_oDrawContext, or null if no
	/// asynchronous drawing is in progress.

	protected GraphicsState m_oGraphicsState;

	/// true if the Graphics object in m_oDrawContext must be disposed when
	/// done drawing.

	protected Boolean m_bDisposeGraphics;
}

}
