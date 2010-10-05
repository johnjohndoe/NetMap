
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;
using Microsoft.VisualStudio.Tools.Applications;
using System.Diagnostics;
using Microsoft.SocialNetworkLib;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: NodeXLWorkbookUtil
//
/// <summary>
/// Utility methods for working with the NodeXL workbook.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class NodeXLWorkbookUtil
{
    //*************************************************************************
    //  Method: FileIsNodeXLWorkbook()
    //
    /// <summary>
    /// Gets a flag indicating whether a file is a NodeXL workbook.
    /// </summary>
    ///
    /// <param name="filePath">
    /// Path to the file.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="filePath" /> is a NodeXL workbook.
    /// </returns>
    ///
    /// <exception cref="System.IO.IOException">
    /// Occurs when a workbook is open in Excel, or has any other problem that
    /// prevents it from being opened.
    /// </exception>
    //*************************************************************************

    public static Boolean
    FileIsNodeXLWorkbook
    (
        String filePath
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(filePath) );

        if ( ServerDocument.IsCustomized(filePath) )
        {
            Guid oSolutionID = new Guid(ApplicationUtil.SolutionID);

            using ( ServerDocument oServerDocument =
                new ServerDocument(filePath) )
            {
                if (oServerDocument.SolutionId == oSolutionID)
                {
                    return (true);
                }
            }
        }

        return (false);
    }

    //*************************************************************************
    //  Method: ClearAllNodeXLTables()
    //
    /// <summary>
    /// Clears all the tables in the specified NodeXL workbook.
    /// </summary>
    ///
    /// <param name="workbook">
    /// NodeXL workbook.
    /// </param>
    ///
    /// <remarks>
    /// This method reduces the NodeXL tables to their header row and one data
    /// body row.  The data body row contains the original formatting and data
    /// validation, but its contents are cleared.
    ///
    /// <para>
    /// If a table doesn't exist, this method skips it.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static void
    ClearAllNodeXLTables
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        Debug.Assert(workbook != null);

        TableImagePopulator.DeleteImagesInColumn(workbook,
            WorksheetNames.Vertices, VertexTableColumnNames.SubgraphImage);

        ExcelUtil.ClearTables(workbook,
            WorksheetNames.Edges, TableNames.Edges,
            WorksheetNames.Vertices, TableNames.Vertices,
            WorksheetNames.OverallMetrics, TableNames.OverallMetrics,
            WorksheetNames.Miscellaneous, TableNames.DynamicFilterSettings
            );

        ClearGroupTables(workbook);

        ( new PerWorkbookSettings(workbook) ).OnWorkbookTablesCleared();
    }

    //*************************************************************************
    //  Method: ClearGroupTables()
    //
    /// <summary>
    /// Clears the group tables in the specified NodeXL workbook.
    /// </summary>
    ///
    /// <param name="workbook">
    /// NodeXL workbook.
    /// </param>
    ///
    /// <remarks>
    /// This method reduces the group and group-vertex NodeXL tables to their
    /// header row and one data body row.  The data body row contains the
    /// original formatting and data validation, but its contents are cleared.
    ///
    /// <para>
    /// If a table doesn't exist, this method skips it.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static void
    ClearGroupTables
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        Debug.Assert(workbook != null);

        ExcelUtil.ClearTables(workbook,
            WorksheetNames.Groups, TableNames.Groups,
            WorksheetNames.GroupVertices, TableNames.GroupVertices
            );
    }

    //*************************************************************************
    //  Method: PopulateEdgeTableWithParticipantPairs()
    //
    /// <summary>
    /// Populates the edge table with an array of participant pairs.
    /// </summary>
    ///
    /// <param name="edgeTable">
    /// The edge table to populate.
    /// </param>
    ///
    /// <param name="participantPairs">
    /// Zero or more <see cref="ParticipantPair" /> objects.
    /// </param>
    ///
    /// <param name="edgeTableRowOffset">
    /// Offset of the first row in the edge table to write to, measured from
    /// the first data body row.
    /// </param>
    ///
    /// <remarks>
    /// This method populates the edge table with pairs of participants in a
    /// social network.  The Vertex 1 column is filled with the first
    /// participant in each pair and the Vertex 2 column is filled with the 
    /// second participant in each pair.
    ///
    /// <para>
    /// If either column is missing, this method skips it.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static void
    PopulateEdgeTableWithParticipantPairs
    (
        ListObject edgeTable,
        ParticipantPair [] participantPairs,
        Int32 edgeTableRowOffset
    )
    {
        Debug.Assert(edgeTable != null);
        Debug.Assert(participantPairs != null);
        Debug.Assert(edgeTableRowOffset >= 0);

        Int32 iParticipantPairs = participantPairs.Length;

        if (iParticipantPairs == 0)
        {
            return;
        }

        // Create and populate arrays of vertex names.

        Object [,] aoVertex1Names = new Object [iParticipantPairs, 1];
        Object [,] aoVertex2Names = new Object [iParticipantPairs, 1];

        for (Int32 i = 0; i < iParticipantPairs; i++)
        {
            ParticipantPair oParticipantPair = participantPairs[i];

            aoVertex1Names[i, 0] = oParticipantPair.Participant1;
            aoVertex2Names[i, 0] = oParticipantPair.Participant2;
        }

        // Write the arrays to the edge table.

        Range oVertexColumnData;

        foreach ( String sColumnName in new String [] {
            EdgeTableColumnNames.Vertex1Name,
            EdgeTableColumnNames.Vertex2Name} )
        {
            if ( ExcelUtil.TryGetTableColumnData(edgeTable, sColumnName,
                out oVertexColumnData) )
            {
                ExcelUtil.OffsetRange(ref oVertexColumnData,
                    edgeTableRowOffset, 0);

                ExcelUtil.SetRangeValues(oVertexColumnData,
                    sColumnName == EdgeTableColumnNames.Vertex1Name ?
                    aoVertex1Names : aoVertex2Names);
            }
        }
    }

    //*************************************************************************
    //  Method: GetVertexIDsInGroup()
    //
    /// <summary>
    /// Gets the row IDs of the vertices in a specified group.
    /// </summary>
    ///
    /// <param name="workbook">
    /// NodeXL workbook.
    /// </param>
    ///
    /// <param name="groupName">
    /// Name of the group to get the IDs for.
    /// </param>
    ///
    /// <returns>
    /// A collection of vertex row IDs for the specified group.  The collection
    /// can be empty but is never null.
    /// </returns>
    ///
    /// <remarks>
    /// The returned IDs are the vertex IDs from the vertex table for the
    /// vertices in the specified group.
    /// </remarks>
    //*************************************************************************

    public static ICollection<Int32>
    GetVertexIDsInGroup
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        String groupName
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert( !String.IsNullOrEmpty(groupName) );

        ListObject oGroupVertexTable;
        LinkedList<Int32> oVertexIDsInGroup = new LinkedList<Int32>();

        if ( ExcelUtil.TryGetTable(workbook, WorksheetNames.GroupVertices,
                TableNames.GroupVertices, out oGroupVertexTable) )
        {
            foreach ( ExcelTableReader.ExcelTableRow oRow in
                ( new ExcelTableReader(oGroupVertexTable) ).GetRows() )
            {
                String sGroupName;
                Int32 iVertexID;

                if (
                    oRow.TryGetNonEmptyStringFromCell(
                        GroupVertexTableColumnNames.GroupName, out sGroupName)
                    &&
                    sGroupName == groupName
                    &&
                    oRow.TryGetInt32FromCell(
                        GroupVertexTableColumnNames.VertexID, out iVertexID)
                    )
                {
                    oVertexIDsInGroup.AddLast(iVertexID);
                }
            }
        }

        return (oVertexIDsInGroup);
    }

    //*************************************************************************
    //  Method: GetGroupNamesByVertexName()
    //
    /// <summary>
    /// Gets the names of the groups containing a specified collection of
    /// vertex names.
    /// </summary>
    ///
    /// <param name="workbook">
    /// NodeXL workbook.
    /// </param>
    ///
    /// <param name="vertexNames">
    /// A collection of vertex names.
    /// </param>
    ///
    /// <returns>
    /// A collection of unique group names, one for each group that contains a
    /// vertex in <paramref name="vertexNames"/>.  The collection can be empty
    /// but is never null.
    /// </returns>
    //*************************************************************************

    public static ICollection<String>
    GetGroupNamesByVertexName
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        ICollection<String> vertexNames
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert(vertexNames != null);

        // Store the vertex names in a HashSet for quick lookup.

        HashSet<String> oVertexNames = new HashSet<String>(vertexNames);

        HashSet<String> oGroupNames = new HashSet<String>();

        ListObject oGroupVertexTable;

        if ( ExcelUtil.TryGetTable(workbook, WorksheetNames.GroupVertices,
                TableNames.GroupVertices, out oGroupVertexTable) )
        {
            foreach ( ExcelTableReader.ExcelTableRow oRow in
                ( new ExcelTableReader(oGroupVertexTable) ).GetRows() )
            {
                String sGroupName, sVertexName;

                if (
                    oRow.TryGetNonEmptyStringFromCell(
                        GroupVertexTableColumnNames.VertexName,
                        out sVertexName)
                    &&
                    oVertexNames.Contains(sVertexName)
                    &&
                    oRow.TryGetNonEmptyStringFromCell(
                        GroupVertexTableColumnNames.GroupName, out sGroupName)
                    )
                {
                    oGroupNames.Add(sGroupName);
                }
            }
        }

        return (oGroupNames);
    }

    //*************************************************************************
    //  Method: TryGetColor()
    //
    /// <summary>
    /// Attempts to get a color from the user in a format appropriate for use
    /// in the workbook.
    /// </summary>
    ///
    /// <param name="color">
    /// Where the color gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if a color was obtained from the user, false if the user
    /// cancelled.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryGetColor
    (
        out String color
    )
    {
        color = null;

        // A single ColorDialog instance is used throughout the application to
        // allow any custom colors the user creates to be retained across
        // invocations.

        ColorDialog oColorDialog = ColorPicker.ColorDialog;

        if (oColorDialog.ShowDialog() == DialogResult.OK)
        {
            color = ( new ColorConverter2() ).GraphToWorkbook(
                oColorDialog.Color);

            return (true);
        }

        return (false);
    }
}

}
