
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NetMap.Core;
using Microsoft.NetMap.ApplicationUtil;

namespace Microsoft.NetMap.UnitTests
{
//*****************************************************************************
//  Class: RequestSelectionEventArgsTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="RequestSelectionEventArgs" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class RequestSelectionEventArgsTest : Object
{
    //*************************************************************************
    //  Constructor: RequestSelectionEventArgsTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="RequestSelectionEventArgsTest" /> class.
    /// </summary>
    //*************************************************************************

    public RequestSelectionEventArgsTest()
    {
        m_oRequestSelectionEventArgs = null;
		m_aoVertices = null;
		m_aoEdges = null;
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
		IGraph oGraph = new Graph();
		IVertexCollection oVertices = oGraph.Vertices;
		IEdgeCollection oEdges = oGraph.Edges;

		m_aoVertices = new IVertex [] {
			oVertices.Add(),
			oVertices.Add()
			};

		m_aoEdges = new IEdge [] {
			oEdges.Add( m_aoVertices[0], m_aoVertices[1] )
			};

        m_oRequestSelectionEventArgs =
			new RequestSelectionEventArgs(m_aoVertices, m_aoEdges);
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
        m_oRequestSelectionEventArgs = null;
		m_aoVertices = null;
		m_aoEdges = null;
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
		Assert.AreEqual(m_aoVertices, m_oRequestSelectionEventArgs.Vertices);
		Assert.AreEqual(m_aoEdges, m_oRequestSelectionEventArgs.Edges);
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
		// Null vertices.

		try
		{
			m_oRequestSelectionEventArgs =
				new RequestSelectionEventArgs(null, m_aoEdges);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.ApplicationUtil."
				+ "RequestSelectionEventArgs.Constructor: vertices argument"
				+ " can't be null.\r\n"
				+ "Parameter name: vertices"
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
    /// Tests the constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestConstructorBad2()
    {
		// Null edges.

		try
		{
			m_oRequestSelectionEventArgs =
				new RequestSelectionEventArgs(m_aoVertices, null);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

                "Microsoft.NetMap.ApplicationUtil."
				+ "RequestSelectionEventArgs.Constructor: edges argument"
				+ " can't be null.\r\n"
				+ "Parameter name: edges"
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

    protected RequestSelectionEventArgs m_oRequestSelectionEventArgs;

	/// Vertices in m_oRequestSelectionEventArgs.

	protected IVertex [] m_aoVertices;

	/// Edges in m_oRequestSelectionEventArgs.

	protected IEdge [] m_aoEdges;
}

}
