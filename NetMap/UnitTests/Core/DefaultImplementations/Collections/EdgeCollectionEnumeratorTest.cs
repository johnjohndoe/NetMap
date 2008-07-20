
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NetMap.Core;
using Microsoft.NetMap.Tests;

namespace Microsoft.NetMap.UnitTests
{
//*****************************************************************************
//  Class: EdgeCollectionEnumeratorTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="EdgeCollection.Enumerator" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class EdgeCollectionEnumeratorTest : Object
{
    //*************************************************************************
    //  Constructor: EdgeCollectionEnumeratorTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="EdgeCollectionEnumeratorTest" /> class.
    /// </summary>
    //*************************************************************************

    public EdgeCollectionEnumeratorTest()
    {
		m_oEnumerator = null;
		m_oGraph = null;
		m_oVertexCollection = null;
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

		Debug.Assert(m_oGraph.Edges is EdgeCollection);

        IEdgeCollection oEdgeCollection = m_oGraph.Edges;

		Debug.Assert(oEdgeCollection.GetEnumerator() is
			EdgeCollection.Enumerator);

        m_oEnumerator =
			(EdgeCollection.Enumerator)oEdgeCollection.GetEnumerator();

		Debug.Assert(m_oGraph.Vertices is VertexCollection);

        m_oVertexCollection = m_oGraph.Vertices;
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
		m_oEnumerator.Reset();

		m_oEnumerator = null;
		m_oGraph = null;
		m_oVertexCollection = null;
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
        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: TestCurrentBad()
    //
    /// <summary>
    /// Tests the Current property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestCurrentBad()
    {
		// MoveNext() not called.

		try
		{
			IEdge oEdge = m_oEnumerator.Current;
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Core."
				+ "EdgeCollection+Enumerator.Current: MoveNext() hasn't been"
				+ " called."
				,
				oApplicationException.Message
				);

			throw oApplicationException;
		}
    }

    //*************************************************************************
    //  Method: TestMoveNext()
    //
    /// <summary>
    /// Tests the MoveNext() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestMoveNext()
    {
		// 0 vertices.

		Assert.IsFalse( m_oEnumerator.MoveNext() );
    }

    //*************************************************************************
    //  Method: TestMoveNext2()
    //
    /// <summary>
    /// Tests the MoveNext() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestMoveNext2()
    {
		// N vertices, 0 edges.

		const Int32 Vertices = 100;

		IVertex[] aoVertices = AddVertices(Vertices);

		Assert.IsFalse( m_oEnumerator.MoveNext() );
    }

    //*************************************************************************
    //  Method: TestMoveNext3()
    //
    /// <summary>
    /// Tests the MoveNext() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestMoveNext3()
    {
		// N vertices, connect first vertex to all other vertices.

		const Int32 Vertices = 300;

		// Add the vertices.

		IVertex[] aoVertices = AddVertices(Vertices);

		// Connect the first vertex to all other vertices.

		Int32 iEdges = Vertices - 1;

		IEdge[] aoAddedEdges = new IEdge[iEdges];

		EdgeFactory oEdgeFactory = new EdgeFactory();

		IVertex oVertex1 = aoVertices[0];

		IEdgeCollection oEdgeCollection = m_oGraph.Edges;

		Int32 i;

		for (i = 1; i < Vertices; i++)
		{
			IVertex oVertex2 = aoVertices[i];

			IEdge oEdge = oEdgeFactory.CreateEdge(
				oVertex1, oVertex2, false);

			oEdge.Name = oEdge.ID.ToString();

			aoAddedEdges[i - 1] = oEdge;

			oEdgeCollection.Add(oEdge);
		}

		// Enumerate the edges using m_oEnumerator and compare the enumerated
		// edges with the edges that were added to the edge collection.

		EnumerateAndCompare(aoAddedEdges);

		// Reset the enumerator and repeat.

		m_oEnumerator.Reset();

		EnumerateAndCompare(aoAddedEdges);
    }

    //*************************************************************************
    //  Method: TestMoveNext4()
    //
    /// <summary>
    /// Tests the MoveNext() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestMoveNext4()
    {
		// N vertices, connect last vertex to all other vertices.

		const Int32 Vertices = 2000;

		// Add the vertices.

		IVertex[] aoVertices = AddVertices(Vertices);

		// Connect the first vertex to all other vertices.

		Int32 iEdges = Vertices - 1;

		IEdge[] aoAddedEdges = new IEdge[iEdges];

		EdgeFactory oEdgeFactory = new EdgeFactory();

		IVertex oVertex1 = aoVertices[Vertices - 1];

		IEdgeCollection oEdgeCollection = m_oGraph.Edges;

		Int32 i;

		for (i = 0; i < Vertices - 1; i++)
		{
			IVertex oVertex2 = aoVertices[i];

			IEdge oEdge = oEdgeFactory.CreateEdge(
				oVertex1, oVertex2, false);

			oEdge.Name = oEdge.ID.ToString();

			aoAddedEdges[i] = oEdge;

			oEdgeCollection.Add(oEdge);
		}

		// Enumerate the edges using m_oEnumerator and compare the enumerated
		// edges with the edges that were added to the edge collection.

		EnumerateAndCompare(aoAddedEdges);

		// Reset the enumerator and repeat.

		m_oEnumerator.Reset();

		EnumerateAndCompare(aoAddedEdges);
    }

    //*************************************************************************
    //  Method: TestMoveNext5()
    //
    /// <summary>
    /// Tests the MoveNext() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestMoveNext5()
    {
		// N vertices, connect each vertex to all other vertices.

		const Int32 Vertices = 100;

		// Add the vertices.

		IVertex[] aoVertices = AddVertices(Vertices);

		// Connect each vertex to all other vertices.

		IEdge[] aoAddedEdges =
            GraphUtil.MakeGraphComplete(m_oGraph, aoVertices, true);

		// Enumerate the edges using m_oEnumerator and compare the enumerated
		// edges with the edges that were added to the edge collection.

		EnumerateAndCompare(aoAddedEdges);

		// Reset the enumerator and repeat.

		m_oEnumerator.Reset();

		EnumerateAndCompare(aoAddedEdges);
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

		return ( GraphUtil.AddVertices(m_oGraph, iVerticesToAdd) );
    }

    //*************************************************************************
    //  Method: EnumerateAndCompare()
    //
    /// <summary>
    /// Enumerates the edges using m_oEnumerator and compares the enumerated
	/// edges with the edges that were added to the edge collection.
    /// </summary>
	///
	/// <param name="aoAddedEdges">
	/// Array of edges that were added to the edge collection.
	/// </param>
    //*************************************************************************

    protected void
    EnumerateAndCompare
	(
		IEdge[] aoAddedEdges
	)
    {
		Debug.Assert(aoAddedEdges != null);

		// Enumerate the edges into a second array.

		Int32 iEdges = aoAddedEdges.Length;

		IEdge[] aoEnumeratedEdges = new IEdge[iEdges];

		Int32 i = 0;

		while ( m_oEnumerator.MoveNext() )
		{
			Assert.IsTrue(i < iEdges);

			aoEnumeratedEdges[i] = m_oEnumerator.Current;

			i++;
		}

		Assert.AreEqual(i, iEdges);

		// Verify that all the added edges are in the array of enumerated
		// edges.

		for (i = 0; i < iEdges; i++)
		{
			IEdge oAddedEdge = aoAddedEdges[i];

			Assert.IsTrue(Array.IndexOf(aoEnumeratedEdges, oAddedEdge) >= 0);
		}

		// Verify that all the enumerated edges are in the array of added
		// edges.

		for (i = 0; i < iEdges; i++)
		{
			IEdge oEnumeratedEdge = aoEnumeratedEdges[i];

			Assert.IsTrue(Array.IndexOf(aoAddedEdges, oEnumeratedEdge) >= 0);
		}
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected EdgeCollection.Enumerator m_oEnumerator;

	/// Graph that owns m_oEnumerator.

	protected IGraph m_oGraph;

	/// Vertices owned by m_oGraph.

    protected IVertexCollection m_oVertexCollection;
}

}
