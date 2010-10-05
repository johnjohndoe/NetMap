
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Enum: GroupCommands
//
/// <summary>
/// Specifies one or more group-related command that can be run.
/// </summary>
///
/// <remarks>
/// The flags can be ORed together.
/// </remarks>
//*****************************************************************************

[System.FlagsAttribute]

public enum
GroupCommands
{
    /// <summary>
    /// No commands.
    /// </summary>

    None = 0,

    /// <summary>
    /// Collapse the selected groups.
    /// </summary>

    CollapseSelectedGroups = 1,

    /// <summary>
    /// Expand the selected groups.
    /// </summary>

    ExpandSelectedGroups = 2,

    /// <summary>
    /// Collapse all groups.
    /// </summary>

    CollapseAllGroups = 4,

    /// <summary>
    /// Expand all groups.
    /// </summary>

    ExpandAllGroups = 8,

    /// <summary>
    /// Select the groups containing the selected vertices.
    /// </summary>

    SelectGroupsWithSelectedVertices = 16,

    /// <summary>
    /// Select all groups.
    /// </summary>

    SelectAllGroups = 32,

    /// <summary>
    /// Remove the selected vertices from their groups.
    /// </summary>

    RemoveSelectedVerticesFromGroups = 64,

    /// <summary>
    /// Add the selected vertices to a group.
    /// </summary>

    AddSelectedVerticesToGroup = 128,

    /// <summary>
    /// Remove the selected groups.
    /// </summary>

    RemoveSelectedGroups = 256,

    /// <summary>
    /// Remove all groups.
    /// </summary>

    RemoveAllGroups = 512,
}

}
