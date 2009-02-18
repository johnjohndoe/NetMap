
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.NodeXL.Visualization.Wpf;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GeneralUserSettingsDialog
//
/// <summary>
/// Edits a <see cref="GeneralUserSettings" /> object.
/// </summary>
///
/// <remarks>
/// Pass a <see cref="GeneralUserSettings" /> object to the constructor.  If
/// the user edits the object, <see cref="Form.ShowDialog()" /> returns
/// DialogResult.OK.  Otherwise, the object is not modified and <see
/// cref="Form.ShowDialog()" /> returns DialogResult.Cancel.
/// </remarks>
//*****************************************************************************

public partial class GeneralUserSettingsDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: GeneralUserSettingsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GeneralUserSettingsDialog" /> class.
    /// </summary>
    ///
    /// <param name="generalUserSettings">
    /// The object being edited.
    /// </param>
    //*************************************************************************

    public GeneralUserSettingsDialog
    (
        GeneralUserSettings generalUserSettings
    )
    {
        Debug.Assert(generalUserSettings != null);
        generalUserSettings.AssertValid();

        m_oGeneralUserSettings = generalUserSettings;
        m_oFont = m_oGeneralUserSettings.Font;
        m_oLayoutUserSettings = m_oGeneralUserSettings.LayoutUserSettings;

        // Instantiate an object that saves and retrieves the position of this
        // dialog.  Note that the object automatically saves the settings when
        // the form closes.

        m_oGeneralUserSettingsDialogUserSettings =
            new GeneralUserSettingsDialogUserSettings(this);

        InitializeComponent();

        Object[] oAllGraphAndWorkbookValues =
            ( new ColorConverter2() ).GetAllGraphAndWorkbookValues(false);

        cbxBackColor.PopulateWithObjectsAndText(oAllGraphAndWorkbookValues);
        cbxVertexColor.PopulateWithObjectsAndText(oAllGraphAndWorkbookValues);

        cbxPrimaryLabelFillColor.PopulateWithObjectsAndText(
            oAllGraphAndWorkbookValues);

        cbxSelectedVertexColor.PopulateWithObjectsAndText(
            oAllGraphAndWorkbookValues);

        cbxEdgeColor.PopulateWithObjectsAndText(oAllGraphAndWorkbookValues);

        cbxSelectedEdgeColor.PopulateWithObjectsAndText(
            oAllGraphAndWorkbookValues);

        nudEdgeWidth.Minimum =
            (Decimal)EdgeWidthConverter.MinimumWidthWorkbook;

        nudEdgeWidth.Maximum =
            (Decimal)EdgeWidthConverter.MaximumWidthWorkbook;

        nudSelectedEdgeWidth.Minimum =
            (Decimal)EdgeWidthConverter.MinimumWidthWorkbook;

        nudSelectedEdgeWidth.Maximum =
            (Decimal)EdgeWidthConverter.MaximumWidthWorkbook;

        nudRelativeArrowSize.Minimum =
            (Decimal)EdgeDrawer.MinimumRelativeArrowSize;

        nudRelativeArrowSize.Maximum =
            (Decimal)EdgeDrawer.MaximumRelativeArrowSize;

        nudVertexRadius.Minimum =
            (Decimal)VertexRadiusConverter.MinimumRadiusWorkbook;

        nudVertexRadius.Maximum = 
            (Decimal)VertexRadiusConverter.MaximumRadiusWorkbook;

        ( new VertexShapeConverter() ).PopulateComboBox(cbxVertexShape, false);

        nudVertexAlpha.Minimum = nudEdgeAlpha.Minimum =
            nudFilteredAlpha.Minimum =
            (Decimal)AlphaConverter.MinimumAlphaWorkbook;

        nudVertexAlpha.Maximum = nudEdgeAlpha.Maximum =
            nudFilteredAlpha.Maximum =
            (Decimal)AlphaConverter.MaximumAlphaWorkbook;

        lblVertexAlphaMessage.Text = lblEdgeAlphaMessage.Text =
            lblFilteredAlphaMessage.Text =
            AlphaConverter.MaximumAlphaMessage;

        DoDataExchange(false);

        AssertValid();
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
            Single fEdgeWidth, fSelectedEdgeWidth, fRelativeArrowSize,
                fVertexRadius, fVertexAlpha, fEdgeAlpha, fFilteredAlpha;

            if (
                !ValidateNumericUpDown(nudVertexRadius,
                    "a vertex radius", out fVertexRadius)
                ||
                !ValidateNumericUpDown(nudVertexAlpha, "a vertex opacity",
                    out fVertexAlpha)
                ||
                !ValidateNumericUpDown(nudEdgeWidth,
                    "a width for unselected edges", out fEdgeWidth)
                ||
                !ValidateNumericUpDown(nudEdgeAlpha, "an edge opacity",
                    out fEdgeAlpha)
                ||
                !ValidateNumericUpDown(nudRelativeArrowSize,
                    "an arrow size", out fRelativeArrowSize)
                ||
                !ValidateNumericUpDown(nudSelectedEdgeWidth,
                    "a width for selected edges", out fSelectedEdgeWidth)
                ||
                !ValidateNumericUpDown(nudFilteredAlpha,
                    "an opacity for dynamically filtered vertices and edges",
                    out fFilteredAlpha)
                )
            {
                return (false);
            }

            m_oGeneralUserSettings.BackColor =
                (KnownColor)cbxBackColor.SelectedValue;

            m_oGeneralUserSettings.FilteredAlpha = fFilteredAlpha;

            m_oGeneralUserSettings.EdgeWidth = fEdgeWidth;
            m_oGeneralUserSettings.SelectedEdgeWidth = fSelectedEdgeWidth;

            m_oGeneralUserSettings.RelativeArrowSize = fRelativeArrowSize;

            m_oGeneralUserSettings.EdgeColor =
                (KnownColor)cbxEdgeColor.SelectedValue;

            m_oGeneralUserSettings.EdgeAlpha = (Single)nudEdgeAlpha.Value;

            m_oGeneralUserSettings.SelectedEdgeColor =
                (KnownColor)cbxSelectedEdgeColor.SelectedValue;

            m_oGeneralUserSettings.VertexShape =
                (VertexShape)cbxVertexShape.SelectedValue;

            m_oGeneralUserSettings.VertexRadius = fVertexRadius;

            m_oGeneralUserSettings.VertexColor =
                (KnownColor)cbxVertexColor.SelectedValue;

            m_oGeneralUserSettings.VertexAlpha = (Single)nudVertexAlpha.Value;

            m_oGeneralUserSettings.PrimaryLabelFillColor =
                (KnownColor)cbxPrimaryLabelFillColor.SelectedValue;

            m_oGeneralUserSettings.SelectedVertexColor =
                (KnownColor)cbxSelectedVertexColor.SelectedValue;

            m_oGeneralUserSettings.AutoSelect = chkAutoSelect.Checked;

            m_oGeneralUserSettings.Font = m_oFont;

            m_oGeneralUserSettings.LayoutUserSettings = m_oLayoutUserSettings;
        }
        else
        {
            cbxBackColor.SelectedValue = m_oGeneralUserSettings.BackColor;

            nudFilteredAlpha.Value =
                (Decimal)m_oGeneralUserSettings.FilteredAlpha;

            nudEdgeWidth.Value = (Decimal)m_oGeneralUserSettings.EdgeWidth;

            nudSelectedEdgeWidth.Value =
                (Decimal)m_oGeneralUserSettings.SelectedEdgeWidth;

            nudRelativeArrowSize.Value =
                (Decimal)m_oGeneralUserSettings.RelativeArrowSize;

            cbxEdgeColor.SelectedValue = m_oGeneralUserSettings.EdgeColor;

            nudEdgeAlpha.Value = (Decimal)m_oGeneralUserSettings.EdgeAlpha;

            cbxSelectedEdgeColor.SelectedValue =
                m_oGeneralUserSettings.SelectedEdgeColor;

            cbxVertexShape.SelectedValue = m_oGeneralUserSettings.VertexShape;

            nudVertexRadius.Value =
                (Decimal)m_oGeneralUserSettings.VertexRadius;

            cbxVertexColor.SelectedValue = m_oGeneralUserSettings.VertexColor;

            nudVertexAlpha.Value = (Decimal)m_oGeneralUserSettings.VertexAlpha;

            cbxPrimaryLabelFillColor.SelectedValue =
                m_oGeneralUserSettings.PrimaryLabelFillColor;

            cbxSelectedVertexColor.SelectedValue =
                m_oGeneralUserSettings.SelectedVertexColor;

            chkAutoSelect.Checked = m_oGeneralUserSettings.AutoSelect;

            m_oFont = m_oGeneralUserSettings.Font;

            m_oLayoutUserSettings =
                m_oGeneralUserSettings.LayoutUserSettings.Copy();
        }

        return (true);
    }

    //*************************************************************************
    //  Method: btnFont_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnFont button.
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
    btnFont_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        FontDialog oFontDialog = new FontDialog();

        // Note that the FontDialog makes a copy of m_oFont, so if the user
        // edits the font within FontDialog but then cancels this
        // GeneralUserSettingsDialog, the m_oGeneralUserSettings.Font object
        // doesn't get modified.  This is the correct behavior.

        oFontDialog.Font = m_oFont;
        oFontDialog.FontMustExist = true;

        // The FontConverter class implicity used by ApplicationsSettingsBase
        // to persist GeneralUserSettings.Font does not persist the script, so
        // don't allow the user to change the script from the default.

        oFontDialog.AllowScriptChange = false;

        if (oFontDialog.ShowDialog() == DialogResult.OK)
        {
            m_oFont = oFontDialog.Font;
        }
    }

    //*************************************************************************
    //  Method: btnLayout_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnLayout button.
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
    btnLayout_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        LayoutUserSettingsDialog oLayoutUserSettingsDialog =
            new LayoutUserSettingsDialog(m_oLayoutUserSettings);

        oLayoutUserSettingsDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: btnResetAll_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnResetAll button.
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
    btnResetAll_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        m_oGeneralUserSettings.Reset();

        DoDataExchange(false);
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

    public  override void
    AssertValid()
    {
        base.AssertValid();

        Debug.Assert(m_oGeneralUserSettings != null);
        Debug.Assert(m_oFont != null);
        Debug.Assert(m_oLayoutUserSettings != null);
        Debug.Assert(m_oGeneralUserSettingsDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object whose properties are being edited.

    protected GeneralUserSettings m_oGeneralUserSettings;

    /// A copy of the LayoutUserSettings object owned by
    /// m_oGeneralUserSettings.  This gets edited by a
    /// LayoutUserSettingsDialog.

    protected LayoutUserSettings m_oLayoutUserSettings;

    /// Font property of m_oGeneralUserSettings.  This gets edited by a
    /// FontDialog.

    protected Font m_oFont;

    /// User settings for this dialog.

    protected GeneralUserSettingsDialogUserSettings
        m_oGeneralUserSettingsDialogUserSettings;
}


//*****************************************************************************
//  Class: GeneralUserSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="GeneralUserSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("GeneralUserSettingsDialog2") ]

public class GeneralUserSettingsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: GeneralUserSettingsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GeneralUserSettingsDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public GeneralUserSettingsDialogUserSettings
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
