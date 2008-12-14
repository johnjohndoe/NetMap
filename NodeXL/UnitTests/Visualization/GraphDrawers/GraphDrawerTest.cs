
//	Copyright (c) Microsoft Corporation.  All rights reserved.


// If SAVE_TEST_IMAGES is defined, running the unit tests creates a set of
// image files and saves them to a directory specified by the
// TestImageRelativeDirectory constant defined below.  If SAVE_TEST_IMAGES is
// not defined, running the unit tests creates a set of in-memory bimaps and
// compares them to the saved image files.
//
// SAVE_TEST_IMAGES should be defined when unit tests are being written.  It
// should not be defined when unit tests are being run.

// #define SAVE_TEST_IMAGES


using System;
using System.Drawing;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Visualization;
using Microsoft.NodeXL.Common;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: GraphDrawerTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="GraphDrawer" />
/// class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class GraphDrawerTest : Object
{
    //*************************************************************************
    //  Constructor: GraphDrawerTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphDrawerTest" /> class.
    /// </summary>
    //*************************************************************************

    public GraphDrawerTest()
    {
        m_oGraphDrawer = null;
		m_oBitmap = null;
		m_bRedrawRequired = false;
		m_bLayoutRequired = false;
    }

    //*************************************************************************
    //  Method: SetUp()
    //
    /// <summary>
    /// Gets run before each test.
    /// </summary>
    //*************************************************************************

    [TestInitializeAttribute]

    public void
    SetUp()
    {
        m_oGraphDrawer = new GraphDrawer();

		m_oGraphDrawer.Layout = new GridLayout();

		m_oGraphDrawer.VertexDrawer = new VertexDrawer();

		m_oGraphDrawer.RedrawRequired +=
			new EventHandler(this.GraphDrawer_RedrawRequired);

		m_oGraphDrawer.LayoutRequired +=
			new EventHandler(this.GraphDrawer_LayoutRequired);

		m_oBitmap = new Bitmap(BitmapWidth, BitmapHeight);

		m_bRedrawRequired = false;
		m_bLayoutRequired = false;
    }

    //*************************************************************************
    //  Method: TearDown()
    //
    /// <summary>
    /// Gets run after each test.
    /// </summary>
    //*************************************************************************

    [TestCleanupAttribute]

    public void
    TearDown()
    {
        m_oGraphDrawer = null;

        if (m_oBitmap != null)
		{
			m_oBitmap.Dispose();
			m_oBitmap = null;
		}
    }

    //*************************************************************************
    //  Method: TestConstructor()
    //
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestConstructor()
    {
		Assert.AreEqual(SystemColors.Window, m_oGraphDrawer.BackColor);

		Assert.IsInstanceOfType( m_oGraphDrawer.EdgeDrawer,
			typeof(EdgeDrawer) );

		Assert.IsInstanceOfType( m_oGraphDrawer.Graph, typeof(Graph) );

		Assert.AreEqual(0, m_oGraphDrawer.Graph.Vertices.Count);

		Assert.AreEqual(0, m_oGraphDrawer.Graph.Edges.Count);

		Assert.IsInstanceOfType( m_oGraphDrawer.Layout, typeof(GridLayout) );

		Assert.IsInstanceOfType( m_oGraphDrawer.VertexDrawer,
			typeof(VertexDrawer) );
    }

    //*************************************************************************
    //  Method: TestBackColor()
    //
    /// <summary>
    /// Tests the BackColor property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestBackColor()
    {
        Color TestValue = Color.Orange;

        m_oGraphDrawer.BackColor = TestValue;
        Assert.AreEqual(TestValue, m_oGraphDrawer.BackColor);

		Assert.IsTrue(m_bRedrawRequired);
		Assert.IsFalse(m_bLayoutRequired);

		m_bRedrawRequired = false;

        m_oGraphDrawer.BackColor = TestValue;

		Assert.IsFalse(m_bRedrawRequired);
		Assert.IsFalse(m_bLayoutRequired);
    }

    //*************************************************************************
    //  Method: TestEdgeDrawer()
    //
    /// <summary>
    /// Tests the EdgeDrawer property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestEdgeDrawer()
    {
		IEdgeDrawer oEdgeDrawer = new EdgeDrawer();

		m_oGraphDrawer.EdgeDrawer = oEdgeDrawer;
        Assert.AreEqual(oEdgeDrawer, m_oGraphDrawer.EdgeDrawer);

		Assert.IsTrue(m_bRedrawRequired);
		Assert.IsFalse(m_bLayoutRequired);

		m_bRedrawRequired = false;

		m_oGraphDrawer.EdgeDrawer = oEdgeDrawer;

		Assert.IsFalse(m_bRedrawRequired);
		Assert.IsFalse(m_bLayoutRequired);
    }

    //*************************************************************************
    //  Method: TestEdgeDrawerRedrawRequired()
    //
    /// <summary>
    /// Tests that the GraphDrawer fires RedrawRequired when one of the
	/// properties is changed on the EdgeDrawer object.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestEdgeDrawerRedrawRequired()
    {
		Assert.IsInstanceOfType(
			m_oGraphDrawer.EdgeDrawer, typeof(EdgeDrawer) );

		EdgeDrawer oEdgeDrawer = (EdgeDrawer)m_oGraphDrawer.EdgeDrawer;

		oEdgeDrawer.Color = Color.Orange;

		Assert.IsTrue(m_bRedrawRequired);
		Assert.IsFalse(m_bLayoutRequired);

		m_bRedrawRequired = false;

		oEdgeDrawer.SelectedColor = Color.LightBlue;

		Assert.IsTrue(m_bRedrawRequired);
		Assert.IsFalse(m_bLayoutRequired);

		m_bRedrawRequired = false;

		oEdgeDrawer.Width = EdgeDrawer.MaximumWidth;

		Assert.IsTrue(m_bRedrawRequired);
		Assert.IsFalse(m_bLayoutRequired);
    }

    //*************************************************************************
    //  Method: TestEdgeDrawerBad()
    //
    /// <summary>
    /// Tests the EdgeDrawer property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestEdgeDrawerBad()
    {
		// null value.

		try
		{
			m_oGraphDrawer.EdgeDrawer = null;
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual(

				m_oGraphDrawer.GetType().FullName +
					".EdgeDrawer: Can't be null."
				,
				oApplicationException.Message
				);

			throw oApplicationException;
		}
    }

    //*************************************************************************
    //  Method: TestGraph()
    //
    /// <summary>
    /// Tests the Graph property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraph()
    {
		IGraph oGraph = new Graph();

		m_oGraphDrawer.Graph = oGraph;
        Assert.AreEqual(oGraph, m_oGraphDrawer.Graph);

		Assert.IsTrue(m_bRedrawRequired);
		Assert.IsFalse(m_bLayoutRequired);

		m_bRedrawRequired = false;

		m_oGraphDrawer.Graph = oGraph;

		Assert.IsFalse(m_bRedrawRequired);
		Assert.IsFalse(m_bLayoutRequired);
    }

    //*************************************************************************
    //  Method: TestGraphRedrawRequired()
    //
    /// <summary>
    /// Tests that the GraphDrawer fires RedrawRequired when one of the
	/// properties is changed on the Graph object.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraphRedrawRequired()
    {
		m_oGraphDrawer.BackColor = Color.Orange;

		Assert.IsTrue(m_bRedrawRequired);
		Assert.IsFalse(m_bLayoutRequired);

		m_bRedrawRequired = false;

		m_oGraphDrawer.BackColor = Color.Orange;

		Assert.IsFalse(m_bRedrawRequired);
		Assert.IsFalse(m_bLayoutRequired);
    }

    //*************************************************************************
    //  Method: TestGraphLayoutRequired()
    //
    /// <summary>
    /// Tests that the GraphDrawer fires LayoutRequired when one of the
	/// properties is changed on the Graph object.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraphLayoutRequired()
    {
		IVertex oVertex = m_oGraphDrawer.Graph.Vertices.Add();

		Assert.IsFalse(m_bRedrawRequired);
		Assert.IsTrue(m_bLayoutRequired);

		m_bRedrawRequired = false;

		m_oGraphDrawer.Graph.Vertices.Remove(oVertex);

		Assert.IsFalse(m_bRedrawRequired);
		Assert.IsTrue(m_bLayoutRequired);

		IVertex oVertex1 = m_oGraphDrawer.Graph.Vertices.Add();
		IVertex oVertex2 = m_oGraphDrawer.Graph.Vertices.Add();

		m_bRedrawRequired = false;

		IEdge oEdge = m_oGraphDrawer.Graph.Edges.Add(oVertex1, oVertex2);

		Assert.IsFalse(m_bRedrawRequired);
		Assert.IsTrue(m_bLayoutRequired);

		m_bRedrawRequired = false;

		m_oGraphDrawer.Graph.Edges.Remove(oEdge);

		Assert.IsFalse(m_bRedrawRequired);
		Assert.IsTrue(m_bLayoutRequired);
    }

    //*************************************************************************
    //  Method: TestGraphBad()
    //
    /// <summary>
    /// Tests the Graph property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestGraphBad()
    {
		// null value.

		try
		{
			m_oGraphDrawer.Graph = null;
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual(

				m_oGraphDrawer.GetType().FullName
				+ ".Graph: Can't be null."
				,
				oApplicationException.Message
				);

			throw oApplicationException;
		}
    }

    //*************************************************************************
    //  Method: TestGraphBad2()
    //
    /// <summary>
    /// Tests the Graph property.
    /// </summary>
    //*************************************************************************

	#if false  // This isn't implemented.

    [TestMethodAttribute]
	[ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestGraphBad2()
    {
		// Graph already connected to another GraphDrawer.

		try
		{
			m_oGraphDrawer.Graph = ( new GraphDrawer() ).Graph;
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual(

				m_oGraphDrawer.GetType().FullName
				+ ".Graph: The specified graph is already being used by"
				+ " another graph drawer.  If you want to simultaneously draw"
				+ " the same graph with two different graph drawers, make a"
				+ " copy of the graph using IGraph.Clone()."
				,
				oApplicationException.Message
				);

			throw oApplicationException;
		}
    }

	#endif

    //*************************************************************************
    //  Method: TestLayout()
    //
    /// <summary>
    /// Tests the Layout property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLayout()
    {
		ILayout oLayout = new SugiyamaLayout();

		m_oGraphDrawer.Layout = oLayout;
        Assert.AreEqual(oLayout, m_oGraphDrawer.Layout);

		Assert.IsFalse(m_bRedrawRequired);
		Assert.IsTrue(m_bLayoutRequired);

		m_bLayoutRequired = false;

		m_oGraphDrawer.Layout = oLayout;

		Assert.IsFalse(m_bRedrawRequired);
		Assert.IsFalse(m_bLayoutRequired);
    }

    //*************************************************************************
    //  Method: TestLayoutBad()
    //
    /// <summary>
    /// Tests the Layout property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestLayoutBad()
    {
		// null value.

		try
		{
			m_oGraphDrawer.Layout = null;
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual(

				m_oGraphDrawer.GetType().FullName
				+ ".Layout: Can't be null."
				,
				oApplicationException.Message
				);

			throw oApplicationException;
		}
    }

    //*************************************************************************
    //  Method: TestVertexDrawer()
    //
    /// <summary>
    /// Tests the VertexDrawer property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestVertexDrawer()
    {
		IVertexDrawer oVertexDrawer = new VertexDrawer();

		m_oGraphDrawer.VertexDrawer = oVertexDrawer;
        Assert.AreEqual(oVertexDrawer, m_oGraphDrawer.VertexDrawer);

		Assert.IsTrue(m_bRedrawRequired);
		Assert.IsFalse(m_bLayoutRequired);

		m_bRedrawRequired = false;

		m_oGraphDrawer.VertexDrawer = oVertexDrawer;

		Assert.IsFalse(m_bRedrawRequired);
		Assert.IsFalse(m_bLayoutRequired);
    }

    //*************************************************************************
    //  Method: TestVertexDrawerRedrawRequired()
    //
    /// <summary>
    /// Tests that the GraphDrawer fires RedrawRequired when one of the
	/// properties is changed on the VertexDrawer object.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestVertexDrawerRedrawRequired()
    {
		Assert.IsInstanceOfType(
			m_oGraphDrawer.VertexDrawer, typeof(VertexDrawer) );

		VertexDrawer oVertexDrawer = (VertexDrawer)m_oGraphDrawer.VertexDrawer;

		oVertexDrawer.Color = Color.Orange;

		Assert.IsTrue(m_bRedrawRequired);
		Assert.IsFalse(m_bLayoutRequired);

		m_bRedrawRequired = false;

		oVertexDrawer.SelectedColor = Color.LightBlue;

		Assert.IsTrue(m_bRedrawRequired);
		Assert.IsFalse(m_bLayoutRequired);

		m_bRedrawRequired = false;

		oVertexDrawer.Shape = VertexDrawer.VertexShape.Sphere;

		Assert.IsTrue(m_bRedrawRequired);
		Assert.IsFalse(m_bLayoutRequired);
    }

    //*************************************************************************
    //  Method: TestVertexDrawerLayoutRequired()
    //
    /// <summary>
    /// Tests that the GraphDrawer fires LayoutRequired when one of the
	/// properties is changed on the VertexDrawer object.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestVertexDrawerLayoutRequired()
    {
		Assert.IsInstanceOfType(
			m_oGraphDrawer.VertexDrawer, typeof(VertexDrawer) );

		VertexDrawer oVertexDrawer = (VertexDrawer)m_oGraphDrawer.VertexDrawer;

		oVertexDrawer.Radius = VertexDrawer.MaximumRadius;

		Assert.IsFalse(m_bRedrawRequired);
		Assert.IsTrue(m_bLayoutRequired);
    }

    //*************************************************************************
    //  Method: TestVertexDrawerBad()
    //
    /// <summary>
    /// Tests the VertexDrawer property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestVertexDrawerBad()
    {
		// null value.

		try
		{
			m_oGraphDrawer.VertexDrawer = null;
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual(

				m_oGraphDrawer.GetType().FullName
				+ ".VertexDrawer: Can't be null."
				,
				oApplicationException.Message
				);

			throw oApplicationException;
		}
    }

    //*************************************************************************
    //  Method: TestDraw()
    //
    /// <summary>
    /// Tests the Draw(Bitmap) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestDraw()
    {
		// Complete graph.

		IVertex [] aoVertices;
		IEdge [] aoEdges;

		const Int32 Vertices = 50;

		PopulateGraph(Vertices, out aoVertices, out aoEdges);

		m_oGraphDrawer.Draw(m_oBitmap);

		const String TestImageFileNameNoExtension = "TestDraw";

		SaveOrCompareTestImage(m_oBitmap, TestImageFileNameNoExtension);
    }

    //*************************************************************************
    //  Method: TestDraw2()
    //
    /// <summary>
    /// Tests the Draw(Bitmap) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestDraw2()
    {
		// Empty graph.

		m_oGraphDrawer.Draw(m_oBitmap);

		const String TestImageFileNameNoExtension = "TestDraw2";

		SaveOrCompareTestImage(m_oBitmap, TestImageFileNameNoExtension);
    }

    //*************************************************************************
    //  Method: MeasureDraw_()
    //
    /// <summary>
	/// Measures the time required to draw a large graph.
    /// </summary>
	///
	/// <remarks>
	/// A crude, simple timer is used.  The results are written to the trace
	/// output.
	/// </remarks>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    MeasureDraw_()
    {
		const Int32 Vertices = 100000;

		IGraph oGraph = m_oGraphDrawer.Graph;

		IVertex [] aoVertices = GraphUtil.AddVertices(oGraph, Vertices);

		// Connect the first vertex to all other vertices.

		IEdgeCollection oEdges = oGraph.Edges;

		for (Int32 i = 1; i < Vertices; i++)
		{
			oEdges.Add( aoVertices[0], aoVertices[i] );
		}

		DateTime oStartTime = DateTime.Now;

		m_oGraphDrawer.Draw(m_oBitmap);

		DateTime oEndTime = DateTime.Now;

		const String TestImageFileNameNoExtension = "MeasureDraw_";

		SaveOrCompareTestImage(m_oBitmap, TestImageFileNameNoExtension);

		// Write the results.

		TimeSpan oElapsedTime = oEndTime - oStartTime;

		Double dElapsedMs = oElapsedTime.TotalMilliseconds;

		Double dMsPerVertex = dElapsedMs / (Double)Vertices;

		Trace.WriteLine( String.Format(

			"GraphDrawer.MeastureDraw_: Time spent drawing {0}"
			+ " vertices and edges: {1} ms, or {2} ms per vertex/edge."
			,
			Vertices,
			dElapsedMs,
			dMsPerVertex
			) );

		// Test results on 1/9/2007: 0.346898816 ms per vertex.
    }

    //*************************************************************************
    //  Method: TestDrawBad()
    //
    /// <summary>
    /// Tests the Draw(Bitmap) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestDrawBad()
    {
		// null bitmap.

		try
		{
			m_oGraphDrawer.Draw(null);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				m_oGraphDrawer.GetType().FullName
				+ ".Draw: bitmap argument can't be null.\r\n"
				+ "Parameter name: bitmap"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestDraw2_()
    //
    /// <summary>
    /// Tests the Draw(Bitmap, Rectangle) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestDraw2_()
    {
		// Complete graph, full rectangle.

		IVertex [] aoVertices;
		IEdge [] aoEdges;

		const Int32 Vertices = 33;

		PopulateGraph(Vertices, out aoVertices, out aoEdges);

		Rectangle oRectangle = GetBitmapRectangle();

		FillBitmap(Color.Red);

		m_oGraphDrawer.Draw(m_oBitmap, oRectangle);

		const String TestImageFileNameNoExtension = "TestDraw2_";

		SaveOrCompareTestImage(m_oBitmap, TestImageFileNameNoExtension);
    }

    //*************************************************************************
    //  Method: TestDraw2_2()
    //
    /// <summary>
    /// Tests the Draw(Bitmap, Rectangle) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestDraw2_2()
    {
		// Complete graph, deflated rectangle.

		IVertex [] aoVertices;
		IEdge [] aoEdges;

		const Int32 Vertices = 38;

		PopulateGraph(Vertices, out aoVertices, out aoEdges);

		Int32 iWidth = m_oBitmap.Width;
		Int32 iHeight = m_oBitmap.Height;

		Rectangle oRectangle = GetBitmapRectangle();

		oRectangle.Inflate(-iWidth / 4, -iHeight / 4);

		FillBitmap(Color.Blue);

		m_oGraphDrawer.Draw(m_oBitmap, oRectangle);

		const String TestImageFileNameNoExtension = "TestDraw2_2";

		SaveOrCompareTestImage(m_oBitmap, TestImageFileNameNoExtension);
    }

    //*************************************************************************
    //  Method: TestDraw2_3()
    //
    /// <summary>
    /// Tests the Draw(Bitmap, Rectangle) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestDraw2_3()
    {
		// Complete graph, shifted rectangle.

		IVertex [] aoVertices;
		IEdge [] aoEdges;

		const Int32 Vertices = 18;

		PopulateGraph(Vertices, out aoVertices, out aoEdges);

		Rectangle oRectangle = GetBitmapRectangle();

		oRectangle.Offset(-20, -40);

		FillBitmap(Color.Green);

		m_oGraphDrawer.Draw(m_oBitmap, oRectangle);

		const String TestImageFileNameNoExtension = "TestDraw2_3";

		SaveOrCompareTestImage(m_oBitmap, TestImageFileNameNoExtension);
    }

    //*************************************************************************
    //  Method: TestDraw2_Bad()
    //
    /// <summary>
    /// Tests the Draw(Bitmap, Rectangle) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestDraw2_Bad()
    {
		// null bitmap.

		try
		{
			Bitmap oBitmap = null;

			m_oGraphDrawer.Draw(oBitmap, Rectangle.Empty);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				m_oGraphDrawer.GetType().FullName
				+ ".Draw: bitmap argument can't be null.\r\n"
				+ "Parameter name: bitmap"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestDraw3_()
    //
    /// <summary>
    /// Tests the Draw(Graphics, Rectangle) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestDraw3_()
    {
		// Complete graph, full rectangle.

		IVertex [] aoVertices;
		IEdge [] aoEdges;

		const Int32 Vertices = 39;

		PopulateGraph(Vertices, out aoVertices, out aoEdges);

		Rectangle oRectangle = GetBitmapRectangle();

		Graphics oGraphics = Graphics.FromImage(m_oBitmap);

		FillBitmap(Color.Red);

		m_oGraphDrawer.Draw(oGraphics, oRectangle);

		GraphicsUtil.DisposeGraphics(ref oGraphics);

		const String TestImageFileNameNoExtension = "TestDraw3_";

		SaveOrCompareTestImage(m_oBitmap, TestImageFileNameNoExtension);
    }

    //*************************************************************************
    //  Method: TestDraw3_2()
    //
    /// <summary>
    /// Tests the Draw(Graphics, Rectangle) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestDraw3_2()
    {
		// Complete graph, deflated rectangle.

		IVertex [] aoVertices;
		IEdge [] aoEdges;

		const Int32 Vertices = 45;

		PopulateGraph(Vertices, out aoVertices, out aoEdges);

		Rectangle oRectangle = GetBitmapRectangle();

		oRectangle.Inflate(-oRectangle.Width / 5, -oRectangle.Height / 5);

		Graphics oGraphics = Graphics.FromImage(m_oBitmap);

		FillBitmap(Color.Orange);

		m_oGraphDrawer.Draw(oGraphics, oRectangle);

		GraphicsUtil.DisposeGraphics(ref oGraphics);

		const String TestImageFileNameNoExtension = "TestDraw3_2";

		SaveOrCompareTestImage(m_oBitmap, TestImageFileNameNoExtension);
    }

    //*************************************************************************
    //  Method: TestDraw3_Bad()
    //
    /// <summary>
    /// Tests the Draw(Graphics, Rectangle) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestDraw3_Bad()
    {
		// null bitmap.

		try
		{
			Graphics oGraphics = null;

			m_oGraphDrawer.Draw(oGraphics, Rectangle.Empty);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				m_oGraphDrawer.GetType().FullName
				+ ".Draw: graphics argument can't be null.\r\n"
				+ "Parameter name: graphics"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestGetVertexFromPoint()
    //
    /// <summary>
    /// Tests the GetVertexFromPoint() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetVertexFromPoint()
    {
		// No vertices.

		const Int32 X = 0;
		const Int32 Y = 0;

		TestGetVertexFromPoint(X, Y, null);
    }

    //*************************************************************************
    //  Method: TestGetVertexFromPoint2()
    //
    /// <summary>
    /// Tests the GetVertexFromPoint() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetVertexFromPoint2()
    {
		// One vertex, point outside vertex.

		IVertex [] aoVertices;
		IEdge [] aoEdges;

		const Int32 Vertices = 1;

		PopulateGraph(Vertices, out aoVertices, out aoEdges);

		aoVertices[0].Location = new Point(100, 200);

		const Int32 X = 5;
		const Int32 Y = 7;

		TestGetVertexFromPoint(X, Y, null);
    }

    //*************************************************************************
    //  Method: TestGetVertexFromPoint3()
    //
    /// <summary>
    /// Tests the GetVertexFromPoint() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetVertexFromPoint3()
    {
		// One vertex, point within vertex.

		IVertex [] aoVertices;
		IEdge [] aoEdges;

		const Int32 Vertices = 1;

		PopulateGraph(Vertices, out aoVertices, out aoEdges);

		aoVertices[0].Location = new Point(100, 200);

		const Int32 X = 100;
		const Int32 Y = 200;

		TestGetVertexFromPoint( X, Y, aoVertices[0] );
    }

    //*************************************************************************
    //  Method: TestGetVertexFromPoint4()
    //
    /// <summary>
    /// Tests the GetVertexFromPoint() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetVertexFromPoint4()
    {
		// Vertices arranged in a grid.

		const Int32 GridWidth = 10;
		const Int32 GridHeight = 12;

		IVertex [] aoVertices;
		IEdge [] aoEdges;

		Int32 iVertices = GridWidth * GridHeight;

		PopulateGraph(iVertices, out aoVertices, out aoEdges);

		Int32 iVertex = 0;

		Int32 iX;
		Int32 iY;

		// Space the vertices far enough apart to avoid overlap.

		const Int32 NoOverlapFactor = 100;

		for (Int32 i = 0; i < GridWidth; i++)
		{
			for (Int32 j = 0; j < GridHeight; j++)
			{
				iX = i * NoOverlapFactor;

				iY = j * NoOverlapFactor;

				aoVertices[iVertex].Location = new Point(iX, iY);

				iVertex++;
			}
		}

		// Loop through points that fall at the center of the vertices.

		iVertex = 0;

		for (Int32 i = 0; i < GridWidth; i++)
		{
			for (Int32 j = 0; j < GridHeight; j++)
			{
				iX = i * NoOverlapFactor;

				iY = j * NoOverlapFactor;

				TestGetVertexFromPoint( iX, iY, aoVertices[iVertex] );

				iVertex++;
			}
		}

		// These points fall outside the vertices.

		iX = GridWidth * NoOverlapFactor;

		iY = GridHeight * NoOverlapFactor;

		TestGetVertexFromPoint(iX, iY, null);

		iX = -100;

		iY = -200;

		TestGetVertexFromPoint(iX, iY, null);
    }

    //*************************************************************************
    //  Method: TestGetVertexFromPoint5()
    //
    /// <summary>
    /// Tests the GetVertexFromPoint() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetVertexFromPoint5()
    {
		// Vertices on top of each other.

		const Int32 Vertices = 10;

		IVertex [] aoVertices;
		IEdge [] aoEdges;

		PopulateGraph(Vertices, out aoVertices, out aoEdges);

		Int32 X = 435;
		Int32 Y = 123;

		foreach (IVertex oVertex in aoVertices)
		{
			oVertex.Location = new Point(X, Y);
		}

		// The topmost vertex (the last one drawn) should be returned.

		TestGetVertexFromPoint( X, Y, aoVertices[Vertices - 1] );

		TestGetVertexFromPoint(0, 0, null);
    }

    //*************************************************************************
    //  Method: TestUsageScenario()
    //
    /// <summary>
    /// Tests a usage scenario.
    /// </summary>
	///
	/// <remarks>
	/// Most of the the code in this method is meant to be copied to the
	/// documentation for the GraphDrawer class.
	/// </remarks>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestUsageScenario()
    {
		// Everything between the lines of asterisks should be copied to the
		// documentation.


		//************************************************************

		// Create an object for drawing graphs.

		GraphDrawer oGraphDrawer = new GraphDrawer();

		// The object owns an empty graph.  Get the graph's vertex and edge
		// collections.

		IGraph oGraph = oGraphDrawer.Graph;
		IVertexCollection oVertices = oGraph.Vertices;
		IEdgeCollection oEdges = oGraph.Edges;

		// Create some vertices and add them to the graph.

		IVertex oVertex1 = oVertices.Add();
		IVertex oVertex2 = oVertices.Add();
		IVertex oVertex3 = oVertices.Add();

		// Add some metadata to the vertices.  You can use the Key property to
		// add a single piece of metadata, or the SetValue() method to add
		// arbitrary key/value pairs.

		oVertex1.Tag = "This is vertex 1.";
		oVertex2.Tag = "This is vertex 2.";
		oVertex3.Tag = "This is vertex 3.";

		// Connect some of the vertices with undirected edges.

		IEdge oEdge1 = oEdges.Add(oVertex1, oVertex2);
		IEdge oEdge2 = oEdges.Add(oVertex2, oVertex3);

		// Add some metadata to the edges.

		oEdge1.SetValue("Weight", 5);
		oEdge2.SetValue("Weight", 3);

		// Set the graph's colors.

		oGraphDrawer.BackColor = Color.DarkBlue;

		// The GraphDrawer's default vertex drawer is a VertexDrawer object.

		VertexDrawer oVertexDrawer = (VertexDrawer)oGraphDrawer.VertexDrawer;

		oVertexDrawer.Color = Color.White;

		// The GraphDrawer's default edge drawer is an EdgeDrawer object.

		EdgeDrawer oEdgeDrawer = (EdgeDrawer)oGraphDrawer.EdgeDrawer;

		oEdgeDrawer.Color = Color.White;

		// Create a bitmap and draw the graph onto it.

		Bitmap oBitmap = new Bitmap(400, 200);

		oGraphDrawer.Draw(oBitmap);

		// Do something with the bitmap...

		//************************************************************


		const String TestImageFileNameNoExtension = "TestUsageScenario";

		SaveOrCompareTestImage(oBitmap, TestImageFileNameNoExtension);
    }

    //*************************************************************************
    //  Method: TestGetVertexFromPoint()
    //
    /// <summary>
    /// Tests the GetVertexFromPoint() methods.
    /// </summary>
	///
	/// <param name="iX">
	/// x-coordinate of point.
	/// </param>
	///
	/// <param name="iY">
	/// y-coordinate of point.
	/// </param>
	///
	/// <param name="oExpectedVertex">
	/// Vertex expected at point, or null if no vertex is expected.
	/// </param>
    //*************************************************************************

    protected void
    TestGetVertexFromPoint
	(
		Int32 iX,
		Int32 iY,
		IVertex oExpectedVertex
	)
    {
		IVertex oVertex = null;

        m_oGraphDrawer.DrawCurrentLayout( Graphics.FromImage(m_oBitmap),
			new Rectangle(Point.Empty, m_oBitmap.Size) );

		Boolean bReturnValue = m_oGraphDrawer.GetVertexFromPoint(
			iX, iY, out oVertex);

		TestGetVertexFromPoint(bReturnValue, oExpectedVertex, oVertex);

		// Repeat for the other overload.

		oVertex = null;

		bReturnValue = m_oGraphDrawer.GetVertexFromPoint(
			new Point(iX, iY), out oVertex);

		TestGetVertexFromPoint(bReturnValue, oExpectedVertex, oVertex);
    }

    //*************************************************************************
    //  Method: TestGetVertexFromPoint()
    //
    /// <summary>
    /// Tests one of the GetVertexFromPoint() methods.
    /// </summary>
	///
	/// <param name="bActualReturnValue">
	/// Return value expected from GetVertexFromPoint().
	/// </param>
	///
	/// <param name="oExpectedVertex">
	/// Vertex expected from GetVertexFromPoint(), or null if a return value of
	/// false is expected.
	/// </param>
	///
	/// <param name="oActualVertex">
	/// Vertex output by GetVertexFromPoint().
	/// </param>
    //*************************************************************************

    protected void
    TestGetVertexFromPoint
	(
		Boolean bActualReturnValue,
		IVertex oExpectedVertex,
		IVertex oActualVertex
	)
    {
		Boolean bExpectedReturnValue = (oExpectedVertex != null);

		Assert.AreEqual(bExpectedReturnValue, bActualReturnValue);

		if (bExpectedReturnValue)
		{
			Assert.AreEqual(oExpectedVertex, oActualVertex);
		}
		else
		{
			Assert.IsNull(oActualVertex);
		}
    }

    //*************************************************************************
    //  Method: PopulateGraph()
    //
    /// <summary>
    /// Populates the graph owned by m_oGraphDrawer.
    /// </summary>
	///
	/// <param name="iVertices">
	/// Number of vertices to add to the graph.
	/// </param>
	///
	/// <param name="aoVertices">
	/// Where the graph's vertices get stored.
	/// </param>
	///
	/// <param name="aoEdges">
	/// Where the graph's edges get stored.
	/// </param>
    //*************************************************************************

    protected void
    PopulateGraph
	(
		Int32 iVertices,
		out IVertex [] aoVertices,
		out IEdge [] aoEdges
	)
    {
		IGraph oGraph = m_oGraphDrawer.Graph;

		aoVertices = GraphUtil.AddVertices(oGraph, iVertices);

		aoEdges = GraphUtil.MakeGraphComplete(oGraph, aoVertices, false);
    }

    //*************************************************************************
    //  Method: GetBitmapRectangle()
    //
    /// <summary>
    /// Returns a Rectangle that bounds m_oBitmap.
    /// </summary>
	///
    /// <returns>
    /// A Rectangle that bounds m_oBitmap.
    /// </returns>
    //*************************************************************************

    protected Rectangle
    GetBitmapRectangle()
    {
		return ( new Rectangle(0, 0, m_oBitmap.Width, m_oBitmap.Height) );
    }

    //*************************************************************************
    //  Method: FillBitmap()
    //
    /// <summary>
    /// Fills m_oBitmap with a specified color.
    /// </summary>
	///
	/// <param name="oColor">
	/// Fill color.
	/// </param>
    //*************************************************************************

    protected void
    FillBitmap
	(
		Color oColor
	)
    {
		Graphics oGraphics = Graphics.FromImage(m_oBitmap);

		oGraphics.Clear(oColor);

		GraphicsUtil.DisposeGraphics(ref oGraphics);
    }

    //*************************************************************************
    //  Method: SaveOrCompareTestImage()
    //
    /// <summary>
    /// Saves a test image file to disk or compares a bitmap to a test image
	/// file stored on disk.
    /// </summary>
	///
	/// <param name="oBitmap">
	/// Bitmap to save or compare.
	/// </param>
	///
	/// <param name="sTestImageFileNameNoExtension">
	/// Name of the test image file, without a path or extension.  Sample:
	/// "DrawVertex1".
	/// </param>
	///
	/// <remarks>
	/// If SAVE_TEST_IMAGES is defined, this method saves a test image file to
	/// disk.  Otherwise, it compares a bitmap to a test image file stored on
	/// disk.
	/// </remarks>
    //*************************************************************************

    protected void
    SaveOrCompareTestImage
	(
		Bitmap oBitmap,
		String sTestImageFileNameNoExtension
	)
    {
		Debug.Assert(oBitmap != null);
		Debug.Assert( !String.IsNullOrEmpty(sTestImageFileNameNoExtension) );

		#if SAVE_TEST_IMAGES

		TestImageUtil.SaveTestImage(oBitmap, TestImageRelativeDirectory,
			sTestImageFileNameNoExtension);

		#else

		TestImageUtil.CompareTestImage(oBitmap, TestImageRelativeDirectory,
			sTestImageFileNameNoExtension);

		#endif
    }

    //*************************************************************************
    //  Method: GraphDrawer_RedrawRequired()
    //
    /// <summary>
	/// Handles the RedrawRequired event on the m_oGraphDrawer object.
    /// </summary>
	///
	/// <param name="oSender">
	/// Standard event argument.
	/// </param>
	///
	/// <param name="oEventArgs">
	/// Standard event argument.
	/// </param>
    //*************************************************************************

	protected void
	GraphDrawer_RedrawRequired
	(
		Object oSender,
		EventArgs oEventArgs
	)
	{
		if ( oSender == null || !(oSender is GraphDrawer) )
		{
			throw new ApplicationException(
				"RedrawRequired event provided incorrect oSender argument."
				);
		}

		m_bRedrawRequired = true;
	}

    //*************************************************************************
    //  Method: GraphDrawer_LayoutRequired()
    //
    /// <summary>
	/// Handles the LayoutRequired event on the m_oGraphDrawer object.
    /// </summary>
	///
	/// <param name="oSender">
	/// Standard event argument.
	/// </param>
	///
	/// <param name="oEventArgs">
	/// Standard event argument.
	/// </param>
    //*************************************************************************

	protected void
	GraphDrawer_LayoutRequired
	(
		Object oSender,
		EventArgs oEventArgs
	)
	{
		if ( oSender == null || !(oSender is GraphDrawer) )
		{
			throw new ApplicationException(
				"LayoutRequired event provided incorrect oSender argument."
				);
		}

		m_bLayoutRequired = true;
	}


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	/// Some unit tests create in-memory bitmaps that are compared to saved
	/// image files.  The image files are stored in the directory specified
	/// by TestImageRelativeDirectory, which is relative to the NodeXL\UnitTests
	/// directory.

	protected const String TestImageRelativeDirectory =
		"Visualization\\GraphDrawers\\TestImageFiles\\GraphDrawer";

	/// Width of m_oBitmap.

	protected const Int32 BitmapWidth = 500;

	/// Height of m_oBitmap.

	protected const Int32 BitmapHeight = 500;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected GraphDrawer m_oGraphDrawer;

	/// Bitmap to draw onto.

	protected Bitmap m_oBitmap;

	/// Gets set by GraphDrawer_RedrawRequired().

	protected Boolean m_bRedrawRequired;

	/// Gets set by GraphDrawer_LayoutRequired().

	protected Boolean m_bLayoutRequired;
}

}
