
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.ExcelTemplate
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

	public abstract String
	GraphMetricDescription
	{
		get;
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
	/// See <see cref="IGraphMetricCalculator.CalculateGraphMetrics" /> for
	/// details.
	/// </remarks>
    //*************************************************************************

    public abstract GraphMetricColumn []
    CalculateGraphMetrics
    (
		IGraph graph,
		CalculateGraphMetricsContext calculateGraphMetricsContext
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

			"Calculating {0}."
			,
			this.GraphMetricDescription
			);

		oBackgroundWorker.ReportProgress( iPercentProgress,
			new GraphMetricProgress(sProgress, false) );
	}

    //*************************************************************************
    //  Method: TryGetRowID()
    //
    /// <summary>
    /// Attempts to get the worksheet row ID for an edge or vertex.
    /// </summary>
    ///
    /// <param name="oEdgeOrVertex">
    /// The edge or vertex to get the ID for.
    /// </param>
    ///
    /// <param name="iRowID">
    /// Where the row ID gets stored if true is returned.
    /// </param>
    ///
	/// <returns>
	/// true if successful.
	/// </returns>
	///
	/// <remarks>
	/// The returned ID is the worksheet row ID, not the ID stored in the edge
	/// or vertex's ID property.
	/// </remarks>
    //*************************************************************************

	protected Boolean
	TryGetRowID
	(
		IMetadataProvider oEdgeOrVertex,
		out Int32 iRowID
	)
	{
		Debug.Assert(oEdgeOrVertex != null);

		iRowID = Int32.MinValue;

		// The worksheet row ID is stored in the edge or vertex's Tag, as an
		// Int32.

		if ( oEdgeOrVertex.Tag == null || !(oEdgeOrVertex.Tag is Int32) )
		{
			return (false);
		}

		iRowID = (Int32)oEdgeOrVertex.Tag;

		return (true);
	}

    //*************************************************************************
    //  Method: CalculateEdgesInFullyConnectedNeighborhood()
    //
    /// <summary>
    /// Calculates the number of edges in the neighborhood of a vertex if the
	/// neighborhood were fully connected.
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
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
