
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Layouts
{
//*****************************************************************************
//  Class: SortableLayoutBase
//
/// <summary>
/// Base class for layouts that support vertex sorting and selective layout.
/// </summary>
///
/// <remarks>
/// This is the base class for several layouts that support vertex sorting and
/// the layout of a subset of the graph's vertices.  The derived class must
/// implement the <see cref="LayOutGraphCoreSorted" /> abstract method.
///
/// <para>
/// If you want the vertices to be placed in a certain order, set the <see
/// cref="VertexSorter" /> property to an object that will sort them.
/// </para>
///
/// <para>
/// If the graph has a metadata key of <see
/// cref="ReservedMetadataKeys.LayOutTheseVerticesOnly" />, only the vertices
/// specified in the value's IVertex array are laid out and all other vertices
/// are completely ignored.
/// </para>
///
/// </remarks>
//*****************************************************************************

public abstract class SortableLayoutBase : AsyncLayoutBase
{
    //*************************************************************************
    //  Constructor: SortableLayoutBase()
    //
    /// <summary>
    /// Initializes a new instance of the SortableLayoutBase class.
    /// </summary>
    //*************************************************************************

    public SortableLayoutBase()
    {
        m_oVertexSorter = null;

        // AssertValid();
    }

    //*************************************************************************
    //  Property: VertexSorter
    //
    /// <summary>
    /// Gets or sets the vertex sorter to use.
    /// </summary>
    ///
    /// <value>
    /// The vertex sorter to use to sort the vertices before they are laid out,
    /// as an <see cref="IVertexSorter" />, or null if the vertices should not
    /// be sorted.
    /// </value>
    ///
    /// <remarks>
    /// If you want the vertices to be placed in a certain order, set the <see
    /// cref="VertexSorter" /> property to an object that will sort them.  The
    /// ByMetadataVertexSorter and ByDelegateVertexSorter classes can be used
    /// for this.
    /// </remarks>
    //*************************************************************************

    public IVertexSorter
    VertexSorter
    {
        get
        {
            AssertValid();

            return (m_oVertexSorter);
        }

        set
        {
            m_oVertexSorter = value;

            AssertValid();
        }
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

        // Sort the vertices if necessary.

        if (m_oVertexSorter != null)
        {
            verticesToLayOut = m_oVertexSorter.Sort(verticesToLayOut);
        }

        return ( LayOutGraphCoreSorted(graph, verticesToLayOut, layoutContext,
            backgroundWorker) );
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

    protected abstract Boolean
    LayOutGraphCoreSorted
    (
        IGraph graph,
        ICollection<IVertex> verticesToLayOut,
        LayoutContext layoutContext,
        BackgroundWorker backgroundWorker
    );


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

        // m_oVertexSorter
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The vertex sorter to use to sort the vertices before they are laid out,
    /// or null if the vertices should not be sorted.

    protected IVertexSorter m_oVertexSorter;
}

}
