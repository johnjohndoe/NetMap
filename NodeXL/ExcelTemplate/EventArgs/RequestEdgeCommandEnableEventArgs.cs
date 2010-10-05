
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: RequestEdgeCommandEnableEventArgs
//
/// <summary>
/// Provides event information for the <see
/// cref="WorksheetContextMenuManager.RequestEdgeCommandEnable" /> event.
/// </summary>
//*****************************************************************************

public class RequestEdgeCommandEnableEventArgs : EventArgs
{
    //*************************************************************************
    //  Constructor: RequestEdgeCommandEnableEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="RequestEdgeCommandEnableEventArgs" /> class.
    /// </summary>
    ///
    /// <param name="edgeRowID">
    /// ID of the edge that was right-clicked in the edge table in the edge
    /// worksheet, or <see cref="WorksheetContextMenuManager.NoRowID" /> if an
    /// edge wasn't right-clicked.  This is an ID that is stored in the
    /// worksheet, NOT an IEdge.ID value.
    /// </param>
    //*************************************************************************

    public RequestEdgeCommandEnableEventArgs
    (
        Int32 edgeRowID
    )
    {
        m_iEdgeRowID = edgeRowID;
        m_bEnableSelectAllEdges = false;
        m_bEnableDeselectAllEdges = false;
        m_bEnableSelectAdjacentVertices = false;
        m_bEnableDeselectAdjacentVertices = false;

        AssertValid();
    }

    //*************************************************************************
    //  Property: EdgeRowID
    //
    /// <summary>
    /// Gets the row ID of the edge that was right-clicked in the edge table in
    /// the edge worksheet.
    /// </summary>
    ///
    /// <value>
    /// The row ID of the right-clicked edge, or <see
    /// cref="WorksheetContextMenuManager.NoRowID" /> if an edge wasn't right-
    /// clicked.  This is a row ID that is stored in the worksheet, NOT an
    /// IEdge.ID value.
    /// </value>
    //*************************************************************************

    public Int32
    EdgeRowID
    {
        get
        {
            AssertValid();

            return (m_iEdgeRowID);
        }
    }

    //*************************************************************************
    //  Property: EnableSelectAllEdges
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the Select All Edges command
    /// should be enabled.
    /// </summary>
    ///
    /// <value>
    /// true to enable the command.  The default is false.
    /// </value>
    //*************************************************************************

    public Boolean
    EnableSelectAllEdges
    {
        get
        {
            AssertValid();

            return (m_bEnableSelectAllEdges);
        }

        set
        {
            m_bEnableSelectAllEdges = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EnableDeselectAllEdges
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the Deselect All Edges command
    /// should be enabled.
    /// </summary>
    ///
    /// <value>
    /// true to enable the command.  The default is false.
    /// </value>
    //*************************************************************************

    public Boolean
    EnableDeselectAllEdges
    {
        get
        {
            AssertValid();

            return (m_bEnableDeselectAllEdges);
        }

        set
        {
            m_bEnableDeselectAllEdges = value;

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
        Debug.Assert(m_iEdgeRowID == WorksheetContextMenuManager.NoRowID ||
            m_iEdgeRowID > 0);

        // m_bEnableSelectAllEdges
        // m_bEnableDeselectAllEdges
        // m_bEnableSelectAdjacentVertices
        // m_bEnableDeselectAdjacentVertices
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Row ID of the edge that was right-clicked in the edge table in the edge
    /// worksheet, or WorksheetContextMenuManager.NoRowID if an edge wasn't
    /// right-clicked.

    protected Int32 m_iEdgeRowID;

    /// true to enable the Select All Edges command.

    protected Boolean m_bEnableSelectAllEdges;

    /// true to enable the Deselect All Edges command.

    protected Boolean m_bEnableDeselectAllEdges;

    /// true to enable the Select Adjacent Vertices command.

    protected Boolean m_bEnableSelectAdjacentVertices;

    /// true to enable the Deselect Adjacent Vertices command.

    protected Boolean m_bEnableDeselectAdjacentVertices;
}


//*****************************************************************************
//  Delegate: RequestEdgeCommandEnableEventHandler
//
/// <summary>
/// Represents a method that will handle the <see
/// cref="WorksheetContextMenuManager.RequestEdgeCommandEnable" /> event.
/// </summary>
///
/// <param name="sender">
/// The source of the event.
/// </param>
///
/// <param name="e">
/// A <see cref="RequestEdgeCommandEnableEventArgs" /> object that contains
/// the event data.
/// </param>
//*****************************************************************************

public delegate void
RequestEdgeCommandEnableEventHandler
(
    Object sender,
    RequestEdgeCommandEnableEventArgs e
);

}
