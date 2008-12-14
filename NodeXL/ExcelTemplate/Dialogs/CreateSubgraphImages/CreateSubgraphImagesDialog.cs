

//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//	Class: CreateSubgraphImagesDialog
//
/// <summary>
/// Dialog that creates a subgraph image for each of a graph's vertices.
/// </summary>
///
/// <remarks>
/// Call <see cref="Form.ShowDialog()" /> to run the dialog.  All image
/// creation is performed within the dialog.
///
/// <para>
/// A <see cref="SubgraphImageCreator" /> object does most of the work.  The
/// image creation is done asynchronously, so it doesn't hang the UI and can be
/// cancelled by the user.  However, the optional insertion of thumbnail images
/// into the vertex worksheet is done synchronously, because you can't update
/// the UI in Windows from a background thread.  During thumbnail insertion,
/// all dialog controls are disabled and the UI doesn't respond to user input.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class CreateSubgraphImagesDialog : ExcelTemplateForm
{
	//*************************************************************************
	//	Constructor: CreateSubgraphImagesDialog()
	//
	/// <overloads>
	///	Initializes a new instance of the <see
	/// cref="CreateSubgraphImagesDialog" /> class.
	/// </overloads>
	///
	/// <summary>
	///	Initializes a new instance of the <see
	/// cref="CreateSubgraphImagesDialog" /> class with a workbook.
	/// </summary>
	///
    /// <param name="workbook">
	/// Workbook containing the graph data.
    /// </param>
	///
    /// <param name="selectedVertexNames">
	/// Array of zero or more vertex names corresponding to the selected rows
	/// in the vertex worksheet.  Can't be null.
    /// </param>
	//*************************************************************************

	public CreateSubgraphImagesDialog
	(
        Microsoft.Office.Interop.Excel.Workbook workbook,
		String [] selectedVertexNames
	)
	: this()
	{
		Debug.Assert(workbook != null);
		Debug.Assert(selectedVertexNames != null);

		// Instantiate an object that saves and retrieves the user settings for
		// this dialog.  Note that the object automatically saves the settings
		// when the form closes.

		m_oCreateSubgraphImagesDialogUserSettings =
			new CreateSubgraphImagesDialogUserSettings(this);

		m_oWorkbook = workbook;
		m_asSelectedVertexNames = selectedVertexNames;

		m_oSubgraphImageCreator = new SubgraphImageCreator();

		m_oSubgraphImageCreator.ImageCreationProgressChanged +=
			new ProgressChangedEventHandler(
				SubgraphImageCreator_ImageCreationProgressChanged);

		m_oSubgraphImageCreator.ImageCreationCompleted +=
			new RunWorkerCompletedEventHandler(
				SubgraphImageCreator_ImageCreationCompleted);

		m_eState = DialogState.Idle;

		SaveableImageFormats.InitializeListControl(cbxImageFormat);

		DoDataExchange(false);

		AssertValid();
	}

	//*************************************************************************
	//	Constructor: CreateSubgraphImagesDialog()
	//
	/// <summary>
	///	Initializes a new instance of the <see
	/// cref="CreateSubgraphImagesDialog" /> class for the Visual Studio
	/// designer.
	/// </summary>
	///
	/// <remarks>
	/// Do not use this constructor.  It is for use by the Visual Studio
	/// designer only.
	/// </remarks>
	//*************************************************************************

	public CreateSubgraphImagesDialog()
	{
		InitializeComponent();

		// AssertValid();
	}

	//*************************************************************************
	//  Enum: DialogState
	//
	/// <summary>
	/// Indicates the state of the dialog.
	/// </summary>
	//*************************************************************************

	protected enum
	DialogState
	{
		/// Idle, waiting for user action.

		Idle,

		/// Creating subgraph images.

		CreatingSubgraphImages,

		/// Populating the subgraph image column on the vertex worksheet.

		PopulatingImageColumn,
	}

    //*************************************************************************
    //  Property: State
    //
    /// <summary>
    /// Gets or sets the state of the dialog.
    /// </summary>
    ///
    /// <value>
    /// The state of the dialog.
    /// </value>
    //*************************************************************************

    protected DialogState
    State
    {
        get
        {
            AssertValid();

			return (m_eState);
        }

        set
        {
            m_eState = value;

			EnableControls();

            AssertValid();
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
			Boolean bSaveToFolder = chkSaveToFolder.Checked;
			String sFolder = null;
			Int32 iImageWidthPx = Int32.MinValue;
			Int32 iImageHeightPx = Int32.MinValue;

			if (bSaveToFolder)
			{
				if (
					!ValidateDirectoryTextBox(txbFolder,
						"Enter or browse for an existing folder where the"
						+ " subgraph images will be saved.",
						out sFolder)
					||
					!ValidateNumericUpDown(nudImageWidthPx, "image width",
						out iImageWidthPx)
					||
					!ValidateNumericUpDown(nudImageHeightPx, "image height",
                        out iImageHeightPx)
					)
				{
					return (false);
				}
			}

			Boolean bInsertThumbnails = chkInsertThumbnails.Checked;
			Int32 iThumbnailWidthPx = Int32.MinValue;
			Int32 iThumbnailHeightPx = Int32.MinValue;

			if (bInsertThumbnails)
			{
				if (
					!ValidateNumericUpDown(nudThumbnailWidthPx,
						"thumbnail width", out iThumbnailWidthPx)
					||
					!ValidateNumericUpDown(nudThumbnailHeightPx,
						"thumbnail height", out iThumbnailHeightPx)
					)
				{
					return (false);
				}
			}

			// All data is now valid.

			if (bSaveToFolder)
			{
				m_oCreateSubgraphImagesDialogUserSettings.Folder = sFolder;

				m_oCreateSubgraphImagesDialogUserSettings.ImageSizePx =
					new Size(iImageWidthPx, iImageHeightPx);

				m_oCreateSubgraphImagesDialogUserSettings.ImageFormat =
					(ImageFormat)cbxImageFormat.SelectedValue;
			}

			if (bInsertThumbnails)
			{
				m_oCreateSubgraphImagesDialogUserSettings.ThumbnailSizePx =
					new Size(iThumbnailWidthPx, iThumbnailHeightPx);
			}

			m_oCreateSubgraphImagesDialogUserSettings.Levels =
				usrSubgraphLevels.Levels;

			m_oCreateSubgraphImagesDialogUserSettings.SaveToFolder =
				bSaveToFolder;

			m_oCreateSubgraphImagesDialogUserSettings.InsertThumbnails =
				bInsertThumbnails;

			m_oCreateSubgraphImagesDialogUserSettings.SelectedVerticesOnly =
				chkSelectedVerticesOnly.Checked;

			m_oCreateSubgraphImagesDialogUserSettings.SelectVertex =
				chkSelectVertex.Checked;

			m_oCreateSubgraphImagesDialogUserSettings.SelectIncidentEdges =
				chkSelectIncidentEdges.Checked;
		}
		else
		{
			usrSubgraphLevels.Levels =
				m_oCreateSubgraphImagesDialogUserSettings.Levels;

			chkSaveToFolder.Checked =
				m_oCreateSubgraphImagesDialogUserSettings.SaveToFolder;

			txbFolder.Text = m_oCreateSubgraphImagesDialogUserSettings.Folder;

			Size oImageSizePx =
				m_oCreateSubgraphImagesDialogUserSettings.ImageSizePx;

			nudImageWidthPx.Value = oImageSizePx.Width;
			nudImageHeightPx.Value = oImageSizePx.Height;

			cbxImageFormat.SelectedValue = 
				m_oCreateSubgraphImagesDialogUserSettings.ImageFormat;

			chkInsertThumbnails.Checked =
				m_oCreateSubgraphImagesDialogUserSettings.InsertThumbnails;

			Size oThumbnailSizePx =
				m_oCreateSubgraphImagesDialogUserSettings.ThumbnailSizePx;

			nudThumbnailWidthPx.Value = oThumbnailSizePx.Width;
			nudThumbnailHeightPx.Value = oThumbnailSizePx.Height;

			chkSelectedVerticesOnly.Checked =
				m_oCreateSubgraphImagesDialogUserSettings.SelectedVerticesOnly;

			chkSelectVertex.Checked =
				m_oCreateSubgraphImagesDialogUserSettings.SelectVertex;

			chkSelectIncidentEdges.Checked =
				m_oCreateSubgraphImagesDialogUserSettings.SelectIncidentEdges;

			EnableControls();
		}

		return (true);
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

		const String StopText = "Stop";

		switch (this.State)
		{
			case DialogState.Idle:

				pnlDisableWhileCreating.Enabled = true;
				btnCreate.Enabled = true;
				btnCreate.Text = "Create";
				btnClose.Enabled = true;
				this.UseWaitCursor = false;

				pnlSaveToFolder.Enabled = chkSaveToFolder.Checked;
				grpThumbnailSize.Enabled = chkInsertThumbnails.Checked;

				if (m_asSelectedVertexNames.Length > 0)
				{
					chkSelectedVerticesOnly.Enabled = true;
				}
				else
				{
					chkSelectedVerticesOnly.Checked = false;
					chkSelectedVerticesOnly.Enabled = false;
				}

				break;

			case DialogState.CreatingSubgraphImages:

				pnlDisableWhileCreating.Enabled = false;
				btnCreate.Enabled = true;
				btnCreate.Text = StopText;
				btnClose.Enabled = true;
				this.UseWaitCursor = true;

				break;

			case DialogState.PopulatingImageColumn:

				pnlDisableWhileCreating.Enabled = false;
				btnCreate.Enabled = false;
				btnCreate.Text = StopText;
				btnClose.Enabled = false;
				this.UseWaitCursor = true;

				break;

			default:

				Debug.Assert(false);
				break;
		}
	}

    //*************************************************************************
    //  Method: StartImageCreation()
    //
    /// <summary>
	/// Starts the creation of subgraph images.
    /// </summary>
	///
	/// <remarks>
	/// It's assumed that m_oCreateSubgraphImagesDialogUserSettings contains
	/// valid settings.
	/// </remarks>
    //*************************************************************************

    protected void
	StartImageCreation()
    {
		AssertValid();

		// Read the workbook into a new IGraph.

		IGraph oGraph;

		try
		{
			oGraph = ReadWorkbook(m_oWorkbook);
		}
		catch (Exception oException)
		{
			ErrorUtil.OnException(oException);

			return;
		}

		lblStatus.Text = "Creating subgraph images.";

		IVertex [] aoSelectedVertices = new IVertex[0];

		if (m_oCreateSubgraphImagesDialogUserSettings.SelectedVerticesOnly)
		{
			// Get the vertices corresponding to the selected rows in the
			// vertex worksheet.

			aoSelectedVertices =
				GetSelectedVertices(oGraph, m_asSelectedVertexNames);
		}

		m_oSubgraphImageCreator.CreateSubgraphImagesAsync(
			oGraph,
			aoSelectedVertices,
			m_oCreateSubgraphImagesDialogUserSettings.Levels,
			m_oCreateSubgraphImagesDialogUserSettings.SaveToFolder,
			m_oCreateSubgraphImagesDialogUserSettings.Folder,
			m_oCreateSubgraphImagesDialogUserSettings.ImageSizePx,
			m_oCreateSubgraphImagesDialogUserSettings.ImageFormat,
			m_oCreateSubgraphImagesDialogUserSettings.InsertThumbnails,
			m_oCreateSubgraphImagesDialogUserSettings.ThumbnailSizePx,
			m_oCreateSubgraphImagesDialogUserSettings.SelectedVerticesOnly,
			m_oCreateSubgraphImagesDialogUserSettings.SelectVertex,
			m_oCreateSubgraphImagesDialogUserSettings.SelectIncidentEdges,
			new GeneralUserSettings()
			);
	}

    //*************************************************************************
    //  Method: ReadWorkbook()
    //
    /// <summary>
	/// Reads a workbook into a new graph.
    /// </summary>
	///
	/// <param name="oWorkbook">
	/// The workbook to read.
	/// </param>
	///
	/// <returns>
	/// The new graph.
	/// </returns>
    //*************************************************************************

	protected IGraph
	ReadWorkbook
	(
		Microsoft.Office.Interop.Excel.Workbook oWorkbook
	)
	{
		Debug.Assert(oWorkbook != null);
		AssertValid();

		ReadWorkbookContext oReadWorkbookContext = new ReadWorkbookContext();

		WorkbookReader oWorkbookReader = new WorkbookReader();

		// Convert the workbook contents to a Graph object.

		return ( oWorkbookReader.ReadWorkbook(
			oWorkbook, oReadWorkbookContext) );
	}

    //*************************************************************************
    //  Method: GetSelectedVertices()
    //
    /// <summary>
	/// Gets the vertices corresponding to the selected rows in the vertex
	/// worksheet.
    /// </summary>
    ///
	/// <param name="oGraph">
	/// Graph created from the workbook.
	/// </param>
	///
    /// <param name="asSelectedVertexNames">
	/// Array of zero or more vertex names corresponding to the selected rows
	/// in the vertex worksheet.  Can't be null.
    /// </param>
	///
	/// <returns>
	/// Array of vertices in <paramref name="oGraph" /> corresponding to the
	/// vertex names in <paramref name="asSelectedVertexNames" />.
	/// </returns>
    //*************************************************************************

	protected IVertex []
	GetSelectedVertices
	(
		IGraph oGraph,
		String [] asSelectedVertexNames
	)
	{
		Debug.Assert(oGraph != null);
		Debug.Assert(asSelectedVertexNames != null);
		AssertValid();

		List<IVertex> oSelectedVertices = new List<IVertex>();

		// Store the selected vertex names in a dictionary for quick lookup.
		// The key is the vertex name and the value isn't used.

		Dictionary<String, Char> oSelectedVertexNames =
			new Dictionary<String, Char>();

		foreach (String sSelectedVertexName in asSelectedVertexNames)
		{
			Debug.Assert( !String.IsNullOrEmpty(sSelectedVertexName) );

			oSelectedVertexNames[sSelectedVertexName] = ' ';
		}

		// Loop through the graph's vertices, looking for vertex names that are
		// in the dictionary.

		foreach (IVertex oVertex in oGraph.Vertices)
		{
			Debug.Assert( !String.IsNullOrEmpty(oVertex.Name) );

			if (oSelectedVertexNames.ContainsKey(oVertex.Name) )
			{
				oSelectedVertices.Add(oVertex);
			}
		}

		return ( oSelectedVertices.ToArray() );
	}

    //*************************************************************************
    //  Method: OnImageCreationCompleted()
    //
    /// <summary>
	/// Handles the ImageCreationCompleted event on the SubgraphImageCreator
	/// object.
    /// </summary>
    ///
	/// <param name="e">
	/// Standard event argument.
	/// </param>
    //*************************************************************************

	protected void
	OnImageCreationCompleted
	(
		RunWorkerCompletedEventArgs e
	)
	{
		AssertValid();

        if (e.Cancelled)
        {
			this.State = DialogState.Idle;

			lblStatus.Text = "Image creation stopped.";
        }
        else if (e.Error != null)
        {
			this.State = DialogState.Idle;

			Exception oException = e.Error;

			if (oException is System.IO.IOException)
			{
				lblStatus.Text = "Image creation error.";

				this.ShowWarning(oException.Message);
			}
			else
			{
				ErrorUtil.OnException(oException);
			}
        }
        else
        {
			// Success.  Were temporary images created that need to be inserted
			// into the vertex worksheet?

			Debug.Assert(e.Result is TemporaryImages);

			TemporaryImages oTemporaryImages = (TemporaryImages)e.Result;

			if (oTemporaryImages.Folder != null)
			{
				// Yes.  Insert them, then delete the temporary images.

				this.State = DialogState.PopulatingImageColumn;

				String sLastStatusFromSubgraphImageCreator = lblStatus.Text;

				lblStatus.Text =
					"Inserting subgraph thumbnails into the worksheet.  Please"
					+ " wait...";

				TableImagePopulator.PopulateColumnWithImages(m_oWorkbook,
					WorksheetNames.Vertices, TableNames.Vertices,
					VertexTableColumnNames.SubgraphImage, 2,
					VertexTableColumnNames.VertexName,
					oTemporaryImages
					);

				lblStatus.Text = sLastStatusFromSubgraphImageCreator;
			}

			this.State = DialogState.Idle;
        }
	}

    //*************************************************************************
    //  Method: OnClosed()
    //
    /// <summary>
	/// Handles the Closed event.
    /// </summary>
    ///
	/// <param name="e">
	/// Standard event argument.
	/// </param>
    //*************************************************************************

	protected override void
	OnClosed
	(
		EventArgs e
	)
	{
		AssertValid();

		if (m_oSubgraphImageCreator.IsBusy)
		{
			// Let the background thread cancel its task, but don't try to
			// notify this dialog.

			m_oSubgraphImageCreator.ImageCreationProgressChanged -=
				new ProgressChangedEventHandler(
					SubgraphImageCreator_ImageCreationProgressChanged);

			m_oSubgraphImageCreator.ImageCreationCompleted -=
				new RunWorkerCompletedEventHandler(
					SubgraphImageCreator_ImageCreationCompleted);

			m_oSubgraphImageCreator.CancelAsync();
		}
	}

    //*************************************************************************
    //  Method: OnEventThatRequiresControlEnabling()
    //
    /// <summary>
	/// Handles any event that should changed the enabled state of the dialog's
	/// controls.
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
	OnEventThatRequiresControlEnabling
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

		EnableControls();
    }

	//*************************************************************************
	//	Method: btnBrowse_Click()
	//
	/// <summary>
	///	Handles the Click event on the btnBrowse button.
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
	btnBrowse_Click
	(
		object sender,
		System.EventArgs e
	)
	{
		// Show the folder browser dialog.

		FolderBrowserDialog oFolderBrowserDialog =
			new FolderBrowserDialog();

		oFolderBrowserDialog.Description =
			"Browse for the folder where the subgraph image files will be"
			+ " saved.";

		oFolderBrowserDialog.SelectedPath = txbFolder.Text;

		if (oFolderBrowserDialog.ShowDialog() == DialogResult.OK)
		{
			txbFolder.Text = oFolderBrowserDialog.SelectedPath;
		}
	}

    //*************************************************************************
    //  Method: lnkHelp_LinkClicked()
    //
    /// <summary>
	/// Handles the LinkClicked event on the lnkHelp LinkLabel.
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
	lnkHelp_LinkClicked
	(
		object sender,
		LinkLabelLinkClickedEventArgs e
	)
    {
        AssertValid();

		const String HelpMessage =

		"Use this feature to create a subgraph for each vertex in the graph,"
		+ " and to save the subgraphs as image files in a folder.  You"
		+ " can also insert subgraph thumbnails into the {0} worksheet."
		+ "\r\n\r\n"
		+ "The level-1.0 subgraph for a vertex consists of the vertex, its"
		+ " adjacent vertices, and the edges that connect the vertex to its"
		+ " adjacent vertices.  All other vertices and edges in the graph are"
		+ " discarded.  A level-1.5 subgraph adds any edges connecting the"
		+ " adjacent vertices to each other.  A level-2.0 subgraph adds the"
		+ " vertices adjacent to the adjacent vertices, and so on.  You can"
		+ " include up to 4.5 levels of adjacent vertices in the subgraphs."
		+ "\r\n\r\n"
		+ "If you save the subgraph images to a folder, each image file is"
		+ " named after the vertex.  The JPEG image file for a vertex named"
		+ " \"Vertex 123\" will be \"Vertex 123.jpg,\" for example.  If a"
		+ " vertex name includes characters that aren't valid in file names,"
		+ " those characters get replaced with hexadecimal representations."
		+ "  The JPEG image file for a vertex named \"A\\B\" will be"
		+ " \"A%5CB.jpg,\" for example.  (The backslash, which is not valid in"
		+ " file names, gets replaced with %5C, its hexadecimal"
		+ " representation.)"
		+ "\r\n\r\n"
		+ "If you want to create subgraph images for certain vertices only,"
		+ " select those vertices in the {0} worksheet before opening the"
		+ " Create Subgraph Images dialog box, then check the \"Create"
		+ " subgraph images for selected vertices only\" checkbox."
		+ "\r\n\r\n"
		+ "The \"{1}\", \"{2}\" and \"{3}\" columns on the {0} worksheet are"
		+ " ignored when the subgraph images are created."
		;

        this.ShowInformation( String.Format(
			HelpMessage
			,
			WorksheetNames.Vertices,
			VertexTableColumnNames.X,
			VertexTableColumnNames.Y,
			VertexTableColumnNames.Locked
			) );
    }

    //*************************************************************************
    //  Method: btnCreate_Click()
    //
    /// <summary>
	/// Handles the Click event on the btnCreate button.
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
	btnCreate_Click
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

		switch (this.State)
		{
			case DialogState.Idle:

				if (!m_oSubgraphImageCreator.IsBusy)
				{
					if ( DoDataExchange(true) )
					{
						this.State = DialogState.CreatingSubgraphImages;

						StartImageCreation();
					}
				}

				break;

			case DialogState.CreatingSubgraphImages:

				if (m_oSubgraphImageCreator.IsBusy)
				{
					// Request to cancel image creation.  When the request is
					// completed, SubgraphImageCreator_ImageCreationCompleted()
					// will be called.

					m_oSubgraphImageCreator.CancelAsync();
				}

				break;

			case DialogState.PopulatingImageColumn:

				// (Do nothing.)

				break;

			default:

				Debug.Assert(false);
				break;
		}
    }

    //*************************************************************************
    //  Method: SubgraphImageCreator_ImageCreationProgressChanged()
    //
    /// <summary>
	/// Handles the ImageCreationProgressChanged event on the
	/// SubgraphImageCreator object.
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
	SubgraphImageCreator_ImageCreationProgressChanged
	(
		object sender,
		ProgressChangedEventArgs e
	)
	{
		Debug.Assert(e.UserState is String);
		AssertValid();

		lblStatus.Text = (String)e.UserState;
	}

    //*************************************************************************
    //  Method: SubgraphImageCreator_ImageCreationCompleted()
    //
    /// <summary>
	/// Handles the ImageCreationCompleted event on the SubgraphImageCreator
	/// object.
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
	SubgraphImageCreator_ImageCreationCompleted
	(
		object sender,
		RunWorkerCompletedEventArgs e
	)
	{
		AssertValid();

		OnImageCreationCompleted(e);
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

		Debug.Assert(m_oCreateSubgraphImagesDialogUserSettings != null);
		Debug.Assert(m_oWorkbook != null);
		Debug.Assert(m_asSelectedVertexNames != null);
		Debug.Assert(m_oSubgraphImageCreator != null);
		// m_eState
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// User settings for this dialog.

	protected CreateSubgraphImagesDialogUserSettings
		m_oCreateSubgraphImagesDialogUserSettings;

	/// Workbook containing the graph data.

	protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

	/// Array of zero or more vertex names corresponding to the selected rows
	/// in the vertex worksheet.

	protected String [] m_asSelectedVertexNames;

	/// Object that does most of the work.

	protected SubgraphImageCreator m_oSubgraphImageCreator;

	/// Indicates the state of the dialog.

	protected DialogState m_eState;
}

}
