
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Adapters;
using Microsoft.NodeXL.Common;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: PajekGraphAdapterTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="PajekGraphAdapter" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class PajekGraphAdapterTest : Object
{
    //*************************************************************************
    //  Constructor: PajekGraphAdapterTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="PajekGraphAdapterTest" />
    /// class.
    /// </summary>
    //*************************************************************************

    public PajekGraphAdapterTest()
    {
        m_oGraphAdapter = null;
        m_sTempFileName = null;
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
        m_oGraphAdapter = new PajekGraphAdapter();

        m_sTempFileName = Path.GetTempFileName();
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
        m_oGraphAdapter = null;

        if ( File.Exists(m_sTempFileName) )
        {
            File.Delete(m_sTempFileName);
        }
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
    //  Method: TestLoadGraphFromFile()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFile()
    {
        // Overall test.

        const Int32 Vertices = 10;

        const String FileContents =

            "/* This is a comment. */\r\n"
            + "\r\n"
            + "*vertices 10\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3 ignored parameters\r\n"
            + "2 \"Vertex 2\" 0.2 0.2 0.3\r\n"
            + "3 \"Vertex 3\" 0.3 0.2 0.3\r\n"
            + "4 \"Vertex 4\" 0.4 0.2 0.3\r\n"
            + "   /* This is a comment. */\r\n"
            + "5 \"Vertex 5\" 0.5 0.2 0.3\r\n"
            + "6 \"Vertex 6\" 0.6 0.2 0.3\r\n"
            + "7 \"Vertex 7\" 0.7 0.2 0.3\r\n"
            + "8 \"Vertex 8\" 0.8 0.2 0.3\r\n"
            + "9 \"Vertex 9\" 0.9 0.2 0.3\r\n"
            + "10 \"Vertex 10\" 0.10 0.2 0.3\r\n"
            + "\r\n"
            + "*edges\r\n"
            + "1 2 3.1 ignored parameters\r\n"
            + "   /* This is a comment. */\r\n"
            + "3 4 4.2\r\n"
            + "9 8 1.2\r\n"
            + "\r\n"
            + "*edgeslist\r\n"
            + "1 4 5\r\n"
            + "   /* This is a comment. */\r\n"
            + "6 8 3 2\r\n"
            + "\r\n"
            + "*arcs\r\n"
            + "10 9 123.34 ignored parameters\r\n"
            + "9 1 98.7\r\n"
            + "\r\n"
            + "*arcslist\r\n"
            + "   /* This is a comment. */\r\n"
            + "4 2 5\r\n"
            + "8 7 1 2\r\n"
            + "\r\n"
            + "/* This is a comment. */\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Mixed, oGraph.Directedness);

        IVertexCollection oVertices = oGraph.Vertices;

        Assert.AreEqual(Vertices, oVertices.Count);

        for (Int32 i = 0; i < Vertices; i++)
        {
            Assert.IsTrue( oVertices.Contains(GetVertexName(i + 1) ) );
        }

        IVertex oVertex;
        
        oVertices.Find("Vertex 9", out oVertex);

        Assert.IsNotNull(oVertex);
        Assert.AreEqual(0.9F, oVertex.Location.X);
        Assert.AreEqual(0.2F, oVertex.Location.Y);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(15, oEdges.Count);

        // *edges

        FindEdge(oGraph, 1, 2, false, 3.1);
        FindEdge(oGraph, 3, 4, false, 4.2);
        FindEdge(oGraph, 9, 8, false, 1.2);

        // *edgeslist

        FindEdge(oGraph, 1, 4, false, 1);
        FindEdge(oGraph, 1, 5, false, 1);

        FindEdge(oGraph, 6, 8, false, 1);
        FindEdge(oGraph, 6, 3, false, 1);
        FindEdge(oGraph, 6, 2, false, 1);

        // *arcs

        FindEdge(oGraph, 10, 9, true, 123.34);
        FindEdge(oGraph, 9, 1, true, 98.7);

        // *arcslist

        FindEdge(oGraph, 4, 2, true, 1);
        FindEdge(oGraph, 4, 5, true, 1);

        FindEdge(oGraph, 8, 7, true, 1);
        FindEdge(oGraph, 8, 1, true, 1);
        FindEdge(oGraph, 8, 2, true, 1);

        // Verify that every edge was searched for and found.

        foreach (IEdge oEdge in oGraph.Edges)
        {
            Assert.IsTrue( oEdge.ContainsKey(EdgeFoundKey) );
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFile2()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFile2()
    {
        // Empty graph.

        const Int32 Vertices = 0;

        WriteFile(String.Empty);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Undirected, oGraph.Directedness);

        IVertexCollection oVertices = oGraph.Vertices;

        Assert.AreEqual(Vertices, oVertices.Count);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(0, oEdges.Count);
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFile3()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFile3()
    {
        // No edges.

        const Int32 Vertices = 1;

        const String FileContents =

            "*vertices 1\r\n"
            + "1 \"Vertex 1\" 0.2 0.4 0\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Undirected, oGraph.Directedness);

        IVertexCollection oVertices = oGraph.Vertices;

        Assert.AreEqual(Vertices, oVertices.Count);

        IVertex oVertex;
        
        oVertices.Find("Vertex 1", out oVertex);

        Assert.IsNotNull(oVertex);
        Assert.AreEqual(0.2F, oVertex.Location.X);
        Assert.AreEqual(0.4F, oVertex.Location.Y);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(0, oEdges.Count);
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFile4()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFile4()
    {
        // No vertex coordinates.

        const Int32 Vertices = 1;

        const String FileContents =

            "*vertices 1\r\n"
            + "1 \"Vertex 1\"\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Undirected, oGraph.Directedness);

        IVertexCollection oVertices = oGraph.Vertices;

        Assert.AreEqual(Vertices, oVertices.Count);

        IVertex oVertex;
        
        oVertices.Find("Vertex 1", out oVertex);

        Assert.IsNotNull(oVertex);
        Assert.AreEqual(0F, oVertex.Location.X);
        Assert.AreEqual(0F, oVertex.Location.Y);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(0, oEdges.Count);
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFile5()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFile5()
    {
        // Empty edge sections.

        const Int32 Vertices = 1;

        const String FileContents =

            "*vertices 1\r\n"
            + "1 \"Vertex 1\" 0.2 0.4 0\r\n"
            + "*edges\r\n"
            + "*edgeslist\r\n"
            + "*arcs\r\n"
            + "*arcslist\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Undirected, oGraph.Directedness);

        IVertexCollection oVertices = oGraph.Vertices;

        Assert.AreEqual(Vertices, oVertices.Count);

        IVertex oVertex;
        
        oVertices.Find("Vertex 1", out oVertex);

        Assert.IsNotNull(oVertex);
        Assert.AreEqual(0.2F, oVertex.Location.X);
        Assert.AreEqual(0.4F, oVertex.Location.Y);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(0, oEdges.Count);
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFile6()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFile6()
    {
        // Duplicate edges.

        const Int32 Vertices = 10;

        const String FileContents =

            "*vertices 10\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3\r\n"
            + "2 \"Vertex 2\" 0.2 0.2 0.3\r\n"
            + "3 \"Vertex 3\" 0.3 0.2 0.3\r\n"
            + "4 \"Vertex 4\" 0.4 0.2 0.3\r\n"
            + "5 \"Vertex 5\" 0.5 0.2 0.3\r\n"
            + "6 \"Vertex 6\" 0.6 0.2 0.3\r\n"
            + "7 \"Vertex 7\" 0.7 0.2 0.3\r\n"
            + "8 \"Vertex 8\" 0.8 0.2 0.3\r\n"
            + "9 \"Vertex 9\" 0.9 0.2 0.3\r\n"
            + "10 \"Vertex 10\" 0.10 0.2 0.3\r\n"
            + "\r\n"
            + "*edges\r\n"
            + "1 2 3.1\r\n"
            + "1 2 3.1\r\n"
            + "\r\n"
            + "*edgeslist\r\n"
            + "1 4 5\r\n"
            + "1 4 5\r\n"
            + "\r\n"
            + "*arcs\r\n"
            + "10 9 123.34\r\n"
            + "10 9 123.34\r\n"
            + "\r\n"
            + "*arcslist\r\n"
            + "4 2 5\r\n"
            + "4 2 5\r\n"
            + "\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Mixed, oGraph.Directedness);

        IVertexCollection oVertices = oGraph.Vertices;

        Assert.AreEqual(Vertices, oVertices.Count);

        for (Int32 i = 0; i < Vertices; i++)
        {
            Assert.IsTrue( oVertices.Contains(GetVertexName(i + 1) ) );
        }

        IVertex oVertex;
        
        oVertices.Find("Vertex 9", out oVertex);

        Assert.IsNotNull(oVertex);
        Assert.AreEqual(0.9F, oVertex.Location.X);
        Assert.AreEqual(0.2F, oVertex.Location.Y);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(12, oEdges.Count);

        // *edges

        FindEdge(oGraph, 1, 2, false, 3.1);
        FindEdge(oGraph, 1, 2, false, 3.1);

        // *edgeslist

        FindEdge(oGraph, 1, 4, false, 1);
        FindEdge(oGraph, 1, 4, false, 1);
        FindEdge(oGraph, 1, 5, false, 1);
        FindEdge(oGraph, 1, 5, false, 1);

        // *arcs

        FindEdge(oGraph, 10, 9, true, 123.34);
        FindEdge(oGraph, 10, 9, true, 123.34);

        // *arcslist

        FindEdge(oGraph, 4, 2, true, 1);
        FindEdge(oGraph, 4, 2, true, 1);
        FindEdge(oGraph, 4, 5, true, 1);
        FindEdge(oGraph, 4, 5, true, 1);

        // Verify that every edge was searched for and found.

        foreach (IEdge oEdge in oGraph.Edges)
        {
            Assert.IsTrue( oEdge.ContainsKey(EdgeFoundKey) );
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFile7()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFile7()
    {
        // Reversed section order.

        const Int32 Vertices = 10;

        const String FileContents =

            "/* This is a comment. */\r\n"
            + "\r\n"
            + "*vertices 10\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3\r\n"
            + "2 \"Vertex 2\" 0.2 0.2 0.3\r\n"
            + "3 \"Vertex 3\" 0.3 0.2 0.3\r\n"
            + "4 \"Vertex 4\" 0.4 0.2 0.3\r\n"
            + "   /* This is a comment. */\r\n"
            + "5 \"Vertex 5\" 0.5 0.2 0.3\r\n"
            + "6 \"Vertex 6\" 0.6 0.2 0.3\r\n"
            + "7 \"Vertex 7\" 0.7 0.2 0.3\r\n"
            + "8 \"Vertex 8\" 0.8 0.2 0.3\r\n"
            + "9 \"Vertex 9\" 0.9 0.2 0.3\r\n"
            + "10 \"Vertex 10\" 0.10 0.2 0.3\r\n"
            + "\r\n"
            + "*arcslist\r\n"
            + "   /* This is a comment. */\r\n"
            + "4 2 5\r\n"
            + "8 7 1 2\r\n"
            + "\r\n"
            + "/* This is a comment. */\r\n"
            + "*arcs\r\n"
            + "10 9 123.34\r\n"
            + "9 1 98.7\r\n"
            + "\r\n"
            + "*edgeslist\r\n"
            + "1 4 5\r\n"
            + "   /* This is a comment. */\r\n"
            + "6 8 3 2\r\n"
            + "\r\n"
            + "*edges\r\n"
            + "1 2 3.1\r\n"
            + "   /* This is a comment. */\r\n"
            + "3 4 4.2\r\n"
            + "9 8 1.2\r\n"
            + "\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Mixed, oGraph.Directedness);

        IVertexCollection oVertices = oGraph.Vertices;

        Assert.AreEqual(Vertices, oVertices.Count);

        for (Int32 i = 0; i < Vertices; i++)
        {
            Assert.IsTrue( oVertices.Contains(GetVertexName(i + 1) ) );
        }

        IVertex oVertex;
        
        oVertices.Find("Vertex 9", out oVertex);

        Assert.IsNotNull(oVertex);
        Assert.AreEqual(0.9F, oVertex.Location.X);
        Assert.AreEqual(0.2F, oVertex.Location.Y);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(15, oEdges.Count);

        // *edges

        FindEdge(oGraph, 1, 2, false, 3.1);
        FindEdge(oGraph, 3, 4, false, 4.2);
        FindEdge(oGraph, 9, 8, false, 1.2);

        // *edgeslist

        FindEdge(oGraph, 1, 4, false, 1);
        FindEdge(oGraph, 1, 5, false, 1);

        FindEdge(oGraph, 6, 8, false, 1);
        FindEdge(oGraph, 6, 3, false, 1);
        FindEdge(oGraph, 6, 2, false, 1);

        // *arcs

        FindEdge(oGraph, 10, 9, true, 123.34);
        FindEdge(oGraph, 9, 1, true, 98.7);

        // *arcslist

        FindEdge(oGraph, 4, 2, true, 1);
        FindEdge(oGraph, 4, 5, true, 1);

        FindEdge(oGraph, 8, 7, true, 1);
        FindEdge(oGraph, 8, 1, true, 1);
        FindEdge(oGraph, 8, 2, true, 1);

        // Verify that every edge was searched for and found.

        foreach (IEdge oEdge in oGraph.Edges)
        {
            Assert.IsTrue( oEdge.ContainsKey(EdgeFoundKey) );
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFile8()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFile8()
    {
        // Unrecognized sections.

        const Int32 Vertices = 10;

        const String FileContents =

            "/* This is a comment. */\r\n"
            + "\r\n"
            + "*vertices 10\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3\r\n"
            + "2 \"Vertex 2\" 0.2 0.2 0.3\r\n"
            + "3 \"Vertex 3\" 0.3 0.2 0.3\r\n"
            + "4 \"Vertex 4\" 0.4 0.2 0.3\r\n"
            + "   /* This is a comment. */\r\n"
            + "5 \"Vertex 5\" 0.5 0.2 0.3\r\n"
            + "6 \"Vertex 6\" 0.6 0.2 0.3\r\n"
            + "7 \"Vertex 7\" 0.7 0.2 0.3\r\n"
            + "8 \"Vertex 8\" 0.8 0.2 0.3\r\n"
            + "9 \"Vertex 9\" 0.9 0.2 0.3\r\n"
            + "10 \"Vertex 10\" 0.10 0.2 0.3\r\n"
            + "\r\n"
            + "*unrecognized\r\n"
            + "junk\r\n"
            + "junk\r\n"
            + "*edges\r\n"
            + "1 2 3.1\r\n"
            + "   /* This is a comment. */\r\n"
            + "3 4 4.2\r\n"
            + "9 8 1.2\r\n"
            + "\r\n"
            + "*unrecognized\r\n"
            + "junk\r\n"
            + "junk\r\n"
            + "*edgeslist\r\n"
            + "1 4 5\r\n"
            + "   /* This is a comment. */\r\n"
            + "6 8 3 2\r\n"
            + "\r\n"
            + "*unrecognized\r\n"
            + "junk\r\n"
            + "junk\r\n"
            + "*arcs\r\n"
            + "10 9 123.34\r\n"
            + "9 1 98.7\r\n"
            + "\r\n"
            + "*unrecognized\r\n"
            + "junk\r\n"
            + "junk\r\n"
            + "*arcslist\r\n"
            + "   /* This is a comment. */\r\n"
            + "4 2 5\r\n"
            + "8 7 1 2\r\n"
            + "\r\n"
            + "*unrecognized\r\n"
            + "junk\r\n"
            + "junk\r\n"
            + "/* This is a comment. */\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Mixed, oGraph.Directedness);

        IVertexCollection oVertices = oGraph.Vertices;

        Assert.AreEqual(Vertices, oVertices.Count);

        for (Int32 i = 0; i < Vertices; i++)
        {
            Assert.IsTrue( oVertices.Contains(GetVertexName(i + 1) ) );
        }

        IVertex oVertex;
        
        oVertices.Find("Vertex 9", out oVertex);

        Assert.IsNotNull(oVertex);
        Assert.AreEqual(0.9F, oVertex.Location.X);
        Assert.AreEqual(0.2F, oVertex.Location.Y);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(15, oEdges.Count);

        // *edges

        FindEdge(oGraph, 1, 2, false, 3.1);
        FindEdge(oGraph, 3, 4, false, 4.2);
        FindEdge(oGraph, 9, 8, false, 1.2);

        // *edgeslist

        FindEdge(oGraph, 1, 4, false, 1);
        FindEdge(oGraph, 1, 5, false, 1);

        FindEdge(oGraph, 6, 8, false, 1);
        FindEdge(oGraph, 6, 3, false, 1);
        FindEdge(oGraph, 6, 2, false, 1);

        // *arcs

        FindEdge(oGraph, 10, 9, true, 123.34);
        FindEdge(oGraph, 9, 1, true, 98.7);

        // *arcslist

        FindEdge(oGraph, 4, 2, true, 1);
        FindEdge(oGraph, 4, 5, true, 1);

        FindEdge(oGraph, 8, 7, true, 1);
        FindEdge(oGraph, 8, 1, true, 1);
        FindEdge(oGraph, 8, 2, true, 1);

        // Verify that every edge was searched for and found.

        foreach (IEdge oEdge in oGraph.Edges)
        {
            Assert.IsTrue( oEdge.ContainsKey(EdgeFoundKey) );
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFile9()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFile9()
    {
        // No quotes around vertex names.

        const Int32 Vertices = 10;

        const String FileContents =

            "*vertices 10\r\n"
            + "1 \"Vertex1\" 0.1 0.2 0.3\r\n"
            + "2 \"Vertex2\" 0.2 0.2 0.3\r\n"
            + "3 \"Vertex3\" 0.3 0.2 0.3\r\n"
            + "4 \"Vertex4\" 0.4 0.2 0.3\r\n"
            + "5 \"Vertex5\" 0.5 0.2 0.3\r\n"
            + "6 \"Vertex6\" 0.6 0.2 0.3\r\n"
            + "7 \"Vertex7\" 0.7 0.2 0.3\r\n"
            + "8 \"Vertex8\" 0.8 0.2 0.3\r\n"
            + "9 \"Vertex9\" 0.9 0.2 0.3\r\n"
            + "10 \"Vertex10\" 0.10 0.2 0.3\r\n"
            + "\r\n"
            + "*edges\r\n"
            + "1 2 3.1\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Undirected, oGraph.Directedness);

        IVertexCollection oVertices = oGraph.Vertices;

        Assert.AreEqual(Vertices, oVertices.Count);

        for (Int32 i = 0; i < Vertices; i++)
        {
            Assert.IsTrue( oVertices.Contains("Vertex" + (i + 1).ToString() ) );
        }

        IVertex oVertex;
        
        oVertices.Find("Vertex9", out oVertex);

        Assert.IsNotNull(oVertex);
        Assert.AreEqual(0.9F, oVertex.Location.X);
        Assert.AreEqual(0.2F, oVertex.Location.Y);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(1, oEdges.Count);
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFile10()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFile10()
    {
        // Multiple sections of same type.

        const Int32 Vertices = 10;

        const String FileContents =

            "*vertices 10\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3\r\n"
            + "2 \"Vertex 2\" 0.2 0.2 0.3\r\n"
            + "3 \"Vertex 3\" 0.3 0.2 0.3\r\n"
            + "4 \"Vertex 4\" 0.4 0.2 0.3\r\n"
            + "5 \"Vertex 5\" 0.5 0.2 0.3\r\n"
            + "6 \"Vertex 6\" 0.6 0.2 0.3\r\n"
            + "7 \"Vertex 7\" 0.7 0.2 0.3\r\n"
            + "8 \"Vertex 8\" 0.8 0.2 0.3\r\n"
            + "9 \"Vertex 9\" 0.9 0.2 0.3\r\n"
            + "10 \"Vertex 10\" 0.10 0.2 0.3\r\n"
            + "\r\n"
            + "*edges\r\n"
            + "1 2 3.1\r\n"
            + "3 4 4.2\r\n"
            + "\r\n"
            + "*edgeslist\r\n"
            + "1 4 5\r\n"
            + "\r\n"
            + "*arcs\r\n"
            + "10 9 123.34\r\n"
            + "\r\n"
            + "*arcslist\r\n"
            + "4 2 5\r\n"
            + "*edges\r\n"
            + "9 8 1.2\r\n"
            + "\r\n"
            + "*edgeslist\r\n"
            + "6 8 3 2\r\n"
            + "\r\n"
            + "*arcs\r\n"
            + "9 1 98.7\r\n"
            + "\r\n"
            + "*arcslist\r\n"
            + "8 7 1 2\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Mixed, oGraph.Directedness);

        IVertexCollection oVertices = oGraph.Vertices;

        Assert.AreEqual(Vertices, oVertices.Count);

        for (Int32 i = 0; i < Vertices; i++)
        {
            Assert.IsTrue( oVertices.Contains(GetVertexName(i + 1) ) );
        }

        IVertex oVertex;
        
        oVertices.Find("Vertex 9", out oVertex);

        Assert.IsNotNull(oVertex);
        Assert.AreEqual(0.9F, oVertex.Location.X);
        Assert.AreEqual(0.2F, oVertex.Location.Y);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(15, oEdges.Count);

        // *edges

        FindEdge(oGraph, 1, 2, false, 3.1);
        FindEdge(oGraph, 3, 4, false, 4.2);
        FindEdge(oGraph, 9, 8, false, 1.2);

        // *edgeslist

        FindEdge(oGraph, 1, 4, false, 1);
        FindEdge(oGraph, 1, 5, false, 1);

        FindEdge(oGraph, 6, 8, false, 1);
        FindEdge(oGraph, 6, 3, false, 1);
        FindEdge(oGraph, 6, 2, false, 1);

        // *arcs

        FindEdge(oGraph, 10, 9, true, 123.34);
        FindEdge(oGraph, 9, 1, true, 98.7);

        // *arcslist

        FindEdge(oGraph, 4, 2, true, 1);
        FindEdge(oGraph, 4, 5, true, 1);

        FindEdge(oGraph, 8, 7, true, 1);
        FindEdge(oGraph, 8, 1, true, 1);
        FindEdge(oGraph, 8, 2, true, 1);

        // Verify that every edge was searched for and found.

        foreach (IEdge oEdge in oGraph.Edges)
        {
            Assert.IsTrue( oEdge.ContainsKey(EdgeFoundKey) );
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFile11()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFile11()
    {
        // Vertex name doesn't have closing quote.

        const Int32 Vertices = 1;

        const String FileContents =

            "*vertices 1\r\n"
            + "1 \"Vertex1 0.1 0.2 0.3\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Undirected, oGraph.Directedness);

        IVertexCollection oVertices = oGraph.Vertices;

        Assert.AreEqual(Vertices, oVertices.Count);

        IVertex oVertex;
        
        oVertices.Find("\"Vertex1", out oVertex);

        Assert.IsNotNull(oVertex);
        Assert.AreEqual(0.1F, oVertex.Location.X);
        Assert.AreEqual(0.2F, oVertex.Location.Y);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(0, oEdges.Count);
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFile12()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFile12()
    {
        // Mixed tabs and spaces.

        const Int32 Vertices = 10;

        const String FileContents =

            "*vertices   10\r\n"
            +    "1      \"Vertex1\"     0.1     0.2     0.3\r\n"
            +    "2      \"Vertex2\"     0.2     0.2     0.3\r\n"
            +    "3      \"Vertex3\"     0.3     0.2     0.3\r\n"
            +    "4      \"Vertex4\"     0.4     0.2     0.3\r\n"
            +    "5      \"Vertex5\"     0.5     0.2     0.3\r\n"
            +    "6      \"Vertex6\"     0.6     0.2     0.3\r\n"
            +    "7      \"Vertex7\"     0.7     0.2     0.3\r\n"
            +    "8      \"Vertex8\"     0.8     0.2     0.3\r\n"
            +    "9      \"Vertex9\"     0.9     0.2     0.3\r\n"
            +    "10     \"Vertex10\"    0.10    0.2     0.3\r\n"
            +    "\r\n"
            +    "*edges\r\n"
            +    "1      2   3.1\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Undirected, oGraph.Directedness);

        IVertexCollection oVertices = oGraph.Vertices;

        Assert.AreEqual(Vertices, oVertices.Count);

        for (Int32 i = 0; i < Vertices; i++)
        {
            Assert.IsTrue( oVertices.Contains("Vertex" + (i + 1).ToString() ) );
        }

        IVertex oVertex;
        
        oVertices.Find("Vertex9", out oVertex);

        Assert.IsNotNull(oVertex);
        Assert.AreEqual(0.9F, oVertex.Location.X);
        Assert.AreEqual(0.2F, oVertex.Location.Y);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(1, oEdges.Count);
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFile13()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFile13()
    {
        // Sample file copied from 
        // http://www.stanford.edu/group/sonia/documentation/inputFormats.html.

        const Int32 Vertices = 3;

        const String FileContents =

            "*Vertices 3\r\n"
            + "1 \"a\" c blue [5-10,12-14]\r\n"
            + "2 \"b\" c red [1-3,7]\r\n"
            + "3 \"e\" c green [4-*]\r\n"
            + "*Edges\r\n"
            + "1 2 1 c gray [7]\r\n"
            + "1 3 1 [6-8]\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Undirected, oGraph.Directedness);

        IVertexCollection oVertices = oGraph.Vertices;

        Assert.AreEqual(Vertices, oVertices.Count);

        Assert.IsTrue( oVertices.Contains("a") );
        Assert.IsTrue( oVertices.Contains("b") );
        Assert.IsTrue( oVertices.Contains("e") );

        IVertex oVertex;
        
        oVertices.Find("a", out oVertex);

        Assert.IsNotNull(oVertex);
        Assert.AreEqual(0F, oVertex.Location.X);
        Assert.AreEqual(0F, oVertex.Location.Y);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(2, oEdges.Count);

        // *edges

        FindEdge(oGraph, "a", "b", false, 1);
        FindEdge(oGraph, "a", "e", false, 1);

        // Verify that every edge was searched for and found.

        foreach (IEdge oEdge in oGraph.Edges)
        {
            Assert.IsTrue( oEdge.ContainsKey(EdgeFoundKey) );
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFile14()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFile14()
    {
        const Int32 Vertices = 14;

        /* SSS_WARNINGS_OFF */

        // Sample file copied from 
        // http://www.ccsr.ac.uk/methods/publications/snacourse/netdata.html.
        // Edge weights were added.  The SSS_WARNINGS_OFF comment is to prevent
        // Microsoft's sss.exe "sensitive terms" tool from flagging Dickson as
        // obscene.

        const String FileContents =

            "/* Cut from this line to the last line                 */\r\n"
            + "/* Save this in your local directory, say C:\temp      */\r\n"
            + "/* give it a name:                                     */\r\n"
            + "/* hawthorne-friend.net                                */\r\n"
            + "/* Of course you can just download this data from the  */\r\n"
            + "/*      link above by right clicking and saving it     */\r\n"
            + "/*                                                     */\r\n"
            + "/* Source: Roethlisberger and Dickson 1939 :501ff      */\r\n"
            + "/*                                                     */\r\n"
            + "/* An example of non directed or simple social network */\r\n"
            + "/* It has two parts (each marked by asterisk) which    */\r\n"
            + "/*   closely followed Graph theory definition of graph */\r\n"
            + "\r\n"
            + "*Vertices 14\r\n"
            + "1 I1\r\n"
            + "2 I3\r\n"
            + "3 W1\r\n"
            + "4 W2\r\n"
            + "5 W3\r\n"
            + "6 W4\r\n"
            + "7 W5\r\n"
            + "8 W6\r\n"
            + "9 W7\r\n"
            + "10 W8\r\n"
            + "11 W9\r\n"
            + "12 S1\r\n"
            + "13 S2\r\n"
            + "14 S4\r\n"
            + "*Edges\r\n"
            + "1 5 1\r\n"
            + "3 5 1\r\n"
            + "3 6 1\r\n"
            + "5 6 1\r\n"
            + "9 10 1\r\n"
            + "9 11 1\r\n"
            + "10 11 1\r\n"
            + "3 12 1\r\n"
            + "5 12 1\r\n"
            + "6 12 1\r\n"
            + "10 14 1\r\n"
            + "11 14 1\r\n"
            + "9 12 1\r\n"
            ;

        /* SSS_WARNINGS_ON */

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Undirected, oGraph.Directedness);

        IVertexCollection oVertices = oGraph.Vertices;

        Assert.AreEqual(Vertices, oVertices.Count);

        IVertex oVertex;
        
        oVertices.Find("I1", out oVertex);

        Assert.IsNotNull(oVertex);
        Assert.AreEqual(0F, oVertex.Location.X);
        Assert.AreEqual(0F, oVertex.Location.Y);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(13, oEdges.Count);

        // *edges

        FindEdge(oGraph, "I1", "W3", false, 1);
        FindEdge(oGraph, "W1", "W3", false, 1);
        FindEdge(oGraph, "W1", "W4", false, 1);
        FindEdge(oGraph, "W3", "W4", false, 1);
        FindEdge(oGraph, "W7", "W8", false, 1);
        FindEdge(oGraph, "W7", "W9", false, 1);
        FindEdge(oGraph, "W8", "W9", false, 1);
        FindEdge(oGraph, "W1", "S1", false, 1);
        FindEdge(oGraph, "W3", "S1", false, 1);
        FindEdge(oGraph, "W4", "S1", false, 1);
        FindEdge(oGraph, "W8", "S4", false, 1);
        FindEdge(oGraph, "W9", "S4", false, 1);
        FindEdge(oGraph, "W7", "S1", false, 1);

        // Verify that every edge was searched for and found.

        foreach (IEdge oEdge in oGraph.Edges)
        {
            Assert.IsTrue( oEdge.ContainsKey(EdgeFoundKey) );
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileNNNN()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFileNNNN()
    {
        // *edges: no
        // *edgeslist: no
        // *arcs: no
        // *arcslist: no

        const String FileContents =

            "*vertices 10\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3\r\n"
            + "2 \"Vertex 2\" 0.2 0.2 0.3\r\n"
            + "3 \"Vertex 3\" 0.3 0.2 0.3\r\n"
            + "4 \"Vertex 4\" 0.4 0.2 0.3\r\n"
            + "5 \"Vertex 5\" 0.5 0.2 0.3\r\n"
            + "6 \"Vertex 6\" 0.6 0.2 0.3\r\n"
            + "7 \"Vertex 7\" 0.7 0.2 0.3\r\n"
            + "8 \"Vertex 8\" 0.8 0.2 0.3\r\n"
            + "9 \"Vertex 9\" 0.9 0.2 0.3\r\n"
            + "10 \"Vertex 10\" 0.10 0.2 0.3\r\n"
            + "*edges\r\n"
            + "*edgeslist\r\n"
            + "*arcs\r\n"
            + "*arcslist\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Undirected, oGraph.Directedness);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(0, oEdges.Count);

        // *edges

        // (None.)

        // *edgeslist

        // (None.)

        // *arcs

        // (None.)

        // *arcslist

        // (None.)

        // Verify that every edge was searched for and found.

        foreach (IEdge oEdge in oGraph.Edges)
        {
            Assert.IsTrue( oEdge.ContainsKey(EdgeFoundKey) );
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileNNNY()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFileNNNY()
    {
        // *edges: no
        // *edgeslist: no
        // *arcs: no
        // *arcslist: yes

        const String FileContents =

            "*vertices 10\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3\r\n"
            + "2 \"Vertex 2\" 0.2 0.2 0.3\r\n"
            + "3 \"Vertex 3\" 0.3 0.2 0.3\r\n"
            + "4 \"Vertex 4\" 0.4 0.2 0.3\r\n"
            + "5 \"Vertex 5\" 0.5 0.2 0.3\r\n"
            + "6 \"Vertex 6\" 0.6 0.2 0.3\r\n"
            + "7 \"Vertex 7\" 0.7 0.2 0.3\r\n"
            + "8 \"Vertex 8\" 0.8 0.2 0.3\r\n"
            + "9 \"Vertex 9\" 0.9 0.2 0.3\r\n"
            + "10 \"Vertex 10\" 0.10 0.2 0.3\r\n"
            + "*arcslist\r\n"
            + "4 2 5\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Directed, oGraph.Directedness);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(2, oEdges.Count);

        // *edges

        // (None.)

        // *edgeslist

        // (None.)

        // *arcs

        // (None.)

        // *arcslist

        FindEdge(oGraph, 4, 2, true, 1);
        FindEdge(oGraph, 4, 5, true, 1);

        // Verify that every edge was searched for and found.

        foreach (IEdge oEdge in oGraph.Edges)
        {
            Assert.IsTrue( oEdge.ContainsKey(EdgeFoundKey) );
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileNNYN()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFileNNYN()
    {
        // *edges: no
        // *edgeslist: no
        // *arcs: yes
        // *arcslist: no

        const String FileContents =

            "*vertices 10\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3\r\n"
            + "2 \"Vertex 2\" 0.2 0.2 0.3\r\n"
            + "3 \"Vertex 3\" 0.3 0.2 0.3\r\n"
            + "4 \"Vertex 4\" 0.4 0.2 0.3\r\n"
            + "5 \"Vertex 5\" 0.5 0.2 0.3\r\n"
            + "6 \"Vertex 6\" 0.6 0.2 0.3\r\n"
            + "7 \"Vertex 7\" 0.7 0.2 0.3\r\n"
            + "8 \"Vertex 8\" 0.8 0.2 0.3\r\n"
            + "9 \"Vertex 9\" 0.9 0.2 0.3\r\n"
            + "10 \"Vertex 10\" 0.10 0.2 0.3\r\n"
            + "*arcs\r\n"
            + "10 9 123.34\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Directed, oGraph.Directedness);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(1, oEdges.Count);

        // *edges

        // (None.)

        // *edgeslist

        // (None.)

        // *arcs

        FindEdge(oGraph, 10, 9, true, 123.34);

        // *arcslist

        // (None.)

        // Verify that every edge was searched for and found.

        foreach (IEdge oEdge in oGraph.Edges)
        {
            Assert.IsTrue( oEdge.ContainsKey(EdgeFoundKey) );
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileNNYY()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFileNNYY()
    {
        // *edges: no
        // *edgeslist: no
        // *arcs: yes
        // *arcslist: yes

        const String FileContents =

            "*vertices 10\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3\r\n"
            + "2 \"Vertex 2\" 0.2 0.2 0.3\r\n"
            + "3 \"Vertex 3\" 0.3 0.2 0.3\r\n"
            + "4 \"Vertex 4\" 0.4 0.2 0.3\r\n"
            + "5 \"Vertex 5\" 0.5 0.2 0.3\r\n"
            + "6 \"Vertex 6\" 0.6 0.2 0.3\r\n"
            + "7 \"Vertex 7\" 0.7 0.2 0.3\r\n"
            + "8 \"Vertex 8\" 0.8 0.2 0.3\r\n"
            + "9 \"Vertex 9\" 0.9 0.2 0.3\r\n"
            + "10 \"Vertex 10\" 0.10 0.2 0.3\r\n"
            + "*arcs\r\n"
            + "10 9 123.34\r\n"
            + "*arcslist\r\n"
            + "4 2 5\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Directed, oGraph.Directedness);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(3, oEdges.Count);

        // *edges

        // (None.)

        // *edgeslist

        // (None.)

        // *arcs

        FindEdge(oGraph, 10, 9, true, 123.34);

        // *arcslist

        FindEdge(oGraph, 4, 2, true, 1);
        FindEdge(oGraph, 4, 5, true, 1);

        // Verify that every edge was searched for and found.

        foreach (IEdge oEdge in oGraph.Edges)
        {
            Assert.IsTrue( oEdge.ContainsKey(EdgeFoundKey) );
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileNYNN()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFileNYNN()
    {
        // *edges: no
        // *edgeslist: yes
        // *arcs: no
        // *arcslist: no

        const String FileContents =

            "*vertices 10\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3\r\n"
            + "2 \"Vertex 2\" 0.2 0.2 0.3\r\n"
            + "3 \"Vertex 3\" 0.3 0.2 0.3\r\n"
            + "4 \"Vertex 4\" 0.4 0.2 0.3\r\n"
            + "5 \"Vertex 5\" 0.5 0.2 0.3\r\n"
            + "6 \"Vertex 6\" 0.6 0.2 0.3\r\n"
            + "7 \"Vertex 7\" 0.7 0.2 0.3\r\n"
            + "8 \"Vertex 8\" 0.8 0.2 0.3\r\n"
            + "9 \"Vertex 9\" 0.9 0.2 0.3\r\n"
            + "10 \"Vertex 10\" 0.10 0.2 0.3\r\n"
            + "*edgeslist\r\n"
            + "1 4 5\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Undirected, oGraph.Directedness);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(2, oEdges.Count);

        // *edges

        // (None.)

        // *edgeslist

        FindEdge(oGraph, 1, 4, false, 1);
        FindEdge(oGraph, 1, 5, false, 1);

        // *arcs

        // (None.)

        // *arcslist

        // (None.)

        // Verify that every edge was searched for and found.

        foreach (IEdge oEdge in oGraph.Edges)
        {
            Assert.IsTrue( oEdge.ContainsKey(EdgeFoundKey) );
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileNYNY()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFileNYNY()
    {
        // *edges: no
        // *edgeslist: yes
        // *arcs: no
        // *arcslist: yes

        const String FileContents =

            "*vertices 10\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3\r\n"
            + "2 \"Vertex 2\" 0.2 0.2 0.3\r\n"
            + "3 \"Vertex 3\" 0.3 0.2 0.3\r\n"
            + "4 \"Vertex 4\" 0.4 0.2 0.3\r\n"
            + "5 \"Vertex 5\" 0.5 0.2 0.3\r\n"
            + "6 \"Vertex 6\" 0.6 0.2 0.3\r\n"
            + "7 \"Vertex 7\" 0.7 0.2 0.3\r\n"
            + "8 \"Vertex 8\" 0.8 0.2 0.3\r\n"
            + "9 \"Vertex 9\" 0.9 0.2 0.3\r\n"
            + "10 \"Vertex 10\" 0.10 0.2 0.3\r\n"
            + "*edgeslist\r\n"
            + "1 4 5\r\n"
            + "\r\n"
            + "*arcslist\r\n"
            + "4 2 5\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Mixed, oGraph.Directedness);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(4, oEdges.Count);

        // *edges

        // (None.)

        // *edgeslist

        FindEdge(oGraph, 1, 4, false, 1);
        FindEdge(oGraph, 1, 5, false, 1);

        // *arcs

        // (None.)

        // *arcslist

        FindEdge(oGraph, 4, 2, true, 1);
        FindEdge(oGraph, 4, 5, true, 1);

        // Verify that every edge was searched for and found.

        foreach (IEdge oEdge in oGraph.Edges)
        {
            Assert.IsTrue( oEdge.ContainsKey(EdgeFoundKey) );
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileNYYN()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFileNYYN()
    {
        // *edges: no
        // *edgeslist: yes
        // *arcs: yes
        // *arcslist: no

        const String FileContents =

            "*vertices 10\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3\r\n"
            + "2 \"Vertex 2\" 0.2 0.2 0.3\r\n"
            + "3 \"Vertex 3\" 0.3 0.2 0.3\r\n"
            + "4 \"Vertex 4\" 0.4 0.2 0.3\r\n"
            + "5 \"Vertex 5\" 0.5 0.2 0.3\r\n"
            + "6 \"Vertex 6\" 0.6 0.2 0.3\r\n"
            + "7 \"Vertex 7\" 0.7 0.2 0.3\r\n"
            + "8 \"Vertex 8\" 0.8 0.2 0.3\r\n"
            + "9 \"Vertex 9\" 0.9 0.2 0.3\r\n"
            + "10 \"Vertex 10\" 0.10 0.2 0.3\r\n"
            + "*edgeslist\r\n"
            + "1 4 5\r\n"
            + "\r\n"
            + "*arcs\r\n"
            + "10 9 123.34\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Mixed, oGraph.Directedness);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(3, oEdges.Count);

        // *edges

        // (None.)

        // *edgeslist

        FindEdge(oGraph, 1, 4, false, 1);
        FindEdge(oGraph, 1, 5, false, 1);

        // *arcs

        FindEdge(oGraph, 10, 9, true, 123.34);

        // *arcslist

        // (None.)

        // Verify that every edge was searched for and found.

        foreach (IEdge oEdge in oGraph.Edges)
        {
            Assert.IsTrue( oEdge.ContainsKey(EdgeFoundKey) );
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileNYYY()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFileNYYY()
    {
        // *edges: no
        // *edgeslist: yes
        // *arcs: yes
        // *arcslist: yes

        const String FileContents =

            "*vertices 10\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3\r\n"
            + "2 \"Vertex 2\" 0.2 0.2 0.3\r\n"
            + "3 \"Vertex 3\" 0.3 0.2 0.3\r\n"
            + "4 \"Vertex 4\" 0.4 0.2 0.3\r\n"
            + "5 \"Vertex 5\" 0.5 0.2 0.3\r\n"
            + "6 \"Vertex 6\" 0.6 0.2 0.3\r\n"
            + "7 \"Vertex 7\" 0.7 0.2 0.3\r\n"
            + "8 \"Vertex 8\" 0.8 0.2 0.3\r\n"
            + "9 \"Vertex 9\" 0.9 0.2 0.3\r\n"
            + "10 \"Vertex 10\" 0.10 0.2 0.3\r\n"
            + "*edgeslist\r\n"
            + "1 4 5\r\n"
            + "\r\n"
            + "*arcs\r\n"
            + "10 9 123.34\r\n"
            + "\r\n"
            + "*arcslist\r\n"
            + "4 2 5\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Mixed, oGraph.Directedness);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(5, oEdges.Count);

        // *edges

        // (None.)

        // *edgeslist

        FindEdge(oGraph, 1, 4, false, 1);
        FindEdge(oGraph, 1, 5, false, 1);

        // *arcs

        FindEdge(oGraph, 10, 9, true, 123.34);

        // *arcslist

        FindEdge(oGraph, 4, 2, true, 1);
        FindEdge(oGraph, 4, 5, true, 1);

        // Verify that every edge was searched for and found.

        foreach (IEdge oEdge in oGraph.Edges)
        {
            Assert.IsTrue( oEdge.ContainsKey(EdgeFoundKey) );
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileYNNN()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFileYNNN()
    {
        // *edges: yes
        // *edgeslist: no
        // *arcs: no
        // *arcslist: no

        const String FileContents =

            "*vertices 10\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3\r\n"
            + "2 \"Vertex 2\" 0.2 0.2 0.3\r\n"
            + "3 \"Vertex 3\" 0.3 0.2 0.3\r\n"
            + "4 \"Vertex 4\" 0.4 0.2 0.3\r\n"
            + "5 \"Vertex 5\" 0.5 0.2 0.3\r\n"
            + "6 \"Vertex 6\" 0.6 0.2 0.3\r\n"
            + "7 \"Vertex 7\" 0.7 0.2 0.3\r\n"
            + "8 \"Vertex 8\" 0.8 0.2 0.3\r\n"
            + "9 \"Vertex 9\" 0.9 0.2 0.3\r\n"
            + "10 \"Vertex 10\" 0.10 0.2 0.3\r\n"
            + "*edges\r\n"
            + "1 2 3.1\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Undirected, oGraph.Directedness);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(1, oEdges.Count);

        // *edges

        FindEdge(oGraph, 1, 2, false, 3.1);

        // *edgeslist

        // (None.)

        // *arcs

        // (None.)

        // *arcslist

        // (None.)

        // Verify that every edge was searched for and found.

        foreach (IEdge oEdge in oGraph.Edges)
        {
            Assert.IsTrue( oEdge.ContainsKey(EdgeFoundKey) );
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileYNNY()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFileYNNY()
    {
        // *edges: yes
        // *edgeslist: no
        // *arcs: no
        // *arcslist: yes

        const String FileContents =

            "*vertices 10\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3\r\n"
            + "2 \"Vertex 2\" 0.2 0.2 0.3\r\n"
            + "3 \"Vertex 3\" 0.3 0.2 0.3\r\n"
            + "4 \"Vertex 4\" 0.4 0.2 0.3\r\n"
            + "5 \"Vertex 5\" 0.5 0.2 0.3\r\n"
            + "6 \"Vertex 6\" 0.6 0.2 0.3\r\n"
            + "7 \"Vertex 7\" 0.7 0.2 0.3\r\n"
            + "8 \"Vertex 8\" 0.8 0.2 0.3\r\n"
            + "9 \"Vertex 9\" 0.9 0.2 0.3\r\n"
            + "10 \"Vertex 10\" 0.10 0.2 0.3\r\n"
            + "*edges\r\n"
            + "1 2 3.1\r\n"
            + "\r\n"
            + "*arcslist\r\n"
            + "4 2 5\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Mixed, oGraph.Directedness);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(3, oEdges.Count);

        // *edges

        FindEdge(oGraph, 1, 2, false, 3.1);

        // *edgeslist

        // (None.)

        // *arcs

        // (None.)

        // *arcslist

        FindEdge(oGraph, 4, 2, true, 1);
        FindEdge(oGraph, 4, 5, true, 1);

        // Verify that every edge was searched for and found.

        foreach (IEdge oEdge in oGraph.Edges)
        {
            Assert.IsTrue( oEdge.ContainsKey(EdgeFoundKey) );
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileYNYN()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFileYNYN()
    {
        // *edges: yes
        // *edgeslist: no
        // *arcs: yes
        // *arcslist: no

        const String FileContents =

            "*vertices 10\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3\r\n"
            + "2 \"Vertex 2\" 0.2 0.2 0.3\r\n"
            + "3 \"Vertex 3\" 0.3 0.2 0.3\r\n"
            + "4 \"Vertex 4\" 0.4 0.2 0.3\r\n"
            + "5 \"Vertex 5\" 0.5 0.2 0.3\r\n"
            + "6 \"Vertex 6\" 0.6 0.2 0.3\r\n"
            + "7 \"Vertex 7\" 0.7 0.2 0.3\r\n"
            + "8 \"Vertex 8\" 0.8 0.2 0.3\r\n"
            + "9 \"Vertex 9\" 0.9 0.2 0.3\r\n"
            + "10 \"Vertex 10\" 0.10 0.2 0.3\r\n"
            + "*edges\r\n"
            + "1 2 3.1\r\n"
            + "\r\n"
            + "*arcs\r\n"
            + "10 9 123.34\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Mixed, oGraph.Directedness);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(2, oEdges.Count);

        // *edges

        FindEdge(oGraph, 1, 2, false, 3.1);

        // *edgeslist

        // (None.)

        // *arcs

        FindEdge(oGraph, 10, 9, true, 123.34);

        // *arcslist

        // (None.)

        // Verify that every edge was searched for and found.

        foreach (IEdge oEdge in oGraph.Edges)
        {
            Assert.IsTrue( oEdge.ContainsKey(EdgeFoundKey) );
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileYNYY()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFileYNYY()
    {
        // *edges: yes
        // *edgeslist: no
        // *arcs: yes
        // *arcslist: yes

        const String FileContents =

            "*vertices 10\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3\r\n"
            + "2 \"Vertex 2\" 0.2 0.2 0.3\r\n"
            + "3 \"Vertex 3\" 0.3 0.2 0.3\r\n"
            + "4 \"Vertex 4\" 0.4 0.2 0.3\r\n"
            + "5 \"Vertex 5\" 0.5 0.2 0.3\r\n"
            + "6 \"Vertex 6\" 0.6 0.2 0.3\r\n"
            + "7 \"Vertex 7\" 0.7 0.2 0.3\r\n"
            + "8 \"Vertex 8\" 0.8 0.2 0.3\r\n"
            + "9 \"Vertex 9\" 0.9 0.2 0.3\r\n"
            + "10 \"Vertex 10\" 0.10 0.2 0.3\r\n"
            + "*edges\r\n"
            + "1 2 3.1\r\n"
            + "\r\n"
            + "*arcs\r\n"
            + "10 9 123.34\r\n"
            + "\r\n"
            + "*arcslist\r\n"
            + "4 2 5\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Mixed, oGraph.Directedness);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(4, oEdges.Count);

        // *edges

        FindEdge(oGraph, 1, 2, false, 3.1);

        // *edgeslist

        // (None.)

        // *arcs

        FindEdge(oGraph, 10, 9, true, 123.34);

        // *arcslist

        FindEdge(oGraph, 4, 2, true, 1);
        FindEdge(oGraph, 4, 5, true, 1);

        // Verify that every edge was searched for and found.

        foreach (IEdge oEdge in oGraph.Edges)
        {
            Assert.IsTrue( oEdge.ContainsKey(EdgeFoundKey) );
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileYYNN()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFileYYNN()
    {
        // *edges: yes
        // *edgeslist: yes
        // *arcs: no
        // *arcslist: no

        const String FileContents =

            "*vertices 10\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3\r\n"
            + "2 \"Vertex 2\" 0.2 0.2 0.3\r\n"
            + "3 \"Vertex 3\" 0.3 0.2 0.3\r\n"
            + "4 \"Vertex 4\" 0.4 0.2 0.3\r\n"
            + "5 \"Vertex 5\" 0.5 0.2 0.3\r\n"
            + "6 \"Vertex 6\" 0.6 0.2 0.3\r\n"
            + "7 \"Vertex 7\" 0.7 0.2 0.3\r\n"
            + "8 \"Vertex 8\" 0.8 0.2 0.3\r\n"
            + "9 \"Vertex 9\" 0.9 0.2 0.3\r\n"
            + "10 \"Vertex 10\" 0.10 0.2 0.3\r\n"
            + "*edges\r\n"
            + "1 2 3.1\r\n"
            + "\r\n"
            + "*edgeslist\r\n"
            + "1 4 5\r\n"
            + "\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Undirected, oGraph.Directedness);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(3, oEdges.Count);

        // *edges

        FindEdge(oGraph, 1, 2, false, 3.1);

        // *edgeslist

        FindEdge(oGraph, 1, 4, false, 1);
        FindEdge(oGraph, 1, 5, false, 1);

        // *arcs

        // (None.)

        // *arcslist

        // (None.)

        // Verify that every edge was searched for and found.

        foreach (IEdge oEdge in oGraph.Edges)
        {
            Assert.IsTrue( oEdge.ContainsKey(EdgeFoundKey) );
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileYYNY()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFileYYNY()
    {
        // *edges: yes
        // *edgeslist: yes
        // *arcs: no
        // *arcslist: yes

        const String FileContents =

            "*vertices 10\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3\r\n"
            + "2 \"Vertex 2\" 0.2 0.2 0.3\r\n"
            + "3 \"Vertex 3\" 0.3 0.2 0.3\r\n"
            + "4 \"Vertex 4\" 0.4 0.2 0.3\r\n"
            + "5 \"Vertex 5\" 0.5 0.2 0.3\r\n"
            + "6 \"Vertex 6\" 0.6 0.2 0.3\r\n"
            + "7 \"Vertex 7\" 0.7 0.2 0.3\r\n"
            + "8 \"Vertex 8\" 0.8 0.2 0.3\r\n"
            + "9 \"Vertex 9\" 0.9 0.2 0.3\r\n"
            + "10 \"Vertex 10\" 0.10 0.2 0.3\r\n"
            + "*edges\r\n"
            + "1 2 3.1\r\n"
            + "\r\n"
            + "*edgeslist\r\n"
            + "1 4 5\r\n"
            + "\r\n"
            + "*arcslist\r\n"
            + "4 2 5\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Mixed, oGraph.Directedness);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(5, oEdges.Count);

        // *edges

        FindEdge(oGraph, 1, 2, false, 3.1);

        // *edgeslist

        FindEdge(oGraph, 1, 4, false, 1);
        FindEdge(oGraph, 1, 5, false, 1);

        // *arcs

        // (None.)

        // *arcslist

        FindEdge(oGraph, 4, 2, true, 1);
        FindEdge(oGraph, 4, 5, true, 1);

        // Verify that every edge was searched for and found.

        foreach (IEdge oEdge in oGraph.Edges)
        {
            Assert.IsTrue( oEdge.ContainsKey(EdgeFoundKey) );
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileYYYN()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFileYYYN()
    {
        // *edges: yes
        // *edgeslist: yes
        // *arcs: yes
        // *arcslist: no

        const String FileContents =

            "*vertices 10\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3\r\n"
            + "2 \"Vertex 2\" 0.2 0.2 0.3\r\n"
            + "3 \"Vertex 3\" 0.3 0.2 0.3\r\n"
            + "4 \"Vertex 4\" 0.4 0.2 0.3\r\n"
            + "5 \"Vertex 5\" 0.5 0.2 0.3\r\n"
            + "6 \"Vertex 6\" 0.6 0.2 0.3\r\n"
            + "7 \"Vertex 7\" 0.7 0.2 0.3\r\n"
            + "8 \"Vertex 8\" 0.8 0.2 0.3\r\n"
            + "9 \"Vertex 9\" 0.9 0.2 0.3\r\n"
            + "10 \"Vertex 10\" 0.10 0.2 0.3\r\n"
            + "*edges\r\n"
            + "1 2 3.1\r\n"
            + "\r\n"
            + "*edgeslist\r\n"
            + "1 4 5\r\n"
            + "\r\n"
            + "*arcs\r\n"
            + "10 9 123.34\r\n"
            + "\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Mixed, oGraph.Directedness);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(4, oEdges.Count);

        // *edges

        FindEdge(oGraph, 1, 2, false, 3.1);

        // *edgeslist

        FindEdge(oGraph, 1, 4, false, 1);
        FindEdge(oGraph, 1, 5, false, 1);

        // *arcs

        FindEdge(oGraph, 10, 9, true, 123.34);

        // *arcslist

        // (None.)

        // Verify that every edge was searched for and found.

        foreach (IEdge oEdge in oGraph.Edges)
        {
            Assert.IsTrue( oEdge.ContainsKey(EdgeFoundKey) );
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileYYYY()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromFileYYYY()
    {
        // *edges: yes
        // *edgeslist: yes
        // *arcs: yes
        // *arcslist: yes

        const String FileContents =

            "*vertices 10\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3\r\n"
            + "2 \"Vertex 2\" 0.2 0.2 0.3\r\n"
            + "3 \"Vertex 3\" 0.3 0.2 0.3\r\n"
            + "4 \"Vertex 4\" 0.4 0.2 0.3\r\n"
            + "5 \"Vertex 5\" 0.5 0.2 0.3\r\n"
            + "6 \"Vertex 6\" 0.6 0.2 0.3\r\n"
            + "7 \"Vertex 7\" 0.7 0.2 0.3\r\n"
            + "8 \"Vertex 8\" 0.8 0.2 0.3\r\n"
            + "9 \"Vertex 9\" 0.9 0.2 0.3\r\n"
            + "10 \"Vertex 10\" 0.10 0.2 0.3\r\n"
            + "*edges\r\n"
            + "1 2 3.1\r\n"
            + "\r\n"
            + "*edgeslist\r\n"
            + "1 4 5\r\n"
            + "\r\n"
            + "*arcs\r\n"
            + "10 9 123.34\r\n"
            + "\r\n"
            + "*arcslist\r\n"
            + "4 2 5\r\n"
            ;

        WriteFile(FileContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Mixed, oGraph.Directedness);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(6, oEdges.Count);

        // *edges

        FindEdge(oGraph, 1, 2, false, 3.1);

        // *edgeslist

        FindEdge(oGraph, 1, 4, false, 1);
        FindEdge(oGraph, 1, 5, false, 1);

        // *arcs

        FindEdge(oGraph, 10, 9, true, 123.34);

        // *arcslist

        FindEdge(oGraph, 4, 2, true, 1);
        FindEdge(oGraph, 4, 5, true, 1);

        // Verify that every edge was searched for and found.

        foreach (IEdge oEdge in oGraph.Edges)
        {
            Assert.IsTrue( oEdge.ContainsKey(EdgeFoundKey) );
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestLoadGraphFromFileBad()
    {
        // null filename.

        try
        {
            String sFileName = null;

            m_oGraphAdapter.LoadGraphFromFile(sFileName);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Adapters."
                + "PajekGraphAdapter.LoadGraphFromFile: filename argument"
                + " can't be null.\r\n"
                + "Parameter name: filename"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad2()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestLoadGraphFromFileBad2()
    {
        // Empty filename.

        try
        {
            String sFileName = String.Empty;

            m_oGraphAdapter.LoadGraphFromFile(sFileName);
        }
        catch (ArgumentNullException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Adapters."
                + "PajekGraphAdapter.LoadGraphFromFile: filename argument must"
                + " have a length greater than zero.\r\n"
                + "Parameter name: filename"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad3()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(DirectoryNotFoundException) ) ]

    public void
    TestLoadGraphFromFileBad3()
    {
        // Non-existent filename.

        try
        {
            String sFileName = "X:\\abc\\def\\ghi.txt";

            m_oGraphAdapter.LoadGraphFromFile(sFileName);
        }
        catch (DirectoryNotFoundException oDirectoryNotFoundException)
        {
            Assert.IsTrue(oDirectoryNotFoundException.Message.Contains(
                "Could not find a part of the path"
                ) );

            throw oDirectoryNotFoundException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad4()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad4()
    {
        // 2 *vertices sections.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3\r\n"
            + "*vertices 1\r\n"
            + "2 \"Vertex 2\" 0.1 0.2 0.3\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 3 is not in the expected format.  This is line 3:"
                + " \"*vertices 1\".  There can't be more than one *vertices"
                + " section."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad5()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad5()
    {
        // Too many field on *vertices line.

        const String FileContents =

            "*vertices 1 xx\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 1 is not in the expected format.  This is line 1:"
                + " \"*vertices 1 xx\".  The expected format is \"*vertices"
                + " N\"."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad6()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad6()
    {
        // Bad specified vertex count.

        const String FileContents =

            "*vertices a\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 1 is not in the expected format.  This is line 1:"
                + " \"*vertices a\".  The expected format is \"*vertices N\"."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad7()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad7()
    {
        // Too few vertices.

        const String FileContents =

            "*vertices 2\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "The *vertices section specified 2 vertices but contained 1"
                + " vertex."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad8()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad8()
    {
        // Too many vertices.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3\r\n"
            + "2 \"Vertex 2\" 0.1 0.2 0.3\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "There are too many vertices in the *vertices section, which"
                + " specified only 1 vertex on the *vertices line."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad9()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad9()
    {
        // Too many vertices.

        const String FileContents =

            "*vertices 0\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "There are too many vertices in the *vertices section, which"
                + " specified only 0 vertices on the *vertices line."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad10()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad10()
    {
        // *edges section without *vertices.

        const String FileContents =

            "*edges\r\n"
            + "3 4 5.11\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 1 is not in the expected format.  This is line 1:"
                + " \"*edges\".  There can't be an *edges section without a"
                + " *vertices section."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad11()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad11()
    {
        // *edgeslist section without *vertices.

        const String FileContents =

            "*edgeslist\r\n"
            + "3 4 5\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 1 is not in the expected format.  This is line 1:"
                + " \"*edgeslist\".  There can't be an *edgeslist section"
                + " without a *vertices section."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad12()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad12()
    {
        // *arcs section without *vertices.

        const String FileContents =

            "*arcs\r\n"
            + "3 4 5.11\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 1 is not in the expected format.  This is line 1:"
                + " \"*arcs\".  There can't be an *arcs section without a"
                + " *vertices section."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad13()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad13()
    {
        // *arcslist section without *vertices.

        const String FileContents =

            "*arcslist\r\n"
            + "3 4 5\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 1 is not in the expected format.  This is line 1:"
                + " \"*arcslist\".  There can't be an *arcslist section"
                + " without a *vertices section."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad14()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad14()
    {
        // Bad vertex number.

        const String FileContents =

            "*vertices 1\r\n"
            + "a Vertex1 0 0 0\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 2 is not in the expected format.  This is line 2:"
                + " \"a Vertex1 0 0 0\".  The expected format is \"N \"vertex"
                + " name\" [x y z]\"."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad15()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad15()
    {
        // Wrong vertex number.

        const String FileContents =

            "*vertices 1\r\n"
            + "2 Vertex1 0 0 0\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 2 is not in the expected format.  This is line 2:"
                + " \"2 Vertex1 0 0 0\".  Vertices must be numbered"
                + " consecutively starting at 1."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad16()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad16()
    {
        // X-coordinate < 0.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 -1 0 0\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 2 is not in the expected format.  This is line 2:"
                + " \"1 Vertex1 -1 0 0\".  Vertex coordinates must be between"
                + " 0 and 1.0."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad17()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad17()
    {
        // X-coordinate > 1.0.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 2 0 0\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 2 is not in the expected format.  This is line 2:"
                + " \"1 Vertex1 2 0 0\".  Vertex coordinates must be between"
                + " 0 and 1.0."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad18()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad18()
    {
        // Y-coordinate < 0.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 0 -1 0\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 2 is not in the expected format.  This is line 2:"
                + " \"1 Vertex1 0 -1 0\".  Vertex coordinates must be between"
                + " 0 and 1.0."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad19()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad19()
    {
        // Y-coordinate > 1.0.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 0 2 0\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 2 is not in the expected format.  This is line 2:"
                + " \"1 Vertex1 0 2 0\".  Vertex coordinates must be between"
                + " 0 and 1.0."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad20()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad20()
    {
        // Bad first vertex number on edge.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 0 0 0\r\n"
            + "*edges\r\n"
            + "1yz 1 3.1\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 4 is not in the expected format.  This is line 4:"
                + " \"1yz 1 3.1\".  The expected format is \"Vi Vj weight\"."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad21()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad21()
    {
        // Bad second vertex number on edge.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 0 0 0\r\n"
            + "*edges\r\n"
            + "1 1yx 3.1\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 4 is not in the expected format.  This is line 4:"
                + " \"1 1yx 3.1\".  The expected format is \"Vi Vj weight\"."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad22()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad22()
    {
        // Bad weight on edge.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 0 0 0\r\n"
            + "*edges\r\n"
            + "1 1 a3.1\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 4 is not in the expected format.  This is line 4:"
                + " \"1 1 a3.1\".  The expected format is \"Vi Vj weight\"."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad23()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad23()
    {
        // Bad weight on edge.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 0 0 0\r\n"
            + "*edges\r\n"
            + "1 1 3.1.2\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 4 is not in the expected format.  This is line 4:"
                + " \"1 1 3.1.2\".  The expected format is \"Vi Vj weight\"."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad24()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad24()
    {
        // Bad first vertex number on arc.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 0 0 0\r\n"
            + "*arcs\r\n"
            + "1yz 1 3.1\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 4 is not in the expected format.  This is line 4:"
                + " \"1yz 1 3.1\".  The expected format is \"Vi Vj weight\"."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad25()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad25()
    {
        // Bad second vertex number on arc.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 0 0 0\r\n"
            + "*arcs\r\n"
            + "1 1yx 3.1\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 4 is not in the expected format.  This is line 4:"
                + " \"1 1yx 3.1\".  The expected format is \"Vi Vj weight\"."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad26()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad26()
    {
        // Bad weight on arc.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 0 0 0\r\n"
            + "*arcs\r\n"
            + "1 1 a3.1\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 4 is not in the expected format.  This is line 4:"
                + " \"1 1 a3.1\".  The expected format is \"Vi Vj weight\"."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad27()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad27()
    {
        // Too few vertices in edge list.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 0 0 0\r\n"
            + "*edgeslist\r\n"
            + "1\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 4 is not in the expected format.  This is line 4:"
                + " \"1\".  The expected format is \"Vi Vj Vk ...\"."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad28()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad28()
    {
        // Bad first vertex number in edge list.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 0 0 0\r\n"
            + "*edgeslist\r\n"
            + "dddd 1\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 4 is not in the expected format.  This is line 4:"
                + " \"dddd 1\".  The expected format is \"Vi Vj Vk ...\"."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad29()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad29()
    {
        // Zero first vertex number in edge list.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 0 0 0\r\n"
            + "*edgeslist\r\n"
            + "0 1\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 4 is not in the expected format.  This is line 4:"
                + " \"0 1\".  Vertex numbers must be greater than 0."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad30()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad30()
    {
        // Negative first vertex number in edge list.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 0 0 0\r\n"
            + "*edgeslist\r\n"
            + "-1 1\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 4 is not in the expected format.  This is line 4:"
                + " \"-1 1\".  Vertex numbers must be greater than 0."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad31()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad31()
    {
        // First vertex number in edge list greater than number of vertices.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 0 0 0\r\n"
            + "*edgeslist\r\n"
            + "2 1\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 4 is not in the expected format.  This is line 4:"
                + " \"2 1\".  Vertex numbers can't be greater than the number"
                + " of vertices."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad32()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad32()
    {
        // Bad second vertex number in edge list.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 0 0 0\r\n"
            + "*edgeslist\r\n"
            + "1 dddd\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 4 is not in the expected format.  This is line 4:"
                + " \"1 dddd\".  The expected format is \"Vi Vj Vk ...\"."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad33()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad33()
    {
        // Zero second vertex number in edge list.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 0 0 0\r\n"
            + "*edgeslist\r\n"
            + "1 0\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 4 is not in the expected format.  This is line 4:"
                + " \"1 0\".  Vertex numbers must be greater than 0."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad34()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad34()
    {
        // Negative second vertex number in edge list.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 0 0 0\r\n"
            + "*edgeslist\r\n"
            + "1 -1\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 4 is not in the expected format.  This is line 4:"
                + " \"1 -1\".  Vertex numbers must be greater than 0."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad35()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad35()
    {
        // Second vertex number in edge list greater than number of vertices.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 0 0 0\r\n"
            + "*edgeslist\r\n"
            + "1 2\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 4 is not in the expected format.  This is line 4:"
                + " \"1 2\".  Vertex numbers can't be greater than the number"
                + " of vertices."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad36()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad36()
    {
        // Too few vertices in arc list.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 0 0 0\r\n"
            + "*arcslist\r\n"
            + "1\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 4 is not in the expected format.  This is line 4:"
                + " \"1\".  The expected format is \"Vi Vj Vk ...\"."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad37()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad37()
    {
        // Bad first vertex number in arc list.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 0 0 0\r\n"
            + "*arcslist\r\n"
            + "dddd 1\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 4 is not in the expected format.  This is line 4:"
                + " \"dddd 1\".  The expected format is \"Vi Vj Vk ...\"."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad38()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad38()
    {
        // Zero first vertex number in arc list.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 0 0 0\r\n"
            + "*arcslist\r\n"
            + "0 1\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 4 is not in the expected format.  This is line 4:"
                + " \"0 1\".  Vertex numbers must be greater than 0."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad39()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad39()
    {
        // Negative first vertex number in arc list.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 0 0 0\r\n"
            + "*arcslist\r\n"
            + "-1 1\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 4 is not in the expected format.  This is line 4:"
                + " \"-1 1\".  Vertex numbers must be greater than 0."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad40()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad40()
    {
        // First vertex number in arc list greater than number of vertices.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 0 0 0\r\n"
            + "*arcslist\r\n"
            + "2 1\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 4 is not in the expected format.  This is line 4:"
                + " \"2 1\".  Vertex numbers can't be greater than the number"
                + " of vertices."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad41()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad41()
    {
        // Bad second vertex number in arc list.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 0 0 0\r\n"
            + "*arcslist\r\n"
            + "1 dddd\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 4 is not in the expected format.  This is line 4:"
                + " \"1 dddd\".  The expected format is \"Vi Vj Vk ...\"."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad42()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad42()
    {
        // Zero second vertex number in arc list.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 0 0 0\r\n"
            + "*arcslist\r\n"
            + "1 0\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 4 is not in the expected format.  This is line 4:"
                + " \"1 0\".  Vertex numbers must be greater than 0."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad43()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad43()
    {
        // Negative second vertex number in arc list.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 0 0 0\r\n"
            + "*arcslist\r\n"
            + "1 -1\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 4 is not in the expected format.  This is line 4:"
                + " \"1 -1\".  Vertex numbers must be greater than 0."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromFileBad44()
    //
    /// <summary>
    /// Tests the LoadGraphFromFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(FormatException) ) ]

    public void
    TestLoadGraphFromFileBad44()
    {
        // Second vertex number in arc list greater than number of vertices.

        const String FileContents =

            "*vertices 1\r\n"
            + "1 Vertex1 0 0 0\r\n"
            + "*arcslist\r\n"
            + "1 2\r\n"
            ;

        WriteFile(FileContents);

        try
        {
            m_oGraphAdapter.LoadGraphFromFile(m_sTempFileName);
        }
        catch (FormatException oFormatException)
        {
            Assert.AreEqual(

                "Line 4 is not in the expected format.  This is line 4:"
                + " \"1 2\".  Vertex numbers can't be greater than the number"
                + " of vertices."
                ,
                oFormatException.Message
                );

            throw oFormatException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadFromStream()
    //
    /// <summary>
    /// Tests the LoadGraphFromStream() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadFromStream()
    {
        // Overall test.

        const Int32 Vertices = 10;

        const String StreamContents =

            "/* This is a comment. */\r\n"
            + "\r\n"
            + "*vertices 10\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3 ignored parameters\r\n"
            + "2 \"Vertex 2\" 0.2 0.2 0.3\r\n"
            + "3 \"Vertex 3\" 0.3 0.2 0.3\r\n"
            + "4 \"Vertex 4\" 0.4 0.2 0.3\r\n"
            + "   /* This is a comment. */\r\n"
            + "5 \"Vertex 5\" 0.5 0.2 0.3\r\n"
            + "6 \"Vertex 6\" 0.6 0.2 0.3\r\n"
            + "7 \"Vertex 7\" 0.7 0.2 0.3\r\n"
            + "8 \"Vertex 8\" 0.8 0.2 0.3\r\n"
            + "9 \"Vertex 9\" 0.9 0.2 0.3\r\n"
            + "10 \"Vertex 10\" 0.10 0.2 0.3\r\n"
            + "\r\n"
            + "*edges\r\n"
            + "1 2 3.1 ignored parameters\r\n"
            + "   /* This is a comment. */\r\n"
            + "3 4 4.2\r\n"
            + "9 8 1.2\r\n"
            + "\r\n"
            + "*edgeslist\r\n"
            + "1 4 5\r\n"
            + "   /* This is a comment. */\r\n"
            + "6 8 3 2\r\n"
            + "\r\n"
            + "*arcs\r\n"
            + "10 9 123.34 ignored parameters\r\n"
            + "9 1 98.7\r\n"
            + "\r\n"
            + "*arcslist\r\n"
            + "   /* This is a comment. */\r\n"
            + "4 2 5\r\n"
            + "8 7 1 2\r\n"
            + "\r\n"
            + "/* This is a comment. */\r\n"
            ;

        StringStream oStream = new StringStream(StreamContents);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromStream(oStream);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Mixed, oGraph.Directedness);

        IVertexCollection oVertices = oGraph.Vertices;

        Assert.AreEqual(Vertices, oVertices.Count);

        for (Int32 i = 0; i < Vertices; i++)
        {
            Assert.IsTrue( oVertices.Contains(GetVertexName(i + 1) ) );
        }

        IVertex oVertex;
        
        oVertices.Find("Vertex 9", out oVertex);

        Assert.IsNotNull(oVertex);
        Assert.AreEqual(0.9F, oVertex.Location.X);
        Assert.AreEqual(0.2F, oVertex.Location.Y);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(15, oEdges.Count);

        // *edges

        FindEdge(oGraph, 1, 2, false, 3.1);
        FindEdge(oGraph, 3, 4, false, 4.2);
        FindEdge(oGraph, 9, 8, false, 1.2);

        // *edgeslist

        FindEdge(oGraph, 1, 4, false, 1);
        FindEdge(oGraph, 1, 5, false, 1);

        FindEdge(oGraph, 6, 8, false, 1);
        FindEdge(oGraph, 6, 3, false, 1);
        FindEdge(oGraph, 6, 2, false, 1);

        // *arcs

        FindEdge(oGraph, 10, 9, true, 123.34);
        FindEdge(oGraph, 9, 1, true, 98.7);

        // *arcslist

        FindEdge(oGraph, 4, 2, true, 1);
        FindEdge(oGraph, 4, 5, true, 1);

        FindEdge(oGraph, 8, 7, true, 1);
        FindEdge(oGraph, 8, 1, true, 1);
        FindEdge(oGraph, 8, 2, true, 1);

        // Verify that every edge was searched for and found.

        foreach (IEdge oEdge in oGraph.Edges)
        {
            Assert.IsTrue( oEdge.ContainsKey(EdgeFoundKey) );
        }
    }

    //*************************************************************************
    //  Method: TestLoadFromStreamBad()
    //
    /// <summary>
    /// Tests the LoadGraphFromStream() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestLoadFromStreamBad()
    {
        // null stream.

        try
        {
            m_oGraphAdapter.LoadGraphFromStream(null);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Adapters."
                + "PajekGraphAdapter.LoadGraph: stream argument can't"
                + " be null.\r\n"
                + "Parameter name: stream"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromString()
    //
    /// <summary>
    /// Tests the LoadGraphFromString() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromString()
    {
        // Overall test.

        const Int32 Vertices = 10;

        const String StringContents =

            "/* This is a comment. */\r\n"
            + "\r\n"
            + "*vertices 10\r\n"
            + "1 \"Vertex 1\" 0.1 0.2 0.3 ignored parameters\r\n"
            + "2 \"Vertex 2\" 0.2 0.2 0.3\r\n"
            + "3 \"Vertex 3\" 0.3 0.2 0.3\r\n"
            + "4 \"Vertex 4\" 0.4 0.2 0.3\r\n"
            + "   /* This is a comment. */\r\n"
            + "5 \"Vertex 5\" 0.5 0.2 0.3\r\n"
            + "6 \"Vertex 6\" 0.6 0.2 0.3\r\n"
            + "7 \"Vertex 7\" 0.7 0.2 0.3\r\n"
            + "8 \"Vertex 8\" 0.8 0.2 0.3\r\n"
            + "9 \"Vertex 9\" 0.9 0.2 0.3\r\n"
            + "10 \"Vertex 10\" 0.10 0.2 0.3\r\n"
            + "\r\n"
            + "*edges\r\n"
            + "1 2 3.1 ignored parameters\r\n"
            + "   /* This is a comment. */\r\n"
            + "3 4 4.2\r\n"
            + "9 8 1.2\r\n"
            + "\r\n"
            + "*edgeslist\r\n"
            + "1 4 5\r\n"
            + "   /* This is a comment. */\r\n"
            + "6 8 3 2\r\n"
            + "\r\n"
            + "*arcs\r\n"
            + "10 9 123.34 ignored parameters\r\n"
            + "9 1 98.7\r\n"
            + "\r\n"
            + "*arcslist\r\n"
            + "   /* This is a comment. */\r\n"
            + "4 2 5\r\n"
            + "8 7 1 2\r\n"
            + "\r\n"
            + "/* This is a comment. */\r\n"
            ;

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromString(StringContents);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        Assert.AreEqual(GraphDirectedness.Mixed, oGraph.Directedness);

        IVertexCollection oVertices = oGraph.Vertices;

        Assert.AreEqual(Vertices, oVertices.Count);

        for (Int32 i = 0; i < Vertices; i++)
        {
            Assert.IsTrue( oVertices.Contains(GetVertexName(i + 1) ) );
        }

        IVertex oVertex;
        
        oVertices.Find("Vertex 9", out oVertex);

        Assert.IsNotNull(oVertex);
        Assert.AreEqual(0.9F, oVertex.Location.X);
        Assert.AreEqual(0.2F, oVertex.Location.Y);

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(15, oEdges.Count);

        // *edges

        FindEdge(oGraph, 1, 2, false, 3.1);
        FindEdge(oGraph, 3, 4, false, 4.2);
        FindEdge(oGraph, 9, 8, false, 1.2);

        // *edgeslist

        FindEdge(oGraph, 1, 4, false, 1);
        FindEdge(oGraph, 1, 5, false, 1);

        FindEdge(oGraph, 6, 8, false, 1);
        FindEdge(oGraph, 6, 3, false, 1);
        FindEdge(oGraph, 6, 2, false, 1);

        // *arcs

        FindEdge(oGraph, 10, 9, true, 123.34);
        FindEdge(oGraph, 9, 1, true, 98.7);

        // *arcslist

        FindEdge(oGraph, 4, 2, true, 1);
        FindEdge(oGraph, 4, 5, true, 1);

        FindEdge(oGraph, 8, 7, true, 1);
        FindEdge(oGraph, 8, 1, true, 1);
        FindEdge(oGraph, 8, 2, true, 1);

        // Verify that every edge was searched for and found.

        foreach (IEdge oEdge in oGraph.Edges)
        {
            Assert.IsTrue( oEdge.ContainsKey(EdgeFoundKey) );
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromStringBad()
    //
    /// <summary>
    /// Tests the LoadGraphFromString() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestLoadGraphFromStringBad()
    {
        // null string.

        try
        {
            m_oGraphAdapter.LoadGraphFromString(null);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Adapters."
                + "PajekGraphAdapter.LoadGraph: theString argument can't"
                + " be null.\r\n"
                + "Parameter name: theString"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestSaveGraph()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSaveGraph()
    {
        // Mixed graph.

        const Int32 Vertices = 10;

        IGraph oGraph = CreateGraph(GraphDirectedness.Mixed);

        IVertexCollection oVertices = oGraph.Vertices;

        IEdgeCollection oEdges = oGraph.Edges;

        IVertex [] aoVertices = TestGraphUtil.AddVertices(oGraph, Vertices);

        for (Int32 i = 0; i < Vertices; i++)
        {
            aoVertices[i].Name = (i + 1).ToString();
        }

        aoVertices[0].Location = new PointF(0.12F, 0.34F);

        IEdge oEdge;

        oEdge = oEdges.Add(aoVertices[0], aoVertices[1], false);
        oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, 1.23);

        oEdge = oEdges.Add(aoVertices[1], aoVertices[2], true);
        oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, 2.01);

        oEdge = oEdges.Add(aoVertices[2], aoVertices[3], false);
        oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, 5.11);

        oEdges.Add(aoVertices[3], aoVertices[4], true);
        // (No weight, should be default.)

        m_oGraphAdapter.SaveGraph(oGraph, m_sTempFileName);

        String sFileContents;

        using ( StreamReader oStreamReader = new StreamReader(m_sTempFileName) )
        {
            sFileContents = oStreamReader.ReadToEnd();
        }

        const String ExpectedFileContents =

            "*vertices 10\r\n"
            + "1 \"1\" 1.000000 1.000000 0\r\n"
            + "2 \"2\" 0.000000 0.000000 0\r\n"
            + "3 \"3\" 0.000000 0.000000 0\r\n"
            + "4 \"4\" 0.000000 0.000000 0\r\n"
            + "5 \"5\" 0.000000 0.000000 0\r\n"
            + "6 \"6\" 0.000000 0.000000 0\r\n"
            + "7 \"7\" 0.000000 0.000000 0\r\n"
            + "8 \"8\" 0.000000 0.000000 0\r\n"
            + "9 \"9\" 0.000000 0.000000 0\r\n"
            + "10 \"10\" 0.000000 0.000000 0\r\n"
            + "*edges\r\n"
            + "3 4 5.11\r\n"
            + "1 2 1.23\r\n"
            + "*arcs\r\n"
            + "4 5 1\r\n"
            + "2 3 2.01\r\n"
            ;

        Assert.AreEqual(ExpectedFileContents, sFileContents);
    }

    //*************************************************************************
    //  Method: TestSaveGraph2()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSaveGraph2()
    {
        // Empty graph.

        IGraph oGraph = CreateGraph();

        m_oGraphAdapter.SaveGraph(oGraph, m_sTempFileName);

        String sFileContents;

        using ( StreamReader oStreamReader = new StreamReader(m_sTempFileName) )
        {
            sFileContents = oStreamReader.ReadToEnd();
        }

        const String ExpectedFileContents =
            "*vertices 0\r\n"
            ;

        Assert.AreEqual(ExpectedFileContents, sFileContents);
    }

    //*************************************************************************
    //  Method: TestSaveGraph3()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSaveGraph3()
    {
        // Vertices, no edges.

        const Int32 Vertices = 10;

        IGraph oGraph = CreateGraph(GraphDirectedness.Mixed);

        IVertexCollection oVertices = oGraph.Vertices;

        IEdgeCollection oEdges = oGraph.Edges;

        IVertex [] aoVertices = TestGraphUtil.AddVertices(oGraph, Vertices);

        for (Int32 i = 0; i < Vertices; i++)
        {
            aoVertices[i].Name = (i + 1).ToString();
        }

        aoVertices[0].Location = new PointF(0.12F, 0.34F);

        m_oGraphAdapter.SaveGraph(oGraph, m_sTempFileName);

        String sFileContents;

        using ( StreamReader oStreamReader = new StreamReader(m_sTempFileName) )
        {
            sFileContents = oStreamReader.ReadToEnd();
        }

        const String ExpectedFileContents =

            "*vertices 10\r\n"
            + "1 \"1\" 1.000000 1.000000 0\r\n"
            + "2 \"2\" 0.000000 0.000000 0\r\n"
            + "3 \"3\" 0.000000 0.000000 0\r\n"
            + "4 \"4\" 0.000000 0.000000 0\r\n"
            + "5 \"5\" 0.000000 0.000000 0\r\n"
            + "6 \"6\" 0.000000 0.000000 0\r\n"
            + "7 \"7\" 0.000000 0.000000 0\r\n"
            + "8 \"8\" 0.000000 0.000000 0\r\n"
            + "9 \"9\" 0.000000 0.000000 0\r\n"
            + "10 \"10\" 0.000000 0.000000 0\r\n"
            ;

        Assert.AreEqual(ExpectedFileContents, sFileContents);
    }

    //*************************************************************************
    //  Method: TestSaveGraph4()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSaveGraph4()
    {
        // Duplicate edges.

        const Int32 Vertices = 10;

        IGraph oGraph = CreateGraph(GraphDirectedness.Mixed);

        IVertexCollection oVertices = oGraph.Vertices;

        IEdgeCollection oEdges = oGraph.Edges;

        IVertex [] aoVertices = TestGraphUtil.AddVertices(oGraph, Vertices);

        for (Int32 i = 0; i < Vertices; i++)
        {
            aoVertices[i].Name = (i + 1).ToString();
        }

        aoVertices[0].Location = new PointF(0.12F, 0.34F);

        IEdge oEdge;

        oEdge = oEdges.Add(aoVertices[0], aoVertices[1], false);
        oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, 1.23);

        oEdge = oEdges.Add(aoVertices[0], aoVertices[1], false);
        oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, 1.23);

        oEdge = oEdges.Add(aoVertices[1], aoVertices[2], true);
        oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, 2.01);

        oEdge = oEdges.Add(aoVertices[2], aoVertices[3], false);
        oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, 5.11);

        oEdges.Add(aoVertices[3], aoVertices[4], true);
        // (No weight, should be default.)

        m_oGraphAdapter.SaveGraph(oGraph, m_sTempFileName);

        String sFileContents;

        using ( StreamReader oStreamReader = new StreamReader(m_sTempFileName) )
        {
            sFileContents = oStreamReader.ReadToEnd();
        }

        const String ExpectedFileContents =

            "*vertices 10\r\n"
            + "1 \"1\" 1.000000 1.000000 0\r\n"
            + "2 \"2\" 0.000000 0.000000 0\r\n"
            + "3 \"3\" 0.000000 0.000000 0\r\n"
            + "4 \"4\" 0.000000 0.000000 0\r\n"
            + "5 \"5\" 0.000000 0.000000 0\r\n"
            + "6 \"6\" 0.000000 0.000000 0\r\n"
            + "7 \"7\" 0.000000 0.000000 0\r\n"
            + "8 \"8\" 0.000000 0.000000 0\r\n"
            + "9 \"9\" 0.000000 0.000000 0\r\n"
            + "10 \"10\" 0.000000 0.000000 0\r\n"
            + "*edges\r\n"
            + "3 4 5.11\r\n"
            + "1 2 1.23\r\n"
            + "1 2 1.23\r\n"
            + "*arcs\r\n"
            + "4 5 1\r\n"
            + "2 3 2.01\r\n"
            ;

        Assert.AreEqual(ExpectedFileContents, sFileContents);
    }

    //*************************************************************************
    //  Method: TestSaveGraph5()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSaveGraph5()
    {
        // Directed graph.

        const Int32 Vertices = 10;

        IGraph oGraph = CreateGraph(GraphDirectedness.Directed);

        IVertexCollection oVertices = oGraph.Vertices;

        IEdgeCollection oEdges = oGraph.Edges;

        IVertex [] aoVertices = TestGraphUtil.AddVertices(oGraph, Vertices);

        for (Int32 i = 0; i < Vertices; i++)
        {
            aoVertices[i].Name = (i + 1).ToString();
        }

        aoVertices[0].Location = new PointF(0.12F, 0.34F);

        IEdge oEdge;

        oEdge = oEdges.Add(aoVertices[0], aoVertices[1], true);
        oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, 1.23);

        oEdge = oEdges.Add(aoVertices[1], aoVertices[2], true);
        oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, 2.01);

        oEdge = oEdges.Add(aoVertices[2], aoVertices[3], true);
        oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, 5.11);

        oEdges.Add(aoVertices[3], aoVertices[4], true);
        // (No weight, should be default.)

        m_oGraphAdapter.SaveGraph(oGraph, m_sTempFileName);

        String sFileContents;

        using ( StreamReader oStreamReader = new StreamReader(m_sTempFileName) )
        {
            sFileContents = oStreamReader.ReadToEnd();
        }

        const String ExpectedFileContents =

            "*vertices 10\r\n"
            + "1 \"1\" 1.000000 1.000000 0\r\n"
            + "2 \"2\" 0.000000 0.000000 0\r\n"
            + "3 \"3\" 0.000000 0.000000 0\r\n"
            + "4 \"4\" 0.000000 0.000000 0\r\n"
            + "5 \"5\" 0.000000 0.000000 0\r\n"
            + "6 \"6\" 0.000000 0.000000 0\r\n"
            + "7 \"7\" 0.000000 0.000000 0\r\n"
            + "8 \"8\" 0.000000 0.000000 0\r\n"
            + "9 \"9\" 0.000000 0.000000 0\r\n"
            + "10 \"10\" 0.000000 0.000000 0\r\n"
            + "*arcs\r\n"
            + "4 5 1\r\n"
            + "3 4 5.11\r\n"
            + "2 3 2.01\r\n"
            + "1 2 1.23\r\n"
            ;

        Assert.AreEqual(ExpectedFileContents, sFileContents);
    }

    //*************************************************************************
    //  Method: TestSaveGraph6()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSaveGraph6()
    {
        // Unirected graph.

        const Int32 Vertices = 10;

        IGraph oGraph = CreateGraph(GraphDirectedness.Undirected);

        IVertexCollection oVertices = oGraph.Vertices;

        IEdgeCollection oEdges = oGraph.Edges;

        IVertex [] aoVertices = TestGraphUtil.AddVertices(oGraph, Vertices);

        for (Int32 i = 0; i < Vertices; i++)
        {
            aoVertices[i].Name = (i + 1).ToString();
        }

        aoVertices[0].Location = new PointF(0.12F, 0.34F);

        IEdge oEdge;

        oEdge = oEdges.Add(aoVertices[0], aoVertices[1], false);
        oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, 1.23);

        oEdge = oEdges.Add(aoVertices[1], aoVertices[2], false);
        oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, 2.01);

        oEdge = oEdges.Add(aoVertices[2], aoVertices[3], false);
        oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, 5.11);

        oEdges.Add(aoVertices[3], aoVertices[4], false);
        // (No weight, should be default.)

        m_oGraphAdapter.SaveGraph(oGraph, m_sTempFileName);

        String sFileContents;

        using ( StreamReader oStreamReader = new StreamReader(m_sTempFileName) )
        {
            sFileContents = oStreamReader.ReadToEnd();
        }

        const String ExpectedFileContents =

            "*vertices 10\r\n"
            + "1 \"1\" 1.000000 1.000000 0\r\n"
            + "2 \"2\" 0.000000 0.000000 0\r\n"
            + "3 \"3\" 0.000000 0.000000 0\r\n"
            + "4 \"4\" 0.000000 0.000000 0\r\n"
            + "5 \"5\" 0.000000 0.000000 0\r\n"
            + "6 \"6\" 0.000000 0.000000 0\r\n"
            + "7 \"7\" 0.000000 0.000000 0\r\n"
            + "8 \"8\" 0.000000 0.000000 0\r\n"
            + "9 \"9\" 0.000000 0.000000 0\r\n"
            + "10 \"10\" 0.000000 0.000000 0\r\n"
            + "*edges\r\n"
            + "4 5 1\r\n"
            + "3 4 5.11\r\n"
            + "2 3 2.01\r\n"
            + "1 2 1.23\r\n"
            ;

        Assert.AreEqual(ExpectedFileContents, sFileContents);
    }

    //*************************************************************************
    //  Method: TestSaveGraph7()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSaveGraph7()
    {
        // Vertex locations out of bounds.

        const Int32 Vertices = 10;

        IGraph oGraph = CreateGraph(GraphDirectedness.Mixed);

        IVertexCollection oVertices = oGraph.Vertices;

        IEdgeCollection oEdges = oGraph.Edges;

        IVertex [] aoVertices = TestGraphUtil.AddVertices(oGraph, Vertices);

        for (Int32 i = 0; i < Vertices; i++)
        {
            aoVertices[i].Name = (i + 1).ToString();
        }

        aoVertices[0].Location = new PointF(-10F, -20);
        aoVertices[1].Location = new PointF(30F, 30);

        IEdge oEdge;

        oEdge = oEdges.Add(aoVertices[0], aoVertices[1], false);
        oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, 1.23);

        oEdge = oEdges.Add(aoVertices[1], aoVertices[2], true);
        oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, 2.01);

        oEdge = oEdges.Add(aoVertices[2], aoVertices[3], false);
        oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, 5.11);

        oEdges.Add(aoVertices[3], aoVertices[4], true);
        // (No weight, should be default.)

        m_oGraphAdapter.SaveGraph(oGraph, m_sTempFileName);

        String sFileContents;

        using ( StreamReader oStreamReader = new StreamReader(m_sTempFileName) )
        {
            sFileContents = oStreamReader.ReadToEnd();
        }

        const String ExpectedFileContents =

            "*vertices 10\r\n"
            + "1 \"1\" 0.000000 0.000000 0\r\n"
            + "2 \"2\" 1.000000 1.000000 0\r\n"
            + "3 \"3\" 0.250000 0.400000 0\r\n"
            + "4 \"4\" 0.250000 0.400000 0\r\n"
            + "5 \"5\" 0.250000 0.400000 0\r\n"
            + "6 \"6\" 0.250000 0.400000 0\r\n"
            + "7 \"7\" 0.250000 0.400000 0\r\n"
            + "8 \"8\" 0.250000 0.400000 0\r\n"
            + "9 \"9\" 0.250000 0.400000 0\r\n"
            + "10 \"10\" 0.250000 0.400000 0\r\n"
            + "*edges\r\n"
            + "3 4 5.11\r\n"
            + "1 2 1.23\r\n"
            + "*arcs\r\n"
            + "4 5 1\r\n"
            + "2 3 2.01\r\n"
            ;

        Assert.AreEqual(ExpectedFileContents, sFileContents);
    }

    //*************************************************************************
    //  Method: TestSaveGraphBad()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestSaveGraphBad()
    {
        // null graph.

        try
        {
            m_oGraphAdapter.SaveGraph(null, "x");
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Adapters."
                + "PajekGraphAdapter.SaveGraph: graph argument can't be"
                + " null.\r\n"
                + "Parameter name: graph"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestSaveGraphBad2()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestSaveGraphBad2()
    {
        // null filename.

        String sFileName = null;

        try
        {
            m_oGraphAdapter.SaveGraph(CreateGraph(), sFileName);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Adapters."
                + "PajekGraphAdapter.SaveGraph: filename argument can't be"
                + " null.\r\n"
                + "Parameter name: filename"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestSaveGraphBad3()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestSaveGraphBad3()
    {
        // Empty filename.

        String sFileName = String.Empty;

        try
        {
            m_oGraphAdapter.SaveGraph(CreateGraph(), sFileName);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Adapters."
                + "PajekGraphAdapter.SaveGraph: filename argument must have a"
                + " length greater than zero.\r\n"
                + "Parameter name: filename"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestSaveGraph2_Bad()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestSaveGraph2_Bad()
    {
        // null graph.

        try
        {
            m_oGraphAdapter.SaveGraph( null, new MemoryStream() );
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Adapters."
                + "PajekGraphAdapter.SaveGraph: graph argument can't be"
                + " null.\r\n"
                + "Parameter name: graph"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestSaveGraph2_Bad2()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestSaveGraph2_Bad2()
    {
        // null stream.

        Stream oStream = null;

        try
        {
            m_oGraphAdapter.SaveGraph(CreateGraph(), oStream);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Adapters."
                + "PajekGraphAdapter.SaveGraph: stream argument can't be"
                + " null.\r\n"
                + "Parameter name: stream"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestSupportsDirectedness()
    //
    /// <summary>
    /// Tests the SupportsDirectedness() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSupportsDirectedness()
    {
        Assert.IsTrue( m_oGraphAdapter.SupportsDirectedness(
            GraphDirectedness.Undirected) );
    }

    //*************************************************************************
    //  Method: TestSupportsDirectedness2()
    //
    /// <summary>
    /// Tests the SupportsDirectedness() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSupportsDirectedness2()
    {
        Assert.IsTrue( m_oGraphAdapter.SupportsDirectedness(
            GraphDirectedness.Directed) );
    }

    //*************************************************************************
    //  Method: TestSupportsDirectedness3()
    //
    /// <summary>
    /// Tests the SupportsDirectedness() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSupportsDirectedness3()
    {
        Assert.IsTrue( m_oGraphAdapter.SupportsDirectedness(
            GraphDirectedness.Mixed) );
    }

    //*************************************************************************
    //  Method: CreateGraph()
    //
    /// <overloads>
    /// Creates a graph that is compatible with <see
    /// cref="PajekGraphAdapter" />.
    /// </overloads>
    ///
    /// <summary>
    /// Creates a directed graph that is compatible with <see
    /// cref="PajekGraphAdapter" />.
    /// </summary>
    ///
    /// <returns>
    /// A new compatible graph.
    /// </returns>
    //*************************************************************************

    protected IGraph
    CreateGraph()
    {
        return ( CreateGraph(GraphDirectedness.Directed) );
    }

    //*************************************************************************
    //  Method: CreateGraph()
    //
    /// <summary>
    /// Creates a graph of a specified directedness that is compatible with
    /// <see cref="PajekGraphAdapter" />.
    /// </summary>
    ///
    /// <param name="eDirectedness">
    /// Directedness of the new graph.
    /// </param>
    ///
    /// <returns>
    /// A new compatible graph.
    /// </returns>
    //*************************************************************************

    protected IGraph
    CreateGraph
    (
        GraphDirectedness eDirectedness
    )
    {
        IGraph oGraph = new Graph(eDirectedness);

        return (oGraph);
    }

    //*************************************************************************
    //  Method: WriteFile()
    //
    /// <summary>
    /// Writes file contents to a temporary file.
    /// </summary>
    ///
    /// <param name="sFileContents">
    /// Contents to write to the file.
    /// </param>
    //*************************************************************************

    protected void
    WriteFile
    (
        String sFileContents
    )
    {
        using ( StreamWriter oStreamWriter = new StreamWriter(m_sTempFileName) )
        {
            oStreamWriter.Write(sFileContents);
        }
    }

    //*************************************************************************
    //  Method: FindEdge()
    //
    /// <overloads>
    /// Checks whether a graph contains a specified edge.
    /// </overloads>
    ///
    /// <summary>
    /// Checks whether a graph contains an edge specified with vertex indices,
    /// without a weight.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// Graph to check.
    /// </param>
    ///
    /// <param name="iOneBasedVertex1Index">
    /// One-based index of the edge's first vertex.
    /// </param>
    ///
    /// <param name="iOneBasedVertex2Index">
    /// One-based index of the edge's second vertex.
    /// </param>
    ///
    /// <param name="bIsDirected">
    /// true if the edge should be directed.
    /// </param>
    //*************************************************************************

    protected void
    FindEdge
    (
        IGraph oGraph,
        Int32 iOneBasedVertex1Index,
        Int32 iOneBasedVertex2Index,
        Boolean bIsDirected
    )
    {
        FindEdge(oGraph, iOneBasedVertex1Index, iOneBasedVertex2Index,
            bIsDirected, -1);
    }

    //*************************************************************************
    //  Method: FindEdge()
    //
    /// <summary>
    /// Checks whether a graph contains an edge specified with vertex indices,
    /// with an optional weight.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// Graph to check.
    /// </param>
    ///
    /// <param name="iOneBasedVertex1Index">
    /// One-based index of the edge's first vertex.
    /// </param>
    ///
    /// <param name="iOneBasedVertex2Index">
    /// One-based index of the edge's second vertex.
    /// </param>
    ///
    /// <param name="bIsDirected">
    /// true if the edge should be directed.
    /// </param>
    ///
    /// <param name="dWeight">
    /// Expected edge weight, or -1 if no weight is expected.
    /// </param>
    //*************************************************************************

    protected void
    FindEdge
    (
        IGraph oGraph,
        Int32 iOneBasedVertex1Index,
        Int32 iOneBasedVertex2Index,
        Boolean bIsDirected,
        Double dWeight
    )
    {
        String sVertex1Name = GetVertexName(iOneBasedVertex1Index);
        String sVertex2Name = GetVertexName(iOneBasedVertex2Index);

        FindEdge(oGraph, sVertex1Name, sVertex2Name, bIsDirected, dWeight);
    }

    //*************************************************************************
    //  Method: FindEdge()
    //
    /// <summary>
    /// Checks whether a graph contains an edge specified with vertex names,
    /// with an optional weight.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// Graph to check.
    /// </param>
    ///
    /// <param name="sVertex1Name">
    /// Name of the edge's first vertex.
    /// </param>
    ///
    /// <param name="sVertex2Name">
    /// Name of the edge's second vertex.
    /// </param>
    ///
    /// <param name="bIsDirected">
    /// true if the edge should be directed.
    /// </param>
    ///
    /// <param name="dWeight">
    /// Expected edge weight, or -1 if no weight is expected.
    /// </param>
    //*************************************************************************

    protected void
    FindEdge
    (
        IGraph oGraph,
        String sVertex1Name,
        String sVertex2Name,
        Boolean bIsDirected,
        Double dWeight
    )
    {
        const String ClassName = "PajekGraphAdapterTest";
        const String MethodName = "FindEdge";

        foreach (IEdge oEdge in oGraph.Edges)
        {
            IVertex oVertex1, oVertex2;

            EdgeUtil.EdgeToVertices(
                oEdge, ClassName, MethodName, out oVertex1, out oVertex2);

            if (oVertex1.Name != sVertex1Name)
            {
                continue;
            }

            if (oVertex2.Name != sVertex2Name)
            {
                continue;
            }

            if (oEdge.IsDirected != bIsDirected)
            {
                continue;
            }

            Object oWeight;

            Boolean bHasWeight = oEdge.TryGetValue(
                ReservedMetadataKeys.EdgeWeight, out oWeight);

            if (dWeight == -1)
            {
                if (bHasWeight)
                {
                    continue;
                }
            }
            else
            {
                if (!bHasWeight)
                {
                    continue;
                }
            }

            if ( (Double)oWeight != dWeight )
            {
                continue;
            }

            if ( oEdge.ContainsKey(EdgeFoundKey) )
            {
                // The edge was found in a previous call to this method.

                continue;
            }

            // Don't let the edge be found again.

            oEdge.SetValue(EdgeFoundKey, null);

            return;
        }

        // The specified edge wasn't found.

        Assert.Fail();
    }

    //*************************************************************************
    //  Method: GetVertexName()
    //
    /// <summary>
    /// Returns the name of a vertex.
    /// </summary>
    ///
    /// <param name="iOneBasedVertexIndex">
    /// One-based index of the vertex.
    /// </param>
    ///
    /// <returns>
    /// Vertex name.
    /// </returns>
    //*************************************************************************

    protected String
    GetVertexName
    (
        Int32 iOneBasedVertexIndex
    )
    {
        return ( "Vertex " + iOneBasedVertexIndex.ToString() );
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    // Key added to an edge to mark it as having been found by FindEdge().

    protected const String EdgeFoundKey = "EdgeFound";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected IGraphAdapter m_oGraphAdapter;

    /// Name of the temporary file that may be created by the unit tests.

    protected String m_sTempFileName;
}

}
