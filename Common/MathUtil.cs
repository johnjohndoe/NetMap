using System;
using System.Globalization;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: MathUtil
//
/// <summary>
/// Utility methods for working with numbers.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class MathUtil
{
    //*************************************************************************
    //  Method: CountDecimalPlaces()
    //
    /// <summary>
    /// Counts the number of decimal places in a Decimal.
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
    //  Method: GetDecimalFraction()
    //
    /// <summary>
    /// Returns the fractional part of a Decimal.
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

    //*************************************************************************
    //  Method: DegreesToRadians()
    //
    /// <summary>
    /// Converts an angle from degrees to radians.
    /// </summary>
    ///
    /// <param name="degrees">
    /// An angle in degrees.
    /// </param>
    ///
    /// <returns>
    /// The angle in radians.
    /// </returns>
    //*************************************************************************

    public static Double
    DegreesToRadians
    (
        Double degrees
    )
    {
        return ( ( degrees * (2.0 * Math.PI) ) / 360.0 );
    }

    //*************************************************************************
    //  Method: RadiansToDegrees()
    //
    /// <summary>
    /// Converts an angle from radians to degrees.
    /// </summary>
    ///
    /// <param name="radians">
    /// An angle in radians.
    /// </param>
    ///
    /// <returns>
    /// The angle in degrees.
    /// </returns>
    //*************************************************************************

    public static Double
    RadiansToDegrees
    (
        Double radians
    )
    {
        return ( ( radians * 360.0 ) / (2.0 * Math.PI) );
    }

    //*************************************************************************
    //  Method: ParseCultureInvariantInt32()
    //
    /// <summary>
	/// Parses a string containing an Int32 using the invariant culture.
    /// </summary>
    ///
    /// <param name="s">
    /// String containing an Int32, formatted with the invariant culture.
    /// </param>
    ///
    /// <returns>
    /// The parsed Int32.
    /// </returns>
    //*************************************************************************

    public static Int32
    ParseCultureInvariantInt32
    (
        String s
    )
    {
        return ( Int32.Parse(s, CultureInfo.InvariantCulture) );
    }

    //*************************************************************************
    //  Method: ParseCultureInvariantSingle()
    //
    /// <summary>
	/// Parses a string containing a Single using the invariant culture.
    /// </summary>
    ///
    /// <param name="s">
    /// String containing a Single, formatted with the invariant culture.
    /// </param>
    ///
    /// <returns>
    /// The parsed Single.
    /// </returns>
    //*************************************************************************

    public static Single
    ParseCultureInvariantSingle
    (
        String s
    )
    {
        return ( Single.Parse(s, CultureInfo.InvariantCulture) );
    }

    //*************************************************************************
    //  Method: ParseCultureInvariantDouble()
    //
    /// <summary>
	/// Parses a string containing a Double using the invariant culture.
    /// </summary>
    ///
    /// <param name="s">
    /// String containing a Double, formatted with the invariant culture.
    /// </param>
    ///
    /// <returns>
    /// The parsed Double.
    /// </returns>
    //*************************************************************************

    public static Double
    ParseCultureInvariantDouble
    (
        String s
    )
    {
        return ( Double.Parse(s, CultureInfo.InvariantCulture) );
    }

    //*************************************************************************
    //  Method: TryParseCultureInvariantInt32()
    //
    /// <summary>
	/// Attempts to parse a string containing an Int32 using the invariant
	/// culture.
    /// </summary>
    ///
    /// <param name="s">
    /// String that might contain an Int32, formatted with the invariant
	/// culture.
    /// </param>
    ///
    /// <param name="result">
    /// Where the parsed Int32 gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the string was successfully parsed.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryParseCultureInvariantInt32
    (
        String s,
		out Int32 result
    )
    {
        return ( Int32.TryParse(s, NumberStyles.Integer,
			CultureInfo.InvariantCulture, out result) );
    }

    //*************************************************************************
    //  Method: TryParseCultureInvariantSingle()
    //
    /// <summary>
	/// Attempts to parse a string containing a Single using the invariant
	/// culture.
    /// </summary>
    ///
    /// <param name="s">
    /// String that might contain a Single, formatted with the invariant
	/// culture.
    /// </param>
    ///
    /// <param name="result">
    /// Where the parsed Single gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the string was successfully parsed.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryParseCultureInvariantSingle
    (
        String s,
		out Single result
    )
    {
        return ( Single.TryParse(s, NumberStyles.Float,
			CultureInfo.InvariantCulture, out result) );
    }


    //*************************************************************************
    //  Method: TryParseCultureInvariantDouble()
    //
    /// <summary>
	/// Attempts to parse a string containing a Double using the invariant
	/// culture.
    /// </summary>
    ///
    /// <param name="s">
    /// String that might contain a Double, formatted with the invariant
	/// culture.
    /// </param>
    ///
    /// <param name="result">
    /// Where the parsed Double gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the string was successfully parsed.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryParseCultureInvariantDouble
    (
        String s,
		out Double result
    )
    {
        return ( Double.TryParse(s, NumberStyles.Float,
			CultureInfo.InvariantCulture, out result) );
    }
}

}
