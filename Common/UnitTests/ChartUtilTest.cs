using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Research.CommunityTechnologies.ChartLib;

namespace Microsoft.Research.CommunityTechnologies.Common.UnitTests
{
//*****************************************************************************
//  Class: ChartUtilTest
//
/// <summary>
/// This is a test fixture for the ChartUtil class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class ChartUtilTest : Object
{
    //*************************************************************************
    //  Constructor: ChartUtilTest()
    //
    /// <summary>
    /// Initializes a new instance of the ChartUtilTest class.
    /// </summary>
    //*************************************************************************

    public ChartUtilTest()
    {
		// (Do nothing.)
    }

	//*************************************************************************
	//	Method: SetUp()
	//
	/// <summary>
	/// Gets run before each test.
	/// </summary>
	//*************************************************************************

	[TestInitializeAttribute]

	public void
	SetUp()
	{
		// (Do nothing.)
	}

	//*************************************************************************
	//	Method: TearDown()
	//
	/// <summary>
	/// Gets run after each test.
	/// </summary>
	//*************************************************************************

	[TestCleanupAttribute]

	public void
	TearDown()
	{
		// (Do nothing.)
	}

	//*************************************************************************
	//	Method: TestGetLogAxisGridlineValues()
	//
	/// <summary>
	/// Tests the GetLogAxisGridlineValues method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestGetLogAxisGridlineValues()
	{
		Single [] afGridlineValues =
			ChartUtil.GetLogAxisGridlineValues(0.3F, 103.0F);

		Assert.AreEqual(5, afGridlineValues.Length);
		Assert.AreEqual( 0.1F, afGridlineValues[0] );
		Assert.AreEqual( 1.0F, afGridlineValues[1] );
		Assert.AreEqual( 10.0F, afGridlineValues[2] );
		Assert.AreEqual( 100.0F, afGridlineValues[3] );
		Assert.AreEqual( 1000.0F, afGridlineValues[4] );
	}

	//*************************************************************************
	//	Method: TestGetLogAxisGridlineValues2()
	//
	/// <summary>
	/// Tests the GetLogAxisGridlineValues method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestGetLogAxisGridlineValues2()
	{
		Single [] afGridlineValues =
			ChartUtil.GetLogAxisGridlineValues(1.0F, 1.0F);

		Assert.AreEqual(2, afGridlineValues.Length);
		Assert.AreEqual( 1.0F, afGridlineValues[0] );
		Assert.AreEqual( 10.0F, afGridlineValues[1] );
	}

	//*************************************************************************
	//	Method: TestGetLogAxisGridlineValues3()
	//
	/// <summary>
	/// Tests the GetLogAxisGridlineValues method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestGetLogAxisGridlineValues3()
	{
		Single [] afGridlineValues =
			ChartUtil.GetLogAxisGridlineValues(0.1F, 0.1F);

		Assert.AreEqual(2, afGridlineValues.Length);
		Assert.AreEqual( 0.1F, afGridlineValues[0] );
		Assert.AreEqual( 1.0F, afGridlineValues[1] );
	}

	//*************************************************************************
	//	Method: TestGetLogAxisGridlineValues4()
	//
	/// <summary>
	/// Tests the GetLogAxisGridlineValues method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestGetLogAxisGridlineValues4()
	{
		Single [] afGridlineValues =
			ChartUtil.GetLogAxisGridlineValues(0.3F, 0.3F);

		Assert.AreEqual(2, afGridlineValues.Length);
		Assert.AreEqual( 0.1F, afGridlineValues[0] );
		Assert.AreEqual( 1.0F, afGridlineValues[1] );
	}

	//*************************************************************************
	//	Method: TestGetLogAxisGridlineValues5()
	//
	/// <summary>
	/// Tests the GetLogAxisGridlineValues method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestGetLogAxisGridlineValues5()
	{
		Single [] afGridlineValues =
			ChartUtil.GetLogAxisGridlineValues(0.119F, 138495.24F);

		Assert.AreEqual(8, afGridlineValues.Length);
		Assert.AreEqual( 0.1F, afGridlineValues[0] );
		Assert.AreEqual( 1.0F, afGridlineValues[1] );
		Assert.AreEqual( 10.0F, afGridlineValues[2] );
		Assert.AreEqual( 100.0F, afGridlineValues[3] );
		Assert.AreEqual( 1000.0F, afGridlineValues[4] );
		Assert.AreEqual( 10000.0F, afGridlineValues[5] );
		Assert.AreEqual( 100000.0F, afGridlineValues[6] );
		Assert.AreEqual( 1000000.0F, afGridlineValues[7] );
	}

	//*************************************************************************
	//	Method: TestGetLogAxisGridlineValuesBad()
	//
	/// <summary>
	/// Tests the constructor.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]
	[ExpectedException(typeof(InvalidOperationException))]

	public void
	TestGetLogAxisGridlineValuesBad()
	{
		try
		{
			Single [] afGridlineValues =
				ChartUtil.GetLogAxisGridlineValues(10.0F, 1.0F);
		}
		catch (InvalidOperationException oInvalidOperationException)
		{
			Assert.AreEqual(

				"ChartUtil.GetLogAxisGridlineValues: Maximum value must be"
				+ " >= minimum value."
				,
				oInvalidOperationException.Message
				);

			throw;
		}
	}

	//*************************************************************************
	//	Method: TestGetLogAxisGridlineValuesBad2()
	//
	/// <summary>
	/// Tests the constructor.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]
	[ExpectedException(typeof(InvalidOperationException))]

	public void
	TestGetLogAxisGridlineValuesBad2()
	{
        try
        {
            Single[] afGridlineValues =
                ChartUtil.GetLogAxisGridlineValues(-0.1F, 1.0F);
        }
        catch (InvalidOperationException oInvalidOperationException)
        {
            Assert.AreEqual(

                "ChartUtil.GetLogAxisGridlineValues: Minimum value must be"
                + " > 0 when using log scaling."
                ,
                oInvalidOperationException.Message
                );

            throw;
        }
	}

	//*************************************************************************
	//	Method: TestGetLogAxisGridlineValuesBad3()
	//
	/// <summary>
	/// Tests the constructor.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]
	[ExpectedException(typeof(InvalidOperationException))]

	public void
	TestGetLogAxisGridlineValuesBad3()
	{
		try
		{
			Single [] afGridlineValues =
				ChartUtil.GetLogAxisGridlineValues(0F, 1.0F);
		}
		catch (InvalidOperationException oInvalidOperationException)
		{
			Assert.AreEqual(

				"ChartUtil.GetLogAxisGridlineValues: Minimum value must be"
				+ " > 0 when using log scaling."
				,
				oInvalidOperationException.Message
				);

			throw;
		}
	}


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
