
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: EdgeTableColumnWidths
//
/// <summary>
/// Provides the widths of the dynamically-added columns in the edge table.
/// </summary>
///
/// <remarks>
/// Most dynamically-added columns are auto-sized and don't need an explicit
/// width.  This class contains the widths of the columns that are NOT
/// auto-sized.
/// </remarks>
//*****************************************************************************

public static class EdgeTableColumnWidths
{
	// (There are no such columns.)
}


//*****************************************************************************
//  Class: VertexTableColumnWidths
//
/// <summary>
/// Provides the widths of the dynamically-added columns in the vertex table.
/// </summary>
///
/// <remarks>
/// Most dynamically-added columns are auto-sized and don't need an explicit
/// width.  This class contains the widths of the columns that are NOT
/// auto-sized.
/// </remarks>
//*****************************************************************************

public static class VertexTableColumnWidths
{
	/// <summary>
	/// Width of the custom menu item text columns, in characters.
	/// </summary>

	public const Single CustomMenuItemText = 14.8F;

	/// <summary>
	/// Width of the custom menu item action columns, in characters.
	/// </summary>

	public const Single CustomMenuItemAction = 14.8F;

	/// <summary>
	/// Width of the betweenness centrality column, in characters.
	/// </summary>

	public const Single BetweennessCentrality = 13.6F;

	/// <summary>
	/// Width of the clustering coefficient column, in characters.
	/// </summary>

	public const Single ClusteringCoefficient = 13.7F;
}

//*****************************************************************************
//  Class: ClusterTableColumnWidths
//
/// <summary>
/// Provides the widths of the dynamically-added columns in the cluster table.
/// </summary>
///
/// <remarks>
/// The widths of static columns that are included in the template are not
/// included in this class.
/// </remarks>
//*****************************************************************************

public static class ClusterTableColumnWidths
{
	// (There are no such columns.)
}


//*****************************************************************************
//  Class: ClusterVertexTableColumnWidths
//
/// <summary>
/// Provides the widths of the dynamically-added columns in the cluster-vertex
/// table.
/// </summary>
///
/// <remarks>
/// Most dynamically-added columns are auto-sized and don't need an explicit
/// width.  This class contains the widths of the columns that are NOT
/// auto-sized.
/// </remarks>
//*****************************************************************************

public static class ClusterVertexTableColumnWidths
{
	// (There are no such columns.)
}


//*****************************************************************************
//  Class: OverallMetricsTableColumnWidths
//
/// <summary>
/// Provides the widths of the dynamically-added columns in the overall metrics
/// table.
/// </summary>
///
/// <remarks>
/// Most dynamically-added columns are auto-sized and don't need an explicit
/// width.  This class contains the widths of the columns that are NOT
/// auto-sized.
/// </remarks>
//*****************************************************************************

public static class OverallMetricsTableColumnWidths
{
	/// <summary>
	/// Width of the metric name column, in characters.
	/// </summary>

	public const Single Name = 20.2F;

	/// <summary>
	/// Width of the metric value column, in characters.
	/// </summary>

	public const Single Value = 13.2F;
}

}
