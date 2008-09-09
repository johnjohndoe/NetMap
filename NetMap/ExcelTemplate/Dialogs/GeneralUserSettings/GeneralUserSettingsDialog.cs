
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.NetMap.Visualization;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//	Class: GeneralUserSettingsDialog
//
/// <summary>
///	Edits a <see cref="GeneralUserSettings" /> object.
/// </summary>
///
/// <remarks>
///	Pass a <see cref="GeneralUserSettings" /> object to the constructor.  If
/// the user edits the object, <see cref="Form.ShowDialog()" /> returns
/// DialogResult.OK.  Otherwise, the object is not modified and <see
/// cref="Form.ShowDialog()" /> returns DialogResult.Cancel.
/// </remarks>
//*****************************************************************************

public partial class GeneralUserSettingsDialog : ExcelTemplateForm
{
	//*************************************************************************
	//	Constructor: GeneralUserSettingsDialog()
	//
	/// <summary>
	///	Initializes a new instance of the <see
	/// cref="GeneralUserSettingsDialog" /> class.
	/// </summary>
	///
	/// <param name="generalUserSettings">
	/// The object being edited.
	/// </param>
	//*************************************************************************

	public GeneralUserSettingsDialog
	(
		GeneralUserSettings generalUserSettings
	)
	{
		Debug.Assert(generalUserSettings != null);
		generalUserSettings.AssertValid();

		m_oGeneralUserSettings = generalUserSettings;
		m_oFont = m_oGeneralUserSettings.Font;

		// Instantiate an object that saves and retrieves the position of this
		// dialog.  Note that the object automatically saves the settings when
		// the form closes.

		m_oGeneralUserSettingsDialogUserSettings =
			new GeneralUserSettingsDialogUserSettings(this);

		InitializeComponent();

        Object[] oAllGraphAndWorkbookValues =
            ( new ColorConverter2() ).GetAllGraphAndWorkbookValues(false);

        cbxBackColor.PopulateWithObjectsAndText(oAllGraphAndWorkbookValues);
        cbxVertexColor.PopulateWithObjectsAndText(oAllGraphAndWorkbookValues);

        cbxPrimaryLabelFillColor.PopulateWithObjectsAndText(
			oAllGraphAndWorkbookValues);

        cbxSelectedVertexColor.PopulateWithObjectsAndText(
			oAllGraphAndWorkbookValues);

        cbxEdgeColor.PopulateWithObjectsAndText(oAllGraphAndWorkbookValues);

        cbxSelectedEdgeColor.PopulateWithObjectsAndText(
			oAllGraphAndWorkbookValues);

		nudMargin.Minimum = 0;
		nudMargin.Maximum = 100;

		nudEdgeWidth.Minimum =
			(Decimal)EdgeWidthConverter.MinimumWidthWorkbook;

		nudEdgeWidth.Maximum =
			(Decimal)EdgeWidthConverter.MaximumWidthWorkbook;

		nudSelectedEdgeWidth.Minimum =
			(Decimal)EdgeWidthConverter.MinimumWidthWorkbook;

		nudSelectedEdgeWidth.Maximum =
			(Decimal)EdgeWidthConverter.MaximumWidthWorkbook;

		nudRelativeArrowSize.Minimum = EdgeDrawer.MinimumRelativeArrowSize;
		nudRelativeArrowSize.Maximum = EdgeDrawer.MaximumRelativeArrowSize;

		nudVertexRadius.Minimum =
			(Decimal)VertexRadiusConverter.MinimumRadiusWorkbook;

		nudVertexRadius.Maximum = 
			(Decimal)VertexRadiusConverter.MaximumRadiusWorkbook;

		( new VertexShapeConverter() ).PopulateComboBox(cbxVertexShape, false);

		nudVertexAlpha.Minimum = (Decimal)AlphaConverter.MinimumAlphaWorkbook;
		nudVertexAlpha.Maximum = (Decimal)AlphaConverter.MaximumAlphaWorkbook;

		nudEdgeAlpha.Minimum = (Decimal)AlphaConverter.MinimumAlphaWorkbook;
		nudEdgeAlpha.Maximum = (Decimal)AlphaConverter.MaximumAlphaWorkbook;

        lblMaximumAlphaMessage.Text = lblMaximumAlphaMessage2.Text =
            AlphaConverter.MaximumAlphaMessage;

		DoDataExchange(false);

		AssertValid();
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
			Int32 iMargin;

			Single fEdgeWidth, fSelectedEdgeWidth, fRelativeArrowSize,
				fVertexRadius, fVertexAlpha, fEdgeAlpha;

			if (
				!ValidateNumericUpDown(nudVertexRadius,
					"a vertex radius", out fVertexRadius)
				||
				!ValidateNumericUpDown(nudVertexAlpha, "a vertex opacity",
					out fVertexAlpha)
				||
				!ValidateNumericUpDown(nudEdgeWidth,
					"a width for unselected edges", out fEdgeWidth)
				||
				!ValidateNumericUpDown(nudEdgeAlpha, "an edge opacity",
					out fEdgeAlpha)
				||
				!ValidateNumericUpDown(nudRelativeArrowSize,
					"an arrow size", out fRelativeArrowSize)
				||
				!ValidateNumericUpDown(nudSelectedEdgeWidth,
					"a width for selected edges", out fSelectedEdgeWidth)
				||
				!ValidateNumericUpDown(nudMargin,
					"a graph margin", out iMargin)
				)
			{
				return (false);
			}

			if (fSelectedEdgeWidth < fEdgeWidth)
			{
				return ( OnInvalidNumericUpDown(nudSelectedEdgeWidth,
					"Selected edges must be at least as wide as unselected"
					+ " edges."
					) );
			}

			m_oGeneralUserSettings.BackColor =
				(KnownColor)cbxBackColor.SelectedValue;

			m_oGeneralUserSettings.Margin = iMargin;

			m_oGeneralUserSettings.EdgeWidth = fEdgeWidth;
			m_oGeneralUserSettings.SelectedEdgeWidth = fSelectedEdgeWidth;

			m_oGeneralUserSettings.RelativeArrowSize = fRelativeArrowSize;

			m_oGeneralUserSettings.EdgeColor =
				(KnownColor)cbxEdgeColor.SelectedValue;

			m_oGeneralUserSettings.EdgeAlpha = (Single)nudEdgeAlpha.Value;

			m_oGeneralUserSettings.SelectedEdgeColor =
				(KnownColor)cbxSelectedEdgeColor.SelectedValue;

			m_oGeneralUserSettings.VertexShape =
				(VertexDrawer.VertexShape)cbxVertexShape.SelectedValue;

			m_oGeneralUserSettings.VertexRadius = fVertexRadius;

			m_oGeneralUserSettings.VertexColor =
				(KnownColor)cbxVertexColor.SelectedValue;

			m_oGeneralUserSettings.VertexAlpha = (Single)nudVertexAlpha.Value;

			m_oGeneralUserSettings.PrimaryLabelFillColor =
				(KnownColor)cbxPrimaryLabelFillColor.SelectedValue;

			m_oGeneralUserSettings.SelectedVertexColor =
				(KnownColor)cbxSelectedVertexColor.SelectedValue;

			m_oGeneralUserSettings.AutoSelect = chkAutoSelect.Checked;

			m_oGeneralUserSettings.Font = m_oFont;
		}
		else
		{
			cbxBackColor.SelectedValue = m_oGeneralUserSettings.BackColor;

			nudMargin.Value = m_oGeneralUserSettings.Margin;

			nudEdgeWidth.Value = (Decimal)m_oGeneralUserSettings.EdgeWidth;

			nudSelectedEdgeWidth.Value =
				(Decimal)m_oGeneralUserSettings.SelectedEdgeWidth;

			nudRelativeArrowSize.Value =
				(Decimal)m_oGeneralUserSettings.RelativeArrowSize;

			cbxEdgeColor.SelectedValue = m_oGeneralUserSettings.EdgeColor;

			nudEdgeAlpha.Value = (Decimal)m_oGeneralUserSettings.EdgeAlpha;

			cbxSelectedEdgeColor.SelectedValue =
				m_oGeneralUserSettings.SelectedEdgeColor;

			cbxVertexShape.SelectedValue = m_oGeneralUserSettings.VertexShape;

			nudVertexRadius.Value =
				(Decimal)m_oGeneralUserSettings.VertexRadius;

			cbxVertexColor.SelectedValue = m_oGeneralUserSettings.VertexColor;

			nudVertexAlpha.Value = (Decimal)m_oGeneralUserSettings.VertexAlpha;

			cbxPrimaryLabelFillColor.SelectedValue =
				m_oGeneralUserSettings.PrimaryLabelFillColor;

			cbxSelectedVertexColor.SelectedValue =
				m_oGeneralUserSettings.SelectedVertexColor;

			chkAutoSelect.Checked = m_oGeneralUserSettings.AutoSelect;

			m_oFont = m_oGeneralUserSettings.Font;
		}

		return (true);
	}

	//*************************************************************************
	//	Method: btnFont_Click()
	//
	/// <summary>
	///	Handles the Click event on the btnFont button.
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
	btnFont_Click
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

		FontDialog oFontDialog = new FontDialog();

		oFontDialog.Font = m_oFont;
		oFontDialog.FontMustExist = true;

		// The FontConverter class implicity used by ApplicationsSettingsBase
		// to persist GeneralUserSettings.Font does not persist the script, so
		// don't allow the user to change the script from the default.

		oFontDialog.AllowScriptChange = false;

		if (oFontDialog.ShowDialog() == DialogResult.OK)
		{
			m_oFont = oFontDialog.Font;
		}
    }

	//*************************************************************************
	//	Method: btnResetAll_Click()
	//
	/// <summary>
	///	Handles the Click event on the btnResetAll button.
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
	btnResetAll_Click
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

        m_oGeneralUserSettings.Reset();

		DoDataExchange(false);
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
	//	Method: AssertValid()
	//
	/// <summary>
	///	Asserts if the object is in an invalid state.  Debug-only.
	/// </summary>
	//*************************************************************************

	// [Conditional("DEBUG")] 

	public  override void
	AssertValid()
	{
        base.AssertValid();

		Debug.Assert(m_oGeneralUserSettings != null);
		Debug.Assert(m_oFont != null);
		Debug.Assert(m_oGeneralUserSettingsDialogUserSettings != null);
	}


	//*************************************************************************
	//	Protected fields
	//*************************************************************************

	/// Object whose properties are being edited.

	protected GeneralUserSettings m_oGeneralUserSettings;

	/// Font property of m_oGeneralUserSettings.  This gets edited by a
	/// FontDialog.

	protected Font m_oFont;

	/// User settings for this dialog.

	protected GeneralUserSettingsDialogUserSettings
		m_oGeneralUserSettingsDialogUserSettings;
}


//*****************************************************************************
//  Class: GeneralUserSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="GeneralUserSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("GeneralUserSettingsDialog2") ]

public class GeneralUserSettingsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: GeneralUserSettingsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="GeneralUserSettingsDialogUserSettings" /> class.
    /// </summary>
	///
	/// <param name="oForm">
	/// The form to save settings for.
	/// </param>
    //*************************************************************************

    public GeneralUserSettingsDialogUserSettings
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
