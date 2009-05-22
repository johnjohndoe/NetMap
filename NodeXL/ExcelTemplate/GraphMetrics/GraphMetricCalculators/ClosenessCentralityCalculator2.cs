
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ClosenessCentralityCalculator2
//
/// <summary>
/// Calculates the closeness centrality for each of the graph's vertices.
/// </summary>
///
/// <remarks>
/// See <see cref="Algorithms.ClosenessCentralityCalculator" /> for details on
/// how closeness centralities are calculated.
/// </remarks>
//*****************************************************************************

public class ClosenessCentralityCalculator2 : GraphMetricCalculatorBase2
{
    //*************************************************************************
    //  Constructor: ClosenessCentralityCalculator2()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ClosenessCentralityCalculator2" /> class.
    /// </summary>
    //*************************************************************************

    public ClosenessCentralityCalculator2()
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
            CalculateClosenessCentrality)
        {
            return (true);
        }

        // Calculate the closeness centralities for each vertex using the
        // ClosenessCentralityCalculator class in the Algorithms namespace,
        // which knows nothing about Excel.

        Dictionary<Int32, Double> oClosenessCentralities;

        if ( !( new Algorithms.ClosenessCentralityCalculator() ).
            TryCalculateGraphMetrics(graph,
                calculateGraphMetricsContext.BackgroundWorker,
                out oClosenessCentralities) )
        {
            // The user cancelled.

            return (false);
        }

        // Transfer the closeness centralities to an array of GraphMetricValue
        // objects.

        List<GraphMetricValueWithID> oGraphMetricValues =
            new List<GraphMetricValueWithID>();

        foreach (IVertex oVertex in graph.Vertices)
        {
            // Try to get the row ID stored in the worksheet.

            Int32 iRowID;

            if ( TryGetRowID(oVertex, out iRowID) )
            {
                oGraphMetricValues.Add( new GraphMetricValueWithID(iRowID,
                    oClosenessCentralities[oVertex.ID] ) );
            }
        }

        graphMetricColumns = new GraphMetricColumn [] {
            new GraphMetricColumnWithID( WorksheetNames.Vertices,
                TableNames.Vertices,
                VertexTableColumnNames.ClosenessCentrality,
                VertexTableColumnWidths.ClosenessCentrality,
                NumericFormat, CellStyleNames.GraphMetricGood,
                oGraphMetricValues.ToArray()
                ) };

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
    //  Protected constants
    //*************************************************************************

    /// Number format for the column.

    protected const String NumericFormat = "0.000";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
