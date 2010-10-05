
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
        m_oConnectedComponentCalculator = null;
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
        m_oConnectedComponentCalculator = new ConnectedComponentCalculator();
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
        m_oConnectedComponentCalculator = null;
        m_oGraph = null;
        m_oVertices = null;
        m_oEdges = null;
    }

    //*************************************************************************
    //  Method: TestCalculateStronglyConnectedComponents()
    //
    /// <summary>
    /// Tests the CalculateStronglyConnectedComponents() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateStronglyConnectedComponents()
    {
        // Empty graph.

        IList< LinkedList<IVertex> > oStronglyConnectedComponents =
            m_oConnectedComponentCalculator.
            CalculateStronglyConnectedComponents(m_oGraph, true);

        Assert.AreEqual(0, oStronglyConnectedComponents.Count);
    }

    //*************************************************************************
    //  Method: TestCalculateStronglyConnectedComponents2()
    //
    /// <summary>
    /// Tests the CalculateStronglyConnectedComponents() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateStronglyConnectedComponents2()
    {
        // One component with one vertex.

        IVertex oVertex = m_oVertices.Add();

        IList< LinkedList<IVertex> > oStronglyConnectedComponents =
            m_oConnectedComponentCalculator.
            CalculateStronglyConnectedComponents(m_oGraph, true);

        Assert.AreEqual(1, oStronglyConnectedComponents.Count);

        CheckThatComponentConsistsOfVertices(oStronglyConnectedComponents[0],
            oVertex.ID);
    }

    //*************************************************************************
    //  Method: TestCalculateStronglyConnectedComponents3()
    //
    /// <summary>
    /// Tests the CalculateStronglyConnectedComponents() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateStronglyConnectedComponents3()
    {
        // N components with one vertex each.

        const Int32 Vertices = 100;
        Int32 [] aiVertexIDs = new Int32[Vertices];

        for (Int32 i = 0; i < Vertices; i++)
        {
            aiVertexIDs[i] = m_oVertices.Add().ID;
        }

        IList< LinkedList<IVertex> > oStronglyConnectedComponents =
            m_oConnectedComponentCalculator.
            CalculateStronglyConnectedComponents(m_oGraph, true);

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
    //  Method: TestCalculateStronglyConnectedComponents4()
    //
    /// <summary>
    /// Tests the CalculateStronglyConnectedComponents() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateStronglyConnectedComponents4()
    {
        // One component with two vertices.

        IVertex oVertex1 = m_oVertices.Add();
        IVertex oVertex2 = m_oVertices.Add();
        m_oEdges.Add(oVertex1, oVertex2);

        IList< LinkedList<IVertex> > oStronglyConnectedComponents =
            m_oConnectedComponentCalculator.
            CalculateStronglyConnectedComponents(m_oGraph, true);

        Assert.AreEqual(1, oStronglyConnectedComponents.Count);

        CheckThatComponentConsistsOfVertices(oStronglyConnectedComponents[0],
            oVertex1.ID, oVertex2.ID);
    }

    //*************************************************************************
    //  Method: TestCalculateStronglyConnectedComponents5()
    //
    /// <summary>
    /// Tests the CalculateStronglyConnectedComponents() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateStronglyConnectedComponents5()
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

        IList< LinkedList<IVertex> > oStronglyConnectedComponents =
            m_oConnectedComponentCalculator.
            CalculateStronglyConnectedComponents(m_oGraph, true);

        Assert.AreEqual(1, oStronglyConnectedComponents.Count);

        CheckThatComponentConsistsOfVertices(oStronglyConnectedComponents[0],
             aiVertexIDs);
    }

    //*************************************************************************
    //  Method: TestCalculateStronglyConnectedComponents6()
    //
    /// <summary>
    /// Tests the CalculateStronglyConnectedComponents() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateStronglyConnectedComponents6()
    {
        // N components with 1-N vertices each, sort ascending.

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

        IList< LinkedList<IVertex> > oStronglyConnectedComponents =
            m_oConnectedComponentCalculator.
            CalculateStronglyConnectedComponents(m_oGraph, true);

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
    //  Method: TestCalculateStronglyConnectedComponents7()
    //
    /// <summary>
    /// Tests the CalculateStronglyConnectedComponents() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateStronglyConnectedComponents7()
    {
        // N components with 1-N vertices each, sort descending.

        const Int32 Components = 100;
        Int32 [][] aiiVertexIDs = new Int32[Components][];

        // Add the components in increasing order to test component sorting.

        for (Int32 iComponent = 0; iComponent < Components; iComponent++)
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

        IList< LinkedList<IVertex> > oStronglyConnectedComponents =
            m_oConnectedComponentCalculator.
            CalculateStronglyConnectedComponents(m_oGraph, false);

        Assert.AreEqual(Components, oStronglyConnectedComponents.Count);

        Int32 j = oStronglyConnectedComponents.Count - 1;

        foreach (LinkedList<IVertex> oStronglyConnectedComponent in
            oStronglyConnectedComponents)
        {
            CheckThatComponentConsistsOfVertices( 
                oStronglyConnectedComponent, aiiVertexIDs[j] );

            j--;
        }
    }

    //*************************************************************************
    //  Method: TestCalculateStronglyConnectedComponents8()
    //
    /// <summary>
    /// Tests the CalculateStronglyConnectedComponents() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateStronglyConnectedComponents8()
    {
        // Test component sorting when layout sort order is specified.

        IVertex [] aoVertices = new IVertex[14];

        for (Int32 i = 0; i < aoVertices.Length; i++)
        {
            IVertex oVertex = m_oVertices.Add();

            // Make sure that sorting can handle a missing key.  They key is
            // optional.

            if (i != 12)
            {
                oVertex.SetValue(ReservedMetadataKeys.SortableLayoutOrder,
                    (Single)i);
            }

            aoVertices[i] = oVertex;
        }

        m_oGraph.SetValue(ReservedMetadataKeys.SortableLayoutOrderSet, null);

        // Each group of Add() calls here is a strongly connected component.

        m_oEdges.Add( aoVertices[6], aoVertices[7] );

        m_oEdges.Add( aoVertices[4], aoVertices[5] );

        m_oEdges.Add( aoVertices[2], aoVertices[3] );

        m_oEdges.Add( aoVertices[13], aoVertices[12] );
        m_oEdges.Add( aoVertices[13], aoVertices[11] );

        m_oEdges.Add( aoVertices[10], aoVertices[9] );
        m_oEdges.Add( aoVertices[10], aoVertices[8] );

        IList< LinkedList<IVertex> > oStronglyConnectedComponents =
            m_oConnectedComponentCalculator.
            CalculateStronglyConnectedComponents(m_oGraph, true);

        Assert.AreEqual(7, oStronglyConnectedComponents.Count);

        CheckThatComponentConsistsOfVertices( oStronglyConnectedComponents[0],
            new Int32[]{aoVertices[0].ID} );

        CheckThatComponentConsistsOfVertices( oStronglyConnectedComponents[1],
            new Int32[]{aoVertices[1].ID} );

        CheckThatComponentConsistsOfVertices( oStronglyConnectedComponents[2],
            new Int32[]{aoVertices[2].ID, aoVertices[3].ID} );

        CheckThatComponentConsistsOfVertices( oStronglyConnectedComponents[3],
            new Int32[]{aoVertices[4].ID, aoVertices[5].ID} );

        CheckThatComponentConsistsOfVertices( oStronglyConnectedComponents[4],
            new Int32[]{aoVertices[6].ID, aoVertices[7].ID} );

        CheckThatComponentConsistsOfVertices( oStronglyConnectedComponents[5],
            new Int32[]{aoVertices[8].ID, aoVertices[9].ID,
                aoVertices[10].ID} );

        CheckThatComponentConsistsOfVertices( oStronglyConnectedComponents[6],
            new Int32[]{aoVertices[11].ID, aoVertices[12].ID,
                aoVertices[13].ID} );
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

    /// The object being tested.

    protected ConnectedComponentCalculator m_oConnectedComponentCalculator;

    /// The graph being tested.

    protected IGraph m_oGraph;

    /// The graph's vertices;

    protected IVertexCollection m_oVertices;

    /// The graph's Edges;

    protected IEdgeCollection m_oEdges;
}

}
