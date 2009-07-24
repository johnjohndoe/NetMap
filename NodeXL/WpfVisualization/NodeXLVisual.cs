
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
/// <example>
/// Here is sample C# code that uses a <see cref="NodeXLVisual" /> object to
/// write a NodeXL graph to an ASP.NET Web page.
///
/// <code>
/**
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Layouts;
using Microsoft.NodeXL.Visualization.Wpf;

namespace WebApplication1
{
public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        const Int32 GraphWidth = 200;
        const Int32 GraphHeight = 100;

        // The graph will be written to the response stream as a Gif.

        Response.ContentType = "image/gif";

        // Create a graph.  The graph has no visual representation; it is just
        // a data structure.
            
        Graph oGraph = new Graph(GraphDirectedness.Directed);
        IVertexCollection oVertices = oGraph.Vertices;
        IEdgeCollection oEdges = oGraph.Edges;
            
        // Add three vertices.
            
        IVertex oVertexA = oVertices.Add();
        oVertexA.Name = "Vertex A";
        IVertex oVertexB = oVertices.Add();
        oVertexB.Name = "Vertex B";
        IVertex oVertexC = oVertices.Add();
        oVertexC.Name = "Vertex C";
            
        // Connect the vertices with directed edges.
            
        IEdge oEdge1 = oEdges.Add(oVertexA, oVertexB, true);
        IEdge oEdge2 = oEdges.Add(oVertexB, oVertexC, true);
        IEdge oEdge3 = oEdges.Add(oVertexC, oVertexA, true);
            
        // Synchronously lay out the graph using one of the NodeXL-supplied
        // layout objects.
            
        ILayout oLayout = new FruchtermanReingoldLayout();
            
        LayoutContext oLayoutContext = new LayoutContext(
            new Rectangle(0, 0, GraphWidth, GraphHeight) );
            
        oLayout.LayOutGraph(oGraph, oLayoutContext);

        // Create an object that can render a NodeXL graph as a Visual.

        NodeXLVisual oNodeXLVisual = new NodeXLVisual();

        // Use the NodeXLVisual object's GraphDrawer to draw the graph onto the
        // Visual.

        GraphDrawingContext oGraphDrawingContext = new GraphDrawingContext(
            new Rect(0, 0, GraphWidth, GraphHeight), oLayout.Margin,
            System.Windows.Media.Color.FromRgb(255, 255, 255) );

        oNodeXLVisual.GraphDrawer.DrawGraph(oGraph, oGraphDrawingContext);

        // Convert the Visual to a Bitmap.

        RenderTargetBitmap oRenderTargetBitmap = new RenderTargetBitmap(
            GraphWidth, GraphHeight, 96, 96, PixelFormats.Default);

        oRenderTargetBitmap.Render(oNodeXLVisual);
        BmpBitmapEncoder oBmpBitmapEncoder = new BmpBitmapEncoder();
        oBmpBitmapEncoder.Frames.Add( BitmapFrame.Create(oRenderTargetBitmap) );
        MemoryStream oMemoryStream = new MemoryStream();
        oBmpBitmapEncoder.Save(oMemoryStream);
        Bitmap oBitmap = new Bitmap(oMemoryStream);

        // Write the Bitmap's contents to the response stream.

        oBitmap.Save(this.Response.OutputStream, ImageFormat.Gif);
    }
}
}
*/
/// </code>
/// </example>
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
