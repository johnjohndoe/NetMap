using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.NodeXL.Visualization.Wpf;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ApplicationUtil
{
//*****************************************************************************
//  Class: GraphZoomAndScaleControl
//
/// <summary>
/// Provides TrackBars for setting the zoom and scale properties on a
/// <see cref="Visualization.Wpf.NodeXLControl" />.
/// </summary>
///
/// <remarks>
/// Set the <see cref="GraphZoomAndScaleControl.NodeXLControl" /> property to
/// an initialized <see cref="Visualization.Wpf.NodeXLControl" />.  Once this
/// is done, the TrackBars within this control will set the zoom and scale
/// properties on the <see cref="Visualization.Wpf.NodeXLControl" />, and if
/// the user zooms the graph by scrolling the mouse within the <see
/// cref="Visualization.Wpf.NodeXLControl" />, the zoom TrackBar within this
/// control is automatically updated.
/// </remarks>
//*****************************************************************************

public partial class GraphZoomAndScaleControl : UserControl
{
    //*************************************************************************
    //  Constructor: GraphZoomAndScaleControl()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GraphZoomAndScaleControl" /> class.
    /// </summary>
    //*************************************************************************

    public GraphZoomAndScaleControl()
    {
        InitializeComponent();

        m_oNodeXLControl = null;
        m_bChangingNodeXLControlValue = false;

        tbGraphZoom.Minimum =
            NodeXLControlValueToTrackBarValue(NodeXLControl.MinimumGraphZoom);

        tbGraphZoom.Maximum =
            NodeXLControlValueToTrackBarValue(NodeXLControl.MaximumGraphZoom);

        tbGraphScale.Minimum =
            NodeXLControlValueToTrackBarValue(NodeXLControl.MinimumGraphScale);

        tbGraphScale.Maximum =
            NodeXLControlValueToTrackBarValue(NodeXLControl.MaximumGraphScale);

        AssertValid();
    }

    //*************************************************************************
    //  Property: NodeXLControl
    //
    /// <summary>
    /// Gets or sets the <see cref="Visualization.Wpf.NodeXLControl" /> whose
    /// zoom and scale are being controlled by this control.
    /// </summary>
    ///
    /// <value>
    /// Set this to an initialized <see
    /// cref="Visualization.Wpf.NodeXLControl" />.  It is null by default.
    /// </value>
    //*************************************************************************

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.ReadOnly(true)]

    public NodeXLControl
    NodeXLControl
    {
        get
        {
            AssertValid();

            return (m_oNodeXLControl);
        }

        set
        {
            const String PropertyName = "NodeXLControl";

            this.ArgumentChecker.CheckPropertyNotNull(PropertyName, value);

            m_oNodeXLControl = value;

            m_oNodeXLControl.GraphZoomChanged +=
                new EventHandler(this.m_oNodeXLControl_GraphZoomChanged);

            tbGraphZoom.Value = NodeXLControlValueToTrackBarValue(
                m_oNodeXLControl.GraphZoom);

            tbGraphScale.Value = NodeXLControlValueToTrackBarValue(
                m_oNodeXLControl.GraphScale);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: NodeXLControlValueToTrackBarValue()
    //
    /// <summary>
    /// Converts a graph zoom or graph scale value from the NodeXLControl to a
    /// value that can be used in one of this control's TrackBars.
    /// </summary>
    ///
    /// <param name="dNodeXLControlValue">
    /// A graph zoom or graph scale value from the NodeXLControl.
    /// </param>
    ///
    /// <returns>
    /// A converted value for use in a TrackBar.
    /// </returns>
    //*************************************************************************

    protected Int32
    NodeXLControlValueToTrackBarValue
    (
        Double dNodeXLControlValue
    )
    {
        AssertValid();

        return ( (Int32)(TrackBarFactor * dNodeXLControlValue) );
    }

    //*************************************************************************
    //  Method: TrackBarValueToNodeXLControlValue()
    //
    /// <summary>
    /// Converts a graph zoom or graph scale value from one of this control's
    /// TrackBars to a value that can be used in the NodeXLControl.
    /// </summary>
    ///
    /// <param name="iTrackBarValue">
    /// A graph zoom or graph scale value from a TrackBar.
    /// </param>
    ///
    /// <returns>
    /// A converted value for use in the NodeXLControl.
    /// </returns>
    //*************************************************************************

    protected Double
    TrackBarValueToNodeXLControlValue
    (
        Int32 iTrackBarValue
    )
    {
        AssertValid();

        return ( (Double)iTrackBarValue / TrackBarFactor );
    }

    //*************************************************************************
    //  Property: ClassName
    //
    /// <summary>
    /// Gets the full name of the class.
    /// </summary>
    ///
    /// <value>
    /// The full name of the class, suitable for use in error messages.
    /// </value>
    //*************************************************************************

    protected String
    ClassName
    {
        get
        {
            return (this.GetType().FullName);
        }
    }

    //*************************************************************************
    //  Property: ArgumentChecker
    //
    /// <summary>
    /// Gets a new initialized <see cref="ArgumentChecker" /> object.
    /// </summary>
    ///
    /// <value>
    /// A new initialized <see cref="ArgumentChecker" /> object.
    /// </value>
    ///
    /// <remarks>
    /// The returned object can be used to check the validity of property
    /// values and method parameters.
    /// </remarks>
    //*************************************************************************

    internal ArgumentChecker
    ArgumentChecker
    {
        get
        {
            return ( new ArgumentChecker(this.ClassName) );
        }
    }

    //*************************************************************************
    //  Method: m_oNodeXLControl_GraphZoomChanged()
    //
    /// <summary>
    /// Handles the GraphZoomChanged event on the m_oNodeXLControl object.
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

        if (!m_bChangingNodeXLControlValue)
        {
            tbGraphZoom.Value =
                NodeXLControlValueToTrackBarValue(m_oNodeXLControl.GraphZoom);
        }
    }

    //*************************************************************************
    //  Method: tbGraphZoom_Scroll()
    //
    /// <summary>
    /// Handles the Scroll event on the tbGraphZoom TrackBar.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    tbGraphZoom_Scroll
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        if (m_oNodeXLControl == null)
        {
            return;
        }

        m_bChangingNodeXLControlValue = true;

        m_oNodeXLControl.GraphZoom =
            TrackBarValueToNodeXLControlValue(tbGraphZoom.Value);

        m_bChangingNodeXLControlValue = false;
    }

    //*************************************************************************
    //  Method: tbGraphScale_Scroll()
    //
    /// <summary>
    /// Handles the Scroll event on the tbGraphScale TrackBar.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    tbGraphScale_Scroll
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        if (m_oNodeXLControl == null)
        {
            return;
        }

        m_bChangingNodeXLControlValue = true;

        m_oNodeXLControl.GraphScale =
            TrackBarValueToNodeXLControlValue(tbGraphScale.Value);

        m_bChangingNodeXLControlValue = false;
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
        // m_oNodeXLControl
        // m_bChangingNodeXLControlValue
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Conversion factor that allows the integer TrackBar to control a Double
    /// property in the NodeXLControl.

    protected const Double TrackBarFactor = 100.0;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The NodeXLControl whose zoom and scale are being controlled by this
    /// control, or null if not set yet.

    protected NodeXLControl m_oNodeXLControl;

    /// Protects against an endless loop when setting a NodeXLControl property.

    protected Boolean m_bChangingNodeXLControlValue;
}
}
