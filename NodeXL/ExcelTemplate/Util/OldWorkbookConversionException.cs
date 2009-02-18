
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: OldWorkbookConversionException
//
/// <summary>
/// Represents an exception thrown when an old workbook can't be converted to
/// a new workbook.
/// </summary>
//*****************************************************************************

[System.SerializableAttribute()]

public class OldWorkbookConversionException : Exception
{
    //*************************************************************************
    //  Constructor: OldWorkbookConversionException()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="OldWorkbookConversionException" /> class.
    /// </summary>
    ///
    /// <param name="message">
    /// Error message, suitable for displaying to the user.
    /// </param>
    //*************************************************************************

    public OldWorkbookConversionException
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
