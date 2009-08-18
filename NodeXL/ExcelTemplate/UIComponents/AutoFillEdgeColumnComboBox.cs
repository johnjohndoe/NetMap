
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AutoFillEdgeColumnComboBox
//
/// <summary>
/// Represents a ComboBox that contains a list of edge worksheet columns that
/// can be used as a source column by NodeXL's autofill feature.
/// </summary>
///
/// <remarks>
/// Call <see cref="AutoFillColumnComboBox.PopulateWithSourceColumnNames" /> to
/// populate the ComboBox with the names of the edge worksheet columns that can
/// be used as a source column.
/// </remarks>
//*****************************************************************************

public class AutoFillEdgeColumnComboBox : AutoFillColumnComboBox
{
    //*************************************************************************
    //  Constructor: AutoFillEdgeColumnComboBox()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AutoFillEdgeColumnComboBox" /> class.
    /// </summary>
    //*************************************************************************

    public AutoFillEdgeColumnComboBox()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: GetTableColumnNames()
    //
    /// <summary>
    /// Gets the names of the table columns that can be used as a source column
    /// by NodeXL's autofill feature.
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
            EdgeTableColumnNames.Alpha,
            EdgeTableColumnNames.Visibility,
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
