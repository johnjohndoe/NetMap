
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.NodeXL.Core;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphImporter
//
/// <summary>
/// Imports edges and vertices from a graph to the edge and vertex worksheets.
/// </summary>
///
/// <remarks>
/// This class is typically used when an external file is imported into a
/// NodeXL workbook.  The caller should first read the file into a NodeXL graph
/// using one of the graph adapter classes, then call the <see
/// cref="ImportGraph" /> method in this class to import the edges and vertices
/// from the graph to a NodeXL workbook.
/// </remarks>
//*****************************************************************************

public class GraphImporter : Object
{
    //*************************************************************************
    //  Constructor: GraphImporter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphImporter" /> class.
    /// </summary>
    //*************************************************************************

    public GraphImporter()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: ImportGraph()
    //
    /// <summary>
    /// Imports edges and vertices from a graph to the edge and vertex
    /// worksheets.
    /// </summary>
    ///
    /// <param name="sourceGraph">
    /// Graph to import the edges and vertices from.
    /// </param>
    ///
    /// <param name="edgeAttributes">
    /// Array of edge attribute names that have been added to the metadata of
    /// the graph's vertices.  Can be null.
    /// </param>
    ///
    /// <param name="vertexAttributes">
    /// Array of vertex attribute names that have been added to the metadata of
    /// the graph's vertices.  Can be null.
    /// </param>
    ///
    /// <param name="clearTablesFirst">
    /// true if the NodeXL tables in <paramref
    /// name="destinationNodeXLWorkbook" /> should be cleared first.
    /// </param>
    ///
    /// <param name="destinationNodeXLWorkbook">
    /// NodeXL workbook the edges and vertices will be imported to.
    /// </param>
    ///
    /// <remarks>
    /// This method creates a row in the edge worksheet for each edge in
    /// <paramref name="sourceGraph" />, and a row in the vertex worksheet for
    /// each edge.
    ///
    /// <para>
    /// For each attribute name in <paramref name="edgeAttributes" /> (if
    /// <paramref name="edgeAttributes" /> is not null), a column is added to
    /// the edge worksheet if the column doesn't already exist, and the
    /// corresponding attribute values stored on the edges are use to fill the
    /// column.  The same is done for <paramref name="vertexAttributes" /> and
    /// the vertex worksheet.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    ImportGraph
    (
        IGraph sourceGraph,
        String [] edgeAttributes,
        String [] vertexAttributes,
        Boolean clearTablesFirst,
        Microsoft.Office.Interop.Excel.Workbook destinationNodeXLWorkbook
    )
    {
        Debug.Assert(sourceGraph != null);
        Debug.Assert(destinationNodeXLWorkbook != null);
        AssertValid();

        if (clearTablesFirst)
        {
            NodeXLWorkbookUtil.ClearTables(destinationNodeXLWorkbook);
        }

        // Get the required table that contains edge data.  GetEdgeTable()
        // throws an exception if the table is missing.

        EdgeWorksheetReader oEdgeWorksheetReader = new EdgeWorksheetReader();

        ListObject oEdgeTable =
            oEdgeWorksheetReader.GetEdgeTable(destinationNodeXLWorkbook);

        // Get the required columns.

        Range oVertex1NameColumnData = null;
        Range oVertex2NameColumnData = null;

        if (
            !ExcelUtil.TryGetTableColumnData(oEdgeTable,
                EdgeTableColumnNames.Vertex1Name, out oVertex1NameColumnData)
            ||
            !ExcelUtil.TryGetTableColumnData(oEdgeTable,
                EdgeTableColumnNames.Vertex2Name, out oVertex2NameColumnData)
            )
        {
            ErrorUtil.OnMissingColumn();
        }

        // Import the edges and their attributes into the workbook.

        ImportEdges(sourceGraph, edgeAttributes, oEdgeTable,
            oVertex1NameColumnData, oVertex2NameColumnData, !clearTablesFirst);

        // Populate the vertex worksheet with the name of each unique vertex in
        // the edge worksheet.

        ( new VertexWorksheetPopulator() ).PopulateVertexWorksheet(
            destinationNodeXLWorkbook, false);

        // Get the table that contains vertex data.

        ListObject oVertexTable;
        Range oVertexNameColumnData = null;
        Range oVisibilityColumnData = null;

        if (
            !ExcelUtil.TryGetTable(destinationNodeXLWorkbook,
                WorksheetNames.Vertices, TableNames.Vertices, out oVertexTable)
            ||
            !ExcelUtil.TryGetTableColumnData(oVertexTable,
                VertexTableColumnNames.VertexName, out oVertexNameColumnData)
            ||
            !ExcelUtil.TryGetTableColumnData(oVertexTable,
                VertexTableColumnNames.Visibility, out oVisibilityColumnData)
            )
        {
            ErrorUtil.OnMissingColumn();
        }

        // Import isolated vertices and the attributes for all the graph's
        // vertices.

        ImportVertices(sourceGraph, vertexAttributes, oVertexTable,
            oVertexNameColumnData, oVisibilityColumnData);
    }

    //*************************************************************************
    //  Method: ImportEdges()
    //
    /// <summary>
    /// Imports edges and their attributes from a graph to the edge worksheet.
    /// </summary>
    ///
    /// <param name="oSourceGraph">
    /// Graph to import the edges from.
    /// </param>
    ///
    /// <param name="asEdgeAttributes">
    /// Array of edge attribute names that have been added to the metadata of
    /// the graph's edges.  Can be null.
    /// </param>
    ///
    /// <param name="oEdgeTable">
    /// Edge table the edges will be imported to.
    /// </param>
    ///
    /// <param name="oVertex1NameColumnData">
    /// Data body range of the vertex 1 name column.
    /// </param>
    ///
    /// <param name="oVertex2NameColumnData">
    /// Data body range of the vertex 2 name column.
    /// </param>
    ///
    /// <param name="bAppendToTable">
    /// true to append the edges to any edges already in the edge table, false
    /// to overwrite any edges.
    /// </param>
    //*************************************************************************

    protected void
    ImportEdges
    (
        IGraph oSourceGraph,
        String [] asEdgeAttributes,
        ListObject oEdgeTable,
        Range oVertex1NameColumnData,
        Range oVertex2NameColumnData,
        Boolean bAppendToTable
    )
    {
        Debug.Assert(oSourceGraph != null);
        Debug.Assert(oEdgeTable != null);
        Debug.Assert(oVertex1NameColumnData != null);
        Debug.Assert(oVertex2NameColumnData != null);
        AssertValid();

        Int32 iRowOffsetToWriteTo = 0;

        if (bAppendToTable)
        {
            iRowOffsetToWriteTo =
                ExcelUtil.GetOffsetOfFirstEmptyTableRow(oEdgeTable);

            ExcelUtil.OffsetRange(ref oVertex1NameColumnData,
                iRowOffsetToWriteTo, 0);

            ExcelUtil.OffsetRange(ref oVertex2NameColumnData,
                iRowOffsetToWriteTo, 0);
        }

        Range [] aoEdgeAttributeColumnData = null;
        Object [][,] aaoEdgeAttributeValues = null;
        Int32 iEdgeAttributes = 0;
        IEdgeCollection oEdges = oSourceGraph.Edges;
        Int32 iEdges = oEdges.Count;

        // Create vertex name and edge attribute arrays that will be written to
        // the edge table.

        Object [,] aoVertex1NameValues =
            ExcelUtil.GetSingleColumn2DArray(iEdges);

        Object [,] aoVertex2NameValues =
            ExcelUtil.GetSingleColumn2DArray(iEdges);

        if (asEdgeAttributes != null)
        {
            iEdgeAttributes = asEdgeAttributes.Length;
            aoEdgeAttributeColumnData = new Range[iEdgeAttributes];
            aaoEdgeAttributeValues = new Object[iEdgeAttributes][,];
            ListColumn oEdgeAttributeColumn;
            Range oEdgeAttributeColumnData;

            for (Int32 i = 0; i < iEdgeAttributes; i++)
            {
                String sEdgeAttribute = asEdgeAttributes[i];

                if (
                    !ExcelUtil.TryGetOrAddTableColumn(oEdgeTable,
                        sEdgeAttribute, ExcelUtil.AutoColumnWidth, null,
                        out oEdgeAttributeColumn)
                    ||
                    !ExcelUtil.TryGetTableColumnData(oEdgeAttributeColumn,
                        out oEdgeAttributeColumnData)
                    )
                {
                    throw new WorkbookFormatException(
                        "The " + sEdgeAttribute + " column couldn't be added."
                        );
                }

                if (bAppendToTable)
                {
                    ExcelUtil.OffsetRange(ref oEdgeAttributeColumnData,
                        iRowOffsetToWriteTo, 0);
                }

                aoEdgeAttributeColumnData[i] = oEdgeAttributeColumnData;

                aaoEdgeAttributeValues[i] =
                    ExcelUtil.GetSingleColumn2DArray(iEdges);
            }
        }

        // Fill in the vertex name and edge attribute arrays.

        Int32 iEdge = 1;

        foreach (IEdge oEdge in oEdges)
        {
            IVertex [] aoVertices = oEdge.Vertices;

            aoVertex1NameValues[iEdge, 1] = aoVertices[0].Name;
            aoVertex2NameValues[iEdge, 1] = aoVertices[1].Name;

            Object oEdgeAttribute;

            for (Int32 i = 0; i < iEdgeAttributes; i++)
            {
                if ( oEdge.TryGetValue(asEdgeAttributes[i],
                    out oEdgeAttribute) )
                {
                    aaoEdgeAttributeValues[i][iEdge, 1] = oEdgeAttribute;
                }
            }

            iEdge++;
        }

        // Write the vertex name and edge attribute arrays to the table.

        ExcelUtil.SetRangeValues( (Range)oVertex1NameColumnData.Cells[1, 1],
            aoVertex1NameValues );

        ExcelUtil.SetRangeValues( (Range)oVertex2NameColumnData.Cells[1, 1],
            aoVertex2NameValues );

        for (Int32 i = 0; i < iEdgeAttributes; i++)
        {
            ExcelUtil.SetRangeValues(
                (Range)aoEdgeAttributeColumnData[i].Cells[1, 1],
                aaoEdgeAttributeValues[i] );
        }
    }

    //*************************************************************************
    //  Method: ImportVertices()
    //
    /// <summary>
    /// Imports vertices and their attributes from a graph to the vertex
    /// worksheet.
    /// </summary>
    ///
    /// <param name="oSourceGraph">
    /// Graph to import the edges from.
    /// </param>
    ///
    /// <param name="asVertexAttributes">
    /// Array of vertex attribute names that have been added to the metadata of
    /// the graph's vertices.  Can be null.
    /// </param>
    ///
    /// <param name="oVertexTable">
    /// Vertex table the vertices will be imported to.
    /// </param>
    ///
    /// <param name="oVertexNameColumnData">
    /// Data body range of the vertex name column.
    /// </param>
    ///
    /// <param name="oVisibilityColumnData">
    /// Data body range of the vertex visibility column.
    /// </param>
    //*************************************************************************

    protected void
    ImportVertices
    (
        IGraph oSourceGraph,
        String [] asVertexAttributes,
        ListObject oVertexTable,
        Range oVertexNameColumnData,
        Range oVisibilityColumnData
    )
    {
        Debug.Assert(oSourceGraph != null);
        Debug.Assert(oVertexTable != null);
        Debug.Assert(oVertexNameColumnData != null);
        Debug.Assert(oVisibilityColumnData != null);
        AssertValid();

        // Create a dictionary that maps vertex names to row numbers in the
        // vertex worksheet.

        Dictionary<String, Int32> oVertexDictionary =
            new Dictionary<String, Int32>();

        Object [,] aoVertexNameValues =
            ExcelUtil.GetRangeValues(oVertexNameColumnData);

        Int32 iRows = oVertexNameColumnData.Rows.Count;

        for (Int32 iRowOneBased = 1; iRowOneBased <= iRows; iRowOneBased++)
        {
            String sVertexName;

            if ( ExcelUtil.TryGetNonEmptyStringFromCell(aoVertexNameValues,
                iRowOneBased, 1, out sVertexName) )
            {
                oVertexDictionary[sVertexName] = iRowOneBased;
            }
        }

        // Create a list of vertices not already included in the vertex table. 
        // This can occur when the graph has isolated vertices.

        List<String> oIsolatedVertexNames = new List<String>();

        foreach (IVertex oVertex in oSourceGraph.Vertices)
        {
            String sVertexName = oVertex.Name;

            if ( !oVertexDictionary.ContainsKey(sVertexName) )
            {
                oIsolatedVertexNames.Add(sVertexName);
            }
        }

        Int32 iIsolatedVertices = oIsolatedVertexNames.Count;

        if (iIsolatedVertices > 0)
        {
            // Append the isolated vertices to the table.  The vertex
            // visibilities should be set to Show to force them to be shown
            // even though they are not included in edges.

            String [,] asAddedVertexNameValues =
                new String [iIsolatedVertices, 1];

            String [,] asAddedVisibilityValues =
                new String [iIsolatedVertices, 1];

            String sShow = ( new VertexVisibilityConverter() ).GraphToWorkbook(
                VertexWorksheetReader.Visibility.Show);

            for (Int32 i = 0; i < iIsolatedVertices; i++)
            {
                String sIsolatedVertexName = oIsolatedVertexNames[i];
                asAddedVertexNameValues[i, 0] = sIsolatedVertexName;
                asAddedVisibilityValues[i, 0] = sShow;
                oVertexDictionary[sIsolatedVertexName] = iRows + i + 1;
            }

            ExcelUtil.SetRangeValues(
                oVertexNameColumnData.get_Offset(iRows, 0),
                asAddedVertexNameValues);

            ExcelUtil.SetRangeValues(
                oVisibilityColumnData.get_Offset(iRows, 0),
                asAddedVisibilityValues);
        }

        if (asVertexAttributes != null)
        {
            ImportVertexAttributes(oSourceGraph, asVertexAttributes,
                oVertexDictionary, oVertexTable);
        }
    }

    //*************************************************************************
    //  Method: ImportVertexAttributes()
    //
    /// <summary>
    /// Imports attributes from the graph's vertices to the vertex worksheet.
    /// </summary>
    ///
    /// <param name="oSourceGraph">
    /// Graph to import the edges from.
    /// </param>
    ///
    /// <param name="asVertexAttributes">
    /// Array of vertex attribute names that have been added to the metadata of
    /// the graph's vertices.  Can't be null.
    /// </param>
    ///
    /// <param name="oVertexDictionary">
    /// The key is a vertex name and the value is the one-based row offset.
    /// </param>
    ///
    /// <param name="oVertexTable">
    /// Vertex table the vertices will be imported to.
    /// </param>
    //*************************************************************************

    protected void
    ImportVertexAttributes
    (
        IGraph oSourceGraph,
        String [] asVertexAttributes,
        Dictionary<String, Int32> oVertexDictionary,
        ListObject oVertexTable
    )
    {
        Debug.Assert(oSourceGraph != null);
        Debug.Assert(asVertexAttributes != null);
        Debug.Assert(oVertexDictionary != null);
        Debug.Assert(oVertexTable != null);
        AssertValid();

        // Create vertex attribute arrays that will be written to the vertex
        // table.

        Int32 iVertexAttributes = asVertexAttributes.Length;

        Range [] aoVertexAttributeColumnData = new Range[iVertexAttributes];

        Object [][,] aaoVertexAttributeValues =
            new Object[iVertexAttributes][,];

        ListColumn oVertexAttributeColumn;
        Range oVertexAttributeColumnData;

        for (Int32 i = 0; i < iVertexAttributes; i++)
        {
            String sVertexAttribute = asVertexAttributes[i];

            if (
                !ExcelUtil.TryGetOrAddTableColumn(oVertexTable,
                    sVertexAttribute, ExcelUtil.AutoColumnWidth, null,
                    out oVertexAttributeColumn)
                ||
                !ExcelUtil.TryGetTableColumnData(oVertexAttributeColumn,
                    out oVertexAttributeColumnData)
                )
            {
                throw new WorkbookFormatException( String.Format(

                    "The {0} column couldn't be added."
                    ,
                    sVertexAttribute
                    ) );
            }

            aoVertexAttributeColumnData[i] = oVertexAttributeColumnData;

            aaoVertexAttributeValues[i] =
                ExcelUtil.GetRangeValues(oVertexAttributeColumnData);
        }

        foreach (IVertex oVertex in oSourceGraph.Vertices)
        {
            String sVertexName = oVertex.Name;
            Object oVertexAttribute;

            Int32 iOneBasedRowOffset;

            if ( !oVertexDictionary.TryGetValue(sVertexName,
                out iOneBasedRowOffset) )
            {
                Debug.Assert(false);
            }

            for (Int32 i = 0; i < iVertexAttributes; i++)
            {
                if ( oVertex.TryGetValue(asVertexAttributes[i],
                    out oVertexAttribute) )
                {
                    Debug.Assert(iOneBasedRowOffset >= 1);

                    Debug.Assert( iOneBasedRowOffset <=
                        aaoVertexAttributeValues[i].GetUpperBound(0) );

                    aaoVertexAttributeValues[i][iOneBasedRowOffset, 1] =
                        oVertexAttribute;
                }
            }
        }

        for (Int32 i = 0; i < iVertexAttributes; i++)
        {
            aoVertexAttributeColumnData[i].set_Value( Missing.Value,
                aaoVertexAttributeValues[i] );
        }
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
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
