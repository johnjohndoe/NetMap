

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;
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
/// DialogResult.OK, get the network from the <see
/// cref="GraphDataProviderDialogBase.Results" /> property.
/// </remarks>
//*****************************************************************************

public partial class TwitterGetUserNetworkDialog :
    TwitterGraphDataProviderDialogBase
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
    base ( new TwitterUserNetworkAnalyzer() )
    {
        InitializeComponent();
        InitializeTwitterAuthorizationManager(usrTwitterAuthorization);

        // m_sScreenNameToAnalyze
        // m_bIncludeFollowedVertices
        // m_bIncludeFollowerVertices
        // m_bIncludeFollowedFollowerEdges
        // m_bIncludeRepliesToEdges
        // m_bIncludeMentionsEdges
        // m_eNetworkLevel
        // m_bIncludeLatestStatuses
        // m_iMaximumPeoplePerRequest

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Property: ToolStripStatusLabel
    //
    /// <summary>
    /// Gets the dialog's ToolStripStatusLabel control.
    /// </summary>
    ///
    /// <value>
    /// The dialog's ToolStripStatusLabel control, or null if the dialog
    /// doesn't have one.  The default is null.
    /// </value>
    ///
    /// <remarks>
    /// If the derived dialog overrides this property and returns a non-null
    /// ToolStripStatusLabel control, the control's text will automatically get
    /// updated when the HttpNetworkAnalyzer fires a ProgressChanged event.
    /// </remarks>
    //*************************************************************************

    protected override ToolStripStatusLabel
    ToolStripStatusLabel
    {
        get
        {
            AssertValid();

            return (this.slStatusLabel);
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

    protected override Boolean
    DoDataExchange
    (
        Boolean bFromControls
    )
    {
        if (bFromControls)
        {
            // Validate the controls.

            if ( !ValidateRequiredTextBox(txbScreenNameToAnalyze,
                    "Enter the screen name of a Twitter user.",
                    out m_sScreenNameToAnalyze) )
            {
                return (false);
            }

            m_bIncludeFollowedVertices = m_bIncludeFollowerVertices = false;

            if (radIncludeFollowedVertices.Checked)
            {
                m_bIncludeFollowedVertices = true;
            }
            else if (radIncludeFollowerVertices.Checked)
            {
                m_bIncludeFollowerVertices = true;
            }
            else
            {
                m_bIncludeFollowedVertices = m_bIncludeFollowerVertices = true;
            }

            m_bIncludeFollowedFollowerEdges =
                chkIncludeFollowedFollowerEdges.Checked;

            m_bIncludeRepliesToEdges = chkIncludeRepliesToEdges.Checked;
            m_bIncludeMentionsEdges = chkIncludeMentionsEdges.Checked;
            m_eNetworkLevel = usrNetworkLevel.Level;
            m_bIncludeLatestStatuses = chkIncludeLatestStatuses.Checked;
            m_iMaximumPeoplePerRequest = usrLimitToN.N;
        }
        else
        {
            txbScreenNameToAnalyze.Text = m_sScreenNameToAnalyze;

            if (m_bIncludeFollowedVertices)
            {
                if (m_bIncludeFollowerVertices)
                {
                    radIncludeFollowedAndFollower.Checked = true;
                }
                else
                {
                    radIncludeFollowedVertices.Checked = true;
                }
            }
            else
            {
                radIncludeFollowerVertices.Checked = true;
            }

            chkIncludeFollowedFollowerEdges.Checked =
                m_bIncludeFollowedFollowerEdges;

            chkIncludeRepliesToEdges.Checked = m_bIncludeRepliesToEdges;
            chkIncludeMentionsEdges.Checked = m_bIncludeMentionsEdges;
            usrNetworkLevel.Level = m_eNetworkLevel;
            chkIncludeLatestStatuses.Checked = m_bIncludeLatestStatuses;
            usrLimitToN.N = m_iMaximumPeoplePerRequest;

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

        Debug.Assert(m_oHttpNetworkAnalyzer is TwitterUserNetworkAnalyzer);

        TwitterUserNetworkAnalyzer.WhatToInclude eWhatToInclude =

            (m_bIncludeFollowedVertices ?
                TwitterUserNetworkAnalyzer.WhatToInclude.FollowedVertices : 0)
            |
            (m_bIncludeFollowerVertices ?
                TwitterUserNetworkAnalyzer.WhatToInclude.FollowerVertices : 0)
            |
            (m_bIncludeLatestStatuses ?
                TwitterUserNetworkAnalyzer.WhatToInclude.LatestStatuses : 0)
            |
            (m_bIncludeFollowedFollowerEdges ?
                TwitterUserNetworkAnalyzer.WhatToInclude.FollowedFollowerEdges
                : 0)
            |
            (m_bIncludeRepliesToEdges ?
                TwitterUserNetworkAnalyzer.WhatToInclude.RepliesToEdges : 0)
            |
            (m_bIncludeMentionsEdges ?
                TwitterUserNetworkAnalyzer.WhatToInclude.MentionsEdges : 0)
            ;

        ( (TwitterUserNetworkAnalyzer)m_oHttpNetworkAnalyzer ).
            GetNetworkAsync(m_sScreenNameToAnalyze, eWhatToInclude,
                m_eNetworkLevel, m_iMaximumPeoplePerRequest);
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
        // m_bIncludeFollowedVertices
        // m_bIncludeFollowerVertices
        // m_bIncludeFollowedFollowerEdges
        // m_bIncludeRepliesToEdges
        // m_bIncludeMentionsEdges
        // m_eNetworkLevel
        // m_bIncludeLatestStatuses
        Debug.Assert(m_iMaximumPeoplePerRequest > 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // These are static so that the dialog's controls will retain their values
    // between dialog invocations.  Most NodeXL dialogs persist control values
    // via ApplicationSettingsBase, but this plugin does not have access to
    // that and so it resorts to static fields.

    /// The screen name of the Twitter user whose network should be analyzed.

    protected static String m_sScreenNameToAnalyze = "bob";

    /// true to include a vertex for each person followed by the user.

    protected static Boolean m_bIncludeFollowedVertices = true;

    /// true to include a vertex for each person following the user.

    protected static Boolean m_bIncludeFollowerVertices = false;

    /// true to include an edge for each followed/following relationship.

    protected static Boolean m_bIncludeFollowedFollowerEdges = true;

    /// true to include an edge from person A to person B if person A's latest
    /// tweet is a reply to person B.

    protected static Boolean m_bIncludeRepliesToEdges = false;

    /// true to include an edge from person A to person B if person A's latest
    /// tweet is mentions person B.

    protected static Boolean m_bIncludeMentionsEdges = false;

    /// Network level to include.

    protected static NetworkLevel m_eNetworkLevel = NetworkLevel.One;

    /// true to include each user's latest status.

    protected static Boolean m_bIncludeLatestStatuses;

    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.

    protected static Int32 m_iMaximumPeoplePerRequest = Int32.MaxValue;
}
}
