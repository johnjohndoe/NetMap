using System;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//	Class: MathUtil
//
/// <summary>
/// Utility methods for working with numbers.
/// </summary>
///
///	<remarks>
/// All methods are static.
///	</remarks>
//*****************************************************************************

public static class MathUtil
{
	//*************************************************************************
	//	Method: CountDecimalPlaces()
	//
	/// <summary>
	///	Counts the number of decimal places in a Decimal.
	/// </summary>
	///
	/// <param name="theDecimal">
	/// The Decimal to count decimal places for.  Sample: 1.2345.
	/// </param>
	///
	/// <returns>
	/// The number of decimal places in <paramref name="theDecimal" />.
	/// Sample: 4.
	/// </returns>
	//*************************************************************************

	public static Int32
	CountDecimalPlaces
	(
		Decimal theDecimal
	)
	{
		Int32 iDecimalPlaces = 0;

		while (true)
		{
			theDecimal = GetDecimalFraction(theDecimal);

			if (theDecimal == 0)
			{
				break;
			}

			theDecimal *= 10M;
			iDecimalPlaces++;
		}

		return (iDecimalPlaces);
	}

	//*************************************************************************
	//	Method: GetDecimalFraction()
	//
	/// <summary>
	///	Returns the fractional part of a Decimal.
	/// </summary>
	///
	/// <param name="theDecimal">
	/// The Decimal to get the fractional part for.  Sample: 1.2345.
	/// </param>
	///
	/// <returns>
	/// The fractional part of <paramref name="theDecimal" />.  Sample: 0.2345.
	/// </returns>
	//*************************************************************************

	public static Decimal
	GetDecimalFraction
	(
		Decimal theDecimal
	)
	{
		return ( theDecimal - Decimal.Truncate(theDecimal) );
	}
}

}
