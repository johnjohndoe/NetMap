
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: RunEdgeCommandEventArgs
//
/// <summary>
/// Provides event information for the <see
/// cref="WorksheetContextMenuManager.RunEdgeCommand" /> event.
/// </summary>
//*****************************************************************************

public class RunEdgeCommandEventArgs : EventArgs
{
    //*************************************************************************
    //  Constructor: RunEdgeCommandEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="RunEdgeCommandEventArgs" /> class.
    /// </summary>
	///
	/// <param name="edgeID">
    /// ID of the edge that was right-clicked in the edge table in the edge
	/// worksheet, or <see cref="WorksheetContextMenuManager.NoID" /> if
	/// an edge wasn't right-clicked.  This is an ID that is stored in the
	/// worksheet, NOT an IEdge.ID value.
	/// </param>
	///
	/// <param name="edgeCommand">
	/// Command to run.
	/// </param>
    //*************************************************************************

    public RunEdgeCommandEventArgs
	(
		Int32 edgeID,
		WorksheetContextMenuManager.EdgeCommand edgeCommand
	)
    {
        m_iEdgeID = edgeID;
		m_eEdgeCommand = edgeCommand;

		AssertValid();
    }

    //*************************************************************************
    //  Property: EdgeID
    //
    /// <summary>
    /// Gets the ID of the edge that was right-clicked in the edge table in the
	/// edge worksheet.
    /// </summary>
    ///
    /// <value>
    /// The ID of the right-clicked edge, or <see
	/// cref="WorksheetContextMenuManager.NoID" /> if an edge wasn't right-
	/// clicked.  This is an ID that is stored in the worksheet, NOT an
	/// IEdge.ID value.
    /// </value>
    //*************************************************************************

    public Int32
    EdgeID
    {
        get
        {
            AssertValid();

            return (m_iEdgeID);
        }
    }

    //*************************************************************************
    //  Property: EdgeCommand
    //
    /// <summary>
    /// Gets the command to run.
    /// </summary>
    ///
    /// <value>
    /// The command to run.
    /// </value>
    //*************************************************************************

    public WorksheetContextMenuManager.EdgeCommand
    EdgeCommand
    {
        get
        {
            AssertValid();

            return (m_eEdgeCommand);
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
		Debug.Assert(m_iEdgeID == WorksheetContextMenuManager.NoID ||
			m_iEdgeID > 0);

		// m_eEdgeCommand
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// ID of the edge that was right-clicked in the edge table in the edge
	/// worksheet, or WorksheetContextMenuManager.NoID if an edge wasn't right-
	/// clicked.

	protected Int32 m_iEdgeID;

	/// The command to run.

    protected WorksheetContextMenuManager.EdgeCommand m_eEdgeCommand;
}


//*****************************************************************************
//  Delegate: RunEdgeCommandEventHandler
//
/// <summary>
/// Represents a method that will handle the <see
/// cref="WorksheetContextMenuManager.RunEdgeCommand" /> event.
/// </summary>
///
/// <param name="sender">
/// The source of the event.
/// </param>
///
/// <param name="e">
///	A <see cref="RunEdgeCommandEventArgs" /> object that contains the event
/// data.
/// </param>
//*****************************************************************************

public delegate void
RunEdgeCommandEventHandler
(
	Object sender,
	RunEdgeCommandEventArgs e
);

}
