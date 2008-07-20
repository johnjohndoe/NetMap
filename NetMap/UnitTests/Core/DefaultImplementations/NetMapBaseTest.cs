
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.UnitTests
{
//*****************************************************************************
//  Class: NetMapBaseTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="NetMapBase" />
/// class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class NetMapBaseTest : Object
{
    //*************************************************************************
    //  Constructor: NetMapBaseTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="NetMapBaseTest" /> class.
    /// </summary>
    //*************************************************************************

    public NetMapBaseTest()
    {
        m_oNetMapBase = null;
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
        m_oNetMapBase = new NetMapBase();
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
        m_oNetMapBase = null;
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
			"Microsoft.NetMap.Core.NetMapBase",
			m_oNetMapBase.ClassName
			);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected NetMapBase m_oNetMapBase;
}

}
