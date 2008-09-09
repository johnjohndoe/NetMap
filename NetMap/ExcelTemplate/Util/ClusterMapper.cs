
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;
using Microsoft.NetMap.Visualization;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: ClusterMapper
//
/// <summary>
/// Maps vertex clusters in the cluster and cluster vertices worksheets to
/// vertex attribute columns in the vertex worksheet.
/// </summary>
///
/// <remarks>
/// Use <see cref="MapClusters" /> to map the vertex clusters.
/// </remarks>
//*****************************************************************************

public class ClusterMapper : Object
{
    //*************************************************************************
    //  Constructor: ClusterMapper()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ClusterMapper" /> class.
    /// </summary>
    //*************************************************************************

    public ClusterMapper()
    {
        // (Do nothing.)

		AssertValid();
    }

    //*************************************************************************
    //  Method: MapClusters()
    //
    /// <summary>
	/// Maps vertex clusters in the cluster and cluster vertices worksheets to
	/// vertex attribute columns in the vertex worksheet.
    /// </summary>
    ///
    /// <param name="workbook">
	/// Workbook containing the vertex clusters.
    /// </param>
    ///
    /// <remarks>
	/// This method implements a poor-man's visualization of vertex clusters.
	/// It reads the clusters in the cluster and cluster vertices worksheets,
	/// which should have already been populated, and then sets the shape and
	/// color of all the vertices in each cluster to the same values.
	/// Cluster 1 vertices might all be blue circles, while cluster 2 vertices
	/// might be red triangles.
    /// </remarks>
    //*************************************************************************

    public void
    MapClusters
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
		Debug.Assert(workbook != null);
        AssertValid();

		ListObject oClusterVertexTable;
		Range oClusterNameColumnData = null;
		Range oVertexNameColumnData = null;

		// Get the columns from the cluster vertex worksheet.

		if (
			!ExcelUtil.TryGetTable(workbook, WorksheetNames.ClusterVertices,
				TableNames.ClusterVertices, out oClusterVertexTable) 
			||
			!ExcelUtil.TryGetTableColumnData(oClusterVertexTable,
				ClusterVerticesTableColumnNames.ClusterName,
				out oClusterNameColumnData)
			||
			!ExcelUtil.TryGetTableColumnData(oClusterVertexTable,
				ClusterVerticesTableColumnNames.VertexName,
				out oVertexNameColumnData)
			)
		{
			// Nothing can be done without the columns.

			ErrorUtil.OnMissingColumn();
		}

		// Create a dictionary to map vertices to clusters.  The key is the
		// vertex name and the value is the cluster name.

		Dictionary<String,String> oVertexDictionary =
			new Dictionary<String, String>();

		// Create a dictionary to map clusters to vertex attributes.  The key
		// is the cluster name and the value is a VertexAttributes object.

		Dictionary<String, VertexAttributes> oClusterDictionary =
			new Dictionary<String, VertexAttributes>();

		// Populate the dictionaries by reading the cluster and vertex columns.

		foreach (Range oClusterNameSubrange in
			ExcelRangeSplitter.SplitRange(oClusterNameColumnData) )
		{
			Int32 iRows = oClusterNameSubrange.Rows.Count;

			Range oVertexNameSubrange = ExcelRangeSplitter.GetParallelSubrange(
				oClusterNameSubrange, oVertexNameColumnData.Column);

			Object [,] aoClusterNameValues =
				ExcelUtil.GetRangeValues(oClusterNameSubrange);

			Object [,] aoVertexNameValues =
				ExcelUtil.GetRangeValues(oVertexNameSubrange);

			String sClusterName, sVertexName;

			for (Int32 i = 1; i <= iRows; i++)
			{
				if (
					!ExcelUtil.TryGetNonEmptyStringFromCell(aoClusterNameValues,
						i, 1, out sClusterName)
					||
					!ExcelUtil.TryGetNonEmptyStringFromCell(aoVertexNameValues,
						i, 1, out sVertexName)
					)
				{
					continue;
				}

				oVertexDictionary[sVertexName] = sClusterName;

				// The attributes in the VertexAttributes objects will get set
				// later.

				oClusterDictionary[sClusterName] = new VertexAttributes();
			}
		}

		// Set the attributes in the VertexAttributes object for each cluster.

		CreateVertexAttributes(oClusterDictionary);

		// Populate the vertex worksheet with the cluster attributes.

		PopulateVertexWorksheet(workbook, oVertexDictionary,
			oClusterDictionary);
    }

    //*************************************************************************
    //  Method: CreateVertexAttributes()
    //
    /// <summary>
	/// Sets the attributes in the VertexAttributes object for each cluster.
    /// </summary>
    ///
    /// <param name="oClusterDictionary">
	/// Maps clusters to vertex attributes.  The key is the cluster name and
	/// the value is a VertexAttributes object.
    /// </param>
    ///
    /// <remarks>
	/// For each key in <paramref name="oClusterDictionary" />, this method
	/// sets the attributes in the VertexAttributes object stored in the key's
	/// value.
    /// </remarks>
    //*************************************************************************

	protected void
	CreateVertexAttributes
	(
		Dictionary<String, VertexAttributes> oClusterDictionary
	)
	{
		Debug.Assert(oClusterDictionary != null);
		AssertValid();

		Int32 iCluster = 0;
		Int32 iTotalClusters = oClusterDictionary.Count;

		foreach (String sKey in oClusterDictionary.Keys)
		{
			VertexAttributes oVertexAttributes = oClusterDictionary[sKey];

			Color oColor;
			VertexDrawer.VertexShape eShape;

			GetVertexAttributes(iCluster, iTotalClusters, out oColor,
				out eShape);

			oVertexAttributes.Color = oColor;
			oVertexAttributes.Shape = eShape;

			iCluster++;
		}
	}

    //*************************************************************************
    //  Method: PopulateVertexWorksheet()
    //
    /// <summary>
	/// Populates the vertex worksheet with the cluster attributes.
    /// </summary>
    ///
    /// <param name="oWorkbook">
	/// Workbook containing the vertex clusters.
    /// </param>
    ///
    /// <param name="oVertexDictionary">
	/// Maps vertices to clusters.  The key is the vertex name and the value is
	/// the cluster name.
    /// </param>
    ///
    /// <param name="oClusterDictionary">
	/// Maps clusters to vertex attributes.  The key is the cluster name and
	/// the value is a VertexAttributes object.
    /// </param>
    //*************************************************************************

	protected void
	PopulateVertexWorksheet
	(
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
		Dictionary<String, String> oVertexDictionary,
		Dictionary<String, VertexAttributes> oClusterDictionary
	)
	{
		Debug.Assert(oVertexDictionary != null);
		Debug.Assert(oClusterDictionary != null);
		AssertValid();

		ListObject VertexTable;
		Range oVertexNameColumnData = null;
		Range oShapeColumnData = null;
		Range oColorColumnData = null;

		// Get the columns from the vertex table.

		if (
			!ExcelUtil.TryGetTable(oWorkbook, WorksheetNames.Vertices,
				TableNames.Vertices, out VertexTable) 
			||
			!ExcelUtil.TryGetTableColumnData(VertexTable,
				VertexTableColumnNames.VertexName, out oVertexNameColumnData)
			||
			!ExcelUtil.TryGetTableColumnData(VertexTable,
				VertexTableColumnNames.Shape, out oShapeColumnData)
			||
			!ExcelUtil.TryGetTableColumnData(VertexTable,
				VertexTableColumnNames.Color, out oColorColumnData)
			)
		{
			// Nothing can be done without the columns.

			ErrorUtil.OnMissingColumn();
		}

		VertexShapeConverter oVertexShapeConverter =
			new VertexShapeConverter();

		ColorConverter oColorConverter = new ColorConverter();

		// Loop through the vertex names.

		foreach (Range oVertexNameSubrange in
			ExcelRangeSplitter.SplitRange(oVertexNameColumnData) )
		{
			Int32 iRows = oVertexNameSubrange.Rows.Count;

			Range oShapeSubrange = ExcelRangeSplitter.GetParallelSubrange(
				oVertexNameSubrange, oShapeColumnData.Column);

			Range oColorSubrange = ExcelRangeSplitter.GetParallelSubrange(
				oVertexNameSubrange, oColorColumnData.Column);

			Object [,] aoVertexNameValues =
				ExcelUtil.GetRangeValues(oVertexNameSubrange);

			Object [,] aoShapeValues = ExcelUtil.GetSingleColumn2DArray(iRows);
			Object [,] aoColorValues = ExcelUtil.GetSingleColumn2DArray(iRows);

			// Loop through the rows.

			for (Int32 i = 1; i <= iRows; i++)
			{
				// Retrieve the VertexAttributes object for the vertex.  Don't
				// assume that each vertex is in a cluster.

				String sVertexName, sClusterName;

				if (
					!ExcelUtil.TryGetNonEmptyStringFromCell(
						aoVertexNameValues, i, 1, out sVertexName)
					||
					!oVertexDictionary.TryGetValue(sVertexName,
						out sClusterName)
					)
				{
					continue;
				}

				// It's safe to use Dictionary.Item() here as opposed to
				// TryGetValue(), because this class created both dictionaries.

				VertexAttributes oVertexAttributes =
					oClusterDictionary[sClusterName];

				aoShapeValues[i, 1] = oVertexShapeConverter.GraphToWorkbook(
					oVertexAttributes.Shape);

				// Write the color in a format that is understood by
				// ColorConverter.ConvertFromString(), which is what
				// WorksheetReaderBase uses.

				aoColorValues[i, 1] = oColorConverter.ConvertToString(
					oVertexAttributes.Color);
			}

			oShapeSubrange.set_Value(Missing.Value, aoShapeValues);
			oColorSubrange.set_Value(Missing.Value, aoColorValues);
		}
	}

	//*************************************************************************
	//	Method: GetVertexAttributes()
	//
	/// <summary>
	/// Gets the vertex attributes for a specified cluster.
	/// </summary>
	///
	/// <param name="iCluster">
	/// Zero-based cluster index.
	/// </param>
	///
	/// <param name="iTotalClusters">
	/// Total number of clusters.
	/// </param>
	///
	/// <param name="oColor">
	/// Where the color gets stored.
	/// </param>
	///
	/// <param name="eShape">
	/// Where the shape gets stored.
	/// </param>
	//*************************************************************************

	protected void
	GetVertexAttributes
	(
		Int32 iCluster,
		Int32 iTotalClusters,
		out Color oColor,
		out VertexDrawer.VertexShape eShape
	)
	{
		Debug.Assert(iCluster >= 0);
		Debug.Assert(iTotalClusters > 0);
		Debug.Assert(iCluster < iTotalClusters);

		// Algorithm:
		//
		// Cycle through the set of hues with one shape, then cycle through the
		// set of hues with the next shape, and so on.  When all shapes have
		// been used, repeat the process with another saturation.

		Int32 iHues = Hues.Length;
		Int32 iShapes = Shapes.Length;

		Int32 iSaturations = (Int32)Math.Ceiling(
			(Single)iTotalClusters / (Single)(iHues * iShapes) );

		Int32 iHueIndex = iCluster % iHues;
		Int32 iDividend = iCluster / iHues;
		Int32 iShapeIndex = iDividend % iShapes;
		Int32 iSaturationIndex = iDividend / iShapes;

		Single fHue = Hues[iHueIndex];

		Single fSaturation = StartSaturation
			+ ( (Single)iSaturationIndex /  (Single)iSaturations )
				* (EndSaturation - StartSaturation);

		oColor = ColorUtil.HsbToRgb(fHue, fSaturation, Brightness);
		eShape = Shapes[iShapeIndex];
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
		// (Do nothing.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	/// Hues to cycle through when creating vertex colors.  Hues range from
	/// 0 to 360.

	protected static Single [] Hues = new Single[] {
		240F,  // Red
		0F,    // Blue
		200F,  // Orange
		120F,  // Green
		300F,  // Magenta
		180F,  // Yellow
		60F,   // Cyan
		};

	/// Shapes to cycle through.

	protected static VertexDrawer.VertexShape [] Shapes =
		new VertexDrawer.VertexShape[] {

		VertexDrawer.VertexShape.Disk,
		VertexDrawer.VertexShape.SolidSquare,
		VertexDrawer.VertexShape.SolidDiamond,
		VertexDrawer.VertexShape.SolidTriangle,
		VertexDrawer.VertexShape.Sphere,
		// VertexDrawer.VertexShape.Circle,
		// VertexDrawer.VertexShape.Square,
		// VertexDrawer.VertexShape.Diamond,
		// VertexDrawer.VertexShape.Triangle,
		};

	/// Saturation to start with.  Saturation ranges from 0 to 1.0, where 0 is
	/// grayscale and 1.0 is the most saturated.

	protected const Single StartSaturation = 1.0F;

	/// Saturation to end with.

	protected const Single EndSaturation = 0.2F;

	/// Brightness to use.  Brightness ranges from 0 to 1.0, where 0 represents
	/// black and 1.0 represents white.

	protected const Single Brightness = 0.5F;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)


	//*************************************************************************
	//  Embedded class: VertexAttributes
	//
	/// <summary>
	/// Contains the vertex attributes to use for one cluster.
	/// </summary>
	//*************************************************************************

	protected class VertexAttributes : Object
	{
		/// The shape of each vertex in the cluster.

		public VertexDrawer.VertexShape Shape;

		/// The color of each vertex in the cluster.

		public Color Color;
	}
}

}
