
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: GraphMetricCalculatorBase
//
/// <summary>
/// Base class for classes that implement <see
/// cref="IGraphMetricCalculator" />.
/// </summary>
//*****************************************************************************

public abstract class GraphMetricCalculatorBase :
    Object, IGraphMetricCalculator
{
    //*************************************************************************
    //  Constructor: GraphMetricCalculatorBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GraphMetricCalculatorBase" /> class.
    /// </summary>
    //*************************************************************************

    public GraphMetricCalculatorBase()
    {
        // (Do nothing.)

        // AssertValid();
    }

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

    public abstract String
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

    public Object
    CalculateGraphMetrics
    (
        IGraph graph
    )
    {
        Debug.Assert(graph != null);
        AssertValid();

        Object oGraphMetrics;

        TryCalculateGraphMetricsCore(graph, null, out oGraphMetrics);

        return (oGraphMetrics);
    }

    //*************************************************************************
    //  Method: TryCalculateGraphMetrics()
    //
    /// <overloads>
    /// Attempts to calculate a set of one or more related metrics while
    /// running on a background thread.
    /// </overloads>
    ///
    /// <summary>
    /// Attempts to calculate a set of one or more related metrics while
    /// running on a background thread, and provides the metrics as an Object.
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
    /// Where the graph metrics get stored if true is returned.  The type is
    /// defined and documented by each implementation.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated, false if the user wants to
    /// cancel.
    /// </returns>
    ///
    /// <remarks>
    /// This method periodically checks <paramref
    /// name="backgroundWorker" />.<see
    /// cref="BackgroundWorker.CancellationPending" />.  If true, false is
    /// immediately returned.
    ///
    /// <para>
    /// It also periodically reports progress by calling the
    /// BackgroundWorker.<see
    /// cref="BackgroundWorker.ReportProgress(Int32, Object)" /> method.  The
    /// second argument is a string in the format "Calculating
    /// [GraphMetricDescription]."
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Boolean
    TryCalculateGraphMetrics
    (
        IGraph graph,
        BackgroundWorker backgroundWorker,
        out Object graphMetrics
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(backgroundWorker != null);
        AssertValid();

        return ( TryCalculateGraphMetricsCore(graph, backgroundWorker,
            out graphMetrics) );
    }

    //*************************************************************************
    //  Method: TryCalculateGraphMetricsCore()
    //
    /// <summary>
    /// Attempts to calculate a set of one or more related metrics while
    /// optionally running on a background thread.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// The BackgroundWorker whose thread is calling this method, or null if
    /// the method is being called by some other thread.
    /// </param>
    ///
    /// <param name="oGraphMetrics">
    /// Where the graph metrics get stored if true is returned.  The return
    /// type must be defined and documented by each implementation.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated, false if the user wants to
    /// cancel.
    /// </returns>
    //*************************************************************************

    protected abstract Boolean
    TryCalculateGraphMetricsCore
    (
        IGraph oGraph,
        BackgroundWorker oBackgroundWorker,
        out Object oGraphMetrics
    );


    //*************************************************************************
    //  Method: ReportProgress()
    //
    /// <summary>
    /// Reports progress to the calling thread.
    /// </summary>
    ///
    /// <param name="iCalculationsSoFar">
    /// Number of calculations that have been performed so far.
    /// </param>
    ///
    /// <param name="iTotalCalculations">
    /// Total number of calculations.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// The <see cref="BackgroundWorker" /> object that is performing all graph
    /// metric calculations.
    /// </param>
    //*************************************************************************

    protected void
    ReportProgress
    (
        Int32 iCalculationsSoFar,
        Int32 iTotalCalculations,
        BackgroundWorker oBackgroundWorker
    )
    {
        Debug.Assert(iCalculationsSoFar >= 0);
        Debug.Assert(iTotalCalculations >= 0);
        Debug.Assert(iCalculationsSoFar <= iTotalCalculations);
        Debug.Assert(oBackgroundWorker != null);
        AssertValid();

        Int32 iPercentProgress = 0;

        if (iTotalCalculations > 0)
        {
            iPercentProgress = (Int32) (100F *
                (Single)iCalculationsSoFar / (Single)iTotalCalculations);
        }

        String sProgress = String.Format(

            "Calculating {0}."
            ,
            this.GraphMetricDescription
            );

        oBackgroundWorker.ReportProgress(iPercentProgress, sProgress);
    }

    //*************************************************************************
    //  Method: ReportCannotCalculateGraphMetrics()
    //
    /// <summary>
    /// Throws an exception to the calling thread indicating a condition that
    /// prevents the graph metrics from being calculated.
    /// </summary>
    ///
    /// <param name="sMessage">
    /// Error message, suitable for displaying to the user.
    /// </param>
    ///
    /// <remarks>
    /// This method throws a <see cref="GraphMetricException" />.
    /// </remarks>
    //*************************************************************************

    protected void
    ReportCannotCalculateGraphMetrics
    (
        String sMessage
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sMessage) );
        AssertValid();

        throw new GraphMetricException(
            sMessage + "  No graph metrics have been calculated.");
    }

    //*************************************************************************
    //  Method: CalculateEdgesInFullyConnectedNeighborhood()
    //
    /// <summary>
    /// Calculates the number of edges there would be in the neighborhood of a
    /// vertex if the neighborhood were fully connected.
    /// </summary>
    ///
    /// <param name="iAdjacentVertices">
    /// The number of the vertex's adjacent vertices.
    /// </param>
    ///
    /// <param name="bGraphIsDirected">
    /// true if the graph is directed, false if it's undirected.
    /// </param>
    ///
    /// <returns>
    /// The number of edges in the fully connected neighborhood.
    /// </returns>
    //*************************************************************************

    protected Int32
    CalculateEdgesInFullyConnectedNeighborhood
    (
        Int32 iAdjacentVertices,
        Boolean bGraphIsDirected
    )
    {
        Debug.Assert(iAdjacentVertices >= 0);
        AssertValid();

        return ( ( iAdjacentVertices * (iAdjacentVertices - 1) ) /
            (bGraphIsDirected ? 1: 2) );
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public virtual void
    AssertValid()
    {
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
