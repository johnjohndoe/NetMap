

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.SocialNetworkLib;

namespace Microsoft.NodeXL.GraphDataProviders.Flickr
{
//*****************************************************************************
//  Class: FlickrGetUserNetworkDialog
//
/// <summary>
/// Gets a network of Flickr users.
/// </summary>
///
/// <remarks>
/// Call <see cref="Form.ShowDialog()" /> to show the dialog.  If it returns
/// DialogResult.OK, get the network from the <see
/// cref="GraphDataProviderDialogBase.Results" /> property.
/// </remarks>
//*****************************************************************************

public partial class FlickrGetUserNetworkDialog :
    FlickrGraphDataProviderDialogBase
{
    //*************************************************************************
    //  Constructor: FlickrGetUserNetworkDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="FlickrGetUserNetworkDialog" /> class.
    /// </summary>
    //*************************************************************************

    public FlickrGetUserNetworkDialog()
    :
    base ( new FlickrUserNetworkAnalyzer() )
    {
        InitializeComponent();

        // m_sScreenName
        // m_bIncludeContactVertices
        // m_bIncludeCommenterVertices
        // m_bIncludeUserInformation
        // m_eNetworkLevel
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

            if (
                !ValidateRequiredTextBox(txbScreenName,
                "Enter the screen name of a Flickr user.", out m_sScreenName)
                ||
                !usrFlickrApiKey.Validate()
                )
            {
                return (false);
            }

            m_bIncludeContactVertices = m_bIncludeCommenterVertices = false;

            if (radIncludeContactVertices.Checked)
            {
                m_bIncludeContactVertices = true;
            }
            else if (radIncludeCommenterVertices.Checked)
            {
                m_bIncludeCommenterVertices = true;
            }
            else
            {
                m_bIncludeContactVertices = m_bIncludeCommenterVertices =
                    true;
            }

            m_bIncludeUserInformation = chkIncludeUserInformation.Checked;
            m_eNetworkLevel = usrNetworkLevel.Level;
            m_iMaximumPeoplePerRequest = usrLimitToN.N;
            m_sApiKey = usrFlickrApiKey.ApiKey;
        }
        else
        {
            txbScreenName.Text = m_sScreenName;

            if (m_bIncludeContactVertices)
            {
                if (m_bIncludeCommenterVertices)
                {
                    radIncludeContactsAndCommenters.Checked = true;
                }
                else
                {
                    radIncludeContactVertices.Checked = true;
                }
            }
            else
            {
                radIncludeCommenterVertices.Checked = true;
            }

            chkIncludeUserInformation.Checked = m_bIncludeUserInformation;
            usrNetworkLevel.Level = m_eNetworkLevel;
            usrLimitToN.N = m_iMaximumPeoplePerRequest;
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

        Debug.Assert(m_oHttpNetworkAnalyzer is FlickrUserNetworkAnalyzer);

        FlickrUserNetworkAnalyzer.WhatToInclude eWhatToInclude =

            (m_bIncludeContactVertices ?
                FlickrUserNetworkAnalyzer.WhatToInclude.ContactVertices : 0)
            |
            (m_bIncludeCommenterVertices ?
                FlickrUserNetworkAnalyzer.WhatToInclude.CommenterVertices : 0)
            |
            (m_bIncludeUserInformation ?
                FlickrUserNetworkAnalyzer.WhatToInclude.UserInformation : 0)
            ;

        ( (FlickrUserNetworkAnalyzer)m_oHttpNetworkAnalyzer ).
            GetNetworkAsync(m_sScreenName, eWhatToInclude, m_eNetworkLevel,
                m_iMaximumPeoplePerRequest, m_sApiKey);
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
        txbScreenName.Focus();
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

        Debug.Assert(m_sScreenName != null);
        // m_bIncludeContactVertices
        // m_bIncludeCommenterVertices
        // m_bIncludeUserInformation
        // m_eNetworkLevel
        Debug.Assert(m_iMaximumPeoplePerRequest > 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // These are static so that the dialog's controls will retain their values
    // between dialog invocations.  Most NodeXL dialogs persist control values
    // via ApplicationSettingsBase, but this plugin does not have access to
    // that and so it resorts to static fields.

    /// The user name of the Flickr user whose network should be analyzed.

    protected static String m_sScreenName = "Mark";

    /// true to include a vertex for each of the user's contacts.

    protected static Boolean m_bIncludeContactVertices = true;

    /// true to include a vertex for each user who has commented on the user's
    /// photos.

    protected static Boolean m_bIncludeCommenterVertices = false;

    /// true to include information about each user.

    protected static Boolean m_bIncludeUserInformation = false;

    /// Network level to include.

    protected static NetworkLevel m_eNetworkLevel = NetworkLevel.One;

    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.

    protected static Int32 m_iMaximumPeoplePerRequest = Int32.MaxValue;
}
}
