
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Reflection;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.ApplicationUtil;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: OverallMetricCalculator
//
/// <summary>
/// Calculates the overall metrics for the graph.
/// </summary>
///
/// <para>
/// The calculations for graph density skip all self-loops, which would render
/// the density invalid.  The graph density is rendered invalid if the graph
/// has duplicate edges, however.
/// </para>
///
//*****************************************************************************

public class OverallMetricCalculator : GraphMetricCalculatorBase
{
    //*************************************************************************
    //  Constructor: OverallMetricCalculator()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="OverallMetricCalculator" /> class.
    /// </summary>
    //*************************************************************************

    public OverallMetricCalculator()
    {
		// (Do nothing.)

		AssertValid();
    }

	//*************************************************************************
	//	Property: GraphMetricDescription
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

			return ("overall metrics");
		}
	}

    //*************************************************************************
    //  Method: CalculateGraphMetrics()
    //
    /// <summary>
    /// Calculates a set of one or more related metrics.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
	/// edges and self-loops.
    /// </param>
	///
    /// <param name="calculateGraphMetricsContext">
	/// Provides access to objects needed for calculating graph metrics.
    /// </param>
    ///
	/// <returns>
	/// An array of GraphMetricColumn objects, one for each related metric
	/// calculated by this method.
	/// </returns>
	///
	/// <remarks>
	/// This method should periodically check BackgroundWorker.<see
	/// cref="BackgroundWorker.CancellationPending" />.  If true, the method
	/// should set DoWorkEventArgs.<see cref="CancelEventArgs.Cancel" /> to
	/// true and return null immediately.
	///
	/// <para>
	/// It should also periodically report progress by calling the
	/// BackgroundWorker.<see
	/// cref="BackgroundWorker.ReportProgress(Int32, Object)" /> method.  The
	/// userState argument must be a <see cref="GraphMetricProgress" /> object.
	/// </para>
	///
	/// <para>
	/// Calculated metrics for hidden rows are ignored by the caller, because
	/// Excel misbehaves when values are written to hidden cells.
	/// </para>
	///
	/// </remarks>
    //*************************************************************************

    public override GraphMetricColumn []
    CalculateGraphMetrics
    (
		IGraph graph,
		CalculateGraphMetricsContext calculateGraphMetricsContext
    )
	{
		Debug.Assert(graph != null);
		Debug.Assert(calculateGraphMetricsContext != null);
		AssertValid();

		if (!calculateGraphMetricsContext.GraphMetricUserSettings.
			CalculateOverallMetrics)
		{
			return ( new GraphMetricColumn[0] );
		}

		// Check for cancellation and report progress.  (The progress numbers
		// used here are arbitrary.)

		BackgroundWorker oBackgroundWorker =
			calculateGraphMetricsContext.BackgroundWorker;

		if (oBackgroundWorker.CancellationPending)
		{
			calculateGraphMetricsContext.DoWorkEventArgs.Cancel = true;

			return (null);
		}

		ReportProgress(5, 100, oBackgroundWorker);

		Int32 iVertices = graph.Vertices.Count;
		Int32 iEdges = graph.Edges.Count;

		Boolean bGraphIsDirected =
			(graph.Directedness == GraphDirectedness.Directed);

		DuplicateEdgeDetector oDuplicateEdgeDetector =
			calculateGraphMetricsContext.DuplicateEdgeDetector;

		Int32 iSelfLoops = CountSelfLoops(graph);

		// The definition of graph density for an undirected graph is given
		// here:
		//
		// http://en.wikipedia.org/wiki/Dense_graph
		//
		// For directed graphs, the equation must be divided by 2 to account
		// for the doubled number of possible edges.
		//
		// The equation doesn't take self-loops into account.  They should not
		// be included.

		Int32 iNonSelfLoops = iEdges - iSelfLoops;

		Object oGraphDensity = "Not Applicable";

		if (iVertices > 1)
		{
			Double dVertices = (Double)iVertices;

			Double dGraphDensity = (2 * (Double)iNonSelfLoops) /
				( dVertices * (dVertices - 1) );

			if (bGraphIsDirected)
			{
				dGraphDensity /= 2.0;
			}

            // Don't allow rounding errors to create a very small negative
            // number.

            dGraphDensity = Math.Max(0, dGraphDensity);

			oGraphDensity = (Single)dGraphDensity;
		}

		String sDuplicateEdgeStyle = GraphMetricColumn.ExcelStyleNormal;

		if (calculateGraphMetricsContext.DuplicateEdgeDetector.
			GraphContainsDuplicateEdges)
		{
			// The graph density is rendered invalid when the graph has
			// duplicate edges.

			sDuplicateEdgeStyle = GraphMetricColumn.ExcelStyleBad;
		}

		GraphMetricValueOrdered [] aoMetricNameGraphMetricValues =
			new GraphMetricValueOrdered [] {

				new GraphMetricValueOrdered("Graph Type"),
				new GraphMetricValueOrdered(),
				new GraphMetricValueOrdered("Unique Edges"),

				new GraphMetricValueOrdered("Edges With Duplicates",
					sDuplicateEdgeStyle),

				new GraphMetricValueOrdered("Total Edges"),
				new GraphMetricValueOrdered(),
				new GraphMetricValueOrdered("Self-Loops"),
				new GraphMetricValueOrdered(),
				new GraphMetricValueOrdered("Vertices"),
				new GraphMetricValueOrdered(),

				new GraphMetricValueOrdered("Graph Density",
					sDuplicateEdgeStyle),

				new GraphMetricValueOrdered(),
				new GraphMetricValueOrdered("NodeXL Version"),
				};

		GraphMetricValueOrdered [] aoMetricValueGraphMetricValues =
			new GraphMetricValueOrdered [] {

				new GraphMetricValueOrdered(bGraphIsDirected ?
					"Directed" : "Undirected"),

				new GraphMetricValueOrdered(),

				new GraphMetricValueOrdered( FormatInt32(
					oDuplicateEdgeDetector.UniqueEdges) ),

				new GraphMetricValueOrdered( FormatInt32(
					oDuplicateEdgeDetector.EdgesWithDuplicates) ),

				new GraphMetricValueOrdered( FormatInt32(iEdges) ),
				new GraphMetricValueOrdered(),
				new GraphMetricValueOrdered( FormatInt32(iSelfLoops) ),
				new GraphMetricValueOrdered(),
				new GraphMetricValueOrdered( FormatInt32(iVertices) ),
				new GraphMetricValueOrdered(),

				new GraphMetricValueOrdered(oGraphDensity,
					sDuplicateEdgeStyle),

				new GraphMetricValueOrdered(),
				new GraphMetricValueOrdered( AssemblyUtil2.GetFileVersion() ),
				};

		return ( new GraphMetricColumn[] {

			new GraphMetricColumnOrdered(WorksheetNames.OverallMetrics,
				TableNames.OverallMetrics,
				OverallMetricsTableColumnNames.Name,
				OverallMetricsTableColumnWidths.Name,
				null, null, aoMetricNameGraphMetricValues
				),

			new GraphMetricColumnOrdered(WorksheetNames.OverallMetrics,
				TableNames.OverallMetrics,
				OverallMetricsTableColumnNames.Value,
				OverallMetricsTableColumnWidths.Value,
				null, null, aoMetricValueGraphMetricValues
				),
			} );
	}

    //*************************************************************************
    //  Method: CountSelfLoops()
    //
    /// <summary>
    /// Counts the number of self-loops in the graph.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.
    /// </param>
	///
	/// <returns>
	/// The number of self-loops in the graph.
	/// </returns>
    //*************************************************************************

    protected Int32
    CountSelfLoops
    (
		IGraph oGraph
    )
	{
		Debug.Assert(oGraph != null);
		AssertValid();

		Int32 iSelfLoops = 0;

		foreach (IEdge oEdge in oGraph.Edges)
		{
			if (oEdge.IsSelfLoop)
			{
				iSelfLoops++;
			}
		}

		return (iSelfLoops);
	}

    //*************************************************************************
    //  Method: FormatInt32()
    //
    /// <summary>
    /// Formats an Int32 for use in the metric value column
    /// </summary>
    ///
    /// <param name="iInt32">
    /// Int32 to format.
    /// </param>
	///
	/// <returns>
	/// Formatted Int32.
	/// </returns>
    //*************************************************************************

    protected String
    FormatInt32
    (
		Int32 iInt32
    )
	{
		AssertValid();

		return ( iInt32.ToString(ExcelTemplateForm.Int32Format) );
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
