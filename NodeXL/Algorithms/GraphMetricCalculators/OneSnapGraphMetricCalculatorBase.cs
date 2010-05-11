
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: OneSnapGraphMetricCalculatorBase
//
/// <summary>
/// Calculates one per-vertex graph metric using the SNAP graph library.
/// </summary>
///
/// <remarks>
/// This is a base class for several classes that use the SNAP graph library
/// to calculate one per-vertex graph metric in the <see
/// cref="GraphMetricCalculatorBase.SnapGraphMetrics" /> enumeration.
/// "Per-vertex" means that a metric is computed for each vertex in the graph,
/// as opposed to "per-graph," for which there is a single metric value for the
/// entire graph.
///
/// <para>
/// The metric values are provided as a Dictionary&lt;Int32, Double&gt;.  There
/// is one key/value pair for each vertex in the graph.  The key is the
/// IVertex.ID and the value is the vertex's metric, as a Double.
/// </para>
///
/// <para>
/// If a vertex is isolated, its metric value is zero.
/// </para>
///
/// </remarks>
//*****************************************************************************

public abstract class OneSnapGraphMetricCalculatorBase :
    GraphMetricCalculatorBase
{
    //*************************************************************************
    //  Constructor: OneSnapGraphMetricCalculatorBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="OneSnapGraphMetricCalculatorBase" /> class.
    /// </summary>
    ///
    /// <param name="snapGraphMetric">
    /// The graph metric to calculate.  Must be just one value in the <see
    /// cref="GraphMetricCalculatorBase.SnapGraphMetrics" /> enumeration.
    /// </param>
    ///
    /// <param name="graphMetricDescription">
    /// A description suitable for use within the sentence "Calculating
    /// [GraphMetricDescription]."
    /// </param>
    ///
    /// <param name="expectedHeaderLineInOutputFile">
    /// The expected header line in the output file created by SNAP.  This is
    /// used for diagnostic purposes.
    /// </param>
    //*************************************************************************

    protected OneSnapGraphMetricCalculatorBase
    (
        SnapGraphMetrics snapGraphMetric,
        String graphMetricDescription,
        String expectedHeaderLineInOutputFile
    )
    {
        m_eSnapGraphMetric = snapGraphMetric;
        m_sGraphMetricDescription = graphMetricDescription;
        m_sExpectedHeaderLineInOutputFile = expectedHeaderLineInOutputFile;

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
    /// <param name="graphMetrics">
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
        out Dictionary<Int32, Double> graphMetrics
    )
    {
        Debug.Assert(graph != null);

        Object oGraphMetricsAsObject;

        Boolean bReturn = TryCalculateGraphMetricsCore(graph, backgroundWorker,
            out oGraphMetricsAsObject);

        graphMetrics = ( Dictionary<Int32, Double> )oGraphMetricsAsObject;

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

        Stopwatch oStopwatch = Stopwatch.StartNew();

        IVertexCollection oVertices = oGraph.Vertices;

        Dictionary<Int32, Double> oGraphMetricDictionary =
            new Dictionary<Int32, Double>(oVertices.Count);

        oGraphMetrics = oGraphMetricDictionary;

        if (oBackgroundWorker != null)
        {
            if (oBackgroundWorker.CancellationPending)
            {
                return (false);
            }

            ReportProgress(1, 3, oBackgroundWorker);
        }

        // The code below doesn't calculate metric values for isolates, so
        // start with zero for each vertex.  Values for non-isolate vertices
        // will get overwritten later.

        foreach (IVertex oVertex in oVertices)
        {
            oGraphMetricDictionary.Add(oVertex.ID, 0);
        }

        String sOutputFilePath = CalculateSnapGraphMetrics(oGraph,
            m_eSnapGraphMetric);

        if (oBackgroundWorker != null)
        {
            ReportProgress(2, 3, oBackgroundWorker);
        }

        using ( StreamReader oStreamReader = new StreamReader(
            sOutputFilePath) ) 
        {
            // The first line is a header.

            String sLine = oStreamReader.ReadLine();

            Debug.Assert(sLine == m_sExpectedHeaderLineInOutputFile);

            // The remaining lines are the metric values, one line per vertex.

            while (oStreamReader.Peek() >= 0)
            {
                // The line has two fields: The vertex ID, and the metric
                // value.

                sLine = oStreamReader.ReadLine();
                String [] asFields = sLine.Split('\t');
                Debug.Assert(asFields.Length == 2);

                oGraphMetricDictionary[
                    ParseSnapInt32GraphMetricValue(asFields, 0) ] = 
                    ParseSnapDoubleGraphMetricValue(asFields, 1);
            }
        }

        File.Delete(sOutputFilePath);

        Debug.WriteLine( String.Format(
            "Time spent calculating {0} using the SNAP library: {1} ms."
            ,
            this.GraphMetricDescription,
            oStopwatch.ElapsedMilliseconds
            ) );

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

        // m_eSnapGraphMetric

        Debug.Assert( !String.IsNullOrEmpty(m_sGraphMetricDescription) );

        Debug.Assert( !String.IsNullOrEmpty(
            m_sExpectedHeaderLineInOutputFile) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The graph metric to calculate.

    protected SnapGraphMetrics m_eSnapGraphMetric;

    /// A description of the graph metrics calculated by the derived class.

    protected String m_sGraphMetricDescription;

    /// The expected header line in the output file created by SNAP.

    protected String m_sExpectedHeaderLineInOutputFile;
}

}
