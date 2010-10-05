

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: ExcelApplicationKiller
//
/// <summary>
/// Kills an Excel application's process on demand.
/// </summary>
///
/// <remarks>
/// The <see cref="KillExcelApplication" /> method kills the process running
/// an Excel application.  It can be used to guarantee that an Excel
/// application started from a parent process is removed from memory when the
/// parent process is done with the Excel application.
///
/// <para>
/// There are various situations in which calling Application.Quit() will not
/// remove the Excel application from memory; see this posting for more
/// information:
/// </para>
///
/// <para>
/// http://www.dotnet247.com/247reference/msgs/68/344322.aspx
/// </para>
///
/// <para>
/// Suggested solutions involving calls to Marshall.ReleaseComObject(), never
/// referencing an Excel object more than one level deep without storing it in
/// an intermediate variable, and so on, are either unreliable or fragile and
/// grossly impractical.  This class offers a brute-force workaround for all
/// those problems.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class ExcelApplicationKiller : Object
{
    //*************************************************************************
    //  Constructor: ExcelApplicationKiller()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ExcelApplicationKiller" />
    /// class.
    /// </summary>
    ///
    /// <param name="visibleExcelApplication">
    /// The Excel application that will be killed by <see
    /// cref="KillExcelApplication" />.  The application must be visible.
    /// </param>
    //*************************************************************************

    public ExcelApplicationKiller
    (
        Microsoft.Office.Interop.Excel.Application visibleExcelApplication
    )
    {
        Debug.Assert(visibleExcelApplication != null);

        if (!visibleExcelApplication.Visible)
        {
            // When the application isn't visible, its Hwnd is zero.

            throw new ArgumentException(
                "The Excel application must be visible.",
                "visibleExcelApplication"
                );
        }

        Int32 iHwnd = visibleExcelApplication.Hwnd;
        Debug.Assert(iHwnd != 0);

        // Find the Excel process with the same Hwnd.

        Boolean bProcessFound = false;

        foreach ( Process oProcess in Process.GetProcessesByName("Excel") )
        {
            if (oProcess.MainWindowHandle.ToInt32() == iHwnd)
            {
                m_iProcessID = oProcess.Id;
                bProcessFound = true;
                break;
            }
        }

        if (!bProcessFound)
        {
            throw new InvalidOperationException(
                "The Excel application's process couldn't be found."
                );
        }

        AssertValid();
    }

    //*************************************************************************
    //  Method: KillExcelApplication()
    //
    /// <summary>
    /// Kills the process running the Excel application passed to the
    /// constructor.
    /// </summary>
    //*************************************************************************

    public void
    KillExcelApplication()
    {
        AssertValid();

        foreach ( Process oProcess in Process.GetProcessesByName("Excel") )
        {
            if (oProcess.Id == m_iProcessID)
            {
                oProcess.Kill();
                break;
            }
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
        // m_iProcessID
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Process ID of the Excel application that will be killed.

    protected Int32 m_iProcessID;
}
}
