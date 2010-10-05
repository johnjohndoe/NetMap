

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AutoFillWorkbookWithSchemeDialog
//
/// <summary>
/// Automatically fills visual attribute columns with a scheme.
/// </summary>
///
/// <remarks>
/// This dialog fills the edge and vertex attribute columns with a
/// user-specified scheme.  A <see cref="WorkbookSchemeAutoFiller" /> object
/// does most of the work.
/// </remarks>
//*****************************************************************************

public partial class AutoFillWorkbookWithSchemeDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: AutoFillWorkbookWithSchemeDialog()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
    /// cref="AutoFillWorkbookWithSchemeDialog" /> class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AutoFillWorkbookWithSchemeDialog" /> class with a workbook.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph data.
    /// </param>
    //*************************************************************************

    public AutoFillWorkbookWithSchemeDialog
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    : this()
    {
        Debug.Assert(workbook != null);

        m_oWorkbook = workbook;

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oAutoFillWorkbookWithSchemeDialogUserSettings =
            new AutoFillWorkbookWithSchemeDialogUserSettings(this);

        // Populate the column ComboBoxes with the names of the table columns
        // that can be used.

        ListObject oTable;

        if ( ExcelUtil.TryGetTable(m_oWorkbook, WorksheetNames.Vertices,
            TableNames.Vertices, out oTable) )
        {
            cbxVertexCategoryColumnName.PopulateWithTableColumnNames(oTable);
            cbxVertexLabelColumnName.PopulateWithTableColumnNames(oTable);

            cbxVertexLabelColumnName.Items.Add(
                VertexTableColumnNames.VertexName);
        }

        if ( ExcelUtil.TryGetTable(m_oWorkbook, WorksheetNames.Edges,
            TableNames.Edges, out oTable) )
        {
            cbxEdgeWeightColumnName.PopulateWithTableColumnNames(oTable);
            cbxEdgeTimestampColumnName.PopulateWithTableColumnNames(oTable);
        }

        DoDataExchange(false);

        // Select default column names if the user settings didn't specify
        // other column names.
        //
        // Note that because the DropDownStyle of all the ComboBoxes is set to
        // DropDownList, setting the ComboBox text to a column name that is not
        // currently in the workbook leaves the text empty.

        cbxVertexCategoryColumnName.SetTextIfEmpty(
            DefaultVertexCategoryColumnName);

        cbxEdgeWeightColumnName.SetTextIfEmpty(DefaultEdgeWeightColumnName);

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: AutoFillWorkbookWithSchemeDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AutoFillWorkbookWithSchemeDialog" /> class for the Visual Studio
    /// designer.
    /// </summary>
    ///
    /// <remarks>
    /// Do not use this constructor.  It is for use by the Visual Studio
    /// designer only.
    /// </remarks>
    //*************************************************************************

    public AutoFillWorkbookWithSchemeDialog()
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
            VisualScheme eScheme = VisualScheme.VertexCategories;
            ComboBoxPlus oComboBoxToCheck = null;
            String sColumnType = null;

            // Perform error checking.

            if (radVertexCategories.Checked)
            {
                oComboBoxToCheck = cbxVertexCategoryColumnName;
                sColumnType = "category";
            }
            else if (radEdgeWeights.Checked)
            {
                eScheme = VisualScheme.EdgeWeights;
                oComboBoxToCheck = cbxEdgeWeightColumnName;
                sColumnType = "weight";
            }
            else if (radEdgeTimestamps.Checked)
            {
                eScheme = VisualScheme.EdgeTimestamps;
                oComboBoxToCheck = cbxEdgeTimestampColumnName;
                sColumnType = "timestamp";
            }
            else
            {
                Debug.Assert(false);
            }

            Boolean bShowVertexLabels = chkShowVertexLabels.Checked;
            String sSchemeColumnName = null;
            String sVertexLabelColumnName = null;

            if (oComboBoxToCheck.Items.Count == 0)
            {
                OnInvalidComboBox(oComboBoxToCheck,
                    "There are no columns that can be used for that scheme.");

                return (false);
            }

            if (
                !ValidateRequiredComboBox(oComboBoxToCheck,
                    "Select a " + sColumnType + " column.",
                    out sSchemeColumnName)
                ||
                ( bShowVertexLabels && !ValidateRequiredComboBox(
                    cbxVertexLabelColumnName,
                    "Select a label column.",
                    out sVertexLabelColumnName) )
                )
            {
                return (false);
            }

            // Error checking passed.  Transfer the data.

            m_oAutoFillWorkbookWithSchemeDialogUserSettings.Scheme = eScheme;

            switch (eScheme)
            {
                case VisualScheme.VertexCategories:

                    m_oAutoFillWorkbookWithSchemeDialogUserSettings.
                        VertexCategoryColumnName = sSchemeColumnName;

                    break;

                case VisualScheme.EdgeWeights:

                    m_oAutoFillWorkbookWithSchemeDialogUserSettings.
                        EdgeWeightColumnName = sSchemeColumnName;

                    break;

                case VisualScheme.EdgeTimestamps:

                    m_oAutoFillWorkbookWithSchemeDialogUserSettings.
                        EdgeTimestampColumnName = sSchemeColumnName;

                    break;

                default:

                    Debug.Assert(false);
                    break;
            }

            m_oAutoFillWorkbookWithSchemeDialogUserSettings.ShowVertexLabels =
                bShowVertexLabels;

            if (bShowVertexLabels)
            {
                m_oAutoFillWorkbookWithSchemeDialogUserSettings.
                    VertexLabelColumnName = sVertexLabelColumnName;
            }
        }
        else
        {
            switch (m_oAutoFillWorkbookWithSchemeDialogUserSettings.Scheme)
            {
                case VisualScheme.VertexCategories:

                    radVertexCategories.Checked = true;
                    break;

                case VisualScheme.EdgeWeights:

                    radEdgeWeights.Checked = true;
                    break;

                case VisualScheme.EdgeTimestamps:

                    radEdgeTimestamps.Checked = true;
                    break;

                default:

                    Debug.Assert(false);
                    break;
            }

            cbxVertexCategoryColumnName.Text =
                m_oAutoFillWorkbookWithSchemeDialogUserSettings.
                    VertexCategoryColumnName;

            cbxEdgeWeightColumnName.Text =
                m_oAutoFillWorkbookWithSchemeDialogUserSettings.
                EdgeWeightColumnName;

            cbxEdgeTimestampColumnName.Text =
                m_oAutoFillWorkbookWithSchemeDialogUserSettings.
                EdgeTimestampColumnName;

            chkShowVertexLabels.Checked =
                m_oAutoFillWorkbookWithSchemeDialogUserSettings.
                ShowVertexLabels;

            cbxVertexLabelColumnName.Text =
                m_oAutoFillWorkbookWithSchemeDialogUserSettings.
                VertexLabelColumnName;

            EnableControls();
        }

        return (true);
    }

    //*************************************************************************
    //  Method: EnableControls()
    //
    /// <summary>
    /// Enables or disables the dialog's controls.
    /// </summary>
    //*************************************************************************

    protected void
    EnableControls()
    {
        AssertValid();

        cbxVertexCategoryColumnName.Enabled =
            lblVertexCategoryColumnName.Enabled =
            radVertexCategories.Checked;

        cbxEdgeWeightColumnName.Enabled = 
            lblEdgeWeightColumnName.Enabled =
            radEdgeWeights.Checked;

        cbxEdgeTimestampColumnName.Enabled = 
            lblEdgeTimestampColumnName.Enabled =
            radEdgeTimestamps.Checked;

        cbxVertexLabelColumnName.Enabled =
            lblVertexLabelColumnName.Enabled =
            chkShowVertexLabels.Checked;
    }

    //*************************************************************************
    //  Method: AutoFillWorkbookWithScheme()
    //
    /// <summary>
    /// Fills visual attribute columns with a scheme.
    /// </summary>
    ///
    /// <remarks>
    /// It's assumed that DoDataExchange(true) has been called and that it
    /// returned true.
    /// </remarks>
    //*************************************************************************

    protected void
    AutoFillWorkbookWithScheme()
    {
        AssertValid();

        Boolean bShowVertexLabels =
            m_oAutoFillWorkbookWithSchemeDialogUserSettings.ShowVertexLabels;

        String sVertexLabelColumnName =
            m_oAutoFillWorkbookWithSchemeDialogUserSettings.
                VertexLabelColumnName;

        switch (m_oAutoFillWorkbookWithSchemeDialogUserSettings.Scheme)
        {
            case VisualScheme.VertexCategories:

                WorkbookSchemeAutoFiller.AutoFillByVertexCategory(m_oWorkbook,

                    m_oAutoFillWorkbookWithSchemeDialogUserSettings.
                        VertexCategoryColumnName,

                    bShowVertexLabels, sVertexLabelColumnName
                    );

                break;

            case VisualScheme.EdgeWeights:

                WorkbookSchemeAutoFiller.AutoFillByEdgeWeight(m_oWorkbook,

                    m_oAutoFillWorkbookWithSchemeDialogUserSettings.
                        EdgeWeightColumnName,

                    bShowVertexLabels, sVertexLabelColumnName
                    );

                break;

            case VisualScheme.EdgeTimestamps:

                WorkbookSchemeAutoFiller.AutoFillByEdgeTimestamp(m_oWorkbook,

                    m_oAutoFillWorkbookWithSchemeDialogUserSettings.
                        EdgeTimestampColumnName,

                    bShowVertexLabels, sVertexLabelColumnName
                    );

                break;

            default:

                Debug.Assert(false);
                break;
        }
    }

    //*************************************************************************
    //  Method: OnEventThatRequiresControlEnabling()
    //
    /// <summary>
    /// Handles any event that should changed the enabled state of the dialog's
    /// controls.
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
    OnEventThatRequiresControlEnabling
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        if (sender is PictureBox)
        {
            // Clicking a PictureBox should check the corresponding
            // RadioButton.
            //
            // Sample PictureBox name: "picVertexCategories"
            //
            // Sample corresponding RadioButton name: "radVertexCategories"

            ( (RadioButton)pnlVisualSchemes.Controls[
                ( (PictureBox)sender ).Name.Replace("pic", "rad") ] ).Checked =
                true;
        }

        EnableControls();
    }

    //*************************************************************************
    //  Method: btnOK_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnOK button.
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
    btnOK_Click
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

        try
        {
            AutoFillWorkbookWithScheme();
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
            return;
        }

        this.DialogResult = DialogResult.OK;
        this.Close();
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

        Debug.Assert(m_oWorkbook != null);
        Debug.Assert(m_oAutoFillWorkbookWithSchemeDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Vertex category column name to select the first time the dialog is
    /// opened, if the column exists.

    protected const String DefaultVertexCategoryColumnName = "Category";

    /// Edge weight column name to select the first time the dialog is opened,
    /// if the column exists.

    protected const String DefaultEdgeWeightColumnName =
        EdgeTableColumnNames.EdgeWeight;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Workbook containing the graph data.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

    /// User settings for this dialog.

    protected AutoFillWorkbookWithSchemeDialogUserSettings
        m_oAutoFillWorkbookWithSchemeDialogUserSettings;
}


//*****************************************************************************
//  Enum: VisualScheme
//
/// <summary>
/// Specifies a visual scheme to apply to the graph.
/// </summary>
//*****************************************************************************

public enum
VisualScheme
{
    /// <summary>
    /// Vertices are grouped by category and all vertices in a category have
    /// the same shape, color, and size.
    /// </summary>

    VertexCategories,

    /// <summary>
    /// Edges have weights and the width of each edge is proportional to its
    /// weight.
    /// </summary>

    EdgeWeights,

    /// <summary>
    /// Edges have timestamps and the color of each edge is based on the
    /// timestamp.
    /// </summary>

    EdgeTimestamps,
}


//*****************************************************************************
//  Class: AutoFillWorkbookWithSchemeDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="AutoFillWorkbookWithSchemeDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("AutoFillWorkbookWithSchemeDialog") ]

public class AutoFillWorkbookWithSchemeDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: AutoFillWorkbookWithSchemeDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AutoFillWorkbookWithSchemeDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public AutoFillWorkbookWithSchemeDialogUserSettings
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
    //  Property: Scheme
    //
    /// <summary>
    /// Gets or sets a value specifying the visual scheme to apply to the
    /// graph.
    /// </summary>
    ///
    /// <value>
    /// The visual scheme to apply to the graph.  The default is
    /// VertexCategories.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("VertexCategories") ]

    public VisualScheme
    Scheme
    {
        get
        {
            AssertValid();

            return ( (VisualScheme)this[SchemeKey] );
        }

        set
        {
            this[SchemeKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexCategoryColumnName
    //
    /// <summary>
    /// Gets or sets the name of the vertex table column containing vertex
    /// categories.
    /// </summary>
    ///
    /// <value>
    /// The name of the vertex table column containing vertex categories.  The
    /// default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    VertexCategoryColumnName
    {
        get
        {
            AssertValid();

            return ( (String)this[VertexCategoryColumnNameKey] );
        }

        set
        {
            this[VertexCategoryColumnNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeWeightColumnName
    //
    /// <summary>
    /// Gets or sets the name of the edge table column containing edge weights.
    /// </summary>
    ///
    /// <value>
    /// The name of the edge table column containing edge weights.  The default
    /// is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    EdgeWeightColumnName
    {
        get
        {
            AssertValid();

            return ( (String)this[EdgeWeightColumnNameKey] );
        }

        set
        {
            this[EdgeWeightColumnNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeTimestampColumnName
    //
    /// <summary>
    /// Gets or sets the name of the edge table column containing edge
    /// timestamps.
    /// </summary>
    ///
    /// <value>
    /// The name of the edge table column containing edge timestamps.  The
    /// default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    EdgeTimestampColumnName
    {
        get
        {
            AssertValid();

            return ( (String)this[EdgeTimestampColumnNameKey] );
        }

        set
        {
            this[EdgeTimestampColumnNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ShowVertexLabels
    //
    /// <summary>
    /// Gets or sets a flag specifying whether vertex labels should be shown.
    /// </summary>
    ///
    /// <value>
    /// true to show vertex labels.  The default is false.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("false") ]

    public Boolean
    ShowVertexLabels
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[ShowVertexLabelsKey] );
        }

        set
        {
            this[ShowVertexLabelsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexLabelColumnName
    //
    /// <summary>
    /// Gets or sets the name of the vertex table column containing vertex
    /// labels.
    /// </summary>
    ///
    /// <value>
    /// The name of the vertex table column containing vertex labels.  The
    /// default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    VertexLabelColumnName
    {
        get
        {
            AssertValid();

            return ( (String)this[VertexLabelColumnNameKey] );
        }

        set
        {
            this[VertexLabelColumnNameKey] = value;

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

    /// Name of the settings key for the Scheme property.

    protected const String SchemeKey = "Scheme";

    /// Name of the settings key for the VertexCategoryColumnName property.

    protected const String VertexCategoryColumnNameKey =
        "VertexCategoryColumnName";

    /// Name of the settings key for the EdgeWeightColumnName property.

    protected const String EdgeWeightColumnNameKey = "EdgeWeightColumnName";

    /// Name of the settings key for the EdgeTimestampColumnName property.

    protected const String EdgeTimestampColumnNameKey =
        "EdgeTimestampColumnName";

    /// Name of the settings key for the ShowVertexLabels property.

    protected const String ShowVertexLabelsKey = "ShowVertexLabels";

    /// Name of the settings key for the VertexLabelColumnName property.

    protected const String VertexLabelColumnNameKey = "VertexLabelColumnName";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}
}
