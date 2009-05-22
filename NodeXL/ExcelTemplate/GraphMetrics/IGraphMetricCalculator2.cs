
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Interface: IGraphMetricCalculator2
//
/// <summary>
/// Supports the calculation of one set of graph metrics.
/// </summary>
///
/// <remarks>
/// There is a family of classes that calculate metrics for a graph.  Each
/// class calculates a set of one or more related metrics, and each class
/// implements this interface.  Each calculated metric eventually gets written
/// to a worksheet column.
///
/// <para>
/// To add a new class that calculates graph metrics, do the following:
/// </para>
///
/// <list type="number">
///
/// <item><description>
/// Implement a class that implements <see cref="IGraphMetricCalculator2" />.
/// Optionally derive the class from <see cref="GraphMetricCalculatorBase2" />.
/// </description></item>
///
/// <item><description>
/// Modify <see cref="GraphMetricUserSettings" />.
/// </description></item>
///
/// <item><description>
/// Modify <see cref="GraphMetricsDialog" />.
/// </description></item>
///
/// <item><description>
/// Add an entry to the array of <see cref="IGraphMetricCalculator2" />
/// implementations within the <see cref="GraphMetricCalculationManager" />
/// class.
/// </description></item>
///
/// </list>
///
/// </remarks>
//*****************************************************************************

public interface IGraphMetricCalculator2
{
    //*************************************************************************
    //  Method: TryCalculateGraphMetrics()
    //
    /// <summary>
    /// Attempts to calculate a set of one or more related metrics.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <param name="calculateGraphMetricsContext">
    /// Provides access to objects needed for calculating graph metrics.
    /// </param>
    ///
    /// <param name="graphMetricColumns">
    /// Where an array of GraphMetricColumn objects gets stored if true is
    /// returned, one for each related metric calculated by this method.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated, false if the user wants to
    /// cancel.
    /// </returns>
    ///
    /// <remarks>
    /// This method should periodically check BackgroundWorker.<see
    /// cref="BackgroundWorker.CancellationPending" />.  If true, the method
    /// should immediately return false.
    ///
    /// <para>
    /// It should also periodically report progress by calling the
    /// BackgroundWorker.<see
    /// cref="BackgroundWorker.ReportProgress(Int32, Object)" /> method.  The
    /// userState argument must be a <see cref="GraphMetricProgress" /> object.
    /// </para>
    ///
    /// <para>
    /// Calculated metrics for hidden rows are ignored by the caller, because
    /// Excel misbehaves when values are written to hidden cells.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    Boolean
    TryCalculateGraphMetrics
    (
        IGraph graph,
        CalculateGraphMetricsContext calculateGraphMetricsContext,
        out GraphMetricColumn [] graphMetricColumns
    );
}

}
