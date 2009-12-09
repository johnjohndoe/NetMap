

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Net;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.GraphDataProviders.Flickr
{
//*****************************************************************************
//  Class: FlickrGraphDataProviderDialogBase
//
/// <summary>
/// Base class for dialogs that get Flickr graph data.
/// </summary>
//*****************************************************************************

public partial class FlickrGraphDataProviderDialogBase :
    GraphDataProviderDialogBase
{
    //*************************************************************************
    //  Constructor: FlickrGraphDataProviderDialogBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="FlickrGraphDataProviderDialogBase" /> class.
    /// </summary>
    //*************************************************************************

    public FlickrGraphDataProviderDialogBase
    (
        HttpNetworkAnalyzerBase httpNetworkAnalyzer
    )
    : base (httpNetworkAnalyzer)
    {
        // m_sApiKey

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: FlickrGraphDataProviderDialogBase()
    //
    /// <summary>
    /// Do not use this constructor.
    /// </summary>
    ///
    /// <remarks>
    /// Do not use this constructor.  It is for the Visual Studio designer
    /// only.
    /// </remarks>
    //*************************************************************************

    public FlickrGraphDataProviderDialogBase()
    {
        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: ExceptionToMessage()
    //
    /// <summary>
    /// Converts an exception to an error message appropriate for the UI.
    /// </summary>
    ///
    /// <param name="oException">
    /// The exception that occurred.
    /// </param>
    ///
    /// <returns>
    /// An error message appropriate for the UI.
    /// </returns>
    //*************************************************************************

    protected override String
    ExceptionToMessage
    (
        Exception oException
    )
    {
        Debug.Assert(oException != null);
        AssertValid();

        String sMessage = null;

        const String TimeoutMessage =
            "The Flickr Web service didn't respond.";

        if (oException is FlickrException)
        {
            sMessage = oException.Message;
        }
        else if (oException is WebException)
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
            sMessage = ExceptionUtil.GetMessageTrace(oException);
        }

        return (sMessage);
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

        Debug.Assert(m_sApiKey != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // This is static so that the dialog's controls will retain their values
    // between dialog invocations.  Most NodeXL dialogs persist control values
    // via ApplicationSettingsBase, but this plugin does not have access to
    // that and so it resorts to static fields.

    /// Flickr API key.  Can be empty but not null.

    protected static String m_sApiKey = String.Empty;
}
}
