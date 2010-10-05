
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Algorithms;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: SubgraphCalculatorTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="SubgraphCalculator" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class SubgraphCalculatorTest : Object
{
    //*************************************************************************
    //  Constructor: SubgraphCalculatorTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="SubgraphCalculatorTest" />
    /// class.
    /// </summary>
    //*************************************************************************

    public SubgraphCalculatorTest()
    {
        m_oGraph = null;
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
        m_oGraph = null;
        m_oVertices = null;
        m_oEdges = null;
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
        m_oGraph = null;
        m_oVertices = null;
        m_oEdges = null;
    }

    //*************************************************************************
    //  Method: TestGetSubgraphAsNewGraph()
    //
    /// <summary>
    /// Tests the GetSubgraphAsNewGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetSubgraphAsNewGraph()
    {
        // Include one vertex, no edges.

        CreateGraph(true);

        Dictionary<String, IVertex> oVertexDictionary = AddVertices(
            "A"
            );

        IGraph oNewGraph = SubgraphCalculator.GetSubgraphAsNewGraph(
            new IVertex[] { oVertexDictionary["A"] } );

        CheckVertices(oNewGraph, "A");
        CheckEdges(oNewGraph);
    }

    //*************************************************************************
    //  Method: TestGetSubgraphAsNewGraph2()
    //
    /// <summary>
    /// Tests the GetSubgraphAsNewGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetSubgraphAsNewGraph2()
    {
        // Include one vertex, with edge that is not included.

        CreateGraph(true);

        Dictionary<String, IVertex> oVertexDictionary = AddVertices(
            "A", "B"
            );

        IGraph oNewGraph = SubgraphCalculator.GetSubgraphAsNewGraph(
            new IVertex[] { oVertexDictionary["A"] } );

        CheckVertices(oNewGraph, "A");
        CheckEdges(oNewGraph);
    }

    //*************************************************************************
    //  Method: TestGetSubgraphAsNewGraph3()
    //
    /// <summary>
    /// Tests the GetSubgraphAsNewGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetSubgraphAsNewGraph3()
    {
        // Include one vertex, with self-loop edge that is included.

        CreateGraph(true);

        Dictionary<String, IVertex> oVertexDictionary = AddVertices(
            "A"
            );

        AddEdges(oVertexDictionary, "A", "A");

        IGraph oNewGraph = SubgraphCalculator.GetSubgraphAsNewGraph(
            new IVertex[] { oVertexDictionary["A"] } );

        CheckVertices(oNewGraph, "A");
        CheckEdges(oNewGraph, "A", "A");
    }

    //*************************************************************************
    //  Method: TestGetSubgraphAsNewGraph4()
    //
    /// <summary>
    /// Tests the GetSubgraphAsNewGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetSubgraphAsNewGraph4()
    {
        // Include two vertices, with edge that is included.

        CreateGraph(true);

        Dictionary<String, IVertex> oVertexDictionary = AddVertices(
            "A", "B"
            );

        AddEdges(oVertexDictionary, "A", "B");

        IGraph oNewGraph = SubgraphCalculator.GetSubgraphAsNewGraph(
            new IVertex[] {
                oVertexDictionary["A"],
                oVertexDictionary["B"]
                } );

        CheckVertices(oNewGraph, "A", "B");
        CheckEdges(oNewGraph, "A", "B");
    }

    //*************************************************************************
    //  Method: TestGetSubgraphAsNewGraph5()
    //
    /// <summary>
    /// Tests the GetSubgraphAsNewGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetSubgraphAsNewGraph5()
    {
        // Include two vertices, with edge that is included.

        CreateGraph(true);

        Dictionary<String, IVertex> oVertexDictionary = AddVertices(
            "A", "B"
            );

        AddEdges(oVertexDictionary, "B", "A");

        IGraph oNewGraph = SubgraphCalculator.GetSubgraphAsNewGraph(
            new IVertex[] {
                oVertexDictionary["A"],
                oVertexDictionary["B"]
                } );

        CheckVertices(oNewGraph, "A", "B");
        CheckEdges(oNewGraph, "B", "A");
    }

    //*************************************************************************
    //  Method: TestGetSubgraphAsNewGraph6()
    //
    /// <summary>
    /// Tests the GetSubgraphAsNewGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetSubgraphAsNewGraph6()
    {
        CreateGraph(true);

        // Inclue two vertices, with edges that are included.

        Dictionary<String, IVertex> oVertexDictionary = AddVertices(
            "A", "B"
            );

        AddEdges(oVertexDictionary,
            "A", "B",
            "A", "B",
            "B", "A",
            "B", "A"
            );

        IGraph oNewGraph = SubgraphCalculator.GetSubgraphAsNewGraph(
            new IVertex[] {
                oVertexDictionary["A"],
                oVertexDictionary["B"]
                } );

        CheckVertices(oNewGraph, "A", "B");

        CheckEdges(oNewGraph,
            "A", "B",
            "A", "B",
            "B", "A",
            "B", "A"
            );
    }

    //*************************************************************************
    //  Method: TestGetSubgraphAsNewGraph7()
    //
    /// <summary>
    /// Tests the GetSubgraphAsNewGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetSubgraphAsNewGraph7()
    {
        // Include two vertices, with edges that are included, undirected
        // graph.

        CreateGraph(false);

        Dictionary<String, IVertex> oVertexDictionary = AddVertices(
            "A", "B"
            );

        AddEdges(oVertexDictionary,
            "A", "B",
            "A", "B",
            "B", "A",
            "B", "A"
            );

        IGraph oNewGraph = SubgraphCalculator.GetSubgraphAsNewGraph(
            new IVertex[] {
                oVertexDictionary["A"],
                oVertexDictionary["B"]
                } );

        CheckVertices(oNewGraph, "A", "B");

        CheckEdges(oNewGraph,
            "A", "B",
            "A", "B",
            "B", "A",
            "B", "A"
            );
    }

    //*************************************************************************
    //  Method: TestGetSubgraphAsNewGraph8()
    //
    /// <summary>
    /// Tests the GetSubgraphAsNewGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetSubgraphAsNewGraph8()
    {
        // Complete graph, include one vertex.

        foreach (Boolean bDirected in TestGraphUtil.AllBoolean)
        {
            CreateGraph(bDirected);

            Dictionary<String, IVertex> oVertexDictionary = AddVertices(
                "A", "B", "C", "D"
                );

            AddEdgesForCompleteGraph(oVertexDictionary);

            IGraph oNewGraph = SubgraphCalculator.GetSubgraphAsNewGraph(
                new IVertex[] {
                    oVertexDictionary["A"],
                    } );

            CheckVertices(oNewGraph, "A");

            CheckEdges(oNewGraph,
                "A", "A"
                );
        }
    }

    //*************************************************************************
    //  Method: TestGetSubgraphAsNewGraph9()
    //
    /// <summary>
    /// Tests the GetSubgraphAsNewGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetSubgraphAsNewGraph9()
    {
        // Complete graph, include two vertices.

        foreach (Boolean bDirected in TestGraphUtil.AllBoolean)
        {
            CreateGraph(bDirected);

            Dictionary<String, IVertex> oVertexDictionary = AddVertices(
                "A", "B", "C", "D"
                );

            AddEdgesForCompleteGraph(oVertexDictionary);

            IGraph oNewGraph = SubgraphCalculator.GetSubgraphAsNewGraph(
                new IVertex[] {
                    oVertexDictionary["A"],
                    oVertexDictionary["B"],
                    } );

            CheckVertices(oNewGraph, "A", "B");

            CheckEdges(oNewGraph,
                "A", "A",
                "A", "B",
                "B", "B",
                "B", "A"
                );
        }
    }

    //*************************************************************************
    //  Method: TestGetSubgraphAsNewGraph10()
    //
    /// <summary>
    /// Tests the GetSubgraphAsNewGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetSubgraphAsNewGraph10()
    {
        // Complete graph, include three vertices.

        foreach (Boolean bDirected in TestGraphUtil.AllBoolean)
        {
            CreateGraph(bDirected);

            Dictionary<String, IVertex> oVertexDictionary = AddVertices(
                "A", "B", "C", "D"
                );

            AddEdgesForCompleteGraph(oVertexDictionary);

            IGraph oNewGraph = SubgraphCalculator.GetSubgraphAsNewGraph(
                new IVertex[] {
                    oVertexDictionary["A"],
                    oVertexDictionary["B"],
                    oVertexDictionary["C"],
                    } );

            CheckVertices(oNewGraph, "A", "B", "C");

            CheckEdges(oNewGraph,
                "A", "A",
                "A", "B",
                "A", "C",

                "B", "A",
                "B", "B",
                "B", "C",

                "C", "A",
                "C", "B",
                "C", "C"
                );
        }
    }

    //*************************************************************************
    //  Method: TestGetSubgraphAsNewGraph11()
    //
    /// <summary>
    /// Tests the GetSubgraphAsNewGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetSubgraphAsNewGraph11()
    {
        // Complete graph, include four vertices.

        foreach (Boolean bDirected in TestGraphUtil.AllBoolean)
        {
            CreateGraph(bDirected);

            Dictionary<String, IVertex> oVertexDictionary = AddVertices(
                "A", "B", "C", "D"
                );

            AddEdgesForCompleteGraph(oVertexDictionary);

            IGraph oNewGraph = SubgraphCalculator.GetSubgraphAsNewGraph(
                new IVertex[] {
                    oVertexDictionary["A"],
                    oVertexDictionary["B"],
                    oVertexDictionary["C"],
                    oVertexDictionary["D"],
                    } );

            CheckVertices(oNewGraph, "A", "B", "C", "D");

            CheckEdges(oNewGraph,
                "A", "A",
                "A", "B",
                "A", "C",
                "A", "D",

                "B", "A",
                "B", "B",
                "B", "C",
                "B", "D",

                "C", "A",
                "C", "B",
                "C", "C",
                "C", "D",

                "D", "A",
                "D", "B",
                "D", "C",
                "D", "D"
                );
        }
    }

    //*************************************************************************
    //  Method: CreateGraph()
    //
    /// <summary>
    /// Creates the original graph.
    /// </summary>
    ///
    /// <param name="bDirected">
    /// true for directed, false for undirected.
    /// </param>
    //*************************************************************************

    protected void
    CreateGraph
    (
        Boolean bDirected
    )
    {
        m_oGraph = new Graph(bDirected ? GraphDirectedness.Directed :
            GraphDirectedness.Undirected);

        m_oVertices = m_oGraph.Vertices;
        m_oEdges = m_oGraph.Edges;
    }

    //*************************************************************************
    //  Method: AddVertices()
    //
    /// <summary>
    /// Adds vertices to the original graph.
    /// </summary>
    ///
    /// <param name="asVertexNames">
    /// Zero or more vertex names.
    /// </param>
    ///
    /// <returns>
    /// A dictionary.  The key is the vertex name and the value is the
    /// corresponding added vertex.
    /// </returns>
    //*************************************************************************

    protected Dictionary<String, IVertex>
    AddVertices
    (
        params String [] asVertexNames
    )
    {
        Dictionary<String, IVertex> oVertexDictionary =
            new Dictionary<String, IVertex>(asVertexNames.Length);

        foreach (String sVertexName in asVertexNames)
        {
            IVertex oVertex = new Vertex();
            oVertex.Name = sVertexName;
            m_oVertices.Add(oVertex);

            oVertexDictionary.Add(sVertexName, oVertex);
        }

        return (oVertexDictionary);
    }

    //*************************************************************************
    //  Method: AddEdges()
    //
    /// <summary>
    /// Adds edges to the original graph.
    /// </summary>
    ///
    /// <param name="oVertexDictionary">
    /// The key is the vertex name and the value is the corresponding vertex in
    /// the original graph.
    /// </param>
    ///
    /// <param name="asVertexNamePairs">
    /// Pairs of vertex names to connect with edges.
    /// </param>
    //*************************************************************************

    protected void
    AddEdges
    (
        Dictionary<String, IVertex> oVertexDictionary,
        params String [] asVertexNamePairs
    )
    {
        Debug.Assert(asVertexNamePairs.Length % 2 == 0);

        for (Int32 i = 0; i < asVertexNamePairs.Length; i +=2 )
        {
            String sVertexName1 = asVertexNamePairs[i + 0];
            String sVertexName2 = asVertexNamePairs[i + 1];

            m_oEdges.Add(oVertexDictionary[sVertexName1],
                oVertexDictionary[sVertexName2],
                m_oGraph.Directedness == GraphDirectedness.Directed);
        }
    }

    //*************************************************************************
    //  Method: AddEdgesForCompleteGraph()
    //
    /// <summary>
    /// Tests the GetSubgraphAsNewGraph() method.
    /// </summary>
    ///
    /// <param name="oVertexDictionary">
    /// The key is the vertex name and the value is the corresponding vertex in
    /// the original graph.
    /// </param"
    //*************************************************************************

    protected void
    AddEdgesForCompleteGraph
    (
        Dictionary<String, IVertex> oVertexDictionary
    )
    {
        AddEdges(oVertexDictionary,
            "A", "A",
            "A", "B",
            "A", "C",
            "A", "D",

            "B", "A",
            "B", "B",
            "B", "C",
            "B", "D",

            "C", "A",
            "C", "B",
            "C", "C",
            "C", "D",

            "D", "A",
            "D", "B",
            "D", "C",
            "D", "D"
            );
    }

    //*************************************************************************
    //  Method: CheckVertices()
    //
    /// <summary>
    /// Checks whether the new graph contains the expected vertices.
    /// </summary>
    ///
    /// <param name="oNewGraph">
    /// Graph created by SubgraphCalculator.GetSubgraphAsNewGraph().
    /// </param>
    ///
    /// <param name="asExpectedVertexNames">
    /// Zero or more expected vertex names.
    /// </param>
    //*************************************************************************

    protected void
    CheckVertices
    (
        IGraph oNewGraph,
        params String [] asExpectedVertexNames
    )
    {
        Assert.IsNotNull(oNewGraph);

        HashSet<String> oNewVertexNames = new HashSet<String>(
            from oNewVertex in oNewGraph.Vertices
            select oNewVertex.Name
            );

        Assert.AreEqual(asExpectedVertexNames.Length,
            oNewGraph.Vertices.Count);

        foreach (String sExpectedVertexNames in asExpectedVertexNames)
        {
            Assert.IsTrue( oNewVertexNames.Contains(sExpectedVertexNames) );
        }
    }

    //*************************************************************************
    //  Method: CheckEdges()
    //
    /// <summary>
    /// Checks whether the new graph contains the expected edges.
    /// </summary>
    ///
    /// <param name="oNewGraph">
    /// Graph created by SubgraphCalculator.GetSubgraphAsNewGraph().
    /// </param>
    ///
    /// <param name="asExpectedVertexNamePairs">
    /// Zero or more expected vertex name pairs.
    /// </param>
    //*************************************************************************

    protected void
    CheckEdges
    (
        IGraph oNewGraph,
        params String [] asExpectedVertexNamePairs
    )
    {
        Assert.IsNotNull(oNewGraph);
        Debug.Assert(asExpectedVertexNamePairs.Length % 2 == 0);

        HashSet<String> oNewVertexNamePairs = new HashSet<String>(
            from oNewEdge in oNewGraph.Edges
            select oNewEdge.Vertices[0].Name + "\t" + oNewEdge.Vertices[1].Name
            );

        Assert.AreEqual(asExpectedVertexNamePairs.Length / 2,
            oNewGraph.Edges.Count);

        for (Int32 i = 0; i < asExpectedVertexNamePairs.Length; i +=2 )
        {
            String sExpectedVertexName1 = asExpectedVertexNamePairs[i + 0];
            String sExpectedVertexName2 = asExpectedVertexNamePairs[i + 1];

            Assert.IsTrue(oNewVertexNamePairs.Contains(
                sExpectedVertexName1 + "\t" + sExpectedVertexName2
                ) );
        }
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Original graph.

    protected IGraph m_oGraph;

    /// Vertices in the original graph.

    protected IVertexCollection m_oVertices;

    /// Edges in the original graph.

    protected IEdgeCollection m_oEdges;
}

}
