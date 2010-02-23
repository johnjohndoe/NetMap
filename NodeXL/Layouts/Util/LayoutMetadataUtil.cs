
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.NodeXL.Core;
using System.Diagnostics;

namespace Microsoft.NodeXL.Layouts
{
//*****************************************************************************
//  Class: LayoutMetadataUtil
//
/// <summary>
/// Utility methods for dealing with a graph's layout metadata.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class LayoutMetadataUtil
{
    //*************************************************************************
    //  Method: MarkGraphAsLaidOut()
    //
    /// <summary>
    /// Marks a graph as having been laid out.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph that was laid out.
    /// </param>
    ///
    /// <remarks>
    /// This should be called after <paramref name="graph" /> has been
    /// successfully laid out.  It adds a metadata key to the graph.
    /// </remarks>
    //*************************************************************************

    public static void
    MarkGraphAsLaidOut
    (
        IGraph graph
    )
    {
        Debug.Assert(graph != null);

        graph.SetValue(ReservedMetadataKeys.LayoutBaseLayoutComplete, null);
    }

    //*************************************************************************
    //  Method: MarkGraphAsNotLaidOut()
    //
    /// <summary>
    /// Removes the metadata that indicates a graph has been laid out.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to remove the metadata from.
    /// </param>
    //*************************************************************************

    public static void
    MarkGraphAsNotLaidOut
    (
        IGraph graph
    )
    {
        Debug.Assert(graph != null);

        graph.RemoveKey(ReservedMetadataKeys.LayoutBaseLayoutComplete);
    }

    //*************************************************************************
    //  Method: GraphHasBeenLaidOut()
    //
    /// <summary>
    /// Gets a flag indicating whether a graph has been laid out.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to check.
    /// </param>
    ///
    /// <returns>
    /// true if the graph has been laid out.
    /// </returns>
    //*************************************************************************

    public static Boolean
    GraphHasBeenLaidOut
    (
        IGraph graph
    )
    {
        Debug.Assert(graph != null);

        return ( graph.ContainsKey(
            ReservedMetadataKeys.LayoutBaseLayoutComplete) );
    }
}

}
