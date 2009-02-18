
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.Layouts
{
//*****************************************************************************
//  Class: LayoutContext
//
/// <summary>
/// Provides access to objects needed for laying out a graph.
/// </summary>
//*****************************************************************************

public class LayoutContext : LayoutsBase
{
    //*************************************************************************
    //  Constructor: LayoutContext()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="LayoutContext" /> class.
    /// </summary>
    ///
    /// <param name="graphRectangle">
    /// The <see cref="System.Drawing.Rectangle" /> the graph is being laid out
    /// within.
    /// </param>
    //*************************************************************************

    public LayoutContext
    (
        Rectangle graphRectangle
    )
    {
        m_oGraphRectangle = graphRectangle;

        AssertValid();
    }

    //*************************************************************************
    //  Property: GraphRectangle
    //
    /// <summary>
    /// Gets the <see cref="System.Drawing.Rectangle" /> the graph is being
    /// laid out within.
    /// </summary>
    ///
    /// <value>
    /// The <see cref="System.Drawing.Rectangle" /> the graph is being laid out
    /// within.
    /// </value>
    //*************************************************************************

    public Rectangle
    GraphRectangle
    {
        get
        {
            AssertValid();

            return (m_oGraphRectangle);
        }
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
        base.AssertValid();

        // m_oGraphRectangle
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The Rectangle the graph is being laid out within.

    protected Rectangle m_oGraphRectangle;
}

}
