
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Runtime.Serialization;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: WorkbookFormatException
//
/// <summary>
/// Represents an exception thrown when a workbook contains invalid data.
/// </summary>
//*****************************************************************************

[System.SerializableAttribute()]

public class WorkbookFormatException : FormatException
{
    //*************************************************************************
    //  Constructor: WorkbookFormatException()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="WorkbookFormatException" />  class with a specified error
	/// message.
    /// </summary>
	///
    /// <param name="message">
	/// Error message.
    /// </param>
    //*************************************************************************

    public WorkbookFormatException
	(
		String message
	)
	: this(message, null)
    {
		// (Do nothing.)
    }

    //*************************************************************************
    //  Constructor: WorkbookFormatException()
    //
    /// <summary>
	/// Initializes a new instance of the <see
	/// cref="WorkbookFormatException" />  class with a specified
	/// error message and a range that the catch block should select.
    /// </summary>
	///
    /// <param name="message">
	/// Error message.
    /// </param>
	///
    /// <param name="rangeToSelect">
	/// The range that the catch block should select to highlight the workbook
	/// error, or null if a range shouldn't be selected.
    /// </param>
    //*************************************************************************

    public WorkbookFormatException
	(
		String message,
		Range rangeToSelect
	)
	: base(message)
    {
		Initialize();

		m_oRangeToSelect = rangeToSelect;

		AssertValid();
    }

    //*************************************************************************
    //  Constructor: WorkbookFormatException()
    //
    /// <summary>
    /// Do not use this constructor.  It is for binary serialization only.
    /// </summary>
    //*************************************************************************

    protected WorkbookFormatException
	(
		SerializationInfo oSerializationInfo,
		StreamingContext oStreamingContext
	)
	:
	base
	(
		oSerializationInfo,
		oStreamingContext
	)
    {
		// Do not use this constructor.  It is for binary serialization only.
		//
		// This is required because the System.Exception base class implements
		// ISerializable.  All serializable classes derived from an
		// ISerializable implementor must have a constructor with this
		// signature.

		Initialize();

		AssertValid();
    }

	//*************************************************************************
	//	Property: RangeToSelect
	//
	/// <summary>
	/// Gets or sets the range that the catch block should select to highlight
	/// the workbook error.
	/// </summary>
	///
	/// <value>
	/// The range that the catch block should select to highlight the workbook
	/// error, or null if a range shouldn't be selected.
	/// </value>
	//*************************************************************************

	public Range
	RangeToSelect
	{
		get
		{
			AssertValid();

			return (m_oRangeToSelect);
		}
		 
		set
		{
			m_oRangeToSelect = value;

			AssertValid();
		}
	}

    //*************************************************************************
    //  Method: Initialize()
    //
    /// <summary>
	/// Initializes the object.
    /// </summary>
    //*************************************************************************

	protected void
	Initialize()
	{
		m_oRangeToSelect = null;
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
		// m_oRangeToSelect
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// The range that the catch block should select to highlight the workbook
	/// error, or null if a range shouldn't be selected.

	protected Range m_oRangeToSelect;
}

}
