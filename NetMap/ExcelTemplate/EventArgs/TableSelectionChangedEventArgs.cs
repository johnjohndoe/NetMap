
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: TableSelectionChangedEventArgs
//
/// <summary>
/// Provides event information for events that are fired when the selection in
/// a table changes.
/// </summary>
///
/// <remarks>
/// The table must have a column of unique IDs.
/// </remarks>
//*****************************************************************************

public class TableSelectionChangedEventArgs : EventArgs
{
    //*************************************************************************
    //  Constructor: TableSelectionChangedEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="TableSelectionChangedEventArgs" /> class.
    /// </summary>
	///
    /// <param name="selectedIDs">
	/// Array of unique IDs of table rows that have at least one selected cell.
    /// </param>
	///
    /// <param name="eventOrigin">
	/// Specifies how the event originated.
    /// </param>
    //*************************************************************************

    public TableSelectionChangedEventArgs
	(
		Int32 [] selectedIDs,
		TableSelectionChangedEventOrigin eventOrigin
	)
    {
		m_aiSelectedIDs = selectedIDs;
		m_eEventOrigin = eventOrigin;

		AssertValid();
    }

    //*************************************************************************
    //  Property: SelectedIDs
    //
    /// <summary>
	/// Gets an array of unique IDs of rows that have at least one selected
	/// cell.
    /// </summary>
    ///
    /// <value>
	/// An array of unique IDs of rows that have at least one selected cell.
	/// Can be empty, but is never null.
    /// </value>
	///
	/// <remarks>
	/// The IDs are those that are stored in the table's ID column and
	/// are different from the IEdge.ID and IVertex.ID values used within the
	/// NetMap graph.
	/// </remarks>
    //*************************************************************************

    public Int32 []
    SelectedIDs
    {
        get
        {
            AssertValid();

            return (m_aiSelectedIDs);
        }
    }

    //*************************************************************************
    //  Property: EventOrigin
    //
    /// <summary>
	/// Gets a value that specifies how the event originated.
    /// </summary>
    ///
    /// <value>
	/// An <see cref="EventOrigin" /> value that specifies how the event
	/// originated.
    /// </value>
    //*************************************************************************

    public TableSelectionChangedEventOrigin
    EventOrigin
    {
        get
        {
            AssertValid();

            return (m_eEventOrigin);
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
		Debug.Assert(m_aiSelectedIDs != null);
		// m_eEventOrigin
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Array of unique IDs of table rows that have at least one selected cell.

	protected Int32 [] m_aiSelectedIDs;

	/// Specifies how the event originated.

	protected TableSelectionChangedEventOrigin m_eEventOrigin;
}


//*****************************************************************************
//  Enum: TableSelectionChangedEventOrigin
//
/// <summary>
/// Specifies how a TableSelectionChanged event originated.
/// </summary>
//*****************************************************************************

public enum
TableSelectionChangedEventOrigin
{
	/// <summary>
	/// The user changed the selection in the table.
	/// </summary>

	SelectionChangedInTable,

	/// <summary>
	/// The user changed the selection in the NetMap graph.
	/// </summary>

	SelectionChangedInGraph,
}


//*****************************************************************************
//  Delegate: TableSelectionChangedEventHandler
//
/// <summary>
/// Represents a method that will handle the event that is fired when the
/// selection in a table changes.
/// </summary>
///
/// <param name="sender">
/// The source of the event.
/// </param>
///
/// <param name="e">
///	An <see cref="TableSelectionChangedEventArgs" /> object that contains the
/// event data.
/// </param>
//*****************************************************************************

public delegate void
TableSelectionChangedEventHandler
(
	Object sender,
	TableSelectionChangedEventArgs e
);

}
