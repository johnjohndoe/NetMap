
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.GraphicsLib
{
//*****************************************************************************
//	Class: ToolTipTracker
//
/// <summary>
/// Helper class for displaying tooltips.
/// </summary>
///
/// <remarks>
///	This is meant for use by a Control object that displays various objects
///	within its window and wants to show a tooltip for each object.  The ToolTip
///	class in the FCL makes it easy to show a single tooltip for an entire
///	control, but it does not support different tooltips for different parts of
///	the control's window.
///
/// <para>
///	To use ToolTipTracker, call <see cref="OnMouseMoveOverObject" /> from the
/// control's MouseMove event handler.  If the mouse is currently over an
/// object that has a tooltip associated with it, pass the object as the
///	method's oObjectToTrack parameter.  Otherwise, pass null.  Also, call
///	<see cref="OnMouseMoveOverObject" /> with a null parameter from the
///	control's MouseLeave event handler.
/// </para>
///
/// <para>
/// If the mouse remains over an object for a period of <see
///	cref="ShowDelayMs" /> milliseconds, ToolTipTracker fires a <see
///	cref="ShowToolTip" /> event.  The event arguments include the object being
///	tracked.
/// </para>
///
/// <para>
///	A <see cref="HideToolTip" /> event is fired when the tooltip should be
///	hidden.  This occurs <see cref="HideDelayMs" /> after the <see
///	cref="ShowToolTip" /> event fires if the mouse remains over the object, or
///	immediately if OnMouseMoveOverObject(null) is called.
/// </para>
///
/// <para>
///	Note that ToolTipTracker does not actually show or hide the tooltip; that's
///	up to the application.  The easiest way to do this is to create a child
///	ToolTipPanel control and call its Show and Hide methods in response to the
/// <see cref="ShowToolTip" /> and <see cref="HideToolTip" /> events.
/// </para>
///
/// <para>
/// If the mouse is moved to another object within <see cref="ReshowDelayMs" />
///	milliseconds, another <see cref="ShowToolTip" /> event is fired.
///	Otherwise, the waiting period reverts to <see cref="ShowDelayMs" />.
/// </para>
///
///	<para>
///	Call <see cref="Reset" /> to reset ToolTipTracker to its initial state.
///	This forces a <see cref="HideToolTip" /> event if a tooltip is showing.
///	</para>
///
///	<para>
///	<b>IMPORTANT</b>
///	</para>
///
///	<para>
///	The control must call <see cref="Dispose()" /> from its own Dispose method.
/// This prevents timer-based events from firing after the control no longer
/// has a handle.
///	</para>
///
/// </remarks>
//*****************************************************************************

internal class ToolTipTracker : Object, IDisposable
{
	//*************************************************************************
	//	Public constants
	//*************************************************************************

	// Minimum and maximum values for ShowDelayMs, HideDelayMs, and
	// ReshowDelayMs properties.

	public const Int32 MinDelayMs = 1;
	public const Int32 MaxDelayMs = 10000;


	//*************************************************************************
	//	Constructor: ToolTipTracker()
	//
	/// <summary>
	/// ToolTipTracker constructor.
	/// </summary>
	//*************************************************************************

	public ToolTipTracker()
	{
		m_iShowDelayMs = DefaultShowDelayMs;
		m_iHideDelayMs = DefaultHideDelayMs;
		m_iReshowDelayMs = DefaultReshowDelayMs;
		m_iState = State.NotDoingAnything;
		m_oObjectBeingTracked = null;
		m_bDisposed = false;

		// Create a timer but don't start it yet.

		m_oTimer = new System.Windows.Forms.Timer();
		m_oTimer.Tick += new EventHandler(TimerTick);
	}

	//*************************************************************************
	//	Destructor: ~ToolTipTracker()
	//
	/// <summary>
	/// ToolTipTracker destructor.
	/// </summary>
	//*************************************************************************

	~ToolTipTracker()      
	{
		Dispose(false);
	}

	//*************************************************************************
	//	Property: ShowDelayMs
	//
	/// <summary>
	/// ShowDelayMs property.
	/// </summary>
	///
	/// <value>
	/// Int32.  Number of milliseconds to wait to fire the ShowToolTip event
	///	after OnMouseMoveOverObject(oObjectToTrack) is first called.
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
		}
	}

	//*************************************************************************
	//	Property: HideDelayMs
	//
	/// <summary>
	/// HideDelayMs property.
	/// </summary>
	///
	/// <value>
	/// Int32.  Number of milliseconds to wait to fire the HideToolTip event
	///	after ShowToolTip is fired.
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
		}
	}

	//*************************************************************************
	//	Property: ReshowDelayMs
	//
	/// <summary>
	/// ReshowDelayMs property.
	/// </summary>
	///
	/// <value>
	/// Int32.  Period after a HideToolTip event during which a ShowToolTip
	/// event will be fired immediately if the mouse is moved over another
	///	object.  If this period elapses without
	///	OnMouseMoveOverObject(oObjectToTrack) being called, the waiting period
	///	reverts to m_iShowDelayMs.
	/// </value>
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
		}
	}

	//*************************************************************************
	//	Method: OnMouseMoveOverObject()
	//
	/// <summary>
	/// OnMouseMoveOverObject method.
	/// </summary>
	///
	/// <param name="oObjectToTrack">
	/// Object.  Object to track, or null to stop tracking.
	/// </param>
	///
	/// <remarks>
	///	Call this with an Object parameter when the mouse moves over an object
	///	that should be tracked.  Call it with a null parameter when the mouse
	///	moves over an area of the control where there is no object, and when
	///	the mouse leaves the control.
	/// </remarks>
	//*************************************************************************

	public void
	OnMouseMoveOverObject
	(
		Object oObjectToTrack
	)
	{
		AssertValid();

		// Debug.WriteLine("OnMouseMoveOverObject: " + oObjectToTrack);

		switch (m_iState)
		{
			case State.NotDoingAnything:

				// Nothing is being tracked.

				if (oObjectToTrack != null)
				{
					// Fire a ShowToolTip event after m_iShowDelayMs.

					ChangeState(State.WaitingForShowTimeout, oObjectToTrack);
				}

				break;

			case State.WaitingForShowTimeout:

				// An object is being tracked and we're waiting to show its
				// tooltip.

				if (oObjectToTrack == null)
				{
					// Stop the timer.

					ChangeState(State.NotDoingAnything, null);
				}
				else if (oObjectToTrack == m_oObjectBeingTracked)
				{
					// (No need to do anything, just keep waiting.)
				}
				else
				{
					// Start tracking the new object.  Fire a ShowToolTip
					// event after m_iShowDelayMs.

					ChangeState(State.WaitingForShowTimeout, oObjectToTrack);
				}

				break;

			case State.WaitingForHideTimeout:

				// A tooltip for m_oObjectBeingTracked is currently being
				// shown and we're waiting to hide it.

				if (oObjectToTrack == null)
				{
					// Hide the tooltip.

					FireHideToolTipEvent(m_oObjectBeingTracked);

					// Wait up to m_iReshowDelayMs for another object to
					// track.

					ChangeState(State.WaitingForReshowTimeout, null);
				}
				else if (oObjectToTrack == m_oObjectBeingTracked)
				{
					// The mouse was moved within the same object.  Restart the
					// timer.

					ChangeState(State.WaitingForHideTimeout, oObjectToTrack);
				}
				else
				{
					// Hide the old tooltip.

					FireHideToolTipEvent(m_oObjectBeingTracked);

					// Show a new tooltip.

					FireShowToolTipEvent(oObjectToTrack);

					// Fire a HideToolTip event after m_iHideDelayMs.

					ChangeState(State.WaitingForHideTimeout, oObjectToTrack);
				}

				break;

			case State.WaitingForReshowTimeout:

				// A tooltip was recently hidden and we're waiting for another
				// object to track.

				if (oObjectToTrack == null)
				{
					// (No need to do anything, just keep waiting.)
				}
				else
				{
					// Show a new tooltip.

					FireShowToolTipEvent(oObjectToTrack);

					// Fire a HideToolTip event after m_iHideDelayMs.

					ChangeState(State.WaitingForHideTimeout, oObjectToTrack);
				}

				break;

			default:

				Debug.Assert(false);
				break;
		}
	}

	//*************************************************************************
	//	Method: Reset()
	//
	/// <summary>
	/// Reset method.
	/// </summary>
	///
	/// <remarks>
	///	Resets the object to its initial state.  This forces a HideToolTip
	///	event if a tooltip is showing.
	/// </remarks>
	//*************************************************************************

	public void
	Reset()
	{
		AssertValid();

		m_oTimer.Stop();

		if (m_iState == State.WaitingForHideTimeout)
		{
			// A tooltip for m_oObjectBeingTracked is currently being
			// shown and we're waiting to hide it.

			// Hide the tooltip immediately.

			FireHideToolTipEvent(m_oObjectBeingTracked);
		}

		ChangeState(State.NotDoingAnything, null);
	}

	//*************************************************************************
	//	Method: Dispose()
	//
	/// <summary>
	/// Dispose method.
	/// </summary>
	///
	/// <remarks>
	///	Performs application-defined tasks associated with freeing, releasing,
	/// or resetting unmanaged resources.
	/// </remarks>
	//*************************************************************************

	public void
	Dispose()
	{
		Dispose(true);

		GC.SuppressFinalize(this);
	}

	//*************************************************************************
	//	Delegate: ToopTipTrackerEvent
	//
	/// <summary>
	/// ToolTipTrackerEvent delegate.
	/// </summary>
	///
	/// <param name="oSource">
	/// Object.  Source of the event.
	/// </param>
	///
	/// <param name="oToolTipTrackerEventArgs">
	/// ToolTipTrackerEventArgs.  Provides information about the object for
	///	which a tooltip window should be shown or hidden.
	/// </param>
	///
	/// <remarks>
	///	This delegate is used in all events fired by ToolTipTracker.
	/// </remarks>
	//*************************************************************************

	public delegate void ToolTipTrackerEvent(Object oSource,
		ToolTipTrackerEventArgs oToolTipTrackerEventArgs);

	//*************************************************************************
	//	Event: ShowToolTip
	//
	/// <summary>
	/// ShowToolTip event.
	/// </summary>
	///
	/// <remarks>
	///	Fired when a tooltip window should be shown.
	/// </remarks>
	//*************************************************************************

	public event ToolTipTrackerEvent ShowToolTip;

	//*************************************************************************
	//	Event: HideToolTip
	//
	/// <summary>
	/// HideToolTip event.
	/// </summary>
	///
	/// <remarks>
	///	Fired when a tooltip window should be hidden.
	/// </remarks>
	//*************************************************************************

	public event ToolTipTrackerEvent HideToolTip;

	//*************************************************************************
	//	Method: ValidateDelayProperty
	//
	/// <summary>
	/// ValidateDelayProperty method.
	/// </summary>
	///
	/// <param name="iValue">
	/// Int32.  Property value.
	/// </param>
	///
	/// <param name="sPropertyName">
	/// String.  Name of the property being validated.  Sample: "ShowDelayMs".
	/// </param>
	///
	/// <remarks>
	/// Validates one of the xxDelayMs properties.  Throws an exception if the
	///	value is out of range.
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
				"ToolTipTracker." + sPropertyName + ": Must be between "
				+ MinDelayMs + " and " + MaxDelayMs + ".");
			}
	}

	//*************************************************************************
	//	Method: ChangeState()
	//
	/// <summary>
	/// ChangeState method.
	/// </summary>
	///
	/// <param name="iState">
	/// State.  New object state.
	/// </param>
	///
	/// <param name="oObjectToTrack">
	/// Object.  Object to track, or null to stop tracking.
	/// </param>
	///
	/// <remarks>
	///	Changes the state of the object.
	/// </remarks>
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
		m_oObjectBeingTracked = oObjectToTrack;

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
	//	Method: FireShowToolTipEvent()
	//
	/// <summary>
	/// FireShowToolTipEvent method.
	/// </summary>
	///
	/// <param name="oObject">
	/// Object.  Object to show a tooltip for.
	/// </param>
	///
	/// <remarks>
	///	Fires the ShowToolTip event.
	/// </remarks>
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
	//	Method: FireHideToolTipEvent()
	//
	/// <summary>
	/// FireHideToolTipEvent method.
	/// </summary>
	///
	/// <param name="oObject">
	/// Object.  Object to hide the tooltip for.
	/// </param>
	///
	/// <remarks>
	///	Fires the HideToolTip event.
	/// </remarks>
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
	//	Method: TimerTick()
	//
	/// <summary>
	/// TimerTick method.
	/// </summary>
	///
	/// <param name="oSource">
	/// Object.  Source of the event.
	/// </param>
	/// 
	/// <param name="oEventArgs">
	/// Standard event arguments.
	/// </param>
	///
	/// <remarks>
	///	Timer event handler.
	/// </remarks>
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

				FireShowToolTipEvent(m_oObjectBeingTracked);

				// Fire a HideToolTip event after m_iHideTimeoutMs.

				ChangeState(State.WaitingForHideTimeout, m_oObjectBeingTracked);

				break;

			case State.WaitingForHideTimeout:

				// A tooltip for m_oObjectBeingTracked is currently being
				// shown and we're waiting to hide it.
				
				// Hide it.

				FireHideToolTipEvent(m_oObjectBeingTracked);

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
	//	Method: Dispose()
	//
	/// <summary>
	/// Dispose method.
	/// </summary>
	///
	/// <remarks>
	///	Performs application-defined tasks associated with freeing, releasing,
	/// or resetting unmanaged resources.
	/// </remarks>
	///
	/// <param name="bDisposing">
	///	See IDisposable.
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
	//	Method: AssertValid()
	//
	/// <summary>
	/// AssertValid method.
	/// </summary>
	///
	/// <remarks>
	///	Asserts if the object is in an invalid state.  Debug-only.
	/// </remarks>
	//*************************************************************************

	[Conditional("DEBUG")] 

	public void
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

				Debug.Assert(m_oObjectBeingTracked == null);
				break;

			case State.WaitingForShowTimeout:

				Debug.Assert(m_oObjectBeingTracked != null);
				break;

			case State.WaitingForHideTimeout:

				Debug.Assert(m_oObjectBeingTracked != null);
				break;

			case State.WaitingForReshowTimeout:

				Debug.Assert(m_oObjectBeingTracked == null);
				break;

			default:

				Debug.Assert(false);
				break;
		}

		// m_oTimer
		// m_bDisposed
	}


	//*************************************************************************
	//	Protected constants
	//*************************************************************************

	/// Default value for the ShowDelayMs property.

	protected const Int32 DefaultShowDelayMs = 500;

	/// Default value for the HideDelayMs property.

	protected const Int32 DefaultHideDelayMs = 5000;

	/// Default value for the ReshowDelayMs property.

	protected const Int32 DefaultReshowDelayMs = 50;


	//*************************************************************************
	//	Protected enumerations
	//*************************************************************************

	// States that this object can be in.

	protected enum State
	{
		// Nothing is being tracked.

		NotDoingAnything,

		// An object is being tracked and we're waiting to show its tooltip.

		WaitingForShowTimeout,

		// A tooltip for m_oObjectBeingTracked is currently being shown and
		// we're waiting to hide it.

		WaitingForHideTimeout,

		// A tooltip was recently hidden and we're waiting for another object
		// to track.

		WaitingForReshowTimeout
	}

	//*************************************************************************
	//	Protected field
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

	protected Object m_oObjectBeingTracked;

	// Timer.

	protected System.Windows.Forms.Timer m_oTimer;

	// Used to implement IDispose.

	protected Boolean m_bDisposed;
}


//*****************************************************************************
//	Class: ToolTipTrackerEventArgs
//
/// <summary>
///	ToolTipTrackerEventArgs class.
/// </summary>
///
/// <remarks>
///	Events fired by the ToolTipTracker object include a ToolTipTrackerEventArgs
///	object as one of the event arguments.
/// </remarks>
//*****************************************************************************

internal class ToolTipTrackerEventArgs : EventArgs
{
	//*************************************************************************
	//	Constructor: ToolTipTrackerEventArgs()
	//
	/// <summary>
	/// ToolTipTrackerEventArgs constructor.
	/// </summary>
	///
	/// <param name="oObject">
	/// Object.  Object for which a tooltip window should be shown or hidden.
	/// </param>
	//*************************************************************************

	public ToolTipTrackerEventArgs
	(
		Object oObject
	)
	{
		m_oObject = oObject;
	}

	//*************************************************************************
	//	Property: Object
	//
	/// <summary>
	/// Object property.
	/// </summary>
	///
	/// <value>
	/// Object.  Returns the Object for which a tooltip window should be shown
	///	or hidden.  Read-only.
	/// </value>
	//*************************************************************************

	public Object Object
	{
		get
		{
			AssertValid();

			return (m_oObject);
		}
	}

	//***m**********************************************************************
	//	Method: AssertValid()
	//
	/// <summary>
	/// AssertValid method.
	/// </summary>
	///
	/// <remarks>
	///	Asserts if the object is in an invalid state.  Debug-only.
	/// </remarks>
	//*************************************************************************

	[Conditional("DEBUG")] 

	public void
	AssertValid()
	{
		Debug.Assert(m_oObject != null);
	}


	//*************************************************************************
	//	Protected fields
	//*************************************************************************

	// Object for which a tooltip window should be shown or hidden.

	Object m_oObject;
}

}
