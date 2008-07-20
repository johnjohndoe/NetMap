
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NetMap.Visualization;

namespace Microsoft.NetMap.UnitTests
{
//*****************************************************************************
//  Class: LayoutContextTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="LayoutContext" />
///	class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class LayoutContextTest : Object
{
    //*************************************************************************
    //  Constructor: LayoutContextTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="LayoutContextTest" /> class.
    /// </summary>
    //*************************************************************************

    public LayoutContextTest()
    {
        m_oLayoutContext = null;

		m_oGraphRectangle = Rectangle.Empty;

		m_oGraphDrawer = null;
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

		Rectangle oGraphRectangle = new Rectangle(
			Point.Empty, new Size(RectangleWidth, RectangleHeight) );

		m_oLayoutContext = new LayoutContext(oGraphRectangle, m_oGraphDrawer);
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
        m_oLayoutContext = null;

		m_oGraphRectangle = Rectangle.Empty;

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
		Assert.AreEqual(new Rectangle(0, 0, RectangleWidth, RectangleHeight),
			m_oLayoutContext.GraphRectangle);

		Assert.AreEqual(m_oGraphDrawer, m_oLayoutContext.GraphDrawer);
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
			m_oLayoutContext = new LayoutContext(Rectangle.Empty, null);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Visualization."
				+ "LayoutContext.Constructor: graphDrawer argument can't be"
				+ " null.\r\n"
				+ "Parameter name: graphDrawer"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	/// Rectangle width.

	protected const Int32 RectangleWidth = 123;

	/// Rectangle height.

	protected const Int32 RectangleHeight = 456;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected LayoutContext m_oLayoutContext;

	/// Rectangle object within m_oLayoutContext.

	protected Rectangle m_oGraphRectangle;

	/// IGraphDrawer object within m_oLayoutContext.

	protected IGraphDrawer m_oGraphDrawer;
}

}
