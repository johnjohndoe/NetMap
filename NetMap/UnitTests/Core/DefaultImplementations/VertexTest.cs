
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NetMap.Core;
using Microsoft.NetMap.Tests;

namespace Microsoft.NetMap.UnitTests
{
//*****************************************************************************
//  Class: VertexTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="Vertex" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class VertexTest : Object
{
    //*************************************************************************
    //  Constructor: VertexTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexTest" /> class.
    /// </summary>
    //*************************************************************************

    public VertexTest()
    {
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
        m_oVertex = (Vertex)( new VertexFactory() ).CreateVertex();
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
		Assert.IsNotNull(m_oVertex.AdjacentVertices);
		Assert.AreEqual(0, m_oVertex.AdjacentVertices.Length);

		Assert.AreEqual(0, m_oVertex.Degree);

		Assert.IsNotNull(m_oVertex.IncidentEdges);
		Assert.AreEqual(0, m_oVertex.IncidentEdges.Length);

		Assert.IsNotNull(m_oVertex.IncomingEdges);
		Assert.AreEqual(0, m_oVertex.IncomingEdges.Length);

		Assert.AreEqual(PointF.Empty, m_oVertex.Location);

		Assert.IsNull(m_oVertex.Name);

		Assert.IsNotNull(m_oVertex.OutgoingEdges);
		Assert.AreEqual(0, m_oVertex.OutgoingEdges.Length);

		Assert.IsNull(m_oVertex.ParentGraph);

		Assert.IsNotNull(m_oVertex.PredecessorVertices);
		Assert.AreEqual(0, m_oVertex.PredecessorVertices.Length);

		Assert.IsNotNull(m_oVertex.SuccessorVertices);
		Assert.AreEqual(0, m_oVertex.SuccessorVertices.Length);

		Assert.IsNull(m_oVertex.Tag);
    }

    //*************************************************************************
    //  Method: TestName()
    //
    /// <summary>
    /// Tests the Name property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestName()
    {
		const String Name = null;

		m_oVertex.Name = Name;

		Assert.AreEqual(Name, m_oVertex.Name);
    }

    //*************************************************************************
    //  Method: TestName2()
    //
    /// <summary>
    /// Tests the Name property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestName2()
    {
		String Name = String.Empty;

		m_oVertex.Name = Name;

		Assert.AreEqual(Name, m_oVertex.Name);
    }

    //*************************************************************************
    //  Method: TestName3()
    //
    /// <summary>
    /// Tests the Name property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestName3()
    {
		const String Name = " jfkd jkreui2 rfdjk*";

		m_oVertex.Name = Name;

		Assert.AreEqual(Name, m_oVertex.Name);
    }

    //*************************************************************************
    //  Method: TestLocation()
    //
    /// <summary>
    /// Tests the Location property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLocation()
    {
		Single fX = 1234.23F;
		Single fY = 9876.77F;

		m_oVertex.Location = new PointF(fX, fY);

		Assert.AreEqual(fX, m_oVertex.Location.X);
		Assert.AreEqual(fY, m_oVertex.Location.Y);
    }

    //*************************************************************************
    //  Method: TestLocation2()
    //
    /// <summary>
    /// Tests the Location property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLocation2()
    {
		Single fX = -1234.123F;
		Single fY = -9876.009F;

		m_oVertex.Location = new PointF(fX, fY);

		Assert.AreEqual(fX, m_oVertex.Location.X);
		Assert.AreEqual(fY, m_oVertex.Location.Y);
    }

    //*************************************************************************
    //  Method: TestParentGraph()
    //
    /// <summary>
    /// Tests the ParentGraph property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestParentGraph()
    {
		// Add the vertex to a graph.

		IGraph oGraph = new Graph();

		oGraph.Vertices.Add(m_oVertex);

		Assert.AreEqual(oGraph, m_oVertex.ParentGraph);
    }

    //*************************************************************************
    //  Method: TestParentGraph2()
    //
    /// <summary>
    /// Tests the ParentGraph property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestParentGraph2()
    {
		// Create a graph, populate the graph with vertices and edges, remove
		// the vertex from the graph.

		const Int32 Vertices = 100;

		// Create a graph with each vertex connected to all others.

		IGraph oGraph = new Graph();

		IVertex [] aoVertices = GraphUtil.AddVertices(oGraph, Vertices);

        GraphUtil.MakeGraphComplete(oGraph, aoVertices, false);

		// Pick one of the vertices to test.

        IVertex oVertex = aoVertices[0];

		Assert.AreEqual(oGraph, oVertex.ParentGraph);

		// Remove the vertex.

		oGraph.Vertices.Remove(oVertex);

		Assert.IsNull(oVertex.ParentGraph);
    }

    //*************************************************************************
    //  Method: TestIncomingEdges()
    //
    /// <summary>
    /// Tests the IncomingEdges property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestIncomingEdges()
    {
		TestIncomingOrOutgoingEdges(true);
    }

    //*************************************************************************
    //  Method: TestIncomingEdges2()
    //
    /// <summary>
    /// Tests the IncomingEdges property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestIncomingEdges2()
    {
		// Verify that a self-loop is handled correctly.

		TestLoopEdges(true, false, false);
    }

    //*************************************************************************
    //  Method: TestOutgoingEdges()
    //
    /// <summary>
    /// Tests the OutgoingEdges property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestOutgoingEdges()
    {
		TestIncomingOrOutgoingEdges(false);
    }

    //*************************************************************************
    //  Method: TestOutgoingEdges2()
    //
    /// <summary>
    /// Tests the OutgoingEdges property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestOutgoingEdges2()
    {
		// Verify that a self-loop is handled correctly.

		TestLoopEdges(false, true, false);
    }

    //*************************************************************************
    //  Method: TestIncidentEdges()
    //
    /// <summary>
    /// Tests the IncidentEdges and Degree properties.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestIncidentEdges()
    {
		// Create a graph and populate it with a random mix of incoming and
		// outgoing edges.

		IGraph oGraph = CreateAndPopulateGraph(100);

		// Loop through the graph's vertices.

		foreach (IVertex oVertex in oGraph.Vertices)
		{
			// Loop through the vertex's incident edges.

			IEdge [] aoIncidentEdges = oVertex.IncidentEdges;

            Assert.IsNotNull(aoIncidentEdges);

			Assert.AreEqual(aoIncidentEdges.Length, oVertex.Degree);

			foreach (IEdge oEdge in aoIncidentEdges)
			{
				// Verify that the edge is incident to the vertex.

				IVertex [] aoVertices = oEdge.Vertices;

				Assert.IsTrue(
					aoVertices[0] == oVertex || aoVertices[1] == oVertex);

				// Verify that the edge is in the graph.

				Assert.IsTrue( oGraph.Edges.Contains(oEdge) );
			}

			// Loop through the graph's edges.

			foreach (IEdge oEdge in oGraph.Edges)
			{
				// If the edge is incident to the vertex, verify that it's
				// included in the vertex's incident edges.

				IVertex [] aoVertices = oEdge.Vertices;

				if (aoVertices[0] == oVertex || aoVertices[1] == oVertex)
				{
					Assert.IsTrue(
						Array.IndexOf(aoIncidentEdges, oEdge) >= 0);
				}
			}
		}
    }

    //*************************************************************************
    //  Method: TestIncidentEdges2()
    //
    /// <summary>
    /// Tests the IncidentEdges and Degree properties.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestIncidentEdges2()
    {
		// Verify that the incoming edges are the union of the incoming and
		// outgoing edges.

		// Create a graph and populate it with a random mix of incoming and
		// outgoing edges.

		IGraph oGraph = CreateAndPopulateGraph(987);

		// Loop through the graph's vertices.

		foreach (IVertex oVertex in oGraph.Vertices)
		{
			// Get the vertex's incoming, outgoing, and incident edges.

			IEdge [] aoIncomingEdges = oVertex.IncomingEdges;
			IEdge [] aoOutgoingEdges = oVertex.OutgoingEdges;
			IEdge [] aoIncidentEdges = oVertex.IncidentEdges;

			Assert.IsTrue(aoIncomingEdges.Length +
				aoOutgoingEdges.Length >= aoIncidentEdges.Length);

			// Verify that each incoming edge is an incident edge.

			foreach (IEdge oEdge in aoIncomingEdges)
			{
				Assert.IsTrue(Array.IndexOf(aoIncidentEdges, oEdge) >= 0);
			}

			// Verify that each outgoing edge is an incident edge.

			foreach (IEdge oEdge in aoOutgoingEdges)
			{
				Assert.IsTrue(Array.IndexOf(aoIncidentEdges, oEdge) >= 0);
			}

			// Verify that each incident edge is either an incoming edge or an
			// outgoing edge.

			foreach (IEdge oEdge in aoIncidentEdges)
			{
				Assert.IsTrue(
					Array.IndexOf(aoIncomingEdges, oEdge) >= 0
					||
					Array.IndexOf(aoOutgoingEdges, oEdge) >= 0
					);
			}

			Assert.AreEqual(aoIncidentEdges.Length, oVertex.Degree);
		}
    }

    //*************************************************************************
    //  Method: TestIncidentEdges3()
    //
    /// <summary>
    /// Tests the IncidentEdges property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestIncidentEdges3()
    {
		// Verify that a self-loop is handled correctly.

		TestLoopEdges(false, false, true);
    }

    //*************************************************************************
    //  Method: TestGetConnectingEdges()
    //
    /// <summary>
    /// Tests the GetConnectingEdges() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetConnectingEdges()
    {
		// Create a graph and populate it with a random mix of incoming and
		// outgoing edges.

		IGraph oGraph = CreateAndPopulateGraph(25);

		// Loop through the graph's vertices.

		foreach (IVertex oVertex1 in oGraph.Vertices)
		{
			// Loop through all vertices except oVertex1.

			foreach (IVertex oVertex2 in oGraph.Vertices)
			{
				if (oVertex2 == oVertex1)
				{
					continue;
				}

				// Get oVertex1's connecting edges.

				IEdge [] aoConnectingEdges =
					oVertex1.GetConnectingEdges(oVertex2);

                Assert.IsNotNull(aoConnectingEdges);

				foreach (IEdge oConnectingEdge in aoConnectingEdges)
				{
					// Verify that the edge connects the two vertices.

					IVertex [] aoVertices = oConnectingEdge.Vertices;

					Assert.IsTrue(
						(aoVertices[0] == oVertex1 &&
							aoVertices[1] == oVertex2)
						||
						(aoVertices[1] == oVertex1 &&
							aoVertices[0] == oVertex2)
						);

					// Verify that the edge is in the graph.

					Assert.IsTrue( oGraph.Edges.Contains(oConnectingEdge) );
				}

				// Loop through the graph's edges.

				foreach (IEdge oEdge in oGraph.Edges)
				{
					// If the edge connects the two vertices, verify that it's
					// included in aoConnectingEdges.

					IVertex [] aoVertices = oEdge.Vertices;

					if (
						(aoVertices[0] == oVertex1 &&
							aoVertices[1] == oVertex2)
						||
						(aoVertices[1] == oVertex1 &&
							aoVertices[0] == oVertex2)
						)
					{
						Assert.IsTrue(
							Array.IndexOf(aoConnectingEdges, oEdge) >= 0);
					}
				}
			}
		}
    }

    //*************************************************************************
    //  Method: TestGetConnectingEdges2()
    //
    /// <summary>
    /// Tests the GetConnectingEdges() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetConnectingEdges2()
    {
		// Create a graph and populate it with multiple self-loops.

		IGraph oGraph = new Graph();

		// Add two vertices.

		IVertex [] aoVertices = GraphUtil.AddVertices(oGraph, 2);

		IVertex oVertex = aoVertices[0];

		IEdgeCollection oEdgeCollection = oGraph.Edges;

		// Add multiple self-loops, both undirected and directed.

		IEdge oUndirectedEdge1 = oEdgeCollection.Add(oVertex, oVertex, false);
		IEdge oUndirectedEdge2 = oEdgeCollection.Add(oVertex, oVertex, false);

		IEdge oDirectedEdge1 = oEdgeCollection.Add(oVertex, oVertex, true);
		IEdge oDirectedEdge2 = oEdgeCollection.Add(oVertex, oVertex, true);

		// Add a non-self-loop.

		IEdge oNonSelfLoopEdge = oEdgeCollection.Add(
			oVertex, aoVertices[1], false);

		// If GetConnectingEdges() is passed its own vertex, it should return
		// self-loops.

		IEdge [] aoConnectingEdges = oVertex.GetConnectingEdges(oVertex);

		Assert.IsNotNull(aoConnectingEdges);

		Assert.AreEqual(4, aoConnectingEdges.Length);

		// If GetConnectingEdges() is not passed its own vertex, it should not
		// return self-loops.

		aoConnectingEdges = oVertex.GetConnectingEdges( aoVertices[1] );

		Assert.IsNotNull(aoConnectingEdges);

		Assert.AreEqual(1, aoConnectingEdges.Length);

		Assert.AreEqual( oNonSelfLoopEdge, aoConnectingEdges[0] );
    }

    //*************************************************************************
    //  Method: TestGetConnectingEdgesBad()
    //
    /// <summary>
    /// Tests the GetConnectingEdges() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestGetConnectingEdgesBad()
    {
		// null otherVertex.

		try
		{
			IGraph oGraph = new Graph();

			IVertex [] aoVertices = GraphUtil.AddVertices(oGraph, 1);

			IEdge [] aoConnectingEdges =
				aoVertices[0].GetConnectingEdges(null);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Core."
				+ "Vertex.GetConnectingEdges: otherVertex argument can't be"
				+ " null.\r\n"
				+ "Parameter name: otherVertex"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestIsIncidentEdge()
    //
    /// <summary>
    /// Tests the IsIncidentEdge(), IsOutgoingEdge(), and IsIncomingEdge()
	/// methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestIsIncidentEdge()
    {
		IGraph oGraph = new Graph();

		IVertex [] aoVertices = GraphUtil.AddVertices(oGraph, 4);

		IVertex oVertex0 = aoVertices[0];
		IVertex oVertex1 = aoVertices[1];
		IVertex oVertex2 = aoVertices[2];
		IVertex oVertex3 = aoVertices[3];

		IEdgeCollection oEdges = oGraph.Edges;

		IEdge oUndirectedEdge = oEdges.Add(oVertex0, oVertex1, false);

		Assert.IsTrue( oVertex0.IsIncidentEdge(oUndirectedEdge) );
		Assert.IsTrue( oVertex0.IsOutgoingEdge(oUndirectedEdge) );
		Assert.IsTrue( oVertex0.IsIncomingEdge(oUndirectedEdge) );

		IEdge oIncomingEdge = oEdges.Add(oVertex2, oVertex0, true);

		Assert.IsTrue( oVertex0.IsIncidentEdge(oIncomingEdge) );
		Assert.IsFalse( oVertex0.IsOutgoingEdge(oIncomingEdge) );
		Assert.IsTrue( oVertex0.IsIncomingEdge(oIncomingEdge) );

		IEdge oOutgoingEdge = oEdges.Add(oVertex0, oVertex3, true);

		Assert.IsTrue( oVertex0.IsIncidentEdge(oOutgoingEdge) );
		Assert.IsTrue( oVertex0.IsOutgoingEdge(oOutgoingEdge) );
		Assert.IsFalse( oVertex0.IsIncomingEdge(oOutgoingEdge) );

		IEdge oUndirectedSelfLoop = oEdges.Add(oVertex0, oVertex0, false);

		Assert.IsTrue( oVertex0.IsIncidentEdge(oUndirectedSelfLoop) );
		Assert.IsTrue( oVertex0.IsOutgoingEdge(oUndirectedSelfLoop) );
		Assert.IsTrue( oVertex0.IsIncomingEdge(oUndirectedSelfLoop) );

		IEdge oDirectedSelfLoop = oEdges.Add(oVertex0, oVertex0, true);

		Assert.IsTrue( oVertex0.IsIncidentEdge(oDirectedSelfLoop) );
		Assert.IsTrue( oVertex0.IsOutgoingEdge(oDirectedSelfLoop) );
		Assert.IsTrue( oVertex0.IsIncomingEdge(oDirectedSelfLoop) );

		IEdge oDisconnectedEdge = oEdges.Add(oVertex2, oVertex3, false);

		Assert.IsFalse( oVertex0.IsIncidentEdge(oDisconnectedEdge) );
		Assert.IsFalse( oVertex0.IsOutgoingEdge(oDisconnectedEdge) );
		Assert.IsFalse( oVertex0.IsIncomingEdge(oDisconnectedEdge) );
    }

    //*************************************************************************
    //  Method: TestIsIncidentEdgeBad()
    //
    /// <summary>
    /// Tests the IsIncidentEdge() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestIsIncidentEdgeBad()
    {
		// null edge.

		try
		{
			IGraph oGraph = new Graph();

			IVertex [] aoVertices = GraphUtil.AddVertices(oGraph, 1);

			aoVertices[0].IsIncidentEdge(null);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Core."
				+ "Vertex.IsIncidentEdge: edge argument can't be null.\r\n"
				+ "Parameter name: edge"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestIsOutgoingEdgeBad()
    //
    /// <summary>
    /// Tests the IsOutgoingEdge() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestIsOutgoingEdgeBad()
    {
		// null edge.

		try
		{
			IGraph oGraph = new Graph();

			IVertex [] aoVertices = GraphUtil.AddVertices(oGraph, 1);

			aoVertices[0].IsOutgoingEdge(null);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Core."
				+ "Vertex.IsOutgoingEdge: edge argument can't be null.\r\n"
				+ "Parameter name: edge"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestIsIncomingEdgeBad()
    //
    /// <summary>
    /// Tests the IsIncomingEdge() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestIsIncomingEdgeBad()
    {
		// null edge.

		try
		{
			IGraph oGraph = new Graph();

			IVertex [] aoVertices = GraphUtil.AddVertices(oGraph, 1);

			aoVertices[0].IsIncomingEdge(null);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Core."
				+ "Vertex.IsIncomingEdge: edge argument can't be null.\r\n"
				+ "Parameter name: edge"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestPredecessorVertices()
    //
    /// <summary>
    /// Tests the PredecessorVertices property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestPredecessorVertices()
    {
		TestPredecessorOrSuccessorVertices(true);
    }

    //*************************************************************************
    //  Method: TestPredecessorVertices2()
    //
    /// <summary>
    /// Tests the PredecessorVertices property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestPredecessorVertices2()
    {
		// Verify that a self-loop is handled correctly.

		TestLoopVertices(true, false, false);
    }

    //*************************************************************************
    //  Method: TestSuccessorVertices()
    //
    /// <summary>
    /// Tests the SuccessorVertices property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSuccessorVertices()
    {
		TestPredecessorOrSuccessorVertices(false);
    }

    //*************************************************************************
    //  Method: TestSuccessorVertices2()
    //
    /// <summary>
    /// Tests the SuccessorVertices property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSuccessorVertices2()
    {
		// Verify that a self-loop is handled correctly.

		TestLoopVertices(false, true, false);
    }

    //*************************************************************************
    //  Method: TestAdjacentVertices()
    //
    /// <summary>
    /// Tests the AdjacentVertices property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdjacentVertices()
    {
		// Create a graph and populate it with a random mix of incoming and
		// outgoing edges.

		IGraph oGraph = CreateAndPopulateGraph(125);

		// Use a dictionary to keep track of the adjacent vertices that are
		// counted below.  The same vertex should not be counted twice.
		//
		// The dictionary key is the vertex ID and the value is the vertex.

		Dictionary<Int32, IVertex> oCountedVertices =
			new Dictionary<Int32, IVertex>();

		// Loop through the graph's vertices.

		foreach (IVertex oVertex in oGraph.Vertices)
		{
			// Get the vertex's adjacent vertices.

			IVertex [] aoAdjacentVertices = oVertex.AdjacentVertices;

			// Loop through the graph's edges.

			foreach (IEdge oEdge in oGraph.Edges)
			{
				IVertex oAdjacentVertex = null;

				// Get the edge's vertices.

				IVertex [] aoVertices = oEdge.Vertices;

				IVertex oVertex1 = aoVertices[0];
				IVertex oVertex2 = aoVertices[1];

				if (oVertex1 != oVertex && oVertex2 != oVertex)
				{
					// The edge is not incident to oVertex.

					continue;
				}

				if (oEdge.IsDirected)
				{
					if (oEdge.FrontVertex == oVertex)
					{
						// The edge's back vertex is an adjacent vertex.

						oAdjacentVertex = oEdge.BackVertex;
					}

					if (oEdge.BackVertex == oVertex)
					{
						// The edge's front vertex is an adjacent vertex.

						oAdjacentVertex = oEdge.FrontVertex;
					}
				}
				else
				{
					// The edge is undirected.

					if (oVertex1 == oVertex)
					{
						oAdjacentVertex = oVertex2;
					}
					else if (oVertex2 == oVertex)
					{
						oAdjacentVertex = oVertex1;
					}
					else
					{
						Assert.IsTrue(false);
					}
				}

				// If there is an adjacent vertex and it hasn't been added to
				// the dictionary yet, add it.

				if (
					oAdjacentVertex != null &&

					!oCountedVertices.ContainsKey(oAdjacentVertex.ID)
					)
				{
					oCountedVertices.Add(oAdjacentVertex.ID, oAdjacentVertex);
				}
			}

			// Verify that each vertex in oCountedVertices is included in
			// aoAdjacentVertices.

			foreach (IVertex oVertexA in oCountedVertices.Values)
			{
				Assert.IsTrue(Array.IndexOf(
					aoAdjacentVertices, oVertexA) >= 0);
			}

			// Verify that each vertex in aoAdjacentVertices is
			// in oCountedVertices.

			foreach (IVertex oVertexA in aoAdjacentVertices)
			{
				Assert.IsTrue( oCountedVertices.ContainsValue(oVertexA) );
			}

			// Verify that the number of vertices in aoAdjacentVertices is the
			// same as the number of vertices that were just counted.

			Assert.AreEqual(oCountedVertices.Count, aoAdjacentVertices.Length);

			oCountedVertices.Clear();
		}
    }

    //*************************************************************************
    //  Method: TestAdjacentVertices2()
    //
    /// <summary>
    /// Tests the AdjacentVertices property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdjacentVertices2()
    {
		// Create a graph and populate it with a random mix of incoming and
		// outgoing edges.

		IGraph oGraph = CreateAndPopulateGraph(987);

		// Loop through the graph's vertices.

		foreach (IVertex oVertex in oGraph.Vertices)
		{
			// Get the vertex's predecessor, successor, and adjacent vertices.

			IVertex [] aoPredecessorVertices = oVertex.PredecessorVertices;
			IVertex [] aoSuccessorVertices = oVertex.SuccessorVertices;
			IVertex [] aoAdjacentVertices = oVertex.AdjacentVertices;

			Assert.IsTrue(aoPredecessorVertices.Length +
				aoSuccessorVertices.Length >= aoAdjacentVertices.Length);

			// The adjacent vertices must be the union of the predecessor and
			// successor vertices.

			// Verify that each predecessor vertex is an adjacent vertex.

			foreach (IVertex oVertexA in aoPredecessorVertices)
			{
				Assert.IsTrue(Array.IndexOf(
					aoAdjacentVertices, oVertexA) >= 0);
			}

			// Verify that each successor vertex is an adjacent vertex.

			foreach (IVertex oVertexA in aoSuccessorVertices)
			{
				Assert.IsTrue(Array.IndexOf(
					aoAdjacentVertices, oVertexA) >= 0);
			}

			// Verify that each adjacent vertex is either a predecessor vertex
			// or a successor vertex.

			foreach (IVertex oVertexA in aoAdjacentVertices)
			{
				Assert.IsTrue(
					Array.IndexOf(aoPredecessorVertices, oVertexA) >= 0
					||
					Array.IndexOf(aoSuccessorVertices, oVertexA) >= 0
					);
			}
		}
    }

    //*************************************************************************
    //  Method: TestAdjacentVertices3()
    //
    /// <summary>
    /// Tests the AdjacentVertices property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdjacentVertices3()
    {
		// Verify that a self-loop is handled correctly.

		TestLoopVertices(false, false, true);
    }

    //*************************************************************************
    //  Method: TestClone()
    //
    /// <summary>
    /// Tests the Clone() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestClone()
    {
		TestClone(false, false, false);
    }

    //*************************************************************************
    //  Method: TestClone2()
    //
    /// <summary>
    /// Tests the Clone() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestClone2()
    {
		TestClone(false, false, true);
    }

    //*************************************************************************
    //  Method: TestClone3()
    //
    /// <summary>
    /// Tests the Clone() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestClone3()
    {
		TestClone(false, true, false);
    }

    //*************************************************************************
    //  Method: TestClone4()
    //
    /// <summary>
    /// Tests the Clone() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestClone4()
    {
		TestClone(false, true, true);
    }

    //*************************************************************************
    //  Method: TestClone5()
    //
    /// <summary>
    /// Tests the Clone() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestClone5()
    {
		TestClone(true, false, false);
    }

    //*************************************************************************
    //  Method: TestClone6()
    //
    /// <summary>
    /// Tests the Clone() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestClone6()
    {
		TestClone(true, false, true);
    }

    //*************************************************************************
    //  Method: TestClone7()
    //
    /// <summary>
    /// Tests the Clone() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestClone7()
    {
		TestClone(true, true, false);
    }

    //*************************************************************************
    //  Method: TestClone8()
    //
    /// <summary>
    /// Tests the Clone() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestClone8()
    {
		TestClone(true, true, true);
    }

    //*************************************************************************
    //  Method: TestCloneBad()
    //
    /// <summary>
    /// Tests the Clone() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestCloneBad()
    {
		// null newVertexFactory.

		try
		{
			IGraph oGraph = new Graph();

			IVertex [] aoVertices = GraphUtil.AddVertices(oGraph, 1);

			IVertex oCopy = aoVertices[0].Clone(true, true, null);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Core."
				+ "Vertex.Clone: newVertexFactory argument can't be null.\r\n"
				+ "Parameter name: newVertexFactory"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
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
		// Default format.

		Assert.AreEqual( "ID = " +
			m_oVertex.ID.ToString(NetMapBase.Int32Format),
			m_oVertex.ToString() );
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
		// null format.

		Assert.AreEqual( "ID = " +
			m_oVertex.ID.ToString(NetMapBase.Int32Format),
			m_oVertex.ToString(null) );
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
		// Empty string format.

		Assert.AreEqual( "ID = " +
			m_oVertex.ID.ToString(NetMapBase.Int32Format),
			m_oVertex.ToString(String.Empty) );
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
		// G format.

		Assert.AreEqual( "ID = " +
			m_oVertex.ID.ToString(NetMapBase.Int32Format),
			m_oVertex.ToString("G") );
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
		// P format, simple vertex.

		String sExpectedValue =

		"ID = " + m_oVertex.ID.ToString(NetMapBase.Int32Format) + "\r\n"
		+ "Name = [null]\r\n"
		+ "Tag = [null]\r\n"
		+ "Values = 0 key/value pairs\r\n"
		+ "AdjacentVertices = 0 vertices\r\n"
		+ "Degree = 0\r\n"
		+ "IncidentEdges = 0 edges\r\n"
		+ "IncomingEdges = 0 edges\r\n"
		+ "Location = {X=0, Y=0}\r\n"
		+ "OutgoingEdges = 0 edges\r\n"
		+ "ParentGraph = [null]\r\n"
		+ "PredecessorVertices = 0 vertices\r\n"
		+ "SuccessorVertices = 0 vertices\r\n"
		;

		Assert.AreEqual( sExpectedValue, m_oVertex.ToString("P") );
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
		// P format, vertex part of graph.

		const Int32 Vertices = 3;

		const String Name = "fjdkjre";
		const String Tag = "the tag";
		PointF Location = new PointF(45, -64);

		const String Key1 = "FirstKey";
		const String Value1 = "hello";

		const String Key2 = "SecondKey";
		const Int32 Value2 = 12345;

		// Create a graph with each vertex connected to all others.

		IGraph oGraph = new Graph();

		IVertex [] aoVertices = GraphUtil.AddVertices(oGraph, Vertices);

        GraphUtil.MakeGraphComplete(oGraph, aoVertices, false);

		// Pick one of the vertices to test.

        IVertex oVertex = aoVertices[0];

		// Set the vertex's properties.

		oVertex.Name = Name;

		oVertex.Tag = Tag;

		oVertex.Location = Location;

		oVertex.SetValue(Key1, Value1);
		oVertex.SetValue(Key2, Value2);

		String sExpectedValue =

		"ID = " + oVertex.ID.ToString(NetMapBase.Int32Format) + "\r\n"
		+ "Name = " + Name + "\r\n"
		+ "Tag = " + Tag + "\r\n"
		+ "Values = 2 key/value pairs\r\n"
		+ "AdjacentVertices = 2 vertices\r\n"
		+ "Degree = 2\r\n"
		+ "IncidentEdges = 2 edges\r\n"
		+ "IncomingEdges = 2 edges\r\n"
		+ "Location = " + Location + "\r\n"
		+ "OutgoingEdges = 2 edges\r\n"

		+ "ParentGraph = ID = " + oGraph.ID.ToString(NetMapBase.Int32Format)
			+ "\r\n"

		+ "PredecessorVertices = 2 vertices\r\n"
		+ "SuccessorVertices = 2 vertices\r\n"
		;

		Assert.AreEqual( sExpectedValue, oVertex.ToString("P") );
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
		// D format, simple vertex.

		String sExpectedValue =

		"ID = " + m_oVertex.ID.ToString(NetMapBase.Int32Format) + "\r\n"
		+ "Name = [null]\r\n"
		+ "Tag = [null]\r\n"
		+ "Values = 0 key/value pairs\r\n"
		+ "AdjacentVertices = 0 vertices\r\n"
		+ "Degree = 0\r\n"
		+ "IncidentEdges = 0 edges\r\n"
		+ "IncomingEdges = 0 edges\r\n"
		+ "Location = {X=0, Y=0}\r\n"
		+ "OutgoingEdges = 0 edges\r\n"
		+ "ParentGraph = [null]\r\n"
		+ "PredecessorVertices = 0 vertices\r\n"
		+ "SuccessorVertices = 0 vertices\r\n"
		;

		Assert.AreEqual( sExpectedValue, m_oVertex.ToString("D") );
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
		// D format, vertex part of graph.

		const Int32 Vertices = 3;

		const String Name = "fjdkjre";
		const String Tag = "the tag";
		PointF Location = new PointF(45, -64);

		const String Key1 = "FirstKey";
		const String Value1 = "hello";

		const String Key2 = "SecondKey";
		const Int32 Value2 = 12345;

		// Create a graph with each vertex connected to all others.

		IGraph oGraph = new Graph();

		IVertex [] aoVertices = GraphUtil.AddVertices(oGraph, Vertices);

        GraphUtil.MakeGraphComplete(oGraph, aoVertices, false);

		// Pick one of the vertices to test.

        IVertex oVertex = aoVertices[0];

		// Set the vertex's properties.

		oVertex.Name = Name;

		oVertex.Tag = Tag;

		oVertex.Location = Location;

		oVertex.SetValue(Key1, Value1);
		oVertex.SetValue(Key2, Value2);

		String sExpectedValue =

		"ID = " + oVertex.ID.ToString(NetMapBase.Int32Format) + "\r\n"
		+ "Name = " + Name + "\r\n"
		+ "Tag = " + Tag + "\r\n"
		+ "Values = 2 key/value pairs\r\n"
		+ "\tKey = " + Key1 + ", Value = " + Value1 + "\r\n"
		+ "\tKey = " + Key2 + ", Value = " + Value2 + "\r\n"

		+ "AdjacentVertices = 2 vertices\r\n"
		+ "\tID = " + oVertex.AdjacentVertices[0].ID.ToString(
			NetMapBase.Int32Format) + "\r\n"
		+ "\tID = " + oVertex.AdjacentVertices[1].ID.ToString(
			NetMapBase.Int32Format) + "\r\n"

		+ "Degree = 2\r\n"

		+ "IncidentEdges = 2 edges\r\n"
		+ "\tID = " + oVertex.IncidentEdges[0].ID.ToString(
			NetMapBase.Int32Format) + "\r\n"
		+ "\tID = " + oVertex.IncidentEdges[1].ID.ToString(
			NetMapBase.Int32Format) + "\r\n"

		+ "IncomingEdges = 2 edges\r\n"
		+ "\tID = " + oVertex.IncomingEdges[0].ID.ToString(
			NetMapBase.Int32Format) + "\r\n"
		+ "\tID = " + oVertex.IncomingEdges[1].ID.ToString(
			NetMapBase.Int32Format) + "\r\n"

		+ "Location = " + Location + "\r\n"

		+ "OutgoingEdges = 2 edges\r\n"
		+ "\tID = " + oVertex.OutgoingEdges[0].ID.ToString(
			NetMapBase.Int32Format) + "\r\n"
		+ "\tID = " + oVertex.OutgoingEdges[1].ID.ToString(
			NetMapBase.Int32Format) + "\r\n"

		+ "ParentGraph = ID = " + oGraph.ID.ToString(NetMapBase.Int32Format)
			+ "\r\n"

		+ "PredecessorVertices = 2 vertices\r\n"
		+ "\tID = " + oVertex.PredecessorVertices[0].ID.ToString(
			NetMapBase.Int32Format) + "\r\n"
		+ "\tID = " + oVertex.PredecessorVertices[1].ID.ToString(
			NetMapBase.Int32Format) + "\r\n"

		+ "SuccessorVertices = 2 vertices\r\n"
		+ "\tID = " + oVertex.SuccessorVertices[0].ID.ToString(
			NetMapBase.Int32Format) + "\r\n"
		+ "\tID = " + oVertex.SuccessorVertices[1].ID.ToString(
			NetMapBase.Int32Format) + "\r\n"
		;

		Assert.AreEqual( sExpectedValue, oVertex.ToString("D") );
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
			m_oVertex.ToString("Bad");
		}
		catch (FormatException oFormatException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Core."
				+ "Vertex.ToString: Invalid format.  Available formats are"
				+ " G, P, and D."
				,
				oFormatException.Message
				);

			throw oFormatException;
		}
    }

    //*************************************************************************
    //  Method: TestIncomingOrOutgoingEdges()
    //
    /// <summary>
    /// Tests the IncomingEdges or OutgoingEdges property.
    /// </summary>
	///
	/// <param name="bIncoming">
	/// true to test the IncomingEdges property, false to test the
	/// OutgoingEdges property.
	/// </param>
    //*************************************************************************

    protected void
    TestIncomingOrOutgoingEdges
	(
		Boolean bIncoming
	)
    {
		// Create a graph and populate it with a random mix of incoming and
		// outgoing edges.

		IGraph oGraph = CreateAndPopulateGraph(143);

		// Loop through the graph's vertices.

		foreach (IVertex oVertex in oGraph.Vertices)
		{
			// Loop through the vertex's incoming or outgoing edges.

			IEdge [] aoIncomingOrOutgoingEdges =
				bIncoming ? oVertex.IncomingEdges : oVertex.OutgoingEdges;

            Assert.IsNotNull(aoIncomingOrOutgoingEdges);

			foreach (IEdge oEdge in aoIncomingOrOutgoingEdges)
			{
				// An incoming edge is either a directed edge that has this
				// vertex at its front, or an undirected edge connected to this
				// vertex. 
				///
				// An outgoing edge is either a directed edge that has this
				// vertex at its back, or an undirected edge connected to this
				// vertex. 
				//
				// Verify that the edge is incoming or outgoing.

				if (oEdge.IsDirected)
				{
					Assert.AreEqual(oVertex,
						bIncoming ? oEdge.FrontVertex : oEdge.BackVertex);
				}
				else
				{
					IVertex [] aoVertices = oEdge.Vertices;

					Assert.IsTrue(
						aoVertices[0] == oVertex || aoVertices[1] == oVertex);
				}

				// Verify that the edge is in the graph.

				Assert.IsTrue( oGraph.Edges.Contains(oEdge) );
			}

			// Loop through the graph's edges.

			foreach (IEdge oEdge in oGraph.Edges)
			{
				// If the edge is incoming (or outgoing), verify that it's
				/// included in the vertex's incoming (or incoming) edges.

				Boolean bShouldBeInArray = false;

				if (oEdge.IsDirected)
				{
					if ( oVertex ==
						(bIncoming ? oEdge.FrontVertex : oEdge.BackVertex) )
					{
						bShouldBeInArray = true;
					}
				}
				else
				{
					IVertex [] aoVertices = oEdge.Vertices;

					if (aoVertices[0] == oVertex || aoVertices[1] == oVertex)
					{
						bShouldBeInArray = true;
					}
				}

				if (bShouldBeInArray)
				{
					Assert.IsTrue(
						Array.IndexOf(aoIncomingOrOutgoingEdges, oEdge) >= 0);
				}
			}
		}
    }

    //*************************************************************************
    //  Method: TestPredecessorOrSuccessorVertices()
    //
    /// <summary>
    /// Tests the PredecessorVertices or SuccessorVertices property.
    /// </summary>
	///
	/// <param name="bPredecessor">
	/// true to test the PredecessorVertices property, false to test the
	/// SuccessorVertices property.
	/// </param>
    //*************************************************************************

    protected void
    TestPredecessorOrSuccessorVertices
	(
		Boolean bPredecessor
	)
    {
		// Create a graph and populate it with a random mix of incoming and
		// outgoing edges.

		IGraph oGraph = CreateAndPopulateGraph(100);

		// Use a dictionary to keep track of the predecessor or successor
		// vertices that are counted below.  The same vertex should not be
		// counted twice.
		//
		// The dictionary key is the vertex ID and the value is the vertex.

		Dictionary<Int32, IVertex> oCountedVertices =
			new Dictionary<Int32, IVertex>();

		// Loop through the graph's vertices.

		foreach (IVertex oVertex in oGraph.Vertices)
		{
			// Get the vertex's predecessor or successor vertices.

			IVertex [] aoPredecessorOrSuccessorVertices = bPredecessor ?
				oVertex.PredecessorVertices : oVertex.SuccessorVertices;

			// Loop through the graph's edges.

			foreach (IEdge oEdge in oGraph.Edges)
			{
				IVertex oPredecessorOrSuccessorVertex = null;

				// Get the edge's vertices.

				IVertex [] aoVertices = oEdge.Vertices;

				IVertex oVertex1 = aoVertices[0];
				IVertex oVertex2 = aoVertices[1];

				if (oVertex1 != oVertex && oVertex2 != oVertex)
				{
					// The edge is not incident to oVertex.

					continue;
				}

				if (oEdge.IsDirected)
				{
					if (oEdge.FrontVertex == oVertex)
					{
						// The edge is incoming.

						if (bPredecessor)
						{
							// The edge's back vertex is a predecessor vertex.

							oPredecessorOrSuccessorVertex = oEdge.BackVertex;
						}
					}

					if (oEdge.BackVertex == oVertex)
					{
						// The edge is outgoing.

						if (!bPredecessor)
						{
							// The edge's front vertex is a successor vertex.

							oPredecessorOrSuccessorVertex = oEdge.FrontVertex;
						}
					}
				}
				else
				{
					// The edge is undirected.

					if (oVertex1 == oVertex)
					{
						// oVertex2 is both a predecessor and successor vertex.

						oPredecessorOrSuccessorVertex = oVertex2;
					}
					else if (oVertex2 == oVertex)
					{
						// oVertex1 is both a predecessor and successor vertex.

						oPredecessorOrSuccessorVertex = oVertex1;
					}
					else
					{
						Assert.IsTrue(false);
					}
				}

				// If there is a predecessor or successor vertex and it hasn't
				// been added to the dictionary yet, add it.

				if (
					oPredecessorOrSuccessorVertex != null &&

					!oCountedVertices.ContainsKey(
						oPredecessorOrSuccessorVertex.ID)
					)
				{
					oCountedVertices.Add(oPredecessorOrSuccessorVertex.ID,
						oPredecessorOrSuccessorVertex);
				}
			}

			// Verify that each vertex in oCountedVertices is included in
			// aoPredecessorOrSuccessorVertices.

			foreach (IVertex oVertexA in oCountedVertices.Values)
			{
				Assert.IsTrue(Array.IndexOf(
					aoPredecessorOrSuccessorVertices, oVertexA) >= 0);
			}

			// Verify that each vertex in aoPredecessorOrSuccessorVertices is
			// in oCountedVertices.

			foreach (IVertex oVertexA in aoPredecessorOrSuccessorVertices)
			{
				Assert.IsTrue( oCountedVertices.ContainsValue(oVertexA) );
			}

			// Verify that the number of vertices in
			// aoPredecessorOrSuccessorVertices is the same as the number of
			// vertices that were just counted.

			Assert.AreEqual(oCountedVertices.Count,
				aoPredecessorOrSuccessorVertices.Length);

			oCountedVertices.Clear();
		}
    }

    //*************************************************************************
    //  Method: CreateAndPopulateGraph()
    //
    /// <summary>
	/// Creates a graph and populates it with a random mix of incoming and
	/// outgoing edges.
    /// </summary>
	///
	/// <param name="iVertices">
	/// Number of vertices to add to the graph.
	/// </param>
	///
	/// <returns>
	/// The new graph.
	/// </returns>
    //*************************************************************************

    protected IGraph
    CreateAndPopulateGraph
	(
		Int32 iVertices
	)
    {
		Debug.Assert(iVertices >= 0);

		Random oRandom = new Random(1);

		IGraph oGraph = new Graph();

		// Add the vertices.

		IVertex [] aoVertices = GraphUtil.AddVertices(oGraph, iVertices);

		IEdgeCollection oEdgeCollection = oGraph.Edges;

		// Add random directed and undirected edges.

		Int32 iRandomEdges = 100 * iVertices;

		for (Int32 i = 0; i < iRandomEdges; i++)
		{
			Int32 iVertex1Index = oRandom.Next(iVertices);
			Int32 iVertex2Index = oRandom.Next(iVertices);

			Boolean bIsDirected = (oRandom.Next(2) == 0);

			oEdgeCollection.Add(
				aoVertices[iVertex2Index], aoVertices[iVertex1Index],
				bIsDirected);
		}

		return (oGraph);
    }

    //*************************************************************************
    //  Method: TestLoopEdges()
    //
    /// <summary>
    /// Tests the IncomingEdges, OutgoingEdges, or IncidentEdges property when
	/// an edge is a self-loop (an edge that connects a vertex to itself).
    /// </summary>
	///
	/// <param name="bTestIncomingEdges">
	/// true to test the IncomingEdges property.
	/// </param>
	///
	/// <param name="bTestOutgoingEdges">
	/// true to test the OutgoingEdges property.
	/// </param>
	///
	/// <param name="bTestIncidentEdges">
	/// true to test the IncidentEdges property.
	/// </param>
    //*************************************************************************

    protected void
    TestLoopEdges
	(
		Boolean bTestIncomingEdges,
		Boolean bTestOutgoingEdges,
		Boolean bTestIncidentEdges
	)
    {
		IGraph oGraph = new Graph();

		// Add one vertex.

		IVertex [] aoVertices = GraphUtil.AddVertices(oGraph, 1);

		IVertex oVertex = aoVertices[0];

		IEdgeCollection oEdgeCollection = oGraph.Edges;

		// Add multiple self-loops, both undirected and directed.

		IEdge oUndirectedEdge1 = oEdgeCollection.Add(oVertex, oVertex, false);
		IEdge oUndirectedEdge2 = oEdgeCollection.Add(oVertex, oVertex, false);

		IEdge oDirectedEdge1 = oEdgeCollection.Add(oVertex, oVertex, true);
		IEdge oDirectedEdge2 = oEdgeCollection.Add(oVertex, oVertex, true);

		IEdge [] aoEdges = null;

		if (bTestIncomingEdges)
		{
			aoEdges = oVertex.IncomingEdges;
		}
		else if (bTestOutgoingEdges)
		{
			aoEdges = oVertex.OutgoingEdges;
		}
		else if (bTestIncidentEdges)
		{
			aoEdges = oVertex.IncidentEdges;

			Assert.AreEqual(aoEdges.Length, oVertex.Degree);
		}
		else
		{
			Debug.Assert(false);
		}

		Assert.AreEqual(4, aoEdges.Length);

		Assert.IsTrue(Array.IndexOf(aoEdges, oUndirectedEdge1) >= 0);
		Assert.IsTrue(Array.IndexOf(aoEdges, oUndirectedEdge2) >= 0);

		Assert.IsTrue(Array.IndexOf(aoEdges, oDirectedEdge1) >= 0);
		Assert.IsTrue(Array.IndexOf(aoEdges, oDirectedEdge2) >= 0);
    }

    //*************************************************************************
    //  Method: TestLoopVertices()
    //
    /// <summary>
    /// Tests the PredecessorVertices, SuccessorVertices, or AdjacentVertices
	/// property when an edge is a self-loop (an edge that connects a vertex to
	/// itself).
    /// </summary>
	///
	/// <param name="bTestPredecessorVertices">
	/// true to test the PredecessorVertices property.
	/// </param>
	///
	/// <param name="bTestSuccessorVertices">
	/// true to test the SuccessorVertices property.
	/// </param>
	///
	/// <param name="bTestAdjacentVertices">
	/// true to test the AdjacentVertices property.
	/// </param>
    //*************************************************************************

    protected void
    TestLoopVertices
	(
		Boolean bTestPredecessorVertices,
		Boolean bTestSuccessorVertices,
		Boolean bTestAdjacentVertices
	)
    {
		IGraph oGraph = new Graph();

		// Add one vertex.

		IVertex [] aoVertices = GraphUtil.AddVertices(oGraph, 1);

		IVertex oVertex = aoVertices[0];

		IEdgeCollection oEdgeCollection = oGraph.Edges;

		// Add multiple self-loops, both undirected and directed.

		IEdge oUndirectedEdge1 = oEdgeCollection.Add(oVertex, oVertex, false);
		IEdge oUndirectedEdge2 = oEdgeCollection.Add(oVertex, oVertex, false);

		IEdge oDirectedEdge1 = oEdgeCollection.Add(oVertex, oVertex, true);
		IEdge oDirectedEdge2 = oEdgeCollection.Add(oVertex, oVertex, true);

		IVertex [] aoVerticesA = null;

		if (bTestPredecessorVertices)
		{
			aoVerticesA = oVertex.PredecessorVertices;
		}
		else if (bTestSuccessorVertices)
		{
			aoVerticesA = oVertex.SuccessorVertices;
		}
		else if (bTestAdjacentVertices)
		{
			aoVerticesA = oVertex.AdjacentVertices;
		}
		else
		{
			Debug.Assert(false);
		}

		Assert.AreEqual(1, aoVerticesA.Length);

		Assert.IsTrue(Array.IndexOf(aoVerticesA, oVertex) >= 0);
    }

    //*************************************************************************
    //  Method: TestClone()
    //
    /// <summary>
    /// Tests the Clone() methods.
    /// </summary>
	///
	/// <param name="bUseFirstOverload">
	/// true to test <see cref="Clone(Boolean,Boolean)" />, false to test <see
	/// cref="Clone(Boolean, Boolean, IVertexFactory" />.
	/// </param>
	///
	/// <param name="bCopyMetadataValues">
	/// true to copy metadata values while cloning.
	/// </param>
	///
	/// <param name="bCopyTag">
	/// true to copy tag values while cloning.
	/// </param>
    //*************************************************************************

    protected void
    TestClone
	(
		Boolean bUseFirstOverload,
		Boolean bCopyMetadataValues,
		Boolean bCopyTag
	)
    {
		// Create N objects, set random metadata and Tag on each object, clone
		// each object, check new object.

        const Int32 Vertices = 1000;

		Vertex [] aoVertices = new Vertex[Vertices];

		VertexFactory oVertexFactory = new VertexFactory();

		// Set random values on each object.

		for (Int32 i = 0; i < Vertices; i++)
		{
			Vertex oVertex = aoVertices[i] =
				(Vertex)oVertexFactory.CreateVertex();

			MetadataUtil.SetRandomMetadata(oVertex, true, true, i);

			oVertex.Name = oVertex.ID.ToString();
		}

		for (Int32 i = 0; i < Vertices; i++)
		{
			// Clone the object.

			Vertex oVertex = aoVertices[i];

			Vertex oNewVertex = (Vertex)
				(
				bUseFirstOverload ?

				oVertex.Clone(bCopyMetadataValues, bCopyTag)
				:
				oVertex.Clone(bCopyMetadataValues, bCopyTag,
					new VertexFactory() )
				)
				;

			// Check the metadata on the new object.

			MetadataUtil.CheckRandomMetadata(
				oNewVertex, bCopyMetadataValues, bCopyTag, i);

			// Check the name and ID on the new object.

			Assert.AreEqual(oVertex.Name, oNewVertex.Name);

			Assert.AreNotEqual(oVertex.ID, oNewVertex.ID);
		}
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected Vertex m_oVertex;
}

}
