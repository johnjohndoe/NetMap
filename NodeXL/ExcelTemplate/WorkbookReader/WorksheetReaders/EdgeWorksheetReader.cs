
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Algorithms;
using Microsoft.NodeXL.Visualization.Wpf;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: EdgeWorksheetReader
//
/// <summary>
/// Class that knows how to read an Excel worksheet containing edge data.
/// </summary>
///
/// <remarks>
/// Call <see cref="ReadWorksheet" /> to read an edge worksheet.
/// </remarks>
//*****************************************************************************

public class EdgeWorksheetReader : WorksheetReaderBase
{
    //*************************************************************************
    //  Constructor: EdgeWorksheetReader()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EdgeWorksheetReader" />
    /// class.
    /// </summary>
    //*************************************************************************

    public EdgeWorksheetReader()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Enum: Visibility
    //
    /// <summary>
    /// Specifies the visibility of an edge.
    /// </summary>
    //*************************************************************************

    public enum
    Visibility
    {
        /// <summary>
        /// Read the edge into the graph and show it.  This is the default.
        /// </summary>

        Show,

        /// <summary>
        /// Skip the edge row.  Do not read it into the graph.
        /// </summary>

        Skip,

        /// <summary>
        /// Read the edge into the graph but hide it.
        /// </summary>

        Hide,
    }

    //*************************************************************************
    //  Method: ReadWorksheet()
    //
    /// <summary>
    /// Reads the edge worksheet and adds the contents to a graph.
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
    /// <param name="graph">
    /// Graph to add edge data to.
    /// </param>
    ///
    /// <remarks>
    /// If the edge worksheet in <paramref name="workbook" /> contains valid
    /// edge data, the data is added to <paramref name="graph" />, <paramref
    /// name="readWorkbookContext" />.VertexNameDictionary, and <paramref
    /// name="readWorkbookContext" />.EdgeIDDictionary.  Otherwise, a <see
    /// cref="WorkbookFormatException" /> is thrown.
    /// </remarks>
    //*************************************************************************

    public void
    ReadWorksheet
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        ReadWorkbookContext readWorkbookContext,
        IGraph graph
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert(readWorkbookContext != null);
        Debug.Assert(graph != null);
        AssertValid();

        // Get the required table that contains edge data, then add the edges
        // and vertices in the table to the graph.

        ListObject oEdgeTable = GetEdgeTable(workbook);

        // The code that reads the table can handle hidden rows, but not hidden
        // columns.  Temporarily show all hidden columns in the table.

        ExcelHiddenColumns oHiddenColumns =
            ExcelColumnHider.ShowHiddenColumns(oEdgeTable);

        try
        {
            ReadEdgeTable(oEdgeTable, readWorkbookContext, graph);
        }
        finally
        {
            ExcelColumnHider.RestoreHiddenColumns(oEdgeTable, oHiddenColumns);
        }
    }

    //*************************************************************************
    //  Method: GetEdgeTable()
    //
    /// <summary>
    /// Gets the required table that contains the edge data.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <returns>
    /// The table that contains the edge data.
    /// </returns>
    ///
    /// <remarks>
    /// If the table can't be returned, a <see
    /// cref="WorkbookFormatException" /> is thrown.
    /// </remarks>
    //*************************************************************************

    public ListObject
    GetEdgeTable
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        Debug.Assert(workbook != null);
        AssertValid();

        // Get the worksheet that contains edge data.

        Worksheet oEdgeWorksheet;

        if ( !ExcelUtil.TryGetWorksheet(workbook, WorksheetNames.Edges,
            out oEdgeWorksheet) )
        {
            OnWorkbookFormatError( String.Format(

                "The workbook must contain a worksheet named \"{0}\" that"
                + " contains edge data.\r\n\r\n{1}"
                ,
                WorksheetNames.Edges,
                ErrorUtil.GetTemplateMessage()
                ) );
        }

        // Get the table (ListObject) that contains edge data.

        ListObject oEdgeTable;

        if ( !ExcelUtil.TryGetTable(oEdgeWorksheet, TableNames.Edges,
            out oEdgeTable) )
        {
            OnWorkbookFormatError( String.Format(

                "The worksheet named \"{0}\" must have a table named \"{1}\""
                + " that contains edge data.\r\n\r\n{2}"
                ,
                WorksheetNames.Edges,
                TableNames.Edges,
                ErrorUtil.GetTemplateMessage()
                ) );
        }

        // Make sure the vertex name columns exist.

        GetTableColumnIndex(oEdgeTable, EdgeTableColumnNames.Vertex1Name,
            true);

        GetTableColumnIndex(oEdgeTable, EdgeTableColumnNames.Vertex2Name,
            true);

        return (oEdgeTable);
    }

    //*************************************************************************
    //  Method: ReadEdgeTable()
    //
    /// <summary>
    /// Reads the edge table and adds the contents to a graph.
    /// </summary>
    ///
    /// <param name="oEdgeTable">
    /// Table that contains the edge data.
    /// </param>
    ///
    /// <param name="oReadWorkbookContext">
    /// Provides access to objects needed for converting an Excel workbook to a
    /// NodeXL graph.
    /// </param>
    ///
    /// <param name="oGraph">
    /// Graph to add edges to.
    /// </param>
    //*************************************************************************

    protected void
    ReadEdgeTable
    (
        ListObject oEdgeTable,
        ReadWorkbookContext oReadWorkbookContext,
        IGraph oGraph
    )
    {
        Debug.Assert(oEdgeTable != null);
        Debug.Assert(oReadWorkbookContext != null);
        Debug.Assert(oGraph != null);
        AssertValid();

        Boolean bReadAllEdgeAndVertexColumns =
            oReadWorkbookContext.ReadAllEdgeAndVertexColumns;

        if (oReadWorkbookContext.FillIDColumns)
        {
            FillIDColumn(oEdgeTable);
        }

        Dictionary<String, IVertex> oVertexNameDictionary =
            oReadWorkbookContext.VertexNameDictionary;

        EdgeVisibilityConverter oEdgeVisibilityConverter =
            new EdgeVisibilityConverter();

        Boolean bGraphIsDirected =
            (oGraph.Directedness == GraphDirectedness.Directed);

        ExcelTableReader oExcelTableReader = new ExcelTableReader(oEdgeTable);
        IVertexCollection oVertices = oGraph.Vertices;
        IEdgeCollection oEdges = oGraph.Edges;

        HashSet<String> oColumnNamesToExclude = new HashSet<String>(
            new String[] {
                EdgeTableColumnNames.Vertex1Name,
                EdgeTableColumnNames.Vertex2Name
                } );

        foreach ( ExcelTableReader.ExcelTableRow oRow in
            oExcelTableReader.GetRows() )
        {
            // Get the names of the edge's vertices.

            String sVertex1Name, sVertex2Name;

            Boolean bVertex1IsEmpty = !oRow.TryGetNonEmptyStringFromCell(
                EdgeTableColumnNames.Vertex1Name, out sVertex1Name);

            Boolean bVertex2IsEmpty = !oRow.TryGetNonEmptyStringFromCell(
                EdgeTableColumnNames.Vertex2Name, out sVertex2Name);

            if (bVertex1IsEmpty && bVertex2IsEmpty)
            {
                // Skip empty rows.

                continue;
            }

            if (bVertex1IsEmpty || bVertex2IsEmpty)
            {
                // A half-empty row is an error.

                OnHalfEmptyEdgeRow(oRow, bVertex1IsEmpty);
            }

            // Assume a default visibility.

            Visibility eVisibility = Visibility.Show;

            String sVisibility;

            if (
                oRow.TryGetNonEmptyStringFromCell(
                    CommonTableColumnNames.Visibility, out sVisibility)
                &&
                !oEdgeVisibilityConverter.TryWorkbookToGraph(
                    sVisibility, out eVisibility)
                )
            {
                OnInvalidVisibility(oRow);
            }

            if (eVisibility == Visibility.Skip)
            {
                // Skip the edge an continue to the next edge.

                continue;
            }

            // Create the specified vertices or retrieve them from the
            // dictionary.

            IVertex oVertex1 = VertexNameToVertex(
                sVertex1Name, oVertices, oVertexNameDictionary);

            IVertex oVertex2 = VertexNameToVertex(
                sVertex2Name, oVertices, oVertexNameDictionary);

            // Add an edge connecting the vertices.

            IEdge oEdge = oEdges.Add(oVertex1, oVertex2, bGraphIsDirected);

            // If there is an ID column, add the edge to the edge row ID
            // dictionary and set the edge's Tag to the row ID.

            AddToRowIDDictionary(oRow, oEdge,
                oReadWorkbookContext.EdgeRowIDDictionary);

            if (bReadAllEdgeAndVertexColumns)
            {
                // All columns except the vertex names should be read and
                // stored as metadata on the edge.

                ReadAllColumns(oExcelTableReader, oRow, oEdge,
                    oColumnNamesToExclude);

                continue;
            }

            if (eVisibility == Visibility.Hide)
            {
                // Hide the edge and continue to the next edge.

                oEdge.SetValue(ReservedMetadataKeys.Visibility,
                    VisibilityKeyValue.Hidden);

                continue;
            }

            // Alpha.

            Boolean bAlphaIsZero = ReadAlpha(oRow, oEdge);

            if (bAlphaIsZero)
            {
                continue;
            }

            // Color.

            ReadColor(oRow, EdgeTableColumnNames.Color, oEdge,
                ReservedMetadataKeys.PerColor,
                oReadWorkbookContext.ColorConverter2);

            // Width.

            ReadWidth(oRow, oReadWorkbookContext.EdgeWidthConverter, oEdge);

            // Style.

            ReadStyle(oRow, oReadWorkbookContext.EdgeStyleConverter, oEdge);

            // Label.

            if (oReadWorkbookContext.ReadEdgeLabels)
            {
                ReadCellAndSetMetadata(oRow, EdgeTableColumnNames.Label, oEdge,
                    ReservedMetadataKeys.PerEdgeLabel);
            }

            // Weight.

            if (oReadWorkbookContext.ReadEdgeWeights)
            {
                ReadEdgeWeight(oRow, oEdge);
            }
        }

        if (bReadAllEdgeAndVertexColumns)
        {
            // Store the edge column names on the graph.

            oGraph.SetValue( ReservedMetadataKeys.AllEdgeMetadataKeys,
                FilterColumnNames(oExcelTableReader, oColumnNamesToExclude) );
        }
    }

    //*************************************************************************
    //  Method: ReadWidth()
    //
    /// <summary>
    /// If a width has been specified for an edge, sets the edge's width.
    /// </summary>
    ///
    /// <param name="oRow">
    /// Row containing the edge data.
    /// </param>
    ///
    /// <param name="oEdgeWidthConverter">
    /// Object that converts an edge width between values used in the Excel
    /// workbook and values used in the NodeXL graph.
    /// </param>
    ///
    /// <param name="oEdge">
    /// Edge to set the width on.
    /// </param>
    //*************************************************************************

    protected void
    ReadWidth
    (
        ExcelTableReader.ExcelTableRow oRow,
        EdgeWidthConverter oEdgeWidthConverter,
        IEdge oEdge
    )
    {
        Debug.Assert(oRow != null);
        Debug.Assert(oEdgeWidthConverter != null);
        AssertValid();

        String sWidth;

        if ( !oRow.TryGetNonEmptyStringFromCell(EdgeTableColumnNames.Width,
            out sWidth) )
        {
            return;
        }

        Single fWidth;

        if ( !Single.TryParse(sWidth, out fWidth) )
        {
            Range oInvalidCell = oRow.GetRangeForCell(
                EdgeTableColumnNames.Width);

            OnWorkbookFormatError( String.Format(

                "The cell {0} contains an invalid width.  The edge width,"
                + " which is optional, must be a number.  Any number is"
                + " acceptable, although {1} is used for any number less than"
                + " {1} and {2} is used for any number greater than {2}."
                ,
                ExcelUtil.GetRangeAddress(oInvalidCell),
                EdgeWidthConverter.MinimumWidthWorkbook,
                EdgeWidthConverter.MaximumWidthWorkbook
                ),

                oInvalidCell
            );
        }

        oEdge.SetValue(ReservedMetadataKeys.PerEdgeWidth, 
            oEdgeWidthConverter.WorkbookToGraph(fWidth) );
    }

    //*************************************************************************
    //  Method: ReadStyle()
    //
    /// <summary>
    /// If a style has been specified for an edge, sets the edge's style.
    /// </summary>
    ///
    /// <param name="oRow">
    /// Row containing the edge data.
    /// </param>
    ///
    /// <param name="oEdgeStyleConverter">
    /// Object that converts an edge style between values used in the Excel
    /// workbook and values used in the NodeXL graph.
    /// </param>
    ///
    /// <param name="oEdge">
    /// Edge to set the style on.
    /// </param>
    //*************************************************************************

    protected void
    ReadStyle
    (
        ExcelTableReader.ExcelTableRow oRow,
        EdgeStyleConverter oEdgeStyleConverter,
        IEdge oEdge
    )
    {
        Debug.Assert(oRow != null);
        Debug.Assert(oEdgeStyleConverter != null);
        AssertValid();

        String sStyle;

        if ( !oRow.TryGetNonEmptyStringFromCell(EdgeTableColumnNames.Style,
            out sStyle) )
        {
            return;
        }

        EdgeStyle eStyle;

        if ( !oEdgeStyleConverter.TryWorkbookToGraph(sStyle, out eStyle) )
        {
            OnWorkbookFormatErrorWithDropDown(oRow, EdgeTableColumnNames.Style,
                "style");
        }

        oEdge.SetValue(ReservedMetadataKeys.PerEdgeStyle, eStyle);
    }

    //*************************************************************************
    //  Method: ReadEdgeWeight()
    //
    /// <summary>
    /// Reads the edge weight column.
    /// </summary>
    ///
    /// <param name="oRow">
    /// Row containing the edge data.
    /// </param>
    ///
    /// <param name="oEdge">
    /// Edge to set the edge weight on.
    /// </param>
    ///
    /// <remarks>
    /// If there is no edge weight column, the edge weight on the edge is set
    /// to 1.
    /// </remarks>
    //*************************************************************************

    protected void
    ReadEdgeWeight
    (
        ExcelTableReader.ExcelTableRow oRow,
        IEdge oEdge
    )
    {
        Debug.Assert(oRow != null);
        AssertValid();

        Double dEdgeWeight = 0;

        if ( !oRow.TryGetDoubleFromCell(EdgeTableColumnNames.EdgeWeight,
            out dEdgeWeight)
            )
        {
            // There is no edge weight column, or the edge weight cell for this
            // edge is empty.

            dEdgeWeight = 1;
        }

        oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, dEdgeWeight);
    }

    //*************************************************************************
    //  Method: OnHalfEmptyEdgeRow()
    //
    /// <summary>
    /// Handles an error in which a worksheet row contains only one vertex
    /// name.
    /// </summary>
    ///
    /// <param name="oRow">
    /// Invalid row.
    /// </param>
    ///
    /// <param name="bVertex1IsEmpty">
    /// true if the "vertex 1" cell in the row is empty, false if the
    /// "vertex 2" cell is empty.
    /// </param>
    ///
    /// <remarks>
    /// This method throws a <see cref="WorkbookFormatException" />.
    /// </remarks>
    //*************************************************************************

    protected void
    OnHalfEmptyEdgeRow
    (
        ExcelTableReader.ExcelTableRow oRow,
        Boolean bVertex1IsEmpty
    )
    {
        Debug.Assert(oRow != null);

        AssertValid();

        Range oVertex1Cell = oRow.GetRangeForCell(
            EdgeTableColumnNames.Vertex1Name);

        Range oVertex2Cell = oRow.GetRangeForCell(
            EdgeTableColumnNames.Vertex2Name);

        Range oEmptyCell = bVertex1IsEmpty ? oVertex1Cell : oVertex2Cell;
        Range oNonEmptyCell = bVertex1IsEmpty ? oVertex2Cell : oVertex1Cell;

        String sEmptyRangeAddress = ExcelUtil.GetRangeAddress(oEmptyCell);
        String sNonEmptyRangeAddress = ExcelUtil.GetRangeAddress(oNonEmptyCell);

        String sErrorMessage = String.Format(

            "Cell {0} contains a vertex name but cell {1} is empty."
            + "  You can include an empty row, which will be ignored,"
            + " but you can't include a half-empty row."
            + "\r\n\r\n"
            + "You can fix the problem by entering a vertex name in {1} or"
            + " deleting the name in {0}."
            ,
            sNonEmptyRangeAddress,
            sEmptyRangeAddress
            );

        OnWorkbookFormatError(sErrorMessage, oEmptyCell);
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
