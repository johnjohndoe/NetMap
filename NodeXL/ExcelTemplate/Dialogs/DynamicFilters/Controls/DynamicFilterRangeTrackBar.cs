
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
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
/// cref="RangeTrackBar" /> and other track bar controls in a simple and
/// consistent manner.
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
    //  Constructor: DynamicFilterRangeTrackBar()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="DynamicFilterRangeTrackBar" /> class.
    /// </summary>
    ///
    /// <param name="tableName">
    /// Name of the table containing the column.
    /// </param>
    ///
    /// <param name="columnName">
    /// Name of the column.
    /// </param>
    //*************************************************************************

    public DynamicFilterRangeTrackBar
    (
        String tableName,
        String columnName
    )
    {
        m_sTableName = tableName;
        m_sColumnName = columnName;

        AssertValid();
    }

    //*************************************************************************
    //  Property: TableName
    //
    /// <summary>
    /// Gets the name of the table containing the column.
    /// </summary>
    ///
    /// <value>
    /// The name of the table containing the column.
    /// </value>
    //*************************************************************************

    public String
    TableName
    {
        get
        {
            AssertValid();
            Debug.Assert( !String.IsNullOrEmpty(m_sTableName) );

            return (m_sTableName);
        }
    }

    //*************************************************************************
    //  Property: ColumnName
    //
    /// <summary>
    /// Gets the name of the column.
    /// </summary>
    ///
    /// <value>
    /// The name of the column.
    /// </value>
    //*************************************************************************

    public String
    ColumnName
    {
        get
        {
            AssertValid();
            Debug.Assert( !String.IsNullOrEmpty(m_sColumnName) );

            return (m_sColumnName);
        }
    }

    //*************************************************************************
    //  Property: AvailableRangeSelected
    //
    /// <summary>
    /// Gets a flag indicating whether the user has selected the filter's
    /// entire available range.
    /// </summary>
    ///
    /// <value>
    /// true if the user has selected the filter's entire available range.
    /// </value>
    //*************************************************************************

    public Boolean
    AvailableRangeSelected
    {
        get
        {
            AssertValid();

            return (this.SelectedMinimum == this.AvailableMinimum &&
                this.SelectedMaximum == this.AvailableMaximum);
        }
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
        Debug.Assert(dynamicFilterParameters != null);
        Debug.Assert(dynamicFilterParameters is NumericFilterParameters);

        SetDecimalPlaces( (NumericFilterParameters)dynamicFilterParameters );

        Decimal decSmallChange;

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
    //  Method: ValueToString()
    //
    /// <summary>
    /// Converts a value to a string.
    /// </summary>
    ///
    /// <param name="value">
    /// The value to convert.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="value" /> converted to a string as it would appear in
    /// the control.
    /// </returns>
    ///
    /// <remarks>
    /// This method uses the wrapped control's formatting properties to convert
    /// a value to a string.
    /// </remarks>
    //*************************************************************************

    public String
    ValueToString
    (
        Decimal value
    )
    {
        AssertValid();

        return ( value.ToString( "N" + this.DecimalPlaces.ToString() ) );
    }

    //*************************************************************************
    //  Method: SetDecimalPlaces()
    //
    /// <summary>
    /// Sets the number of decimal places to show.
    /// </summary>
    ///
    /// <param name="oNumericFilterParameters">
    /// Parameters for the range track bar.
    /// </param>
    //*************************************************************************

    protected void
    SetDecimalPlaces
    (
        NumericFilterParameters oNumericFilterParameters
    )
    {
        AssertValid();
        Debug.Assert(oNumericFilterParameters != null);

        Int32 iDecimalPlaces = oNumericFilterParameters.DecimalPlaces;

        if (iDecimalPlaces > 0)
        {
            // Start with the number of decimal places displayed in the column
            // plus 1.

            iDecimalPlaces++;

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

        // Can't do this, because the base class calls AssertValid() several
        // times during construction.

        // Debug.Assert( !String.IsNullOrEmpty(m_sTableName) );
        // Debug.Assert( !String.IsNullOrEmpty(m_sColumnName) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Name of the table containing the column being filtered on.

    protected String m_sTableName;

    /// Name of the column being filtered on.

    protected String m_sColumnName;
}

}
