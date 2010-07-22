
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Algorithms
{
//*****************************************************************************
//  Interface: IGraphMetricCalculator
//
/// <summary>
/// Supports the calculation of one set of graph metrics.
/// </summary>
///
/// <remarks>
/// There is a family of classes that calculate metrics for a graph.  Each
/// class calculates a set of one or more related metrics, and each class
/// implements this interface.
/// </remarks>
//*****************************************************************************

public interface IGraphMetricCalculator
{
    //*************************************************************************
    //  Property: GraphMetricDescription
    //
    /// <summary>
    /// Gets a description of the graph metrics calculated by the
    /// implementation.
    /// </summary>
    ///
    /// <value>
    /// A description suitable for use within the sentence "Calculating
    /// [GraphMetricDescription]."
    /// </value>
    //*************************************************************************

    String
    GraphMetricDescription
    {
        get;
    }

    //*************************************************************************
    //  Method: CalculateGraphMetrics()
    //
    /// <summary>
    /// Calculate a set of one or more related metrics.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <returns>
    /// The graph metrics.  The return type must be defined and documented by
    /// each implementation.
    /// </returns>
    //*************************************************************************

    Object
    CalculateGraphMetrics
    (
        IGraph graph
    );


    //*************************************************************************
    //  Method: TryCalculateGraphMetrics()
    //
    /// <summary>
    /// Attempts to calculate a set of one or more related metrics while
    /// running on a background thread.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <param name="backgroundWorker">
    /// The BackgroundWorker whose thread is calling this method.
    /// </param>
    ///
    /// <param name="graphMetrics">
    /// Where the graph metrics get stored if true is returned.  The type must
    /// be defined and documented by each implementation.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated, false if the user wants to
    /// cancel.
    /// </returns>
    ///
    /// <remarks>
    /// This method should periodically check <paramref
    /// name="backgroundWorker" />.<see
    /// cref="BackgroundWorker.CancellationPending" />.  If true, the method
    /// should immediately return false.
    ///
    /// <para>
    /// It should also periodically report progress by calling the
    /// BackgroundWorker.<see
    /// cref="BackgroundWorker.ReportProgress(Int32, Object)" /> method.  The
    /// second argument must be a string in the format "Calculating
    /// [GraphMetricDescription]."
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    Boolean
    TryCalculateGraphMetrics
    (
        IGraph graph,
        BackgroundWorker backgroundWorker,
        out Object graphMetrics
    );
}

}
