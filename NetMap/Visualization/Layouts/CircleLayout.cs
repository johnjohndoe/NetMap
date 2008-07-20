
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.Visualization
{
//*****************************************************************************
//  Class: CircleLayout
//
/// <summary>
///	Lays out a graph by placing the vertices on a circle.
/// </summary>
///
/// <remarks>
/// This layout places a graph's vertices on a circle whose diameter is the
/// smaller of the rectangle's dimensions.
///
/// <para>
/// If the graph has a metadata key of <see
/// cref="ReservedMetadataKeys.LayOutTheseVerticesOnly" />, only the vertices
/// specified in the value's IVertex array are laid out and all other vertices
/// are completely ignored.
/// </para>
///
/// <para>
/// If a vertex has a metadata key of <see
/// cref="ReservedMetadataKeys.LockVertexLocation" /> with a value of true, it
/// is included in layout calculations but its own location is left unmodified.
/// </para>
///
/// <para>
/// If you want the vertices to be placed in a certain order, set the <see
/// cref="SortableLayoutBase.VertexSorter" /> property to an object that will
/// sort them.
/// </para>
///
/// <para>
/// <see cref="VertexDrawer" /> and <see cref="EdgeDrawer" /> can be used to
/// draw the graph after it is laid out.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class CircleLayout : SortableLayoutBase
{
    //*************************************************************************
    //  Constructor: CircleLayout()
    //
    /// <summary>
    /// Initializes a new instance of the CircleLayout class.
    /// </summary>
    //*************************************************************************

    public CircleLayout()
    {
		// (Do nothing.)

		AssertValid();
    }

    //*************************************************************************
    //  Method: LayOutGraphCore()
    //
    /// <summary>
    /// Lays out a graph synchronously or asynchronously using specified
	/// vertices that may be sorted.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to lay out.
    /// </param>
    ///
    /// <param name="verticesToLayOut">
    /// Vertices to lay out.  The collection is guaranteed to have at least one
	/// vertex.
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
		ICollection verticesToLayOut,
		BackgroundWorker backgroundWorker
	)
	{
		Debug.Assert(graph != null);
		Debug.Assert(layoutContext != null);
		Debug.Assert(verticesToLayOut != null);
		AssertValid();

		Int32 iVertices = verticesToLayOut.Count;

		Debug.Assert(iVertices > 0);

		// The vertices are placed at equal angles around the circle's
		// circumference.

		Double dAngleBetweenVertices = (2 * Math.PI) / (Double)iVertices;

		// The layout is animated the first time the graph is drawn by
		// uniformly increasing the circle's radius with each iteration.

		Int32 iIterations = AnimationIterations;

		if ( graph.ContainsKey(ReservedMetadataKeys.CircleLayoutCircleDrawn) )
		{
			// The graph has been completely drawn before.  Don't animate it
			// this time.

			iIterations = 1;
		}

		Double dCenterX, dCenterY, dHalfSize;

		GetRectangleCenterAndHalfSize(layoutContext.GraphRectangle,
			out dCenterX, out dCenterY, out dHalfSize);

		for (Int32 i = 0; i < iIterations; i++)
		{
			if (backgroundWorker != null &&
				backgroundWorker.CancellationPending)
			{
				return (false);
			}

			Double dRadius =
				dHalfSize * ( (Double)i + 1) / (Double)iIterations;

			Double dAngle = 0;

			// Set the location on each vertex.

			foreach (IVertex oVertex in verticesToLayOut)
			{
				if ( !VertexIsLocked(oVertex) )
				{
					Double dX = dCenterX + dRadius * Math.Cos(dAngle);
					Double dY = dCenterY + dRadius * Math.Sin(dAngle);

					oVertex.Location = new PointF( (Single)dX,  (Single)dY );
				}

				dAngle += dAngleBetweenVertices;
			}

			System.Threading.Thread.Sleep(AnimationSleepMs);

			if (backgroundWorker != null)
			{
				FireLayOutGraphIterationCompleted();
			}
		}

		// Mark the graph as having been completely drawn.

		graph.SetValue(ReservedMetadataKeys.CircleLayoutCircleDrawn, null);

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

		// (Do nothing.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	/// Number of iterations used to animate the graph the first time it is
	/// drawn.

	protected const Int32 AnimationIterations = 10;

	/// Number of milliseconds to sleep between animation iterations.

	protected const Int32 AnimationSleepMs = 10;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
