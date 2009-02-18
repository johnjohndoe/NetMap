
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.GraphicsLib
{
//*****************************************************************************
//  Class: ToolTipTrackerBase
//
/// <summary>
/// Helper class for displaying tooltips.
/// </summary>
///
/// <remarks>
/// This is meant for use by a Control object that displays various objects
/// within its window and wants to show a tooltip for each object.  The ToolTip
/// class in the FCL makes it easy to show a single tooltip for an entire
/// control, but it does not support different tooltips for different parts of
/// the control's window.
///
/// <para>
/// To use ToolTipTracker, call <see cref="OnMouseMoveOverObject" /> from the
/// control's MouseMove event handler.  If the mouse is currently over an
/// object that has a tooltip associated with it, pass the object as the
/// method's oObjectToTrack parameter.  Otherwise, pass null.  Also, call
/// <see cref="OnMouseMoveOverObject" /> with a null parameter from the
/// control's MouseLeave event handler.
/// </para>
///
/// <para>
/// If the mouse remains over an object for a period of <see
/// cref="ShowDelayMs" /> milliseconds, ToolTipTracker fires a <see
/// cref="ShowToolTip" /> event.  The event arguments include the object being
/// tracked.
/// </para>
///
/// <para>
/// A <see cref="HideToolTip" /> event is fired when the tooltip should be
/// hidden.  This occurs <see cref="HideDelayMs" /> after the <see
/// cref="ShowToolTip" /> event fires if the mouse remains over the object, or
/// immediately if OnMouseMoveOverObject(null) is called.
/// </para>
///
/// <para>
/// Note that ToolTipTracker does not actually show or hide the tooltip; that's
/// up to the application.  The easiest way to do this is to create a child
/// ToolTipPanel control and call its Show and Hide methods in response to the
/// <see cref="ShowToolTip" /> and <see cref="HideToolTip" /> events.
/// </para>
///
/// <para>
/// If the mouse is moved to another object within <see cref="ReshowDelayMs" />
/// milliseconds, another <see cref="ShowToolTip" /> event is fired.
/// Otherwise, the waiting period reverts to <see cref="ShowDelayMs" />.
/// </para>
///
/// <para>
/// Call <see cref="Reset" /> to reset ToolTipTracker to its initial state.
/// This forces a <see cref="HideToolTip" /> event if a tooltip is showing.
/// </para>
///
/// <para>
/// <b>IMPORTANT</b>
/// </para>
///
/// <para>
/// The control must call <see cref="Dispose()" /> from its own Dispose method.
/// This prevents timer-based events from firing after the control no longer
/// has a handle.
/// </para>
///
/// </remarks>
//*****************************************************************************

internal class ToolTipTrackerBase : Object, IDisposable
{
    //*************************************************************************
    //  Constructor: ToolTipTrackerBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ToolTipTrackerBase" />
    /// class.
    /// </summary>
    ///
    /// <param name="toolTipTimer">
    /// The timer to use internally.  See the <see cref="IToolTipTimer" />
    /// interface for details.
    /// </param>
    //*************************************************************************

    public ToolTipTrackerBase
    (
        IToolTipTimer toolTipTimer
    )
    {
        m_iShowDelayMs = DefaultShowDelayMs;
        m_iHideDelayMs = DefaultHideDelayMs;
        m_iReshowDelayMs = DefaultReshowDelayMs;
        m_iState = State.NotDoingAnything;
        m_oTrackedObject = null;

        // Save the timer but don't start it yet.

        m_oTimer = toolTipTimer;
        m_oTimer.Tick += new EventHandler(TimerTick);

        m_bDisposed = false;

        // AssertValid();
    }

    //*************************************************************************
    //  Destructor: ~ToolTipTrackerBase()
    //
    /// <summary>
    /// ToolTipTrackerBase destructor.
    /// </summary>
    //*************************************************************************

    ~ToolTipTrackerBase()      
    {
        Dispose(false);
    }

    //*************************************************************************
    //  Property: ShowDelayMs
    //
    /// <summary>
    /// Gets or sets the number of milliseconds to wait to fire the <see
    /// cref="ShowToolTip" /> event after <see cref="OnMouseMoveOverObject" />
    /// is first called.
    /// </summary>
    ///
    /// <value>
    /// The number of milliseconds to wait.
    /// </value>
    //*************************************************************************

    public Int32 ShowDelayMs
    {
        get
        {
            AssertValid();

            return (m_iShowDelayMs);
        }

        set
        {
            ValidateDelayProperty(value, "ShowDelayMs");
            m_iShowDelayMs = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: HideDelayMs
    //
    /// <summary>
    /// Gets or sets the number of milliseconds to wait to fire the <see
    /// cref="HideToolTip" /> event after <see cref="ShowToolTip" /> is fired.
    /// </summary>
    ///
    /// <value>
    /// The number of milliseconds to wait.
    /// </value>
    //*************************************************************************

    public Int32 HideDelayMs
    {
        get
        {
            AssertValid();

            return (m_iHideDelayMs);
        }

        set
        {
            ValidateDelayProperty(value, "HideDelayMs");
            m_iHideDelayMs = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ReshowDelayMs
    //
    /// <summary>
    /// Gets or sets the number of milliseconds to wait after a <see
    /// cref="HideToolTip" /> event before a <see cref="ShowToolTip" /> event
    /// will be fired if the mouse is moved over another object.
    /// </summary>
    ///
    /// <value>
    /// The delay, in milliseconds.
    /// </value>
    ///
    /// <remarks>
    /// If this delay elapses without <see cref="OnMouseMoveOverObject" />
    /// being called, the waiting period reverts to <see cref="ShowDelayMs" />.
    /// </remarks>
    //*************************************************************************

    public Int32 ReshowDelayMs
    {
        get
        {
            AssertValid();

            return (m_iReshowDelayMs);
        }

        set
        {
            ValidateDelayProperty(value, "ReshowDelayMs");
            m_iReshowDelayMs = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ToolTipShown
    //
    /// <summary>
    /// Gets a flag indicating whether a tooltip is being shown.
    /// </summary>
    ///
    /// <value>
    /// true if a tooltip is being shown.
    /// </value>
    ///
    /// <remarks>
    /// This gets set to true at the same time the <see cref="ShowToolTip" />
    /// event fires, and to false at the same time the <see
    /// cref="HideToolTip" /> event fires.
    /// </remarks>
    //*************************************************************************

    public Boolean ToolTipShown
    {
        get
        {
            AssertValid();

            return (m_iState == State.WaitingForHideTimeout);
        }
    }

    //*************************************************************************
    //  Property: TrackedObject
    //
    /// <summary>
    /// Gets the Object being tracked.
    /// </summary>
    ///
    /// <value>
    /// The Object most recently passed to <see
    /// cref="OnMouseMoveOverObject" />, or null if an object isn't being
    /// tracked.
    /// </value>
    //*************************************************************************

    public Object TrackedObject
    {
        get
        {
            AssertValid();

            return (m_oTrackedObject);
        }
    }

    //*************************************************************************
    //  Method: OnMouseMoveOverObject()
    //
    /// <summary>
    /// Specifies that the mouse has moved over an object.
    /// </summary>
    ///
    /// <param name="objectToTrack">
    /// Object to track, or null to stop tracking.
    /// </param>
    ///
    /// <remarks>
    /// Call this with an Object parameter when the mouse moves over an object
    /// that should be tracked.  Call it with a null parameter when the mouse
    /// moves over an area of the control where there is no object, and when
    /// the mouse leaves the control.
    /// </remarks>
    //*************************************************************************

    public void
    OnMouseMoveOverObject
    (
        Object objectToTrack
    )
    {
        AssertValid();

        // Debug.WriteLine("OnMouseMoveOverObject: " + objectToTrack);

        switch (m_iState)
        {
            case State.NotDoingAnything:

                // Nothing is being tracked.

                if (objectToTrack != null)
                {
                    // Fire a ShowToolTip event after m_iShowDelayMs.

                    ChangeState(State.WaitingForShowTimeout, objectToTrack);
                }

                break;

            case State.WaitingForShowTimeout:

                // An object is being tracked and we're waiting to show its
                // tooltip.

                if (objectToTrack == null)
                {
                    // Stop the timer.

                    ChangeState(State.NotDoingAnything, null);
                }
                else if (objectToTrack == m_oTrackedObject)
                {
                    // (No need to do anything, just keep waiting.)
                }
                else
                {
                    // Start tracking the new object.  Fire a ShowToolTip
                    // event after m_iShowDelayMs.

                    ChangeState(State.WaitingForShowTimeout, objectToTrack);
                }

                break;

            case State.WaitingForHideTimeout:

                // A tooltip for m_oTrackedObject is currently being shown and
                // we're waiting to hide it.

                if (objectToTrack == null)
                {
                    // Hide the tooltip.

                    FireHideToolTipEvent(m_oTrackedObject);

                    // Wait up to m_iReshowDelayMs for another object to
                    // track.

                    ChangeState(State.WaitingForReshowTimeout, null);
                }
                else if (objectToTrack == m_oTrackedObject)
                {
                    // The mouse was moved within the same object.  Restart the
                    // timer.

                    ChangeState(State.WaitingForHideTimeout, objectToTrack);
                }
                else
                {
                    // Hide the old tooltip.

                    FireHideToolTipEvent(m_oTrackedObject);

                    // Show a new tooltip.

                    FireShowToolTipEvent(objectToTrack);

                    // Fire a HideToolTip event after m_iHideDelayMs.

                    ChangeState(State.WaitingForHideTimeout, objectToTrack);
                }

                break;

            case State.WaitingForReshowTimeout:

                // A tooltip was recently hidden and we're waiting for another
                // object to track.

                if (objectToTrack == null)
                {
                    // (No need to do anything, just keep waiting.)
                }
                else
                {
                    // Show a new tooltip.

                    FireShowToolTipEvent(objectToTrack);

                    // Fire a HideToolTip event after m_iHideDelayMs.

                    ChangeState(State.WaitingForHideTimeout, objectToTrack);
                }

                break;

            default:

                Debug.Assert(false);
                break;
        }
    }

    //*************************************************************************
    //  Method: Reset()
    //
    /// <summary>
    /// Resets the object to its initial state.
    /// </summary>
    ///
    /// <remarks>
    /// This forces a <see cref="HideToolTip" /> event if a tooltip is showing.
    /// </remarks>
    //*************************************************************************

    public void
    Reset()
    {
        AssertValid();

        m_oTimer.Stop();

        if (m_iState == State.WaitingForHideTimeout)
        {
            // A tooltip for m_oTrackedObject is currently being shown and
            // we're waiting to hide it.

            // Hide the tooltip immediately.

            FireHideToolTipEvent(m_oTrackedObject);
        }

        ChangeState(State.NotDoingAnything, null);
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
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    //*************************************************************************
    //  Delegate: ToopTipTrackerEvent
    //
    /// <summary>
    /// Represents a method that will handle an event fired by <see
    /// cref="ToolTipTrackerBase" />.
    /// </summary>
    ///
    /// <param name="source">
    /// Source of the event.
    /// </param>
    ///
    /// <param name="toolTipTrackerEventArgs">
    /// Provides information about the object for which a tooltip window should
    /// be shown or hidden.
    /// </param>
    ///
    /// <remarks>
    /// This delegate is used in all events fired by ToolTipTracker.
    /// </remarks>
    //*************************************************************************

    public delegate void ToolTipTrackerEvent(Object source,
        ToolTipTrackerEventArgs toolTipTrackerEventArgs);


    //*************************************************************************
    //  Event: ShowToolTip
    //
    /// <summary>
    /// Fired when a tooltip window should be shown.
    /// </summary>
    //*************************************************************************

    public event ToolTipTrackerEvent ShowToolTip;


    //*************************************************************************
    //  Event: HideToolTip
    //
    /// <summary>
    /// Fired when a tooltip window should be hidden.
    /// </summary>
    //*************************************************************************

    public event ToolTipTrackerEvent HideToolTip;


    //*************************************************************************
    //  Method: ValidateDelayProperty
    //
    /// <summary>
    /// Validates one of the xxDelayMs properties.
    /// </summary>
    ///
    /// <param name="iValue">
    /// Property value.
    /// </param>
    ///
    /// <param name="sPropertyName">
    /// Name of the property being validated.  Sample: "ShowDelayMs".
    /// </param>
    ///
    /// <remarks>
    /// This throws an exception if the value is out of range.
    /// </remarks>
    //*************************************************************************

    protected void
    ValidateDelayProperty
    (
        Int32 iValue,
        String sPropertyName
    )
    {
        if (iValue < MinDelayMs || iValue > MaxDelayMs)
        {
            throw new ArgumentOutOfRangeException(sPropertyName, iValue,
                "ToolTipTrackerBase." + sPropertyName + ": Must be between "
                + MinDelayMs + " and " + MaxDelayMs + ".");
            }
    }

    //*************************************************************************
    //  Method: ChangeState()
    //
    /// <summary>
    /// Changes the state of the object.
    /// </summary>
    ///
    /// <param name="iState">
    /// New object state.
    /// </param>
    ///
    /// <param name="oObjectToTrack">
    /// Object to track, or null to stop tracking.
    /// </param>
    //*************************************************************************

    protected void
    ChangeState
    (
        State iState,
        Object oObjectToTrack
    )
    {
        AssertValid();

        m_oTimer.Stop();

        m_iState = iState;
        m_oTrackedObject = oObjectToTrack;

        const Int32 NoTimeout = -1;
        Int32 iTimeoutMs;

        switch (iState)
        {
            case State.NotDoingAnything:

                iTimeoutMs = NoTimeout;
                break;

            case State.WaitingForShowTimeout:

                iTimeoutMs = m_iShowDelayMs;
                break;

            case State.WaitingForHideTimeout:

                iTimeoutMs = m_iHideDelayMs;
                break;

            case State.WaitingForReshowTimeout:

                iTimeoutMs = m_iReshowDelayMs;
                break;

            default:

                Debug.Assert(false);
                iTimeoutMs = NoTimeout;
                break;
        }

        if (iTimeoutMs != NoTimeout)
        {
            m_oTimer.Interval = iTimeoutMs;
            m_oTimer.Start();
        }

        AssertValid();
    }

    //*************************************************************************
    //  Method: FireShowToolTipEvent()
    //
    /// <summary>
    /// Fires the ShowToolTip event.
    /// </summary>
    ///
    /// <param name="oObject">
    /// Object to show a tooltip for.
    /// </param>
    //*************************************************************************

    protected void
    FireShowToolTipEvent
    (
        Object oObject
    )
    {
        Debug.Assert(oObject != null);

        if (ShowToolTip != null)
        {
            // Fire a ShowToolTip event.

            ShowToolTip( this, new ToolTipTrackerEventArgs(oObject) );
        }
    }

    //*************************************************************************
    //  Method: FireHideToolTipEvent()
    //
    /// <summary>
    /// Fires the HideToolTip event.
    /// </summary>
    ///
    /// <param name="oObject">
    /// Object to hide the tooltip for.
    /// </param>
    //*************************************************************************

    protected void
    FireHideToolTipEvent
    (
        Object oObject
    )
    {
        Debug.Assert(oObject != null);

        if (HideToolTip != null)
        {
            // Fire a HideToolTip event.

            HideToolTip( this, new ToolTipTrackerEventArgs(oObject) );

        }
    }

    //*************************************************************************
    //  Method: TimerTick()
    //
    /// <summary>
    /// Timer event handler.
    /// </summary>
    ///
    /// <param name="oSource">
    /// Source of the event.
    /// </param>
    /// 
    /// <param name="oEventArgs">
    /// Standard event arguments.
    /// </param>
    //*************************************************************************

    protected void
    TimerTick
    (
        Object oSource,
        EventArgs oEventArgs
    )
    {
        AssertValid();

        m_oTimer.Stop();

        switch (m_iState)
        {
            case State.NotDoingAnything:

                // Nothing is being tracked.

                Debug.Assert(false);

                break;

            case State.WaitingForShowTimeout:

                // An object is being tracked and we're waiting to show its
                // tooltip.
                
                // Show it.

                FireShowToolTipEvent(m_oTrackedObject);

                // Fire a HideToolTip event after m_iHideTimeoutMs.

                ChangeState(State.WaitingForHideTimeout, m_oTrackedObject);

                break;

            case State.WaitingForHideTimeout:

                // A tooltip for m_oTrackedObject is currently being shown and
                // we're waiting to hide it.
                
                // Hide it.

                FireHideToolTipEvent(m_oTrackedObject);

                // Wait up to m_iReshowDelayMs for another object to
                // track.

                ChangeState(State.WaitingForReshowTimeout, null);

                break;

            case State.WaitingForReshowTimeout:

                // A tooltip was recently hidden and we're waiting for another
                // object to track.

                // Stop the timer.

                ChangeState(State.NotDoingAnything, null);

                break;

            default:

                Debug.Assert(false);
                break;
        }
    }

    //*************************************************************************
    //  Method: Dispose()
    //
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing,
    /// or resetting unmanaged resources.
    /// </summary>
    ///
    /// <param name="bDisposing">
    /// See IDisposable.
    /// </param>
    //*************************************************************************

    protected void
    Dispose
    (
        Boolean bDisposing
    )
    {
        if (!m_bDisposed)
        {
            if (bDisposing)
            {
                // Stop the timer from firing events to a window that may no
                // longer have a handle.

                m_oTimer.Stop();
                m_oTimer.Dispose();
            }
        }

        m_bDisposed = true;         
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
        Debug.Assert(m_iShowDelayMs >= MinDelayMs);
        Debug.Assert(m_iShowDelayMs <= MaxDelayMs);

        Debug.Assert(m_iHideDelayMs >= MinDelayMs);
        Debug.Assert(m_iHideDelayMs <= MaxDelayMs);

        Debug.Assert(m_iReshowDelayMs >= MinDelayMs);
        Debug.Assert(m_iReshowDelayMs <= MaxDelayMs);

        switch (m_iState)
        {
            case State.NotDoingAnything:

                Debug.Assert(m_oTrackedObject == null);
                break;

            case State.WaitingForShowTimeout:

                Debug.Assert(m_oTrackedObject != null);
                break;

            case State.WaitingForHideTimeout:

                Debug.Assert(m_oTrackedObject != null);
                break;

            case State.WaitingForReshowTimeout:

                Debug.Assert(m_oTrackedObject == null);
                break;

            default:

                Debug.Assert(false);
                break;
        }

        // m_oTimer
        // m_bDisposed
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// <summary>
    /// Minimum value for the <see cref="ShowDelayMs" />, <see
    /// cref="HideDelayMs" />, and <see cref="ReshowDelayMs" /> properties.
    /// </summary>

    public const Int32 MinDelayMs = 1;

    /// <summary>
    /// Maximum value for the <see cref="ShowDelayMs" />, <see
    /// cref="HideDelayMs" />, and <see cref="ReshowDelayMs" /> properties.
    /// </summary>

    public const Int32 MaxDelayMs = 10000;


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Default value for the ShowDelayMs property.

    protected const Int32 DefaultShowDelayMs = 500;

    /// Default value for the HideDelayMs property.

    protected const Int32 DefaultHideDelayMs = 5000;

    /// Default value for the ReshowDelayMs property.

    protected const Int32 DefaultReshowDelayMs = 50;


    //*************************************************************************
    //  Protected enumerations
    //*************************************************************************

    // States that this object can be in.

    protected enum State
    {
        // Nothing is being tracked.

        NotDoingAnything,

        // An object is being tracked and we're waiting to show its tooltip.

        WaitingForShowTimeout,

        // A tooltip for m_oTrackedObject is currently being shown and we're
        // waiting to hide it.

        WaitingForHideTimeout,

        // A tooltip was recently hidden and we're waiting for another object
        // to track.

        WaitingForReshowTimeout
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // Number of milliseconds to wait to fire the ShowToolTip event after
    // OnMouseMoveOverObject(oObjectToTrack) is first called.

    protected Int32 m_iShowDelayMs;

    // Number of milliseconds to wait to fire the HideToolTip event after
    // ShowToolTip is fired.

    protected Int32 m_iHideDelayMs;

    // Period after a HideToolTip event during which a ShowToolTip event will
    // be fired immediately if the mouse is moved over another object.  If this
    // period elapses without OnMouseMoveOverObject(oObjectToTrack) being
    // called, the waiting period reverts to m_iShowDelayMs.

    protected Int32 m_iReshowDelayMs;

    // State the object is in.

    protected State m_iState;

    // Object being tracked, or null for none.

    protected Object m_oTrackedObject;

    // Timer.

    protected IToolTipTimer m_oTimer;

    // Used to implement IDispose.

    protected Boolean m_bDisposed;
}


//*****************************************************************************
//  Class: ToolTipTrackerEventArgs
//
/// <summary>
/// Event arguments of <see cref="ToolTipTrackerBase" /> events.
/// </summary>
//*****************************************************************************

internal class ToolTipTrackerEventArgs : EventArgs
{
    //*************************************************************************
    //  Constructor: ToolTipTrackerEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ToolTipTrackerEventArgs" /> class.
    /// </summary>
    ///
    /// <param name="trackedObject">
    /// Object for which a tooltip window should be shown or hidden.
    /// </param>
    //*************************************************************************

    public ToolTipTrackerEventArgs
    (
        Object trackedObject
    )
    {
        m_oTrackedObject = trackedObject;
    }

    //*************************************************************************
    //  Property: Object
    //
    /// <summary>
    /// Gets the Object for which a tooltip window should be shown or hidden.
    /// </summary>
    ///
    /// <value>
    /// The Object for which a tooltip window should be shown or hidden.
    /// </value>
    ///
    /// <remarks>
    /// This should be called TrackedObject to make it consistent with the
    /// <see cref="ToolTipTrackerBase.TrackedObject" /> property.  It is called
    /// Object for backward compatibility with older projects.
    /// </remarks>
    //*************************************************************************

    public Object Object
    {
        get
        {
            AssertValid();

            return (m_oTrackedObject);
        }
    }

    //***m**********************************************************************
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
        Debug.Assert(m_oTrackedObject != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // Object for which a tooltip window should be shown or hidden.

    protected Object m_oTrackedObject;
}


//*****************************************************************************
//  Interface: IToolTipTimer
//
/// <summary>
/// Defines the timer interface used by <see cref="ToolTipTrackerBase" />.
/// </summary>
///
/// <remarks>
/// The <see cref="ToolTipTrackerBase" /> class uses an internal timer.  To
/// allow the class to be used in both Windows Forms and WPF applications, the
/// <see cref="ToolTipTrackerBase" /> constructor accepts an <see
/// cref="IToolTipTimer" /> argument that wraps either a Windows Forms timer or
/// a WPF DispatcherTimer, without <see cref="ToolTipTrackerBase" /> knowing
/// which timer is being used.
/// </remarks>
//*****************************************************************************

public interface IToolTipTimer
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

    Int32
    Interval
    {
        get;
        set;
    }

    //*************************************************************************
    //  Method: Start()
    //
    /// <summary>
    /// Starts the timer.
    /// </summary>
    //*************************************************************************

    void
    Start();

    //*************************************************************************
    //  Method: Stop()
    //
    /// <summary>
    /// Stops the timer.
    /// </summary>
    //*************************************************************************

    void
    Stop();

    //*************************************************************************
    //  Method: Dispose()
    //
    /// <summary>
    /// Disposes of the timer.
    /// </summary>
    //*************************************************************************

    void
    Dispose();

    //*************************************************************************
    //  Event: Tick
    //
    /// <summary>
    /// Occurs when the timer interval has elapsed. 
    /// </summary>
    //*************************************************************************

    event EventHandler Tick;
}

}
