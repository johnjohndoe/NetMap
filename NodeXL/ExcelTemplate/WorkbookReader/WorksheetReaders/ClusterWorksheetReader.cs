
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Visualization.Wpf;
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
    /// Reads the cluster worksheets and adds the contents to a graph.
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
            // The code that reads the tables can handle hidden rows, but not
            // hidden columns.  Temporarily show all hidden columns in the
            // table.

            ExcelHiddenColumns oHiddenClusterColumns =
                ExcelColumnHider.ShowHiddenColumns(oClusterTable);

            ExcelHiddenColumns oHiddenClusterVertexColumns =
                ExcelColumnHider.ShowHiddenColumns(oClusterVertexTable);

            try
            {
                ReadClusterTables(oClusterTable, oClusterVertexTable,
                    readWorkbookContext, graph);
            }
            finally
            {
                ExcelColumnHider.RestoreHiddenColumns(oClusterTable,
                    oHiddenClusterColumns);

                ExcelColumnHider.RestoreHiddenColumns(oClusterVertexTable,
                    oHiddenClusterVertexColumns);
            }
        }
    }

    //*************************************************************************
    //  Method: ReadClusterTables()
    //
    /// <summary>
    /// Reads the cluster tables and add the contents to a graph.
    /// </summary>
    ///
    /// <param name="oClusterTable">
    /// Table that contains the cluster data.
    /// </param>
    ///
    /// <param name="oClusterVertexTable">
    /// Table that contains the cluster vertex data.
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
    ReadClusterTables
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

        // If a required column is missing, do nothing.

        ListColumn oColumn;

        if (
            !ExcelUtil.TryGetTableColumn(oClusterTable,
                ClusterTableColumnNames.Name, out oColumn)
            ||
            !ExcelUtil.TryGetTableColumn(oClusterTable,
                ClusterTableColumnNames.VertexColor, out oColumn)
            ||
            !ExcelUtil.TryGetTableColumn(oClusterTable,
                ClusterTableColumnNames.VertexShape, out oColumn)
            ||
            !ExcelUtil.TryGetTableColumn(oClusterVertexTable,
                ClusterVertexTableColumnNames.ClusterName, out oColumn)
            ||
            !ExcelUtil.TryGetTableColumn(oClusterVertexTable,
                ClusterVertexTableColumnNames.VertexName, out oColumn)
            )
        {
            return;
        }

        // Create a dictionary from the cluster table.  The key is the cluster
        // name and the value is a ClusterInformation object for the cluster.

        Dictionary<String, ClusterInformation> oClusterNameDictionary =
            ReadClusterTable(oClusterTable);

        // Read the cluster vertex table and add the cluster vertex information
        // to the graph.

        ReadClusterVertexTable(oClusterVertexTable, oClusterNameDictionary,
            oReadWorkbookContext.VertexNameDictionary, oGraph);
    }

    //*************************************************************************
    //  Method: ReadClusterTable()
    //
    /// <summary>
    /// Reads the cluster table.
    /// </summary>
    ///
    /// <param name="oClusterTable">
    /// The cluster table.
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
        ListObject oClusterTable
    )
    {
        Debug.Assert(oClusterTable != null);
        AssertValid();

        Dictionary<String, ClusterInformation> oClusterNameDictionary =
            new Dictionary<String, ClusterInformation>();

        ColorConverter2 oColorConverter2 = new ColorConverter2();

        ExcelTableReader oExcelTableReader =
            new ExcelTableReader(oClusterTable);

        foreach ( ExcelTableReader.ExcelTableRow oRow in
            oExcelTableReader.GetRows() )
        {
            // Get the cluster information.

            String sClusterName;
            Color oVertexColor;
            VertexShape eVertexShape;

            if (
                !oRow.TryGetNonEmptyStringFromCell(
                    ClusterTableColumnNames.Name, out sClusterName)
                ||
                !TryGetColor(oRow, ClusterTableColumnNames.VertexColor,
                    oColorConverter2, out oVertexColor)
                ||
                !TryGetVertexShape(oRow, ClusterTableColumnNames.VertexShape,
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
                Range oInvalidCell = oRow.GetRangeForCell(
                    ClusterTableColumnNames.Name);

                OnWorkbookFormatError( String.Format(

                    "The cell {0} contains a duplicate cluster name.  There"
                    + " can't be two rows with the same cluster name."
                    ,
                    ExcelUtil.GetRangeAddress(oInvalidCell)
                    ),

                    oInvalidCell
                );
            }
        }

        return (oClusterNameDictionary);
    }

    //*************************************************************************
    //  Method: ReadClusterVertexTable()
    //
    /// <summary>
    /// Reads the cluster vertex table.
    /// </summary>
    ///
    /// <param name="oClusterVertexTable">
    /// The cluster vertex table.
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
        ListObject oClusterVertexTable,
        Dictionary<String, ClusterInformation> oClusterNameDictionary,
        Dictionary<String, IVertex> oVertexNameDictionary,
        IGraph oGraph
    )
    {
        Debug.Assert(oClusterVertexTable != null);
        Debug.Assert(oClusterNameDictionary != null);
        Debug.Assert(oVertexNameDictionary != null);
        Debug.Assert(oGraph != null);
        AssertValid();

        ExcelTableReader oExcelTableReader =
            new ExcelTableReader(oClusterVertexTable);

        foreach ( ExcelTableReader.ExcelTableRow oRow in
            oExcelTableReader.GetRows() )
        {
            // Get the cluster vertex information from the row.

            String sClusterName, sVertexName;

            if (
                !oRow.TryGetNonEmptyStringFromCell(
                    ClusterVertexTableColumnNames.ClusterName,
                    out sClusterName)
                ||
                !oRow.TryGetNonEmptyStringFromCell(
                    ClusterVertexTableColumnNames.VertexName, out sVertexName)
                )
            {
                continue;
            }

            // Get the cluster information for the vertex and store the cluster
            // information in the vertex.

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

        public VertexShape VertexShape;
    }
}

}
