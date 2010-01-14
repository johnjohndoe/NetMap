

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: BackgroundDialog
//
/// <summary>
/// Edits the background properties in a <see cref="PerWorkbookSettings" />
/// object.
/// </summary>
///
/// <remarks>
/// Pass a <see cref="PerWorkbookSettings" /> object to the constructor.  If
/// the user edits the object, <see cref="Form.ShowDialog()" /> returns
/// DialogResult.OK.  Otherwise, the object is not modified and <see
/// cref="Form.ShowDialog()" /> returns DialogResult.Cancel.
/// </remarks>
//*****************************************************************************

public partial class BackgroundDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: BackgroundDialog()
    //
    /// <overloads>
    /// Initializes a new instance of the <see cref="BackgroundDialog" />
    /// class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="BackgroundDialog" /> class with a PerWorkbookSettings object.
    /// </summary>
    ///
    /// <param name="perWorkbookSettings">
    /// The object being edited.
    /// </param>
    //*************************************************************************

    public BackgroundDialog
    (
        PerWorkbookSettings perWorkbookSettings
    )
    : this()
    {
        Debug.Assert(perWorkbookSettings != null);
        perWorkbookSettings.AssertValid();

        m_oPerWorkbookSettings = perWorkbookSettings;

        m_oOpenFileDialog = new OpenFileDialog();

        m_oOpenFileDialog.Filter =
            "All files (*.*)|*.*|" + SaveableImageFormats.Filter
            ;

        m_oOpenFileDialog.Title = "Browse for Background Image";

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oBackgroundDialogUserSettings =
            new BackgroundDialogUserSettings(this);

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: BackgroundDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="BackgroundDialog" /> class
    /// for the Visual Studio designer.
    /// </summary>
    ///
    /// <remarks>
    /// Do not use this constructor.  It is for use by the Visual Studio
    /// designer only.
    /// </remarks>
    //*************************************************************************

    public BackgroundDialog()
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
        Nullable<Color> oBackColor;
        String sImageUri;

        if (bFromControls)
        {
            if (radUseBackColor.Checked)
            {
                oBackColor = new Nullable<Color>(usrBackColor.Color);
            }
            else
            {
                oBackColor = new Nullable<Color>();
            }

            if (radUseImage.Checked)
            {
                if ( !ValidateRequiredTextBox(txbImageUri,

                    "Enter or browse for a full path to an image file on your"
                    + " computer or local network, or enter an URL to an image"
                    + " on the Internet.",

                    out sImageUri) )
                {
                    return (false);
                }
            }
            else
            {
                sImageUri = null;
            }

            m_oPerWorkbookSettings.BackColor = oBackColor;
            m_oPerWorkbookSettings.BackgroundImageUri = sImageUri;
        }
        else
        {
            oBackColor = m_oPerWorkbookSettings.BackColor;
            Boolean bUseBackColor = oBackColor.HasValue;

            radUseDefaultBackColor.Checked =
                !(radUseBackColor.Checked = bUseBackColor);

            if (bUseBackColor)
            {
                usrBackColor.Color = oBackColor.Value;
            }

            sImageUri = m_oPerWorkbookSettings.BackgroundImageUri;
            Boolean bUseImage = !String.IsNullOrEmpty(sImageUri);
            radNoImage.Checked = !(radUseImage.Checked = bUseImage);
            txbImageUri.Text = sImageUri;

            EnableControls();
        }

        return (true);
    }

    //*************************************************************************
    //  Method: EnableControls()
    //
    /// <summary>
    /// Enables or disables the dialog's controls.
    /// </summary>
    //*************************************************************************

    protected void
    EnableControls()
    {
        AssertValid();

        usrBackColor.Enabled = radUseBackColor.Checked;
        txbImageUri.Enabled = btnBrowse.Enabled = radUseImage.Checked;
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
            txbImageUri.Text = m_oOpenFileDialog.FileName;
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

        Debug.Assert(m_oPerWorkbookSettings != null);
        Debug.Assert(m_oBackgroundDialogUserSettings != null);
        Debug.Assert(m_oOpenFileDialog != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object whose properties are being edited.

    protected PerWorkbookSettings m_oPerWorkbookSettings;

    /// User settings for this dialog.

    protected BackgroundDialogUserSettings m_oBackgroundDialogUserSettings;

    /// Dialog for selecting an image file.

    protected OpenFileDialog m_oOpenFileDialog;
}


//*****************************************************************************
//  Class: BackgroundDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="BackgroundDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("BackgroundDialog") ]

public class BackgroundDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: BackgroundDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="BackgroundDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public BackgroundDialogUserSettings
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
