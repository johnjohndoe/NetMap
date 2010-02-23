
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: NodeXLWorkbookConversionException
//
/// <summary>
/// Represents an exception thrown by <see
/// cref="NodeXLWorkbookConverter.ConvertNodeXLWorkbook" />.
/// </summary>
//*****************************************************************************

[System.SerializableAttribute()]

public class NodeXLWorkbookConversionException : Exception
{
    //*************************************************************************
    //  Constructor: NodeXLWorkbookConversionException()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="NodeXLWorkbookConversionException" /> class.
    /// </summary>
    ///
    /// <param name="message">
    /// Error message, suitable for displaying to the user.
    /// </param>
    //*************************************************************************

    public NodeXLWorkbookConversionException
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
