

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Net;
using System.Xml;
using System.IO;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.XmlLib;

namespace Microsoft.NodeXL.GraphDataProviders.YouTube
{
//*****************************************************************************
//  Class: YouTubeGraphDataProviderDialogBase
//
/// <summary>
/// Base class for dialogs that get YouTube graph data.
/// </summary>
//*****************************************************************************

public class YouTubeGraphDataProviderDialogBase : GraphDataProviderDialogBase
{
    //*************************************************************************
    //  Constructor: YouTubeGraphDataProviderDialogBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="YouTubeGraphDataProviderDialogBase" /> class.
    /// </summary>
    ///
    /// <param name="httpNetworkAnalyzer">
    /// Object that does most of the work.
    /// </param>
    //*************************************************************************

    public YouTubeGraphDataProviderDialogBase
    (
        HttpNetworkAnalyzerBase httpNetworkAnalyzer
    )
    : base (httpNetworkAnalyzer)
    {
        // (Do nothing else.)

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: YouTubeGraphDataProviderDialogBase()
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

    public YouTubeGraphDataProviderDialogBase()
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
            "The YouTube Web service didn't respond.";

        if (oException is WebException)
        {
            WebException oWebException = (WebException)oException;

            if (oWebException.Response is HttpWebResponse)
            {
                HttpWebResponse oHttpWebResponse =
                    (HttpWebResponse)oWebException.Response;

                switch (oHttpWebResponse.StatusCode)
                {
                    case HttpStatusCode.Forbidden:  // HTTP 403.

                        sMessage =
                            "The YouTube Web service refused to provide the"
                            + " information because it is private."
                            ;

                        break;

                    case HttpStatusCode.NotFound:  // HTTP 404.

                        // Note: When information is requested for a user who
                        // doesn't exist, YouTube returns a 404 error.  When
                        // searching for videos using a search term that
                        // doesn't exist, an XML document with zero <entry>
                        // tags is returned instead.  Therefore, it is okay for
                        // now to use a message here that refers to users.

                        sMessage =
                            "There is no YouTube user with that username."
                            ;

                        break;

                    case HttpStatusCode.RequestTimeout:  // HTTP 408.

                        sMessage = TimeoutMessage;
                        break;

                    default:

                        // The following method may return null, which is
                        // checked for below.

                        sMessage =
                            YouTubeErrorResponseToMessage(oHttpWebResponse);

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
    //  Method: YouTubeErrorResponseToMessage()
    //
    /// <summary>
    /// Converts an error response from YouTube to an error message appropriate
    /// for the UI.
    /// </summary>
    ///
    /// <param name="oHttpWebResponse">
    /// The error response from YouTube.
    /// </param>
    ///
    /// <returns>
    /// An error message appropriate for the UI, or null if an error message
    /// couldn't be obtained.
    /// </returns>
    //*************************************************************************

    protected String
    YouTubeErrorResponseToMessage
    (
        HttpWebResponse oHttpWebResponse 
    )
    {
        Debug.Assert(oHttpWebResponse != null);
        AssertValid();

        // YouTube provides error information as an XmlDocument.

        String sMessage = null;
        XmlDocument oXmlDocument = new XmlDocument();
        Stream oStream = null;

        try
        {
            oStream = oHttpWebResponse.GetResponseStream();
            oXmlDocument.Load(oStream);
        }
        catch (XmlException)
        {
            return (null);
        }
        finally
        {
            if (oStream != null)
            {
                oStream.Close();
            }
        }

        XmlNamespaceManager oXmlNamespaceManager =
            YouTubeUserNetworkAnalyzer.CreateXmlNamespaceManager(oXmlDocument);

        // Although the document may contain multiple error nodes, just look at
        // the first one for now.

        if ( XmlUtil2.SelectSingleNode(oXmlDocument,
            "gd:errors/gd:error/gd:code/text()", oXmlNamespaceManager, false,
            out sMessage) )
        {
            if (sMessage.IndexOf("too_many_recent_calls") >= 0)
            {
                sMessage =
                    "You have made too many YouTube network requests recently."
                    + "  Wait a few minutes and try again."
                    ;
            }
        }
        else
        {
            sMessage = null;
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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}
}
