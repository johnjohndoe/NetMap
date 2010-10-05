
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.Office.Interop.Excel;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: NodeXLWorkbookUpdater
//
/// <summary>
/// Updates the worksheet, table, and column names in an older NodeXL workbook.
/// </summary>
///
/// <remarks>
/// Use <see cref="UpdateNames" /> to update a workbook.
///
/// <para>
/// All methods are static.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class NodeXLWorkbookUpdater : Object
{
    //*************************************************************************
    //  Method: UpdateNames()
    //
    /// <summary>
    /// Updates the worksheet, table, and column names in an older NodeXL
    /// workbook if neccessary.
    /// </summary>
    ///
    /// <param name="workbook">
    /// The NodeXL workbook to update.
    /// </param>
    ///
    /// <remarks>
    /// Some column, table, and worksheet names have changed in later versions
    /// of NodeXL.  This method looks for the older names and updates them to
    /// the latest names if it finds them.
    /// </remarks>
    //*************************************************************************

    public static void
    UpdateNames
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        Debug.Assert(workbook != null);

        Worksheet oWorksheet;
        ListObject oTable;
        ListColumn oColumn;

        if ( ExcelUtil.TryGetTable(workbook, "Vertices", "Vertices",
            out oTable)
            &&
            ExcelUtil.TryGetTableColumn(oTable, "Radius", out oColumn)
        )
        {
            oColumn.Name = "Size";
        }

        if ( ExcelUtil.TryGetWorksheet(workbook, "Clusters", out oWorksheet) )
        {
            oWorksheet.Name = "Groups";

            if ( ExcelUtil.TryGetTable(oWorksheet, "Clusters", out oTable) )
            {
                oTable.Name = "Groups";

                if ( ExcelUtil.TryGetTableColumn(oTable, "Cluster",
                    out oColumn) )
                {
                    oColumn.Name = "Group";
                }
            }
        }

        if ( ExcelUtil.TryGetWorksheet(workbook, "Cluster Vertices",
            out oWorksheet) )
        {
            oWorksheet.Name = "Group Vertices";

            if ( ExcelUtil.TryGetTable(oWorksheet, "ClusterVertices",
                out oTable) )
            {
                oTable.Name = "GroupVertices";

                if ( ExcelUtil.TryGetTableColumn(oTable, "Cluster",
                    out oColumn) )
                {
                    oColumn.Name = "Group";
                }
            }
        }
    }


    //*************************************************************************
    //  Private fields
    //*************************************************************************

    // (None.)
}

}
