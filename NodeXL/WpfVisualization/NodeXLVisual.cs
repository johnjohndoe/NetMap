
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Media;
using System.Diagnostics;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: NodeXLVisual
//
/// <summary>
/// Visual object that renders a NodeXL graph.
/// </summary>
///
/// <remarks>
/// This class wraps a <see cref="Wpf.GraphDrawer" /> object and renders the
/// drawn graph as a Visual.
///
/// <para>
/// To draw a graph that has already been laid out, call the <see
/// cref="Wpf.GraphDrawer.DrawGraph" /> method on the object returned by the
/// <see cref="GraphDrawer" /> property.  Note that neither <see
/// cref="GraphDrawer" /> nor <see cref="NodeXLVisual" /> lays out the graph.
/// You should lay out the graph using one of the provided layout classes
/// before calling <see cref="Wpf.GraphDrawer.DrawGraph" />.
/// </para>
///
/// <para>
/// <see cref="NodeXLVisual" /> is a lower-level alternative to using
/// NodeXLControl, which is a FrameworkElement class meant for use in WPF
/// desktop applications.  Unlike <see cref="NodeXLVisual" /> and <see
/// cref="GraphDrawer" />, NodeXLControl automatically lays out the graph
/// before drawing it.  Use <see cref="NodeXLVisual" /> where the overhead
/// of a FrameworkElement is not required or more control over the graph's
/// layout is needed.
/// </para>
///
/// <para>
/// Another alternative is to use <see cref="Wpf.GraphDrawer" /> directly.
/// However, <see cref="Wpf.GraphDrawer" /> cannot be directly rendered.  You
/// must implement a custom wrapper that hosts the GraphDrawer.<see
/// cref="Wpf.GraphDrawer.VisualCollection" />.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class NodeXLVisual : Visual
{
    //*************************************************************************
    //  Constructor: NodeXLVisual()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="NodeXLVisual" /> class.
    /// </summary>
    //*************************************************************************

    public NodeXLVisual()
    {
        m_oGraphDrawer = new GraphDrawer(this);

        AssertValid();
    }

    //*************************************************************************
    //  Property: GraphDrawer
    //
    /// <summary>
    /// Gets the object that draws a graph.
    /// </summary>
    ///
    /// <value>
    /// The <see cref="Wpf.GraphDrawer" /> object that draws a graph.
    /// </value>
    ///
    /// <remarks>
    /// To draw a graph, call the <see cref="Wpf.GraphDrawer.DrawGraph" />
    /// method on the returned object.
    /// </remarks>
    //*************************************************************************

    public GraphDrawer
    GraphDrawer
    {
        get
        {
            return (m_oGraphDrawer);
        }
    }

    //*************************************************************************
    //  Property: VisualChildrenCount
    //
    /// <summary>
    /// Gets the number of visual child elements within this element.
    /// </summary>
    ///
    /// <value>
    /// The number of visual child elements for this element.
    /// </value>
    //*************************************************************************

    protected override Int32
    VisualChildrenCount
    {
        get
        {
            return (m_oGraphDrawer.VisualCollection.Count);
        }
    }

    //*************************************************************************
    //  Method: GetVisualChild()
    //
    /// <summary>
    /// Returns a child at the specified index from a collection of child
    /// elements. 
    /// </summary>
    ///
    /// <param name="index">
    /// The zero-based index of the requested child element in the collection.
    /// </param>
    ///
    /// <returns>
    /// The requested child element.
    /// </returns>
    //*************************************************************************

    protected override Visual
    GetVisualChild
    (
        Int32 index
    )
    {
        Debug.Assert(index >= 0);

        return ( m_oGraphDrawer.VisualCollection[index] );
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
        Debug.Assert(m_oGraphDrawer != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The object that draws the graph.

    protected GraphDrawer m_oGraphDrawer;
}

}
