

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Office.Interop.Excel;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AddSelectedVerticesToGroupDialog
//
/// <summary>
/// Adds the selected vertices to a new or existing group.
/// </summary>
///
/// <remarks>
/// If <see cref="Form.ShowDialog()" /> returns DialogResult.OK, read the <see
/// cref="GroupName" /> property to get the name of the group to add the
/// selected vertices to, and <see cref="IsNewGroup" /> to check whether the
/// group is a new group or an existing group.
///
/// <para>
/// This class does not modify the workbook.  It is up to the caller to do the
/// actual workbook modifications that are required to add the selected
/// vertices to the specified group.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class AddSelectedVerticesToGroupDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: AddSelectedVerticesToGroupDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AddSelectedVerticesToGroupDialog" /> class.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph data.
    /// </param>
    //*************************************************************************

    public AddSelectedVerticesToGroupDialog
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        Debug.Assert(workbook != null);

        m_oWorkbook = workbook;
        m_sGroupName = null;
        m_bIsNewGroup = false;

        InitializeComponent();

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oAddSelectedVerticesToGroupDialogUserSettings =
            new AddSelectedVerticesToGroupDialogUserSettings(this);

        ICollection<String> oUniqueGroupNames;

        if ( ExcelUtil.TryGetUniqueTableColumnStringValues(m_oWorkbook,
                WorksheetNames.Groups, TableNames.Groups,
                GroupTableColumnNames.Name, out oUniqueGroupNames) )
        {
            cbxGroupName.DataSource = oUniqueGroupNames.ToArray();
        }

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Property: GroupName
    //
    /// <summary>
    /// Gets the name of the vertices to add the selected vertices to.
    /// </summary>
    ///
    /// <value>
    /// The name of the vertices to add the selected vertices to.
    /// </value>
    ///
    /// <remarks>
    /// This should be read only if <see cref="Form.ShowDialog()" /> returned
    /// DialogResult.OK.
    ///
    /// <para>
    /// Read the <see cref="IsNewGroup" /> property to determine whether the
    /// returned group is a new group or an existing group.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public String
    GroupName
    {
        get
        {
            Debug.Assert(this.DialogResult == DialogResult.OK);
            Debug.Assert( !String.IsNullOrEmpty(m_sGroupName) );
            AssertValid();

            return (m_sGroupName);
        }
    }

    //*************************************************************************
    //  Property: IsNewGroup
    //
    /// <summary>
    /// Gets a flag indicating whether the group to add the vertices to is a
    /// new group.
    /// </summary>
    ///
    /// <value>
    /// true if the group returned by <see cref="GroupName" /> is a new group,
    /// false if it is an existing group.
    /// </value>
    ///
    /// <remarks>
    /// This should be read only if <see cref="Form.ShowDialog()" /> returned
    /// DialogResult.OK.
    /// </remarks>
    //*************************************************************************

    public Boolean
    IsNewGroup
    {
        get
        {
            Debug.Assert(this.DialogResult == DialogResult.OK);
            AssertValid();

            return (m_bIsNewGroup);
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
            String sGroupName;

            if ( !ValidateRequiredComboBox(cbxGroupName,

                "Either select the name of an existing group from the"
                + " drop-down list, or enter a new group name.", 

                out sGroupName) )
            {
                return (false);
            }

            // We need to determine whether the group name is an existing group
            // that was selected from the drop-down list, or a new group name
            // that was typed.  SelectedItem is empty when text is typed, so
            // that seems like an adequate test.  However, the user may have
            // typed an existing group name with leading or trailing spaces,
            // which ValidateRequiredComboBox() removed, and an empty
            // SelectedItem would give an incorrect answer in this case.
            //
            // Instead, take the trimmed Text property that
            // ValidateRequiredComboBox() returned and search for it in the
            // ComboBox's drop-down list.

            Int32 iGroupNameIndex = cbxGroupName.FindString(sGroupName);
            m_bIsNewGroup = (iGroupNameIndex == -1);

            if (m_bIsNewGroup)
            {
                m_sGroupName = sGroupName;
            }
            else
            {
                // ComboBox.FindString() does a case-insensitive search.  We
                // don't want "G1" and "g2" to be separate groups, so retrieve
                // the exact spelling of the existing group name.

                m_sGroupName = (String)cbxGroupName.Items[iGroupNameIndex];
            }
        }
        else
        {
            // (Do nothing.)
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

        Debug.Assert(m_oAddSelectedVerticesToGroupDialogUserSettings != null);
        Debug.Assert(m_oWorkbook != null);
        // m_sGroupName
        // m_bIsNewGroup
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// User settings for this dialog.

    protected AddSelectedVerticesToGroupDialogUserSettings
        m_oAddSelectedVerticesToGroupDialogUserSettings;

    /// Workbook containing the graph data.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

    /// The name of the vertices to add the selected vertices to, or null if
    /// a name hasn't been entered or selected yet.

    protected String m_sGroupName;

    /// true if the group is a new group, false if it is an existing group.

    protected Boolean m_bIsNewGroup;
}


//*****************************************************************************
//  Class: AddSelectedVerticesToGroupDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="AddSelectedVerticesToGroupDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("AddSelectedVerticesToGroupDialog") ]

public class AddSelectedVerticesToGroupDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: AddSelectedVerticesToGroupDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AddSelectedVerticesToGroupDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public AddSelectedVerticesToGroupDialogUserSettings
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
