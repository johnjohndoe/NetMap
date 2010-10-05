
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Common;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: GraphTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="Graph" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class GraphTest : Object
{
    //*************************************************************************
    //  Constructor: GraphTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphTest" /> class.
    /// </summary>
    //*************************************************************************

    public GraphTest()
    {
        m_oGraph = null;
        m_bEdgeAdded = false;
        m_oAddedEdge = null;
        m_bVertexAdded = false;
        m_oAddedVertex = null;
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
        InitializeGraph(GraphDirectedness.Mixed);
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
        m_bEdgeAdded = false;
        m_oAddedEdge = null;
        m_bVertexAdded = false;
        m_oAddedVertex = null;
    }

    //*************************************************************************
    //  Method: TestConstructor()
    //
    /// <summary>
    /// Tests the Graph() constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestConstructor()
    {
        m_oGraph = new Graph();

        Assert.AreEqual(GraphDirectedness.Mixed, m_oGraph.Directedness);

        Assert.IsNotNull(m_oGraph.Edges);
        Assert.AreEqual(0, m_oGraph.Edges.Count);
        Assert.IsNull(m_oGraph.Name);
        Assert.IsNull(m_oGraph.Tag);
        Assert.IsNotNull(m_oGraph.Vertices);
        Assert.AreEqual(0, m_oGraph.Vertices.Count);
    }

    //*************************************************************************
    //  Method: TestConstructor2()
    //
    /// <summary>
    /// Tests the Graph(Directedness) constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestConstructor2()
    {
        m_oGraph = new Graph(GraphDirectedness.Undirected);

        Assert.AreEqual(GraphDirectedness.Undirected, m_oGraph.Directedness);

        Assert.IsNotNull(m_oGraph.Edges);
        Assert.AreEqual(0, m_oGraph.Edges.Count);
        Assert.IsNull(m_oGraph.Name);
        Assert.IsNull(m_oGraph.Tag);
        Assert.IsNotNull(m_oGraph.Vertices);
        Assert.AreEqual(0, m_oGraph.Vertices.Count);
    }

    //*************************************************************************
    //  Method: TestConstructorBad()
    //
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestConstructorBad()
    {
        // Invalid directedness.

        try
        {
            m_oGraph = new Graph( (GraphDirectedness)(-1) );
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "Graph.Constructor: Must be a member of the"
                + " GraphDirectedness enumeration.\r\n"
                + "Parameter name: directedness"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
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
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestConstructorBad2()
    {
        // Invalid directedness.

        try
        {
            m_oGraph = new Graph( (GraphDirectedness)(-1) );
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "Graph.Constructor: Must be a member of the"
                + " GraphDirectedness enumeration.\r\n"
                + "Parameter name: directedness"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }
 
    //*************************************************************************
    //  Method: TestDirectedness()
    //
    /// <summary>
    /// Tests the Directedness property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestDirectedness()
    {
        foreach (GraphDirectedness eDirectedness in
            TestGraphUtil.AllGraphDirectedness)
        {
            InitializeGraph(eDirectedness);

            Assert.AreEqual(eDirectedness, m_oGraph.Directedness);
        }
    }

    //*************************************************************************
    //  Method: TestVertices()
    //
    /// <summary>
    /// Tests the Vertices property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestVertices()
    {
        Assert.IsNotNull(m_oGraph.Vertices);
        Assert.IsInstanceOfType(m_oGraph.Vertices, typeof(VertexCollection));
        Assert.AreEqual(0, m_oGraph.Vertices.Count);

        const Int32 Vertices = 1000;

        IVertex [] aoVertices = TestGraphUtil.AddVertices(m_oGraph, Vertices);

        Assert.AreEqual(Vertices, m_oGraph.Vertices.Count);

        foreach (IVertex oVertex in aoVertices)
        {
            Assert.IsTrue( m_oGraph.Vertices.Contains(oVertex) );
        }
    }

    //*************************************************************************
    //  Method: TestEdges()
    //
    /// <summary>
    /// Tests the Edges property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestEdges()
    {
        Assert.IsNotNull(m_oGraph.Edges);
        Assert.IsInstanceOfType(m_oGraph.Edges, typeof(EdgeCollection));
        Assert.AreEqual(0, m_oGraph.Edges.Count);

        const Int32 Vertices = 100;

        IVertex [] aoVertices = TestGraphUtil.AddVertices(m_oGraph, Vertices);

        IEdge [] aoEdges =
            TestGraphUtil.MakeGraphComplete(m_oGraph, aoVertices, false);

        Assert.AreEqual(aoEdges.Length, m_oGraph.Edges.Count);

        foreach (IEdge oEdge in aoEdges)
        {
            Assert.IsTrue( m_oGraph.Edges.Contains(oEdge) );
        }
    }

    //*************************************************************************
    //  Method: TestVertexAdded()
    //
    /// <summary>
    /// Tests the VertexAdded event.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestVertexAdded()
    {
        const Int32 Vertices = 2000;

        for (Int32 i = 0; i < Vertices; i++)
        {
            IVertex oVertex = new Vertex();

            m_bVertexAdded = false;

            m_oAddedVertex = null;

            m_oGraph.Vertices.Add(oVertex);

            Assert.IsTrue(m_bVertexAdded);

            Assert.AreEqual(oVertex, m_oAddedVertex);
        }
    }

    //*************************************************************************
    //  Method: TestEdgeAdded()
    //
    /// <summary>
    /// Tests the EdgeAdded event.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestEdgeAdded()
    {
        const Int32 Vertices = 2000;

        IVertex [] aoVertices = TestGraphUtil.AddVertices(m_oGraph, Vertices);

        for (Int32 i = 1; i < Vertices; i++)
        {
            IEdge oEdge = new Edge(aoVertices[0], aoVertices[i], false);

            m_bEdgeAdded = false;

            m_oAddedEdge = null;

            m_oGraph.Edges.Add(oEdge);

            Assert.IsTrue(m_bEdgeAdded);

            Assert.AreEqual(oEdge, m_oAddedEdge);
        }
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
        // Loop through multiple scenarios.

        Int32 [] aiVertices = new Int32 [] {0, 6};

        foreach (Boolean bCopyMetadataValues in TestGraphUtil.AllBoolean)

        foreach (Boolean bCopyTag in TestGraphUtil.AllBoolean)

        foreach (GraphDirectedness eDirectedness in
            TestGraphUtil.AllGraphDirectedness)

        foreach (Int32 iVertices in aiVertices)

        foreach (Boolean bAddEdges in TestGraphUtil.AllBoolean)
        {
            const String Name = "fdjkerwuio";

            // Prepare the graph to be cloned.

            InitializeGraph(eDirectedness);

            m_oGraph.Name = Name;

            MetadataUtil.SetRandomMetadata(m_oGraph, true, true, m_oGraph.ID);

            // Add the vertices and set metadata on them.  For the seed, use
            // the vertex ID.

            IVertex [] aoVertices =
                TestGraphUtil.AddVertices(m_oGraph, iVertices);

            for (Int32 i = 0; i < iVertices; i++)
            {
                IVertex oVertex = aoVertices[i];

                MetadataUtil.SetRandomMetadata(
                    oVertex, true, true, oVertex.ID);
            }

            if (bAddEdges)
            {
                // Add the edges and set metadata on them.  For the seed, use
                // the edge ID.

                IEdge [] aoEdges = TestGraphUtil.MakeGraphComplete(
                    m_oGraph, aoVertices,
                    (eDirectedness == GraphDirectedness.Directed)
                    );

                foreach (IEdge oEdge in m_oGraph.Edges)
                {
                    MetadataUtil.SetRandomMetadata(
                        oEdge, true, true, oEdge.ID);
                }
            }

            // Clone the graph.

            IGraph oNewGraph = m_oGraph.Clone(bCopyMetadataValues, bCopyTag);

            // Check the metadata on the new graph.

            MetadataUtil.CheckRandomMetadata(
                oNewGraph, bCopyMetadataValues, bCopyTag, m_oGraph.ID);

            // Check the vertices on the new graph.

            IVertexCollection oNewVertexCollection = oNewGraph.Vertices;

            Assert.IsNotNull(oNewVertexCollection);

            Assert.AreEqual(iVertices, oNewVertexCollection.Count);

            // Loop through the original vertices.

            foreach (IVertex oVertex in m_oGraph.Vertices)
            {
                // Find the corresponding new vertex, by name.

                IVertex oNewVertex;

                Assert.IsTrue( oNewVertexCollection.Find(
                    oVertex.Name, out oNewVertex) );

                Assert.AreNotEqual(oVertex.ID, oNewVertex.ID);

                // Check the vertex's metadata.  Use the original vertex ID as
                // a seed.

                MetadataUtil.CheckRandomMetadata(oNewVertex,
                    bCopyMetadataValues, bCopyTag, oVertex.ID);
            }

            // Check the edges on the new graph.  

            IEdgeCollection oNewEdgeCollection = oNewGraph.Edges;

            Assert.IsNotNull(oNewEdgeCollection);

            Assert.AreEqual(

                bAddEdges ?
                TestGraphUtil.GetEdgeCountForCompleteGraph(iVertices) : 0,

                oNewEdgeCollection.Count
                );

            // Loop through the original edges.

            foreach (IEdge oEdge in m_oGraph.Edges)
            {
                // Find the corresponding new edge, by name.

                IEdge oNewEdge;

                Assert.IsTrue( oNewEdgeCollection.Find(
                    oEdge.Name, out oNewEdge) );

                Assert.AreNotEqual(oEdge.ID, oNewEdge.ID);

                // Check the edge's metadata.  Use the original edge ID as a
                // seed.

                MetadataUtil.CheckRandomMetadata(oNewEdge,
                    bCopyMetadataValues, bCopyTag, oEdge.ID);

                // Verify that the new edge and original edge connect vertices
                // that correspond. 

                Assert.IsNotNull( oNewEdge.Vertices[0] );
                Assert.IsNotNull( oNewEdge.Vertices[1] );

                Assert.AreEqual(oNewEdge.Vertices[0].Name,
                    oEdge.Vertices[0].Name);

                Assert.AreEqual(oNewEdge.Vertices[1].Name,
                    oEdge.Vertices[1].Name);
            }

            // Check the other properties on the new graph.

            Assert.AreEqual(Name, oNewGraph.Name);

            Assert.AreEqual(eDirectedness, oNewGraph.Directedness);

            Assert.AreNotEqual(m_oGraph.ID, oNewGraph.ID);
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

        Assert.AreEqual( "ID=" + m_oGraph.ID.ToString(NodeXLBase.Int32Format),
            m_oGraph.ToString() );
    }

    //*************************************************************************
    //  Method: TestLargeGraph()
    //
    /// <summary>
    /// Tests a large graph.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLargeGraph()
    {
        // Create a large graph, make sure there are no exceptions or
        // assertions.

        const Int32 Vertices = 1000000;

        const Int32 Edges = 100000;

        IVertex [] aoVertices = TestGraphUtil.AddVertices(m_oGraph, Vertices);

        Random oRandom = new Random(1);

        for (Int32 i = 0; i < Edges; i++)
        {
            IVertex oVertex1 = aoVertices[ oRandom.Next(Vertices) ];
            IVertex oVertex2 = aoVertices[ oRandom.Next(Vertices) ];

            IEdgeCollection oEdgeCollection = m_oGraph.Edges;

            oEdgeCollection.Add(oVertex1, oVertex2);
        }

        Assert.AreEqual(Vertices, m_oGraph.Vertices.Count);
        Assert.AreEqual(Edges, m_oGraph.Edges.Count);

        m_oGraph.Edges.Clear();

        Assert.AreEqual(Vertices, m_oGraph.Vertices.Count);
        Assert.AreEqual(0, m_oGraph.Edges.Count);

        m_oGraph.Vertices.Clear();

        Assert.AreEqual(0, m_oGraph.Vertices.Count);
        Assert.AreEqual(0, m_oGraph.Edges.Count);
    }

    //*************************************************************************
    //  Method: TestRandomGraphs()
    //
    /// <summary>
    /// Tests a set of random graphs.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRandomGraphs()
    {
        // Large number of small graphs.

        TestRandomGraphs(10000, 50);
    }

    //*************************************************************************
    //  Method: TestRandomGraphs2()
    //
    /// <summary>
    /// Tests a set of random graphs.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRandomGraphs2()
    {
        // Small number of large graphs.

        TestRandomGraphs(5, 100000);
    }

    //*************************************************************************
    //  Method: InitializeGraph()
    //
    /// <summary>
    /// Initializes m_oGraph and related member fields.
    /// </summary>
    ///
    /// <param name="eDirectedness">
    /// Directedness of m_oGraph.
    /// </param>
    //*************************************************************************

    protected void
    InitializeGraph
    (
        GraphDirectedness eDirectedness
    )
    {
        m_oGraph = new Graph(eDirectedness);

        m_oGraph.Edges.EdgeAdded += new EdgeEventHandler(
            this.EdgeCollection_EdgeAdded);

        m_bEdgeAdded = false;

        m_oAddedEdge = null;

        m_oGraph.Vertices.VertexAdded += new VertexEventHandler(
            this.VertexCollection_VertexAdded);

        m_bVertexAdded = false;

        m_oAddedVertex = null;
    }

    //*************************************************************************
    //  Method: TestRandomGraphs()
    //
    /// <summary>
    /// Tests a set of random graphs.
    /// </summary>
    ///
    /// <param name="iGraphs">
    /// Number of random graphs to create.
    /// </param>
    ///
    /// <param name="iMaximumVertices">
    /// Maximum number of vertices to add to each graph.
    /// </param>
    //*************************************************************************

    protected void
    TestRandomGraphs
    (
        Int32 iGraphs,
        Int32 iMaximumVertices
    )
    {
        Debug.Assert(iGraphs > 0);
        Debug.Assert(iMaximumVertices >= 0);

        Random oRandom = new Random(1);

        for (Int32 iGraph = 0; iGraph < iGraphs; iGraph++)
        {
            Int32 iVertices = oRandom.Next(iMaximumVertices + 1);

            TestRandomGraph(iVertices, oRandom);
        }
    }

    //*************************************************************************
    //  Method: TestRandomGraph()
    //
    /// <summary>
    /// Tests a random graph.
    /// </summary>
    ///
    /// <param name="iVertices">
    /// Number of vertices to add to the graph.
    /// </param>
    ///
    /// <param name="oRandom">
    /// Random number generator to use.
    /// </param>
    //*************************************************************************

    protected void
    TestRandomGraph
    (
        Int32 iVertices,
        Random oRandom
    )
    {
        Debug.Assert(iVertices >= 0);
        Debug.Assert(oRandom != null);

        // Stores the edges actually added to the graph.

        List<IEdge> oActualEdges = new List<IEdge>();

        // Create a graph with random directedness.

        GraphDirectedness eDirectedness = TestGraphUtil.AllGraphDirectedness[
            oRandom.Next(TestGraphUtil.AllGraphDirectedness.Length) ];

        InitializeGraph(eDirectedness);

        // Add random vertices.

        IVertex [] aoVertices = TestGraphUtil.AddVertices(m_oGraph, iVertices);

        Assert.AreEqual(iVertices, m_oGraph.Vertices.Count);
        Assert.AreEqual(0, m_oGraph.Edges.Count);

        // Add random edges.

        Int32 iAttemptedEdges = oRandom.Next(iVertices);

        IEdgeCollection oEdgeCollection = m_oGraph.Edges;

        for (Int32 i = 0; i < iAttemptedEdges; i++)
        {
            Boolean bIsDirected = false;

            switch (eDirectedness)
            {
                case GraphDirectedness.Undirected:

                    bIsDirected = false;
                    break;

                case GraphDirectedness.Directed:

                    bIsDirected = true;
                    break;

                case GraphDirectedness.Mixed:

                    bIsDirected = (oRandom.Next(2) % 2 == 0);
                    break;

                default:

                    Debug.Assert(false);
                    break;
            }

            IVertex oVertex1 = aoVertices[ oRandom.Next(iVertices) ];
            IVertex oVertex2 = aoVertices[ oRandom.Next(iVertices) ];
            IEdge oEdge = oEdgeCollection.Add(oVertex1, oVertex2, bIsDirected);
            oActualEdges.Add(oEdge);
        }

        Assert.AreEqual(iVertices, m_oGraph.Vertices.Count);
        Assert.AreEqual(oActualEdges.Count, m_oGraph.Edges.Count);

        // Set random metadata.

        foreach (IVertex oVertex in m_oGraph.Vertices)
        {
            String sName = null;

            if (oRandom.Next(3) % 3 == 0)
            {
                MetadataUtil.SetRandomMetadata(
                    oVertex, true, true, oVertex.ID);

                // Mark the vertex as having metadata.

                sName = MetadataMarker;
            }

            oVertex.Name = sName;
        }

        foreach (IEdge oEdge in m_oGraph.Edges)
        {
            String sName = null;

            if (oRandom.Next(4) % 4 == 0)
            {
                MetadataUtil.SetRandomMetadata(oEdge, true, true, oEdge.ID);

                sName = MetadataMarker;
            }

            oEdge.Name = sName;
        }

        MetadataUtil.SetRandomMetadata(m_oGraph, true, true, m_oGraph.ID);

        // Check the random metadata.

        CheckRandomMetadataOnRandomGraph();

        // Remove random edges.

        Int32 iRemovedEdges = 0;

        if (oRandom.Next(2) % 2 == 0)
        {
            foreach (IEdge oEdge in oActualEdges)
            {
                if (oRandom.Next(5) % 5 == 0)
                {
                    m_oGraph.Edges.Remove(oEdge);

                    iRemovedEdges++;
                }
            }
        }

        Assert.AreEqual(iVertices, m_oGraph.Vertices.Count);

        Assert.AreEqual(oActualEdges.Count - iRemovedEdges,
            m_oGraph.Edges.Count);

        // Remove random vertices.

        Int32 iRemovedVertices = 0;

        if (oRandom.Next(2) % 2 == 0)
        {
            foreach (IVertex oVertex in aoVertices)
            {
                if (oRandom.Next(3) % 3 == 0)
                {
                    m_oGraph.Vertices.Remove(oVertex);

                    iRemovedVertices++;
                }
            }
        }

        Assert.AreEqual(iVertices - iRemovedVertices, m_oGraph.Vertices.Count);

        // Note: Can't test m_oGraph.Edges.Count here, because removing
        // vertices probably removed some edges as well.

        // Check the random metadata on the remaining objects.

        CheckRandomMetadataOnRandomGraph();

        // Check all the vertices, including the ones that were removed.
        // First, store all the non-removed vertex IDs in a dictionary to avoid
        // having to repeatedly call Vertices.Contains(), which is slow.

        Dictionary<Int32, Byte> oContainedVertexIDs =
            new Dictionary<Int32, Byte>();

        foreach (IVertex oVertex in m_oGraph.Vertices)
        {
            oContainedVertexIDs.Add(oVertex.ID, 0);
        }

        foreach (IVertex oVertex in aoVertices)
        {
            Boolean bContainedInGraph =
                oContainedVertexIDs.ContainsKey(oVertex.ID);

            Assert.AreEqual(bContainedInGraph, oVertex.ParentGraph != null);

            if (oVertex.Name == MetadataMarker)
            {
                MetadataUtil.CheckRandomMetadata(
                    oVertex, true, true, oVertex.ID);
            }
            else
            {
                Assert.IsNull(oVertex.Tag);
            }
        }

        oContainedVertexIDs.Clear();

        // Remove all edges.

        m_oGraph.Edges.Clear();

        Assert.AreEqual(iVertices - iRemovedVertices,
            m_oGraph.Vertices.Count);

        Assert.AreEqual(0, m_oGraph.Edges.Count);

        // Remove all vertices.

        m_oGraph.Vertices.Clear();

        Assert.AreEqual(0, m_oGraph.Vertices.Count);

        Assert.AreEqual(0, m_oGraph.Edges.Count);

        // Check all the vertices.

        foreach (IVertex oVertex in aoVertices)
        {
            Boolean bContainedInGraph = m_oGraph.Vertices.Contains(oVertex);

            Assert.IsFalse(bContainedInGraph);

            if (oVertex.Name == MetadataMarker)
            {
                MetadataUtil.CheckRandomMetadata(
                    oVertex, true, true, oVertex.ID);
            }
            else
            {
                Assert.IsNull(oVertex.Tag);
            }
        }
    }

    //*************************************************************************
    //  Method: CheckRandomMetadataOnRandomGraph()
    //
    /// <summary>
    /// Tests the random metadata set by <see cref="TestRandomGraph" />.
    /// </summary>
    //*************************************************************************

    protected void
    CheckRandomMetadataOnRandomGraph()
    {
        foreach (IVertex oVertex in m_oGraph.Vertices)
        {
            if (oVertex.Name == MetadataMarker)
            {
                MetadataUtil.CheckRandomMetadata(
                    oVertex, true, true, oVertex.ID);
            }
            else
            {
                Assert.IsNull(oVertex.Tag);
            }
        }

        foreach (IEdge oEdge in m_oGraph.Edges)
        {
            if (oEdge.Name == MetadataMarker)
            {
                MetadataUtil.CheckRandomMetadata(
                    oEdge, true, true, oEdge.ID);
            }
            else
            {
                Assert.IsNull(oEdge.Tag);
            }
        }

        MetadataUtil.CheckRandomMetadata(
            m_oGraph, true, true, m_oGraph.ID);
    }

    //*************************************************************************
    //  Method: EdgeCollection_EdgeAdded()
    //
    /// <summary>
    /// Handles the EdgeAdded event on the EdgeCollection object.
    /// </summary>
    ///
    /// <param name="oSender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="oEdgeEventArgs">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    EdgeCollection_EdgeAdded
    (
        Object oSender,
        EdgeEventArgs oEdgeEventArgs
    )
    {
        if ( oSender == null || !(oSender is IEdgeCollection) )
        {
            throw new ApplicationException(
                "EdgeAdded event provided incorrect oSender argument."
                );
        }

        m_bEdgeAdded = true;

        m_oAddedEdge = oEdgeEventArgs.Edge;
    }

    //*************************************************************************
    //  Method: VertexCollection_VertexAdded()
    //
    /// <summary>
    /// Handles the VertexAdded event on the VertexCollection object.
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
        if ( oSender == null || !(oSender is IVertexCollection) )
        {
            throw new ApplicationException(
                "VertexAdded event provided incorrect oSender argument."
                );
        }

        m_bVertexAdded = true;

        m_oAddedVertex = oVertexEventArgs.Vertex;
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// TestRandomGraph() sets the name of an object to this constant if it
    /// sets random metadata on the object.

    protected const String MetadataMarker = "HasMetadata";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected IGraph m_oGraph;

    /// Gets set by EdgeCollection_EdgeAdded().

    protected Boolean m_bEdgeAdded;

    /// Gets set by EdgeCollection_EdgeAdded().

    protected IEdge m_oAddedEdge;

    /// Gets set by VertexCollection_VertexAdded().

    protected Boolean m_bVertexAdded;

    /// Gets set by VertexCollection_VertexAdded().

    protected IVertex m_oAddedVertex;
}

}
