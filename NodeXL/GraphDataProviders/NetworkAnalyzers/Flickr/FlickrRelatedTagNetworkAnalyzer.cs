
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Net;
using System.Xml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.XmlLib;
using Microsoft.SocialNetworkLib;

namespace Microsoft.NodeXL.GraphDataProviders.Flickr
{
//*****************************************************************************
//  Class: FlickrRelatedTagNetworkAnalyzer
//
/// <summary>
/// Gets a network of related Flickr tags.
/// </summary>
///
/// <remarks>
/// Use <see cref="GetNetworkAsync" /> to asynchronously get an undirected
/// network of Flickr tags that are related to a specified tag.
/// </remarks>
//*****************************************************************************

public class FlickrRelatedTagNetworkAnalyzer : FlickrNetworkAnalyzerBase
{
    //*************************************************************************
    //  Constructor: FlickrRelatedTagNetworkAnalyzer()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="FlickrRelatedTagNetworkAnalyzer" /> class.
    /// </summary>
    //*************************************************************************

    public FlickrRelatedTagNetworkAnalyzer()
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
        /// Include a sample thumbnail for each tag.
        /// </summary>

        SampleThumbnails = 1,
    }

    //*************************************************************************
    //  Method: GetNetworkAsync()
    //
    /// <summary>
    /// Asynchronously gets an undirected network of Flickr tags related to a
    /// specified tag.
    /// </summary>
    ///
    /// <param name="tag">
    /// Tag to get related tags for.
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
        String tag,
        WhatToInclude whatToInclude,
        NetworkLevel networkLevel,
        String apiKey
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(tag) );

        Debug.Assert(networkLevel == NetworkLevel.One ||
            networkLevel == NetworkLevel.OnePointFive ||
            networkLevel == NetworkLevel.Two);

        Debug.Assert( !String.IsNullOrEmpty(apiKey) );
        AssertValid();

        const String MethodName = "GetNetworkAsync";
        CheckIsBusy(MethodName);

        // Wrap the arguments in an object that can be passed to
        // BackgroundWorker.RunWorkerAsync().

        GetNetworkAsyncArgs oGetNetworkAsyncArgs = new GetNetworkAsyncArgs();

        oGetNetworkAsyncArgs.Tag = tag;
        oGetNetworkAsyncArgs.NetworkLevel = networkLevel;
        oGetNetworkAsyncArgs.WhatToInclude = whatToInclude;
        oGetNetworkAsyncArgs.ApiKey = apiKey;

        m_oBackgroundWorker.RunWorkerAsync(oGetNetworkAsyncArgs);
    }

    //*************************************************************************
    //  Method: TryGetRelatedTagsInternal()
    //
    /// <overloads>
    /// Attempts to gets the Flickr tags related to a specified tag.
    /// </overloads>
    ///
    /// <summary>
    /// Attempts to gets the Flickr tags related to a specified tag.
    /// </summary>
    ///
    /// <param name="sTag">
    /// Tag to get related tags for.
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
    /// <param name="sApiKey">
    /// Flickr API key.
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
    /// true if the related Flickr tags were obtained, or false if the user
    /// cancelled.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetRelatedTagsInternal
    (
        String sTag,
        WhatToInclude eWhatToInclude,
        NetworkLevel eNetworkLevel,
        String sApiKey,
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs,
        out XmlDocument oXmlDocument
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sTag) );

        Debug.Assert(eNetworkLevel == NetworkLevel.One ||
            eNetworkLevel == NetworkLevel.OnePointFive ||
            eNetworkLevel == NetworkLevel.Two);

        Debug.Assert( !String.IsNullOrEmpty(sApiKey) );
        Debug.Assert(oBackgroundWorker == null || oDoWorkEventArgs != null);
        AssertValid();

        oXmlDocument = null;

        GraphMLXmlDocument oGraphMLXmlDocument = CreateGraphMLXmlDocument(
            WhatToIncludeFlagIsSet(eWhatToInclude,
                WhatToInclude.SampleThumbnails) );

        RequestStatistics oRequestStatistics = new RequestStatistics();

        try
        {
            if ( !TryGetRelatedTagsInternal(sTag, eWhatToInclude,
                eNetworkLevel, sApiKey, oRequestStatistics, oBackgroundWorker,
                oDoWorkEventArgs, oGraphMLXmlDocument) )
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
    //  Method: TryGetRelatedTagsInternal()
    //
    /// <summary>
    /// Attempts to gets the Flickr tags related to a specified tag, given a
    /// GraphXMLXmlDocument.
    /// </summary>
    ///
    /// <param name="sTag">
    /// Tag to get related tags for.
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
    /// <param name="sApiKey">
    /// Flickr API key.
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
    /// true if the related Flickr tags were obtained, or false if the user
    /// cancelled.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetRelatedTagsInternal
    (
        String sTag,
        WhatToInclude eWhatToInclude,
        NetworkLevel eNetworkLevel,
        String sApiKey,
        RequestStatistics oRequestStatistics,
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs,
        GraphMLXmlDocument oGraphMLXmlDocument
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sTag) );

        Debug.Assert(eNetworkLevel == NetworkLevel.One ||
            eNetworkLevel == NetworkLevel.OnePointFive ||
            eNetworkLevel == NetworkLevel.Two);

        Debug.Assert( !String.IsNullOrEmpty(sApiKey) );
        Debug.Assert(oRequestStatistics != null);
        Debug.Assert(oBackgroundWorker == null || oDoWorkEventArgs != null);
        Debug.Assert(oGraphMLXmlDocument != null);
        AssertValid();

        // The key is the tag name and the value is the corresponding GraphML
        // XML node that represents the tag.  This is used to prevent the same
        // tag from being added to the XmlDocument twice.

        Dictionary<String, XmlNode> oTagDictionary =
            new Dictionary<String, XmlNode>();

        if ( !TryGetRelatedTagsRecursive(sTag, eWhatToInclude, eNetworkLevel,
            sApiKey, 1, oGraphMLXmlDocument, oTagDictionary,
            oRequestStatistics, oBackgroundWorker, oDoWorkEventArgs) )
        {
            return (false);
        }

        if ( WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.SampleThumbnails) )
        {
            if ( !TryAppendSampleThumbnails(oTagDictionary,
                oGraphMLXmlDocument, sApiKey, oRequestStatistics,
                oBackgroundWorker, oDoWorkEventArgs) )
            {
                return (false);
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryGetRelatedTagsRecursive()
    //
    /// <summary>
    /// Attempts to recursively get a tag's related tags.
    /// </summary>
    ///
    /// <param name="sTag">
    /// Tag to get related tags for.
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
    /// <param name="oTagDictionary">
    /// The key is the tag name and the value is the corresponding GraphML XML
    /// node that represents the tag.
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
    /// true if the related Flickr tags were obtained, or false if the user
    /// cancelled.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetRelatedTagsRecursive
    (
        String sTag,
        WhatToInclude eWhatToInclude,
        NetworkLevel eNetworkLevel,
        String sApiKey,
        Int32 iRecursionLevel,
        GraphMLXmlDocument oGraphMLXmlDocument,
        Dictionary<String, XmlNode> oTagDictionary,
        RequestStatistics oRequestStatistics,
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sTag) );

        Debug.Assert(eNetworkLevel == NetworkLevel.One ||
            eNetworkLevel == NetworkLevel.OnePointFive ||
            eNetworkLevel == NetworkLevel.Two);

        Debug.Assert( !String.IsNullOrEmpty(sApiKey) );
        Debug.Assert(iRecursionLevel == 1 || iRecursionLevel == 2);
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oTagDictionary != null);
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

        if ( CancelIfRequested(oBackgroundWorker, oDoWorkEventArgs) )
        {
            return (false);
        }

        Boolean bNeedToRecurse = GetNeedToRecurse(eNetworkLevel,
            iRecursionLevel);

        Boolean bNeedToAppendVertices = GetNeedToAppendVertices(
            eNetworkLevel, iRecursionLevel);

        ReportProgress("Getting tags related to \"" + sTag + "\"");

        String sUrl = GetFlickrMethodUrl( "flickr.tags.getRelated", sApiKey,
            "&tag=" + UrlUtil.EncodeUrlParameter(sTag) );

        XmlDocument oXmlDocument;

        try
        {
            oXmlDocument = GetXmlDocument(sUrl, oRequestStatistics);
        }
        catch (Exception oException)
        {
            // If the exception is not a WebException or XmlException, or if
            // the none of the network has been obtained yet, throw the
            // exception.

            if (!ExceptionIsWebOrXml(oException) ||
                !oGraphMLXmlDocument.HasVertexXmlNode)
            {
                throw oException;
            }

            return (true);
        }

        // The document consists of a single "tags" node with zero or more
        // "tag" child nodes.

        String sOtherTag = null;

        XmlNodeList oTagNodes = oXmlDocument.DocumentElement.SelectNodes(
            "tags/tag");

        if (oTagNodes.Count > 0)
        {
            AppendVertexXmlNode(sTag, oGraphMLXmlDocument, oTagDictionary);
        }

        foreach (XmlNode oTagNode in oTagNodes)
        {
            sOtherTag = XmlUtil2.SelectRequiredSingleNodeAsString(oTagNode,
                "text()", null);

            if (bNeedToAppendVertices)
            {
                AppendVertexXmlNode(sOtherTag, oGraphMLXmlDocument,
                    oTagDictionary);
            }

            if ( bNeedToAppendVertices ||
                oTagDictionary.ContainsKey(sOtherTag) )
            {
                oGraphMLXmlDocument.AppendEdgeXmlNode(sTag, sOtherTag);
            }
        }

        if (bNeedToRecurse)
        {
            foreach (XmlNode oTagNode in oTagNodes)
            {
                sOtherTag = XmlUtil2.SelectRequiredSingleNodeAsString(oTagNode,
                    "text()", null);

                if ( !TryGetRelatedTagsRecursive(sOtherTag, eWhatToInclude,
                    eNetworkLevel, sApiKey, 2, oGraphMLXmlDocument,
                    oTagDictionary, oRequestStatistics, oBackgroundWorker,
                    oDoWorkEventArgs) )
                {
                    return (false);
                }
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryAppendSampleThumbnails()
    //
    /// <summary>
    /// Attempts to append sample thumbnails to the GraphMLXmlDocument being
    /// populated.
    /// </summary>
    ///
    /// <param name="oTagDictionary">
    /// The key is the tag name and the value is the corresponding GraphML XML
    /// node that represents the tag.
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
    /// true if the thumbnails were appended, or false if the user cancelled.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryAppendSampleThumbnails
    (
        Dictionary<String, XmlNode> oTagDictionary,
        GraphMLXmlDocument oGraphMLXmlDocument,
        String sApiKey,
        RequestStatistics oRequestStatistics,
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs
    )
    {
        Debug.Assert(oTagDictionary != null);
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert( !String.IsNullOrEmpty(sApiKey) );
        Debug.Assert(oRequestStatistics != null);
        Debug.Assert(oBackgroundWorker == null || oDoWorkEventArgs != null);
        AssertValid();

        foreach (KeyValuePair<String, XmlNode> oKeyValuePair in oTagDictionary)
        {
            if ( CancelIfRequested(oBackgroundWorker, oDoWorkEventArgs) )
            {
                return (false);
            }

            String sTag = oKeyValuePair.Key;

            ReportProgress("Getting sample image file for \"" + sTag + "\"");

            String sSampleImageUrl;

            if ( TryGetSampleImageUrl(sTag, sApiKey, oRequestStatistics,
                out sSampleImageUrl) )
            {
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                    oKeyValuePair.Value, ImageFileID, sSampleImageUrl);
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryGetSampleImageUrl()
    //
    /// <summary>
    /// Attempts to get an URL to a sample image for a tag.
    /// </summary>
    ///
    /// <param name="sTag">
    /// Tag to get a sample image URL for.
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
    /// <param name="sSampleImageUrl">
    /// Where the URL gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the sample image URL was obtained, false if some sort of error
    /// occurred while attempting to get it.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetSampleImageUrl
    (
        String sTag,
        String sApiKey,
        RequestStatistics oRequestStatistics,
        out String sSampleImageUrl
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sTag) );
        Debug.Assert( !String.IsNullOrEmpty(sApiKey) );
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        sSampleImageUrl = null;

        String sUrl = GetFlickrMethodUrl( "flickr.tags.getClusterPhotos",
            sApiKey, "&tag=" + UrlUtil.EncodeUrlParameter(sTag) );

        XmlDocument oXmlDocument;
        String sPhotoID;
        
        if (
            !TryGetXmlDocument(sUrl, oRequestStatistics, out oXmlDocument)
            ||
            ! XmlUtil2.TrySelectSingleNodeAsString(oXmlDocument,
                "rsp/photos/photo/@id", null, out sPhotoID)
            )
        {
            return (false);
        }

        sUrl = GetFlickrMethodUrl( "flickr.photos.getSizes", sApiKey,
            "&photo_id=" + UrlUtil.EncodeUrlParameter(sPhotoID) );

        if (
            !TryGetXmlDocument(sUrl, oRequestStatistics, out oXmlDocument)
            ||
            ! XmlUtil2.TrySelectSingleNodeAsString(oXmlDocument,
                "rsp/sizes/size[@label='Thumbnail']/@source", null,
                out sSampleImageUrl)
            )
        {
            return (false);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: AppendVertexXmlNode()
    //
    /// <summary>
    /// Appends a vertex XML node to the document for a tag if such a node
    /// doesn't already exist.
    /// </summary>
    ///
    /// <param name="sTag">
    /// Tag to add a vertex XML node for.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oTagDictionary">
    /// The key is the tag name and the value is the corresponding GraphML XML
    /// node that represents the tag.
    /// </param>
    //*************************************************************************

    protected void
    AppendVertexXmlNode
    (
        String sTag,
        GraphMLXmlDocument oGraphMLXmlDocument,
        Dictionary<String, XmlNode> oTagDictionary
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sTag) );
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oTagDictionary != null);

        if ( !oTagDictionary.ContainsKey(sTag) )
        {
            XmlNode oVertexXmlNode = oGraphMLXmlDocument.AppendVertexXmlNode(
                sTag);

            oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode,
                LabelID, sTag);

            oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode,
                MenuTextID, "Open Flickr Page for This Tag");

            oGraphMLXmlDocument.AppendGraphMLAttributeValue( oVertexXmlNode,
                MenuActionID,
                
                String.Format(
                    "http://www.flickr.com/photos/tags/{0}/"
                    ,
                    UrlUtil.EncodeUrlParameter(sTag)
                ) );

            oTagDictionary.Add(sTag, oVertexXmlNode);
        }
    }

    //*************************************************************************
    //  Method: CreateGraphMLXmlDocument()
    //
    /// <summary>
    /// Creates a GraphMLXmlDocument representing a network of Flickr related
    /// tags.
    /// </summary>
    ///
    /// <param name="bIncludeSampleThumbnails">
    /// true to include a sample thumbnail for each tag.
    /// photos.
    /// </param>
    ///
    /// <returns>
    /// A GraphMLXmlDocument representing a network of Flickr related tags.
    /// The document includes GraphML-attribute definitions but no vertices or
    /// edges.
    /// </returns>
    //*************************************************************************

    protected GraphMLXmlDocument
    CreateGraphMLXmlDocument
    (
        Boolean bIncludeSampleThumbnails
    )
    {
        AssertValid();

        GraphMLXmlDocument oGraphMLXmlDocument = new GraphMLXmlDocument(true);

        DefineLabelGraphMLAttribute(oGraphMLXmlDocument);
        DefineCustomMenuGraphMLAttributes(oGraphMLXmlDocument);

        if (bIncludeSampleThumbnails)
        {
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
        
        if ( TryGetRelatedTagsInternal(oGetNetworkAsyncArgs.Tag,
            oGetNetworkAsyncArgs.WhatToInclude,
            oGetNetworkAsyncArgs.NetworkLevel,
            oGetNetworkAsyncArgs.ApiKey, oBackgroundWorker, e,
            out oGraphMLDocument) )
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
    //  Embedded class: GetNetworkAsyncArgs()
    //
    /// <summary>
    /// Contains the arguments needed to asynchronously get the Flickr tags
    /// related to a specified tag.
    /// </summary>
    //*************************************************************************

    protected class GetNetworkAsyncArgs : GetNetworkAsyncArgsBase
    {
        ///
        public String Tag;
        ///
        public WhatToInclude WhatToInclude;
        ///
        public NetworkLevel NetworkLevel;
    };
}

}
