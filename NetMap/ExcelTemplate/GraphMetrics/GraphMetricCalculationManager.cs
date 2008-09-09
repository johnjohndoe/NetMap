
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.ExcelTemplate
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
/// calculations.  Handle the <see cref="DuplicateEdgeDetected" />, <see
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

		AssertValid();
    }

	//*************************************************************************
	//	Property: IsBusy
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
	/// Asynchronously calculates one or more sets of all graph metrics and
	/// returns the results as an array of <see cref="GraphMetricColumn" />
	/// objects.
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

		IGraphMetricCalculator[] aoAllGraphMetricCalculators =
			new IGraphMetricCalculator[] {

				new VertexDegreeCalculator(),
				new ClusteringCoefficientCalculator(),
				new BetweennessCentralityCalculator(),
				new OverallMetricCalculator(),
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
	/// An array of <see cref="IGraphMetricCalculator" /> implementations, one
	/// for each set of graph metrics that should be calculated.
	/// </param>
	///
	/// <param name="graphMetricUserSettings">
	/// User settings for calculating graph metrics.
	/// </param>
	///
    /// <remarks>
	/// For each <see cref="IGraphMetricCalculator" /> implementation in the
	/// <paramref name="graphMetricCalculators" /> array, this method calls the
	/// implementation's <see
	/// cref="IGraphMetricCalculator.CalculateGraphMetrics" /> method.  The
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
	/// <para>
	/// If <paramref name="workbook" /> contains duplicate edges, a <see
	/// cref="DuplicateEdgeDetected" /> event is fired on the caller's thread
	/// before asynchronous calculations begin.  Because duplicate edges can
	/// cause unexpected results with some graph calculators, the event handler
	/// is given the opportunity to cancel all calculations by setting the
	/// event's <see cref="CancelEventArgs.Cancel" /> property to true.  If
	/// there is no event handler or the handler leaves the <see
	/// cref="CancelEventArgs.Cancel" /> property set to false, calculations
	/// continue anyway.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

	public void
    CalculateGraphMetricsAsync
	(
        Microsoft.Office.Interop.Excel.Workbook workbook,
		IGraphMetricCalculator [] graphMetricCalculators,
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

		IGraph oGraph = ReadWorkbook(workbook);

		// Check for duplicate edges, which can cause unexpected results with
		// some graph calculators.

		DuplicateEdgeDetector oDuplicateEdgeDetector =
			new DuplicateEdgeDetector(oGraph);

		if (oDuplicateEdgeDetector.GraphContainsDuplicateEdges)
		{
			CancelEventHandler oDuplicateEdgeDetected =
				this.DuplicateEdgeDetected;

			if (oDuplicateEdgeDetected != null)
			{
				// Fire a DuplicateEdgeDetected event and give the handler a
				// chance to cancel calculations.

				CancelEventArgs oCancelEventArgs = new CancelEventArgs(false);

				oDuplicateEdgeDetected(this, oCancelEventArgs);

				if (oCancelEventArgs.Cancel)
				{
					return;
				}
			}
		}

		// Wrap the arguments in an object that can be passed to
		// BackgroundWorker.RunWorkerAsync().

		CalculateGraphMetricsAsyncArgs oCalculateGraphMetricsAsyncArgs =
			new CalculateGraphMetricsAsyncArgs();

		oCalculateGraphMetricsAsyncArgs.Graph = oGraph;

		oCalculateGraphMetricsAsyncArgs.GraphMetricCalculators =
			graphMetricCalculators;

		oCalculateGraphMetricsAsyncArgs.GraphMetricUserSettings =
			graphMetricUserSettings;

		oCalculateGraphMetricsAsyncArgs.DuplicateEdgeDetector =
			oDuplicateEdgeDetector;

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
	//	Event: DuplicateEdgeDetected
	//
	/// <summary>
	///	Occurs when a duplicate edge is detected by <see
	/// cref="CalculateGraphMetricsAsync(
	/// Microsoft.Office.Interop.Excel.Workbook, GraphMetricUserSettings)" />.
	/// </summary>
	///
    /// <remarks>
	/// Because duplicate edges can cause unexpected results with some graph
	/// calculators, the event handler is given the opportunity to cancel all
	/// calculations by setting the event's <see
	/// cref="CancelEventArgs.Cancel" /> property to true.  If there is no
	/// event handler or the handler leaves the <see
	/// cref="CancelEventArgs.Cancel" /> property set to false, calculations
	/// continue anyway.
    /// </remarks>
	//*************************************************************************

	public event CancelEventHandler DuplicateEdgeDetected;


	//*************************************************************************
	//	Event: GraphMetricCalculationProgressChanged
	//
	/// <summary>
	///	Occurs when progress is made during the graph metric calculations
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
	//	Event: GraphMetricCalculationCompleted
	//
	/// <summary>
	///	Occurs when the graph metric calculations started by <see
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
	//	Property: ClassName
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
	/// Reads the workbook contents into a NetMap graph.
    /// </summary>
	///
	/// <param name="oWorkbook">
    /// Workbook containing the graph contents.
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
        Microsoft.Office.Interop.Excel.Workbook oWorkbook
	)
	{
		Debug.Assert(oWorkbook != null);
		AssertValid();

		ReadWorkbookContext oReadWorkbookContext = new ReadWorkbookContext();
		oReadWorkbookContext.FillIDColumns = true;
		oReadWorkbookContext.PopulateVertexWorksheet = true;

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

		// Retrieve the graph metric arguments.

		IGraph oGraph = oCalculateGraphMetricsAsyncArgs.Graph;

		IGraphMetricCalculator [] aoGraphMetricCalculators =
			oCalculateGraphMetricsAsyncArgs.GraphMetricCalculators;

		List<GraphMetricColumn> oAggregatedGraphMetricColumns =
			new List<GraphMetricColumn>();

        CalculateGraphMetricsContext oCalculateGraphMetricsContext =
            new CalculateGraphMetricsContext(
				oCalculateGraphMetricsAsyncArgs.GraphMetricUserSettings,
				oCalculateGraphMetricsAsyncArgs.DuplicateEdgeDetector,
                oBackgroundWorker, oDoWorkEventArgs);

		// Loop through the IGraphMetricCalculator implementations.

		Int32 iGraphMetricCalculators = aoGraphMetricCalculators.Length;

		for (Int32 i = 0; i < iGraphMetricCalculators; i++)
		{
			IGraphMetricCalculator oGraphMetricCalculator =
				aoGraphMetricCalculators[i];

			// Calculate the implementation's graph metrics.

			GraphMetricColumn [] aoGraphMetricColumns =
				oGraphMetricCalculator.CalculateGraphMetrics(oGraph,
					oCalculateGraphMetricsContext);

			if (oDoWorkEventArgs.Cancel)
			{
				oBackgroundWorker.ReportProgress(0,
					new GraphMetricProgress(
						"Graph metric calculations cancelled.", false)
					);

				return;
			}

			// Aggregate the results.

			oAggregatedGraphMetricColumns.AddRange(aoGraphMetricColumns);
		}

		oDoWorkEventArgs.Result = oAggregatedGraphMetricColumns.ToArray();

		oBackgroundWorker.ReportProgress(100,
			new GraphMetricProgress(
				"Graph metric calculations completed, writing results to"
				+ " workbook.",
				true)
			);

		// Let the dialog the display the final progress report and update its
		// controls.

		System.Threading.Thread.Sleep(1);
    }

	//*************************************************************************
	//	Method: BackgroundWorker_DoWork()
	//
	/// <summary>
	///	Handles the DoWork event on the BackgroundWorker object.
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
	//	Method: BackgroundWorker_ProgressChanged()
	//
	/// <summary>
	///	Handles the ProgressChanged event on the BackgroundWorker object.
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
			oGraphMetricCalculationProgressChanged(this, e);
		}
	}

	//*************************************************************************
	//	Method: BackgroundWorker_RunWorkerCompleted()
	//
	/// <summary>
	///	Handles the RunWorkerCompleted event on the BackgroundWorker object.
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
		public IGraphMetricCalculator [] GraphMetricCalculators;
		///
		public GraphMetricUserSettings GraphMetricUserSettings;
		///
		public DuplicateEdgeDetector DuplicateEdgeDetector;
	};
}

}
