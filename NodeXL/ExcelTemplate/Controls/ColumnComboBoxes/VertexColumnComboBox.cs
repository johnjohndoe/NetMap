
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: VertexColumnComboBox
//
/// <summary>
/// Represents a ComboBox that lets the user select a column from the vertex
/// table.
/// </summary>
///
/// <remarks>
/// Call <see cref="ColumnComboBox.PopulateWithTableColumnNames" /> to populate
/// the ComboBox with the vertex table column names.
/// </remarks>
//*****************************************************************************

public class VertexColumnComboBox : ColumnComboBox
{
    //*************************************************************************
    //  Constructor: VertexColumnComboBox()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexColumnComboBox" />
    /// class.
    /// </summary>
    //*************************************************************************

    public VertexColumnComboBox()
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
            TableColumnNamesToExclude, TableColumnNameBasesToExclude) );
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
            VertexTableColumnNames.VertexName,
            VertexTableColumnNames.Color,
            VertexTableColumnNames.Shape,
            VertexTableColumnNames.Radius,
            VertexTableColumnNames.ImageUri,
            VertexTableColumnNames.Label,
            VertexTableColumnNames.LabelFillColor,
            VertexTableColumnNames.LabelPosition,
            CommonTableColumnNames.Alpha,
            VertexTableColumnNames.ToolTip,
            CommonTableColumnNames.Visibility,
            VertexTableColumnNames.LayoutOrder,
            VertexTableColumnNames.Locked,
            VertexTableColumnNames.X,
            VertexTableColumnNames.Y,
            VertexTableColumnNames.PolarR,
            VertexTableColumnNames.PolarAngle,
            VertexTableColumnNames.SubgraphImage,
            CommonTableColumnNames.ID,
            CommonTableColumnNames.DynamicFilter,
            CommonTableColumnNames.AddColumnsHere,
            };

    /// Table column name bases to exclude from the ComboBox.  If the array
    /// includes "Custom", for example, then columns named "Custom 1" and
    /// "Custom 2" will be excluded.

    protected static readonly String [] TableColumnNameBasesToExclude =
        new String [] {
            VertexTableColumnNames.CustomMenuItemTextBase,
            VertexTableColumnNames.CustomMenuItemActionBase,
            };
}

}
