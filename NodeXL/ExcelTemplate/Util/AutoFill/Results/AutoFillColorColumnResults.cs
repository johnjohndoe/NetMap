
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AutoFillColorColumnResults
//
/// <summary>
/// Stores one color column of the results of a call to <see
/// cref="WorkbookAutoFiller.AutoFillWorkbook" />.
/// </summary>
//*****************************************************************************

public class AutoFillColorColumnResults : AutoFillColumnResults
{
    //*************************************************************************
    //  Constructor: AutoFillColorColumnResults()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
    /// cref="AutoFillColorColumnResults" /> class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AutoFillColorColumnResults" /> class with default values that
    /// indicate that the column wasn't autofilled.
    /// </summary>
    //*************************************************************************

    public AutoFillColorColumnResults()
    : this(null, 0, 0, 0, Color.Black, Color.Black)
    {
        // (Do nothing else.)

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: AutoFillColorColumnResults()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AutoFillColorColumnResults" /> class with specified
    /// values.
    /// </summary>
    ///
    /// <param name="sourceColumnName">
    /// Name of the source column, or null if the column wasn't autofilled.
    /// </param>
    ///
    /// <param name="sourceCalculationNumber1">
    /// The actual first source number used in the calculations.
    /// </param>
    ///
    /// <param name="sourceCalculationNumber2">
    /// The actual second source number used in the calculations.
    /// </param>
    ///
    /// <param name="decimalPlaces">
    /// The number of decimal places displayed in the column.
    /// </param>
    ///
    /// <param name="destinationColor1">
    /// The first color used in the destination column.
    /// </param>
    ///
    /// <param name="destinationColor2">
    /// The second color used in the destination column.
    /// </param>
    //*************************************************************************

    public AutoFillColorColumnResults
    (
        String sourceColumnName,
        Double sourceCalculationNumber1,
        Double sourceCalculationNumber2,
        Int32 decimalPlaces,
        Color destinationColor1,
        Color destinationColor2
    )
    : base(sourceColumnName, sourceCalculationNumber1,
        sourceCalculationNumber2, decimalPlaces)
    {
        m_oDestinationColor1 = destinationColor1;
        m_oDestinationColor2 = destinationColor2;

        AssertValid();
    }

    //*************************************************************************
    //  Property: DestinationColor1
    //
    /// <summary>
    /// Gets the first color used in the destination column.
    /// </summary>
    ///
    /// <returns>
    /// The first color used in the destination column, or Color.Black if the
    /// column wasn't autofilled.
    /// </returns>
    //*************************************************************************

    public Color
    DestinationColor1
    {
        get
        {
            AssertValid();

            return (m_oDestinationColor1);
        }
    }

    //*************************************************************************
    //  Property: DestinationColor2
    //
    /// <summary>
    /// Gets the second color used in the destination column.
    /// </summary>
    ///
    /// <returns>
    /// The second color used in the destination column, or Color.Black if the
    /// column wasn't autofilled.
    /// </returns>
    //*************************************************************************

    public Color
    DestinationColor2
    {
        get
        {
            AssertValid();

            return (m_oDestinationColor2);
        }
    }

    //*************************************************************************
    //  Method: ConvertToString()
    //
    /// <summary>
    /// Converts the object to a string that can be persisted.
    /// </summary>
    ///
    /// <returns>
    /// The object converted into a String.
    /// </returns>
    ///
    /// <remarks>
    /// Use <see cref="ConvertFromString" /> to go in the other direction.
    /// </remarks>
    //*************************************************************************

    public new String
    ConvertToString()
    {
        AssertValid();

        // Use string concatenation.

        ColorConverter oColorConverter = new ColorConverter();

        return ( String.Join(PerWorkbookSettings.FieldSeparatorString,

            new String [] {
                base.ConvertToString(),
                oColorConverter.ConvertToString(m_oDestinationColor1),
                oColorConverter.ConvertToString(m_oDestinationColor2),
            } ) );
    }

    //*************************************************************************
    //  Method: ConvertFromString()
    //
    /// <summary>
    /// Sets the column mapping from a persisted string.
    /// </summary>
    ///
    /// <param name="asFields">
    /// Array of column result fields.
    /// </param>
    ///
    /// <param name="iStartIndex">
    /// Index of the next unread field in <paramref name="asFields" />.
    /// </param>
    ///
    /// <returns>
    /// Index of the next unread field in <paramref name="asFields" />.
    /// </returns>
    //*************************************************************************

    public new Int32
    ConvertFromString
    (
        String [] asFields,
        Int32 iStartIndex
    )
    {
        Debug.Assert(asFields != null);
        Debug.Assert(iStartIndex >= 0);

        iStartIndex = base.ConvertFromString(asFields, iStartIndex);

        Debug.Assert(iStartIndex + 1 < asFields.Length);

        ColorConverter oColorConverter = new ColorConverter();

        m_oDestinationColor1 = (Color)oColorConverter.ConvertFromString(
            asFields[iStartIndex + 0] );

        m_oDestinationColor2 = (Color)oColorConverter.ConvertFromString(
            asFields[iStartIndex + 1] );

        // Note:
        //
        // If another field is added here, the total expected field count in 
        // AutoFillWorkbookResults.ConvertFromString() must be increased.

        return (iStartIndex + 2);
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

        // m_oDestinationColor1
        // m_oDestinationColor2
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The first color used in the destination column.

    protected Color m_oDestinationColor1;

    /// The second color used in the destination column.

    protected Color m_oDestinationColor2;
}

}
