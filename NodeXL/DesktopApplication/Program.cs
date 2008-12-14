
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.DesktopApplication
{
static class Program
{
	//*************************************************************************
	//	Method: Main()
	//
	/// <summary>
	///	Applicaton's entry point.
	/// </summary>
	///
	/// <param name="asCommandLineArguments">
	/// Command line arguments.
	/// </param>
	//*************************************************************************

	[STAThread]

	static void
	Main
	(
		String [] asCommandLineArguments
	)
	{
		Debug.Assert(asCommandLineArguments != null);

		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);

		// This exception-handling pattern is recommended in the MSDN article
		// "Unexpected Errors in Managed Applications."

		try 
		{
			Main2(asCommandLineArguments);
		}
		catch (Exception e)
		{
			OnUnhandledException(e);
		}
	}

	//*************************************************************************
	//	Method: Main2()
	//
	/// <summary>
	///	Applicaton's entry point.
	/// </summary>
	///
	/// <param name="asCommandLineArguments">
	/// Command line arguments.
	/// </param>
	//*************************************************************************

	private static void
	Main2
	(
		String [] asCommandLineArguments
	)
	{
		Debug.Assert(asCommandLineArguments != null);

		InstallGlobalExceptionHandlers();

		Application.Run( new MainForm(asCommandLineArguments) );
	}

	//*************************************************************************
	//	Method: InstallGlobalExceptionHandlers()
	//
	/// <summary>
	/// Installs global exception handlers that handle exceptions not handled
	/// elsewhere.
	/// </summary>
	//*************************************************************************

	private static void
	InstallGlobalExceptionHandlers()
	{
		// This exception-handling pattern is recommended in the MSDN article
		// "Unexpected Errors in Managed Applications."

		// Handle CLR exceptions.

		AppDomain.CurrentDomain.UnhandledException +=
			new UnhandledExceptionEventHandler(
				CurrentDomain_UnhandledException);

		// Handle Windows Forms exceptions.

		Application.ThreadException +=
			new System.Threading.ThreadExceptionEventHandler(
				Application_ThreadException);
	}

	//*************************************************************************
	//	Method: CurrentDomain_UnhandledException()
	//
	/// <summary>
	///	Handles the UnhandledException event on the AppDomain.CurrentDomain
	/// object.
	/// </summary>
	///
	/// <param name="sender">
	///	Standard event argument.
	/// </param>
	///
	/// <param name="e">
	/// Standard event argument.
	/// </param>
	//*************************************************************************

	private static void
	CurrentDomain_UnhandledException
	(
		Object sender,
		UnhandledExceptionEventArgs e
	)
	{
		Debug.Assert(e.ExceptionObject is Exception);

		OnUnhandledException( (Exception)e.ExceptionObject );
	}

	//*************************************************************************
	//	Method: Application_ThreadException()
	//
	/// <summary>
	///	Handles the ThreadException event on the Application object.
	/// </summary>
	///
	/// <param name="sender">
	///	Standard event argument.
	/// </param>
	///
	/// <param name="e">
	/// Standard event argument.
	/// </param>
	//*************************************************************************

	private static void
	Application_ThreadException
	(
		object sender,
		System.Threading.ThreadExceptionEventArgs e
	)
	{
		OnUnhandledException(e.Exception);
	}

	//*************************************************************************
	//	Method: OnUnhandledException()
	//
	/// <summary>
	///	Handles exceptions not handled elsewhere in the application.
	/// </summary>
	///
	/// <param name="oException">
	/// Unhandled exception.
	/// </param>
	//*************************************************************************

	private static void
	OnUnhandledException
	(
		Exception oException
	)
	{
		// Notify the user, then exit.

		const String GeneralDescription = 
			"An unexpected problem occurred and the application must exit."
			;

		ExceptionNotifier.OnException(oException, GeneralDescription);

		Environment.Exit(1);
	}
}

}
