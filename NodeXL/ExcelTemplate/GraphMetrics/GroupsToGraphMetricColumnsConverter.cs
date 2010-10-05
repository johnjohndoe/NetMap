
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Visualization.Wpf;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GroupsToGraphMetricColumnsConverter
//
/// <summary>
/// Converts a collection of vertex groups to a collection of <see
/// cref="GraphMetricColumn" /> objects.
/// </summary>
///
/// <remarks>
/// This is used by graph metric calculator classes that partition the graph
/// into a collection of groups.  They use the <see cref="Convert" /> method
/// to convert the collection of groups, the type of which is determined by
/// the graph metric calculator class, into a collection of <see
/// cref="GraphMetricColumn" /> objects suitable for populating the workbook.
///
/// <para>
/// All methods are static.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class GroupsToGraphMetricColumnsConverter
{
    //*************************************************************************
    //  Delegate: GroupToGroupVertices()
    //
    /// <summary>
    /// Gets the collection of vertices in a group.
    /// </summary>
    ///
    /// <typeparam name="TGroup">
    /// The type of the group.
    /// </typeparam>
    ///
    /// <param name="group">
    /// The group to get the vertices for.
    /// </param>
    ///
    /// <returns>
    /// The collection of vertices in the group.
    /// </returns>
    //*************************************************************************

    public delegate ICollection<IVertex>
        GroupToGroupVertices<TGroup>(TGroup group);


    //*************************************************************************
    //  Method: Convert()
    //
    /// <summary>
    /// Converts a collection of groups to an array of <see
    /// cref="GraphMetricColumn" /> objects.
    /// </summary>
    ///
    /// <typeparam name="TGroup">
    /// The type of the group.
    /// </typeparam>
    ///
    /// <param name="groups">
    /// Collection of groups, each of which is of type TGroup.
    /// </param>
    ///
    /// <param name="groupToGroupVertices">
    /// Method to get the collection of vertices in a group.
    /// </param>
    ///
    /// <returns>
    /// An array of <see cref="GraphMetricColumn" /> objects.
    /// </returns>
    //*************************************************************************

    public static GraphMetricColumn []
    Convert<TGroup>
    (
        ICollection<TGroup> groups,
        GroupToGroupVertices<TGroup> groupToGroupVertices
    )
    {
        Debug.Assert(groups != null);
        Debug.Assert(groupToGroupVertices != null);

        // These columns are needed:
        //
        // * Group name on the group worksheet.
        //
        // * Vertex color on the group worksheet.
        //
        // * Vertex shape on the group worksheet.
        //
        // * Collapsed flag on the group worksheet.
        //
        // * Group name on the group-vertex worksheet.
        //
        // * Vertex name on the group-vertex worksheet.
        //
        // * Vertex ID on the group-vertex worksheet.  This gets copied from
        //   the hidden ID column on the Vertex worksheet via an Excel
        //   formula.


        List<GraphMetricValueOrdered> oGroupNamesForGroupWorksheet =
            new List<GraphMetricValueOrdered>();

        List<GraphMetricValueOrdered> oVertexColorsForGroupWorksheet =
            new List<GraphMetricValueOrdered>();

        List<GraphMetricValueOrdered> oVertexShapesForGroupWorksheet =
            new List<GraphMetricValueOrdered>();

        // This column is empty.

        GraphMetricValueOrdered [] aoCollapsedFlagsForGroupWorksheet =
            new GraphMetricValueOrdered[0];

        List<GraphMetricValueOrdered> oGroupNamesForGroupVertexWorksheet =
            new List<GraphMetricValueOrdered>();

        List<GraphMetricValueOrdered> oVertexNamesForGroupVertexWorksheet =
            new List<GraphMetricValueOrdered>();

        // This column contains a constant value, which is a formula.

        GraphMetricValueOrdered [] aoVertexIDsForGroupVertexWorksheet = {
            new GraphMetricValueOrdered(
                GroupManager.GetExcelFormulaForVertexID() )
            };

        Int32 iGroups = groups.Count;
        Int32 iGroup = 0;

        ColorConverter2 oColorConverter2 = new ColorConverter2();

        VertexShapeConverter oVertexShapeConverter =
            new VertexShapeConverter();

        foreach (TGroup oGroup in groups)
        {
            String sGroupName = 'G' + (iGroup + 1).ToString(
                CultureInfo.InvariantCulture);

            Color oColor;
            VertexShape eShape;

            GroupManager.GetVertexAttributes(iGroup, iGroups, out oColor,
                out eShape);

            GraphMetricValueOrdered oGroupNameGraphMetricValue =
                new GraphMetricValueOrdered(sGroupName);

            oGroupNamesForGroupWorksheet.Add(oGroupNameGraphMetricValue);

            // Write the color in a format that is understood by
            // ColorConverter2.WorkbookToGraph(), which is what
            // WorksheetReaderBase uses.

            oVertexColorsForGroupWorksheet.Add(
                new GraphMetricValueOrdered(
                    oColorConverter2.GraphToWorkbook(oColor) ) );

            oVertexShapesForGroupWorksheet.Add(
                new GraphMetricValueOrdered(
                    oVertexShapeConverter.GraphToWorkbook(eShape) ) );

            Int32 iVertices = 0;

            foreach ( IVertex oVertex in groupToGroupVertices(oGroup) )
            {
                oGroupNamesForGroupVertexWorksheet.Add(
                    oGroupNameGraphMetricValue);

                oVertexNamesForGroupVertexWorksheet.Add(
                    new GraphMetricValueOrdered(
                        ExcelUtil.ForceCellText(oVertex.Name) ) );

                iVertices++;
            }

            iGroup++;
        }

        return ( new GraphMetricColumn [] {

            new GraphMetricColumnOrdered( WorksheetNames.Groups,
                TableNames.Groups,
                GroupTableColumnNames.Name,
                ExcelUtil.AutoColumnWidth, null, CellStyleNames.Required,
                oGroupNamesForGroupWorksheet.ToArray()
                ),

            new GraphMetricColumnOrdered( WorksheetNames.Groups,
                TableNames.Groups,
                GroupTableColumnNames.VertexColor,
                ExcelUtil.AutoColumnWidth, null, CellStyleNames.VisualProperty,
                oVertexColorsForGroupWorksheet.ToArray()
                ),

            new GraphMetricColumnOrdered( WorksheetNames.Groups,
                TableNames.Groups,
                GroupTableColumnNames.VertexShape,
                ExcelUtil.AutoColumnWidth, null, CellStyleNames.VisualProperty,
                oVertexShapesForGroupWorksheet.ToArray()
                ),

            new GraphMetricColumnOrdered( WorksheetNames.Groups,
                TableNames.Groups,
                GroupTableColumnNames.Collapsed,
                ExcelUtil.AutoColumnWidth, null, CellStyleNames.VisualProperty,
                aoCollapsedFlagsForGroupWorksheet
                ),

            new GraphMetricColumnOrdered( WorksheetNames.GroupVertices,
                TableNames.GroupVertices,
                GroupVertexTableColumnNames.GroupName,
                ExcelUtil.AutoColumnWidth, null, null,
                oGroupNamesForGroupVertexWorksheet.ToArray()
                ),

            new GraphMetricColumnOrdered( WorksheetNames.GroupVertices,
                TableNames.GroupVertices,
                GroupVertexTableColumnNames.VertexName,
                ExcelUtil.AutoColumnWidth, null, null,
                oVertexNamesForGroupVertexWorksheet.ToArray()
                ),

            new GraphMetricColumnOrdered( WorksheetNames.GroupVertices,
                TableNames.GroupVertices,
                GroupVertexTableColumnNames.VertexID,
                ExcelUtil.AutoColumnWidth, null, null,
                aoVertexIDsForGroupVertexWorksheet
                ),
                } );
    }
}

}
