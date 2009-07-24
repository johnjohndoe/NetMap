
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;

namespace Microsoft.WpfGraphicsLib
{
//*****************************************************************************
//  Class: WpfToolTipTracker
//
/// <summary>
/// Helper class for displaying tooltips in WPF applications.
/// </summary>
///
/// <remarks>
/// See the <see cref="ToolTipTrackerBase" /> base class for details on how to
/// use this class.
/// </remarks>
//*****************************************************************************

internal class WpfToolTipTracker : ToolTipTrackerBase
{
    //*************************************************************************
    //  Constructor: WpfToolTipTracker()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="WpfToolTipTracker" />
    /// class.
    /// </summary>
    //*************************************************************************

    public WpfToolTipTracker()
    :
    base ( new WpfToolTipTimer() )
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
//  Class: WpfToolTipTimer
//
/// <summary>
/// Implements a timer used by <see cref="ToolTipTrackerBase" /> in WPF
/// applications.
/// </summary>
///
/// <remarks>
/// This class wraps the WPF DispatcherTimer in an <see cref="IToolTipTimer" />
/// interface for use by <see cref="ToolTipTrackerBase" />.
/// </remarks>
//*****************************************************************************

internal class WpfToolTipTimer :
    System.Windows.Threading.DispatcherTimer, IToolTipTimer
{
    //*************************************************************************
    //  Property: Interval
    //
    /// <summary>
    /// Gets or sets the time, in milliseconds, before the Tick event is raised
    /// relative to the last occurrence of the Tick event.
    /// </summary>
    ///
    /// <value>
    /// The timer interval, in milliseconds.
    /// </value>
    //*************************************************************************

    public new Int32
    Interval
    {
        get
        {
            return ( (Int32)base.Interval.TotalMilliseconds );
        }

        set
        {
            base.Interval = TimeSpan.FromMilliseconds(value);
        }
    }

    //*************************************************************************
    //  Method: Dispose()
    //
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing,
    /// or resetting unmanaged resources.
    /// </summary>
    //*************************************************************************

    public void
    Dispose()
    {
        // (Do nothing.)
    }
}

}
