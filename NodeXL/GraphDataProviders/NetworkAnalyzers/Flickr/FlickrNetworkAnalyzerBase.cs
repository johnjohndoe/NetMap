
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Xml;
using System.Web;
using System.Net;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.XmlLib;

namespace Microsoft.NodeXL.GraphDataProviders.Flickr
{
//*****************************************************************************
//  Class: FlickrNetworkAnalyzerBase
//
/// <summary>
/// Base class for classes that analyze a Flickr network.
/// </summary>
//*****************************************************************************

public abstract class FlickrNetworkAnalyzerBase : HttpNetworkAnalyzerBase
{
    //*************************************************************************
    //  Constructor: FlickrNetworkAnalyzerBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="FlickrNetworkAnalyzerBase" /> class.
    /// </summary>
    //*************************************************************************

    public FlickrNetworkAnalyzerBase()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: ExceptionToMessage()
    //
    /// <summary>
    /// Converts an exception to an error message appropriate for a user
    /// interface.
    /// </summary>
    ///
    /// <param name="oException">
    /// The exception that occurred.
    /// </param>
    ///
    /// <returns>
    /// An error message appropriate for a user interface.
    /// </returns>
    //*************************************************************************

    public override String
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
    //  Method: GetFlickrMethodUrl()
    //
    /// <summary>
    /// Constructs an URL for a Flickr API method.
    /// </summary>
    ///
    /// <param name="sFlickrMethodName">
    /// Flickr method name.  Sample: "flickr.test.echo".
    /// </param>
    ///
    /// <param name="sApiKey">
    /// Flickr API key.
    /// </param>
    ///
    /// <param name="sAdditionalParameters">
    /// Optional additional URL parameters, or null for none.  If not null,
    /// must start with an ampersand and must already be URL-encoded.
    /// </param>
    ///
    /// <returns>
    /// An URL for the specified Flickr method.
    /// URL parameters.
    /// </returns>
    //*************************************************************************

    protected String
    GetFlickrMethodUrl
    (
        String sFlickrMethodName,
        String sApiKey,
        String sAdditionalParameters
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sFlickrMethodName) );
        Debug.Assert( !String.IsNullOrEmpty(sApiKey) );

        Debug.Assert( sAdditionalParameters == null ||
            sAdditionalParameters.StartsWith("&") );

        AssertValid();

        return ( String.Format(

            "http://api.flickr.com/services/rest/?method={0}&api_key={1}{2}"
            ,
            sFlickrMethodName,
            HttpUtility.UrlEncode(sApiKey),

            (sAdditionalParameters == null) ? String.Empty
                : sAdditionalParameters
            ) );
    }

    //*************************************************************************
    //  Method: GetUserIDUrlParameter()
    //
    /// <summary>
    /// Gets an URL parameter for a Flickr user ID.
    /// </summary>
    ///
    /// <param name="sUserID">
    /// Flickr user ID.
    /// </param>
    ///
    /// <returns>
    /// URL parameter, including a leading ampersand.
    /// </returns>
    //*************************************************************************

    protected String
    GetUserIDUrlParameter
    (
        String sUserID
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUserID) );
        AssertValid();

        return ( "&user_id=" + UrlUtil.EncodeUrlParameter(sUserID) );
    }

    //*************************************************************************
    //  Method: TryGetXmlDocument()
    //
    /// <summary>
    /// Attempts to get an XML document from an URL.
    /// </summary>
    ///
    /// <param name="sUrl">
    /// The URL to get the document from.  Can include URL parameters.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <param name="oXmlDocument">
    /// Where the XmlDocument gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the XmlDocument was obtained, false if an error occurred.
    /// </returns>
    ///
    /// <remarks>
    /// If an error occurs, false is returned.  A WebException or XmlException
    /// is never rethrown.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    TryGetXmlDocument
    (
        String sUrl,
        RequestStatistics oRequestStatistics,
        out XmlDocument oXmlDocument
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUrl) );
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        oXmlDocument = null;

        try
        {
            oXmlDocument = GetXmlDocument(sUrl, oRequestStatistics);

            return (true);
        }
        catch (Exception oException)
        {
            // Note: Because a FlickrException is also a WebException, the
            // following logic will cause false to be returned for a
            // FlickrException.

            if ( !ExceptionIsWebOrXml(oException) )
            {
                throw oException;
            }

            return (false);
        }
    }

    //*************************************************************************
    //  Method: GetXmlDocument()
    //
    /// <summary>
    /// Gets an XML document from an URL.
    /// </summary>
    ///
    /// <param name="sUrl">
    /// The URL to get the document from.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <returns>
    /// The XmlDocument from the URL.
    /// </returns>
    ///
    /// <remarks>
    /// If an error occurs, whether it's a WebException or an error document
    /// returned by Flickr, an exception is thrown.
    /// </remarks>
    //*************************************************************************

    protected XmlDocument
    GetXmlDocument
    (
        String sUrl,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUrl) );
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        XmlDocument oXmlDocument = GetXmlDocumentWithRetries(sUrl,
            HttpStatusCodesToFailImmediately, oRequestStatistics);

        String sStatus;

        if (
            XmlUtil2.TrySelectSingleNodeAsString(oXmlDocument, "rsp/@stat",
                null, out sStatus)
            &&
            sStatus == "ok"
            )
        {
            return (oXmlDocument);
        }

        // Flickr indicates errors by returning an XML document containing an
        // rsp/err node.  The following code turns such an error document into
        // a custom FlickrException.

        String sErrorMessage;

        if ( XmlUtil2.TrySelectSingleNodeAsString(oXmlDocument, "rsp/err/@msg",
            null, out sErrorMessage) )
        {
            if (sErrorMessage.ToLower().IndexOf("user not found") >= 0)
            {
                sErrorMessage =
                    "The user wasn't found.  Either there is no such user, or"
                    + " she has hidden herself from public searches."
                    ;
            }
        }
        else
        {
            sErrorMessage =
                "Flickr provided information in an unrecognized format.";
        }

        throw new FlickrException(sErrorMessage);
    }

    //*************************************************************************
    //  Method: EnumerateXmlNodes()
    //
    /// <summary>
    /// Get an XmlDocument from an URL, then enumerates a specified set of
    /// child nodes.
    /// </summary>
    ///
    /// <param name="sUrl">
    /// The URL to get the document from.  Can include URL parameters.
    /// </param>
    ///
    /// <param name="sXPath">
    /// The XPath of the child nodes to enumerate.  Sample:
    /// "feed/entry".
    /// </param>
    ///
    /// <param name="iMaximumXmlNodes">
    /// Maximum number of child nodes to return, or Int32.MaxValue for no
    /// limit.
    /// </param>
    ///
    /// <param name="bSkipMostPage1Errors">
    /// If true, most page-1 errors are skipped over.  If false, they are
    /// rethrown.  (Errors that occur on page 2 and above are always skipped.)
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <remarks>
    /// This should be used only for Flickr methods that use paged results.
    /// </remarks>
    //*************************************************************************

    protected IEnumerable<XmlNode>
    EnumerateXmlNodes
    (
        String sUrl,
        String sXPath,
        Int32 iMaximumXmlNodes,
        Boolean bSkipMostPage1Errors,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUrl) );
        Debug.Assert( !String.IsNullOrEmpty(sXPath) );
        Debug.Assert(iMaximumXmlNodes > 0);
        Debug.Assert(oRequestStatistics != null);

        AssertValid();

        // A maximum per-page value of 500 works for
        // flickr.contacts.getPublicList (maximum per_page = 1000) and
        // flickr.people.getPublicPhotos (maximum per_page = 500).

        Debug.Assert(sUrl.IndexOf("flickr.contacts.getPublicList") >= 0 ||
            sUrl.IndexOf("flickr.people.getPublicPhotos") >= 0);

        Int32 iMaximumPerPage = Math.Min(500, iMaximumXmlNodes);

        Int32 iPage = 1;
        Int32 iXmlNodesEnumerated = 0;

        while (true)
        {
            String sUrlWithPagination = String.Format(
            
                "{0}{1}per_page={2}&page={3}"
                ,
                sUrl,
                sUrl.IndexOf('?') == -1 ? '?' : '&',
                iMaximumPerPage,
                iPage
                );

            XmlDocument oXmlDocument;

            try
            {
                oXmlDocument = GetXmlDocument(sUrlWithPagination,
                    oRequestStatistics);
            }
            catch (Exception oException)
            {
                if ( !ExceptionIsWebOrXml(oException) )
                {
                    throw oException;
                }

                if (iPage > 1 || bSkipMostPage1Errors)
                {
                    // Always skip errors on page 2 and above.

                    yield break;
                }

                throw (oException);
            }

            XmlNodeList oXmlNodesThisPage = oXmlDocument.SelectNodes(sXPath,
                null);

            Int32 iXmlNodesThisPage = oXmlNodesThisPage.Count;

            if (iXmlNodesThisPage == 0)
            {
                yield break;
            }

            for (Int32 i = 0; i < iXmlNodesThisPage; i++)
            {
                yield return ( oXmlNodesThisPage[i] );

                iXmlNodesEnumerated++;

                if (iXmlNodesEnumerated == iMaximumXmlNodes)
                {
                    yield break;
                }
            }

            iPage++;

            // Get the next page...
        }
    }

    //*************************************************************************
    //  Method: FlickrScreenNameToUserID()
    //
    /// <summary>
    /// Gets the user ID for a Flickr screen name.
    /// </summary>
    ///
    /// <param name="sScreenNameAnyCase">
    /// Flickr screen name, with any combination of upper- and lower-case
    /// characters.  Sample: "barbsmith".
    /// </param>
    ///
    /// <param name="sApiKey">
    /// Flickr API key.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <param name="sUserID">
    /// Where the user ID for the specified screen name gets stored.
    /// </param>
    ///
    /// <param name="sScreenNameCorrectCase">
    /// Where the screen name gets stored, with the correct combination of
    /// upper- and lower-case characters.  Sample: "BarbSmith".
    /// </param>
    ///
    /// <remarks>
    /// If there is no such user, a FlickrException is thrown.
    /// </remarks>
    //*************************************************************************

    protected void
    FlickrScreenNameToUserID
    (
        String sScreenNameAnyCase,
        String sApiKey,
        RequestStatistics oRequestStatistics,
        out String sUserID,
        out String sScreenNameCorrectCase
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenNameAnyCase) );
        Debug.Assert( !String.IsNullOrEmpty(sApiKey) );
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        XmlDocument oXmlDocument = GetXmlDocument(

            GetFlickrMethodUrl("flickr.people.findByUsername", sApiKey,
                "&username=" + UrlUtil.EncodeUrlParameter(sScreenNameAnyCase) ),

            oRequestStatistics
            );

        sUserID = XmlUtil2.SelectRequiredSingleNodeAsString(
            oXmlDocument, "rsp/user/@nsid", null);

        sScreenNameCorrectCase = XmlUtil2.SelectRequiredSingleNodeAsString(
            oXmlDocument, "rsp/user/username/text()", null);
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
    //  Protected constants
    //*************************************************************************

    /// HTTP status codes that have special meaning with Flickr.  When they
    /// occur, the requests are not retried.

    protected static readonly HttpStatusCode []
        HttpStatusCodesToFailImmediately = new HttpStatusCode[] {

            // (There are no such special status codes for Flickr, which
            // handles errors by returning an XML document containing error
            // information.  This contrasts with some other Web services, which
            // return HTTP status codes such as HttpStatusCode.NotFound or
            // Forbidden when an error occurs.
        };


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)


    //*************************************************************************
    //  Embedded class: GetNetworkAsyncArgsBase()
    //
    /// <summary>
    /// Base class for classes that contain the arguments needed to
    /// asynchronously get a Flickr network.
    /// </summary>
    //*************************************************************************

    protected class GetNetworkAsyncArgsBase
    {
        ///
        public String ApiKey;
    };
}

}
