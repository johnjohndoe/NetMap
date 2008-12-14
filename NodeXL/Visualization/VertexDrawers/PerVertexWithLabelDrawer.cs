
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Visualization
{
//*****************************************************************************
//  Class: PerVertexWithLabelDrawer
//
/// <summary>
///	Draws a vertex as one or two labels.
/// </summary>
///
/// <remarks>
/// By default, this class delegates vertex drawing to its base class.  If you
/// add appropriate metadata to a vertex, however, this class uses the metadata
/// to draw a text label instead of whatever the base class would draw, or a
/// text label in addition to whatever the base class draws, or both.
///
/// <para>
/// If a vertex has the <see
/// cref="ReservedMetadataKeys.PerVertexPrimaryLabel" /> key, the vertex is
/// drawn as a rectangle containing the text specified by the key's value.  If
/// a vertex has the <see
/// cref="ReservedMetadataKeys.PerVertexSecondaryLabel" /> key, the vertex is
/// drawn by the base class and then annotated with the text specified by the
/// key's value.  If a vertex has both keys, both labels are drawn.
/// </para>
///
/// <para>
/// If a vertex has the <see
/// cref="ReservedMetadataKeys.PerVertexPrimaryLabel" /> and <see
/// cref="ReservedMetadataKeys.PerVertexPrimaryLabelFillColor" /> keys, the
/// specified fill color is used to fill the primary label rectangle.  The
/// default fill color is <see cref="PrimaryLabelFillColor" />.
/// </para>
///
/// <para>
/// To force this class to ignore the <see
/// cref="ReservedMetadataKeys.PerVertexPrimaryLabel" /> key and delegate
/// drawing to the base class, add a <see
/// cref="ReservedMetadataKeys.PerVertexDrawerPrecedence" /> key and set its
/// value to anything except <see
/// cref="VertexDrawerPrecedence.PrimaryLabel" />.
/// </para>
///
/// </remarks>
///
/// <seealso cref="PerVertexDrawer" />
//*****************************************************************************

public class PerVertexWithLabelDrawer : PerVertexWithImageDrawer
{
    //*************************************************************************
    //  Constructor: PerVertexWithLabelDrawer()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="PerVertexWithLabelDrawer" /> class.
    /// </summary>
    //*************************************************************************

    public PerVertexWithLabelDrawer()
    {
		m_oFont = SystemFonts.DefaultFont;
		m_oPrimaryLabelFillColor = SystemColors.Window;

		AssertValid();
    }

	//*************************************************************************
	//	Property: Font
	//
	/// <summary>
	///	Gets or sets the font to use for drawing labels.
	/// </summary>
	///
	/// <value>
	///	The font to use for drawing labels.  The default is the system default
	/// font.
	/// </value>
	//*************************************************************************

	public Font
	Font
	{
		get
		{
			AssertValid();

			return (m_oFont);
		}

		set
		{
			m_oFont = value;

			AssertValid();
		}
	}

	//*************************************************************************
	//	Property: PrimaryLabelFillColor
	//
	/// <summary>
	///	Gets or sets the fill color to use for primary labels.
	/// </summary>
	///
	/// <value>
	///	The fill color to use for primary labels.  The default is
	/// SystemColors.Window.
	/// </value>
	///
	/// <remarks>
	/// <see cref="Color" /> is used for the primary label text.
	/// </remarks>
	//*************************************************************************

	public Color
	PrimaryLabelFillColor
	{
		get
		{
			AssertValid();

			return (m_oPrimaryLabelFillColor);
		}

		set
		{
			m_oPrimaryLabelFillColor = value;

			AssertValid();
		}
	}

	//*************************************************************************
	//	Method: TryGetPrimaryLabelDrawInfo()
	//
	/// <summary>
	/// Attempts to get the primary label drawing information for a vertex.
	/// </summary>
	///
	/// <param name="vertex">
	/// Vertex to get drawing information for.
	/// </param>
	///
	/// <param name="primaryLabelDrawInfo">
	/// Where the primary label drawing information gets stored if true is
	/// returned.
	/// </param>
	///
	/// <returns>
	/// true if <paramref name="primaryLabelDrawInfo" /> was obtained.
	/// </returns>
	///
	/// <remarks>
	/// This method attempts to retrieve a <see cref="PrimaryLabelDrawInfo" />
	/// object that may have been stored in a vertex's metadata by <see
	/// cref="PreDrawVertexCore" />.
	/// </remarks>
	//*************************************************************************

	public static Boolean
	TryGetPrimaryLabelDrawInfo
	(
		IVertex vertex,
		out PrimaryLabelDrawInfo primaryLabelDrawInfo
	)
	{
		Debug.Assert(vertex != null);

		primaryLabelDrawInfo = null;

		// Attempt to retrieve the drawing information cached by
		// PerVertexWithLabelDrawer.PreDrawVertexCore().

		Object oObject;

		if ( vertex.TryGetValue(
				ReservedMetadataKeys.PerVertexPrimaryLabelDrawInfo,
				typeof(PrimaryLabelDrawInfo), out oObject) )
		{
			primaryLabelDrawInfo = (PrimaryLabelDrawInfo)oObject;

			return (true);
		}

		return (false);
	}

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
    //*************************************************************************

    protected override void
    PreDrawVertexCore
    (
        IVertex vertex,
		DrawContext drawContext
    )
	{
		Debug.Assert(vertex != null);
		Debug.Assert(drawContext != null);
		AssertValid();

		// To draw the vertex as a primary label, the vertex must have a
		// ReservedMetadataKeys.PerVertexPrimaryLabel value, and if it has a
		// ReservedMetadataKeys.PerVertexDrawerPrecedence key, the value must
		// be VertexDrawerPrecedence.PrimaryLabel.

		Object oPrimaryLabel, oVertexDrawerPrecedence;

		if (
			!vertex.TryGetValue(ReservedMetadataKeys.PerVertexPrimaryLabel,
				typeof(String), out oPrimaryLabel)
			||
				(
				vertex.TryGetValue(
					ReservedMetadataKeys.PerVertexDrawerPrecedence,
					typeof(VertexDrawerPrecedence),
					out oVertexDrawerPrecedence)
				&&
				(VertexDrawerPrecedence)oVertexDrawerPrecedence !=
					VertexDrawerPrecedence.PrimaryLabel
				)
			)
		{
			// Remove the metadata that may have been added by a previous call
			// to this method.

			vertex.RemoveKey(
				ReservedMetadataKeys.PerVertexPrimaryLabelDrawInfo);

			// Defer to the base class.

			base.PreDrawVertexCore(vertex, drawContext);

			return;
		}

		// Compute and cache the information needed to draw the primary label.

		String sPrimaryLabel = (String)oPrimaryLabel;

		if ( String.IsNullOrEmpty(sPrimaryLabel) )
		{
			// Behave gracefully in this case.

			sPrimaryLabel = " ";
		}

		Boolean bDrawSelected = VertexShouldBeDrawnSelected(vertex);

		// Get the rectangles in which to draw the primary label.

		RectangleF oTextRectangleF;
		Rectangle oOutlineRectangle;

		GetPrimaryLabelRectangles(vertex, drawContext, sPrimaryLabel,
			bDrawSelected, out oTextRectangleF, out oOutlineRectangle);

		// Adjust the vertex location so it is always in the center of the
		// rectangles.

		vertex.Location = new PointF(
            oTextRectangleF.Left + oTextRectangleF.Width / 2F,
            oTextRectangleF.Top + oTextRectangleF.Height / 2F
			);

		PrimaryLabelDrawInfo oPrimaryLabelDrawInfo = new PrimaryLabelDrawInfo(
			sPrimaryLabel, oTextRectangleF, oOutlineRectangle);

		vertex.SetValue(ReservedMetadataKeys.PerVertexPrimaryLabelDrawInfo,
            oPrimaryLabelDrawInfo);
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

		Boolean bDrawSelected = VertexShouldBeDrawnSelected(vertex);

		Color oOutlineColor, oTextColor;

		if (bDrawSelected)
		{
			// The text should be the per-vertex color with no alpha applied.
			// The outline should be the selected color with no alpha applied.

			oTextColor = this.Color;

			Object oPerVertexColorAsObject;

			if ( vertex.TryGetValue(ReservedMetadataKeys.PerColor,
				typeof(Color), out oPerVertexColorAsObject) )
			{
				// Use the per-vertex color instead of the base-class color.

				oTextColor = (Color)oPerVertexColorAsObject;
			}

			oOutlineColor = this.SelectedColor;
		}
		else
		{
			// The text should be the per-vertex color with alpha applied.
			// The outline should be the per-vertex color with alpha applied.

			oTextColor = oOutlineColor = GetActualColor(vertex, false);
		}

		Rectangle oVertexBounds;

		// Attempt to retrieve the drawing information cached by
		// PreDrawVertexCore().

		PrimaryLabelDrawInfo oPrimaryLabelDrawInfo;

		if ( TryGetPrimaryLabelDrawInfo(vertex, out oPrimaryLabelDrawInfo) )
		{
			// Draw the vertex as a rectangle containing the primary label
			// text.

			DrawPrimaryLabel(vertex, drawContext, oPrimaryLabelDrawInfo,
				oTextColor, oOutlineColor, bDrawSelected);

			oVertexBounds = oPrimaryLabelDrawInfo.OutlineRectangle;
		}
		else
		{
			// Let the base class draw the vertex.

			oVertexBounds = base.DrawVertexCore(vertex, drawContext);
		}

		Object oSecondaryLabel;

		if ( vertex.TryGetValue(ReservedMetadataKeys.PerVertexSecondaryLabel,
			typeof(String), out oSecondaryLabel) )
		{
			// Add the secondary vertex label to whatever has already been
			// drawn.

			DrawSecondaryLabel(vertex, drawContext, (String)oSecondaryLabel,
				oVertexBounds, oTextColor);
		}

		return (oVertexBounds);
	}

    //*************************************************************************
    //  Method: DrawPrimaryLabel()
    //
    /// <summary>
    /// Draws a vertex as a rectangle containing the primary label text.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to draw.
    /// </param>
    ///
    /// <param name="oDrawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
    ///
    /// <param name="oPrimaryLabelDrawInfo">
	/// Information needed to draw the primary label.
    /// </param>
	///
    /// <param name="oTextColor">
	/// Color to use for the text.
    /// </param>
	///
    /// <param name="oOutlineColor">
	/// Color to use for the outline.
    /// </param>
	///
    /// <param name="bDrawSelected">
	/// true if the vertex should be drawn selected.
    /// </param>
    //*************************************************************************

	protected void
	DrawPrimaryLabel
	(
		IVertex oVertex,
		DrawContext oDrawContext,
		PrimaryLabelDrawInfo oPrimaryLabelDrawInfo,
		Color oTextColor,
		Color oOutlineColor,
		Boolean bDrawSelected
	)
	{
		Debug.Assert(oVertex != null);
		Debug.Assert(oDrawContext != null);
		Debug.Assert(oPrimaryLabelDrawInfo != null);
		AssertValid();

		// If a per-vertex fill color hasn't been specified, use the default
		// fill color.

		Color oFillColor;
		Object oFillColorAsObject;

		if ( oVertex.TryGetValue(
			ReservedMetadataKeys.PerVertexPrimaryLabelFillColor, typeof(Color),
			out oFillColorAsObject) )
		{
			oFillColor = (Color)oFillColorAsObject;
		}
		else
		{
			oFillColor = m_oPrimaryLabelFillColor;
		}

		// Use the alpha value of the outline color for the fill color.

		oFillColor = Color.FromArgb(oOutlineColor.A, oFillColor);

		// Get the rectangles in which to draw the label.

		RectangleF oTextRectangleF = oPrimaryLabelDrawInfo.TextRectangle;
		Rectangle oOutlineRectangle = oPrimaryLabelDrawInfo.OutlineRectangle;

		Graphics oGraphics = oDrawContext.Graphics;

		// Fill and draw the rectangle.

		SmoothingMode eOldSmoothingMode = oGraphics.SmoothingMode;
		oGraphics.SmoothingMode = SmoothingMode.None;

		oGraphics.FillRectangle(GetSharedSolidBrush(oFillColor),
			oOutlineRectangle);

		DrawVertexOutline(oDrawContext, oOutlineRectangle, oOutlineColor,
			bDrawSelected);

		oGraphics.SmoothingMode = eOldSmoothingMode;

		// Draw the label.

		oGraphics.DrawString(oPrimaryLabelDrawInfo.PrimaryLabel, m_oFont,
			GetSharedSolidBrush(oTextColor), oTextRectangleF);

		// Add a hit-test area to the vertex's metadata.  This gets used by
		// VertexContainsPoint().

		SetRectangularHitTestArea(oVertex, oOutlineRectangle);
	}

    //*************************************************************************
    //  Method: DrawSecondaryLabel()
    //
    /// <summary>
	/// Adds a secondary vertex label to whatever has already been drawn.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to draw.
    /// </param>
    ///
    /// <param name="oDrawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
    ///
    /// <param name="sSecondaryLabel">
	/// Secondary label text.  Can be null or empty.
    /// </param>
	///
    /// <param name="oVertexBounds">
	/// Bounding rectangle of the vertex.
    /// </param>
    ///
    /// <param name="oTextColor">
	/// Color to use.
    /// </param>
    //*************************************************************************

	protected void
	DrawSecondaryLabel
	(
		IVertex oVertex,
		DrawContext oDrawContext,
		String sSecondaryLabel,
		Rectangle oVertexBounds,
		Color oTextColor
	)
	{
		Debug.Assert(oVertex != null);
		Debug.Assert(oDrawContext != null);
		AssertValid();

		if ( String.IsNullOrEmpty(sSecondaryLabel) )
		{
			// Behave gracefully in this case.

			return;
		}

		RectangleF oSecondaryLabelRectangleF = GetSecondaryLabelRectangle(
			oVertex, oDrawContext, sSecondaryLabel, oVertexBounds);

		Graphics oGraphics = oDrawContext.Graphics;

		// Use a text rendering hint that results in text looking the same when
		// drawn twice.  Why is this necessary?  Because when a vertex is
		// selected in NodeXLControl, its incident edges get selected as well.
		// Redrawing the incident edges as selected requires that the adjacent
		// vertices be redrawn.  Because of this, even though the adjacent
		// vertices aren't selected, they get redrawn.  If the text rendering
		// hint were left at its default value, the text might get heavier when
		// drawn on top of itself.

		TextRenderingHint eOldTextRenderingHint = oGraphics.TextRenderingHint;

		oGraphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

		SolidBrush oSharedSolidBrush = GetSharedSolidBrush(oTextColor);

		oGraphics.DrawString(sSecondaryLabel, m_oFont, oSharedSolidBrush,
            oSecondaryLabelRectangleF);

		oGraphics.TextRenderingHint = eOldTextRenderingHint;
	}

    //*************************************************************************
    //  Method: GetPrimaryLabelRectangles()
    //
    /// <summary>
    /// Gets the rectangles in which to draw a vertex's primary label.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to draw.
    /// </param>
    ///
    /// <param name="oDrawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
    ///
    /// <param name="sPrimaryLabel">
	/// Primary label text.
    /// </param>
    ///
    /// <param name="bDrawSelected">
	/// true if the vertex should be drawn selected.
    /// </param>
    ///
    /// <param name="oTextRectangleF">
	/// Where the rectangle in which the label text should be drawn gets
	/// stored.
    /// </param>
    ///
    /// <param name="oOutlineRectangle">
	/// Where the rectangle to draw an outline around gets stored.
    /// </param>
    //*************************************************************************

	protected void
	GetPrimaryLabelRectangles
	(
		IVertex oVertex,
		DrawContext oDrawContext,
		String sPrimaryLabel,
		Boolean bDrawSelected,
		out RectangleF oTextRectangleF,
		out Rectangle oOutlineRectangle
	)
	{
		Debug.Assert(oVertex != null);
		Debug.Assert(oDrawContext != null);
        Debug.Assert( !String.IsNullOrEmpty(sPrimaryLabel) );
		AssertValid();

		Graphics oGraphics = oDrawContext.Graphics;

		// Get the size required for drawing the text.

		SizeF oLabelSizeF = oGraphics.MeasureString(
			sPrimaryLabel, m_oFont, MaximumLabelWidth);

		// Convert to integers immediately.  If this isn't done, two vertices
		// with the same label but different locations can end up with
		// different paddings or sizes due to rounding and truncation errors.

		Size oLabelSize = Size.Ceiling(oLabelSizeF);

		// Center the text rectangle on the vertex.  Again, covert to integers
		// to avoid inconsistencies.

		oTextRectangleF = new RectangleF(
			new PointF( (Int32)oVertex.Location.X,  (Int32)oVertex.Location.Y ),
			oLabelSize);

		oTextRectangleF.Offset(-oLabelSize.Width / 2, -oLabelSize.Height / 2);

		// Add padding.

		oOutlineRectangle = Rectangle.Truncate(oTextRectangleF);

		oOutlineRectangle.Inflate(LabelPadding, LabelPadding);

		// Adjust the rectangles so they don't get clipped by the graph
		// rectangle.

		AdjustPrimaryLabelRectangles(oDrawContext, ref oTextRectangleF,
			ref oOutlineRectangle);
	}

    //*************************************************************************
    //  Method: AdjustPrimaryLabelRectangles()
    //
    /// <summary>
    /// Adjusts the rectangles in which to draw a vertex's primary label so
	/// they don't get clipped by the graph rectangle.
    /// </summary>
    ///
    /// <param name="oDrawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
    ///
    /// <param name="oTextRectangleF">
	/// The rectangle in which the label text should be drawn.  This gets
	/// updated with an adjusted rectangle.
    /// </param>
    ///
    /// <param name="oOutlineRectangle">
	/// The rectangle to draw an outline around gets stored.  This gets updated
	/// with an adjusted rectangle.
    /// </param>
    //*************************************************************************

	protected void
	AdjustPrimaryLabelRectangles
	(
		DrawContext oDrawContext,
		ref RectangleF oTextRectangleF,
		ref Rectangle oOutlineRectangle
	)
	{
		Debug.Assert(oDrawContext != null);
		AssertValid();

		Rectangle oGraphRectangleMinusMargin =
			oDrawContext.GraphRectangleMinusMargin;

		if (oGraphRectangleMinusMargin.IsEmpty)
		{
			return;
		}

		Int32 iX = oGraphRectangleMinusMargin.Right - oOutlineRectangle.Right;

		if (iX < 0)
		{
			oTextRectangleF.Offset(iX, 0);
			oOutlineRectangle.Offset(iX, 0);
		}

		iX = oGraphRectangleMinusMargin.Left - oOutlineRectangle.Left;

		if (iX > 0)
		{
			oTextRectangleF.Offset(iX, 0);
			oOutlineRectangle.Offset(iX, 0);
		}

		Int32 iY = oGraphRectangleMinusMargin.Bottom - oOutlineRectangle.Bottom;

		if (iY < 0)
		{
			oTextRectangleF.Offset(0, iY);
			oOutlineRectangle.Offset(0, iY);
		}

		iY = oGraphRectangleMinusMargin.Top - oOutlineRectangle.Top;

		if (iY > 0)
		{
			oTextRectangleF.Offset(0, iY);
			oOutlineRectangle.Offset(0, iY);
		}
	}

    //*************************************************************************
    //  Method: GetSecondaryLabelRectangle()
    //
    /// <summary>
    /// Gets the rectangle in which to draw a vertex's secondary label.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to draw.
    /// </param>
    ///
    /// <param name="oDrawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
    ///
    /// <param name="sSecondaryLabel">
	/// Secondary label text.
    /// </param>
    ///
    /// <param name="oVertexBounds">
	/// Bounding rectangle of the vertex.
    /// </param>
    ///
    /// <returns>
	/// The rectangle in which the secondary label text should be drawn.
    /// </returns>
    //*************************************************************************

	protected RectangleF
	GetSecondaryLabelRectangle
	(
		IVertex oVertex,
		DrawContext oDrawContext,
		String sSecondaryLabel,
		Rectangle oVertexBounds
	)
	{
		Debug.Assert(oVertex != null);
		Debug.Assert(oDrawContext != null);
        Debug.Assert( !String.IsNullOrEmpty(sSecondaryLabel) );
		AssertValid();

		Graphics oGraphics = oDrawContext.Graphics;

		SizeF oSizeF = oGraphics.MeasureString(sSecondaryLabel, m_oFont);

		// Position the rectangle so its lower-left corner touches the
		// upper-right corner of the vertex.

		RectangleF oSecondaryLabelRectangleF = RectangleF.FromLTRB(
			oVertexBounds.Right,
			oVertexBounds.Top - oSizeF.Height,
			oVertexBounds.Right + oSizeF.Width,
			oVertexBounds.Top
			);

		// Adjust the rectangle so it doesn't get clipped by the graph
		// rectangle.

		AdjustSecondaryLabelRectangle(oDrawContext,
			ref oSecondaryLabelRectangleF);

		return (oSecondaryLabelRectangleF);
	}

    //*************************************************************************
    //  Method: AdjustSecondaryLabelRectangle()
    //
    /// <summary>
    /// Adjusts the rectangle in which to draw a vertex's secondary label so
	/// they it doesn't get clipped by the graph rectangle.
    /// </summary>
    ///
    /// <param name="oDrawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
    ///
    /// <param name="oSecondaryLabelRectangleF">
	/// The rectangle in which the secondary label text should be drawn.  This
	/// gets updated with an adjusted rectangle.
    /// </param>
    //*************************************************************************

	protected void
	AdjustSecondaryLabelRectangle
	(
		DrawContext oDrawContext,
		ref RectangleF oSecondaryLabelRectangleF
	)
	{
		Debug.Assert(oDrawContext != null);
		AssertValid();

		Rectangle oGraphRectangleMinusMargin =
			oDrawContext.GraphRectangleMinusMargin;

		if (oGraphRectangleMinusMargin.IsEmpty)
		{
			return;
		}

		// Don't let the label rectangle exceed the right edge of the graph
		// rectangle.

		Single fX = oGraphRectangleMinusMargin.Right
			- oSecondaryLabelRectangleF.Right;

		if (fX < 0)
		{
			oSecondaryLabelRectangleF.Offset(fX, 0);
		}

		// Don't let the label rectangle exceed the top edge of the graph
		// rectangle.

		Single fY = oGraphRectangleMinusMargin.Top
			- oSecondaryLabelRectangleF.Top;

		if (fY > 0)
		{
			oSecondaryLabelRectangleF.Offset(0, fY);
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

		Debug.Assert(m_oFont != null);
		// m_oPrimaryLabelFillColor
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	/// Maximum with of a label, not including the label padding.

	protected const Int32 MaximumLabelWidth = 100;

	/// Padding between the label text and label outline.

	protected const Int32 LabelPadding = 3;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	///	The font to use for drawing labels.

	protected Font m_oFont;

	///	The fill color to use for primary labels.

	protected Color m_oPrimaryLabelFillColor;
}

}
