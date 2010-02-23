
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.NodeXL.Layouts;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ApplicationUtil
{
//*****************************************************************************
//  Class: LayoutManagerForMenu
//
/// <summary>
/// Helper class for managing graph layouts.
/// </summary>
///
/// <remarks>
/// This class is meant for use in applications that allow the user to select
/// a graph layout via a parent "Layout" menu that has one child menu item per
/// available layout.  It provides methods for populating the menu and for
/// creating a layout of the selected type.
///
/// <para>
/// Call <see cref="AddMenuItems" /> during application initialization.  When
/// the user selects one of the menu items added by this method, the <see
/// cref="LayoutManager.LayoutChanged" /> event fires.  In the event handler,
/// call <see cref="LayoutManager.CreateLayout" /> to create a layout of the
/// selected type.
/// </para>
///
/// </remarks>
///
/// <seealso cref="LayoutManager" />
/// <seealso cref="LayoutManagerForToolStripSplitButton" />
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
    /// event fires.
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

        // AllLayouts.GetAllLayouts() returns one array entry for each layout
        // in the LayoutType enumeration.  There may be additional entries that
        // are set to the LayoutGroupSeparator constant.

        foreach ( LayoutInfo oLayoutInfo in AllLayouts.GetAllLayouts() )
        {
            if (oLayoutInfo == AllLayouts.LayoutGroupSeparator)
            {
                AddMenuItemSeparator(parentDropDownItem);
            }
            else
            {
                m_aoMenuItems[ (Int32)oLayoutInfo.Layout ] = AddMenuItem(
                    parentDropDownItem, oLayoutInfo.Layout,
                    oLayoutInfo.MenuText, oLayoutInfo.Description
                    );
            }
        }

        // Check the menu item corresponding to the current layout.

        m_aoMenuItems[ (Int32)this.Layout ].Checked = true;

        #if DEBUG

        // Verify that AllLayouts.GetAllLayouts() returned one array entry for
        // each layout in the LayoutType enumeration.

        for (Int32 i = 0; i < m_aoMenuItems.Length; i++)
        {
            Debug.Assert(m_aoMenuItems[i] != null);
        }

        #endif
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
    /// <param name="sMenuText">
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
        String sMenuText,
        String sToolTipText
    )
    {
        Debug.Assert(oParentDropDownItem != null);
        Debug.Assert( !String.IsNullOrEmpty(sMenuText) );
        Debug.Assert( !String.IsNullOrEmpty(sToolTipText) );
        AssertValid();

        ToolStripMenuItem oMenuItem = new ToolStripMenuItem();

        oMenuItem.Name = sMenuText;
        oMenuItem.Tag = eLayoutType;
        oMenuItem.Text = sMenuText;
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
    /// Parent item to add a separator menu item to.
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
    //  Method: MenuItem_Click()
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

        this.Layout = (LayoutType)oMenuItem.Tag;
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

    /// One menu item for each layout in the LayoutType enumeration, or null if
    /// AddMenuItems() hasn't been called yet.  The items are indexed by the
    /// LayoutType enumeration values cast to Int32s.

    protected ToolStripMenuItem [] m_aoMenuItems;
}

}
