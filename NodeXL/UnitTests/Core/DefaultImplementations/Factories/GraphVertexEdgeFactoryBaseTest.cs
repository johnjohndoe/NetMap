
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: GraphVertexEdgeFactoryBaseTest
//
/// <summary>
/// This is Visual Studio test fixture for the <see
/// cref="GraphVertexEdgeFactoryBase" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class GraphVertexEdgeFactoryBaseTest : Object
{
    //*************************************************************************
    //  Constructor: GraphVertexEdgeFactoryBaseTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="GraphVertexEdgeFactoryBaseTest" /> class.
    /// </summary>
    //*************************************************************************

    public GraphVertexEdgeFactoryBaseTest()
    {
        m_oGraphVertexEdgeFactoryBase = null;
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
        m_oGraphVertexEdgeFactoryBase = new GraphVertexEdgeFactoryBase();
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
        m_oGraphVertexEdgeFactoryBase = null;
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
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected GraphVertexEdgeFactoryBase m_oGraphVertexEdgeFactoryBase;
}

}
