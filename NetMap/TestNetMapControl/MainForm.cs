
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using Microsoft.NetMap.Core;
using Microsoft.NetMap.Adapters;
using Microsoft.NetMap.Visualization;

namespace TestNetMapControl
{
public partial class MainForm : Form
{
	public MainForm()
	{
		InitializeComponent();

		chkShowToolTips.Checked = oNetMapControl.ShowToolTips;

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

		oNetMapControl.BeginUpdate();

		oNetMapControl.Layout = new FruchtermanReingoldLayout();

        ( (VertexDrawer)oNetMapControl.VertexDrawer ).Radius = 5;

		oNetMapControl.Graph = oGraphAdapter.LoadGraph(
			"..\\..\\SampleGraph.txt");

		foreach (IVertex oVertex in oNetMapControl.Graph.Vertices)
		{
			oVertex.SetValue(ReservedMetadataKeys.ToolTip, String.Format(

				"This is the tooltip for the vertex with ID {0}."
				,
				oVertex.ID.ToString()
				) );
		}

		oNetMapControl.EndUpdate();
	}

	protected void
	AddSelectedVerticesToStatus()
	{
		AddToStatus("IDs in SelectedVertices:");

		foreach (IVertex oVertex in oNetMapControl.SelectedVertices)
		{
			AddToStatus( oVertex.ID.ToString() );
		}
	}

	protected void
	AddSelectedEdgesToStatus()
	{
		AddToStatus("IDs in SelectedEdges:");

		foreach (IEdge oEdge in oNetMapControl.SelectedEdges)
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
	oNetMapControl_SelectionChanged
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
        oNetMapControl.DeselectAll();
    }

    private void
	chkShowToolTips_CheckedChanged
	(
		object sender,
		EventArgs e
	)
    {
        oNetMapControl.ShowToolTips = chkShowToolTips.Checked;
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

            if ( !oNetMapControl.Graph.Vertices.Find(iVertexID, out oVertex) )
            {
                throw new ArgumentException("No such ID.");
            }

            oNetMapControl.SetVertexSelected(oVertex,
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

            if ( !oNetMapControl.Graph.Edges.Find(iEdgeID, out oEdge) )
            {
                throw new ArgumentException("No such ID.");
            }

            oNetMapControl.SetEdgeSelected(oEdge,
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
        oNetMapControl.MouseSelectionMode =
			(MouseSelectionMode)cbxMouseSelectionMode.SelectedValue;
    }
}

}
