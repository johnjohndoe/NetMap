

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.SocialNetworkLib;

namespace Microsoft.NodeXL.GraphDataProviders.Flickr
{
//*****************************************************************************
//  Class: FlickrGetRelatedTagNetworkDialog
//
/// <summary>
/// Gets the Flickr tags related to a tag specified by the user.
/// </summary>
///
/// <remarks>
/// Call <see cref="Form.ShowDialog()" /> to show the dialog.  If it returns
/// DialogResult.OK, get the related tags from the <see
/// cref="GraphDataProviderDialogBase.Results" /> property.
/// </remarks>
//*****************************************************************************

public partial class FlickrGetRelatedTagNetworkDialog :
    FlickrGraphDataProviderDialogBase
{
    //*************************************************************************
    //  Constructor: FlickrGetRelatedTagNetworkDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="FlickrGetRelatedTagNetworkDialog" /> class.
    /// </summary>
    //*************************************************************************

    public FlickrGetRelatedTagNetworkDialog()
    :
    base ( new FlickrRelatedTagNetworkAnalyzer() )
    {
        InitializeComponent();

        // m_sTag
        // m_eNetworkLevel
        // m_bIncludeSampleThumbnails

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

            String sTag;

            if (
                !ValidateRequiredTextBox(txbTag,
                    "Enter a Flickr tag.",
                    out sTag)
                ||
                !usrFlickrApiKey.Validate()
                )
            {
                return (false);
            }

            m_sTag = sTag;
            m_eNetworkLevel = usrNetworkLevel.Level;
            m_bIncludeSampleThumbnails = chkIncludeSampleThumbnails.Checked;
            m_sApiKey = usrFlickrApiKey.ApiKey;
        }
        else
        {
            txbTag.Text = m_sTag;
            usrNetworkLevel.Level = m_eNetworkLevel;
            chkIncludeSampleThumbnails.Checked = m_bIncludeSampleThumbnails;
            usrFlickrApiKey.ApiKey = m_sApiKey;

            EnableControls();
        }

        return (true);
    }

    //*************************************************************************
    //  Method: StartAnalysis()
    //
    /// <summary>
    /// Starts the Flickr analysis.
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

        Debug.Assert(m_oHttpNetworkAnalyzer is
            FlickrRelatedTagNetworkAnalyzer);

        FlickrRelatedTagNetworkAnalyzer.WhatToInclude eWhatToInclude =

            (m_bIncludeSampleThumbnails ?
                FlickrRelatedTagNetworkAnalyzer.WhatToInclude.SampleThumbnails
                : 0)
            ;

        ( (FlickrRelatedTagNetworkAnalyzer)m_oHttpNetworkAnalyzer ).
            GetNetworkAsync(m_sTag, eWhatToInclude, m_eNetworkLevel,
                m_sApiKey);
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

        this.ShowInformation("That tag has no related tags.");
        txbTag.Focus();
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

        Debug.Assert(m_sTag != null);
        // m_eNetworkLevel
        // m_bIncludeSampleThumbnails
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Flickr Web page for requesting an API key.

    protected const String RequestFlickrApiKeyUrl =
        "http://www.flickr.com/services/api/misc.api_keys.html";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // These are static so that the dialog's controls will retain their values
    // between dialog invocations.  Most NodeXL dialogs persist control values
    // via ApplicationSettingsBase, but this plugin does not have access to
    // that and so it resorts to static fields.

    /// Tag to get the related tags for.  Can be empty but not null.

    protected static String m_sTag = "sociology";

    /// Network level to include.

    protected static NetworkLevel m_eNetworkLevel = NetworkLevel.OnePointFive;

    /// true to include a sample thumbnail for each tag.

    protected static Boolean m_bIncludeSampleThumbnails = false;
}
}
