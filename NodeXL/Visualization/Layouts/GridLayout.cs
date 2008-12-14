
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
//  Class: GridLayout
//
/// <summary>
///	Lays out a graph by placing the vertices on a grid.
/// </summary>
///
/// <remarks>
/// This layout places a graph's vertices on a simple grid.
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

public class GridLayout : SortableLayoutBase
{
    //*************************************************************************
    //  Constructor: GridLayout()
    //
    /// <summary>
    /// Initializes a new instance of the GridLayout class.
    /// </summary>
    //*************************************************************************

    public GridLayout()
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
		Debug.Assert(verticesToLayOut.Count > 0);
		AssertValid();

		RectangleF oRectangleF = layoutContext.GraphRectangle;

		Debug.Assert(oRectangleF.Width > 0 && oRectangleF.Height > 0);

		// The layout is animated the first time the graph is drawn by
		// uniformly increasing the size of the grid with each iteration.  The
		// grid's upper-left corner is always the upper-left corner of the
		// specified rectangle, and the grid's aspect ratio is always equal to
		// the aspect ratio of the rectangle.

		Int32 iIterations = AnimationIterations;

		if ( graph.ContainsKey(ReservedMetadataKeys.GridLayoutGridDrawn) )
		{
			// The graph has been completely drawn before.  Don't animate it
			// this time.

			iIterations = 1;
		}

		// Get the number of rows and columns to use in the grid.

		Int32 iRows, iColumns;

		GetRowsAndColumns(verticesToLayOut, layoutContext,
			out iRows, out iColumns);

		Debug.Assert(iRows > 0);
		Debug.Assert(iColumns > 0);

		for (Int32 i = 0; i < iIterations; i++)
		{
			if (backgroundWorker != null &&
				backgroundWorker.CancellationPending)
			{
				return (false);
			}

			// Calculate the rectangle to use for this iteration.  Don't let
			// the rectangle have a zero dimension, which would cause various
			// problems with the calculations.

			Double dIterationFactor =
				( (Double)i + 1 ) / (Double)iIterations;

			RectangleF oIterationRectangleF = new RectangleF(
				oRectangleF.X,
				oRectangleF.Y,
				Math.Max( (Single)(oRectangleF.Width * dIterationFactor), 1 ),
				Math.Max( (Single)(oRectangleF.Height * dIterationFactor), 1 )
				);

			// Get the distances between vertices;

			Double dRowSpacing =
				(Double)oIterationRectangleF.Height / (Double)iRows;

			Double dColumnSpacing =
				(Double)oIterationRectangleF.Width / (Double)iColumns;

			// Set the location on each vertex.  The vertices are placed at the
			// intersections of the grid lines.

			Double dX = oIterationRectangleF.Left + dColumnSpacing;
			Double dY = oIterationRectangleF.Top + dRowSpacing;

			Int32 iColumn = 0;

			foreach (IVertex oVertex in verticesToLayOut)
			{
				if ( !VertexIsLocked(oVertex) )
				{
					oVertex.Location = new PointF( (Single)dX,  (Single)dY );
				}

				iColumn++;

				if (iColumn >= iColumns - 1)
				{
					dX = oIterationRectangleF.Left + dColumnSpacing;

					dY += dRowSpacing;

					iColumn = 0;
				}
				else
				{
					dX += dColumnSpacing;
				}
			}

			System.Threading.Thread.Sleep(AnimationSleepMs);

			if (backgroundWorker != null)
			{
				FireLayOutGraphIterationCompleted();
			}
		}

		// Mark the graph as having been completely drawn.

		graph.SetValue(ReservedMetadataKeys.GridLayoutGridDrawn, null);

		return (true);
	}

    //*************************************************************************
    //  Method: GetRowsAndColumns()
    //
    /// <summary>
	/// Gets the number of rows and columns to use in the grid.
    /// </summary>
    ///
    /// <param name="oVerticesToLayOut">
    /// Vertices to lay out.
    /// </param>
    ///
    /// <param name="oLayoutContext">
    /// Provides access to objects needed to lay out the graph.
    /// </param>
    ///
    /// <param name="iRows">
    /// Where the number of grid rows gets stored.  Because the first row of
	/// vertices is drawn on the first horizontal grid line and not on the top
	/// edge of the rectangle, the number of vertices to draw per column is
	/// <paramref name="iRows" /> minus one.
    /// </param>
    ///
    /// <param name="iColumns">
    /// Where the number of grid columns gets stored.  Because the first column
	/// of vertices is drawn on the first vertical grid line and not on the
	/// left edge of the rectangle, the number of vertices to draw per row is
	/// <paramref name="iColumns" /> minus one.
    /// </param>
    //*************************************************************************

    protected void
    GetRowsAndColumns
    (
		ICollection oVerticesToLayOut,
		LayoutContext oLayoutContext,
		out Int32 iRows,
		out Int32 iColumns
	)
	{
		Debug.Assert(oVerticesToLayOut != null);
		Debug.Assert(oLayoutContext != null);
		AssertValid();

        #if false

        Some definitions:

            W = rectangle width

            H = rectangle height

            A = rectangle aspect ratio = W / H

            V = number of vertices in graph

            Vrow = number of vertices per row

            Vcol = number of vertices per column

            R = number of grid rows = Vcol + 1

            C = number of grid columns = Vrow + 1


        First simulataneous equation, allowing Vrow and Vcol to be fractional
		for now:

            Vrow * Vcol = V


        Second simulataneous equation:

            C / R = A


        Combining these equations yield this quadratic equation:

             2
            C  + [ (-A - 1) * C ] + [ A * (1 - V) ] = 0

        which is of the form:

              2
            ax  + bx + c = 0

		where

			a = 1

			b = (-A - 1)

			c = A * (1 - V)


		The following code uses the standard quadratic equation solution to
		find C.

        #endif

		Int32 V = oVerticesToLayOut.Count;

		// Compute the aspect ratio.

		RectangleF oRectangleF = oLayoutContext.GraphRectangle;

		Debug.Assert(oRectangleF.Height != 0);

		Double A = oRectangleF.Width / oRectangleF.Height;

		// Compute a, b, and c for the quadratic equation derived above.

		Double a = 1;

		Double b = (-A - 1);

		Double c = A * (1 - V);

		// Solve the quadratic to get the number of columns.

		Double C1, C2;

		SolveQuadratic(a, b, c, out C1, out C2);

		if (C1 > 0)
		{
			iColumns = (Int32)Math.Ceiling(C1);
		}
		else
		{
			Debug.Assert(C2 > 0);

			iColumns = (Int32)Math.Ceiling(C2);
		}

		Debug.Assert(iColumns != 0);

		// Compute the number of rows, trying the floor first.

		Double dRows = (double)iColumns / A;

		iRows = (Int32)Math.Floor(dRows);

		if ( (iColumns - 1) * (iRows - 1) < V )
		{
			// Use the ceiling instead.

			iRows++;
		}

		Debug.Assert( (iColumns - 1) * (iRows - 1) >= V );
	}

    //*************************************************************************
    //  Method: SolveQuadratic()
    //
    /// <summary>
	/// Solves a quadratic equation.
    /// </summary>
    ///
    /// <param name="a">
	/// See remarks.  Can't be zero.
    /// </param>
    ///
    /// <param name="b">
	/// See remarks.
    /// </param>
    ///
    /// <param name="c">
	/// See remarks.
    /// </param>
    ///
    /// <param name="x1">
    /// Where the first solution gets stored.
    /// </param>
    ///
    /// <param name="x2">
    /// Where the second solution gets stored.
    /// </param>
    ///
    /// <remarks>
	/// This method solves this quadratic formula:
	///
	/// <para>
    ///   2
    /// ax  + bx + c = 0
	/// </para>
	///
	/// <para>
	/// It stores the two values of x at <paramref name="x1" /> and <paramref
	/// name="x2" />.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

	protected void
	SolveQuadratic
	(
		Double a,
		Double b,
		Double c,
		out Double x1,
		out Double x2
	)
	{
		Debug.Assert(a != 0);
		AssertValid();

		Double dSquare = (b * b) - (4 * a * c);

		Debug.Assert(dSquare >= 0);

		Double dSquareRoot = Math.Sqrt(dSquare);

		Double d2a = 2 * a;

		x1 = (-b + dSquareRoot) / d2a;
		x2 = (-b - dSquareRoot) / d2a;
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
