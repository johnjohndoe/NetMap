

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Xml;
using System.ComponentModel;
using System.Net;
using System.Diagnostics;
using Microsoft.SocialNetworkLib;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.XmlLib;

namespace Microsoft.NodeXL.GraphDataProviders
{
//*****************************************************************************
//  Class: GraphDataProviderDialogBase
//
/// <summary>
/// Base class for dialogs that get graph data.
/// </summary>
///
/// <remarks>
/// This class and its virtual methods should actually be declared abstract,
/// but the Visual Studio designer doesn't allow dialog classes to have an
/// abstract base class.
/// </remarks>
//*****************************************************************************

public class GraphDataProviderDialogBase : FormPlus
{
    //*************************************************************************
    //  Constructor: GraphDataProviderDialogBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GraphDataProviderDialogBase" /> class.
    /// </summary>
    ///
    /// <param name="httpNetworkAnalyzer">
    /// Object that does most of the work.
    /// </param>
    //*************************************************************************

    public GraphDataProviderDialogBase
    (
        HttpNetworkAnalyzerBase httpNetworkAnalyzer
    )
    {
        Debug.Assert(httpNetworkAnalyzer != null);

        this.ShowInTaskbar = false;
        m_oGraphMLXmlDocument = null;

        m_oHttpNetworkAnalyzer = httpNetworkAnalyzer;

        m_oHttpNetworkAnalyzer.HttpWebRequestTimeoutMs =
            HttpWebRequestTimeoutMs;

        m_oHttpNetworkAnalyzer.HttpWebRequestRetries = HttpWebRequestRetries;

        m_oHttpNetworkAnalyzer.AnalysisCompleted +=
            new RunWorkerCompletedEventHandler(
                HttpNetworkAnalyzer_AnalysisCompleted);

        // AssertValid();
    }

    //*************************************************************************
    //  Constructor: GraphDataProviderDialogBase()
    //
    /// <summary>
    /// Do not use this constructor.
    /// </summary>
    ///
    /// <remarks>
    /// Do not use this constructor.  It is for the Visual Studio designer
    /// only.
    /// </remarks>
    //*************************************************************************

    public GraphDataProviderDialogBase()
    {
        // (Do nothing.)
    }

    //*************************************************************************
    //  Property: Results
    //
    /// <summary>
    /// Gets the dialog results.
    /// </summary>
    ///
    /// <value>
    /// The dialog results, as an XmlDocument containing GraphML.
    /// </value>
    ///
    /// <remarks>
    /// Read this property only if <see cref="Form.ShowDialog()" /> returns
    /// DialogResult.OK.
    /// </remarks>
    //*************************************************************************

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Never)]

    public XmlDocument
    Results
    {
        get
        {
            if (this.DesignMode)
            {
                // What does it take to get the Visual Studio designer to stop
                // reading this property?  If the DesignMode property isn't
                // checked here and the project is built while the designer has
                // a derived dialog open, this property is read after the build
                // and the AssertValid() call below asserts because the object
                // hasn't been constructed with the proper constructor.  None
                // of the attributes listed above fix this.

                return (null);
            }

            AssertValid();
            Debug.Assert(m_oGraphMLXmlDocument != null);

            return (m_oGraphMLXmlDocument);
        }
    }

    //*************************************************************************
    //  Method: DoDataExchange()
    //
    /// <summary>
    /// Transfers data between the dialog's fields and its controls.
    /// </summary>
    ///
    /// <param name="bFromControls">
    /// true to transfer data from the dialog's controls to its fields, false
    /// for the other direction.
    /// </param>
    ///
    /// <returns>
    /// true if the transfer was successful.
    /// </returns>
    //*************************************************************************

    protected virtual Boolean
    DoDataExchange
    (
        Boolean bFromControls
    )
    {
        Debug.Assert(false);
        return (false);
    }

    //*************************************************************************
    //  Method: EnableControls()
    //
    /// <summary>
    /// Enables or disables the dialog's controls.
    /// </summary>
    //*************************************************************************

    protected virtual void
    EnableControls()
    {
        Debug.Assert(false);
    }

    //*************************************************************************
    //  Method: StartAnalysis()
    //
    /// <summary>
    /// Starts the Twitter analysis.
    /// </summary>
    ///
    /// <remarks>
    /// It's assumed that DoDataExchange(true) was called and succeeded.
    /// </remarks>
    //*************************************************************************

    protected virtual void
    StartAnalysis()
    {
        Debug.Assert(false);
    }

    //*************************************************************************
    //  Method: OnEmptyGraph()
    //
    /// <summary>
    /// Handles the case where a graph was successfully obtained by is empty.
    /// </summary>
    //*************************************************************************

    protected virtual void
    OnEmptyGraph()
    {
        Debug.Assert(false);
    }

    //*************************************************************************
    //  Method: OnAnalysisCompleted()
    //
    /// <summary>
    /// Handles the AnalysisCompleted event on the HttpNetworkAnalyzer object.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    OnAnalysisCompleted
    (
        RunWorkerCompletedEventArgs e
    )
    {
        AssertValid();

        EnableControls();

        if (e.Cancelled)
        {
            // (Do nothing.)
        }
        else if (e.Error != null)
        {
            OnAnalysisException(e.Error);
        }
        else
        {
            if ( OnAnalysisSuccess(e) )
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                OnEmptyGraph();
            }
        }
    }

    //*************************************************************************
    //  Method: OnAnalysisSuccess()
    //
    /// <summary>
    /// Handles the AnalysisCompleted event on the HttpNetworkAnalyzer object
    /// when the analysis is successful.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    ///
    /// <returns>
    /// true if at least one vertex was returned.
    /// </returns>
    //*************************************************************************

    protected Boolean
    OnAnalysisSuccess
    (
        RunWorkerCompletedEventArgs e
    )
    {
        Debug.Assert(e != null);
        AssertValid();

        Debug.Assert(e.Result is XmlDocument);

        XmlDocument oGraphMLXmlDocument = (XmlDocument)e.Result;

        // Check whether at least one vertex was returned.
        //
        // Note that any prefix will do, so long as it is also included in
        // every XPath expression.

        XmlNamespaceManager oXmlNamespaceManager = new XmlNamespaceManager(
            oGraphMLXmlDocument.NameTable);

        oXmlNamespaceManager.AddNamespace("g",
            GraphMLXmlDocument.GraphMLNamespaceUri);

        if (oGraphMLXmlDocument.SelectSingleNode("g:graphml/g:graph/g:node",
            oXmlNamespaceManager) != null)
        {
            m_oGraphMLXmlDocument = oGraphMLXmlDocument;
            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: OnAnalysisException()
    //
    /// <summary>
    /// Handles the AnalysisCompleted event on the HttpNetworkAnalyzer object
    /// when an exception occurs.
    /// </summary>
    ///
    /// <param name="oException">
    /// The exception that occurred.
    /// </param>
    //*************************************************************************

    protected virtual void
    OnAnalysisException
    (
        Exception oException
    )
    {
        Debug.Assert(false);
    }

    //*************************************************************************
    //  Method: OnOKClick()
    //
    /// <summary>
    /// Handles the Click event on the btnOK button.
    /// </summary>
    //*************************************************************************

    protected void
    OnOKClick()
    {
        AssertValid();

        // Avoid any possibility of a race condition by checking IsBusy.

        if ( !m_oHttpNetworkAnalyzer.IsBusy && DoDataExchange(true) )
        {
            StartAnalysis();
        }

        EnableControls();
    }

    //*************************************************************************
    //  Method: OnClosed()
    //
    /// <summary>
    /// Handles the Closed event.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected override void
    OnClosed
    (
        EventArgs e
    )
    {
        AssertValid();

        if (m_oHttpNetworkAnalyzer.IsBusy)
        {
            // Let the background thread cancel its task, but don't try to
            // notify this dialog.

            m_oHttpNetworkAnalyzer.AnalysisCompleted -=
                new RunWorkerCompletedEventHandler(
                    HttpNetworkAnalyzer_AnalysisCompleted);

            m_oHttpNetworkAnalyzer.CancelAsync();
        }
    }

    //*************************************************************************
    //  Method: HttpNetworkAnalyzer_AnalysisCompleted()
    //
    /// <summary>
    /// Handles the AnalysisCompleted event on the HttpNetworkAnalyzer object.
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

    private void
    HttpNetworkAnalyzer_AnalysisCompleted
    (
        object sender,
        RunWorkerCompletedEventArgs e
    )
    {
        AssertValid();

        OnAnalysisCompleted(e);
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

        // m_oGraphMLXmlDocument
        Debug.Assert(m_oHttpNetworkAnalyzer != null);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// The timeout to use for Web requests, in milliseconds.

    protected const Int32 HttpWebRequestTimeoutMs = 10000;

    /// The maximum number of retries per Web request.

    protected const Int32 HttpWebRequestRetries = 2;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Dialog results, or null.

    protected XmlDocument m_oGraphMLXmlDocument;

    /// Object that does most of the work.

    protected HttpNetworkAnalyzerBase m_oHttpNetworkAnalyzer;
}
}
