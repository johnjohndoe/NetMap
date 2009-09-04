

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.SocialNetworkLib;

namespace Microsoft.NodeXL.GraphDataProviders.Flickr
{
//*****************************************************************************
//  Class: FlickrGetRelatedTagsDialog
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

public partial class FlickrGetRelatedTagsDialog : GraphDataProviderDialogBase
{
    //*************************************************************************
    //  Constructor: FlickrGetRelatedTagsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="FlickrGetRelatedTagsDialog" /> class.
    /// </summary>
    //*************************************************************************

    public FlickrGetRelatedTagsDialog()
    :
    base ( new FlickrNetworkAnalyzer() )
    {
        InitializeComponent();

        // m_sFlickrApiKey
        // m_sTag

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

            String sFlickrApiKey, sTag;

            if (
                !ValidateRequiredTextBox(txbFlickrApiKey,
                    "Enter a Flickr API key.",
                    out sFlickrApiKey)
                ||
                !ValidateRequiredTextBox(txbTag,
                    "Enter a Flickr tag to get related tags for.",
                    out sTag)
                )
            {
                return (false);
            }

            m_sFlickrApiKey = sFlickrApiKey;
            m_sTag = sTag;
        }
        else
        {
            txbFlickrApiKey.Text = m_sFlickrApiKey;
            txbTag.Text = m_sTag;

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

        Debug.Assert(m_oHttpNetworkAnalyzer is FlickrNetworkAnalyzer);

        ( (FlickrNetworkAnalyzer)m_oHttpNetworkAnalyzer ).GetRelatedTagsAsync(
            m_sTag, m_sFlickrApiKey);
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
    //  Method: OnAnalysisException()
    //
    /// <summary>
    /// Handles the AnalysisCompleted event on the HttpNetworkAnalyzer object
    /// when an exception occurs.
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
            "The Flickr Web service didn't respond.";

        if (oException is WebException)
        {
            WebException oWebException = (WebException)oException;

            if (oWebException.Response is HttpWebResponse)
            {
                HttpWebResponse oHttpWebResponse =
                    (HttpWebResponse)oWebException.Response;

                switch (oHttpWebResponse.StatusCode)
                {
                    case HttpStatusCode.RequestTimeout:  // HTTP 408.

                        sMessage = TimeoutMessage;
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
                "The network of related tags couldn't be obtained."
                + "\r\n\r\nDetails:\r\n\r\n"
                + ExceptionUtil.GetMessageTrace(oException)
                ;
        }

        this.ShowWarning(sMessage);
    }

    //*************************************************************************
    //  Method: lnkRequestFlickrApiKey_LinkClicked()
    //
    /// <summary>
    /// Handles the LinkClicked event on the lnkRequestFlickrApiKey LinkLabel.
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
    lnkRequestFlickrApiKey_LinkClicked
    (
        object sender, LinkLabelLinkClickedEventArgs e
    )
    {
        AssertValid();

        Process.Start(RequestFlickrApiKeyUrl);
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

        Debug.Assert(m_sFlickrApiKey != null);
        Debug.Assert(m_sTag != null);
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

    /// Flickr API key.  Can be empty but not null.
    ///
    /// This is static so that the TextBox that displays it will retain its
    /// text between dialog invocations.  Most NodeXL dialogs persist control
    /// values via ApplicationSettingsBase, but this plugin does not have
    /// access to that and so it resorts to static fields.

    protected static String m_sFlickrApiKey = String.Empty;

    /// Tag to get the related tags for.  Can be empty but not null.

    protected static String m_sTag = "sociology";
}
}
