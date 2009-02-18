
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: VertexToolTipShownEventArgs
//
/// <summary>
/// Provides information for events fired when a tooltip is shown for a vertex.
/// </summary>
//*****************************************************************************

public class VertexToolTipShownEventArgs : VertexEventArgs
{
    //*************************************************************************
    //  Constructor: VertexToolTipShownEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the VertexToolTipShownEventArgs class.
    /// </summary>
    ///
    /// <param name="vertex">
    /// Vertex for which a tooltip is shown.
    /// </param>
    //*************************************************************************

    public VertexToolTipShownEventArgs
    (
        IVertex vertex

    )
    : base(vertex)
    {
        m_oVertexToolTip = null;

        AssertValid();
    }

    //*************************************************************************
    //  Property: VertexToolTip
    //
    /// <summary>
    /// Gets or sets the UIElement to use for the vertex tooltip.
    /// </summary>
    ///
    /// <value>
    /// The UIElement to use.  The default value is null.
    /// </value>
    ///
    /// <remarks>
    /// See <see cref="NodeXLControl.ShowVertexToolTips" /> for information on
    /// how to display a custom tooltip when the mouse is hovered over a
    /// vertex.
    ///
    /// <para>
    /// If this property is left at its default value of null, the text stored
    /// on the vertex's ReservedMetadataKeys.<see
    /// cref="ReservedMetadataKeys.VertexToolTip" /> key is used for the
    /// tooltip.  If there is no such key, a tooltip isn't displayed.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public UIElement
    VertexToolTip
    {
        get
        {
            AssertValid();

            return (m_oVertexToolTip);
        }

        set
        {
            m_oVertexToolTip = value;

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

    // [Conditional("DEBUG")] 

    public override void
    AssertValid()
    {
        base.AssertValid();

        // m_oVertexToolTip
    }


    //*************************************************************************
    //  Protected member data
    //*************************************************************************

    /// The UIElement to use for the vertex tooltip, or null to use the tooltip
    /// string stored in the vertex's metadata.

    protected UIElement m_oVertexToolTip;
}


//*****************************************************************************
//  Delegate: VertexToolTipShownEventHandler
//
/// <summary>
/// Represents a method that will handle an event fired when a tooltip is shown
/// for a vertex.
/// </summary>
///
/// <param name="sender">
/// The object that fired the event.
/// </param>
///
/// <param name="e">
/// Provides information about the vertex and its tooltip.
/// </param>
//*****************************************************************************

public delegate void
VertexToolTipShownEventHandler
(
    Object sender,
    VertexToolTipShownEventArgs e
);

}
