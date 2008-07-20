
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
//  Class: VertexDrawer
//
/// <summary>
///	Draws a vertex as a shape.
/// </summary>
///
/// <remarks>
///	This class draws a vertex as a circle, disk, or sphere centered on the
///	vertex's <see cref="IVertex.Location" />.  It is typically used with <see
/// cref="EdgeDrawer" />, which takes the <see cref="Radius" /> into account
/// when edges are drawn, although other edge drawers can be used as well.
///
/// <para>
/// This class works in conjunction with <see
/// cref="MultiSelectionGraphDrawer" /> to draw vertices as either selected or
/// unselected.  If a vertex has been marked as selected by <see
/// cref="MultiSelectionGraphDrawer.SelectVertex(IVertex)" />, it is drawn
/// using <see cref="SelectedColor" />.  Otherwise, <see cref="Color" /> is
/// used.  Set the <see cref="UseSelection" /> property to false to force all
/// vertices to be drawn as unselected.
/// </para>
///
/// <para>
/// If this class is used by a graph drawer other than <see
/// cref="MultiSelectionGraphDrawer" />, all vertices are drawn as unselected.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class VertexDrawer : VertexDrawerBase, IVertexDrawer
{
    //*************************************************************************
    //  Constructor: VertexDrawer()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexDrawer" /> class.
    /// </summary>
    //*************************************************************************

    public VertexDrawer()
    {
		m_bUseSelection = true;

		m_eShape = VertexShape.Disk;

		m_fRadius = 3.0F;

		m_oColor = SystemColors.WindowText;

		m_oSelectedColor = Color.Red;

		m_oOutlinePen = null;

		m_oSharedSolidBrush = null;

		CreateDrawingObjects();

		// AssertValid();
    }

	//*************************************************************************
	//  Enum: VertexShape
	//
	/// <summary>
	/// Specifies the shape of the vertices.
	/// </summary>
	//*************************************************************************

	public enum
	VertexShape
	{
		/// <summary>
		/// The vertices are drawn as circles.
		/// </summary>

		Circle = 0,

		/// <summary>
		/// The vertices are drawn as disks.
		/// </summary>

		Disk = 1,

		/// <summary>
		/// The vertices are drawn as spheres.
		/// </summary>

		Sphere = 2,
	}

    //*************************************************************************
    //  Property: UseSelection
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the selected state of a vertex
	/// should be used.
    /// </summary>
    ///
    /// <value>
	/// If true, a vertex is drawn using either <see cref="Color" /> or <see
	/// cref="SelectedColor" />, depending on whether the vertex has been
	/// marked as selected by <see
	/// cref="MultiSelectionGraphDrawer.SelectVertex(IVertex)" />.  If false,
	/// <see cref="Color" /> is used regardless of whether the vertex has been
	/// marked as selected.
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
    //  Property: Shape
    //
    /// <summary>
    /// Gets or sets the shape of the vertices.
    /// </summary>
    ///
    /// <value>
	///	The shape of the vertices, as a <see cref="VertexShape" />.  The
	/// default value is <see cref="VertexShape.Disk" />.
    /// </value>
    //*************************************************************************

    public VertexShape
    Shape
    {
        get
		{
			AssertValid();

			return (m_eShape);
		}

		set
		{
			const String PropertyName = "Shape";

			this.ArgumentChecker.CheckPropertyIsDefined(
				PropertyName, value, typeof(VertexShape) );

			if (m_eShape == value)
			{
				return;
			}

			m_eShape = value;

			FireRedrawRequired();

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: Color
    //
    /// <summary>
    /// Gets or sets the color of a vertex that is not selected.
    /// </summary>
    ///
    /// <value>
	///	The color of an unselected vertex, as a <see
	/// cref="System.Drawing.Color" />.  The default value is <see
	///	cref="SystemColors.WindowText" />.
    /// </value>
	///
	/// <remarks>
	/// See <see cref="UseSelection" /> for details on selected vs. unselected
	/// vertices.
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
    /// Gets or sets the color of a vertex that is selected.
    /// </summary>
    ///
    /// <value>
	///	The color of a selected vertex, as a <see
	/// cref="System.Drawing.Color" />.  The default value is <see
	///	cref="System.Drawing.Color.Red" />.
    /// </value>
	///
	/// <remarks>
	/// See <see cref="UseSelection" /> for details on selected vs. unselected
	/// vertices.
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
    //  Property: Radius
    //
    /// <summary>
    /// Gets or sets the radius of the vertices.
    /// </summary>
    ///
    /// <value>
	///	The radius of the vertices, as a <see cref="Single" />.  Must be
	/// between <see cref="MinimumRadius" /> and <see cref="MaximumRadius" />,
	/// inclusive.  The default value is 3.0.
    /// </value>
	///
	/// <remarks>
	/// This base class uses <see cref="Radius" /> as the actual vertex radius.
	/// A derived class can use an actual radius that differs from <see
	/// cref="Radius" /> by overriding the virtual <see
	/// cref="GetActualRadius" /> method.
	///
	/// <para>
	/// The units are determined by the <see cref="DrawContext.Graphics" />
	/// object passed to the <see cref="VertexDrawerBase.DrawVertex" /> method.
	/// </para>
	///
	/// </remarks>
    //*************************************************************************

    public Single
    Radius
    {
        get
		{
			AssertValid();

			return (m_fRadius);
		}

		set
		{
			const String PropertyName = "Radius";

			this.ArgumentChecker.CheckPropertyInRange(PropertyName, value,
				MinimumRadius, MaximumRadius);

			if (m_fRadius == value)
			{
				return;
			}

			m_fRadius = value;

			// The layout may depend on the vertex size, so fire
			// LayoutRequired, not RedrawRequired.

			FireLayoutRequired();

			AssertValid();
		}
    }

    //*************************************************************************
    //  Method: GetActualShape()
    //
    /// <summary>
    /// Gets the actual shape to use for a vertex.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex to get the actual shape for.
    /// </param>
    ///
	/// <returns>
	/// The actual shape to use.
	/// </returns>
    ///
    /// <remarks>
	/// This base class uses <see cref="Shape" /> as the actual vertex shape.
	/// A derived class can use an actual shape that differs from this by
	/// overriding this virtual method.
    /// </remarks>
    //*************************************************************************

    public virtual VertexShape
    GetActualShape
    (
        IVertex vertex
    )
	{
		AssertValid();

		return (m_eShape);
	}

    //*************************************************************************
    //  Method: GetActualRadius()
    //
    /// <summary>
    /// Gets the actual radius to use for a vertex.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex to get the actual radius for.
    /// </param>
    ///
	/// <returns>
	/// The actual radius to use.
	/// </returns>
    ///
    /// <remarks>
	/// This base class uses <see cref="Radius" /> as the actual vertex radius.
	/// A derived class can use an actual radius that differs from <see
	/// cref="Radius" /> by overriding this virtual method.
	///
	/// <para>
	/// A virtual <see cref="GetActualRadius" /> method is used instead of just
	/// making the <see cref="Radius" /> property virtual because the actual
	/// radius may require an <see cref="IVertex" /> for its implementation,
	/// and the <see cref="Radius" /> property does not have access to an <see
	/// cref="IVertex" />.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    public virtual Single
    GetActualRadius
    (
        IVertex vertex
    )
	{
		AssertValid();

		return (m_fRadius);
	}

    //*************************************************************************
    //  Method: GetActualColor()
    //
    /// <summary>
    /// Gets the actual color to use for a vertex.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex to get the actual color for.
    /// </param>
    ///
    /// <param name="showAsSelected">
    /// true if <paramref name="vertex" /> should be shown as selected.
    /// </param>
    ///
	/// <returns>
	/// The actual color to use.
	/// </returns>
    ///
    /// <remarks>
	/// This base class uses either <see cref="Color" /> or <see
	/// cref="SelectedColor" /> as the actual vertex color.  A derived class
	/// can use an actual color that differs from this by overriding this
	/// virtual method.
    /// </remarks>
    //*************************************************************************

    protected virtual Color
    GetActualColor
    (
        IVertex vertex,
		Boolean showAsSelected
    )
	{
		AssertValid();

		return (showAsSelected ? m_oSelectedColor : m_oColor);
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

    protected override Rectangle
    DrawVertexCore
    (
        IVertex vertex,
		DrawContext drawContext
    )
	{
		Debug.Assert(vertex != null);
		Debug.Assert(drawContext != null);
		AssertValid();

		PointF oLocation = vertex.Location;

		Single fX = oLocation.X;
		Single fY = oLocation.Y;

		// Determine which color to use.

		Boolean bDrawSelected = VertexShouldBeDrawnSelected(vertex);

		Color oActualColor = GetActualColor(vertex, bDrawSelected);

		Graphics oGraphics = drawContext.Graphics;

		Debug.Assert(oGraphics != null);

		Single fActualRadius = this.GetActualRadius(vertex);

		Int32 iBoundsRadius = (Int32)Math.Ceiling(fActualRadius);

		switch ( this.GetActualShape(vertex) )
		{
			case VertexShape.Circle:

				m_oOutlinePen.Color = oActualColor;

				m_oOutlinePen.Width =
					bDrawSelected ? SelectedOutlinePenWidth : OutlinePenWidth;

				m_oOutlinePen.Alignment = PenAlignment.Center;

				GraphicsUtil.DrawCircle(oGraphics, m_oOutlinePen, fX, fY,
					fActualRadius);

				break;

			case VertexShape.Disk:

				if (bDrawSelected)
				{
					// NetMapControl uses a two-bitmap scheme in which a
					// selected vertex is displayed by drawing on top of the
					// unselected vertex.  With anti-aliasing turned on, the
					// selected vertex radius must be greater than the
					// unselected radius to hide the anti-aliasing "feathering"
					// at the vertex edges.

					fActualRadius += 0.5F;
				}

				GraphicsUtil.FillCircle(oGraphics,
					GetSharedSolidBrush(oActualColor), fX, fY, fActualRadius);

				break;

			case VertexShape.Sphere:

				GraphicsUtil.FillCircle3D(oGraphics, oActualColor, fX, fY,
					fActualRadius);

				break;

			default:

				Debug.Assert(false);

				break;
		}

		// If the vertex isn't hidden, add a hit-test area to the vertex's
		// metadata.  This gets used by VertexContainsPoint().

		if (oActualColor.A != 0)
		{
			SetHitTestArea( vertex,
				new CircularHitTestArea(oLocation, fActualRadius) );
		}

		Rectangle oVertexBounds = Rectangle.FromLTRB(
			(Int32)Math.Floor(fX - iBoundsRadius),
			(Int32)Math.Floor(fY - iBoundsRadius),
			(Int32)Math.Ceiling(fX + iBoundsRadius),
			(Int32)Math.Ceiling(fY + iBoundsRadius)
			);

		return (oVertexBounds);
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

    protected override Boolean
    VertexContainsPointCore
    (
        IVertex vertex,
		Point point
    )
	{
		Debug.Assert(vertex != null);
		AssertValid();

		// This vertex drawer and all vertex drawers derived from it store a
		// hit-test area in the vertex's metadata when the vertex is drawn.

		HitTestArea oHitTestArea;

		if ( !TryGetHitTestArea(vertex, out oHitTestArea) )
		{
			return (false);
		}

		return ( oHitTestArea.Contains(point) );
	}

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

    protected override Boolean
    VertexIntersectsWithRectangleCore
    (
        IVertex vertex,
		Rectangle rectangle
    )
	{
		Debug.Assert(vertex != null);
		AssertValid();

		// This vertex drawer and all vertex drawers derived from it store a
		// hit-test area in the vertex's metadata when the vertex is drawn.

		HitTestArea oHitTestArea;

		if ( !TryGetHitTestArea(vertex, out oHitTestArea) )
		{
			return (false);
		}

		return ( oHitTestArea.IntersectsWith(rectangle) );
	}

    //*************************************************************************
    //  Method: DrawVertexOutline()
    //
    /// <summary>
    /// Draws an outline around a rectangular vertex given the vertex's
	/// outline rectangle.
    /// </summary>
    ///
    /// <param name="oDrawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
    ///
    /// <param name="oOutlineRectangle">
    /// The outline rectangle to draw.
    /// </param>
    ///
    /// <param name="bDrawSelected">
	/// true if the vertex should be drawn as selected.
    /// </param>
	///
    /// <remarks>
    /// This method can be used by derived classes that draw vertices as
	/// rectangles.  It draws an outline around the specified rectangle.  The
	/// outline is drawn with <see cref="SelectedColor" /> and <see
	/// cref="SelectedOutlinePenWidth" /> if the vertex should be drawn as
	/// selected, or <see cref="Color" /> and <see cref="OutlinePenWidth" /> if
	/// not.
    /// </remarks>
    //*************************************************************************

    protected void
    DrawVertexOutline
    (
		DrawContext oDrawContext,
		Rectangle oOutlineRectangle,
		Boolean bDrawSelected
    )
	{
		Debug.Assert(oDrawContext != null);
		AssertValid();

		Graphics oGraphics = oDrawContext.Graphics;

		m_oOutlinePen.Color = bDrawSelected ? this.SelectedColor : this.Color;

		m_oOutlinePen.Width = bDrawSelected ? SelectedOutlinePenWidth :
			OutlinePenWidth;

		m_oOutlinePen.Alignment = PenAlignment.Inset;

		// Adjust for a GDI+ oddity.  If the rectangle width is N and the pen
		// width is 2 or greater, Graphics.DrawRectangle() will draw a
		// rectangle of width N.  If the pen width is 1, however, it will draw
		// a rectangle of width N+1.  The same holds for the height.

		if (m_oOutlinePen.Width == 1)
		{
			oOutlineRectangle = new Rectangle(
				oOutlineRectangle.Left,
				oOutlineRectangle.Top,
				oOutlineRectangle.Width - 1,
				oOutlineRectangle.Height - 1
				);
		}

		oGraphics.DrawRectangle(m_oOutlinePen, oOutlineRectangle);
	}

    //*************************************************************************
    //  Method: AdjustVertexLocation()
    //
    /// <summary>
    /// If necessary, adjusts the vertex location so the vertex doesn't get
	/// clipped by the graph rectangle.
    /// </summary>
	///
    /// <param name="oVertex">
    /// The vertex to adjust.
    /// </param>
    ///
    /// <param name="oDrawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
    ///
    /// <param name="oVertexBounds">
	/// Rectangle that contains the vertex.
    /// </param>
    //*************************************************************************

	protected void
	AdjustVertexLocation
	(
		IVertex oVertex,
		DrawContext oDrawContext,
		Rectangle oVertexBounds
	)
	{
		Debug.Assert(oVertex != null);
		Debug.Assert(oDrawContext != null);
		AssertValid();

		Rectangle oGraphRectangleMinusMargin =
			oDrawContext.GraphRectangleMinusMargin;

		if (oGraphRectangleMinusMargin.IsEmpty)
		{
			return;
		}

		Single fX = oVertex.Location.X;
		Single fY = oVertex.Location.Y;

		Int32 iX = oGraphRectangleMinusMargin.Right - oVertexBounds.Right;

		if (iX < 0)
		{
			fX += iX;
		}

		iX = oGraphRectangleMinusMargin.Left - oVertexBounds.Left;

		if (iX > 0)
		{
			fX += iX;
		}

		Int32 iY = oGraphRectangleMinusMargin.Bottom - oVertexBounds.Bottom;

		if (iY < 0)
		{
			fY += iY;
		}

		iY = oGraphRectangleMinusMargin.Top - oVertexBounds.Top;

		if (iY > 0)
		{
			fY += iY;
		}

		oVertex.Location = new PointF(fX, fY);
	}

    //*************************************************************************
    //  Method: SetHitTestArea()
    //
    /// <summary>
	/// Adds a hit-test area to the vertex's metadata.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to add a hit-test area to.
    /// </param>
    ///
    /// <param name="oHitTestArea">
	/// Hit-test area.
    /// </param>
	///
    /// <remarks>
    /// This method adds a hit-test area to the vertex's metadata.  The area
	/// can be retrieved by <see cref="TryGetHitTestArea" />.
	/// </remarks>
    //*************************************************************************

	protected void
	SetHitTestArea
	(
		IVertex oVertex,
		HitTestArea oHitTestArea
	)
	{
		Debug.Assert(oVertex != null);
		Debug.Assert(oHitTestArea != null);
		AssertValid();

		oVertex.SetValue(ReservedMetadataKeys.HitTestArea, oHitTestArea);
	}

    //*************************************************************************
    //  Method: SetRectangularHitTestArea()
    //
    /// <summary>
	/// Adds a rectangular hit-test area to the vertex's metadata.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to add a hit-test area to.
    /// </param>
    ///
    /// <param name="oHitTestRectangle">
	/// Hit-test rectangle.
    /// </param>
	///
    /// <remarks>
    /// This method can be used by derived classes that draw vertices as
	/// rectangles.  It adds a rectangular hit-test area to the vertex's
	/// metadata.  The area is retrieved by <see
	/// cref="VertexContainsPointCore" />.
	/// </remarks>
    //*************************************************************************

	protected void
	SetRectangularHitTestArea
	(
		IVertex oVertex,
		Rectangle oHitTestRectangle
	)
	{
		Debug.Assert(oVertex != null);
		AssertValid();

		SetHitTestArea( oVertex,
			new RectangularHitTestArea(oHitTestRectangle) );
	}

	//*************************************************************************
	//	Method: TryGetHitTestArea()
	//
	/// <summary>
	/// Attempts to get the hit-test area for a vertex.
	/// </summary>
	///
	/// <param name="oVertex">
	/// Vertex to get a hit-test area for.
	/// </param>
	///
	/// <param name="oHitTestArea">
	/// Where the hit-test area gets stored if true is returned.
	/// </param>
	///
	/// <returns>
	/// true if <paramref name="oHitTestArea" /> was obtained.
	/// </returns>
	///
	/// <remarks>
	/// This method attempts to retrieve the <see cref="HitTestArea" /> object
	/// that may have been stored in a vertex's metadata by <see
	/// cref="SetHitTestArea" />.
	/// </remarks>
	//*************************************************************************

	protected Boolean
	TryGetHitTestArea
	(
		IVertex oVertex,
		out HitTestArea oHitTestArea
	)
	{
		Debug.Assert(oVertex != null);
		AssertValid();

		oHitTestArea = null;

		Object oObject;

		if ( oVertex.TryGetValue(ReservedMetadataKeys.HitTestArea,
			typeof(HitTestArea), out oObject) )
		{
			oHitTestArea = (HitTestArea)oObject;

			return (true);
		}

		return (false);
	}

    //*************************************************************************
    //  Method: VertexShouldBeDrawnSelected()
    //
    /// <summary>
    /// Determines whether a vertex should be drawn as selected.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to check.
    /// </param>
    ///
    /// <returns>
	/// true if <paramref name="oVertex" /> should be drawn as selected.
    /// </returns>
	///
    /// <remarks>
	/// The selected state of <paramref name="oVertex" /> as well as the <see
	/// cref="UseSelection" /> property are used to determine whether <paramref
	/// name="oVertex" /> should be drawn as selected.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    VertexShouldBeDrawnSelected
    (
        IVertex oVertex
    )
	{
		Debug.Assert(oVertex != null);
		AssertValid();

		return (m_bUseSelection &&
			MultiSelectionGraphDrawer.VertexIsSelected(oVertex) );
	}

    //*************************************************************************
    //  Method: GetSharedSolidBrush()
    //
    /// <summary>
    /// Gets a solid brush shared by many drawing operations.
    /// </summary>
    ///
    /// <param name="oColor">
    /// Color to set the brush to.
    /// </param>
    ///
	/// <returns>
	/// A shared solid brush.  The brush must not be disposed.
	/// </returns>
    ///
    /// <remarks>
	/// The returned brush is shared by many drawing operations and must not be
	/// disposed.  It is an alternative to repeatedly creating and then
	/// disposing a solid brush for each drawing operation.
    /// </remarks>
    //*************************************************************************

    protected SolidBrush
    GetSharedSolidBrush
    (
		Color oColor
    )
	{
		AssertValid();

		m_oSharedSolidBrush.Color = oColor;

		return (m_oSharedSolidBrush);
	}

	//*************************************************************************
	//	Method: CreateDrawingObjects()
	//
	/// <summary>
	/// Creates the objects used to draw vertices.
	/// </summary>
	//*************************************************************************

	protected void
	CreateDrawingObjects()
	{
		// AssertValid();

		DisposeManagedObjects();

		// Although the pen and brush are created with the unselected color,
		// they may get changed to the selected color before they are used.

		m_oOutlinePen = new Pen(m_oColor);

		m_oSharedSolidBrush = new SolidBrush(m_oColor);

		// AssertValid();
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

		GraphicsUtil.DisposePen(ref m_oOutlinePen);
		GraphicsUtil.DisposeSolidBrush(ref m_oSharedSolidBrush);
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
		// m_eShape

		Debug.Assert(m_fRadius >= MinimumRadius);
		Debug.Assert(m_fRadius <= MaximumRadius);

		// m_oColor
		// m_oSelectedColor

		Debug.Assert(m_oOutlinePen != null);
		Debug.Assert(m_oSharedSolidBrush != null);
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

	// Note: A radius of 0.0 doesn't work with GraphicsUtil.FillCircle3D(),
	// because you can't create a PathGradientBrush across a circle of zero
	// radius.  Make the minimum 0.1.

	/// <summary>
	/// Minimum value of the <see cref="Radius" /> property.  The value is 0.1.
	/// </summary>

	public static Single MinimumRadius = 0.1F;

	/// <summary>
	/// Maximum value of the <see cref="Radius" /> property.  The value is
	/// 50.0.
	/// </summary>

	public static Single MaximumRadius = 50.0F;


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	/// Width of the pen used to draw an unselected outline.

	protected const Int32 OutlinePenWidth = 1;

	/// Width of the pen used to draw a selected outline.

	protected const Int32 SelectedOutlinePenWidth = 3;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// true if vertices marked as selected should be drawn as selected, false
	/// to draw all vertices as unselected.

	protected Boolean m_bUseSelection;

	/// Shape of the vertex.

	protected VertexShape m_eShape;

    /// Radius of an unselected vertex.

	protected Single m_fRadius;

	/// Color of an unselected vertex.

	protected Color m_oColor;

	/// Color of a selected vertex.

	protected Color m_oSelectedColor;

	/// Pen used to draw outlines.

	protected Pen m_oOutlinePen;

	/// Brush used for various drawing operations.

	protected SolidBrush m_oSharedSolidBrush;
}

}
