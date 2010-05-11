
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Layouts
{
//*****************************************************************************
//  Class: RandomLayout
//
/// <summary>
/// Lays out a graph by placing the vertices in random locations.
/// </summary>
///
/// <para>
/// If the graph has a metadata key of <see
/// cref="ReservedMetadataKeys.LayOutTheseVerticesOnly" />, only the vertices
/// specified in the value's IVertex collection are laid out and all other
/// vertices are completely ignored.
/// </para>
///
/// <remarks>
/// If a vertex has a metadata key of <see
/// cref="ReservedMetadataKeys.LockVertexLocation" /> with a value of true, it
/// is included in layout calculations but its own location is left unmodified.
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

        base.RandomizeVertexLocations(verticesToLayOut, layoutContext,
            new Random(), false);

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
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
