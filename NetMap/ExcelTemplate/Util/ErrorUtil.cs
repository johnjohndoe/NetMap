
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//	Class: ErrorUtil
//
/// <summary>
///	Error utility methods.
/// </summary>
///
///	<remarks>
///	This class contains static utility methods for dealing with various errors.
///	</remarks>
//*****************************************************************************

public static class ErrorUtil
{
    //*************************************************************************
    //  Method: OnException()
    //
    /// <summary>
	/// Handles all types of exceptions.
    /// </summary>
    ///
	/// <param name="exception">
	/// The exception that was caught.
	/// </param>
	///
	/// <remarks>
	/// If <paramref name="exception" /> is of type <see
	/// cref="WorkbookFormatException" />, <see
	/// cref="OnWorkbookFormatException" /> is called.
	/// </remarks>
    //*************************************************************************

	public static void
	OnException
	(
		Exception exception
	)
	{
		Debug.Assert(exception != null);

		if (exception is WorkbookFormatException)
		{
			OnWorkbookFormatException( (WorkbookFormatException)exception );
			return;
		}

		FormUtil.ShowWarning( String.Format(

			"An unexpected problem occurred.  If it occurs again, please"
			+ " copy the details to the clipboard by typing Ctrl-C, then"
			+ " email the details to {0}."
			+ "\r\n\r\n"
			+ "Details:\r\n\r\n"
			+ "{1}"
			+ "\r\n\r\n"
			+ "{2}"
			,
			BugReportEmailAddress,
			ExceptionUtil.GetMessageTrace(exception),
			exception.StackTrace
			) );
	}

    //*************************************************************************
    //  Method: OnWorkbookFormatException()
    //
    /// <summary>
	/// Handles a <see cref="WorkbookFormatException" />.
    /// </summary>
    ///
	/// <param name="workbookFormatException">
	/// The exception that was caught.
	/// </param>
    //*************************************************************************

	public static void
	OnWorkbookFormatException
	(
		WorkbookFormatException workbookFormatException
	)
	{
		Debug.Assert(workbookFormatException != null);

		Range oRangeToSelect = workbookFormatException.RangeToSelect;

		if (oRangeToSelect != null)
		{
			ExcelUtil.SelectRange(oRangeToSelect);
		}

		FormUtil.ShowWarning(workbookFormatException.Message);
	}

    //*************************************************************************
    //  Method: OnMissingColumn()
    //
    /// <summary>
	/// Throws a <see cref="WorkbookFormatException" /> when a column required
	/// for a feature is missing.
    /// </summary>
    //*************************************************************************

	public static void
	OnMissingColumn()
	{
		throw new WorkbookFormatException(
			"The workbook is missing a column that is required to use this"
			+ " feature."
			+ "\r\n\r\n"
			+ GetTemplateMessage()
			);
	}

    //*************************************************************************
    //  Method: GetNoOpenWorkbookMessage()
    //
    /// <summary>
	/// Returns a message telling the user that there are no open workbooks.
    /// </summary>
    ///
	/// <returns>
	/// A message telling the user that there are no open workbooks.
	/// </returns>
    //*************************************************************************

	public static String
	GetNoOpenWorkbookMessage()
	{
		return( String.Format(

			"There is no open workbook.\r\n\r\n{0}"
			,
			GetTemplateMessage()
			) );
	}

    //*************************************************************************
    //  Method: GetTemplateMessage()
    //
    /// <summary>
	/// Returns a message telling the user to use the NetMap template.
    /// </summary>
    ///
	/// <returns>
	/// A message telling the user to use the NetMap template.
	/// </returns>
    //*************************************************************************

	public static String
	GetTemplateMessage()
	{
		return ( String.Format(

			"The easiest way to use {0} is to create a new workbook from the"
			+ " Excel template named \"{1}\".  The template contains all"
			+ " required worksheets, tables, and columns."
			,
			ThisWorkbook.DocumentCustomizationName,
			WorksheetReaderBase.TemplateName
			) );
	}


    //*************************************************************************
    //  Public constants
    //*************************************************************************

	/// Email address to send bug reports to.

	public const String BugReportEmailAddress = "netmap@microsoft.com";
}

}
