
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: DraggedVertices
//
/// <summary>
/// Represents one or more vertices that might be dragged with the mouse.
/// </summary>
///
/// <remarks>
/// Create an instance of this class when a vertex is clicked.  When the mouse
/// is moved, check <see cref="MouseDrag.OnMouseMove" /> to determine whether
/// the mouse has moved far enough to begin a vertex drag.  If <see
/// cref="MouseDrag.OnMouseMove" /> returns true, call <see
/// cref="CreateVisual" /> to create a Visual to represent the dragged
/// vertices.
///
/// <para>
/// Call <see cref="CancelDrag" /> if the user wants to cancel the drag
/// operation.
/// </para>
///
/// <para>
/// Call <see cref="RemoveMetadataFromVertices" /> when the drag operation
/// completes or is cancelled.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class DraggedVertices : MouseDragWithVisual
{
    //*************************************************************************
    //  Constructor: DraggedVertices()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="DraggedVertices" /> class.
    /// </summary>
    ///
    /// <param name="vertices">
    /// An array of one or more vertices being dragged.
    /// </param>
    ///
    /// <param name="mouseDownLocation">
    /// Location where the vertex was clicked, in client coordinates.
    /// </param>
    ///
    /// <param name="graphRectangle">
    /// The graph rectangle.
    /// </param>
    ///
    /// <param name="margin">
    /// The graph margin.
    /// </param>
    //*************************************************************************

    public DraggedVertices
    (
        IVertex [] vertices,
        Point mouseDownLocation,
        Rect graphRectangle,
        Int32 margin
    )
    : base(mouseDownLocation, graphRectangle, margin)
    {
        m_aoVertices = vertices;

        // Add required metadata to the vertices being dragged.

        AddMetadataToVertices();

        AssertValid();
    }

    //*************************************************************************
    //  Property: Vertices
    //
    /// <summary>
    /// Gets the vertices being dragged.
    /// </summary>
    ///
    /// <value>
    /// An array of one or move vertices being dragged.
    /// </value>
    //*************************************************************************

    public IVertex []
    Vertices
    {
        get
        {
            AssertValid();

            return (m_aoVertices);
        }
    }

    //*************************************************************************
    //  Method: CreateVisual()
    //
    /// <summary>
    /// Creates the Visual that should be used to represent the dragged
    /// vertices.
    /// </summary>
    ///
    /// <param name="currentMouseLocation">
    /// The current mouse location.
    /// </param>
    ///
    /// <param name="backColor">
    /// The graph's background color.
    /// </param>
    ///
    /// <param name="vertexDrawer">
    /// The VertexDrawer that should be used to draw the dragged vertices.
    /// </param>
    ///
    /// <returns>
    /// The Visual that should be used to represent the dragged vertices.
    /// </returns>
    ///
    /// <remarks>
    /// The returned Visual can be retrieved later via the <see
    /// cref="MouseDragWithVisual.Visual" /> property.
    /// </remarks>
    //*************************************************************************

    public Visual
    CreateVisual
    (
        Point currentMouseLocation,
        Color backColor,
        VertexDrawer vertexDrawer
    )
    {
        Debug.Assert(vertexDrawer != null);
        Debug.Assert(m_bDragIsInProgress);
        AssertValid();

        // This method redraws the dragged vertices at an offset location, and
        // adds the resulting Visuals to a ContainerVisual.
        //
        // Figure out the offset.

        Double dOffsetX = currentMouseLocation.X - m_oMouseDownLocation.X;
        Double dOffsetY = currentMouseLocation.Y - m_oMouseDownLocation.Y;

        GraphDrawingContext oGraphDrawingContext = new GraphDrawingContext(
            m_oGraphRectangle, m_iMargin, backColor);

        ContainerVisual oContainerVisual = new ContainerVisual();

        foreach (IVertex oVertex in m_aoVertices)
        {
            System.Drawing.PointF oOriginalLocation =
                GetOriginalVertexLocation(oVertex);

            oVertex.Location = new System.Drawing.PointF(
                oOriginalLocation.X + (Single)dOffsetX,
                oOriginalLocation.Y + (Single)dOffsetY
                );

            VertexDrawingHistory oVertexDrawingHistory;

            if ( vertexDrawer.TryDrawVertex(oVertex, oGraphDrawingContext,
                out oVertexDrawingHistory) )
            {
                oContainerVisual.Children.Add(
                    oVertexDrawingHistory.DrawingVisual);
            }
        }

        m_oVisual = oContainerVisual;

        return (m_oVisual);
    }

    //*************************************************************************
    //  Method: CancelDrag()
    //
    /// <summary>
    /// Restores the vertices to their original locations.
    /// </summary>
    ///
    /// <remarks>
    /// This should be called when the user cancel the drag operation.
    /// </remarks>
    //*************************************************************************

    public void
    CancelDrag()
    {
        AssertValid();

        // Restore the original vertex locations.

        foreach (IVertex oVertex in m_aoVertices)
        {
            oVertex.Location = GetOriginalVertexLocation(oVertex);
        }
    }

    //*************************************************************************
    //  Method: RemoveMetadataFromVertices()
    //
    /// <summary>
    /// Removes added metadata from the vertices being dragged.
    /// </summary>
    ///
    /// <remarks>
    /// This should be called after the drag operation completes or is
    /// cancelled.  It removes vertex metadata that this class added during the
    /// drag.
    /// </remarks>
    //*************************************************************************

    public void
    RemoveMetadataFromVertices()
    {
        AssertValid();

        foreach (IVertex oVertex in m_aoVertices)
        {
            oVertex.RemoveKey(
                ReservedMetadataKeys.DraggedVerticesOriginalLocation);
        }
    }

    //*************************************************************************
    //  Method: AddMetadataToVertices()
    //
    /// <summary>
    /// Adds required metadata to the vertices being dragged.
    /// </summary>
    ///
    /// <remarks>
    /// The metadata should be removed by the owner of this class by calling
    /// <see cref="RemoveMetadataFromVertices" />.
    /// </remarks>
    //*************************************************************************

    protected void
    AddMetadataToVertices()
    {
        AssertValid();

        // Save the original vertex locations before the vertices get moved.

        foreach (IVertex oVertex in m_aoVertices)
        {
            oVertex.SetValue(
                ReservedMetadataKeys.DraggedVerticesOriginalLocation,
                oVertex.Location);
        }
    }

    //*************************************************************************
    //  Method: GetOriginalVertexLocation()
    //
    /// <summary>
    /// Gets the original location of a vertex.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to the the original location for.
    /// </param>
    ///
    /// <returns>
    /// The vertex's original location, as a PointF.
    /// </returns>
    //*************************************************************************

    protected System.Drawing.PointF
    GetOriginalVertexLocation
    (
        IVertex oVertex
    )
    {
        Debug.Assert(oVertex != null);
        AssertValid();

        return ( (System.Drawing.PointF)oVertex.GetRequiredValue(
            ReservedMetadataKeys.DraggedVerticesOriginalLocation,
            typeof(System.Drawing.PointF) ) );
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

        Debug.Assert(m_aoVertices != null);
        Debug.Assert(m_aoVertices.Length > 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The vertices being dragged.  Can't be null or empty.

    protected IVertex [] m_aoVertices;
}

}
