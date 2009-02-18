
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.ExcelTemplate;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: AlphaConverterTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="AlphaConverter" />
/// class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class AlphaConverterTest : Object
{
    //*************************************************************************
    //  Constructor: AlphaConverterTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AlphaConverterTest" /> class.
    /// </summary>
    //*************************************************************************

    public AlphaConverterTest()
    {
        m_oAlphaConverter = null;
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
        m_oAlphaConverter = new AlphaConverter();
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
        m_oAlphaConverter = null;
    }

    //*************************************************************************
    //  Method: TestWorkbookToGraph()
    //
    /// <summary>
    /// Tests the WorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToGraph()
    {
        // Minimum.

        Single fAlphaWorkbook = AlphaConverter.MinimumAlphaWorkbook;

        Byte btExpectedAlphaGraph = 0;

        Byte btAlphaGraph = m_oAlphaConverter.WorkbookToGraph(fAlphaWorkbook);

        Assert.AreEqual(btExpectedAlphaGraph, btAlphaGraph);
    }

    //*************************************************************************
    //  Method: TestWorkbookToGraph2()
    //
    /// <summary>
    /// Tests the WorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToGraph2()
    {
        // Maximum.

        Single fAlphaWorkbook = AlphaConverter.MaximumAlphaWorkbook;

        Byte btExpectedAlphaGraph = 255;

        Byte btAlphaGraph = m_oAlphaConverter.WorkbookToGraph(fAlphaWorkbook);

        Assert.AreEqual(btExpectedAlphaGraph, btAlphaGraph);
    }

    //*************************************************************************
    //  Method: TestWorkbookToGraph3()
    //
    /// <summary>
    /// Tests the WorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToGraph3()
    {
        // Midpoint.

        Single fAlphaWorkbook = AlphaConverter.MinimumAlphaWorkbook
            + (AlphaConverter.MaximumAlphaWorkbook -
                AlphaConverter.MinimumAlphaWorkbook) / 2;

        Byte btExpectedAlphaGraph = (Byte)(0 + (255 - 0) / 2);

        Byte btAlphaGraph = m_oAlphaConverter.WorkbookToGraph(fAlphaWorkbook);

        Assert.AreEqual(btExpectedAlphaGraph, btAlphaGraph);
    }

    //*************************************************************************
    //  Method: TestWorkbookToGraph4()
    //
    /// <summary>
    /// Tests the WorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToGraph4()
    {
        // Below minimum.

        Single fAlphaWorkbook = AlphaConverter.MinimumAlphaWorkbook - 1;

        Byte btExpectedAlphaGraph = 0;

        Byte btAlphaGraph = m_oAlphaConverter.WorkbookToGraph(fAlphaWorkbook);

        Assert.AreEqual(btExpectedAlphaGraph, btAlphaGraph);
    }

    //*************************************************************************
    //  Method: TestWorkbookToGraph5()
    //
    /// <summary>
    /// Tests the WorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToGraph5()
    {
        // Above maximum.

        Single fAlphaWorkbook = AlphaConverter.MaximumAlphaWorkbook + 1;

        Byte btExpectedAlphaGraph = 255;

        Byte btAlphaGraph = m_oAlphaConverter.WorkbookToGraph(fAlphaWorkbook);

        Assert.AreEqual(btExpectedAlphaGraph, btAlphaGraph);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected AlphaConverter m_oAlphaConverter;
}

}
