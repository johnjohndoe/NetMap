
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: EdgeImporter
//
/// <summary>
/// Imports edges from another open workbook to the edge worksheet.
/// </summary>
///
/// <remarks>
/// Use <see cref="ImportEdges" /> to import edges from another open workbook
/// to the edge worksheet of a NodeXL workbook.
/// </remarks>
//*****************************************************************************

public class EdgeImporter : Object
{
    //*************************************************************************
    //  Constructor: EdgeImporter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EdgeImporter" /> class.
    /// </summary>
    //*************************************************************************

    public EdgeImporter()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: ImportEdges()
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
    /// <param name="destinationNodeXLWorkbook">
    /// NodeXL workbook the edge columns will be imported to.
    /// </param>
    ///
    /// <remarks>
    /// This method copies the specified columns from the active worksheet of
    /// <paramref name="sourceWorkbookName" /> to the edge worksheet of
    /// <paramref name="destinationNodeXLWorkbook" />.  If the columns can't be
    /// copied, an <see cref="ImportEdgeException" /> is thrown.
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
    ImportEdges
    (
        String sourceWorkbookName,
        ICollection<Int32> oneBasedColumnNumbersToImport,
        Int32 columnNumberToUseForVertex1OneBased,
        Int32 columnNumberToUseForVertex2OneBased,
        Boolean sourceColumnsHaveHeaders,
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

        // Clear the various tables.

        NodeXLWorkbookUtil.ClearTables(destinationNodeXLWorkbook);

        // Copy the vertex columns to the destination NodeXL workbook.

        CopyVertexColumn(oSourceWorksheet, columnNumberToUseForVertex1OneBased,
            sourceColumnsHaveHeaders, iFirstNonEmptySourceRowOneBased,
            iLastNonEmptySourceRowOneBased, oDestinationEdgeTable,
            EdgeTableColumnNames.Vertex1Name);

        CopyVertexColumn(oSourceWorksheet, columnNumberToUseForVertex2OneBased,
            sourceColumnsHaveHeaders, iFirstNonEmptySourceRowOneBased,
            iLastNonEmptySourceRowOneBased, oDestinationEdgeTable,
            EdgeTableColumnNames.Vertex2Name);

        // Copy the other columns.

        CopyOtherColumns(oSourceWorksheet, oneBasedColumnNumbersToImport,
            columnNumberToUseForVertex1OneBased,
            columnNumberToUseForVertex2OneBased, sourceColumnsHaveHeaders,
            iFirstNonEmptySourceRowOneBased, iLastNonEmptySourceRowOneBased,
            oDestinationEdgeTable);

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
    //  Method: GetActiveSourceWorksheet()
    //
    /// <summary>
    /// Gets the active worksheet of the source workbook.
    /// </summary>
    ///
    /// <param name="oApplication">
    /// Excel application.
    /// </param>
    ///
    /// <param name="sSourceWorkbookName">
    /// Workbook.Name of the open workbook that contains the edge columns to
    /// import.
    /// </param>
    ///
    /// <returns>
    /// The source workbook's active worksheet.
    /// </returns>
    //*************************************************************************

    protected Worksheet
    GetActiveSourceWorksheet
    (
        Application oApplication,
        String sSourceWorkbookName
    )
    {
        Debug.Assert(oApplication != null);
        Debug.Assert( !String.IsNullOrEmpty(sSourceWorkbookName) );
        AssertValid();

        Microsoft.Office.Interop.Excel.Workbook oSourceWorkbook =
            oApplication.Workbooks[sSourceWorkbookName];

        Object oSourceWorksheetAsObject = oSourceWorkbook.ActiveSheet;

        Debug.Assert(oSourceWorksheetAsObject != null);
        Debug.Assert(oSourceWorksheetAsObject is Worksheet);

        return ( (Worksheet)oSourceWorksheetAsObject );
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

        if ( !ExcelUtil.TryGetNonEmptyRange(oSourceWorksheet,
            out oNonEmptySourceRange) )
        {
            throw new ImportEdgeException(
                "The active worksheet in the other workbook is empty."
                );
        }

        return (oNonEmptySourceRange);
    }

    //*************************************************************************
    //  Method: GetDestinationEdgeTable()
    //
    /// <summary>
    /// Gets the edge table in the destination NodeXL workbook.
    /// </summary>
    ///
    /// <param name="oDestinationNodeXLWorkbook">
    /// NodeXL Workbook containing the edge table.
    /// </param>
    ///
    /// <returns>
    /// The edge table.
    /// </returns>
    //*************************************************************************

    protected ListObject
    GetDestinationEdgeTable
    (
        Microsoft.Office.Interop.Excel.Workbook oDestinationNodeXLWorkbook
    )
    {
        Debug.Assert(oDestinationNodeXLWorkbook != null);

        EdgeWorksheetReader oEdgeWorksheetReader = new EdgeWorksheetReader();

        return ( oEdgeWorksheetReader.GetEdgeTable(
            oDestinationNodeXLWorkbook) );
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
    /// <param name="sDestinationColumnName">
    /// Name of the column to copy to.
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
        String sDestinationColumnName
    )
    {
        Debug.Assert(oSourceWorksheet != null);
        Debug.Assert(iSourceColumnNumberOneBased >= 1);
        Debug.Assert(iFirstNonEmptySourceRowOneBased >= 1);
        Debug.Assert(iLastNonEmptySourceRowOneBased >= 1);
        Debug.Assert( !String.IsNullOrEmpty(sDestinationColumnName) );
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

        Range oDestinationColumnData;

        if (
            !ExcelUtil.TryGetTableColumnData(oDestinationEdgeTable,
                sDestinationColumnName, out oDestinationColumnData)
            )
        {
            throw new ImportEdgeException(
                "One of the edge columns is missing from the NodeXL workbook."
                );
        }

        oVertexSourceColumn.Copy(Missing.Value);
        ExcelUtil.PasteValues(oDestinationColumnData);
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

        // Get a dictionary of reserved edge table column names.  The key is
        // the reserved column name and the value isn't used.

        Dictionary<String, Char> oReservedColumnNames =
            GetReservedColumnNames();

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
                else if ( oReservedColumnNames.ContainsKey(
                    sDestinationColumnName) )
                {
                    // The source column name is one of NodeXL's reserved
                    // column names.  Add a suffix to distinguish the new
                    // column from the reserved column.

                    sDestinationColumnName += " 2";
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

            Range oDestinationColumnData = null;

            if ( !ExcelUtil.TryGetTableColumnData(oDestinationEdgeTable,
                sDestinationColumnName, out oDestinationColumnData) )
            {
                // Add a new column to the edge table.

                ListColumn oDestinationColumn;

                if (
                    !ExcelUtil.TryAddTableColumn(oDestinationEdgeTable,
                        sDestinationColumnName, ExcelUtil.AutoColumnWidth,
                        null, out oDestinationColumn)
                    ||
                    !ExcelUtil.TryGetTableColumnData(oDestinationEdgeTable,
                        oDestinationColumn.Name, out oDestinationColumnData)
                    )
                {
                    throw new ImportEdgeException(
                        "One of the columns couldn't be imported.  Importation"
                        + " was stopped."
                        );
                }
            }

            // Copy the source column to the destination column.

            oSourceColumn.Copy(Missing.Value);
            ExcelUtil.PasteValues(oDestinationColumnData);
        }
    }

    //*************************************************************************
    //  Method: GetReservedColumnNames()
    //
    /// <summary>
    /// Gets a dictionary of reserved edge table column names.
    /// </summary>
    ///
    /// <returns>
    /// A dictionary of reserved edge table column names.  The key is the
    /// reserved column name and the value isn't used.
    /// </returns>
    //*************************************************************************

    protected Dictionary<String, Char>
    GetReservedColumnNames()
    {
        AssertValid();

        Dictionary<String, Char> oReservedColumnNames =
            new Dictionary<String, Char>();

        // Use reflection to get the column names, which are declared as fields
        // within the static EdgeTableColumnNames class.

        Type oEdgeTableColumnNamesType = typeof(EdgeTableColumnNames);

        foreach ( FieldInfo oFieldInfo in
            oEdgeTableColumnNamesType.GetFields() )
        {
            if (oFieldInfo.DeclaringType == oEdgeTableColumnNamesType)
            {
                String sColumnName = oFieldInfo.GetValue(null).ToString();
                oReservedColumnNames.Add(sColumnName, ' ');
            }
        }

        return (oReservedColumnNames);
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
            throw new ImportEdgeException(
                "There are no edges to import."
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

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
