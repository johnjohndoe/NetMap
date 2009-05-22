using System;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphMetricColumn
//
/// <summary>
/// Base class for a family of classes that store an entire column of graph
/// metric values.
/// </summary>
///
/// <remarks>
/// The derived classes differ in how the graph metric values are mapped to
/// worksheet rows.
/// </remarks>
//*****************************************************************************

public abstract class GraphMetricColumn : Object
{
    //*************************************************************************
    //  Constructor: GraphMetricColumn()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphMetricColumn" />
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
    /// <param name="style">
    /// Style of the column, or null to apply Excel's normal style.  Sample:
    /// "Bad".
    /// </param>
    //*************************************************************************

    public GraphMetricColumn
    (
        String worksheetName,
        String tableName,
        String columnName,
        Double columnWidthChars,
        String numberFormat,
        String style
    )
    {
        m_sWorksheetName = worksheetName;
        m_sTableName = tableName;
        m_sColumnName = columnName;
        m_fColumnWidthChars = columnWidthChars;
        m_sNumberFormat = numberFormat;
        m_sStyle = style;

        // AssertValid();
    }

    //*************************************************************************
    //  Constructor: GraphMetricColumn()
    //
    /// <summary>
    /// Do not use this constructor.
    /// </summary>
    //*************************************************************************

    protected GraphMetricColumn()
    {
        // (Do nothing.)
    }

    //*************************************************************************
    //  Property: WorksheetName
    //
    /// <summary>
    /// Gets or sets the name of the worksheet containing the column.
    /// </summary>
    ///
    /// <value>
    /// The name of the worksheet containing the column.
    /// </value>
    //*************************************************************************

    public String
    WorksheetName
    {
        get
        {
            AssertValid();

            return (m_sWorksheetName);
        }

        set
        {
            m_sWorksheetName = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: TableName
    //
    /// <summary>
    /// Gets or sets the name of the table (ListObject) containing the column.
    /// </summary>
    ///
    /// <value>
    /// The name of the table containing the column.
    /// </value>
    //*************************************************************************

    public String
    TableName
    {
        get
        {
            AssertValid();

            return (m_sTableName);
        }

        set
        {
            m_sTableName = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ColumnName
    //
    /// <summary>
    /// Gets or sets the name of the column.
    /// </summary>
    ///
    /// <value>
    /// The name of the column.
    /// </value>
    //*************************************************************************

    public String
    ColumnName
    {
        get
        {
            AssertValid();

            return (m_sColumnName);
        }

        set
        {
            m_sColumnName = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ColumnWidthChars
    //
    /// <summary>
    /// Gets or sets the width of the column, in characters.
    /// </summary>
    ///
    /// <value>
    /// The width of the column, in characters, or <see
    /// cref="ExcelUtil.AutoColumnWidth" /> to automatically size the column.
    /// </value>
    //*************************************************************************

    public Double
    ColumnWidthChars
    {
        get
        {
            AssertValid();

            return (m_fColumnWidthChars);
        }

        set
        {
            m_fColumnWidthChars = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: NumberFormat
    //
    /// <summary>
    /// Gets or sets the number format of the column.
    /// </summary>
    ///
    /// <value>
    /// The number format of the column, or null if the column is not numeric.
    /// </value>
    //*************************************************************************

    public String
    NumberFormat
    {
        get
        {
            AssertValid();

            return (m_sNumberFormat);
        }

        set
        {
            m_sNumberFormat = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Style
    //
    /// <summary>
    /// Gets or sets the style of the column.
    /// </summary>
    ///
    /// <value>
    /// The style of the column, or null to apply Excel's normal style.
    /// Sample: "Bad".
    /// </value>
    //*************************************************************************

    public String
    Style
    {
        get
        {
            AssertValid();

            return (m_sStyle);
        }

        set
        {
            m_sStyle = value;

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

    [Conditional("DEBUG")]

    public virtual void
    AssertValid()
    {
        Debug.Assert( !String.IsNullOrEmpty(m_sWorksheetName) );
        Debug.Assert( !String.IsNullOrEmpty(m_sTableName) );
        Debug.Assert( !String.IsNullOrEmpty(m_sColumnName) );

        Debug.Assert(m_fColumnWidthChars == ExcelUtil.AutoColumnWidth ||
            m_fColumnWidthChars > 0);

        // m_sNumberFormat
        // m_sStyle
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Name of the worksheet containing the column.

    protected String m_sWorksheetName;

    /// Name of the table (ListObject) containing the column.

    protected String m_sTableName;

    /// Name of the column.

    protected String m_sColumnName;

    /// Width of the column, in characters.

    protected Double m_fColumnWidthChars;

    /// Number format of the column, or null if the column is not numeric.

    protected String m_sNumberFormat;

    /// Style of the column, or null to not apply a style.  Sample: "Bad".

    protected String m_sStyle;
}

}
