
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
using Microsoft.NodeXL.Visualization;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Common;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: PerVertexWithLabelDrawerTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="PerVertexWithLabelDrawer" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class PerVertexWithLabelDrawerTest : Object
{
    //*************************************************************************
    //  Constructor: PerVertexWithLabelDrawerTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="PerVertexWithLabelDrawerTest" /> class.
    /// </summary>
    //*************************************************************************

    public PerVertexWithLabelDrawerTest()
    {
        m_oPerVertexWithLabelDrawer = null;
		m_oBitmap = null;
		m_oDrawContext = null;
		m_oVertex = null;
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
        m_oPerVertexWithLabelDrawer = new PerVertexWithLabelDrawer();

		m_oBitmap = new Bitmap(BitmapWidth, BitmapHeight);

		Rectangle oGraphRectangle = new Rectangle(Point.Empty, m_oBitmap.Size);

		Graphics oGraphics = Graphics.FromImage(m_oBitmap);

		oGraphics.Clip = new Region(oGraphRectangle);

		oGraphics.Clear(Color.Gray);

		m_oDrawContext = new DrawContext(
            new MockGraphDrawer(), oGraphics, oGraphRectangle, Margin);

		Graph oGraph = new Graph();

		m_oVertex = oGraph.Vertices.Add();
		m_oVertex.Location = new Point(BitmapWidth / 2, BitmapHeight / 2);
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
        if (m_oPerVertexWithLabelDrawer != null)
		{
			m_oPerVertexWithLabelDrawer.Dispose();
			m_oPerVertexWithLabelDrawer = null;
		}

        if (m_oBitmap != null)
		{
			m_oBitmap.Dispose();
			m_oBitmap = null;
		}

		m_oDrawContext = null;
		m_oVertex = null;
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
		Assert.IsTrue(m_oPerVertexWithLabelDrawer.UseSelection);

		Assert.AreEqual(SystemColors.WindowText,
			m_oPerVertexWithLabelDrawer.Color);

		Assert.AreEqual(Color.Red, m_oPerVertexWithLabelDrawer.SelectedColor);

		Assert.AreEqual(3.0F, m_oPerVertexWithLabelDrawer.Radius);

		Assert.AreEqual(VertexDrawer.VertexShape.Disk,
			m_oPerVertexWithLabelDrawer.Shape);
    }

    //*************************************************************************
    //  Method: TestPerVertexPrimaryLabel()
    //
    /// <summary>
    /// Tests the DrawVertex() method using a primary label.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestPerVertexPrimaryLabel()
    {
		// Average-length label.

		m_oVertex.SetValue(ReservedMetadataKeys.PerVertexPrimaryLabel,
			"Primary label");

		m_oPerVertexWithLabelDrawer.PreDrawVertex(m_oVertex, m_oDrawContext);
		m_oPerVertexWithLabelDrawer.DrawVertex(m_oVertex, m_oDrawContext);

		SaveOrCompareTestImage(m_oBitmap, "TestPerVertexPrimaryLabel");
    }

    //*************************************************************************
    //  Method: TestPerVertexPrimaryLabel2()
    //
    /// <summary>
    /// Tests the DrawVertex() method using a primary label.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestPerVertexPrimaryLabel2()
    {
		// Long label.

		m_oVertex.SetValue(ReservedMetadataKeys.PerVertexPrimaryLabel,
			"This is a long long long label with a lot of text.");

		m_oPerVertexWithLabelDrawer.PreDrawVertex(m_oVertex, m_oDrawContext);
		m_oPerVertexWithLabelDrawer.DrawVertex(m_oVertex, m_oDrawContext);

		SaveOrCompareTestImage(m_oBitmap, "TestPerVertexPrimaryLabel2");
    }

    //*************************************************************************
    //  Method: TestPerVertexPrimaryLabel3()
    //
    /// <summary>
    /// Tests the DrawVertex() method using a primary label.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestPerVertexPrimaryLabel3()
    {
		// Null label.

		m_oVertex.SetValue(ReservedMetadataKeys.PerVertexPrimaryLabel, null);

		m_oPerVertexWithLabelDrawer.PreDrawVertex(m_oVertex, m_oDrawContext);
		m_oPerVertexWithLabelDrawer.DrawVertex(m_oVertex, m_oDrawContext);

		SaveOrCompareTestImage(m_oBitmap, "TestPerVertexPrimaryLabel3");
    }

    //*************************************************************************
    //  Method: TestPerVertexPrimaryLabel4()
    //
    /// <summary>
    /// Tests the DrawVertex() method using a primary label.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestPerVertexPrimaryLabel4()
    {
		// Empty label.

		m_oVertex.SetValue(ReservedMetadataKeys.PerVertexPrimaryLabel,
			String.Empty);

		m_oPerVertexWithLabelDrawer.PreDrawVertex(m_oVertex, m_oDrawContext);
		m_oPerVertexWithLabelDrawer.DrawVertex(m_oVertex, m_oDrawContext);

		SaveOrCompareTestImage(m_oBitmap, "TestPerVertexPrimaryLabel4");
    }

    //*************************************************************************
    //  Method: TestPerVertexPrimaryLabel5()
    //
    /// <summary>
    /// Tests the DrawVertex() method using a primary label.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestPerVertexPrimaryLabel5()
    {
		// Long single-word label.

		m_oVertex.SetValue(ReservedMetadataKeys.PerVertexPrimaryLabel,
			"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");

		m_oPerVertexWithLabelDrawer.PreDrawVertex(m_oVertex, m_oDrawContext);
		m_oPerVertexWithLabelDrawer.DrawVertex(m_oVertex, m_oDrawContext);

		SaveOrCompareTestImage(m_oBitmap, "TestPerVertexPrimaryLabel5");
    }

    //*************************************************************************
    //  Method: TestPerVertexPrimaryLabel6()
    //
    /// <summary>
    /// Tests the DrawVertex() method using a primary label.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestPerVertexPrimaryLabel6()
    {
		// Selected vertex.

		MultiSelectionGraphDrawer.SelectVertex(m_oVertex);

		m_oVertex.SetValue(ReservedMetadataKeys.PerVertexPrimaryLabel,
			"Primary label");

		m_oPerVertexWithLabelDrawer.PreDrawVertex(m_oVertex, m_oDrawContext);
		m_oPerVertexWithLabelDrawer.DrawVertex(m_oVertex, m_oDrawContext);

		SaveOrCompareTestImage(m_oBitmap, "TestPerVertexPrimaryLabel6");
    }

    //*************************************************************************
    //  Method: TestPerVertexPrimaryLabel7()
    //
    /// <summary>
    /// Tests the DrawVertex() method using a primary label.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestPerVertexPrimaryLabel7()
    {
		// Change default fill color.

		m_oVertex.SetValue(ReservedMetadataKeys.PerVertexPrimaryLabel,
			"Primary label");

		m_oPerVertexWithLabelDrawer.PrimaryLabelFillColor = Color.Orange;

		m_oPerVertexWithLabelDrawer.PreDrawVertex(m_oVertex, m_oDrawContext);
		m_oPerVertexWithLabelDrawer.DrawVertex(m_oVertex, m_oDrawContext);

		SaveOrCompareTestImage(m_oBitmap, "TestPerVertexPrimaryLabel7");
    }

    //*************************************************************************
    //  Method: TestPerVertexPrimaryLabel8()
    //
    /// <summary>
    /// Tests the DrawVertex() method using a primary label.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestPerVertexPrimaryLabel8()
    {
		// Change default text color.

		m_oVertex.SetValue(ReservedMetadataKeys.PerVertexPrimaryLabel,
			"Primary label");

		m_oPerVertexWithLabelDrawer.Color = Color.Green;

		m_oPerVertexWithLabelDrawer.PreDrawVertex(m_oVertex, m_oDrawContext);
		m_oPerVertexWithLabelDrawer.DrawVertex(m_oVertex, m_oDrawContext);

		SaveOrCompareTestImage(m_oBitmap, "TestPerVertexPrimaryLabel8");
    }

    //*************************************************************************
    //  Method: TestPerVertexPrimaryLabel9()
    //
    /// <summary>
    /// Tests the DrawVertex() method using a primary label.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestPerVertexPrimaryLabel9()
    {
		// Change per-vertex fill color.

		m_oVertex.SetValue(ReservedMetadataKeys.PerVertexPrimaryLabel,
			"Primary label");

		m_oVertex.SetValue(ReservedMetadataKeys.PerVertexPrimaryLabelFillColor,
			Color.Yellow);

		m_oPerVertexWithLabelDrawer.PreDrawVertex(m_oVertex, m_oDrawContext);
		m_oPerVertexWithLabelDrawer.DrawVertex(m_oVertex, m_oDrawContext);

		SaveOrCompareTestImage(m_oBitmap, "TestPerVertexPrimaryLabel9");
    }

    //*************************************************************************
    //  Method: TestPerVertexPrimaryLabel10()
    //
    /// <summary>
    /// Tests the DrawVertex() method using a primary label.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestPerVertexPrimaryLabel10()
    {
		// Change per-vertex text color.

		m_oVertex.SetValue(ReservedMetadataKeys.PerVertexPrimaryLabel,
			"Primary label");

		m_oVertex.SetValue(ReservedMetadataKeys.PerColor, Color.Orange);

		m_oPerVertexWithLabelDrawer.PreDrawVertex(m_oVertex, m_oDrawContext);
		m_oPerVertexWithLabelDrawer.DrawVertex(m_oVertex, m_oDrawContext);

		SaveOrCompareTestImage(m_oBitmap, "TestPerVertexPrimaryLabel10");
    }

    //*************************************************************************
    //  Method: TestPerVertexPrimaryLabel11()
    //
    /// <summary>
    /// Tests the DrawVertex() method using a primary label.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestPerVertexPrimaryLabel11()
    {
		// Change font.

		m_oVertex.SetValue(ReservedMetadataKeys.PerVertexPrimaryLabel,
			"Primary label");

		m_oPerVertexWithLabelDrawer.Font = new Font("Courier New", 15F);

		m_oPerVertexWithLabelDrawer.PreDrawVertex(m_oVertex, m_oDrawContext);
		m_oPerVertexWithLabelDrawer.DrawVertex(m_oVertex, m_oDrawContext);

		SaveOrCompareTestImage(m_oBitmap, "TestPerVertexPrimaryLabel11");
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
	/// by TestImageRelativeDirectory, which is relative to the NodeXL\UnitTests
	/// directory.

	protected const String TestImageRelativeDirectory =
		"Visualization\\VertexDrawers\\TestImageFiles\\"
		+ "PerVertexWithLabelDrawer";

	/// Width of m_oBitmap.

	protected const Int32 BitmapWidth = 120;

	/// Height of m_oBitmap.

	protected const Int32 BitmapHeight = 120;

	/// Layout margin.

	protected const Int32 Margin = 6;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected PerVertexWithLabelDrawer m_oPerVertexWithLabelDrawer;

	/// DrawContext object to draw with.

	protected DrawContext m_oDrawContext;

	/// Bitmap connected to m_oDrawContext.

	protected Bitmap m_oBitmap;

	/// Vertex to draw.

	protected IVertex m_oVertex;
}

}
