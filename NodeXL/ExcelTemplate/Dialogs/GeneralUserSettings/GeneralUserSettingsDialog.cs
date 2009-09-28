
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
    ///
    /// <param name="workbook">
    /// Workbook containing the graph data.
    /// </param>
    //*************************************************************************

    public GeneralUserSettingsDialog
    (
        GeneralUserSettings generalUserSettings,
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        Debug.Assert(generalUserSettings != null);
        Debug.Assert(workbook != null);
        generalUserSettings.AssertValid();

        m_oGeneralUserSettings = generalUserSettings;
        m_oWorkbook = workbook;
        m_oLabelFont = m_oGeneralUserSettings.LabelFont;
        m_oAxisFont = m_oGeneralUserSettings.AxisFont;
        m_oLayoutUserSettings = m_oGeneralUserSettings.LayoutUserSettings;

        // Instantiate an object that saves and retrieves the position of this
        // dialog.  Note that the object automatically saves the settings when
        // the form closes.

        m_oGeneralUserSettingsDialogUserSettings =
            new GeneralUserSettingsDialogUserSettings(this);

        InitializeComponent();

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

        nudVertexRadius.Minimum = nudVertexImageSize.Minimum =
            (Decimal)VertexRadiusConverter.MinimumRadiusWorkbook;

        nudVertexRadius.Maximum = nudVertexImageSize.Maximum =
            (Decimal)VertexRadiusConverter.MaximumRadiusWorkbook;

        ( new VertexShapeConverter() ).PopulateComboBox(cbxVertexShape, false);

        nudVertexAlpha.Minimum = nudEdgeAlpha.Minimum =
            (Decimal)AlphaConverter.MinimumAlphaWorkbook;

        nudVertexAlpha.Maximum = nudEdgeAlpha.Maximum =
            (Decimal)AlphaConverter.MaximumAlphaWorkbook;

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
            Boolean bUseSpecifiedVertexImageSize =
                radUseSpecifiedVertexImageSize.Checked;

            Single fEdgeWidth, fSelectedEdgeWidth, fRelativeArrowSize,
                fVertexRadius, fVertexAlpha, fEdgeAlpha;

            Single fVertexImageSize = 0;

            if (
                !ValidateNumericUpDown(nudVertexRadius,
                    "a size for vertex shapes", out fVertexRadius)
                ||
                ( bUseSpecifiedVertexImageSize &&
                    !ValidateNumericUpDown(nudVertexImageSize,
                    "a size for vertex images", out fVertexImageSize) )
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
                )
            {
                return (false);
            }

            m_oGeneralUserSettings.BackColor = usrBackColor.Color;
            m_oGeneralUserSettings.EdgeWidth = fEdgeWidth;
            m_oGeneralUserSettings.SelectedEdgeWidth = fSelectedEdgeWidth;
            m_oGeneralUserSettings.RelativeArrowSize = fRelativeArrowSize;
            m_oGeneralUserSettings.EdgeColor = usrEdgeColor.Color;
            m_oGeneralUserSettings.EdgeAlpha = fEdgeAlpha;

            m_oGeneralUserSettings.SelectedEdgeColor =
                usrSelectedEdgeColor.Color;

            m_oGeneralUserSettings.VertexShape =
                (VertexShape)cbxVertexShape.SelectedValue;

            m_oGeneralUserSettings.VertexRadius = fVertexRadius;

            m_oGeneralUserSettings.VertexImageSize =
                bUseSpecifiedVertexImageSize ? fVertexImageSize :
                new Nullable<Single>();

            m_oGeneralUserSettings.VertexColor = usrVertexColor.Color;
            m_oGeneralUserSettings.VertexAlpha = fVertexAlpha;

            m_oGeneralUserSettings.PrimaryLabelFillColor =
                usrPrimaryLabelFillColor.Color;

            m_oGeneralUserSettings.SelectedVertexColor =
                usrSelectedVertexColor.Color;

            m_oGeneralUserSettings.AutoSelect = chkAutoSelect.Checked;
            m_oGeneralUserSettings.LabelFont = m_oLabelFont;
            m_oGeneralUserSettings.AxisFont = m_oAxisFont;
            m_oGeneralUserSettings.LayoutUserSettings = m_oLayoutUserSettings;

            m_oGeneralUserSettings.AutoReadWorkbook =
                chkAutoReadWorkbook.Checked;
        }
        else
        {
            usrBackColor.Color = m_oGeneralUserSettings.BackColor;
            nudEdgeWidth.Value = (Decimal)m_oGeneralUserSettings.EdgeWidth;

            nudSelectedEdgeWidth.Value =
                (Decimal)m_oGeneralUserSettings.SelectedEdgeWidth;

            nudRelativeArrowSize.Value =
                (Decimal)m_oGeneralUserSettings.RelativeArrowSize;

            usrEdgeColor.Color = m_oGeneralUserSettings.EdgeColor;
            nudEdgeAlpha.Value = (Decimal)m_oGeneralUserSettings.EdgeAlpha;

            usrSelectedEdgeColor.Color =
                m_oGeneralUserSettings.SelectedEdgeColor;

            cbxVertexShape.SelectedValue = m_oGeneralUserSettings.VertexShape;

            nudVertexRadius.Value =
                (Decimal)m_oGeneralUserSettings.VertexRadius;

            Nullable<Single> oVertexImageSize =
                m_oGeneralUserSettings.VertexImageSize;

            if (oVertexImageSize.HasValue)
            {
                radUseSpecifiedVertexImageSize.Checked = true;
                nudVertexImageSize.Value = (Decimal)oVertexImageSize.Value;
            }
            else
            {
                radUseActualVertexImageSize.Checked = true;
            }

            usrVertexColor.Color = m_oGeneralUserSettings.VertexColor;
            nudVertexAlpha.Value = (Decimal)m_oGeneralUserSettings.VertexAlpha;

            usrPrimaryLabelFillColor.Color =
                m_oGeneralUserSettings.PrimaryLabelFillColor;

            usrSelectedVertexColor.Color =
                m_oGeneralUserSettings.SelectedVertexColor;

            chkAutoSelect.Checked = m_oGeneralUserSettings.AutoSelect;

            m_oLabelFont = m_oGeneralUserSettings.LabelFont;
            m_oAxisFont = m_oGeneralUserSettings.AxisFont;

            m_oLayoutUserSettings =
                m_oGeneralUserSettings.LayoutUserSettings.Copy();

            chkAutoReadWorkbook.Checked =
                m_oGeneralUserSettings.AutoReadWorkbook;

            EnableControls();
        }

        return (true);
    }

    //*************************************************************************
    //  Method: EditFont()
    //
    /// <summary>
    /// Edits a Font object.
    /// </summary>
    ///
    /// <param name="oFont">
    /// The Font object to edit.
    /// </param>
    //*************************************************************************

    protected void
    EditFont
    (
        ref Font oFont
    )
    {
        AssertValid();

        FontDialog oFontDialog = new FontDialog();

        // Note that the FontDialog makes a copy of oFont, so if the user edits
        // the font within FontDialog but then cancels this
        // GeneralUserSettingsDialog, the m_oGeneralUserSettings.Font object
        // doesn't get modified.  This is the correct behavior.

        oFontDialog.Font = oFont;
        oFontDialog.FontMustExist = true;
        oFontDialog.ScriptsOnly = true;
        oFontDialog.ShowEffects = false;

        // The FontConverter class implicity used by ApplicationsSettingsBase
        // to persist GeneralUserSettings's Font objects does not persist the
        // script, so don't allow the user to change the script from the
        // default.

        oFontDialog.AllowScriptChange = false;

        try
        {
            if (oFontDialog.ShowDialog() == DialogResult.OK)
            {
                oFont = oFontDialog.Font;
            }
        }
        catch (ArgumentException)
        {
            // Known bug: Selecting the Visual UI font in the dialog throws an
            // ArgumentException of "Only TrueType fonts are supported. This is
            // not a TrueType font."  See the following post:
            //
            // http://social.msdn.microsoft.com/Forums/en-US/vbgeneral/thread/
            // e50a3dc2-a9d9-4eea-aae6-39bc7c18b04e
        }
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

        nudVertexImageSize.Enabled = radUseSpecifiedVertexImageSize.Checked;
    }

    //*************************************************************************
    //  Method: btnCustomizeVertexMenu_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnCustomizeVertexMenu button.
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
    btnCustomizeVertexMenu_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        const String Message =
            "Use this to add custom menu items to the menu that appears when"
            + " you right-click a vertex in the NodeXL graph."
            + "\r\n\r\n"
            + "Clicking \"Yes\" below will add a pair of columns to the"
            + " Vertices worksheet -- one for menu item text and another for"
            + " the action to take when the menu item is selected."
            + "\r\n\r\n"
            + "For example, if you add the column pair and enter \"Send Mail"
            + " To\" for a vertex's menu item text and \"mailto:bob@msn.com\""
            + " for the action, then right-clicking the vertex in the NodeXL"
            + " graph and selecting \"Send Mail To\" from the right-click menu"
            + " will open a new email message addressed to bob@msn.com."
            + "\r\n\r\n"
            + "If you want to open a Web page when the menu item is selected,"
            + " enter an URL for the action."
            + "\r\n\r\n"
            + "If you want to add more than one custom menu item to a vertex's"
            + " right-click menu, run this again to add another pair of"
            + " columns."
            + "\r\n\r\n"
            + "Do you want to add a pair of columns to the Vertices worksheet?"
            ;

        if (MessageBox.Show(Message, this.ApplicationName,
                MessageBoxButtons.YesNo, MessageBoxIcon.Information) !=
                DialogResult.Yes)
        {
            return;
        }

        // Create and use the object that adds the columns to the vertex
        // table.

        TableColumnAdder oTableColumnAdder = new TableColumnAdder();

        this.UseWaitCursor = true;

        try
        {
            oTableColumnAdder.AddColumnPair(m_oWorkbook,
                WorksheetNames.Vertices, TableNames.Vertices,
                VertexTableColumnNames.CustomMenuItemTextBase,
                VertexTableColumnWidths.CustomMenuItemText,
                VertexTableColumnNames.CustomMenuItemActionBase,
                VertexTableColumnWidths.CustomMenuItemAction
                );

            this.UseWaitCursor = false;
        }
        catch (Exception oException)
        {
            this.UseWaitCursor = false;

            ErrorUtil.OnException(oException);
        }
    }

    //*************************************************************************
    //  Method: btnLabelFont_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnLabelFont button.
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
    btnLabelFont_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        EditFont(ref m_oLabelFont);
    }

    //*************************************************************************
    //  Method: btnAxisFont_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnAxisFont button.
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
    btnAxisFont_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        EditFont(ref m_oAxisFont);
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
        Debug.Assert(m_oWorkbook != null);
        Debug.Assert(m_oLabelFont != null);
        Debug.Assert(m_oAxisFont != null);
        Debug.Assert(m_oLayoutUserSettings != null);
        Debug.Assert(m_oGeneralUserSettingsDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object whose properties are being edited.

    protected GeneralUserSettings m_oGeneralUserSettings;

    /// Workbook containing the graph data.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

    /// A copy of the LayoutUserSettings object owned by
    /// m_oGeneralUserSettings.  This gets edited by a
    /// LayoutUserSettingsDialog.

    protected LayoutUserSettings m_oLayoutUserSettings;

    /// Font properties of m_oGeneralUserSettings.  These get edited by a
    /// FontDialog.

    protected Font m_oLabelFont;
    ///
    protected Font m_oAxisFont;

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

[ SettingsGroupNameAttribute("GeneralUserSettingsDialog4") ]

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
