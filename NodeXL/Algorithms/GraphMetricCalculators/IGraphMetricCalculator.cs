
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.Algorithms
{
//*****************************************************************************
//  Interface: IGraphMetricCalculator
//
/// <summary>
/// Supports the calculation of one set of graph metrics.
/// </summary>
///
/// <remarks>
/// There is a family of classes that calculate metrics for a graph.  Each
/// class calculates a set of one or more related metrics, and each class
/// implements this interface.
/// </remarks>
//*****************************************************************************

public interface IGraphMetricCalculator
{
    //*************************************************************************
    //  Property: GraphMetricDescription
    //
    /// <summary>
    /// Gets a description of the graph metrics calculated by the
    /// implementation.
    /// </summary>
    ///
    /// <value>
    /// A description suitable for use within the sentence "Calculating
    /// [GraphMetricDescription]."
    /// </value>
    //*************************************************************************

    String
    GraphMetricDescription
    {
        get;
    }
}

}
