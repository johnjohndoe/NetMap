
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Globalization;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.NodeXL.Visualization.Wpf;
using Microsoft.WpfGraphicsLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: LabelUserSettings
//
/// <summary>
/// Stores the user's settings for graph labels.
/// </summary>
//*****************************************************************************

[ TypeConverterAttribute( typeof(LabelUserSettingsTypeConverter) ) ]

public class LabelUserSettings : Object
{
    //*************************************************************************
    //  Constructor: LabelUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the LabelUserSettings class.
    /// </summary>
    //*************************************************************************

    public LabelUserSettings()
    {
        m_oFont = (Font)( new FontConverter() ).ConvertFromString(
            GeneralUserSettings.DefaultFont);

        m_oVertexLabelFillColor = Color.White;
        m_eVertexLabelPosition = VertexLabelPosition.TopRight;

        m_iVertexLabelMaximumLength = m_iEdgeLabelMaximumLength =
            Int32.MaxValue;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Font
    //
    /// <summary>
    /// Gets or sets the font used for the graph's labels.
    /// </summary>
    ///
    /// <value>
    /// The label font, as a Font.  The default value is Microsoft Sans Serif,
    /// 8.25pt.
    /// </value>
    //*************************************************************************

    public Font
    Font
    {
        // Note that the font type is System.Drawing.Font, which is what the
        // System.Windows.Forms.FontDialog class uses.  It gets converted to
        // WPF font types in TransferToGraphDrawer().

        get
        {
            AssertValid();

            return (m_oFont);
        }

        set
        {
            m_oFont = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexLabelFillColor
    //
    /// <summary>
    /// Gets or sets the fill color of vertices that that have the shape Label.
    /// </summary>
    ///
    /// <value>
    /// The fill color of vertices that have the shape Label, as a Color.  The
    /// default value is Color.White.
    /// </value>
    //*************************************************************************

    public Color
    VertexLabelFillColor
    {
        get
        {
            AssertValid();

            return (m_oVertexLabelFillColor);
        }

        set
        {
            m_oVertexLabelFillColor = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexLabelPosition
    //
    /// <summary>
    /// Gets or sets the position of a vertex label drawn as an annotation.
    /// </summary>
    ///
    /// <value>
    /// The position of a vertex label drawn as an annotation.  The
    /// default value is <see
    /// cref="Visualization.Wpf.VertexLabelPosition.TopRight" />.
    /// </value>
    //*************************************************************************

    public VertexLabelPosition
    VertexLabelPosition
    {
        get
        {
            AssertValid();

            return (m_eVertexLabelPosition);
        }

        set
        {
            m_eVertexLabelPosition = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexLabelMaximumLength
    //
    /// <summary>
    /// Gets or sets the maximum number of characters to show in a vertex
    /// label.
    /// </summary>
    ///
    /// <value>
    /// The maximum number of characters to show, or Int32.MaxValue for no
    /// maximum.  Must be greater than or equal to zero.  The default is
    /// Int32.MaxValue.
    /// </value>
    //*************************************************************************

    public Int32
    VertexLabelMaximumLength
    {
        get
        {
            AssertValid();

            return (m_iVertexLabelMaximumLength);
        }

        set
        {
            m_iVertexLabelMaximumLength = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeLabelMaximumLength
    //
    /// <summary>
    /// Gets or sets the maximum number of characters to show in an edge label.
    /// </summary>
    ///
    /// <value>
    /// The maximum number of characters to show, or Int32.MaxValue for no
    /// maximum.  Must be greater than or equal to zero.  The default is
    /// Int32.MaxValue.
    /// </value>
    //*************************************************************************

    public Int32
    EdgeLabelMaximumLength
    {
        get
        {
            AssertValid();

            return (m_iEdgeLabelMaximumLength);
        }

        set
        {
            m_iEdgeLabelMaximumLength = value;

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

    public LabelUserSettings
    Copy()
    {
        AssertValid();

        LabelUserSettings oCopy = new LabelUserSettings();

        oCopy.Font = this.Font;
        oCopy.VertexLabelFillColor = this.VertexLabelFillColor;
        oCopy.VertexLabelPosition = this.VertexLabelPosition;
        oCopy.VertexLabelMaximumLength = this.VertexLabelMaximumLength;
        oCopy.EdgeLabelMaximumLength = this.EdgeLabelMaximumLength;

        return (oCopy);
    }

    //*************************************************************************
    //  Method: TransferToGraphDrawer()
    //
    /// <summary>
    /// Transfers the settings to a <see cref="GraphDrawer" />.
    /// </summary>
    ///
    /// <param name="graphDrawer">
    /// Graph drawer to transfer the settings to.
    /// </param>
    //*************************************************************************

    public void
    TransferToGraphDrawer
    (
        GraphDrawer graphDrawer
    )
    {
        Debug.Assert(graphDrawer != null);
        AssertValid();

        Font oFont = this.Font;

        System.Windows.Media.Typeface oTypeface =
            WpfGraphicsUtil.FontToTypeface(oFont);

        Double dFontSize = WpfGraphicsUtil.WindowsFormsFontSizeToWpfFontSize(
            oFont.Size);

        VertexDrawer oVertexDrawer = graphDrawer.VertexDrawer;

        oVertexDrawer.SetFont(oTypeface, dFontSize);

        oVertexDrawer.LabelFillColor =
            WpfGraphicsUtil.ColorToWpfColor(this.VertexLabelFillColor);

        oVertexDrawer.LabelPosition = this.VertexLabelPosition;
        oVertexDrawer.MaximumLabelLength = this.VertexLabelMaximumLength;

        EdgeDrawer oEdgeDrawer = graphDrawer.EdgeDrawer;

        oEdgeDrawer.SetFont(oTypeface, dFontSize);
        oEdgeDrawer.MaximumLabelLength = this.EdgeLabelMaximumLength;
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
        Debug.Assert(m_oFont != null);
        // m_oVertexLabelFillColor
        // m_eVertexLabelPosition
        Debug.Assert(m_iVertexLabelMaximumLength >= 0);
        Debug.Assert(m_iEdgeLabelMaximumLength >= 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Font.

    protected Font m_oFont;

    /// The fill color of vertices that have the shape Label.

    protected Color m_oVertexLabelFillColor;

    /// The position of a vertex label drawn as an annotation.

    protected VertexLabelPosition m_eVertexLabelPosition;

    /// The maximum number of characters to show in a vertex label, or
    /// Int32.MaxValue for no maximum.

    protected Int32 m_iVertexLabelMaximumLength;

    /// The maximum number of characters to show in an edge label, or
    /// Int32.MaxValue for no maximum.

    protected Int32 m_iEdgeLabelMaximumLength;
}


//*****************************************************************************
//  Class: LabelUserSettingsTypeConverter
//
/// <summary>
/// Converts a LabelUserSettings object to and from a String.
/// </summary>
/// 
/// <remarks>
/// One of the properties of <see cref="GeneralUserSettings" /> is of type <see
/// cref="LabelUserSettings" />.  The application settings architecture
/// requires a type converter for such a complex type.
/// </remarks>
//*****************************************************************************

public class LabelUserSettingsTypeConverter : TypeConverter
{
    //*************************************************************************
    //  Constructor: LabelUserSettingsTypeConverter()
    //
    /// <summary>
    /// Initializes a new instance of the LabelUserSettingsTypeConverter class.
    /// </summary>
    //*************************************************************************

    public LabelUserSettingsTypeConverter()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: CanConvertTo()
    //
    /// <summary>
    /// Returns whether this converter can convert the object to the specified
    /// type, using the specified context.
    /// </summary>
    ///
    /// <param name="context">
    /// An ITypeDescriptorContext that provides a format context. 
    /// </param>
    ///
    /// <param name="sourceType">
    /// A Type that represents the type you want to convert to. 
    /// </param>
    ///
    /// <returns>
    /// true if this converter can perform the conversion; otherwise, false.
    /// </returns>
    //*************************************************************************

    public override Boolean
    CanConvertTo
    (
        ITypeDescriptorContext context,
        Type sourceType
    )
    {
        AssertValid();

        return ( sourceType == typeof(String) );
    }

    //*************************************************************************
    //  Method: CanConvertFrom()
    //
    /// <summary>
    /// Returns whether this converter can convert an object of the given type
    /// to the type of this converter, using the specified context.
    /// </summary>
    ///
    /// <param name="context">
    /// An ITypeDescriptorContext that provides a format context. 
    /// </param>
    ///
    /// <param name="sourceType">
    /// A Type that represents the type you want to convert from. 
    /// </param>
    ///
    /// <returns>
    /// true if this converter can perform the conversion; otherwise, false.
    /// </returns>
    //*************************************************************************

    public override Boolean
    CanConvertFrom
    (
        ITypeDescriptorContext context,
        Type sourceType
    )
    {
        AssertValid();

        return ( sourceType == typeof(String) );
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
        Debug.Assert(value is LabelUserSettings);
        Debug.Assert( destinationType == typeof(String) );
        AssertValid();

        LabelUserSettings oLabelUserSettings = (LabelUserSettings)value;

        // Use a simple tab-delimited format.  Sample string:
        //
        // "Microsoft Sans Serif, 8.25pt\tWhite\tTopRight\t2147483647\t
        // 4294967295";

        return ( String.Format(CultureInfo.InvariantCulture,

            "{0}\t{1}\t{2}\t{3}\t{4}"
            ,
            ( new FontConverter() ).ConvertToString(oLabelUserSettings.Font),

            ( new ColorConverter() ).ConvertToString(
                oLabelUserSettings.VertexLabelFillColor),

            oLabelUserSettings.VertexLabelPosition,
            oLabelUserSettings.VertexLabelMaximumLength,
            oLabelUserSettings.EdgeLabelMaximumLength
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
    /// A CultureInfo. If null is passed, the current culture is assumed. 
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

        LabelUserSettings oLabelUserSettings = new LabelUserSettings();

        String [] asStrings = ( (String)value ).Split( new Char[] {'\t'} );

        Debug.Assert(asStrings.Length == 5);

        oLabelUserSettings.Font = (Font)
            ( new FontConverter() ).ConvertFromString(asStrings[0] );

        oLabelUserSettings.VertexLabelFillColor = (Color)
            ( new ColorConverter() ).ConvertFromString(asStrings[1] );

        oLabelUserSettings.VertexLabelPosition = (VertexLabelPosition)
            Enum.Parse( typeof(VertexLabelPosition), asStrings[2] );

        oLabelUserSettings.VertexLabelMaximumLength =
            Int32.Parse( asStrings[3] );

        oLabelUserSettings.EdgeLabelMaximumLength =
            Int32.Parse( asStrings[4] );

        return (oLabelUserSettings);
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
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
