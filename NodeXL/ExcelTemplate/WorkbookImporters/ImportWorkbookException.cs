
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ImportWorkbookException
//
/// <summary>
/// Represents an exception thrown by when a workbook containing a graph can't
/// be imported.
/// </summary>
//*****************************************************************************

[System.SerializableAttribute()]

public class ImportWorkbookException : Exception
{
    //*************************************************************************
    //  Constructor: ImportWorkbookException()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ImportWorkbookException" /> class.
    /// </summary>
    ///
    /// <param name="message">
    /// Error message, suitable for displaying to the user.
    /// </param>
    //*************************************************************************

    public ImportWorkbookException
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
