
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Algorithms;
using Microsoft.NodeXL.Visualization.Wpf;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ClusterCalculator2
//
/// <summary>
/// Partitions a graph into clusters.
/// </summary>
///
/// <remarks>
/// See <see cref="Algorithms.ClusterCalculator" /> for details on how the
/// graph is partitioned into clusters.
/// </remarks>
//*****************************************************************************

public class ClusterCalculator2 : GraphMetricCalculatorBase2
{
    //*************************************************************************
    //  Constructor: ClusterCalculator2()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ClusterCalculator2" />
    /// class.
    /// </summary>
    //*************************************************************************

    public ClusterCalculator2()
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

        graphMetricColumns = null;

        // Partition the graph into clusters using the ClusterCalculator class
        // in the Algorithms namespace, which knows nothing about Excel.

        LinkedList<Community> oCommunities;

        if ( !( new Algorithms.ClusterCalculator() ).
            TryCalculateGraphMetrics(graph,
                calculateGraphMetricsContext.BackgroundWorker,
                out oCommunities) )
        {
            // The user cancelled.

            return (false);
        }

        // Convert the list of communities to an array of GraphMetricColumn
        // objects.

        graphMetricColumns = CommunitiesToGraphMetricColumns(oCommunities);

        return (true);
    }

    //*************************************************************************
    //  Method: CommunitiesToGraphMetricColumns()
    //
    /// <summary>
    /// Converts the final list of communities to an array of <see
    /// cref="GraphMetricColumn" /> objects.
    /// </summary>
    ///
    /// <param name="oCommunities">
    /// List of all communities.
    /// </param>
    ///
    /// <returns>
    /// An array of <see cref="GraphMetricColumn" /> objects.
    /// </returns>
    //*************************************************************************

    protected GraphMetricColumn []
    CommunitiesToGraphMetricColumns
    (
        LinkedList<Community> oCommunities
    )
    {
        Debug.Assert(oCommunities != null);
        AssertValid();

        // These columns are needed:
        //
        // 1. Cluster name on the cluster worksheet.
        //
        // 2. Vertex color on the cluster worksheet.
        //
        // 3. Vertex shape on the cluster worksheet.
        //
        // 4. Cluster name on the cluster-vertex worksheet.
        //
        // 5. Vertex name on the cluster-vertex worksheet.

        List<GraphMetricValueOrdered> oClusterNamesForClusterWorksheet =
            new List<GraphMetricValueOrdered>();

        List<GraphMetricValueOrdered> oVertexColorsForClusterWorksheet =
            new List<GraphMetricValueOrdered>();

        List<GraphMetricValueOrdered> oVertexShapesForClusterWorksheet =
            new List<GraphMetricValueOrdered>();

        List<GraphMetricValueOrdered> oClusterNamesForClusterVertexWorksheet =
            new List<GraphMetricValueOrdered>();

        List<GraphMetricValueOrdered> oVertexNamesForClusterVertexWorksheet =
            new List<GraphMetricValueOrdered>();

        Int32 iCommunities = oCommunities.Count;
        Int32 iCommunity = 0;

        ColorConverter2 oColorConverter2 = new ColorConverter2();

        VertexShapeConverter oVertexShapeConverter =
            new VertexShapeConverter();

        foreach (Community oCommunity in oCommunities)
        {
            String sClusterName = CommunityToClusterName(oCommunity);

            Color oColor;
            VertexShape eShape;

            GetVertexAttributes(iCommunity, iCommunities, out oColor,
                out eShape);

            GraphMetricValueOrdered oClusterNameGraphMetricValue =
                new GraphMetricValueOrdered(sClusterName);

            oClusterNamesForClusterWorksheet.Add(
                oClusterNameGraphMetricValue);

            // Write the color in a format that is understood by
            // ColorConverter2.WorkbookToGraph(), which is what
            // WorksheetReaderBase uses.

            oVertexColorsForClusterWorksheet.Add(
                new GraphMetricValueOrdered(
                    oColorConverter2.GraphToWorkbook(oColor) ) );

            oVertexShapesForClusterWorksheet.Add(
                new GraphMetricValueOrdered(
                    oVertexShapeConverter.GraphToWorkbook(eShape) ) );

            foreach (IVertex oVertex in oCommunity.Vertices)
            {
                oClusterNamesForClusterVertexWorksheet.Add(
                    oClusterNameGraphMetricValue);

                oVertexNamesForClusterVertexWorksheet.Add(
                    new GraphMetricValueOrdered(oVertex.Name) );
            }

            iCommunity++;
        }

        return ( new GraphMetricColumn [] {

            new GraphMetricColumnOrdered( WorksheetNames.Clusters,
                TableNames.Clusters,
                ClusterTableColumnNames.Name,
                ExcelUtil.AutoColumnWidth, null, null,
                oClusterNamesForClusterWorksheet.ToArray()
                ),

            new GraphMetricColumnOrdered( WorksheetNames.Clusters,
                TableNames.Clusters,
                ClusterTableColumnNames.VertexColor,
                ExcelUtil.AutoColumnWidth, null, null,
                oVertexColorsForClusterWorksheet.ToArray()
                ),

            new GraphMetricColumnOrdered( WorksheetNames.Clusters,
                TableNames.Clusters,
                ClusterTableColumnNames.VertexShape,
                ExcelUtil.AutoColumnWidth, null, null,
                oVertexShapesForClusterWorksheet.ToArray()
                ),

            new GraphMetricColumnOrdered( WorksheetNames.ClusterVertices,
                TableNames.ClusterVertices,
                ClusterVertexTableColumnNames.ClusterName,
                ExcelUtil.AutoColumnWidth, null, null,
                oClusterNamesForClusterVertexWorksheet.ToArray()
                ),

            new GraphMetricColumnOrdered( WorksheetNames.ClusterVertices,
                TableNames.ClusterVertices,
                ClusterVertexTableColumnNames.VertexName,
                ExcelUtil.AutoColumnWidth, null, null,
                oVertexNamesForClusterVertexWorksheet.ToArray()
                ),
                } );
    }

    //*************************************************************************
    //  Method: CommunityToClusterName()
    //
    /// <summary>
    /// Gets a cluster name to use in the workbook.
    /// </summary>
    ///
    /// <param name="oCommunity">
    /// The community to get a cluster name for.
    /// </param>
    ///
    /// <returns>
    /// A cluster name to use in the workbook.
    /// </returns>
    //*************************************************************************

    protected String
    CommunityToClusterName
    (
        Community oCommunity
    )
    {
        Debug.Assert(oCommunity != null);
        AssertValid();

        return ( (oCommunity.ID).ToString(ExcelTemplateForm.Int32Format) );
    }

    //*************************************************************************
    //  Method: GetVertexAttributes()
    //
    /// <summary>
    /// Gets the vertex attributes for a specified community.
    /// </summary>
    ///
    /// <param name="iCommunity">
    /// Zero-based community index.
    /// </param>
    ///
    /// <param name="iTotalCommunities">
    /// Total number of communities.
    /// </param>
    ///
    /// <param name="oColor">
    /// Where the color gets stored.
    /// </param>
    ///
    /// <param name="eShape">
    /// Where the shape gets stored.
    /// </param>
    //*************************************************************************

    protected void
    GetVertexAttributes
    (
        Int32 iCommunity,
        Int32 iTotalCommunities,
        out Color oColor,
        out VertexShape eShape
    )
    {
        Debug.Assert(iCommunity >= 0);
        Debug.Assert(iTotalCommunities > 0);
        Debug.Assert(iCommunity < iTotalCommunities);

        // Algorithm:
        //
        // Cycle through the set of hues with one shape, then cycle through the
        // set of hues with the next shape, and so on.  When all shapes have
        // been used, repeat the process with another saturation.

        Int32 iHues = Hues.Length;
        Int32 iShapes = Shapes.Length;

        Int32 iSaturations = (Int32)Math.Ceiling(
            (Single)iTotalCommunities / (Single)(iHues * iShapes) );

        Int32 iHueIndex = iCommunity % iHues;
        Int32 iDividend = iCommunity / iHues;
        Int32 iShapeIndex = iDividend % iShapes;
        Int32 iSaturationIndex = iDividend / iShapes;

        Single fHue = Hues[iHueIndex];

        Single fSaturation;

        if (iSaturations == 1)
        {
            fSaturation = StartSaturation;
        }
        else
        {
            fSaturation = StartSaturation
                + ( (Single)iSaturationIndex /  (Single)(iSaturations - 1) )
                * (EndSaturation - StartSaturation);
        }

        oColor = ColorUtil.HsbToRgb(fHue, fSaturation, Brightness);
        eShape = Shapes[iShapeIndex];
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

    /// Hues to cycle through when creating vertex colors.  Hues range from
    /// 0 to 360.

    protected static Single [] Hues = new Single[] {
        240F,  // Red
        0F,    // Blue
        200F,  // Orange
        120F,  // Green
        300F,  // Magenta
        180F,  // Yellow
        60F,   // Cyan
        };

    /// Shapes to cycle through.

    protected static VertexShape [] Shapes = new VertexShape[] {

        VertexShape.Disk,
        VertexShape.SolidSquare,
        VertexShape.SolidDiamond,
        VertexShape.SolidTriangle,
        VertexShape.Sphere,
        // VertexShape.Circle,
        // VertexShape.Square,
        // VertexShape.Diamond,
        // VertexShape.Triangle,
        };

    /// Saturation to start with.  Saturation ranges from 0 to 1.0, where 0 is
    /// grayscale and 1.0 is the most saturated.

    protected const Single StartSaturation = 1.0F;

    /// Saturation to end with.

    protected const Single EndSaturation = 0.2F;

    /// Brightness to use.  Brightness ranges from 0 to 1.0, where 0 represents
    /// black and 1.0 represents white.

    protected const Single Brightness = 0.5F;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
