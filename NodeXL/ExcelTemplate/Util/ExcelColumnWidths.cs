
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
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
    // (For some reason, autosizing the degree and out-degree columns works
    // fine, but autosizing the in-degree column results in the final "ee"
    // getting trucated by the dynamic filter down-arrow.

    /// <summary>
    /// Width of the in-degree text column, in characters.
    /// </summary>

    public const Single InDegree = 8.57F;

    /// <summary>
    /// Width of the custom menu item text columns, in characters.
    /// </summary>

    public const Single CustomMenuItemText = 25.6F;

    /// <summary>
    /// Width of the custom menu item action columns, in characters.
    /// </summary>

    public const Single CustomMenuItemAction = 27.6F;

    /// <summary>
    /// Width of the betweenness centrality column, in characters.
    /// </summary>

    public const Single BetweennessCentrality = 13.6F;

    /// <summary>
    /// Width of the eigenvector centrality column, in characters.
    /// </summary>

    public const Single EigenvectorCentrality = 13.6F;

    /// <summary>
    /// Width of the PageRank column, in characters.
    /// </summary>

    public const Single PageRank = 13.6F;

    /// <summary>
    /// Width of the closeness centrality column, in characters.
    /// </summary>

    public const Single ClosenessCentrality = 13.6F;

    /// <summary>
    /// Width of the clustering coefficient column, in characters.
    /// </summary>

    public const Single ClusteringCoefficient = 13.7F;
}

}
