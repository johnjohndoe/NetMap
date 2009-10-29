
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AutoFillColumnComboBox
//
/// <summary>
/// Base class for ComboBoxes that contain a list of table columns that can be
/// used as a source column by NodeXL's autofill feature.
/// </summary>
//*****************************************************************************

public abstract class AutoFillColumnComboBox : ComboBoxPlus
{
    //*************************************************************************
    //  Constructor: AutoFillColumnComboBox()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoFillColumnComboBox" />
    /// class.
    /// </summary>
    //*************************************************************************

    public AutoFillColumnComboBox()
    {
        // (Do nothing.)

        // AssertValid();
    }

    //*************************************************************************
    //  Method: PopulateWithSourceColumnNames()
    //
    /// <summary>
    /// Populates the ComboBox with the names of the table columns that can be
    /// used as a source column by NodeXL's autofill feature.
    /// </summary>
    ///
    /// <param name="table">
    /// The table containing the columns.
    /// </param>
    //*************************************************************************

    public void
    PopulateWithSourceColumnNames
    (
        Microsoft.Office.Interop.Excel.ListObject table
    )
    {
        Debug.Assert(table != null);
        AssertValid();

        ComboBox.ObjectCollection oItems = this.Items;
        oItems.Clear();
        oItems.AddRange( GetTableColumnNames(table) );
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

    protected abstract String []
    GetTableColumnNames
    (
        Microsoft.Office.Interop.Excel.ListObject oTable
    );


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public virtual void
    AssertValid()
    {
        // (Do nothing.)
    }
}

}
