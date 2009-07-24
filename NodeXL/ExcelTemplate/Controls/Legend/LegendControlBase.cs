
//  Copyright (c) Microsoft Corporation.  All rights reserved.

﻿using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: LegendControlBase
//
/// <summary>
/// Base class for several classes that display a graph legend.
/// </summary>
//*****************************************************************************

public class LegendControlBase : Control
{
    //*************************************************************************
    //  Constructor: LegendControlBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="LegendControlBase" />
    /// class.
    /// </summary>
    //*************************************************************************

    public LegendControlBase()
    {
        // (Do nothing.)

        // AssertValid();
    }

    //*************************************************************************
    //  Method: CreateDrawingObjects()
    //
    /// <summary>
    /// Create the objects used to draw the control.
    /// </summary>
    ///
    /// <param name="oGraphics">
    /// Object to draw with.
    /// </param>
    ///
    /// <param name="oControlRectangle">
    /// Rectangle to draw the control within.
    /// </param>
    ///
    /// <returns>
    /// A DrawingObjects objects.
    /// </returns>
    //*************************************************************************

    protected DrawingObjects
    CreateDrawingObjects
    (
        Graphics oGraphics,
        Rectangle oControlRectangle
    )
    {
        Debug.Assert(oGraphics != null);
        AssertValid();

        DrawingObjects oDrawingObjects = new DrawingObjects();

        oDrawingObjects.Graphics = oGraphics;
        oDrawingObjects.ControlRectangle = oControlRectangle;
        oDrawingObjects.Font = this.Font;

        oDrawingObjects.FontHeight =
            oDrawingObjects.Font.GetHeight(oDrawingObjects.Graphics);

        oDrawingObjects.TrimmingStringFormat = new StringFormat();

        oDrawingObjects.TrimmingStringFormat.Trimming =
            StringTrimming.EllipsisCharacter;

        oDrawingObjects.RightAlignStringFormat = new StringFormat();
        oDrawingObjects.RightAlignStringFormat.Alignment = StringAlignment.Far;

        oDrawingObjects.CenterAlignStringFormat = new StringFormat();

        oDrawingObjects.CenterAlignStringFormat.Alignment =
            StringAlignment.Center;

        return (oDrawingObjects);
    }

    //*************************************************************************
    //  Method: CreateClippedDrawingObjects()
    //
    /// <summary>
    /// Create the objects used to draw the control using a clipped Graphics
    /// object.
    /// </summary>
    ///
    /// <returns>
    /// A DrawingObjects object with a clipped Graphics object.
    /// </returns>
    ///
    /// <remarks>
    /// The Graphics object within the returned DrawingObjects has its clip
    /// region set to empty.  This can be used to calculate the control's
    /// height without actually drawing it.
    /// </remarks>
    //*************************************************************************

    protected DrawingObjects
    CreateClippedDrawingObjects()
    {
        AssertValid();

        Graphics oGraphics = this.CreateGraphics();
        Region oRegion = new Region();
        oRegion.MakeEmpty();
        oGraphics.Clip = oRegion;

        Rectangle oControlRectangle =
            new Rectangle(0, 0, this.ClientRectangle.Width, 1000);

        return ( CreateDrawingObjects(oGraphics, oControlRectangle) );
    }

    //*************************************************************************
    //  Method: ControlRectangleToTwoColumns()
    //
    /// <summary>
    /// Splits the control rectangle into 2 columns.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="oColumn1Rectangle">
    /// Where the first column's rectangle gets stored.
    /// </param>
    ///
    /// <param name="oColumn2Rectangle">
    /// Where the second column's rectangle gets stored.
    /// </param>
    //*************************************************************************

    protected void
    ControlRectangleToTwoColumns
    (
        DrawingObjects oDrawingObjects,
        out Rectangle oColumn1Rectangle,
        out Rectangle oColumn2Rectangle
    )
    {
        Debug.Assert(oDrawingObjects != null);
        AssertValid();

        Rectangle oControlRectangle = oDrawingObjects.ControlRectangle;
        Int32 iTwoColumnWidth = GetTwoColumnWidth(oDrawingObjects);

        oColumn1Rectangle = new Rectangle(oControlRectangle.Left,
            oControlRectangle.Top, iTwoColumnWidth,
            oControlRectangle.Height);

        oColumn2Rectangle = oColumn1Rectangle;
        oColumn2Rectangle.Offset(iTwoColumnWidth, 0);
    }

    //*************************************************************************
    //  Method: GetTwoColumnWidth()
    //
    /// <summary>
    /// Gets the width to use for each column of a two-column legend.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <returns>
    /// The width of each of two columns.
    /// </returns>
    ///
    /// <remarks>
    /// The returned width is either an arbitrary width based on the font size
    /// or half the control width, whichever is greater.
    /// </remarks>
    //*************************************************************************

    protected Int32
    GetTwoColumnWidth
    (
        DrawingObjects oDrawingObjects
    )
    {
        Debug.Assert(oDrawingObjects != null);
        AssertValid();

        // Note for the DynamicFiltersLegendControl derived class:
        //
        // The widest filter will be one that filters on a date and time, so
        // the reliable way to compute a minimum width would be to base it on
        // the width of a date and time string.  This yields too great a width
        // for most other filters, however, so just use an arbitrary multiple
        // of the font size instead.

        return ( (Int32)Math.Max(
            oDrawingObjects.ControlRectangle.Width / 2F,
            8.5F * oDrawingObjects.FontHeight
            ) );
    }

    //*************************************************************************
    //  Method: AddMarginToRectangle()
    //
    /// <summary>
    /// Adds a margin to a rectangle.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="oRectangle">
    /// Rectangle without margin.
    /// </param>
    ///
    /// <returns>
    /// A copy of <paramref name="oRectangle" /> with a margin.
    /// </returns>
    //*************************************************************************

    protected Rectangle
    AddMarginToRectangle
    (
        DrawingObjects oDrawingObjects,
        Rectangle oRectangle
    )
    {
        Debug.Assert(oDrawingObjects != null);
        AssertValid();

        Rectangle oRectangleWithMargin = oRectangle;
        const Int32 Margin = -3;
        oRectangleWithMargin.Inflate(Margin, Margin);

        return (oRectangleWithMargin);
    }

    //*************************************************************************
    //  Method: DrawColumnHeader()
    //
    /// <summary>
    /// Draws a header at the top of a legend column.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="sHeaderText">
    /// Text to draw.
    /// </param>
    ///
    /// <param name="iLeft">
    /// Left x-coordinate.
    /// </param>
    ///
    /// <param name="iRight">
    /// Right x-coordinate.
    /// </param>
    ///
    /// <param name="iTop">
    /// Top y-coordinate.  Gets updated.
    /// </param>
    ///
    /// <remarks>
    /// This method assumes that the legend will be drawn as one or more
    /// columns, with a heading at the top of each column.
    /// </remarks>
    //*************************************************************************

    protected void
    DrawColumnHeader
    (
        DrawingObjects oDrawingObjects,
        String sHeaderText,
        Int32 iLeft,
        Int32 iRight,
        ref Int32 iTop
    )
    {
        Debug.Assert(oDrawingObjects != null);
        Debug.Assert( !String.IsNullOrEmpty(sHeaderText) );
        AssertValid();

        Int32 iColumnHeaderHeight = (Int32)(oDrawingObjects.FontHeight * 1.3F);

        oDrawingObjects.Graphics.FillRectangle(SystemBrushes.ControlLight,
            iLeft, iTop, iRight - iLeft + 1, iColumnHeaderHeight);

        oDrawingObjects.Graphics.DrawString(sHeaderText, oDrawingObjects.Font,
            SystemBrushes.ControlText,
            iLeft + (Int32)(oDrawingObjects.FontHeight * 0.1F),
            iTop + (Int32)(oDrawingObjects.FontHeight * 0.15F)
            );

        iTop += iColumnHeaderHeight;
    }

    //*************************************************************************
    //  Method: DrawExcelColumnName()
    //
    /// <summary>
    /// Draws the name of an Excel column.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="sColumnName">
    /// Name of the Excel column.
    /// </param>
    ///
    /// <param name="iLeft">
    /// Left x-coordinate where the column name should be drawn.
    /// </param>
    ///
    /// <param name="iRight">
    /// Right x-coordinate where the column name should be drawn.
    /// </param>
    ///
    /// <param name="iTop">
    /// Top y-coordinate where the column name should be drawn.  Gets updated.
    /// </param>
    //*************************************************************************

    protected void
    DrawExcelColumnName
    (
        DrawingObjects oDrawingObjects,
        String sColumnName,
        Int32 iLeft,
        Int32 iRight,
        ref Int32 iTop
    )
    {
        Debug.Assert(oDrawingObjects != null);
        Debug.Assert( !String.IsNullOrEmpty(sColumnName) );

        iTop += (Int32)(oDrawingObjects.FontHeight * 0.4F);

        oDrawingObjects.Graphics.DrawString(sColumnName + ':',
            oDrawingObjects.Font, SystemBrushes.ControlText,

            Rectangle.FromLTRB( iLeft, iTop, iRight,
                (Int32)(iTop + oDrawingObjects.FontHeight) ),

            oDrawingObjects.TrimmingStringFormat);

        iTop += (Int32)(oDrawingObjects.FontHeight * 1.05F);
    }

    //*************************************************************************
    //  Method: DrawRangeText()
    //
    /// <summary>
    /// Draws text describing a range.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="sLeftText">
    /// The text to draw at the left edge of the control.
    /// </param>
    ///
    /// <param name="sRightText">
    /// The text to draw at the right edge of the control.
    /// </param>
    ///
    /// <param name="oBrush">
    /// The brush to use.
    /// </param>
    ///
    /// <param name="iLeft">
    /// Left x-coordinate.
    /// </param>
    ///
    /// <param name="iRight">
    /// Right x-coordinate.
    /// </param>
    ///
    /// <param name="iTop">
    /// Top y-coordinate.  Gets updated.
    /// </param>
    //*************************************************************************

    protected void
    DrawRangeText
    (
        DrawingObjects oDrawingObjects,
        String sLeftText,
        String sRightText,
        Brush oBrush,
        Int32 iLeft,
        Int32 iRight,
        ref Int32 iTop
    )
    {
        Debug.Assert(oDrawingObjects != null);
        Debug.Assert( !String.IsNullOrEmpty(sLeftText) );
        Debug.Assert( !String.IsNullOrEmpty(sRightText) );
        Debug.Assert(oBrush != null);
        AssertValid();

        // Note for the DynamicFiltersLegendControl derived class:
        //
        // To avoid off-by-one or -two errors, use the same alignments that
        // will be used later to draw the selected minimum and maximum text.

        oDrawingObjects.Graphics.DrawString(sLeftText, oDrawingObjects.Font,
            oBrush,

            iLeft + oDrawingObjects.Graphics.MeasureString(
                sLeftText, oDrawingObjects.Font).Width, iTop,

            oDrawingObjects.RightAlignStringFormat);

        oDrawingObjects.Graphics.DrawString(sRightText, oDrawingObjects.Font,
            oBrush,

            iRight - oDrawingObjects.Graphics.MeasureString(
                sRightText, oDrawingObjects.Font).Width, iTop);

        iTop += (Int32)(oDrawingObjects.FontHeight * 1.2F);
    }

    //*************************************************************************
    //  Method: DrawHorizontalSeparator()
    //
    /// <summary>
    /// Draws a horizontal line at the bottom of a legend element.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="oColumnRectangle">
    /// Column rectangle without margin.
    /// </param>
    ///
    /// <param name="iTop">
    /// Top y-coordinate where the separator should be drawn.  Gets updated.
    /// </param>
    //*************************************************************************

    protected void
    DrawHorizontalSeparator
    (
        DrawingObjects oDrawingObjects,
        Rectangle oColumnRectangle,
        ref Int32 iTop
    )
    {
        Debug.Assert(oDrawingObjects != null);
        AssertValid();

        oDrawingObjects.Graphics.DrawLine(SystemPens.ControlDark,
            oColumnRectangle.Left, iTop, oColumnRectangle.Right, iTop);

        iTop += 1;
    }

    //*************************************************************************
    //  Method: DrawColumnSeparator()
    //
    /// <summary>
    /// Draws a vertical separator between the control's two columns.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="oColumn2Rectangle">
    /// The second column's rectangle.
    /// </param>
    //*************************************************************************

    protected void
    DrawColumnSeparator
    (
        DrawingObjects oDrawingObjects,
        Rectangle oColumn2Rectangle
    )
    {
        Debug.Assert(oDrawingObjects != null);
        AssertValid();

        oDrawingObjects.Graphics.DrawLine(SystemPens.ControlDark,
            oColumn2Rectangle.Left, oColumn2Rectangle.Top,
            oColumn2Rectangle.Left, oColumn2Rectangle.Bottom);
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
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)


    //*************************************************************************
    //  Embedded class: DrawingObjects
    //
    /// <summary>
    /// Contains objects used to draw the control.
    /// </summary>
    ///
    /// <remarks>
    /// One of these is created by <see cref="CreateDrawingObjects" /> and
    /// passed to the various drawing methods.  It saves having to pass too
    /// many arguments, and having to modify multiple method signatures when a
    /// new drawing object is added in the future.
    /// </remarks>
    //*************************************************************************

    protected class
    DrawingObjects
    {
        /// <summary>
        /// Object to draw with.
        /// </summary>

        public Graphics Graphics;

        /// <summary>
        /// Rectangle to draw the control within.
        /// </summary>

        public Rectangle ControlRectangle;

        /// <summary>
        /// Font to use.
        /// </summary>

        public Font Font;

        /// <summary>
        /// Height of the font.
        /// </summary>

        public Single FontHeight;

        /// <summary>
        /// StringFormat object for drawing trimmed text.
        /// </summary>

        public StringFormat TrimmingStringFormat;

        /// <summary>
        /// StringFormat object for drawing right-aligned text.
        /// </summary>

        public StringFormat RightAlignStringFormat;

        /// <summary>
        /// StringFormat object for drawing centered text.
        /// </summary>

        public StringFormat CenterAlignStringFormat;
    }
}
}
