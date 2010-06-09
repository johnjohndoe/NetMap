

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
        // m_sCredentialsScreenName
        // m_sCredentialsPassword

        AssertValid();
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

        // m_sCredentialsScreenName
        // m_sCredentialsPassword
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // These are static so that the dialog's controls will retain their values
    // between dialog invocations.  Most NodeXL dialogs persist control values
    // via ApplicationSettingsBase, but this plugin does not have access to
    // that and so it resorts to static fields.

    /// The Twitter credentials to use.
    ///
    /// These are shared among the derived dialogs so that the user doesn't
    /// have to re-enter his credentials in each dialog.

    protected static String m_sCredentialsScreenName = String.Empty;
    ///
    protected static String m_sCredentialsPassword = String.Empty;
}
}
