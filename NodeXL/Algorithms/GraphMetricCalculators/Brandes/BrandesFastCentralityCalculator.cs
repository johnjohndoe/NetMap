
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
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
/// If a vertex is isolated, its betweenness and closeness centralities are
/// zero.
///
/// <para>
/// In a previous version, the centralities were calculated simultaneously by
/// this class using the algorithm described in the paper "A Faster Algorithm
/// for Betweenness Centrality," by Ulrik Brandes.  The paper can be found
/// here:
/// </para>
///
/// <para>
/// http://www.inf.uni-konstanz.de/algo/publications/b-fabc-01.pdf
/// </para>
///
/// <para>
/// Section 4 of the paper explains how other centrality measures can be
/// computed within the same algorithm at little additional cost.  That is why
/// closeness centrality was calculated by this class along with betweenness.
/// </para>
///
/// <para>
/// Starting with version 1.0.1.122 of NodeXL, the centralities are calculated
/// using the SNAP graph library, which is much faster than the previous NodeXL
/// code.  SNAP calculates betweenness and closeness centralities separately,
/// so it might make sense to split this class in two in a future release.  Or
/// would it make more sense to modify SNAP to calculate the two
/// simultaneously, as suggested by Brandes?
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
    /// graph.  The key is the IVertex.ID and the value is a <see
    /// cref="BrandesVertexCentralities" /> object.
    /// </returns>
    //*************************************************************************

    public Dictionary<Int32, BrandesVertexCentralities>
    CalculateGraphMetrics
    (
        IGraph graph
    )
    {
        Debug.Assert(graph != null);
        AssertValid();

        Dictionary<Int32, BrandesVertexCentralities> oGraphMetrics;

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
    /// and the value is a <see cref="BrandesVertexCentralities" /> object.
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
        out Dictionary<Int32, BrandesVertexCentralities> graphMetrics
    )
    {
        Debug.Assert(graph != null);
        AssertValid();

        Stopwatch oStopwatch = Stopwatch.StartNew();

        IVertexCollection oVertices = graph.Vertices;

        // The key is an IVertex.ID and the value is the corresponding
        // BrandesVertexCentralities object.

        Dictionary<Int32, BrandesVertexCentralities>
            oBrandesVertexCentralities =
            new Dictionary<Int32, BrandesVertexCentralities>(oVertices.Count);

        graphMetrics = oBrandesVertexCentralities;

        // The code below doesn't calculate metric values for isolates, so
        // start with zero for each vertex.  Values for non-isolate vertices
        // will get overwritten later.

        foreach (IVertex oVertex in oVertices)
        {
            oBrandesVertexCentralities.Add(
                oVertex.ID, new BrandesVertexCentralities() );
        }

        if (backgroundWorker != null)
        {
            if (backgroundWorker.CancellationPending)
            {
                return (false);
            }

            ReportProgress(1, 3, backgroundWorker);
        }

        String sOutputFilePath = CalculateSnapGraphMetrics(graph,
            SnapGraphMetrics.ClosenessCentrality |
            SnapGraphMetrics.BetweennessCentrality
            );

        if (backgroundWorker != null)
        {
            ReportProgress(2, 3, backgroundWorker);
        }

        using ( StreamReader oStreamReader = new StreamReader(
            sOutputFilePath) ) 
        {
            // The first line is a header.

            String sLine = oStreamReader.ReadLine();

            Debug.Assert(sLine ==
                "Vertex ID\tCloseness Centrality\tBetweenness Centrality");

            // The remaining lines are the centralities, one line per vertex.

            while (oStreamReader.Peek() >= 0)
            {
                // The line has three fields: The vertex ID, the closeness
                // centrality, and the betweenness centrality.

                sLine = oStreamReader.ReadLine();
                String [] asFields = sLine.Split('\t');
                Debug.Assert(asFields.Length == 3);

                BrandesVertexCentralities oCentralities =
                    oBrandesVertexCentralities[
                        ParseSnapInt32GraphMetricValue(asFields, 0) ];

                oCentralities.ClosenessCentrality =
                    ParseSnapDoubleGraphMetricValue(asFields, 1);

                oCentralities.BetweennessCentrality =
                    ParseSnapDoubleGraphMetricValue(asFields, 2);
            }
        }

        File.Delete(sOutputFilePath);

        Debug.WriteLine(
            "Time spent calculating closeness and betweenness centralities"
            + " using the SNAP library, in ms: " +
            oStopwatch.ElapsedMilliseconds);

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
