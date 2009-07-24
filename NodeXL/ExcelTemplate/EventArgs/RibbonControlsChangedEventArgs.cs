
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: RibbonControlsChangedEventArgs
//
/// <summary>
/// Provides information for the <see cref="Ribbon.RibbonControlsChanged" />
/// event.
/// </summary>
//*****************************************************************************

public class RibbonControlsChangedEventArgs : EventArgs
{
    //*************************************************************************
    //  Constructor: RibbonControlsChangedEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="RibbonControlsChangedEventArgs" /> class.
    /// </summary>
    ///
    /// <param name="ribbonControls">
    /// The controls in the ribbon whose states have changed.
    /// </param>
    //*************************************************************************

    public RibbonControlsChangedEventArgs
    (
        RibbonControls ribbonControls
    )
    {
        m_eRibbonControls = ribbonControls;

        AssertValid();
    }

    //*************************************************************************
    //  Property: RibbonControls
    //
    /// <summary>
    /// Gets the controls in the ribbon whose states have changed.
    /// </summary>
    ///
    /// <value>
    /// The controls in the ribbon whose states have changed.
    /// </value>
    //*************************************************************************

    public RibbonControls
    RibbonControls
    {
        get
        {
            AssertValid();

            return (m_eRibbonControls);
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
        // m_eRibbonControl
    }


    //*************************************************************************
    //  Protected member data
    //*************************************************************************

    /// The controls in the ribbon whose states have changed.

    protected RibbonControls m_eRibbonControls;
}


//*****************************************************************************
//  Delegate: RibbonControlsChangedEventHandler
//
/// <summary>
/// Represents a method that will handle the <see
/// cref="Ribbon.RibbonControlsChanged" /> event.
/// </summary>
///
/// <param name="sender">
/// The source of the event.
/// </param>
///
/// <param name="e">
/// A <see cref="RibbonControlsChangedEventArgs" /> object that contains the
/// event data.
/// </param>
//*****************************************************************************

public delegate void
RibbonControlsChangedEventHandler
(
    Object sender,
    RibbonControlsChangedEventArgs e
);

}
