
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Algorithms;

namespace Microsoft.NodeXL.Layouts
{
//*****************************************************************************
//  Class: BinnedLayout
//
/// <summary>
/// Lays out a graph in "bins."
/// </summary>
///
/// <remarks>
/// This layout splits the graph into strongly connected components, lays out
/// the smaller components and places them along the bottom of the rectangle,
/// and combines and lays out the remaining components within the remaining
/// space.
///
/// <para>
/// If the graph has a metadata key of <see
/// cref="ReservedMetadataKeys.LayOutTheseVerticesOnly" />, only the vertices
/// specified in the value's IVertex array are laid out and all other vertices
/// are completely ignored.
/// </para>
///
/// <para>
/// If a vertex has a metadata key of <see
/// cref="ReservedMetadataKeys.LockVertexLocation" /> with a value of true, it
/// is included in layout calculations but its own location is left unmodified.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class BinnedLayout : AsyncLayoutBase
{
    //*************************************************************************
    //  Constructor: BinnedLayout()
    //
    /// <summary>
    /// Initializes a new instance of the BinnedLayout class.
    /// </summary>
    //*************************************************************************

    public BinnedLayout()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: LayOutGraphCore()
    //
    /// <summary>
    /// Lays out a graph synchronously or asynchronously.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to lay out.  The graph is guaranteed to have at least one vertex.
    /// </param>
    ///
    /// <param name="layoutContext">
    /// Provides access to objects needed to lay out the graph.  The <see
    /// cref="LayoutContext.GraphRectangle" /> is guaranteed to have non-zero
    /// width and height.
    /// </param>
    ///
    /// <param name="backgroundWorker">
    /// <see cref="BackgroundWorker" /> whose worker thread called this method
    /// if the graph is being laid out asynchronously, or null if the graph is
    /// being laid out synchronously.
    /// </param>
    ///
    /// <returns>
    /// true if the layout was successfully completed, false if the layout was
    /// cancelled.  The layout can be cancelled only if the graph is being laid
    /// out asynchronously.
    /// </returns>
    ///
    /// <remarks>
    /// This method lays out the graph <paramref name="graph" /> either
    /// synchronously (if <paramref name="backgroundWorker" /> is null) or
    /// asynchronously (if (<paramref name="backgroundWorker" /> is not null)
    /// by setting the the <see cref="IVertex.Location" /> property on all of
    /// the graph's vertices and optionally adding geometry metadata to the
    /// graph, vertices, or edges.
    ///
    /// <para>
    /// In the asynchronous case, the <see
    /// cref="BackgroundWorker.CancellationPending" /> property on the
    /// <paramref name="backgroundWorker" /> object should be checked before
    /// each layout iteration.  If it's true, the method should immediately
    /// return false.  Also, <see
    /// cref="AsyncLayoutBase.FireLayOutGraphIterationCompleted()" /> should be
    /// called after each iteration.
    /// </para>
    ///
    /// <para>
    /// The arguments have already been checked for validity.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected override Boolean
    LayOutGraphCore
    (
        IGraph graph,
        LayoutContext layoutContext,
        BackgroundWorker backgroundWorker
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(layoutContext != null);
        AssertValid();

        if (backgroundWorker != null && backgroundWorker.CancellationPending)
        {
            return (false);
        }

        // Honor the optional LayOutTheseVerticesOnly key on the graph.

        ICollection oVerticesToLayOut = GetVerticesToLayOut(graph);

        // Although the caller has guaranteed that there is at least one vertex
        // in the graph, the collection returned by GetVerticesToLayOut() may
        // be empty.

        if (oVerticesToLayOut.Count == 0)
        {
            return (true);
        }

        Boolean bGraphHasBeenLaidOut = GraphHasBeenLaidOut(graph);

        // First split the vertices into strongly connected components, sorted
        // in increasing order of vertex count.

        List< LinkedList<IVertex> > oComponents =
            ConnectedComponentCalculator.GetStronglyConnectedComponents(
                oVerticesToLayOut);

        Int32 iComponents = oComponents.Count;

        // This object will split the graph rectangle into bin rectangles.

        RectangleBinner oRectangleBinner = new RectangleBinner(
            layoutContext.GraphRectangle, BinLength);

        Int32 iComponent = 0;

        for (iComponent = 0; iComponent < iComponents; iComponent++)
        {
            LinkedList<IVertex> oComponent = oComponents[iComponent];
            Int32 iVerticesInComponent = oComponent.Count;

            if (iVerticesInComponent> MaximumVerticesPerBin)
            {
                // The vertices in the remaining components should not be
                // binned.

                break;
            }

            Rectangle oBinRectangle;

            if ( !oRectangleBinner.TryGetNextBin(out oBinRectangle) )
            {
                // There is no room for an additional bin rectangle.

                break;
            }

            // Lay out the component within the bin rectangle.

            IVertex [] aoVerticesInBin = new IVertex[iVerticesInComponent];
            oComponent.CopyTo(aoVerticesInBin, 0);

            LayOutTheseVerticesOnly(graph, aoVerticesInBin, oBinRectangle,
                !bGraphHasBeenLaidOut);
        }

        // Lay out the remaining unbinned vertices within the remaining space.

        Rectangle oRemainingRectangle;

        if ( oRectangleBinner.TryGetRemainingRectangle(
            out oRemainingRectangle) )
        {
            LayOutTheseVerticesOnly(graph, 
                GetRemainingVertices(oComponents, iComponent),
                oRemainingRectangle, !bGraphHasBeenLaidOut);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: LayOutTheseVerticesOnly()
    //
    /// <summary>
    /// Lays out a specified set of vertices within a specified rectangle.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// Graph being laid out.
    /// </param>
    ///
    /// <param name="aoVertices">
    /// The vertices to lay out.
    /// </param>
    ///
    /// <param name="oRectangle">
    /// The rectangle to lay out the vertices within.
    /// </param>
    ///
    /// <param name="bMarkGraphAsNotLaidOut">
    /// If true, the graph is marked as having not been laid out before the
    /// vertices are laid out.
    /// </param>
    //*************************************************************************

    protected void
    LayOutTheseVerticesOnly
    (
        IGraph oGraph,
        IVertex [] aoVertices,
        Rectangle oRectangle,
        Boolean bMarkGraphAsNotLaidOut
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(aoVertices != null);
        AssertValid();

        if (bMarkGraphAsNotLaidOut)
        {
            // If the graph is marked as laid out, the FruchtermanReingold
            // class will not randomize the vertices before laying them out.
            // This will likely result in vertices falling on top of each
            // other.

            MarkGraphAsNotLaidOut(oGraph);
        }

        IVertex [] oOriginalLayOutTheseVerticesOnly =
            ( IVertex [] )oGraph.GetValue(
                ReservedMetadataKeys.LayOutTheseVerticesOnly,
                typeof( IVertex [] ) );

        oGraph.SetValue(ReservedMetadataKeys.LayOutTheseVerticesOnly,
            aoVertices);

        ILayout oLayout = new FruchtermanReingoldLayout();
        oLayout.Margin = BinMargin;
        LayoutContext oLayoutContext = new LayoutContext(oRectangle);
        oLayout.LayOutGraph(oGraph, oLayoutContext);

        // Clean up.

        if (oOriginalLayOutTheseVerticesOnly != null)
        {
            oGraph.SetValue(ReservedMetadataKeys.LayOutTheseVerticesOnly,
                oOriginalLayOutTheseVerticesOnly);
        }
        else
        {
            oGraph.RemoveKey(ReservedMetadataKeys.LayOutTheseVerticesOnly);
        }
    }

    //*************************************************************************
    //  Method: GetRemainingVertices()
    //
    /// <summary>
    /// Copies the remaining vertices into an array.
    /// </summary>
    ///
    /// <param name="oComponents">
    /// The graph's strongly connected components.
    /// </param>
    ///
    /// <param name="iFirstRemainingComponent">
    /// Index of the first remaining component in <paramref
    /// name="oComponents" />.
    /// </param>
    ///
    /// <returns>
    /// An array of remaining vertices that have not been binned.
    /// </returns>
    //*************************************************************************

    protected IVertex []
    GetRemainingVertices
    (
        List< LinkedList<IVertex> > oComponents,
        Int32 iFirstRemainingComponent
    )
    {
        Debug.Assert(oComponents != null);
        Debug.Assert(iFirstRemainingComponent >= 0);
        AssertValid();

        Int32 iComponents = oComponents.Count;
        Int32 iRemainingVertices = 0;

        for (Int32 iRemainingComponent = iFirstRemainingComponent;
            iRemainingComponent < iComponents; iRemainingComponent++)
        {
            iRemainingVertices += oComponents[iRemainingComponent].Count;
        }

        IVertex [] aoRemainingVertices = new IVertex[iRemainingVertices];
        Int32 iCopyIndex = 0;

        for (Int32 iRemainingComponent = iFirstRemainingComponent;
            iRemainingComponent < iComponents; iRemainingComponent++)
        {
            LinkedList<IVertex> oComponent = oComponents[iRemainingComponent];
            oComponent.CopyTo(aoRemainingVertices, iCopyIndex);
            iCopyIndex += oComponent.Count;
        }

        return (aoRemainingVertices);
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
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// If a strongly connected component of the graph has
    /// MaximumVerticesPerBin vertices or fewer, the component is placed in a
    /// bin.

    protected const Int32 MaximumVerticesPerBin = 3;

    /// Height and width of each bin, in graph rectangle units.

    protected const Int32 BinLength = 16;

    /// Margin within each bin.

    protected const Int32 BinMargin = 4;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
