
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Visualization
{
//*****************************************************************************
//  Class: SugiyamaEdgeDrawer
//
/// <summary>
///	Draws an edge in conjunction with <see cref="SugiyamaLayout" /> and <see
/// cref="SugiyamaVertexDrawer" />.
/// </summary>
///
/// <remarks>
/// This class draws edges that have been laid out by <see
/// cref="SugiyamaLayout" /> as curved lines.  It is meant for use with <see
/// cref="SugiyamaVertexDrawer" />.
///
/// <para>
/// This class works in conjunction with <see
/// cref="MultiSelectionGraphDrawer" /> to draw edges as either selected or
/// unselected.  If an edge has been marked as selected by <see
/// cref="MultiSelectionGraphDrawer.SelectEdge(IEdge)" />, it is drawn using
/// <see cref="EdgeDrawer.SelectedColor" /> and <see
/// cref="EdgeDrawer.SelectedWidth" />.  Otherwise, <see
/// cref="EdgeDrawer.Color" /> and <see cref="EdgeDrawer.Width" /> are used.
/// Set the <see cref="EdgeDrawer.UseSelection" /> property to false to force
/// all edges to be drawn as unselected.
/// </para>
///
/// <para>
/// If this class is used by a graph drawer other than <see
/// cref="MultiSelectionGraphDrawer" />, all edges are drawn as unselected.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class SugiyamaEdgeDrawer : PerEdgeDrawer
{
    //*************************************************************************
    //  Constructor: SugiyamaEdgeDrawer()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="SugiyamaEdgeDrawer" />
	/// class.
    /// </summary>
    //*************************************************************************

    public SugiyamaEdgeDrawer()
    {
		// (Do nothing.)

		AssertValid();
    }

    //*************************************************************************
    //  Method: DrawEdgeCore()
    //
    /// <summary>
    /// Draws an edge.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge to draw.
    /// </param>
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
    /// <remarks>
    /// This method gets called repeatedly while a graph is being drawn, once
	/// for each of the graph's edges.  The <see cref="IVertex.Location" />
	///	property on all of the graph's vertices is set by ILayout.<see
	///	cref="ILayout.LayOutGraph" /> before this method is called.
	///
	/// <para>
	/// The arguments have already been checked for validity.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    protected override void
    DrawEdgeCore
    (
        IEdge edge,
		IVertex vertex1,
		IVertex vertex2,
		DrawContext drawContext
    )
	{
		Debug.Assert(edge != null);
		Debug.Assert(vertex1 != null);
		Debug.Assert(vertex2 != null);
		Debug.Assert(drawContext != null);
		AssertValid();

		// Get the edge's curve points that SugiyamaLayout stored in metadata,
		// if they're available.  (If they're not available, then this edge
		// drawer is being used with the wrong layout.  Behave gracefully in
		// this case.)
		//
		// See the definition of ReservedMetadataKeys.SugiyamaCurvePoints for
		// details on what is stored in the key's value.

		Object oValue;

		if ( !edge.TryGetValue(ReservedMetadataKeys.SugiyamaCurvePoints,
			typeof( PointF [] ), out oValue) )
		{
			base.DrawEdgeCore(edge, vertex1, vertex2, drawContext);

			return;
		}

		// Retrieve the edge's curve points.

		PointF [] aoCurvePoints = ( PointF [] )oValue;

		// Draw them.

		Graphics oGraphics = drawContext.Graphics;

		Debug.Assert(oGraphics != null);

		GraphicsPath oGraphicsPath = new GraphicsPath();

		oGraphicsPath.AddBeziers(aoCurvePoints);

		Boolean bShowAsSelected = EdgeShouldBeDrawnSelected(edge);

		Pen oPen = GetPen(edge, false, bShowAsSelected);

		try
		{
			oGraphics.DrawPath(oPen, oGraphicsPath);
		}
		catch (OutOfMemoryException)
		{
			// This is to work around the following apparent bug in
			// Graphics.DrawPath():
			//
			// 1. Shrink the application window so that the graph's window has
			//    zero height.
			//
			// 2. Enlarge the application window so that the graph's window has
			//    a non-zero height.
			//
			// 3. Result: An OutOfMemoryException in Graphics.DrawPath().
			//
			// This is almost certainly a bogus exception.  The memory usage
			// does not go up when this happens, and ignoring the exception
			// does no apparent harm.
			//
			// There have been several Usenet postings about this problem,
			// although no one has posted a solution.

			return;
		}
		finally
		{
			GraphicsUtil.DisposeGraphicsPath(ref oGraphicsPath);
		}

		// Get the edge's endpoint.  See the definition of
		// ReservedMetadataKeys.SugiyamaEndpoint for details on what is stored
		// in the key's value.

		oValue = edge.GetRequiredValue(
			ReservedMetadataKeys.SugiyamaEndpoint, typeof(PointF) );

		PointF oEndpoint = (PointF)oValue;

		// If the edge is directed, show an arrow at the front vertex,
		// oVertex2.

        oPen = GetPen(edge, edge.IsDirected, bShowAsSelected);

		// Draw a line between the edge's last curve point and the endpoint.

		oGraphics.DrawLine(oPen,
			aoCurvePoints[aoCurvePoints.Length - 1],
			oEndpoint
			);
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
