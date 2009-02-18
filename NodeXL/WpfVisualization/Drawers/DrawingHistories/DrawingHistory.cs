
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Media;
using System.Diagnostics;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: DrawingHistory
//
/// <summary>
/// Retains information about how an object was drawn.
/// </summary>
///
/// <remarks>
/// This is a base class for a family of classes that retain information about
/// how various objects were drawn using the DrawingVisual class.
/// </remarks>
//*****************************************************************************

public abstract class DrawingHistory : VisualizationBase
{
    //*************************************************************************
    //  Constructor: DrawingHistory()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="DrawingHistory" /> class.
    /// </summary>
    ///
    /// <param name="drawingVisual">
    /// The DrawingVisual object that was used to draw the object.
    /// </param>
    ///
    /// <param name="drawnAsSelected">
    /// true if the object was drawn as selected.
    /// </param>
    //*************************************************************************

    public DrawingHistory
    (
        DrawingVisual drawingVisual,
        Boolean drawnAsSelected
    )
    {
        m_oDrawingVisual = drawingVisual;
        m_bDrawnAsSelected = drawnAsSelected;

        // AssertValid();
    }

    //*************************************************************************
    //  Property: DrawingVisual
    //
    /// <summary>
    /// Gets the DrawingVisual object that was used to draw the object.
    /// </summary>
    ///
    /// <value>
    /// The DrawingVisual object that was used to draw the object.
    /// </value>
    //*************************************************************************

    public DrawingVisual
    DrawingVisual
    {
        get
        {
            AssertValid();

            return (m_oDrawingVisual);
        }
    }

    //*************************************************************************
    //  Property: DrawnAsSelected
    //
    /// <summary>
    /// Gets a flag indicating whether the object was drawn as selected.
    /// </summary>
    ///
    /// <value>
    /// true if the object was drawn as selected.
    /// </value>
    //*************************************************************************

    public Boolean
    DrawnAsSelected
    {
        get
        {
            AssertValid();

            return (m_bDrawnAsSelected);
        }
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

        Debug.Assert(m_oDrawingVisual != null);
        // m_bDrawnAsSelected
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The DrawingVisual object that was used to draw the object.

    protected DrawingVisual m_oDrawingVisual;

    /// true if the object was drawn as selected.
    
    protected Boolean m_bDrawnAsSelected;
}

}
