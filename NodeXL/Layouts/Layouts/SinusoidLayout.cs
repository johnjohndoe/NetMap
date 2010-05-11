
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Layouts
{
//*****************************************************************************
//  Class: SinusoidHorizontalLayout
//
/// <summary>
/// Lays out a graph by placing the vertices on a horizontal sine wave.
/// </summary>
///
/// <remarks>
/// This layout places a graph's vertices on one cycle of a sine wave that
/// spans the rectangle horizontally from left to right.
///
/// <para>
/// If the graph has a metadata key of <see
/// cref="ReservedMetadataKeys.LayOutTheseVerticesOnly" />, only the vertices
/// specified in the value's IVertex collection are laid out and all other
/// vertices are completely ignored.
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
/// </remarks>
//*****************************************************************************

public class SinusoidHorizontalLayout : SinusoidLayout
{
    //*************************************************************************
    //  Constructor: SinusoidHorizontalLayout()
    //
    /// <summary>
    /// Initializes a new instance of the SinusoidHorizontalLayout class.
    /// </summary>
    //*************************************************************************

    public SinusoidHorizontalLayout()
    :
    base(true, 4 * Math.PI)
    {
        // (Do nothing.)

        AssertValid();
    }
}


//*****************************************************************************
//  Class: SinusoidVerticalLayout
//
/// <summary>
/// Lays out a graph by placing the vertices on a vertical sine wave.
/// </summary>
///
/// <remarks>
/// This layout places a graph's vertices on one cycle of a sine wave that
/// spans the rectangle vertically from top to bottom.
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
/// cref="ReservedMetadataKeys.LockVertexLocation" />, it is included in layout
/// calculations but its own location is left unmodified.
/// </para>
///
/// <para>
/// If you want the vertices to be placed in a certain order, set the <see
/// cref="SortableLayoutBase.VertexSorter" /> property to an object that will
/// sort them.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class SinusoidVerticalLayout : SinusoidLayout
{
    //*************************************************************************
    //  Constructor: SinusoidVerticalLayout()
    //
    /// <summary>
    /// Initializes a new instance of the SinusoidVerticalLayout class.
    /// </summary>
    //*************************************************************************

    public SinusoidVerticalLayout()
    :
    base(false, 4 * Math.PI)
    {
        // (Do nothing.)

        AssertValid();
    }
}


//*****************************************************************************
//  Class: SinusoidLayout
//
/// <summary>
/// Lays out a graph by placing the vertices on a horizontal or vertical sine
/// wave.
/// </summary>
///
/// <remarks>
/// This layout places a graph's vertices on one cycle of a sine wave that
/// spans the rectangle either horizontally or vertically.
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
/// cref="ReservedMetadataKeys.LockVertexLocation" />, it is included in layout
/// calculations but its own location is left unmodified.
/// </para>
///
/// <para>
/// If you want the vertices to be placed in a certain order, set the <see
/// cref="SortableLayoutBase.VertexSorter" /> property to an object that will
/// sort them.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class SinusoidLayout : SortableLayoutBase
{
    //*************************************************************************
    //  Constructor: SinusoidLayout()
    //
    /// <summary>
    /// Initializes a new instance of the SinusoidLayout class.
    /// </summary>
    ///
    /// <param name="isHorizontal">
    /// true for a sine wave that runs from left to right, false for top to
    /// bottom.
    /// </param>
    ///
    /// <param name="cycleLength">
    /// Length of the sine wave cycle, in radians.
    /// </param>
    //*************************************************************************

    public SinusoidLayout
    (
        Boolean isHorizontal,
        Double cycleLength
    )
    {
        m_bIsHorizontal = isHorizontal;
        m_dCycleLength = cycleLength;

        // AssertValid();
    }

    //*************************************************************************
    //  Method: LayOutGraphCoreSorted()
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
    LayOutGraphCoreSorted
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

        Int32 iVertices = verticesToLayOut.Count;

        // The vertices are placed at equal angles along one cycle of a sine
        // wave.

        Rectangle oRectangle = layoutContext.GraphRectangle;

        Double dWidth = (Double)oRectangle.Width;
        Double dHeight = (Double)oRectangle.Height;
        Double dWidthOrHeight = (m_bIsHorizontal ? dWidth : dHeight);
        Double dHeightOrWidth = (m_bIsHorizontal ? dHeight : dWidth);

        Double dXorYIncrement = dWidthOrHeight / (Double)iVertices;
        Double dAmplitude = dHeightOrWidth / 2.0;
        Double dSinFactor = m_dCycleLength / dWidthOrHeight;

        Single fYorXOffset =
            (m_bIsHorizontal ? oRectangle.Top : oRectangle.Left) +
            (Single)dHeightOrWidth / 2.0F;

        Single fXorYOffset =
            m_bIsHorizontal ? oRectangle.Left : oRectangle.Top;

        Double dXorY = 0;

        foreach (IVertex oVertex in verticesToLayOut)
        {
            if ( !VertexIsLocked(oVertex) )
            {
                Single fYorX = fYorXOffset -
                    (Single)( dAmplitude * Math.Sin(dXorY * dSinFactor) );

                oVertex.Location = m_bIsHorizontal ?

                    new PointF( (Single)dXorY + fXorYOffset, fYorX)
                    :
                    new PointF(fYorX, (Single)dXorY + fXorYOffset);
            }

            dXorY += dXorYIncrement;
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

        // m_bIsHorizontal
        Debug.Assert(m_dCycleLength > 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// true for a sine wave that runs from left to right, false for top to
    /// bottom.

    protected Boolean m_bIsHorizontal;

    /// Length of the sine wave cycle, in radians.

    protected Double m_dCycleLength;
}

}
