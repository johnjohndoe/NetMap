
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: EdgeUtilTest
//
/// <summary>
/// This is Visual Studio test fixture for the <see cref="EdgeUtil" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class EdgeUtilTest : Object
{
    //*************************************************************************
    //  Constructor: EdgeUtilTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EdgeUtilTest" /> class.
    /// </summary>
    //*************************************************************************

    public EdgeUtilTest()
    {
		// (Do nothing.)
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
		// (Do nothing.)
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
		// (Do nothing.)
    }

    //*************************************************************************
    //  Method: TestEdgeToVertices()
    //
    /// <summary>
    /// Tests the EdgeToVertices() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestEdgeToVertices()
    {
		// Valid vertices.

		MockVertex oVertex1 = new MockVertex();
		MockVertex oVertex2 = new MockVertex();

		IGraph oGraph = new Graph();

		oVertex1.ParentGraph = oGraph;
		oVertex2.ParentGraph = oGraph;

		IEdge oEdge = new MockEdge(oVertex1, oVertex2, false, false, 2);

		IVertex oVertexA, oVertexB;

		EdgeUtil.EdgeToVertices(oEdge, ClassName, MethodOrPropertyName,
			out oVertexA, out oVertexB);

		Assert.AreEqual(oVertex1, oVertexA);
		Assert.AreEqual(oVertex2, oVertexB);
    }

    //*************************************************************************
    //  Method: TestEdgeToVerticesBad()
    //
    /// <summary>
    /// Tests the EdgeToVertices() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestEdgeToVerticesBad()
    {
		// Null Edge.Vertices.

		try
		{
			IEdge oEdge = new MockEdge(null, null, false, true, 0);

			IVertex oVertexA, oVertexB;

			EdgeUtil.EdgeToVertices(oEdge, ClassName, MethodOrPropertyName,
				out oVertexA, out oVertexB);
		}
		catch (ApplicationException ApplicationException)
		{
			Assert.AreEqual(

				"TheClass.TheMethodOrProperty: The edge's Vertices property is"
				+ " null."
				,
				ApplicationException.Message
				);

			throw ApplicationException;
		}
    }

    //*************************************************************************
    //  Method: TestEdgeToVerticesBad2()
    //
    /// <summary>
    /// Tests the EdgeToVertices() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestEdgeToVerticesBad2()
    {
		// Edge.Vertices contains 0 vertices.

		try
		{
			IEdge oEdge = new MockEdge(null, null, false, false, 0);

			IVertex oVertexA, oVertexB;

			EdgeUtil.EdgeToVertices(oEdge, ClassName, MethodOrPropertyName,
				out oVertexA, out oVertexB);
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual(

				"TheClass.TheMethodOrProperty: The edge does not connect two"
				+ " vertices."
				,
				oApplicationException.Message
				);

			throw oApplicationException;
		}
    }

    //*************************************************************************
    //  Method: TestEdgeToVerticesBad3()
    //
    /// <summary>
    /// Tests the EdgeToVertices() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestEdgeToVerticesBad3()
    {
		// Edge.Vertices contains 1 vertex only.

		try
		{
			IEdge oEdge = new MockEdge(null, null, false, false, 1);

			IVertex oVertexA, oVertexB;

			EdgeUtil.EdgeToVertices(oEdge, ClassName, MethodOrPropertyName,
				out oVertexA, out oVertexB);
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual(

				"TheClass.TheMethodOrProperty: The edge does not connect two"
				+ " vertices."
				,
				oApplicationException.Message
				);

			throw oApplicationException;
		}
    }

    //*************************************************************************
    //  Method: TestEdgeToVerticesBad4()
    //
    /// <summary>
    /// Tests the EdgeToVertices() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestEdgeToVerticesBad4()
    {
		// First vertex is null.

		try
		{
			IEdge oEdge = new MockEdge(
				null, new MockVertex(), false, false, 2);

			IVertex oVertexA, oVertexB;

			EdgeUtil.EdgeToVertices(oEdge, ClassName, MethodOrPropertyName,
				out oVertexA, out oVertexB);
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual(

				"TheClass.TheMethodOrProperty: The edge's first vertex is"
				+ " null."
				,
				oApplicationException.Message
				);

			throw oApplicationException;
		}
    }

    //*************************************************************************
    //  Method: TestEdgeToVerticesBad5()
    //
    /// <summary>
    /// Tests the EdgeToVertices() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestEdgeToVerticesBad5()
    {
		// Second vertex is null.

		try
		{
			IEdge oEdge = new MockEdge(
				new MockVertex(), null, false, false, 2);

			IVertex oVertexA, oVertexB;

			EdgeUtil.EdgeToVertices(oEdge, ClassName, MethodOrPropertyName,
				out oVertexA, out oVertexB);
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual(

				"TheClass.TheMethodOrProperty: The edge's second vertex is"
				+ " null."
				,
				oApplicationException.Message
				);

			throw oApplicationException;
		}
    }

    //*************************************************************************
    //  Method: TestEdgeToVerticesBad6()
    //
    /// <summary>
    /// Tests the EdgeToVertices() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestEdgeToVerticesBad6()
    {
		// First vertex has no parent.

		try
		{
			MockVertex oVertex1 = new MockVertex();
			MockVertex oVertex2 = new MockVertex();

			IGraph oGraph = new Graph();

			// oVertex1.ParentGraph = oGraph;
			oVertex2.ParentGraph = oGraph;

			IEdge oEdge = new MockEdge(oVertex1, oVertex2, false, false, 2);

			IVertex oVertexA, oVertexB;

			EdgeUtil.EdgeToVertices(oEdge, ClassName, MethodOrPropertyName,
				out oVertexA, out oVertexB);
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual(

				"TheClass.TheMethodOrProperty: The edge's first vertex does"
				+ " not belong to a graph."
				,
				oApplicationException.Message
				);

			throw oApplicationException;
		}
    }

    //*************************************************************************
    //  Method: TestEdgeToVerticesBad7()
    //
    /// <summary>
    /// Tests the EdgeToVertices() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestEdgeToVerticesBad7()
    {
		// Second vertex has no parent.

		try
		{
			MockVertex oVertex1 = new MockVertex();
			MockVertex oVertex2 = new MockVertex();

			IGraph oGraph = new Graph();

			oVertex1.ParentGraph = oGraph;
			// oVertex2.ParentGraph = oGraph;

			IEdge oEdge = new MockEdge(oVertex1, oVertex2, false, false, 2);

			IVertex oVertexA, oVertexB;

			EdgeUtil.EdgeToVertices(oEdge, ClassName, MethodOrPropertyName,
				out oVertexA, out oVertexB);
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual(

				"TheClass.TheMethodOrProperty: The edge's second vertex does"
				+ " not belong to a graph."
				,
				oApplicationException.Message
				);

			throw oApplicationException;
		}
    }

    //*************************************************************************
    //  Method: TestEdgeToVerticesBad8()
    //
    /// <summary>
    /// Tests the EdgeToVertices() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestEdgeToVerticesBad8()
    {
		// Vertices not part of the same graph.

		try
		{
			MockVertex oVertex1 = new MockVertex();
			MockVertex oVertex2 = new MockVertex();

			IGraph oGraph1 = new Graph(GraphDirectedness.Mixed);
			IGraph oGraph2 = new Graph(GraphDirectedness.Mixed);

			oVertex1.ParentGraph = oGraph1;
			oVertex2.ParentGraph = oGraph2;

			IEdge oEdge = new MockEdge(oVertex1, oVertex2, false, false, 2);

			IVertex oVertexA, oVertexB;

			EdgeUtil.EdgeToVertices(oEdge, ClassName, MethodOrPropertyName,
				out oVertexA, out oVertexB);
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual(

				"TheClass.TheMethodOrProperty: The edge connects vertices not"
				+ " in the same graph."
				,
				oApplicationException.Message
				);

			throw oApplicationException;
		}
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Arguments to pass to EdgeUtil methods.

	protected const String ClassName = "TheClass";
	///
	protected const String MethodOrPropertyName = "TheMethodOrProperty";
}

}
