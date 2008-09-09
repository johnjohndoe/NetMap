
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NetMap.Core;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: ClusterCalculator
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

		IVertexCollection oVertices = graph.Vertices;
		Int32 iVertices = oVertices.Count;
		Int32 iEdges = graph.Edges.Count;

		if (iVertices == 0 || iEdges == 0)
		{
			return ( new GraphMetricColumnOrdered[0] );
		}

		BackgroundWorker oBackgroundWorker =
			calculateGraphMetricsContext.BackgroundWorker;

		oBackgroundWorker.ReportProgress( 5, new GraphMetricProgress(
			"Creating initial clusters.", false) );

		// This is the list of communities.  Initially, there will be one
		// community for each of the graph's vertices.

		LinkedList<Community> oCommunities = new LinkedList<Community>();

		// This temporary dictionary is used to map a vertex ID to a community.
		// The key is the IVertex.ID and the value is the corresponding
		// Community object.

		Dictionary<Int32, Community> oVertexIDDictionary =
			new Dictionary<Int32, Community>(iVertices);

		IDGenerator oIDGenerator = new IDGenerator(1);

		// First, create a community for each of the graph's vertices.  Each
		// community contains just the vertex.

		foreach (IVertex oVertex in oVertices)
		{
			Community oCommunity = new Community();

			Int32 iID = oIDGenerator.GetNextID();

			oCommunity.ID = iID;
			oCommunity.Vertices.AddLast(oVertex);
			oCommunity.Degree = oVertex.AdjacentVertices.Length;

            oCommunities.AddLast(oCommunity);
			oVertexIDDictionary.Add(oVertex.ID, oCommunity);
		}

		// Now populate each community's list of community pairs.

		foreach (IVertex oVertex in oVertices)
		{
			Community oCommunity1 = oVertexIDDictionary[oVertex.ID];

			SortedList<Int32, CommunityPair> oCommunityPairs =
				oCommunity1.CommunityPairs;

			IVertex [] oAdjacentVertices = oVertex.AdjacentVertices;

			foreach (IVertex oAdjacentVertex in oAdjacentVertices)
			{
				Community oCommunity2 =
					oVertexIDDictionary[oAdjacentVertex.ID];

				CommunityPair oCommunityPair = new CommunityPair();
				oCommunityPair.Community1 = oCommunity1;
				oCommunityPair.Community2 = oCommunity2;

				oCommunityPairs.Add(oCommunity2.ID, oCommunityPair);
			}
		}

		// This dictionary is no longer needed.

		oVertexIDDictionary = null;

		// This max heap is used to keep track of the maximum delta Q value in
		// each community.  There is an element in the max heap for each
		// community.  The key is the Community and the value is the
		// Community's maximum delta Q.

		DeltaQMaxHeap oDeltaQMaxHeap = new DeltaQMaxHeap(iVertices);

		// Initialize all the delta Q values.

		InitializeDeltaQs(oCommunities, oDeltaQMaxHeap, iEdges);

		if (oBackgroundWorker.CancellationPending)
		{
			calculateGraphMetricsContext.DoWorkEventArgs.Cancel = true;

			return (null);
		}

		oBackgroundWorker.ReportProgress( 10, new GraphMetricProgress(
			"Merging clusters.", false) );

		#if false
		WriteCommunitiesToTrace(oCommunities);
		#endif

		// Run the algorithm outlined in the Wakita/Tsurumi paper.

		// Retrieve the community pair with the largest delta Q.

		BinaryHeapItem<Community, Single> oBinaryHeapItemWithMaximumDeltaQ;

		while ( oDeltaQMaxHeap.TryGetTop(out oBinaryHeapItemWithMaximumDeltaQ) )
		{
			Community oCommunityWithMaximumDeltaQ =
				oBinaryHeapItemWithMaximumDeltaQ.Key;

			Single fMaximumGlobalDeltaQ =
				oBinaryHeapItemWithMaximumDeltaQ.Value;

			if (fMaximumGlobalDeltaQ < 0)
			{
				// Merging additional communities would yield worse results, so
				// quit.

				break;
			}

			// Merge the communities in a community pair, update the maximum
			// delta Q values for all communities, and update the global max
			// heap.

			CommunityPair oCommunityPairWithMaximumDeltaQ =
				oCommunityWithMaximumDeltaQ.CommunityPairWithMaximumDeltaQ;

			MergeCommunities(oCommunities, oCommunityPairWithMaximumDeltaQ,
				oDeltaQMaxHeap, iEdges, oIDGenerator);
		}

		// Convert the final list of communities to an array of
		// GraphMetricColumn objects.

		return ( CommunitiesToGraphMetricColumns(oCommunities) );
	}

    //*************************************************************************
    //  Method: InitializeDeltaQs()
    //
    /// <summary>
    /// Initializes the delta Q values for each community.
    /// </summary>
    ///
    /// <param name="oCommunities">
    /// List of all communities.
    /// </param>
    ///
    /// <param name="oDeltaQMaxHeap">
	/// Max heap, used to keep track of the maximum delta Q value in each
	/// community.  There is an element in the max heap for each community.
	/// The key is the Community and the value is the Community's maximum
	/// delta Q.
    /// </param>
	///
    /// <param name="iEdgesInGraph">
    /// Number of edges in the graph.
    /// </param>
    //*************************************************************************

	protected void
	InitializeDeltaQs
	(
		LinkedList<Community> oCommunities,
		DeltaQMaxHeap oDeltaQMaxHeap,
		Int32 iEdgesInGraph
	)
	{
		Debug.Assert(oCommunities != null);
		Debug.Assert(oDeltaQMaxHeap != null);
		Debug.Assert(iEdgesInGraph > 0);
		AssertValid();

		foreach (Community oCommunity in oCommunities)
		{
			// Initialize the delta Q values for the community.

			oCommunity.InitializeDeltaQs(oCommunities, iEdgesInGraph);

			// Store the community's maximum delta Q on the max heap.

            oDeltaQMaxHeap.Add(oCommunity, oCommunity.MaximumDeltaQ);
		}
	}

    //*************************************************************************
    //  Method: MergeCommunities()
    //
    /// <summary>
	/// Merges the communities in a community pair, updates the maximum delta Q
	/// values for all communities, and updates the global max heap.
    /// </summary>
    ///
    /// <param name="oCommunities">
    /// List of all communities.
    /// </param>
	///
    /// <param name="oCommunityPairToMerge">
	/// The community pair to merge.
    /// </param>
	///
    /// <param name="oDeltaQMaxHeap">
	/// Max heap, used to keep track of the maximum delta Q value in each
	/// community.  There is an element in the max heap for each community.
	/// The key is the Community and the value is the Community's maximum
	/// delta Q.
    /// </param>
	///
    /// <param name="iEdgesInGraph">
    /// Number of edges in the graph.
    /// </param>
	///
    /// <param name="oIDGenerator">
	/// ID generator for new communities.
    /// </param>
    //*************************************************************************

	protected void
	MergeCommunities
	(
		LinkedList<Community> oCommunities,
		CommunityPair oCommunityPairToMerge,
		DeltaQMaxHeap oDeltaQMaxHeap,
		Int32 iEdgesInGraph,
		IDGenerator oIDGenerator
	)
	{
		Debug.Assert(oCommunityPairToMerge != null);
		Debug.Assert(oCommunities != null);
		Debug.Assert(oDeltaQMaxHeap != null);
		Debug.Assert(iEdgesInGraph > 0);
		Debug.Assert(oIDGenerator != null);

		// Merge Community1 and Community2 into a NewCommunity.

		Community oCommunity1 = oCommunityPairToMerge.Community1;
		Community oCommunity2 = oCommunityPairToMerge.Community2;

		Community oNewCommunity = new Community();

		oNewCommunity.ID = oIDGenerator.GetNextID();
		oNewCommunity.Degree = oCommunity1.Degree + oCommunity2.Degree;

		LinkedList<IVertex> oNewCommunityVertices = oNewCommunity.Vertices;

		foreach (IVertex oVertex in oCommunity1.Vertices)
		{
			oNewCommunityVertices.AddLast(oVertex);
		}

		foreach (IVertex oVertex in oCommunity2.Vertices)
		{
			oNewCommunityVertices.AddLast(oVertex);
		}

		// These lists are sorted by the ID of the "other community" in each
		// community pair.

		SortedList<Int32, CommunityPair> oCommunity1CommunityPairs =
			oCommunity1.CommunityPairs;

		SortedList<Int32, CommunityPair> oCommunity2CommunityPairs =
			oCommunity2.CommunityPairs;

		SortedList<Int32, CommunityPair> oNewCommunityCommunityPairs =
			oNewCommunity.CommunityPairs;

		Int32 iCommunity1CommunityPairs = oCommunity1CommunityPairs.Count;
		Int32 iCommunity2CommunityPairs = oCommunity2CommunityPairs.Count;

		IList<Int32> oCommunity1Keys = oCommunity1CommunityPairs.Keys;

		IList<CommunityPair> oCommunity1Values =
			oCommunity1CommunityPairs.Values;

		IList<Int32> oCommunity2Keys = oCommunity2CommunityPairs.Keys;

		IList<CommunityPair> oCommunity2Values =
			oCommunity2CommunityPairs.Values;

		// Step through the community pairs in oCommunity1 and oCommunity2.

		Int32 iCommunity1Index = 0;
		Int32 iCommunity2Index = 0;

		Single fMaximumDeltaQ = Single.MinValue;
		CommunityPair oCommunityPairWithMaximumDeltaQ = null;
		Single fTwoTimesEdgesInGraph = 2F * iEdgesInGraph;

		while (iCommunity1Index < iCommunity1CommunityPairs ||
               iCommunity2Index < iCommunity2CommunityPairs)
		{
			Int32 iCommunity1OtherCommunityID =
				(iCommunity1Index < iCommunity1CommunityPairs) ?
				oCommunity1Keys[iCommunity1Index] : Int32.MaxValue;

			Int32 iCommunity2OtherCommunityID =
				(iCommunity2Index < iCommunity2CommunityPairs) ?
				oCommunity2Keys[iCommunity2Index] : Int32.MaxValue;

			CommunityPair oNewCommunityPair = new CommunityPair();
			oNewCommunityPair.Community1 = oNewCommunity;

			if (iCommunity1OtherCommunityID == oCommunity2.ID)
			{
				// This is an internal connection eliminated by the merge.

				iCommunity1Index++;
				continue;
			}
			else if (iCommunity2OtherCommunityID == oCommunity1.ID)
			{
				// This is an internal connection eliminated by the merge.

				iCommunity2Index++;
				continue;
			}
			else if (iCommunity1OtherCommunityID == iCommunity2OtherCommunityID)
			{
				// The other community is connected to both commmunity 1 and
				// community 2.
				//
				// This is equation 10a from the paper "Finding Community
				// Structure in Very Large Networks," by Clauset, Newman, and
				// Moore.

				oNewCommunityPair.Community2 =
					oCommunity1Values[iCommunity1Index].Community2;

				oNewCommunityPair.DeltaQ = 
					oCommunity1Values[iCommunity1Index].DeltaQ +
					oCommunity2Values[iCommunity2Index].DeltaQ;

				iCommunity1Index++;
				iCommunity2Index++;
			}
			else if (iCommunity1OtherCommunityID < iCommunity2OtherCommunityID)
			{
				// The other community is connected only to commmunity 1.
				//
				// This is equation 10b from the same paper.

				Community oOtherCommunity =
					oCommunity1Values[iCommunity1Index].Community2;

				oNewCommunityPair.Community2 = oOtherCommunity;

				Single fAj = oCommunity2.Degree / 2F * (Single)iEdgesInGraph;

				Single fAk = oOtherCommunity.Degree / fTwoTimesEdgesInGraph;

				oNewCommunityPair.DeltaQ = 
					oCommunity1Values[iCommunity1Index].DeltaQ -
					2F * fAj * fAk;

				iCommunity1Index++;
			}
			else
			{
				// The other community is connected only to commmunity 2.
				//
				// This is equation 10c from the same paper.

				Community oOtherCommunity =
					oCommunity2Values[iCommunity2Index].Community2;

				oNewCommunityPair.Community2 = oOtherCommunity;

				Single fAi = oCommunity1.Degree / 2F * (Single)iEdgesInGraph;

				Single fAk = oOtherCommunity.Degree / fTwoTimesEdgesInGraph;

				oNewCommunityPair.DeltaQ = 
					oCommunity2Values[iCommunity2Index].DeltaQ -
					2F * fAi * fAk;

				iCommunity2Index++;
			}

			oNewCommunityCommunityPairs.Add(oNewCommunityPair.Community2.ID,
				oNewCommunityPair);

            Single fNewCommunityPairDeltaQ = oNewCommunityPair.DeltaQ;

            if (fNewCommunityPairDeltaQ > fMaximumDeltaQ)
			{
				fMaximumDeltaQ = oNewCommunityPair.DeltaQ;
				oCommunityPairWithMaximumDeltaQ = oNewCommunityPair;
			}

			// The other community is connected to one or both of the merged
			// communities.  Update it.

			oNewCommunityPair.Community2.OnMergedCommunities(
				oCommunity1, oCommunity2, oNewCommunity,
				fNewCommunityPairDeltaQ, oDeltaQMaxHeap);
		}

		oNewCommunity.CommunityPairWithMaximumDeltaQ =
			oCommunityPairWithMaximumDeltaQ;

		// Update the community list.

		oCommunities.Remove(oCommunity1);
		oCommunities.Remove(oCommunity2);
		oCommunities.AddLast(oNewCommunity);

		// Update the max heap.

		oDeltaQMaxHeap.Remove(oCommunity1);
		oDeltaQMaxHeap.Remove(oCommunity2);
		oDeltaQMaxHeap.Add(oNewCommunity, oNewCommunity.MaximumDeltaQ);
	}

    //*************************************************************************
    //  Method: CommunitiesToGraphMetricColumns()
    //
    /// <summary>
    /// Converts the final list of communities to an array of <see
	/// cref="GraphMetricColumn" /> objects.
    /// </summary>
    ///
    /// <param name="oCommunities">
    /// List of all communities.
    /// </param>
	///
	/// <returns>
    /// An array of <see cref="GraphMetricColumn" /> objects.
	/// </returns>
    //*************************************************************************

	protected GraphMetricColumn []
	CommunitiesToGraphMetricColumns
	(
		LinkedList<Community> oCommunities
	)
	{
		Debug.Assert(oCommunities != null);
		AssertValid();

		// Three columns get added:
		//
		// 1. Cluster name on the clusters worksheet.
		//
		// 2. Cluster name on the cluster vertices worksheet.
		//
		// 3. Vertex name on the cluster vertices worksheet.

		List<GraphMetricValueOrdered> oClusterNamesForClustersWorksheet =
			new List<GraphMetricValueOrdered>();

		List<GraphMetricValueOrdered> oClusterNamesForClusterVerticesWorksheet =
			new List<GraphMetricValueOrdered>();

		List<GraphMetricValueOrdered> oVertexNamesForClusterVerticesWorksheet =
			new List<GraphMetricValueOrdered>();

		foreach (Community oCommunity in oCommunities)
		{
			String sClusterName = CommunityToClusterName(oCommunity);

			GraphMetricValueOrdered oClusterNameGraphMetricValue =
				new GraphMetricValueOrdered(sClusterName);

			oClusterNamesForClustersWorksheet.Add(
				oClusterNameGraphMetricValue);

			foreach (IVertex oVertex in oCommunity.Vertices)
			{
				oClusterNamesForClusterVerticesWorksheet.Add(
					oClusterNameGraphMetricValue);

				oVertexNamesForClusterVerticesWorksheet.Add(
					new GraphMetricValueOrdered(oVertex.Name) );
			}
		}

		return ( new GraphMetricColumn [] {

			new GraphMetricColumnOrdered( WorksheetNames.Clusters,
				TableNames.Clusters,
				ClusterTableColumnNames.Name,
				ExcelUtil.AutoColumnWidth, null, null,
				oClusterNamesForClustersWorksheet.ToArray()
				),

			new GraphMetricColumnOrdered( WorksheetNames.ClusterVertices,
				TableNames.ClusterVertices,
				ClusterVerticesTableColumnNames.ClusterName,
				ExcelUtil.AutoColumnWidth, null, null,
				oClusterNamesForClusterVerticesWorksheet.ToArray()
				),

			new GraphMetricColumnOrdered( WorksheetNames.ClusterVertices,
				TableNames.ClusterVertices,
				ClusterVerticesTableColumnNames.VertexName,
				ExcelUtil.AutoColumnWidth, null, null,
				oVertexNamesForClusterVerticesWorksheet.ToArray()
				),
				} );
	}

    //*************************************************************************
    //  Method: CommunityToClusterName()
    //
    /// <summary>
    /// Gets a cluster name to use in the workbook.
    /// </summary>
    ///
    /// <param name="oCommunity">
    /// The community to get a cluster name for.
    /// </param>
    ///
    /// <returns>
	/// A cluster name to use in the workbook.
    /// </returns>
    //*************************************************************************

	protected String
	CommunityToClusterName
	(
		Community oCommunity
	)
	{
		Debug.Assert(oCommunity != null);
		AssertValid();

		return ( (oCommunity.ID).ToString(ExcelTemplateForm.Int32Format) );
	}

    //*************************************************************************
    //  Method: WriteCommunitiesToTrace()
    //
    /// <summary>
    /// Writes the collection of communities to the TraceListeners collection.
	/// Debug-only.
    /// </summary>
    ///
    /// <param name="oCommunities">
    /// List of all communities.
    /// </param>
    //*************************************************************************

    [Conditional("DEBUG")]

    protected void
    WriteCommunitiesToTrace
    (
		LinkedList<Community> oCommunities
    )
	{
		Debug.WriteLine(oCommunities != null);
		AssertValid();

		foreach (Community oCommunity in oCommunities)
		{
			Debug.Write( oCommunity.ToString("D") );
		}
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
