
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Xml;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.Diagnostics;

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
        m_iHttpWebRequestTimeoutMs = 10000;
        m_iHttpWebRequestRetries = 0;

        m_oBackgroundWorker = new BackgroundWorker();
        m_oBackgroundWorker.WorkerSupportsCancellation = true;

        m_oBackgroundWorker.DoWork += new DoWorkEventHandler(
            BackgroundWorker_DoWork);

        m_oBackgroundWorker.RunWorkerCompleted +=
            new RunWorkerCompletedEventHandler(
                BackgroundWorker_RunWorkerCompleted);

        // AssertValid();
    }

    //*************************************************************************
    //  Property: HttpWebRequestTimeoutMs
    //
    /// <summary>
    /// Gets or sets the timeout to use for Web requests.
    /// </summary>
    ///
    /// <value>
    /// The timeout to use for Web requests, in milliseconds.  Must be greater
    /// than zero.  The default value is 10,000.
    /// </value>
    //*************************************************************************

    public Int32
    HttpWebRequestTimeoutMs
    {
        get
        {
            AssertValid();

            return (m_iHttpWebRequestTimeoutMs);
        }

        set
        {
            m_iHttpWebRequestTimeoutMs = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: HttpWebRequestRetries
    //
    /// <summary>
    /// Gets or sets the maximum number of retries per request to the HTTP Web
    /// service.
    /// </summary>
    ///
    /// <value>
    /// The maximum number of retries per request to the HTTP Web service.
    /// Must be greater than or equal to zero.  The default value is zero.
    /// </value>
    //*************************************************************************

    public Int32
    HttpWebRequestRetries
    {
        get
        {
            AssertValid();

            return (m_iHttpWebRequestRetries);
        }

        set
        {
            m_iHttpWebRequestRetries = value;

            AssertValid();
        }
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
    //  Event: AnalysisCompleted
    //
    /// <summary>
    /// Occurs when the analysis started by an async method completes, is
    /// cancelled, or encounters an error.
    /// </summary>
    //*************************************************************************

    public event RunWorkerCompletedEventHandler AnalysisCompleted;


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
    //  Method: GetXmlDocument()
    //
    /// <summary>
    /// Gets an XML document given an HttpWebRequest object.
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
    /// This method sets the Timeout property on <paramref
    /// name="oHttpWebRequest" /> before it is used.
    /// </remarks>
    //*************************************************************************

    protected XmlDocument
    GetXmlDocument
    (
        HttpWebRequest oHttpWebRequest
    )
    {
        Debug.Assert(oHttpWebRequest != null);
        AssertValid();

        HttpWebResponse oHttpWebResponse = null;
        Stream oStream = null;
        XmlDocument oXmlDocument;

        try
        {
            oHttpWebRequest.Timeout = m_iHttpWebRequestTimeoutMs;
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
        Debug.Assert(m_iHttpWebRequestTimeoutMs > 0);
        Debug.Assert(m_iHttpWebRequestRetries >= 0);
        Debug.Assert(m_oBackgroundWorker != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The timeout to use for HTTP Web requests, in milliseconds.

    protected Int32 m_iHttpWebRequestTimeoutMs;

    /// The maximum number of retries per request to the HTTP Web service.

    protected Int32 m_iHttpWebRequestRetries;

    /// Used for asynchronous analysis.

    protected BackgroundWorker m_oBackgroundWorker;
}

}
