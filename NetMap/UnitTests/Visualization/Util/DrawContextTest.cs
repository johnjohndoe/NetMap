
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NetMap.Visualization;

namespace Microsoft.NetMap.UnitTests
{
//*****************************************************************************
//  Class: DrawContextTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="DrawContext" />
///	class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class DrawContextTest : Object
{
    //*************************************************************************
    //  Constructor: DrawContextTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="DrawContextTest" /> class.
    /// </summary>
    //*************************************************************************

    public DrawContextTest()
    {
        m_oDrawContext = null;

		m_oGraphDrawer = null;

		m_oGraphics = null;
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
		m_oGraphDrawer = new MockGraphDrawer();

		Bitmap oBitmap = new Bitmap(BitmapWidth, BitmapHeight);

		m_oGraphics = Graphics.FromImage(oBitmap);

		m_oGraphRectangle = new Rectangle(Point.Empty, oBitmap.Size);

		m_oGraphics.Clip = new Region(m_oGraphRectangle);

		m_oDrawContext = new DrawContext(
            m_oGraphDrawer, m_oGraphics, m_oGraphRectangle, Margin);
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
		GraphicsUtil.DisposeGraphics(ref m_oGraphics);

        m_oDrawContext = null;

		m_oGraphDrawer = null;
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
		Assert.AreEqual(m_oGraphDrawer, m_oDrawContext.GraphDrawer);

		Assert.AreEqual(m_oGraphics, m_oDrawContext.Graphics);

		Assert.AreEqual(m_oGraphRectangle, m_oDrawContext.GraphRectangle);

		Assert.AreEqual(
		
			new Rectangle(Margin, Margin,
				BitmapWidth - 2 * Margin, BitmapHeight - 2 * Margin),

			m_oDrawContext.GraphRectangleMinusMargin
			);

		Assert.AreEqual(Margin, m_oDrawContext.Margin);
    }

    //*************************************************************************
    //  Method: TestConstructorBad()
    //
    /// <summary>
    /// Tests the Constructor() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestConstructorBad()
    {
		// null graphDrawer.

		try
		{
			m_oDrawContext = new DrawContext(
                null, m_oGraphics, Rectangle.Empty, Margin);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Visualization."
				+ "DrawContext.Constructor: graphDrawer argument can't be"
				+ " null.\r\n"
				+ "Parameter name: graphDrawer"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestConstructorBad2()
    //
    /// <summary>
    /// Tests the Constructor() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestConstructorBad2()
    {
		// null graphics.

		try
		{
			m_oDrawContext = new DrawContext(
                new MockGraphDrawer(), null, Rectangle.Empty, Margin);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Visualization."
				+ "DrawContext.Constructor: graphics argument can't be"
				+ " null.\r\n"
				+ "Parameter name: graphics"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestConstructorBad3()
    //
    /// <summary>
    /// Tests the Constructor() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestConstructorBad3()
    {
		// Graphics.Clip not set.

		try
		{
			Bitmap oBitmap = new Bitmap(BitmapWidth, BitmapHeight);

			Graphics oGraphics = Graphics.FromImage(oBitmap);

			DrawContext oDrawContext = new DrawContext(
                new MockGraphDrawer(), oGraphics, m_oGraphRectangle, Margin);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Visualization."
				+ "DrawContext.Constructor: The graphics.ClipBounds rectangle"
				+ " differs from the graphRectangle argument.  Set"
				+ " graphics.Clip before calling the constructor.\r\n"
				+ "Parameter name: graphics"
				,
				oArgumentException.Message
				);

			throw oArgumentException;
		}
    }

    //*************************************************************************
    //  Method: TestConstructorBad4()
    //
    /// <summary>
    /// Tests the Constructor() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestConstructorBad4()
    {
		// Graphics.Clip set to shifted rectangle.

		try
		{
			Bitmap oBitmap = new Bitmap(BitmapWidth, BitmapHeight);

			Graphics oGraphics = Graphics.FromImage(oBitmap);

            Rectangle oShiftedGraphRectangle = m_oGraphRectangle;

            oShiftedGraphRectangle.Offset(-1, 0);

            oGraphics.Clip = new Region(oShiftedGraphRectangle);

			DrawContext oDrawContext = new DrawContext(
                new MockGraphDrawer(), oGraphics, m_oGraphRectangle, Margin);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Visualization."
				+ "DrawContext.Constructor: The graphics.ClipBounds rectangle"
				+ " differs from the graphRectangle argument.  Set"
				+ " graphics.Clip before calling the constructor.\r\n"
				+ "Parameter name: graphics"
				,
				oArgumentException.Message
				);

			throw oArgumentException;
		}
    }

    //*************************************************************************
    //  Method: TestConstructorBad5()
    //
    /// <summary>
    /// Tests the Constructor() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestConstructorBad5()
    {
		// Negative margin.

		try
		{
			m_oDrawContext = new DrawContext(
				m_oGraphDrawer, m_oGraphics, m_oGraphRectangle, -1);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Visualization.DrawContext.Constructor: margin"
				+ " argument must be greater than or equal to zero.\r\n"
				+ "Parameter name: margin"
				,
				oArgumentException.Message
				);

			throw oArgumentException;
		}
    }

    //*************************************************************************
    //  Method: TestRectangleMinusMargin()
    //
    /// <summary>
    /// Tests the RectangleMinusMargin property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRectangleMinusMargin()
    {
		// 0 margin.

		m_oDrawContext = new DrawContext(
            m_oGraphDrawer, m_oGraphics, m_oGraphRectangle, 0);

		Assert.AreEqual(m_oGraphRectangle,
			m_oDrawContext.GraphRectangleMinusMargin);
    }

    //*************************************************************************
    //  Method: TestRectangleMinusMargin2()
    //
    /// <summary>
    /// Tests the RectangleMinusMargin property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRectangleMinusMargin2()
    {
		// Almost-empty rectangle minus margin.

		const Int32 Margin2 = BitmapWidth / 2 - 1;

		m_oDrawContext = new DrawContext(
            m_oGraphDrawer, m_oGraphics, m_oGraphRectangle, Margin2);

		Assert.AreEqual(
			Rectangle.FromLTRB(Margin2, Margin2,
				BitmapWidth - Margin2, BitmapHeight - Margin2),

			m_oDrawContext.GraphRectangleMinusMargin
			);
    }

    //*************************************************************************
    //  Method: TestRectangleMinusMargin3()
    //
    /// <summary>
    /// Tests the RectangleMinusMargin property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRectangleMinusMargin3()
    {
		// Empty rectangle minus margin.

		const Int32 Margin2 = BitmapWidth / 2;

		m_oDrawContext = new DrawContext(
            m_oGraphDrawer, m_oGraphics, m_oGraphRectangle, Margin2);

		Assert.AreEqual(Rectangle.Empty,
			m_oDrawContext.GraphRectangleMinusMargin);

		Assert.IsTrue(m_oDrawContext.GraphRectangleMinusMargin.IsEmpty);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	/// Bitmap width.

	protected const Int32 BitmapWidth = 234;

	/// Bitmap height.

	protected const Int32 BitmapHeight = 346;

	protected const Int32 Margin = 6;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected DrawContext m_oDrawContext;

	/// IGraphDrawer object within m_oDrawContext.

	protected IGraphDrawer m_oGraphDrawer;

	/// Graphics object within m_oDrawContext.

	protected Graphics m_oGraphics;

	/// Graph rectangle.

	protected Rectangle m_oGraphRectangle;
}

}
