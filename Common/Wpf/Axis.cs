
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.ChartLib;

namespace Microsoft.WpfGraphicsLib
{
//*****************************************************************************
//  Class: Axis
//
/// <summary>
/// Draws a linear axis with major and minor gridlines, major gridline values,
/// and an axis label.
/// </summary>
///
/// <remarks>
/// This can be used as either a horizontal x-axis (the default) or a vertical
/// y-axis.  For the latter, set <see cref="IsXAxis" /> to false.
///
/// <para>
/// The axis is meant to be docked below or to the left of the control to which
/// the axis applies.  If you set the <see
/// cref="DockedControlRenderTransform" /> to the render transform of the
/// docked control, the axis will adjust itself as the docked control is zoomed
/// or translated.
/// </para>
///
/// <para>
/// Call <see cref="SetRange" /> to set the range of values in the axis.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class Axis : FrameworkElement
{
    //*************************************************************************
    //  Constructor: Axis()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="Axis" /> class.
    /// </summary>
    //*************************************************************************

    public Axis()
    {
        m_bIsXAxis = true;
        m_sLabel = String.Empty;
        m_dNearValue = 0;
        m_dFarValue = 100;
        m_dNearOffset = 25;
        m_dFarOffset = 25;
        m_oDockedControlRenderTransform = new TranslateTransform();

        m_oTypeface = new Typeface(SystemFonts.CaptionFontFamily,
            FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);

        m_dLabelFontSizeEm = 11;

        // Turn off anti-aliasing for line drawing.  If this isn't done, the
        // one-pixel lines drawn by this class appear blurry.

        RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);

        AssertValid();
    }

    //*************************************************************************
    //  Property: IsXAxis
    //
    /// <summary>
    /// Gets or sets a flag specifying whether this is the x-axis.
    /// </summary>
    ///
    /// <value>
    /// true if this control represents the x-axis, false if it represents the
    /// y-axis.
    /// </value>
    ///
    /// <remarks>
    /// When true, the axis is drawn horizontally.  The near end of the axis
    /// range is at the left end of the control, and the far end of the axis
    /// range is the right end of the control.
    ///
    /// <para>
    /// When false, the axis is drawn vertically.  The near end of the axis
    /// range is the bottom end of the control, and the far end of the axis
    /// range is at the top end of the control.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Boolean
    IsXAxis
    {
        get
        {
            AssertValid();

            return (m_bIsXAxis);
        }

        set
        {
            m_bIsXAxis = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Label
    //
    /// <summary>
    /// Gets or sets the label for the axis.
    /// </summary>
    ///
    /// <value>
    /// The label for the axis.  Can be null.  The default is an empty string.
    /// </value>
    //*************************************************************************

    public String
    Label
    {
        get
        {
            AssertValid();

            return (m_sLabel);
        }

        set
        {
            m_sLabel = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: DockedControlRenderTransform
    //
    /// <summary>
    /// Gets or sets the render transform for the control to which this axis is
    /// docked.
    /// </summary>
    ///
    /// <value>
    /// The docked control's render transform.  The default is an
    /// IdentityTransform.
    /// </value>
    //*************************************************************************

    public Transform
    DockedControlRenderTransform
    {
        get
        {
            AssertValid();

            return (m_oDockedControlRenderTransform);
        }

        set
        {
            m_oDockedControlRenderTransform = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: SetRange()
    //
    /// <summary>
    /// Sets the range of values displayed in the axis.
    /// </summary>
    ///
    /// <param name="nearValue">
    /// The value at the near end of the axis.  The default is 0.
    /// </param>
    ///
    /// <param name="nearOffset">
    /// The offset of <paramref name="nearValue" /> from the near end of the
    /// axis.  The default is 25.0.
    /// </param>
    ///
    /// <param name="farValue">
    /// The value at the far end of the axis.  The default is 100.0.
    /// </param>
    ///
    /// <param name="farOffset">
    /// The offset of <paramref name="farValue" /> from the far end of the
    /// axis.  The default is 25.0.
    /// </param>
    ///
    /// <remarks>
    /// If this is the x-axis, the near end of the axis range is at the left
    /// end of the control, and the far end of the axis range is the right end
    /// of the control.
    ///
    /// <para>
    /// If this is the y-axis, the near end of the axis range is the bottom end
    /// of the control, and the far end of the axis range is at the top end of
    /// the control.
    /// </para>
    ///
    /// <para>
    /// <paramref name="farValue" /> is not necessarily greater than <paramref
    /// name="nearValue" />.  The axis values can increase or decrease as you
    /// go from its near end to its far end.
    /// </para>
    ///
    /// <para>
    /// The values and offsets passed to this method should NOT take into
    /// account any zoom or translation that has been applied to the docked
    /// control.  If you set the <see cref="DockedControlRenderTransform" /> to
    /// the render transform of the docked control, the axis will adjust itself
    /// as the docked control is zoomed or translated.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    SetRange
    (
        Double nearValue,
        Double nearOffset,
        Double farValue,
        Double farOffset
    )
    {
        if (nearValue == m_dNearValue && nearOffset == m_dNearOffset
            && farValue == m_dFarValue && farOffset == m_dFarOffset)
        {
            return;
        }

        m_dNearValue = nearValue;
        m_dNearOffset = nearOffset;
        m_dFarValue = farValue;
        m_dFarOffset = farOffset;
        this.InvalidateVisual();

        AssertValid();
    }

    //*************************************************************************
    //  Method: SetFont()
    //
    /// <summary>
    /// Sets the font used to draw the axis label and values.
    /// </summary>
    ///
    /// <param name="typeface">
    /// The Typeface to use.
    /// </param>
    ///
    /// <param name="labelEmSize">
    /// The font size to use for the axis label, in ems.  (A slightly smaller
    /// size is used for the axis values.)
    /// </param>
    ///
    /// <remarks>
    /// The default font is the SystemFonts.CaptionFontFamily at size 11.
    /// </remarks>
    //*************************************************************************

    public void
    SetFont
    (
        Typeface typeface,
        Double labelEmSize
    )
    {
        Debug.Assert(typeface != null);
        Debug.Assert(labelEmSize > 0);
        AssertValid();

        if (m_oTypeface == typeface && m_dLabelFontSizeEm == labelEmSize)
        {
            return;
        }

        m_oTypeface = typeface;
        m_dLabelFontSizeEm = labelEmSize;
        this.InvalidateVisual();
    }

    //*************************************************************************
    //  Property: ValueFontSizeEm
    //
    /// <summary>
    /// Gets the font size to use for axis values.
    /// </summary>
    ///
    /// <value>
    /// The font size to use for axis values, in ems.
    /// </value>
    //*************************************************************************

    protected Double
    ValueFontSizeEm
    {
        get
        {
            AssertValid();

            return (m_dLabelFontSizeEm * ValueFontSizeFactor);
        }
    }

    //*************************************************************************
    //  Property: MajorGridlineMargin
    //
    /// <summary>
    /// Gets the margin between the end of a major gridline and the gridline's
    /// value.
    /// </summary>
    ///
    /// <value>
    /// The margin between the end of a major gridline and the gridline's
    /// value.
    /// </value>
    //*************************************************************************

    protected Double
    MajorGridlineMargin
    {
        get
        {
            AssertValid();

            return (m_dLabelFontSizeEm * MajorGridlineMarginFactor);
        }
    }

    //*************************************************************************
    //  Property: LabelMargin
    //
    /// <summary>
    /// Gets the margin between the gridline value and the axis label.
    /// </summary>
    ///
    /// <value>
    /// The margin between the gridline value and the axis label.
    /// </value>
    //*************************************************************************

    protected Double
    LabelMargin
    {
        get
        {
            AssertValid();

            return (m_dLabelFontSizeEm * LabelMarginFactor);
        }
    }

    //*************************************************************************
    //  Method: MeasureOverride()
    //
    /// <summary>
    /// When overridden in a derived class, measures the size in layout
    /// required for child elements and determines a size for the
    /// FrameworkElement-derived class. 
    /// </summary>
    ///
    /// <param name="availableSize">
    /// The available size that this element can give to child elements.
    /// Infinity can be specified as a value to indicate that the element will
    /// size to whatever content is available.
    /// </param>
    ///
    /// <returns>
    /// The size that this element determines it needs during layout, based on
    /// its calculations of child element sizes.
    /// </returns>
    //*************************************************************************

    protected override Size
    MeasureOverride
    (
        Size availableSize
    )
    {
        AssertValid();

        // It's assumed that this is being used in a Grid, so either the height
        // (x-axis) or width (y-axis) needs to be computed.  The other
        // dimension is irrelevant, because the Grid will given it the entire
        // available width (x-axis) or height (y-axis).

        Double dHeightOrWidth = MajorGridlineLength + MajorGridlineMargin
            + ValueFontSizeEm + LabelMargin + m_dLabelFontSizeEm
            + LabelMargin;

        const Double ArbitraryWidthOrHeight = 100;

        if (m_bIsXAxis)
        {
            return ( new Size(ArbitraryWidthOrHeight, dHeightOrWidth) );
        }

        return ( new Size(dHeightOrWidth, ArbitraryWidthOrHeight) );
    }

    //*************************************************************************
    //  Method: OnRender()
    //
    /// <summary>
    /// Renders the control.
    /// </summary>
    ///
    /// <param name="drawingContext">
    /// The drawing instructions for a specific element. This context is
    /// provided to the layout system.
    /// </param>
    //*************************************************************************

    protected override void
    OnRender
    (
        DrawingContext drawingContext
    )
    {
        AssertValid();

        Double dAxisLength = m_bIsXAxis ? this.ActualWidth : this.ActualHeight;

        if (dAxisLength == 0)
        {
            return;
        }

        Double dNearValue, dNearOffset, dFarValue, dFarOffset;

        if (m_bIsXAxis)
        {
            dNearValue = m_dNearValue;
            dNearOffset = m_dNearOffset;
            dFarValue = m_dFarValue;
            dFarOffset = m_dFarOffset;
        }
        else
        {
            // The y-axis has to be flipped, so that increasing values run
            // from bottom to top.

            dNearValue = m_dFarValue;
            dNearOffset = m_dFarOffset;
            dFarValue = m_dNearValue;
            dFarOffset = m_dNearOffset;
        }

        // Figure out where the offsets fall after being transformed.

        Double dAdjustedNearOffset = WpfGraphicsUtil.TransformLength(
            dNearOffset, m_oDockedControlRenderTransform, m_bIsXAxis);

        Double dFarX = dAxisLength - dFarOffset;

        Double dAdjustedFarX = WpfGraphicsUtil.TransformLength(
            dFarX, m_oDockedControlRenderTransform, m_bIsXAxis);

        if (dAdjustedFarX == dAdjustedNearOffset)
        {
            DrawOffsetBackground( drawingContext, new Rect(0, 0,
                this.ActualWidth, this.ActualHeight) );

            return;
        }

        Double dAdjustedNearValue = dNearValue;
        Double dAdjustedFarValue = dFarValue;

        // Use the point-slope equation of a line to map adjusted offsets to
        // values, if necessary.

        Double dX1 = dAdjustedNearOffset;
        Double dY1 = dNearValue;
        Double dX2 = dAdjustedFarX;
        Double dY2 = dFarValue;

        Debug.Assert(dX1 != dX2);
        Double dM = (dY1 - dY2) / (dX1 - dX2);

        if (dAdjustedNearOffset < 0)
        {
            // Find the value at the near end of the axis.

            dAdjustedNearValue = dY1 + dM * (0 - dX1);
            dAdjustedNearOffset = 0;
        }

        Double dAdjustedFarOffset;

        if (dAdjustedFarX <= dAxisLength)
        {
            dAdjustedFarOffset = dAxisLength - dAdjustedFarX;
        }
        else
        {
            // Find the value at the far end of the axis.

            dAdjustedFarValue = dY1 + dM * (dAxisLength - dX1);
            dAdjustedFarOffset = 0;
        }

        if (m_bIsXAxis)
        {
            OnRender(drawingContext, dAdjustedNearValue, dAdjustedNearOffset,
                dAdjustedFarValue, dAdjustedFarOffset);
        }
        else
        {
            // The y-axis is flipped.

            OnRender(drawingContext, dAdjustedFarValue, dAdjustedFarOffset,
                dAdjustedNearValue, dAdjustedNearOffset);
        }
    }

    //*************************************************************************
    //  Method: OnRender()
    //
    /// <summary>
    /// Renders the control.
    /// </summary>
    ///
    /// <param name="oDrawingContext">
    /// The drawing instructions for a specific element. This context is
    /// provided to the layout system.
    /// </param>
    ///
    /// <param name="dAdjustedNearValue">
    /// The value at the near end of the axis, adjusted for the docked
    /// control's render transform.
    /// </param>
    ///
    /// <param name="dAdjustedNearOffset">
    /// The offset of dAdjustedNearValue from the near end of the axis,
    /// adjusted for the docked control's render transform.
    /// </param>
    ///
    /// <param name="dAdjustedFarValue">
    /// The value at the far end of the axis, adjusted for the docked
    /// control's render transform.
    /// </param>
    ///
    /// <param name="dAdjustedFarOffset">
    /// The offset of dAdjustedFarValue from the far end of the axis,
    /// adjusted for the docked control's render transform.
    /// </param>
    //*************************************************************************

    protected void
    OnRender
    (
        DrawingContext oDrawingContext,
        Double dAdjustedNearValue,
        Double dAdjustedNearOffset,
        Double dAdjustedFarValue,
        Double dAdjustedFarOffset
    )
    {
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(dAdjustedNearOffset >= 0);
        Debug.Assert(dAdjustedFarOffset >= 0);
        AssertValid();

        Rect oClientRectangle =
            new Rect(0, 0, this.ActualWidth, this.ActualHeight);

        Double dAxisLength = m_bIsXAxis ? this.ActualWidth : this.ActualHeight;

        if (dAxisLength == 0)
        {
            return;
        }

        // If the pen is not one pixel wide, gridlines drawn on a large-font
        // machine vary in width, depending on their position.

        Pen oPen = WpfGraphicsUtil.CreateOnePixelPen(this,
            SystemColors.WindowTextBrush);

        WpfGraphicsUtil.FreezeIfFreezable(oPen);

        DrawBackground(oDrawingContext, oClientRectangle, dAdjustedNearOffset,
            dAdjustedFarOffset);

        DrawAxisLine(oDrawingContext, oClientRectangle, oPen);

        if (dAdjustedFarValue == dAdjustedNearValue)
        {
            // This is a degenerate case.  Draw just one gridline.

            DrawMajorGridlineAndValue(oDrawingContext, oClientRectangle, oPen,
                dAdjustedNearValue, dAdjustedNearOffset, "N");

            return;
        }

        if (dAdjustedNearOffset + dAdjustedFarOffset > dAxisLength)
        {
            return;
        }

        DrawLabel(oDrawingContext, oClientRectangle);

        Double [] adMajorGridlineValues;
        Int32 iDecimalPlacesToShow;

        ChartUtil.GetAxisGridlineValues(dAdjustedNearValue, dAdjustedFarValue,
            out adMajorGridlineValues, out iDecimalPlacesToShow);

        String sFormatSpecifier = GetFormatSpecifier(adMajorGridlineValues,
            iDecimalPlacesToShow);

        // Use the point-slope equation of a line to get the major gridline
        // coordinates.

        Double dX1 = dAdjustedNearValue;
        Double dY1 = dAdjustedNearOffset;
        Double dX2 = dAdjustedFarValue;
        Double dY2 = dAxisLength - dAdjustedFarOffset;

        Debug.Assert(dX1 != dX2);
        Double dM = (dY1 - dY2) / (dX1 - dX2);

        Double dPreviousMajorGridlineOffset = Double.NaN;

        foreach (Double dMajorGridlineValue in adMajorGridlineValues)
        {
            // Compute the major gridline's offset, then draw it.

            Double dMajorGridlineOffset =
                dY1 + dM * (dMajorGridlineValue - dX1);

            DrawMajorGridlineAndValue(oDrawingContext, oClientRectangle, oPen,
                dMajorGridlineValue, dMajorGridlineOffset, sFormatSpecifier);

            if ( !Double.IsNaN(dPreviousMajorGridlineOffset) )
            {
                // Draw minor gridlines between the major gridlines.

                DrawMinorGridlines(oDrawingContext, oClientRectangle, oPen,
                    dMajorGridlineOffset, dPreviousMajorGridlineOffset);
            }

            dPreviousMajorGridlineOffset = dMajorGridlineOffset;
        }
    }

    //*************************************************************************
    //  Method: DrawBackground()
    //
    /// <summary>
    /// Draws the control's background.
    /// </summary>
    ///
    /// <param name="oDrawingContext">
    /// The DrawingContext to draw with.
    /// </param>
    ///
    /// <param name="oClientRectangle">
    /// The control's client rectangle.
    /// </param>
    ///
    /// <param name="dAdjustedNearOffset">
    /// The offset of dAdjustedNearValue from the near end of the axis,
    /// adjusted for the docked control's render transform.
    /// </param>
    ///
    /// <param name="dAdjustedFarOffset">
    /// The offset of dAdjustedFarValue from the far end of the axis,
    /// adjusted for the docked control's render transform.
    /// </param>
    //*************************************************************************

    protected void
    DrawBackground
    (
        DrawingContext oDrawingContext,
        Rect oClientRectangle,
        Double dAdjustedNearOffset,
        Double dAdjustedFarOffset
    )
    {
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(dAdjustedNearOffset >= 0);
        Debug.Assert(dAdjustedFarOffset >= 0);
        AssertValid();

        // Draw the entire background.

        oDrawingContext.DrawRectangle(SystemColors.WindowBrush, null,
            oClientRectangle);

        // Draw rectangles in a different color to demarcate the offsets.

        Rect oNearOffsetRectangle, oFarOffsetRectangle;

        if (m_bIsXAxis)
        {
            oNearOffsetRectangle = new Rect(
                oClientRectangle.Left,
                oClientRectangle.Top,
                dAdjustedNearOffset,
                oClientRectangle.Height
                );

            oFarOffsetRectangle = new Rect(
                oClientRectangle.Right - dAdjustedFarOffset,
                oClientRectangle.Top,
                dAdjustedFarOffset,
                oClientRectangle.Height
                );
        }
        else
        {
            oNearOffsetRectangle = new Rect(
                oClientRectangle.Left,
                oClientRectangle.Bottom - dAdjustedNearOffset,
                oClientRectangle.Width,
                dAdjustedNearOffset
                );

            oFarOffsetRectangle = new Rect(
                oClientRectangle.Left,
                oClientRectangle.Top,
                oClientRectangle.Width,
                dAdjustedFarOffset
                );
        }

        DrawOffsetBackground(oDrawingContext, oNearOffsetRectangle);
        DrawOffsetBackground(oDrawingContext, oFarOffsetRectangle);
    }

    //*************************************************************************
    //  Method: DrawOffsetBackground()
    //
    /// <summary>
    /// Draws the background for the rectangle that demarcates one of the range
    /// limits.
    /// </summary>
    ///
    /// <param name="oDrawingContext">
    /// The DrawingContext to draw with.
    /// </param>
    ///
    /// <param name="oOffsetRectangle">
    /// The offset rectangle to draw.
    /// </param>
    //*************************************************************************

    protected void
    DrawOffsetBackground
    (
        DrawingContext oDrawingContext,
        Rect oOffsetRectangle
    )
    {
        Debug.Assert(oDrawingContext != null);
        AssertValid();

        oDrawingContext.DrawRectangle(SystemColors.ControlLightBrush, null,
            oOffsetRectangle);
    }

    //*************************************************************************
    //  Method: DrawAxisLine()
    //
    /// <summary>
    /// Draws the axis line.
    /// </summary>
    ///
    /// <param name="oDrawingContext">
    /// The DrawingContext to draw with.
    /// </param>
    ///
    /// <param name="oClientRectangle">
    /// The control's client rectangle.
    /// </param>
    ///
    /// <param name="oPen">
    /// The Pen to draw with.
    /// </param>
    //*************************************************************************

    protected void
    DrawAxisLine
    (
        DrawingContext oDrawingContext,
        Rect oClientRectangle,
        Pen oPen
    )
    {
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oPen != null);
        AssertValid();

        Point oPoint1, oPoint2;

        if (m_bIsXAxis)
        {
            oPoint1 = oClientRectangle.TopLeft;
            oPoint2 = oClientRectangle.TopRight;
        }
        else
        {
            oPoint1 = oClientRectangle.BottomRight;
            oPoint2 = oClientRectangle.TopRight;
        }

        oDrawingContext.DrawLine(oPen, oPoint1, oPoint2);
    }

    //*************************************************************************
    //  Method: DrawMajorGridlineAndValue()
    //
    /// <summary>
    /// Draws a major gridline and its value.
    /// </summary>
    ///
    /// <param name="oDrawingContext">
    /// The DrawingContext to draw with.
    /// </param>
    ///
    /// <param name="oClientRectangle">
    /// The control's client rectangle.
    /// </param>
    ///
    /// <param name="oPen">
    /// The Pen to draw with.
    /// </param>
    ///
    /// <param name="dMajorGridlineValue">
    /// The gridline's value.
    /// </param>
    ///
    /// <param name="dMajorGridlineOffset">
    /// The offset of the gridline from the left edge of the control if this is
    /// the x-axis, or the bottom edge of the control if this is the y-axis.
    /// </param>
    ///
    /// <param name="sFormatSpecifier">
    /// ToString() format specifier.
    /// </param>
    //*************************************************************************

    protected void
    DrawMajorGridlineAndValue
    (
        DrawingContext oDrawingContext,
        Rect oClientRectangle,
        Pen oPen,
        Double dMajorGridlineValue,
        Double dMajorGridlineOffset,
        String sFormatSpecifier
    )
    {
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oPen != null);
        Debug.Assert( !String.IsNullOrEmpty(sFormatSpecifier) );
        AssertValid();

        DrawGridline(oDrawingContext, oClientRectangle, oPen,
            dMajorGridlineOffset, MajorGridlineLength);

        DrawMajorGridlineValue(oDrawingContext, oClientRectangle,
            dMajorGridlineValue, dMajorGridlineOffset, sFormatSpecifier);
    }

    //*************************************************************************
    //  Method: DrawGridline()
    //
    /// <summary>
    /// Draws a major or minor gridline.
    /// </summary>
    ///
    /// <param name="oDrawingContext">
    /// The DrawingContext to draw with.
    /// </param>
    ///
    /// <param name="oClientRectangle">
    /// The control's client rectangle.
    /// </param>
    ///
    /// <param name="oPen">
    /// The Pen to draw with.
    /// </param>
    ///
    /// <param name="dGridlineOffset">
    /// The offset of the gridline from the left edge of the control if this is
    /// the x-axis, or the bottom edge of the control if this is the y-axis.
    /// </param>
    ///
    /// <param name="dGridlineLength">
    /// The length of the gridline.
    /// </param>
    //*************************************************************************

    protected void
    DrawGridline
    (
        DrawingContext oDrawingContext,
        Rect oClientRectangle,
        Pen oPen,
        Double dGridlineOffset,
        Double dGridlineLength
    )
    {
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oPen != null);
        Debug.Assert(dGridlineLength >= 0);
        AssertValid();

        if (dGridlineOffset < 0)
        {
            return;
        }

        Point oPoint1, oPoint2;

        if (m_bIsXAxis)
        {
            oPoint1 = new Point(dGridlineOffset, oClientRectangle.Top);

            oPoint2 = new Point(dGridlineOffset,
                oClientRectangle.Top + dGridlineLength);
        }
        else
        {
            oPoint1 = new Point(oClientRectangle.Right,
                oClientRectangle.Bottom - dGridlineOffset);

            oPoint2 = new Point(oClientRectangle.Right - dGridlineLength,
                oPoint1.Y);
        }

        oDrawingContext.DrawLine(oPen, oPoint1, oPoint2);
    }

    //*************************************************************************
    //  Method: DrawMajorGridlineValue()
    //
    /// <summary>
    /// Draws the value for a major gridline.
    /// </summary>
    ///
    /// <param name="oDrawingContext">
    /// The DrawingContext to draw with.
    /// </param>
    ///
    /// <param name="oClientRectangle">
    /// The control's client rectangle.
    /// </param>
    ///
    /// <param name="dMajorGridlineValue">
    /// The gridline's value.
    /// </param>
    ///
    /// <param name="dMajorGridlineOffset">
    /// The offset of the gridline from the left edge of the control if this is
    /// the x-axis, or the bottom edge of the control if this is the y-axis.
    /// </param>
    ///
    /// <param name="sFormatSpecifier">
    /// ToString() format specifier.
    /// </param>
    //*************************************************************************

    protected void
    DrawMajorGridlineValue
    (
        DrawingContext oDrawingContext,
        Rect oClientRectangle,
        Double dMajorGridlineValue,
        Double dMajorGridlineOffset,
        String sFormatSpecifier
    )
    {
        Debug.Assert(oDrawingContext != null);
        Debug.Assert( !String.IsNullOrEmpty(sFormatSpecifier) );
        AssertValid();

        String sMajorGridlineValue =
            dMajorGridlineValue.ToString(sFormatSpecifier);

        FormattedText oFormattedText = GetCenteredFormattedText(
            sMajorGridlineValue, m_oTypeface, this.ValueFontSizeEm);

        // Don't draw the value if it would exceed the control's bounds.
        //
        // Note: When the FormattedText.TextAlignment is set to
        // TextAlignment.Center, the Width property actually returns half the
        // text width.

        Double dFormattedTextHalfWidth = oFormattedText.Width;

        if (m_bIsXAxis)
        {
            if (
                dMajorGridlineOffset - dFormattedTextHalfWidth >
                    oClientRectangle.Left
                &&
                dMajorGridlineOffset + dFormattedTextHalfWidth <
                    oClientRectangle.Right
                )
            {
                oDrawingContext.DrawText( oFormattedText,
                    new Point(
                        dMajorGridlineOffset,
                        oClientRectangle.Top + MajorGridlineLength
                            + MajorGridlineMargin
                    ) );
            }
        }
        else
        {
            Double dDistanceFromTop =
                oClientRectangle.Bottom - dMajorGridlineOffset;

            if (
                dDistanceFromTop - dFormattedTextHalfWidth >
                    oClientRectangle.Top
                &&
                dDistanceFromTop + dFormattedTextHalfWidth <
                    oClientRectangle.Bottom
                )
            {
                DrawRotatedText(oDrawingContext, oFormattedText,

                    oClientRectangle.Right - MajorGridlineLength
                        - MajorGridlineMargin - ValueFontSizeEm,

                    dDistanceFromTop
                    );
            }
        }
    }

    //*************************************************************************
    //  Method: DrawMinorGridlines()
    //
    /// <summary>
    /// Draw minor gridlines between two major gridlines.
    /// </summary>
    ///
    /// <param name="oDrawingContext">
    /// The DrawingContext to draw with.
    /// </param>
    ///
    /// <param name="oClientRectangle">
    /// The control's client rectangle.
    /// </param>
    ///
    /// <param name="oPen">
    /// The Pen to draw with.
    /// </param>
    ///
    /// <param name="dMajorGridline2Offset">
    /// The offset of the second gridline from the left edge of the control if
    /// this is the x-axis, or the bottom edge of the control if this is the
    /// y-axis.
    /// </param>
    ///
    /// <param name="dMajorGridline1Offset">
    /// The offset of the first gridline from the left edge of the control if
    /// this is the x-axis, or the bottom edge of the control if this is the
    /// y-axis.
    /// </param>
    //*************************************************************************

    protected void
    DrawMinorGridlines
    (
        DrawingContext oDrawingContext,
        Rect oClientRectangle,
        Pen oPen,
        Double dMajorGridline2Offset,
        Double dMajorGridline1Offset
    )
    {
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oPen != null);
        AssertValid();

        Double dMinorGridlineSpacing =
            (dMajorGridline2Offset - dMajorGridline1Offset) / MinorGridlines;

        Double dMinorGridlineOffset = dMajorGridline1Offset;

        for (Int32 i = 0; i < MinorGridlines; i++)
        {
            dMinorGridlineOffset += dMinorGridlineSpacing;

            DrawGridline(oDrawingContext, oClientRectangle, oPen,
                dMinorGridlineOffset, MinorGridlineLength);
        }
    }

    //*************************************************************************
    //  Method: DrawLabel()
    //
    /// <summary>
    /// Draws the axis label.
    /// </summary>
    ///
    /// <param name="oDrawingContext">
    /// The DrawingContext to draw with.
    /// </param>
    ///
    /// <param name="oClientRectangle">
    /// The control's client rectangle.
    /// </param>
    //*************************************************************************

    protected void
    DrawLabel
    (
        DrawingContext oDrawingContext,
        Rect oClientRectangle
    )
    {
        Debug.Assert(oDrawingContext != null);
        AssertValid();

        if ( String.IsNullOrEmpty(m_sLabel) )
        {
            return;
        }

        FormattedText oFormattedText = GetCenteredFormattedText(
            m_sLabel, m_oTypeface, m_dLabelFontSizeEm);

        if (m_bIsXAxis)
        {
            oDrawingContext.DrawText(oFormattedText, new Point(

                oClientRectangle.Left + (oClientRectangle.Width / 2.0),

                oClientRectangle.Top + MajorGridlineLength
                    + MajorGridlineMargin + ValueFontSizeEm + LabelMargin
            ) );
        }
        else
        {
            DrawRotatedText(oDrawingContext, oFormattedText,

                oClientRectangle.Right - MajorGridlineLength
                    - MajorGridlineMargin - ValueFontSizeEm - LabelMargin
                    - m_dLabelFontSizeEm,

                oClientRectangle.Top + (oClientRectangle.Height / 2.0)
                );
        }
    }

    //*************************************************************************
    //  Method: DrawRotatedText()
    //
    /// <summary>
    /// Draws rotated text.
    /// </summary>
    ///
    /// <param name="oDrawingContext">
    /// The DrawingContext to draw with.
    /// </param>
    ///
    /// <param name="oFormattedText">
    /// The text to draw.
    /// </param>
    ///
    /// <param name="dCenterX">
    /// x-coordinate where the top-center of the text will be drawn
    /// </param>
    ///
    /// <param name="dCenterY">
    /// y-coordinate where the top-center of the text will be drawn
    /// </param>
    ///
    /// <remarks>
    /// The top-center of the text will appear at (dCenterX, dCenterY), rotated
    /// 270 degrees.
    /// </remarks>
    //*************************************************************************

    protected void
    DrawRotatedText
    (
        DrawingContext oDrawingContext,
        FormattedText oFormattedText,
        Double dCenterX,
        Double dCenterY
    )
    {
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oFormattedText != null);
        AssertValid();

        oDrawingContext.PushTransform( GetRotatedTextTransform(
            dCenterX, dCenterY) );

        oDrawingContext.DrawText( oFormattedText, new Point(0, 0) );
        oDrawingContext.Pop();
    }

    //*************************************************************************
    //  Method: GetFormatSpecifier()
    //
    /// <summary>
    /// Gets the format specifier to use for gridline values.
    /// </summary>
    ///
    /// <param name="adMajorGridlineValues">
    /// The gridline values returned by ChartUtil.GetAxisGridlineValues().
    /// </param>
    ///
    /// <param name="iDecimalPlacesToShow">
    /// The number of decimal places recommended by
    /// ChartUtil.GetAxisGridlineValues().
    /// </param>
    ///
    /// <returns>
    /// The format specifier to use.
    /// </returns>
    //*************************************************************************

    protected String
    GetFormatSpecifier
    (
        Double [] adMajorGridlineValues,
        Int32 iDecimalPlacesToShow
    )
    {
        Debug.Assert(adMajorGridlineValues != null);
        Debug.Assert(adMajorGridlineValues.Length >= 2);
        Debug.Assert(iDecimalPlacesToShow >= 0);
        AssertValid();

        Double dFirstGridlineValue = Math.Abs( adMajorGridlineValues[0] );

        Double dLastGridlineValue = Math.Abs( adMajorGridlineValues[
            adMajorGridlineValues.Length - 1] );

        if (Math.Max(dFirstGridlineValue, dLastGridlineValue) > 100000)
        {
            return ("e3");
        }

        return ( "N" + iDecimalPlacesToShow.ToString() );
    }

    //*************************************************************************
    //  Method: GetCenteredFormattedText()
    //
    /// <summary>
    /// Gets a FormattedText object configured for centered text.
    /// </summary>
    ///
    /// <param name="sText">
    /// The text that will be drawn.  Can't be null.
    /// </param>
    ///
    /// <param name="oTypeface">
    /// The Typeface to use.
    /// </param>
    ///
    /// <param name="dFontSizeEm">
    /// The font size to use, in Ems.
    /// </param>
    ///
    /// <returns>
    /// A new FormattedText object configured for centered text.
    /// </returns>
    //*************************************************************************

    protected FormattedText
    GetCenteredFormattedText
    (
        String sText,
        Typeface oTypeface,
        Double dFontSizeEm
    )
    {
        Debug.Assert(sText != null);
        Debug.Assert(oTypeface != null);
        AssertValid();

        FormattedText oFormattedText = new FormattedText(sText,
            System.Globalization.CultureInfo.CurrentCulture,
            FlowDirection.LeftToRight, oTypeface, dFontSizeEm,
            SystemColors.WindowTextBrush);

        oFormattedText.TextAlignment = TextAlignment.Center;

        return (oFormattedText);
    }

    //*************************************************************************
    //  Method: GetRotatedTextTransform()
    //
    /// <summary>
    /// Gets a Transform suitable for drawing rotated text.
    /// </summary>
    ///
    /// <param name="dCenterX">
    /// x-coordinate where the top-center of the text will be drawn
    /// </param>
    ///
    /// <param name="dCenterY">
    /// y-coordinate where the top-center of the text will be drawn
    /// </param>
    ///
    /// <returns>
    /// A Transform that will draw rotated text.
    /// </returns>
    ///
    /// <remarks>
    /// After the returned Transform is pushed onto the DrawingContext, text
    /// drawn at (0,0) will have its top-center appear at (dCenterX, dCenterY),
    /// rotated 270 degrees.
    /// </remarks>
    //*************************************************************************

    protected Transform
    GetRotatedTextTransform
    (
        Double dCenterX,
        Double dCenterY
    )
    {
        AssertValid();

        TranslateTransform oTranslateTransform = new TranslateTransform();
        RotateTransform oRotateTransform = new RotateTransform(270);
        TransformGroup oTransformGroup = new TransformGroup();
        oTransformGroup.Children.Add(oTranslateTransform);
        oTransformGroup.Children.Add(oRotateTransform);

        oTranslateTransform.X = oRotateTransform.CenterX = dCenterX;
        oTranslateTransform.Y = oRotateTransform.CenterY = dCenterY;

        return (oTransformGroup);
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
        // m_bIsXAxis
        // m_sLabel
        // m_dNearValue
        // m_dFarValue
        Debug.Assert(m_dNearOffset >= 0);
        Debug.Assert(m_dFarOffset >= 0);
        Debug.Assert(m_oDockedControlRenderTransform != null);
        Debug.Assert(m_oTypeface != null);
        Debug.Assert(m_dLabelFontSizeEm > 0);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Font size to use for values, as a multiple of m_dLabelFontSizeEm.

    protected const Double ValueFontSizeFactor = 9.0 / 11.0;

    /// Length of a major gridline.

    protected const Double MajorGridlineLength = 9;

    /// Length of a minor gridline.

    protected const Double MinorGridlineLength = MajorGridlineLength * 0.5;

    /// Margin between the end of a major gridline and the gridline's value,
    /// as a multiple of m_dLabelFontSize.

    protected const Double MajorGridlineMarginFactor =
        ValueFontSizeFactor * 0.2;

    /// Margin between the gridline value and the axis label, as a multiple of
    /// m_dLabelFontSize.

    protected const Double LabelMarginFactor = ValueFontSizeFactor * 0.3;

    /// Number of minor gridlines between each pair of major gridlines.

    protected const Int32 MinorGridlines = 5;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// true if this is the x-axis.

    protected Boolean m_bIsXAxis;

    /// The label for the axis.

    protected String m_sLabel;

    /// The values at the near and far ends of the axis.

    protected Double m_dNearValue;
    ///
    protected Double m_dFarValue;

    /// The offset of m_dNearValue from the near end of the axis.

    protected Double m_dNearOffset;

    /// The offset of m_dFarValue from the far end of the axis.

    protected Double m_dFarOffset;

    /// The docked control's render transform.

    protected Transform m_oDockedControlRenderTransform;

    /// The Typeface to use to draw the label and values.

    protected Typeface m_oTypeface;

    /// The font size to use to draw the label.  (A slightly smaller size is
    /// used for the axis values.)

    protected Double m_dLabelFontSizeEm;
}

}
