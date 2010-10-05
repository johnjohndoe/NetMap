
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: NodeXLApplicationSettingsBase
//
/// <summary>
/// Base class for NodeXL's user settings classes.
/// </summary>
///
/// <remarks>
/// In releases prior to NodeXL's use of the ClickOnce deployment technology,
/// this class implemented a workaround for a problem involving the location of
/// the user's settings file.  ClickOnce eliminated that problem, but this
/// class is retained as a placeholder for future code common to all NodeXL
/// user settings classes.
/// </remarks>
//*****************************************************************************

public class NodeXLApplicationSettingsBase : ApplicationSettingsBase
{
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
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
