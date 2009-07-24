
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Windows.Forms;
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

        TableImagePopulator.DeleteImagesInColumn(workbook,
            WorksheetNames.Vertices, VertexTableColumnNames.SubgraphImage);

        ExcelUtil.ClearTables(workbook,
            WorksheetNames.Edges, TableNames.Edges,
            WorksheetNames.Vertices, TableNames.Vertices,
            WorksheetNames.Clusters, TableNames.Clusters,
            WorksheetNames.ClusterVertices, TableNames.ClusterVertices,
            WorksheetNames.OverallMetrics, TableNames.OverallMetrics
            );

        ( new PerWorkbookSettings(workbook) ).OnWorkbookTablesCleared();
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

        if (m_oColorDialog == null)
        {
            m_oColorDialog = new ColorDialog();
        }

        if (m_oColorDialog.ShowDialog() == DialogResult.OK)
        {
            color = ( new ColorConverter2() ).GraphToWorkbook(
                m_oColorDialog.Color);

            return (true);
        }

        return (false);
    }


    //*************************************************************************
    //  Private fields
    //*************************************************************************

    /// ColorDialog used by TryGetColor(), or null if TryGetColor() hasn't been
    /// called yet.  This is static so that the dialog will retain custom
    /// colors between invocations.

    private static ColorDialog m_oColorDialog = null;
}

}
