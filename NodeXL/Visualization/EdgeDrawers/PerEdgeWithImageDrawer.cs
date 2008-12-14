
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Visualization
{
//*****************************************************************************
//  Class: PerEdgeWithImageDrawer
//
/// <summary>
///	Draws an edge as a simple line.  Allows per-edge customizations, and
/// accommodates vertices drawn as images.
/// </summary>
///
/// <remarks>
/// By default, this class delegates edge drawing to its base class.  However,
/// if an edge's second vertex is drawn as an image by <see
/// cref="PerVertexWithImageDrawer" />, the edge's second endpoint is adjusted
/// to touch the edge of the image.  Self-loop edges are adjusted as well.
/// </remarks>
///
/// <seealso cref="PerEdgeDrawer" />
//*****************************************************************************

public class PerEdgeWithImageDrawer : PerEdgeDrawer
{
    //*************************************************************************
    //  Constructor: PerEdgeWithImageDrawer()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="PerEdgeWithImageDrawer" /> class.
    /// </summary>
    //*************************************************************************

    public PerEdgeWithImageDrawer()
    {
		// (Do nothing.)

		AssertValid();
    }

    //*************************************************************************
    //  Method: GetEdgeEndpoints()
    //
    /// <summary>
    /// Gets the endpoints of the edge.
    /// </summary>
    ///
    /// <param name="vertex1">
	/// The edge's first vertex.
    /// </param>
	///
    /// <param name="vertex2">
	/// The edge's second vertex.
    /// </param>
	///
    /// <param name="drawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
	///
    /// <param name="edgeEndpoint1">
	/// Where the edge's first endpoint gets stored.
    /// </param>
	///
    /// <param name="edgeEndpoint2">
	/// Where the edge's second endpoint gets stored.
    /// </param>
	///
    /// <remarks>
	/// The edge's first endpoint is the endpoint on the <paramref
	/// name="vertex1" /> side of the edge.  The edge's second endpoint is the
	/// endpoint on the <paramref name="vertex2" /> side of the edge.
    /// </remarks>
    //*************************************************************************

	protected override void
	GetEdgeEndpoints
	(
		IVertex vertex1,
		IVertex vertex2,
		DrawContext drawContext,
		out PointF edgeEndpoint1,
		out PointF edgeEndpoint2
	)
	{
		Debug.Assert(vertex1 != null);
		Debug.Assert(vertex2 != null);
		Debug.Assert(drawContext != null);
		AssertValid();

		// Check whether the second vertex is drawn as an image.

        Image oImage2;

		if ( !PerVertexWithImageDrawer.TryGetImage(vertex2, out oImage2) )
		{
			// No.  Defer to the base class.

			base.GetEdgeEndpoints(vertex1, vertex2, drawContext,
				out edgeEndpoint1, out edgeEndpoint2);

			return;
		}

		// Get the first endpoint from the base class.

		edgeEndpoint1 = base.GetEdgeEndpoint1(vertex1, vertex2, drawContext);

		// Get a point on the image rectangle that is near the first endpoint.

		Rectangle oImageRectangle2 =
			PerVertexWithImageDrawer.GetImageRectangle(vertex2, oImage2);

		edgeEndpoint2 = GetNearestPointOnVertexRectangle(edgeEndpoint1,
			oImageRectangle2);
	}

    //*************************************************************************
    //  Method: DrawSelfLoop()
    //
    /// <summary>
    /// Draws an edge that is a self-loop.
    /// </summary>
    ///
    /// <param name="pen">
	/// Pen to use.
    /// </param>
	///
    /// <param name="vertex">
	/// Vertex that the edge connects to itself.
    /// </param>
	///
    /// <param name="drawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
    //*************************************************************************

	protected override void
	DrawSelfLoop
	(
		Pen pen,
		IVertex vertex,
		DrawContext drawContext
	)
	{
		Debug.Assert(pen != null);
		Debug.Assert(vertex != null);
		Debug.Assert(drawContext != null);
		AssertValid();

        // Check whether the vertex is drawn as an image.

        Image oImage;

        if ( !PerVertexWithImageDrawer.TryGetImage(vertex, out oImage) )
        {
            // Defer to the base class.

            base.DrawSelfLoop(pen, vertex, drawContext);

            return;
        }

		Rectangle oImageRectangle = PerVertexWithImageDrawer.GetImageRectangle(
			vertex, oImage);

		DrawSelfLoopOnVertexRectangle(pen, vertex, oImageRectangle,
			drawContext);
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
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
