
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

        if (!calculateGraphMetricsContext.GraphMetricUserSettings.
            CalculateOverallMetrics)
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

        String sDuplicateEdgeStyle = CellStyleNames.GraphMetricGood;
        String sDuplicateEdgeComments = String.Empty;
        String sGraphDensityComments = String.Empty;

        if (oOverallMetrics.EdgesWithDuplicates > 0)
        {
            // The graph density is rendered invalid when the graph has
            // duplicate edges.

            sDuplicateEdgeStyle = CellStyleNames.GraphMetricBad;

            sDuplicateEdgeComments =
                "You can merge duplicate edges using NodeXL, Data, Prepare"
                + " Data, Merge Duplicate Edges."
                ;

            sGraphDensityComments =
                "The workbook contains duplicate edges that have caused the"
                + " graph density to be inaccurate.  "
                + sDuplicateEdgeComments
                ;
        }

        AddRow(oOverallMetricRows);
        AddRow("Unique Edges", oOverallMetrics.UniqueEdges, oOverallMetricRows);

        AddRow("Edges With Duplicates",
            FormatInt32(oOverallMetrics.EdgesWithDuplicates),
            sDuplicateEdgeComments, sDuplicateEdgeStyle,
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

        String sMaximumGeodesicDistance, sAverageGeodesicDistance;

        GetGeodesicDistanceStrings(oOverallMetrics,
            out sMaximumGeodesicDistance, out sAverageGeodesicDistance);

        AddRow(oOverallMetricRows);

        AddRow("Maximum Geodesic Distance (Diameter)",
            sMaximumGeodesicDistance, oOverallMetricRows);

        AddRow("Average Geodesic Distance", sAverageGeodesicDistance,
            oOverallMetricRows);


        //*********************************
        // Graph density
        //*********************************

        Nullable<Double> dGraphDensity = oOverallMetrics.GraphDensity;

        AddRow(oOverallMetricRows);

        AddRow("Graph Density", dGraphDensity.HasValue ?
            FormatDouble(dGraphDensity.Value) : NotApplicableMessage,
            sGraphDensityComments, sDuplicateEdgeStyle, oOverallMetricRows);


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

            CreateGraphMetricColumnOrdered(
                OverallMetricsTableColumnNames.Comments,
                oOverallMetricRows.MetricComments),
            };

        return (true);
    }

    //*************************************************************************
    //  Method: GetGeodesicDistanceStrings()
    //
    /// <summary>
    /// Gets strings that describe the geodesic distances.
    /// </summary>
    ///
    /// <param name="oOverallMetrics">
    /// Contains the graph's overall metrics.
    /// </param>
    ///
    /// <param name="sMaximumGeodesicDistance">
    /// Where a string describing the maximum geodesic distance gets stored.
    /// </param>
    ///
    /// <param name="sAverageGeodesicDistance">
    /// Where a string describing the average geodesic distance gets stored.
    /// </param>
    //*************************************************************************

    protected void
    GetGeodesicDistanceStrings
    (
        OverallMetrics oOverallMetrics,
        out String sMaximumGeodesicDistance,
        out String sAverageGeodesicDistance
    )
    {
        Debug.Assert(oOverallMetrics != null);
        AssertValid();

        if (oOverallMetrics.MaximumGeodesicDistance.HasValue)
        {
            sMaximumGeodesicDistance = FormatInt32(
                oOverallMetrics.MaximumGeodesicDistance.Value);

            // The maximum and average are computed together.

            Debug.Assert(oOverallMetrics.AverageGeodesicDistance.HasValue);

            sAverageGeodesicDistance = FormatDouble(
                oOverallMetrics.AverageGeodesicDistance.Value);
        }
        else
        {
            sMaximumGeodesicDistance = sAverageGeodesicDistance =
                NotApplicableMessage;
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

        AddRow(String.Empty, String.Empty, String.Empty,
            CellStyleNames.GraphMetricSeparatorRow, oOverallMetricRows);
    }

    //*************************************************************************
    //  Method: AddRow()
    //
    /// <summary>
    /// Adds an Int32 row to the overall metrics table.
    /// </summary>
    ///
    /// <param name="sMetricName">
    /// Name of the metric.  Can be empty but not null.
    /// </param>
    ///
    /// <param name="iMetricValue">
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
        Int32 iMetricValue,
        OverallMetricRows oOverallMetricRows
    )
    {
        Debug.Assert(sMetricName != null);
        Debug.Assert(oOverallMetricRows != null);
        AssertValid();

        AddRow(sMetricName, FormatInt32(iMetricValue), String.Empty, null,
            oOverallMetricRows);
    }

    //*************************************************************************
    //  Method: AddRow()
    //
    /// <summary>
    /// Adds a row to the overall metrics table.
    /// </summary>
    ///
    /// <param name="sMetricName">
    /// Name of the metric.  Can be empty but not null.
    /// </param>
    ///
    /// <param name="sMetricValue">
    /// Value of the metric.  Can be empty but not null.
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
        String sMetricValue,
        OverallMetricRows oOverallMetricRows
    )
    {
        Debug.Assert(sMetricName != null);
        Debug.Assert(sMetricValue != null);
        Debug.Assert(oOverallMetricRows != null);
        AssertValid();

        AddRow(sMetricName, sMetricValue, String.Empty, null,
            oOverallMetricRows);
    }

    //*************************************************************************
    //  Method: AddRow()
    //
    /// <summary>
    /// Adds a row with comments and a style to the overall metrics table.
    /// </summary>
    ///
    /// <param name="sMetricName">
    /// Name of the metric.  Can be empty but not null.
    /// </param>
    ///
    /// <param name="sMetricValue">
    /// Value of the metric.  Can be empty but not null.
    /// </param>
    ///
    /// <param name="sMetricComments">
    /// Comments for the metric.  Can be empty but not null.
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
        String sMetricValue,
        String sMetricComments,
        String sStyle,
        OverallMetricRows oOverallMetricRows
    )
    {
        Debug.Assert(sMetricName != null);
        Debug.Assert(sMetricValue != null);
        Debug.Assert(sMetricComments != null);
        Debug.Assert(oOverallMetricRows != null);
        AssertValid();

        oOverallMetricRows.MetricNames.Add( new GraphMetricValueOrdered(
            sMetricName, sStyle) );

        oOverallMetricRows.MetricValues.Add( new GraphMetricValueOrdered(
            sMetricValue, sStyle) );

        oOverallMetricRows.MetricComments.Add( new GraphMetricValueOrdered(
            sMetricComments, sStyle) );
    }

    //*************************************************************************
    //  Method: FormatInt32()
    //
    /// <summary>
    /// Formats an Int32 for use in the metric value column
    /// </summary>
    ///
    /// <param name="iInt32">
    /// Int32 to format.
    /// </param>
    ///
    /// <returns>
    /// Formatted Int32.
    /// </returns>
    //*************************************************************************

    protected String
    FormatInt32
    (
        Int32 iInt32
    )
    {
        AssertValid();

        return ( iInt32.ToString(ExcelTemplateForm.Int32Format) );
    }

    //*************************************************************************
    //  Method: FormatDouble()
    //
    /// <summary>
    /// Formats a Double for use in the metric value column
    /// </summary>
    ///
    /// <param name="dDouble">
    /// Double to format.
    /// </param>
    ///
    /// <returns>
    /// Formatted Double.
    /// </returns>
    //*************************************************************************

    protected String
    FormatDouble
    (
        Double dDouble
    )
    {
        AssertValid();

        return ( dDouble.ToString("N2") );
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
            m_oRowInformation = new List<GraphMetricValueOrdered> [3] {
                new List<GraphMetricValueOrdered>(),
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
        //  Property: MetricComments
        //
        /// <summary>
        /// Gets the List of metric comments.
        /// </summary>
        ///
        /// <value>
        /// The List of metric comments.
        /// </value>
        //*********************************************************************

        public List<GraphMetricValueOrdered>
        MetricComments
        {
            get
            {
                AssertValid();

                return ( m_oRowInformation[2] );
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
            Debug.Assert(m_oRowInformation.Length == 3);
        }


        //*********************************************************************
        //  Protected fields
        //*********************************************************************

        /// 3 Lists, one for each column in the overall metrics table.

        protected List<GraphMetricValueOrdered> [] m_oRowInformation;
    }

}

}
