
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Xml;
using System.Web;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.SocialNetworkLib;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.XmlLib;

namespace Microsoft.NodeXL.GraphDataProviders.YouTube
{
//*****************************************************************************
//  Class: YouTubeUserNetworkAnalyzer
//
/// <summary>
/// Gets a network of YouTube users.
/// </summary>
///
/// <remarks>
/// Use <see cref="GetNetworkAsync" /> to asynchronously get a directed network
/// of YouTube users.
/// </remarks>
//*****************************************************************************

public class YouTubeUserNetworkAnalyzer : YouTubeNetworkAnalyzerBase
{
    //*************************************************************************
    //  Constructor: YouTubeUserNetworkAnalyzer()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="YouTubeUserNetworkAnalyzer" /> class.
    /// </summary>
    //*************************************************************************

    public YouTubeUserNetworkAnalyzer()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Enum: WhatToInclude
    //
    /// <summary>
    /// Flags that specify what should be included in a network requested from
    /// this class.
    /// </summary>
    ///
    /// <remarks>
    /// The flags can be ORed together.
    /// </remarks>
    //*************************************************************************

    [System.FlagsAttribute]

    public enum
    WhatToInclude
    {
        /// <summary>
        /// Include nothing.
        /// </summary>

        None = 0,

        /// <summary>
        /// Include a vertex for each of the user's friends.
        /// </summary>

        FriendVertices = 1,

        /// <summary>
        /// Include a vertex for each person subscribed to by the user.
        /// </summary>

        SubscriptionVertices = 2,

        /// <summary>
        /// Include all statistics for each user.
        /// </summary>

        AllStatistics = 4,
    }

    //*************************************************************************
    //  Method: GetNetworkAsync()
    //
    /// <summary>
    /// Asynchronously gets a directed network of YouTube users.
    /// </summary>
    ///
    /// <param name="userNameToAnalyze">
    /// The user name of the YouTube user whose network should be analyzed.
    /// </param>
    ///
    /// <param name="whatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="networkLevel">
    /// Network level to include.
    /// </param>
    ///
    /// <param name="maximumPeoplePerRequest">
    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.
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
    GetNetworkAsync
    (
        String userNameToAnalyze,
        WhatToInclude whatToInclude,
        NetworkLevel networkLevel,
        Int32 maximumPeoplePerRequest
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(userNameToAnalyze) );

        Debug.Assert(networkLevel == NetworkLevel.One ||
            networkLevel == NetworkLevel.OnePointFive ||
            networkLevel == NetworkLevel.Two);

        Debug.Assert(maximumPeoplePerRequest > 0);

        AssertValid();

        const String MethodName = "GetNetworkAsync";
        CheckIsBusy(MethodName);

        // Wrap the arguments in an object that can be passed to
        // BackgroundWorker.RunWorkerAsync().

        GetNetworkAsyncArgs oGetNetworkAsyncArgs = new GetNetworkAsyncArgs();

        oGetNetworkAsyncArgs.UserNameToAnalyze = userNameToAnalyze;
        oGetNetworkAsyncArgs.WhatToInclude = whatToInclude;
        oGetNetworkAsyncArgs.NetworkLevel = networkLevel;
        oGetNetworkAsyncArgs.MaximumPeoplePerRequest = maximumPeoplePerRequest;

        m_oBackgroundWorker.RunWorkerAsync(oGetNetworkAsyncArgs);
    }

    //*************************************************************************
    //  Method: TryGetUserNetworkInternal()
    //
    /// <overloads>
    /// Attempts to get a network of YouTube users.
    /// </overloads>
    ///
    /// <summary>
    /// Attempts to get a network of YouTube users.
    /// </summary>
    ///
    /// <param name="sUserNameToAnalyze">
    /// The user name of the YouTube user whose network should be analyzed.
    /// </param>
    ///
    /// <param name="eWhatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="eNetworkLevel">
    /// Network level to include.  Must be NetworkLevel.One, OnePointFive, or
    /// Two.
    /// </param>
    ///
    /// <param name="iMaximumPeoplePerRequest">
    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.
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
        String sUserNameToAnalyze,
        WhatToInclude eWhatToInclude,
        NetworkLevel eNetworkLevel,
        Int32 iMaximumPeoplePerRequest,
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs,
        out XmlDocument oXmlDocument
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUserNameToAnalyze) );

        Debug.Assert(eNetworkLevel == NetworkLevel.One ||
            eNetworkLevel == NetworkLevel.OnePointFive ||
            eNetworkLevel == NetworkLevel.Two);

        Debug.Assert(iMaximumPeoplePerRequest > 0);

        Debug.Assert(oBackgroundWorker == null || oDoWorkEventArgs != null);
        AssertValid();

        oXmlDocument = null;

        GraphMLXmlDocument oGraphMLXmlDocument =
            CreateGraphMLXmlDocument( WhatToIncludeFlagIsSet(
                eWhatToInclude, WhatToInclude.AllStatistics) );

        RequestStatistics oRequestStatistics = new RequestStatistics();

        try
        {
            if ( !TryGetUserNetworkInternal(sUserNameToAnalyze,
                eWhatToInclude, eNetworkLevel, iMaximumPeoplePerRequest,
                oRequestStatistics, oBackgroundWorker, oDoWorkEventArgs,
                oGraphMLXmlDocument) )
            {
                // The user cancelled.

                return (false);
            }
        }
        catch (Exception oException)
        {
            OnTerminatingException(oException);
        }

        return ( OnNetworkObtainedWithoutTerminatingException(
            oGraphMLXmlDocument, oRequestStatistics, out oXmlDocument) );
    }

    //*************************************************************************
    //  Method: TryGetUserNetworkInternal()
    //
    /// <summary>
    /// Attempts to get a network of YouTube users, given a GraphMLXmlDocument.
    /// </summary>
    ///
    /// <param name="sUserNameToAnalyze">
    /// The user name of the YouTube user whose network should be analyzed.
    /// </param>
    ///
    /// <param name="eWhatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="eNetworkLevel">
    /// Network level to include.  Must be NetworkLevel.One, OnePointFive, or
    /// Two.
    /// </param>
    ///
    /// <param name="iMaximumPeoplePerRequest">
    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
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
    /// <param name="oGraphMLXmlDocument">
    /// The GraphMLXmlDocument to populate with the requested network.
    /// </param>
    ///
    /// <returns>
    /// true if the network was obtained, or false if the user cancelled.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetUserNetworkInternal
    (
        String sUserNameToAnalyze,
        WhatToInclude eWhatToInclude,
        NetworkLevel eNetworkLevel,
        Int32 iMaximumPeoplePerRequest,
        RequestStatistics oRequestStatistics,
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs,
        GraphMLXmlDocument oGraphMLXmlDocument
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUserNameToAnalyze) );

        Debug.Assert(eNetworkLevel == NetworkLevel.One ||
            eNetworkLevel == NetworkLevel.OnePointFive ||
            eNetworkLevel == NetworkLevel.Two);

        Debug.Assert(iMaximumPeoplePerRequest > 0);
        Debug.Assert(oRequestStatistics != null);
        Debug.Assert(oBackgroundWorker == null || oDoWorkEventArgs != null);
        Debug.Assert(oGraphMLXmlDocument != null);
        AssertValid();

        // The key is the user name and the value is the corresponding GraphML
        // XML node that represents the user.  This is used to prevent the same
        // user name from being added to the XmlDocument twice.

        Dictionary<String, XmlNode> oUserNameDictionary =
            new Dictionary<String, XmlNode>();

        // Include friends, subscriptions, both, or neither.

        Boolean [] abIncludes = new Boolean[] {

            WhatToIncludeFlagIsSet(eWhatToInclude,
                WhatToInclude.FriendVertices),

            WhatToIncludeFlagIsSet(eWhatToInclude,
                WhatToInclude.SubscriptionVertices),
            };

        for (Int32 i = 0; i < 2; i++)
        {
            if ( abIncludes[i] )
            {
                if ( !TryGetUserNetworkRecursive(sUserNameToAnalyze,
                    eWhatToInclude, (i == 0), eNetworkLevel,
                    iMaximumPeoplePerRequest, 1, oGraphMLXmlDocument,
                    oUserNameDictionary, oRequestStatistics, oBackgroundWorker,
                    oDoWorkEventArgs) )
                {
                    return (false);
                }
            }
        }

        if ( WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.AllStatistics) )
        {
            if ( !AppendAllStatisticGraphMLAttributeValues(oGraphMLXmlDocument,
                oUserNameDictionary, oRequestStatistics, oBackgroundWorker,
                oDoWorkEventArgs) )
            {
                return (false);
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryGetUserNetworkRecursive()
    //
    /// <summary>
    /// Attempts to recursively get a network of YouTube users.
    /// </summary>
    ///
    /// <param name="sUserName">
    /// The user name to use in this call.
    /// </param>
    ///
    /// <param name="eWhatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="bIncludeFriendsThisCall">
    /// true to include the user's friends, false to include the people
    /// subscribed to by the user.
    /// </param>
    ///
    /// <param name="eNetworkLevel">
    /// Network level to include.  Must be NetworkLevel.One, OnePointFive, or
    /// Two.
    /// </param>
    ///
    /// <param name="iMaximumPeoplePerRequest">
    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.
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
    /// <param name="oUserNameDictionary">
    /// The key is the user name and the value is the corresponding GraphML XML
    /// node that represents the user.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
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
        String sUserName,
        WhatToInclude eWhatToInclude,
        Boolean bIncludeFriendsThisCall,
        NetworkLevel eNetworkLevel,
        Int32 iMaximumPeoplePerRequest,
        Int32 iRecursionLevel,
        GraphMLXmlDocument oGraphMLXmlDocument,
        Dictionary<String, XmlNode> oUserNameDictionary,
        RequestStatistics oRequestStatistics,
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUserName) );

        Debug.Assert(eNetworkLevel == NetworkLevel.One ||
            eNetworkLevel == NetworkLevel.OnePointFive ||
            eNetworkLevel == NetworkLevel.Two);

        Debug.Assert(iMaximumPeoplePerRequest > 0);
        Debug.Assert(iRecursionLevel == 1 || iRecursionLevel == 2);
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oUserNameDictionary != null);
        Debug.Assert(oRequestStatistics != null);
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

        Boolean bNeedToRecurse = GetNeedToRecurse(eNetworkLevel,
            iRecursionLevel);

        List<String> oUserNamesToRecurse = new List<String>();

        ReportProgressForFriendsOrSubscriptions(sUserName,
            bIncludeFriendsThisCall);

        Boolean bThisUserAppended = false;

        // If the GraphMLXmlDocument already contains at least one vertex node,
        // then this is the second time that this method has been called, a
        // partial network has already been obtained, and most errors should
        // now be skipped.  However, if none of the network has been obtained
        // yet, errors on page 1 should throw an immediate exception.

        Boolean bSkipMostPage1Errors = oGraphMLXmlDocument.HasVertexXmlNode;

        // The document consists of a single "feed" node with zero or more
        // "entry" child nodes.

        foreach ( XmlNode oEntryXmlNode in EnumerateXmlNodes(
            GetFriendsOrSubscriptionsUrl(sUserName, bIncludeFriendsThisCall),
            "a:feed/a:entry", iMaximumPeoplePerRequest, bSkipMostPage1Errors,
            oRequestStatistics) )
        {
            if ( CancelIfRequested(oBackgroundWorker, oDoWorkEventArgs) )
            {
                return (false);
            }

            XmlNamespaceManager oXmlNamespaceManager =
                CreateXmlNamespaceManager(oEntryXmlNode.OwnerDocument);

            String sOtherUserName;

            if ( !XmlUtil2.SelectSingleNode(oEntryXmlNode, "yt:username/text()",
                oXmlNamespaceManager, false, out sOtherUserName) )
            {
                continue;
            }

            if (!bThisUserAppended)
            {
                // Append a vertex node for this request's user.
                //
                // This used to be done after the foreach loop, which avoided
                // the need for a "bThisUserAppended" flag.  That caused the
                // following bug: If a YouTube error occurred within
                // EnumerateXmlNodes() after some edges had been added, and the
                // user decided to import the resulting partial network, the
                // GraphML might contain edges that referenced "this user"
                // without containing a vertex for "this user."  That is an
                // illegal state for GraphML, which the ExcelTemplate project
                // caught and reported as an error.

                TryAppendVertexXmlNode(sUserName, null, oGraphMLXmlDocument,
                    oUserNameDictionary);

                bThisUserAppended = true;
            }

            Boolean bNeedToAppendVertices = GetNeedToAppendVertices(
                eNetworkLevel, iRecursionLevel);

            if (bNeedToAppendVertices)
            {
                if (
                    TryAppendVertexXmlNode(sOtherUserName, oEntryXmlNode,
                        oGraphMLXmlDocument, oUserNameDictionary)
                    &&
                    bNeedToRecurse
                    )
                {
                    oUserNamesToRecurse.Add(sOtherUserName);
                }
            }

            if ( bNeedToAppendVertices ||
                oUserNameDictionary.ContainsKey(sOtherUserName) )
            {
                String sRelationship;

                if (bIncludeFriendsThisCall)
                {
                    sRelationship = "Friend of";
                }
                else
                {
                    sRelationship = "Subscribes to";
                    String sSubscriptionType = null;

                    if ( XmlUtil2.SelectSingleNode(oEntryXmlNode,

                        "a:category[@scheme='http://gdata.youtube.com/schemas/"
                        + "2007/subscriptiontypes.cat']/@term",

                        oXmlNamespaceManager, false, out sSubscriptionType) )
                    {
                        sRelationship += " " + sSubscriptionType;
                    }
                }

                AppendEdgeXmlNode(oGraphMLXmlDocument, sUserName,
                    sOtherUserName, sRelationship);
            }
        }

        if (bNeedToRecurse)
        {
            foreach (String sUserNameToRecurse in oUserNamesToRecurse)
            {
                if ( !TryGetUserNetworkRecursive(sUserNameToRecurse,
                    eWhatToInclude, bIncludeFriendsThisCall, eNetworkLevel,
                    iMaximumPeoplePerRequest, 2, oGraphMLXmlDocument,
                    oUserNameDictionary, oRequestStatistics, oBackgroundWorker,
                    oDoWorkEventArgs) )
                {
                    return (false);
                }
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: CreateGraphMLXmlDocument()
    //
    /// <summary>
    /// Creates a GraphMLXmlDocument representing a network of YouTube users.
    /// </summary>
    ///
    /// <param name="bIncludeAllStatistics">
    /// true to include each user's statistics.
    /// </param>
    ///
    /// <returns>
    /// A GraphMLXmlDocument representing a network of YouTube users.  The
    /// document includes GraphML-attribute definitions but no vertices or
    /// edges.
    /// </returns>
    //*************************************************************************

    protected GraphMLXmlDocument
    CreateGraphMLXmlDocument
    (
        Boolean bIncludeAllStatistics
    )
    {
        AssertValid();

        GraphMLXmlDocument oGraphMLXmlDocument = new GraphMLXmlDocument(true);

        DefineCustomMenuGraphMLAttributes(oGraphMLXmlDocument);
        DefineRelationshipGraphMLAttribute(oGraphMLXmlDocument);

        if (bIncludeAllStatistics)
        {
            oGraphMLXmlDocument.DefineGraphMLAttribute(false, FriendsID,
                "Friends", "int", null);

            oGraphMLXmlDocument.DefineGraphMLAttribute(false, SubscriptionsID,
                "People Subscribed To", "int", null);

            oGraphMLXmlDocument.DefineGraphMLAttribute(false, SubscribersID,
                "Subscribers", "int", null);

            oGraphMLXmlDocument.DefineGraphMLAttribute(false, VideosWatchedID,
                "Videos Watched", "int", null);

            oGraphMLXmlDocument.DefineGraphMLAttribute(false,
                JoinedDateID, "Joined YouTube Date", "string", null);

            oGraphMLXmlDocument.DefineGraphMLAttribute(false, VideosUploadedID,
                "Videos Uploaded", "int", null);

            DefineImageFileGraphMLAttribute(oGraphMLXmlDocument);
        }

        return (oGraphMLXmlDocument);
    }

    //*************************************************************************
    //  Method: WhatToIncludeFlagIsSet()
    //
    /// <summary>
    /// Checks whether a flag is set in an ORed combination of WhatToInclude
    /// flags.
    /// </summary>
    ///
    /// <param name="eORedEnumFlags">
    /// Zero or more ORed Enum flags.
    /// </param>
    ///
    /// <param name="eORedEnumFlagsToCheck">
    /// One or more Enum flags to check.
    /// </param>
    ///
    /// <returns>
    /// true if any of the <paramref name="eORedEnumFlagsToCheck" /> flags are
    /// set in <paramref name="eORedEnumFlags" />.
    /// </returns>
    //*************************************************************************

    protected Boolean
    WhatToIncludeFlagIsSet
    (
        WhatToInclude eORedEnumFlags,
        WhatToInclude eORedEnumFlagsToCheck
    )
    {
        return ( (eORedEnumFlags & eORedEnumFlagsToCheck) != 0 );
    }

    //*************************************************************************
    //  Method: GetFriendsOrSubscriptionsUrl()
    //
    /// <summary>
    /// Gets the YouTube API URL for a user's friends or subscriptions.
    /// </summary>
    ///
    /// <param name="sUserName">
    /// The user name.
    /// </param>
    ///
    /// <param name="bFriends">
    /// true to get the URL for the user's friends, false to get the URL for
    /// the user's subscriptions.
    /// </param>
    ///
    /// <returns>
    /// The URL for the user's friends or subscriptions.  Does not include any
    /// URL parameters.
    /// </returns>
    //*************************************************************************

    protected String
    GetFriendsOrSubscriptionsUrl
    (
        String sUserName,
        Boolean bFriends
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUserName) );
        AssertValid();

        return ( String.Format(

            "http://gdata.youtube.com/feeds/api/users/{0}/{1}"
            ,
            EncodeUrlParameter(sUserName),
            bFriends ? "contacts" : "subscriptions"
            ) );
    }

    //*************************************************************************
    //  Method: ReportProgressForFriendsOrSubscriptions()
    //
    /// <summary>
    /// Reports progress before getting a user's friends or subscriptions.
    /// </summary>
    ///
    /// <param name="sUserName">
    /// The user name.
    /// </param>
    ///
    /// <param name="bFriends">
    /// true if getting the user's friends, false if getting the user's
    /// subscriptions.
    /// </param>
    //*************************************************************************

    protected void
    ReportProgressForFriendsOrSubscriptions
    (
        String sUserName,
        Boolean bFriends
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUserName) );
        AssertValid();

        ReportProgress( String.Format(

            "Getting {0} \"{1}\""
            ,
            bFriends ? "friends of" : "people subscribed to by",
            sUserName
            ) );
    }

    //*************************************************************************
    //  Method: TryAppendVertexXmlNode()
    //
    /// <summary>
    /// Appends a vertex XML node to the GraphML document for a user if such a
    /// node doesn't already exist.
    /// </summary>
    ///
    /// <param name="sUserName">
    /// User name to add a vertex XML node for.
    /// </param>
    ///
    /// <param name="oEntryXmlNode">
    /// The "entry" XML node returned by YouTube, or null if an entry node
    /// isn't available.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oUserNameDictionary">
    /// The key is the user name and the value is the corresponding GraphML XML
    /// node that represents the user.
    /// </param>
    ///
    /// <returns>
    /// true if a vertex XML node was added, false if a vertex XML node already
    /// exists.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryAppendVertexXmlNode
    (
        String sUserName,
        XmlNode oEntryXmlNode,
        GraphMLXmlDocument oGraphMLXmlDocument,
        Dictionary<String, XmlNode> oUserNameDictionary
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUserName) );
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oUserNameDictionary != null);

        XmlNode oVertexXmlNode;

        if ( oUserNameDictionary.TryGetValue(sUserName, out oVertexXmlNode) )
        {
            return (false);
        }

        oVertexXmlNode = oGraphMLXmlDocument.AppendVertexXmlNode(sUserName);
        oUserNameDictionary.Add(sUserName, oVertexXmlNode);

        oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode,
            MenuTextID, "Open YouTube Page for This Person");

        oGraphMLXmlDocument.AppendGraphMLAttributeValue( oVertexXmlNode,
            MenuActionID, String.Format(WebPageUrlPattern, sUserName) );

        return (true);
    }

    //*************************************************************************
    //  Method: AppendAllStatisticGraphMLAttributeValues()
    //
    /// <overloads>
    /// Appends statistic GraphML attribute values to the GraphML document.
    /// </overloads>
    ///
    /// <summary>
    /// Appends statistic GraphML attribute values to the GraphML document for
    /// all users in the network.
    /// </summary>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oUserNameDictionary">
    /// The key is the user name and the value is the corresponding GraphML XML
    /// node that represents the user.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
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
    /// true if the statistics were appended, or false if the user cancelled.
    /// </returns>
    //*************************************************************************

    protected Boolean
    AppendAllStatisticGraphMLAttributeValues
    (
        GraphMLXmlDocument oGraphMLXmlDocument,
        Dictionary<String, XmlNode> oUserNameDictionary,
        RequestStatistics oRequestStatistics,
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs
    )
    {
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oUserNameDictionary != null);
        Debug.Assert(oRequestStatistics != null);
        Debug.Assert(oBackgroundWorker == null || oDoWorkEventArgs != null);
        AssertValid();

        foreach (KeyValuePair<String, XmlNode> oKeyValuePair in
            oUserNameDictionary)
        {
            if ( CancelIfRequested(oBackgroundWorker, oDoWorkEventArgs) )
            {
                return (false);
            }

            String sUserName = oKeyValuePair.Key;

            ReportProgress( String.Format(

                "Getting statistics for \"{0}\""
                ,
                sUserName
                ) );

            AppendAllStatisticGraphMLAttributeValues(sUserName,
                oKeyValuePair.Value, oGraphMLXmlDocument, oRequestStatistics);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: AppendAllStatisticGraphMLAttributeValues()
    //
    /// <summary>
    /// Appends statistic GraphML attribute values to the GraphML document for
    /// one user in the network.
    /// </summary>
    ///
    /// <param name="sUserName">
    /// The user name.
    /// </param>
    ///
    /// <param name="oVertexXmlNode">
    /// The GraphML XML node corresponding to the user.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    //*************************************************************************

    protected void
    AppendAllStatisticGraphMLAttributeValues
    (
        String sUserName,
        XmlNode oVertexXmlNode,
        GraphMLXmlDocument oGraphMLXmlDocument,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUserName) );
        Debug.Assert(oVertexXmlNode != null);
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        XmlDocument oXmlDocument;
        XmlNamespaceManager oXmlNamespaceManager;

        // Some of the statistics are available in the user's profile.

        String sUrl = "http://gdata.youtube.com/feeds/api/users/"
            + EncodeUrlParameter(sUserName);

        if ( TryGetXmlDocument(sUrl, oRequestStatistics, out oXmlDocument,
            out oXmlNamespaceManager) )
        {
            AppendYouTubeDateGraphMLAttributeValue(oXmlDocument,
                "a:entry/a:published/text()", oXmlNamespaceManager,
                oGraphMLXmlDocument, oVertexXmlNode, JoinedDateID);

            AppendStringGraphMLAttributeValue(oXmlDocument,
                "a:entry/media:thumbnail/@url", oXmlNamespaceManager,
                oGraphMLXmlDocument, oVertexXmlNode, ImageFileID);

            XmlNode oStatisticsXmlNode;

            if ( XmlUtil2.SelectSingleNode(oXmlDocument,
                "a:entry/yt:statistics", oXmlNamespaceManager, false,
                out oStatisticsXmlNode) )
            {
                AppendInt32GraphMLAttributeValue(oStatisticsXmlNode,
                    "@videoWatchCount", oXmlNamespaceManager,
                    oGraphMLXmlDocument, oVertexXmlNode, VideosWatchedID);

                AppendInt32GraphMLAttributeValue(oStatisticsXmlNode,
                    "@subscriberCount", oXmlNamespaceManager,
                    oGraphMLXmlDocument, oVertexXmlNode, SubscribersID);
            }
        }

        // The remaining statistics must be obtained from the totalResults
        // XML node within documents obtained from other URLs.

        const String TotalResultsXPath =
            "a:feed/openSearch:totalResults/text()";

        sUrl = GetFriendsOrSubscriptionsUrl(sUserName, true)
            + "?max-results=1";

        AppendInt32GraphMLAttributeValue(sUrl, TotalResultsXPath,
            oGraphMLXmlDocument, oRequestStatistics, oVertexXmlNode,
            FriendsID);

        sUrl = GetFriendsOrSubscriptionsUrl(sUserName, false)
            + "?max-results=1";

        AppendInt32GraphMLAttributeValue(sUrl, TotalResultsXPath,
            oGraphMLXmlDocument, oRequestStatistics, oVertexXmlNode,
            SubscriptionsID);

        sUrl = "http://gdata.youtube.com/feeds/api/users/"
            + EncodeUrlParameter(sUserName) + "/uploads?max-results=1";

        AppendInt32GraphMLAttributeValue(sUrl, TotalResultsXPath,
            oGraphMLXmlDocument, oRequestStatistics, oVertexXmlNode,
            VideosUploadedID);
    }

    //*************************************************************************
    //  Method: AppendInt32GraphMLAttributeValue()
    //
    /// <summary>
    /// Appends an Int32 GraphML-Attribute value from an XmlDocument to a
    /// vertex XML node. 
    /// </summary>
    ///
    /// <param name="sUrl">
    /// The URL to get the XmlDocument from.
    /// </param>
    /// 
    /// <param name="sXPath">
    /// Path to the value within the XmlDocument.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
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
    /// This method looks for an Int32 value within an XmlDocument returned by
    /// YouTube and stores the value on a vertex XML node in the GraphML
    /// document.
    /// </remarks>
    //*************************************************************************

    protected void
    AppendInt32GraphMLAttributeValue
    (
        String sUrl,
        String sXPath,
        GraphMLXmlDocument oGraphMLXmlDocument,
        RequestStatistics oRequestStatistics,
        XmlNode oVertexXmlNode,
        String sGraphMLAttributeID
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUrl) );
        Debug.Assert( !String.IsNullOrEmpty(sXPath) );
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oRequestStatistics != null);
        Debug.Assert(oVertexXmlNode != null);
        Debug.Assert( !String.IsNullOrEmpty(sGraphMLAttributeID) );
        AssertValid();

        XmlDocument oXmlDocument;
        XmlNamespaceManager oXmlNamespaceManager;

        if ( !TryGetXmlDocument(sUrl, oRequestStatistics, out oXmlDocument,
            out oXmlNamespaceManager) )
        {
            return;
        }

        AppendInt32GraphMLAttributeValue(oXmlDocument, sXPath,
            oXmlNamespaceManager, oGraphMLXmlDocument, oVertexXmlNode,
            sGraphMLAttributeID);
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

        Debug.Assert(e.Argument is GetNetworkAsyncArgs);

        GetNetworkAsyncArgs oGetNetworkAsyncArgs =
            (GetNetworkAsyncArgs)e.Argument;

        XmlDocument oGraphMLDocument;
        
        if ( TryGetUserNetworkInternal(
            oGetNetworkAsyncArgs.UserNameToAnalyze,
            oGetNetworkAsyncArgs.WhatToInclude,
            oGetNetworkAsyncArgs.NetworkLevel,
            oGetNetworkAsyncArgs.MaximumPeoplePerRequest,
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
    //  Protected fields
    //*************************************************************************

    /// GraphML-attribute IDs.

    protected const String SubscribersID = "Subscribers";
    ///
    protected const String VideosWatchedID = "VideosWatched";
    ///
    protected const String JoinedDateID = "JoinedDate";
    ///
    protected const String FriendsID = "Friends";
    ///
    protected const String SubscriptionsID = "Subscriptions";
    ///
    protected const String VideosUploadedID = "VideosUploaded";


    /// Format pattern for the URL of the YouTube Web page for a person.  The
    /// {0} argument must be replaced with a YouTube user name.

    protected const String WebPageUrlPattern =
        "http://www.youtube.com/user/{0}";


    //*************************************************************************
    //  Embedded class: GetNetworkAsyncArgs()
    //
    /// <summary>
    /// Contains the arguments needed to asynchronously get a network of
    /// YouTube users.
    /// </summary>
    //*************************************************************************

    protected class GetNetworkAsyncArgs : Object
    {
        ///
        public String UserNameToAnalyze;
        ///
        public WhatToInclude WhatToInclude;
        ///
        public NetworkLevel NetworkLevel;
        ///
        public Int32 MaximumPeoplePerRequest;
    };
}

}
