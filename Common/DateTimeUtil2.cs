
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.DateTimeLib
{
//*****************************************************************************
//  Class: DateTimeUtil2
//
/// <summary>
/// Static utility methods involving dates and times.
/// </summary>
///
/// <remarks>
/// This is a replacement for DateTimeUtil, which should not be used in new
/// projects.
/// </remarks>
//*****************************************************************************

public static class DateTimeUtil2
{
    //*************************************************************************
    //  Method: TruncateToMinutes()
    //
    /// <summary>
    /// Returns a new DateTime with the seconds and milliseconds set to 0.
    /// </summary>
    ///
    /// <param name="dateTime">
    /// DateTime to copy.  Does not get modified.
    /// </param>
    ///
    /// <returns>
    /// A copy of <paramref name="dateTime" /> with the seconds and
    /// milliseconds set to 0.
    /// </returns>
    ///
    /// <remarks>
    /// This method can't be used in .NET 1.1 applications.  It uses
    /// DateTimeKind, which was introduced in .NET 2.0.
    /// </remarks>
    //*************************************************************************

    public static DateTime
    TruncateToMinutes
    (
        DateTime dateTime
    )
    {
        return ( new DateTime(
            dateTime.Year,
            dateTime.Month,
            dateTime.Day,
            dateTime.Hour,
            dateTime.Minute,
            0,
            dateTime.Kind
            ) );
    }

    //*************************************************************************
    //  Method: ForceUtc()
    //
    /// <summary>
    /// Returns a new DateTime with the Kind property set to DateTimeKind.Utc.
    /// </summary>
    ///
    /// <param name="dateTimeUtc">
    /// DateTime to copy, in UTC.  Does not get modified.
    /// </param>
    ///
    /// <returns>
    /// A copy of <paramref name="dateTimeUtc" /> with the Kind property set
    /// to DateTimeKind.Utc.
    /// </returns>
    ///
    /// <remarks>
    /// Use this method to force a DateTime obtained via Remoting to be UTC.
    /// (The Kind property does not seem to be properly transmitted via
    /// Remoting.)
    ///
    /// <para>
    /// This method can't be used in .NET 1.1 applications.  It uses
    /// DateTimeKind, which was introduced in .NET 2.0.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static DateTime
    ForceUtc
    (
        DateTime dateTimeUtc
    )
    {
        return ( new DateTime(dateTimeUtc.Ticks, DateTimeKind.Utc) );
    }

    //*************************************************************************
    //  Method: UnixTimestampToDateTimeUtc()
    //
    /// <summary>
    /// Converts a Unix timestamp to a DateTime.
    /// </summary>
    ///
    /// <param name="unixTimestampUtc">
    /// Unix timestamp to convert, in UTC.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="unixTimestamp" /> converted to a DateTime, in UTC.
    /// </returns>
    //*************************************************************************

    public static DateTime
    UnixTimestampToDateTimeUtc
    (
        UInt32 unixTimestampUtc
    )
    {
        // A Unix timestamp is the number of seconds since 1/1/1970.

        DateTime oDateTime = new DateTime(1970, 1, 1, 0, 0, 0,
            DateTimeKind.Utc);

        return ( oDateTime.AddSeconds(unixTimestampUtc) );
    }
}

}
