
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NetMap.Core;
using Microsoft.NetMap.ExcelTemplate;

namespace Microsoft.NetMap.UnitTests
{
//*****************************************************************************
//  Class: DuplicateEdgeDetectorTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="DuplicateEdgeDetector" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class DuplicateEdgeDetectorTest : Object
{
    //*************************************************************************
    //  Constructor: DuplicateEdgeDetectorTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="DuplicateEdgeDetectorTest" /> class.
    /// </summary>
    //*************************************************************************

    public DuplicateEdgeDetectorTest()
    {
		m_oDuplicateEdgeDetector = null;

		m_oDirectedGraph = null;
		m_oDirectedVertexA = null;
		m_oDirectedVertexB = null;
		m_oDirectedVertexC = null;
		m_oDirectedVertexD = null;
		m_oDirectedVertexWithNullName = null;

		m_oUndirectedGraph = null;
		m_oUndirectedVertexA = null;
		m_oUndirectedVertexB = null;
		m_oUndirectedVertexC = null;
		m_oUndirectedVertexD = null;
		m_oUndirectedVertexWithNullName = null;
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
		m_oDirectedGraph = new Graph(GraphDirectedness.Directed);
		IVertexCollection oDirectedVertices = m_oDirectedGraph.Vertices;

		m_oDirectedVertexA = oDirectedVertices.Add();
		m_oDirectedVertexA.Name = "A";

		m_oDirectedVertexB = oDirectedVertices.Add();
		m_oDirectedVertexB.Name = "B";

		m_oDirectedVertexC = oDirectedVertices.Add();
		m_oDirectedVertexC.Name = "C";

		m_oDirectedVertexD = oDirectedVertices.Add();
		m_oDirectedVertexD.Name = "D";

		m_oDirectedVertexWithNullName = oDirectedVertices.Add();
		m_oDirectedVertexWithNullName.Name = null;


		m_oUndirectedGraph = new Graph(GraphDirectedness.Undirected);
		IVertexCollection oUndirectedVertices = m_oUndirectedGraph.Vertices;

		m_oUndirectedVertexA = oUndirectedVertices.Add();
		m_oUndirectedVertexA.Name = "A";

		m_oUndirectedVertexB = oUndirectedVertices.Add();
		m_oUndirectedVertexB.Name = "B";

		m_oUndirectedVertexC = oUndirectedVertices.Add();
		m_oUndirectedVertexC.Name = "C";

		m_oUndirectedVertexD = oUndirectedVertices.Add();
		m_oUndirectedVertexD.Name = "D";

		m_oUndirectedVertexWithNullName = oUndirectedVertices.Add();
		m_oUndirectedVertexWithNullName.Name = null;
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
		m_oDuplicateEdgeDetector = null;

		m_oDirectedGraph = null;
		m_oDirectedVertexA = null;
		m_oDirectedVertexB = null;
		m_oDirectedVertexC = null;
		m_oDirectedVertexD = null;
		m_oDirectedVertexWithNullName = null;

		m_oUndirectedGraph = null;
		m_oUndirectedVertexA = null;
		m_oUndirectedVertexB = null;
		m_oUndirectedVertexC = null;
		m_oUndirectedVertexD = null;
		m_oUndirectedVertexWithNullName = null;
    }

    //*************************************************************************
    //  Method: TestGraphContainsDuplicateEdges()
    //
    /// <summary>
    /// Tests the GraphContainsDuplicateEdges() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraphContainsDuplicateEdges()
    {
		// Directed graph, no duplicate edges.

		m_oDuplicateEdgeDetector = new DuplicateEdgeDetector(m_oDirectedGraph);

		IEdgeCollection oEdges = m_oDirectedGraph.Edges;

		oEdges.Add(m_oDirectedVertexA, m_oDirectedVertexB, true);
		oEdges.Add(m_oDirectedVertexB, m_oDirectedVertexA, true);
		oEdges.Add(m_oDirectedVertexA, m_oDirectedVertexA, true);
		oEdges.Add(m_oDirectedVertexB, m_oDirectedVertexB, true);
		oEdges.Add(m_oDirectedVertexC, m_oDirectedVertexD, true);
		oEdges.Add(m_oDirectedVertexC, m_oDirectedVertexWithNullName, true);

		Assert.IsFalse(m_oDuplicateEdgeDetector.GraphContainsDuplicateEdges);
		Assert.AreEqual(5, m_oDuplicateEdgeDetector.Edges);
		Assert.AreEqual(5, m_oDuplicateEdgeDetector.UniqueEdges);
		Assert.AreEqual(0, m_oDuplicateEdgeDetector.EdgesWithDuplicates);
    }

    //*************************************************************************
    //  Method: TestGraphContainsDuplicateEdges2()
    //
    /// <summary>
    /// Tests the GraphContainsDuplicateEdges() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraphContainsDuplicateEdges2()
    {
		// Directed graph, duplicate A,B.

		m_oDuplicateEdgeDetector = new DuplicateEdgeDetector(m_oDirectedGraph);

		IEdgeCollection oEdges = m_oDirectedGraph.Edges;

		oEdges.Add(m_oDirectedVertexA, m_oDirectedVertexB, true);
		oEdges.Add(m_oDirectedVertexB, m_oDirectedVertexA, true);
		oEdges.Add(m_oDirectedVertexA, m_oDirectedVertexA, true);
		oEdges.Add(m_oDirectedVertexB, m_oDirectedVertexB, true);
		oEdges.Add(m_oDirectedVertexC, m_oDirectedVertexD, true);
		oEdges.Add(m_oDirectedVertexC, m_oDirectedVertexWithNullName, true);

		oEdges.Add(m_oDirectedVertexA, m_oDirectedVertexB, true);

		Assert.IsTrue(m_oDuplicateEdgeDetector.GraphContainsDuplicateEdges);
		Assert.AreEqual(6, m_oDuplicateEdgeDetector.Edges);
		Assert.AreEqual(4, m_oDuplicateEdgeDetector.UniqueEdges);
		Assert.AreEqual(2, m_oDuplicateEdgeDetector.EdgesWithDuplicates);
    }

    //*************************************************************************
    //  Method: TestGraphContainsDuplicateEdges3()
    //
    /// <summary>
    /// Tests the GraphContainsDuplicateEdges() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraphContainsDuplicateEdges3()
    {
		// Directed graph, duplicate B,A.

		m_oDuplicateEdgeDetector = new DuplicateEdgeDetector(m_oDirectedGraph);

		IEdgeCollection oEdges = m_oDirectedGraph.Edges;

		oEdges.Add(m_oDirectedVertexA, m_oDirectedVertexB, true);
		oEdges.Add(m_oDirectedVertexB, m_oDirectedVertexA, true);
		oEdges.Add(m_oDirectedVertexA, m_oDirectedVertexA, true);
		oEdges.Add(m_oDirectedVertexB, m_oDirectedVertexB, true);
		oEdges.Add(m_oDirectedVertexC, m_oDirectedVertexD, true);
		oEdges.Add(m_oDirectedVertexC, m_oDirectedVertexWithNullName, true);

		oEdges.Add(m_oDirectedVertexB, m_oDirectedVertexA, true);

		Assert.IsTrue(m_oDuplicateEdgeDetector.GraphContainsDuplicateEdges);
		Assert.AreEqual(6, m_oDuplicateEdgeDetector.Edges);
		Assert.AreEqual(4, m_oDuplicateEdgeDetector.UniqueEdges);
		Assert.AreEqual(2, m_oDuplicateEdgeDetector.EdgesWithDuplicates);
    }

    //*************************************************************************
    //  Method: TestGraphContainsDuplicateEdges4()
    //
    /// <summary>
    /// Tests the GraphContainsDuplicateEdges() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraphContainsDuplicateEdges4()
    {
		// Directed graph, duplicate A,A.

		m_oDuplicateEdgeDetector = new DuplicateEdgeDetector(m_oDirectedGraph);

		IEdgeCollection oEdges = m_oDirectedGraph.Edges;

		oEdges.Add(m_oDirectedVertexA, m_oDirectedVertexB, true);
		oEdges.Add(m_oDirectedVertexB, m_oDirectedVertexA, true);
		oEdges.Add(m_oDirectedVertexA, m_oDirectedVertexA, true);
		oEdges.Add(m_oDirectedVertexB, m_oDirectedVertexB, true);
		oEdges.Add(m_oDirectedVertexC, m_oDirectedVertexD, true);
		oEdges.Add(m_oDirectedVertexC, m_oDirectedVertexWithNullName, true);

		oEdges.Add(m_oDirectedVertexA, m_oDirectedVertexA, true);

		Assert.IsTrue(m_oDuplicateEdgeDetector.GraphContainsDuplicateEdges);
		Assert.AreEqual(6, m_oDuplicateEdgeDetector.Edges);
		Assert.AreEqual(4, m_oDuplicateEdgeDetector.UniqueEdges);
		Assert.AreEqual(2, m_oDuplicateEdgeDetector.EdgesWithDuplicates);
    }

    //*************************************************************************
    //  Method: TestGraphContainsDuplicateEdges5()
    //
    /// <summary>
    /// Tests the GraphContainsDuplicateEdges() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraphContainsDuplicateEdges5()
    {
		// Directed graph, duplicate C, null name

		m_oDuplicateEdgeDetector = new DuplicateEdgeDetector(m_oDirectedGraph);

		IEdgeCollection oEdges = m_oDirectedGraph.Edges;

		oEdges.Add(m_oDirectedVertexA, m_oDirectedVertexB, true);
		oEdges.Add(m_oDirectedVertexB, m_oDirectedVertexA, true);
		oEdges.Add(m_oDirectedVertexA, m_oDirectedVertexA, true);
		oEdges.Add(m_oDirectedVertexB, m_oDirectedVertexB, true);
		oEdges.Add(m_oDirectedVertexC, m_oDirectedVertexD, true);
		oEdges.Add(m_oDirectedVertexC, m_oDirectedVertexWithNullName, true);

		oEdges.Add(m_oDirectedVertexC, m_oDirectedVertexWithNullName, true);

		Assert.IsFalse(m_oDuplicateEdgeDetector.GraphContainsDuplicateEdges);
		Assert.AreEqual(5, m_oDuplicateEdgeDetector.Edges);
		Assert.AreEqual(5, m_oDuplicateEdgeDetector.UniqueEdges);
		Assert.AreEqual(0, m_oDuplicateEdgeDetector.EdgesWithDuplicates);
    }

    //*************************************************************************
    //  Method: TestGraphContainsDuplicateEdges6()
    //
    /// <summary>
    /// Tests the GraphContainsDuplicateEdges() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraphContainsDuplicateEdges6()
    {
		// Directed graph, non-duplicate null name, C.

		m_oDuplicateEdgeDetector = new DuplicateEdgeDetector(m_oDirectedGraph);

		IEdgeCollection oEdges = m_oDirectedGraph.Edges;

		oEdges.Add(m_oDirectedVertexA, m_oDirectedVertexB, true);
		oEdges.Add(m_oDirectedVertexB, m_oDirectedVertexA, true);
		oEdges.Add(m_oDirectedVertexA, m_oDirectedVertexA, true);
		oEdges.Add(m_oDirectedVertexB, m_oDirectedVertexB, true);
		oEdges.Add(m_oDirectedVertexC, m_oDirectedVertexD, true);
		oEdges.Add(m_oDirectedVertexC, m_oDirectedVertexWithNullName, true);

		oEdges.Add(m_oDirectedVertexWithNullName, m_oDirectedVertexC, true);

		Assert.IsFalse(m_oDuplicateEdgeDetector.GraphContainsDuplicateEdges);
		Assert.AreEqual(5, m_oDuplicateEdgeDetector.Edges);
		Assert.AreEqual(5, m_oDuplicateEdgeDetector.UniqueEdges);
		Assert.AreEqual(0, m_oDuplicateEdgeDetector.EdgesWithDuplicates);
    }

    //*************************************************************************
    //  Method: TestGraphContainsDuplicateEdges7()
    //
    /// <summary>
    /// Tests the GraphContainsDuplicateEdges() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraphContainsDuplicateEdges7()
    {
		// Directed graph, many duplicates.

		m_oDuplicateEdgeDetector = new DuplicateEdgeDetector(m_oDirectedGraph);

		IEdgeCollection oEdges = m_oDirectedGraph.Edges;

		oEdges.Add(m_oDirectedVertexA, m_oDirectedVertexB, true);
		oEdges.Add(m_oDirectedVertexA, m_oDirectedVertexA, true);
		oEdges.Add(m_oDirectedVertexA, m_oDirectedVertexA, true);
		oEdges.Add(m_oDirectedVertexA, m_oDirectedVertexB, true);
		oEdges.Add(m_oDirectedVertexB, m_oDirectedVertexA, true);
		oEdges.Add(m_oDirectedVertexB, m_oDirectedVertexB, true);
		oEdges.Add(m_oDirectedVertexB, m_oDirectedVertexA, true);
		oEdges.Add(m_oDirectedVertexB, m_oDirectedVertexA, true);
		oEdges.Add(m_oDirectedVertexB, m_oDirectedVertexB, true);
		oEdges.Add(m_oDirectedVertexA, m_oDirectedVertexA, true);
		oEdges.Add(m_oDirectedVertexC, m_oDirectedVertexD, true);
		oEdges.Add(m_oDirectedVertexB, m_oDirectedVertexA, true);
		oEdges.Add(m_oDirectedVertexA, m_oDirectedVertexB, true);


		Assert.IsTrue(m_oDuplicateEdgeDetector.GraphContainsDuplicateEdges);
		Assert.AreEqual(13, m_oDuplicateEdgeDetector.Edges);
		Assert.AreEqual(1, m_oDuplicateEdgeDetector.UniqueEdges);
		Assert.AreEqual(12, m_oDuplicateEdgeDetector.EdgesWithDuplicates);
    }

    //*************************************************************************
    //  Method: TestGraphContainsDuplicateEdges8()
    //
    /// <summary>
    /// Tests the GraphContainsDuplicateEdges() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraphContainsDuplicateEdges8()
    {
		// Undirected graph, no duplicate edges.

		m_oDuplicateEdgeDetector =
			new DuplicateEdgeDetector(m_oUndirectedGraph);

		IEdgeCollection oEdges = m_oUndirectedGraph.Edges;

		oEdges.Add(m_oUndirectedVertexA, m_oUndirectedVertexB, false);
		oEdges.Add(m_oUndirectedVertexA, m_oUndirectedVertexA, false);
		oEdges.Add(m_oUndirectedVertexB, m_oUndirectedVertexB, false);
		oEdges.Add(m_oUndirectedVertexC, m_oUndirectedVertexD, false);
		oEdges.Add(m_oUndirectedVertexC, m_oUndirectedVertexWithNullName,
			false);

		Assert.IsFalse(m_oDuplicateEdgeDetector.GraphContainsDuplicateEdges);
		Assert.AreEqual(4, m_oDuplicateEdgeDetector.Edges);
		Assert.AreEqual(4, m_oDuplicateEdgeDetector.UniqueEdges);
		Assert.AreEqual(0, m_oDuplicateEdgeDetector.EdgesWithDuplicates);
    }

    //*************************************************************************
    //  Method: TestGraphContainsDuplicateEdges9()
    //
    /// <summary>
    /// Tests the GraphContainsDuplicateEdges() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraphContainsDuplicateEdges9()
    {
		// Undirected graph, duplicate A,B.

		m_oDuplicateEdgeDetector =
			new DuplicateEdgeDetector(m_oUndirectedGraph);

		IEdgeCollection oEdges = m_oUndirectedGraph.Edges;

		oEdges.Add(m_oUndirectedVertexA, m_oUndirectedVertexB, false);
		oEdges.Add(m_oUndirectedVertexA, m_oUndirectedVertexA, false);
		oEdges.Add(m_oUndirectedVertexB, m_oUndirectedVertexB, false);
		oEdges.Add(m_oUndirectedVertexC, m_oUndirectedVertexD, false);
		oEdges.Add(m_oUndirectedVertexC, m_oUndirectedVertexWithNullName,
			false);

		oEdges.Add(m_oUndirectedVertexA, m_oUndirectedVertexB, false);

		Assert.IsTrue(m_oDuplicateEdgeDetector.GraphContainsDuplicateEdges);
		Assert.AreEqual(5, m_oDuplicateEdgeDetector.Edges);
		Assert.AreEqual(3, m_oDuplicateEdgeDetector.UniqueEdges);
		Assert.AreEqual(2, m_oDuplicateEdgeDetector.EdgesWithDuplicates);
    }

    //*************************************************************************
    //  Method: TestGraphContainsDuplicateEdges10()
    //
    /// <summary>
    /// Tests the GraphContainsDuplicateEdges() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraphContainsDuplicateEdges10()
    {
		// Undirected graph, duplicate A,B, B,A.

		m_oDuplicateEdgeDetector =
			new DuplicateEdgeDetector(m_oUndirectedGraph);

		IEdgeCollection oEdges = m_oUndirectedGraph.Edges;

		oEdges.Add(m_oUndirectedVertexA, m_oUndirectedVertexB, false);
		oEdges.Add(m_oUndirectedVertexA, m_oUndirectedVertexA, false);
		oEdges.Add(m_oUndirectedVertexB, m_oUndirectedVertexB, false);
		oEdges.Add(m_oUndirectedVertexC, m_oUndirectedVertexD, false);
		oEdges.Add(m_oUndirectedVertexC, m_oUndirectedVertexWithNullName,
			false);

		oEdges.Add(m_oUndirectedVertexB, m_oUndirectedVertexA, false);

		Assert.IsTrue(m_oDuplicateEdgeDetector.GraphContainsDuplicateEdges);
		Assert.AreEqual(5, m_oDuplicateEdgeDetector.Edges);
		Assert.AreEqual(3, m_oDuplicateEdgeDetector.UniqueEdges);
		Assert.AreEqual(2, m_oDuplicateEdgeDetector.EdgesWithDuplicates);
    }

    //*************************************************************************
    //  Method: TestGraphContainsDuplicateEdges11()
    //
    /// <summary>
    /// Tests the GraphContainsDuplicateEdges() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraphContainsDuplicateEdges11()
    {
		// Undirected graph, duplicate C, null name.

		m_oDuplicateEdgeDetector =
			new DuplicateEdgeDetector(m_oUndirectedGraph);

		IEdgeCollection oEdges = m_oUndirectedGraph.Edges;

		oEdges.Add(m_oUndirectedVertexA, m_oUndirectedVertexB, false);
		oEdges.Add(m_oUndirectedVertexA, m_oUndirectedVertexA, false);
		oEdges.Add(m_oUndirectedVertexB, m_oUndirectedVertexB, false);
		oEdges.Add(m_oUndirectedVertexC, m_oUndirectedVertexD, false);
		oEdges.Add(m_oUndirectedVertexC, m_oUndirectedVertexWithNullName,
			false);

		oEdges.Add(m_oUndirectedVertexC, m_oUndirectedVertexWithNullName,
			false);

		Assert.IsFalse(m_oDuplicateEdgeDetector.GraphContainsDuplicateEdges);
		Assert.AreEqual(4, m_oDuplicateEdgeDetector.Edges);
		Assert.AreEqual(4, m_oDuplicateEdgeDetector.UniqueEdges);
		Assert.AreEqual(0, m_oDuplicateEdgeDetector.EdgesWithDuplicates);
    }

    //*************************************************************************
    //  Method: TestGraphContainsDuplicateEdges12()
    //
    /// <summary>
    /// Tests the GraphContainsDuplicateEdges() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraphContainsDuplicateEdges12()
    {
		// Undirected graph, duplicate null name, C.

		m_oDuplicateEdgeDetector =
			new DuplicateEdgeDetector(m_oUndirectedGraph);

		IEdgeCollection oEdges = m_oUndirectedGraph.Edges;

		oEdges.Add(m_oUndirectedVertexA, m_oUndirectedVertexB, false);
		oEdges.Add(m_oUndirectedVertexA, m_oUndirectedVertexA, false);
		oEdges.Add(m_oUndirectedVertexB, m_oUndirectedVertexB, false);
		oEdges.Add(m_oUndirectedVertexC, m_oUndirectedVertexD, false);
		oEdges.Add(m_oUndirectedVertexC, m_oUndirectedVertexWithNullName,
			false);

		oEdges.Add(m_oUndirectedVertexWithNullName, m_oUndirectedVertexC,
			false);

		Assert.IsFalse(m_oDuplicateEdgeDetector.GraphContainsDuplicateEdges);
		Assert.AreEqual(4, m_oDuplicateEdgeDetector.Edges);
		Assert.AreEqual(4, m_oDuplicateEdgeDetector.UniqueEdges);
		Assert.AreEqual(0, m_oDuplicateEdgeDetector.EdgesWithDuplicates);
    }

    //*************************************************************************
    //  Method: TestGraphContainsDuplicateEdges13()
    //
    /// <summary>
    /// Tests the GraphContainsDuplicateEdges() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraphContainsDuplicateEdges13()
    {
		// Undirected graph, many duplicates.

		m_oDuplicateEdgeDetector =
			new DuplicateEdgeDetector(m_oUndirectedGraph);

		IEdgeCollection oEdges = m_oUndirectedGraph.Edges;

		oEdges.Add(m_oUndirectedVertexB, m_oUndirectedVertexA, false);
		oEdges.Add(m_oUndirectedVertexC, m_oUndirectedVertexD, false);
		oEdges.Add(m_oUndirectedVertexA, m_oUndirectedVertexA, false);
		oEdges.Add(m_oUndirectedVertexB, m_oUndirectedVertexB, false);
		oEdges.Add(m_oUndirectedVertexC, m_oUndirectedVertexD, false);
		oEdges.Add(m_oUndirectedVertexA, m_oUndirectedVertexB, false);
		oEdges.Add(m_oUndirectedVertexA, m_oUndirectedVertexB, false);
		oEdges.Add(m_oUndirectedVertexB, m_oUndirectedVertexA, false);
		oEdges.Add(m_oUndirectedVertexC, m_oUndirectedVertexD, false);

		Assert.IsTrue(m_oDuplicateEdgeDetector.GraphContainsDuplicateEdges);
		Assert.AreEqual(9, m_oDuplicateEdgeDetector.Edges);
		Assert.AreEqual(2, m_oDuplicateEdgeDetector.UniqueEdges);
		Assert.AreEqual(7, m_oDuplicateEdgeDetector.EdgesWithDuplicates);
    }

    //*************************************************************************
    //  Method: TestGetVertexNamePair()
    //
    /// <summary>
    /// Tests the GetVertexNamePair() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetVertexNamePair()
    {
		Assert.AreEqual( "A\vB",
			DuplicateEdgeDetector.GetVertexNamePair("A", "B", true) );
    }

    //*************************************************************************
    //  Method: TestGetVertexNamePair2()
    //
    /// <summary>
    /// Tests the GetVertexNamePair() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetVertexNamePair2()
    {
		Assert.AreEqual( "B\vA",
			DuplicateEdgeDetector.GetVertexNamePair("B", "A", true) );
    }

    //*************************************************************************
    //  Method: TestGetVertexNamePair3()
    //
    /// <summary>
    /// Tests the GetVertexNamePair() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetVertexNamePair3()
    {
		Assert.AreEqual( "A\vB",
			DuplicateEdgeDetector.GetVertexNamePair("A", "B", false) );
    }

    //*************************************************************************
    //  Method: TestGetVertexNamePair4()
    //
    /// <summary>
    /// Tests the GetVertexNamePair() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetVertexNamePair4()
    {
		Assert.AreEqual( "A\vB",
			DuplicateEdgeDetector.GetVertexNamePair("B", "A", false) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Object being tested.

	protected DuplicateEdgeDetector m_oDuplicateEdgeDetector;

	/// Directed graph being tested.

	protected IGraph m_oDirectedGraph;

	/// Vertices to add.

	protected IVertex m_oDirectedVertexA;

	///
	protected IVertex m_oDirectedVertexB;

	///
	protected IVertex m_oDirectedVertexC;

	///
	protected IVertex m_oDirectedVertexD;

	///
	protected IVertex m_oDirectedVertexWithNullName;


	/// Undirected graph being tested.

	protected IGraph m_oUndirectedGraph;

	/// Vertices to add.

	protected IVertex m_oUndirectedVertexA;

	///
	protected IVertex m_oUndirectedVertexB;

	///
	protected IVertex m_oUndirectedVertexC;

	///
	protected IVertex m_oUndirectedVertexD;

	///
	protected IVertex m_oUndirectedVertexWithNullName;
}

}
