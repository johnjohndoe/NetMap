
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: EdgeColumnComboBox
//
/// <summary>
/// Represents a ComboBox that lets the user select a column from the edge
/// table.
/// </summary>
///
/// <remarks>
/// Call <see cref="ColumnComboBox.PopulateWithTableColumnNames" /> to populate
/// the ComboBox with the edge table column names.
/// </remarks>
//*****************************************************************************

public class EdgeColumnComboBox : ColumnComboBox
{
    //*************************************************************************
    //  Constructor: EdgeColumnComboBox()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EdgeColumnComboBox" />
    /// class.
    /// </summary>
    //*************************************************************************

    public EdgeColumnComboBox()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: GetTableColumnNames()
    //
    /// <summary>
    /// Gets the names of the table's columns.
    /// </summary>
    ///
    /// <param name="oTable">
    /// The table containing the columns.
    /// </param>
    ///
    /// <returns>
    /// The table column names.
    /// </returns>
    //*************************************************************************

    protected override String []
    GetTableColumnNames
    (
        Microsoft.Office.Interop.Excel.ListObject oTable
    )
    {
        AssertValid();

        return ( ExcelUtil.GetTableColumnNames(oTable,
            TableColumnNamesToExclude, TableColumnNameBasesToExclude)
            );
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

    /// Table column names to exclude from the ComboBox.

    protected static readonly String [] TableColumnNamesToExclude =
        new String [] {
            EdgeTableColumnNames.Vertex1Name,
            EdgeTableColumnNames.Vertex2Name,
            EdgeTableColumnNames.Color,
            EdgeTableColumnNames.Width,
            EdgeTableColumnNames.Style,
            CommonTableColumnNames.Alpha,
            CommonTableColumnNames.Visibility,
            EdgeTableColumnNames.Label,
            CommonTableColumnNames.ID,
            CommonTableColumnNames.DynamicFilter,
            CommonTableColumnNames.AddColumnsHere,
            };

    /// Table column name bases to exclude from the ComboBox.  If the array
    /// includes "Custom", for example, then columns named "Custom 1" and
    /// "Custom 2" will be excluded.

    protected static readonly String [] TableColumnNameBasesToExclude =
        new String[0];
}

}
