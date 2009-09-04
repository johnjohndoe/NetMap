
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.NodeXL.Visualization.Wpf;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: WorkbookSchemeAutoFiller
//
/// <summary>
/// Automatically fills one or more edge and vertex attribute columns with a
/// scheme of attribute values.
/// </summary>
///
/// <remarks>
/// Call <see cref="AutoFillByVertexCategory(Workbook, String, Boolean,
/// String)" /> to assign vertex attributes based on a vertex category column,
/// <see cref="AutoFillByEdgeWeight" /> to assign edge widths based on an edge
/// weight column, or <see cref="AutoFillByEdgeTimestamp" /> to assign edge
/// colors based on an edge timestamp column.
///
/// <para>
/// All methods are static.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class WorkbookSchemeAutoFiller : Object
{
    //*************************************************************************
    //  Method: AutoFillByVertexCategory()
    //
    /// <summary>
    /// Assigns attributes based on a vertex category column.
    /// </summary>
    ///
    /// <param name="workbook">
    /// The workbook to autofill.
    /// </param>
    ///
    /// <param name="vertexCategoryColumnName">
    /// The name of the vertex table column containing vertex categories.
    /// </param>
    ///
    /// <param name="showVertexLabels">
    /// true if vertex labels should be shown.
    /// </param>
    ///
    /// <param name="vertexLabelColumnName">
    /// The name of the vertex table column containing vertex labels.  Used
    /// only if <paramref name="showVertexLabels" /> is true.
    /// </param>
    ///
    /// <remarks>
    /// This method reads a vertex category column that specifies which
    /// category each vertex belongs to.  It then assigns a set of shape,
    /// color, and radius attributes to each category (red circles with a
    /// radius of 2.0, for example) and fills in the vertex shape, color, and
    /// radius columns.  It also fills in other attribute columns with constant
    /// values, and fills in the vertex primary label column if <paramref
    /// name="showVertexLabels" /> is true.
    ///
    /// <para>
    /// In addition to autofilling columns, this method stores the results of
    /// the autofill as a <see cref="AutoFillWorkbookWithSchemeResults" />
    /// using <see cref="PerWorkbookSettings" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static void
    AutoFillByVertexCategory
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        String vertexCategoryColumnName,
        Boolean showVertexLabels,
        String vertexLabelColumnName
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert( !String.IsNullOrEmpty(vertexCategoryColumnName) );

        Debug.Assert( !showVertexLabels ||
            !String.IsNullOrEmpty(vertexLabelColumnName) );

        ListObject oVertexTable = null;
        ListObject oEdgeTable = null;
        ExcelHiddenColumns oHiddenVertexColumns = null;
        ExcelHiddenColumns oHiddenEdgeColumns = null;

        try
        {
            if (
                !TryStartAutoFill(workbook, showVertexLabels,
                    vertexLabelColumnName, out oEdgeTable, out oVertexTable,
                    out oHiddenEdgeColumns, out oHiddenVertexColumns)
                ||
                ExcelUtil.TableIsEmpty(oVertexTable)
                )
            {
                return;
            }

            AutoFillByVertexCategory(workbook, oEdgeTable, oVertexTable,
                vertexCategoryColumnName, showVertexLabels,
                vertexLabelColumnName);
        }
        finally
        {
            EndAutoFill(workbook, oEdgeTable, oVertexTable,
                oHiddenEdgeColumns, oHiddenVertexColumns);
        }
    }

    //*************************************************************************
    //  Method: AutoFillByEdgeWeight()
    //
    /// <summary>
    /// Assigns attributes based on an edge weight column.
    /// </summary>
    ///
    /// <param name="workbook">
    /// The workbook to autofill.
    /// </param>
    ///
    /// <param name="edgeWeightColumnName">
    /// The name of the edge table column containing edge weights.
    /// </param>
    ///
    /// <param name="showVertexLabels">
    /// true if vertex labels should be shown.
    /// </param>
    ///
    /// <param name="vertexLabelColumnName">
    /// The name of the vertex table column containing vertex labels.  Used
    /// only if <paramref name="showVertexLabels" /> is true.
    /// </param>
    ///
    /// <remarks>
    /// This method maps an edge weight column to the edge width column,
    /// fills in other attribute columns with constant values, and fills in the
    /// vertex primary label column if <paramref name="showVertexLabels" /> is
    /// true.
    ///
    /// <para>
    /// In addition to autofilling columns, this method stores the results of
    /// the autofill as a <see cref="AutoFillWorkbookWithSchemeResults" />
    /// using <see cref="PerWorkbookSettings" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static void
    AutoFillByEdgeWeight
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        String edgeWeightColumnName,
        Boolean showVertexLabels,
        String vertexLabelColumnName
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert( !String.IsNullOrEmpty(edgeWeightColumnName) );

        Debug.Assert( !showVertexLabels ||
            !String.IsNullOrEmpty(vertexLabelColumnName) );

        ListObject oVertexTable = null;
        ListObject oEdgeTable = null;
        ExcelHiddenColumns oHiddenVertexColumns = null;
        ExcelHiddenColumns oHiddenEdgeColumns = null;

        AutoFillWorkbookWithSchemeResults oAutoFillWorkbookWithSchemeResults =
            new AutoFillWorkbookWithSchemeResults();

        try
        {
            if (
                !TryStartAutoFill(workbook, showVertexLabels,
                    vertexLabelColumnName, out oEdgeTable, out oVertexTable,
                    out oHiddenEdgeColumns, out oHiddenVertexColumns)
                ||
                ExcelUtil.TableIsEmpty(oEdgeTable)
                )
            {
                return;
            }

            // Map the edge weight column to the edge width column.

            Double dSourceCalculationNumber1, dSourceCalculationNumber2;

            if ( !TableColumnMapper.TryMapToNumericRange(oEdgeTable,
                edgeWeightColumnName, EdgeTableColumnNames.Width, false, false,
                0, 0,
                MinimumEdgeWeightWidthWorkbook, MaximumEdgeWeightWidthWorkbook,
                false, false,
                out dSourceCalculationNumber1, out dSourceCalculationNumber2
                ) )
            {
                return;
            }

            // Fill in other columns with constants.

            String sBlack = ( new ColorConverter2() ).GraphToWorkbook(
                Color.FromArgb(0, 0, 0) );

            FillColumnsWithConstants(

                oEdgeTable, EdgeTableColumnNames.Color, sBlack,

                oEdgeTable, EdgeTableColumnNames.Alpha,
                    0.6F * (AlphaConverter.MaximumAlphaWorkbook -
                        AlphaConverter.MinimumAlphaWorkbook),

                oVertexTable, VertexTableColumnNames.Shape,
                    ( new VertexShapeConverter() ).GraphToWorkbook(
                        VertexShape.Circle),

                oVertexTable, VertexTableColumnNames.Color, sBlack,

                oVertexTable, VertexTableColumnNames.Radius, 3.0F,

                oVertexTable, VertexTableColumnNames.Alpha,
                    AlphaConverter.MaximumAlphaWorkbook
                );

            // Save the results.

            oAutoFillWorkbookWithSchemeResults.SetEdgeWeightResults(
                edgeWeightColumnName, dSourceCalculationNumber1,
                dSourceCalculationNumber2);
        }
        finally
        {
            ( new PerWorkbookSettings(workbook) ).
                AutoFillWorkbookWithSchemeResults =
                    oAutoFillWorkbookWithSchemeResults;

            EndAutoFill(workbook, oEdgeTable, oVertexTable,
                oHiddenEdgeColumns, oHiddenVertexColumns);
        }
    }

    //*************************************************************************
    //  Method: AutoFillByEdgeTimestamp()
    //
    /// <summary>
    /// Assigns attributes based on an edge timestamp column.
    /// </summary>
    ///
    /// <param name="workbook">
    /// The workbook to autofill.
    /// </param>
    ///
    /// <param name="edgeTimestampColumnName">
    /// The name of the edge table column containing edge timestamps.
    /// </param>
    ///
    /// <param name="showVertexLabels">
    /// true if vertex labels should be shown.
    /// </param>
    ///
    /// <param name="vertexLabelColumnName">
    /// The name of the vertex table column containing vertex labels.  Used
    /// only if <paramref name="showVertexLabels" /> is true.
    /// </param>
    ///
    /// <remarks>
    /// This method maps an edge timestamp column to the edge color column,
    /// fills in other attribute columns with constant values, and fills in the
    /// vertex primary label column if <paramref name="showVertexLabels" /> is
    /// true.
    ///
    /// <para>
    /// In addition to autofilling columns, this method stores the results of
    /// the autofill as a <see cref="AutoFillWorkbookWithSchemeResults" />
    /// using <see cref="PerWorkbookSettings" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static void
    AutoFillByEdgeTimestamp
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        String edgeTimestampColumnName,
        Boolean showVertexLabels,
        String vertexLabelColumnName
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert( !String.IsNullOrEmpty(edgeTimestampColumnName) );

        Debug.Assert( !showVertexLabels ||
            !String.IsNullOrEmpty(vertexLabelColumnName) );

        ListObject oVertexTable = null;
        ListObject oEdgeTable = null;
        ExcelHiddenColumns oHiddenVertexColumns = null;
        ExcelHiddenColumns oHiddenEdgeColumns = null;

        AutoFillWorkbookWithSchemeResults oAutoFillWorkbookWithSchemeResults =
            new AutoFillWorkbookWithSchemeResults();

        try
        {
            if (
                !TryStartAutoFill(workbook, showVertexLabels,
                    vertexLabelColumnName, out oEdgeTable, out oVertexTable,
                    out oHiddenEdgeColumns, out oHiddenVertexColumns)
                ||
                ExcelUtil.TableIsEmpty(oEdgeTable)
                )
            {
                return;
            }

            // Map the edge timestamp column to the edge color column.

            Double dSourceCalculationNumber1, dSourceCalculationNumber2;

            if ( !TableColumnMapper.TryMapToColor(oEdgeTable,
                edgeTimestampColumnName, EdgeTableColumnNames.Color, false,
                false, 0, 0, MinimumEdgeTimestampColor,
                MaximumEdgeTimestampColor, false, false,
                out dSourceCalculationNumber1, out dSourceCalculationNumber2
                ) )
            {
                return;
            }

            // Fill in other columns with constants.

            FillColumnsWithConstants(

                oEdgeTable, EdgeTableColumnNames.Width, 2.0F,

                oEdgeTable, EdgeTableColumnNames.Alpha,
                    AlphaConverter.MaximumAlphaWorkbook,

                oVertexTable, VertexTableColumnNames.Shape,
                    ( new VertexShapeConverter() ).GraphToWorkbook(
                        VertexShape.Circle),

                oVertexTable, VertexTableColumnNames.Color,
                    ( new ColorConverter2() ).GraphToWorkbook(
                        Color.FromArgb(0, 0, 0) ),

                oVertexTable, VertexTableColumnNames.Radius, 1.5F,

                oVertexTable, VertexTableColumnNames.Alpha,
                    AlphaConverter.MaximumAlphaWorkbook
                );

            // Save the results.

            ListColumn oEdgeTimestampColumn;

            ExcelUtil.TryGetTableColumn(oEdgeTable, edgeTimestampColumnName,
                out oEdgeTimestampColumn);

            oAutoFillWorkbookWithSchemeResults.SetEdgeTimestampResults(
                edgeTimestampColumnName,
                ExcelUtil.GetColumnFormat(oEdgeTimestampColumn),
                dSourceCalculationNumber1, dSourceCalculationNumber2);
        }
        finally
        {
            ( new PerWorkbookSettings(workbook) ).
                AutoFillWorkbookWithSchemeResults =
                    oAutoFillWorkbookWithSchemeResults;

            EndAutoFill(workbook, oEdgeTable, oVertexTable,
                oHiddenEdgeColumns, oHiddenVertexColumns);
        }
    }

    //*************************************************************************
    //  Method: GetVertexCategoryScheme()
    //
    /// <summary>
    /// Gets the attributes to use for a vertex category scheme.
    /// </summary>
    ///
    /// <param name="schemeIndex">
    /// Scheme index.
    /// </param>
    ///
    /// <param name="shape">
    /// Where the vertex shape gets stored.
    /// </param>
    ///
    /// <param name="color">
    /// Where the vertex color gets stored.
    /// </param>
    ///
    /// <param name="radius">
    /// Where the vertex radius gets stored.
    /// </param>
    //*************************************************************************

    public static void
    GetVertexCategoryScheme
    (
        Int32 schemeIndex,
        out VertexShape shape,
        out Color color,
        out Single radius
    )
    {
        Debug.Assert(schemeIndex >= 0);

        // 96 categories are supported -- 12 colors and 8 shapes.  Each shape
        // has its own assigned radius.

        Color [] aoColors = new Color [] {
            Color.FromArgb(0, 11, 96),
            Color.FromArgb(0, 134, 227),
            Color.FromArgb(0, 100, 50),
            Color.FromArgb(0, 176, 21),
            Color.FromArgb(192, 0, 0),
            Color.FromArgb(230, 118, 0),
            Color.FromArgb(255, 192, 0),
            Color.FromArgb(150, 200, 0),
            Color.FromArgb(200, 0, 120),
            Color.FromArgb(78, 0, 96),
            Color.FromArgb(91, 0, 192),
            Color.FromArgb(0, 97, 129),
            };

        VertexShape [] aeShapes = new VertexShape [] {
            VertexShape.Disk,
            VertexShape.Circle,
            VertexShape.SolidSquare,
            VertexShape.Square,
            VertexShape.SolidTriangle,
            VertexShape.Triangle,
            VertexShape.SolidDiamond,
            VertexShape.Diamond,
            };

        Single [] afRadii = new Single [] {
            3.0F,
            3.0F,
            2.7F,
            2.7F,
            3.2F,
            3.2F,
            3.3F,
            3.3F,
            };

        Int32 iColors = aoColors.Length;
        Int32 iShapes = aeShapes.Length;
        Debug.Assert(afRadii.Length == iShapes);

        Int32 iColorIndex = schemeIndex % iColors;
        Int32 iShapeAndRadiusIndex = (schemeIndex / iColors) % iShapes;

        color = aoColors[iColorIndex];
        shape = aeShapes[iShapeAndRadiusIndex];
        radius = afRadii[iShapeAndRadiusIndex];
    }

    //*************************************************************************
    //  Method: AutoFillByVertexCategory()
    //
    /// <summary>
    /// Assigns attributes based on a vertex category column.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// The workbook to autofill.
    /// </param>
    ///
    /// <param name="oEdgeTable">
    /// The edge table.
    /// </param>
    ///
    /// <param name="oVertexTable">
    /// The vertex table.
    /// </param>
    ///
    /// <param name="sVertexCategoryColumnName">
    /// The name of the vertex table column containing vertex categories.
    /// </param>
    ///
    /// <param name="bShowVertexLabels">
    /// true if vertex labels should be shown.
    /// </param>
    ///
    /// <param name="sVertexLabelColumnName">
    /// The name of the vertex table column containing vertex labels.  Used
    /// only if <paramref name="bShowVertexLabels" /> is true.
    /// </param>
    //*************************************************************************

    private static void
    AutoFillByVertexCategory
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        Microsoft.Office.Interop.Excel.ListObject oEdgeTable,
        Microsoft.Office.Interop.Excel.ListObject oVertexTable,
        String sVertexCategoryColumnName,
        Boolean bShowVertexLabels,
        String sVertexLabelColumnName
    )
    {
        Debug.Assert(oWorkbook != null);
        Debug.Assert(oEdgeTable != null);
        Debug.Assert(oVertexTable != null);
        Debug.Assert( !String.IsNullOrEmpty(sVertexCategoryColumnName) );

        Debug.Assert( !bShowVertexLabels ||
            !String.IsNullOrEmpty(sVertexLabelColumnName) );

        Range oVisibleCategoryRange = null;
        Range oVisibleShapeRange = null;
        Range oVisibleColorRange = null;
        Range oVisibleRadiusRange = null;;

        if (
            !ExcelUtil.TryGetVisibleTableColumnData(oVertexTable,
                sVertexCategoryColumnName, out oVisibleCategoryRange)
            ||
            !ExcelUtil.TryGetVisibleTableColumnData(oVertexTable,
                VertexTableColumnNames.Shape, out oVisibleShapeRange)
            ||
            !ExcelUtil.TryGetVisibleTableColumnData(oVertexTable,
                VertexTableColumnNames.Color, out oVisibleColorRange)
            ||
            !ExcelUtil.TryGetVisibleTableColumnData(oVertexTable,
                VertexTableColumnNames.Radius, out oVisibleRadiusRange)
            )
        {
            ErrorUtil.OnMissingColumn();
        }

        // Loop through the visible areas to create a dictionary of categories.
        // The key is a unique category name and the value is a scheme index.
        // If the category column contains three unique categories, for
        // example, the dictionary entries will look like this:
        //
        // Category A, 0
        // Category B, 1
        // Category C, 2

        Dictionary<String, Int32> oCategoryDictionary =
            new Dictionary<String, Int32>();

        Areas oCategoryAreas = oVisibleCategoryRange.Areas;
        Int32 iAreas = oCategoryAreas.Count;
        Int32 iSchemeIndex = 0;

        for (Int32 iArea = 1; iArea <= iAreas; iArea++)
        {
            Object [,] aoCategoryValues = ExcelUtil.GetRangeValues(
                oCategoryAreas[iArea] );

            Int32 iRows = aoCategoryValues.GetUpperBound(0);
            String sCategory;

            for (Int32 iRow = 1; iRow <= iRows; iRow++)
            {
                if ( ExcelUtil.TryGetNonEmptyStringFromCell(aoCategoryValues,
                    iRow, 1, out sCategory) )
                {
                    if ( !oCategoryDictionary.ContainsKey(sCategory) )
                    {
                        oCategoryDictionary[sCategory] = iSchemeIndex;
                        iSchemeIndex++;
                    }
                }
            }
        }

        // Scheme index that will be used for vertices that have an empty
        // category cell.

        Int32 SchemeIndexForNoCategory = iSchemeIndex;

        Areas oShapeAreas = oVisibleShapeRange.Areas;
        Areas oColorAreas = oVisibleColorRange.Areas;
        Areas oRadiusAreas = oVisibleRadiusRange.Areas;

        Debug.Assert(oShapeAreas.Count == iAreas);
        Debug.Assert(oColorAreas.Count == iAreas);
        Debug.Assert(oRadiusAreas.Count == iAreas);

        ColorConverter2 oColorConverter2 = new ColorConverter2();

        VertexShapeConverter oVertexShapeConverter =
            new VertexShapeConverter();

        // Loop through the visible areas again, this time to fill in the
        // vertex shape, color, and radius columns.

        for (Int32 iArea = 1; iArea <= iAreas; iArea++)
        {
            Object [,] aoCategoryValues = ExcelUtil.GetRangeValues(
                oCategoryAreas[iArea] );

            Range oShapeArea = oShapeAreas[iArea];
            Range oColorArea = oColorAreas[iArea];
            Range oRadiusArea = oRadiusAreas[iArea];

            Object [,] aoShapeValues = ExcelUtil.GetRangeValues(oShapeArea);
            Object [,] aoColorValues = ExcelUtil.GetRangeValues(oColorArea);
            Object [,] aoRadiusValues = ExcelUtil.GetRangeValues(oRadiusArea);

            Int32 iRows = aoCategoryValues.GetUpperBound(0);

            Debug.Assert(aoShapeValues.GetUpperBound(0) == iRows);
            Debug.Assert(aoColorValues.GetUpperBound(0) == iRows);
            Debug.Assert(aoRadiusValues.GetUpperBound(0) == iRows);

            String sCategory;

            for (Int32 iRow = 1; iRow <= iRows; iRow++)
            {
                if ( ExcelUtil.TryGetNonEmptyStringFromCell(aoCategoryValues,
                    iRow, 1, out sCategory) )
                {
                    iSchemeIndex = oCategoryDictionary[sCategory];
                }
                else
                {
                    iSchemeIndex = SchemeIndexForNoCategory;
                }

                VertexShape eShape;
                Color oColor;
                Single fRadius;

                GetVertexCategoryScheme(iSchemeIndex, out eShape, out oColor,
                    out fRadius);

                aoShapeValues[iRow, 1] =
                    oVertexShapeConverter.GraphToWorkbook(eShape);

                // Write the color in a format that is understood by
                // ColorConverter.ConvertFromString(), which is what
                // WorksheetReaderBase uses.

                aoColorValues[iRow, 1] =
                    oColorConverter2.GraphToWorkbook(oColor);

                aoRadiusValues[iRow, 1] = fRadius;
            }

            oShapeArea.set_Value(Missing.Value, aoShapeValues);
            oColorArea.set_Value(Missing.Value, aoColorValues);
            oRadiusArea.set_Value(Missing.Value, aoRadiusValues);
        }

        // Fill in other columns with constants.

        FillColumnsWithConstants(

            oVertexTable, VertexTableColumnNames.Alpha,
                AlphaConverter.MaximumAlphaWorkbook,

            oEdgeTable, EdgeTableColumnNames.Color,
                oColorConverter2.GraphToWorkbook(
                    Color.FromArgb(179, 179, 179) ),

            oEdgeTable, EdgeTableColumnNames.Width, 1.0F,

            oEdgeTable, EdgeTableColumnNames.Alpha,
                AlphaConverter.MaximumAlphaWorkbook
            );

        // Save the results.

        SaveVertexCategoryResults(oWorkbook, sVertexCategoryColumnName,
            oCategoryDictionary);
    }

    //*************************************************************************
    //  Method: SaveVertexCategoryResults()
    //
    /// <summary>
    /// Stores the results of a vertex category autofill as an <see
    /// cref="AutoFillWorkbookWithSchemeResults" /> using <see
    /// cref="PerWorkbookSettings" />.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// The workbook to autofill.
    /// </param>
    ///
    /// <param name="sVertexCategoryColumnName">
    /// The name of the vertex table column containing vertex categories.
    /// </param>
    ///
    /// <param name="oCategoryDictionary">
    /// The key is a unique category name and the value is a scheme index.
    /// </param>
    //*************************************************************************

    private static void
    SaveVertexCategoryResults
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        String sVertexCategoryColumnName,
        Dictionary<String, Int32> oCategoryDictionary
    )
    {
        Debug.Assert(oWorkbook != null);
        Debug.Assert( !String.IsNullOrEmpty(sVertexCategoryColumnName) );

        // Convert the dictionary keys to an array ordered by scheme index.

        Int32 iCategories = oCategoryDictionary.Count;

        String [] asVertexCategoryNames = new String[iCategories];

        foreach (KeyValuePair<String, Int32> oKeyValuePair in
            oCategoryDictionary)
        {
            Debug.Assert(asVertexCategoryNames[oKeyValuePair.Value] == null);

            asVertexCategoryNames[oKeyValuePair.Value] = oKeyValuePair.Key;
        }

        AutoFillWorkbookWithSchemeResults oAutoFillWorkbookWithSchemeResults =
            new AutoFillWorkbookWithSchemeResults();

        oAutoFillWorkbookWithSchemeResults.SetVertexCategoryResults(
            sVertexCategoryColumnName, asVertexCategoryNames);

        ( new PerWorkbookSettings(oWorkbook) ).
            AutoFillWorkbookWithSchemeResults =
                oAutoFillWorkbookWithSchemeResults;
    }

    //*************************************************************************
    //  Method: SetScreenUpdating()
    //
    /// <summary>
    /// Turns screen updating on or off.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// The workbook to autofill.
    /// </param>
    ///
    /// <param name="bUpdateScreen">
    /// true to turn screen updating on.
    /// </param>
    //*************************************************************************

    private static void
    SetScreenUpdating
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        Boolean bUpdateScreen
    )
    {
        Debug.Assert(oWorkbook != null);

        oWorkbook.Application.ScreenUpdating = bUpdateScreen;
    }

    //*************************************************************************
    //  Method: TryStartAutoFill()
    //
    /// <summary>
    /// Performs preliminary tasks required by every scheme.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// The workbook to autofill.
    /// </param>
    ///
    /// <param name="bShowVertexLabels">
    /// true if vertex labels should be shown.
    /// </param>
    ///
    /// <param name="sVertexLabelColumnName">
    /// The name of the vertex table column containing vertex labels.  Used
    /// only if <paramref name="bShowVertexLabels" /> is true.
    /// </param>
    ///
    /// <param name="oEdgeTable">
    /// Where the edge table gets stored if true is returned.
    /// </param>
    ///
    /// <param name="oVertexTable">
    /// Where the vertex table gets stored if true is returned.
    /// </param>
    ///
    /// <param name="oHiddenEdgeColumns">
    /// Where information about the previously hidden edge columns gets stored.
    /// </param>
    ///
    /// <param name="oHiddenVertexColumns">
    /// Where information about the previously hidden vertex columns gets
    /// stored.
    /// </param>
    ///
    /// <returns>
    /// true if autofill should continue.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryStartAutoFill
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        Boolean bShowVertexLabels,
        String sVertexLabelColumnName,
        out ListObject oEdgeTable,
        out ListObject oVertexTable,
        out ExcelHiddenColumns oHiddenEdgeColumns,
        out ExcelHiddenColumns oHiddenVertexColumns
    )
    {
        Debug.Assert(oWorkbook != null);

        SetScreenUpdating(oWorkbook, false);

        ( new VertexWorksheetPopulator() ).PopulateVertexWorksheet(
            oWorkbook, false);

        oEdgeTable = oVertexTable = null;
        oHiddenEdgeColumns = oHiddenVertexColumns = null;

        if (
            !ExcelUtil.TryGetTable(oWorkbook, WorksheetNames.Edges,
                TableNames.Edges, out oEdgeTable)
            ||
            !ExcelUtil.TryGetTable(oWorkbook, WorksheetNames.Vertices,
                TableNames.Vertices, out oVertexTable)
            )
        {
            return (false);
        }

        // Only visible cells are read and written to.  Temporarily show all
        // hidden columns in the tables.

        oHiddenEdgeColumns = ExcelColumnHider.ShowHiddenColumns(oEdgeTable);

        oHiddenVertexColumns = ExcelColumnHider.ShowHiddenColumns(
            oVertexTable);

        if (bShowVertexLabels)
        {
            TableColumnMapper.MapViaCopy(oVertexTable, sVertexLabelColumnName,
                VertexTableColumnNames.SecondaryLabel);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: FillColumnsWithConstants()
    //
    /// <summary>
    /// Fills one or more columns with constant values.
    /// </summary>
    ///
    /// <param name="aoColumnInfoTriplets">
    /// Array of column information.  There must be three elements in
    /// the array for each column to fill.  The first element is the table
    /// containing the column as a ListObject, the second is the column name as
    /// a String, and the third is the constant value, as an Object.
    /// </param>
    //*************************************************************************

    private static void
    FillColumnsWithConstants
    (
        params Object[] aoColumnInfoTriplets
    )
    {
        Debug.Assert(aoColumnInfoTriplets != null);
        Debug.Assert(aoColumnInfoTriplets.Length % 3 == 0);

        for (Int32 i = 0; i < aoColumnInfoTriplets.Length; i += 3)
        {
            // Note: Can't check for a ListObject, because it's actually a
            // System.Runtime.Remoting.Proxies.__TransparentProxy.  And for
            // some reason, the compiler won't recognize that type.

            /*
            Debug.Assert( aoColumnInfoTriplets[i + 0].GetType()
                == typeof(ListObject) );
            */

            Debug.Assert( aoColumnInfoTriplets[i + 1].GetType()
                == typeof(String) );

            ExcelUtil.SetVisibleTableColumnData(
                (ListObject)aoColumnInfoTriplets[i + 0],
                (String)aoColumnInfoTriplets[i + 1],
                aoColumnInfoTriplets[i + 2]
                );
        }
    }

    //*************************************************************************
    //  Method: EndAutoFill()
    //
    /// <summary>
    /// Performs end tasks required by every scheme.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// The workbook to autofill.
    /// </param>
    ///
    /// <param name="oEdgeTable">
    /// The edge table.  Can be null.
    /// </param>
    ///
    /// <param name="oVertexTable">
    /// The vertex table.  Can be null.
    /// </param>
    ///
    /// <param name="oHiddenEdgeColumns">
    /// Information about the previously hidden edge columns.  Can be null.
    /// </param>
    ///
    /// <param name="oHiddenVertexColumns">
    /// Where information about the previously hidden vertex columns.  Can be
    /// null.
    /// </param>
    //*************************************************************************

    private static void
    EndAutoFill
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        ListObject oEdgeTable,
        ListObject oVertexTable,
        ExcelHiddenColumns oHiddenEdgeColumns,
        ExcelHiddenColumns oHiddenVertexColumns
    )
    {
        Debug.Assert(oWorkbook != null);

        if (oEdgeTable != null && oHiddenEdgeColumns != null)
        {
            ExcelColumnHider.RestoreHiddenColumns(oEdgeTable,
                oHiddenEdgeColumns);
        }

        if (oVertexTable != null && oHiddenVertexColumns != null)
        {
            ExcelColumnHider.RestoreHiddenColumns(oVertexTable,
                oHiddenVertexColumns);
        }

        SetScreenUpdating(oWorkbook, true);
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// <summary>
    /// Minimum edge width used by <see cref="AutoFillByEdgeWeight" />.
    /// </summary>

    public static Single MinimumEdgeWeightWidthWorkbook =
        EdgeWidthConverter.MinimumWidthWorkbook;

    /// <summary>
    /// Maximum edge width used by <see cref="AutoFillByEdgeWeight" />.
    /// </summary>

    public static Single MaximumEdgeWeightWidthWorkbook =
        EdgeWidthConverter.MaximumWidthWorkbook;

    /// <summary>
    /// Minimum edge color used by <see cref="AutoFillByEdgeTimestamp" />.
    /// </summary>

    public static Color MinimumEdgeTimestampColor = Color.FromArgb(0, 11, 96);
    
    /// <summary>
    /// Maximum edge width used by <see cref="AutoFillByEdgeWeight" />.
    /// </summary>

    public static Color MaximumEdgeTimestampColor =
        Color.FromArgb(0, 134, 227);


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}


//*****************************************************************************
//  Enum: AutoFillSchemeType
//
/// <summary>
/// Specifies a type of autofill scheme.
/// </summary>
//*****************************************************************************

public enum
AutoFillSchemeType
{
    /// <summary>
    /// No scheme.
    /// </summary>

    None,

    /// <summary>
    /// The scheme filled in by <see
    /// cref="WorkbookSchemeAutoFiller.AutoFillByVertexCategory(
    /// Microsoft.Office.Interop.Excel.Workbook, String, Boolean, String)"
    /// />.
    /// </summary>

    VertexCategory,

    /// <summary>
    /// The scheme filled in by <see
    /// cref="WorkbookSchemeAutoFiller.AutoFillByEdgeWeight" />.
    /// </summary>

    EdgeWeight,

    /// <summary>
    /// The scheme filled in by <see
    /// cref="WorkbookSchemeAutoFiller.AutoFillByEdgeTimestamp" />.
    /// </summary>

    EdgeTimestamp,
}

}
