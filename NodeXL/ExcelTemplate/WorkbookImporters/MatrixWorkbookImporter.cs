
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: MatrixWorkbookImporter
//
/// <summary>
/// Imports a graph from an open matrix workbook to the edge worksheet.
/// </summary>
///
/// <remarks>
/// Call <see cref="ImportMatrixWorkbook" /> to import a graph from an open
/// matrix workbook to the edge worksheet of a NodeXL workbook.
/// </remarks>
//*****************************************************************************

public class MatrixWorkbookImporter : WorkbookImporterBase
{
    //*************************************************************************
    //  Constructor: MatrixWorkbookImporter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="MatrixWorkbookImporter" />
    /// class.
    /// </summary>
    //*************************************************************************

    public MatrixWorkbookImporter()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: ImportMatrixWorkbook()
    //
    /// <summary>
    /// Imports a graph from an open matrix workbook to the edge worksheet of a
    /// NodeXL workbook.
    /// </summary>
    ///
    /// <param name="sourceWorkbookName">
    /// Workbook.Name of the open workbook that contains the matrix to import.
    /// </param>
    ///
    /// <param name="sourceWorkbookHasVertexNames">
    /// true if the source workbook has vertex names in row 1 and column A,
    /// false if the source workbook has no vertex names and sequential vertex
    /// names should be assigned during importation.
    /// </param>
    ///
    /// <param name="sourceWorkbookDirectedness">
    /// The directedness of the graph represented by the source workbook.
    /// </param>
    ///
    /// <param name="destinationNodeXLWorkbook">
    /// NodeXL workbook the graph will be imported to.
    /// </param>
    ///
    /// <remarks>
    /// The source workbook must specify a numerical edge weight for each pair
    /// of vertices.  Empty edge weight cells are not allowed.
    ///
    /// <para>
    /// If the source workbook has vertex names, the names must be in row 1,
    /// starting in B1; and in column A, starting in A2.  The edge weights must
    /// start in B2.
    /// </para>
    ///
    /// <para>
    /// If the source workbook does not have vertex names, the edge weights
    /// must start in A1.
    /// </para>
    ///
    /// <para>
    /// If the source workbook represents a directed graph, every cell in the
    /// matrix is read.  If the source workbook represents an undirected graph,
    /// the matrix must be symmetric and only the cells in the matrix's
    /// diagonal and above are read.
    ///
    /// An edge weight on the diagonal, which represents a self-loop, is not
    /// treated in any special way.  It's up to the user to decide which
    /// convention she wants to adopt regarding the meaning of self-loop edge
    /// weights.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    ImportMatrixWorkbook
    (
        String sourceWorkbookName,
        Boolean sourceWorkbookHasVertexNames,
        GraphDirectedness sourceWorkbookDirectedness,
        Microsoft.Office.Interop.Excel.Workbook destinationNodeXLWorkbook
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sourceWorkbookName) );
        Debug.Assert(destinationNodeXLWorkbook != null);
        AssertValid();

        // Get the active worksheet of the source workbook.

        Application oApplication = destinationNodeXLWorkbook.Application;

        Worksheet oSourceWorksheet = GetActiveSourceWorksheet(
            oApplication, sourceWorkbookName);

        // Read or create the names of the vertices in the source workbook.

        String [] asVertexNames;
        Int32 iFirstEdgeWeightRowOneBased, iFirstEdgeWeightColumnOneBased;

        if (sourceWorkbookHasVertexNames)
        {
            asVertexNames = ReadVertexNames(oSourceWorksheet);
            iFirstEdgeWeightRowOneBased = iFirstEdgeWeightColumnOneBased = 2;
        }
        else
        {
            asVertexNames = CreateVertexNames(oSourceWorksheet);
            iFirstEdgeWeightRowOneBased = iFirstEdgeWeightColumnOneBased = 1;
        }

        Boolean bGraphIsDirected = false;

        switch (sourceWorkbookDirectedness)
        {
            case GraphDirectedness.Directed:

                bGraphIsDirected = true;
                break;

            case GraphDirectedness.Undirected:

                CheckSymmetryOfUndirectedMatrix(oSourceWorksheet,
                    iFirstEdgeWeightRowOneBased,
                    iFirstEdgeWeightColumnOneBased, asVertexNames.Length);

                break;

            default:

                Debug.Assert(false);
                break;
        }

        // Read the edge weights and write the results to the destination
        // workbook.

        LinkedList<String> oVertex1Names = new LinkedList<String>();
        LinkedList<String> oVertex2Names = new LinkedList<String>();
        LinkedList <Double> oEdgeWeights = new LinkedList<Double>();

        ReadEdgeWeights(oSourceWorksheet, bGraphIsDirected,
            iFirstEdgeWeightRowOneBased, iFirstEdgeWeightColumnOneBased,
            asVertexNames, oVertex1Names, oVertex2Names, oEdgeWeights
            );

        WriteToDestinationWorkbook(oVertex1Names, oVertex2Names, oEdgeWeights,
            destinationNodeXLWorkbook);
    }

    //*************************************************************************
    //  Method: ReadVertexNames()
    //
    /// <summary>
    /// Reads the names of the vertices in the source workbook.
    /// </summary>
    ///
    /// <param name="oSourceWorksheet">
    /// Worksheet that contains the matrix to import.
    /// </param>
    ///
    /// <returns>
    /// An array of vertex names.  Each element of the array will contain a
    /// string with a length greater than zero.
    /// </returns>
    ///
    /// <remarks>
    /// An ImportWorkbookException is thrown if the vertex names couldn't
    /// be read because of invalid source workbook contents.
    /// </remarks>
    //*************************************************************************

    protected String []
    ReadVertexNames
    (
        Microsoft.Office.Interop.Excel.Worksheet oSourceWorksheet
    )
    {
        Debug.Assert(oSourceWorksheet != null);
        AssertValid();

        String sA1String;

        if ( ExcelUtil.TryGetNonEmptyStringFromCell(oSourceWorksheet, 1, 1,
            out sA1String) )
        {
            OnInvalidSourceWorkbook(
                "A matrix workbook that has vertex names must have an empty A1"
                + " cell.",
                oSourceWorksheet, 1, 1
                );
        }

        // Get the row of vertex names, which is the range from B1 to the
        // rightmost contiguous, non-empty cell.

        Object [,] aoVertexNames;

        if ( !ExcelUtil.TryGetContiguousValuesInRow(oSourceWorksheet, 1, 2,
            out aoVertexNames) )
        {
            OnInvalidSourceWorkbook(
                "A matrix workbook that has vertex names must have the first"
                + " vertex name in B1.",
                oSourceWorksheet, 1, 2
                );
        }

        Int32 iVertexNames = aoVertexNames.GetUpperBound(1);
        String [] asVertexNames = new String[iVertexNames];

        // Read the vertex names.

        for (Int32 iColumnOneBased = 1; iColumnOneBased <= iVertexNames;
            iColumnOneBased++)
        {
            if ( !ExcelUtil.TryGetNonEmptyStringFromCell( aoVertexNames,
                1, iColumnOneBased,
                out asVertexNames[iColumnOneBased - 1] ) )
            {
                OnInvalidSourceWorkbook(SpacesOnlyMessage, oSourceWorksheet,
                    1, iColumnOneBased + 1);
            }
        }

        // Get the column of vertex names, which is the range from A2 to the
        // bottommost contiguous, non-empty cell.

        if ( !ExcelUtil.TryGetContiguousValuesInColumn(oSourceWorksheet, 2, 1,
            out aoVertexNames) )
        {
            OnInvalidSourceWorkbook(
                "A matrix workbook that has vertex names must have the first"
                + " vertex name in A2.",
                oSourceWorksheet, 2, 1
                );
        }

        Int32 iVertexNamesInColumnA = aoVertexNames.GetUpperBound(0);

        if (iVertexNamesInColumnA != iVertexNames)
        {
            OnInvalidSourceWorkbook( String.Format(

                "A matrix workbook that has vertex names must have the same"
                + " number of names in row 1 as in column A.  It looks like"
                + " there are {0} {1} in row 1 and {2} {3} in column A."
                ,
                iVertexNames,
                StringUtil.MakePlural("name", iVertexNames),
                iVertexNamesInColumnA,
                StringUtil.MakePlural("name", iVertexNamesInColumnA)
                ),
                oSourceWorksheet, iVertexNamesInColumnA + 1, 1
                );
        }

        // Compare the vertex names in row 1 to the names in column A.

        for (Int32 iRowOneBased = 1; iRowOneBased <= iVertexNames;
            iRowOneBased++)
        {
            String sVertexName;

            if ( !ExcelUtil.TryGetNonEmptyStringFromCell(aoVertexNames,
                iRowOneBased, 1, out sVertexName) )
            {
                OnInvalidSourceWorkbook(SpacesOnlyMessage, oSourceWorksheet,
                    iRowOneBased + 1, 1);
            }

            if ( sVertexName != asVertexNames[iRowOneBased - 1] )
            {
                OnInvalidSourceWorkbook( String.Format(

                    "The vertex name in {0} does not match the vertex name"
                    + " in {1}.  The vertex names in row 1 must exactly match"
                    + " the vertex names in column A."
                    ,
                    ExcelUtil.GetCellAddress(oSourceWorksheet,
                        iRowOneBased + 1, 1),

                    ExcelUtil.GetCellAddress(oSourceWorksheet,
                        1, iRowOneBased + 1)
                    ),
                    oSourceWorksheet, iRowOneBased + 1, 1
                    );
            }
        }

        aoVertexNames = null;

        // Check for duplicate names.  Why not use a Dictionary from the start
        // instead of an array?  Because the elements need to be ordered as
        // they appear in the workbook, and a Dictionary doesn't provide
        // ordering.

        Dictionary<String, Byte> oDuplicateNameChecker =
            new Dictionary<String, Byte>(iVertexNames);

        for (Int32 i = 0; i < iVertexNames; i++)
        {
            String sVertexName = asVertexNames[i];

            if ( oDuplicateNameChecker.ContainsKey(sVertexName) )
            {
                OnInvalidSourceWorkbook( String.Format(

                    "The vertex name in {0} is a duplicate.  Vertex names must"
                    + " be unique."
                    ,
                    ExcelUtil.GetCellAddress(oSourceWorksheet,
                        1, i + 2)
                    ),
                    oSourceWorksheet, 1, i + 2
                    );
            }

            oDuplicateNameChecker.Add(sVertexName, 0);
        }

        return (asVertexNames);
    }

    //*************************************************************************
    //  Method: CreateVertexNames()
    //
    /// <summary>
    /// Creates names for the vertices in the source workbook.
    /// </summary>
    ///
    /// <param name="oSourceWorksheet">
    /// Worksheet that contains the matrix to import.
    /// </param>
    ///
    /// <returns>
    /// An array of vertex names.  Each element of the array will contain a
    /// string with a length greater than zero.
    /// </returns>
    ///
    /// <remarks>
    /// An ImportWorkbookException is thrown if the vertex names couldn't
    /// be created because of invalid source workbook contents.
    /// </remarks>
    //*************************************************************************

    protected String []
    CreateVertexNames
    (
        Microsoft.Office.Interop.Excel.Worksheet oSourceWorksheet
    )
    {
        Debug.Assert(oSourceWorksheet != null);
        AssertValid();

        String sA1String;

        if ( !ExcelUtil.TryGetNonEmptyStringFromCell(oSourceWorksheet, 1, 1,
            out sA1String) )
        {
            OnInvalidSourceWorkbook(
                "In a matrix workbook that does not have vertex names, the"
                + " first edge weight number must be in A1.",
                oSourceWorksheet, 1, 1
                );
        }

        // To determine the size of the matrix, get the first row of edge
        // weights.

        Object [,] aoEdgeWeights;

        if ( !ExcelUtil.TryGetContiguousValuesInRow(oSourceWorksheet, 1, 1,
            out aoEdgeWeights) )
        {
            Debug.Assert(false);
        }

        Int32 iColumns = aoEdgeWeights.GetUpperBound(1);

        // Compare to the first column of edge weights.

        if ( !ExcelUtil.TryGetContiguousValuesInColumn(oSourceWorksheet, 1, 1,
            out aoEdgeWeights) )
        {
            Debug.Assert(false);
        }

        Int32 iRows = aoEdgeWeights.GetUpperBound(0);

        if (iRows != iColumns)
        {
            OnInvalidSourceWorkbook( String.Format(

                "The matrix workbook must have an equal number of rows and"
                + " columns.  Judging from the first empty cell in column 1"
                + " and the first empty cell in row 1, it looks like there"
                + " are {0} {1} and {2} {3}."
                ,
                iRows,
                StringUtil.MakePlural("row", iRows),
                iColumns,
                StringUtil.MakePlural("column", iColumns)
                ),
                oSourceWorksheet, 1, 1
                );
        }

        // Generate the vertex names.

        String [] asVertexNames = new String[iRows];

        for (Int32 i = 0; i < iRows; i++)
        {
            asVertexNames[i] = "Vertex " + (i + 1).ToString();
        }

        return (asVertexNames);
    }

    //*************************************************************************
    //  Method: ReadEdgeWeights()
    //
    /// <summary>
    /// Reads the edge weights in the source workbook.
    /// </summary>
    ///
    /// <param name="oSourceWorksheet">
    /// Worksheet that contains the matrix to import.
    /// </param>
    ///
    /// <param name="bGraphIsDirected">
    /// true if the matrix represents a directed graph, false if it represents
    /// an undirected graph.
    /// </param>
    ///
    /// <param name="iFirstEdgeWeightRowOneBased">
    /// One-based row number of the first edge weight cell.
    /// </param>
    ///
    /// <param name="iFirstEdgeWeightColumnOneBased">
    /// One-based column number of the first edge weight cell.
    /// </param>
    ///
    /// <param name="asVertexNames">
    /// An array of vertex names, one for each row (and column) of the matrix.
    /// Every element is of non-zero length.
    /// </param>
    ///
    /// <param name="oVertex1Names">
    /// Where the vertex names to store in the vertex 1 column of the
    /// destination NodeXL workbook get stored.
    /// </param>
    ///
    /// <param name="oVertex2Names">
    /// Where the vertex names to store in the vertex 2 column of the
    /// destination NodeXL workbook get stored.
    /// </param>
    ///
    /// <param name="oEdgeWeights">
    /// Where the values to store in the edge weight column of the destination
    /// NodeXL workbook get stored.
    /// </param>
    ///
    /// <remarks>
    /// The lengths of <paramref name="oVertex1Names" />, <paramref
    /// name="oVertex2Value" />, and <paramref name="oEdgeWeights" /> are
    /// equal when this method returns.
    ///
    /// <para>
    /// An ImportWorkbookException is thrown if the edge weights couldn't be
    /// read because of invalid source workbook contents.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected void
    ReadEdgeWeights
    (
        Microsoft.Office.Interop.Excel.Worksheet oSourceWorksheet,
        Boolean bGraphIsDirected,
        Int32 iFirstEdgeWeightRowOneBased,
        Int32 iFirstEdgeWeightColumnOneBased,
        String [] asVertexNames,
        LinkedList<String> oVertex1Names,
        LinkedList<String> oVertex2Names,
        LinkedList<Double> oEdgeWeights
    )
    {
        Debug.Assert(oSourceWorksheet != null);
        Debug.Assert(iFirstEdgeWeightRowOneBased >= 1);
        Debug.Assert(iFirstEdgeWeightColumnOneBased >= 1);
        Debug.Assert(asVertexNames != null);
        Debug.Assert(asVertexNames.Length > 0);
        Debug.Assert(oVertex1Names != null);
        Debug.Assert(oVertex1Names.Count == 0);
        Debug.Assert(oVertex2Names != null);
        Debug.Assert(oVertex2Names.Count == 0);
        Debug.Assert(oEdgeWeights != null);
        Debug.Assert(oEdgeWeights.Count == 0);

        Int32 iVertices = asVertexNames.Length;

        for (Int32 i = 0; i < iVertices; i++)
        {
            Int32 iRowOneBased = iFirstEdgeWeightRowOneBased + i;

            Object [,] oRowOfEdgeWeightValues = ExcelUtil.GetValuesInRow(
                oSourceWorksheet, iRowOneBased, iFirstEdgeWeightColumnOneBased,
                iVertices);

            // In a directed graph, look at the whole row.  In an undirected
            // graph, look only at the cell on the diagonal and the cells to
            // the right.

            for (Int32 j = (bGraphIsDirected ? 0 : i); j < iVertices; j++)
            {
                Double dEdgeWeight = 0;
                Int32 iColumnOneBased = iFirstEdgeWeightColumnOneBased + j;

                if ( !ExcelUtil.TryGetDoubleFromCell(oRowOfEdgeWeightValues, 1,
                        j + 1, out dEdgeWeight) )
                {
                    OnInvalidSourceWorkbook( String.Format(

                        "The edge weight in {0} must be a number."
                        ,
                        ExcelUtil.GetCellAddress(oSourceWorksheet,
                            iRowOneBased, iColumnOneBased)
                        ),
                        oSourceWorksheet, iRowOneBased, iColumnOneBased
                        );
                }

                if (dEdgeWeight != 0)
                {
                    oVertex1Names.AddLast( asVertexNames[i] );
                    oVertex2Names.AddLast( asVertexNames[j] );
                    oEdgeWeights.AddLast(dEdgeWeight);
                }
            }
        }
    }

    //*************************************************************************
    //  Method: CheckSymmetryOfUndirectedMatrix()
    //
    /// <summary>
    /// Checks the symmetry of an undirected matrix.
    /// </summary>
    ///
    /// <param name="oSourceWorksheet">
    /// Worksheet that contains the matrix to import.
    /// </param>
    ///
    /// <param name="iFirstEdgeWeightRowOneBased">
    /// One-based row number of the first edge weight cell.
    /// </param>
    ///
    /// <param name="iFirstEdgeWeightColumnOneBased">
    /// One-based column number of the first edge weight cell.
    /// </param>
    ///
    /// <param name="iVertices">
    /// Number of vertices in the graph.
    /// </param>
    ///
    /// <remarks>
    /// An ImportWorkbookException is thrown if the matrix is not symmetric.
    /// </remarks>
    //*************************************************************************

    protected void
    CheckSymmetryOfUndirectedMatrix
    (
        Microsoft.Office.Interop.Excel.Worksheet oSourceWorksheet,
        Int32 iFirstEdgeWeightRowOneBased,
        Int32 iFirstEdgeWeightColumnOneBased,
        Int32 iVertices
    )
    {
        Debug.Assert(oSourceWorksheet != null);
        Debug.Assert(iFirstEdgeWeightRowOneBased >= 1);
        Debug.Assert(iFirstEdgeWeightColumnOneBased >= 1);
        Debug.Assert(iVertices > 0);

        for (Int32 i = 0; i < iVertices; i++)
        {
            // Read row i and column i.

            Object [,] oRowOfEdgeWeightValues = ExcelUtil.GetValuesInRow(
                oSourceWorksheet, iFirstEdgeWeightRowOneBased + i,
                iFirstEdgeWeightColumnOneBased, iVertices);

            Object [,] oColumnOfEdgeWeightValues =
                ExcelUtil.GetValuesInColumn(oSourceWorksheet,
                iFirstEdgeWeightRowOneBased,
                iFirstEdgeWeightColumnOneBased + i, iVertices);

            for (Int32 j = 0; j < iVertices; j++)
            {
                String sRowString = null;
                String sColumnString = null;

                if (
                    !ExcelUtil.TryGetNonEmptyStringFromCell(
                        oRowOfEdgeWeightValues, 1, j + 1, out sRowString)
                    ||
                    !ExcelUtil.TryGetNonEmptyStringFromCell(
                        oColumnOfEdgeWeightValues, j + 1, 1, out sColumnString)
                    ||
                    sRowString != sColumnString
                    )
                {
                    Int32 iBadCoordinate1 = iFirstEdgeWeightRowOneBased + i;
                    Int32 iBadCoordinate2 = iFirstEdgeWeightColumnOneBased + j;

                    OnInvalidSourceWorkbook( String.Format(

                        "The edge weights in {0} and {1} must be identical"
                        + " numbers.  (The matrix for an undirected graph must"
                        + " be symmetric.)"
                        ,
                        ExcelUtil.GetCellAddress(oSourceWorksheet,
                            iBadCoordinate1, iBadCoordinate2),

                        ExcelUtil.GetCellAddress(oSourceWorksheet,
                            iBadCoordinate2, iBadCoordinate1)
                        ),
                        oSourceWorksheet, iBadCoordinate1, iBadCoordinate2
                        );
                }
            }
        }
    }

    //*************************************************************************
    //  Method: WriteToDestinationWorkbook()
    //
    /// <summary>
    /// Writes the imported graph to the destination workbook.
    /// </summary>
    ///
    /// <param name="oVertex1Names">
    /// Vertex names to write to the vertex 1 column.
    /// </param>
    ///
    /// <param name="oVertex2Names">
    /// Vertex names to write to the vertex 2 column.
    /// </param>
    ///
    /// <param name="oEdgeWeights">
    /// Edge weights to write to the edge weight column.
    /// </param>
    ///
    /// <param name="oDestinationNodeXLWorkbook">
    /// NodeXL workbook the graph will be imported to.
    /// </param>
    //*************************************************************************

    protected void
    WriteToDestinationWorkbook
    (
        LinkedList<String> oVertex1Names,
        LinkedList<String> oVertex2Names,
        LinkedList <Double> oEdgeWeights,
        Microsoft.Office.Interop.Excel.Workbook oDestinationNodeXLWorkbook
    )
    {
        Debug.Assert(oVertex1Names != null);
        Debug.Assert(oVertex2Names != null);
        Debug.Assert(oEdgeWeights != null);
        Debug.Assert(oVertex2Names.Count == oVertex1Names.Count);
        Debug.Assert(oEdgeWeights.Count == oVertex1Names.Count);
        Debug.Assert(oDestinationNodeXLWorkbook != null);
        AssertValid();

        Int32 iVertexNames = oVertex1Names.Count;

        ListObject oDestinationEdgeTable = GetDestinationEdgeTable(
            oDestinationNodeXLWorkbook);

        ExcelUtil.ActivateWorksheet(oDestinationEdgeTable);

        // Clear the various tables.

        NodeXLWorkbookUtil.ClearTables(oDestinationNodeXLWorkbook);

        // Create an array to hold the vertex 1 names and write them to the
        // edge table.  Repeat for the vertex 2 names.

        String [,] asVertexNames =
            ExcelUtil.GetSingleColumn2DStringArray(iVertexNames);

        Int32 iRowOneBased;

        foreach ( Boolean bVertex1 in new Boolean [] {true, false} )
        {
            iRowOneBased = 1;

            foreach (String sVertexName in
                bVertex1 ? oVertex1Names: oVertex2Names)
            {
                // VertexWorksheetPopulator.PopulateVertexWorksheet() writes
                // strings to the vertex column of the vertex table, so the
                // values must all be strings.

                asVertexNames[iRowOneBased, 1] = sVertexName;
                iRowOneBased++;
            }

            Range oVertexColumnData = GetVertexColumnData(
                oDestinationEdgeTable, bVertex1);

            ExcelUtil.SetRangeValues(oVertexColumnData, asVertexNames);
        }

        asVertexNames = null;

        // Create an array to hold the edge weights and write them to the edge
        // table.

        Object [,] aoEdgeWeights =
            ExcelUtil.GetSingleColumn2DArray(iVertexNames);

        iRowOneBased = 1;

        foreach (Double dEdgeWeight in oEdgeWeights)
        {
            aoEdgeWeights[iRowOneBased, 1] = dEdgeWeight;
            iRowOneBased++;
        }

        ListColumn oEdgeWeightColumn;
        Range oEdgeWeightColumnData = null;

        if (
            !ExcelUtil.TryGetOrAddTableColumn(oDestinationEdgeTable,
                EdgeTableColumnNames.EdgeWeight, ExcelUtil.AutoColumnWidth,
                null, out oEdgeWeightColumn)
            ||
            !ExcelUtil.TryGetTableColumnData(oDestinationEdgeTable,
                EdgeTableColumnNames.EdgeWeight, out oEdgeWeightColumnData)
            )
        {
            OnInvalidSourceWorkbook( String.Format(

                "An {0} column couldn't be added to the edge worksheet."
                ,
                EdgeTableColumnNames.EdgeWeight
                ) );
        }

        ExcelUtil.SetRangeValues(oEdgeWeightColumnData, aoEdgeWeights);
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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Message to show when a vertex name consists only of spaces.

    protected const String SpacesOnlyMessage =
        "A vertex name can't consist only of spaces."
        ;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
