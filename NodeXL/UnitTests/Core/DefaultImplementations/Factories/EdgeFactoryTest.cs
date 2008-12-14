
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Common;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: EdgeFactoryTest
//
/// <summary>
/// This is Visual Studio test fixture for the <see cref="EdgeFactory" />
/// class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class EdgeFactoryTest : Object
{
    //*************************************************************************
    //  Constructor: EdgeFactoryTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EdgeFactoryTest" />
	/// class.
    /// </summary>
    //*************************************************************************

    public EdgeFactoryTest()
    {
        m_oEdgeFactory = null;
		m_oVertex1 = null;
		m_oVertex2 = null;
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
        m_oEdgeFactory = new EdgeFactory();

		// Add two vertices to a graph.

		VertexFactory oVertexFactory = new VertexFactory();

		m_oVertex1 = oVertexFactory.CreateVertex();
		m_oVertex2 = oVertexFactory.CreateVertex();

		IGraph oGraph = new Graph();

		oGraph.Vertices.Add(m_oVertex1);
		oGraph.Vertices.Add(m_oVertex2);
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
        m_oEdgeFactory = null;
		m_oVertex1 = null;
		m_oVertex2 = null;
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
    //  Method: TestCreateEdge()
    //
    /// <summary>
    /// Tests the CreateEdge method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCreateEdge()
    {
		Int32 iFirstEdgeID = Int32.MinValue;

		foreach ( Int32 iEdges in new Int32 [] {1, 1000} )

		foreach (Boolean bDirected in GraphUtil.AllBoolean)

		for (Int32 i = 0; i < iEdges; i++)

		{
		IEdge oEdge = m_oEdgeFactory.CreateEdge(
			m_oVertex1, m_oVertex2, bDirected);

		Assert.IsNotNull(oEdge);
		Assert.IsTrue(oEdge is Edge);

		Assert.IsNull(oEdge.Name);

		if (i == 0)
		{
			iFirstEdgeID = oEdge.ID;
		}
		else
		{
			// Make sure the assigned IDs are consecutive.

			Assert.AreEqual(iFirstEdgeID + i, oEdge.ID);
		}

		if (bDirected)
		{
			Assert.IsNotNull(oEdge.BackVertex);
			Assert.AreEqual(m_oVertex1, oEdge.BackVertex);

			Assert.IsNotNull(oEdge.FrontVertex);
			Assert.AreEqual(m_oVertex2, oEdge.FrontVertex);
		}

		Boolean bIsDirected = oEdge.IsDirected;

		Assert.AreEqual(bIsDirected, bDirected);

		Assert.IsFalse(oEdge.IsSelfLoop);

		Assert.AreEqual(m_oVertex1.ParentGraph, oEdge.ParentGraph);
		Assert.AreEqual(m_oVertex2.ParentGraph, oEdge.ParentGraph);

		Assert.IsNull(oEdge.Tag);

		Assert.IsNotNull(oEdge.Vertices);
		Assert.AreEqual(2, oEdge.Vertices.Length);
		Assert.AreEqual( m_oVertex1, oEdge.Vertices[0] );
		Assert.AreEqual( m_oVertex2, oEdge.Vertices[1] );
		}
    }

    //*************************************************************************
    //  Method: TestCreateEdgeBad()
    //
    /// <summary>
    /// Tests the CreateEdge() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestCreateEdgeBad()
    {
		// null vertex1.

		try
		{
			IEdge oEdge = m_oEdgeFactory.CreateEdge(null, m_oVertex2, false);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "EdgeFactory.CreateEdge: vertex1 argument can't be null.\r\n"
				+ "Parameter name: vertex1"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestCreateEdgeBad2()
    //
    /// <summary>
    /// Tests the CreateEdge() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestCreateEdgeBad2()
    {
		// vertex1 not part of a graph.

		try
		{
			IVertex oNonGraphVertex = ( new VertexFactory() ).CreateVertex();

			IEdge oEdge =
				m_oEdgeFactory.CreateEdge(oNonGraphVertex, m_oVertex2, false);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "Edge.Constructor: vertex1 has not been added to a graph.  A"
				+ " vertex must be added to a graph before it can be connected"
				+ " to an edge.\r\n"
				+ "Parameter name: vertex1"
				,
				oArgumentException.Message
				);

			throw oArgumentException;
		}
    }

    //*************************************************************************
    //  Method: TestCreateEdgeBad2()
    //
    /// <summary>
    /// Tests the CreateEdge() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestCreateEdgeBad3()
    {
		// null vertex2.

		try
		{
			IEdge oEdge = m_oEdgeFactory.CreateEdge(m_oVertex1, null, false);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "EdgeFactory.CreateEdge: vertex2 argument can't be null.\r\n"
				+ "Parameter name: vertex2"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestCreateEdgeBad4()
    //
    /// <summary>
    /// Tests the CreateEdge() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestCreateEdgeBad4()
    {
		// vertex2 not part of a graph.

		try
		{
			IVertex oNonGraphVertex = ( new VertexFactory() ).CreateVertex();

			IEdge oEdge =
				m_oEdgeFactory.CreateEdge(m_oVertex1, oNonGraphVertex, false);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "Edge.Constructor: vertex2 has not been added to a graph.  A"
				+ " vertex must be added to a graph before it can be connected"
				+ " to an edge.\r\n"
				+ "Parameter name: vertex2"
				,
				oArgumentException.Message
				);

			throw oArgumentException;
		}
    }

    //*************************************************************************
    //  Method: TestCreateEdgeBad5()
    //
    /// <summary>
    /// Tests the CreateEdge() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestCreateEdgeBad5()
    {
		// vertex1 and vertex2 belong to different graphs.

		try
		{
			IVertex oOtherGraphVertex = ( new VertexFactory() ).CreateVertex();

			( new Graph() ).Vertices.Add(oOtherGraphVertex);

			IEdge oEdge =
				m_oEdgeFactory.CreateEdge(m_oVertex1, oOtherGraphVertex, false);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "Edge.Constructor: vertex1 and vertex2 have been added to"
				+ " different graphs.  An edge can't connect vertices from"
				+ " different graphs.\r\n"
				+ "Parameter name: vertex2"
				,
				oArgumentException.Message
				);

			throw oArgumentException;
		}
    }

    //*************************************************************************
    //  Method: TestCreateUndirectedEdge()
    //
    /// <summary>
    /// Tests the CreateUndirectedEdge method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCreateUndirectedEdge()
    {
		IEdge oEdge =
			m_oEdgeFactory.CreateUndirectedEdge(m_oVertex1, m_oVertex2);

		Assert.IsNotNull(oEdge);
		Assert.IsTrue(oEdge is Edge);

		Assert.IsNull(oEdge.Name);

		// Assert.IsNotNull(oEdge.BackVertex);
		// Assert.AreEqual(m_oVertex1, oEdge.BackVertex);

		// Assert.IsNotNull(oEdge.FrontVertex);
		// Assert.AreEqual(m_oVertex2, oEdge.FrontVertex);

		Assert.IsFalse(oEdge.IsDirected);

		Assert.IsFalse(oEdge.IsSelfLoop);

		Assert.AreEqual(m_oVertex1.ParentGraph, oEdge.ParentGraph);
		Assert.AreEqual(m_oVertex2.ParentGraph, oEdge.ParentGraph);

		Assert.IsNull(oEdge.Tag);

		Assert.IsNotNull(oEdge.Vertices);
		Assert.AreEqual(2, oEdge.Vertices.Length);
		Assert.AreEqual( m_oVertex1, oEdge.Vertices[0] );
		Assert.AreEqual( m_oVertex2, oEdge.Vertices[1] );
    }

    //*************************************************************************
    //  Method: TestCreateUndirectedEdgeBad()
    //
    /// <summary>
    /// Tests the CreateUndirectedEdge() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestCreateUndirectedEdgeBad()
    {
		// null vertex1.

		try
		{
			IEdge oEdge =
				m_oEdgeFactory.CreateUndirectedEdge(null, m_oVertex2);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "EdgeFactory.CreateUndirectedEdge: vertex1 argument can't"
                + " be null.\r\n"
				+ "Parameter name: vertex1"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestCreateUndirectedEdgeBad2()
    //
    /// <summary>
    /// Tests the CreateUndirectedEdge() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestCreateUndirectedEdgeBad2()
    {
		// vertex1 not part of a graph.

		try
		{
			IVertex oNonGraphVertex = ( new VertexFactory() ).CreateVertex();

			IEdge oEdge = m_oEdgeFactory.CreateUndirectedEdge(
				oNonGraphVertex, m_oVertex2);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "Edge.Constructor: vertex1 has not been added to a graph.  A"
				+ " vertex must be added to a graph before it can be connected"
				+ " to an edge.\r\n"
				+ "Parameter name: vertex1"
				,
				oArgumentException.Message
				);

			throw oArgumentException;
		}
    }

    //*************************************************************************
    //  Method: TestCreateUndirectedEdgeBad2()
    //
    /// <summary>
    /// Tests the CreateUndirectedEdge() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestCreateUndirectedEdgeBad3()
    {
		// null vertex2.

		try
		{
			IEdge oEdge =
				m_oEdgeFactory.CreateUndirectedEdge(m_oVertex1, null);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "EdgeFactory.CreateUndirectedEdge: vertex2 argument can't"
                + " be null.\r\n"
				+ "Parameter name: vertex2"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestCreateUndirectedEdgeBad4()
    //
    /// <summary>
    /// Tests the CreateUndirectedEdge() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestCreateUndirectedEdgeBad4()
    {
		// vertex2 not part of a graph.

		try
		{
			IVertex oNonGraphVertex = ( new VertexFactory() ).CreateVertex();

			IEdge oEdge = m_oEdgeFactory.CreateUndirectedEdge(
				m_oVertex1, oNonGraphVertex);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "Edge.Constructor: vertex2 has not been added to a graph.  A"
				+ " vertex must be added to a graph before it can be connected"
				+ " to an edge.\r\n"
				+ "Parameter name: vertex2"
				,
				oArgumentException.Message
				);

			throw oArgumentException;
		}
    }

    //*************************************************************************
    //  Method: TestCreateUndirectedEdgeBad5()
    //
    /// <summary>
    /// Tests the CreateUndirectedEdge() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestCreateUndirectedEdgeBad5()
    {
		// vertex1 and vertex2 belong to different graphs.

		try
		{
			IVertex oOtherGraphVertex = ( new VertexFactory() ).CreateVertex();

			( new Graph() ).Vertices.Add(oOtherGraphVertex);

			IEdge oEdge = m_oEdgeFactory.CreateUndirectedEdge(
				m_oVertex1, oOtherGraphVertex);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "Edge.Constructor: vertex1 and vertex2 have been added to"
				+ " different graphs.  An edge can't connect vertices from"
				+ " different graphs.\r\n"
				+ "Parameter name: vertex2"
				,
				oArgumentException.Message
				);

			throw oArgumentException;
		}
    }

    //*************************************************************************
    //  Method: TestCreateDirectedEdge()
    //
    /// <summary>
    /// Tests the CreateDirectedEdge method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCreateDirectedEdge()
    {
		IEdge oEdge =
			m_oEdgeFactory.CreateDirectedEdge(m_oVertex1, m_oVertex2);

		Assert.IsNotNull(oEdge);
		Assert.IsTrue(oEdge is Edge);

		Assert.IsNull(oEdge.Name);

		Assert.IsNotNull(oEdge.BackVertex);
		Assert.AreEqual(m_oVertex1, oEdge.BackVertex);

		Assert.IsNotNull(oEdge.FrontVertex);
		Assert.AreEqual(m_oVertex2, oEdge.FrontVertex);

		Assert.IsTrue(oEdge.IsDirected);

		Assert.IsFalse(oEdge.IsSelfLoop);

		Assert.AreEqual(m_oVertex1.ParentGraph, oEdge.ParentGraph);
		Assert.AreEqual(m_oVertex2.ParentGraph, oEdge.ParentGraph);

		Assert.IsNull(oEdge.Tag);

		Assert.IsNotNull(oEdge.Vertices);
		Assert.AreEqual(2, oEdge.Vertices.Length);
		Assert.AreEqual( m_oVertex1, oEdge.Vertices[0] );
		Assert.AreEqual( m_oVertex2, oEdge.Vertices[1] );
    }

    //*************************************************************************
    //  Method: TestCreateDirectedEdgeBad()
    //
    /// <summary>
    /// Tests the CreateDirectedEdge() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestCreateDirectedEdgeBad()
    {
		// null vertex1.

		try
		{
			IEdge oEdge =
				m_oEdgeFactory.CreateDirectedEdge(null, m_oVertex2);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "EdgeFactory.CreateDirectedEdge: backVertex argument can't"
                + " be null.\r\n"
				+ "Parameter name: backVertex"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestCreateDirectedEdgeBad2()
    //
    /// <summary>
    /// Tests the CreateDirectedEdge() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestCreateDirectedEdgeBad2()
    {
		// vertex1 not part of a graph.

		try
		{
			IVertex oNonGraphVertex = ( new VertexFactory() ).CreateVertex();

			IEdge oEdge = m_oEdgeFactory.CreateDirectedEdge(
				oNonGraphVertex, m_oVertex2);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "Edge.Constructor: vertex1 has not been added to a graph.  A"
				+ " vertex must be added to a graph before it can be connected"
				+ " to an edge.\r\n"
				+ "Parameter name: vertex1"
				,
				oArgumentException.Message
				);

			throw oArgumentException;
		}
    }

    //*************************************************************************
    //  Method: TestCreateDirectedEdgeBad2()
    //
    /// <summary>
    /// Tests the CreateDirectedEdge() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestCreateDirectedEdgeBad3()
    {
		// null vertex2.

		try
		{
			IEdge oEdge =
				m_oEdgeFactory.CreateDirectedEdge(m_oVertex1, null);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "EdgeFactory.CreateDirectedEdge: frontVertex argument"
                + " can't be null.\r\n"
				+ "Parameter name: frontVertex"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestCreateDirectedEdgeBad4()
    //
    /// <summary>
    /// Tests the CreateDirectedEdge() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestCreateDirectedEdgeBad4()
    {
		// vertex2 not part of a graph.

		try
		{
			IVertex oNonGraphVertex = ( new VertexFactory() ).CreateVertex();

			IEdge oEdge = m_oEdgeFactory.CreateDirectedEdge(
				m_oVertex1, oNonGraphVertex);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "Edge.Constructor: vertex2 has not been added to a graph.  A"
				+ " vertex must be added to a graph before it can be connected"
				+ " to an edge.\r\n"
				+ "Parameter name: vertex2"
				,
				oArgumentException.Message
				);

			throw oArgumentException;
		}
    }

    //*************************************************************************
    //  Method: TestCreateDirectedEdgeBad5()
    //
    /// <summary>
    /// Tests the CreateDirectedEdge() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestCreateDirectedEdgeBad5()
    {
		// vertex1 and vertex2 belong to different graphs.

		try
		{
			IVertex oOtherGraphVertex = ( new VertexFactory() ).CreateVertex();

			( new Graph() ).Vertices.Add(oOtherGraphVertex);

			IEdge oEdge = m_oEdgeFactory.CreateDirectedEdge(
				m_oVertex1, oOtherGraphVertex);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "Edge.Constructor: vertex1 and vertex2 have been added to"
				+ " different graphs.  An edge can't connect vertices from"
				+ " different graphs.\r\n"
				+ "Parameter name: vertex2"
				,
				oArgumentException.Message
				);

			throw oArgumentException;
		}
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected IEdgeFactory m_oEdgeFactory;

	/// First vertex to connect with an edge.

	protected IVertex m_oVertex1;

	/// Second vertex to connect with an edge.

	protected IVertex m_oVertex2;
}

}
