
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ConnectedComponentCalculator2
//
/// <summary>
/// Partitions a graph into strongly connected components.
/// </summary>
///
/// <remarks>
/// See <see cref="Algorithms.ConnectedComponentCalculator" /> for details on
/// how the graph is partitioned into strongly connected components.
/// </remarks>
//*****************************************************************************

public class ConnectedComponentCalculator2 : GraphMetricCalculatorBase2
{
    //*************************************************************************
    //  Constructor: ConnectedComponentCalculator2()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ConnectedComponentCalculator2" />
    /// class.
    /// </summary>
    //*************************************************************************

    public ConnectedComponentCalculator2()
    {
        AssertValid();

        // (Do nothing else.)
    }

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
    /// true if the graph metrics were calculated or don't need to be
    /// calculated, false if the user wants to cancel.
    /// </returns>
    ///
    /// <remarks>
    /// This method periodically checks BackgroundWorker.<see
    /// cref="BackgroundWorker.CancellationPending" />.  If true, the method
    /// immediately returns false.
    ///
    /// <para>
    /// It also periodically reports progress by calling the
    /// BackgroundWorker.<see
    /// cref="BackgroundWorker.ReportProgress(Int32, Object)" /> method.  The
    /// userState argument is a <see cref="GraphMetricProgress" /> object.
    /// </para>
    ///
    /// <para>
    /// Calculated metrics for hidden rows are ignored by the caller, because
    /// Excel misbehaves when values are written to hidden cells.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public override Boolean
    TryCalculateGraphMetrics
    (
        IGraph graph,
        CalculateGraphMetricsContext calculateGraphMetricsContext,
        out GraphMetricColumn [] graphMetricColumns
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(calculateGraphMetricsContext != null);
        AssertValid();

        graphMetricColumns = null;

        // Partition the graph into strongly connected components using the
        // ConnectedComponentCalculator class in the Algorithms namespace,
        // which knows nothing about Excel.
        //
        // Note that ConnectedComponentCalculator does its work synchronously.

        Algorithms.ConnectedComponentCalculator oConnectedComponentCalculator =
            new Algorithms.ConnectedComponentCalculator();

        IList< LinkedList<IVertex> > oComponents =
            oConnectedComponentCalculator.CalculateStronglyConnectedComponents(
                graph, false);

        // Convert the collection of components to an array of
        // GraphMetricColumn objects.

        graphMetricColumns =
            GroupsToGraphMetricColumnsConverter.Convert< LinkedList<IVertex> >(
                oComponents,
                (oComponent) => oComponent
                );

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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
