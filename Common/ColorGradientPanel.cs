
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: ColorGradientPanel
//
/// <summary>
/// Panel control that displays a color gradient across the width of the
/// control.
/// </summary>
///
/// <remarks>
/// When the panel is drawn, it has a gradient from <see cref="MinimumColor" />
/// on the left side to <see cref="MaximumColor" /> on the right side.
/// </remarks>
//*****************************************************************************

public class ColorGradientPanel : Panel
{
    //*************************************************************************
    //  Constructor: ColorGradientPanel()
    //
    /// <summary>
    /// Initializes a new instance of the ColorGradientPanel class.
    /// </summary>
    //*************************************************************************

    public ColorGradientPanel()
    {
		m_oMinimumColor = Color.White;
		m_oMaximumColor = Color.Black;
    }

    //*************************************************************************
    //  Property: MinimumColor
    //
    /// <summary>
    /// Gets or sets the color to use at the left edge of the panel.
    /// </summary>
    ///
    /// <value>
	/// The color of the left edge of the panel.
    /// </value>
    //*************************************************************************

    public Color
    MinimumColor
    {
        get
        {
            AssertValid();

            return (m_oMinimumColor);
        }

        set
        {
			m_oMinimumColor = value;

			Invalidate();

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: MaximumColor
    //
    /// <summary>
    /// Gets or sets the color to use at the right edge of the panel.
    /// </summary>
    ///
    /// <value>
	/// The color of the right edge of the panel.
    /// </value>
    //*************************************************************************

    public Color
    MaximumColor
    {
        get
        {
            AssertValid();

            return (m_oMaximumColor);
        }

        set
        {
			m_oMaximumColor = value;

			Invalidate();

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: OnPaint()
    //
    /// <summary>
    /// Handles the Paint event.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event arguments.
    /// </param>
    //*************************************************************************

	protected override void
	OnPaint
	(
		PaintEventArgs e
	)
    {
        AssertValid();

		FillRectangleWithLinearGradient(e.Graphics, this.ClientRectangle,
			m_oMinimumColor, m_oMaximumColor);
    }

    //*************************************************************************
    //  Method: FillRectangleWithLinearGradient()
    //
    /// <summary>
	/// Fills a rectangle with a linear gradient from left to right.
    /// </summary>
    ///
    /// <param name="oGraphics">
    /// Object to draw with.
    /// </param>
    ///
    /// <param name="oRectangle">
    /// Rectangle to fill.
    /// </param>
    ///
    /// <param name="oLeftColor">
    /// Color to use at the left edge of the rectangle.
    /// </param>
    ///
    /// <param name="oRightColor">
    /// Color to use at the right edge of the rectangle.
    /// </param>
    //*************************************************************************

	protected void
	FillRectangleWithLinearGradient
	(
		Graphics oGraphics,
		Rectangle oRectangle,
		Color oLeftColor,
		Color oRightColor
	)
	{
		Debug.Assert(oGraphics != null);

		// There is an oddity with the LinearGradientBrush.  If the rectangle
		// passed to it has an even Right property value, the color at the
		// right edge (oRightColor) wraps around to the vertical line of pixels
		// at the left edge, which should actually be oLeftColor.  This does
		// not occur if the Right value is odd.  Is this a Framework bug?
		// 
		// Compensate by extending the brush.

		Rectangle oBrushRectangle = Rectangle.FromLTRB(
			oRectangle.Left - 1,
			oRectangle.Top,
			oRectangle.Right,
			oRectangle.Bottom
			);

		LinearGradientBrush oLinearGradientBrush = new LinearGradientBrush(
			oBrushRectangle, oLeftColor, oRightColor,
			LinearGradientMode.Horizontal);

		oGraphics.FillRectangle(oLinearGradientBrush, oRectangle);

		oLinearGradientBrush.Dispose();
	}


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public virtual void
    AssertValid()
    {
		// m_oMinimumColor
		// m_oMaximumColor
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Color to use at the left edge of the panel.

	protected Color m_oMinimumColor;

	/// Color to use at the right edge of the panel.

	protected Color m_oMaximumColor;
}

}
