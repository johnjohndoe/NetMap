
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.NetMap.Core;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NetMap.ApplicationUtil
{
//*****************************************************************************
//	Class: GraphInfoViewer
//
/// <summary>
/// Displays information about a graph's vertices and edges.
/// </summary>
///
/// <remarks>
/// This control is meant to be used in conjunction with a NetMapControl.  It
/// has one ListView that displays information about the vertices in the
/// NetMapControl's graph, and another that displays information about the
/// graph's edges.  The user can select one or more vertices and edges by
/// clicking on the NetMapControl, which causes the corresponding ListView items
/// to be selected.  He can also select one or more ListView items, which
/// causes the corresponding vertices and edges to be selected in the
/// NetMapControl.  Context menus on the ListViews provide additional options.
///
/// <para>
/// Initialize this control by setting its <see cref="Graph" /> property.  When
/// the user changes the selected vertices and edges in the NetMapControl, call
/// this control's  <see cref="SetSelection" /> method.  When this control's
/// <see cref="RequestSelection" /> event fires, select the specified vertices
/// and edges in the NetMapControl.
/// </para>
///
/// <para>
/// If <see cref="UseHotKeys" /> is true, the control uses these HotKeys: D, V.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class GraphInfoViewer : UserControl
{
	//*************************************************************************
	//	Constructor: GraphInfoViewer()
	//
	/// <summary>
	///	Initializes a new instance of the <see cref="GraphInfoViewer" />
	/// class.
	/// </summary>
	//*************************************************************************

	public GraphInfoViewer()
	{
		InitializeComponent();

		m_bUseHotKeys = true;
		m_bSettingSelection = false;
		m_bFiringRequestSelection = false;
		m_oSelectedIndexChangedTimer = null;

		// Instantiate an object that saves and retrieves the user settings for
		// this GraphInfoViewer.  Note that the object automatically saves the
		// settings when the parent form closes.

        m_oGraphInfoViewerSettings = new GraphInfoViewerSettings(this);

		AssertValid();
	}

	//*************************************************************************
	//	Property: Graph
	//
	/// <summary>
	///	Sets the graph whose vertex and edge information should be displayed in
	/// the control.
	/// </summary>
	///
	/// <value>
	///	The graph whose vertex and edge information should be displayed in the
	/// control, as an <see cref="IGraph" />.
	/// </value>
	//*************************************************************************

	public IGraph
	Graph
	{
		set
		{
			Debug.Assert(value != null);

			PopulateVerticesListView(value);
			PopulateEdgesListView(value);

			AssertValid();
		}
	}

    //*************************************************************************
    //  Property: UseHotKeys
    //
    /// <summary>
    /// Gets or sets a flag specifying whether HotKeys should be used in
	/// labels.
    /// </summary>
    ///
    /// <value>
	/// true to use HotKeys.  The default is true.
    /// </value>
    ///
    /// <remarks>
	/// This should be set to false in applications that do not support
	/// HotKeys, such as VSTO Excel applications.
    /// </remarks>
    //*************************************************************************

	[System.ComponentModel.CategoryAttribute("Behavior")]

    public Boolean
	UseHotKeys
    {
        get
        {
            AssertValid();

            return (m_bUseHotKeys);
        }

        set
        {
			m_bUseHotKeys = value;

            AssertValid();
        }
    }

	//*************************************************************************
	//	Method: SetSelection
	//
	/// <summary>
	/// Selects vertices and edges in this control.
	/// </summary>
	///
	/// <param name="selectedVertices">
	///	Zero or more vertices that should be selected.
	/// </param>
	///
	/// <param name="selectedEdges">
	///	Zero or more edges that should be selected.
	/// </param>
	///
	/// <remarks>
	/// All vertices and edges not specified in <paramref
	/// name="selectedVertices" /> and <paramref name="selectedEdges" /> are
	/// deselected.
	/// </remarks>
	//*************************************************************************

	public void
	SetSelection
	(
		IVertex [] selectedVertices,
		IEdge [] selectedEdges
	)
	{
		Debug.Assert(selectedVertices != null);
		Debug.Assert(selectedEdges != null);
		AssertValid();

		if (m_bFiringRequestSelection)
		{
			// A RequestSelection event is in progress, meaning that this
			// GraphInfoViewer is telling the NetMapControl to change its
			// selection and the NetMapControl is letting the GraphInfoViewer
			// know that the selection has changed.  The selection state of the
			// ListView items is already in sync with the NetMapControl, so
			// ignore the event.

			return;
		}

		this.UseWaitCursor = true;

		OnSetSelection(selectedVertices, selectedEdges);

		this.UseWaitCursor = false;

		AssertValid();
	}

	//*************************************************************************
	//	Event: RequestSelection
	//
	/// <summary>
	///	Occurs when the user wants to modify the selection in the associated
	/// NetMapControl.
	/// </summary>
	//*************************************************************************

	public event RequestSelectionEventHandler RequestSelection;


	//*************************************************************************
	//	Method: PopulateVerticesListView
	//
	/// <summary>
	/// Populates the lvwVertices ListView.
	/// </summary>
	///
	/// <param name="oGraph">
	///	Graph whose information is displayed in this control.  Can't be null.
	/// </param>
	//*************************************************************************

	protected void
	PopulateVerticesListView
	(
		IGraph oGraph
	)
	{
		Debug.Assert(oGraph != null);
		AssertValid();

		Boolean bGraphIsUndirected =
			(oGraph.Directedness == GraphDirectedness.Undirected);

		// Clear the item and column collections.

		ListView.ListViewItemCollection oItems = lvwVertices.Items;
		ListView.ColumnHeaderCollection oColumns = lvwVertices.Columns;

		oItems.Clear();
        oColumns.Clear();

		// Populate the column collection.

		oColumns.Add("Name");

		oColumns.Add("Degree", 60, HorizontalAlignment.Right);

		if (bGraphIsUndirected)
		{
			lvwVertices.EnableSorting( typeof(String), typeof(Int32) );
		}
		else
		{
			oColumns.Add("In-Degree", 70, HorizontalAlignment.Right);
			oColumns.Add("Out-Degree", 70, HorizontalAlignment.Right);

			lvwVertices.EnableSorting( typeof(String), typeof(Int32),
				typeof(Int32), typeof(Int32) );
		}

		// Populate the list of vertices.

		IVertexCollection oVertices = oGraph.Vertices;

		lvwVertices.BeginUpdate();

		foreach (IVertex oVertex in oVertices)
		{
			// Add an item for the vertex and save the vertex in the item's
			// Tag.

			ListViewItem oItem = oItems.Add(oVertex.Name);

			oItem.Tag = oVertex;

			// Add subitems for the item.

			IEdge [] aoIncidentEdges = oVertex.IncidentEdges;

			Int32 iIncomingEdges, iOutgoingEdges;

			GetEdgeStatistics(oVertex, aoIncidentEdges, out iIncomingEdges,
				out iOutgoingEdges);

			ListViewItem.ListViewSubItemCollection oSubItems =
				oItem.SubItems;

			oSubItems.Add(aoIncidentEdges.Length.ToString(Int32Format) );

			if (!bGraphIsUndirected)
			{
				oSubItems.Add(iIncomingEdges.ToString(Int32Format) );

				oSubItems.Add(iOutgoingEdges.ToString(Int32Format) );
			}

			// Save the item as vertex metadata.  This allows a vertex's item
			// to be quickly retrieved without looping through all the items in
			// the ListView.

			oVertex.SetValue(
				ReservedMetadataKeys.GraphInfoViewerListViewItem, oItem);
		}

		Int32 iVertices = oVertices.Count;

		if (iVertices == 0)
		{
			// Add a "none" item.  Leave its Tag set to null.

			oItems.Add(EmptyListText);
		}

		lvwVertices.EndUpdate();

		// Display the number of vertices.

		lblVertexCount.Text = String.Format(

			"{0} {1}{2}:"
			,
			iVertices.ToString(Int32Format),
			m_bUseHotKeys ? "&" : String.Empty,
			(iVertices == 1) ? "vertex" : "vertices"
			);
	}

	//*************************************************************************
	//	Method: PopulateEdgesListView
	//
	/// <summary>
	/// Populates the lvwEdges ListView.
	/// </summary>
	///
	/// <param name="oGraph">
	///	Graph whose information is displayed in this control.  Can't be null.
	/// </param>
	//*************************************************************************

	protected void
	PopulateEdgesListView
	(
		IGraph oGraph
	)
	{
		Debug.Assert(oGraph != null);
		AssertValid();

		// Clear the item and column collections.

		ListView.ListViewItemCollection oItems = lvwEdges.Items;
		ListView.ColumnHeaderCollection oColumns = lvwEdges.Columns;

		oItems.Clear();
        oColumns.Clear();

		// Populate the column collection.

		oColumns.Add("Vertex 1");

		oColumns.Add("Vertex 2", 60, HorizontalAlignment.Left);

		oColumns.Add("Weight", 60, HorizontalAlignment.Right);

		lvwEdges.EnableSorting(
			typeof(String), typeof(String), typeof(Single) );

		// Populate the list of edges.

		IEdgeCollection oEdges = oGraph.Edges;

		lvwEdges.BeginUpdate();

		foreach (IEdge oEdge in oEdges)
		{
			// Add an item for the edge and save the edge in the item's Tag.

			IVertex [] aoVertices = oEdge.Vertices;

			ListViewItem oItem = oItems.Add(aoVertices[0].Name);

			oItem.Tag = oEdge;

			// Add subitems for the item.

			ListViewItem.ListViewSubItemCollection oSubItems =
				oItem.SubItems;

			oSubItems.Add(aoVertices[1].Name);

			// If the edge has a weight, add it to the third column.

			Object oEdgeWeight;
			String sEdgeWeight = String.Empty;

			if ( oEdge.TryGetValue(ReservedMetadataKeys.EdgeWeight,
				typeof(Single), out oEdgeWeight ) )
			{
				sEdgeWeight = ( (Single)oEdgeWeight ).ToString(SingleFormat);
			}

			oItem.SubItems.Add(sEdgeWeight);

			// Save the item as edge metadata.  This allows an edge's item to
			// be quickly retrieved without looping through all the items in
			// the ListView.

			oEdge.SetValue(
				ReservedMetadataKeys.GraphInfoViewerListViewItem, oItem);
		}

		Int32 iEdges = oEdges.Count;

		if (iEdges == 0)
		{
			// Add a "none" item.  Leave its Tag set to null.

			oItems.Add(EmptyListText);
		}

		lvwEdges.EndUpdate();

		// Display the number of edges.

		lblEdgeCount.Text = String.Format(

			"{0} e{1}dge{2}:"
			,
			iEdges.ToString(Int32Format),
			m_bUseHotKeys ? "&" : String.Empty,
			(iEdges == 1) ? String.Empty : "s"
			);
	}

	//*************************************************************************
	//	Method: SelectVerticesInVerticesListView
	//
	/// <summary>
	///	Selects vertices in the lvwVertices ListView.
	/// </summary>
	///
	/// <param name="aoVertices">
	/// Vertices to select.  Can't be null.
	/// </param>
	//*************************************************************************

	protected void
	SelectVerticesInVerticesListView
	(
		IVertex [] aoVertices
	)
	{
		Debug.Assert(aoVertices != null);
		AssertValid();

		SelectItemsInListView(lvwVertices, ( Object [] )aoVertices);
	}

	//*************************************************************************
	//	Method: SelectEdgesInEdgesListView
	//
	/// <summary>
	///	Selects edges in the lvwEdges ListView.
	/// </summary>
	///
	/// <param name="aoEdges">
	/// Edges to select.  Can't be null.
	/// </param>
	//*************************************************************************

	protected void
	SelectEdgesInEdgesListView
	(
		IEdge [] aoEdges
	)
	{
		Debug.Assert(aoEdges != null);
		AssertValid();

		SelectItemsInListView(lvwEdges, ( Object [] )aoEdges);
	}

	//*************************************************************************
	//	Method: SelectItemsInListView
	//
	/// <summary>
	///	Selects items in the lvwVertices or lvwEdges ListView.
	/// </summary>
	///
	/// <param name="oListView">
	/// ListView to select items within.
	/// </param>
	///
	/// <param name="aoVerticesOrEdges">
	/// Array of vertices or edges to select.
	/// </param>
	//*************************************************************************

	protected void
	SelectItemsInListView
	(
		ListViewPlus oListView,
        Object[] aoVerticesOrEdges
	)
	{
		Debug.Assert(oListView != null);
		Debug.Assert(aoVerticesOrEdges != null);
		AssertValid();

		m_bSettingSelection = true;

		oListView.DeselectAllItems();

		ListViewItem oFirstItem = null;

		foreach (IMetadataProvider oVertexOrEdge in aoVerticesOrEdges)
		{
			// Retrieve the ListViewItem corresponding to the vertex or edge,
			// which was stored as metadata.

            ListViewItem oItem = (ListViewItem)oVertexOrEdge.GetRequiredValue(
				ReservedMetadataKeys.GraphInfoViewerListViewItem,
				typeof(ListViewItem)
				);

			oItem.Selected = true;

			if (oFirstItem == null)
			{
				oFirstItem = oItem;
			}
		}

		// Make the first selected item visible.

		if (oFirstItem != null)
		{
			oFirstItem.EnsureVisible();
		}

		m_bSettingSelection = false;
	}

	//*************************************************************************
	//	Method: SelectIncidentEdgesOfSelectedVertices()
	//
	/// <summary>
	/// Selects or deselects all incident edges of all vertices that are
	/// selected in the lvwVertices ListView.
	/// </summary>
	///
	/// <param name="bSelected">
	/// true to select the edges, false to deselect them.
	/// </param>
	//*************************************************************************

	protected void
	SelectIncidentEdgesOfSelectedVertices
	(
		Boolean bSelected
	)
	{
		AssertValid();

		// Loop through the selected items in the lvwVertices ListView.

		foreach (ListViewItem oItem in lvwVertices.SelectedItems)
		{
			// Get the vertex corresponding to the item.

			Debug.Assert(oItem.Tag != null);
			Debug.Assert(oItem.Tag is IVertex);

			IVertex oVertex = (IVertex)oItem.Tag;

			// Loop through the vertex's incident edges.

			foreach (IEdge oEdge in oVertex.IncidentEdges)
			{
				// Get the edge's item in the lvwEdges ListView and select or
				// deselect the item.

				ListViewItem oEdgeItem = (ListViewItem)oEdge.GetRequiredValue(
					ReservedMetadataKeys.GraphInfoViewerListViewItem,
					typeof(ListViewItem)
					);

				oEdgeItem.Selected = bSelected;
			}
		}

		FireRequestSelection();
	}

	//*************************************************************************
	//	Method: SelectAdjacentVerticesOfSelectedVertices()
	//
	/// <summary>
	/// Selects or deselects all adjacent vertices of all vertices that are
	/// selected in the lvwVertices ListView.
	/// </summary>
	///
	/// <param name="bSelected">
	/// true to select the vertices, false to deselect them.
	/// </param>
	//*************************************************************************

	protected void
	SelectAdjacentVerticesOfSelectedVertices
	(
		Boolean bSelected
	)
	{
		AssertValid();

		// Copy the array of items selected in the lvwVertices ListView.  Other
		// items will be selected or deselected below, so a static copy is
		// needed.

		ListView.SelectedListViewItemCollection oSelectedItems =
			lvwVertices.SelectedItems;

		ListViewItem [] aoSelectedItems =
			new ListViewItem[oSelectedItems.Count];

		oSelectedItems.CopyTo(aoSelectedItems, 0);

		// Loop through the selected items.

		foreach (ListViewItem oItem in aoSelectedItems)
		{
			// Get the vertex corresponding to the item.

			Debug.Assert(oItem.Tag != null);
			Debug.Assert(oItem.Tag is IVertex);

			IVertex oVertex = (IVertex)oItem.Tag;

			// Loop through the vertex's adjacent vertices.

			foreach (IVertex oAdjacentVertex in oVertex.AdjacentVertices)
			{
				// Get the adjacent vertex's item in the lvwVertices ListView
				// and select or deselect the item.

				ListViewItem oAdjacentVertexItem =
					(ListViewItem)oAdjacentVertex.GetRequiredValue(
						ReservedMetadataKeys.GraphInfoViewerListViewItem,
						typeof(ListViewItem)
						);

				oAdjacentVertexItem.Selected = bSelected;
			}
		}

		FireRequestSelection();
	}

	//*************************************************************************
	//	Method: SelectAdjacentVerticesOfSelectedEdges()
	//
	/// <summary>
	/// Selects or deselects all adjacent vertices of all edges that are
	/// selected in the lvwEdges ListView.
	/// </summary>
	///
	/// <param name="bSelected">
	/// true to select the vertices, false to deselect them.
	/// </param>
	//*************************************************************************

	protected void
	SelectAdjacentVerticesOfSelectedEdges
	(
		Boolean bSelected
	)
	{
		AssertValid();

		// Loop through the selected items in the lvwEdges ListView.

		foreach (ListViewItem oItem in lvwEdges.SelectedItems)
		{
			// Get the edge corresponding to the item.

			Debug.Assert(oItem.Tag != null);
			Debug.Assert(oItem.Tag is IEdge);

			IEdge oEdge = (IEdge)oItem.Tag;

			// Loop through the edge's adjacent vertices.

			foreach (IVertex oVertex in oEdge.Vertices)
			{
				// Get the vertex's item in the lvwVertices ListView and select
				// or deselect the item.

				ListViewItem oVertexItem =
					(ListViewItem)oVertex.GetRequiredValue(
						ReservedMetadataKeys.GraphInfoViewerListViewItem,
						typeof(ListViewItem)
						);

				oVertexItem.Selected = bSelected;
			}
		}

		FireRequestSelection();
	}

	//*************************************************************************
	//	Method: GetEdgeStatistics
	//
	/// <summary>
	/// Calculates statistics about a vertex's incident edges.
	/// </summary>
	///
	/// <param name="oVertex">
	/// The vertex.
	/// </param>
	///
	/// <param name="aoIncidentEdges">
	/// The vertex's incident edges.
	/// </param>
	///
	/// <param name="iIncomingEdges">
	/// Where the number of incoming edges gets stored.
	/// </param>
	///
	/// <param name="iOutgoingEdges">
	/// Where the number of outgoing edges gets stored.
	/// </param>
	//*************************************************************************

	protected void
	GetEdgeStatistics
	(
		IVertex oVertex,
		IEdge [] aoIncidentEdges,
		out Int32 iIncomingEdges,
		out Int32 iOutgoingEdges
	)
	{
		Debug.Assert(oVertex != null);
		Debug.Assert(aoIncidentEdges != null);
		AssertValid();

		iIncomingEdges = 0;
		iOutgoingEdges = 0;

		foreach (IEdge oEdge in aoIncidentEdges)
		{
			if ( oVertex.IsIncomingEdge(oEdge) )
			{
				iIncomingEdges++;
			}

			if ( oVertex.IsOutgoingEdge(oEdge) )
			{
				iOutgoingEdges++;
			}
		}
	}

	//*************************************************************************
	//	Method: ListViewHasZeroRealItems
	//
	/// <summary>
	/// Determines whether the only item in a ListView is the "none" item.
	/// </summary>
	///
	/// <param name="oListView">
	/// ListView to test.
	/// </param>
	///
	/// <returns>
	/// true if the only item in <paramref name="oListView" /> is the "none"
	/// item.
	/// </returns>
	//*************************************************************************

	protected Boolean
	ListViewHasZeroRealItems
	(
		ListView oListView
	)
	{
		Debug.Assert(oListView != null);
		AssertValid();

		ListView.ListViewItemCollection oItems = oListView.Items;

		// The "none" item has a null tag.

		return (oItems.Count == 1 && oItems[0].Tag == null);
	}

	//*************************************************************************
	//	Method: FireRequestSelection()
	//
	/// <summary>
	///	Fires the <see cref="RequestSelection" /> event if appropriate.
	/// </summary>
	//*************************************************************************

    protected void
	FireRequestSelection()
    {
		AssertValid();

		this.UseWaitCursor = true;

		// Create an array of selected vertices.

		IVertex [] aoSelectedVertices;

		if ( ListViewHasZeroRealItems(lvwVertices) )
		{
			aoSelectedVertices = new IVertex[0];
		}
		else
		{
			ListView.SelectedListViewItemCollection oSelectedItems =
				lvwVertices.SelectedItems;

			aoSelectedVertices = new IVertex[oSelectedItems.Count];

			Int32 i = 0;

			// Note: Don't use the SelectedListViewItemCollection indexer here,
			// which is very slow.

			foreach (ListViewItem oItem in oSelectedItems)
			{
				Debug.Assert(oItem.Tag != null);
				Debug.Assert(oItem.Tag is IVertex);

				aoSelectedVertices[i] = (IVertex)oItem.Tag;

				i++;
			}
		}

		// Create an array of selected edges.

		IEdge [] aoSelectedEdges;

		if ( ListViewHasZeroRealItems(lvwEdges) )
		{
			aoSelectedEdges = new IEdge[0];
		}
		else
		{
			ListView.SelectedListViewItemCollection oSelectedItems =
				lvwEdges.SelectedItems;

			aoSelectedEdges = new IEdge[oSelectedItems.Count];

			Int32 i = 0;

			foreach (ListViewItem oItem in oSelectedItems)
			{
				Debug.Assert(oItem.Tag != null);
				Debug.Assert(oItem.Tag is IEdge);

				aoSelectedEdges[i] = (IEdge)oItem.Tag;

				i++;
			}
		}

		FireRequestSelection(aoSelectedVertices, aoSelectedEdges);

		this.UseWaitCursor = false;
    }

	//*************************************************************************
	//	Method: FireRequestSelection()
	//
	/// <summary>
	///	Fires the <see cref="RequestSelection" /> event if appropriate.
	/// </summary>
	///
	/// <param name="aoSelectedVertices">
	/// Selected vertices.  Can't be null.
	/// </param>
	///
	/// <param name="aoSelectedEdges">
	/// Selected edges.  Can't be null.
	/// </param>
	//*************************************************************************

    protected void
	FireRequestSelection
	(
		IVertex [] aoSelectedVertices,
		IEdge [] aoSelectedEdges
	)
    {
		Debug.Assert(aoSelectedVertices != null);
		Debug.Assert(aoSelectedEdges != null);
		AssertValid();

		RequestSelectionEventHandler oRequestSelectionEventHandler =
			this.RequestSelection;

		if (oRequestSelectionEventHandler != null)
		{
			m_bFiringRequestSelection = true;

			oRequestSelectionEventHandler(this,
				new RequestSelectionEventArgs(
					aoSelectedVertices, aoSelectedEdges)
				);

			m_bFiringRequestSelection = false;
		}
    }

	//*************************************************************************
	//	Method: OnSetSelection()
	//
	/// <summary>
	/// Performs tasks required when <see cref="SetSelection" /> is called.
	/// </summary>
	///
	/// <param name="aoSelectedVertices">
	/// Selected vertices.  Can't be null.
	/// </param>
	///
	/// <param name="aoSelectedEdges">
	/// Selected edges.  Can't be null.
	/// </param>
	//*************************************************************************

    protected void
	OnSetSelection
	(
		IVertex [] aoSelectedVertices,
		IEdge [] aoSelectedEdges
	)
    {
		Debug.Assert(aoSelectedVertices != null);
		Debug.Assert(aoSelectedEdges != null);
		AssertValid();

		SelectVerticesInVerticesListView(aoSelectedVertices);
		SelectEdgesInEdgesListView(aoSelectedEdges);
    }

	//*************************************************************************
	//	Method: OnSelectedIndexChanged()
	//
	/// <summary>
	/// Handles the SelectedIndexChanged event on the lvwVertices and lvwEdges
	/// ListViews.
	/// </summary>
	//*************************************************************************

    protected void
	OnSelectedIndexChanged()
    {
		AssertValid();

		if ( ListViewHasZeroRealItems(lvwVertices) )
		{
			return;
		}

		if (m_bSettingSelection)
		{
			// Items in the ListViews are being programatically selected.
			// Ignore the event.

			return;
		}

		if (m_oSelectedIndexChangedTimer != null)
		{
			// The timer is already running.  Ignore the event.

			return;
		}

		// In a multi-selection ListView, selecting items with the mouse and
		// keyboard results in multiple SelectedIndexChanged events.  For
		// example, if item A is currently selected and the user adds items
		// B, C, and D to the selection by shift-clicking on item D, three
		// events will fire -- one for each item added to the selection.  It's
		// one user action, but multiple events get fired.
		//
		// This would cause performance problems if each ListView event
		// resulted in a RequestSelection event being fired to the
		// NetMapControl.  To prevent this, fire the RequestSelection event from
		// a timer event, after all the ListView events have fired.  (Although
		// this isn't guaranteed to work, Windows gives low priority to timer
		// events and in testing they always followed the complete set of
		// ListView events.)

		m_oSelectedIndexChangedTimer = new Timer();
		m_oSelectedIndexChangedTimer.Interval = 1;

		m_oSelectedIndexChangedTimer.Tick +=

			delegate
			{
				m_oSelectedIndexChangedTimer.Enabled = false;
				m_oSelectedIndexChangedTimer = null;

				FireRequestSelection();
			};

        m_oSelectedIndexChangedTimer.Enabled = true;
    }

	//*************************************************************************
	//	Method: lvwVertices_SelectedIndexChanged()
	//
	/// <summary>
	/// Handles the SelectedIndexChanged event on the lvwVertices ListView.
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

    protected void
	lvwVertices_SelectedIndexChanged
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

		OnSelectedIndexChanged();
    }

	//*************************************************************************
	//	Method: lvwVertices_MouseUp()
	//
	/// <summary>
	/// Handles the MouseUp event on the lvwVertices ListView.
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

    protected void
	lvwVertices_MouseUp
	(
		object sender,
		MouseEventArgs e
	)
    {
		AssertValid();

		// Return if the user didn't right-click on a vertex.

		ListViewItem oItem = lvwVertices.GetRightClickedItem(e);

		if (oItem == null || oItem.Tag == null)
			return;

		Debug.Assert(oItem.Tag is IVertex);

		// Add menu items to the standard context menu supported by
		// ListViewPlus.

		ContextMenu oContextMenu = new ContextMenu();

		oContextMenu.MenuItems.AddRange( new MenuItem [] {

			new MenuItem(
				"&Select Incident Edges",
				new EventHandler(VerticesMenuSelectIncidentEdges_Click)
				),

			new MenuItem(
				"&Deselect Incident Edges",
				new EventHandler(VerticesMenuDeselectIncidentEdges_Click)
				),

			new MenuItem("-"),

			new MenuItem(
				"Select &Adjacent Vertices",
				new EventHandler(VerticesMenuSelectAdjacentVertices_Click)
				),

			new MenuItem(
				"Deselect Adjacent &Vertices",
				new EventHandler(VerticesMenuDeselectAdjacentVertices_Click)
				)
			} );

		lvwVertices.AddStandardContextMenuItems(oContextMenu, true);

		oContextMenu.Show( lvwVertices, new Point(e.X, e.Y) );
    }

	//*************************************************************************
	//	Method: VerticesMenuSelectIncidentEdges_Click()
	//
	/// <summary>
	/// Handles the Click event on the "Select Incident Edges" menu on the
	/// context menu for the lvwVertices ListView.
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

	protected void
	VerticesMenuSelectIncidentEdges_Click
	(
		object sender,
		EventArgs e
	)
	{
		AssertValid();

		SelectIncidentEdgesOfSelectedVertices(true);
	}

	//*************************************************************************
	//	Method: VerticesMenuDeselectIncidentEdges_Click()
	//
	/// <summary>
	/// Handles the Click event on the "Deselect Incident Edges" menu on the
	/// context menu for the lvwVertices ListView.
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

	protected void
	VerticesMenuDeselectIncidentEdges_Click
	(
		object sender,
		EventArgs e
	)
	{
		AssertValid();

		SelectIncidentEdgesOfSelectedVertices(false);
	}

	//*************************************************************************
	//	Method: VerticesMenuSelectAdjacentVertices_Click()
	//
	/// <summary>
	/// Handles the Click event on the "Select Adjacent Vertices" menu on the
	/// context menu for the lvwVertices ListView.
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

	protected void
	VerticesMenuSelectAdjacentVertices_Click
	(
		object sender,
		EventArgs e
	)
	{
		AssertValid();

		SelectAdjacentVerticesOfSelectedVertices(true);
	}

	//*************************************************************************
	//	Method: VerticesMenuDeselectAdjacentVertices_Click()
	//
	/// <summary>
	/// Handles the Click event on the "Deselect Adjacent Vertices" menu on the
	/// context menu for the lvwVertices ListView.
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

	protected void
	VerticesMenuDeselectAdjacentVertices_Click
	(
		object sender,
		EventArgs e
	)
	{
		AssertValid();

		SelectAdjacentVerticesOfSelectedVertices(false);
	}

	//*************************************************************************
	//	Method: lvwEdges_SelectedIndexChanged()
	//
	/// <summary>
	/// Handles the SelectedIndexChanged event on the lvwEdges ListView.
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

    protected void
	lvwEdges_SelectedIndexChanged
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

		OnSelectedIndexChanged();
    }

	//*************************************************************************
	//	Method: lvwEdges_MouseUp()
	//
	/// <summary>
	/// Handles the MouseUp event on the lvwEdges ListView.
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

    protected void
	lvwEdges_MouseUp
	(
		object sender,
		MouseEventArgs e
	)
    {
		AssertValid();

		// Return if the user didn't right-click on an edge.

		ListViewItem oItem = lvwEdges.GetRightClickedItem(e);

		if (oItem == null || oItem.Tag == null)
			return;

		Debug.Assert(oItem.Tag is IEdge);

		// Add menu items to the standard context menu supported by
		// ListViewPlus.

		ContextMenu oContextMenu = new ContextMenu();

		oContextMenu.MenuItems.AddRange( new MenuItem [] {

			new MenuItem(
				"Select &Adjacent Vertices",
				new EventHandler(EdgesMenuSelectAdjacentVertices_Click)
				),

			new MenuItem(
				"Deselect Adjacent &Vertices",
				new EventHandler(EdgesMenuDeselectAdjacentVertices_Click)
				)
			} );

		lvwEdges.AddStandardContextMenuItems(oContextMenu, true);

		oContextMenu.Show( lvwEdges, new Point(e.X, e.Y) );
    }

	//*************************************************************************
	//	Method: EdgesMenuSelectAdjacentVertices_Click()
	//
	/// <summary>
	/// Handles the Click event on the "Select Adjacent Vertices" menu on the
	/// context menu for the lvwEdges ListView.
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

	protected void
	EdgesMenuSelectAdjacentVertices_Click
	(
		object sender,
		EventArgs e
	)
	{
		AssertValid();

		SelectAdjacentVerticesOfSelectedEdges(true);
	}

	//*************************************************************************
	//	Method: EdgesMenuDeselectAdjacentVertices_Click()
	//
	/// <summary>
	/// Handles the Click event on the "Deselect Adjacent Vertices" menu on the
	/// context menu for the lvwEdges ListView.
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

	protected void
	EdgesMenuDeselectAdjacentVertices_Click
	(
		object sender,
		EventArgs e
	)
	{
		AssertValid();

		SelectAdjacentVerticesOfSelectedEdges(false);
	}

	//*************************************************************************
	//	Method: lnkDefinitions_LinkClicked()
	//
	/// <summary>
	/// Handles the LinkClicked event on the lnkDefinitions LinkLabel.
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

    protected void
	lnkDefinitions_LinkClicked
	(
		object sender,
		LinkLabelLinkClickedEventArgs e
	)
    {
		AssertValid();

		const String Message =

			"Definitions:\r\n\r\n"

			+ "Adjacent vertex: A vertex at the other side of an incident"
			+ " edge.\r\n\r\n"

			+ "Degree: The number of this vertex's incident edges.\r\n\r\n"

			+ "Incident edge: An edge connected to this vertex.\r\n\r\n"

			+ "In-degree: The number of this vertex's incoming edges.\r\n\r\n"

			+ "Incoming edge: A directed edge that has this vertex at its"
			+ " front, or an undirected edge connected to this vertex.\r\n\r\n"

			+ "Out-degree: The number of this vertex's outgoing edges.\r\n\r\n"

			+ "Outgoing edge: A directed edge that has this vertex at its"
			+ " back, or an undirected edge connected to this vertex.\r\n\r\n"

			+ "Predecessor vertex: A vertex at the other side of an incoming"
			+ " edge.\r\n\r\n"

			+ "Successor vertex: A vertex at the other side of an outgoing"
			+ " edge.\r\n\r\n"
			;

		FormUtil.ShowInformation(Message);
    }


	//*************************************************************************
	//	Method: AssertValid()
	//
	/// <summary>
	///	Asserts if the object is in an invalid state.  Debug-only.
	/// </summary>
	//*************************************************************************

	[Conditional("DEBUG")] 

	public void
	AssertValid()
	{
		// m_bUseHotKeys
		// m_bSettingSelection
		// m_bFiringRequestSelection
		// m_oSelectedIndexChangedTimer
		Debug.Assert(m_oGraphInfoViewerSettings != null);
	}


	//*************************************************************************
	//	Protected constants
	//*************************************************************************

	/// String format for Int32s.

	protected static readonly String Int32Format = NetMapBase.Int32Format;

	/// String format for Singles.

	protected static readonly String SingleFormat = NetMapBase.SingleFormat;

	/// Text to display in an empty list.

	protected static readonly String EmptyListText = "(none)";


	//*************************************************************************
	//	Protected fields
	//*************************************************************************

	/// true to use HotKeys.  The default is true.

	protected Boolean m_bUseHotKeys;

	/// true if items in the ListViews are being programatically selected.

	protected Boolean m_bSettingSelection;

	/// true while a RequestSelection event is in progress.

	protected Boolean m_bFiringRequestSelection;

	/// Timer used by SelectedIndexChanged(), or null if selected indexes
	/// aren't changing.

	protected Timer m_oSelectedIndexChangedTimer;

	/// User settings for this GraphInfoViewer.

	private GraphInfoViewerSettings m_oGraphInfoViewerSettings;
}

}
