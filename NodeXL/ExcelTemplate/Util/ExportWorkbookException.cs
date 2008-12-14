
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ExportWorkbookException
//
/// <summary>
/// Represents an exception thrown when a workbook can't be exported.
/// </summary>
//*****************************************************************************

[System.SerializableAttribute()]

public class ExportWorkbookException : Exception
{
    //*************************************************************************
    //  Constructor: ExportWorkbookException()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="ExportWorkbookException" /> class.
    /// </summary>
	///
    /// <param name="message">
	/// Error message, suitable for displaying to the user.
    /// </param>
    //*************************************************************************

    public ExportWorkbookException
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
