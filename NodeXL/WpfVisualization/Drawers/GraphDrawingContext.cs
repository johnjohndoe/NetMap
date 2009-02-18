
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: GraphDrawingContext
//
/// <summary>
/// Provides access to objects needed for graph-drawing operations.
/// </summary>
//*****************************************************************************

public class GraphDrawingContext : VisualizationBase
{
    //*************************************************************************
    //  Constructor: GraphDrawingContext()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphDrawingContext" />
    /// class.
    /// </summary>
    ///
    /// <param name="graphRectangle">
    /// The <see cref="Rect" /> the graph is being drawn within.
    /// </param>
    ///
    /// <param name="margin">
    /// The margin that was used to lay out the graph.  If <paramref
    /// name="graphRectangle" /> is {L=0, T=0, R=50, B=30} and <paramref
    /// name="margin" /> is 5, for example, then the graph was laid out within
    /// the rectangle {L=5, T=5, R=45, B=25}.
    /// </param>
    ///
    /// <param name="backColor">
    /// The graph's background color.
    /// </param>
    //*************************************************************************

    public GraphDrawingContext
    (
        Rect graphRectangle,
        Int32 margin,
        Color backColor
    )
    {
        const String MethodName = "Constructor";

        ArgumentChecker oArgumentChecker = this.ArgumentChecker;

        oArgumentChecker.CheckArgumentNotNegative(
            MethodName, "margin", margin);

        m_oGraphRectangle = graphRectangle;
        m_iMargin = margin;
        m_oBackColor = backColor;

        m_oVertexDrawingHistories =
            new Dictionary<Int32, VertexDrawingHistory>();

        m_oEdgeDrawingHistories = new Dictionary<Int32, EdgeDrawingHistory>();

        AssertValid();
    }

    //*************************************************************************
    //  Property: GraphRectangle
    //
    /// <summary>
    /// Gets the rectangle the graph is being drawn within.
    /// </summary>
    ///
    /// <value>
    /// The rectangle the graph is being drawn within, as a <see
    /// cref="Rect" />.
    /// </value>
    //*************************************************************************

    public Rect
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
    /// Gets the rectangle the graph is being drawn within, reduced on all
    /// sides by the margin.
    /// </summary>
    ///
    /// <value>
    /// The rectangle the graph is being drawn within, as a <see
    /// cref="Rect" />, reduced by <see cref="Margin" />.
    /// </value>
    ///
    /// <remarks>
    /// If the graph rectangle is narrower or shorter than twice the <see
    /// cref="Margin" />, Rect.Empty is returned.
    /// </remarks>
    //*************************************************************************

    public Rect
    GraphRectangleMinusMargin
    {
        get
        {
            AssertValid();

            return ( WpfGraphicsUtil.GetRectangleMinusMargin(
                m_oGraphRectangle, m_iMargin) );
        }
    }

    //*************************************************************************
    //  Property: Margin
    //
    /// <summary>
    /// Gets the margin the graph was laid out within.
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
    //  Property: BackColor
    //
    /// <summary>
    /// Gets the graph's background color.
    /// </summary>
    ///
    /// <value>
    /// The graph's background color.
    /// </value>
    //*************************************************************************

    public Color
    BackColor
    {
        get
        {
            AssertValid();

            return (m_oBackColor);
        }
    }

    //*************************************************************************
    //  Property: VertexDrawingHistories
    //
    /// <summary>
    /// Gets a dictionary of objects that retain information about each vertex
    /// that was drawn.
    /// </summary>
    ///
    /// <value>
    /// A dictionary of objects that retain information about each vertex that
    /// was drawn.
    /// </value>
    ///
    /// <remarks>
    /// The key is the IVertex.ID and the value is a VertexDrawingHistory
    /// object for that vertex.
    ///
    /// <para>
    /// If a vertex isn't drawn (if it has a <see
    /// cref="Microsoft.NodeXL.Core.ReservedMetadataKeys.Visibility" /> value
    /// of <see cref="Microsoft.NodeXL.Core.VisibilityKeyValue.Filtered" />,
    /// for example), it does not get added to the dictionary.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Dictionary<Int32, VertexDrawingHistory>
    VertexDrawingHistories
    {
        get
        {
            AssertValid();

            return (m_oVertexDrawingHistories);
        }
    }

    //*************************************************************************
    //  Property: EdgeDrawingHistories
    //
    /// <summary>
    /// Gets a dictionary of objects that retain information about each edge
    /// that was drawn.
    /// </summary>
    ///
    /// <value>
    /// A dictionary of objects that retain information about each edge that
    /// was drawn.
    /// </value>
    ///
    /// <remarks>
    /// The key is the IEdge.ID and the value is an EdgeDrawingHistory object
    /// for that edge.
    ///
    /// <para>
    /// If an edge isn't drawn (if it has a <see
    /// cref="Microsoft.NodeXL.Core.ReservedMetadataKeys.Visibility" /> value
    /// of <see cref="Microsoft.NodeXL.Core.VisibilityKeyValue.Filtered" />,
    /// for example), it does not get added to the dictionary.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Dictionary<Int32, EdgeDrawingHistory>
    EdgeDrawingHistories
    {
        get
        {
            AssertValid();

            return (m_oEdgeDrawingHistories);
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
        Debug.Assert(m_iMargin >= 0);
        // m_oBackColor
        Debug.Assert(m_oVertexDrawingHistories != null);
        Debug.Assert(m_oEdgeDrawingHistories != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Rectangle to draw within.

    protected Rect m_oGraphRectangle;

    /// Margin the graph was laid out within.

    protected Int32 m_iMargin;

    /// The graph's background color.

    protected Color m_oBackColor;

    /// Dictionary of objects that retain information about each vertex that
    /// was drawn.  The key is the IVertex.ID and the value is a
    /// VertexDrawingHistory object for that vertex.

    protected Dictionary<Int32, VertexDrawingHistory>
        m_oVertexDrawingHistories;

    /// Dictionary of objects that retain information about each edge that was
    /// drawn.  The key is the IEdge.ID and the value is an EdgeDrawingHistory
    /// object for that edge.

    protected Dictionary<Int32, EdgeDrawingHistory> m_oEdgeDrawingHistories;
}

}
