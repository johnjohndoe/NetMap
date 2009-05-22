
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: WorkbookAutoFiller
//
/// <summary>
/// Automatically fills one or more edge and vertex attribute columns by
/// mapping values from user-specified source columns.
/// </summary>
///
/// <remarks>
/// Call the <see cref="AutoFillWorkbook" /> method to autofill one or more
/// edge and vertex attribute columns.  The method takes an <see
/// cref="AutoFillUserSettings" /> object that specifies one or more
/// source-to-destination column mappings.  For example, the settings might
/// specify that the vertex color column should be autofilled by mapping the
/// numbers in a numeric source to a specified range of colors, and that the
/// edge width column should be autofilled by mapping the numbers in another
/// numeric source column to a specified range of widths.
///
/// <para>
/// All methods are static.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class WorkbookAutoFiller : Object
{
    //*************************************************************************
    //  Method: AutoFillWorkbook()
    //
    /// <summary>
    /// Runs the application's AutoFill feature on a workbook.
    /// </summary>
    ///
    /// <param name="workbook">
    /// The workbook to autofill.
    /// </param>
    ///
    /// <param name="autoFillUserSettings">
    /// Specifies one or more source-to-destination column mappings.
    /// </param>
    ///
    /// <remarks>
    /// See the class topic for information on the AutoFill feature.
    /// </remarks>
    //*************************************************************************

    public static void
    AutoFillWorkbook
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        AutoFillUserSettings autoFillUserSettings
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert(autoFillUserSettings != null);

        Application oApplication = workbook.Application;

        oApplication.ScreenUpdating = false;

        try
        {
            AutoFillWorkbookInternal(workbook, autoFillUserSettings);
        }
        finally
        {
            oApplication.ScreenUpdating = true;
        }
    }

    //*************************************************************************
    //  Method: AutoFillWorkbookInternal()
    //
    /// <summary>
    /// Runs the application's AutoFill feature on a workbook.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// The workbook to autofill.
    /// </param>
    ///
    /// <param name="oAutoFillUserSettings">
    /// Specifies one or more source-to-destination column mappings.
    /// </param>
    //*************************************************************************

    private static void
    AutoFillWorkbookInternal
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        AutoFillUserSettings oAutoFillUserSettings
    )
    {
        Debug.Assert(oWorkbook != null);
        Debug.Assert(oAutoFillUserSettings != null);

        // Populate the vertex worksheet with the name of each unique vertex in
        // the edge worksheet.

        ( new VertexWorksheetPopulator() ).PopulateVertexWorksheet(
            oWorkbook, false);

        ListObject oTable;
        ExcelHiddenColumns oHiddenColumns;

        if ( ExcelUtil.TryGetTable(oWorkbook, WorksheetNames.Edges,
            TableNames.Edges, out oTable) )
        {
            // The TableColumnMapper class that does the actual autofilling
            // fills only visible cells.  Temporarily show all hidden columns
            // in the table.

            oHiddenColumns = ExcelColumnHider.ShowHiddenColumns(oTable);

            try
            {
                AutoFillEdgeTable(oTable, oAutoFillUserSettings);
            }
            finally
            {
                ExcelColumnHider.RestoreHiddenColumns(oTable, oHiddenColumns);
            }
        }

        if ( ExcelUtil.TryGetTable(oWorkbook, WorksheetNames.Vertices,
            TableNames.Vertices, out oTable) )
        {
            oHiddenColumns = ExcelColumnHider.ShowHiddenColumns(oTable);

            try
            {
                AutoFillVertexTable(oTable, oAutoFillUserSettings);
            }
            finally
            {
                ExcelColumnHider.RestoreHiddenColumns(oTable, oHiddenColumns);
            }
        }
    }

    //*************************************************************************
    //  Method: AutoFillEdgeTable()
    //
    /// <summary>
    /// Runs the application's AutoFill feature on the edge table.
    /// </summary>
    ///
    /// <param name="oEdgeTable">
    /// The table to autofill.
    /// </param>
    ///
    /// <param name="oAutoFillUserSettings">
    /// Specifies one or more source-to-destination column mappings.
    /// </param>
    //*************************************************************************

    private static void
    AutoFillEdgeTable
    (
        ListObject oEdgeTable,
        AutoFillUserSettings oAutoFillUserSettings
    )
    {
        Debug.Assert(oEdgeTable != null);
        Debug.Assert(oAutoFillUserSettings != null);

        AutoFillColorColumn(oEdgeTable,
            oAutoFillUserSettings.EdgeColorSourceColumnName,
            EdgeTableColumnNames.Color,
            oAutoFillUserSettings.EdgeColorDetails
            );

        AutoFillNumericRangeColumn(oEdgeTable,
            oAutoFillUserSettings.EdgeWidthSourceColumnName,
            EdgeTableColumnNames.Width,
            oAutoFillUserSettings.EdgeWidthDetails
            );

        AutoFillNumericRangeColumn(oEdgeTable,
            oAutoFillUserSettings.EdgeAlphaSourceColumnName,
            EdgeTableColumnNames.Alpha,
            oAutoFillUserSettings.EdgeAlphaDetails
            );

        AutoFillNumericComparisonColumn(oEdgeTable,
            oAutoFillUserSettings.EdgeVisibilitySourceColumnName,
            EdgeTableColumnNames.Visibility,
            oAutoFillUserSettings.EdgeVisibilityDetails
            );
    }

    //*************************************************************************
    //  Method: AutoFillVertexTable()
    //
    /// <summary>
    /// Runs the application's AutoFill feature on the vertex table.
    /// </summary>
    ///
    /// <param name="oVertexTable">
    /// The table to autofill.
    /// </param>
    ///
    /// <param name="oAutoFillUserSettings">
    /// Specifies one or more source-to-destination column mappings.
    /// </param>
    //*************************************************************************

    private static void
    AutoFillVertexTable
    (
        ListObject oVertexTable,
        AutoFillUserSettings oAutoFillUserSettings
    )
    {
        Debug.Assert(oVertexTable != null);
        Debug.Assert(oAutoFillUserSettings != null);

        AutoFillColorColumn(oVertexTable,
            oAutoFillUserSettings.VertexColorSourceColumnName,
            VertexTableColumnNames.Color,
            oAutoFillUserSettings.VertexColorDetails
            );

        AutoFillNumericComparisonColumn(oVertexTable,
            oAutoFillUserSettings.VertexShapeSourceColumnName,
            VertexTableColumnNames.Shape,
            oAutoFillUserSettings.VertexShapeDetails
            );

        AutoFillNumericRangeColumn(oVertexTable,
            oAutoFillUserSettings.VertexRadiusSourceColumnName,
            VertexTableColumnNames.Radius,
            oAutoFillUserSettings.VertexRadiusDetails
            );

        AutoFillNumericRangeColumn(oVertexTable,
            oAutoFillUserSettings.VertexAlphaSourceColumnName,
            VertexTableColumnNames.Alpha,
            oAutoFillUserSettings.VertexAlphaDetails
            );

        AutoFillColumnViaCopy(oVertexTable,
            oAutoFillUserSettings.VertexPrimaryLabelSourceColumnName,
            VertexTableColumnNames.PrimaryLabel
            );

        AutoFillColorColumn(oVertexTable,
            oAutoFillUserSettings.VertexPrimaryLabelFillColorSourceColumnName,
            VertexTableColumnNames.PrimaryLabelFillColor,
            oAutoFillUserSettings.VertexPrimaryLabelFillColorDetails
            );

        AutoFillColumnViaCopy(oVertexTable,
            oAutoFillUserSettings.VertexSecondaryLabelSourceColumnName,
            VertexTableColumnNames.SecondaryLabel
            );

        AutoFillColumnViaCopy(oVertexTable,
            oAutoFillUserSettings.VertexToolTipSourceColumnName,
            VertexTableColumnNames.ToolTip
            );

        AutoFillNumericComparisonColumn(oVertexTable,
            oAutoFillUserSettings.VertexVisibilitySourceColumnName,
            VertexTableColumnNames.Visibility,
            oAutoFillUserSettings.VertexVisibilityDetails
            );

        AutoFillNumericRangeColumn(oVertexTable,
            oAutoFillUserSettings.VertexLayoutOrderSourceColumnName,
            VertexTableColumnNames.LayoutOrder,
            oAutoFillUserSettings.VertexLayoutOrderDetails
            );

        AutoFillNumericRangeColumn(oVertexTable,
            oAutoFillUserSettings.VertexXSourceColumnName,
            VertexTableColumnNames.X,
            oAutoFillUserSettings.VertexXDetails
            );

        AutoFillNumericRangeColumn(oVertexTable,
            oAutoFillUserSettings.VertexYSourceColumnName,
            VertexTableColumnNames.Y,
            oAutoFillUserSettings.VertexYDetails
            );

        if ( !String.IsNullOrEmpty(
                oAutoFillUserSettings.VertexXSourceColumnName)
            ||
            !String.IsNullOrEmpty(
                oAutoFillUserSettings.VertexYSourceColumnName)
            )
        {
            // The X or Y columns have been autofilled.  To prevent them from
            // getting overwritten when the workbook is read, lock them.

            LockVertices(oVertexTable);
        }

        AutoFillNumericRangeColumn(oVertexTable,
            oAutoFillUserSettings.VertexPolarRSourceColumnName,
            VertexTableColumnNames.PolarR,
            oAutoFillUserSettings.VertexPolarRDetails
            );

        AutoFillNumericRangeColumn(oVertexTable,
            oAutoFillUserSettings.VertexPolarAngleSourceColumnName,
            VertexTableColumnNames.PolarAngle,
            oAutoFillUserSettings.VertexPolarAngleDetails
            );

    }

    //*************************************************************************
    //  Method: AutoFillColorColumn()
    //
    /// <summary>
    /// Runs the application's AutoFill feature on a destination column that
    /// should contain colors.
    /// </summary>
    ///
    /// <param name="oTable">
    /// The table containing the source and destination columns.
    /// </param>
    ///
    /// <param name="sSourceColumnName">
    /// Name of the source column.  If null or empty, this method does nothing.
    /// </param>
    ///
    /// <param name="sDestinationColumnName">
    /// Name of the destination column.
    /// </param>
    ///
    /// <param name="oDetails">
    /// User-specified details for the destination column.
    /// </param>
    //*************************************************************************

    private static void
    AutoFillColorColumn
    (
        ListObject oTable,
        String sSourceColumnName,
        String sDestinationColumnName,
        ColorColumnAutoFillUserSettings oDetails
    )
    {
        Debug.Assert(oTable != null);
        Debug.Assert( !String.IsNullOrEmpty(sDestinationColumnName) );
        Debug.Assert(oDetails != null);

        if ( String.IsNullOrEmpty(sSourceColumnName) )
        {
            return;
        }

        TableColumnMapper.MapToColor(
            oTable, sSourceColumnName, sDestinationColumnName,
            oDetails.UseSourceNumber1,
            oDetails.UseSourceNumber2,
            oDetails.SourceNumber1,
            oDetails.SourceNumber2,
            oDetails.DestinationColor1,
            oDetails.DestinationColor2,
            oDetails.IgnoreOutliers
            );
    }

    //*************************************************************************
    //  Method: AutoFillNumericRangeColumn()
    //
    /// <summary>
    /// Runs the application's AutoFill feature on a destination column that
    /// should contain a numeric range.
    /// </summary>
    ///
    /// <param name="oTable">
    /// The table containing the source and destination columns.
    /// </param>
    ///
    /// <param name="sSourceColumnName">
    /// Name of the source column.  If null or empty, this method does nothing.
    /// </param>
    ///
    /// <param name="sDestinationColumnName">
    /// Name of the destination column.
    /// </param>
    ///
    /// <param name="oDetails">
    /// User-specified details for the destination column.
    /// </param>
    //*************************************************************************

    private static void
    AutoFillNumericRangeColumn
    (
        ListObject oTable,
        String sSourceColumnName,
        String sDestinationColumnName,
        NumericRangeColumnAutoFillUserSettings oDetails
    )
    {
        Debug.Assert(oTable != null);
        Debug.Assert( !String.IsNullOrEmpty(sDestinationColumnName) );
        Debug.Assert(oDetails != null);

        if ( String.IsNullOrEmpty(sSourceColumnName) )
        {
            return;
        }

        TableColumnMapper.MapToNumericRange(
            oTable, sSourceColumnName, sDestinationColumnName,
            oDetails.UseSourceNumber1,
            oDetails.UseSourceNumber2,
            oDetails.SourceNumber1,
            oDetails.SourceNumber2,
            oDetails.DestinationNumber1,
            oDetails.DestinationNumber2,
            oDetails.IgnoreOutliers
            );
    }

    //*************************************************************************
    //  Method: AutoFillNumericComparisonColumn()
    //
    /// <summary>
    /// Runs the application's AutoFill feature on a destination column that
    /// should contain one of two strings.
    /// </summary>
    ///
    /// <param name="oTable">
    /// The table containing the source and destination columns.
    /// </param>
    ///
    /// <param name="sSourceColumnName">
    /// Name of the source column.  If null or empty, this method does nothing.
    /// </param>
    ///
    /// <param name="sDestinationColumnName">
    /// Name of the destination column.
    /// </param>
    ///
    /// <param name="oDetails">
    /// User-specified details for the destination column.
    /// </param>
    //*************************************************************************

    private static void
    AutoFillNumericComparisonColumn
    (
        ListObject oTable,
        String sSourceColumnName,
        String sDestinationColumnName,
        NumericComparisonColumnAutoFillUserSettings oDetails
    )
    {
        Debug.Assert(oTable != null);
        Debug.Assert( !String.IsNullOrEmpty(sDestinationColumnName) );
        Debug.Assert(oDetails != null);

        if ( String.IsNullOrEmpty(sSourceColumnName) )
        {
            return;
        }

        TableColumnMapper.MapToTwoStrings(
            oTable, sSourceColumnName, sDestinationColumnName,
            oDetails.ComparisonOperator,
            oDetails.SourceNumberToCompareTo,
            oDetails.DestinationString1,
            oDetails.DestinationString2
            );
    }

    //*************************************************************************
    //  Method: AutoFillColumnViaCopy()
    //
    /// <summary>
    /// Runs the application's AutoFill feature on a destination column that
    /// should contain a copy of the source column.
    /// </summary>
    ///
    /// <param name="oTable">
    /// The table containing the source and destination columns.
    /// </param>
    ///
    /// <param name="sSourceColumnName">
    /// Name of the source column.  If null or empty, this method does nothing.
    /// </param>
    ///
    /// <param name="sDestinationColumnName">
    /// Name of the destination column.
    /// </param>
    //*************************************************************************

    private static void
    AutoFillColumnViaCopy
    (
        ListObject oTable,
        String sSourceColumnName,
        String sDestinationColumnName
    )
    {
        Debug.Assert(oTable != null);
        Debug.Assert( !String.IsNullOrEmpty(sDestinationColumnName) );

        if ( String.IsNullOrEmpty(sSourceColumnName) )
        {
            return;
        }

        TableColumnMapper.MapViaCopy(
            oTable, sSourceColumnName, sDestinationColumnName
            );
    }

    //*************************************************************************
    //  Method: LockVertices()
    //
    /// <summary>
    /// Sets the Lock cell of each visible row in the vertex table to "1".
    /// </summary>
    ///
    /// <param name="oVertexTable">
    /// Vertex table.
    /// </param>
    //*************************************************************************

    private static void
    LockVertices
    (
        ListObject oVertexTable
    )
    {
        Debug.Assert(oVertexTable != null);

        Range oLockedColumnData;

        if ( !ExcelUtil.TryGetTableColumnData(oVertexTable,
                VertexTableColumnNames.Locked, out oLockedColumnData) )
        {
            return;
        }

        ExcelUtil.SetVisibleRangeValue(oLockedColumnData, "1");
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
