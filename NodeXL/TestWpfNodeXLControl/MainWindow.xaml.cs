

//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows;
using Microsoft.NodeXL.Core;

namespace TestWpfNodeXLControl
{
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
        PopulateGraph();
	}

	protected void
	PopulateGraph()
	{
		oNodeXLControl.BeginUpdate();

		IVertexCollection oVertices = oNodeXLControl.Graph.Vertices;
		IEdgeCollection oEdges = oNodeXLControl.Graph.Edges;
		Double dWidth = this.Width;
		Double dHeight = this.Height;
		Random oRandom = new Random();

		Int32 Vertices = 20;

		IVertex oFirstVertex = oVertices.Add();

		for (Int32 i = 1; i < Vertices; i++)
		{
			IVertex oVertex = oVertices.Add();

			oVertex.Location = new System.Drawing.PointF(
				(Single)( dWidth * oRandom.NextDouble() ),
				(Single)( dHeight * oRandom.NextDouble() )
				);

			oEdges.Add(oFirstVertex, oVertex);
		}

		oNodeXLControl.EndUpdate();
	}
}
}
