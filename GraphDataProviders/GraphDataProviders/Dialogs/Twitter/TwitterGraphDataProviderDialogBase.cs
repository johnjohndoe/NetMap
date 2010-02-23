

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterGraphDataProviderDialogBase
//
/// <summary>
/// Base class for dialogs that get Twitter graph data.
/// </summary>
//*****************************************************************************

public class TwitterGraphDataProviderDialogBase : GraphDataProviderDialogBase
{
    //*************************************************************************
    //  Constructor: TwitterGraphDataProviderDialogBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterGraphDataProviderDialogBase" /> class.
    /// </summary>
    ///
    /// <param name="httpNetworkAnalyzer">
    /// Object that does most of the work.
    /// </param>
    //*************************************************************************

    public TwitterGraphDataProviderDialogBase
    (
        HttpNetworkAnalyzerBase httpNetworkAnalyzer
    )
    : base (httpNetworkAnalyzer)
    {
        // m_sCredentialsScreenName
        // m_sCredentialsPassword

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: TwitterGraphDataProviderDialogBase()
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

    public TwitterGraphDataProviderDialogBase()
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
            "The Twitter Web service didn't respond.";

        const String RefusedMessage =
            "The Twitter Web service refused to provide the requested"
            + " information."
            ;

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
                            RefusedMessage
                            + "  One possible cause is that the Twitter user"
                            + " has protected her tweets."
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
                    case (HttpStatusCode)420:

                        // See TwitterNetworkAnalyzerBase for an explanation of
                        // why there are two status codes for this case.

                        sMessage = String.Format(

                            RefusedMessage 
                            + "  A likely cause is that you have made too many"
                            + " requests in the last hour.  (Twitter limits"
                            + " information requests to prevent its service"
                            + " from being attacked.  Click the '{0}' link for"
                            + " details.)"
                            + "\r\n\r\n"
                            + "Wait 60 minutes and try again."
                            ,
                            TwitterCredentialsControl.RateLimitingLinkText
                            );

                        break;

                    case HttpStatusCode.Forbidden:  // HTTP 403.

                        sMessage = RefusedMessage;
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

        // m_sCredentialsScreenName
        // m_sCredentialsPassword
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // These are static so that the dialog's controls will retain their values
    // between dialog invocations.  Most NodeXL dialogs persist control values
    // via ApplicationSettingsBase, but this plugin does not have access to
    // that and so it resorts to static fields.

    /// The Twitter credentials to use.
    ///
    /// These are shared among the derived dialogs so that the user doesn't
    /// have to re-enter his credentials in each dialog.

    protected static String m_sCredentialsScreenName = String.Empty;
    ///
    protected static String m_sCredentialsPassword = String.Empty;
}
}
