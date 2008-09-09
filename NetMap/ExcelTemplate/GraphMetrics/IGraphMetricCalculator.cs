
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.ExcelTemplate
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
/// Implement a class that implements <see cref="IGraphMetricCalculator" />.
/// Optionally derive the class from <see cref="GraphMetricCalculatorBase" />.
/// </description></item>
///
/// <item><description>
/// Modify <see cref="GraphMetricUserSettings" />.
/// </description></item>
///
/// <item><description>
/// Modify <see cref="GraphMetricUserSettingsDialog" />.
/// </description></item>
///
/// <item><description>
/// Add an entry to the array of <see cref="IGraphMetricCalculator" />
/// implementations within the <see cref="GraphMetricCalculationManager" />
/// class.
/// </description></item>
///
/// </list>
///
/// </remarks>
//*****************************************************************************

public interface IGraphMetricCalculator
{
	//*************************************************************************
	//	Property: GraphMetricDescription
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
    /// Calculates a set of one or more related metrics.
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
	/// <returns>
	/// An array of GraphMetricColumn objects, one for each related metric
	/// calculated by this method.
	/// </returns>
	///
	/// <remarks>
	/// This method should periodically check BackgroundWorker.<see
	/// cref="BackgroundWorker.CancellationPending" />.  If true, the method
	/// should set DoWorkEventArgs.<see cref="CancelEventArgs.Cancel" /> to
	/// true and return null immediately.
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

    GraphMetricColumn []
    CalculateGraphMetrics
    (
		IGraph graph,
		CalculateGraphMetricsContext calculateGraphMetricsContext
    );
}

}
