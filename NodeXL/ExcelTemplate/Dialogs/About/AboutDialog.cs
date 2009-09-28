

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.NodeXL.ApplicationUtil;
using Microsoft.NodeXL.Common;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AboutDialog
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
    //  Constructor: AboutDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="AboutDialog" /> class.
    /// </summary>
    //*************************************************************************

    public AboutDialog()
    {
        InitializeComponent();

        // Instantiate an object that retrieves and saves the location of this
        // dialog.  Note that the object automatically saves the settings when
        // the form closes.

        m_oAboutDialogUserSettings = new AboutDialogUserSettings(this);

        lblVersion.Text = String.Format(

            "Version {0}"
            ,
            AssemblyUtil2.GetFileVersion()
            );

        lblCopyright.Text = String.Format(

            "Copyright © {0} Microsoft Corporation"
            ,
            DateTime.Now.Year
            );

        lnkDiscussionUrl.Text = ProjectInformation.DiscussionUrl;
        lnkDiscussionUrl.FileName = ProjectInformation.DiscussionUrl;

        ttToolTip.SetToolTip(lnkDiscussionUrl, String.Format(
        
            "Go to {0}"
            ,
            ProjectInformation.DiscussionUrl
            ) );

        // AssertValid();
    }

    //*************************************************************************
    //  Method: lnkTeam_LinkClicked()
    //
    /// <summary>
    /// Handles the LinkClicked event on the lnkTeam LinkLabel.
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
    lnkTeam_LinkClicked
    (
        object sender,
        LinkLabelLinkClickedEventArgs e
    )
    {
        AssertValid();

        this.ShowInformation( String.Format(

            "Microsoft NodeXL team members:\r\n\r\n{0}"
            ,
            ProjectInformation.TeamMembers
            ) );
    }

    //*************************************************************************
    //  Method: btnEnableAllNotifications_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnEnableAllNotifications button.
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
    btnEnableAllNotifications_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        NotificationUserSettings oNotificationUserSettings =
            new NotificationUserSettings();

        oNotificationUserSettings.EnableAllNotifications();
        oNotificationUserSettings.Save();
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

[ SettingsGroupNameAttribute("AboutDialog4") ]

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
