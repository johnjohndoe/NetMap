
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: RunRibbonCommandEventArgs
//
/// <summary>
/// Provides information for the <see cref="Ribbon.RunRibbonCommand" /> event.
/// </summary>
//*****************************************************************************

public class RunRibbonCommandEventArgs : EventArgs
{
    //*************************************************************************
    //  Constructor: RunRibbonCommandEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="RunRibbonCommandEventArgs" /> class.
    /// </summary>
    ///
    /// <param name="ribbonCommand">
    /// The ribbon command that needs to be run.
    /// </param>
    //*************************************************************************

    public RunRibbonCommandEventArgs
    (
        RibbonCommand ribbonCommand
    )
    {
        m_eRibbonCommand = ribbonCommand;

        AssertValid();
    }

    //*************************************************************************
    //  Property: RibbonCommand
    //
    /// <summary>
    /// Gets the ribbon command that needs to be run.
    /// </summary>
    ///
    /// <value>
    /// The ribbon command that needs to be run.
    /// </value>
    //*************************************************************************

    public RibbonCommand
    RibbonCommand
    {
        get
        {
            AssertValid();

            return (m_eRibbonCommand);
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

    public void
    AssertValid()
    {
        // m_eRibbonCommand
    }


    //*************************************************************************
    //  Protected member data
    //*************************************************************************

    /// The ribbon command that needs to be run.

    protected RibbonCommand m_eRibbonCommand;
}


//*****************************************************************************
//  Delegate: RunRibbonCommandEventHandler
//
/// <summary>
/// Represents a method that will handle the <see
/// cref="Ribbon.RunRibbonCommand" /> event.
/// </summary>
///
/// <param name="sender">
/// The source of the event.
/// </param>
///
/// <param name="e">
/// A <see cref="RunRibbonCommandEventArgs" /> object that contains the event
/// data.
/// </param>
//*****************************************************************************

public delegate void
RunRibbonCommandEventHandler
(
    Object sender,
    RunRibbonCommandEventArgs e
);

}
