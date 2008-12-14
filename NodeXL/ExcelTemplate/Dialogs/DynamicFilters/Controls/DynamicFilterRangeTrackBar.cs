
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.WinFormsControls;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: DynamicFilterRangeTrackBar
//
/// <summary>
/// Wraps a <see cref="RangeTrackBar" /> for use in the <see
/// cref="DynamicFilterDialog" />.
/// </summary>
///
/// <remarks>
/// This wrapper implements <see cref="IDynamicFilterRangeTrackBar" />, which
/// allows <see cref="DynamicFilterDialog" /> to communicate with the <see
/// cref="RangeTrackBar" /> and other track bar controls in a
/// simple and consistent manner.
///
/// <para>
/// This wrapper uses the base-class implementations for most of the
/// interface's properties and methods.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class DynamicFilterRangeTrackBar :
	RangeTrackBar, IDynamicFilterRangeTrackBar
{
	//*************************************************************************
	//	Method: SetCustomProperties()
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
		Debug.Assert(dynamicFilterParameters != null);

		Decimal decSmallChange;

		SetDecimalPlaces();

		if (this.DecimalPlaces == 0)
		{
			decSmallChange = 1;
		}
		else
		{
			decSmallChange =
				(this.AvailableMaximum - this.AvailableMinimum) / 100;
		}

		this.SmallChange = decSmallChange;
	}

	//*************************************************************************
	//	Method: SetDecimalPlaces()
	//
	/// <summary>
	///	Sets the number of decimal places to show.
	/// </summary>
	///
	/// <remarks>
	/// It's assumed that the RangeTrackBar's available range has already been
	/// set.
	/// </remarks>
	//*************************************************************************

	protected void
	SetDecimalPlaces()
	{
		AssertValid();

		Int32 iDecimalPlaces;

		Int32 iAvailableMinimumDecimalPlaces =
			MathUtil.CountDecimalPlaces(this.AvailableMinimum);

		Int32 iAvailableMaximumDecimalPlaces =
			MathUtil.CountDecimalPlaces(this.AvailableMaximum);

		if (iAvailableMinimumDecimalPlaces == 0 &&
			iAvailableMaximumDecimalPlaces == 0)
		{
			iDecimalPlaces = 0;
		}
		else
		{
			// Start with 1 plus the maximum number of decimal places in the
			// available range.

			iDecimalPlaces = 1 + Math.Max(
				iAvailableMinimumDecimalPlaces, iAvailableMaximumDecimalPlaces
				);

			// Show at least 2.

			iDecimalPlaces = Math.Max(iDecimalPlaces, 2);

			// Don't show more than 6.

			iDecimalPlaces = Math.Min(iDecimalPlaces, 6);
		}

		this.DecimalPlaces = iDecimalPlaces;
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
