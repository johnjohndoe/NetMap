
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Algorithms;
using Microsoft.NodeXL.Common;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: OverallMetricCalculator2
//
/// <summary>
/// Calculates the overall metrics for the graph.
/// </summary>
///
/// <remarks>
/// See <see cref="Algorithms.OverallMetricCalculator" /> for details on how
/// overall metrics are calculated.
/// </remarks>
//*****************************************************************************

public class OverallMetricCalculator2 : GraphMetricCalculatorBase2
{
    //*************************************************************************
    //  Constructor: OverallMetricCalculator2()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="OverallMetricCalculator2" /> class.
    /// </summary>
    //*************************************************************************

    public OverallMetricCalculator2()
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

        graphMetricColumns = new GraphMetricColumn[0];

        if (!calculateGraphMetricsContext.GraphMetricUserSettings.
            CalculateOverallMetrics)
        {
            return (true);
        }

        // Calculate the overall metrics using the OverallMetricCalculator
        // class in the Algorithms namespace, which knows nothing about Excel.

        OverallMetrics oOverallMetrics;

        if ( !( new Algorithms.OverallMetricCalculator() ).
            TryCalculateGraphMetrics(graph,
                calculateGraphMetricsContext.BackgroundWorker,
                out oOverallMetrics) )
        {
            // The user cancelled.

            return (false);
        }

        String sDuplicateEdgeStyle = CellStyleNames.GraphMetricGood;

        if (oOverallMetrics.EdgesWithDuplicates > 0)
        {
            // The graph density is rendered invalid when the graph has
            // duplicate edges.

            sDuplicateEdgeStyle = CellStyleNames.GraphMetricBad;
        }

        Double dGraphDensity = oOverallMetrics.GraphDensity;

        GraphMetricValueOrdered [] aoMetricNameGraphMetricValues =
            new GraphMetricValueOrdered [] {

                new GraphMetricValueOrdered("Graph Type"),
                new GraphMetricValueOrdered(),
                new GraphMetricValueOrdered("Unique Edges"),

                new GraphMetricValueOrdered("Edges With Duplicates",
                    sDuplicateEdgeStyle),

                new GraphMetricValueOrdered("Total Edges"),
                new GraphMetricValueOrdered(),
                new GraphMetricValueOrdered("Self-Loops"),
                new GraphMetricValueOrdered(),
                new GraphMetricValueOrdered("Vertices"),
                new GraphMetricValueOrdered(),

                new GraphMetricValueOrdered("Graph Density",
                    sDuplicateEdgeStyle),

                new GraphMetricValueOrdered(),
                new GraphMetricValueOrdered("NodeXL Version"),
                };

        GraphMetricValueOrdered [] aoMetricValueGraphMetricValues =
            new GraphMetricValueOrdered [] {

                new GraphMetricValueOrdered(
                    oOverallMetrics.Directedness.ToString() ),

                new GraphMetricValueOrdered(),

                new GraphMetricValueOrdered( FormatInt32(
                    oOverallMetrics.UniqueEdges) ),

                new GraphMetricValueOrdered( FormatInt32(
                    oOverallMetrics.EdgesWithDuplicates) ),

                new GraphMetricValueOrdered(
                    FormatInt32(oOverallMetrics.TotalEdges) ),

                new GraphMetricValueOrdered(),

                new GraphMetricValueOrdered(
                    FormatInt32(oOverallMetrics.SelfLoops) ),

                new GraphMetricValueOrdered(),

                new GraphMetricValueOrdered(
                    FormatInt32(oOverallMetrics.Vertices) ),

                new GraphMetricValueOrdered(),

                new GraphMetricValueOrdered(
                    (dGraphDensity == OverallMetrics.NoGraphDensity) ?
                        "Not Applicable" : dGraphDensity.ToString(),
                    sDuplicateEdgeStyle),

                new GraphMetricValueOrdered(),
                new GraphMetricValueOrdered( AssemblyUtil2.GetFileVersion() ),
                };

        graphMetricColumns = new GraphMetricColumn[] {

            new GraphMetricColumnOrdered(WorksheetNames.OverallMetrics,
                TableNames.OverallMetrics,
                OverallMetricsTableColumnNames.Name,
                OverallMetricsTableColumnWidths.Name,
                null, CellStyleNames.GraphMetricGood,
                aoMetricNameGraphMetricValues
                ),

            new GraphMetricColumnOrdered(WorksheetNames.OverallMetrics,
                TableNames.OverallMetrics,
                OverallMetricsTableColumnNames.Value,
                OverallMetricsTableColumnWidths.Value,
                null, CellStyleNames.GraphMetricGood,
                aoMetricValueGraphMetricValues
                ),
            };

        return (true);
    }

    //*************************************************************************
    //  Method: FormatInt32()
    //
    /// <summary>
    /// Formats an Int32 for use in the metric value column
    /// </summary>
    ///
    /// <param name="iInt32">
    /// Int32 to format.
    /// </param>
    ///
    /// <returns>
    /// Formatted Int32.
    /// </returns>
    //*************************************************************************

    protected String
    FormatInt32
    (
        Int32 iInt32
    )
    {
        AssertValid();

        return ( iInt32.ToString(ExcelTemplateForm.Int32Format) );
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
