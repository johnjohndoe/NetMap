
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Visualization;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.DesktopApplication
{
public partial class View : DesktopApplicationForm
{
//*****************************************************************************
//  Nested class: ViewSettings
//
/// <summary>
/// Saves some of the View's properties in the user's settings.
/// </summary>
///
/// <remarks>
/// This class automatically saves and loads some of the View's properties in
/// the user's settings.  To use it, create a ViewSettings instance in the
/// View's constructor and store the instance in a member field.
///
/// <para>
/// It is not necessary to call <see cref="ApplicationSettingsBase.Save" />
/// when the View is closing.  This class does that automatically.
/// </para>
///
/// <para>
/// This class is nested within the <see cref="View" /> class, so its type is
/// View.ViewSettings.
/// </para>
///
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("View") ]

internal class ViewSettings : ApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: ViewSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ViewSettings" /> class.
    /// </summary>
	///
	/// <param name="oView">
	/// The View to save settings for.
	/// </param>
    //*************************************************************************

    protected internal ViewSettings
	(
		View oView
	)
    {
		Debug.Assert(oView != null);

		// Subscribe to some control events.
		//
		// Note: Don't use the Load event to retrieve and apply the saved
		// settings to the view.  This would work if the MainForm didn't
		// maximize the view after creating it, but when the view is maximized,
		// the layout of the child controls doesn't seem to be finished when
		// the Load event fires and applying saved settings doesn't work.

		oView.Shown += new EventHandler(this.View_Shown);

		oView.FormClosing +=
			new FormClosingEventHandler(this.View_FormClosing);

		AssertValid();
    }

    //*************************************************************************
    //  Property: GraphInfoViewerWidth
    //
    /// <summary>
    /// Gets or sets the width of the GraphInfoViewerControl to use when the
	/// View loads.
    /// </summary>
    ///
    /// <value>
	/// Width of the GraphInfoViewerControl, as an <see cref="Int32" />.  Must
	/// be positive.  The default is 140.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("140") ]

    public Int32
	GraphInfoViewerWidth
    {
        get
        {
            AssertValid();

            return ( (Int32)this[GraphInfoViewerWidthKey] );
        }

        set
        {
			this.ArgumentChecker.CheckPropertyPositive(
				"GraphInfoViewerWidth", value);

            this[GraphInfoViewerWidthKey] = value;

            AssertValid();
        }
    }

	//*************************************************************************
	//	Method: View_Shown()
	//
	/// <summary>
	///	Handles the Shown event on the View that owns this object.
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
	View_Shown
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();
		Debug.Assert(sender is View);

		View oView = (View)sender;

		// Set the location of the splitter in the SplitContainer.  The
		// GraphInfoViewerControl is docked in the panel to the right of the
		// splitter, so setting the splitter location sets the panel width and
		// therefore the width of the GraphInfoViewerControl.

		SplitContainer oSplitContainer = oView.spcSplitContainer;

		Int32 iSplitterDistance =
			oView.ClientSize.Width - this.GraphInfoViewerWidth -
				oSplitContainer.SplitterWidth;

		iSplitterDistance = Math.Max(1, iSplitterDistance);

		oSplitContainer.SplitterDistance = iSplitterDistance;
    }

	//*************************************************************************
	//	Method: View_FormClosing()
	//
	/// <summary>
	///	Handles the FormClosing event on the View that owns this object.
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
	View_FormClosing
	(
		object sender,
		FormClosingEventArgs e
	)
    {
		AssertValid();
		Debug.Assert(sender is View);

		View oView = (View)sender;

		// Get the width of the panel containing the GraphInfoViewerControl.
		// Don't use GraphInfoViewerControl.Width, because that reports
		// GraphInfoViewerControl.MinimumSize.Width if the panel is narrower
		// than the control's minimum width.

        this.GraphInfoViewerWidth = oView.spcSplitContainer.Panel2.Width;

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
		// (Do nothing.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	/// Name of the settings key for the GraphInfoViewerWidth property.

	protected const String GraphInfoViewerWidthKey = "GraphInfoViewerWidth";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}

}
