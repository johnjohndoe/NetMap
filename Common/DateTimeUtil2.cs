
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
    //  Method: RemoveTime()
    //
    /// <summary>
    /// Copies a DateTime and sets the copy's time to 12:00 AM.
    /// </summary>
    ///
    /// <param name="dateTime">
    /// DateTime to copy.  Does not get modified.
    /// </param>
    ///
    /// <returns>
    /// A copy of <paramref name="dateTime" /> with the time set to 12:00 AM.
    /// </returns>
    //*************************************************************************

    public static DateTime
    RemoveTime
    (
        DateTime dateTime
    )
    {
        return ( new DateTime(
            dateTime.Year,
            dateTime.Month,
            dateTime.Day,
            0,
            0,
            0,
            dateTime.Kind
            ) );
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
