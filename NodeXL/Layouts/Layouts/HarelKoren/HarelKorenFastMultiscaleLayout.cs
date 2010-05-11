
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.Layouts
{
//*****************************************************************************
//  Class: HarelKorenFastMultiscaleLayout
//
/// <summary>
/// Lays out a graph using the Harel-Koren fast multiscale algorithm.
/// </summary>
///
/// <remarks>
/// For details on the layout algorithm, see "A Fast Multi-Scale Method for
/// Drawing Large Graphs," David Harel and Yehuda Koren, Journal of Graph
/// Algorithms and Applications, Vol. 6 No. 3, 2002.
///
/// <para>
/// Most property names are identical to the names used in the Harel-Koren
/// paper.  The exception is Iterations, which is renamed <see
/// cref="LocalIterations" />.
/// </para>
///
/// <para>
/// If the graph has a metadata key of <see
/// cref="ReservedMetadataKeys.LayOutTheseVerticesOnly" />, only the vertices
/// specified in the value's IVertex collection are laid out and all other
/// vertices are completely ignored.
/// </para>
///
/// <para>
/// If a vertex has a metadata key of <see
/// cref="ReservedMetadataKeys.LockVertexLocation" /> with a value of true, its
/// location is left unmodified.
/// </para>
///
/// <para>
/// This class wraps a C# implementation written by Janez Brank at Microsoft
/// Research Cambridge in May 2009.  The code from Janez is in the file
/// HarelKorenFastMultiscaleLayoutInternal.cs.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class HarelKorenFastMultiscaleLayout : AsyncLayoutBase
{
    //*************************************************************************
    //  Constructor: HarelKorenFastMultiscaleLayout()
    //
    /// <summary>
    /// Initializes a new instance of the HarelKorenFastMultiscaleLayout class.
    /// </summary>
    //*************************************************************************

    public HarelKorenFastMultiscaleLayout()
    {
        m_iRad = 7;
        m_iLocalIterations = 10;
        m_iRatio = 3;
        m_iMinSize = 10;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Rad
    //
    /// <summary>
    /// Gets or sets the constant that determines the radius of local
    /// neighborhoods.
    /// </summary>
    ///
    /// <value>
    /// The "Rad" constant in the Harel-Koren paper.  Must be greater than 0.
    /// The default value is 7.
    /// </value>
    //*************************************************************************

    public Int32
    Rad
    {
        get
        {
            AssertValid();

            return (m_iRad);
        }

        set
        {
            const String PropertyName = "Rad";

            this.ArgumentChecker.CheckPropertyPositive(PropertyName, value);

            if (value == m_iRad)
            {
                return;
            }

            m_iRad = value;

            FireLayoutRequired();

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: LocalIterations
    //
    /// <summary>
    /// Gets or sets the constant that determines the number of iterations used
    /// for local beautification.
    /// </summary>
    ///
    /// <value>
    /// The "Iterations" constant in the Harel-Koren paper.  Must be greater
    /// than 0.  The default value is 10.
    /// </value>
    ///
    /// <remarks>
    /// This is named LocalIterations because "Iterations" might be confused
    /// with NodeXL's notion of an iteration, which in the Harel-Koren
    /// algorithm is the number of coarse graph iterations.  The number of
    /// coarse graph iterations is determined by the <see cref="Ratio" />
    /// property and the number of vertices in the graph.
    /// </remarks>
    //*************************************************************************

    public Int32
    LocalIterations
    {
        get
        {
            AssertValid();

            return (m_iLocalIterations);
        }

        set
        {
            const String PropertyName = "LocalIterations";

            this.ArgumentChecker.CheckPropertyPositive(PropertyName, value);

            if (value == m_iLocalIterations)
            {
                return;
            }

            m_iLocalIterations = value;

            FireLayoutRequired();

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Ratio
    //
    /// <summary>
    /// Gets or sets the ratio between the number of vertices in two coarse
    /// graph iterations.
    /// </summary>
    ///
    /// <value>
    /// The "Ratio" constant in the Harel-Koren paper.  Must be greater than 1.
    /// The default value is 3.
    /// </value>
    //*************************************************************************

    public Int32
    Ratio
    {
        get
        {
            AssertValid();

            return (m_iRatio);
        }

        set
        {
            const String PropertyName = "Ratio";

            this.ArgumentChecker.CheckPropertyPositive(PropertyName, value);

            if (value == m_iRatio)
            {
                return;
            }

            m_iRatio = value;

            FireLayoutRequired();

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: MinSize
    //
    /// <summary>
    /// Gets or sets the minimum number of clusters in the graph.
    /// </summary>
    ///
    /// <value>
    /// The "MinSize" constant in the Harel-Koren paper.  Must be greater than
    /// zero.  The default value is 10.
    /// </value>
    //*************************************************************************

    public Int32
    MinSize
    {
        get
        {
            AssertValid();

            return (m_iMinSize);
        }

        set
        {
            const String PropertyName = "MinSize";

            this.ArgumentChecker.CheckPropertyPositive(PropertyName, value);

            if (value == m_iMinSize)
            {
                return;
            }

            m_iMinSize = value;

            FireLayoutRequired();

            AssertValid();
        }
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
    /// <param name="verticesToLayOut">
    /// Vertices to lay out.  The collection is guaranteed to have at least one
    /// vertex.
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
    /// by setting the the <see cref="IVertex.Location" /> property on the
    /// vertices in <paramref name="verticesToLayOut" /> and optionally adding
    /// geometry metadata to the graph, vertices, or edges.
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
        ICollection<IVertex> verticesToLayOut,
        LayoutContext layoutContext,
        BackgroundWorker backgroundWorker
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(verticesToLayOut != null);
        Debug.Assert(verticesToLayOut.Count > 0);
        Debug.Assert(layoutContext != null);
        AssertValid();

        if (backgroundWorker != null && backgroundWorker.CancellationPending)
        {
            return (false);
        }

        ICollection<IEdge> oEdgesToLayOut =
            GetEdgesToLayOut(graph, verticesToLayOut);

        Int32 iVertices = verticesToLayOut.Count;

        // The MultiScaleLayout class uses a simple scheme where the graph's
        // vertices consist of the integers 0 through N-1, where N is the
        // number of vertices in the graph.  NodeXL uses IVertex objects and
        // the vertex collection isn't indexed.  Work around this
        // incompatibility by creating a dictionary that maps vertices to
        // zero-based vertex indexes.

        Dictionary<IVertex, Int32> oVertexDictionary =
            new Dictionary<IVertex, Int32>(iVertices);

        Int32 iVertexIndex = 0;

        foreach (IVertex oVertex in verticesToLayOut)
        {
            oVertexDictionary.Add(oVertex, iVertexIndex);
            iVertexIndex++;
        }

        // Create and populate a MultiScaleLayout graph.

        MultiScaleLayout.Graph oMultiScaleLayoutGraph =
            new MultiScaleLayout.Graph(iVertices, String.Empty);

        foreach (IEdge oEdge in oEdgesToLayOut)
        {
            IVertex [] aoEdgeVertices = oEdge.Vertices;

            oMultiScaleLayoutGraph.AddEdge(
                oVertexDictionary[ aoEdgeVertices[0] ],
                oVertexDictionary[ aoEdgeVertices[1] ]
                );
        }

        // Lay it out.

        oMultiScaleLayoutGraph.PrepareForUse();

        MultiScaleLayout.GraphLayoutSettings oGraphLayoutSettings =
            new MultiScaleLayout.GraphLayoutSettings(m_iRad,
                m_iLocalIterations, m_iRatio, m_iMinSize);

        oMultiScaleLayoutGraph.MultiScaleLayout(
            ( new Random() ).Next(), oGraphLayoutSettings);

        // Retrieve the laid out vertex coordinates, which are normalized to
        // fall within the range [0,1].

        MultiScaleLayout.PointD [] oMultiScaleLayoutLocations =
            oMultiScaleLayoutGraph.vertexCoords;

        Rectangle oRectangle = layoutContext.GraphRectangle;

        Debug.Assert(oRectangle.Width > 0);
        Debug.Assert(oRectangle.Height > 0);

        Int32 iLeft = oRectangle.Left;
        Int32 iTop = oRectangle.Top;
        Int32 iWidth = oRectangle.Width;
        Int32 iHeight = oRectangle.Height;

        foreach (IVertex oVertex in verticesToLayOut)
        {
            if ( !VertexIsLocked(oVertex) )
            {
                // Convert the normalized coordinates to coordinates within the
                // layout rectangle.

                MultiScaleLayout.PointD oMultiScaleLayoutLocation =
                    oMultiScaleLayoutLocations[ oVertexDictionary[oVertex] ];

                oVertex.Location = new PointF(
                    iLeft + (Single)(oMultiScaleLayoutLocation.x * iWidth),
                    iTop + (Single)(oMultiScaleLayoutLocation.y * iHeight)
                    );
            }
        }

        return (true);
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

        Debug.Assert(m_iRad > 0);
        Debug.Assert(m_iLocalIterations > 0);
        Debug.Assert(m_iRatio > 1);
        Debug.Assert(m_iMinSize > 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The "Rad" constant in the Harel-Koren paper.

    protected Int32 m_iRad;

    /// The "Iterations" constant in the Harel-Koren paper.

    protected Int32 m_iLocalIterations;

    /// The "Ratio" constant in the Harel-Koren paper.

    protected Int32 m_iRatio;

    /// The "MinSize" constant in the Harel-Koren paper.

    protected Int32 m_iMinSize;
}

}
