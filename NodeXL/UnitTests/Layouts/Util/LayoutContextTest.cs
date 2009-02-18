
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.Layouts;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: LayoutContextTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="LayoutContext" />
/// class.
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
        Rectangle oGraphRectangle = new Rectangle(
            Point.Empty, new Size(RectangleWidth, RectangleHeight) );

        m_oLayoutContext = new LayoutContext(oGraphRectangle);
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
}

}
