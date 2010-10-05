
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
//  Class: GroupWorksheetReader
//
/// <summary>
/// Class that knows how to read Excel worksheets containing group data.
/// </summary>
///
/// <remarks>
/// Call <see cref="ReadWorksheet" /> to read the group worksheets.
/// </remarks>
//*****************************************************************************

public class GroupWorksheetReader : WorksheetReaderBase
{
    //*************************************************************************
    //  Constructor: GroupWorksheetReader()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupWorksheetReader" />
    /// class.
    /// </summary>
    //*************************************************************************

    public GroupWorksheetReader()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: ReadWorksheet()
    //
    /// <summary>
    /// Reads the group worksheets and adds the contents to a graph.
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
    /// Graph to add group data to.
    /// </param>
    ///
    /// <remarks>
    /// If the group worksheets in <paramref name="workbook" /> contain valid
    /// group data, the data is added to <paramref name="graph" />.  Otherwise,
    /// a <see cref="WorkbookFormatException" /> is thrown.
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

        // Attempt to get the optional tables that contain group data.

        ListObject oGroupTable, oGroupVertexTable;

        if (
            ExcelUtil.TryGetTable(workbook, WorksheetNames.Groups,
                TableNames.Groups, out oGroupTable)
            &&
            ExcelUtil.TryGetTable(workbook, WorksheetNames.GroupVertices,
                TableNames.GroupVertices, out oGroupVertexTable)
            )
        {
            // The code that reads the tables can handle hidden rows, but not
            // hidden columns.  Temporarily show all hidden columns in the
            // table.

            ExcelHiddenColumns oHiddenGroupColumns =
                ExcelColumnHider.ShowHiddenColumns(oGroupTable);

            ExcelHiddenColumns oHiddenGroupVertexColumns =
                ExcelColumnHider.ShowHiddenColumns(oGroupVertexTable);

            try
            {
                ReadGroupTables(oGroupTable, oGroupVertexTable,
                    readWorkbookContext, graph);
            }
            finally
            {
                ExcelColumnHider.RestoreHiddenColumns(oGroupTable,
                    oHiddenGroupColumns);

                ExcelColumnHider.RestoreHiddenColumns(oGroupVertexTable,
                    oHiddenGroupVertexColumns);
            }
        }
    }

    //*************************************************************************
    //  Method: VertexIsCollapsedGroup()
    //
    /// <summary>
    /// Returns a flag indicating whether a vertex represents a collapsed
    /// group.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex to check.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="vertex" /> represents a collapsed group, false
    /// if it is a normal vertex.
    /// </returns>
    ///
    /// <remarks>
    /// A collapsed group is represented by a vertex that differs in appearance
    /// from a regular vertex.  It corresponds to a row in the group worksheet
    /// instead of a row in the vertex worksheet.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    VertexIsCollapsedGroup
    (
        IVertex vertex
    )
    {
        Debug.Assert(vertex != null);

        // A normal vertex has its Tag set to the vertex's row ID in the vertex
        // worksheet.  A vertex that represents a collapsed group does not have
        // its Tag set.

        return ( !(vertex.Tag is Int32) );
    }

    //*************************************************************************
    //  Method: ReadGroupTables()
    //
    /// <summary>
    /// Reads the group tables and add the contents to a graph.
    /// </summary>
    ///
    /// <param name="oGroupTable">
    /// Table that contains the group data.
    /// </param>
    ///
    /// <param name="oGroupVertexTable">
    /// Table that contains the group vertex data.
    /// </param>
    ///
    /// <param name="oReadWorkbookContext">
    /// Provides access to objects needed for converting an Excel workbook to a
    /// NodeXL graph.
    /// </param>
    ///
    /// <param name="oGraph">
    /// Graph to add group data to.
    /// </param>
    //*************************************************************************

    protected void
    ReadGroupTables
    (
        ListObject oGroupTable,
        ListObject oGroupVertexTable,
        ReadWorkbookContext oReadWorkbookContext,
        IGraph oGraph
    )
    {
        Debug.Assert(oGroupTable != null);
        Debug.Assert(oGroupVertexTable != null);
        Debug.Assert(oReadWorkbookContext != null);
        Debug.Assert(oGraph != null);
        AssertValid();

        // If a required column is missing, do nothing.

        ListColumn oColumn;

        if (
            !ExcelUtil.TryGetTableColumn(oGroupTable,
                GroupTableColumnNames.Name, out oColumn)
            ||
            !ExcelUtil.TryGetTableColumn(oGroupTable,
                GroupTableColumnNames.VertexColor, out oColumn)
            ||
            !ExcelUtil.TryGetTableColumn(oGroupTable,
                GroupTableColumnNames.VertexShape, out oColumn)
            ||
            !ExcelUtil.TryGetTableColumn(oGroupVertexTable,
                GroupVertexTableColumnNames.GroupName, out oColumn)
            ||
            !ExcelUtil.TryGetTableColumn(oGroupVertexTable,
                GroupVertexTableColumnNames.VertexName, out oColumn)
            )
        {
            return;
        }

        // Create a dictionary from the group table.  The key is the group name
        // and the value is a GroupInformation object for the group.

        Dictionary<String, GroupInformation> oGroupNameDictionary =
            ReadGroupTable(oGroupTable, oReadWorkbookContext);

        // Read the group vertex table and set the color and shape of each
        // group vertex in the graph.

        ReadGroupVertexTable(oGroupVertexTable, oReadWorkbookContext,
            oGroupNameDictionary, oGraph);

        // Save the group information on the graph.

        Debug.Assert( oGroupNameDictionary.Values is
            ICollection<GroupInformation> );

        oGraph.SetValue(ReservedMetadataKeys.GroupInformation,
            oGroupNameDictionary.Values);
    }

    //*************************************************************************
    //  Method: ReadGroupTable()
    //
    /// <summary>
    /// Reads the group table.
    /// </summary>
    ///
    /// <param name="oGroupTable">
    /// The group table.
    /// </param>
    ///
    /// <param name="oReadWorkbookContext">
    /// Provides access to objects needed for converting an Excel workbook to a
    /// NodeXL graph.
    /// </param>
    ///
    /// <returns>
    /// A dictionary.  The key is the group name and the value is a
    /// GroupInformation object for the group.
    /// </returns>
    //*************************************************************************

    protected Dictionary<String, GroupInformation>
    ReadGroupTable
    (
        ListObject oGroupTable,
        ReadWorkbookContext oReadWorkbookContext
    )
    {
        Debug.Assert(oGroupTable != null);
        Debug.Assert(oReadWorkbookContext != null);
        AssertValid();

        if (oReadWorkbookContext.FillIDColumns)
        {
            FillIDColumn(oGroupTable);
        }

        Dictionary<String, GroupInformation> oGroupNameDictionary =
            new Dictionary<String, GroupInformation>();

        ColorConverter2 oColorConverter2 =
            oReadWorkbookContext.ColorConverter2;

        BooleanConverter oBooleanConverter =
            oReadWorkbookContext.BooleanConverter;

        ExcelTableReader oExcelTableReader = new ExcelTableReader(oGroupTable);

        foreach ( ExcelTableReader.ExcelTableRow oRow in
            oExcelTableReader.GetRows() )
        {
            // Get the group information.

            String sGroupName;
            Color oVertexColor;
            VertexShape eVertexShape;

            if (
                !oRow.TryGetNonEmptyStringFromCell(GroupTableColumnNames.Name,
                    out sGroupName)
                ||
                !TryGetColor(oRow, GroupTableColumnNames.VertexColor,
                    oColorConverter2, out oVertexColor)
                ||
                !TryGetVertexShape(oRow, GroupTableColumnNames.VertexShape,
                    out eVertexShape)
                )
            {
                continue;
            }

            Boolean bCollapsed = false;
            Boolean bCollapsedCellValue;

            if (
                TryGetBoolean(oRow, GroupTableColumnNames.Collapsed,
                    oBooleanConverter, out bCollapsedCellValue)
                &&
                bCollapsedCellValue
                )
            {
                bCollapsed = true;
            }

            Int32 iRowIDAsInt32;
            Nullable<Int32> iRowID = null;

            if ( oRow.TryGetInt32FromCell(CommonTableColumnNames.ID,
                out iRowIDAsInt32) )
            {
                iRowID = iRowIDAsInt32;
            }

            GroupInformation oGroupInformation = new GroupInformation(
                sGroupName, iRowID, oVertexColor, eVertexShape, bCollapsed);

            if (oReadWorkbookContext.SaveGroupVertices)
            {
                // ReadGroupVertexTable() will save the group's vertices in
                // this LinkedList.

                oGroupInformation.Vertices = new LinkedList<IVertex>();
            }

            try
            {
                oGroupNameDictionary.Add(sGroupName, oGroupInformation);
            }
            catch (ArgumentException)
            {
                Range oInvalidCell = oRow.GetRangeForCell(
                    GroupTableColumnNames.Name);

                OnWorkbookFormatError( String.Format(

                    "The cell {0} contains a duplicate group name.  There"
                    + " can't be two rows with the same group name."
                    ,
                    ExcelUtil.GetRangeAddress(oInvalidCell)
                    ),

                    oInvalidCell
                );
            }
        }

        return (oGroupNameDictionary);
    }

    //*************************************************************************
    //  Method: ReadGroupVertexTable()
    //
    /// <summary>
    /// Reads the group vertex table.
    /// </summary>
    ///
    /// <param name="oGroupVertexTable">
    /// The group vertex table.
    /// </param>
    ///
    /// <param name="oReadWorkbookContext">
    /// Provides access to objects needed for converting an Excel workbook to a
    /// NodeXL graph.
    /// </param>
    ///
    /// <param name="oGroupNameDictionary">
    /// The key is the group name and the value is the GroupInformation object
    /// for the group.
    /// </param>
    ///
    /// <param name="oGraph">
    /// Graph to add group data to.
    /// </param>
    //*************************************************************************

    protected void
    ReadGroupVertexTable
    (
        ListObject oGroupVertexTable,
        ReadWorkbookContext oReadWorkbookContext,
        Dictionary<String, GroupInformation> oGroupNameDictionary,
        IGraph oGraph
    )
    {
        Debug.Assert(oGroupVertexTable != null);
        Debug.Assert(oReadWorkbookContext != null);
        Debug.Assert(oGroupNameDictionary != null);
        Debug.Assert(oGraph != null);
        AssertValid();

        Dictionary<String, IVertex> oVertexNameDictionary =
            oReadWorkbookContext.VertexNameDictionary;

        ExcelTableReader oExcelTableReader =
            new ExcelTableReader(oGroupVertexTable);

        foreach ( ExcelTableReader.ExcelTableRow oRow in
            oExcelTableReader.GetRows() )
        {
            // Get the group vertex information from the row.

            String sGroupName, sVertexName;

            if (
                !oRow.TryGetNonEmptyStringFromCell(
                    GroupVertexTableColumnNames.GroupName, out sGroupName)
                ||
                !oRow.TryGetNonEmptyStringFromCell(
                    GroupVertexTableColumnNames.VertexName, out sVertexName)
                )
            {
                continue;
            }

            // Get the group information for the vertex and store the group
            // information in the vertex.

            GroupInformation oGroupInformation;
            IVertex oVertex;

            if (
                !oGroupNameDictionary.TryGetValue(sGroupName,
                    out oGroupInformation)
                ||
                !oVertexNameDictionary.TryGetValue(sVertexName,
                    out oVertex)
                )
            {
                continue;
            }

            oVertex.SetValue(ReservedMetadataKeys.PerColor,
                oGroupInformation.VertexColor);

            oVertex.SetValue(ReservedMetadataKeys.PerVertexShape,
                oGroupInformation.VertexShape);

            if (oReadWorkbookContext.SaveGroupVertices)
            {
                Debug.Assert(oGroupInformation.Vertices != null);

                oGroupInformation.Vertices.Add(oVertex);
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
}

}
