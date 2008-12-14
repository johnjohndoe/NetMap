
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Interface: IDynamicFilterRangeTrackBar
//
/// <summary>
/// Represents a range track bar used in the <see
/// cref="DynamicFilterDialog" />.
/// </summary>
///
/// <remarks>
/// This interface is implemented by several classes that wrap a specific type
/// of range track bar control.  The <see cref="DynamicFilterDialog" />
/// instantiates several such controls, and it uses this interface to
/// communicate with them in a simple and consistent manner.
///
/// <para>
/// The available range is the range the user can select from.  The
/// selected range is the range within the available range that the user
/// has selected.
/// </para>
///
/// </remarks>
//*****************************************************************************

public interface IDynamicFilterRangeTrackBar
{
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

    Decimal
    AvailableMinimum
    {
        get;
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

    Decimal
    AvailableMaximum
    {
        get;
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

    Decimal
    SelectedMinimum
    {
        get;
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

    Decimal
    SelectedMaximum
    {
        get;
    }

	//*************************************************************************
	//	Method: SetAvailableRange()
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

	void
	SetAvailableRange
	(
		Decimal availableMinimum,
		Decimal availableMaximum
	);

	//*************************************************************************
	//	Method: SetSelectedRange()
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

	void
	SetSelectedRange
	(
		Decimal selectedMinimum,
		Decimal selectedMaximum
	);

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
	/// <see cref="SetAvailableRange" /> and <see cref="SetSelectedRange" />.
	/// The implementation should set any custom properties that are specific
	/// to the wrapped control and not otherwise exposed via this interface.
	/// </remarks>
	//*************************************************************************

	void
	SetCustomProperties
	(
		DynamicFilterParameters dynamicFilterParameters
	);


	//*************************************************************************
	//	Event: SelectedRangeChanged
	//
	/// <summary>
	///	Occurs when the selected range changes.
	/// </summary>
	//*************************************************************************

	event EventHandler SelectedRangeChanged;
}

}
