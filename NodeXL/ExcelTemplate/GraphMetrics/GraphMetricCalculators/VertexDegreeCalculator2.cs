
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Algorithms;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: VertexDegreeCalculator2
//
/// <summary>
/// Calculates the in-degree, out-degree, and degree for each of the graph's
/// vertices.
/// </summary>
///
/// <remarks>
/// See <see cref="Algorithms.VertexDegreeCalculator" /> for details on how
/// degrees are calculated.
/// </remarks>
//*****************************************************************************

public class VertexDegreeCalculator2 : GraphMetricCalculatorBase2
{
    //*************************************************************************
    //  Constructor: VertexDegreeCalculator2()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="VertexDegreeCalculator2" /> class.
    /// </summary>
    //*************************************************************************

    public VertexDegreeCalculator2()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: TryCalculateGraphMetrics()
    //
    /// <summary>
    /// Attempts to calculate a set of one or more related metrics.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <param name="calculateGraphMetricsContext">
    /// Provides access to objects needed for calculating graph metrics.
    /// </param>
    ///
    /// <param name="graphMetricColumns">
    /// Where an array of GraphMetricColumn objects gets stored if true is
    /// returned, one for each related metric calculated by this method.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated, false if the user wants to
    /// cancel.
    /// </returns>
    ///
    /// <remarks>
    /// This method periodically checks BackgroundWorker.<see
    /// cref="BackgroundWorker.CancellationPending" />.  If true, the method
    /// immediately returns false.
    ///
    /// <para>
    /// It also periodically reports progress by calling the
    /// BackgroundWorker.<see
    /// cref="BackgroundWorker.ReportProgress(Int32, Object)" /> method.  The
    /// userState argument is a <see cref="GraphMetricProgress" /> object.
    /// </para>
    ///
    /// <para>
    /// Calculated metrics for hidden rows are ignored by the caller, because
    /// Excel misbehaves when values are written to hidden cells.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public override Boolean
    TryCalculateGraphMetrics
    (
        IGraph graph,
        CalculateGraphMetricsContext calculateGraphMetricsContext,
        out GraphMetricColumn [] graphMetricColumns
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(calculateGraphMetricsContext != null);
        AssertValid();

        graphMetricColumns = new GraphMetricColumn[0];

        GraphMetricUserSettings oGraphMetricUserSettings =
            calculateGraphMetricsContext.GraphMetricUserSettings;

        if (!oGraphMetricUserSettings.CalculateDegree &&
            !oGraphMetricUserSettings.CalculateInDegree &&
            !oGraphMetricUserSettings.CalculateOutDegree)
        {
            return (true);
        }

        IVertexCollection oVertices = graph.Vertices;
        Int32 iVertices = oVertices.Count;

        // The following lists correspond to vertex worksheet columns.

        List<GraphMetricValueWithID> oInDegreeGraphMetricValues =
            new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID> oOutDegreeGraphMetricValues =
            new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID> oDegreeGraphMetricValues =
            new List<GraphMetricValueWithID>();

        // Create a dictionary to keep track of vertex degrees.  The key is the
        // IVertex.ID and the value is a zero-based index into the above lists.

        Dictionary<Int32, Int32> oVertexIDDictionary =
            new Dictionary<Int32, Int32>();

        // Calculate the degrees for each vertex using the
        // VertexDegreeCalculator class in the Algorithms namespace, which
        // knows nothing about Excel.
        //
        // For simplicity, all degree metrics (in-degree, out-degree, and
        // degree) are calculated regardless of whether the graph is directed
        // or undirected.  After all metrics are calculated,
        // FilterGraphMetricColumns() filters out the metrics that don't apply
        // to the graph, based on its directedness.

        Dictionary<Int32, VertexDegrees> oVertexDegreeDictionary;

        if ( !( new Algorithms.VertexDegreeCalculator() ).
            TryCalculateGraphMetrics(graph,
                calculateGraphMetricsContext.BackgroundWorker,
                out oVertexDegreeDictionary) )
        {
            // The user cancelled.

            return (false);
        }

        Int32 iRowID;

        foreach (IVertex oVertex in oVertices)
        {
            if ( !TryGetRowID(oVertex, out iRowID) )
            {
                continue;
            }

            VertexDegrees oVertexDegrees;

            if ( !oVertexDegreeDictionary.TryGetValue(oVertex.ID,
                out oVertexDegrees) )
            {
                Debug.Assert(false);
            }

            oInDegreeGraphMetricValues.Add(new GraphMetricValueWithID(
                iRowID, oVertexDegrees.InDegree) );

            oOutDegreeGraphMetricValues.Add(new GraphMetricValueWithID(
                iRowID, oVertexDegrees.OutDegree) );

            oDegreeGraphMetricValues.Add(new GraphMetricValueWithID(
                iRowID, oVertexDegrees.Degree) );

            Debug.Assert(oInDegreeGraphMetricValues.Count ==
                oOutDegreeGraphMetricValues.Count);

            oVertexIDDictionary.Add(oVertex.ID,
                oInDegreeGraphMetricValues.Count - 1);
        }

        // The following lists correspond to edge worksheet columns.  There
        // is a set of three lists for each of the edge's two vertices.

        List<GraphMetricValueWithID> oVertex1InDegreeGraphMetricValues =
            new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID> oVertex1OutDegreeGraphMetricValues =
            new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID> oVertex1DegreeGraphMetricValues =
            new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID> oVertex2InDegreeGraphMetricValues =
            new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID> oVertex2OutDegreeGraphMetricValues =
            new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID> oVertex2DegreeGraphMetricValues =
            new List<GraphMetricValueWithID>();

        // Loop through the edges.

        foreach (IEdge oEdge in graph.Edges)
        {
            if ( !TryGetRowID(oEdge, out iRowID) )
            {
                continue;
            }

            IVertex [] oEdgeVertices = oEdge.Vertices;
            IVertex oVertex1 = oEdgeVertices[0];
            IVertex oVertex2 = oEdgeVertices[1];
            Int32 iIndex;

            if ( oVertexIDDictionary.TryGetValue(oVertex1.ID, out iIndex) )
            {
                oVertex1InDegreeGraphMetricValues.Add(
                    new GraphMetricValueWithID(
                        iRowID,
                        oInDegreeGraphMetricValues[iIndex].Value)
                        );

                oVertex1OutDegreeGraphMetricValues.Add(
                    new GraphMetricValueWithID(
                        iRowID,
                        oOutDegreeGraphMetricValues[iIndex].Value)
                        );

                oVertex1DegreeGraphMetricValues.Add(
                    new GraphMetricValueWithID(
                        iRowID,
                        oDegreeGraphMetricValues[iIndex].Value)
                        );
            }

            if ( oVertexIDDictionary.TryGetValue(oVertex2.ID, out iIndex) )
            {
                oVertex2InDegreeGraphMetricValues.Add(
                    new GraphMetricValueWithID(
                        iRowID,
                        oInDegreeGraphMetricValues[iIndex].Value)
                        );

                oVertex2OutDegreeGraphMetricValues.Add(
                    new GraphMetricValueWithID(
                        iRowID,
                        oOutDegreeGraphMetricValues[iIndex].Value)
                        );

                oVertex2DegreeGraphMetricValues.Add(
                    new GraphMetricValueWithID(
                        iRowID,
                        oDegreeGraphMetricValues[iIndex].Value)
                        );
            }
        }

        // Figure out which columns to add.

        graphMetricColumns = FilterGraphMetricColumns(graph,
            calculateGraphMetricsContext,
            oInDegreeGraphMetricValues, oOutDegreeGraphMetricValues,
            oDegreeGraphMetricValues, oVertex1InDegreeGraphMetricValues,
            oVertex1OutDegreeGraphMetricValues,
            oVertex1DegreeGraphMetricValues, oVertex2InDegreeGraphMetricValues,
            oVertex2OutDegreeGraphMetricValues, oVertex2DegreeGraphMetricValues
            );

        return (true);
    }

    //*************************************************************************
    //  Method: FilterGraphMetricColumns()
    //
    /// <summary>
    /// Determines which GraphMetricColumn objects to return to the caller.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <param name="oCalculateGraphMetricsContext">
    /// Provides access to objects needed for calculating graph metrics.
    /// </param>
    ///
    /// <param name="oInDegreeGraphMetricValues">
    /// List of GraphMetricValue objects for the in-degree column on the vertex
    /// worksheet.
    /// </param>
    ///
    /// <param name="oOutDegreeGraphMetricValues">
    /// List of GraphMetricValue objects for the out-degree column on the
    /// vertex worksheet.
    /// </param>
    ///
    /// <param name="oDegreeGraphMetricValues">
    /// List of GraphMetricValue objects for the degree column on the vertex
    /// worksheet.
    /// </param>
    ///
    /// <param name="oVertex1InDegreeGraphMetricValues">
    /// List of GraphMetricValue objects for the vertex 1 in-degree column on
    /// the edge worksheet.
    /// </param>
    ///
    /// <param name="oVertex1OutDegreeGraphMetricValues">
    /// List of GraphMetricValue objects for the vertex 1 out-degree column on
    /// the edge worksheet.
    /// </param>
    ///
    /// <param name="oVertex1DegreeGraphMetricValues">
    /// List of GraphMetricValue objects for the vertex 1 degree column on the
    /// edge worksheet.
    /// </param>
    ///
    /// <param name="oVertex2InDegreeGraphMetricValues">
    /// List of GraphMetricValue objects for the vertex 2 in-degree column on
    /// the edge worksheet.
    /// </param>
    ///
    /// <param name="oVertex2OutDegreeGraphMetricValues">
    /// List of GraphMetricValue objects for the vertex 2 out-degree column on
    /// the edge worksheet.
    /// </param>
    ///
    /// <param name="oVertex2DegreeGraphMetricValues">
    /// List of GraphMetricValue objects for the vertex 2 degree column on the
    /// edge worksheet.
    /// </param>
    ///
    /// <returns>
    /// An array of GraphMetricColumn objects.
    /// </returns>
    //*************************************************************************

    protected GraphMetricColumn []
    FilterGraphMetricColumns
    (
        IGraph oGraph,
        CalculateGraphMetricsContext oCalculateGraphMetricsContext,
        List<GraphMetricValueWithID> oInDegreeGraphMetricValues,
        List<GraphMetricValueWithID> oOutDegreeGraphMetricValues,
        List<GraphMetricValueWithID> oDegreeGraphMetricValues,
        List<GraphMetricValueWithID> oVertex1InDegreeGraphMetricValues,
        List<GraphMetricValueWithID> oVertex1OutDegreeGraphMetricValues,
        List<GraphMetricValueWithID> oVertex1DegreeGraphMetricValues,
        List<GraphMetricValueWithID> oVertex2InDegreeGraphMetricValues,
        List<GraphMetricValueWithID> oVertex2OutDegreeGraphMetricValues,
        List<GraphMetricValueWithID> oVertex2DegreeGraphMetricValues
    )
    {
        AssertValid();

        Debug.Assert(oGraph != null);
        Debug.Assert(oCalculateGraphMetricsContext != null);
        Debug.Assert(oInDegreeGraphMetricValues != null);
        Debug.Assert(oOutDegreeGraphMetricValues != null);
        Debug.Assert(oDegreeGraphMetricValues != null);
        Debug.Assert(oVertex1InDegreeGraphMetricValues != null);
        Debug.Assert(oVertex1OutDegreeGraphMetricValues != null);
        Debug.Assert(oVertex1DegreeGraphMetricValues != null);
        Debug.Assert(oVertex2InDegreeGraphMetricValues != null);
        Debug.Assert(oVertex2OutDegreeGraphMetricValues != null);
        Debug.Assert(oVertex2DegreeGraphMetricValues != null);

        GraphMetricUserSettings oGraphMetricUserSettings =
            oCalculateGraphMetricsContext.GraphMetricUserSettings;

        Boolean bGraphIsDirected =
            (oGraph.Directedness == GraphDirectedness.Directed);

        Boolean bCalculateInDegree = bGraphIsDirected &&
            oGraphMetricUserSettings.CalculateInDegree;

        Boolean bCalculateOutDegree = bGraphIsDirected &&
            oGraphMetricUserSettings.CalculateOutDegree;

        Boolean bCalculateDegree = !bGraphIsDirected &&
            oGraphMetricUserSettings.CalculateDegree;

        String sStyle = null;

        if (oCalculateGraphMetricsContext.DuplicateEdgeDetector.
            GraphContainsDuplicateEdges)
        {
            // The calculations include duplicate edges, which may not be
            // expected by the user.  Warn her with Excel's "bad" pre-defined
            // style.

            sStyle = GraphMetricColumn.ExcelStyleBad;
        }

        // Figure out which columns to add.

        List<GraphMetricColumn> oGraphMetricColumns =
            new List<GraphMetricColumn>();

        if (bCalculateInDegree)
        {
            oGraphMetricColumns.Add( new GraphMetricColumnWithID(
                WorksheetNames.Vertices, TableNames.Vertices,
                VertexTableColumnNames.InDegree,
                VertexTableColumnWidths.InDegree,
                NumericFormat, sStyle,
                oInDegreeGraphMetricValues.ToArray() ) );
        }

        if (bCalculateOutDegree)
        {
            oGraphMetricColumns.Add( new GraphMetricColumnWithID(
                WorksheetNames.Vertices, TableNames.Vertices,
                VertexTableColumnNames.OutDegree,
                ExcelUtil.AutoColumnWidth,
                NumericFormat, sStyle,
                oOutDegreeGraphMetricValues.ToArray() ) );
        }

        if (bCalculateDegree)
        {
            oGraphMetricColumns.Add( new GraphMetricColumnWithID(
                WorksheetNames.Vertices, TableNames.Vertices,
                VertexTableColumnNames.Degree,
                ExcelUtil.AutoColumnWidth,
                NumericFormat, sStyle,
                oDegreeGraphMetricValues.ToArray() ) );
        }

        if (bCalculateInDegree)
        {
            oGraphMetricColumns.Add( new GraphMetricColumnWithID(
                WorksheetNames.Edges, TableNames.Edges,
                EdgeTableColumnNames.Vertex1InDegree,
                ExcelUtil.AutoColumnWidth,
                NumericFormat, sStyle,
                oVertex1InDegreeGraphMetricValues.ToArray() ) );
        }

        if (bCalculateOutDegree)
        {
            oGraphMetricColumns.Add( new GraphMetricColumnWithID(
                WorksheetNames.Edges, TableNames.Edges,
                EdgeTableColumnNames.Vertex1OutDegree,
                ExcelUtil.AutoColumnWidth,
                NumericFormat, sStyle,
                oVertex1OutDegreeGraphMetricValues.ToArray() ) );
        }

        if (bCalculateDegree)
        {
            oGraphMetricColumns.Add( new GraphMetricColumnWithID(
                WorksheetNames.Edges, TableNames.Edges,
                EdgeTableColumnNames.Vertex1Degree,
                ExcelUtil.AutoColumnWidth,
                NumericFormat, sStyle,
                oVertex1DegreeGraphMetricValues.ToArray() ) );
        }

        if (bCalculateInDegree)
        {
            oGraphMetricColumns.Add( new GraphMetricColumnWithID(
                WorksheetNames.Edges, TableNames.Edges,
                EdgeTableColumnNames.Vertex2InDegree,
                ExcelUtil.AutoColumnWidth,
                NumericFormat, sStyle,
                oVertex2InDegreeGraphMetricValues.ToArray() ) );
        }

        if (bCalculateOutDegree)
        {

            oGraphMetricColumns.Add( new GraphMetricColumnWithID(
                WorksheetNames.Edges, TableNames.Edges,
                EdgeTableColumnNames.Vertex2OutDegree,
                ExcelUtil.AutoColumnWidth,
                NumericFormat, sStyle,
                oVertex2OutDegreeGraphMetricValues.ToArray() ) );
        }

        if (bCalculateDegree)
        {
            oGraphMetricColumns.Add( new GraphMetricColumnWithID(
                WorksheetNames.Edges, TableNames.Edges,
                EdgeTableColumnNames.Vertex2Degree,
                ExcelUtil.AutoColumnWidth,
                NumericFormat, sStyle,
                oVertex2DegreeGraphMetricValues.ToArray() ) );
        }

        return ( oGraphMetricColumns.ToArray() );
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

    /// Number format for all columns.

    protected const String NumericFormat = "0";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
