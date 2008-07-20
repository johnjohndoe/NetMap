
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NetMap.Visualization
{
//*****************************************************************************
//  Class: LayoutContext
//
/// <summary>
/// Provides access to objects needed for laying out a graph.
/// </summary>
//*****************************************************************************

public class LayoutContext : VisualizationBase
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
	///
	/// <param name="graphDrawer">
    /// The <see cref="IGraphDrawer" /> that will be used to draw the graph
	/// after it is laid out.
	/// </param>
    //*************************************************************************

    public LayoutContext
	(
		Rectangle graphRectangle,
		IGraphDrawer graphDrawer
	)
    {
		const String MethodName = "Constructor";

		ArgumentChecker oArgumentChecker = this.ArgumentChecker;

        oArgumentChecker.CheckArgumentNotNull(
			MethodName, "graphDrawer", graphDrawer);

        m_oGraphRectangle = graphRectangle;

		m_oGraphDrawer = graphDrawer;

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
    //  Property: GraphDrawer
    //
    /// <summary>
    /// Gets the <see cref="IGraphDrawer" /> that will be used to draw the
	/// graph after it is laid out.
    /// </summary>
    ///
    /// <value>
    /// The <see cref="IGraphDrawer" /> that will be used to draw the graph
	/// after it is laid out.
    /// </value>
    //*************************************************************************

    public IGraphDrawer
    GraphDrawer
    {
        get
        {
            AssertValid();

            return (m_oGraphDrawer);
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

		Debug.Assert(m_oGraphDrawer != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The Rectangle the graph is being laid out within.

	protected Rectangle m_oGraphRectangle;

    /// The IGraphDrawer that will be used to draw the graph after it is laid
	/// out.

	protected IGraphDrawer m_oGraphDrawer;
}

}
