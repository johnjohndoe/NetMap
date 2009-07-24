

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ColorColumnAutoFillUserSettingsDialog
//
/// <summary>
/// Edits a <see cref="ColorColumnAutoFillUserSettings" /> object.
/// </summary>
///
/// <remarks>
/// Pass a <see cref="ColorColumnAutoFillUserSettings" /> object to the
/// constructor.  If the user edits the object, <see
/// cref="Form.ShowDialog()" /> returns DialogResult.OK.  Otherwise, the object
/// is not modified and <see cref="Form.ShowDialog()" /> returns
/// DialogResult.Cancel.
/// </remarks>
//*****************************************************************************

public partial class ColorColumnAutoFillUserSettingsDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: ColorColumnAutoFillUserSettingsDialog()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
    /// cref="ColorColumnAutoFillUserSettingsDialog" /> class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ColorColumnAutoFillUserSettingsDialog" /> class with a <see
    /// cref="ColorColumnAutoFillUserSettings" /> object.
    /// </summary>
    ///
    /// <param name="colorColumnAutoFillUserSettings">
    /// Object to edit.
    /// </param>
    ///
    /// <param name="dialogCaption">
    /// Dialog caption.
    /// </param>
    //*************************************************************************

    public ColorColumnAutoFillUserSettingsDialog
    (
        ColorColumnAutoFillUserSettings colorColumnAutoFillUserSettings,
        String dialogCaption
    )
    : this()
    {
        Debug.Assert(colorColumnAutoFillUserSettings != null);
        Debug.Assert( !String.IsNullOrEmpty(dialogCaption) );

        m_oColorColumnAutoFillUserSettings = colorColumnAutoFillUserSettings;
        this.Text = dialogCaption;

        // Instantiate an object that saves and retrieves the position of this
        // dialog.  Note that the object automatically saves the settings when
        // the form closes.

        m_oColorColumnAutoFillUserSettingsDialogUserSettings =
            new ColorColumnAutoFillUserSettingsDialogUserSettings(this);

        DoDataExchange(false);

        UpdateColorGradient();

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: ColorColumnAutoFillUserSettingsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ColorColumnAutoFillUserSettingsDialog" /> class for the Visual
    /// Studio
    /// designer.
    /// </summary>
    ///
    /// <remarks>
    /// Do not use this constructor.  It is for use by the Visual Studio
    /// designer only.
    /// </remarks>
    //*************************************************************************

    public ColorColumnAutoFillUserSettingsDialog()
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
            Double dSourceNumber1 = 0;
            Double dSourceNumber2 = 0;

            const String Message = "Enter a number.";

            Boolean bUseSourceNumber1 = radUseSourceNumber1.Checked;
            Boolean bUseSourceNumber2 = radUseSourceNumber2.Checked;

            if (
                (bUseSourceNumber1
                &&
                !this.ValidateDoubleTextBox(txbSourceNumber1,
                    Double.MinValue, Double.MaxValue, Message,
                    out dSourceNumber1) )
                ||
                (bUseSourceNumber2
                &&
                !this.ValidateDoubleTextBox(txbSourceNumber2,
                    Double.MinValue, Double.MaxValue, Message,
                    out dSourceNumber2) )
                )
            {
                return (false);
            }

            m_oColorColumnAutoFillUserSettings.UseSourceNumber1 =
                bUseSourceNumber1;

            m_oColorColumnAutoFillUserSettings.UseSourceNumber2 =
                bUseSourceNumber2;

            m_oColorColumnAutoFillUserSettings.SourceNumber1 = dSourceNumber1;
            m_oColorColumnAutoFillUserSettings.SourceNumber2 = dSourceNumber2;

            m_oColorColumnAutoFillUserSettings.DestinationColor1 =
                usrDestinationColor1.Color;

            m_oColorColumnAutoFillUserSettings.DestinationColor2 =
                usrDestinationColor2.Color;

            m_oColorColumnAutoFillUserSettings.IgnoreOutliers =
                chkIgnoreOutliers.Checked;
        }
        else
        {
            radUseSourceNumber1.Checked =
                m_oColorColumnAutoFillUserSettings.UseSourceNumber1;

            radUseSourceNumber2.Checked =
                m_oColorColumnAutoFillUserSettings.UseSourceNumber2;

            txbSourceNumber1.Text =
                m_oColorColumnAutoFillUserSettings.SourceNumber1.ToString();

            txbSourceNumber2.Text =
                m_oColorColumnAutoFillUserSettings.SourceNumber2.ToString();

            usrDestinationColor1.Color =
                m_oColorColumnAutoFillUserSettings.DestinationColor1;

            usrDestinationColor2.Color =
                m_oColorColumnAutoFillUserSettings.DestinationColor2;

            chkIgnoreOutliers.Checked =
                m_oColorColumnAutoFillUserSettings.IgnoreOutliers;
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

        txbSourceNumber1.Enabled = radUseSourceNumber1.Checked;
        txbSourceNumber2.Enabled = radUseSourceNumber2.Checked;

        // The user should be able to ignore outliers only if the entire range
        // of source numbers is specified.

        if (radUseMinimumSourceNumber.Checked &&
            radUseMaximumSourceNumber.Checked)
        {
            chkIgnoreOutliers.Enabled = true;
        }
        else
        {
            chkIgnoreOutliers.Checked = false;
            chkIgnoreOutliers.Enabled = false;
        }
    }

    //*************************************************************************
    //  Method: UpdateColorGradient()
    //
    /// <summary>
    /// Updates the colors in the pnlColorGradient control.
    /// </summary>
    //*************************************************************************

    protected void
    UpdateColorGradient()
    {
        AssertValid();

        pnlColorGradient.MinimumColor = usrDestinationColor1.Color;
        pnlColorGradient.MaximumColor = usrDestinationColor2.Color;
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
    //  Method: ColorPicker_ColorChanged()
    //
    /// <summary>
    /// Handles the ColorChanged event on the usrDestinationColor1 and
    /// usrDestinationColor2 ColorPickers.
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
    ColorPicker_ColorChanged
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        UpdateColorGradient();
    }

    //*************************************************************************
    //  Method: btnSwapDestinationColors_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnSwapDestinationColors button.
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
    btnSwapDestinationColors_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        Color oTempColor = usrDestinationColor1.Color;

        usrDestinationColor1.Color = usrDestinationColor2.Color;
        usrDestinationColor2.Color = oTempColor;
        UpdateColorGradient();
    }

    //*************************************************************************
    //  Method: lnkIgnoreOutliers_LinkClicked()
    //
    /// <summary>
    /// Handles the LinkClicked event on the lnkIgnoreOutliers LinkButton.
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
    lnkIgnoreOutliers_LinkClicked
    (
        object sender,
        LinkLabelLinkClickedEventArgs e
    )
    {
        AssertValid();

        this.ShowInformation(AutoFillWorkbookDialog.IgnoreOutliersMessage);
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

        Debug.Assert(m_oColorColumnAutoFillUserSettingsDialogUserSettings !=
            null);

        Debug.Assert(m_oColorColumnAutoFillUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// User settings for this dialog.

    protected ColorColumnAutoFillUserSettingsDialogUserSettings
        m_oColorColumnAutoFillUserSettingsDialogUserSettings;

    /// Object being edited.

    protected ColorColumnAutoFillUserSettings
        m_oColorColumnAutoFillUserSettings;
}


//*****************************************************************************
//  Class: ColorColumnAutoFillUserSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="ColorColumnAutoFillUserSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("ColorColumnAutoFillUserSettingsDialog") ]

public class ColorColumnAutoFillUserSettingsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: ColorColumnAutoFillUserSettingsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ColorColumnAutoFillUserSettingsDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public ColorColumnAutoFillUserSettingsDialogUserSettings
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
