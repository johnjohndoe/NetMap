
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
    protected Microsoft.NodeXL.Visualization.Wpf.NodeXLWithAxesControl
        m_oNodeXLWithAxesControl;

    protected Microsoft.NodeXL.Visualization.Wpf.NodeXLControl
        m_oNodeXLControl;

    public MainForm()
    {
        InitializeComponent();

        CreateNodeXLControl();

        chkShowVertexToolTips.Checked = m_oNodeXLControl.ShowVertexToolTips;
        chkShowAxes.Checked = m_oNodeXLWithAxesControl.ShowAxes;

        cbxMouseSelectionMode.PopulateWithEnumValues(
            typeof(Microsoft.NodeXL.Visualization.Wpf.MouseSelectionMode),
            false);

        cbxMouseSelectionMode.SelectedValue =
            Microsoft.NodeXL.Visualization.Wpf.MouseSelectionMode.
                SelectVertexAndIncidentEdges;

        tbGraphScale.Value = (Int32)m_oNodeXLControl.GraphScale * 100;

        #if false
        m_oNodeXLControl.Layout =
            new Microsoft.NodeXL.Visualization.CircleLayout();
        #endif

        PopulateGraph();
    }

    protected void
    CreateNodeXLControl()
    {
        m_oNodeXLWithAxesControl = new NodeXLWithAxesControl();
        m_oNodeXLWithAxesControl.XAxis.Label = "This is the x-axis";
        m_oNodeXLWithAxesControl.YAxis.Label = "This is the y-axis";
        m_oNodeXLControl = m_oNodeXLWithAxesControl.NodeXLControl;

        m_oNodeXLControl.SelectionChanged +=
            new System.EventHandler(this.m_oNodeXLControl_SelectionChanged);

        m_oNodeXLControl.VertexClick +=
            new Microsoft.NodeXL.Core.VertexEventHandler(
            this.m_oNodeXLControl_VertexClick);

        m_oNodeXLControl.VertexDoubleClick +=
            new Microsoft.NodeXL.Core.VertexEventHandler(
            this.m_oNodeXLControl_VertexDoubleClick);

        m_oNodeXLControl.VertexMouseHover +=
            new Microsoft.NodeXL.Core.VertexEventHandler(
            this.m_oNodeXLControl_VertexMouseHover);

        m_oNodeXLControl.VertexMouseLeave +=
            new EventHandler(this.m_oNodeXLControl_VertexMouseLeave);

        m_oNodeXLControl.VerticesMoved +=
            new Microsoft.NodeXL.Visualization.Wpf.VerticesMovedEventHandler(
            this.m_oNodeXLControl_VerticesMoved);

        m_oNodeXLControl.PreviewVertexToolTipShown +=
            new VertexToolTipShownEventHandler(
                this.m_oNodeXLControl_PreviewVertexToolTipShown);

        m_oNodeXLControl.GraphMouseDown +=
            new Microsoft.NodeXL.Visualization.Wpf.GraphMouseButtonEventHandler(
            this.m_oNodeXLControl_GraphMouseDown);

        m_oNodeXLControl.GraphMouseUp +=
            new Microsoft.NodeXL.Visualization.Wpf.GraphMouseButtonEventHandler(
            this.m_oNodeXLControl_GraphMouseUp);

        m_oNodeXLControl.GraphZoomChanged +=
            new System.EventHandler(this.m_oNodeXLControl_GraphZoomChanged);

        m_oNodeXLControl.DrawingGraph +=
            new System.EventHandler(this.m_oNodeXLControl_DrawingGraph);

        m_oNodeXLControl.GraphDrawn +=
            new System.EventHandler(this.m_oNodeXLControl_GraphDrawn);

        ehElementHost.Child = m_oNodeXLWithAxesControl;
    }

    protected void
    PopulateGraphFromFile()
    {
        IGraphAdapter oGraphAdapter = new SimpleGraphAdapter();

        /*
        ( (Microsoft.NodeXL.Visualization.Wpf.VertexDrawer)
            m_oNodeXLControl.VertexDrawer ).Radius = 5;
        */

        m_oNodeXLControl.Graph = oGraphAdapter.LoadGraph(
            "..\\..\\SampleGraph.txt");

        AddToolTipsToVertices();

        m_oNodeXLControl.DrawGraph(true);
    }

    protected void
    PopulateGraph()
    {
        IVertexCollection oVertices = m_oNodeXLControl.Graph.Vertices;
        IEdgeCollection oEdges = m_oNodeXLControl.Graph.Edges;
        Double dWidth = this.Width;
        Double dHeight = this.Height;
        Random oRandom = new Random();

        // m_oNodeXLControl.Layout.Margin = 0;

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

        m_oNodeXLControl.DrawGraph(true);

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

        m_oNodeXLControl.DrawGraph(true);

        return;

        #endif
        }

        Microsoft.NodeXL.Visualization.Wpf.VertexShape[] aeShapes
            = (Microsoft.NodeXL.Visualization.Wpf.VertexShape[])
            Enum.GetValues(typeof(Microsoft.NodeXL.Visualization.Wpf.
                VertexShape));

        Int32 iShapes = aeShapes.Length;

        Int32 Vertices = 100;

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
                            "..\\..\\Images\\TestImage1.gif" :
                            "..\\..\\Images\\TestImage2.jpg",
                            UriKind.Relative)));
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

            oEdge.SetValue(ReservedMetadataKeys.PerEdgeLabel,
                "This is an edge label");


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
                IEdge oRandomEdge = oEdges.Add(oPreviousVertex, oVertex, true);

                #if true  // Edge label.

                oRandomEdge.SetValue(ReservedMetadataKeys.PerEdgeLabel,
                    "This is a random edge label");

                #endif
            }
            #endif

            oPreviousVertex = oVertex;
        }

        AddToolTipsToVertices();

        m_oNodeXLControl.DrawGraph(true);
    }

    protected void
    AddToolTipsToVertices()
    {
        foreach (IVertex oVertex in m_oNodeXLControl.Graph.Vertices)
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

        foreach (IVertex oVertex in m_oNodeXLControl.SelectedVertices)
        {
            AddToStatus( oVertex.ID.ToString() );
        }
    }

    protected void
    AddSelectedEdgesToStatus()
    {
        AddToStatus("IDs in SelectedEdges:");

        foreach (IEdge oEdge in m_oNodeXLControl.SelectedEdges)
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
    m_oNodeXLControl_SelectionChanged
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
    m_oNodeXLControl_GraphZoomChanged
    (
        object sender,
        EventArgs e
    )
    {
        AddToStatus("GraphZoomChanged");
    }

    private void
    m_oNodeXLControl_DrawingGraph
    (
        object sender,
        EventArgs e
    )
    {
        AddToStatus("DrawingGraph");
    }

    private void
    m_oNodeXLControl_GraphDrawn
    (
        object sender,
        EventArgs e
    )
    {
        AddToStatus("GraphDrawn");
    }

    private void
    m_oNodeXLControl_VertexClick
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

        // m_oNodeXLControl.ForceRedraw();

        #endif
    }

    private void
    m_oNodeXLControl_VertexDoubleClick
    (
        object sender,
        VertexEventArgs vertexEventArgs
    )
    {
        AddToStatus("VertexDoubleClick: " + vertexEventArgs.Vertex);
    }

    private void
    m_oNodeXLControl_VertexMouseHover
    (
        object sender,
        VertexEventArgs vertexEventArgs
    )
    {
        AddToStatus("VertexMouseHover: " + vertexEventArgs.Vertex);
    }

    private void
    m_oNodeXLControl_VertexMouseLeave
    (
        object sender,
        EventArgs eventArgs
    )
    {
        AddToStatus("VertexMouseLeave");
    }

    private void
    m_oNodeXLControl_VerticesMoved
    (
        object sender,
        VerticesMovedEventArgs verticesMovedEventArgs
    )
    {
        AddToStatus("VerticesMoved: " +
            verticesMovedEventArgs.MovedVertices.Length);
    }

    private void
    m_oNodeXLControl_PreviewVertexToolTipShown
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
    m_oNodeXLControl_GraphMouseDown
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
    m_oNodeXLControl_GraphMouseUp
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
    }

    private void
    btnDeselectAll_Click
    (
        object sender,
        EventArgs e
    )
    {
        m_oNodeXLControl.DeselectAll();
    }

    private void
    chkShowVertexToolTips_CheckedChanged
    (
        object sender,
        EventArgs e
    )
    {
        m_oNodeXLControl.ShowVertexToolTips = chkShowVertexToolTips.Checked;
    }

    private void
    chkShowAxes_CheckedChanged
    (
        object sender,
        EventArgs e
    )
    {
        m_oNodeXLWithAxesControl.ShowAxes = chkShowAxes.Checked;
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

            if ( !m_oNodeXLControl.Graph.Vertices.Find(
                iVertexID, out oVertex) )
            {
                throw new ArgumentException("No such ID.");
            }

            m_oNodeXLControl.SetVertexSelected(oVertex,
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

            if ( !m_oNodeXLControl.Graph.Edges.Find(iEdgeID, out oEdge) )
            {
                throw new ArgumentException("No such ID.");
            }

            m_oNodeXLControl.SetEdgeSelected(oEdge,
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
        m_oNodeXLControl.MouseSelectionMode =
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
        foreach (IVertex oSelectedVertex in m_oNodeXLControl.SelectedVertices)
        {
            oSelectedVertex.SetValue(ReservedMetadataKeys.Visibility,
                VisibilityKeyValue.Hidden);
        }

        foreach (IEdge oSelectedEdge in m_oNodeXLControl.SelectedEdges)
        {
            oSelectedEdge.SetValue(ReservedMetadataKeys.Visibility,
                VisibilityKeyValue.Hidden);
        }

        m_oNodeXLControl.DrawGraph();
    }

    private void
    btnShowSelected_Click
    (
        object sender,
        EventArgs e
    )
    {
        foreach (IVertex oSelectedVertex in m_oNodeXLControl.SelectedVertices)
        {
            oSelectedVertex.RemoveKey(ReservedMetadataKeys.Visibility);
        }

        foreach (IEdge oSelectedEdge in m_oNodeXLControl.SelectedEdges)
        {
            oSelectedEdge.RemoveKey(ReservedMetadataKeys.Visibility);
        }

        m_oNodeXLControl.DrawGraph();
    }

    private void
    tbGraphScale_Scroll
    (
        object sender,
        EventArgs e
    )
    {
        m_oNodeXLControl.GraphScale = tbGraphScale.Value / 100.0;
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
