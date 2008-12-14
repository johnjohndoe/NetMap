
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Visualization
{
//*****************************************************************************
//	Class: GraphDrawerBase
//
/// <summary>
///	Base class for graph drawers.
/// </summary>
///
/// <remarks>
/// This abstract class can serve as a base class for <see
/// cref="IGraphDrawer" /> implementations.  Its implementations of the <see
/// cref="IGraphDrawer" /> public methods provide error checking but usually
/// defer the actual work to protected abstract methods.
/// </remarks>
//*****************************************************************************

public abstract class GraphDrawerBase : DrawerBase, IGraphDrawer
{
    //*************************************************************************
    //  Constructor: GraphDrawerBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphDrawerBase" /> class.
    /// </summary>
    //*************************************************************************

    public GraphDrawerBase()
    {
        m_oGraph = new Graph();

		ConnectGraphEvents(m_oGraph);

		m_oLayout = new FruchtermanReingoldLayout();

		ConnectLayoutEvents(m_oLayout);

		m_oVertexDrawer = new VertexDrawer();

		ConnectVertexDrawerEvents(m_oVertexDrawer);

		m_oEdgeDrawer = new EdgeDrawer();

		ConnectEdgeDrawerEvents(m_oEdgeDrawer);

		m_oBackColor = SystemColors.Window;

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
	/// </remarks>
    //*************************************************************************

    public IGraph
    Graph
    {
        get
		{
			AssertValid();

			return (m_oGraph);
		}

        set
		{
			const String PropertyName = "Graph";

			this.ArgumentChecker.CheckPropertyNotNull(PropertyName, value);

			if (m_oGraph == value)
			{
				return;
			}

			// TODO: Prevent use by another IGraphDrawer.

			m_oGraph = value;

			ConnectGraphEvents(m_oGraph);

			FireRedrawRequired();

			AssertValid();
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
	/// The default value is a <see cref="FruchtermanReingoldLayout" />
	/// object.
    /// </value>
    //*************************************************************************

    public ILayout
    Layout
    {
        get
		{
			AssertValid();

			return (m_oLayout);
		}

        set
		{
			const String PropertyName = "Layout";

			this.ArgumentChecker.CheckPropertyNotNull(PropertyName, value);

			if (m_oLayout == value)
			{
				return;
			}

			m_oLayout = value;

			ConnectLayoutEvents(m_oLayout);

			FireLayoutRequired();

			AssertValid();
		}
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
	///	cref="IVertexDrawer" />.  The default value is a <see
	/// cref="Visualization.VertexDrawer" /> object.
    /// </value>
    //*************************************************************************

    public IVertexDrawer
    VertexDrawer
	{
        get
		{
			AssertValid();

			return (m_oVertexDrawer);
		}

        set
		{
			const String PropertyName = "VertexDrawer";

			this.ArgumentChecker.CheckPropertyNotNull(PropertyName, value);

			if (m_oVertexDrawer == value)
			{
				return;
			}

			m_oVertexDrawer = value;

			ConnectVertexDrawerEvents(m_oVertexDrawer);

			FireRedrawRequired();

			AssertValid();
		}
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
	///	cref="IEdgeDrawer" />.  The default value is an <see
	/// cref="Visualization.EdgeDrawer" /> object.
    /// </value>
    //*************************************************************************

    public IEdgeDrawer
    EdgeDrawer
    {
        get
		{
			AssertValid();

			return (m_oEdgeDrawer);
		}

        set
		{
			const String PropertyName = "EdgeDrawer";

			this.ArgumentChecker.CheckPropertyNotNull(PropertyName, value);

			if (m_oEdgeDrawer == value)
			{
				return;
			}

			m_oEdgeDrawer = value;

			ConnectEdgeDrawerEvents(m_oEdgeDrawer);

			FireRedrawRequired();

			AssertValid();
		}
    }

	//*************************************************************************
	//	Property: BackColor
	//
	/// <summary>
	/// Gets or sets the graph's background color.
	/// </summary>
	///
	/// <value>
	/// The graph's background color, as a <see cref="Color" />.  The default
	/// value is SystemColors.<see cref="SystemColors.Window" />.
	/// </value>
	//*************************************************************************

	public Color
	BackColor
	{
		get
		{
			AssertValid();

			return (m_oBackColor);
		}

		set
		{
			if (m_oBackColor == value)
			{
				return;
			}

			m_oBackColor = value;

			FireRedrawRequired();

			AssertValid();
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
	/// Lays out and draws the graph within the entire rectangle of a <see
	/// cref="Bitmap" />.
	/// </summary>
	///
	/// <param name="bitmap">
	/// <see cref="Bitmap" /> to draw onto.
	/// </param>
	//*************************************************************************

	public void
	Draw
	(
		Bitmap bitmap
	)
	{
		AssertValid();

		const String MethodName = "Draw";

		this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "bitmap", bitmap);

		Rectangle oGraphRectangle = new Rectangle(Point.Empty, bitmap.Size);

		Draw(bitmap, oGraphRectangle);
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
	//*************************************************************************

	public void
	Draw
	(
		Bitmap bitmap,
		Rectangle graphRectangle
	)
	{
		AssertValid();

		const String MethodName = "Draw";

		ArgumentChecker oArgumentChecker = this.ArgumentChecker;

		oArgumentChecker.CheckArgumentNotNull(MethodName, "bitmap", bitmap);

		Graphics oGraphics = null;

		try
		{
			// Get a Graphics object to use.

			oGraphics = Graphics.FromImage(bitmap);

			// Draw the graph onto the Graphics object.

			Draw(oGraphics, graphRectangle);
		}
		finally
		{
			GraphicsUtil.DisposeGraphics(ref oGraphics);
		}
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
	//*************************************************************************

	public void
	Draw
	(
		Graphics graphics,
		Rectangle graphRectangle
	)
	{
		AssertValid();

		const String MethodName = "Draw";

		this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "graphics", graphics);

		DrawCore(graphics, graphRectangle);
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

	public Boolean
	GetVertexFromPoint
	(
		Point point,
		out IVertex vertex
	)
	{
		AssertValid();

		vertex = null;

		// Loop backwards through the vertex collection.  This causes the most-
		// recently drawn vertices to be checked first.

		foreach ( IVertex oVertex in m_oGraph.Vertices.GetReverseEnumerable() )
		{
			if ( m_oVertexDrawer.VertexContainsPoint(oVertex, point) )
			{
				vertex = oVertex;
				return (true);
			}
		}

		return (false);
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
	/// </remarks>
	//*************************************************************************

	public Boolean
	GetVertexFromPoint
	(
		Int32 x,
		Int32 y,
		out IVertex vertex
	)
	{
		AssertValid();

		return ( GetVertexFromPoint(new Point(x, y), out vertex) );
	}

	//*************************************************************************
	//	Method: DrawCore()
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
	/// The arguments have already been checked for validity.
	/// </remarks>
	//*************************************************************************

	protected abstract void
	DrawCore
	(
		Graphics graphics,
		Rectangle graphRectangle
	);

	//*************************************************************************
	//	Method: SetSmoothingModeForVerticesAndEdges()
	//
	/// <summary>
	/// Sets the SmoothingMode on a <see cref="Graphics" /> object for drawing
	/// vertices and edges.
	/// </summary>
	///
	/// <param name="oGraphics">
	/// <see cref="Graphics" /> object to set the Smoothing mode on.
	/// </param>
	///
	/// <returns>
	/// The previous SmoothingMode.
	/// </returns>
	//*************************************************************************

	protected SmoothingMode
	SetSmoothingModeForVerticesAndEdges
	(
		Graphics oGraphics
	)
	{
		AssertValid();
		Debug.Assert(oGraphics != null);

		SmoothingMode eOldSmoothingMode = oGraphics.SmoothingMode;

		oGraphics.SmoothingMode = SmoothingMode.AntiAlias;

		return (eOldSmoothingMode);
	}

	//*************************************************************************
	//	Method: DrawNoLayout()
	//
	/// <summary>
	/// Draws the graph without laying it out.
	/// </summary>
	///
    /// <param name="oDrawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
	///
	/// <remarks>
	/// It's assumed that the graph has been laid out before this method is
	/// called.
	/// </remarks>
	//*************************************************************************

	protected void
	DrawNoLayout
	(
		DrawContext oDrawContext
	)
	{
		Debug.Assert(oDrawContext != null);
		AssertValid();

		DrawBackground(oDrawContext);

		Graphics oGraphics = oDrawContext.Graphics;

		SmoothingMode eOldSmoothingMode =
			SetSmoothingModeForVerticesAndEdges(oGraphics);

		// Give the vertex drawer a chance to do any pre-drawing calculations
		// and to change vertex locations.

		PreDrawVertices(oDrawContext);

		DrawEdges(oDrawContext);

		DrawVertices(oDrawContext);

		oGraphics.SmoothingMode = eOldSmoothingMode;
	}

	//*************************************************************************
	//	Method: DrawBackground()
	//
	/// <summary>
	/// Draws the graph's background.
	/// </summary>
	///
    /// <param name="oDrawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
	///
	/// <remarks>
	/// This method draws a solid background using <see cref="BackColor" />.
	/// A derived class may override this method to customize the background.
	/// </remarks>
	//*************************************************************************

	protected virtual void
	DrawBackground
	(
		DrawContext oDrawContext
	)
	{
		AssertValid();
		Debug.Assert(oDrawContext != null);

		Brush oBrush = new SolidBrush(m_oBackColor);

		oDrawContext.Graphics.FillRectangle(
			oBrush, oDrawContext.GraphRectangle);

		GraphicsUtil.DisposeBrush(ref oBrush);
	}

	//*************************************************************************
	//	Method: PreDrawVertices()
	//
	/// <summary>
	/// Gives the vertex drawer a chance to do any pre-drawing calculations
	/// and to change vertex locations.
	/// </summary>
	///
    /// <param name="oDrawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
	//*************************************************************************

	protected void
	PreDrawVertices
	(
		DrawContext oDrawContext
	)
	{
		AssertValid();
		Debug.Assert(oDrawContext != null);

		foreach (IVertex oVertex in m_oGraph.Vertices)
		{
			m_oVertexDrawer.PreDrawVertex(oVertex, oDrawContext);
		}
	}

	//*************************************************************************
	//	Method: DrawEdges()
	//
	/// <summary>
	/// Draws the graph's edges.
	/// </summary>
	///
    /// <param name="oDrawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
	///
	/// <remarks>
	/// This method draws each edge in the graph's Edges collection.  A derived
	/// class may override this method to customize the edges.
	/// </remarks>
	//*************************************************************************

	protected virtual void
	DrawEdges
	(
		DrawContext oDrawContext
	)
	{
		AssertValid();
		Debug.Assert(oDrawContext != null);

		foreach (IEdge oEdge in m_oGraph.Edges)
		{
			m_oEdgeDrawer.DrawEdge(oEdge, oDrawContext);
		}
	}

	//*************************************************************************
	//	Method: DrawVertices()
	//
	/// <summary>
	/// Draws the graph's vertices.
	/// </summary>
	///
    /// <param name="oDrawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
	///
	/// <remarks>
	/// This method draws each vertex in the graph's Vertices collection.  A
	/// derived class may override this method to customize the vertices.
	/// </remarks>
	//*************************************************************************

	protected virtual void
	DrawVertices
	(
		DrawContext oDrawContext
	)
	{
		AssertValid();
		Debug.Assert(oDrawContext != null);

		foreach (IVertex oVertex in m_oGraph.Vertices)
		{
			m_oVertexDrawer.DrawVertex(oVertex, oDrawContext);
		}
	}

    //*************************************************************************
    //  Method: ConnectGraphEvents()
    //
    /// <summary>
    /// Connects event handlers to an <see cref="IGraph" /> object's events.
    /// </summary>
	///
    /// <param name="oGraph">
	/// Object whose events need to be handled.
    /// </param>
    //*************************************************************************

    protected void
	ConnectGraphEvents
	(
		IGraph oGraph
	)
    {
		Debug.Assert(oGraph != null);

		oGraph.Vertices.VertexAdded +=
			new VertexEventHandler(this.OnVertexAddedOrRemoved);

		oGraph.Vertices.VertexRemoved +=
			new VertexEventHandler(this.OnVertexAddedOrRemoved);

		oGraph.Edges.EdgeAdded +=
			new EdgeEventHandler(this.OnEdgeAddedOrRemoved);

		oGraph.Edges.EdgeRemoved +=
			new EdgeEventHandler(this.OnEdgeAddedOrRemoved);
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

    protected virtual void
	ConnectLayoutEvents
	(
		ILayout oLayout
	)
    {
		Debug.Assert(oLayout != null);

		oLayout.LayoutRequired += new EventHandler(this.OnChildLayoutRequired);
    }

    //*************************************************************************
    //  Method: ConnectVertexDrawerEvents()
    //
    /// <summary>
    /// Connects event handlers to an <see cref="IVertexDrawer" /> object's
	/// events.
    /// </summary>
	///
    /// <param name="oVertexDrawer">
	/// Object whose events need to be handled.
    /// </param>
    //*************************************************************************

    protected void
	ConnectVertexDrawerEvents
	(
		IVertexDrawer oVertexDrawer
	)
    {
		Debug.Assert(oVertexDrawer != null);

		oVertexDrawer.RedrawRequired +=
			new EventHandler(this.OnChildRedrawRequired);

		oVertexDrawer.LayoutRequired +=
			new EventHandler(this.OnChildLayoutRequired);
    }

    //*************************************************************************
    //  Method: ConnectEdgeDrawerEvents()
    //
    /// <summary>
    /// Connects event handlers to an <see cref="IEdgeDrawer" /> object's
	/// events.
    /// </summary>
	///
    /// <param name="oEdgeDrawer">
	/// Object whose events need to be handled.
    /// </param>
    //*************************************************************************

    protected void
	ConnectEdgeDrawerEvents
	(
		IEdgeDrawer oEdgeDrawer
	)
    {
		Debug.Assert(oEdgeDrawer != null);

		oEdgeDrawer.RedrawRequired +=
			new EventHandler(this.OnChildRedrawRequired);

		oEdgeDrawer.LayoutRequired +=
			new EventHandler(this.OnChildLayoutRequired);
    }

    //*************************************************************************
    //  Method: OnChildLayoutRequired()
    //
    /// <summary>
	/// Handles the LayoutRequired event on several child objects.
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
	OnChildLayoutRequired
	(
		Object oSender,
		EventArgs oEventArgs
	)
	{
		FireLayoutRequired();
	}

    //*************************************************************************
    //  Method: OnChildRedrawRequired()
    //
    /// <summary>
	/// Handles the RedrawRequired event on several child objects.
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
	OnChildRedrawRequired
	(
		Object oSender,
		EventArgs oEventArgs
	)
	{
		FireRedrawRequired();
	}

    //*************************************************************************
    //  Method: OnVertexAddedOrRemoved()
    //
    /// <summary>
	/// Handles the <see cref="IVertexCollection.VertexAdded" /> and <see
	/// cref="IVertexCollection.VertexRemoved" />
	/// events.
    /// </summary>
	///
	/// <param name="oSender">
	/// Standard event argument.
	/// </param>
	///
	/// <param name="oVertexEventArgs">
	/// Standard event argument.
	/// </param>
    //*************************************************************************

	protected void
	OnVertexAddedOrRemoved
	(
		Object oSender,
		VertexEventArgs oVertexEventArgs
	)
	{
		FireLayoutRequired();
	}

    //*************************************************************************
    //  Method: OnEdgeAddedOrRemoved()
    //
    /// <summary>
	/// Handles the <see cref="IEdgeCollection.EdgeAdded" /> and <see
	/// cref="IEdgeCollection.EdgeRemoved" />
	/// events.
    /// </summary>
	///
	/// <param name="oSender">
	/// Standard event argument.
	/// </param>
	///
	/// <param name="oEdgeEventArgs">
	/// Standard event argument.
	/// </param>
    //*************************************************************************

	protected void
	OnEdgeAddedOrRemoved
	(
		Object oSender,
		EdgeEventArgs oEdgeEventArgs
	)
	{
		FireLayoutRequired();
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

		Debug.Assert(m_oGraph != null);
		Debug.Assert(m_oLayout != null);
		Debug.Assert(m_oVertexDrawer != null);
		Debug.Assert(m_oEdgeDrawer != null);
		// m_oBackColor
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Graph to draw.

	protected IGraph m_oGraph;

    /// Object to use to lay out the graph.

	protected ILayout m_oLayout;

    /// Object to use to draw the graph's vertices.

    protected IVertexDrawer m_oVertexDrawer;

    /// Object to use to draw the graph's edges.

    protected IEdgeDrawer m_oEdgeDrawer;

	/// Background color.

	protected Color m_oBackColor;
}

}
