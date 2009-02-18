
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.NodeXL.Core;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: PajekGraphImporter
//
/// <summary>
/// Imports edges and vertices from a graph created from a Pajek file to the
/// edge and vertex worksheets.
/// </summary>
///
/// <remarks>
/// This class is used when a Pajek file is imported into a NodeXL workbook.
/// The caller should first read the Pajek file into a NodeXL graph using <see
/// cref="Microsoft.NodeXL.Adapters.PajekGraphAdapter" />, then call the <see
/// cref="ImportPajekGraph" /> method in this class to import the edges and
/// vertices from the graph to a NodeXL workbook.
/// </remarks>
//*****************************************************************************

public class PajekGraphImporter : Object
{
    //*************************************************************************
    //  Constructor: PajekGraphImporter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="PajekGraphImporter" />
    /// class.
    /// </summary>
    //*************************************************************************

    public PajekGraphImporter()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: ImportPajekGraph()
    //
    /// <summary>
    /// Imports edges and vertices from a graph created from a Pajek file to
    /// the edge and vertex worksheets.
    /// </summary>
    ///
    /// <param name="sourceGraph">
    /// Graph to import the edges and vertices from.
    /// </param>
    ///
    /// <param name="destinationNodeXLWorkbook">
    /// NodeXL workbook the edges and vertices will be imported to.
    /// </param>
    ///
    /// <remarks>
    /// This method creates a row in the edge worksheet for each edge in
    /// <paramref name="sourceGraph" />, and a row in the vertex worksheet for
    /// each vertex.  Only the vertex names are imported; any edge and vertex
    /// attributes in the graph are ignored.
    /// </remarks>
    //*************************************************************************

    public void
    ImportPajekGraph
    (
        IGraph sourceGraph,
        Microsoft.Office.Interop.Excel.Workbook destinationNodeXLWorkbook
    )
    {
        Debug.Assert(sourceGraph != null);
        Debug.Assert(destinationNodeXLWorkbook != null);
        AssertValid();

        // Get the required table that contains edge data.  GetEdgeTable()
        // throws an exception if the table is missing.

        EdgeWorksheetReader oEdgeWorksheetReader = new EdgeWorksheetReader();

        ListObject oEdgeTable =
            oEdgeWorksheetReader.GetEdgeTable(destinationNodeXLWorkbook);

        // Get the required columns.

        Range oVertex1NameColumnData, oVertex2NameColumnData;

        if (
            !ExcelUtil.TryGetTableColumnData(oEdgeTable,
                EdgeTableColumnNames.Vertex1Name, out oVertex1NameColumnData)
            ||
            !ExcelUtil.TryGetTableColumnData(oEdgeTable,
                EdgeTableColumnNames.Vertex2Name, out oVertex2NameColumnData)
            )
        {
            throw new WorkbookFormatException( String.Format(

                "To use this feature, the worksheet named \"{0}\" must have a"
                + " table named \"{1}\" that contains edge data.\r\n\r\n{2}"
                ,
                WorksheetNames.Edges,
                TableNames.Edges,
                ErrorUtil.GetTemplateMessage()
                ) );
        }

        // Get the table that contains vertex data.  This table isn't normally
        // required, but because the Pajek file may have isolated vertices that
        // need to be written to the vertex worksheet, require it here.

        ListObject oVertexTable;
        Range oVertexNameColumnData, oVisibilityColumnData;

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
            throw new WorkbookFormatException( String.Format(

                "To use this feature, the worksheet named \"{0}\" must have a"
                + " table named \"{1}\" that contains vertex data.\r\n\r\n{2}"
                ,
                WorksheetNames.Vertices,
                TableNames.Vertices,
                ErrorUtil.GetTemplateMessage()
                ) );
        }

        // Clear the required tables and other optional tables.

        NodeXLWorkbookUtil.ClearTables(destinationNodeXLWorkbook);

        // Import the edges and isolated vertices into the workbook.

        ImportPajekEdges(sourceGraph, oEdgeTable, oVertex1NameColumnData,
            oVertex2NameColumnData);

        ImportPajekIsolatedVertices(sourceGraph, oVertexTable,
            oVertexNameColumnData, oVisibilityColumnData);

        // Populate the vertex worksheet with the name of each unique vertex in
        // the edge worksheet.

        VertexWorksheetPopulator oVertexWorksheetPopulator =
            new VertexWorksheetPopulator();

        oVertexWorksheetPopulator.PopulateVertexWorksheet(
            destinationNodeXLWorkbook, false);
    }

    //*************************************************************************
    //  Method: ImportPajekEdges()
    //
    /// <summary>
    /// Imports edges from a graph created from a Pajek file to the edge
    /// worksheet.
    /// </summary>
    ///
    /// <param name="oSourceGraph">
    /// Graph to import the edges from.
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
    //*************************************************************************

    protected void
    ImportPajekEdges
    (
        IGraph oSourceGraph,
        ListObject oEdgeTable,
        Range oVertex1NameColumnData,
        Range oVertex2NameColumnData
    )
    {
        Debug.Assert(oSourceGraph != null);
        Debug.Assert(oEdgeTable != null);
        Debug.Assert(oVertex1NameColumnData != null);
        Debug.Assert(oVertex2NameColumnData != null);
        AssertValid();

        IEdgeCollection oEdges = oSourceGraph.Edges;
        Int32 iEdges = oEdges.Count;

        // Create arrays that will be written to the edge table.

        Object [,] aoVertex1NameValues =
            ExcelUtil.GetSingleColumn2DArray(iEdges);

        Object [,] aoVertex2NameValues =
            ExcelUtil.GetSingleColumn2DArray(iEdges);

        Int32 iEdge = 1;

        foreach (IEdge oEdge in oEdges)
        {
            IVertex [] aoVertices = oEdge.Vertices;

            aoVertex1NameValues[iEdge, 1] = aoVertices[0].Name;
            aoVertex2NameValues[iEdge, 1] = aoVertices[1].Name;

            iEdge++;
        }

        ExcelUtil.SetRangeValues( (Range)oVertex1NameColumnData.Cells[1, 1],
            aoVertex1NameValues );

        ExcelUtil.SetRangeValues( (Range)oVertex2NameColumnData.Cells[1, 1],
            aoVertex2NameValues );
    }

    //*************************************************************************
    //  Method: ImportPajekIsolatedVertices()
    //
    /// <summary>
    /// Imports isolated vertices from a graph created from a Pajek file to the
    /// vertex worksheet.
    /// </summary>
    ///
    /// <param name="oSourceGraph">
    /// Graph to import the vertices from.
    /// </param>
    ///
    /// <param name="oVertexTable">
    /// Vertex table the isolated vertices will be imported to.
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
    ImportPajekIsolatedVertices
    (
        IGraph oSourceGraph,
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

        // Create a list of isolated vertices not included in the edge
        // worksheet.

        LinkedList<String> oIsolatedVertexNames = new LinkedList<String>();

        foreach (IVertex oVertex in oSourceGraph.Vertices)
        {
            if (oVertex.Degree == 0)
            {
                oIsolatedVertexNames.AddLast(oVertex.Name);
            }
        }

        Int32 iIsolatedVertices = oIsolatedVertexNames.Count;

        if (iIsolatedVertices == 0)
        {
            return;
        }

        // Create arrays that will be written to the vertex table.

        Object [,] aoVertexNameValues =
            ExcelUtil.GetSingleColumn2DArray(iIsolatedVertices);

        Object [,] aoVisibilityValues =
            ExcelUtil.GetSingleColumn2DArray(iIsolatedVertices);

        // The vertex visibilities should be set to Show to force them to be
        // read even though they are not included in edges.

        VertexVisibilityConverter oVertexVisibilityConverter =
            new VertexVisibilityConverter();

        String sShow = oVertexVisibilityConverter.GraphToWorkbook(
            VertexWorksheetReader.Visibility.Show);

        Int32 iIsolatedVertex = 1;

        foreach (String sIsolatedVertexName in oIsolatedVertexNames)
        {
            aoVertexNameValues[iIsolatedVertex, 1] = sIsolatedVertexName;
            aoVisibilityValues[iIsolatedVertex, 1] = sShow;

            iIsolatedVertex++;
        }

        ExcelUtil.SetRangeValues( (Range)oVertexNameColumnData.Cells[1, 1],
            aoVertexNameValues );

        ExcelUtil.SetRangeValues( (Range)oVisibilityColumnData.Cells[1, 1],
            aoVisibilityValues );
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
