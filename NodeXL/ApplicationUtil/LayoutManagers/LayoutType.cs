
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.NodeXL.Layouts;

namespace Microsoft.NodeXL.ApplicationUtil
{
//*****************************************************************************
//  Enum: LayoutType
//
/// <summary>
/// Specifies the layouts supported by the <see cref="LayoutManager" /> class.
/// </summary>
//*****************************************************************************

public enum
LayoutType
{
    /// <summary>
    /// Use a <see cref="CircleLayout" />.
    /// </summary>

    Circle,

    /// <summary>
    /// Use a <see cref="SpiralLayout" />.
    /// </summary>

    Spiral,

    /// <summary>
    /// Use a <see cref="SinusoidHorizontalLayout" />.
    /// </summary>

    SinusoidHorizontal,

    /// <summary>
    /// Use a <see cref="SinusoidVerticalLayout" />.
    /// </summary>

    SinusoidVertical,

    /// <summary>
    /// Use a <see cref="FruchtermanReingoldLayout" />.
    /// </summary>

    FruchtermanReingold,

    /// <summary>
    /// Use a <see cref="GridLayout" />.
    /// </summary>

    Grid,

    /// <summary>
    /// Use a <see cref="RandomLayout" />.
    /// </summary>

    Random,

    /// <summary>
    /// Use a <see cref="SugiyamaLayout" />.
    /// </summary>

    Sugiyama,

    /// <summary>
    /// Use a <see cref="NullLayout" />.
    /// </summary>

    Null,

    /// <summary>
    /// Use a <see cref="PolarLayout" />.
    /// </summary>

    Polar,

    /// <summary>
    /// Use a <see cref="PolarAbsoluteLayout" />.
    /// </summary>

    PolarAbsolute,

    /// <summary>
    /// Use a <see cref="HarelKorenFastMultiscaleLayout" />.
    /// </summary>

    HarelKorenFastMultiscale,


    // To add support for an additional layout, do the following:
    //
    //   1. Add a value to this enumeration.
    //
    //   2. Add an array entry to AllLayouts.m_aoAllLayouts, below.
    //
    //   3. Add a case statement to AllLayouts.CreateLayout(), below.
}


//*****************************************************************************
//  Class: LayoutInfo
//
/// <summary>
/// Provides information about a layout supported by the <see
/// cref="LayoutManager" /> class.
/// </summary>
//*****************************************************************************

public class LayoutInfo
{
    //*************************************************************************
    //  Constructor: LayoutInfo()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="LayoutInfo" /> class.
    /// </summary>
    ///
    /// <param name="layout">
    /// Layout type.
    /// </param>
    ///
    /// <param name="menuText">
    /// Text to display in a menu item that represents the layout.  Must
    /// contain an ampersand that specifies the menu shortcut key.
    /// </param>
    ///
    /// <param name="description">
    /// Friendly description of the layout.
    /// </param>
    //*************************************************************************

    public LayoutInfo
    (
        LayoutType layout,
        String menuText,
        String description
    )
    {
        m_eLayout = layout;
        m_sMenuText = menuText;
        m_sDescription = description;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Layout
    //
    /// <summary>
    /// Gets the layout type.
    /// </summary>
    ///
    /// <value>
    /// The layout type.
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
    }

    //*************************************************************************
    //  Property: MenuText
    //
    /// <summary>
    /// Gets the text to display in a menu item that represents the layout.
    /// </summary>
    ///
    /// <value>
    /// The menu text to display.  Contains an ampersand that specifies the
    /// menu shortcut key.
    /// </value>
    //*************************************************************************

    public String
    MenuText
    {
        get
        {
            AssertValid();

            return (m_sMenuText);
        }
    }

    //*************************************************************************
    //  Property: Text
    //
    /// <summary>
    /// Gets the text to display in an item that represents the layout.
    /// </summary>
    ///
    /// <value>
    /// The text to display.  This is the same as <see cref="MenuText" />, but
    /// without the ampersand that specifies a menu shortcut key.
    /// </value>
    //*************************************************************************

    public String
    Text
    {
        get
        {
            AssertValid();

            return ( m_sMenuText.Replace("&", String.Empty) );
        }
    }

    //*************************************************************************
    //  Property: Description
    //
    /// <summary>
    /// Gets the friendly description of the layout.
    /// </summary>
    ///
    /// <value>
    /// The friendly description.
    /// </value>
    //*************************************************************************

    public String
    Description
    {
        get
        {
            AssertValid();

            return (m_sDescription);
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
        // m_eLayout
        Debug.Assert( !String.IsNullOrEmpty(m_sMenuText) );
        Debug.Assert(m_sMenuText.IndexOf("&") >= 0);
        Debug.Assert( !String.IsNullOrEmpty(m_sDescription) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Layout type.

    protected LayoutType m_eLayout;

    /// Text to display in a menu item that represents the layout.

    protected String m_sMenuText;

    /// Friendly description of the layout.

    protected String m_sDescription;
}


//*****************************************************************************
//  Class: AllLayouts
//
/// <summary>
/// Provides information about all layouts supported by the <see
/// cref="LayoutManager" /> class.
/// </summary>
//*****************************************************************************

public static class AllLayouts
{
    //*************************************************************************
    //  Method: GetAllLayouts()
    //
    /// <summary>
    /// Gets information about all layouts supported by the <see
    /// cref="LayoutManager" /> class.
    /// </summary>
    ///
    /// <returns>
    /// An array of <see cref="LayoutInfo" /> and <see
    /// cref="LayoutGroupSeparator" /> objects.
    /// </returns>
    ///
    /// <remarks>
    /// Array entries that are the constant <see cref="LayoutGroupSeparator" />
    /// represent a separator between groups of related layouts.
    /// </remarks>
    //*************************************************************************

    public static LayoutInfo []
    GetAllLayouts()
    {
        return (m_aoAllLayouts);
    }

    //*************************************************************************
    //  Method: CreateLayout()
    //
    /// <summary>
    /// Creates a layout of a specified type.
    /// </summary>
    ///
    /// <param name="layoutType">
    /// The type of layout to create.
    /// </param>
    ///
    /// <returns>
    /// A layout of type <paramref name="layoutType" />.
    /// </returns>
    //*************************************************************************

    public static IAsyncLayout
    CreateLayout
    (
        LayoutType layoutType
    )
    {
        switch (layoutType)
        {
            case LayoutType.Circle:

                return ( new CircleLayout() );

            case LayoutType.Spiral:

                return ( new SpiralLayout() );

            case LayoutType.SinusoidHorizontal:

                return ( new SinusoidHorizontalLayout() );

            case LayoutType.SinusoidVertical:

                return ( new SinusoidVerticalLayout() );

            case LayoutType.Grid:

                return ( new GridLayout() );

            case LayoutType.FruchtermanReingold:

                return ( new FruchtermanReingoldLayout() );

            case LayoutType.Random:

                return ( new RandomLayout() );

            case LayoutType.Sugiyama:

                return ( new SugiyamaLayout() );

            case LayoutType.Null:

                return ( new NullLayout() );

            case LayoutType.Polar:

                return ( new PolarLayout() );

            case LayoutType.PolarAbsolute:

                return ( new PolarAbsoluteLayout() );

            case LayoutType.HarelKorenFastMultiscale:

                return ( new HarelKorenFastMultiscaleLayout() );

            default:

                Debug.Assert(false);
                return (null);
        }
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// Indicates that a group separator should be inserted between groups of
    /// related layouts.

    public static readonly LayoutInfo LayoutGroupSeparator = null;


    //*************************************************************************
    //  Private constants
    //*************************************************************************

    /// Information about all layouts in the LayoutType enumeration.
    ///
    /// IMPORTANT: Derived classes assume that there is one array entry for
    /// each layout in the LayoutType enumeration.  There may be additional
    /// entries that are set to the LayoutGroupSeparator constant.
    ///
    /// Note: The descriptions are terminated with periods because they appear
    /// as "SuperTips" within the Excel ribbon, and SuperTips in Excel are
    /// complete sentences.

    private static readonly LayoutInfo [] m_aoAllLayouts = new LayoutInfo [] {

        new LayoutInfo(
            LayoutType.FruchtermanReingold,
            "&Fruchterman-Reingold",
            "Use a Fruchterman-Reingold force-directed layout."
            ),

        new LayoutInfo(
            LayoutType.HarelKorenFastMultiscale,
            "Harel-&Koren Fast Multiscale",
            "Use a Harel-Koren fast multiscale layout."
            ),

        LayoutGroupSeparator,

        new LayoutInfo(
            LayoutType.Circle,
            "&Circle",
            "Place the vertices on the circumference of a circle."
            ),

        new LayoutInfo(
            LayoutType.Spiral,
            "&Spiral",
            "Place the vertices along a spiral."
            ),

        new LayoutInfo(
            LayoutType.SinusoidHorizontal,
            "&Horizontal Sine Wave",
            "Place the vertices along a sine wave running left to right."
            ),

        new LayoutInfo(
            LayoutType.SinusoidVertical,
            "&Vertical Sine Wave",
            "Place the vertices along a sine wave running top to bottom."
            ),

        new LayoutInfo(
            LayoutType.Grid,
            "&Grid",
            "Place the vertices on an evenly-spaced grid."
            ),

        new LayoutInfo(
            LayoutType.Polar,
            "&Polar",

            "Place the vertices in a polar coordinate space using the"
            + " Polar R and Polar Angle columns on the Vertices worksheet,"
            + " where Polar R ranges from 0 to 1."
            ),

        new LayoutInfo(
            LayoutType.PolarAbsolute,
            "Polar &Absolute",

            "Place the vertices in a polar coordinate space using the"
            + " Polar R and Polar Angle columns on the Vertices worksheet,"
            + " where Polar R is in absolute units of 1/96 inch."
            ),

        LayoutGroupSeparator,

        new LayoutInfo(
            LayoutType.Sugiyama,
            "Sugi&yama",

            "Use a modified Sugiyama layered layout that tries to minimize"
            + " edge crossings."
            ),

        new LayoutInfo(
            LayoutType.Random,
            "&Random",
            "Place the vertices at random locations."
            ),

        LayoutGroupSeparator,

        new LayoutInfo(
            LayoutType.Null,
            "&None",
            "Leave the vertices exactly where they are."
            ),
        };
}

}
