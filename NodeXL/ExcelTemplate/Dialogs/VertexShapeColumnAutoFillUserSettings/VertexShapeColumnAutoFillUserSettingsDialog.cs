

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: VertexShapeColumnAutoFillUserSettingsDialog
//
/// <summary>
/// Edits a <see cref="NumericComparisonColumnAutoFillUserSettings" /> object
/// used to specify autofill settings for the vertex shape column.
/// </summary>
///
/// <remarks>
/// Pass a <see cref="NumericComparisonColumnAutoFillUserSettings" /> object to
/// the constructor.  If the user edits the object, <see
/// cref="Form.ShowDialog()" /> returns DialogResult.OK.  Otherwise, the object
/// is not modified and <see cref="Form.ShowDialog()" /> returns
/// DialogResult.Cancel.
///
/// <para>
/// There is a generic <see
/// cref="NumericComparisonColumnAutoFillUserSettingsDialog" /> that can be
/// used to edit other <see
/// cref="NumericComparisonColumnAutoFillUserSettings" /> objects.  The vertex
/// shape column UI, however, looks different enough from that dialog that it
/// requires its own dialog.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class VertexShapeColumnAutoFillUserSettingsDialog :
    ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: VertexShapeColumnAutoFillUserSettingsDialog()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
    /// cref="VertexShapeColumnAutoFillUserSettingsDialog" /> class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="NumericComparisonColumnAutoFillUserSettingsDialog" /> class with
    /// a <see cref="NumericComparisonColumnAutoFillUserSettings" /> object.
    /// </summary>
    ///
    /// <param name="numericComparisonColumnAutoFillUserSettings">
    /// Object to edit.
    /// </param>
    //*************************************************************************

    public VertexShapeColumnAutoFillUserSettingsDialog
    (
        NumericComparisonColumnAutoFillUserSettings
            numericComparisonColumnAutoFillUserSettings
    )
    : this()
    {
        Debug.Assert(numericComparisonColumnAutoFillUserSettings != null);

        m_oNumericComparisonColumnAutoFillUserSettings =
            numericComparisonColumnAutoFillUserSettings;

        // Instantiate an object that saves and retrieves the position of this
        // dialog.  Note that the object automatically saves the settings when
        // the form closes.

        m_oVertexShapeColumnAutoFillUserSettingsDialogUserSettings =
            new VertexShapeColumnAutoFillUserSettingsDialogUserSettings(this);

        cbxComparisonOperator.PopulateWithObjectsAndText(
            ComparisonOperator.LessThan, "Less than",
            ComparisonOperator.LessThanOrEqual, "Less than or equal to",
            ComparisonOperator.Equal, "Equal to",
            ComparisonOperator.NotEqual, "Not equal to",
            ComparisonOperator.GreaterThan, "Greater than",
            ComparisonOperator.GreaterThanOrEqual, "Greater than or equal to"
            );

        ( new VertexShapeConverter() ).PopulateComboBox(cbxVertexShape, false);

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: VertexShapeColumnAutoFillUserSettingsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="VertexShapeColumnAutoFillUserSettingsDialog" /> class for the
    /// Visual Studio designer.
    /// </summary>
    ///
    /// <remarks>
    /// Do not use this constructor.  It is for use by the Visual Studio
    /// designer only.
    /// </remarks>
    //*************************************************************************

    public VertexShapeColumnAutoFillUserSettingsDialog()
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
            Double dSourceNumberToCompareTo;

            if ( !this.ValidateDoubleTextBox(txbSourceNumber,
                Double.MinValue, Double.MaxValue, "Enter a number.",
                out dSourceNumberToCompareTo) )
            {
                return (false);
            }

            m_oNumericComparisonColumnAutoFillUserSettings.ComparisonOperator =
                (ComparisonOperator)cbxComparisonOperator.SelectedValue;

            m_oNumericComparisonColumnAutoFillUserSettings.
                SourceNumberToCompareTo = dSourceNumberToCompareTo;

            m_oNumericComparisonColumnAutoFillUserSettings.DestinationString1 =
                cbxVertexShape.Text;

            m_oNumericComparisonColumnAutoFillUserSettings.DestinationString2
                = null;
        }
        else
        {
            cbxComparisonOperator.SelectedValue =
                m_oNumericComparisonColumnAutoFillUserSettings.
                    ComparisonOperator;

            txbSourceNumber.Text =
                m_oNumericComparisonColumnAutoFillUserSettings.
                    SourceNumberToCompareTo.ToString();

            cbxVertexShape.Text = 
                m_oNumericComparisonColumnAutoFillUserSettings.
                    DestinationString1;
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

        Debug.Assert(
            m_oVertexShapeColumnAutoFillUserSettingsDialogUserSettings !=
                null);

        Debug.Assert(m_oNumericComparisonColumnAutoFillUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// User settings for this dialog.

    protected VertexShapeColumnAutoFillUserSettingsDialogUserSettings
        m_oVertexShapeColumnAutoFillUserSettingsDialogUserSettings;

    /// Object being edited.

    protected NumericComparisonColumnAutoFillUserSettings
        m_oNumericComparisonColumnAutoFillUserSettings;
}


//*****************************************************************************
//  Class: VertexShapeColumnAutoFillUserSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="VertexShapeColumnAutoFillUserSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("VertexShapeColumnAutoFillUserSettingsDialog") ]

public class VertexShapeColumnAutoFillUserSettingsDialogUserSettings :
    FormSettings
{
    //*************************************************************************
    //  Constructor: VertexShapeColumnAutoFillUserSettingsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="VertexShapeColumnAutoFillUserSettingsDialogUserSettings" />
    /// class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public VertexShapeColumnAutoFillUserSettingsDialogUserSettings
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
