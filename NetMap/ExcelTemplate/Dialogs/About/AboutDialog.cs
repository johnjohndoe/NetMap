

//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//	Class: AboutDialog
//
/// <summary>
/// This is the application's About dialog.
/// </summary>
///
/// <remarks>
/// Call <see cref="Form.ShowDialog()" /> to run the dialog.
/// </remarks>
//*****************************************************************************

public partial class AboutDialog : ExcelTemplateForm
{
	//*************************************************************************
	//	Constructor: AboutDialog()
	//
	/// <summary>
	///	Initializes a new instance of the <see cref="AboutDialog" /> class.
	/// </summary>
	//*************************************************************************

	public AboutDialog()
	{
		InitializeComponent();

		// Instantiate an object that retrieves and saves the location of this
		// dialog.  Note that the object automatically saves the settings when
		// the form closes.

		m_oAboutDialogUserSettings = new AboutDialogUserSettings(this);

		DoDataExchange(false);

		// AssertValid();
	}

	//*************************************************************************
	//	Method: DoDataExchange()
	//
	/// <summary>
	///	Transfers data between the dialog's fields and its controls.
	/// </summary>
	///
	/// <param name="bFromControls">
	///	true to transfer data from the dialog's controls to its fields, false
	///	for the other direction.
	/// </param>
	///
	/// <returns>
	///	true if the transfer was successful.
	/// </returns>
	//*************************************************************************

	protected Boolean
	DoDataExchange
	(
		Boolean bFromControls
	)
	{
		if (bFromControls)
		{
			// (Do nothing.)
		}
		else
		{
			lblVersion.Text = String.Format(

				"Version {0}"
				,
				FileVersionInfo.GetVersionInfo(
					Assembly.GetExecutingAssembly().Location).FileVersion
				);

			lblCopyright.Text = String.Format(

                "Copyright © {0} Microsoft Corporation"
				,
				DateTime.Now.Year
				);

			lnkContact.Text = ErrorUtil.BugReportEmailAddress;
		}

		return (true);
	}

	//*************************************************************************
	//	Method: lnkContact_LinkClicked()
	//
	/// <summary>
	///	Handles the LinkClicked event on the lnkContact LinkLabel.
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
	lnkContact_LinkClicked
	(
		object sender,
		LinkLabelLinkClickedEventArgs e
	)
    {
		AssertValid();

		System.Diagnostics.Process.Start(
			"mailto:" + ErrorUtil.BugReportEmailAddress
			);
    }

	//*************************************************************************
	//	Method: btnOK_Click()
	//
	/// <summary>
	///	Handles the Click event on the btnOK button.
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
	btnOK_Click
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

        this.Close();
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
		base.AssertValid();

		Debug.Assert(m_oAboutDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// User settings for this dialog.

	protected AboutDialogUserSettings m_oAboutDialogUserSettings;
}


//*****************************************************************************
//  Class: AboutDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="AboutDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("AboutDialog") ]

public class AboutDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: AboutDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="AboutDialogUserSettings" /> class.
    /// </summary>
	///
	/// <param name="oForm">
	/// The form to save settings for.
	/// </param>
    //*************************************************************************

    public AboutDialogUserSettings
	(
		Form oForm
	)
	: base (oForm, true)
    {
		Debug.Assert(oForm != null);

		// (Do nothing.)

		AssertValid();
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
		base.AssertValid();

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}
}
