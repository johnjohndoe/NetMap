
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Common;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: EdgeTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="Edge" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class EdgeTest : Object
{
    //*************************************************************************
    //  Constructor: EdgeTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EdgeTest" /> class.
    /// </summary>
    //*************************************************************************

    public EdgeTest()
    {
        m_aoVertices = null;
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
        const Int32 Vertices = 100;

        CreateGraph(GraphDirectedness.Mixed, Vertices);
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
        m_aoVertices = null;
        m_oGraph = null;
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
        IVertex oVertex1 = m_aoVertices[0];
        IVertex oVertex2 = m_aoVertices[1];

        IEdge oEdge = CreateEdge(oVertex1, oVertex2, true);

        Assert.AreEqual(oVertex1, oEdge.BackVertex);
        Assert.AreEqual(oVertex2, oEdge.FrontVertex);

        Assert.IsTrue(oEdge.IsDirected);
        Assert.IsFalse(oEdge.IsSelfLoop);
        Assert.IsNull(oEdge.Name);
        Assert.AreEqual(m_oGraph, oEdge.ParentGraph);

        Assert.IsNotNull(oEdge.Vertices);
        Assert.AreEqual(2, oEdge.Vertices.Length);

        Assert.IsNotNull( oEdge.Vertices[0] );
        Assert.AreEqual( oVertex1, oEdge.Vertices[0] );

        Assert.IsNotNull( oEdge.Vertices[1] );
        Assert.AreEqual( oVertex2, oEdge.Vertices[1] );
    }
 
    //*************************************************************************
    //  Method: TestBackAndFrontVertex()
    //
    /// <summary>
    /// Tests the BackVertex and FrontVertex properties.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestBackAndFrontVertex()
    {
        IVertex oVertex1 = m_aoVertices[0];
        IVertex oVertex2 = m_aoVertices[1];

        IEdge oEdge = CreateEdge(oVertex1, oVertex2, true);

        Assert.AreEqual(oVertex1, oEdge.BackVertex);
        Assert.AreEqual(oVertex2, oEdge.FrontVertex);

        oEdge = CreateEdge(oVertex2, oVertex1, true);

        Assert.AreEqual(oVertex2, oEdge.BackVertex);
        Assert.AreEqual(oVertex1, oEdge.FrontVertex);
    }
 
    //*************************************************************************
    //  Method: TestBackVertexBad()
    //
    /// <summary>
    /// Tests the BackVertex property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestBackVertexBad()
    {
        // Ask for BackVertex on an undirected edge.

        IVertex oVertex1 = m_aoVertices[0];
        IVertex oVertex2 = m_aoVertices[1];

        IEdge oEdge = CreateEdge(oVertex1, oVertex2, false);

        try
        {
            IVertex oVertex = oEdge.BackVertex;
        }
        catch (ApplicationException oApplicationException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "Edge.BackVertex: The edge is not directed, so it does not"
                + " have a back vertex."
                ,
                oApplicationException.Message
                );

            throw oApplicationException;
        }
    }
 
    //*************************************************************************
    //  Method: TestFrontVertexBad()
    //
    /// <summary>
    /// Tests the FrontVertex property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestFrontVertexBad()
    {
        // Ask for FrontVertex on an undirected edge.

        IVertex oVertex1 = m_aoVertices[0];
        IVertex oVertex2 = m_aoVertices[1];

        IEdge oEdge = CreateEdge(oVertex1, oVertex2, false);

        try
        {
            IVertex oVertex = oEdge.FrontVertex;
        }
        catch (ApplicationException oApplicationException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "Edge.FrontVertex: The edge is not directed, so it does not"
                + " have a front vertex."
                ,
                oApplicationException.Message
                );

            throw oApplicationException;
        }
    }
 
    //*************************************************************************
    //  Method: TestIsDirected()
    //
    /// <summary>
    /// Tests the IsDirected property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestIsDirected()
    {
        IVertex oVertex1 = m_aoVertices[0];
        IVertex oVertex2 = m_aoVertices[1];

        IEdge oEdge = CreateEdge(oVertex1, oVertex2, true);

        Assert.IsTrue(oEdge.IsDirected);
    }
 
    //*************************************************************************
    //  Method: TestIsDirected2()
    //
    /// <summary>
    /// Tests the IsDirected property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestIsDirected2()
    {
        IVertex oVertex1 = m_aoVertices[0];
        IVertex oVertex2 = m_aoVertices[1];

        IEdge oEdge = CreateEdge(oVertex1, oVertex2, false);

        Assert.IsFalse(oEdge.IsDirected);
    }
 
    //*************************************************************************
    //  Method: TestIsSelfLoop()
    //
    /// <summary>
    /// Tests the IsSelfLoop property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestIsSelfLoop()
    {
        IVertex oVertex1 = m_aoVertices[0];
        IVertex oVertex2 = m_aoVertices[1];

        IEdge oEdge = CreateEdge(oVertex1, oVertex2, true);

        Assert.IsFalse(oEdge.IsSelfLoop);
    }
 
    //*************************************************************************
    //  Method: TestIsSelfLoop2()
    //
    /// <summary>
    /// Tests the IsSelfLoop property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestIsSelfLoop2()
    {
        IVertex oVertex1 = m_aoVertices[0];

        IEdge oEdge = CreateEdge(oVertex1, oVertex1, true);

        Assert.IsTrue(oEdge.IsSelfLoop);
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

        IVertex oVertex1 = m_aoVertices[0];
        IVertex oVertex2 = m_aoVertices[1];

        IEdge oEdge = CreateEdge(oVertex1, oVertex2, true);

        oEdge.Name = Name;

        Assert.AreEqual(Name, oEdge.Name);
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

        IVertex oVertex1 = m_aoVertices[0];
        IVertex oVertex2 = m_aoVertices[1];

        IEdge oEdge = CreateEdge(oVertex1, oVertex2, true);

        oEdge.Name = Name;

        Assert.AreEqual(Name, oEdge.Name);
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

        IVertex oVertex1 = m_aoVertices[0];
        IVertex oVertex2 = m_aoVertices[1];

        IEdge oEdge = CreateEdge(oVertex1, oVertex2, true);

        oEdge.Name = Name;

        Assert.AreEqual(Name, oEdge.Name);
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
        IVertex oVertex1 = m_aoVertices[0];
        IVertex oVertex2 = m_aoVertices[1];

        IEdge oEdge = CreateEdge(oVertex1, oVertex2, true);

        Assert.AreEqual(m_oGraph, oEdge.ParentGraph);
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
        IVertex oVertex1 = m_aoVertices[0];
        IVertex oVertex2 = m_aoVertices[1];

        IEdge oEdge = CreateEdge(oVertex1, oVertex2, true);

        Assert.IsNotNull(oEdge.Vertices);
        Assert.AreEqual(2, oEdge.Vertices.Length);

        Assert.IsNotNull( oEdge.Vertices[0] );
        Assert.AreEqual( oVertex1, oEdge.Vertices[0] ); 
        Assert.AreEqual( oEdge.BackVertex, oEdge.Vertices[0] ); 

        Assert.IsNotNull( oEdge.Vertices[1] );
        Assert.AreEqual( oVertex2, oEdge.Vertices[1] ); 
        Assert.AreEqual( oEdge.FrontVertex, oEdge.Vertices[1] ); 
    }

    //*************************************************************************
    //  Method: TestIsParallelTo()
    //
    /// <summary>
    /// Tests the IsParallelTo and IsAntiparallelTo properties.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestIsParallelTo()
    {
        IsParallelToInfo [] aoIsParallelToInfo = new IsParallelToInfo[] {

            new IsParallelToInfo
            (
                GraphDirectedness.Directed,
                0, 1, true,
                0, 1, true,
                true
            ),

            new IsParallelToInfo
            (
                GraphDirectedness.Directed,
                0, 1, true,
                1, 0, true,
                false
            ),

            new IsParallelToInfo
            (
                GraphDirectedness.Directed,
                0, 1, true,
                0, 2, true,
                false
            ),

            new IsParallelToInfo
            (
                GraphDirectedness.Directed,
                0, 1, true,
                2, 1, true,
                false
            ),

            new IsParallelToInfo
            (
                GraphDirectedness.Undirected,
                0, 1, false,
                0, 1, false,
                true
            ),

            new IsParallelToInfo
            (
                GraphDirectedness.Undirected,
                1, 0, false,
                0, 1, false,
                true
            ),

            new IsParallelToInfo
            (
                GraphDirectedness.Undirected,
                0, 1, false,
                1, 0, false,
                true
            ),

            new IsParallelToInfo
            (
                GraphDirectedness.Undirected,
                0, 2, false,
                0, 1, false,
                false
            ),

            new IsParallelToInfo
            (
                GraphDirectedness.Undirected,
                0, 1, false,
                2, 1, false,
                false
            ),

            new IsParallelToInfo
            (
                GraphDirectedness.Mixed,
                0, 1, false,
                0, 1, false,
                true
            ),

            new IsParallelToInfo
            (
                GraphDirectedness.Mixed,
                0, 1, false,
                0, 1, true,
                true
            ),

            new IsParallelToInfo
            (
                GraphDirectedness.Mixed,
                0, 1, false,
                1, 0, true,
                true
            ),

            new IsParallelToInfo
            (
                GraphDirectedness.Mixed,
                0, 1, true,
                0, 1, false,
                true
            ),

            new IsParallelToInfo
            (
                GraphDirectedness.Mixed,
                0, 1, true,
                0, 1, true,
                true
            ),

            new IsParallelToInfo
            (
                GraphDirectedness.Mixed,
                0, 1, true,
                1, 0, true,
                false
            ),

            new IsParallelToInfo
            (
                GraphDirectedness.Mixed,
                1, 0, true,
                0, 1, false,
                true
            ),

            new IsParallelToInfo
            (
                GraphDirectedness.Mixed,
                1, 0, true,
                0, 1, true,
                false
            ),

            new IsParallelToInfo
            (
                GraphDirectedness.Mixed,
                1, 0, true,
                1, 0, true,
                true
            ),

            new IsParallelToInfo
            (
                GraphDirectedness.Mixed,
                0, 1, false,
                0, 2, false,
                false
            ),

            new IsParallelToInfo
            (
                GraphDirectedness.Mixed,
                0, 1, false,
                2, 1, false,
                false
            ),

            new IsParallelToInfo
            (
                GraphDirectedness.Mixed,
                0, 1, false,
                0, 2, true,
                false
            ),

            new IsParallelToInfo
            (
                GraphDirectedness.Mixed,
                0, 1, false,
                2, 1, true,
                false
            ),

            new IsParallelToInfo
            (
                GraphDirectedness.Mixed,
                0, 1, true,
                0, 2, false,
                false
            ),

            new IsParallelToInfo
            (
                GraphDirectedness.Mixed,
                0, 1, true,
                2, 1, false,
                false
            ),

            new IsParallelToInfo
            (
                GraphDirectedness.Mixed,
                0, 1, true,
                0, 2, true,
                false
            ),

            new IsParallelToInfo
            (
                GraphDirectedness.Mixed,
                0, 1, true,
                2, 1, true,
                false
            ),

            };

        foreach (IsParallelToInfo oIsParallelToInfo in aoIsParallelToInfo)
        {
            TestIsParallelTo(

                oIsParallelToInfo.Directedness,
                oIsParallelToInfo.Edge1Vertex1,
                oIsParallelToInfo.Edge1Vertex2,
                oIsParallelToInfo.Edge1IsDirected,
                oIsParallelToInfo.Edge2Vertex1,
                oIsParallelToInfo.Edge2Vertex2,
                oIsParallelToInfo.Edge2IsDirected,
                oIsParallelToInfo.ExpectedEdge1IsParallelToEdge2
            );
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
        TestClone(false, false, CloneOverload.SameType);
        TestClone(false, false, CloneOverload.SpecifiedVertices);
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
        TestClone(false, true, CloneOverload.SameType);
        TestClone(false, true, CloneOverload.SpecifiedVertices);
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
        TestClone(true, false, CloneOverload.SameType);
        TestClone(true, false, CloneOverload.SpecifiedVertices);
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
        TestClone(true, true, CloneOverload.SameType);
        TestClone(true, true, CloneOverload.SpecifiedVertices);
    }

    //*************************************************************************
    //  Method: TestCloneBad2()
    //
    /// <summary>
    /// Tests the Clone() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestCloneBad2()
    {
        // null vertex 1.

        try
        {
            IVertex oVertex1 = m_aoVertices[0];
            IVertex oVertex2 = m_aoVertices[1];

            IEdge oEdge = CreateEdge(oVertex1, oVertex2, true);

            oEdge.Clone(true, true, null, oVertex2, true);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "Edge.Clone: vertex1 argument can't be null.\r\n"
                + "Parameter name: vertex1"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestCloneBad3()
    //
    /// <summary>
    /// Tests the Clone() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestCloneBad3()
    {
        // null vertex 2.

        try
        {
            IVertex oVertex1 = m_aoVertices[0];
            IVertex oVertex2 = m_aoVertices[1];

            IEdge oEdge = CreateEdge(oVertex1, oVertex2, true);

            oEdge.Clone(true, true, oVertex1, null, true);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "Edge.Clone: vertex2 argument can't be null.\r\n"
                + "Parameter name: vertex2"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestCloneBad4()
    //
    /// <summary>
    /// Tests the Clone() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestCloneBad4()
    {
        // Vertices not in same graph.

        try
        {
            IVertex oVertex1 = m_aoVertices[0];
            IVertex oVertex2 = m_aoVertices[1];

            IEdge oEdge = CreateEdge(oVertex1, oVertex2, true);

            IGraph oGraph2 = new Graph();

            IVertex [] aoVertices2 = TestGraphUtil.AddVertices(oGraph2, 2);

            oEdge.Clone(true, true, oVertex1, aoVertices2[0], true);
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
    //  Method: TestGetAdjacentVertex()
    //
    /// <summary>
    /// Tests the GetAdjacentVertex() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetAdjacentVertex()
    {
        IVertex oVertex1 = m_aoVertices[0];
        IVertex oVertex2 = m_aoVertices[1];

        IEdge oEdge = CreateEdge(oVertex1, oVertex2, true);

        Assert.AreEqual( oVertex2, oEdge.GetAdjacentVertex(oVertex1) );
        Assert.AreEqual( oVertex1, oEdge.GetAdjacentVertex(oVertex2) );
    }

    //*************************************************************************
    //  Method: TestGetAdjacentVertexBad()
    //
    /// <summary>
    /// Tests the Clone() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestGetAdjacentVertexBad()
    {
        // Vertex not in edge.

        try
        {
            IVertex oVertex1 = m_aoVertices[0];
            IVertex oVertex2 = m_aoVertices[1];

            IEdge oEdge = CreateEdge(oVertex1, oVertex2, true);

            oEdge.GetAdjacentVertex( m_aoVertices[2] );
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "Edge.GetAdjacentVertex: The specified vertex is not one of"
                + " the edge's vertices.\r\n"
                + "Parameter name: vertex"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
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
        Assert.AreEqual( "A\vB", Edge.GetVertexNamePair("A", "B", true) );
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
        Assert.AreEqual( "B\vA", Edge.GetVertexNamePair("B", "A", true) );
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
        Assert.AreEqual( "A\vB", Edge.GetVertexNamePair("A", "B", false) );
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
        Assert.AreEqual( "A\vB", Edge.GetVertexNamePair("B", "A", false) );
    }

    //*************************************************************************
    //  Enum: CloneOverload
    //
    /// <summary>
    /// Specifies which overload of Edge.Clone() to call.
    /// </summary>
    //*************************************************************************

    protected enum
    CloneOverload
    {
        /// <summary>
        /// Call Clone(Boolean, Boolean).
        /// </summary>

        SameType,

        /// <summary>
        /// Call Clone(Boolean, Boolean, IVertex, IVertex,
        /// Boolean).
        /// </summary>

        SpecifiedVertices,
    }

    //*************************************************************************
    //  Method: TestClone()
    //
    /// <summary>
    /// Tests the Clone() methods.
    /// </summary>
    ///
    /// <param name="bCopyMetadataValues">
    /// true to copy metadata values while cloning.
    /// </param>
    ///
    /// <param name="bCopyTag">
    /// true to copy tag values while cloning.
    /// </param>
    ///
    /// <param name="eCloneOverload">
    /// Specifies which overload of Clone() to call.
    /// </param>
    //*************************************************************************

    protected void
    TestClone
    (
        Boolean bCopyMetadataValues,
        Boolean bCopyTag,
        CloneOverload eCloneOverload
    )
    {
        // Create N objects, set random metadata and Tag on each object, clone
        // each object, check new object.

        const Int32 Vertices = 1000;

        CreateGraph(GraphDirectedness.Directed, Vertices);

        // Connect the first vertex to each of the other vertices.

        IEdge [] aoEdges = new Edge[Vertices - 1];

        for (Int32 i = 0; i < Vertices - 1; i++)
        {
            IEdge oEdge = aoEdges[i] =
                CreateEdge(m_aoVertices[0], m_aoVertices[i + 1], true);

            MetadataUtil.SetRandomMetadata(oEdge, true, true, i);

            oEdge.Name = oEdge.ID.ToString();
        }

        // Create a second graph with 2 vertices for the
        // CloneOverload.SpecifiedVertices case.

        IGraph oGraph2 = new Graph();

        IVertex [] aoVertices2 = TestGraphUtil.AddVertices(oGraph2, 2);

        for (Int32 i = 0; i < Vertices - 1; i++)
        {
            // Clone the object.

            IEdge oEdge = aoEdges[i];

            IEdge oNewEdge = null;

            switch (eCloneOverload)
            {
                case CloneOverload.SameType:

                    oNewEdge = oEdge.Clone(bCopyMetadataValues, bCopyTag);

                    break;

                case CloneOverload.SpecifiedVertices:

                    oNewEdge = oEdge.Clone(
                        bCopyMetadataValues, bCopyTag, aoVertices2[0],
                        aoVertices2[1], true);

                    break;

                default:

                    Debug.Assert(false);
                    break;
            }

            // Check the metadata on the new object.

            MetadataUtil.CheckRandomMetadata(
                oNewEdge, bCopyMetadataValues, bCopyTag, i);

            // Check the name and ID on the new object.

            Assert.AreEqual(oEdge.Name, oNewEdge.Name);

            Assert.AreNotEqual(oEdge.ID, oNewEdge.ID);

            // Check the vertices on the new object.

            Assert.IsNotNull(oNewEdge.Vertices);
            Assert.AreEqual(2, oNewEdge.Vertices.Length);

            if (eCloneOverload == CloneOverload.SpecifiedVertices)
            {
                Assert.AreEqual( aoVertices2[0], oNewEdge.Vertices[0] );
                Assert.AreEqual( aoVertices2[1], oNewEdge.Vertices[1] );

                // Make sure the cloned edge can be added to the second graph.

                oGraph2.Edges.Add(oNewEdge);
            }
            else
            {
                Assert.AreEqual( oEdge.Vertices[0], oNewEdge.Vertices[0] );
                Assert.AreEqual( oEdge.Vertices[1], oNewEdge.Vertices[1] );
            }
        }
    }

    //*************************************************************************
    //  Method: TestIsParallelTo()
    //
    /// <summary>
    /// Tests the IsParallelTo and IsAntiparallelTo properties.
    /// </summary>
    ///
    /// <param name="eDirectedness">
    /// Specifies the type of edges that can be added to the graph. 
    /// </param>
    ///
    /// <param name="iEdge1Vertex1">
    /// Index of the first vertex of the first edge.
    /// </param>
    ///
    /// <param name="iEdge1Vertex2">
    /// Index of the second vertex of the first edge.
    /// </param>
    ///
    /// <param name="bEdge1IsDirected">
    /// true if the first edge is directed.
    /// </param>
    ///
    /// <param name="iEdge2Vertex1">
    /// Index of the first vertex of the second edge.
    /// </param>
    ///
    /// <param name="iEdge2Vertex2">
    /// Index of the second vertex of the second edge.
    /// </param>
    ///
    /// <param name="bEdge2IsDirected">
    /// true if the second edge is directed.
    /// </param>
    ///
    /// <param name="bExpectedEdge1IsParallelToEdge2">
    /// true if the the first edge should be considered parallel to the second
    /// edge.
    /// </param>
    //*************************************************************************

    protected void
    TestIsParallelTo
    (
        GraphDirectedness eDirectedness,
        Int32 iEdge1Vertex1,
        Int32 iEdge1Vertex2,
        Boolean bEdge1IsDirected,
        Int32 iEdge2Vertex1,
        Int32 iEdge2Vertex2,
        Boolean bEdge2IsDirected,
        Boolean bExpectedEdge1IsParallelToEdge2
    )
    {
        const Int32 Vertices = 100;

        CreateGraph(eDirectedness, Vertices);

        IVertex oEdge1Vertex1 = m_aoVertices[iEdge1Vertex1];
        IVertex oEdge1Vertex2 = m_aoVertices[iEdge1Vertex2];

        IVertex oEdge2Vertex1 = m_aoVertices[iEdge2Vertex1];
        IVertex oEdge2Vertex2 = m_aoVertices[iEdge2Vertex2];

        IEdge oEdge1 =
            CreateEdge(oEdge1Vertex1, oEdge1Vertex2, bEdge1IsDirected);

        IEdge oEdge2 =
            CreateEdge(oEdge2Vertex1, oEdge2Vertex2, bEdge2IsDirected);

        IEdgeCollection oEdgeCollection = m_oGraph.Edges;

        oEdgeCollection.Add(oEdge1);
        oEdgeCollection.Add(oEdge2);

        Boolean bActualEdge1IsParallelToEdge2 = oEdge1.IsParallelTo(oEdge2);

        Assert.AreEqual(
            bExpectedEdge1IsParallelToEdge2, bActualEdge1IsParallelToEdge2);

        Boolean bActualEdge2IsParallelToEdge1 = oEdge2.IsParallelTo(oEdge1);

        Assert.AreEqual(
            bExpectedEdge1IsParallelToEdge2, bActualEdge2IsParallelToEdge1);

        Boolean bActualEdge1IsAntiparallelToEdge2 =
            oEdge1.IsAntiparallelTo(oEdge2);

        Assert.AreEqual(
            !bExpectedEdge1IsParallelToEdge2,
            bActualEdge1IsAntiparallelToEdge2
            );

        Boolean bActualEdge2IsAntiparallelToEdge1 =
            oEdge2.IsAntiparallelTo(oEdge1);

        Assert.AreEqual(
            !bExpectedEdge1IsParallelToEdge2,
            bActualEdge2IsAntiparallelToEdge1
            );
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
        IVertex oVertex1 = m_aoVertices[0];
        IVertex oVertex2 = m_aoVertices[1];

        IEdge oEdge = CreateEdge(oVertex1, oVertex2, true);

        Assert.AreEqual( "ID=" + oEdge.ID.ToString(NodeXLBase.Int32Format),
            oEdge.ToString() );
    }

    //*************************************************************************
    //  Method: CreateGraph()
    //
    /// <summary>
    /// Creates a graph and adds vertices to it.
    /// </summary>
    ///
    /// <param name="eDirectedness">
    /// Specifies the type of edges that can be added to the graph. 
    /// </param>
    ///
    /// <param name="iVertices">
    /// Number of vertices to add to the graph.
    /// </param>
    //*************************************************************************

    protected void
    CreateGraph
    (
        GraphDirectedness eDirectedness,
        Int32 iVertices
    )
    {
        Debug.Assert(iVertices >= 0);

        m_oGraph = new Graph(eDirectedness);

        m_aoVertices = TestGraphUtil.AddVertices(m_oGraph, iVertices);
    }

    //*************************************************************************
    //  Method: CreateEdge()
    //
    /// <summary>
    /// Creates an Edge object.
    /// </summary>
    ///
    /// <param name="oVertex1">
    /// The edge's first vertex.  The vertex must have already been added to
    /// the graph to which the new edge will be added.
    /// </param>
    ///
    /// <param name="oVertex2">
    /// The edge's second vertex.  The vertex must have already been added to
    /// the graph to which the new edge will be added.
    /// </param>
    ///
    /// <param name="bIsDirected">
    /// If true, <paramref name="oVertex1" /> is the edge's back vertex and
    /// <paramref name="oVertex2" /> is the edge's front vertex.  If false, the
    /// edge is undirected.
    /// </param>
    ///
    /// <returns>
    /// The new Edge object.
    /// </returns>
    //*************************************************************************

    protected IEdge
    CreateEdge
    (
        IVertex oVertex1,
        IVertex oVertex2,
        Boolean bIsDirected
    )
    {
        return ( new Edge(oVertex1, oVertex2, bIsDirected) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Array of vertices belonging to a graph.

    protected IVertex [] m_aoVertices;

    /// Graph that owns the vertices.

    protected IGraph m_oGraph;


    //*************************************************************************
    //  Struct: TestIsParallelToInfo
    //
    /// <summary>
    /// Stores test data for the TestIsParallelTo() method.
    /// </summary>
    //*************************************************************************

    protected struct IsParallelToInfo
    {
        public IsParallelToInfo
        (
            GraphDirectedness eDirectedness,
            Int32 iEdge1Vertex1,
            Int32 iEdge1Vertex2,
            Boolean bEdge1IsDirected,
            Int32 iEdge2Vertex1,
            Int32 iEdge2Vertex2,
            Boolean bEdge2IsDirected,
            Boolean bExpectedEdge1IsParallelToEdge2
        )
        {
            Directedness = eDirectedness;
            Edge1Vertex1 = iEdge1Vertex1;
            Edge1Vertex2 = iEdge1Vertex2;
            Edge1IsDirected = bEdge1IsDirected;
            Edge2Vertex1 = iEdge2Vertex1;
            Edge2Vertex2 = iEdge2Vertex2;
            Edge2IsDirected = bEdge2IsDirected;
            ExpectedEdge1IsParallelToEdge2 = bExpectedEdge1IsParallelToEdge2;
        }

        public GraphDirectedness Directedness;
        public Int32 Edge1Vertex1;
        public Int32 Edge1Vertex2;
        public Boolean Edge1IsDirected;
        public Int32 Edge2Vertex1;
        public Int32 Edge2Vertex2;
        public Boolean Edge2IsDirected;
        public Boolean ExpectedEdge1IsParallelToEdge2;
    }
}

}
