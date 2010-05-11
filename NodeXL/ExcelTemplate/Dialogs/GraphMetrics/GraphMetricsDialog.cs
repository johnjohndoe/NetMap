
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphMetricsDialog
//
/// <summary>
/// Edits a <see cref="GraphMetricUserSettings" /> object.
/// </summary>
///
/// <remarks>
/// Pass a <see cref="GraphMetricUserSettings" /> object to the constructor.
/// If the user edits the object, <see cref="Form.ShowDialog()" /> returns
/// DialogResult.OK.  Otherwise, the object is not modified and <see
/// cref="Form.ShowDialog()" /> returns DialogResult.Cancel.
/// </remarks>
//*****************************************************************************

public partial class GraphMetricsDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: GraphMetricsDialog()
    //
    /// <overloads>
    /// Initializes a new instance of the <see cref="GraphMetricsDialog" />
    /// class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphMetricsDialog" />
    /// class with a GraphMetricUserSettings object.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph contents.
    /// </param>
    ///
    /// <param name="graphMetricUserSettings">
    /// The object being edited.
    /// </param>
    //*************************************************************************

    public GraphMetricsDialog
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        GraphMetricUserSettings graphMetricUserSettings
    )
    : this()
    {
        Debug.Assert(workbook != null);
        Debug.Assert(graphMetricUserSettings != null);

        m_oWorkbook = workbook;
        m_oGraphMetricUserSettings = graphMetricUserSettings;

        // Instantiate an object that saves and retrieves the position of this
        // dialog.  Note that the object automatically saves the settings when
        // the form closes.

        m_oGraphMetricsDialogUserSettings =
            new GraphMetricsDialogUserSettings(this);

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: GraphMetricsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphMetricsDialog" />
    /// class for the Visual Studio designer.
    /// </summary>
    ///
    /// <remarks>
    /// Do not use this constructor.  It is for use by the Visual Studio
    /// designer only.
    /// </remarks>
    //*************************************************************************

    public GraphMetricsDialog()
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
        AssertValid();

        if (bFromControls)
        {
            m_oGraphMetricUserSettings.CalculateInDegree = chkInDegree.Checked;

            m_oGraphMetricUserSettings.CalculateOutDegree =
                chkOutDegree.Checked;

            m_oGraphMetricUserSettings.CalculateDegree = chkDegree.Checked;

            m_oGraphMetricUserSettings.CalculateBrandesFastCentralities =
                chkBrandesFastCentralities.Checked;

            m_oGraphMetricUserSettings.CalculateEigenvectorCentrality =
                chkEigenvectorCentrality.Checked;

            m_oGraphMetricUserSettings.CalculatePageRank = chkPageRank.Checked;

            m_oGraphMetricUserSettings.CalculateClusteringCoefficient =
                chkClusteringCoefficient.Checked;

            m_oGraphMetricUserSettings.CalculateOverallMetrics =
                chkOverallMetrics.Checked;
        }
        else
        {
            chkInDegree.Checked = m_oGraphMetricUserSettings.CalculateInDegree;

            chkOutDegree.Checked =
                m_oGraphMetricUserSettings.CalculateOutDegree;

            chkDegree.Checked = m_oGraphMetricUserSettings.CalculateDegree;

            chkBrandesFastCentralities.Checked =
                m_oGraphMetricUserSettings.CalculateBrandesFastCentralities;

            chkEigenvectorCentrality.Checked =
                m_oGraphMetricUserSettings.CalculateEigenvectorCentrality;

            chkPageRank.Checked = m_oGraphMetricUserSettings.CalculatePageRank;

            chkClusteringCoefficient.Checked =
                m_oGraphMetricUserSettings.CalculateClusteringCoefficient;

            chkOverallMetrics.Checked =
                m_oGraphMetricUserSettings.CalculateOverallMetrics;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: CheckAllCheckBoxes()
    //
    /// <summary>
    /// Checks or unchecks all the dialog's CheckBoxes.
    /// </summary>
    ///
    /// <param name="oControl">
    /// Control whose CheckBoxes should be checked or unchecked.
    /// </param>
    ///
    /// <param name="bCheck">
    /// true to check all CheckBoxes, false to uncheck them.
    /// </param>
    //*************************************************************************

    protected void
    CheckAllCheckBoxes
    (
        Control oControl,
        Boolean bCheck
    )
    {
        Debug.Assert(oControl != null);
        AssertValid();

        foreach (Control oChildControl in oControl.Controls)
        {
            if (oChildControl is CheckBox)
            {
                ( (CheckBox)oChildControl ).Checked = bCheck;
            }
            else
            {
                CheckAllCheckBoxes(oChildControl, bCheck);
            }
        }
    }

    //*************************************************************************
    //  Method: btnCheckAll_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnCheckAll button.
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
    btnCheckAll_Click
    (
        object sender,
        System.EventArgs e
    )
    {
        AssertValid();

        CheckAllCheckBoxes(this, true);
    }

    //*************************************************************************
    //  Method: btnUncheckAll_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnUncheckAll button.
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
    btnUncheckAll_Click
    (
        object sender,
        System.EventArgs e
    )
    {
        AssertValid();

        CheckAllCheckBoxes(this, false);
    }

    //*************************************************************************
    //  Method: lnkHelpLink_LinkClicked()
    //
    /// <summary>
    /// Handles the LinkClicked event on all of the help LinkLabels.
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
    lnkHelpLink_LinkClicked
    (
        object sender,
        LinkLabelLinkClickedEventArgs e
    )
    {
        AssertValid();

        // The help text is stored in the LinkLabel's Tag.

        Debug.Assert(sender is LinkLabel);
        Debug.Assert( ( (LinkLabel)sender ).Tag is String );

        this.ShowInformation( (String)( (LinkLabel)sender ).Tag );
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
        System.EventArgs e
    )
    {
        if ( !DoDataExchange(true) )
        {
            return;
        }

        if (!m_oGraphMetricUserSettings.AtLeastOneMetricSelected)
        {
            this.ShowInformation("No metrics have been selected.");

            return;
        }

        // The CalculateGraphMetricsDialog does all the work.  Use the
        // constructor overload that uses a default list of graph metric
        // calculators.

        CalculateGraphMetricsDialog oCalculateGraphMetricsDialog =
            new CalculateGraphMetricsDialog(m_oWorkbook,
                m_oGraphMetricUserSettings, new NotificationUserSettings()
                );

        if (oCalculateGraphMetricsDialog.ShowDialog() == DialogResult.OK)
        {
            DialogResult = DialogResult.OK;
            this.Close();
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

        Debug.Assert(m_oWorkbook != null);
        Debug.Assert(m_oGraphMetricUserSettings != null);
        Debug.Assert(m_oGraphMetricsDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Workbook containing the graph contents.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

    /// Object whose properties are being edited.

    protected GraphMetricUserSettings m_oGraphMetricUserSettings;

    /// User settings for this dialog.

    protected GraphMetricsDialogUserSettings m_oGraphMetricsDialogUserSettings;
}


//*****************************************************************************
//  Class: GraphMetricsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="GraphMetricsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("GraphMetricsDialog3") ]

public class GraphMetricsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: GraphMetricsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GraphMetricsDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public GraphMetricsDialogUserSettings
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
    //  Protected fields
    //*************************************************************************

    // (None.)
}
}
