
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.ExcelTemplate;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: NumericFilterParametersTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="NumericFilterParameters" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class NumericFilterParametersTest : Object
{
    //*************************************************************************
    //  Constructor: NumericFilterParametersTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="NumericFilterParametersTest" /> class.
    /// </summary>
    //*************************************************************************

    public NumericFilterParametersTest()
    {
        m_oNumericFilterParameters = null;
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
        m_oNumericFilterParameters = new NumericFilterParameters(
			ColumnName, MinimumCellValue, MaximumCellValue);
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
        m_oNumericFilterParameters = null;
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
		Assert.AreEqual(ColumnName, m_oNumericFilterParameters.ColumnName);

		Assert.AreEqual(MinimumCellValue,
			m_oNumericFilterParameters.MinimumCellValue);

		Assert.AreEqual(MaximumCellValue,
			m_oNumericFilterParameters.MaximumCellValue);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	///
	protected const String ColumnName = "The column";
	///
	protected const Double MinimumCellValue = -123.456;
	///
	protected const Double MaximumCellValue = 987.654;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected NumericFilterParameters m_oNumericFilterParameters;
}

}
