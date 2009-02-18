
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Globalization;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: NumericComparisonColumnAutoFillUserSettings
//
/// <summary>
/// Stores the user's settings for a column that gets autofilled with one of
/// two strings.
/// </summary>
/// 
/// <remarks>
/// The AutoFill feature automatically fills edge and vertex attribute columns
/// using values from user-specified source columns.  This class stores the
/// settings for a destination column that gets autofilled with one of two
/// strings, depending on how the numbers in the source column compare to a
/// specified number.
/// </remarks>
//*****************************************************************************

[ TypeConverterAttribute(
    typeof(NumericComparisonColumnAutoFillUserSettingsTypeConverter) ) ]

public class NumericComparisonColumnAutoFillUserSettings : Object
{
    //*************************************************************************
    //  Constructor: NumericComparisonColumnAutoFillUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the
    /// NumericComparisonColumnAutoFillUserSettings class.
    /// </summary>
    //*************************************************************************

    public NumericComparisonColumnAutoFillUserSettings()
    {
        m_eComparisonOperator = ComparisonOperator.GreaterThan;
        m_dSourceNumberToCompareTo = 0;
        m_sDestinationString1 = null;
        m_sDestinationString2 = null;

        AssertValid();
    }

    //*************************************************************************
    //  Property: ComparisonOperator
    //
    /// <summary>
    /// Gets or sets the operator to use when comparing a cell in the source
    /// column.
    /// </summary>
    ///
    /// <value>
    /// Operator to use when comparing a cell in the source column.  The
    /// default is ComparisonOperator.GreaterThan.
    /// </value>
    //*************************************************************************

    public ComparisonOperator
    ComparisonOperator
    {
        get
        {
            AssertValid();

            return (m_eComparisonOperator);
        }

        set
        {
            m_eComparisonOperator = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SourceNumberToCompareTo
    //
    /// <summary>
    /// Gets or sets the number to use when comparing a cell in the source
    /// column.
    /// </summary>
    ///
    /// <value>
    /// The number to use when comparing a cell in the source column.  The
    /// default is zero.
    /// </value>
    //*************************************************************************

    public Double
    SourceNumberToCompareTo
    {
        get
        {
            AssertValid();

            return (m_dSourceNumberToCompareTo);
        }

        set
        {
            m_dSourceNumberToCompareTo = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: DestinationString1
    //
    /// <summary>
    /// Gets or sets the string to write to a destination cell if the source
    /// cell satisfies the comparison criteria.
    /// </summary>
    ///
    /// <value>
    /// The string to write to a destination cell if the source cell satisfies
    /// the comparison criteria, or null to not write a string.
    /// </value>
    //*************************************************************************

    public String
    DestinationString1
    {
        get
        {
            AssertValid();

            return (m_sDestinationString1);
        }

        set
        {
            m_sDestinationString1 = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: DestinationString2
    //
    /// <summary>
    /// Gets or sets the string to write to a destination cell if the source
    /// cell does not satisfy the comparison criteria.
    /// </summary>
    ///
    /// <value>
    /// The string to write to a destination cell if the source cell does not
    /// satisfy the comparison criteria, or null to not write a string.
    /// </value>
    //*************************************************************************

    public String
    DestinationString2
    {
        get
        {
            AssertValid();

            return (m_sDestinationString2);
        }

        set
        {
            m_sDestinationString2 = value;

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

    public NumericComparisonColumnAutoFillUserSettings
    Copy()
    {
        AssertValid();

        NumericComparisonColumnAutoFillUserSettings oCopy =
            new NumericComparisonColumnAutoFillUserSettings();

        oCopy.ComparisonOperator = this.ComparisonOperator;
        oCopy.SourceNumberToCompareTo = this.SourceNumberToCompareTo;
        oCopy.DestinationString1 = String.Copy(this.DestinationString1);
        oCopy.DestinationString2 = String.Copy(this.DestinationString2);

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
        // m_eComparisonOperator
        // m_dSourceNumberToCompareTo
        // m_sDestinationString1
        // m_sDestinationString2
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Operator to use when comparing a cell in the source column.

    protected ComparisonOperator m_eComparisonOperator;

    /// The number to use when comparing a cell in the source column.

    protected Double m_dSourceNumberToCompareTo;

    /// The string to write to a destination cell if the source cell satisfies
    /// the comparison criteria, or null to not write a string.

    protected String m_sDestinationString1;

    /// The string to write to a destination cell if the source cell does not
    /// satisfy the comparison criteria, or null to not write a string.

    protected String m_sDestinationString2;
}


//*****************************************************************************
//  Class: NumericComparisonColumnAutoFillUserSettingsTypeConverter
//
/// <summary>
/// Converts a NumericComparisonColumnAutoFillUserSettings object to and from a
/// String.
/// </summary>
/// 
/// <remarks>
/// Several properties of <see cref="AutoFillUserSettings" /> are of type <see
/// cref="NumericComparisonColumnAutoFillUserSettings" />.  The application
/// settings architecture requires a type converter for such a complex type.
/// </remarks>
//*****************************************************************************

public class NumericComparisonColumnAutoFillUserSettingsTypeConverter :
    AutoFillUserSettingsTypeConverterBase
{
    //*************************************************************************
    //  Constructor: NumericComparisonColumnAutoFillUserSettingsTypeConverter()
    //
    /// <summary>
    /// Initializes a new instance of the
    /// NumericComparisonColumnAutoFillUserSettingsTypeConverter class.
    /// </summary>
    //*************************************************************************

    public NumericComparisonColumnAutoFillUserSettingsTypeConverter()
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
    /// A CultureInfo. If null is passed, the current culture is assumed. 
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
        Debug.Assert(value is NumericComparisonColumnAutoFillUserSettings);
        Debug.Assert( destinationType == typeof(String) );
        AssertValid();

        NumericComparisonColumnAutoFillUserSettings
            oNumericComparisonColumnAutoFillUserSettings =
            (NumericComparisonColumnAutoFillUserSettings)value;

        // Use a simple tab-delimited format.  Sample string:
        //
        // "GreaterThan\t0\tDV1\tDV2"

        return ( String.Format(CultureInfo.InvariantCulture,

            "{0}\t{1}\t{2}\t{3}"
            ,
            oNumericComparisonColumnAutoFillUserSettings.ComparisonOperator,

            oNumericComparisonColumnAutoFillUserSettings.
                SourceNumberToCompareTo,

            oNumericComparisonColumnAutoFillUserSettings.DestinationString1,
            oNumericComparisonColumnAutoFillUserSettings.DestinationString2
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

        NumericComparisonColumnAutoFillUserSettings
            oNumericComparisonColumnAutoFillUserSettings =
            new NumericComparisonColumnAutoFillUserSettings();

        String [] asStrings = ( (String)value ).Split( new Char[] {'\t'} );

        Debug.Assert(asStrings.Length == 4);

        oNumericComparisonColumnAutoFillUserSettings.ComparisonOperator =
            (ComparisonOperator)Enum.Parse(
                typeof(ComparisonOperator), asStrings[0] );

        oNumericComparisonColumnAutoFillUserSettings.SourceNumberToCompareTo =
            MathUtil.ParseCultureInvariantDouble( asStrings[1] );

        oNumericComparisonColumnAutoFillUserSettings.DestinationString1 =
            asStrings[2];

        oNumericComparisonColumnAutoFillUserSettings.DestinationString2 =
            asStrings[3];

        return (oNumericComparisonColumnAutoFillUserSettings);
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
