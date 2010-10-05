
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: WorkbookReader
//
/// <summary>
/// Contains methods for creating a NodeXL graph from the contents of an Excel
/// workbook.
/// </summary>
///
/// <remarks>
/// Call <see cref="ReadWorkbook" /> to create a NodeXL graph from the contents
/// of an Excel workbook.
/// </remarks>
//*****************************************************************************

public class WorkbookReader : Object
{
    //*************************************************************************
    //  Constructor: WorkbookReader()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="WorkbookReader" /> class.
    /// </summary>
    //*************************************************************************

    public WorkbookReader()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: ReadWorkbook()
    //
    /// <summary>
    /// Creates a NodeXL graph from the contents of an Excel workbook.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <param name="readWorkbookContext">
    /// Provides access to objects needed for converting an Excel workbook to a
    /// NodeXL graph.
    /// </param>
    ///
    /// <returns>
    /// A new graph.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="workbook" /> contains valid graph data, a new <see
    /// cref="IGraph" /> is created from the workbook contents and returned.
    /// Otherwise, a <see cref="WorkbookFormatException" /> is thrown.
    /// </remarks>
    //*************************************************************************

    public IGraph
    ReadWorkbook
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        ReadWorkbookContext readWorkbookContext
    )
    {
        Debug.Assert(readWorkbookContext != null);
        Debug.Assert(workbook != null);
        AssertValid();

        IGraph oGraph = null;

        // Turn off screen updating, for two reasons:
        //
        // 1. Reading the workbook can involve writing to edge and vertex ID
        //    columns, which can be slow when updating is turned on.
        //
        // 2. Any hidden columns get temporarily shown as each worksheet is
        //    read.

        Application oApplication = workbook.Application;
        Boolean bOldScreenUpdating = oApplication.ScreenUpdating;
        oApplication.ScreenUpdating = false;

        try
        {
            oGraph = ReadWorkbookInternal(workbook, readWorkbookContext);
        }
        catch (WorkbookFormatException oWorkbookFormatException)
        {
            Range oRangeToSelect = oWorkbookFormatException.RangeToSelect;

            if (oRangeToSelect != null)
            {
                // The user may have hidden the column group containing the
                // error.  Make sure the column is visible.

                oRangeToSelect.EntireColumn.Hidden = false;
            }

            throw oWorkbookFormatException;
        }
        finally
        {
            oApplication.ScreenUpdating = bOldScreenUpdating;
        }

        return (oGraph);
    }

    //*************************************************************************
    //  Method: ReadWorkbookInternal()
    //
    /// <summary>
    /// Creates a NodeXL graph from the contents of an Excel workbook.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <param name="readWorkbookContext">
    /// Provides access to objects needed for converting an Excel workbook to a
    /// NodeXL graph.
    /// </param>
    ///
    /// <returns>
    /// A new graph.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="workbook" /> contains valid graph data, a new <see
    /// cref="IGraph" /> is created from the workbook contents and returned.
    /// Otherwise, a <see cref="WorkbookFormatException" /> is thrown.
    /// </remarks>
    //*************************************************************************

    protected IGraph
    ReadWorkbookInternal
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        ReadWorkbookContext readWorkbookContext
    )
    {
        Debug.Assert(readWorkbookContext != null);
        Debug.Assert(workbook != null);
        AssertValid();

        if (readWorkbookContext.PopulateVertexWorksheet)
        {
            // Create and use the object that fills in the vertex worksheet.

            VertexWorksheetPopulator oVertexWorksheetPopulator =
                new VertexWorksheetPopulator();

            try
            {
                oVertexWorksheetPopulator.PopulateVertexWorksheet(
                    workbook, false);
            }
            catch (WorkbookFormatException)
            {
                // Ignore this type of error, which occurs when the vertex
                // worksheet is missing, for example.
            }
        }

        // Create a graph with the appropriate directedness.

        PerWorkbookSettings oPerWorkbookSettings =
            new PerWorkbookSettings(workbook);

        IGraph oGraph = new Graph(oPerWorkbookSettings.GraphDirectedness);

        // Read the edge worksheet.  This adds data to oGraph, 
        // ReadWorkbookContext.VertexNameDictionary, and
        // ReadWorkbookContext.EdgeIDDictionary.

        EdgeWorksheetReader oEdgeWorksheetReader = new EdgeWorksheetReader();

        oEdgeWorksheetReader.ReadWorksheet(workbook, readWorkbookContext,
            oGraph);

        oEdgeWorksheetReader = null;

        // Read the vertex worksheet.  This adds metadata to the vertices in
        // oGraph; adds any isolated vertices to oGraph and
        // ReadWorkbookContext.VertexNameDictionary; and removes any skipped
        // vertices (and their incident edges) from
        // ReadWorkbookContext.VertexNameDictionary,
        // ReadWorkbookContext.EdgeIDDictionary, and oGraph.

        VertexWorksheetReader oVertexWorksheetReader =
            new VertexWorksheetReader();

        oVertexWorksheetReader.ReadWorksheet(workbook, readWorkbookContext,
            oGraph);

        oVertexWorksheetReader = null;

        if (readWorkbookContext.ReadAllEdgeAndVertexColumns)
        {
            // The other worksheets should be ignored.

            return (oGraph);
        }

        if (readWorkbookContext.ReadGroups)
        {
            // Read the group worksheets.  This adds metadata to the vertices
            // in oGraph and to oGraph itself.

            GroupWorksheetReader oGroupWorksheetReader =
                new GroupWorksheetReader();

            oGroupWorksheetReader.ReadWorksheet(workbook,
                readWorkbookContext, oGraph);

            oGroupWorksheetReader = null;
        }

        // Read the per-workbook settings that are stored directly on the
        // graph.

        oPerWorkbookSettings.ReadWorksheet(workbook, readWorkbookContext,
            oGraph);

        return (oGraph);
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
