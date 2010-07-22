
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Common;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: EdgeCollectionTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="EdgeCollection" />
/// class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class EdgeCollectionTest : Object
{
    //*************************************************************************
    //  Constructor: EdgeCollectionTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EdgeCollectionTest" />
    /// class.
    /// </summary>
    //*************************************************************************

    public EdgeCollectionTest()
    {
        m_oEdgeCollection = null;
        m_oGraph = null;

        m_bEdgeAdded = false;
        m_oAddedEdge = null;

        m_bEdgeRemoved = false;
        m_oRemovedEdge = null;
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
        m_oEdgeCollection = null;
        m_oGraph = null;

        m_bEdgeAdded = false;
        m_oAddedEdge = null;

        m_bEdgeRemoved = false;
        m_oRemovedEdge = null;
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
        Assert.AreEqual(0, m_oEdgeCollection.Count);
    }

    //*************************************************************************
    //  Method: TestAdd()
    //
    /// <summary>
    /// Tests the Add(IEdge) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdd()
    {
        // Add undirected edges to mixed graph.

        AddEdges(100, GraphDirectedness.Undirected, AddOverload.IEdge);
    }

    //*************************************************************************
    //  Method: TestAdd2()
    //
    /// <summary>
    /// Tests the Add(IEdge) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdd2()
    {
        // Add directed edges to mixed graph.

        AddEdges(100, GraphDirectedness.Directed, AddOverload.IEdge);
    }

    //*************************************************************************
    //  Method: TestAdd3()
    //
    /// <summary>
    /// Tests the Add(IEdge) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdd3()
    {
        // Add mixed edges to mixed graph.

        AddEdges(100, GraphDirectedness.Mixed, AddOverload.IEdge);
    }

    //*************************************************************************
    //  Method: TestAdd4()
    //
    /// <summary>
    /// Tests the Add(IEdge) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdd4()
    {
        // Add undirected edges to undirected graph.

        InitializeGraph(GraphDirectedness.Undirected);

        AddEdges(100, GraphDirectedness.Undirected, AddOverload.IEdge);
    }

    //*************************************************************************
    //  Method: TestAdd5()
    //
    /// <summary>
    /// Tests the Add(IEdge) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdd5()
    {
        // Add directed edges to directed graph.

        InitializeGraph(GraphDirectedness.Directed);

        AddEdges(100, GraphDirectedness.Directed, AddOverload.IEdge);
    }

    //*************************************************************************
    //  Method: TestAdd6()
    //
    /// <summary>
    /// Tests the Add(IEdge) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdd6()
    {
        // Add parallel edge to graph that allows it.

        InitializeGraph(GraphDirectedness.Mixed);

        IVertex [] aoVertices = AddVertices(2);

        IEdge oEdge = new Edge(aoVertices[0], aoVertices[1], false);

        Int32 iFirstEdgeID = oEdge.ID;

        m_oEdgeCollection.Add(oEdge);

        oEdge = new Edge(aoVertices[0], aoVertices[1], false);

        Int32 iSecondEdgeID = oEdge.ID;

        m_oEdgeCollection.Add(oEdge);

        Assert.IsTrue( m_oEdgeCollection.Contains(iFirstEdgeID) );
        Assert.IsTrue( m_oEdgeCollection.Contains(iSecondEdgeID) );
    }

    //*************************************************************************
    //  Method: TestAdd7()
    //
    /// <summary>
    /// Tests the Add(IEdge) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdd7()
    {
        // Add anit-parallel edge to graph that prohibits parallel edges.

        InitializeGraph(
            GraphDirectedness.Directed, GraphRestrictions.NoParallelEdges);

        IVertex [] aoVertices = AddVertices(2);

        IEdge oEdge = new Edge(aoVertices[0], aoVertices[1], true);

        Int32 iFirstEdgeID = oEdge.ID;

        m_oEdgeCollection.Add(oEdge);

        oEdge = new Edge(aoVertices[1], aoVertices[0], true);

        Int32 iSecondEdgeID = oEdge.ID;

        m_oEdgeCollection.Add(oEdge);

        Assert.IsTrue( m_oEdgeCollection.Contains(iFirstEdgeID) );
        Assert.IsTrue( m_oEdgeCollection.Contains(iSecondEdgeID) );
    }

    //*************************************************************************
    //  Method: TestAddBad()
    //
    /// <summary>
    /// Tests the Add(IEdge) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestAddBad()
    {
        // Null edge.

        IEdge oEdge = null;

        try
        {
            m_oEdgeCollection.Add(oEdge);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: edge argument can't be null.\r\n"
                + "Parameter name: edge"
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
    /// Tests the Add(IEdge) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestAddBad2()
    {
        // Null Edge.Vertices.

        try
        {
            IEdge oEdge = new MockEdge(null, null, false, true, 0);

            m_oEdgeCollection.Add(oEdge);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(
                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: The edge is invalid.\r\n"
                + "Parameter name: edge"
                ,
                oArgumentException.Message
                );

            Assert.IsNotNull(oArgumentException.InnerException);

            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: The edge's Vertices property is null."
                ,
                oArgumentException.InnerException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestAddBad3()
    //
    /// <summary>
    /// Tests the Add(IEdge) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestAddBad3()
    {
        // Edge.Vertices contains 0 vertices.

        try
        {
            IEdge oEdge = new MockEdge(null, null, false, false, 0);

            m_oEdgeCollection.Add(oEdge);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(
                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: The edge is invalid.\r\n"
                + "Parameter name: edge"
                ,
                oArgumentException.Message
                );

            Assert.IsNotNull(oArgumentException.InnerException);

            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: The edge does not connect two vertices."
                ,
                oArgumentException.InnerException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestAddBad4()
    //
    /// <summary>
    /// Tests the Add(IEdge) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestAddBad4()
    {
        // Edge.Vertices contains 1 vertex only.

        try
        {
            IEdge oEdge = new MockEdge(null, null, false, false, 1);

            m_oEdgeCollection.Add(oEdge);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(
                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: The edge is invalid.\r\n"
                + "Parameter name: edge"
                ,
                oArgumentException.Message
                );

            Assert.IsNotNull(oArgumentException.InnerException);

            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: The edge does not connect two vertices."
                ,
                oArgumentException.InnerException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestAddBad5()
    //
    /// <summary>
    /// Tests the Add(IEdge) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestAddBad5()
    {
        // First vertex is null.

        try
        {
            IVertex [] aoVertices = AddVertices(1);

            IEdge oEdge = new MockEdge(null, aoVertices[0], false, false, 2);

            m_oEdgeCollection.Add(oEdge);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(
                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: The edge is invalid.\r\n"
                + "Parameter name: edge"
                ,
                oArgumentException.Message
                );

            Assert.IsNotNull(oArgumentException.InnerException);

            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: The edge's first vertex is null."
                ,
                oArgumentException.InnerException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestAddBad6()
    //
    /// <summary>
    /// Tests the Add(IEdge) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestAddBad6()
    {
        // Second vertex is null.

        try
        {
            IVertex [] aoVertices = AddVertices(1);

            IEdge oEdge = new MockEdge(aoVertices[0], null, false, false, 2);

            m_oEdgeCollection.Add(oEdge);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(
                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: The edge is invalid.\r\n"
                + "Parameter name: edge"
                ,
                oArgumentException.Message
                );

            Assert.IsNotNull(oArgumentException.InnerException);

            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: The edge's second vertex is null."
                ,
                oArgumentException.InnerException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestAddBad7()
    //
    /// <summary>
    /// Tests the Add(IEdge) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestAddBad7()
    {
        // First vertex not in graph.

        try
        {
            IVertex [] aoVertices = AddVertices(1);

            IVertex oNonContainedVertex = new Vertex();

            IEdge oEdge = new MockEdge(
                oNonContainedVertex, aoVertices[0], false, false, 2);

            m_oEdgeCollection.Add(oEdge);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(
                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: The edge is invalid.\r\n"
                + "Parameter name: edge"
                ,
                oArgumentException.Message
                );

            Assert.IsNotNull(oArgumentException.InnerException);

            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: The edge's first vertex does not belong"
                + " to a graph."
                ,
                oArgumentException.InnerException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestAddBad8()
    //
    /// <summary>
    /// Tests the Add(IEdge) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestAddBad8()
    {
        // Second vertex not in graph.

        try
        {
            IVertex [] aoVertices = AddVertices(1);

            IVertex oNonContainedVertex = new Vertex();

            IEdge oEdge = new MockEdge(
                aoVertices[0], oNonContainedVertex, false, false, 2);

            m_oEdgeCollection.Add(oEdge);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(
                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: The edge is invalid.\r\n"
                + "Parameter name: edge"
                ,
                oArgumentException.Message
                );

            Assert.IsNotNull(oArgumentException.InnerException);

            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: The edge's second vertex does not"
                + " belong to a graph."
                ,
                oArgumentException.InnerException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestAddBad9()
    //
    /// <summary>
    /// Tests the Add(IEdge) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestAddBad9()
    {
        // First vertex is of wrong type.

        try
        {
            IVertex [] aoVertices = AddVertices(1);

            MockVertex oWrongTypeVertex = new MockVertex();

            oWrongTypeVertex.ParentGraph = m_oGraph;

            IEdge oEdge = new MockEdge(
                oWrongTypeVertex, aoVertices[0], false, false, 2);

            m_oEdgeCollection.Add(oEdge);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(
                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: The edge is invalid.\r\n"
                + "Parameter name: edge"
                ,
                oArgumentException.Message
                );

            Assert.IsNotNull(oArgumentException.InnerException);

            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: A vertex is not of type Vertex.  The"
                + " type is Microsoft.NodeXL"
                + ".UnitTests.MockVertex."
                ,
                oArgumentException.InnerException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestAddBad10()
    //
    /// <summary>
    /// Tests the Add(IEdge) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestAddBad10()
    {
        // Second vertex is of wrong type.

        try
        {
            IVertex [] aoVertices = AddVertices(1);

            MockVertex oWrongTypeVertex = new MockVertex();

            oWrongTypeVertex.ParentGraph = m_oGraph;

            IEdge oEdge = new MockEdge(
                aoVertices[0], oWrongTypeVertex, false, false, 2);

            m_oEdgeCollection.Add(oEdge);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(
                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: The edge is invalid.\r\n"
                + "Parameter name: edge"
                ,
                oArgumentException.Message
                );

            Assert.IsNotNull(oArgumentException.InnerException);

            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: A vertex is not of type Vertex.  The"
                + " type is Microsoft.NodeXL"
                + ".UnitTests.MockVertex."
                ,
                oArgumentException.InnerException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestAddBad11()
    //
    /// <summary>
    /// Tests the Add(IEdge) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestAddBad11()
    {
        // Directedness is wrong.

        try
        {
            InitializeGraph(GraphDirectedness.Directed);

            IVertex [] aoVertices = AddVertices(2);

            IEdge oEdge = new MockEdge(
                aoVertices[0], aoVertices[1], false, false, 2);

            m_oEdgeCollection.Add(oEdge);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: An undirected edge can't be added to a"
                + " directed graph.\r\n"
                + "Parameter name: edge"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestAddBad12()
    //
    /// <summary>
    /// Tests the Add(IEdge) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestAddBad12()
    {
        // Directedness is wrong.

        try
        {
            InitializeGraph(GraphDirectedness.Undirected);

            IVertex [] aoVertices = AddVertices(2);

            IEdge oEdge = new MockEdge(
                aoVertices[0], aoVertices[1], true, false, 2);

            m_oEdgeCollection.Add(oEdge);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: A directed edge can't be added to an"
                + " undirected graph.\r\n"
                + "Parameter name: edge"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestAddBad13()
    //
    /// <summary>
    /// Tests the Add(IEdge) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestAddBad13()
    {
        // Add edge twice.

        IEdge oEdge = null;

        try
        {
            IVertex [] aoVertices = AddVertices(2);

            oEdge = new Edge(aoVertices[0], aoVertices[1], false);

            m_oEdgeCollection.Add(oEdge);
            m_oEdgeCollection.Add(oEdge);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual( String.Format(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: An edge with the ID {0} already exists"
                + " in the collection.\r\n"
                + "Parameter name: edge"
                ,
                oEdge.ID
                )
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestAddBad14()
    //
    /// <summary>
    /// Tests the Add(IEdge) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestAddBad14()
    {
        // Add self-loop to graph that doesn't allow it.

        try
        {
            InitializeGraph(
                GraphDirectedness.Mixed, GraphRestrictions.NoSelfLoops);

            IVertex [] aoVertices = AddVertices(1);

            IEdge oEdge = new Edge(aoVertices[0], aoVertices[0], false);

            m_oEdgeCollection.Add(oEdge);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: The edge is a self-loop, and the parent"
                + " graph's Restrictions property includes the NoSelfLoops"
                + " flag.\r\n"
                + "Parameter name: edge"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestAddBad15()
    //
    /// <summary>
    /// Tests the Add(IEdge) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestAddBad15()
    {
        // Add parallel edge to graph that doesn't allow it.

        Int32 iFirstEdgeID = -1;

        try
        {
            InitializeGraph(
                GraphDirectedness.Mixed, GraphRestrictions.NoParallelEdges);

            IVertex [] aoVertices = AddVertices(2);

            IEdge oEdge = new Edge(aoVertices[0], aoVertices[1], false);

            iFirstEdgeID = oEdge.ID;

            m_oEdgeCollection.Add(oEdge);

            oEdge = new Edge(aoVertices[0], aoVertices[1], false);

            m_oEdgeCollection.Add(oEdge);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual( String.Format(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: The edge is parallel to the edge with"
                + " the ID {0}, and the parent graph's Restrictions property"
                + " includes the NoParallelEdges flag.\r\n"
                + "Parameter name: edge"
                ,
                iFirstEdgeID
                )
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestAdd3_()
    //
    /// <summary>
    /// Tests the Add(IVertex, IVertex, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdd3_()
    {
        // Add undirected edges to mixed graph.

        AddEdges(100, GraphDirectedness.Undirected, AddOverload.IVertex);
    }

    //*************************************************************************
    //  Method: TestAdd3_2()
    //
    /// <summary>
    /// Tests the Add(IVertex, IVertex, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdd3_2()
    {
        // Add directed edges to mixed graph.

        AddEdges(100, GraphDirectedness.Directed, AddOverload.IVertex);
    }

    //*************************************************************************
    //  Method: TestAdd3_3()
    //
    /// <summary>
    /// Tests the Add(IVertex, IVertex, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdd3_3()
    {
        // Add mixed edges to mixed graph.

        AddEdges(100, GraphDirectedness.Mixed, AddOverload.IVertex);
    }

    //*************************************************************************
    //  Method: TestAdd3_4()
    //
    /// <summary>
    /// Tests the Add(IVertex, IVertex, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdd3_4()
    {
        // Add undirected edges to undirected graph.

        InitializeGraph(GraphDirectedness.Undirected);

        AddEdges(100, GraphDirectedness.Undirected, AddOverload.IVertex);
    }

    //*************************************************************************
    //  Method: TestAdd3_5()
    //
    /// <summary>
    /// Tests the Add(IVertex, IVertex, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdd3_5()
    {
        // Add directed edges to directed graph.

        InitializeGraph(GraphDirectedness.Directed);

        AddEdges(100, GraphDirectedness.Directed, AddOverload.IVertex);
    }

    //*************************************************************************
    //  Method: TestAdd3_Bad1()
    //
    /// <summary>
    /// Tests the Add(IVertex, IVertex, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestAdd3_Bad1()
    {
        // First vertex is null.

        try
        {
            IVertex [] aoVertices = AddVertices(1);

            m_oEdgeCollection.Add(null, aoVertices[0], false);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core.EdgeCollection.Add:"
                + " vertex1 argument can't be null.\r\n"
                + "Parameter name: vertex1"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestAdd3_Bad2()
    //
    /// <summary>
    /// Tests the Add(IVertex, IVertex, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestAdd3_Bad2()
    {
        // Second vertex is null.

        try
        {
            IVertex [] aoVertices = AddVertices(1);

            m_oEdgeCollection.Add(aoVertices[0], null, false);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core.EdgeCollection.Add:"
                + " vertex2 argument can't be null.\r\n"
                + "Parameter name: vertex2"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestAdd3_Bad3()
    //
    /// <summary>
    /// Tests the Add(IVertex, IVertex, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestAdd3_Bad3()
    {
        // First vertex not in graph.

        try
        {
            IVertex [] aoVertices = AddVertices(1);

            IVertex oNonContainedVertex = new Vertex();

            m_oEdgeCollection.Add(oNonContainedVertex, aoVertices[0], false);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core.EdgeCollection.Add:"
                + " One of the vertices is not"
                + " contained in this graph.\r\n"
                + "Parameter name: vertex1"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestAdd3_Bad4()
    //
    /// <summary>
    /// Tests the Add(IVertex, IVertex, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestAdd3_Bad4()
    {
        // Second vertex not in graph.

        try
        {
            IVertex [] aoVertices = AddVertices(1);

            IVertex oNonContainedVertex = new Vertex();

            m_oEdgeCollection.Add(aoVertices[0], oNonContainedVertex, false);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core.EdgeCollection.Add:"
                + " One of the vertices is not"
                + " contained in this graph.\r\n"
                + "Parameter name: vertex2"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestAdd3_Bad5()
    //
    /// <summary>
    /// Tests the Add(IVertex, IVertex, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestAdd3_Bad5()
    {
        // First vertex is of wrong type.

        try
        {
            IVertex [] aoVertices = AddVertices(1);

            MockVertex oWrongTypeVertex = new MockVertex();

            oWrongTypeVertex.ParentGraph = m_oGraph;

            m_oEdgeCollection.Add(oWrongTypeVertex, aoVertices[0], false);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(
                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: The edge is invalid.\r\n"
                + "Parameter name: edge"
                ,
                oArgumentException.Message
                );

            Assert.IsNotNull(oArgumentException.InnerException);

            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: A vertex is not of type Vertex.  The"
                + " type is Microsoft.NodeXL"
                + ".UnitTests.MockVertex."
                ,
                oArgumentException.InnerException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestAdd3_Bad6()
    //
    /// <summary>
    /// Tests the Add(IVertex, IVertex, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestAdd3_Bad6()
    {
        // Second vertex is of wrong type.

        try
        {
            IVertex [] aoVertices = AddVertices(1);

            MockVertex oWrongTypeVertex = new MockVertex();

            oWrongTypeVertex.ParentGraph = m_oGraph;

            m_oEdgeCollection.Add(aoVertices[0], oWrongTypeVertex, false);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(
                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: The edge is invalid.\r\n"
                + "Parameter name: edge"
                ,
                oArgumentException.Message
                );

            Assert.IsNotNull(oArgumentException.InnerException);

            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: A vertex is not of type Vertex.  The"
                + " type is Microsoft.NodeXL"
                + ".UnitTests.MockVertex."
                ,
                oArgumentException.InnerException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestAdd3_Bad7()
    //
    /// <summary>
    /// Tests the Add(IVertex, IVertex, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestAdd3_Bad7()
    {
        // Directedness is wrong.

        try
        {
            InitializeGraph(GraphDirectedness.Directed);

            IVertex [] aoVertices = AddVertices(2);

            m_oEdgeCollection.Add(aoVertices[0], aoVertices[1], false);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: An undirected edge can't be added to a"
                + " directed graph.\r\n"
                + "Parameter name: edge"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestAdd3_Bad8()
    //
    /// <summary>
    /// Tests the Add(IVertex, IVertex, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestAdd3_Bad8()
    {
        // Directedness is wrong.

        try
        {
            InitializeGraph(GraphDirectedness.Undirected);

            IVertex [] aoVertices = AddVertices(2);

            m_oEdgeCollection.Add(aoVertices[0], aoVertices[1], true);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: A directed edge can't be added to an"
                + " undirected graph.\r\n"
                + "Parameter name: edge"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestAdd4_()
    //
    /// <summary>
    /// Tests the Add(IVertex, IVertex) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdd4_()
    {
        // Add undirected edges to undirected graph.

        InitializeGraph(GraphDirectedness.Undirected);

        AddEdges(100, GraphDirectedness.Undirected,
            AddOverload.IVertexUndirected);
    }

    //*************************************************************************
    //  Method: TestAdd4_Bad1()
    //
    /// <summary>
    /// Tests the Add(IVertex, IVertex) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestAdd4_Bad1()
    {
        // First vertex is null.

        try
        {
            InitializeGraph(GraphDirectedness.Undirected);

            IVertex [] aoVertices = AddVertices(1);

            m_oEdgeCollection.Add( null, aoVertices[0] );
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: vertex1 argument can't be null.\r\n"
                + "Parameter name: vertex1"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestAdd4_Bad2()
    //
    /// <summary>
    /// Tests the Add(IVertex, IVertex) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestAdd4_Bad2()
    {
        // Second vertex is null.

        try
        {
            InitializeGraph(GraphDirectedness.Undirected);

            IVertex [] aoVertices = AddVertices(1);

            m_oEdgeCollection.Add(aoVertices[0], null);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: vertex2 argument can't be null.\r\n"
                + "Parameter name: vertex2"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestAdd4_Bad3()
    //
    /// <summary>
    /// Tests the Add(IVertex, IVertex) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestAdd4_Bad3()
    {
        // First vertex not in graph.

        try
        {
            InitializeGraph(GraphDirectedness.Undirected);

            IVertex [] aoVertices = AddVertices(1);

            IVertex oNonContainedVertex = new Vertex();

            m_oEdgeCollection.Add( oNonContainedVertex, aoVertices[0] );
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: One of the vertices is not"
                + " contained in this graph.\r\n"
                + "Parameter name: vertex1"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestAdd4_Bad4()
    //
    /// <summary>
    /// Tests the Add(IVertex, IVertex) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestAdd4_Bad4()
    {
        // Second vertex not in graph.

        try
        {
            InitializeGraph(GraphDirectedness.Undirected);

            IVertex [] aoVertices = AddVertices(1);

            IVertex oNonContainedVertex = new Vertex();

            m_oEdgeCollection.Add(aoVertices[0], oNonContainedVertex);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: One of the vertices is not"
                + " contained in this graph.\r\n"
                + "Parameter name: vertex2"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestAdd4_Bad5()
    //
    /// <summary>
    /// Tests the Add(IVertex, IVertex) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestAdd4_Bad5()
    {
        // First vertex is of wrong type.

        try
        {
            InitializeGraph(GraphDirectedness.Undirected);

            IVertex [] aoVertices = AddVertices(1);

            MockVertex oWrongTypeVertex = new MockVertex();

            oWrongTypeVertex.ParentGraph = m_oGraph;

            m_oEdgeCollection.Add( oWrongTypeVertex, aoVertices[0] );
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(
                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: The edge is invalid.\r\n"
                + "Parameter name: edge"
                ,
                oArgumentException.Message
                );

            Assert.IsNotNull(oArgumentException.InnerException);

            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: A vertex is not of type Vertex.  The"
                + " type is Microsoft.NodeXL"
                + ".UnitTests.MockVertex."
                ,
                oArgumentException.InnerException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestAdd4_Bad6()
    //
    /// <summary>
    /// Tests the Add(IVertex, IVertex) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestAdd4_Bad6()
    {
        // Second vertex is of wrong type.

        try
        {
            InitializeGraph(GraphDirectedness.Undirected);

            IVertex [] aoVertices = AddVertices(1);

            MockVertex oWrongTypeVertex = new MockVertex();

            oWrongTypeVertex.ParentGraph = m_oGraph;

            m_oEdgeCollection.Add(aoVertices[0], oWrongTypeVertex);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(
                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: The edge is invalid.\r\n"
                + "Parameter name: edge"
                ,
                oArgumentException.Message
                );

            Assert.IsNotNull(oArgumentException.InnerException);

            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: A vertex is not of type Vertex.  The"
                + " type is Microsoft.NodeXL"
                + ".UnitTests.MockVertex."
                ,
                oArgumentException.InnerException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestAdd4_Bad7()
    //
    /// <summary>
    /// Tests the Add(IVertex, IVertex) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestAdd4_Bad7()
    {
        // Directedness is wrong.

        try
        {
            InitializeGraph(GraphDirectedness.Directed);

            IVertex [] aoVertices = AddVertices(2);

            m_oEdgeCollection.Add( aoVertices[0], aoVertices[1] );
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Add: An undirected edge can't be added to a"
                + " directed graph.\r\n"
                + "Parameter name: edge"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
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
        const Int32 Vertices = 765;

        Assert.AreEqual(0, m_oEdgeCollection.Count);
        Assert.AreEqual(0, m_oGraph.Vertices.Count);

        Int32 iEdges = AddEdges(
            Vertices, GraphDirectedness.Undirected, AddOverload.IEdge).Length;

        Assert.AreEqual(Vertices, m_oGraph.Vertices.Count);
        Assert.AreEqual(iEdges, m_oEdgeCollection.Count);

        foreach (IVertex oVertex in m_oGraph.Vertices)
        {
            Assert.IsTrue(oVertex.IncidentEdges.Count > 0);

            Assert.AreEqual(oVertex.ParentGraph, m_oGraph);
        }

        m_bEdgeRemoved = false;
        m_oRemovedEdge = null;

        m_oEdgeCollection.Clear();

        Assert.IsFalse(m_bEdgeRemoved);
        Assert.IsNull(m_oRemovedEdge);

        Assert.AreEqual(0, m_oEdgeCollection.Count);
        Assert.AreEqual(Vertices, m_oGraph.Vertices.Count);

        foreach (IVertex oVertex in m_oGraph.Vertices)
        {
            Assert.IsTrue(oVertex.IncidentEdges.Count == 0);

            Assert.AreEqual(oVertex.ParentGraph, m_oGraph);
        }
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
        // Add a small number of vertices.

        const Int32 Vertices = 10;

        TestContainsAndFind(Vertices);
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
        // Add N vertices.

        const Int32 Vertices = 1512;

        m_oGraph.PerformExtraValidations = false;

        TestContainsAndFind(Vertices);
    }

    //*************************************************************************
    //  Method: TestContainsBad()
    //
    /// <summary>
    /// Tests the Contains(IEdge) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestContainsBad()
    {
        // Null edge.

        IEdge oEdge = null;

        try
        {
            m_oEdgeCollection.Contains(oEdge);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Contains: edge argument can't be null.\r\n"
                + "Parameter name: edge"
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

        String sName = null;

        try
        {
            m_oEdgeCollection.Contains(sName);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Contains: name argument can't be null.\r\n"
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
            m_oEdgeCollection.Contains(String.Empty);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Contains: name argument must have a length"
                + " greater than zero.\r\n"
                + "Parameter name: name"
                ,
                oArgumentException.Message
                );

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
            IEdge oFoundEdge = null;

            m_oEdgeCollection.Find(null, out oFoundEdge);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Find: name argument can't be null.\r\n"
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
            IEdge oFoundEdge = null;

            m_oEdgeCollection.Find(String.Empty, out oFoundEdge);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Find: name argument must have a length"
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

        AddEdges(0, GraphDirectedness.Directed, AddOverload.IEdge);

        IEdge [] aoCopiedEdges = new IEdge[0];

        m_oEdgeCollection.CopyTo(aoCopiedEdges, 0);
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

        TestCopyTo(300, 0);
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

        TestCopyTo(300, 1);
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

        TestCopyTo(300, 500);
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
            AddEdges(1, GraphDirectedness.Directed, AddOverload.IEdge);

            m_oEdgeCollection.CopyTo(null, 0);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.CopyTo: array argument can't be"
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
        // Negative index.

        try
        {
            AddEdges(1, GraphDirectedness.Directed, AddOverload.IEdge);

            IEdge [] aoEdges = new IEdge[1];

            m_oEdgeCollection.CopyTo(aoEdges, -1);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.CopyTo: index argument must be greater than"
                + " or equal to zero.\r\n"
                + "Parameter name: index"
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
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestCopyToBad3()
    {
        // Array too small.

        try
        {
            const Int32 Vertices = 100;

            Int32 iEdges = AddEdges(
                Vertices, GraphDirectedness.Directed, AddOverload.IEdge).Length;

            IEdge [] aoEdges = new IEdge[iEdges - 1];

            m_oEdgeCollection.CopyTo(aoEdges, 0);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.CopyTo: The array is not large enough to"
                + " hold the copied collection.\r\n"
                + "Parameter name: array"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
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
        // Array too small, with offset.

        try
        {
            const Int32 Vertices = 100;

            Int32 iEdges = AddEdges(
                Vertices, GraphDirectedness.Directed, AddOverload.IEdge).Length;

            IEdge [] aoEdges = new IEdge[iEdges];

            m_oEdgeCollection.CopyTo(aoEdges, 1);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.CopyTo: The array is not large enough to"
                + " hold the copied collection.\r\n"
                + "Parameter name: array"
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
        TestGetEnumerator(9999);
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
        // Edge not in collection.

        const Int32 Vertices = 65;

        // Add the vertices and edges.

        IEdge [] aoEdges = AddEdges(
            Vertices, GraphDirectedness.Directed, AddOverload.IEdge);

        Int32 iEdges = aoEdges.Length;

        // Check the number of edges.

        Assert.AreEqual(iEdges, m_oEdgeCollection.Count);

        // Create an edge but don't add it to the collection.

        IVertex [] aoVertices = AddVertices(2);

        IEdge oNonContainedEdge = new Edge(
            aoVertices[0], aoVertices[1], false);

        oNonContainedEdge.Name = "Name";

        // Attempt to remove the non-contained edge.

        Boolean bRemoved = m_oEdgeCollection.Remove(oNonContainedEdge);

        Assert.IsFalse(bRemoved);

        bRemoved = m_oEdgeCollection.Remove(oNonContainedEdge.ID);

        Assert.IsFalse(bRemoved);

        bRemoved = m_oEdgeCollection.Remove(oNonContainedEdge.Name);

        Assert.IsFalse(bRemoved);

        // Check the number of edges.

        Assert.AreEqual(iEdges, m_oEdgeCollection.Count);
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
        // Remove first edge.

        TestRemove(11, new Int32 [] {0} );
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
        // Remove last edge.

        TestRemove(11, new Int32 [] {9} );
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
        // Remove all edges.

        TestRemove(11, new Int32 [] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9} );
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
        // Remove all edges, backwards.

        TestRemove(11, new Int32 [] {9, 8, 7, 6, 5, 4, 3, 2, 1, 0} );
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
        // Remove every even edge.

        TestRemove(11, new Int32 [] {0, 2, 4, 6, 8} );
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
        // Remove every odd edge.

        TestRemove(11, new Int32 [] {1, 3, 5, 7, 9} );
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
        // Remove all but last edge.

        TestRemove(11, new Int32 [] {0, 1, 2, 3, 4, 5, 6, 7, 8} );
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
        // Remove all but first edge.

        TestRemove(11, new Int32 [] {1, 2, 3, 4, 5, 6, 7, 8, 9} );
    }

    //*************************************************************************
    //  Method: TestRemoveBad()
    //
    /// <summary>
    /// Tests the Remove(IEdge) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestRemoveBad()
    {
        // Null edge.

        IEdge oEdge = null;

        try
        {
            m_oEdgeCollection.Remove(oEdge);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Remove: edge argument can't be null.\r\n"
                + "Parameter name: edge"
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
            m_oEdgeCollection.Remove(sName);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Remove: name argument can't be null.\r\n"
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
            m_oEdgeCollection.Remove(String.Empty);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.Remove: name argument must have a length"
                + " greater than zero.\r\n"
                + "Parameter name: name"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestRemoveBad4()
    //
    /// <summary>
    /// Tests the Remove(IEdge) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(InvalidOperationException) ) ]

    public void
    TestRemoveBad4()
    {
        // Attempt to access Edge.Parent after removing an edge and its
        // vertices.

        const Int32 Vertices = 25;

        try
        {
            IEdge [] aoAddedEdges = AddEdges(
                Vertices, GraphDirectedness.Directed, AddOverload.IEdge);

            m_oGraph.Vertices.Clear();
            
            IGraph oGraph = aoAddedEdges[0].ParentGraph;
        }
        catch (InvalidOperationException oInvalidOperationException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "Edge.ParentGraph: The edge has been removed from its parent"
                + " graph and is no longer valid.  Do not attempt to access an"
                + " edge that has been removed from its graph."
                ,
                oInvalidOperationException.Message
                );

            throw oInvalidOperationException;
        }
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
        // No connecting edges.

        IVertex [] aoVertices = AddVertices(2);

        ICollection<IEdge> oConnectingEdges =
            m_oEdgeCollection.GetConnectingEdges(
                aoVertices[0], aoVertices[1] );

        Assert.IsNotNull(oConnectingEdges);
        Assert.AreEqual(0, oConnectingEdges.Count);

        oConnectingEdges = m_oEdgeCollection.GetConnectingEdges(
            aoVertices[1], aoVertices[0] );

        Assert.IsNotNull(oConnectingEdges);
        Assert.AreEqual(0, oConnectingEdges.Count);

        oConnectingEdges = m_oEdgeCollection.GetConnectingEdges(
            aoVertices[0], aoVertices[0] );

        Assert.IsNotNull(oConnectingEdges);
        Assert.AreEqual(0, oConnectingEdges.Count);

        oConnectingEdges = m_oEdgeCollection.GetConnectingEdges(
            aoVertices[1], aoVertices[1] );

        Assert.IsNotNull(oConnectingEdges);
        Assert.AreEqual(0, oConnectingEdges.Count);
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
        // One connecting edge.

        m_oGraph.PerformExtraValidations = false;

        const Int32 Vertices = 10000;

        IVertex [] aoVertices;
        IEdge [] aoEdges;
        
        AddEdges(Vertices, GraphDirectedness.Undirected, AddOverload.IEdge,
            out aoVertices, out aoEdges);

        for (Int32 i = 1; i < Vertices; i++)
        {
            // Vertex 0 is connected to vertex i.

            ICollection<IEdge> oConnectingEdges =
                m_oEdgeCollection.GetConnectingEdges(
                    aoVertices[0], aoVertices[i] );

            Assert.IsNotNull(oConnectingEdges);

            Assert.AreEqual(1, oConnectingEdges.Count);

            Assert.AreEqual(aoEdges[i - 1].ID, oConnectingEdges.First().ID);

            // Vertex i is connected to vertex 0.

            oConnectingEdges = m_oEdgeCollection.GetConnectingEdges(
                aoVertices[i], aoVertices[0] );

            Assert.IsNotNull(oConnectingEdges);

            Assert.AreEqual(1, oConnectingEdges.Count);

            Assert.AreEqual(aoEdges[i - 1].ID, oConnectingEdges.First().ID);
        }
    }

    //*************************************************************************
    //  Method: TestGetConnectingEdges3()
    //
    /// <summary>
    /// Tests the GetConnectingEdges() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetConnectingEdges3()
    {
        // No connecting edges.

        const Int32 Vertices = 1000;

        IVertex [] aoVertices;
        IEdge [] aoEdges;
        
        AddEdges(Vertices, GraphDirectedness.Undirected, AddOverload.IEdge,
            out aoVertices, out aoEdges);

        for (Int32 i = 1; i < Vertices - 1; i++)
        {
            // Vertex i is not connected to vertex i + 1.

            ICollection<IEdge> oConnectingEdges =
                m_oEdgeCollection.GetConnectingEdges(
                    aoVertices[i], aoVertices[i + 1] );

            Assert.IsNotNull(oConnectingEdges);

            Assert.AreEqual(0, oConnectingEdges.Count);

            // Vertex i + 1 is not connected to vertex i.

            oConnectingEdges = m_oEdgeCollection.GetConnectingEdges(
                aoVertices[i + 1], aoVertices[i] );

            Assert.IsNotNull(oConnectingEdges);

            Assert.AreEqual(0, oConnectingEdges.Count);
        }
    }

    //*************************************************************************
    //  Method: TestGetConnectingEdges4()
    //
    /// <summary>
    /// Tests the GetConnectingEdges() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetConnectingEdges4()
    {
        // Complete graph.

        const Int32 Vertices = 100;

        IVertex [] aoVertices = AddVertices(Vertices);

        IEdge [] aoEdges = TestGraphUtil.MakeGraphComplete(
            m_oGraph, aoVertices, false);
        
        for (Int32 i = 0; i < Vertices; i++)
        {
            for (Int32 j = 0; j < Vertices; j++)
            {
                ICollection<IEdge> oConnectingEdgesForward =
                    m_oEdgeCollection.GetConnectingEdges(
                        aoVertices[i], aoVertices[j] );

                ICollection<IEdge> oConnectingEdgesBackward =
                    m_oEdgeCollection.GetConnectingEdges(
                        aoVertices[j], aoVertices[i] );

                Assert.IsNotNull(oConnectingEdgesForward);
                Assert.IsNotNull(oConnectingEdgesBackward);

                if (i == j)
                {
                    // There are no self-loops.

                    Assert.AreEqual(0, oConnectingEdgesForward.Count);
                    Assert.AreEqual(0, oConnectingEdgesBackward.Count);
                }
                else
                {
                    // The edge is connected once to every other vertex.

                    Assert.AreEqual(1, oConnectingEdgesForward.Count);

                    Assert.AreEqual(1, oConnectingEdgesBackward.Count);

                    Assert.AreEqual( oConnectingEdgesForward.First(),
                        oConnectingEdgesBackward.First() );
                }
            }
        }
    }

    //*************************************************************************
    //  Method: TestGetConnectingEdges5()
    //
    /// <summary>
    /// Tests the GetConnectingEdges() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetConnectingEdges5()
    {
        // Complete graph, plus each vertex has one self-loop.

        const Int32 Vertices = 100;

        IVertex [] aoVertices = AddVertices(Vertices);

        TestGraphUtil.MakeGraphComplete(m_oGraph, aoVertices, false);

        IEdge [] aoSelfLoopEdges = new IEdge[Vertices];

        for (Int32 i = 0; i < Vertices; i++)
        {
            IVertex oVertex = aoVertices[i];

            IEdge oSelfLoopEdge = aoSelfLoopEdges[i] =
                new Edge(oVertex, oVertex, false);

            m_oEdgeCollection.Add(oSelfLoopEdge);
        }

        for (Int32 i = 0; i < Vertices; i++)
        {
            IVertex oVertex = aoVertices[i];

            ICollection<IEdge> oConnectingEdges =
                m_oEdgeCollection.GetConnectingEdges(oVertex, oVertex);

            Assert.AreEqual(1, oConnectingEdges.Count);

            Assert.AreEqual( aoSelfLoopEdges[i], oConnectingEdges.First() );
        }
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
        // First vertex is null.

        try
        {
            IVertex [] aoVertices = AddVertices(1);

            ICollection<IEdge> oConnectingEdges =
                m_oEdgeCollection.GetConnectingEdges(null, aoVertices[0] );
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.GetConnectingEdges: vertex1 argument can't"
                + " be null.\r\n"
                + "Parameter name: vertex1"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestGetConnectingEdgesBad2()
    //
    /// <summary>
    /// Tests the GetConnectingEdges() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestGetConnectingEdgesBad2()
    {
        // Second vertex is null.

        try
        {
            IVertex [] aoVertices = AddVertices(1);

            ICollection<IEdge> oConnectingEdges =
                m_oEdgeCollection.GetConnectingEdges(aoVertices[0], null);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeCollection.GetConnectingEdges: vertex2 argument can't"
                + " be null.\r\n"
                + "Parameter name: vertex2"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestGetConnectingEdgesBad3()
    //
    /// <summary>
    /// Tests the GetConnectingEdges() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestGetConnectingEdgesBad3()
    {
        // First vertex not in graph.

        try
        {
            IVertex [] aoVertices = AddVertices(1);

            IVertex oNonContainedVertex = new Vertex();

            ICollection<IEdge> oConnectingEdges =
                m_oEdgeCollection.GetConnectingEdges(
                    oNonContainedVertex, aoVertices[0] );
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(
                "Microsoft.NodeXL.Core."
                + "EdgeCollection.GetConnectingEdges: One of the vertices is"
                + " not contained in this graph.\r\n"
                + "Parameter name: vertex1"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestGetConnectingEdgesBad4()
    //
    /// <summary>
    /// Tests the GetConnectingEdges() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestGetConnectingEdgesBad4()
    {
        // Second vertex not in graph.

        try
        {
            IVertex [] aoVertices = AddVertices(1);

            IVertex oNonContainedVertex = new Vertex();

            ICollection<IEdge> oConnectingEdges =
                m_oEdgeCollection.GetConnectingEdges(
                    aoVertices[0], oNonContainedVertex );
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(
                "Microsoft.NodeXL.Core."
                + "EdgeCollection.GetConnectingEdges: One of the vertices is"
                + " not contained in this graph.\r\n"
                + "Parameter name: vertex2"
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
        // 0 edges.

        Assert.AreEqual(
            "0 edges",
            m_oEdgeCollection.ToString()
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
        // 1 edge.

        Int32 Vertices = 2;

        IEdge [] aoEdges = AddEdges(
            Vertices, GraphDirectedness.Directed, AddOverload.IEdge);

        Int32 iEdges = aoEdges.Length;

        Assert.AreEqual(
            "1 edge",
            m_oEdgeCollection.ToString()
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
        // N edges.

        Int32 Vertices = 99;

        IEdge [] aoEdges = AddEdges(
            Vertices, GraphDirectedness.Directed, AddOverload.IEdge);

        Int32 iEdges = aoEdges.Length;

        Assert.AreEqual(
            "98 edges",
            m_oEdgeCollection.ToString()
            );
    }

    //*************************************************************************
    //  Enum: AddOverload
    //
    /// <summary>
    /// Specifies which overload of EdgeCollection.Add() to call.
    /// </summary>
    //*************************************************************************

    protected enum
    AddOverload
    {
        /// <summary>
        /// Call Add(IEdge).
        /// </summary>

        IEdge,

        /// <summary>
        /// Call Add(IVertex, IVertex, Boolean).
        /// </summary>

        IVertex,

        /// <summary>
        /// Call Add(IVertex, IVertex).
        /// </summary>

        IVertexUndirected,
    }

    //*************************************************************************
    //  Enum: RemoveOverload
    //
    /// <summary>
    /// Specifies which overload of EdgeCollection.Remove() to call.
    /// </summary>
    //*************************************************************************

    protected enum
    RemoveOverload
    {
        /// <summary>
        /// Call Remove(IEdge).
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
        InitializeGraph(eDirectedness, GraphRestrictions.None);
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
    ///
    /// <param name="eRestrictions">
    /// Restrictions to apply to m_oGraph.
    /// </param>
    //*************************************************************************

    protected void
    InitializeGraph
    (
        GraphDirectedness eDirectedness,
        GraphRestrictions eRestrictions
    )
    {
        m_oGraph = new Graph(eDirectedness, eRestrictions);

        m_oGraph.PerformExtraValidations = true;

        Debug.Assert(m_oGraph.Edges is EdgeCollection);

        m_oEdgeCollection = m_oGraph.Edges;

        ( (EdgeCollection)m_oEdgeCollection ).EdgeAdded +=
            new EdgeEventHandler(this.EdgeCollection_EdgeAdded);

        ( (EdgeCollection)m_oEdgeCollection ).EdgeRemoved +=
            new EdgeEventHandler(this.EdgeCollection_EdgeRemoved);

        m_bEdgeAdded = false;
        m_oAddedEdge = null;

        m_bEdgeRemoved = false;
        m_oRemovedEdge = null;
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
        Debug.Assert(iVerticesToAdd >= 3);

        // Add the vertices.

        IVertex[] aoVertices = AddVertices(iVerticesToAdd);

        // Connect some of them with edges.

        IEdge [] aoContainedEdges = new IEdge[iVerticesToAdd - 1];

        for (Int32 i = 1; i < iVerticesToAdd; i++)
        {
            IEdge oContainedEdge = new Edge(
                aoVertices[0], aoVertices[i], true);

            oContainedEdge.Name = i.ToString();

            aoContainedEdges[i - 1] = oContainedEdge;

            m_oEdgeCollection.Add(oContainedEdge);
        }

        // Create an edge but don't add it.

        IEdge oNonContainedEdge = new Edge(
            aoVertices[1], aoVertices[2], false);

        oNonContainedEdge.Name = "ekdjrmnek";

        IEdge oFoundEdge;

        // The collection should not contain oNonContainedEdge.

        Assert.IsFalse( m_oEdgeCollection.Contains(oNonContainedEdge) );

        Assert.IsFalse( m_oEdgeCollection.Contains(oNonContainedEdge.ID) );

        Assert.IsFalse( m_oEdgeCollection.Find(
            oNonContainedEdge.ID, out oFoundEdge) );

        Assert.IsNull(oFoundEdge);

        Assert.IsFalse( m_oEdgeCollection.Contains(oNonContainedEdge.Name) );

        Assert.IsFalse( m_oEdgeCollection.Find(
            oNonContainedEdge.Name, out oFoundEdge) );

        Assert.IsNull(oFoundEdge);

        // The collection should contain the edges in aoContainedEdges.

        foreach (IEdge oContainedEdge in aoContainedEdges)
        {
            Assert.IsTrue( m_oEdgeCollection.Contains(oContainedEdge) );

            Assert.IsTrue( m_oEdgeCollection.Contains(oContainedEdge.ID) );

            Assert.IsTrue( m_oEdgeCollection.Find(
                oContainedEdge.ID, out oFoundEdge) );

            Assert.AreEqual(oFoundEdge, oContainedEdge);

            Assert.IsTrue( m_oEdgeCollection.Contains(oContainedEdge.Name) );

            Assert.IsTrue( m_oEdgeCollection.Find(
                oContainedEdge.Name, out oFoundEdge) );

            Assert.AreEqual(oFoundEdge, oContainedEdge);
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

        m_oGraph.PerformExtraValidations = false;

        IEdge [] aoAddedEdges = AddEdges(
            iVerticesToAdd, GraphDirectedness.Directed, AddOverload.IEdge);

        Int32 iAddedEdges = aoAddedEdges.Length;

        IEdge [] aoCopiedEdges = new IEdge[iAddedEdges + iOffset];

        m_oEdgeCollection.CopyTo(aoCopiedEdges, iOffset);

        // The entries in aoCopiedEdges before the offset should not have been
        // touched.

        for (Int32 i = 0; i < iOffset; i++)
        {
            Assert.IsNull( aoCopiedEdges[i] );
        }

        // Each edge in aoAddedEdges must be in aoCopiedEdges.  Note that
        // aoCopiedEdges is not in any specified order.

        foreach (IEdge oAddedEdge in aoAddedEdges)
        {
            Assert.IsTrue( Array.IndexOf(aoCopiedEdges, oAddedEdge) >= 0);
        }

        // Each edge in aoCopiedEdges must be in aoAddedEdges.

        foreach (IEdge oCopiedEdge in aoCopiedEdges)
        {
            if (oCopiedEdge == null)
            {
                continue;
            }

            Assert.IsTrue( Array.IndexOf(aoAddedEdges, oCopiedEdge) >= 0);
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
        Debug.Assert(iVerticesToAdd >= 0);

        m_oGraph.PerformExtraValidations = false;

        IEdge [] aoAddedEdges = AddEdges(
            iVerticesToAdd, GraphDirectedness.Directed, AddOverload.IEdge);

        Int32 iAddedEdges = aoAddedEdges.Length;

        // Each edge in aoAddedEdges must be in m_oEdgeCollection.  Note that
        // the enumerated edges are not in any specified order.

        foreach (IEdge oAddedEdge in aoAddedEdges)
        {
            Boolean bFound = false;

            foreach (IEdge oEdge in m_oEdgeCollection)
            {
                if (oEdge == oAddedEdge)
                {
                    bFound = true;
                    break;
                }
            }

            Assert.IsTrue(bFound);
        }

        // Each enumerated edge must be in aoAddedEdges.

        Int32 iEnumeratedEdges = 0;

        foreach (IEdge oEdge in m_oEdgeCollection)
        {
            Assert.IsTrue( Array.IndexOf(aoAddedEdges, oEdge) >= 0);

            iEnumeratedEdges++;
        }

        Assert.AreEqual(iAddedEdges, iEnumeratedEdges);
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
    /// <param name="aiIndexesOfEdgesToRemove">
    /// Array of indexes of the edges to remove.
    /// </param>
    //*************************************************************************

    protected void
    TestRemove
    (
        Int32 iVerticesToAdd,
        Int32 [] aiIndexesOfEdgesToRemove
    )
    {
        Debug.Assert(iVerticesToAdd >= 0);
        Debug.Assert(aiIndexesOfEdgesToRemove != null);

        TestRemove(iVerticesToAdd, aiIndexesOfEdgesToRemove,
            RemoveOverload.ByReference);

        TestRemove(iVerticesToAdd, aiIndexesOfEdgesToRemove,
            RemoveOverload.ByID);

        TestRemove(iVerticesToAdd, aiIndexesOfEdgesToRemove,
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
    /// <param name="aiIndexesOfEdgesToRemove">
    /// Array of indexes of the edges to remove.
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
        Int32 [] aiIndexesOfEdgesToRemove,
        RemoveOverload eRemoveOverload
    )
    {
        Debug.Assert(iVerticesToAdd >= 0);
        Debug.Assert(aiIndexesOfEdgesToRemove != null);

        Debug.Assert(
            Enum.IsDefined(typeof(RemoveOverload), eRemoveOverload) );

        // Remove any existing edges.

        m_oEdgeCollection.Clear();

        // Add the vertices and edges.

        IEdge [] aoEdges = AddEdges(
            iVerticesToAdd, GraphDirectedness.Directed, AddOverload.IEdge);

        Int32 iEdges = aoEdges.Length;

        // Check the number of edges.

        Assert.AreEqual(iEdges, m_oEdgeCollection.Count);

        // Remove the specified edges.

        Int32 iEdgesToRemove = aiIndexesOfEdgesToRemove.Length;

        for (Int32 i = 0; i < iEdgesToRemove; i++)
        {
            IEdge oEdgeToRemove = aoEdges[ aiIndexesOfEdgesToRemove[i] ];

            Boolean bRemoved = false;

            m_bEdgeRemoved = false;
            m_oRemovedEdge = null;

            switch (eRemoveOverload)
            {
                case RemoveOverload.ByReference:

                    bRemoved = m_oEdgeCollection.Remove(oEdgeToRemove);

                    break;

                case RemoveOverload.ByID:

                    bRemoved = m_oEdgeCollection.Remove(oEdgeToRemove.ID);

                    break;

                case RemoveOverload.ByName:

                    bRemoved = m_oEdgeCollection.Remove(oEdgeToRemove.Name);

                    break;

                default:

                    Debug.Assert(false);
                    break;
            }

            Assert.IsTrue(bRemoved);

            Assert.IsTrue(m_bEdgeRemoved);
            Assert.AreEqual(oEdgeToRemove, m_oRemovedEdge);
        }

        // Check the number of edges.

        Int32 iRemainingEdges = iEdges - iEdgesToRemove;

        Assert.AreEqual(iRemainingEdges, m_oEdgeCollection.Count);

        // Verify that the correct edges are still in the collection.

        for (Int32 i = 0; i < iEdges; i++)
        {
            Boolean bContains = m_oEdgeCollection.Contains( aoEdges[i] );

            if (Array.IndexOf(aiIndexesOfEdgesToRemove, i) >= 0)
            {
                // i is in aiIndexesOfEdgesToRemove, so aiEdges[i] should not
                // be in the collection.

                Assert.IsFalse(bContains);
            }
            else
            {
                Assert.IsTrue(bContains);
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

        return ( TestGraphUtil.AddVertices(m_oGraph, iVerticesToAdd) );
    }

    //*************************************************************************
    //  Method: AddEdges()
    //
    /// <summary>
    /// Adds a specified number of vertices to m_oGraph using one of the Add()
    /// methods, then connects some of them with edges.
    /// </summary>
    ///
    /// <param name="iVerticesToAdd">
    /// Number of vertices to add.
    /// </param>
    ///
    /// <param name="eDirectedness">
    /// Directedness of the added edges.
    /// </param>
    ///
    /// <param name="eAddOverload">
    /// Specifies which overload of Add() to call.
    /// </param>
    ///
    /// <returns>
    /// An array of the added edges.
    /// </returns>
    ///
    /// <remarks>
    /// The first vertex is connected to every other vertex.  Thus, the number
    /// of added edges is <paramref name="iVerticesToAdd" /> minus one.
    /// </remarks>
    //*************************************************************************

    protected IEdge []
    AddEdges
    (
        Int32 iVerticesToAdd,
        GraphDirectedness eDirectedness,
        AddOverload eAddOverload
    )
    {
        Debug.Assert(iVerticesToAdd >= 0);

        IVertex [] aoVertices;

        IEdge[] aoEdges;

        AddEdges(iVerticesToAdd, eDirectedness, eAddOverload, out aoVertices,
            out aoEdges);

        return (aoEdges);
    }

    //*************************************************************************
    //  Method: AddEdges()
    //
    /// <summary>
    /// Adds a specified number of vertices to m_oGraph using one of the Add()
    /// methods, then connects some of them with edges.
    /// </summary>
    ///
    /// <param name="iVerticesToAdd">
    /// Number of vertices to add.
    /// </param>
    ///
    /// <param name="eDirectedness">
    /// Directedness of the added edges.
    /// </param>
    ///
    /// <param name="eAddOverload">
    /// Specifies which overload of Add() to call.
    /// </param>
    ///
    /// <param name="aoVertices">
    /// Where the added vertices get stored.
    /// </param>
    ///
    /// <param name="aoEdges">
    /// Where the added edges get stored.
    /// </param>
    ///
    /// <remarks>
    /// The first vertex is connected to every other vertex.  Thus, the number
    /// of added edges is <paramref name="iVerticesToAdd" /> minus one.
    /// </remarks>
    //*************************************************************************

    protected void
    AddEdges
    (
        Int32 iVerticesToAdd,
        GraphDirectedness eDirectedness,
        AddOverload eAddOverload,
        out IVertex [] aoVertices,
        out IEdge [] aoEdges
    )
    {
        Debug.Assert(iVerticesToAdd >= 0);

        aoVertices = AddVertices(iVerticesToAdd);

        aoEdges = new IEdge[ Math.Max(0, iVerticesToAdd - 1) ];

        for (Int32 i = 1; i < iVerticesToAdd; i++)
        {
            Boolean bDirected = false;

            switch (eDirectedness)
            {
                case GraphDirectedness.Directed:

                    bDirected = true;
                    break;

                case GraphDirectedness.Undirected:

                    bDirected = false;
                    break;

                case GraphDirectedness.Mixed:

                    // Make every other edge directed.

                    bDirected = (i % 2 == 0);
                    break;

                default:

                    Debug.Assert(false);
                    break;
            }

            m_bEdgeAdded = false;
            m_oAddedEdge = null;

            IVertex oVertex1 = aoVertices[0];
            IVertex oVertex2 = aoVertices[i];

            IEdge oEdge = null;

            switch (eAddOverload)
            {
                case AddOverload.IEdge:

                    oEdge = new Edge(oVertex1, oVertex2, bDirected);
                    m_oEdgeCollection.Add(oEdge);

                    break;

                case AddOverload.IVertex:

                    oEdge = m_oEdgeCollection.Add(
                        oVertex1, oVertex2, bDirected);

                    break;

                case AddOverload.IVertexUndirected:

                    oEdge = m_oEdgeCollection.Add(oVertex1, oVertex2);
                    break;

                default:

                    Debug.Assert(false);
                    break;
            }

            oEdge.Name = oEdge.ID.ToString();

            aoEdges[i - 1] = oEdge;

            Assert.IsTrue(m_bEdgeAdded);
            Assert.AreEqual(oEdge, m_oAddedEdge);
        }

        Assert.AreEqual( Math.Max(0, iVerticesToAdd - 1),
            m_oEdgeCollection.Count);
    }

    //*************************************************************************
    //  Method: EdgeCollection_EdgeAdded()
    //
    /// <summary>
    /// Handles the EdgeAdded event on the m_oEdgeCollection object.
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
        if ( oSender == null || !(oSender is EdgeCollection) )
        {
            throw new ApplicationException(
                "EdgeAdded event provided incorrect oSender argument."
                );
        }

        m_bEdgeAdded = true;

        m_oAddedEdge = oEdgeEventArgs.Edge;
    }

    //*************************************************************************
    //  Method: EdgeCollection_EdgeRemoved()
    //
    /// <summary>
    /// Handles the EdgeRemoved event on the m_oEdgeCollection object.
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
    EdgeCollection_EdgeRemoved
    (
        Object oSender,
        EdgeEventArgs oEdgeEventArgs
    )
    {
        if ( oSender == null || !(oSender is EdgeCollection) )
        {
            throw new ApplicationException(
                "EdgeRemoved event provided incorrect oSender argument."
                );
        }

        m_bEdgeRemoved = true;

        m_oRemovedEdge = oEdgeEventArgs.Edge;
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected IEdgeCollection m_oEdgeCollection;

    /// Graph that owns m_oEdgeCollection.

    protected IGraph m_oGraph;

    /// Gets set by EdgeCollection_EdgeAdded().

    protected Boolean m_bEdgeAdded;

    /// Gets set by EdgeCollection_EdgeAdded().

    protected IEdge m_oAddedEdge;

    /// Gets set by EdgeCollection_EdgeRemoved().

    protected Boolean m_bEdgeRemoved;

    /// Gets set by EdgeCollection_EdgeRemoved().

    protected IEdge m_oRemovedEdge;
}

}
