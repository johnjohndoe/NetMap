
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: MaximumLabelLengthControl
//
/// <summary>
/// Control for getting a maximum label length.
/// </summary>
///
/// <remarks>
/// Set the <see cref="Value" /> property after the control is created.  To
/// retrieve the edited length, call <see cref="Validate" />, and if <see
/// cref="Validate" /> returns true, read the <see cref="Value" /> property.
/// </remarks>
//*****************************************************************************

public partial class MaximumLabelLengthControl : UserControl
{
    //*************************************************************************
    //  Constructor: MaximumLabelLengthControl()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="MaximumLabelLengthControl" /> class.
    /// </summary>
    //*************************************************************************

    public MaximumLabelLengthControl()
    {
        InitializeComponent();

        m_iMaximumLabelLength = Int32.MaxValue;
        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Property: Value
    //
    /// <summary>
    /// Gets or sets the maximum length of a label.
    /// </summary>
    ///
    /// <value>
    /// The maximum length of a label, or Int32.MaxValue if there is no
    /// maximum.  Must be greater than or equal to zero.  The default value is
    /// Int32.MaxValue.
    /// </value>
    //*************************************************************************

    public Int32
    Value
    {
        get
        {
            AssertValid();

            return (m_iMaximumLabelLength);
        }

        set
        {
            m_iMaximumLabelLength = value;
            DoDataExchange(false);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: AccessKey
    //
    /// <summary>
    /// Sets the access key used by the control.
    /// </summary>
    ///
    /// <value>
    /// The access key used by the control.  Must be one of the characters in
    /// "Truncate labels longer than".  The default is none.
    /// </value>
    //*************************************************************************

    public Char
    AccessKey
    {
        set
        {
            chkUseMaximumLabelLength.Text =
                chkUseMaximumLabelLength.Text.Replace(
                    value.ToString(), "&" + value);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: Validate()
    //
    /// <summary>
    /// Validates the user's settings.
    /// </summary>
    ///
    /// <returns>
    /// true if the validation was successful.
    /// </returns>
    ///
    /// <remarks>
    /// If validation fails, an error message is displayed and false is
    /// returned.
    /// </remarks>
    //*************************************************************************

    public new Boolean
    Validate()
    {
        AssertValid();

        return ( DoDataExchange(true) );
    }

    //*************************************************************************
    //  Method: DoDataExchange()
    //
    /// <summary>
    /// Transfers data between the control's fields and its controls.
    /// </summary>
    ///
    /// <param name="bFromControls">
    /// true to transfer data from the control's controls to its fields, false
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

        Boolean bUseMaximumLabelLength;

        if (bFromControls)
        {
            bUseMaximumLabelLength = chkUseMaximumLabelLength.Checked;
            Int32 iMaximumLabelLength = 0;

            if (
                bUseMaximumLabelLength
                &&
                !FormUtil.ValidateNumericUpDown(nudMaximumLabelLength,
                    "a maximum label length", out iMaximumLabelLength)
                )
            {
                return (false);
            }

            m_iMaximumLabelLength = bUseMaximumLabelLength ?
                iMaximumLabelLength : Int32.MaxValue;
        }
        else
        {
            bUseMaximumLabelLength = (m_iMaximumLabelLength != Int32.MaxValue);

            chkUseMaximumLabelLength.Checked = bUseMaximumLabelLength;

            // The 20 is to provide a reasonable default in case the user later
            // checks chkUseMaximumLabelLength.

            nudMaximumLabelLength.Value =
                bUseMaximumLabelLength ? m_iMaximumLabelLength : 20;

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

        nudMaximumLabelLength.Enabled = chkUseMaximumLabelLength.Checked;
    }

    //*************************************************************************
    //  Method: chkUseMaximumLabelLength_CheckedChanged()
    //
    /// <summary>
    /// Handles the CheckedChanged event on the chkUseMaximumLabelLength
    /// CheckBox.
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
    chkUseMaximumLabelLength_CheckedChanged
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        EnableControls();
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")] 

    public void
    AssertValid()
    {
        Debug.Assert(m_iMaximumLabelLength >= 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Maximum label length, or Int32.MaxValue for no maximum.

    protected Int32 m_iMaximumLabelLength;
}

}
