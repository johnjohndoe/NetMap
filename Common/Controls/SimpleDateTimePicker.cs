
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: SimpleDateTimePicker
//
/// <summary>
/// DateTimePicker control that uses a simplified set of date/time formats.
/// </summary>
///
/// <remarks>
/// This control displays either a date, time, or a date and time.  Use it when
/// you need a DateTimePicker that displays only these formats and does so in a
/// consistent manner.
///
/// <para>
/// Set the <see cref="SimpleFormat" /> property instead of the base-class
/// <see cref="DateTimePicker.Format" /> property.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class SimpleDateTimePicker : DateTimePicker
{
    //*************************************************************************
    //  Constructor: SimpleDateTimePicker()
    //
    /// <summary>
    /// Initializes a new instance of the SimpleDateTimePicker class.
    /// </summary>
    //*************************************************************************

    public SimpleDateTimePicker()
    {
        this.SimpleFormat = SimpleDateTimeFormat.Date;

        AssertValid();
    }

    //*************************************************************************
    //  Property: SimpleFormat
    //
    /// <summary>
    /// Gets or sets the format of the date and time displayed in the control.
    /// </summary>
    ///
    /// <value>
    /// The format of the date and time displayed in the control, as a <see
    /// cref="SimpleDateTimeFormat" />.  The default is <see
    /// cref="SimpleDateTimeFormat.Date" />.
    /// </value>
    //*************************************************************************

    public SimpleDateTimeFormat
    SimpleFormat
    {
        get
        {
            AssertValid();

            String sCustomFormat = this.CustomFormat;

            if ( sCustomFormat == SimpleDateTimeFormatUtil.GetFormatPattern(
                SimpleDateTimeFormat.Date) )
            {
                return (SimpleDateTimeFormat.Date);
            }

            if ( sCustomFormat == SimpleDateTimeFormatUtil.GetFormatPattern(
                SimpleDateTimeFormat.Time) )
            {
                return (SimpleDateTimeFormat.Time);
            }

            if ( sCustomFormat == SimpleDateTimeFormatUtil.GetFormatPattern(
                SimpleDateTimeFormat.DateAndTime) )
            {
                return (SimpleDateTimeFormat.DateAndTime);
            }

            throw new InvalidOperationException(
                "SimpleDateTimePicker.CustomFormat should not be set by"
                + " application."
                );
        }

        set
        {
            this.Format = DateTimePickerFormat.Custom;

            this.CustomFormat =
                SimpleDateTimeFormatUtil.GetFormatPattern(value);

            this.ShowUpDown = (value == SimpleDateTimeFormat.Time);

            AssertValid();
        }
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public virtual void
    AssertValid()
    {
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
