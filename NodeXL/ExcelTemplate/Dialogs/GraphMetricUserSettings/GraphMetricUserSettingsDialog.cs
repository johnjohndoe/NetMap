
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphMetricUserSettingsDialog
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

public partial class GraphMetricUserSettingsDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: GraphMetricUserSettingsDialog()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
    /// cref="GraphMetricUserSettingsDialog" /> class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GraphMetricUserSettingsDialog" /> class with a
    /// GraphMetricUserSettings object.
    /// </summary>
    ///
    /// <param name="graphMetricUserSettings">
    /// The object being edited.
    /// </param>
    //*************************************************************************

    public GraphMetricUserSettingsDialog
    (
        GraphMetricUserSettings graphMetricUserSettings
    )
    : this()
    {
        Debug.Assert(graphMetricUserSettings != null);

        m_oGraphMetricUserSettings = graphMetricUserSettings;

        // Instantiate an object that saves and retrieves the position of this
        // dialog.  Note that the object automatically saves the settings when
        // the form closes.

        m_oGraphMetricUserSettingsDialogUserSettings =
            new GraphMetricUserSettingsDialogUserSettings(this);

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: GraphMetricUserSettingsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GraphMetricUserSettingsDialog" /> class for the Visual Studio
    /// designer.
    /// </summary>
    ///
    /// <remarks>
    /// Do not use this constructor.  It is for use by the Visual Studio
    /// designer only.
    /// </remarks>
    //*************************************************************************

    public GraphMetricUserSettingsDialog()
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
            m_oGraphMetricUserSettings.CalculateInDegree = cbxInDegree.Checked;

            m_oGraphMetricUserSettings.CalculateOutDegree =
                cbxOutDegree.Checked;

            m_oGraphMetricUserSettings.CalculateDegree = cbxDegree.Checked;

            m_oGraphMetricUserSettings.CalculateBetweennessCentrality =
                cbxBetweennessCentrality.Checked;

            m_oGraphMetricUserSettings.CalculateClosenessCentrality =
                cbxClosenessCentrality.Checked;

            m_oGraphMetricUserSettings.CalculateEigenvectorCentrality =
                cbxEigenvectorCentrality.Checked;

            m_oGraphMetricUserSettings.CalculateClusteringCoefficient =
                cbxClusteringCoefficient.Checked;

            m_oGraphMetricUserSettings.CalculateOverallMetrics =
                cbxOverallMetrics.Checked;
        }
        else
        {
            cbxInDegree.Checked = m_oGraphMetricUserSettings.CalculateInDegree;

            cbxOutDegree.Checked =
                m_oGraphMetricUserSettings.CalculateOutDegree;

            cbxDegree.Checked = m_oGraphMetricUserSettings.CalculateDegree;

            cbxBetweennessCentrality.Checked =
                m_oGraphMetricUserSettings.CalculateBetweennessCentrality;

            cbxClosenessCentrality.Checked =
                m_oGraphMetricUserSettings.CalculateClosenessCentrality;

            cbxEigenvectorCentrality.Checked =
                m_oGraphMetricUserSettings.CalculateEigenvectorCentrality;

            cbxClusteringCoefficient.Checked =
                m_oGraphMetricUserSettings.CalculateClusteringCoefficient;

            cbxOverallMetrics.Checked =
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
        if ( DoDataExchange(true) )
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

        Debug.Assert(m_oGraphMetricUserSettings != null);
        Debug.Assert(m_oGraphMetricUserSettingsDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object whose properties are being edited.

    protected GraphMetricUserSettings m_oGraphMetricUserSettings;

    /// User settings for this dialog.

    protected GraphMetricUserSettingsDialogUserSettings
        m_oGraphMetricUserSettingsDialogUserSettings;
}


//*****************************************************************************
//  Class: GraphMetricUserSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="GraphMetricUserSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("GraphMetricUserSettingsDialog2") ]

public class GraphMetricUserSettingsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: GraphMetricUserSettingsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GraphMetricUserSettingsDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public GraphMetricUserSettingsDialogUserSettings
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
