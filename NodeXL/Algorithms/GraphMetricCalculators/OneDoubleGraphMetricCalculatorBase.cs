
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: OneDoubleGraphMetricCalculatorBase
//
/// <summary>
/// Calculates one graph metric of type Double for each of the graph's
/// vertices.
/// </summary>
///
/// <remarks>
/// This is the base class for several derived classes that calculate a graph
/// metric of type Double.
/// </remarks>
//*****************************************************************************

public abstract class OneDoubleGraphMetricCalculatorBase :
    GraphMetricCalculatorBase
{
    //*************************************************************************
    //  Constructor: OneDoubleGraphMetricCalculatorBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="OneDoubleGraphMetricCalculatorBase" /> class.
    /// </summary>
    //*************************************************************************

    public OneDoubleGraphMetricCalculatorBase()
    {
        // (Do nothing.)

        // AssertValid();
    }

    //*************************************************************************
    //  Method: CalculateGraphMetrics()
    //
    /// <summary>
    /// Calculate the graph metrics.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <returns>
    /// The graph metrics.  There is one key/value pair for each vertex in the
    /// graph.  The key is the IVertex.ID and the value is the vertex's metric,
    /// as a Double.
    /// </returns>
    //*************************************************************************

    public Dictionary<Int32, Double>
    CalculateGraphMetrics
    (
        IGraph graph
    )
    {
        Debug.Assert(graph != null);
        AssertValid();

        Dictionary<Int32, Double> oGraphMetrics;

        TryCalculateGraphMetrics(graph, null, out oGraphMetrics);

        return (oGraphMetrics);
    }

    //*************************************************************************
    //  Method: TryCalculateGraphMetrics()
    //
    /// <summary>
    /// Attempts to calculate the graph metrics while optionally running on a
    /// background thread.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <param name="backgroundWorker">
    /// The BackgroundWorker whose thread is calling this method, or null if
    /// the method is being called by some other thread.
    /// </param>
    ///
    /// <param name="graphMetrics">
    /// Where the graph metrics get stored if true is returned.  There is one
    /// key/value pair for each vertex in the graph.  The key is the IVertex.ID
    /// and the value is the vertex's metric, as a Double.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated, false if the user wants to
    /// cancel.
    /// </returns>
    //*************************************************************************

    public abstract Boolean
    TryCalculateGraphMetrics
    (
        IGraph graph,
        BackgroundWorker backgroundWorker,
        out Dictionary<Int32, Double> graphMetrics
    );


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
