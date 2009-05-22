
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.NodeXL.Core;

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
/// Use <see cref="ExportSelectionToNewNodeXLWorkbook" /> to export the
/// selected rows of the edge and vertex tables to a new NodeXL workbook.
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
    /// <param name="workbookToExport">
    /// The NodeXL workbook to export.
    /// </param>
    //*************************************************************************

    public WorkbookExporter
    (
        Microsoft.Office.Interop.Excel.Workbook workbookToExport
    )
    {
        m_oWorkbookToExport = workbookToExport;

        AssertValid();
    }

    //*************************************************************************
    //  Method: ExportSelectionToNewNodeXLWorkbook()
    //
    /// <summary>
    /// Exports the selected rows of the edge and vertex tables to a new NodeXL
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
    /// an error occurs while exporting the selected rows, an <see
    /// cref="ExportWorkbookException" /> exception is thrown.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Microsoft.Office.Interop.Excel.Workbook
    ExportSelectionToNewNodeXLWorkbook()
    {
        AssertValid();

        Application oApplication = m_oWorkbookToExport.Application;
        Workbook oNewNodeXLWorkbook = null;

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
            ExcelUtil.TryGetVisibleSelectedTableRange(m_oWorkbookToExport,
                WorksheetNames.Edges, TableNames.Edges, out oTable,
                out oVisibleSelectedTableRange)
            &&
            !ExcelUtil.TableIsEmpty(oTable)
            )
        {
            // Create the new workbook and copy the edge table's selected rows
            // to it.

            oNewNodeXLWorkbook = oApplication.Workbooks.Add(sTemplatePath);

            CopyRowsToNewNodeXLWorkbook(oTable, oVisibleSelectedTableRange,
                oNewNodeXLWorkbook);
        }

        // Get the selected range within the vertex table.

        if (
            ExcelUtil.TryGetVisibleSelectedTableRange(m_oWorkbookToExport,
                WorksheetNames.Vertices, TableNames.Vertices, out oTable,
                out oVisibleSelectedTableRange)
            &&
            !ExcelUtil.TableIsEmpty(oTable)
            )
        {
            // Create the new workbook if necessary and copy the vertex table's
            // selected rows to it.

            if (oNewNodeXLWorkbook == null)
            {
                oNewNodeXLWorkbook = oApplication.Workbooks.Add(sTemplatePath);
            }

            CopyRowsToNewNodeXLWorkbook(oTable, oVisibleSelectedTableRange,
                oNewNodeXLWorkbook);
        }

        if (oNewNodeXLWorkbook == null)
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
                ExcelUtil.TryGetTable(m_oWorkbookToExport,
                    WorksheetNames.Images, TableNames.Images, out oTable)
                &&
                ExcelUtil.TryGetVisibleTableRange(oTable,
                    out oVisibleTableRange)
                )
            {
                CopyRowsToNewNodeXLWorkbook(oTable, oVisibleTableRange,
                    oNewNodeXLWorkbook);
            }
        }

        return (oNewNodeXLWorkbook);
    }

    //*************************************************************************
    //  Method: ExportToNewMatrixWorkbook()
    //
    /// <summary>
    /// Exports the edge table to a new workbook as an adjacency matrix.
    /// </summary>
    ///
    /// <returns>
    /// The new workbook.
    /// </returns>
    ///
    /// <remarks>
    /// If there are no rows in the edge table, or if an error occurs while
    /// exporting the rows, an <see cref="ExportWorkbookException" /> exception
    /// is thrown.
    /// </remarks>
    //*************************************************************************

    public Microsoft.Office.Interop.Excel.Workbook
    ExportToNewMatrixWorkbook()
    {
        AssertValid();

        // Merge duplicate edges and add an edge weight column.

        ( new DuplicateEdgeMerger() ).MergeDuplicateEdges(m_oWorkbookToExport);

        // Read the workbook, including the edge weight column.

        ReadWorkbookContext oReadWorkbookContext = new ReadWorkbookContext();
        oReadWorkbookContext.SetEdgeWeightValues = true;

        IGraph oGraph = ( new WorkbookReader() ).ReadWorkbook(
            m_oWorkbookToExport, oReadWorkbookContext);

        // Get an array of non-isolated vertices.  Isolated vertices don't get
        // exported, at least for now.  (How are isolated vertices typically
        // handled when working with adjacency matrices?)

        List<IVertex> oNonIsolatedVertices = new List<IVertex>();

        foreach (IVertex oVertex in oGraph.Vertices)
        {
            if (oVertex.IncidentEdges.Length > 0)
            {
                oNonIsolatedVertices.Add(oVertex);
            }
        }

        Int32 iNonIsolatedVertices = oNonIsolatedVertices.Count;

        if (iNonIsolatedVertices == 0)
        {
            throw new ExportWorkbookException(
                "There are no edges to export to a new workbook."
                );
        }

        Workbook oNewWorkbook =
            m_oWorkbookToExport.Application.Workbooks.Add(Missing.Value);

        Worksheet oNewWorksheet = (Worksheet)oNewWorkbook.ActiveSheet;

        // Fill in row 1 and column A with the vertex names, starting at B1 and
        // A2, respectively.

        String [,] asVertexNamesForRow1 = ExcelUtil.GetSingleRow2DStringArray(
            iNonIsolatedVertices);

        String [,] asVertexNamesForColumnA =
            ExcelUtil.GetSingleColumn2DStringArray(iNonIsolatedVertices);

        for (Int32 i = 0; i < iNonIsolatedVertices; i++)
        {
            asVertexNamesForRow1[1, i + 1] = asVertexNamesForColumnA[i + 1, 1]
                = oNonIsolatedVertices[i].Name;
        }

        ExcelUtil.SetRangeValues( (Range)oNewWorksheet.Cells[1, 2],
            asVertexNamesForRow1 );

        ExcelUtil.SetRangeValues( (Range)oNewWorksheet.Cells[2, 1],
            asVertexNamesForColumnA );

        asVertexNamesForRow1 = asVertexNamesForColumnA = null;

        // Now fill in the edge weights, row by row.

        Range oFirstColumnCell = (Range)oNewWorksheet.Cells[2, 2];

        for (Int32 i = 0; i < iNonIsolatedVertices; i++)
        {
            Object [,] aoEdgeWeights = ExcelUtil.GetSingleRow2DArray(
                iNonIsolatedVertices);

            IVertex oVertexI = oNonIsolatedVertices[i];

            for (Int32 j = 0; j < iNonIsolatedVertices; j++)
            {
                aoEdgeWeights[1, j + 1] = GetEdgeWeight( oVertexI,
                    oNonIsolatedVertices[j] );
            }

            ExcelUtil.SetRangeValues(oFirstColumnCell, aoEdgeWeights);

            oFirstColumnCell = oFirstColumnCell.get_Offset(1, 0);
        }

        return (oNewWorkbook);
    }

    //*************************************************************************
    //  Method: GetEdgeWeight()
    //
    /// <summary>
    /// Gets the edge weight between two vertices.
    /// </summary>
    ///
    /// <param name="oVertex1">
    /// The first vertex.
    /// </param>
    ///
    /// <param name="oVertex2">
    /// The second vertex.
    /// </param>
    ///
    /// <returns>
    /// The edge weight between the two vertices.
    /// </returns>
    ///
    /// <remarks>
    /// It's assumed that DuplicateEdgeMerger has been used to merge duplicate
    /// edges and set the edge weight value on all the edges.
    /// </remarks>
    //*************************************************************************

    protected Double
    GetEdgeWeight
    (
        IVertex oVertex1,
        IVertex oVertex2
    )
    {
        Debug.Assert(oVertex1 != null);
        Debug.Assert(oVertex2 != null);
        AssertValid();

        Double dEdgeWeight = 0;

        // Get the edges that connect the two vertices.  This includes all
        // connecting edges and does not take directedness into account.

        IEdge [] aoConnectingEdges = oVertex1.GetConnectingEdges(oVertex2);
        Int32 iConnectingEdges = aoConnectingEdges.Length;
        IEdge oConnectingEdgeWithEdgeWeight = null;

        switch (oVertex1.ParentGraph.Directedness)
        {
            case GraphDirectedness.Directed:

                // There can be 0, 1, or 2 edges between the vertices.  Only
                // one of them can originate at oVertex1.

                Debug.Assert(iConnectingEdges <= 2);

                foreach (IEdge oConnectingEdge in aoConnectingEdges)
                {
                    if (oConnectingEdge.BackVertex == oVertex1)
                    {
                        oConnectingEdgeWithEdgeWeight = oConnectingEdge;
                        break;
                    }
                }

                break;

            case GraphDirectedness.Undirected:

                // There can be 0 or 1 edges between the vertices.  There can't
                // be 2 edges, because DuplicateEdgeMerger combined (A,B) with
                // (B,A).

                Debug.Assert(iConnectingEdges <= 1);

                if (iConnectingEdges == 1)
                {
                    oConnectingEdgeWithEdgeWeight = aoConnectingEdges[0];
                }

                break;

            default:

                Debug.Assert(false);
                break;
        }

        if (oConnectingEdgeWithEdgeWeight != null)
        {
            dEdgeWeight = (Double)
                oConnectingEdgeWithEdgeWeight.GetRequiredValue(
                    ReservedMetadataKeys.EdgeWeight, typeof(Double) );
        }

        return (dEdgeWeight);
    }

    //*************************************************************************
    //  Method: CopyRowsToNewNodeXLWorkbook()
    //
    /// <summary>
    /// Copies specified rows in a table to a new NodeXL workbook.
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
    /// <param name="oNewNodeXLWorkbook">
    /// The new NodeXL workbook to copy the rows to.
    /// </param>
    //*************************************************************************

    protected void
    CopyRowsToNewNodeXLWorkbook
    (
        ListObject oSourceTable,
        Range oSourceTableRangeToCopy,
        Workbook oNewNodeXLWorkbook
    )
    {
        Debug.Assert(oSourceTable != null);
        Debug.Assert(oSourceTableRangeToCopy != null);
        Debug.Assert(oNewNodeXLWorkbook != null);
        AssertValid();

        String sTableName = oSourceTable.Name;

        Debug.Assert(oSourceTable.Parent is Worksheet);

        String sWorksheetName = ( (Worksheet)oSourceTable.Parent ).Name;

        ListObject oNewTable;

        if ( !ExcelUtil.TryGetTable(oNewNodeXLWorkbook, sWorksheetName,
            sTableName, out oNewTable) )
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
    /// Adds a column to a new NodeXL table if the column doesn't already
    /// exist.
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
        Debug.Assert(m_oWorkbookToExport != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// NodeXL workbook to export.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbookToExport;
}

}
