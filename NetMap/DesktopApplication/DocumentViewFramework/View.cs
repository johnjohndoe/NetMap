
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.NetMap.Core;
using Microsoft.NetMap.Visualization;
using Microsoft.NetMap.ApplicationUtil;

namespace Microsoft.NetMap.DesktopApplication
{
//*****************************************************************************
//	Class: View
//
/// <summary>
/// Renders a <see cref="Document" /> in a window.
/// </summary>
///
/// <remarks>
/// For information on the application's document/view architecture, see
/// <see cref="Document" />.
///
/// </remarks>
//*****************************************************************************

public partial class View : DesktopApplicationForm
{
	//*************************************************************************
	//	Constructor: View()
	//
	/// <overloads>
	/// Initializes a new instance of the <see cref="View" /> class.
	/// </overloads>
	///
	/// <summary>
	/// Initializes a new instance of the <see cref="View" /> class with a
	/// default layout.
	/// </summary>
	///
	/// <remarks>
	/// This default constructor is meant for use by the Visual Studio designer
	/// only.
	/// </remarks>
	//*************************************************************************

	public View()
	:
	this( new LayoutManager() )
	{

		AssertValid();
	}

	//*************************************************************************
	//	Constructor: View()
	//
	/// <summary>
	/// Initializes a new instance of the <see cref="View" /> class with a
	/// specified layout.
	/// </summary>
	///
    /// <param name="layoutManager">
	/// Helper object for managing layouts.
    /// </param>
	//*************************************************************************

	public View
	(
		LayoutManager layoutManager
	)
	{
		Debug.Assert(layoutManager != null);

		m_oLayoutManager = layoutManager;

		m_oLayoutManager.LayoutChanged +=
			new EventHandler(this.LayoutManager_LayoutChanged);

		InitializeComponent();

		// Create a temporary document.  The owner will replace this with a
		// real document via the Document property.

		m_oDocument = new Document("Temporary");

		// Instantiate an object that saves and retrieves the user settings for
		// this View.  Note that the object automatically saves the settings
		// when the View closes.

		m_oViewSettings = new ViewSettings(this);

		// Instantiate an object that saves and retrieves the user's settings
		// for the graph in this View.  The save is NOT automatic.

		m_oGraphSettings = new GraphSettings();

		AssertValid();
	}

	//*************************************************************************
	//	Property: Document
	//
	/// <summary>
	/// Gets or sets the document associated with the view.
	/// </summary>
	///
	/// <value>
	/// The document associated with the view, or null if the document hasn't
	/// been set yet.
	/// </value>
	//*************************************************************************

	[System.ComponentModel.Browsable(false)]

	public Document
	Document
	{
		get
		{
			AssertValid();

			return (m_oDocument);
		}

		set
		{
			Debug.Assert(value != null);

			m_oDocument = value;

			m_oDocument.TitleChanged +=
				new EventHandler(this.Document_TitleChanged);

			// Set the text in the title bar.

			this.Text = m_oDocument.Title;

			// Assign the graph to the NetMapControl and specify the layout to
			// use.

			IGraph oGraph = m_oDocument.GraphData.Graph;

			oNetMapControl.BeginUpdate();

			oNetMapControl.Graph = oGraph;

			SetNetMapControlLayout();

			oNetMapControl.EndUpdate();

			// Assign the graph to the GraphInfoViewer.

			oGraphInfoViewer.Graph = oGraph;

			AssertValid();
		}
	}

    //*************************************************************************
    //  Property: IsDrawing
    //
    /// <summary>
	/// Gets a value indicating whether an asynchronous drawing operation is in
	/// progress.
    /// </summary>
    ///
    /// <value>
    /// true if an asynchronous drawing operation is in progress.
    /// </value>
    //*************************************************************************

    public Boolean
    IsDrawing
    {
        get
        {
            AssertValid();

			return (oNetMapControl.IsDrawing);
        }
    }

	//*************************************************************************
	//	Property: GraphSettings
	//
	/// <summary>
	/// Gets or sets the user's settings for the graph in this View.
	/// </summary>
	///
	/// <value>
	/// The user's graph settings for the graph in this view.
	/// </value>
	//*************************************************************************

	[System.ComponentModel.Browsable(false)]

	public GraphSettings
	GraphSettings
	{
		get
		{
			AssertValid();

			return (m_oGraphSettings);
		}

		set
		{
			Debug.Assert(value != null);

			m_oGraphSettings = value;

			ApplyGraphSettings(m_oGraphSettings);

			AssertValid();
		}
	}

	//*************************************************************************
	//	Method: SaveDocument()
	//
	/// <summary>
	///	Saves the document to a file.
	/// </summary>
	///
	/// <returns>
	/// true if the save was successful.
	/// </returns>
	///
	/// <remarks>
	/// If an error occurs, the error message is displayed in a message box and
	/// false is returned.
	/// </remarks>
	//*************************************************************************

	public Boolean
	SaveDocument()
	{
		AssertValid();

		Boolean bSuccess = true;

		if (m_oDocument.HasBeenSaved)
		{
			// The document has already been saved.  Save it again.

			MainForm.ShowWaitCursor = true;

			try
			{
				Debug.Assert( !String.IsNullOrEmpty(m_oDocument.FileName) );

				bSuccess = m_oDocument.Save(m_oDocument.FileName, true);
			}
			finally
			{
				MainForm.ShowWaitCursor = false;
			}
		}
		else
		{
			// The document has never been saved.

			bSuccess = SaveDocumentAs();
		}

		return (bSuccess);
	}

	//*************************************************************************
	//	Method: SaveDocumentAs()
	//
	/// <summary>
	/// Opens the Save File As dialog.
	/// </summary>
	///
	/// <returns>
	/// true if the save was successful.
	/// </returns>
	///
	/// <remarks>
	/// If an error occurs, the error message is displayed in a message box and
	/// false is returned.
	/// </remarks>
	//*************************************************************************

	public Boolean
	SaveDocumentAs()
	{
		AssertValid();

		if (m_oSaveDocumentFileDialog == null)
		{
			// Create a dialog for saving the document.

			m_oSaveDocumentFileDialog =	new SaveDocumentFileDialog();
		}

		// Allow the user to save the document.

		DialogResult eDialogResult =
			m_oSaveDocumentFileDialog.ShowDialogAndSaveDocument(m_oDocument);

		return (eDialogResult == DialogResult.OK);
	}

	//*************************************************************************
	//	Method: CopyGraphBitmap()
	//
	/// <summary>
	///	Copies the graph bitmap to the clipboard.
	/// </summary>
	///
	/// <remarks>
	/// Do not call this if <see cref="IsDrawing" /> is true.
	/// </remarks>
	//*************************************************************************

	public void
	CopyGraphBitmap()
	{
		AssertValid();
		Debug.Assert(!this.IsDrawing);

		// Get the size of the bitmap.

		Size oBitmapSize = oNetMapControl.ClientSize;

		if (oBitmapSize.Width == 0 || oBitmapSize.Height == 0)
		{
			// The size is unusable.

			this.ShowWarning(
				"The graph is too small to copy.  Make the graph window"
				+ " larger."
				);

			return;
		}

		// Tell the NetMapControl to copy its bitmap.

		Bitmap oBitmapCopy = oNetMapControl.CopyBitmap();

		// Copy the bitmap to the clipboard.

		Clipboard.SetDataObject(oBitmapCopy);

		// Note: Do not call oBitmapCopy.Dispose().
	}

	//*************************************************************************
	//	Method: RefreshLayout()
	//
	/// <summary>
	///	Lays out the graph again.
	/// </summary>
	///
	/// <remarks>
	/// Do not call this if <see cref="IsDrawing" /> is true.
	/// </remarks>
	//*************************************************************************

	public void
	RefreshLayout()
	{
		AssertValid();
		Debug.Assert(!this.IsDrawing);

		// Force the NetMapControl to lay out the graph again.

		oNetMapControl.ForceLayout();
	}

	//*************************************************************************
	//	Method: ApplyGraphSettings()
	//
	/// <summary>
	///	Applies the user's graph settings to the NetMapControl.
	/// </summary>
	///
    /// <param name="oGraphSettings">
	/// Graph settings to apply.
    /// </param>
	//*************************************************************************

	protected void
	ApplyGraphSettings
	(
		GraphSettings oGraphSettings
	)
	{
		Debug.Assert(oGraphSettings != null);
		AssertValid();

		oGraphSettings.TransferToNetMapControl(this.oNetMapControl);
	}

    //*************************************************************************
    //  Method: SetNetMapControlLayout()
    //
    /// <summary>
	/// Sets the layout used by the NetMapControl.
    /// </summary>
    ///
	/// <remarks>
	/// Important: The call to this method must be surrounded by
	/// NetMapControl.BeginUpdate() and NetMapControl.EndUpdate() to avoid
	/// unwanted graph redraws.
	/// </remarks>
    //*************************************************************************

    protected void
    SetNetMapControlLayout()
	{
		AssertValid();

        // Set the layout-related properties on the NetMapControl.

		m_oLayoutManager.ApplyLayoutToNetMapControl(
			oNetMapControl, m_oGraphSettings.Margin);

		// Apply the graph settings to the new layout, vertex drawer, and edge
		// drawer.

		ApplyGraphSettings(m_oGraphSettings);
    }

	//*************************************************************************
	//	Method: OnClosing()
	//
	/// <summary>
	///	Handles the Closing event.
	/// </summary>
	///
	/// <param name="e">
	/// Standard event argument.
	/// </param>
	//*************************************************************************

	protected override void
	OnClosing
	(
		System.ComponentModel.CancelEventArgs e
	)
	{
		AssertValid();

		// If the document isn't dirty, allow the view to be closed.

		if (m_oDocument.IsDirty)
		{
			switch (
		
				MessageBox.Show(

					String.Format(
						"{0} has been edited.  Do you want to save it?",
						m_oDocument.Title
						),

					ApplicationName,
					MessageBoxButtons.YesNoCancel,
					MessageBoxIcon.Warning
					)
				)
			{
				case DialogResult.Yes:

					if ( !SaveDocument() )
					{
						// The save failed.

						e.Cancel = true;
						return;
					}

					break;

				case DialogResult.No:

					// (Do nothing.)
					break;

				case DialogResult.Cancel:

					e.Cancel = true;
					return;

				default:

					Debug.Assert(false);
					break;
			}
		}

		Debug.Assert(!e.Cancel);
	}

	//*************************************************************************
	//	Method: Document_TitleChanged()
	//
	/// <summary>
	/// Handles the TitleChanged event on the document.
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
	Document_TitleChanged
	(
		object sender,
		System.EventArgs e
	)
	{
		AssertValid();

		this.Text = m_oDocument.Title;
	}

	//*************************************************************************
	//	Method: oNetMapControl_SelectionChanged()
	//
	/// <summary>
	/// Handles the SelectionChanged event on the oNetMapControl control.
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
	oNetMapControl_SelectionChanged
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

		// Inform the GraphInfoViewer that the selection has changed.

        oGraphInfoViewer.SetSelection(
			oNetMapControl.SelectedVertices, oNetMapControl.SelectedEdges
			);
    }

	//*************************************************************************
	//	Method: oNetMapControl_DrawingGraph()
	//
	/// <summary>
	/// Handles the DrawingGraph event on the oNetMapControl control.
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
	oNetMapControl_DrawingGraph
	(
		object sender,
		EventArgs e
	)
    {
		// To avoid having the user select a vertex in the GraphInfoViewer
		// while the graph is drawing, the GraphInfoViewer should be disabled.

		oGraphInfoViewer.Enabled = false;
    }

	//*************************************************************************
	//	Method: oNetMapControl_GraphDrawn()
	//
	/// <summary>
	/// Handles the GraphDrawn event on the oNetMapControl control.
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
	oNetMapControl_GraphDrawn
	(
		object sender,
		EventArgs e
	)
    {
		// To avoid having the user select a vertex in the GraphInfoViewer
		// while the graph is drawing, the GraphInfoViewer is disabled during
		// drawing.  Enable it now.

		oGraphInfoViewer.Enabled = true;
    }

	//*************************************************************************
	//	Method: oGraphInfoViewer_RequestSelection()
	//
	/// <summary>
	/// Handles the RequestSelection event on the oGraphInfoViewer control.
	/// </summary>
	///
	/// <param name="sender">
	/// Standard event argument.
	/// </param>
	///
	/// <param name="requestSelectionEventArgs">
	/// Standard event argument.
	/// </param>
	//*************************************************************************

    protected void
	oGraphInfoViewer_RequestSelection
	(
		object sender,
		RequestSelectionEventArgs requestSelectionEventArgs
	)
    {
		AssertValid();

		// Select the vertices and edges that were requested by the
		// GraphInfoViewer.

		oNetMapControl.SetSelected(requestSelectionEventArgs.Vertices,
			requestSelectionEventArgs.Edges);
    }

	//*************************************************************************
	//	Method: LayoutManager_LayoutChanged()
	//
	/// <summary>
	/// Handles the LayoutChanged event on the m_oLayoutManager object.
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
	LayoutManager_LayoutChanged
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

		oNetMapControl.BeginUpdate();

		SetNetMapControlLayout();

		oNetMapControl.EndUpdate();
    }


	//*************************************************************************
	//	Method: AssertValid()
	//
	/// <summary>
	///	Asserts if the object is in an invalid state.  Debug-only.
	/// </summary>
	//*************************************************************************

	// [Conditional("DEBUG")] 

	public override void
	AssertValid()
	{
		base.AssertValid();

		Debug.Assert(m_oDocument != null);
		// m_oSaveDocumentFileDialog
		Debug.Assert(m_oViewSettings != null);
		Debug.Assert(m_oGraphSettings != null);
		Debug.Assert(m_oLayoutManager != null);
	}


	//*************************************************************************
	//	Protected fields
	//*************************************************************************

	/// Document rendered by this view.

	protected Document m_oDocument;

	/// Dialog for saving a document, or null if the user hasn't saved a
	/// document yet.

	protected static SaveDocumentFileDialog m_oSaveDocumentFileDialog = null;

	/// User settings for this View.

	private ViewSettings m_oViewSettings;

	/// User settings for the graph in this View.

	protected GraphSettings m_oGraphSettings;

	/// Helper object for managing layouts.

	protected LayoutManager m_oLayoutManager;
}

}
