

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: RegisterUserDialog
//
/// <summary>
/// Allows the user to register for email updates.
/// </summary>
///
/// <remarks>
/// The form contains a single RegisterUser control that does all the work.
/// </remarks>
//*****************************************************************************

public partial class RegisterUserDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: RegisterUserDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterUserDialog" />
    /// class.
    /// </summary>
    //*************************************************************************

    public RegisterUserDialog()
    {
        InitializeComponent();

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oRegisterUserDialogUserSettings =
            new RegisterUserDialogUserSettings(this);

        AssertValid();
    }

    //*************************************************************************
    //  Method: usrRegisterUser_Done()
    //
    /// <summary>
    /// Handles the Done event on the usrRegisterUser UserControl.
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
    usrRegisterUser_Done
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

        Debug.Assert(m_oRegisterUserDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// User settings for this dialog.

    protected RegisterUserDialogUserSettings m_oRegisterUserDialogUserSettings;
}


//*****************************************************************************
//  Class: RegisterUserDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="RegisterUserDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("RegisterUserDialog2") ]

public class RegisterUserDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: RegisterUserDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="RegisterUserDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public RegisterUserDialogUserSettings
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
