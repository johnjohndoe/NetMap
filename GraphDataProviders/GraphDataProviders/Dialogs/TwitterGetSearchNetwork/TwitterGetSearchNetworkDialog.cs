

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;

namespace Microsoft.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterGetSearchNetworkDialog
//
/// <summary>
/// Gets the the network of people who have tweeted a specified search term.
/// </summary>
///
/// <remarks>
/// Call <see cref="Form.ShowDialog()" /> to show the dialog.  If it returns
/// DialogResult.OK, get the related tags from the <see
/// cref="GraphDataProviderDialogBase.Results" /> property.
/// </remarks>
//*****************************************************************************

public partial class TwitterGetSearchNetworkDialog :
    TwitterGraphDataProviderDialogBase
{
    //*************************************************************************
    //  Constructor: TwitterGetSearchNetworkDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterGetSearchNetworkDialog" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterGetSearchNetworkDialog()
    :
    base( new TwitterSearchNetworkAnalyzer() )
    {
        InitializeComponent();

        // m_sSearchTerm
        // m_bIncludeFollowedEdges
        // m_bIncludeRepliesToEdges
        // m_bIncludeMentionsEdges
        // m_iMaximumPeoplePerRequest
        // m_bIncludeStatuses
        // m_bIncludeStatistics

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
                !ValidateRequiredTextBox(txbSearchTerm,
                    "Enter one or more words to search for.",
                    out m_sSearchTerm)
                ||
                !usrTwitterCredentials.Validate()
                )
            {
                return (false);
            }

            String sSearchTermLower = m_sSearchTerm.ToLower();

            foreach (String sProhibitedTerm in new String [] {
                "near:",
                "within:"
                } )
            {
                if (sSearchTermLower.IndexOf(sProhibitedTerm) >= 0)
                {
                    OnInvalidTextBox(txbSearchTerm, String.Format(
                    
                        "Although you can use \"{0}\" on Twitter's own search"
                        + " page, Twitter doesn't allow it to be used when"
                        + " searching from another program, such as NodeXL."
                        + "  Remove the \"{0}\" and try again."
                        ,
                        sProhibitedTerm
                        ) );

                    return (false);
                }
            }

            m_bIncludeFollowedEdges = chkIncludeFollowedEdges.Checked;
            m_bIncludeRepliesToEdges = chkIncludeRepliesToEdges.Checked;
            m_bIncludeMentionsEdges = chkIncludeMentionsEdges.Checked;
            m_iMaximumPeoplePerRequest = usrLimitToNPeople.N;
            m_bIncludeStatuses = chkIncludeStatuses.Checked;
            m_bIncludeStatistics = chkIncludeStatistics.Checked;
            m_sCredentialsScreenName = usrTwitterCredentials.ScreenName;
            m_sCredentialsPassword = usrTwitterCredentials.Password;
        }
        else
        {
            txbSearchTerm.Text = m_sSearchTerm;
            chkIncludeFollowedEdges.Checked = m_bIncludeFollowedEdges;
            chkIncludeRepliesToEdges.Checked = m_bIncludeRepliesToEdges;
            chkIncludeMentionsEdges.Checked = m_bIncludeMentionsEdges;
            usrLimitToNPeople.N = m_iMaximumPeoplePerRequest;
            chkIncludeStatuses.Checked = m_bIncludeStatuses;
            chkIncludeStatistics.Checked = m_bIncludeStatistics;
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

        Debug.Assert(m_oHttpNetworkAnalyzer is TwitterSearchNetworkAnalyzer);

        String sCredentialsScreenName =
            String.IsNullOrEmpty(m_sCredentialsScreenName) ? null :
            m_sCredentialsScreenName;

        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude =

            (m_bIncludeStatuses ?
                TwitterSearchNetworkAnalyzer.WhatToInclude.Statuses : 0)
            |
            (m_bIncludeStatistics ?
                TwitterSearchNetworkAnalyzer.WhatToInclude.Statistics : 0)
            |
            (m_bIncludeFollowedEdges ?
                TwitterSearchNetworkAnalyzer.WhatToInclude.FollowedEdges : 0)
            |
            (m_bIncludeRepliesToEdges ?
                TwitterSearchNetworkAnalyzer.WhatToInclude.RepliesToEdges : 0)
            |
            (m_bIncludeMentionsEdges ?
                TwitterSearchNetworkAnalyzer.WhatToInclude.MentionsEdges : 0)
            ;

        ( (TwitterSearchNetworkAnalyzer)m_oHttpNetworkAnalyzer ).
            GetSearchNetworkAsync(m_sSearchTerm, eWhatToInclude,
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
    //  Method: OnProgressChanged()
    //
    /// <summary>
    /// Handles the ProgressChanged event on the HttpNetworkAnalyzer.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected override void
    OnProgressChanged
    (
        ProgressChangedEventArgs e
    )
    {
        AssertValid();

        Debug.Assert(e.UserState is String);

        this.slStatusLabel.Text = (String)e.UserState;
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
        txbSearchTerm.Focus();
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

        Debug.Assert(m_sSearchTerm != null);
        // m_bIncludeFollowedEdges
        // m_bIncludeRepliesToEdges
        // m_bIncludeMentionsEdges
        Debug.Assert(m_iMaximumPeoplePerRequest > 0);
        // m_bIncludeStatuses
        // m_bIncludeStatistics
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // These are static so that the dialog's controls will retain their values
    // between dialog invocations.  Most NodeXL dialogs persist control values
    // via ApplicationSettingsBase, but this plugin does not have access to
    // that and so it resorts to static fields.

    /// The search term to use.

    protected static String m_sSearchTerm = "NodeXL";

    /// true to include an edge for each followed relationship.

    protected static Boolean m_bIncludeFollowedEdges = false;

    /// true to include an edge from person A to person B if person A's tweet
    /// is a reply to person B.

    protected static Boolean m_bIncludeRepliesToEdges = false;

    /// true to include an edge from person A to person B if person A's tweet
    /// is mentions person B.

    protected static Boolean m_bIncludeMentionsEdges = true;

    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.

    protected static Int32 m_iMaximumPeoplePerRequest = 100;

    /// true to include each status.

    protected static Boolean m_bIncludeStatuses;

    /// true to include statistics for each user.

    protected static Boolean m_bIncludeStatistics = false;
}
}
