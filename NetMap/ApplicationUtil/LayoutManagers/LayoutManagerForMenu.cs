
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.NetMap.Visualization;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NetMap.ApplicationUtil
{
//*****************************************************************************
//  Class: LayoutManagerForMenu
//
/// <summary>
/// Helper class for managing layouts used by the <see cref="NetMapControl" />.
/// </summary>
///
/// <remarks>
/// This class is meant for use in applications that allow the user to select
/// the layout used by a <see cref="NetMapControl" /> via a parent "Layout" menu
/// that has one child menu item per available layout.  It provides methods for
/// populating the menu and for setting the <see cref="NetMapControl.Layout" />,
/// <see cref="NetMapControl.VertexDrawer" />, and <see
/// cref="NetMapControl.EdgeDrawer" /> properties on a <see
/// cref="NetMapControl" /> with a set of objects that are designed to work
/// together.
///
/// <para>
/// Call <see cref="AddMenuItems" /> during application initialization.  When
/// the user selects one of the menu items added by this method, the <see
/// cref="LayoutManager.LayoutChanged" /> event fires.  In the event handler,
/// call <see cref="LayoutManager.ApplyLayoutToNetMapControl" />.
/// </para>
///
/// </remarks>
///
/// <seealso cref="LayoutManager" />
//*****************************************************************************

public class LayoutManagerForMenu : LayoutManager
{
    //*************************************************************************
    //  Constructor: LayoutManagerForMenu()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="LayoutManagerForMenu" />
	/// class.
    /// </summary>
    //*************************************************************************

    public LayoutManagerForMenu()
    {
		m_aoMenuItems = null;

		AssertValid();
    }

    //*************************************************************************
    //  Property: Layout
    //
    /// <summary>
    /// Gets or sets the layout type to use.
    /// </summary>
    ///
    /// <value>
    /// The layout type to use, as a <see cref="LayoutType" />. The default
    /// is <see cref="LayoutType.FruchtermanReingold" />.
    /// </value>
    //*************************************************************************

    public new LayoutType
    Layout
    {
        get
        {
            AssertValid();

            return (base.Layout);
        }

		set
		{
			if (value == base.Layout)
			{
				return;
			}

			// If menu items have been added, uncheck the item for the old
			// layout type and check the item for the new one.

			if (m_aoMenuItems != null)
			{
				m_aoMenuItems[ (Int32)base.Layout ].Checked = false;
				m_aoMenuItems[ (Int32)value].Checked = true;
			}

			base.Layout = value;

			AssertValid();
		}
    }

    //*************************************************************************
    //  Method: AddMenuItems()
    //
    /// <summary>
	/// Adds a set of child menu items to a parent menu item, one for each
	/// layout supported by this class.
    /// </summary>
    ///
    /// <param name="parentDropDownItem">
    /// Parent item to add child menu items to.  This can be either a
	/// ToolStripMenuItem or a ToolStripDropDownButton.
    /// </param>
	///
	/// <remarks>
	/// When one of the child menu items is selected, the <see cref="Layout" />
	/// property is changed and the <see cref="LayoutManager.LayoutChanged" />
	/// event fires.  You may want to call <see
	/// cref="LayoutManager.ApplyLayoutToNetMapControl" /> from the event
	/// handler.
	/// </remarks>
    //*************************************************************************

    public void
    AddMenuItems
    (
        ToolStripDropDownItem parentDropDownItem
    )
    {
		const String MethodName = "AddMenuItems";

		this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "parentDropDownItem", parentDropDownItem);

        AssertValid();

		if (m_aoMenuItems != null)
		{
			this.ArgumentChecker.ThrowArgumentException(MethodName,
				"parentDropDownItem",
				"This method has already been called.  Don't call it twice."
				);
		}

		Int32 iMenuItems = Enum.GetValues( typeof(LayoutType) ).Length;

		m_aoMenuItems = new ToolStripMenuItem[iMenuItems];

		m_aoMenuItems[ (Int32)LayoutType.FruchtermanReingold ] =
			AddMenuItem(parentDropDownItem, LayoutType.FruchtermanReingold,
				"&Fruchterman-Reingold",
				"Use a Fruchterman-Reingold force-directed layout"
				);

		AddMenuItemSeparator(parentDropDownItem);

		m_aoMenuItems[ (Int32)LayoutType.Circle ] =
			AddMenuItem(parentDropDownItem, LayoutType.Circle, "&Circle",
				"Place the vertices on the circumference of a circle"
				);

		m_aoMenuItems[ (Int32)LayoutType.Spiral ] =
			AddMenuItem(parentDropDownItem, LayoutType.Spiral, "&Spiral",
				"Place the vertices along a spiral"
				);

		m_aoMenuItems[ (Int32)LayoutType.SinusoidHorizontal ] =
			AddMenuItem(parentDropDownItem, LayoutType.SinusoidHorizontal,
				"&Horizontal Sine Wave",
				"Place the vertices along a sine wave running left to right"
				);

		m_aoMenuItems[ (Int32)LayoutType.SinusoidVertical ] =
			AddMenuItem(parentDropDownItem, LayoutType.SinusoidVertical,
				"&Vertical Sine Wave",
				"Place the vertices along a sine wave running top to bottom"
				);

		m_aoMenuItems[ (Int32)LayoutType.Grid ] =
			AddMenuItem(parentDropDownItem, LayoutType.Grid, "&Grid",
				"Place the vertices on an evenly-spaced grid"
				);

		AddMenuItemSeparator(parentDropDownItem);

		m_aoMenuItems[ (Int32)LayoutType.Sugiyama ] =
			AddMenuItem(parentDropDownItem, LayoutType.Sugiyama, "Sugi&yama",
				"Use a modified Sugiyama layered layout that tries to minimize"
				+ " edge crossings"
				);

		m_aoMenuItems[ (Int32)LayoutType.Random ] =
			AddMenuItem(parentDropDownItem, LayoutType.Random, "&Random",
				"Place the vertices at random locations"
				);

		// Check the menu item corresponding to the current layout.

		m_aoMenuItems[ (Int32)this.Layout ].Checked = true;

		#if DEBUG

		for (Int32 i = 0; i < m_aoMenuItems.Length; i++)
		{
			Debug.Assert(m_aoMenuItems[i] != null);
		}

		#endif
    }

    //*************************************************************************
    //  Method: EnableMenuItems()
    //
    /// <summary>
	/// Enables or disables the child menu items added by <see
	/// cref="AddMenuItems" />.
    /// </summary>
    ///
    /// <param name="enable">
    /// true to enable the menu items, false to disable them.
    /// </param>
    //*************************************************************************

    public void
    EnableMenuItems
    (
		Boolean enable
    )
    {
		if (m_aoMenuItems == null)
		{
			// AddMenuItems() hasn't been called.

			return;
		}

		foreach (ToolStripMenuItem oMenuItem in m_aoMenuItems)
		{
			oMenuItem.Enabled = enable;
		}
    }

    //*************************************************************************
    //  Method: AddMenuItem()
    //
    /// <summary>
	/// Adds a child menu item to a parent menu item for one layout supported
	/// by this class.
    /// </summary>
    ///
    /// <param name="oParentDropDownItem">
    /// Parent item to add the child menu item to.
    /// </param>
    ///
    /// <param name="eLayoutType">
    /// Layout represented by the child menu item.
    /// </param>
    ///
    /// <param name="sText">
    /// Text for the menu item.
    /// </param>
    /// 
    /// <param name="sToolTipText">
    /// Tooltip for the menu item.
    /// </param>
    /// 
    /// <returns>
    /// The new child menu item.
    /// </returns>
    //*************************************************************************

	protected ToolStripMenuItem
	AddMenuItem
	(
		ToolStripDropDownItem oParentDropDownItem,
		LayoutType eLayoutType,
		String sText,
		String sToolTipText
	)
	{
		Debug.Assert(oParentDropDownItem != null);
		Debug.Assert( !String.IsNullOrEmpty(sText) );
		Debug.Assert( !String.IsNullOrEmpty(sToolTipText) );
		AssertValid();

		ToolStripMenuItem oMenuItem = new ToolStripMenuItem();

		oMenuItem.Name = sText;
		oMenuItem.Tag = eLayoutType;
		oMenuItem.Text = sText;
		oMenuItem.ToolTipText = sToolTipText;
		oMenuItem.Click += new System.EventHandler(this.MenuItem_Click);

		oParentDropDownItem.DropDownItems.Add(oMenuItem);

        return (oMenuItem);
	}

    //*************************************************************************
    //  Method: AddMenuItemSeparator()
    //
    /// <summary>
	/// Adds a separator menu item to a parent menu item.
    /// </summary>
    ///
    /// <param name="oParentDropDownItem">
    /// Parent item to add a spearator menu item to.
    /// </param>
    //*************************************************************************

	protected void
	AddMenuItemSeparator
	(
		ToolStripDropDownItem oParentDropDownItem
	)
	{
		Debug.Assert(oParentDropDownItem != null);
		AssertValid();

		oParentDropDownItem.DropDownItems.Add( new ToolStripSeparator() );
	}

	//*************************************************************************
	//	Method: MenuItem_Click()
	//
	/// <summary>
	/// Handles the Click event on each of the child menu items added by <see
	/// cref="AddMenuItems" />.
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
	MenuItem_Click
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

		Debug.Assert(sender is ToolStripMenuItem);

		ToolStripMenuItem oMenuItem = (ToolStripMenuItem)sender;

		// Each child menu item's Tag is set to a LayoutType value.
		// Retrieve it.

		Debug.Assert(oMenuItem.Tag is LayoutType);

		LayoutType eLayoutType = (LayoutType)oMenuItem.Tag;

		this.Layout = eLayoutType;
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

		// m_aoMenuItems
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// One menu item for each layout, or null if AddMenuItems() hasn't been
	/// called yet.

	protected ToolStripMenuItem [] m_aoMenuItems;
}

}
