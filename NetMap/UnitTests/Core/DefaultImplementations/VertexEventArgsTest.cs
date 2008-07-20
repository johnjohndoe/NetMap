
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.UnitTests
{
//*****************************************************************************
//  Class: VertexEventArgsTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="VertexEventArgs" />
/// class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class VertexEventArgsTest : Object
{
    //*************************************************************************
    //  Constructor: VertexEventArgsTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexEventArgsTest" />
	/// class.
    /// </summary>
    //*************************************************************************

    public VertexEventArgsTest()
    {
        m_oVertexEventArgs = null;
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
		VertexFactory oVertexFactory = new VertexFactory();

		m_oVertex = oVertexFactory.CreateVertex();

        m_oVertexEventArgs = new VertexEventArgs(m_oVertex);
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
        m_oVertexEventArgs = null;
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
		Assert.AreEqual(m_oVertex, m_oVertexEventArgs.Vertex);
    }

    //*************************************************************************
    //  Method: TestConstructorBad()
    //
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestConstructorBad()
    {
		// Null argument.

		try
		{
			m_oVertexEventArgs = new VertexEventArgs(null);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Core."
				+ "VertexEventArgs.Constructor: vertex argument can't be"
				+ " null.\r\n"
				+ "Parameter name: vertex"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected VertexEventArgs m_oVertexEventArgs;

	/// Vertex in m_oVertexEventArgs.

	protected IVertex m_oVertex;
}

}
