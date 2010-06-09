
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.NetworkServer
{
//*****************************************************************************
//  Enum: NetworkFileFormats
//
/// <summary>
/// Specifies the file formats to save a network to.
/// </summary>
///
/// <remarks>
/// The values can be ORed together.
/// </remarks>
//*****************************************************************************

[System.FlagsAttribute]

public enum
NetworkFileFormats
{
    /// <summary>
    /// Don't save the network.
    /// </summary>

    None = 0,

    /// <summary>
    /// Save the network to a GraphML file.
    /// </summary>

    GraphML,

    /// <summary>
    /// Save the network to a NodeXL workbook.
    /// </summary>

    NodeXLWorkbook,
}

}
