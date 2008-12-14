
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Adapters;
using Microsoft.NodeXL.Visualization;

namespace TestNodeXLControl
{
public partial class MainForm : Form
{
	public MainForm()
	{
		InitializeComponent();

		chkShowToolTips.Checked = oNodeXLControl.ShowToolTips;

        cbxMouseSelectionMode.PopulateWithEnumValues(
            typeof(MouseSelectionMode), false);

        cbxMouseSelectionMode.SelectedValue =
            MouseSelectionMode.SelectVertexAndIncidentEdges;

		PopulateGraph();
	}

	protected void
	PopulateGraph()
	{
		IGraphAdapter oGraphAdapter = new SimpleGraphAdapter();

		oNodeXLControl.BeginUpdate();

        ( (VertexDrawer)oNodeXLControl.VertexDrawer ).Radius = 5;

		#if false

		oNodeXLControl.Layout = new GridLayout();

		IVertexCollection oVertices = oNodeXLControl.Graph.Vertices;
		IEdgeCollection oEdges = oNodeXLControl.Graph.Edges;

		Int32 Vertices = 2000;

		IVertex oFirstVertex = oVertices.Add();

		for (Int32 i = 1; i < Vertices; i++)
		{
			IVertex oVertex = oVertices.Add();
			oEdges.Add(oFirstVertex, oVertex);
		}

		#else

		oNodeXLControl.Graph = oGraphAdapter.LoadGraph(
			"..\\..\\SampleGraph.txt");

		#endif

		foreach (IVertex oVertex in oNodeXLControl.Graph.Vertices)
		{
			oVertex.SetValue(ReservedMetadataKeys.ToolTip, String.Format(

				"This is the tooltip for the vertex with ID {0}."
				,
				oVertex.ID.ToString()
				) );
		}

		oNodeXLControl.EndUpdate();
	}

	protected void
	AddSelectedVerticesToStatus()
	{
		AddToStatus("IDs in SelectedVertices:");

		foreach (IVertex oVertex in oNodeXLControl.SelectedVertices)
		{
			AddToStatus( oVertex.ID.ToString() );
		}
	}

	protected void
	AddSelectedEdgesToStatus()
	{
		AddToStatus("IDs in SelectedEdges:");

		foreach (IEdge oEdge in oNodeXLControl.SelectedEdges)
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

		txbStatus.Focus();
		txbStatus.Select(sStatusText.Length, 0);
		txbStatus.ScrollToCaret();
	}

    private void
	oNodeXLControl_SelectionChanged
	(
		object sender,
		EventArgs e
	)
    {
		AddToStatus("SelectionChanged");
		AddSelectedVerticesToStatus();
		AddSelectedEdgesToStatus();
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
        oNodeXLControl.DeselectAll();
    }

    private void
	chkShowToolTips_CheckedChanged
	(
		object sender,
		EventArgs e
	)
    {
        oNodeXLControl.ShowToolTips = chkShowToolTips.Checked;
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

            if ( !oNodeXLControl.Graph.Vertices.Find(iVertexID, out oVertex) )
            {
                throw new ArgumentException("No such ID.");
            }

            oNodeXLControl.SetVertexSelected(oVertex,
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

            if ( !oNodeXLControl.Graph.Edges.Find(iEdgeID, out oEdge) )
            {
                throw new ArgumentException("No such ID.");
            }

            oNodeXLControl.SetEdgeSelected(oEdge,
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
        oNodeXLControl.MouseSelectionMode =
			(MouseSelectionMode)cbxMouseSelectionMode.SelectedValue;
    }

    private void
	btnHideSelected_Click
	(
		object sender,
		EventArgs e
	)
    {
        foreach (IVertex oSelectedVertex in oNodeXLControl.SelectedVertices)
        {
            oSelectedVertex.SetValue(ReservedMetadataKeys.Visibility,
				VisibilityKeyValue.Hidden);
        }

        foreach (IEdge oSelectedEdge in oNodeXLControl.SelectedEdges)
        {
            oSelectedEdge.SetValue(ReservedMetadataKeys.Visibility,
				VisibilityKeyValue.Hidden);
        }

		oNodeXLControl.ForceRedraw();
    }
}

}
