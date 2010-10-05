
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Enum: GraphMetrics
//
/// <summary>
/// Specifies one or more graph metrics.
/// </summary>
///
/// <remarks>
/// The flags can be ORed together.
/// </remarks>
//*****************************************************************************

[System.FlagsAttribute]

public enum
GraphMetrics
{
    /// <summary>
    /// No graph metrics.
    /// </summary>

    None = 0,

    /// <summary>
    /// In-degree.
    /// </summary>

    InDegree = 1,

    /// <summary>
    /// Out-degree.
    /// </summary>

    OutDegree = 2,

    /// <summary>
    /// Degree.
    /// </summary>

    Degree = 4,

    /// <summary>
    /// Clustering coefficient.
    /// </summary>

    ClusteringCoefficient = 8,

    /// <summary>
    /// Brandes fast centralities.
    /// </summary>

    BrandesFastCentralities = 16,

    /// <summary>
    /// Eigenvector centrality.
    /// </summary>

    EigenvectorCentrality = 32,

    /// <summary>
    /// PageRank.
    /// </summary>

    PageRank = 64,

    /// <summary>
    /// Overall metrics.
    /// </summary>

    OverallMetrics = 128,

    /// <summary>
    /// Group metrics.
    /// </summary>

    GroupMetrics = 256,
}

}
