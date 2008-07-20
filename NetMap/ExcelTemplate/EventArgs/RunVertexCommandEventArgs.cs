
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: RunVertexCommandEventArgs
//
/// <summary>
/// Provides event information for the <see
/// cref="WorksheetContextMenuManager.RunVertexCommand" /> event.
/// </summary>
//*****************************************************************************

public class RunVertexCommandEventArgs : EventArgs
{
    //*************************************************************************
    //  Constructor: RunVertexCommandEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="RunVertexCommandEventArgs" /> class.
    /// </summary>
	///
	/// <param name="vertexID">
    /// ID of the vertex that was right-clicked in the vertex table in the
	/// vertex worksheet, or <see cref="WorksheetContextMenuManager.NoID" /> if
	/// a vertex wasn't right-clicked.  This is an ID that is stored in the
	/// worksheet, NOT an IEdge.ID value.
	/// </param>
	///
	/// <param name="vertexCommand">
	/// Command to run.
	/// </param>
    //*************************************************************************

    public RunVertexCommandEventArgs
	(
		Int32 vertexID,
		WorksheetContextMenuManager.VertexCommand vertexCommand
	)
    {
        m_iVertexID = vertexID;
		m_eVertexCommand = vertexCommand;

		AssertValid();
    }

    //*************************************************************************
    //  Property: VertexID
    //
    /// <summary>
    /// Gets the ID of the vertex that was right-clicked in the vertex table in
	/// the vertex worksheet.
    /// </summary>
    ///
    /// <value>
    /// The ID of the right-clicked vertex, or <see
	/// cref="WorksheetContextMenuManager.NoID" /> if a vertex wasn't right-
	/// clicked.  This is an ID that is stored in the worksheet, NOT an
	/// IEdge.ID value.
    /// </value>
    //*************************************************************************

    public Int32
    VertexID
    {
        get
        {
            AssertValid();

            return (m_iVertexID);
        }
    }

    //*************************************************************************
    //  Property: VertexCommand
    //
    /// <summary>
    /// Gets the command to run.
    /// </summary>
    ///
    /// <value>
    /// The command to run.
    /// </value>
    //*************************************************************************

    public WorksheetContextMenuManager.VertexCommand
    VertexCommand
    {
        get
        {
            AssertValid();

            return (m_eVertexCommand);
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
		Debug.Assert(m_iVertexID == WorksheetContextMenuManager.NoID ||
			m_iVertexID > 0);

		// m_eVertexCommand
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// ID of the vertex that was right-clicked in the vertex table in the
	/// vertex worksheet, or WorksheetContextMenuManager.NoID if a vertex
	/// wasn't right-clicked.

	protected Int32 m_iVertexID;

	/// The command to run.

    protected WorksheetContextMenuManager.VertexCommand m_eVertexCommand;
}


//*****************************************************************************
//  Delegate: RunVertexCommandEventHandler
//
/// <summary>
/// Represents a method that will handle the <see
/// cref="WorksheetContextMenuManager.RunVertexCommand" /> event.
/// </summary>
///
/// <param name="sender">
/// The source of the event.
/// </param>
///
/// <param name="e">
///	A <see cref="RunVertexCommandEventArgs" /> object that contains the event
/// data.
/// </param>
//*****************************************************************************

public delegate void
RunVertexCommandEventHandler
(
	Object sender,
	RunVertexCommandEventArgs e
);

}
