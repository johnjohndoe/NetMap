
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.NetworkServer
{
//*****************************************************************************
//  Enum: ExitCode
//
/// <summary>
/// Specifies the program's exit codes.
/// </summary>
//*****************************************************************************

public enum
ExitCode
{
    /// <summary>
    /// The network was successfully obtained.
    /// </summary>

    Success = 0,

    /// <summary>
    /// An unexpected exception occurred.
    /// </summary>

    UnexpectedException = 1,

    /// <summary>
    /// The command line arguments were invalid.
    /// </summary>

    InvalidCommandLineArguments = 2,

    /// <summary>
    /// The network configuration file was invalid.
    /// </summary>

    InvalidNetworkConfigurationFile = 3,

    /// <summary>
    /// An unrecoverable error prevented the network from being obtained.
    /// </summary>

    CouldNotGetNetwork = 4,

    /// <summary>
    /// The NodeXL Excel template file could not be found.
    /// </summary>

    CouldNotFindNodeXLTemplate = 5,

    /// <summary>
    /// Excel could not be opened.
    /// </summary>

    CouldNotOpenExcel = 6,

    /// <summary>
    /// A NodeXL workbook could not be created from the NodeXL template.
    /// </summary>

    CouldNotCreateNodeXLWorkbook = 7,

    /// <summary>
    /// A NodeXL workbook was created but the network graph could not be
    /// imported into it.
    /// </summary>

    CouldNotImportGraphIntoNodeXLWorkbook = 8,

    /// <summary>
    /// An error occcurred while saving the network file.
    /// </summary>

    SaveNetworkFileError = 9,

    /// <summary>
    /// An error occcurred while opening a NodeXL workbook to automate it.
    /// </summary>

    CouldNotAutomateNodeXLWorkbook = 10,
}

}
