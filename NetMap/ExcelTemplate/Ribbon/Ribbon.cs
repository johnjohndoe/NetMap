

//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Ribbon;
using Microsoft.NetMap.Core;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: Ribbon
//
/// <summary>
/// Implements the template's ribbon customizations.
/// </summary>
//*****************************************************************************

public partial class Ribbon : OfficeRibbon
{
    //*************************************************************************
    //  Constructor: Ribbon()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="Ribbon" /> class.
    /// </summary>
    //*************************************************************************

	public Ribbon()
	{
		InitializeComponent();

		AssertValid();
	}

	//*************************************************************************
	//	Property: GraphDirectedness
	//
	/// <summary>
	/// Gets or sets the graph directedness of the active workbook.
	/// </summary>
	///
	/// <value>
	/// A GraphDirectedness value.
	/// </value>
	//*************************************************************************

	public GraphDirectedness
	GraphDirectedness
	{
		get
		{
			AssertValid();

			// The user selects the directedness with the rddGraphDirectedness
			// RibbonDropDown.  The GraphDirectedness for each item is stored
			// in the item's Tag.

			return ( (GraphDirectedness)rddGraphDirectedness.SelectedItem.Tag );
		}

		set
		{
			foreach (RibbonDropDownItem oItem in rddGraphDirectedness.Items)
			{
				if ( (GraphDirectedness)oItem.Tag == value )
				{
					rddGraphDirectedness.SelectedItem = oItem;

					break;
				}
			}

			AssertValid();
		}
	}

    //*************************************************************************
    //  Method: Ribbon_Load()
    //
    /// <summary>
	/// Handles the Load event on the ribbon.
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
	Ribbon_Load
	(
		object sender,
		RibbonUIEventArgs e
	)
	{
		AssertValid();

		PerWorkbookSettings oPerWorkbookSettings = new PerWorkbookSettings(
			Globals.ThisWorkbook.InnerObject);

		// The graph directedness RibbonDropDown should be enabled only if the
		// template the workbook is based on supports changing the
		// directedness.

		Int32 iTemplateVersion = oPerWorkbookSettings.TemplateVersion;

		rddGraphDirectedness.Enabled = (iTemplateVersion >= 51);

		// The ability to create clusters depends on the template version.

		btnCreateClusters.Enabled = (iTemplateVersion >= 54);
	}

    //*************************************************************************
    //  Method: btnImportEdges_Click()
    //
    /// <summary>
	/// Handles the Click event on the btnImportEdges button.
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
	btnImportEdges_Click
	(
		object sender,
		RibbonControlEventArgs e
	)
    {
		AssertValid();

        Globals.ThisWorkbook.ImportEdges();
    }

    //*************************************************************************
    //  Method: rddGraphDirectedness_SelectionChanged()
    //
    /// <summary>
	/// Handles the SelectionChanged event on the rddGraphDirectedness
	/// RibbonDropDown.
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
	rddGraphDirectedness_SelectionChanged
	(
		object sender,
		RibbonControlEventArgs e
	)
    {
		AssertValid();

		GraphDirectedness eGraphDirectedness = this.GraphDirectedness;

		if ( !Globals.ThisWorkbook.ExcelApplicationIsReady(true) )
		{
			// Cancel the change.

			this.GraphDirectedness =
				(eGraphDirectedness == GraphDirectedness.Directed) ?
				GraphDirectedness.Undirected : GraphDirectedness.Directed;
		}
		else
		{
			// Notify the workbook.

			Globals.ThisWorkbook.GraphDirectedness = eGraphDirectedness;
		}
    }

    //*************************************************************************
    //  Method: btnToggleGraphVisibility_Click()
    //
    /// <summary>
	/// Handles the Click event on the btnToggleGraphVisibility button.
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
	btnToggleGraphVisibility_Click
	(
		object sender,
		RibbonControlEventArgs e
	)
	{
		AssertValid();

		// Note: The button should actually be a checkbox.  However, no event
		// is fired when the user manually closes the ActionsPane, so it's not
		// possible to update a checkbox when he does so.  Using a button that
		// toggles the visibility of the action pane is a workaround.

        Globals.ThisWorkbook.ToggleGraphVisibility();
	}

    //*************************************************************************
    //  Method: btnMergeDuplicateEdges_Click()
    //
    /// <summary>
	/// Handles the Click event on the btnMergeDuplicateEdges button.
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
	btnMergeDuplicateEdges_Click
	(
		object sender,
		RibbonControlEventArgs e
	)
    {
		AssertValid();

        Globals.ThisWorkbook.MergeDuplicateEdges();
    }

    //*************************************************************************
    //  Method: btnPopulateVertexWorksheet_Click()
    //
    /// <summary>
	/// Handles the Click event on the btnPopulateVertexWorksheet button.
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
	btnPopulateVertexWorksheet_Click
	(
		object sender,
		RibbonControlEventArgs e
	)
    {
		AssertValid();

        Globals.ThisWorkbook.PopulateVertexWorksheet(true, true);
    }

    //*************************************************************************
    //  Method: btnCustomizeVertexMenu_Click()
    //
    /// <summary>
	/// Handles the Click event on the btnCustomizeVertexMenu button.
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
	btnCustomizeVertexMenu_Click
	(
		object sender,
		RibbonControlEventArgs e
	)
    {
		AssertValid();

        Globals.ThisWorkbook.CustomizeVertexMenu(true);
    }

    //*************************************************************************
    //  Method: btnEditGraphMetricUserSettings_Click()
    //
    /// <summary>
	/// Handles the Click event on the btnEditGraphMetricUserSettings button.
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
	btnEditGraphMetricUserSettings_Click
	(
		object sender,
		RibbonControlEventArgs e
	)
    {
		AssertValid();

        Globals.ThisWorkbook.EditGraphMetricUserSettings();
    }

    //*************************************************************************
    //  Method: btnCalculateGraphMetrics_Click()
    //
    /// <summary>
	/// Handles the Click event on the btnCalculateGraphMetrics button.
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
	spltCalculateGraphMetrics_Click
	(
		object sender,
		RibbonControlEventArgs e
	)
    {
		AssertValid();

        Globals.ThisWorkbook.CalculateGraphMetrics();
    }

    //*************************************************************************
    //  Method: btnCreateClusters_Click()
    //
    /// <summary>
	/// Handles the Click event on the btnCreateClusters button.
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
	btnCreateClusters_Click
	(
		object sender,
		RibbonControlEventArgs e
	)
    {
		AssertValid();

        Globals.ThisWorkbook.CreateClusters();
    }

    //*************************************************************************
    //  Method: btnCreateSubgraphImages_Click()
    //
    /// <summary>
	/// Handles the Click event on the btnCreateSubgraphImages button.
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
	btnCreateSubgraphImages_Click
	(
		object sender,
		RibbonControlEventArgs e
	)
    {
		AssertValid();

        Globals.ThisWorkbook.CreateSubgraphImages();
    }

    //*************************************************************************
    //  Method: btnAnalyzeEmailNetwork_Click()
    //
    /// <summary>
	/// Handles the Click event on the btnAnalyzeEmailNetwork button.
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
	btnAnalyzeEmailNetwork_Click
	(
		object sender,
		RibbonControlEventArgs e
	)
    {
		AssertValid();

        Globals.ThisWorkbook.AnalyzeEmailNetwork();
    }

    //*************************************************************************
    //  Method: EditAutoFillUserSettings_Click()
    //
    /// <summary>
	/// Handles the Click event on the EditAutoFillUserSettings button.
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
	EditAutoFillUserSettings_Click
	(
		object sender,
		RibbonControlEventArgs e
	)
    {
		AssertValid();

        Globals.ThisWorkbook.EditAutoFillUserSettings();
    }

    //*************************************************************************
    //  Method: btnAbout_Click()
    //
    /// <summary>
	/// Handles the Click event on the btnAbout button.
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
	btnAbout_Click
	(
		object sender,
		RibbonControlEventArgs e
	)
    {
		AssertValid();

		AboutDialog oAboutDialog = new AboutDialog();

		oAboutDialog.ShowDialog();
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
		// (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
