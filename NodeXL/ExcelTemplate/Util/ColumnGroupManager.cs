
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Enum: ColumnGroup
//
/// <summary>
/// Specifies a group of related columns in a single table.
/// </summary>
//*****************************************************************************

public enum
ColumnGroup
{
    //*************************************************************************
    //  Edge table column groups
    //
    //  Important Note:
    //
    //  Column groups in the edge table must start with "Edge".
    //*************************************************************************

    /// <summary>
    /// The columns in the edge table that must always be visible.
    /// </summary>

    EdgeDoNotHide,

    /// <summary>
    /// The visual attribute columns in the edge table.
    /// </summary>

    EdgeVisualAttributes,

    /// <summary>
    /// The label columns in the edge table.
    /// </summary>

    EdgeLabels,

    /// <summary>
    /// The columns in the edge table that are used only by NodeXL and should
    /// not be edited by the user.
    /// </summary>

    EdgeInternalUse,

    /// <summary>
    /// All columns in the edge table that are not included in one of the
    /// previous groups.
    /// </summary>

    EdgeOtherColumns,

    // Important Note:
    //
    // When a column group is added, every switch statement in the
    // ColumnGroupManager class must be updated.


    //*************************************************************************
    //  Vertex table column groups
    //
    //  Important Note:
    //
    //  Column groups in the vertex table must start with "Vertex".
    //*************************************************************************

    /// <summary>
    /// The columns in the vertex table that must always be visible.
    /// </summary>

    VertexDoNotHide,

    /// <summary>
    /// The visual attribute columns in the vertex table.
    /// </summary>

    VertexVisualAttributes,

    /// <summary>
    /// The graph metric columns in the vertex table.
    /// </summary>

    VertexGraphMetrics,

    /// <summary>
    /// The label columns in the vertex table.
    /// </summary>

    VertexLabels,

    /// <summary>
    /// The layout columns in the vertex table.
    /// </summary>

    VertexLayout,

    /// <summary>
    /// The columns in the vertex table that are used only by NodeXL and should
    /// not be edited by the user.
    /// </summary>

    VertexInternalUse,

    /// <summary>
    /// All columns in the vertex table that are not included in one of the
    /// previous groups.
    /// </summary>

    VertexOtherColumns,

    // Important Note:
    //
    // When a column group is added, every switch statement in the
    // ColumnGroupManager class must be updated.
}


//*****************************************************************************
//  Class: ColumnGroupManager
//
/// <summary>
/// Hides and shows a group of related columns.
/// </summary>
///
/// <remarks>
/// Call <see cref="ShowOrHideColumnGroup" /> to show or hide a specified group
/// of related columns in a single table.  Call <see
/// cref="ShowOrHideAllColumnGroups" /> to show or hide all column groups in
/// the workbook.
///
/// <para>
/// All methods are static.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class ColumnGroupManager
{
    //*************************************************************************
    //  Method: ShowOrHideColumnGroup()
    //
    /// <summary>
    /// Shows or hides a specified group of related columns in a single table.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the column group.
    /// </param>
    ///
    /// <param name="columnGroup">
    /// The group of columns to show or hide.
    /// </param>
    ///
    /// <param name="show">
    /// true to show the column group, false to hide it.
    /// </param>
    ///
    /// <param name="activateWorksheet">
    /// true to activate the worksheet containing the column group after
    /// showing or hiding the group.
    /// </param>
    ///
    /// <remarks>
    /// Columns that don't exist are ignored.
    ///
    /// <para>
    /// The column group's new show/hide state is stored in the workbook using
    /// the <see cref="PerWorkbookSettings" /> class.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static void
    ShowOrHideColumnGroup
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        ColumnGroup columnGroup,
        Boolean show,
        Boolean activateWorksheet
    )
    {
        Debug.Assert(workbook != null);

        ListObject oTable;

        if ( TryGetColumnGroupTable(workbook, columnGroup, out oTable) )
        {
            ExcelColumnHider.ShowOrHideColumns(oTable,
                GetColumnNames(workbook, columnGroup), show);

            if (columnGroup == ColumnGroup.VertexOtherColumns)
            {
                // Hiding the subgraph image column doesn't hide the images in
                // the column.

                TableImagePopulator.ShowOrHideImagesInColumn(workbook,
                    WorksheetNames.Vertices,
                    VertexTableColumnNames.SubgraphImage, show);
            }

            if (activateWorksheet)
            {
                ExcelUtil.ActivateWorksheet(oTable);
            }

            ( new PerWorkbookSettings(workbook) ).SetColumnGroupVisibility(
                columnGroup, show);
        }
    }

    //*************************************************************************
    //  Method: ShowOrHideAllColumnGroups()
    //
    /// <summary>
    /// Shows or hides all column groups in the workbook.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the column groups.
    /// </param>
    ///
    /// <param name="show">
    /// true to show all column groups, false to hide them.
    /// </param>
    ///
    /// <remarks>
    /// Column groups that are for NodeXL's internal use are not modified.
    ///
    /// <para>
    /// Columns that don't exist are ignored.
    /// </para>
    ///
    /// <para>
    /// The column groups' new show/hide states are stored in the workbook
    /// using the <see cref="PerWorkbookSettings" /> class.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static void
    ShowOrHideAllColumnGroups
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        Boolean show
    )
    {
        Debug.Assert(workbook != null);

        foreach ( ColumnGroup eColumnGroup in
            Enum.GetValues(typeof(ColumnGroup) ) )
        {
            String sColumnGroup = eColumnGroup.ToString();

            if (
                sColumnGroup.IndexOf("DoNotHide") == -1
                &&
                sColumnGroup.IndexOf("InternalUse") == -1
                )
            {
                ShowOrHideColumnGroup(workbook, eColumnGroup, show, false);
            }
        }
    }

    //*************************************************************************
    //  Method: TryGetColumnGroupTable()
    //
    /// <summary>
    /// Attempts to get the table that contains a specified column group.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the column group.
    /// </param>
    ///
    /// <param name="eColumnGroup">
    /// The group of columns to get the table for.
    /// </param>
    ///
    /// <param name="oTable">
    /// Where the table gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryGetColumnGroupTable
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        ColumnGroup eColumnGroup,
        out ListObject oTable
    )
    {
        Debug.Assert(oWorkbook != null);

        oTable = null;
        String sColumnGroup = eColumnGroup.ToString();

        if ( sColumnGroup.StartsWith("Edge") )
        {
            return ( ExcelUtil.TryGetTable(oWorkbook, WorksheetNames.Edges,
                TableNames.Edges, out oTable) );
        }

        if ( sColumnGroup.StartsWith("Vertex") )
        {
            return ( ExcelUtil.TryGetTable(oWorkbook, WorksheetNames.Vertices,
                TableNames.Vertices, out oTable) );
        }

        Debug.Assert(false);
        return (false);
    }

    //*************************************************************************
    //  Method: GetColumnNames()
    //
    /// <summary>
    /// Gets the names of the columns in a specified column group.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the column group.
    /// </param>
    ///
    /// <param name="eColumnGroup">
    /// The group to get the column names for.
    /// </param>
    ///
    /// <returns>
    /// An array of zero or more column names.
    /// </returns>
    //*************************************************************************

    private static String []
    GetColumnNames
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        ColumnGroup eColumnGroup
    )
    {
        Debug.Assert(oWorkbook != null);

        String [] asColumnNames = null;

        switch (eColumnGroup)
        {
            case ColumnGroup.EdgeDoNotHide:

                asColumnNames = new String [] {
                    EdgeTableColumnNames.Vertex1Name,
                    EdgeTableColumnNames.Vertex2Name,
                    };

                break;

            case ColumnGroup.EdgeVisualAttributes:

                asColumnNames = new String [] {
                    EdgeTableColumnNames.Color,
                    EdgeTableColumnNames.Width,
                    EdgeTableColumnNames.Alpha,
                    EdgeTableColumnNames.Visibility,
                    };

                break;

            case ColumnGroup.EdgeLabels:

                asColumnNames = new String [] {
                    EdgeTableColumnNames.Label,
                    };

                break;

            case ColumnGroup.VertexDoNotHide:

                asColumnNames = new String [] {
                    VertexTableColumnNames.VertexName,
                    };

                break;

            case ColumnGroup.VertexVisualAttributes:

                asColumnNames = new String [] {
                    VertexTableColumnNames.Color,
                    VertexTableColumnNames.Shape,
                    VertexTableColumnNames.Radius,
                    VertexTableColumnNames.Alpha,
                    VertexTableColumnNames.ImageUri,
                    VertexTableColumnNames.Visibility,
                    };

                break;

            case ColumnGroup.VertexGraphMetrics:

                asColumnNames = new String [] {
                    VertexTableColumnNames.Degree,
                    VertexTableColumnNames.InDegree,
                    VertexTableColumnNames.OutDegree,
                    VertexTableColumnNames.BetweennessCentrality,
                    VertexTableColumnNames.ClosenessCentrality,
                    VertexTableColumnNames.EigenvectorCentrality,
                    VertexTableColumnNames.ClusteringCoefficient,
                    };

                break;

            case ColumnGroup.VertexLabels:

                asColumnNames = new String [] {
                    VertexTableColumnNames.Label,
                    VertexTableColumnNames.LabelFillColor,
                    VertexTableColumnNames.LabelPosition,
                    VertexTableColumnNames.ToolTip,
                    };

                break;

            case ColumnGroup.VertexLayout:

                asColumnNames = new String [] {
                    VertexTableColumnNames.LayoutOrder,
                    VertexTableColumnNames.X,
                    VertexTableColumnNames.Y,
                    VertexTableColumnNames.Locked,
                    VertexTableColumnNames.PolarR,
                    VertexTableColumnNames.PolarAngle,
                    };

                break;

            case ColumnGroup.EdgeInternalUse:
            case ColumnGroup.VertexInternalUse:

                asColumnNames = new String [] {
                    CommonTableColumnNames.ID,
                    CommonTableColumnNames.DynamicFilter,
                    };

                break;

            case ColumnGroup.EdgeOtherColumns:
            case ColumnGroup.VertexOtherColumns:

                asColumnNames = GetOtherColumnNames(oWorkbook, eColumnGroup);
                break;

            default:

                Debug.Assert(false);
                break;
        }
        
        Debug.Assert(asColumnNames != null);

        return (asColumnNames);
    }

    //*************************************************************************
    //  Method: GetOtherColumnNames()
    //
    /// <summary>
    /// Gets the names of the columns in the ColumnGroup.EdgeOtherColumns or
    /// VertexOtherColumns column group.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the column group.
    /// </param>
    ///
    /// <param name="eOtherColumnGroup">
    /// The group to get the column names for.
    /// </param>
    ///
    /// <returns>
    /// An array of zero or more column names.
    /// </returns>
    //*************************************************************************

    private static String []
    GetOtherColumnNames
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        ColumnGroup eOtherColumnGroup
    )
    {
        Debug.Assert(oWorkbook != null);

        List<String> oOtherColumnNames = new List<String>();
        ListObject oTable;

        if ( TryGetColumnGroupTable(oWorkbook, eOtherColumnGroup, out oTable) )
        {
            // Create a dictionary of the column names that are included in all
            // the non-other column groups in the table.  The key is the column
            // name and the value isn't used.

            Dictionary<String, Byte> oColumnNamesInNonOtherGroups =
                new Dictionary<String, Byte>();

            // All column groups in the edge table start with "Edge", and all
            // column groups in the vertex table start with "Vertex".

            String sStartsWith = null;

            switch (eOtherColumnGroup)
            {
                case ColumnGroup.EdgeOtherColumns:

                    sStartsWith = "Edge";
                    break;

                case ColumnGroup.VertexOtherColumns:

                    sStartsWith = "Vertex";
                    break;

                default:

                    Debug.Assert(false);
                    break;
            }

            foreach ( ColumnGroup eColumnGroup in
                Enum.GetValues(typeof(ColumnGroup) ) )
            {
                if (
                    eColumnGroup != eOtherColumnGroup
                    &&
                    eColumnGroup.ToString().StartsWith(sStartsWith)
                    )
                {
                    foreach ( String sColumnNameInNonOtherGroup in 
                        GetColumnNames(oWorkbook, eColumnGroup) )
                    {
                        oColumnNamesInNonOtherGroups[
                            sColumnNameInNonOtherGroup] = 0;
                    }
                }
            }

            // Any column not included in one of the non-other column groups is
            // an "other" column.

            foreach (ListColumn oColumn in oTable.ListColumns)
            {
                String sColumnName = oColumn.Name;

                if ( !oColumnNamesInNonOtherGroups.ContainsKey(sColumnName) )
                {
                    oOtherColumnNames.Add(sColumnName);
                }
            }
        }
        
        return ( oOtherColumnNames.ToArray() );
    }
}
}
