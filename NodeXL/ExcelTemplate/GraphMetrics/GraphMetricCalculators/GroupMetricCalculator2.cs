
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Algorithms;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GroupMetricCalculator2
//
/// <summary>
/// Calculates overall metrics for each of the groups on the group worksheet.
/// </summary>
///
/// <remarks>
/// The overall metrics for each group are calculated as if the group's
/// vertices and the edges connecting those vertices were the only vertices and
/// edges in the graph.
/// </remarks>
//*****************************************************************************

public class GroupMetricCalculator2 : GraphMetricCalculatorBase2
{
    //*************************************************************************
    //  Constructor: GroupMetricCalculator2()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupMetricCalculator2" />
    /// class.
    /// </summary>
    //*************************************************************************

    public GroupMetricCalculator2()
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
    /// true if the graph metrics were calculated or don't need to be
    /// calculated, false if the user wants to cancel.
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

        Object oGroupInformationAsObject;

        // Attempt to retrieve the group information the WorkbookReader object
        // may have stored as metadata on the graph.

        if (
            !calculateGraphMetricsContext.GraphMetricUserSettings.
                ShouldCalculateGraphMetrics(GraphMetrics.GroupMetrics)
            ||
            !graph.TryGetValue(ReservedMetadataKeys.GroupInformation,
                typeof( ICollection<GroupInformation> ),
                out oGroupInformationAsObject)
            )
        {
            return (true);
        }

        // The following lists correspond to group worksheet columns.

        List<GraphMetricValueWithID> oVerticesGraphMetricValues =
            new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID> oUniqueEdgesGraphMetricValues =
            new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID> oEdgesWithDuplicatesGraphMetricValues =
            new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID> oTotalEdgesGraphMetricValues =
            new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID> oSelfLoopsGraphMetricValues =
            new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID> oConnectedComponentsGraphMetricValues =
            new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID>
            oSingleVertexConnectedComponentsGraphMetricValues =
                new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID>
            oMaximumConnectedComponentVerticesGraphMetricValues =
                new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID>
            oMaximumConnectedComponentEdgesGraphMetricValues =
                new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID> oMaximumGeodesicDistanceGraphMetricValues =
            new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID> oAverageGeodesicDistanceGraphMetricValues =
            new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID> oGraphDensityGraphMetricValues =
            new List<GraphMetricValueWithID>();

        foreach (GroupInformation oGroupInformation in
            ( ICollection<GroupInformation> )oGroupInformationAsObject)
        {
            ICollection<IVertex> oVertices = oGroupInformation.Vertices;

            if (oVertices == null || oVertices.Count == 0 ||
                !oGroupInformation.RowID.HasValue)
            {
                continue;
            }

            OverallMetrics oOverallMetrics;

            if ( !TryCalculateGraphMetricsForOneGroup(oGroupInformation,
                calculateGraphMetricsContext, out oOverallMetrics) )
            {
                // The user cancelled.

                return (false);
            }

            Int32 iRowID = oGroupInformation.RowID.Value;

            oVerticesGraphMetricValues.Add( new GraphMetricValueWithID(
                iRowID, oOverallMetrics.Vertices) );

            oUniqueEdgesGraphMetricValues.Add( new GraphMetricValueWithID(
                iRowID, oOverallMetrics.UniqueEdges) );

            oEdgesWithDuplicatesGraphMetricValues.Add(
                new GraphMetricValueWithID(iRowID,
                    oOverallMetrics.EdgesWithDuplicates) );

            oTotalEdgesGraphMetricValues.Add(new GraphMetricValueWithID(iRowID,
                oOverallMetrics.TotalEdges) );

            oSelfLoopsGraphMetricValues.Add(new GraphMetricValueWithID(iRowID,
                oOverallMetrics.SelfLoops) );

            oConnectedComponentsGraphMetricValues.Add(
                new GraphMetricValueWithID(iRowID,
                    oOverallMetrics.ConnectedComponents) );

            oSingleVertexConnectedComponentsGraphMetricValues.Add(
                new GraphMetricValueWithID(iRowID,
                    oOverallMetrics.SingleVertexConnectedComponents) );

            oMaximumConnectedComponentVerticesGraphMetricValues.Add(
                new GraphMetricValueWithID(iRowID,
                    oOverallMetrics.MaximumConnectedComponentVertices) );

            oMaximumConnectedComponentEdgesGraphMetricValues.Add(
                new GraphMetricValueWithID(iRowID,
                    oOverallMetrics.MaximumConnectedComponentEdges) );

            oMaximumGeodesicDistanceGraphMetricValues.Add(
                new GraphMetricValueWithID(iRowID,
                    OverallMetricCalculator2.NullableToGraphMetricValue<Int32>(
                        oOverallMetrics.MaximumGeodesicDistance) ) );

            oAverageGeodesicDistanceGraphMetricValues.Add(
                new GraphMetricValueWithID( iRowID,
                    OverallMetricCalculator2.NullableToGraphMetricValue<Double>(
                        oOverallMetrics.AverageGeodesicDistance) ) );

            oGraphDensityGraphMetricValues.Add(
                new GraphMetricValueWithID( iRowID,
                    OverallMetricCalculator2.NullableToGraphMetricValue<Double>(
                        oOverallMetrics.GraphDensity) ) );
        }

        graphMetricColumns = new GraphMetricColumn [] {

            new GraphMetricColumnWithID( WorksheetNames.Groups,
                TableNames.Groups, GroupTableColumnNames.Vertices,
                ExcelUtil.AutoColumnWidth, Int32NumericFormat,
                CellStyleNames.GraphMetricGood,
                oVerticesGraphMetricValues.ToArray() ),

            new GraphMetricColumnWithID( WorksheetNames.Groups,
                TableNames.Groups, GroupTableColumnNames.UniqueEdges,
                ExcelUtil.AutoColumnWidth, Int32NumericFormat,
                CellStyleNames.GraphMetricGood,
                oUniqueEdgesGraphMetricValues.ToArray() ),

            new GraphMetricColumnWithID( WorksheetNames.Groups,
                TableNames.Groups, GroupTableColumnNames.EdgesWithDuplicates,
                ExcelUtil.AutoColumnWidth, Int32NumericFormat,
                CellStyleNames.GraphMetricGood,
                oEdgesWithDuplicatesGraphMetricValues.ToArray() ),

            new GraphMetricColumnWithID( WorksheetNames.Groups,
                TableNames.Groups, GroupTableColumnNames.TotalEdges,
                ExcelUtil.AutoColumnWidth, Int32NumericFormat,
                CellStyleNames.GraphMetricGood,
                oTotalEdgesGraphMetricValues.ToArray() ),

            new GraphMetricColumnWithID( WorksheetNames.Groups,
                TableNames.Groups, GroupTableColumnNames.SelfLoops,
                ExcelUtil.AutoColumnWidth, Int32NumericFormat,
                CellStyleNames.GraphMetricGood,
                oSelfLoopsGraphMetricValues.ToArray() ),

            new GraphMetricColumnWithID( WorksheetNames.Groups,
                TableNames.Groups, GroupTableColumnNames.ConnectedComponents,
                ExcelUtil.AutoColumnWidth, Int32NumericFormat,
                CellStyleNames.GraphMetricGood,
                oConnectedComponentsGraphMetricValues.ToArray() ),

            new GraphMetricColumnWithID( WorksheetNames.Groups,
                TableNames.Groups,
                GroupTableColumnNames.SingleVertexConnectedComponents,
                ExcelUtil.AutoColumnWidth, Int32NumericFormat,
                CellStyleNames.GraphMetricGood,
                oSingleVertexConnectedComponentsGraphMetricValues.ToArray() ),

            new GraphMetricColumnWithID( WorksheetNames.Groups,
                TableNames.Groups,
                GroupTableColumnNames.MaximumConnectedComponentVertices,
                ExcelUtil.AutoColumnWidth, Int32NumericFormat,
                CellStyleNames.GraphMetricGood,
                oMaximumConnectedComponentVerticesGraphMetricValues.ToArray()
                ),

            new GraphMetricColumnWithID( WorksheetNames.Groups,
                TableNames.Groups,
                GroupTableColumnNames.MaximumConnectedComponentEdges,
                ExcelUtil.AutoColumnWidth, Int32NumericFormat,
                CellStyleNames.GraphMetricGood,
                oMaximumConnectedComponentEdgesGraphMetricValues.ToArray()
                ),

            new GraphMetricColumnWithID( WorksheetNames.Groups,
                TableNames.Groups,
                GroupTableColumnNames.MaximumGeodesicDistance,
                ExcelUtil.AutoColumnWidth, Int32NumericFormat,
                CellStyleNames.GraphMetricGood,
                oMaximumGeodesicDistanceGraphMetricValues.ToArray() ),

            new GraphMetricColumnWithID( WorksheetNames.Groups,
                TableNames.Groups,
                GroupTableColumnNames.AverageGeodesicDistance,
                ExcelUtil.AutoColumnWidth, DoubleNumericFormat,
                CellStyleNames.GraphMetricGood,
                oAverageGeodesicDistanceGraphMetricValues.ToArray() ),

            new GraphMetricColumnWithID( WorksheetNames.Groups,
                TableNames.Groups, GroupTableColumnNames.GraphDensity,
                ExcelUtil.AutoColumnWidth, DoubleNumericFormat,
                CellStyleNames.GraphMetricGood,
                oGraphDensityGraphMetricValues.ToArray() ),
            };

        return (true);
    }

    //*************************************************************************
    //  Method: TryCalculateGraphMetricsForOneGroup()
    //
    /// <summary>
    /// Attempts to calculate the metrics for one group.
    /// </summary>
    ///
    /// <param name="oGroupInformation">
    /// Contains information about the group.  The group must have at least
    /// one vertex.
    /// </param>
    ///
    /// <param name="oCalculateGraphMetricsContext">
    /// Provides access to objects needed for calculating graph metrics.
    /// </param>
    ///
    /// <param name="oOverallMetrics">
    /// Where the graph metrics get stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated, false if the user wants to
    /// cancel.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryCalculateGraphMetricsForOneGroup
    (
        GroupInformation oGroupInformation,
        CalculateGraphMetricsContext oCalculateGraphMetricsContext,
        out OverallMetrics oOverallMetrics
    )
    {
        Debug.Assert(oGroupInformation != null);
        Debug.Assert(oGroupInformation.Vertices != null);
        Debug.Assert(oGroupInformation.Vertices.Count > 0);
        Debug.Assert(oCalculateGraphMetricsContext != null);
        AssertValid();

        oOverallMetrics = null;

        ICollection<IVertex> oVertices = oGroupInformation.Vertices;

        // Create a new graph from the vertices in the group and the edges that
        // connect them.

        IGraph oNewGraph = SubgraphCalculator.GetSubgraphAsNewGraph(oVertices);

        // Calculate the overall metrics for the new graph using the
        // OverallMetricCalculator class in the Algorithms namespace, which
        // knows nothing about Excel.

        return ( (new Algorithms.OverallMetricCalculator() ).
            TryCalculateGraphMetrics(oNewGraph,
                oCalculateGraphMetricsContext.BackgroundWorker,
                out oOverallMetrics) );
    }

    //*************************************************************************
    //  Property: HandlesDuplicateEdges
    //
    /// <summary>
    /// Gets a flag indicating whether duplicate edges are properly handled.
    /// </summary>
    ///
    /// <value>
    /// true if the graph metric calculator handles duplicate edges, false if
    /// duplicate edges should be removed from the graph before the
    /// calculator's <see cref="TryCalculateGraphMetrics" /> method is called.
    /// </value>
    //*************************************************************************

    public override Boolean
    HandlesDuplicateEdges
    {
        get
        {
            AssertValid();

            // Duplicate edges get counted by this class, so they should be
            // included in the graph.

            return (true);
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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Number format for Int32 columns.

    protected const String Int32NumericFormat = "0";

    /// Number format for Double columns.

    protected const String DoubleNumericFormat = "0.000";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
