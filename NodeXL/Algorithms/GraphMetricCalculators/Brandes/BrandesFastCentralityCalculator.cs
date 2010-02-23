
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: BrandesFastCentralityCalculator
//
/// <summary>
/// Calculates the betweenness and closeness centralities for each of the
/// graph's vertices.
/// </summary>
///
/// <remarks>
/// The betweenness and closeness centralities are provided as a <see
/// cref="BrandesCentralities" /> object.
///
/// <para>
/// If a vertex is isolated, its betweenness and closeness centralities are
/// zero.
/// </para>
///
/// <para>
/// The algorithm used to calculate betweenness centrality is taken from the
/// paper "A Faster Algorithm for Betweenness Centrality," by Ulrik Brandes.
/// The paper can be found here:
/// </para>
///
/// <para>
/// http://www.inf.uni-konstanz.de/algo/publications/b-fabc-01.pdf
/// </para>
///
/// <para>
/// Section 4 of the paper explains how other centrality measures can be
/// computed within the same algorithm at little additional cost.  That is why
/// closeness centrality is calculated by this class along with betweenness.
/// </para>
///
/// <para>
/// According to the paper, the algorithm works even if the graph has
/// self-loops and duplicate edges.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class BrandesFastCentralityCalculator : GraphMetricCalculatorBase
{
    //*************************************************************************
    //  Constructor: BrandesFastCentralityCalculator()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="BrandesFastCentralityCalculator" /> class.
    /// </summary>
    //*************************************************************************

    public BrandesFastCentralityCalculator()
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

            return ("betweenness and closeness centralities");
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
    /// <param name="brandesCentralities">
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
        out BrandesCentralities brandesCentralities
    )
    {
        Debug.Assert(graph != null);

        Object oGraphMetricsAsObject;

        Boolean bReturn = TryCalculateGraphMetricsCore(graph, backgroundWorker,
            out oGraphMetricsAsObject);

        brandesCentralities = (BrandesCentralities)oGraphMetricsAsObject;

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
    /// notes for the return type.
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

        oGraphMetrics = null;

        IVertexCollection oVertices = oGraph.Vertices;
        Int32 iVertices = oVertices.Count;

        Boolean bGraphIsDirected =
            (oGraph.Directedness == GraphDirectedness.Directed);

        // Note: The variable names and some of the comments below are those
        // used in Algorithm 1 of the Brandes paper mentioned in the class
        // comments.

        // The key is an IVertex.ID and the value is the corresponding
        // betweenness centrality.

        Dictionary<Int32, BrandesVertexCentralities> Cb =
            new Dictionary<Int32, BrandesVertexCentralities>(iVertices);

        foreach (IVertex oVertex in oVertices)
        {
            Cb.Add( oVertex.ID, new BrandesVertexCentralities() );
        }

        // These objects are created once and cleared before the betweenness
        // centrality for each vertex is calculated.  For the dictionaries, the
        // key is an IVertex.ID.

        Stack<IVertex> S = new Stack<IVertex>();

        // The LinkedList elements within P are created on demand.

        Dictionary< Int32, LinkedList<IVertex> > P =
            new Dictionary< Int32, LinkedList<IVertex> >();

        Dictionary<Int32, Int32> sigma = new Dictionary<Int32, Int32>();
        Dictionary<Int32, Int32> d = new Dictionary<Int32, Int32>();
        Queue<IVertex> Q = new Queue<IVertex>();
        Dictionary<Int32, Double> delta = new Dictionary<Int32, Double>();

        Int32 iCalculations = 0;
        Double MaximumCb = 0;

        // These are for calculating geodesic distances.

        Int64 lGeodesicDistanceSum = 0;
        Int32 iGeodesicDistanceCount = 0;
        Int32 iMaximumGeodesicDistance = Int32.MinValue;

        foreach (IVertex s in oVertices)
        {
            // Check for cancellation and report progress every
            // VerticesPerProgressReport calculations.

            if (oBackgroundWorker != null &&
                iCalculations % VerticesPerProgressReport == 0)
            {
                if (oBackgroundWorker.CancellationPending)
                {
                    return (false);
                }

                ReportProgress(iCalculations, iVertices, oBackgroundWorker);
            }

            S.Clear();
            P.Clear();
            sigma.Clear();
            d.Clear();
            Q.Clear();
            delta.Clear();

            foreach (IVertex oVertex in oVertices)
            {
                sigma.Add(oVertex.ID, 0);
                d.Add(oVertex.ID, -1);
                delta.Add(oVertex.ID, 0);
            }

            sigma[s.ID] = 1;
            d[s.ID] = 0;
            Q.Enqueue(s);

            while (Q.Count > 0)
            {
                IVertex v = Q.Dequeue();
                S.Push(v);

                foreach (IVertex w in v.AdjacentVertices)
                {
                    // w found for the first time?

                    if (d[w.ID] < 0)
                    {
                        Q.Enqueue(w);
                        d[w.ID] = d[v.ID] + 1;
                    }

                    // Shortest path to w via v?

                    if (d[w.ID] == d[v.ID] + 1)
                    {
                        sigma[w.ID] += sigma[v.ID];

                        LinkedList<IVertex> PofW;

                        if ( !P.TryGetValue(w.ID, out PofW) )
                        {
                            PofW = new LinkedList<IVertex>();
                            P.Add(w.ID, PofW);
                        }

                        PofW.AddLast(v);
                    }
                }
            }

            // These are for calculating closeness centrality.

            Int64 lClosenessSum = 0;
            Int32 iClosenessOtherVertices = 0;

            // S returns vertices in order of non-increasing distance from s.

            while (S.Count > 0)
            {
                IVertex w = S.Pop();

                LinkedList<IVertex> PofW;

                if ( P.TryGetValue(w.ID, out PofW) )
                {
                    foreach (IVertex v in PofW)
                    {
                        Debug.Assert(sigma[w.ID] != 0);

                        delta[v.ID] +=
                            ( (Double)sigma[v.ID] / (Double)sigma[w.ID] )
                            * ( 1.0 + delta[w.ID] );
                    }
                }

                if (w.ID != s.ID)
                {
                    BrandesVertexCentralities oCentralities = Cb[w.ID];

                    Double CbOfW =
                        oCentralities.BetweennessCentrality + delta[w.ID];

                    MaximumCb = Math.Max(MaximumCb, CbOfW);
                    oCentralities.BetweennessCentrality = CbOfW;

                    Int32 iDistance = d[w.ID];

                    lClosenessSum += iDistance;
                    iClosenessOtherVertices++;

                    lGeodesicDistanceSum += iDistance;
                    iGeodesicDistanceCount++;

                    iMaximumGeodesicDistance =
                        Math.Max(iMaximumGeodesicDistance, iDistance);
                }
            }

            if (iClosenessOtherVertices > 0)
            {
                BrandesVertexCentralities oCentralities = Cb[s.ID];

                oCentralities.ClosenessCentrality =
                    (Double)lClosenessSum / (Double)iClosenessOtherVertices;
            }

            iCalculations++;
        }

        // As noted in the Brandes paper, "the centrality scores need to be
        // divided by two if the graph is undirected, since all shortest paths
        // are considered twice."
        //
        // (Because the calculated centralities are normalized below, this
        // actually has no effect.  The divide-by-two code is included in case
        // normalization is removed in the future.)

        if (!bGraphIsDirected)
        {
            MaximumCb /= 2.0;
        }

        // Adjust the final values.

        foreach (BrandesVertexCentralities oCentralities in Cb.Values)
        {
            Double ThisCb = oCentralities.BetweennessCentrality;

            if (!bGraphIsDirected)
            {
                ThisCb /= 2.0;
            }

            if (MaximumCb != 0)
            {
                // Normalize the value.

                ThisCb /= MaximumCb;
            }

            oCentralities.BetweennessCentrality = ThisCb;
        }

        if (iGeodesicDistanceCount > 0)
        {
            oGraphMetrics = new BrandesCentralities(Cb,

                new Nullable<Int32>(iMaximumGeodesicDistance),

                new Nullable<Single>( (Single)(
                    (Double)lGeodesicDistanceSum /
                    (Double)iGeodesicDistanceCount)
                    )
                );
        }
        else
        {
            oGraphMetrics = new BrandesCentralities( Cb,
                new Nullable<Int32>(),
                new Nullable<Single>() );
        }

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

    /// Number of vertices that are processed before progress is reported and
    /// the cancellation flag is checked.

    protected const Int32 VerticesPerProgressReport = 100;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
