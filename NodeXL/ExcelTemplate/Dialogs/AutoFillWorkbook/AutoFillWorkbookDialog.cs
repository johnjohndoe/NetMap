

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AutoFillWorkbookDialog
//
/// <summary>
/// Runs the application's AutoFill feature on a workbook.
/// </summary>
///
/// <remarks>
/// This is a modeless dialog.  To show it, call its <see
/// cref="Form.Show(IWin32Window)" /> method.
///
/// <para>
/// The AutoFill feature automatically fills edge and vertex attribute columns
/// using values from user-specified source columns.  This dialog lets the user
/// specify AutoFill settings, then uses a <see cref="WorkbookAutoFiller" /> to
/// do most of the work.  A <see cref="WorkbookAutoFilled" /> event is fired
/// when the workbook is autofilled.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class AutoFillWorkbookDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: AutoFillWorkbookDialog()
    //
    /// <overloads>
    /// Initializes a new instance of the <see cref="AutoFillWorkbookDialog" />
    /// class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoFillWorkbookDialog" />
    /// class with an AutoFillUserSettings object.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph data.
    /// </param>
    //*************************************************************************

    public AutoFillWorkbookDialog
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    : this()
    {
        Debug.Assert(workbook != null);

        m_oWorkbook = workbook;
        m_oAutoFillUserSettings = new AutoFillUserSettings();

        // Instantiate an object that retrieves and saves the position of this
        // dialog.  Note that the object automatically saves the settings when
        // the form closes.

        m_oAutoFillWorkbookDialogUserSettings =
            new AutoFillWorkbookDialogUserSettings(this);

        // Initialize the ComboBoxes used to specify the data sources for the
        // edge and vertex table columns.

        m_aoEdgeSourceColumnNameComboBoxes =
            new AutoFillEdgeColumnComboBox []
            {
            cbxEdgeColorSourceColumnName,
            cbxEdgeWidthSourceColumnName,
            cbxEdgeAlphaSourceColumnName,
            cbxEdgeVisibilitySourceColumnName,
            };

        m_aoVertexSourceColumnNameComboBoxes =
            new AutoFillVertexColumnComboBox []
            {
            cbxVertexColorSourceColumnName,
            cbxVertexShapeSourceColumnName,
            cbxVertexRadiusSourceColumnName,
            cbxVertexAlphaSourceColumnName,
            cbxVertexPrimaryLabelSourceColumnName,
            cbxVertexPrimaryLabelFillColorSourceColumnName,
            cbxVertexSecondaryLabelSourceColumnName,
            cbxVertexToolTipSourceColumnName,
            cbxVertexVisibilitySourceColumnName,
            cbxVertexLayoutOrderSourceColumnName,
            cbxVertexXSourceColumnName,
            cbxVertexYSourceColumnName,
            cbxVertexPolarRSourceColumnName,
            cbxVertexPolarAngleSourceColumnName,
            };

        InitializeLabels();
        InitializeEdgeComboBoxes(m_oWorkbook);
        InitializeVertexComboBoxes(m_oWorkbook);

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: AutoFillWorkbookDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoFillWorkbookDialog" />
    /// class for the Visual Studio designer.
    /// </summary>
    ///
    /// <remarks>
    /// Do not use this constructor.  It is for use by the Visual Studio
    /// designer only.
    /// </remarks>
    //*************************************************************************

    public AutoFillWorkbookDialog()
    {
        InitializeComponent();

        // AssertValid();
    }

    //*************************************************************************
    //  Event: WorkbookAutoFilled
    //
    /// <summary>
    /// Occurs when the workbook is autofilled.
    /// </summary>
    //*************************************************************************

    public event EventHandler WorkbookAutoFilled;


    //*************************************************************************
    //  Method: InitializeLabels()
    //
    /// <summary>
    /// Initializes the Labels that represent worksheet columns.
    /// </summary>
    //*************************************************************************

    protected void
    InitializeLabels()
    {
        // Make the worksheet, table, and column names available to
        // tsmClearColumn_Click().

        String sEdgePrefix =
            WorksheetNames.Edges + "\t" + TableNames.Edges + "\t";

        lblEdgeColor.Tag = sEdgePrefix + EdgeTableColumnNames.Color;
        lblEdgeWidth.Tag = sEdgePrefix + EdgeTableColumnNames.Width;
        lblEdgeAlpha.Tag = sEdgePrefix + EdgeTableColumnNames.Alpha;
        lblEdgeVisibility.Tag = sEdgePrefix + EdgeTableColumnNames.Visibility;

        String sVertexPrefix =
            WorksheetNames.Vertices + "\t" + TableNames.Vertices + "\t";

        lblVertexColor.Tag = sVertexPrefix + VertexTableColumnNames.Color;
        lblVertexShape.Tag = sVertexPrefix + VertexTableColumnNames.Shape;
        lblVertexRadius.Tag = sVertexPrefix + VertexTableColumnNames.Radius;
        lblVertexAlpha.Tag = sVertexPrefix + VertexTableColumnNames.Alpha;

        lblVertexPrimaryLabel.Tag =
            sVertexPrefix + VertexTableColumnNames.PrimaryLabel;

        lblVertexPrimaryLabelFillColor.Tag =
            sVertexPrefix + VertexTableColumnNames.PrimaryLabelFillColor;

        lblVertexSecondaryLabel.Tag =
            sVertexPrefix + VertexTableColumnNames.SecondaryLabel;

        lblVertexToolTip.Tag = sVertexPrefix + VertexTableColumnNames.ToolTip;

        lblVertexVisibility.Tag =
            sVertexPrefix + VertexTableColumnNames.Visibility;

        lblVertexLayoutOrder.Tag =
            sVertexPrefix + VertexTableColumnNames.LayoutOrder;

        lblVertexX.Tag = sVertexPrefix + VertexTableColumnNames.X;
        lblVertexY.Tag = sVertexPrefix + VertexTableColumnNames.Y;

        lblVertexPolarR.Tag = sVertexPrefix + VertexTableColumnNames.PolarR;

        lblVertexPolarAngle.Tag =
            sVertexPrefix + VertexTableColumnNames.PolarAngle;
    }

    //*************************************************************************
    //  Method: InitializeEdgeComboBoxes()
    //
    /// <summary>
    /// Initializes the ComboBoxes used to specify the data sources for the
    /// edge table columns.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the graph data.
    /// </param>
    //*************************************************************************

    protected void
    InitializeEdgeComboBoxes
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook
    )
    {
        Debug.Assert(oWorkbook != null);

        ListObject oEdgeTable;

        if ( ExcelUtil.TryGetTable(oWorkbook, WorksheetNames.Edges,
            TableNames.Edges, out oEdgeTable) )
        {
            // Populate the edge table column ComboBoxes with the source column
            // names.

            foreach (AutoFillEdgeColumnComboBox oComboBox in
                m_aoEdgeSourceColumnNameComboBoxes)
            {
                oComboBox.PopulateWithSourceColumnNames(oEdgeTable);
            }
        }

        // Store the name of the column corresponding to the ComboBox in each
        // ComboBox's Tag.  This gets used for error checking by
        // DoDataExchange().

        cbxEdgeColorSourceColumnName.Tag = EdgeTableColumnNames.Color;
        cbxEdgeWidthSourceColumnName.Tag = EdgeTableColumnNames.Width;
        cbxEdgeAlphaSourceColumnName.Tag = EdgeTableColumnNames.Alpha;
        cbxEdgeVisibilitySourceColumnName.Tag = EdgeTableColumnNames.Visibility;
    }

    //*************************************************************************
    //  Method: InitializeVertexComboBoxes()
    //
    /// <summary>
    /// Initializes the ComboBoxes used to specify the data sources for the
    /// vertex table columns.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the graph data.
    /// </param>
    //*************************************************************************

    protected void
    InitializeVertexComboBoxes
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook
    )
    {
        Debug.Assert(oWorkbook != null);

        ListObject oVertexTable;

        if ( ExcelUtil.TryGetTable(oWorkbook, WorksheetNames.Vertices,
            TableNames.Vertices, out oVertexTable) )
        {
            // Populate the vertex table column ComboBoxes with the source
            // column names.

            foreach (AutoFillVertexColumnComboBox oComboBox in
                m_aoVertexSourceColumnNameComboBoxes)
            {
                oComboBox.PopulateWithSourceColumnNames(oVertexTable);
            }

            // Insert a few special items.

            foreach (ComboBox oComboBox in new ComboBox [] {
                cbxVertexPrimaryLabelSourceColumnName,
                cbxVertexSecondaryLabelSourceColumnName,
                cbxVertexToolTipSourceColumnName
                } )
            {
                oComboBox.Items.Insert(0, VertexTableColumnNames.VertexName);
            }
        }

        // Store the name of the column corresponding to the ComboBox in each
        // ComboBox's Tag.  This gets used for error checking by
        // DoDataExchange().

        cbxVertexColorSourceColumnName.Tag = VertexTableColumnNames.Color;
        cbxVertexShapeSourceColumnName.Tag = VertexTableColumnNames.Shape;
        cbxVertexRadiusSourceColumnName.Tag = VertexTableColumnNames.Radius;
        cbxVertexAlphaSourceColumnName.Tag = VertexTableColumnNames.Alpha;

        cbxVertexPrimaryLabelSourceColumnName.Tag =
            VertexTableColumnNames.PrimaryLabel;

        cbxVertexPrimaryLabelFillColorSourceColumnName.Tag =
            VertexTableColumnNames.PrimaryLabelFillColor;

        cbxVertexSecondaryLabelSourceColumnName.Tag =
            VertexTableColumnNames.SecondaryLabel;

        cbxVertexToolTipSourceColumnName.Tag = VertexTableColumnNames.ToolTip;

        cbxVertexVisibilitySourceColumnName.Tag =
            VertexTableColumnNames.Visibility;

        cbxVertexLayoutOrderSourceColumnName.Tag =
            VertexTableColumnNames.LayoutOrder;

        cbxVertexXSourceColumnName.Tag = VertexTableColumnNames.X;
        cbxVertexYSourceColumnName.Tag = VertexTableColumnNames.Y;

        cbxVertexPolarRSourceColumnName.Tag = VertexTableColumnNames.PolarR;

        cbxVertexPolarAngleSourceColumnName.Tag =
            VertexTableColumnNames.PolarAngle;
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
            if ( !ValidateSourceColumnNameComboBoxes(
                    m_aoEdgeSourceColumnNameComboBoxes)
                ||
                !ValidateSourceColumnNameComboBoxes(
                    m_aoVertexSourceColumnNameComboBoxes)
                )
            {
                return (false);
            }

            String sVertexXSourceColumnName = GetSourceColumnNameFromComboBox(
                cbxVertexXSourceColumnName);

            String sVertexYSourceColumnName = GetSourceColumnNameFromComboBox(
                cbxVertexYSourceColumnName);

            if (
                (sVertexXSourceColumnName.Length == 0 &&
                    sVertexYSourceColumnName.Length > 0)
                ||
                (sVertexYSourceColumnName.Length == 0 &&
                    sVertexXSourceColumnName.Length > 0)
                )
            {
                return ( this.OnInvalidComboBox(cbxVertexXSourceColumnName,
                    "If you autofill one of the Vertex X or Vertex Y columns,"
                    + " you must autofill both of them."
                    ) );
            }

            m_oAutoFillUserSettings.EdgeColorSourceColumnName =
                GetSourceColumnNameFromComboBox(cbxEdgeColorSourceColumnName);

            m_oAutoFillUserSettings.EdgeWidthSourceColumnName =
                GetSourceColumnNameFromComboBox(cbxEdgeWidthSourceColumnName);

            m_oAutoFillUserSettings.EdgeAlphaSourceColumnName =
                GetSourceColumnNameFromComboBox(cbxEdgeAlphaSourceColumnName);

            m_oAutoFillUserSettings.EdgeVisibilitySourceColumnName =
                GetSourceColumnNameFromComboBox(
                    cbxEdgeVisibilitySourceColumnName);

            m_oAutoFillUserSettings.VertexColorSourceColumnName =
                GetSourceColumnNameFromComboBox(
                    cbxVertexColorSourceColumnName);

            m_oAutoFillUserSettings.VertexShapeSourceColumnName =
                GetSourceColumnNameFromComboBox(cbxVertexShapeSourceColumnName);

            m_oAutoFillUserSettings.VertexRadiusSourceColumnName =
                GetSourceColumnNameFromComboBox(
                    cbxVertexRadiusSourceColumnName);

            m_oAutoFillUserSettings.VertexAlphaSourceColumnName =
                GetSourceColumnNameFromComboBox(
                    cbxVertexAlphaSourceColumnName);

            m_oAutoFillUserSettings.VertexPrimaryLabelSourceColumnName =
                GetSourceColumnNameFromComboBox(
                    cbxVertexPrimaryLabelSourceColumnName);

            m_oAutoFillUserSettings.VertexPrimaryLabelFillColorSourceColumnName
                = GetSourceColumnNameFromComboBox(
                    cbxVertexPrimaryLabelFillColorSourceColumnName);

            m_oAutoFillUserSettings.VertexSecondaryLabelSourceColumnName =
                GetSourceColumnNameFromComboBox(
                    cbxVertexSecondaryLabelSourceColumnName);

            m_oAutoFillUserSettings.VertexToolTipSourceColumnName =
                GetSourceColumnNameFromComboBox(
                    cbxVertexToolTipSourceColumnName);

            m_oAutoFillUserSettings.VertexVisibilitySourceColumnName =
                GetSourceColumnNameFromComboBox(
                    cbxVertexVisibilitySourceColumnName);

            m_oAutoFillUserSettings.VertexLayoutOrderSourceColumnName =
                GetSourceColumnNameFromComboBox(
                    cbxVertexLayoutOrderSourceColumnName);

            m_oAutoFillUserSettings.VertexXSourceColumnName =
                sVertexXSourceColumnName;

            m_oAutoFillUserSettings.VertexYSourceColumnName =
                sVertexYSourceColumnName;

            m_oAutoFillUserSettings.VertexPolarRSourceColumnName =
                GetSourceColumnNameFromComboBox(
                    cbxVertexPolarRSourceColumnName);

            m_oAutoFillUserSettings.VertexPolarAngleSourceColumnName =
                GetSourceColumnNameFromComboBox(
                    cbxVertexPolarAngleSourceColumnName);
        }
        else
        {
            cbxEdgeColorSourceColumnName.Text =
                m_oAutoFillUserSettings.EdgeColorSourceColumnName;

            cbxEdgeWidthSourceColumnName.Text =
                m_oAutoFillUserSettings.EdgeWidthSourceColumnName;

            cbxEdgeAlphaSourceColumnName.Text =
                m_oAutoFillUserSettings.EdgeAlphaSourceColumnName;

            cbxEdgeVisibilitySourceColumnName.Text =
                m_oAutoFillUserSettings.EdgeVisibilitySourceColumnName;

            cbxVertexColorSourceColumnName.Text =
                m_oAutoFillUserSettings.VertexColorSourceColumnName;

            cbxVertexShapeSourceColumnName.Text =
                m_oAutoFillUserSettings.VertexShapeSourceColumnName;

            cbxVertexRadiusSourceColumnName.Text =
                m_oAutoFillUserSettings.VertexRadiusSourceColumnName;

            cbxVertexAlphaSourceColumnName.Text =
                m_oAutoFillUserSettings.VertexAlphaSourceColumnName;

            cbxVertexPrimaryLabelSourceColumnName.Text =
                m_oAutoFillUserSettings.VertexPrimaryLabelSourceColumnName;

            cbxVertexPrimaryLabelFillColorSourceColumnName.Text =
                m_oAutoFillUserSettings.
                    VertexPrimaryLabelFillColorSourceColumnName;

            cbxVertexSecondaryLabelSourceColumnName.Text =
                m_oAutoFillUserSettings.VertexSecondaryLabelSourceColumnName;

            cbxVertexToolTipSourceColumnName.Text =
                m_oAutoFillUserSettings.VertexToolTipSourceColumnName;

            cbxVertexVisibilitySourceColumnName.Text =
                m_oAutoFillUserSettings.VertexVisibilitySourceColumnName;

            cbxVertexLayoutOrderSourceColumnName.Text =
                m_oAutoFillUserSettings.VertexLayoutOrderSourceColumnName;

            cbxVertexXSourceColumnName.Text =
                m_oAutoFillUserSettings.VertexXSourceColumnName;

            cbxVertexYSourceColumnName.Text =
                m_oAutoFillUserSettings.VertexYSourceColumnName;

            cbxVertexPolarRSourceColumnName.Text =
                m_oAutoFillUserSettings.VertexPolarRSourceColumnName;

            cbxVertexPolarAngleSourceColumnName.Text =
                m_oAutoFillUserSettings.VertexPolarAngleSourceColumnName;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: ValidateSourceColumnNameComboBoxes()
    //
    /// <summary>
    /// Validates the text in an array of ComboBoxes that may contain source
    /// column names.
    /// </summary>
    ///
    /// <param name="aoSourceColumnNameComboBoxes">
    /// Array of ComboBoxes that may contain source column names.
    /// </param>
    ///
    /// <returns>
    /// true if the ComboBoxes all contain valid text.
    /// </returns>
    //*************************************************************************

    protected Boolean
    ValidateSourceColumnNameComboBoxes
    (
        ComboBox [] aoSourceColumnNameComboBoxes
    )
    {
        Debug.Assert(aoSourceColumnNameComboBoxes != null);

        foreach (ComboBox oComboBox in aoSourceColumnNameComboBoxes)
        {
            if (oComboBox.Tag is String)
            {
                // The name of the column corresponding to each ComboBox is
                // stored in the ComboBox's Tag.  Use the column name to
                // prevent the user from trying to autofill a column with
                // itself.

                if (oComboBox.Text.ToLower() ==
                    ( (String)oComboBox.Tag ).ToLower() )
                {
                    this.OnInvalidComboBox(oComboBox,
                        "You can't autofill a column with itself."
                        );

                    return (false);
                }
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: GetSourceColumnNameFromComboBox()
    //
    /// <summary>
    /// Gets a source column name from a ComboBox.
    /// </summary>
    ///
    /// <param name="oComboBox">
    /// ComboBox to get a source column name from.
    /// </param>
    ///
    /// <returns>
    /// The text in <paramref name="oComboBox" />, or String.Empty if the
    /// ComboBox contains nothing but spaces.
    /// </returns>
    //*************************************************************************

    protected String
    GetSourceColumnNameFromComboBox
    (
        ComboBox oComboBox
    )
    {
        Debug.Assert(oComboBox != null);
        AssertValid();

        String sSourceColumnName = oComboBox.Text;

        if (sSourceColumnName.Trim().Length == 0)
        {
            sSourceColumnName = String.Empty;
        }

        return (sSourceColumnName);
    }

    //*************************************************************************
    //  Method: GetComboBoxFromLabelName()
    //
    /// <summary>
    /// Gets the ComboBox associated with a Label that represents a worksheet
    /// column.
    /// </summary>
    ///
    /// <param name="sLabelName">
    /// Name of a Label that represents a worksheet column.
    /// </param>
    ///
    /// <returns>
    /// The associated ComboBox, which contains the name of the source column.
    /// </returns>
    //*************************************************************************

    protected ComboBox
    GetComboBoxFromLabelName
    (
        String sLabelName
    )
    {
        AssertValid();

        // Sample label name: "lblEdgeColor"

        Debug.Assert( sLabelName.StartsWith("lbl") );

        // Sample corresponding ComboBox name: "cbxEdgeColorSourceColumnName";

        String sComboBoxName =
            "cbx" + sLabelName.Substring(3) + "SourceColumnName";

        Control [] aoComboBox =
            tlpTableLayoutPanel.Controls.Find(sComboBoxName, false);

        Debug.Assert(aoComboBox.Length == 1);
        Debug.Assert(aoComboBox[0] is ComboBox);

        return ( (ComboBox)( aoComboBox[0] ) );
    }

    //*************************************************************************
    //  Method: TryGetDetailsButtonFromLabelName()
    //
    /// <summary>
    /// Attempts to get the details button associated with a Label that
    /// represents a worksheet column.
    /// </summary>
    ///
    /// <param name="sLabelName">
    /// Name of a Label that represents a worksheet column.
    /// </param>
    ///
    /// <param name="oDetailsButton">
    /// Where the associated details button gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if there is a details button associated with the specified Label.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetDetailsButtonFromLabelName
    (
        String sLabelName,
        out System.Windows.Forms.Button oDetailsButton
    )
    {
        AssertValid();

        oDetailsButton = null;

        // Sample label name: "lblEdgeColor"

        Debug.Assert( sLabelName.StartsWith("lbl") );

        // Sample corresponding details button name: "btnEdgeColorDetails"

        String sDetailsButtonName =
            "btn" + sLabelName.Substring(3) + "Details";

        Control [] aoDetailsButton =
            tlpTableLayoutPanel.Controls.Find(sDetailsButtonName, false);

        if (aoDetailsButton.Length == 0)
        {
            // There is no corresponding details button.

            return (false);
        }

        Debug.Assert(aoDetailsButton.Length == 1);
        Debug.Assert(aoDetailsButton[0] is System.Windows.Forms.Button);

        oDetailsButton = (System.Windows.Forms.Button)( aoDetailsButton[0] );

        return (true);
    }

    //*************************************************************************
    //  Method: AutoFillWorkbookDialog_FormClosing()
    //
    /// <summary>
    /// Handles the FormClosing event.
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
    AutoFillWorkbookDialog_FormClosing
    (
        object sender,
        FormClosingEventArgs e
    )
    {
        if ( DoDataExchange(true) )
        {
            m_oAutoFillUserSettings.Save();
        }
        else
        {
            e.Cancel = true;
        }
    }

    //*************************************************************************
    //  Method: btnEdgeColorDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnEdgeColorDetails button.
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
    btnEdgeColorDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        ColorColumnAutoFillUserSettingsDialog
            oColorColumnAutoFillUserSettingsDialog =
            new ColorColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.EdgeColorDetails,
                "Edge Color Options");

        oColorColumnAutoFillUserSettingsDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: btnEdgeWidthDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnEdgeWidthDetails button.
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
    btnEdgeWidthDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        NumericRangeColumnAutoFillUserSettingsDialog
            oNumericRangeColumnAutoFillUserSettingsDialog =
            new NumericRangeColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.EdgeWidthDetails,
                "Edge Width Options", "edge width", "Widths",
                EdgeWidthConverter.MinimumWidthWorkbook,
                EdgeWidthConverter.MaximumWidthWorkbook
                );

        oNumericRangeColumnAutoFillUserSettingsDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: btnEdgeAlphaDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnEdgeAlphaDetails button.
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
    btnEdgeAlphaDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        NumericRangeColumnAutoFillUserSettingsDialog
            oNumericRangeColumnAutoFillUserSettingsDialog =
            new NumericRangeColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.EdgeAlphaDetails,
                "Edge Opacity Options", "edge opacity", "Opacities",
                AlphaConverter.MinimumAlphaWorkbook,
                AlphaConverter.MaximumAlphaWorkbook
                );

        oNumericRangeColumnAutoFillUserSettingsDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: btnEdgeVisibilityDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnEdgeVisibilityDetails button.
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
    btnEdgeVisibilityDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        NumericComparisonColumnAutoFillUserSettingsDialog
            oNumericComparisonColumnAutoFillUserSettingsDialog =
            new NumericComparisonColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.EdgeVisibilityDetails,
                "Edge Visibility Options",
                "&Show the edge if the source column number is:",
                "Otherwise, skip the edge"
                );

        oNumericComparisonColumnAutoFillUserSettingsDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: btnVertexColorDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnVertexColorDetails button.
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
    btnVertexColorDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        ColorColumnAutoFillUserSettingsDialog
            oColorColumnAutoFillUserSettingsDialog =
            new ColorColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.VertexColorDetails,
                "Vertex Color Options");

        oColorColumnAutoFillUserSettingsDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: btnVertexShapeDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnVertexShapeDetails button.
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
    btnVertexShapeDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        VertexShapeColumnAutoFillUserSettingsDialog
            oVertexShapeColumnAutoFillUserSettingsDialog =
            new VertexShapeColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.VertexShapeDetails
                );

        oVertexShapeColumnAutoFillUserSettingsDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: btnVertexRadiusDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnVertexRadiusDetails button.
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
    btnVertexRadiusDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        NumericRangeColumnAutoFillUserSettingsDialog
            oNumericRangeColumnAutoFillUserSettingsDialog =
            new NumericRangeColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.VertexRadiusDetails,
                "Vertex Size Options", "vertex size", "Sizes",
                VertexRadiusConverter.MinimumRadiusWorkbook,
                VertexRadiusConverter.MaximumRadiusWorkbook
                );

        oNumericRangeColumnAutoFillUserSettingsDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: btnVertexAlphaDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnVertexAlphaDetails button.
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
    btnVertexAlphaDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        NumericRangeColumnAutoFillUserSettingsDialog
            oNumericRangeColumnAutoFillUserSettingsDialog =
            new NumericRangeColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.VertexAlphaDetails,
                "Vertex Opacity Options", "vertex opacity", "Opacities",
                AlphaConverter.MinimumAlphaWorkbook,
                AlphaConverter.MaximumAlphaWorkbook
                );

        oNumericRangeColumnAutoFillUserSettingsDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: btnVertexPrimaryLabelFillColorDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnVertexPrimaryLabelFillColorDetails
    /// button.
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
    btnVertexPrimaryLabelFillColorDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        ColorColumnAutoFillUserSettingsDialog
            oColorColumnAutoFillUserSettingsDialog =
            new ColorColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.VertexPrimaryLabelFillColorDetails,
                "Vertex Primary Label Fill Color Options");

        oColorColumnAutoFillUserSettingsDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: btnVertexVisibilityDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnVertexVisibilityDetails button.
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
    btnVertexVisibilityDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        NumericComparisonColumnAutoFillUserSettingsDialog
            oNumericComparisonColumnAutoFillUserSettingsDialog =
            new NumericComparisonColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.VertexVisibilityDetails,
                "Vertex Visibility Options",

                "&Show the vertex if it is part of an edge and the source"
                    + " column number is:",

                "Otherwise, skip the vertex"
                );

        oNumericComparisonColumnAutoFillUserSettingsDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: btnVertexLayoutOrderDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnVertexLayoutOrderDetails button.
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
    btnVertexLayoutOrderDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        NumericRangeColumnAutoFillUserSettingsDialog
            oNumericRangeColumnAutoFillUserSettingsDialog =
            new NumericRangeColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.VertexLayoutOrderDetails,
                "Vertex Layout Order Options", "vertex layout order", "Orders",
                1, 99999
                );

        oNumericRangeColumnAutoFillUserSettingsDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: btnVertexXDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnVertexXDetails button.
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
    btnVertexXDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        NumericRangeColumnAutoFillUserSettingsDialog
            oNumericRangeColumnAutoFillUserSettingsDialog =
            new NumericRangeColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.VertexXDetails,
                "Vertex X Options", "vertex x-coordinate",
                CoordinateColumnNamePlural,
                VertexLocationConverter.MinimumXYWorkbook,
                VertexLocationConverter.MaximumXYWorkbook
                );

        oNumericRangeColumnAutoFillUserSettingsDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: btnVertexYDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnVertexYDetails button.
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
    btnVertexYDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        NumericRangeColumnAutoFillUserSettingsDialog
            oNumericRangeColumnAutoFillUserSettingsDialog =
            new NumericRangeColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.VertexYDetails,
                "Vertex Y Options", "vertex y-coordinate",
                CoordinateColumnNamePlural,
                VertexLocationConverter.MinimumXYWorkbook,
                VertexLocationConverter.MaximumXYWorkbook
                );

        oNumericRangeColumnAutoFillUserSettingsDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: btnVertexPolarRDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnVertexPolarRDetails button.
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
    btnVertexPolarRDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        NumericRangeColumnAutoFillUserSettingsDialog
            oNumericRangeColumnAutoFillUserSettingsDialog =
            new NumericRangeColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.VertexPolarRDetails,
                "Vertex Polar R Options", "vertex polar R coordinate",
                CoordinateColumnNamePlural,
                0, 1
                );

        oNumericRangeColumnAutoFillUserSettingsDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: btnVertexPolarAngleDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnVertexPolarAngleDetails button.
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
    btnVertexPolarAngleDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        NumericRangeColumnAutoFillUserSettingsDialog
            oNumericRangeColumnAutoFillUserSettingsDialog =
            new NumericRangeColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.VertexPolarAngleDetails,
                "Vertex Polar Angle Options", "vertex polar angle coordinate",
                CoordinateColumnNamePlural,
                -99999,
                99999
                );

        oNumericRangeColumnAutoFillUserSettingsDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: lnkHowAutoFillWorks_LinkClicked()
    //
    /// <summary>
    /// Handles the LinkClicked event on the lnkHowAutoFillWorks LinkButton.
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
    lnkHowAutoFillWorks_LinkClicked
    (
        object sender,
        LinkLabelLinkClickedEventArgs e
    )
    {
        AssertValid();

        Debug.Assert(lnkHowAutoFillWorks.Tag is String);

        this.ShowInformation( (String)lnkHowAutoFillWorks.Tag );
    }

    //*************************************************************************
    //  Method: cmsWorksheetColumn_Opening()
    //
    /// <summary>
    /// Handles the Opening event on the cmsWorksheetColumn ContextMenuStrip.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    ///
    /// <remarks>
    /// The ContextMenuStrip is the context menu attached to every Label that
    /// represents a worksheet column.
    /// </remarks>
    //*************************************************************************

    private void
    cmsWorksheetColumn_Opening
    (
        object sender,
        System.ComponentModel.CancelEventArgs e
    )
    {
        AssertValid();

        Debug.Assert(this.cmsWorksheetColumn.SourceControl is
            System.Windows.Forms.Label);

        System.Windows.Forms.Label oLabel =
            (System.Windows.Forms.Label)this.cmsWorksheetColumn.SourceControl;

        String sLabelName = oLabel.Name;
        String sWorksheetColumnName = oLabel.Text.Replace("&", String.Empty);

        if ( sWorksheetColumnName.Contains("Fill Color") )
        {
            sWorksheetColumnName = "Vertex Primary Label Fill Color";
        }

        this.tsmClearColumn.Text =
            "Clear " + sWorksheetColumnName + " &Worksheet Column Now";

        this.tsmClearSourceColumnName.Text =
            "Clear " + sWorksheetColumnName + " &Source Column Name";

        this.tsmWorksheetColumnDetails.Text =
            sWorksheetColumnName + " &Options...";

        this.tsmClearSourceColumnName.Enabled =
            (GetComboBoxFromLabelName(sLabelName).Text.Length > 0);

        System.Windows.Forms.Button oDetailsButton;

        this.tsmWorksheetColumnDetails.Enabled =
            TryGetDetailsButtonFromLabelName(sLabelName, out oDetailsButton);
    }

    //*************************************************************************
    //  Method: tsmClearColumn_Click()
    //
    /// <summary>
    /// Handles the Click event on the tsmClearColumn ToolStripMenuItem.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    ///
    /// <remarks>
    /// The tsmClearColumn ToolStripMenuItem is part of the cmsWorksheetColumn
    /// ContextMenuStrip, which is the context menu attached to every Label
    /// that represents a worksheet column.
    /// </remarks>
    //*************************************************************************

    private void
    tsmClearColumn_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        // InitializeLabels() stored the worksheet, table, and column names in
        // the Label's Tag.

        Debug.Assert(this.cmsWorksheetColumn.SourceControl.Tag is String);
        String sTag = (String)this.cmsWorksheetColumn.SourceControl.Tag;
        String [] asNames = sTag.Split('\t');

        Debug.Assert(asNames.Length == 3);

        String sWorksheetName = asNames[0];
        String sTableName = asNames[1];
        String sColumnName = asNames[2];

        ListObject oTable;

        if ( ExcelUtil.TryGetTable(m_oWorkbook, sWorksheetName, sTableName,
                out oTable) )
        {
            ExcelUtil.TryClearTableColumnDataContents(oTable, sColumnName);
        }
    }

    //*************************************************************************
    //  Method: tsmClearSourceColumnName_Click()
    //
    /// <summary>
    /// Handles the Click event on the tsmClearSourceColumnName
    /// ToolStripMenuItem.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    ///
    /// <remarks>
    /// The tsmClearSourceColumnName ToolStripMenuItem is part of the
    /// cmsWorksheetColumn ContextMenuStrip, which is the context menu attached
    /// to every Label that represents a worksheet column.
    /// </remarks>
    //*************************************************************************

    private void
    tsmClearSourceColumnName_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        GetComboBoxFromLabelName(this.cmsWorksheetColumn.SourceControl.Name).
            Text = String.Empty;
    }

    //*************************************************************************
    //  Method: tsmClearAllSourceColumnNames_Click()
    //
    /// <summary>
    /// Handles the Click event on the tsmClearAllSourceColumnNames
    /// ToolStripMenuItem.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    ///
    /// <remarks>
    /// The tsmClearSourceColumnName ToolStripMenuItem is part of the
    /// cmsWorksheetColumn ContextMenuStrip, which is the context menu attached
    /// to every Label that represents a worksheet column.
    /// </remarks>
    //*************************************************************************

    private void
    tsmClearAllSourceColumnNames_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        foreach (Control oControl in this.tlpTableLayoutPanel.Controls)
        {
            if (oControl is ComboBox)
            {
                ( (ComboBox)oControl ).Text = String.Empty;
            }
        }
    }

    //*************************************************************************
    //  Method: tsmWorksheetColumnDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the tsmWorksheetColumnDetails
    /// ToolStripMenuItem.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    ///
    /// <remarks>
    /// The tsmWorksheetColumnDetails ToolStripMenuItem is part of the
    /// cmsWorksheetColumn ContextMenuStrip, which is the context menu attached
    /// to every Label that represents a worksheet column.
    /// </remarks>
    //*************************************************************************

    private void
    tsmWorksheetColumnDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        System.Windows.Forms.Button oDetailsButton;

        // Get and click the details button corresponding to the right-clicked
        // Label.

        if ( TryGetDetailsButtonFromLabelName(
            this.cmsWorksheetColumn.SourceControl.Name, out oDetailsButton) )
        {
            oDetailsButton.PerformClick();
        }
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

        if ( MessageBox.Show(this,

                "This will clear all the source column names and reset any"
                + " options you've changed.  It will not modify any worksheet"
                + " columns.\r\n\r\nDo you want to reset all?"
                ,
                ApplicationUtil.ApplicationName, MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
                == DialogResult.Yes
            )
        {
            m_oAutoFillUserSettings.Reset();
            DoDataExchange(false);
        }
    }

    //*************************************************************************
    //  Method: btnAutoFill_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnAutoFill button.
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
    btnAutoFill_Click
    (
        object sender,
        System.EventArgs e
    )
    {
        if ( DoDataExchange(true) )
        {
            try
            {
                WorkbookAutoFiller.AutoFillWorkbook(
                    m_oWorkbook, m_oAutoFillUserSettings);

                EventUtil.FireEvent(this, this.WorkbookAutoFilled);
            }
            catch (Exception oException)
            {
                ErrorUtil.OnException(oException);
            }
        }
    }

    //*************************************************************************
    //  Method: btnClose_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnClose button.
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
    btnClose_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        this.Close();
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
        Debug.Assert(m_oAutoFillUserSettings != null);
        Debug.Assert(m_oAutoFillWorkbookDialogUserSettings != null);
        Debug.Assert(m_aoEdgeSourceColumnNameComboBoxes != null);
        Debug.Assert(m_aoVertexSourceColumnNameComboBoxes != null);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Message to display when the user asks for an explanation of how
    /// outliers are ignored.

    protected internal const String IgnoreOutliersMessage =

        "If you map from the smallest and largest numbers in the source"
        + " column, the results may be skewed by a few unusually small or"
        + " large numbers, or \"outliers.\"  You can prevent this by checking"
        + " the \"Ignore outliers\" checkbox, which causes all source column"
        + " numbers that fall outside one standard deviation of the average"
        + " number to be ignored in the internal calculations that determine"
        + " how the numbers are mapped."
        ;

    /// destinationColumnNamePlural argument to the
    /// NumericRangeColumnAutoFillUserSettingsDialog constructor for coordinate
    /// columns.

    protected const String CoordinateColumnNamePlural = "Coordinates";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Workbook containing the graph data.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

    /// AutoFill user settings edited by this dialog.

    protected AutoFillUserSettings m_oAutoFillUserSettings;

    /// User settings for this dialog.

    protected AutoFillWorkbookDialogUserSettings
        m_oAutoFillWorkbookDialogUserSettings;

    /// Array of ComboBoxes for the edge source column names.

    protected AutoFillEdgeColumnComboBox [] m_aoEdgeSourceColumnNameComboBoxes;

    /// Array of ComboBoxes for the vertex source column names.

    protected AutoFillVertexColumnComboBox []
        m_aoVertexSourceColumnNameComboBoxes;
}


//*****************************************************************************
//  Class: AutoFillWorkbookDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="AutoFillWorkbookDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("AutoFillWorkbookDialog") ]

public class AutoFillWorkbookDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: AutoFillWorkbookDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AutoFillWorkbookDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public AutoFillWorkbookDialogUserSettings
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
