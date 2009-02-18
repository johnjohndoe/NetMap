
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Layouts;
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
    /// Fills part of a table column with a sequence of unique IDs.
    /// </summary>
    ///
    /// <param name="oTable">
    /// Table containing the ID column.
    /// </param>
    ///
    /// <param name="iIDColumnIndex">
    /// One-based index of the table column that contains unique IDs, or
    /// NoSuchColumn if there is no such column.  In the latter case, this
    /// method does nothing.
    /// </param>
    ///
    /// <param name="oSubrange">
    /// Subrange of the table that encompasses the rows within the ID column
    /// that should be filled in.  Can't contain multiple areas.
    /// </param>
    //*************************************************************************

    protected void
    FillIDColumn
    (
        ListObject oTable,
        Int32 iIDColumnIndex,
        Range oSubrange
    )
    {
        Debug.Assert(oTable != null);
        Debug.Assert(iIDColumnIndex == NoSuchColumn || iIDColumnIndex >= 1);
        Debug.Assert(oSubrange != null);
        Debug.Assert(oSubrange.Areas.Count == 1);
        AssertValid();

        if (iIDColumnIndex == NoSuchColumn)
        {
            return;
        }

        Range oDataBodyRange = oTable.DataBodyRange;

        if (oDataBodyRange == null)
        {
            return;
        }

        Range oUsedRange;

        if ( !ExcelUtil.TryGetNonEmptyRange(oDataBodyRange, out oUsedRange) )
        {
            // The table is empty, so do nothing.  (The default state of a
            // table in a new workbook is one empty row.)

            return;
        }

        Debug.Assert(oTable.Parent is Worksheet);

        Worksheet oWorksheet = (Worksheet)oTable.Parent;

        // Get the rows within the ID column that should be filled in.

        Int32 iSubrangeStartRowOneBased = oSubrange.Row;
        Int32 iRowsInSubrange = oSubrange.Rows.Count;
        Int32 iTableStartRowOneBased = oTable.Range.Row;

        Range oIDRange = oWorksheet.get_Range(

            oDataBodyRange.Cells[
                iSubrangeStartRowOneBased - iTableStartRowOneBased,
                iIDColumnIndex
                ],

            oDataBodyRange.Cells[
                iSubrangeStartRowOneBased - iTableStartRowOneBased
                    + iRowsInSubrange - 1,
                iIDColumnIndex
                ]
            );

        // Use the Excel row numbers as the unique IDs.  Create a one-column
        // array, then fill it in with the row numbers.

        Int32 iRows = oIDRange.Rows.Count;

        Object [,] aoValues = ExcelUtil.GetSingleColumn2DArray(iRows);

        for (Int32 i = 1; i <= iRows; i++)
        {
            aoValues[i, 1] = iSubrangeStartRowOneBased + i - 1;
        }

        oIDRange.Value2 = aoValues;

        #if false

        // Note: Don't use the following clever code to fill in the row
        // numbers.  On large worksheets, the calculations take forever.

        oIDRange.Value2 = "=ROW()";
        oIDRange.Value2 = oIDRange.Value2;

        #endif
    }

    //*************************************************************************
    //  Method: CheckForAlpha()
    //
    /// <summary>
    /// If an alpha has been specified for an edge or vertex, sets the alpha
    /// value on the edge or vertex.
    /// </summary>
    ///
    /// <param name="oRange">
    /// Range containing the edge or vertex data.
    /// </param>
    ///
    /// <param name="aoValues">
    /// Values from <paramref name="oRange" />.
    /// </param>
    ///
    /// <param name="iRowOneBased">
    /// One-based row index to check.
    /// </param>
    ///
    /// <param name="iAlphaColumnOneBased">
    /// One-based alpha column index to check.
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
    CheckForAlpha
    (
        Range oRange,
        Object [,] aoValues,
        Int32 iRowOneBased,
        Int32 iAlphaColumnOneBased,
        IMetadataProvider oEdgeOrVertex
    )
    {
        Debug.Assert(oRange != null);
        Debug.Assert(aoValues != null);
        Debug.Assert(iRowOneBased >= 1);
        Debug.Assert(iAlphaColumnOneBased >= 1);
        Debug.Assert(oEdgeOrVertex != null);

        AssertValid();

        String sString;

        if ( !ExcelUtil.TryGetNonEmptyStringFromCell(aoValues, iRowOneBased,
            iAlphaColumnOneBased, out sString) )
        {
            return (false);
        }

        Single fAlpha;

        if ( !Single.TryParse(sString, out fAlpha) )
        {
            Range oInvalidCell =
                (Range)oRange.Cells[iRowOneBased, iAlphaColumnOneBased];

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

        Byte btAlpha = m_oAlphaConverter.WorkbookToGraph(fAlpha);

        oEdgeOrVertex.SetValue(ReservedMetadataKeys.PerAlpha, btAlpha);

        return (btAlpha == 0);
    }

    //*************************************************************************
    //  Method: CheckForColor()
    //
    /// <summary>
    /// If a color has been specified for an edge or vertex, sets the edge's
    /// or vertex's color.
    /// </summary>
    ///
    /// <param name="oRange">
    /// Range containing the edge or vertex data.
    /// </param>
    ///
    /// <param name="aoValues">
    /// Values from <paramref name="oRange" />.
    /// </param>
    ///
    /// <param name="iRowOneBased">
    /// One-based row index to check.
    /// </param>
    ///
    /// <param name="iColumnOneBased">
    /// One-based column index to check.
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
    CheckForColor
    (
        Range oRange,
        Object [,] aoValues,
        Int32 iRowOneBased,
        Int32 iColumnOneBased,
        IMetadataProvider oEdgeOrVertex,
        String sColorKey,
        ColorConverter2 oColorConverter2
    )
    {
        Debug.Assert(oRange != null);
        Debug.Assert(aoValues != null);
        Debug.Assert(iRowOneBased >= 1);
        Debug.Assert(iColumnOneBased >= 1);
        Debug.Assert(oEdgeOrVertex != null);
        Debug.Assert( !String.IsNullOrEmpty(sColorKey) );
        Debug.Assert(oColorConverter2 != null);
        AssertValid();

        Color oColor;

        if ( TryGetColor(oRange, aoValues, iRowOneBased, iColumnOneBased,
                oColorConverter2, out oColor) )
        {
            oEdgeOrVertex.SetValue(sColorKey, oColor);
        }
    }

    //*************************************************************************
    //  Method: CheckForVertexShape()
    //
    /// <summary>
    /// If a shape has been specified for a vertex, sets the vertex's shape.
    /// </summary>
    ///
    /// <param name="oVertexRange">
    /// Range containing the vertex data.
    /// </param>
    ///
    /// <param name="aoValues">
    /// Values from <paramref name="oVertexRange" />.
    /// </param>
    ///
    /// <param name="iRowOneBased">
    /// One-based row index to check.
    /// </param>
    ///
    /// <param name="iColumnOneBased">
    /// One-based column index to check.
    /// </param>
    ///
    /// <param name="oVertex">
    /// Vertex to set the shape on.
    /// </param>
    //*************************************************************************

    protected void
    CheckForVertexShape
    (
        Range oVertexRange,
        Object [,] aoValues,
        Int32 iRowOneBased,
        Int32 iColumnOneBased,
        IVertex oVertex
    )
    {
        Debug.Assert(oVertexRange != null);
        Debug.Assert(aoValues != null);
        Debug.Assert(iRowOneBased >= 1);
        Debug.Assert(iColumnOneBased >= 1);
        Debug.Assert(oVertex != null);
        AssertValid();

        VertexShape eShape;

        if ( TryGetVertexShape(oVertexRange, aoValues, iRowOneBased,
            iColumnOneBased, out eShape) )
        {
            oVertex.SetValue(ReservedMetadataKeys.PerVertexShape, eShape);
        }
    }

    //*************************************************************************
    //  Method: TryGetColor()
    //
    /// <summary>
    /// Attempts to get a color from a worksheet cell.
    /// </summary>
    ///
    /// <param name="oRange">
    /// Range to check.
    /// </param>
    ///
    /// <param name="aoValues">
    /// Values from <paramref name="oRange" />.
    /// </param>
    ///
    /// <param name="iRowOneBased">
    /// One-based row index to check.
    /// </param>
    ///
    /// <param name="iColumnOneBased">
    /// One-based column index to check.
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
        Range oRange,
        Object [,] aoValues,
        Int32 iRowOneBased,
        Int32 iColumnOneBased,
        ColorConverter2 oColorConverter2,
        out Color oColor
    )
    {
        Debug.Assert(oRange != null);
        Debug.Assert(aoValues != null);
        Debug.Assert(iRowOneBased >= 1);
        Debug.Assert(iColumnOneBased >= 1);
        Debug.Assert(oColorConverter2 != null);
        AssertValid();

        oColor = Color.Empty;

        String sColor;

        if ( !ExcelUtil.TryGetNonEmptyStringFromCell(aoValues,
            iRowOneBased, iColumnOneBased, out sColor) )
        {
            return (false);
        }

        if ( !oColorConverter2.TryWorkbookToGraph(sColor, out oColor) )
        {
            Range oInvalidCell =
                (Range)oRange.Cells[iRowOneBased, iColumnOneBased];

            OnWorkbookFormatError( String.Format(

                "The cell {0} contains an unrecognized color.  Try"
                + " selecting from the cell's drop-down list instead."
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
    /// <param name="oVertexRange">
    /// Range containing the vertex data.
    /// </param>
    ///
    /// <param name="aoValues">
    /// Values from <paramref name="oVertexRange" />.
    /// </param>
    ///
    /// <param name="iRowOneBased">
    /// One-based row index to check.
    /// </param>
    ///
    /// <param name="iColumnOneBased">
    /// One-based column index to check.
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
    /// If the specified cell is empty, false is returned.  If the cell
    /// contains a valid vertex shape, the shape gets stored at <paramref
    /// name="eShape" /> and true is returned.  If the cell contains an invalid
    /// shape, a <see cref="WorkbookFormatException" /> is thrown.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    TryGetVertexShape
    (
        Range oVertexRange,
        Object [,] aoValues,
        Int32 iRowOneBased,
        Int32 iColumnOneBased,
        out VertexShape eShape
    )
    {
        Debug.Assert(oVertexRange != null);
        Debug.Assert(aoValues != null);
        Debug.Assert(iRowOneBased >= 1);
        Debug.Assert(iColumnOneBased >= 1);
        AssertValid();

        eShape = VertexShape.Circle;

        String sShape;

        if ( !ExcelUtil.TryGetNonEmptyStringFromCell(aoValues,
            iRowOneBased, iColumnOneBased, out sShape) )
        {
            return (false);
        }

        VertexShapeConverter oVertexShapeConverter =
            new VertexShapeConverter();

        if ( !oVertexShapeConverter.TryWorkbookToGraph(sShape, out eShape) )
        {
            Range oInvalidCell =
                (Range)oVertexRange.Cells[iRowOneBased, iColumnOneBased];

            OnWorkbookFormatError( String.Format(

                "The cell {0} contains an invalid shape.  Try selecting from"
                + " the cell's drop-down list instead."
                ,
                ExcelUtil.GetRangeAddress(oInvalidCell)
                ),

                oInvalidCell
            );
        }

        return (true);
    }

    //*************************************************************************
    //  Method: AddToIDDictionary()
    //
    /// <summary>
    /// Adds an edge or vertex to a dictionary and sets the edge or vertex's
    /// Tag to the ID.
    /// </summary>
    ///
    /// <param name="oRange">
    /// Range containing the edge or vertex data.
    /// </param>
    ///
    /// <param name="aoValues">
    /// Values from <paramref name="oRange" />.
    /// </param>
    ///
    /// <param name="iRowOneBased">
    /// One-based row index to check.
    /// </param>
    ///
    /// <param name="iIDColumnOneBased">
    /// One-based ID column index to check.
    /// </param>
    ///
    /// <param name="oEdgeOrVertex">
    /// Edge or vertex to get the ID for.
    /// </param>
    ///
    /// <param name="oDictionary">
    /// Keeps track of the edges or vertices that have been added to the graph,
    /// depending on which derived class this method is being called from.  The
    /// key is the edge or vertex ID read from the edge or vertex worksheet and
    /// the value is the edge or vertex.
    /// </param>
    //*************************************************************************

    protected void
    AddToIDDictionary
    (
        Range oRange,
        Object [,] aoValues,
        Int32 iRowOneBased,
        Int32 iIDColumnOneBased,
        IIdentityProvider oEdgeOrVertex,
        Dictionary<Int32, IIdentityProvider> oDictionary
    )
    {
        Debug.Assert(oRange != null);
        Debug.Assert(aoValues != null);
        Debug.Assert(iRowOneBased >= 1);
        Debug.Assert(iIDColumnOneBased >= 1);
        Debug.Assert(oEdgeOrVertex != null);
        Debug.Assert(oDictionary != null);
        AssertValid();

        // Because the derived class fills in its ID column if the column
        // exists, each cell in the column must be valid.

        String sID;
        Int32 iID = Int32.MinValue;

        if (
            ExcelUtil.TryGetNonEmptyStringFromCell(aoValues, iRowOneBased,
                iIDColumnOneBased, out sID)
            &&
            Int32.TryParse(sID, out iID)
            )
        {
            oDictionary.Add(iID, oEdgeOrVertex);

            Debug.Assert(oEdgeOrVertex is IMetadataProvider);

            // Store the ID in the edge or vertex tag.

            ( (IMetadataProvider)oEdgeOrVertex ).Tag = iID;
        }
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

        // Start out with a vertex location that will cause
        // FruchtermanReingoldLayout, if it's being used, to begin the layout
        // by randomizing the vertex location.  This location may get
        // overwritten later.

        oVertex.Location = LayoutBase.RandomizeThisLocation;

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
    /// <param name="oRange">
    /// Range containing the invalid visibility.
    /// </param>
    ///
    /// <param name="iRowOneBased">
    /// One-based row number of the cell containing the invalid visibility.
    /// </param>
    ///
    /// <param name="iColumnOneBased">
    /// One-based column number of the cell containing the invalid visibility.
    /// </param>
    ///
    /// <remarks>
    /// This method throws a <see cref="WorkbookFormatException" />.
    /// </remarks>
    //*************************************************************************

    protected void
    OnInvalidVisibility
    (
        Range oRange,
        Int32 iRowOneBased,
        Int32 iColumnOneBased
    )
    {
        Debug.Assert(oRange != null);
        Debug.Assert(iRowOneBased >= 1);
        Debug.Assert(iColumnOneBased >= 1);
        AssertValid();

        Range oInvalidCell =
            (Range)oRange.Cells[iRowOneBased, iColumnOneBased];

        OnWorkbookFormatError( String.Format(

            "The cell {0} contains an unrecognized visibility.  Try selecting"
            + " from the cell's drop-down list instead."
            ,
            ExcelUtil.GetRangeAddress(oInvalidCell)
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
