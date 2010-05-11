
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.Research.CommunityTechnologies.AppLib;

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
    /// Static constructor for the <see cref="GraphMetricCalculatorBase" />
    /// class.
    /// </summary>
    //*************************************************************************

    static GraphMetricCalculatorBase()
    {
        m_sSnapGraphMetricCalculatorPath = null;
    }

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
    //  Method: SetSnapGraphMetricCalculatorPath()
    //
    /// <summary>
    /// Sets the path to the executable that calculates graph metrics using
    /// the SNAP library.
    /// </summary>
    ///
    /// <param name="snapGraphMetricCalculatorPath">
    /// Full path to the executable.  Sample:
    /// "C:\MyProgram\SnapGraphMetricCalculator.exe".
    /// </param>
    ///
    /// <remarks>
    /// Some of the derived classes use a separate executable to do their graph
    /// metric calculations.  This executable, which is custom built for NodeXL
    /// and is provided with the NodeXL source code, uses the SNAP library
    /// created by Jure Leskovec at Stanford.  The SNAP code is written in C++
    /// and is optimized for speed and scalability, so it can calculate certain
    /// graph much faster than could be done in C# code.
    ///
    /// <para>
    /// By default, the NodeXL build process copies the executable, which is
    /// named SnapGraphMetricCalculator.exe, to the Algorithm project's output
    /// directory, which is either bin\Debug or bin\Release.  Also by default,
    /// derived classes look for this executable in the same folder as the
    /// Algorithm assembly.  That means that for many projects, the executable
    /// will be automatically found and this method does not need to be called.
    /// </para>
    ///
    /// <para>
    /// If your application's deployment places the executable somewhere else,
    /// however, you must call this static method once to provide the derived
    /// classes with the executable's path.
    /// </para>
    ///
    /// <para>
    /// A future release may wrap the SNAP library in an interop DLL,
    /// eliminating the need for a separate executable.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    static public void
    SetSnapGraphMetricCalculatorPath
    (
        String snapGraphMetricCalculatorPath
    )
    {
        m_sSnapGraphMetricCalculatorPath = snapGraphMetricCalculatorPath;
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
    /// second argument is a string in the format "Computing
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

            "Computing {0}."
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
    //  Enum: SnapGraphMetrics
    //
    /// <summary>
    /// Specifies one or more graph metrics that can be calculated by the SNAP
    /// library.
    /// </summary>
    ///
    /// <remarks>
    /// See the GraphMetrics enumeration in the Snap/GraphMetricCalculator
    /// project for details on how graph metrics must be specified.
    /// </remarks>
    //*************************************************************************

    [System.FlagsAttribute]

    protected enum SnapGraphMetrics
    {
        /// <summary>
        /// Closeness centrality.  Type: Per-vertex.
        /// </summary>

        ClosenessCentrality = 1,

        /// <summary>
        /// Betweenness centrality.  Type: Per-vertex.
        /// </summary>

        BetweennessCentrality = 2,

        /// <summary>
        /// Eigenvector centrality.  Type: Per-vertex.
        /// </summary>

        EigenvectorCentrality = 4,

        /// <summary>
        /// PageRank.  Type: Per-vertex.
        /// </summary>

        PageRank = 8,

        /// <summary>
        /// Maximum geodesic distance, also known as graph diameter, and
        /// average geodesic distance.  Type: Per-graph.
        /// </summary>

        GeodesicDistances = 16,

        /// <summary>
        /// Clusters using the Girvan-Newman algorithm.  Type: Clusters.
        /// </summary>

        GirvanNewmanClusters = 32,

        /// <summary>
        /// Clusters using the Clauset-Newman-Moore algorithm.  Type: Clusters.
        /// </summary>
    
        ClausetNewmanMooreClusters = 64,

        /// <summary>
        /// No graph metrics.
        /// </summary>

        None = 0,
    };

    //*************************************************************************
    //  Method: CalculateSnapGraphMetrics()
    //
    /// <summary>
    /// Calculates one or more graph metrics using the SNAP executable.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <param name="eSnapGraphMetrics">
    /// One or more graph metrics to calculate.
    /// </param>
    ///
    /// <returns>
    /// Full path to a temporary output file containing the calculated metrics.
    /// The caller must delete this file after it is done with it.
    /// </returns>
    ///
    /// <remarks>
    /// If an error occurs while calling the executable, an IOException is
    /// thrown.
    /// </remarks>
    //*************************************************************************

    protected String
    CalculateSnapGraphMetrics
    (
        IGraph oGraph,
        SnapGraphMetrics eSnapGraphMetrics
    )
    {
        AssertValid();

        String sInputFilePath = null;
        String sOutputFilePath = null;

        try
        {
            // The SNAP executable expects an input text file that contains one
            // line per edge.  The line contains a tab-separated pair of
            // integer vertex IDs.  Create the file.

            sInputFilePath = Path.GetTempFileName();

            using ( StreamWriter oStreamWriter = new StreamWriter(
                sInputFilePath) ) 
            {
                foreach (IEdge oEdge in oGraph.Edges)
                {
                    IVertex [] oVertices = oEdge.Vertices;

                    oStreamWriter.WriteLine(
                        "{0}\t{1}"
                        ,
                        oVertices[0].ID,
                        oVertices[1].ID
                        );
                }
            }

            // SNAP will overwrite this output file, which initially has zero
            // length:

            sOutputFilePath = Path.GetTempFileName();

            // The arguments should look like this:
            //
            // InputFilePath IsDirected GraphMetricsToCalculate OutputFilePath

            String sArguments = String.Format(
                "\"{0}\" {1} {2} \"{3}\""
                ,
                sInputFilePath,

                oGraph.Directedness == GraphDirectedness.Directed ?
                    "true" : "false",

                (Int32)eSnapGraphMetrics,
                sOutputFilePath
                );

            String sStandardError;

            if ( !TryCallSnapGraphMetricCalculator(sArguments,
                out sStandardError) )
            {
                throw new IOException(
                    "A problem occurred while calling the executable that"
                    + " calculates SNAP graph metrics.  Details: "
                    + sStandardError
                    );
            }

            Debug.Assert( File.Exists(sOutputFilePath) );
        }
        catch
        {
            // Delete the output file on error.

            if ( sOutputFilePath != null && File.Exists(sOutputFilePath) )
            {
                File.Delete(sOutputFilePath);
            }

            throw;
        }
        finally
        {
            // Always delete the input file.

            if ( sInputFilePath != null && File.Exists(sInputFilePath) )
            {
                File.Delete(sInputFilePath);
            }
        }

        return (sOutputFilePath);
    }

    //*************************************************************************
    //  Method: ParseSnapInt32GraphMetricValue()
    //
    /// <summary>
    /// Parses an Int32 graph metric value read from the output file created by
    /// the SNAP command-line executable.
    /// </summary>
    ///
    /// <param name="asFieldsFromSnapOutputFileLine">
    /// Fields read from one line of the output file created by the SNAP
    /// command-line executable.
    /// </param>
    ///
    /// <param name="iFieldIndex">
    /// The zero-based index of the field to parse.
    /// </param>
    ///
    /// <returns>
    /// The parsed Int32 graph metric value.
    /// </returns>
    //*************************************************************************

    protected Int32
    ParseSnapInt32GraphMetricValue
    (
        String [] asFieldsFromSnapOutputFileLine,
        Int32 iFieldIndex
    )
    {
        Debug.Assert(asFieldsFromSnapOutputFileLine != null);
        Debug.Assert(iFieldIndex >= 0);
        Debug.Assert(iFieldIndex < asFieldsFromSnapOutputFileLine.Length);
        AssertValid();

        Int32 iGraphMetricValue;

        if ( !MathUtil.TryParseCultureInvariantInt32(
            asFieldsFromSnapOutputFileLine[iFieldIndex], out iGraphMetricValue)
            )
        {
            throw new FormatException(
                "A field read from the SNAP output file is not an Int32."
                );
        }

        return (iGraphMetricValue);
    }

    //*************************************************************************
    //  Method: ParseSnapDoubleGraphMetricValue()
    //
    /// <summary>
    /// Parses a Double graph metric value read from the output file created by
    /// the SNAP command-line executable.
    /// </summary>
    ///
    /// <param name="asFieldsFromSnapOutputFileLine">
    /// Fields read from one line of the output file created by the SNAP
    /// command-line executable.
    /// </param>
    ///
    /// <param name="iFieldIndex">
    /// The zero-based index of the field to parse.
    /// </param>
    ///
    /// <returns>
    /// The parsed Double graph metric value.
    /// </returns>
    //*************************************************************************

    protected Double
    ParseSnapDoubleGraphMetricValue
    (
        String [] asFieldsFromSnapOutputFileLine,
        Int32 iFieldIndex
    )
    {
        Debug.Assert(asFieldsFromSnapOutputFileLine != null);
        Debug.Assert(iFieldIndex >= 0);
        Debug.Assert(iFieldIndex < asFieldsFromSnapOutputFileLine.Length);
        AssertValid();

        Double dGraphMetricValue;

        if ( !MathUtil.TryParseCultureInvariantDouble(
            asFieldsFromSnapOutputFileLine[iFieldIndex], out dGraphMetricValue)
            )
        {
            throw new FormatException(
                "A field read from the SNAP output file is not a Double."
                );
        }

        return (dGraphMetricValue);
    }

    //*************************************************************************
    //  Method: TryCallSnapGraphMetricCalculator()
    //
    /// <summary>
    /// Calls the SNAP command-line executable.
    /// </summary>
    ///
    /// <param name="sArguments">
    /// Command line arguments.
    /// </param>
    ///
    /// <param name="sStandardError">
    /// Where the standard error string gets stored if false is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the executable was successfully called, false if the executable
    /// reported an error via the StandardError stream.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryCallSnapGraphMetricCalculator
    (
        String sArguments,
        out String sStandardError
    )
    {
        String sSnapGraphMetricCalculatorPath =
            m_sSnapGraphMetricCalculatorPath;

        if (sSnapGraphMetricCalculatorPath == null)
        {
            // SetSnapGraphMetricCalculatorPath() hasn't been called.  Use a
            // default path.

            sSnapGraphMetricCalculatorPath = Path.Combine(

                Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().Location),

                DefaultSnapGraphMetricCalculatorFileName
                );
        }

        if ( !File.Exists(sSnapGraphMetricCalculatorPath) )
        {
            throw new IOException(
                "The executable that calculates SNAP graph metrics can't be"
                + " found.  See the comments for the GraphMetricCalculatorBase"
                + ".SetSnapGraphMetricCalculatorPath() method."
                );
        }

        ProcessStartInfo oProcessStartInfo = new ProcessStartInfo(
            sSnapGraphMetricCalculatorPath, sArguments);

        oProcessStartInfo.UseShellExecute = false;
        oProcessStartInfo.RedirectStandardError = true;
        oProcessStartInfo.CreateNoWindow = true;

        Process oProcess = new Process();
        oProcess.StartInfo = oProcessStartInfo;
        oProcess.Start();
        sStandardError = oProcess.StandardError.ReadToEnd();
        oProcess.WaitForExit();

        return (sStandardError.Length == 0);
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
    //  Protected constants
    //*************************************************************************

    /// Default file name of the executable that computes graph metrics using
    /// the SNAP library.  Not used if m_sSnapGraphMetricCalculatorPath is set.

    protected const String DefaultSnapGraphMetricCalculatorFileName =
        "SnapGraphMetricCalculator.exe";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)


    //*************************************************************************
    //  Private fields
    //*************************************************************************

    /// Path to the executable that computes graph metrics using the SNAP
    /// library, or null to use a default path.

    private static String m_sSnapGraphMetricCalculatorPath;
}

}
