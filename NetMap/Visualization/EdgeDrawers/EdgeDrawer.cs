
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.Visualization
{
//*****************************************************************************
//  Class: EdgeDrawer
//
/// <summary>
///	Draws an edge as a simple line.
/// </summary>
///
/// <remarks>
///	This class draws an edge as a simple line connecting the edge's vertices.
/// It is meant to be used with <see cref="VertexDrawer" />, whose <see
/// cref="VertexDrawer.Radius" /> is taken into account when the edge is drawn.
/// If a vertex drawer other than <see cref="VertexDrawer" /> is used, the edge
/// is drawn between the centers of the edge's vertices.
///
/// <para>
/// This class works in conjunction with <see
/// cref="MultiSelectionGraphDrawer" /> to draw edges as either selected or
/// unselected.  If an edge has been marked as selected by <see
/// cref="MultiSelectionGraphDrawer.SelectEdge(IEdge)" />, it is drawn using
/// <see cref="SelectedColor" /> and <see cref="SelectedWidth" />.  Otherwise,
/// <see cref="Color" /> and <see cref="Width" /> are used.  Set the <see
/// cref="UseSelection" /> property to false to force all edges to be drawn as
/// unselected.
/// </para>
///
/// <para>
/// If this class is used by a graph drawer other than <see
/// cref="MultiSelectionGraphDrawer" />, all edges are drawn as unselected.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class EdgeDrawer : EdgeDrawerBase, IEdgeDrawer
{
    //*************************************************************************
    //  Constructor: EdgeDrawer()
    //
    /// <summary>
    /// Initializes a new instance of the EdgeDrawer class.
    /// </summary>
    //*************************************************************************

    public EdgeDrawer()
    {
		m_bUseSelection = true;

		m_iWidth = 1;

		m_iSelectedWidth = 2;

		m_bDrawArrowOnDirectedEdge = true;

		m_fRelativeArrowSize = 3.0F;

		m_oColor = SystemColors.WindowText;

		m_oSelectedColor = Color.Red;

		m_oUndirectedPen = null;

		m_oDirectedPen = null;

		m_oDirectedEndCap = null;

		CreateDrawingObjects();
    }

    //*************************************************************************
    //  Property: UseSelection
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the selected state of an edge
	/// should be used.
    /// </summary>
    ///
    /// <value>
	/// If true, an edge is drawn using either <see cref="Color" /> and <see
	/// cref="Width" /> or <see cref="SelectedColor" /> and <see
	/// cref="SelectedWidth" />, depending on whether the edge has been marked
	/// as selected by <see
	/// cref="MultiSelectionGraphDrawer.SelectEdge(IEdge)" />.  If false, <see
	/// cref="Color" /> and <see cref="Width" /> are used regardless of whether
	/// the edge has been marked as selected.
    /// </value>
    //*************************************************************************

    public Boolean
    UseSelection
    {
        get
		{
			AssertValid();

			return (m_bUseSelection);
		}

		set
		{
			if (m_bUseSelection == value)
			{
				return;
			}

			m_bUseSelection = value;

			// Note: Don't do this.  MultiSelectionGraphDrawer sets this
			// property while drawing the graph, and drawing the graph should
			// not result in another redraw.
			//
			// FireRedrawRequired();

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: Color
    //
    /// <summary>
    /// Gets or sets the color of an edge that is not selected.
    /// </summary>
    ///
    /// <value>
	///	The color of an unselected edge, as a <see
	/// cref="System.Drawing.Color" />.  The default value is <see
	///	cref="SystemColors.WindowText" />.
    /// </value>
	///
	/// <remarks>
	/// See <see cref="UseSelection" /> for details on selected vs. unselected
	/// edges.
	/// </remarks>
	///
	/// <seealso cref="SelectedColor" />
    //*************************************************************************

    public Color
    Color
    {
        get
		{
			AssertValid();

			return (m_oColor);
		}

		set
		{
			if (m_oColor == value)
			{
				return;
			}

			m_oColor = value;

			FireRedrawRequired();

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: SelectedColor
    //
    /// <summary>
    /// Gets or sets the color of an edge that is selected.
    /// </summary>
    ///
    /// <value>
	///	The color of a selected edge, as a <see
	/// cref="System.Drawing.Color" />.  The default value is <see
	///	cref="System.Drawing.Color.Red" />.
    /// </value>
	///
	/// <remarks>
	/// See <see cref="UseSelection" /> for details on selected vs. unselected
	/// edges.
	/// </remarks>
	///
	/// <seealso cref="Color" />
    //*************************************************************************

    public Color
    SelectedColor
    {
        get
		{
			AssertValid();

			return (m_oSelectedColor);
		}

		set
		{
			if (m_oSelectedColor == value)
			{
				return;
			}

			m_oSelectedColor = value;

			FireRedrawRequired();

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: Width
    //
    /// <summary>
    /// Gets or sets the width of an edge that is not selected.
    /// </summary>
    ///
    /// <value>
	///	The width of the edge, as an <see cref="Int32" />.  Must be between
	/// <see cref="MinimumWidth" /> and <see cref="MaximumWidth" />, inclusive.
	/// The default value is 1.
    /// </value>
	///
	/// <remarks>
	/// The units are determined by the <see cref="DrawContext.Graphics" />
	/// object passed to the <see cref="EdgeDrawerBase.DrawEdge" /> method.
	///
	/// <para>
	/// See <see cref="UseSelection" /> for details on selected vs. unselected
	/// edges.
	/// </para>
	///
	/// </remarks>
	///
	/// <seealso cref="SelectedWidth" />
    //*************************************************************************

    public Int32
    Width
    {
        get
		{
			AssertValid();

			return (m_iWidth);
		}

		set
		{
			const String PropertyName = "Width";

			this.ArgumentChecker.CheckPropertyInRange(PropertyName, value,
				MinimumWidth, MaximumWidth);

			if (m_iWidth == value)
			{
				return;
			}

			m_iWidth = value;

			FireRedrawRequired();

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: SelectedWidth
    //
    /// <summary>
    /// Gets or sets the width of an edge that is selected.
    /// </summary>
    ///
    /// <value>
	///	The width of the edge, as an <see cref="Int32" />.  Must be between
	/// <see cref="MinimumWidth" /> and <see cref="MaximumWidth" />, inclusive.
	/// The default value is 2.
    /// </value>
	///
	/// <remarks>
	/// The units are determined by the <see cref="DrawContext.Graphics" />
	/// object passed to the <see cref="EdgeDrawerBase.DrawEdge" /> method.
	///
	/// <para>
	/// See <see cref="UseSelection" /> for details on selected vs. unselected
	/// edges.
	/// </para>
	///
	/// </remarks>
	///
	/// <seealso cref="Width" />
    //*************************************************************************

    public Int32
    SelectedWidth
    {
        get
		{
			AssertValid();

			return (m_iSelectedWidth);
		}

		set
		{
			const String PropertyName = "SelectedWidth";

			this.ArgumentChecker.CheckPropertyInRange(PropertyName, value,
				MinimumWidth, MaximumWidth);

			if (m_iSelectedWidth == value)
			{
				return;
			}

			m_iSelectedWidth = value;

			FireRedrawRequired();

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: DrawArrowOnDirectedEdge
    //
    /// <summary>
    /// Gets or sets a flag that determines whether an arrow should be drawn on
	/// directed edges.
    /// </summary>
    ///
    /// <value>
	/// true to draw an arrow on directed edges, false otherwise.  The default
	/// value is true.
    /// </value>
	///
	/// <remarks>
	/// By default, an edge with <see cref="IEdge.IsDirected" /> set to true is
	/// drawn with an arrow pointing to the front vertex.  If this property is
	/// set to false, the arrow is not drawn.
	/// </remarks>
    //*************************************************************************

    public Boolean
    DrawArrowOnDirectedEdge
    {
        get
		{
			AssertValid();

			return (m_bDrawArrowOnDirectedEdge);
		}

		set
		{
			if (m_bDrawArrowOnDirectedEdge == value)
			{
				return;
			}

			m_bDrawArrowOnDirectedEdge = value;

			FireRedrawRequired();

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: RelativeArrowSize
    //
    /// <summary>
    /// Gets or sets the relative size of arrow heads on directed edges.
    /// </summary>
    ///
    /// <value>
	///	The relative size of arrow heads, as a <see cref="Single" />.  Must be
	/// between <see cref="MinimumRelativeArrowSize" /> and <see
	/// cref="MaximumRelativeArrowSize" />, inclusive.  The default value is 3.
    /// </value>
	///
	/// <remarks>
	/// The value is relative to <see cref="Width" /> and <see
	/// cref="SelectedWidth" />.  If the width or selected width is increased,
	/// the arrow size on unselected or selected edges is increased
	/// proportionally.
	/// </remarks>
    //*************************************************************************

    public Single
    RelativeArrowSize
    {
        get
		{
			AssertValid();

			return (m_fRelativeArrowSize);
		}

		set
		{
			const String PropertyName = "RelativeArrowSize";

			this.ArgumentChecker.CheckPropertyInRange(PropertyName, value,
				MinimumRelativeArrowSize, MaximumRelativeArrowSize);

			if (m_fRelativeArrowSize == value)
			{
				return;
			}

			m_fRelativeArrowSize = value;

			CreateDrawingObjects();

			FireRedrawRequired();

			AssertValid();
		}
    }

    //*************************************************************************
    //  Method: DrawEdgeCore()
    //
    /// <summary>
    /// Draws an edge.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge to draw.
    /// </param>
	///
    /// <param name="vertex1">
    /// The edge's first vertex.
    /// </param>
	///
    /// <param name="vertex2">
    /// The edge's second vertex.
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
	///
	/// <para>
	/// The arguments have already been checked for validity.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    protected override void
    DrawEdgeCore
    (
        IEdge edge,
		IVertex vertex1,
		IVertex vertex2,
		DrawContext drawContext
    )
	{
		Debug.Assert(edge != null);
		Debug.Assert(vertex1 != null);
		Debug.Assert(vertex2 != null);
		Debug.Assert(drawContext != null);
		AssertValid();

		Boolean bShowArrow = m_bDrawArrowOnDirectedEdge && edge.IsDirected;

		Boolean bShowAsSelected = EdgeShouldBeDrawnSelected(edge);

		// Get a pen to use.

		Pen oPen = GetPen(edge, bShowArrow, bShowAsSelected);

		if (edge.IsSelfLoop)
		{
			DrawSelfLoop(oPen, vertex1, drawContext);

			return;
		}

		// Get the edge's endpoints.

		PointF oEndpoint1, oEndpoint2;

		GetEdgeEndpoints(vertex1, vertex2, drawContext,
			out oEndpoint1, out oEndpoint2);

		Graphics oGraphics = drawContext.Graphics;

		oGraphics.DrawLine(oPen, oEndpoint1, oEndpoint2);
	}

	//*************************************************************************
	//  Enum: RectangleEdge
	//
	/// <summary>
	/// Specifies the edges of a rectangle.
	/// </summary>
	//*************************************************************************

	protected enum
	RectangleEdge
	{
		/// <summary>
		/// Left edge.
		/// </summary>

		Left,

		/// <summary>
		/// Right edge.
		/// </summary>

		Right,

		/// <summary>
		/// Top edge.
		/// </summary>

		Top,

		/// <summary>
		/// Bottom edge.
		/// </summary>

		Bottom,
	}

    //*************************************************************************
    //  Method: DrawSelfLoop()
    //
    /// <summary>
    /// Draws an edge that is a self-loop.
    /// </summary>
    ///
    /// <param name="pen">
	/// Pen to use.
    /// </param>
	///
    /// <param name="vertex">
	/// Vertex that the edge connects to itself.
    /// </param>
	///
    /// <param name="drawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
    //*************************************************************************

	protected virtual void
	DrawSelfLoop
	(
		Pen pen,
		IVertex vertex,
		DrawContext drawContext
	)
	{
		Debug.Assert(pen != null);
		Debug.Assert(vertex != null);
		Debug.Assert(drawContext != null);
		AssertValid();

		// To keep things simple, a self-loop is drawn as a Bezier curve that
		// starts at a point on the vertex's circle, arcs out towards the most
		// distant edge of the graph rectangle, and ends at the starting point.
		//
		// Future improvement: Make sure the curve doesn't cross the rectangle
		// boundary.

		// Get the radius of the edge's vertices, if possible.

		Single fVertexRadius;

		if ( !GetVertexRadius(vertex, drawContext, out fVertexRadius) )
		{
			fVertexRadius = 0;
		}

		Int32 iVertexRadius = (Int32)fVertexRadius;

		// Determine which graph rectangle edge is farthest from the vertex.

		Rectangle oGraphRectangle = drawContext.GraphRectangle;

		Point oVertexLocation = Point.Round(vertex.Location);

		Int32 iVertexX = oVertexLocation.X;
		Int32 iVertexY = oVertexLocation.Y;

		Int32 iDistanceFromLeft = iVertexX - oGraphRectangle.Left;
		Int32 iDistanceFromRight = oGraphRectangle.Right - iVertexX;
		Int32 iDistanceFromTop = iVertexY - oGraphRectangle.Top;
		Int32 iDistanceFromBottom = oGraphRectangle.Bottom - iVertexY;

		RectangleEdge eFarthestEdge = RectangleEdge.Left;
		Int32 iGreatestDistance = iDistanceFromLeft;

		if (iDistanceFromRight > iGreatestDistance)
		{
			eFarthestEdge = RectangleEdge.Right;
			iGreatestDistance = iDistanceFromRight;
		}

		if (iDistanceFromTop > iGreatestDistance)
		{
			eFarthestEdge = RectangleEdge.Top;
			iGreatestDistance = iDistanceFromTop;
		}

		if (iDistanceFromBottom > iGreatestDistance)
		{
			eFarthestEdge = RectangleEdge.Bottom;
			iGreatestDistance = iDistanceFromBottom;
		}

		Int32 iPoint0X = 0, iPoint0Y = 0;
		Int32 iPoint1X = 0, iPoint1Y = 0;
		Int32 iPoint2X = 0, iPoint2Y = 0;

		switch (eFarthestEdge)
		{
			case RectangleEdge.Left:

				iPoint0X = iVertexX - iVertexRadius;
				iPoint0Y = iVertexY;

				iPoint1X = iPoint0X - SelfLoopBezierWidth;
				iPoint1Y = iPoint0Y + SelfLoopBezierHeight;

				iPoint2X = iPoint0X - SelfLoopBezierWidth;
				iPoint2Y = iPoint0Y - SelfLoopBezierHeight;

				break;

			case RectangleEdge.Right:

				iPoint0X = iVertexX + iVertexRadius;
				iPoint0Y = iVertexY;

				iPoint1X = iPoint0X + SelfLoopBezierWidth;
				iPoint1Y = iPoint0Y + SelfLoopBezierHeight;

				iPoint2X = iPoint0X + SelfLoopBezierWidth;
				iPoint2Y = iPoint0Y - SelfLoopBezierHeight;

				break;

			case RectangleEdge.Top:

				iPoint0X = iVertexX;
				iPoint0Y = iVertexY - iVertexRadius;

				iPoint1X = iPoint0X + SelfLoopBezierHeight;
				iPoint1Y = iPoint0Y - SelfLoopBezierWidth;

				iPoint2X = iPoint0X - SelfLoopBezierHeight;
				iPoint2Y = iPoint0Y - SelfLoopBezierWidth;

				break;

			case RectangleEdge.Bottom:

				iPoint0X = iVertexX;
				iPoint0Y = iVertexY + iVertexRadius;

				iPoint1X = iPoint0X + SelfLoopBezierHeight;
				iPoint1Y = iPoint0Y + SelfLoopBezierWidth;

				iPoint2X = iPoint0X - SelfLoopBezierHeight;
				iPoint2Y = iPoint0Y + SelfLoopBezierWidth;

				break;

			default:

				Debug.Assert(false);
				break;
		}

		Point oPoint0 = new Point(iPoint0X, iPoint0Y);
		Point oPoint1 = new Point(iPoint1X, iPoint1Y);
		Point oPoint2 = new Point(iPoint2X, iPoint2Y);

		Graphics oGraphics = drawContext.Graphics;

		oGraphics.DrawBezier(pen, oPoint0, oPoint1, oPoint2, oPoint0);
	}

    //*************************************************************************
    //  Method: GetActualColor()
    //
    /// <summary>
    /// Gets the actual color to use for an edge.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge to get the actual color for.
    /// </param>
    ///
    /// <param name="showAsSelected">
    /// true if <paramref name="edge" /> should be shown as selected.
    /// </param>
    ///
	/// <returns>
	/// The actual color to use.
	/// </returns>
    ///
    /// <remarks>
	/// This base class uses either <see cref="Color" /> or <see
	/// cref="SelectedColor" /> as the actual edge color.  A derived class can
	/// use an actual color that differs from this by overriding this virtual
	/// method.
    /// </remarks>
    //*************************************************************************

    protected virtual Color
    GetActualColor
    (
        IEdge edge,
		Boolean showAsSelected
    )
	{
		AssertValid();

		return (showAsSelected ? m_oSelectedColor : m_oColor);
	}

    //*************************************************************************
    //  Method: GetActualWidth()
    //
    /// <summary>
    /// Gets the actual width to use for an edge.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge to get the actual width for.
    /// </param>
    ///
    /// <param name="showAsSelected">
    /// true if <paramref name="edge" /> should be shown as selected.
    /// </param>
    ///
	/// <returns>
	/// The actual width to use.
	/// </returns>
    ///
    /// <remarks>
	/// This base class uses either <see cref="Width" /> or <see
	/// cref="SelectedWidth" /> as the actual edge width.  A derived class can
	/// use an actual width that differs from this by overriding this virtual
	/// method.
    /// </remarks>
    //*************************************************************************

    protected virtual Int32
    GetActualWidth
    (
        IEdge edge,
		Boolean showAsSelected
    )
	{
		AssertValid();

		if (showAsSelected)
		{
			// A selected line must be at least as wide as an unselected line.
			// Furthermore, NetMapControl uses a two-bitmap scheme in which a
			// selected line is displayed by drawing on top of the unselected
			// line.  With anti-aliasing turned on, the selected line width
			// must be GREATER than the unselected line width to hide the anti-
			// aliasing "feathering" at the line edges.

			return ( Math.Max(m_iSelectedWidth, m_iWidth + 1) );
		}

		return (m_iWidth);
	}

    //*************************************************************************
    //  Method: GetPen()
    //
    /// <summary>
    /// Gets a pen to use to draw the edge.
    /// </summary>
	///
    /// <param name="oEdge">
    /// The edge to draw.
    /// </param>
    ///
    /// <param name="bShowArrow">
	/// true if the pen should include an arrow on the front-vertex end of the
	/// edge.
    /// </param>
	///
    /// <param name="bShowAsSelected">
	/// true if the edge should be shown as selected.
    /// </param>
	///
    /// <returns>
	/// The pen to use to draw the edge.
    /// </returns>
    //*************************************************************************

	protected Pen
	GetPen
	(
        IEdge oEdge,
		Boolean bShowArrow,
		Boolean bShowAsSelected
	)
	{
		Debug.Assert(oEdge != null);
		AssertValid();

		Pen oPen = bShowArrow ? m_oDirectedPen : m_oUndirectedPen;

		oPen.Color = GetActualColor(oEdge, bShowAsSelected);
		oPen.Width = GetActualWidth(oEdge, bShowAsSelected);

		return (oPen);
	}

    //*************************************************************************
    //  Method: GetVertexRadius()
    //
    /// <summary>
    /// Gets the radius of one of the edge's vertices, if possible.
    /// </summary>
	///
    /// <param name="oVertex">
	/// One of the edge's vertices.
    /// </param>
    ///
    /// <param name="oDrawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
	///
    /// <param name="fVertexRadius">
	/// Where the radius gets stored if true is returned.
    /// </param>
	///
    /// <returns>
	/// true if the radius was obtained, false if not.
    /// </returns>
	///
    /// <remarks>
	/// If a <see cref="VertexDrawer" /> is being used, this method stores the
	/// actual radius of <paramref name="oVertex" /> at <paramref
	/// name="fVertexRadius" /> and returns true.  false is returned otherwise.
    /// </remarks>
    //*************************************************************************

	protected Boolean
	GetVertexRadius
	(
		IVertex oVertex,
		DrawContext oDrawContext,
		out Single fVertexRadius
	)
	{
		Debug.Assert(oVertex != null);
		Debug.Assert(oDrawContext != null);
		AssertValid();

		IVertexDrawer oVertexDrawer = oDrawContext.GraphDrawer.VertexDrawer;

		if (oVertexDrawer is VertexDrawer)
		{
			fVertexRadius =
				( (VertexDrawer)oVertexDrawer ).GetActualRadius(oVertex);

			return (true);
		}

		fVertexRadius = Single.MinValue;

		return (false);
	}

    //*************************************************************************
    //  Method: GetVertexShape()
    //
    /// <summary>
    /// Gets the shape of one of the edge's vertices, if possible.
    /// </summary>
	///
    /// <param name="oVertex">
	/// One of the edge's vertices.
    /// </param>
    ///
    /// <param name="oDrawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
	///
    /// <param name="eVertexShape">
	/// Where the shape gets stored if true is returned.
    /// </param>
	///
    /// <returns>
	/// true if the shape was obtained, false if not.
    /// </returns>
	///
    /// <remarks>
	/// If a <see cref="VertexDrawer" /> is being used, this method stores its
	/// actual shape at <paramref name="eVertexShape" /> and returns true.
	/// false is returned otherwise.
    /// </remarks>
    //*************************************************************************

	protected Boolean
	GetVertexShape
	(
		IVertex oVertex,
		DrawContext oDrawContext,
		out VertexDrawer.VertexShape eVertexShape
	)
	{
		Debug.Assert(oVertex != null);
		Debug.Assert(oDrawContext != null);
		AssertValid();

		IVertexDrawer oVertexDrawer = oDrawContext.GraphDrawer.VertexDrawer;

		if (oVertexDrawer is VertexDrawer)
		{
			eVertexShape =
				( (VertexDrawer)oVertexDrawer ).GetActualShape(oVertex);

			return (true);
		}

		eVertexShape = VertexDrawer.VertexShape.Circle;

		return (false);
	}

    //*************************************************************************
    //  Method: GetEdgeEndpoints()
    //
    /// <summary>
    /// Gets the endpoints of the edge.
    /// </summary>
    ///
    /// <param name="vertex1">
	/// The edge's first vertex.
    /// </param>
	///
    /// <param name="vertex2">
	/// The edge's second vertex.
    /// </param>
	///
    /// <param name="drawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
	///
    /// <param name="edgeEndpoint1">
	/// Where the edge's first endpoint gets stored.
    /// </param>
	///
    /// <param name="edgeEndpoint2">
	/// Where the edge's second endpoint gets stored.
    /// </param>
	///
    /// <remarks>
	/// The edge's first endpoint is the endpoint on the <paramref
	/// name="vertex1" /> side of the edge.  The edge's second endpoint is the
	/// endpoint on the <paramref name="vertex2" /> side of the edge.
    /// </remarks>
    //*************************************************************************

	protected virtual void
	GetEdgeEndpoints
	(
		IVertex vertex1,
		IVertex vertex2,
		DrawContext drawContext,
		out PointF edgeEndpoint1,
		out PointF edgeEndpoint2
	)
	{
		Debug.Assert(vertex1 != null);
		Debug.Assert(vertex2 != null);
		Debug.Assert(drawContext != null);
		AssertValid();

		edgeEndpoint1 = GetEdgeEndpoint1(vertex1, vertex2, drawContext);
		edgeEndpoint2 = GetEdgeEndpoint2(vertex1, vertex2, drawContext);
	}

    //*************************************************************************
    //  Method: GetEdgeEndpoint1()
    //
    /// <summary>
    /// Gets the first endpoint of the edge.
    /// </summary>
    ///
    /// <param name="oVertex1">
	/// The edge's first vertex.
    /// </param>
	///
    /// <param name="oVertex2">
	/// The edge's second vertex.
    /// </param>
	///
    /// <param name="oDrawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
	///
    /// <returns>
	/// The edge's first endpoint, which is the endpoint on the <paramref
	/// name="oVertex1" /> side of the edge.
    /// </returns>
    //*************************************************************************

	protected PointF
	GetEdgeEndpoint1
	(
		IVertex oVertex1,
		IVertex oVertex2,
		DrawContext oDrawContext
	)
	{
		Debug.Assert(oVertex1 != null);
		Debug.Assert(oVertex2 != null);
		Debug.Assert(oDrawContext != null);

		// Attempt to get the shape of Vertex1.

		VertexDrawer.VertexShape eVertexShape;

		if (GetVertexShape(oVertex1, oDrawContext, out eVertexShape) &&
			eVertexShape == VertexDrawer.VertexShape.Circle)
		{
			// Vertex1 is drawn as a circle.  The first endpoint should be on
			// the circle.

			Single fVertex1Radius;

			if ( GetVertexRadius(oVertex1, oDrawContext, out fVertex1Radius) )
			{
				return ( GetEdgeEndpoint(oVertex2.Location, oVertex1.Location,
					fVertex1Radius) );
			}
		}

		// The first endpoint should be at the center of Vertex1.

		return (oVertex1.Location);
	}

    //*************************************************************************
    //  Method: GetEdgeEndpoint2()
    //
    /// <summary>
    /// Gets the second endpoint of the edge.
    /// </summary>
    ///
    /// <param name="oVertex1">
	/// The edge's first vertex.
    /// </param>
	///
    /// <param name="oVertex2">
	/// The edge's second vertex.
    /// </param>
	///
    /// <param name="oDrawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
	///
    /// <returns>
	/// The edge's second endpoint, which is the endpoint on the <paramref
	/// name="oVertex2" /> side of the edge.
    /// </returns>
    //*************************************************************************

	protected PointF
	GetEdgeEndpoint2
	(
		IVertex oVertex1,
		IVertex oVertex2,
		DrawContext oDrawContext
	)
	{
		Debug.Assert(oVertex1 != null);
		Debug.Assert(oVertex2 != null);
		Debug.Assert(oDrawContext != null);

		Boolean bEndpoint2OnCircle = false;

		if (m_fRelativeArrowSize != 0)
		{
			// An arrow is being drawn.  The second endpoint should be on the
			// circle of Vertex2.

			bEndpoint2OnCircle = true;
		}
		else
		{
			// An arrow isn't being drawn.  Attempt to get the shape of
			// Vertex2.

			VertexDrawer.VertexShape eVertexShape;

			if (GetVertexShape(oVertex2, oDrawContext, out eVertexShape) &&
				eVertexShape == VertexDrawer.VertexShape.Circle)
			{
				// Vertex2 is drawn as a circle.  The second endpoint should be
				// on the circle.

				bEndpoint2OnCircle = true;
			}
		}

		Single fVertex2Radius;

		if (bEndpoint2OnCircle &&
			GetVertexRadius(oVertex2, oDrawContext, out fVertex2Radius)
			)
		{
			// The second endpoint should be on the circle of Vertex2.

			return ( GetEdgeEndpoint(oVertex1.Location, oVertex2.Location,
				fVertex2Radius) );
		}

		return (oVertex2.Location);
	}

    //*************************************************************************
    //  Method: GetEdgeEndpoint()
    //
    /// <summary>
    /// Gets one of the edge's endpoint.
    /// </summary>
    ///
    /// <param name="oVertexALocation">
	/// Location of one of the edge's vertices.
    /// </param>
	///
    /// <param name="oVertexBLocation">
	/// Location of the edge's other vertex.
    /// </param>
	///
    /// <param name="fEndpointDistanceFromVertexB">
	/// Distance from VertexB to the endpoint.
    /// </param>
	///
	/// <returns>
	/// The endpoint on the VertexB end of the edge.  The endpoint is computed
	/// to be <paramref name="fEndpointDistanceFromVertexB" /> units away from
	/// VertexB's location.
	/// </returns>
    //*************************************************************************

	protected PointF
	GetEdgeEndpoint
	(
		PointF oVertexALocation,
		PointF oVertexBLocation,
		Single fEndpointDistanceFromVertexB
	)
	{
		Debug.Assert(fEndpointDistanceFromVertexB >= 0);
		AssertValid();

		Double dVertexAX = oVertexALocation.X;
		Double dVertexAY = oVertexALocation.Y;

		Double dVertexBX = oVertexBLocation.X;
		Double dVertexBY = oVertexBLocation.Y;

		// Compute the angle between the vertices.

		Double dAngle = Math.Atan2(
			dVertexAY - dVertexBY,
			dVertexAX - dVertexBX
			);

		Double dEndpointDistanceFromVertexB =
			(Double)fEndpointDistanceFromVertexB;

		// Compute the point on the line between the two vertices that is
		// dEndpointDistanceFromVertexB units away from VertexB's location.

		Double dVertexBPointX =
			dVertexBX + ( dEndpointDistanceFromVertexB * Math.Cos(dAngle) );

		Double dVertexBPointY =
			dVertexBY + ( dEndpointDistanceFromVertexB * Math.Sin(dAngle) );

		return (
			new PointF( (Single)dVertexBPointX,  (Single)dVertexBPointY) );
	}

    //*************************************************************************
    //  Method: EdgeShouldBeDrawnSelected()
    //
    /// <summary>
    /// Determines whether an edge should be drawn as selected.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The edge to check.
    /// </param>
    ///
    /// <returns>
	/// true if <paramref name="oEdge" /> should be drawn as selected.
    /// </returns>
	///
    /// <remarks>
	/// The selected state of <paramref name="oEdge" /> as well as the <see
	/// cref="UseSelection" /> property are used to determine whether <paramref
	/// name="oEdge" /> should be drawn as selected.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    EdgeShouldBeDrawnSelected
    (
        IEdge oEdge
    )
	{
		Debug.Assert(oEdge != null);
		AssertValid();

		return (m_bUseSelection &&
			MultiSelectionGraphDrawer.EdgeIsSelected(oEdge) );
	}

    //*************************************************************************
    //  Method: GetPointNearVertexRectangle()
    //
    /// <summary>
    /// Gets a point on a vertex rectangle that is near a specified point.
    /// </summary>
    ///
    /// <param name="oPoint">
	/// Specified point.
    /// </param>
	///
    /// <param name="oVertexRectangle">
	/// A vertex rectangle to find a point on.
    /// </param>
	///
    /// <returns>
	/// A point on <paramref name="oVertexRectangle" /> that is near <paramref
	/// name="oPoint" />.
    /// </returns>
	///
	/// <remarks>
	/// This is meant for use by derived classes that draw a vertex as a
	/// rectangle.
	/// </remarks>
    //*************************************************************************

	protected Point
	GetPointNearVertexRectangle
	(
		Point oPoint,
		Rectangle oVertexRectangle
	)
	{
		AssertValid();

		// Get an array of points on the rectangle's edge to consider.

		Point [] aoPointsOnRectangle =
			GetPointsOnVertexRectangle(oVertexRectangle);

		// Loop through all the points to find the shortest distance to oPoint.

		Int32 iMinimumDistanceSquared = Int32.MaxValue;

		Point oNearbyPoint = Point.Empty;

		foreach (Point oPointOnRectangle in aoPointsOnRectangle)
		{
			Int32 iDistanceX = oPoint.X - oPointOnRectangle.X;
			Int32 iDistanceY = oPoint.Y - oPointOnRectangle.Y;

			Int32 iDistanceSquared =
				iDistanceX * iDistanceX + iDistanceY * iDistanceY;

			if (iDistanceSquared < iMinimumDistanceSquared)
			{
				oNearbyPoint = oPointOnRectangle;

				iMinimumDistanceSquared = iDistanceSquared;
			}
		}

		return (oNearbyPoint);
	}

    //*************************************************************************
    //  Method: GetPointsOnVertexRectangle()
    //
    /// <summary>
    /// Gets an array of points on a vertex rectangle's edge to consider as an
	/// edge's second endpoint.
    /// </summary>
    ///
    /// <param name="oVertexRectangle">
	/// Rectangle to get points for.
    /// </param>
	///
    /// <returns>
	/// An array of points to consider as an edge's second endpoint.
    /// </returns>
    //*************************************************************************

	protected Point []
	GetPointsOnVertexRectangle
	(
		Rectangle oVertexRectangle
	)
	{
		AssertValid();

		// Use points 1/4, 1/2, and 3/4 along each edge.

		Int32 iOneQuarterWidth = oVertexRectangle.Width / 4;
		Int32 iHalfWidth = iOneQuarterWidth * 2;
		Int32 iThreeQuarterWidth = iHalfWidth + iOneQuarterWidth;

		Int32 iOneQuarterHeight = oVertexRectangle.Height / 4;
		Int32 iHalfHeight = iOneQuarterHeight * 2;
		Int32 iThreeQuarterHeight = iHalfHeight + iOneQuarterHeight;

		Int32 iTop = oVertexRectangle.Top;
		Int32 iBottom = oVertexRectangle.Bottom;
		Int32 iLeft = oVertexRectangle.Left;
		Int32 iRight = oVertexRectangle.Right;

		Point [] aoPoints = new Point[] {

			new Point(iLeft, iTop + iOneQuarterHeight),
			new Point(iLeft, iTop + iHalfHeight),
			new Point(iLeft, iTop + iThreeQuarterHeight),

			new Point(iRight, iTop + iOneQuarterHeight),
			new Point(iRight, iTop + iHalfHeight),
			new Point(iRight, iTop + iThreeQuarterHeight),

			new Point(iLeft + iOneQuarterWidth, iTop),
			new Point(iLeft + iHalfWidth, iTop),
			new Point(iLeft + iThreeQuarterWidth, iTop),

			new Point(iLeft + iOneQuarterWidth, iBottom),
			new Point(iLeft + iHalfWidth, iBottom),
			new Point(iLeft + iThreeQuarterWidth, iBottom),
			};

		return (aoPoints);
	}

    //*************************************************************************
    //  Method: DrawSelfLoopOnVertexRectangle()
    //
    /// <summary>
    /// Draws an edge that is a self-loop on a vertex drawn as a rectangle.
    /// </summary>
    ///
    /// <param name="oPen">
	/// Pen to use.
    /// </param>
	///
    /// <param name="oVertex">
	/// Vertex that the edge connects to itself.
    /// </param>
	///
    /// <param name="oVertexRectangle">
	/// The vertex's rectangle.
    /// </param>
	///
    /// <param name="oDrawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
	///
	/// <remarks>
	/// This is meant for use by derived classes that draw a vertex as a
	/// rectangle.
	/// </remarks>
    //*************************************************************************

	protected void
	DrawSelfLoopOnVertexRectangle
	(
		Pen oPen,
		IVertex oVertex,
		Rectangle oVertexRectangle,
		DrawContext oDrawContext
	)
	{
		Debug.Assert(oPen != null);
		Debug.Assert(oVertex != null);
		Debug.Assert(oDrawContext != null);
		AssertValid();

		// To keep things simple, a self-loop is drawn as a Bezier curve that
		// starts at a point at the midpoint of one of the vertex's edges, arcs
		// out towards the most distant edge of the graph rectangle, and ends
		// near the starting point.
		//
		// Future improvement: Make sure the curve doesn't cross the rectangle
		// boundary.

		// Determine which graph rectangle edge is farthest from the vertex
		// rectangle.

		Rectangle oGraphRectangle = oDrawContext.GraphRectangle;

		Int32 iDistanceFromLeft = oVertexRectangle.Left - oGraphRectangle.Left;

		Int32 iDistanceFromRight =
			oGraphRectangle.Right - oVertexRectangle.Right;

		Int32 iDistanceFromTop = oVertexRectangle.Top - oGraphRectangle.Top;

		Int32 iDistanceFromBottom =
			oGraphRectangle.Bottom - oVertexRectangle.Bottom;

		RectangleEdge eFarthestEdge = RectangleEdge.Left;
		Int32 iGreatestDistance = iDistanceFromLeft;

		if (iDistanceFromRight > iGreatestDistance)
		{
			eFarthestEdge = RectangleEdge.Right;
			iGreatestDistance = iDistanceFromRight;
		}

		if (iDistanceFromTop > iGreatestDistance)
		{
			eFarthestEdge = RectangleEdge.Top;
			iGreatestDistance = iDistanceFromTop;
		}

		if (iDistanceFromBottom > iGreatestDistance)
		{
			eFarthestEdge = RectangleEdge.Bottom;
			iGreatestDistance = iDistanceFromBottom;
		}

		Int32 fPoint0X = 0, fPoint0Y = 0;
		Int32 fPoint1X = 0, fPoint1Y = 0;
		Int32 fPoint2X = 0, fPoint2Y = 0;
		Int32 fPoint3X = 0, fPoint3Y = 0;

		switch (eFarthestEdge)
		{
			case RectangleEdge.Left:

				fPoint0X = oVertexRectangle.Left;
				fPoint0Y = oVertexRectangle.Top + oVertexRectangle.Height / 2;

				fPoint1X = fPoint0X - RectangleSelfLoopBezierWidth;
				fPoint1Y = fPoint0Y + RectangleSelfLoopBezierHeight;

				fPoint2X = fPoint0X - RectangleSelfLoopBezierWidth;
				fPoint2Y = fPoint0Y - RectangleSelfLoopBezierHeight;

				fPoint3X = fPoint0X;
				fPoint3Y = fPoint0Y - RectangleSelfLoopBezierPoint3Offset;

				break;

			case RectangleEdge.Right:

				fPoint0X = oVertexRectangle.Right;
				fPoint0Y = oVertexRectangle.Top + oVertexRectangle.Height / 2;

				fPoint1X = fPoint0X + RectangleSelfLoopBezierWidth;
				fPoint1Y = fPoint0Y + RectangleSelfLoopBezierHeight;

				fPoint2X = fPoint0X + RectangleSelfLoopBezierWidth;
				fPoint2Y = fPoint0Y - RectangleSelfLoopBezierHeight;

				fPoint3X = fPoint0X;
				fPoint3Y = fPoint0Y - RectangleSelfLoopBezierPoint3Offset;

				break;

			case RectangleEdge.Top:

				fPoint0X = oVertexRectangle.Left + oVertexRectangle.Width / 2;
				fPoint0Y = oVertexRectangle.Top;

				fPoint1X = fPoint0X + RectangleSelfLoopBezierHeight;
				fPoint1Y = fPoint0Y - RectangleSelfLoopBezierWidth;

				fPoint2X = fPoint0X - RectangleSelfLoopBezierHeight;
				fPoint2Y = fPoint0Y - RectangleSelfLoopBezierWidth;

				fPoint3X = fPoint0X - RectangleSelfLoopBezierPoint3Offset;
				fPoint3Y = fPoint0Y;

				break;

			case RectangleEdge.Bottom:

				fPoint0X = oVertexRectangle.Left + oVertexRectangle.Width / 2;
				fPoint0Y = oVertexRectangle.Bottom;

				fPoint1X = fPoint0X + RectangleSelfLoopBezierHeight;
				fPoint1Y = fPoint0Y + RectangleSelfLoopBezierWidth;

				fPoint2X = fPoint0X - RectangleSelfLoopBezierHeight;
				fPoint2Y = fPoint0Y + RectangleSelfLoopBezierWidth;

				fPoint3X = fPoint0X - RectangleSelfLoopBezierPoint3Offset;
				fPoint3Y = fPoint0Y;

				break;

			default:

				Debug.Assert(false);
				break;
		}

		Point oPoint0 = new Point(fPoint0X, fPoint0Y);
		Point oPoint1 = new Point(fPoint1X, fPoint1Y);
		Point oPoint2 = new Point(fPoint2X, fPoint2Y);
		Point oPoint3 = new Point(fPoint3X, fPoint3Y);

		Graphics oGraphics = oDrawContext.Graphics;

		oGraphics.DrawBezier(oPen, oPoint0, oPoint1, oPoint2, oPoint3);
	}

	//*************************************************************************
	//	Method: CreateDrawingObjects()
	//
	/// <summary>
	/// Creates the objects used to draw edges.
	/// </summary>
	//*************************************************************************

	protected void
	CreateDrawingObjects()
	{
		// AssertValid();

		DisposeManagedObjects();

		// Two pens are used -- one for directed edges and one for undirected
		// edges.  The directed pen uses an AdjustableArrowCap to show an arrow
		// on the front-vertex end of the edge.
		//
		// An alternative scheme would be to use a single pen and change its
		// CustomEndCap to either a CustomLineCap or null as needed, but
		// setting the Pen.CustomEndCap to null throws an ArgumentException.

		// Note that the pens are created with the unselected color, but the
		// color may get changed to the selected color before a pen is used.

		m_oUndirectedPen = new Pen(m_oColor);

		m_oDirectedPen = new Pen(m_oColor);

		m_oDirectedEndCap = new AdjustableArrowCap(
			m_fRelativeArrowSize, m_fRelativeArrowSize);

		m_oDirectedPen.CustomEndCap = m_oDirectedEndCap;

		AssertValid();
	}

	//*************************************************************************
	//	Method: DisposeManagedObjects()
	//
	/// <summary>
	/// Disposes of managed objects used for drawing.
	/// </summary>
	//*************************************************************************

	protected override void
	DisposeManagedObjects()
	{
		// AssertValid();

        base.DisposeManagedObjects();

		GraphicsUtil.DisposePen(ref m_oUndirectedPen);
		GraphicsUtil.DisposePen(ref m_oDirectedPen);

		if (m_oDirectedEndCap != null)
		{
			m_oDirectedEndCap.Dispose();
			m_oDirectedEndCap = null;
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

		// m_bUseSelection

		Debug.Assert(m_iWidth >= MinimumWidth);
		Debug.Assert(m_iWidth <= MaximumWidth);

		Debug.Assert(m_iSelectedWidth >= MinimumWidth);
		Debug.Assert(m_iSelectedWidth <= MaximumWidth);

		// m_bDrawArrowOnDirectedEdge
		Debug.Assert(m_fRelativeArrowSize >= MinimumRelativeArrowSize);
		Debug.Assert(m_fRelativeArrowSize <= MaximumRelativeArrowSize);

		// m_oColor
		// m_oSelectedColor
		Debug.Assert(m_oUndirectedPen != null);
		Debug.Assert(m_oDirectedPen != null);
		Debug.Assert(m_oDirectedEndCap != null);
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

	/// <summary>
	/// Minimum value of the <see cref="Width" /> and <see
	/// cref="SelectedWidth" /> properties.  The value is 1.
	/// </summary>

	public static Int32 MinimumWidth = 1;

	/// <summary>
	/// Maximum value of the <see cref="Width" /> and <see
	/// cref="SelectedWidth" /> properties.  The value is 20.
	/// </summary>

	public static Int32 MaximumWidth = 20;

	/// <summary>
	/// Minimum value of the <see cref="RelativeArrowSize" /> property.  The
	/// value is 0.
	/// </summary>

	public static Int32 MinimumRelativeArrowSize = 0;

	/// <summary>
	/// Maximum value of the <see cref="RelativeArrowSize" /> property.  The
	/// value is 20.
	/// </summary>

	public static Int32 MaximumRelativeArrowSize = 20;


    //*************************************************************************
    //  Self-loop protected constants used by DrawSelfLoop()
    //*************************************************************************

	/// Offsets of the control points of the Bezier curve used to draw a
	/// self-loop.

	protected const Int32 SelfLoopBezierWidth = 60;
	///
	protected const Int32 SelfLoopBezierHeight = 40;


    //*************************************************************************
    //  Self-loop protected constants used by DrawSelfLoopOnVertexRectangle()
    //*************************************************************************

	/// Offsets of the control points of the Bezier curve used to draw a
	/// self-loop on a rectangular vertex.

	protected const Int32 RectangleSelfLoopBezierWidth = 30;
	///
	protected const Int32 RectangleSelfLoopBezierHeight = 20;

	/// Offset of point 3 from point 0 in the Bezier curve used to draw a self-
	/// loop on a rectangular vertex.

	protected const Int32 RectangleSelfLoopBezierPoint3Offset = 8;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// true if edges marked as selected should be drawn as selected, false to
	/// draw all edges as unselected.

	protected Boolean m_bUseSelection;

    /// Width of an unselected edge.

	protected Int32 m_iWidth;

    /// Width of a selected edge.

	protected Int32 m_iSelectedWidth;

	/// true to draw an arrow on directed edges.

	protected Boolean m_bDrawArrowOnDirectedEdge;

	/// Width and height of the arrow drawn by m_oDirectedEndCap.  Note that
	/// these dimensions get automatically scaled to the pen width; see the
	/// AdjustableArrowCap.WidthScale property.

	protected Single m_fRelativeArrowSize;

	/// Color of an unselected edge.

	protected Color m_oColor;

	/// Color of a selected edge.

	protected Color m_oSelectedColor;

	/// Pen used to draw undirected edges.

	protected Pen m_oUndirectedPen;

	/// Pen used to draw directed edges.

	protected Pen m_oDirectedPen;

	/// Arrow line cap to use on the front-vertex end of lines drawn by
	/// m_oDirectedPen.

	protected CustomLineCap m_oDirectedEndCap;
}

}
