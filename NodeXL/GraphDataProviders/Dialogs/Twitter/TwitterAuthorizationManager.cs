
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Web;
using System.Diagnostics;

namespace Microsoft.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterAuthorizationManager
//
/// <summary>
/// Manages the process of authorizing NodeXL with Twitter.
/// </summary>
///
/// <remarks>
/// One of these objects is owned by the dialogs that import Twitter networks.
/// The dialogs should call <see cref="AuthorizeIfRequested" /> before the
/// network importation starts.
/// </remarks>
//*****************************************************************************

public class TwitterAuthorizationManager : Object
{
    //*************************************************************************
    //  Constructor: TwitterAuthorizationManager()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterAuthorizationManager" /> class.
    /// </summary>
    ///
    /// <param name="twitterAuthorizationControl">
    /// The TwitterAuthorizationControl owned by the parent dialog.  This class
    /// manages the control's <see cref="TwitterAuthorizationControl.Status" />
    /// property.
    /// </param>
    //*************************************************************************

    public TwitterAuthorizationManager
    (
        TwitterAuthorizationControl twitterAuthorizationControl 
    )
    {
        m_oTwitterAuthorizationControl = twitterAuthorizationControl;

        // If the user has already authorized, he doesn't need to do so again.
        // Otherwise, assume he doesn't have a Twitter account.

        m_oTwitterAuthorizationControl.Status =
            TwitterAccessToken.Exists() ?
                TwitterAuthorizationStatus.HasTwitterAccountAuthorized :
                TwitterAuthorizationStatus.NoTwitterAccount;

        AssertValid();
    }

    //*************************************************************************
    //  Method: AuthorizeIfRequested()
    //
    /// <summary>
    /// Authorizes NodeXL with Twitter if requested to do so.
    /// </summary>
    ///
    /// <returns>
    /// true if no authorization was requested, or if authorization succeeded.
    /// false if authorization was requested but failed.
    /// </returns>
    ///
    /// <exception cref="System.Net.WebException">
    /// Thrown if a problem occurs while requesting authorization.
    /// </exception>
    ///
    /// <remarks>
    /// If the <see cref="TwitterAuthorizationControl.Status" /> property on
    /// the TwitterAuthorizationControl owned by the parent dialog indicates
    /// that user wants to authorize with Twitter, this method runs the
    /// authorization process.  Otherwise, it does nothing.
    /// </remarks>
    //*************************************************************************

    public Boolean
    AuthorizeIfRequested()
    {
        AssertValid();

        switch (m_oTwitterAuthorizationControl.Status)
        {
            case TwitterAuthorizationStatus.NoTwitterAccount:

                // Delete any access token that exists.

                TwitterAccessToken.Delete();
                return (true);

            case TwitterAuthorizationStatus.HasTwitterAccountAuthorized:

                return (true);

            case TwitterAuthorizationStatus.HasTwitterAccountNotAuthorized:

                TwitterAccessToken.Delete();

                // Continue below.

                break;

            default:

                Debug.Assert(false);
                break;
        }

        // Get a Twitter request token.

        oAuthTwitter oTwitterAuth = new oAuthTwitter();
        Uri oAuthorizationUri = new Uri( oTwitterAuth.AuthorizationLinkGet() );

        String sRequestToken = HttpUtility.ParseQueryString(
            oAuthorizationUri.Query)["oauth_token"];

        Debug.Assert( !String.IsNullOrEmpty(sRequestToken) );

        // Open the Twitter authorization page.

        Process.Start( oAuthorizationUri.ToString() );

        // Tell the user that the Twitter authorization page has been opened in
        // a browser window, and get the PIN that Twitter provides after
        // authorization is complete.

        TwitterAuthorizingDialog oTwitterAuthorizingDialog =
            new TwitterAuthorizingDialog();

        if (oTwitterAuthorizingDialog.ShowDialog(
            m_oTwitterAuthorizationControl.ParentForm) != DialogResult.OK)
        {
            return (false);
        }

        // Convert the request token to an access token.

        oTwitterAuth.Token = sRequestToken;

        oTwitterAuth.AccessTokenGet(
            sRequestToken, oTwitterAuthorizingDialog.Pin);

        Debug.Assert( !String.IsNullOrEmpty(oTwitterAuth.Token) );
        Debug.Assert( !String.IsNullOrEmpty(oTwitterAuth.TokenSecret) );

        // Save the access token.

        TwitterAccessToken oTwitterAccessToken = new TwitterAccessToken();
        oTwitterAccessToken.Save(oTwitterAuth.Token, oTwitterAuth.TokenSecret);

        return (true);
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
        Debug.Assert(m_oTwitterAuthorizationControl != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The TwitterAuthorizationControl owned by the parent dialog.

    protected TwitterAuthorizationControl m_oTwitterAuthorizationControl;
}

}
