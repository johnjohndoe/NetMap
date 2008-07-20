

//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//	Class: AutoFillUserSettingsDialog
//
/// <summary>
///	Edits a <see cref="AutoFillUserSettings" /> object.
/// </summary>
///
/// <remarks>
///	Pass a <see cref="AutoFillUserSettings" /> object to the constructor, which
/// makes a copy of it.  If the user edits the copy, <see
/// cref="Form.ShowDialog()" /> returns DialogResult.OK and the edited copy can
/// be obtained from the <see cref="AutoFillUserSettings" /> property.
/// Otherwise, <see cref="Form.ShowDialog()" /> returns DialogResult.Cancel.
/// </remarks>
//*****************************************************************************

public partial class AutoFillUserSettingsDialog : ExcelTemplateForm
{
	//*************************************************************************
	//	Constructor: AutoFillUserSettingsDialog()
	//
	/// <overloads>
	///	Initializes a new instance of the <see
	/// cref="AutoFillUserSettingsDialog" /> class.
	/// </overloads>
	///
	/// <summary>
	///	Initializes a new instance of the <see
	/// cref="AutoFillUserSettingsDialog" /> class with an AutoFillUserSettings
	/// object.
	/// </summary>
	///
    /// <param name="workbook">
	/// Workbook containing the graph data.
    /// </param>
	///
	/// <param name="autoFillUserSettings">
	/// The object to copy and edit.
	/// </param>
	//*************************************************************************

	public AutoFillUserSettingsDialog
	(
        Microsoft.Office.Interop.Excel.Workbook workbook,
		AutoFillUserSettings autoFillUserSettings
	)
	: this()
	{
		Debug.Assert(workbook != null);
		Debug.Assert(autoFillUserSettings != null);

		m_oAutoFillUserSettings = autoFillUserSettings.Copy();

		// Instantiate an object that retrieves and saves the position of this
		// dialog.  Note that the object automatically saves the settings when
		// the form closes.

		m_oAutoFillUserSettingsDialogUserSettings =
			new AutoFillUserSettingsDialogUserSettings(this);

		// Initialize the ComboBoxes used to specify the data sources for the
		// edge and vertex table columns.

		m_aoEdgeSourceColumnNameComboBoxes = new ComboBox [] {
			cbxEdgeColorSourceColumnName,
			cbxEdgeWidthSourceColumnName,
			cbxEdgeAlphaSourceColumnName,
			cbxEdgeVisibilitySourceColumnName,
			};

		m_aoVertexSourceColumnNameComboBoxes = new ComboBox [] {
			cbxVertexColorSourceColumnName,
			cbxVertexShapeSourceColumnName,
			cbxVertexRadiusSourceColumnName,
			cbxVertexAlphaSourceColumnName,
			cbxVertexPrimaryLabelSourceColumnName,
			cbxVertexSecondaryLabelSourceColumnName,
			cbxVertexToolTipSourceColumnName,
			cbxVertexVisibilitySourceColumnName,
			cbxVertexXSourceColumnName,
			cbxVertexYSourceColumnName,
			};

		InitializeEdgeComboBoxes(workbook);
		InitializeVertexComboBoxes(workbook);

		DoDataExchange(false);

		AssertValid();
	}

	//*************************************************************************
	//	Constructor: AutoFillUserSettingsDialog()
	//
	/// <summary>
	///	Initializes a new instance of the <see
	/// cref="AutoFillUserSettingsDialog" /> class for the Visual Studio
	/// designer.
	/// </summary>
	///
	/// <remarks>
	/// Do not use this constructor.  It is for use by the Visual Studio
	/// designer only.
	/// </remarks>
	//*************************************************************************

	public AutoFillUserSettingsDialog()
	{
		InitializeComponent();

		// AssertValid();
	}

    //*************************************************************************
    //  Property: AutoFillUserSettings
    //
    /// <summary>
    /// Gets the edited AutoFillUserSettings object.
    /// </summary>
    ///
    /// <value>
	/// The edited AutoFillUserSettings object.  This is an edited copy of the
	/// object that was passed to the constructor.
    /// </value>
	///
    /// <remarks>
	/// Read this property only if <see cref="Form.ShowDialog()" /> returns
	/// DialogResult.OK.  It is invalid otherwise.
    /// </remarks>
    //*************************************************************************

    public AutoFillUserSettings
	AutoFillUserSettings
    {
        get
        {
			AssertValid();
			Debug.Assert(DialogResult == DialogResult.OK);

			return (m_oAutoFillUserSettings);
        }
    }

	//*************************************************************************
	//	Method: InitializeEdgeComboBoxes()
	//
	/// <summary>
	///	Initializes the ComboBoxes used to specify the data sources for the
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
			// Get the names of the edge table columns that can be used as a
			// data source.

			String [] asEdgeTableColumnNamesToExclude = new String [] {
				EdgeTableColumnNames.Vertex1Name,
				EdgeTableColumnNames.Vertex2Name,
				EdgeTableColumnNames.Color,
				EdgeTableColumnNames.Width,
				EdgeTableColumnNames.Alpha,
				EdgeTableColumnNames.Visibility,
				CommonTableColumnNames.ID,
				};

			String [] asEdgeTableColumnNameBasesToExclude = new String [] {};

			String [] asEdgeTableSourceColumnNames =
				ExcelUtil.GetTableColumnNames(oEdgeTable,
					asEdgeTableColumnNamesToExclude,
					asEdgeTableColumnNameBasesToExclude
					);

			// Populate the edge table column ComboBoxes with the source column
			// names.

			foreach (ComboBox oComboBox in m_aoEdgeSourceColumnNameComboBoxes)
			{
				oComboBox.Items.AddRange(asEdgeTableSourceColumnNames);
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
	//	Method: InitializeVertexComboBoxes()
	//
	/// <summary>
	///	Initializes the ComboBoxes used to specify the data sources for the
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
			// Get the names of the vertex table columns that can be used as a
			// data source.

			String [] asVertexTableColumnNamesToExclude = new String [] {
				VertexTableColumnNames.VertexName,
				VertexTableColumnNames.Color,
				VertexTableColumnNames.Shape,
				VertexTableColumnNames.Radius,
				VertexTableColumnNames.ImageKey,
				VertexTableColumnNames.PrimaryLabel,
				VertexTableColumnNames.SecondaryLabel,
				VertexTableColumnNames.Alpha,
				VertexTableColumnNames.ToolTip,
				VertexTableColumnNames.VertexDrawerPrecedence,
				VertexTableColumnNames.Visibility,
				VertexTableColumnNames.Locked,
				VertexTableColumnNames.X,
				VertexTableColumnNames.Y,
				CommonTableColumnNames.ID,
				};

			String [] asVertexTableColumnNameBasesToExclude = new String [] {
				VertexTableColumnNames.CustomMenuItemTextBase,
				VertexTableColumnNames.CustomMenuItemActionBase,
				};

			String [] asVertexTableSourceColumnNames =
				ExcelUtil.GetTableColumnNames(oVertexTable,
					asVertexTableColumnNamesToExclude,
					asVertexTableColumnNameBasesToExclude
					);

			// Populate the vertex table column ComboBoxes with the source
			// column names.

			foreach (ComboBox oComboBox in m_aoVertexSourceColumnNameComboBoxes)
			{
				oComboBox.Items.AddRange(asVertexTableSourceColumnNames);
			}

			// Add a few special items.

			String sVertexColumnName = VertexTableColumnNames.VertexName;

			cbxVertexPrimaryLabelSourceColumnName.Items.Add(sVertexColumnName);

			cbxVertexSecondaryLabelSourceColumnName.Items.Add(
				sVertexColumnName);

			cbxVertexToolTipSourceColumnName.Items.Add(sVertexColumnName);
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

		cbxVertexSecondaryLabelSourceColumnName.Tag =
			VertexTableColumnNames.SecondaryLabel;

		cbxVertexToolTipSourceColumnName.Tag = VertexTableColumnNames.ToolTip;

		cbxVertexVisibilitySourceColumnName.Tag =
			VertexTableColumnNames.Visibility;

		cbxVertexXSourceColumnName.Tag = VertexTableColumnNames.X;
		cbxVertexYSourceColumnName.Tag = VertexTableColumnNames.Y;
	}

	//*************************************************************************
	//	Method: EnableControls()
	//
	/// <summary>
	///	Enables or disables the dialog's controls.
	/// </summary>
	//*************************************************************************

	protected void
	EnableControls()
	{
		AssertValid();

        tlpTableLayoutPanel.Enabled = cbxUseAutoFill.Checked;
	}

	//*************************************************************************
	//	Method: DoDataExchange()
	//
	/// <summary>
	///	Transfers data between the dialog's fields and its controls.
	/// </summary>
	///
	/// <param name="bFromControls">
	///	true to transfer data from the dialog's controls to its fields, false
	///	for the other direction.
	/// </param>
	///
	/// <returns>
	///	true if the transfer was successful.
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

			m_oAutoFillUserSettings.UseAutoFill = cbxUseAutoFill.Checked;

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

			m_oAutoFillUserSettings.VertexSecondaryLabelSourceColumnName =
				GetSourceColumnNameFromComboBox(
					cbxVertexSecondaryLabelSourceColumnName);

			m_oAutoFillUserSettings.VertexToolTipSourceColumnName =
				GetSourceColumnNameFromComboBox(
					cbxVertexToolTipSourceColumnName);

			m_oAutoFillUserSettings.VertexVisibilitySourceColumnName =
				GetSourceColumnNameFromComboBox(
					cbxVertexVisibilitySourceColumnName);

			m_oAutoFillUserSettings.VertexXSourceColumnName =
				sVertexXSourceColumnName;

			m_oAutoFillUserSettings.VertexYSourceColumnName =
				sVertexYSourceColumnName;
		}
		else
		{
			cbxUseAutoFill.Checked = m_oAutoFillUserSettings.UseAutoFill;

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

			cbxVertexSecondaryLabelSourceColumnName.Text =
				m_oAutoFillUserSettings.VertexSecondaryLabelSourceColumnName;

			cbxVertexToolTipSourceColumnName.Text =
				m_oAutoFillUserSettings.VertexToolTipSourceColumnName;

			cbxVertexVisibilitySourceColumnName.Text =
				m_oAutoFillUserSettings.VertexVisibilitySourceColumnName;

			cbxVertexXSourceColumnName.Text =
				m_oAutoFillUserSettings.VertexXSourceColumnName;

			cbxVertexYSourceColumnName.Text =
				m_oAutoFillUserSettings.VertexYSourceColumnName;
		}

		return (true);
	}

	//*************************************************************************
	//	Method: ValidateSourceColumnNameComboBoxes()
	//
	/// <summary>
	///	Validates the text in an array of ComboBoxes that may contain source
	/// column names.
	/// </summary>
	///
	/// <param name="aoSourceColumnNameComboBoxes">
	///	Array of ComboBoxes that may contain source column names.
	/// </param>
	///
	/// <returns>
	///	true if the ComboBoxes all contain valid text.
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
	//	Method: GetSourceColumnNameFromComboBox()
	//
	/// <summary>
	///	Gets a source column name from a ComboBox.
	/// </summary>
	///
	/// <param name="oComboBox">
	/// ComboBox to get a source column name from.
	/// </param>
	///
	/// <returns>
	///	The text in <paramref name="oComboBox" />, or String.Empty if the
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
	//	Method: cbxUseAutoFill_CheckedChanged()
	//
	/// <summary>
	///	Handles the CheckedChanged event on the cbxUseAutoFill ComboBox.
	/// </summary>
	///
	/// <param name="sender">
	///	Standard event argument.
	/// </param>
	///
	/// <param name="e">
	/// Standard event argument.
	/// </param>
	//*************************************************************************

    private void
	cbxUseAutoFill_CheckedChanged
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

		EnableControls();
    }

	//*************************************************************************
	//	Method: btnEdgeColorDetails_Click()
	//
	/// <summary>
	///	Handles the Click event on the btnEdgeColorDetails button.
	/// </summary>
	///
	/// <param name="sender">
	///	Standard event argument.
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
	//	Method: btnEdgeWidthDetails_Click()
	//
	/// <summary>
	///	Handles the Click event on the btnEdgeWidthDetails button.
	/// </summary>
	///
	/// <param name="sender">
	///	Standard event argument.
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
                "Edge Width Options",
                "edge width",
                EdgeWidthConverter.MinimumWidthWorkbook,
                EdgeWidthConverter.MaximumWidthWorkbook
                );

		oNumericRangeColumnAutoFillUserSettingsDialog.ShowDialog();
    }

	//*************************************************************************
	//	Method: btnEdgeAlphaDetails_Click()
	//
	/// <summary>
	///	Handles the Click event on the btnEdgeAlphaDetails button.
	/// </summary>
	///
	/// <param name="sender">
	///	Standard event argument.
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
                "Edge Opacity Options",
                "edge opacity",
                AlphaConverter.MinimumAlphaWorkbook,
                AlphaConverter.MaximumAlphaWorkbook
                );

		oNumericRangeColumnAutoFillUserSettingsDialog.ShowDialog();
    }

	//*************************************************************************
	//	Method: btnEdgeVisibilityDetails_Click()
	//
	/// <summary>
	///	Handles the Click event on the btnEdgeVisibilityDetails button.
	/// </summary>
	///
	/// <param name="sender">
	///	Standard event argument.
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
	//	Method: btnVertexColorDetails_Click()
	//
	/// <summary>
	///	Handles the Click event on the btnVertexColorDetails button.
	/// </summary>
	///
	/// <param name="sender">
	///	Standard event argument.
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
	//	Method: btnVertexShapeDetails_Click()
	//
	/// <summary>
	///	Handles the Click event on the btnVertexShapeDetails button.
	/// </summary>
	///
	/// <param name="sender">
	///	Standard event argument.
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
	//	Method: btnVertexRadiusDetails_Click()
	//
	/// <summary>
	///	Handles the Click event on the btnVertexRadiusDetails button.
	/// </summary>
	///
	/// <param name="sender">
	///	Standard event argument.
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
                "Vertex Radius Options",
                "vertex radius",
                VertexRadiusConverter.MinimumRadiusWorkbook,
                VertexRadiusConverter.MaximumRadiusWorkbook
                );

		oNumericRangeColumnAutoFillUserSettingsDialog.ShowDialog();
    }

	//*************************************************************************
	//	Method: btnVertexAlphaDetails_Click()
	//
	/// <summary>
	///	Handles the Click event on the btnVertexAlphaDetails button.
	/// </summary>
	///
	/// <param name="sender">
	///	Standard event argument.
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
                "Vertex Opacity Options",
                "vertex opacity",
                AlphaConverter.MinimumAlphaWorkbook,
                AlphaConverter.MaximumAlphaWorkbook
                );

		oNumericRangeColumnAutoFillUserSettingsDialog.ShowDialog();
    }

	//*************************************************************************
	//	Method: btnVertexVisibilityDetails_Click()
	//
	/// <summary>
	///	Handles the Click event on the btnVertexVisibilityDetails button.
	/// </summary>
	///
	/// <param name="sender">
	///	Standard event argument.
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
	//	Method: btnVertexXDetails_Click()
	//
	/// <summary>
	///	Handles the Click event on the btnVertexXDetails button.
	/// </summary>
	///
	/// <param name="sender">
	///	Standard event argument.
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
                "Vertex X Options",
                "vertex x-coordinate",
                VertexLocationConverter.MinimumXYWorkbook,
                VertexLocationConverter.MaximumXYWorkbook
                );

		oNumericRangeColumnAutoFillUserSettingsDialog.ShowDialog();
    }

	//*************************************************************************
	//	Method: btnVertexYDetails_Click()
	//
	/// <summary>
	///	Handles the Click event on the btnVertexYDetails button.
	/// </summary>
	///
	/// <param name="sender">
	///	Standard event argument.
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
                "Vertex Y Options",
                "vertex y-coordinate",
                VertexLocationConverter.MinimumXYWorkbook,
                VertexLocationConverter.MaximumXYWorkbook
                );

		oNumericRangeColumnAutoFillUserSettingsDialog.ShowDialog();
    }

	//*************************************************************************
	//	Method: lnkHowAutoFillWorks_LinkClicked()
	//
	/// <summary>
	///	Handles the Click event on the lnkHowAutoFillWorks LinkButton.
	/// </summary>
	///
	/// <param name="sender">
	///	Standard event argument.
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

		const String Message =
"Use the AutoFill Columns feature to automatically calculate and fill in the graph's visual attribute columns, such as edge width and vertex shape, using values from other columns."
+ "\r\n\r\n"
+ "There are three ways to control the graph's visual attributes.  Take edge width, for example.  If you leave the Width column on the Edges worksheet empty, the edges are drawn in the graph using the default edge width you enter in the Options dialog box, accessible from the graph pane."
+ "\r\n\r\n"
+ "You can override the default edge width for one or more edges by entering numbers in the Width column.  Any edges with empty Width cells use the default edge width."
+ "\r\n\r\n"
+ "The third option is to specify a \"source column\" containing numbers you want .NetMap to use to automatically calculate edge widths.  Let's say you have a numerical Tie Strength column on the Edges worksheet.  If you open the AutoFill Columns dialog box, select \"Tie Strength\" from the Edge Width drop-down, and click Show Graph in the graph pane, .NetMap will calculate a width for each edge based on the edge's Tie Strength, fill in the Width column, and use the calculated widths in the graph.  (By default, .NetMap calculates the widths using a linear mapping between the Tie Strength numbers and the range of available widths, so that the edge with the smallest Tie Strength gets the smallest width and the edge with the largest Tie Strength gets the largest width.  You can modify this behavior by clicking the Edge Width Options button in the AutoFill Columns dialog box.)"
+ "\r\n\r\n"
+ "Here are some things to know about AutoFill:"
+ "\r\n\r\n"
+ "* You configure AutoFill with the AutoFill Columns dialog box, but the columns you specify don't get autofilled until you click Read Workbook in the graph pane."
+ "\r\n\r\n"
+ "* If a column's source column doesn't exist when Read Workbook is clicked, the column doesn't get autofilled.  (If Tie Strength doesn't exist in the above example, then the Width column on the Edges worksheet won't get autofilled.)"
+ "\r\n\r\n"
+ "* Your AutoFill Columns settings apply to all .NetMap workbooks."
+ "\r\n\r\n"
+ "* Autofilling multiple columns in a large workbook can slow down Read Workbook, especially if one of your worksheets is filtered."
			;

		this.ShowInformation(Message);
    }

	//*************************************************************************
	//	Method: btnOK_Click()
	//
	/// <summary>
	///	Handles the Click event on the btnOK button.
	/// </summary>
	///
	/// <param name="sender">
	///	Standard event argument.
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

		Debug.Assert(m_oAutoFillUserSettingsDialogUserSettings != null);
		Debug.Assert(m_oAutoFillUserSettings != null);
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


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// User settings for this dialog.

	protected AutoFillUserSettingsDialogUserSettings
		m_oAutoFillUserSettingsDialogUserSettings;

	/// Object being edited.  This is a copy of the object passed to the
	/// constructor.

	protected AutoFillUserSettings m_oAutoFillUserSettings;

	/// Array of ComboBoxes for the edge source column names.

	protected ComboBox [] m_aoEdgeSourceColumnNameComboBoxes;

	/// Array of ComboBoxes for the vertex source column names.

	protected ComboBox [] m_aoVertexSourceColumnNameComboBoxes;
}


//*****************************************************************************
//  Class: AutoFillUserSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="AutoFillUserSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("AutoFillUserSettingsDialog") ]

public class AutoFillUserSettingsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: AutoFillUserSettingsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="AutoFillUserSettingsDialogUserSettings" /> class.
    /// </summary>
	///
	/// <param name="oForm">
	/// The form to save settings for.
	/// </param>
    //*************************************************************************

    public AutoFillUserSettingsDialogUserSettings
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
