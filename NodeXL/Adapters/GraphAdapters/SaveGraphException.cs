
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace Microsoft.NodeXL.Adapters
{
//*****************************************************************************
//  Class: SaveGraphException
//
/// <summary>
/// Represents an exception thrown when a graph adapter is unable to save a
/// graph to a file.
/// </summary>
//*****************************************************************************

[System.SerializableAttribute()]

public class SaveGraphException : Exception
{
    //*************************************************************************
    //  Constructor: SaveGraphException()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="SaveGraphException" />
    /// class.
    /// </summary>
    ///
    /// <param name="message">
    /// Error message, suitable for displaying to the user.
    /// </param>
    //*************************************************************************

    public SaveGraphException
    (
        String message
    )
    : base(message)
    {
        // (Do nothing.)
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
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}
}
