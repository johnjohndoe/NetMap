
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;

namespace Microsoft.NetMap.Visualization
{
//*****************************************************************************
//  Enum: MouseSelectionMode
//
/// <summary>
/// Determines what happens when a vertex is clicked in the <see
/// cref="NetMapControl" />.
/// </summary>
///
/// <remarks>
/// This is the type of the <see cref="NetMapControl.MouseSelectionMode" />
/// property.
/// </remarks>
//*****************************************************************************

public enum
MouseSelectionMode
{
    /// <summary>
    /// Clicking a vertex does not select anything.
    /// </summary>

    SelectNothing,

    /// <summary>
    /// Clicking a vertex selects it without selecting its incident edges.
    /// </summary>

    SelectVertexOnly,

    /// <summary>
    /// Clicking a vertex selects it and its incident edges.
    /// </summary>

    SelectVertexAndIncidentEdges,
}

}
