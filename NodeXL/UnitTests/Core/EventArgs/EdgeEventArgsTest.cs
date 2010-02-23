
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: EdgeEventArgsTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="EdgeEventArgs" />
/// class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class EdgeEventArgsTest : Object
{
    //*************************************************************************
    //  Constructor: EdgeEventArgsTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EdgeEventArgsTest" />
    /// class.
    /// </summary>
    //*************************************************************************

    public EdgeEventArgsTest()
    {
        m_oEdgeEventArgs = null;
        m_oEdge = null;
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
        IVertex oVertex1 = new Vertex();
        IVertex oVertex2 = new Vertex();

        IGraph oGraph = new Graph();

        oGraph.Vertices.Add(oVertex1);
        oGraph.Vertices.Add(oVertex2);

        m_oEdge = new Edge(oVertex1, oVertex2, true);

        m_oEdgeEventArgs = new EdgeEventArgs(m_oEdge);
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
        m_oEdgeEventArgs = null;
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
        Assert.AreEqual(m_oEdge, m_oEdgeEventArgs.Edge);
    }

    //*************************************************************************
    //  Method: TestConstructorBad()
    //
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestConstructorBad()
    {
        // Null argument.

        try
        {
            m_oEdgeEventArgs = new EdgeEventArgs(null);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core."
                + "EdgeEventArgs.Constructor: edge argument can't be null.\r\n"
                + "Parameter name: edge"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected EdgeEventArgs m_oEdgeEventArgs;

    /// Edge in m_oEdgeEventArgs.

    protected IEdge m_oEdge;
}

}
