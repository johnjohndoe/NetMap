
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: WorksheetNames
//
/// <summary>
/// Provides the names of the Excel worksheets used by the add-in.
/// </summary>
///
/// <remarks>
/// All worksheet names are available as public constants.
/// </remarks>
//*****************************************************************************

public static class WorksheetNames
{
    /// <summary>
    /// Name of the required worksheet that contains edge data.
    /// </summary>

    public const String Edges = "Edges";

    /// <summary>
    /// Name of the optional worksheet that contains vertex data.
    /// </summary>

    public const String Vertices = "Vertices";

    /// <summary>
    /// Name of the optional worksheet that contains image data.
    /// </summary>

    public const String Images = "Images";

    /// <summary>
    /// Name of the optional worksheet that contains cluster data.
    /// </summary>

    public const String Clusters = "Clusters";

    /// <summary>
    /// Name of the optional worksheet that contains cluster-vertex data.
    /// </summary>

    public const String ClusterVertices = "Cluster Vertices";

    /// <summary>
    /// Name of the optional worksheet that contains overall graph metrics.
    /// </summary>

    public const String OverallMetrics = "Overall Metrics";

    /// <summary>
    /// Name of the optional worksheet that contains miscellaneous information.
    /// </summary>

    public const String Miscellaneous = "Misc";
}


//*****************************************************************************
//  Class: TableNames
//
/// <summary>
/// Provides the names of the Excel tables (ListObjects) used by the add-in.
/// </summary>
///
/// <remarks>
/// All table names are available as public constants.
/// </remarks>
//*****************************************************************************

public static class TableNames
{
    /// <summary>
    /// Name of the required table that contains edge data.
    /// </summary>

    public const String Edges = "Edges";

    /// <summary>
    /// Name of the optional table that contains vertex data.
    /// </summary>

    public const String Vertices = "Vertices";

    /// <summary>
    /// Name of the optional table that contains image data.
    /// </summary>

    public const String Images = "Images";

    /// <summary>
    /// Name of the optional table that contains cluster data.
    /// </summary>

    public const String Clusters = "Clusters";

    /// <summary>
    /// Name of the optional table that contains vertex-to-cluster data.
    /// </summary>

    public const String ClusterVertices = "ClusterVertices";

    /// <summary>
    /// Name of the optional table that contains overall graph metrics.
    /// </summary>

    public const String OverallMetrics = "OverallMetrics";

    /// <summary>
    /// Name of the optional table that contains per-workbook settings.
    /// </summary>

    public const String PerWorkbookSettings = "PerWorkbookSettings";

    /// <summary>
    /// Name of the optional table that contains dynamic filter settings.
    /// </summary>

    public const String DynamicFilterSettings = "DynamicFilterSettings";
}


//*****************************************************************************
//  Class: EdgeTableColumnNames
//
/// <summary>
/// Provides the names of the columns in the edge table.
/// </summary>
///
/// <remarks>
/// All column names are available as public constants.
/// </remarks>
//*****************************************************************************

public static class EdgeTableColumnNames
{
    /// <summary>
    /// Name of the required table column containing the edge's first vertex.
    /// </summary>

    public const String Vertex1Name = "Vertex 1";

    /// <summary>
    /// Name of the required table column containing the edge's second vertex.
    /// </summary>

    public const String Vertex2Name = "Vertex 2";

    /// <summary>
    /// Name of the optional table column containing the edge's color.
    /// </summary>

    public const String Color = "Color";

    /// <summary>
    /// Name of the optional table column containing the edge's width.
    /// </summary>

    public const String Width = "Width";

    /// <summary>
    /// Name of the optional table column containing the edge's alpha, from
    /// 0 (transparent) to 10 (opaque).
    /// </summary>

    public const String Alpha = "Opacity";

    /// <summary>
    /// Name of the optional table column containing the edge's visibility.
    /// </summary>

    public const String Visibility = "Visibility";

    /// <summary>
    /// Name of the optional table column containing the edge's label.
    /// </summary>

    public const String Label = "Label";

    /// <summary>
    /// Name of the optional table column containing the edge weight.  This
    /// gets added to the table on demand by various classes.
    /// </summary>

    public const String EdgeWeight = "Edge Weight";

    // IMPORTANT NOTES:
    //
    // 1. If a new column name is added, AutoFillEdgeColumnComboBox may need to
    //    be modified to exclude the new column name.
    //
    // 2. If the new column is part of a column group, ColumnGroupManager must
    //    be modified.
}


//*****************************************************************************
//  Class: VertexTableColumnNames
//
/// <summary>
/// Provides the names of the columns in the vertex table.
/// </summary>
///
/// <remarks>
/// All column names are available as public constants.
/// </remarks>
//*****************************************************************************

public static class VertexTableColumnNames
{
    /// <summary>
    /// Name of the optional table column containing the vertex name.
    /// </summary>

    public const String VertexName = "Vertex";

    /// <summary>
    /// Name of the optional table column containing the vertex's color.
    /// </summary>

    public const String Color = "Color";

    /// <summary>
    /// Name of the optional table column containing the vertex's shape.
    /// </summary>

    public const String Shape = "Shape";

    /// <summary>
    /// Name of the optional table column containing the vertex's radius.  Note
    /// that "radius" would more accurately be called "size" and is called that
    /// in the UI, but that changing "radius" to "size" throughout the source
    /// code is too difficult.  Thus, the source code uses "radius" but the UI
    /// uses "size".
    /// </summary>

    public const String Radius = "Size";

    /// <summary>
    /// Old name of the radius column.  When an old workbook is opened, the
    /// old name gets changed to the new name.  Therefore, the old name doesn't
    /// need to be used anywhere outside of the workbook opening code.
    /// </summary>

    public const String RadiusOld = "Radius";

    /// <summary>
    /// Name of the optional table column containing the vertex's image key.
    /// </summary>

    public const String ImageKey = "Image ID";

    /// <summary>
    /// Name of the optional table column containing the vertex's primary
    /// label.
    /// </summary>

    public const String PrimaryLabel = "Primary Label";

    /// <summary>
    /// Name of the optional table column containing the vertex's primary
    /// label fill color.
    /// </summary>

    public const String PrimaryLabelFillColor = "Primary Label Fill Color";

    /// <summary>
    /// Name of the optional table column containing the vertex's secondary
    /// label.
    /// </summary>

    public const String SecondaryLabel = "Secondary Label";

    /// <summary>
    /// Name of the optional table column containing the vertex's alpha, from
    /// 0 (transparent) to 10 (opaque).
    /// </summary>

    public const String Alpha = "Opacity";

    /// <summary>
    /// Name of the optional table column containing the vertex's tooltip.
    /// </summary>

    public const String ToolTip = "Tooltip";

    /// <summary>
    /// Name of the optional table column that specifies how the vertex should
    /// be drawn.
    /// </summary>

    public const String VertexDrawingPrecedence = "What to Show";

    /// <summary>
    /// Name of the optional table column containing the vertex's visibility.
    /// </summary>

    public const String Visibility = "Visibility";

    /// <summary>
    /// Name of the optional table column containing the vertex's layout order.
    /// </summary>

    public const String LayoutOrder = "Layout Order";

    /// <summary>
    /// Name of the optional table column containing a boolean flag indicating
    /// whether the vertex should be locked at its current location.
    /// </summary>

    public const String Locked = "Locked?";

    /// <summary>
    /// Name of the optional table column containing the vertex's x-coordinate.
    /// </summary>

    public const String X = "X";

    /// <summary>
    /// Name of the optional table column containing the vertex's y-coordinate.
    /// </summary>

    public const String Y = "Y";

    /// <summary>
    /// Name of the optional table column containing the vertex's polar R
    /// coordinate.
    /// </summary>

    public const String PolarR = "Polar R";

    /// <summary>
    /// Name of the optional table column containing the vertex's polar angle
    /// coordinate.
    /// </summary>

    public const String PolarAngle = "Polar Angle";

    /// <summary>
    /// Name of the optional table column containing the "this row is marked"
    /// flag.  This gets added to the table on demand when the user marks one
    /// or more vertices in the graph.
    /// </summary>

    public const String Marked = "Marked?";

    /// <summary>
    /// Name of the optional table column containing an image of the vertex's
    /// subgraph.  This gets added to the table on demand by <see
    /// cref="TableImagePopulator" />.
    /// </summary>

    public const String SubgraphImage = "Subgraph";

    /// <summary>
    /// Name of the optional table column containing the text of a custom menu
    /// item to add to the vertex context menu in the graph.  "Base" means that
    /// there can be more than one such column.  Additional columns have "2",
    /// "3", and so on appended to the base column name.
    /// </summary>

    public const String CustomMenuItemTextBase =
        "Custom Menu Item Text";

    /// <summary>
    /// Name of the optional table column containing the action to take when
    /// the menu item corresponding to <see cref="CustomMenuItemTextBase" /> is
    /// selected.  "Base" means that there can be more than one such column.
    /// Additional columns have "2", "3", and so on appended to the base
    /// column name.
    /// </summary>

    public const String CustomMenuItemActionBase =
        "Custom Menu Item Action";

    /// <summary>
    /// Name of the optional table column containing the vertex's in-degree.
    /// This gets added to the table on demand by <see
    /// cref="VertexDegreeCalculator2" />.
    /// </summary>

    public const String InDegree = "In-Degree";

    /// <summary>
    /// Name of the optional table column containing the vertex's out-degree.
    /// This gets added to the table on demand by <see
    /// cref="VertexDegreeCalculator2" />.
    /// </summary>

    public const String OutDegree = "Out-Degree";

    /// <summary>
    /// Name of the optional table column containing the vertex's degree.  This
    /// gets added to the table on demand by <see
    /// cref="VertexDegreeCalculator2" />.
    /// </summary>

    public const String Degree = "Degree";

    /// <summary>
    /// Name of the optional table column containing the vertex's betweenness
    /// centrality.  This gets added to the table on demand by <see
    /// cref="BetweennessCentralityCalculator2" />.
    /// </summary>

    public const String BetweennessCentrality =
        "Betweenness Centrality";

    /// <summary>
    /// Name of the optional table column containing the vertex's closeness
    /// centrality.  This gets added to the table on demand by <see
    /// cref="ClosenessCentralityCalculator2" />.
    /// </summary>

    public const String ClosenessCentrality =
        "Closeness Centrality";

    /// <summary>
    /// Name of the optional table column containing the vertex's eigenvector
    /// centrality.  This gets added to the table on demand by <see
    /// cref="EigenvectorCentralityCalculator2" />.
    /// </summary>

    public const String EigenvectorCentrality =
        "Eigenvector Centrality";

    /// <summary>
    /// Name of the optional table column containing the vertex's clustering
    /// coefficient.  This gets added to the table on demand by <see
    /// cref="ClusteringCoefficientCalculator2" />.
    /// </summary>

    public const String ClusteringCoefficient =
        "Clustering Coefficient";

    // IMPORTANT NOTES:
    //
    // 1. If a new column name is added, AutoFillVertexColumnComboBox may need
    //    to be modified to exclude the new column name.
    //
    // 2. If the new column is part of a column group, ColumnGroupManager must
    //    be modified.
}


//*****************************************************************************
//  Class: ImageTableColumnNames
//
/// <summary>
/// Provides the names of the columns in the image table.
/// </summary>
///
/// <remarks>
/// All column names are available as public constants.
/// </remarks>
//*****************************************************************************

public static class ImageTableColumnNames
{
    /// <summary>
    /// Name of the optional table column containing the unique key for the
    /// image.
    /// </summary>

    public const String Key = "Image ID";

    /// <summary>
    /// Name of the optional table column containing the full path to the
    /// image file.
    /// </summary>

    public const String FilePath = "Image File Path";
}


//*****************************************************************************
//  Class: ClusterTableColumnNames
//
/// <summary>
/// Provides the names of the columns in the cluster table.
/// </summary>
///
/// <remarks>
/// All column names are available as public constants.
/// </remarks>
//*****************************************************************************

public static class ClusterTableColumnNames
{
    /// <summary>
    /// Name of the optional table column containing the cluster name.
    /// </summary>

    public const String Name = "Cluster";

    /// <summary>
    /// Name of the optional table column containing the color of the vertices
    /// in the cluster.
    /// </summary>

    public const String VertexColor = "Vertex Color";

    /// <summary>
    /// Name of the optional table column containing the shape of the vertices
    /// in the cluster.
    /// </summary>

    public const String VertexShape = "Vertex Shape";
}


//*****************************************************************************
//  Class: ClusterVertexTableColumnNames
//
/// <summary>
/// Provides the names of the columns in the cluster-vertex table.
/// </summary>
///
/// <remarks>
/// All column names are available as public constants.
/// </remarks>
//*****************************************************************************

public static class ClusterVertexTableColumnNames
{
    /// <summary>
    /// Name of the optional table column containing the cluster name.
    /// </summary>

    public const String ClusterName = "Cluster";

    /// <summary>
    /// Name of the optional table column containing the vertex name.
    /// </summary>

    public const String VertexName = "Vertex";
}


//*****************************************************************************
//  Class: OverallMetricsTableColumnNames
//
/// <summary>
/// Provides the names of the columns in the overall metrics table.
/// </summary>
///
/// <remarks>
/// All column names are available as public constants.
/// </remarks>
//*****************************************************************************

public static class OverallMetricsTableColumnNames
{
    /// <summary>
    /// Name of the optional table column containing the metric name.
    /// </summary>

    public const String Name = "Metric";

    /// <summary>
    /// Name of the optional table column containing the metric value.
    /// </summary>

    public const String Value = "Value";
}


//*****************************************************************************
//  Class: PerWorkbookSettingsTableColumnNames
//
/// <summary>
/// Provides the names of the columns in the per-workbook settings table.
/// </summary>
///
/// <remarks>
/// All column names are available as public constants.
/// </remarks>
//*****************************************************************************

public static class PerWorkbookSettingsTableColumnNames
{
    /// <summary>
    /// Name of the optional table column containing the setting name.
    /// </summary>

    public const String Name = "Per-Workbook Setting";

    /// <summary>
    /// Name of the optional table column containing the setting value.
    /// </summary>

    public const String Value = "Value";
}


//*****************************************************************************
//  Class: DynamicFilterSettingsTableColumnNames
//
/// <summary>
/// Provides the names of the columns in the dynamic filter settings table.
/// </summary>
///
/// <remarks>
/// All column names are available as public constants.
/// </remarks>
//*****************************************************************************

public static class DynamicFilterSettingsTableColumnNames
{
    /// <summary>
    /// Name of the table column containing the name of the table containing
    /// the column being filtered on.
    /// </summary>

    public const String TableName = "Table Name";

    /// <summary>
    /// Name of the table column containing the name of the column being
    /// filtered on.
    /// </summary>

    public const String ColumnName = "Column Name";

    /// <summary>
    /// Name of the table column containing the minimum value to show.
    /// </summary>

    public const String SelectedMinimum = "Selected Minimum";

    /// <summary>
    /// Name of the table column containing the maximum value to show.
    /// </summary>

    public const String SelectedMaximum = "Selected Maximum";
}


//*****************************************************************************
//  Class: CommonTableColumnNames
//
/// <summary>
/// Provides the names of the columns that are present in multiple tables.
/// </summary>
///
/// <remarks>
/// All common column names are available as public constants.
/// </remarks>
//*****************************************************************************

public static class CommonTableColumnNames
{
    /// <summary>
    /// Name of the optional table column containing the row's unique ID.
    /// </summary>

    public const String ID = "ID";

    /// <summary>
    /// Name of the optional table column indicating to users where their own
    /// columns can be added.
    /// </summary>

    public const String AddColumnsHere = "Add Your Own Columns Here";

    /// <summary>
    /// Name of the optional table column used by the dynamic filter feature.
    /// </summary>

    public const String DynamicFilter = "Dynamic Filter";
}


//*****************************************************************************
//  Class: CellStyleNames
//
/// <summary>
/// Names of cell styles that get applied programatically.
/// </summary>
///
/// <remarks>
/// This class includes standard Excel styles as well as custom styles defined
/// in the NodeXL template.
/// </remarks>
//*****************************************************************************

public static class CellStyleNames
{
    /// <summary>
    /// Style applied to graph metric cells for which graph metric values were
    /// successfully calculated.  This is a custom NodeXL style.
    /// </summary>

    public const String GraphMetricGood = "NodeXL Graph Metric";

    /// <summary>
    /// Style applied to graph metric cells for which graph metric values could
    /// not be calculated.  This is a standard Excel style.
    /// </summary>

    public const String GraphMetricBad = "Bad";
}

}
