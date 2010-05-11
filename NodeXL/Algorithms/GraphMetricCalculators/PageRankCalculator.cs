
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;

namespace Microsoft.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: PageRankCalculator
//
/// <summary>
/// Calculates the PageRanks for each of the graph's vertices.
/// </summary>
///
/// <remarks>
/// The PageRanks are provided as a Dictionary&lt;Int32, Double&gt;.  There is
/// one key/value pair for each vertex in the graph.  The key is the IVertex.ID
/// and the value is the vertex's PageRank, as a Double.
///
/// <para>
/// If a vertex is isolated, its PageRank is zero.
/// </para>
///
/// <para>
/// The PageRanks are calculated using the SNAP graph library.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class PageRankCalculator : OneSnapGraphMetricCalculatorBase
{
    //*************************************************************************
    //  Constructor: PageRankCalculator()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="PageRankCalculator" />
    /// class.
    /// </summary>
    //*************************************************************************

    public PageRankCalculator()
    :
    base(SnapGraphMetrics.PageRank, "PageRanks", "Vertex ID\tPageRank")
    {
        // (Do nothing.)

        AssertValid();
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
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
