
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Xml;
using System.Net;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.XmlLib;

namespace Microsoft.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterNetworkAnalyzerBase
//
/// <summary>
/// Base class for classes that analyze a Twitter network.
/// </summary>
///
/// <remarks>
/// The derived class must call <see cref="BeforeGetNetwork()" /> before
/// getting each network.
/// </remarks>
//*****************************************************************************

public abstract class TwitterNetworkAnalyzerBase : HttpNetworkAnalyzerBase
{
    //*************************************************************************
    //  Constructor: TwitterNetworkAnalyzerBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterNetworkAnalyzerBase" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterNetworkAnalyzerBase()
    {
        m_oTwitterAccessToken = null;

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
            "The Twitter Web service didn't respond.";

        const String RefusedMessage =
            "The Twitter Web service refused to provide the requested"
            + " information."
            ;

        if (oException is WebException)
        {
            WebException oWebException = (WebException)oException;

            if ( WebExceptionIsDueToRateLimit(oWebException) )
            {
                // Note that this shouldn't actually occur, because
                // this.GetXmlDocument() pauses and retries when Twitter rate
                // limits kick in.  This "if" clause is included in case
                // Twitter misbehaves, or if the pause-retry code is ever
                // removed from GetXmlDocument().

                sMessage = String.Format(

                    RefusedMessage 
                    + "  A likely cause is that you have made too many"
                    + " requests in the last hour.  (Twitter limits"
                    + " information requests to prevent its service from being"
                    + " attacked.  Click the '{0}' link for details.)"
                    + "\r\n\r\n"
                    + "Wait 60 minutes and try again."
                    ,
                    TwitterAuthorizationControl.RateLimitingLinkText
                    );
            }
            else if (oWebException.Response is HttpWebResponse)
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
    //  Method: WebExceptionIsDueToRateLimit()
    //
    /// <summary>
    /// Determines whether a WebException is due to Twitter rate limits.
    /// </summary>
    ///
    /// <param name="oWebException">
    /// The WebException to check.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="oWebException" /> is due to Twitter rate limits
    /// kicking in.
    /// </returns>
    //*************************************************************************

    protected Boolean
    WebExceptionIsDueToRateLimit
    (
        WebException oWebException
    )
    {
        Debug.Assert(oWebException != null);
        AssertValid();

        return ( WebExceptionHasHttpStatusCode(oWebException,
            HttpStatusCode.BadRequest, (HttpStatusCode)420) );
    }

    //*************************************************************************
    //  Method: GetRateLimitPauseMs()
    //
    /// <summary>
    /// Gets the time to pause before retrying a request after Twitter rate
    /// limits kicks in.
    /// </summary>
    ///
    /// <param name="oWebException">
    /// The WebException to check.
    /// </param>
    ///
    /// <returns>
    /// The time to pause before retrying a request after Twitter rate limits
    /// kick in, in milliseconds.
    /// </returns>
    //*************************************************************************

    protected Int32
    GetRateLimitPauseMs
    (
        WebException oWebException
    )
    {
        Debug.Assert(oWebException != null);
        AssertValid();

        // The Twitter REST API provides a custom X-RateLimit-Reset header in
        // the response headers.  This is the time at which the request should
        // be made again, in seconds since 1/1/1970, in UTC.  If this header is
        // available, use it.  Otherwise, use a default pause time.
        //
        // Note that the Twitter search API uses a different header for the
        // same purpose, Retry-After, that is in "seconds to wait."  This
        // method doesn't check for that header, because NodeXL makes
        // relatively few calls to the search API and is unlikely to encounter
        // search API rate limiting.

        WebResponse oWebResponse = oWebException.Response;

        if (oWebResponse != null)
        {
            String sXRateLimitReset =
                oWebResponse.Headers["X-RateLimit-Reset"];

            Int32 iSecondsSince1970;

            // (Note that Int32.TryParse() can handle null, which indicates a
            // missing header.)

            if ( Int32.TryParse(sXRateLimitReset, out iSecondsSince1970) )
            {
                DateTime oResetTimeUtc =
                    new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).
                        AddSeconds(iSecondsSince1970);

                Double dRateLimitPauseMs =
                    (oResetTimeUtc - DateTime.UtcNow).TotalMilliseconds;

                // Don't wait longer than two hours.

                if (dRateLimitPauseMs > 0 &&
                    dRateLimitPauseMs <= 2 * 60 * 60 * 1000)
                {
                    return ( (Int32)dRateLimitPauseMs );
                }
            }
        }

        return (DefaultRateLimitPauseMs);
    }

    //*************************************************************************
    //  Method: BeforeGetNetwork()
    //
    /// <summary>
    /// Performs tasks required before getting a network.
    /// </summary>
    ///
    /// <remarks>
    /// The derived class must call this method before getting each network.
    /// </remarks>
    //*************************************************************************

    protected void
    BeforeGetNetwork()
    {
        AssertValid();

        // TwitterAccessToken caches the access token it reads from disk.  Make
        // sure the latest access token is read.

        m_oTwitterAccessToken = new TwitterAccessToken();
    }

    //*************************************************************************
    //  Method: CreateGraphMLXmlDocument()
    //
    /// <summary>
    /// Creates a GraphMLXmlDocument representing a network of Twitter users.
    /// </summary>
    ///
    /// <param name="bIncludeStatistics">
    /// true to include each user's statistics.
    /// </param>
    ///
    /// <param name="bIncludeLatestStatuses">
    /// true to include each user's latest status.
    /// </param>
    ///
    /// <returns>
    /// A GraphMLXmlDocument representing a network of Twitter users.  The
    /// document includes GraphML-attribute definitions but no vertices or
    /// edges.
    /// </returns>
    //*************************************************************************

    protected GraphMLXmlDocument
    CreateGraphMLXmlDocument
    (
        Boolean bIncludeStatistics,
        Boolean bIncludeLatestStatuses
    )
    {
        AssertValid();

        GraphMLXmlDocument oGraphMLXmlDocument = new GraphMLXmlDocument(true);

        if (bIncludeStatistics)
        {
            oGraphMLXmlDocument.DefineGraphMLAttribute(false, FollowedID,
                "Followed", "int", null);

            oGraphMLXmlDocument.DefineGraphMLAttribute(false, FollowersID,
                "Followers", "int", null);

            oGraphMLXmlDocument.DefineGraphMLAttribute(false, StatusesID,
                "Tweets", "int", null);

            oGraphMLXmlDocument.DefineGraphMLAttribute(false, FavoritesID,
                "Favorites", "int", null);

            oGraphMLXmlDocument.DefineGraphMLAttribute(false, DescriptionID,
                "Description", "string", null);

            oGraphMLXmlDocument.DefineGraphMLAttribute(false, TimeZoneID,
                "Time Zone", "string", null);

            oGraphMLXmlDocument.DefineGraphMLAttribute(false, UtcOffsetID,
                "Time Zone UTC Offset (Seconds)", "int", null);

            oGraphMLXmlDocument.DefineGraphMLAttribute(false, JoinedDateUtcID,
                "Joined Twitter Date (UTC)", "string", null);
        }

        if (bIncludeLatestStatuses)
        {
            oGraphMLXmlDocument.DefineGraphMLAttribute(false, LatestStatusID,
                "Latest Tweet", "string", null);

            oGraphMLXmlDocument.DefineGraphMLAttribute(false,
                LatestStatusDateUtcID, "Latest Tweet Date (UTC)", "string",
                null);
        }

        DefineImageFileGraphMLAttribute(oGraphMLXmlDocument);
        DefineCustomMenuGraphMLAttributes(oGraphMLXmlDocument);
        DefineRelationshipGraphMLAttribute(oGraphMLXmlDocument);

        oGraphMLXmlDocument.DefineGraphMLAttribute(true,
            RelationshipDateUtcID, "Relationship Date (UTC)", "string",
            null);

        return (oGraphMLXmlDocument);
    }

    //*************************************************************************
    //  Method: GetFollowedOrFollowingUrl()
    //
    /// <summary>
    /// Gets the Twitter API URL for the people followed by a user or the
    /// people following a user.
    /// </summary>
    ///
    /// <param name="sScreenName">
    /// The user' screen name.
    /// </param>
    ///
    /// <param name="bFollowed">
    /// true to get the URL for the people followed by the user, false to get
    /// the URL for the people following the user.
    /// </param>
    ///
    /// <returns>
    /// The URL for the user's followed or followers.
    /// </returns>
    //*************************************************************************

    protected String
    GetFollowedOrFollowingUrl
    (
        String sScreenName,
        Boolean bFollowed
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenName) );
        AssertValid();

        return ( String.Format(

            "http://twitter.com/statuses/{0}/{1}.xml"
            ,
            bFollowed ? "friends" : "followers",
            UrlUtil.EncodeUrlParameter(sScreenName)
            ) );
    }

    //*************************************************************************
    //  Method: ReportProgressForFollowedOrFollowing()
    //
    /// <summary>
    /// Reports progress before getting the people followed by a user or the
    /// people following a user.
    /// </summary>
    ///
    /// <param name="sScreenName">
    /// The user' screen name.
    /// </param>
    ///
    /// <param name="bFollowed">
    /// true if getting the people followed by the user, false if getting the
    /// people following the user.
    /// </param>
    //*************************************************************************

    protected void
    ReportProgressForFollowedOrFollowing
    (
        String sScreenName,
        Boolean bFollowed
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenName) );
        AssertValid();

        ReportProgress( String.Format(

            "Getting people {0} \"{1}\"."
            ,
            bFollowed ? "followed by" : "following",
            sScreenName
            ) );
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
    /// If an error occurs, an exception is thrown.
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

        Debug.Assert(m_oTwitterAccessToken != null);

        // Has the user authorized NodeXL to use her Twitter account?

        String sToken, sSecret;

        if ( m_oTwitterAccessToken.TryLoad(out sToken, out sSecret) )
        {
            // Yes.  Add the required authorization information to the URL.

            oAuthTwitter oAuthTwitter = new oAuthTwitter();
            oAuthTwitter.Token = sToken;
            oAuthTwitter.TokenSecret = sSecret;

            String sAuthorizedUrl, sAuthorizedPostData;

            oAuthTwitter.ConstructAuthWebRequest(oAuthTwitter.Method.GET, sUrl,
                String.Empty, out sAuthorizedUrl, out sAuthorizedPostData);

            sUrl = sAuthorizedUrl;
        }

        Int32 iRateLimitPauses = 0;

        while (true)
        {
            try
            {
                return ( GetXmlDocumentWithRetries(sUrl,
                    HttpStatusCodesToFailImmediately, oRequestStatistics,
                    null) );
            }
            catch (WebException oWebException)
            {
                if (!WebExceptionIsDueToRateLimit(oWebException) ||
                    iRateLimitPauses > 0)
                {
                    throw;
                }

                // Twitter rate limits have kicked in.  Pause and try again.

                iRateLimitPauses++;
                Int32 iRateLimitPauseMs = GetRateLimitPauseMs(oWebException);

                DateTime oWakeUpTime = DateTime.Now.AddMilliseconds(
                    iRateLimitPauseMs);

                ReportProgress( String.Format(

                    "Reached Twitter rate limits.  Pausing until {0}."
                    ,
                    oWakeUpTime.ToLongTimeString()
                    ) );

                // Don't pause in one large interval, which would prevent
                // cancellation.

                const Int32 SleepCycleDurationMs = 1000;

                Int32 iSleepCycles = (Int32)Math.Ceiling(
                    (Double)iRateLimitPauseMs / SleepCycleDurationMs) ;

                for (Int32 i = 0; i < iSleepCycles; i++)
                {
                    CheckCancellationPending();
                    System.Threading.Thread.Sleep(SleepCycleDurationMs);
                }
            }
        }
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
    /// "a:feed/a:entry/a:author/a:name".
    /// </param>
    ///
    /// <param name="sNamespacePrefix">
    /// Namespace prefix used in the XPath, or null if no namespace is used.
    /// Sample: "a".
    /// </param>
    ///
    /// <param name="sNamespaceUri">
    /// Namespace URI of the namespace prefix used in the XPath, or null if no
    /// namespace is used.  Sample: "http://www.w3.org/2005/Atom".
    /// </param>
    ///
    /// <param name="iMaximumPages">
    /// Maximum number of pages to request, or Int32.MaxValue for no limit.
    /// (This is required for the Twitter search API, which returns an error
    /// instead of an empty list if you request more than 15 pages.)
    /// </param>
    ///
    /// <param name="iMaximumXmlNodes">
    /// Maximum number of child nodes to return, or Int32.MaxValue for no
    /// limit.
    /// </param>
    ///
    /// <param name="bUsePageParameter">
    /// If true, a "page" URL parameter is used for pagination.  If false, a
    /// "cursor" URL parameter is used.
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
    //*************************************************************************

    protected IEnumerable<XmlNode>
    EnumerateXmlNodes
    (
        String sUrl,
        String sXPath,
        String sNamespacePrefix,
        String sNamespaceUri,
        Int32 iMaximumPages,
        Int32 iMaximumXmlNodes,
        Boolean bUsePageParameter,
        Boolean bSkipMostPage1Errors,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUrl) );
        Debug.Assert( !String.IsNullOrEmpty(sXPath) );
        Debug.Assert(iMaximumPages > 0);
        Debug.Assert(iMaximumXmlNodes > 0);
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        // Twitter uses two different paging schemes, and the one to use is
        // specified by bUsePageParameter.  If true, a one-based "page"
        // parameter is used.  If false, a "cursor" parameter is used, and the
        // cursor for the next page is obtained from a "next_cursor" XML node
        // in the results for the current page.

        Int32 iPage = 1;
        String sCursor = "-1";
        Int32 iXmlNodesEnumerated = 0;

        while (true)
        {
            if (iPage > 1)
            {
                ReportProgress("Getting page " + iPage + ".");
            }

            String sUrlWithPagination = String.Format(
            
                "{0}{1}{2}={3}"
                ,
                sUrl,
                sUrl.IndexOf('?') == -1 ? '?' : '&',
                bUsePageParameter ? "page" : "cursor",
                bUsePageParameter ? iPage.ToString() : sCursor
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

                if (iPage > 1)
                {
                    // Always skip errors on page 2 and above.

                    yield break;
                }

                // Don't skip rate limit exceptions on page 1.

                if (
                    bSkipMostPage1Errors
                    &&
                    oException is WebException
                    &&
                    !WebExceptionIsDueToRateLimit( (WebException)oException )
                    )
                {
                    yield break;
                }

                throw (oException);
            }

            XmlNamespaceManager oXmlNamespaceManager =
                new XmlNamespaceManager(oXmlDocument.NameTable);

            if (sNamespacePrefix != null)
            {
                Debug.Assert( !String.IsNullOrEmpty(sNamespacePrefix) );
                Debug.Assert( !String.IsNullOrEmpty(sNamespaceUri) );

                oXmlNamespaceManager.AddNamespace(sNamespacePrefix,
                    sNamespaceUri);
            }

            XmlNodeList oXmlNodesThisPage = oXmlDocument.SelectNodes(sXPath,
                oXmlNamespaceManager);

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

            if (iMaximumPages != Int32.MaxValue && iPage == iMaximumPages + 1)
            {
                yield break;
            }

            if (!bUsePageParameter)
            {
                // A next_cursor value of 0 means "end of data."

                if (
                    !XmlUtil2.TrySelectSingleNodeAsString(oXmlDocument,
                        "users_list/next_cursor/text()", oXmlNamespaceManager,
                        out sCursor)
                    ||
                    sCursor == "0"
                    )
                {
                    yield break;
                }
            }

            // Get the next page...
        }
    }

    //*************************************************************************
    //  Method: TryGetScreenName()
    //
    /// <summary>
    /// Attempts to get a screen name from a "user" XML node.
    /// </summary>
    ///
    /// <param name="oUserXmlNode">
    /// The "user" XML node returned by Twitter.
    /// </param>
    ///
    /// <param name="sScreenName">
    /// Where the screen name gets stored if true is returned.  Always in lower
    /// case.
    /// </param>
    ///
    /// <returns>
    /// true if the screen name was obtained.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetScreenName
    (
        XmlNode oUserXmlNode,
        out String sScreenName
    )
    {
        Debug.Assert(oUserXmlNode != null);
        AssertValid();

        sScreenName = null;

        String sScreenNameUnknownCase;

        if ( XmlUtil2.TrySelectSingleNodeAsString(oUserXmlNode,
            "screen_name/text()", null, out sScreenNameUnknownCase) )
        {
            sScreenName = sScreenNameUnknownCase.ToLower();
            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: TryAppendVertexXmlNode()
    //
    /// <overloads>
    /// Appends a vertex XML node to the GraphML document for a person if such
    /// a node doesn't already exist.
    /// </overloads>
    ///
    /// <summary>
    /// Appends a vertex XML node to the GraphML document for a person if such
    /// a node doesn't already exist and provides the TwitterVertex for the
    /// node.
    /// </summary>
    ///
    /// <param name="sScreenName">
    /// Screen name to add a vertex XML node for.
    /// </param>
    ///
    /// <param name="oUserXmlNode">
    /// The "user" XML node returned by Twitter, or null if a user node isn't
    /// available.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oScreenNameDictionary">
    /// The key is the screen name in lower case and the value is the
    /// corresponding TwitterVertex.
    /// </param>
    ///
    /// <param name="bIncludeStatistics">
    /// true to include the user's statistics if <paramref
    /// name="oUserXmlNode" /> is not null.
    /// </param>
    ///
    /// <param name="bIncludeLatestStatus">
    /// true to include a latest status attribute value if <paramref
    /// name="oUserXmlNode" /> is not null.
    /// </param>
    ///
    /// <param name="oTwitterVertex">
    /// Where the TwitterVertex that wraps the vertex XML node gets stored.
    /// This gets set regardless of whether the node already existed.
    /// </param>
    ///
    /// <returns>
    /// true if a vertex XML node was added, false if a vertex XML node already
    /// existed.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryAppendVertexXmlNode
    (
        String sScreenName,
        XmlNode oUserXmlNode,
        GraphMLXmlDocument oGraphMLXmlDocument,
        Dictionary<String, TwitterVertex> oScreenNameDictionary,
        Boolean bIncludeStatistics,
        Boolean bIncludeLatestStatus,
        out TwitterVertex oTwitterVertex
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenName) );
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oScreenNameDictionary != null);

        oTwitterVertex = null;

        if ( oScreenNameDictionary.TryGetValue(sScreenName,
            out oTwitterVertex) )
        {
            return (false);
        }

        XmlNode oVertexXmlNode = oGraphMLXmlDocument.AppendVertexXmlNode(
            sScreenName);

        oTwitterVertex = new TwitterVertex(oVertexXmlNode);
        oScreenNameDictionary.Add(sScreenName, oTwitterVertex);

        oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode,
            MenuTextID, "Open Twitter Page for This Person");

        oGraphMLXmlDocument.AppendGraphMLAttributeValue( oVertexXmlNode,
            MenuActionID, String.Format(WebPageUrlPattern, sScreenName) );

        if (oUserXmlNode != null)
        {
            AppendFromUserXmlNode(oUserXmlNode, oGraphMLXmlDocument,
                oTwitterVertex, bIncludeStatistics, bIncludeLatestStatus);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryAppendVertexXmlNode()
    //
    /// <summary>
    /// Appends a vertex XML node to the GraphML document for a person if such
    /// a node doesn't already exist.
    /// </summary>
    ///
    /// <param name="sScreenName">
    /// Screen name to add a vertex XML node for.
    /// </param>
    ///
    /// <param name="oUserXmlNode">
    /// The "user" XML node returned by Twitter, or null if a user node isn't
    /// available.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oScreenNameDictionary">
    /// The key is the screen name in lower case and the value is the
    /// corresponding TwitterVertex.
    /// </param>
    ///
    /// <param name="bIncludeStatistics">
    /// true to include the user's statistics.
    /// </param>
    ///
    /// <param name="bIncludeLatestStatus">
    /// true to include a latest status attribute value.
    /// </param>
    ///
    /// <returns>
    /// true if a vertex XML node was added, false if a vertex XML node already
    /// existed.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryAppendVertexXmlNode
    (
        String sScreenName,
        XmlNode oUserXmlNode,
        GraphMLXmlDocument oGraphMLXmlDocument,
        Dictionary<String, TwitterVertex> oScreenNameDictionary,
        Boolean bIncludeStatistics,
        Boolean bIncludeLatestStatus
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenName) );
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oScreenNameDictionary != null);

        TwitterVertex oTwitterVertex;

        return ( TryAppendVertexXmlNode(sScreenName, oUserXmlNode,
            oGraphMLXmlDocument, oScreenNameDictionary, bIncludeStatistics,
            bIncludeLatestStatus, out oTwitterVertex) );
    }

    //*************************************************************************
    //  Method: AppendFromUserXmlNode()
    //
    /// <summary>
    /// Appends GraphML-Attribute values from a "user" XML node returned by
    /// Twitter to a vertex XML node.
    /// </summary>
    ///
    /// <param name="oUserXmlNode">
    /// The "user" XML node returned by Twitter.  Can't be null.
    /// </param>
    /// 
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oTwitterVertex">
    /// Contains the vertex XML node from <paramref
    /// name="oGraphMLXmlDocument" /> to add the GraphML attribute values to.
    /// </param>
    ///
    /// <param name="bIncludeStatistics">
    /// true to include the user's statistics.
    /// </param>
    ///
    /// <param name="bIncludeLatestStatus">
    /// true to include a latest status attribute value.
    /// </param>
    ///
    /// <remarks>
    /// This method reads information from a "user" XML node returned by
    /// Twitter and appends the information to a vertex XML node in the GraphML
    /// document.
    /// </remarks>
    //*************************************************************************

    protected void
    AppendFromUserXmlNode
    (
        XmlNode oUserXmlNode,
        GraphMLXmlDocument oGraphMLXmlDocument,
        TwitterVertex oTwitterVertex,
        Boolean bIncludeStatistics,
        Boolean bIncludeLatestStatus
    )
    {
        Debug.Assert(oUserXmlNode != null);
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oTwitterVertex != null);
        AssertValid();

        XmlNode oVertexXmlNode = oTwitterVertex.VertexXmlNode;

        if (bIncludeStatistics)
        {
            AppendInt32GraphMLAttributeValue(oUserXmlNode,
                "friends_count/text()", null, oGraphMLXmlDocument,
                oVertexXmlNode, FollowedID);

            AppendInt32GraphMLAttributeValue(oUserXmlNode,
                "followers_count/text()", null, oGraphMLXmlDocument,
                oVertexXmlNode, FollowersID);

            AppendInt32GraphMLAttributeValue(oUserXmlNode,
                "statuses_count/text()", null, oGraphMLXmlDocument,
                oVertexXmlNode, StatusesID);

            AppendInt32GraphMLAttributeValue(oUserXmlNode,
                "favourites_count/text()", null, oGraphMLXmlDocument,
                oVertexXmlNode, FavoritesID);

            AppendStringGraphMLAttributeValue(oUserXmlNode,
                "description/text()", null, oGraphMLXmlDocument,
                oVertexXmlNode, DescriptionID);

            AppendStringGraphMLAttributeValue(oUserXmlNode,
                "time_zone/text()", null, oGraphMLXmlDocument, oVertexXmlNode,
                TimeZoneID);

            AppendStringGraphMLAttributeValue(oUserXmlNode,
                "utc_offset/text()", null, oGraphMLXmlDocument, oVertexXmlNode,
                UtcOffsetID);

            String sJoinedDateUtc;

            if ( XmlUtil2.TrySelectSingleNodeAsString(oUserXmlNode,
                "created_at/text()", null, out sJoinedDateUtc) )
            {
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                    oVertexXmlNode, JoinedDateUtcID,
                    TwitterDateParser.ParseTwitterDate(sJoinedDateUtc) );
            }
        }

        String sLatestStatus;

        if ( XmlUtil2.TrySelectSingleNodeAsString(oUserXmlNode,
            "status/text/text()", null, out sLatestStatus) )
        {
            String sLatestStatusDateUtc;

            if ( XmlUtil2.TrySelectSingleNodeAsString(oUserXmlNode,
                "status/created_at/text()", null, out sLatestStatusDateUtc) )
            {
                sLatestStatusDateUtc = TwitterDateParser.ParseTwitterDate(
                    sLatestStatusDateUtc);
            }

            // Don't overwrite any status the derived class may have already
            // stored on the TwitterVertex object.

            if (oTwitterVertex.StatusForAnalysis == null)
            {
                oTwitterVertex.StatusForAnalysis = sLatestStatus;
                oTwitterVertex.StatusForAnalysisDateUtc = sLatestStatusDateUtc;
            }

            if (bIncludeLatestStatus)
            {
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode,
                    LatestStatusID, sLatestStatus);

                if ( !String.IsNullOrEmpty(sLatestStatusDateUtc) )
                {
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                        oVertexXmlNode, LatestStatusDateUtcID,
                        sLatestStatusDateUtc);
                }
            }
        }

        // Add an image URL GraphML-attribute if it hasn't already been added.
        // (It might have been added from an "entry" node by
        // TwitterSearchNetworkAnalyzer.)

        if (oVertexXmlNode.SelectSingleNode(
            "a:data[@key='" + ImageFileID + "']", 
            oGraphMLXmlDocument.CreateXmlNamespaceManager("a") ) == null)
        {
            AppendStringGraphMLAttributeValue(oUserXmlNode,
                "profile_image_url/text()", null, oGraphMLXmlDocument,
                oVertexXmlNode, ImageFileID);
        }
    }

    //*************************************************************************
    //  Method: AppendFromUserXmlNodeCalled()
    //
    /// <summary>
    /// Checks whether <see cref="AppendFromUserXmlNode" /> has been called for
    /// a vertex XML node.
    /// </summary>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oVertexXmlNode">
    /// The vertex XML node to check.
    /// </param>
    ///
    /// <remarks>
    /// true if <see cref="AppendFromUserXmlNode" /> has been called for the
    /// vertex XML node.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    AppendFromUserXmlNodeCalled
    (
        GraphMLXmlDocument oGraphMLXmlDocument,
        XmlNode oVertexXmlNode
    )
    {
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oVertexXmlNode != null);

        XmlNamespaceManager oXmlNamespaceManager =
            oGraphMLXmlDocument.CreateXmlNamespaceManager("a");

        // The FollowedID and LatestStatusID GraphML-attributes are appended
        // only by AppendFromUserXmlNode(), so if either is present,
        // AppendFromUserXmlNode() has been called.

        return (
            oVertexXmlNode.SelectSingleNode(
                "a:data[@key='" + FollowedID + "']", oXmlNamespaceManager)
                != null
            ||
            oVertexXmlNode.SelectSingleNode(
                "a:data[@key='" + LatestStatusID + "']", oXmlNamespaceManager)
                != null
            );
    }

    //*************************************************************************
    //  Method: AppendMissingGraphMLAttributeValues()
    //
    /// <summary>
    /// Appends any missing vertex GraphML attribute values to the GraphML
    /// document.
    /// </summary>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oScreenNameDictionary">
    /// The key is the screen name in lower case and the value is the
    /// corresponding TwitterVertex.
    /// </param>
    ///
    /// <param name="bIncludeStatistics">
    /// true to include each user's statistics.
    /// </param>
    ///
    /// <param name="bIncludeLatestStatuses">
    /// true to include each user's latest status.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <remarks>
    /// This method loops through the vertex nodes in <paramref
    /// name="oGraphMLXmlDocument" /> and for any that are missing attribute
    /// values, attempts to append the values.
    ///
    /// <para>
    /// "Missing" doesn't mean that something went wrong when populating the
    /// GraphML document.  When you create a 1-level network of people followed
    /// by Bob, for example, the "http://twitter.com/statuses/followed/bob.xml"
    /// page returns a detailed "user" XML node for each person followed by
    /// Bob, but not one for Bob himself.
    /// TwitterUserNetworkAnalyzer.GetUserNetworkRecursive() appends a vertex
    /// XML node for Bob to the document, but it consists only of Bob's screen
    /// name without attributes.  This method makes a separate call to Twitter
    /// to fill in Bob's missing attributes.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected void
    AppendMissingGraphMLAttributeValues
    (
        GraphMLXmlDocument oGraphMLXmlDocument,
        Dictionary<String, TwitterVertex> oScreenNameDictionary,
        Boolean bIncludeStatistics,
        Boolean bIncludeLatestStatuses,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oScreenNameDictionary != null);
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        if (!bIncludeStatistics && !bIncludeLatestStatuses)
        {
            return;
        }

        foreach (TwitterVertex oTwitterVertex in oScreenNameDictionary.Values)
        {
            XmlNode oVertexXmlNode = oTwitterVertex.VertexXmlNode;

            if ( AppendFromUserXmlNodeCalled(oGraphMLXmlDocument,
                oVertexXmlNode) )
            {
                continue;
            }


            String sScreenName = XmlUtil2.SelectRequiredSingleNodeAsString(
                oVertexXmlNode, "@id", null);

            String sUrl = String.Format(

                "http://twitter.com/users/show/{0}.xml"
                ,
                UrlUtil.EncodeUrlParameter(sScreenName)
                );

            ReportProgress( String.Format(

                "Getting information about \"{0}\"."
                ,
                sScreenName
                ) );

            XmlDocument oXmlDocument;
            
            try
            {
                oXmlDocument = GetXmlDocument(sUrl, oRequestStatistics);
            }
            catch (Exception oException)
            {
                if ( !ExceptionIsWebOrXml(oException) )
                {
                    throw oException;
                }

                // Skip WebExceptions and XmlExceptions. The missing attributes
                // aren't critical.

                continue;
            }

            // The document consists of a single "user" node.

            XmlNode oUserXmlNode;

            if ( XmlUtil2.TrySelectSingleNode(oXmlDocument, "user", null,
                out oUserXmlNode) )
            {
                AppendFromUserXmlNode(oUserXmlNode, oGraphMLXmlDocument,
                    oTwitterVertex, bIncludeStatistics,
                    bIncludeLatestStatuses);
            }
        }
    }

    //*************************************************************************
    //  Method: AppendRelationshipDateUtcGraphMLAttributeValue()
    //
    /// <summary>
    /// Appends a GraphML attribute value for the relationship date to an edge
    /// XML node.
    /// </summary>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oEdgeXmlNode">
    /// The edge XML node to add the Graph-ML attribute value to.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <remarks>
    /// The Graph-ML attribute value is set to the start time of the network
    /// request.
    /// </remarks>
    //*************************************************************************

    protected void
    AppendRelationshipDateUtcGraphMLAttributeValue
    (
        GraphMLXmlDocument oGraphMLXmlDocument,
        XmlNode oEdgeXmlNode,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oEdgeXmlNode != null);
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode,
            RelationshipDateUtcID, 

            TwitterDateParser.ToCultureInvariantString(
                oRequestStatistics.StartTimeUtc)
            );
    }

    //*************************************************************************
    //  Method: AppendRepliesToAndMentionsXmlNodes()
    //
    /// <summary>
    /// Appends edge XML nodes for replies-to and mentions relationships.
    /// </summary>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oScreenNameDictionary">
    /// The key is the screen name in lower case and the value is the
    /// corresponding TwitterVertex.
    /// </param>
    ///
    /// <param name="bIncludeRepliesToEdges">
    /// true to append edges for replies-to relationships.
    /// </param>
    ///
    /// <param name="bIncludeMentionsEdges">
    /// true to append edges for mentions relationships.
    /// </param>
    //*************************************************************************

    protected void
    AppendRepliesToAndMentionsXmlNodes
    (
        GraphMLXmlDocument oGraphMLXmlDocument,
        Dictionary<String, TwitterVertex> oScreenNameDictionary,
        Boolean bIncludeRepliesToEdges,
        Boolean bIncludeMentionsEdges
    )
    {
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oScreenNameDictionary != null);
        AssertValid();

        if (!bIncludeRepliesToEdges && !bIncludeMentionsEdges)
        {
            return;
        }

        XmlNamespaceManager oGraphMLXmlNamespaceManager =
            oGraphMLXmlDocument.CreateXmlNamespaceManager("g");

        ReportProgress("Examining relationships.");

        // "Starts with a screen name," which means it's a "reply-to".

        Regex oReplyToRegex = new Regex(@"^@(?<ScreenName>\w+)");

        // "Contains a screen name," which means it's a "mentions".
        //
        // Note that a "reply-to" is also a "mentions."

        Regex oMentionsRegex = new Regex(@"(^|\s)@(?<ScreenName>\w+)");

        foreach (KeyValuePair<String, TwitterVertex> oKeyValuePair in
            oScreenNameDictionary)
        {
            String sScreenName = oKeyValuePair.Key;
            TwitterVertex oTwitterVertex = oKeyValuePair.Value;
            String sStatusForAnalysis = oTwitterVertex.StatusForAnalysis;

            String sStatusForAnalysisDateUtc =
                oTwitterVertex.StatusForAnalysisDateUtc;

            if ( String.IsNullOrEmpty(sStatusForAnalysis) )
            {
                continue;
            }

            if (bIncludeRepliesToEdges)
            {
                Match oReplyToMatch = oReplyToRegex.Match(sStatusForAnalysis);

                if (oReplyToMatch.Success)
                {
                    String sReplyToScreenName =
                        oReplyToMatch.Groups["ScreenName"].Value.ToLower();

                    if (
                        sReplyToScreenName != sScreenName
                        &&
                        oScreenNameDictionary.ContainsKey(sReplyToScreenName)
                        )
                    {
                        XmlNode oEdgeXmlNode = AppendEdgeXmlNode(
                            oGraphMLXmlDocument, sScreenName,
                            sReplyToScreenName, "Replies to");

                        if ( !String.IsNullOrEmpty(sStatusForAnalysisDateUtc) )
                        {
                            oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                                oEdgeXmlNode, RelationshipDateUtcID,
                                sStatusForAnalysisDateUtc);
                        }
                    }
                }
            }

            if (bIncludeMentionsEdges)
            {
                Match oMentionsMatch =
                    oMentionsRegex.Match(sStatusForAnalysis);

                while (oMentionsMatch.Success)
                {
                    String sMentionsScreenName =
                        oMentionsMatch.Groups["ScreenName"].Value.ToLower();

                    if (
                        sMentionsScreenName != sScreenName
                        &&
                        oScreenNameDictionary.ContainsKey(sMentionsScreenName)
                        )
                    {
                        XmlNode oEdgeXmlNode = AppendEdgeXmlNode(
                            oGraphMLXmlDocument, sScreenName,
                            sMentionsScreenName, "Mentions");

                        if ( !String.IsNullOrEmpty(sStatusForAnalysisDateUtc) )
                        {
                            oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                                oEdgeXmlNode, RelationshipDateUtcID,
                                sStatusForAnalysisDateUtc);
                        }
                    }

                    oMentionsMatch = oMentionsMatch.NextMatch();
                }
            }
        }
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

        // m_oTwitterAccessToken
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// HTTP status codes that have special meaning with Twitter.  When they
    /// occur, the requests are not retried.

    protected static readonly HttpStatusCode []
        HttpStatusCodesToFailImmediately = new HttpStatusCode[] {

            // Occurs when information about a user who has "protected" status
            // is requested, for example.

            HttpStatusCode.Unauthorized,

            // Occurs when the Twitter search API returns a tweet from a user,
            // but then refuses to provide a list of the people followed by the
            // user, probably because the user has protected her account.
            // (But if she has protected her account, why is the search API
            // returning one of her tweets?)

            HttpStatusCode.NotFound,

            // Twitter used to always return BadRequest when rate limiting
            // kicked in.  Starting January 18, 2010, the search API was
            // changed to return 420 instead.  The REST API still returns
            // BadRequest.

            HttpStatusCode.BadRequest,
            (HttpStatusCode)420,

            // Not sure about what causes this one.

            HttpStatusCode.Forbidden,
        };


    /// Default time to pause before retrying a request after Twitter rate
    /// limits kick in, in milliseconds.

    protected const Int32 DefaultRateLimitPauseMs = 60 * 60 * 1000;


    /// GraphML-attribute IDs.

    protected const String StatusesID = "Statuses";
    ///
    protected const String FavoritesID = "Favorites";
    ///
    protected const String FollowedID = "Followed";
    ///
    protected const String FollowersID = "Followers";
    ///
    protected const String LatestStatusID = "LatestStatus";
    ///
    protected const String LatestStatusDateUtcID = "LatestStatusDateUtc";
    ///
    protected const String DescriptionID = "Description";
    ///
    protected const String TimeZoneID = "TimeZone";
    ///
    protected const String UtcOffsetID = "UtcOffset";
    ///
    protected const String JoinedDateUtcID = "JoinedDateUtc";
    ///
    protected const String RelationshipDateUtcID = "RelationshipDateUtc";

    /// Format pattern for the URL of the Twitter Web page for a person.  The
    /// {0} argument must be replaced with a Twitter screen name.

    protected const String WebPageUrlPattern =
        "http://twitter.com/{0}";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Gets a Twitter access token if the user has been authorized.  null if
    /// BeforeGetNetwork() hasn't been called.

    protected TwitterAccessToken m_oTwitterAccessToken;


    //*************************************************************************
    //  Embedded class: GetNetworkAsyncArgsBase()
    //
    /// <summary>
    /// Base class for classes that contain the arguments needed to
    /// asynchronously get a Twitter network.
    /// </summary>
    //*************************************************************************

    protected class GetNetworkAsyncArgsBase
    {
        ///
        public Int32 MaximumPeoplePerRequest;
    };
}

}
