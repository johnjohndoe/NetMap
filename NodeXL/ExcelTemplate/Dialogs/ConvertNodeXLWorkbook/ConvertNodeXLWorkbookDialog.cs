

//  Copyright (c) Microsoft Corporation.  All rights reserved.

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
//  Class: ConvertNodeXLWorkbookDialog
//
/// <summary>
/// Copies a NodeXL workbook created on another machine and converts the copy
/// to work on this machine.
/// </summary>
///
/// <remarks>
/// A <see cref="NodeXLWorkbookConverter" /> object does most of the work.
/// </remarks>
//*****************************************************************************

public partial class ConvertNodeXLWorkbookDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: ConvertNodeXLWorkbookDialog()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
    /// cref="ConvertNodeXLWorkbookDialog" /> class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ConvertNodeXLWorkbookDialog" /> class with an Excel Application
    /// object.
    /// </summary>
    ///
    /// <param name="application">
    /// Excel application.
    /// </param>
    //*************************************************************************

    public ConvertNodeXLWorkbookDialog
    (
        Microsoft.Office.Interop.Excel.Application application
    )
    : this()
    {
        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oConvertNodeXLWorkbookDialogUserSettings =
            new ConvertNodeXLWorkbookDialogUserSettings(this);

        m_oApplication = application;
        m_sOtherWorkbookFile = String.Empty;
        m_sConvertedWorkbookFile = String.Empty;

        m_oOpenFileDialog = new OpenFileDialog();

        m_oOpenFileDialog.Filter =
            "Excel Workbook (*.xlsx)|*.xlsx|All files (*.*)|*.*";

        m_oOpenFileDialog.Title = "Browse for NodeXL Workbook";

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: ConvertNodeXLWorkbookDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ConvertNodeXLWorkbookDialog" /> class for the Visual Studio
    /// designer.
    /// </summary>
    ///
    /// <remarks>
    /// Do not use this constructor.  It is for use by the Visual Studio
    /// designer only.
    /// </remarks>
    //*************************************************************************

    public ConvertNodeXLWorkbookDialog()
    {
        InitializeComponent();

        // AssertValid();
    }

    //*************************************************************************
    //  Method: DoDataExchange()
    //
    /// <summary>
    /// Transfers data between the dialog's fields and its controls.
    /// </summary>
    ///
    /// <param name="bFromControls">
    /// true to transfer data from the dialog's controls to its fields, false
    /// for the other direction.
    /// </param>
    ///
    /// <returns>
    /// true if the transfer was successful.
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
            if ( !this.ValidateFileTextBox(txbOtherWorkbookFile,
                "Enter or browse for a NodeXL workbook.",
                out m_sOtherWorkbookFile) )
            {
                return (false);
            }

            m_sConvertedWorkbookFile = txbConvertedWorkbookFile.Text;

            // The txbOtherWorkbookFile_TextChanged() event handler guarantees
            // that the converted workbook file will not be null or empty if
            // the other file exists.

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

            m_oConvertNodeXLWorkbookDialogUserSettings.OpenConvertedWorkbook =
                cbxOpenConvertedWorkbook.Checked;
        }
        else
        {
            txbOtherWorkbookFile.Text = m_sOtherWorkbookFile;
            txbConvertedWorkbookFile.Text = m_sConvertedWorkbookFile;

            cbxOpenConvertedWorkbook.Checked =
                m_oConvertNodeXLWorkbookDialogUserSettings.
                    OpenConvertedWorkbook;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: OtherToConvertedWorkbook()
    //
    /// <summary>
    /// Derives a path to a converted workbook given the other workbook path.
    /// </summary>
    ///
    /// <param name="sOtherWorkbookFile">
    /// Full path to the other workbook.  The workbook may or may not exist.
    /// Can be null or empty.
    /// </param>
    ///
    /// <returns>
    /// Full path to use for the converted workbook, or String.Empty if the
    /// other workbook doesn't exist.
    /// </returns>
    //*************************************************************************

    protected String
    OtherToConvertedWorkbook
    (
        String sOtherWorkbookFile
    )
    {
        AssertValid();

        if (
            String.IsNullOrEmpty(sOtherWorkbookFile)
            ||
            !File.Exists(sOtherWorkbookFile)
            )
        {
            return (String.Empty);
        }

        return (
            Path.Combine(
                Path.GetDirectoryName(sOtherWorkbookFile),
                Path.GetFileNameWithoutExtension(sOtherWorkbookFile)
                    + "-Copy"
                )
            + Path.GetExtension(sOtherWorkbookFile)
            );
    }

    //*************************************************************************
    //  Method: ConvertNodeXLWorkbook()
    //
    /// <summary>
    /// Copies a NodeXL workbook created on another machine and converts the
    /// copy to work on this machine.
    /// </summary>
    ///
    /// <param name="sOtherWorkbookFile">
    /// Full path to the other workbook.  The workbook must exist.
    /// </param>
    ///
    /// <param name="sConvertedWorkbookFile">
    /// Full path to the converted workbook this method will create.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    ConvertNodeXLWorkbook
    (
        String sOtherWorkbookFile,
        String sConvertedWorkbookFile
    )
    {
        AssertValid();
        Debug.Assert( !String.IsNullOrEmpty(sOtherWorkbookFile) );
        Debug.Assert( !String.IsNullOrEmpty(sConvertedWorkbookFile) );

        NodeXLWorkbookConverter oNodeXLWorkbookConverter =
            new NodeXLWorkbookConverter();

        try
        {
            oNodeXLWorkbookConverter.ConvertNodeXLWorkbook(sOtherWorkbookFile,
                sConvertedWorkbookFile, m_oApplication);
        }
        catch (NodeXLWorkbookConversionException
            oNodeXLWorkbookConversionException)
        {
            this.ShowWarning(oNodeXLWorkbookConversionException.Message);

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
    //  Method: txbOtherWorkbookFile_TextChanged()
    //
    /// <summary>
    /// Handles the TextChanged event on the txbOtherWorkbookFile TextBox.
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
    txbOtherWorkbookFile_TextChanged
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        txbConvertedWorkbookFile.Text =
            OtherToConvertedWorkbook(txbOtherWorkbookFile.Text);
    }

    //*************************************************************************
    //  Method: btnBrowse_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnBrowse button.
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
    btnBrowse_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        if (m_oOpenFileDialog.ShowDialog() == DialogResult.OK)
        {
            txbOtherWorkbookFile.Text = m_oOpenFileDialog.FileName;
        }
    }

    //*************************************************************************
    //  Method: btnOK_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnOK button.
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
            !ConvertNodeXLWorkbook(m_sOtherWorkbookFile,
                m_sConvertedWorkbookFile)
            )
        {
            return;
        }

        if (m_oConvertNodeXLWorkbookDialogUserSettings.OpenConvertedWorkbook)
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

        Debug.Assert(m_oConvertNodeXLWorkbookDialogUserSettings != null);
        Debug.Assert(m_oApplication != null);
        Debug.Assert(m_sOtherWorkbookFile != null);
        Debug.Assert(m_sConvertedWorkbookFile != null);
        Debug.Assert(m_oOpenFileDialog != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// User settings for this dialog.

    protected ConvertNodeXLWorkbookDialogUserSettings
        m_oConvertNodeXLWorkbookDialogUserSettings;

    /// Excel application.

    protected Microsoft.Office.Interop.Excel.Application m_oApplication;

    /// Full path to the other workbook file, or String.Empty.

    protected String m_sOtherWorkbookFile;

    /// Full path to the converted workbook file, or String.Empty.

    protected String m_sConvertedWorkbookFile;

    /// Dialog for selecting the other workbook.

    protected OpenFileDialog m_oOpenFileDialog;
}


//*****************************************************************************
//  Class: ConvertNodeXLWorkbookDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="ConvertNodeXLWorkbookDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("ConvertNodeXLWorkbookDialog") ]

public class ConvertNodeXLWorkbookDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: ConvertNodeXLWorkbookDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ConvertNodeXLWorkbookDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public ConvertNodeXLWorkbookDialogUserSettings
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
