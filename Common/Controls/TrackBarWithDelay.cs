
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: TrackBarWithDelay
//
/// <summary>
/// TrackBar control that delays its <see cref="TrackBar.Scroll" /> event until
/// the thumb has stopped moving.
/// </summary>
///
/// <remarks>
/// This control fires a <see cref="TrackBar.Scroll" /> event <see
/// cref="ScrollDelayMs" /> milliseconds after the thumb was last moved.  Use
/// this control instead of TrackBar in applications where the event handler
/// code is slow and handling every <see cref="TrackBar.Scroll" /> event would
/// result in sluggish performance.
/// </remarks>
//*****************************************************************************

public class TrackBarWithDelay : TrackBar
{
    //*************************************************************************
    //  Constructor: TrackBarWithDelay()
    //
    /// <summary>
    /// Initializes a new instance of the TrackBarWithDelay class.
    /// </summary>
    //*************************************************************************

    public TrackBarWithDelay()
    {
        m_oDelayTimer = new Timer();
        m_oDelayTimer.Interval = 50;
        m_oDelayTimer.Tick += new EventHandler(this.m_oDelayTimer_Tick);
    }

    //*************************************************************************
    //  Property: ScrollDelayMs
    //
    /// <summary>
    /// Gets or sets the delay after the thumb was last moved before the <see
    /// cref="TrackBar.Scroll" /> event fires.
    /// </summary>
    ///
    /// <value>
    /// The delay, in milliseconds.  The default value is 50 ms.
    /// </value>
    //*************************************************************************

    public Int32
    ScrollDelayMs
    {
        get
        {
            AssertValid();

            return (m_oDelayTimer.Interval);
        }

        set
        {
            m_oDelayTimer.Interval = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: OnScroll()
    //
    /// <summary>
    /// Handles the Scroll event.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event arguments.
    /// </param>
    //*************************************************************************

    protected override void
    OnScroll
    (
        EventArgs e
    )
    {
        AssertValid();

        // Reset the timer.

        m_oDelayTimer.Stop();
        m_oDelayTimer.Start();

        // Note that the base-class method isn't called here.  That prevents
        // registered Scroll delgates from getting called.  They will be called
        // instead from the timer's Tick event handler.
    }

    //*************************************************************************
    //  Method: m_oDelayTimer_Tick()
    //
    /// <summary>
    /// Handles the Tick event on the delay timer.
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
    m_oDelayTimer_Tick
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        m_oDelayTimer.Stop();

        // Fire the Scroll event.

        base.OnScroll(EventArgs.Empty);
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
        Debug.Assert(m_oDelayTimer != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Timer used to delay the Scroll event.

    protected Timer m_oDelayTimer;
}

}
