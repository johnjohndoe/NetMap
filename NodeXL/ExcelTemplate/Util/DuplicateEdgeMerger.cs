
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Algorithms;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: DuplicateEdgeMerger
//
/// <summary>
/// Merges duplicate edges in the edge worksheet.
/// </summary>
///
/// <remarks>
/// Use <see cref="MergeDuplicateEdges(Workbook)" /> to merge duplicate edges
/// in the edge worksheet.
/// </remarks>
//*****************************************************************************

public class DuplicateEdgeMerger : Object
{
    //*************************************************************************
    //  Constructor: DuplicateEdgeMerger()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateEdgeMerger" />
    /// class.
    /// </summary>
    //*************************************************************************

    public DuplicateEdgeMerger()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: MergeDuplicateEdges()
    //
    /// <summary>
    /// Merges duplicate edges in the edge worksheet.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the edge worksheet.
    /// </param>
    ///
    /// <remarks>
    /// This method adds a tie strength column to the edge worksheet, then
    /// searches for rows that represent the same edge.  For each set of
    /// duplicate rows, all but the first row in the set are deleted, and the
    /// tie strength cell for the first row is set to the number of duplicate
    /// edges.
    ///
    /// <para>
    /// Any AutoFiltering on the edge table is cleared.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    MergeDuplicateEdges
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        Debug.Assert(workbook != null);
        AssertValid();

        ListObject oEdgeTable;

        if (
            !ExcelUtil.TryGetTable(workbook, WorksheetNames.Edges,
                TableNames.Edges, out oEdgeTable)
            ||
            oEdgeTable.DataBodyRange == null
            )
        {
            // (The DataBodyRange test catches the odd case where the user
            // deletes the first data row of the table.  It looks like the row
            // is still there, but it's not.  Continuing with a null
            // DataBodyRange can cause a variety of problems.)

            return;
        }

        ExcelUtil.ActivateWorksheet(oEdgeTable);

        // Clear AutoFiltering, which would make this code much more
        // complicated.

        ExcelUtil.ClearTableAutoFilters(oEdgeTable);

        // Get the vertex name data.

        Range oVertex1NameData, oVertex2NameData;
        Object [,] aoVertex1NameValues, aoVertex2NameValues;
        
        if (
            !ExcelUtil.TryGetTableColumnDataAndValues(oEdgeTable,
                EdgeTableColumnNames.Vertex1Name, out oVertex1NameData,
                out aoVertex1NameValues)
            ||
            !ExcelUtil.TryGetTableColumnDataAndValues(oEdgeTable,
                EdgeTableColumnNames.Vertex2Name, out oVertex2NameData,
                out aoVertex2NameValues)
            )
        {
            return;
        }

        // Add a tie strength column if it doesn't already exist.

        ListColumn oTieStrengthColumn;

        if (
            !ExcelUtil.TryGetTableColumn(oEdgeTable,
                EdgeTableColumnNames.TieStrength, out oTieStrengthColumn)
            &&
            !ExcelUtil.TryAddTableColumn(oEdgeTable,
                EdgeTableColumnNames.TieStrength,
                ExcelUtil.AutoColumnWidth, null,
                out oTieStrengthColumn)
            )
        {
            return;
        }

        // Get the tie strength data.

        Range oTieStrengthData;
        Object [,] aoTieStrengthValues;

        if ( !ExcelUtil.TryGetTableColumnDataAndValues(oEdgeTable,
                EdgeTableColumnNames.TieStrength, out oTieStrengthData,
                out aoTieStrengthValues) )
        {
            return;
        }

        // Determine whether the graph is directed.

        PerWorkbookSettings oPerWorkbookSettings =
            new PerWorkbookSettings(workbook);

        Boolean bGraphIsDirected = (oPerWorkbookSettings.GraphDirectedness ==
            GraphDirectedness.Directed);

        // Now that the required information has been gathered, do the actual
        // merge.

        MergeDuplicateEdges(oEdgeTable, oVertex1NameData, aoVertex1NameValues,
            oVertex2NameData, aoVertex2NameValues, oTieStrengthColumn,
            oTieStrengthData, aoTieStrengthValues, bGraphIsDirected);

        oEdgeTable.HeaderRowRange.Select();
    }

    //*************************************************************************
    //  Method: MergeDuplicateEdges()
    //
    /// <summary>
    /// Merges duplicate edges in the edge worksheet given information about
    /// the edges.
    /// </summary>
    ///
    /// <param name="oEdgeTable">
    /// Edge table.
    /// </param>
    ///
    /// <param name="oVertex1NameData">
    /// Data range for the vertex 1 name column.
    /// </param>
    ///
    /// <param name="aoVertex1NameValues">
    /// Data values from the vertex 1 name column.
    /// </param>
    ///
    /// <param name="oVertex2NameData">
    /// Data range for the vertex 2 name column.
    /// </param>
    ///
    /// <param name="aoVertex2NameValues">
    /// Data values from the vertex 2 name column.
    /// </param>
    ///
    /// <param name="oTieStrengthColumn">
    /// Tie strength column.
    /// </param>
    ///
    /// <param name="oTieStrengthData">
    /// Data range for the tie strength column.
    /// </param>
    ///
    /// <param name="aoTieStrengthValues">
    /// Data values from the tie strength column.
    /// </param>
    ///
    /// <param name="bGraphIsDirected">
    /// true if the graph is directed, false if it is undirected.
    /// </param>
    //*************************************************************************

    protected void
    MergeDuplicateEdges
    (
        ListObject oEdgeTable,
        Range oVertex1NameData,
        Object [,] aoVertex1NameValues,
        Range oVertex2NameData,
        Object [,] aoVertex2NameValues,
        ListColumn oTieStrengthColumn,
        Range oTieStrengthData,
        Object [,] aoTieStrengthValues,
        Boolean bGraphIsDirected
    )
    {
        Debug.Assert(oEdgeTable != null);
        Debug.Assert(oVertex1NameData != null);
        Debug.Assert(aoVertex1NameValues != null);
        Debug.Assert(oVertex2NameData != null);
        Debug.Assert(aoVertex2NameValues != null);
        Debug.Assert(oTieStrengthColumn != null);
        Debug.Assert(oTieStrengthData != null);
        Debug.Assert(aoTieStrengthValues != null);
        AssertValid();

        // This dictionary contains one key/value pair for each unique edge.
        // The key is a vertex name pair and the value is the one-based table
        // row number of the first instance of the edge.  If the edge worksheet
        // contains these rows, for example:
        //
        // 1  X,Y
        // 2  A,B
        // 3  A,B
        // 4  C,D
        // 5  A,B
        // 6  A,B
        //
        // then the dictionary will have three entries: one for X,Y, one for
        // A,B, and one for C,D.  The entry for X,Y will have a value of 1 (the
        // one-based table row number of the first instance of X,Y), the entry
        // for A,B will have a value of 2, and the entry for C,D will have a
        // value of 4.

        Dictionary<String, Int32> oUniqueEdges =
            new Dictionary<String, Int32>();

        // Loop through the table rows.

        Int32 iRows = oVertex1NameData.Rows.Count;

        for (Int32 iRowOneBased = 1; iRowOneBased <= iRows; iRowOneBased++)
        {
            String sVertex1Name, sVertex2Name;

            if (
                !ExcelUtil.TryGetNonEmptyStringFromCell(aoVertex1NameValues,
                    iRowOneBased, 1, out sVertex1Name)
                ||
                !ExcelUtil.TryGetNonEmptyStringFromCell(aoVertex2NameValues,
                    iRowOneBased, 1, out sVertex2Name)
                )
            {
                continue;
            }

            // Does the row already have a tie strength?

            Int32 iInitialTieStrength = 1;

            Object oInitialTieStrength = aoTieStrengthValues[iRowOneBased, 1];

            if (oInitialTieStrength != null && oInitialTieStrength is Double)
            {
                // Yes.

                iInitialTieStrength = (Int32)(Double)oInitialTieStrength;
            }
            else
            {
                // No.  Set the initial tie strength.

                aoTieStrengthValues[iRowOneBased, 1] = 1.0;
            }

            // Has an instance of this edge already been found?

            String sVertexNamePair = DuplicateEdgeDetector.GetVertexNamePair(
                sVertex1Name, sVertex2Name, bGraphIsDirected);

            Int32 iFirstInstanceRowOneBased;

            if ( oUniqueEdges.TryGetValue(
                sVertexNamePair, out iFirstInstanceRowOneBased) )
            {
                // Yes.  This row will need to be deleted.  Mark it by nulling
                // its tie strength.

                aoTieStrengthValues[iRowOneBased, 1] = null;

                // Update the tie strength for the row with the edge's first
                // instance.

                Debug.Assert(aoTieStrengthValues[iFirstInstanceRowOneBased, 1]
                    is Double);

                aoTieStrengthValues[iFirstInstanceRowOneBased, 1] = 
                    (Double)aoTieStrengthValues[iFirstInstanceRowOneBased, 1]
                    + (Double)iInitialTieStrength;
            }
            else
            {
                // No.  Add a dictionary entry.

                oUniqueEdges.Add(sVertexNamePair, iRowOneBased);
            }
        }

        // Save the updated tie strengths to the table.

        oTieStrengthData.set_Value(Missing.Value, aoTieStrengthValues);

        // Delete the duplicate rows.

        DeleteDuplicateRows(oEdgeTable, oTieStrengthColumn, oTieStrengthData,
            aoTieStrengthValues);
    }

    //*************************************************************************
    //  Method: DeleteDuplicateRows()
    //
    /// <summary>
    /// Deletes the duplicate rows from the edge table.
    /// </summary>
    ///
    /// <param name="oEdgeTable">
    /// Edge table.
    /// </param>
    ///
    /// <param name="oTieStrengthColumn">
    /// Tie strength column.
    /// </param>
    ///
    /// <param name="oTieStrengthData">
    /// Data range for the tie strength column.
    /// </param>
    ///
    /// <param name="aoTieStrengthValues">
    /// Data values from the tie strength column.
    /// </param>
    ///
    /// <remarks>
    /// All rows for which the tie strength cell is null are deleted.
    /// </remarks>
    //*************************************************************************

    protected void
    DeleteDuplicateRows
    (
        ListObject oEdgeTable,
        ListColumn oTieStrengthColumn,
        Range oTieStrengthData,
        Object [,] aoTieStrengthValues
    )
    {
        Debug.Assert(oEdgeTable != null);
        Debug.Assert(oTieStrengthColumn != null);
        Debug.Assert(oTieStrengthData != null);
        Debug.Assert(aoTieStrengthValues != null);
        AssertValid();

        Range oDuplicateRows = null;

        // Find the rows with null tie strengths, which are the duplicates.  To
        // avoid multiple areas, which can slow things down signficantly, sort
        // the table on the tie strength column.  That forces the duplicates
        // to be contiguous.
        //
        // But first, add a temporary column and set its values to the
        // worksheet row numbers.  This will be used later to restore the
        // original sort order.

        ListColumn oTemporaryColumn;

        if ( !ExcelUtil.TryAddTableColumnWithRowNumbers(oEdgeTable,
            "Temporary for Sort", 5F, null, out oTemporaryColumn) )
        {
            return;
        }

        Sort oSort = oEdgeTable.Sort;
        SortFields oSortFields = oSort.SortFields;
        oSortFields.Clear();

        oSortFields.Add(oTieStrengthColumn.Range, XlSortOn.xlSortOnValues,
            XlSortOrder.xlAscending, Missing.Value,
            XlSortDataOption.xlSortNormal);

        oSort.Apply();

        if (oTieStrengthData.Rows.Count != 1)
        {
            try
            {
                oDuplicateRows = oTieStrengthData.SpecialCells(
                    XlCellType.xlCellTypeBlanks, Missing.Value);
            }
            catch (COMException)
            {
                // There are no such rows.

                oDuplicateRows = null;
            }
        }
        else
        {
            // Range.SpecialCells() can't be used in the one-cell case, for
            // which it behaves in a bizarre manner.  See this posting:
            //
            // http://ewbi.blogs.com/develops/2006/03/determine_if_a_.html
            //
            // ...of which this is an excerpt:
            //
            // "SpecialCells ignores any source Range consisting of only one
            // cell. When executing SpecialCells on a Range having only one
            // cell, it will instead consider all of the cells falling within
            // the boundary marked by the bottom right cell of the source Range
            // sheet's UsedRange."
            //
            // Instead, just check the single row.

            if (aoTieStrengthValues[1, 1] == null)
            {
                oDuplicateRows = oTieStrengthData.EntireRow;
            }
        }

        if (oDuplicateRows != null)
        {
            // Delete the duplicate rows, which are contiguous.

            Debug.Assert(oDuplicateRows.Areas.Count == 1);

            oDuplicateRows.EntireRow.Delete(XlDeleteShiftDirection.xlShiftUp);
        }

        // Restore the original sort order by sorting on the temporary column
        // that contains the original worksheet row numbers.

        Debug.Assert(oTemporaryColumn != null);

        oSortFields.Clear();

        oSortFields.Add(oTemporaryColumn.Range, XlSortOn.xlSortOnValues,
            XlSortOrder.xlAscending, Missing.Value,
            XlSortDataOption.xlSortNormal);

        oSort.Apply();
        oSortFields.Clear();

        oTemporaryColumn.Delete();

        oSort.Apply();
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
