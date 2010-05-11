
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
}

}
