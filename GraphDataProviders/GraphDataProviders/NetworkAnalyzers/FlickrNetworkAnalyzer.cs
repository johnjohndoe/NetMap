
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Net;
using System.Web;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.XmlLib;

namespace Microsoft.NodeXL.GraphDataProviders.Flickr
{
//*****************************************************************************
//  Class: FlickrNetworkAnalyzer
//
/// <summary>
/// Analyzes a Flickr network.
/// </summary>
///
/// <remarks>
/// Use <see cref="GetRelatedTags" /> to synchronously get the directed
/// 1.5-degree network of Flickr tags that are related to a specified tag, or
/// use <see cref="GetRelatedTagsAsync" /> to do it asynchronously.
/// </remarks>
//*****************************************************************************

public class FlickrNetworkAnalyzer : HttpNetworkAnalyzerBase
{
    //*************************************************************************
    //  Constructor: FlickrNetworkAnalyzer()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="FlickrNetworkAnalyzer" />
    /// class.
    /// </summary>
    //*************************************************************************

    public FlickrNetworkAnalyzer()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: GetRelatedTags()
    //
    /// <summary>
    /// Synchronously gets the directed 1.5-degree network of Flickr tags
    /// related to a specified tag.
    /// </summary>
    ///
    /// <param name="tag">
    /// Tag to get related tags for.
    /// </param>
    ///
    /// <param name="apiKey">
    /// Flickr API key.
    /// </param>
    ///
    /// <returns>
    /// An XmlDocument containing the network as GraphML.
    /// </returns>
    //*************************************************************************

    public XmlDocument
    GetRelatedTags
    (
        String tag,
        String apiKey
    )
    {
        AssertValid();

        XmlDocument oXmlDocument;

        Boolean bNotCancelled = TryGetRelatedTagsInternal(tag, apiKey, null,
			null, out oXmlDocument);

        Debug.Assert(bNotCancelled);

        return (oXmlDocument);
    }

    //*************************************************************************
    //  Method: GetRelatedTagsAsync()
    //
    /// <summary>
    /// Asynchronously gets the directed 1.5-degree network of Flickr tags
    /// related to a specified tag.
    /// </summary>
    ///
    /// <param name="tag">
    /// Tag to get related tags for.
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
    GetRelatedTagsAsync
    (
        String tag,
        String apiKey
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(tag) );
        Debug.Assert( !String.IsNullOrEmpty(apiKey) );
        AssertValid();

        const String MethodName = "GetRelatedTagsAsync";
        CheckIsBusy(MethodName);

        // Wrap the arguments in an object that can be passed to
        // BackgroundWorker.RunWorkerAsync().

        GetRelatedTagsAsyncArgs oGetRelatedTagsAsyncArgs =
            new GetRelatedTagsAsyncArgs();

        oGetRelatedTagsAsyncArgs.Tag = tag;
        oGetRelatedTagsAsyncArgs.ApiKey = apiKey;

        m_oBackgroundWorker.RunWorkerAsync(oGetRelatedTagsAsyncArgs);
    }

    //*************************************************************************
    //  Method: TryGetRelatedTagsInternal()
    //
    /// <summary>
    /// Attempts to gets the Flickr tags related to a specified tag.
    /// </summary>
    ///
    /// <param name="sTag">
    /// Tag to get related tags for.
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
        String sApiKey,
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs,
        out XmlDocument oXmlDocument
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sTag) );
        Debug.Assert( !String.IsNullOrEmpty(sApiKey) );
        Debug.Assert(oBackgroundWorker == null || oDoWorkEventArgs != null);
        AssertValid();

        oXmlDocument = null;

        // The key is the tag name and the value isn't used.  This is used to
        // prevent the same tag from being added to the XmlDocument twice.

        Dictionary<String, Byte> oTagDictionary =
            new Dictionary<String, Byte>();

        // Here is the XML created by this method:
        //
        /*
        <?xml version="1.0" encoding="UTF-8"?>
        <graphml xmlns="http://graphml.graphdrawing.org/xmlns">

            <key id="Label" for="node" attr.name="Primary Label"
                attr.type="string" />
            <key id="MenuText" for="node" attr.name="Custom Menu Item Text"
                attr.type="string" />
            <key id="MenuAction" for="node" attr.name="Custom Menu Item Action"
                attr.type="string" />
            </key>

            <graph edgedefault="directed">
                <node id="Tag1">
                    <data key="Label">Tag1</data>
                    <data key="MenuText">Open Flickr Page for This Tag</data>
                    <data key="MenuAction">http://www.flickr.com/...</data>
                </node>
                <node id="Tag2">
                    <data key="Label">Tag2</data>
                    <data key="MenuText">Open Flickr Page for This Tag</data>
                    <data key="MenuAction">http://www.flickr.com/...</data>
                </node>
                <node id="Tag3">
                    <data key="Label">Tag3</data>
                    <data key="MenuText">Open Flickr Page for This Tag</data>
                    <data key="MenuAction">http://www.flickr.com/...</data>
                </node>
                ...

                <edge source="Tag1" target="Tag2" />
                <edge source="Tag1" target="Tag3" />
                ...
            </graph>
        </graphml>
        */

        GraphMLXmlDocument oGraphMLXmlDocument = new GraphMLXmlDocument(true);

        oGraphMLXmlDocument.DefineGraphMLAttribute(false, PrimaryLabelID,
            "Primary Label", "string", null);

        oGraphMLXmlDocument.DefineGraphMLAttribute(false, MenuTextID,
            "Custom Menu Item Text", "string", null);

        oGraphMLXmlDocument.DefineGraphMLAttribute(false, MenuActionID,
            "Custom Menu Item Action", "string", null);

        if ( TryGetRelatedTagsRecursive(sTag, sApiKey, 1, oGraphMLXmlDocument,
            oTagDictionary, oBackgroundWorker, oDoWorkEventArgs) )
        {
            oXmlDocument = oGraphMLXmlDocument;
            return (true);
        }

        return (false);
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
    /// The key is the tag name and the value isn't used.
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
        String sApiKey,
        Int32 iRecursionLevel,
        GraphMLXmlDocument oGraphMLXmlDocument,
        Dictionary<String, Byte> oTagDictionary,
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sTag) );
        Debug.Assert( !String.IsNullOrEmpty(sApiKey) );
        Debug.Assert(iRecursionLevel == 1 || iRecursionLevel == 2);
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oTagDictionary != null);
        Debug.Assert(oBackgroundWorker == null || oDoWorkEventArgs != null);
        AssertValid();

        if ( CancelIfRequested(oBackgroundWorker, oDoWorkEventArgs) )
        {
            return (false);
        }

        String sUrl = String.Format(
            GetRelatedTagsUrlPattern
            ,
            HttpUtility.UrlEncode(sApiKey),
            HttpUtility.UrlEncode(sTag)
            );

        XmlDocument oFlickrXmlDocument = GetXmlDocument(sUrl,
            m_iHttpWebRequestRetries);

        // The document consists of a single "tags" node with zero or more
        // "tag" child nodes.

        String sOtherTag = null;

        XmlNodeList oTagNodes =
            oFlickrXmlDocument.DocumentElement.SelectNodes("tags/tag");

        if (oTagNodes.Count > 0)
        {
            AppendVertexXmlNode(sTag, oGraphMLXmlDocument, oTagDictionary);
        }

        foreach (XmlNode oTagNode in oTagNodes)
        {
            XmlUtil.GetInnerText(oTagNode, true, out sOtherTag);

            if (iRecursionLevel == 1)
            {
                // All tags related to sTag, which is the top-level tag, should
                // be included and connected to sTag by an edge.

                AppendVertexXmlNode(sOtherTag, oGraphMLXmlDocument,
                    oTagDictionary);

                oGraphMLXmlDocument.AppendEdgeXmlNode(sTag, sOtherTag);
            }
            else
            {
                Debug.Assert(iRecursionLevel == 2);

                // Because we're seeking only the 1.5-degree network, only tags
                // already related to sTag should be connected by an edge.

                if ( oTagDictionary.ContainsKey(sOtherTag) )
                {
                    oGraphMLXmlDocument.AppendEdgeXmlNode(sTag, sOtherTag);
                }
            }
        }

        if (iRecursionLevel == 1)
        {
            foreach (XmlNode oTagNode in oTagNodes)
            {
                XmlUtil.GetInnerText(oTagNode, true, out sOtherTag);

                if ( !TryGetRelatedTagsRecursive(sOtherTag, sApiKey, 2,
                    oGraphMLXmlDocument, oTagDictionary, oBackgroundWorker,
                    oDoWorkEventArgs) )
                {
                    return (false);
                }
            }
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
    /// The key is the tag name and the value isn't used.
    /// </param>
    //*************************************************************************

    protected void
    AppendVertexXmlNode
    (
        String sTag,
        GraphMLXmlDocument oGraphMLXmlDocument,
        Dictionary<String, Byte> oTagDictionary
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sTag) );
        Debug.Assert(oTagDictionary != null);
        Debug.Assert(oGraphMLXmlDocument != null);

        if ( !oTagDictionary.ContainsKey(sTag) )
        {
            XmlNode oVertexXmlNode = oGraphMLXmlDocument.AppendVertexXmlNode(
                sTag);

            oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode,
                PrimaryLabelID, sTag);

            oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode,
                MenuTextID, "Open Flickr Page for This Tag");

            oGraphMLXmlDocument.AppendGraphMLAttributeValue( oVertexXmlNode,
                MenuActionID, String.Format(WebPageUrlPattern, sTag) );

            oTagDictionary.Add(sTag, 0);
        }
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
        Int32 iRetries
    )
    {
        Debug.Assert(iRetries >= 0);
        AssertValid();

        while (true)
        {
            try
            {
                return ( GetXmlDocument(sUrl) );
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
    /// <returns>
    /// The XmlDocument from the URL.
    /// </returns>
    //*************************************************************************

    protected XmlDocument
    GetXmlDocument
    (
        String sUrl
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUrl) );
        AssertValid();

        XmlDocument oXmlDocument = GetXmlDocument(
            (HttpWebRequest)WebRequest.Create(sUrl) );

        XmlNode oRspNode = oXmlDocument.DocumentElement;

        String sStatus;

        XmlUtil.GetAttribute(oRspNode, "stat", true, out sStatus);

        if (sStatus != "ok")
        {
            XmlNode oErrNode = XmlUtil.SelectRequiredSingleNode(oRspNode,
                "err");

            String sErrorMessage;

            XmlUtil.GetAttribute(oErrNode, "msg", true, out sErrorMessage);
            throw new WebException(sErrorMessage);
        }

        return (oXmlDocument);
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

        Debug.Assert(e.Argument is GetRelatedTagsAsyncArgs);

        GetRelatedTagsAsyncArgs oGetRelatedTagsAsyncArgs =
            (GetRelatedTagsAsyncArgs)e.Argument;

        XmlDocument oGraphMLDocument;
        
        if ( TryGetRelatedTagsInternal(oGetRelatedTagsAsyncArgs.Tag,
            oGetRelatedTagsAsyncArgs.ApiKey, oBackgroundWorker, e,
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
    //  Protected constants
    //*************************************************************************

    /// Format pattern for the URL of the Flickr Web service getRelatedTags
    /// method.  The arguments must be replaced.

    const String GetRelatedTagsUrlPattern =
        "http://api.flickr.com/services/rest/?method="
        + "flickr.tags.getRelated&api_key={0}&tag={1}"
        ;

    /// GraphML-attribute IDs.

    protected const String PrimaryLabelID = "Label";
    ///
    protected const String MenuTextID = "MenuText";
    ///
    protected const String MenuActionID = "MenuAction";

    /// Format pattern for the URL of the Flickr Web page for a tag.  The {0}
    /// argument must be replaced with the tag name.

    protected const String WebPageUrlPattern =
        "http://www.flickr.com/photos/tags/{0}/";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)


    //*************************************************************************
    //  Embedded class: GetRelatedTagsAsyncArguments()
    //
    /// <summary>
    /// Contains the arguments needed to asynchronously get the Flickr tags
    /// related to a specified tag.
    /// </summary>
    //*************************************************************************

    protected class GetRelatedTagsAsyncArgs
    {
        ///
        public String Tag;
        ///
        public String ApiKey;
    };
}

}
