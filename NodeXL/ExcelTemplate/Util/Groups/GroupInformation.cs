
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Visualization.Wpf;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GroupInformation
//
/// <summary>
/// Contains information about one group of vertices.
/// </summary>
//*****************************************************************************

public class GroupInformation : Object
{
    //*************************************************************************
    //  Constructor: GroupInformation()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupInformation" />
    /// class.
    /// </summary>
    ///
    /// <param name="name">
    /// The name of the group.
    /// </param>
    ///
    /// <param name="rowID">
    /// The group's row ID in the group worksheet, or null if an ID isn't
    /// available.
    /// </param>
    ///
    /// <param name="vertexColor">
    /// The color to use for each of the group's vertices.
    /// </param>
    ///
    /// <param name="vertexShape">
    /// The shape to use for each of the group's vertices.
    /// </param>
    ///
    /// <param name="isCollapsed">
    /// true if the group should be collapsed.
    /// </param>
    //*************************************************************************

    public GroupInformation
    (
        String name,
        Nullable<Int32> rowID,
        Color vertexColor,
        VertexShape vertexShape,
        Boolean isCollapsed
    )
    {
        m_sName = name;
        m_iRowID = rowID;
        m_oVertexColor = vertexColor;
        m_eVertexShape = vertexShape;
        m_bIsCollapsed = isCollapsed;
        m_oVertices = null;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Name
    //
    /// <summary>
    /// Gets the name of the group.
    /// </summary>
    ///
    /// <value>
    /// The name of the group.
    /// </value>
    //*************************************************************************

    public String
    Name
    {
        get
        {
            AssertValid();

            return (m_sName);
        }
    }

    //*************************************************************************
    //  Property: RowID
    //
    /// <summary>
    /// Gets the group's row ID in the group worksheet.
    /// </summary>
    ///
    /// <value>
    /// The group's row ID in the group worksheet, or null if an ID isn't
    /// available.
    /// </value>
    //*************************************************************************

    public Nullable<Int32>
    RowID
    {
        get
        {
            AssertValid();

            return (m_iRowID);
        }
    }

    //*************************************************************************
    //  Property: VertexColor
    //
    /// <summary>
    /// Gets the color to use for each of the group's vertices.
    /// </summary>
    ///
    /// <value>
    /// The color to use for each of the group's vertices.
    /// </value>
    //*************************************************************************

    public Color
    VertexColor
    {
        get
        {
            AssertValid();

            return (m_oVertexColor);
        }
    }

    //*************************************************************************
    //  Property: VertexShape
    //
    /// <summary>
    /// Gets the shape to use for each of the group's vertices.
    /// </summary>
    ///
    /// <value>
    /// The shape to use for each of the group's vertices.
    /// </value>
    //*************************************************************************

    public VertexShape
    VertexShape
    {
        get
        {
            AssertValid();

            return (m_eVertexShape);
        }
    }

    //*************************************************************************
    //  Property: IsCollapsed
    //
    /// <summary>
    /// Gets a flag indicating whether the group should be collapsed.
    /// </summary>
    ///
    /// <value>
    /// true if the group should be collapsed.
    /// </value>
    //*************************************************************************

    public Boolean
    IsCollapsed
    {
        get
        {
            AssertValid();

            return (m_bIsCollapsed);
        }
    }

    //*************************************************************************
    //  Property: Vertices
    //
    /// <summary>
    /// Gets or sets a collection of the vertices in the group, if available.
    /// </summary>
    ///
    /// <value>
    /// A collection of the vertices in the group, or null if they are not
    /// available.  The default value is null.
    /// </value>
    ///
    /// <remarks>
    /// When the workbook is read, the group's vertices are provided only if
    /// both <see cref="ReadWorkbookContext.ReadGroups" /> and <see
    /// cref="ReadWorkbookContext.SaveGroupVertices" /> are set to true.
    /// </remarks>
    //*************************************************************************

    public ICollection<IVertex>
    Vertices
    {
        get
        {
            AssertValid();

            return (m_oVertices);
        }

        set
        {
            m_oVertices = value;

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
        Debug.Assert( !String.IsNullOrEmpty(m_sName) );
        // m_iRowID
        // m_oVertexColor
        // m_eVertexShape
        // m_bIsCollapsed
        // m_oVertices
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The name of the group.

    protected String m_sName;

    /// The group's row ID in the group worksheet, or null.

    protected Nullable<Int32> m_iRowID;

    /// The color to use for each of the group's vertices.

    protected Color m_oVertexColor;

    /// The shape to use for each of the group's vertices.

    protected VertexShape m_eVertexShape;

    /// true if the group should be collapsed.

    protected Boolean m_bIsCollapsed;

    /// Collection of the group's vertices, or null.

    protected ICollection<IVertex> m_oVertices;
}

}
