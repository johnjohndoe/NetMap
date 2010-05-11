
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: DoubleClickedVertexInfo
//
/// <summary>
/// Stores information about the vertex that was most recently double-clicked
/// in the <see cref="NodeXLControl" />.
/// </summary>
//*****************************************************************************

public class DoubleClickedVertexInfo : Object
{
    //*************************************************************************
    //  Constructor: DoubleClickedVertexInfo()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="DoubleClickedVertexInfo" />  class.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex that was double-clicked.
    /// </param>
    //*************************************************************************

    public DoubleClickedVertexInfo
    (
        IVertex vertex
    )
    {
        m_oVertex = vertex;
        m_decSelectedSubgraphLevels = 0.0M;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Vertex
    //
    /// <summary>
    /// Gets the vertex that was double-clicked.
    /// </summary>
    ///
    /// <value>
    /// The vertex that was double-clicked.  Never null.
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

        set
        {
            m_oVertex = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SelectedSubgraphLevels
    //
    /// <summary>
    /// Gets or sets the number of subgraph levels selected for the
    /// double-clicked vertex.
    /// </summary>
    ///
    /// <value>
    /// The number of subgraph levels that are selected for the double-clicked
    /// vertex.  See SubgraphCalculator.GetSubgraph() for more details on
    /// subgraph levels.  The default value is 0.0.
    /// </value>
    //*************************************************************************

    public Decimal
    Levels
    {
        get
        {
            AssertValid();

            return (m_decSelectedSubgraphLevels);
        }

        set
        {
            m_decSelectedSubgraphLevels = value;

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
        Debug.Assert(m_oVertex != null);
        Debug.Assert(m_decSelectedSubgraphLevels >= 0);
    }


    //*************************************************************************
    //  Protected member data
    //*************************************************************************

    /// The vertex that was double-clicked.

    protected IVertex m_oVertex;

    /// The number of subgraph levels that are selected for the double-clicked
    /// vertex.

    protected Decimal m_decSelectedSubgraphLevels;
}
}
