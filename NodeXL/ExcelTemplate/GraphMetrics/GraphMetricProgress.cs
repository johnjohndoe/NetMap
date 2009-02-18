using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphMetricProgress
//
/// <summary>
/// Used by the <see cref="IGraphMetricCalculator2" /> implementations to
/// report progress to the main thread.
/// </summary>
///
/// <remarks>
/// An <see cref="IGraphMetricCalculator2" /> implementation should
/// periodically report progress to the main thread by passing a <see
/// cref="GraphMetricProgress" /> object as the userState argument to the
/// BackgroundWorker.<see
/// cref="System.ComponentModel.BackgroundWorker.ReportProgress(Int32, Object)"
/// /> method.
/// </remarks>
//*****************************************************************************

public class GraphMetricProgress : Object
{
    //*************************************************************************
    //  Constructor: GraphMetricProgress()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphMetricProgress" />
    /// class with specified values.
    /// </summary>
    ///
    /// <param name="progressMessage">
    /// Progress message suitable for display in the UI.
    /// </param>
    ///
    /// <param name="allGraphMetricsCalculated">
    /// true if all <see cref="IGraphMetricCalculator2" /> implementations have
    /// calculated their metrics.
    /// </param>
    //*************************************************************************

    public GraphMetricProgress
    (
        String progressMessage,
        Boolean allGraphMetricsCalculated
    )
    {
        m_sProgressMessage = progressMessage;
        m_bAllGraphMetricsCalculated = allGraphMetricsCalculated;

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: GraphMetricProgress()
    //
    /// <summary>
    /// Do not use this constructor.
    /// </summary>
    //*************************************************************************

    private GraphMetricProgress()
    {
        // (Do nothing.)
    }

    //*************************************************************************
    //  Property: ProgressMessage
    //
    /// <summary>
    /// Gets or sets the progress message suitable for display in the UI.
    /// </summary>
    ///
    /// <value>
    /// The progress message suitable for display in the UI.
    /// </value>
    //*************************************************************************

    public String
    ProgressMessage
    {
        get
        {
            AssertValid();

            return (m_sProgressMessage);
        }

        set
        {
            m_sProgressMessage = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: AllGraphMetricsCalculated
    //
    /// <summary>
    /// Gets or sets a flag indicating whether all <see
    /// cref="IGraphMetricCalculator2" /> implementations have calculated their
    /// metrics.
    /// </summary>
    ///
    /// <value>
    /// true if all <see cref="IGraphMetricCalculator2" /> implementations have
    /// calculated their metrics.
    /// </value>
    //*************************************************************************

    public Boolean
    AllGraphMetricsCalculated
    {
        get
        {
            AssertValid();

            return (m_bAllGraphMetricsCalculated);
        }

        set
        {
            m_bAllGraphMetricsCalculated = value;

            AssertValid();
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
        Debug.Assert( !String.IsNullOrEmpty(m_sProgressMessage) );
        // m_bAllGraphMetricsCalculated
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The progress message suitable for display in the UI.

    protected String m_sProgressMessage;

    /// true if all IGraphMetricCalculator2 implementations have calculated
    /// their metrics.

    protected Boolean m_bAllGraphMetricsCalculated;
}

}
