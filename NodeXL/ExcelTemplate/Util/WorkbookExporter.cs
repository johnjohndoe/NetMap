
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: WorkbookExporter
//
/// <summary>
/// Exports a workbook's contents.
/// </summary>
///
/// <remarks>
/// Use <see cref="ExportSelectionToNewWorkbook" /> to export the selected rows
/// of the edge and vertex tables to a new workbook.
/// </remarks>
//*****************************************************************************

public class WorkbookExporter
{
    //*************************************************************************
    //  Constructor: WorkbookExporter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="WorkbookExporter" />
    /// class.
    /// </summary>
    ///
    /// <param name="workbook">
    /// The workbook to export.
    /// </param>
    //*************************************************************************

    public WorkbookExporter
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        m_oWorkbook = workbook;

        AssertValid();
    }

    //*************************************************************************
    //  Method: ExportSelectionToNewWorkbook()
    //
    /// <summary>
    /// Exports the selected rows of the edge and vertex tables to a new
    /// workbook.
    /// </summary>
    ///
    /// <returns>
    /// The new workbook.
    /// </returns>
    ///
    /// <remarks>
    /// The new workbook is created from the NodeXL template.
    ///
    /// <para>
    /// The active worksheet is changed several times.  The caller should turn
    /// off screen updating and save and restore the active worksheet.
    /// </para>
    ///
    /// <para>
    /// If there are no selected rows in the edge or vertex tables, or if
    /// an error occurs while exported the selected rows, an <see
    /// cref="ExportWorkbookException" /> exception is thrown.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Microsoft.Office.Interop.Excel.Workbook
    ExportSelectionToNewWorkbook()
    {
        AssertValid();

        Application oApplication = m_oWorkbook.Application;
        Workbook oNewWorkbook = null;

        // Get the path to the application's template.

        String sTemplatePath;

        if ( !ApplicationUtil.TryGetTemplatePath(oApplication,
            out sTemplatePath) )
        {
            throw new ExportWorkbookException(
                ApplicationUtil.GetMissingTemplateMessage(oApplication) );
        }

        #if false  // For development only.
        sTemplatePath = @"E:\NodeXL\ExcelTemplate\bin\Debug\NodeXLGraph.xltx";
        #endif

        Range oVisibleSelectedTableRange;
        ListObject oTable;

        // Get the visible, selected range within the edge table.

        if (
            ExcelUtil.TryGetVisibleSelectedTableRange(m_oWorkbook,
                WorksheetNames.Edges, TableNames.Edges, out oTable,
                out oVisibleSelectedTableRange)
            &&
            !ExcelUtil.TableIsEmpty(oTable)
            )
        {
            // Create the new workbook and copy the edge table's selected rows
            // to it.

            oNewWorkbook = oApplication.Workbooks.Add(sTemplatePath);

            CopyRowsToNewWorkbook(oTable, oVisibleSelectedTableRange,
                oNewWorkbook);
        }

        // Get the selected range within the vertex table.

        if (
            ExcelUtil.TryGetVisibleSelectedTableRange(m_oWorkbook,
                WorksheetNames.Vertices, TableNames.Vertices, out oTable,
                out oVisibleSelectedTableRange)
            &&
            !ExcelUtil.TableIsEmpty(oTable)
            )
        {
            // Create the new workbook if necessary and copy the vertex table's
            // selected rows to it.

            if (oNewWorkbook == null)
            {
                oNewWorkbook = oApplication.Workbooks.Add(sTemplatePath);
            }

            CopyRowsToNewWorkbook(oTable, oVisibleSelectedTableRange,
                oNewWorkbook);
        }

        if (oNewWorkbook == null)
        {
            throw new ExportWorkbookException(
                "There are no selected edges or vertices to export to a new"
                + " workbook."
                );
        }
        else
        {
            // Copy the image table.  To keep things simple, copy the entire
            // table, even though some or none of the rows may be referenced
            // by the copied vertices.

            Range oVisibleTableRange;

            if (
                ExcelUtil.TryGetTable(m_oWorkbook, WorksheetNames.Images,
                    TableNames.Images, out oTable)
                &&
                ExcelUtil.TryGetVisibleTableRange(oTable,
                    out oVisibleTableRange)
                )
            {
                CopyRowsToNewWorkbook(oTable, oVisibleTableRange,
                    oNewWorkbook);
            }
        }

        return (oNewWorkbook);
    }

    //*************************************************************************
    //  Method: CopyRowsToNewWorkbook()
    //
    /// <summary>
    /// Copies specified rows in a table to a new workbook.
    /// </summary>
    ///
    /// <param name="oSourceTable">
    /// Table to copy from.
    /// </param>
    ///
    /// <param name="oSourceTableRangeToCopy">
    /// For each row in <paramref name="oSourceTable" /> that intersects this
    /// range, the row is copied to the new workbook.  The may contain multiple
    /// areas.
    /// </param>
    ///
    /// <param name="oNewWorkbook">
    /// The new workbook to copy the rows to.
    /// </param>
    //*************************************************************************

    protected void
    CopyRowsToNewWorkbook
    (
        ListObject oSourceTable,
        Range oSourceTableRangeToCopy,
        Workbook oNewWorkbook
    )
    {
        Debug.Assert(oSourceTable != null);
        Debug.Assert(oSourceTableRangeToCopy != null);
        Debug.Assert(oNewWorkbook != null);
        AssertValid();

        String sTableName = oSourceTable.Name;

        Debug.Assert(oSourceTable.Parent is Worksheet);

        String sWorksheetName = ( (Worksheet)oSourceTable.Parent ).Name;

        ListObject oNewTable;

        if ( !ExcelUtil.TryGetTable(oNewWorkbook, sWorksheetName, sTableName,
            out oNewTable) )
        {
            throw new ExportWorkbookException(
                "A table is missing in the new workbook."
                );
        }

        foreach (ListColumn oSourceColumn in oSourceTable.ListColumns)
        {
            // Add the column to the new table if it doesn't already exist.

            Range oNewColumnData =
                GetOrAddTableColumn(oSourceColumn, oNewTable);

            Range oSourceColumnData = oSourceColumn.DataBodyRange;

            Debug.Assert(oSourceColumnData != null);
            Debug.Assert(oNewColumnData != null);

            // Read the source column.

            Object [,] aoSourceValues =
                ExcelUtil.GetRangeValues(oSourceColumnData);

            // Create a collection to hold the specified column cells to be
            // copied.

            LinkedList<Object> oNewValues = new LinkedList<Object>();

            // Get the cells to be copied.

            foreach (Range oSourceTableRangeArea in
                oSourceTableRangeToCopy.Areas)
            {
                Int32 iFirstRowOneBased =
                    oSourceTableRangeArea.Row - oSourceColumnData.Row + 1;

                Int32 iLastRowOneBased =
                    iFirstRowOneBased + oSourceTableRangeArea.Rows.Count - 1;

                for (Int32 iRowOneBased = iFirstRowOneBased;
                    iRowOneBased <= iLastRowOneBased; iRowOneBased++)
                {
                    oNewValues.AddLast( aoSourceValues[iRowOneBased, 1] );
                }
            }

            // Copy the cells to the new workbook.

            Int32 iValuesToCopy = oNewValues.Count;

            if (iValuesToCopy > 0)
            {
                Object [,] aoNewValues =
                    ExcelUtil.GetSingleColumn2DArray(iValuesToCopy);

                Int32 iNewValueIndexOneBased = 1;

                foreach (Object oNewValue in oNewValues)
                {
                    aoNewValues[iNewValueIndexOneBased, 1] = oNewValue;
                    iNewValueIndexOneBased++;
                }

                ExcelUtil.SetRangeValues(oNewColumnData, aoNewValues);
            }
        }
    }

    //*************************************************************************
    //  Method: GetOrAddTableColumn()
    //
    /// <summary>
    /// Adds a column to a new table if the column doesn't already exist.
    /// </summary>
    ///
    /// <param name="oSourceColumn">
    /// Column in the source table.
    /// </param>
    ///
    /// <param name="oNewTable">
    /// Table to add a column to if necessary.
    /// </param>
    ///
    /// <returns>
    /// The data body range of the column in the new table.
    /// </returns>
    //*************************************************************************

    protected Range
    GetOrAddTableColumn
    (
        ListColumn oSourceColumn,
        ListObject oNewTable
    )
    {
        Debug.Assert(oSourceColumn != null);
        Debug.Assert(oNewTable != null);
        AssertValid();

        String sColumnName = oSourceColumn.Name;

        Range oSourceColumnData = oSourceColumn.DataBodyRange;
        Debug.Assert(oSourceColumnData != null);

        ListColumn oNewColumn;

        if ( !ExcelUtil.TryGetOrAddTableColumn(oNewTable, sColumnName,
                (Double)oSourceColumnData.ColumnWidth, null,
                out oNewColumn) )
        {
            throw new ExportWorkbookException(
                "A column couldn't be added to the new workbook."
                );
        }

        Range oNewColumnData = oNewColumn.DataBodyRange;

        Debug.Assert(oNewColumnData != null);

        oNewColumnData.NumberFormat = oSourceColumnData.NumberFormat;

        return (oNewColumnData);
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
        Debug.Assert(m_oWorkbook != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Workbook to export.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;
}

}
