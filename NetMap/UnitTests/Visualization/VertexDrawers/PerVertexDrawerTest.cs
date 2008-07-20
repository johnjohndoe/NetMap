
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
//  Class: PerVertexDrawerTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="PerVertexDrawer" />
/// class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class PerVertexDrawerTest : Object
{
    //*************************************************************************
    //  Constructor: PerVertexDrawerTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="PerVertexDrawerTest" />
	/// class.
    /// </summary>
    //*************************************************************************

    public PerVertexDrawerTest()
    {
        m_oPerVertexDrawer = null;
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
        m_oPerVertexDrawer = new PerVertexDrawer();

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
        if (m_oPerVertexDrawer != null)
		{
			m_oPerVertexDrawer.Dispose();
			m_oPerVertexDrawer = null;
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
		Assert.IsTrue(m_oPerVertexDrawer.UseSelection);

		Assert.AreEqual(SystemColors.WindowText, m_oPerVertexDrawer.Color);

		Assert.AreEqual(Color.Red, m_oPerVertexDrawer.SelectedColor);

		Assert.AreEqual(3.0F, m_oPerVertexDrawer.Radius);

		Assert.AreEqual(VertexDrawer.VertexShape.Disk,
			m_oPerVertexDrawer.Shape);
    }

    //*************************************************************************
    //  Method: TestPerVertexColors()
    //
    /// <summary>
    /// Tests the DrawVertex() method using per-edge colors and alphas.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestPerVertexColors()
    {
		Graph oGraph = new Graph();
		IVertexCollection oVertices = oGraph.Vertices;

		Graphics oGraphics = m_oDrawContext.Graphics;

		oGraphics.Clear(Color.White);

		IVertex oVertex;

		// Not selected, no color, no transparency.

		oVertex = oVertices.Add();
		oVertex.Location = new Point(10, 10);
		m_oPerVertexDrawer.DrawVertex(oVertex, m_oDrawContext);


		// Not selected, no color, transparency.

		oVertex = oVertices.Add();
		oVertex.Location = new Point(20, 10);
		oVertex.SetValue(ReservedMetadataKeys.PerAlpha, 128);
		m_oPerVertexDrawer.DrawVertex(oVertex, m_oDrawContext);


		// Not selected, color, no transparency.

		oVertex = oVertices.Add();
		oVertex.Location = new Point(30, 10);
		oVertex.SetValue(ReservedMetadataKeys.PerColor, Color.Orange);
		m_oPerVertexDrawer.DrawVertex(oVertex, m_oDrawContext);

		// Not selected, color, transparency.

		oVertex = oVertices.Add();
		oVertex.Location = new Point(40, 10);
		oVertex.SetValue(ReservedMetadataKeys.PerColor, Color.Orange);
		oVertex.SetValue(ReservedMetadataKeys.PerAlpha, 128);
		m_oPerVertexDrawer.DrawVertex(oVertex, m_oDrawContext);

		// Selected, no color, no transparency.

		oVertex = oVertices.Add();
		oVertex.Location = new Point(10, 30);
		MultiSelectionGraphDrawer.SelectVertex(oVertex, true);
		m_oPerVertexDrawer.DrawVertex(oVertex, m_oDrawContext);


		// Selected, no color, transparency.

		oVertex = oVertices.Add();
		oVertex.Location = new Point(20, 30);
		MultiSelectionGraphDrawer.SelectVertex(oVertex, true);
		oVertex.SetValue(ReservedMetadataKeys.PerAlpha, 128);
		m_oPerVertexDrawer.DrawVertex(oVertex, m_oDrawContext);


		// Selected, color, no transparency.

		oVertex = oVertices.Add();
		oVertex.Location = new Point(30, 30);
		MultiSelectionGraphDrawer.SelectVertex(oVertex, true);
		oVertex.SetValue(ReservedMetadataKeys.PerColor, Color.Orange);
		m_oPerVertexDrawer.DrawVertex(oVertex, m_oDrawContext);

		// Selected, color, transparency.

		oVertex = oVertices.Add();
		oVertex.Location = new Point(40, 30);
		MultiSelectionGraphDrawer.SelectVertex(oVertex, true);
		oVertex.SetValue(ReservedMetadataKeys.PerColor, Color.Orange);
		oVertex.SetValue(ReservedMetadataKeys.PerAlpha, 128);
		m_oPerVertexDrawer.DrawVertex(oVertex, m_oDrawContext);


		// Not selected, color, hidden.

		oVertex = oVertices.Add();
		oVertex.Location = new Point(10, 40);
		oVertex.SetValue(ReservedMetadataKeys.PerColor, Color.Orange);
		oVertex.SetValue(ReservedMetadataKeys.Hide, null);
		m_oPerVertexDrawer.DrawVertex(oVertex, m_oDrawContext);

		// Selected, color, hidden.

		oVertex = oVertices.Add();
		oVertex.Location = new Point(20, 40);
		MultiSelectionGraphDrawer.SelectVertex(oVertex, true);
		oVertex.SetValue(ReservedMetadataKeys.PerColor, Color.Orange);
		oVertex.SetValue(ReservedMetadataKeys.Hide, null);
		m_oPerVertexDrawer.DrawVertex(oVertex, m_oDrawContext);


		SaveOrCompareTestImage(m_oBitmap, "TestPerVertexColors");
    }

    //*************************************************************************
    //  Method: TestPerVertexColorsBad()
    //
    /// <summary>
    /// Tests the DrawVertex() method using per-edge colors and alphas.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(FormatException) ) ]

    public void
    TestPerVertexColorsBad()
    {
		// Bad alpha.

		Graph oGraph = new Graph();
		IVertexCollection oVertices = oGraph.Vertices;

		Graphics oGraphics = m_oDrawContext.Graphics;

		oGraphics.Clear(Color.White);

		IVertex oVertex;

		oVertex = oVertices.Add();
		oVertex.SetValue(ReservedMetadataKeys.PerAlpha, -1);

		try
		{
			m_oPerVertexDrawer.DrawVertex(oVertex, m_oDrawContext);
		}
		catch (FormatException oFormatException)
		{
			Assert.AreEqual( String.Format(

				"Microsoft.NetMap.Visualization.PerVertexDrawer: The vertex"
				+ " with the ID {0} has an out-of-range ~PDAlpha value.  Valid"
				+ " values are between 0 and 255."
				,
				oVertex.ID
				)
				,
				oFormatException.Message
				);

			throw oFormatException;
		}
    }

    //*************************************************************************
    //  Method: TestPerVertexColorsBad2()
    //
    /// <summary>
    /// Tests the DrawVertex() method using per-edge colors and alphas.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(FormatException) ) ]

    public void
    TestPerVertexColorsBad2()
    {
		// Bad alpha.

		Graph oGraph = new Graph();
		IVertexCollection oVertices = oGraph.Vertices;

		Graphics oGraphics = m_oDrawContext.Graphics;

		oGraphics.Clear(Color.White);

		IVertex oVertex;

		oVertex = oVertices.Add();
		oVertex.SetValue(ReservedMetadataKeys.PerAlpha, 256);

		try
		{
			m_oPerVertexDrawer.DrawVertex(oVertex, m_oDrawContext);
		}
		catch (FormatException oFormatException)
		{
			Assert.AreEqual( String.Format(

				"Microsoft.NetMap.Visualization.PerVertexDrawer: The vertex"
				+ " with the ID {0} has an out-of-range ~PDAlpha value.  Valid"
				+ " values are between 0 and 255."
				,
				oVertex.ID
				)
				,
				oFormatException.Message
				);

			throw oFormatException;
		}
    }

    //*************************************************************************
    //  Method: TestPerVertexShapes()
    //
    /// <summary>
    /// Tests the DrawVertex() method using per-edge shapes.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestPerVertexShapes()
    {
		Graph oGraph = new Graph();
		IVertexCollection oVertices = oGraph.Vertices;

		Graphics oGraphics = m_oDrawContext.Graphics;

		oGraphics.Clear(Color.White);

		m_oPerVertexDrawer.Radius = 5F;

		IVertex oVertex;

		// Circle.

		oVertex = oVertices.Add();
		oVertex.Location = new Point(10, 20);

		oVertex.SetValue(ReservedMetadataKeys.PerVertexShape,
			VertexDrawer.VertexShape.Circle);

		m_oPerVertexDrawer.DrawVertex(oVertex, m_oDrawContext);


		// Disk.

		oVertex = oVertices.Add();
		oVertex.Location = new Point(20, 20);

		oVertex.SetValue(ReservedMetadataKeys.PerVertexShape,
			VertexDrawer.VertexShape.Disk);

		m_oPerVertexDrawer.DrawVertex(oVertex, m_oDrawContext);


		// Sphere.

		oVertex = oVertices.Add();
		oVertex.Location = new Point(30, 20);

		oVertex.SetValue(ReservedMetadataKeys.PerVertexShape,
			VertexDrawer.VertexShape.Sphere);

		m_oPerVertexDrawer.DrawVertex(oVertex, m_oDrawContext);


		SaveOrCompareTestImage(m_oBitmap, "TestPerVertexShapes");
    }

    //*************************************************************************
    //  Method: TestPerVertexRadii()
    //
    /// <summary>
    /// Tests the DrawVertex() method using per-edge radii.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestPerVertexRadii()
    {
		Graph oGraph = new Graph();
		IVertexCollection oVertices = oGraph.Vertices;

		Graphics oGraphics = m_oDrawContext.Graphics;

		oGraphics.Clear(Color.White);

		IVertex oVertex;

		oVertex = oVertices.Add();
		oVertex.Location = new Point(10, 20);
		oVertex.SetValue(ReservedMetadataKeys.PerVertexRadius, 2F);
		m_oPerVertexDrawer.DrawVertex(oVertex, m_oDrawContext);

		oVertex = oVertices.Add();
		oVertex.Location = new Point(20, 20);
		oVertex.SetValue(ReservedMetadataKeys.PerVertexRadius, 4F);
		m_oPerVertexDrawer.DrawVertex(oVertex, m_oDrawContext);

		oVertex = oVertices.Add();
		oVertex.Location = new Point(40, 20);
		oVertex.SetValue(ReservedMetadataKeys.PerVertexRadius, 5F);
		m_oPerVertexDrawer.DrawVertex(oVertex, m_oDrawContext);

		SaveOrCompareTestImage(m_oBitmap, "TestPerVertexRadii");
    }

    //*************************************************************************
    //  Method: TestPerVertexRadiiBad()
    //
    /// <summary>
    /// Tests the DrawVertex() method using per-edge radii.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(FormatException) ) ]

    public void
    TestPerVertexRadiiBad()
    {
		// Bad radius.

		Graph oGraph = new Graph();
		IVertexCollection oVertices = oGraph.Vertices;

		Graphics oGraphics = m_oDrawContext.Graphics;

		oGraphics.Clear(Color.White);

		IVertex oVertex;

		oVertex = oVertices.Add();

		oVertex.SetValue(ReservedMetadataKeys.PerVertexRadius,
			VertexDrawer.MinimumRadius - 1F);

		try
		{
			m_oPerVertexDrawer.DrawVertex(oVertex, m_oDrawContext);
		}
		catch (FormatException oFormatException)
		{
			Assert.AreEqual( String.Format(

				"Microsoft.NetMap.Visualization.PerVertexDrawer: The vertex"
				+ " with the ID {0} has an out-of-range ~PVDRadius value."
				+ "  Valid values are between {1} and {2}."
				,
				oVertex.ID,
				VertexDrawer.MinimumRadius,
				VertexDrawer.MaximumRadius
				)
				,
				oFormatException.Message
				);

			throw oFormatException;
		}
    }

    //*************************************************************************
    //  Method: TestPerVertexRadiiBad2()
    //
    /// <summary>
    /// Tests the DrawVertex() method using per-edge radii.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(FormatException) ) ]

    public void
    TestPerVertexRadiiBad2()
    {
		// Bad radius.

		Graph oGraph = new Graph();
		IVertexCollection oVertices = oGraph.Vertices;

		Graphics oGraphics = m_oDrawContext.Graphics;

		oGraphics.Clear(Color.White);

		IVertex oVertex;

		oVertex = oVertices.Add();

		oVertex.SetValue(ReservedMetadataKeys.PerVertexRadius,
			VertexDrawer.MaximumRadius + 1F);

		try
		{
			m_oPerVertexDrawer.DrawVertex(oVertex, m_oDrawContext);
		}
		catch (FormatException oFormatException)
		{
			Assert.AreEqual( String.Format(

				"Microsoft.NetMap.Visualization.PerVertexDrawer: The vertex"
				+ " with the ID {0} has an out-of-range ~PVDRadius value."
				+ "  Valid values are between {1} and {2}."
				,
				oVertex.ID,
				VertexDrawer.MinimumRadius,
				VertexDrawer.MaximumRadius
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
    //  Protected constants
    //*************************************************************************

	/// Some unit tests create in-memory bitmaps that are compared to saved
	/// image files.  The image files are stored in the directory specified
	/// by TestImageRelativeDirectory, which is relative to the NetMap\UnitTests
	/// directory.

	protected const String TestImageRelativeDirectory =
		"Visualization\\VertexDrawers\\TestImageFiles\\PerVertexDrawer";

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

    protected PerVertexDrawer m_oPerVertexDrawer;

	/// DrawContext object to draw with.

	protected DrawContext m_oDrawContext;

	/// Bitmap connected to m_oDrawContext.

	protected Bitmap m_oBitmap;
}

}
