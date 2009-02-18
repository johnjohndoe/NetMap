
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Enum: VertexDrawingPrecedence
//
/// <summary>
/// Specifies the order in which the metadata keys that specify primary label,
/// image, and shape are checked.
/// </summary>
//*****************************************************************************

public enum
VertexDrawingPrecedence
{
    /// <summary>
    /// Draw the vertex as a shape.
    /// </summary>

    Shape = 0,

    /// <summary>
    /// If an image key exists, draw the vertex as an image.  Otherwise, draw
    /// the vertex as a shape.
    /// </summary>

    Image = 1,

    /// <summary>
    /// If a primary label key exists, draw the vertex as a primary label.
    /// Otherwise, if an image key exists, draw the vertex as an image.
    /// Otherwise, draw the vertex as a shape.
    /// </summary>

    PrimaryLabel = 2,
}
}
