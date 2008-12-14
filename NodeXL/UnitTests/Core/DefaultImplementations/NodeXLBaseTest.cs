
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: NodeXLBaseTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="NodeXLBase" />
/// class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class NodeXLBaseTest : Object
{
    //*************************************************************************
    //  Constructor: NodeXLBaseTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="NodeXLBaseTest" /> class.
    /// </summary>
    //*************************************************************************

    public NodeXLBaseTest()
    {
        m_oNodeXLBase = null;
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
        m_oNodeXLBase = new NodeXLBase();
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
        m_oNodeXLBase = null;
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
        // (Do nothing else.)
    }

    //*************************************************************************
    //  Method: TestClassName()
    //
    /// <summary>
    /// Tests the ClassName property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestClassName()
    {
		Assert.AreEqual(
			"Microsoft.NodeXL.Core.NodeXLBase",
			m_oNodeXLBase.ClassName
			);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected NodeXLBase m_oNodeXLBase;
}

}
