
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: VertexAttributesEditedEventArgs
//
/// <summary>
/// Provides event information for the <see
/// cref="TaskPane.VertexAttributesEditedInGraph" /> and <see
/// cref="ThisWorkbook.VertexAttributesEditedInGraph" /> events.
/// </summary>
//*****************************************************************************

public class VertexAttributesEditedEventArgs : EventArgs
{
    //*************************************************************************
    //  Constructor: VertexAttributesEditedEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="VertexAttributesEditedEventArgs" /> class.
    /// </summary>
	///
    /// <param name="vertexIDs">
	/// Array of IDs of the vertices whose attributes were edited in the graph.
	/// The IDs came from the vertex worksheet's ID column.  Can be empty but
	/// not null.
    /// </param>
	///
    /// <param name="editedVertexAttributes">
	/// Vertex attributes that were applied to the vertices.
    /// </param>
    //*************************************************************************

    public VertexAttributesEditedEventArgs
	(
		Int32 [] vertexIDs,
		EditedVertexAttributes editedVertexAttributes
	)
    {
		m_aiVertexIDs = vertexIDs;
		m_oEditedVertexAttributes = editedVertexAttributes;

		AssertValid();
    }

    //*************************************************************************
    //  Property: VertexIDs
    //
    /// <summary>
	/// Gets an array of IDs of the vertices whose attributes were edited.
    /// </summary>
    ///
    /// <value>
	/// An array of IDs of the vertices whose attributes were edited.  Can be
	/// empty but not null.
    /// </value>
	///
	/// <remarks>
	/// The IDs are those that are stored in the vertex worksheet's ID column
	/// and are different from the IVertex.ID values in the graph, which the
	/// vertex worksheet knows nothing about.
	/// </remarks>
    //*************************************************************************

    public Int32 []
    VertexIDs
    {
        get
        {
            AssertValid();

            return (m_aiVertexIDs);
        }
    }

    //*************************************************************************
    //  Property: EditedVertexAttributes
    //
    /// <summary>
	/// Gets the vertex attributes that were applied to the vertices.
    /// </summary>
    ///
    /// <value>
	/// The vertex attributes that were applied to the vertices.
    /// </value>
    //*************************************************************************

    public EditedVertexAttributes
    EditedVertexAttributes
    {
        get
        {
            AssertValid();

            return (m_oEditedVertexAttributes);
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
		Debug.Assert(m_aiVertexIDs != null);
		Debug.Assert(m_oEditedVertexAttributes != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Array of IDs of vertices whose attributes were edited in the graph.

	protected Int32 [] m_aiVertexIDs;

	/// Vertex attributes that were applied to the vertices.

	protected EditedVertexAttributes m_oEditedVertexAttributes;
}


//*****************************************************************************
//  Delegate: VertexAttributesEditedEventHandler
//
/// <summary>
/// Represents a method that will handle the <see
/// cref="TaskPane.VertexAttributesEditedInGraph" /> and <see
/// cref="ThisWorkbook.VertexAttributesEditedInGraph" /> events.
/// </summary>
///
/// <param name="sender">
/// The source of the event.
/// </param>
///
/// <param name="e">
///	An <see cref="VertexAttributesEditedEventArgs" /> object that contains the
/// event data.
/// </param>
//*****************************************************************************

public delegate void
VertexAttributesEditedEventHandler
(
	Object sender,
	VertexAttributesEditedEventArgs e
);

}
