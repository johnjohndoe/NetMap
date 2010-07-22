
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Xml;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.SocialNetworkLib;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.XmlLib;

namespace Microsoft.NodeXL.GraphDataProviders
{
//*****************************************************************************
//  Class: HttpNetworkAnalyzerBase
//
/// <summary>
/// Abstract base class for classes that analyze network information obtained
/// via HTTP Web requests.
/// </summary>
///
/// <remarks>
/// This base class implements properties related to HTTP Web requests, a
/// BackgroundWorker instance, and properties, methods, and events related to
/// the BackgroundWorker.  The derived class must implement a method to start
/// an analysis and implement the <see cref="BackgroundWorker_DoWork" />
/// method.
/// </remarks>
//*****************************************************************************

public abstract class HttpNetworkAnalyzerBase : Object
{
    //*************************************************************************
    //  Constructor: HttpNetworkAnalyzerBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="HttpNetworkAnalyzerBase" /> class.
    /// </summary>
    //*************************************************************************

    public HttpNetworkAnalyzerBase()
    {
        m_oBackgroundWorker = new BackgroundWorker();
        m_oBackgroundWorker.WorkerSupportsCancellation = true;
        m_oBackgroundWorker.WorkerReportsProgress = true;

        m_oBackgroundWorker.DoWork += new DoWorkEventHandler(
            BackgroundWorker_DoWork);

        m_oBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(
            BackgroundWorker_ProgressChanged);

        m_oBackgroundWorker.RunWorkerCompleted +=
            new RunWorkerCompletedEventHandler(
                BackgroundWorker_RunWorkerCompleted);

        // AssertValid();
    }

    //*************************************************************************
    //  Property: IsBusy
    //
    /// <summary>
    /// Gets a flag indicating whether an asynchronous operation is in
    /// progress.
    /// </summary>
    ///
    /// <value>
    /// true if an asynchronous operation is in progress.
    /// </value>
    //*************************************************************************

    public Boolean
    IsBusy
    {
        get
        {
            return (m_oBackgroundWorker.IsBusy);
        }
    }

    //*************************************************************************
    //  Method: CancelAsync()
    //
    /// <summary>
    /// Cancels the analysis started by an async method.
    /// </summary>
    ///
    /// <remarks>
    /// When the analysis cancels, the <see cref="AnalysisCompleted" /> event
    /// fires.  The <see cref="AsyncCompletedEventArgs.Cancelled" /> property
    /// will be true.
    ///
    /// <para>
    /// Important note: If the background thread started by an async method
    /// is running a Web request when <see cref="CancelAsync" /> is called, the
    /// cancel won't occur until the request completes.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    CancelAsync()
    {
        AssertValid();

        if (this.IsBusy)
        {
            m_oBackgroundWorker.CancelAsync();
        }
    }

    //*************************************************************************
    //  Event: ProgressChanged
    //
    /// <summary>
    /// Occurs when progress is reported.
    /// </summary>
    //*************************************************************************

    public event ProgressChangedEventHandler ProgressChanged;


    //*************************************************************************
    //  Event: AnalysisCompleted
    //
    /// <summary>
    /// Occurs when the analysis started by an async method completes, is
    /// cancelled, or encounters an error.
    /// </summary>
    //*************************************************************************

    public event RunWorkerCompletedEventHandler AnalysisCompleted;


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

    public abstract String
    ExceptionToMessage
    (
        Exception oException
    );


    //*************************************************************************
    //  Property: ClassName
    //
    /// <summary>
    /// Gets the full name of this class.
    /// </summary>
    ///
    /// <value>
    /// The full name of this class, suitable for use in error messages.
    /// </value>
    //*************************************************************************

    protected String
    ClassName
    {
        get
        {
            return (this.GetType().FullName);
        }
    }

    //*************************************************************************
    //  Method: CheckIsBusy()
    //
    /// <summary>
    /// Throws an exception if an asynchronous operation is in progress.
    /// </summary>
    ///
    /// <param name="sMethodName">
    /// Name of the calling method.
    /// </param>
    //*************************************************************************

    protected void
    CheckIsBusy
    (
        String sMethodName
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sMethodName) );

        if (this.IsBusy)
        {
            throw new InvalidOperationException( String.Format(

                "{0}:{1}: An asynchronous operation is already in progress."
                ,
                this.ClassName,
                sMethodName
                ) );
        }
    }

    //*************************************************************************
    //  Method: GetXmlDocumentWithRetries()
    //
    /// <summary>
    /// Gets an XML document given an URL.  Retries after an error.
    /// </summary>
    ///
    /// <param name="sUrl">
    /// URL to use.
    /// </param>
    ///
    /// <param name="aeHttpStatusCodesToFailImmediately">
    /// An array of status codes that should be failed immediately, or null to
    /// retry all failures.  An example is HttpStatusCode.Unauthorized (401),
    /// which Twitter returns when information about a user who has "protected"
    /// status is requested.  This should not be retried, because the retries
    /// would produce exactly the same error response.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <param name="asOptionalHeaderNameValuePairs">
    /// Array of name/value pairs for HTTP headers to add to the request, or
    /// null to not add any pairs.  Sample: {"Authorization", "Basic 36A4E798"}
    /// </param>
    ///
    /// <returns>
    /// The XmlDocument.
    /// </returns>
    ///
    /// <remarks>
    /// If the request fails and the HTTP status code is not one of the codes
    /// specified in <paramref name="aeHttpStatusCodesToFailImmediately" />,
    /// the request is retried.  If the retries also fail, an exception is
    /// thrown.
    ///
    /// <para>
    /// If the request fails with one of the HTTP status code contained in
    /// <paramref name="aeHttpStatusCodesToFailImmediately" />, an exception is
    /// thrown immediately.
    /// </para>
    ///
    /// <para>
    /// In either case, it is always up to the caller to handle the exceptions.
    /// This method never ignores an exception; it either retries it and throws
    /// it if all retries fail, or throws it immediately.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected XmlDocument
    GetXmlDocumentWithRetries
    (
        String sUrl,
        HttpStatusCode [] aeHttpStatusCodesToFailImmediately,
        RequestStatistics oRequestStatistics,
        params String[] asOptionalHeaderNameValuePairs
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUrl) );
        Debug.Assert(oRequestStatistics != null);

        Debug.Assert(asOptionalHeaderNameValuePairs == null ||
            asOptionalHeaderNameValuePairs.Length % 2 == 0);

        AssertValid();

        Int32 iMaximumRetries = HttpRetryDelaysSec.Length;
        Int32 iRetriesSoFar = 0;

        while (true)
        {
            if (iRetriesSoFar > 0)
            {
                ReportProgress("Retrying request");
            }

            // Important Note: You cannot use the same HttpWebRequest object
            // for the retries.  The object must be recreated each time.

            HttpWebRequest oHttpWebRequest =
                (HttpWebRequest)WebRequest.Create(sUrl);

            Int32 iHeaderNamesAndValues =
                (asOptionalHeaderNameValuePairs == null) ?
                0 : asOptionalHeaderNameValuePairs.Length;

            for (Int32 i = 0; i < iHeaderNamesAndValues; i += 2)
            {
                String sHeaderName = asOptionalHeaderNameValuePairs[i + 0];
                String sHeaderValue = asOptionalHeaderNameValuePairs[i + 1];

                Debug.Assert( !String.IsNullOrEmpty(sHeaderName) );
                Debug.Assert( !String.IsNullOrEmpty(sHeaderValue) );

                oHttpWebRequest.Headers[sHeaderName] = sHeaderValue;
            }

            try
            {
                XmlDocument oXmlDocument =
                    GetXmlDocumentNoRetries(oHttpWebRequest);

                if (iRetriesSoFar > 0)
                {
                    ReportProgress("Retry succeeded, continuing...");
                }

                oRequestStatistics.OnSuccessfulRequest();

                return (oXmlDocument);
            }
            catch (Exception oException)
            {
                if ( !ExceptionIsWebOrXml(oException) )
                {
                    throw oException;
                }

                // A WebException or XmlException has occurred.

                if (iRetriesSoFar == iMaximumRetries)
                {
                    oRequestStatistics.OnUnexpectedException(oException);

                    throw (oException);
                }

                // If the status code is one of the ones specified in
                // aeHttpStatusCodesToFailImmediately, rethrow the exception
                // without retrying the request.

                if (aeHttpStatusCodesToFailImmediately != null &&
                    oException is WebException)
                {
                    if ( WebExceptionHasHttpStatusCode(
                            (WebException)oException,
                            aeHttpStatusCodesToFailImmediately) )
                    {
                        throw (oException);
                    }
                }

                Int32 iSeconds = HttpRetryDelaysSec[iRetriesSoFar];

                ReportProgress( String.Format(

                    "Request failed, pausing {0} {1} before retrying..."
                    ,
                    iSeconds,
                    StringUtil.MakePlural("second", iSeconds)
                    ) );

                System.Threading.Thread.Sleep(1000 * iSeconds);
                iRetriesSoFar++;
            }
        }
    }

    //*************************************************************************
    //  Method: GetXmlDocumentNoRetries()
    //
    /// <summary>
    /// Gets an XML document given an HttpWebRequest object.  Does not retry
    /// after an error.
    /// </summary>
    ///
    /// <param name="oHttpWebRequest">
    /// HttpWebRequest object to use.
    /// </param>
    ///
    /// <returns>
    /// The XmlDocument.
    /// </returns>
    ///
    /// <remarks>
    /// This method sets several properties on <paramref
    /// name="oHttpWebRequest" /> before it is used.
    /// </remarks>
    //*************************************************************************

    protected XmlDocument
    GetXmlDocumentNoRetries
    (
        HttpWebRequest oHttpWebRequest
    )
    {
        Debug.Assert(oHttpWebRequest != null);
        AssertValid();

        oHttpWebRequest.Timeout = HttpWebRequestTimeoutMs;

        // According to the Twitter API documentation, "Consumers using the
        // Search API but failing to include a User Agent string will
        // receive a lower rate limit."

        oHttpWebRequest.UserAgent = UserAgent;

        // This is to prevent "The request was aborted: The request was
        // canceled" WebExceptions that arose for Twitter on at least one
        // user's machine, at the expense of performance.  This is not a good
        // solution, but see this posting:
        //
        // http://arnosoftwaredev.blogspot.com/2006/09/
        // net-20-httpwebrequestkeepalive-and.html

        oHttpWebRequest.KeepAlive = false;

        HttpWebResponse oHttpWebResponse = null;
        Stream oStream = null;
        XmlDocument oXmlDocument;

        try
        {
            oHttpWebResponse = (HttpWebResponse)oHttpWebRequest.GetResponse();
            oStream = oHttpWebResponse.GetResponseStream();

            oXmlDocument = new XmlDocument();
            oXmlDocument.Load(oStream);
        }
        finally
        {
            if (oStream != null)
            {
                oStream.Close();
            }

            if (oHttpWebResponse != null)
            {
                oHttpWebResponse.Close();
            }
        }

        return (oXmlDocument);
    }

    //*************************************************************************
    //  Method: DefineImageFileGraphMLAttribute()
    //
    /// <summary>
    /// Defines a GraphML-Attribute for vertex image files.
    /// </summary>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    //*************************************************************************

    protected void
    DefineImageFileGraphMLAttribute
    (
        GraphMLXmlDocument oGraphMLXmlDocument
    )
    {
        Debug.Assert(oGraphMLXmlDocument != null);
        AssertValid();

        oGraphMLXmlDocument.DefineGraphMLAttribute(false, ImageFileID,
            ImageColumnName, "string", null);
    }

    //*************************************************************************
    //  Method: DefineLabelGraphMLAttribute()
    //
    /// <summary>
    /// Defines a GraphML-Attribute for vertex labels.
    /// </summary>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    //*************************************************************************

    protected void
    DefineLabelGraphMLAttribute
    (
        GraphMLXmlDocument oGraphMLXmlDocument
    )
    {
        Debug.Assert(oGraphMLXmlDocument != null);
        AssertValid();

        oGraphMLXmlDocument.DefineGraphMLAttribute(false, LabelID,
            LabelColumnName, "string", null);
    }

    //*************************************************************************
    //  Method: DefineCustomMenuGraphMLAttributes()
    //
    /// <summary>
    /// Defines the GraphML-Attributes for custom menu items.
    /// </summary>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    //*************************************************************************

    protected void
    DefineCustomMenuGraphMLAttributes
    (
        GraphMLXmlDocument oGraphMLXmlDocument
    )
    {
        Debug.Assert(oGraphMLXmlDocument != null);
        AssertValid();

        oGraphMLXmlDocument.DefineGraphMLAttribute(false, MenuTextID,
            MenuTextColumnName, "string", null);

        oGraphMLXmlDocument.DefineGraphMLAttribute(false, MenuActionID,
            MenuActionColumnName, "string", null);
    }

    //*************************************************************************
    //  Method: DefineRelationshipGraphMLAttribute()
    //
    /// <summary>
    /// Defines a GraphML-Attribute for edge relationships.
    /// </summary>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    //*************************************************************************

    protected void
    DefineRelationshipGraphMLAttribute
    (
        GraphMLXmlDocument oGraphMLXmlDocument
    )
    {
        Debug.Assert(oGraphMLXmlDocument != null);
        AssertValid();

        oGraphMLXmlDocument.DefineGraphMLAttribute(true, RelationshipID,
            "Relationship", "string", null);
    }

    //*************************************************************************
    //  Method: AppendStringGraphMLAttributeValue()
    //
    /// <summary>
    /// Appends a String GraphML-Attribute value to an edge or vertex XML node. 
    /// </summary>
    ///
    /// <param name="oXmlNodeToSelectFrom">
    /// Node to select from.
    /// </param>
    /// 
    /// <param name="sXPath">
    /// XPath expression to a String descendant of <paramref
    /// name="oXmlNodeToSelectFrom" />.
    /// </param>
    ///
    /// <param name="oXmlNamespaceManager">
    /// NamespaceManager to use, or null to not use one.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oEdgeOrVertexXmlNode">
    /// The edge or vertex XML node from <paramref
    /// name="oGraphMLXmlDocument" /> to add the GraphML attribute value to.
    /// </param>
    ///
    /// <param name="sGraphMLAttributeID">
    /// GraphML ID of the attribute.
    /// </param>
    ///
    /// <returns>
    /// true if the GraphML-Attribute was appended.
    /// </returns>
    ///
    /// <remarks>
    /// This method selects from <paramref name="oXmlNodeToSelectFrom" /> using
    /// the <paramref name="sXPath" /> expression.  If the selection is
    /// successful, the specified String value gets stored on <paramref
    /// name="oEdgeOrVertexXmlNode" /> as a Graph-ML Attribute.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    AppendStringGraphMLAttributeValue
    (
        XmlNode oXmlNodeToSelectFrom,
        String sXPath,
        XmlNamespaceManager oXmlNamespaceManager,
        GraphMLXmlDocument oGraphMLXmlDocument,
        XmlNode oEdgeOrVertexXmlNode,
        String sGraphMLAttributeID
    )
    {
        Debug.Assert(oXmlNodeToSelectFrom != null);
        Debug.Assert( !String.IsNullOrEmpty(sXPath) );
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oEdgeOrVertexXmlNode != null);
        Debug.Assert( !String.IsNullOrEmpty(sGraphMLAttributeID) );
        AssertValid();

        String sAttributeValue;

        if ( XmlUtil2.TrySelectSingleNodeAsString(oXmlNodeToSelectFrom, sXPath,
            oXmlNamespaceManager, out sAttributeValue) )
        {
            oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                oEdgeOrVertexXmlNode, sGraphMLAttributeID, sAttributeValue);

            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: AppendInt32GraphMLAttributeValue()
    //
    /// <summary>
    /// Appends an Int32 GraphML-Attribute value to an edge or vertex XML node. 
    /// </summary>
    ///
    /// <param name="oXmlNodeToSelectFrom">
    /// Node to select from.
    /// </param>
    /// 
    /// <param name="sXPath">
    /// XPath expression to an Int32 descendant of <paramref
    /// name="oXmlNodeToSelectFrom" />.
    /// </param>
    ///
    /// <param name="oXmlNamespaceManager">
    /// NamespaceManager to use, or null to not use one.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oEdgeOrVertexXmlNode">
    /// The edge or vertex XML node from <paramref
    /// name="oGraphMLXmlDocument" /> to add the GraphML attribute value to.
    /// </param>
    ///
    /// <param name="sGraphMLAttributeID">
    /// GraphML ID of the attribute.
    /// </param>
    ///
    /// <returns>
    /// true if the GraphML-Attribute was appended.
    /// </returns>
    ///
    /// <remarks>
    /// This method selects from <paramref name="oXmlNodeToSelectFrom" /> using
    /// the <paramref name="sXPath" /> expression.  If the selection is
    /// successful, the specified Int32 value gets stored on <paramref
    /// name="oEdgeOrVertexXmlNode" /> as a Graph-ML Attribute.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    AppendInt32GraphMLAttributeValue
    (
        XmlNode oXmlNodeToSelectFrom,
        String sXPath,
        XmlNamespaceManager oXmlNamespaceManager,
        GraphMLXmlDocument oGraphMLXmlDocument,
        XmlNode oEdgeOrVertexXmlNode,
        String sGraphMLAttributeID
    )
    {
        Debug.Assert(oXmlNodeToSelectFrom != null);
        Debug.Assert( !String.IsNullOrEmpty(sXPath) );
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oEdgeOrVertexXmlNode != null);
        Debug.Assert( !String.IsNullOrEmpty(sGraphMLAttributeID) );
        AssertValid();

        Int32 iAttributeValue;

        if ( XmlUtil2.TrySelectSingleNodeAsInt32(oXmlNodeToSelectFrom, sXPath,
            oXmlNamespaceManager, out iAttributeValue) )
        {
            oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                oEdgeOrVertexXmlNode, sGraphMLAttributeID, iAttributeValue);

            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: AppendDoubleGraphMLAttributeValue()
    //
    /// <summary>
    /// Appends a Double GraphML-Attribute value to an edge or vertex XML node. 
    /// </summary>
    ///
    /// <param name="oXmlNodeToSelectFrom">
    /// Node to select from.
    /// </param>
    /// 
    /// <param name="sXPath">
    /// XPath expression to a Double descendant of <paramref
    /// name="oXmlNodeToSelectFrom" />.
    /// </param>
    ///
    /// <param name="oXmlNamespaceManager">
    /// NamespaceManager to use, or null to not use one.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oEdgeOrVertexXmlNode">
    /// The edge or vertex XML node from <paramref
    /// name="oGraphMLXmlDocument" /> to add the GraphML attribute value to.
    /// </param>
    ///
    /// <param name="sGraphMLAttributeID">
    /// GraphML ID of the attribute.
    /// </param>
    ///
    /// <returns>
    /// true if the GraphML-Attribute was appended.
    /// </returns>
    ///
    /// <remarks>
    /// This method selects from <paramref name="oXmlNodeToSelectFrom" /> using
    /// the <paramref name="sXPath" /> expression.  If the selection is
    /// successful, the specified Double value gets stored on <paramref
    /// name="oEdgeOrVertexXmlNode" /> as a Graph-ML Attribute.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    AppendDoubleGraphMLAttributeValue
    (
        XmlNode oXmlNodeToSelectFrom,
        String sXPath,
        XmlNamespaceManager oXmlNamespaceManager,
        GraphMLXmlDocument oGraphMLXmlDocument,
        XmlNode oEdgeOrVertexXmlNode,
        String sGraphMLAttributeID
    )
    {
        Debug.Assert(oXmlNodeToSelectFrom != null);
        Debug.Assert( !String.IsNullOrEmpty(sXPath) );
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oEdgeOrVertexXmlNode != null);
        Debug.Assert( !String.IsNullOrEmpty(sGraphMLAttributeID) );
        AssertValid();

        Double dAttributeValue;

        if ( XmlUtil2.TrySelectSingleNodeAsDouble(oXmlNodeToSelectFrom, sXPath,
            oXmlNamespaceManager, out dAttributeValue) )
        {
            oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                oEdgeOrVertexXmlNode, sGraphMLAttributeID, dAttributeValue);

            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: AppendEdgeXmlNode()
    //
    /// <summary>
    /// Appends an edge XML node to a GraphML document.
    /// </summary>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="sVertex1ID">
    /// ID of the edge's first vertex.
    /// </param>
    ///
    /// <param name="sVertex2ID">
    /// ID of the edge's second vertex.
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
        String sVertex1ID,
        String sVertex2ID,
        String sRelationship
    )
    {
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert( !String.IsNullOrEmpty(sVertex1ID) );
        Debug.Assert( !String.IsNullOrEmpty(sVertex2ID) );
        Debug.Assert( !String.IsNullOrEmpty(sRelationship) );
        AssertValid();

        XmlNode oEdgeXmlNode = oGraphMLXmlDocument.AppendEdgeXmlNode(
            sVertex1ID, sVertex2ID);

        oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode,
            RelationshipID, sRelationship);

        return (oEdgeXmlNode);
    }

    //*************************************************************************
    //  Method: WebExceptionHasHttpStatusCode()
    //
    /// <summary>
    /// Determines whether a WebException has an HttpWebResponse with one of a
    /// specified set of HttpStatusCodes.
    /// </summary>
    ///
    /// <param name="oWebException">
    /// The WebException to check.
    /// </param>
    ///
    /// <param name="aeHttpStatusCodes">
    /// One or more HttpStatus codes to look for.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="oWebException" /> has an HttpWebResponse with
    /// an HttpStatusCode contained within <paramref
    /// name="aeHttpStatusCodes" />.
    /// </returns>
    //*************************************************************************

    protected Boolean
    WebExceptionHasHttpStatusCode
    (
        WebException oWebException,
        params HttpStatusCode [] aeHttpStatusCodes
    )
    {
        Debug.Assert(oWebException != null);
        Debug.Assert(aeHttpStatusCodes != null);
        AssertValid();

        if ( !(oWebException.Response is HttpWebResponse) )
        {
            return (false);
        }

        HttpWebResponse oHttpWebResponse =
            (HttpWebResponse)oWebException.Response;

        return (Array.IndexOf<HttpStatusCode>(
            aeHttpStatusCodes, oHttpWebResponse.StatusCode) >= 0);
    }

    //*************************************************************************
    //  Method: ExceptionIsWebOrXml()
    //
    /// <summary>
    /// Determines whether an exception is a WebException or XmlException.
    /// </summary>
    ///
    /// <param name="oException">
    /// The exception to test.
    /// </param>
    ///
    /// <returns>
    /// true if the exception is a WebException or XmlException.
    /// </returns>
    //*************************************************************************

    protected Boolean
    ExceptionIsWebOrXml
    (
        Exception oException
    )
    {
        Debug.Assert(oException != null);

        return (oException is WebException || oException is XmlException);
    }

    //*************************************************************************
    //  Method: ReportProgress()
    //
    /// <summary>
    /// Reports progress.
    /// </summary>
    ///
    /// <param name="sProgressMessage">
    /// Progress message.  Can be empty but not null.
    /// </param>
    //*************************************************************************

    protected void
    ReportProgress
    (
        String sProgressMessage
    )
    {
        Debug.Assert(sProgressMessage != null);

        // This method is meant to be called when the derived class wants to
        // report progress.  It results in the
        // BackgroundWorker_ProgressChanged() method being called on the main
        // thread, which in turn fires the ProgressChanged event.

        m_oBackgroundWorker.ReportProgress(0, sProgressMessage);
    }

    //*************************************************************************
    //  Method: GetNeedToRecurse()
    //
    /// <summary>
    /// Determines whether a method getting a recursive network needs to
    /// recurse.
    /// </summary>
    ///
    /// <param name="eNetworkLevel">
    /// Network level to include.  Must be NetworkLevel.One, OnePointFive, or
    /// Two.
    /// </param>
    ///
    /// <param name="iRecursionLevel">
    /// Recursion level for the current call.  Must be 1 or 2.
    /// </param>
    ///
    /// <returns>
    /// true if the caller needs to recurse.
    /// </returns>
    ///
    /// <remarks>
    /// This is meant for network analyzers that analyze a recursive network.
    /// Call this from the method that uses recursion to get the different
    /// network levels, and use the return value to determine whether to
    /// recurse.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    GetNeedToRecurse
    (
        NetworkLevel eNetworkLevel,
        Int32 iRecursionLevel
    )
    {
        Debug.Assert(eNetworkLevel == NetworkLevel.One ||
            eNetworkLevel == NetworkLevel.OnePointFive ||
            eNetworkLevel == NetworkLevel.Two);

        Debug.Assert(iRecursionLevel == 1 || iRecursionLevel == 2);
        AssertValid();

        return (
            iRecursionLevel == 1
            &&
            (eNetworkLevel == NetworkLevel.OnePointFive ||
            eNetworkLevel == NetworkLevel.Two)
            );
    }

    //*************************************************************************
    //  Method: GetNeedToAppendVertices()
    //
    /// <summary>
    /// Determines whether a method getting a recursive network needs to
    /// add vertices for a specified network and recursion level.
    /// </summary>
    ///
    /// <param name="eNetworkLevel">
    /// Network level to include.  Must be NetworkLevel.One, OnePointFive, or
    /// Two.
    /// </param>
    ///
    /// <param name="iRecursionLevel">
    /// Recursion level for the current call.  Must be 1 or 2.
    /// </param>
    ///
    /// <returns>
    /// true if the caller needs to add vertices for the specified network and
    /// recursion levels.
    /// </returns>
    ///
    /// <remarks>
    /// This is meant for network analyzers that analyze a recursive network.
    /// Call this from the method that uses recursion to get the different
    /// network levels, and use the return value to determine whether to add
    /// vertices for the current network and recursion levels.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    GetNeedToAppendVertices
    (
        NetworkLevel eNetworkLevel,
        Int32 iRecursionLevel
    )
    {
        Debug.Assert(eNetworkLevel == NetworkLevel.One ||
            eNetworkLevel == NetworkLevel.OnePointFive ||
            eNetworkLevel == NetworkLevel.Two);

        Debug.Assert(iRecursionLevel == 1 || iRecursionLevel == 2);
        AssertValid();

        return (
            (eNetworkLevel != NetworkLevel.OnePointFive ||
            iRecursionLevel == 1)
            );
    }

    //*************************************************************************
    //  Method: CancelIfRequested()
    //
    /// <summary>
    /// Cancels the asynchronous operation if requested.
    /// </summary>
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
    /// true if an asynchronous operation was cancelled.  If true, the caller
    /// should stop what it is doing and return.
    /// </returns>
    //*************************************************************************

    protected Boolean
    CancelIfRequested
    (
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs
    )
    {
        Debug.Assert(oBackgroundWorker == null || oDoWorkEventArgs != null);
        AssertValid();

        if (oBackgroundWorker != null && oBackgroundWorker.CancellationPending)
        {
            Debug.Assert(oDoWorkEventArgs != null);

            oDoWorkEventArgs.Cancel = true;
            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: FireProgressChanged()
    //
    /// <summary>
    /// Fires the ProgressChanged event if appropriate.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    FireProgressChanged
    (
        ProgressChangedEventArgs e
    )
    {
        AssertValid();

        ProgressChangedEventHandler oProgressChanged = this.ProgressChanged;

        if (oProgressChanged != null)
        {
            oProgressChanged(this, e);
        }
    }

    //*************************************************************************
    //  Method: OnNetworkObtainedWithoutTerminatingException()
    //
    /// <summary>
    /// Call this when part or all of the network has been obtained without a
    /// terminating exception occurring.
    /// </summary>
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
    /// <param name="oXmlDocument">
    /// Where an XmlDocument containing the network as GraphML gets stored if
    /// true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the entire network was obtained.
    /// </returns>
    ///
    /// <remarks>
    /// If the entire network has been obtained, <paramref
    /// name="oGraphMLXmlDocument" /> gets stored at <paramref
    /// name="oXmlDocument" /> and true is returned.  Otherise, a
    /// PartialNetworkException is thrown.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    OnNetworkObtainedWithoutTerminatingException
    (
        GraphMLXmlDocument oGraphMLXmlDocument,
        RequestStatistics oRequestStatistics,
        out XmlDocument oXmlDocument
    )
    {
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        if (oRequestStatistics.UnexpectedExceptions == 0)
        {
            // The entire document was obtained without any unexpected
            // exceptions.

            oXmlDocument = oGraphMLXmlDocument;
            return (true);
        }

        // The network is partial.

        throw new PartialNetworkException(oGraphMLXmlDocument,
            oRequestStatistics);
    }

    //*************************************************************************
    //  Method: OnTerminatingException()
    //
    /// <summary>
    /// Handles an exception that unexpectedly terminated the process of
    /// getting the network.
    /// </summary>
    ///
    /// <param name="oException">
    /// The exception that occurred.
    /// </param>
    ///
    /// <remarks>
    /// This should be called only when an unexpected exception occurs,
    /// retrying the request doesn't fix it, and the process must be
    /// terminated.
    /// </remarks>
    //*************************************************************************

    protected void
    OnTerminatingException
    (
        Exception oException
    )
    {
        Debug.Assert(oException != null);
        AssertValid();

        // For now, just rethrow the exception.  In the future, some other code
        // that needs to run whenever an unexpected termination occurs can go
        // here rather than be duplicated in all the network analayzers.

        throw (oException);
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
    /// Standard event arguments.
    /// </param>
    //*************************************************************************

    protected abstract void
    BackgroundWorker_DoWork
    (
        object sender,
        DoWorkEventArgs e
    );

    //*************************************************************************
    //  Method: BackgroundWorker_ProgressChanged()
    //
    /// <summary>
    /// Handles the ProgressChanged event on the BackgroundWorker.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    BackgroundWorker_ProgressChanged
    (
        object sender,
        ProgressChangedEventArgs e
    )
    {
        AssertValid();

        FireProgressChanged(e);
    }

    //*************************************************************************
    //  Method: BackgroundWorker_RunWorkerCompleted()
    //
    /// <summary>
    /// Handles the RunWorkerCompleted event on the BackgroundWorker object.
    /// </summary>
    ///
    /// <param name="sender">
    /// Source of the event.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event arguments.
    /// </param>
    //*************************************************************************

    protected void
    BackgroundWorker_RunWorkerCompleted
    (
        object sender,
        RunWorkerCompletedEventArgs e
    )
    {
        AssertValid();

        FireProgressChanged( new ProgressChangedEventArgs(0, String.Empty) );

        // Forward the event.

        RunWorkerCompletedEventHandler oAnalysisCompleted =
            this.AnalysisCompleted;

        if (oAnalysisCompleted != null)
        {
            oAnalysisCompleted(this, e);
        }
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public virtual void
    AssertValid()
    {
        Debug.Assert(m_oBackgroundWorker != null);
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// User agent to use for all Web requests.

    public const String UserAgent = "Microsoft NodeXL";

    /// The timeout to use for HTTP Web requests, in milliseconds.

    public const Int32 HttpWebRequestTimeoutMs = 30000;


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// GraphML-attribute IDs for vertices.

    protected const String ImageFileID = "Image";
    ///
    protected const String LabelID = "Label";
    ///
    protected const String MenuTextID = "MenuText";
    ///
    protected const String MenuActionID = "MenuAction";

    /// GraphML-attribute IDs for edges.

    protected const String RelationshipID = "Relationship";

    /// NodeXL Excel template column names.

    protected const String ImageColumnName = "Image File";
    ///
    protected const String LabelColumnName = "Label";
    ///
    protected const String MenuTextColumnName = "Custom Menu Item Text";
    ///
    protected const String MenuActionColumnName = "Custom Menu Item Action";

    /// URI of the Atom namespace.

    protected const String AtomNamespaceUri =
        "http://www.w3.org/2005/Atom";

    /// Time to wait between retries to the HTTP Web service, in seconds.  The
    /// length of the array determines the number of retries: three array
    /// elements means there will be up to three retries, or four attempts
    /// total.

    protected static Int32 [] HttpRetryDelaysSec =
        new Int32 [] {1, 1, 5,};


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Used for asynchronous analysis.

    protected BackgroundWorker m_oBackgroundWorker;
}

}
