
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Globalization;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Enum: SimpleDateTimeFormat
//
/// <summary>
/// Specifies one of a simplified set of DateTime formats.
/// </summary>
//*****************************************************************************

public enum
SimpleDateTimeFormat
{
    /// <summary>
    /// The DateTime value is formatted using the short date pattern set by the
    /// user's operating system.
    /// </summary>

    Date,

    /// <summary>
    /// The DateTime value is formatted using the long time pattern set by the
    /// user's operating system.
    /// </summary>

    Time,

    /// <summary>
    /// The DateTime value is formatted using a concatenation of the short date
    /// pattern and the short time pattern set by the user's operating system.
    /// </summary>

    DateAndTime,
}

//*****************************************************************************
//  Class: SimpleDateTimeFormatUtil
//
/// <summary>
/// Methods for dealing with <see cref="SimpleDateTimeFormat" /> enum values.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class SimpleDateTimeFormatUtil
{
    //*************************************************************************
    //  Method: GetFormatPattern()
    //
    /// <summary>
    /// Gets a format pattern for a <see cref="SimpleDateTimeFormat" /> value.
    /// </summary>
    ///
    /// <param name="simpleDateTimeFormat">
    /// The <see cref="SimpleDateTimeFormat" /> value to get a format pattern
    /// for.
    /// </param>
    ///
    /// <returns>
    /// A format pattern string.
    /// </returns>
    //*************************************************************************

    public static String
    GetFormatPattern
    (
        SimpleDateTimeFormat simpleDateTimeFormat
    )
    {
        DateTimeFormatInfo oDateTimeFormatInfo =
            CultureInfo.CurrentCulture.DateTimeFormat;

        switch (simpleDateTimeFormat)
        {
            case SimpleDateTimeFormat.Date:

                return (oDateTimeFormatInfo.ShortDatePattern);

            case SimpleDateTimeFormat.Time:

                return (oDateTimeFormatInfo.ShortTimePattern);

            case SimpleDateTimeFormat.DateAndTime:

                return (oDateTimeFormatInfo.ShortDatePattern + " "
                    + oDateTimeFormatInfo.ShortTimePattern);

            default:

                Debug.Assert(false);
                return (null);
        }
    }
}

}
