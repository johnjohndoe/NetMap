
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Algorithms;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: ConnectedComponentCalculatorTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="ConnectedComponentCalculator" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class ConnectedComponentCalculatorTest : Object
{
    //*************************************************************************
    //  Constructor: ConnectedComponentCalculatorTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ConnectedComponentCalculatorTest" /> class.
    /// </summary>
    //*************************************************************************

    public ConnectedComponentCalculatorTest()
    {
        m_oGraph = null;
        m_oVertices = null;
        m_oEdges = null;
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
        m_oVertices = m_oGraph.Vertices;
        m_oEdges = m_oGraph.Edges;
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
    //  Method: TestGetStronglyConnectedComponents()
    //
    /// <summary>
    /// Tests the GetStronglyConnectedComponents() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetStronglyConnectedComponents()
    {
        // Empty graph.

        List< LinkedList<IVertex> > oStronglyConnectedComponents =
            ConnectedComponentCalculator.GetStronglyConnectedComponents(
                m_oGraph);

        Assert.AreEqual(0, oStronglyConnectedComponents.Count);
    }

    //*************************************************************************
    //  Method: TestGetStronglyConnectedComponents2()
    //
    /// <summary>
    /// Tests the GetStronglyConnectedComponents() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetStronglyConnectedComponents2()
    {
        // One component with one vertex.

        IVertex oVertex = m_oVertices.Add();

        List< LinkedList<IVertex> > oStronglyConnectedComponents =
            ConnectedComponentCalculator.GetStronglyConnectedComponents(
                m_oGraph);

        Assert.AreEqual(1, oStronglyConnectedComponents.Count);

        CheckThatComponentConsistsOfVertices(oStronglyConnectedComponents[0],
            oVertex.ID);
    }

    //*************************************************************************
    //  Method: TestGetStronglyConnectedComponents3()
    //
    /// <summary>
    /// Tests the GetStronglyConnectedComponents() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetStronglyConnectedComponents3()
    {
        // N components with one vertex each.

        const Int32 Vertices = 100;
        Int32 [] aiVertexIDs = new Int32[Vertices];

        for (Int32 i = 0; i < Vertices; i++)
        {
            aiVertexIDs[i] = m_oVertices.Add().ID;
        }

        List< LinkedList<IVertex> > oStronglyConnectedComponents =
            ConnectedComponentCalculator.GetStronglyConnectedComponents(
                m_oGraph);

        Assert.AreEqual(Vertices, oStronglyConnectedComponents.Count);

        HashSet<Int32> oFoundVertexIDs = new HashSet<Int32>();

        foreach (LinkedList<IVertex> oStronglyConnectedComponent in
            oStronglyConnectedComponents)
        {
            Assert.AreEqual(1, oStronglyConnectedComponent.Count);
            Int32 iFoundVertexID = oStronglyConnectedComponent.First.Value.ID;

            if ( oFoundVertexIDs.Contains(iFoundVertexID) )
            {
                Assert.Fail("Two components contain the same vertex.");
            }

            oFoundVertexIDs.Add(iFoundVertexID);
        }

        foreach (Int32 iVertexID in aiVertexIDs)
        {
            if ( !oFoundVertexIDs.Contains(iVertexID) )
            {
                Assert.Fail("A vertex is not contained in a component.");
            }
        }
    }

    //*************************************************************************
    //  Method: TestGetStronglyConnectedComponents4()
    //
    /// <summary>
    /// Tests the GetStronglyConnectedComponents() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetStronglyConnectedComponents4()
    {
        // One component with two vertices.

        IVertex oVertex1 = m_oVertices.Add();
        IVertex oVertex2 = m_oVertices.Add();
        m_oEdges.Add(oVertex1, oVertex2);

        List< LinkedList<IVertex> > oStronglyConnectedComponents =
            ConnectedComponentCalculator.GetStronglyConnectedComponents(
                m_oGraph);

        Assert.AreEqual(1, oStronglyConnectedComponents.Count);

        CheckThatComponentConsistsOfVertices(oStronglyConnectedComponents[0],
            oVertex1.ID, oVertex2.ID);
    }

    //*************************************************************************
    //  Method: TestGetStronglyConnectedComponents5()
    //
    /// <summary>
    /// Tests the GetStronglyConnectedComponents() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetStronglyConnectedComponents5()
    {
        // One component with N vertices.

        const Int32 Vertices = 100;
        Int32 [] aiVertexIDs = new Int32[Vertices];
        IVertex oFirstVertex = null;

        for (Int32 i = 0; i < Vertices; i++)
        {
            IVertex oVertex = m_oVertices.Add();
            aiVertexIDs[i] = oVertex.ID;

            if (i == 0)
            {
                oFirstVertex = oVertex;
            }
            else
            {
                m_oEdges.Add(oFirstVertex, oVertex);
            }
        }

        List< LinkedList<IVertex> > oStronglyConnectedComponents =
            ConnectedComponentCalculator.GetStronglyConnectedComponents(
                m_oGraph);

        Assert.AreEqual(1, oStronglyConnectedComponents.Count);

        CheckThatComponentConsistsOfVertices(oStronglyConnectedComponents[0],
             aiVertexIDs);
    }

    //*************************************************************************
    //  Method: TestGetStronglyConnectedComponents6()
    //
    /// <summary>
    /// Tests the GetStronglyConnectedComponents() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetStronglyConnectedComponents6()
    {
        // N components with 1-N vertices each.

        const Int32 Components = 100;
        Int32 [][] aiiVertexIDs = new Int32[Components][];

        // Add the components in decreasing order to test component sorting.

        for (Int32 iComponent = Components - 1; iComponent >= 0; iComponent--)
        {
            Int32 iVertices = iComponent + 1;
            Int32 [] aiVertexIDs = new Int32[iVertices];
            aiiVertexIDs[iComponent] = aiVertexIDs;

            IVertex oFirstVertex = null;

            for (Int32 i = 0; i < iVertices; i++)
            {
                IVertex oVertex = m_oVertices.Add();
                aiVertexIDs[i] = oVertex.ID;

                if (i == 0)
                {
                    oFirstVertex = oVertex;
                }
                else
                {
                    m_oEdges.Add(oFirstVertex, oVertex);
                }
            }
        }

        List< LinkedList<IVertex> > oStronglyConnectedComponents =
            ConnectedComponentCalculator.GetStronglyConnectedComponents(
                m_oGraph);

        Assert.AreEqual(Components, oStronglyConnectedComponents.Count);

        Int32 j = 0;

        foreach (LinkedList<IVertex> oStronglyConnectedComponent in
            oStronglyConnectedComponents)
        {
            CheckThatComponentConsistsOfVertices( 
                oStronglyConnectedComponent, aiiVertexIDs[j] );

            j++;
        }
    }

    //*************************************************************************
    //  Method: CheckThatComponentConsistsOfVertices()
    //
    /// <summary>
    /// Determines whether a component consists of a specified set of vertices.
    /// </summary>
    ///
    /// <param name="oComponent">
    /// Strongly connected component.
    /// </param>
    ///
    /// <param name="aiVertexIDs">
    /// Set of vertices that the component must consist of, in no particular
    /// order.
    /// </param>
    //*************************************************************************

    protected void
    CheckThatComponentConsistsOfVertices
    (
        LinkedList<IVertex> oComponent,
        params Int32 [] aiVertexIDs
    )
    {
        Assert.IsNotNull(oComponent != null);
        Assert.AreEqual(aiVertexIDs.Length, oComponent.Count);

        // The key is a vertex ID from aiVertexIDs and the value is false if
        // the ID hasn't been found yet in the component.

        Dictionary<Int32, Boolean> oVertexIDFlags =
            new Dictionary<Int32, Boolean>(aiVertexIDs.Length);

        foreach (Int32 iVertexID in aiVertexIDs)
        {
            oVertexIDFlags.Add(iVertexID, false);
        }

        foreach (IVertex oVertex in oComponent)
        {
            Int32 iVertexID = oVertex.ID;

            if ( !oVertexIDFlags.ContainsKey(iVertexID) )
            {
                Assert.Fail("Unexpected vertex in component.");
            }

            if ( oVertexIDFlags[iVertexID] )
            {
                Assert.Fail("Vertex in component two times.");
            }

            oVertexIDFlags[iVertexID] = true;
        }

        foreach (KeyValuePair<Int32, Boolean> oKeyValuePair in oVertexIDFlags)
        {
            if (!oKeyValuePair.Value)
            {
                Assert.Fail("Expected vertex not found in component.");
            }
        }
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The graph being test.

    protected IGraph m_oGraph;

    /// The graph's vertices;

    protected IVertexCollection m_oVertices;

    /// The graph's Edges;

    protected IEdgeCollection m_oEdges;
}

}
