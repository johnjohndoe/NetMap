
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ApplicationUtil
{
public partial class GraphInfoViewer : UserControl
{
//*****************************************************************************
//  Nested class: GraphInfoViewerSettings
//
/// <summary>
/// Saves some of the GraphInfoViewer's properties in the user's settings.
/// </summary>
///
/// <remarks>
/// This class automatically saves and loads some of the GraphInfoViewer's
/// properties in the user's settings.  To use it, create a
/// GraphInfoViewerSettings instance in the GraphInfoViewer's constructor and
/// store the instance in a member field.
///
/// <para>
/// It is not necessary to call <see cref="ApplicationSettingsBase.Save" />
/// when the GraphInfoViewer is closing.  This class does that automatically.
/// </para>
///
/// <para>
/// This class is nested within the <see cref="GraphInfoViewer" /> class, so
/// its type is GraphInfoViewer.GraphInfoViewerSettings.
/// </para>
///
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("GraphInfoViewer") ]

internal class GraphInfoViewerSettings : ApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: GraphInfoViewerSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="GraphInfoViewerSettings" /> class.
    /// </summary>
	///
	/// <param name="oGraphInfoViewer">
	/// The GraphInfoViewer to save settings for.
	/// </param>
    //*************************************************************************

    protected internal GraphInfoViewerSettings
	(
		GraphInfoViewer oGraphInfoViewer
	)
    {
		Debug.Assert(oGraphInfoViewer != null);

		m_oGraphInfoViewer = oGraphInfoViewer;

		// Subscribe to the Load event.

		m_oGraphInfoViewer.Load +=
			new EventHandler(this.GraphInfoViewer_Load);

		AssertValid();
    }

    //*************************************************************************
    //  Property: EdgePanelHeight
    //
    /// <summary>
    /// Gets or sets the height of the edge panel to use when the
	/// GraphInfoViewer loads.
    /// </summary>
    ///
    /// <value>
	/// Height of the edge panel, as an <see cref="Int32" />.  Must be
	/// positive.  The default is 140.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("140") ]

    public Int32
	EdgePanelHeight
    {
        get
        {
            AssertValid();

            return ( (Int32)this[EdgePanelHeightKey] );
        }

        set
        {
			this.ArgumentChecker.CheckPropertyPositive(
				"EdgePanelHeight", value);

            this[EdgePanelHeightKey] = value;

            AssertValid();
        }
    }

	//*************************************************************************
	//	Method: GraphInfoViewer_Load()
	//
	/// <summary>
	///	Handles the Load event on the GraphInfoViewer that owns this object.
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
	GraphInfoViewer_Load
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

		// Subscribe to some of the  parent form's events.
		//
		// Note regarding this control's use in the DesktopApplication project:
		// Don't use the parent form's Load event to retrieve and apply
		// the saved settings to the GraphInfoViewer.  This would work if the
		// MainForm didn't maximize the View after creating it, but when the
		// parent View is maximized, the layout of its child controls doesn't
		// seem to be finished when the Load event fires and applying saved
		// settings doesn't work.

		Form oParentForm = m_oGraphInfoViewer.ParentForm;

		if (oParentForm == null)
		{
			// This occurs in design mode, which is understandable.
			//
			// BUG: It also occurs within VSTO Excel projects, which means that
			// the EdgePanelHeight never gets saved or retrieved in such
			// projects.
			// 

			return;
		}

		Debug.Assert(oParentForm != null);

		oParentForm.Shown += new EventHandler(this.GraphInfoViewer_Shown);

		oParentForm.FormClosing +=
			new FormClosingEventHandler(this.GraphInfoViewer_FormClosing);
    }

	//*************************************************************************
	//	Method: GraphInfoViewer_Shown()
	//
	/// <summary>
	///	Handles the Shown event on the parent form of the GraphInfoViewer
	/// that owns this object.
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
	GraphInfoViewer_Shown
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

		// Set the location of the splitter in the SplitContainer.  This is
		// derived from the client height and the height of the panel that
		// contains the edge ListView.

		SplitContainer oSplitContainer = m_oGraphInfoViewer.spcSplitContainer;

		Int32 iSplitterDistance =
			m_oGraphInfoViewer.ClientSize.Height - this.EdgePanelHeight -
				oSplitContainer.SplitterWidth;

		iSplitterDistance = Math.Max(1, iSplitterDistance);

		oSplitContainer.SplitterDistance = iSplitterDistance;
    }

	//*************************************************************************
	//	Method: GraphInfoViewer_FormClosing()
	//
	/// <summary>
	///	Handles the FormClosing event on the parent form of the GraphInfoViewer
	/// that owns this object.
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
	GraphInfoViewer_FormClosing
	(
		object sender,
		FormClosingEventArgs e
	)
    {
		AssertValid();

		// Save the height of the panel that contains the edge ListView.

        this.EdgePanelHeight =
			m_oGraphInfoViewer.spcSplitContainer.Panel2.Height;

		// Save all properties.

		this.Save();
    }

	//*************************************************************************
	//	Property: ArgumentChecker
	//
	/// <summary>
	/// Gets a new initialized ArgumentChecker object.
	/// </summary>
	///
	/// <value>
	/// A new initialized ArgumentChecker object.
	/// </value>
	//*************************************************************************

	private ArgumentChecker
	ArgumentChecker
	{
		get
		{
			AssertValid();

			return ( new ArgumentChecker(this.GetType().FullName) );
		}
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
		Debug.Assert(m_oGraphInfoViewer != null);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	/// Name of the settings key for the EdgePanelHeight property.

	protected const String EdgePanelHeightKey = "EdgePanelHeight";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// The GraphInfoViewer to save settings for.
	
	protected GraphInfoViewer m_oGraphInfoViewer;
}

}

}
