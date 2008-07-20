
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.Visualization
{
//*****************************************************************************
//  Class: KamadaKawaiiLayout
//
/// <summary>
///	Lays out a graph using the Kamada-Kawaii layout.
/// </summary>
///
/// <remarks>
///	For details on the layout algorithm, see Kamada, T., and Kawai, S., "A
///	General Framework for Visualizing Abstract Objects and Relations", ACM
///	Transactions on Graphics (TOG) 10, 1 (1991), 1--39.
/// </remarks>
//*****************************************************************************

public class KamadaKawaiiLayout : AsyncLayoutBase
{
    //*************************************************************************
    //  Constructor: KamadaKawaiiLayout()
    //
    /// <summary>
    /// Initializes a new instance of the KamadaKawaiiLayout class.
    /// </summary>
    //*************************************************************************

    public KamadaKawaiiLayout()
    {
        // TODO
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

    protected override Boolean
    LayOutGraphCore
    (
		IGraph graph,
		LayoutContext layoutContext,
		BackgroundWorker backgroundWorker
	)
	{
		Debug.Assert(graph != null);
		Debug.Assert(layoutContext != null);
		AssertValid();

		// TODO: Simulate a long layout.

		const Int32 Iterations = 5;

		for (Int32 i = 0; i < Iterations; i++)
		{
			if (backgroundWorker != null &&
				backgroundWorker.CancellationPending)
			{
				return (false);
			}

            #if false
			if (i == 5)
			{
				throw new ApplicationException("TODO: Testing exceptions.");
			}
            #endif

			RandomizeVertexLocations(graph, layoutContext, 1);

			/*
			Trace.WriteLine(DateTime.Now.ToLongTimeString() + ": "
				+ "BackgroundWorker_DoWork: About to sleep."
				);
			*/

			System.Threading.Thread.Sleep(100);

			/*
			Trace.WriteLine(DateTime.Now.ToLongTimeString() + ": "
				+ "BackgroundWorker_DoWork: Awoke from sleep."
				);

			Trace.WriteLine(DateTime.Now.ToLongTimeString() + ": "
				+ "BackgroundWorker_DoWork: About to fire event."
				);
			*/

			if (backgroundWorker != null)
			{
				FireLayOutGraphIterationCompleted();
			}

			/*
			Trace.WriteLine(DateTime.Now.ToLongTimeString() + ": "
				+ "BackgroundWorker_DoWork: Returned from event."
				);
			*/
		}

		return (true);
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

        // TODO
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // TODO
}

}
