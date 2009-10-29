
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Xml;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.XmlLib;
using Microsoft.SocialNetworkLib;

namespace Microsoft.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterSearchNetworkAnalyzer
//
/// <summary>
/// Gets the network of people who have tweeted a specified search term.
/// </summary>
///
/// <remarks>
/// Use <see cref="GetSearchNetwork" /> to synchronously get the network of
/// people who have tweeted a specified search term, or use <see
/// cref="GetSearchNetworkAsync" /> to do it asynchronously.
/// </remarks>
//*****************************************************************************

public class TwitterSearchNetworkAnalyzer : TwitterNetworkAnalyzerBase
{
    //*************************************************************************
    //  Constructor: TwitterSearchNetworkAnalyzer()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterSearchNetworkAnalyzer" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterSearchNetworkAnalyzer()
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
        /// Include each person's status.
        /// </summary>

        Statuses = 1,

        /// <summary>
        /// Include each person's statistics.
        /// </summary>

        Statistics = 2,

        /// <summary>
        /// Include an edge for each followed relationship.
        /// </summary>

        FollowedEdges = 4,

        /// <summary>
        /// Include an edge from person A to person B if person A's tweet is a
        /// reply to person B.
        /// </summary>

        RepliesToEdges = 8,

        /// <summary>
        /// Include an edge from person A to person B if person A's tweet
        /// mentions person B.
        /// </summary>

        MentionsEdges = 16,
    }

    //*************************************************************************
    //  Method: GetSearchNetwork()
    //
    /// <summary>
    /// Synchronously gets the network of people who have tweeted a specified
    /// search term.
    /// </summary>
    ///
    /// <param name="searchTerm">
    /// The term to search for.
    /// </param>
    ///
    /// <param name="whatToInclude">
    /// Specifies what should be included in the network.
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
    GetSearchNetwork
    (
        String searchTerm,
        WhatToInclude whatToInclude,
        Int32 maximumPeoplePerRequest,
        String credentialsScreenName,
        String credentialsPassword
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(searchTerm) );
        Debug.Assert(maximumPeoplePerRequest > 0);

        Debug.Assert( credentialsScreenName == null ||
            ( credentialsScreenName.Length > 0 &&
                !String.IsNullOrEmpty(credentialsPassword) ) );

        AssertValid();

        XmlDocument oXmlDocument;

        Boolean bNotCancelled = TryGetSearchNetworkInternal(searchTerm,
            whatToInclude, maximumPeoplePerRequest, credentialsScreenName,
            credentialsPassword, null, null, out oXmlDocument);

        Debug.Assert(bNotCancelled);

        return (oXmlDocument);
    }

    //*************************************************************************
    //  Method: GetSearchNetworkAsync()
    //
    /// <summary>
    /// Asynchronously gets the network of people who have tweeted a specified
    /// search term.
    /// </summary>
    ///
    /// <param name="searchTerm">
    /// The term to search for.
    /// </param>
    ///
    /// <param name="whatToInclude">
    /// Specifies what should be included in the network.
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
    GetSearchNetworkAsync
    (
        String searchTerm,
        WhatToInclude whatToInclude,
        Int32 maximumPeoplePerRequest,
        String credentialsScreenName,
        String credentialsPassword
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(searchTerm) );
        Debug.Assert(maximumPeoplePerRequest > 0);

        Debug.Assert( credentialsScreenName == null ||
            ( credentialsScreenName.Length > 0 &&
                !String.IsNullOrEmpty(credentialsPassword) ) );

        AssertValid();

        const String MethodName = "GetSearchNetworkAsync";
        CheckIsBusy(MethodName);

        // Wrap the arguments in an object that can be passed to
        // BackgroundWorker.RunWorkerAsync().

        GetSearchNetworkAsyncArgs oGetSearchNetworkAsyncArgs =
            new GetSearchNetworkAsyncArgs();

        oGetSearchNetworkAsyncArgs.SearchTerm = searchTerm;
        oGetSearchNetworkAsyncArgs.WhatToInclude = whatToInclude;

        oGetSearchNetworkAsyncArgs.MaximumPeoplePerRequest =
            maximumPeoplePerRequest;

        oGetSearchNetworkAsyncArgs.CredentialsScreenName =
            credentialsScreenName;

        oGetSearchNetworkAsyncArgs.CredentialsPassword = credentialsPassword;

        m_oBackgroundWorker.RunWorkerAsync(oGetSearchNetworkAsyncArgs);
    }

    //*************************************************************************
    //  Method: TryGetSearchNetworkInternal()
    //
    /// <summary>
    /// Attempts to get the network of people who have tweeted a specified
    /// search term.
    /// </summary>
    ///
    /// <param name="sSearchTerm">
    /// The term to search for.
    /// </param>
    ///
    /// <param name="eWhatToInclude">
    /// Specifies what should be included in the network.
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
    TryGetSearchNetworkInternal
    (
        String sSearchTerm,
        WhatToInclude eWhatToInclude,
        Int32 iMaximumPeoplePerRequest,
        String sCredentialsScreenName,
        String sCredentialsPassword,
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs,
        out XmlDocument oXmlDocument
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sSearchTerm) );
        Debug.Assert(iMaximumPeoplePerRequest > 0);

        Debug.Assert( sCredentialsScreenName == null ||
            ( sCredentialsScreenName.Length > 0 &&
                !String.IsNullOrEmpty(sCredentialsPassword) ) );

        Debug.Assert(oBackgroundWorker == null || oDoWorkEventArgs != null);
        AssertValid();

        oXmlDocument = null;

        Boolean bIncludeStatistics = WhatToIncludeFlagIsSet(
            eWhatToInclude, WhatToInclude.Statistics);

        GraphMLXmlDocument oGraphMLXmlDocument = CreateGraphMLXmlDocument(
            bIncludeStatistics, false);

        if ( WhatToIncludeFlagIsSet(eWhatToInclude, WhatToInclude.Statuses) )
        {
            oGraphMLXmlDocument.DefineGraphMLAttribute(false, StatusID,
                "Tweet", "string", null);
        }

        // The key is the screen name and the value is the corresponding
        // TwitterVertex.  This is used to prevent the same screen name from
        // being added to the XmlDocument twice.

        Dictionary<String, TwitterVertex> oScreenNameDictionary =
            new Dictionary<String, TwitterVertex>();

        // First, add a vertex for each person who has tweeted the search term.

        if ( !TryAppendVertexXmlNodes(sSearchTerm, eWhatToInclude,
            iMaximumPeoplePerRequest, sCredentialsScreenName,
            sCredentialsPassword, oGraphMLXmlDocument, oScreenNameDictionary,
            oBackgroundWorker, oDoWorkEventArgs) )
        {
            return (false);
        }

        if ( WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.FollowedEdges) )
        {
            // Look at the people followed by each author, and if a followed
            // has also tweeted the search term, add an edge between the author
            // and the followed.

            if ( !TryAppendFollowedEdgeXmlNodes(eWhatToInclude,
                iMaximumPeoplePerRequest, sCredentialsScreenName,
                sCredentialsPassword, oGraphMLXmlDocument,
                oScreenNameDictionary, oBackgroundWorker, oDoWorkEventArgs) )
            {
                return (false);
            }
        }

        AppendMissingGraphMLAttributeValues(oGraphMLXmlDocument,
            oScreenNameDictionary, bIncludeStatistics, false,
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
    //  Method: TryAppendVertexXmlNodes()
    //
    /// <summary>
    /// Attempts to append a vertex XML node for each person who has tweeted a
    /// specified search term.
    /// </summary>
    ///
    /// <param name="sSearchTerm">
    /// The term to search for.
    /// </param>
    ///
    /// <param name="eWhatToInclude">
    /// Specifies what should be included in the network.
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
    /// true if the vertices were added, or false if the user cancelled.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryAppendVertexXmlNodes
    (
        String sSearchTerm,
        WhatToInclude eWhatToInclude,
        Int32 iMaximumPeoplePerRequest,
        String sCredentialsScreenName,
        String sCredentialsPassword,
        GraphMLXmlDocument oGraphMLXmlDocument,
        Dictionary<String, TwitterVertex> oScreenNameDictionary,
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sSearchTerm) );
        Debug.Assert(iMaximumPeoplePerRequest > 0);

        Debug.Assert( sCredentialsScreenName == null ||
            ( sCredentialsScreenName.Length > 0 &&
                !String.IsNullOrEmpty(sCredentialsPassword) ) );

        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oScreenNameDictionary != null);
        Debug.Assert(oBackgroundWorker == null || oDoWorkEventArgs != null);
        AssertValid();

        // Convert spaces in the search term to a plus sign.
        //
        // (Note: Don't try to use Twitter's "show_user" URL parameter in an
        // attempt to get a "user" XML node for each author.  That's not what
        // the URL parameter does.)

        String sUrl = String.Format(

            "http://search.twitter.com/search.atom?q={0}&rpp=100"
            ,
            UrlUtil.EncodeUrlParameter(sSearchTerm).Replace("%20", "+")
            );

        ReportProgress("Getting a list of tweets");

        Boolean bIncludeStatistics = WhatToIncludeFlagIsSet(
            eWhatToInclude, WhatToInclude.Statistics);

        Boolean bIncludeStatuses = WhatToIncludeFlagIsSet(
            eWhatToInclude, WhatToInclude.Statuses);

        // The document consists of an "entry" XML node for each tweet that
        // contains the search term.  Multiple tweets may have the same author,
        // so we have to enumerate until the requested maximum number of unique
        // authors is reached.

        foreach ( XmlNode oAuthorNameXmlNode in EnumerateXmlNodes(
            sUrl, "a:feed/a:entry/a:author/a:name", "a", AtomNamespaceUri,
            15, Int32.MaxValue, true, false, sCredentialsScreenName,
            sCredentialsPassword) )
        {
            if ( CancelIfRequested(oBackgroundWorker, oDoWorkEventArgs) )
            {
                return (false);
            }

            if (oScreenNameDictionary.Count == iMaximumPeoplePerRequest)
            {
                break;
            }

            // The author name is in this format:
            //
            // james_laker (James Laker)

            String sAuthorName;

            if ( !XmlUtil.GetInnerText(oAuthorNameXmlNode, false,
                out sAuthorName) )
            {
                continue;
            }

            Int32 iIndexOfSpace = sAuthorName.IndexOf(' ');
            String sScreenName = sAuthorName;

            if (iIndexOfSpace != -1)
            {
                sScreenName = sAuthorName.Substring(0, iIndexOfSpace);
            }

            TwitterVertex oTwitterVertex;
            
            if ( !TryAppendVertexXmlNode(sScreenName, null,
                oGraphMLXmlDocument, oScreenNameDictionary, bIncludeStatistics,
                false, out oTwitterVertex) )
            {
                // A tweet for the author has already been found.

                continue;
            }

            XmlNode oVertexXmlNode = oTwitterVertex.VertexXmlNode;
            XmlNode oEntryXmlNode = oAuthorNameXmlNode.ParentNode.ParentNode;

            XmlNamespaceManager oXmlNamespaceManager = new XmlNamespaceManager(
                oEntryXmlNode.OwnerDocument.NameTable);

            oXmlNamespaceManager.AddNamespace("a", AtomNamespaceUri);

            // Tet the image URL and status for the tweet's author.

            XmlNode oLinkXmlNode = oEntryXmlNode.SelectSingleNode(
                "a:link[@rel='image']", oXmlNamespaceManager);

            if (oLinkXmlNode != null)
            {
                String sImageUrl;

                if ( XmlUtil.GetAttribute(oLinkXmlNode, "href", false,
                    out sImageUrl) )
                {
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                        oVertexXmlNode, ImageUrlID, sImageUrl);
                }
            }

            XmlNode oTitleXmlNode = oEntryXmlNode.SelectSingleNode(
                "a:title", oXmlNamespaceManager);

            if (oTitleXmlNode != null)
            {
                String sStatus = oTitleXmlNode.InnerText;
                oTwitterVertex.StatusForAnalysis = sStatus;

                if (bIncludeStatuses)
                {
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                        oVertexXmlNode, StatusID, sStatus);
                }
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryAppendFollowedEdgeXmlNodes()
    //
    /// <summary>
    /// Attempts to append an edge XML node for each pair of people who have
    /// tweeted a specified search term and one follows the other.
    /// </summary>
    ///
    /// <param name="eWhatToInclude">
    /// Specifies what should be included in the network.
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
    /// true if the edges were added, or false if the user cancelled.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryAppendFollowedEdgeXmlNodes
    (
        WhatToInclude eWhatToInclude,
        Int32 iMaximumPeoplePerRequest,
        String sCredentialsScreenName,
        String sCredentialsPassword,
        GraphMLXmlDocument oGraphMLXmlDocument,
        Dictionary<String, TwitterVertex> oScreenNameDictionary,
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs
    )
    {
        Debug.Assert(iMaximumPeoplePerRequest > 0);

        Debug.Assert( sCredentialsScreenName == null ||
            ( sCredentialsScreenName.Length > 0 &&
                !String.IsNullOrEmpty(sCredentialsPassword) ) );

        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oScreenNameDictionary != null);
        Debug.Assert(oBackgroundWorker == null || oDoWorkEventArgs != null);
        AssertValid();

        Boolean bIncludeStatistics = WhatToIncludeFlagIsSet(
            eWhatToInclude, WhatToInclude.Statistics);

        // Look at the people followed by each author, and if a followed has
        // also tweeted the search term, add an edge between the author and the
        // followed.

        foreach (String sScreenName in oScreenNameDictionary.Keys)
        {
            ReportProgressForFollowedOrFollowing(sScreenName, true);

            foreach ( XmlNode oOtherUserXmlNode in EnumerateXmlNodes(
                GetFollowedOrFollowingUrl(sScreenName, true),
                "users_list/users/user", null, null, Int32.MaxValue,
                iMaximumPeoplePerRequest, false, true, sCredentialsScreenName,
                sCredentialsPassword) )
            {
                if ( CancelIfRequested(oBackgroundWorker, oDoWorkEventArgs) )
                {
                    return (false);
                }

                String sOtherScreenName;

                if ( !TryGetScreenName(oOtherUserXmlNode,
                    out sOtherScreenName) )
                {
                    continue;
                }

                TwitterVertex oOtherTwitterVertex;

                if ( oScreenNameDictionary.TryGetValue(sOtherScreenName,
                    out oOtherTwitterVertex) )
                {
                    XmlNode oOtherVertexXmlNode =
                        oOtherTwitterVertex.VertexXmlNode;

                    AppendEdgeXmlNode(oGraphMLXmlDocument, sScreenName,
                        sOtherScreenName, "Followed");

                    if (bIncludeStatistics &&
                        !AppendFromUserXmlNodeCalled(oGraphMLXmlDocument,
                            oOtherVertexXmlNode) )
                    {
                        // The other vertex node has no statistics.  Add them.

                        AppendFromUserXmlNode(sOtherScreenName,
                            oOtherUserXmlNode, oGraphMLXmlDocument,
                            oOtherTwitterVertex, bIncludeStatistics, false);
                    }
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

        Debug.Assert(e.Argument is GetSearchNetworkAsyncArgs);

        GetSearchNetworkAsyncArgs oGetSearchNetworkAsyncArgs =
            (GetSearchNetworkAsyncArgs)e.Argument;

        XmlDocument oGraphMLDocument;
        
        if ( TryGetSearchNetworkInternal(
            oGetSearchNetworkAsyncArgs.SearchTerm,
            oGetSearchNetworkAsyncArgs.WhatToInclude,
            oGetSearchNetworkAsyncArgs.MaximumPeoplePerRequest,
            oGetSearchNetworkAsyncArgs.CredentialsScreenName,
            oGetSearchNetworkAsyncArgs.CredentialsPassword,
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

    /// GraphML-attribute IDs.

    protected const String StatusID = "Status";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)


    //*************************************************************************
    //  Embedded class: GetSearchNetworkAsyncArguments()
    //
    /// <summary>
    /// Contains the arguments needed to asynchronously get the network of
    /// people who have tweeted a specified search term.
    /// </summary>
    //*************************************************************************

    protected class GetSearchNetworkAsyncArgs : GetNetworkAsyncArgs
    {
        ///
        public String SearchTerm;
        ///
        public WhatToInclude WhatToInclude;
    };
}

}
