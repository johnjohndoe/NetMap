

//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplateRegisterUser
{
//*****************************************************************************
//	Class: MainForm
//
/// <summary>
/// The application's main form.
/// </summary>
///
/// <remarks>
/// The form contains a single RegisterUser control that does all the work.
/// </remarks>
//*****************************************************************************

public partial class MainForm : Form
{
	//*************************************************************************
	//	Constructor: MainForm()
	//
	/// <summary>
	///	Initializes a new instance of the <see cref="MainForm" /> class.
	/// </summary>
	//*************************************************************************

	public MainForm()
	{
		InitializeComponent();

		AssertValid();
	}

	//*************************************************************************
	//	Method: usrRegisterUser_Done()
	//
	/// <summary>
	///	Handles the Done event on the usrRegisterUser UserControl.
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

    private void
	usrRegisterUser_Done
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

        Application.Exit();
    }


	//*************************************************************************
	//	Method: AssertValid()
	//
	/// <summary>
	///	Asserts if the object is in an invalid state.  Debug-only.
	/// </summary>
	//*************************************************************************

	[Conditional("DEBUG")] 

	public void
	AssertValid()
	{
		// (Do nothing.)
	}


	//*************************************************************************
	//	Protected fields
	//*************************************************************************

	// (None.)
}
}
