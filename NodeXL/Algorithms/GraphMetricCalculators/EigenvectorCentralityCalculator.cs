
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;

namespace Microsoft.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: EigenvectorCentralityCalculator
//
/// <summary>
/// Calculates the eigenvector centrality for each of the graph's vertices.
/// </summary>
///
/// <remarks>
/// The eigenvector centralities are provided as a
/// Dictionary&lt;Int32, Double&gt;.  There is one key/value pair for each
/// vertex in the graph.  The key is the IVertex.ID and the value is the
/// vertex's eigenvector centrality, as a Double.
///
/// <para>
/// If a vertex is isolated, its eigenvector centrality is zero.
/// </para>
///
/// <para>
/// Eigenvector centrality is defined in this article:
/// </para>
///
/// <para>
/// http://en.wikipedia.org/wiki/Eigenvector_centrality#Eigenvector_centrality
/// </para>
///
/// <para>
/// The eigenvector centralities are calculated using the SNAP graph library.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class EigenvectorCentralityCalculator :
    OneSnapGraphMetricCalculatorBase
{
    //*************************************************************************
    //  Constructor: EigenvectorCentralityCalculator()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="EigenvectorCentralityCalculator" /> class.
    /// </summary>
    //*************************************************************************

    public EigenvectorCentralityCalculator()
    :
    base(SnapGraphMetrics.EigenvectorCentrality, "eigenvector centralities",
        "Vertex ID\tEigenvector Centrality")
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
