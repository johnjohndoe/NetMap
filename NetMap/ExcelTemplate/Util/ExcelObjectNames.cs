
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: WorksheetNames
//
/// <summary>
/// Provides the names of the Excel worksheets used by the add-in.
/// </summary>
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
	/// Name of the optional worksheet that contains vertex-to-cluster data.
	/// </summary>

	public const String ClusterVertices = "Cluster Vertices";
}


//*****************************************************************************
//  Class: TableNames
//
/// <summary>
/// Provides the names of the Excel tables (ListObjects) used by the add-in.
/// </summary>
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
}


//*****************************************************************************
//  Class: EdgeTableColumnNames
//
/// <summary>
/// Provides the names of the columns in the edge table.
/// </summary>
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
	/// Name of the optional table column containing the vertex 1 degree.  This
	/// gets added to the table on demand by <see
	/// cref="VertexDegreeCalculator" />.
	/// </summary>

	public const String Vertex1Degree = "Vertex 1 Degree";

	/// <summary>
	/// Name of the optional table column containing the vertex 1 in-degree.
	/// This gets added to the table on demand by <see
	/// cref="VertexDegreeCalculator" />.
	/// </summary>

	public const String Vertex1InDegree = "Vertex 1 In-Degree";

	/// <summary>
	/// Name of the optional table column containing the vertex 1 out-degree.
	/// This gets added to the table on demand by <see
	/// cref="VertexDegreeCalculator" />.
	/// </summary>

	public const String Vertex1OutDegree = "Vertex 1 Out-Degree";

	/// <summary>
	/// Name of the optional table column containing the vertex 2 degree.  This
	/// gets added to the table on demand by <see
	/// cref="VertexDegreeCalculator" />.
	/// </summary>

	public const String Vertex2Degree = "Vertex 2 Degree";

	/// <summary>
	/// Name of the optional table column containing the vertex 2 in-degree.
	/// This gets added to the table on demand by <see
	/// cref="VertexDegreeCalculator" />.
	/// </summary>

	public const String Vertex2InDegree = "Vertex 2 In-Degree";

	/// <summary>
	/// Name of the optional table column containing the vertex 2 out-degree.
	/// This gets added to the table on demand by <see
	/// cref="VertexDegreeCalculator" />.
	/// </summary>

	public const String Vertex2OutDegree = "Vertex 2 Out-Degree";

	/// <summary>
	/// Name of the optional table column containing the tie strength.  This
	/// gets added to the table on demand by <see
	/// cref="AnalyzeEmailNetworkDialog" />.
	/// </summary>

	public const String TieStrength = "Tie Strength";
}


//*****************************************************************************
//  Class: VertexTableColumnNames
//
/// <summary>
/// Provides the names of the columns in the vertex table.
/// </summary>
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
	/// Name of the optional table column containing the vertex's
	/// radius.
	/// </summary>

	public const String Radius = "Radius";

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
	/// Name of the optional table column that specifies what vertex drawer
	/// should be used for the vertex.
	/// </summary>

	public const String VertexDrawerPrecedence = "What to Show";

	/// <summary>
	/// Name of the optional table column containing the vertex's visibility.
	/// </summary>

	public const String Visibility = "Visibility";

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
	/// Name of the optional table column containing the vertex's degree.  This
	/// gets added to the table on demand by <see
	/// cref="VertexDegreeCalculator" />.
	/// </summary>

	public const String Degree = "Degree";

	/// <summary>
	/// Name of the optional table column containing the vertex's in-degree.
	/// This gets added to the table on demand by <see
	/// cref="VertexDegreeCalculator" />.
	/// </summary>

	public const String InDegree = "In-Degree";

	/// <summary>
	/// Name of the optional table column containing the vertex's out-degree.
	/// This gets added to the table on demand by <see
	/// cref="VertexDegreeCalculator" />.
	/// </summary>

	public const String OutDegree = "Out-Degree";

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
	/// Name of the optional table column containing the vertex's clustering
	/// coefficient.  This gets added to the table on demand by <see
	/// cref="ClusteringCoefficientCalculator" />.
	/// </summary>

	public const String ClusteringCoefficient =
		"Clustering Coefficient";

	/// <summary>
	/// Name of the optional table column containing the vertex's betweenness
	/// centrality.  This gets added to the table on demand by <see
	/// cref="BetweennessCentralityCalculator" />.
	/// </summary>

	public const String BetweennessCentrality =
		"Betweenness Centrality";
}


//*****************************************************************************
//  Class: ImageTableColumnNames
//
/// <summary>
/// Provides the names of the columns in the image table.
/// </summary>
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
//*****************************************************************************

public static class ClusterTableColumnNames
{
	/// <summary>
	/// Name of the optional table column containing the cluster name.
	/// </summary>

	public const String ClusterName = "Cluster";
}


//*****************************************************************************
//  Class: ClusterVerticesTableColumnNames
//
/// <summary>
/// Provides the names of the columns in the cluster table.
/// </summary>
//*****************************************************************************

public static class ClusterVerticesTableColumnNames
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
//  Class: CommonTableColumnNames
//
/// <summary>
/// Provides the names of the columns that are present in multiple tables.
/// </summary>
//*****************************************************************************

public static class CommonTableColumnNames
{
	/// <summary>
	/// Name of the optional table column containing the row's unique ID.
	/// </summary>

	public const String ID = "ID";
}

}
