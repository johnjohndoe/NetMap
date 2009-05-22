
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.Layouts
{
//*****************************************************************************
//  Class: PolarLayout
//
/// <summary>
/// Lays out a graph by placing the vertices within a polar coordinate space.
/// </summary>
///
/// <remarks>
/// This layout defines a polar coordinate space that uses (R, Angle) to
/// specify a point.  R represents the distance of the point from the origin,
/// which is the center of the graph rectangle, and can vary from 0.0 to 1.0.
/// 1.0 represents half of either the graph rectangle height or width,
/// whichever is smaller.  Angle is the counterclockwise angle of the point
/// from the positive x-axis and can vary from 0.0 to 360.0 degrees.
///
/// <para>
/// To specify the polar coordinates of a vertex, add the <see
/// cref="ReservedMetadataKeys.PolarLayoutCoordinates" /> key to the vertex.
/// If a vertex is missing this key, the vertex is placed at the origin.
/// </para>
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
/// cref="ReservedMetadataKeys.LockVertexLocation" /> with a value of true, its
/// location is left unmodified.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class PolarLayout : AsyncLayoutBase
{
    //*************************************************************************
    //  Constructor: PolarLayout()
    //
    /// <summary>
    /// Initializes a new instance of the PolarLayout class.
    /// </summary>
    //*************************************************************************

    public PolarLayout()
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
    /// This method lays out the graph <paramref name="graph" /> either
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

        if (backgroundWorker != null && backgroundWorker.CancellationPending)
        {
            return (false);
        }

        // Honor the optional LayOutTheseVerticesOnly key on the graph.

        ICollection oVerticesToLayOut = GetVerticesToLayOut(graph);

        Double dCenterX, dCenterY, dHalfSize;

        GetRectangleCenterAndHalfSize(layoutContext.GraphRectangle,
            out dCenterX, out dCenterY, out dHalfSize);

        foreach (IVertex oVertex in oVerticesToLayOut)
        {
            if ( VertexIsLocked(oVertex) )
            {
                continue;
            }

            Double dX = dCenterX;
            Double dY = dCenterY;
            Object oSinglePolarCoordinatesAsObject;

            if ( oVertex.TryGetValue(
                ReservedMetadataKeys.PolarLayoutCoordinates,
                typeof(SinglePolarCoordinates),
                out oSinglePolarCoordinatesAsObject) )
            {
                SinglePolarCoordinates oSinglePolarCoordinates =
                    (SinglePolarCoordinates)oSinglePolarCoordinatesAsObject;

                // Pin the R coordinate to the range (0, 1).

                Double dR = Math.Min(oSinglePolarCoordinates.R, 1.0);
                dR = Math.Max(dR, 0.0) * dHalfSize;

                Double dAngleRadians = -MathUtil.DegreesToRadians(
                    oSinglePolarCoordinates.Angle);

                dX = dCenterX + dR * Math.Cos(dAngleRadians);
                dY = dCenterY + dR * Math.Sin(dAngleRadians);
            }

            oVertex.Location = new PointF( (Single)dX,  (Single)dY );
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

        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
