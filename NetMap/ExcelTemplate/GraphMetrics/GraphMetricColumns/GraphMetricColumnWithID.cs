using System;
using System.Diagnostics;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphMetricColumnWithID
//
/// <summary>
/// Stores an entire column of graph metric values, where the values are mapped
/// to worksheet rows by row IDs.
/// </summary>
///
/// <remarks>
/// The graph metric value returned by <see cref="GraphMetricValuesWithID" />
/// should be written to the worksheet rows identified by the <see
/// cref="GraphMetricValueWithID.RowID" /> property.  (Row IDs get written to
/// the worksheet when the worksheet is read by <see cref="WorkbookReader" />.)
/// </remarks>
//*****************************************************************************

public class GraphMetricColumnWithID : GraphMetricColumn
{
    //*************************************************************************
    //  Constructor: GraphMetricColumnWithID()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphMetricColumnWithID" />
	/// class.
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
	/// Width of the column, in characters.
	/// </param>
	///
	/// <param name="numberFormat">
	/// Number format of the column, or null if the column is not numeric.
	/// Sample: "0.00".
	/// </param>
	///
	/// <param name="graphMetricValuesWithID">
	/// Array of graph metric values, one value per calculated column cell.
	/// </param>
    //*************************************************************************

    public GraphMetricColumnWithID
	(
		String worksheetName,
		String tableName,
		String columnName,
		Single columnWidthChars,
		String numberFormat,
		GraphMetricValueWithID [] graphMetricValuesWithID
	)
	: base(worksheetName, tableName, columnName, columnWidthChars,
		numberFormat)
    {
		m_aoGraphMetricValuesWithID = graphMetricValuesWithID;

		AssertValid();
    }

    //*************************************************************************
    //  Constructor: GraphMetricColumnWithID()
    //
    /// <summary>
    /// Do not use this constructor.
    /// </summary>
    //*************************************************************************

    protected GraphMetricColumnWithID()
    {
		// (Do nothing.)
    }

    //*************************************************************************
    //  Property: GraphMetricValuesWithID
    //
    /// <summary>
    /// Gets or sets the array of graph metric values.
    /// </summary>
    ///
    /// <value>
    /// The array of graph metric values, one value per calculated column cell.
    /// </value>
    //*************************************************************************

    public GraphMetricValueWithID []
    GraphMetricValuesWithID
    {
        get
        {
            AssertValid();

            return (m_aoGraphMetricValuesWithID);
        }

        set
        {
			m_aoGraphMetricValuesWithID = value;

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

		Debug.Assert(m_aoGraphMetricValuesWithID != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Array of graph metric values, one value per calculated column cell.

	protected GraphMetricValueWithID [] m_aoGraphMetricValuesWithID;
}

}
