

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: ExcelColumnHider
//
/// <summary>
/// Hides and shows columns in an Excel workbook.
/// </summary>
///
/// <remarks>
/// Call <see cref="ShowHiddenColumns" /> to show all the hidden columns in a
/// table, then call <see cref="RestoreHiddenColumns" /> to hide them again.
/// Use <see cref="ShowColumns" />, <see cref="HideColumns" />, and <see
/// cref="ShowOrHideColumns" /> to show or hide a specified set of table
/// columns.
///
/// <para>
/// All methods are static.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class ExcelColumnHider
{
    //*************************************************************************
    //  Method: ShowHiddenColumns()
    //
    /// <summary>
    /// Shows all hidden columns in an Excel table.
    /// </summary>
    ///
    /// <param name="table">
    /// The table whose hidden columns should be shown.
    /// </param>
    ///
    /// <returns>
    /// An <see cref="ExcelHiddenColumns" /> object to pass to <see
    /// cref="RestoreHiddenColumns" />.
    /// </returns>
    ///
    /// <remarks>
    /// Call <see cref="RestoreHiddenColumns" /> to hide the columns again.
    /// </remarks>
    //*************************************************************************

    public static ExcelHiddenColumns
    ShowHiddenColumns
    (
        ListObject table
    )
    {
        Debug.Assert(table != null);

        ExcelHiddenColumns oExcelHiddenColumns = new ExcelHiddenColumns();

        foreach (ListColumn oColumn in table.ListColumns)
        {
            if ( (Boolean)oColumn.Range.EntireColumn.Hidden )
            {
                oExcelHiddenColumns.AddLast(oColumn.Name);
                ShowOrHideColumn(oColumn, true);
            }
        }

        return (oExcelHiddenColumns);
    }

    //*************************************************************************
    //  Method: RestoreHiddenColumns()
    //
    /// <summary>
    /// Hides the table columns that were shown by <see
    /// cref="ShowHiddenColumns" />.
    /// </summary>
    ///
    /// <param name="table">
    /// The table whose columns should be hidden again.
    /// </param>
    ///
    /// <param name="excelHiddenColumns">
    /// The object that was returned by <see cref="ShowHiddenColumns" />.
    /// </param>
    ///
    /// <remarks>
    /// If a column that was shown by <see cref="ShowHiddenColumns" /> no
    /// longer exists, this method ignores the column.
    /// </remarks>
    //*************************************************************************

    public static void
    RestoreHiddenColumns
    (
        ListObject table,
        ExcelHiddenColumns excelHiddenColumns
    )
    {
        Debug.Assert(table != null);
        Debug.Assert(excelHiddenColumns != null);

        foreach (String sColumnName in excelHiddenColumns)
        {
            ListColumn oColumn;

            if ( ExcelUtil.TryGetTableColumn(table, sColumnName, out oColumn) )
            {
                ShowOrHideColumn(oColumn, false);
            }
        }
    }

    //*************************************************************************
    //  Method: ShowColumns()
    //
    /// <summary>
    /// Shows a specified set of table columns.
    /// </summary>
    ///
    /// <param name="table">
    /// The table whose columns should be shown.
    /// </param>
    ///
    /// <param name="columnNames">
    /// Array of names of the columns to show.
    /// </param>
    ///
    /// <remarks>
    /// If a column specified in <paramref name="columnNames" /> doesn't exist,
    /// this method ignores the column.
    /// </remarks>
    //*************************************************************************

    public static void
    ShowColumns
    (
        ListObject table,
        String [] columnNames
    )
    {
        Debug.Assert(table != null);
        Debug.Assert(columnNames != null);

        ShowOrHideColumns(table, columnNames, true);
    }

    //*************************************************************************
    //  Method: HideColumns()
    //
    /// <summary>
    /// Hides a specified set of table columns.
    /// </summary>
    ///
    /// <param name="table">
    /// The table whose columns should be hidden.
    /// </param>
    ///
    /// <param name="columnNames">
    /// Array of names of the columns to hide.
    /// </param>
    ///
    /// <remarks>
    /// If a column specified in <paramref name="columnNames" /> doesn't exist,
    /// this method ignores the column.
    /// </remarks>
    //*************************************************************************

    public static void
    HideColumns
    (
        ListObject table,
        String [] columnNames
    )
    {
        Debug.Assert(table != null);
        Debug.Assert(columnNames != null);

        ShowOrHideColumns(table, columnNames, false);
    }

    //*************************************************************************
    //  Method: ShowOrHideColumns()
    //
    /// <summary>
    /// Shows or hides a specified set of table columns.
    /// </summary>
    ///
    /// <param name="table">
    /// The table whose columns should be shown or hidden.
    /// </param>
    ///
    /// <param name="columnNames">
    /// Array of names of the columns to show or hide.
    /// </param>
    ///
    /// <param name="show">
    /// true to show the columns, false to hide them.
    /// </param>
    ///
    /// <remarks>
    /// If a column specified in <paramref name="columnNames" /> doesn't exist,
    /// this method ignores the column.
    /// </remarks>
    //*************************************************************************

    public static void
    ShowOrHideColumns
    (
        ListObject table,
        String [] columnNames,
        Boolean show
    )
    {
        Debug.Assert(table != null);
        Debug.Assert(columnNames != null);

        foreach (String sColumnName in columnNames)
        {
            ShowOrHideColumn(table, sColumnName, show);
        }
    }

    //*************************************************************************
    //  Method: ShowOrHideColumn()
    //
    /// <summary>
    /// Shows or hides a specified table column.
    /// </summary>
    ///
    /// <param name="table">
    /// The table whose column should be shown or hidden.
    /// </param>
    ///
    /// <param name="columnName">
    /// Name of the column to show or hide.
    /// </param>
    ///
    /// <param name="show">
    /// true to show the column, false to hide it.
    /// </param>
    ///
    /// <returns>
    /// true if the column was visible before this method was called.
    /// </returns>
    ///
    /// <remarks>
    /// If the column doesn't exist, this method does nothing.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    ShowOrHideColumn
    (
        ListObject table,
        String columnName,
        Boolean show
    )
    {
        Debug.Assert(table != null);
        Debug.Assert( !String.IsNullOrEmpty(columnName) );

        Boolean bColumnWasVisible = false;
        ListColumn oColumn;

        if ( ExcelUtil.TryGetTableColumn(table, columnName, out oColumn) )
        {
            bColumnWasVisible = ShowOrHideColumn(oColumn, show);
        }

        return (bColumnWasVisible);
    }

    //*************************************************************************
    //  Method: ShowOrHideColumn()
    //
    /// <summary>
    /// Shows or hides a table column.
    /// </summary>
    ///
    /// <param name="column">
    /// The column that should be shown or hidden.
    /// </param>
    ///
    /// <param name="show">
    /// true to show the column, false to hide it.
    /// </param>
    ///
    /// <returns>
    /// true if the column was visible before this method was called.
    /// </returns>
    //*************************************************************************

    public static Boolean
    ShowOrHideColumn
    (
        ListColumn column,
        Boolean show
    )
    {
        Debug.Assert(column != null);

        Range oEntireColumn = column.Range.EntireColumn;
        Boolean bColumnWasVisible = !(Boolean)oEntireColumn.Hidden;
        oEntireColumn.Hidden = !show;

        return (bColumnWasVisible);
    }
}
}
