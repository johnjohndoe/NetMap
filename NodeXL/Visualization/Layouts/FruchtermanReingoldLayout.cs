
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Collections;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

// TMathType is the type used by this class for most arithmetic operations.
// This layout has been tested using the type System.Single.
//
// In the Fruchterman-Reingold paper referenced below, the authors state, "We
// used integer arithmetic for speed; this often required that expressions be
// carefully crafted to preserve significance, but it proved worth while."
//
// If the type used here is switched to Int32, the algorithm doesn't behave
// properly, probably because significance is lost in critical places.  A
// possible to-do item is to switch to Int32 and find and fix those critical
// places, although it's doubtful that this would significantly speed up the
// calculations.

using TMathType = System.Single;


namespace Microsoft.NodeXL.Visualization
{
//*****************************************************************************
//  Class: FruchtermanReingoldLayout
//
/// <summary>
///	Lays out a graph using the Fruchterman-Reingold layout.
/// </summary>
///
/// <remarks>
///	For details on the layout algorithm, see
/// http://www.cs.ubc.ca/rr/proceedings/spe91-95/spe/vol21/issue11/spe060tf.pdf.
///
/// <para>
/// If the graph has a metadata key of <see
/// cref="ReservedMetadataKeys.LayOutTheseVerticesOnly" />, only the vertices
/// specified in the value's IVertex array are laid out and all other vertices
/// are completely ignored.
/// </para>
///
/// <para>
/// By default, the layout is initialized by setting the vertices to random
/// locations.  If the graph has a metadata key of <see
/// cref="ReservedMetadataKeys.FruchtermanReingoldLayoutSelectivelyRandomize"
/// />, however, only those vertices whose <see cref="IVertex.Location" /> is
/// set to  <see cref="LayoutBase.RandomizeThisLocation" /> are randomized.
/// </para>
///
/// <para>
/// If a vertex has a metadata key of <see
/// cref="ReservedMetadataKeys.LockVertexLocation" /> with a value of true, it
/// is included in layout calculations but its own location is left unmodified.
/// </para>
///
/// <para>
/// <see cref="VertexDrawer" /> and <see cref="EdgeDrawer" /> can be used to
/// draw the graph after it is laid out.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class FruchtermanReingoldLayout : AsyncLayoutBase
{
    //*************************************************************************
    //  Constructor: FruchtermanReingoldLayout()
    //
    /// <summary>
    /// Initializes a new instance of the FruchtermanReingoldLayout class.
    /// </summary>
    //*************************************************************************

    public FruchtermanReingoldLayout()
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
		ICollection oEdgesToLayOut = GetEdgesToLayOut(graph);

		Int32 iVertices = oVerticesToLayOut.Count;

		// Although the caller has guaranteed that there is at least one vertex
		// in the graph, the collection returned by GetVerticesToLayOut() may
		// be empty.

		if (iVertices == 0)
		{
			return (true);
		}

		// If the graph has already been laid out, use the current vertex
		// locations as initial values.

		if ( !graph.ContainsKey(
			ReservedMetadataKeys.LayoutBaseLayoutComplete) )
		{
			// The graph has not been laid out.  By default, randomize the
			// locations of those vertices that are not locked.  If the graph
			// has the FruchtermanReingoldLayoutSelectivelyRandomize key,
			// however, randomize the locations of those vertices that have
			// IVertex.Location set to LayoutBase.RandomizeThisLocation and are
			// not locked.

			Boolean bSelectivelyRandomize = graph.ContainsKey(
				ReservedMetadataKeys.
					FruchtermanReingoldLayoutSelectivelyRandomize);

			RandomizeVertexLocations(oVerticesToLayOut, layoutContext,
				new Random(1), bSelectivelyRandomize);
		}

		// Store required metadata on the graph's vertices.

		InitializeMetadata(oVerticesToLayOut);

		Rectangle oRectangle = layoutContext.GraphRectangle;

		Single fArea = oRectangle.Width * oRectangle.Height;

		Debug.Assert(iVertices > 0);

		Single k = (Single)Math.Sqrt(fArea / (Single)iVertices);

		// The rectangle is guaranteed to have non-zero width and height, so
		// k should never be zero.

		Debug.Assert(k != 0);

		// Use the simple cooling algorithm suggested in the Fruchterman-
		// Reingold paper.

		Single fTemperature = oRectangle.Width / 10F;

		Debug.Assert(Iterations != 0);

		Single fTemperatureDecrement = fTemperature / (Single)Iterations;

		while (fTemperature > 0)
		{
			if (backgroundWorker != null &&
				backgroundWorker.CancellationPending)
			{
				return (false);
			}

			// Calculate the attractive and repulsive forces between the
			// vertices.  The results get written to metadata on the vertices.

			CalculateRepulsiveForces(oVerticesToLayOut, k);
			CalculateAttractiveForces(oEdgesToLayOut, k);

			// Set the location of each vertex based on the vertex's current
			// location and the calculated forces.

			SetVertexLocations(oVerticesToLayOut, layoutContext, fTemperature);

			// Decrease the temperature.

			fTemperature -= fTemperatureDecrement;

			if (backgroundWorker != null)
			{
				FireLayOutGraphIterationCompleted();
			}
		}

		RemoveMetadata(oVerticesToLayOut);

		return (true);
	}

    //*************************************************************************
    //  Method: InitializeMetadata()
    //
    /// <summary>
    /// Stores required metadata on the graph's vertices before the layout
	/// begins.
    /// </summary>
    ///
    /// <param name="verticesToLayOut">
    /// Vertices to lay out.  The collection is guaranteed to have at least one
	/// vertex.
    /// </param>
    //*************************************************************************

	protected void
	InitializeMetadata
	(
		ICollection verticesToLayOut
	)
	{
		Debug.Assert(verticesToLayOut != null);
		AssertValid();

		foreach (IVertex oVertex in verticesToLayOut)
		{
			// Create an object that will store all calculated values for the
			// vertex.

			FruchtermanReingoldVertexInfo oFruchtermanReingoldVertexInfo =
				new FruchtermanReingoldVertexInfo(oVertex.Location);

			// The object could be stored in a metadata key, but because the
			// number of retrievals can be very large, it's more efficient to
			// store it in the Tag.  If a Tag already exists, save it in a
			// metadata key.

			Object oTag = oVertex.Tag;

			if (oTag != null)
			{
				oVertex.SetValue(
					ReservedMetadataKeys.FruchtermanReingoldLayoutTagStorage,
					oTag
					);
			}

			oVertex.Tag = oFruchtermanReingoldVertexInfo;
		}
	}

    //*************************************************************************
    //  Method: RemoveMetadata()
    //
    /// <summary>
	/// Removes metadata from the graph's vertices after the layout is
	/// complete.
    /// </summary>
    ///
    /// <param name="verticesToLayOut">
    /// Vertices to lay out.  The collection is guaranteed to have at least one
	/// vertex.
    /// </param>
    //*************************************************************************

	protected void
	RemoveMetadata
	(
		ICollection verticesToLayOut
	)
	{
		Debug.Assert(verticesToLayOut != null);
		AssertValid();

		foreach (IVertex oVertex in verticesToLayOut)
		{
			// If the vertex had a Tag before InitializeMetadata() was called,
			// restore it.

			Object oTag = oVertex.GetValue(
				ReservedMetadataKeys.FruchtermanReingoldLayoutTagStorage);

			oVertex.Tag = oTag;

			if (oTag != null)
			{
				oVertex.RemoveKey(
					ReservedMetadataKeys.FruchtermanReingoldLayoutTagStorage);
			}
		}
	}

    //*************************************************************************
    //  Method: CalculateRepulsiveForces()
    //
    /// <summary>
    /// Calculates the repulsive forces between the vertices.
    /// </summary>
    ///
    /// <param name="verticesToLayOut">
    /// Vertices to lay out.  The collection is guaranteed to have at least one
	/// vertex.
    /// </param>
	///
    /// <param name="k">
    /// The "k" constant in the Fruchterman-Reingold algorithm.
    /// </param>
	///
    /// <remarks>
	/// The results are stored in the FruchtermanReingoldVertexInfo object
	/// stored in each vertex's Tag.
    /// </remarks>
    //*************************************************************************

	protected void
	CalculateRepulsiveForces
	(
		ICollection verticesToLayOut,
		Single k
	)
	{
		Debug.Assert(verticesToLayOut != null);
		AssertValid();

		TMathType tkSquared = (TMathType)(k * k);

		foreach (IVertex oVertexV in verticesToLayOut)
		{
			// Retrieve the object that stores calculated values for the
			// vertex.

			FruchtermanReingoldVertexInfo oVertexInfoV =
				(FruchtermanReingoldVertexInfo)oVertexV.Tag;

			TMathType tDisplacementX = 0;
			TMathType tDisplacementY = 0;

			foreach (IVertex oVertexU in verticesToLayOut)
			{
				if (oVertexU == oVertexV)
				{
					continue;
				}

				FruchtermanReingoldVertexInfo oVertexInfoU =
					(FruchtermanReingoldVertexInfo)oVertexU.Tag;

				TMathType tDeltaX =
					(TMathType)oVertexInfoV.UnboundedLocationX -
					(TMathType)oVertexInfoU.UnboundedLocationX;

				TMathType tDeltaY =
					(TMathType)oVertexInfoV.UnboundedLocationY -
					(TMathType)oVertexInfoU.UnboundedLocationY;

				TMathType tDelta = (TMathType)Math.Sqrt(
					(tDeltaX * tDeltaX) + (tDeltaY * tDeltaY)
					);

				// The Fruchterman-Reingold paper says this about vertices in
				// the same location:
				//
				// "A special case occurs when vertices are in the same
				// position: our implementation acts as though the two vertices
				// are a small distance apart in a randomly chosen orientation:
				// this leads to a violent repulsive effect separating them."
				//
				// Handle this case by arbitrarily setting a small
				// displacement.

				if (tDelta == 0)
				{
					tDisplacementX += 1;
					tDisplacementY += 1;
				}
				else
				{
					Debug.Assert(tDelta != 0);

					TMathType fr = tkSquared / tDelta;

					TMathType frOverDelta = fr / tDelta;

					tDisplacementX += tDeltaX * frOverDelta;
					tDisplacementY += tDeltaY * frOverDelta;
				}
			}

			// Save the results for VertexV.

			oVertexInfoV.DisplacementX = tDisplacementX;
			oVertexInfoV.DisplacementY = tDisplacementY;
		}
	}

    //*************************************************************************
    //  Method: CalculateAttractiveForces()
    //
    /// <summary>
    /// Calculates the attractive forces between the vertices.
    /// </summary>
    ///
    /// <param name="edgesToLayOut">
    /// Edges to lay out.
    /// </param>
	///
    /// <param name="k">
    /// The "k" constant in the Fruchterman-Reingold algorithm.
    /// </param>
	///
    /// <remarks>
	/// The results are added to the existing FruchtermanReingoldVertexInfo
	/// object stored in each vertex's Tag.
    /// </remarks>
    //*************************************************************************

	protected void
	CalculateAttractiveForces
	(
		ICollection edgesToLayOut,
		Single k
	)
	{
		Debug.Assert(edgesToLayOut != null);
		Debug.Assert(k != 0);
		AssertValid();

		const String MethodName = "CalculateAttractiveForces";

		foreach (IEdge oEdge in edgesToLayOut)
		{
			if (oEdge.IsSelfLoop)
			{
				// A vertex isn't attracted to itself.

				continue;
			}

			// Get the edge's vertices.

			IVertex oVertexV, oVertexU;

			EdgeUtil.EdgeToVertices(oEdge, this.ClassName, MethodName,
				out oVertexV, out oVertexU);

			// Retrieve the objects that store calculated values for the
			// vertices.

			FruchtermanReingoldVertexInfo oVertexInfoV =
				(FruchtermanReingoldVertexInfo)oVertexV.Tag;

			FruchtermanReingoldVertexInfo oVertexInfoU =
				(FruchtermanReingoldVertexInfo)oVertexU.Tag;

            TMathType tDeltaX =
                (TMathType)oVertexInfoV.UnboundedLocationX -
				(TMathType)oVertexInfoU.UnboundedLocationX;

			TMathType tDeltaY =
				(TMathType)oVertexInfoV.UnboundedLocationY -
				(TMathType)oVertexInfoU.UnboundedLocationY;

			TMathType tDelta = (TMathType)Math.Sqrt(
				(tDeltaX * tDeltaX) + (tDeltaY * tDeltaY)
				);

			TMathType tDisplacementV_X = (TMathType)oVertexInfoV.DisplacementX;
			TMathType tDisplacementV_Y = (TMathType)oVertexInfoV.DisplacementY;

			TMathType tDisplacementU_X = (TMathType)oVertexInfoU.DisplacementX;
			TMathType tDisplacementU_Y = (TMathType)oVertexInfoU.DisplacementY;

			// (Note that there is an obvious typo in the Fruchterman-Reingold
			// paper for computing the attractive force.  The function fa(z) at
			// the top of Figure 1 is defined as x squared over k.  It should
			// read z squared over k.)

			TMathType fa = (tDelta * tDelta) / (TMathType)k;

			if (tDelta == 0)
			{
				// TODO: Is this the correct way to handle vertices in the same
				// location?  See the notes in CalculateRepulsiveForces().

				continue;
			}

			Debug.Assert(tDelta != 0);

			TMathType faOverDelta = fa / tDelta;

			TMathType tFactorX = tDeltaX * faOverDelta;
			TMathType tFactorY = tDeltaY * faOverDelta;

			tDisplacementV_X -= tFactorX;
			tDisplacementV_Y -= tFactorY;

			tDisplacementU_X += tFactorX;
			tDisplacementU_Y += tFactorY;

			oVertexInfoV.DisplacementX = (Single)tDisplacementV_X;
			oVertexInfoV.DisplacementY = (Single)tDisplacementV_Y;

			oVertexInfoU.DisplacementX = (Single)tDisplacementU_X;
			oVertexInfoU.DisplacementY = (Single)tDisplacementU_Y;
		}
	}

    //*************************************************************************
    //  Method: SetVertexLocations()
    //
    /// <summary>
    /// Sets the location of each vertex.
    /// </summary>
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
    /// <param name="fTemperature">
	/// Current temperature.  Must be greater than zero.
    /// </param>
	///
	/// <remarks>
	/// This method is called at the end of each layout iteration.  For each
	/// vertex, it modifies the vertex's location within an unbounded rectangle
	/// based on the repulsive and attractive forces that have been calculated
	/// for the vertex, transforms that unbounded location to a point within
	/// the bounded graph rectangle, and sets the vertex's <see
	/// cref="IVertex.Location" /> property to that bounded point.
	/// </remarks>
    //*************************************************************************

    protected void
    SetVertexLocations
    (
		ICollection verticesToLayOut,
		LayoutContext layoutContext,
		Single fTemperature
	)
	{
		Debug.Assert(verticesToLayOut != null);
		Debug.Assert(layoutContext != null);
		Debug.Assert(fTemperature > 0);
		AssertValid();

		// The following variables define the unbounded rectangle.

		TMathType tMinLocationX = Single.MaxValue;
		TMathType tMaxLocationX = Single.MinValue;

		TMathType tMinLocationY = Single.MaxValue;
		TMathType tMaxLocationY = Single.MinValue;

		foreach (IVertex oVertex in verticesToLayOut)
		{
			// Retrieve the object that stores calculated values for the
			// vertex.  We need the vertex's current unbounded location and
			// the displacement created by the repulsive and attractive forces
			// on the vertex.

			FruchtermanReingoldVertexInfo oVertexInfo =
				(FruchtermanReingoldVertexInfo)oVertex.Tag;

			TMathType tUnboundedLocationX =
				(TMathType)oVertexInfo.UnboundedLocationX;

			TMathType tUnboundedLocationY =
				(TMathType)oVertexInfo.UnboundedLocationY;

			TMathType tDisplacementX = (TMathType)oVertexInfo.DisplacementX;
			TMathType tDisplacementY = (TMathType)oVertexInfo.DisplacementY;

			TMathType tDisplacement = (TMathType)Math.Sqrt(
				(tDisplacementX * tDisplacementX) +
				(tDisplacementY * tDisplacementY)
				);

			if (tDisplacement != 0)
			{
				// Calculate a new unbounded location, limited by the current
				// temperature.

				// (Note that there is a typo in the Fruchterman-Reingold paper
				// for limiting the maximum displacement.  Following the text
				// in the paper, the first argument to the Math.Min() method
				// below would be tDisplacementX.  It should be tDisplacement.)

				tUnboundedLocationX += (tDisplacementX / tDisplacement) *
					Math.Min(tDisplacement, (TMathType)fTemperature);

				tUnboundedLocationY += (tDisplacementY / tDisplacement) *
					Math.Min(tDisplacement, (TMathType)fTemperature);
			}

			// Update the vertex's unbounded location.

			oVertexInfo.UnboundedLocationX = (Single)tUnboundedLocationX;
			oVertexInfo.UnboundedLocationY = (Single)tUnboundedLocationY;

			// Expand the unbounded rectangle if necessary.

			tMinLocationX = Math.Min(tUnboundedLocationX, tMinLocationX);
			tMaxLocationX = Math.Max(tUnboundedLocationX, tMaxLocationX);

			tMinLocationY = Math.Min(tUnboundedLocationY, tMinLocationY);
			tMaxLocationY = Math.Max(tUnboundedLocationY, tMaxLocationY);
		}

		Debug.Assert(verticesToLayOut.Count != 0);

		Debug.Assert(tMinLocationX != Single.MaxValue);
		Debug.Assert(tMaxLocationX != Single.MinValue);
		Debug.Assert(tMinLocationY != Single.MaxValue);
		Debug.Assert(tMaxLocationY != Single.MinValue);

		// Get a see Matrix that will transform vertex locations from
		// coordinates in the unbounded rectangle to cooordinates in the
		// bounded graph rectangle.

		Matrix oTransformationMatrix = LayoutUtil.GetRectangleTransformation(

			RectangleF.FromLTRB(
				(Single)tMinLocationX,
				(Single)tMinLocationY,
				(Single)tMaxLocationX,
				(Single)tMaxLocationY
				),
			
			layoutContext.GraphRectangle
			);

		// Transform the vertex locations.

		foreach (IVertex oVertex in verticesToLayOut)
		{
			FruchtermanReingoldVertexInfo oVertexInfo =
				(FruchtermanReingoldVertexInfo)oVertex.Tag;

			PointF [] aoLocation = new PointF [] {
				new PointF(
					oVertexInfo.UnboundedLocationX,
					oVertexInfo.UnboundedLocationY
					)
				};

			oTransformationMatrix.TransformPoints(aoLocation);

			if ( !VertexIsLocked(oVertex) )
			{
				oVertex.Location = aoLocation[0];
			}
		}
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
    //  Protected constants
    //*************************************************************************

	/// Number of iterations to run the algorithm.

	protected const Int32 Iterations = 10;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
