
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: SelectionChangedEventArgs
//
/// <summary>
/// Provides event information for the <see
/// cref="ThisWorkbook.SelectionChangedInWorkbook" /> and <see
/// cref="TaskPane.SelectionChangedInGraph" /> events.
/// </summary>
//*****************************************************************************

public class SelectionChangedEventArgs : EventArgs
{
    //*************************************************************************
    //  Constructor: SelectionChangedEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="SelectionChangedEventArgs" /> class.
    /// </summary>
    ///
    /// <param name="selectedEdgeIDs">
    /// Array of unique IDs of edges that have at least one selected cell.  Can
    /// be empty but not null.
    /// </param>
    ///
    /// <param name="selectedVertexIDs">
    /// Array of unique IDs of vertices that have at least one selected cell.
    /// Can be empty but not null.
    /// </param>
    //*************************************************************************

    public SelectionChangedEventArgs
    (
        Int32 [] selectedEdgeIDs,
        Int32 [] selectedVertexIDs
    )
    {
        m_aiSelectedEdgeIDs = selectedEdgeIDs;
        m_aiSelectedVertexIDs = selectedVertexIDs;

        AssertValid();
    }

    //*************************************************************************
    //  Property: SelectedEdgeIDs
    //
    /// <summary>
    /// Gets an array of unique IDs of edges that have at least one selected
    /// cell.
    /// </summary>
    ///
    /// <value>
    /// An array of unique IDs of edges that have at least one selected cell.
    /// Can be empty but not null.
    /// </value>
    ///
    /// <remarks>
    /// The IDs are those that are stored in the edge worksheet's ID column and
    /// are different from the IEdge.ID values in the graph, which the edge
    /// worksheet knows nothing about.
    /// </remarks>
    //*************************************************************************

    public Int32 []
    SelectedEdgeIDs
    {
        get
        {
            AssertValid();

            return (m_aiSelectedEdgeIDs);
        }
    }

    //*************************************************************************
    //  Property: SelectedVertexIDs
    //
    /// <summary>
    /// Gets an array of unique IDs of vertices that have at least one selected
    /// cell.
    /// </summary>
    ///
    /// <value>
    /// An array of unique IDs of vertices that have at least one selected
    /// cell.  Can be empty but not null.
    /// </value>
    ///
    /// <remarks>
    /// The IDs are those that are stored in the vertex worksheet's ID column
    /// and are different from the IVertex.ID values in the graph, which the
    /// vertex worksheet knows nothing about.
    /// </remarks>
    //*************************************************************************

    public Int32 []
    SelectedVertexIDs
    {
        get
        {
            AssertValid();

            return (m_aiSelectedVertexIDs);
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
        Debug.Assert(m_aiSelectedEdgeIDs != null);
        Debug.Assert(m_aiSelectedVertexIDs != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Array of unique IDs of edges that have at least one selected cell.

    protected Int32 [] m_aiSelectedEdgeIDs;

    /// Array of unique IDs of vertices that have at least one selected cell.

    protected Int32 [] m_aiSelectedVertexIDs;
}


//*****************************************************************************
//  Delegate: SelectionChangedEventHandler
//
/// <summary>
/// Represents a method that will handle the <see
/// cref="ThisWorkbook.SelectionChangedInWorkbook" /> and <see
/// cref="TaskPane.SelectionChangedInGraph" /> events.
/// </summary>
///
/// <param name="sender">
/// The source of the event.
/// </param>
///
/// <param name="e">
/// An <see cref="SelectionChangedEventArgs" /> object that contains the
/// event data.
/// </param>
//*****************************************************************************

public delegate void
SelectionChangedEventHandler
(
    Object sender,
    SelectionChangedEventArgs e
);

}
