
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Net;
using System.Xml;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.SocialNetworkLib;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.XmlLib;
using Microsoft.Research.CommunityTechnologies.DateTimeLib;

namespace Microsoft.NodeXL.GraphDataProviders.Flickr
{
//*****************************************************************************
//  Class: FlickrUserNetworkAnalyzer
//
/// <summary>
/// Gets a network of Flickr users.
/// </summary>
///
/// <remarks>
/// Use <see cref="GetNetworkAsync" /> to asynchronously get a directed network
/// of Flickr users.
/// </remarks>
//*****************************************************************************

public class FlickrUserNetworkAnalyzer : FlickrNetworkAnalyzerBase
{
    //*************************************************************************
    //  Constructor: FlickrUserNetworkAnalyzer()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="FlickrUserNetworkAnalyzer" /> class.
    /// </summary>
    //*************************************************************************

    public FlickrUserNetworkAnalyzer()
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
        /// Include a vertex for each of the user's contacts.
        /// </summary>

        ContactVertices = 1,

        /// <summary>
        /// Include a vertex for each user who has commented on the user's
        /// photos.
        /// </summary>

        CommenterVertices = 2,

        /// <summary>
        /// Include information about each user in the network.
        /// </summary>

        UserInformation = 4,
    }

    //*************************************************************************
    //  Method: GetNetworkAsync()
    //
    /// <summary>
    /// Asynchronously gets a directed network of Flickr users.
    /// </summary>
    ///
    /// <param name="screenName">
    /// The screen name of the Flickr user whose network should be analyzed.
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
    /// <param name="maximumPerRequest">
    /// Maximum number of people or photos to request for each query, or
    /// Int32.MaxValue for no limit.
    /// </param>
    ///
    /// <param name="apiKey">
    /// Flickr API key.
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
        String screenName,
        WhatToInclude whatToInclude,
        NetworkLevel networkLevel,
        Int32 maximumPerRequest,
        String apiKey
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(screenName) );

        Debug.Assert(networkLevel == NetworkLevel.One ||
            networkLevel == NetworkLevel.OnePointFive ||
            networkLevel == NetworkLevel.Two);

        Debug.Assert(maximumPerRequest > 0);
        Debug.Assert( !String.IsNullOrEmpty(apiKey) );
        AssertValid();

        const String MethodName = "GetNetworkAsync";
        CheckIsBusy(MethodName);

        // Wrap the arguments in an object that can be passed to
        // BackgroundWorker.RunWorkerAsync().

        GetNetworkAsyncArgs oGetNetworkAsyncArgs = new GetNetworkAsyncArgs();

        oGetNetworkAsyncArgs.ScreenName = screenName;
        oGetNetworkAsyncArgs.WhatToInclude = whatToInclude;
        oGetNetworkAsyncArgs.NetworkLevel = networkLevel;
        oGetNetworkAsyncArgs.MaximumPerRequest = maximumPerRequest;
        oGetNetworkAsyncArgs.ApiKey = apiKey;

        m_oBackgroundWorker.RunWorkerAsync(oGetNetworkAsyncArgs);
    }

    //*************************************************************************
    //  Method: GetUserNetworkInternal()
    //
    /// <overloads>
    /// Gets a network of Flickr users.
    /// </overloads>
    ///
    /// <summary>
    /// Gets a network of Flickr users.
    /// </summary>
    ///
    /// <param name="sScreenName">
    /// Flickr screen name.
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
    /// <param name="iMaximumPerRequest">
    /// Maximum number of people or photos to request for each query, or
    /// Int32.MaxValue for no limit.
    /// </param>
    ///
    /// <param name="sApiKey">
    /// Flickr API key.
    /// </param>
    ///
    /// <returns>
    /// An XmlDocument containing the network as GraphML.
    /// </returns>
    //*************************************************************************

    protected XmlDocument
    GetUserNetworkInternal
    (
        String sScreenName,
        WhatToInclude eWhatToInclude,
        NetworkLevel eNetworkLevel,
        Int32 iMaximumPerRequest,
        String sApiKey
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenName) );

        Debug.Assert(eNetworkLevel == NetworkLevel.One ||
            eNetworkLevel == NetworkLevel.OnePointFive ||
            eNetworkLevel == NetworkLevel.Two);

        Debug.Assert(iMaximumPerRequest > 0);
        Debug.Assert( !String.IsNullOrEmpty(sApiKey) );

        AssertValid();

        GraphMLXmlDocument oGraphMLXmlDocument = CreateGraphMLXmlDocument(

            WhatToIncludeFlagIsSet(eWhatToInclude,
                WhatToInclude.CommenterVertices),

            WhatToIncludeFlagIsSet(eWhatToInclude,
                WhatToInclude.UserInformation)
                );

        RequestStatistics oRequestStatistics = new RequestStatistics();

        try
        {
            GetUserNetworkInternal(sScreenName, eWhatToInclude, eNetworkLevel,
                iMaximumPerRequest, sApiKey, oRequestStatistics,
                oGraphMLXmlDocument);
        }
        catch (Exception oException)
        {
            OnTerminatingException(oException);
        }

        OnNetworkObtainedWithoutTerminatingException(oGraphMLXmlDocument,
            oRequestStatistics);

        return (oGraphMLXmlDocument);
    }

    //*************************************************************************
    //  Method: GetUserNetworkInternal()
    //
    /// <summary>
    /// Gets a network of Flickr users, given a GraphMLXmlDocument.
    /// </summary>
    ///
    /// <param name="sScreenName">
    /// Flickr screen name.
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
    /// <param name="iMaximumPerRequest">
    /// Maximum number of people or photos to request for each query, or
    /// Int32.MaxValue for no limit.
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
    /// <param name="oGraphMLXmlDocument">
    /// The GraphMLXmlDocument to populate with the requested network.
    /// </param>
    //*************************************************************************

    protected void
    GetUserNetworkInternal
    (
        String sScreenName,
        WhatToInclude eWhatToInclude,
        NetworkLevel eNetworkLevel,
        Int32 iMaximumPerRequest,
        String sApiKey,
        RequestStatistics oRequestStatistics,
        GraphMLXmlDocument oGraphMLXmlDocument
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenName) );

        Debug.Assert(eNetworkLevel == NetworkLevel.One ||
            eNetworkLevel == NetworkLevel.OnePointFive ||
            eNetworkLevel == NetworkLevel.Two);

        Debug.Assert(iMaximumPerRequest > 0);
        Debug.Assert( !String.IsNullOrEmpty(sApiKey) );
        Debug.Assert(oRequestStatistics != null);
        Debug.Assert(oGraphMLXmlDocument != null);
        AssertValid();

        String sUserID;

        // Get the user's ID and the correct case of the screen name.

        FlickrScreenNameToUserID(sScreenName, sApiKey, oRequestStatistics,
            out sUserID, out sScreenName);

        // The key is the user ID name and the value is the corresponding
        // GraphML XML node that represents the user.  This is used to prevent
        // the same user ID from being added to the XmlDocument twice.

        Dictionary<String, XmlNode> oUserIDDictionary =
            new Dictionary<String, XmlNode>();

        // Include contacts, commenters, both, or neither.

        Boolean [] abIncludes = new Boolean[] {

            WhatToIncludeFlagIsSet(eWhatToInclude,
                WhatToInclude.ContactVertices),

            WhatToIncludeFlagIsSet(eWhatToInclude,
                WhatToInclude.CommenterVertices),
            };

        for (Int32 i = 0; i < 2; i++)
        {
            if ( abIncludes[i] )
            {
                GetUserNetworkRecursive(sUserID, sScreenName, eWhatToInclude,
                    (i == 0), eNetworkLevel, iMaximumPerRequest, sApiKey, 1,
                    oGraphMLXmlDocument, oUserIDDictionary,
                    oRequestStatistics);
            }
        }

        if ( WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.UserInformation) )
        {
            AppendUserInformationGraphMLAttributeValues(oGraphMLXmlDocument,
                oUserIDDictionary, sApiKey, oRequestStatistics);
        }
    }

    //*************************************************************************
    //  Method: GetUserNetworkRecursive()
    //
    /// <summary>
    /// Recursively gets a network of Flickr users.
    /// </summary>
    ///
    /// <param name="sUserID">
    /// Flickr user ID.
    /// </param>
    ///
    /// <param name="sScreenName">
    /// Flickr screen name.
    /// </param>
    ///
    /// <param name="eWhatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="bIncludeContactsThisCall">
    /// true to include the user's contacts, false to include the people who
    /// have commented on the user's photos.
    /// </param>
    ///
    /// <param name="eNetworkLevel">
    /// Network level to include.  Must be NetworkLevel.One, OnePointFive, or
    /// Two.
    /// </param>
    ///
    /// <param name="iMaximumPerRequest">
    /// Maximum number of people or photos to request for each query, or
    /// Int32.MaxValue for no limit.
    /// </param>
    ///
    /// <param name="sApiKey">
    /// Flickr API key.
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
    /// <param name="oUserIDDictionary">
    /// The key is the user ID and the value is the corresponding GraphML XML
    /// node that represents the user.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    //*************************************************************************

    protected void
    GetUserNetworkRecursive
    (
        String sUserID,
        String sScreenName,
        WhatToInclude eWhatToInclude,
        Boolean bIncludeContactsThisCall,
        NetworkLevel eNetworkLevel,
        Int32 iMaximumPerRequest,
        String sApiKey,
        Int32 iRecursionLevel,
        GraphMLXmlDocument oGraphMLXmlDocument,
        Dictionary<String, XmlNode> oUserIDDictionary,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUserID) );
        Debug.Assert( !String.IsNullOrEmpty(sScreenName) );

        Debug.Assert(eNetworkLevel == NetworkLevel.One ||
            eNetworkLevel == NetworkLevel.OnePointFive ||
            eNetworkLevel == NetworkLevel.Two);

        Debug.Assert(iMaximumPerRequest > 0);
        Debug.Assert( !String.IsNullOrEmpty(sApiKey) );
        Debug.Assert(iRecursionLevel == 1 || iRecursionLevel == 2);
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oUserIDDictionary != null);
        Debug.Assert(oRequestStatistics != null);
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

        Boolean bNeedToAppendVertices = GetNeedToAppendVertices(eNetworkLevel,
            iRecursionLevel);

        List<String> oUserIDsToRecurse = new List<String>();

        ReportProgressForContactsOrCommenters(sScreenName,
            bIncludeContactsThisCall);

        Boolean bThisUserAppended = false;

        foreach ( XmlNode oChildXmlNode in GetContactsOrCommentersEnumerator(
            sUserID, bIncludeContactsThisCall, iMaximumPerRequest,
            oGraphMLXmlDocument, sApiKey, oRequestStatistics) )
        {
            String sOtherScreenName, sOtherUserID;

            if (
                !XmlUtil2.TrySelectSingleNodeAsString(oChildXmlNode,
                    bIncludeContactsThisCall ? "@username" : "@authorname",
                    null, out sOtherScreenName)
                ||
                !XmlUtil2.TrySelectSingleNodeAsString(oChildXmlNode,
                    bIncludeContactsThisCall ? "@nsid" : "@author",
                    null, out sOtherUserID)
                )
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

                TryAppendVertexXmlNode(sUserID, sScreenName,
                    oGraphMLXmlDocument, oUserIDDictionary);

                bThisUserAppended = true;
            }

            if (bNeedToAppendVertices)
            {
                if (
                    TryAppendVertexXmlNode(sOtherUserID, sOtherScreenName,
                        oGraphMLXmlDocument, oUserIDDictionary)
                    &&
                    bNeedToRecurse
                    )
                {
                    oUserIDsToRecurse.Add(sOtherUserID);
                }
            }

            if ( bNeedToAppendVertices ||
                oUserIDDictionary.ContainsKey(sOtherUserID) )
            {
                // Append an edge node and optional attributes.

                XmlNode oEdgeXmlNode;

                if (bIncludeContactsThisCall)
                {
                    oEdgeXmlNode = AppendEdgeXmlNode(oGraphMLXmlDocument,
                        sScreenName, sOtherScreenName, "Contact");
                }
                else
                {
                    // (Note the swapping of screen names in the commenter
                    // case.)

                    oEdgeXmlNode = AppendEdgeXmlNode(oGraphMLXmlDocument,
                        sOtherScreenName, sScreenName, "Commenter");

                    UInt32 uCommentDateUtc;

                    if ( XmlUtil2.TrySelectSingleNodeAsUInt32(oChildXmlNode,
                            "@datecreate", null, out uCommentDateUtc) )
                    {
                        DateTime oCommentDateUtc =
                            DateTimeUtil2.UnixTimestampToDateTimeUtc(
                                uCommentDateUtc);

                        oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                            oEdgeXmlNode, CommentDateUtcID,

                            ExcelDateTimeUtil.DateTimeToStringLocale1033(
                                oCommentDateUtc, ExcelColumnFormat.DateAndTime)
                            );
                    }

                    AppendStringGraphMLAttributeValue(oChildXmlNode,
                        "@permalink", null, oGraphMLXmlDocument, oEdgeXmlNode,
                        CommentUrlID);
                }
            }
        }

        if (bNeedToRecurse)
        {
            foreach (String sUserIDToRecurse in oUserIDsToRecurse)
            {
                XmlNode oVertexXmlNode = oUserIDDictionary[sUserIDToRecurse];

                String sScreenNameToRecurse = GetScreenNameFromVertexXmlNode(
                    oVertexXmlNode);

                GetUserNetworkRecursive(sUserIDToRecurse, sScreenNameToRecurse,
                    eWhatToInclude, bIncludeContactsThisCall, eNetworkLevel,
                    iMaximumPerRequest, sApiKey, 2, oGraphMLXmlDocument,
                    oUserIDDictionary, oRequestStatistics);
            }
        }
    }

    //*************************************************************************
    //  Method: CreateGraphMLXmlDocument()
    //
    /// <summary>
    /// Creates a GraphMLXmlDocument representing a network of Flickr users.
    /// </summary>
    ///
    /// <param name="bIncludeCommenterVertices">
    /// true to include a vertex for each user who has commented on the user's
    /// photos.
    /// </param>
    ///
    /// <param name="bIncludeUserInformation">
    /// true to include information about each user in the network.
    /// </param>
    ///
    /// <returns>
    /// A GraphMLXmlDocument representing a network of Flickr users.  The
    /// document includes GraphML-attribute definitions but no vertices or
    /// edges.
    /// </returns>
    //*************************************************************************

    protected GraphMLXmlDocument
    CreateGraphMLXmlDocument
    (
        Boolean bIncludeCommenterVertices,
        Boolean bIncludeUserInformation
    )
    {
        AssertValid();

        GraphMLXmlDocument oGraphMLXmlDocument = new GraphMLXmlDocument(true);

        DefineRelationshipGraphMLAttribute(oGraphMLXmlDocument);

        if (bIncludeCommenterVertices)
        {
            oGraphMLXmlDocument.DefineGraphMLAttribute(true, CommentDateUtcID,
                "Comment Time (UTC)", "string", null);

            oGraphMLXmlDocument.DefineGraphMLAttribute(true, CommentUrlID,
                "Comment URL", "string", null);
        }

        if (bIncludeUserInformation)
        {
            oGraphMLXmlDocument.DefineGraphMLAttribute(false, RealNameID,
                "Real Name", "string", null);

            oGraphMLXmlDocument.DefineGraphMLAttribute(false, TotalPhotosID,
                "Total Photos", "int", null);

            oGraphMLXmlDocument.DefineGraphMLAttribute(false, IsProfessionalID,
                "Is Professional", "string", null);

            DefineImageFileGraphMLAttribute(oGraphMLXmlDocument);
            DefineCustomMenuGraphMLAttributes(oGraphMLXmlDocument);
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
    //  Method: GetContactsOrCommentersEnumerator()
    //
    /// <summary>
    /// Gets an object that will enumerate the child XML nodes containing
    /// information about a Flickr user.
    /// </summary>
    ///
    /// <param name="sUserID">
    /// Flickr user ID.
    /// </param>
    ///
    /// <param name="bForContacts">
    /// true if getting the user's contacts, false if getting the user's
    /// commenters.
    /// </param>
    ///
    /// <param name="iMaximumPerRequest">
    /// Maximum number of people or photos to request for each query, or
    /// Int32.MaxValue for no limit.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    /// <param name="sApiKey">
    /// Flickr API key.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <returns>
    /// The requested enumerator.
    /// </returns>
    //*************************************************************************

    protected IEnumerable<XmlNode>
    GetContactsOrCommentersEnumerator
    (
        String sUserID,
        Boolean bForContacts,
        Int32 iMaximumPerRequest,
        GraphMLXmlDocument oGraphMLXmlDocument,
        String sApiKey,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUserID) );
        Debug.Assert(iMaximumPerRequest > 0);
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert( !String.IsNullOrEmpty(sApiKey) );
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        // If the GraphMLXmlDocument already contains at least one vertex node,
        // then this is the second time that this method has been called, a
        // partial network has already been obtained, and most errors should
        // now be skipped.  However, if none of the network has been obtained
        // yet, errors on page 1 should throw an immediate exception.

        Boolean bSkipMostPage1Errors = oGraphMLXmlDocument.HasVertexXmlNode;

        if (bForContacts)
        {
            String sUrl = GetFlickrMethodUrl( "flickr.contacts.getPublicList",
                sApiKey, GetUserIDUrlParameter(sUserID) );

            return ( EnumerateXmlNodes(sUrl, "rsp/contacts/contact",
                iMaximumPerRequest, bSkipMostPage1Errors,
                oRequestStatistics) );
        }

        return ( GetCommentersEnumerator(sUserID, iMaximumPerRequest,
            bSkipMostPage1Errors, sApiKey, oRequestStatistics) );
    }

    //*************************************************************************
    //  Method: GetCommentersEnumerator()
    //
    /// <summary>
    /// Gets an object that will enumerate the "comment" XML nodes for the
    /// people who have commented on a Flickr user's photos.
    /// </summary>
    ///
    /// <param name="sUserID">
    /// User ID to get the comments for.
    /// </param>
    ///
    /// <param name="iMaximumPerRequest">
    /// Maximum number of people or photos to request for each query, or
    /// Int32.MaxValue for no limit.
    /// </param>
    ///
    /// <param name="bSkipMostPage1Errors">
    /// If true, most page-1 errors are skipped over.  If false, they are
    /// rethrown.  (Errors that occur on page 2 and above are always skipped.)
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
    //*************************************************************************

    protected IEnumerable<XmlNode>
    GetCommentersEnumerator
    (
        String sUserID,
        Int32 iMaximumPerRequest,
        Boolean bSkipMostPage1Errors,
        String sApiKey,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUserID) );
        Debug.Assert(iMaximumPerRequest > 0);
        Debug.Assert( !String.IsNullOrEmpty(sApiKey) );
        Debug.Assert(oRequestStatistics != null);

        AssertValid();

        // Get the user's public photos, which are paged.

        String sUrl = GetFlickrMethodUrl( "flickr.people.getPublicPhotos",
            sApiKey, GetUserIDUrlParameter(sUserID) );

        foreach ( XmlNode oPhotoXmlNode in EnumerateXmlNodes(sUrl, 
            "rsp/photos/photo", iMaximumPerRequest, bSkipMostPage1Errors,
            oRequestStatistics) )
        {
            String sPhotoID;

            if ( !XmlUtil2.TrySelectSingleNodeAsString(oPhotoXmlNode, "@id",
                null, out sPhotoID) )
            {
                continue;
            }

            // Get the photo's comments, which are not paged.

            ReportProgress( String.Format(

                "Getting comments for the photo \"{0}\"."
                ,
                sPhotoID
                ) );

            sUrl = GetFlickrMethodUrl("flickr.photos.comments.getList",
                sApiKey, "&photo_id=" + UrlUtil.EncodeUrlParameter(sPhotoID)
                );

            XmlDocument oXmlDocument;

            if ( TryGetXmlDocument(sUrl, oRequestStatistics,
                out oXmlDocument) )
            {
                foreach ( XmlNode oCommentXmlNode in oXmlDocument.SelectNodes(
                    "rsp/comments/comment") )
                {
                    yield return oCommentXmlNode;
                }
            }
        }
    }

    //*************************************************************************
    //  Method: ReportProgressForContactsOrCommenters()
    //
    /// <summary>
    /// Reports progress before getting a user's contacts or subscribers.
    /// </summary>
    ///
    /// <param name="sScreenName">
    /// Flickr screen name.
    /// </param>
    ///
    /// <param name="bForContacts">
    /// true if getting the user's contacts, false if getting the user's
    /// commenters.
    /// </param>
    //*************************************************************************

    protected void
    ReportProgressForContactsOrCommenters
    (
        String sScreenName,
        Boolean bForContacts
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenName) );
        AssertValid();

        ReportProgress( String.Format(

            "Getting {0} \"{1}\"."
            ,
            bForContacts ? "contacts of" : "people who commented on",
            sScreenName
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
    /// <param name="sUserID">
    /// User ID to add a vertex XML node for.
    /// </param>
    ///
    /// <param name="sScreenName">
    /// Screen name to add a vertex XML node for.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oUserIDDictionary">
    /// The key is the user ID and the value is the corresponding GraphML XML
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
        String sUserID,
        String sScreenName,
        GraphMLXmlDocument oGraphMLXmlDocument,
        Dictionary<String, XmlNode> oUserIDDictionary
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUserID) );
        Debug.Assert( !String.IsNullOrEmpty(sScreenName) );
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oUserIDDictionary != null);

        XmlNode oVertexXmlNode;

        if ( oUserIDDictionary.TryGetValue(sUserID, out oVertexXmlNode) )
        {
            return (false);
        }

        oVertexXmlNode = oGraphMLXmlDocument.AppendVertexXmlNode(sScreenName);
        oUserIDDictionary.Add(sUserID, oVertexXmlNode);

        return (true);
    }

    //*************************************************************************
    //  Method: GetScreenNameFromVertexXmlNode()
    //
    /// <summary>
    /// Gets the user name from a vertex XML node in the GraphML document.
    /// </summary>
    ///
    /// <param name="oVertexXmlNode">
    /// A vertex node from the GraphML document.
    /// </param>
    ///
    /// <returns>
    /// The user name from the vertex XML node.
    /// </returns>
    //*************************************************************************

    protected String
    GetScreenNameFromVertexXmlNode
    (
        XmlNode oVertexXmlNode
    )
    {
        Debug.Assert(oVertexXmlNode != null);
        AssertValid();

        return ( XmlUtil2.SelectRequiredSingleNodeAsString(oVertexXmlNode,
            "@id", null) );
    }

    //*************************************************************************
    //  Method: AppendUserInformationGraphMLAttributeValues()
    //
    /// <overloads>
    /// Appends user information GraphML attribute values to the GraphML
    /// document.
    /// </overloads>
    ///
    /// <summary>
    /// Appends user information GraphML attribute values to the GraphML
    /// document for all users in the network.
    /// </summary>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oUserIDDictionary">
    /// The key is the user ID and the value is the corresponding GraphML XML
    /// node that represents the user.
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
    //*************************************************************************

    protected void
    AppendUserInformationGraphMLAttributeValues
    (
        GraphMLXmlDocument oGraphMLXmlDocument,
        Dictionary<String, XmlNode> oUserIDDictionary,
        String sApiKey,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oUserIDDictionary != null);
        Debug.Assert( !String.IsNullOrEmpty(sApiKey) );
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        foreach (KeyValuePair<String, XmlNode> oKeyValuePair in
            oUserIDDictionary)
        {
            String sUserID = oKeyValuePair.Key;
            XmlNode oVertexXmlNode = oKeyValuePair.Value;

            ReportProgress( String.Format(

                "Getting information about \"{0}\"."
                ,
                GetScreenNameFromVertexXmlNode(oVertexXmlNode)
                ) );

            AppendUserInformationGraphMLAttributeValues(sUserID,
                oVertexXmlNode, oGraphMLXmlDocument, sApiKey,
                oRequestStatistics);
        }
    }

    //*************************************************************************
    //  Method: AppendUserInformationGraphMLAttributeValues()
    //
    /// <summary>
    /// Appends user information GraphML attribute values to the GraphML
    /// document for one user in the network.
    /// </summary>
    ///
    /// <param name="sUserID">
    /// The user ID.
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
    /// <param name="sApiKey">
    /// Flickr API key.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    //*************************************************************************

    protected void
    AppendUserInformationGraphMLAttributeValues
    (
        String sUserID,
        XmlNode oVertexXmlNode,
        GraphMLXmlDocument oGraphMLXmlDocument,
        String sApiKey,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUserID) );
        Debug.Assert(oVertexXmlNode != null);
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert( !String.IsNullOrEmpty(sApiKey) );
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        String sUrl = GetFlickrMethodUrl( "flickr.people.getInfo",
            sApiKey, GetUserIDUrlParameter(sUserID) );

        XmlDocument oXmlDocument;

        if ( !TryGetXmlDocument(sUrl, oRequestStatistics, out oXmlDocument) )
        {
            // Ignore errors.  The user information isn't critical.

            return;
        }

        const String XPathRoot = "rsp/person/";

        AppendStringGraphMLAttributeValue(oXmlDocument,
            XPathRoot + "realname/text()", null, oGraphMLXmlDocument,
            oVertexXmlNode, RealNameID);

        AppendInt32GraphMLAttributeValue(oXmlDocument,
            XPathRoot + "photos/count/text()", null, oGraphMLXmlDocument,
            oVertexXmlNode, TotalPhotosID);

        String sIsProfessional;

        if ( XmlUtil2.TrySelectSingleNodeAsString(oXmlDocument,
            XPathRoot + "@ispro", null, out sIsProfessional) )
        {
            oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode,
                IsProfessionalID,
                (sIsProfessional == "0") ? "No" : "Yes"
                );
        }

        // Get the URL to the user's buddy icon, which is explained here:
        //
        // http://www.flickr.com/services/api/misc.buddyicons.html

        Int32 iIconServer, iIconFarm;
        String sBuddyIconUrl;

        if (
            XmlUtil2.TrySelectSingleNodeAsInt32(oXmlDocument,
                XPathRoot + "@iconserver", null, out iIconServer)
            &&
            XmlUtil2.TrySelectSingleNodeAsInt32(oXmlDocument,
                XPathRoot + "@iconfarm", null, out iIconFarm)
            &&
            iIconServer > 0
            )
        {
            sBuddyIconUrl = String.Format(

                "http://farm{0}.static.flickr.com/{1}/buddyicons/{2}.jpg"
                ,
                iIconFarm,
                iIconServer,
                sUserID
                );
        }
        else
        {
            sBuddyIconUrl = "http://www.flickr.com/images/buddyicon.jpg";
        }

        oGraphMLXmlDocument.AppendGraphMLAttributeValue(
            oVertexXmlNode, ImageFileID, sBuddyIconUrl);

        if ( AppendStringGraphMLAttributeValue(oXmlDocument,
            XPathRoot + "photosurl/text()", null, oGraphMLXmlDocument,
            oVertexXmlNode, MenuActionID) )
        {
            oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode,
                MenuTextID, "Open Flickr Page for This Person");
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

        Debug.Assert(e.Argument is GetNetworkAsyncArgs);

        GetNetworkAsyncArgs oGetNetworkAsyncArgs =
            (GetNetworkAsyncArgs)e.Argument;

        try
        {
            e.Result = GetUserNetworkInternal(oGetNetworkAsyncArgs.ScreenName,
                oGetNetworkAsyncArgs.WhatToInclude,
                oGetNetworkAsyncArgs.NetworkLevel,
                oGetNetworkAsyncArgs.MaximumPerRequest,
                oGetNetworkAsyncArgs.ApiKey);
        }
        catch (CancellationPendingException)
        {
            e.Cancel = true;
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

    protected const String CommentDateUtcID = "CommentDateUtc";
    ///
    protected const String CommentUrlID = "CommentUrl";
    ///
    protected const String RealNameID = "RealName";
    ///
    protected const String TotalPhotosID = "TotalPhotos";
    ///
    protected const String IsProfessionalID = "IsProfessional";


    //*************************************************************************
    //  Embedded class: GetNetworkAsyncArgs()
    //
    /// <summary>
    /// Contains the arguments needed to asynchronously get a network of Flickr
    /// users.
    /// </summary>
    //*************************************************************************

    protected class GetNetworkAsyncArgs : GetNetworkAsyncArgsBase
    {
        ///
        public String ScreenName;
        ///
        public WhatToInclude WhatToInclude;
        ///
        public NetworkLevel NetworkLevel;
        ///
        public Int32 MaximumPerRequest;
    };
}

}
