using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphMetricColumnOrdered
//
/// <summary>
/// Stores an entire column of graph metric values, where the values are mapped
/// to the next worksheet row.
/// </summary>
///
/// <remarks>
/// The graph metric value returned by <see cref="GraphMetricValuesOrdered" />
/// should be written to the worksheet rows in sequential row order -- the
/// first value in the first row, the second value in the second row, and so
/// on.
/// </remarks>
//*****************************************************************************

public class GraphMetricColumnOrdered : GraphMetricColumn
{
    //*************************************************************************
    //  Constructor: GraphMetricColumnOrdered()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GraphMetricColumnOrdered" /> class.
    /// </summary>
    ///
    /// <param name="worksheetName">
    /// Name of the worksheet containing the column.
    /// </param>
    ///
    /// <param name="tableName">
    /// Name of the table (ListObject) containing the column.
    /// </param>
    ///
    /// <param name="columnName">
    /// Name of the column.
    /// </param>
    ///
    /// <param name="columnWidthChars">
    /// Width of the column, in characters, or <see
    /// cref="Microsoft.Research.CommunityTechnologies.AppLib.ExcelUtil.
    /// AutoColumnWidth" /> to set the width automatically.
    /// </param>
    ///
    /// <param name="numberFormat">
    /// Number format of the column, or null if the column is not numeric.
    /// Sample: "0.00".
    /// </param>
    ///
    /// <param name="style">
    /// Style of the column, or null to not apply a style.  Sample: "Bad".
    /// </param>
    ///
    /// <param name="graphMetricValuesOrdered">
    /// Array of graph metric values, one value per calculated column cell.
    /// </param>
    //*************************************************************************

    public GraphMetricColumnOrdered
    (
        String worksheetName,
        String tableName,
        String columnName,
        Double columnWidthChars,
        String numberFormat,
        String style,
        GraphMetricValueOrdered [] graphMetricValuesOrdered
    )
    : base(worksheetName, tableName, columnName, columnWidthChars,
        numberFormat, style)
    {
        m_aoGraphMetricValuesOrdered = graphMetricValuesOrdered;

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: GraphMetricColumnOrdered()
    //
    /// <summary>
    /// Do not use this constructor.
    /// </summary>
    //*************************************************************************

    protected GraphMetricColumnOrdered()
    {
        // (Do nothing.)
    }

    //*************************************************************************
    //  Property: GraphMetricValuesOrdered
    //
    /// <summary>
    /// Gets or sets the array of graph metric values.
    /// </summary>
    ///
    /// <value>
    /// The array of graph metric values, one value per calculated column cell.
    /// </value>
    //*************************************************************************

    public GraphMetricValueOrdered []
    GraphMetricValuesOrdered
    {
        get
        {
            AssertValid();

            return (m_aoGraphMetricValuesOrdered);
        }

        set
        {
            m_aoGraphMetricValuesOrdered = value;

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

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
        base.AssertValid();

        Debug.Assert(m_aoGraphMetricValuesOrdered != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Array of graph metric values, one value per calculated column cell.

    protected GraphMetricValueOrdered [] m_aoGraphMetricValuesOrdered;
}

}
