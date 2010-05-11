
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Layouts
{
//*****************************************************************************
//  Interface: IAsyncLayout
//
/// <summary>
/// Supports laying out a graph within a rectangle using an event-based
/// asynchronous pattern.
/// </summary>
///
/// <remarks>
/// This interface adds asynchronous semantics to the <see cref="ILayout" />
/// base interface.  A class that implements this interface is responsible for
/// laying out a graph within a specified rectangle by setting the <see
/// cref="IVertex.Location" /> property on all of the graph's vertices in
/// either a synchronous or asynchronous manner, depending on whether <see
/// cref="ILayout.LayOutGraph" /> or <see cref="LayOutGraphAsync" /> is called.
/// It may also add geometry metadata to the graph, vertices, or edges.
///
/// <para>
/// The asynchronous semantics follow the guidelines outlined in the article
/// "Multithreaded Programming with the Event-based Asynchronous Pattern" in
/// the .NET Framework Developer's Guide.  <see cref="LayOutGraphAsync" />
/// starts the layout on a worker thread and returns immediately.  The <see
/// cref="LayOutGraphIterationCompleted" /> event may fire repeatedly while the
/// layout is occurring.  The <see cref="LayOutGraphCompleted" /> event fires
/// when the layout is complete, an error occurs, or the layout is cancelled.
/// <see cref="LayOutGraphAsyncCancel" /> cancels the layout.
/// </para>
///
/// </remarks>
//*****************************************************************************

public interface IAsyncLayout : ILayout
{
    //*************************************************************************
    //  Property: SupportsBinning
    //
    /// <summary>
    /// Gets a flag indicating whether binning can be used when the entire
    /// graph is laid out.
    /// </summary>
    ///
    /// <value>
    /// true if binning can be used.
    /// </value>
    //*************************************************************************

    Boolean
    SupportsBinning
    {
        get;
    }

    //*************************************************************************
    //  Property: UseBinning
    //
    /// <summary>
    /// Gets or sets a flag indicating whether binning should be used when the
    /// graph is laid out.
    /// </summary>
    ///
    /// <value>
    /// true to use binning.  The default value is false.
    /// </value>
    ///
    /// <remarks>
    /// When this property and <see cref="SupportsBinning" /> are both true and
    /// the entire graph is being laid out, the graph is split into
    /// strongly connected components, the smaller components are laid out and
    /// placed along the bottom of the rectangle using the <see
    /// cref="FruchtermanReingoldLayout" />, and the remaining components are
    /// laid out within the remaining rectangle using the algorithm implemented
    /// in the derived class.
    /// </remarks>
    //*************************************************************************

    Boolean
    UseBinning
    {
        get;
        set;
    }

    //*************************************************************************
    //  Property: MaximumVerticesPerBin
    //
    /// <summary>
    /// Gets or sets the maximum number of vertices a binned component can
    /// have.
    /// </summary>
    ///
    /// <value>
    /// The maximum number of vertices a binned component can have.
    /// </value>
    ///
    /// <remarks>
    /// If <see cref="UseBinning" /> is true and a strongly connected component
    /// of the graph has <see cref="MaximumVerticesPerBin" /> vertices or
    /// fewer, the component is placed in a bin.
    /// </remarks>
    //*************************************************************************

    Int32
    MaximumVerticesPerBin
    {
        get;
        set;
    }

    //*************************************************************************
    //  Property: BinLength
    //
    /// <summary>
    /// Gets or sets the height and width of each bin, in graph rectangle
    /// units.
    /// </summary>
    ///
    /// <value>
    /// The height and width of each bin, in graph rectangle units.
    /// </value>
    ///
    /// <remarks>
    /// This property is ignored if <see cref="UseBinning" /> is false.
    /// </remarks>
    //*************************************************************************

    Int32
    BinLength
    {
        get;
        set;
    }

    //*************************************************************************
    //  Property: IsBusy
    //
    /// <summary>
    /// Gets a value indicating whether an asynchronous operation is in
    /// progress.
    /// </summary>
    ///
    /// <value>
    /// true if an asynchronous operation is in progress.
    /// </value>
    //*************************************************************************

    Boolean
    IsBusy
    {
        get;
    }

    //*************************************************************************
    //  Method: LayOutGraph()
    //
    /// <summary>
    /// Lays out a graph synchronously.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to lay out.
    /// </param>
    ///
    /// <param name="layoutContext">
    /// Provides access to objects needed to lay out the graph.
    /// </param>
    ///
    /// <remarks>
    /// This method lays out the graph <paramref name="graph" /> by setting the
    /// <see cref="IVertex.Location" /> property on all of the graph's
    /// vertices, and optionally adding geometry metadata to the graph,
    /// vertices, or edges.
    /// </remarks>
    //*************************************************************************

    new void
    LayOutGraph
    (
        IGraph graph,
        LayoutContext layoutContext
    );


    //*************************************************************************
    //  Method: LayOutGraphAsync()
    //
    /// <summary>
    /// Lays out a graph asynchronously.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to lay out.
    /// </param>
    ///
    /// <param name="layoutContext">
    /// Provides access to objects needed to lay out the graph.
    /// </param>
    ///
    /// <remarks>
    /// This method asynchronously lays out the graph <paramref
    /// name="graph" />.  It returns immediately.  A worker thread sets the
    /// <see cref="IVertex.Location" /> property on all of the graph's
    /// vertices, and optionally adds geometry metadata to the graph, vertices,
    /// or edges.
    ///
    /// <para>
    /// The <see cref="LayOutGraphIterationCompleted" /> event may fire
    /// repeatedly while the layout is occurring.  The <see
    /// cref="LayOutGraphCompleted" /> event fires when the layout is complete,
    /// an error occurs, or the layout is cancelled.  <see
    /// cref="LayOutGraphAsyncCancel" /> cancels the layout.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    void
    LayOutGraphAsync
    (
        IGraph graph,
        LayoutContext layoutContext
    );

    //*************************************************************************
    //  Method: LayOutGraphAsyncCancel()
    //
    /// <summary>
    /// Cancels the layout started by <see cref="LayOutGraphAsync" />.
    /// </summary>
    ///
    /// <remarks>
    /// The layout may or may not cancel, but the <see
    /// cref="LayOutGraphCompleted" /> event is guaranteed to fire.  The <see
    /// cref="AsyncCompletedEventArgs" /> object passed to the event handler
    /// contains a <see cref="AsyncCompletedEventArgs.Cancelled" /> property
    /// that indicates whether the cancellation occurred.
    ///
    /// <para>
    /// If a layout is not in progress, this method does nothing.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    void
    LayOutGraphAsyncCancel();


    //*************************************************************************
    //  Event: LayOutGraphIterationCompleted
    //
    /// <summary>
    /// Occurs when a layout started by <see cref="LayOutGraphAsync" />
    /// completes one iteration.
    /// </summary>
    ///
    /// <remarks>
    /// If the implementation uses an iterative layout algorithm, it should
    /// fire this event after each iteration.  The event handler may draw the
    /// intermediate graph using the <see cref="IVertex.Location" /> property
    /// on all of the graph's vertices.
    ///
    /// <para>
    /// The event fires on the thread on which <see cref="LayOutGraphAsync" />
    /// was called.  Although the <see cref="IVertex.Location" /> property is
    /// not required to be thread-safe, it is safe to read the property during
    /// the event because the implementation's worker thread blocks until the
    /// event handler returns.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    event EventHandler LayOutGraphIterationCompleted;


    //*************************************************************************
    //  Event: LayOutGraphCompleted
    //
    /// <summary>
    /// Occurs when a layout started by <see cref="LayOutGraphAsync" />
    /// completes, is cancelled, or ends with an error.
    /// </summary>
    ///
    /// <remarks>
    /// The event fires on the thread on which <see cref="LayOutGraphAsync" />
    /// was called.
    /// </remarks>
    //*************************************************************************

    event AsyncCompletedEventHandler LayOutGraphCompleted;


    //*************************************************************************
    //  Event: LayoutRequired
    //
    /// <summary>
    /// Occurs when a change occurs that requires the graph to be laid out
    /// again.
    /// </summary>
    ///
    /// <remarks>
    /// The implementation must fire this event when any change is made to the
    /// object that might affect the layout of the graph, such as a property
    /// change that affects the layout algorithm.
    ///
    /// <para>
    /// The owner should lay out the graph and redraw it in response to the
    /// event.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    new event EventHandler LayoutRequired;
}

}
