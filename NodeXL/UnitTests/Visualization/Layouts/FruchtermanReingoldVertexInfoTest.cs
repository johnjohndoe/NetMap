
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.Visualization;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: FruchtermanReingoldVertexInfoTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="FruchtermanReingoldVertexInfo" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class FruchtermanReingoldVertexInfoTest : Object
{
    //*************************************************************************
    //  Constructor: FruchtermanReingoldVertexInfoTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="FruchtermanReingoldVertexInfoTest" /> class.
    /// </summary>
    //*************************************************************************

    public FruchtermanReingoldVertexInfoTest()
    {
        m_oFruchtermanReingoldVertexInfo = null;
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
        m_oFruchtermanReingoldVertexInfo = new FruchtermanReingoldVertexInfo(
			new PointF(InitialLocationX, InitialLocationY) );
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
        m_oFruchtermanReingoldVertexInfo = null;
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
        Assert.AreEqual(InitialLocationX,
			m_oFruchtermanReingoldVertexInfo.UnboundedLocationX);

        Assert.AreEqual(InitialLocationY,
			m_oFruchtermanReingoldVertexInfo.UnboundedLocationY);

        Assert.AreEqual(0F, m_oFruchtermanReingoldVertexInfo.DisplacementX);

        Assert.AreEqual(0F, m_oFruchtermanReingoldVertexInfo.DisplacementY);
    }

    //*************************************************************************
    //  Method: TestUnboundedLocationX()
    //
    /// <summary>
    /// Tests the UnboundedLocationX property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestUnboundedLocationX()
    {
        const Single TestValue = 4783.3456F;

        m_oFruchtermanReingoldVertexInfo.UnboundedLocationX = TestValue;

        Assert.AreEqual(TestValue,
			m_oFruchtermanReingoldVertexInfo.UnboundedLocationX);
    }

    //*************************************************************************
    //  Method: TestUnboundedLocationY()
    //
    /// <summary>
    /// Tests the UnboundedLocationY property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestUnboundedLocationY()
    {
        const Single TestValue = -76.3458F;

        m_oFruchtermanReingoldVertexInfo.UnboundedLocationY = TestValue;

        Assert.AreEqual(TestValue,
			m_oFruchtermanReingoldVertexInfo.UnboundedLocationY);
    }

    //*************************************************************************
    //  Method: TestDisplacementX()
    //
    /// <summary>
    /// Tests the DisplacementX property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestDisplacementX()
    {
        const Single TestValue = 183.3F;

        m_oFruchtermanReingoldVertexInfo.DisplacementX = TestValue;

        Assert.AreEqual(TestValue,
			m_oFruchtermanReingoldVertexInfo.DisplacementX);
    }

    //*************************************************************************
    //  Method: TestDisplacementY()
    //
    /// <summary>
    /// Tests the DisplacementY property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestDisplacementY()
    {
        const Single TestValue = 345.34F;

        m_oFruchtermanReingoldVertexInfo.DisplacementY = TestValue;

        Assert.AreEqual(TestValue,
			m_oFruchtermanReingoldVertexInfo.DisplacementY);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	protected Single InitialLocationX = 123.456F;

	protected Single InitialLocationY = -765.432F;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected FruchtermanReingoldVertexInfo m_oFruchtermanReingoldVertexInfo;
}

}
