
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.DesktopApplication
{
//*****************************************************************************
//  Class: ExceptionNotifier
//
/// <summary>
/// Contains static methods that are used to notify the user that an exception
/// has occurred.
/// </summary>
//*****************************************************************************

public static class ExceptionNotifier : Object
{
	//*************************************************************************
	//	Method: OnException()
	//
	/// <summary>
	/// Informs the user that an exception has occurred.
	/// </summary>
	///
	/// <param name="exception">
	/// Exception object that was thrown.
	/// </param>
	///
	/// <param name="generalDescription">
	/// A general description of what happened.  This gets included in the
	/// error message that gets displayed.  It must be a complete sentence.
	/// </param>
	//*************************************************************************

	public static void
	OnException
	(
		Exception exception,
		String generalDescription
	)
	{
		Debug.Assert(exception != null);
		Debug.Assert( !String.IsNullOrEmpty(generalDescription) );

		// Tell the user what happened.

		MessageBox.Show( ExceptionToMessage(exception, generalDescription) );
	}

	//*************************************************************************
	//	Method: ExceptionToMessage()
	//
	/// <summary>
	/// Converts an exception to a user-friendly message.
	/// </summary>
	///
	/// <param name="exception">
	/// Exception object that was thrown.
	/// </param>
	///
	/// <param name="generalDescription">
	/// A general description of what happened.  This gets included in the
	/// message.  It must be a complete sentence.
	/// </param>
	///
	/// <returns>
	/// User-friendly message suitable for display on the screen.
	/// </returns>
	//*************************************************************************

	public static String
	ExceptionToMessage
	(
		Exception exception,
		String generalDescription
	)
	{
		Debug.Assert( !String.IsNullOrEmpty(generalDescription) );
		Debug.Assert(exception != null);

		StringBuilder oStringBuilder = new StringBuilder(); 

		oStringBuilder.Append(generalDescription);

		oStringBuilder.Append(

			"\r\n\r\nIf the problem persists, please press Ctrl-C to copy the"
			+ " details to the clipboard, then e-mail the details to "
			);

		oStringBuilder.Append(ExceptionEMailAddress);

		oStringBuilder.Append(".\r\n\r\nDetails:\r\n\r\n");

		oStringBuilder.Append( ExceptionUtil.GetMessageTrace(exception) );

		return ( oStringBuilder.ToString() );
	}


	//*************************************************************************
	//	Private constants
	//*************************************************************************

	/// Address to send e-mail to when an exception occurs.

	private const String ExceptionEMailAddress = "conviz@microsoft.com";
}

}
