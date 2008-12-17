

//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//	Class: NumericRangeColumnAutoFillUserSettingsDialog
//
/// <summary>
///	Edits a <see cref="NumericRangeColumnAutoFillUserSettings" /> object.
/// </summary>
///
/// <remarks>
///	Pass a <see cref="NumericRangeColumnAutoFillUserSettings" /> object to the
/// constructor.  If the user edits the object, <see
/// cref="Form.ShowDialog()" /> returns DialogResult.OK.  Otherwise, the object
/// is not modified and <see cref="Form.ShowDialog()" /> returns
/// DialogResult.Cancel.
/// </remarks>
//*****************************************************************************

public partial class NumericRangeColumnAutoFillUserSettingsDialog :
	ExcelTemplateForm
{
	//*************************************************************************
	//	Constructor: NumericRangeColumnAutoFillUserSettingsDialog()
	//
	/// <overloads>
	///	Initializes a new instance of the <see
	/// cref="NumericRangeColumnAutoFillUserSettingsDialog" /> class.
	/// </overloads>
	///
	/// <summary>
	///	Initializes a new instance of the <see
	/// cref="NumericRangeColumnAutoFillUserSettingsDialog" /> class with a
	/// <see cref="NumericRangeColumnAutoFillUserSettings" /> object.
	/// </summary>
	///
    /// <param name="numericRangeColumnAutoFillUserSettings">
	/// Object to edit.
    /// </param>
	///
    /// <param name="dialogCaption">
	/// Dialog caption.
    /// </param>
	///
	/// <param name="destinationColumnName">
    /// The name of the destination column, suitable for use as the placeholder
	/// in MinimumDestinationLabel and DestinationLabel2.  Can't be null
	/// or empty.
    /// </param>
	///
	/// <param name="minimumDestinationNumber">
	/// Minimum value of a cell in the destination column.
	/// </param>
	///
	/// <param name="maximumDestinationNumber">
	/// Maximum value of a cell in the destination column.
	/// </param>
	//*************************************************************************

	public NumericRangeColumnAutoFillUserSettingsDialog
	(
		NumericRangeColumnAutoFillUserSettings
			numericRangeColumnAutoFillUserSettings,

		String dialogCaption,
		String destinationColumnName,
		Double minimumDestinationNumber,
		Double maximumDestinationNumber
	)
	: this()
	{
		Debug.Assert(numericRangeColumnAutoFillUserSettings != null);
		Debug.Assert( !String.IsNullOrEmpty(dialogCaption) );
		Debug.Assert( !String.IsNullOrEmpty(destinationColumnName) );

		m_oNumericRangeColumnAutoFillUserSettings =
			numericRangeColumnAutoFillUserSettings;

		this.Text = dialogCaption;

		lblDestinationNumber1.Text = String.Format(
			DestinationLabel1
			,
			destinationColumnName
			);

		lblDestinationNumber2.Text = String.Format(
			DestinationLabel2
			,
			destinationColumnName
			);

		nudDestinationNumber1.Minimum = nudDestinationNumber2.Minimum =
			(Decimal)minimumDestinationNumber;

		nudDestinationNumber1.Maximum = nudDestinationNumber2.Maximum =
			(Decimal)maximumDestinationNumber;

		// Instantiate an object that saves and retrieves the position of this
		// dialog.  Note that the object automatically saves the settings when
		// the form closes.

		m_oNumericRangeColumnAutoFillUserSettingsDialogUserSettings =
			new NumericRangeColumnAutoFillUserSettingsDialogUserSettings(this);

		DoDataExchange(false);

		AssertValid();
	}

	//*************************************************************************
	//	Constructor: NumericRangeColumnAutoFillUserSettingsDialog()
	//
	/// <summary>
	///	Initializes a new instance of the <see
	/// cref="NumericRangeColumnAutoFillUserSettingsDialog" /> class for the
	/// Visual Studio designer.
	/// </summary>
	///
	/// <remarks>
	/// Do not use this constructor.  It is for use by the Visual Studio
	/// designer only.
	/// </remarks>
	//*************************************************************************

	public NumericRangeColumnAutoFillUserSettingsDialog()
	{
		InitializeComponent();

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
		AssertValid();

		if (bFromControls)
		{
			Double dSourceNumber1 = 0;
			Double dSourceNumber2 = 0;
			Double dDestinationNumber1, dDestinationNumber2;
			const String SourceMessage = "Enter a number.";
			const String DestinationMessage = "a number";

			Boolean bUseSourceNumber1 = radUseSourceNumber1.Checked;
			Boolean bUseSourceNumber2 = radUseSourceNumber2.Checked;

			if (
				(bUseSourceNumber1
				&&
				!this.ValidateDoubleTextBox(txbSourceNumber1,
					Double.MinValue, Double.MaxValue, SourceMessage,
					out dSourceNumber1) )
				||
				(bUseSourceNumber2
				&&
				!this.ValidateDoubleTextBox(txbSourceNumber2,
					Double.MinValue, Double.MaxValue, SourceMessage,
					out dSourceNumber2) )
				||
				!this.ValidateNumericUpDown(nudDestinationNumber1,
					DestinationMessage, out dDestinationNumber1)
				||
				!this.ValidateNumericUpDown(nudDestinationNumber2,
					DestinationMessage, out dDestinationNumber2)
				)
			{
				return (false);
			}

			m_oNumericRangeColumnAutoFillUserSettings.UseSourceNumber1 =
				bUseSourceNumber1;

			m_oNumericRangeColumnAutoFillUserSettings.UseSourceNumber2 =
				bUseSourceNumber2;

			m_oNumericRangeColumnAutoFillUserSettings.SourceNumber1 =
				dSourceNumber1;

			m_oNumericRangeColumnAutoFillUserSettings.SourceNumber2 =
				dSourceNumber2;

			m_oNumericRangeColumnAutoFillUserSettings.DestinationNumber1 =
				dDestinationNumber1;

			m_oNumericRangeColumnAutoFillUserSettings.DestinationNumber2 =
				dDestinationNumber2;

			m_oNumericRangeColumnAutoFillUserSettings.IgnoreOutliers =
				chkIgnoreOutliers.Checked;
		}
		else
		{
			radUseMinimumSourceNumber.Checked = !(radUseSourceNumber1.Checked =
				m_oNumericRangeColumnAutoFillUserSettings.UseSourceNumber1);

			radUseMaximumSourceNumber.Checked = !(radUseSourceNumber2.Checked =
				m_oNumericRangeColumnAutoFillUserSettings.UseSourceNumber2);

			txbSourceNumber1.Text =
				m_oNumericRangeColumnAutoFillUserSettings.SourceNumber1.
					ToString();

			txbSourceNumber2.Text =
				m_oNumericRangeColumnAutoFillUserSettings.SourceNumber2.
					ToString();

			nudDestinationNumber1.Value = (Decimal)
				m_oNumericRangeColumnAutoFillUserSettings.DestinationNumber1;

			nudDestinationNumber2.Value = (Decimal)
				m_oNumericRangeColumnAutoFillUserSettings.DestinationNumber2;

			chkIgnoreOutliers.Checked =
				m_oNumericRangeColumnAutoFillUserSettings.IgnoreOutliers;
		}

		return (true);
	}

	//*************************************************************************
	//	Method: EnableControls()
	//
	/// <summary>
	///	Enables or disables the dialog's controls.
	/// </summary>
	//*************************************************************************

	protected void
	EnableControls()
	{
		AssertValid();

		txbSourceNumber1.Enabled = radUseSourceNumber1.Checked;
		txbSourceNumber2.Enabled = radUseSourceNumber2.Checked;

		// The user should be able to ignore outliers only if the entire range
		// of source numbers is specified.

		if (radUseMinimumSourceNumber.Checked &&
			radUseMaximumSourceNumber.Checked)
		{
			chkIgnoreOutliers.Enabled = true;
		}
		else
		{
			chkIgnoreOutliers.Checked = false;
			chkIgnoreOutliers.Enabled = false;
		}
	}

    //*************************************************************************
    //  Method: OnEventThatRequiresControlEnabling()
    //
    /// <summary>
	/// Handles any event that should changed the enabled state of the dialog's
	/// controls.
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
	OnEventThatRequiresControlEnabling
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

		EnableControls();
    }

	//*************************************************************************
	//	Method: lnkIgnoreOutliers_LinkClicked()
	//
	/// <summary>
	///	Handles the LinkClicked event on the lnkIgnoreOutliers LinkButton.
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
	lnkIgnoreOutliers_LinkClicked
	(
		object sender,
		LinkLabelLinkClickedEventArgs e
	)
    {
		AssertValid();

		this.ShowInformation(AutoFillUserSettingsDialog.IgnoreOutliersMessage);
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
		System.EventArgs e
	)
	{
		if ( DoDataExchange(true) )
		{
			DialogResult = DialogResult.OK;
			this.Close();
		}
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

		Debug.Assert(
			m_oNumericRangeColumnAutoFillUserSettingsDialogUserSettings !=
			null);

		Debug.Assert(m_oNumericRangeColumnAutoFillUserSettings != null);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	/// Label text for the destination numbers.

	protected const String DestinationLabel1 =
		"T&o this {0}:";

	///
	protected const String DestinationLabel2 =
		"To t&his {0}:";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// User settings for this dialog.

	protected NumericRangeColumnAutoFillUserSettingsDialogUserSettings
		m_oNumericRangeColumnAutoFillUserSettingsDialogUserSettings;

	/// Object being edited.

	protected NumericRangeColumnAutoFillUserSettings
		m_oNumericRangeColumnAutoFillUserSettings;
}


//*****************************************************************************
//  Class: NumericRangeColumnAutoFillUserSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="NumericRangeColumnAutoFillUserSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("NumericRangeColumnAutoFillUserSettingsDialog") ]

public class NumericRangeColumnAutoFillUserSettingsDialogUserSettings :
	FormSettings
{
    //*************************************************************************
    //  Constructor: NumericRangeColumnAutoFillUserSettingsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="NumericRangeColumnAutoFillUserSettingsDialogUserSettings" />
	/// class.
    /// </summary>
	///
	/// <param name="oForm">
	/// The form to save settings for.
	/// </param>
    //*************************************************************************

    public NumericRangeColumnAutoFillUserSettingsDialogUserSettings
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
