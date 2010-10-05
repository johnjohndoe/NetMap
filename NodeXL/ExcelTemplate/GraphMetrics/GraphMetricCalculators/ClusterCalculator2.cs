
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Algorithms;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ClusterCalculator2
//
/// <summary>
/// Partitions a graph into clusters.
/// </summary>
///
/// <remarks>
/// Use the <see cref="Algorithm" /> property to specify the clustering
/// algorithm to use.
///
/// <para>
/// See <see cref="Algorithms.ClusterCalculator" /> for details on how the
/// graph is partitioned into clusters.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class ClusterCalculator2 : GraphMetricCalculatorBase2
{
    //*************************************************************************
    //  Constructor: ClusterCalculator2()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ClusterCalculator2" />
    /// class.
    /// </summary>
    //*************************************************************************

    public ClusterCalculator2()
    {
        m_eAlgorithm = ClusterAlgorithm.ClausetNewmanMoore;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Algorithm
    //
    /// <summary>
    /// Gets or sets the algorithm to use to partition the graph into clusters.
    /// </summary>
    ///
    /// <value>
    /// The algorithm to use, as a <see cref="ClusterAlgorithm" />.  The
    /// default is <see cref="ClusterAlgorithm.ClausetNewmanMoore" />.
    /// </value>
    //*************************************************************************

    public ClusterAlgorithm
    Algorithm
    {
        get
        {
            AssertValid();

            return (m_eAlgorithm);
        }

        set
        {
            m_eAlgorithm = value;

            AssertValid();
        }
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

        // Partition the graph into clusters using the ClusterCalculator class
        // in the Algorithms namespace, which knows nothing about Excel.

        ICollection<Community> oCommunities;

        Algorithms.ClusterCalculator oClusterCalculator =
            new Algorithms.ClusterCalculator();

        oClusterCalculator.Algorithm = m_eAlgorithm;

        if ( !oClusterCalculator.TryCalculateGraphMetrics(graph,
                calculateGraphMetricsContext.BackgroundWorker,
                out oCommunities) )
        {
            // The user cancelled.

            return (false);
        }

        // Convert the collection of communities to an array of
        // GraphMetricColumn objects.

        graphMetricColumns =
            GroupsToGraphMetricColumnsConverter.Convert<Community>(
                oCommunities,
                (oCommunity) => oCommunity.Vertices
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

        // m_eAlgorithm
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The algorithm to use.

    protected ClusterAlgorithm m_eAlgorithm;
}

}
