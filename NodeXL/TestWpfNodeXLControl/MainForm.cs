
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Adapters;
using Microsoft.NodeXL.Visualization.Wpf;

namespace TestWpfNodeXLControl
{
public partial class MainForm : Form
{
    protected Microsoft.NodeXL.Visualization.Wpf.NodeXLControl
        oWpfNodeXLControl;

    public MainForm()
    {
        InitializeComponent();

        CreateWpfNodeXLControl();

        chkShowVertexToolTips.Checked = oWpfNodeXLControl.ShowVertexToolTips;

        cbxMouseSelectionMode.PopulateWithEnumValues(
            typeof(Microsoft.NodeXL.Visualization.Wpf.MouseSelectionMode),
            false);

        cbxMouseSelectionMode.SelectedValue =
            Microsoft.NodeXL.Visualization.Wpf.MouseSelectionMode.
                SelectVertexAndIncidentEdges;

        tbGraphScale.Value = (Int32)oWpfNodeXLControl.GraphScale * 100;

        #if false
        oWpfNodeXLControl.Layout =
            new Microsoft.NodeXL.Visualization.CircleLayout();
        #endif

        PopulateGraph();

        #if false
        ScaleTransform oScaleTransform = new ScaleTransform();
        oScaleTransform.ScaleX = 1.0;
        oScaleTransform.ScaleY = 1.0;

        oWpfNodeXLControl.LayoutTransform = oScaleTransform;
        #endif

        #if false
        ScaleTransform oScaleTransform2 = new ScaleTransform();
        oScaleTransform2.ScaleX = 1.0;
        oScaleTransform2.ScaleY = 1.0;

        oWpfNodeXLControl.RenderTransform = oScaleTransform2;

        oWpfNodeXLControl.RenderTransformOrigin =
            new System.Windows.Point(0.0, 0.0);
        #endif

        ehElementHost.Child = oWpfNodeXLControl;
    }

    protected void
    CreateWpfNodeXLControl()
    {
        oWpfNodeXLControl =
            new Microsoft.NodeXL.Visualization.Wpf.NodeXLControl();

        oWpfNodeXLControl.SelectionChanged +=
            new System.EventHandler(this.oWpfNodeXLControl_SelectionChanged);

        oWpfNodeXLControl.VertexClick +=
            new Microsoft.NodeXL.Core.VertexEventHandler(
            this.oWpfNodeXLControl_VertexClick);

        oWpfNodeXLControl.VertexDoubleClick +=
            new Microsoft.NodeXL.Core.VertexEventHandler(
            this.oWpfNodeXLControl_VertexDoubleClick);

        oWpfNodeXLControl.VertexMouseHover +=
            new Microsoft.NodeXL.Core.VertexEventHandler(
            this.oWpfNodeXLControl_VertexMouseHover);

        oWpfNodeXLControl.VertexMouseLeave +=
            new EventHandler(this.oWpfNodeXLControl_VertexMouseLeave);

        oWpfNodeXLControl.VerticesMoved +=
            new Microsoft.NodeXL.Visualization.Wpf.VerticesMovedEventHandler(
            this.oWpfNodeXLControl_VerticesMoved);

        oWpfNodeXLControl.PreviewVertexToolTipShown +=
            new VertexToolTipShownEventHandler(
                this.oWpfNodeXLControl_PreviewVertexToolTipShown);

        oWpfNodeXLControl.GraphMouseDown +=
            new Microsoft.NodeXL.Visualization.Wpf.GraphMouseButtonEventHandler(
            this.oWpfNodeXLControl_GraphMouseDown);

        oWpfNodeXLControl.GraphMouseUp +=
            new Microsoft.NodeXL.Visualization.Wpf.GraphMouseButtonEventHandler(
            this.oWpfNodeXLControl_GraphMouseUp);

        oWpfNodeXLControl.GraphZoomChanged +=
            new System.EventHandler(this.oWpfNodeXLControl_GraphZoomChanged);

        oWpfNodeXLControl.DrawingGraph +=
            new System.EventHandler(this.oWpfNodeXLControl_DrawingGraph);

        oWpfNodeXLControl.GraphDrawn +=
            new System.EventHandler(this.oWpfNodeXLControl_GraphDrawn);
    }

    protected void
    PopulateGraphFromFile()
    {
        IGraphAdapter oGraphAdapter = new SimpleGraphAdapter();

        /*
        ( (Microsoft.NodeXL.Visualization.Wpf.VertexDrawer)
            oWpfNodeXLControl.VertexDrawer ).Radius = 5;
        */

        oWpfNodeXLControl.Graph = oGraphAdapter.LoadGraph(
            "..\\..\\SampleGraph.txt");

        AddToolTipsToVertices();

        oWpfNodeXLControl.DrawGraph(true);
    }

    protected void
    PopulateGraph()
    {
        IVertexCollection oVertices = oWpfNodeXLControl.Graph.Vertices;
        IEdgeCollection oEdges = oWpfNodeXLControl.Graph.Edges;
        Double dWidth = this.Width;
        Double dHeight = this.Height;
        Random oRandom = new Random();

        // oWpfNodeXLControl.Layout.Margin = 0;

        {
        #if false  // Two shapes only.

        IVertex oVertex1 = oVertices.Add();

        oVertex1.SetValue( ReservedMetadataKeys.PerVertexShape,
            VertexDrawer.VertexShape.Circle);

        oVertex1.SetValue(ReservedMetadataKeys.PerVertexRadius, 50.0F);
        oVertex1.SetValue(ReservedMetadataKeys.LockVertexLocation, true);
        oVertex1.Location = new System.Drawing.PointF(300, 300);

        oVertex1.SetValue(ReservedMetadataKeys.PerVertexSecondaryLabel,
            "This is A: " + oVertex1.Location);

        IVertex oVertex2 = oVertices.Add();

        oVertex2.SetValue( ReservedMetadataKeys.PerVertexShape,
            VertexDrawer.VertexShape.Circle);

        oVertex2.SetValue(ReservedMetadataKeys.PerVertexRadius, 50.0F);
        oVertex2.SetValue(ReservedMetadataKeys.LockVertexLocation, true);
        oVertex2.Location = new System.Drawing.PointF(500, 300);

        oVertex2.SetValue(ReservedMetadataKeys.PerVertexSecondaryLabel,
            "This is B: " + oVertex2.Location);

        IEdge oEdge = oEdges.Add(oVertex1, oVertex2, true);

        // oEdge.SetValue(ReservedMetadataKeys.PerEdgeWidth, 20F);

        oWpfNodeXLControl.ForceLayout();

        return;

        #endif
        }

        {
        #if false  // Two primary labels only.

        IVertex oVertex1 = oVertices.Add();

        oVertex1.SetValue(ReservedMetadataKeys.PerVertexPrimaryLabel,
            "This is a primary label.");

        oVertex1.SetValue(ReservedMetadataKeys.LockVertexLocation, true);
        oVertex1.Location = new System.Drawing.PointF(300, 300);

        oVertex1.SetValue(ReservedMetadataKeys.PerVertexSecondaryLabel,
            "This is A: " + oVertex1.Location);

        IVertex oVertex2 = oVertices.Add();

        oVertex2.SetValue(ReservedMetadataKeys.PerVertexPrimaryLabel,
            "This is another primary label.");

        oVertex2.SetValue(ReservedMetadataKeys.LockVertexLocation, true);
        oVertex2.Location = new System.Drawing.PointF(500, 100);

        oVertex2.SetValue(ReservedMetadataKeys.PerVertexSecondaryLabel,
            "This is B: " + oVertex2.Location);

        oEdges.Add(oVertex1, oVertex2, true);

        oWpfNodeXLControl.ForceLayout();

        return;

        #endif
        }

        Microsoft.NodeXL.Visualization.Wpf.VertexShape[] aeShapes
            = (Microsoft.NodeXL.Visualization.Wpf.VertexShape[])
            Enum.GetValues(typeof(Microsoft.NodeXL.Visualization.Wpf.
                VertexShape));

        Int32 iShapes = aeShapes.Length;

        Int32 Vertices = 500;

        IVertex oFirstVertex = oVertices.Add();

        oFirstVertex.SetValue(ReservedMetadataKeys.PerVertexRadius, 15.0F);

        IVertex oPreviousVertex = oFirstVertex;

        for (Int32 i = 1; i < Vertices; i++)
        {
            IVertex oVertex = oVertices.Add();

            oVertex.SetValue( ReservedMetadataKeys.PerVertexShape,
                aeShapes[ oRandom.Next(iShapes) ] );

            #if false  // Hard-coded vertex shape.

            oVertex.SetValue(ReservedMetadataKeys.PerVertexShape,
                VertexDrawer.VertexShape.Diamond);

            #endif

            #if true  // Vertex color.

            oVertex.SetValue( ReservedMetadataKeys.PerColor,
                System.Windows.Media.Color.FromArgb(255,
                (Byte)oRandom.Next(256),
                (Byte)oRandom.Next(256),
                (Byte)oRandom.Next(256))
                );

            #endif

            #if true  // Vertex radius.

            Single fRadius = (Single)(
                Microsoft.NodeXL.Visualization.Wpf.VertexDrawer.MinimumRadius +
                (Microsoft.NodeXL.Visualization.Wpf.VertexDrawer.MaximumRadius
                - Microsoft.NodeXL.Visualization.Wpf.VertexDrawer.MinimumRadius)
                    * oRandom.NextDouble() );

            oVertex.SetValue(ReservedMetadataKeys.PerVertexRadius, fRadius);

            #endif

            #if true

            if (true && oRandom.Next(50) == 0)  // Image
            {
                oVertex.SetValue( ReservedMetadataKeys.PerVertexImage,
                    new System.Windows.Media.Imaging.BitmapImage(
                        new Uri( oRandom.Next(2) == 1 ?
                            "C:\\Temp\\1.jpg" : "C:\\Temp\\2.jpg") ) );
            }
            #endif

            {
            #if true  // Secondary label.

            String sSecondaryLabel = "This is a secondary label";

            oVertex.SetValue(ReservedMetadataKeys.PerVertexSecondaryLabel,
                sSecondaryLabel);

            #endif
            }

            if (true && oRandom.Next(50) == 0)  // Primary label
            {
                String sPrimaryLabel = "This is a primary label";

                if (oRandom.Next(2) == 0)
                {
                    sPrimaryLabel = LongPrimaryLabel;
                }

                oVertex.SetValue(ReservedMetadataKeys.PerVertexPrimaryLabel,
                    sPrimaryLabel);

                /*
                oVertex.SetValue( ReservedMetadataKeys.PerColor,
                    System.Windows.Media.Color.FromArgb(255, 0, 0, 0) );

                oVertex.SetValue(

                    ReservedMetadataKeys.PerVertexPrimaryLabelFillColor,
                        System.Windows.Media.Color.FromArgb(255, 200, 200,
                            200) );

                oVertex.SetValue(ReservedMetadataKeys.PerAlpha, (Byte)128);
                */
            }

            if (true && oRandom.Next(1) == 1)  // Vertex visibility.
            {
                oVertex.SetValue(ReservedMetadataKeys.Visibility,
                    VisibilityKeyValue.Filtered);
            }

            #if false  // Vertex alpha.

            oVertex.SetValue(
                ReservedMetadataKeys.PerAlpha, (Byte)oRandom.Next(256) );

            #endif

            #if true  // VertexDrawingPrecedence.

            oVertex.SetValue(
                ReservedMetadataKeys.PerVertexDrawingPrecedence,
                VertexDrawingPrecedence.PrimaryLabel);

            #endif

            #if false  // Vertex IsSelected.

            oVertex.SetValue(ReservedMetadataKeys.IsSelected, null);

            #endif

            oVertex.Location = new System.Drawing.PointF(
                (Single)( dWidth * oRandom.NextDouble() ),
                (Single)( dHeight * oRandom.NextDouble() )
                );



            IEdge oEdge = oEdges.Add(oFirstVertex, oVertex, true);

            #if false  // Edge color.

            oEdge.SetValue( ReservedMetadataKeys.PerColor,
                System.Windows.Media.Color.FromArgb(255,
                (Byte)oRandom.Next(256),
                (Byte)oRandom.Next(256),
                (Byte)oRandom.Next(256))
                );

            #endif

            #if false  // Edge width.

            Double dEdgeWidth = EdgeDrawer.MinimumWidth +
                (EdgeDrawer.MaximumWidth - EdgeDrawer.MinimumWidth)
                    * oRandom.NextDouble();

            oEdge.SetValue(ReservedMetadataKeys.PerEdgeWidth, dEdgeWidth);

            #endif

            #if true  // Edge visibility.

            oEdge.SetValue(ReservedMetadataKeys.Visibility,
                VisibilityKeyValue.Visible);

            #endif

            #if true  // Edge alpha.

            oEdge.SetValue( ReservedMetadataKeys.PerAlpha,
                (Byte)oRandom.Next(256) );

            #endif

            #if false  // Edge IsSelected.

            oEdge.SetValue(ReservedMetadataKeys.IsSelected, null);

            #endif


            #if true
            if (oRandom.Next(1) == 0)
            {
                oEdges.Add(oPreviousVertex, oVertex, true);
            }
            #endif

            oPreviousVertex = oVertex;
        }

        AddToolTipsToVertices();

        oWpfNodeXLControl.DrawGraph(true);
    }

    protected void
    AddToolTipsToVertices()
    {
        foreach (IVertex oVertex in oWpfNodeXLControl.Graph.Vertices)
        {
            oVertex.SetValue(ReservedMetadataKeys.VertexToolTip, String.Format(

                "This is the tooltip for the vertex with ID {0}."
                ,
                oVertex.ID.ToString()
                ) );
        }
    }

    protected void
    AddSelectedVerticesToStatus()
    {
        AddToStatus("IDs in SelectedVertices:");

        foreach (IVertex oVertex in oWpfNodeXLControl.SelectedVertices)
        {
            AddToStatus( oVertex.ID.ToString() );
        }
    }

    protected void
    AddSelectedEdgesToStatus()
    {
        AddToStatus("IDs in SelectedEdges:");

        foreach (IEdge oEdge in oWpfNodeXLControl.SelectedEdges)
        {
            AddToStatus( oEdge.ID.ToString() );
        }
    }

    protected void
    AddToStatus
    (
        String sText
    )
    {
        // Add the text to the current results.  Precede it with a new line
        // if this isn't the first line.

        String sStatusText = txbStatus.Text;

        if (sStatusText.Length != 0)
            sStatusText += Environment.NewLine;

        sStatusText += sText;
        txbStatus.Text = sStatusText;

        // Scroll to the bottom.

        // txbStatus.Focus();
        txbStatus.Select(sStatusText.Length, 0);
        txbStatus.ScrollToCaret();
    }

    private void
    oWpfNodeXLControl_SelectionChanged
    (
        object sender,
        EventArgs e
    )
    {
        AddToStatus("SelectionChanged");

        #if false

        AddSelectedVerticesToStatus();
        AddSelectedEdgesToStatus();

        #endif
    }

    private void
    oWpfNodeXLControl_GraphZoomChanged
    (
        object sender,
        EventArgs e
    )
    {
        AddToStatus("GraphZoomChanged");
    }

    private void
    oWpfNodeXLControl_DrawingGraph
    (
        object sender,
        EventArgs e
    )
    {
        AddToStatus("DrawingGraph");
    }

    private void
    oWpfNodeXLControl_GraphDrawn
    (
        object sender,
        EventArgs e
    )
    {
        AddToStatus("GraphDrawn");
    }

    private void
    oWpfNodeXLControl_VertexClick
    (
        object sender,
        VertexEventArgs vertexEventArgs
    )
    {
        AddToStatus("VertexClick: " + vertexEventArgs.Vertex);

        #if false

        // Retrieve the clicked vertex.

        IVertex oClickedVertex = vertexEventArgs.Vertex;

        // Get a new image for the vertex.

        oClickedVertex.SetValue( ReservedMetadataKeys.PerVertexImage,
            new System.Windows.Media.Imaging.BitmapImage(
                new Uri("C:\\Temp\\1.jpg") ) );

        // oWpfNodeXLControl.ForceRedraw();

        #endif
    }

    private void
    oWpfNodeXLControl_VertexDoubleClick
    (
        object sender,
        VertexEventArgs vertexEventArgs
    )
    {
        AddToStatus("VertexDoubleClick: " + vertexEventArgs.Vertex);
    }

    private void
    oWpfNodeXLControl_VertexMouseHover
    (
        object sender,
        VertexEventArgs vertexEventArgs
    )
    {
        AddToStatus("VertexMouseHover: " + vertexEventArgs.Vertex);
    }

    private void
    oWpfNodeXLControl_VertexMouseLeave
    (
        object sender,
        EventArgs eventArgs
    )
    {
        AddToStatus("VertexMouseLeave");
    }

    private void
    oWpfNodeXLControl_VerticesMoved
    (
        object sender,
        VerticesMovedEventArgs verticesMovedEventArgs
    )
    {
        AddToStatus("VerticesMoved: " +
            verticesMovedEventArgs.MovedVertices.Length);
    }

    private void
    oWpfNodeXLControl_PreviewVertexToolTipShown
    (
        object sender,
        VertexToolTipShownEventArgs vertexToolTipShownEventArgs
    )
    {
        AddToStatus("PreviewVertexToolTipShown");

        #if false
        System.Windows.Controls.TextBox oTextBox =
            new System.Windows.Controls.TextBox();

        oTextBox.MinWidth = 500;
        oTextBox.MinHeight = 100;

        vertexToolTipShownEventArgs.VertexToolTip = oTextBox;
        #endif
    }

    private void
    oWpfNodeXLControl_GraphMouseDown
    (
        object sender,
        GraphMouseButtonEventArgs graphMouseButtonEventArgs
    )
    {
        IVertex oClickedVertex = graphMouseButtonEventArgs.Vertex;

        if (oClickedVertex == null)
        {
            AddToStatus("GraphMouseDown: No vertex.");
        }
        else
        {
            AddToStatus("GraphMouseDown: " + oClickedVertex);
        }
    }

    private void
    oWpfNodeXLControl_GraphMouseUp
    (
        object sender,
        GraphMouseButtonEventArgs graphMouseButtonEventArgs
    )
    {
        IVertex oClickedVertex = graphMouseButtonEventArgs.Vertex;

        if (oClickedVertex == null)
        {
            AddToStatus("GraphMouseUp: No vertex.");
        }
        else
        {
            AddToStatus("GraphMouseUp: " + oClickedVertex);
        }
    }

    private void
    btnClearStatus_Click
    (
        object sender,
        EventArgs e
    )
    {
        txbStatus.Clear();

        // TODO

        Bitmap oBitmap = oWpfNodeXLControl.CopyGraphToBitmap();

        oBitmap.Save("C:\\Temp\\Temp.jpg",
            System.Drawing.Imaging.ImageFormat.Jpeg);
    }

    private void
    btnDeselectAll_Click
    (
        object sender,
        EventArgs e
    )
    {
        oWpfNodeXLControl.DeselectAll();
    }

    private void
    chkShowVertexToolTips_CheckedChanged
    (
        object sender,
        EventArgs e
    )
    {
        oWpfNodeXLControl.ShowVertexToolTips = chkShowVertexToolTips.Checked;
    }

    private void
    btnSelectedVertices_Click
    (
        object sender,
        EventArgs e
    )
    {
        AddSelectedVerticesToStatus();
    }

    private void
    btnSelectedEdges_Click
    (
        object sender,
        EventArgs e
    )
    {
        AddSelectedEdgesToStatus();
    }

    private void
    btnSetVertexSelected_Click
    (
        object sender,
        EventArgs e
    )
    {
        try
        {
            Int32 iVertexID = Int32.Parse(txbVertexID.Text);

            IVertex oVertex;

            if ( !oWpfNodeXLControl.Graph.Vertices.Find(
                iVertexID, out oVertex) )
            {
                throw new ArgumentException("No such ID.");
            }

            oWpfNodeXLControl.SetVertexSelected(oVertex,
                chkVertexSelected.Checked, chkAlsoIncidentEdges.Checked);
        }
        catch (Exception oException)
        {
            MessageBox.Show(oException.Message);
        }
    }

    private void
    btnSetEdgeSelected_Click
    (
        object sender,
        EventArgs e
    )
    {
        try
        {
            Int32 iEdgeID = Int32.Parse(txbEdgeID.Text);

            IEdge oEdge;

            if ( !oWpfNodeXLControl.Graph.Edges.Find(iEdgeID, out oEdge) )
            {
                throw new ArgumentException("No such ID.");
            }

            oWpfNodeXLControl.SetEdgeSelected(oEdge,
                chkEdgeSelected.Checked, chkAlsoAdjacentVertices.Checked);
        }
        catch (Exception oException)
        {
            MessageBox.Show(oException.Message);
        }
    }

    private void
    cbxMouseSelectionMode_SelectedIndexChanged
    (
        object sender,
        EventArgs e
    )
    {
        oWpfNodeXLControl.MouseSelectionMode =
            (Microsoft.NodeXL.Visualization.Wpf.MouseSelectionMode)
            cbxMouseSelectionMode.SelectedValue;
    }

    private void
    btnHideSelected_Click
    (
        object sender,
        EventArgs e
    )
    {
        foreach (IVertex oSelectedVertex in oWpfNodeXLControl.SelectedVertices)
        {
            oSelectedVertex.SetValue(ReservedMetadataKeys.Visibility,
                VisibilityKeyValue.Hidden);
        }

        foreach (IEdge oSelectedEdge in oWpfNodeXLControl.SelectedEdges)
        {
            oSelectedEdge.SetValue(ReservedMetadataKeys.Visibility,
                VisibilityKeyValue.Hidden);
        }

        oWpfNodeXLControl.DrawGraph();
    }

    private void
    btnShowSelected_Click
    (
        object sender,
        EventArgs e
    )
    {
        foreach (IVertex oSelectedVertex in oWpfNodeXLControl.SelectedVertices)
        {
            oSelectedVertex.RemoveKey(ReservedMetadataKeys.Visibility);
        }

        foreach (IEdge oSelectedEdge in oWpfNodeXLControl.SelectedEdges)
        {
            oSelectedEdge.RemoveKey(ReservedMetadataKeys.Visibility);
        }

        oWpfNodeXLControl.DrawGraph();
    }

    private void
    tbGraphScale_Scroll
    (
        object sender,
        EventArgs e
    )
    {
        oWpfNodeXLControl.GraphScale = tbGraphScale.Value / 100.0;
    }

    protected const String LongPrimaryLabel =
        "The Width of the resulting rectangle is increased or decreased by"
        + " twice the specified width offset, because it is applied to both"
        + " the left and right sides of the rectangle. Likewise, the Height of"
        + " the resulting rectangle is increased or decreased by twice the"
        + " specified height."
        ;
}

}
