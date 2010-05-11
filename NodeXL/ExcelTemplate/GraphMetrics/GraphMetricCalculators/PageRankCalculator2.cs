
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: PageRankCalculator2
//
/// <summary>
/// Calculates the PageRanks for each of the graph's vertices.
/// </summary>
///
/// <remarks>
/// See <see cref="Algorithms.PageRankCalculator" /> for details on how
/// PageRanks are calculated.
/// </remarks>
//*****************************************************************************

public class PageRankCalculator2 : OneDoubleGraphMetricCalculatorBase
{
    //*************************************************************************
    //  Constructor: PageRankCalculator2()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="PageRankCalculator2" />
    /// class.
    /// </summary>
    //*************************************************************************

    public PageRankCalculator2()
    {
        // (Do nothing.)

        AssertValid();
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
    /// true if the graph metrics were calculated, false if the user wants to
    /// cancel.
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

        return ( TryCalculateGraphMetrics(graph, calculateGraphMetricsContext,
            new Algorithms.PageRankCalculator(),

            calculateGraphMetricsContext.GraphMetricUserSettings.
                CalculatePageRank,

            VertexTableColumnNames.PageRank,
            VertexTableColumnWidths.PageRank,
            CellStyleNames.GraphMetricGood, out graphMetricColumns) );
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
