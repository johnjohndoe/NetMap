
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AutomatedGraphImageUserSettingsDialog
//
/// <summary>
/// Edits a <see cref="AutomatedGraphImageUserSettings" /> object.
/// </summary>
///
/// <remarks>
/// Pass an <see cref="AutomatedGraphImageUserSettings" /> object to the
/// constructor.  If the user edits the object, <see
/// cref="Form.ShowDialog()" /> returns DialogResult.OK.  Otherwise, the object
/// is not modified and <see cref="Form.ShowDialog()" /> returns
/// DialogResult.Cancel.
/// </remarks>
//*****************************************************************************

public partial class AutomatedGraphImageUserSettingsDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: AutomatedGraphImageUserSettingsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AutomatedGraphImageUserSettingsDialog" /> class.
    /// </summary>
    ///
    /// <param name="automatedGraphImageUserSettingsUserSettings">
    /// The object being edited.
    /// </param>
    //*************************************************************************

    public AutomatedGraphImageUserSettingsDialog
    (
        AutomatedGraphImageUserSettings
            automatedGraphImageUserSettingsUserSettings
    )
    {
        Debug.Assert(automatedGraphImageUserSettingsUserSettings != null);
        automatedGraphImageUserSettingsUserSettings.AssertValid();

        m_oAutomatedGraphImageUserSettings =
            automatedGraphImageUserSettingsUserSettings;

        // Instantiate an object that saves and retrieves the position of this
        // dialog.  Note that the object automatically saves the settings when
        // the form closes.

        m_oAutomatedGraphImageUserSettingsDialogUserSettings =
            new AutomatedGraphImageUserSettingsDialogUserSettings(this);

        InitializeComponent();
        DoDataExchange(false);

        AssertValid();
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
            if ( !usrImageFormat.Validate() )
            {
                return (false);
            }

            m_oAutomatedGraphImageUserSettings.ImageSizePx =
                usrImageFormat.ImageSizePx;

            m_oAutomatedGraphImageUserSettings.ImageFormat =
                usrImageFormat.ImageFormat;
        }
        else
        {
            usrImageFormat.ImageSizePx = 
                m_oAutomatedGraphImageUserSettings.ImageSizePx;

            usrImageFormat.ImageFormat = 
                m_oAutomatedGraphImageUserSettings.ImageFormat;
        }

        return (true);
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

    public  override void
    AssertValid()
    {
        base.AssertValid();

        Debug.Assert(m_oAutomatedGraphImageUserSettings != null);

        Debug.Assert(m_oAutomatedGraphImageUserSettingsDialogUserSettings !=
            null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object whose properties are being edited.

    protected AutomatedGraphImageUserSettings
        m_oAutomatedGraphImageUserSettings;

    /// User settings for this dialog.

    protected AutomatedGraphImageUserSettingsDialogUserSettings
        m_oAutomatedGraphImageUserSettingsDialogUserSettings;
}


//*****************************************************************************
//  Class: AutomatedGraphImageUserSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="AutomatedGraphImageUserSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("AutomatedGraphImageUserSettingsDialog") ]

public class AutomatedGraphImageUserSettingsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: AutomatedGraphImageUserSettingsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AutomatedGraphImageUserSettingsDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public AutomatedGraphImageUserSettingsDialogUserSettings
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
