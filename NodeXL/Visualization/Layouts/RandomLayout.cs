
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Visualization
{
//*****************************************************************************
//  Class: RandomLayout
//
/// <summary>
///	Lays out a graph by placing the vertices in random locations.
/// </summary>
///
/// <para>
/// If the graph has a metadata key of <see
/// cref="ReservedMetadataKeys.LayOutTheseVerticesOnly" />, only the vertices
/// specified in the value's IVertex array are laid out and all other vertices
/// are completely ignored.
/// </para>
///
/// <remarks>
/// If a vertex has a metadata key of <see
/// cref="ReservedMetadataKeys.LockVertexLocation" /> with a value of true, it
/// is included in layout calculations but its own location is left unmodified.
///
/// <para>
/// <see cref="VertexDrawer" /> and <see cref="EdgeDrawer" /> can be used to
/// draw the graph after it is laid out.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class RandomLayout : AsyncLayoutBase
{
    //*************************************************************************
    //  Constructor: RandomLayout()
    //
    /// <summary>
    /// Initializes a new instance of the RandomLayout class.
    /// </summary>
    //*************************************************************************

    public RandomLayout()
    {
		// (Do nothing.)

		AssertValid();
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

		// Honor the optional LayOutTheseVerticesOnly key on the graph.

		ICollection oVerticesToLayOut = GetVerticesToLayOut(graph);

		// Although the caller has guaranteed that there is at least one vertex
		// in the graph, the collection returned by GetVerticesToLayOut() may
		// be empty.

		if (oVerticesToLayOut.Count == 0)
		{
			return (true);
		}

		for (Int32 i = 0; i < AnimationIterations; i++)
		{
			if (backgroundWorker != null &&
				backgroundWorker.CancellationPending)
			{
				return (false);
			}

			base.RandomizeVertexLocations(oVerticesToLayOut, layoutContext,
				new Random(), false);

			System.Threading.Thread.Sleep(AnimationSleepMs);

			if (backgroundWorker != null)
			{
				FireLayOutGraphIterationCompleted();
			}
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
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	/// Number of iterations used to animate the graph.

	protected const Int32 AnimationIterations = 5;

	/// Number of milliseconds to sleep between animation iterations.

	protected const Int32 AnimationSleepMs = 10;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
