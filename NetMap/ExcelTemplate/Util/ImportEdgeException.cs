
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: ImportEdgeException
//
/// <summary>
/// Represents an exception thrown when edges can't be imported from another
/// workbook.
/// </summary>
//*****************************************************************************

[System.SerializableAttribute()]

public class ImportEdgeException : Exception
{
    //*************************************************************************
    //  Constructor: ImportEdgeException()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ImportEdgeException" />
	/// class.
    /// </summary>
	///
    /// <param name="message">
	/// Error message, suitable for displaying to the user.
    /// </param>
    //*************************************************************************

    public ImportEdgeException
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
