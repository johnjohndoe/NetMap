
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: ExcelWorkbookListBox
//
/// <summary>
/// Represents a ListBox that contains a list of Excel workbook names.
/// </summary>
///
/// <remarks>
/// Call <see cref="PopulateWithOtherWorkbookNames" /> to populate the ListBox
/// with the names of all open Excel workbooks except a specified one.
/// </remarks>
//*****************************************************************************

public class ExcelWorkbookListBox : ListBoxPlus
{
    //*************************************************************************
    //  Constructor: ExcelWorkbookListBox()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ExcelWorkbookListBox" />
    /// class.
    /// </summary>
    //*************************************************************************

    public ExcelWorkbookListBox()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: PopulateWithOtherWorkbookNames()
    //
    /// <summary>
    /// Populates the ListBox with the names of all open Excel workbooks except
    /// a specified one.
    /// </summary>
    ///
    /// <param name="workbook">
    /// The workbook whose name should be excluded from the ListBox.
    /// </param>
    //*************************************************************************

    public void
    PopulateWithOtherWorkbookNames
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        Debug.Assert(workbook != null);
        AssertValid();

        ListBox.ObjectCollection oItems = this.Items;

        oItems.Clear();

        foreach (Microsoft.Office.Interop.Excel.Workbook oOtherWorkbook in
            workbook.Application.Workbooks)
        {
            String sOtherWorkbookName = oOtherWorkbook.Name;

            // Filter out this workbook and the personal macro workbook.  The
            // Workbook.Name of the personal macro workbook is "Personal1" on
            // my machine -- can it be "PERSONAL2" or "PERSONAL3" as well?

            if ( sOtherWorkbookName == workbook.Name ||
                sOtherWorkbookName.ToLower().StartsWith("personal") )
            {
                continue;
            }

            oItems.Add(sOtherWorkbookName);
        }
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        // (Do nothing.)
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// <summary>
    /// Message that can be displayed by the application when there are no
    /// other open workbooks.
    /// </summary>

    public const String NoOtherWorkbooks =
        "There are no other open workbooks."
        ;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
