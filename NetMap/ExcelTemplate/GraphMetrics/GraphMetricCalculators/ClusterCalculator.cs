
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Interface: ClusterCalculator
//
/// <summary>
/// Partitions a graph into clusters.
/// </summary>
///
/// <remarks>
/// The algorithm used by this class is from "Finding Community Structure in
/// Mega-scale Social Networks," by Ken Wakita and Toshiyuki Tsurumi:
///
/// <para>
///	http://arxiv.org/PS_cache/cs/pdf/0702/0702048v1.pdf
/// </para>
///
/// <para>
///	TODO: This class is not implemented yet.  It contains stub code only.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class ClusterCalculator : GraphMetricCalculatorBase
{
    //*************************************************************************
    //  Constructor: ClusterCalculator()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ClusterCalculator" />
	/// class.
    /// </summary>
    //*************************************************************************

    public ClusterCalculator()
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

			return ("clusters");
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
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <param name="graphMetricUserSettings">
    /// The user's settings for calculating graph metrics.
    /// </param>
    ///
    /// <param name="backgroundWorker">
    /// The <see cref="BackgroundWorker" /> object that is performing all graph
	/// metric calculations.
    /// </param>
    ///
    /// <param name="doWorkEventArgs">
    /// The <see cref="DoWorkEventArgs" /> object that was passed to <see
	/// cref="BackgroundWorker.DoWork" />.
    /// </param>
	///
	/// <returns>
	/// An array of GraphMetricColumn objects, one for each related metric
	/// calculated by this method.
	/// </returns>
	///
	/// <remarks>
	/// This method should periodically check backgroundWorker.<see
	/// cref="BackgroundWorker.CancellationPending" />.  If true, the method
	/// should set doWorkEventArgs.<see cref="CancelEventArgs.Cancel" /> to
	/// true and return null immediately.
	///
	/// <para>
	/// It should also periodically report progress by calling the <paramref
	/// name="backgroundWorker" />.<see
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
		GraphMetricUserSettings graphMetricUserSettings,
		BackgroundWorker backgroundWorker,
		DoWorkEventArgs doWorkEventArgs
    )
	{
		AssertValid();

		if (!graphMetricUserSettings.CalculateClusters)
		{
			return ( new GraphMetricColumnOrdered[0] );
		}

		// TODO: This is stub code for testing.  The cluster calculations are
		// not yet implemented.

		List<GraphMetricValueOrdered> oClusterNames =
			new List<GraphMetricValueOrdered>();

		List<GraphMetricValueOrdered> oVertexNames =
			new List<GraphMetricValueOrdered>();

		oClusterNames.Add( new GraphMetricValueOrdered("Cluster 1") );
		oClusterNames.Add( new GraphMetricValueOrdered("Cluster 1") );

		oVertexNames.Add( new GraphMetricValueOrdered("Vertex 1") );
		oVertexNames.Add( new GraphMetricValueOrdered("Vertex 2") );

		return ( new GraphMetricColumn [] {

			new GraphMetricColumnOrdered( WorksheetNames.ClusterVertices,
				TableNames.ClusterVertices,
				ClusterVerticesTableColumnNames.ClusterName, 20F,
				null, oClusterNames.ToArray()
				),

			new GraphMetricColumnOrdered( WorksheetNames.ClusterVertices,
				TableNames.ClusterVertices,
				ClusterVerticesTableColumnNames.VertexName, 20F,
				null, oVertexNames.ToArray()
				),
				} );
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

	// (None.)


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
