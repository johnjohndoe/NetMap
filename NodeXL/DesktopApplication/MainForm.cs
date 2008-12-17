
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.NodeXL.ApplicationUtil;
using Microsoft.NodeXL.Visualization;
using Microsoft.NodeXL.Common;

namespace Microsoft.NodeXL.DesktopApplication
{
//*****************************************************************************
//	Class: MainForm
//
/// <summary>
/// The application's main form.
/// </summary>
///
/// <remarks>
/// The main form is an MDI container for child windows of type <see
/// cref="View" />.
///
/// <para>
/// See <see cref="CommandLineParser" /> for the command line format.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class MainForm : DesktopApplicationForm
{
	//*************************************************************************
	//	Method: Main()
	//
	/// <summary>
	///	Applicaton's entry point.
	/// </summary>
	///
	/// <param name="asCommandLineArguments">
	/// Command line arguments.
	/// </param>
	//*************************************************************************

	public MainForm
	(
		String [] asCommandLineArguments
	)
	{
		Debug.Assert(asCommandLineArguments != null);
		Debug.Assert(m_oMainForm == null);

		m_oMainForm = this;

		// Instantiate an object that saves and retrieves the user's general
		// settings.  Note that the object automatically saves the settings
		// when this form closes.

		m_oGeneralUserSettings = new GeneralUserSettings(this);

		m_oOpenDocumentFileDialog = null;

		InitializeComponent();

		// Instantiate a helper object for managing layouts.

		m_oLayoutManagerForMenu = new LayoutManagerForMenu();

		m_oLayoutManagerForMenu.Layout = m_oGeneralUserSettings.Layout;
		m_oLayoutManagerForMenu.AddMenuItems(this.mniLayout);

		m_oLayoutManagerForMenu.LayoutChanged +=
			new EventHandler(this.LayoutManagerForMenu_LayoutChanged);

		this.ShowInTaskbar = true;

		this.Text = this.ApplicationName;

		ProcessCommandLineArguments(asCommandLineArguments);

		AssertValid();
	}

    //*************************************************************************
    //  Property: ShowWaitCursor
    //
    /// <summary>
    /// Changes the cursor to a wait cursor or the default cursor.
    /// </summary>
    ///
    /// <value>
	/// true to change the cursor to a wait cursor, false to change it to the
	/// default cursor.
    /// </value>
	///
	/// <remarks>
	/// This static property can be set from any place in the application.  Set
	/// it to true at the start of a long operation and false when the
	/// operation completes.
	/// </remarks>
    //*************************************************************************

    public static Boolean
	ShowWaitCursor
    {
        set
        {
			Debug.Assert(m_oMainForm != null);

			m_oMainForm.Cursor =
				(value ? Cursors.WaitCursor : Cursors.Default);
        }
    }

	//*************************************************************************
	//  Property: ActiveView
	//
	/// <summary>
	/// Gets the active view, if one exists.
	/// </summary>
	///
	/// <value>
	/// Returns the <see cref="View" /> that is the active MDI child window, or
	/// null if there are no child windows.
	/// </value>
	//*************************************************************************

	protected View
	ActiveView
	{
		get
		{
			AssertValid();

			Form oActiveForm = this.ActiveMdiChild;

			if (oActiveForm == null)
				return (null);

			Debug.Assert(oActiveForm is View);

			return ( (View)oActiveForm );
		}
	}

	//*************************************************************************
	//  Property: ActiveDocument
	//
	/// <summary>
	/// Gets the active document, if one exists.
	/// </summary>
	///
	/// <value>
	/// Returns the <see cref="Document" /> that belongs to the active view, or
	/// null if there are no child windows.
	/// </value>
	//*************************************************************************

	protected Document
	ActiveDocument
	{
		get
		{
			AssertValid();

			View oActiveView = this.ActiveView;

			if (oActiveView == null)
				return (null);

			return (oActiveView.Document);
		}
	}

    //*************************************************************************
    //  Property: DocumentExists
    //
    /// <summary>
	/// Gets a flag indicating if there is at least one document open.
    /// </summary>
    ///
    /// <value>
	/// true if there is at least one document open.
    /// </value>
    //*************************************************************************

    protected Boolean
	DocumentExists
    {
        get
        {
			AssertValid();

			return (this.ActiveDocument != null);
        }
    }

	//*************************************************************************
	//	Property: ActiveViewExistsAndIsNotDrawing
	//
	/// <summary>
	/// Gets a flag indicating whether an active view exists and an
	/// asynchronous drawing is not in progress in the active view.
	/// </summary>
	///
	/// <value>
	/// true if there is an active view and an asynchronous drawing is not in
	/// progress within the view.
	/// </value>
	//*************************************************************************

    protected Boolean
	ActiveViewExistsAndIsNotDrawing
    {
		get
		{
			AssertValid();

			View oActiveView = this.ActiveView;

			return (oActiveView != null && !oActiveView.IsDrawing);
		}
    }

	//*************************************************************************
	//	Property: AViewIsDrawing
	//
	/// <summary>
	/// Gets a flag indicating whether drawing is occurring in at least one
	/// view.
	/// </summary>
	///
	/// <value>
	/// true if at least one view is drawing.
	/// </value>
	//*************************************************************************

    protected Boolean
	AViewIsDrawing
    {
		get
		{
			AssertValid();

			foreach (Form oForm in this.MdiChildren)
			{
				if (oForm is View && ( (View)oForm ).IsDrawing)
				{
					return (true);
				}
			}

			return (false);
		}
    }

	//*************************************************************************
	//	Method: ProcessCommandLineArguments()
	//
	/// <summary>
	/// Processes the command line arguments.
	/// </summary>
	///
	/// <param name="asCommandLineArguments">
	/// Command line arguments.
	/// </param>
	//*************************************************************************

	protected void
	ProcessCommandLineArguments
	(
		String [] asCommandLineArguments
	)
	{
		Debug.Assert(asCommandLineArguments != null);

		// Parse the arguments.

		CommandLineParser oCommandLineParser =
			new CommandLineParser(asCommandLineArguments);

		String sErrorMessage;

		if ( !oCommandLineParser.Parse(out sErrorMessage) )
		{
			this.ShowWarning(sErrorMessage);

			return;
		}

		Document oDocument = oCommandLineParser.Document;

		if (oDocument != null)
		{
			// A document was created from the arguments.  Create a view,
			// connect the document to it, and show the document.

			CreateViewAndShowDocument(oDocument);
		}
	}

	//*************************************************************************
	//	Method: CreateViewAndShowDocument()
	//
	/// <summary>
	///	Creates a view, connects a document to it, and shows the document.
	/// </summary>
	///
	/// <param name="oDocument">
	/// Document to create a view for.
	/// </param>
	//*************************************************************************

	protected void
	CreateViewAndShowDocument
	(
		Document oDocument
	)
	{
		Debug.Assert(oDocument != null);
		AssertValid();

		// Create and show the view.

		View oView = new View(m_oLayoutManagerForMenu);

		Debug.Assert(this.IsMdiContainer);

		oView.MdiParent = this;
        oView.WindowState = FormWindowState.Maximized;
		oView.Show();

		// Attach the view to the document.

		oDocument.View = oView;

		// Attach the document to the view.  Doing this causes the view to
		// start drawing the graph.  Because the view's child window sizes
		// don't stabilize until after the view has been laid out and painted,
		// directly attaching the document to the view here would result in
		// multiple resizes and graph redraws.  To avoid this, attach the
		// document to the view via a timer, after the various paint-related
		// messages in the message queue have been processed.

		Timer oTimer = new Timer();
		oTimer.Interval = 1;
		oTimer.Enabled = true;

		oTimer.Tick += delegate(Object sender, EventArgs e)
		{
			oView.Document = oDocument;

			oTimer.Enabled = false;
			oTimer = null;
		};
	}

	//*************************************************************************
	//	Method: GetDocumentByFileName()
	//
	/// <summary>
	///	Returns the open document that has a specified file name.
	/// </summary>
	///
	/// <param name="sFileName">
	/// Full path of the file where the document was last saved.
	/// </param>
	///
	/// <returns>
	/// The open <see cref="Document" /> that was saved to the file <paramref
	/// name="sFileName" />, or null if no such document is open.
	/// </returns>
	//*************************************************************************

	protected Document
	GetDocumentByFileName
	(
		String sFileName
	)
	{
		Debug.Assert( !String.IsNullOrEmpty(sFileName) );
		AssertValid();

		foreach (Form oForm in this.MdiChildren)
		{
			if ( !(oForm is View) )
			{
				continue;
			}

			View oView = (View)oForm;
			Document oDocument = oView.Document;

			if (oDocument.FileName == sFileName)
				return (oDocument);
		}

		return (null);
	}

	//*************************************************************************
	//	Method: PopulateRecentDocumentsMenu()
	//
	/// <summary>
	/// Populates the mniFileRecentDocuments menu item with child menu items.
	/// </summary>
	//*************************************************************************

	protected void
	PopulateRecentDocumentsMenu()
	{
		AssertValid();

		// Start by clearing any child menu items.

		ToolStripItemCollection oRecentDocumentDropDownItems =
			mniFileRecentDocuments.DropDownItems;

		oRecentDocumentDropDownItems.Clear();

		// Get the most recently used document file names.

		String [] asRecentDocumentFileNames =
			m_oGeneralUserSettings.DocumentMruList.Array;

		Int32 iRecentDocumentFileNames = asRecentDocumentFileNames.Length;

		// Add a child menu item for each file name.

		for (Int32 i = 0; i < iRecentDocumentFileNames; i++)
		{
			String sRecentDocumentFileName = asRecentDocumentFileNames[i];

			ToolStripMenuItem oToolStripMenuItem = new ToolStripMenuItem(

				String.Format(
					"&{0}  {1}"
					,
					i + 1,
					sRecentDocumentFileName
					)
				);

			oToolStripMenuItem.Click +=
				new EventHandler(this.mniFileRecentDocument_Click);

			oToolStripMenuItem.Tag = sRecentDocumentFileName;

			oRecentDocumentDropDownItems.Add(oToolStripMenuItem);
		}

		// Enable the parent menu item only if it has at least one child.

		mniFileRecentDocuments.Enabled = (iRecentDocumentFileNames > 0);
	}

	//*************************************************************************
	//	Method: AddActiveDocumentToMruList()
	//
	/// <summary>
	///	Adds the file name for the active document to the list of recent
	/// documents.
	/// </summary>
	///
	/// <remarks>
	/// If the active document has not been saved, this method does nothing.
	///
	/// <para>
	/// Do not call this method if there is no active document.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	protected void
	AddActiveDocumentToMruList()
	{
		Debug.Assert(this.ActiveDocument != null);
		AssertValid();

		String sActiveDocumentFileName = this.ActiveDocument.FileName;

		if (sActiveDocumentFileName != null)
			AddDocumentToMruList(sActiveDocumentFileName);
	}

	//*************************************************************************
	//	Method: AddDocumentToMruList()
	//
	/// <summary>
	///	Adds a document file name to the list of recent documents.
	/// </summary>
	///
	/// <param name="sDocumentFileName">
	/// File name to add, including a full path.
	/// </param>
	//*************************************************************************

	protected void
	AddDocumentToMruList
	(
		String sDocumentFileName
	)
	{
		Debug.Assert( !String.IsNullOrEmpty(sDocumentFileName) );
		AssertValid();

		StringMruList oDocumentMruList =
            m_oGeneralUserSettings.DocumentMruList;

		// Remove it if it's already somewhere in the list, then add it to the
		// top of the list.

		oDocumentMruList.Remove(sDocumentFileName);
		oDocumentMruList.Add(sDocumentFileName);
	}

	//*************************************************************************
	//	Method: CopyGraphBitmap()
	//
	/// <summary>
	///	Copies the active view's graph bitmap to the clipboard, if possible.
	/// </summary>
	///
	/// <remarks>
	///	If an active view exists and an asynchronous drawing is not in progress
	/// in the active view, this method copies the active view's graph bitmap
	/// to the clipboard.
	/// </remarks>
	//*************************************************************************

	protected void
	CopyGraphBitmap()
	{
		AssertValid();

		if (this.ActiveViewExistsAndIsNotDrawing)
		{
			View oActiveView = this.ActiveView;

			Debug.Assert(oActiveView != null);
			Debug.Assert(!oActiveView.IsDrawing);

			oActiveView.CopyGraphBitmap();
		}
	}

	//*************************************************************************
	//	Method: ApplyGraphSettingsToAllViews()
	//
	/// <summary>
	/// Applies a <see cref="GraphSettings" /> object to all graphs.
	/// </summary>
	///
	/// <param name="oGraphSettings">
	/// Graph settings to apply.
	/// </param>
	//*************************************************************************

    protected void
	ApplyGraphSettingsToAllViews
	(
		GraphSettings oGraphSettings
	)
    {
		Debug.Assert(oGraphSettings != null);
		AssertValid();

		foreach (Form oForm in this.MdiChildren)
		{
			if (oForm is View)
			{
				View oView = (View)oForm;

				oView.GraphSettings = oGraphSettings;
			}
		}
    }

	//*************************************************************************
	//	Method: OnDropDownClosed()
	//
	/// <summary>
	/// Handles the DropDownClosed event on a top-level ToolStripMenuItem.
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
	OnDropDownClosed
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

		Debug.Assert(sender is ToolStripMenuItem);

		ToolStripMenuItem oToolStripMenuItem = (ToolStripMenuItem)sender;

		// A DropDownOpening handler for the menu item may have selectively
		// disabled some child menu items.  Enable them again to allow menu
		// item shortcuts to function properly.

		MenuUtil.EnableAllDescendentToolStripMenuItems(
			true, oToolStripMenuItem);
    }

	//*************************************************************************
	//	Method: OnClosing()
	//
	/// <summary>
	/// Handles the Closing event.
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
		// Hide all child MDI forms before closing.  This is to work around the
		// following bug that occurs if the child forms aren't hidden:
		//
		// 1. If an MDI child is maximized when the application is closed, it
		//    gets resized once before closing.  I don't know why this is.
		//
		// 2. The NodeXLControl.OnResize() method gets called.
		//
		// 3. NodeXLControl.OnResize() starts an asynchronous drawing operation.
		//
		// 4. AsyncLayoutBase fires a LayOutGraphIterationCompleted event
		//    some time after drawing starts.
		//
		// 5. Because the layout is asynchronous, the target of the event
		//    (NodeXLControl) may or may not still exist when the event fires.
		//
		// 6. If the target no longer exists, firing the event results in a
		//    NullReferenceException from within the event management code in
		//    the framework.
		//
		// Hiding the MDI form prevents it from getting resized before closing,
		// thus working around the bug.

        foreach (Form oForm in this.MdiChildren)
        {
            oForm.Hide();
        }
	}

	//*************************************************************************
	//	Method: mniFile_DropDownOpening()
	//
	/// <summary>
	/// Handles the DropDownOpening event on the mniFile menu item.
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
	mniFile_DropDownOpening
	(
		object sender,
		System.EventArgs e
	)
	{
		AssertValid();

		// The following commands should be enabled only if there is at least
		// one document open.  (Note that OnDropDownClosed() will reenable all
		// commands when the drop-down closes.)

		MenuUtil.EnableToolStripMenuItems(this.DocumentExists,
			mniFileSave,
			mniFileClose,
			mniFileSaveAs
			);

		// Populate the mniFileRecentDocuments menu item with child menu items.

		PopulateRecentDocumentsMenu();
	}

	//*************************************************************************
	//	Method: mniFileOpen_Click()
	//
	/// <summary>
	///	Handles the Click event on the mniFileOpen menu item.
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

	protected void
	mniFileOpen_Click
	(
		object sender,
		System.EventArgs e
	)
	{
		AssertValid();

		if (m_oOpenDocumentFileDialog == null)
		{
			// Create a dialog for opening a document.

			m_oOpenDocumentFileDialog =	new OpenDocumentFileDialog();
		}

		// Allow the user to open a document.

		Document oNewDocument;

		if (m_oOpenDocumentFileDialog.ShowDialogAndOpenDocumentFile(
			out oNewDocument) == DialogResult.OK)
		{
			// Is the document selected by the user already open?

			Document oAlreadyOpenDocument =
				GetDocumentByFileName(oNewDocument.FileName);

			if (oAlreadyOpenDocument == null)
			{
				// No.  Create a view, connect the document to it, and show the
				// document.

				CreateViewAndShowDocument(oNewDocument);

				AddDocumentToMruList(oNewDocument.FileName);
			}
			else
			{
				// Yes.  Discard the new document and just activate the
				// already-open document.

				oAlreadyOpenDocument.View.Activate();
			}
		}
	}

	//*************************************************************************
	//	Method: mniFileSave_Click()
	//
	/// <summary>
	///	Handles the Click event on the mniFileSave menu item.
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

	protected void
	mniFileSave_Click
	(
		object sender,
		System.EventArgs e
	)
	{
		AssertValid();

		View oActiveView = this.ActiveView;
		Debug.Assert(oActiveView != null);

		oActiveView.SaveDocument();

		AddActiveDocumentToMruList();
	}

	//*************************************************************************
	//	Method: mniFileSaveAs_Click()
	//
	/// <summary>
	///	Handles the Click event on the mniFileSaveAs menu item.
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

	protected void
	mniFileSaveAs_Click
	(
		object sender,
		System.EventArgs e
	)
	{
		AssertValid();

		View oActiveView = this.ActiveView;
		Debug.Assert(oActiveView != null);

		oActiveView.SaveDocumentAs();

		AddActiveDocumentToMruList();
	}

	//*************************************************************************
	//	Method: mniFileClose_Click()
	//
	/// <summary>
	///	Handles the Click event on the mniFileClose menu item.
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

	protected void
	mniFileClose_Click
	(
		object sender,
		System.EventArgs e
	)
	{
		AssertValid();

		View oActiveView = this.ActiveView;
		Debug.Assert(oActiveView != null);

		oActiveView.Close();
	}

	//*************************************************************************
	//	Method: mniFileRecentDocument_Click()
	//
	/// <summary>
	/// Handles the Click event on each of the menu items for recent documents.
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
	mniFileRecentDocument_Click
	(
		object sender,
		System.EventArgs e
	)
	{
		AssertValid();

		// PopulateRecentDocumentsMenu() set the menu item's Tag to a file
		// name.

		Debug.Assert(sender is ToolStripMenuItem);
		Debug.Assert( ( (ToolStripMenuItem)sender ).Tag is String );

		String sDocumentFileName = (String)( (ToolStripMenuItem)sender ).Tag;

		// Is the document selected by the user already open?

		Document oDocument = GetDocumentByFileName(sDocumentFileName);

		if (oDocument != null)
		{
			// Yes.  Just activate the already-open document.

			oDocument.View.Activate();

			return;
		}

		// No.  Create a document from the file.

		MainForm.ShowWaitCursor = true;

		Boolean bSuccess = false;

		try
		{
			bSuccess = Document.Load(sDocumentFileName, true, out oDocument);
		}
		finally
		{
			MainForm.ShowWaitCursor = false;
		}

		if (bSuccess)
		{
			// Create a view, connect the document to it, and show the
			// document.

			CreateViewAndShowDocument(oDocument);

			AddDocumentToMruList(sDocumentFileName);
		}
	}

	//*************************************************************************
	//	Method: mniFileExit_Click()
	//
	/// <summary>
	/// Handles the Click event on the mniFileExit menu item.
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
	mniFileExit_Click
	(
		object sender,
		System.EventArgs e
	)
	{
		AssertValid();

		this.Close();
	}

	//*************************************************************************
	//	Method: mniEdit_DropDownOpening()
	//
	/// <summary>
	/// Handles the DropDownOpening event on the mniEdit menu item.
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
	mniEdit_DropDownOpening
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

		// The mniEditCopy menu item should be enabled only if an
		// asynchronous drawing is not in progress in the active view. (Note
		// that OnDropDownClosed() will reenable all commands when the
		// drop-down closes.)

		MenuUtil.EnableToolStripMenuItems(
			this.ActiveViewExistsAndIsNotDrawing,
			mniEditCopy
			);
    }

	//*************************************************************************
	//	Method: mniEditCopy_Click()
	//
	/// <summary>
	/// Handles the Click event on the mniEditCopy menu item.
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
	mniEditCopy_Click
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

		CopyGraphBitmap();
    }

	//*************************************************************************
	//	Method: mniLayout_DropDownOpening()
	//
	/// <summary>
	/// Handles the DropDownOpening event on the mniLayout menu item.
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
	mniLayout_DropDownOpening
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

		// Enable or disable the layout menu items.

		m_oLayoutManagerForMenu.EnableMenuItems(!this.AViewIsDrawing);

		// Enable or disable the Refresh menu item.

        mniLayoutRefresh.Enabled = this.ActiveViewExistsAndIsNotDrawing;
    }

	//*************************************************************************
	//	Method: mniLayoutRefresh_Click()
	//
	/// <summary>
	/// Handles the Click event on the mniLayoutRefresh menu item.
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
	mniLayoutRefresh_Click
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

		if (this.ActiveViewExistsAndIsNotDrawing)
		{
			this.ActiveView.RefreshLayout();
		}
	}

	//*************************************************************************
	//	Method: mniTools_DropDownOpening()
	//
	/// <summary>
	/// Handles the DropDownOpening event on the mniTools menu item.
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
	mniTools_DropDownOpening
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

		// The following commands should be enabled only if no drawing is in
		// progress.  (Note that OnDropDownClosed() will reenable all commands
		// when the drop-down closes.)

		MenuUtil.EnableToolStripMenuItems(!this.AViewIsDrawing,
			mniToolsOptions
			);
    }

	//*************************************************************************
	//	Method: mniToolsOptions_Click()
	//
	/// <summary>
	/// Handles the Click event on the mniToolsOptions menu item.
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

    protected void mniToolsOptions_Click
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

		if (this.AViewIsDrawing)
		{
			return;
		}

		View oActiveView = this.ActiveView;

		GraphSettings oGraphSettings;

		if (oActiveView != null)
		{
			// Start with the active view's graph settings.

			oGraphSettings = oActiveView.GraphSettings;
		}
		else
		{
			// Start with the most recently saved graph settings.

			oGraphSettings = new GraphSettings();
		}

		// Allow the user to edit the graph settings.

		GraphSettingsDialog oDialog = new GraphSettingsDialog(
			oGraphSettings, (oActiveView != null) );

		if (oDialog.ShowDialog() == DialogResult.Cancel)
		{
			return;
		}

		MainForm.ShowWaitCursor = true;

		Boolean bSaveGraphSettings = true;

		if (oDialog.ApplyToAllGraphs)
		{
			// The user wants to apply her edits to all graphs.  Pass the graph
			// settings to all views.

			ApplyGraphSettingsToAllViews(oGraphSettings);
		}
		else if (oActiveView != null)
		{
			// The user wants to apply her edits to the active graph only.
			// Don't save the graph settings.

			oActiveView.GraphSettings = oGraphSettings;

			bSaveGraphSettings = false;
		}
		else
		{
			// There is no active graph.  Just save the graph settings.
		}

		if (bSaveGraphSettings)
		{
			oGraphSettings.Save();
		}

		MainForm.ShowWaitCursor = false;
    }

	//*************************************************************************
	//	Method: mniWindow_DropDownOpening()
	//
	/// <summary>
	/// Handles the DropDownOpening event on the mniWindow menu item.
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
	mniWindow_DropDownOpening
	(
		object sender,
		System.EventArgs e
	)
	{
		// The MDI layout commands should be enabled only if there is at least
		// one document open.  (Note that OnDropDownClosed() will reenable all
		// commands when the drop-down closes.)

		MenuUtil.EnableToolStripMenuItems(this.DocumentExists,
			mniWindowCascade,
			mniWindowTileHorizontally,
			mniWindowTileVertically,
			mniWindowCloseAll
			);

		// Windows Forms does not refresh the window list text in the
		// MdiWindowListItem when Form.Text changes.  This leads to the
		// following bug:
		//
		// 1. Open a graph file.
		//
		// 2. Click the Window menu.
		//
		// 3. Result: The window list contains "View" instead of the name of
		//    the file that was opened.
		//
		// Use the following workaround, which was suggested at
		// http://forums.microsoft.com/MSDN/ShowPost.aspx?PostID=377528&SiteID=1

		View oActiveView = this.ActiveView;

		if (oActiveView != null)
		{
			this.ActivateMdiChild(null);
			this.ActivateMdiChild(oActiveView);
		}
	}

	//*************************************************************************
	//	Method: mniWindowArrange_Click()
	//
	/// <summary>
	/// Handles the Click event on each of the child menu items of mniWindow
	/// that corresponds to an MDI layout.
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
	mniWindowArrange_Click
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

		Debug.Assert(sender is ToolStripMenuItem);

		ToolStripMenuItem oToolStripMenuItem = (ToolStripMenuItem)sender;

		// Each child menu item that corresponds to an MDI layout has its Tag
		// set to an MdiLayout value. 

		Debug.Assert(oToolStripMenuItem.Tag is MdiLayout);

		this.LayoutMdi( (MdiLayout)oToolStripMenuItem.Tag );
    }

	//*************************************************************************
	//	Method: mniWindowCloseAll_Click()
	//
	/// <summary>
	/// Handles the Click event on the mniWindowCloseAll menu item.
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
	mniWindowCloseAll_Click
	(
		object sender,
		System.EventArgs e
	)
	{
		AssertValid();

		foreach (Form oForm in this.MdiChildren)
		{
			oForm.Close();
		}
	}

	//*************************************************************************
	//	Method: mniHelpContents_Click()
	//
	/// <summary>
	///	Handles the Click event on the mniHelpContents menu item.
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
	mniHelpContents_Click
	(
		object sender,
		System.EventArgs e
	)
	{
		Help.ShowHelp(this, String.Format(
			"{0}\\{1}",
			Application.StartupPath,
			HelpFileName
			) );
	}

	//*************************************************************************
	//	Method: mniHelpAbout_Click()
	//
	/// <summary>
	///	Handles the Click event on the mniHelpAbout menu item.
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

    protected void
	mniHelpAbout_Click
	(
		object sender,
		EventArgs e
	)
    {
		String sApplicationName = this.ApplicationName;

		String sText = String.Format(

			"{0}\r\n\r\n"

			+ "Version {1}\r\n\r\n"

			+ "Created by the Community Technologies team at Microsoft"
			+ " Research\r\n\r\n"

			+ "Copyright (c) {2} Microsoft Corporation"
			,
			sApplicationName,
			AssemblyUtil2.GetFileVersion(),
			DateTime.Now.Year
			);

		MessageBox.Show(sText, sApplicationName);
    }

	//*************************************************************************
	//	Method: LayoutManagerForMenu_LayoutChanged()
	//
	/// <summary>
	/// Handles the LayoutChanged event on the m_oLayoutManagerForMenu object.
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
	LayoutManagerForMenu_LayoutChanged
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

		// Save the new layout.

		m_oGeneralUserSettings.Layout = m_oLayoutManagerForMenu.Layout;

		m_oGeneralUserSettings.Save();

		// Note: Each View object also handles this event and updates its
		// NodeXLControl in its event handler.
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

		// m_oMainForm
		Debug.Assert(m_oGeneralUserSettings != null);
		// m_oOpenDocumentFileDialog
		Debug.Assert(m_oLayoutManagerForMenu != null);
	}


	//*************************************************************************
	//	Protected constants
	//*************************************************************************

	/// Root for the title assigned to new documents.

	protected const String TitleRoot = "Graph";

	/// Path to the application's help file, relative to the application's
	/// executable after the application is installed on the user's machine.

	protected static readonly String HelpFileName =
		"Documents\\Help\\NodeXLDesktopApplication.chm";



	//*************************************************************************
	//	Protected fields
	//*************************************************************************

	/// The application's main form.

	protected static MainForm m_oMainForm;

	/// General user settings.

	protected GeneralUserSettings m_oGeneralUserSettings;

	/// Dialog for opening a document, or null if the user hasn't opened a
	/// document yet.

	protected OpenDocumentFileDialog m_oOpenDocumentFileDialog;

	/// Helper object for managing layouts.

	protected LayoutManagerForMenu m_oLayoutManagerForMenu;
}

}
