
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
//  Class: PerEdgeDrawerTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="PerEdgeDrawer" />
/// class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class PerEdgeDrawerTest : Object
{
    //*************************************************************************
    //  Constructor: PerEdgeDrawerTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="PerEdgeDrawerTest" />
	/// class.
    /// </summary>
    //*************************************************************************

    public PerEdgeDrawerTest()
    {
        m_oPerEdgeDrawer = null;
		m_oBitmap = null;
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
        m_oPerEdgeDrawer = new PerEdgeDrawer();

		m_oBitmap = new Bitmap(BitmapWidth, BitmapHeight);

		Rectangle oGraphRectangle = new Rectangle(Point.Empty, m_oBitmap.Size);

		Graphics oGraphics = Graphics.FromImage(m_oBitmap);

		oGraphics.Clip = new Region(oGraphRectangle);

		m_oDrawContext = new DrawContext(
            new MockGraphDrawer(), oGraphics, oGraphRectangle, Margin);
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
        if (m_oPerEdgeDrawer != null)
		{
			m_oPerEdgeDrawer.Dispose();
			m_oPerEdgeDrawer = null;
		}

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
		Assert.IsTrue(m_oPerEdgeDrawer.UseSelection);

		Assert.AreEqual(SystemColors.WindowText, m_oPerEdgeDrawer.Color);

		Assert.AreEqual(Color.Red, m_oPerEdgeDrawer.SelectedColor);

		Assert.IsTrue(m_oPerEdgeDrawer.DrawArrowOnDirectedEdge);

		Assert.AreEqual(3F, m_oPerEdgeDrawer.RelativeArrowSize);

		Assert.AreEqual(1, m_oPerEdgeDrawer.Width);

		Assert.AreEqual(2, m_oPerEdgeDrawer.SelectedWidth);
    }

    //*************************************************************************
    //  Method: TestPerEdgeColors()
    //
    /// <summary>
    /// Tests the DrawEdge() method using per-edge colors and alphas.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestPerEdgeColors()
    {
		Graph oGraph = new Graph();
		IVertexCollection oVertices = oGraph.Vertices;
		IEdgeCollection oEdges = oGraph.Edges;

		Graphics oGraphics = m_oDrawContext.Graphics;

		oGraphics.Clear(Color.White);

		IVertex oVertex1;
		IVertex oVertex2;
		IEdge oEdge;

		// Not selected, no color, no transparency.

		oVertex1 = oVertices.Add();
		oVertex2 = oVertices.Add();
		oVertex1.Location = new Point(10, 5);
		oVertex2.Location = new Point(40, 5);
		oEdge = oEdges.Add(oVertex1, oVertex2);
		m_oPerEdgeDrawer.DrawEdge(oEdge, m_oDrawContext);


		// Not selected, no color, transparency.

		oVertex1 = oVertices.Add();
		oVertex2 = oVertices.Add();
		oVertex1.Location = new Point(10, 10);
		oVertex2.Location = new Point(40, 10);
		oEdge = oEdges.Add(oVertex1, oVertex2);
		oEdge.SetValue(ReservedMetadataKeys.PerAlpha, 128);
		m_oPerEdgeDrawer.DrawEdge(oEdge, m_oDrawContext);


		// Not selected, color, no transparency.

		oVertex1 = oVertices.Add();
		oVertex2 = oVertices.Add();
		oVertex1.Location = new Point(10, 15);
		oVertex2.Location = new Point(40, 15);
		oEdge = oEdges.Add(oVertex1, oVertex2);
		oEdge.SetValue(ReservedMetadataKeys.PerColor, Color.Orange);
		m_oPerEdgeDrawer.DrawEdge(oEdge, m_oDrawContext);

		// Not selected, color, transparency.

		oVertex1 = oVertices.Add();
		oVertex2 = oVertices.Add();
		oVertex1.Location = new Point(10, 20);
		oVertex2.Location = new Point(40, 20);
		oEdge = oEdges.Add(oVertex1, oVertex2);
		oEdge.SetValue(ReservedMetadataKeys.PerColor, Color.Orange);
		oEdge.SetValue(ReservedMetadataKeys.PerAlpha, 128);
		m_oPerEdgeDrawer.DrawEdge(oEdge, m_oDrawContext);

		// Selected, no color, no transparency.

		oVertex1 = oVertices.Add();
		oVertex2 = oVertices.Add();
		oVertex1.Location = new Point(10, 25);
		oVertex2.Location = new Point(40, 25);
		oEdge = oEdges.Add(oVertex1, oVertex2);
		MultiSelectionGraphDrawer.SelectEdge(oEdge, true);
		m_oPerEdgeDrawer.DrawEdge(oEdge, m_oDrawContext);


		// Selected, no color, transparency.

		oVertex1 = oVertices.Add();
		oVertex2 = oVertices.Add();
		oVertex1.Location = new Point(10, 30);
		oVertex2.Location = new Point(40, 30);
		oEdge = oEdges.Add(oVertex1, oVertex2);
		MultiSelectionGraphDrawer.SelectEdge(oEdge, true);
		oEdge.SetValue(ReservedMetadataKeys.PerAlpha, 128);
		m_oPerEdgeDrawer.DrawEdge(oEdge, m_oDrawContext);


		// Selected, color, no transparency.

		oVertex1 = oVertices.Add();
		oVertex2 = oVertices.Add();
		oVertex1.Location = new Point(10, 35);
		oVertex2.Location = new Point(40, 35);
		oEdge = oEdges.Add(oVertex1, oVertex2);
		MultiSelectionGraphDrawer.SelectEdge(oEdge, true);
		oEdge.SetValue(ReservedMetadataKeys.PerColor, Color.Green);
		m_oPerEdgeDrawer.DrawEdge(oEdge, m_oDrawContext);

		// Selected, color, transparency.

		oVertex1 = oVertices.Add();
		oVertex2 = oVertices.Add();
		oVertex1.Location = new Point(10, 40);
		oVertex2.Location = new Point(40, 40);
		oEdge = oEdges.Add(oVertex1, oVertex2);
		MultiSelectionGraphDrawer.SelectEdge(oEdge, true);
		oEdge.SetValue(ReservedMetadataKeys.PerColor, Color.Green);
		oEdge.SetValue(ReservedMetadataKeys.PerAlpha, 128);
		m_oPerEdgeDrawer.DrawEdge(oEdge, m_oDrawContext);

		// Not selected, color, hidden.

		oVertex1 = oVertices.Add();
		oVertex2 = oVertices.Add();
		oVertex1.Location = new Point(10, 45);
		oVertex2.Location = new Point(40, 45);
		oEdge = oEdges.Add(oVertex1, oVertex2);
		oEdge.SetValue(ReservedMetadataKeys.PerColor, Color.Orange);
		oEdge.SetValue(ReservedMetadataKeys.Hide, null);
		m_oPerEdgeDrawer.DrawEdge(oEdge, m_oDrawContext);

		// Selected, color, hidden.

		oVertex1 = oVertices.Add();
		oVertex2 = oVertices.Add();
		oVertex1.Location = new Point(10, 50);
		oVertex2.Location = new Point(40, 50);
		oEdge = oEdges.Add(oVertex1, oVertex2);
		MultiSelectionGraphDrawer.SelectEdge(oEdge, true);
		oEdge.SetValue(ReservedMetadataKeys.PerColor, Color.Green);
		oEdge.SetValue(ReservedMetadataKeys.Hide, null);
		m_oPerEdgeDrawer.DrawEdge(oEdge, m_oDrawContext);


		SaveOrCompareTestImage(m_oBitmap, "TestPerEdgeColors");
    }

    //*************************************************************************
    //  Method: TestPerEdgeColorsBad()
    //
    /// <summary>
    /// Tests the DrawEdge() method using per-edge colors and alphas.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(FormatException) ) ]

    public void
    TestPerEdgeColorsBad()
    {
		// Bad alpha.

		Graph oGraph = new Graph();
		IVertexCollection oVertices = oGraph.Vertices;
		IEdgeCollection oEdges = oGraph.Edges;

		Graphics oGraphics = m_oDrawContext.Graphics;

		oGraphics.Clear(Color.White);

		IVertex oVertex1;
		IVertex oVertex2;
		IEdge oEdge;

		oVertex1 = oVertices.Add();
		oVertex2 = oVertices.Add();
		oEdge = oEdges.Add(oVertex1, oVertex2);
		oEdge.SetValue(ReservedMetadataKeys.PerAlpha, -1);

		try
		{
			m_oPerEdgeDrawer.DrawEdge(oEdge, m_oDrawContext);
		}
		catch (FormatException oFormatException)
		{
			Assert.AreEqual( String.Format(

				"Microsoft.NetMap.Visualization.PerEdgeDrawer: The edge with"
				+ " the ID {0} has an out-of-range ~PDAlpha value.  Valid"
				+ " values are between 0 and 255."
				,
				oEdge.ID
				)
				,
				oFormatException.Message
				);

			throw oFormatException;
		}
    }

    //*************************************************************************
    //  Method: TestPerEdgeColorsBad2()
    //
    /// <summary>
    /// Tests the DrawEdge() method using per-edge colors and alphas.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(FormatException) ) ]

    public void
    TestPerEdgeColorsBad2()
    {
		// Bad alpha.

		Graph oGraph = new Graph();
		IVertexCollection oVertices = oGraph.Vertices;
		IEdgeCollection oEdges = oGraph.Edges;

		Graphics oGraphics = m_oDrawContext.Graphics;

		oGraphics.Clear(Color.White);

		IVertex oVertex1;
		IVertex oVertex2;
		IEdge oEdge;

		oVertex1 = oVertices.Add();
		oVertex2 = oVertices.Add();
		oEdge = oEdges.Add(oVertex1, oVertex2);
		oEdge.SetValue(ReservedMetadataKeys.PerAlpha, 256);

		try
		{
			m_oPerEdgeDrawer.DrawEdge(oEdge, m_oDrawContext);
		}
		catch (FormatException oFormatException)
		{
			Assert.AreEqual( String.Format(

				"Microsoft.NetMap.Visualization.PerEdgeDrawer: The edge with"
				+ " the ID {0} has an out-of-range ~PDAlpha value.  Valid"
				+ " values are between 0 and 255."
				,
				oEdge.ID
				)
				,
				oFormatException.Message
				);

			throw oFormatException;
		}
    }

    //*************************************************************************
    //  Method: TestPerEdgeWidths()
    //
    /// <summary>
    /// Tests the DrawEdge() method using per-edge widths.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestPerEdgeWidths()
    {
		Graph oGraph = new Graph();
		IVertexCollection oVertices = oGraph.Vertices;
		IEdgeCollection oEdges = oGraph.Edges;

		Graphics oGraphics = m_oDrawContext.Graphics;

		oGraphics.Clear(Color.White);

		IVertex oVertex1;
		IVertex oVertex2;
		IEdge oEdge;

		// Not selected, not customized.

		oVertex1 = oVertices.Add();
		oVertex2 = oVertices.Add();
		oVertex1.Location = new Point(10, 10);
		oVertex2.Location = new Point(40, 10);
		oEdge = oEdges.Add(oVertex1, oVertex2);
		m_oPerEdgeDrawer.DrawEdge(oEdge, m_oDrawContext);


		// Selected, not customized.

		oVertex1 = oVertices.Add();
		oVertex2 = oVertices.Add();
		oVertex1.Location = new Point(10, 20);
		oVertex2.Location = new Point(40, 20);
		oEdge = oEdges.Add(oVertex1, oVertex2);
		MultiSelectionGraphDrawer.SelectEdge(oEdge, true);
		m_oPerEdgeDrawer.DrawEdge(oEdge, m_oDrawContext);

		// Not selected, customized.

		oVertex1 = oVertices.Add();
		oVertex2 = oVertices.Add();
		oVertex1.Location = new Point(10, 30);
		oVertex2.Location = new Point(40, 30);
		oEdge = oEdges.Add(oVertex1, oVertex2);
		oEdge.SetValue(ReservedMetadataKeys.PerEdgeWidth, 5);
		m_oPerEdgeDrawer.DrawEdge(oEdge, m_oDrawContext);

		// Selected, customized.

		oVertex1 = oVertices.Add();
		oVertex2 = oVertices.Add();
		oVertex1.Location = new Point(10, 40);
		oVertex2.Location = new Point(40, 40);
		oEdge = oEdges.Add(oVertex1, oVertex2);
		MultiSelectionGraphDrawer.SelectEdge(oEdge, true);
		oEdge.SetValue(ReservedMetadataKeys.PerEdgeWidth, 7);
		m_oPerEdgeDrawer.DrawEdge(oEdge, m_oDrawContext);

		SaveOrCompareTestImage(m_oBitmap, "TestPerEdgeWidths");
    }

    //*************************************************************************
    //  Method: TestPerEdgeWidthsBad()
    //
    /// <summary>
    /// Tests the DrawEdge() method using per-edge widths.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(FormatException) ) ]

    public void
    TestPerEdgeWidthsBad()
    {
		// Bad width.

		Graph oGraph = new Graph();
		IVertexCollection oVertices = oGraph.Vertices;
		IEdgeCollection oEdges = oGraph.Edges;

		Graphics oGraphics = m_oDrawContext.Graphics;

		oGraphics.Clear(Color.White);

		IVertex oVertex1;
		IVertex oVertex2;
		IEdge oEdge;

		oVertex1 = oVertices.Add();
		oVertex2 = oVertices.Add();
		oEdge = oEdges.Add(oVertex1, oVertex2);

		oEdge.SetValue(ReservedMetadataKeys.PerEdgeWidth,
			PerEdgeDrawer.MinimumWidth - 1);

		try
		{
			m_oPerEdgeDrawer.DrawEdge(oEdge, m_oDrawContext);
		}
		catch (FormatException oFormatException)
		{
			Assert.AreEqual( String.Format(

				"Microsoft.NetMap.Visualization.PerEdgeDrawer: The edge with"
				+ " the ID {0} has an out-of-range ~PEDWidth value.  Valid"
				+ " values are between {1} and {2}."
				,
				oEdge.ID,
				PerEdgeDrawer.MinimumWidth,
				PerEdgeDrawer.MaximumWidth
				)
				,
				oFormatException.Message
				);

			throw oFormatException;
		}
    }

    //*************************************************************************
    //  Method: TestPerEdgeWidthsBad2()
    //
    /// <summary>
    /// Tests the DrawEdge() method using per-edge widths.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(FormatException) ) ]

    public void
    TestPerEdgeWidthsBad2()
    {
		// Bad width.

		Graph oGraph = new Graph();
		IVertexCollection oVertices = oGraph.Vertices;
		IEdgeCollection oEdges = oGraph.Edges;

		Graphics oGraphics = m_oDrawContext.Graphics;

		oGraphics.Clear(Color.White);

		IVertex oVertex1;
		IVertex oVertex2;
		IEdge oEdge;

		oVertex1 = oVertices.Add();
		oVertex2 = oVertices.Add();
		oEdge = oEdges.Add(oVertex1, oVertex2);

		oEdge.SetValue(ReservedMetadataKeys.PerEdgeWidth,
			PerEdgeDrawer.MaximumWidth + 1);

		try
		{
			m_oPerEdgeDrawer.DrawEdge(oEdge, m_oDrawContext);
		}
		catch (FormatException oFormatException)
		{
			Assert.AreEqual( String.Format(

				"Microsoft.NetMap.Visualization.PerEdgeDrawer: The edge with"
				+ " the ID {0} has an out-of-range ~PEDWidth value.  Valid"
				+ " values are between {1} and {2}."
				,
				oEdge.ID,
				PerEdgeDrawer.MinimumWidth,
				PerEdgeDrawer.MaximumWidth
				)
				,
				oFormatException.Message
				);

			throw oFormatException;
		}
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
	/// "DrawEdge1".
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
    //  Protected constants
    //*************************************************************************

	/// Some unit tests create in-memory bitmaps that are compared to saved
	/// image files.  The image files are stored in the directory specified
	/// by TestImageRelativeDirectory, which is relative to the NetMap\UnitTests
	/// directory.

	protected const String TestImageRelativeDirectory =
		"Visualization\\EdgeDrawers\\TestImageFiles\\PerEdgeDrawer";

	/// Width of m_oBitmap.

	protected const Int32 BitmapWidth = 50;

	/// Height of m_oBitmap.

	protected const Int32 BitmapHeight = 80;

	/// Layout margin.

	protected const Int32 Margin = 6;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected PerEdgeDrawer m_oPerEdgeDrawer;

	/// DrawContext object to draw with.

	protected DrawContext m_oDrawContext;

	/// Bitmap connected to m_oDrawContext.

	protected Bitmap m_oBitmap;
}

}
