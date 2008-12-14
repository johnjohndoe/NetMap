
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.Visualization
{
//*****************************************************************************
//  Class: DrawContext
//
/// <summary>
/// Provides access to objects needed for drawing operations.
/// </summary>
//*****************************************************************************

public class DrawContext : VisualizationBase
{
    //*************************************************************************
    //  Constructor: DrawContext()
    //
    /// <summary>
    /// Initializes a new instance of the DrawContext class.
    /// </summary>
	///
	/// <param name="graphDrawer">
    /// The <see cref="IGraphDrawer" /> that is drawing the graph.
	/// </param>
	///
	/// <param name="graphics">
    /// The <see cref="System.Drawing.Graphics" /> object to draw with.  The
    /// <see cref="System.Drawing.Graphics.Clip" /> property must already be
	/// set to <paramref name="graphRectangle" /> to prevent drawing operations
	/// from exceeding the graph's bounds.
	/// </param>
	///
	/// <param name="graphRectangle">
    /// The <see cref="System.Drawing.Rectangle" /> the graph is being drawn
	/// within.
	/// </param>
	///
	/// <param name="margin">
    /// The margin that was used to lay out the graph.  If <paramref
	/// name="graphRectangle" /> is {L=0, T=0, R=50, B=30} and <paramref
	/// name="margin" /> is 5, for example, then the graph was laid out within
	/// the rectangle {L=5, T=5, R=45, B=25}.
	/// </param>
    //*************************************************************************

    public DrawContext
	(
		IGraphDrawer graphDrawer,
		Graphics graphics,
		Rectangle graphRectangle,
		Int32 margin
	)
    {
		const String MethodName = "Constructor";

		ArgumentChecker oArgumentChecker = this.ArgumentChecker;

        oArgumentChecker.CheckArgumentNotNull(
			MethodName, "graphDrawer", graphDrawer);

        oArgumentChecker.CheckArgumentNotNull(
			MethodName, "graphics", graphics);

		if ( graphics.Clip.IsInfinite(graphics) ||
			( graphics.ClipBounds != Rectangle.Ceiling(graphRectangle) ) )
		{
            oArgumentChecker.ThrowArgumentException(MethodName, "graphics",

				"The graphics.ClipBounds rectangle differs from the"
				+ " graphRectangle argument.  Set graphics.Clip before calling"
				+ " the constructor."
				);
		}

        oArgumentChecker.CheckArgumentNotNegative(
			MethodName, "margin", margin);

		m_oGraphDrawer = graphDrawer;

		m_oGraphics = graphics;

        m_oGraphRectangle = graphRectangle;

		m_iMargin = margin;

		AssertValid();
    }

    //*************************************************************************
    //  Property: GraphDrawer
    //
    /// <summary>
    /// Gets the <see cref="IGraphDrawer" /> that is drawing the graph.
    /// </summary>
    ///
    /// <value>
    /// The <see cref="IGraphDrawer" /> that is drawing the graph.
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
    //  Property: Graphics
    //
    /// <summary>
    /// Gets the <see cref="System.Drawing.Graphics" /> object to draw with.
    /// </summary>
    ///
    /// <value>
    /// The <see cref="System.Drawing.Graphics" /> object to draw with.
    /// </value>
	///
	/// <remarks>
    /// The <see cref="System.Drawing.Graphics.Clip" /> property is already
	/// set to <see cref="GraphRectangle" /> to prevent drawing operations
	/// from exceeding the graph's bounds.
	/// </remarks>
    //*************************************************************************

    public Graphics
    Graphics
    {
        get
        {
            AssertValid();

            return (m_oGraphics);
        }
    }

    //*************************************************************************
    //  Property: GraphRectangle
    //
    /// <summary>
    /// Gets the <see cref="System.Drawing.Rectangle" /> the graph is being
	/// drawn within.
    /// </summary>
    ///
    /// <value>
    /// The <see cref="System.Drawing.Rectangle" /> the graph is being drawn
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
    //  Property: GraphRectangleMinusMargin
    //
    /// <summary>
    /// Gets the <see cref="System.Drawing.Rectangle" /> the graph is being
	/// drawn within, reduced on all sides by the <see cref="Margin" />.
    /// </summary>
    ///
    /// <value>
    /// The <see cref="System.Drawing.Rectangle" /> the graph is being drawn
	/// within, reduced by <see cref="Margin" />.
    /// </value>
	///
	/// <remarks>
	/// If the graph rectangle is narrower or shorter than twice the <see
	/// cref="Margin" />, Rectangle.Empty is returned.
	/// </remarks>
    //*************************************************************************

    public Rectangle
    GraphRectangleMinusMargin
    {
        get
        {
            AssertValid();

			Rectangle oRectangleMinusMargin = m_oGraphRectangle;

			oRectangleMinusMargin.Inflate(-m_iMargin, -m_iMargin);

			if (oRectangleMinusMargin.Width <= 0 ||
				oRectangleMinusMargin.Height <= 0)
			{
				return (Rectangle.Empty);
			}

            return (oRectangleMinusMargin);
        }
    }

    //*************************************************************************
    //  Property: Margin
    //
    /// <summary>
	///	Gets the margin the graph was laid out within.
    /// </summary>
    ///
    /// <value>
	/// The margin that was used to lay out the graph.  Always greater than or
	/// equal to zero.
    /// </value>
	///
	/// <remarks>
    /// If <see cref="GraphRectangle" /> is {L=0, T=0, R=50, B=30} and <see
	/// cref="Margin" /> is 5, for example, then the graph was laid out within
	/// the rectangle {L=5, T=5, R=45, B=25}.
	/// </remarks>
    //*************************************************************************

    public Int32
    Margin
    {
        get
		{
			AssertValid();

			return (m_iMargin);
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

		Debug.Assert(m_oGraphDrawer != null);

		Debug.Assert(m_oGraphics != null);

		// m_oGraphRectangle

		Debug.Assert(m_iMargin >= 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// The IGraphDrawer that is drawing the graph.

	protected IGraphDrawer m_oGraphDrawer;

    /// Graphics object to draw with.

	protected Graphics m_oGraphics;

	/// Rectangle to draw within.

	protected Rectangle m_oGraphRectangle;

	///	Margin the graph was laid out within.

	protected Int32 m_iMargin;
}

}
