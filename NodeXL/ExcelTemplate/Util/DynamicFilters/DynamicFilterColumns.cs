

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Enum: DynamicFilterColumns
//
/// <summary>
/// Specifies the dynamic filter column in the edge table, the vertex
/// table, or both.
/// </summary>
///
/// <remarks>
/// These values can be ORed together.
/// </remarks>
//*****************************************************************************

[System.FlagsAttribute]

public enum
DynamicFilterColumns
{
    /// <summary>
    /// Specifies no dynamic filter column.
    /// </summary>

    None = 0,

    /// <summary>
    /// Specifies the dynamic filter column in the edge table.
    /// </summary>

    EdgeTable = 1,

    /// <summary>
    /// Specifies the dynamic filter column in the vertex table.
    /// </summary>

    VertexTable = 2,

    /// <summary>
    /// Specifies the dynamic filter column in all tables.
    /// </summary>

    AllTables = EdgeTable | VertexTable,
}

}
