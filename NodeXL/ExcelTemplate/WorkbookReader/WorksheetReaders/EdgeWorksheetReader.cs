
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
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
    /// Reads the edge worksheet and adds the edge data to a graph.
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
            AddEdgeTableToGraph(oEdgeTable, readWorkbookContext, graph);
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

        if (oEdgeTable.ListColumns.Count < MinimumEdgeTableColumns)
        {
            OnWorkbookFormatError( String.Format(

                "The table named \"{0}\" must have at least {1} columns."
                + "\r\n\r\n{2}"
                ,
                TableNames.Edges,
                MinimumEdgeTableColumns,
                ErrorUtil.GetTemplateMessage()
                ) );
        }

        return (oEdgeTable);
    }

    //*************************************************************************
    //  Method: GetEdgeTableColumnIndexes()
    //
    /// <summary>
    /// Gets the one-based indexes of the columns within the table that
    /// contains the edge data.
    /// </summary>
    ///
    /// <param name="edgeTable">
    /// Table that contains the edge data.
    /// </param>
    ///
    /// <returns>
    /// The column indexes, as an <see cref="EdgeTableColumnIndexes" />.
    /// </returns>
    ///
    /// <remarks>
    /// If the indexes can't be returned, a <see
    /// cref="WorkbookFormatException" /> is thrown.
    /// </remarks>
    //*************************************************************************

    public EdgeTableColumnIndexes
    GetEdgeTableColumnIndexes
    (
        ListObject edgeTable
    )
    {
        Debug.Assert(edgeTable != null);
        AssertValid();

        EdgeTableColumnIndexes oEdgeTableColumnIndexes =
            new EdgeTableColumnIndexes();

        oEdgeTableColumnIndexes.Vertex1Name = GetTableColumnIndex(
            edgeTable, EdgeTableColumnNames.Vertex1Name, true);

        oEdgeTableColumnIndexes.Vertex2Name = GetTableColumnIndex(
            edgeTable, EdgeTableColumnNames.Vertex2Name, true);

        oEdgeTableColumnIndexes.Color = GetTableColumnIndex(
            edgeTable, EdgeTableColumnNames.Color, false);

        oEdgeTableColumnIndexes.Width = GetTableColumnIndex(
            edgeTable, EdgeTableColumnNames.Width, false);

        oEdgeTableColumnIndexes.Alpha = GetTableColumnIndex(
            edgeTable, EdgeTableColumnNames.Alpha, false);

        oEdgeTableColumnIndexes.Visibility = GetTableColumnIndex(
            edgeTable, EdgeTableColumnNames.Visibility, false);

        oEdgeTableColumnIndexes.EdgeWeight = GetTableColumnIndex(
            edgeTable, EdgeTableColumnNames.EdgeWeight, false);

        oEdgeTableColumnIndexes.ID = GetTableColumnIndex(
            edgeTable, CommonTableColumnNames.ID, false);

        return (oEdgeTableColumnIndexes);
    }

    //*************************************************************************
    //  Method: AddEdgeTableToGraph()
    //
    /// <summary>
    /// Adds the contents of the edge table to a NodeXL graph.
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
    AddEdgeTableToGraph
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

        // Read the range that contains visible edge data.  If the table is
        // filtered, the range may contain multiple areas.

        Range oVisibleEdgeRange;

        if ( !ExcelUtil.TryGetVisibleTableRange(oEdgeTable,
            out oVisibleEdgeRange) )
        {
            return;
        }

        // Get the indexes of the columns within the table.

        EdgeTableColumnIndexes oEdgeTableColumnIndexes =
            GetEdgeTableColumnIndexes(oEdgeTable);

        // Loop through the areas, and split each area into subranges if the
        // area contains too many rows.

        foreach ( Range oSubrange in
            ExcelRangeSplitter.SplitRange(oVisibleEdgeRange) )
        {
            if (oReadWorkbookContext.FillIDColumns)
            {
                // If the ID column exists, fill the rows within it that are
                // contained within the subrange with a sequence of unique IDs.

                FillIDColumn(oEdgeTable, oEdgeTableColumnIndexes.ID,
                    oSubrange);
            }

            // Add the contents of the subrange to the graph.

            AddEdgeSubrangeToGraph(oSubrange, oEdgeTableColumnIndexes,
                oGraph, oReadWorkbookContext);
        }
    }

    //*************************************************************************
    //  Method: AddEdgeSubrangeToGraph()
    //
    /// <summary>
    /// Adds the contents of one subrange of the edge range to a NodeXL graph.
    /// </summary>
    ///
    /// <param name="oEdgeSubrange">
    /// One subrange of the range that contains edge data.
    /// </param>
    ///
    /// <param name="oEdgeTableColumnIndexes">
    /// One-based indexes of the columns within the edge table.
    /// </param>
    ///
    /// <param name="oGraph">
    /// Graph to add edges to.
    /// </param>
    ///
    /// <param name="oReadWorkbookContext">
    /// Provides access to objects needed for converting an Excel workbook to a
    /// NodeXL graph.
    /// </param>
    //*************************************************************************

    protected void
    AddEdgeSubrangeToGraph
    (
        Range oEdgeSubrange,
        EdgeTableColumnIndexes oEdgeTableColumnIndexes,
        IGraph oGraph,
        ReadWorkbookContext oReadWorkbookContext
    )
    {
        Debug.Assert(oEdgeSubrange != null);
        Debug.Assert(oGraph != null);
        Debug.Assert(oReadWorkbookContext != null);
        AssertValid();

        IVertexCollection oVertices = oGraph.Vertices;
        IEdgeCollection oEdges = oGraph.Edges;

        Object [,] aoEdgeValues = ExcelUtil.GetRangeValues(oEdgeSubrange);

        Dictionary<String, IVertex> oVertexNameDictionary =
            oReadWorkbookContext.VertexNameDictionary;

        EdgeVisibilityConverter oEdgeVisibilityConverter =
            new EdgeVisibilityConverter();

        Boolean bGraphIsDirected =
            (oGraph.Directedness == GraphDirectedness.Directed);

        // Loop through the rows.  Each row contains two vertex names at a
        // minimum and represents an edge.

        Int32 iRows = oEdgeSubrange.Rows.Count;

        for (Int32 iRowOneBased = 1; iRowOneBased <= iRows; iRowOneBased++)
        {
            // Get the names of the edge's vertices.

            String sVertex1Name, sVertex2Name;

            Boolean bVertex1IsEmpty = !ExcelUtil.TryGetNonEmptyStringFromCell(
                aoEdgeValues, iRowOneBased,
                oEdgeTableColumnIndexes.Vertex1Name, out sVertex1Name);

            Boolean bVertex2IsEmpty = !ExcelUtil.TryGetNonEmptyStringFromCell(
                aoEdgeValues, iRowOneBased,
                oEdgeTableColumnIndexes.Vertex2Name, out sVertex2Name);

            if (bVertex1IsEmpty && bVertex2IsEmpty)
            {
                // Skip empty rows.

                continue;
            }

            if (bVertex1IsEmpty || bVertex2IsEmpty)
            {
                // A half-empty row is an error.

                OnHalfEmptyEdgeRow(oEdgeSubrange, iRowOneBased,
                    oEdgeTableColumnIndexes, bVertex1IsEmpty);
            }

            // Assume a default visibility.

            Visibility eVisibility = Visibility.Show;

            String sVisibility;

            if (
                oEdgeTableColumnIndexes.Visibility != NoSuchColumn
                &&
                ExcelUtil.TryGetNonEmptyStringFromCell(aoEdgeValues,
                    iRowOneBased, oEdgeTableColumnIndexes.Visibility,
                    out sVisibility)
                &&
                !oEdgeVisibilityConverter.TryWorkbookToGraph(
                    sVisibility, out eVisibility)
                )
            {
                OnInvalidVisibility(oEdgeSubrange, iRowOneBased,
                    oEdgeTableColumnIndexes.Visibility);
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

            // If there is an ID column, add the edge to the edge ID dictionary
            // and set the edge's Tag to the ID.

            if (oEdgeTableColumnIndexes.ID != NoSuchColumn)
            {
                AddToIDDictionary(oEdgeSubrange, aoEdgeValues, iRowOneBased,
                    oEdgeTableColumnIndexes.ID, oEdge,
                    oReadWorkbookContext.EdgeIDDictionary);
            }

            if (eVisibility == Visibility.Hide)
            {
                // Hide the edge and continue to the next edge.

                oEdge.SetValue(ReservedMetadataKeys.Visibility,
                    VisibilityKeyValue.Hidden);

                continue;
            }

            // If there is an alpha column and the alpha for this row isn't
            // empty, set the edge's alpha.

            if (oEdgeTableColumnIndexes.Alpha != NoSuchColumn)
            {
                Boolean bAlphaIsZero = CheckForAlpha(oEdgeSubrange,
                    aoEdgeValues, iRowOneBased, oEdgeTableColumnIndexes.Alpha,
                    oEdge);

                if (bAlphaIsZero)
                {
                    continue;
                }
            }

            // If there is a color column and the color for this row isn't
            // empty, set the edge's color.

            if (oEdgeTableColumnIndexes.Color != NoSuchColumn)
            {
                CheckForColor(oEdgeSubrange, aoEdgeValues, iRowOneBased,
                    oEdgeTableColumnIndexes.Color, oEdge,
                    ReservedMetadataKeys.PerColor,
                    oReadWorkbookContext.ColorConverter2);
            }

            // If there is a width column and the width for this row isn't
            // empty, set the edge's width.

            if (oEdgeTableColumnIndexes.Width != NoSuchColumn)
            {
                CheckForWidth(oEdgeSubrange, aoEdgeValues, iRowOneBased,
                    oEdgeTableColumnIndexes.Width,
                    oReadWorkbookContext.EdgeWidthConverter, oEdge);
            }

            // If edge weight values should be set, set the edge's weight.

            if (oReadWorkbookContext.SetEdgeWeightValues)
            {
                SetEdgeWeight(oEdgeSubrange, aoEdgeValues, iRowOneBased,
                    oEdgeTableColumnIndexes.EdgeWeight, oEdge);
            }
        }
    }

    //*************************************************************************
    //  Method: CheckForWidth()
    //
    /// <summary>
    /// If a width has been specified for an edge, sets the edge's width.
    /// </summary>
    ///
    /// <param name="oEdgeRange">
    /// Range containing the edge data.
    /// </param>
    ///
    /// <param name="aoEdgeValues">
    /// Values from <paramref name="oEdgeRange" />.
    /// </param>
    ///
    /// <param name="iRowOneBased">
    /// One-based row index to check.
    /// </param>
    ///
    /// <param name="iColumnOneBased">
    /// One-based column index to check.
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
    CheckForWidth
    (
        Range oEdgeRange,
        Object [,] aoEdgeValues,
        Int32 iRowOneBased,
        Int32 iColumnOneBased,
        EdgeWidthConverter oEdgeWidthConverter,
        IEdge oEdge
    )
    {
        Debug.Assert(oEdge != null);
        Debug.Assert(oEdgeRange != null);
        Debug.Assert(aoEdgeValues != null);
        Debug.Assert(iRowOneBased >= 1);
        Debug.Assert(iColumnOneBased >= 1);
        Debug.Assert(oEdgeWidthConverter != null);
        AssertValid();

        String sWidth;

        if ( !ExcelUtil.TryGetNonEmptyStringFromCell(aoEdgeValues,
            iRowOneBased, iColumnOneBased, out sWidth) )
        {
            return;
        }

        Single fWidth;

        if ( !Single.TryParse(sWidth, out fWidth) )
        {
            Range oInvalidCell =
                (Range)oEdgeRange.Cells[iRowOneBased, iColumnOneBased];

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
    //  Method: SetEdgeWeight()
    //
    /// <summary>
    /// Sets the edge weight on an edge.
    /// </summary>
    ///
    /// <param name="oEdgeRange">
    /// Range containing the edge data.
    /// </param>
    ///
    /// <param name="aoEdgeValues">
    /// Values from <paramref name="oEdgeRange" />.
    /// </param>
    ///
    /// <param name="iRowOneBased">
    /// One-based row index to check.
    /// </param>
    ///
    /// <param name="iColumnOneBased">
    /// One-based column index to check, or NoSuchColumn if there is no edge
    /// weight column.
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
    SetEdgeWeight
    (
        Range oEdgeRange,
        Object [,] aoEdgeValues,
        Int32 iRowOneBased,
        Int32 iColumnOneBased,
        IEdge oEdge
    )
    {
        Debug.Assert(oEdge != null);
        Debug.Assert(oEdgeRange != null);
        Debug.Assert(aoEdgeValues != null);
        Debug.Assert(iRowOneBased >= 1);
        Debug.Assert(iColumnOneBased == NoSuchColumn || iColumnOneBased >= 1);
        AssertValid();

        Double dEdgeWeight = 0;

        if (
            iColumnOneBased == NoSuchColumn
            ||
            !ExcelUtil.TryGetDoubleFromCell(aoEdgeValues,
                iRowOneBased, iColumnOneBased, out dEdgeWeight)
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
    /// <param name="oEdgeRange">
    /// Range containing edge data.
    /// </param>
    ///
    /// <param name="iHalfEmptyRowOneBased">
    /// One-based index of the row in <paramref name="oEdgeRange" /> that
    /// is half-empty.
    /// </param>
    ///
    /// <param name="oEdgeTableColumnIndexes">
    /// One-based indexes of the columns within the edge table.
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
        Range oEdgeRange,
        Int32 iHalfEmptyRowOneBased,
        EdgeTableColumnIndexes oEdgeTableColumnIndexes,
        Boolean bVertex1IsEmpty
    )
    {
        Debug.Assert(oEdgeRange != null);
        Debug.Assert(iHalfEmptyRowOneBased >= 1);

        AssertValid();

        Range oVertex1Cell = (Range)oEdgeRange.Cells[
            iHalfEmptyRowOneBased, oEdgeTableColumnIndexes.Vertex1Name
            ];

        Range oVertex2Cell = (Range)oEdgeRange.Cells[
            iHalfEmptyRowOneBased, oEdgeTableColumnIndexes.Vertex2Name
            ];

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
    //  Protected constants
    //*************************************************************************

    /// Minimum number of columns in the edges table.  (Some of the columns
    /// used by this class are optional, and the user can add his own columns
    /// to the table.)

    protected const Int32 MinimumEdgeTableColumns = 2;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)


    //*************************************************************************
    //  Embedded class: EdgeTableColumnIndexes
    //
    /// <summary>
    /// Contains the one-based indexes of the columns in the required edge
    /// table.
    /// </summary>
    //*************************************************************************

    public class EdgeTableColumnIndexes
    {
        /// Name of the edge's first vertex.

        public Int32 Vertex1Name;

        /// Name of the edge's second vertex.

        public Int32 Vertex2Name;

        /// The edge's optional color.

        public Int32 Color;

        /// The edge's optional width.

        public Int32 Width;

        /// The edge's optional alpha, from 0 (transparent) to 10 (opaque).

        public Int32 Alpha;

        /// The edge's optional visibility.

        public Int32 Visibility;

        /// The edge's optional edge weight.

        public Int32 EdgeWeight;

        /// The edge's ID, which is filled in by ReadWorksheet().

        public Int32 ID;
    }
}

}
