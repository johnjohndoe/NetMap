
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.ExcelTemplate;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: DynamicFilterParametersTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="DynamicFilterParameters" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class DynamicFilterParametersTest : Object
{
    //*************************************************************************
    //  Constructor: DynamicFilterParametersTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="DynamicFilterParametersTest" /> class.
    /// </summary>
    //*************************************************************************

    public DynamicFilterParametersTest()
    {
        m_oDynamicFilterParameters = null;
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
        m_oDynamicFilterParameters = new DynamicFilterParameters(ColumnName);
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
        m_oDynamicFilterParameters = null;
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
        Assert.AreEqual(ColumnName, m_oDynamicFilterParameters.ColumnName);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    ///
    protected const String ColumnName = "The column";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected DynamicFilterParameters m_oDynamicFilterParameters;
}

}
