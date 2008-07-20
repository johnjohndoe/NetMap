
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.NetMap.Visualization;

namespace Microsoft.NetMap.DesktopApplication
{
//*****************************************************************************
//	Class: GraphSettingsDialog
//
/// <summary>
///	Edits a <see cref="GraphSettings" /> object.
/// </summary>
///
/// <remarks>
///	Pass a <see cref="GraphSettings" /> object to the constructor.  If the user
/// edits the object, <see cref="Form.ShowDialog()" /> returns DialogResult.OK.
/// Otherwise, the object is not modified and <see cref="Form.ShowDialog()" />
/// returns DialogResult.Cancel.
/// </remarks>
//*****************************************************************************

public partial class GraphSettingsDialog : FormPlus
{
	//*************************************************************************
	//	Constructor: GraphSettingsDialog()
	//
	/// <summary>
	///	Initializes a new instance of the <see cref="GraphSettingsDialog" />
	/// class.
	/// </summary>
	///
	/// <param name="graphSettings">
	/// Object being edited.
	/// </param>
	///
	/// <param name="allowApplyToAllGraphs">
	/// If true, the user can choose between "apply to all graphs" and "apply
	/// to active graph only."  If false, "apply to all graphs" is the only
	/// option.  The user's selection is returned in the <see
	/// cref="ApplyToAllGraphs" /> property.
	/// </param>
	//*************************************************************************

	public GraphSettingsDialog
	(
		GraphSettings graphSettings,
		Boolean allowApplyToAllGraphs
	)
	{
		Debug.Assert(graphSettings != null);
		graphSettings.AssertValid();

		m_oGraphSettings = graphSettings;

		// Instantiate an object that saves and retrieves the user settings for
		// this dialog.  Note that the object automatically saves the settings
		// when the form closes.

		m_oGraphSettingsDialogUserSettings =
			new GraphSettingsDialogUserSettings(this);

		InitializeComponent();

		nudMargin.Minimum = 0;
		nudMargin.Maximum = 100;

		nudEdgeWidth.Minimum = EdgeDrawer.MinimumWidth;
		nudEdgeWidth.Maximum = EdgeDrawer.MaximumWidth;

		nudSelectedEdgeWidth.Minimum = EdgeDrawer.MinimumWidth;
		nudSelectedEdgeWidth.Maximum = EdgeDrawer.MaximumWidth;

		nudRelativeArrowSize.Minimum = EdgeDrawer.MinimumRelativeArrowSize;
		nudRelativeArrowSize.Maximum = EdgeDrawer.MaximumRelativeArrowSize;

		nudVertexRadius.Minimum = (Decimal)VertexDrawer.MinimumRadius;
		nudVertexRadius.Maximum = (Decimal)VertexDrawer.MaximumRadius;

		cbxVertexShape.PopulateWithEnumValues(
			typeof(VertexDrawer.VertexShape), true);

		DoDataExchange(false);

		if (!allowApplyToAllGraphs)
		{
			grpApplyTo.Enabled = false;
		}

		AssertValid();
	}

	//*************************************************************************
	//  Property: ApplyToAllGraphs
	//
	/// <summary>
	/// Gets a flag indicating whether the edits should apply to all graphs.
	/// </summary>
	///
	/// <value>
	/// true if the user wants to apply her edits to all graphs, false to apply
	/// them to the active graph only.
	/// </value>
	//*************************************************************************

	public Boolean
	ApplyToAllGraphs
	{
		get
		{
			AssertValid();

			return (grpApplyTo.Enabled && radAllGraphs.Checked);
		}
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
			Int32 iMargin, iEdgeWidth, iSelectedEdgeWidth;
			Single fRelativeArrowSize, fVertexRadius;

			if (
				!ValidateNumericUpDown(nudVertexRadius,
					"a vertex radius", out fVertexRadius)
				||
				!ValidateNumericUpDown(nudEdgeWidth,
					"a width for unselected edges", out iEdgeWidth)
				||
				!ValidateNumericUpDown(nudRelativeArrowSize,
					"an arrow size", out fRelativeArrowSize)
				||
				!ValidateNumericUpDown(nudSelectedEdgeWidth,
					"a width for selected edges", out iSelectedEdgeWidth)
				||
				!ValidateNumericUpDown(nudMargin,
					"a graph margin", out iMargin)
				)
			{
				return (false);
			}

			if (iSelectedEdgeWidth < iEdgeWidth)
			{
				return ( OnInvalidNumericUpDown(nudSelectedEdgeWidth,
					"Selected edges must be at least as wide as unselected"
					+ " edges."
					) );
			}

			m_oGraphSettings.BackColor = usrBackColor.Color;

			m_oGraphSettings.Margin = iMargin;

			m_oGraphSettings.EdgeWidth = iEdgeWidth;
			m_oGraphSettings.SelectedEdgeWidth = iSelectedEdgeWidth;

			m_oGraphSettings.RelativeArrowSize = fRelativeArrowSize;

			m_oGraphSettings.EdgeColor = Color.FromArgb(
				trbEdgeAlpha.Value, usrEdgeColor.Color
				);

			m_oGraphSettings.SelectedEdgeColor = usrSelectedEdgeColor.Color;

			m_oGraphSettings.VertexShape =
				(VertexDrawer.VertexShape)cbxVertexShape.SelectedValue;

			m_oGraphSettings.VertexRadius = fVertexRadius;

			m_oGraphSettings.VertexColor = Color.FromArgb(
				trbVertexAlpha.Value, usrVertexColor.Color
				);

			m_oGraphSettings.SelectedVertexColor =
				usrSelectedVertexColor.Color;
		}
		else
		{
			usrBackColor.Color = m_oGraphSettings.BackColor;

			nudMargin.Value = m_oGraphSettings.Margin;

			nudEdgeWidth.Value = m_oGraphSettings.EdgeWidth;
			nudSelectedEdgeWidth.Value = m_oGraphSettings.SelectedEdgeWidth;

			nudRelativeArrowSize.Value =
				(Decimal)m_oGraphSettings.RelativeArrowSize;

			Color oEdgeColor = m_oGraphSettings.EdgeColor;

			usrEdgeColor.Color = Color.FromArgb(255, oEdgeColor);

			trbEdgeAlpha.Value = oEdgeColor.A;

			usrSelectedEdgeColor.Color = m_oGraphSettings.SelectedEdgeColor;

			cbxVertexShape.SelectedValue = m_oGraphSettings.VertexShape;

			nudVertexRadius.Value = (Decimal)m_oGraphSettings.VertexRadius;

			Color oVertexColor = m_oGraphSettings.VertexColor;

			usrVertexColor.Color = Color.FromArgb(255, oVertexColor);

			trbVertexAlpha.Value = oVertexColor.A;

			usrSelectedVertexColor.Color =
				m_oGraphSettings.SelectedVertexColor;
		}

		return (true);
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

        m_oGraphSettings.Reset();

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

		Debug.Assert(m_oGraphSettings != null);
		Debug.Assert(m_oGraphSettingsDialogUserSettings != null);
	}


	//*************************************************************************
	//	Protected fields
	//*************************************************************************

	/// Object whose properties are being edited.

	protected GraphSettings m_oGraphSettings;

	/// User settings for this dialog.

	protected GraphSettingsDialogUserSettings
		m_oGraphSettingsDialogUserSettings;
}


//*****************************************************************************
//  Class: GraphSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="GraphSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("GraphSettingsDialog") ]

public class GraphSettingsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: GraphSettingsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="GraphSettingsDialogUserSettings" /> class.
    /// </summary>
	///
	/// <param name="oForm">
	/// The form to save settings for.
	/// </param>
    //*************************************************************************

    public GraphSettingsDialogUserSettings
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
