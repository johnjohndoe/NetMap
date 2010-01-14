
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Media;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Visualization.Wpf;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.WpfGraphicsLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: VertexWorksheetReader
//
/// <summary>
/// Class that knows how to read an Excel worksheet containing vertex data.
/// </summary>
///
/// <remarks>
/// Call <see cref="ReadWorksheet" /> to read the worksheet.
/// </remarks>
//*****************************************************************************

public class VertexWorksheetReader : WorksheetReaderBase
{
    //*************************************************************************
    //  Constructor: VertexWorksheetReader()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexWorksheetReader" />
    /// class.
    /// </summary>
    //*************************************************************************

    public VertexWorksheetReader()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Enum: Visibility
    //
    /// <summary>
    /// Specifies the visibility of a vertex.
    /// </summary>
    //*************************************************************************

    public enum
    Visibility
    {
        /// <summary>
        /// If the vertex is part of an edge, show it using the specified
        /// vertex attributes.  Otherwise, ignore the vertex row.  This is the
        /// default.
        /// </summary>

        ShowIfInAnEdge,

        /// <summary>
        /// Skip the vertex row and any edge rows that include the vertex.  Do
        /// not read them into the graph.
        /// </summary>

        Skip,

        /// <summary>
        /// If the vertex is part of an edge, hide it and its incident edges.
        /// Otherwise, ignore the vertex row.
        /// </summary>

        Hide,

        /// <summary>
        /// Show the vertex using the specified attributes regardless of
        /// whether it is part of an edge.
        /// </summary>

        Show,
    }

    //*************************************************************************
    //  Method: ReadWorksheet()
    //
    /// <summary>
    /// Reads the vertex worksheet and adds the vertex data to a graph.
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
    ///
    /// <param name="graph">
    /// Graph to add vertex data to.
    /// </param>
    ///
    /// <remarks>
    /// If the vertex worksheet in <paramref name="workbook" /> contains valid
    /// vertex data, the vertices in <paramref name="graph" /> are marked with
    /// metadata; any isolated vertices are added to <paramref
    /// name="graph" /> and <paramref
    /// name="readWorkbookContext" />.VertexNameDictionary; and any
    /// skipped vertices (and their incident edges) are removed from <paramref
    /// name="graph" />, <paramref
    /// name="readWorkbookContext" />.VertexNameDictionary, and
    /// <paramref name="readWorkbookContext" />.EdgeIDDictionary.  Otherwise, a
    /// <see cref="WorkbookFormatException" /> is thrown.
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

        // Attempt to get the optional table that contains vertex data.

        ListObject oVertexTable;

        if ( ExcelUtil.TryGetTable(workbook, WorksheetNames.Vertices,
            TableNames.Vertices, out oVertexTable) )
        {
            // The code that reads the table can handle hidden rows, but not
            // hidden columns.  Temporarily show all hidden columns in the
            // table.

            ExcelHiddenColumns oHiddenColumns =
                ExcelColumnHider.ShowHiddenColumns(oVertexTable);

            try
            {
                // Add the vertices in the table to the graph.

                AddVertexTableToGraph(oVertexTable, readWorkbookContext,
                    graph);
            }
            finally
            {
                ExcelColumnHider.RestoreHiddenColumns(oVertexTable,
                    oHiddenColumns);
            }

            if (readWorkbookContext.LayoutOrderSet)
            {
                // The layout order was specified for at least one vertex.
                // The ByMetadataVertexSorter used by SortableLayoutBase
                // requires that if layout order is set on one vertex, it must
                // be set on all vertices.  This isn't required in the Excel
                // template, though, so set a default layout order for each
                // vertex that doesn't already specify one.

                SetUnsetVertexOrders(graph);

                // The layout order is ignored unless this key is added to the
                // graph.

                graph.SetValue(
                    ReservedMetadataKeys.SortableLayoutOrderSet, null);
            }
        }
    }

    //*************************************************************************
    //  Method: GetVertexTableColumnIndexes()
    //
    /// <summary>
    /// Gets the one-based indexes of the columns within the table that
    /// contains the vertex data.
    /// </summary>
    ///
    /// <param name="oVertexTable">
    /// Table that contains the vertex data.
    /// </param>
    ///
    /// <returns>
    /// The column indexes, as a <see cref="VertexTableColumnIndexes" />.
    /// </returns>
    //*************************************************************************

    public VertexTableColumnIndexes
    GetVertexTableColumnIndexes
    (
        ListObject oVertexTable
    )
    {
        Debug.Assert(oVertexTable != null);
        AssertValid();

        VertexTableColumnIndexes oVertexTableColumnIndexes =
            new VertexTableColumnIndexes();

        oVertexTableColumnIndexes.VertexName = GetTableColumnIndex(
            oVertexTable, VertexTableColumnNames.VertexName, false);

        oVertexTableColumnIndexes.Color = GetTableColumnIndex(
            oVertexTable, VertexTableColumnNames.Color, false);

        oVertexTableColumnIndexes.Shape = GetTableColumnIndex(
            oVertexTable, VertexTableColumnNames.Shape, false);

        oVertexTableColumnIndexes.Radius = GetTableColumnIndex(
            oVertexTable, VertexTableColumnNames.Radius, false);

        oVertexTableColumnIndexes.ImageUri = GetTableColumnIndex(
            oVertexTable, VertexTableColumnNames.ImageUri, false);

        oVertexTableColumnIndexes.Label = GetTableColumnIndex(
            oVertexTable, VertexTableColumnNames.Label, false);

        oVertexTableColumnIndexes.LabelFillColor = GetTableColumnIndex(
            oVertexTable, VertexTableColumnNames.LabelFillColor, false);

        oVertexTableColumnIndexes.LabelPosition = GetTableColumnIndex(
            oVertexTable, VertexTableColumnNames.LabelPosition, false);

        oVertexTableColumnIndexes.Alpha = GetTableColumnIndex(
            oVertexTable, VertexTableColumnNames.Alpha, false);

        oVertexTableColumnIndexes.ToolTip = GetTableColumnIndex(
            oVertexTable, VertexTableColumnNames.ToolTip, false);

        oVertexTableColumnIndexes.Visibility = GetTableColumnIndex(
            oVertexTable, VertexTableColumnNames.Visibility, false);

        oVertexTableColumnIndexes.Order = GetTableColumnIndex(
            oVertexTable, VertexTableColumnNames.LayoutOrder, false);

        oVertexTableColumnIndexes.X = GetTableColumnIndex(
            oVertexTable, VertexTableColumnNames.X, false);

        oVertexTableColumnIndexes.Y = GetTableColumnIndex(
            oVertexTable, VertexTableColumnNames.Y, false);

        oVertexTableColumnIndexes.PolarR = GetTableColumnIndex(
            oVertexTable, VertexTableColumnNames.PolarR, false);

        oVertexTableColumnIndexes.PolarAngle = GetTableColumnIndex(
            oVertexTable, VertexTableColumnNames.PolarAngle, false);

        oVertexTableColumnIndexes.Locked = GetTableColumnIndex(
            oVertexTable, VertexTableColumnNames.Locked, false);

        oVertexTableColumnIndexes.ID = GetTableColumnIndex(
            oVertexTable, CommonTableColumnNames.ID, false);

        oVertexTableColumnIndexes.Marked = GetTableColumnIndex(
            oVertexTable, VertexTableColumnNames.Marked, false);

        return (oVertexTableColumnIndexes);
    }

    //*************************************************************************
    //  Method: AddVertexTableToGraph()
    //
    /// <summary>
    /// Adds the contents of the vertex table to a NodeXL graph.
    /// </summary>
    ///
    /// <param name="oVertexTable">
    /// Table that contains the vertex data.
    /// </param>
    ///
    /// <param name="oReadWorkbookContext">
    /// Provides access to objects needed for converting an Excel workbook to a
    /// NodeXL graph.
    /// </param>
    ///
    /// <param name="oGraph">
    /// Graph to add vertices to.
    /// </param>
    //*************************************************************************

    protected void
    AddVertexTableToGraph
    (
        ListObject oVertexTable,
        ReadWorkbookContext oReadWorkbookContext,
        IGraph oGraph
    )
    {
        Debug.Assert(oVertexTable != null);
        Debug.Assert(oReadWorkbookContext != null);
        Debug.Assert(oGraph != null);
        AssertValid();

        // Read the range that contains visible vertex data.  If the table is
        // filtered, the range may contain multiple areas.

        Range oVisibleVertexRange;

        if ( !ExcelUtil.TryGetVisibleTableRange(
            oVertexTable, out oVisibleVertexRange) )
        {
            // There is no visible vertex data.

            return;
        }

        // Get the indexes of the columns within the table.

        VertexTableColumnIndexes oVertexTableColumnIndexes =
            GetVertexTableColumnIndexes(oVertexTable);

        if (oVertexTableColumnIndexes.VertexName == NoSuchColumn)
        {
            // There are no vertex names.

            return;
        }

        // Get the one-based indexes of all the column pairs that are used to
        // add custom menu items to the vertex context menu in the graph.

        TableColumnAdder oTableColumnAdder = new TableColumnAdder();

        KeyValuePair<Int32, Int32> [] aoCustomMenuItemPairIndexes =
            oTableColumnAdder.GetColumnPairIndexes(oVertexTable,
                VertexTableColumnNames.CustomMenuItemTextBase,
                VertexTableColumnNames.CustomMenuItemActionBase);

        // Loop through the areas, and split each area into subranges if the
        // area contains too many rows.

        foreach ( Range oSubrange in
            ExcelRangeSplitter.SplitRange(oVisibleVertexRange) )
        {
            if (oReadWorkbookContext.FillIDColumns)
            {
                // If the ID column exists, fill the rows within it that
                // are contained within the subrange with a sequence of
                // unique IDs.

                FillIDColumn(oVertexTable, oVertexTableColumnIndexes.ID,
                    oSubrange);
            }

            // Add the contents of the subrange to the graph.

            AddVertexSubrangeToGraph(oSubrange,
                oVertexTableColumnIndexes, aoCustomMenuItemPairIndexes,
                oReadWorkbookContext, oGraph);
        }
    }

    //*************************************************************************
    //  Method: AddVertexSubrangeToGraph()
    //
    /// <summary>
    /// Adds the contents of one subrange of the vertex range to a NodeXL
    /// graph.
    /// </summary>
    ///
    /// <param name="oVertexSubrange">
    /// One subrange of the range that contains vertex data.
    /// </param>
    ///
    /// <param name="oVertexTableColumnIndexes">
    /// One-based indexes of the columns within the vertex table.
    /// </param>
    ///
    /// <param name="aoCustomMenuItemPairIndexes">
    /// Array of pairs of one-based column indexes, one array element for each
    /// pair of columns that are used to add custom menu items to the vertex
    /// context menu in the graph.  They key is the index of the custom menu
    /// item text and the value is the index of the custom menu item action.
    /// </param>
    ///
    /// <param name="oReadWorkbookContext">
    /// Provides access to objects needed for converting an Excel workbook to a
    /// NodeXL graph.
    /// </param>
    ///
    /// <param name="oGraph">
    /// Graph to add vertices to.
    /// </param>
    //*************************************************************************

    protected void
    AddVertexSubrangeToGraph
    (
        Range oVertexSubrange,
        VertexTableColumnIndexes oVertexTableColumnIndexes,
        KeyValuePair<Int32, Int32> [] aoCustomMenuItemPairIndexes,
        ReadWorkbookContext oReadWorkbookContext,
        IGraph oGraph
    )
    {
        Debug.Assert(oVertexSubrange != null);
        Debug.Assert(oVertexTableColumnIndexes.VertexName != NoSuchColumn);
        Debug.Assert(aoCustomMenuItemPairIndexes != null);
        Debug.Assert(oReadWorkbookContext != null);
        Debug.Assert(oGraph != null);
        AssertValid();

        IVertexCollection oVertices = oGraph.Vertices;
        Object [,] aoVertexValues = ExcelUtil.GetRangeValues(oVertexSubrange);

        Dictionary<String, IVertex> oVertexNameDictionary =
            oReadWorkbookContext.VertexNameDictionary;

        Dictionary<Int32, IIdentityProvider> oEdgeIDDictionary =
            oReadWorkbookContext.EdgeIDDictionary;

        VertexVisibilityConverter oVertexVisibilityConverter =
            new VertexVisibilityConverter();

        VertexLabelPositionConverter oVertexLabelPositionConverter =
            new VertexLabelPositionConverter();

        Int32 iRows = oVertexSubrange.Rows.Count;

        for (Int32 iRowOneBased = 1; iRowOneBased <= iRows; iRowOneBased++)
        {
            // Get the name of the vertex.

            String sVertexName;

            if ( !ExcelUtil.TryGetNonEmptyStringFromCell(aoVertexValues,
                    iRowOneBased, oVertexTableColumnIndexes.VertexName,
                    out sVertexName) )
            {
                // There is no vertex name.  Skip the row.

                continue;
            }

            // If the vertex was added to the graph as part of an edge,
            // retrieve the vertex.

            IVertex oVertex;

            if ( !oVertexNameDictionary.TryGetValue(sVertexName, out oVertex) )
            {
                oVertex = null;
            }

            // Assume a default visibility.

            Visibility eVisibility = Visibility.ShowIfInAnEdge;

            String sVisibility;

            if (
                oVertexTableColumnIndexes.Visibility != NoSuchColumn
                &&
                ExcelUtil.TryGetNonEmptyStringFromCell(aoVertexValues,
                    iRowOneBased, oVertexTableColumnIndexes.Visibility,
                    out sVisibility)
                )
            {
                if ( !oVertexVisibilityConverter.TryWorkbookToGraph(
                    sVisibility, out eVisibility) )
                {
                    OnInvalidVisibility(oVertexSubrange, iRowOneBased,
                        oVertexTableColumnIndexes.Visibility);
                }
            }

            switch (eVisibility)
            {
                case Visibility.ShowIfInAnEdge:

                    // If the vertex is part of an edge, show it using the
                    // specified vertex attributes.  Otherwise, skip the vertex
                    // row.

                    if (oVertex == null)
                    {
                        continue;
                    }

                    break;

                case Visibility.Skip:

                    // Skip the vertex row and any edge rows that include the
                    // vertex.  Do not read them into the graph.

                    if (oVertex != null)
                    {
                        // Remove the vertex and its incident edges from the
                        // graph and dictionaries.

                        foreach (IEdge oIncidentEdge in oVertex.IncidentEdges)
                        {
                            if (oIncidentEdge.Tag is Int32)
                            {
                                oEdgeIDDictionary.Remove(
                                    (Int32)oIncidentEdge.Tag);
                            }
                        }

                        oVertexNameDictionary.Remove(sVertexName);

                        oVertices.Remove(oVertex);

                        // (The vertex doesn't get added to
                        // ReadWorkbookContext.VertexIDDictionary until after
                        // this switch statement, so it doesn't need to be
                        // removed from that dictionary.)
                    }

                    continue;

                case Visibility.Hide:

                    // If the vertex is part of an edge, hide it and its
                    // incident edges.  Otherwise, skip the vertex row.

                    if (oVertex == null)
                    {
                        continue;
                    }

                    // Hide the vertex and its incident edges.

                    oVertex.SetValue(ReservedMetadataKeys.Visibility,
                        VisibilityKeyValue.Hidden);

                    foreach (IEdge oIncidentEdge in oVertex.IncidentEdges)
                    {
                        oIncidentEdge.SetValue(ReservedMetadataKeys.Visibility,
                            VisibilityKeyValue.Hidden);
                    }

                    break;

                case Visibility.Show:

                    // Show the vertex using the specified attributes
                    // regardless of whether it is part of an edge.

                    if (oVertex == null)
                    {
                        oVertex = CreateVertex(
                            sVertexName, oVertices, oVertexNameDictionary);
                    }

                    break;

                default:

                    Debug.Assert(false);
                    break;
            }

            Debug.Assert(oVertex != null);

            // If there is an ID column, add the vertex to the vertex ID
            // dictionary and set the vertex's Tag to the ID.

            if (oVertexTableColumnIndexes.ID != NoSuchColumn)
            {
                AddToIDDictionary(oVertexSubrange, aoVertexValues,
                    iRowOneBased, oVertexTableColumnIndexes.ID, oVertex,
                    oReadWorkbookContext.VertexIDDictionary);
            }

            // Vertex order.

            if (oVertexTableColumnIndexes.Order != NoSuchColumn)
            {
                if ( CheckForOrder(oVertexSubrange, aoVertexValues,
                    iRowOneBased, oVertexTableColumnIndexes.Order, oVertex) )
                {
                    oReadWorkbookContext.LayoutOrderSet = true;
                }
            }

            if (!oReadWorkbookContext.IgnoreVertexLocations)
            {
                Boolean bLocationSpecified = false;

                // If there are X and Y columns and a location has been
                // specified for the vertex, set the vertex's location.

                if (oVertexTableColumnIndexes.X != NoSuchColumn &&
                    oVertexTableColumnIndexes.Y != NoSuchColumn)
                {
                    bLocationSpecified = CheckForLocation(
                        oVertexSubrange, aoVertexValues, iRowOneBased,
                        oVertexTableColumnIndexes.X,
                        oVertexTableColumnIndexes.Y,
                        oReadWorkbookContext.VertexLocationConverter,
                        oVertex);
                }

                // If there is a lock column and a lock flag has been specified
                // for the vertex, set the vertex's lock flag.  ("Lock" means
                // "prevent the layout algorithm from moving the vertex.")

                if (oVertexTableColumnIndexes.Locked != NoSuchColumn)
                {
                    CheckForLocked(oVertexSubrange, aoVertexValues,
                        iRowOneBased, oVertexTableColumnIndexes.Locked,
                        bLocationSpecified, oVertex);
                }
            }

            // Polar coordinates.

            if (oVertexTableColumnIndexes.PolarR != NoSuchColumn &&
                oVertexTableColumnIndexes.PolarAngle != NoSuchColumn)
            {
                CheckForPolarCoordinates(oVertexSubrange, aoVertexValues,
                    iRowOneBased, oVertexTableColumnIndexes.PolarR,
                    oVertexTableColumnIndexes.PolarAngle, oVertex);
            }

            // Marked.  ("Marking" is something the user does for himself.  A
            // marked vertex doesn't behave differently from an unmarked
            // vertex.)

            if (oVertexTableColumnIndexes.Marked != NoSuchColumn)
            {
                CheckForMarked(oVertexSubrange, aoVertexValues, iRowOneBased,
                    oVertexTableColumnIndexes.Marked, oVertex);
            }

            // If there is at least one pair of columns that are used to add
            // custom menu items to the vertex context menu in the graph, and
            // if custom menu items have been specified for the vertex, store
            // the custom menu item information in the vertex.

            if (aoCustomMenuItemPairIndexes.Length > 0)
            {
                CheckForCustomMenuItems(oVertexSubrange, aoVertexValues,
                    iRowOneBased, aoCustomMenuItemPairIndexes, oVertex);
            }

            // Alpha.

            if (oVertexTableColumnIndexes.Alpha != NoSuchColumn)
            {
                CheckForAlpha(oVertexSubrange, aoVertexValues, iRowOneBased,
                    oVertexTableColumnIndexes.Alpha, oVertex);
            }

            // Tooltip.

            if (oVertexTableColumnIndexes.ToolTip != NoSuchColumn)
            {
                if ( CheckForNonEmptyCell(aoVertexValues, iRowOneBased,
                    oVertexTableColumnIndexes.ToolTip, oVertex,
                    ReservedMetadataKeys.VertexToolTip) )
                {
                    oReadWorkbookContext.ToolTipsUsed = true;
                }
            }

            // Label.

            if (oReadWorkbookContext.ReadVertexLabels &&
                oVertexTableColumnIndexes.Label != NoSuchColumn)
            {
                CheckForNonEmptyCell(aoVertexValues, iRowOneBased,
                    oVertexTableColumnIndexes.Label, oVertex,
                    ReservedMetadataKeys.PerVertexLabel);
            }

            // Label fill color.

            if (oVertexTableColumnIndexes.LabelFillColor != NoSuchColumn)
            {
                CheckForColor(oVertexSubrange, aoVertexValues, iRowOneBased,
                    oVertexTableColumnIndexes.LabelFillColor, oVertex,
                    ReservedMetadataKeys.PerVertexLabelFillColor,
                    oReadWorkbookContext.ColorConverter2);
            }

            // Label position.

            if (oVertexTableColumnIndexes.LabelPosition != NoSuchColumn)
            {
                CheckForLabelPosition(oVertexSubrange, aoVertexValues,
                    iRowOneBased, oVertexTableColumnIndexes.LabelPosition,
                    oVertexLabelPositionConverter, oVertex);
            }

            // Radius.

            Nullable<Single> oRadiusWorkbook = new Nullable<Single>();

            if (oVertexTableColumnIndexes.Radius != NoSuchColumn)
            {
                oRadiusWorkbook = CheckForRadius(oVertexSubrange,
                    aoVertexValues, iRowOneBased,
                    oVertexTableColumnIndexes.Radius,
                    oReadWorkbookContext.VertexRadiusConverter, oVertex);
            }

            // Shape.

            VertexShape eVertexShape = oReadWorkbookContext.DefaultVertexShape;

            if (oVertexTableColumnIndexes.Shape != NoSuchColumn)
            {
                VertexShape ePerVertexShape;

                if ( CheckForShape(oVertexSubrange, aoVertexValues,
                    iRowOneBased, oVertexTableColumnIndexes.Shape, oVertex,
                    out ePerVertexShape) )
                {
                    eVertexShape = ePerVertexShape;
                }
            }

            // Label font size.

            if (eVertexShape == VertexShape.Label && oRadiusWorkbook.HasValue)
            {
                // The vertex radius is used to specify font size when the
                // shape is Label.

                oVertex.SetValue( ReservedMetadataKeys.PerVertexLabelFontSize,
                    oReadWorkbookContext.VertexRadiusConverter.
                        WorkbookToLabelFontSize(oRadiusWorkbook.Value) );
            }

            // Image URI.

            if (eVertexShape == VertexShape.Image &&
                oReadWorkbookContext.ReadVertexImages &&
                oVertexTableColumnIndexes.ImageUri != NoSuchColumn)
            {
                CheckForImageUri(oVertexSubrange, aoVertexValues, iRowOneBased,
                    oVertexTableColumnIndexes.ImageUri, oVertex,
                    oReadWorkbookContext.VertexRadiusConverter,

                    oRadiusWorkbook.HasValue ? oRadiusWorkbook :
                        oReadWorkbookContext.DefaultVertexImageSize
                    );
            }

            // Color

            if (oVertexTableColumnIndexes.Color != NoSuchColumn)
            {
                CheckForColor(oVertexSubrange, aoVertexValues, iRowOneBased,
                    oVertexTableColumnIndexes.Color, oVertex,
                    ReservedMetadataKeys.PerColor,
                    oReadWorkbookContext.ColorConverter2);
            }
        }
    }

    //*************************************************************************
    //  Method: CheckForLabelPosition()
    //
    /// <summary>
    /// If a label position has been specified for a vertex, sets the vertex's
    /// label position.
    /// </summary>
    ///
    /// <param name="oVertexRange">
    /// Range containing the vertex data.
    /// </param>
    ///
    /// <param name="aoVertexValues">
    /// Values from <paramref name="oVertexRange" />.
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
    /// <param name="oVertexLabelPositionConverter">
    /// Object that converts a vertex label position between values used in the
    /// Excel workbook and values used in the NodeXL graph.
    /// </param>
    ///
    /// <param name="oVertex">
    /// Vertex to set the label position on.
    /// </param>
    //*************************************************************************

    protected void
    CheckForLabelPosition
    (
        Range oVertexRange,
        Object [,] aoVertexValues,
        Int32 iRowOneBased,
        Int32 iColumnOneBased,
        VertexLabelPositionConverter oVertexLabelPositionConverter,
        IVertex oVertex
    )
    {
        Debug.Assert(oVertexRange != null);
        Debug.Assert(aoVertexValues != null);
        Debug.Assert(iRowOneBased >= 1);
        Debug.Assert(iColumnOneBased >= 1);
        Debug.Assert(oVertex != null);
        Debug.Assert(oVertexLabelPositionConverter != null);
        AssertValid();

        String sLabelPosition;

        if ( !ExcelUtil.TryGetNonEmptyStringFromCell(aoVertexValues,
            iRowOneBased, iColumnOneBased, out sLabelPosition) )
        {
            return;
        }

        VertexLabelPosition eLabelPosition;

        if ( !oVertexLabelPositionConverter.TryWorkbookToGraph(sLabelPosition,
            out eLabelPosition) )
        {
            Range oInvalidCell =
                (Range)oVertexRange.Cells[iRowOneBased, iColumnOneBased];

            OnWorkbookFormatError( String.Format(

                "The cell {0} contains an invalid label position.  Try"
                + " selecting from the cell's drop-down list instead."
                ,
                ExcelUtil.GetRangeAddress(oInvalidCell)
                ),

                oInvalidCell
            );
        }

        oVertex.SetValue( ReservedMetadataKeys.PerVertexLabelPosition,
            eLabelPosition);
    }

    //*************************************************************************
    //  Method: CheckForRadius()
    //
    /// <summary>
    /// If a radius has been specified for a vertex, sets the vertex's radius.
    /// </summary>
    ///
    /// <param name="oVertexRange">
    /// Range containing the vertex data.
    /// </param>
    ///
    /// <param name="aoVertexValues">
    /// Values from <paramref name="oVertexRange" />.
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
    /// <param name="oVertexRadiusConverter">
    /// Object that converts a vertex radius between values used in the Excel
    /// workbook and values used in the NodeXL graph.
    /// </param>
    ///
    /// <param name="oVertex">
    /// Vertex to set the radius on.
    /// </param>
    ///
    /// <returns>
    /// If a radius has been specified for the vertex, the radius in workbook
    /// units is returned.  Otherwise, a Nullable that has no value is
    /// returned.
    /// </returns>
    //*************************************************************************

    protected Nullable<Single>
    CheckForRadius
    (
        Range oVertexRange,
        Object [,] aoVertexValues,
        Int32 iRowOneBased,
        Int32 iColumnOneBased,
        VertexRadiusConverter oVertexRadiusConverter,
        IVertex oVertex
    )
    {
        Debug.Assert(oVertexRange != null);
        Debug.Assert(aoVertexValues != null);
        Debug.Assert(iRowOneBased >= 1);
        Debug.Assert(iColumnOneBased >= 1);
        Debug.Assert(oVertex != null);
        Debug.Assert(oVertexRadiusConverter != null);
        AssertValid();

        String sRadius;

        if ( !ExcelUtil.TryGetNonEmptyStringFromCell(aoVertexValues,
            iRowOneBased, iColumnOneBased, out sRadius) )
        {
            return ( new Nullable<Single>() );
        }

        Single fRadius;

        if ( !Single.TryParse(sRadius, out fRadius) )
        {
            Range oInvalidCell =
                (Range)oVertexRange.Cells[iRowOneBased, iColumnOneBased];

            OnWorkbookFormatError( String.Format(

                "The cell {0} contains an invalid size.  The vertex size,"
                + " which is optional, must be a number.  Any number is"
                + " acceptable, although {1} is used for any number less than"
                + " {1} and {2} is used for any number greater than {2}."
                ,
                ExcelUtil.GetRangeAddress(oInvalidCell),
                VertexRadiusConverter.MinimumRadiusWorkbook,
                VertexRadiusConverter.MaximumRadiusWorkbook
                ),

                oInvalidCell
            );
        }

        oVertex.SetValue( ReservedMetadataKeys.PerVertexRadius,
            oVertexRadiusConverter.WorkbookToGraph(fRadius) );

        return ( new Nullable<Single>(fRadius) );
    }

    //*************************************************************************
    //  Method: CheckForShape()
    //
    /// <summary>
    /// If a shape has been specified for a vertex, sets the vertex's shape.
    /// </summary>
    ///
    /// <param name="oVertexRange">
    /// Range containing the vertex data.
    /// </param>
    ///
    /// <param name="aoValues">
    /// Values from <paramref name="oVertexRange" />.
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
    /// <param name="oVertex">
    /// Vertex to set the shape on.
    /// </param>
    ///
    /// <param name="eShape">
    /// Where the shape gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if a shape has been specified for the vertex.
    /// </returns>
    //*************************************************************************

    protected Boolean
    CheckForShape
    (
        Range oVertexRange,
        Object [,] aoValues,
        Int32 iRowOneBased,
        Int32 iColumnOneBased,
        IVertex oVertex,
        out VertexShape eShape
    )
    {
        Debug.Assert(oVertexRange != null);
        Debug.Assert(aoValues != null);
        Debug.Assert(iRowOneBased >= 1);
        Debug.Assert(iColumnOneBased >= 1);
        Debug.Assert(oVertex != null);
        AssertValid();

        if ( TryGetVertexShape(oVertexRange, aoValues, iRowOneBased,
            iColumnOneBased, out eShape) )
        {
            oVertex.SetValue(ReservedMetadataKeys.PerVertexShape, eShape);
            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: CheckForImageUri()
    //
    /// <summary>
    /// If an image URI has been specified for a vertex, sets the vertex's
    /// image.
    /// </summary>
    ///
    /// <param name="oVertexRange">
    /// Range containing the vertex data.
    /// </param>
    ///
    /// <param name="aoVertexValues">
    /// Values from <paramref name="oVertexRange" />.
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
    /// <param name="oVertex">
    /// Vertex to set the image on.
    /// </param>
    ///
    /// <param name="oVertexRadiusConverter">
    /// Object that converts a vertex radius between values used in the Excel
    /// workbook and values used in the NodeXL graph.
    /// </param>
    ///
    /// <param name="oVertexImageSize">
    /// The size to use for the image (in workbook units), or a Nullable that
    /// has no value to use the image's actual size.
    /// </param>
    ///
    /// <returns>
    /// true if an image key was specified.
    /// </returns>
    //*************************************************************************

    protected Boolean
    CheckForImageUri
    (
        Range oVertexRange,
        Object [,] aoVertexValues,
        Int32 iRowOneBased,
        Int32 iColumnOneBased,
        IVertex oVertex,
        VertexRadiusConverter oVertexRadiusConverter,
        Nullable<Single> oVertexImageSize
    )
    {
        Debug.Assert(oVertexRange != null);
        Debug.Assert(aoVertexValues != null);
        Debug.Assert(iRowOneBased >= 1);
        Debug.Assert(iColumnOneBased >= 1);
        Debug.Assert(oVertex != null);
        Debug.Assert(oVertexRadiusConverter != null);
        AssertValid();

        String sImageUri;

        if ( !ExcelUtil.TryGetNonEmptyStringFromCell(aoVertexValues,
            iRowOneBased, iColumnOneBased, out sImageUri) )
        {
            return (false);
        }

        if ( sImageUri.ToLower().StartsWith("www.") )
        {
            // The Uri class thinks that "www.somewhere.com" is a relative
            // path.  Fix that.

            sImageUri= "http://" + sImageUri;
        }

        Uri oUri;

        // Is the URI either an URL or a full file path?

        if ( !Uri.TryCreate(sImageUri, UriKind.Absolute, out oUri) )
        {
            // No.  It appears to be a relative path.

            String sWorkbookPath =
                ( (Workbook)(oVertexRange.Worksheet.Parent) ).Path;

            if ( !String.IsNullOrEmpty(sWorkbookPath) )
            {
                sImageUri = Path.Combine(sWorkbookPath, sImageUri);
            }
            else
            {
                Range oInvalidCell = (Range)oVertexRange.Cells[
                    iRowOneBased, iColumnOneBased];

                OnWorkbookFormatError( String.Format(

                    "The image file path specified in cell {0} is a relative"
                    + " path.  Relative paths must be relative to the saved"
                    + " workbook file, but the workbook hasn't been saved yet."
                    + "  Either save the workbook or change the image file to"
                    + " an absolute path, such as \"C:\\MyImages\\Image.jpg\"."
                    ,
                    ExcelUtil.GetRangeAddress(oInvalidCell)
                    ),

                    oInvalidCell
                    );
            }
        }

        // Note that sImageUri may or may not be a valid URI string.  If it is
        // not, GetImageSynchronousIgnoreDpi() will return an error image.

        ImageSource oImage =
            ( new WpfImageUtil() ).GetImageSynchronousIgnoreDpi(sImageUri);

        if (oVertexImageSize.HasValue)
        {
            // Resize the image.

            Double dLongerDimension =
                oVertexRadiusConverter.WorkbookToLongerImageDimension(
                    oVertexImageSize.Value);

            Debug.Assert(dLongerDimension >= 1);

            oImage = ( new WpfImageUtil() ).ResizeImage(oImage,
                (Int32)dLongerDimension);
        }

        oVertex.SetValue(ReservedMetadataKeys.PerVertexImage, oImage);

        return (true);
    }

    //*************************************************************************
    //  Method: CheckForOrder()
    //
    /// <summary>
    /// If a sort order has been specified for a vertex, sets the vertex's
    /// sort order.
    /// </summary>
    ///
    /// <param name="oVertexRange">
    /// Range containing the vertex data.
    /// </param>
    ///
    /// <param name="aoVertexValues">
    /// Values from <paramref name="oVertexRange" />.
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
    /// <param name="oVertex">
    /// Vertex to set the sort order on.
    /// </param>
    ///
    /// <returns>
    /// true if a sort order was specified.
    /// </returns>
    //*************************************************************************

    protected Boolean
    CheckForOrder
    (
        Range oVertexRange,
        Object [,] aoVertexValues,
        Int32 iRowOneBased,
        Int32 iColumnOneBased,
        IVertex oVertex
    )
    {
        Debug.Assert(oVertexRange != null);
        Debug.Assert(aoVertexValues != null);
        Debug.Assert(iRowOneBased >= 1);
        Debug.Assert(iColumnOneBased >= 1);
        Debug.Assert(oVertex != null);
        AssertValid();

        String sOrder;

        if ( !ExcelUtil.TryGetNonEmptyStringFromCell(aoVertexValues,
            iRowOneBased, iColumnOneBased, out sOrder) )
        {
            return (false);
        }

        Single fOrder;

        if ( !Single.TryParse(sOrder, out fOrder) )
        {
            Range oInvalidCell =
                (Range)oVertexRange.Cells[iRowOneBased, iColumnOneBased];

            OnWorkbookFormatError( String.Format(

                "The cell {0} contains an invalid layout order.  The layout"
                + " order, which is optional, must be a number."
                ,
                ExcelUtil.GetRangeAddress(oInvalidCell)
                ),

                oInvalidCell
            );
        }

        oVertex.SetValue( ReservedMetadataKeys.SortableLayoutOrder, fOrder);

        return (true);
    }

    //*************************************************************************
    //  Method: CheckForLocation()
    //
    /// <summary>
    /// If a location has been specified for a vertex, sets the vertex's
    /// location.
    /// </summary>
    ///
    /// <param name="oVertexRange">
    /// Range containing the vertex data.
    /// </param>
    ///
    /// <param name="aoValues">
    /// Values from <paramref name="oVertexRange" />.
    /// </param>
    ///
    /// <param name="iRowOneBased">
    /// One-based row index to check.
    /// </param>
    ///
    /// <param name="iXColumnOneBased">
    /// One-based X column index to check.
    /// </param>
    ///
    /// <param name="iYColumnOneBased">
    /// One-based Y column index to check.
    /// </param>
    ///
    /// <param name="oVertexLocationConverter">
    /// Object that converts a vertex location between coordinates used in the
    /// Excel workbook and coordinates used in the NodeXL graph.
    /// </param>
    ///
    /// <param name="oVertex">
    /// Vertex to set the location on.
    /// </param>
    ///
    /// <returns>
    /// true if a location was specified.
    /// </returns>
    //*************************************************************************

    protected Boolean
    CheckForLocation
    (
        Range oVertexRange,
        Object [,] aoValues,
        Int32 iRowOneBased,
        Int32 iXColumnOneBased,
        Int32 iYColumnOneBased,
        VertexLocationConverter oVertexLocationConverter,
        IVertex oVertex
    )
    {
        Debug.Assert(oVertexRange != null);
        Debug.Assert(aoValues != null);
        Debug.Assert(iRowOneBased >= 1);
        Debug.Assert(iXColumnOneBased >= 1);
        Debug.Assert(iYColumnOneBased >= 1);
        Debug.Assert(oVertexLocationConverter != null);
        Debug.Assert(oVertex != null);
        AssertValid();

        String sX;

        Boolean bHasX = ExcelUtil.TryGetNonEmptyStringFromCell(aoValues,
            iRowOneBased, iXColumnOneBased, out sX);

        String sY;

        Boolean bHasY = ExcelUtil.TryGetNonEmptyStringFromCell(aoValues,
            iRowOneBased, iYColumnOneBased, out sY);

        if (bHasX != bHasY)
        {
            // X or Y alone won't do.

            goto Error;
        }

        if (!bHasX && !bHasY)
        {
            return (false);
        }

        Single fX, fY;

        if ( !Single.TryParse(sX, out fX) || !Single.TryParse(sY, out fY) )
        {
            goto Error;
        }

        // Transform the location from workbook coordinates to graph
        // coordinates.

        oVertex.Location = oVertexLocationConverter.WorkbookToGraph(fX, fY);

        return (true);

        Error:

            Range oInvalidCell = (Range)oVertexRange.Cells[
                iRowOneBased, iXColumnOneBased];

            OnWorkbookFormatError( String.Format(

                "There is a problem with the vertex location at {0}.  If you"
                + " enter a vertex location, it must include both X and Y"
                + " numbers.  Any numbers are acceptable, although {1} is used"
                + " for any number less than {1} and and {2} is used for any"
                + " number greater than {2}."
                ,
                ExcelUtil.GetRangeAddress(oInvalidCell),

                VertexLocationConverter.MinimumXYWorkbook.ToString(
                    ExcelTemplateForm.Int32Format),

                VertexLocationConverter.MaximumXYWorkbook.ToString(
                    ExcelTemplateForm.Int32Format)
                ),

                oInvalidCell
                );

            // Make the compiler happy.

            return (false);
    }

    //*************************************************************************
    //  Method: CheckForPolarCoordinates()
    //
    /// <summary>
    /// If polar coordinates have been specified for a vertex, sets the
    /// vertex's polar coordinates.
    /// </summary>
    ///
    /// <param name="oVertexRange">
    /// Range containing the vertex data.
    /// </param>
    ///
    /// <param name="aoValues">
    /// Values from <paramref name="oVertexRange" />.
    /// </param>
    ///
    /// <param name="iRowOneBased">
    /// One-based row index to check.
    /// </param>
    ///
    /// <param name="iPolarRColumnOneBased">
    /// One-based polar R coordinate column index to check.
    /// </param>
    ///
    /// <param name="iPolarAngleColumnOneBased">
    /// One-based polar angle coordinate column index to check.
    /// </param>
    ///
    /// <param name="oVertex">
    /// Vertex to set the location on.
    /// </param>
    ///
    /// <returns>
    /// true if a location was specified.
    /// </returns>
    //*************************************************************************

    protected Boolean
    CheckForPolarCoordinates
    (
        Range oVertexRange,
        Object [,] aoValues,
        Int32 iRowOneBased,
        Int32 iPolarRColumnOneBased,
        Int32 iPolarAngleColumnOneBased,
        IVertex oVertex
    )
    {
        Debug.Assert(oVertexRange != null);
        Debug.Assert(aoValues != null);
        Debug.Assert(iRowOneBased >= 1);
        Debug.Assert(iPolarRColumnOneBased >= 1);
        Debug.Assert(iPolarAngleColumnOneBased >= 1);
        Debug.Assert(oVertex != null);
        AssertValid();

        String sR;

        Boolean bHasR = ExcelUtil.TryGetNonEmptyStringFromCell(aoValues,
            iRowOneBased, iPolarRColumnOneBased, out sR);

        String sAngle;

        Boolean bHasAngle = ExcelUtil.TryGetNonEmptyStringFromCell(aoValues,
            iRowOneBased, iPolarAngleColumnOneBased, out sAngle);

        if (bHasR != bHasAngle)
        {
            // R or Angle alone won't do.

            goto Error;
        }

        if (!bHasR && !bHasAngle)
        {
            return (false);
        }

        Single fR, fAngle;

        if ( !Single.TryParse(sR, out fR) ||
            !Single.TryParse(sAngle, out fAngle) )
        {
            goto Error;
        }

        oVertex.SetValue(ReservedMetadataKeys.PolarLayoutCoordinates,
            new SinglePolarCoordinates(fR, fAngle) );

        return (true);

        Error:

            Range oInvalidCell = (Range)oVertexRange.Cells[
                iRowOneBased, iPolarRColumnOneBased];

            OnWorkbookFormatError( String.Format(

                "There is a problem with the vertex polar coordinates at {0}."
                + " If you enter polar coordinates, they must include both"
                + " {1} and {2} numbers.  Any numbers are acceptable."
                + "\r\n\r\n"
                + "Polar coordinates are used only when a Layout Type of Polar"
                + " or Polar Absolute is selected in the graph pane."
                ,
                ExcelUtil.GetRangeAddress(oInvalidCell),
                VertexTableColumnNames.PolarR,
                VertexTableColumnNames.PolarAngle
                ),

                oInvalidCell
                );

            // Make the compiler happy.

            return (false);
    }

    //*************************************************************************
    //  Method: CheckForLocked()
    //
    /// <summary>
    /// If a locked flag has been specified for a vertex, sets the vertex's
    /// locked flag.
    /// </summary>
    ///
    /// <param name="oVertexRange">
    /// Range containing the vertex data.
    /// </param>
    ///
    /// <param name="aoValues">
    /// Values from <paramref name="oVertexRange" />.
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
    /// <param name="bLocationSpecified">
    /// true if a location was specified for the vertex.
    /// </param>
    ///
    /// <param name="oVertex">
    /// Vertex to set the lock flag on.
    /// </param>
    //*************************************************************************

    protected void
    CheckForLocked
    (
        Range oVertexRange,
        Object [,] aoValues,
        Int32 iRowOneBased,
        Int32 iColumnOneBased,
        Boolean bLocationSpecified,
        IVertex oVertex
    )
    {
        Debug.Assert(oVertexRange != null);
        Debug.Assert(aoValues != null);
        Debug.Assert(iRowOneBased >= 1);
        Debug.Assert(iColumnOneBased >= 1);
        Debug.Assert(oVertex != null);
        AssertValid();

        String sLocked;

        if ( !ExcelUtil.TryGetNonEmptyStringFromCell(aoValues,
            iRowOneBased, iColumnOneBased, out sLocked) )
        {
            return;
        }

        Boolean bLocked;
        String sErrorMessage;

        if ( !( new BooleanConverter() ).TryWorkbookToGraph(
            sLocked, out bLocked) )
        {
            sErrorMessage = 
                "The cell {0} contains an invalid lock value.  Try selecting"
                + " from the cell's drop-down list instead."
                ;

            goto Error;
        }

        if (bLocked && !bLocationSpecified)
        {
            sErrorMessage = 
                "The cell {0} indicates that the vertex should be locked,"
                + " but the vertex has no X and Y location values.  Either"
                + " clear the lock or specify a vertex location."
                ;

            goto Error;
        }

        // Set the "lock vertex" key.

        oVertex.SetValue(ReservedMetadataKeys.LockVertexLocation, bLocked);

        return;

        Error:

            Debug.Assert(sErrorMessage != null);
            Debug.Assert(sErrorMessage.IndexOf("{0}") >= 0);

            Range oInvalidCell =
                (Range)oVertexRange.Cells[iRowOneBased, iColumnOneBased];

            OnWorkbookFormatError( String.Format(

                sErrorMessage
                ,
                ExcelUtil.GetRangeAddress(oInvalidCell)
                ),

                oInvalidCell
            );
    }

    //*************************************************************************
    //  Method: CheckForMarked()
    //
    /// <summary>
    /// If a marked flag has been specified for a vertex, sets the vertex's
    /// marked flag.
    /// </summary>
    ///
    /// <param name="oVertexRange">
    /// Range containing the vertex data.
    /// </param>
    ///
    /// <param name="aoValues">
    /// Values from <paramref name="oVertexRange" />.
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
    /// <param name="oVertex">
    /// Vertex to set the marked flag on.
    /// </param>
    //*************************************************************************

    protected void
    CheckForMarked
    (
        Range oVertexRange,
        Object [,] aoValues,
        Int32 iRowOneBased,
        Int32 iColumnOneBased,
        IVertex oVertex
    )
    {
        Debug.Assert(oVertexRange != null);
        Debug.Assert(aoValues != null);
        Debug.Assert(iRowOneBased >= 1);
        Debug.Assert(iColumnOneBased >= 1);
        Debug.Assert(oVertex != null);
        AssertValid();

        String sMarked;

        if ( !ExcelUtil.TryGetNonEmptyStringFromCell(aoValues,
            iRowOneBased, iColumnOneBased, out sMarked) )
        {
            return;
        }

        Boolean bMarked;

        if ( !( new BooleanConverter() ).TryWorkbookToGraph(
            sMarked, out bMarked) )
        {
            Range oInvalidCell = (Range)oVertexRange.Cells[
                iRowOneBased, iColumnOneBased];

            OnWorkbookFormatError( String.Format(

                "The cell {0} contains an invalid marked value.  Try selecting"
                + " from the cell's drop-down list instead."
                ,
                ExcelUtil.GetRangeAddress(oInvalidCell)
                ),

                oInvalidCell
                );
        }

        // Set the "mark vertex" key.

        oVertex.SetValue(ReservedMetadataKeys.Marked, bMarked);

        return;
    }

    //*************************************************************************
    //  Method: CheckForCustomMenuItems()
    //
    /// <summary>
    /// If custom menu items have been specified for a vertex, stores the
    /// custom menu item information in the vertex.
    /// </summary>
    ///
    /// <param name="oVertexRange">
    /// Range containing the vertex data.
    /// </param>
    ///
    /// <param name="aoValues">
    /// Values from <paramref name="oVertexRange" />.
    /// </param>
    ///
    /// <param name="iRowOneBased">
    /// One-based row index to check.
    /// </param>
    ///
    /// <param name="aoCustomMenuItemPairIndexes">
    /// Array of pairs of one-based column indexes, one array element for each
    /// pair of columns that are used to add custom menu items to the vertex
    /// context menu in the graph.  They key is the index of the custom menu
    /// item text and the value is the index of the custom menu item action.
    /// </param>
    ///
    /// <param name="oVertex">
    /// Vertex to add custom menu item information to.
    /// </param>
    //*************************************************************************

    protected void
    CheckForCustomMenuItems
    (
        Range oVertexRange,
        Object [,] aoValues,
        Int32 iRowOneBased,
        KeyValuePair<Int32, Int32> [] aoCustomMenuItemPairIndexes,
        IVertex oVertex
    )
    {
        Debug.Assert(oVertexRange != null);
        Debug.Assert(aoValues != null);
        Debug.Assert(iRowOneBased >= 1);
        Debug.Assert(aoCustomMenuItemPairIndexes != null);
        Debug.Assert(oVertex != null);
        AssertValid();

        // List of string pairs, one pair for each custom menu item to add to
        // the vertex's context menu in the graph.  The key is the custom menu
        // item text and the value is the custom menu item action.

        List<KeyValuePair<String, String>> oCustomMenuItemInformation =
            new List<KeyValuePair<String, String>>();

        // Loop through the column index pairs.

        foreach (KeyValuePair<Int32, Int32> oCustomMenuItemPairIndex in
            aoCustomMenuItemPairIndexes)
        {
            String sCustomMenuItemText, sCustomMenuItemAction;

            // Both the menu item text and menu item action must be specified. 
            // Skip the pair if either is missing.

            if (
                !ExcelUtil.TryGetNonEmptyStringFromCell(aoValues,
                    iRowOneBased, oCustomMenuItemPairIndex.Key,
                    out sCustomMenuItemText)
                ||
                !ExcelUtil.TryGetNonEmptyStringFromCell(aoValues,
                    iRowOneBased, oCustomMenuItemPairIndex.Value,
                    out sCustomMenuItemAction)
                )
            {
                continue;
            }

            Int32 iCustomMenuItemTextLength = sCustomMenuItemText.Length;

            if (iCustomMenuItemTextLength > MaximumCustomMenuItemTextLength)
            {
                Range oInvalidCell = (Range)oVertexRange.Cells[
                    iRowOneBased, oCustomMenuItemPairIndex.Key];

                OnWorkbookFormatError( String.Format(

                    "The cell {0} contains custom menu item text that is {1}"
                    + " characters long.  Custom menu item text can't be"
                    + " longer than {2} characters."
                    ,
                    ExcelUtil.GetRangeAddress(oInvalidCell),
                    iCustomMenuItemTextLength,
                    MaximumCustomMenuItemTextLength
                    ),

                    oInvalidCell
                    );
            }

            oCustomMenuItemInformation.Add( new KeyValuePair<String, String>(
                sCustomMenuItemText, sCustomMenuItemAction) );
        }

        if (oCustomMenuItemInformation.Count > 0)
        {
            oVertex.SetValue( ReservedMetadataKeys.CustomContextMenuItems,
                oCustomMenuItemInformation.ToArray() );
        }
    }

    //*************************************************************************
    //  Method: SetUnsetVertexOrders()
    //
    /// <summary>
    /// Sets a default layout order for each vertex that doesn't already
    /// specify one.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// Graph containing the vertices.
    /// </param>
    //*************************************************************************

    protected void
    SetUnsetVertexOrders
    (
        IGraph oGraph
    )
    {
        Debug.Assert(oGraph != null);
        AssertValid();

        foreach (IVertex oVertex in oGraph.Vertices)
        {
            if ( !oVertex.ContainsKey(
                ReservedMetadataKeys.SortableLayoutOrder) )
            {
                // This causes SortableLayoutBase to put the vertex at the end
                // of the sort order.

                oVertex.SetValue(
                    ReservedMetadataKeys.SortableLayoutOrder,
                    Single.MaxValue);
            }
        }
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

    /// Maximum length of custom menu item text.

    protected const Int32 MaximumCustomMenuItemTextLength = 50;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)


    //*************************************************************************
    //  Embedded class: VertexTableColumnIndexes
    //
    /// <summary>
    /// Contains the one-based indexes of the columns in the optional vertex
    /// table.
    /// </summary>
    //*************************************************************************

    public class VertexTableColumnIndexes
    {
        /// Name of the vertex, required if any of the other columns are to be
        /// used.

        public Int32 VertexName;

        /// The vertex's optional color.

        public Int32 Color;

        /// The vertex's optional shape.

        public Int32 Shape;

        /// The vertex's optional radius.

        public Int32 Radius;

        /// The vertex's optional image URI.

        public Int32 ImageUri;

        /// The vertex's optional label.

        public Int32 Label;

        /// The vertex's optional label fill color.

        public Int32 LabelFillColor;

        /// The vertex's optional label position.

        public Int32 LabelPosition;

        /// The vertex's optional alpha.

        public Int32 Alpha;

        /// The vertex's optional tooltip.

        public Int32 ToolTip;

        /// The vertex's optional visibility.

        public Int32 Visibility;

        /// The vertex's optional sort order.

        public Int32 Order;

        /// The vertex's optional x-coordinate.

        public Int32 X;

        /// The vertex's optional y-coordinate.

        public Int32 Y;

        /// The vertex's optional polar R coordinate.

        public Int32 PolarR;

        /// The vertex's optional polar angle coordinate.

        public Int32 PolarAngle;

        /// The vertex's optional "lock vertex location" boolean flag.

        public Int32 Locked;

        /// The vertex's ID, which is filled in by ReadWorksheet().

        public Int32 ID;

        /// The vertex's optional "mark vertex" boolean flag.

        public Int32 Marked;
    }
}

}
