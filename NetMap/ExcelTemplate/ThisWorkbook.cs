

//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;
using System.Reflection;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NetMap.ExcelTemplate
{
public partial class ThisWorkbook
{
	//*************************************************************************
	//	Property: WorksheetContextMenuManager
	//
	/// <summary>
	/// Gets the object that adds custom menu items to the Excel context menus
	/// that appear when the vertex or edge table is right-clicked.
	/// </summary>
	///
	/// <value>
	/// A WorksheetContextMenuManager object.
	/// </value>
	//*************************************************************************

	public WorksheetContextMenuManager
	WorksheetContextMenuManager
	{
		get
		{
			AssertValid();

            return (m_oWorksheetContextMenuManager);
		}
	}

	//*************************************************************************
	//	Method: ToggleGraphVisibility()
	//
	/// <summary>
	/// Toggles the visibility of the NetMap graph.
	/// </summary>
	//*************************************************************************

	public void
	ToggleGraphVisibility()
	{
		AssertValid();

		if ( !this.ExcelApplicationIsReady(true) )
		{
			return;
		}

		Microsoft.Office.Core.CommandBar oDocumentActionsCommandBar=
			Application.CommandBars["Document Actions"];

		if (oDocumentActionsCommandBar.Visible)
		{
			oDocumentActionsCommandBar.Visible = false;

			return;
		}

		// The NetMap task pane is created in a lazy manner.

		if (!m_bTaskPaneCreated)
		{
			TaskPane oTaskPane = new TaskPane(this);

			this.ActionsPane.Controls.Add(oTaskPane);

            oTaskPane.Dock = DockStyle.Fill;

			oTaskPane.SelectionChangedInGraph +=
				new SelectionChangedEventHandler(
					this.TaskPane_SelectionChangedInGraph);

			oTaskPane.VertexAttributesEditedInGraph +=
				new VertexAttributesEditedEventHandler(
					this.TaskPane_VertexAttributesEditedInGraph);

			oTaskPane.GraphDrawn += new GraphDrawnEventHandler(
				this.TaskPane_GraphDrawn);

			oTaskPane.VertexMoved += new VertexMovedEventHandler(
				this.TaskPane_VertexMoved);

			m_bTaskPaneCreated = true;
		}

		oDocumentActionsCommandBar.Visible = true;
	}

    //*************************************************************************
    //  Method: PopulateVertexWorksheet()
    //
    /// <summary>
	/// Populates the vertex worksheet with the name of each unique vertex in
	/// the edge worksheet.
    /// </summary>
	///
    /// <param name="activateVertexWorksheetWhenDone">
	/// true to activate the vertex worksheet after it is populated.
    /// </param>
	///
    /// <param name="notifyUserOnError">
	/// If true, the user is notified when an error occurs.  If false, an
	/// exception is thrown when an error occurs.
    /// </param>
	///
	/// <returns>
	/// true if successful.
	/// </returns>
    //*************************************************************************

    public Boolean
	PopulateVertexWorksheet
	(
		Boolean activateVertexWorksheetWhenDone,
		Boolean notifyUserOnError
	)
    {
		AssertValid();

		if ( !this.ExcelApplicationIsReady(true) )
		{
			return (false);
		}

		// Create and use the object that fills in the vertex worksheet.

		VertexWorksheetPopulator oVertexWorksheetPopulator =
			new VertexWorksheetPopulator();

        this.ScreenUpdating = false;

		try
		{
			oVertexWorksheetPopulator.PopulateVertexWorksheet(
				this.InnerObject, activateVertexWorksheetWhenDone);

			this.ScreenUpdating = true;

			return (true);
		}
		catch (Exception oException)
		{
			// Don't let Excel handle unhandled exceptions.

			this.ScreenUpdating = true;

			if (notifyUserOnError)
			{
				ErrorUtil.OnException(oException);

				return (false);
			}
			else
			{
				throw oException;
			}
		}
    }

    //*************************************************************************
    //  Method: CreateSubgraphImages()
    //
    /// <summary>
	/// Creates a subgraph of each of the graph's vertices and saves the images
	/// to disk or the workbook.
    /// </summary>
    //*************************************************************************

    public void
	CreateSubgraphImages()
    {
		AssertValid();

		if ( !this.ExcelApplicationIsReady(true) )
		{
			return;
		}

		// Populate the vertex worksheet.  This is necessary in case the user
		// opts to insert images into the vertex worksheet.  Note that
		// PopulateVertexWorksheet() returns false if the vertex worksheet or
		// table is missing, and that it activates the vertex worksheet.

		if ( !PopulateVertexWorksheet(true, true) )
		{
			return;
		}

		// Get an array of vertex names that are selected in the vertex
		// worksheet.

		String [] asSelectedVertexNames = new String[0];

		Debug.Assert(this.Application.ActiveSheet is Worksheet);

		Debug.Assert( ( (Worksheet)this.Application.ActiveSheet ).Name ==
			WorksheetNames.Vertices);

		Object oSelection = this.Application.Selection;

		if (oSelection != null && oSelection is Range)
		{
			asSelectedVertexNames =
				Globals.Sheet2.GetSelectedVertexNames( (Range)oSelection );
		}

		CreateSubgraphImagesDialog oCreateSubgraphImagesDialog =
			new CreateSubgraphImagesDialog(this.InnerObject,
				asSelectedVertexNames);

		oCreateSubgraphImagesDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: CustomizeVertexMenu()
    //
    /// <summary>
	/// Adds two columns to the vertex table for customizing vertex context
	/// menus in the .NetMap graph.
    /// </summary>
	///
    /// <param name="notifyUserOnError">
	/// If true, the user is notified when an error occurs.  If false, an
	/// exception is thrown when an error occurs.
    /// </param>
    //*************************************************************************

    public void
	CustomizeVertexMenu
	(
		Boolean notifyUserOnError
	)
    {
		AssertValid();

		if ( !this.ExcelApplicationIsReady(true) )
		{
			return;
		}

		const String Message =
			"Use this to add custom menu items to the menu that appears when"
			+ " you right-click a vertex in the .NetMap graph."
			+ "\r\n\r\n"
			+ "Clicking \"Yes\" below will add a pair of columns to the"
			+ " Vertices worksheet -- one for menu item text and another for"
			+ " the action to take when the menu item is selected."
			+ "\r\n\r\n"
			+ " For example, if you add the column pair and enter \"Send Mail"
			+ " To\" for a vertex's menu item text and \"mailto:bob@msn.com\""
			+ " for the action, then right-clicking the vertex in the .NetMap"
			+ " graph and selecting \"Send Mail To\" from the right-click menu"
			+ " will open a new email message addressed to bob@msn.com."
			+ "\r\n\r\n"
			+ "If you want to open a Web page when the menu item is selected,"
			+ " enter an URL for the action."
			+ "\r\n\r\n"
			+ "If you want to add more than one custom menu item to a vertex's"
			+ " right-click menu, run this again to add another pair of"
			+ " columns."
			+ "\r\n\r\n"
			+ "Do you want to add a pair of columns to the Vertices worksheet?"
			;

		if (MessageBox.Show(Message, FormUtil.ApplicationName,
				MessageBoxButtons.YesNo, MessageBoxIcon.Information) !=
				DialogResult.Yes)
		{
			return;
		}

		// Create and use the object that adds the columns to the vertex
		// table.

		TableColumnAdder oTableColumnAdder = new TableColumnAdder();

        this.ScreenUpdating = false;

		try
		{
			oTableColumnAdder.AddColumnPair(this.InnerObject,
				WorksheetNames.Vertices, TableNames.Vertices,
				VertexTableColumnNames.CustomMenuItemTextBase, 25,
				VertexTableColumnNames.CustomMenuItemActionBase, 26
				);

			this.ScreenUpdating = true;
		}
		catch (Exception oException)
		{
			// Don't let Excel handle unhandled exceptions.

			this.ScreenUpdating = true;

			if (notifyUserOnError)
			{
				ErrorUtil.OnException(oException);
			}
			else
			{
				throw oException;
			}
		}
    }

    //*************************************************************************
    //  Method: AnalyzeEmailNetwork()
    //
    /// <summary>
	/// Shows the dialog that analyzes a user's email network and writes the
	/// results to the edge worksheet.
    /// </summary>
    //*************************************************************************

    public void
	AnalyzeEmailNetwork()
    {
		AssertValid();

		if ( !this.ExcelApplicationIsReady(true) )
		{
			return;
		}

		AnalyzeEmailNetworkDialog oAnalyzeEmailNetworkDialog =
			new AnalyzeEmailNetworkDialog(this.InnerObject);

		oAnalyzeEmailNetworkDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: EditAutoFillUserSettings()
    //
    /// <summary>
	/// Shows the dialog that lets the user edit his settings for the
	/// application's AutoFill feature, which automatically fills edge and
	/// vertex attribute columns using values from user-specified source
	/// columns.
    /// </summary>
    //*************************************************************************

    public void
	EditAutoFillUserSettings()
    {
		AssertValid();

		if ( !this.ExcelApplicationIsReady(true) )
		{
			return;
		}

		// Allow the user to edit the AutoFill settings.  The dialog makes a
		// copy of the settings.

		AutoFillUserSettings oAutoFillUserSettings =
			new AutoFillUserSettings();

		AutoFillUserSettingsDialog oAutoFillUserSettingsDialog =
			new AutoFillUserSettingsDialog(this.InnerObject,
				oAutoFillUserSettings);

		if (oAutoFillUserSettingsDialog.ShowDialog() == DialogResult.OK)
		{
			// Save the edited copy.

			oAutoFillUserSettingsDialog.AutoFillUserSettings.Save();
		}
    }

    //*************************************************************************
    //  Method: EditGraphMetricUserSettings()
    //
    /// <summary>
	/// Shows the dialog that lets the user edit his settings for calculating
	/// graph metrics.
    /// </summary>
    //*************************************************************************

    public void
	EditGraphMetricUserSettings()
    {
		AssertValid();

		if ( !this.ExcelApplicationIsReady(true) )
		{
			return;
		}

		// Allow the user to edit the graph metric settings.

		GraphMetricUserSettings oGraphMetricUserSettings =
			new GraphMetricUserSettings();

		GraphMetricUserSettingsDialog oGraphMetricUserSettingsDialog =
			new GraphMetricUserSettingsDialog(oGraphMetricUserSettings);

		if (oGraphMetricUserSettingsDialog.ShowDialog() == DialogResult.OK)
		{
			// Save the edited object.

			oGraphMetricUserSettings.Save();
		}
    }

    //*************************************************************************
    //  Method: CalculateGraphMetrics()
    //
    /// <summary>
	/// Calculates the graph metrics.
    /// </summary>
    //*************************************************************************

    public void
	CalculateGraphMetrics()
    {
		AssertValid();

		if ( !this.ExcelApplicationIsReady(true) )
		{
			return;
		}

		GraphMetricUserSettings oGraphMetricUserSettings =
			new GraphMetricUserSettings();

		if (!oGraphMetricUserSettings.AtLeastOneMetricSelected)
		{
			FormUtil.ShowWarning(
				"No graph metrics have been selected.  To select one or more"
				+ " graph metrics, click the arrow to the right of the"
				+ " \"Calculate Graph Metrics\" button, then click the"
				+ " \"Select Graph Metrics\" button."
				);

			return;
		}

		// The CalculateGraphMetricsDialog does all the work.

		CalculateGraphMetricsDialog oCalculateGraphMetricsDialog =
			new CalculateGraphMetricsDialog( this.InnerObject,
			oGraphMetricUserSettings
			);

		oCalculateGraphMetricsDialog.ShowDialog();
    }

	//*************************************************************************
	//	Event: SelectionChangedInWorkbook
	//
	/// <summary>
	///	Occurs when the selection state of the edge or vertex table changes.
	/// </summary>
	///
	/// <remarks>
	/// If the selection state of the edge table changes, the <see
	/// cref="SelectionChangedEventArgs.SelectedEdgeIDs" /> property of the
	/// <see cref="SelectionChangedEventArgs" /> object passed to the event
	/// handler contains the IDs of the selected edges.  The <see
	/// cref="SelectionChangedEventArgs.SelectedVertexIDs" /> property is an
	/// empty array in this case.
	///
	/// <para>
	/// If the selection state of the vertex table changes, the <see
	/// cref="SelectionChangedEventArgs.SelectedVertexIDs" /> property of the
	/// <see cref="SelectionChangedEventArgs" /> object passed to the event
	/// handler contains the IDs of the selected vertices.  The <see
	/// cref="SelectionChangedEventArgs.SelectedEdgeIDs" /> property is an
	/// empty array in this case.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public event SelectionChangedEventHandler SelectionChangedInWorkbook;


	//*************************************************************************
	//	Event: SelectionChangedInGraph
	//
	/// <summary>
	///	Occurs when the selection state of the NetMap graph changes.
	/// </summary>
	///
	/// <remarks>
	/// The <see cref="SelectionChangedEventArgs" /> object passed to the event
	/// handler contains the IDs of all vertices and edges that are currently
	/// selected in the graph.
	/// </remarks>
	//*************************************************************************

	public event SelectionChangedEventHandler SelectionChangedInGraph;


	//*************************************************************************
	//	Event: VertexAttributesEditedInGraph
	//
	/// <summary>
	///	Occurs when vertex attributes are edited in the NetMap graph.
	/// </summary>
	//*************************************************************************

	public event VertexAttributesEditedEventHandler
		VertexAttributesEditedInGraph;


	//*************************************************************************
	//	Event: GraphDrawn
	//
	/// <summary>
	///	Occurs after graph drawing completes.
	/// </summary>
	///
	/// <remarks>
	/// Graph drawing occurs asynchronously.  This event fires when the graph
	/// is completely drawn.
	/// </remarks>
	//*************************************************************************

	public event GraphDrawnEventHandler GraphDrawn;


	//*************************************************************************
	//	Event: VertexMoved
	//
	/// <summary>
	///	Occurs after a vertex is moved to a new location in the graph.
	/// </summary>
	///
	/// <remarks>
	/// This event is fired when the user releases the mouse button after
	/// dragging a vertex to a new location in the graph.
	/// </remarks>
	//*************************************************************************

	public event VertexMovedEventHandler VertexMoved;


	//*************************************************************************
	//	Property: ScreenUpdating
	//
	/// <summary>
	/// Set a flag specifying whether Excel's screen updating is on or off.
	/// </summary>
	///
	/// <value>
	/// true to turn on screen updating.
	/// </value>
	//*************************************************************************

	private Boolean
	ScreenUpdating
	{
		set
		{
            this.Application.ScreenUpdating = value;

			AssertValid();
		}
	}

    //*************************************************************************
    //  Method: ExcelApplicationIsReady
    //
    /// <summary>
    /// Determines whether the Excel application is ready to accept method
	/// calls.
    /// </summary>
	///
	/// <param name="bShowBusyMessage">
	/// true if a busy message should be displayed if the application is not
	/// ready.
	/// </param>
    ///
    /// <returns>
	/// true if the Excel application is ready to accept method calls.
    /// </returns>
    //*************************************************************************

	private Boolean
	ExcelApplicationIsReady
	(
		Boolean bShowBusyMessage
	)
    {
		AssertValid();

		if ( !ExcelUtil.ExcelApplicationIsReady(this.Application) )
		{
			if (bShowBusyMessage)
			{
				FormUtil.ShowWarning(
					"This feature isn't available while a worksheet cell is"
					+ " being edited.  Press Enter to finish editing the cell,"
					+ " then try again."
				);
			}

			return (false);
		}

		return (true);
    }


    //*************************************************************************
    //  Method: FireSelectionChangedInWorkbook()
    //
    /// <summary>
	/// Fires the <see cref="SelectionChangedInWorkbook" /> event if
    /// appropriate.
    /// </summary>
	///
    /// <param name="aiSelectedEdgeIDs">
	/// Array of unique IDs of edges that have at least one selected cell.  Can
	/// be empty but not null.
    /// </param>
	///
    /// <param name="aiSelectedVertexIDs">
	/// Array of unique IDs of vertices that have at least one selected cell.
	/// Can be empty but not null.
    /// </param>
    //*************************************************************************

    private void
	FireSelectionChangedInWorkbook
	(
		Int32 [] aiSelectedEdgeIDs,
		Int32 [] aiSelectedVertexIDs
	)
    {
		Debug.Assert(aiSelectedEdgeIDs != null);
		Debug.Assert(aiSelectedVertexIDs != null);
		AssertValid();

		SelectionChangedEventHandler oSelectionChangedInWorkbook =
            this.SelectionChangedInWorkbook;

        if (oSelectionChangedInWorkbook != null)
		{
			try
			{
				oSelectionChangedInWorkbook(this,
					new SelectionChangedEventArgs(
						aiSelectedEdgeIDs, aiSelectedVertexIDs) );
			}
			catch (Exception oException)
			{
				// If exceptions aren't caught here, Excel consumes them
				// without indicating that anything is wrong.

				ErrorUtil.OnException(oException);
			}
		}
    }

    //*************************************************************************
    //  Method: Workbook_Startup()
    //
    /// <summary>
	/// Handles the Startup event on the workbook.
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
	ThisWorkbook_Startup
	(
		object sender,
		System.EventArgs e
	)
	{
		m_bTaskPaneCreated = false;

		m_oWorksheetContextMenuManager = new WorksheetContextMenuManager(
			this, Globals.Sheet1, Globals.Sheet1.Edges, Globals.Sheet2,
			Globals.Sheet2.Vertices);

		// Prevent this situation: The user doesn't close the actions pane
		// before saving the document.  When the document is reopened, an empty
		// actions pane is shown.

		this.ActionsPane.Clear();

		// In message boxes, show the name of this document customization
		// instead of the default, which is the name of the Excel application.

		FormUtil.ApplicationName = DocumentCustomizationName;

		Globals.Sheet1.EdgeSelectionChanged +=
            new TableSelectionChangedEventHandler(
				this.Sheet1_EdgeSelectionChanged);

		Globals.Sheet2.VertexSelectionChanged +=
            new TableSelectionChangedEventHandler(
				this.Sheet2_VertexSelectionChanged);

        AssertValid();
	}

    //*************************************************************************
    //  Method: Workbook_Shutdown()
    //
    /// <summary>
	/// Handles the Shutdown event on the workbook.
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
	ThisWorkbook_Shutdown
	(
		object sender,
		System.EventArgs e
	)
	{
		AssertValid();

		// (Do nothing.)
	}

    //*************************************************************************
    //  Method: Sheet1_EdgeSelectionChanged()
    //
    /// <summary>
	/// Handles the EdgeSelectionChanged event on the Sheet1 (edge) worksheet.
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
	Sheet1_EdgeSelectionChanged
	(
		Object sender,
		TableSelectionChangedEventArgs e
	)
    {
		Debug.Assert(e != null);
		Debug.Assert(e.SelectedIDs != null);
		AssertValid();

		if ( !this.ExcelApplicationIsReady(false) )
		{
			return;
		}

		// If the event was caused by the user selecting edges in the edge
		// worksheet, forward the event to the TaskPane.  Otherwise, avoid an
		// endless loop by doing nothing.

		if (e.EventOrigin ==
			TableSelectionChangedEventOrigin.SelectionChangedInTable)
		{
			FireSelectionChangedInWorkbook(e.SelectedIDs,
				WorksheetReaderBase.EmptyIDArray);
		}
    }

    //*************************************************************************
    //  Method: Sheet2_VertexSelectionChanged()
    //
    /// <summary>
	/// Handles the VertexSelectionChanged event on the Sheet2 (vertex)
	/// worksheet.
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
	Sheet2_VertexSelectionChanged
	(
		Object sender,
		TableSelectionChangedEventArgs e
	)
    {
		Debug.Assert(e != null);
		Debug.Assert(e.SelectedIDs != null);
		AssertValid();

		if ( !this.ExcelApplicationIsReady(false) )
		{
			return;
		}

		// If the event was caused by the user selecting vertices in the vertex
		// worksheet, forward the event to the TaskPane.  Otherwise, avoid an
		// endless loop and do nothing.

		if (e.EventOrigin == TableSelectionChangedEventOrigin.
			SelectionChangedInTable)
		{
			FireSelectionChangedInWorkbook(WorksheetReaderBase.EmptyIDArray,
				e.SelectedIDs);
		}
    }

	//*************************************************************************
	//	Method: TaskPane_SelectionChangedInGraph()
	//
	/// <summary>
	/// Handles the SelectionChangedInGraph event on the TaskPane.
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
	/// This event is fired when the user clicks on the NetMap graph.
	/// </remarks>
	//*************************************************************************

	private void
	TaskPane_SelectionChangedInGraph
	(
		Object sender,
		SelectionChangedEventArgs e
	)
	{
		Debug.Assert(e != null);
		AssertValid();

		if ( !this.ExcelApplicationIsReady(false) )
		{
			return;
		}

		// Forward the event.

		SelectionChangedEventHandler oSelectionChangedInGraph =
			this.SelectionChangedInGraph;

		if (oSelectionChangedInGraph != null)
		{
			try
			{
				oSelectionChangedInGraph(this, e);
			}
			catch (Exception oException)
			{
				ErrorUtil.OnException(oException);
			}
		}
	}

	//*************************************************************************
	//	Method: TaskPane_VertexAttributesEditedInGraph()
	//
	/// <summary>
	/// Handles the VertexAttributesEditedInGraph event on the TaskPane.
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
	/// This event is fired when the user edits vertex attributes in the NetMap
	/// graph.
	/// </remarks>
	//*************************************************************************

	private void
	TaskPane_VertexAttributesEditedInGraph
	(
		Object sender,
		VertexAttributesEditedEventArgs e
	)
	{
		Debug.Assert(e != null);
		AssertValid();

		if ( !this.ExcelApplicationIsReady(false) )
		{
			return;
		}

		// Forward the event.

		VertexAttributesEditedEventHandler oVertexAttributesEditedInGraph =
			this.VertexAttributesEditedInGraph;

		if (oVertexAttributesEditedInGraph != null)
		{
			try
			{
				oVertexAttributesEditedInGraph(this, e);
			}
			catch (Exception oException)
			{
				ErrorUtil.OnException(oException);
			}
		}
	}

	//*************************************************************************
	//	Method: TaskPane_GraphDrawn()
	//
	/// <summary>
	/// Handles the GraphDrawn event on the TaskPane.
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
	/// Graph drawing occurs asynchronously.  This event fires when the graph
	/// is completely drawn.
	/// </remarks>
	//*************************************************************************

	private void
	TaskPane_GraphDrawn
	(
		Object sender,
		GraphDrawnEventArgs e
	)
	{
		Debug.Assert(e != null);
		AssertValid();

		if ( !this.ExcelApplicationIsReady(false) )
		{
			return;
		}

		// Forward the event.

		GraphDrawnEventHandler oGraphDrawn = this.GraphDrawn;

		if (oGraphDrawn != null)
		{
			try
			{
				oGraphDrawn(this, e);
			}
			catch (Exception oException)
			{
				ErrorUtil.OnException(oException);
			}
		}
	}

	//*************************************************************************
	//	Method: TaskPane_VertexMoved()
	//
	/// <summary>
	/// Handles the VertexMoved event on the TaskPane.
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
	/// Graph drawing occurs asynchronously.  This event fires when the graph
	/// is completely drawn.
	/// </remarks>
	//*************************************************************************

	private void
	TaskPane_VertexMoved
	(
		Object sender,
		VertexMovedEventArgs e
	)
	{
		Debug.Assert(e != null);
		AssertValid();

		if ( !this.ExcelApplicationIsReady(false) )
		{
			return;
		}

		// Forward the event.

		VertexMovedEventHandler oVertexMoved = this.VertexMoved;

		if (oVertexMoved != null)
		{
			try
			{
				oVertexMoved(this, e);
			}
			catch (Exception oException)
			{
				ErrorUtil.OnException(oException);
			}
		}
	}


	#region VSTO Designer generated code

	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InternalStartup()
	{
		this.Startup += new System.EventHandler(ThisWorkbook_Startup);
		this.Shutdown += new System.EventHandler(ThisWorkbook_Shutdown);
	}
        
	#endregion


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
		Debug.Assert(m_oWorksheetContextMenuManager != null);
		// m_bTaskPaneCreated
    }


    //*************************************************************************
    //  Public fields
    //*************************************************************************

	/// Friendly name of this document customization.

	public static readonly String DocumentCustomizationName =
		"Microsoft .NetMap";


    //*************************************************************************
    //  Private fields
    //*************************************************************************

	/// Object that adds custom menu items to the Excel context menus that
	/// appear when the vertex or edge table is right-clicked.

	private WorksheetContextMenuManager m_oWorksheetContextMenuManager;

	/// true if the task pane has been created.

	private Boolean m_bTaskPaneCreated;
}

}
