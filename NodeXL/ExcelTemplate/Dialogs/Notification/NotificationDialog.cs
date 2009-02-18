

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: NotificationDialog
//
/// <summary>
/// Displays a notification message, yes and no buttons, and a checkbox for
/// disabling the notification in the future.
/// </summary>
///
/// <remarks>
/// Pass the notification message to the constructor, then call <see
/// cref="Form.ShowDialog()" />.  When <see cref="Form.ShowDialog()" />
/// returns, read <see cref="DisableFutureNotifications" /> to check whether
/// the user wants to disable future notifications.
///
/// <para>
/// The dialog does not resize itself to the message.  This should be a future
/// improvement.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class NotificationDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: NotificationDialog()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
    /// cref="NotificationDialog" /> class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="NotificationDialog" /> class with a window title, icon, and
    /// notification message.
    /// </summary>
    ///
    /// <param name="title">
    /// Window title.
    /// </param>
    ///
    /// <param name="systemIcon">
    /// Icon to use within the dialog.  Should be a member of the SystemIcons
    /// class.
    /// </param>
    ///
    /// <param name="message">
    /// Notification message.
    /// </param>
    //*************************************************************************

    public NotificationDialog
    (
        String title,
        Icon systemIcon,
        String message
    )
    : this()
    {
        Debug.Assert( !String.IsNullOrEmpty(title) );
        Debug.Assert(systemIcon != null);
        Debug.Assert( !String.IsNullOrEmpty(message) );

        // Instantiate an object that saves and retrieves the size and position
        // settings of this dialog.  Note that the object automatically saves
        // the settings when the form closes.

        m_oNotificationDialogUserSettings =
            new NotificationDialogUserSettings(this);

        this.Text = title;
        picNotification.Image = Bitmap.FromHicon(systemIcon.Handle);
        lblMessage.Text = message;

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: NotificationDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="NotificationDialog" /> class for the Visual Studio
    /// designer.
    /// </summary>
    ///
    /// <remarks>
    /// Do not use this constructor.  It is for use by the Visual Studio
    /// designer only.
    /// </remarks>
    //*************************************************************************

    public NotificationDialog()
    {
        InitializeComponent();

        // AssertValid();
    }

    //*************************************************************************
    //  Property: DisableFutureNotifications
    //
    /// <summary>
    /// Gets or sets the state of the checkbox that indicates whether the user
    /// wants to disable future notifications.
    /// </summary>
    ///
    /// <value>
    /// true to disable future notifications.  The default is false.
    /// </value>
    //*************************************************************************

    public Boolean
    DisableFutureNotifications
    {
        get
        {
            AssertValid();

            return (chkDisableFutureNotifications.Checked);
        }

        set
        {
            chkDisableFutureNotifications.Checked = value;

            AssertValid();
        }
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
            // (Do nothing.)
        }
        else
        {
            // (Do nothing.)
        }

        return (true);
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

        Debug.Assert(m_oNotificationDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// User settings for this dialog.

    protected NotificationDialogUserSettings m_oNotificationDialogUserSettings;
}


//*****************************************************************************
//  Class: NotificationDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="NotificationDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("NotificationDialog") ]

public class NotificationDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: NotificationDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="NotificationDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public NotificationDialogUserSettings
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
