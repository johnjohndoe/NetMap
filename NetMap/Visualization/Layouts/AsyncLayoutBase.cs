
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Threading;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.Visualization
{
//*****************************************************************************
//  Class: AsyncLayoutBase
//
/// <summary>
///	Base class for asynchronous layouts.
/// </summary>
///
/// <remarks>
/// This abstract class can serve as a base class for <see
/// cref="IAsyncLayout" /> implementations.  Its implementations of the <see
/// cref="IAsyncLayout" /> public methods provide error checking but defer the
/// actual work to protected abstract methods.
/// </remarks>
//*****************************************************************************

public abstract class AsyncLayoutBase : LayoutBase, IAsyncLayout
{
    //*************************************************************************
    //  Constructor: AsyncLayoutBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncLayoutBase" /> class.
    /// </summary>
    //*************************************************************************

    public AsyncLayoutBase()
    {
		// Create the BackgroundWorker used by LayOutGraphAsync() and handle
		// its DoWork event.

		m_oBackgroundWorker = new BackgroundWorker();

		m_oBackgroundWorker.WorkerSupportsCancellation  = true;

		m_oBackgroundWorker.DoWork += new DoWorkEventHandler(
			this.BackgroundWorker_DoWork);

		m_oBackgroundWorker.RunWorkerCompleted +=
			new RunWorkerCompletedEventHandler(
				this.BackgroundWorker_RunWorkerCompleted);

		m_oSynchronizationContext = null;

		// AssertValid();
    }

    //*************************************************************************
    //  Property: IsBusy
    //
    /// <summary>
	/// Gets a value indicating whether an asynchronous operation is in
	/// progress.
    /// </summary>
    ///
    /// <value>
    /// true if an asynchronous operation is in progress.
    /// </value>
    //*************************************************************************

    public Boolean
    IsBusy
    {
        get
        {
            AssertValid();

            return (m_oBackgroundWorker.IsBusy);
        }
    }

    //*************************************************************************
    //  Method: LayOutGraphAsync()
    //
    /// <summary>
    /// Lays out a graph asynchronously.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to lay out.
    /// </param>
    ///
    /// <param name="layoutContext">
    /// Provides access to objects needed to lay out the graph.
    /// </param>
    ///
    /// <remarks>
	///	This method asynchronously lays out the graph <paramref name="graph" />
	/// by setting the <see cref="IVertex.Location" /> property on all of the
	/// graph's vertices, and optionally adding geometry metadata to the graph,
	/// vertices, or edges.  It starts a worker thread and then returns
	/// immediately.
	///
	/// <para>
	/// The <see cref="LayOutGraphIterationCompleted" /> event may fire
	/// repeatedly while the layout is occurring.  The <see
	/// cref="LayOutGraphCompleted" /> event fires when the layout is complete,
	/// an error occurs, or the layout is cancelled.  <see
	/// cref="LayOutGraphAsyncCancel" /> cancels the layout.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    LayOutGraphAsync
    (
		IGraph graph,
		LayoutContext layoutContext
    )
	{
		AssertValid();

		const String MethodName = "LayOutGraphAsync";

		this.ArgumentChecker.CheckArgumentNotNull(MethodName, "graph", graph);

		this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "layoutContext", layoutContext);

		if (m_oBackgroundWorker.IsBusy)
		{
			throw new InvalidOperationException(String.Format(

				"{0}.{1}: A layout is already in progress."
				,
				this.ClassName,
				MethodName
				) );
		}

		// Save the SynchronizationContext of the calling thread for use by
		// FireLayOutGraphIterationCompleted().

		Debug.Assert(m_oSynchronizationContext == null);

		m_oSynchronizationContext = SynchronizationContext.Current;

        Debug.Assert(m_oSynchronizationContext != null);

		// Start a worker thread, then return immediately.

        m_oBackgroundWorker.RunWorkerAsync(
			new LayOutGraphAsyncArguments(graph, layoutContext) );
	}

    //*************************************************************************
    //  Method: LayOutGraphAsyncCancel()
    //
    /// <summary>
    /// Cancels the layout started by <see cref="LayOutGraphAsync" />.
    /// </summary>
    ///
    /// <remarks>
	/// The layout may or may not cancel, but the <see
	/// cref="LayOutGraphCompleted" /> event is guaranteed to fire.  The <see
	/// cref="AsyncCompletedEventArgs" /> object passed to the event handler
	/// contains a <see cref="AsyncCompletedEventArgs.Cancelled" /> property
	/// that indicates whether the cancellation occurred.
	///
	/// <para>
	/// If a layout is not in progress, this method does nothing.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    public void
    LayOutGraphAsyncCancel()
	{
		AssertValid();

		if (!m_oBackgroundWorker.IsBusy)
		{
			return;
		}

		m_oBackgroundWorker.CancelAsync();
	}

	//*************************************************************************
	//	Event: LayOutGraphIterationCompleted
	//
	/// <summary>
	///	Occurs when a layout started by <see cref="LayOutGraphAsync" />
	/// completes one iteration.
	/// </summary>
	///
	/// <remarks>
	/// If the implementation uses an iterative layout algorithm, it should
	/// fire this event after each iteration.  The event handler may draw the
	/// intermediate graph using the <see cref="IVertex.Location" /> property
	/// on all of the graph's vertices.
	///
	/// <para>
	/// The event fires on the thread on which <see cref="LayOutGraphAsync" />
	/// was called.  Although the <see cref="IVertex.Location" /> property is
	/// not required to be thread-safe, it is safe to read the property during
	/// the event because the implementation's worker thread blocks until the
	/// event handler returns.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public event EventHandler LayOutGraphIterationCompleted;


	//*************************************************************************
	//	Event: LayOutGraphCompleted
	//
	/// <summary>
	///	Occurs when a layout started by <see cref="LayOutGraphAsync" />
	/// completes, is cancelled, or ends with an error.
	/// </summary>
	///
	/// <remarks>
	/// The event fires on the thread on which <see cref="LayOutGraphAsync" />
	/// was called.
	/// </remarks>
	//*************************************************************************

	public event AsyncCompletedEventHandler LayOutGraphCompleted;


    //*************************************************************************
    //  Method: LayOutGraphCore()
    //
    /// <summary>
    /// Lays out a graph.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to lay out.  The graph is guaranteed to have at least one vertex.
    /// </param>
    ///
    /// <param name="layoutContext">
    /// Provides access to objects needed to lay out the graph.  The <see
	/// cref="LayoutContext.GraphRectangle" /> is guaranteed to have non-zero
	/// width and height.
    /// </param>
    ///
    /// <remarks>
	///	This method lays out the graph <paramref name="graph" /> by setting the
	/// <see cref="IVertex.Location" /> property on all of the graph's
	/// vertices, and optionally adding geometry metadata to the graph,
	/// vertices, or edges.
	///
	/// <para>
	/// The arguments have already been checked for validity.
	/// </para>
	///
	/// <para>
	/// This is the concrete implementation of an abstract virtual method
	/// defined in <see cref="LayoutBase" />.  It delegates the work to a new,
	/// overloaded abstract virtual method defined in this class.  The new
	/// overload takes an additional BackgroundWorker argument.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    protected override void
    LayOutGraphCore
    (
		IGraph graph,
		LayoutContext layoutContext
    )
	{
		Debug.Assert(graph != null);
		Debug.Assert(layoutContext != null);
		AssertValid();

		Debug.Assert(graph.Vertices.Count > 0);
		Debug.Assert(layoutContext.GraphRectangle.Width > 0);
		Debug.Assert(layoutContext.GraphRectangle.Height > 0);

		// Let the derived class do the work.

		LayOutGraphCore(graph, layoutContext, null);
	}

    //*************************************************************************
    //  Method: LayOutGraphCore()
    //
    /// <summary>
    /// Lays out a graph synchronously or asynchronously.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to lay out.  The graph is guaranteed to have at least one vertex.
    /// </param>
    ///
    /// <param name="layoutContext">
    /// Provides access to objects needed to lay out the graph.  The <see
	/// cref="LayoutContext.GraphRectangle" /> is guaranteed to have non-zero
	/// width and height.
    /// </param>
    ///
    /// <param name="backgroundWorker">
    /// <see cref="BackgroundWorker" /> whose worker thread called this method
	/// if the graph is being laid out asynchronously, or null if the graph is
	/// being laid out synchronously.
    /// </param>
    ///
    /// <returns>
	/// true if the layout was successfully completed, false if the layout was
	/// cancelled.  The layout can be cancelled only if the graph is being laid
	/// out asynchronously.
    /// </returns>
	///
    /// <remarks>
	///	This method lays out the graph <paramref name="graph" /> either
	/// synchronously (if <paramref name="backgroundWorker" /> is null) or
	/// asynchronously (if (<paramref name="backgroundWorker" /> is not null)
	/// by setting the the <see cref="IVertex.Location" /> property on all of
	/// the graph's vertices and optionally adding geometry metadata to the
	/// graph, vertices, or edges.
	///
	/// <para>
	/// In the asynchronous case, the <see
	/// cref="BackgroundWorker.CancellationPending" /> property on the
	/// <paramref name="backgroundWorker" /> object should be checked before
	/// each layout iteration.  If it's true, the method should immediately
	/// return false.  Also, <see
	/// cref="AsyncLayoutBase.FireLayOutGraphIterationCompleted()" /> should be
	/// called after each iteration.
	/// </para>
	///
	/// <para>
	/// The arguments have already been checked for validity.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    protected abstract Boolean
    LayOutGraphCore
    (
		IGraph graph,
		LayoutContext layoutContext,
		BackgroundWorker backgroundWorker
	);

	//*************************************************************************
	//	Method: FireLayOutGraphIterationCompleted()
	//
	/// <overloads>
	///	Fires the <see cref="LayOutGraphIterationCompleted" /> event if
	/// appropriate.
	/// </overloads>
	///
	/// <summary>
	///	Fires the <see cref="LayOutGraphIterationCompleted" /> event if
	/// appropriate.
	/// </summary>
	///
	/// <remarks>
	/// This method should be called from <see
	/// cref="BackgroundWorker_DoWork" /> after each layout iteration.  It
	/// synchronously fires the <see cref="LayOutGraphIterationCompleted" />
	/// event on the thread from which <see cref="LayOutGraphAsync" /> was
	/// called.
	///
	/// <para>
	/// Note that the <see cref="BackgroundWorker.ReportProgress(Int32)" />
	/// method can't be used to report completion of an iteration.  That's
	/// because <see cref="BackgroundWorker.ReportProgress(Int32)" /> fires a
	/// <see cref="BackgroundWorker.ProgressChanged" /> event asynchronously,
	/// meaning that the worker thread gets back to work immediately without
	/// waiting for the event handler to return.  Because the worker thread is
	/// modifying the Location properties of all the graph's vertices, which is
	/// exactly what the event handler wants to read, the vertex locations
	/// would not be in a stable state for reading.  Using
	/// SynchronizationContext to synchronously Send an event solves this
	/// problem, because the worker thread can't get back to work until the
	/// event handler returns.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	protected void
	FireLayOutGraphIterationCompleted()
	{
		AssertValid();

		// Marshall the call to the thread on which LayOutGraphAsync() was
		// called.

		Debug.Assert(m_oSynchronizationContext != null);

		m_oSynchronizationContext.Send(
			new SendOrPostCallback(this.FireLayOutGraphIterationCompleted),
			null
			);
	}

	//*************************************************************************
	//	Method: FireLayOutGraphIterationCompleted()
	//
	/// <summary>
	///	Fires the <see cref="LayOutGraphIterationCompleted" /> event if
	/// appropriate.
	/// </summary>
	///
	/// <param name="oState">
	/// Required by the SendOrPostCallback method signature, but not used.
	/// </param>
	///
	/// <remarks>
	/// This method can be called only from the thread on which <see
	/// cref="LayOutGraphAsync" /> was called.
	/// </remarks>
	//*************************************************************************

	protected void
	FireLayOutGraphIterationCompleted
	(
		Object oState
	)
	{
		AssertValid();

		EventUtil.FireEvent(this, this.LayOutGraphIterationCompleted);
	}

	//*************************************************************************
	//	Method: FireLayOutGraphCompleted()
	//
	/// <summary>
	///	Fires the <see cref="LayOutGraphCompleted" /> event if appropriate.
	/// </summary>
	///
	/// <param name="oAsyncCompletedEventArgs">
	/// An <see cref="AsyncCompletedEventArgs" /> that contains the event data.
	/// </param>
	//*************************************************************************

	protected void
	FireLayOutGraphCompleted
	(
		AsyncCompletedEventArgs oAsyncCompletedEventArgs
	)
	{
		AssertValid();

		AsyncCompletedEventHandler oEventHandler =
			this.LayOutGraphCompleted;

		if (oEventHandler != null)
		{
			oEventHandler(this, oAsyncCompletedEventArgs);
		}
	}

	//*************************************************************************
	//	Method: BackgroundWorker_DoWork()
	//
	/// <summary>
	/// Handles the DoWork event on the BackgroundWorker object.
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
	BackgroundWorker_DoWork
	(
		Object sender,
		DoWorkEventArgs e
	)
	{
		AssertValid();

		Debug.Assert(sender != null);
		Debug.Assert(sender is BackgroundWorker);
		Debug.Assert(sender == m_oBackgroundWorker);

		BackgroundWorker oBackgroundWorker = (BackgroundWorker)sender;

		Debug.Assert(e.Argument is LayOutGraphAsyncArguments);

		LayOutGraphAsyncArguments oLayOutGraphAsyncArguments =
			(LayOutGraphAsyncArguments)e.Argument;

		IGraph oGraph = oLayOutGraphAsyncArguments.Graph;

		if (oGraph.Vertices.Count == 0)
		{
			return;
		}

        LayoutContext oLayoutContext =
            oLayOutGraphAsyncArguments.LayoutContext;

		// Subtract the margin from the graph rectangle.

		LayoutContext oLayoutContext2;

		if ( !SubtractMarginFromRectangle(oLayoutContext, out oLayoutContext2) )
		{
			return;
		}

		// Let the derived class do the work.

		if ( !LayOutGraphCore(oGraph, oLayoutContext2, oBackgroundWorker) )
		{
			// LayOutGraphAsyncCancel() was called.

			e.Cancel = true;
		}
		else
		{
			// Mark the graph as having been laid out.

			MarkGraphAsLaidOut(oGraph, oLayoutContext);
		}
	}


	//*************************************************************************
	//	Method: BackgroundWorker_RunWorkerCompleted()
	//
	/// <summary>
	/// Handles the RunWorkerCompleted event on the BackgroundWorker object.
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
	BackgroundWorker_RunWorkerCompleted
	(
		Object sender,
		RunWorkerCompletedEventArgs e
	)
	{
		AssertValid();

		FireLayOutGraphCompleted(e);

		m_oSynchronizationContext = null;
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

		Debug.Assert(m_oBackgroundWorker != null);
		// m_oSynchronizationContext
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// BackgroundWorker used by LayOutGraphAsync().

	protected BackgroundWorker m_oBackgroundWorker;

	/// SynchronizationContext of the thread that called LayOutGraphAsync(), or
	/// null if LayOutGraphAsync() hasn't been called.

	protected SynchronizationContext m_oSynchronizationContext;
}

}
