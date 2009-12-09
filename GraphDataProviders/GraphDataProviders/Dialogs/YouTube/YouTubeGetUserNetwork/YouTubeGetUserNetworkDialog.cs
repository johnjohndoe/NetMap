

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Microsoft.SocialNetworkLib;

namespace Microsoft.NodeXL.GraphDataProviders.YouTube
{
//*****************************************************************************
//  Class: YouTubeGetUserNetworkDialog
//
/// <summary>
/// Gets a network of YouTube users.
/// </summary>
///
/// <remarks>
/// Call <see cref="Form.ShowDialog()" /> to show the dialog.  If it returns
/// DialogResult.OK, get the network from the <see
/// cref="GraphDataProviderDialogBase.Results" /> property.
/// </remarks>
//*****************************************************************************

public partial class YouTubeGetUserNetworkDialog :
    YouTubeGraphDataProviderDialogBase
{
    //*************************************************************************
    //  Constructor: YouTubeGetUserNetworkDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="YouTubeGetUserNetworkDialog" /> class.
    /// </summary>
    //*************************************************************************

    public YouTubeGetUserNetworkDialog()
    :
    base ( new YouTubeUserNetworkAnalyzer() )
    {
        InitializeComponent();

        // m_sUserName
        // m_bIncludeFriendVertices
        // m_bIncludeSubscriptionVertices
        // m_eNetworkLevel
        // m_bIncludeAllStatistics
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

            if ( !ValidateUserNameTextBox(txbUserName, out m_sUserName) )
            {
                return (false);
            }

            m_bIncludeFriendVertices = m_bIncludeSubscriptionVertices = false;

            if (radIncludeFriendVertices.Checked)
            {
                m_bIncludeFriendVertices = true;
            }
            else if (radIncludeSubscriptionVertices.Checked)
            {
                m_bIncludeSubscriptionVertices = true;
            }
            else
            {
                m_bIncludeFriendVertices = m_bIncludeSubscriptionVertices =
                    true;
            }

            m_eNetworkLevel = usrNetworkLevel.Level;
            m_bIncludeAllStatistics = chkIncludeAllStatistics.Checked;
            m_iMaximumPeoplePerRequest = usrLimitToN.N;
        }
        else
        {
            txbUserName.Text = m_sUserName;

            if (m_bIncludeFriendVertices)
            {
                if (m_bIncludeSubscriptionVertices)
                {
                    radIncludeFollowedAndFollower.Checked = true;
                }
                else
                {
                    radIncludeFriendVertices.Checked = true;
                }
            }
            else
            {
                radIncludeSubscriptionVertices.Checked = true;
            }

            usrNetworkLevel.Level = m_eNetworkLevel;
            chkIncludeAllStatistics.Checked = m_bIncludeAllStatistics;
            usrLimitToN.N = m_iMaximumPeoplePerRequest;

            EnableControls();
        }

        return (true);
    }

    //*************************************************************************
    //  Method: ValidateUserNameTextBox()
    //
    /// <summary>
    /// Validates a TextBox that must contain a valid YouTube user name.
    /// </summary>
    ///
    /// <param name="oTextBox">
    /// TextBox to validate.
    /// </param>
    ///
    /// <param name="sUserName">
    /// Where the trimmed user name gets stored.
    /// </param>
    ///
    /// <returns>
    /// true if validation passes.
    /// </returns>
    //*************************************************************************

    protected Boolean
    ValidateUserNameTextBox
    (
        TextBox oTextBox,
        out String sUserName
    )
    {
        Debug.Assert(oTextBox != null);
        AssertValid();

        if ( !ValidateRequiredTextBox(oTextBox,
            "Enter the username of a YouTube user.", out sUserName) )
        {
            return (false);
        }

        if ( Regex.IsMatch(sUserName, "[^a-zA-Z0-9]") )
        {
            return ( OnInvalidTextBox(oTextBox,

                "YouTube usernames can contain only the letters A to Z and the"
                + " numbers 0 to 9.") );
        }

        return (true);
    }

    //*************************************************************************
    //  Method: StartAnalysis()
    //
    /// <summary>
    /// Starts the YouTube analysis.
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

        Debug.Assert(m_oHttpNetworkAnalyzer is YouTubeUserNetworkAnalyzer);

        YouTubeUserNetworkAnalyzer.WhatToInclude eWhatToInclude =

            (m_bIncludeFriendVertices ?
                YouTubeUserNetworkAnalyzer.WhatToInclude.FriendVertices : 0)
            |
            (m_bIncludeSubscriptionVertices ?
                YouTubeUserNetworkAnalyzer.WhatToInclude.SubscriptionVertices
                    : 0)
            |
            (m_bIncludeAllStatistics ?
                YouTubeUserNetworkAnalyzer.WhatToInclude.AllStatistics : 0)
            ;

        ( (YouTubeUserNetworkAnalyzer)m_oHttpNetworkAnalyzer ).
            GetNetworkAsync(m_sUserName, eWhatToInclude, m_eNetworkLevel,
                m_iMaximumPeoplePerRequest);
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
        txbUserName.Focus();
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

        Debug.Assert(m_sUserName != null);
        // m_bIncludeFriendVertices
        // m_bIncludeSubscriptionVertices
        // m_eNetworkLevel
        // m_bIncludeAllStatistics
        Debug.Assert(m_iMaximumPeoplePerRequest > 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // These are static so that the dialog's controls will retain their values
    // between dialog invocations.  Most NodeXL dialogs persist control values
    // via ApplicationSettingsBase, but this plugin does not have access to
    // that and so it resorts to static fields.

    /// The user name of the YouTube user whose network should be analyzed.

    protected static String m_sUserName = "andyland74";

    /// true to include a vertex for each of the user's friends.

    protected static Boolean m_bIncludeFriendVertices = true;

    /// true to include a vertex for each person subscribed to by the user.

    protected static Boolean m_bIncludeSubscriptionVertices = false;

    /// Network level to include.

    protected static NetworkLevel m_eNetworkLevel = NetworkLevel.One;

    /// true to include all statistics for each user.

    protected static Boolean m_bIncludeAllStatistics;

    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.

    protected static Int32 m_iMaximumPeoplePerRequest = Int32.MaxValue;
}
}
