
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Media;
using Microsoft.Office.Interop.Excel;
using System.Linq;
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
    /// Reads the vertex worksheet and adds the contents to a graph.
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
    /// <paramref name="readWorkbookContext" />.EdgeRowIDDictionary.
    /// Otherwise, a <see cref="WorkbookFormatException" /> is thrown.
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

            Boolean bLayoutOrderSet, bToolTipSet;

            try
            {
                ReadVertexTable(oVertexTable, readWorkbookContext, graph,
                    out bLayoutOrderSet, out bToolTipSet);
            }
            finally
            {
                ExcelColumnHider.RestoreHiddenColumns(oVertexTable,
                    oHiddenColumns);
            }

            if (bLayoutOrderSet)
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

            if (bToolTipSet)
            {
                graph.SetValue(ReservedMetadataKeys.ToolTipSet, null);
            }
        }
    }

    //*************************************************************************
    //  Method: ReadVertexTable()
    //
    /// <summary>
    /// Reads the vertex table and adds the contents to a graph.
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
    ///
    /// <param name="bLayoutOrderSet">
    /// Gets set to true if a vertex layout order was specified for at least
    /// one vertex, or false otherwise.
    /// </param>
    ///
    /// <param name="bToolTipSet">
    /// Gets set to true if a tooltip was specified for at least one vertex, or
    /// false otherwise.
    /// </param>
    //*************************************************************************

    protected void
    ReadVertexTable
    (
        ListObject oVertexTable,
        ReadWorkbookContext oReadWorkbookContext,
        IGraph oGraph,
        out Boolean bLayoutOrderSet,
        out Boolean bToolTipSet
    )
    {
        Debug.Assert(oVertexTable != null);
        Debug.Assert(oReadWorkbookContext != null);
        Debug.Assert(oGraph != null);
        AssertValid();

        bLayoutOrderSet = bToolTipSet = false;

        if (GetTableColumnIndex(oVertexTable,
            VertexTableColumnNames.VertexName, false) == NoSuchColumn)
        {
            // Nothing can be done without vertex names.

            return;
        }

        Boolean bReadAllEdgeAndVertexColumns =
            oReadWorkbookContext.ReadAllEdgeAndVertexColumns;

        if (oReadWorkbookContext.FillIDColumns)
        {
            FillIDColumn(oVertexTable);
        }

        // Get the names of all the column pairs that are used to add custom
        // menu items to the vertex context menu in the graph.

        TableColumnAdder oTableColumnAdder = new TableColumnAdder();

        ICollection< KeyValuePair<String, String> > aoCustomMenuItemPairNames =
            oTableColumnAdder.GetColumnPairNames(oVertexTable,
                VertexTableColumnNames.CustomMenuItemTextBase,
                VertexTableColumnNames.CustomMenuItemActionBase);

        IVertexCollection oVertices = oGraph.Vertices;

        Dictionary<String, IVertex> oVertexNameDictionary =
            oReadWorkbookContext.VertexNameDictionary;

        Dictionary<Int32, IIdentityProvider> oEdgeRowIDDictionary =
            oReadWorkbookContext.EdgeRowIDDictionary;

        BooleanConverter oBooleanConverter =
            oReadWorkbookContext.BooleanConverter;

        VertexVisibilityConverter oVertexVisibilityConverter =
            new VertexVisibilityConverter();

        VertexLabelPositionConverter oVertexLabelPositionConverter =
            new VertexLabelPositionConverter();

        ExcelTableReader oExcelTableReader =
            new ExcelTableReader(oVertexTable);

        HashSet<String> oColumnNamesToExclude = new HashSet<String>(
            new String[] {
                VertexTableColumnNames.VertexName
                } );

        foreach ( ExcelTableReader.ExcelTableRow oRow in
            oExcelTableReader.GetRows() )
        {
            // Get the name of the vertex.

            String sVertexName;

            if ( !oRow.TryGetNonEmptyStringFromCell(
                VertexTableColumnNames.VertexName, out sVertexName) )
            {
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

            if ( oRow.TryGetNonEmptyStringFromCell(
                    CommonTableColumnNames.Visibility, out sVisibility) )
            {
                if ( !oVertexVisibilityConverter.TryWorkbookToGraph(
                    sVisibility, out eVisibility) )
                {
                    OnInvalidVisibility(oRow);
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
                                oEdgeRowIDDictionary.Remove(
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
                        oVertex = CreateVertex(sVertexName, oVertices,
                            oVertexNameDictionary);
                    }

                    break;

                default:

                    Debug.Assert(false);
                    break;
            }

            Debug.Assert(oVertex != null);

            // If there is an ID column, add the vertex to the vertex ID
            // dictionary and set the vertex's Tag to the ID.

            AddToRowIDDictionary(oRow, oVertex,
                oReadWorkbookContext.VertexRowIDDictionary);

            if (bReadAllEdgeAndVertexColumns)
            {
                // All columns except the vertex name should be read and stored
                // as metadata on the vertex.

                ReadAllColumns( oExcelTableReader, oRow, oVertex,
                    oColumnNamesToExclude);

                continue;
            }

            // Layout order.

            if ( ReadLayoutOrder(oRow, oVertex) )
            {
                bLayoutOrderSet = true;
            }

            // Location and Locked.

            if (!oReadWorkbookContext.IgnoreVertexLocations)
            {
                Boolean bLocationSpecified = false;

                bLocationSpecified = ReadLocation(oRow,
                    oReadWorkbookContext.VertexLocationConverter, oVertex);

                ReadLocked(oRow, oBooleanConverter, bLocationSpecified,
                    oVertex);
            }

            // Polar coordinates.

            ReadPolarCoordinates(oRow, oVertex);

            // Marked.

            ReadMarked(oRow, oBooleanConverter, oVertex);

            // Custom menu items.

            if (aoCustomMenuItemPairNames.Count > 0)
            {
                ReadCustomMenuItems(oRow, aoCustomMenuItemPairNames, oVertex);
            }

            // Alpha.

            ReadAlpha(oRow, oVertex);

            // Tooltip.

            if ( ReadCellAndSetMetadata(oRow, VertexTableColumnNames.ToolTip,
                oVertex, ReservedMetadataKeys.VertexToolTip) )
            {
                bToolTipSet = true;
            }

            // Label.

            if (oReadWorkbookContext.ReadVertexLabels)
            {
                ReadCellAndSetMetadata(oRow, VertexTableColumnNames.Label,
                    oVertex, ReservedMetadataKeys.PerVertexLabel);
            }

            // Label fill color.

            ReadColor(oRow, VertexTableColumnNames.LabelFillColor, oVertex,
                ReservedMetadataKeys.PerVertexLabelFillColor,
                oReadWorkbookContext.ColorConverter2);

            // Label position.

            ReadLabelPosition(oRow, oVertexLabelPositionConverter, oVertex);

            // Radius.

            Nullable<Single> oRadiusWorkbook = new Nullable<Single>();

            oRadiusWorkbook = ReadRadius(oRow,
                oReadWorkbookContext.VertexRadiusConverter, oVertex);

            // Shape.

            VertexShape eVertexShape;

            if ( !ReadShape(oRow, oVertex, out eVertexShape) )
            {
                eVertexShape = oReadWorkbookContext.DefaultVertexShape;
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
                oReadWorkbookContext.ReadVertexImages)
            {
                ReadImageUri(oRow, oVertex,
                    oReadWorkbookContext.VertexRadiusConverter,

                    oRadiusWorkbook.HasValue ? oRadiusWorkbook :
                        oReadWorkbookContext.DefaultVertexImageSize
                    );
            }

            // Color

            ReadColor(oRow, VertexTableColumnNames.Color, oVertex,
                ReservedMetadataKeys.PerColor,
                oReadWorkbookContext.ColorConverter2);
        }

        if (bReadAllEdgeAndVertexColumns)
        {
            // Store the vertex column names on the graph.

            oGraph.SetValue( ReservedMetadataKeys.AllVertexMetadataKeys,
                FilterColumnNames(oExcelTableReader, oColumnNamesToExclude) );
        }
    }

    //*************************************************************************
    //  Method: ReadLabelPosition()
    //
    /// <summary>
    /// If a label position has been specified for a vertex, sets the vertex's
    /// label position.
    /// </summary>
    ///
    /// <param name="oRow">
    /// Row containing the vertex data.
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
    ReadLabelPosition
    (
        ExcelTableReader.ExcelTableRow oRow,
        VertexLabelPositionConverter oVertexLabelPositionConverter,
        IVertex oVertex
    )
    {
        Debug.Assert(oRow != null);
        Debug.Assert(oVertex != null);
        Debug.Assert(oVertexLabelPositionConverter != null);
        AssertValid();

        String sLabelPosition;

        if ( !oRow.TryGetNonEmptyStringFromCell(
            VertexTableColumnNames.LabelPosition, out sLabelPosition) )
        {
            return;
        }

        VertexLabelPosition eLabelPosition;

        if ( !oVertexLabelPositionConverter.TryWorkbookToGraph(sLabelPosition,
            out eLabelPosition) )
        {
            OnWorkbookFormatErrorWithDropDown(oRow,
                VertexTableColumnNames.LabelPosition, "label position");
        }

        oVertex.SetValue( ReservedMetadataKeys.PerVertexLabelPosition,
            eLabelPosition);
    }

    //*************************************************************************
    //  Method: ReadRadius()
    //
    /// <summary>
    /// If a radius has been specified for a vertex, sets the vertex's radius.
    /// </summary>
    ///
    /// <param name="oRow">
    /// Row containing the vertex data.
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
    ReadRadius
    (
        ExcelTableReader.ExcelTableRow oRow,
        VertexRadiusConverter oVertexRadiusConverter,
        IVertex oVertex
    )
    {
        Debug.Assert(oRow != null);
        Debug.Assert(oVertex != null);
        Debug.Assert(oVertexRadiusConverter != null);
        AssertValid();

        String sRadius;

        if ( !oRow.TryGetNonEmptyStringFromCell(VertexTableColumnNames.Radius,
            out sRadius) )
        {
            return ( new Nullable<Single>() );
        }

        Single fRadius;

        if ( !Single.TryParse(sRadius, out fRadius) )
        {
            Range oInvalidCell = oRow.GetRangeForCell(
                VertexTableColumnNames.Radius);

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
    //  Method: ReadShape()
    //
    /// <summary>
    /// If a shape has been specified for a vertex, sets the vertex's shape.
    /// </summary>
    ///
    /// <param name="oRow">
    /// Row containing the vertex data.
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
    ReadShape
    (
        ExcelTableReader.ExcelTableRow oRow,
        IVertex oVertex,
        out VertexShape eShape
    )
    {
        Debug.Assert(oRow != null);
        Debug.Assert(oVertex != null);
        AssertValid();

        if ( TryGetVertexShape(oRow, VertexTableColumnNames.Shape,
            out eShape) )
        {
            oVertex.SetValue(ReservedMetadataKeys.PerVertexShape, eShape);
            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: ReadImageUri()
    //
    /// <summary>
    /// If an image URI has been specified for a vertex, sets the vertex's
    /// image.
    /// </summary>
    ///
    /// <param name="oRow">
    /// Row containing the vertex data.
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
    ReadImageUri
    (
        ExcelTableReader.ExcelTableRow oRow,
        IVertex oVertex,
        VertexRadiusConverter oVertexRadiusConverter,
        Nullable<Single> oVertexImageSize
    )
    {
        Debug.Assert(oRow != null);
        Debug.Assert(oVertex != null);
        Debug.Assert(oVertexRadiusConverter != null);
        AssertValid();

        String sImageUri;

        if ( !oRow.TryGetNonEmptyStringFromCell(
            VertexTableColumnNames.ImageUri, out sImageUri) )
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

            Range oCell = oRow.GetRangeForCell(
                VertexTableColumnNames.ImageUri);

            String sWorkbookPath =
                ( (Workbook)(oCell.Worksheet.Parent) ).Path;

            if ( !String.IsNullOrEmpty(sWorkbookPath) )
            {
                sImageUri = Path.Combine(sWorkbookPath, sImageUri);
            }
            else
            {
                OnWorkbookFormatError( String.Format(

                    "The image file path specified in cell {0} is a relative"
                    + " path.  Relative paths must be relative to the saved"
                    + " workbook file, but the workbook hasn't been saved yet."
                    + "  Either save the workbook or change the image file to"
                    + " an absolute path, such as \"C:\\MyImages\\Image.jpg\"."
                    ,
                    ExcelUtil.GetRangeAddress(oCell)
                    ),

                    oCell
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
    //  Method: ReadLayoutOrder()
    //
    /// <summary>
    /// If a layout order has been specified for a vertex, sets the vertex's
    /// layout order.
    /// </summary>
    ///
    /// <param name="oRow">
    /// Row containing the vertex data.
    /// </param>
    ///
    /// <param name="oVertex">
    /// Vertex to set the layout order on.
    /// </param>
    ///
    /// <returns>
    /// true if a layout order was specified.
    /// </returns>
    //*************************************************************************

    protected Boolean
    ReadLayoutOrder
    (
        ExcelTableReader.ExcelTableRow oRow,
        IVertex oVertex
    )
    {
        Debug.Assert(oRow != null);
        Debug.Assert(oVertex != null);
        AssertValid();

        String sOrder;

        if ( !oRow.TryGetNonEmptyStringFromCell(
            VertexTableColumnNames.LayoutOrder, out sOrder) )
        {
            return (false);
        }

        Single fOrder;

        if ( !Single.TryParse(sOrder, out fOrder) )
        {
            Range oInvalidCell = oRow.GetRangeForCell(
                VertexTableColumnNames.LayoutOrder);

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
    //  Method: ReadLocation()
    //
    /// <summary>
    /// If a location has been specified for a vertex, sets the vertex's
    /// location.
    /// </summary>
    ///
    /// <param name="oRow">
    /// Row containing the vertex data.
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
    ReadLocation
    (
        ExcelTableReader.ExcelTableRow oRow,
        VertexLocationConverter oVertexLocationConverter,
        IVertex oVertex
    )
    {
        Debug.Assert(oRow != null);
        Debug.Assert(oVertexLocationConverter != null);
        Debug.Assert(oVertex != null);
        AssertValid();

        String sX;

        Boolean bHasX = oRow.TryGetNonEmptyStringFromCell(
            VertexTableColumnNames.X, out sX);

        String sY;

        Boolean bHasY = oRow.TryGetNonEmptyStringFromCell(
            VertexTableColumnNames.Y, out sY);

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

            Range oInvalidCell = oRow.GetRangeForCell(
                VertexTableColumnNames.X);

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
    //  Method: ReadPolarCoordinates()
    //
    /// <summary>
    /// If polar coordinates have been specified for a vertex, sets the
    /// vertex's polar coordinates.
    /// </summary>
    ///
    /// <param name="oRow">
    /// Row containing the vertex data.
    /// </param>
    ///
    /// <param name="oVertex">
    /// Vertex to set the polar coordinates on.
    /// </param>
    ///
    /// <returns>
    /// true if a location was specified.
    /// </returns>
    //*************************************************************************

    protected Boolean
    ReadPolarCoordinates
    (
        ExcelTableReader.ExcelTableRow oRow,
        IVertex oVertex
    )
    {
        Debug.Assert(oRow != null);
        Debug.Assert(oVertex != null);
        AssertValid();

        String sR;

        Boolean bHasR = oRow.TryGetNonEmptyStringFromCell(
            VertexTableColumnNames.PolarR, out sR);

        String sAngle;

        Boolean bHasAngle = oRow.TryGetNonEmptyStringFromCell(
            VertexTableColumnNames.PolarAngle, out sAngle);

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

            Range oInvalidCell = oRow.GetRangeForCell(
                VertexTableColumnNames.PolarR);

            OnWorkbookFormatError( String.Format(

                "There is a problem with the vertex polar coordinates at {0}."
                + " If you enter polar coordinates, they must include both"
                + " {1} and {2} numbers.  Any numbers are acceptable."
                + "\r\n\r\n"
                + "Polar coordinates are used only when a Layout of Polar"
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
    //  Method: ReadLocked()
    //
    /// <summary>
    /// If a locked flag has been specified for a vertex, sets the vertex's
    /// locked flag.
    /// </summary>
    ///
    /// <param name="oRow">
    /// Row containing the vertex data.
    /// </param>
    ///
    /// <param name="oBooleanConverter">
    /// Object that converts a Boolean between values used in the Excel
    /// workbook and values used in the NodeXL graph.
    /// </param>
    ///
    /// <param name="bLocationSpecified">
    /// true if a location was specified for the vertex.
    /// </param>
    ///
    /// <param name="oVertex">
    /// Vertex to set the lock flag on.
    /// </param>
    ///
    /// <remarks>
    /// "Locked" means "prevent the layout algorithm from moving the vertex."
    /// </remarks>
    //*************************************************************************

    protected void
    ReadLocked
    (
        ExcelTableReader.ExcelTableRow oRow,
        BooleanConverter oBooleanConverter,
        Boolean bLocationSpecified,
        IVertex oVertex
    )
    {
        Debug.Assert(oRow != null);
        Debug.Assert(oBooleanConverter != null);
        Debug.Assert(oVertex != null);
        AssertValid();

        Boolean bLocked;

        if ( !TryGetBoolean(oRow, VertexTableColumnNames.Locked,
            oBooleanConverter, out bLocked) )
        {
            return;
        }

        if (bLocked && !bLocationSpecified)
        {
            Range oInvalidCell = oRow.GetRangeForCell(
                VertexTableColumnNames.Locked);

            OnWorkbookFormatError( String.Format(

                "The cell {0} indicates that the vertex should be locked,"
                + " but the vertex has no X and Y location values.  Either"
                + " clear the lock or specify a vertex location."
                ,
                ExcelUtil.GetRangeAddress(oInvalidCell)
                ),

                oInvalidCell
            );
        }

        oVertex.SetValue(ReservedMetadataKeys.LockVertexLocation, bLocked);
    }

    //*************************************************************************
    //  Method: ReadMarked()
    //
    /// <summary>
    /// If a marked flag has been specified for a vertex, sets the vertex's
    /// marked flag.
    /// </summary>
    ///
    /// <param name="oRow">
    /// Row containing the vertex data.
    /// </param>
    ///
    /// <param name="oBooleanConverter">
    /// Object that converts a Boolean between values used in the Excel
    /// workbook and values used in the NodeXL graph.
    /// </param>
    ///
    /// <param name="oVertex">
    /// Vertex to set the marked flag on.
    /// </param>
    ///
    /// <remarks>
    /// "Marking" is something the user does for himself.  A marked vertex
    /// doesn't behave differently from an unmarked vertex.)
    /// </remarks>
    //*************************************************************************

    protected void
    ReadMarked
    (
        ExcelTableReader.ExcelTableRow oRow,
        BooleanConverter oBooleanConverter,
        IVertex oVertex
    )
    {
        Debug.Assert(oRow != null);
        Debug.Assert(oBooleanConverter != null);
        Debug.Assert(oVertex != null);
        AssertValid();

        Boolean bMarked;

        if ( !TryGetBoolean(oRow, VertexTableColumnNames.Marked,
            oBooleanConverter, out bMarked) )
        {
            return;
        }

        oVertex.SetValue(ReservedMetadataKeys.Marked, bMarked);

        return;
    }

    //*************************************************************************
    //  Method: ReadCustomMenuItems()
    //
    /// <summary>
    /// If custom menu items have been specified for a vertex, stores the
    /// custom menu item information in the vertex.
    /// </summary>
    ///
    /// <param name="oRow">
    /// Row containing the vertex data.
    /// </param>
    ///
    /// <param name="aoCustomMenuItemPairNames">
    /// Collection of pairs of column names, one element for each pair of
    /// columns that are used to add custom menu items to the vertex context
    /// menu in the graph.  They key is the name of the custom menu item text
    /// and the value is the name of the custom menu item action.
    /// </param>
    ///
    /// <param name="oVertex">
    /// Vertex to add custom menu item information to.
    /// </param>
    //*************************************************************************

    protected void
    ReadCustomMenuItems
    (
        ExcelTableReader.ExcelTableRow oRow,
        ICollection< KeyValuePair<String, String> > aoCustomMenuItemPairNames,
        IVertex oVertex
    )
    {
        Debug.Assert(oRow != null);
        Debug.Assert(aoCustomMenuItemPairNames != null);
        Debug.Assert(oVertex != null);
        AssertValid();

        // List of string pairs, one pair for each custom menu item to add to
        // the vertex's context menu in the graph.  The key is the custom menu
        // item text and the value is the custom menu item action.

        List<KeyValuePair<String, String>> oCustomMenuItemInformation =
            new List<KeyValuePair<String, String>>();

        foreach (KeyValuePair<String, String> oPairNames in
            aoCustomMenuItemPairNames)
        {
            String sCustomMenuItemText, sCustomMenuItemAction;

            // Both the menu item text and menu item action must be specified. 
            // Skip the pair if either is missing.

            if (
                !oRow.TryGetNonEmptyStringFromCell(oPairNames.Key,
                    out sCustomMenuItemText)
                ||
                !oRow.TryGetNonEmptyStringFromCell(oPairNames.Value,
                    out sCustomMenuItemAction)
                )
            {
                continue;
            }

            Int32 iCustomMenuItemTextLength = sCustomMenuItemText.Length;

            if (iCustomMenuItemTextLength > MaximumCustomMenuItemTextLength)
            {
                Range oInvalidCell = oRow.GetRangeForCell(oPairNames.Key);

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
}

}
