
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.Visualization;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Common;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: EdgeDrawerTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="EdgeDrawer" />
/// class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class EdgeDrawerTest : Object
{
    //*************************************************************************
    //  Constructor: EdgeDrawerTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EdgeDrawerTest" /> class.
    /// </summary>
    //*************************************************************************

    public EdgeDrawerTest()
    {
        m_oEdgeDrawer = null;
		m_oEdge = null;
		m_oBitmap = null;
		m_bRedrawRequired = false;
		m_bLayoutRequired = false;
		m_oDrawContext = null;
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
        m_oEdgeDrawer = new EdgeDrawer();

		m_oEdgeDrawer.RedrawRequired +=
			new EventHandler(this.EdgeDrawer_RedrawRequired);

		m_oEdgeDrawer.LayoutRequired +=
			new EventHandler(this.EdgeDrawer_LayoutRequired);

		m_oEdge = CreateEdge(2, 3, 15, 19, true);

		m_oBitmap = new Bitmap(BitmapWidth, BitmapHeight);

		Rectangle oGraphRectangle = new Rectangle(Point.Empty, m_oBitmap.Size);

		Graphics oGraphics = Graphics.FromImage(m_oBitmap);

		oGraphics.Clip = new Region(oGraphRectangle);

		m_oDrawContext = new DrawContext(
            new MockGraphDrawer(), oGraphics, oGraphRectangle, Margin);

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
        if (m_oEdgeDrawer != null)
		{
			m_oEdgeDrawer.Dispose();
			m_oEdgeDrawer = null;
		}

		m_oEdge = null;

        if (m_oBitmap != null)
		{
			m_oBitmap.Dispose();
			m_oBitmap = null;
		}

		m_oDrawContext = null;
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
		Assert.IsTrue(m_oEdgeDrawer.UseSelection);

		Assert.AreEqual(SystemColors.WindowText, m_oEdgeDrawer.Color);

		Assert.AreEqual(Color.Red, m_oEdgeDrawer.SelectedColor);

		Assert.IsTrue(m_oEdgeDrawer.DrawArrowOnDirectedEdge);

		Assert.AreEqual(3F, m_oEdgeDrawer.RelativeArrowSize);

		Assert.AreEqual(1, m_oEdgeDrawer.Width);

		Assert.AreEqual(2, m_oEdgeDrawer.SelectedWidth);
    }

    //*************************************************************************
    //  Method: TestUseSelection()
    //
    /// <summary>
    /// Tests the UseSelection property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestUseSelection()
    {
        m_oEdgeDrawer.UseSelection = false;
        Assert.IsFalse(m_oEdgeDrawer.UseSelection);

        m_oEdgeDrawer.UseSelection = true;
        Assert.IsTrue(m_oEdgeDrawer.UseSelection);
    }

    //*************************************************************************
    //  Method: TestColor()
    //
    /// <summary>
    /// Tests the Color property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestColor()
    {
        Color oTestValue = Color.Tomato;

		Color oSelectedColor = m_oEdgeDrawer.SelectedColor;

        m_oEdgeDrawer.Color = oTestValue;
        Assert.AreEqual(oTestValue, m_oEdgeDrawer.Color);

		// Make sure the selected color hasn't been modified.

		Assert.AreEqual(oSelectedColor, m_oEdgeDrawer.SelectedColor);

		Assert.IsTrue(m_bRedrawRequired);
		Assert.IsFalse(m_bLayoutRequired);

		m_bRedrawRequired = false;

        m_oEdgeDrawer.Color = oTestValue;

		Assert.IsFalse(m_bRedrawRequired);
		Assert.IsFalse(m_bLayoutRequired);
    }

    //*************************************************************************
    //  Method: TestSelectedColor()
    //
    /// <summary>
    /// Tests the SelectedColor property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSelectedColor()
    {
        Color oTestValue = Color.DarkGreen;

		Color oColor = m_oEdgeDrawer.Color;

        m_oEdgeDrawer.SelectedColor = oTestValue;
        Assert.AreEqual(oTestValue, m_oEdgeDrawer.SelectedColor);

		// Make sure the unselected color hasn't been modified.

		Assert.AreEqual(oColor, m_oEdgeDrawer.Color);

		Assert.IsTrue(m_bRedrawRequired);
		Assert.IsFalse(m_bLayoutRequired);

		m_bRedrawRequired = false;

        m_oEdgeDrawer.SelectedColor = oTestValue;

		Assert.IsFalse(m_bRedrawRequired);
		Assert.IsFalse(m_bLayoutRequired);
    }

    //*************************************************************************
    //  Method: TestDrawArrowOnDirectedEdge()
    //
    /// <summary>
    /// Tests the DrawArrowOnDirectedEdge property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestDrawArrowOnDirectedEdge()
    {
        m_oEdgeDrawer.DrawArrowOnDirectedEdge = false;
        Assert.IsFalse(m_oEdgeDrawer.DrawArrowOnDirectedEdge);

        m_oEdgeDrawer.DrawArrowOnDirectedEdge = true;
        Assert.IsTrue(m_oEdgeDrawer.DrawArrowOnDirectedEdge);

        m_oEdgeDrawer.DrawArrowOnDirectedEdge = false;
        Assert.IsFalse(m_oEdgeDrawer.DrawArrowOnDirectedEdge);
    }

    //*************************************************************************
    //  Method: TestRelativeArrowSize()
    //
    /// <summary>
    /// Tests the RelativeArrowSize property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRelativeArrowSize()
    {
		foreach ( Single fTestValue in new Int32 [] {

			EdgeDrawer.MaximumRelativeArrowSize,

			EdgeDrawer.MinimumRelativeArrowSize,

			(EdgeDrawer.MaximumRelativeArrowSize -
				EdgeDrawer.MinimumRelativeArrowSize) / 2
			} )
		{
			m_oEdgeDrawer.RelativeArrowSize = fTestValue;
			Assert.AreEqual(fTestValue, m_oEdgeDrawer.RelativeArrowSize);

			Assert.IsTrue(m_bRedrawRequired);
			Assert.IsFalse(m_bLayoutRequired);

			m_bRedrawRequired = false;

			m_oEdgeDrawer.RelativeArrowSize = fTestValue;

			Assert.IsFalse(m_bRedrawRequired);
			Assert.IsFalse(m_bLayoutRequired);
		}
    }

    //*************************************************************************
    //  Method: TestRelativeArrowSizeBad()
    //
    /// <summary>
    /// Tests the RelativeArrowSize property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestRelativeArrowSizeBad()
    {
		try
		{
			m_oEdgeDrawer.RelativeArrowSize =
				EdgeDrawer.MinimumRelativeArrowSize - 1;
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual( String.Format(

				"Microsoft.NodeXL.Visualization."
				+ "EdgeDrawer.RelativeArrowSize: Must be between {0} and {1}."
				,
				EdgeDrawer.MinimumRelativeArrowSize,
				EdgeDrawer.MaximumRelativeArrowSize
				)
				,
				oApplicationException.Message
				);

			throw oApplicationException;
		}
    }

    //*************************************************************************
    //  Method: TestRelativeArrowSizeBad2()
    //
    /// <summary>
    /// Tests the RelativeArrowSize property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestRelativeArrowSizeBad2()
    {
		try
		{
			m_oEdgeDrawer.RelativeArrowSize =
				EdgeDrawer.MaximumRelativeArrowSize + 1;
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual( String.Format(

				"Microsoft.NodeXL.Visualization."
				+ "EdgeDrawer.RelativeArrowSize: Must be between {0} and {1}."
				,
				EdgeDrawer.MinimumRelativeArrowSize,
				EdgeDrawer.MaximumRelativeArrowSize
				)
				,
				oApplicationException.Message
				);

			throw oApplicationException;
		}
    }

    //*************************************************************************
    //  Method: TestWidth()
    //
    /// <summary>
    /// Tests the Width property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWidth()
    {
		Int32 iSelectedWidth = m_oEdgeDrawer.SelectedWidth;

		foreach ( Int32 iTestValue in new Int32 [] {

			EdgeDrawer.MaximumWidth,

			EdgeDrawer.MinimumWidth,

			(EdgeDrawer.MaximumWidth - EdgeDrawer.MinimumWidth) / 2
			} )
		{
			m_oEdgeDrawer.Width = iTestValue;
			Assert.AreEqual(iTestValue, m_oEdgeDrawer.Width);

			Assert.IsTrue(m_bRedrawRequired);
			Assert.IsFalse(m_bLayoutRequired);

			m_bRedrawRequired = false;

			m_oEdgeDrawer.Width = iTestValue;

			Assert.IsFalse(m_bRedrawRequired);
			Assert.IsFalse(m_bLayoutRequired);

			// Make sure the unselected width hasn't been modified.

			Assert.AreEqual(iSelectedWidth, m_oEdgeDrawer.SelectedWidth);
		}
    }

    //*************************************************************************
    //  Method: TestWidthBad()
    //
    /// <summary>
    /// Tests the Width property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestWidthBad()
    {
		try
		{
			m_oEdgeDrawer.Width = EdgeDrawer.MinimumWidth - 1;
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual( String.Format(

				"Microsoft.NodeXL.Visualization."
				+ "EdgeDrawer.Width: Must be between {0} and {1}."
				,
				EdgeDrawer.MinimumWidth,
				EdgeDrawer.MaximumWidth
				)
				,
				oApplicationException.Message
				);

			throw oApplicationException;
		}
    }

    //*************************************************************************
    //  Method: TestWidthBad2()
    //
    /// <summary>
    /// Tests the Width property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestWidthBad2()
    {
		try
		{
			m_oEdgeDrawer.Width = EdgeDrawer.MaximumWidth + 1;
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual( String.Format(

				"Microsoft.NodeXL.Visualization."
				+ "EdgeDrawer.Width: Must be between {0} and {1}."
				,
				EdgeDrawer.MinimumWidth,
				EdgeDrawer.MaximumWidth
				)
				,
				oApplicationException.Message
				);

			throw oApplicationException;
		}
    }

    //*************************************************************************
    //  Method: TestSelectedWidth()
    //
    /// <summary>
    /// Tests the SelectedWidth property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSelectedWidth()
    {
		Int32 iWidth = m_oEdgeDrawer.Width;

		foreach ( Int32 iTestValue in new Int32 [] {

			EdgeDrawer.MaximumWidth,

			EdgeDrawer.MinimumWidth,

			(EdgeDrawer.MaximumWidth - EdgeDrawer.MinimumWidth) / 2
			} )
		{
			m_oEdgeDrawer.SelectedWidth = iTestValue;
			Assert.AreEqual(iTestValue, m_oEdgeDrawer.SelectedWidth);

			Assert.IsTrue(m_bRedrawRequired);
			Assert.IsFalse(m_bLayoutRequired);

			m_bRedrawRequired = false;

			m_oEdgeDrawer.SelectedWidth = iTestValue;

			Assert.IsFalse(m_bRedrawRequired);
			Assert.IsFalse(m_bLayoutRequired);

			// Make sure the unselected width hasn't been modified.

			Assert.AreEqual(iWidth, m_oEdgeDrawer.Width);
		}
    }

    //*************************************************************************
    //  Method: TestSelectedWidthBad()
    //
    /// <summary>
    /// Tests the SelectedWidth property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestSelectedWidthBad()
    {
		try
		{
			m_oEdgeDrawer.SelectedWidth = EdgeDrawer.MinimumWidth - 1;
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual( String.Format(

				"Microsoft.NodeXL.Visualization."
				+ "EdgeDrawer.SelectedWidth: Must be between {0} and {1}."
				,
				EdgeDrawer.MinimumWidth,
				EdgeDrawer.MaximumWidth
				)
				,
				oApplicationException.Message
				);

			throw oApplicationException;
		}
    }

    //*************************************************************************
    //  Method: TestSelectedWidthBad2()
    //
    /// <summary>
    /// Tests the SelectedWidth property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestSelectedWidthBad2()
    {
		try
		{
			m_oEdgeDrawer.SelectedWidth = EdgeDrawer.MaximumWidth + 1;
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual( String.Format(

				"Microsoft.NodeXL.Visualization."
				+ "EdgeDrawer.SelectedWidth: Must be between {0} and {1}."
				,
				EdgeDrawer.MinimumWidth,
				EdgeDrawer.MaximumWidth
				)
				,
				oApplicationException.Message
				);

			throw oApplicationException;
		}
    }

    //*************************************************************************
    //  Method: TestDrawEdge()
    //
    /// <summary>
    /// Tests the DrawEdge() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestDrawEdge()
    {
		foreach ( Int32 iVertex1X in new Int32 [] {-10, 35} )

		foreach ( Int32 iVertex1Y in new Int32 [] {-7, 15} )

		foreach ( Int32 iVertex2X in new Int32 [] {32, 68} )

		foreach ( Int32 iVertex2Y in new Int32 [] {22, 79} )

		foreach ( Int32 iWidth in new Int32 [] {1, 5} )

		foreach ( Int32 iSelectedWidth in new Int32 [] {2, 10} )

		foreach ( Color oColor in new Color [] {Color.Red} )

		foreach ( Color oSelectedColor in new Color [] {Color.Blue} )

		foreach (Boolean bSelected in GraphUtil.AllBoolean)

		foreach (Boolean bDrawArrowOnDirectedEdge in GraphUtil.AllBoolean)
		{
			TestDrawEdge(
				iVertex1X,
				iVertex1Y,
				iVertex2X,
				iVertex2Y,
				iWidth,
				iSelectedWidth,
				oColor,
				oSelectedColor,
				bSelected,
				bDrawArrowOnDirectedEdge
				);
		}
    }

    //*************************************************************************
    //  Method: TestDrawEdgeBad()
    //
    /// <summary>
    /// Tests the DrawEdge() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestDrawEdgeBad()
    {
		// null edge.

		try
		{
			m_oEdgeDrawer.DrawEdge(null, m_oDrawContext);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Visualization."
				+ "EdgeDrawer.DrawEdge: edge argument can't be null.\r\n"
				+ "Parameter name: edge"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestDrawEdgeBad2()
    //
    /// <summary>
    /// Tests the DrawEdge() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestDrawEdgeBad2()
    {
		// null drawContext.

		try
		{
			m_oEdgeDrawer.DrawEdge(m_oEdge, null);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Visualization."
				+ "EdgeDrawer.DrawEdge: drawContext argument can't be"
				+ " null.\r\n"
				+ "Parameter name: drawContext"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: CreateEdge()
    //
    /// <summary>
    /// Creates an edge and adds it to a graph.
    /// </summary>
	///
	/// <param name="iVertex1X">
	/// x-coordinate of first vertex.
	/// </param>
	///
	/// <param name="iVertex1Y">
	/// y-coordinate of first vertex.
	/// </param>
	///
	/// <param name="iVertex2X">
	/// x-coordinate of second vertex.
	/// </param>
	///
	/// <param name="iVertex2Y">
	/// y-coordinate of second vertex.
	/// </param>
	///
	/// <param name="bDirected">
	/// true if the edge should be directed.
	/// </param>
    //*************************************************************************

    protected IEdge
    CreateEdge
	(
		Int32 iVertex1X,
		Int32 iVertex1Y,
		Int32 iVertex2X,
		Int32 iVertex2Y,
		Boolean bDirected
	)
    {
		IGraph oGraph = new Graph(GraphDirectedness.Mixed);

		IVertexCollection oVertices = oGraph.Vertices;

		IVertex oVertex1 = oVertices.Add();

		oVertex1.Location = new Point(iVertex1X, iVertex1Y);

		IVertex oVertex2 = oVertices.Add();

		oVertex2.Location = new Point(iVertex2X, iVertex2Y);

		return ( oGraph.Edges.Add(oVertex1, oVertex2, bDirected) );
    }

    //*************************************************************************
    //  Method: TestDrawEdge()
    //
    /// <summary>
    /// Tests the DrawEdge() method.
    /// </summary>
	///
	/// <param name="iVertex1X">
	/// x-coordinate of first vertex.
	/// </param>
	///
	/// <param name="iVertex1Y">
	/// y-coordinate of second vertex.
	/// </param>
	///
	/// <param name="iVertex2X">
	/// x-coordinate of second vertex.
	/// </param>
	///
	/// <param name="iVertex2Y">
	/// y-coordinate of second vertex.
	/// </param>
	///
	/// <param name="iWidth">
	/// Width of the edge if <paramref name="bSelected" /> is false.
	/// </param>
	///
	/// <param name="iSelectedWidth">
	/// Width of the edge if <paramref name="bSelected" /> is true.
	/// </param>
	///
	/// <param name="oColor">
	/// Color of the edge if <paramref name="bSelected" /> is false.
	/// </param>
	///
	/// <param name="oSelectedColor">
	/// Color of the edge if <paramref name="bSelected" /> is true.
	/// </param>
	///
	/// <param name="bSelected">
	/// true to draw the edge as selected.
	/// </param>
	///
	/// <param name="bDrawArrowOnDirectedEdge">
	/// true to draw an arrow on directed edges.
	/// </param>
    //*************************************************************************

    protected void
    TestDrawEdge
	(
		Int32 iVertex1X,
		Int32 iVertex1Y,
		Int32 iVertex2X,
		Int32 iVertex2Y,
		Int32 iWidth,
		Int32 iSelectedWidth,
		Color oColor,
		Color oSelectedColor,
		Boolean bSelected,
		Boolean bDrawArrowOnDirectedEdge
	)
    {
		Graphics oGraphics = m_oDrawContext.Graphics;

		oGraphics.Clear(Color.White);

		m_oEdge = CreateEdge(iVertex1X, iVertex1Y, iVertex2X, iVertex2Y, true);

		m_oEdgeDrawer.Color = oColor;
		m_oEdgeDrawer.SelectedColor = oSelectedColor;
		m_oEdgeDrawer.Width = iWidth;
		m_oEdgeDrawer.DrawArrowOnDirectedEdge = bDrawArrowOnDirectedEdge;

		MultiSelectionGraphDrawer.SelectEdge(m_oEdge, bSelected);

		m_oEdgeDrawer.DrawEdge(m_oEdge, m_oDrawContext);

		String sTestImageFileNameNoExtension = String.Format(

			"({0})-({1})-({2})-({3})-{4}-{5}-{6}-{7}-{8}-{9}"
			,
			iVertex1X,
			iVertex1Y,
			iVertex2X,
			iVertex2Y,
			iWidth,
			iSelectedWidth,
			oColor,
			oSelectedColor,
			bSelected,
			bDrawArrowOnDirectedEdge
			);

		#if SAVE_TEST_IMAGES

		TestImageUtil.SaveTestImage(m_oBitmap, TestImageRelativeDirectory,
			sTestImageFileNameNoExtension);

		#else

		TestImageUtil.CompareTestImage(m_oBitmap, TestImageRelativeDirectory,
			sTestImageFileNameNoExtension);

		#endif
    }

    //*************************************************************************
    //  Method: EdgeDrawer_RedrawRequired()
    //
    /// <summary>
	/// Handles the RedrawRequired event on the m_oEdgeDrawer object.
    /// </summary>
	///
	/// <param name="oSender">
	/// Standard event argument.
	/// </param>
	///
	/// <param name="oEdgeEventArgs">
	/// Standard event argument.
	/// </param>
    //*************************************************************************

	protected void
	EdgeDrawer_RedrawRequired
	(
		Object oSender,
		EventArgs oEventArgs
	)
	{
		if ( oSender == null || !(oSender is EdgeDrawer) )
		{
			throw new ApplicationException(
				"RedrawRequired event provided incorrect oSender argument."
				);
		}

		m_bRedrawRequired = true;
	}

    //*************************************************************************
    //  Method: EdgeDrawer_LayoutRequired()
    //
    /// <summary>
	/// Handles the LayoutRequired event on the m_oEdgeDrawer object.
    /// </summary>
	///
	/// <param name="oSender">
	/// Standard event argument.
	/// </param>
	///
	/// <param name="oEdgeEventArgs">
	/// Standard event argument.
	/// </param>
    //*************************************************************************

	protected void
	EdgeDrawer_LayoutRequired
	(
		Object oSender,
		EventArgs oEventArgs
	)
	{
		if ( oSender == null || !(oSender is EdgeDrawer) )
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
		"Visualization\\EdgeDrawers\\TestImageFiles\\EdgeDrawer";

	/// Width of m_oBitmap.

	protected const Int32 BitmapWidth = 50;

	/// Height of m_oBitmap.

	protected const Int32 BitmapHeight = 50;

	/// Layout margin.

	protected const Int32 Margin = 6;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected EdgeDrawer m_oEdgeDrawer;

	/// Edge to draw.

	protected IEdge m_oEdge;

	/// DrawContext object to draw with.

	protected DrawContext m_oDrawContext;

	/// Bitmap connected to m_oDrawContext.

	protected Bitmap m_oBitmap;

	/// Gets set by EdgeDrawer_RedrawRequired().

	protected Boolean m_bRedrawRequired;

	/// Gets set by EdgeDrawer_LayoutRequired().

	protected Boolean m_bLayoutRequired;
}

}
