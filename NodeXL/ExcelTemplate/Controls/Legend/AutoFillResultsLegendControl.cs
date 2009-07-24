
//  Copyright (c) Microsoft Corporation.  All rights reserved.

﻿using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using Microsoft.NodeXL.Visualization.Wpf;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AutoFillResultsLegendControl
//
/// <summary>
/// Displays a graph legend containing the results of the autofill or autofill
/// with scheme feature.
/// </summary>
///
/// <remarks>
/// Because the autofill and "autofill with scheme" features are mutually
/// exclusive, this control displays the results for one or the other, but not
/// both.  Call one of the Update overloads to specify which results to
/// display.  If both Update methods are called, only the results passed to the
/// most recent method call are displayed.
///
/// <para>
/// Call <see cref="Clear" /> to clear the legend.
/// </para>
///
/// <para>
/// Both Update methods set the control's height to allow the entire legend to
/// fit within the control.
/// </para>
///
/// <para>
/// See the <see cref="WorkbookAutoFiller" /> and <see
/// cref="WorkbookSchemeAutoFiller" /> classes for details on the autofill and
/// "autofill with scheme" features.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class AutoFillResultsLegendControl : LegendControlBase
{
    //*************************************************************************
    //  Constructor: AutoFillResultsLegendControl()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AutoFillResultsLegendControl" /> class.
    /// </summary>
    //*************************************************************************

    public AutoFillResultsLegendControl()
    {
        m_iLastResizeWidth = Int32.MinValue;

        // Start with empty (but not null) objects.

        m_oAutoFillWorkbookResults = new AutoFillWorkbookResults();

        m_oAutoFillWorkbookWithSchemeResults =
            new AutoFillWorkbookWithSchemeResults();

        this.DoubleBuffered = true;
        this.BackColor = SystemColors.Window;

        AssertValid();
    }

    //*************************************************************************
    //  Method: Update()
    //
    /// <overloads>
    /// Updates the graph legend.
    /// </overloads>
    ///
    /// <summary>
    /// Updates the graph legend with results for the autofill feature.
    /// </summary>
    ///
    /// <param name="autoFillWorkbookResults">
    /// The results of the autofill.
    /// </param>
    //*************************************************************************

    public void
    Update
    (
        AutoFillWorkbookResults autoFillWorkbookResults
    )
    {
        Debug.Assert(autoFillWorkbookResults != null);
        AssertValid();

        Update( autoFillWorkbookResults,
            new AutoFillWorkbookWithSchemeResults() );
    }

    //*************************************************************************
    //  Method: Update()
    //
    /// <summary>
    /// Updates the graph legend with results for the "autofill with scheme"
    /// feature.
    /// </summary>
    ///
    /// <param name="autoFillWorkbookWithSchemeResults">
    /// The results of the "autofill with scheme."
    /// </param>
    //*************************************************************************

    public void
    Update
    (
        AutoFillWorkbookWithSchemeResults
            autoFillWorkbookWithSchemeResults
    )
    {
        Debug.Assert(autoFillWorkbookWithSchemeResults != null);
        AssertValid();

        Update(new AutoFillWorkbookResults(),
            autoFillWorkbookWithSchemeResults);
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

        Update( new AutoFillWorkbookResults(),
            new AutoFillWorkbookWithSchemeResults() );
    }

    //*************************************************************************
    //  Method: Update()
    //
    /// <summary>
    /// Updates the graph legend.
    /// </summary>
    ///
    /// <param name="oAutoFillWorkbookResults">
    /// The results of the autofill.
    /// </param>
    ///
    /// <param name="oAutoFillWorkbookWithSchemeResults">
    /// The results of the "autofill with scheme."
    /// </param>
    //*************************************************************************

    protected void
    Update
    (
        AutoFillWorkbookResults oAutoFillWorkbookResults,

        AutoFillWorkbookWithSchemeResults
            oAutoFillWorkbookWithSchemeResults
    )
    {
        Debug.Assert(oAutoFillWorkbookResults != null);
        Debug.Assert(oAutoFillWorkbookWithSchemeResults != null);
        AssertValid();

        m_oAutoFillWorkbookResults = oAutoFillWorkbookResults;

        m_oAutoFillWorkbookWithSchemeResults =
            oAutoFillWorkbookWithSchemeResults;

        this.Height = CalculateHeight();
        Invalidate();
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
    //  Method: OnResize()
    //
    /// <summary>
    /// Handles the Resize event.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event arguments.
    /// </param>
    //*************************************************************************

    protected override
    void OnResize
    (
        EventArgs e
    )
    {
        AssertValid();

        base.OnResize(e);

        Int32 iWidth = this.ClientRectangle.Width;

        if (iWidth != m_iLastResizeWidth &&

            m_oAutoFillWorkbookWithSchemeResults.SchemeType ==
                AutoFillSchemeType.VertexCategory)
        {
            // The control height for the vertex category scheme depends on the
            // control's width, which just changed.  Recalculate the control
            // height.

            this.Height = CalculateHeight();
        }

        m_iLastResizeWidth = iWidth;
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

        if (m_oAutoFillWorkbookResults.AutoFilledNonXYColumnCount > 0)
        {
            return ( DrawAutoFillWorkbookResults(oDrawingObjects) );
        }

        if (m_oAutoFillWorkbookWithSchemeResults.SchemeType !=
            AutoFillSchemeType.None)
        {
            return ( DrawAutoFillWorkbookWithSchemeResults(oDrawingObjects) );
        }

        return (oDrawingObjects.ControlRectangle.Top);
    }

    //*************************************************************************
    //  Method: DrawAutoFillWorkbookResults()
    //
    /// <summary>
    /// Draws the results of the autofill.
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
    DrawAutoFillWorkbookResults
    (
        DrawingObjects oDrawingObjects
    )
    {
        Debug.Assert(oDrawingObjects != null);
        AssertValid();

        // Two columns are drawn: The autofilled edge results are in the first
        // column, and the autofilled vertex results are in the second.

        Rectangle oColumn1Rectangle, oColumn2Rectangle;

        ControlRectangleToTwoColumns(oDrawingObjects, out oColumn1Rectangle,
            out oColumn2Rectangle);

        Int32 iBottom = Math.Max(
            DrawAutoFilledEdgeResults(oDrawingObjects, oColumn1Rectangle),
            DrawAutoFilledVertexResults(oDrawingObjects, oColumn2Rectangle)
            );

        DrawColumnSeparator(oDrawingObjects, oColumn2Rectangle);

        return (iBottom);
    }

    //*************************************************************************
    //  Method: DrawAutoFillWorkbookWithSchemeResults()
    //
    /// <summary>
    /// Draws the results of the "autofill with scheme."
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
    DrawAutoFillWorkbookWithSchemeResults
    (
        DrawingObjects oDrawingObjects
    )
    {
        Debug.Assert(oDrawingObjects != null);
        AssertValid();

        switch (m_oAutoFillWorkbookWithSchemeResults.SchemeType)
        {
            case AutoFillSchemeType.None:

                break;

            case AutoFillSchemeType.VertexCategory:

                return ( DrawVertexCategorySchemeResults(oDrawingObjects) );
    
            case AutoFillSchemeType.EdgeWeight:

                return ( DrawEdgeWeightSchemeResults(oDrawingObjects) );
    
            case AutoFillSchemeType.EdgeTimestamp:

                return ( DrawEdgeTimestampSchemeResults(oDrawingObjects) );
    
            default:

                Debug.Assert(false);
                break;
        }

        return (oDrawingObjects.ControlRectangle.Top);
    }

    //*************************************************************************
    //  Method: DrawAutoFilledEdgeResults()
    //
    /// <summary>
    /// Draws the results for autofilled edge columns.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="oColumnRectangle">
    /// Rectangle to draw the results within.
    /// </param>
    ///
    /// <returns>
    /// The bottom of the area that was drawn.
    /// </returns>
    //*************************************************************************

    protected Int32
    DrawAutoFilledEdgeResults
    (
        DrawingObjects oDrawingObjects,
        Rectangle oColumnRectangle
    )
    {
        Debug.Assert(oDrawingObjects != null);
        AssertValid();

        Int32 iTop = oColumnRectangle.Top;

        DrawColumnHeader(oDrawingObjects, "Autofilled Edge Properties",
            oColumnRectangle.Left, oColumnRectangle.Right, ref iTop);

        // Provide a margin.

        Rectangle oColumnRectangleWithMargin = AddMarginToRectangle(
            oDrawingObjects, oColumnRectangle);

        Int32 iResultsLeft = oColumnRectangleWithMargin.Left;
        Int32 iResultsRight = oColumnRectangleWithMargin.Right;

        AutoFillColorColumnResults oEdgeColorResults =
            m_oAutoFillWorkbookResults.EdgeColorResults;

        if (oEdgeColorResults.ColumnAutoFilled)
        {
            DrawColorBarResults(oDrawingObjects,
                oEdgeColorResults.SourceColumnName, ColorCaption,
                oEdgeColorResults.SourceCalculationNumber1,
                oEdgeColorResults.SourceCalculationNumber2,
                oEdgeColorResults.DestinationColor1,
                oEdgeColorResults.DestinationColor2,
                oColumnRectangle, iResultsLeft, iResultsRight, ref iTop);
        }

        AutoFillNumericRangeColumnResults oEdgeWidthResults =
            m_oAutoFillWorkbookResults.EdgeWidthResults;

        if (oEdgeWidthResults.ColumnAutoFilled)
        {
            DrawSteppedBarResults(oDrawingObjects,
                oEdgeWidthResults.SourceColumnName, EdgeWidthCaption,
                oEdgeWidthResults.SourceCalculationNumber1,
                oEdgeWidthResults.SourceCalculationNumber2,
                oEdgeWidthResults.DestinationNumber1,
                oEdgeWidthResults.DestinationNumber2,
                EdgeWidthConverter.MaximumWidthWorkbook,
                oColumnRectangle, iResultsLeft, iResultsRight, ref iTop);
        }

        AutoFillNumericRangeColumnResults oEdgeAlphaResults =
            m_oAutoFillWorkbookResults.EdgeAlphaResults;

        if (oEdgeAlphaResults.ColumnAutoFilled)
        {
            DrawAutoFilledAlphaResults(oDrawingObjects,
                oEdgeAlphaResults.SourceColumnName,
                oEdgeAlphaResults.SourceCalculationNumber1,
                oEdgeAlphaResults.SourceCalculationNumber2,
                oEdgeAlphaResults.DestinationNumber1,
                oEdgeAlphaResults.DestinationNumber2,
                oColumnRectangle, iResultsLeft, iResultsRight, ref iTop);
        }

        return (iTop);
    }

    //*************************************************************************
    //  Method: DrawAutoFilledVertexResults()
    //
    /// <summary>
    /// Draws the results for autofilled vertex columns.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="oColumnRectangle">
    /// Rectangle to draw the results within.
    /// </param>
    ///
    /// <returns>
    /// The bottom of the area that was drawn.
    /// </returns>
    //*************************************************************************

    protected Int32
    DrawAutoFilledVertexResults
    (
        DrawingObjects oDrawingObjects,
        Rectangle oColumnRectangle
    )
    {
        Debug.Assert(oDrawingObjects != null);
        AssertValid();

        Int32 iTop = oColumnRectangle.Top;

        DrawColumnHeader(oDrawingObjects, "Autofilled Vertex Properties",
            oColumnRectangle.Left, oColumnRectangle.Right, ref iTop);

        // Provide a margin.

        Rectangle oColumnRectangleWithMargin = AddMarginToRectangle(
            oDrawingObjects, oColumnRectangle);

        Int32 iResultsLeft = oColumnRectangleWithMargin.Left;
        Int32 iResultsRight = oColumnRectangleWithMargin.Right;

        AutoFillColorColumnResults oVertexColorResults =
            m_oAutoFillWorkbookResults.VertexColorResults;

        if (oVertexColorResults.ColumnAutoFilled)
        {
            DrawColorBarResults(oDrawingObjects,
                oVertexColorResults.SourceColumnName, ColorCaption,
                oVertexColorResults.SourceCalculationNumber1,
                oVertexColorResults.SourceCalculationNumber2,
                oVertexColorResults.DestinationColor1,
                oVertexColorResults.DestinationColor2,
                oColumnRectangle, iResultsLeft, iResultsRight, ref iTop);
        }

        AutoFillNumericRangeColumnResults oVertexRadiusResults =
            m_oAutoFillWorkbookResults.VertexRadiusResults;

        if (oVertexRadiusResults.ColumnAutoFilled)
        {
            DrawSteppedBarResults(oDrawingObjects,
                oVertexRadiusResults.SourceColumnName, "Size",
                oVertexRadiusResults.SourceCalculationNumber1,
                oVertexRadiusResults.SourceCalculationNumber2,
                oVertexRadiusResults.DestinationNumber1,
                oVertexRadiusResults.DestinationNumber2,
                VertexRadiusConverter.MaximumRadiusWorkbook, oColumnRectangle,
                iResultsLeft, iResultsRight, ref iTop);
        }

        AutoFillNumericRangeColumnResults oVertexAlphaResults =
            m_oAutoFillWorkbookResults.VertexAlphaResults;

        if (oVertexAlphaResults.ColumnAutoFilled)
        {
            DrawAutoFilledAlphaResults(oDrawingObjects,
                oVertexAlphaResults.SourceColumnName,
                oVertexAlphaResults.SourceCalculationNumber1,
                oVertexAlphaResults.SourceCalculationNumber2,
                oVertexAlphaResults.DestinationNumber1,
                oVertexAlphaResults.DestinationNumber2,
                oColumnRectangle, iResultsLeft, iResultsRight, ref iTop);
        }

        return (iTop);
    }

    //*************************************************************************
    //  Method: DrawAutoFilledAlphaResults()
    //
    /// <summary>
    /// Draws the results for one autofilled alpha column.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="sSourceColumnName">
    /// The name of the source column.
    /// </param>
    ///
    /// <param name="dSourceCalculationNumber1">
    /// The actual first source number used in the calculations.
    /// </param>
    ///
    /// <param name="dSourceCalculationNumber2">
    /// The actual second source number used in the calculations.
    /// </param>
    ///
    /// <param name="dDestinationNumber1">
    /// The first number used in the destination column.
    /// </param>
    ///
    /// <param name="dDestinationNumber2">
    /// The second number used in the destination column.
    /// </param>
    ///
    /// <param name="oColumnRectangle">
    /// Rectangle to draw the results within.
    /// </param>
    ///
    /// <param name="iResultsLeft">
    /// Left x-coordinate where the results should be drawn.
    /// </param>
    ///
    /// <param name="iResultsRight">
    /// Right x-coordinate where the results should be drawn.
    /// </param>
    ///
    /// <param name="iTop">
    /// Top y-coordinate where the results should be drawn.  Gets updated.
    /// </param>
    //*************************************************************************

    protected void
    DrawAutoFilledAlphaResults
    (
        DrawingObjects oDrawingObjects,
        String sSourceColumnName,
        Double dSourceCalculationNumber1,
        Double dSourceCalculationNumber2,
        Double dDestinationNumber1,
        Double dDestinationNumber2,
        Rectangle oColumnRectangle,
        Int32 iResultsLeft,
        Int32 iResultsRight,
        ref Int32 iTop
    )
    {
        Debug.Assert(oDrawingObjects != null);
        Debug.Assert( !String.IsNullOrEmpty(sSourceColumnName) );
        AssertValid();

        // The alpha results look like the color results, with the colors set
        // to black with different alphas.  Calculate the colors.

        AlphaConverter oAlphaConverter = new AlphaConverter();

        Int32 iDestinationAlpha1 =
            oAlphaConverter.WorkbookToGraph( (Single)dDestinationNumber1 );

        Int32 iDestinationAlpha2 =
            oAlphaConverter.WorkbookToGraph( (Single)dDestinationNumber2 );

        Color oDestinationColor1 =
            Color.FromArgb(iDestinationAlpha1, SystemColors.WindowText);

        Color oDestinationColor2 =
            Color.FromArgb(iDestinationAlpha2, SystemColors.WindowText);

        DrawColorBarResults(oDrawingObjects, sSourceColumnName, "Opacity",
            dSourceCalculationNumber1, dSourceCalculationNumber2,
            oDestinationColor1, oDestinationColor2, oColumnRectangle,
            iResultsLeft, iResultsRight, ref iTop);
    }

    //*************************************************************************
    //  Method: DrawVertexCategorySchemeResults()
    //
    /// <summary>
    /// Draws the results of the autofill with vertex category scheme.
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
    DrawVertexCategorySchemeResults
    (
        DrawingObjects oDrawingObjects
    )
    {
        Debug.Assert(oDrawingObjects != null);
        AssertValid();

        Graphics oGraphics = oDrawingObjects.Graphics;
        Rectangle oControlRectangle = oDrawingObjects.ControlRectangle;
        Int32 iTop = oControlRectangle.Top;

        DrawColumnHeader(oDrawingObjects, "Categorized Graph Scheme",
            oControlRectangle.Left, oControlRectangle.Right, ref iTop);

        iTop += (Int32)(oDrawingObjects.FontHeight * 0.4F);

        // The categories are drawn from left to right.  Each category is drawn
        // as a shape followed by the category name.
        //
        // Calculate the width of one category column.

        Single fShapeWidth = CategoryShapeFactor * oDrawingObjects.FontHeight;
        Single fShapeHalfWidth = fShapeWidth / 2F;

        Single fColumnWidth = (Int32)( fShapeWidth +
            (MaximumVertexCategoryNameLength * oDrawingObjects.FontHeight) );

        // How many columns will fit?  There must be at least one, even if
        // there isn't enough room for one column.

        Int32 iColumns = Math.Max( 1,
            (Int32)(oControlRectangle.Width / fColumnWidth) );

        String sVertexCategoryColumnName;
        String [] asVertexCategoryNames;

        m_oAutoFillWorkbookWithSchemeResults.GetVertexCategoryResults(
            out sVertexCategoryColumnName, out asVertexCategoryNames);

        Int32 iVertexCategories = asVertexCategoryNames.Length;
        Int32 iColumn = 0;
        Single fLeft = oControlRectangle.Left;

        Pen oPen = new Pen(Color.Black, 1.0F);
        oPen.Alignment = PenAlignment.Inset;

        SolidBrush oBrush = new SolidBrush(Color.Black);

        SmoothingMode eOldSmoothingMode = oGraphics.SmoothingMode;
        oGraphics.SmoothingMode = SmoothingMode.HighQuality;

        for (Int32 i = 0; i < iVertexCategories; i++)
        {
            String sVertexCategoryName = asVertexCategoryNames[i];

            // Get the shape and color for this category.  The radius is
            // ignored -- this method assigns its own sizes.

            VertexShape eVertexShape;
            Color oVertexColor;
            Single fVertexRadius;

            WorkbookSchemeAutoFiller.GetVertexCategoryScheme(i,
                out eVertexShape, out oVertexColor, out fVertexRadius);

            DrawVertexCategoryShape(oDrawingObjects, eVertexShape, oPen,
                oBrush, oVertexColor,
                fLeft + fShapeHalfWidth + 2,
                iTop + fShapeHalfWidth,
                fShapeHalfWidth);

            // Don't let the category name spill over into the next column.

            Single fTopOffset = fShapeWidth * 0.15F;

            Rectangle oNameRectangle = Rectangle.FromLTRB(
                (Int32)(fLeft + fShapeWidth * 1.4F),
                (Int32)(iTop + fTopOffset),
                (Int32)(fLeft + fColumnWidth),
                (Int32)(iTop + oDrawingObjects.FontHeight + fTopOffset)
                );

            oGraphics.DrawString(sVertexCategoryName, oDrawingObjects.Font,
                SystemBrushes.WindowText, oNameRectangle,
                oDrawingObjects.TrimmingStringFormat);

            if (iVertexCategories > MaximumVertexCategories &&
                i == MaximumVertexCategories - 1)
            {
                oGraphics.DrawString(
                    "There are additional categories that are not shown here.",
                    oDrawingObjects.Font, SystemBrushes.WindowText,
                    oControlRectangle.Left,
                    iTop + 1.5F * oDrawingObjects.FontHeight);

                iTop += (Int32)(3F * oDrawingObjects.FontHeight);
                break;
            }

            iColumn++;
            fLeft += fColumnWidth;
            Boolean bIncrementTop = false;

            if (iColumn == iColumns)
            {
                iColumn = 0;
                fLeft = oControlRectangle.Left;
                bIncrementTop = true;
            }
            else if (i == iVertexCategories - 1)
            {
                bIncrementTop = true;
            }

            if (bIncrementTop)
            {
                iTop += (Int32)(1.6F * oDrawingObjects.FontHeight);
            }
        }

        oPen.Dispose();
        oBrush.Dispose();
        oGraphics.SmoothingMode = eOldSmoothingMode;

        return (iTop);
    }

    //*************************************************************************
    //  Method: DrawEdgeWeightSchemeResults()
    //
    /// <summary>
    /// Draws the results of the autofill with edge weight scheme.
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
    DrawEdgeWeightSchemeResults
    (
        DrawingObjects oDrawingObjects
    )
    {
        Debug.Assert(oDrawingObjects != null);
        AssertValid();

        Rectangle oControlRectangle = oDrawingObjects.ControlRectangle;

        Rectangle oControlRectangleWithMargin = AddMarginToRectangle(
            oDrawingObjects, oControlRectangle);

        Int32 iTop = oControlRectangle.Top;

        DrawColumnHeader(oDrawingObjects, "Weighted Graph Scheme",
            oControlRectangle.Left, oControlRectangle.Right, ref iTop);

        String sEdgeWeightColumnName;
        Double dSourceCalculationNumber1, dSourceCalculationNumber2;

        m_oAutoFillWorkbookWithSchemeResults.GetEdgeWeightResults(
            out sEdgeWeightColumnName, out dSourceCalculationNumber1,
            out dSourceCalculationNumber2);

        DrawSteppedBarResults(oDrawingObjects, sEdgeWeightColumnName,
            "Edge Width", dSourceCalculationNumber1,
            dSourceCalculationNumber2,
            WorkbookSchemeAutoFiller.MinimumEdgeWeightWidthWorkbook,
            WorkbookSchemeAutoFiller.MaximumEdgeWeightWidthWorkbook,
            EdgeWidthConverter.MaximumWidthWorkbook,
            oControlRectangle, oControlRectangleWithMargin.Left,
            oControlRectangleWithMargin.Right, ref iTop);

        return (iTop);
    }

    //*************************************************************************
    //  Method: DrawEdgeTimestampSchemeResults()
    //
    /// <summary>
    /// Draws the results of the autofill with edge timestamp scheme.
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
    DrawEdgeTimestampSchemeResults
    (
        DrawingObjects oDrawingObjects
    )
    {
        Debug.Assert(oDrawingObjects != null);
        AssertValid();

        Rectangle oControlRectangle = oDrawingObjects.ControlRectangle;

        Rectangle oControlRectangleWithMargin = AddMarginToRectangle(
            oDrawingObjects, oControlRectangle);

        Int32 iTop = oControlRectangle.Top;

        DrawColumnHeader(oDrawingObjects, "Temporal Graph Scheme",
            oControlRectangle.Left, oControlRectangle.Right, ref iTop);

        String sEdgeTimestampColumnName;
        ExcelColumnFormat eEdgeTimestampColumnFormat;
        Double dSourceCalculationNumber1, dSourceCalculationNumber2;

        m_oAutoFillWorkbookWithSchemeResults.GetEdgeTimestampResults(
            out sEdgeTimestampColumnName, out eEdgeTimestampColumnFormat,
            out dSourceCalculationNumber1, out dSourceCalculationNumber2);

        String sSourceCalculationMinimum =
            SourceCalculationNumberToTimestampString(dSourceCalculationNumber1,
                eEdgeTimestampColumnFormat);

        String sSourceCalculationMaximum =
            SourceCalculationNumberToTimestampString(dSourceCalculationNumber2,
                eEdgeTimestampColumnFormat);

        DrawColorBarResults(oDrawingObjects, sEdgeTimestampColumnName,
            "Edge Color", sSourceCalculationMinimum, sSourceCalculationMaximum,
            WorkbookSchemeAutoFiller.MinimumEdgeTimestampColor,

            // If all the source numbers were the same, draw one color only.

            (dSourceCalculationNumber2 == dSourceCalculationNumber1) ? 
                WorkbookSchemeAutoFiller.MinimumEdgeTimestampColor:
                WorkbookSchemeAutoFiller.MaximumEdgeTimestampColor,

            oControlRectangle, oControlRectangleWithMargin.Left,
            oControlRectangleWithMargin.Right, ref iTop);

        return (iTop);
    }

    //*************************************************************************
    //  Method: SourceCalculationNumberToTimestampString()
    //
    /// <summary>
    /// Converts a source calculation number to a string for the edge timestamp
    /// scheme.
    /// </summary>
    ///
    /// <param name="dSourceCalculationNumber">
    /// The number to convert.
    /// </param>
    ///
    /// <param name="eEdgeTimestampColumnFormat">
    /// The source column format.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="dSourceCalculationNumber" /> converted to a string.
    /// </returns>
    //*************************************************************************

    protected String
    SourceCalculationNumberToTimestampString
    (
        Double dSourceCalculationNumber,
        ExcelColumnFormat eEdgeTimestampColumnFormat
    )
    {
        AssertValid();

        switch (eEdgeTimestampColumnFormat)
        {
            case ExcelColumnFormat.Date:
            case ExcelColumnFormat.DateAndTime:

                // The calculation numbers are in ticks.

                return ( ExcelDateTimeUtil.DateTimeToString(
                    (Int64)dSourceCalculationNumber,
                    eEdgeTimestampColumnFormat) );

            case ExcelColumnFormat.Time:

                // The calculation numbers are in fractions of a day.

                return ( ExcelDateTimeUtil.DateTimeToString(
                    (Int64)(dSourceCalculationNumber * TimeSpan.TicksPerDay),
                    ExcelColumnFormat.Time) );

            default:

                // The calculation numbers are actual numbers.

                return ( dSourceCalculationNumber.ToString() );
        }
    }

    //*************************************************************************
    //  Method: DrawColorBarResults()
    //
    /// <remarks>
    /// Draws the results for one autofilled column using a color gradient bar.
    /// </remarks>
    ///
    /// <summary>
    /// Draws the results for one autofilled column using a color gradient bar
    /// and source calculation numbers.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="sSourceColumnName">
    /// The name of the source column.
    /// </param>
    ///
    /// <param name="sCaption">
    /// Caption to draw above the color gradient bar.
    /// </param>
    ///
    /// <param name="dSourceCalculationNumber1">
    /// The actual first source number used in the calculations.
    /// </param>
    ///
    /// <param name="dSourceCalculationNumber2">
    /// The actual second source number used in the calculations.
    /// </param>
    ///
    /// <param name="oColor1">
    /// The color to use at the left edge of the color gradient bar.
    /// </param>
    ///
    /// <param name="oColor2">
    /// The color to use at the left edge of the color gradient bar.
    /// </param>
    ///
    /// <param name="oColumnRectangle">
    /// Rectangle to draw the results within.
    /// </param>
    ///
    /// <param name="iResultsLeft">
    /// Left x-coordinate where the results should be drawn.
    /// </param>
    ///
    /// <param name="iResultsRight">
    /// Right x-coordinate where the results should be drawn.
    /// </param>
    ///
    /// <param name="iTop">
    /// Top y-coordinate where the results should be drawn.  Gets updated.
    /// </param>
    //*************************************************************************

    protected void
    DrawColorBarResults
    (
        DrawingObjects oDrawingObjects,
        String sSourceColumnName,
        String sCaption,
        Double dSourceCalculationNumber1,
        Double dSourceCalculationNumber2,
        Color oColor1,
        Color oColor2,
        Rectangle oColumnRectangle,
        Int32 iResultsLeft,
        Int32 iResultsRight,
        ref Int32 iTop
    )
    {
        Debug.Assert(oDrawingObjects != null);
        Debug.Assert( !String.IsNullOrEmpty(sSourceColumnName) );
        Debug.Assert( !String.IsNullOrEmpty(sCaption) );
        AssertValid();

        if (dSourceCalculationNumber2 == dSourceCalculationNumber1)
        {
            // All the source numbers were the same.  Draw one color only.

            oColor2 = oColor1;
        }

        DrawColorBarResults(oDrawingObjects, sSourceColumnName, sCaption,
            dSourceCalculationNumber1.ToString(),
            dSourceCalculationNumber2.ToString(), oColor1, oColor2,
            oColumnRectangle, iResultsLeft, iResultsRight, ref iTop);
    }

    //*************************************************************************
    //  Method: DrawColorBarResults()
    //
    /// <remarks>
    /// Draws the results for one autofilled column using a color gradient bar
    /// and source calculation strings.
    /// </remarks>
    ///
    /// <summary>
    /// Draws the results for one autofilled column using a color gradient bar
    /// and source calculation strings.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="sSourceColumnName">
    /// The name of the source column.
    /// </param>
    ///
    /// <param name="sCaption">
    /// Caption to draw above the color gradient bar.
    /// </param>
    ///
    /// <param name="sSourceCalculationMinimum">
    /// The actual minimum value used in the calculations.
    /// </param>
    ///
    /// <param name="sSourceCalculationMaximum">
    /// The actual maximum value used in the calculations.
    /// </param>
    ///
    /// <param name="oColor1">
    /// The color to use at the left edge of the color gradient bar.
    /// </param>
    ///
    /// <param name="oColor2">
    /// The color to use at the left edge of the color gradient bar.
    /// </param>
    ///
    /// <param name="oColumnRectangle">
    /// Rectangle to draw the results within.
    /// </param>
    ///
    /// <param name="iResultsLeft">
    /// Left x-coordinate where the results should be drawn.
    /// </param>
    ///
    /// <param name="iResultsRight">
    /// Right x-coordinate where the results should be drawn.
    /// </param>
    ///
    /// <param name="iTop">
    /// Top y-coordinate where the results should be drawn.  Gets updated.
    /// </param>
    //*************************************************************************

    protected void
    DrawColorBarResults
    (
        DrawingObjects oDrawingObjects,
        String sSourceColumnName,
        String sCaption,
        String sSourceCalculationMinimum,
        String sSourceCalculationMaximum,
        Color oColor1,
        Color oColor2,
        Rectangle oColumnRectangle,
        Int32 iResultsLeft,
        Int32 iResultsRight,
        ref Int32 iTop
    )
    {
        Debug.Assert(oDrawingObjects != null);
        Debug.Assert( !String.IsNullOrEmpty(sSourceColumnName) );
        Debug.Assert( !String.IsNullOrEmpty(sCaption) );
        Debug.Assert( !String.IsNullOrEmpty(sSourceCalculationMinimum) );
        Debug.Assert( !String.IsNullOrEmpty(sSourceCalculationMaximum) );
        AssertValid();

        DrawExcelColumnName(oDrawingObjects, sSourceColumnName, iResultsLeft,
            iResultsRight, ref iTop);

        DrawCenteredCaption(oDrawingObjects, sCaption, iResultsLeft,
            iResultsRight, iTop);

        DrawRangeText(oDrawingObjects, sSourceCalculationMinimum,
            sSourceCalculationMaximum, SystemBrushes.WindowText, iResultsLeft,
            iResultsRight, ref iTop);

        Rectangle oColorBarRectangle = new Rectangle(iResultsLeft, iTop,
            iResultsRight - iResultsLeft, ColorBarHeight);

        // There is a known bug in the LinearGradientBrush that sometimes
        // causes a single line of the wrong color to be drawn at the start of
        // the gradient.  The workaround is to use a brush rectangle that is
        // one pixel larger than the fill rectangle on all sides.

        Rectangle oBrushRectangle = oColorBarRectangle;
        oBrushRectangle.Inflate(1, 1);

        LinearGradientBrush oBrush = new LinearGradientBrush(
            oBrushRectangle, oColor1, oColor2, 0F);

        oDrawingObjects.Graphics.FillRectangle(oBrush, oColorBarRectangle);

        oBrush.Dispose();

        iTop += (Int32)(ColorBarHeight + oDrawingObjects.FontHeight * 0.4F);

        // Draw a line separating these results from the next.

        DrawHorizontalSeparator(oDrawingObjects, oColumnRectangle, ref iTop);
    }

    //*************************************************************************
    //  Method: DrawSteppedBarResults()
    //
    /// <summary>
    /// Draws the results for one autofilled column using a stepped bar that
    /// increases in height from left to right.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="sSourceColumnName">
    /// The name of the source column.
    /// </param>
    ///
    /// <param name="sCaption">
    /// Caption to draw above the color gradient bar.
    /// </param>
    ///
    /// <param name="dSourceCalculationNumber1">
    /// The actual first source number used in the calculations.
    /// </param>
    ///
    /// <param name="dSourceCalculationNumber2">
    /// The actual second source number used in the calculations.
    /// </param>
    ///
    /// <param name="dDestinationNumber1">
    /// The first number used in the destination column.
    /// </param>
    ///
    /// <param name="dDestinationNumber2">
    /// The second number used in the destination column.
    /// </param>
    ///
    /// <param name="dMaximumDestinationNumber">
    /// The maximum possible destination number.
    /// </param>
    ///
    /// <param name="oColumnRectangle">
    /// Rectangle to draw the results within.
    /// </param>
    ///
    /// <param name="iResultsLeft">
    /// Left x-coordinate where the results should be drawn.
    /// </param>
    ///
    /// <param name="iResultsRight">
    /// Right x-coordinate where the results should be drawn.
    /// </param>
    ///
    /// <param name="iTop">
    /// Top y-coordinate where the results should be drawn.  Gets updated.
    /// </param>
    //*************************************************************************

    protected void
    DrawSteppedBarResults
    (
        DrawingObjects oDrawingObjects,
        String sSourceColumnName,
        String sCaption,
        Double dSourceCalculationNumber1,
        Double dSourceCalculationNumber2,
        Double dDestinationNumber1,
        Double dDestinationNumber2,
        Double dMaximumDestinationNumber,
        Rectangle oColumnRectangle,
        Int32 iResultsLeft,
        Int32 iResultsRight,
        ref Int32 iTop
    )
    {
        Debug.Assert(oDrawingObjects != null);
        Debug.Assert( !String.IsNullOrEmpty(sSourceColumnName) );
        Debug.Assert( !String.IsNullOrEmpty(sCaption) );
        AssertValid();

        DrawExcelColumnName(oDrawingObjects, sSourceColumnName, iResultsLeft,
            iResultsRight, ref iTop);

        DrawCenteredCaption(oDrawingObjects, sCaption, iResultsLeft,
            iResultsRight, iTop);

        DrawRangeText(oDrawingObjects, dSourceCalculationNumber1.ToString(),
            dSourceCalculationNumber2.ToString(), SystemBrushes.WindowText,
            iResultsLeft, iResultsRight, ref iTop);

        Single fStepWidth = (Single)( (iResultsRight - iResultsLeft) /
            (Single)SteppedBarSegments );

        Single fStepHeight1 = (Single)(SteppedBarHeight *
            dDestinationNumber1 / dMaximumDestinationNumber);

        Single fStepHeight2 = (Single)(SteppedBarHeight *
            dDestinationNumber2 / dMaximumDestinationNumber);

        Single fStepHeightIncrement = (fStepHeight2 - fStepHeight1) /
            ( (Single)SteppedBarSegments - 1F );

        if (dSourceCalculationNumber2 == dSourceCalculationNumber1)
        {
            // All the source numbers were the same.  Don't draw steps.

            fStepHeightIncrement = 0;
        }

        // The bottom of each segment is fixed.

        Single fStepLeft = iResultsLeft;
        Single fStepRight = fStepLeft + fStepWidth;
        Single fStepBottom = iTop + SteppedBarHeight;
        Single fStepTop = fStepBottom - fStepHeight1;

        for (Int32 i = 0; i < SteppedBarSegments; i++)
        {
            // The Math.Min() call is to prevent a height less than 1, which
            // sometimes gets drawn and sometimes doesn't.

            RectangleF oSteppedBarRectangle = RectangleF.FromLTRB(
                fStepLeft, Math.Min(fStepTop, fStepBottom - 1F), fStepRight,
                fStepBottom);

            oDrawingObjects.Graphics.FillRectangle(SystemBrushes.ControlDark,
                oSteppedBarRectangle);

            fStepLeft += fStepWidth;
            fStepTop -= fStepHeightIncrement;
            fStepRight += fStepWidth;
        }

        iTop += (Int32)(SteppedBarHeight + oDrawingObjects.FontHeight * 0.4F);

        // Draw a line separating these results from the next.

        DrawHorizontalSeparator(oDrawingObjects, oColumnRectangle, ref iTop);
    }

    //*************************************************************************
    //  Method: DrawCenteredCaption()
    //
    /// <summary>
    /// Draws a centered caption for the results.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="sCaption">
    /// Caption to draw.
    /// </param>
    ///
    /// <param name="iLeft">
    /// Left x-coordinate where the caption should be drawn.
    /// </param>
    ///
    /// <param name="iRight">
    /// Right x-coordinate where the caption should be drawn.
    /// </param>
    ///
    /// <param name="iTop">
    /// Top y-coordinate where the caption should be drawn.
    /// </param>
    //*************************************************************************

    protected void
    DrawCenteredCaption
    (
        DrawingObjects oDrawingObjects,
        String sCaption,
        Int32 iLeft,
        Int32 iRight,
        Int32 iTop
    )
    {
        Debug.Assert(oDrawingObjects != null);
        Debug.Assert( !String.IsNullOrEmpty(sCaption) );
        AssertValid();

        oDrawingObjects.Graphics.DrawString(sCaption, oDrawingObjects.Font,
            SystemBrushes.WindowText,
            iLeft + (iRight - iLeft) / 2F, iTop,
            oDrawingObjects.CenterAlignStringFormat);
    }

    //*************************************************************************
    //  Method: DrawVertexCategoryShape()
    //
    /// <summary>
    /// Draws the shape for a vertex category.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="eVertexShape">
    /// The shape to draw.
    /// </param>
    ///
    /// <param name="oPen">
    /// The pen to draw with.
    /// </param>
    ///
    /// <param name="oBrush">
    /// The brush to draw with.
    /// </param>
    ///
    /// <param name="oColor">
    /// The color to use.
    /// </param>
    ///
    /// <param name="fXShapeCenter">
    /// x-coordinate of the shape's center.
    /// </param>
    ///
    /// <param name="fYShapeCenter">
    /// y-coordinate of the shape's center.
    /// </param>
    ///
    /// <param name="fShapeHalfWidth">
    /// One half the width of the shape.
    /// </param>
    //*************************************************************************

    protected void
    DrawVertexCategoryShape
    (
        DrawingObjects oDrawingObjects,
        VertexShape eVertexShape,
        Pen oPen,
        SolidBrush oBrush,
        Color oColor,
        Single fXShapeCenter,
        Single fYShapeCenter,
        Single fShapeHalfWidth
    )
    {
        Debug.Assert(oDrawingObjects != null);
        Debug.Assert(oPen != null);
        Debug.Assert(oBrush != null);
        Debug.Assert(fShapeHalfWidth >= 0);
        AssertValid();

        Graphics oGraphics = oDrawingObjects.Graphics;
        oPen.Color = oColor;
        oBrush.Color = oColor;

        switch (eVertexShape)
        {
            case VertexShape.Circle:

                GraphicsUtil.DrawCircle(oGraphics, oPen, fXShapeCenter,
                    fYShapeCenter, fShapeHalfWidth);

                break;

            case VertexShape.Disk:

                GraphicsUtil.FillCircle(oGraphics, oBrush, fXShapeCenter,
                    fYShapeCenter, fShapeHalfWidth);

                break;

            case VertexShape.Sphere:

                GraphicsUtil.FillCircle3D(oGraphics, oColor,
                    fXShapeCenter, fYShapeCenter, fShapeHalfWidth);

                break;

            case VertexShape.Square:

                GraphicsUtil.DrawSquare(oGraphics, oPen, fXShapeCenter,
                    fYShapeCenter, fShapeHalfWidth);

                break;

            case VertexShape.SolidSquare:

                GraphicsUtil.FillSquare(oGraphics, oBrush, fXShapeCenter,
                    fYShapeCenter, fShapeHalfWidth);

                break;

            case VertexShape.Diamond:

                GraphicsUtil.DrawDiamond(oGraphics, oPen, fXShapeCenter,
                    fYShapeCenter, fShapeHalfWidth);

                break;

            case VertexShape.SolidDiamond:

                GraphicsUtil.FillDiamond(oGraphics, oBrush, fXShapeCenter,
                    fYShapeCenter, fShapeHalfWidth);

                break;

            case VertexShape.Triangle:

                GraphicsUtil.DrawTriangle(oGraphics, oPen, fXShapeCenter,
                    fYShapeCenter, fShapeHalfWidth);

                break;

            case VertexShape.SolidTriangle:

                GraphicsUtil.FillTriangle(oGraphics, oBrush, fXShapeCenter,
                    fYShapeCenter, fShapeHalfWidth);

                break;

            default:

                Debug.Assert(false);
                break;
        }
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

        Debug.Assert(m_oAutoFillWorkbookResults != null);
        Debug.Assert(m_oAutoFillWorkbookWithSchemeResults != null);
        // m_iLastResizeWidth
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Height of the color bar drawn by DrawColorBarResults().

    protected const Int32 ColorBarHeight = 8;

    /// Height of the stepped bar drawn by DrawSteppedBarResults().

    protected const Int32 SteppedBarHeight = 8;

    /// Number of horizontal segments drawn by DrawSteppedBarResults().

    protected const Int32 SteppedBarSegments = 4;

    /// Caption for edge and vertex color results.

    protected const String ColorCaption = "Color";

    /// Caption for edge width results.

    protected const String EdgeWidthCaption = "Width";

    /// Width of the shapes used to draw categories, as a multiple of the font
    /// height.

    protected const Single CategoryShapeFactor = 1.2F;

    /// Maximum vertex categories to show.

    protected const Int32 MaximumVertexCategories = 96;

    /// Maximum number of characters to use for a category name.

    protected const Int32 MaximumVertexCategoryNameLength = 10;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The results of the autofill.

    protected AutoFillWorkbookResults m_oAutoFillWorkbookResults;

    /// The results of the "autofill with scheme."

    protected AutoFillWorkbookWithSchemeResults
        m_oAutoFillWorkbookWithSchemeResults;

    /// Width of the control during the last Resize event.

    protected Int32 m_iLastResizeWidth;
}
}
