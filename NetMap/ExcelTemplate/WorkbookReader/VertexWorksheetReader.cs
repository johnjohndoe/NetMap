
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using Microsoft.NetMap.Core;
using Microsoft.NetMap.Visualization;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: VertexWorksheetReader
//
/// <summary>
/// Class that knows how to read an Excel worksheet containing vertex data.
/// </summary>
///
/// <remarks>
/// Call <see cref="ReadWorksheet" /> to read the worksheet.
/// </remarks>
//*****************************************************************************

public class VertexWorksheetReader : WorksheetReaderBase
{
    //*************************************************************************
    //  Constructor: VertexWorksheetReader()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexWorksheetReader" />
	/// class.
    /// </summary>
    //*************************************************************************

    public VertexWorksheetReader()
    {
		// (Do nothing.)

		AssertValid();
    }

	//*************************************************************************
	//  Enum: Visibility
	//
	/// <summary>
	/// Specifies the visibility of a vertex.
	/// </summary>
	//*************************************************************************

	public enum
	Visibility
	{
		/// <summary>
		/// If the vertex is part of an edge, show it using the specified
		/// vertex attributes.  Otherwise, ignore the vertex row.  This is the
		/// default.
		/// </summary>

		ShowIfInAnEdge,

		/// <summary>
		/// Skip the vertex row and any edge rows that include the vertex.  Do
		/// not read them into the graph.
		/// </summary>

		Skip,

		/// <summary>
		/// If the vertex is part of an edge, hide it and its incident edges.
		/// Otherwise, ignore the vertex row.
		/// </summary>

		Hide,

		/// <summary>
		/// Show the vertex using the specified attributes regardless of
		/// whether it is part of an edge.
		/// </summary>

		Show,
	}

    //*************************************************************************
    //  Method: ReadWorksheet()
    //
    /// <summary>
	/// Reads the vertex worksheet and adds the vertex data to a graph.
    /// </summary>
    ///
    /// <param name="workbook">
	/// Workbook containing the graph data.
    /// </param>
	///
    /// <param name="readWorkbookContext">
	/// Provides access to objects needed for converting an Excel workbook to a
	/// NetMap graph.
    /// </param>
	///
	///
    /// <param name="graph">
	/// Graph to add vertex data to.
    /// </param>
	///
    /// <remarks>
	/// If the vertex worksheet in <paramref name="workbook" /> contains valid
	/// vertex data, the vertices in <paramref name="graph" /> are marked with
	/// metadata; any isolated vertices are added to <paramref
	/// name="graph" /> and <paramref
	/// name="readWorkbookContext" />.VertexNameDictionary; and any
	/// skipped vertices (and their incident edges) are removed from <paramref
	/// name="graph" />, <paramref
	/// name="readWorkbookContext" />.VertexNameDictionary, and
	/// <paramref name="readWorkbookContext" />.EdgeIDDictionary.  Otherwise, a
	/// <see cref="WorkbookFormatException" /> is thrown.
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

		// Attempt to get the optional table that contains vertex data.

		ListObject oVertexTable;

		if ( ExcelUtil.TryGetTable(workbook, WorksheetNames.Vertices,
			TableNames.Vertices, out oVertexTable) )
		{
			// Add the vertices in the table to the graph.

			AddVertexTableToGraph(oVertexTable, readWorkbookContext, graph);
		}
    }

    //*************************************************************************
    //  Method: GetVertexTableColumnIndexes()
    //
    /// <summary>
	/// Gets the one-based indexes of the columns within the table that
	/// contains the vertex data.
    /// </summary>
    ///
    /// <param name="oVertexTable">
	/// Table that contains the vertex data.
    /// </param>
    ///
	/// <returns>
	/// The column indexes, as a <see cref="VertexTableColumnIndexes" />.
	/// </returns>
    //*************************************************************************

	public VertexTableColumnIndexes
	GetVertexTableColumnIndexes
	(
		ListObject oVertexTable
	)
	{
		Debug.Assert(oVertexTable != null);
		AssertValid();

		VertexTableColumnIndexes oVertexTableColumnIndexes =
			new VertexTableColumnIndexes();

		oVertexTableColumnIndexes.VertexName = GetTableColumnIndex(
			oVertexTable, VertexTableColumnNames.VertexName, false);

		oVertexTableColumnIndexes.Color = GetTableColumnIndex(
			oVertexTable, VertexTableColumnNames.Color, false);

		oVertexTableColumnIndexes.Shape = GetTableColumnIndex(
			oVertexTable, VertexTableColumnNames.Shape, false);

		oVertexTableColumnIndexes.Radius = GetTableColumnIndex(
			oVertexTable, VertexTableColumnNames.Radius, false);

		oVertexTableColumnIndexes.ImageKey = GetTableColumnIndex(
			oVertexTable, VertexTableColumnNames.ImageKey, false);

		oVertexTableColumnIndexes.PrimaryLabel = GetTableColumnIndex(
			oVertexTable, VertexTableColumnNames.PrimaryLabel, false);

		oVertexTableColumnIndexes.SecondaryLabel = GetTableColumnIndex(
			oVertexTable, VertexTableColumnNames.SecondaryLabel, false);

		oVertexTableColumnIndexes.Alpha = GetTableColumnIndex(
			oVertexTable, VertexTableColumnNames.Alpha, false);

		oVertexTableColumnIndexes.ToolTip = GetTableColumnIndex(
			oVertexTable, VertexTableColumnNames.ToolTip, false);

		oVertexTableColumnIndexes.VertexDrawerPrecedence = GetTableColumnIndex(
			oVertexTable, VertexTableColumnNames.VertexDrawerPrecedence,
			false);

		oVertexTableColumnIndexes.Visibility = GetTableColumnIndex(
			oVertexTable, VertexTableColumnNames.Visibility, false);

		oVertexTableColumnIndexes.X = GetTableColumnIndex(
			oVertexTable, VertexTableColumnNames.X, false);

		oVertexTableColumnIndexes.Y = GetTableColumnIndex(
			oVertexTable, VertexTableColumnNames.Y, false);

		oVertexTableColumnIndexes.Locked = GetTableColumnIndex(
			oVertexTable, VertexTableColumnNames.Locked, false);

		oVertexTableColumnIndexes.ID = GetTableColumnIndex(
			oVertexTable, CommonTableColumnNames.ID, false);

		oVertexTableColumnIndexes.Marked = GetTableColumnIndex(
			oVertexTable, VertexTableColumnNames.Marked, false);

		return (oVertexTableColumnIndexes);
	}

    //*************************************************************************
    //  Method: AddVertexTableToGraph()
    //
    /// <summary>
	/// Adds the contents of the vertex table to a NetMap graph.
    /// </summary>
    ///
    /// <param name="oVertexTable">
	/// Table that contains the vertex data.
    /// </param>
	///
    /// <param name="oReadWorkbookContext">
	/// Provides access to objects needed for converting an Excel workbook to a
	/// NetMap graph.
    /// </param>
	///
    /// <param name="oGraph">
	/// Graph to add vertices to.
    /// </param>
    //*************************************************************************

    protected void
    AddVertexTableToGraph
    (
		ListObject oVertexTable,
		ReadWorkbookContext oReadWorkbookContext,
		IGraph oGraph
    )
    {
		Debug.Assert(oVertexTable != null);
		Debug.Assert(oReadWorkbookContext != null);
		Debug.Assert(oGraph != null);
        AssertValid();

		// Read the range that contains visible vertex data.  If the table is
		// filtered, the range may contain multiple areas.

        Range oVertexRange;

		if ( !ExcelUtil.TryGetVisibleTableRange(
            oVertexTable, out oVertexRange) )
		{
			// There is no visible vertex data.

			return;
		}

		// Get the indexes of the columns within the table.

		VertexTableColumnIndexes oVertexTableColumnIndexes =
			GetVertexTableColumnIndexes(oVertexTable);

		if (oVertexTableColumnIndexes.VertexName == NoSuchColumn)
		{
			// There are no vertex names.

			return;
		}

		// Get the one-based indexes of all the column pairs that are used to
		// add custom menu items to the vertex context menu in the graph.

		TableColumnAdder oTableColumnAdder = new TableColumnAdder();

		KeyValuePair<Int32, Int32> [] aoCustomMenuItemPairIndexes =
			oTableColumnAdder.GetColumnPairIndexes(oVertexTable,
				VertexTableColumnNames.CustomMenuItemTextBase,
				VertexTableColumnNames.CustomMenuItemActionBase);

		// Loop through the areas.

		foreach (Range oVertexRangeArea in oVertexRange.Areas)
		{
			if (oReadWorkbookContext.FillIDColumns)
			{
				// If the ID column exists, fill the rows within it that are
				// contained within the area with a sequence of unique IDs.

				FillIDColumn(oVertexTable, oVertexTableColumnIndexes.ID,
					oVertexRangeArea);
			}

			// Add the contents of the area to the graph.

			AddVertexRangeAreaToGraph(oVertexRangeArea,
				oVertexTableColumnIndexes, aoCustomMenuItemPairIndexes,
				oReadWorkbookContext, oGraph);
		}
    }

    //*************************************************************************
    //  Method: AddVertexRangeAreaToGraph()
    //
    /// <summary>
	/// Adds the contents of one area of the vertex range to a NetMap graph.
    /// </summary>
    ///
    /// <param name="oVertexRangeArea">
	/// One area of the range that contains vertex data.
    /// </param>
	///
    /// <param name="oVertexTableColumnIndexes">
	/// One-based indexes of the columns within the vertex table.
    /// </param>
    ///
    /// <param name="aoCustomMenuItemPairIndexes">
	/// Array of pairs of one-based column indexes, one array element for each
	/// pair of columns that are used to add custom menu items to the vertex
	/// context menu in the graph.  They key is the index of the custom menu
	/// item text and the value is the index of the custom menu item action.
    /// </param>
	///
    /// <param name="oReadWorkbookContext">
	/// Provides access to objects needed for converting an Excel workbook to a
	/// NetMap graph.
    /// </param>
    ///
    /// <param name="oGraph">
	/// Graph to add vertices to.
    /// </param>
	///
    /// <remarks>
	/// This method should be called once for each area in the range that
	/// contains vertex data.
    /// </remarks>
    //*************************************************************************

    protected void
    AddVertexRangeAreaToGraph
    (
		Range oVertexRangeArea,
		VertexTableColumnIndexes oVertexTableColumnIndexes,
		KeyValuePair<Int32, Int32> [] aoCustomMenuItemPairIndexes,
		ReadWorkbookContext oReadWorkbookContext,
		IGraph oGraph
    )
    {
		Debug.Assert(oVertexRangeArea != null);
		Debug.Assert(oVertexTableColumnIndexes.VertexName != NoSuchColumn);
		Debug.Assert(aoCustomMenuItemPairIndexes != null);
		Debug.Assert(oReadWorkbookContext != null);
		Debug.Assert(oGraph != null);
        AssertValid();

		IVertexCollection oVertices = oGraph.Vertices;
		Object [,] aoVertexValues = ExcelUtil.GetRangeValues(oVertexRangeArea);

		Dictionary<String, IVertex> oVertexNameDictionary =
			oReadWorkbookContext.VertexNameDictionary;

		Dictionary<Int32, IIdentityProvider> oEdgeIDDictionary =
			oReadWorkbookContext.EdgeIDDictionary;

		Dictionary<String, Image> oImageIDDictionary =
			oReadWorkbookContext.ImageIDDictionary;

		VertexVisibilityConverter oVertexVisibilityConverter =
			new VertexVisibilityConverter();

		// Loop through the rows.

		Int32 iRows = oVertexRangeArea.Rows.Count;

		for (Int32 iRowOneBased = 1; iRowOneBased <= iRows; iRowOneBased++)
		{
			// Get the name of the vertex.

			String sVertexName;

			if ( !ExcelUtil.TryGetNonEmptyStringFromCell(aoVertexValues,
					iRowOneBased, oVertexTableColumnIndexes.VertexName,
					out sVertexName) )
			{
				// There is no vertex name.  Skip the row.

				continue;
			}

			// If the vertex was added to the graph as part of an edge,
			// retrieve the vertex.

			IVertex oVertex;

			if ( !oVertexNameDictionary.TryGetValue(sVertexName, out oVertex) )
			{
				oVertex = null;
			}

			// Assume a default visibility.

			Visibility eVisibility = Visibility.ShowIfInAnEdge;

			String sVisibility;

			if (
				oVertexTableColumnIndexes.Visibility != NoSuchColumn
				&&
				ExcelUtil.TryGetNonEmptyStringFromCell(aoVertexValues,
					iRowOneBased, oVertexTableColumnIndexes.Visibility,
					out sVisibility)
				)
			{
				if ( !oVertexVisibilityConverter.TryWorkbookToGraph(
					sVisibility, out eVisibility) )
				{
					OnInvalidVisibility(oVertexRangeArea, iRowOneBased,
						oVertexTableColumnIndexes.Visibility);
				}
			}

			switch (eVisibility)
			{
				case Visibility.ShowIfInAnEdge:

					// If the vertex is part of an edge, show it using the
					// specified vertex attributes.  Otherwise, skip the vertex
					// row.

					if (oVertex == null)
					{
						continue;
					}

					break;

				case Visibility.Skip:

					// Skip the vertex row and any edge rows that include the
					// vertex.  Do not read them into the graph.

					if (oVertex != null)
					{
						// Remove the vertex and its incident edges from the
						// graph and dictionaries.

						foreach (IEdge oIncidentEdge in oVertex.IncidentEdges)
						{
							if (oIncidentEdge.Tag is Int32)
							{
								oEdgeIDDictionary.Remove(
									(Int32)oIncidentEdge.Tag);
							}
						}

						oVertexNameDictionary.Remove(sVertexName);

						oVertices.Remove(oVertex);

                        // (The vertex doesn't get added to
						// ReadWorkbookContext.VertexIDDictionary until after
						// this switch statement, so it doesn't need to be
						// removed from that dictionary.)
					}

					continue;

				case Visibility.Hide:

					// If the vertex is part of an edge, hide it and its
					// incident edges.  Otherwise, skip the vertex row.

					if (oVertex == null)
					{
						continue;
					}

					// Hide the vertex and its incident edges.

					oVertex.SetValue(ReservedMetadataKeys.Hide, null);

					foreach (IEdge oIncidentEdge in oVertex.IncidentEdges)
					{
						oIncidentEdge.SetValue(ReservedMetadataKeys.Hide,
							null);
					}

					break;

				case Visibility.Show:

					// Show the vertex using the specified attributes
					// regardless of whether it is part of an edge.

					if (oVertex == null)
					{
						oVertex = CreateVertex(
							sVertexName, oVertices, oVertexNameDictionary);
					}

					break;

				default:

					Debug.Assert(false);
					break;
			}

			Debug.Assert(oVertex != null);

			// If there is an ID column, add the vertex to the vertex ID
			// dictionary and set the vertex's Tag to the ID.

			if (oVertexTableColumnIndexes.ID != NoSuchColumn)
			{
				AddToIDDictionary(oVertexRangeArea, aoVertexValues,
					iRowOneBased, oVertexTableColumnIndexes.ID, oVertex,
					oReadWorkbookContext.VertexIDDictionary);
			}

			if (!oReadWorkbookContext.IgnoreVertexLocations)
			{
			    Boolean bLocationSpecified = false;

			    // If there are X and Y columns and a location has been
				// specified for the vertex, set the vertex's location.

			    if (oVertexTableColumnIndexes.X != NoSuchColumn &&
				    oVertexTableColumnIndexes.Y != NoSuchColumn)
			    {
				    bLocationSpecified = CheckForLocation(
					    oVertexRangeArea, aoVertexValues, iRowOneBased,
					    oVertexTableColumnIndexes.X,
						oVertexTableColumnIndexes.Y,
						oReadWorkbookContext.VertexLocationConverter,
						oVertex);
			    }

			    // If there is a lock column and a lock flag has been specified
				// for the vertex, set the vertex's lock flag.  ("Lock" means
				// "prevent the layout algorithm from moving the vertex.")

			    if (oVertexTableColumnIndexes.Locked != NoSuchColumn)
			    {
				    CheckForLocked(oVertexRangeArea, aoVertexValues,
						iRowOneBased, oVertexTableColumnIndexes.Locked,
						bLocationSpecified, oVertex);
			    }
			}

			// If there is a mark column and a mark flag has been specified for
			// the vertex, set the vertex's mark flag.  ("Marking" is something
			// the user does for himself.  A marked vertex doesn't behave
			// differently from an unmarked vertex.)

			if (oVertexTableColumnIndexes.Marked != NoSuchColumn)
			{
				CheckForMarked(oVertexRangeArea, aoVertexValues, iRowOneBased,
					oVertexTableColumnIndexes.Marked, oVertex);
			}

			// If there is at least one pair of columns that are used to add
			// custom menu items to the vertex context menu in the graph, and
			// if custom menu items have been specified for the vertex, store
			// the custom menu item information in the vertex.

			if (aoCustomMenuItemPairIndexes.Length > 0)
			{
				CheckForCustomMenuItems(oVertexRangeArea, aoVertexValues,
					iRowOneBased, aoCustomMenuItemPairIndexes, oVertex);
			}

			// If there is an alpha column and the alpha for this row isn't
			// empty, set the vertex's alpha.

			if (oVertexTableColumnIndexes.Alpha != NoSuchColumn)
			{
				CheckForAlpha(oVertexRangeArea, aoVertexValues, iRowOneBased,
					oVertexTableColumnIndexes.Alpha, oVertex);
			}

			// If there is a secondary label column and the secondary label for
			// this row isn't empty, set the vertex's secondary label.

			if (oVertexTableColumnIndexes.SecondaryLabel != NoSuchColumn)
			{
				CheckForNonEmptyCell(oVertexRangeArea, aoVertexValues,
					iRowOneBased, oVertexTableColumnIndexes.SecondaryLabel,
					oVertex, ReservedMetadataKeys.PerVertexSecondaryLabel);
			}

			// If there is a tooltip column and the tooltip for this row isn't
			// empty, set the vertex's tooltip.

			if (oVertexTableColumnIndexes.ToolTip != NoSuchColumn)
			{
				if ( CheckForNonEmptyCell(oVertexRangeArea, aoVertexValues,
					iRowOneBased, oVertexTableColumnIndexes.ToolTip, oVertex,
					ReservedMetadataKeys.ToolTip) )
				{
					oReadWorkbookContext.ToolTipsUsed = true;
				}
			}

			// If there is a primary label column and the primary label for
			// this row isn't empty, set the vertex's primary label.

			if (oVertexTableColumnIndexes.PrimaryLabel != NoSuchColumn)
			{
				CheckForNonEmptyCell(oVertexRangeArea, aoVertexValues,
					iRowOneBased, oVertexTableColumnIndexes.PrimaryLabel,
					oVertex, ReservedMetadataKeys.PerVertexPrimaryLabel);
			}

			// If there is an image key column and the image key for this row
			// isn't empty, set the vertex's image.

			if (oVertexTableColumnIndexes.ImageKey != NoSuchColumn)
			{
				CheckForImageKey(oVertexRangeArea, aoVertexValues,
					iRowOneBased, oVertexTableColumnIndexes.ImageKey,
					oImageIDDictionary, oVertex);
			}

			// If there is a color column and the color for this row isn't
			// empty, set the vertex's color.

			if (oVertexTableColumnIndexes.Color != NoSuchColumn)
			{
				CheckForColor(oVertexRangeArea, aoVertexValues, iRowOneBased,
					oVertexTableColumnIndexes.Color, oVertex,
					oReadWorkbookContext.ColorConverter2);
			}

			// If there is a shape column and the shape for this row isn't
			// empty, set the vertex's shape.

			if (oVertexTableColumnIndexes.Shape != NoSuchColumn)
			{
				CheckForShape(oVertexRangeArea, aoVertexValues,
					iRowOneBased, oVertexTableColumnIndexes.Shape, oVertex);
			}

			// If there is a radius column and the radius for this row isn't
			// empty, set the vertex's radius.

			if (oVertexTableColumnIndexes.Radius != NoSuchColumn)
			{
				CheckForRadius(oVertexRangeArea, aoVertexValues, iRowOneBased,
					oVertexTableColumnIndexes.Radius,
					oReadWorkbookContext.VertexRadiusConverter, oVertex);
			}

			// If there is a vertex drawer precedence column and the value for
			// this row isn't empty, set the vertex's drawer precedence.

			if (oVertexTableColumnIndexes.VertexDrawerPrecedence !=
				NoSuchColumn)
			{
				CheckForVertexDrawerPrecedence(oVertexRangeArea,
					aoVertexValues, iRowOneBased,
					oVertexTableColumnIndexes.VertexDrawerPrecedence, oVertex);
			}

		}
    }

    //*************************************************************************
    //  Method: CheckForShape()
    //
    /// <summary>
	/// If a shape has been specified for a vertex, sets the vertex's shape.
    /// </summary>
    ///
    /// <param name="oVertexRange">
	/// Range containing the vertex data.
    /// </param>
    ///
    /// <param name="aoValues">
	/// Values from <paramref name="oVertexRange" />.
    /// </param>
    ///
    /// <param name="iRowOneBased">
	/// One-based row index to check.
    /// </param>
	///
    /// <param name="iColumnOneBased">
	/// One-based column index to check.
    /// </param>
    ///
    /// <param name="oVertex">
	/// Vertex to set the shape on.
    /// </param>
    //*************************************************************************

	protected void
	CheckForShape
	(
		Range oVertexRange,
		Object [,] aoValues,
		Int32 iRowOneBased,
		Int32 iColumnOneBased,
		IVertex oVertex
	)
	{
		Debug.Assert(oVertexRange != null);
		Debug.Assert(aoValues != null);
		Debug.Assert(iRowOneBased >= 1);
		Debug.Assert(iColumnOneBased >= 1);
		Debug.Assert(oVertex != null);
        AssertValid();

		String sShape;

		if ( !ExcelUtil.TryGetNonEmptyStringFromCell(aoValues,
			iRowOneBased, iColumnOneBased, out sShape) )
		{
			return;
		}

		VertexShapeConverter oVertexShapeConverter =
			new VertexShapeConverter();

		VertexDrawer.VertexShape eShape;

		if ( !oVertexShapeConverter.TryWorkbookToGraph(sShape, out eShape) )
		{
			Range oInvalidCell =
				(Range)oVertexRange.Cells[iRowOneBased, iColumnOneBased];

			OnWorkbookFormatError( String.Format(

				"The cell {0} contains an invalid shape.  Try selecting from"
				+ " the cell's drop-down list instead."
				,
				ExcelUtil.GetRangeAddress(oInvalidCell)
				),

				oInvalidCell
			);
		}

		oVertex.SetValue(ReservedMetadataKeys.PerVertexShape, eShape);
	}

    //*************************************************************************
    //  Method: CheckForRadius()
    //
    /// <summary>
	/// If a radius has been specified for a vertex, sets the vertex's radius.
    /// </summary>
    ///
    /// <param name="oVertexRange">
	/// Range containing the vertex data.
    /// </param>
    ///
    /// <param name="aoVertexValues">
	/// Values from <paramref name="oVertexRange" />.
    /// </param>
    ///
    /// <param name="iRowOneBased">
	/// One-based row index to check.
    /// </param>
	///
    /// <param name="iColumnOneBased">
	/// One-based column index to check.
    /// </param>
	///
    /// <param name="oVertexRadiusConverter">
	/// Object that converts a vertex radius between values used in the Excel
	/// workbook and values used in the NetMap graph.
    /// </param>
    ///
    /// <param name="oVertex">
	/// Vertex to set the radius on.
    /// </param>
    //*************************************************************************

	protected void
	CheckForRadius
	(
		Range oVertexRange,
		Object [,] aoVertexValues,
		Int32 iRowOneBased,
		Int32 iColumnOneBased,
		VertexRadiusConverter oVertexRadiusConverter,
		IVertex oVertex
	)
	{
		Debug.Assert(oVertexRange != null);
		Debug.Assert(aoVertexValues != null);
		Debug.Assert(iRowOneBased >= 1);
		Debug.Assert(iColumnOneBased >= 1);
		Debug.Assert(oVertex != null);
		Debug.Assert(oVertexRadiusConverter != null);
        AssertValid();

		String sRadius;

		if ( !ExcelUtil.TryGetNonEmptyStringFromCell(aoVertexValues,
			iRowOneBased, iColumnOneBased, out sRadius) )
		{
			return;
		}

		Single fRadius;

		if ( !Single.TryParse(sRadius, out fRadius) )
		{
			Range oInvalidCell =
				(Range)oVertexRange.Cells[iRowOneBased, iColumnOneBased];

			OnWorkbookFormatError( String.Format(

				"The cell {0} contains an invalid radius.  The vertex radius,"
				+ " which is optional, must be a number.  Any number is"
				+ " acceptable, although {1} is used for any number less than"
				+ " {1} and {2} is used for any number greater than {2}."
				,
				ExcelUtil.GetRangeAddress(oInvalidCell),
				VertexRadiusConverter.MinimumRadiusWorkbook,
                VertexRadiusConverter.MaximumRadiusWorkbook
				),

				oInvalidCell
			);
		}

		oVertex.SetValue( ReservedMetadataKeys.PerVertexRadius,
			oVertexRadiusConverter.WorkbookToGraph(fRadius) );
	}

    //*************************************************************************
    //  Method: CheckForImageKey()
    //
    /// <summary>
	/// If an image key has been specified for a vertex, sets the vertex's
	/// image key.
    /// </summary>
    ///
    /// <param name="oVertexRange">
	/// Range containing the vertex data.
    /// </param>
    ///
    /// <param name="aoVertexValues">
	/// Values from <paramref name="oVertexRange" />.
    /// </param>
    ///
    /// <param name="iRowOneBased">
	/// One-based row index to check.
    /// </param>
	///
    /// <param name="iColumnOneBased">
	/// One-based column index to check.
    /// </param>
    ///
    /// <param name="oImageIDDictionary">
	/// Keeps track of vertex images.  The key is a unique image identifier
	/// specified in the image worksheet and the value is the corresponding
	/// System.Drawing.Image.
    /// </param>
	///
    /// <param name="oVertex">
	/// Vertex to set the image key on.
    /// </param>
	///
	/// <returns>
	/// true if an image key was specified.
	/// </returns>
    //*************************************************************************

	protected Boolean
	CheckForImageKey
	(
		Range oVertexRange,
		Object [,] aoVertexValues,
		Int32 iRowOneBased,
		Int32 iColumnOneBased,
		Dictionary<String, Image> oImageIDDictionary,
		IVertex oVertex
	)
	{
		Debug.Assert(oVertexRange != null);
		Debug.Assert(aoVertexValues != null);
		Debug.Assert(iRowOneBased >= 1);
		Debug.Assert(iColumnOneBased >= 1);
		Debug.Assert(oImageIDDictionary != null);
		Debug.Assert(oVertex != null);
        AssertValid();

		String sImageKey;

		if ( !ExcelUtil.TryGetNonEmptyStringFromCell(aoVertexValues,
			iRowOneBased, iColumnOneBased, out sImageKey) )
		{
			return (false);
		}

		sImageKey = sImageKey.ToLower();

		// Retieve the Image corresponding to the image key from the image
		// dictionary.

		Image oVertexImage;

		if ( !oImageIDDictionary.TryGetValue(sImageKey, out oVertexImage) )
		{
			return (false);
		}

		oVertex.SetValue(ReservedMetadataKeys.PerVertexImage, oVertexImage);

		return (true);
	}

    //*************************************************************************
    //  Method: CheckForVertexDrawerPrecedence()
    //
    /// <summary>
	/// If a vertex drawer precedence value has been specified for a vertex,
	/// sets the vertex's drawer precedence.
    /// </summary>
    ///
    /// <param name="oVertexRange">
	/// Range containing the vertex data.
    /// </param>
    ///
    /// <param name="aoValues">
	/// Values from <paramref name="oVertexRange" />.
    /// </param>
    ///
    /// <param name="iRowOneBased">
	/// One-based row index to check.
    /// </param>
	///
    /// <param name="iColumnOneBased">
	/// One-based column index to check.
    /// </param>
    ///
    /// <param name="oVertex">
	/// Vertex to set the vertex drawer precedence on.
    /// </param>
    //*************************************************************************

	protected void
	CheckForVertexDrawerPrecedence
	(
		Range oVertexRange,
		Object [,] aoValues,
		Int32 iRowOneBased,
		Int32 iColumnOneBased,
		IVertex oVertex
	)
	{
		Debug.Assert(oVertexRange != null);
		Debug.Assert(aoValues != null);
		Debug.Assert(iRowOneBased >= 1);
		Debug.Assert(iColumnOneBased >= 1);
		Debug.Assert(oVertex != null);
        AssertValid();

		String sVertexDrawerPrecedence;

		if ( !ExcelUtil.TryGetNonEmptyStringFromCell(aoValues,
			iRowOneBased, iColumnOneBased, out sVertexDrawerPrecedence) )
		{
			return;
		}

		VertexDrawerPrecedence eVertexDrawerPrecedence;

		if ( !( new VertexDrawerPrecedenceConverter() ).TryWorkbookToGraph(
			sVertexDrawerPrecedence, out eVertexDrawerPrecedence) )
		{
			Range oInvalidCell =
				(Range)oVertexRange.Cells[iRowOneBased, iColumnOneBased];

			OnWorkbookFormatError( String.Format(

				"The cell {0} contains an invalid value.  Try selecting from"
				+ " the cell's drop-down list instead."
				,
				ExcelUtil.GetRangeAddress(oInvalidCell)
				),

				oInvalidCell
			);
		}

		oVertex.SetValue(ReservedMetadataKeys.PerVertexDrawerPrecedence,
			eVertexDrawerPrecedence);
	}

    //*************************************************************************
    //  Method: CheckForNonEmptyCell()
    //
    /// <summary>
	/// If a cell is not empty, sets a metadata value on the vertex to the
	/// cell contents.
    /// </summary>
    ///
    /// <param name="oVertexRange">
	/// Range containing the vertex data.
    /// </param>
    ///
    /// <param name="aoVertexValues">
	/// Values from <paramref name="oVertexRange" />.
    /// </param>
    ///
    /// <param name="iRowOneBased">
	/// One-based row index to check.
    /// </param>
	///
    /// <param name="iColumnOneBased">
	/// One-based column index to check.
    /// </param>
    ///
    /// <param name="oVertex">
	/// Vertex to set the metadata value on.
    /// </param>
	///
    /// <param name="sKeyName">
	/// Name of the metadata key to set.
    /// </param>
	///
	/// <returns>
	/// true if the metadata value was set on the vertex.
	/// </returns>
    //*************************************************************************

	protected Boolean
	CheckForNonEmptyCell
	(
		Range oVertexRange,
		Object [,] aoVertexValues,
		Int32 iRowOneBased,
		Int32 iColumnOneBased,
		IVertex oVertex,
		String sKeyName
	)
	{
		Debug.Assert(oVertexRange != null);
		Debug.Assert(aoVertexValues != null);
		Debug.Assert(iRowOneBased >= 1);
		Debug.Assert(iColumnOneBased >= 1);
		Debug.Assert(oVertex != null);
		Debug.Assert( !String.IsNullOrEmpty(sKeyName) );
        AssertValid();

		String sNonEmptyString;

		if ( !ExcelUtil.TryGetNonEmptyStringFromCell(aoVertexValues,
			iRowOneBased, iColumnOneBased, out sNonEmptyString) )
		{
			return (false);
		}

		oVertex.SetValue(sKeyName, sNonEmptyString);

		return (true);
	}

    //*************************************************************************
    //  Method: CheckForLocation()
    //
    /// <summary>
	/// If a location has been specified for a vertex, sets the vertex's
	/// location.
    /// </summary>
    ///
    /// <param name="oVertexRange">
	/// Range containing the vertex data.
    /// </param>
    ///
    /// <param name="aoValues">
	/// Values from <paramref name="oVertexRange" />.
    /// </param>
    ///
    /// <param name="iRowOneBased">
	/// One-based row index to check.
    /// </param>
	///
    /// <param name="iXColumnOneBased">
	/// One-based X column index to check.
    /// </param>
    ///
    /// <param name="iYColumnOneBased">
	/// One-based Y column index to check.
    /// </param>
	///
    /// <param name="oVertexLocationConverter">
	/// Object that converts a vertex location between coordinates used in the
	/// Excel workbook and coordinates used in the NetMap graph.
    /// </param>
    ///
    /// <param name="oVertex">
	/// Vertex to set the location on.
    /// </param>
	///
	/// <returns>
	/// true if a location was specified.
	/// </returns>
    //*************************************************************************

	protected Boolean
	CheckForLocation
	(
		Range oVertexRange,
		Object [,] aoValues,
		Int32 iRowOneBased,
		Int32 iXColumnOneBased,
		Int32 iYColumnOneBased,
		VertexLocationConverter oVertexLocationConverter,
		IVertex oVertex
	)
	{
		Debug.Assert(oVertexRange != null);
		Debug.Assert(aoValues != null);
		Debug.Assert(iRowOneBased >= 1);
		Debug.Assert(iXColumnOneBased >= 1);
		Debug.Assert(iYColumnOneBased >= 1);
		Debug.Assert(oVertexLocationConverter != null);
		Debug.Assert(oVertex != null);
        AssertValid();

		String sX;

		Boolean bHasX = ExcelUtil.TryGetNonEmptyStringFromCell(aoValues,
			iRowOneBased, iXColumnOneBased, out sX);

		String sY;

		Boolean bHasY = ExcelUtil.TryGetNonEmptyStringFromCell(aoValues,
			iRowOneBased, iYColumnOneBased, out sY);

		if (bHasX != bHasY)
		{
			// X or Y alone won't do.

			goto Error;
		}

		if (!bHasX && !bHasY)
		{
			return (false);
		}

		Single fX, fY;

		if ( !Single.TryParse(sX, out fX) || !Single.TryParse(sY, out fY) )
		{
			goto Error;
		}

		// Transform the location from workbook coordinates to graph
		// coordinates.

		oVertex.Location = oVertexLocationConverter.WorkbookToGraph(fX, fY);

		return (true);

		Error:

			Range oInvalidCell = (Range)oVertexRange.Cells[
				iRowOneBased, iXColumnOneBased];

			OnWorkbookFormatError( String.Format(

				"There is a problem with the vertex location at {0}.  If you"
				+ " enter a vertex location, it must include both X and Y"
				+ " numbers.  Any numbers are acceptable, although {1} is used"
				+ " for any number less than {1} and and {2} is used for any"
				+ " number greater than {2}."
				,
				ExcelUtil.GetRangeAddress(oInvalidCell),

                VertexLocationConverter.MinimumXYWorkbook.ToString(
					ExcelTemplateForm.Int32Format),

                VertexLocationConverter.MaximumXYWorkbook.ToString(
					ExcelTemplateForm.Int32Format)
				),

				oInvalidCell
				);

			// Make the compiler happy.

			return (false);
	}

    //*************************************************************************
    //  Method: CheckForLocked()
    //
    /// <summary>
	/// If a locked flag has been specified for a vertex, sets the vertex's
	/// locked flag.
    /// </summary>
    ///
    /// <param name="oVertexRange">
	/// Range containing the vertex data.
    /// </param>
    ///
    /// <param name="aoValues">
	/// Values from <paramref name="oVertexRange" />.
    /// </param>
    ///
    /// <param name="iRowOneBased">
	/// One-based row index to check.
    /// </param>
	///
    /// <param name="iColumnOneBased">
	/// One-based column index to check.
    /// </param>
    ///
    /// <param name="bLocationSpecified">
	/// true if a location was specified for the vertex.
    /// </param>
    ///
    /// <param name="oVertex">
	/// Vertex to set the lock flag on.
    /// </param>
    //*************************************************************************

	protected void
	CheckForLocked
	(
		Range oVertexRange,
		Object [,] aoValues,
		Int32 iRowOneBased,
		Int32 iColumnOneBased,
		Boolean bLocationSpecified,
		IVertex oVertex
	)
	{
		Debug.Assert(oVertexRange != null);
		Debug.Assert(aoValues != null);
		Debug.Assert(iRowOneBased >= 1);
		Debug.Assert(iColumnOneBased >= 1);
		Debug.Assert(oVertex != null);
        AssertValid();

		String sLocked;

		if ( !ExcelUtil.TryGetNonEmptyStringFromCell(aoValues,
			iRowOneBased, iColumnOneBased, out sLocked) )
		{
			return;
		}

		Boolean bLocked;
		String sErrorMessage;

		if ( !( new BooleanConverter() ).TryWorkbookToGraph(
			sLocked, out bLocked) )
		{
			sErrorMessage = 
				"The cell {0} contains an invalid lock value.  Try selecting"
				+ " from the cell's drop-down list instead."
				;

			goto Error;
		}

		if (bLocked && !bLocationSpecified)
		{
			sErrorMessage = 
				"The cell {0} indicates that the vertex should be locked,"
				+ " but the vertex has no X and Y location values.  Either"
				+ " clear the lock or specify a vertex location."
				;

			goto Error;
		}

		// Set the "lock vertex" key.

		oVertex.SetValue(ReservedMetadataKeys.LockVertexLocation, bLocked);

		return;

		Error:

			Debug.Assert(sErrorMessage != null);
			Debug.Assert(sErrorMessage.IndexOf("{0}") >= 0);

			Range oInvalidCell =
				(Range)oVertexRange.Cells[iRowOneBased, iColumnOneBased];

			OnWorkbookFormatError( String.Format(

				sErrorMessage
				,
				ExcelUtil.GetRangeAddress(oInvalidCell)
				),

				oInvalidCell
			);
	}

    //*************************************************************************
    //  Method: CheckForMarked()
    //
    /// <summary>
	/// If a marked flag has been specified for a vertex, sets the vertex's
	/// marked flag.
    /// </summary>
    ///
    /// <param name="oVertexRange">
	/// Range containing the vertex data.
    /// </param>
    ///
    /// <param name="aoValues">
	/// Values from <paramref name="oVertexRange" />.
    /// </param>
    ///
    /// <param name="iRowOneBased">
	/// One-based row index to check.
    /// </param>
	///
    /// <param name="iColumnOneBased">
	/// One-based column index to check.
    /// </param>
    ///
    /// <param name="oVertex">
	/// Vertex to set the marked flag on.
    /// </param>
    //*************************************************************************

	protected void
	CheckForMarked
	(
		Range oVertexRange,
		Object [,] aoValues,
		Int32 iRowOneBased,
		Int32 iColumnOneBased,
		IVertex oVertex
	)
	{
		Debug.Assert(oVertexRange != null);
		Debug.Assert(aoValues != null);
		Debug.Assert(iRowOneBased >= 1);
		Debug.Assert(iColumnOneBased >= 1);
		Debug.Assert(oVertex != null);
        AssertValid();

		String sMarked;

		if ( !ExcelUtil.TryGetNonEmptyStringFromCell(aoValues,
			iRowOneBased, iColumnOneBased, out sMarked) )
		{
			return;
		}

		Boolean bMarked;

		if ( !( new BooleanConverter() ).TryWorkbookToGraph(
			sMarked, out bMarked) )
		{
			Range oInvalidCell = (Range)oVertexRange.Cells[
				iRowOneBased, iColumnOneBased];

			OnWorkbookFormatError( String.Format(

				"The cell {0} contains an invalid marked value.  Try selecting"
				+ " from the cell's drop-down list instead."
				,
				ExcelUtil.GetRangeAddress(oInvalidCell)
				),

				oInvalidCell
				);
		}

		// Set the "mark vertex" key.

		oVertex.SetValue(ReservedMetadataKeys.Marked, bMarked);

		return;
	}

    //*************************************************************************
    //  Method: CheckForCustomMenuItems()
    //
    /// <summary>
	/// If custom menu items have been specified for a vertex, stores the
	/// custom menu item information in the vertex.
    /// </summary>
    ///
    /// <param name="oVertexRange">
	/// Range containing the vertex data.
    /// </param>
    ///
    /// <param name="aoValues">
	/// Values from <paramref name="oVertexRange" />.
    /// </param>
    ///
    /// <param name="iRowOneBased">
	/// One-based row index to check.
    /// </param>
	///
    /// <param name="aoCustomMenuItemPairIndexes">
	/// Array of pairs of one-based column indexes, one array element for each
	/// pair of columns that are used to add custom menu items to the vertex
	/// context menu in the graph.  They key is the index of the custom menu
	/// item text and the value is the index of the custom menu item action.
    /// </param>
    ///
    /// <param name="oVertex">
	/// Vertex to add custom menu item information to.
    /// </param>
    //*************************************************************************

	protected void
	CheckForCustomMenuItems
	(
		Range oVertexRange,
		Object [,] aoValues,
		Int32 iRowOneBased,
		KeyValuePair<Int32, Int32> [] aoCustomMenuItemPairIndexes,
		IVertex oVertex
	)
	{
		Debug.Assert(oVertexRange != null);
		Debug.Assert(aoValues != null);
		Debug.Assert(iRowOneBased >= 1);
		Debug.Assert(aoCustomMenuItemPairIndexes != null);
		Debug.Assert(oVertex != null);
		AssertValid();

		// List of string pairs, one pair for each custom menu item to add to
		// the vertex's context menu in the graph.  The key is the custom menu
		// item text and the value is the custom menu item action.

		List<KeyValuePair<String, String>> oCustomMenuItemInformation =
			new List<KeyValuePair<String, String>>();

		// Loop through the column index pairs.

		foreach (KeyValuePair<Int32, Int32> oCustomMenuItemPairIndex in
			aoCustomMenuItemPairIndexes)
		{
			String sCustomMenuItemText, sCustomMenuItemAction;

			// Both the menu item text and menu item action must be specified. 
			// Skip the pair if either is missing.

			if (
				!ExcelUtil.TryGetNonEmptyStringFromCell(aoValues,
					iRowOneBased, oCustomMenuItemPairIndex.Key,
					out sCustomMenuItemText)
				||
				!ExcelUtil.TryGetNonEmptyStringFromCell(aoValues,
					iRowOneBased, oCustomMenuItemPairIndex.Value,
					out sCustomMenuItemAction)
				)
			{
				continue;
			}

			Int32 iCustomMenuItemTextLength = sCustomMenuItemText.Length;

			if (iCustomMenuItemTextLength > MaximumCustomMenuItemTextLength)
			{
				Range oInvalidCell = (Range)oVertexRange.Cells[
					iRowOneBased, oCustomMenuItemPairIndex.Key];

				OnWorkbookFormatError( String.Format(

					"The cell {0} contains custom menu item text that is {1}"
					+ " characters long.  Custom menu item text can't be"
					+ " longer than {2} characters."
					,
					ExcelUtil.GetRangeAddress(oInvalidCell),
					iCustomMenuItemTextLength,
					MaximumCustomMenuItemTextLength
					),

					oInvalidCell
					);
			}

			oCustomMenuItemInformation.Add( new KeyValuePair<String, String>(
				sCustomMenuItemText, sCustomMenuItemAction) );
		}

		if (oCustomMenuItemInformation.Count > 0)
		{
			oVertex.SetValue( ReservedMetadataKeys.CustomContextMenuItems,
				oCustomMenuItemInformation.ToArray() );
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
    //  Protected constants
    //*************************************************************************

	/// Maximum length of custom menu item text.

	protected const Int32 MaximumCustomMenuItemTextLength = 50;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)


	//*************************************************************************
	//  Embedded class: VertexTableColumnIndexes
	//
	/// <summary>
	/// Contains the one-based indexes of the columns in the optional vertex
	/// table.
	/// </summary>
	//*************************************************************************

	public class VertexTableColumnIndexes
	{
		/// Name of the vertex, required if any of the other columns are to be
		/// used.

		public Int32 VertexName;

		/// The edge's optional color.

		public Int32 Color;

		/// The edge's optional shape.

		public Int32 Shape;

		/// The edge's optional radius.

		public Int32 Radius;

		/// The edge's optional image key.  If specified, the image is drawn
		/// instead of a shape.

		public Int32 ImageKey;

		/// The edge's optional primary label.  If specified, the primary label
		/// is drawn instead of a shape or image.

		public Int32 PrimaryLabel;

		/// The edge's optional secondary label.  If specified, the secondary
		/// label is drawn in addition to a shape, image, or primary label.

		public Int32 SecondaryLabel;

		/// The vertex's optional alpha, from 0 (transparent) to 10 (opaque).

		public Int32 Alpha;

		/// The vertex's optional tooltip.

		public Int32 ToolTip;

		/// The optional value specifying which vertex drawer should be used
		/// for the vertex.

		public Int32 VertexDrawerPrecedence;

		/// The vertex's optional visibility.

		public Int32 Visibility;

		/// The vertex's optional x-coordinate.

		public Int32 X;

		/// The vertex's optional y-coordinate.

		public Int32 Y;

		/// The vertex's optional "lock vertex location" boolean flag.

		public Int32 Locked;

		/// The vertex's ID, which is filled in by ReadWorksheet().

		public Int32 ID;

		/// The vertex's optional "mark vertex" boolean flag.

		public Int32 Marked;
	}
}

}
