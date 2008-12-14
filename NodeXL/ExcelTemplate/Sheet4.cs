

//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: Sheet4
//
/// <summary>
/// Represents the miscellaneous worksheet.
/// </summary>
//*****************************************************************************

public partial class Sheet4
{
    //*************************************************************************
    //  Method: Sheet4_Startup()
    //
    /// <summary>
	/// Handles the Startup event on the worksheet.
    /// </summary>
    ///
	/// <param name="sender">
	/// Standard event argument.
	/// </param>
    ///
	/// <param name="e">
	/// Standard event argument.
	/// </param>
    //*************************************************************************

	private void
	Sheet4_Startup
	(
		object sender,
		System.EventArgs e
	)
	{
		AssertValid();

		// (Do nothing.)
	}

    //*************************************************************************
    //  Method: Sheet4_Shutdown()
    //
    /// <summary>
	/// Handles the Shutdown event on the worksheet.
    /// </summary>
    ///
	/// <param name="sender">
	/// Standard event argument.
	/// </param>
    ///
	/// <param name="e">
	/// Standard event argument.
	/// </param>
    //*************************************************************************

	private void
	Sheet4_Shutdown
	(
		object sender,
		System.EventArgs e
	)
	{
		AssertValid();

		// (Do nothing.)
	}


	#region VSTO Designer generated code

	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InternalStartup()
	{
		this.Startup += new System.EventHandler(Sheet4_Startup);
		this.Shutdown += new System.EventHandler(Sheet4_Shutdown);
	}
        
	#endregion


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
