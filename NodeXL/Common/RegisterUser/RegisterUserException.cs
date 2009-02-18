
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace Microsoft.NodeXL.Common
{
//*****************************************************************************
//  Class: RegisterUserException
//
/// <summary>
/// Represents an exception thrown when a user can't be registered.
/// </summary>
//*****************************************************************************

[System.SerializableAttribute()]

public class RegisterUserException : Exception
{
    //*************************************************************************
    //  Constructor: RegisterUserException()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterUserException" />
    /// class.
    /// </summary>
    ///
    /// <param name="message">
    /// Error message, suitable for displaying to the user.
    /// </param>
    ///
    /// <param name="innerException">
    /// Inner exception, or null.
    /// </param>
    //*************************************************************************

    public RegisterUserException
    (
        String message,
        Exception innerException
    )
    : base(message, innerException)
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
