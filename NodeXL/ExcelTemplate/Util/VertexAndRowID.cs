
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: VertexAndRowID
//
/// <summary>
/// Stores a vertex along with its row ID from the vertex worksheet.
/// </summary>
//*****************************************************************************

public class VertexAndRowID : Object
{
    //*************************************************************************
    //  Constructor: VertexAndRowID()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexAndRowID" /> class.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex.
    /// </param>
    ///
    /// <param name="vertexRowID">
    /// The vertex's row ID from the vertex worksheet.
    /// </param>
    //*************************************************************************

    public VertexAndRowID
    (
        IVertex vertex,
        Int32 vertexRowID
    )
    {
        m_oVertex = vertex;
        m_iVertexRowID = vertexRowID;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Vertex
    //
    /// <summary>
    /// Gets the vertex.
    /// </summary>
    ///
    /// <value>
    /// The vertex, as an IVertex.
    /// </value>
    //*************************************************************************

    public IVertex
    Vertex
    {
        get
        {
            AssertValid();

            return (m_oVertex);
        }
    }

    //*************************************************************************
    //  Property: VertexRowID
    //
    /// <summary>
    /// Gets the vertex's row ID from the vertex worksheet.
    /// </summary>
    ///
    /// <value>
    /// The vertex's row ID from the vertex worksheet, as an Int32.
    /// </value>
    //*************************************************************************

    public Int32
    VertexRowID
    {
        get
        {
            AssertValid();

            return (m_iVertexRowID);
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
        Debug.Assert(m_oVertex != null);
        // m_iVertexRowID
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The vertex.

    protected IVertex m_oVertex;

    /// The vertex's row ID from the vertex worksheet.

    protected Int32 m_iVertexRowID;
}

}
