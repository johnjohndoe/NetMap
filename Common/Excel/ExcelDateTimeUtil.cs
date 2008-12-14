
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: ExcelDateTimeUtil
//
/// <summary>
///	Static utility methods for working with Excel dates and times.
/// </summary>
//*****************************************************************************

public static class ExcelDateTimeUtil : Object
{
	//*************************************************************************
	//	Constructor: ExcelDateTimeUtil()
	//
	/// <summary>
	///	Static constructor for the <see cref="ExcelDateTimeUtil" /> class.
	/// </summary>
	//*************************************************************************

	static ExcelDateTimeUtil()
	{
		// Excel uses zero to represent December 31, 1899, midnight.

		m_oExcelBaseDate = new DateTime(1899, 12, 31);

		// There is an intentional bug in Excel involving February 29, 1900,
		// which isn't a real date.  The result is an off-by-one error for
		// dates of March 1, 1900 and later.  See "Excel's Intentional Date
		// Bug" in "Excel 2007: The Missing Manual," Matthew MacDonald, p.321.
		//
		// Here is how ExcelDateTimeUtil handles the bug.  When converting a
		// DateTime to a Decimal, these are the results:
		//
		// 2/28/1900 -> 59.0
		// 2/29/1900 -> 60.0  (Date doesn't exist)
		// 3/01/1900 -> 61.0
		//
		// When converting a Decimal to a DateTime, these are the results:
		//
		// 59.0 -> 2/28/1900
		// 60.0 -> 3/01/1900
		// 61.0 -> 3/01/1900

		m_oExcelOffByOneDate = new DateTime(1900, 3, 1);

		AssertValid();
	}

	//*************************************************************************
	//	Method: DateTimeToExcelDecimal()
	//
	/// <summary>
    /// Converts a DateTime to Excel decimal format.
	/// </summary>
	///
	/// <param name="dateTime">
	/// The DateTime to convert.  Sample: 1/1/2008 3 PM.
	/// </param>
	///
	/// <returns>
	/// The DateTime in Excel decimal format.  Sample: 39448.625.
	/// </returns>
	//*************************************************************************

	public static Decimal
	DateTimeToExcelDecimal
	(
		DateTime dateTime
	)
	{
		AssertValid();

		Int32 iDays = (Int32)( (dateTime - m_oExcelBaseDate).TotalDays );
		Int32 iSeconds = (Int32)dateTime.TimeOfDay.TotalSeconds;

		Debug.Assert(iDays >= 0);
		Debug.Assert(iSeconds >= 0);

		if (dateTime >= m_oExcelOffByOneDate)
		{
			// Compensate for Excel's off-by-one date bug by adding 1.

			iDays++;
		}

		return ( (Decimal)iDays
			+ (Decimal)( (Double)iSeconds / SecondsPerDay )
			);
	}

	//*************************************************************************
	//	Method: ExcelDecimalToDateTime()
	//
	/// <summary>
    /// Converts an Excel decimal date/time to a DateTime.
	/// </summary>
	///
	/// <param name="excelDecimal">
	/// The Excel decimal date/time to convert.  Sample: 39448.625.
	/// </param>
	///
	/// <returns>
	/// A DateTime.  Sample: 1/1/2008 3 PM.
	/// </returns>
	//*************************************************************************

	public static DateTime
	ExcelDecimalToDateTime
	(
		Decimal excelDecimal
	)
	{
		AssertValid();

		Decimal decDays = Decimal.Truncate(excelDecimal);
		Decimal decFractionOfDay = MathUtil.GetDecimalFraction(excelDecimal);
		Double dSeconds = (Double)decFractionOfDay * SecondsPerDay;

		Debug.Assert(decDays >= 0);
		Debug.Assert(decFractionOfDay >= 0);
		Debug.Assert(dSeconds >= 0);

		if (decDays >= ExcelOffByOneDays)
		{
			// Compensate for Excel's off-by-one date bug by subtracting 1.

			decDays--;
		}

        Debug.Assert(decDays >= 0);

		return ( m_oExcelBaseDate
			+ TimeSpan.FromDays( (Double)decDays )
			+ TimeSpan.FromSeconds(dSeconds)
			);
	}


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public static void
    AssertValid()
    {
		// m_oExcelBaseDate
		// m_oExcelOffByOneDate
    }


    //*************************************************************************
    //  Private constants
    //*************************************************************************

	/// Number of seconds in a day.

	private const Double SecondsPerDay = 24 * 60 * 60;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Date that Excel uses for the zero point.

	private static DateTime m_oExcelBaseDate;

	/// Date before which the Excel off-by-one bug occurs.

	private static DateTime m_oExcelOffByOneDate;

	/// Number of days between m_oExcelBaseDate and m_oExcelOffByOneDate.

	private const Int32 ExcelOffByOneDays = 61;
}

}
