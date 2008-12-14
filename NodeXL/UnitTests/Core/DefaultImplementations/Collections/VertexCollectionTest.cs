
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Common;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: VertexCollectionTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="VertexCollection" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class VertexCollectionTest : Object
{
    //*************************************************************************
    //  Constructor: VertexCollectionTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexCollectionTest" />
	/// class.
    /// </summary>
    //*************************************************************************

    public VertexCollectionTest()
    {
        m_oVertexCollection = null;
		m_oGraph = null;

		m_bVertexAdded = false;
		m_oAddedVertex = null;

		m_bVertexRemoved = false;
		m_oRemovedVertex = null;
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
		m_oGraph = new Graph();

		m_oGraph.PerformExtraValidations = true;

		Debug.Assert(m_oGraph.Vertices is VertexCollection);

        m_oVertexCollection = m_oGraph.Vertices;

		( (VertexCollection)m_oVertexCollection ).VertexAdded +=
			new VertexEventHandler(this.VertexCollection_VertexAdded);

		( (VertexCollection)m_oVertexCollection ).VertexRemoved +=
			new VertexEventHandler(this.VertexCollection_VertexRemoved);

		m_bVertexAdded = false;
		m_oAddedVertex = null;

		m_bVertexRemoved = false;
		m_oRemovedVertex = null;
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
        m_oVertexCollection = null;
		m_oGraph = null;

		m_bVertexAdded = false;
		m_oAddedVertex = null;

		m_bVertexRemoved = false;
		m_oRemovedVertex = null;
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
		Assert.AreEqual(0, m_oVertexCollection.Count);
		Assert.IsFalse(m_oVertexCollection.IsSynchronized);
		Assert.IsNotNull(m_oVertexCollection.SyncRoot);
    }

    //*************************************************************************
    //  Method: TestAdd()
    //
    /// <summary>
    /// Tests the Add(IVertex) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdd()
    {
		// Add 1 vertex.

		AddVertices(1);

		Assert.AreEqual(1, m_oVertexCollection.Count);
    }

    //*************************************************************************
    //  Method: TestAdd2()
    //
    /// <summary>
    /// Tests the Add(IVertex) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdd2()
    {
		// Add N vertices.

		m_oGraph.PerformExtraValidations = false;

		const Int32 Vertices = 1000;

		AddVertices(Vertices);

		Assert.AreEqual(Vertices, m_oVertexCollection.Count);
    }

    //*************************************************************************
    //  Method: TestAddBad()
    //
    /// <summary>
    /// Tests the Add(IVertex) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestAddBad()
    {
		// Null vertex.

		IVertex oVertex = null;

		try
		{
			m_oVertexCollection.Add(oVertex);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "VertexCollection.Add: vertex argument can't be null.\r\n"
				+ "Parameter name: vertex"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestAddBad2()
    //
    /// <summary>
    /// Tests the Add(IVertex) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestAddBad2()
    {
		// Vertex already added to collection.

		VertexFactory oVertexFactory = new VertexFactory();

		IVertex oVertex = oVertexFactory.CreateVertex();

		m_oVertexCollection.Add(oVertex);

		try
		{
			m_oVertexCollection.Add(oVertex);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "VertexCollection.Add: The vertex already belongs to a"
				+ " graph.  A vertex can't be added twice.\r\n"
				+ "Parameter name: vertex"
				,
				oArgumentException.Message
				);

			throw oArgumentException;
		}
    }

    //*************************************************************************
    //  Method: TestAdd2_()
    //
    /// <summary>
    /// Tests the Add(IVertexFactory) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdd2_()
    {
		// Add 1 vertex.

		VertexFactory oVertexFactory = new VertexFactory();

		IVertex oVertex = m_oVertexCollection.Add(oVertexFactory);

		Assert.IsNotNull(oVertex);

		Assert.AreEqual(1, m_oVertexCollection.Count);
    }

    //*************************************************************************
    //  Method: TestAdd2_2()
    //
    /// <summary>
    /// Tests the Add(IVertexFactory) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdd2_2()
    {
		// Add N vertices.

		m_oGraph.PerformExtraValidations = false;

		VertexFactory oVertexFactory = new VertexFactory();

		const Int32 Vertices = 1000;

		for (Int32 i = 0; i < Vertices; i++)
		{
			IVertex oVertex = m_oVertexCollection.Add(oVertexFactory);

			Assert.IsNotNull(oVertex);
		}

		Assert.AreEqual(Vertices, m_oVertexCollection.Count);
    }

    //*************************************************************************
    //  Method: TestAdd2_Bad()
    //
    /// <summary>
    /// Tests the Add(IVertexFactory) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestAdd2_Bad()
    {
		// Null vertex factory.

		IVertexFactory oVertexFactory = null;

		try
		{
			m_oVertexCollection.Add(oVertexFactory);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "VertexCollection.Add: vertexFactory argument can't be"
				+ " null.\r\n"
				+ "Parameter name: vertexFactory"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestAdd3_()
    //
    /// <summary>
    /// Tests the Add() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdd3_()
    {
		// Add 1 vertex.

		IVertex oVertex = m_oVertexCollection.Add();

		Assert.IsNotNull(oVertex);

        Assert.IsInstanceOfType( oVertex, typeof(Vertex));

		Assert.AreEqual(1, m_oVertexCollection.Count);
    }

    //*************************************************************************
    //  Method: TestAdd3_2()
    //
    /// <summary>
    /// Tests the Add() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdd3_2()
    {
		// Add N vertices.

		m_oGraph.PerformExtraValidations = false;

		const Int32 Vertices = 1000;

		for (Int32 i = 0; i < Vertices; i++)
		{
			IVertex oVertex = m_oVertexCollection.Add();

			Assert.IsNotNull(oVertex);

			Assert.IsInstanceOfType( oVertex, typeof(Vertex));
		}

		Assert.AreEqual(Vertices, m_oVertexCollection.Count);
    }

    //*************************************************************************
    //  Method: TestClear()
    //
    /// <summary>
    /// Tests the Clear() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestClear()
    {
		// Add 0 vertices, then call Clear().

		TestClear(0, false);

		TestClear(0, true);
    }

    //*************************************************************************
    //  Method: TestClear2()
    //
    /// <summary>
    /// Tests the Clear() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestClear2()
    {
		// Add 1 vertex, then call Clear().

		TestClear(1, false);

		TestClear(1, true);
    }

    //*************************************************************************
    //  Method: TestClear3()
    //
    /// <summary>
    /// Tests the Clear() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestClear3()
    {
		// Add 2 vertices, then call Clear().

		TestClear(2, false);

		TestClear(2, true);
    }

    //*************************************************************************
    //  Method: TestClear4()
    //
    /// <summary>
    /// Tests the Clear() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestClear4()
    {
		// Add N vertices, then call Clear().

		m_oGraph.PerformExtraValidations = false;

		const Int32 Vertices = 867;

		TestClear(Vertices, false);

		TestClear(Vertices, true);
    }

    //*************************************************************************
    //  Method: TestContainsAndFind()
    //
    /// <summary>
    /// Tests the Contains() and Find() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestContainsAndFind()
    {
		// Add 0 vertices.

		TestContainsAndFind(0);
    }

    //*************************************************************************
    //  Method: TestContainsAndFind2()
    //
    /// <summary>
    /// Tests the Contains() and Find() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestContainsAndFind2()
    {
		// Add 2 vertices.

		TestContainsAndFind(2);
    }

    //*************************************************************************
    //  Method: TestContainsAndFind3()
    //
    /// <summary>
    /// Tests the Contains() and Find() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestContainsAndFind3()
    {
		// Add N vertices.

		const Int32 Vertices = 6512;

        m_oGraph.PerformExtraValidations = false;

		TestContainsAndFind(Vertices);
    }

    //*************************************************************************
    //  Method: TestContainsBad()
    //
    /// <summary>
    /// Tests the Contains(IVertex) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestContainsBad()
    {
		// Null IVertex.

		try
		{
			IVertex oVertex = null;

			Boolean bContains = m_oVertexCollection.Contains(oVertex);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "VertexCollection.Contains: vertex argument can't be"
				+ " null.\r\n"
				+ "Parameter name: vertex"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestContains3_Bad()
    //
    /// <summary>
    /// Tests the Contains(String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestContains3_Bad()
    {
		// Null name.

		try
		{
			String sName = null;

			Boolean bContains = m_oVertexCollection.Contains(sName);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "VertexCollection.Contains: name argument can't be"
				+ " null.\r\n"
				+ "Parameter name: name"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestContains3_Bad2()
    //
    /// <summary>
    /// Tests the Contains(String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestContains3_Bad2()
    {
		// Empty name.

		try
		{
			Boolean bContains = m_oVertexCollection.Contains(String.Empty);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "VertexCollection.Contains: name argument must have a length"
				+ " greater than zero.\r\n"
				+ "Parameter name: name"
				,
				oArgumentException.Message
				);

			throw oArgumentException;
		}
    }

    //*************************************************************************
    //  Method: TestCopyTo()
    //
    /// <summary>
    /// Tests the CopyTo() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCopyTo()
    {
		// Empty collection.

		AddVertices(0);

		IVertex [] aoCopiedVertices = new IVertex[0];

		m_oVertexCollection.CopyTo(aoCopiedVertices, 0);
    }

    //*************************************************************************
    //  Method: TestCopyTo2()
    //
    /// <summary>
    /// Tests the CopyTo() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCopyTo2()
    {
		// Full collection.

		TestCopyTo(2000, 0);
    }

    //*************************************************************************
    //  Method: TestCopyTo3()
    //
    /// <summary>
    /// Tests the CopyTo() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCopyTo3()
    {
		// Full collection, with offset.

		TestCopyTo(1876, 1);
    }

    //*************************************************************************
    //  Method: TestCopyTo4()
    //
    /// <summary>
    /// Tests the CopyTo() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCopyTo4()
    {
		// Full collection, with offset.

		TestCopyTo(3872, 5000);
    }

    //*************************************************************************
    //  Method: TestCopyToBad()
    //
    /// <summary>
    /// Tests the CopyTo() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestCopyToBad()
    {
		// Null array.

		try
		{
			AddVertices(1);

			m_oVertexCollection.CopyTo(null, 0);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "VertexCollection.CopyTo: array argument can't be"
				+ " null.\r\n"
				+ "Parameter name: array"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestCopyToBad2()
    //
    /// <summary>
    /// Tests the CopyTo() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestCopyToBad2()
    {
		// Array of wrong type.

		try
		{
			AddVertices(1);

			Int32 [] aiInt = new Int32[1];

			m_oVertexCollection.CopyTo(aiInt, 0);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "VertexCollection.CopyTo: array is not of type"
				+ " IVertex[].\r\n"
				+ "Parameter name: array"
				,
				oArgumentException.Message
				);

			throw oArgumentException;
		}
    }

    //*************************************************************************
    //  Method: TestCopyToBad3()
    //
    /// <summary>
    /// Tests the CopyTo() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentOutOfRangeException) ) ]

    public void
    TestCopyToBad3()
    {
		// Negative index.

		try
		{
			AddVertices(1);

			IVertex [] aoVertices = new IVertex[1];

			m_oVertexCollection.CopyTo(aoVertices, -1);
		}
		catch (ArgumentOutOfRangeException oArgumentOutOfRangeException)
		{
			// (Don't check the exception message, which originates in the
			// .NET Framework.  Just make sure that an exception of the
			// expected type is thrown.)

			throw oArgumentOutOfRangeException;
		}
    }

    //*************************************************************************
    //  Method: TestCopyToBad4()
    //
    /// <summary>
    /// Tests the CopyTo() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestCopyToBad4()
    {
		// Array too small.

		try
		{
			const Int32 Vertices = 100;

			AddVertices(Vertices);

			IVertex [] aoVertices = new IVertex[Vertices - 1];

			m_oVertexCollection.CopyTo(aoVertices, 0);
		}
		catch (ArgumentException oArgumentException)
		{
			// (Don't check the exception message, which originates in the
			// .NET Framework.  Just make sure that an exception of the
			// expected type is thrown.)

			throw oArgumentException;
		}
    }

    //*************************************************************************
    //  Method: TestCopyToBad5()
    //
    /// <summary>
    /// Tests the CopyTo() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestCopyToBad5()
    {
		// Array too small, with offset.

		try
		{
			const Int32 Vertices = 100;

			AddVertices(Vertices);

			IVertex [] aoVertices = new IVertex[Vertices];

			m_oVertexCollection.CopyTo(aoVertices, 1);
		}
		catch (ArgumentException oArgumentException)
		{
			// (Don't check the exception message, which originates in the
			// .NET Framework.  Just make sure that an exception of the
			// expected type is thrown.)

			throw oArgumentException;
		}
    }

    //*************************************************************************
    //  Method: TestFind3_Bad()
    //
    /// <summary>
    /// Tests the Find(String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestFind3_Bad()
    {
		// Null name.

		try
		{
			IVertex oFoundVertex = null;

			m_oVertexCollection.Find(null, out oFoundVertex);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "VertexCollection.Find: name argument can't be null.\r\n"
				+ "Parameter name: name"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestFind3_Bad2()
    //
    /// <summary>
    /// Tests the Find(String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestFind3_Bad2()
    {
		// Empty name.

		try
		{
			IVertex oFoundVertex = null;

			m_oVertexCollection.Find(String.Empty, out oFoundVertex);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "VertexCollection.Find: name argument must have a length"
				+ " greater than zero.\r\n"
				+ "Parameter name: name"
				,
				oArgumentException.Message
				);

			throw oArgumentException;
		}
    }

    //*************************************************************************
    //  Method: TestGetEnumerator()
    //
    /// <summary>
    /// Tests the GetEnumerator() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetEnumerator()
    {
		TestGetEnumerator(0);
    }

    //*************************************************************************
    //  Method: TestGetEnumerator2()
    //
    /// <summary>
    /// Tests the GetEnumerator() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetEnumerator2()
    {
		TestGetEnumerator(1);
    }

    //*************************************************************************
    //  Method: TestGetEnumerator3()
    //
    /// <summary>
    /// Tests the GetEnumerator() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetEnumerator3()
    {
		TestGetEnumerator(99);
    }

    //*************************************************************************
    //  Method: TestGetReverseEnumerable()
    //
    /// <summary>
    /// Tests the GetReverseEnumerable() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetReverseEnumerable()
    {
		TestGetReverseEnumerable(0);
    }

    //*************************************************************************
    //  Method: TestGetReverseEnumerable2()
    //
    /// <summary>
    /// Tests the GetReverseEnumerable() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetReverseEnumerable2()
    {
		TestGetReverseEnumerable(1);
    }

    //*************************************************************************
    //  Method: TestGetReverseEnumerable3()
    //
    /// <summary>
    /// Tests the GetReverseEnumerable() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetReverseEnumerable3()
    {
		TestGetReverseEnumerable(99);
    }

    //*************************************************************************
    //  Method: TestRemove()
    //
    /// <summary>
    /// Tests the Remove() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRemove()
    {
		// Vertex not in collection.

		const Int32 Vertices = 65;

		// Add the vertices and edges.

		IVertex[] aoVertices = AddVertices(Vertices);

		GraphUtil.MakeGraphComplete(m_oGraph, aoVertices, false);

		// Check the number of vertices and edges.

		Assert.AreEqual(Vertices, m_oVertexCollection.Count);

		Int32 iEdges = m_oGraph.Edges.Count;

		Assert.AreEqual(
			GraphUtil.GetEdgeCountForCompleteGraph(Vertices), iEdges
			);

		// Attempt to remove a vertex not in the collection.

		IVertex oVertex = ( new VertexFactory() ).CreateVertex();

		Boolean bRemoved = m_oVertexCollection.Remove(oVertex);

		Assert.IsFalse(bRemoved);

		bRemoved = m_oVertexCollection.Remove(oVertex.ID);

		Assert.IsFalse(bRemoved);

		bRemoved = m_oVertexCollection.Remove("abc");

		Assert.IsFalse(bRemoved);

		// Check the number of vertices and edges.

		Assert.AreEqual(Vertices, m_oVertexCollection.Count);

		Assert.AreEqual(iEdges, m_oGraph.Edges.Count);
    }

    //*************************************************************************
    //  Method: TestRemove2()
    //
    /// <summary>
    /// Tests the Remove() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRemove2()
    {
		// Remove first vertex.

		TestRemove(10, new Int32 [] {0} );
    }

    //*************************************************************************
    //  Method: TestRemove3()
    //
    /// <summary>
    /// Tests the Remove() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRemove3()
    {
		// Remove last vertex.

		TestRemove(10, new Int32 [] {9} );
    }

    //*************************************************************************
    //  Method: TestRemove4()
    //
    /// <summary>
    /// Tests the Remove() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRemove4()
    {
		// Remove all vertices.

		TestRemove(10, new Int32 [] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9} );
    }

    //*************************************************************************
    //  Method: TestRemove5()
    //
    /// <summary>
    /// Tests the Remove() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRemove5()
    {
		// Remove all vertices, backwards.

		TestRemove(10, new Int32 [] {9, 8, 7, 6, 5, 4, 3, 2, 1, 0} );
    }

    //*************************************************************************
    //  Method: TestRemove6()
    //
    /// <summary>
    /// Tests the Remove() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRemove6()
    {
		// Remove every even vertex.

		TestRemove(10, new Int32 [] {0, 2, 4, 6, 8} );
    }

    //*************************************************************************
    //  Method: TestRemove7()
    //
    /// <summary>
    /// Tests the Remove() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRemove7()
    {
		// Remove every odd vertex.

		TestRemove(10, new Int32 [] {1, 3, 5, 7, 9} );
    }

    //*************************************************************************
    //  Method: TestRemove8()
    //
    /// <summary>
    /// Tests the Remove() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRemove8()
    {
		// Remove all but last vertex.

		TestRemove(10, new Int32 [] {0, 1, 2, 3, 4, 5, 6, 7, 8} );
    }

    //*************************************************************************
    //  Method: TestRemove9()
    //
    /// <summary>
    /// Tests the Remove() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRemove9()
    {
		// Remove all but first vertex.

		TestRemove(10, new Int32 [] {1, 2, 3, 4, 5, 6, 7, 8, 9} );
    }

    //*************************************************************************
    //  Method: TestRemoveBad()
    //
    /// <summary>
    /// Tests the Remove(IVertex) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestRemoveBad()
    {
		// Null vertex.

		IVertex oVertex = null;

		try
		{
			m_oVertexCollection.Remove(oVertex);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "VertexCollection.Remove: vertex argument can't be null.\r\n"
				+ "Parameter name: vertex"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestRemoveBad2()
    //
    /// <summary>
    /// Tests the Remove(String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestRemoveBad2()
    {
		// Null name.

		String sName = null;

		try
		{
			m_oVertexCollection.Remove(sName);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "VertexCollection.Remove: name argument can't be null.\r\n"
				+ "Parameter name: name"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestRemoveBad3()
    //
    /// <summary>
    /// Tests the Remove(String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestRemoveBad3()
    {
		// Empty name.

		try
		{
			m_oVertexCollection.Remove(String.Empty);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "VertexCollection.Remove: name argument must have a length"
				+ " greater than zero.\r\n"
				+ "Parameter name: name"
				,
				oArgumentException.Message
				);

			throw oArgumentException;
		}
    }

    //*************************************************************************
    //  Method: TestToString()
    //
    /// <summary>
    /// Tests the ToString() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestToString()
    {
		// Default format, 0 vertices.

		Assert.AreEqual(
			"0 vertices",
			m_oVertexCollection.ToString()
			);
    }

    //*************************************************************************
    //  Method: TestToString2()
    //
    /// <summary>
    /// Tests the ToString() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestToString2()
    {
		// Default format, 1 vertex.

		Int32 Vertices = 1;

		AddVertices(Vertices);

		Assert.AreEqual(
			"1 vertex",
			m_oVertexCollection.ToString()
			);
    }

    //*************************************************************************
    //  Method: TestToString3()
    //
    /// <summary>
    /// Tests the ToString() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestToString3()
    {
		// Default format, N vertices.

		Int32 Vertices = 99;

		AddVertices(Vertices);

		Assert.AreEqual(
			"99 vertices",
			m_oVertexCollection.ToString()
			);
    }

    //*************************************************************************
    //  Method: TestToString4()
    //
    /// <summary>
    /// Tests the ToString() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestToString4()
    {
		// null format.

		Assert.AreEqual(
			"0 vertices",
			m_oVertexCollection.ToString(null)
			);
    }

    //*************************************************************************
    //  Method: TestToString5()
    //
    /// <summary>
    /// Tests the ToString() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestToString5()
    {
		// Empty string format.

		Assert.AreEqual(
			"0 vertices",
			m_oVertexCollection.ToString(String.Empty)
			);
    }

    //*************************************************************************
    //  Method: TestToString6()
    //
    /// <summary>
    /// Tests the ToString() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestToString6()
    {
		// G format.

		Assert.AreEqual(
			"0 vertices",
			m_oVertexCollection.ToString("G")
			);
    }

    //*************************************************************************
    //  Method: TestToString7()
    //
    /// <summary>
    /// Tests the ToString() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestToString7()
    {
		// P format.

		Assert.AreEqual(
			"0 vertices\r\n",
			m_oVertexCollection.ToString("P")
			);
    }

    //*************************************************************************
    //  Method: TestToString8()
    //
    /// <summary>
    /// Tests the ToString() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestToString8()
    {
		// D format.

		Int32 Vertices = 4;

		IVertex [] aoVertices = AddVertices(Vertices);

		String sExpectedValue =

			"4 vertices\r\n"

			+ "\tID = " + aoVertices[0].ID.ToString(NodeXLBase.Int32Format)
			+ "\r\n"

			+ "\tID = " + aoVertices[1].ID.ToString(NodeXLBase.Int32Format)
			+ "\r\n"

			+ "\tID = " + aoVertices[2].ID.ToString(NodeXLBase.Int32Format)
			+ "\r\n"

			+ "\tID = " + aoVertices[3].ID.ToString(NodeXLBase.Int32Format)
			+ "\r\n"
			;

		Assert.AreEqual(
			sExpectedValue,
			m_oVertexCollection.ToString("D")
			);
    }

    //*************************************************************************
    //  Method: TestToStringBad()
    //
    /// <summary>
    /// Tests the ToString() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(FormatException) ) ]

    public void
    TestToStringBad()
    {
		// Bad format.

		try
		{
			m_oVertexCollection.ToString("Bad");
		}
		catch (FormatException oFormatException)
		{
			Assert.AreEqual(

				"Microsoft.NodeXL.Core."
				+ "VertexCollection.ToString: Invalid format.  Available"
				+ " formats are G, P, and D."
				,
				oFormatException.Message
				);

			throw oFormatException;
		}
    }

    //*************************************************************************
    //  Enum: RemoveOverload
    //
    /// <summary>
	/// Specifies which overload of VertexCollection.Remove() to call.
    /// </summary>
    //*************************************************************************

    protected enum
    RemoveOverload
    {
        /// <summary>
        /// Call Remove(IVertex).
        /// </summary>

        ByReference = 0,

        /// <summary>
        /// Call Remove(Int32).
        /// </summary>

        ByID = 1,

        /// <summary>
        /// Call Remove(String).
        /// </summary>

        ByName = 2,
    }

    //*************************************************************************
    //  Method: TestClear()
    //
    /// <summary>
    /// Tests the Clear() method.
    /// </summary>
	///
	/// <param name="iVerticesToAdd">
	/// Number of vertices to add before calling Clear().
	/// </param>
	///
	/// <param name="bMakeGraphComplete">
	/// If true, an edge is added between each pair of added vertices.
	/// </param>
    //*************************************************************************

    protected void
    TestClear
	(
		Int32 iVerticesToAdd,
		Boolean bMakeGraphComplete
	)
    {
		Debug.Assert(iVerticesToAdd >= 0);

		// Add the vertices.

		IVertex[] aoVertices = AddVertices(iVerticesToAdd);

		if (bMakeGraphComplete)
		{
			GraphUtil.MakeGraphComplete(m_oGraph, aoVertices, false);
		}

		Assert.AreEqual(iVerticesToAdd, m_oVertexCollection.Count);

		if (bMakeGraphComplete && iVerticesToAdd > 1)
		{
			Assert.IsTrue(m_oGraph.Edges.Count > 0);
		}
		else
		{
			Assert.AreEqual(0, m_oGraph.Edges.Count);
		}

		foreach (IVertex oVertex in aoVertices)
		{
			Assert.AreEqual(m_oGraph, oVertex.ParentGraph);
		}

		m_bVertexRemoved = false;
		m_oRemovedVertex = null;

		m_oVertexCollection.Clear();

		Assert.IsFalse(m_bVertexRemoved);
		Assert.IsNull(m_oRemovedVertex);

		Assert.AreEqual(0, m_oVertexCollection.Count);

		Assert.AreEqual(0, m_oGraph.Edges.Count);

		foreach (IVertex oVertex in aoVertices)
		{
			Assert.IsNull(oVertex.ParentGraph);
		}
    }

    //*************************************************************************
    //  Method: TestContainsAndFind()
    //
    /// <summary>
    /// Tests the Contains() and Find() methods.
    /// </summary>
	///
	/// <param name="iVerticesToAdd">
	/// Number of vertices to add.
	/// </param>
    //*************************************************************************

    protected void
    TestContainsAndFind
	(
		Int32 iVerticesToAdd
	)
    {
		// Add the vertices.

		IVertex[] aoContainedVertices = AddVertices(iVerticesToAdd);

		// Create a vertex but don't add it.

		IVertex oNonContainedVertex = ( new VertexFactory() ).CreateVertex();
        oNonContainedVertex.Name = "ekdjrmnek";

		IVertex oFoundVertex;

		// The collection should not contain oNonContainedVertex.

		Assert.IsFalse( m_oVertexCollection.Contains(oNonContainedVertex) );

		Assert.IsFalse( m_oVertexCollection.Contains(oNonContainedVertex.ID) );

		Assert.IsFalse( m_oVertexCollection.Find(
			oNonContainedVertex.ID, out oFoundVertex) );

		Assert.IsNull(oFoundVertex);

		Assert.IsFalse(
			m_oVertexCollection.Contains(oNonContainedVertex.Name) );

		Assert.IsFalse( m_oVertexCollection.Find(
			oNonContainedVertex.Name, out oFoundVertex) );

		Assert.IsNull(oFoundVertex);

		// The collection should contain the vertices in aoContainedVertices.

		foreach (IVertex oContainedVertex in aoContainedVertices)
		{
			Assert.IsTrue( m_oVertexCollection.Contains(oContainedVertex) );

			Assert.IsTrue( m_oVertexCollection.Contains(oContainedVertex.ID) );

			Assert.IsTrue( m_oVertexCollection.Find(
				oContainedVertex.ID, out oFoundVertex) );

			Assert.AreEqual(oFoundVertex, oContainedVertex);

			Assert.IsTrue(
				m_oVertexCollection.Contains(oContainedVertex.Name) );

			Assert.IsTrue( m_oVertexCollection.Find(
				oContainedVertex.Name, out oFoundVertex) );

			Assert.AreEqual(oFoundVertex, oContainedVertex);
		}
    }

    //*************************************************************************
    //  Method: TestCopyTo()
    //
    /// <summary>
    /// Tests the CopyTo() method.
    /// </summary>
	///
	/// <param name="iVerticesToAdd">
	/// Number of vertices to add.
	/// </param>
	///
	/// <param name="iOffset">
	/// Offset to copy to.
	/// </param>
    //*************************************************************************

    protected void
    TestCopyTo
	(
		Int32 iVerticesToAdd,
		Int32 iOffset
	)
    {
		Debug.Assert(iVerticesToAdd >= 0);
		Debug.Assert(iOffset >= 0);

		IVertex [] aoAddedVertices = AddVertices(iVerticesToAdd);

		IVertex [] aoCopiedVertices = new IVertex[iVerticesToAdd + iOffset];

		m_oVertexCollection.CopyTo(aoCopiedVertices, iOffset);

		for (Int32 i = 0; i < iOffset; i++)
		{
			Assert.IsNull( aoCopiedVertices[i] );
		}

		for (Int32 i = iOffset; i < iVerticesToAdd + iOffset; i++)
		{
			Assert.AreEqual(
				aoCopiedVertices[i], aoAddedVertices[i - iOffset]
				);
		}
    }

    //*************************************************************************
    //  Method: TestGetEnumerator()
    //
    /// <summary>
    /// Tests the GetEnumerator() method.
    /// </summary>
	///
	/// <param name="iVerticesToAdd">
	/// Number of vertices to add.
	/// </param>
    //*************************************************************************

    protected void
    TestGetEnumerator
	(
		Int32 iVerticesToAdd
	)
    {
		IVertex [] aoAddedVertices = AddVertices(iVerticesToAdd);

		Int32 i = 0;

		foreach (IVertex oVertex in m_oVertexCollection)
		{
			Assert.AreEqual( oVertex, aoAddedVertices[i] );

			Assert.AreEqual(oVertex.ID, aoAddedVertices[i].ID);

			i++;
		}

		Assert.AreEqual(iVerticesToAdd, i);
    }

    //*************************************************************************
    //  Method: TestGetReverseEnumerable()
    //
    /// <summary>
    /// Tests the GetReverseEnumerable() method.
    /// </summary>
	///
	/// <param name="iVerticesToAdd">
	/// Number of vertices to add.
	/// </param>
    //*************************************************************************

    protected void
    TestGetReverseEnumerable
	(
		Int32 iVerticesToAdd
	)
    {
		IVertex [] aoAddedVertices = AddVertices(iVerticesToAdd);

		Int32 i = iVerticesToAdd - 1;

		foreach ( IVertex oVertex in
            m_oVertexCollection.GetReverseEnumerable() )
		{
			Assert.AreEqual( oVertex, aoAddedVertices[i] );

			Assert.AreEqual(oVertex.ID, aoAddedVertices[i].ID);

			i--;
		}

		Assert.AreEqual(-1, i);
    }

    //*************************************************************************
    //  Method: TestRemove()
    //
    /// <summary>
    /// Tests the Remove() methods.
    /// </summary>
	///
	/// <param name="iVerticesToAdd">
	/// Number of vertices to add.
	/// </param>
	///
	/// <param name="aiIndexesOfVerticesToRemove">
	/// Array of indexes of the vertices to remove.
	/// </param>
    //*************************************************************************

    protected void
    TestRemove
	(
		Int32 iVerticesToAdd,
		Int32 [] aiIndexesOfVerticesToRemove
	)
    {
		Debug.Assert(iVerticesToAdd >= 0);
		Debug.Assert(aiIndexesOfVerticesToRemove != null);

		TestRemove(iVerticesToAdd, aiIndexesOfVerticesToRemove,
			RemoveOverload.ByReference);

		TestRemove(iVerticesToAdd, aiIndexesOfVerticesToRemove,
			RemoveOverload.ByID);

		TestRemove(iVerticesToAdd, aiIndexesOfVerticesToRemove,
			RemoveOverload.ByName);
    }

    //*************************************************************************
    //  Method: TestRemove()
    //
    /// <summary>
    /// Tests a Remove() method.
    /// </summary>
	///
	/// <param name="iVerticesToAdd">
	/// Number of vertices to add.
	/// </param>
	///
	/// <param name="aiIndexesOfVerticesToRemove">
	/// Array of indexes of the vertices to remove.
	/// </param>
	///
	/// <param name="eRemoveOverload">
	/// Specifies which overload of Remove() to call.
	/// </param>
    //*************************************************************************

    protected void
    TestRemove
	(
		Int32 iVerticesToAdd,
		Int32 [] aiIndexesOfVerticesToRemove,
		RemoveOverload eRemoveOverload
	)
    {
		Debug.Assert(iVerticesToAdd >= 0);
		Debug.Assert(aiIndexesOfVerticesToRemove != null);

		Debug.Assert(
			Enum.IsDefined(typeof(RemoveOverload), eRemoveOverload) );

		// Remove any existing vertices and edges.

		m_oVertexCollection.Clear();

		m_oGraph.Edges.Clear();

		// Add the vertices and edges.

		IVertex[] aoVertices = AddVertices(iVerticesToAdd);

		GraphUtil.MakeGraphComplete(m_oGraph, aoVertices, false);

		// Check the number of vertices and edges.

		Assert.AreEqual(iVerticesToAdd, m_oVertexCollection.Count);

		Assert.AreEqual(
			GraphUtil.GetEdgeCountForCompleteGraph(iVerticesToAdd),
			m_oGraph.Edges.Count
			);

		// Remove the specified vertices.

		Int32 iVerticesToRemove = aiIndexesOfVerticesToRemove.Length;

		for (Int32 i = 0; i < iVerticesToRemove; i++)
		{
			IVertex oVertexToRemove =
				aoVertices[ aiIndexesOfVerticesToRemove[i] ];

			Boolean bRemoved = false;

			m_bVertexRemoved = false;
			m_oRemovedVertex = null;

			switch (eRemoveOverload)
			{
				case RemoveOverload.ByReference:

					bRemoved = m_oVertexCollection.Remove(oVertexToRemove);

					break;

				case RemoveOverload.ByID:

					bRemoved = m_oVertexCollection.Remove(oVertexToRemove.ID);

					break;

				case RemoveOverload.ByName:

					bRemoved = m_oVertexCollection.Remove(oVertexToRemove.Name);

					break;

				default:

					Debug.Assert(false);
					break;
			}

			Assert.IsTrue(bRemoved);

			Assert.IsTrue(m_bVertexRemoved);
			Assert.AreEqual(oVertexToRemove, m_oRemovedVertex);

			Assert.IsNull(oVertexToRemove.ParentGraph);
		}

		// Check the number of vertices and edges.

		Int32 iRemainingVertices = iVerticesToAdd - iVerticesToRemove;

		Assert.AreEqual(iRemainingVertices, m_oVertexCollection.Count);

		Assert.AreEqual(
			GraphUtil.GetEdgeCountForCompleteGraph(iRemainingVertices),
			m_oGraph.Edges.Count
			);

		// Verify that the correct vertices are still in the collection.

		for (Int32 i = 0; i < iVerticesToAdd; i++)
		{
			Boolean bContains = m_oVertexCollection.Contains( aoVertices[i] );

			if (Array.IndexOf(aiIndexesOfVerticesToRemove, i) >= 0)
			{
				// i is in aiIndexesOfVerticesToRemove, so aoVertices[i] should
				// not be in the collection.

				Assert.IsFalse(bContains);
			}
			else
			{
				Assert.IsTrue(bContains);

				Assert.IsNotNull(aoVertices[i].ParentGraph);
			}
		}
    }

    //*************************************************************************
    //  Method: AddVertices()
    //
    /// <summary>
	/// Adds a specified number of vertices to m_oGraph using the Add(IVertex)
	/// method.
    /// </summary>
	///
	/// <param name="iVerticesToAdd">
	/// Number of vertices to add.
	/// </param>
	///
	/// <returns>
	/// An array of the added vertices.
	/// </returns>
    //*************************************************************************

    protected IVertex[]
    AddVertices
	(
		Int32 iVerticesToAdd
	)
    {
		Debug.Assert(iVerticesToAdd >= 0);

		IVertex[] aoVertices = new IVertex[iVerticesToAdd];

		// Add the vertices.

		VertexFactory oVertexFactory = new VertexFactory();

		for (Int32 i = 0; i < iVerticesToAdd; i++)
		{
			IVertex oVertex = aoVertices[i] = oVertexFactory.CreateVertex();

			oVertex.Name = i.ToString();

			m_bVertexAdded = false;

			m_oAddedVertex = null;

			IVertex oAddedVertex = m_oVertexCollection.Add(oVertex);

			Assert.IsNotNull(oAddedVertex);

			Assert.AreEqual(oAddedVertex, oVertex);

			Assert.IsTrue(m_bVertexAdded);

			Assert.AreEqual(oVertex, m_oAddedVertex);
		}

		return (aoVertices);
    }

    //*************************************************************************
    //  Method: VertexCollection_VertexAdded()
    //
    /// <summary>
	/// Handles the VertexAdded event on the m_oVertexCollection object.
    /// </summary>
	///
	/// <param name="oSender">
	/// Standard event argument.
	/// </param>
	///
	/// <param name="oVertexEventArgs">
	/// Standard event argument.
	/// </param>
    //*************************************************************************

	protected void
	VertexCollection_VertexAdded
	(
		Object oSender,
		VertexEventArgs oVertexEventArgs
	)
	{
		if ( oSender == null || !(oSender is VertexCollection) )
		{
			throw new ApplicationException(
				"VertexAdded event provided incorrect oSender argument."
				);
		}

		m_bVertexAdded = true;

		m_oAddedVertex = oVertexEventArgs.Vertex;
	}

    //*************************************************************************
    //  Method: VertexCollection_VertexRemoved()
    //
    /// <summary>
	/// Handles the VertexRemoved event on the m_oVertexCollection object.
    /// </summary>
	///
	/// <param name="oSender">
	/// Standard event argument.
	/// </param>
	///
	/// <param name="oVertexEventArgs">
	/// Standard event argument.
	/// </param>
    //*************************************************************************

	protected void
	VertexCollection_VertexRemoved
	(
		Object oSender,
		VertexEventArgs oVertexEventArgs
	)
	{
		if ( oSender == null || !(oSender is VertexCollection) )
		{
			throw new ApplicationException(
				"VertexRemoved event provided incorrect oSender argument."
				);
		}

		m_bVertexRemoved = true;

		m_oRemovedVertex = oVertexEventArgs.Vertex;
	}


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected IVertexCollection m_oVertexCollection;

	/// Graph that owns m_oVertexCollection.

	protected IGraph m_oGraph;

	/// Gets set by VertexCollection_VertexAdded().

	protected Boolean m_bVertexAdded;

	/// Gets set by VertexCollection_VertexAdded().

	protected IVertex m_oAddedVertex;

	/// Gets set by VertexCollection_VertexRemoved().

	protected Boolean m_bVertexRemoved;

	/// Gets set by VertexCollection_VertexRemoved().

	protected IVertex m_oRemovedVertex;
}

}
