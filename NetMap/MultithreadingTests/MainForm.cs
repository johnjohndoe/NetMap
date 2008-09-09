
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.NetMap.Core;
using Microsoft.NetMap.Visualization;
using Microsoft.NetMap.Tests;

namespace Microsoft.NetMap.MultithreadingTests
{
public partial class MainForm : Form
{
	public MainForm()
	{
		m_oAsyncLayout = CreateAsyncLayout();

		m_oAsyncGraphDrawer = CreateAsyncGraphDrawer();

		m_oMultiSelectionGraphDrawer = CreateMultiSelectionGraphDrawer();

		m_oResizeTimer = new Timer();

		m_oResizeTimer.Tick += new EventHandler(this.ResizeTimer_Tick);

		m_oResizeTimer.Interval = 50;

		m_bTestingMultiSelectionGraphDrawer = false;


		InitializeComponent();

		Trace.Listeners.Add(txbStatus.TraceListener);
	}

	protected IAsyncLayout
	CreateAsyncLayout()
	{
		IAsyncLayout oAsyncLayout = new FruchtermanReingoldLayout();

		oAsyncLayout.LayOutGraphIterationCompleted +=
			new EventHandler(this.AsyncLayout_LayOutGraphIterationCompleted);

		oAsyncLayout.LayOutGraphCompleted +=
			new AsyncCompletedEventHandler(
				this.AsyncLayout_LayOutGraphCompleted);

		return (oAsyncLayout);
	}

	protected AsyncGraphDrawer
	CreateAsyncGraphDrawer()
	{
		AsyncGraphDrawer oAsyncGraphDrawer = new AsyncGraphDrawer();

		oAsyncGraphDrawer.Layout = new FruchtermanReingoldLayout();

		IGraph oGraph = oAsyncGraphDrawer.Graph;

		oGraph.Vertices.Clear();

		const Int32 Vertices = 20;

		IVertex [] aoVertices = GraphUtil.AddVertices(oGraph, Vertices);

		GraphUtil.MakeGraphComplete(oGraph, aoVertices, false);

		oAsyncGraphDrawer.DrawAsyncIterationCompleted +=
			new EventHandler(
				this.AsyncGraphDrawer_DrawAsyncIterationCompleted);

		oAsyncGraphDrawer.DrawAsyncCompleted +=
			new AsyncCompletedEventHandler(
				this.AsyncGraphDrawer_DrawAsyncCompleted);

		return (oAsyncGraphDrawer);
	}

	protected MultiSelectionGraphDrawer
	CreateMultiSelectionGraphDrawer()
	{
		MultiSelectionGraphDrawer oMultiSelectionGraphDrawer =
			new MultiSelectionGraphDrawer();

		oMultiSelectionGraphDrawer.Layout = new FruchtermanReingoldLayout();

		IGraph oGraph = oMultiSelectionGraphDrawer.Graph;

		oGraph.Vertices.Clear();

		const Int32 Vertices = 20;

		IVertex [] aoVertices = GraphUtil.AddVertices(oGraph, Vertices);

		IEdge [] aoEdges = GraphUtil.MakeGraphComplete(
			oGraph, aoVertices, false);

		Int32 iEdges = aoEdges.Length;

		for (Int32 i = 0; i < Vertices; i++)
		{
			if (i % 2 == 0)
			{
				MultiSelectionGraphDrawer.SelectVertex( aoVertices[i] );
			}
		}

		for (Int32 i = 0; i < iEdges; i++)
		{
			if (i % 2 == 0)
			{
				MultiSelectionGraphDrawer.SelectEdge( aoEdges[i] );
			}
		}

		oMultiSelectionGraphDrawer.DrawAsyncIterationCompleted +=
			new EventHandler(this.AsyncGraphDrawer_DrawAsyncIterationCompleted);

		oMultiSelectionGraphDrawer.DrawAsyncCompleted +=
			new AsyncCompletedEventHandler(
				this.AsyncGraphDrawer_DrawAsyncCompleted);

		return (oMultiSelectionGraphDrawer);
	}

    protected void
	AsyncGraphDrawerDrawAsync()
    {
		Size oBitmapSize = picGraph.ClientSize;

		Int32 iWidth = oBitmapSize.Width;
		Int32 iHeight = oBitmapSize.Height;

		if (iWidth == 0 || iHeight == 0)
		{
			return;
		}

		Bitmap oBitmap = new Bitmap(iWidth, iHeight);

		m_oAsyncGraphDrawer.DrawAsync(oBitmap);

		picGraph.Image = oBitmap;
    }

    protected void
	MultiSelectionGraphDrawerDrawAsync()
    {
		Size oBitmapSize = picGraph.ClientSize;

		Int32 iWidth = oBitmapSize.Width;
		Int32 iHeight = oBitmapSize.Height;

		if (iWidth == 0 || iHeight == 0)
		{
			return;
		}

		Bitmap oBitmap = new Bitmap(iWidth, iHeight);

		m_oMultiSelectionGraphDrawer.DrawAsync(
			oBitmap, chkDrawSelection.Checked);

		picGraph.Image = oBitmap;
    }

	protected void
	WriteToStatus
	(
		String sMessage
	)
	{
		Trace.WriteLine(DateTime.Now.ToLongTimeString() + ": " + sMessage);
	}

	protected void
	btnLayOutGraphAsync_Click
	(
		object sender,
		EventArgs e
	)
	{
		IGraph oGraph = new Graph();

		const Int32 Vertices = 5;

		IVertex [] aoVertices = GraphUtil.AddVertices(oGraph, Vertices);

		GraphUtil.MakeGraphComplete(oGraph, aoVertices, false);

		Rectangle oRectangle =
			new Rectangle( Point.Empty, new Size(300, 200) );

        LayoutContext oLayoutContext =
            new LayoutContext(oRectangle, m_oAsyncGraphDrawer);

		m_oAsyncLayout.LayOutGraphAsync(oGraph, oLayoutContext);
	}

    protected void
	btnLayOutGraphAsyncCancel_Click
	(
		object sender,
		EventArgs e
	)
    {
		m_oAsyncLayout.LayOutGraphAsyncCancel();
    }

    protected void
	btnAsyncGraphDrawerDrawAsync_Click
	(
		object sender,
		EventArgs e
	)
    {
		m_bTestingMultiSelectionGraphDrawer = false;

		AsyncGraphDrawerDrawAsync();
    }

    protected void
	btnAsyncGraphDrawerDrawAsyncCancel_Click
	(
		object sender,
		EventArgs e
	)
    {
		m_oAsyncGraphDrawer.DrawAsyncCancel();
    }

    protected void
	bntMultiSelectionGraphDrawerDrawAsync_Click
	(
		object sender,
		EventArgs e
	)
    {
		m_bTestingMultiSelectionGraphDrawer = true;

		MultiSelectionGraphDrawerDrawAsync();
    }

    protected void
	btnMultiSelectionGraphDrawerDrawAsyncCancel_Click
	(
		object sender,
		EventArgs e
	)
    {
		m_oMultiSelectionGraphDrawer.DrawAsyncCancel();
    }

	protected void
	AsyncLayout_LayOutGraphIterationCompleted
	(
		Object oSender,
		EventArgs oEventArgs
	)
	{
		WriteToStatus("LayOutGraphIterationCompleted");

		Debug.Assert(!this.InvokeRequired);

		#if false
		WriteToStatus("LayOutGraphIterationCompleted, about to sleep.");

		System.Threading.Thread.Sleep(2000);

		WriteToStatus("LayOutGraphIterationCompleted, woke up from sleep.");
		#endif
	}

	protected void
	AsyncLayout_LayOutGraphCompleted
	(
		Object oSender,
		AsyncCompletedEventArgs oAsyncCompletedEventArgs
	)
	{
		WriteToStatus("LayOutGraphCompleted");

        if (oAsyncCompletedEventArgs.Error != null)
        {
            WriteToStatus("Error: " + oAsyncCompletedEventArgs.Error.Message);
        }
        else if (oAsyncCompletedEventArgs.Cancelled)
        {
            WriteToStatus("Cancelled.");
        }
        else
        {
            WriteToStatus("Completed.");
        }
	}

	protected void
	AsyncGraphDrawer_DrawAsyncIterationCompleted
	(
		Object oSender,
		EventArgs oEventArgs
	)
	{
		WriteToStatus("DrawAsyncIterationCompleted");

		picGraph.Invalidate();

		#if false
		WriteToStatus("DrawAsyncIterationCompleted, about to sleep.");

		System.Threading.Thread.Sleep(2000);

		WriteToStatus("DrawAsyncIterationCompleted, woke up from sleep.");
		#endif
	}

	protected void
	AsyncGraphDrawer_DrawAsyncCompleted
	(
		Object oSender,
		AsyncCompletedEventArgs oAsyncCompletedEventArgs
	)
	{
		WriteToStatus("DrawAsyncCompleted");

        if (oAsyncCompletedEventArgs.Error != null)
        {
            WriteToStatus("Error: " + oAsyncCompletedEventArgs.Error.Message);
        }
        else if (oAsyncCompletedEventArgs.Cancelled)
        {
            WriteToStatus("Cancelled.");
        }
        else
        {
            WriteToStatus("Completed.");
        }

		picGraph.Invalidate();
	}

    protected void
	picGraph_Resize
	(
		object sender,
		EventArgs e
	)
    {
		m_oAsyncGraphDrawer.DrawAsyncCancel();
		m_oMultiSelectionGraphDrawer.DrawAsyncCancel();

		m_oResizeTimer.Enabled = true;
    }

    protected void
	ResizeTimer_Tick
	(
		object sender,
		EventArgs e
	)
    {
		if (m_bTestingMultiSelectionGraphDrawer)
		{
			if (!m_oMultiSelectionGraphDrawer.IsBusy)
			{
				Trace.WriteLine("Restarting draw.");

				m_oResizeTimer.Enabled = false;

				MultiSelectionGraphDrawerDrawAsync();
			}
		}
		else
		{
			if (!m_oAsyncGraphDrawer.IsBusy)
			{
				Trace.WriteLine("Restarting draw.");

				m_oResizeTimer.Enabled = false;

				AsyncGraphDrawerDrawAsync();
			}
		}
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	protected IAsyncLayout m_oAsyncLayout;

	protected AsyncGraphDrawer m_oAsyncGraphDrawer;

	protected MultiSelectionGraphDrawer m_oMultiSelectionGraphDrawer;

	protected Timer m_oResizeTimer;

	protected Boolean m_bTestingMultiSelectionGraphDrawer;
}

}
