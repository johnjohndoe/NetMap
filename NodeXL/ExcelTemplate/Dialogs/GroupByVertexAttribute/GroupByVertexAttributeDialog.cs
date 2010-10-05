

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GroupByVertexAttributeDialog
//
/// <summary>
/// Allows the user to group the workbook's vertices using the values in a
/// specified vertex column.
/// </summary>
///
/// <remarks>
/// Call <see cref="Form.ShowDialog()" /> to show the dialog.  The grouping is
/// performed entirely by the dialog; the caller doesn't have to do anything
/// else.
/// </remarks>
//*****************************************************************************

public partial class GroupByVertexAttributeDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: GroupByVertexAttributeDialog()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
    /// cref="GroupByVertexAttributeDialog" /> class.
    /// </overloads>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph contents.
    /// </param>
    //*************************************************************************

    public GroupByVertexAttributeDialog
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        InitializeComponent();

        m_oWorkbook = workbook;

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oGroupByVertexAttributeDialogUserSettings =
            new GroupByVertexAttributeDialogUserSettings(this);

        if ( ExcelUtil.TryGetTable(m_oWorkbook, WorksheetNames.Vertices,
            TableNames.Vertices, out m_oVertexTable) )
        {
            cbxVertexColumnName.PopulateWithTableColumnNames(m_oVertexTable);
        }

        cbxVertexColumnFormat.PopulateWithObjectsAndText(
            ExcelColumnFormat.Other, "Categories",
            ExcelColumnFormat.Number, "Numbers",
            ExcelColumnFormat.Date, "Dates",
            ExcelColumnFormat.Time, "Times",
            ExcelColumnFormat.DateAndTime, "Dates with times"
            );

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Property: SelectedVertexColumnFormat
    //
    /// <summary>
    /// Gets or sets the selected vertex column format.
    /// </summary>
    ///
    /// <value>
    /// The selected vertex column format, as an ExcelColumnFormat.
    /// </value>
    //*************************************************************************

    protected ExcelColumnFormat
    SelectedVertexColumnFormat
    {
        get
        {
            AssertValid();

            return ( (ExcelColumnFormat)cbxVertexColumnFormat.SelectedValue );
        }

        set
        {
            cbxVertexColumnFormat.SelectedValue = value;

            AssertValid();
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
            // Perform error checking.

            if (cbxVertexColumnName.Items.Count == 0)
            {
                OnInvalidComboBox(cbxVertexColumnName,
                    "There are no columns that can be used.");

                return (false);
            }

            String sVertexColumnName;

            if ( !ValidateRequiredComboBox(cbxVertexColumnName,
                "Select a column.", out sVertexColumnName)
                )
            {
                return (false);
            }

            // Error checking passed.  Transfer the data.

            m_oGroupByVertexAttributeDialogUserSettings.VertexColumnName =
                sVertexColumnName;

            ExcelColumnFormat eSelectedVertexColumnFormat =
                this.SelectedVertexColumnFormat;

            m_oGroupByVertexAttributeDialogUserSettings.VertexColumnFormat =
                eSelectedVertexColumnFormat;

            String sMinimumValues = String.Empty;

            switch (eSelectedVertexColumnFormat)
            {
                case ExcelColumnFormat.Number:

                    sMinimumValues =
                        lbxMinimumValues.ItemsToCultureInvariantString<
                            FormattableNumber, Double>();
                    break;

                case ExcelColumnFormat.Date:

                    sMinimumValues =
                        lbxMinimumValues.ItemsToCultureInvariantString<
                            FormattableDate, DateTime>();

                    break;

                case ExcelColumnFormat.Time:

                    sMinimumValues =
                        lbxMinimumValues.ItemsToCultureInvariantString<
                            FormattableTime, DateTime>();

                    break;

                case ExcelColumnFormat.DateAndTime:

                    sMinimumValues =
                        lbxMinimumValues.ItemsToCultureInvariantString<
                            FormattableDateAndTime, DateTime>();

                    break;

                case ExcelColumnFormat.Other:

                    break;

                default:

                    Debug.Assert(false);
                    break;
            }

            m_oGroupByVertexAttributeDialogUserSettings.MinimumValues =
                sMinimumValues;
        }
        else
        {
            cbxVertexColumnName.Text =
                m_oGroupByVertexAttributeDialogUserSettings.VertexColumnName;

            ExcelColumnFormat eSelectedVertexColumnFormat =
                m_oGroupByVertexAttributeDialogUserSettings.VertexColumnFormat;

            this.SelectedVertexColumnFormat = eSelectedVertexColumnFormat;

            String sMinimumValues =
                m_oGroupByVertexAttributeDialogUserSettings.MinimumValues;

            switch (eSelectedVertexColumnFormat)
            {
                case ExcelColumnFormat.Number:

                    lbxMinimumValues.CultureInvariantStringToItems<
                        FormattableNumber, Double>(sMinimumValues);

                    break;

                case ExcelColumnFormat.Date:

                    lbxMinimumValues.CultureInvariantStringToItems<
                        FormattableDate, DateTime>(sMinimumValues);

                    break;

                case ExcelColumnFormat.Time:

                    lbxMinimumValues.CultureInvariantStringToItems<
                        FormattableTime, DateTime>(sMinimumValues);

                    break;

                case ExcelColumnFormat.DateAndTime:

                    lbxMinimumValues.CultureInvariantStringToItems<
                        FormattableDateAndTime, DateTime>(sMinimumValues);

                    break;

                case ExcelColumnFormat.Other:

                    break;

                default:

                    Debug.Assert(false);
                    break;
            }


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

        ExcelColumnFormat eSelectedVertexColumnFormat =
            this.SelectedVertexColumnFormat;

        Boolean bEnableMinimumValueToAddTextBox = false;
        SimpleDateTimeFormat eSimpleDateTimeFormat = SimpleDateTimeFormat.Date;

        switch (eSelectedVertexColumnFormat)
        {
            case ExcelColumnFormat.Other:
            case ExcelColumnFormat.Number:

                bEnableMinimumValueToAddTextBox = true;
                break;

            case ExcelColumnFormat.Date:

                break;

            case ExcelColumnFormat.Time:

                eSimpleDateTimeFormat = SimpleDateTimeFormat.Time;
                break;

            case ExcelColumnFormat.DateAndTime:

                eSimpleDateTimeFormat = SimpleDateTimeFormat.DateAndTime;
                break;

            default:

                Debug.Assert(false);
                break;
        }

        // Note that the txbMinimumValueToAdd TextBox and the
        // dtpMinimumValueToAdd DateTimePicker have the same size and location.
        // Only one is visible at any given time.

        txbMinimumValueToAdd.Enabled = txbMinimumValueToAdd.Visible =
            bEnableMinimumValueToAddTextBox;

        dtpMinimumValueToAdd.Enabled = dtpMinimumValueToAdd.Visible =
            !bEnableMinimumValueToAddTextBox;

        dtpMinimumValueToAdd.SimpleFormat = eSimpleDateTimeFormat;

        btnRemoveSelected.Enabled =
            (lbxMinimumValues.SelectedIndices.Count > 0);

        btnRemoveAll.Enabled = (lbxMinimumValues.Items.Count > 0);

        grpMinimumValues.Enabled = 
            !String.IsNullOrEmpty(cbxVertexColumnName.Text) &&
            (eSelectedVertexColumnFormat != ExcelColumnFormat.Other);
    }

    //*************************************************************************
    //  Method: GroupByVertexAttribute()
    //
    /// <summary>
    /// Groups the workbook's vertices.
    /// </summary>
    ///
    /// <remarks>
    /// It's assumed that DoDataExchange(true) has been called and returned
    /// true.
    /// </remarks>
    //*************************************************************************

    protected void
    GroupByVertexAttribute()
    {
        AssertValid();

        String sVertexColumnName =
            m_oGroupByVertexAttributeDialogUserSettings.VertexColumnName;

        switch (m_oGroupByVertexAttributeDialogUserSettings.VertexColumnFormat)
        {
            case ExcelColumnFormat.Number:

                VertexAttributeGrouper.GroupByVertexAttributeNumber(
                    m_oWorkbook, sVertexColumnName,

                    lbxMinimumValues.GetAllContainedValues<FormattableNumber,
                        Double>()
                    );

                break;

            case ExcelColumnFormat.Date:

                VertexAttributeGrouper.GroupByVertexAttributeDate(
                    m_oWorkbook, sVertexColumnName,

                    lbxMinimumValues.GetAllContainedValues<FormattableDate,
                        DateTime>()
                    );

                break;

            case ExcelColumnFormat.Time:

                VertexAttributeGrouper.GroupByVertexAttributeTime(
                    m_oWorkbook, sVertexColumnName,

                    lbxMinimumValues.GetAllContainedValues<FormattableTime,
                        DateTime>()
                    );

                break;

            case ExcelColumnFormat.DateAndTime:

                VertexAttributeGrouper.GroupByVertexAttributeDateAndTime(
                    m_oWorkbook, sVertexColumnName,

                    lbxMinimumValues.GetAllContainedValues<
                        FormattableDateAndTime, DateTime>()
                    );

                break;

            case ExcelColumnFormat.Other:

                VertexAttributeGrouper.GroupByVertexAttributeOther(
                    m_oWorkbook, sVertexColumnName);

                break;

            default:

                Debug.Assert(false);
                break;
        }
    }

    //*************************************************************************
    //  Method: cbxVertexColumnName_SelectedIndexChanged()
    //
    /// <summary>
    /// Handles the SelectedIndexChanged event on the cbxVertexColumnName
    /// ComboBox.
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
    cbxVertexColumnName_SelectedIndexChanged
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        String sVertexColumnName = cbxVertexColumnName.Text;
        ListColumn oVertexColumn;

        if (
            String.IsNullOrEmpty(sVertexColumnName)
            ||
            m_oVertexTable == null
            ||
            !ExcelUtil.TryGetTableColumn(m_oVertexTable, sVertexColumnName,
                out oVertexColumn)
            )
        {
            return;
        }

        cbxVertexColumnFormat.SelectedValue = ExcelUtil.GetColumnFormat(
            oVertexColumn);

        EnableControls();
    }

    //*************************************************************************
    //  Method: cbxVertexColumnFormat_SelectedIndexChanged()
    //
    /// <summary>
    /// Handles the SelectedIndexChanged event on the cbxVertexColumnFormat
    /// ComboBox.
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
    cbxVertexColumnFormat_SelectedIndexChanged
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        lbxMinimumValues.Items.Clear();

        EnableControls();
    }

    //*************************************************************************
    //  Method: btnAddMinimumValue_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnAddMinimumValue button.
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
    btnAddMinimumValue_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        ExcelColumnFormat eSelectedVertexColumnFormat =
            this.SelectedVertexColumnFormat;

        switch (eSelectedVertexColumnFormat)
        {
            case ExcelColumnFormat.Number:

                Double dMinimumValue;

                if ( !ValidateDoubleTextBox(txbMinimumValueToAdd,
                    Double.MinValue, Double.MaxValue, "Enter a numeric value.",
                    out dMinimumValue) )
                {
                    return;
                }

                lbxMinimumValues.AddItem<FormattableNumber, Double>(
                    new FormattableNumber(dMinimumValue) );

                txbMinimumValueToAdd.Focus();
                txbMinimumValueToAdd.SelectAll();
                return;

            case ExcelColumnFormat.Date:

                lbxMinimumValues.AddItem<FormattableDate, DateTime>(
                    new FormattableDate(dtpMinimumValueToAdd.Value) );

                break;

            case ExcelColumnFormat.Time:

                lbxMinimumValues.AddItem<FormattableTime, DateTime>(
                    new FormattableTime(dtpMinimumValueToAdd.Value) );

                break;

            case ExcelColumnFormat.DateAndTime:

                lbxMinimumValues.AddItem<FormattableDateAndTime, DateTime>(
                    new FormattableDateAndTime(dtpMinimumValueToAdd.Value) );

                break;

            case ExcelColumnFormat.Other:

                // This should never occur, because the Add button is disabled
                // when the column format is Other.

                Debug.Assert(false);
                break;

            default:

                Debug.Assert(false);
                break;
        }

        dtpMinimumValueToAdd.Focus();
    }

    //*************************************************************************
    //  Method: lbxMinimumValues_SelectedIndexChanged()
    //
    /// <summary>
    /// Handles the SelectedIndexChanged event on the lbxMinimumValues ListBox.
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
    lbxMinimumValues_SelectedIndexChanged
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        EnableControls();
    }

    //*************************************************************************
    //  Method: btnRemoveSelected_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnRemoveSelected button.
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
    btnRemoveSelected_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        System.Windows.Forms.ListBox.ObjectCollection oItems =
            lbxMinimumValues.Items;

        System.Windows.Forms.ListBox.SelectedIndexCollection oSelectedIndices =
            lbxMinimumValues.SelectedIndices;

        Int32 iSelectedIndices = oSelectedIndices.Count;

        for (Int32 i = iSelectedIndices - 1; i >= 0; i--)
        {
            oItems.RemoveAt( oSelectedIndices[i] );
        }
    }

    //*************************************************************************
    //  Method: btnRemoveAll_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnRemoveAll button.
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
    btnRemoveAll_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        lbxMinimumValues.Items.Clear();
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
        EventArgs e
    )
    {
        AssertValid();

        if ( !DoDataExchange(true) )
        {
            return;
        }

        this.UseWaitCursor = true;

        try
        {
            GroupByVertexAttribute();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
        }
        finally
        {
            this.UseWaitCursor = false;
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

        Debug.Assert(m_oWorkbook != null);
        Debug.Assert(m_oGroupByVertexAttributeDialogUserSettings != null);
        // m_oVertexTable
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Workbook containing the graph contents.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

    /// User settings for this dialog.

    protected GroupByVertexAttributeDialogUserSettings
        m_oGroupByVertexAttributeDialogUserSettings;

    /// Vertex table, or null if it doesn't exist.

    protected ListObject m_oVertexTable;
}


//*****************************************************************************
//  Class: GroupByVertexAttributeDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="GroupByVertexAttributeDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("GroupByVertexAttributeDialog") ]

public class GroupByVertexAttributeDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: GroupByVertexAttributeDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GroupByVertexAttributeDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public GroupByVertexAttributeDialogUserSettings
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
    //  Property: VertexColumnName
    //
    /// <summary>
    /// Gets or sets the name of the vertex table column to use.
    /// </summary>
    ///
    /// <value>
    /// The name of the vertex table column.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    VertexColumnName
    {
        get
        {
            AssertValid();

            return ( (String)this[VertexColumnNameKey] );
        }

        set
        {
            this[VertexColumnNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexColumnFormat
    //
    /// <summary>
    /// Gets or sets the format of the vertex table column to use.
    /// </summary>
    ///
    /// <value>
    /// The format of the vertex table column.  The default is
    /// ExcelColumnFormat.Other.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("Other") ]

    public ExcelColumnFormat
    VertexColumnFormat
    {
        get
        {
            AssertValid();

            return ( (ExcelColumnFormat)this[VertexColumnFormatKey] );
        }

        set
        {
            this[VertexColumnFormatKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: MinimumValues
    //
    /// <summary>
    /// Gets or sets the minimum values in the groups.
    /// </summary>
    ///
    /// <value>
    /// The minimum values in the groups, as a string returned by <see
    /// cref="MinimumValueListBox.ItemsToCultureInvariantString" />.  The
    /// default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    MinimumValues
    {
        get
        {
            AssertValid();

            return ( (String)this[MinimumValuesKey] );
        }

        set
        {
            this[MinimumValuesKey] = value;

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

    /// Name of the settings key for the VertexColumnName property.

    protected const String VertexColumnNameKey = "VertexColumnName";

    /// Name of the settings key for the VertexColumnFormat property.

    protected const String VertexColumnFormatKey = "VertexColumnFormat";

    /// Name of the settings key for the MinimumValues property.

    protected const String MinimumValuesKey = "MinimumValues";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}
}
