
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
using Microsoft.NetMap.Visualization;
using Microsoft.NetMap.Core;
using Microsoft.NetMap.Tests;

namespace Microsoft.NetMap.UnitTests
{
//*****************************************************************************
//  Class: SugiyamaVertexDrawerTest
//
/// <summary>
/// This is a Visual Studio test fixture for the
/// <see cref="SugiyamaVertexDrawer" /> class.
/// </summary>
///
/// <remarks>
/// This fixture repeats most of the tests from VertexDrawerTest, then adds
/// tests for the functionality that SugiyamaVertexDrawer adds to VertexDrawer.
/// </remarks>
//*****************************************************************************

[TestClassAttribute]

public class SugiyamaVertexDrawerTest : Object
{
    //*************************************************************************
    //  Constructor: SugiyamaVertexDrawerTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="SugiyamaVertexDrawerTest" /> class.
    /// </summary>
    //*************************************************************************

    public SugiyamaVertexDrawerTest()
    {
        m_oVertexDrawer = null;
		m_oVertex = null;
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
        m_oVertexDrawer = new SugiyamaVertexDrawer();

		m_oVertexDrawer.RedrawRequired +=
			new EventHandler(this.VertexDrawer_RedrawRequired);

		m_oVertexDrawer.LayoutRequired +=
			new EventHandler(this.VertexDrawer_LayoutRequired);

		m_oVertex = ( new Graph() ).Vertices.Add();

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
        if (m_oVertexDrawer != null)
		{
			m_oVertexDrawer.Dispose();
			m_oVertexDrawer = null;
		}

		m_oVertex = null;

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
		Assert.IsTrue(m_oVertexDrawer.UseSelection);

		Assert.AreEqual(SystemColors.WindowText, m_oVertexDrawer.Color);

		Assert.AreEqual(Color.Red, m_oVertexDrawer.SelectedColor);

		Assert.AreEqual(3.0F, m_oVertexDrawer.Radius);

		Assert.AreEqual(VertexDrawer.VertexShape.Disk, m_oVertexDrawer.Shape);

		Assert.IsFalse(m_bRedrawRequired);
		Assert.IsFalse(m_bLayoutRequired);
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
        m_oVertexDrawer.UseSelection = false;
        Assert.IsFalse(m_oVertexDrawer.UseSelection);

        m_oVertexDrawer.UseSelection = true;
        Assert.IsTrue(m_oVertexDrawer.UseSelection);
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
        Color oTestValue = Color.PeachPuff;

		Color oSelectedColor = m_oVertexDrawer.SelectedColor;

        m_oVertexDrawer.Color = oTestValue;
        Assert.AreEqual(oTestValue, m_oVertexDrawer.Color);

		// Make sure the selected color hasn't been modified.

		Assert.AreEqual(oSelectedColor, m_oVertexDrawer.SelectedColor);

		Assert.IsTrue(m_bRedrawRequired);
		Assert.IsFalse(m_bLayoutRequired);

		m_bRedrawRequired = false;

        m_oVertexDrawer.Color = oTestValue;

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
        Color oTestValue = Color.Azure;

		Color oColor = m_oVertexDrawer.Color;

        m_oVertexDrawer.SelectedColor = oTestValue;
        Assert.AreEqual(oTestValue, m_oVertexDrawer.SelectedColor);

		// Make sure the unselected color hasn't been modified.

		Assert.AreEqual(oColor, m_oVertexDrawer.Color);

		Assert.IsTrue(m_bRedrawRequired);
		Assert.IsFalse(m_bLayoutRequired);

		m_bRedrawRequired = false;

        m_oVertexDrawer.SelectedColor = oTestValue;

		Assert.IsFalse(m_bRedrawRequired);
		Assert.IsFalse(m_bLayoutRequired);
    }

    //*************************************************************************
    //  Method: TestRadius()
    //
    /// <summary>
    /// Tests the Radius property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRadius()
    {
		foreach ( Single fTestValue in new Single [] {

			VertexDrawer.MinimumRadius,

			VertexDrawer.MaximumRadius,

			(VertexDrawer.MaximumRadius - VertexDrawer.MinimumRadius) / 2
			} )
		{
			m_oVertexDrawer.Radius = fTestValue;
			Assert.AreEqual(fTestValue, m_oVertexDrawer.Radius);

			Assert.IsFalse(m_bRedrawRequired);
			Assert.IsTrue(m_bLayoutRequired);

			m_bLayoutRequired = false;

			m_oVertexDrawer.Radius = fTestValue;

			Assert.IsFalse(m_bRedrawRequired);
			Assert.IsFalse(m_bLayoutRequired);
		}
    }

    //*************************************************************************
    //  Method: TestRadiusBad()
    //
    /// <summary>
    /// Tests the Radius property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestRadiusBad()
    {
		try
		{
			m_oVertexDrawer.Radius = VertexDrawer.MinimumRadius - 1;
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual( String.Format(

				"Microsoft.NetMap.Visualization."
				+ "SugiyamaVertexDrawer.Radius: Must be between {0} and {1}."
				,
				VertexDrawer.MinimumRadius,
				VertexDrawer.MaximumRadius
				)
				,
				oApplicationException.Message
				);

			throw oApplicationException;
		}
    }

    //*************************************************************************
    //  Method: TestRadiusBad2()
    //
    /// <summary>
    /// Tests the Radius property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestRadiusBad2()
    {
		try
		{
			m_oVertexDrawer.Radius = VertexDrawer.MaximumRadius + 1;
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual( String.Format(

				"Microsoft.NetMap.Visualization."
				+ "SugiyamaVertexDrawer.Radius: Must be between {0} and {1}."
				,
				VertexDrawer.MinimumRadius,
				VertexDrawer.MaximumRadius
				)
				,
				oApplicationException.Message
				);

			throw oApplicationException;
		}
    }

    //*************************************************************************
    //  Method: TestShape()
    //
    /// <summary>
    /// Tests the Shape property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestShape()
    {
		foreach ( VertexDrawer.VertexShape eTestValue in
			Enum.GetValues( typeof(VertexDrawer.VertexShape) ) )
		{
			m_oVertexDrawer.Shape = eTestValue;
			Assert.AreEqual(eTestValue, m_oVertexDrawer.Shape);

			Assert.IsTrue(m_bRedrawRequired);
			Assert.IsFalse(m_bLayoutRequired);

			m_bRedrawRequired = false;

			m_oVertexDrawer.Shape = eTestValue;

			Assert.IsFalse(m_bRedrawRequired);
			Assert.IsFalse(m_bLayoutRequired);
		}
    }

    //*************************************************************************
    //  Method: TestShapeBad()
    //
    /// <summary>
    /// Tests the Shape property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestShapeBad()
    {
		// Invalid directedness.

		try
		{
			m_oVertexDrawer.Shape =
				(VertexDrawer.VertexShape)(-1);
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Visualization."
				+ "SugiyamaVertexDrawer.Shape: Must be a member of the"
                 + " VertexShape enumeration."
				,
				oApplicationException.Message
				);

			throw oApplicationException;
		}
    }
 
	//*************************************************************************
	//  Struct: VertexContainsPointTestData
	//
	/// <summary>
	/// Contains data used to test the VertexContainsPoint() method.
	/// </summary>
	//*************************************************************************

	struct VertexContainsPointTestData
	{
		//*********************************************************************
		//  Constructor: VertexContainsPointTestData()
		//
		/// <summary>
		/// Initializes a new instance of the <see
		/// cref="VertexContainsPointTestData" /> struct.
		/// </summary>
		//*********************************************************************

		public VertexContainsPointTestData
		(
			Int32 iRadius,
			Int32 iVertexX,
			Int32 iVertexY,
			Int32 iPointX,
			Int32 iPointY,
			Boolean bVertexContainsPoint
		)
		{
			Radius = iRadius;
			VertexX = iVertexX;
			VertexY = iVertexY;
			PointX = iPointX;
			PointY = iPointY;
			VertexContainsPoint = bVertexContainsPoint;
		}


		//*********************************************************************
		//  Public fields
		//*********************************************************************

		public Int32 Radius;
		public Int32 VertexX;
		public Int32 VertexY;
		public Int32 PointX;
		public Int32 PointY;
		public Boolean VertexContainsPoint;
	};


    //*************************************************************************
    //  Method: TestVertexContainsPoint()
    //
    /// <summary>
    /// Tests the VertexContainsPoint method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestVertexContainsPoint()
    {
		VertexContainsPointTestData [] aoTestData =
			new VertexContainsPointTestData []
		{
			// Radius, VertexX, VertexY, PointX, PointY, VertexContainsPoint

			new VertexContainsPointTestData(1, 0, 0, 0, 0, true),
			new VertexContainsPointTestData(1, 0, 0, 0, 1, true),
			new VertexContainsPointTestData(1, 0, 0, 1, 0, true),
			new VertexContainsPointTestData(1, 0, 0, 1, 1, false),
			new VertexContainsPointTestData(2, 0, 0, 1, 1, true),

			new VertexContainsPointTestData(1, 1, 1, 1, 1, true),
			new VertexContainsPointTestData(1, 1, 1, 1, 2, true),
			new VertexContainsPointTestData(1, 1, 1, 2, 1, true),
			new VertexContainsPointTestData(1, 1, 1, 2, 2, false),
			new VertexContainsPointTestData(2, 1, 1, 2, 2, true),

			new VertexContainsPointTestData(1, -1, -1, -1, -1, true),
			new VertexContainsPointTestData(1, -1, -1, -1, 0, true),
			new VertexContainsPointTestData(1, -1, -1, 0, -1, true),
			new VertexContainsPointTestData(1, -1, -1, 0, 0, false),
			new VertexContainsPointTestData(2, -1, -1, 0, 0, true),

			new VertexContainsPointTestData(50, 0, 0, 35, 35, true),
			new VertexContainsPointTestData(50, 0, 0, 35, 36, false),
			new VertexContainsPointTestData(50, 0, 0, 36, 35, false),

			new VertexContainsPointTestData(50, 0, 0, -35, -35, true),
			new VertexContainsPointTestData(50, 0, 0, -35, -36, false),
			new VertexContainsPointTestData(50, 0, 0, -36, -35, false),

			new VertexContainsPointTestData(50, 100, 100, 135, 135, true),
			new VertexContainsPointTestData(50, 100, 100, 135, 136, false),
			new VertexContainsPointTestData(50, 100, 100, 136, 135, false),

			new VertexContainsPointTestData(50, 100, 100, 65, 65, true),
			new VertexContainsPointTestData(50, 100, 100, 65, 64, false),
			new VertexContainsPointTestData(50, 100, 100, 64, 65, false),

			new VertexContainsPointTestData(50, -100, -100, -65, -65, true),
			new VertexContainsPointTestData(50, -100, -100, -65, -64, false),
			new VertexContainsPointTestData(50, -100, -100, -64, -65, false),

			new VertexContainsPointTestData(50, -100, -100, -135, -135, true),
			new VertexContainsPointTestData(50, -100, -100, -135, -136, false),
			new VertexContainsPointTestData(50, -100, -100, -136, -135, false),
		};

		Int32 iTestData = aoTestData.Length;

		for (Int32 i = 0; i < iTestData; i++)
		{
			VertexContainsPointTestData oTestData = aoTestData[i];

			Point oPoint = new Point (oTestData.PointX, oTestData.PointY);

			m_oVertexDrawer.Radius = oTestData.Radius;

			m_oVertex.Location = new Point(
				oTestData.VertexX, oTestData.VertexY);

            m_oVertexDrawer.DrawVertex(m_oVertex, m_oDrawContext);

			Boolean bVertexContainsPoint =
				m_oVertexDrawer.VertexContainsPoint(m_oVertex, oPoint);

			Assert.AreEqual(
				oTestData.VertexContainsPoint, bVertexContainsPoint, 

				"Failed on aoTestData[{0}]."
				,
				i
				);
		}
    }

    //*************************************************************************
    //  Method: TestVertexContainsPoint2()
    //
    /// <summary>
    /// Tests the VertexContainsPoint method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestVertexContainsPoint2()
    {
		// Test without setting the SugiyamaLayout metadata on the graph,
		// which forces SugiyamaVertexDrawer to route VertexContainsPoint()
		// calls to its VertexDrawer base class.

		Point oPoint = new Point(2, 2);

		m_oVertexDrawer.Radius = 1;

		m_oVertex.Location = Point.Empty;

		Boolean bVertexContainsPoint =
			m_oVertexDrawer.VertexContainsPoint(m_oVertex, oPoint);

		Assert.IsFalse(bVertexContainsPoint);

		// Now set the metadata.

		m_oVertex.ParentGraph.SetValue(
            ReservedMetadataKeys.SugiyamaComputedRadius, 4.0F);

        m_oVertexDrawer.DrawVertex(m_oVertex, m_oDrawContext);

		bVertexContainsPoint =
			m_oVertexDrawer.VertexContainsPoint(m_oVertex, oPoint);

		Assert.IsTrue(bVertexContainsPoint);
    }

    //*************************************************************************
    //  Method: TestVertexContainsPointBad()
    //
    /// <summary>
    /// Tests the VertexContainsPoint() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestVertexContainsPointBad()
    {
		// null vertex.

		try
		{
			Point oPoint = new Point (0, 0);

			m_oVertexDrawer.VertexContainsPoint(null, oPoint);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Visualization."
				+ "SugiyamaVertexDrawer.VertexContainsPoint: vertex argument"
				+ " can't be null.\r\n"
				+ "Parameter name: vertex"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestDrawVertex()
    //
    /// <summary>
    /// Tests the DrawVertex() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestDrawVertex()
    {
		foreach (
			VertexDrawer.VertexShape eShape in
			Enum.GetValues( typeof(VertexDrawer.VertexShape) )
			)

		foreach ( Int32 iX in new Int32 [] {25} )

		foreach ( Int32 iY in new Int32 [] {27} )

		foreach ( Int32 iRadius in new Int32 [] {10} )

		foreach ( Color oColor in new Color [] {Color.Red} )

		foreach ( Color oSelectedColor in new Color [] {Color.Blue} )

		foreach (Boolean bSelected in GraphUtil.AllBoolean)

		foreach (Boolean bSetMetadata in GraphUtil.AllBoolean)
		{
			TestDrawVertex(
				eShape,
				iX,
				iY,
				iRadius,
				oColor,
				oSelectedColor,
				bSelected,
				bSetMetadata
				);
		}
    }

    //*************************************************************************
    //  Method: TestDrawVertexBad()
    //
    /// <summary>
    /// Tests the DrawVertex() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestDrawVertexBad()
    {
		// null vertex.

		try
		{
			m_oVertexDrawer.DrawVertex(null, m_oDrawContext);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Visualization."
				+ "SugiyamaVertexDrawer.DrawVertex: vertex argument can't be"
                + " null.\r\n"
				+ "Parameter name: vertex"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestDrawVertexBad2()
    //
    /// <summary>
    /// Tests the DrawVertex() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestDrawVertexBad2()
    {
		// null drawContext.

		try
		{
			m_oVertexDrawer.DrawVertex(m_oVertex, null);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Visualization."
				+ "SugiyamaVertexDrawer.DrawVertex: drawContext argument can't"
				+ " be null.\r\n"
				+ "Parameter name: drawContext"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestDrawVertex()
    //
    /// <summary>
    /// Tests the DrawVertex() method.
    /// </summary>
	///
	/// <param name="eShape">
	/// Shape of the vertex.
	/// </param>
	///
	/// <param name="iX">
	/// x-coordinate of the vertex.
	/// </param>
	///
	/// <param name="iY">
	/// y-coordinate of the vertex.
	/// </param>
	///
	/// <param name="iRadius">
	/// Radius of the vertex if <paramref name="bSelected" /> is false.
	/// </param>
	///
	/// <param name="oColor">
	/// Color of the vertex if <paramref name="bSelected" /> is false.
	/// </param>
	///
	/// <param name="oSelectedColor">
	/// Color of the vertex if <paramref name="bSelected" /> is true.
	/// </param>
	///
	/// <param name="bSelected">
	/// true to draw the vertex as selected.
	/// </param>
	///
	/// <param name="bSetMetadata">
	/// true to store metadata on the graph that specifies the computed radius
	/// to draw.
	/// </param>
    //*************************************************************************

    protected void
    TestDrawVertex
	(
		VertexDrawer.VertexShape eShape,
		Int32 iX,
		Int32 iY,
		Int32 iRadius,
		Color oColor,
		Color oSelectedColor,
		Boolean bSelected,
		Boolean bSetMetadata
	)
    {
		Graphics oGraphics = m_oDrawContext.Graphics;

		oGraphics.Clear(Color.White);

		m_oVertex.Location = new Point(iX, iY);

		m_oVertexDrawer.Color = oColor;
		m_oVertexDrawer.SelectedColor = oSelectedColor;
		m_oVertexDrawer.Radius = iRadius;
		m_oVertexDrawer.Shape = eShape;

		MultiSelectionGraphDrawer.SelectVertex(m_oVertex, bSelected);

        if (bSetMetadata)
        {
            m_oVertex.ParentGraph.SetValue(
                ReservedMetadataKeys.SugiyamaComputedRadius,
				(Single)(2 * iRadius)
				);
        }
        else
        {
            m_oVertex.ParentGraph.RemoveKey(
				ReservedMetadataKeys.SugiyamaComputedRadius);
        }

		m_oVertexDrawer.DrawVertex(m_oVertex, m_oDrawContext);

		String sTestImageFileNameNoExtension = String.Format(

			"{0}-({1})-({2})-{3}-{4}-{5}-{6}-{7}"
			,
			eShape,
			iX,
			iY,
			iRadius,
			oColor,
			oSelectedColor,
			bSelected,
			bSetMetadata
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
    //  Method: VertexDrawer_RedrawRequired()
    //
    /// <summary>
	/// Handles the RedrawRequired event on the m_oVertexDrawer object.
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
	VertexDrawer_RedrawRequired
	(
		Object oSender,
		EventArgs oEventArgs
	)
	{
		if ( oSender == null || !(oSender is VertexDrawer) )
		{
			throw new ApplicationException(
				"RedrawRequired event provided incorrect oSender argument."
				);
		}

		m_bRedrawRequired = true;
	}

    //*************************************************************************
    //  Method: VertexDrawer_LayoutRequired()
    //
    /// <summary>
	/// Handles the LayoutRequired event on the m_oVertexDrawer object.
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
	VertexDrawer_LayoutRequired
	(
		Object oSender,
		EventArgs oEventArgs
	)
	{
		if ( oSender == null || !(oSender is VertexDrawer) )
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
	/// by TestImageRelativeDirectory, which is relative to the NetMap\UnitTests
	/// directory.

	protected const String TestImageRelativeDirectory =
		"Visualization\\VertexDrawers\\TestImageFiles\\SugiyamaVertexDrawer";

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

    protected SugiyamaVertexDrawer m_oVertexDrawer;

	/// Vertex to draw.

	protected IVertex m_oVertex;

	/// DrawContext object to draw with.

	protected DrawContext m_oDrawContext;

	/// Bitmap connected to m_oDrawContext.

	protected Bitmap m_oBitmap;

	/// Gets set by VertexDrawer_RedrawRequired().

	protected Boolean m_bRedrawRequired;

	/// Gets set by VertexDrawer_LayoutRequired().

	protected Boolean m_bLayoutRequired;
}

}
