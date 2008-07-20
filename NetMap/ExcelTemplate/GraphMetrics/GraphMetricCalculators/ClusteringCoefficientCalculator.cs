
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Interface: ClusteringCoefficientCalculator
//
/// <summary>
/// Calculates the clustering coefficient for each of the graph's vertices.
/// </summary>
///
/// <remarks>
/// See this article for a definition of clustering coefficient:
///
/// <para>
/// http://www.wandora.org/wandora/wiki/index.php?title=Clustering_coefficient
/// </para>
///
/// </remarks>
//*****************************************************************************

public class ClusteringCoefficientCalculator : GraphMetricCalculatorBase
{
    //*************************************************************************
    //  Constructor: ClusteringCoefficientCalculator()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="ClusteringCoefficientCalculator" /> class.
    /// </summary>
    //*************************************************************************

    public ClusteringCoefficientCalculator()
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

			return ("clustering coefficients");
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

		if (!graphMetricUserSettings.CalculateClusteringCoefficient)
		{
			return ( new GraphMetricColumn[0] );
		}

		IVertexCollection oVertices = graph.Vertices;
		Int32 iVertices = oVertices.Count;

		List<GraphMetricValueWithID> oGraphMetricValues =
			new List<GraphMetricValueWithID>();

		Int32 iCalculations = 0;

		foreach (IVertex oVertex in oVertices)
		{
			// Check for cancellation and report progress every
			// VerticesPerProgressReport calculations.

			if (iCalculations % VerticesPerProgressReport == 0)
			{
				if (backgroundWorker.CancellationPending)
				{
					doWorkEventArgs.Cancel = true;

					return (null);
				}

                ReportProgress(iCalculations, iVertices, backgroundWorker);
			}

			Int32 iRowID;

			if ( !TryGetRowID(oVertex, out iRowID) )
			{
				continue;
			}

			oGraphMetricValues.Add(new GraphMetricValueWithID(
				iRowID, CalculateClusteringCoefficient(oVertex) ) );

			iCalculations++;
		}

		return ( new GraphMetricColumn [] {
			new GraphMetricColumnWithID( WorksheetNames.Vertices,
				TableNames.Vertices,
				VertexTableColumnNames.ClusteringCoefficient, ColumnWidthChars,
				NumericFormat, oGraphMetricValues.ToArray()
				) } );
	}

    //*************************************************************************
    //  Method: CalculateClusteringCoefficient()
    //
    /// <summary>
    /// Calculates a vertex's clustering coefficient.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to calculate the clustering coefficient for.
    /// </param>
    ///
	/// <returns>
	/// The vertex's clustering coefficient.
	/// </returns>
    //*************************************************************************

    protected Double
    CalculateClusteringCoefficient
    (
		IVertex oVertex
    )
	{
		Debug.Assert(oVertex != null);
		AssertValid();

		IVertex [] aoAdjacentVertices = oVertex.AdjacentVertices;
		Int32 iAdjacentVertices = aoAdjacentVertices.Length;

		if (iAdjacentVertices == 0)
		{
			return (0);
		}

		// Create a dictionary of the vertex's adjacent vertices.  The key is
		// the IVertex.ID and the value isn't used.

		Dictionary<Int32, Char> oAdjacentVertexIDDictionary =
			new Dictionary<Int32, Char>();

		foreach (IVertex oAdjacentVertex in aoAdjacentVertices)
		{
			oAdjacentVertexIDDictionary.Add(oAdjacentVertex.ID, ' ');
			
		}

		// Create a dictionary of the unique edges in the vertex's
		// neighborhood.  These are the edges that connect adjacent vertices to
		// each other but not to the vertex.  The key is the IEdge.ID and the
		// value isn't used.

		Dictionary<Int32, Char> oEdgesInNeighborhoodIDDictionary =
			new Dictionary<Int32, Char>();

		// Loop through the vertex's adjacent vertices.

		foreach (IVertex oAdjacentVertex in aoAdjacentVertices)
		{
			// Loop through the adjacent vertex's incident edges.

			foreach (IEdge oIncidentEdge in oAdjacentVertex.IncidentEdges)
			{
				// If this incident edge connects the adjacent vertex to
				// another adjacent vertex, add it to the dictionary if it
				// isn't already there.

				if ( oAdjacentVertexIDDictionary.ContainsKey(
					oIncidentEdge.GetAdjacentVertex(oAdjacentVertex).ID) )
				{
					oEdgesInNeighborhoodIDDictionary[oIncidentEdge.ID] = ' ';
				}
			}
		}

		Double dNumerator = (Double)oEdgesInNeighborhoodIDDictionary.Count;

        Debug.Assert(iAdjacentVertices > 0);

        Double dDenominator =
			CalculateEdgesInFullyConnectedNeighborhood(iAdjacentVertices);

        if (dDenominator == 0)
        {
            return (0);
        }

		return (dNumerator / dDenominator);
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

	/// Width of the clustering coefficient column, in characters.

	protected const Single ColumnWidthChars = 22;

	/// Number format for the column.

	protected const String NumericFormat = "0.000";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
