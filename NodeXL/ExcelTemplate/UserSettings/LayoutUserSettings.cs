
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Globalization;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.NodeXL.Layouts;
using Microsoft.NodeXL.ApplicationUtil;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: LayoutUserSettings
//
/// <summary>
/// Stores the user's settings for all the graph layouts used by the
/// application.
/// </summary>
//*****************************************************************************

[ TypeConverterAttribute( typeof(LayoutUserSettingsTypeConverter) ) ]

public class LayoutUserSettings : Object
{
    //*************************************************************************
    //  Constructor: LayoutUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the LayoutUserSettings class.
    /// </summary>
    //*************************************************************************

    public LayoutUserSettings()
    {
        m_eLayout = LayoutType.FruchtermanReingold;
        m_iMargin = 6;
        m_fFruchtermanReingoldC = 3.0F;
        m_iFruchtermanReingoldIterations = 10;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Layout
    //
    /// <summary>
    /// Gets or sets the layout type to use.
    /// </summary>
    ///
    /// <value>
    /// The layout type to use, as a <see cref="LayoutType" />.  The default is
    /// <see cref="LayoutType.FruchtermanReingold" />.
    /// </value>
    //*************************************************************************

    public LayoutType
    Layout
    {
        get
        {
            AssertValid();

            return (m_eLayout);
        }

        set
        {
            m_eLayout = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Margin
    //
    /// <summary>
    /// Gets or sets the margin to subtract from each edge of the graph
    /// rectangle before laying out the graph.
    /// </summary>
    ///
    /// <value>
    /// The margin to subtract from each edge.  Must be greater than or equal
    /// to zero.  The default value is 6.
    /// </value>
    //*************************************************************************

    public Int32
    Margin
    {
        get
        {
            AssertValid();

            return (m_iMargin);
        }

        set
        {
            m_iMargin = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: FruchtermanReingoldC
    //
    /// <summary>
    /// Gets or sets the constant that determines the strength of the
    /// attractive and repulsive forces between the vertices when using the
    /// FruchtermanReingoldLayout.
    /// </summary>
    ///
    /// <value>
    /// The "C" constant in the "Modelling the forces" section of the
    /// Fruchterman-Reingold paper.  Must be greater than 0.  The default value
    /// is 3.0.
    /// </value>
    //*************************************************************************

    public Single
    FruchtermanReingoldC
    {
        get
        {
            AssertValid();

            return (m_fFruchtermanReingoldC);
        }

        set
        {
            m_fFruchtermanReingoldC = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: FruchtermanReingoldIterations
    //
    /// <summary>
    /// Gets or sets the number of times to run the Fruchterman-Reingold
    /// algorithm when using the FruchtermanReingoldLayout.
    /// </summary>
    ///
    /// <value>
    /// The number of times to run the Fruchterman-Reingold algorithm when the
    /// graph is laid out, as an Int32.  Must be greater than zero.  The
    /// default value is 10.
    /// </value>
    //*************************************************************************

    public Int32
    FruchtermanReingoldIterations
    {
        get
        {
            AssertValid();

            return (m_iFruchtermanReingoldIterations);
        }

        set
        {
            m_iFruchtermanReingoldIterations = value;

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

    public LayoutUserSettings
    Copy()
    {
        AssertValid();

        LayoutUserSettings oCopy = new LayoutUserSettings();

        oCopy.Margin = this.Margin;

        oCopy.FruchtermanReingoldC = this.FruchtermanReingoldC;

        oCopy.FruchtermanReingoldIterations =
            this.FruchtermanReingoldIterations;

        return (oCopy);
    }

    //*************************************************************************
    //  Method: TransferToLayout()
    //
    /// <summary>
    /// Transfers the settings to an <see cref="ILayout" /> object.
    /// </summary>
    ///
    /// <param name="layout">
    /// Layout to transfer the settings to.
    /// </param>
    //*************************************************************************

    public void
    TransferToLayout
    (
        ILayout layout
    )
    {
        Debug.Assert(layout != null);
        AssertValid();

        layout.Margin = m_iMargin;

        if (layout is FruchtermanReingoldLayout)
        {
            FruchtermanReingoldLayout oFruchtermanReingoldLayout =
                (FruchtermanReingoldLayout)layout;

            oFruchtermanReingoldLayout.C = m_fFruchtermanReingoldC;

            oFruchtermanReingoldLayout.Iterations =
                m_iFruchtermanReingoldIterations;
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
        // m_eLayout
        Debug.Assert(m_iMargin >= 0);
        Debug.Assert(m_fFruchtermanReingoldC > 0);
        Debug.Assert(m_iFruchtermanReingoldIterations > 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Layout type.

    protected LayoutType m_eLayout;

    /// The margin to subtract from each edge of the graph rectangle before
    /// laying out the graph.

    protected Int32 m_iMargin;

    /// The constant that determines the strength of the attractive and
    /// repulsive forces between the vertices when using the
    /// FruchtermanReingoldLayout.

    protected Single m_fFruchtermanReingoldC;

    /// The number of times to run the Fruchterman-Reingold algorithm when
    /// using the FruchtermanReingoldLayout.

    protected Int32 m_iFruchtermanReingoldIterations;
}


//*****************************************************************************
//  Class: LayoutUserSettingsTypeConverter
//
/// <summary>
/// Converts a LayoutUserSettings object to and from a String.
/// </summary>
/// 
/// <remarks>
/// One of the properties of <see cref="GeneralUserSettings" /> is of type <see
/// cref="LayoutUserSettings" />.  The application settings architecture
/// requires a type converter for such a complex type.
/// </remarks>
//*****************************************************************************

public class LayoutUserSettingsTypeConverter : TypeConverter
{
    //*************************************************************************
    //  Constructor: LayoutUserSettingsTypeConverter()
    //
    /// <summary>
    /// Initializes a new instance of the LayoutUserSettingsTypeConverter
    /// class.
    /// </summary>
    //*************************************************************************

    public LayoutUserSettingsTypeConverter()
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
        Debug.Assert(value is LayoutUserSettings);
        Debug.Assert( destinationType == typeof(String) );
        AssertValid();

        LayoutUserSettings oLayoutUserSettings = (LayoutUserSettings)value;

        // Use a simple tab-delimited format.  Sample string:
        //
        // "FruchtermanReingold\t6\t3.0\t10"

        return ( String.Format(CultureInfo.InvariantCulture,

            "{0}\t{1}\t{2}\t{3}"
            ,
            oLayoutUserSettings.Layout,
            oLayoutUserSettings.Margin,
            oLayoutUserSettings.FruchtermanReingoldC,
            oLayoutUserSettings.FruchtermanReingoldIterations
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

        LayoutUserSettings oLayoutUserSettings = new LayoutUserSettings();

        String [] asStrings = ( (String)value ).Split( new Char[] {'\t'} );

        Debug.Assert(asStrings.Length == 4);

        oLayoutUserSettings.Layout = (LayoutType)
            Enum.Parse( typeof(LayoutType), asStrings[0] );

        oLayoutUserSettings.Margin =
            MathUtil.ParseCultureInvariantInt32( asStrings[1] );

        oLayoutUserSettings.FruchtermanReingoldC =
            MathUtil.ParseCultureInvariantSingle( asStrings[2] );

        oLayoutUserSettings.FruchtermanReingoldIterations =
            MathUtil.ParseCultureInvariantInt32( asStrings[3] );

        return (oLayoutUserSettings);
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
