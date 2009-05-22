

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphImageUserSettingsDialog
//
/// <summary>
/// Edits a <see cref="GraphImageUserSettings" /> object.
/// </summary>
///
/// <remarks>
/// Pass a <see cref="GraphImageUserSettings" /> object to the constructor.  If
/// the user edits the object, <see cref="Form.ShowDialog()" /> returns
/// DialogResult.OK.  Otherwise, the object is not modified and <see
/// cref="Form.ShowDialog()" /> returns DialogResult.Cancel.
/// </remarks>
//*****************************************************************************

public partial class GraphImageUserSettingsDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: GraphImageUserSettingsDialog()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
    /// cref="GraphImageUserSettingsDialog" /> class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GraphImageUserSettingsDialog" /> class with a
    /// <see cref="GraphImageUserSettings" /> object and a NodeXLControlSize.
    /// </summary>
    ///
    /// <param name="graphImageUserSettings">
    /// The object being edited.
    /// </param>
    ///
    /// <param name="nodeXLControlSizePx">
    /// The size of the NodeXLControl, in pixels.
    /// </param>
    //*************************************************************************

    public GraphImageUserSettingsDialog
    (
        GraphImageUserSettings graphImageUserSettings,
        Size nodeXLControlSizePx
    )
    : this()
    {
        Debug.Assert(graphImageUserSettings != null);
        graphImageUserSettings.AssertValid();
        Debug.Assert(nodeXLControlSizePx.Width >= 0);
        Debug.Assert(nodeXLControlSizePx.Height >= 0);

        m_oGraphImageUserSettings = graphImageUserSettings;

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oGraphImageUserSettingsDialogUserSettings =
            new GraphImageUserSettingsDialogUserSettings(this);

        lblControlWidth.Text =
            nodeXLControlSizePx.Width.ToString(ExcelTemplateForm.Int32Format);

        lblControlHeight.Text =
            nodeXLControlSizePx.Height.ToString(ExcelTemplateForm.Int32Format);

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: GraphImageUserSettingsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GraphImageUserSettingsDialog" /> class for the Visual Studio
    /// designer.
    /// </summary>
    ///
    /// <remarks>
    /// Do not use this constructor.  It is for use by the Visual Studio
    /// designer only.
    /// </remarks>
    //*************************************************************************

    public GraphImageUserSettingsDialog()
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
            Boolean bUseControlSize = radUseControlSize.Checked;

            if (!bUseControlSize)
            {
                // Validate the controls.

                Int32 iWidthPx, iHeightPx;

                if (
                    !ValidateNumericUpDown(nudWidthPx,
                        "a width in pixels", out iWidthPx)
                    ||
                    !ValidateNumericUpDown(nudHeightPx,
                        "a height in pixels", out iHeightPx)
                    )
                {
                    return (false);
                }

                m_oGraphImageUserSettings.WidthPx = iWidthPx;
                m_oGraphImageUserSettings.HeightPx = iHeightPx;
            }

            m_oGraphImageUserSettings.UseControlSize = bUseControlSize;
        }
        else
        {
            radThisSize.Checked = !(radUseControlSize.Checked =
                m_oGraphImageUserSettings.UseControlSize);

            nudWidthPx.Value = m_oGraphImageUserSettings.WidthPx;
            nudHeightPx.Value = m_oGraphImageUserSettings.HeightPx;

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

        grpThisSize.Enabled =
            !(grpUseControlSize.Enabled = radUseControlSize.Checked);
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

    public override void
    AssertValid()
    {
        base.AssertValid();

        Debug.Assert(m_oGraphImageUserSettings != null);
        Debug.Assert(m_oGraphImageUserSettingsDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object whose properties are being edited.

    protected GraphImageUserSettings m_oGraphImageUserSettings;

    /// User settings for this dialog.

    protected GraphImageUserSettingsDialogUserSettings
        m_oGraphImageUserSettingsDialogUserSettings;
}


//*****************************************************************************
//  Class: GraphImageUserSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="GraphImageUserSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("GraphImageUserSettingsDialog") ]

public class GraphImageUserSettingsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: GraphImageUserSettingsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GraphImageUserSettingsDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public GraphImageUserSettingsDialogUserSettings
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
