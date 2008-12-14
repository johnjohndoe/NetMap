
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.ExcelTemplate;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: DateTimeFilterParametersTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="DateTimeFilterParameters" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class DateTimeFilterParametersTest : Object
{
    //*************************************************************************
    //  Constructor: DateTimeFilterParametersTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="DateTimeFilterParametersTest" /> class.
    /// </summary>
    //*************************************************************************

    public DateTimeFilterParametersTest()
    {
        m_oDateTimeFilterParameters = null;
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
        m_oDateTimeFilterParameters = new DateTimeFilterParameters(
			ColumnName, MinimumCellValue, MaximumCellValue, Format);
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
        m_oDateTimeFilterParameters = null;
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
		Assert.AreEqual(ColumnName, m_oDateTimeFilterParameters.ColumnName);

		Assert.AreEqual(MinimumCellValue,
			m_oDateTimeFilterParameters.MinimumCellValue);

		Assert.AreEqual(MaximumCellValue,
			m_oDateTimeFilterParameters.MaximumCellValue);

		Assert.AreEqual(Format,
			m_oDateTimeFilterParameters.Format);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	///
	protected const String ColumnName = "The column";
	///
	protected const Double MinimumCellValue = 123.456789;
	///
	protected const Double MaximumCellValue = 987.654321;
	///
	protected const ExcelColumnFormat Format = ExcelColumnFormat.Time;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected DateTimeFilterParameters m_oDateTimeFilterParameters;
}

}
