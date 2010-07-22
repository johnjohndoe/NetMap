
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Globalization;
using System.Diagnostics;

namespace Microsoft.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterDateParser
//
/// <summary>
/// Parses dates received from the Twitter API.
/// </summary>
///
/// <remarks>
/// As of July 2010, Twitter uses two date formats: one for the REST API, and
/// another for the search API.  This class can parse both formats.
///
/// <para>
/// All methods are static.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class TwitterDateParser
{
    //*************************************************************************
    //  Method: ParseTwitterDate()
    //
    /// <summary>
    /// Parses a date received from the Twitter API.
    /// </summary>
    ///
    /// <param name="twitterDate">
    /// Date received from the Twitter API.  Can't be null.
    /// </param>
    ///
    /// <returns>
    /// The parsed date as a culture-invariant UTC string if parsing
    /// succeeded, or <paramref name="twitterDate" /> if parsing failed.
    /// Sample parsed date: "2006-04-17 21:22:48".
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="twitterDate" /> can be parsed into a DateTime, this
    /// method converts that DateTime to a culture-invariant UTC string and
    /// returns the string.  Otherwise, <paramref name="twitterDate" /> is
    /// returned unmodified.
    /// </remarks>
    //*************************************************************************

    public static String
    ParseTwitterDate
    (
        String twitterDate
    )
    {
        Debug.Assert(twitterDate != null);

        DateTime oParsedTwitterDate;

        if ( !TryParseTwitterDate(twitterDate, out oParsedTwitterDate) )
        {
            return (twitterDate);
        }

        return ( ToCultureInvariantString(oParsedTwitterDate) );
    }

    //*************************************************************************
    //  Method: TryParseTwitterDate()
    //
    /// <summary>
    /// Attempts to parse a date received from the Twitter API.
    /// </summary>
    ///
    /// <param name="twitterDate">
    /// Date received from the Twitter API.  Can't be null.
    /// </param>
    ///
    /// <param name="parsedTwitterDate">
    /// Where the parsed DateTime gets stored if true is returned.  The Kind of
    /// the DateTime is DateTimeKind.Utc.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="tiwtterDate" /> was successfully parsed.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryParseTwitterDate
    (
        String twitterDate,
        out DateTime parsedTwitterDate
    )
    {
        Debug.Assert(twitterDate != null);

        parsedTwitterDate = DateTime.MinValue;

        String [] asFormatsToCheck = new String [] {

            // Twitter REST API format:
            //
            // Sat Mar 21 00:50:51 +0000 2009

              "ddd MMM dd HH:mm:ss +0000 yyyy",

            // Twitter search API format:
            //
            // 2010-07-19T21:23:16Z

              "yyyy-MM-ddTHH:mm:ssZ"
            };

        return ( DateTime.TryParseExact(twitterDate, asFormatsToCheck,
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
            out parsedTwitterDate) );
    }

    //*************************************************************************
    //  Method: ToCultureInvariantString()
    //
    /// <summary>
    /// Converts a DateTime to a culture-invariant string that Excel will
    /// recognize.
    /// </summary>
    ///
    /// <param name="dateTime">
    /// The date to convert.
    /// </param>
    ///
    /// <returns>
    /// The date as a culture-invariant UTC string that Excel will recognize.
    /// Sample: "2006-04-17 21:22:48".
    /// </returns>
    //*************************************************************************

    public static String
    ToCultureInvariantString
    (
        DateTime dateTime
    )
    {
        // Start with the the "UniversalSortableDateTimePattern," which
        // produces this:
        //
        // 2006-04-17 21:22:48Z
        //
        // ...and then remove the "Z", which Excel doesn't recognize:
        //
        // 2006-04-17 21:22:48

        return ( dateTime.ToString("u").Replace("Z", String.Empty) );
    }
}

}
