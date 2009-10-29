
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
        // (Do nothing.)

        AssertValid();
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

        // Here is the XML created by this method, assuming that
        // bIncludeStatistics and bIncludeLatestStatuses are true, and that
        // this.AppendVertexXmlNode() and
        // GraphMLXmlDocument.AppendEdgeXmlNode() are called after this method
        // returns.
        //
        /*
        <?xml version="1.0" encoding="UTF-8"?>
        <graphml xmlns="http://graphml.graphdrawing.org/xmlns">

            <key id="Followed" for="node" attr.name="Followed"
                attr.type="int" />
            <key id="Followers" for="node" attr.name="Followers"
                attr.type="int" />
            <key id="Statuses" for="node" attr.name="Tweets" attr.type="int" />
            <key id="LatestStatus" for="node" attr.name="Latest Tweet"
                attr.type="string" />
            <key id="ImageUrl" for="node" attr.name="Image File"
                attr.type="string" />
            <key id="MenuText" for="node" attr.name="Custom Menu Item Text"
                attr.type="string" />
            <key id="MenuAction" for="node" attr.name="Custom Menu Item Action"
                attr.type="string" />
            <key id="Relationship" for="edge" attr.name="Relationship"
                attr.type="string" />

            <graph edgedefault="directed">
                <node id="ScreenName1">
                    <data key="Followed">9</data>
                    <data key="Followers">10</data>
                    <data key="Statuses">3</data>
                    <data key="LatestStatus">Hello...</data> 
                    <data key="ImageUrl">http://...</data> 
                    <data key="MenuText">Open Twitter Page for This Person
                        </data>
                    <data key="MenuAction">http://twitter.com/ScreenName1</data>
                </node>
                <node id="ScreenName2">
                    <data key="Followed">98</data>
                    <data key="Followers">248</data>
                    <data key="Statuses">469</data>
                    <data key="LatestStatus">Goodbye...</data> 
                    <data key="ImageUrl">http://...</data> 
                    <data key="MenuText">Open Twitter Page for This Person
                        </data>
                    <data key="MenuAction">http://twitter.com/ScreenName2</data>
                </node>
                ...
                <edge source="ScreenName1" target="ScreenName2">
                    <data key="Relationship">Followed</data>
                </edge>
                ...
            </graph>
        </graphml>
        */

        GraphMLXmlDocument oGraphMLXmlDocument = new GraphMLXmlDocument(true);

        if (bIncludeStatistics)
        {
            oGraphMLXmlDocument.DefineGraphMLAttribute(false, FollowedID,
                "Followed", "int", null);

            oGraphMLXmlDocument.DefineGraphMLAttribute(false, FollowersID,
                "Followers", "int", null);

            oGraphMLXmlDocument.DefineGraphMLAttribute(false, StatusesID,
                "Tweets", "int", null);
        }

        if (bIncludeLatestStatuses)
        {
            oGraphMLXmlDocument.DefineGraphMLAttribute(false, LatestStatusID,
                "Latest Tweet", "string", null);
        }

        oGraphMLXmlDocument.DefineGraphMLAttribute(false, ImageUrlID,
            "Image File", "string", null);

        oGraphMLXmlDocument.DefineGraphMLAttribute(false, MenuTextID,
            "Custom Menu Item Text", "string", null);

        oGraphMLXmlDocument.DefineGraphMLAttribute(false, MenuActionID,
            "Custom Menu Item Action", "string", null);

        oGraphMLXmlDocument.DefineGraphMLAttribute(true, RelationshipID,
            "Relationship", "string", null);

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

            "Getting people {0} \"{1}\""
            ,
            bFollowed ? "followed by" : "following",
            sScreenName
            ) );
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
        // Debug.WriteLine("sUrl=" + sUrl);
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

            WRONG: oHttpWebRequest.Credentials = new NetworkCredential(
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
    /// <param name="bSkipUnauthorizedAndNotFoundErrors">
    /// If true, HTTP "unauthorized" (401) and "not found" (404) errors are
    /// skipped over.  If false, they are rethrown.
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
        Boolean bSkipUnauthorizedAndNotFoundErrors,
        String sCredentialsScreenName,
        String sCredentialsPassword
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUrl) );
        Debug.Assert( !String.IsNullOrEmpty(sXPath) );
        Debug.Assert(iMaximumPages > 0);
        Debug.Assert(iMaximumXmlNodes > 0);

        Debug.Assert( sCredentialsScreenName == null ||
            ( sCredentialsScreenName.Length > 0 &&
                !String.IsNullOrEmpty(sCredentialsPassword) ) );

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
                    sCredentialsScreenName, sCredentialsPassword,
                    this.HttpWebRequestRetries);
            }
            catch (WebException oWebException)
            {
                if (bSkipUnauthorizedAndNotFoundErrors)
                {
                    // An "unauthorized" error occurs when information about a
                    // user who has "protected" status is requested, for
                    // example.
                    //
                    // "Not found" errors can occur when the Twitter search API
                    // returns a tweet from a user, but then refuses to provide
                    // a list of the people followed by the user, probably
                    // because the user has protected her account.  (But if she
                    // has protected her account, why is the search API
                    // returning one of her tweets?)

                    if (
                        WebExceptionHasHttpStatusCode(oWebException,
                            HttpStatusCode.Unauthorized)
                        ||
                        WebExceptionHasHttpStatusCode(oWebException,
                            HttpStatusCode.NotFound)
                        )
                    {
                        yield break;
                    }
                }

                throw (oWebException);
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
                XmlNode oNextCursorXmlNode =
                    oXmlDocument.DocumentElement.SelectSingleNode(
                        "next_cursor", oXmlNamespaceManager);

                if (oNextCursorXmlNode == null)
                {
                    yield break;
                }

                sCursor = oNextCursorXmlNode.InnerText;

                // A next_cursor value of 0 means "end of data."

                if (String.IsNullOrEmpty(sCursor) || sCursor == "0")
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
    /// Where the screen name gets stored if true is returned.
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

        return ( XmlUtil.GetStringNodeValue(oUserXmlNode, "screen_name", false,
            out sScreenName) );
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
    /// The key is the screen name and the value is the corresponding
    /// TwitterVertex.
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
        oScreenNameDictionary.Add( sScreenName, oTwitterVertex);

        oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode,
            MenuTextID, "Open Twitter Page for This Person");

        oGraphMLXmlDocument.AppendGraphMLAttributeValue( oVertexXmlNode,
            MenuActionID, String.Format(WebPageUrlPattern, sScreenName) );

        if (oUserXmlNode != null)
        {
            AppendFromUserXmlNode(sScreenName, oUserXmlNode,
                oGraphMLXmlDocument, oTwitterVertex, bIncludeStatistics,
                bIncludeLatestStatus);
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
    /// The key is the screen name and the value is the corresponding
    /// TwitterVertex.
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
    //  Method: AppendEdgeXmlNode()
    //
    /// <summary>
    /// Appends an edge XML node to the GraphML document.
    /// </summary>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="sScreenName1">
    /// The edge's first screen name.
    /// </param>
    ///
    /// <param name="sScreenName2">
    /// The edge's second screen name.
    /// </param>
    ///
    /// <param name="sRelationship">
    /// The value of the edge's RelationshipID GraphML-attribute.
    /// </param>
    ///
    /// <returns>
    /// The new edge XML node.
    /// </returns>
    //*************************************************************************

    protected XmlNode
    AppendEdgeXmlNode
    (
        GraphMLXmlDocument oGraphMLXmlDocument,
        String sScreenName1,
        String sScreenName2,
        String sRelationship
    )
    {
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert( !String.IsNullOrEmpty(sScreenName1) );
        Debug.Assert( !String.IsNullOrEmpty(sScreenName2) );
        Debug.Assert( !String.IsNullOrEmpty(sRelationship) );
        AssertValid();

        XmlNode oEdgeXmlNode = oGraphMLXmlDocument.AppendEdgeXmlNode(
            sScreenName1, sScreenName2);

        oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode,
            RelationshipID, sRelationship);

        return (oEdgeXmlNode);
    }

    //*************************************************************************
    //  Method: AppendFromUserXmlNode()
    //
    /// <summary>
    /// Appends GraphML-Attribute values from a "user" XML node returned by
    /// Twitter to a vertex XML node.
    /// </summary>
    ///
    /// <param name="sScreenName">
    /// Screen name corresponding to the "user" XML node.
    /// </param>
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
        String sScreenName,
        XmlNode oUserXmlNode,
        GraphMLXmlDocument oGraphMLXmlDocument,
        TwitterVertex oTwitterVertex,
        Boolean bIncludeStatistics,
        Boolean bIncludeLatestStatus
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenName) );
        Debug.Assert(oUserXmlNode != null);
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oTwitterVertex != null);
        AssertValid();

        XmlNode oVertexXmlNode = oTwitterVertex.VertexXmlNode;

        if (bIncludeStatistics)
        {
            AppendInt32GraphMLAttributeValue(oUserXmlNode, "friends_count",
                oGraphMLXmlDocument, oVertexXmlNode, FollowedID);

            AppendInt32GraphMLAttributeValue(oUserXmlNode, "followers_count",
                oGraphMLXmlDocument, oVertexXmlNode, FollowersID);

            AppendInt32GraphMLAttributeValue(oUserXmlNode, "statuses_count",
                oGraphMLXmlDocument, oVertexXmlNode, StatusesID);
        }

        String sLatestStatus;

        if ( XmlUtil.GetStringNodeValue(oUserXmlNode, "status/text", false,
            out sLatestStatus) )
        {
            // Don't overwrite any status the derived class may have already
            // stored on the TwitterVertex object.

            if (oTwitterVertex.StatusForAnalysis == null)
            {
                oTwitterVertex.StatusForAnalysis = sLatestStatus;
            }

            if (bIncludeLatestStatus)
            {
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode,
                    LatestStatusID, sLatestStatus);
            }
        }

        // Add an image URL GraphML-attribute if it hasn't already been added.
        // (It might have been added from an "entry" node by
        // TwitterSearchNetworkAnalyzer.)

        if (oVertexXmlNode.SelectSingleNode(
            "a:data[@key='" + ImageUrlID + "']", 
            oGraphMLXmlDocument.CreateXmlNamespaceManager("a") ) == null)
        {
            String sImageUrl;

            if ( XmlUtil.GetStringNodeValue(oUserXmlNode, "profile_image_url",
                false, out sImageUrl) )
            {
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode,
                    ImageUrlID, sImageUrl);
            }
        }
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
    /// The key is the screen name and the value is the corresponding
    /// TwitterVertex.
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
        String sCredentialsScreenName,
        String sCredentialsPassword,
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs
    )
    {
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oScreenNameDictionary != null);

        Debug.Assert( sCredentialsScreenName == null ||
            ( sCredentialsScreenName.Length > 0 &&
                !String.IsNullOrEmpty(sCredentialsPassword) ) );

        Debug.Assert(oBackgroundWorker == null || oDoWorkEventArgs != null);
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
                UrlUtil.EncodeUrlParameter(sScreenName)
                );

            ReportProgress( String.Format(

                "Getting information about \"{0}\""
                ,
                sScreenName
                ) );

            XmlDocument oXmlDocument;
            
            try
            {
                oXmlDocument = GetXmlDocument(sUrl, sCredentialsScreenName,
                    sCredentialsPassword, this.HttpWebRequestRetries);
            }
            catch (WebException oWebException)
            {
                if (
                    WebExceptionHasHttpStatusCode(oWebException,
                        HttpStatusCode.Unauthorized)
                    ||
                    WebExceptionHasHttpStatusCode(oWebException,
                        HttpStatusCode.NotFound)
                    )
                {
                    continue;
                }

                throw (oWebException);
            }

            // The document consists of a single "user" node.

            XmlNode oUserXmlNode = oXmlDocument.SelectSingleNode("user");

            if (oUserXmlNode != null)
            {
                AppendFromUserXmlNode(sScreenName, oUserXmlNode,
                    oGraphMLXmlDocument, oTwitterVertex, bIncludeStatistics,
                    bIncludeLatestStatuses);
            }
        }
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
    /// The key is the screen name and the value is the corresponding
    /// TwitterVertex.
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

        foreach (KeyValuePair<String, TwitterVertex> oKeyValuePair1 in
            oScreenNameDictionary)
        {
            foreach (KeyValuePair<String, TwitterVertex> oKeyValuePair2 in
                oScreenNameDictionary)
            {
                String sStatusForAnalysis1 =
                    oKeyValuePair1.Value.StatusForAnalysis;

                if ( String.IsNullOrEmpty(sStatusForAnalysis1) )
                {
                    continue;
                }

                String sScreenName1 = oKeyValuePair1.Key;
                String sScreenName2 = oKeyValuePair2.Key;

                if (sScreenName1 == sScreenName2)
                {
                    continue;
                }

                String sScreenName2WithAt = "@" + sScreenName2;
                const String WhitespaceOrEndOfStatus = "(\\s|$)";

                // Does the status start with screen name 2, meaning it's a
                // reply-to?  The Regex here is "starts with @ScreenName2
                // followed by whitespace or the end of the status."

                if (
                    bIncludeRepliesToEdges
                    &&
                    Regex.IsMatch(sStatusForAnalysis1,
                        "^" + sScreenName2WithAt + WhitespaceOrEndOfStatus)
                    )
                {
                    AppendEdgeXmlNode(oGraphMLXmlDocument, sScreenName1,
                        sScreenName2, "Replies To");
                }

                // Does the status include screen name B, meaning it's a
                // "mentions"?  The Regex here is "whitespace followed by
                // @ScreenName2 followed by whitespace or the end of the
                // status."

                if (
                    bIncludeMentionsEdges
                    &&
                    Regex.IsMatch(sStatusForAnalysis1,
                        "\\s" + sScreenName2WithAt + WhitespaceOrEndOfStatus)
                    )
                {
                    AppendEdgeXmlNode(oGraphMLXmlDocument, sScreenName1,
                        sScreenName2, "Mentions");
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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// GraphML-attribute IDs.

    protected const String StatusesID = "Statuses";
    ///
    protected const String FollowedID = "Followed";
    ///
    protected const String FollowersID = "Followers";
    ///
    protected const String LatestStatusID = "LatestStatus";
    ///
    protected const String ImageUrlID = "ImageUrlID";
    ///
    protected const String RelationshipID = "Relationship";

    /// Format pattern for the URL of the Twitter Web page for a person.  The
    /// {0} argument must be replaced with a Twitter screen name.

    protected const String WebPageUrlPattern =
        "http://twitter.com/{0}";

    /// URI of the Atom namespace that Twitter uses in some of its responses.

    protected const String AtomNamespaceUri =
        "http://www.w3.org/2005/Atom";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)


    //*************************************************************************
    //  Embedded class: GetNetworkAsyncArguments()
    //
    /// <summary>
    /// Contains the arguments needed to asynchronously get a network.
    /// </summary>
    //*************************************************************************

    protected class GetNetworkAsyncArgs
    {
        ///
        public Int32 MaximumPeoplePerRequest;
        ///
        public String CredentialsScreenName;
        ///
        public String CredentialsPassword;
    };
}

}
