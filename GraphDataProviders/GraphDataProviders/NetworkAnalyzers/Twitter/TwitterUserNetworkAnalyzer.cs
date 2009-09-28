
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Xml;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.XmlLib;
using Microsoft.SocialNetworkLib;

namespace Microsoft.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterUserNetworkAnalyzer
//
/// <summary>
/// Analyzes a network of Twitter users.
/// </summary>
///
/// <remarks>
/// Use <see cref="GetUserNetwork" /> to synchronously get the network of
/// people followed by a Twitter user or people whom a Twitter user follows, or
/// use <see cref="GetUserNetworkAsync" /> to do it asynchronously.
/// </remarks>
//*****************************************************************************

public class TwitterUserNetworkAnalyzer : TwitterNetworkAnalyzerBase
{
    //*************************************************************************
    //  Constructor: TwitterUserNetworkAnalyzer()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterUserNetworkAnalyzer" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterUserNetworkAnalyzer()
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
        /// Include a vertex for each person followed by the user.
        /// </summary>

        FollowedVertices = 1,

        /// <summary>
        /// Include a vertex for each person following the user.
        /// </summary>

        FollowerVertices = 2,

        /// <summary>
        /// Include each person's latest status.
        /// </summary>

        LatestStatuses = 4,

        /// <summary>
        /// Include an edge for each followed relationship if FollowedVertices
        /// is specified, and include an edge for each follower relationship if
        /// FollowerVertices is specified,
        /// </summary>

        FollowedFollowerEdges = 8,

        /// <summary>
        /// Include an edge from person A to person B if person A's latest
        /// tweet is a reply to person B.
        /// </summary>

        RepliesToEdges = 16,

        /// <summary>
        /// Include an edge from person A to person B if person A's latest
        /// tweet mentions person B.
        /// </summary>

        MentionsEdges = 32,
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
    /// <param name="whatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="networkLevel">
    /// Network level to include.  Must be NetworkLevel.One, OnePointFive, or
    /// Two.
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
        WhatToInclude whatToInclude,
        NetworkLevel networkLevel,
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
            whatToInclude, networkLevel, maximumPeoplePerRequest,
            credentialsScreenName, credentialsPassword, null, null,
            out oXmlDocument);

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
        WhatToInclude whatToInclude,
        NetworkLevel networkLevel,
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
        oGetUserNetworkAsyncArgs.WhatToInclude = whatToInclude;
        oGetUserNetworkAsyncArgs.NetworkLevel = networkLevel;

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
        WhatToInclude eWhatToInclude,
        NetworkLevel eNetworkLevel,
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

        Boolean bIncludeLatestStatus = WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.LatestStatuses);

        GraphMLXmlDocument oGraphMLXmlDocument =
            CreateGraphMLXmlDocument(true, bIncludeLatestStatus);

        // The key is the screen name and the value is the corresponding
        // TwitterVertex.  This is used to prevent the same screen name from
        // being added to the XmlDocument twice.

        Dictionary<String, TwitterVertex> oScreenNameDictionary =
            new Dictionary<String, TwitterVertex>();

        // Include followed, followers, both, or neither.

        Boolean [] abIncludes = new Boolean[] {

            WhatToIncludeFlagIsSet(eWhatToInclude,
                WhatToInclude.FollowedVertices),

            WhatToIncludeFlagIsSet(eWhatToInclude,
                WhatToInclude.FollowerVertices),
            };

        for (Int32 i = 0; i < 2; i++)
        {
            if ( abIncludes[i] )
            {
                if ( !TryGetUserNetworkRecursive(sScreenNameToAnalyze,
                    eWhatToInclude, (i == 0), eNetworkLevel,
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
            oScreenNameDictionary, true, bIncludeLatestStatus,
            sCredentialsScreenName, sCredentialsPassword, oBackgroundWorker,
            oDoWorkEventArgs);

        AppendRepliesToAndMentionsXmlNodes(oGraphMLXmlDocument,
            oScreenNameDictionary,

            WhatToIncludeFlagIsSet(eWhatToInclude,
                WhatToInclude.RepliesToEdges),

            WhatToIncludeFlagIsSet(eWhatToInclude,
                WhatToInclude.MentionsEdges)
            );

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
    /// <param name="eWhatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="bIncludeFollowedThisCall">
    /// true to include the people followed by the user, false to include the
    /// people following the user.
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
    /// The key is the screen name and the value is the corresponding
    /// TwitterVertex.
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
        WhatToInclude eWhatToInclude,
        Boolean bIncludeFollowedThisCall,
        NetworkLevel eNetworkLevel,
        Int32 iMaximumPeoplePerRequest,
        String sCredentialsScreenName,
        String sCredentialsPassword,
        Int32 iRecursionLevel,
        GraphMLXmlDocument oGraphMLXmlDocument,
        Dictionary<String, TwitterVertex> oScreenNameDictionary,
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenName) );

        Debug.Assert(eNetworkLevel == NetworkLevel.One ||
            eNetworkLevel == NetworkLevel.OnePointFive ||
            eNetworkLevel == NetworkLevel.Two);

        Debug.Assert(iMaximumPeoplePerRequest > 0);

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
        iRecursionLevel parameters.  It's assumed that
        eWhatToInclude.FollowedFollowerEdge is set.

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

        // The document consists of a single "users" node with zero or more
        // "user" child nodes.

        Boolean bUserNodeExists = false;
        List<String> oScreenNamesToRecurse = new List<String>();

        Boolean bIncludeLatestStatuses = WhatToIncludeFlagIsSet(
            eWhatToInclude, WhatToInclude.LatestStatuses);

        ReportProgressForFollowedOrFollowing(sScreenName,
            bIncludeFollowedThisCall);

        // Note that "unauthorized" and "not found" errors are skipped when
        // iRecursionLevel == 2.

        foreach ( XmlNode oUserXmlNode in EnumerateXmlNodes(
            GetFollowedOrFollowingUrl(sScreenName, bIncludeFollowedThisCall),
            "users/user", null, null, 100, Int32.MaxValue,
            iMaximumPeoplePerRequest, iRecursionLevel != 1,
            sCredentialsScreenName, sCredentialsPassword) )
        {
            if ( CancelIfRequested(oBackgroundWorker, oDoWorkEventArgs) )
            {
                return (false);
            }

            String sOtherScreenName;

            if ( !TryGetScreenName(oUserXmlNode, out sOtherScreenName) )
            {
                // Nothing can be done without a screen name.

                continue;
            }

            bUserNodeExists = true;

            if (eNetworkLevel != NetworkLevel.OnePointFive ||
                iRecursionLevel == 1)
            {
                TryAppendVertexXmlNode(sOtherScreenName, oUserXmlNode,
                    oGraphMLXmlDocument, oScreenNameDictionary,
                    true, bIncludeLatestStatuses);
            }

            if (bNeedToRecurse)
            {
                oScreenNamesToRecurse.Add(sOtherScreenName);
            }

            if ( WhatToIncludeFlagIsSet(eWhatToInclude,
                WhatToInclude.FollowedFollowerEdges) )
            {
                if ( eNetworkLevel != NetworkLevel.OnePointFive ||
                    iRecursionLevel == 1 ||
                    oScreenNameDictionary.ContainsKey(sOtherScreenName) )
                {
                    XmlNode oEdgeXmlNode;

                    if (bIncludeFollowedThisCall)
                    {
                        oEdgeXmlNode = AppendEdgeXmlNode(oGraphMLXmlDocument,
                            sScreenName, sOtherScreenName, "Followed");
                    }
                    else
                    {
                        oEdgeXmlNode = AppendEdgeXmlNode(oGraphMLXmlDocument,
                            sOtherScreenName, sScreenName, "Follower");
                    }
                }
            }
        }

        if (bUserNodeExists)
        {
            TryAppendVertexXmlNode(sScreenName, null, oGraphMLXmlDocument,
                oScreenNameDictionary, true, bIncludeLatestStatuses);
        }

        if (bNeedToRecurse)
        {
            foreach (String sScreenNameToRecurse in oScreenNamesToRecurse)
            {
                if ( !TryGetUserNetworkRecursive(sScreenNameToRecurse,
                    eWhatToInclude, bIncludeFollowedThisCall, eNetworkLevel,
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
            oGetUserNetworkAsyncArgs.WhatToInclude,
            oGetUserNetworkAsyncArgs.NetworkLevel,
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

    protected class GetUserNetworkAsyncArgs : GetNetworkAsyncArgs
    {
        ///
        public String ScreenNameToAnalyze;
        ///
        public WhatToInclude WhatToInclude;
        ///
        public NetworkLevel NetworkLevel;
    };
}

}
