
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.ExcelTemplate;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: BooleanConverterTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="BooleanConverter" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class BooleanConverterTest : Object
{
    //*************************************************************************
    //  Constructor: BooleanConverterTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="BooleanConverterTest" /> class.
    /// </summary>
    //*************************************************************************

    public BooleanConverterTest()
    {
        m_oBooleanConverter = null;
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
        m_oBooleanConverter = new BooleanConverter();
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
        m_oBooleanConverter = null;
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
        Boolean bValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "yes (1)",
            "YeS (1)",
            "1",
            } )
        {
            Assert.IsTrue( m_oBooleanConverter.TryWorkbookToGraph(
                sValueWorkbook, out bValueGraph) );

            Assert.IsTrue(bValueGraph);
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
        Boolean bValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "no (0)",
            "nO (0)",
            "0",
            } )
        {
            Assert.IsTrue( m_oBooleanConverter.TryWorkbookToGraph(
                sValueWorkbook, out bValueGraph) );

            Assert.IsFalse(bValueGraph);
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
        Boolean bValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "",
            " ",
            " 0",
            "1 ",
            "x",
            " no",
            "yes ",
            } )
        {
            Assert.IsFalse( m_oBooleanConverter.TryWorkbookToGraph(
                sValueWorkbook, out bValueGraph) );
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
        Assert.AreEqual( "Yes (1)",
            m_oBooleanConverter.GraphToWorkbook(true) );

        Assert.AreEqual( "No (0)",
            m_oBooleanConverter.GraphToWorkbook(false) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected BooleanConverter m_oBooleanConverter;
}

}
