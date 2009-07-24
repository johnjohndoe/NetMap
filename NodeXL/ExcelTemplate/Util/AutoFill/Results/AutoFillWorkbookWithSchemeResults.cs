
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Globalization;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AutoFillWorkbookWithSchemeResults
//
/// <summary>
/// Stores the results of a call to a <see cref="WorkbookSchemeAutoFiller" />
/// method.
/// </summary>
///
/// <remarks>
/// The object can be persisted to and from a string using <see
/// cref="ConvertToString()" /> and <see cref="ConvertFromString" />.
/// </remarks>
//*****************************************************************************

public class AutoFillWorkbookWithSchemeResults : Object
{
    //*************************************************************************
    //  Constructor: AutoFillWorkbookWithSchemeResults()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AutoFillWorkbookWithSchemeResults" /> class.
    /// </summary>
    //*************************************************************************

    public AutoFillWorkbookWithSchemeResults()
    {
        m_eSchemeType = AutoFillSchemeType.None;

        m_sVertexCategoryColumnName = null;
        m_asVertexCategoryNames = null;

        m_sEdgeWeightColumnName = null;
        m_dEdgeWeightSourceCalculationNumber1 = 0;
        m_dEdgeWeightSourceCalculationNumber2 = 0;

        m_sEdgeTimestampColumnName = null;
        m_eEdgeTimestampColumnFormat = ExcelColumnFormat.Other;
        m_dEdgeTimestampSourceCalculationNumber1 = 0;
        m_dEdgeTimestampSourceCalculationNumber2 = 0;

        AssertValid();
    }

    //*************************************************************************
    //  Property: SchemeType
    //
    /// <summary>
    /// Gets the type of scheme that was autofilled.
    /// </summary>
    ///
    /// <value>
    /// A GraphDirectedness value.
    /// The type of scheme that was autofilled.
    /// </value>
    //*************************************************************************

    public AutoFillSchemeType
    SchemeType
    {
        get
        {
            AssertValid();

            return (m_eSchemeType);
        }
    }

    //*************************************************************************
    //  Method: SetVertexCategoryResults()
    //
    /// <summary>
    /// Stores the results of a call to <see
    /// cref="WorkbookSchemeAutoFiller.AutoFillByVertexCategory(
    /// Microsoft.Office.Interop.Excel.Workbook, String, Boolean, String)" />.
    /// </summary>
    ///
    /// <param name="vertexCategoryColumnName">
    /// The name of the vertex table column containing vertex categories.
    /// </param>
    ///
    /// <param name="vertexCategoryNames">
    /// An array of category names, indexed by vertex category scheme.
    /// </param>
    ///
    /// <remarks>
    /// Vertex category schemes can be obtained from <see
    /// cref="WorkbookSchemeAutoFiller.GetVertexCategoryScheme" />.
    /// </remarks>
    //*************************************************************************

    public void
    SetVertexCategoryResults
    (
        String vertexCategoryColumnName,
        String [] vertexCategoryNames
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(vertexCategoryColumnName) );
        Debug.Assert(vertexCategoryNames != null);
        AssertValid();

        m_eSchemeType = AutoFillSchemeType.VertexCategory;
        m_sVertexCategoryColumnName = vertexCategoryColumnName;
        m_asVertexCategoryNames = vertexCategoryNames;
    }

    //*************************************************************************
    //  Method: GetVertexCategoryResults()
    //
    /// <summary>
    /// Gets the results of a call to <see
    /// cref="WorkbookSchemeAutoFiller.AutoFillByVertexCategory(
    /// Microsoft.Office.Interop.Excel.Workbook, String, Boolean, String)" />.
    /// </summary>
    ///
    /// <param name="vertexCategoryColumnName">
    /// Where the name of the vertex table column containing vertex categories
    /// gets stored.
    /// </param>
    ///
    /// <param name="vertexCategoryNames">
    /// Where an array of category names indexed by vertex category scheme gets
    /// stored.
    /// </param>
    ///
    /// <remarks>
    /// Call this only if <see cref="Type" /> returns <see
    /// cref="AutoFillSchemeType.VertexCategory" />.
    /// </remarks>
    ///
    /// <remarks>
    /// Vertex category schemes can be obtained from <see
    /// cref="WorkbookSchemeAutoFiller.GetVertexCategoryScheme" />.
    /// </remarks>
    //*************************************************************************

    public void
    GetVertexCategoryResults
    (
        out String vertexCategoryColumnName,
        out String [] vertexCategoryNames
    )
    {
        Debug.Assert(m_eSchemeType == AutoFillSchemeType.VertexCategory);
        AssertValid();

        vertexCategoryColumnName = m_sVertexCategoryColumnName;
        vertexCategoryNames = m_asVertexCategoryNames;
    }

    //*************************************************************************
    //  Method: SetEdgeWeightResults()
    //
    /// <summary>
    /// Stores the results of a call to <see
    /// cref="WorkbookSchemeAutoFiller.AutoFillByEdgeWeight" />.
    /// </summary>
    ///
    /// <param name="edgeWeightColumnName">
    /// The name of the edge table column containing edge weights.
    /// </param>
    ///
    /// <param name="sourceCalculationNumber1">
    /// The actual first source number used in the calculations.
    /// </param>
    ///
    /// <param name="sourceCalculationNumber2">
    /// The actual second source number used in the calculations.
    /// </param>
    //*************************************************************************

    public void
    SetEdgeWeightResults
    (
        String edgeWeightColumnName,
        Double sourceCalculationNumber1,
        Double sourceCalculationNumber2
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(edgeWeightColumnName) );
        AssertValid();

        m_eSchemeType = AutoFillSchemeType.EdgeWeight;
        m_sEdgeWeightColumnName = edgeWeightColumnName;
        m_dEdgeWeightSourceCalculationNumber1 = sourceCalculationNumber1;
        m_dEdgeWeightSourceCalculationNumber2 = sourceCalculationNumber2;
    }

    //*************************************************************************
    //  Method: GetEdgeWeightResults()
    //
    /// <summary>
    /// Gets the results of a call to <see
    /// cref="WorkbookSchemeAutoFiller.AutoFillByEdgeWeight" />.
    /// </summary>
    ///
    /// <param name="edgeWeightColumnName">
    /// Where the name of the edge table column containing edge weights gets
    /// stored.
    /// </param>
    ///
    /// <param name="sourceCalculationNumber1">
    /// Where the actual first source number used in the calculations gets
    /// stored if true is returned.
    /// </param>
    ///
    /// <param name="sourceCalculationNumber2">
    /// Where the actual second source number used in the calculations gets
    /// stored if true is returned.
    /// </param>
    ///
    /// <remarks>
    /// Call this only if <see cref="SchemeType" /> returns <see
    /// cref="AutoFillSchemeType.EdgeWeight" />.
    /// </remarks>
    //*************************************************************************

    public void
    GetEdgeWeightResults
    (
        out String edgeWeightColumnName,
        out Double sourceCalculationNumber1,
        out Double sourceCalculationNumber2
    )
    {
        Debug.Assert(m_eSchemeType == AutoFillSchemeType.EdgeWeight);
        AssertValid();

        edgeWeightColumnName = m_sEdgeWeightColumnName;
        sourceCalculationNumber1 = m_dEdgeWeightSourceCalculationNumber1;
        sourceCalculationNumber2 = m_dEdgeWeightSourceCalculationNumber2;
    }

    //*************************************************************************
    //  Method: SetEdgeTimestampResults()
    //
    /// <summary>
    /// Stores the results of a call to <see
    /// cref="WorkbookSchemeAutoFiller.AutoFillByEdgeTimestamp" />.
    /// </summary>
    ///
    /// <param name="edgeTimestampColumnName">
    /// The name of the edge table column containing edge timestamps.
    /// </param>
    ///
    /// <param name="edgeTimestampColumnFormat">
    /// The format of the edge table column containing edge timestamps.
    /// </param>
    ///
    /// <param name="sourceCalculationNumber1">
    /// The actual first source number used in the calculations.
    /// </param>
    ///
    /// <param name="sourceCalculationNumber2">
    /// The actual second source number used in the calculations.
    /// </param>
    //*************************************************************************

    public void
    SetEdgeTimestampResults
    (
        String edgeTimestampColumnName,
        ExcelColumnFormat edgeTimestampColumnFormat,
        Double sourceCalculationNumber1,
        Double sourceCalculationNumber2
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(edgeTimestampColumnName) );
        AssertValid();

        m_eSchemeType = AutoFillSchemeType.EdgeTimestamp;
        m_sEdgeTimestampColumnName = edgeTimestampColumnName;
        m_eEdgeTimestampColumnFormat = edgeTimestampColumnFormat;
        m_dEdgeTimestampSourceCalculationNumber1 = sourceCalculationNumber1;
        m_dEdgeTimestampSourceCalculationNumber2 = sourceCalculationNumber2;
    }

    //*************************************************************************
    //  Method: GetEdgeTimestampResults()
    //
    /// <summary>
    /// Gets the results of a call to <see
    /// cref="WorkbookSchemeAutoFiller.AutoFillByEdgeTimestamp" />.
    /// </summary>
    ///
    /// <param name="edgeTimestampColumnName">
    /// Where the name of the edge table column containing edge timestamps gets
    /// stored.
    /// </param>
    ///
    /// <param name="edgeTimestampColumnFormat">
    /// Where the format of the edge table column containing edge timestamps
    /// gets stored.
    /// </param>
    ///
    /// <param name="sourceCalculationNumber1">
    /// Where the actual first source number used in the calculations gets
    /// stored if true is returned.
    /// </param>
    ///
    /// <param name="sourceCalculationNumber2">
    /// Where the actual second source number used in the calculations gets
    /// stored if true is returned.
    /// </param>
    ///
    /// <remarks>
    /// Call this only if <see cref="Type" /> returns <see
    /// cref="AutoFillSchemeType.EdgeTimestamp" />.
    /// </remarks>
    //*************************************************************************

    public void
    GetEdgeTimestampResults
    (
        out String edgeTimestampColumnName,
        out ExcelColumnFormat edgeTimestampColumnFormat,
        out Double sourceCalculationNumber1,
        out Double sourceCalculationNumber2
    )
    {
        Debug.Assert(m_eSchemeType == AutoFillSchemeType.EdgeTimestamp);
        AssertValid();

        edgeTimestampColumnName = m_sEdgeTimestampColumnName;
        edgeTimestampColumnFormat = m_eEdgeTimestampColumnFormat;
        sourceCalculationNumber1 = m_dEdgeTimestampSourceCalculationNumber1;
        sourceCalculationNumber2 = m_dEdgeTimestampSourceCalculationNumber2;
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

    public String
    ConvertToString()
    {
        AssertValid();

        // Use string concatenation.

        IFormatProvider oInvariantCulture = CultureInfo.InvariantCulture;
        String sSchemeType = m_eSchemeType.ToString();

        switch (m_eSchemeType)
        {
            case AutoFillSchemeType.None:

                return (sSchemeType);

            case AutoFillSchemeType.VertexCategory:

                return ( String.Join(
                    AutoFillWorkbookResults.FieldSeparatorString,

                    new String [] {
                        sSchemeType,
                        m_sVertexCategoryColumnName,

                        String.Join(VertexCategoryNameSeparatorString,
                            m_asVertexCategoryNames)
                    } ) );

            case AutoFillSchemeType.EdgeWeight:

                return ( String.Join(
                    AutoFillWorkbookResults.FieldSeparatorString,

                    new String [] {
                        sSchemeType,
                        m_sEdgeWeightColumnName,

                        m_dEdgeWeightSourceCalculationNumber1.ToString(
                            oInvariantCulture),

                        m_dEdgeWeightSourceCalculationNumber2.ToString(
                            oInvariantCulture)
                    } ) );

            case AutoFillSchemeType.EdgeTimestamp:

                return ( String.Join(
                    AutoFillWorkbookResults.FieldSeparatorString,

                    new String [] {
                        sSchemeType,
                        m_sEdgeTimestampColumnName,
                        m_eEdgeTimestampColumnFormat.ToString(),

                        m_dEdgeTimestampSourceCalculationNumber1.ToString(
                            oInvariantCulture),

                        m_dEdgeTimestampSourceCalculationNumber2.ToString(
                            oInvariantCulture)
                    } ) );

            default:

                Debug.Assert(false);
                return (null);
        }
    }

    //*************************************************************************
    //  Method: ConvertFromString()
    //
    /// <summary>
    /// Creates a <see cref="AutoFillWorkbookWithSchemeResults" /> object from
    /// a persisted string.
    /// </summary>
    ///
    /// <param name="theString">
    /// String created by <see cref="ConvertToString()" />.
    /// </param>
    ///
    /// <returns>
    /// A <see cref="AutoFillWorkbookWithSchemeResults" /> object created from
    /// <paramref name="theString" />.
    /// </returns>
    //*************************************************************************

    public static AutoFillWorkbookWithSchemeResults
    ConvertFromString
    (
        String theString
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(theString) );

        AutoFillWorkbookWithSchemeResults oAutoFillWorkbookWithSchemeResults =
            new AutoFillWorkbookWithSchemeResults();

        String [] asFields = theString.Split(
            AutoFillWorkbookResults.FieldSeparator);

        Int32 iFields = asFields.Length;

        if (iFields == 0)
        {
            goto Done;
        }

        AutoFillSchemeType eSchemeType;

        try
        {
            eSchemeType = (AutoFillSchemeType)Enum.Parse(
                typeof(AutoFillSchemeType), asFields[0] );
        }
        catch (ArgumentException)
        {
            goto Done;
        }

        switch (eSchemeType)
        {
            case AutoFillSchemeType.VertexCategory:

                if (iFields == 3)
                {
                    String [] asVertexCategoryNames = asFields[2].Split(
                        VertexCategoryNameSeparator);

                    oAutoFillWorkbookWithSchemeResults.
                        SetVertexCategoryResults(asFields[1],
                            asVertexCategoryNames);
                }

                break;

            case AutoFillSchemeType.EdgeWeight:

                if (iFields == 4)
                {
                    oAutoFillWorkbookWithSchemeResults.SetEdgeWeightResults(
                        asFields[1],
                        MathUtil.ParseCultureInvariantDouble( asFields[2] ),
                        MathUtil.ParseCultureInvariantDouble( asFields[3] )
                        );
                }

                break;

            case AutoFillSchemeType.EdgeTimestamp:

                if (iFields == 5)
                {
                    oAutoFillWorkbookWithSchemeResults.SetEdgeTimestampResults(
                        asFields[1],

                        (ExcelColumnFormat)Enum.Parse(
                            typeof(ExcelColumnFormat), asFields[2] ),

                        MathUtil.ParseCultureInvariantDouble( asFields[3] ),
                        MathUtil.ParseCultureInvariantDouble( asFields[4] )
                        );
                }

                break;

            default:

                break;
        }

        Done:

        return (oAutoFillWorkbookWithSchemeResults);
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
        // m_eSchemeType

        #if DEBUG

        switch (m_eSchemeType)
        {
            case AutoFillSchemeType.None:

                break;

            case AutoFillSchemeType.VertexCategory:

                Debug.Assert( !String.IsNullOrEmpty(
                    m_sVertexCategoryColumnName) );

                Debug.Assert(m_asVertexCategoryNames != null);
                break;

            case AutoFillSchemeType.EdgeWeight:

                Debug.Assert( !String.IsNullOrEmpty(m_sEdgeWeightColumnName) );
                break;

            case AutoFillSchemeType.EdgeTimestamp:

                Debug.Assert( !String.IsNullOrEmpty(
                    m_sEdgeTimestampColumnName) );

                break;

            default:

                Debug.Assert(false);
                break;
        }

        #endif
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// Vertex category name separator used by ConvertToString() and
    /// ConvertFromString().  This is the Unicode "light shade" character,
    /// which is unlikely to be used in category names.  This is defined in
    /// both character array and string versions to accommodate String.Split()
    /// and String.Join().

    public static readonly Char [] VertexCategoryNameSeparator =
        new Char[] {'\u2591'};
    ///
    public const String VertexCategoryNameSeparatorString = "\u2591";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Specifies the type of scheme that was autofilled.

    protected AutoFillSchemeType m_eSchemeType;


    //**************************
    //  Vertex categories
    //**************************

    /// The name of the vertex table column containing vertex categories.

    protected String m_sVertexCategoryColumnName;

    /// An array of category names.

    protected String [] m_asVertexCategoryNames;


    //**************************
    //  Edge weights
    //**************************

    /// The name of the edge table column containing edge weights.

    protected String m_sEdgeWeightColumnName;

    /// The actual first source number used in the calculations.

    protected Double m_dEdgeWeightSourceCalculationNumber1;

    /// The actual second source number used in the calculations.

    protected Double m_dEdgeWeightSourceCalculationNumber2;


    //**************************
    //  Edge timestamps
    //**************************

    /// The name of the edge table column containing edge timestamps.

    protected String m_sEdgeTimestampColumnName;

    /// The format of the edge table column containing edge timestamps.

    protected ExcelColumnFormat m_eEdgeTimestampColumnFormat;

    /// The actual first source number used in the calculations.

    protected Double m_dEdgeTimestampSourceCalculationNumber1;

    /// The actual second source number used in the calculations.

    protected Double m_dEdgeTimestampSourceCalculationNumber2;
}

}
