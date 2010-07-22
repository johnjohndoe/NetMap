

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterGraphDataProviderDialogBase
//
/// <summary>
/// Base class for dialogs that get Twitter graph data.
/// </summary>
///
/// <remarks>
/// The derived class must call the <see
/// cref="InitializeTwitterAuthorizationManager" /> method.
/// </remarks>
//*****************************************************************************

public class TwitterGraphDataProviderDialogBase : GraphDataProviderDialogBase
{
    //*************************************************************************
    //  Constructor: TwitterGraphDataProviderDialogBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterGraphDataProviderDialogBase" /> class.
    /// </summary>
    ///
    /// <param name="httpNetworkAnalyzer">
    /// Object that does most of the work.
    /// </param>
    //*************************************************************************

    public TwitterGraphDataProviderDialogBase
    (
        HttpNetworkAnalyzerBase httpNetworkAnalyzer
    )
    : base (httpNetworkAnalyzer)
    {
        m_oTwitterAuthorizationManager = null;

        // AssertValid();
    }

    //*************************************************************************
    //  Constructor: TwitterGraphDataProviderDialogBase()
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

    public TwitterGraphDataProviderDialogBase()
    {
        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: InitializeTwitterAuthorizationManager()
    //
    /// <summary>
    /// Initializes the TwitterAuthorizationManager object.
    /// </summary>
    ///
    /// <param name="oTwitterAuthorizationControl">
    /// The TwitterAuthorizationControl owned by the parent dialog.
    /// </param>
    //*************************************************************************

    protected void
    InitializeTwitterAuthorizationManager
    (
        TwitterAuthorizationControl oTwitterAuthorizationControl 
    )
    {
        Debug.Assert(oTwitterAuthorizationControl != null);
        AssertValid();

        m_oTwitterAuthorizationManager = new TwitterAuthorizationManager(
            oTwitterAuthorizationControl);
    }

    //*************************************************************************
    //  Method: PreviewStartAnalysis()
    //
    /// <summary>
    /// Performs tasks required before the analysis is started.
    /// </summary>
    ///
    /// <returns>
    /// true if the analysis should be started, false to stop.
    /// </returns>
    ///
    /// <remarks>
    /// The derived dialog should overload this method if it needs to perform
    /// tasks before analysis is started.
    /// </remarks>
    //*************************************************************************

    protected override Boolean
    PreviewStartAnalysis()
    {
        AssertValid();

        Debug.Assert(m_oTwitterAuthorizationManager != null);

        if ( !base.PreviewStartAnalysis() )
        {
            return (false);
        }

        this.UseWaitCursor = true;

        try
        {
            return ( m_oTwitterAuthorizationManager.AuthorizeIfRequested() );
        }
        catch (WebException oWebException)
        {
            this.ShowWarning(
                "A problem occurred while attempting to authorize NodeXL to"
                + " use your Twitter account.  Details:"
                + "\r\n\r\n"
                + oWebException.Message
                );

            return (false);
        }
        finally
        {
            this.UseWaitCursor = false;
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

        // m_oTwitterAuthorizationManager
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Manages the process of authorizing NodeXL with Twitter, or null if
    /// InitializeTwitterAuthorizationManager() hasn't been called yet.

    protected TwitterAuthorizationManager m_oTwitterAuthorizationManager;
}
}
