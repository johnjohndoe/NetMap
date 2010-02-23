
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Algorithms;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: CalculateGraphMetricsContext
//
/// <summary>
/// Provides access to objects needed for calculating graph metrics.
/// </summary>
///
/// <remarks>
/// An instance of this class gets passed to <see
/// cref="IGraphMetricCalculator2.TryCalculateGraphMetrics" />.
/// </remarks>
//*****************************************************************************

public class CalculateGraphMetricsContext : Object
{
    //*************************************************************************
    //  Constructor: CalculateGraphMetricsContext()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="CalculateGraphMetricsContext" /> class.
    /// </summary>
    ///
    /// <param name="graphMetricUserSettings">
    /// The user's settings for calculating graph metrics.
    /// </param>
    ///
    /// <param name="duplicateEdgeDetector">
    /// Object that counts duplicate and unique edges in the graph.
    /// </param>
    ///
    /// <param name="backgroundWorker">
    /// The BackgroundWorker object that is performing all graph metric
    /// calculations.
    /// </param>
    //*************************************************************************

    public CalculateGraphMetricsContext
    (
        GraphMetricUserSettings graphMetricUserSettings,
        DuplicateEdgeDetector duplicateEdgeDetector,
        BackgroundWorker backgroundWorker
    )
    {
        m_oGraphMetricUserSettings = graphMetricUserSettings;
        m_oDuplicateEdgeDetector = duplicateEdgeDetector;
        m_oBackgroundWorker = backgroundWorker;
        m_oSimpleGraphMatrix = null;
        m_iSimpleGraphMatrixGraphID = Int32.MinValue;
        m_oBrandesCentralities = null;

        AssertValid();
    }

    //*************************************************************************
    //  Property: GraphMetricUserSettings
    //
    /// <summary>
    /// Gets the user's settings for calculating graph metrics.
    /// </summary>
    ///
    /// <value>
    /// The user's settings for calculating graph metrics.
    /// </value>
    //*************************************************************************

    public GraphMetricUserSettings
    GraphMetricUserSettings
    {
        get
        {
            AssertValid();

            return (m_oGraphMetricUserSettings);
        }
    }

    //*************************************************************************
    //  Property: DuplicateEdgeDetector
    //
    /// <summary>
    /// Gets the object that counts duplicate and unique edges in the graph.
    /// </summary>
    ///
    /// <value>
    /// The object that counts duplicate and unique edges in the graph.
    /// </value>
    //*************************************************************************

    public DuplicateEdgeDetector
    DuplicateEdgeDetector
    {
        get
        {
            AssertValid();

            return (m_oDuplicateEdgeDetector);
        }
    }

    //*************************************************************************
    //  Property: BackgroundWorker
    //
    /// <summary>
    /// Gets the BackgroundWorker object that is performing all graph metric
    /// calculations.
    /// </summary>
    ///
    /// <value>
    /// The BackgroundWorker object that is performing all graph metric
    /// calculations.
    /// </value>
    //*************************************************************************

    public BackgroundWorker
    BackgroundWorker
    {
        get
        {
            AssertValid();

            return (m_oBackgroundWorker);
        }
    }

    //*************************************************************************
    //  Property: BrandesCentralities
    //
    /// <summary>
    /// Gets or sets the Brandes centralities.
    /// </summary>
    ///
    /// <value>
    /// The <see cref="BrandesCentralities" /> object created by <see
    /// cref="BrandesFastCentralityCalculator2" />, or null if the centralities
    /// haven't been calculated.  The default value is null.
    /// </value>
    ///
    /// <remarks>
    /// Most of the graph metric calculators have no need to pass calculated
    /// metrics to each other.  The exception is <see
    /// cref="BrandesFastCentralityCalculator2" />, which calculates geodesic
    /// distances that must be written to the overall metrics worksheet.  The
    /// class responsible for writing to the overall metrics worksheet is <see
    /// cref="OverallMetricCalculator2" />, not <see
    /// cref="BrandesFastCentralityCalculator2" />, so this property exists to
    /// allow those geodesic distances to be passed from one calculator to the
    /// other.
    /// </remarks>
    //*************************************************************************

    public BrandesCentralities
    BrandesCentralities
    {
        get
        {
            AssertValid();

            return (m_oBrandesCentralities);
        }

        set
        {
            m_oBrandesCentralities = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: GetSimpleGraphMatrix()
    //
    /// <summary>
    /// Gets an object that simulates a matrix that can be used to determine
    /// whether two vertices are connected by an edge.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph for which metrics are being calculated.
    /// </param>
    ///
    /// <returns>
    /// A <see cref="SimpleGraphMatrix" /> object for <paramref
    /// name="graph" />.
    /// </returns>
    ///
    /// <remarks>
    /// Creating a <see cref="SimpleGraphMatrix" /> object can be expensive, so
    /// this method creates it only on demand and then caches it for use by
    /// multiple graph metric calculators.
    /// </remarks>
    //*************************************************************************

    public SimpleGraphMatrix
    GetSimpleGraphMatrix
    (
        IGraph graph
    )
    {
        Debug.Assert(graph != null);
        AssertValid();

        if (m_oSimpleGraphMatrix == null ||
            m_iSimpleGraphMatrixGraphID != graph.ID)
        {
            m_oSimpleGraphMatrix = new SimpleGraphMatrix(graph);
            m_iSimpleGraphMatrixGraphID = graph.ID;
        }

        return (m_oSimpleGraphMatrix);
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        Debug.Assert(m_oGraphMetricUserSettings != null);
        Debug.Assert(m_oDuplicateEdgeDetector != null);
        Debug.Assert(m_oBackgroundWorker != null);

        Debug.Assert(m_iSimpleGraphMatrixGraphID == Int32.MinValue ||
            m_oSimpleGraphMatrix != null);

        // m_oBrandesCentralities
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The user's settings for calculating graph metrics.

    protected GraphMetricUserSettings m_oGraphMetricUserSettings;

    /// Object that counts duplicate and unique edges in the graph.

    protected DuplicateEdgeDetector m_oDuplicateEdgeDetector;

    /// Object that is performing all graph metric calculations.

    protected BackgroundWorker m_oBackgroundWorker;

    /// This gets created on demand by GetSimpleGraphMatrix().

    protected SimpleGraphMatrix m_oSimpleGraphMatrix;

    /// ID of the graph that m_oSimpleGraphMatrix was created for.

    protected Int32 m_iSimpleGraphMatrixGraphID;

    /// The BrandesCentralities object created by
    /// BrandesFastCentralityCalculator2, or null if the centralities haven't
    /// been calculated.

    protected BrandesCentralities m_oBrandesCentralities;
}

}
