
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Configuration;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Visualization.Wpf;
using Microsoft.WpfGraphicsLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GeneralUserSettings
//
/// <summary>
/// Stores the user's general settings.
/// </summary>
//*****************************************************************************

[ SettingsGroupNameAttribute("GeneralUserSettings4") ]

public class GeneralUserSettings : ApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: GeneralUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the GeneralUserSettings class.
    /// </summary>
    //*************************************************************************

    public GeneralUserSettings()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: NewWorkbookGraphDirectedness
    //
    /// <summary>
    /// Gets or sets the graph directedness applied to a new workbook.
    /// </summary>
    ///
    /// <value>
    /// The graph directedness applied to a new workbook, as a
    /// GraphDirectedness.  The default value is GraphDirectedness.Undirected.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("Undirected") ]

    public GraphDirectedness
    NewWorkbookGraphDirectedness
    {
        get
        {
            AssertValid();

            return ( (GraphDirectedness)
                this[NewWorkbookGraphDirectednessKey] );
        }

        set
        {
            this[NewWorkbookGraphDirectednessKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ReadClusters
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the cluster worksheets should be
    /// read when the workbook is read into the graph.
    /// </summary>
    ///
    /// <value>
    /// true to read the cluster worksheets.  The default value is true.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("true") ]

    public Boolean
    ReadClusters
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[ReadClustersKey] );
        }

        set
        {
            this[ReadClustersKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ShowGraphLegend
    //
    /// <summary>
    /// Gets a flag indicating whether the graph legend should be shown in the
    /// graph pane.
    /// </summary>
    ///
    /// <value>
    /// true to show the graph legend in the graph pane.  The default value is
    /// false.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("false") ]

    public Boolean
    ShowGraphLegend
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[ShowGraphLegendKey] );
        }

        set
        {
            this[ShowGraphLegendKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ShowGraphAxes
    //
    /// <summary>
    /// Gets a flag indicating whether the graph axes should be shown in the
    /// graph pane.
    /// </summary>
    ///
    /// <value>
    /// true to show the graph axes in the graph pane.  The default value is
    /// false.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("false") ]

    public Boolean
    ShowGraphAxes
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[ShowGraphAxesKey] );
        }

        set
        {
            this[ShowGraphAxesKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: AutoReadWorkbook
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the workbook should be read into
    /// the graph when a visual property is set in the workbook, a scheme is
    /// applied, or the workbook is autofilled.
    /// </summary>
    ///
    /// <value>
    /// true to read the workbook into the graph.  The default value is true.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("true") ]

    public Boolean
    AutoReadWorkbook
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[AutoReadWorkbookKey] );
        }

        set
        {
            this[AutoReadWorkbookKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: LabelFont
    //
    /// <summary>
    /// Gets or sets the font used for the graph's labels.
    /// </summary>
    ///
    /// <value>
    /// The label font, as a Font.  The default value is Microsoft Sans Serif,
    /// 8.25pt.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute(DefaultFont) ]

    public Font
    LabelFont
    {
        // Note that the font type is System.Drawing.Font, which is what the
        // System.Windows.Forms.FontDialog class uses.  It gets converted to
        // WPF font types in TransferToGraphDrawer().

        get
        {
            AssertValid();

            return ( (Font)this[LabelFontKey] );
        }

        set
        {
            this[LabelFontKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: AxisFont
    //
    /// <summary>
    /// Gets or sets the font used for the graph's axes.
    /// </summary>
    ///
    /// <value>
    /// The axis font, as a Font.  The default value is Microsoft Sans Serif,
    /// 8.25pt.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute(DefaultFont) ]

    public Font
    AxisFont
    {
        get
        {
            AssertValid();

            return ( (Font)this[AxisFontKey] );
        }

        set
        {
            this[AxisFontKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: BackColor
    //
    /// <summary>
    /// Gets or sets the graph's background color.
    /// </summary>
    ///
    /// <value>
    /// The graph's background color, as a Color.  The default value is
    /// Color.White.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("White") ]

    public Color
    BackColor
    {
        get
        {
            AssertValid();

            return ( (Color)this[BackColorKey] );
        }

        set
        {
            this[BackColorKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeWidth
    //
    /// <summary>
    /// Gets or sets the width of unselected edges.
    /// </summary>
    ///
    /// <value>
    /// The width of unselected edges, as a Single.  Must be between
    /// EdgeWidthConverter.MinimumWidthWorkbook and
    /// EdgeWidthConverter.MaximumWidthConverter.  The default value
    /// is 1.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("1") ]

    public Single
    EdgeWidth
    {
        get
        {
            AssertValid();

            return ( (Single)this[EdgeWidthKey] );
        }

        set
        {
            Debug.Assert(value >= EdgeWidthConverter.MinimumWidthWorkbook);
            Debug.Assert(value <= EdgeWidthConverter.MaximumWidthWorkbook);

            this[EdgeWidthKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SelectedEdgeWidth
    //
    /// <summary>
    /// Gets or sets the width of selected edges.
    /// </summary>
    ///
    /// <value>
    /// The width of selected edges, as a Single.  Must be between
    /// EdgeWidthConverter.MinimumWidthWorkbook and
    /// EdgeWidthConverter.MaximumWidthWorkbook.  The default value is 1.5.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("1.5") ]

    public Single
    SelectedEdgeWidth
    {
        get
        {
            AssertValid();

            return ( (Single)this[SelectedEdgeWidthKey] );
        }

        set
        {
            Debug.Assert(value >= EdgeWidthConverter.MinimumWidthWorkbook);
            Debug.Assert(value <= EdgeWidthConverter.MaximumWidthWorkbook);

            this[SelectedEdgeWidthKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeColor
    //
    /// <summary>
    /// Gets or sets the color of unselected edges.
    /// </summary>
    ///
    /// <value>
    /// The color of unselected edges, as a Color.  The default value is
    /// Color.Black.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("Black") ]

    public Color
    EdgeColor
    {
        get
        {
            AssertValid();

            return ( (Color)this[EdgeColorKey] );
        }

        set
        {
            this[EdgeColorKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeAlpha
    //
    /// <summary>
    /// Gets or sets the alpha component of the color of unselected edges.
    /// </summary>
    ///
    /// <value>
    /// The alpha component of the color of unselected edges.  Must be between
    /// AlphaConverter.MinimumAlphaWorkbook and
    /// AlphaConverter.MaximumAlphaWorkbook.  The default value is 100.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("100") ]

    public Single
    EdgeAlpha
    {
        get
        {
            AssertValid();

            return ( (Single)this[EdgeAlphaKey] );
        }

        set
        {
            this[EdgeAlphaKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SelectedEdgeColor
    //
    /// <summary>
    /// Gets or sets the color of selected edges.
    /// </summary>
    ///
    /// <value>
    /// The color of selected edges, as a Color.  The default value is
    /// Color.Red.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("Red") ]

    public Color
    SelectedEdgeColor
    {
        get
        {
            AssertValid();

            return ( (Color)this[SelectedEdgeColorKey] );
        }

        set
        {
            this[SelectedEdgeColorKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: RelativeArrowSize
    //
    /// <summary>
    /// Gets or sets the relative size of arrow heads on directed edges.
    /// </summary>
    ///
    /// <value>
    /// The relative size of arrow heads, as a <see cref="Single" />.  Must be
    /// between <see cref="EdgeDrawer.MinimumRelativeArrowSize" /> and <see
    /// cref="EdgeDrawer.MaximumRelativeArrowSize" />, inclusive.  The default
    /// value is 3.
    /// </value>
    ///
    /// <remarks>
    /// The value is relative to <see cref="EdgeWidth" /> and <see
    /// cref="SelectedEdgeWidth" />.  If the width or selected width is
    /// increased, the arrow size on unselected or selected edges is increased
    /// proportionally.
    /// </remarks>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("3") ]

    public Single
    RelativeArrowSize
    {
        get
        {
            AssertValid();

            return ( (Single)this[RelativeArrowSizeKey] );
        }

        set
        {
            Debug.Assert(value >= EdgeDrawer.MinimumRelativeArrowSize);
            Debug.Assert(value <= EdgeDrawer.MaximumRelativeArrowSize);

            this[RelativeArrowSizeKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexShape
    //
    /// <summary>
    /// Gets or sets the shape of the vertices.
    /// </summary>
    ///
    /// <value>
    /// The shape of the vertices, as a <see cref="VertexShape" />.  The
    /// default value is <see cref="Visualization.Wpf.VertexShape.Disk" />.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("Disk") ]

    public VertexShape
    VertexShape
    {
        get
        {
            AssertValid();

            return ( (VertexShape)this[VertexShapeKey] );
        }

        set
        {
            this[VertexShapeKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexRadius
    //
    /// <summary>
    /// Gets or sets the radius of the vertices.
    /// </summary>
    ///
    /// <value>
    /// The radius of the vertices, as a Single.  Must be between
    /// VertexRadiusConverter.MinimumRadiusWorkbook and
    /// VertexRadiusConverter.MaximumRadiusWorkbook.  The default value is 1.5.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("1.5") ]

    public Single
    VertexRadius
    {
        get
        {
            AssertValid();

            return ( (Single)this[VertexRadiusKey] );
        }

        set
        {
            Debug.Assert(value >= VertexRadiusConverter.MinimumRadiusWorkbook);
            Debug.Assert(value <= VertexRadiusConverter.MaximumRadiusWorkbook);

            this[VertexRadiusKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexColor
    //
    /// <summary>
    /// Gets or sets the color of unselected vertices.
    /// </summary>
    ///
    /// <value>
    /// The color of unselected vertices, as a Color.  The default value is
    /// Color.Black.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("Black") ]

    public Color
    VertexColor
    {
        get
        {
            AssertValid();

            return ( (Color)this[VertexColorKey] );
        }

        set
        {
            this[VertexColorKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: PrimaryLabelFillColor
    //
    /// <summary>
    /// Gets or sets the fill color of vertices that are drawn as primary
    /// labels.
    /// </summary>
    ///
    /// <value>
    /// The fill color of vertices that are drawn as primary labels, as a
    /// Color.  The default value is Color.White.
    /// </value>
    ///
    /// <remarks>
    /// <see cref="VertexColor" /> is used as the primary label's text color.
    /// </remarks>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("White") ]

    public Color
    PrimaryLabelFillColor
    {
        get
        {
            AssertValid();

            return ( (Color)this[PrimaryLabelFillColorKey] );
        }

        set
        {
            this[PrimaryLabelFillColorKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexAlpha
    //
    /// <summary>
    /// Gets or sets the alpha component of the color of unselected vertices.
    /// </summary>
    ///
    /// <value>
    /// The alpha component of the color of unselected vertices.  Must be
    /// between AlphaConverter.MinimumAlphaWorkbook and
    /// AlphaConverter.MaximumAlphaWorkbook.  The default value is 100.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("100") ]

    public Single
    VertexAlpha
    {
        get
        {
            AssertValid();

            return ( (Single)this[VertexAlphaKey] );
        }

        set
        {
            this[VertexAlphaKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SelectedVertexColor
    //
    /// <summary>
    /// Gets or sets the color of selected vertices.
    /// </summary>
    ///
    /// <value>
    /// The color of selected vertices, as a Color.  The default value is
    /// Color.Red.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("Red") ]

    public Color
    SelectedVertexColor
    {
        get
        {
            AssertValid();

            return ( (Color)this[SelectedVertexColorKey] );
        }

        set
        {
            this[SelectedVertexColorKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: LayoutUserSettings
    //
    /// <summary>
    /// Gets or sets the user's settings for all the graph layouts used by the
    /// application.
    /// </summary>
    ///
    /// <value>
    /// The user's settings for all the graph layouts used by the application,
    /// as a LayoutUserSettings.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("FruchtermanReingold\t6\t3.0\t10") ]

    public LayoutUserSettings
    LayoutUserSettings
    {
        get
        {
            AssertValid();

            return ( (LayoutUserSettings)this[LayoutUserSettingsKey] );
        }

        set
        {
            this[LayoutUserSettingsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: AutoSelect
    //
    /// <summary>
    /// Gets or sets a flag that determines edge and vertex selection behavior.
    /// </summary>
    ///
    /// <value>
    /// If true, selecting a vertex in the graph or workbook automatically
    /// selects the vertex's incident edges, and selecting an edge in the
    /// workbook automatically selects the edge's adjacent vertices.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("true") ]

    public Boolean
    AutoSelect
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[AutoSelectKey] );
        }

        set
        {
            this[AutoSelectKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ClearTablesBeforeImport
    //
    /// <summary>
    /// Gets or sets a flag indicating whether NodeXL tables should be cleared
    /// before anything is imported into the workbook.
    /// </summary>
    ///
    /// <value>
    /// If true, the NodeXL tables are cleared before anything is imported into
    /// the workbook.  If false, the imported contents are appended to the
    /// workbook.  The default value is true.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("true") ]

    public Boolean
    ClearTablesBeforeImport
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[ClearTablesBeforeImportKey] );
        }

        set
        {
            this[ClearTablesBeforeImportKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: TransferToNodeXLWithAxesControl()
    //
    /// <summary>
    /// Transfers the settings to a <see cref="NodeXLWithAxesControl" />.
    /// </summary>
    ///
    /// <param name="nodeXLWithAxesControl">
    /// Control to transfer the settings to.
    /// </param>
    //*************************************************************************

    public void
    TransferToNodeXLWithAxesControl
    (
        NodeXLWithAxesControl nodeXLWithAxesControl
    )
    {
        Debug.Assert(nodeXLWithAxesControl != null);
        AssertValid();

        Font oAxisFont = this.AxisFont;

        nodeXLWithAxesControl.SetFont(
            WpfGraphicsUtil.FontToTypeface(oAxisFont),

            WpfGraphicsUtil.WindowsFormsFontSizeToWpfFontSize(
                oAxisFont.Size) );

        TransferToNodeXLControl(nodeXLWithAxesControl.NodeXLControl);
    }

    //*************************************************************************
    //  Method: TransferToNodeXLControl()
    //
    /// <summary>
    /// Transfers the settings to a <see cref="NodeXLControl" />.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
    /// Control to transfer the settings to.
    /// </param>
    //*************************************************************************

    public void
    TransferToNodeXLControl
    (
        NodeXLControl nodeXLControl
    )
    {
        Debug.Assert(nodeXLControl != null);
        AssertValid();

        this.LayoutUserSettings.TransferToLayout(nodeXLControl.Layout);

        TransferToGraphDrawer(nodeXLControl.GraphDrawer);

        nodeXLControl.MouseSelectionMode = this.AutoSelect ?
            MouseSelectionMode.SelectVertexAndIncidentEdges :
            MouseSelectionMode.SelectVertexOnly;
    }

    //*************************************************************************
    //  Method: TransferToGraphDrawer()
    //
    /// <summary>
    /// Transfers the settings to a <see cref="GraphDrawer" />.
    /// </summary>
    ///
    /// <param name="graphDrawer">
    /// Graph drawer to transfer the settings to.
    /// </param>
    //*************************************************************************

    public void
    TransferToGraphDrawer
    (
        GraphDrawer graphDrawer
    )
    {
        Debug.Assert(graphDrawer != null);
        AssertValid();

        graphDrawer.BackColor =
            WpfGraphicsUtil.ColorToWpfColor(this.BackColor);

        EdgeDrawer oEdgeDrawer = graphDrawer.EdgeDrawer;
        VertexDrawer oVertexDrawer = graphDrawer.VertexDrawer;

        EdgeWidthConverter oEdgeWidthConverter = new EdgeWidthConverter();
        AlphaConverter oAlphaConverter = new AlphaConverter();

        oEdgeDrawer.Width =
            oEdgeWidthConverter.WorkbookToGraph(this.EdgeWidth);

        oEdgeDrawer.SelectedWidth =
            oEdgeWidthConverter.WorkbookToGraph(this.SelectedEdgeWidth);

        oEdgeDrawer.Color = WpfGraphicsUtil.ColorToWpfColor(
            Color.FromArgb(oAlphaConverter.WorkbookToGraph(this.EdgeAlpha),
                this.EdgeColor)
                );

        oEdgeDrawer.SelectedColor = WpfGraphicsUtil.ColorToWpfColor(
            this.SelectedEdgeColor);

        oEdgeDrawer.RelativeArrowSize = this.RelativeArrowSize;

        Font oLabelFont = this.LabelFont;

        System.Windows.Media.Typeface oTypeface =
            WpfGraphicsUtil.FontToTypeface(oLabelFont);

        Double dFontSize = WpfGraphicsUtil.WindowsFormsFontSizeToWpfFontSize(
            oLabelFont.Size);

        oEdgeDrawer.SetFont(oTypeface, dFontSize);

        oVertexDrawer.Shape = this.VertexShape;

        oVertexDrawer.Radius = ( new VertexRadiusConverter() ).WorkbookToGraph(
            this.VertexRadius);

        oVertexDrawer.Color = WpfGraphicsUtil.ColorToWpfColor(
            Color.FromArgb(oAlphaConverter.WorkbookToGraph(this.VertexAlpha),
                this.VertexColor)
                );

        oVertexDrawer.SelectedColor = WpfGraphicsUtil.ColorToWpfColor(
            this.SelectedVertexColor);

        oVertexDrawer.PrimaryLabelFillColor =
            WpfGraphicsUtil.ColorToWpfColor(this.PrimaryLabelFillColor);

        oVertexDrawer.SetFont(oTypeface, dFontSize);
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
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Default font to use for the graph's labels and axes.

    protected const String DefaultFont = "Microsoft Sans Serif, 8.25pt";

    /// Name of the settings key for the ReadClusters property.

    protected const String ReadClustersKey =
        "ReadClusters";

    /// Name of the settings key for the ShowGraphLegend property.

    protected const String ShowGraphLegendKey =
        "ShowGraphLegend";

    /// Name of the settings key for the ShowGraphAxes property.

    protected const String ShowGraphAxesKey =
        "ShowGraphAxes";

    /// Name of the settings key for the AutoReadWorkbook property.

    protected const String AutoReadWorkbookKey =
        "AutoReadWorkbook";

    /// Name of the settings key for the NewWorkbookGraphDirectedness property.

    protected const String NewWorkbookGraphDirectednessKey =
        "NewWorkbookGraphDirectedness";

    /// Name of the settings key for the LabelFont property.

    protected const String LabelFontKey =
        "LabelFont";

    /// Name of the settings key for the AxisFont property.

    protected const String AxisFontKey =
        "AxisFont";

    /// Name of the settings key for the BackColor property.

    protected const String BackColorKey =
        "BackColor";

    /// Name of the settings key for the EdgeColor property.

    protected const String EdgeColorKey =
        "EdgeColor";

    /// Name of the settings key for the EdgeAlpha property.

    protected const String EdgeAlphaKey =
        "EdgeAlpha";

    /// Name of the settings key for the SelectedEdgeColor property.

    protected const String SelectedEdgeColorKey =
        "SelectedEdgeColor";

    /// Name of the settings key for the EdgeWidth property.

    protected const String EdgeWidthKey =
        "EdgeWidth";

    /// Name of the settings key for the SelectedEdgeWidth property.

    protected const String SelectedEdgeWidthKey =
        "SelectedEdgeWidth";

    /// Name of the settings key for the RelativeArrowSize property.

    protected const String RelativeArrowSizeKey =
        "RelativeArrowSize";

    /// Name of the settings key for the VertexShape property.

    protected const String VertexShapeKey =
        "VertexShape";

    /// Name of the settings key for the VertexRadius property.

    protected const String VertexRadiusKey =
        "VertexRadius";

    /// Name of the settings key for the VertexColor property.

    protected const String VertexColorKey =
        "VertexColor";

    /// Name of the settings key for the PrimaryLabelFillColor property.

    protected const String PrimaryLabelFillColorKey =
        "PrimaryLabelFillColor";

    /// Name of the settings key for the VertexAlpha property.

    protected const String VertexAlphaKey =
        "VertexAlpha";

    /// Name of the settings key for the SelectedVertexColor property.

    protected const String SelectedVertexColorKey =
        "SelectedVertexColor";

    /// Name of the settings key for the LayoutUserSettings property.

    protected const String LayoutUserSettingsKey =
        "LayoutUserSettings";

    /// Name of the settings key for the AutoSelect property.

    protected const String AutoSelectKey =
        "AutoSelect";

    /// Name of the settings key for the ClearTablesBeforeImport property.

    protected const String ClearTablesBeforeImportKey =
        "ClearTablesBeforeImport";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
