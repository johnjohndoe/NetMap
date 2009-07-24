
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Globalization;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AutoFillNumericRangeColumnResults
//
/// <summary>
/// Stores one numeric range column of the results of a call to <see
/// cref="WorkbookAutoFiller.AutoFillWorkbook" />.
/// </summary>
//*****************************************************************************

public class AutoFillNumericRangeColumnResults : AutoFillColumnResults
{
    //*************************************************************************
    //  Constructor: AutoFillNumericRangeColumnResults()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
    /// cref="AutoFillNumericRangeColumnResults" /> class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AutoFillNumericRangeColumnResults" /> class with default values.
    /// that indicate that the column wasn't autofilled.
    /// </summary>
    //*************************************************************************

    public AutoFillNumericRangeColumnResults()
    :
    this(null, 0, 0, 0, 0)
    {
        // (Do nothing else.)

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: AutoFillNumericRangeColumnResults()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AutoFillNumericRangeColumnResults" /> class with specified
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
    /// <param name="destinationNumber1">
    /// The first number used in the destination column.
    /// </param>
    ///
    /// <param name="destinationNumber2">
    /// The second number used in the destination column.
    /// </param>
    //*************************************************************************

    public AutoFillNumericRangeColumnResults
    (
        String sourceColumnName,
        Double sourceCalculationNumber1,
        Double sourceCalculationNumber2,
        Double destinationNumber1,
        Double destinationNumber2
    )
    : base(sourceColumnName, sourceCalculationNumber1,
        sourceCalculationNumber2)
    {
        m_dDestinationNumber1 = destinationNumber1;
        m_dDestinationNumber2 = destinationNumber2;

        AssertValid();
    }

    //*************************************************************************
    //  Property: DestinationNumber1
    //
    /// <summary>
    /// Gets the first number used in the destination column.
    /// </summary>
    ///
    /// <returns>
    /// The first number used in the destination column, or 0 if the column
    /// wasn't autofilled.
    /// </returns>
    //*************************************************************************

    public Double
    DestinationNumber1
    {
        get
        {
            AssertValid();

            return (m_dDestinationNumber1);
        }
    }

    //*************************************************************************
    //  Property: DestinationNumber2
    //
    /// <summary>
    /// Gets the second number used in the destination column.
    /// </summary>
    ///
    /// <returns>
    /// The second number used in the destination column, or 0 if the column
    /// wasn't autofilled.
    /// </returns>
    //*************************************************************************

    public Double
    DestinationNumber2
    {
        get
        {
            AssertValid();

            return (m_dDestinationNumber2);
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

        IFormatProvider oInvariantCulture = CultureInfo.InvariantCulture;

        return (String.Join(AutoFillWorkbookResults.FieldSeparatorString,

            new String [] {
                base.ConvertToString(),
                m_dDestinationNumber1.ToString(oInvariantCulture),
                m_dDestinationNumber2.ToString(oInvariantCulture),
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
        Debug.Assert(asFields.Length >= 5);
        Debug.Assert(iStartIndex >= 0);

        iStartIndex = base.ConvertFromString(asFields, iStartIndex);

        m_dDestinationNumber1 = MathUtil.ParseCultureInvariantDouble(
            asFields[iStartIndex + 0] );

        m_dDestinationNumber2 = MathUtil.ParseCultureInvariantDouble(
            asFields[iStartIndex + 1] );

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

        // m_dDestinationNumber1
        // m_dDestinationNumber2
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The first number used in the destination column.

    protected Double m_dDestinationNumber1;

    /// The second number used in the destination column.

    protected Double m_dDestinationNumber2;
}

}
