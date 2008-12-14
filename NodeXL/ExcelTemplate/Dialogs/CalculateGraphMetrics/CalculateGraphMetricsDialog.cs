

//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//	Class: CalculateGraphMetricsDialog
//
/// <summary>
/// Calculates one or more sets of graph metrics and stores the result in the
/// workbook.
/// </summary>
///
/// <remarks>
/// <see cref="GraphMetricCalculationManager" /> and <see
/// cref="GraphMetricWriter" /> objects do most of the work.  The calculations
/// are done asynchronously, so they don't hang the UI and can be cancelled by
/// the user.  Writing the results to the worksheet is done synchronously,
/// however, and can't be cancelled.
///
/// <para>
/// If graph metrics are successfully calculated, <see
/// cref="Form.ShowDialog()" /> returns DialogResult.OK.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class CalculateGraphMetricsDialog : ExcelTemplateForm
{
	//*************************************************************************
	//	Constructor: CalculateGraphMetricsDialog()
	//
	/// <overloads>
	///	Initializes a new instance of the <see
	/// cref="CalculateGraphMetricsDialog" /> class.
	/// </overloads>
	///
	/// <summary>
	///	Initializes a new instance of the <see
	/// cref="CalculateGraphMetricsDialog" /> class with a specified list of
	/// graph metric calculators.
	/// </summary>
	///
	/// <param name="workbook">
    /// Workbook containing the graph contents.
	/// </param>
	///
	/// <param name="graphMetricCalculators">
	/// An array of <see cref="IGraphMetricCalculator" /> implementations, one
	/// for each set of graph metrics that should be calculated.
	/// </param>
	///
	/// <param name="graphMetricUserSettings">
	/// User settings for calculating graph metrics.
	/// </param>
	///
	/// <param name="notificationUserSettings">
	/// User settings for notifications.
	/// </param>
	///
	/// <param name="ignoreDuplicateEdges">
	/// true to ignore duplicate edges in the graph.
	/// </param>
	///
	/// <param name="dialogTitle">
	/// Title for the dialog.
	/// </param>
	//*************************************************************************

	public CalculateGraphMetricsDialog
	(
        Microsoft.Office.Interop.Excel.Workbook workbook,
		IGraphMetricCalculator [] graphMetricCalculators,
		GraphMetricUserSettings graphMetricUserSettings,
		NotificationUserSettings notificationUserSettings,
		Boolean ignoreDuplicateEdges,
		String dialogTitle
	)
	: this(workbook, graphMetricUserSettings, notificationUserSettings)
	{
		Debug.Assert(graphMetricCalculators != null);
		Debug.Assert( !String.IsNullOrEmpty(dialogTitle) );

		m_oGraphMetricCalculators = graphMetricCalculators;
		m_bIgnoreDuplicateEdges = ignoreDuplicateEdges;
		this.Text = dialogTitle;

		AssertValid();
	}

	//*************************************************************************
	//	Constructor: CalculateGraphMetricsDialog()
	//
	/// <summary>
	///	Initializes a new instance of the <see
	/// cref="CalculateGraphMetricsDialog" /> class with a default list of
	/// graph metric calculators.
	/// </summary>
	///
	/// <param name="workbook">
    /// Workbook containing the graph contents.
	/// </param>
	///
	/// <param name="graphMetricUserSettings">
	/// User settings for calculating graph metrics.
	/// </param>
	///
	/// <param name="notificationUserSettings">
	/// User settings for notifications.
	/// </param>
	//*************************************************************************

	public CalculateGraphMetricsDialog
	(
        Microsoft.Office.Interop.Excel.Workbook workbook,
		GraphMetricUserSettings graphMetricUserSettings,
		NotificationUserSettings notificationUserSettings
	)
	: this()
	{
		// Instantiate an object that saves and retrieves the position of this
		// dialog.  Note that the object automatically saves the settings when
		// the form closes.

		m_oCalculateGraphMetricsDialogUserSettings =
			new CalculateGraphMetricsDialogUserSettings(this);

		m_oWorkbook = workbook;
		m_oGraphMetricCalculators = null;
		m_bIgnoreDuplicateEdges = false;
		m_oGraphMetricUserSettings = graphMetricUserSettings;
		m_oNotificationUserSettings = notificationUserSettings;

		m_oGraphMetricCalculationManager = new GraphMetricCalculationManager();

		m_oGraphMetricCalculationManager.DuplicateEdgeDetected +=
			new CancelEventHandler(
				GraphMetricCalculationManager_DuplicateEdgeDetected);

		m_oGraphMetricCalculationManager.GraphMetricCalculationProgressChanged
			+= new ProgressChangedEventHandler(
		GraphMetricCalculationManager_GraphMetricCalculationProgressChanged);

		m_oGraphMetricCalculationManager.GraphMetricCalculationCompleted +=
			new RunWorkerCompletedEventHandler(
				GraphMetricCalculationManager_GraphMetricCalculationCompleted);

		DoDataExchange(false);

		// Assume that calculations will not succeed.

		this.DialogResult = DialogResult.Cancel;

		AssertValid();
	}

	//*************************************************************************
	//	Constructor: CalculateGraphMetricsDialog()
	//
	/// <summary>
	///	Initializes a new instance of the <see
	/// cref="CalculateGraphMetricsDialog" /> class for the Visual Studio
	/// designer.
	/// </summary>
	///
	/// <remarks>
	/// Do not use this constructor.  It is for use by the Visual Studio
	/// designer only.
	/// </remarks>
	//*************************************************************************

	public CalculateGraphMetricsDialog()
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

		Microsoft.Office.Interop.Excel.Application oApplication =
			m_oWorkbook.Application;

		GraphMetricWriter oGraphMetricWriter = new GraphMetricWriter();

		oApplication.ScreenUpdating = false;

		try
		{
			oGraphMetricWriter.WriteGraphMetricColumnsToWorkbook(
				aoGraphMetricColumns, m_oWorkbook);
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

		// Start the calculations.

		try
		{
			if (m_oGraphMetricCalculators != null)
			{
				// Use the specified graph metric calculators.

				m_oGraphMetricCalculationManager.CalculateGraphMetricsAsync(
					m_oWorkbook, m_oGraphMetricCalculators,
					m_oGraphMetricUserSettings);
			}
			else
			{
				// Use a default list of graph metric calculators.

				m_oGraphMetricCalculationManager.CalculateGraphMetricsAsync(
					m_oWorkbook, m_oGraphMetricUserSettings);
			}
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
    //  Method: GraphMetricCalculationManager_DuplicateEdgeDetected()
    //
    /// <summary>
	/// Handles the DuplicateEdgeDetected event on the
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
	GraphMetricCalculationManager_DuplicateEdgeDetected
	(
		object sender,
		CancelEventArgs e
	)
	{
		AssertValid();

		if (m_bIgnoreDuplicateEdges ||
			!m_oNotificationUserSettings.GraphHasDuplicateEdge)
		{
			return;
		}

		const String Message =
			"The workbook contains duplicate edges that will cause some of"
			+ " the graph metrics to be inaccurate.  Do you want to calculate"
			+ " graph metrics anyway?"
			+ "\r\n\r\n"
			+ "If you answer Yes, the inaccurate metrics will be highlighted"
			+ " with Excel's \"Bad\" style, which is usually red."
			;

		NotificationDialog oNotificationDialog = new NotificationDialog(
			"Duplicate Edges", SystemIcons.Warning, Message);

		if (oNotificationDialog.ShowDialog() != DialogResult.Yes)
		{
			e.Cancel = true;
			this.Close();
		}

		if (oNotificationDialog.DisableFutureNotifications)
		{
			m_oNotificationUserSettings.GraphHasDuplicateEdge = false;
			m_oNotificationUserSettings.Save();
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

			WriteGraphMetricColumnsToWorkbook(
				( GraphMetricColumn[] )e.Result );

			// Everything succeeded.

			this.DialogResult = DialogResult.OK;
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

		Debug.Assert(m_oCalculateGraphMetricsDialogUserSettings != null);
		Debug.Assert(m_oWorkbook != null);
		// m_oGraphMetricCalculators
		// m_bIgnoreDuplicateEdges
		Debug.Assert(m_oGraphMetricUserSettings != null);
		Debug.Assert(m_oNotificationUserSettings != null);
		Debug.Assert(m_oGraphMetricCalculationManager != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// User settings for this dialog.

	protected CalculateGraphMetricsDialogUserSettings
		m_oCalculateGraphMetricsDialogUserSettings;

    /// Workbook containing the graph contents.

	protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

	/// An array of IGraphMetricCalculator implementations, one
	/// for each set of graph metrics that should be calculated, or null to use
	/// a default list of implementations.

	protected IGraphMetricCalculator [] m_oGraphMetricCalculators;

	/// true to ignore duplicate edges in the graph.

	protected Boolean m_bIgnoreDuplicateEdges;

	/// User settings for calculating graph metrics.

	protected GraphMetricUserSettings m_oGraphMetricUserSettings;

	/// User settings for notifications.

	protected NotificationUserSettings m_oNotificationUserSettings;

	/// Object that does most of the work.

	protected GraphMetricCalculationManager m_oGraphMetricCalculationManager;
}


//*****************************************************************************
//  Class: CalculateGraphMetricsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="CalculateGraphMetricsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("CalculateGraphMetricsDialog") ]

public class CalculateGraphMetricsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: CalculateGraphMetricsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="CalculateGraphMetricsDialogUserSettings" /> class.
    /// </summary>
	///
	/// <param name="oForm">
	/// The form to save settings for.
	/// </param>
    //*************************************************************************

    public CalculateGraphMetricsDialogUserSettings
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
