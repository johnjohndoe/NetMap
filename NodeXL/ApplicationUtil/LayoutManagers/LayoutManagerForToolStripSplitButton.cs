
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.NodeXL.Layouts;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ApplicationUtil
{
//*****************************************************************************
//  Class: LayoutManagerForToolStripSplitButton
//
/// <summary>
/// Helper class for managing graph layouts.
/// </summary>
///
/// <remarks>
/// This class is meant for use in applications that allow the user to select
/// a graph layout via a ToolStripSplitButton that has one item per available
/// layout.  It provides methods for populating the ToolStripSplitButton and
/// for creating a layout of the selected type.
///
/// <para>
/// Call <see cref="AddItems" /> during application initialization.  When the
/// user clicks one of the items added by this method, the <see
/// cref="LayoutManager.LayoutChanged" /> event fires.  In the event handler,
/// call <see cref="LayoutManager.CreateLayout" /> to create a layout of the
/// selected type.
/// </para>
///
/// <para>
/// The ToolStripSplitButton object used with this class ends up behaving like
/// a ToolStripComboBox with a DropDownStyle of DropDownList.  It improves on
/// the ToolStripComboBox, however, in that its drop-down list can contain
/// other clickable items, whereas the ToolStripComboBox can contain nothing
/// but items that display text.  If the caller has added drop-down items to
/// the ToolStripSplitButton object before calling <see cref="AddItems" />,
/// this class will preserve those items.
/// </para>
///
/// </remarks>
///
/// <seealso cref="LayoutManager" />
/// <seealso cref="LayoutManagerForMenu" />
//*****************************************************************************

public class LayoutManagerForToolStripSplitButton : LayoutManager
{
    //*************************************************************************
    //  Constructor: LayoutManagerForToolStripSplitButton()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="LayoutManagerForToolStripSplitButton" /> class.
    /// </summary>
    //*************************************************************************

    public LayoutManagerForToolStripSplitButton()
    {
        m_oToolStripSplitButton = null;

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
    /// The layout type to use, as a <see cref="LayoutType" />.  The default
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
            if (m_oToolStripSplitButton != null)
            {
                foreach (ToolStripItem oItem in
                    m_oToolStripSplitButton.DropDownItems)
                {
                    if ( !(oItem.Tag is LayoutInfo) )
                    {
                        continue;
                    }

                    LayoutInfo oLayoutInfo = (LayoutInfo)oItem.Tag;

                    Debug.Assert(oItem is ToolStripMenuItem);

                    ToolStripMenuItem oToolStripMenuItem =
                        (ToolStripMenuItem)oItem;

                    Boolean bChecked = false;

                    if (oLayoutInfo.Layout == value)
                    {
                        bChecked = true;

                        // Make the text of the standard button portion of the
                        // ToolStripSplitButton correspond to the clicked
                        // layout.

                        m_oToolStripSplitButton.Text = oLayoutInfo.Text;

                        base.Layout = oLayoutInfo.Layout;
                    }

                    oToolStripMenuItem.Checked = bChecked;
                }
            }

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: AddItems()
    //
    /// <summary>
    /// Adds a set of items to a ToolStripSplitButton, one for each layout
    /// supported by this class.
    /// </summary>
    ///
    /// <param name="toolStripSplitButton">
    /// ToolStripSplitButton to add items to.
    /// </param>
    ///
    /// <remarks>
    /// When the ToolStripSplitButton's selection is changed, the <see
    /// cref="Layout" /> property is changed and the <see
    /// cref="LayoutManager.LayoutChanged" /> event fires.
    /// </remarks>
    //*************************************************************************

    public void
    AddItems
    (
        ToolStripSplitButton toolStripSplitButton
    )
    {
        const String MethodName = "AddItems";

        this.ArgumentChecker.CheckArgumentNotNull(
            MethodName, "toolStripSplitButton", toolStripSplitButton);

        AssertValid();

        if (m_oToolStripSplitButton != null)
        {
            this.ArgumentChecker.ThrowArgumentException(MethodName,
                "toolStripSplitButton",
                "This method has already been called.  Don't call it twice."
                );
        }

        m_oToolStripSplitButton = toolStripSplitButton;

        m_oToolStripSplitButton.ButtonClick += new EventHandler(
            this.m_oToolStripSplitButton_ButtonClick);

        ToolStripItemCollection oItems = m_oToolStripSplitButton.DropDownItems;
        Int32 iInsertionIndex = 0;

        foreach ( LayoutInfo oLayoutInfo in AllLayouts.GetAllLayouts() )
        {
            ToolStripItem oToolStripItem;

            if (oLayoutInfo == AllLayouts.LayoutGroupSeparator)
            {
                oToolStripItem = new ToolStripSeparator();
            }
            else
            {
                oToolStripItem = new ToolStripMenuItem( oLayoutInfo.Text, null,
                    new EventHandler(this.OnLayoutMenuItemClick) );

                oToolStripItem.Tag = oLayoutInfo;

                oToolStripItem.ToolTipText =
                    oLayoutInfo.Description.TrimEnd('.');
            }

            // Don't call ToolStripItemCollection.Add(), because the
            // ToolStripSplitButton may already have items added to it by the
            // caller.  The items added here should precede any existing items.

            oItems.Insert(iInsertionIndex, oToolStripItem);
            iInsertionIndex++;
        }
    }

    //*************************************************************************
    //  Method: OnLayoutMenuItemClick()
    //
    /// <summary>
    /// Handles the Click event on every drop-down ToolStripMenuItem that
    /// represents a layout type.
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
    OnLayoutMenuItemClick
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        Debug.Assert(sender is ToolStripMenuItem);
        ToolStripMenuItem oToolStripMenuItem = (ToolStripMenuItem)sender;

        Debug.Assert(oToolStripMenuItem.Tag is LayoutInfo);
        LayoutInfo oLayoutInfo = (LayoutInfo)oToolStripMenuItem.Tag;

        this.Layout = oLayoutInfo.Layout;
    }

    //*************************************************************************
    //  Method: m_oToolStripSplitButton_ButtonClick()
    //
    /// <summary>
    /// Handles the ButtonClick event on the standard button portion of the
    /// ToolStripSplitButton.
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
    m_oToolStripSplitButton_ButtonClick
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        // To simulate the behavior of ToolStripComboBox, show the drop-down
        // items when the standard button portion of the ToolStripSplitButton
        // is clicked.  This does not happen by default; clicking the standard
        // button normally just clicks that button.

        m_oToolStripSplitButton.ShowDropDown();
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

        // m_oToolStripSplitButton
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// ToolStripSplitButton that contains one item for each available layout,
    /// or null if AddItems() hasn't been called yet.

    protected ToolStripSplitButton m_oToolStripSplitButton;
}

}
