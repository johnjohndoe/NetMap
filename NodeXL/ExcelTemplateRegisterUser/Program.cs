

//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;

namespace Microsoft.NodeXL.ExcelTemplateRegisterUser
{
//*****************************************************************************
//	Class: Program
//
/// <summary>
/// The application's entry point.
/// </summary>
//*****************************************************************************

static class Program
{
	//*************************************************************************
	//	Method: Main()
	//
	/// <summary>
	/// The main entry point for the application.
	/// </summary>
	//*************************************************************************

	[STAThread]

	static void Main()
	{
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		Application.Run(new MainForm());
	}
}
}
