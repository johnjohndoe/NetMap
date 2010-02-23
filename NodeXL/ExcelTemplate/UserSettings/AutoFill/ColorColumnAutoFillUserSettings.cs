
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Globalization;
using System.Drawing;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ColorColumnAutoFillUserSettings
//
/// <summary>
/// Stores the user's settings for a column that gets autofilled with a range
/// of colors.
/// </summary>
/// 
/// <remarks>
/// The AutoFill feature automatically fills edge and vertex attribute columns
/// using values from user-specified source columns.  This class stores the
/// settings for a destination column that gets autofilled with a range of
/// colors mapped from the numbers in a source column.
/// </remarks>
//*****************************************************************************

[ TypeConverterAttribute(
    typeof(ColorColumnAutoFillUserSettingsTypeConverter) ) ]

public class ColorColumnAutoFillUserSettings : Object
{
    //*************************************************************************
    //  Constructor: ColorColumnAutoFillUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the ColorColumnAutoFillUserSettings
    /// class.
    /// </summary>
    //*************************************************************************

    public ColorColumnAutoFillUserSettings()
    {
        m_bUseSourceNumber1 = false;
        m_bUseSourceNumber2 = false;
        m_dSourceNumber1 = 0;
        m_dSourceNumber2 = 10;
        m_eDestinationColor1 = Color.Red;
        m_eDestinationColor2 = Color.Green;
        m_bIgnoreOutliers = false;
        m_bUseLogs = false;

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
    /// largets number in the source column should be used.  The default is
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
    //  Property: DestinationColor1
    //
    /// <summary>
    /// Gets or sets the first color to use in the destination column.
    /// </summary>
    ///
    /// <value>
    /// The first color to use in the destination column.  The default is Red.
    /// </value>
    //*************************************************************************

    public Color
    DestinationColor1
    {
        get
        {
            AssertValid();

            return (m_eDestinationColor1);
        }

        set
        {
            m_eDestinationColor1 = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: DestinationColor2
    //
    /// <summary>
    /// Gets or sets the second color to use in the destination column.
    /// </summary>
    ///
    /// <value>
    /// The second color to use in the destination column.  The default is
    /// Green.
    /// </value>
    //*************************************************************************

    public Color
    DestinationColor2
    {
        get
        {
            AssertValid();

            return (m_eDestinationColor2);
        }

        set
        {
            m_eDestinationColor2 = value;

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
    /// name="useSourceNumber2" /> are false.  The default is false.
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
    //  Property: UseLogs
    //
    /// <summary>
    /// Gets or sets a flag indicating whether logarithms should be used.
    /// </summary>
    ///
    /// <value>
    /// true if the log of the source column numbers should be used, false if
    /// the source column numbers should be used directly.
    /// </value>
    //*************************************************************************

    public Boolean
    UseLogs
    {
        get
        {
            AssertValid();

            return (m_bUseLogs);
        }

        set
        {
            m_bUseLogs = value;

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

    public void
    AssertValid()
    {
        // m_bUseSourceNumber1
        // m_bUseSourceNumber2
        // m_dSourceNumber1
        // m_dSourceNumber2
        // m_eDestinationColor1
        // m_eDestinationColor2
        // m_bIgnoreOutliers
        // m_bUseLogs
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

    /// The first color to use in the destination column.

    protected Color m_eDestinationColor1;

    /// The second color to use in the destination column.

    protected Color m_eDestinationColor2;

    /// true if outliers should be ignored in the source column.

    protected Boolean m_bIgnoreOutliers;

    /// true if the log of the source column numbers should be used.

    protected Boolean m_bUseLogs;
}


//*****************************************************************************
//  Class: ColorColumnAutoFillUserSettingsTypeConverter
//
/// <summary>
/// Converts a ColorColumnAutoFillUserSettings object to and from a String.
/// </summary>
/// 
/// <remarks>
/// Several properties of <see cref="AutoFillUserSettings" /> are of type <see
/// cref="ColorColumnAutoFillUserSettings" />.  The application settings
/// architecture requires a type converter for such a complex type.
/// </remarks>
//*****************************************************************************

public class ColorColumnAutoFillUserSettingsTypeConverter :
    AutoFillUserSettingsTypeConverterBase
{
    //*************************************************************************
    //  Constructor: ColorColumnAutoFillUserSettingsTypeConverter()
    //
    /// <summary>
    /// Initializes a new instance of the
    /// ColorColumnAutoFillUserSettingsTypeConverter class.
    /// </summary>
    //*************************************************************************

    public ColorColumnAutoFillUserSettingsTypeConverter()
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
        Debug.Assert(value is ColorColumnAutoFillUserSettings);
        Debug.Assert( destinationType == typeof(String) );
        AssertValid();

        ColorColumnAutoFillUserSettings oColorColumnAutoFillUserSettings =
            (ColorColumnAutoFillUserSettings)value;

        ColorConverter oColorConverter = new ColorConverter();

        // Use a simple tab-delimited format.  Sample string:
        //
        // "false\tfalse\t0\t10\tRed\tGreen\tfalse\tfalse"
        //
        // WARNING: If this format is changed, you must also change the
        // DefaultSettingValueAttribute for each property in the
        // AutoFillUserSettings class that is of type
        // ColorColumnAutoFillUserSettings.

        return ( String.Format(CultureInfo.InvariantCulture,

            "{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}"
            ,
            oColorColumnAutoFillUserSettings.UseSourceNumber1,
            oColorColumnAutoFillUserSettings.UseSourceNumber2,
            oColorColumnAutoFillUserSettings.SourceNumber1,
            oColorColumnAutoFillUserSettings.SourceNumber2,

            oColorConverter.ConvertToString(
                oColorColumnAutoFillUserSettings.DestinationColor1),

            oColorConverter.ConvertToString(
                oColorColumnAutoFillUserSettings.DestinationColor2),

            oColorColumnAutoFillUserSettings.IgnoreOutliers,
            oColorColumnAutoFillUserSettings.UseLogs
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

        ColorColumnAutoFillUserSettings oColorColumnAutoFillUserSettings =
            new ColorColumnAutoFillUserSettings();

        ColorConverter oColorConverter = new ColorConverter();

        String [] asStrings = ( (String)value ).Split( new Char[] {'\t'} );

        Debug.Assert(asStrings.Length >= 7);

        oColorColumnAutoFillUserSettings.UseSourceNumber1 =
            Boolean.Parse( asStrings[0] );

        oColorColumnAutoFillUserSettings.UseSourceNumber2 =
            Boolean.Parse( asStrings[1] );

        oColorColumnAutoFillUserSettings.SourceNumber1 =
            MathUtil.ParseCultureInvariantDouble( asStrings[2] );

        oColorColumnAutoFillUserSettings.SourceNumber2 =
            MathUtil.ParseCultureInvariantDouble(asStrings[3]);

        oColorColumnAutoFillUserSettings.DestinationColor1 =
            (Color)oColorConverter.ConvertFromString( asStrings[4] );

        oColorColumnAutoFillUserSettings.DestinationColor2 =
            (Color)oColorConverter.ConvertFromString( asStrings[5] );

        oColorColumnAutoFillUserSettings.IgnoreOutliers =
            Boolean.Parse( asStrings[6] );

        // The UseLogs property wasn't added until NodeXL version 1.0.1.92.

        oColorColumnAutoFillUserSettings.UseLogs =
            (asStrings.Length > 7) ? Boolean.Parse( asStrings[7] ) : false;

        return (oColorColumnAutoFillUserSettings);
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
