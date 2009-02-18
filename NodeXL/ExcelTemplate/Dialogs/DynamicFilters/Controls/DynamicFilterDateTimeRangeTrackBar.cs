
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.WinFormsControls;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: DynamicFilterDateTimeRangeTrackBar
//
/// <summary>
/// Wraps a <see cref="DateTimeRangeTrackBar" /> for use in the <see
/// cref="DynamicFilterDialog" />.
/// </summary>
///
/// <remarks>
/// This wrapper implements <see cref="IDynamicFilterRangeTrackBar" />, which
/// allows <see cref="DynamicFilterDialog" /> to communicate with the <see
/// cref="DateTimeRangeTrackBar" /> and other track bar controls in a
/// simple and consistent manner.
///
/// <para>
/// The main task of this wrapper is to convert the Decimal range values used
/// by the <see cref="DynamicFilterDialog" /> to and from the DateTime values 
/// used by the <see cref="DateTimeRangeTrackBar" /> control.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class DynamicFilterDateTimeRangeTrackBar :
    DateTimeRangeTrackBar, IDynamicFilterRangeTrackBar
{
    //*************************************************************************
    //  Constructor: DynamicFilterDateTimeRangeTrackBar()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="DynamicFilterDateTimeRangeTrackBar" /> class.
    /// </summary>
    //*************************************************************************

    public DynamicFilterDateTimeRangeTrackBar()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: AvailableMinimum
    //
    /// <summary>
    /// Gets the minimum value in the available range.
    /// </summary>
    ///
    /// <value>
    /// The minimum value in the available range, as a Decimal.
    /// </value>
    //*************************************************************************

    public new Decimal
    AvailableMinimum
    {
        get
        {
            AssertValid();

            return ( ExcelDateTimeUtil.DateTimeToExcelDecimal(
                base.AvailableMinimum) );
        }
    }

    //*************************************************************************
    //  Property: AvailableMaximum
    //
    /// <summary>
    /// Gets the maximum value in the available range.
    /// </summary>
    ///
    /// <value>
    /// The maximum value in the available range, as a Decimal.
    /// </value>
    //*************************************************************************

    public new Decimal
    AvailableMaximum
    {
        get
        {
            AssertValid();

            return ( ExcelDateTimeUtil.DateTimeToExcelDecimal(
                base.AvailableMaximum) );
        }
    }

    //*************************************************************************
    //  Property: SelectedMinimum
    //
    /// <summary>
    /// Gets the minimum value in the selected range.
    /// </summary>
    ///
    /// <value>
    /// The minimum value in the selected range, as a Decimal.
    /// </value>
    //*************************************************************************

    public new Decimal
    SelectedMinimum
    {
        get
        {
            AssertValid();

            return ( ExcelDateTimeUtil.DateTimeToExcelDecimal(
                base.SelectedMinimum) );
        }
    }

    //*************************************************************************
    //  Property: SelectedMaximum
    //
    /// <summary>
    /// Gets the maximum value in the selected range.
    /// </summary>
    ///
    /// <value>
    /// The maximum value in the selected range, as a Decimal.
    /// </value>
    //*************************************************************************

    public new Decimal
    SelectedMaximum
    {
        get
        {
            AssertValid();

            return ( ExcelDateTimeUtil.DateTimeToExcelDecimal(
                base.SelectedMaximum) );
        }
    }

    //*************************************************************************
    //  Method: SetAvailableRange()
    //
    /// <summary>
    /// Sets the available range.
    /// </summary>
    ///
    /// <param name="availableMinimum">
    /// The minimum value in the available range.  Must be less than or equal
    /// to <paramref name="availableMaximum" />.
    /// </param>
    ///
    /// <param name="availableMaximum">
    /// The maximum value in the available range.  Must be greater than or
    /// equal to <paramref name="availableMinimum" />.
    /// </param>
    //*************************************************************************

    public void
    SetAvailableRange
    (
        Decimal availableMinimum,
        Decimal availableMaximum
    )
    {
        AssertValid();

        base.SetAvailableRange(
            ExcelDateTimeUtil.ExcelDecimalToDateTime(availableMinimum),
            ExcelDateTimeUtil.ExcelDecimalToDateTime(availableMaximum)
            );
    }

    //*************************************************************************
    //  Method: SetSelectedRange()
    //
    /// <summary>
    /// Sets the available range.
    /// </summary>
    ///
    /// <param name="selectedMinimum">
    /// The minimum value in the selected range.  Must be less than or equal to
    /// <paramref name="selectedMaximum" />.
    /// </param>
    ///
    /// <param name="selectedMaximum">
    /// The maximum value in the selected range.  Must be greater than or equal
    /// to <paramref name="selectedMinimum" />.
    /// </param>
    //*************************************************************************

    public void
    SetSelectedRange
    (
        Decimal selectedMinimum,
        Decimal selectedMaximum
    )
    {
        AssertValid();

        base.SetSelectedRange(
            ExcelDateTimeUtil.ExcelDecimalToDateTime(selectedMinimum),
            ExcelDateTimeUtil.ExcelDecimalToDateTime(selectedMaximum)
            );
    }

    //*************************************************************************
    //  Method: SetCustomProperties()
    //
    /// <summary>
    /// Sets the properties specific to the wrapped control.
    /// </summary>
    ///
    /// <param name="dynamicFilterParameters">
    /// Parameters for the range track bar.
    /// </param>
    ///
    /// <remarks>
    /// The <see cref="DynamicFilterDialog" /> calls this after it has called
    /// <see cref="IDynamicFilterRangeTrackBar.SetAvailableRange" /> and <see
    /// cref="IDynamicFilterRangeTrackBar.SetSelectedRange" />.  The
    /// implementation should set any custom properties that are specific to
    /// the wrapped control and not otherwise exposed via this interface.
    /// </remarks>
    //*************************************************************************

    public void
    SetCustomProperties
    (
        DynamicFilterParameters dynamicFilterParameters
    )
    {
        AssertValid();
        Debug.Assert(dynamicFilterParameters is DateTimeFilterParameters);

        DateTimeFilterParameters oDateTimeFilterParameters =
            (DateTimeFilterParameters)dynamicFilterParameters;

        // Set the format of the wrapped control based on the Excel column
        // format.

        DateTimeRangeTrackBarFormat eDateTimeRangeTrackBarFormat =
            DateTimeRangeTrackBarFormat.Date;

        switch (oDateTimeFilterParameters.Format)
        {
            case ExcelColumnFormat.Date:

                break;

            case ExcelColumnFormat.Time:

                eDateTimeRangeTrackBarFormat =
                    DateTimeRangeTrackBarFormat.Time;

                break;

            case ExcelColumnFormat.DateAndTime:

                eDateTimeRangeTrackBarFormat =
                    DateTimeRangeTrackBarFormat.DateAndTime;

                break;

            default:

                Debug.Assert(false);
                break;
        }

        this.Format = eDateTimeRangeTrackBarFormat;

        this.SmallChangeMinutes = Math.Max(1,
            (Decimal)( (base.AvailableMaximum - base.AvailableMinimum).
                TotalMinutes / 100.0 ) );
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
        base.AssertValid();

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
