
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: RequestVertexCommandEnableEventArgs
//
/// <summary>
/// Provides event information for the <see
/// cref="WorksheetContextMenuManager.RequestVertexCommandEnable" /> event.
/// </summary>
//*****************************************************************************

public class RequestVertexCommandEnableEventArgs : EventArgs
{
    //*************************************************************************
    //  Constructor: RequestVertexCommandEnableEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="RequestVertexCommandEnableEventArgs" /> class.
    /// </summary>
	///
	/// <param name="vertexID">
    /// ID of the vertex that was right-clicked in the vertex table in the
	/// vertex worksheet, or <see cref="WorksheetContextMenuManager.NoID" /> if
	/// a vertex wasn't right-clicked.  This is an ID that is stored in the
	/// worksheet, NOT an IEdge.ID value.
	/// </param>
    //*************************************************************************

    public RequestVertexCommandEnableEventArgs
	(
		Int32 vertexID
	)
    {
        m_iVertexID = vertexID;
		m_bEnableSelectAllVertices = false;
		m_bEnableDeselectAllVertices = false;
		m_bEnableSelectAdjacentVertices = false;
		m_bEnableDeselectAdjacentVertices = false;
		m_bEnableSelectIncidentEdges = false;
		m_bEnableDeselectIncidentEdges = false;
		m_bEnableEditVertexAttributes = false;
		m_bEnableSelectSubgraphs = false;

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
    //  Property: EnableSelectAllVertices
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the Select All Vertices command
	/// should be enabled.
    /// </summary>
    ///
    /// <value>
	/// true to enable the command.  The default is false.
    /// </value>
    //*************************************************************************

    public Boolean
    EnableSelectAllVertices
    {
        get
        {
            AssertValid();

            return (m_bEnableSelectAllVertices);
        }

		set
		{
            m_bEnableSelectAllVertices = value;

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: EnableDeselectAllVertices
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the Deselect All Vertices
	/// command should be enabled.
    /// </summary>
    ///
    /// <value>
	/// true to enable the command.  The default is false.
    /// </value>
    //*************************************************************************

    public Boolean
    EnableDeselectAllVertices
    {
        get
        {
            AssertValid();

            return (m_bEnableDeselectAllVertices);
        }

		set
		{
            m_bEnableDeselectAllVertices = value;

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: EnableSelectAdjacentVertices
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the Select Adjacent Vertices
	/// command should be enabled.
    /// </summary>
    ///
    /// <value>
	/// true to enable the command.  The default is false.
    /// </value>
    //*************************************************************************

    public Boolean
    EnableSelectAdjacentVertices
    {
        get
        {
            AssertValid();

            return (m_bEnableSelectAdjacentVertices);
        }

		set
		{
            m_bEnableSelectAdjacentVertices = value;

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: EnableDeselectAdjacentVertices
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the Deselect Adjacent Vertices
	/// command should be enabled.
    /// </summary>
    ///
    /// <value>
	/// true to enable the command.  The default is false.
    /// </value>
    //*************************************************************************

    public Boolean
    EnableDeselectAdjacentVertices
    {
        get
        {
            AssertValid();

            return (m_bEnableDeselectAdjacentVertices);
        }

		set
		{
            m_bEnableDeselectAdjacentVertices = value;

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: EnableSelectIncidentEdges
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the Select Incident Edges
	/// command should be enabled.
    /// </summary>
    ///
    /// <value>
	/// true to enable the command.  The default is false.
    /// </value>
    //*************************************************************************

    public Boolean
    EnableSelectIncidentEdges
    {
        get
        {
            AssertValid();

            return (m_bEnableSelectIncidentEdges);
        }

		set
		{
            m_bEnableSelectIncidentEdges = value;

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: EnableDeselectIncidentEdges
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the Deselect Incident Edges
	/// command should be enabled.
    /// </summary>
    ///
    /// <value>
	/// true to enable the command.  The default is false.
    /// </value>
    //*************************************************************************

    public Boolean
    EnableDeselectIncidentEdges
    {
        get
        {
            AssertValid();

            return (m_bEnableDeselectIncidentEdges);
        }

		set
		{
            m_bEnableDeselectIncidentEdges = value;

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: EnableEditVertexAttributes
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the Edit Selected Vertex
	/// Attributes command should be enabled.
    /// </summary>
    ///
    /// <value>
	/// true to enable the command.  The default is false.
    /// </value>
    //*************************************************************************

    public Boolean
    EnableEditVertexAttributes
    {
        get
        {
            AssertValid();

            return (m_bEnableEditVertexAttributes);
        }

		set
		{
            m_bEnableEditVertexAttributes = value;

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: EnableSelectSubgraphs
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the Select Subgraphs command
	/// should be enabled.
    /// </summary>
    ///
    /// <value>
	/// true to enable the command.  The default is false.
    /// </value>
    //*************************************************************************

    public Boolean
    EnableSelectSubgraphs
    {
        get
        {
            AssertValid();

            return (m_bEnableSelectSubgraphs);
        }

		set
		{
            m_bEnableSelectSubgraphs = value;

			AssertValid();
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

		// m_bEnableSelectAllVertices
		// m_bEnableDeselectAllVertices
		// m_bEnableSelectAdjacentVertices
		// m_bEnableDeselectAdjacentVertices
		// m_bEnableSelectIncidentEdges
		// m_bEnableDeselectIncidentEdges
		// m_bEnableEditVertexAttributes
		// m_bEnableSelectSubgraphs
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// ID of the vertex that was right-clicked in the vertex table in the
	/// vertex worksheet, or WorksheetContextMenuManager.NoID if a vertex
	/// wasn't right-clicked.

	protected Int32 m_iVertexID;

	/// true to enable the Select All Vertices command.

	protected Boolean m_bEnableSelectAllVertices;

	/// true to enable the Deselect All Vertices command.

	protected Boolean m_bEnableDeselectAllVertices;

	/// true to enable the Select Adjacent Vertices command.

	protected Boolean m_bEnableSelectAdjacentVertices;

	/// true to enable the Deselect Adjacent Vertices command.

	protected Boolean m_bEnableDeselectAdjacentVertices;

	/// true to enable the Select Incident Edges command.

	protected Boolean m_bEnableSelectIncidentEdges;

	/// true to enable the Deselect Incident Edges command.

	protected Boolean m_bEnableDeselectIncidentEdges;

	/// true to enable the Edit Selected Vertex Attributes command.

	protected Boolean m_bEnableEditVertexAttributes;

	/// true to enable the Select Subgraphs command.

	protected Boolean m_bEnableSelectSubgraphs;
}


//*****************************************************************************
//  Delegate: RequestVertexCommandEnableEventHandler
//
/// <summary>
/// Represents a method that will handle the <see
/// cref="WorksheetContextMenuManager.RequestVertexCommandEnable" /> event.
/// </summary>
///
/// <param name="sender">
/// The source of the event.
/// </param>
///
/// <param name="e">
///	A <see cref="RequestVertexCommandEnableEventArgs" /> object that contains
/// the event data.
/// </param>
//*****************************************************************************

public delegate void
RequestVertexCommandEnableEventHandler
(
	Object sender,
	RequestVertexCommandEnableEventArgs e
);

}
