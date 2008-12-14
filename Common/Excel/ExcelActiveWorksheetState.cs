

//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//	Class: ExcelActiveWorksheetState
//
/// <summary>
/// Retains the state of Excel's active worksheet.
/// </summary>
///
/// <remarks>
/// An <see cref="ExcelActiveWorksheetState" /> object is returned by <see
/// cref="ExcelActiveWorksheetRestorer.ActivateWorksheet" /> and passed to <see
/// cref="ExcelActiveWorksheetRestorer.Restore" />.
/// </remarks>
//*****************************************************************************

public class ExcelActiveWorksheetState : Object
{
	//*************************************************************************
	//	Constructor: ExcelActiveWorksheetState()
	//
	/// <summary>
	///	Initializes a new instance of the <see
	/// cref="ExcelActiveWorksheetState" /> class.
	/// </summary>
	///
	/// <param name="activeWorksheet">
	/// Worksheet that was active before <see
	/// cref="ExcelActiveWorksheetRestorer.ActivateWorksheet" /> was called, or
	/// null if there was no active worksheet.
	/// </param>
	///
	/// <param name="screenUpdating">
	/// true if Excel's screen updating was turned on before <see
	/// cref="ExcelActiveWorksheetRestorer.ActivateWorksheet" /> was called.
	/// </param>
	//*************************************************************************

	public ExcelActiveWorksheetState
	(
		Microsoft.Office.Interop.Excel.Worksheet activeWorksheet,
		Boolean screenUpdating
	)
	{
		m_oActiveWorksheet = activeWorksheet;
		m_bScreenUpdating = screenUpdating;

		AssertValid();
	}

    //*************************************************************************
    //  Property: ActiveWorksheet
    //
    /// <summary>
	/// Gets the worksheet that was active before <see
	/// cref="ExcelActiveWorksheetRestorer" /> was called.
    /// </summary>
    ///
    /// <value>
	/// The worksheet that was active before <see
	/// cref="ExcelActiveWorksheetRestorer" /> was called, or null if there was
	/// no active worksheet.
    /// </value>
    //*************************************************************************

    public Microsoft.Office.Interop.Excel.Worksheet
	ActiveWorksheet
    {
        get
        {
            AssertValid();

			return (m_oActiveWorksheet);
        }
    }

    //*************************************************************************
    //  Property: ScreenUpdating
    //
    /// <summary>
	/// Gets a flag indicating whether Excel's screen updating was turned on
	/// before <see cref="ExcelActiveWorksheetRestorer.ActivateWorksheet" />
	/// was called.
    /// </summary>
    ///
    /// <value>
	/// true if Excel's screen updating was turned on before <see
	/// cref="ExcelActiveWorksheetRestorer.ActivateWorksheet" /> was called.
    /// </value>
    //*************************************************************************

    public Boolean
	ScreenUpdating
    {
        get
        {
            AssertValid();

			return (m_bScreenUpdating);
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
		// m_oActiveWorksheet
		// m_bScreenUpdating
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Worksheet that was active before
	/// ExcelActiveWorksheetRestorer.ActivateWorksheet() was called, or null.

	protected Worksheet m_oActiveWorksheet;

	/// true if Excel's screen updating was turned on before 
	/// ExcelActiveWorksheetRestorer.ActivateWorksheet() was called.

	protected Boolean m_bScreenUpdating;
}
}
