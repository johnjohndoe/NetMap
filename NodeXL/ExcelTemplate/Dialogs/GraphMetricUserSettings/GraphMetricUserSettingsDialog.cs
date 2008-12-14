

//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//	Class: GraphMetricUserSettingsDialog
//
/// <summary>
///	Edits a <see cref="GraphMetricUserSettings" /> object.
/// </summary>
///
/// <remarks>
///	Pass a <see cref="GraphMetricUserSettings" /> object to the constructor.
/// If the user edits the object, <see cref="Form.ShowDialog()" /> returns
/// DialogResult.OK.  Otherwise, the object is not modified and <see
/// cref="Form.ShowDialog()" /> returns DialogResult.Cancel.
/// </remarks>
//*****************************************************************************

public partial class GraphMetricUserSettingsDialog : ExcelTemplateForm
{
	//*************************************************************************
	//	Constructor: GraphMetricUserSettingsDialog()
	//
	/// <overloads>
	///	Initializes a new instance of the <see
	/// cref="GraphMetricUserSettingsDialog" /> class.
	/// </overloads>
	///
	/// <summary>
	///	Initializes a new instance of the <see
	/// cref="GraphMetricUserSettingsDialog" /> class with a
	/// GraphMetricUserSettings object.
	/// </summary>
	///
    /// <param name="graphMetricUserSettings">
	/// The object being edited.
    /// </param>
	//*************************************************************************

	public GraphMetricUserSettingsDialog
	(
		GraphMetricUserSettings graphMetricUserSettings
	)
	: this()
	{
		Debug.Assert(graphMetricUserSettings != null);

		m_oGraphMetricUserSettings = graphMetricUserSettings;

		// Instantiate an object that saves and retrieves the position of this
		// dialog.  Note that the object automatically saves the settings when
		// the form closes.

		m_oGraphMetricUserSettingsDialogUserSettings =
			new GraphMetricUserSettingsDialogUserSettings(this);

		clbGraphMetrics.Items.AddRange( new Object [] {

			new ObjectWithText( GraphMetric.InDegree,
				"In-Degree  (directed graphs only)"),

			new ObjectWithText( GraphMetric.OutDegree,
				"Out-Degree  (directed graphs only)"),

			new ObjectWithText( GraphMetric.Degree,
				"Degree  (undirected graphs only)"),

			new ObjectWithText( GraphMetric.ClusteringCoefficient,
				"Clustering Coefficient"),

			new ObjectWithText( GraphMetric.BetweennessCentrality,
				"Betweenness Centrality"),

			new ObjectWithText( GraphMetric.OverallMetrics,
				"Overall Metrics"),
			} );

		DoDataExchange(false);

		AssertValid();
	}

	//*************************************************************************
	//	Constructor: GraphMetricUserSettingsDialog()
	//
	/// <summary>
	///	Initializes a new instance of the <see
	/// cref="GraphMetricUserSettingsDialog" /> class for the Visual Studio
	/// designer.
	/// </summary>
	///
	/// <remarks>
	/// Do not use this constructor.  It is for use by the Visual Studio
	/// designer only.
	/// </remarks>
	//*************************************************************************

	public GraphMetricUserSettingsDialog()
	{
		InitializeComponent();

		// AssertValid();
	}

    //*************************************************************************
    //  Enum: GraphMetric
    //
    /// <summary>
	/// Graph metric identifiers.
    /// </summary>
    //*************************************************************************

    protected enum
    GraphMetric
    {
        /// In-degree.

        InDegree,

        /// Out-degree.

        OutDegree,

        /// Degree.

        Degree,

		/// Clustering coefficient.

		ClusteringCoefficient,

		/// Betweenness centrality.

		BetweennessCentrality,

		/// Overall graph metrics.

		OverallMetrics,
    }

    //*************************************************************************
    //  Method: GraphMetricCheckBoxIsChecked
    //
    /// <summary>
    /// Gets a flag indicating whether a checkbox for a graph metric is
	/// checked.
    /// </summary>
    ///
	/// <param name="eGraphMetric">
	///	The GraphMetric corresponding to the checkbox.
	/// </param>
	///
    /// <returns>
	/// true if the specified checkbox is checked.
    /// </returns>
    //*************************************************************************

    protected Boolean
    GraphMetricCheckBoxIsChecked
	(
		GraphMetric eGraphMetric
	)
    {
		AssertValid();

		// Loop through the checked checkbox items in the list of graph
		// metrics.

		foreach (Object oCheckedItem in clbGraphMetrics.CheckedItems)
		{
			if (ListBoxItemToGraphMetric(oCheckedItem) == eGraphMetric)
			{
				return (true);
			}
		}

		return (false);
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
        AssertValid();

		CheckedListBox.ObjectCollection oGraphMetricItems =
			clbGraphMetrics.Items;

		Int32 iGraphMetricItems = oGraphMetricItems.Count;

		// Loop through the checkbox items in the list of graph metrics.

		for (Int32 i = 0; i < iGraphMetricItems; i++)
		{
			Boolean bItemIsChecked = clbGraphMetrics.GetItemChecked(i);

			switch ( ListBoxItemToGraphMetric( oGraphMetricItems[i] ) )
			{
				case GraphMetric.InDegree:

					if (bFromControls)
					{
						m_oGraphMetricUserSettings.CalculateInDegree =
							bItemIsChecked;
					}
					else
					{
						bItemIsChecked =
							m_oGraphMetricUserSettings.CalculateInDegree;
					}

					break;

				case GraphMetric.OutDegree:

					if (bFromControls)
					{
						m_oGraphMetricUserSettings.CalculateOutDegree =
							bItemIsChecked;
					}
					else
					{
						bItemIsChecked =
							m_oGraphMetricUserSettings.CalculateOutDegree;
					}

					break;

				case GraphMetric.Degree:

					if (bFromControls)
					{
						m_oGraphMetricUserSettings.CalculateDegree =
							bItemIsChecked;
					}
					else
					{
						bItemIsChecked =
							m_oGraphMetricUserSettings.CalculateDegree;
					}

					break;

				case GraphMetric.ClusteringCoefficient:

					if (bFromControls)
					{
						m_oGraphMetricUserSettings.
							CalculateClusteringCoefficient = bItemIsChecked;
					}
					else
					{
						bItemIsChecked = m_oGraphMetricUserSettings.
							CalculateClusteringCoefficient;
					}

					break;

				case GraphMetric.BetweennessCentrality:

					if (bFromControls)
					{
						m_oGraphMetricUserSettings.
							CalculateBetweennessCentrality = bItemIsChecked;
					}
					else
					{
						bItemIsChecked = m_oGraphMetricUserSettings.
							CalculateBetweennessCentrality;
					}

					break;

				case GraphMetric.OverallMetrics:

					if (bFromControls)
					{
						m_oGraphMetricUserSettings.
							CalculateOverallMetrics = bItemIsChecked;
					}
					else
					{
						bItemIsChecked = m_oGraphMetricUserSettings.
							CalculateOverallMetrics;
					}

					break;

				default:

					Debug.Assert(false);
					break;
			}

			if (!bFromControls)
			{
				clbGraphMetrics.SetItemChecked(i, bItemIsChecked);
			}
		}

		return (true);
	}

	//*************************************************************************
	//	Method: CheckAllListBoxItems()
	//
	/// <summary>
	/// Checks or unchecks all items in the clbGraphMetrics CheckedListBox.
	/// </summary>
	///
	/// <param name="bCheck">
	///	true to check all items, false to uncheck them.
	/// </param>
	//*************************************************************************

	protected void
	CheckAllListBoxItems
	(
		Boolean bCheck
	)
	{
        AssertValid();

		Int32 iGraphMetricItems = clbGraphMetrics.Items.Count;

		for (Int32 i = 0; i < iGraphMetricItems; i++)
		{
			clbGraphMetrics.SetItemChecked(i, bCheck);
		}
	}

	//*************************************************************************
	//	Method: ListBoxItemToGraphMetric()
	//
	/// <summary>
	///	Retrieves the GraphMetric value stored in an item from the
	/// clbGraphMetrics CheckedListBox.
	/// </summary>
	///
	/// <param name="oListBoxItem">
	///	Item from the clbGraphMetrics CheckedListBox.
	/// </param>
	///
	/// <returns>
	///	The GraphMetric value stored within <paramref name="oListBoxItem" />.
	/// </returns>
	//*************************************************************************

	protected GraphMetric
	ListBoxItemToGraphMetric
	(
		Object oListBoxItem
	)
	{
		Debug.Assert(oListBoxItem != null);
        AssertValid();

		Debug.Assert(oListBoxItem is ObjectWithText);

		ObjectWithText oObjectWithText = (ObjectWithText)oListBoxItem;

		Debug.Assert(oObjectWithText.Object is GraphMetric);

		return ( (GraphMetric)oObjectWithText.Object );
	}

	//*************************************************************************
	//	Method: btnCheckAll_Click()
	//
	/// <summary>
	///	Handles the Click event on the btnCheckAll button.
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
	btnCheckAll_Click
	(
		object sender,
		System.EventArgs e
	)
	{
		AssertValid();

		CheckAllListBoxItems(true);
	}

	//*************************************************************************
	//	Method: btnUncheckAll_Click()
	//
	/// <summary>
	///	Handles the Click event on the btnUncheckAll button.
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
	btnUncheckAll_Click
	(
		object sender,
		System.EventArgs e
	)
	{
		AssertValid();

		CheckAllListBoxItems(false);
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

		Debug.Assert(m_oGraphMetricUserSettings != null);
		Debug.Assert(m_oGraphMetricUserSettingsDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Object whose properties are being edited.

	protected GraphMetricUserSettings m_oGraphMetricUserSettings;

	/// User settings for this dialog.

	protected GraphMetricUserSettingsDialogUserSettings
		m_oGraphMetricUserSettingsDialogUserSettings;
}


//*****************************************************************************
//  Class: GraphMetricUserSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="GraphMetricUserSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("GraphMetricUserSettingsDialog") ]

public class GraphMetricUserSettingsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: GraphMetricUserSettingsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="GraphMetricUserSettingsDialogUserSettings" /> class.
    /// </summary>
	///
	/// <param name="oForm">
	/// The form to save settings for.
	/// </param>
    //*************************************************************************

    public GraphMetricUserSettingsDialogUserSettings
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
