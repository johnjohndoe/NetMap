
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Interface: VertexDegreeCalculator
//
/// <summary>
/// Calculates the in-degree, out-degree, and degree for each of the graph's
/// vertices.
/// </summary>
//*****************************************************************************

public class VertexDegreeCalculator : GraphMetricCalculatorBase
{
    //*************************************************************************
    //  Constructor: VertexDegreeCalculator()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="VertexDegreeCalculator" /> class.
    /// </summary>
    //*************************************************************************

    public VertexDegreeCalculator()
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

			return ("vertex degrees");
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

		if (!graphMetricUserSettings.CalculateDegree &&
			!graphMetricUserSettings.CalculateInDegree &&
			!graphMetricUserSettings.CalculateOutDegree)
		{
			return ( new GraphMetricColumn[0] );
		}

		IVertexCollection oVertices = graph.Vertices;
		Int32 iVertices = oVertices.Count;

		// The following lists correspond to vertex worksheet columns.

		List<GraphMetricValueWithID> oInDegreeGraphMetricValues =
			new List<GraphMetricValueWithID>();

		List<GraphMetricValueWithID> oOutDegreeGraphMetricValues =
			new List<GraphMetricValueWithID>();

		List<GraphMetricValueWithID> oDegreeGraphMetricValues =
			new List<GraphMetricValueWithID>();

		// Create a dictionary to keep track of vertex degrees.  The key is the
		// IVertex.ID and the value is a zero-based index into the above lists.

		Dictionary<Int32, Int32> oVertexIDDictionary =
			new Dictionary<Int32, Int32>();

		Int32 iCalculations = 0;
		Int32 iRowID;

		// Calculate the degrees for each vertex.

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

			if ( !TryGetRowID(oVertex, out iRowID) )
			{
				continue;
			}

			Int32 iInDegree, iOutDegree;

			CalculateVertexDegrees(oVertex, out iInDegree, out iOutDegree);

			oInDegreeGraphMetricValues.Add(new GraphMetricValueWithID(
				iRowID, iInDegree) );

			oOutDegreeGraphMetricValues.Add(new GraphMetricValueWithID(
				iRowID, iOutDegree) );

			oDegreeGraphMetricValues.Add(new GraphMetricValueWithID(
				iRowID, iInDegree + iOutDegree) );

			Debug.Assert(oInDegreeGraphMetricValues.Count ==
				oOutDegreeGraphMetricValues.Count);

			oVertexIDDictionary.Add(oVertex.ID,
				oInDegreeGraphMetricValues.Count - 1);

			iCalculations++;
		}

		// The following lists correspond to edge worksheet columns.  There
		// is a set of three lists for each of the edge's two vertices.

		List<GraphMetricValueWithID> oVertex1InDegreeGraphMetricValues =
			new List<GraphMetricValueWithID>();

		List<GraphMetricValueWithID> oVertex1OutDegreeGraphMetricValues =
			new List<GraphMetricValueWithID>();

		List<GraphMetricValueWithID> oVertex1DegreeGraphMetricValues =
			new List<GraphMetricValueWithID>();

		List<GraphMetricValueWithID> oVertex2InDegreeGraphMetricValues =
			new List<GraphMetricValueWithID>();

		List<GraphMetricValueWithID> oVertex2OutDegreeGraphMetricValues =
			new List<GraphMetricValueWithID>();

		List<GraphMetricValueWithID> oVertex2DegreeGraphMetricValues =
			new List<GraphMetricValueWithID>();

		// Loop through the edges.

		foreach (IEdge oEdge in graph.Edges)
		{
			if ( !TryGetRowID(oEdge, out iRowID) )
			{
				continue;
			}

			IVertex [] oEdgeVertices = oEdge.Vertices;
			IVertex oVertex1 = oEdgeVertices[0];
			IVertex oVertex2 = oEdgeVertices[1];
			Int32 iIndex;

			if ( oVertexIDDictionary.TryGetValue(oVertex1.ID, out iIndex) )
			{
				oVertex1InDegreeGraphMetricValues.Add(
					new GraphMetricValueWithID(
						iRowID,
						oInDegreeGraphMetricValues[iIndex].Value)
						);

				oVertex1OutDegreeGraphMetricValues.Add(
					new GraphMetricValueWithID(
						iRowID,
						oOutDegreeGraphMetricValues[iIndex].Value)
						);

				oVertex1DegreeGraphMetricValues.Add(
					new GraphMetricValueWithID(
						iRowID,
						oDegreeGraphMetricValues[iIndex].Value)
						);
			}

			if ( oVertexIDDictionary.TryGetValue(oVertex2.ID, out iIndex) )
			{
				oVertex2InDegreeGraphMetricValues.Add(
					new GraphMetricValueWithID(
						iRowID,
						oInDegreeGraphMetricValues[iIndex].Value)
						);

				oVertex2OutDegreeGraphMetricValues.Add(
					new GraphMetricValueWithID(
						iRowID,
						oOutDegreeGraphMetricValues[iIndex].Value)
						);

				oVertex2DegreeGraphMetricValues.Add(
					new GraphMetricValueWithID(
						iRowID,
						oDegreeGraphMetricValues[iIndex].Value)
						);
			}
		}

		// Figure out which columns to add.

		return ( FilterGraphMetricColumns(graphMetricUserSettings,
			oInDegreeGraphMetricValues, oOutDegreeGraphMetricValues,
			oDegreeGraphMetricValues, oVertex1InDegreeGraphMetricValues,
			oVertex1OutDegreeGraphMetricValues,
			oVertex1DegreeGraphMetricValues, oVertex2InDegreeGraphMetricValues,
			oVertex2OutDegreeGraphMetricValues, oVertex2DegreeGraphMetricValues
			) );
	}

    //*************************************************************************
    //  Method: CalculateVertexDegrees()
    //
    /// <summary>
    /// Calculates a vertex's in-degree and out-degree.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to calculate the in-degree and out-degree for.
    /// </param>
    ///
    /// <param name="iInDegree">
    /// Where the vertex's in-degree gets stored.
    /// </param>
    ///
    /// <param name="iOutDegree">
    /// Where the vertex's out-degree gets stored.
    /// </param>
    //*************************************************************************

    protected void
    CalculateVertexDegrees
    (
		IVertex oVertex,
		out Int32 iInDegree,
		out Int32 iOutDegree
    )
	{
		Debug.Assert(oVertex != null);
		AssertValid();

		iInDegree = 0;
		iOutDegree = 0;

		foreach (IEdge oIncidentEdge in oVertex.IncidentEdges)
		{
			IVertex [] aoVertices = oIncidentEdge.Vertices;

			if (aoVertices[0] == oVertex)
			{
				iOutDegree++;
			}
			else
			{
				iInDegree++;
			}
		}
	}

    //*************************************************************************
    //  Method: FilterGraphMetricColumns()
    //
    /// <summary>
    /// Determines which GraphMetricColumn objects to return to the caller.
    /// </summary>
    ///
    /// <param name="oGraphMetricUserSettings">
    /// The user's settings for calculating graph metrics.
    /// </param>
    ///
    /// <param name="oInDegreeGraphMetricValues">
	/// List of GraphMetricValue objects for the in-degree column on the vertex
	/// worksheet.
    /// </param>
    ///
    /// <param name="oOutDegreeGraphMetricValues">
	/// List of GraphMetricValue objects for the out-degree column on the
	/// vertex worksheet.
    /// </param>
    ///
    /// <param name="oDegreeGraphMetricValues">
	/// List of GraphMetricValue objects for the degree column on the vertex
	/// worksheet.
    /// </param>
    ///
    /// <param name="oVertex1InDegreeGraphMetricValues">
	/// List of GraphMetricValue objects for the vertex 1 in-degree column on
	/// the edge worksheet.
    /// </param>
    ///
    /// <param name="oVertex1OutDegreeGraphMetricValues">
	/// List of GraphMetricValue objects for the vertex 1 out-degree column on
	/// the edge worksheet.
    /// </param>
    ///
    /// <param name="oVertex1DegreeGraphMetricValues">
	/// List of GraphMetricValue objects for the vertex 1 degree column on the
	/// edge worksheet.
    /// </param>
    ///
    /// <param name="oVertex2InDegreeGraphMetricValues">
	/// List of GraphMetricValue objects for the vertex 2 in-degree column on
	/// the edge worksheet.
    /// </param>
    ///
    /// <param name="oVertex2OutDegreeGraphMetricValues">
	/// List of GraphMetricValue objects for the vertex 2 out-degree column on
	/// the edge worksheet.
    /// </param>
    ///
    /// <param name="oVertex2DegreeGraphMetricValues">
	/// List of GraphMetricValue objects for the vertex 2 degree column on the
	/// edge worksheet.
    /// </param>
    ///
	/// <returns>
	/// An array of GraphMetricColumn objects.
	/// </returns>
    //*************************************************************************

    protected GraphMetricColumn []
    FilterGraphMetricColumns
    (
		GraphMetricUserSettings oGraphMetricUserSettings,
		List<GraphMetricValueWithID> oInDegreeGraphMetricValues,
		List<GraphMetricValueWithID> oOutDegreeGraphMetricValues,
		List<GraphMetricValueWithID> oDegreeGraphMetricValues,
		List<GraphMetricValueWithID> oVertex1InDegreeGraphMetricValues,
		List<GraphMetricValueWithID> oVertex1OutDegreeGraphMetricValues,
		List<GraphMetricValueWithID> oVertex1DegreeGraphMetricValues,
		List<GraphMetricValueWithID> oVertex2InDegreeGraphMetricValues,
		List<GraphMetricValueWithID> oVertex2OutDegreeGraphMetricValues,
		List<GraphMetricValueWithID> oVertex2DegreeGraphMetricValues
    )
	{
		AssertValid();

		Debug.Assert(oGraphMetricUserSettings != null);
		Debug.Assert(oInDegreeGraphMetricValues != null);
		Debug.Assert(oOutDegreeGraphMetricValues != null);
		Debug.Assert(oDegreeGraphMetricValues != null);
		Debug.Assert(oVertex1InDegreeGraphMetricValues != null);
		Debug.Assert(oVertex1OutDegreeGraphMetricValues != null);
		Debug.Assert(oVertex1DegreeGraphMetricValues != null);
		Debug.Assert(oVertex2InDegreeGraphMetricValues != null);
		Debug.Assert(oVertex2OutDegreeGraphMetricValues != null);
		Debug.Assert(oVertex2DegreeGraphMetricValues != null);

		Boolean bCalculateInDegree =
			oGraphMetricUserSettings.CalculateInDegree;

		Boolean bCalculateOutDegree =
			oGraphMetricUserSettings.CalculateOutDegree;

		Boolean bCalculateDegree = oGraphMetricUserSettings.CalculateDegree;

		// Figure out which columns to add.

		List<GraphMetricColumn> oGraphMetricColumns =
			new List<GraphMetricColumn>();

		if (bCalculateInDegree)
		{
			oGraphMetricColumns.Add( new GraphMetricColumnWithID(
				WorksheetNames.Vertices, TableNames.Vertices,
				VertexTableColumnNames.InDegree, VertexInDegreeWidthChars,
				NumericFormat, oInDegreeGraphMetricValues.ToArray() ) );
		}

		if (bCalculateOutDegree)
		{
			oGraphMetricColumns.Add( new GraphMetricColumnWithID(
				WorksheetNames.Vertices, TableNames.Vertices,
				VertexTableColumnNames.OutDegree, VertexOutDegreeWidthChars,
				NumericFormat, oOutDegreeGraphMetricValues.ToArray() ) );
		}

		if (bCalculateDegree)
		{
			oGraphMetricColumns.Add( new GraphMetricColumnWithID(
				WorksheetNames.Vertices, TableNames.Vertices,
				VertexTableColumnNames.Degree, VertexDegreeWidthChars,
				NumericFormat, oDegreeGraphMetricValues.ToArray() ) );
		}

		if (bCalculateInDegree)
		{
			oGraphMetricColumns.Add( new GraphMetricColumnWithID(
				WorksheetNames.Edges, TableNames.Edges,
				EdgeTableColumnNames.Vertex1InDegree, EdgeInDegreeWidthChars,
				NumericFormat, oVertex1InDegreeGraphMetricValues.ToArray() ) );
		}

		if (bCalculateOutDegree)
		{
			oGraphMetricColumns.Add( new GraphMetricColumnWithID(
				WorksheetNames.Edges, TableNames.Edges,
				EdgeTableColumnNames.Vertex1OutDegree, EdgeOutDegreeWidthChars,
				NumericFormat, oVertex1OutDegreeGraphMetricValues.ToArray() ) );
		}

		if (bCalculateDegree)
		{
			oGraphMetricColumns.Add( new GraphMetricColumnWithID(
				WorksheetNames.Edges, TableNames.Edges,
				EdgeTableColumnNames.Vertex1Degree, EdgeDegreeWidthChars,
				NumericFormat, oVertex1DegreeGraphMetricValues.ToArray() ) );
		}

		if (bCalculateInDegree)
		{
			oGraphMetricColumns.Add( new GraphMetricColumnWithID(
				WorksheetNames.Edges, TableNames.Edges,
				EdgeTableColumnNames.Vertex2InDegree, EdgeInDegreeWidthChars,
				NumericFormat, oVertex2InDegreeGraphMetricValues.ToArray() ) );
		}

		if (bCalculateOutDegree)
		{

			oGraphMetricColumns.Add( new GraphMetricColumnWithID(
				WorksheetNames.Edges, TableNames.Edges,
				EdgeTableColumnNames.Vertex2OutDegree, EdgeOutDegreeWidthChars,
				NumericFormat, oVertex2OutDegreeGraphMetricValues.ToArray() ) );
		}

		if (bCalculateDegree)
		{
			oGraphMetricColumns.Add( new GraphMetricColumnWithID(
				WorksheetNames.Edges, TableNames.Edges,
				EdgeTableColumnNames.Vertex2Degree, EdgeDegreeWidthChars,
				NumericFormat, oVertex2DegreeGraphMetricValues.ToArray() ) );
		}

		return ( oGraphMetricColumns.ToArray() );
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

	/// Width of the in-degree column on the vertex worksheet, in characters.

	protected const Single VertexInDegreeWidthChars = 11.3F;

	/// Width of the out-degree column on the vertex worksheet, in characters.

	protected const Single VertexOutDegreeWidthChars = 12.9F;

	/// Width of the degree column on the vertex worksheet, in characters.

	protected const Single VertexDegreeWidthChars = 8.9F;


	/// Width of the vertex in-degree columns on the edge worksheet, in
	/// characters.

	protected const Single EdgeInDegreeWidthChars = 19.5F;

	/// Width of the out-degree columns on the edge worksheet, in characters.

	protected const Single EdgeOutDegreeWidthChars = 21.0F;

	/// Width of the degree columns on the edge worksheet, in characters.

	protected const Single EdgeDegreeWidthChars = 16.9F;


	/// Number format for all columns.

	protected const String NumericFormat = "0";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
