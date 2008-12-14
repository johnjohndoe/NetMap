
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Visualization;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ClusterWorksheetReader
//
/// <summary>
/// Class that knows how to read Excel worksheets containing cluster data.
/// </summary>
///
/// <remarks>
/// Call <see cref="ReadWorksheet" /> to read the cluster worksheets.
/// </remarks>
//*****************************************************************************

public class ClusterWorksheetReader : WorksheetReaderBase
{
    //*************************************************************************
    //  Constructor: ClusterWorksheetReader()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ClusterWorksheetReader" />
	/// class.
    /// </summary>
    //*************************************************************************

    public ClusterWorksheetReader()
    {
		// (Do nothing.)

		AssertValid();
    }

    //*************************************************************************
    //  Method: ReadWorksheet()
    //
    /// <summary>
	/// Reads the cluster worksheets and adds the cluster data to a graph.
    /// </summary>
    ///
    /// <param name="workbook">
	/// Workbook containing the graph data.
    /// </param>
	///
    /// <param name="readWorkbookContext">
	/// Provides access to objects needed for converting an Excel workbook to a
	/// NodeXL graph.
    /// </param>
	///
    /// <param name="graph">
	/// Graph to add cluster data to.
    /// </param>
	///
    /// <remarks>
	/// If the cluster worksheets in <paramref name="workbook" /> contain valid
	/// cluster data, the data is added to <paramref name="graph" />.
	/// Otherwise, a <see cref="WorkbookFormatException" /> is thrown.
    /// </remarks>
    //*************************************************************************

    public void
    ReadWorksheet
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
		ReadWorkbookContext readWorkbookContext,
		IGraph graph
    )
    {
		Debug.Assert(workbook != null);
		Debug.Assert(readWorkbookContext != null);
		Debug.Assert(graph != null);
        AssertValid();

		// Attempt to get the optional tables that contain cluster data.

		ListObject oClusterTable, oClusterVertexTable;

		if (
			ExcelUtil.TryGetTable(workbook, WorksheetNames.Clusters,
				TableNames.Clusters, out oClusterTable)
			&&
			ExcelUtil.TryGetTable(workbook, WorksheetNames.ClusterVertices,
				TableNames.ClusterVertices, out oClusterVertexTable)
			)
		{
			// Add the cluster data in the tables to the graph.

			AddClusterTablesToGraph(oClusterTable, oClusterVertexTable,
				readWorkbookContext, graph);
		}
    }

    //*************************************************************************
    //  Method: AddClusterTablesToGraph()
    //
    /// <summary>
	/// Adds the contents of the cluster tables to a NodeXL graph.
    /// </summary>
    ///
    /// <param name="oClusterTable">
	/// Table that contains the cluster data.
    /// </param>
	///
    /// <param name="oClusterVertexTable">
	/// Table that contains the cluster-vertex data.
    /// </param>
	///
    /// <param name="oReadWorkbookContext">
	/// Provides access to objects needed for converting an Excel workbook to a
	/// NodeXL graph.
    /// </param>
	///
    /// <param name="oGraph">
	/// Graph to add cluster data to.
    /// </param>
    //*************************************************************************

	protected void
	AddClusterTablesToGraph
	(
		ListObject oClusterTable,
		ListObject oClusterVertexTable,
		ReadWorkbookContext oReadWorkbookContext,
		IGraph oGraph
	)
	{
		Debug.Assert(oClusterTable != null);
		Debug.Assert(oClusterVertexTable != null);
		Debug.Assert(oReadWorkbookContext != null);
		Debug.Assert(oGraph != null);
		AssertValid();

		// Get the ranges that contain visible cluster data.  If a table is
		// filtered, a range may contain multiple areas.

        Range oVisibleClusterRange, oVisibleClusterVertexRange;

		if (
			!ExcelUtil.TryGetVisibleTableRange(
				oClusterTable, out oVisibleClusterRange)
			||
			!ExcelUtil.TryGetVisibleTableRange(
				oClusterVertexTable, out oVisibleClusterVertexRange)
			)
		{
			// There is no visible cluster data.

			return;
		}

		// Get the indexes of the columns within the tables.

		ClusterTableColumnIndexes oClusterTableColumnIndexes =
			GetClusterTableColumnIndexes(oClusterTable);

		ClusterVertexTableColumnIndexes oClusterVertexTableColumnIndexes =
			GetClusterVertexTableColumnIndexes(oClusterVertexTable);

		if (
			oClusterTableColumnIndexes.ClusterName == NoSuchColumn ||
			oClusterTableColumnIndexes.VertexColor == NoSuchColumn ||
			oClusterTableColumnIndexes.VertexShape == NoSuchColumn ||
			oClusterVertexTableColumnIndexes.ClusterName == NoSuchColumn ||
			oClusterVertexTableColumnIndexes.VertexName == NoSuchColumn
			)
		{
			return;
		}

		// Create a dictionary from the cluster table.  The key is the cluster
		// name and the value is a ClusterInformation object for the cluster.

		Dictionary<String, ClusterInformation> oClusterNameDictionary =
			ReadClusterTable(oVisibleClusterRange, oClusterTableColumnIndexes);

		// Read the cluster-vertex table and add the cluster-vertex information
		// to the graph.

		ReadClusterVertexTable(oVisibleClusterVertexRange,
			oClusterVertexTableColumnIndexes, oClusterNameDictionary,
			oReadWorkbookContext.VertexNameDictionary, oGraph);
	}

    //*************************************************************************
    //  Method: GetClusterTableColumnIndexes()
    //
    /// <summary>
	/// Gets the one-based indexes of the columns within the cluster table.
    /// </summary>
    ///
    /// <param name="clusterTable">
	/// Table that contains the cluster data.
    /// </param>
    ///
	/// <returns>
	/// The column indexes, as an <see cref="ClusterTableColumnIndexes" />.
	/// </returns>
    //*************************************************************************

	public ClusterTableColumnIndexes
	GetClusterTableColumnIndexes
	(
		ListObject clusterTable
	)
	{
		Debug.Assert(clusterTable != null);
		AssertValid();

		ClusterTableColumnIndexes oClusterTableColumnIndexes =
			new ClusterTableColumnIndexes();

		oClusterTableColumnIndexes.ClusterName = GetTableColumnIndex(
			clusterTable, ClusterTableColumnNames.Name, false);

		oClusterTableColumnIndexes.VertexColor = GetTableColumnIndex(
			clusterTable, ClusterTableColumnNames.VertexColor, false);

		oClusterTableColumnIndexes.VertexShape = GetTableColumnIndex(
			clusterTable, ClusterTableColumnNames.VertexShape, false);

		return (oClusterTableColumnIndexes);
	}

    //*************************************************************************
    //  Method: GetClusterVertexTableColumnIndexes()
    //
    /// <summary>
	/// Gets the one-based indexes of the columns within the cluster-vertex
	/// table.
    /// </summary>
    ///
    /// <param name="clusterVertexTable">
	/// Table that contains the cluster-vertex data.
    /// </param>
    ///
	/// <returns>
	/// The column indexes, as an <see
	/// cref="ClusterVertexTableColumnIndexes" />.
	/// </returns>
    //*************************************************************************

	public ClusterVertexTableColumnIndexes
	GetClusterVertexTableColumnIndexes
	(
		ListObject clusterVertexTable
	)
	{
		Debug.Assert(clusterVertexTable != null);
		AssertValid();

		ClusterVertexTableColumnIndexes oClusterVertexTableColumnIndexes =
			new ClusterVertexTableColumnIndexes();

		oClusterVertexTableColumnIndexes.ClusterName = GetTableColumnIndex(
			clusterVertexTable, ClusterVertexTableColumnNames.ClusterName,
			false);

		oClusterVertexTableColumnIndexes.VertexName = GetTableColumnIndex(
			clusterVertexTable, ClusterVertexTableColumnNames.VertexName,
			false);

		return (oClusterVertexTableColumnIndexes);
	}

    //*************************************************************************
    //  Method: ReadClusterTable()
    //
    /// <summary>
	/// Reads the cluster table.
    /// </summary>
    ///
    /// <param name="oVisibleClusterRange">
	/// Visible range within the cluster table.  May contain multiple areas.
    /// </param>
    ///
    /// <param name="oClusterTableColumnIndexes">
	/// One-based indexes of the columns within the cluster table.
    /// </param>
    ///
	/// <returns>
	/// A dictionary.  The key is the cluster name and the value is a
	/// ClusterInformation object for the cluster.
	/// </returns>
    //*************************************************************************

	protected Dictionary<String, ClusterInformation>
	ReadClusterTable
	(
		Range oVisibleClusterRange,
		ClusterTableColumnIndexes oClusterTableColumnIndexes
	)
	{
		Debug.Assert(oVisibleClusterRange != null);
		Debug.Assert(oClusterTableColumnIndexes != null);
		AssertValid();

		Dictionary<String, ClusterInformation> oClusterNameDictionary =
			new Dictionary<String, ClusterInformation>();

		ColorConverter2 oColorConverter2 = new ColorConverter2();

		// Loop through the areas, and split each area into subranges if the
		// area contains too many rows.

		foreach ( Range oClusterSubrange in
			ExcelRangeSplitter.SplitRange(oVisibleClusterRange) )
		{
			Object [,] aoClusterValues =
				ExcelUtil.GetRangeValues(oClusterSubrange);

			// Loop through the rows.

			Int32 iRows = oClusterSubrange.Rows.Count;

			for (Int32 iRowOneBased = 1; iRowOneBased <= iRows; iRowOneBased++)
			{
				// Get the cluster information.

				String sClusterName;
				Color oVertexColor;
				VertexDrawer.VertexShape eVertexShape;

				if (
					!ExcelUtil.TryGetNonEmptyStringFromCell(aoClusterValues,
						iRowOneBased, oClusterTableColumnIndexes.ClusterName,
						out sClusterName)
					||
					!TryGetColor(oClusterSubrange, aoClusterValues,
						iRowOneBased, oClusterTableColumnIndexes.VertexColor,
						oColorConverter2, out oVertexColor)
					||
					!TryGetVertexShape(oClusterSubrange, aoClusterValues,
						iRowOneBased, oClusterTableColumnIndexes.VertexShape,
						out eVertexShape)
					)
				{
					continue;
				}

				// Add the cluster information to the dictionary.

				ClusterInformation oClusterInformation =
					new ClusterInformation();

				oClusterInformation.VertexColor = oVertexColor;
				oClusterInformation.VertexShape = eVertexShape;

				try
				{
					oClusterNameDictionary.Add(
						sClusterName, oClusterInformation);
				}
				catch (ArgumentException)
				{
					Range oInvalidCell =
                        (Range)oClusterSubrange.Cells[iRowOneBased,
							oClusterTableColumnIndexes.ClusterName];

					OnWorkbookFormatError( String.Format(

						"The cell {0} contains a duplicate cluster name."
						+ "  There can't be two rows with the same cluster"
						+ " name."
						,
						ExcelUtil.GetRangeAddress(oInvalidCell)
						),

						oInvalidCell
					);
				}
			}
		}

		return (oClusterNameDictionary);
	}

    //*************************************************************************
    //  Method: ReadClusterVertexTable()
    //
    /// <summary>
	/// Reads the cluster-vertex table.
    /// </summary>
    ///
    /// <param name="oVisibleClusterVertexRange">
	/// Visible range within the cluster-vertex table.  May contain multiple
	/// areas.
    /// </param>
    ///
    /// <param name="oClusterVertexTableColumnIndexes">
	/// One-based indexes of the columns within the cluster-vertex table.
    /// </param>
    ///
    /// <param name="oVertexNameDictionary">
	/// The key is the vertex name from the edge or vertex worksheet and the
	/// value is the IVertex object.
    /// </param>
    ///
    /// <param name="oClusterNameDictionary">
	/// The key is the cluster name and the value is the ClusterInformation
	/// object for the cluster.
    /// </param>
    ///
    /// <param name="oGraph">
	/// Graph to add cluster data to.
    /// </param>
	///
	/// <returns>
	/// A dictionary.  The key is the cluster name and the value is the
	/// ClusterInformation object for the cluster.
	/// </returns>
    //*************************************************************************

	protected void
	ReadClusterVertexTable
	(
		Range oVisibleClusterVertexRange,
		ClusterVertexTableColumnIndexes oClusterVertexTableColumnIndexes,
		Dictionary<String, ClusterInformation> oClusterNameDictionary,
		Dictionary<String, IVertex> oVertexNameDictionary,
		IGraph oGraph
	)
	{
		Debug.Assert(oVisibleClusterVertexRange != null);
		Debug.Assert(oClusterVertexTableColumnIndexes != null);
		Debug.Assert(oClusterNameDictionary != null);
		Debug.Assert(oVertexNameDictionary != null);
		Debug.Assert(oGraph != null);
		AssertValid();

		// Loop through the areas, and split each area into subranges if the
		// area contains too many rows.

		foreach ( Range oClusterVertexSubrange in
			ExcelRangeSplitter.SplitRange(oVisibleClusterVertexRange) )
		{
			Object [,] aoClusterVertexValues =
				ExcelUtil.GetRangeValues(oClusterVertexSubrange);

			// Loop through the rows.

			Int32 iRows = oClusterVertexSubrange.Rows.Count;

			for (Int32 iRowOneBased = 1; iRowOneBased <= iRows; iRowOneBased++)
			{
				// Get the cluster-vertex information from the row.

				String sClusterName, sVertexName;

				if (
					!ExcelUtil.TryGetNonEmptyStringFromCell(
						aoClusterVertexValues, iRowOneBased,
						oClusterVertexTableColumnIndexes.ClusterName,
						out sClusterName)
					||
					!ExcelUtil.TryGetNonEmptyStringFromCell(
						aoClusterVertexValues, iRowOneBased,
						oClusterVertexTableColumnIndexes.VertexName,
						out sVertexName)
					)
				{
					continue;
				}

				// Get the cluster information for the vertex and store the
				// cluster information in the vertex.

				ClusterInformation oClusterInformation;
				IVertex oVertex;

				if (
					!oClusterNameDictionary.TryGetValue(sClusterName,
						out oClusterInformation)
					||
					!oVertexNameDictionary.TryGetValue(sVertexName,
						out oVertex)
					)
				{
					continue;
				}

				oVertex.SetValue(ReservedMetadataKeys.PerColor,
					oClusterInformation.VertexColor);

				oVertex.SetValue(ReservedMetadataKeys.PerVertexShape,
					oClusterInformation.VertexShape);
			}
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


	//*************************************************************************
	//  Embedded class: ClusterTableColumnIndexes
	//
	/// <summary>
	/// Contains the one-based indexes of the columns in the optional cluster
	/// table.
	/// </summary>
	//*************************************************************************

	public class ClusterTableColumnIndexes
	{
		/// Name of the cluster.

		public Int32 ClusterName;

		/// Color of the vertices in the cluster.

		public Int32 VertexColor;

		/// Shape of the vertices in the cluster.

		public Int32 VertexShape;
	}


	//*************************************************************************
	//  Embedded class: ClusterVertexTableColumnIndexes
	//
	/// <summary>
	/// Contains the one-based indexes of the columns in the optional
	/// cluster-vertex table.
	/// </summary>
	//*************************************************************************

	public class ClusterVertexTableColumnIndexes
	{
		/// Name of the cluster.

		public Int32 ClusterName;

		/// Name of the vertex in the cluster.

		public Int32 VertexName;
	}


	//*************************************************************************
	//  Embedded class: ClusterInformation
	//
	/// <summary>
	/// Contains information about one cluster.
	/// </summary>
	//*************************************************************************

	protected class ClusterInformation
	{
		/// Color to use for all vertices in the cluster.

		public Color VertexColor;

		/// Shape to use for all vertices in the cluster.

		public VertexDrawer.VertexShape VertexShape;
	}
}

}
