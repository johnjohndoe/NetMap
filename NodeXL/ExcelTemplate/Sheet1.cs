

//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: Sheet1
//
/// <summary>
/// Represents the edge worksheet.
/// </summary>
//*****************************************************************************

public partial class Sheet1
{
	//*************************************************************************
	//	Event: EdgeSelectionChanged
	//
	/// <summary>
	///	Occurs when the selection state of the edge table changes.
	/// </summary>
	//*************************************************************************

	public event TableSelectionChangedEventHandler EdgeSelectionChanged;


    //*************************************************************************
    //  Method: Sheet1_Startup()
    //
    /// <summary>
	/// Handles the Startup event on the worksheet.
    /// </summary>
    ///
	/// <param name="sender">
	/// Standard event argument.
	/// </param>
    ///
	/// <param name="e">
	/// Standard event argument.
	/// </param>
    //*************************************************************************

	private void
	Sheet1_Startup
	(
		object sender,
		System.EventArgs e
	)
	{
		// Create the object that does most of the work for this class.

        m_oSheets1And2Helper = new Sheets1And2Helper(this, this.Edges);

        m_oSheets1And2Helper.TableSelectionChanged +=
			new TableSelectionChangedEventHandler(
				m_oSheets1And2Helper_TableSelectionChanged);

        m_oSheets1And2Helper.Sheet_Startup(sender, e);

        AssertValid();
	}

    //*************************************************************************
    //  Method: m_oSheets1And2Helper_TableSelectionChanged()
    //
    /// <summary>
	/// Handles the TableSelectionChanged event on the m_oSheets1And2Helper
	/// object.
    /// </summary>
    ///
	/// <param name="sender">
	/// Standard event argument.
	/// </param>
    ///
	/// <param name="e">
	/// Standard event argument.
	/// </param>
    //*************************************************************************

	private void
	m_oSheets1And2Helper_TableSelectionChanged
	(
		object sender,
		TableSelectionChangedEventArgs e
	)
	{
		AssertValid();

		FireEdgeSelectionChanged(e);
	}

    //*************************************************************************
    //  Method: Sheet1_Shutdown()
    //
    /// <summary>
	/// Handles the Shutdown event on the worksheet.
    /// </summary>
    ///
	/// <param name="sender">
	/// Standard event argument.
	/// </param>
    ///
	/// <param name="e">
	/// Standard event argument.
	/// </param>
    //*************************************************************************

	private void
	Sheet1_Shutdown
	(
		object sender,
		System.EventArgs e
	)
	{
		AssertValid();

        m_oSheets1And2Helper.Sheet_Shutdown(sender, e);
	}

    //*************************************************************************
    //  Method: FireEdgeSelectionChanged()
    //
    /// <summary>
	/// Fires the <see cref="EdgeSelectionChanged" /> event if appropriate.
    /// </summary>
    ///
	/// <param name="e">
	/// Standard event argument.
	/// </param>
    //*************************************************************************

    private void
	FireEdgeSelectionChanged
	(
		TableSelectionChangedEventArgs e
	)
    {
		Debug.Assert(e != null);
		AssertValid();

		TableSelectionChangedEventHandler oEdgeSelectionChanged =
			this.EdgeSelectionChanged;

		if (oEdgeSelectionChanged != null)
		{
			try
			{
				oEdgeSelectionChanged(this, e);
			}
			catch (Exception oException)
			{
				// If exceptions aren't caught here, Excel consumes them
				// without indicating that anything is wrong.

				ErrorUtil.OnException(oException);
			}
		}
    }


	#region VSTO Designer generated code

	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InternalStartup()
	{
		this.Startup += new System.EventHandler(Sheet1_Startup);
		this.Shutdown += new System.EventHandler(Sheet1_Shutdown);
	}
        
	#endregion


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
		Debug.Assert(m_oSheets1And2Helper != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Object that does most of the work for this class.

	private Sheets1And2Helper m_oSheets1And2Helper;
}

}
