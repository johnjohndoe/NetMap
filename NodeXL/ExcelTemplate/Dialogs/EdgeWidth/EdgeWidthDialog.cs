

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: EdgeWidthDialog
//
/// <summary>
/// Gets an edge width from the user.
/// </summary>
///
/// <remarks>
/// If <see cref="Form.ShowDialog()" /> returns DialogResult.OK, read the <see
/// cref="EdgeWidth" /> property to get the edge width specified by the
/// user.
/// </remarks>
//*****************************************************************************

public partial class EdgeWidthDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: EdgeWidthDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EdgeWidthDialog" /> class.
    /// </summary>
    //*************************************************************************

    public EdgeWidthDialog()
    {
        InitializeComponent();

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oEdgeWidthDialogUserSettings = new EdgeWidthDialogUserSettings(this);

        nudEdgeWidth.Minimum =
            (Decimal)EdgeWidthConverter.MinimumWidthWorkbook;

        nudEdgeWidth.Maximum = 
            (Decimal)EdgeWidthConverter.MaximumWidthWorkbook;

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Property: EdgeWidth
    //
    /// <summary>
    /// Gets the edge width specified by the user.
    /// </summary>
    ///
    /// <value>
    /// The edge width specified by the user.  This is between
    /// EdgeWidthConverter.MinimumWidthWorkbook and
    /// EdgeWidthConverter.MaximumWidthWorkbook.
    /// </value>
    ///
    /// <remarks>
    /// This should be read only if <see cref="Form.ShowDialog()" /> returned
    /// DialogResult.OK.
    /// </remarks>
    //*************************************************************************

    public Single
    EdgeWidth
    {
        get
        {
            Debug.Assert(this.DialogResult == DialogResult.OK);
            AssertValid();

            return (m_oEdgeWidthDialogUserSettings.EdgeWidth);
        }
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
            Single fEdgeWidth;

            if (!ValidateNumericUpDown(nudEdgeWidth, "an edge width",
                out fEdgeWidth) )
            {
                return (false);
            }

            m_oEdgeWidthDialogUserSettings.EdgeWidth = fEdgeWidth;

        }
        else
        {
            nudEdgeWidth.Value =
                (Decimal)m_oEdgeWidthDialogUserSettings.EdgeWidth;
        }

        return (true);
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
            this.DialogResult = DialogResult.OK;
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

        Debug.Assert(m_oEdgeWidthDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// User settings for this dialog.

    protected EdgeWidthDialogUserSettings m_oEdgeWidthDialogUserSettings;
}


//*****************************************************************************
//  Class: EdgeWidthDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="EdgeWidthDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("EdgeWidthDialog") ]

public class EdgeWidthDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: EdgeWidthDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="EdgeWidthDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public EdgeWidthDialogUserSettings
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
    //  Property: EdgeWidth
    //
    /// <summary>
    /// Gets or sets the edge width.
    /// </summary>
    ///
    /// <value>
    /// The edge width.  Must be between
    /// EdgeWidthConverter.MinimumEdgeWidthWorkbook and
    /// EdgeWidthConverter.MaximumEdgeWidthWorkbook.  The default value
    /// is 1.0.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("1.0") ]

    public Single
    EdgeWidth
    {
        get
        {
            AssertValid();

            return ( (Single)this[EdgeWidthKey] );
        }

        set
        {
            this[EdgeWidthKey] = value;

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

    /// Name of the settings key for the EdgeWidth property.

    protected const String EdgeWidthKey =
        "EdgeWidth";
}
}
