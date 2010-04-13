using System;
using System.Configuration;
using System.Windows.Forms;
using System.Linq;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Visualization.Wpf;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: SelectSubgraphsDialog
//
/// <summary>
/// Allows the user to select subgraphs for one or more of the graph's
/// vertices.
/// </summary>
///
/// <remarks>
/// Call <see cref="Form.ShowDialog()" /> to run the dialog.  All manipulation
/// of the graph in the NodeXLControl is handled by the dialog.
/// </remarks>
//*****************************************************************************

public partial class SelectSubgraphsDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: SelectSubgraphsDialog()
    //
    /// <overloads>
    /// Initializes a new instance of the <see cref="SelectSubgraphsDialog" />
    /// class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see cref="SelectSubgraphsDialog" />
    /// class with a NodeXLControl.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
    /// The NodeXLControl whose subgraphs should be selected.
    /// </param>
    ///
    /// <param name="clickedVertex">
    /// Vertex that was clicked in the graph, or null if no vertex was clicked.
    /// </param>
    //*************************************************************************

    public SelectSubgraphsDialog
    (
        NodeXLControl nodeXLControl,
        IVertex clickedVertex
    )
    : this()
    {
        // Instantiate an object that retrieves and saves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oSelectSubgraphsDialogUserSettings =
            new SelectSubgraphsDialogUserSettings(this);

        m_oNodeXLControl = nodeXLControl;
        m_oClickedVertex = clickedVertex;

        m_aoInitiallySelectedVertices =
            nodeXLControl.SelectedVertices.ToArray();

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: SelectSubgraphsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="SelectSubgraphsDialog" />
    /// class for the Visual Studio designer.
    /// </summary>
    ///
    /// <remarks>
    /// Do not use this constructor.  It is for use by the Visual Studio
    /// designer only.
    /// </remarks>
    //*************************************************************************

    public SelectSubgraphsDialog()
    {
        InitializeComponent();

        // AssertValid();
    }

    //*************************************************************************
    //  Method: DoDataExchange()
    //
    /// <summary>
    /// Transfers data between the dialog's fields and its controls.
    /// </summary>
    ///
    /// <param name="bFromControls">
    /// true to transfer data from the dialog's controls to its fields, false
    /// for the other direction.
    /// </param>
    ///
    /// <returns>
    /// true if the transfer was successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    DoDataExchange
    (
        Boolean bFromControls
    )
    {
        if (bFromControls)
        {
            m_oSelectSubgraphsDialogUserSettings.UseAllSelectedVertices =
                radUseAllSelectedVertices.Checked;

            m_oSelectSubgraphsDialogUserSettings.Levels =
                usrSubgraphLevels.Levels;

            m_oSelectSubgraphsDialogUserSettings.SelectConnectingEdges =
                cbxSelectConnectingEdges.Checked;
        }
        else
        {
            if (m_oClickedVertex == null)
            {
                radUseAllSelectedVertices.Checked = true;
                radUseClickedVertexOnly.Enabled = false;
            }
            else
            {
                radUseAllSelectedVertices.Checked =
                    m_oSelectSubgraphsDialogUserSettings.
                        UseAllSelectedVertices;

                radUseClickedVertexOnly.Enabled = true;
            }

            radUseClickedVertexOnly.Checked =
                !radUseAllSelectedVertices.Checked;

            usrSubgraphLevels.Levels =
                m_oSelectSubgraphsDialogUserSettings.Levels;

            cbxSelectConnectingEdges.Checked =
                m_oSelectSubgraphsDialogUserSettings.SelectConnectingEdges;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: SelectSubgraphs()
    //
    /// <summary>
    /// Selects the subgraphs specified by the user.
    /// </summary>
    ///
    /// <param name="oNodeXLControl">
    /// The NodeXLControl whose subgraphs should be selected.
    /// </param>
    ///
    /// <param name="oClickedVertex">
    /// Vertex that was clicked in the graph, or null if no vertex was clicked.
    /// </param>
    ///
    /// <param name="bUseAllSelectedVertices">
    /// true to select subgraphs for all initially selected vertices, false to
    /// select the subgraph for the clicked vertex only.
    /// </param>
    ///
    /// <param name="decLevels">
    /// The number of levels to select in each subgraph.
    /// </param>
    ///
    /// <param name="bSelectConnectingEdges">
    /// true to select the subgraphs' connecting edges.
    /// </param>
    //*************************************************************************

    protected void
    SelectSubgraphs
    (
        NodeXLControl oNodeXLControl,
        IVertex oClickedVertex,
        Boolean bUseAllSelectedVertices,
        Decimal decLevels,
        Boolean bSelectConnectingEdges
    )
    {
        Debug.Assert(oNodeXLControl != null);
        Debug.Assert(bUseAllSelectedVertices || oClickedVertex != null);
        Debug.Assert(decLevels > 0);
        AssertValid();

        // Determine which vertices need to have their subgraphs selected.

        IVertex [] aoVertices;

        if (bUseAllSelectedVertices)
        {
            aoVertices = m_aoInitiallySelectedVertices;
        }
        else
        {
            Debug.Assert(oClickedVertex != null);

            aoVertices = new IVertex [] {oClickedVertex};
        }

        // Select the vertices' subgraphs.

        NodeXLControlUtil.SelectSubgraphs(oNodeXLControl, aoVertices,
            decLevels, bSelectConnectingEdges);
    }

    //*************************************************************************
    //  Method: btnSelect_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnSelect button.
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
    btnSelect_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        if ( !DoDataExchange(true) )
        {
            return;
        }

        this.UseWaitCursor = true;

        SelectSubgraphs(
            m_oNodeXLControl, m_oClickedVertex,
            m_oSelectSubgraphsDialogUserSettings.UseAllSelectedVertices,
            m_oSelectSubgraphsDialogUserSettings.Levels,
            m_oSelectSubgraphsDialogUserSettings.SelectConnectingEdges
            );

        this.UseWaitCursor = false;
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

        Debug.Assert(m_oSelectSubgraphsDialogUserSettings != null);
        Debug.Assert(m_oNodeXLControl != null);
        // m_oClickedVertex
        Debug.Assert(m_aoInitiallySelectedVertices != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// User settings for this dialog.

    protected SelectSubgraphsDialogUserSettings
        m_oSelectSubgraphsDialogUserSettings;

    /// The NodeXLControl whose vertices and edges should be selected.

    protected NodeXLControl m_oNodeXLControl;

    /// Vertex that was clicked in the graph, or null if no vertex was clicked.

    protected IVertex m_oClickedVertex;

    /// Array of zero or more vertices that were selected when the dialog was
    /// opened.

    protected IVertex [] m_aoInitiallySelectedVertices;
}


//*****************************************************************************
//  Class: SelectSubgraphsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="SelectSubgraphsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("SelectSubgraphsDialog") ]

public class SelectSubgraphsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: SelectSubgraphsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="SelectSubgraphsDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public SelectSubgraphsDialogUserSettings
    (
        Form oForm
    )
    : base (oForm, true)
    {
        Debug.Assert(oForm != null);

        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: UseAllSelectedVertices
    //
    /// <summary>
    /// Gets or sets a flag indicating whether subgraphs should be selected for
    /// all initially selected vertices.
    /// </summary>
    ///
    /// <value>
    /// true to select subgraphs for all initially selected vertices, false to
    /// select a subgraph for the clicked vertex only.  The default is true.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("true") ]

    public Boolean
    UseAllSelectedVertices
    {
        get
        {
            AssertValid();

            Boolean bUseAllSelectedVertices =
                (Boolean)this[UseAllSelectedVerticesKey];

            return (bUseAllSelectedVertices);
        }

        set
        {
            this[UseAllSelectedVerticesKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Levels
    //
    /// <summary>
    /// Gets or sets the number of levels of adjacent vertices to include in
    /// each subgraph.
    /// </summary>
    ///
    /// <value>
    /// The number of levels of adjacent vertices to include in each subgraph.
    /// The default is 1.0.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("1.0") ]

    public Decimal
    Levels
    {
        get
        {
            AssertValid();

            return ( (Decimal)this[LevelsKey] );
        }

        set
        {
            this[LevelsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SelectConnectingEdges
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the subgraphs' connecting edges
    /// should be selected.
    /// </summary>
    ///
    /// <value>
    /// true to select the subgraphs' connecting edges.  The default is true.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("true") ]

    public Boolean
    SelectConnectingEdges
    {
        get
        {
            AssertValid();

            Boolean bSelectConnectingEdges =
                (Boolean)this[SelectConnectingEdgesKey];

            return (bSelectConnectingEdges);
        }

        set
        {
            this[SelectConnectingEdgesKey] = value;

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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Name of the settings key for the UseAllSelectedVertices property.

    protected const String UseAllSelectedVerticesKey =
        "UseAllSelectedVertices";

    /// Name of the settings key for the Levels property.

    protected const String LevelsKey = "Levels";

    /// Name of the settings key for the SelectConnectingEdges property.

    protected const String SelectConnectingEdgesKey = "SelectConnectingEdges";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}
}
