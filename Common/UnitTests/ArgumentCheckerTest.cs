using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.Research.CommunityTechnologies.Common.UnitTests
{
//*****************************************************************************
//  Class: ArgumentCheckerTest
//
/// <summary>
/// This is a test fixture for the ArgumentChecker class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class ArgumentCheckerTest : Object
{
    //*************************************************************************
    //  Constructor: ArgumentCheckerTest()
    //
    /// <summary>
    /// Initializes a new instance of the ArgumentCheckerTest class.
    /// </summary>
    //*************************************************************************

    public ArgumentCheckerTest()
    {
		m_oArgumentChecker = null;
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
		m_oArgumentChecker = new ArgumentChecker("ClassName");
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
		m_oArgumentChecker = null;
	}

	//*************************************************************************
	//	Method: TestCheckPropertyNotNull()
	//
	/// <summary>
	/// Tests the CheckPropertyNotNull method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestCheckPropertyNotNull()
	{
		m_oArgumentChecker.CheckPropertyNotNull("PropertyName", "not null");
	}

	//*************************************************************************
	//	Method: TestCheckPropertyNotNullBad()
	//
	/// <summary>
	/// Tests the CheckPropertyNotNull method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]
	[ExpectedException(typeof(ApplicationException))]

	public void
	TestCheckPropertyNotNullBad()
	{
		// Should throw an exception.

		try
		{
			m_oArgumentChecker.CheckPropertyNotNull("PropertyName", null);
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual(

				"ClassName.PropertyName: Can't be null."
				,
				oApplicationException.Message
				);

			throw;
		}
	}

	//*************************************************************************
	//	Method: TestCheckPropertyNotEmpty()
	//
	/// <summary>
	/// Tests the CheckPropertyNotEmpty method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestCheckPropertyNotEmpty()
	{
		m_oArgumentChecker.CheckPropertyNotEmpty("PropertyName", "not empty");
	}

	//*************************************************************************
	//	Method: TestCheckPropertyNotEmptyBad()
	//
	/// <summary>
	/// Tests the CheckPropertyNotEmpty method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]
	[ExpectedException(typeof(ApplicationException))]

	public void
	TestCheckPropertyNotEmptyBad()
	{
		// Should throw an exception.

		try
		{
			m_oArgumentChecker.CheckPropertyNotEmpty("PropertyName", null);
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual(

				"ClassName.PropertyName: Can't be null."
				,
				oApplicationException.Message
				);

			throw;
		}
	}

	//*************************************************************************
	//	Method: TestCheckPropertyNotEmptyBad2()
	//
	/// <summary>
	/// Tests the CheckPropertyNotEmpty method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]
	[ExpectedException(typeof(ApplicationException))]

	public void
	TestCheckPropertyNotEmptyBad2()
	{
		// Should throw an exception.

		try
		{
			m_oArgumentChecker.CheckPropertyNotEmpty("PropertyName", "");
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual(

				"ClassName.PropertyName: Must have a length greater than zero."
				,
				oApplicationException.Message
				);

			throw;
		}
	}

	//*************************************************************************
	//	Method: TestCheckPropertyPositive()
	//
	/// <summary>
	/// Tests the CheckPropertyPositive method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestCheckPropertyPositive()
	{
		m_oArgumentChecker.CheckPropertyPositive("PropertyName", 1);
	}

	//*************************************************************************
	//	Method: TestCheckPropertyPositiveBad()
	//
	/// <summary>
	/// Tests the CheckPropertyPositive method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]
	[ExpectedException(typeof(ApplicationException))]

	public void
	TestCheckPropertyPositiveBad()
	{
		try
		{
			m_oArgumentChecker.CheckPropertyPositive("PropertyName", 0);
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual(

				"ClassName.PropertyName: Must be greater than zero."
				,
				oApplicationException.Message
				);

			throw;
		}
	}

	//*************************************************************************
	//	Method: TestCheckPropertyPositiveBad2()
	//
	/// <summary>
	/// Tests the CheckPropertyPositive method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]
	[ExpectedException(typeof(ApplicationException))]

	public void
	TestCheckPropertyPositiveBad2()
	{
		try
		{
			m_oArgumentChecker.CheckPropertyPositive("PropertyName", -1);
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual(

				"ClassName.PropertyName: Must be greater than zero."
				,
				oApplicationException.Message
				);

			throw;
		}
	}

	//*************************************************************************
	//	Method: TestCheckPropertyNotEqual()
	//
	/// <summary>
	/// Tests the CheckPropertyNotEqual method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestCheckPropertyNotEqual()
	{
		m_oArgumentChecker.CheckPropertyNotEqual("PropertyName", "a", "b");
	}

	//*************************************************************************
	//	Method: TestCheckPropertyNotEqual2()
	//
	/// <summary>
	/// Tests the CheckPropertyNotEqual method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestCheckPropertyNotEqual2()
	{
		m_oArgumentChecker.CheckPropertyNotEqual("PropertyName", 8394, 372);
	}

	//*************************************************************************
	//	Method: TestCheckPropertyNotEqualBad()
	//
	/// <summary>
	/// Tests the CheckPropertyNotEqual method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]
	[ExpectedException(typeof(ApplicationException))]

	public void
	TestCheckPropertyNotEqualBad()
	{
		// Should throw an exception.

		try
		{
			m_oArgumentChecker.CheckPropertyNotEqual("PropertyName", "a", "a");
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual(

				"ClassName.PropertyName: Can't be a."
				,
				oApplicationException.Message
				);

			throw;
		}
	}

	//*************************************************************************
	//	Method: TestCheckPropertyNotEqualBad2()
	//
	/// <summary>
	/// Tests the CheckPropertyNotEqual method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]
	[ExpectedException(typeof(ApplicationException))]

	public void
	TestCheckPropertyNotEqualBad2()
	{
		// Should throw an exception.

		try
		{
			m_oArgumentChecker.CheckPropertyNotEqual(
				"PropertyName", -903, -903);
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual(

				"ClassName.PropertyName: Can't be -903."
				,
				oApplicationException.Message
				);

			throw;
		}
	}

	//*************************************************************************
	//	Method: TestCheckPropertyInRange()
	//
	/// <summary>
	/// Tests the CheckPropertyInRange method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestCheckPropertyInRange()
	{
		m_oArgumentChecker.CheckPropertyInRange("PropertyName", 4, -10, 10);
	}

	//*************************************************************************
	//	Method: TestCheckPropertyInRange2()
	//
	/// <summary>
	/// Tests the CheckPropertyInRange method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestCheckPropertyInRange2()
	{
		m_oArgumentChecker.CheckPropertyInRange("PropertyName", -10, -10, 10);
	}

	//*************************************************************************
	//	Method: TestCheckPropertyInRange3()
	//
	/// <summary>
	/// Tests the CheckPropertyInRange method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestCheckPropertyInRange3()
	{
		m_oArgumentChecker.CheckPropertyInRange("PropertyName", 10, -10, 10);
	}

	//*************************************************************************
	//	Method: TestCheckPropertyInRangeBad()
	//
	/// <summary>
	/// Tests the CheckPropertyInRange method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]
	[ExpectedException(typeof(ApplicationException))]

	public void
	TestCheckPropertyInRangeBad()
	{
		// Should throw an exception.

		try
		{
			m_oArgumentChecker.CheckPropertyInRange(
				"PropertyName", -11, -10, 10);
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual(

				"ClassName.PropertyName: Must be between -10 and 10."
				,
				oApplicationException.Message
				);

			throw;
		}
	}

	//*************************************************************************
	//	Method: TestCheckPropertyInRangeBad2()
	//
	/// <summary>
	/// Tests the CheckPropertyInRange method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]
	[ExpectedException(typeof(ApplicationException))]

	public void
	TestCheckPropertyInRangeBad2()
	{
		// Should throw an exception.

		try
		{
			m_oArgumentChecker.CheckPropertyInRange(
				"PropertyName", 11, -10, 10);
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual(

				"ClassName.PropertyName: Must be between -10 and 10."
				,
				oApplicationException.Message
				);

			throw;
		}
	}

	//*************************************************************************
	//	Method: TestCheckPropertyIsDefined()
	//
	/// <summary>
	/// Tests the CheckPropertyIsDefined method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestCheckPropertyIsDefined()
	{
		m_oArgumentChecker.CheckPropertyIsDefined("PropertyName",
			DayOfWeek.Monday, typeof(DayOfWeek) );
	}

	//*************************************************************************
	//	Method: TestCheckPropertyIsDefined2()
	//
	/// <summary>
	/// Tests the CheckPropertyIsDefined method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestCheckPropertyIsDefined2()
	{
		m_oArgumentChecker.CheckPropertyIsDefined("PropertyName",
			DayOfWeek.Sunday, typeof(DayOfWeek) );
	}

	//*************************************************************************
	//	Method: TestCheckPropertyIsDefinedBad()
	//
	/// <summary>
	/// Tests the CheckPropertyIsDefined method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]
	[ExpectedException(typeof(ApplicationException))]

	public void
	TestCheckPropertyIsDefinedBad()
	{
		// Should throw an exception.

		try
		{
			m_oArgumentChecker.CheckPropertyIsDefined("PropertyName",
				(DayOfWeek)(-99), typeof(DayOfWeek) );
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual(

				"ClassName.PropertyName: Must be a member of the DayOfWeek"
					+ " enumeration."
				,
				oApplicationException.Message
				);

			throw;
		}
	}

	//*************************************************************************
	//	Method: TestCheckArgumentNotNull()
	//
	/// <summary>
	/// Tests the CheckArgumentNotNull method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestCheckArgumentNotNull()
	{
		m_oArgumentChecker.CheckArgumentNotNull("MethodName", "ArgumentName",
			"not null");
	}

	//*************************************************************************
	//	Method: TestCheckArgumentNotNullBad()
	//
	/// <summary>
	/// Tests the CheckArgumentNotNull method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]
	[ExpectedException(typeof(ArgumentNullException))]

	public void
	TestCheckArgumentNotNullBad()
	{
		// Should throw an exception.

		try
		{
			m_oArgumentChecker.CheckArgumentNotNull(
				"MethodName", "ArgumentName", null);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"ClassName.MethodName: ArgumentName argument can't be null.\r\n"
					+ "Parameter name: ArgumentName"
				,
				oArgumentNullException.Message
				);

			throw;
		}
	}

	//*************************************************************************
	//	Method: TestCheckArgumentNotEmpty()
	//
	/// <summary>
	/// Tests the CheckArgumentNotEmpty method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestCheckArgumentNotEmpty()
	{
		m_oArgumentChecker.CheckArgumentNotEmpty("MethodName", "ArgumentName",
			"not empty");
	}

	//*************************************************************************
	//	Method: TestCheckArgumentNotEmptyBad()
	//
	/// <summary>
	/// Tests the CheckArgumentNotEmpty method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]
	[ExpectedException(typeof(ArgumentNullException))]

	public void
	TestCheckArgumentNotEmptyBad()
	{
		// Should throw an exception.

		try
		{
			m_oArgumentChecker.CheckArgumentNotEmpty(
				"MethodName", "ArgumentName", null);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"ClassName.MethodName: ArgumentName argument can't be null.\r\n"
				+ "Parameter name: ArgumentName"
				,
				oArgumentNullException.Message
				);

			throw;
		}
	}

	//*************************************************************************
	//	Method: TestCheckArgumentNotEmptyBad2()
	//
	/// <summary>
	/// Tests the CheckArgumentNotEmpty method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]
	[ExpectedException(typeof(ArgumentException))]

	public void
	TestCheckArgumentNotEmptyBad2()
	{
		// Should throw an exception.

		try
		{
			m_oArgumentChecker.CheckArgumentNotEmpty(
				"MethodName", "ArgumentName", "");
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				"ClassName.MethodName: ArgumentName argument must have a"
				+ " length greater than zero.\r\nParameter name: ArgumentName"
				,
				oArgumentException.Message
				);

			throw;
		}
	}

	//*************************************************************************
	//	Method: TestCheckArgumentPositive()
	//
	/// <summary>
	/// Tests the CheckArgumentPositive method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestCheckArgumentPositive()
	{
		m_oArgumentChecker.CheckArgumentPositive("MethodName", "ArgumentName",
			1);
	}

	//*************************************************************************
	//	Method: TestCheckArgumentPositiveBad()
	//
	/// <summary>
	/// Tests the CheckArgumentPositive method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]
	[ExpectedException(typeof(ArgumentException))]

	public void
	TestCheckArgumentPositiveBad()
	{
		// Should throw an exception.

		try
		{
			m_oArgumentChecker.CheckArgumentPositive(
				"MethodName", "ArgumentName", 0);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				"ClassName.MethodName: ArgumentName argument must be greater"
					+ " than zero.\r\nParameter name: ArgumentName"
				,
				oArgumentException.Message
				);

			throw;
		}
	}

	//*************************************************************************
	//	Method: TestCheckArgumentPositiveBad2()
	//
	/// <summary>
	/// Tests the CheckArgumentPositive method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]
	[ExpectedException(typeof(ArgumentException))]

	public void
	TestCheckArgumentPositiveBad2()
	{
		// Should throw an exception.

		try
		{
			m_oArgumentChecker.CheckArgumentPositive(
				"MethodName", "ArgumentName", -1);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				"ClassName.MethodName: ArgumentName argument must be greater"
				+ " than zero.\r\nParameter name: ArgumentName"
				,
				oArgumentException.Message
				);

			throw;
		}
	}


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Object to test.

	private ArgumentChecker m_oArgumentChecker;
}

}
