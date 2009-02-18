
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: EigenvectorCentralityCalculator
//
/// <summary>
/// Calculates the eigenvector centrality for each of the graph's vertices.
/// </summary>
///
/// <remarks>
/// The eigenvector centralities are provided as a
/// Dictionary&lt;Int32, Double&gt;.  There is one key/value pair for each
/// vertex in the graph.  The key is the IVertex.ID and the value is the
/// vertex's eigenvector centrality, as a Double.
///
/// <para>
/// Eigenvector centrality is defined in this article:
/// </para>
///
/// <para>
/// http://en.wikipedia.org/wiki/Eigenvector_centrality#Eigenvector_centrality
/// </para>
///
/// <para>
/// The dominant eigenvector required for eigenvector centrality is found using
/// the "accelerated power method" outlined here:
/// </para>
///
/// <para>
/// http://www.analytictech.com/networks/centaids.htm
/// </para>
///
/// </remarks>
//*****************************************************************************

public class EigenvectorCentralityCalculator : GraphMetricCalculatorBase
{
    //*************************************************************************
    //  Constructor: EigenvectorCentralityCalculator()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="EigenvectorCentralityCalculator" /> class.
    /// </summary>
    //*************************************************************************

    public EigenvectorCentralityCalculator()
    {
        // (Do nothing.)

        AssertValid();
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

    public override String
    GraphMetricDescription
    {
        get
        {
            AssertValid();

            return ("eigenvector centralities");
        }
    }

    //*************************************************************************
    //  Method: TryCalculateGraphMetrics()
    //
    /// <summary>
    /// Attempts to calculate a set of one or more related metrics while
    /// optionally running on a background thread, and provides the metrics in
    /// a type-safe manner.
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
    /// <param name="eigenvectorCentralities">
    /// Where the graph metrics get stored if true is returned.  See the class
    /// notes for details on the type.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated, false if the user wants to
    /// cancel.
    /// </returns>
    //*************************************************************************

    public Boolean
    TryCalculateGraphMetrics
    (
        IGraph graph,
        BackgroundWorker backgroundWorker,
        out Dictionary<Int32, Double> eigenvectorCentralities
    )
    {
        Debug.Assert(graph != null);

        Object oGraphMetricsAsObject;

        Boolean bReturn = TryCalculateGraphMetricsCore(graph, backgroundWorker,
            out oGraphMetricsAsObject);

        eigenvectorCentralities =
            ( Dictionary<Int32, Double> )oGraphMetricsAsObject;

        return (bReturn);
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
    /// Where the graph metrics get stored if true is returned.  See the class
    /// notes for details on the type.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated, false if the user wants to
    /// cancel.
    /// </returns>
    //*************************************************************************

    protected override Boolean
    TryCalculateGraphMetricsCore
    (
        IGraph oGraph,
        BackgroundWorker oBackgroundWorker,
        out Object oGraphMetrics
    )
    {
        Debug.Assert(oGraph != null);
        AssertValid();

        IVertexCollection oVertices = oGraph.Vertices;
        Int32 iVertices = oVertices.Count;

        Dictionary<Int32, Double> oEigenvectorCentralities =
            new Dictionary<Int32, Double>(iVertices);

        oGraphMetrics = oEigenvectorCentralities;

        // Create an object that determines whether two vertices are connected
        // by an edge.

        SimpleGraphMatrix oSimpleGraphMatrix = new SimpleGraphMatrix(oGraph);

        // The terms and steps used here are from the "accelerated power
        // method" algorithm outlined in the article mentioned in the class
        // comments.

        Double dLambda = -1;
        Double dNewLambda = -1;
        Double [] adE = new Double[iVertices];
        Int32 i;

        // Step 0.

        for (i = 0; i < iVertices; i++)
        {
            adE[i] = 1;
        }

        Int32 iIterations = 0;

        while (true)
        {
            // The power iteration algorithm is not guaranteed to converge, so
            // stop after an arbitrary number of iterations.

            if (iIterations == MaximumIterations)
            {
                break;
            }

            // Allow at least two iterations before testing the change in
            // lambda.

            if (iIterations >= 2)
            {
                if (
                    dLambda == 0 
                    ||
                    ( ( 100.0 * Math.Abs(dNewLambda - dLambda) ) / dLambda )
                        <= LambdaDifferencePercentForEquality
                    )
                {
                    break;
                }
            }

            dLambda = dNewLambda;

            // Steps 1 to 3.

            dNewLambda = RunSteps1To3(oVertices, adE, oSimpleGraphMatrix);

            iIterations++;

            // Check for cancellation and report progress every
            // IterationsPerProgressReport iterations.

            if (oBackgroundWorker != null &&
                iIterations % IterationsPerProgressReport == 0)
            {
                if (oBackgroundWorker.CancellationPending)
                {
                    return (false);
                }

                ReportProgress(iIterations, MaximumIterations,
                    oBackgroundWorker);
            }
        }

        // Transfer the eigenvector centralities to the dictionary.

        i = 0;

        foreach (IVertex oVertex in oVertices)
        {
            oEigenvectorCentralities.Add( oVertex.ID, adE[i] );

            i++;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: RunSteps1To3()
    //
    /// <summary>
    /// Runs steps 1 to 3 of the accelerated power method.
    /// </summary>
    ///
    /// <param name="oVertices">
    /// The graph's vertices.
    /// </param>
    ///
    /// <param name="adE">
    /// The current eigenvector.
    /// </param>
    ///
    /// <param name="oSimpleGraphMatrix">
    /// Object that determines whether two vertices are connected by an edge.
    /// </param>
    ///
    /// <returns>
    /// The new value of lambda.
    /// </returns>
    ///
    /// <remarks>
    /// The terms and steps used here are from the "accelerated power method"
    /// algorithm outlined in the article mentioned in the class comments.
    /// </remarks>
    //*************************************************************************

    protected Double
    RunSteps1To3
    (
        IVertexCollection oVertices,
        Double [] adE,
        SimpleGraphMatrix oSimpleGraphMatrix
    )
    {
        Debug.Assert(oVertices != null);
        Debug.Assert(adE != null);
        Debug.Assert(adE.Length == oVertices.Count);
        AssertValid();

        Int32 iVertices = oVertices.Count;
        Double [] adEStar = new Double[iVertices];
        Int32 i = 0;
        Int32 j = 0;

        // Step 1.

        foreach (IVertex oVertexi in oVertices)
        {
            Double dEiStar = 0;
            j = 0;

            foreach (IVertex oVertexj in oVertices)
            {
                if ( i != j && oSimpleGraphMatrix.Aij(oVertexi, oVertexj) )
                {
                    dEiStar += adE[j];
                }

                j++;
            }

            adEStar[i] = dEiStar;
            i++;
        }

        // Step 2.

        Double dLambda = 0;

        for (i = 0; i < iVertices; i++)
        {
            dLambda += Math.Pow(adEStar[i], 2);
        }

        dLambda = Math.Sqrt(dLambda);

        // Step 3.

        for (i = 0; i < iVertices; i++)
        {
            if (dLambda != 0)
            {
                adE[i] = adEStar[i] / dLambda;
            }
            else
            {
                // This can happen when the graph consists of one isolated
                // vertex.

                adE[i] = 0;
            }
        }

        return (dLambda);
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

    /// Number of accelerated power method iterations before progress is
    /// reported and the cancellation flag is checked.

    protected const Int32 IterationsPerProgressReport = 1;

    /// Percent difference in lambda from one iteration to the next for which
    /// the lambdas are considered equal.

    protected const Double LambdaDifferencePercentForEquality = 0.0001;

    /// Maximum number of iterations to run.

    protected const Int32 MaximumIterations = 100;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
