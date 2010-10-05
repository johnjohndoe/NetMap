
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphMetricsDialog
//
/// <summary>
/// Edits a <see cref="GraphMetricUserSettings" /> object and calculates the
/// metrics specified in the object.
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
    ///
    /// <param name="mode">
    /// Indicates the mode in which the dialog is being used.
    /// </param>
    //*************************************************************************

    public GraphMetricsDialog
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        GraphMetricUserSettings graphMetricUserSettings,
        DialogMode mode
    )
    : this()
    {
        Debug.Assert(workbook != null);
        Debug.Assert(graphMetricUserSettings != null);

        m_oWorkbook = workbook;
        m_oGraphMetricUserSettings = graphMetricUserSettings;
        m_eMode = mode;

        if (m_eMode == DialogMode.EditOnly)
        {
            this.Text += " Options";
            btnOK.Text = "OK";
        }

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
    //  Enum: DialogMode
    //
    /// <summary>
    /// Indicates the mode in which the dialog is being used.
    /// </summary>
    //*************************************************************************

    public enum
    DialogMode
    {
        /// The user can edit the dialog settings and then calculate metrics.

        Normal,

        /// The user can edit the dialog settings but cannot calculate metrics.

        EditOnly,
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

        GraphMetrics eGraphMetricsToCalculate;

        if (bFromControls)
        {
            eGraphMetricsToCalculate = GraphMetrics.None;

            // An GraphMetrics flag is stored in the Tag of each CheckBox.

            foreach ( CheckBox oCheckBox in GetGraphMetricCheckBoxes() )
            {
                if (oCheckBox.Checked)
                {
                    eGraphMetricsToCalculate |= (GraphMetrics)oCheckBox.Tag;
                }
            }

            m_oGraphMetricUserSettings.GraphMetricsToCalculate =
                eGraphMetricsToCalculate;
        }
        else
        {
            eGraphMetricsToCalculate =
                m_oGraphMetricUserSettings.GraphMetricsToCalculate;

            foreach ( CheckBox oCheckBox in GetGraphMetricCheckBoxes() )
            {
                oCheckBox.Checked =
                    (eGraphMetricsToCalculate & (GraphMetrics)oCheckBox.Tag)
                    != 0;
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: GetGraphMetricCheckBoxes()
    //
    /// <summary>
    /// Gets an enumerator for enumerating the CheckBox controls that represent
    /// graph metrics.
    /// </summary>
    ///
    /// <returns>
    /// An enumerator for enumerating the CheckBox controls.
    /// </returns>
    //*************************************************************************

    protected IEnumerable<CheckBox>
    GetGraphMetricCheckBoxes()
    {
        AssertValid();

        foreach (Control oControl in this.Controls)
        {
            if (oControl is GroupBox)
            {
                foreach (Control oGroupBoxChild in oControl.Controls)
                {
                    if (oGroupBoxChild is CheckBox &&
                        oGroupBoxChild.Tag is GraphMetrics)
                    {
                        yield return ( (CheckBox)oGroupBoxChild );
                    }
                }
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
    //  Method: lnkDuplicateEdges_LinkClicked()
    //
    /// <summary>
    /// Handles the LinkClicked event on the lnkDuplicateEdge LinkLabel.
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
    lnkDuplicateEdges_LinkClicked
    (
        object sender,
        LinkLabelLinkClickedEventArgs e
    )
    {
        AssertValid();

        this.ShowInformation(
            "Duplicate edges are skipped when graph metrics are calculated,"
            + " except for the edge counts within the overall metrics."
            + "\r\n\r\n"
            + "If you want to permanently merge duplicate edges within the"
            + " workbook, use NodeXL, Data, Prepare Data, Merge Duplicate"
            + " Edges."
            );
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

        if (m_oGraphMetricUserSettings.GraphMetricsToCalculate ==
            GraphMetrics.None)
        {
            this.ShowInformation("No metrics have been selected.");
            return;
        }

        if (m_eMode == DialogMode.EditOnly)
        {
            DialogResult = DialogResult.OK;
            this.Close();
            return;
        }

        // The CalculateGraphMetricsDialog does all the work.  Use the
        // constructor overload that uses a default list of graph metric
        // calculators.

        CalculateGraphMetricsDialog oCalculateGraphMetricsDialog =
            new CalculateGraphMetricsDialog(m_oWorkbook,
                m_oGraphMetricUserSettings);

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
        // m_eMode
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

    /// Indicates the mode in which the dialog is being used.

    protected DialogMode m_eMode;
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

[ SettingsGroupNameAttribute("GraphMetricsDialog5") ]

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
