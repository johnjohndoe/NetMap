
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Algorithms;

namespace Microsoft.NodeXL.Layouts
{
//*****************************************************************************
//  Class: GraphBinner
//
/// <summary>
/// Lays out a graph's smaller components in bins.
/// </summary>
//*****************************************************************************

public class GraphBinner : Object
{
    //*************************************************************************
    //  Constructor: GraphBinner()
    //
    /// <summary>
    /// Initializes a new instance of the GraphBinner class.
    /// </summary>
    //*************************************************************************

    public GraphBinner()
    {
        m_iMaximumVerticesPerBin = 3;
        m_iBinLength = 16;

        AssertValid();
    }

    //*************************************************************************
    //  Property: MaximumVerticesPerBin
    //
    /// <summary>
    /// Gets or sets the maximum number of vertices a binned component can
    /// have.
    /// </summary>
    ///
    /// <value>
    /// The maximum number of vertices a binned component can have.  The
    /// default value is 3.
    /// </value>
    ///
    /// <remarks>
    /// If a strongly connected component of the graph has <see
    /// cref="MaximumVerticesPerBin" /> vertices or fewer, the component is
    /// placed in a bin.
    /// </remarks>
    //*************************************************************************

    public Int32
    MaximumVerticesPerBin
    {
        get
        {
            AssertValid();

            return (m_iMaximumVerticesPerBin);
        }

        set
        {
            m_iMaximumVerticesPerBin = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: BinLength
    //
    /// <summary>
    /// Gets or sets the height and width of each bin, in graph rectangle
    /// units.
    /// </summary>
    ///
    /// <value>
    /// The height and width of each bin, in graph rectangle units.  The
    /// default value is 16.
    /// </value>
    //*************************************************************************

    public Int32
    BinLength
    {
        get
        {
            AssertValid();

            return (m_iBinLength);
        }

        set
        {
            m_iBinLength = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: LayOutSmallerComponentsInBins()
    //
    /// <summary>
    /// Lays out a graph's smaller components in bins.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to lay out.  The graph must have at least one vertex.
    /// </param>
    ///
    /// <param name="verticesToLayOut">
    /// Vertices to lay out.  The collection must have at least one vertex.
    /// </param>
    ///
    /// <param name="layoutContext">
    /// Provides access to objects needed to lay out the graph.  The <see
    /// cref="LayoutContext.GraphRectangle" /> must have non-zero width and
    /// height.
    /// </param>
    ///
    /// <param name="remainingVertices">
    /// Where the vertices that have not been binned get stored if true is
    /// returned.
    /// </param>
    ///
    /// <param name="remainingRectangle">
    /// Where the remaining rectangle gets stored if true is returned.
    /// </param>
    ///
    /// <remarks>
    /// This method splits <paramref name="verticesToLayOut" /> into strongly
    /// connected components, synchronously lays out each of the smaller
    /// components using <see cref="FruchtermanReingoldLayout" />, and places
    /// the components along the bottom of the rectangle.  If there are any
    /// vertices remaining and any space remaining, they get stored at
    /// <paramref name="remainingVertices" /> and <paramref
    /// name="remainingRectangle" /> and true is returned.  Otherwise, false is
    /// returned.
    ///
    /// <para>
    /// If true is returned, the caller should lay out the remaining vertices
    /// in the remaining rectangle.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Boolean
    LayOutSmallerComponentsInBins
    (
        IGraph graph,
        ICollection<IVertex> verticesToLayOut,
        LayoutContext layoutContext,
        out ICollection<IVertex> remainingVertices,
        out Rectangle remainingRectangle
    )
    {
        AssertValid();

        remainingVertices = null;
        remainingRectangle = Rectangle.Empty;

        // This method modifies some of the graph's metadata.  Save the
        // original metadata.

        Boolean bOriginalGraphHasBeenLaidOut =
            LayoutMetadataUtil.GraphHasBeenLaidOut(graph);

        ICollection<IVertex> oOriginalLayOutTheseVerticesOnly =
            ( ICollection<IVertex> )graph.GetValue(
                ReservedMetadataKeys.LayOutTheseVerticesOnly,
                typeof( ICollection<IVertex> ) );

        // Split the vertices into strongly connected components, sorted in
        // increasing order of vertex count.

        List< LinkedList<IVertex> > oComponents =
            ConnectedComponentCalculator.GetStronglyConnectedComponents(
                verticesToLayOut, graph);

        Int32 iComponents = oComponents.Count;

        // This object will split the graph rectangle into bin rectangles.

        RectangleBinner oRectangleBinner = new RectangleBinner(
            layoutContext.GraphRectangle, m_iBinLength);

        Int32 iComponent = 0;

        for (iComponent = 0; iComponent < iComponents; iComponent++)
        {
            LinkedList<IVertex> oComponent = oComponents[iComponent];
            Int32 iVerticesInComponent = oComponent.Count;

            if (iVerticesInComponent> m_iMaximumVerticesPerBin)
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

            LayOutComponentInBin(graph, oComponent, oBinRectangle);
        }

        // Restore the original metadata on the graph.

        if (bOriginalGraphHasBeenLaidOut)
        {
            LayoutMetadataUtil.MarkGraphAsLaidOut(graph);
        }
        else
        {
            LayoutMetadataUtil.MarkGraphAsNotLaidOut(graph);
        }

        if (oOriginalLayOutTheseVerticesOnly != null)
        {
            graph.SetValue(ReservedMetadataKeys.LayOutTheseVerticesOnly,
                oOriginalLayOutTheseVerticesOnly);
        }
        else
        {
            graph.RemoveKey(ReservedMetadataKeys.LayOutTheseVerticesOnly);
        }

        if ( oRectangleBinner.TryGetRemainingRectangle(
            out remainingRectangle) )
        {
            remainingVertices = GetRemainingVertices(oComponents, iComponent);

            return (remainingVertices.Count > 0);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: LayOutComponentInBin()
    //
    /// <summary>
    /// Lays out a graph component in a bin.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// Graph being laid out.
    /// </param>
    ///
    /// <param name="oVerticesInComponent">
    /// The vertices in the bin.
    /// </param>
    ///
    /// <param name="oBinRectangle">
    /// The bin rectangle to lay out the vertices within.
    /// </param>
    //*************************************************************************

    protected void
    LayOutComponentInBin
    (
        IGraph oGraph,
        ICollection<IVertex> oVerticesInComponent,
        Rectangle oBinRectangle
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oVerticesInComponent != null);
        AssertValid();

        oGraph.SetValue(ReservedMetadataKeys.LayOutTheseVerticesOnly,
            oVerticesInComponent);

        // Force the FruchtermanReingoldLayout class to randomize the vertices.

        LayoutMetadataUtil.MarkGraphAsNotLaidOut(oGraph);

        ILayout oLayout = new FruchtermanReingoldLayout();
        oLayout.Margin = BinMargin;
        LayoutContext oLayoutContext = new LayoutContext(oBinRectangle);
        oLayout.LayOutGraph(oGraph, oLayoutContext);
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
    /// A collection of remaining vertices that have not been binned.
    /// </returns>
    //*************************************************************************

    protected ICollection<IVertex>
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

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        Debug.Assert(m_iMaximumVerticesPerBin >= 1);
        Debug.Assert(m_iBinLength >= 1);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Margin within each bin.

    protected const Int32 BinMargin = 4;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The maximum number of vertices a binned component can have.

    protected Int32 m_iMaximumVerticesPerBin;

    /// Height and width of each bin, in graph rectangle units.

    protected Int32 m_iBinLength;
}

}
