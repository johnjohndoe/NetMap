
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.Layouts
{
//*****************************************************************************
//  Class: PolarLayoutBase
//
/// <summary>
/// Base class for classes that lay out a graph by placing the vertices within
/// a polar coordinate space.
/// </summary>
//*****************************************************************************

public class PolarLayoutBase : AsyncLayoutBase
{
    //*************************************************************************
    //  Constructor: PolarLayoutBase()
    //
    /// <summary>
    /// Initializes a new instance of the PolarLayoutBase class.
    /// </summary>
    ///
    /// <param name="polarRIsAbsolute">
    /// If true, the polar R coordinates are in WPF units and have no upper
    /// limit.  If false, the polar R coordinates can vary from 0.0 to 1.0, and
    /// 1.0 represents half of either the graph rectangle height or width,
    /// whichever is smaller.
    /// </param>
    //*************************************************************************

    public PolarLayoutBase
    (
        Boolean polarRIsAbsolute
    )
    {
        m_bPolarRIsAbsolute = polarRIsAbsolute;

        // AssertValid();
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
    /// This method lays out the graph <paramref name="graph" /> either
    /// synchronously (if <paramref name="backgroundWorker" /> is null) or
    /// asynchronously (if (<paramref name="backgroundWorker" /> is not null)
    /// by setting the the <see cref="IVertex.Location" /> property on the
    /// vertices in <paramref name="verticesToLayOut" /> and optionally adding
    /// geometry metadata to the graph, vertices, or edges.
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
        ICollection<IVertex> verticesToLayOut,
        LayoutContext layoutContext,
        BackgroundWorker backgroundWorker
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(verticesToLayOut != null);
        Debug.Assert(verticesToLayOut.Count > 0);
        Debug.Assert(layoutContext != null);
        AssertValid();

        if (backgroundWorker != null && backgroundWorker.CancellationPending)
        {
            return (false);
        }

        Double dCenterX, dCenterY, dHalfSize;

        GetRectangleCenterAndHalfSize(layoutContext.GraphRectangle,
            out dCenterX, out dCenterY, out dHalfSize);

        foreach (IVertex oVertex in verticesToLayOut)
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

                Double dR = oSinglePolarCoordinates.R;

                if (!m_bPolarRIsAbsolute)
                {
                    dR = Math.Max(dR, 0.0);
                    dR = Math.Min(dR, 1.0) * dHalfSize;
                }

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

        // m_bPolarRIsAbsolute
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// If true, the polar R coordinates are in WPF units and have no upper
    /// limit.  If false, the polar R coordinates can vary from 0.0 to 1.0, and
    /// 1.0 represents half of either the graph rectangle height or width,
    /// whichever is smaller.

    protected Boolean m_bPolarRIsAbsolute;
}

}
