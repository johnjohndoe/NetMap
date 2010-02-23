

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.Common
{
//*****************************************************************************
//  Class: RegisterUserControl
//
/// <summary>
/// Registers a user by creating an email.
/// </summary>
///
/// <remarks>
/// The dialog includes a button for creating a registration email via the
/// mailto protocol.
///
/// <para>
/// The parent form should handle the <see cref="Done" /> event and close
/// itself when the event is fired.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class RegisterUserControl : UserControl
{
    //*************************************************************************
    //  Constructor: RegisterUserControl()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterUserControl" />
    /// class.
    /// </summary>
    //*************************************************************************

    public RegisterUserControl()
    {
        InitializeComponent();

        this.grpCreateRegistrationEmail.Enabled =
            UserRegisterer.CanCreateRegistrationEmail();

        AssertValid();
    }

    //*************************************************************************
    //  Event: Done
    //
    /// <summary>
    /// Occurs when the user is done.
    /// </summary>
    ///
    /// <remarks>
    /// The parent form should handle this event by closing itself.
    /// </remarks>
    //*************************************************************************

    public event EventHandler Done;


    //*************************************************************************
    //  Method: FireDone()
    //
    /// <summary>
    /// Fires the <see cref="Done" /> event if appropriate.
    /// </summary>
    //*************************************************************************

    private void
    FireDone()
    {
        AssertValid();

        EventHandler oDone = this.Done;

        if (oDone != null)
        {
            oDone(this, EventArgs.Empty);
        }
    }

    //*************************************************************************
    //  Method: lnkPrivacy_LinkClicked()
    //
    /// <summary>
    /// Handles the LinkClicked event on the lnkPrivacy LinkButton.
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
    lnkPrivacy_LinkClicked
    (
        object sender,
        LinkLabelLinkClickedEventArgs e
    )
    {
        AssertValid();

        FormUtil.ShowInformation( String.Format(

            "We are pleased that you are using Microsoft NodeXL."
            + "\r\n\r\n"
            + "Because NodeXL is a research project, we are interested in"
            + " occasionally contacting you to ask about your experiences with"
            + " NodeXL and to inform you about updates, success"
            + " stories, related publications, bugs, and so on."
            + "\r\n\r\n"
            + "We will not sell or share this email list with other"
            + " individuals or organizations.  This list is only for the"
            + " NodeXL project."
            + "\r\n\r\n"
            + "If you have questions or comments, please go to {0}."
            + "\r\n\r\n"
            + "(Last updated February 17, 2010.)"
            ,
            ProjectInformation.DiscussionUrl
            ) );
    }

    //*************************************************************************
    //  Method: btnCreateRegistrationEmail_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnCreateRegistrationEmail button.
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
    btnCreateRegistrationEmail_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        if (!UserRegisterer.TryCreateRegistrationEmail())
        {
            FormUtil.ShowWarning(
                "The email couldn't be created.  There might not be an email"
                + " program installed on this computer."
                );
        }
    }

    //*************************************************************************
    //  Method: btnCopyRegistrationAddress_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnCopyRegistrationAddress button.
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
    btnCopyRegistrationAddress_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        Clipboard.SetText(ProjectInformation.RegistrationEmailAddress);
    }

    //*************************************************************************
    //  Method: btnCancel_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnCancel button.
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
    btnCancel_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        FireDone();
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")] 

    public void
    AssertValid()
    {
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
