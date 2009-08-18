
//  Copyright (c) Microsoft Corporation.  All rights reserved.

﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: DynamicFiltersLegendControl
//
/// <summary>
/// Displays a graph legend for the dynamic filters that have been applied to
/// the graph.
/// </summary>
///
/// <remarks>
/// Call <see cref="Update" /> whenever the dynamic filters change.  Call <see
/// cref="Clear" /> to clear the legend.
///
/// <para>
/// <see cref="Update" /> sets the control's height to allow the entire legend
/// to fit within the control.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class DynamicFiltersLegendControl : LegendControlBase
{
    //*************************************************************************
    //  Constructor: DynamicFiltersLegendControl()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="DynamicFiltersLegendControl" /> class.
    /// </summary>
    //*************************************************************************

    public DynamicFiltersLegendControl()
    {
        // Start with empty collections of filter track bars.

        ICollection<IDynamicFilterRangeTrackBar> oEmptyCollection =
            CreateEmptyCollection();

        m_oEdgeDynamicFilterRangeTrackBars =
            m_oVertexDynamicFilterRangeTrackBars = oEmptyCollection;

        this.DoubleBuffered = true;
        this.BackColor = SystemColors.Window;

        Debug.Assert(MinimumSelectedLineWidth % 2 == 0);
        AssertValid();
    }

    //*************************************************************************
    //  Method: Update()
    //
    /// <summary>
    /// Updates the graph legend.
    /// </summary>
    ///
    /// <param name="edgeDynamicFilterRangeTrackBars">
    /// The edge dynamic filter track bars.  Can't be null.
    /// </param>
    ///
    /// <param name="vertexDynamicFilterRangeTrackBars">
    /// The vertex dynamic track bars.  Can't be null.
    /// </param>
    //*************************************************************************

    public void
    Update
    (
        ICollection<IDynamicFilterRangeTrackBar>
            edgeDynamicFilterRangeTrackBars,

        ICollection<IDynamicFilterRangeTrackBar>
            vertexDynamicFilterRangeTrackBars
    )
    {
        Debug.Assert(edgeDynamicFilterRangeTrackBars != null);
        Debug.Assert(vertexDynamicFilterRangeTrackBars != null);
        AssertValid();

        m_oEdgeDynamicFilterRangeTrackBars = edgeDynamicFilterRangeTrackBars;

        m_oVertexDynamicFilterRangeTrackBars =
            vertexDynamicFilterRangeTrackBars;

        this.Height = CalculateHeight();
        Invalidate();
    }

    //*************************************************************************
    //  Method: Clear()
    //
    /// <summary>
    /// Clears the graph legend.
    /// </summary>
    //*************************************************************************

    public void
    Clear()
    {
        AssertValid();

        ICollection<IDynamicFilterRangeTrackBar> oEmptyCollection =
            CreateEmptyCollection();

        Update(oEmptyCollection, oEmptyCollection);
    }

    //*************************************************************************
    //  Method: CreateEmptyCollection()
    //
    /// <summary>
    /// Creates an empty collection of IDynamicFilterRangeTrackBar objects.
    /// </summary>
    ///
    /// <returns>
    /// An empty collection of IDynamicFilterRangeTrackBar objects.
    /// </returns>
    //*************************************************************************

    public ICollection<IDynamicFilterRangeTrackBar>
    CreateEmptyCollection()
    {
        // AssertValid();

        return ( new LinkedList<IDynamicFilterRangeTrackBar>() );
    }

    //*************************************************************************
    //  Method: CalculateHeight()
    //
    /// <summary>
    /// Calculates what the control height should be.
    /// </summary>
    ///
    /// <returns>
    /// The calculated control height.
    /// </returns>
    ///
    /// <remarks>
    /// The height is calculated to contain the entire legend.
    /// </remarks>
    //*************************************************************************

    protected Int32
    CalculateHeight()
    {
        AssertValid();

        // Draw the control using a completely clipped region.

        return ( Draw( CreateClippedDrawingObjects() ) );
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

        base.OnPaint(e);
        Draw( CreateDrawingObjects(e.Graphics, this.ClientRectangle) );
    }

    //*************************************************************************
    //  Method: Draw()
    //
    /// <summary>
    /// Draws the control.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <returns>
    /// The bottom of the area that was drawn.
    /// </returns>
    //*************************************************************************

    protected Int32
    Draw
    (
        DrawingObjects oDrawingObjects
    )
    {
        Debug.Assert(oDrawingObjects != null);
        AssertValid();

        // If there are edge track bars and vertex track bars, two columns are
        // drawn, with the edge track bars in the first column and the vertex
        // track bars in the second.  If there are only edge or vertex track
        // bars, one column is drawn.

        Rectangle oColumn1Rectangle, oColumn2Rectangle;
        Boolean bHasTwoColumns = false;

        if (m_oEdgeDynamicFilterRangeTrackBars.Count > 0 &&
            m_oVertexDynamicFilterRangeTrackBars.Count > 0)
        {
            ControlRectangleToTwoColumns(oDrawingObjects,
                out oColumn1Rectangle, out oColumn2Rectangle);

            bHasTwoColumns = true;
        }
        else
        {
            oColumn1Rectangle = oColumn2Rectangle =
                oDrawingObjects.ControlRectangle;
        }

        Int32 iBottom = Math.Max(

            DrawDynamicFilterRangeTrackBars(oDrawingObjects, "Edge Filters",
                m_oEdgeDynamicFilterRangeTrackBars, oColumn1Rectangle),

            DrawDynamicFilterRangeTrackBars(oDrawingObjects, "Vertex Filters",
                m_oVertexDynamicFilterRangeTrackBars, oColumn2Rectangle)
            );

        if (bHasTwoColumns)
        {
            DrawColumnSeparator(oDrawingObjects, oColumn2Rectangle);
        }

        return (iBottom);
    }

    //*************************************************************************
    //  Method: DrawDynamicFilterRangeTrackBars()
    //
    /// <summary>
    /// Draws representations of a group of dynamic filter track bars.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="sGroupName">
    /// Name of the dynamic filter group.
    /// </param>
    ///
    /// <param name="oDynamicFilterRangeTrackBars">
    /// The track bars to draw.
    /// </param>
    ///
    /// <param name="oColumnRectangle">
    /// Rectangle to draw the group of track bars within.
    /// </param>
    ///
    /// <returns>
    /// The bottom of the area that was drawn.
    /// </returns>
    //*************************************************************************

    protected Int32
    DrawDynamicFilterRangeTrackBars
    (
        DrawingObjects oDrawingObjects,
        String sGroupName,
        ICollection<IDynamicFilterRangeTrackBar> oDynamicFilterRangeTrackBars,
        Rectangle oColumnRectangle
    )
    {
        Debug.Assert(oDrawingObjects != null);
        Debug.Assert( !String.IsNullOrEmpty(sGroupName) );
        Debug.Assert(oDynamicFilterRangeTrackBars != null);
        AssertValid();

        Int32 iTop = oColumnRectangle.Top;

        if (oDynamicFilterRangeTrackBars.Count > 0)
        {
            // Draw the group name at the top of the group.

            DrawColumnHeader(oDrawingObjects, sGroupName, oColumnRectangle.Left,
                oColumnRectangle.Right, ref iTop);

            // Provide a margin.

            Rectangle oColumnRectangleWithMargin = AddMarginToRectangle(
                oDrawingObjects, oColumnRectangle);

            Int32 iTrackBarLeft = oColumnRectangleWithMargin.Left;
            Int32 iTrackBarRight = oColumnRectangleWithMargin.Right;

            Pen oAvailableRangeLinePen = new Pen(SystemBrushes.ControlDark,
                AvailableLineHeight);

            Pen oSelectedRangeLinePen = new Pen(SystemBrushes.Highlight,
                SelectedLineHeight);

            foreach (IDynamicFilterRangeTrackBar oDynamicFilterRangeTrackBar in
                oDynamicFilterRangeTrackBars)
            {
                DrawDynamicFilterRangeTrackBar(oDrawingObjects,
                    oDynamicFilterRangeTrackBar, oAvailableRangeLinePen,
                    oSelectedRangeLinePen, oColumnRectangle, iTrackBarLeft,
                    iTrackBarRight, ref iTop);
            }

            oAvailableRangeLinePen.Dispose();
            oSelectedRangeLinePen.Dispose();
        }

        return (iTop);
    }

    //*************************************************************************
    //  Method: DrawDynamicFilterRangeTrackBar()
    //
    /// <summary>
    /// Draws a representation of a dynamic filter track bar.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="oDynamicFilterRangeTrackBar">
    /// The track bar to draw.
    /// </param>
    ///
    /// <param name="oAvailableRangeLinePen">
    /// Pen to use to draw the available range line.
    /// </param>
    ///
    /// <param name="oSelectedRangeLinePen">
    /// Pen to use to draw the selected range line.
    /// </param>
    ///
    /// <param name="oColumnRectangle">
    /// Rectangle to draw the group of track bars within.
    /// </param>
    ///
    /// <param name="iTrackBarLeft">
    /// Left x-coordinate where the track bar should be drawn.
    /// </param>
    ///
    /// <param name="iTrackBarRight">
    /// Right x-coordinate where the track bar should be drawn.
    /// </param>
    ///
    /// <param name="iTop">
    /// Top y-coordinate where the track bar should be drawn.  Gets updated.
    /// </param>
    //*************************************************************************

    protected void
    DrawDynamicFilterRangeTrackBar
    (
        DrawingObjects oDrawingObjects,
        IDynamicFilterRangeTrackBar oDynamicFilterRangeTrackBar,
        Pen oAvailableRangeLinePen,
        Pen oSelectedRangeLinePen,
        Rectangle oColumnRectangle,
        Int32 iTrackBarLeft,
        Int32 iTrackBarRight,
        ref Int32 iTop
    )
    {
        Debug.Assert(oDrawingObjects != null);
        Debug.Assert(oDynamicFilterRangeTrackBar != null);
        Debug.Assert(oAvailableRangeLinePen != null);
        Debug.Assert(oSelectedRangeLinePen != null);

        iTop += oDrawingObjects.GetFontHeightMultiple(0.2F);

        // Draw the text for the available range.

        Int32 iAvailableRangeTextLeft = iTrackBarLeft;
        Int32 iAvailableRangeTextRight = iTrackBarRight;
        Int32 iAvailableRangeTextTop = iTop;

        DrawAvailableRangeText(oDrawingObjects, oDynamicFilterRangeTrackBar,
            ref iAvailableRangeTextLeft, ref iAvailableRangeTextRight,
            ref iTop);

        // Draw the line representing the available range.

        Int32 iLineMargin = oDrawingObjects.GetFontHeightMultiple(0.1F);
        Int32 iAvailableRangeLineLeft = iTrackBarLeft + iLineMargin;
        Int32 iAvailableRangeLineRight = iTrackBarRight - iLineMargin;

        oDrawingObjects.Graphics.DrawLine(oAvailableRangeLinePen,
            iAvailableRangeLineLeft, iTop, iAvailableRangeLineRight, iTop);

        // Draw the line representing the selected range.

        Int32 iSelectedRangeLineLeft, iSelectedRangeLineRight;

        DrawSelectedRangeLine(oDrawingObjects, oDynamicFilterRangeTrackBar,
            oSelectedRangeLinePen, iAvailableRangeLineLeft,
            iAvailableRangeLineRight, out iSelectedRangeLineLeft,
            out iSelectedRangeLineRight, ref iTop);

        // Draw the text for the selected range.

        DrawSelectedRangeText(oDrawingObjects, oDynamicFilterRangeTrackBar,
            iAvailableRangeTextLeft, iAvailableRangeTextRight,
            iSelectedRangeLineLeft, iSelectedRangeLineRight,
            iAvailableRangeTextTop);

        DrawExcelColumnName(oDrawingObjects,
            oDynamicFilterRangeTrackBar.ColumnName, iTrackBarLeft,
            iTrackBarRight, ref iTop);

        iTop += oDrawingObjects.GetFontHeightMultiple(0.3F);

        // Draw a line separating this track bar from the next.

        DrawHorizontalSeparator(oDrawingObjects, oColumnRectangle, ref iTop);
    }

    //*************************************************************************
    //  Method: DrawAvailableRangeText()
    //
    /// <summary>
    /// Draws the text for the available range.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="oDynamicFilterRangeTrackBar">
    /// The track bar being drawn.
    /// </param>
    ///
    /// <param name="iAvailableRangeTextLeft">
    /// x-coordinate of the left edge of the avilable range text.  Gets
    /// updated.
    /// </param>
    ///
    /// <param name="iAvailableRangeTextRight">
    /// x-coordinate of the right edge of the avilable range text.  Gets
    /// updated.
    /// </param>
    ///
    /// <param name="iTop">
    /// Top y-coordinate where the text should be drawn.  Gets updated.
    /// </param>
    //*************************************************************************

    protected void
    DrawAvailableRangeText
    (
        DrawingObjects oDrawingObjects,
        IDynamicFilterRangeTrackBar oDynamicFilterRangeTrackBar,
        ref Int32 iAvailableRangeTextLeft,
        ref Int32 iAvailableRangeTextRight,
        ref Int32 iTop
    )
    {
        Debug.Assert(oDrawingObjects != null);
        Debug.Assert(oDynamicFilterRangeTrackBar != null);
        AssertValid();

        DrawRangeText(oDrawingObjects,

            oDynamicFilterRangeTrackBar.ValueToString(
                oDynamicFilterRangeTrackBar.AvailableMinimum),

            oDynamicFilterRangeTrackBar.ValueToString(
                oDynamicFilterRangeTrackBar.AvailableMaximum),

            SystemBrushes.GrayText, ref iAvailableRangeTextLeft,
            ref iAvailableRangeTextRight, ref iTop
            );

        iTop += oDrawingObjects.GetFontHeightMultiple(0.4F);
    }

    //*************************************************************************
    //  Method: DrawSelectedRangeLine()
    //
    /// <summary>
    /// Draws the line representing the selected range.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="oDynamicFilterRangeTrackBar">
    /// The track bar being drawn.
    /// </param>
    ///
    /// <param name="oSelectedRangeLinePen">
    /// Pen to use.
    /// </param>
    ///
    /// <param name="iAvailableRangeLineLeft">
    /// x-coordinate of the left edge of the available range line.
    /// </param>
    ///
    /// <param name="iAvailableRangeLineRight">
    /// x-coordinate of the right edge of the available range line.
    /// </param>
    ///
    /// <param name="iSelectedRangeLineLeft">
    /// Where the x-coordinate of the left edge of the line gets stored.
    /// </param>
    ///
    /// <param name="iSelectedRangeLineRight">
    /// Where the x-coordinate of the right edge of the line gets stored.
    /// </param>
    ///
    /// <param name="iTop">
    /// Top y-coordinate where the line should be drawn.  Gets updated.
    /// </param>
    //*************************************************************************

    protected void
    DrawSelectedRangeLine
    (
        DrawingObjects oDrawingObjects,
        IDynamicFilterRangeTrackBar oDynamicFilterRangeTrackBar,
        Pen oSelectedRangeLinePen,
        Int32 iAvailableRangeLineLeft,
        Int32 iAvailableRangeLineRight,
        out Int32 iSelectedRangeLineLeft,
        out Int32 iSelectedRangeLineRight,
        ref Int32 iTop
    )
    {
        Debug.Assert(oDrawingObjects != null);
        Debug.Assert(oDynamicFilterRangeTrackBar != null);
        Debug.Assert(oSelectedRangeLinePen != null);
        AssertValid();

        iSelectedRangeLineLeft = GetXWithinAvailableLine(
            oDynamicFilterRangeTrackBar.SelectedMinimum,
            oDynamicFilterRangeTrackBar, iAvailableRangeLineLeft,
            iAvailableRangeLineRight);

        iSelectedRangeLineRight = GetXWithinAvailableLine(
            oDynamicFilterRangeTrackBar.SelectedMaximum,
            oDynamicFilterRangeTrackBar, iAvailableRangeLineLeft,
            iAvailableRangeLineRight);

        // Don't let the line have zero width.

        if (iSelectedRangeLineRight == iSelectedRangeLineLeft)
        {
            if (iSelectedRangeLineLeft == iAvailableRangeLineLeft)
            {
                iSelectedRangeLineRight += MinimumSelectedLineWidth;
            }
            else if (iSelectedRangeLineRight == iAvailableRangeLineRight)
            {
                iSelectedRangeLineLeft -= MinimumSelectedLineWidth;
            }
            else
            {
                iSelectedRangeLineLeft -= (MinimumSelectedLineWidth / 2);
                iSelectedRangeLineRight += (MinimumSelectedLineWidth / 2);
            }
        }

        oDrawingObjects.Graphics.DrawLine(oSelectedRangeLinePen,
            iSelectedRangeLineLeft, iTop, iSelectedRangeLineRight, iTop);

        iTop += AvailableLineHeight;
    }

    //*************************************************************************
    //  Method: DrawSelectedRangeText()
    //
    /// <summary>
    /// Draws the text for the selected range.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="oDynamicFilterRangeTrackBar">
    /// The track bar being drawn.
    /// </param>
    ///
    /// <param name="iAvailableRangeTextLeft">
    /// Minimum x-coordinate where the text can be drawn.
    /// </param>
    ///
    /// <param name="iAvailableRangeTextRight">
    /// Maximum x-coordinate where the text can be drawn.
    /// </param>
    ///
    /// <param name="iSelectedRangeLineLeft">
    /// x-coordinate of the left edge of the selected range line.
    /// </param>
    ///
    /// <param name="iSelectedRangeLineRight">
    /// x-coordinate of the right edge of the selected range line.
    /// </param>
    ///
    /// <param name="iTop">
    /// Top y-coordinate where the text should be drawn.
    /// </param>
    //*************************************************************************

    protected void
    DrawSelectedRangeText
    (
        DrawingObjects oDrawingObjects,
        IDynamicFilterRangeTrackBar oDynamicFilterRangeTrackBar,
        Int32 iAvailableRangeTextLeft,
        Int32 iAvailableRangeTextRight,
        Int32 iSelectedRangeLineLeft,
        Int32 iSelectedRangeLineRight,
        Int32 iTop
    )
    {
        Debug.Assert(oDrawingObjects != null);
        Debug.Assert(oDynamicFilterRangeTrackBar != null);
        AssertValid();

        // Measure the minimum and maximum selected values.

        String sSelectedMinimumText =
            oDynamicFilterRangeTrackBar.ValueToString(
                oDynamicFilterRangeTrackBar.SelectedMinimum);

        Int32 iSelectedMinimumTextWidth = MeasureTextWidth(oDrawingObjects,
            sSelectedMinimumText);

        String sSelectedMaximumText =
            oDynamicFilterRangeTrackBar.ValueToString(
                oDynamicFilterRangeTrackBar.SelectedMaximum);

        Int32 iSelectedMaximumTextWidth = MeasureTextWidth(
            oDrawingObjects, sSelectedMaximumText);

        // If there is enough room, draw the selected minimum text with its
        // right edge aligned with the left edge of the selected line, and the
        // selected maximum text with its left edge aligned with the right edge
        // of the selected line.  Don't let any text exceed the bounds
        // specified by iAvailableRangeTextLeft and iAvailableRangeTextRight,
        // and don't let the text run into each other.

        Int32 iSelectedMinimumTextRight = Math.Max(
            iSelectedRangeLineLeft,
            iAvailableRangeTextLeft + iSelectedMinimumTextWidth
            );

        iSelectedMinimumTextRight = Math.Min(
            iSelectedMinimumTextRight,
            iAvailableRangeTextRight - iSelectedMaximumTextWidth
            );

        Int32 iSelectedMaximumTextLeft = Math.Min(
            iSelectedRangeLineRight,
            iAvailableRangeTextRight - iSelectedMaximumTextWidth
            );

        iSelectedMaximumTextLeft = Math.Max(
            iSelectedMaximumTextLeft,
            iAvailableRangeTextLeft + iSelectedMinimumTextWidth
            );

        if (iSelectedMinimumTextRight - iSelectedMinimumTextWidth >=
            iAvailableRangeTextLeft)
        {
            oDrawingObjects.Graphics.DrawString(sSelectedMinimumText,
                oDrawingObjects.Font, SystemBrushes.Highlight,
                iSelectedMinimumTextRight, iTop,
                oDrawingObjects.RightAlignStringFormat);
        }

        if (iSelectedMaximumTextLeft + iSelectedMaximumTextWidth <=
            iAvailableRangeTextRight)
        {
            oDrawingObjects.Graphics.DrawString(sSelectedMaximumText,
                oDrawingObjects.Font, SystemBrushes.Highlight,
                iSelectedMaximumTextLeft, iTop);
        }
    }

    //*************************************************************************
    //  Method: GetXWithinAvailableLine()
    //
    /// <summary>
    /// Gets the x-coordinate of a value on the line that represents the
    /// available range.
    /// </summary>
    ///
    /// <param name="decValue">
    /// A value within the available range.
    /// </param>
    ///
    /// <param name="oDynamicFilterRangeTrackBar">
    /// The track bar being drawn.
    /// </param>
    ///
    /// <param name="iAvailableLineLeft">
    /// x-coordinate of the left end of the available range line.
    /// </param>
    ///
    /// <param name="iAvailableLineRight">
    /// x-coordinate of the right end of the available range line.
    /// </param>
    ///
    /// <returns>
    /// The x-coordinate.
    /// </returns>
    //*************************************************************************

    protected Int32
    GetXWithinAvailableLine
    (
        Decimal decValue,
        IDynamicFilterRangeTrackBar oDynamicFilterRangeTrackBar,
        Int32 iAvailableLineLeft,
        Int32 iAvailableLineRight
    )
    {
        Debug.Assert(oDynamicFilterRangeTrackBar != null);
        Debug.Assert(iAvailableLineRight >= iAvailableLineLeft);
        AssertValid();

        Decimal decValueX = (Decimal)iAvailableLineLeft + 

            ( (Decimal)iAvailableLineRight - (Decimal)iAvailableLineLeft ) *
            (
                (decValue - oDynamicFilterRangeTrackBar.AvailableMinimum) /

                (oDynamicFilterRangeTrackBar.AvailableMaximum -
                    oDynamicFilterRangeTrackBar.AvailableMinimum)
            );

        return ( (Int32)decValueX );
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

        Debug.Assert(m_oEdgeDynamicFilterRangeTrackBars != null);
        Debug.Assert(m_oVertexDynamicFilterRangeTrackBars != null);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Height of the line representing a filter's available range.

    protected const Int32 AvailableLineHeight = 3;

    /// Height of the line representing a filter's selected range.

    protected const Int32 SelectedLineHeight = 5;

    /// Minimum width of the line representing a filter's selected range.  Must
    /// be a multiple of 2.

    protected const Int32 MinimumSelectedLineWidth = 4;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The edge dynamic filter track bars.

    protected ICollection<IDynamicFilterRangeTrackBar>
        m_oEdgeDynamicFilterRangeTrackBars;

    /// The vertex dynamic filter track bars.

    protected ICollection<IDynamicFilterRangeTrackBar>
        m_oVertexDynamicFilterRangeTrackBars;
}
}
