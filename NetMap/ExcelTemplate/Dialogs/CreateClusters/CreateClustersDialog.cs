

//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//	Class: CreateClustersDialog
//
/// <summary>
/// Partitions the graph into clusters.
/// </summary>
///
/// <remarks>
/// <see cref="GraphMetricCalculationManager" />, <see cref="ClusterMapper" />,
/// and <see cref="GraphMetricWriter" /> objects do most of the work.  The
/// clustering is done asynchronously, so it doesn't hang the UI and can be
/// cancelled by the user.  Writing the results to the worksheet is done
/// synchronously, however, and can't be cancelled.
/// </remarks>
//*****************************************************************************

public partial class CreateClustersDialog : ExcelTemplateForm
{
	//*************************************************************************
	//	Constructor: CreateClustersDialog()
	//
	/// <overloads>
	///	Initializes a new instance of the <see cref="CreateClustersDialog" />
	/// class.
	/// </overloads>
	///
	/// <summary>
	///	Initializes a new instance of the <see cref="CreateClustersDialog" />
	/// class with graph metric arguments.
	/// </summary>
	///
	/// <param name="workbook">
    /// Workbook containing the graph contents.
	/// </param>
	//*************************************************************************

	public CreateClustersDialog
	(
        Microsoft.Office.Interop.Excel.Workbook workbook
	)
	: this()
	{
		// Instantiate an object that saves and retrieves the position of this
		// dialog.  Note that the object automatically saves the settings when
		// the form closes.

		m_oCreateClustersDialogUserSettings =
			new CreateClustersDialogUserSettings(this);

		m_oWorkbook = workbook;

		m_oGraphMetricCalculationManager = new GraphMetricCalculationManager();

		m_oGraphMetricCalculationManager.GraphMetricCalculationProgressChanged
			+= new ProgressChangedEventHandler(
		GraphMetricCalculationManager_GraphMetricCalculationProgressChanged);

		m_oGraphMetricCalculationManager.GraphMetricCalculationCompleted +=
			new RunWorkerCompletedEventHandler(
				GraphMetricCalculationManager_GraphMetricCalculationCompleted);

		DoDataExchange(false);

		AssertValid();
	}

	//*************************************************************************
	//	Constructor: CreateClustersDialog()
	//
	/// <summary>
	///	Initializes a new instance of the <see cref="CreateClustersDialog" />
	/// class for the Visual Studio designer.
	/// </summary>
	///
	/// <remarks>
	/// Do not use this constructor.  It is for use by the Visual Studio
	/// designer only.
	/// </remarks>
	//*************************************************************************

	public CreateClustersDialog()
	{
		InitializeComponent();

		// AssertValid();
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
			// (Do nothing.)
		}
		else
		{
			// (Do nothing.)
		}

		return (true);
	}

	//*************************************************************************
	//	Method: WriteGraphMetricColumnsToWorkbook()
	//
	/// <summary>
	///	Writes an array of GraphMetricColumn objects to the workbook.
	/// </summary>
	///
	/// <param name="aoGraphMetricColumns">
	/// An array of GraphMetricColumn objects, one for each column of metrics
	/// that were calculated.
	/// </param>
	//*************************************************************************

	protected void
	WriteGraphMetricColumnsToWorkbook
	(
		GraphMetricColumn [] aoGraphMetricColumns
	)
	{
		Debug.Assert(aoGraphMetricColumns != null);
		AssertValid();

		GraphMetricWriter oGraphMetricWriter = new GraphMetricWriter();

		oGraphMetricWriter.WriteGraphMetricColumnsToWorkbook(
			aoGraphMetricColumns, m_oWorkbook);
	}

	//*************************************************************************
	//	Method: MapClusters()
	//
	/// <summary>
	/// Maps vertex clusters in the cluster and cluster vertices worksheets to
	/// vertex attribute columns in the vertex worksheet.
	/// </summary>
	//*************************************************************************

	protected void
	MapClusters()
	{
		AssertValid();

		ClusterMapper oClusterMapper = new ClusterMapper();

		oClusterMapper.MapClusters(m_oWorkbook);
	}

	//*************************************************************************
	//	Method: OnLoad()
	//
	/// <summary>
	///	Handles the OnLoad event.
	/// </summary>
	///
	/// <param name="e">
	///	Standard event arguments.
	/// </param>
	//*************************************************************************

	protected override void
	OnLoad
	(
		EventArgs e
	)
	{
		AssertValid();

		base.OnLoad(e);

		// Start the cluster calculations.  Note that this uses the graph
		// metric infrastructure, because calculating clusters follows exactly
		// the same pattern as calculating graph metrics.  In this case, the
		// metrics being calculated are the cluster columns on the cluster and
		// cluster vertices worksheets.

		GraphMetricUserSettings oGraphMetricUserSettings =
			new GraphMetricUserSettings();

		IGraphMetricCalculator [] aoGraphMetricCalculators =
			new IGraphMetricCalculator[] { new ClusterCalculator() };

		try
		{
			m_oGraphMetricCalculationManager.CalculateGraphMetricsAsync(
				m_oWorkbook, aoGraphMetricCalculators, oGraphMetricUserSettings
				);
		}
		catch (Exception oException)
		{
			// An exception was thrown from the synchronous code within
			// CalculateGraphMetricsAsync().  (Exceptions thrown from the
			// asynchronous code are handled by the
			// GraphMetricCalculationCompleted event handler.)

			ErrorUtil.OnException(oException);

			this.Close();
		}
	}

	//*************************************************************************
	//	Method: OnGraphMetricCalculationsComplete()
	//
	/// <summary>
	/// Performs tasks required after graph metrics have been calculated.
	/// </summary>
	///
	/// <param name="aoGraphMetricColumns">
	/// An array of GraphMetricColumn objects, one for each column of metrics
	/// that were calculated.
	/// </param>
	//*************************************************************************

	protected void
	OnGraphMetricCalculationsComplete
	(
		GraphMetricColumn [] aoGraphMetricColumns
	)
	{
		Debug.Assert(aoGraphMetricColumns != null);
		AssertValid();

		Microsoft.Office.Interop.Excel.Application oApplication =
			m_oWorkbook.Application;

		oApplication.ScreenUpdating = false;

		try
		{
			// Write the array of GraphMetricColumn objects to the workbook.
			// The objects contain the vertex clusters.

			WriteGraphMetricColumnsToWorkbook(aoGraphMetricColumns);

			// Map the vertex clusters to attribute columns on the vertex
			// worksheet.

			MapClusters();
		}
		catch (Exception oException)
		{
			oApplication.ScreenUpdating = true;

			ErrorUtil.OnException(oException);

			this.Close();
			return;
		}

		oApplication.ScreenUpdating = true;
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

		if (m_oGraphMetricCalculationManager.IsBusy)
		{
			// Let the background thread cancel its task, but don't try to
			// notify this dialog.

			m_oGraphMetricCalculationManager.
				GraphMetricCalculationProgressChanged -=
				new ProgressChangedEventHandler(
		GraphMetricCalculationManager_GraphMetricCalculationProgressChanged);

			m_oGraphMetricCalculationManager.GraphMetricCalculationCompleted -=
			new RunWorkerCompletedEventHandler(
				GraphMetricCalculationManager_GraphMetricCalculationCompleted);

			m_oGraphMetricCalculationManager.CancelAsync();
		}
	}

    //*************************************************************************
    //  Method: GraphMetricCalculationManager_
	//          GraphMetricCalculationProgressChanged()
    //
    /// <summary>
	/// Handles the GraphMetricCalculationProgressChanged event on the
	/// GraphMetricCalculationManager object.
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
	GraphMetricCalculationManager_GraphMetricCalculationProgressChanged
	(
		object sender,
		ProgressChangedEventArgs e
	)
	{
		Debug.Assert(e.UserState is GraphMetricProgress);
		AssertValid();

		GraphMetricProgress oGraphMetricProgress =
			(GraphMetricProgress)e.UserState;

		lblStatus.Text = oGraphMetricProgress.ProgressMessage;
        pbProgress.Value = e.ProgressPercentage;

		if (oGraphMetricProgress.AllGraphMetricsCalculated)
		{
			// Writing to the workbook will occur shortly within the
			// GraphMetricCalculationCompleted handler.  Writing can't be
			// cancelled, so disable the cancel button.

			btnCancel.Enabled = false;
		}
	}

    //*************************************************************************
    //  Method: GraphMetricCalculationManager_GraphMetricCalculationCompleted()
    //
    /// <summary>
	/// Handles the GraphMetricCalculationCompleted event on the
	/// GraphMetricCalculationManager
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
	GraphMetricCalculationManager_GraphMetricCalculationCompleted
	(
		object sender,
		RunWorkerCompletedEventArgs e
	)
	{
		AssertValid();

		Exception oException = e.Error;

		if (oException != null)
		{
			ErrorUtil.OnException(oException);
		}
		else if (!e.Cancelled)
		{
			Debug.Assert( e.Result is GraphMetricColumn[] );

			OnGraphMetricCalculationsComplete(
				( GraphMetricColumn[] )e.Result );
		}

		this.Close();
	}

    //*************************************************************************
    //  Method: btnCancel_Click()
    //
    /// <summary>
	/// Handles the Clicke event on the btnCancel button.
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
	btnCancel_Click
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

		// Close the dialog, which will cancel the calculations.

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

		Debug.Assert(m_oCreateClustersDialogUserSettings != null);
		Debug.Assert(m_oWorkbook != null);
		Debug.Assert(m_oGraphMetricCalculationManager != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// User settings for this dialog.

	protected CreateClustersDialogUserSettings
		m_oCreateClustersDialogUserSettings;

    /// Workbook containing the graph contents.

	protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

	/// Object that does most of the work.

	protected GraphMetricCalculationManager m_oGraphMetricCalculationManager;
}


//*****************************************************************************
//  Class: CreateClustersDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="CreateClustersDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("CreateClustersDialog") ]

public class CreateClustersDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: CreateClustersDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="CreateClustersDialogUserSettings" /> class.
    /// </summary>
	///
	/// <param name="oForm">
	/// The form to save settings for.
	/// </param>
    //*************************************************************************

    public CreateClustersDialogUserSettings
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
