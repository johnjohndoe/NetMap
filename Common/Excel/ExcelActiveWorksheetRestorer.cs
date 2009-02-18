

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: ExcelActiveWorksheetRestorer
//
/// <summary>
/// Saves and restores the active worksheet of an Excel workbook.
/// </summary>
///
/// <remarks>
/// Writing to a worksheet that isn't active causes problems with the selection
/// in Excel.  To avoid such problems, use this class to activate the worksheet 
/// that needs to be written to and then restore the original active worksheet
/// when you're done writing.  The activations are done with Excel's screen
/// updating turned off, so they are not visible to the user.
///
/// <para>
/// Call <see cref="ActivateWorksheet" /> to activate the worksheet that needs
/// to be written to.  Call <see cref="Restore" /> when you're
/// done writing.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class ExcelActiveWorksheetRestorer : Object
{
    //*************************************************************************
    //  Constructor: ExcelActiveWorksheetRestorer()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ExcelActiveWorksheetRestorer" /> class.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the worksheet that will be activated.
    /// </param>
    //*************************************************************************

    public ExcelActiveWorksheetRestorer
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        m_oWorkbook = workbook;

        AssertValid();
    }

    //*************************************************************************
    //  Method: ActivateWorksheet()
    //
    /// <summary>
    /// Activates a worksheet.
    /// </summary>
    ///
    /// <param name="worksheet">
    /// The worksheet to activate.
    /// </param>
    ///
    /// <returns>
    /// An <see cref="ExcelActiveWorksheetState" /> object to pass to <see
    /// cref="Restore" />.
    /// </returns>
    ///
    /// <remarks>
    /// Excel's screen updating is turned off.
    /// </remarks>
    //*************************************************************************

    public ExcelActiveWorksheetState
    ActivateWorksheet
    (
        Worksheet worksheet
    )
    {
        Debug.Assert(worksheet != null);
        AssertValid();

        Worksheet oActiveWorksheet = null;

        if (m_oWorkbook.ActiveSheet is Worksheet)
        {
            oActiveWorksheet = (Worksheet)m_oWorkbook.ActiveSheet;
        }

        Boolean bScreenUpdating = m_oWorkbook.Application.ScreenUpdating;

        m_oWorkbook.Application.ScreenUpdating = false;

        ExcelUtil.ActivateWorksheet(worksheet);

        return ( new ExcelActiveWorksheetState(oActiveWorksheet, 
            bScreenUpdating) );
    }

    //*************************************************************************
    //  Method: Restore()
    //
    /// <summary>
    /// Activates the worksheet that was active before <see
    /// cref="ActivateWorksheet" /> was called.
    /// </summary>
    ///
    /// <param name="excelActiveWorksheetState">
    /// The object that was returned by <see cref="ActivateWorksheet" />.
    /// </param>
    ///
    /// <remarks>
    /// Excel's screen updating is turned back on if it was on before <see
    /// cref="ActivateWorksheet" /> was called.
    /// </remarks>
    //*************************************************************************

    public void
    Restore
    (
        ExcelActiveWorksheetState excelActiveWorksheetState
    )
    {
        Debug.Assert(excelActiveWorksheetState != null);
        AssertValid();

        Worksheet oWorksheetToActivate =
            excelActiveWorksheetState.ActiveWorksheet;

        if (oWorksheetToActivate != null)
        {
            ExcelUtil.ActivateWorksheet(oWorksheetToActivate);
        }

        m_oWorkbook.Application.ScreenUpdating =
            excelActiveWorksheetState.ScreenUpdating;
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
        Debug.Assert(m_oWorkbook != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Workbook containing the worksheet that will be activated.

    protected Workbook m_oWorkbook;
}
}
