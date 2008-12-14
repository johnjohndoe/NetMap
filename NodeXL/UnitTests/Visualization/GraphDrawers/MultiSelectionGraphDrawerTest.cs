
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
//  Class: MultiSelectionGraphDrawerTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="MultiSelectionGraphDrawer" /> class.
/// </summary>
///
/// <remarks>
/// MultiSelectionGraphDrawer is derived from GraphDrawer.  Also,
/// MultiSelectionGraphDrawerTest derives from GraphDrawerTest.  This
/// allows MultiSelectionGraphDrawer to be tested as a GraphDrawer,
/// although Visual Studio's lack of support for derived classes within unit
/// tests makes this awkward.
///
/// <para>
/// There are two types of unit tests in this class: 1) those that test
/// MultiSelectionGraphDrawer as a GraphDrawer and call base-class test
/// methods to do their work; and 2) those that test functionality exclusive to
/// MultiSelectionGraphDrawer and don't use the base class.
/// </para>
///
/// </remarks>
//*****************************************************************************

[TestClassAttribute]

public class MultiSelectionGraphDrawerTest : GraphDrawerTest
{
    //*************************************************************************
    //  Constructor: MultiSelectionGraphDrawerTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="MultiSelectionGraphDrawerTest" /> class.
    /// </summary>
    //*************************************************************************

    public MultiSelectionGraphDrawerTest()
    {
		// Initialize the MultiSelectionGraphDrawer defined in this derived
		// class.

        m_oMultiSelectionGraphDrawer = null;
    }

    //*************************************************************************
    //  Method: DerivedSetUp()
    //
    /// <summary>
    /// Gets run before each test.
    /// </summary>
    //*************************************************************************

    public void
    DerivedSetUp()
    {
		// The TestInitializeAttribute attribute can't be added to a method in
		// a derived class, so it's not possible to have Visual Studio
		// automatically call an initialize method in a derived class before
		// each test.  Instead, this DerivedSetUp() method must be explicitly
		// called at the start of each test.

		// Replace the base-class GraphDrawer object with a
		// MultiSelectionGraphDrawer.  Base-class tests called from derived
		// tests will then be testing a MultiSelectionGraphDrawer instead of
		// the GraphDrawer that the base class thinks it's testing.

        m_oGraphDrawer = new MultiSelectionGraphDrawer();

		m_oGraphDrawer.Layout = new GridLayout();

		m_oGraphDrawer.VertexDrawer = new VertexDrawer();

		// For tests exclusive to this derived class, cast the base-class
		// object to the derived type.

        m_oMultiSelectionGraphDrawer =
			(MultiSelectionGraphDrawer)m_oGraphDrawer;

        ( (VertexDrawer)m_oMultiSelectionGraphDrawer.VertexDrawer ).
			SelectedColor = Color.Red;

        ( (EdgeDrawer)m_oMultiSelectionGraphDrawer.EdgeDrawer ).
			SelectedColor = Color.Orange;

		m_oMultiSelectionGraphDrawer.RedrawRequired +=
			new EventHandler(this.GraphDrawer_RedrawRequired);

		m_oMultiSelectionGraphDrawer.LayoutRequired +=
			new EventHandler(this.GraphDrawer_LayoutRequired);

        m_bRedrawRequired = false;
        m_bLayoutRequired = false;
    }

    //*************************************************************************
    //  Method: TestConstructor()
    //
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public new void
    TestConstructor()
    {
		DerivedSetUp();

		base.TestConstructor();
    }

    //*************************************************************************
    //  Method: TestBackColor()
    //
    /// <summary>
    /// Tests the BackColor property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public new void
    TestBackColor()
    {
		DerivedSetUp();

		base.TestBackColor();
    }

    //*************************************************************************
    //  Method: TestEdgeDrawer()
    //
    /// <summary>
    /// Tests the EdgeDrawer property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public new void
    TestEdgeDrawer()
    {
		DerivedSetUp();

		base.TestEdgeDrawer();
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

    public new void
    TestEdgeDrawerRedrawRequired()
    {
		DerivedSetUp();

		base.TestEdgeDrawerRedrawRequired();
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

    public new void
    TestEdgeDrawerBad()
    {
		DerivedSetUp();

		base.TestEdgeDrawerBad();
    }

    //*************************************************************************
    //  Method: TestGraph()
    //
    /// <summary>
    /// Tests the Graph property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public new void
    TestGraph()
    {
		DerivedSetUp();

		base.TestGraph();
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

    public new void
    TestGraphRedrawRequired()
    {
		DerivedSetUp();

		base.TestGraphRedrawRequired();
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

    public new void
    TestGraphBad()
    {
		DerivedSetUp();

		base.TestGraphBad();
    }

    //*************************************************************************
    //  Method: TestLayout()
    //
    /// <summary>
    /// Tests the Layout property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public new void
    TestLayout()
    {
		DerivedSetUp();

		base.TestLayout();
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

    public new void
    TestLayoutBad()
    {
		DerivedSetUp();

		base.TestLayoutBad();
    }

    //*************************************************************************
    //  Method: TestVertexDrawer()
    //
    /// <summary>
    /// Tests the VertexDrawer property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public new void
    TestVertexDrawer()
    {
		DerivedSetUp();

		base.TestVertexDrawer();
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

    public new void
    TestVertexDrawerRedrawRequired()
    {
		DerivedSetUp();

		base.TestVertexDrawerRedrawRequired();
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

    public new void
    TestVertexDrawerBad()
    {
		DerivedSetUp();

		base.TestVertexDrawerBad();
    }

    //*************************************************************************
    //  Method: TestDraw()
    //
    /// <summary>
    /// Tests the Draw(Bitmap) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public new void
    TestDraw()
    {
		DerivedSetUp();

		base.TestDraw();
    }

    //*************************************************************************
    //  Method: TestDraw2()
    //
    /// <summary>
    /// Tests the Draw(Bitmap) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public new void
    TestDraw2()
    {
		DerivedSetUp();

		base.TestDraw2();
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

    public new void
    TestDrawBad()
    {
		DerivedSetUp();

		base.TestDrawBad();
    }

    //*************************************************************************
    //  Method: TestDraw2_()
    //
    /// <summary>
    /// Tests the Draw(Bitmap, Rectangle) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public new void
    TestDraw2_()
    {
		DerivedSetUp();

		base.TestDraw2_();
    }

    //*************************************************************************
    //  Method: TestDraw2_2()
    //
    /// <summary>
    /// Tests the Draw(Bitmap, Rectangle) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public new void
    TestDraw2_2()
    {
		DerivedSetUp();

		base.TestDraw2_2();
    }

    //*************************************************************************
    //  Method: TestDraw2_3()
    //
    /// <summary>
    /// Tests the Draw(Bitmap, Rectangle) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public new void
    TestDraw2_3()
    {
		DerivedSetUp();

		base.TestDraw2_3();
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

    public new void
    TestDraw2_Bad()
    {
		DerivedSetUp();

		base.TestDraw2_Bad();
    }

    //*************************************************************************
    //  Method: TestDraw3_()
    //
    /// <summary>
    /// Tests the Draw(Graphics, Rectangle) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public new void
    TestDraw3_()
    {
		DerivedSetUp();

		base.TestDraw3_();
    }

    //*************************************************************************
    //  Method: TestDraw3_2()
    //
    /// <summary>
    /// Tests the Draw(Graphics, Rectangle) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public new void
    TestDraw3_2()
    {
		DerivedSetUp();

		base.TestDraw3_2();
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

    public new void
    TestDraw3_Bad()
    {
		DerivedSetUp();

		base.TestDraw3_Bad();
    }

    //*************************************************************************
    //  Method: TestGetVertexFromPoint()
    //
    /// <summary>
    /// Tests the GetVertexFromPoint() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public new void
    TestGetVertexFromPoint()
    {
		DerivedSetUp();

		base.TestGetVertexFromPoint();
    }

    //*************************************************************************
    //  Method: TestGetVertexFromPoint2()
    //
    /// <summary>
    /// Tests the GetVertexFromPoint() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public new void
    TestGetVertexFromPoint2()
    {
		DerivedSetUp();

		base.TestGetVertexFromPoint2();
    }

    //*************************************************************************
    //  Method: TestGetVertexFromPoint3()
    //
    /// <summary>
    /// Tests the GetVertexFromPoint() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public new void
    TestGetVertexFromPoint3()
    {
		DerivedSetUp();

		base.TestGetVertexFromPoint3();
    }

    //*************************************************************************
    //  Method: TestGetVertexFromPoint4()
    //
    /// <summary>
    /// Tests the GetVertexFromPoint() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public new void
    TestGetVertexFromPoint4()
    {
		DerivedSetUp();

		base.TestGetVertexFromPoint4();
    }

    //*************************************************************************
    //  Method: TestGetVertexFromPoint5()
    //
    /// <summary>
    /// Tests the GetVertexFromPoint() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public new void
    TestGetVertexFromPoint5()
    {
		DerivedSetUp();

		base.TestGetVertexFromPoint5();
    }

    //*************************************************************************
    //  Method: TestUsageScenario()
    //
    /// <summary>
    /// Tests a usage scenario.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public new void
    TestUsageScenario()
    {
		DerivedSetUp();

		base.TestUsageScenario();
    }

    //*************************************************************************
    //  Method: TestDraw4_()
    //
    /// <summary>
    /// Tests the Draw(Bitmap, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestDraw4_()
    {
		// Select a vertex and edge, draw with and without selection.

		foreach (Boolean bDrawSelection in GraphUtil.AllBoolean)
		{
		DerivedSetUp();

		IVertex [] aoVertices;
		IEdge [] aoEdges;

		const Int32 Vertices = 3;

		PopulateGraph(Vertices, out aoVertices, out aoEdges);

		m_oMultiSelectionGraphDrawer.Draw(m_oBitmap, false);

		MultiSelectionGraphDrawer.SelectVertex( aoVertices[0] );
		MultiSelectionGraphDrawer.SelectEdge( aoEdges[0] );

		m_oMultiSelectionGraphDrawer.Draw(m_oBitmap, bDrawSelection);

		String sTestImageFileNameNoExtension =
			"TestDraw4" + "-" + bDrawSelection.ToString();

		SaveOrCompareTestImage(m_oBitmap, sTestImageFileNameNoExtension);
		}
    }

    //*************************************************************************
    //  Method: TestDraw4_Bad()
    //
    /// <summary>
    /// Tests the Draw(Bitmap, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestDraw4_Bad()
    {
		// null bitmap.

		DerivedSetUp();

		try
		{
			m_oMultiSelectionGraphDrawer.Draw(null, true);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				m_oMultiSelectionGraphDrawer.GetType().FullName
				+ ".Draw: bitmap argument can't be null.\r\n"
				+ "Parameter name: bitmap"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestDraw5_()
    //
    /// <summary>
    /// Tests the Draw(Bitmap, Rectangle, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestDraw5_()
    {
		// Select a vertex and edge, draw with and without selection within a
		// deflated rectangle.

		foreach (Boolean bDrawSelection in GraphUtil.AllBoolean)
		{
		DerivedSetUp();

		IVertex [] aoVertices;
		IEdge [] aoEdges;

		const Int32 Vertices = 4;

		PopulateGraph(Vertices, out aoVertices, out aoEdges);

		Rectangle oRectangle = GetBitmapRectangle();

		oRectangle.Inflate(-10, -20);

		m_oMultiSelectionGraphDrawer.Draw(m_oBitmap, oRectangle, false);

		MultiSelectionGraphDrawer.SelectVertex( aoVertices[0] );
		MultiSelectionGraphDrawer.SelectEdge( aoEdges[1] );

		m_oMultiSelectionGraphDrawer.Draw(m_oBitmap, oRectangle,
			bDrawSelection);

		String sTestImageFileNameNoExtension =
			"TestDraw5" + "-" + bDrawSelection.ToString();

		SaveOrCompareTestImage(m_oBitmap, sTestImageFileNameNoExtension);
		}
    }

    //*************************************************************************
    //  Method: TestDraw5_Bad()
    //
    /// <summary>
    /// Tests the Draw(Bitmap, Rectangle, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestDraw5_Bad()
    {
		// null bitmap.

		DerivedSetUp();

		try
		{
			m_oMultiSelectionGraphDrawer.Draw( (Bitmap)null,
				GetBitmapRectangle(), true);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				m_oMultiSelectionGraphDrawer.GetType().FullName
				+ ".Draw: bitmap argument can't be null.\r\n"
				+ "Parameter name: bitmap"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestDraw6(_)
    //
    /// <summary>
    /// Tests the Draw(Graphics, Rectangle, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestDraw6_()
    {
		// Select a vertex and edge, draw with and without selection within a
		// deflated rectangle.

		foreach (Boolean bDrawSelection in GraphUtil.AllBoolean)
		{
		DerivedSetUp();

		IVertex [] aoVertices;
		IEdge [] aoEdges;

		const Int32 Vertices = 5;

		PopulateGraph(Vertices, out aoVertices, out aoEdges);

		Rectangle oRectangle = GetBitmapRectangle();

		oRectangle.Inflate(-10, -20);

		Graphics oGraphics = Graphics.FromImage(m_oBitmap);

		m_oMultiSelectionGraphDrawer.Draw(oGraphics, oRectangle, false);

		MultiSelectionGraphDrawer.SelectVertex( aoVertices[0] );
		MultiSelectionGraphDrawer.SelectEdge( aoEdges[1] );

		m_oMultiSelectionGraphDrawer.Draw(oGraphics, oRectangle,
			bDrawSelection);

		String sTestImageFileNameNoExtension =
			"TestDraw6" + "-" + bDrawSelection.ToString();

		SaveOrCompareTestImage(m_oBitmap, sTestImageFileNameNoExtension);

		GraphicsUtil.DisposeGraphics(ref oGraphics);
		}
    }

    //*************************************************************************
    //  Method: TestDraw6_Bad()
    //
    /// <summary>
    /// Tests the Draw(Graphics, Rectangle, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestDraw6_Bad()
    {
		// null graphics.

		DerivedSetUp();

		try
		{
			m_oMultiSelectionGraphDrawer.Draw( (Graphics)null,
				GetBitmapRectangle(), true);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				m_oMultiSelectionGraphDrawer.GetType().FullName
				+ ".Draw: graphics argument can't be null.\r\n"
				+ "Parameter name: graphics"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestRedrawVertexAndEdge()
    //
    /// <summary>
    /// Tests the RedrawVertex(IVertex, Bitmap, Boolean) and
	/// RedrawEdge(IEdge, Bitmap, Boolean) methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRedrawVertexAndEdge()
    {
		// Select a vertex and edge, redraw with and without selection.

		foreach (Boolean bDrawSelection in GraphUtil.AllBoolean)
		{
		DerivedSetUp();

		IVertex [] aoVertices;
		IEdge [] aoEdges;

		const Int32 Vertices = 3;

		PopulateGraph(Vertices, out aoVertices, out aoEdges);

		m_oMultiSelectionGraphDrawer.Draw(m_oBitmap, false);

		IVertex oSelectedVertex = aoVertices[0];
		IEdge oSelectedEdge = aoEdges[0];

		MultiSelectionGraphDrawer.SelectVertex(oSelectedVertex);
		MultiSelectionGraphDrawer.SelectEdge(oSelectedEdge);

		m_oMultiSelectionGraphDrawer.RedrawVertex(
			oSelectedVertex, m_oBitmap, bDrawSelection);

		m_oMultiSelectionGraphDrawer.RedrawEdge(
			oSelectedEdge, m_oBitmap, bDrawSelection);

		String sTestImageFileNameNoExtension =
			"TestRedrawVertexAndEdge" + "-" + bDrawSelection.ToString();

		SaveOrCompareTestImage(m_oBitmap, sTestImageFileNameNoExtension);
		}
    }

    //*************************************************************************
    //  Method: TestRedrawVertexAndEdge2()
    //
    /// <summary>
    /// Tests the RedrawVertex(IVertex, Bitmap, Rectangle, Boolean) and
	/// RedrawEdge(IEdge, Bitmap, Rectangle, Boolean) methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRedrawVertexAndEdge2()
    {
		// Select a vertex and edge, redraw with and without selection within
		// a deflated rectangle.

		foreach (Boolean bDrawSelection in GraphUtil.AllBoolean)
		{
		DerivedSetUp();

		IVertex [] aoVertices;
		IEdge [] aoEdges;

		const Int32 Vertices = 3;

		PopulateGraph(Vertices, out aoVertices, out aoEdges);

		Rectangle oRectangle = GetBitmapRectangle();

		oRectangle.Inflate(-10, -20);

		m_oMultiSelectionGraphDrawer.Draw(m_oBitmap, oRectangle, false);

		IVertex oSelectedVertex = aoVertices[1];
		IEdge oSelectedEdge = aoEdges[1];

		MultiSelectionGraphDrawer.SelectVertex(oSelectedVertex);
		MultiSelectionGraphDrawer.SelectEdge(oSelectedEdge);

		m_oMultiSelectionGraphDrawer.RedrawVertex(
			oSelectedVertex, m_oBitmap, oRectangle, bDrawSelection);

		m_oMultiSelectionGraphDrawer.RedrawEdge(
			oSelectedEdge, m_oBitmap, oRectangle, bDrawSelection);

		String sTestImageFileNameNoExtension =
			"TestRedrawVertexAndEdge2" + "-" + bDrawSelection.ToString();

		SaveOrCompareTestImage(m_oBitmap, sTestImageFileNameNoExtension);
		}
    }

    //*************************************************************************
    //  Method: TestRedrawVertexAndEdge3()
    //
    /// <summary>
    /// Tests the RedrawVertex(IVertex, Graphics, Rectangle, Boolean) and
	/// RedrawEdge(IEdge, Graphics, Rectangle, Boolean) methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRedrawVertexAndEdge3()
    {
		// Select a vertex and edge, rddraw with and without selection within a
		// deflated rectangle.

		foreach (Boolean bDrawSelection in GraphUtil.AllBoolean)
		{
		DerivedSetUp();

		IVertex [] aoVertices;
		IEdge [] aoEdges;

		const Int32 Vertices = 5;

		PopulateGraph(Vertices, out aoVertices, out aoEdges);

		Rectangle oRectangle = GetBitmapRectangle();

		oRectangle.Inflate(-10, -20);

		Graphics oGraphics = Graphics.FromImage(m_oBitmap);

		m_oMultiSelectionGraphDrawer.Draw(oGraphics, oRectangle, false);

		IVertex oSelectedVertex = aoVertices[3];
		IEdge oSelectedEdge = aoEdges[3];

		MultiSelectionGraphDrawer.SelectVertex(oSelectedVertex);
		MultiSelectionGraphDrawer.SelectEdge(oSelectedEdge);

		m_oMultiSelectionGraphDrawer.RedrawVertex(
			oSelectedVertex, oGraphics, oRectangle, bDrawSelection);

		m_oMultiSelectionGraphDrawer.RedrawEdge(
			oSelectedEdge, oGraphics, oRectangle, bDrawSelection);

		String sTestImageFileNameNoExtension =
			"TestRedrawVertexAndEdge3" + "-" + bDrawSelection.ToString();

		SaveOrCompareTestImage(m_oBitmap, sTestImageFileNameNoExtension);

		GraphicsUtil.DisposeGraphics(ref oGraphics);
		}
    }

    //*************************************************************************
    //  Method: TestRedrawVertexAndEdge4()
    //
    /// <summary>
    /// Tests the RedrawVertex(IVertex, Graphics, Rectangle, Boolean) and
	/// RedrawEdge(IEdge, Graphics, Rectangle, Boolean) methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRedrawVertexAndEdge4()
    {
		// Select all vertices, redraw all vertices.

		DerivedSetUp();

		IVertex [] aoVertices;
		IEdge [] aoEdges;

		const Int32 Vertices = 50;

		PopulateGraph(Vertices, out aoVertices, out aoEdges);

		Rectangle oRectangle = GetBitmapRectangle();

		m_oMultiSelectionGraphDrawer.Draw(m_oBitmap, false);

		foreach (IVertex oVertex in
			m_oMultiSelectionGraphDrawer.Graph.Vertices)
		{
			MultiSelectionGraphDrawer.SelectVertex(oVertex);
		}

		foreach (IVertex oVertex in
			m_oMultiSelectionGraphDrawer.Graph.Vertices)
		{
			m_oMultiSelectionGraphDrawer.RedrawVertex(
				oVertex, m_oBitmap, true);
		}

		const String TestImageFileNameNoExtension =
			"TestRedrawVertexAndEdge4";

		SaveOrCompareTestImage(m_oBitmap, TestImageFileNameNoExtension);
    }

    //*************************************************************************
    //  Method: TestRedrawVertexAndEdge5()
    //
    /// <summary>
    /// Tests the RedrawVertex(IVertex, Graphics, Rectangle, Boolean) and
	/// RedrawEdge(IEdge, Graphics, Rectangle, Boolean) methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRedrawVertexAndEdge5()
    {
		// Select all edges, redraw all edges.

		DerivedSetUp();

		IVertex [] aoVertices;
		IEdge [] aoEdges;

		const Int32 Vertices = 50;

		PopulateGraph(Vertices, out aoVertices, out aoEdges);

		Rectangle oRectangle = GetBitmapRectangle();

		m_oMultiSelectionGraphDrawer.Draw(m_oBitmap, false);

		foreach (IEdge oEdge in m_oMultiSelectionGraphDrawer.Graph.Edges)
		{
			MultiSelectionGraphDrawer.SelectEdge(oEdge);
		}

		foreach (IEdge oEdge in m_oMultiSelectionGraphDrawer.Graph.Edges)
		{
			m_oMultiSelectionGraphDrawer.RedrawEdge(oEdge, m_oBitmap, true);
		}

		const String TestImageFileNameNoExtension =
			"TestRedrawVertexAndEdge5";

		SaveOrCompareTestImage(m_oBitmap, TestImageFileNameNoExtension);
    }

    //*************************************************************************
    //  Method: TestRedrawVertexAndEdge6()
    //
    /// <summary>
    /// Tests the RedrawVertex(IVertex, Graphics, Rectangle, Boolean) and
	/// RedrawEdge(IEdge, Graphics, Rectangle, Boolean) methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRedrawVertexAndEdge6()
    {
		// Select all vertices and edges, redraw all vertices and edges.

		DerivedSetUp();

		IVertex [] aoVertices;
		IEdge [] aoEdges;

		const Int32 Vertices = 50;

		PopulateGraph(Vertices, out aoVertices, out aoEdges);

		Rectangle oRectangle = GetBitmapRectangle();

		m_oMultiSelectionGraphDrawer.Draw(m_oBitmap, false);

		foreach (IVertex oVertex in
			m_oMultiSelectionGraphDrawer.Graph.Vertices)
		{
			MultiSelectionGraphDrawer.SelectVertex(oVertex);
		}

		foreach (IEdge oEdge in m_oMultiSelectionGraphDrawer.Graph.Edges)
		{
			MultiSelectionGraphDrawer.SelectEdge(oEdge);
		}

		foreach (IVertex oVertex in
			m_oMultiSelectionGraphDrawer.Graph.Vertices)
		{
			m_oMultiSelectionGraphDrawer.RedrawVertex(
				oVertex, m_oBitmap, true);
		}

		foreach (IEdge oEdge in m_oMultiSelectionGraphDrawer.Graph.Edges)
		{
			m_oMultiSelectionGraphDrawer.RedrawEdge(oEdge, m_oBitmap, true);
		}

		foreach (IVertex oVertex in
			m_oMultiSelectionGraphDrawer.Graph.Vertices)
		{
			m_oMultiSelectionGraphDrawer.RedrawVertex(
				oVertex, m_oBitmap, true);
		}

		const String TestImageFileNameNoExtension =
			"TestRedrawVertexAndEdge6";

		SaveOrCompareTestImage(m_oBitmap, TestImageFileNameNoExtension);
    }

    //*************************************************************************
    //  Method: TestRedrawVertexAndEdge7()
    //
    /// <summary>
    /// Tests the RedrawVertex(IVertex, Graphics, Rectangle, Boolean) and
	/// RedrawEdge(IEdge, Graphics, Rectangle, Boolean) methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRedrawVertexAndEdge7()
    {
		// Select every other vertex and edge, redraw all vertices and edges.

		DerivedSetUp();

		IVertex [] aoVertices;
		IEdge [] aoEdges;

		const Int32 Vertices = 50;

		PopulateGraph(Vertices, out aoVertices, out aoEdges);

		Rectangle oRectangle = GetBitmapRectangle();

		m_oMultiSelectionGraphDrawer.Draw(m_oBitmap, false);

		Int32 iCount = 0;

		foreach (IVertex oVertex in
			m_oMultiSelectionGraphDrawer.Graph.Vertices)
		{
			if (iCount % 2 == 0)
			{
				MultiSelectionGraphDrawer.SelectVertex(oVertex);
			}

			iCount++;
		}

		iCount = 0;

		foreach (IEdge oEdge in m_oMultiSelectionGraphDrawer.Graph.Edges)
		{
			if (iCount % 2 == 0)
			{
				MultiSelectionGraphDrawer.SelectEdge(oEdge);
			}

			iCount++;
		}

		foreach (IVertex oVertex in
			m_oMultiSelectionGraphDrawer.Graph.Vertices)
		{
			m_oMultiSelectionGraphDrawer.RedrawVertex(
				oVertex, m_oBitmap, true);
		}

		foreach (IEdge oEdge in m_oMultiSelectionGraphDrawer.Graph.Edges)
		{
			m_oMultiSelectionGraphDrawer.RedrawEdge(oEdge, m_oBitmap, true);
		}

		foreach (IVertex oVertex in
			m_oMultiSelectionGraphDrawer.Graph.Vertices)
		{
			m_oMultiSelectionGraphDrawer.RedrawVertex(
				oVertex, m_oBitmap, true);
		}

		const String TestImageFileNameNoExtension =
			"TestRedrawVertexAndEdge7";

		SaveOrCompareTestImage(m_oBitmap, TestImageFileNameNoExtension);
    }

    //*************************************************************************
    //  Method: TestRedrawVertexBad()
    //
    /// <summary>
    /// Tests the RedrawVertex(IVertex, Bitmap, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestRedrawVertexBad()
    {
		// null vertex.

		DerivedSetUp();

		try
		{
			m_oMultiSelectionGraphDrawer.RedrawVertex(null, m_oBitmap, true);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				m_oMultiSelectionGraphDrawer.GetType().FullName
				+ ".RedrawVertex: vertex argument can't be null.\r\n"
				+ "Parameter name: vertex"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestRedrawVertexBad2()
    //
    /// <summary>
    /// Tests the RedrawVertex(IVertex, Bitmap, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestRedrawVertexBad2()
    {
		// null bitmap.

		DerivedSetUp();

		try
		{
			m_oMultiSelectionGraphDrawer.RedrawVertex(
				(new VertexFactory() ).CreateVertex(), null, true);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				m_oMultiSelectionGraphDrawer.GetType().FullName
				+ ".RedrawVertex: bitmap argument can't be null.\r\n"
				+ "Parameter name: bitmap"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestRedrawVertexBad3()
    //
    /// <summary>
    /// Tests the RedrawVertex(IVertex, Bitmap, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestRedrawVertexBad3()
    {
		// Vertex doesn't belong to graph.

		DerivedSetUp();

		try
		{
			m_oMultiSelectionGraphDrawer.RedrawVertex(
				(new VertexFactory() ).CreateVertex(), m_oBitmap, true);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				m_oMultiSelectionGraphDrawer.GetType().FullName
				+ ".RedrawVertex: The specified vertex does not belong to the"
				+ " graph.\r\n"
				+ "Parameter name: vertex"
				,
				oArgumentException.Message
				);

			throw oArgumentException;
		}
    }

    //*************************************************************************
    //  Method: TestRedrawVertex2_Bad()
    //
    /// <summary>
    /// Tests the RedrawVertex(IVertex, Bitmap, Rectangle, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestRedrawVertex2_Bad()
    {
		// null vertex.

		DerivedSetUp();

		try
		{
			m_oMultiSelectionGraphDrawer.RedrawVertex(
				null, m_oBitmap, GetBitmapRectangle(), true);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				m_oMultiSelectionGraphDrawer.GetType().FullName
				+ ".RedrawVertex: vertex argument can't be null.\r\n"
				+ "Parameter name: vertex"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestRedrawVertex2_Bad2()
    //
    /// <summary>
    /// Tests the RedrawVertex(IVertex, Bitmap, Rectangle, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestRedrawVertex2_Bad2()
    {
		// null bitmap.

		DerivedSetUp();

		try
		{
			m_oMultiSelectionGraphDrawer.RedrawVertex(
				(new VertexFactory() ).CreateVertex(), (Bitmap)null,
				GetBitmapRectangle(), true);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				m_oMultiSelectionGraphDrawer.GetType().FullName
				+ ".RedrawVertex: bitmap argument can't be null.\r\n"
				+ "Parameter name: bitmap"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestRedrawVertex2_Bad3()
    //
    /// <summary>
    /// Tests the RedrawVertex(IVertex, Bitmap, Rectangle, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestRedrawVertex2_Bad3()
    {
		// Vertex doesn't belong to graph.

		DerivedSetUp();

		try
		{
			m_oMultiSelectionGraphDrawer.RedrawVertex(
				(new VertexFactory() ).CreateVertex(), m_oBitmap,
				GetBitmapRectangle(), true);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				m_oMultiSelectionGraphDrawer.GetType().FullName
				+ ".RedrawVertex: The specified vertex does not belong to the"
				+ " graph.\r\n"
				+ "Parameter name: vertex"
				,
				oArgumentException.Message
				);

			throw oArgumentException;
		}
    }

    //*************************************************************************
    //  Method: TestRedrawVertex3_Bad()
    //
    /// <summary>
    /// Tests the RedrawVertex(IVertex, Graphics, Rectangle, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestRedrawVertex3_Bad()
    {
		// null vertex.

		DerivedSetUp();

		Graphics oGraphics = Graphics.FromImage(m_oBitmap);

		try
		{
			m_oMultiSelectionGraphDrawer.RedrawVertex(
				null, oGraphics, GetBitmapRectangle(), true);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				m_oMultiSelectionGraphDrawer.GetType().FullName
				+ ".RedrawVertex: vertex argument can't be null.\r\n"
				+ "Parameter name: vertex"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
		finally
		{
			oGraphics.Dispose();
		}
    }

    //*************************************************************************
    //  Method: TestRedrawVertex3_Bad2()
    //
    /// <summary>
    /// Tests the RedrawVertex(IVertex, Graphics, Rectangle, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestRedrawVertex3_Bad2()
    {
		// null graphics.

		DerivedSetUp();

		try
		{
			IVertex [] aoVertices;
			IEdge [] aoEdges;

			const Int32 Vertices = 5;

			PopulateGraph(Vertices, out aoVertices, out aoEdges);

			m_oMultiSelectionGraphDrawer.RedrawVertex(aoVertices[0],
				(Graphics)null, GetBitmapRectangle(), true);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				m_oMultiSelectionGraphDrawer.GetType().FullName
				+ ".RedrawVertex: graphics argument can't be null.\r\n"
				+ "Parameter name: graphics"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestRedrawVertex3_Bad3()
    //
    /// <summary>
    /// Tests the RedrawVertex(IVertex, Graphics, Rectangle, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestRedrawVertex3_Bad3()
    {
		// Vertex doesn't belong to graph.

		DerivedSetUp();

		Graphics oGraphics = Graphics.FromImage(m_oBitmap);

		try
		{
			m_oMultiSelectionGraphDrawer.RedrawVertex(
				(new VertexFactory() ).CreateVertex(), oGraphics,
				GetBitmapRectangle(), true);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				m_oMultiSelectionGraphDrawer.GetType().FullName
				+ ".RedrawVertex: The specified vertex does not belong to the"
				+ " graph.\r\n"
				+ "Parameter name: vertex"
				,
				oArgumentException.Message
				);

			throw oArgumentException;
		}
		finally
		{
			oGraphics.Dispose();
		}
    }

    //*************************************************************************
    //  Method: TestRedrawEdgeBad()
    //
    /// <summary>
    /// Tests the RedrawEdge(IEdge, Bitmap, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestRedrawEdgeBad()
    {
		// null edge.

		DerivedSetUp();

		try
		{
			m_oMultiSelectionGraphDrawer.RedrawEdge(null, m_oBitmap, true);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				m_oMultiSelectionGraphDrawer.GetType().FullName
				+ ".RedrawEdge: edge argument can't be null.\r\n"
				+ "Parameter name: edge"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestRedrawEdgeBad2()
    //
    /// <summary>
    /// Tests the RedrawEdge(IEdge, Bitmap, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestRedrawEdgeBad2()
    {
		// null bitmap.

		DerivedSetUp();

		try
		{
			IVertex [] aoVertices;
			IEdge [] aoEdges;

			const Int32 Vertices = 5;

			PopulateGraph(Vertices, out aoVertices, out aoEdges);

			m_oMultiSelectionGraphDrawer.RedrawEdge(aoEdges[0], null, true);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				m_oMultiSelectionGraphDrawer.GetType().FullName
				+ ".RedrawEdge: bitmap argument can't be null.\r\n"
				+ "Parameter name: bitmap"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestRedrawEdgeBad3()
    //
    /// <summary>
    /// Tests the RedrawEdge(IEdge, Bitmap, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestRedrawEdgeBad3()
    {
		// Edge doesn't belong to graph.

		DerivedSetUp();

		try
		{
			IVertex [] aoVertices;
			IEdge [] aoEdges;

			const Int32 Vertices = 5;

			IGraph oGraph = new Graph();

			aoVertices = GraphUtil.AddVertices(oGraph, Vertices);

			aoEdges = GraphUtil.MakeGraphComplete(oGraph, aoVertices, false);

			m_oMultiSelectionGraphDrawer.RedrawEdge(
				aoEdges[0], m_oBitmap, true);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				m_oMultiSelectionGraphDrawer.GetType().FullName
				+ ".RedrawEdge: The specified edge does not belong to the"
				+ " graph.\r\n"
				+ "Parameter name: edge"
				,
				oArgumentException.Message
				);

			throw oArgumentException;
		}
    }

    //*************************************************************************
    //  Method: TestRedrawEdge2_Bad()
    //
    /// <summary>
    /// Tests the RedrawEdge(IEdge, Bitmap, Rectangle, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestRedrawEdge2_Bad()
    {
		// null edge.

		DerivedSetUp();

		try
		{
			m_oMultiSelectionGraphDrawer.RedrawEdge(
				null, m_oBitmap, GetBitmapRectangle(), true);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				m_oMultiSelectionGraphDrawer.GetType().FullName
				+ ".RedrawEdge: edge argument can't be null.\r\n"
				+ "Parameter name: edge"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestRedrawEdge2_Bad2()
    //
    /// <summary>
    /// Tests the RedrawEdge(IEdge, Bitmap, Rectangle, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestRedrawEdge2_Bad2()
    {
		// null bitmap.

		DerivedSetUp();

		try
		{
			IVertex [] aoVertices;
			IEdge [] aoEdges;

			const Int32 Vertices = 5;

			PopulateGraph(Vertices, out aoVertices, out aoEdges);

			m_oMultiSelectionGraphDrawer.RedrawEdge(
				aoEdges[0], (Bitmap)null, GetBitmapRectangle(), true);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				m_oMultiSelectionGraphDrawer.GetType().FullName
				+ ".RedrawEdge: bitmap argument can't be null.\r\n"
				+ "Parameter name: bitmap"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestRedrawEdge2_Bad3()
    //
    /// <summary>
    /// Tests the RedrawEdge(IEdge, Bitmap, Rectangle, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestRedrawEdge2_Bad3()
    {
		// Edge doesn't belong to graph.

		DerivedSetUp();

		try
		{
			IVertex [] aoVertices;
			IEdge [] aoEdges;

			const Int32 Vertices = 5;

			IGraph oGraph = new Graph();

			aoVertices = GraphUtil.AddVertices(oGraph, Vertices);

			aoEdges = GraphUtil.MakeGraphComplete(oGraph, aoVertices, false);

			m_oMultiSelectionGraphDrawer.RedrawEdge(
				aoEdges[0], m_oBitmap, GetBitmapRectangle(), true);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				m_oMultiSelectionGraphDrawer.GetType().FullName
				+ ".RedrawEdge: The specified edge does not belong to the"
				+ " graph.\r\n"
				+ "Parameter name: edge"
				,
				oArgumentException.Message
				);

			throw oArgumentException;
		}
    }

    //*************************************************************************
    //  Method: TestRedrawEdge3_Bad()
    //
    /// <summary>
    /// Tests the RedrawEdge(IEdge, Graphics, Rectangle, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestRedrawEdge3_Bad()
    {
		// null edge.

		DerivedSetUp();

		Graphics oGraphics = Graphics.FromImage(m_oBitmap);

		try
		{
			m_oMultiSelectionGraphDrawer.RedrawEdge(
				null, oGraphics, GetBitmapRectangle(), true);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				m_oMultiSelectionGraphDrawer.GetType().FullName
				+ ".RedrawEdge: edge argument can't be null.\r\n"
				+ "Parameter name: edge"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
		finally
		{
			oGraphics.Dispose();
		}
    }

    //*************************************************************************
    //  Method: TestRedrawEdge3_Bad2()
    //
    /// <summary>
    /// Tests the RedrawEdge(IEdge, Graphics, Rectangle, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestRedrawEdge3_Bad2()
    {
		// null graphics.

		DerivedSetUp();

		try
		{
			IVertex [] aoVertices;
			IEdge [] aoEdges;

			const Int32 Vertices = 5;

			PopulateGraph(Vertices, out aoVertices, out aoEdges);

			m_oMultiSelectionGraphDrawer.RedrawEdge(
				aoEdges[0], (Graphics)null, GetBitmapRectangle(), true);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				m_oMultiSelectionGraphDrawer.GetType().FullName
				+ ".RedrawEdge: graphics argument can't be null.\r\n"
				+ "Parameter name: graphics"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestRedrawEdge3_Bad3()
    //
    /// <summary>
    /// Tests the RedrawEdge(IEdge, Graphics, Rectangle, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestRedrawEdge3_Bad3()
    {
		// Edge doesn't belong to graph.

		DerivedSetUp();

		Graphics oGraphics = Graphics.FromImage(m_oBitmap);

		try
		{
			IVertex [] aoVertices;
			IEdge [] aoEdges;

			const Int32 Vertices = 5;

			IGraph oGraph = new Graph();

			aoVertices = GraphUtil.AddVertices(oGraph, Vertices);

			aoEdges = GraphUtil.MakeGraphComplete(oGraph, aoVertices, false);

			m_oMultiSelectionGraphDrawer.RedrawEdge(
				aoEdges[0], oGraphics, GetBitmapRectangle(), true);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				m_oMultiSelectionGraphDrawer.GetType().FullName
				+ ".RedrawEdge: The specified edge does not belong to the"
				+ " graph.\r\n"
				+ "Parameter name: edge"
				,
				oArgumentException.Message
				);

			throw oArgumentException;
		}
		finally
		{
			oGraphics.Dispose();
		}
    }

    //*************************************************************************
    //  Method: TestSelectVertexMethods()
    //
    /// <summary>
    /// Tests the SelectVertex(), DeselectVertex(), and VertexIsSelected()
	/// methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSelectVertexMethods()
    {
		DerivedSetUp();

		IVertex oVertex = ( new VertexFactory() ).CreateVertex();

		Assert.IsFalse(
			MultiSelectionGraphDrawer.VertexIsSelected(oVertex) );

		MultiSelectionGraphDrawer.SelectVertex(oVertex);

		Assert.IsTrue(
			MultiSelectionGraphDrawer.VertexIsSelected(oVertex) );

		MultiSelectionGraphDrawer.DeselectVertex(oVertex);

		Assert.IsFalse(
			MultiSelectionGraphDrawer.VertexIsSelected(oVertex) );

		MultiSelectionGraphDrawer.SelectVertex(oVertex, true);

		Assert.IsTrue(
			MultiSelectionGraphDrawer.VertexIsSelected(oVertex) );

		MultiSelectionGraphDrawer.SelectVertex(oVertex, false);

		Assert.IsFalse(
			MultiSelectionGraphDrawer.VertexIsSelected(oVertex) );
    }

    //*************************************************************************
    //  Method: TestSelectEdgeMethods()
    //
    /// <summary>
    /// Tests the SelectEdge(), DeselectEdge(), and EdgeIsSelected() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSelectEdgeMethods()
    {
		DerivedSetUp();

		const Int32 Vertices = 2;

		IGraph oGraph = new Graph();

		IVertex [] aoVertices = GraphUtil.AddVertices(oGraph, Vertices);

		IEdge oEdge = oGraph.Edges.Add( aoVertices[0], aoVertices[1] );

		Assert.IsFalse( MultiSelectionGraphDrawer.EdgeIsSelected(oEdge) );

		MultiSelectionGraphDrawer.SelectEdge(oEdge);

		Assert.IsTrue( MultiSelectionGraphDrawer.EdgeIsSelected(oEdge) );

		MultiSelectionGraphDrawer.DeselectEdge(oEdge);

		Assert.IsFalse( MultiSelectionGraphDrawer.EdgeIsSelected(oEdge) );

		MultiSelectionGraphDrawer.SelectEdge(oEdge, true);

		Assert.IsTrue( MultiSelectionGraphDrawer.EdgeIsSelected(oEdge) );

		MultiSelectionGraphDrawer.SelectEdge(oEdge, false);

		Assert.IsFalse( MultiSelectionGraphDrawer.EdgeIsSelected(oEdge) );
    }

    //*************************************************************************
    //  Method: TestSelectVertexBad()
    //
    /// <summary>
    /// Tests the SelectVertex(IVertex) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestSelectVertexBad()
    {
		// null vertex.

		DerivedSetUp();

		try
		{
			MultiSelectionGraphDrawer.SelectVertex(null);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				m_oMultiSelectionGraphDrawer.GetType().FullName
				+ ".SelectVertex: vertex argument can't be null.\r\n"
				+ "Parameter name: vertex"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestSelectVertex2_Bad()
    //
    /// <summary>
    /// Tests the SelectVertex(IVertex, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestSelectVertex2_Bad()
    {
		// null vertex.

		DerivedSetUp();

		try
		{
			MultiSelectionGraphDrawer.SelectVertex(null, false);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				m_oMultiSelectionGraphDrawer.GetType().FullName
				+ ".SelectVertex: vertex argument can't be null.\r\n"
				+ "Parameter name: vertex"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestDeselectVertexBad()
    //
    /// <summary>
    /// Tests the DeselectVertex() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestDeselectVertexBad()
    {
		// null vertex.

		DerivedSetUp();

		try
		{
			MultiSelectionGraphDrawer.DeselectVertex(null);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				m_oMultiSelectionGraphDrawer.GetType().FullName
				+ ".DeselectVertex: vertex argument can't be null.\r\n"
				+ "Parameter name: vertex"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestVertexIsSelectedBad()
    //
    /// <summary>
    /// Tests the VertexIsSelected() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestVertexIsSelectedBad()
    {
		// null vertex.

		DerivedSetUp();

		try
		{
			MultiSelectionGraphDrawer.VertexIsSelected(null);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				m_oMultiSelectionGraphDrawer.GetType().FullName
				+ ".VertexIsSelected: vertex argument can't be null.\r\n"
				+ "Parameter name: vertex"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestUsageScenario2()
    //
    /// <summary>
    /// Tests a usage scenario.
    /// </summary>
	///
	/// <remarks>
	/// Most of the the code in this method is meant to be copied to the
	/// documentation for the MultiSelectionGraphDrawer class.
	/// </remarks>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestUsageScenario2()
    {
		// Everything between the lines of asterisks should be copied to the
		// documentation.


		//************************************************************

		// Create an object for drawing graphs.

		MultiSelectionGraphDrawer oMultiSelectionGraphDrawer =
			new MultiSelectionGraphDrawer();

		// The object owns an empty graph.  Get the graph's vertex and edge
		// collections.

		IGraph oGraph = oMultiSelectionGraphDrawer.Graph;
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

		// Select a vertex and an edge.

		MultiSelectionGraphDrawer.SelectVertex(oVertex1);
		MultiSelectionGraphDrawer.SelectEdge(oEdge2);

		// Set the graph's colors.

		oMultiSelectionGraphDrawer.BackColor = Color.White;

		// The MultiSelectionGraphDrawer's default vertex drawer is a
		// VertexDrawer object.

		VertexDrawer oVertexDrawer =
			(VertexDrawer)oMultiSelectionGraphDrawer.VertexDrawer;

		oVertexDrawer.Color = Color.Black;
		oVertexDrawer.SelectedColor = Color.Red;

		// The MultiSelectionGraphDrawer's default edge drawer is an EdgeDrawer
		// object.

		EdgeDrawer oEdgeDrawer =
			(EdgeDrawer)oMultiSelectionGraphDrawer.EdgeDrawer;

		oEdgeDrawer.Color = Color.Black;
		oEdgeDrawer.SelectedColor = Color.Red;

		// Create a bitmap and draw the graph onto it.

		Bitmap oBitmap = new Bitmap(400, 200);

		oMultiSelectionGraphDrawer.Draw(oBitmap, true);

		// Do something with the bitmap...

		//************************************************************


		const String TestImageFileNameNoExtension = "TestUsageScenario2";

		SaveOrCompareTestImage(oBitmap, TestImageFileNameNoExtension);
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

    protected new void
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
    //  Protected constants
    //*************************************************************************

	/// Some unit tests create in-memory bitmaps that are compared to saved
	/// image files.  The image files are stored in the directory specified
	/// by TestImageRelativeDirectory, which is relative to the NodeXL\UnitTests
	/// directory.

	protected new const String TestImageRelativeDirectory =
		"Visualization\\GraphDrawers\\TestImageFiles\\"
		+ "MultiSelectionGraphDrawer";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.  This is actually the base-class m_oGraphDrawer cast to
	/// a MultiSelectionGraphDrawer.

    protected MultiSelectionGraphDrawer m_oMultiSelectionGraphDrawer;
}

}
