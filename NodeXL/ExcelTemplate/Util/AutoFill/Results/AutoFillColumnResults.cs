
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Globalization;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AutoFillColumnResults
//
/// <summary>
/// Stores one column of the results of a call to <see
/// cref="WorkbookAutoFiller.AutoFillWorkbook" />.
/// </summary>
///
/// <remarks>
/// This is the base class for derived classes that store one column of the
/// results of a call to <see cref="WorkbookAutoFiller.AutoFillWorkbook" />.
/// See <see cref="AutoFillWorkbookResults" /> for more details.
/// </remarks>
//*****************************************************************************

public class AutoFillColumnResults : Object
{
    //*************************************************************************
    //  Constructor: AutoFillColumnResults()
    //
    /// <overloads>
    /// Initializes a new instance of the <see cref="AutoFillColumnResults" />
    /// class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoFillColumnResults" />
    /// class with default values that indicate that the column wasn't
    /// autofilled.
    /// </summary>
    //*************************************************************************

    public AutoFillColumnResults()
    : this(null, 0, 0, 0)
    {
        // (Do nothing else.)

        // AssertValid();
    }

    //*************************************************************************
    //  Constructor: AutoFillColumnResults()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoFillColumnResults" />
    /// class with specified values.
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
    //*************************************************************************

    public AutoFillColumnResults
    (
        String sourceColumnName,
        Double sourceCalculationNumber1,
        Double sourceCalculationNumber2,
        Int32 decimalPlaces
    )
    {
        m_sSourceColumnName = sourceColumnName;
        m_dSourceCalculationNumber1 = sourceCalculationNumber1;
        m_dSourceCalculationNumber2 = sourceCalculationNumber2;
        m_iDecimalPlaces = decimalPlaces;

        // AssertValid();
    }

    //*************************************************************************
    //  Property: ColumnAutoFilled
    //
    /// <summary>
    /// Gets a flag indicating whether the column was autofilled.
    /// </summary>
    ///
    /// <returns>
    /// true if the column was autofilled.
    /// </returns>
    //*************************************************************************

    public Boolean
    ColumnAutoFilled
    {
        get
        {
            AssertValid();

            return ( !String.IsNullOrEmpty(m_sSourceColumnName) );
        }
    }

    //*************************************************************************
    //  Property: AutoFilledColumnCount
    //
    /// <summary>
    /// Gets the number of columns that were autofilled.
    /// </summary>
    ///
    /// <returns>
    /// 1 if the column was autofilled, 0 if it wasn't.
    /// </returns>
    ///
    /// <remarks>
    /// This is a convenience property for classes that need to count the
    /// number of autofilled columns.
    /// </remarks>
    //*************************************************************************

    public Int32
    AutoFilledColumnCount
    {
        get
        {
            AssertValid();

            return (m_sSourceColumnName == null ? 0 : 1);
        }
    }

    //*************************************************************************
    //  Property: SourceColumnName
    //
    /// <summary>
    /// Gets the name of the source column.
    /// </summary>
    ///
    /// <returns>
    /// The name of the source column, or null if the column wasn't autofilled.
    /// </returns>
    //*************************************************************************

    public String
    SourceColumnName
    {
        get
        {
            AssertValid();

            return (m_sSourceColumnName);
        }
    }

    //*************************************************************************
    //  Property: SourceCalculationNumber1
    //
    /// <summary>
    /// Gets the actual first source number used in the calculations.
    /// </summary>
    ///
    /// <returns>
    /// The actual first source number used in the calculations, or 0 if the
    /// column wasn't autofilled.
    /// </returns>
    //*************************************************************************

    public Double
    SourceCalculationNumber1
    {
        get
        {
            AssertValid();

            return (m_dSourceCalculationNumber1);
        }
    }

    //*************************************************************************
    //  Property: SourceCalculationNumber2
    //
    /// <summary>
    /// Gets the actual second source number used in the calculations.
    /// </summary>
    ///
    /// <returns>
    /// The actual second source number used in the calculations, or 0 if the
    /// column wasn't autofilled.
    /// </returns>
    //*************************************************************************

    public Double
    SourceCalculationNumber2
    {
        get
        {
            AssertValid();

            return (m_dSourceCalculationNumber2);
        }
    }

    //*************************************************************************
    //  Property: DecimalPlaces
    //
    /// <summary>
    /// Gets the number of decimal places displayed in the column.
    /// </summary>
    ///
    /// <value>
    /// The number of decimal places displayed in the column, as an Int32.
    /// </value>
    //*************************************************************************

    public Int32
    DecimalPlaces
    {
        get
        {
            AssertValid();

            return (m_iDecimalPlaces);
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

    protected String
    ConvertToString()
    {
        AssertValid();

        // Use string concatenation.

        IFormatProvider oInvariantCulture = CultureInfo.InvariantCulture;

        return (String.Join(PerWorkbookSettings.FieldSeparatorString,

            new String [] {

            m_sSourceColumnName,
            m_dSourceCalculationNumber1.ToString(oInvariantCulture),
            m_dSourceCalculationNumber2.ToString(oInvariantCulture),
            m_iDecimalPlaces.ToString(oInvariantCulture),
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

    protected Int32
    ConvertFromString
    (
        String [] asFields,
        Int32 iStartIndex
    )
    {
        Debug.Assert(asFields != null);
        Debug.Assert(iStartIndex >= 0);
        Debug.Assert(iStartIndex + 3 < asFields.Length);

        m_sSourceColumnName = asFields[iStartIndex + 0];

        if ( String.IsNullOrEmpty(m_sSourceColumnName) )
        {
            m_sSourceColumnName = null;
        }

        m_dSourceCalculationNumber1 = MathUtil.ParseCultureInvariantDouble(
            asFields[iStartIndex + 1] );

        m_dSourceCalculationNumber2 = MathUtil.ParseCultureInvariantDouble(
            asFields[iStartIndex + 2] );

        m_iDecimalPlaces = MathUtil.ParseCultureInvariantInt32(
            asFields[iStartIndex + 3] );

        // Note:
        //
        // If another field is added here, the total expected field count in 
        // AutoFillWorkbookResults.ConvertFromString() must be increased.

        return (iStartIndex + 4);
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
        // m_sSourceColumnName
        // m_dSourceCalculationNumber1
        // m_dSourceCalculationNumber2
        Debug.Assert(m_iDecimalPlaces >= 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Name of the source column, or null if the column wasn't autofilled.

    protected String m_sSourceColumnName;

    /// The actual first source number used in the calculations.

    protected Double m_dSourceCalculationNumber1;

    /// The actual second source number used in the calculations.

    protected Double m_dSourceCalculationNumber2;

    /// The number of decimal places displayed in the column.

    protected Int32 m_iDecimalPlaces;
}

}
