
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: BetweennessCentralityCalculator
//
/// <summary>
/// Calculates the betweenness centrality for each of the graph's vertices.
/// </summary>
///
/// <remarks>
/// The algorithm used to calculate betweenness centrality is taken from the
/// paper "A Faster Algorithm for Betweenness Centrality," by Ulrik Brandes.
/// The paper can be found here:
///
/// <para>
/// http://www.inf.uni-konstanz.de/algo/publications/b-fabc-01.pdf
/// </para>
///
/// <para>
/// According to the paper, the algorithm works even if the graph has
/// self-loops and multiple edges.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class BetweennessCentralityCalculator : GraphMetricCalculatorBase
{
    //*************************************************************************
    //  Constructor: BetweennessCentralityCalculator()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="BetweennessCentralityCalculator" /> class.
    /// </summary>
    //*************************************************************************

    public BetweennessCentralityCalculator()
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

			return ("betweenness centralities");
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
			CalculateBetweennessCentrality)
		{
			return ( new GraphMetricColumn[0] );
		}

		IVertexCollection oVertices = graph.Vertices;
		Int32 iVertices = oVertices.Count;

		Boolean bGraphIsDirected =
			(graph.Directedness == GraphDirectedness.Directed);

		BackgroundWorker oBackgroundWorker =
			calculateGraphMetricsContext.BackgroundWorker;

		// Note: The variable names and some of the comments below are those
		// used in Alogorithm 1 of the paper "A Faster Algorithm for
		// Betweenness Centrality," by Ulrik Brandes.

		// The key is an IVertex.ID and the value is the corresponding
		// betweenness centrality.

		Dictionary<Int32, Single> Cb = new Dictionary<Int32, Single>();

		foreach (IVertex oVertex in oVertices)
		{
			Cb.Add(oVertex.ID, 0);
		}

		// These objects are created once and cleared before the betweenness
		// centrality for each vertex is calculated.  For the dictionaries, the
		// key is an IVertex.ID.

		Stack<IVertex> S = new Stack<IVertex>();

		// The LinkedList elements within P are created on demand.

		Dictionary< Int32, LinkedList<IVertex> > P =
			new Dictionary< Int32, LinkedList<IVertex> >();

		Dictionary<Int32, Int32> sigma = new Dictionary<Int32, Int32>();
		Dictionary<Int32, Int32> d = new Dictionary<Int32, Int32>();
		Queue<IVertex> Q = new Queue<IVertex>();
		Dictionary<Int32, Single> delta = new Dictionary<Int32, Single>();

		Int32 iCalculations = 0;
		Single MaximumCb = 0;

		foreach (IVertex s in oVertices)
		{
			// Check for cancellation and report progress every
			// VerticesPerProgressReport calculations.

			if (iCalculations % VerticesPerProgressReport == 0)
			{
				if (oBackgroundWorker.CancellationPending)
				{
					calculateGraphMetricsContext.DoWorkEventArgs.Cancel = true;

					return (null);
				}

                ReportProgress(iCalculations, iVertices, oBackgroundWorker);
			}

			S.Clear();
			P.Clear();
			sigma.Clear();
			d.Clear();
			Q.Clear();
			delta.Clear();

			foreach (IVertex oVertex in oVertices)
			{
				sigma.Add(oVertex.ID, 0);
				d.Add(oVertex.ID, -1);
				delta.Add(oVertex.ID, 0);
			}

			sigma[s.ID] = 1;
			d[s.ID] = 0;
			Q.Enqueue(s);

			while (Q.Count > 0)
			{
				IVertex v = Q.Dequeue();
				S.Push(v);

				foreach (IVertex w in v.AdjacentVertices)
				{
					// w found for the first time?

					if (d[w.ID] < 0)
					{
						Q.Enqueue(w);
						d[w.ID] = d[v.ID] + 1;
					}

					// Shortest path to w via v?

					if (d[w.ID] == d[v.ID] + 1)
					{
						sigma[w.ID] += sigma[v.ID];

						LinkedList<IVertex> PofW;

						if ( !P.TryGetValue(w.ID, out PofW) )
						{
							PofW = new LinkedList<IVertex>();
							P.Add(w.ID, PofW);
						}

						PofW.AddLast(v);
					}
				}
			}

			// S returns vertices in order of non-increasing distance from s.

			while (S.Count > 0)
			{
				IVertex w = S.Pop();

				LinkedList<IVertex> PofW;

				if ( P.TryGetValue(w.ID, out PofW) )
				{
					foreach (IVertex v in PofW)
					{
						Debug.Assert(sigma[w.ID] != 0);

						delta[v.ID] +=
							( (Single)sigma[v.ID] / (Single)sigma[w.ID] )
							* ( 1.0F + delta[w.ID] );
					}
				}

				if (w.ID != s.ID)
				{
					Single CbOfW = Cb[w.ID] + delta[w.ID];
					MaximumCb = Math.Max(MaximumCb, CbOfW);
					Cb[w.ID] = CbOfW;
				}
			}

			iCalculations++;
		}

		// As noted in the Brandes paper, "the centrality scores need to be
		// divided by two if the graph is undirected, since all shortest paths
		// are considered twice."
		//
		// (Because the calculated centralities are normalized below, this
		// actually has no effect.  The divide-by-two code is included in case
		// normalization is removed in the future.)

		if (!bGraphIsDirected)
		{
			MaximumCb /= 2.0F;
		}

		// Transfer the betweenness centralities to an array of
		// GraphMetricValue objects.

		List<GraphMetricValueWithID> oGraphMetricValues =
			new List<GraphMetricValueWithID>();

		foreach (IVertex oVertex in oVertices)
		{
			// Try to get the row ID stored in the worksheet.

			Int32 iRowID;

			if ( TryGetRowID(oVertex, out iRowID) )
			{
				Single ThisCb = Cb[oVertex.ID];

				if (!bGraphIsDirected)
				{
					ThisCb /= 2.0F;
				}

				if (MaximumCb != 0)
				{
					// Normalize the value.

					ThisCb /= MaximumCb;
				}

				oGraphMetricValues.Add(
					new GraphMetricValueWithID(iRowID, ThisCb) );
			}
		}

		return ( new GraphMetricColumn [] {
			new GraphMetricColumnWithID( WorksheetNames.Vertices,
				TableNames.Vertices,
				VertexTableColumnNames.BetweennessCentrality,
				VertexTableColumnWidths.BetweennessCentrality,
				NumericFormat, null, oGraphMetricValues.ToArray()
				) } );
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

	/// Number format for the column.

	protected const String NumericFormat = "0.000";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
