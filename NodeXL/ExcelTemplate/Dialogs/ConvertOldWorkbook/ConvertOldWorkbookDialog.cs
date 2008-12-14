

//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Configuration;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//	Class: ConvertOldWorkbookDialog
//
/// <summary>
///	Copies an old workbook created with an old version of NodeXL and converts
/// the copy to work with the current version.
/// </summary>
///
/// <remarks>
/// A <see cref="OldWorkbookConverter" /> object does most of the work.
/// </remarks>
//*****************************************************************************

public partial class ConvertOldWorkbookDialog : ExcelTemplateForm
{
	//*************************************************************************
	//	Constructor: ConvertOldWorkbookDialog()
	//
	/// <overloads>
	///	Initializes a new instance of the <see
	/// cref="ConvertOldWorkbookDialog" /> class.
	/// </overloads>
	///
	/// <summary>
	///	Initializes a new instance of the <see
	/// cref="ConvertOldWorkbookDialog" /> class with an Excel Application
	/// object.
	/// </summary>
	///
    /// <param name="application">
	/// Excel application.
    /// </param>
	//*************************************************************************

	public ConvertOldWorkbookDialog
	(
        Microsoft.Office.Interop.Excel.Application application
	)
	: this()
	{
		// Instantiate an object that saves and retrieves the user settings for
		// this dialog.  Note that the object automatically saves the settings
		// when the form closes.

		m_oConvertOldWorkbookDialogUserSettings =
			new ConvertOldWorkbookDialogUserSettings(this);

		m_oApplication = application;
		m_sOldWorkbookFile = String.Empty;
		m_sConvertedWorkbookFile = String.Empty;

		m_oOpenFileDialog = new OpenFileDialog();

		m_oOpenFileDialog.Filter =
			"Excel Workbook (*.xlsx)|*.xlsx|All files (*.*)|*.*";

		m_oOpenFileDialog.Title = "Browse for Old Workbook";

		DoDataExchange(false);

		AssertValid();
	}

	//*************************************************************************
	//	Constructor: ConvertOldWorkbookDialog()
	//
	/// <summary>
	///	Initializes a new instance of the <see
	/// cref="ConvertOldWorkbookDialog" /> class for the Visual Studio
	/// designer.
	/// </summary>
	///
	/// <remarks>
	/// Do not use this constructor.  It is for use by the Visual Studio
	/// designer only.
	/// </remarks>
	//*************************************************************************

	public ConvertOldWorkbookDialog()
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
			if ( !this.ValidateFileTextBox(txbOldWorkbookFile,
				"Enter or browse for an old workbook.",
				out m_sOldWorkbookFile) )
			{
				return (false);
			}

			m_sConvertedWorkbookFile = txbConvertedWorkbookFile.Text;

			// The txbOldWorkbookFile_TextChanged() event handler guarantees
			// that the converted workbook file will not be null or empty if
			// the old file exists.

			Debug.Assert( !String.IsNullOrEmpty(m_sConvertedWorkbookFile) );

			if (
				File.Exists(m_sConvertedWorkbookFile)
				&&
				MessageBox.Show(
					"The converted copy already exists.  Do you want to"
					+ " overwrite it?",
					ApplicationUtil.ApplicationName, MessageBoxButtons.YesNo,
					MessageBoxIcon.Warning) != DialogResult.Yes
				)
			{
				return (false);
			}

			m_oConvertOldWorkbookDialogUserSettings.OpenConvertedWorkbook =
				cbxOpenConvertedWorkbook.Checked;
		}
		else
		{
			txbOldWorkbookFile.Text = m_sOldWorkbookFile;
			txbConvertedWorkbookFile.Text = m_sConvertedWorkbookFile;

			cbxOpenConvertedWorkbook.Checked =
				m_oConvertOldWorkbookDialogUserSettings.OpenConvertedWorkbook;
		}

		return (true);
	}

	//*************************************************************************
	//	Method: OldToConvertedWorkbook()
	//
	/// <summary>
	///	Derives a path to a converted workbook given the old workbook path.
	/// </summary>
	///
	/// <param name="sOldWorkbookFile">
	///	Full path to the old workbook.  The workbook may or may not exist.  Can
	/// be null or empty.
	/// </param>
	///
	/// <returns>
	///	Full path to use for the converted workbook, or String.Empty if the old
	/// workbook doesn't exist.
	/// </returns>
	//*************************************************************************

	protected String
	OldToConvertedWorkbook
	(
		String sOldWorkbookFile
	)
	{
		AssertValid();

		if (
			String.IsNullOrEmpty(sOldWorkbookFile)
			||
			!File.Exists(sOldWorkbookFile)
			)
		{
			return (String.Empty);
		}

		return (
			Path.Combine(
				Path.GetDirectoryName(sOldWorkbookFile),
				Path.GetFileNameWithoutExtension(sOldWorkbookFile)
					+ "-Copy"
				)
			+ Path.GetExtension(sOldWorkbookFile)
			);
	}

	//*************************************************************************
	//	Method: ConvertOldWorkbook()
	//
	/// <summary>
	///	Copies an old workbook created with an old version of NodeXL and
	/// converts the copy to work with the current version.
	/// </summary>
	///
	/// <param name="sOldWorkbookFile">
	///	Full path to the old workbook.  The workbook must exist.
	/// </param>
	///
	/// <param name="sConvertedWorkbookFile">
	///	Full path to the converted workbook this method will create.
	/// </param>
	///
	/// <returns>
	/// true if successful.
	/// </returns>
	//*************************************************************************

	protected Boolean
	ConvertOldWorkbook
	(
		String sOldWorkbookFile,
		String sConvertedWorkbookFile
	)
	{
		AssertValid();
		Debug.Assert( !String.IsNullOrEmpty(sOldWorkbookFile) );
		Debug.Assert( !String.IsNullOrEmpty(sConvertedWorkbookFile) );

		OldWorkbookConverter oOldWorkbookConverter =
			new OldWorkbookConverter();

		try
		{
			oOldWorkbookConverter.ConvertOldWorkbook(sOldWorkbookFile,
				sConvertedWorkbookFile, m_oApplication);
		}
		catch (OldWorkbookConversionException oOldWorkbookConversionException)
		{
			this.ShowWarning(oOldWorkbookConversionException.Message);

			return (false);
		}
		catch (Exception oException)
		{
			ErrorUtil.OnException(oException);

			return (false);
		}

		return (true);
	}

	//*************************************************************************
	//	Method: txbOldWorkbookFile_TextChanged()
	//
	/// <summary>
	///	Handles the TextChanged event on the txbOldWorkbookFile TextBox.
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
	txbOldWorkbookFile_TextChanged
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

		txbConvertedWorkbookFile.Text =
			OldToConvertedWorkbook(txbOldWorkbookFile.Text);
    }

	//*************************************************************************
	//	Method: btnBrowse_Click()
	//
	/// <summary>
	///	Handles the Click event on the btnBrowse button.
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
	btnBrowse_Click
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

		if (m_oOpenFileDialog.ShowDialog() == DialogResult.OK)
		{
			txbOldWorkbookFile.Text = m_oOpenFileDialog.FileName;
		}
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

		if (
			!DoDataExchange(true)
			||
			!ConvertOldWorkbook(m_sOldWorkbookFile, m_sConvertedWorkbookFile)
			)
		{
			return;
		}

		if (m_oConvertOldWorkbookDialogUserSettings.OpenConvertedWorkbook)
		{
			try
			{
				m_oApplication.Workbooks.Open(m_sConvertedWorkbookFile, 1,
					false, Missing.Value, Missing.Value, Missing.Value, false,
					Missing.Value, Missing.Value, false, Missing.Value,
					Missing.Value, false, true, Missing.Value);
			}
			catch (Exception)
			{
				this.ShowWarning("The converted workbook couldn't be opened.");

				return;
			}
		}

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

		Debug.Assert(m_oConvertOldWorkbookDialogUserSettings != null);
		Debug.Assert(m_oApplication != null);
		Debug.Assert(m_sOldWorkbookFile != null);
		Debug.Assert(m_sConvertedWorkbookFile != null);
		Debug.Assert(m_oOpenFileDialog != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// User settings for this dialog.

	protected ConvertOldWorkbookDialogUserSettings
		m_oConvertOldWorkbookDialogUserSettings;

	/// Excel application.

	protected Microsoft.Office.Interop.Excel.Application m_oApplication;

	/// Full path to the old workbook file, or String.Empty.

	protected String m_sOldWorkbookFile;

	/// Full path to the converted workbook file, or String.Empty.

	protected String m_sConvertedWorkbookFile;

	/// Dialog for selecting an old workbook.

	protected OpenFileDialog m_oOpenFileDialog;
}


//*****************************************************************************
//  Class: ConvertOldWorkbookDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="ConvertOldWorkbookDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("ConvertOldWorkbookDialog") ]

public class ConvertOldWorkbookDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: ConvertOldWorkbookDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="ConvertOldWorkbookDialogUserSettings" /> class.
    /// </summary>
	///
	/// <param name="oForm">
	/// The form to save settings for.
	/// </param>
    //*************************************************************************

    public ConvertOldWorkbookDialogUserSettings
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
    //  Property: OpenConvertedWorkbook
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the converted workbook should be
	/// opened.
    /// </summary>
    ///
    /// <value>
	/// true to open the converted workbook.  The default is true.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("true") ]

    public Boolean
    OpenConvertedWorkbook
    {
        get
        {
            AssertValid();

			return ( (Boolean)this[OpenConvertedWorkbookKey] );
        }

        set
        {
            this[OpenConvertedWorkbookKey] = value;

            AssertValid();
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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	/// Name of the settings key for the OpenConvertedWorkbook property.

	protected const String OpenConvertedWorkbookKey = "OpenConvertedWorkbook";
}
}
