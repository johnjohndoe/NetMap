
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Xml;
using System.Net;
using System.Web;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.XmlLib;
using Microsoft.SocialNetworkLib;

namespace Microsoft.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterNetworkAnalyzer
//
/// <summary>
/// Analyzes a Twitter network.
/// </summary>
///
/// <remarks>
/// Use <see cref="GetUserNetwork" /> to synchronously get the network of
/// people followed by a Twitter user or people whom a Twitter user follows, or
/// use <see cref="GetUserNetworkAsync" /> to do it asynchronously.
/// </remarks>
//*****************************************************************************

public class TwitterNetworkAnalyzer : HttpNetworkAnalyzerBase
{
    //*************************************************************************
    //  Constructor: TwitterNetworkAnalyzer()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="TwitterNetworkAnalyzer" />
    /// class.
    /// </summary>
    //*************************************************************************

    public TwitterNetworkAnalyzer()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: GetUserNetwork()
    //
    /// <summary>
    /// Synchronously gets the network of people followed by a Twitter user or
    /// people whom a Twitter user follows.
    /// </summary>
    ///
    /// <param name="screenNameToAnalyze">
    /// The screen name of the Twitter user whose network should be analyzed.
    /// </param>
    ///
    /// <param name="includeFollowed">
    /// true to include the people followed by the user.
    /// </param>
    ///
    /// <param name="includeFollowers">
    /// true to include the people following by the user.
    /// </param>
    ///
    /// <param name="networkLevel">
    /// Network level to include.  Must be NetworkLevel.One, OnePointFive, or
    /// Two.
    /// </param>
    ///
    /// <param name="includeLatestTweet">
    /// true to include each user's latest tweet.
    /// </param>
    ///
    /// <param name="maximumPeoplePerRequest">
    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.
    /// </param>
    ///
    /// <param name="credentialsScreenName">
    /// The screen name of the Twitter user whose credentials should be used,
    /// or null to not use credentials.
    /// </param>
    ///
    /// <param name="credentialsPassword">
    /// The password of the Twitter user whose credentials should be used.
    /// Used only if <paramref name="credentialsScreenName" /> is specified.
    /// </param>
    ///
    /// <returns>
    /// An XmlDocument containing the network as GraphML.
    /// </returns>
    //*************************************************************************

    public XmlDocument
    GetUserNetwork
    (
        String screenNameToAnalyze,
        Boolean includeFollowed,
        Boolean includeFollowers,
        NetworkLevel networkLevel,
        Boolean includeLatestTweet,
        Int32 maximumPeoplePerRequest,
        String credentialsScreenName,
        String credentialsPassword
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(screenNameToAnalyze) );

        Debug.Assert(networkLevel == NetworkLevel.One ||
            networkLevel == NetworkLevel.OnePointFive ||
            networkLevel == NetworkLevel.Two);

        Debug.Assert(maximumPeoplePerRequest > 0);

        Debug.Assert( credentialsScreenName == null ||
            ( credentialsScreenName.Length > 0 &&
                !String.IsNullOrEmpty(credentialsPassword) ) );

        AssertValid();

        XmlDocument oXmlDocument;

        Boolean bNotCancelled = TryGetUserNetworkInternal(screenNameToAnalyze,
            includeFollowed, includeFollowers, networkLevel,
            includeLatestTweet, maximumPeoplePerRequest, credentialsScreenName,
            credentialsPassword, null, null, out oXmlDocument);

        Debug.Assert(bNotCancelled);

        return (oXmlDocument);
    }

    //*************************************************************************
    //  Method: GetUserNetworkAsync()
    //
    /// <summary>
    /// Asynchronously gets the network of people followed by a Twitter user or
    /// people whom a Twitter user follows.
    /// </summary>
    ///
    /// <param name="screenNameToAnalyze">
    /// The screen name of the Twitter user whose network should be analyzed.
    /// </param>
    ///
    /// <param name="includeFollowed">
    /// true to include the people followed by the user.
    /// </param>
    ///
    /// <param name="includeFollowers">
    /// true to include the people following by the user.
    /// </param>
    ///
    /// <param name="networkLevel">
    /// Network level to include.
    /// </param>
    ///
    /// <param name="includeLatestTweet">
    /// true to include each user's latest tweet.
    /// </param>
    ///
    /// <param name="maximumPeoplePerRequest">
    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.
    /// </param>
    ///
    /// <param name="credentialsScreenName">
    /// The screen name of the Twitter user whose credentials should be used,
    /// or null to not use credentials.
    /// </param>
    ///
    /// <param name="credentialsPassword">
    /// The password of the Twitter user whose credentials should be used.
    /// Used only if <paramref name="credentialsScreenName" /> is specified.
    /// </param>
    ///
    /// <remarks>
    /// When the analysis completes, the <see
    /// cref="HttpNetworkAnalyzerBase.AnalysisCompleted" /> event fires.  The
    /// <see cref="RunWorkerCompletedEventArgs.Result" /> property will return
    /// an XmlDocument containing the network as GraphML.
    ///
    /// <para>
    /// To cancel the analysis, call <see
    /// cref="HttpNetworkAnalyzerBase.CancelAsync" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    GetUserNetworkAsync
    (
        String screenNameToAnalyze,
        Boolean includeFollowed,
        Boolean includeFollowers,
        NetworkLevel networkLevel,
        Boolean includeLatestTweet,
        Int32 maximumPeoplePerRequest,
        String credentialsScreenName,
        String credentialsPassword
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(screenNameToAnalyze) );

        Debug.Assert(networkLevel == NetworkLevel.One ||
            networkLevel == NetworkLevel.OnePointFive ||
            networkLevel == NetworkLevel.Two);

        Debug.Assert(maximumPeoplePerRequest > 0);

        Debug.Assert( credentialsScreenName == null ||
            ( credentialsScreenName.Length > 0 &&
                !String.IsNullOrEmpty(credentialsPassword) ) );

        AssertValid();

        const String MethodName = "GetUserNetworkAsync";
        CheckIsBusy(MethodName);

        // Wrap the arguments in an object that can be passed to
        // BackgroundWorker.RunWorkerAsync().

        GetUserNetworkAsyncArgs oGetUserNetworkAsyncArgs =
            new GetUserNetworkAsyncArgs();

        oGetUserNetworkAsyncArgs.ScreenNameToAnalyze = screenNameToAnalyze;
        oGetUserNetworkAsyncArgs.IncludeFollowed = includeFollowed;
        oGetUserNetworkAsyncArgs.IncludeFollowers = includeFollowers;
        oGetUserNetworkAsyncArgs.NetworkLevel = networkLevel;
        oGetUserNetworkAsyncArgs.IncludeLatestTweet = includeLatestTweet;

        oGetUserNetworkAsyncArgs.MaximumPeoplePerRequest =
            maximumPeoplePerRequest;

        oGetUserNetworkAsyncArgs.CredentialsScreenName = credentialsScreenName;
        oGetUserNetworkAsyncArgs.CredentialsPassword = credentialsPassword;

        m_oBackgroundWorker.RunWorkerAsync(oGetUserNetworkAsyncArgs);
    }

    //*************************************************************************
    //  Method: TryGetUserNetworkInternal()
    //
    /// <summary>
    /// Attempts to get the network of people followed by a Twitter user or
    /// people whom a Twitter user follows.
    /// </summary>
    ///
    /// <param name="sScreenNameToAnalyze">
    /// The screen name of the Twitter user whose network should be analyzed.
    /// </param>
    ///
    /// <param name="bIncludeFollowed">
    /// true to include the people followed by the user.
    /// </param>
    ///
    /// <param name="bIncludeFollowers">
    /// true to include the people following by the user.
    /// </param>
    ///
    /// <param name="eNetworkLevel">
    /// Network level to include.  Must be NetworkLevel.One, OnePointFive, or
    /// Two.
    /// </param>
    ///
    /// <param name="bIncludeLatestTweet">
    /// true to include each user's latest tweet.
    /// </param>
    ///
    /// <param name="iMaximumPeoplePerRequest">
    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.
    /// </param>
    ///
    /// <param name="sCredentialsScreenName">
    /// The screen name of the Twitter user whose credentials should be used,
    /// or null to not use credentials.
    /// </param>
    ///
    /// <param name="sCredentialsPassword">
    /// The password of the Twitter user whose credentials should be used.
    /// Used only if <paramref name="sCredentialsScreenName" /> is specified.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// A BackgroundWorker object if this method is being called
    /// asynchronously, or null if it is being called synchronously.
    /// </param>
    ///
    /// <param name="oDoWorkEventArgs">
    /// A DoWorkEventArgs object if this method is being called
    /// asynchronously, or null if it is being called synchronously.
    /// </param>
    ///
    /// <param name="oXmlDocument">
    /// Where an XmlDocument containing the network as GraphML gets stored if
    /// true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the network was obtained, or false if the user cancelled.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetUserNetworkInternal
    (
        String sScreenNameToAnalyze,
        Boolean bIncludeFollowed,
        Boolean bIncludeFollowers,
        NetworkLevel eNetworkLevel,
        Boolean bIncludeLatestTweet,
        Int32 iMaximumPeoplePerRequest,
        String sCredentialsScreenName,
        String sCredentialsPassword,
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs,
        out XmlDocument oXmlDocument
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenNameToAnalyze) );

        Debug.Assert(eNetworkLevel == NetworkLevel.One ||
            eNetworkLevel == NetworkLevel.OnePointFive ||
            eNetworkLevel == NetworkLevel.Two);

        Debug.Assert(iMaximumPeoplePerRequest > 0);

        Debug.Assert( sCredentialsScreenName == null ||
            ( sCredentialsScreenName.Length > 0 &&
                !String.IsNullOrEmpty(sCredentialsPassword) ) );

        Debug.Assert(oBackgroundWorker == null || oDoWorkEventArgs != null);
        AssertValid();

        oXmlDocument = null;

        // The key is the screen name and the value isn't used.  This is used
        // to prevent the same screen name from being added to the XmlDocument
        // twice.

        Dictionary<String, Byte> oScreenNameDictionary =
            new Dictionary<String, Byte>();

        // Here is the XML created by this method:
        //
        /*
        <?xml version="1.0" encoding="UTF-8"?>
        <graphml xmlns="http://graphml.graphdrawing.org/xmlns">

            <key id="Followed" for="node" attr.name="Followed"
                attr.type="int" />
            <key id="Followers" for="node" attr.name="Followers"
                attr.type="int" />
            <key id="Tweets" for="node" attr.name="Tweets" attr.type="int" />
            <key id="LatestTweet" for="node" attr.name="Latest Tweet"
                attr.type="string" />
            <key id="MenuText" for="node" attr.name="Custom Menu Item Text"
                attr.type="string" />
            <key id="MenuAction" for="node" attr.name="Custom Menu Item Action"
                attr.type="string" />

            <graph edgedefault="directed">
                <node id="ScreenName1">
                    <data key="Followed">9</data>
                    <data key="Followers">10</data>
                    <data key="Tweets">3</data>
                    <data key="MenuText">Open Twitter Page for This Person
                        </data>
                    <data key="MenuAction">http://twitter.com/ScreenName1</data>
                </node>
                <node id="ScreenName2">
                    <data key="Followed">98</data>
                    <data key="Followers">248</data>
                    <data key="Tweets">469</data>
                    <data key="MenuText">Open Twitter Page for This Person
                        </data>
                    <data key="MenuAction">http://twitter.com/ScreenName2</data>
                </node>
                ...
                <edge source="ScreenName1" target="ScreenName2" />
                ...
            </graph>
        </graphml>
        */

        GraphMLXmlDocument oGraphMLXmlDocument = new GraphMLXmlDocument(true);

        oGraphMLXmlDocument.DefineGraphMLAttribute(false, FollowedID,
            "Followed", "int", null);

        oGraphMLXmlDocument.DefineGraphMLAttribute(false, FollowersID,
            "Followers", "int", null);

        oGraphMLXmlDocument.DefineGraphMLAttribute(false, TweetsID,
            "Tweets", "int", null);

        oGraphMLXmlDocument.DefineGraphMLAttribute(false, LatestTweetID,
            "Latest Tweet", "string", null);

        oGraphMLXmlDocument.DefineGraphMLAttribute(false, MenuTextID,
            "Custom Menu Item Text", "string", null);

        oGraphMLXmlDocument.DefineGraphMLAttribute(false, MenuActionID,
            "Custom Menu Item Action", "string", null);

        // Include followed, followers, both, or neither.

        Boolean [] abIncludes = new Boolean[]
            {bIncludeFollowed, bIncludeFollowers};

        for (Int32 i = 0; i < 2; i++)
        {
            if ( abIncludes[i] )
            {
                if ( !TryGetUserNetworkRecursive(sScreenNameToAnalyze,
                    (i == 0), eNetworkLevel, bIncludeLatestTweet,
                    iMaximumPeoplePerRequest, sCredentialsScreenName,
                    sCredentialsPassword, 1, oGraphMLXmlDocument,
                    oScreenNameDictionary, oBackgroundWorker,
                    oDoWorkEventArgs) )
                {
                    return (false);
                }
            }
        }

        AppendMissingGraphMLAttributeValues(oGraphMLXmlDocument,
            bIncludeLatestTweet, sCredentialsScreenName, sCredentialsPassword,
            oBackgroundWorker, oDoWorkEventArgs);

        oXmlDocument = oGraphMLXmlDocument;
        return (true);
    }

    //*************************************************************************
    //  Method: TryGetUserNetworkRecursive()
    //
    /// <summary>
    /// Attempts to recursively get the network of people followed by a Twitter
    /// user or people whom a Twitter user follows.
    /// </summary>
    ///
    /// <param name="sScreenName">
    /// The screen name to use in this call.
    /// </param>
    ///
    /// <param name="bFollowed">
    /// true to include the people followed by the user, false to include the
    /// people following the user.
    /// </param>
    ///
    /// <param name="eNetworkLevel">
    /// Network level to include.  Must be NetworkLevel.One, OnePointFive, or
    /// Two.
    /// </param>
    ///
    /// <param name="bIncludeLatestTweet">
    /// true to include each user's latest tweet.
    /// </param>
    ///
    /// <param name="iMaximumPeoplePerRequest">
    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.
    /// </param>
    ///
    /// <param name="sCredentialsScreenName">
    /// The screen name of the Twitter user whose credentials should be used,
    /// or null to not use credentials.
    /// </param>
    ///
    /// <param name="sCredentialsPassword">
    /// The password of the Twitter user whose credentials should be used.
    /// Used only if <paramref name="sCredentialsScreenName" /> is specified.
    /// </param>
    ///
    /// <param name="iRecursionLevel">
    /// Recursion level for this call.  Must be 1 or 2.  Gets incremented when
    /// recursing.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oScreenNameDictionary">
    /// The key is the screen name and the value isn't used.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// A BackgroundWorker object if this method is being called
    /// asynchronously, or null if it is being called synchronously.
    /// </param>
    ///
    /// <param name="oDoWorkEventArgs">
    /// A DoWorkEventArgs object if this method is being called
    /// asynchronously, or null if it is being called synchronously.
    /// </param>
    ///
    /// <returns>
    /// true if the network was obtained, or false if the user cancelled.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetUserNetworkRecursive
    (
        String sScreenName,
        Boolean bFollowed,
        NetworkLevel eNetworkLevel,
        Boolean bIncludeLatestTweet,
        Int32 iMaximumPeoplePerRequest,
        String sCredentialsScreenName,
        String sCredentialsPassword,
        Int32 iRecursionLevel,
        GraphMLXmlDocument oGraphMLXmlDocument,
        Dictionary<String, Byte> oScreenNameDictionary,
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenName) );

        Debug.Assert(eNetworkLevel == NetworkLevel.One ||
            eNetworkLevel == NetworkLevel.OnePointFive ||
            eNetworkLevel == NetworkLevel.Two);

        Debug.Assert( sCredentialsScreenName == null ||
            ( sCredentialsScreenName.Length > 0 &&
                !String.IsNullOrEmpty(sCredentialsPassword) ) );

        Debug.Assert(iRecursionLevel == 1 || iRecursionLevel == 2);
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oScreenNameDictionary != null);
        Debug.Assert(oBackgroundWorker == null || oDoWorkEventArgs != null);
        AssertValid();

        /*
        Here is what this method should do, based on the eNetworkLevel and
        iRecursionLevel parameters.

                eNetworkLevel

               |One               | OnePointFive      | Two
            ---|------------------| ------------------| ----------------- 
        i   1  |Add all vertices. | Add all vertices. | Add all vertices.
        R      |                  |                   |
        e      |Add all edges.    | Add all edges.    | Add all edges.
        c      |                  |                   |
        u      |Do not recurse.   | Recurse.          | Recurse.
        r      |                  |                   |
        s   ---|------------------|-------------------|------------------
        i   2  |Impossible.       | Do not add        | Add all vertices.
        o      |                  | vertices.         |
        n      |                  |                   |
        L      |                  | Add edges only if | Add all edges.
        e      |                  | vertices are      |
        v      |                  | already included. |
        e      |                  |                   |
        l      |                  | Do not recurse.   | Do not recurse.
               |                  |                   |                  
            ---|------------------|-------------------|------------------
        */

        Boolean bNeedToRecurse = (
            iRecursionLevel == 1
            &&
            (eNetworkLevel == NetworkLevel.OnePointFive ||
            eNetworkLevel == NetworkLevel.Two)
            );

        String sUrl = String.Format(

            "http://twitter.com/statuses/{0}/{1}.xml"
            ,
            bFollowed ? "friends" : "followers",
            HttpUtility.UrlEncode(sScreenName)
            );

        // The document consists of a single "users" node with zero or more
        // "user" child nodes.

        Boolean bUserNodeExists = false;
        List<String> oScreenNamesToRecurse = new List<String>();

        foreach ( XmlNode oUserXmlNode in EnumerateXmlNodes(
            sUrl, "users/user", sCredentialsScreenName, sCredentialsPassword,
            iMaximumPeoplePerRequest) )
        {
            if ( CancelIfRequested(oBackgroundWorker, oDoWorkEventArgs) )
            {
                return (false);
            }

            String sOtherScreenName;

            if ( !XmlUtil.GetStringNodeValue(oUserXmlNode, "screen_name",
                false, out sOtherScreenName) )
            {
                // Nothing can be done without a screen name.

                continue;
            }

            bUserNodeExists = true;

            if (eNetworkLevel != NetworkLevel.OnePointFive ||
                iRecursionLevel == 1)
            {
                AppendVertexXmlNode(sOtherScreenName, oUserXmlNode,
                    oGraphMLXmlDocument, oScreenNameDictionary,
                    bIncludeLatestTweet);
            }

            if (bNeedToRecurse)
            {
                oScreenNamesToRecurse.Add(sOtherScreenName);
            }

            if ( eNetworkLevel != NetworkLevel.OnePointFive ||
                iRecursionLevel == 1 ||
                oScreenNameDictionary.ContainsKey(sOtherScreenName) )
            {
                if (bFollowed)
                {
                    oGraphMLXmlDocument.AppendEdgeXmlNode(sScreenName,
                        sOtherScreenName);
                }
                else
                {
                    oGraphMLXmlDocument.AppendEdgeXmlNode(sOtherScreenName,
                        sScreenName);
                }
            }
        }

        if (bUserNodeExists)
        {
            AppendVertexXmlNode(sScreenName, null, oGraphMLXmlDocument,
                oScreenNameDictionary, bIncludeLatestTweet);
        }

        if (bNeedToRecurse)
        {
            foreach (String sScreenNameToRecurse in oScreenNamesToRecurse)
            {
                if ( !TryGetUserNetworkRecursive(sScreenNameToRecurse,
                    bFollowed, eNetworkLevel, bIncludeLatestTweet,
                    iMaximumPeoplePerRequest, sCredentialsScreenName,
                    sCredentialsPassword, 2, oGraphMLXmlDocument,
                    oScreenNameDictionary, oBackgroundWorker,
                    oDoWorkEventArgs) )
                {
                    return (false);
                }
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: GetXmlDocument()
    //
    /// <overloads>
    /// Gets an XML document from an URL.
    /// </overloads>
    ///
    /// <summary>
    /// Gets an XML document from an URL with a specified number of retries.
    /// </summary>
    ///
    /// <param name="sUrl">
    /// The URL to get the document from.
    /// </param>
    ///
    /// <param name="sCredentialsScreenName">
    /// The screen name of the Twitter user whose credentials should be used,
    /// or null to not use credentials.
    /// </param>
    ///
    /// <param name="sCredentialsPassword">
    /// The password name of the Twitter user whose credentials should be used.
    /// Used only if <paramref name="sCredentialsScreenName" /> is specified.
    /// </param>
    ///
    /// <param name="iRetries">
    /// The maximum number of retries.
    /// </param>
    ///
    /// <returns>
    /// The XmlDocument from the URL.
    /// </returns>
    //*************************************************************************

    protected XmlDocument
    GetXmlDocument
    (
        String sUrl,
        String sCredentialsScreenName,
        String sCredentialsPassword,
        Int32 iRetries
    )
    {
        Debug.Assert(iRetries >= 0);
        AssertValid();

        while (true)
        {
            try
            {
                return ( GetXmlDocument(sUrl, sCredentialsScreenName,
                    sCredentialsPassword) );
            }
            catch (Exception oException)
            {
                iRetries--;

                if (iRetries < 0)
                {
                    throw (oException);
                }
            }
        }
    }

    //*************************************************************************
    //  Method: GetXmlDocument()
    //
    /// <summary>
    /// Gets an XML document from an URL with no retries.
    /// </summary>
    ///
    /// <param name="sUrl">
    /// The URL to get the document from.
    /// </param>
    ///
    /// <param name="sCredentialsScreenName">
    /// The screen name of the Twitter user whose credentials should be used,
    /// or null to not use credentials.
    /// </param>
    ///
    /// <param name="sCredentialsPassword">
    /// The password name of the Twitter user whose credentials should be used.
    /// Used only if <paramref name="sCredentialsScreenName" /> is specified.
    /// </param>
    ///
    /// <returns>
    /// The XmlDocument from the URL.
    /// </returns>
    //*************************************************************************

    protected XmlDocument
    GetXmlDocument
    (
        String sUrl,
        String sCredentialsScreenName,
        String sCredentialsPassword
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUrl) );

        Debug.Assert( sCredentialsScreenName == null ||
            ( sCredentialsScreenName.Length > 0 &&
                !String.IsNullOrEmpty(sCredentialsPassword) ) );

        AssertValid();

        HttpWebRequest oHttpWebRequest =
            (HttpWebRequest)WebRequest.Create(sUrl);

        if (sCredentialsScreenName != null)
        {
            #if false

            Don't use the following standard .NET technique, because .NET
            doesn't send an Authorization header on the first request and
            relies instead on the server to ask for the header via a 401
            response.  Twitter doesn't send a 401 response (it sends a 200
            response instead), so the user would never be authenticated.

            oHttpWebRequest.Credentials = new NetworkCredential(
                sCredentialsScreenName, sCredentialsPassword);

            #endif

            // The following technique was suggested here:
            //
            // http://devproj20.blogspot.com/2008/02/
            // assigning-basic-authorization-http.html 

            String sHeaderValue = String.Format(
                "{0}:{1}"
                ,
                sCredentialsScreenName,
                sCredentialsPassword
                );

            Byte [] abtHeaderValue = Encoding.UTF8.GetBytes(
                sHeaderValue.ToCharArray() );

            oHttpWebRequest.Headers["Authorization"] =
                "Basic " + Convert.ToBase64String(abtHeaderValue);
        }

        return ( GetXmlDocument(oHttpWebRequest) );
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
    /// The URL to get the document from.  Cannot include URL parameters.
    /// </param>
    ///
    /// <param name="sXPath">
    /// The XPath of the child nodes to enumerate.
    /// </param>
    ///
    /// <param name="sCredentialsScreenName">
    /// The screen name of the Twitter user whose credentials should be used,
    /// or null to not use credentials.
    /// </param>
    ///
    /// <param name="sCredentialsPassword">
    /// The password name of the Twitter user whose credentials should be used.
    /// Used only if <paramref name="sCredentialsScreenName" /> is specified.
    /// </param>
    ///
    /// <param name="iMaximumXmlNodes">
    /// Maximum number child nodes to return, or Int32.MaxValue for no limit.
    /// </param>
    //*************************************************************************

    protected IEnumerable<XmlNode>
    EnumerateXmlNodes
    (
        String sUrl,
        String sXPath,
        String sCredentialsScreenName,
        String sCredentialsPassword,
        Int32 iMaximumXmlNodes
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUrl) );
        Debug.Assert(sUrl.IndexOf('?') == -1);
        Debug.Assert( !String.IsNullOrEmpty(sXPath) );

        Debug.Assert( sCredentialsScreenName == null ||
            ( sCredentialsScreenName.Length > 0 &&
                !String.IsNullOrEmpty(sCredentialsPassword) ) );

        AssertValid();

        // Twitter uses a paging scheme, where each page contains up to
        // MaximumPeoplePerPage people and a particular one-based page is
        // specified via an URL parameter.  Loop through the pages.

        Int32 iPage = 1;
        Int32 iXmlNodes = 0;

        while (true)
        {
            String sUrlWithPage = sUrl + "?page=" + iPage.ToString();

            XmlDocument oXmlDocument = GetXmlDocument(sUrlWithPage,
                sCredentialsScreenName, sCredentialsPassword,
                this.HttpWebRequestRetries);

            Int32 iXmlNodesOnThisPage = 0;

            foreach ( XmlNode oXmlNode in oXmlDocument.SelectNodes(sXPath) )
            {
                if (iXmlNodes == iMaximumXmlNodes)
                {
                    yield break;
                }

                iXmlNodesOnThisPage++;
                iXmlNodes++;
                yield return (oXmlNode);
            }

            if (iXmlNodesOnThisPage < MaximumPeoplePerPage)
            {
                // There is no need to request another page, which would be
                // empty.

                yield break;
            }

            iPage++;
        }
    }

    //*************************************************************************
    //  Method: AppendVertexXmlNode()
    //
    /// <summary>
    /// Appends a vertex XML node to the document for a person if such a node
    /// doesn't already exist.
    /// </summary>
    ///
    /// <param name="sScreenName">
    /// Screen name to add a vertex XML node for.
    /// </param>
    ///
    /// <param name="oUserXmlNode">
    /// The "user" XML node returned by Twitter, or null if a user node isn't
    /// available.  (See the notes in <see
    /// cref="AppendMissingGraphMLAttributeValues" /> for details on why this
    /// can be null.)
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oScreenNameDictionary">
    /// The key is the screen name and the value isn't used.
    /// </param>
    ///
    /// <param name="bIncludeLatestTweet">
    /// true to include a latest tweet attribute value.
    /// </param>
    //*************************************************************************

    protected void
    AppendVertexXmlNode
    (
        String sScreenName,
        XmlNode oUserXmlNode,
        GraphMLXmlDocument oGraphMLXmlDocument,
        Dictionary<String, Byte> oScreenNameDictionary,
        Boolean bIncludeLatestTweet
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenName) );
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oScreenNameDictionary != null);

        if ( !oScreenNameDictionary.ContainsKey(sScreenName) )
        {
            XmlNode oVertexXmlNode = oGraphMLXmlDocument.AppendVertexXmlNode(
                sScreenName);

            oScreenNameDictionary.Add(sScreenName, 0);

            if (oUserXmlNode != null)
            {
                AppendGraphMLAttributeValues(sScreenName, oUserXmlNode,
                    oGraphMLXmlDocument, oVertexXmlNode, bIncludeLatestTweet);
            }
        }
    }

    //*************************************************************************
    //  Method: AppendGraphMLAttributeValues()
    //
    /// <summary>
    /// Appends GraphML-Attribute values to a vertex XML node. 
    /// </summary>
    ///
    /// <param name="sScreenName">
    /// Screen name corresponding to the "user" XML node.
    /// </param>
    ///
    /// <param name="oUserXmlNode">
    /// The "user" XML node returned by Twitter.
    /// available.
    /// </param>
    /// 
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oVertexXmlNode">
    /// The vertex XML node from <paramref name="oGraphMLXmlDocument" /> to add
    /// the GraphML attribute values to.
    /// </param>
    ///
    /// <param name="bIncludeLatestTweet">
    /// true to include a latest tweet attribute value.
    /// </param>
    ///
    /// <remarks>
    /// This method reads information from a "user" XML node returned by
    /// Twitter and stores the information on a vertex XML node in the GraphML
    /// document.
    /// </remarks>
    //*************************************************************************

    protected void
    AppendGraphMLAttributeValues
    (
        String sScreenName,
        XmlNode oUserXmlNode,
        GraphMLXmlDocument oGraphMLXmlDocument,
        XmlNode oVertexXmlNode,
        Boolean bIncludeLatestTweet
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenName) );
        Debug.Assert(oUserXmlNode != null);
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oVertexXmlNode != null);
        AssertValid();

        AppendInt32GraphMLAttributeValue(oUserXmlNode, "friends_count",
            oGraphMLXmlDocument, oVertexXmlNode, FollowedID);

        AppendInt32GraphMLAttributeValue(oUserXmlNode, "followers_count",
            oGraphMLXmlDocument, oVertexXmlNode, FollowersID);

        AppendInt32GraphMLAttributeValue(oUserXmlNode, "statuses_count",
            oGraphMLXmlDocument, oVertexXmlNode, TweetsID);

        String sLatestTweet;

        if ( bIncludeLatestTweet && XmlUtil.GetStringNodeValue(
            oUserXmlNode, "status/text", false, out sLatestTweet) )
        {
            oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode,
                LatestTweetID, sLatestTweet);
        }

        oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode,
            MenuTextID, "Open Twitter Page for This Person");

        oGraphMLXmlDocument.AppendGraphMLAttributeValue( oVertexXmlNode,
            MenuActionID, String.Format(WebPageUrlPattern, sScreenName) );
    }

    //*************************************************************************
    //  Method: AppendInt32GraphMLAttributeValue()
    //
    /// <summary>
    /// Appends an Int32 GraphML-Attribute value to a vertex XML node. 
    /// </summary>
    ///
    /// <param name="oUserXmlNode">
    /// The "user" XML node returned by Twitter.
    /// available.
    /// </param>
    /// 
    /// <param name="sUserAttributeName">
    /// Name of the attribute on <paramref name="oUserXmlNode" /> to read.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oVertexXmlNode">
    /// The vertex XML node from <paramref name="oGraphMLXmlDocument" /> to add
    /// the GraphML attribute value to.
    /// </param>
    ///
    /// <param name="sGraphMLAttributeID">
    /// GraphML ID of the attribute.
    /// </param>
    ///
    /// <remarks>
    /// This method reads an Int32 attribute from a "user" XML node returned by
    /// Twitter and stores the attribute value on a vertex XML node in the
    /// GraphML document.
    /// </remarks>
    //*************************************************************************

    protected void
    AppendInt32GraphMLAttributeValue
    (
        XmlNode oUserXmlNode,
        String sUserAttributeName,
        GraphMLXmlDocument oGraphMLXmlDocument,
        XmlNode oVertexXmlNode,
        String sGraphMLAttributeID
    )
    {
        Debug.Assert(oUserXmlNode != null);
        Debug.Assert( !String.IsNullOrEmpty(sUserAttributeName) );
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oVertexXmlNode != null);
        Debug.Assert( !String.IsNullOrEmpty(sGraphMLAttributeID) );
        AssertValid();

        Int32 iAttributeValue;

        if ( XmlUtil.GetInt32NodeValue(oUserXmlNode, sUserAttributeName, false,
            out iAttributeValue) )
        {
            oGraphMLXmlDocument.AppendGraphMLAttributeValue( oVertexXmlNode,
                sGraphMLAttributeID, iAttributeValue.ToString() );
        }
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
    /// <param name="bIncludeLatestTweet">
    /// true to include each user's latest tweet.
    /// </param>
    ///
    /// <param name="sCredentialsScreenName">
    /// The screen name of the Twitter user whose credentials should be used,
    /// or null to not use credentials.
    /// </param>
    ///
    /// <param name="sCredentialsPassword">
    /// The password name of the Twitter user whose credentials should be used.
    /// Used only if <paramref name="sCredentialsScreenName" /> is specified.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// A BackgroundWorker object if this method is being called
    /// asynchronously, or null if it is being called synchronously.
    /// </param>
    ///
    /// <param name="oDoWorkEventArgs">
    /// A DoWorkEventArgs object if this method is being called
    /// asynchronously, or null if it is being called synchronously.
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
    /// Bob, but not one for Bob himself.  GetUserNetworkRecursive() appends a
    /// vertex XML node for Bob to the document, but it consists only of Bob's
    /// screen name without attributes.  This method makes a separate call to
    /// Twitter to fill in Bob's missing attributes.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected void
    AppendMissingGraphMLAttributeValues
    (
        GraphMLXmlDocument oGraphMLXmlDocument,
        Boolean bIncludeLatestTweet,
        String sCredentialsScreenName,
        String sCredentialsPassword,
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs
    )
    {
        Debug.Assert(oGraphMLXmlDocument != null);

        Debug.Assert( sCredentialsScreenName == null ||
            ( sCredentialsScreenName.Length > 0 &&
                !String.IsNullOrEmpty(sCredentialsPassword) ) );

        Debug.Assert(oBackgroundWorker == null || oDoWorkEventArgs != null);
        AssertValid();

        XmlNamespaceManager oXmlNamespaceManager = 
            oGraphMLXmlDocument.CreateXmlNamespaceManager("g");

        foreach ( XmlNode oVertexXmlNode in oGraphMLXmlDocument.SelectNodes(
            "g:graphml/g:graph/g:node", oXmlNamespaceManager) )
        {
            if (oVertexXmlNode.SelectSingleNode("g:data", oXmlNamespaceManager)
                != null)
            {
                // The user node has at least one GraphML-attribute.

                continue;
            }

            if ( CancelIfRequested(oBackgroundWorker, oDoWorkEventArgs) )
            {
                return;
            }

            String sScreenName;

            XmlUtil.GetAttribute(oVertexXmlNode, "id", true,
                out sScreenName);

            String sUrl = String.Format(

                "http://twitter.com/users/show/{0}.xml"
                ,
                HttpUtility.UrlEncode(sScreenName)
                );

            XmlDocument oXmlDocument = GetXmlDocument(sUrl,
                sCredentialsScreenName, sCredentialsPassword,
                this.HttpWebRequestRetries);

            // The document consists of a single "user" node.

            XmlNode oUserXmlNode = oXmlDocument.SelectSingleNode("user");

            if (oUserXmlNode != null)
            {
                AppendGraphMLAttributeValues(sScreenName, oUserXmlNode,
                    oGraphMLXmlDocument, oVertexXmlNode,
                    bIncludeLatestTweet);
            }
        }
    }

    //*************************************************************************
    //  Method: BackgroundWorker_DoWork()
    //
    /// <summary>
    /// Handles the DoWork event on the BackgroundWorker object.
    /// </summary>
    ///
    /// <param name="sender">
    /// Source of the event.
    /// </param>
    ///
    /// <param name="e">
    /// Standard mouse event arguments.
    /// </param>
    //*************************************************************************

    protected override void
    BackgroundWorker_DoWork
    (
        object sender,
        DoWorkEventArgs e
    )
    {
        Debug.Assert(sender is BackgroundWorker);

        BackgroundWorker oBackgroundWorker = (BackgroundWorker)sender;

        Debug.Assert(e.Argument is GetUserNetworkAsyncArgs);

        GetUserNetworkAsyncArgs oGetUserNetworkAsyncArgs =
            (GetUserNetworkAsyncArgs)e.Argument;

        XmlDocument oGraphMLDocument;
        
        if ( TryGetUserNetworkInternal(
            oGetUserNetworkAsyncArgs.ScreenNameToAnalyze,
            oGetUserNetworkAsyncArgs.IncludeFollowed,
            oGetUserNetworkAsyncArgs.IncludeFollowers,
            oGetUserNetworkAsyncArgs.NetworkLevel,
            oGetUserNetworkAsyncArgs.IncludeLatestTweet,
            oGetUserNetworkAsyncArgs.MaximumPeoplePerRequest,
            oGetUserNetworkAsyncArgs.CredentialsScreenName,
            oGetUserNetworkAsyncArgs.CredentialsPassword,
            oBackgroundWorker, e, out oGraphMLDocument) )
        {
            e.Result = oGraphMLDocument;
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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Maximum number of people returned by Twitter in a single page.

    protected const Int32 MaximumPeoplePerPage = 100;

    /// GraphML-attribute IDs.

    protected const String TweetsID = "Tweets";
    ///
    protected const String FollowedID = "Followed";
    ///
    protected const String FollowersID = "Followers";
    ///
    protected const String LatestTweetID = "LatestTweet";
    ///
    protected const String MenuTextID = "MenuText";
    ///
    protected const String MenuActionID = "MenuAction";

    /// Format pattern for the URL of the Twitter Web page for a person.  The
    /// {0} argument must be replaced with a Twitter screen name.

    protected const String WebPageUrlPattern =
        "http://twitter.com/{0}";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)


    //*************************************************************************
    //  Embedded class: GetUserNetworkAsyncArguments()
    //
    /// <summary>
    /// Contains the arguments needed to asynchronously get the network of
    /// people followed by a Twitter user or people whom a Twitter user
    /// follows.
    /// </summary>
    //*************************************************************************

    protected class GetUserNetworkAsyncArgs
    {
        ///
        public String ScreenNameToAnalyze;
        ///
        public Boolean IncludeFollowed;
        ///
        public Boolean IncludeFollowers;
        ///
        public NetworkLevel NetworkLevel;
        ///
        public Boolean IncludeLatestTweet;
        ///
        public Int32 MaximumPeoplePerRequest;
        ///
        public String CredentialsScreenName;
        ///
        public String CredentialsPassword;
    };
}

}
