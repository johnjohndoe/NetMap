using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.GraphDataProviders
{
//*****************************************************************************
//  Class: TwitterCredentialsControl
//
/// <summary>
/// UserControl that gets a user's Twitter credentials.
/// </summary>
///
/// <remarks>
/// Use the <see cref="ScreenName" /> and <see cref="Password" /> properties to
/// set and get the credentials.  Use <see cref="Validate" /> to validate the
/// credentials.
///
/// <para>
/// This control uses the following keyboard shortcuts: N, P
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class TwitterCredentialsControl : UserControl
{
    //*************************************************************************
    //  Constructor: TwitterCredentialsControl()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterCredentialsControl" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterCredentialsControl()
    {
        InitializeComponent();
        this.lnkRateLimiting.Text = RateLimitingLinkText;

        AssertValid();
    }

    //*************************************************************************
    //  Property: ScreenName
    //
    /// <summary>
    /// Gets or sets the user's screen name.
    /// </summary>
    ///
    /// <value>
    /// The user's screen name, as a String.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    public String
    ScreenName
    {
        get
        {
            AssertValid();

            return ( txbScreenName.Text.Trim() );
        }

        set
        {
            txbScreenName.Text = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Password
    //
    /// <summary>
    /// Gets or sets the user's password.
    /// </summary>
    ///
    /// <value>
    /// The user's password, as a String.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    public String
    Password
    {
        get
        {
            AssertValid();

            return ( txbPassword.Text.Trim() );
        }

        set
        {
            txbPassword.Text = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: Validate()
    //
    /// <summary>
    /// Validates the user's entries.
    /// </summary>
    ///
    /// <returns>
    /// true if validation passes.
    /// </returns>
    ///
    /// <remarks>
    /// If the user's entries are invalid, an error message is displayed and
    /// false is returned.  Otherwise, true is returned.
    /// </remarks>
    //*************************************************************************

    public new Boolean
    Validate()
    {
        AssertValid();

        String sScreenName = txbScreenName.Text.Trim();

        if ( !String.IsNullOrEmpty(sScreenName) )
        {
            txbScreenName.Text = sScreenName;

            String sPassword;

            if ( !FormUtil.ValidateRequiredTextBox(txbPassword,

                "If you want to use your Twitter account information, you"
                + " must enter a password.  Otherwise, clear your Twitter"
                + " screen name."
                ,
                out sPassword) )
            {
                return (false);
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: SetFocusToScreenName()
    //
    /// <summary>
    /// Sets focus to the screen name TextBox.
    /// </summary>
    //*************************************************************************

    public void
    SetFocusToScreenName()
    {
        AssertValid();

        txbScreenName.Focus();
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
        object sender, LinkLabelLinkClickedEventArgs e
    )
    {
        AssertValid();

        FormUtil.ShowInformation( String.Format(

            "To protect its Web service, Twitter limits the"
            + " number of information requests that can be made within a"
            + " one-hour period.  They call this \"rate limiting.\"  If you"
            + " attempt to include 1.5 or 2.0 levels in the network, you can"
            + " easily reach Twitter's limit."
            + "\r\n\r\n"
            + "We recommend that you either include only 1.0 levels, or ask"
            + " Twitter to lift the limit for you.  You can do this by"
            + " clicking the \"{0}\" link.  You must be a registered Twitter"
            + " user to do this.  Once Twitter has lifted the limit for you,"
            + " you should enter your Twitter account information when you"
            + " import a Twitter network."
            ,
            lnkRequestWhitelist.Text
            ) );
    }

    //*************************************************************************
    //  Method: lnkRequestWhitelist_LinkClicked()
    //
    /// <summary>
    /// Handles the LinkClicked event on the lnkRequestWhitelist LinkLabel.
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
    lnkRequestWhitelist_LinkClicked
    (
        object sender, LinkLabelLinkClickedEventArgs e
    )
    {
        AssertValid();

        Process.Start(TwitterRequestWhitelistUrl);
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
