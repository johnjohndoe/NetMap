
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Visualization;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: DraggedVertexTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="DraggedVertex" />
/// class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class DraggedVertexTest : Object
{
    //*************************************************************************
    //  Constructor: DraggedVertexTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="DraggedVertexTest" />
	/// class.
    /// </summary>
    //*************************************************************************

    public DraggedVertexTest()
    {
        m_oDraggedVertex = null;
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
		m_oVertex = ( new VertexFactory() ).CreateVertex();

        m_oDraggedVertex = new DraggedVertex(m_oVertex, MouseDownLocation);
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
		m_oVertex = null;
        m_oDraggedVertex = null;
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
		Assert.AreEqual(m_oVertex, m_oDraggedVertex.Vertex);

		Assert.AreEqual(MouseDownLocation, m_oDraggedVertex.MouseDownLocation);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	protected static readonly Point MouseDownLocation = new Point(123, 456);


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected DraggedVertex m_oDraggedVertex;

	/// IVertex to use.

	protected IVertex m_oVertex;
}

}
