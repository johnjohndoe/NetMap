
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.ExcelTemplate;
using Microsoft.NodeXL.Visualization.Wpf;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: VertexDrawingPrecedenceConverterTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="VertexDrawingPrecedenceConverter" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class VertexDrawingPrecedenceConverterTest : Object
{
    //*************************************************************************
    //  Constructor: VertexDrawingPrecedenceConverterTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="VertexDrawingPrecedenceConverterTest" /> class.
    /// </summary>
    //*************************************************************************

    public VertexDrawingPrecedenceConverterTest()
    {
        m_oVertexDrawingPrecedenceConverter = null;
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
        m_oVertexDrawingPrecedenceConverter =
            new VertexDrawingPrecedenceConverter();
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
        m_oVertexDrawingPrecedenceConverter = null;
    }

    //*************************************************************************
    //  Method: TestTryWorkbookToGraph()
    //
    /// <summary>
    /// Tests the TryWorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryWorkbookToGraph()
    {
        VertexDrawingPrecedence eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "shape (1)",
            "sHape (1)",
            "1",
            } )
        {
            Assert.IsTrue(
                m_oVertexDrawingPrecedenceConverter.TryWorkbookToGraph(
                    sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(VertexDrawingPrecedence.Shape, eValueGraph);
        }
    }

    //*************************************************************************
    //  Method: TestTryWorkbookToGraph2()
    //
    /// <summary>
    /// Tests the TryWorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryWorkbookToGraph2()
    {
        VertexDrawingPrecedence eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "image (2)",
            "iMage (2)",
            "2",
            } )
        {
            Assert.IsTrue(
                m_oVertexDrawingPrecedenceConverter.TryWorkbookToGraph(
                    sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(VertexDrawingPrecedence.Image, eValueGraph);
        }
    }

    //*************************************************************************
    //  Method: TestTryWorkbookToGraph3()
    //
    /// <summary>
    /// Tests the TryWorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryWorkbookToGraph3()
    {
        VertexDrawingPrecedence eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "primary label (3)",
            "PriMary label (3)",
            "3",
            } )
        {
            Assert.IsTrue(
                m_oVertexDrawingPrecedenceConverter.TryWorkbookToGraph(
                    sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(VertexDrawingPrecedence.PrimaryLabel, eValueGraph);
        }
    }

    //*************************************************************************
    //  Method: TestTryWorkbookToGraph4()
    //
    /// <summary>
    /// Tests the TryWorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryWorkbookToGraph4()
    {
        VertexDrawingPrecedence eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "",
            " shape (1)",
            "x",
            " 1",
            } )
        {
            Assert.IsFalse(
                m_oVertexDrawingPrecedenceConverter.TryWorkbookToGraph(
                    sValueWorkbook, out eValueGraph) );
        }
    }

    //*************************************************************************
    //  Method: TestGraphToWorkbook()
    //
    /// <summary>
    /// Tests the GraphToWorkbook() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraphToWorkbook()
    {
        Assert.AreEqual( "Shape (1)",
            m_oVertexDrawingPrecedenceConverter.GraphToWorkbook(
                VertexDrawingPrecedence.Shape) );

        Assert.AreEqual( "Image (2)",
            m_oVertexDrawingPrecedenceConverter.GraphToWorkbook(
                VertexDrawingPrecedence.Image) );

        Assert.AreEqual( "Primary Label (3)",
            m_oVertexDrawingPrecedenceConverter.GraphToWorkbook(
                VertexDrawingPrecedence.PrimaryLabel) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected VertexDrawingPrecedenceConverter
        m_oVertexDrawingPrecedenceConverter;
}

}
