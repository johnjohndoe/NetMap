
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Layouts;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: VertexGridSnapperTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="VertexGridSnapper" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class VertexGridSnapperTest : Object
{
    //*************************************************************************
    //  Constructor: VertexGridSnapperTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexGridSnapperTest" />
    /// class.
    /// </summary>
    //*************************************************************************

    public VertexGridSnapperTest()
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
        m_oGraph = new Graph();
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
    }

    //*************************************************************************
    //  Method: TestSnapVerticesToGrid()
    //
    /// <summary>
    /// Tests the TestSnapVerticesToGrid() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSnapVerticesToGrid()
    {
        TestData [] aoTestData = new TestData [] {
            new TestData(3, 0, 0),
            new TestData(3, 0.5F, 0),
            new TestData(3, 1.0F, 0),
            new TestData(3, 1.5F, 0),  // Math.Round() rounds down to 0.
            new TestData(3, 2.0F, 3),
            new TestData(3, 2.5F, 3),
            new TestData(3, 3.0F, 3),
            new TestData(3, 3.5F, 3),
            new TestData(3, 4.0F, 3),
            new TestData(3, 4.5F, 6),  // Math.Round() rounds up to 2.
            new TestData(3, 5.0F, 6),
            new TestData(3, 5.5F, 6),
            new TestData(3, 6.0F, 6),
            new TestData(3, 6.5F, 6),
            new TestData(3, 7.0F, 6),
            new TestData(3, 7.5F, 6),  // Math.Round() rounds down to 2.
            new TestData(3, 8.0F, 9),
            new TestData(3, 8.5F, 9),
            new TestData(3, 9.0F, 9),
            };

        IVertex oVertex = m_oGraph.Vertices.Add();

        foreach (Boolean bSetXCoordinate in TestGraphUtil.AllBoolean)
        {
            foreach (TestData oTestData in aoTestData)
            {
                if (bSetXCoordinate)
                {
                    oVertex.Location = new PointF(oTestData.Coordinate, 0);
                }
                else
                {
                    oVertex.Location = new PointF(0, oTestData.Coordinate);
                }

                VertexGridSnapper.SnapVerticesToGrid(m_oGraph,
                    oTestData.GridSize);

                Assert.AreEqual(oTestData.ExpectedSnappedCoordinate,
                    bSetXCoordinate ? oVertex.Location.X : oVertex.Location.Y);
            }
        }
    }
    
    private class TestData
    {
        public TestData
        (
            Int32 gridSize,
            Single coordinate,
            Single expectedSnappedCoordinate
        )
        {
            GridSize = gridSize;
            Coordinate = coordinate;
            ExpectedSnappedCoordinate = expectedSnappedCoordinate;
        }

        public Int32 GridSize;
        public Single Coordinate;
        public Single ExpectedSnappedCoordinate;
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    protected IGraph m_oGraph;
}

}
