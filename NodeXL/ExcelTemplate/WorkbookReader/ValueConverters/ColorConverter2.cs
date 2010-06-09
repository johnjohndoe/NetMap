
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ColorConverter2
//
/// <summary>
/// Class that converts a color between values used in the Excel workbook and
/// values used in the NodeXL graph.
/// </summary>
///
/// <remarks>
/// This is called ColorConverter2 to distinguish it from
/// System.Drawing.ColorConverter without requiring the specification of a
/// namespace.
///
/// <para>
/// Colors are treated differently from the other value types that derive from
/// TextValueConverterBase, so this class doesn't derive from that base class.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class ColorConverter2 : Object
{
    //*************************************************************************
    //  Constructor: ColorConverter2()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ColorConverter2" /> class.
    /// </summary>
    //*************************************************************************

    public ColorConverter2()
    {
        m_oColorConverter = new System.Drawing.ColorConverter();

        // System.Drawing.ColorConverter.ConvertToString() converts a common
        // Color, such as (255,0,0), to its common name, such as "Red", only if
        // the Color was originally created from a name.  Otherwise, it
        // converts the color to the string "255, 0, 0".  To make colors more
        // readable, this class explicitly converts common colors to their
        // names using a dictionary lookup.

        m_oCommonColorNames = new Dictionary<Color, String>();

        m_oCommonColorNames.Add(Color.FromArgb(  0,   0,   0), "Black");
        m_oCommonColorNames.Add(Color.FromArgb(255, 255, 255), "White");

        m_oCommonColorNames.Add(Color.FromArgb(  0,   0, 255), "Blue");
        m_oCommonColorNames.Add(Color.FromArgb(  0, 255,   0), "Lime");
        m_oCommonColorNames.Add(Color.FromArgb(255,   0,   0), "Red");

        m_oCommonColorNames.Add(Color.FromArgb(0,   255, 255), "Cyan");
        m_oCommonColorNames.Add(Color.FromArgb(255, 255,   0), "Yellow");
        m_oCommonColorNames.Add(Color.FromArgb(255,   0, 255), "Magenta");

        m_oCommonColorNames.Add(Color.FromArgb(255, 165,   0), "Orange");
        m_oCommonColorNames.Add(Color.FromArgb(0,   128,   0), "Green");

        AssertValid();
    }

    //*************************************************************************
    //  Method: TryWorkbookToGraph()
    //
    /// <summary>
    /// Attempts to convert an Excel workbook value to a value suitable for use
    /// in a NodeXL graph.
    /// </summary>
    ///
    /// <param name="workbookValue">
    /// Value read from the Excel workbook.
    /// </param>
    ///
    /// <param name="graphValue">
    /// Where a value suitable for use in a NodeXL graph gets stored if true is
    /// returned.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="workbookValue" /> contains a valid workbook
    /// value.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="workbookValue" /> contains a valid workbook value,
    /// the corresponding graph value gets stored at <paramref
    /// name="graphValue" /> and true is returned.  Otherwise, false is
    /// returned.
    /// </remarks>
    //*************************************************************************

    public Boolean
    TryWorkbookToGraph
    (
        String workbookValue,
        out Color graphValue
    )
    {
        Debug.Assert(workbookValue != null);
        AssertValid();

        graphValue = Color.Empty;

        // Colors can include optional spaces between words: the color
        // LightBlue can be specified as either "Light Blue" or "LightBlue",
        // for example.

        workbookValue = workbookValue.Replace(" ", String.Empty);

        if (workbookValue.Length == 0)
        {
            // ColorConverter converts an empty string to Color.Black.  Bypass
            // ColorConverter.

            return (false);
        }

        try
        {
            graphValue = (Color)m_oColorConverter.ConvertFromString(
                workbookValue);
        }
        catch (Exception)
        {
            // (Format errors raise a System.Exception with an inner exception
            // of type FormatException.  Go figure.)

            return (false);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: GraphToWorkbook()
    //
    /// <summary>
    /// Converts a NodeXL graph value to a value suitable for use in an Excel
    /// workbook.
    /// </summary>
    ///
    /// <param name="graphValue">
    /// Value stored in a NodeXL graph.
    /// </param>
    ///
    /// <returns>
    /// A value suitable for use in an Excel workbook.
    /// </returns>
    //*************************************************************************

    public String
    GraphToWorkbook
    (
        Color graphValue
    )
    {
        AssertValid();

        String sWorkbookValue;

        // For common colors, use the color name.

        if ( !m_oCommonColorNames.TryGetValue(graphValue, out sWorkbookValue) )
        {
            sWorkbookValue = m_oColorConverter.ConvertToString(graphValue);
        }

        return (sWorkbookValue);
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
        Debug.Assert(m_oColorConverter != null);
        Debug.Assert(m_oCommonColorNames != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object that does most of the work.

    protected System.Drawing.ColorConverter m_oColorConverter;

    /// Maps common colors to their names.

    protected Dictionary<Color, String> m_oCommonColorNames;
}

}
