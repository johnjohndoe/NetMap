
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Globalization;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: NumericRangeColumnAutoFillUserSettings
//
/// <summary>
/// Stores the user's settings for a column that gets autofilled with a range
/// of numbers.
/// </summary>
/// 
/// <remarks>
/// The AutoFill feature automatically fills edge and vertex attribute columns
/// using values from user-specified source columns.  This class stores the
/// autofill settings for a destination column that gets autofilled with a
/// range of numbers mapped from the numbers in a source column.
/// </remarks>
//*****************************************************************************

[ TypeConverterAttribute(
	typeof(NumericRangeColumnAutoFillUserSettingsTypeConverter) ) ]

public class NumericRangeColumnAutoFillUserSettings : Object
{
    //*************************************************************************
    //  Constructor: NumericRangeColumnAutoFillUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the
	/// NumericRangeColumnAutoFillUserSettings class.
    /// </summary>
    //*************************************************************************

    public NumericRangeColumnAutoFillUserSettings()
    {
		m_bUseSourceNumber1 = false;
		m_bUseSourceNumber2 = false;
		m_dSourceNumber1 = 0;
		m_dSourceNumber2 = 10;
		m_dDestinationNumber1 = 0;
		m_dDestinationNumber2 = 10;
		m_bIgnoreOutliers = true;

		AssertValid();
    }

    //*************************************************************************
    //  Property: UseSourceNumber1
    //
    /// <summary>
    /// Gets or sets a flag indicating whether <see cref="SourceNumber1" />
	/// should be used for the auto-fill.
    /// </summary>
    ///
    /// <value>
	/// If true, <see cref="SourceNumber1" /> should be used.  If false, the
	/// smallest number in the source column should be used.  The default is
	/// false.
    /// </value>
    //*************************************************************************

    public Boolean
    UseSourceNumber1
    {
        get
        {
            AssertValid();

			return (m_bUseSourceNumber1);
        }

        set
        {
            m_bUseSourceNumber1 = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: UseSourceNumber2
    //
    /// <summary>
    /// Gets or sets a flag indicating whether <see cref="SourceNumber2" />
	/// should be used for the auto-fill.
    /// </summary>
    ///
    /// <value>
	/// If true, <see cref="SourceNumber2" /> should be used.  If false, the
	/// largest number in the source column should be used.  The default is
	/// false.
    /// </value>
    //*************************************************************************

    public Boolean
    UseSourceNumber2
    {
        get
        {
            AssertValid();

			return (m_bUseSourceNumber2);
        }

        set
        {
			m_bUseSourceNumber2 = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SourceNumber1
    //
    /// <summary>
    /// Gets or sets the first number to use in the source column.
    /// </summary>
    ///
    /// <value>
    /// The first number to use in the source column.  Does not have to be less
	/// than <see cref="SourceNumber2" />.  Not valid if <see
	/// cref="UseSourceNumber1" /> is false.  The default is zero.
    /// </value>
    //*************************************************************************

    public Double
    SourceNumber1
    {
        get
        {
            AssertValid();

			return (m_dSourceNumber1);
        }

        set
        {
            m_dSourceNumber1 = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SourceNumber2
    //
    /// <summary>
    /// Gets or sets the second number to use in the source column.
    /// </summary>
    ///
    /// <value>
    /// The second number to use in the source column.  Does not have to be
	/// greater than <see cref="SourceNumber1" />.  Not valid if <see
	/// cref="UseSourceNumber2" /> is false.  The default is 10.
    /// </value>
    //*************************************************************************

    public Double
    SourceNumber2
    {
        get
        {
            AssertValid();

			return (m_dSourceNumber2);
        }

        set
        {
            m_dSourceNumber2 = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: DestinationNumber1
    //
    /// <summary>
    /// Gets or sets the first number to use in the destination column.
    /// </summary>
    ///
    /// <value>
	/// The first number to use in the destination column.  Does not have to be
	/// less than <see cref="DestinationNumber2" />.  The default is zero.
    /// </value>
    //*************************************************************************

    public Double
	DestinationNumber1
    {
        get
        {
            AssertValid();

			return (m_dDestinationNumber1);
        }

        set
        {
			m_dDestinationNumber1 = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: DestinationNumber2
    //
    /// <summary>
    /// Gets or sets the second number to use in the destination column.
    /// </summary>
    ///
    /// <value>
	/// The second number to use in the destination column.  Does not have to
	/// be greater than <see cref="DestinationNumber1" />.  The default is 10.
    /// </value>
    //*************************************************************************

    public Double
	DestinationNumber2
    {
        get
        {
            AssertValid();

			return (m_dDestinationNumber2);
        }

        set
        {
			m_dDestinationNumber2 = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: IgnoreOutliers
    //
    /// <summary>
    /// Gets or sets a flag indicating whether outliers should be ignored in
	/// the source column.
    /// </summary>
    ///
    /// <value>
    /// true if outliers should be ignored in the source column.  Valid only if
	/// <paramref name="useSourceNumber1" /> and <paramref
	/// name="useSourceNumber2" /> are false.  The default is true.
    /// </value>
    //*************************************************************************

    public Boolean
    IgnoreOutliers
    {
        get
        {
            AssertValid();

			return (m_bIgnoreOutliers);
        }

        set
        {
			m_bIgnoreOutliers = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: Copy()
    //
    /// <summary>
    /// Creates a deep copy of the object.
    /// </summary>
	///
	/// <returns>
	/// A deep copy of the object.
	/// </returns>
    //*************************************************************************

    public NumericRangeColumnAutoFillUserSettings
    Copy()
    {
		AssertValid();

		NumericRangeColumnAutoFillUserSettings oCopy =
			new NumericRangeColumnAutoFillUserSettings();

		oCopy.UseSourceNumber1 = this.UseSourceNumber1;
		oCopy.UseSourceNumber2 = this.UseSourceNumber2;
		oCopy.SourceNumber1 = this.SourceNumber1;
		oCopy.SourceNumber2 = this.SourceNumber2;
		oCopy.DestinationNumber1 = this.DestinationNumber1;
		oCopy.DestinationNumber2 = this.DestinationNumber2;
		oCopy.IgnoreOutliers = this.IgnoreOutliers;

		return (oCopy);
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
		// m_bUseSourceNumber1
		// m_bUseSourceNumber2
		// m_dSourceNumber1
		// m_dSourceNumber2
		// m_dDestinationNumber1
		// m_dDestinationNumber2
		// m_bIgnoreOutliers
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Indicates whether m_dSourceNumber1 should be used for the auto-fill.

	protected Boolean m_bUseSourceNumber1;

    /// Indicates whether m_dSourceNumber2 should be used for the auto-fill.

	protected Boolean m_bUseSourceNumber2;

	/// The first number to use in the source column.  Not valid if
	/// m_bUseSourceNumber1 is false.

	protected Double m_dSourceNumber1;

	/// The second number to use in the source column.  Not valid if
	/// m_bUseSourceNumber2 is false.

	protected Double m_dSourceNumber2;

    /// The first number to use in the destination column.

	protected Double m_dDestinationNumber1;

    /// The second number to use in the destination column.

	protected Double m_dDestinationNumber2;

    /// true if outliers should be ignored in the source column.

	protected Boolean m_bIgnoreOutliers;
}


//*****************************************************************************
//  Class: NumericRangeColumnAutoFillUserSettingsTypeConverter
//
/// <summary>
/// Converts a NumericRangeColumnAutoFillUserSettings object to and from a
/// String.
/// </summary>
/// 
/// <remarks>
/// Several properties of <see cref="AutoFillUserSettings" /> are of type <see
/// cref="NumericRangeColumnAutoFillUserSettings" />.  The application settings
/// architecture requires a type converter for such a complex type.
/// </remarks>
//*****************************************************************************

public class NumericRangeColumnAutoFillUserSettingsTypeConverter :
    AutoFillUserSettingsTypeConverterBase
{
    //*************************************************************************
    //  Constructor: NumericRangeColumnAutoFillUserSettingsTypeConverter()
    //
    /// <summary>
    /// Initializes a new instance of the
	/// NumericRangeColumnAutoFillUserSettingsTypeConverter class.
    /// </summary>
    //*************************************************************************

    public NumericRangeColumnAutoFillUserSettingsTypeConverter()
    {
		// (Do nothing.)

		AssertValid();
    }

    //*************************************************************************
    //  Method: ConvertTo()
    //
    /// <summary>
	/// Converts the given value object to the specified type, using the
	/// specified context and culture information.
    /// </summary>
	///
	/// <param name="context">
	/// An ITypeDescriptorContext that provides a format context. 
	/// </param>
	///
	/// <param name="culture">
	/// A CultureInfo. If nullNothingnullptra null reference is passed, the
	/// current culture is assumed. 
	/// </param>
	///
	/// <param name="value">
	/// The Object to convert.
	/// </param>
	///
	/// <param name="destinationType">
	/// The Type to convert the value parameter to. 
	/// </param>
	///
	/// <returns>
	/// An Object that represents the converted value.
	/// </returns>
    //*************************************************************************

	public override Object
	ConvertTo
	(
		ITypeDescriptorContext context,
		CultureInfo culture,
		Object value,
		Type destinationType
	)
	{
		Debug.Assert(value != null);
		Debug.Assert(value is NumericRangeColumnAutoFillUserSettings);
		Debug.Assert( destinationType == typeof(String) );
		AssertValid();

		NumericRangeColumnAutoFillUserSettings
			oNumericRangeColumnAutoFillUserSettings =
			(NumericRangeColumnAutoFillUserSettings)value;

		// Use a simple tab-delimited format.  Sample string:
		//
		// "false\tfalse\t0\t10\t0\t10\ttrue"

		return ( String.Format(

			"{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}"
			,
			oNumericRangeColumnAutoFillUserSettings.UseSourceNumber1,
			oNumericRangeColumnAutoFillUserSettings.UseSourceNumber2,
			oNumericRangeColumnAutoFillUserSettings.SourceNumber1,
			oNumericRangeColumnAutoFillUserSettings.SourceNumber2,
			oNumericRangeColumnAutoFillUserSettings.DestinationNumber1,
			oNumericRangeColumnAutoFillUserSettings.DestinationNumber2,
			oNumericRangeColumnAutoFillUserSettings.IgnoreOutliers
			) );
	}

    //*************************************************************************
    //  Method: ConvertFrom()
    //
    /// <summary>
	/// Converts the given object to the type of this converter, using the
	/// specified context and culture information.
    /// </summary>
	///
	/// <param name="context">
	/// An ITypeDescriptorContext that provides a format context. 
	/// </param>
	///
	/// <param name="culture">
	/// A CultureInfo. If nullNothingnullptra null reference is passed, the
	/// current culture is assumed. 
	/// </param>
	///
	/// <param name="value">
	/// The Object to convert.
	/// </param>
	///
	/// <returns>
	/// An Object that represents the converted value.
	/// </returns>
    //*************************************************************************

	public override Object
	ConvertFrom
	(
		ITypeDescriptorContext context,
		CultureInfo culture,
		Object value
	)
	{
		Debug.Assert(value != null);
		Debug.Assert(value is String);
		AssertValid();

		NumericRangeColumnAutoFillUserSettings
			oNumericRangeColumnAutoFillUserSettings =
			new NumericRangeColumnAutoFillUserSettings();

		String [] asStrings = ( (String)value ).Split( new Char[] {'\t'} );

		Debug.Assert(asStrings.Length == 7);

		oNumericRangeColumnAutoFillUserSettings.UseSourceNumber1 =
			Boolean.Parse( asStrings[0] );

		oNumericRangeColumnAutoFillUserSettings.UseSourceNumber2 =
			Boolean.Parse( asStrings[1] );

		oNumericRangeColumnAutoFillUserSettings.SourceNumber1 =
			Double.Parse( asStrings[2] );

		oNumericRangeColumnAutoFillUserSettings.SourceNumber2 =
			Double.Parse( asStrings[3] );

		oNumericRangeColumnAutoFillUserSettings.DestinationNumber1 =
			Double.Parse( asStrings[4] );

		oNumericRangeColumnAutoFillUserSettings.DestinationNumber2 =
			Double.Parse( asStrings[5] );

		oNumericRangeColumnAutoFillUserSettings.IgnoreOutliers =
			Boolean.Parse( asStrings[6] );

		return (oNumericRangeColumnAutoFillUserSettings);
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
