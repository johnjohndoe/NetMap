
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: WorkbookImporterBase
//
/// <summary>
/// Base class for classes that import a graph from another open workbook.
/// </summary>
//*****************************************************************************

public class WorkbookImporterBase : Object
{
    //*************************************************************************
    //  Constructor: WorkbookImporterBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="WorkbookImporterBase" />
    /// class.
    /// </summary>
    //*************************************************************************

    public WorkbookImporterBase()
    {
        // (Do nothing.)

        // AssertValid();
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
    /// Workbook.Name of the open workbook that contains the graph to import.
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

        Workbook oSourceWorkbook = oApplication.Workbooks[sSourceWorkbookName];
        Object oSourceWorksheetAsObject = oSourceWorkbook.ActiveSheet;

        Debug.Assert(oSourceWorksheetAsObject != null);

        if ( !(oSourceWorksheetAsObject is Worksheet) )
        {
            OnInvalidSourceWorkbook( String.Format(
                SourceWorkbookSheetIsNotWorksheetMessage
                ,
                sSourceWorkbookName
                ) );
        }

        return ( (Worksheet)oSourceWorksheetAsObject );
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
    //  Method: GetVertexColumnData()
    //
    /// <summary>
    /// Gets the data range of one of the vertex columns of the edge table in
    /// the destination NodeXL workbook.
    /// </summary>
    ///
    /// <param name="oDestinationEdgeTable">
    /// Edge table in the destination NodeXL workbook.
    /// </param>
    ///
    /// <param name="bGetVertex1">
    /// true to get the data range of the vertex 1 column, false for the
    /// vertex 2 column.
    /// </param>
    //*************************************************************************

    protected Range
    GetVertexColumnData
    (
        ListObject oDestinationEdgeTable,
        Boolean bGetVertex1
    )
    {
        Debug.Assert(oDestinationEdgeTable != null);
        AssertValid();

        Range oVertexColumnData;

        if (
            !ExcelUtil.TryGetTableColumnData(oDestinationEdgeTable,

                bGetVertex1 ? EdgeTableColumnNames.Vertex1Name :
                    EdgeTableColumnNames.Vertex2Name ,

                out oVertexColumnData)
            )
        {
            OnInvalidSourceWorkbook(
                "One of the vertex columns is missing from the NodeXL"
                + " workbook."
                );
        }

        return (oVertexColumnData);
    }

    //*************************************************************************
    //  Method: OnInvalidSourceWorkbook()
    //
    /// <overloads>
    /// Throws an exception when a source workbook is found to contain invalid
    /// graph data.
    /// </overloads>
    ///
    /// <summary>
    /// Throws an exception when a source workbook is found to contain invalid
    /// graph data, and selects a specified cell in the source workbook.
    /// </summary>
    ///
    /// <param name="sMessage">
    /// Error message, suitable for displaying to the user.
    /// </param>
    ///
    /// <param name="oSourceWorksheet">
    /// Source worksheet containing invalid graph data.
    /// </param>
    ///
    /// <param name="iRowOneBased">
    /// One-based row number of the source workbook cell to select.
    /// </param>
    ///
    /// <param name="iColumnOneBased">
    /// One-based column number of the source workbook cell to select.
    /// </param>
    //*************************************************************************

    protected void
    OnInvalidSourceWorkbook
    (
        String sMessage,
        Worksheet oSourceWorksheet,
        Int32 iRowOneBased,
        Int32 iColumnOneBased
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sMessage) );
        Debug.Assert(iRowOneBased >= 1);
        Debug.Assert(iColumnOneBased >= 1);
        AssertValid();

        ExcelUtil.SelectRange(
            (Range)oSourceWorksheet.Cells[iRowOneBased, iColumnOneBased] );

        OnInvalidSourceWorkbook(sMessage);
    }

    //*************************************************************************
    //  Method: OnInvalidSourceWorkbook()
    //
    /// <summary>
    /// Throws an exception when a source workbook is found to contain invalid
    /// graph data.
    /// </summary>
    ///
    /// <param name="sMessage">
    /// Error message, suitable for displaying to the user.
    /// </param>
    //*************************************************************************

    protected void
    OnInvalidSourceWorkbook
    (
        String sMessage
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sMessage) );
        AssertValid();

        throw new ImportWorkbookException(sMessage);
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
    //  Public constants
    //*************************************************************************

    /// Message to display when a non-worksheet is active in the source
    /// workbook.  The message must be formatted with String.Format().

    public const String SourceWorkbookSheetIsNotWorksheetMessage =

        "The selected sheet in {0} is not a worksheet.  A worksheet must be"
        + " selected in {0}."
        ;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
