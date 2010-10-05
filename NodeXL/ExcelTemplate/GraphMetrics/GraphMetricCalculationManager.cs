
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Algorithms;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphMetricCalculationManager
//
/// <summary>
/// Calculates one or more sets of graph metrics and returns the results as an
/// array of <see cref="GraphMetricColumn" /> objects.
/// </summary>
///
/// <remarks>
/// Call <see cref="CalculateGraphMetricsAsync(
/// Microsoft.Office.Interop.Excel.Workbook, GraphMetricUserSettings)" /> to
/// calculate the graph metrics.  Call <see cref="CancelAsync" /> to stop the
/// calculations.  Handle the <see
/// cref="GraphMetricCalculationProgressChanged" /> and <see
/// cref="GraphMetricCalculationCompleted" /> events to monitor the progress
/// and completion of the calculations.
/// </remarks>
//*****************************************************************************

public class GraphMetricCalculationManager : Object
{
    //*************************************************************************
    //  Constructor: GraphMetricCalculationManager()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GraphMetricCalculationManager" /> class.
    /// </summary>
    //*************************************************************************

    public GraphMetricCalculationManager()
    {
        m_oBackgroundWorker = null;

        // Set the path to the executable that calculates graph metrics using
        // the SNAP library.

        GraphMetricCalculatorBase.SetSnapGraphMetricCalculatorPath(
            Path.Combine(ApplicationUtil.GetApplicationFolder(),
                SnapGraphMetricCalculatorFileName) );

        AssertValid();
    }

    //*************************************************************************
    //  Property: IsBusy
    //
    /// <summary>
    /// Gets a flag indicating whether an asynchronous operation is in
    /// progress.
    /// </summary>
    ///
    /// <value>
    /// true if an asynchronous operation is in progress.
    /// </value>
    //*************************************************************************

    public Boolean
    IsBusy
    {
        get
        {
            return (m_oBackgroundWorker != null && m_oBackgroundWorker.IsBusy);
        }
    }

    //*************************************************************************
    //  Method: CalculateGraphMetricsAsync()
    //
    /// <overloads>
    /// Asynchronously calculates one or more sets of graph metrics and returns
    /// the results as an array of <see cref="GraphMetricColumn" /> objects.
    /// </overloads>
    ///
    /// <summary>
    /// Asynchronously calculates one or more sets of a default list of graph
    /// metrics and returns the results as an array of <see
    /// cref="GraphMetricColumn" /> objects.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph contents.
    /// </param>
    ///
    /// <param name="graphMetricUserSettings">
    /// User settings for calculating graph metrics.
    /// </param>
    ///
    /// <remarks>
    /// (See the other overload for more details.)
    /// </remarks>
    //*************************************************************************

    public void
    CalculateGraphMetricsAsync
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        GraphMetricUserSettings graphMetricUserSettings
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert(graphMetricUserSettings != null);
        AssertValid();

        // Create the default list of graph metrics.

        IGraphMetricCalculator2[] aoAllGraphMetricCalculators =
            new IGraphMetricCalculator2[] {

                new VertexDegreeCalculator2(),
                new BrandesFastCentralityCalculator2(),
                new EigenvectorCentralityCalculator2(),
                new PageRankCalculator2(),
                new ClusteringCoefficientCalculator2(),
                new OverallMetricCalculator2(),
                new GroupMetricCalculator2(),
                };

        this.CalculateGraphMetricsAsync(
            workbook, aoAllGraphMetricCalculators, graphMetricUserSettings);
    }

    //*************************************************************************
    //  Method: CalculateGraphMetricsAsync()
    //
    /// <summary>
    /// Asynchronously calculates one or more sets of specified graph metrics
    /// and returns the results as an array of <see
    /// cref="GraphMetricColumn" /> objects.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph contents.
    /// </param>
    ///
    /// <param name="graphMetricCalculators">
    /// An array of <see cref="IGraphMetricCalculator2" /> implementations, one
    /// for each set of graph metrics that should be calculated.  This method
    /// sorts the array in place, so its contents will likely be in a different
    /// order when the method returns.
    /// </param>
    ///
    /// <param name="graphMetricUserSettings">
    /// User settings for calculating graph metrics.
    /// </param>
    ///
    /// <remarks>
    /// For each <see cref="IGraphMetricCalculator2" /> implementation in the
    /// <paramref name="graphMetricCalculators" /> array, this method calls the
    /// implementation's <see
    /// cref="IGraphMetricCalculator2.TryCalculateGraphMetrics" /> method.  The
    /// <see cref="GraphMetricColumn" /> objects returned by each
    /// implementation are aggregated.  When graph metric calculations
    /// complete, the <see cref="GraphMetricCalculationCompleted" /> event
    /// fires and the aggregated results can be obtained via the <see
    /// cref="RunWorkerCompletedEventArgs.Result" /> property.
    ///
    /// <para>
    /// To cancel the calculations, call <see cref="CancelAsync" />.
    /// </para>
    ///
    /// <para>
    /// If <paramref name="workbook" /> contains invalid graph data, a <see
    /// cref="WorkbookFormatException" /> is thrown on the caller's thread
    /// before asynchronous calculations begin.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    CalculateGraphMetricsAsync
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        IGraphMetricCalculator2 [] graphMetricCalculators,
        GraphMetricUserSettings graphMetricUserSettings
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert(graphMetricCalculators != null);
        Debug.Assert(graphMetricUserSettings != null);
        AssertValid();

        const String MethodName = "CalculateGraphMetricsAsync";

        if (this.IsBusy)
        {
            throw new InvalidOperationException( String.Format(

                "{0}:{1}: An asynchronous operation is already in progress."
                ,
                this.ClassName,
                MethodName
                ) );
        }

        // Read the workbook into a graph.  Do this from the calling thread to
        // avoid reading the Excel UI from a background thread.

        IGraph oGraph = ReadWorkbook(workbook, graphMetricUserSettings);

        // Sort the calculators so that those that can handle duplicate edges
        // are grouped at the beginning of the array.  When the workbook is
        // read into a graph, duplicate edges are included and that graph is
        // passed to the first group of calculators.  When the second group is
        // reached -- those that cannot handle duplicate edges -- the
        // duplicates are removed from the graph before passing the graph to
        // the second group.

        Array.Sort(graphMetricCalculators,
            (x, y) =>
                y.HandlesDuplicateEdges.CompareTo(x.HandlesDuplicateEdges)
            );

        // Wrap the arguments in an object that can be passed to
        // BackgroundWorker.RunWorkerAsync().

        CalculateGraphMetricsAsyncArgs oCalculateGraphMetricsAsyncArgs =
            new CalculateGraphMetricsAsyncArgs();

        oCalculateGraphMetricsAsyncArgs.Graph = oGraph;

        oCalculateGraphMetricsAsyncArgs.SortedGraphMetricCalculators =
            graphMetricCalculators;

        oCalculateGraphMetricsAsyncArgs.GraphMetricUserSettings =
            graphMetricUserSettings;

        // Create a BackgroundWorker and handle its events.

        m_oBackgroundWorker = new BackgroundWorker();

        m_oBackgroundWorker.WorkerReportsProgress = true;
        m_oBackgroundWorker.WorkerSupportsCancellation = true;

        m_oBackgroundWorker.DoWork += new DoWorkEventHandler(
            BackgroundWorker_DoWork);

        m_oBackgroundWorker.ProgressChanged +=
            new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);

        m_oBackgroundWorker.RunWorkerCompleted +=
            new RunWorkerCompletedEventHandler(
                BackgroundWorker_RunWorkerCompleted);

        m_oBackgroundWorker.RunWorkerAsync(oCalculateGraphMetricsAsyncArgs);
    }

    //*************************************************************************
    //  Method: CancelAsync()
    //
    /// <summary>
    /// Cancels the graph metric calculations started by <see
    /// cref="CalculateGraphMetricsAsync(
    /// Microsoft.Office.Interop.Excel.Workbook, GraphMetricUserSettings)" />.
    /// </summary>
    ///
    /// <remarks>
    /// When the calculations cancels, the <see
    /// cref="GraphMetricCalculationCompleted" /> event fires.  The <see
    /// cref="AsyncCompletedEventArgs.Cancelled" /> property will be true.
    /// </remarks>
    //*************************************************************************

    public void
    CancelAsync()
    {
        AssertValid();

        if (this.IsBusy)
        {
            m_oBackgroundWorker.CancelAsync();
        }
    }

    //*************************************************************************
    //  Event: GraphMetricCalculationProgressChanged
    //
    /// <summary>
    /// Occurs when progress is made during the graph metric calculations
    /// started by <see cref="CalculateGraphMetricsAsync(
    /// Microsoft.Office.Interop.Excel.Workbook, GraphMetricUserSettings)" />.
    /// </summary>
    ///
    /// <remarks>
    /// The <see cref="ProgressChangedEventArgs.UserState" /> argument is a
    /// string describing the progress.  The string is suitable for display to
    /// the user.
    /// </remarks>
    //*************************************************************************

    public event ProgressChangedEventHandler
        GraphMetricCalculationProgressChanged;


    //*************************************************************************
    //  Event: GraphMetricCalculationCompleted
    //
    /// <summary>
    /// Occurs when the graph metric calculations started by <see
    /// cref="CalculateGraphMetricsAsync(
    /// Microsoft.Office.Interop.Excel.Workbook, GraphMetricUserSettings)" />
    /// complete, are cancelled, or encounter an error.
    /// </summary>
    ///
    /// <remarks>
    /// If graph metric calculations complete successfully, the <see
    /// cref="RunWorkerCompletedEventArgs.Result" /> argument is an array of
    /// <see cref="GraphMetricColumn" /> objects, one for each column of
    /// metrics that were calculated.
    /// </remarks>
    //*************************************************************************

    public event RunWorkerCompletedEventHandler
        GraphMetricCalculationCompleted;


    //*************************************************************************
    //  Property: ClassName
    //
    /// <summary>
    /// Gets the full name of this class.
    /// </summary>
    ///
    /// <value>
    /// The full name of this class, suitable for use in error messages.
    /// </value>
    //*************************************************************************

    protected String
    ClassName
    {
        get
        {
            return (this.GetType().FullName);
        }
    }

    //*************************************************************************
    //  Method: ReadWorkbook()
    //
    /// <summary>
    /// Reads the workbook contents into a NodeXL graph.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the graph contents.
    /// </param>
    ///
    /// <param name="oGraphMetricUserSettings">
    /// User settings for calculating graph metrics.
    /// </param>
    ///
    /// <returns>
    /// The <see cref="IGraph" /> read from the workbook.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="oWorkbook" /> contains valid graph data, a new <see
    /// cref="IGraph" /> is created from the workbook contents and returned.
    /// Otherwise, a <see cref="WorkbookFormatException" /> is thrown.
    /// </remarks>
    //*************************************************************************

    protected IGraph
    ReadWorkbook
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        GraphMetricUserSettings oGraphMetricUserSettings
    )
    {
        Debug.Assert(oWorkbook != null);
        Debug.Assert(oGraphMetricUserSettings != null);
        AssertValid();

        ReadWorkbookContext oReadWorkbookContext = new ReadWorkbookContext();
        oReadWorkbookContext.FillIDColumns = true;
        oReadWorkbookContext.PopulateVertexWorksheet = true;

        if ( (oGraphMetricUserSettings.GraphMetricsToCalculate &
            GraphMetrics.GroupMetrics) != 0)
        {
            oReadWorkbookContext.ReadGroups = true;
            oReadWorkbookContext.SaveGroupVertices = true;
        }

        WorkbookReader oWorkbookReader = new WorkbookReader();

        return ( oWorkbookReader.ReadWorkbook(
            oWorkbook, oReadWorkbookContext) );
    }

    //*************************************************************************
    //  Method: CalculateGraphMetricsAsyncInternal()
    //
    /// <summary>
    /// Calculates one or more sets of graph metrics and stores the results in
    /// one or more worksheet columns.
    /// </summary>
    ///
    /// <param name="oCalculateGraphMetricsAsyncArgs">
    /// Contains the arguments needed to asynchronously calculate graph
    /// metrics.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// A BackgroundWorker object.
    /// </param>
    ///
    /// <param name="oDoWorkEventArgs">
    /// A DoWorkEventArgs object.
    /// </param>
    //*************************************************************************

    protected void
    CalculateGraphMetricsAsyncInternal
    (
        CalculateGraphMetricsAsyncArgs oCalculateGraphMetricsAsyncArgs,
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs
    )
    {
        Debug.Assert(oCalculateGraphMetricsAsyncArgs != null);
        Debug.Assert(oBackgroundWorker != null);
        Debug.Assert(oDoWorkEventArgs != null);
        AssertValid();

        IGraph oGraph = oCalculateGraphMetricsAsyncArgs.Graph;

        List<GraphMetricColumn> oAggregatedGraphMetricColumns =
            new List<GraphMetricColumn>();

        CalculateGraphMetricsContext oCalculateGraphMetricsContext =
            new CalculateGraphMetricsContext(
                oCalculateGraphMetricsAsyncArgs.GraphMetricUserSettings,
                oBackgroundWorker);

        Boolean bDuplicateEdgesRemoved = false;

        foreach (IGraphMetricCalculator2 oGraphMetricCalculator in
            oCalculateGraphMetricsAsyncArgs.SortedGraphMetricCalculators)
        {
            if (!oGraphMetricCalculator.HandlesDuplicateEdges &&
                !bDuplicateEdgesRemoved)
            {
                // This and the remainder of the graph metric calculators
                // cannot handle duplicate edges.  Remove them from the graph.

                oGraph.Edges.RemoveDuplicates();
                bDuplicateEdgesRemoved = true;
            }

            // Calculate the implementation's graph metrics.

            GraphMetricColumn [] aoGraphMetricColumns;

            if ( !oGraphMetricCalculator.TryCalculateGraphMetrics(oGraph,
                oCalculateGraphMetricsContext, out aoGraphMetricColumns) )
            {
                // The user cancelled.

                oDoWorkEventArgs.Cancel = true;

                oBackgroundWorker.ReportProgress(0,
                    new GraphMetricProgress("Cancelled.", false)
                    );

                return;
            }

            // Aggregate the results.

            Debug.Assert(aoGraphMetricColumns != null);

            oAggregatedGraphMetricColumns.AddRange(aoGraphMetricColumns);
        }

        oDoWorkEventArgs.Result = oAggregatedGraphMetricColumns.ToArray();

        oBackgroundWorker.ReportProgress(100,
            new GraphMetricProgress(
                "Inserting metrics into the workbook.",
                true)
            );

        // Let the dialog the display the final progress report and update its
        // controls.

        System.Threading.Thread.Sleep(1);
    }

    //*************************************************************************
    //  Method: BackgroundWorker_DoWork()
    //
    /// <summary>
    /// Handles the DoWork event on the BackgroundWorker object.
    /// </summary>
    ///
    /// <param name="sender">
    /// Source of the event.
    /// </param>
    ///
    /// <param name="e">
    /// Standard mouse event arguments.
    /// </param>
    //*************************************************************************

    protected void
    BackgroundWorker_DoWork
    (
        object sender,
        DoWorkEventArgs e
    )
    {
        Debug.Assert(sender is BackgroundWorker);
        AssertValid();

        BackgroundWorker oBackgroundWorker = (BackgroundWorker)sender;

        Debug.Assert(e.Argument is CalculateGraphMetricsAsyncArgs);

        CalculateGraphMetricsAsyncArgs oCalculateGraphMetricsAsyncArgs =
            (CalculateGraphMetricsAsyncArgs)e.Argument;

        CalculateGraphMetricsAsyncInternal(oCalculateGraphMetricsAsyncArgs,
            m_oBackgroundWorker, e);
    }

    //*************************************************************************
    //  Method: BackgroundWorker_ProgressChanged()
    //
    /// <summary>
    /// Handles the ProgressChanged event on the BackgroundWorker object.
    /// </summary>
    ///
    /// <param name="sender">
    /// Source of the event.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event arguments.
    /// </param>
    //*************************************************************************

    protected void
    BackgroundWorker_ProgressChanged
    (
        object sender,
        ProgressChangedEventArgs e
    )
    {
        AssertValid();

        // Forward the event.

        ProgressChangedEventHandler oGraphMetricCalculationProgressChanged =
            this.GraphMetricCalculationProgressChanged;

        if (oGraphMetricCalculationProgressChanged != null)
        {
            // There are two sources of this event: the graph metric
            // calculators in the Algorithms namespace, which set e.UserState
            // to a simple string ("Calculating vertex degrees," for example);
            // and this GraphMetricCalculationManager class, which sets
            // e.UserState to a GraphMetricProgress object.  In the first case,
            // wrap the simple string in a new GraphMetricProgress object.

            if (e.UserState is String)
            {
                String sProgressMessage = (String)e.UserState;

                e = new ProgressChangedEventArgs(e.ProgressPercentage,
                    new GraphMetricProgress(sProgressMessage, false) );
            }

            oGraphMetricCalculationProgressChanged(this, e);
        }
    }

    //*************************************************************************
    //  Method: BackgroundWorker_RunWorkerCompleted()
    //
    /// <summary>
    /// Handles the RunWorkerCompleted event on the BackgroundWorker object.
    /// </summary>
    ///
    /// <param name="sender">
    /// Source of the event.
    /// </param>
    ///
    /// <param name="e">
    /// Standard mouse event arguments.
    /// </param>
    //*************************************************************************

    protected void
    BackgroundWorker_RunWorkerCompleted
    (
        object sender,
        RunWorkerCompletedEventArgs e
    )
    {
        AssertValid();

        // Forward the event.

        RunWorkerCompletedEventHandler oGraphMetricCalculationCompleted =
            this.GraphMetricCalculationCompleted;

        if (oGraphMetricCalculationCompleted != null)
        {
            // If the operation was successful, the
            // RunWorkerCompletedEventArgs.Result must be a GraphMetricColumn
            // array.  (Actually, it's always a GraphMetricColumn array
            // regardless of the operation's outcome, but you can't read the
            // Result property unless the operation was successful.)

            Debug.Assert( e.Cancelled || e.Error != null ||
                e.Result is GraphMetricColumn[] );

            oGraphMetricCalculationCompleted(this, e);
        }

        m_oBackgroundWorker = null;
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        // m_oBackgroundWorker
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// File name of the executable that calculates graph metrics using the
    /// SNAP library.

    protected const String SnapGraphMetricCalculatorFileName =
        "SnapGraphMetricCalculator.exe";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Used for asynchronous calculations.  null if an asynchronous
    /// calculations are not in progress.

    protected BackgroundWorker m_oBackgroundWorker;


    //*************************************************************************
    //  Embedded class: CalculateGraphMetricsAsyncArguments()
    //
    /// <summary>
    /// Contains the arguments needed to asynchronously calculate graph
    /// metrics.
    /// </summary>
    //*************************************************************************

    protected class CalculateGraphMetricsAsyncArgs
    {
        ///
        public IGraph Graph;
        ///
        public IGraphMetricCalculator2 [] SortedGraphMetricCalculators;
        ///
        public GraphMetricUserSettings GraphMetricUserSettings;
    };
}

}
