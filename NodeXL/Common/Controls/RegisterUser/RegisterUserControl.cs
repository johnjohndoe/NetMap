

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
/// Registers a user by sending his email address to a Web service.
/// </summary>
///
/// <remarks>
/// Registration is handled within this control when the user clicks OK.  The
/// parent form should handle the <see cref="Done" /> event and
/// close itself when the event is fired.
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

        m_sEmailAddress = String.Empty;

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Event: Done
    //
    /// <summary>
    /// Occurs when the user registers or cancels.
    /// </summary>
    ///
    /// <remarks>
    /// The parent form should handle this event by closing itself.
    /// </remarks>
    //*************************************************************************

    public event EventHandler Done;


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
            if ( !FormUtil.ValidateRequiredTextBox(txbEmailAddress,
                "Enter an email address.", out m_sEmailAddress) )
            {
                return (false);
            }
        }
        else
        {
            txbEmailAddress.Text = m_sEmailAddress;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: RegisterTheUser()
    //
    /// <summary>
    /// Registers the user.
    /// </summary>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    RegisterTheUser()
    {
        AssertValid();

        UserRegisterer oUserRegisterer = new UserRegisterer();

        try
        {
            oUserRegisterer.RegisterUser(m_sEmailAddress);

            return (true);
        }
        catch (RegisterUserException oRegisterUserException)
        {
            FormUtil.ShowWarning(oRegisterUserException.Message);
        }
        catch (Exception oException)
        {
            FormUtil.ShowWarning( String.Format(

                "An unexpected problem occurred.\r\n\r\n"
                + "Details:\r\n\r\n"
                + "{0}"
                ,
                ExceptionUtil.GetMessageTrace(oException)
                ) );
        }

        return (false);
    }


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
            + "The following information will be registered:"
            + "\r\n\r\n"
            + "1. Your email address"
            + "\r\n"
            + "2. The version number of NodeXL"
            + "\r\n"
            + "3. The time you registered"
            + "\r\n\r\n"
            + "If you have questions or comments, please go to {0}."
            + "\r\n\r\n"
            + "(Last updated December 15, 2008.)"
            ,
            ProjectInformation.DiscussionUrl
            ) );
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

        if ( !DoDataExchange(true) )
        {
            return;
        }

        this.UseWaitCursor = true;

        if ( RegisterTheUser() )
        {
            FormUtil.ShowInformation(
                "Thank you for registering."
                );

            FireDone();
        }

        this.UseWaitCursor = false;
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
        Debug.Assert(m_sEmailAddress != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Email address to register.

    protected String m_sEmailAddress;
}

}
