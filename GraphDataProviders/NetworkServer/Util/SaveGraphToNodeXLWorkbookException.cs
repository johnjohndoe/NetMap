
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace Microsoft.NodeXL.NetworkServer
{
//*****************************************************************************
//  Class: SaveGraphToNodeXLWorkbookException
//
/// <summary>
/// Represents an exception thrown by the <see cref="NodeXLWorkbookSaver" />
/// class
/// </summary>
//*****************************************************************************

[System.SerializableAttribute()]

public class SaveGraphToNodeXLWorkbookException : Exception
{
    //*************************************************************************
    //  Constructor: SaveGraphToNodeXLWorkbookException()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="SaveGraphToNodeXLWorkbookException" /> class.
    /// </summary>
    ///
    /// <param name="exitCode">
    /// The program exit code to use.
    /// </param>
    ///
    /// <param name="message">
    /// Error message, suitable for displaying to the user.
    /// </param>
    //*************************************************************************

    public SaveGraphToNodeXLWorkbookException
    (
        ExitCode exitCode,
        String message
    )
    : base(message)
    {
        m_eExitCode = exitCode;

        AssertValid();
    }

    //*************************************************************************
    //  Property: ExitCode
    //
    /// <summary>
    /// Gets the program exit code to use.
    /// </summary>
    ///
    /// <value>
    /// The program exit code to use, as an <see
    /// cref="NetworkServer.ExitCode" />.
    /// </value>
    //*************************************************************************

    public ExitCode
    ExitCode
    {
        get
        {
            return (m_eExitCode);
        }
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        // m_eExitCode
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The program exit code to use.

    protected ExitCode m_eExitCode;
}
}
