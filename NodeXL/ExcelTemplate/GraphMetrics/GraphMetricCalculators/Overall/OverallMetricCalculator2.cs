
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Algorithms;
using Microsoft.NodeXL.Common;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: OverallMetricCalculator2
//
/// <summary>
/// Calculates the overall metrics for the graph.
/// </summary>
///
/// <remarks>
/// See <see cref="Algorithms.OverallMetricCalculator" /> for details on how
/// overall metrics are calculated.
/// </remarks>
//*****************************************************************************

public class OverallMetricCalculator2 : GraphMetricCalculatorBase2
{
    //*************************************************************************
    //  Constructor: OverallMetricCalculator2()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="OverallMetricCalculator2" /> class.
    /// </summary>
    //*************************************************************************

    public OverallMetricCalculator2()
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

        // Note regarding cell styles:
        //
        // Versions of NodeXL earlier than 1.0.1.130 didn't merge duplicate
        // edges before calculating graph metrics, and as a result some metrics
        // were invalid.  That was indicated by applying the CellStyleNames.Bad
        // Excel style to the invalid cells.  Starting in version 1.0.1.130
        // there are no longer invalid metrics, but the
        // CellStyleNames.GraphMetricGood style is always applied to those old
        // potentially bad metric cells (instead of null, which uses the
        // current cell style) in case graph metrics are calculated on an old
        // workbook that had bad metric cells.  If null were used, the old Bad
        // style would always remain on the previously bad cells, even if are
        // now filled with good metric values.

        graphMetricColumns = new GraphMetricColumn[0];

        if ( !calculateGraphMetricsContext.GraphMetricUserSettings.
            ShouldCalculateGraphMetrics(GraphMetrics.OverallMetrics) )
        {
            return (true);
        }

        // Calculate the overall metrics using the OverallMetricCalculator
        // class in the Algorithms namespace, which knows nothing about Excel.

        OverallMetrics oOverallMetrics;

        if ( !( new Algorithms.OverallMetricCalculator() ).
            TryCalculateGraphMetrics(graph,
                calculateGraphMetricsContext.BackgroundWorker,
                out oOverallMetrics) )
        {
            // The user cancelled.

            return (false);
        }

        OverallMetricRows oOverallMetricRows = new OverallMetricRows();


        //*********************************
        // Graph type
        //*********************************

        AddRow("Graph Type", oOverallMetrics.Directedness.ToString(),
            oOverallMetricRows);


        //*********************************
        // Vertex count
        //*********************************

        AddRow(oOverallMetricRows);
        AddRow("Vertices", oOverallMetrics.Vertices, oOverallMetricRows);


        //*********************************
        // Edge counts
        //*********************************

        AddRow(oOverallMetricRows);

        AddRow("Unique Edges", oOverallMetrics.UniqueEdges, oOverallMetricRows);

        AddRow("Edges With Duplicates", oOverallMetrics.EdgesWithDuplicates,
            oOverallMetricRows);

        AddRow("Total Edges", oOverallMetrics.TotalEdges, oOverallMetricRows);


        //*********************************
        // Self-loops
        //*********************************

        AddRow(oOverallMetricRows);

        AddRow("Self-Loops", oOverallMetrics.SelfLoops, oOverallMetricRows);


        //*********************************
        // Connected component counts
        //*********************************

        AddRow(oOverallMetricRows);

        AddRow("Connected Components", oOverallMetrics.ConnectedComponents,
            oOverallMetricRows);

        AddRow("Single-Vertex Connected Components",
            oOverallMetrics.SingleVertexConnectedComponents,
            oOverallMetricRows);

        AddRow("Maximum Vertices in a Connected Component",
            oOverallMetrics.MaximumConnectedComponentVertices,
            oOverallMetricRows);

        AddRow("Maximum Edges in a Connected Component",
            oOverallMetrics.MaximumConnectedComponentEdges,
            oOverallMetricRows);


        //*********************************
        // Geodesic distances
        //*********************************

        AddRow(oOverallMetricRows);

        AddRow("Maximum Geodesic Distance (Diameter)",

            NullableToGraphMetricValue<Int32>(
                oOverallMetrics.MaximumGeodesicDistance),

            oOverallMetricRows);

        AddRow("Average Geodesic Distance",

            NullableToGraphMetricValue<Double>(
                oOverallMetrics.AverageGeodesicDistance),

            oOverallMetricRows);


        //*********************************
        // Graph density
        //*********************************

        AddRow(oOverallMetricRows);

        AddRow("Graph Density",
            NullableToGraphMetricValue<Double>(oOverallMetrics.GraphDensity),
            oOverallMetricRows);


        //*********************************
        // NodeXL version
        //*********************************

        AddRow(oOverallMetricRows);

        AddRow("NodeXL Version", AssemblyUtil2.GetFileVersion(),
            oOverallMetricRows);


        graphMetricColumns = new GraphMetricColumn[] {

            CreateGraphMetricColumnOrdered(
                OverallMetricsTableColumnNames.Name,
                oOverallMetricRows.MetricNames),

            CreateGraphMetricColumnOrdered(
                OverallMetricsTableColumnNames.Value,
                oOverallMetricRows.MetricValues),
            };

        return (true);
    }

    //*************************************************************************
    //  Method: NullableToGraphMetricValue()
    //
    /// <summary>
    /// Converts a Nullable to a graph metric value suitable for inserting into
    /// a cell.
    /// </summary>
    ///
    /// <param name="nullable">
    /// The Nullable to convert.
    /// </param>
    ///
    /// <returns>
    /// If the Nullable has a value, the value is returned.  Otherwise, a "not
    /// applicable" string is returned.
    /// </returns>
    //*************************************************************************

    public static Object
    NullableToGraphMetricValue<T>
    (
        Nullable<T> nullable
    )
    where T : struct
    {
        if (nullable.HasValue)
        {
            return (nullable.Value);
        }

        return (NotApplicableMessage);
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
    //  Method: AddRow()
    //
    /// <overloads>
    /// Adds a row to the overall metrics table.
    /// </overloads>
    ///
    /// <summary>
    /// Adds an empty row to the overall metrics table.
    /// </summary>
    ///
    /// <param name="oOverallMetricRows">
    /// Contains the row data for the overall metrics table.
    /// </param>
    //*************************************************************************

    protected void
    AddRow
    (
        OverallMetricRows oOverallMetricRows
    )
    {
        Debug.Assert(oOverallMetricRows != null);
        AssertValid();

        AddRow(String.Empty, String.Empty,
            CellStyleNames.GraphMetricSeparatorRow, oOverallMetricRows);
    }

    //*************************************************************************
    //  Method: AddRow()
    //
    /// <summary>
    /// Adds a row with a default style to the overall metrics table.
    /// </summary>
    ///
    /// <param name="sMetricName">
    /// Name of the metric.  Can be empty but not null.
    /// </param>
    ///
    /// <param name="oMetricValue">
    /// Value of the metric.
    /// </param>
    ///
    /// <param name="oOverallMetricRows">
    /// Contains the row data for the overall metrics table.
    /// </param>
    //*************************************************************************

    protected void
    AddRow
    (
        String sMetricName,
        Object oMetricValue,
        OverallMetricRows oOverallMetricRows
    )
    {
        Debug.Assert(sMetricName != null);
        Debug.Assert(oMetricValue != null);
        Debug.Assert(oOverallMetricRows != null);
        AssertValid();

        AddRow(sMetricName, oMetricValue, CellStyleNames.GraphMetricGood,
            oOverallMetricRows);
    }

    //*************************************************************************
    //  Method: AddRow()
    //
    /// <summary>
    /// Adds a row with a specified style to the overall metrics table.
    /// </summary>
    ///
    /// <param name="sMetricName">
    /// Name of the metric.  Can be empty but not null.
    /// </param>
    ///
    /// <param name="oMetricValue">
    /// Value of the metric.
    /// </param>
    ///
    /// <param name="sStyle">
    /// Style of the row, or null to not apply a style.
    /// </param>
    ///
    /// <param name="oOverallMetricRows">
    /// Contains the row data for the overall metrics table.
    /// </param>
    //*************************************************************************

    protected void
    AddRow
    (
        String sMetricName,
        Object oMetricValue,
        String sStyle,
        OverallMetricRows oOverallMetricRows
    )
    {
        Debug.Assert(sMetricName != null);
        Debug.Assert(oMetricValue != null);
        Debug.Assert(oOverallMetricRows != null);
        AssertValid();

        oOverallMetricRows.MetricNames.Add( new GraphMetricValueOrdered(
            sMetricName, sStyle) );

        oOverallMetricRows.MetricValues.Add( new GraphMetricValueOrdered(
            oMetricValue, sStyle) );
    }

    //*************************************************************************
    //  Method: CreateGraphMetricColumnOrdered()
    //
    /// <summary>
    /// Creates a GraphMetricColumnOrdered object for one of the columns in the
    /// overall metrics table.
    /// </summary>
    ///
    /// <param name="sColumnName">
    /// Name of the column.
    /// </param>
    ///
    /// <param name="oValues">
    /// Column cell values.
    /// </param>
    ///
    /// <returns>
    /// A GraphMetricColumnOrdered object for the specified column.
    /// </returns>
    //*************************************************************************

    protected GraphMetricColumnOrdered
    CreateGraphMetricColumnOrdered
    (
        String sColumnName,
        List<GraphMetricValueOrdered> oValues
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sColumnName) );
        Debug.Assert(oValues != null);
        AssertValid();

        return ( new GraphMetricColumnOrdered(WorksheetNames.OverallMetrics,
            TableNames.OverallMetrics, sColumnName, ExcelUtil.AutoColumnWidth,
            null, CellStyleNames.GraphMetricGood, oValues.ToArray() ) );
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

    /// Not applicable message.

    const String NotApplicableMessage = "Not Applicable";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)


    //*************************************************************************
    //  Embedded class: OverallMetricRows
    //
    /// <summary>
    /// Contains the row data for the overall metrics table.
    /// </summary>
    //*************************************************************************

    public class OverallMetricRows : Object
    {
        //*********************************************************************
        //  Constructor: OverallMetricRows()
        //
        /// <summary>
        /// Initializes a new instance of the <see cref="OverallMetricRows" />
        /// class.
        /// </summary>
        //*********************************************************************

        public OverallMetricRows()
        {
            m_oRowInformation = new List<GraphMetricValueOrdered> [2] {
                new List<GraphMetricValueOrdered>(),
                new List<GraphMetricValueOrdered>(),
                };

            AssertValid();
        }

        //*********************************************************************
        //  Property: MetricNames
        //
        /// <summary>
        /// Gets the List of metric names.
        /// </summary>
        ///
        /// <value>
        /// The List of metric names.
        /// </value>
        //*********************************************************************

        public List<GraphMetricValueOrdered>
        MetricNames
        {
            get
            {
                AssertValid();

                return ( m_oRowInformation[0] );
            }
        }

        //*********************************************************************
        //  Property: MetricValues
        //
        /// <summary>
        /// Gets the List of metric values.
        /// </summary>
        ///
        /// <value>
        /// The List of metric values.
        /// </value>
        //*********************************************************************

        public List<GraphMetricValueOrdered>
        MetricValues
        {
            get
            {
                AssertValid();

                return ( m_oRowInformation[1] );
            }
        }


        //*********************************************************************
        //  Method: AssertValid()
        //
        /// <summary>
        /// Asserts if the object is in an invalid state.  Debug-only.
        /// </summary>
        //*********************************************************************

        [Conditional("DEBUG")]

        public void
        AssertValid()
        {
            Debug.Assert(m_oRowInformation != null);
            Debug.Assert(m_oRowInformation.Length == 2);
        }


        //*********************************************************************
        //  Protected fields
        //*********************************************************************

        /// 2 Lists, one for each column in the overall metrics table.

        protected List<GraphMetricValueOrdered> [] m_oRowInformation;
    }

}

}
