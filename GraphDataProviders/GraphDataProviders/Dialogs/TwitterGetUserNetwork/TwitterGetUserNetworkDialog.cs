

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.SocialNetworkLib;

namespace Microsoft.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterGetUserNetworkDialog
//
/// <summary>
/// Gets the network of people followed by a Twitter user or people whom a
/// Twitter user follows.
/// </summary>
///
/// <remarks>
/// Call <see cref="Form.ShowDialog()" /> to show the dialog.  If it returns
/// DialogResult.OK, get the related tags from the <see
/// cref="GraphDataProviderDialogBase.Results" /> property.
/// </remarks>
//*****************************************************************************

public partial class TwitterGetUserNetworkDialog : GraphDataProviderDialogBase
{
    //*************************************************************************
    //  Constructor: TwitterGetUserNetworkDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterGetUserNetworkDialog" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterGetUserNetworkDialog()
    :
    base ( new TwitterNetworkAnalyzer() )
    {
        InitializeComponent();

        // m_sScreenNameToAnalyze
        // m_bIncludeFollowed
        // m_bIncludeFollowers
        // m_eNetworkLevel
        // m_bIncludeLatestStatus
        // m_iMaximumPeoplePerRequest
        // m_sCredentialsScreenName
        // m_sCredentialsPassword

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

    protected override Boolean
    DoDataExchange
    (
        Boolean bFromControls
    )
    {
        if (bFromControls)
        {
            // Validate the controls.

            if (
                !ValidateRequiredTextBox(txbScreenNameToAnalyze,
                    "Enter the screen name of a Twitter user.",
                    out m_sScreenNameToAnalyze)
                ||
                !usrTwitterCredentials.Validate()
                )
            {
                return (false);
            }

            m_bIncludeFollowed = m_bIncludeFollowers = false;

            if (radIncludeFollowed.Checked)
            {
                m_bIncludeFollowed = true;
            }
            else if (radIncludeFollowers.Checked)
            {
                m_bIncludeFollowers = true;
            }
            else
            {
                m_bIncludeFollowed = m_bIncludeFollowers = true;
            }

            if ( m_bIncludeFollowers &&
                String.IsNullOrEmpty(usrTwitterCredentials.ScreenName) )
            {
                this.ShowWarning(
                    "Twitter requires your account information before it"
                    + " will provide lists of followers.  Either switch to"
                    + " \"Followed\" or enter your screen name and password."
                    );

                usrTwitterCredentials.SetFocusToScreenName();
                return (false);
            }

            m_eNetworkLevel = usrNetworkLevel.Level;
            m_bIncludeLatestStatus = chkIncludeLatestStatus.Checked;
            m_iMaximumPeoplePerRequest = usrLimitToNPeople.N;
            m_sCredentialsScreenName = usrTwitterCredentials.ScreenName;
            m_sCredentialsPassword = usrTwitterCredentials.Password;
        }
        else
        {
            txbScreenNameToAnalyze.Text = m_sScreenNameToAnalyze;

            if (m_bIncludeFollowed)
            {
                if (m_bIncludeFollowers)
                {
                    radIncludeFollowedAndFollowers.Checked = true;
                }
                else
                {
                    radIncludeFollowed.Checked = true;
                }
            }
            else
            {
                radIncludeFollowers.Checked = true;
            }

            usrNetworkLevel.Level = m_eNetworkLevel;
            chkIncludeLatestStatus.Checked = m_bIncludeLatestStatus;
            usrLimitToNPeople.N = m_iMaximumPeoplePerRequest;
            usrTwitterCredentials.ScreenName = m_sCredentialsScreenName;
            usrTwitterCredentials.Password = m_sCredentialsPassword;

            EnableControls();
        }

        return (true);
    }

    //*************************************************************************
    //  Method: StartAnalysis()
    //
    /// <summary>
    /// Starts the Twitter analysis.
    /// </summary>
    ///
    /// <remarks>
    /// It's assumed that DoDataExchange(true) was called and succeeded.
    /// </remarks>
    //*************************************************************************

    protected override void
    StartAnalysis()
    {
        AssertValid();

        m_oGraphMLXmlDocument = null;

        Debug.Assert(m_oHttpNetworkAnalyzer is TwitterNetworkAnalyzer);

        String sCredentialsScreenName =
            String.IsNullOrEmpty(m_sCredentialsScreenName) ? null :
            m_sCredentialsScreenName;

        ( (TwitterNetworkAnalyzer)m_oHttpNetworkAnalyzer ).
            GetUserNetworkAsync(m_sScreenNameToAnalyze, m_bIncludeFollowed,
                m_bIncludeFollowers, m_eNetworkLevel, m_bIncludeLatestStatus,
                m_iMaximumPeoplePerRequest, sCredentialsScreenName,
                m_sCredentialsPassword);
    }

    //*************************************************************************
    //  Method: EnableControls()
    //
    /// <summary>
    /// Enables or disables the dialog's controls.
    /// </summary>
    //*************************************************************************

    protected override void
    EnableControls()
    {
        AssertValid();

        Boolean bIsBusy = m_oHttpNetworkAnalyzer.IsBusy;

        EnableControls(!bIsBusy, pnlUserInputs);
        btnOK.Enabled = !bIsBusy;
        this.UseWaitCursor = bIsBusy;
    }

    //*************************************************************************
    //  Method: OnEmptyGraph()
    //
    /// <summary>
    /// Handles the case where a graph was successfully obtained by is empty.
    /// </summary>
    //*************************************************************************

    protected override void
    OnEmptyGraph()
    {
        AssertValid();

        this.ShowInformation("There are no people in that network.");
        txbScreenNameToAnalyze.Focus();
    }

    //*************************************************************************
    //  Method: OnAnalysisException()
    //
    /// <summary>
    /// Handles the AnalysisCompleted event on the TwitterNetworkAnalyzer
    /// object when an exception occurs.
    /// </summary>
    ///
    /// <param name="oException">
    /// The exception that occurred.
    /// </param>
    //*************************************************************************

    protected override void
    OnAnalysisException
    (
        Exception oException
    )
    {
        Debug.Assert(oException != null);
        AssertValid();

        String sMessage = null;

        const String TimeoutMessage =
            "The Twitter Web service didn't respond.";

        if (oException is WebException)
        {
            WebException oWebException = (WebException)oException;

            if (oWebException.Response is HttpWebResponse)
            {
                HttpWebResponse oHttpWebResponse =
                    (HttpWebResponse)oWebException.Response;

                switch (oHttpWebResponse.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:  // HTTP 401.

                        sMessage =
                            "The Twitter Web service reports that you are"
                            + " not authorized to request information.  This"
                            + " can occur if you enter the wrong screen name"
                            + " or password for your Twitter account."
                            ;

                        break;

                    case HttpStatusCode.NotFound:  // HTTP 404.

                        sMessage =
                            "There is no Twitter user with that screen name."
                            ;

                        break;

                    case HttpStatusCode.RequestTimeout:  // HTTP 408.

                        sMessage = TimeoutMessage;
                        break;

                    case HttpStatusCode.BadRequest:  // HTTP 400.

                        sMessage = String.Format(

                            "The Twitter Web service refuses to provide any"
                            + " more user information because you have made"
                            + " too many requests in the last hour.  (Twitter"
                            + " limits information requests to prevent its"
                            + " service from being attacked.  Click the '{0}'"
                            + " link for details.)"
                            + "\r\n\r\n"
                            + " Wait 60 minutes and try again."
                            ,
                            TwitterCredentialsControl.RateLimitingLinkText
                            );

                        break;

                    case HttpStatusCode.Forbidden:  // HTTP 403.

                        sMessage =
                            "The Twitter Web service refused to provide"
                            + " information about the user."
                            ;

                        break;

                    default:

                        break;
                }
            }
            else
            {
                switch (oWebException.Status)
                {
                    case WebExceptionStatus.Timeout:

                        sMessage = TimeoutMessage;
                        break;

                    default:

                        break;
                }
            }
        }

        if (sMessage == null)
        {
            sMessage =
                "The Twitter network information couldn't be obtained."
                + "\r\n\r\nDetails:\r\n\r\n"
                + ExceptionUtil.GetMessageTrace(oException)
                ;
        }

        this.ShowWarning(sMessage);
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

    protected void
    btnOK_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        OnOKClick();
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

        Debug.Assert(m_sScreenNameToAnalyze != null);
        // m_bIncludeFollowed
        // m_bIncludeFollowers
        // m_eNetworkLevel
        // m_bIncludeLatestStatus
        // m_iMaximumPeoplePerRequest
        // m_sCredentialsScreenName
        // m_sCredentialsPassword
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The screen name of the Twitter user whose network should be analyzed.
    ///
    /// This is static so that the TextBox that displays it will retain its
    /// text between dialog invocations.  Most NodeXL dialogs persist control
    /// values via ApplicationSettingsBase, but this plugin does not have
    /// access to that and so it resorts to static fields.

    protected static String m_sScreenNameToAnalyze = "bob";

    /// true to include the people followed by the user.

    protected static Boolean m_bIncludeFollowed = true;

    /// true to include the people following by the user.

    protected static Boolean m_bIncludeFollowers = false;

    /// Network level to include.

    protected static NetworkLevel m_eNetworkLevel = NetworkLevel.One;

    /// true to include each user's latest status.

    protected static Boolean m_bIncludeLatestStatus;

    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.

    protected static Int32 m_iMaximumPeoplePerRequest = Int32.MaxValue;

    /// The screen name and password of the Twitter user whose credentials
    /// should be used.

    protected static String m_sCredentialsScreenName = String.Empty;
    ///
    protected static String m_sCredentialsPassword = String.Empty;
}
}
