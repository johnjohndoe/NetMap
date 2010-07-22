

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;

namespace Microsoft.NodeXL.GraphDataProviders.YouTube
{
//*****************************************************************************
//  Class: YouTubeGetVideoNetworkDialog
//
/// <summary>
/// Gets a network of related YouTube videos.
/// </summary>
///
/// <remarks>
/// Call <see cref="Form.ShowDialog()" /> to show the dialog.  If it returns
/// DialogResult.OK, get the network from the <see
/// cref="GraphDataProviderDialogBase.Results" /> property.
/// </remarks>
//*****************************************************************************

public partial class YouTubeGetVideoNetworkDialog :
    YouTubeGraphDataProviderDialogBase
{
    //*************************************************************************
    //  Constructor: YouTubeGetVideoNetworkDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="YouTubeGetVideoNetworkDialog" /> class.
    /// </summary>
    //*************************************************************************

    public YouTubeGetVideoNetworkDialog()
    :
    base( new YouTubeVideoNetworkAnalyzer() )
    {
        InitializeComponent();

        // m_sSearchTerm
        // m_bIncludeSharedCommenterEdges
        // m_bIncludeSharedTagEdges
        // m_bIncludeSharedVideoResponderEdges
        // m_iMaximumVideos

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

            if (
                !ValidateRequiredTextBox(txbSearchTerm,
                    "Enter one or more words to search for.",
                    out m_sSearchTerm)
                )
            {
                return (false);
            }

            m_bIncludeSharedCommenterEdges =
                chkIncludeSharedCommenterEdges.Checked;

            m_bIncludeSharedTagEdges = chkIncludeSharedTagEdges.Checked;

            m_bIncludeSharedVideoResponderEdges =
                chkIncludeSharedVideoResponderEdges.Checked;

            m_iMaximumVideos = usrLimitToN.N;
        }
        else
        {
            txbSearchTerm.Text = m_sSearchTerm;

            chkIncludeSharedCommenterEdges.Checked =
                m_bIncludeSharedCommenterEdges;

            chkIncludeSharedTagEdges.Checked = m_bIncludeSharedTagEdges;

            chkIncludeSharedVideoResponderEdges.Checked =
                m_bIncludeSharedVideoResponderEdges;

            usrLimitToN.N = m_iMaximumVideos;

            EnableControls();
        }

        return (true);
    }

    //*************************************************************************
    //  Method: StartAnalysis()
    //
    /// <summary>
    /// Starts the analysis.
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

        Debug.Assert(m_oHttpNetworkAnalyzer is YouTubeVideoNetworkAnalyzer);

        YouTubeVideoNetworkAnalyzer.WhatToInclude eWhatToInclude =

            (m_bIncludeSharedCommenterEdges ?
                YouTubeVideoNetworkAnalyzer.WhatToInclude.SharedCommenterEdges
                : 0)
            |
            (m_bIncludeSharedTagEdges ?
                YouTubeVideoNetworkAnalyzer.WhatToInclude.SharedTagEdges : 0)
            |
            (m_bIncludeSharedVideoResponderEdges ?
                YouTubeVideoNetworkAnalyzer.WhatToInclude.
                SharedVideoResponderEdges : 0)
            ;

        ( (YouTubeVideoNetworkAnalyzer)m_oHttpNetworkAnalyzer ).
            GetNetworkAsync(m_sSearchTerm, eWhatToInclude,
                m_iMaximumVideos);
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

        this.ShowInformation("There are no videos in that network.");
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
        // m_bIncludeSharedCommenterEdges
        // m_bIncludeSharedTagEdges
        // m_bIncludeSharedVideoResponderEdges
        Debug.Assert(m_iMaximumVideos > 0);
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

    /// true to include an edge for each pair of videos commented on by the
    /// same person.

    protected static Boolean m_bIncludeSharedCommenterEdges = false;

    /// true to include an edge for each pair of videos that share the same
    /// tag.

    protected static Boolean m_bIncludeSharedTagEdges = true;

    /// true to include an edge for each pair of videos for which the same
    /// person responded with a video.

    protected static Boolean m_bIncludeSharedVideoResponderEdges = false;

    /// Maximum number of videos to request, or Int32.MaxValue for no limit.

    protected static Int32 m_iMaximumVideos = 100;
}
}
