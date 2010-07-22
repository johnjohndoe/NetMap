
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterAuthorizationControl
//
/// <summary>
/// UserControl that shows a user's Twitter authorization status and provides
/// some help links concerning whitelisting.
/// </summary>
///
/// <remarks>
/// Get and set the user's Twitter authorization status with the <see
/// cref="Status" /> property.
///
/// <para>
/// This control uses the following keyboard shortcuts: I, V, W.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class TwitterAuthorizationControl : UserControl
{
    //*************************************************************************
    //  Constructor: TwitterAuthorizationControl()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterAuthorizationControl" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterAuthorizationControl()
    {
        InitializeComponent();

        this.Status = TwitterAuthorizationStatus.NoTwitterAccount;

        this.lnkRateLimiting.Text = RateLimitingLinkText;
        this.lnkRequestWhitelist.FileName = TwitterRequestWhitelistUrl;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Status
    //
    /// <summary>
    /// Sets the user's Twitter authorization status.
    /// </summary>
    ///
    /// <value>
    /// The user's Twitter authorization status, as a <see
    /// cref="TwitterAuthorizationStatus" />.
    /// </value>
    //*************************************************************************

    public TwitterAuthorizationStatus
    Status
    {
        set
        {
            // Note that unless the status is HasTwitterAccountAuthorized, the
            // radHasTwitterAccountAuthorized radio button is disabled.  The
            // user can't just declare that he has authorized NodeXL to use
            // his account.  That is determined by the
            // TwitterAuthorizationManager that sets this property.

            Boolean bEnableHasTwitterAccountAuthorized = false;

            switch (value)
            {
                case TwitterAuthorizationStatus.NoTwitterAccount:

                    radNoTwitterAccount.Checked = true;
                    break;

                case TwitterAuthorizationStatus.HasTwitterAccountNotAuthorized:

                    radHasTwitterAccountNotAuthorized.Checked = true;
                    break;

                case TwitterAuthorizationStatus.HasTwitterAccountAuthorized:

                    radHasTwitterAccountAuthorized.Checked = true;
                    bEnableHasTwitterAccountAuthorized = true;
                    break;

                default:

                    Debug.Assert(false);
                    break;
            }

            radHasTwitterAccountAuthorized.Enabled =
                bEnableHasTwitterAccountAuthorized;

            AssertValid();
        }

        get
        {
            AssertValid();

            if (radNoTwitterAccount.Checked)
            {
                return (TwitterAuthorizationStatus.NoTwitterAccount);
            }

            if (radHasTwitterAccountNotAuthorized.Checked)
            {
                return (
                    TwitterAuthorizationStatus.HasTwitterAccountNotAuthorized);
            }

            return (TwitterAuthorizationStatus.HasTwitterAccountAuthorized);
        }
    }

    //*************************************************************************
    //  Method: lnkRateLimiting_LinkClicked()
    //
    /// <summary>
    /// Handles the LinkClicked event on the lnkRateLimiting LinkLabel.
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
    lnkRateLimiting_LinkClicked
    (
        object sender,
        LinkLabelLinkClickedEventArgs e
    )
    {
        AssertValid();

        FormUtil.ShowInformation( String.Format(

            "To protect its Web service, Twitter limits the number of"
            + " information requests that can be made within a one-hour"
            + " period.  They call this \"rate limiting.\"  Depending on the"
            + " types of networks you import, you can easily reach Twitter's"
            + " limit."
            + "\r\n\r\n"
            + "The exact limit that Twitter imposes depends on several"
            + " factors.  If you do not have a Twitter account, or you do have"
            + " an account but you have not yet authorized NodeXL to use it to"
            + " import Twitter networks, then Twitter imposes a severe limit."
            + "  If you have authorized NodeXL to use your account, the limit"
            + " is a bit higher."
            + "\r\n\r\n"
            + "The most generous limit is granted to people who have asked"
            + " Twitter to lift the limit for their account and who have"
            + " authorized NodeXL to use their account.  You can ask Twitter"
            + " to lift the limit for you by clicking the \"{0}\" link."
            ,
            lnkRequestWhitelist.Text
            ) );
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
    //  Public constants
    //*************************************************************************

    /// Text for the lnkRateLimiting LinkLabel.

    public const String RateLimitingLinkText = 
        "Why this might not work: Twitter rate limiting";


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Twitter Web page for requesting whitelisting.

    protected const String TwitterRequestWhitelistUrl =
        "http://twitter.com/help/request_whitelisting";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}
}
