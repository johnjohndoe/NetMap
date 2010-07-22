

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterAuthorizingDialog
//
/// <summary>
/// Tells the user that the Twitter authorization page has been opened in a
/// browser window, and accepts the PIN that Twitter provides after
/// authorization is complete.
/// </summary>
///
/// <remarks>
/// If <see cref="Form.ShowDialog()" /> returns DialogResult.OK, get the PIN
/// from the <see cref="Pin" /> property.
/// </remarks>
//*****************************************************************************

public partial class TwitterAuthorizingDialog : FormPlus
{
    //*************************************************************************
    //  Constructor: TwitterAuthorizing()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterAuthorizingDialog" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterAuthorizingDialog()
    {
        m_sPin = null;

        InitializeComponent();
        this.ShowInTaskbar = false;

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Property: Pin
    //
    /// <summary>
    /// Gets the PIN provided by Twitter.
    /// </summary>
    ///
    /// <value>
    /// The PIN provided by Twitter, as a String.
    /// </value>
    ///
    /// <remarks>
    /// Call this only if <see cref="Form.ShowDialog()" /> returns
    /// DialogResult.OK.
    /// </remarks>
    //*************************************************************************

    public String
    Pin
    {
        get
        {
            AssertValid();

            return (m_sPin);
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
            // Validate the controls.

            if ( !ValidateRequiredTextBox(txbPin,
                "Enter the PIN from the Twitter authorization Web page.",
                out m_sPin) )
            {
                return (false);
            }
        }
        else
        {
            // (Do nothing.)
        }

        return (true);
    }

    //*************************************************************************
    //  Method: lnkPrivacy_LinkClicked()
    //
    /// <summary>
    /// Handles the LinkClicked event on the lnkPrivacy LinkLabel.
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
        this.ShowInformation(
            "NodeXL won't actually access anything in your Twitter account,"
            + " unless you happen to be part of a Twitter network that you"
            + " choose to import into NodeXL."
            + "\r\n\r\n"
            + "NodeXL never has access to your Twitter password."
            + "\r\n\r\n"
            + "You can revoke NodeXL's authorization at any time from within"
            + " your Twitter account."
            + "\r\n\r\n"
            + "To avoid making you authorize NodeXL every time you want to"
            + " import a Twitter network, NodeXL saves an \"access token\" in"
            + " a file on your computer.  This is a long piece of random text"
            + " that Twitter uses to recognize you on subsequent imports."
            + "  The text does not contain your name or any of your Twitter"
            + " account information."
            + "\r\n\r\n"
            + "If you want to learn more about the Twitter authorization"
            + " process that NodeXL uses, enter \"Twitter OAuth\" in a search"
            + " engine."
            + "\r\n\r\n"
            + "(Last updated June 17, 2010.)"
            );
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
            this.DialogResult = DialogResult.OK;
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

        // m_sPin
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The PIN provided by Twitter.

    protected String m_sPin;
}
}
