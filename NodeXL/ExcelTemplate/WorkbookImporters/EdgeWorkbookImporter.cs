
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: EdgeWorkbookImporter
//
/// <summary>
/// Imports two edge columns from another open workbook to the edge worksheet.
/// </summary>
///
/// <remarks>
/// Call <see cref="ImportEdgeWorkbook" /> to import edges from another open
/// workbook to the edge worksheet of a NodeXL workbook.
/// </remarks>
//*****************************************************************************

public class EdgeWorkbookImporter : WorkbookImporterBase
{
    //*************************************************************************
    //  Constructor: EdgeWorkbookImporter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EdgeWorkbookImporter" />
    /// class.
    /// </summary>
    //*************************************************************************

    public EdgeWorkbookImporter()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: ImportEdgeWorkbook()
    //
    /// <summary>
    /// Imports edges from another open workbook to the edge worksheet of a
    /// NodeXL workbook.
    /// </summary>
    ///
    /// <param name="sourceWorkbookName">
    /// Workbook.Name of the open workbook that contains the edge columns to
    /// import.  The workbook's active worksheet can't be empty.
    /// </param>
    ///
    /// <param name="oneBasedColumnNumbersToImport">
    /// Collection of one-based column numbers to import from the source
    /// workbook.
    /// </param>
    ///
    /// <param name="columnNumberToUseForVertex1OneBased">
    /// One-based column number to use for vertex 1.
    /// </param>
    ///
    /// <param name="columnNumberToUseForVertex2OneBased">
    /// One-based column number to use for vertex 2.
    /// </param>
    ///
    /// <param name="sourceColumnsHaveHeaders">
    /// true if the columns have headers that should be ignored.
    /// </param>
    ///
    /// <param name="clearDestinationTablesFirst">
    /// true if the NodeXL tables in <paramref
    /// name="destinationNodeXLWorkbook" /> should be cleared first.
    /// </param>
    ///
    /// <param name="destinationNodeXLWorkbook">
    /// NodeXL workbook the edge columns will be imported to.
    /// </param>
    ///
    /// <remarks>
    /// This method copies the specified columns from the active worksheet of
    /// <paramref name="sourceWorkbookName" /> to the edge worksheet of
    /// <paramref name="destinationNodeXLWorkbook" />.  If the columns can't be
    /// copied, an <see cref="ImportWorkbookException" /> is thrown.
    ///
    /// <para>
    /// The columns are copied starting from either the first or second
    /// non-empty row of the source worksheet, depending on the value of
    /// <paramref name="sourceColumnsHaveHeaders" />, and ending at row N,
    /// where N is the last non-empty row of the source worksheet.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    ImportEdgeWorkbook
    (
        String sourceWorkbookName,
        ICollection<Int32> oneBasedColumnNumbersToImport,
        Int32 columnNumberToUseForVertex1OneBased,
        Int32 columnNumberToUseForVertex2OneBased,
        Boolean sourceColumnsHaveHeaders,
        Boolean clearDestinationTablesFirst,
        Microsoft.Office.Interop.Excel.Workbook destinationNodeXLWorkbook
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sourceWorkbookName) );
        Debug.Assert(oneBasedColumnNumbersToImport != null);
        Debug.Assert(columnNumberToUseForVertex1OneBased >= 1);
        Debug.Assert(columnNumberToUseForVertex2OneBased >= 1);
        Debug.Assert(destinationNodeXLWorkbook != null);
        AssertValid();

        // Get the active worksheet of the source workbook.

        Application oApplication = destinationNodeXLWorkbook.Application;

        Worksheet oSourceWorksheet = GetActiveSourceWorksheet(
            oApplication, sourceWorkbookName);

        // Get the boundaries of the non-empty range in the source worksheet.

        Range oNonEmptySourceRange = GetNonEmptySourceRange(oSourceWorksheet);

        Int32 iFirstNonEmptySourceRowOneBased = oNonEmptySourceRange.Row;

        Int32 iLastNonEmptySourceRowOneBased = oNonEmptySourceRange.Rows.Count
            + iFirstNonEmptySourceRowOneBased - 1;

        // Get the edge table in the destination NodeXL workbook.

        ListObject oDestinationEdgeTable = GetDestinationEdgeTable(
            destinationNodeXLWorkbook);

        ExcelUtil.ActivateWorksheet(oDestinationEdgeTable);

        Int32 iDestinationRowOffset = 0;

        if (clearDestinationTablesFirst)
        {
            NodeXLWorkbookUtil.ClearTables(destinationNodeXLWorkbook);
        }
        else
        {
            iDestinationRowOffset =
                ExcelUtil.GetOffsetOfFirstEmptyTableRow(oDestinationEdgeTable);
        }

        // Copy the vertex columns to the destination NodeXL workbook.

        CopyVertexColumn(oSourceWorksheet, columnNumberToUseForVertex1OneBased,
            sourceColumnsHaveHeaders, iFirstNonEmptySourceRowOneBased,
            iLastNonEmptySourceRowOneBased, oDestinationEdgeTable,
            iDestinationRowOffset, true);

        CopyVertexColumn(oSourceWorksheet, columnNumberToUseForVertex2OneBased,
            sourceColumnsHaveHeaders, iFirstNonEmptySourceRowOneBased,
            iLastNonEmptySourceRowOneBased, oDestinationEdgeTable,
            iDestinationRowOffset, false);

        // Copy the other columns.

        CopyOtherColumns(oSourceWorksheet, oneBasedColumnNumbersToImport,
            columnNumberToUseForVertex1OneBased,
            columnNumberToUseForVertex2OneBased, sourceColumnsHaveHeaders,
            iFirstNonEmptySourceRowOneBased, iLastNonEmptySourceRowOneBased,
            iDestinationRowOffset, oDestinationEdgeTable);

        // Clear the moving border and selection.  The odd cast is to work
        // around the inability to set CutCopyMode to false.

        oApplication.CutCopyMode = (XlCutCopyMode)0;

        Range oHeaderRowRange = oDestinationEdgeTable.HeaderRowRange;

        if (oHeaderRowRange != null)
        {
            ExcelUtil.SelectRange(oHeaderRowRange);
        }
    }

    //*************************************************************************
    //  Method: GetNonEmptySourceRange()
    //
    /// <summary>
    /// Gets the non-empty range in the source worksheet.
    /// </summary>
    ///
    /// <param name="oSourceWorksheet">
    /// Worksheet to get the non-empty range from.
    /// </param>
    ///
    /// <returns>
    /// Non-empty range of <paramref name="oSourceWorksheet" />.
    /// </returns>
    //*************************************************************************

    protected Range
    GetNonEmptySourceRange
    (
        Worksheet oSourceWorksheet
    )
    {
        Debug.Assert(oSourceWorksheet != null);
        AssertValid();

        Range oNonEmptySourceRange;

        if ( !ExcelUtil.TryGetNonEmptyRangeInWorksheet(oSourceWorksheet,
            out oNonEmptySourceRange) )
        {
            OnInvalidSourceWorkbook(
                "The active worksheet in the other workbook is empty.",
                oSourceWorksheet, 1, 1
                );
        }

        return (oNonEmptySourceRange);
    }

    //*************************************************************************
    //  Method: CopyVertexColumn()
    //
    /// <summary>
    /// Copies a vertex column from the source worksheet to the destination
    /// NodeXL workbook.
    /// </summary>
    ///
    /// <param name="oSourceWorksheet">
    /// Source worksheet to copy the vertex column from.
    /// </param>
    ///
    /// <param name="iSourceColumnNumberOneBased">
    /// One-based column number to copy from.
    /// </param>
    ///
    /// <param name="bSourceColumnsHaveHeaders">
    /// true if the columns have headers that should be ignored.
    /// </param>
    ///
    /// <param name="iFirstNonEmptySourceRowOneBased">
    /// One-based row number of the first non-empty row in the source
    /// worksheet.
    /// </param>
    ///
    /// <param name="iLastNonEmptySourceRowOneBased">
    /// One-based row number of the last non-empty row in the source worksheet.
    /// </param>
    ///
    /// <param name="oDestinationEdgeTable">
    /// Edge table in the destination NodeXL workbook.
    /// </param>
    ///
    /// <param name="iDestinationRowOffset">
    /// Offset to copy to in the destination table, measured from the first
    /// row in the table's data range.
    /// </param>
    ///
    /// <param name="bCopyVertex1">
    /// true to copy the vertex 1 column, false to copy the vertex 2 column.
    /// </param>
    //*************************************************************************

    protected void
    CopyVertexColumn
    (
        Worksheet oSourceWorksheet,
        Int32 iSourceColumnNumberOneBased,
        Boolean bSourceColumnsHaveHeaders,
        Int32 iFirstNonEmptySourceRowOneBased,
        Int32 iLastNonEmptySourceRowOneBased,
        ListObject oDestinationEdgeTable,
        Int32 iDestinationRowOffset,
        Boolean bCopyVertex1
    )
    {
        Debug.Assert(oSourceWorksheet != null);
        Debug.Assert(iSourceColumnNumberOneBased >= 1);
        Debug.Assert(iFirstNonEmptySourceRowOneBased >= 1);
        Debug.Assert(iLastNonEmptySourceRowOneBased >= 1);
        Debug.Assert(oDestinationEdgeTable != null);
        AssertValid();

        Range oVertexSourceColumn = oSourceWorksheet.get_Range(

            oSourceWorksheet.Cells[iFirstNonEmptySourceRowOneBased,
                iSourceColumnNumberOneBased],

            oSourceWorksheet.Cells[
                iLastNonEmptySourceRowOneBased,
                iSourceColumnNumberOneBased]
            );

        if (bSourceColumnsHaveHeaders)
        {
            RemoveSourceColumnHeader(ref oVertexSourceColumn);
        }

        Range oDestinationColumnData = GetVertexColumnData(
            oDestinationEdgeTable, bCopyVertex1);

        CopyColumn(oVertexSourceColumn, oDestinationColumnData,
            iDestinationRowOffset);
    }

    //*************************************************************************
    //  Method: CopyOtherColumns()
    //
    /// <summary>
    /// Copies the other columns from the source worksheet to the destination
    /// NodeXL workbook.
    /// </summary>
    ///
    /// <param name="oSourceWorksheet">
    /// Source worksheet to copy the columns from.
    /// </param>
    ///
    /// <param name="oOneBasedColumnNumbersToImport">
    /// Collection of one-based column numbers to import from the source
    /// workbook.
    /// </param>
    ///
    /// <param name="iColumnNumberToUseForVertex1OneBased">
    /// One-based column number that was used for vertex 1.
    /// </param>
    ///
    /// <param name="iColumnNumberToUseForVertex2OneBased">
    /// One-based column number that was used for vertex 2.
    /// </param>
    ///
    /// <param name="bSourceColumnsHaveHeaders">
    /// true if the columns have headers that should be ignored.
    /// </param>
    ///
    /// <param name="iFirstNonEmptySourceRowOneBased">
    /// One-based row number of the first non-empty row in the source
    /// worksheet.
    /// </param>
    ///
    /// <param name="iLastNonEmptySourceRowOneBased">
    /// One-based row number of the last non-empty row in the source worksheet.
    /// </param>
    ///
    /// <param name="iDestinationRowOffset">
    /// Offset to copy to in the destination table, measured from the first
    /// row in the table's data range.
    /// </param>
    ///
    /// <param name="oDestinationEdgeTable">
    /// Edge table in the destination NodeXL workbook.
    /// </param>
    //*************************************************************************

    protected void
    CopyOtherColumns
    (
        Worksheet oSourceWorksheet,
        ICollection<Int32> oOneBasedColumnNumbersToImport,
        Int32 iColumnNumberToUseForVertex1OneBased,
        Int32 iColumnNumberToUseForVertex2OneBased,
        Boolean bSourceColumnsHaveHeaders,
        Int32 iFirstNonEmptySourceRowOneBased,
        Int32 iLastNonEmptySourceRowOneBased,
        Int32 iDestinationRowOffset,
        ListObject oDestinationEdgeTable
    )
    {
        Debug.Assert(oSourceWorksheet != null);
        Debug.Assert(oOneBasedColumnNumbersToImport != null);
        Debug.Assert(iColumnNumberToUseForVertex1OneBased >= 1);
        Debug.Assert(iColumnNumberToUseForVertex2OneBased >= 1);
        Debug.Assert(iFirstNonEmptySourceRowOneBased >= 1);
        Debug.Assert(iLastNonEmptySourceRowOneBased >= 1);
        AssertValid();

        // Number to use for the name of a destination column for which the
        // source column has no header.

        Int32 iHeaderlessColumnNumber = 1;

        foreach (Int32 iOneBasedColumnNumberToImport in
            oOneBasedColumnNumbersToImport)
        {
            if (
                iOneBasedColumnNumberToImport ==
                    iColumnNumberToUseForVertex1OneBased
                ||
                iOneBasedColumnNumberToImport ==
                    iColumnNumberToUseForVertex2OneBased
                )
            {
                // The vertex columns have already been copied.

                continue;
            }

            String sDestinationColumnName = null;

            // Get the source column.

            Range oSourceColumn = oSourceWorksheet.get_Range(

                oSourceWorksheet.Cells[iFirstNonEmptySourceRowOneBased,
                    iOneBasedColumnNumberToImport],

                oSourceWorksheet.Cells[
                    iLastNonEmptySourceRowOneBased,
                    iOneBasedColumnNumberToImport]
                );

            if (bSourceColumnsHaveHeaders)
            {
                // Get the column name from the header.

                if ( !ExcelUtil.TryGetNonEmptyStringFromCell(

                        ExcelUtil.GetRangeValues(
                            (Range)oSourceColumn.Cells[1, 1] ),

                        1, 1, out sDestinationColumnName) )
                {
                    sDestinationColumnName = null;
                }

                // Remove the header.

                RemoveSourceColumnHeader(ref oSourceColumn);
            }

            if ( String.IsNullOrEmpty(sDestinationColumnName) )
            {
                // Assign an arbitrary column name to the new column.

                sDestinationColumnName =
                    "Imported Column " + iHeaderlessColumnNumber.ToString();

                iHeaderlessColumnNumber++;
            }

            // Check whether the destination column already exists.

            ListColumn oDestinationColumn;
            Range oDestinationColumnData = null;

            if (
                !ExcelUtil.TryGetOrAddTableColumn(oDestinationEdgeTable,
                    sDestinationColumnName, ExcelUtil.AutoColumnWidth, null,
                    out oDestinationColumn)
                ||
                !ExcelUtil.TryGetTableColumnData(oDestinationColumn,
                    out oDestinationColumnData)
                )
            {
                OnInvalidSourceWorkbook(
                    "One of the columns couldn't be imported.  Importation"
                    + " was stopped."
                    );
            }

            CopyColumn(oSourceColumn, oDestinationColumnData,
                iDestinationRowOffset);
        }
    }

    //*************************************************************************
    //  Method: CopyColumn()
    //
    /// <summary>
    /// Copies a column from the source worksheet to the destination NodeXL
    /// workbook.
    /// </summary>
    ///
    /// <param name="oSourceColumn">
    /// The source column to copy.
    /// </param>
    ///
    /// <param name="oDestinationColumnData">
    /// Data range of the destination column.
    /// </param>
    ///
    /// <param name="iDestinationRowOffset">
    /// Offset to copy to in the destination table, measured from the first
    /// row in the table's data range.
    /// </param>
    //*************************************************************************

    protected void
    CopyColumn
    (
        Range oSourceColumn,
        Range oDestinationColumnData,
        Int32 iDestinationRowOffset
    )
    {
        Debug.Assert(oSourceColumn != null);
        Debug.Assert(oDestinationColumnData != null);
        AssertValid();

        // The destination range will have more rows in it than the source
        // column if this is not the first copy operation.

        ExcelUtil.ResizeRange(ref oDestinationColumnData,
            oSourceColumn.Rows.Count, 1);

        ExcelUtil.OffsetRange(ref oDestinationColumnData,
            iDestinationRowOffset, 0);

        oSourceColumn.Copy(Missing.Value);
        ExcelUtil.PasteValues(oDestinationColumnData);
    }

    //*************************************************************************
    //  Method: RemoveSourceColumnHeader()
    //
    /// <summary>
    /// Removes the header from one of the source columns.
    /// </summary>
    ///
    /// <param name="oSourceColumn">
    /// The source column to remove the header from.
    /// </param>
    //*************************************************************************

    protected void
    RemoveSourceColumnHeader
    (
        ref Range oSourceColumn
    )
    {
        AssertValid();

        Int32 iRows = oSourceColumn.Rows.Count;

        if (iRows == 1)
        {
            OnInvalidSourceWorkbook(
                "There are no edges to import.",
                oSourceColumn.Worksheet, 1, 1
                );
        }

        // Trim the last row of the range, then shift the range down one row.

        oSourceColumn = oSourceColumn.get_Resize(iRows - 1, 1);
        oSourceColumn = oSourceColumn.get_Offset(1, 0);
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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
