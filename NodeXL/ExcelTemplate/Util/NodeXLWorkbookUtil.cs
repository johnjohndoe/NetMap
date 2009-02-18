
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.Office.Interop.Excel;
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
    //  Method: ClearTables()
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
    ClearTables
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        Debug.Assert(workbook != null);

        ExcelUtil.ClearTables(workbook,
            WorksheetNames.Edges, TableNames.Edges,
            WorksheetNames.Vertices, TableNames.Vertices,
            WorksheetNames.Clusters, TableNames.Clusters,
            WorksheetNames.ClusterVertices, TableNames.ClusterVertices,
            WorksheetNames.OverallMetrics, TableNames.OverallMetrics
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
        ParticipantPair [] participantPairs
    )
    {
        Debug.Assert(edgeTable != null);
        Debug.Assert(participantPairs != null);

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
        
        if ( ExcelUtil.TryGetTableColumnData(edgeTable,
            EdgeTableColumnNames.Vertex1Name, out oVertexColumnData) )
        {
            ExcelUtil.SetRangeValues(oVertexColumnData, aoVertex1Names);
        }

        if ( ExcelUtil.TryGetTableColumnData(edgeTable,
            EdgeTableColumnNames.Vertex2Name, out oVertexColumnData) )
        {
            ExcelUtil.SetRangeValues(oVertexColumnData, aoVertex2Names);
        }
    }
}

}
