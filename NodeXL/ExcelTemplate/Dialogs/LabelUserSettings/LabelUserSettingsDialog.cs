
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
//  Class: LabelUserSettingsDialog
//
/// <summary>
/// Edits a <see cref="LabelUserSettings" /> object.
/// </summary>
///
/// <remarks>
/// Pass a <see cref="LabelUserSettings" /> object to the constructor.  If the
/// user edits the object, <see cref="Form.ShowDialog()" /> returns
/// DialogResult.OK.  Otherwise, the object is not modified and <see
/// cref="Form.ShowDialog()" /> returns DialogResult.Cancel.
/// </remarks>
//*****************************************************************************

public partial class LabelUserSettingsDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: LabelUserSettingsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="LabelUserSettingsDialog" /> class.
    /// </summary>
    ///
    /// <param name="labelUserSettings">
    /// The object being edited.
    /// </param>
    //*************************************************************************

    public LabelUserSettingsDialog
    (
        LabelUserSettings labelUserSettings
    )
    {
        Debug.Assert(labelUserSettings != null);
        labelUserSettings.AssertValid();

        m_oLabelUserSettings = labelUserSettings;
        m_oFont = m_oLabelUserSettings.Font;

        // Instantiate an object that saves and retrieves the position of this
        // dialog.  Note that the object automatically saves the settings when
        // the form closes.

        m_oLabelUserSettingsDialogUserSettings =
            new LabelUserSettingsDialogUserSettings(this);

        InitializeComponent();

        usrVertexLabelMaximumLength.AccessKey = 'T';
        usrEdgeLabelMaximumLength.AccessKey = 'c';

        ( new VertexLabelPositionConverter() ).PopulateComboBox(
            cbxVertexLabelPosition, false);

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
            if (
                !usrVertexLabelMaximumLength.Validate()
                ||
                !usrEdgeLabelMaximumLength.Validate()
                )
            {
                return (false);
            }

            m_oLabelUserSettings.VertexLabelMaximumLength =
                usrVertexLabelMaximumLength.Value;

            m_oLabelUserSettings.VertexLabelFillColor =
                usrVertexLabelFillColor.Color;

            m_oLabelUserSettings.VertexLabelPosition =
                (VertexLabelPosition)cbxVertexLabelPosition.SelectedValue;

            m_oLabelUserSettings.EdgeLabelMaximumLength =
                usrEdgeLabelMaximumLength.Value;

            m_oLabelUserSettings.Font = m_oFont;
        }
        else
        {
            usrVertexLabelMaximumLength.Value =
                m_oLabelUserSettings.VertexLabelMaximumLength;

            usrVertexLabelFillColor.Color =
                m_oLabelUserSettings.VertexLabelFillColor;

            cbxVertexLabelPosition.SelectedValue =
                m_oLabelUserSettings.VertexLabelPosition;

            usrEdgeLabelMaximumLength.Value =
                m_oLabelUserSettings.EdgeLabelMaximumLength;

            m_oFont = m_oLabelUserSettings.Font;
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

        EditFont(ref m_oFont);
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

        Debug.Assert(m_oLabelUserSettings != null);
        Debug.Assert(m_oFont != null);
        Debug.Assert(m_oLabelUserSettingsDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object whose properties are being edited.

    protected LabelUserSettings m_oLabelUserSettings;

    /// Font properties of m_oLabelUserSettings.  These get edited by a
    /// FontDialog.

    protected Font m_oFont;

    /// User settings for this dialog.

    protected LabelUserSettingsDialogUserSettings
        m_oLabelUserSettingsDialogUserSettings;
}


//*****************************************************************************
//  Class: LabelUserSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="LabelUserSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("LabelUserSettingsDialog") ]

public class LabelUserSettingsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: LabelUserSettingsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="LabelUserSettingsDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public LabelUserSettingsDialogUserSettings
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
