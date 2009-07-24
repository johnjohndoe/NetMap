
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AutoFillWorkbookResults
//
/// <summary>
/// Stores the results of a call to <see
/// cref="WorkbookAutoFiller.AutoFillWorkbook" />.
/// </summary>
///
/// <remarks>
/// If a caller of <see cref="WorkbookAutoFiller.AutoFillWorkbook" /> needs to
/// know how the workbook was autofilled, it can't just look at the <see
/// cref="AutoFillUserSettings" /> object it passes in.  That's because one or
/// more columns may be autofilled using the minimum or maximum column values,
/// and the caller doesn't know those values.  This <see
/// cref="AutoFillWorkbookResults" /> class, which is returned by <see
/// cref="WorkbookAutoFiller.AutoFillWorkbook" />, provides the required
/// information.
///
/// <para>
/// The object can be persisted to and from a string using <see
/// cref="ConvertToString()" /> and <see cref="ConvertFromString" />.
/// </para>
///
/// <para>
/// Currently, only the results needed by <see
/// cref="AutoFillResultsLegendControl" /> and the graph axes are stored.  This
/// may be expanded in the future.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class AutoFillWorkbookResults : Object
{
    //*************************************************************************
    //  Constructor: AutoFillWorkbookResults()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AutoFillWorkbookResults" /> class.
    /// </summary>
    //*************************************************************************

    public AutoFillWorkbookResults()
    {
        m_oEdgeColorResults = new AutoFillColorColumnResults();
        m_oEdgeWidthResults = new AutoFillNumericRangeColumnResults();
        m_oEdgeAlphaResults = new AutoFillNumericRangeColumnResults();

        m_oVertexColorResults = new AutoFillColorColumnResults();
        m_oVertexRadiusResults = new AutoFillNumericRangeColumnResults();
        m_oVertexAlphaResults = new AutoFillNumericRangeColumnResults();
        m_oVertexXResults = new AutoFillNumericRangeColumnResults();
        m_oVertexYResults = new AutoFillNumericRangeColumnResults();

        AssertValid();
    }

    //*************************************************************************
    //  Property: AutoFilledNonXYColumnCount
    //
    /// <summary>
    /// Gets the number of columns that were autofilled, not including the
    /// vertex X and Y columns.
    /// </summary>
    ///
    /// <remarks>
    /// The number of columns that were autofilled, not including the vertex X
    /// and Y columns.
    /// </remarks>
    //*************************************************************************

    public Int32
    AutoFilledNonXYColumnCount
    {
        get
        {
            AssertValid();

            return (this.AutoFilledEdgeColumnCount +
                this.AutoFilledVertexNonXYColumnCount);
        }
    }

    //*************************************************************************
    //  Property: AutoFilledEdgeColumnCount
    //
    /// <summary>
    /// Gets the number of edge columns that were autofilled.
    /// </summary>
    ///
    /// <returns>
    /// The number of edge columns that were autofilled.
    /// </returns>
    //*************************************************************************

    public Int32
    AutoFilledEdgeColumnCount
    {
        get
        {
            AssertValid();

            return (
                m_oEdgeColorResults.AutoFilledColumnCount +
                m_oEdgeWidthResults.AutoFilledColumnCount +
                m_oEdgeAlphaResults.AutoFilledColumnCount
                );
        }
    }

    //*************************************************************************
    //  Property: AutoFilledVertexNonXYColumnCount
    //
    /// <summary>
    /// Gets the number of vertex columns that were autofilled, not including
    /// the X and Y columns.
    /// </summary>
    ///
    /// <returns>
    /// The number of vertex columns that were autofilled, not including the
    /// X and Y columns.
    /// </returns>
    //*************************************************************************

    public Int32
    AutoFilledVertexNonXYColumnCount
    {
        get
        {
            AssertValid();

            return (
                m_oVertexColorResults.AutoFilledColumnCount +
                m_oVertexRadiusResults.AutoFilledColumnCount +
                m_oVertexAlphaResults.AutoFilledColumnCount
                );
        }
    }

    //*************************************************************************
    //  Property: EdgeColorResults
    //
    /// <summary>
    /// Gets or sets the edge color column autofill results.
    /// </summary>
    ///
    /// <returns>
    /// The edge color column autofill results, as an <see
    /// cref="AutoFillColorColumnResults" />.  Can't be null.
    /// </returns>
    //*************************************************************************

    public AutoFillColorColumnResults
    EdgeColorResults
    {
        get
        {
            AssertValid();

            return (m_oEdgeColorResults);
        }

        set
        {
            m_oEdgeColorResults = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeWidthResults
    //
    /// <summary>
    /// Gets or sets the edge width column autofill results.
    /// </summary>
    ///
    /// <returns>
    /// The edge width column autofill results, as an <see
    /// cref="AutoFillNumericRangeColumnResults" />.  Can't be null.
    /// </returns>
    //*************************************************************************

    public AutoFillNumericRangeColumnResults
    EdgeWidthResults
    {
        get
        {
            AssertValid();

            return (m_oEdgeWidthResults);
        }

        set
        {
            m_oEdgeWidthResults = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeAlphaResults
    //
    /// <summary>
    /// Gets or sets the edge alpha column autofill results.
    /// </summary>
    ///
    /// <returns>
    /// The edge alpha column autofill results, as an <see
    /// cref="AutoFillNumericRangeColumnResults" />.  Can't be null.
    /// </returns>
    //*************************************************************************

    public AutoFillNumericRangeColumnResults
    EdgeAlphaResults
    {
        get
        {
            AssertValid();

            return (m_oEdgeAlphaResults);
        }

        set
        {
            m_oEdgeAlphaResults = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexColorResults
    //
    /// <summary>
    /// Gets or sets the vertex color column autofill results.
    /// </summary>
    ///
    /// <returns>
    /// The vertex color column autofill results, as an <see
    /// cref="AutoFillColorColumnResults" />.  Can't be null.
    /// </returns>
    //*************************************************************************

    public AutoFillColorColumnResults
    VertexColorResults
    {
        get
        {
            AssertValid();

            return (m_oVertexColorResults);
        }

        set
        {
            m_oVertexColorResults = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexRadiusResults
    //
    /// <summary>
    /// Gets or sets the vertex radius column autofill results.
    /// </summary>
    ///
    /// <returns>
    /// The vertex radius column autofill results, as an <see
    /// cref="AutoFillNumericRangeColumnResults" />.  Can't be null.
    /// </returns>
    //*************************************************************************

    public AutoFillNumericRangeColumnResults
    VertexRadiusResults
    {
        get
        {
            AssertValid();

            return (m_oVertexRadiusResults);
        }

        set
        {
            m_oVertexRadiusResults = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexAlphaResults
    //
    /// <summary>
    /// Gets or sets the vertex alpha column autofill results.
    /// </summary>
    ///
    /// <returns>
    /// The vertex alpha column autofill results, as an <see
    /// cref="AutoFillNumericRangeColumnResults" />.  Can't be null.
    /// </returns>
    //*************************************************************************

    public AutoFillNumericRangeColumnResults
    VertexAlphaResults
    {
        get
        {
            AssertValid();

            return (m_oVertexAlphaResults);
        }

        set
        {
            m_oVertexAlphaResults = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexXResults
    //
    /// <summary>
    /// Gets or sets the vertex X column autofill results.
    /// </summary>
    ///
    /// <returns>
    /// The vertex X column autofill results, as an <see
    /// cref="AutoFillNumericRangeColumnResults" />.  Can't be null.
    /// </returns>
    //*************************************************************************

    public AutoFillNumericRangeColumnResults
    VertexXResults
    {
        get
        {
            AssertValid();

            return (m_oVertexXResults);
        }

        set
        {
            m_oVertexXResults = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexYResults
    //
    /// <summary>
    /// Gets or sets the vertex Y column autofill results.
    /// </summary>
    ///
    /// <returns>
    /// The vertex Y column autofill results, as an <see
    /// cref="AutoFillNumericRangeColumnResults" />.  Can't be null.
    /// </returns>
    //*************************************************************************

    public AutoFillNumericRangeColumnResults
    VertexYResults
    {
        get
        {
            AssertValid();

            return (m_oVertexYResults);
        }

        set
        {
            m_oVertexYResults = value;

            AssertValid();
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

    public String
    ConvertToString()
    {
        AssertValid();

        // Use string concatenation.

        return ( String.Join(AutoFillWorkbookResults.FieldSeparatorString,

            new String [] {

            m_oEdgeColorResults.ConvertToString(),
            m_oEdgeWidthResults.ConvertToString(),
            m_oEdgeAlphaResults.ConvertToString(),

            m_oVertexColorResults.ConvertToString(),
            m_oVertexRadiusResults.ConvertToString(),
            m_oVertexAlphaResults.ConvertToString(),
            m_oVertexXResults.ConvertToString(),
            m_oVertexYResults.ConvertToString()
            } ) );
    }

    //*************************************************************************
    //  Method: FromString()
    //
    /// <summary>
    /// Creates a <see cref="AutoFillWorkbookResults" /> object from a
    /// persisted string.
    /// </summary>
    ///
    /// <param name="theString">
    /// String created by <see cref="ConvertToString()" />.
    /// </param>
    ///
    /// <returns>
    /// A <see cref="AutoFillWorkbookResults" /> object created from <paramref
    /// name="theString" />.
    /// </returns>
    //*************************************************************************

    public static AutoFillWorkbookResults
    FromString
    (
        String theString
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(theString) );

        AutoFillWorkbookResults oAutoFillWorkbookResults =
            new AutoFillWorkbookResults();

        oAutoFillWorkbookResults.ConvertFromString(theString);

        return (oAutoFillWorkbookResults);
    }

    //*************************************************************************
    //  Method: ConvertFromString()
    //
    /// <summary>
    /// Sets the results from a string.
    /// </summary>
    ///
    /// <param name="theString">
    /// String created by <see cref="ConvertToString()" />.
    /// </param>
    //*************************************************************************

    public void
    ConvertFromString
    (
        String theString
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(theString) );

        ColorConverter oColorConverter = new ColorConverter();

        String [] asFields = theString.Split(
            AutoFillWorkbookResults.FieldSeparator);

        if (asFields.Length >= 30)
        {
            m_oEdgeColorResults.ConvertFromString(asFields, 0);
            m_oEdgeWidthResults.ConvertFromString(asFields, 5);
            m_oEdgeAlphaResults.ConvertFromString(asFields, 10);

            m_oVertexColorResults.ConvertFromString(asFields, 15);
            m_oVertexRadiusResults.ConvertFromString(asFields, 20);
            m_oVertexAlphaResults.ConvertFromString(asFields, 25);

            // An earlier version of this class did not keep track of whether
            // the X and Y columns were autofilled, so earlier workbooks have
            // only 30 fields.

            if (asFields.Length >= 40)
            {
                m_oVertexXResults.ConvertFromString(asFields, 30);
                m_oVertexYResults.ConvertFromString(asFields, 35);
            }
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
        Debug.Assert(m_oEdgeColorResults != null);
        Debug.Assert(m_oEdgeWidthResults != null);
        Debug.Assert(m_oEdgeAlphaResults != null);

        Debug.Assert(m_oVertexColorResults != null);
        Debug.Assert(m_oVertexRadiusResults != null);
        Debug.Assert(m_oVertexAlphaResults != null);
        Debug.Assert(m_oVertexXResults != null);
        Debug.Assert(m_oVertexYResults != null);
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// Field separator used by ConvertToString() and ConvertFromString().
    /// This is the Unicode "dark shade" character, which is unlikely to be
    /// used in column names.  This is defined in both character array and
    /// string versions to accommodate String.Split() and String.Join().

    public static readonly Char [] FieldSeparator = new Char[] {'\u2593'};
    ///
    public const String FieldSeparatorString = "\u2593";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Edge color results.

    protected AutoFillColorColumnResults m_oEdgeColorResults;

    /// Edge width results.

    protected AutoFillNumericRangeColumnResults m_oEdgeWidthResults;

    /// Edge alpha results.

    protected AutoFillNumericRangeColumnResults m_oEdgeAlphaResults;

    /// Vertex color results.

    protected AutoFillColorColumnResults m_oVertexColorResults;

    /// Vertex radius results.

    protected AutoFillNumericRangeColumnResults m_oVertexRadiusResults;

    /// Vertex alpha results.

    protected AutoFillNumericRangeColumnResults m_oVertexAlphaResults;

    /// Vertex X/Y results.

    protected AutoFillNumericRangeColumnResults m_oVertexXResults;
    ///
    protected AutoFillNumericRangeColumnResults m_oVertexYResults;
}

}
