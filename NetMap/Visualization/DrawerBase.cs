
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.Visualization
{
//*****************************************************************************
//  Class: DrawerBase
//
/// <summary>
/// Base class for classes that perform drawing operations.
/// </summary>
///
/// <remarks>
///	This class implements the <see cref="IDisposable" /> interface.  The
/// derived class should override <see cref="DisposeManagedObjects" /> if it
/// maintains managed objects that should be disposed.
/// </remarks>
//*****************************************************************************

public class DrawerBase : VisualizationBase, IDisposable
{
    //*************************************************************************
    //  Constructor: DrawerBase()
    //
    /// <summary>
    /// Initializes a new instance of the DrawerBase class.
    /// </summary>
    //*************************************************************************

    public DrawerBase()
    {
		m_bDisposed = false;

		// AssertValid();
    }

	//*************************************************************************
	//	Destructor: ~DrawerBase()
	//
	/// <summary>
	/// DrawerBase destructor.
	/// </summary>
	//*************************************************************************

	~DrawerBase()      
	{
		Dispose(false);
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
	//	Event: RedrawRequired
	//
	/// <summary>
	///	Occurs when a change occurs that requires a graph redraw.
	/// </summary>
	///
	/// <remarks>
	///	The event is fired when a change is made to the object that might
	/// affect the appearance of the graph.  The object owner should redraw the
	/// graph but does not have to lay out the graph again.
	/// </remarks>
	//*************************************************************************

	public event EventHandler RedrawRequired;


	//*************************************************************************
	//	Event: LayoutRequired
	//
	/// <summary>
	///	Occurs when a change occurs that requires the graph to be laid out
	/// again.
	/// </summary>
	///
	/// <remarks>
	///	The event is fired when any change is made to the object that might
	/// affect the layout of the graph.  The owner should lay out the graph and
	/// redraw it in response to the event.
	/// </remarks>
	//*************************************************************************

	public event EventHandler LayoutRequired;


	//*************************************************************************
	//	Method: FireRedrawRequired()
	//
	/// <summary>
	///	Fires the <see cref="RedrawRequired" /> event if appropriate.
	/// </summary>
	//*************************************************************************

	protected void
	FireRedrawRequired()
	{
		AssertValid();

		EventUtil.FireEvent(this, this.RedrawRequired);
	}

	//*************************************************************************
	//	Method: FireLayoutRequired()
	//
	/// <summary>
	///	Fires the <see cref="LayoutRequired" /> event if appropriate.
	/// </summary>
	//*************************************************************************

	protected void
	FireLayoutRequired()
	{
		AssertValid();

		EventUtil.FireEvent(this, this.LayoutRequired);
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
				DisposeManagedObjects();
			}
		}

		m_bDisposed = true;         
	}

	//*************************************************************************
	//	Method: DisposeManagedObjects()
	//
	/// <summary>
	/// Disposes of managed objects used for drawing.
	/// </summary>
	///
	/// <remarks>
	/// The derived class should override this method if it maintains managed
	/// objects used for drawing.
	/// </remarks>
	//*************************************************************************

	protected virtual void
	DisposeManagedObjects()
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

		// m_bDisposed
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Used to implement IDispose.

	protected Boolean m_bDisposed;
}

}
