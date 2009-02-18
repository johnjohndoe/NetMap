
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.GraphicsLib
{
//*****************************************************************************
//  Class: ToolTipTracker
//
/// <summary>
/// Helper class for displaying tooltips in Windows Forms applications.
/// </summary>
///
/// <remarks>
/// See the <see cref="ToolTipTrackerBase" /> base class for details on how to
/// use this class.
/// </remarks>
//*****************************************************************************

internal class ToolTipTracker : ToolTipTrackerBase
{
    //*************************************************************************
    //  Constructor: ToolTipTracker()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ToolTipTracker" /> class.
    /// </summary>
    //*************************************************************************

    public ToolTipTracker()
    :
    base ( new WindowsFormsToolTipTimer() )
    {
        // (Do nothing else.)

        AssertValid();
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
    //  Protected fields
    //*************************************************************************

    // (None.)
}


//*****************************************************************************
//  Class: WindowsFormsToolTipTimer
//
/// <summary>
/// Implements a timer used by <see cref="ToolTipTrackerBase" /> in Windows
/// Forms applications.
/// </summary>
///
/// <remarks>
/// This class wraps the Windows Forms timer in an <see cref="IToolTipTimer" />
/// interface for use by <see cref="ToolTipTrackerBase" />.
/// </remarks>
//*****************************************************************************

internal class WindowsFormsToolTipTimer :
    System.Windows.Forms.Timer, IToolTipTimer
{
}

}
