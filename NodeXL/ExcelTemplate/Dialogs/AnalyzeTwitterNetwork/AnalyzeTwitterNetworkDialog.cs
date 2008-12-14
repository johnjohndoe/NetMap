

//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Reflection;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.SocialNetworkLib;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//	Class: AnalyzeTwitterNetworkDialog
//
/// <summary>
/// Dialog that analyzes a Twitter network and writes the results to the edge
/// and vertex worksheets.
/// </summary>
///
/// <remarks>
/// A <see cref="TwitterNetworkAnalyzer" /> object does most of the work.  The
/// analysis is done asynchronously, so it doesn't hang the UI and can be
/// cancelled by the user.
/// </remarks>
//*****************************************************************************

public partial class AnalyzeTwitterNetworkDialog : ExcelTemplateForm
{
	//*************************************************************************
	//	Constructor: AnalyzeTwitterNetworkDialog()
	//
	/// <overloads>
	///	Initializes a new instance of the <see
	/// cref="AnalyzeTwitterNetworkDialog" /> class.
	/// </overloads>
	///
	/// <summary>
	///	Initializes a new instance of the <see
	/// cref="AnalyzeTwitterNetworkDialog" /> class with a workbook.
	/// </summary>
	///
    /// <param name="workbook">
	/// Workbook containing the graph data.
    /// </param>
	//*************************************************************************

	public AnalyzeTwitterNetworkDialog
	(
        Microsoft.Office.Interop.Excel.Workbook workbook
	)
	: this()
	{
		// Instantiate an object that saves and retrieves the user settings for
		// this dialog.  Note that the object automatically saves the settings
		// when the form closes.

		m_oAnalyzeTwitterNetworkDialogUserSettings =
			new AnalyzeTwitterNetworkDialogUserSettings(this);

		m_sCredentialsPassword = String.Empty;

		m_oWorkbook = workbook;

		m_oTwitterNetworkAnalyzer = new TwitterNetworkAnalyzer();

		m_oTwitterNetworkAnalyzer.HttpWebRequestTimeoutMs =
			HttpWebRequestTimeoutMs;

		m_oTwitterNetworkAnalyzer.AnalysisCompleted +=
			new RunWorkerCompletedEventHandler(
				TwitterNetworkAnalyzer_AnalysisCompleted);

		m_oEdgeTable = null;

		DoDataExchange(false);

		AssertValid();
	}

	//*************************************************************************
	//	Constructor: AnalyzeTwitterNetworkDialog()
	//
	/// <summary>
	///	Initializes a new instance of the <see
	/// cref="AnalyzeTwitterNetworkDialog" /> class for the Visual Studio
	/// designer.
	/// </summary>
	///
	/// <remarks>
	/// Do not use this constructor.  It is for use by the Visual Studio
	/// designer only.
	/// </remarks>
	//*************************************************************************

	public AnalyzeTwitterNetworkDialog()
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
			// Validate the controls.

			String sScreenNameToAnalyze;

			if ( !ValidateRequiredTextBox(txbScreenNameToAnalyze,
				"Enter the screen name of a Twitter user.",
				out sScreenNameToAnalyze) )
			{
				return (false);
			}

			String sCredentialsScreenName =
				txbCredentialsScreenName.Text.Trim();

			if ( !String.IsNullOrEmpty(sCredentialsScreenName) )
			{
				txbCredentialsScreenName.Text = sCredentialsScreenName;

				if ( !ValidateRequiredTextBox(txbCredentialsPassword,

					"If you want to use your Twitter account information, you"
					+ " must enter a password.  Otherwise, clear your Twitter"
					+ " screen name."
					,
					out m_sCredentialsPassword) )
				{
					return (false);
				}
			}
			else
			{
				sCredentialsScreenName = null;
				m_sCredentialsPassword = null;
			}

			m_oAnalyzeTwitterNetworkDialogUserSettings.ScreenNameToAnalyze =
				sScreenNameToAnalyze;

			m_oAnalyzeTwitterNetworkDialogUserSettings.Levels =
				cbxAnalyzeTwoLevels.Checked ? 2 : 1;

			m_oAnalyzeTwitterNetworkDialogUserSettings.CredentialsScreenName =
				sCredentialsScreenName;

			m_oAnalyzeTwitterNetworkDialogUserSettings.PopulatePrimaryLabels =
				cbxPopulatePrimaryLabels.Checked;
		}
		else
		{
			txbScreenNameToAnalyze.Text =
				m_oAnalyzeTwitterNetworkDialogUserSettings.ScreenNameToAnalyze;

			cbxAnalyzeTwoLevels.Checked =
				(m_oAnalyzeTwitterNetworkDialogUserSettings.Levels == 2);

			txbCredentialsScreenName.Text =
				m_oAnalyzeTwitterNetworkDialogUserSettings.
					CredentialsScreenName;

			txbCredentialsPassword.Text = m_sCredentialsPassword;

			cbxPopulatePrimaryLabels.Checked =
				m_oAnalyzeTwitterNetworkDialogUserSettings.
					PopulatePrimaryLabels;

			EnableControls();
		}

		return (true);
	}

    //*************************************************************************
    //  Method: StartAnalysis()
    //
    /// <summary>
	/// Starts the Twitter analysis.
    /// </summary>
	///
	/// <remarks>
	/// It's assumed that m_oAnalyzeTwitterNetworkDialogUserSettings contains
	/// valid settings.
	/// </remarks>
    //*************************************************************************

    protected void
	StartAnalysis()
    {
		AssertValid();

		m_oTwitterNetworkAnalyzer.AnalyzeTwitterNetworkAsync(
			m_oAnalyzeTwitterNetworkDialogUserSettings.ScreenNameToAnalyze,
			m_oAnalyzeTwitterNetworkDialogUserSettings.Levels,
			m_oAnalyzeTwitterNetworkDialogUserSettings.CredentialsScreenName,
			m_sCredentialsPassword
			);
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

		Boolean bIsBusy = m_oTwitterNetworkAnalyzer.IsBusy;

		btnAnalyze.Text = bIsBusy ? "Stop" : "Start";
		EnableControls(!bIsBusy, pnlUserInputs);
		this.UseWaitCursor = bIsBusy;
	}

    //*************************************************************************
    //  Method: OnLoad()
    //
    /// <summary>
	/// Handles the Load event.
    /// </summary>
    ///
	/// <param name="e">
	/// Standard event argument.
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

		// Get the required edge table before the user does anything in the
		// dialog.

		EdgeWorksheetReader oEdgeWorksheetReader = new EdgeWorksheetReader();

		try
		{
			m_oEdgeTable = oEdgeWorksheetReader.GetEdgeTable(m_oWorkbook);
		}
		catch (Exception oException)
		{
			// The edge table couldn't be found.  Tell the user and close the
			// dialog.

			ErrorUtil.OnException(oException);

			this.Close();

			return;
		}
    }

    //*************************************************************************
    //  Method: OnAnalysisCompleted()
    //
    /// <summary>
	/// Handles the AnalysisCompleted event on the TwitterNetworkAnalyzer
	/// object.
    /// </summary>
    ///
	/// <param name="e">
	/// Standard event argument.
	/// </param>
    //*************************************************************************

	protected void
	OnAnalysisCompleted
	(
		RunWorkerCompletedEventArgs e
	)
	{
		AssertValid();

		EnableControls();
		btnAnalyze.Enabled = true;

        if (e.Cancelled)
        {
			// (Do nothing.)
        }
        else if (e.Error != null)
        {
			OnAnalysisException(e);
        }
        else
        {
			OnAnalysisSuccess(e);
        }
	}

    //*************************************************************************
    //  Method: OnAnalysisException()
    //
    /// <summary>
	/// Handles the AnalysisCompleted event on the TwitterNetworkAnalyzer
	/// object when an exception occurs.
    /// </summary>
    ///
	/// <param name="e">
	/// Standard event argument.
	/// </param>
    //*************************************************************************

	protected void
	OnAnalysisException
	(
		RunWorkerCompletedEventArgs e
	)
	{
		AssertValid();

		Exception oException = e.Error;

		String sMessage = null;

		const String TimeoutMessage =
			"The Twitter Web service didn't respond.";

		if (oException is WebException)
		{
			WebException oWebException = (WebException)oException;

			if (oWebException.Response is HttpWebResponse)
			{
				HttpWebResponse oHttpWebResponse =
					(HttpWebResponse)oWebException.Response;

				switch (oHttpWebResponse.StatusCode)
				{
					case HttpStatusCode.NotFound:  // HTTP 404.

						sMessage =
							"There is no Twitter user with that screen name."
							;

						break;

					case HttpStatusCode.RequestTimeout:  // HTTP 408.

						sMessage = TimeoutMessage;

						break;

					case HttpStatusCode.BadRequest:  // HTTP 400.

						sMessage = String.Format(

							"The Twitter Web service refuses to provide any"
							+ " more user information because {0} has made too"
							+ " many requests in the last hour.  (Twitter"
							+ " limits information requests to prevent its"
							+ " service from being attacked.  Click the '{1}'"
							+ " link for details.)"
							+ "\r\n\r\n"
							+ " Wait 60 minutes and try again."
							,
							ApplicationUtil.ApplicationName,
							lnkRateLimiting.Text
							);

						break;

					case HttpStatusCode.Forbidden:  // HTTP 403.

						sMessage =
							"The Twitter Web service refused to provide"
							+ " information about the user."
							;

						break;

					default:

						break;
				}
			}
			else
			{
				switch (oWebException.Status)
				{
					case WebExceptionStatus.Timeout:

						sMessage = TimeoutMessage;

						break;

					default:

						break;
				}
			}
		}

		if (sMessage == null)
		{
			sMessage =
				"The Twitter network information couldn't be obtained."
				+ "\r\n\r\nDetails:\r\n\r\n"
				+ ExceptionUtil.GetMessageTrace(oException)
				;
		}

		this.ShowWarning(sMessage);
	}

    //*************************************************************************
    //  Method: OnAnalysisSuccess()
    //
    /// <summary>
	/// Handles the AnalysisCompleted event on the TwitterNetworkAnalyzer
	/// object when the analysis is successful.
    /// </summary>
    ///
	/// <param name="e">
	/// Standard event argument.
	/// </param>
    //*************************************************************************

	protected void
	OnAnalysisSuccess
	(
		RunWorkerCompletedEventArgs e
	)
	{
		Debug.Assert(e != null);
		AssertValid();

		// Clear the required edge table and other optional tables.

		NodeXLWorkbookUtil.ClearTables(m_oWorkbook);

		Debug.Assert(e.Result is TwitterNetworkAnalysisResults);

		TwitterNetworkAnalysisResults oTwitterNetworkAnalysisResults =
			(TwitterNetworkAnalysisResults)e.Result;

		ParticipantPair [] aoParticipantPairs =
			oTwitterNetworkAnalysisResults.ParticipantPairs;

		if (aoParticipantPairs.Length == 0)
		{
			this.ShowInformation(
				"That Twitter user has no friends."
				);

			return;
		}

		// Populate the edge table with participant pairs.

		NodeXLWorkbookUtil.PopulateEdgeTableWithParticipantPairs(
			m_oEdgeTable, aoParticipantPairs);

		// Populate the vertex table with the screen name of each participant
		// and optionally his latest post.

		PopulateVertexTable(oTwitterNetworkAnalysisResults);
	}

    //*************************************************************************
    //  Method: PopulateVertexTable()
    //
    /// <summary>
	/// Populates the vertex table with the screen name of each participant and
	/// optionally his latest post.
    /// </summary>
    ///
	/// <param name="oTwitterNetworkAnalysisResults">
	/// Results of the successful Twitter network analysis.
	/// </param>
    //*************************************************************************

	protected void
	PopulateVertexTable
	(
		TwitterNetworkAnalysisResults oTwitterNetworkAnalysisResults
	)
	{
		Debug.Assert(oTwitterNetworkAnalysisResults != null);
		AssertValid();

		// Populate the vertex table with the screen name of each participant.

		VertexWorksheetPopulator oVertexWorksheetPopulator =
			new VertexWorksheetPopulator();

		ListObject oVertexTable =
			oVertexWorksheetPopulator.PopulateVertexWorksheet(
				m_oWorkbook, false);

		if (!m_oAnalyzeTwitterNetworkDialogUserSettings.PopulatePrimaryLabels)
		{
			return;
		}

		// Get the vertex name and primary label column ranges.

		Range oVertexNameRange, oPrimaryLabelRange;
		
		if ( !ExcelUtil.TryGetTableColumnData(oVertexTable,
				VertexTableColumnNames.VertexName, out oVertexNameRange)
			||
			!ExcelUtil.TryGetTableColumnData(oVertexTable,
				VertexTableColumnNames.PrimaryLabel, out oPrimaryLabelRange)
			)
		{
			return;
		}

		Dictionary<String, TwitterParticipant> oTwitterParticipants =
			oTwitterNetworkAnalysisResults.Participants;

		// Read the vertex names in the vertex table all at once.

		Object [,] aoVertexNameValues =
			ExcelUtil.GetRangeValues(oVertexNameRange);

		Int32 iVertexNames = aoVertexNameValues.GetUpperBound(0);

		if (iVertexNames == 1 && aoVertexNameValues[1, 1] == null)
		{
			// This is the case when the user has no Twitter friends.

			return;
		}

		// Create an array to hold the primary labels and write them to the
		// vertex table.

		String [,] asPrimaryLabelValues =
			ExcelUtil.GetSingleColumn2DStringArray(iVertexNames);

		for (Int32 iRowOneBased = 1; iRowOneBased <= iVertexNames;
			iRowOneBased++)
		{
			// VertexWorksheetPopulator.PopulateVertexWorksheet() writes
			// strings to the vertex column of the vertex table, so the values
			// must all be strings.

			Debug.Assert(aoVertexNameValues[iRowOneBased, 1] is String);

			asPrimaryLabelValues[iRowOneBased, 1] =
				ScreenNameToPrimaryLabel(
					(String)aoVertexNameValues[iRowOneBased, 1],
					oTwitterParticipants);
		}

		oPrimaryLabelRange.set_Value(Missing.Value, asPrimaryLabelValues);
	}

    //*************************************************************************
    //  Method: ScreenNameToPrimaryLabel()
    //
    /// <summary>
	/// Coverts a Twitter screen name to a vertex primary label.
    /// </summary>
    ///
	/// <param name="sTwitterScreenName">
	/// Screen name to convert.
	/// </param>
    ///
	/// <param name="oTwitterParticipants">
	/// A dictionary of participants.  The key is the participant's Twitter
	/// screen name and the key is the TwitterParticipant object for the
	/// participant.
	/// </param>
	///
	/// <returns>
	/// A primary label to insert into the vertex table.
	/// </returns>
    //*************************************************************************

	protected String
	ScreenNameToPrimaryLabel
	(
		String sTwitterScreenName,
		Dictionary<String, TwitterParticipant> oTwitterParticipants
	)
	{
		Debug.Assert( !String.IsNullOrEmpty(sTwitterScreenName) );
		Debug.Assert(oTwitterParticipants != null);
		AssertValid();

		TwitterParticipant oTwitterParticipant;

		if ( !oTwitterParticipants.TryGetValue(
			sTwitterScreenName, out oTwitterParticipant) )
		{
			Debug.Assert(false);

			return (String.Empty);
		}

		DateTime oLatestStatusTime = oTwitterParticipant.LatestStatusTime;
		String sLatestStatusTime = String.Empty;

		if (oLatestStatusTime != DateTime.MinValue)
		{
			oLatestStatusTime = oLatestStatusTime.ToLocalTime();

			sLatestStatusTime = "\r\n\r\n"
				+ oLatestStatusTime.ToShortDateString() + " "
				+ oLatestStatusTime.ToShortTimeString();
		}

		String sLatestStatus = oTwitterParticipant.LatestStatus;

		if ( String.IsNullOrEmpty(sLatestStatus) )
		{
			sLatestStatus = "(No posts)";
		}

		return ( String.Format(

			"{0}{1}\r\n\r\n{2}"
			,
			sTwitterScreenName,
			sLatestStatusTime,
			sLatestStatus
			) );
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

		if (m_oTwitterNetworkAnalyzer.IsBusy)
		{
			// Let the background thread cancel its task, but don't try to
			// notify this dialog.

			m_oTwitterNetworkAnalyzer.AnalysisCompleted -=
				new RunWorkerCompletedEventHandler(
					TwitterNetworkAnalyzer_AnalysisCompleted);

			m_oTwitterNetworkAnalyzer.CancelAsync();
		}
	}

    //*************************************************************************
    //  Method: lnkRateLimiting_LinkClicked()
    //
    /// <summary>
	/// Handles the LinkClicked event on the lnkRateLimiting LinkLabel.
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
	lnkRateLimiting_LinkClicked
	(
		object sender, LinkLabelLinkClickedEventArgs e
	)
    {
		AssertValid();

		this.ShowInformation( String.Format(

			"To protect its Web service from attacks, Twitter limits the"
			+ " number of information requests that can be made within a"
			+ " one-hour period.  They call this \"rate limiting.\"  If you"
			+ " attempt to show the friends of a Twitter user AND those"
			+ " friends' friends, you can easily reach Twitter's limit."
			+ "\r\n\r\n"
			+ "We recommend that you either leave the \"{0}\" checkbox"
			+ " unchecked, or ask Twitter to lift the limit for you.  You can"
			+ " do this by clicking the \"{1}\" link.  You must be a"
			+ " registered Twitter user to do this.  Once Twitter has lifted"
			+ " the limit for you, you should enter your Twitter account"
			+ " information when you analyze a Twitter network."
			,
			cbxAnalyzeTwoLevels.Text.Replace("&", String.Empty),
			lnkRequestWhitelist.Text
			) );
    }

    //*************************************************************************
    //  Method: lnkRequestWhitelist_LinkClicked()
    //
    /// <summary>
	/// Handles the LinkClicked event on the lnkRequestWhitelist LinkLabel.
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
	lnkRequestWhitelist_LinkClicked
	(
		object sender, LinkLabelLinkClickedEventArgs e
	)
    {
		AssertValid();

		Process.Start(TwitterRequestWhitelistUrl);
    }

    //*************************************************************************
    //  Method: btnAnalyze_Click()
    //
    /// <summary>
	/// Handles the Click event on the btnAnalyze button.
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
	btnAnalyze_Click
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

		Boolean bCancelling = false;

		if (!m_oTwitterNetworkAnalyzer.IsBusy)
		{
			if ( DoDataExchange(true) )
			{
				StartAnalysis();
			}
		}
		else
		{
			// Request to cancel the analysis.  When the request is completed,
			// TwitterNetworkAnalyzer_AnalysisCompleted() will be called.

			m_oTwitterNetworkAnalyzer.CancelAsync();

			bCancelling = true;
		}

		EnableControls();

		if (bCancelling)
		{
			btnAnalyze.Enabled = false;
		}
    }

    //*************************************************************************
    //  Method: TwitterNetworkAnalyzer_AnalysisCompleted()
    //
    /// <summary>
	/// Handles the AnalysisCompleted event on the TwitterNetworkAnalyzer
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
	TwitterNetworkAnalyzer_AnalysisCompleted
	(
		object sender,
		RunWorkerCompletedEventArgs e
	)
	{
		AssertValid();

		try
		{
            OnAnalysisCompleted(e);
		}
		catch (Exception oException)
		{
			ErrorUtil.OnException(oException);
		}
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

		Debug.Assert(m_oAnalyzeTwitterNetworkDialogUserSettings != null);
		// m_sCredentialsPassword
		Debug.Assert(m_oWorkbook != null);
		Debug.Assert(m_oTwitterNetworkAnalyzer != null);
		// m_oEdgeTable
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	/// Twitter Web page for requesting whitelisting.

	protected const String TwitterRequestWhitelistUrl =
		"http://twitter.com/help/request_whitelisting";

	/// The timeout to use for Twitter Web requests, in milliseconds.

	protected const Int32 HttpWebRequestTimeoutMs = 20000;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// User settings for this dialog.

	protected AnalyzeTwitterNetworkDialogUserSettings
		m_oAnalyzeTwitterNetworkDialogUserSettings;

    /// The password of the Twitter user whose credentials should be used.  Not
	/// used if credentials aren't used.  This is stored separately from the
	/// dialog's user settings to prevent the password from being stored in the
	/// application's settings file in plain text.

	protected String m_sCredentialsPassword;

	/// Workbook containing the graph data.

	protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

	/// Object that does most of the work.

	protected TwitterNetworkAnalyzer m_oTwitterNetworkAnalyzer;

	/// Edge table, or null if the edge table couldn't be obtained.

	protected ListObject m_oEdgeTable;
}


//*****************************************************************************
//  Class: AnalyzeTwitterNetworkDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="AnalyzeTwitterNetworkDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("AnalyzeTwitterNetworkDialog2") ]

public class AnalyzeTwitterNetworkDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: AnalyzeTwitterNetworkDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="AnalyzeTwitterNetworkDialogUserSettings" /> class.
    /// </summary>
	///
	/// <param name="oForm">
	/// The form to save settings for.
	/// </param>
    //*************************************************************************

    public AnalyzeTwitterNetworkDialogUserSettings
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
    //  Property: ScreenNameToAnalyze
    //
    /// <summary>
    /// Gets or sets the screen name of the Twitter user whose network should
	/// be analyzed.
    /// </summary>
    ///
    /// <value>
	/// The screen name of the Twitter user whose network should be analyzed.
	/// The default is "bob".
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("bob") ]

    public String
    ScreenNameToAnalyze
    {
        get
        {
            AssertValid();

			String sScreenNameToAnalyze = (String)this[ScreenNameToAnalyzeKey];

			return (sScreenNameToAnalyze);
        }

        set
        {
            this[ScreenNameToAnalyzeKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Levels
    //
    /// <summary>
    /// Gets or sets the number of friendship levels to include in the
	/// analysis.
    /// </summary>
    ///
    /// <value>
    /// The number of friendship levels to include in the analysis.  The
	/// default is 1.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("1") ]

    public Int32
    Levels
    {
        get
        {
            AssertValid();

			Int32 iLevels = (Int32)this[LevelsKey];

			return (iLevels);
        }

        set
        {
            this[LevelsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: CredentialsScreenName
    //
    /// <summary>
    /// Gets or sets the screen name of the Twitter user whose credentials
	/// should be used.
    /// </summary>
    ///
    /// <value>
	/// The screen name of the Twitter user whose credentials should be used.
	/// The default is String.Empty.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("") ]

    public String
    CredentialsScreenName
    {
        get
        {
            AssertValid();

			String sCredentialsScreenName =
				(String)this[CredentialsScreenNameKey];

			return (sCredentialsScreenName);
        }

        set
        {
            this[CredentialsScreenNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: PopulatePrimaryLabels
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the primary label column on the
	/// vertex worksheet should be populated with the Twitter user's friends.
    /// </summary>
    ///
    /// <value>
	/// true to populate the primary label column.  The default is true.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("true") ]

    public Boolean
    PopulatePrimaryLabels
    {
        get
        {
            AssertValid();

			Boolean bPopulatePrimaryLabels =
				(Boolean)this[PopulatePrimaryLabelsKey];

			return (bPopulatePrimaryLabels);
        }

        set
        {
            this[PopulatePrimaryLabelsKey] = value;

            AssertValid();
        }
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
    //  Protected constants
    //*************************************************************************

	/// Name of the settings key for the ScreenNameToAnalyze property.

	protected const String ScreenNameToAnalyzeKey = "ScreenNameToAnalyze";

	/// Name of the settings key for the Levels property.

	protected const String LevelsKey = "Levels";

	/// Name of the settings key for the CredentialsScreenName property.

	protected const String CredentialsScreenNameKey = "CredentialsScreenName";

	/// Name of the settings key for the PopulatePrimaryLabels property.

	protected const String PopulatePrimaryLabelsKey = "PopulatePrimaryLabels";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}
}
