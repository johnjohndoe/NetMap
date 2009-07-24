
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Diagnostics;
using Microsoft.WpfGraphicsLib;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: NodeXLWithAxesControl
//
/// <summary>
/// Adds graph axes to a <see cref="Visualization.Wpf.NodeXLControl" />.
/// </summary>
///
/// <remarks>
/// This control wraps a <see cref="Visualization.Wpf.NodeXLControl" /> in a
/// Grid that also includes <see cref="Axis" /> controls for the x- and y-axes.
///
/// <para>
/// The axes display default ranges.  You should call <see
/// cref="Axis.SetRange" /> on each of the <see cref="XAxis" /> and <see
/// cref="YAxis" /> objects to set the range of values being displayed in the
/// <see cref="Visualization.Wpf.NodeXLControl" />.
/// </para>
///
/// <para>
/// The <see cref="Axis" /> controls automatically adjust themselves when the
/// graph is zoomed or translated.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class NodeXLWithAxesControl : Grid
{
    //*************************************************************************
    //  Constructor: NodeXLWithAxesControl()
    //
    /// <overloads>
    /// Initializes a new instance of the <see cref="NodeXLWithAxesControl" />
    /// class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see cref="NodeXLWithAxesControl" />
    /// class.
    /// </summary>
    //*************************************************************************

    public NodeXLWithAxesControl()
    :
    this( new NodeXLControl() )
    {
        // (Do nothing else.)

        // AssertValid();
    }

    //*************************************************************************
    //  Constructor: NodeXLWithAxesControl()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="NodeXLWithAxesControl" />
    /// class with a specified <see cref="NodeXLControl" />.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
    /// The <see cref="NodeXLControl" /> to embed within the control.
    /// </param>
    //*************************************************************************

    public NodeXLWithAxesControl
    (
        NodeXLControl nodeXLControl
    )
    {
        Debug.Assert(nodeXLControl != null);

        m_oNodeXLControl = nodeXLControl;

        // Create a 1x1 grid.  The y-axis is in (0,0), the x-axis is in (1,1),
        // the embedded NodeXLControl is in (0,1), and a Rectangle control is
        // in (1,0).
        //
        // The x-axis sizes itself vertically and the y-axis sizes itself
        // horizontally, so their row or column definition uses
        // GridLength.Auto.

        RowDefinitionCollection oRowDefinitions = this.RowDefinitions;

        oRowDefinitions.Add( new RowDefinition() );
        RowDefinition oSecondRowDefinition = new RowDefinition();
        oSecondRowDefinition.Height = GridLength.Auto;
        oRowDefinitions.Add(oSecondRowDefinition);

        ColumnDefinitionCollection oColumnDefinitions = this.ColumnDefinitions;

        ColumnDefinition oFirstColumnDefinition = new ColumnDefinition();
        oFirstColumnDefinition.Width = GridLength.Auto;
        oColumnDefinitions.Add(oFirstColumnDefinition);
        oColumnDefinitions.Add( new ColumnDefinition() );

        UIElementCollection oChildren = this.Children;

        Grid.SetColumn(m_oNodeXLControl, 1);
        oChildren.Add(m_oNodeXLControl);

        // The Rectangle in cell (1,0) exists only to cover up the
        // NodeXLControl's contents when it is zoomed, because the Grid does
        // not clip the NodeXLControl.  What is the correct way to force the
        // Grid to clip the NodeXLControl's contents?  Setting
        // NodeXLControl.ClipToBounds to true does not do anything.

        Rectangle oRectangle = new Rectangle();
        oRectangle.Fill = SystemColors.WindowBrush;
        Grid.SetRow(oRectangle, 1);
        oChildren.Add(oRectangle);

        m_oXAxis = new Axis();
        Grid.SetRow(m_oXAxis, 1);
        Grid.SetColumn(m_oXAxis, 1);
        oChildren.Add(m_oXAxis);

        m_oYAxis = new Axis();
        m_oYAxis.IsXAxis = false;
        oChildren.Add(m_oYAxis);

        // Allow the Axis controls to adjust themselves when the graph is
        // zoomed or translated.

        m_oXAxis.DockedControlRenderTransform =
            m_oYAxis.DockedControlRenderTransform =
            m_oNodeXLControl.RenderTransform;

        m_oNodeXLControl.GraphZoomChanged +=
            new EventHandler(this.m_oNodeXLControl_GraphZoomChanged);

        m_oNodeXLControl.GraphTranslationChanged +=
            new EventHandler(this.m_oNodeXLControl_GraphTranslationChanged);

        AssertValid();
    }

    //*************************************************************************
    //  Property: NodeXLControl
    //
    /// <summary>
    /// Gets the embedded <see cref="Visualization.Wpf.NodeXLControl" />.
    /// </summary>
    ///
    /// <value>
    /// The embedded <see cref="Visualization.Wpf.NodeXLControl" />.
    /// </value>
    //*************************************************************************

    public NodeXLControl
    NodeXLControl
    {
        get
        {
            AssertValid();

            return (m_oNodeXLControl);
        }
    }

    //*************************************************************************
    //  Property: ShowAxes
    //
    /// <summary>
    /// Gets or sets a flag specifying whether the graph axes should be shown.
    /// </summary>
    ///
    /// <value>
    /// true to show the graph axes, false to hide them.  They are shown by
    /// default.
    /// </value>
    //*************************************************************************

    public Boolean
    ShowAxes
    {
        get
        {
            AssertValid();

            return (this.RowDefinitions[1].Height.IsAuto);
        }

        set
        {
            GridLength oHeightOrWidth;

            if (value)
            {
                oHeightOrWidth = GridLength.Auto;
            }
            else
            {
                oHeightOrWidth = new GridLength(0);
            }

            m_oXAxis.Visibility = m_oYAxis.Visibility =
                (value ? Visibility.Visible : Visibility.Hidden);

            this.RowDefinitions[1].Height = oHeightOrWidth;
            this.ColumnDefinitions[0].Width = oHeightOrWidth;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: XAxis
    //
    /// <summary>
    /// Gets the x-axis.
    /// </summary>
    ///
    /// <value>
    /// The x-axis, as an <see cref="Axis" />.
    /// </value>
    //*************************************************************************

    public Axis
    XAxis
    {
        get
        {
            AssertValid();

            return (m_oXAxis);
        }
    }

    //*************************************************************************
    //  Property: YAxis
    //
    /// <summary>
    /// Gets the y-axis.
    /// </summary>
    ///
    /// <value>
    /// The y-axis, as an <see cref="Axis" />.
    /// </value>
    //*************************************************************************

    public Axis
    YAxis
    {
        get
        {
            AssertValid();

            return (m_oYAxis);
        }
    }

    //*************************************************************************
    //  Method: SetFont()
    //
    /// <summary>
    /// Sets the font used to draw the axis labels and values.
    /// </summary>
    ///
    /// <param name="typeface">
    /// The Typeface to use.
    /// </param>
    ///
    /// <param name="labelEmSize">
    /// The font size to use for the axis labels, in ems.  (A slightly smaller
    /// size is used for the axis values.)
    /// </param>
    ///
    /// <remarks>
    /// The default font is the SystemFonts.CaptionFontFamily at size 10.
    /// </remarks>
    //*************************************************************************

    public void
    SetFont
    (
        Typeface typeface,
        Double labelEmSize
    )
    {
        Debug.Assert(typeface != null);
        Debug.Assert(labelEmSize > 0);
        AssertValid();

        m_oXAxis.SetFont(typeface, labelEmSize);
        m_oYAxis.SetFont(typeface, labelEmSize);

        if (this.ShowAxes)
        {
            // The Axis objects size themselves based on their font, so the
            // Grid needs to resize its rows and columns.  For some reason,
            // InvalidateArrange() won't do this, so force a resize by
            // temporarily resizing one of the Grid's rows.

            RowDefinition oSecondRowDefinition = this.RowDefinitions[1];

            oSecondRowDefinition.Height = new GridLength(1);
            oSecondRowDefinition.Height = GridLength.Auto;
        }
    }

    //*************************************************************************
    //  Method: InvalidateAxes()
    //
    /// <summary>
    /// Invalidates the axis visuals.
    /// </summary>
    //*************************************************************************

    protected void
    InvalidateAxes()
    {
        AssertValid();

        m_oXAxis.InvalidateVisual();
        m_oYAxis.InvalidateVisual();
    }

    //*************************************************************************
    //  Method: m_oNodeXLControl_GraphZoomChanged()
    //
    /// <summary>
    /// Handles the GraphZoomChanged event on the m_oNodeXLControl control.
    /// </summary>
    ///
    /// <param name="oSender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="oEventArgs">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    m_oNodeXLControl_GraphZoomChanged
    (
        Object oSender,
        EventArgs oEventArgs
    )
    {
        AssertValid();

        InvalidateAxes();
    }

    //*************************************************************************
    //  Method: m_oNodeXLControl_GraphTranslationChanged()
    //
    /// <summary>
    /// Handles the GraphTranslationChanged event on the m_oNodeXLControl
    /// control.
    /// </summary>
    ///
    /// <param name="oSender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="oEventArgs">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    m_oNodeXLControl_GraphTranslationChanged
    (
        Object oSender,
        EventArgs oEventArgs
    )
    {
        AssertValid();

        InvalidateAxes();
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public virtual void
    AssertValid()
    {
        Debug.Assert(m_oNodeXLControl != null);
        Debug.Assert(m_oXAxis != null);
        Debug.Assert(m_oYAxis != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Embedded NodeXLControl.

    protected NodeXLControl m_oNodeXLControl;

    /// x-axis.

    protected Axis m_oXAxis;

    /// y-axis.

    protected Axis m_oYAxis;
}

}
