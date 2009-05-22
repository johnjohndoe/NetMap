
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Enum: VisualAttributes
//
/// <summary>
/// Specifies one or more visual attributes of the graph.
/// </summary>
///
/// <remarks>
/// The flags can be ORed together.
/// </remarks>
//*****************************************************************************

[System.FlagsAttribute]

public enum
VisualAttributes
{
    /// <summary>
    /// No visual properties.
    /// </summary>

    None = 0,

    /// <summary>
    /// Edge or vertex color.
    /// </summary>

    Color = 1,

    /// <summary>
    /// Edge or vertex alpha.
    /// </summary>

    Alpha = 2,

    /// <summary>
    /// Edge width.
    /// </summary>

    EdgeWidth = 4,

    /// <summary>
    /// Edge visibility.
    /// </summary>

    EdgeVisibility = 8,

    /// <summary>
    /// Vertex shape.
    /// </summary>

    VertexShape = 16,

    /// <summary>
    /// Vertex radius.
    /// </summary>

    VertexRadius = 32,

    /// <summary>
    /// Vertex visibility.
    /// </summary>

    VertexVisibility = 64,
}

}
