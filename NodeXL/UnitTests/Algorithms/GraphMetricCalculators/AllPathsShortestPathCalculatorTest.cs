
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Algorithms;
using Microsoft.NodeXL.Common;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: AllPathsShortestPathCalculatorTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="AllPathsShortestPathCalculator" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class AllPathsShortestPathCalculatorTest : Object
{
    //*************************************************************************
    //  Constructor: AllPathsShortestPathCalculatorTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AllPathsShortestPathCalculatorTest" /> class.
    /// </summary>
    //*************************************************************************

    public AllPathsShortestPathCalculatorTest()
    {
        m_oAllPathsShortestPathCalculator = null;
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
        m_oAllPathsShortestPathCalculator =
            new AllPathsShortestPathCalculator();
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
        m_oAllPathsShortestPathCalculator = null;
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
        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: TimeTryCalculateGraphMetrics()
    //
    /// <summary>
    /// Times the TryCalculateGraphMetrics() method.
    /// </summary>
    //*************************************************************************

    // This is for timing purposes only and should not be part of a normal
    // test run.

    #if false
    [TestMethodAttribute]
    #endif

    public void
    TimeTryCalculateGraphMetrics()
    {
        const Int32 Vertices = 5000;
        const Int32 Edges = 8000;

        IGraph oGraph = new Graph();
        IVertex [] aoVertices = TestGraphUtil.AddVertices(oGraph, Vertices);
        IEdgeCollection oEdges = oGraph.Edges;
        Random oRandom = new Random(1);

        for (Int32 i = 0; i < Edges; i++)
        {
            oEdges.Add(
                aoVertices[ oRandom.Next(Vertices) ],
                aoVertices[ oRandom.Next(Vertices) ]
                );
        }

        UInt16 [,] auiAllPathsShortestPath;
        DateTime oStartTime = DateTime.Now;

        Boolean bReturn =
            m_oAllPathsShortestPathCalculator.TryCalculateGraphMetrics(
                oGraph, null, out auiAllPathsShortestPath);

        Assert.IsTrue(bReturn);

        Trace.TraceInformation(

            "TimeTryCalculateGraphMetrics: {0} seconds."
            ,
            (DateTime.Now - oStartTime).TotalSeconds
            );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected AllPathsShortestPathCalculator m_oAllPathsShortestPathCalculator;
}

}
