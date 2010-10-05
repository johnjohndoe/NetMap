
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Visualization.Wpf;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: WorksheetReaderBase
//
/// <summary>
/// Base class for classes that know how to read an Excel worksheet containing
/// graph data.
/// </summary>
///
/// <remarks>
/// There is one derived class for each worksheet in a NodeXL workbook that
/// contains graph data.  There are additional derived classes for reading
/// tables contained in the hidden Miscellaneous worksheet.
/// </remarks>
//*****************************************************************************

public class WorksheetReaderBase : NodeXLBase
{
    //*************************************************************************
    //  Constructor: WorksheetReaderBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="WorksheetReaderBase" />
    /// class.
    /// </summary>
    //*************************************************************************

    public WorksheetReaderBase()
    {
        m_oAlphaConverter = new AlphaConverter();

        // AssertValid();
    }

    //*************************************************************************
    //  Method: GetTableColumnIndex()
    //
    /// <summary>
    /// Gets the one-based index of a column within a table.
    /// </summary>
    ///
    /// <param name="oTable">
    /// Table that contains the specified column.
    /// </param>
    ///
    /// <param name="sColumnName">
    /// Name of the column to get the index for.
    /// </param>
    ///
    /// <param name="bColumnIsRequired">
    /// true if the column is required.
    /// </param>
    ///
    /// <returns>
    /// The one-based index of the column named <paramref
    /// name="sColumnName" />, or <see cref="NoSuchColumn" /> if there is no
    /// such column and <paramref name="bColumnIsRequired" /> is false.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="bColumnIsRequired" /> is true and there is no such
    /// column, a <see cref="WorkbookFormatException" /> is thrown.
    /// </remarks>
    //*************************************************************************

    protected Int32
    GetTableColumnIndex
    (
        ListObject oTable,
        String sColumnName,
        Boolean bColumnIsRequired
    )
    {
        Debug.Assert(oTable != null);
        Debug.Assert( !String.IsNullOrEmpty(sColumnName) );
        AssertValid();

        ListColumn oColumn;

        if ( ExcelUtil.TryGetTableColumn(oTable, sColumnName, out oColumn) )
        {
            return (oColumn.Index);
        }

        if (bColumnIsRequired)
        {
            OnWorkbookFormatError( String.Format(

                "The table named \"{0}\" must have a column named \"{1}.\""
                + "\r\n\r\n{2}"
                ,
                oTable.Name,
                sColumnName,
                ErrorUtil.GetTemplateMessage()
                ) );
        }

        return (NoSuchColumn);
    }

    //*************************************************************************
    //  Method: FillIDColumn()
    //
    /// <summary>
    /// Fills the ID column in a a table with a sequence of unique IDs.
    /// </summary>
    ///
    /// <param name="oTable">
    /// Table containing the ID column to fill.
    /// </param>
    ///
    /// <remarks>
    /// If the table has no ID column, which is a column named
    /// CommonTableColumnNames.ID, this method adds it.
    ///
    /// <para>
    /// Filtered rows are not filled.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected void
    FillIDColumn
    (
        ListObject oTable
    )
    {
        Debug.Assert(oTable != null);
        AssertValid();

        // Read the range that contains visible data.  If the table is
        // filtered, the range may contain multiple areas.

        Range oVisibleRange;
        ListColumn oIDColumn;

        if (
            !ExcelUtil.TryGetVisibleTableRange(oTable, out oVisibleRange)
            ||
            ExcelUtil.VisibleTableRangeIsEmpty(oTable)
            ||
            !ExcelUtil.TryGetOrAddTableColumn(oTable,
                CommonTableColumnNames.ID, ExcelUtil.AutoColumnWidth, null,
                out oIDColumn)
            )
        {
            return;
        }

        Int32 iIDColumnIndex = oIDColumn.Index;

        Range oDataBodyRange = oTable.DataBodyRange;

        Debug.Assert(oDataBodyRange != null);
        Debug.Assert(oTable.Parent is Worksheet);

        Worksheet oWorksheet = (Worksheet)oTable.Parent;

        foreach (Range oArea in oVisibleRange.Areas)
        {
            // Get the rows within the ID column that should be filled in.

            Int32 iAreaStartRowOneBased = oArea.Row;
            Int32 iRowsInArea = oArea.Rows.Count;
            Int32 iTableStartRowOneBased = oTable.Range.Row;

            Range oIDRange = oWorksheet.get_Range(

                oDataBodyRange.Cells[
                    iAreaStartRowOneBased - iTableStartRowOneBased,
                    iIDColumnIndex
                    ],

                oDataBodyRange.Cells[
                    iAreaStartRowOneBased - iTableStartRowOneBased
                        + iRowsInArea - 1,
                    iIDColumnIndex
                    ]
                );

            // Use the Excel row numbers as the unique IDs.  Create a
            // one-column array, then fill it in with the row numbers.

            Int32 iRows = oIDRange.Rows.Count;

            Object [,] aoValues = ExcelUtil.GetSingleColumn2DArray(iRows);

            for (Int32 i = 1; i <= iRows; i++)
            {
                aoValues[i, 1] = iAreaStartRowOneBased + i - 1;
            }

            oIDRange.Value2 = aoValues;

            #if false

            // Note: Don't use the following clever code to fill in the row
            // numbers.  On large worksheets, the calculations take forever.

            oIDRange.Value2 = "=ROW()";
            oIDRange.Value2 = oIDRange.Value2;

            #endif
        }
    }

    //*************************************************************************
    //  Method: ReadAlpha()
    //
    /// <summary>
    /// If an alpha has been specified for an edge or vertex, sets the alpha
    /// value on the edge or vertex.
    /// </summary>
    ///
    /// <param name="oRow">
    /// Row containing the edge or vertex data.
    /// </param>
    ///
    /// <param name="oEdgeOrVertex">
    /// Edge or vertex to set the alpha on.
    /// </param>
    ///
    /// <returns>
    /// true if the edge or vertex was hidden.
    /// </returns>
    //*************************************************************************

    protected Boolean
    ReadAlpha
    (
        ExcelTableReader.ExcelTableRow oRow,
        IMetadataProvider oEdgeOrVertex
    )
    {
        Debug.Assert(oRow != null);
        Debug.Assert(oEdgeOrVertex != null);

        AssertValid();

        String sString;

        if ( !oRow.TryGetNonEmptyStringFromCell(CommonTableColumnNames.Alpha,
            out sString) )
        {
            return (false);
        }

        Single fAlpha;

        if ( !Single.TryParse(sString, out fAlpha) )
        {
            Range oInvalidCell = oRow.GetRangeForCell(
                CommonTableColumnNames.Alpha);

            OnWorkbookFormatError( String.Format(

                "The cell {0} contains an invalid opacity.  The opacity,"
                + " which is optional, must be a number.  Any number is"
                + " acceptable, although {1} (transparent) is used for any"
                + " number less than {1} and {2} (opaque) is used for any"
                + " number greater than {2}."
                ,
                ExcelUtil.GetRangeAddress(oInvalidCell),
                AlphaConverter.MinimumAlphaWorkbook,
                AlphaConverter.MaximumAlphaWorkbook
                ),

                oInvalidCell
            );
        }

        fAlpha = m_oAlphaConverter.WorkbookToGraph(fAlpha);

        oEdgeOrVertex.SetValue(ReservedMetadataKeys.PerAlpha, fAlpha);

        return (fAlpha == 0);
    }

    //*************************************************************************
    //  Method: ReadColor()
    //
    /// <summary>
    /// If a color has been specified for an edge or vertex, sets the edge's
    /// or vertex's color.
    /// </summary>
    ///
    /// <param name="oRow">
    /// Row to check.
    /// </param>
    ///
    /// <param name="sColumnName">
    /// Name of the column to check.
    /// </param>
    ///
    /// <param name="oEdgeOrVertex">
    /// Edge or vertex to set the color on.
    /// </param>
    ///
    /// <param name="sColorKey">
    /// Name of the metadata key that stores the color on the edge or vertex.
    /// </param>
    ///
    /// <param name="oColorConverter2">
    /// Object for converting the color from a string to a Color.
    /// </param>
    //*************************************************************************

    protected void
    ReadColor
    (
        ExcelTableReader.ExcelTableRow oRow,
        String sColumnName,
        IMetadataProvider oEdgeOrVertex,
        String sColorKey,
        ColorConverter2 oColorConverter2
    )
    {
        Debug.Assert(oRow != null);
        Debug.Assert( !String.IsNullOrEmpty(sColumnName) );
        Debug.Assert(oEdgeOrVertex != null);
        Debug.Assert( !String.IsNullOrEmpty(sColorKey) );
        Debug.Assert(oColorConverter2 != null);
        AssertValid();

        Color oColor;

        if ( TryGetColor(oRow, sColumnName, oColorConverter2, out oColor) )
        {
            oEdgeOrVertex.SetValue(sColorKey, oColor);
        }
    }

    //*************************************************************************
    //  Method: ReadCellAndSetMetadata()
    //
    /// <summary>
    /// If a cell is not empty, sets a metadata value on an edge or vertex to
    /// the cell contents.
    /// </summary>
    ///
    /// <param name="oRow">
    /// Row to check.
    /// </param>
    ///
    /// <param name="sColumnName">
    /// Name of the column to check.
    /// </param>
    ///
    /// <param name="oEdgeOrVertex">
    /// Edge or vertex to set the metadata value on.
    /// </param>
    ///
    /// <param name="sKeyName">
    /// Name of the metadata key to set.
    /// </param>
    ///
    /// <returns>
    /// true if the metadata value was set on the edge or vertex.
    /// </returns>
    //*************************************************************************

    protected Boolean
    ReadCellAndSetMetadata
    (
        ExcelTableReader.ExcelTableRow oRow,
        String sColumnName,
        IMetadataProvider oEdgeOrVertex,
        String sKeyName
    )
    {
        Debug.Assert(oRow != null);
        Debug.Assert( !String.IsNullOrEmpty(sColumnName) );
        Debug.Assert(oEdgeOrVertex != null);
        Debug.Assert( !String.IsNullOrEmpty(sKeyName) );
        AssertValid();

        String sNonEmptyString;

        if ( !oRow.TryGetNonEmptyStringFromCell(sColumnName,
            out sNonEmptyString) )
        {
            return (false);
        }

        oEdgeOrVertex.SetValue(sKeyName, sNonEmptyString);

        return (true);
    }

    //*************************************************************************
    //  Method: TryGetBoolean()
    //
    /// <summary>
    /// Attempts to get a Boolean value from a worksheet cell.
    /// </summary>
    ///
    /// <param name="oRow">
    /// Row containing the data.
    /// </param>
    ///
    /// <param name="sColumnName">
    /// Name of the column containing the Boolean value.
    /// </param>
    ///
    /// <param name="oBooleanConverter">
    /// Object for converting the Boolean value from a string to a Boolean.
    /// </param>
    ///
    /// <param name="bBoolean">
    /// Where the Boolean value gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the specified cell contains a valid Boolean value.
    /// </returns>
    ///
    /// <remarks>
    /// If the specified cell is empty, false is returned.  If the cell
    /// contains a valid Boolean value, the value gets stored at <paramref
    /// name="bBoolean" /> and true is returned.  If the cell contains an
    /// invalid Boolean, a <see cref="WorkbookFormatException" /> is thrown.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    TryGetBoolean
    (
        ExcelTableReader.ExcelTableRow oRow,
        String sColumnName,
        BooleanConverter oBooleanConverter,
        out Boolean bBoolean
    )
    {
        Debug.Assert(oRow != null);
        Debug.Assert( !String.IsNullOrEmpty(sColumnName) );
        Debug.Assert(oBooleanConverter != null);
        AssertValid();

        bBoolean = false;
        String sBoolean;

        if ( !oRow.TryGetNonEmptyStringFromCell(sColumnName, out sBoolean) )
        {
            return (false);
        }

        if ( !oBooleanConverter.TryWorkbookToGraph(sBoolean, out bBoolean) )
        {
            OnWorkbookFormatErrorWithDropDown(oRow, sColumnName, "value");
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryGetColor()
    //
    /// <summary>
    /// Attempts to get a color from a worksheet cell.
    /// </summary>
    ///
    /// <param name="oRow">
    /// Row to check.
    /// </param>
    ///
    /// <param name="sColumnName">
    /// Name of the column to check.
    /// </param>
    ///
    /// <param name="oColorConverter2">
    /// Object for converting the color from a string to a Color.
    /// </param>
    ///
    /// <param name="oColor">
    /// Where the color gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the specified cell contains a valid color.
    /// </returns>
    ///
    /// <remarks>
    /// If the specified cell is empty, false is returned.  If the cell
    /// contains a valid color, the color gets stored at <paramref
    /// name="oColor" /> and true is returned.  If the cell contains an invalid
    /// color, a <see cref="WorkbookFormatException" /> is thrown.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    TryGetColor
    (
        ExcelTableReader.ExcelTableRow oRow,
        String sColumnName,
        ColorConverter2 oColorConverter2,
        out Color oColor
    )
    {
        Debug.Assert(oRow != null);
        Debug.Assert( !String.IsNullOrEmpty(sColumnName) );
        Debug.Assert(oColorConverter2 != null);
        AssertValid();

        oColor = Color.Empty;

        String sColor;

        if ( !oRow.TryGetNonEmptyStringFromCell(sColumnName, out sColor) )
        {
            return (false);
        }

        if ( !oColorConverter2.TryWorkbookToGraph(sColor, out oColor) )
        {
            Range oInvalidCell = oRow.GetRangeForCell(sColumnName);

            OnWorkbookFormatError( String.Format(

                "The cell {0} contains an unrecognized color.  Right-click the"
                + " cell and select Select Color on the right-click menu."
                ,
                ExcelUtil.GetRangeAddress(oInvalidCell)
                ),

                oInvalidCell
            );
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryGetVertexShape()
    //
    /// <summary>
    /// Attempts to get a vertex shape from a worksheet cell.
    /// </summary>
    ///
    /// <param name="oRow">
    /// Row containing the vertex data.
    /// </param>
    ///
    /// <param name="sColumnName">
    /// Name of the column containing the vertex shape.
    /// </param>
    ///
    /// <param name="eShape">
    /// Where the vertex shape gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the specified cell contains a valid vertex shape.
    /// </returns>
    ///
    /// <remarks>
    /// If the specified shape cell is empty, false is returned.  If the cell
    /// contains a valid vertex shape, the shape gets stored at <paramref
    /// name="eShape" /> and true is returned.  If the cell contains an invalid
    /// shape, a <see cref="WorkbookFormatException" /> is thrown.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    TryGetVertexShape
    (
        ExcelTableReader.ExcelTableRow oRow,
        String sColumnName,
        out VertexShape eShape
    )
    {
        Debug.Assert(oRow != null);
        Debug.Assert( !String.IsNullOrEmpty(sColumnName) );
        AssertValid();

        eShape = VertexShape.Circle;
        String sShape;

        if ( !oRow.TryGetNonEmptyStringFromCell(sColumnName, out sShape) )
        {
            return (false);
        }

        VertexShapeConverter oVertexShapeConverter =
            new VertexShapeConverter();

        if ( !oVertexShapeConverter.TryWorkbookToGraph(sShape, out eShape) )
        {
            OnWorkbookFormatErrorWithDropDown(oRow, sColumnName, "shape");
        }

        return (true);
    }

    //*************************************************************************
    //  Method: AddToRowIDDictionary()
    //
    /// <summary>
    /// Adds an edge or vertex to a dictionary and sets the edge or vertex's
    /// Tag to the row ID.
    /// </summary>
    ///
    /// <param name="oRow">
    /// Row containing the edge or vertex data.
    /// </param>
    ///
    /// <param name="oEdgeOrVertex">
    /// Edge or vertex to get the row ID for.
    /// </param>
    ///
    /// <param name="oRowIDDictionary">
    /// Keeps track of the edges or vertices that have been added to the graph,
    /// depending on which derived class this method is being called from.  The
    /// key is the edge or vertex row ID read from the edge or vertex worksheet
    /// and the value is the edge or vertex.
    /// </param>
    //*************************************************************************

    protected void
    AddToRowIDDictionary
    (
        ExcelTableReader.ExcelTableRow oRow,
        IIdentityProvider oEdgeOrVertex,
        Dictionary<Int32, IIdentityProvider> oRowIDDictionary
    )
    {
        Debug.Assert(oRow != null);
        Debug.Assert(oEdgeOrVertex != null);
        Debug.Assert(oRowIDDictionary != null);
        AssertValid();

        // Because the derived class fills in its ID column if the column
        // exists, each cell in the column should be valid.

        String sRowID;
        Int32 iRowID = Int32.MinValue;

        if (
            oRow.TryGetNonEmptyStringFromCell(CommonTableColumnNames.ID,
                out sRowID)
            &&
            Int32.TryParse(sRowID, out iRowID)
            )
        {
            oRowIDDictionary.Add(iRowID, oEdgeOrVertex);

            Debug.Assert(oEdgeOrVertex is IMetadataProvider);

            // Store the row ID in the edge or vertex tag.

            ( (IMetadataProvider)oEdgeOrVertex ).Tag = iRowID;
        }
    }

    //*************************************************************************
    //  Method: ReadAllColumns()
    //
    /// <summary>
    /// Reads all columns in a table row and stores the cell values as metadata
    /// on an edge or vertex.
    /// </summary>
    ///
    /// <param name="oExcelTableReader">
    /// Object that is reading the edge or vertex table.
    /// </param>
    ///
    /// <param name="oRow">
    /// Row containing the edge or vertex data.
    /// </param>
    ///
    /// <param name="oEdgeOrVertex">
    /// Edge or vertex to set the metadata on.
    /// </param>
    ///
    /// <param name="oColumnNamesToExclude">
    /// HashSet of zero or more columns to exclude.
    /// </param>
    //*************************************************************************

    protected void
    ReadAllColumns
    (
        ExcelTableReader oExcelTableReader,
        ExcelTableReader.ExcelTableRow oRow,
        IMetadataProvider oEdgeOrVertex,
        HashSet<String> oColumnNamesToExclude
    )
    {
        Debug.Assert(oExcelTableReader != null);
        Debug.Assert(oRow != null);
        Debug.Assert(oEdgeOrVertex != null);
        Debug.Assert(oColumnNamesToExclude != null);
        AssertValid();

        foreach (String sColumnName in oExcelTableReader.ColumnNames)
        {
            String sValue;

            if ( !oColumnNamesToExclude.Contains(sColumnName) &&
                oRow.TryGetNonEmptyStringFromCell(sColumnName, out sValue) )
            {
                oEdgeOrVertex.SetValue(sColumnName, sValue);
            }
        }
    }

    //*************************************************************************
    //  Method: FilterColumnNames()
    //
    /// <summary>
    /// Returns an array of table column names with some names filtered out.
    /// </summary>
    ///
    /// <param name="oExcelTableReader">
    /// Object that is reading the edge or vertex table.
    /// </param>
    ///
    /// <param name="oColumnNamesToExclude">
    /// HashSet of zero or more columns to exclude.
    /// </param>
    //*************************************************************************

    protected String []
    FilterColumnNames
    (
        ExcelTableReader oExcelTableReader,
        HashSet<String> oColumnNamesToExclude
    )
    {
        Debug.Assert(oExcelTableReader != null);
        Debug.Assert(oColumnNamesToExclude != null);
        AssertValid();

        List<String> oFilteredColumnNames = new List<String>();

        foreach (String sColumnName in oExcelTableReader.ColumnNames)
        {
            if ( !oColumnNamesToExclude.Contains(sColumnName) )
            {
                oFilteredColumnNames.Add(sColumnName);
            }
        }

        return ( oFilteredColumnNames.ToArray() );
    }

    //*************************************************************************
    //  Method: VertexNameToVertex()
    //
    /// <summary>
    /// Finds or creates a vertex given a vertex name.
    /// </summary>
    ///
    /// <param name="sVertexName">
    /// Name of the vertex to find or create.
    /// </param>
    ///
    /// <param name="oVertices">
    /// Vertex collection to add a new vertex to if a new vertex is created.
    /// </param>
    ///
    /// <param name="oVertexNameDictionary">
    /// Dictionary of existing vertices.  The key is the vertex name and the
    /// value is the vertex.
    /// </param>
    ///
    /// <returns>
    /// The found or created vertex.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="oVertexNameDictionary" /> contains a vertex named
    /// <paramref name="sVertexName" />, the vertex is returned.  Otherwise, a
    /// vertex is created, added to <paramref name="oVertices" />, added to
    /// <paramref name="oVertexNameDictionary" />, and returned.
    /// </remarks>
    //*************************************************************************

    protected IVertex
    VertexNameToVertex
    (
        String sVertexName,
        IVertexCollection oVertices,
        Dictionary<String, IVertex> oVertexNameDictionary
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sVertexName) );
        Debug.Assert(oVertices != null);
        Debug.Assert(oVertexNameDictionary != null);
        AssertValid();

        IVertex oVertex;

        if ( !oVertexNameDictionary.TryGetValue(sVertexName, out oVertex) )
        {
            oVertex =
                CreateVertex(sVertexName, oVertices, oVertexNameDictionary);
        }

        return (oVertex);
    }

    //*************************************************************************
    //  Method: CreateVertex()
    //
    /// <summary>
    /// Creates a vertex.
    /// </summary>
    ///
    /// <param name="sVertexName">
    /// Name of the vertex to create.
    /// </param>
    ///
    /// <param name="oVertices">
    /// Vertex collection to add the new vertex to.
    /// </param>
    ///
    /// <param name="oVertexNameDictionary">
    /// Dictionary of existing vertices.  The key is the vertex name and the
    /// value is the vertex.  The dictionary can't already contain <paramref
    /// name="sVertexName" />.
    /// </param>
    ///
    /// <returns>
    /// The created vertex.
    /// </returns>
    ///
    /// <remarks>
    /// This method creates a vertex and adds it to both <paramref
    /// name="oVertices" /> and <paramref name="oVertexNameDictionary" />.
    /// </remarks>
    //*************************************************************************

    protected IVertex
    CreateVertex
    (
        String sVertexName,
        IVertexCollection oVertices,
        Dictionary<String, IVertex> oVertexNameDictionary
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sVertexName) );
        Debug.Assert(oVertices != null);
        Debug.Assert(oVertexNameDictionary != null);
        AssertValid();

        IVertex oVertex = oVertices.Add();
        oVertex.Name = sVertexName;

        oVertexNameDictionary.Add(sVertexName, oVertex);

        return (oVertex);
    }

    //*************************************************************************
    //  Method: OnInvalidVisibility()
    //
    /// <summary>
    /// Handles an invalid edge or vertex visibility.
    /// </summary>
    ///
    /// <param name="oRow">
    /// Row containing the invalid visibility.
    /// </param>
    ///
    /// <remarks>
    /// This method throws a <see cref="WorkbookFormatException" />.
    /// </remarks>
    //*************************************************************************

    protected void
    OnInvalidVisibility
    (
        ExcelTableReader.ExcelTableRow oRow
    )
    {
        Debug.Assert(oRow != null);
        AssertValid();

        OnWorkbookFormatErrorWithDropDown(oRow,
            CommonTableColumnNames.Visibility, "visibility");
    }

    //*************************************************************************
    //  Method: OnWorkbookFormatErrorWithDropDown()
    //
    /// <summary>
    /// Handles a workbook format error that prevents a graph from being
    /// created, where the invalid cell has a drop-down list.
    /// </summary>
    ///
    /// <param name="oRow">
    /// Row containing the invalid cell.
    /// </param>
    ///
    /// <param name="sColumnName">
    /// Name of the column containing the invalid cell.
    /// </param>
    ///
    /// <param name="sInvalidCellDescription">
    /// Description of the invalid cell.  Sample: "shape".
    /// </param>
    //*************************************************************************

    protected void
    OnWorkbookFormatErrorWithDropDown
    (
        ExcelTableReader.ExcelTableRow oRow,
        String sColumnName,
        String sInvalidCellDescription
    )
    {
        Debug.Assert(oRow != null);
        Debug.Assert( !String.IsNullOrEmpty(sColumnName) );
        Debug.Assert( !String.IsNullOrEmpty(sInvalidCellDescription) );
        AssertValid();

        Range oInvalidCell = oRow.GetRangeForCell(sColumnName);

        OnWorkbookFormatError( String.Format(

            "The cell {0} contains an invalid {1}.  Try selecting"
            + " from the cell's drop-down list instead."
            ,
            ExcelUtil.GetRangeAddress(oInvalidCell),
            sInvalidCellDescription
            ),

            oInvalidCell
            );
    }

    //*************************************************************************
    //  Method: OnWorkbookFormatError()
    //
    /// <overloads>
    /// Handles a workbook format error that prevents a graph from being
    /// created.
    /// </overloads>
    ///
    /// <summary>
    /// Handles a workbook format error that prevents a graph from being
    /// created using a specified error message.
    /// </summary>
    ///
    /// <param name="sMessage">
    /// Error message.
    /// </param>
    ///
    /// <remarks>
    /// This method throws a <see cref="WorkbookFormatException" />.
    /// </remarks>
    //*************************************************************************

    protected void
    OnWorkbookFormatError
    (
        String sMessage
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sMessage) );
        AssertValid();

        OnWorkbookFormatError(sMessage, null);
    }

    //*************************************************************************
    //  Method: OnWorkbookFormatError()
    //
    /// <summary>
    /// Handles a workbook format error that prevents a graph from being
    /// created using a specified error message and range to select.
    /// </summary>
    ///
    /// <param name="sMessage">
    /// Error message.
    /// </param>
    ///
    /// <param name="oRangeToSelect">
    /// The range that the catch block should select to highlight the workbook
    /// error, or null if a range shouldn't be selected.
    /// </param>
    ///
    /// <remarks>
    /// This method throws a <see cref="WorkbookFormatException" />.
    /// </remarks>
    //*************************************************************************

    protected void
    OnWorkbookFormatError
    (
        String sMessage,
        Range oRangeToSelect
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sMessage) );
        AssertValid();

        throw new WorkbookFormatException(sMessage, oRangeToSelect);
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

        Debug.Assert(m_oAlphaConverter != null);
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// Index returned by GetTableColumn() when the specified column doesn't
    /// exist.

    public const Int32 NoSuchColumn = Int32.MinValue;

    /// Array of empty worksheet IDs.

    public static readonly Int32 [] EmptyIDArray = new Int32 [0];


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object that converts alpha values between those used in the Excel
    /// workbook and those used in the NodeXL graph.

    protected AlphaConverter m_oAlphaConverter;
}

}
