
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: AllPathsShortestPathCalculator
//
/// <summary>
/// Calculates the length of the shortest path between each pair of vertices in
/// a graph.
/// </summary>
///
/// <remarks>
/// The shortest paths are provided as a two-dimensional array of UInt16s.  The
/// [i,j] element is the length of the shortest path between vertices i and j
/// if there is such a path, or the constant <see cref="NoPath" /> if there is
/// no such path.  The elements on the diagonal (i==j) are always zero.  The
/// i and j indexes assume that the graph's vertex collection is being
/// enumerated via a forward enumerator.  (The vertex collection cannot be
/// indexed and must be enumerated.)
///
/// <para>
/// A breadth-first search is used to compute the shortest path lengths.  The
/// execution time is O(V(V + E)).  The algorithm is outlined here:
/// </para>
///
/// <para>
/// http://en.wikipedia.org/wiki/Breadth-first_search
/// </para>
///
/// </remarks>
//*****************************************************************************

public class AllPathsShortestPathCalculator : GraphMetricCalculatorBase
{
    //*************************************************************************
    //  Constructor: AllPathsShortestPathCalculator()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
    /// cref="AllPathsShortestPathCalculator" /> class with a default graph
    /// metric description.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AllPathsShortestPathCalculator" /> class.
    /// </summary>
    //*************************************************************************

    public AllPathsShortestPathCalculator()
    : this("shortest paths")
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: AllPathsShortestPathCalculator()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AllPathsShortestPathCalculator" /> class with a specified graph
    /// metric description.
    /// </summary>
    ///
    /// <param name="graphMetricDescription">
    /// A description suitable for use within the sentence "Calculating
    /// [GraphMetricDescription]."
    /// </param>
    ///
    /// <remarks>
    /// This overload is provided to allow another graph metric calculator to
    /// use this class while having its own graph metric description used in
    /// progress reports.
    /// </remarks>
    //*************************************************************************

    public AllPathsShortestPathCalculator
    (
        String graphMetricDescription
    )
    {
        m_sGraphMetricDescription = graphMetricDescription;

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

            return (m_sGraphMetricDescription);
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
    /// <param name="allPairsPathLengths">
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
        out UInt16 [,] allPairsPathLengths
    )
    {
        Debug.Assert(graph != null);

        Object oGraphMetricsAsObject;

        Boolean bReturn = TryCalculateGraphMetricsCore(graph, backgroundWorker,
            out oGraphMetricsAsObject);

        allPairsPathLengths = ( UInt16 [,] )oGraphMetricsAsObject;

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
    /// notes for the details on the type.
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

        // This implementation is based on code from Janez Brank's May 2009
        // implementation of the Harel-Koren fast multiscale layout algorithm.

        IVertexCollection oVertices = oGraph.Vertices;
        Int32 iVertices = oVertices.Count;
        UInt16 [,] aui16AllPairsPathLengths = null;

        try
        {
            aui16AllPairsPathLengths = new UInt16[iVertices, iVertices];
        }
        catch (OutOfMemoryException)
        {
            ReportCannotCalculateGraphMetrics( String.Format(

                "Calculating {0} requires a large amount of computer memory,"
                + " and there is not enough memory available."
                ,
                this.GraphMetricDescription
                ) );
        }

        oGraphMetrics = aui16AllPairsPathLengths;

        // The key is a vertex and the value is the vertex's zero-based index
        // when enumerating the vertex collection.

        Dictionary<IVertex, Int32> oVertexIndexes =
            new Dictionary<IVertex, Int32>(iVertices);

        Int32 s = 0;

        foreach (IVertex oVertexS in oVertices)
        {
            oVertexIndexes.Add(oVertexS, s);
            s++;
        }

        Queue<IVertex> oQueue = new Queue<IVertex>();
        s = 0;

        foreach (IVertex oVertexS in oVertices)
        {
            for (Int32 v = 0; v < iVertices; v++)
            {
                aui16AllPairsPathLengths[s, v] = NoPath;
            }

            oQueue.Enqueue(oVertexS);
            aui16AllPairsPathLengths[s, s] = 0;

            while (oQueue.Count > 0)
            {
                IVertex oVertexU = oQueue.Dequeue();

                UInt16 ui16SUPathLength =
                    aui16AllPairsPathLengths[ s, oVertexIndexes[oVertexU] ];

                foreach (IVertex oAdjacentVertex in oVertexU.AdjacentVertices)
                {
                    Int32 iAdjacentVertexIndex =
                        oVertexIndexes[oAdjacentVertex];

                    if (aui16AllPairsPathLengths[s, iAdjacentVertexIndex]
                        == NoPath)
                    {
                        // The adjacent vertex hasn't been visited yet.

                        aui16AllPairsPathLengths[s, iAdjacentVertexIndex] =
                            (UInt16)(ui16SUPathLength + 1);

                        oQueue.Enqueue(oAdjacentVertex);
                    }
                }
            }

            if (oBackgroundWorker != null)
            {
                if (oBackgroundWorker.CancellationPending)
                {
                    return (false);
                }

                ReportProgress(s, iVertices, oBackgroundWorker);
            }

            s++;
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

        Debug.Assert( !String.IsNullOrEmpty(m_sGraphMetricDescription) );
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// Value used to indicate that there is no path between two vertices.

    public const UInt16 NoPath = UInt16.MaxValue;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Graph metric description.

    protected String m_sGraphMetricDescription;
}

}
