

//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//	Class: NumericComparisonColumnAutoFillUserSettingsDialog
//
/// <summary>
///	Edits a <see cref="NumericComparisonColumnAutoFillUserSettings" /> object.
/// </summary>
///
/// <remarks>
///	Pass a <see cref="NumericComparisonColumnAutoFillUserSettings" /> object to
/// the constructor.  If the user edits the object, <see
/// cref="Form.ShowDialog()" /> returns DialogResult.OK.  Otherwise, the object
/// is not modified and <see cref="Form.ShowDialog()" /> returns
/// DialogResult.Cancel.
/// </remarks>
//*****************************************************************************

public partial class NumericComparisonColumnAutoFillUserSettingsDialog :
	ExcelTemplateForm
{
	//*************************************************************************
	//	Constructor: NumericComparisonColumnAutoFillUserSettingsDialog()
	//
	/// <overloads>
	///	Initializes a new instance of the <see
	/// cref="NumericComparisonColumnAutoFillUserSettingsDialog" /> class.
	/// </overloads>
	///
	/// <summary>
	///	Initializes a new instance of the <see
	/// cref="NumericComparisonColumnAutoFillUserSettingsDialog" /> class with
	/// a <see cref="NumericComparisonColumnAutoFillUserSettings" /> object.
	/// </summary>
	///
    /// <param name="numericComparisonColumnAutoFillUserSettings">
	/// Object to edit.
    /// </param>
	///
    /// <param name="dialogCaption">
	/// Dialog caption.
    /// </param>
	///
	/// <param name="label1Text">
	/// Text to display above the comparison controls.
    /// </param>
	///
	/// <param name="label2Text">
	/// Text to display below the comparison controls.
	/// </param>
	//*************************************************************************

	public NumericComparisonColumnAutoFillUserSettingsDialog
	(
		NumericComparisonColumnAutoFillUserSettings
			numericComparisonColumnAutoFillUserSettings,

		String dialogCaption,
		String label1Text,
		String label2Text
	)
	: this()
	{
		Debug.Assert(numericComparisonColumnAutoFillUserSettings != null);
		Debug.Assert( !String.IsNullOrEmpty(dialogCaption) );
		Debug.Assert( !String.IsNullOrEmpty(label1Text) );
		Debug.Assert( !String.IsNullOrEmpty(label2Text) );

		m_oNumericComparisonColumnAutoFillUserSettings =
			numericComparisonColumnAutoFillUserSettings;

		this.Text = dialogCaption;
		lblLabel1.Text = label1Text;
		lblLabel2.Text = label2Text;

		cbxComparisonOperator.PopulateWithObjectsAndText(
			ComparisonOperator.LessThan, "Less than",
			ComparisonOperator.LessThanOrEqual, "Less than or equal to",
			ComparisonOperator.Equal, "Equal to",
			ComparisonOperator.NotEqual, "Not equal to",
			ComparisonOperator.GreaterThan, "Greater than",
			ComparisonOperator.GreaterThanOrEqual, "Greater than or equal to"
			);

		// Instantiate an object that saves and retrieves the position of this
		// dialog.  Note that the object automatically saves the settings when
		// the form closes.

		m_oNumericComparisonColumnAutoFillUserSettingsDialogUserSettings =
			new NumericComparisonColumnAutoFillUserSettingsDialogUserSettings(
				this);

		DoDataExchange(false);

		AssertValid();
	}

	//*************************************************************************
	//	Constructor: NumericComparisonColumnAutoFillUserSettingsDialog()
	//
	/// <summary>
	///	Initializes a new instance of the <see
	/// cref="NumericComparisonColumnAutoFillUserSettingsDialog" /> class for
	/// the Visual Studio designer.
	/// </summary>
	///
	/// <remarks>
	/// Do not use this constructor.  It is for use by the Visual Studio
	/// designer only.
	/// </remarks>
	//*************************************************************************

	public NumericComparisonColumnAutoFillUserSettingsDialog()
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
		if (bFromControls)
		{
			Double dSourceNumberToCompareTo;

			if ( !this.ValidateDoubleTextBox(txbSourceNumber,
				Double.MinValue, Double.MaxValue, "Enter a number.",
				out dSourceNumberToCompareTo) )
			{
				return (false);
			}

			m_oNumericComparisonColumnAutoFillUserSettings.ComparisonOperator
				= (ComparisonOperator)cbxComparisonOperator.SelectedValue;

			m_oNumericComparisonColumnAutoFillUserSettings.
				SourceNumberToCompareTo = dSourceNumberToCompareTo;
		}
		else
		{
			cbxComparisonOperator.SelectedValue =
				m_oNumericComparisonColumnAutoFillUserSettings.
					ComparisonOperator;

			txbSourceNumber.Text =
				m_oNumericComparisonColumnAutoFillUserSettings.
					SourceNumberToCompareTo.ToString(
						ExcelTemplateForm.DoubleFormat);
		}

		return (true);
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
			m_oNumericComparisonColumnAutoFillUserSettingsDialogUserSettings
				!= null);

		Debug.Assert(m_oNumericComparisonColumnAutoFillUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// User settings for this dialog.

	protected NumericComparisonColumnAutoFillUserSettingsDialogUserSettings
		m_oNumericComparisonColumnAutoFillUserSettingsDialogUserSettings;

	/// Object being edited.

	protected NumericComparisonColumnAutoFillUserSettings
		m_oNumericComparisonColumnAutoFillUserSettings;
}


//*****************************************************************************
//  Class: NumericComparisonColumnAutoFillUserSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="NumericComparisonColumnAutoFillUserSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute(
	"NumericComparisonColumnAutoFillUserSettingsDialog") ]

public class NumericComparisonColumnAutoFillUserSettingsDialogUserSettings :
	FormSettings
{
    //*************************************************************************
    //  Constructor: NumericComparisonColumnAutoFillUserSettingsDialog
	//     UserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="NumericComparisonColumnAutoFillUserSettingsDialogUserSettings" />
	/// class.
    /// </summary>
	///
	/// <param name="oForm">
	/// The form to save settings for.
	/// </param>
    //*************************************************************************

    public NumericComparisonColumnAutoFillUserSettingsDialogUserSettings
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
