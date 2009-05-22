
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.Office.Tools.Ribbon;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.NodeXL.Layouts;
using Microsoft.NodeXL.ApplicationUtil;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: LayoutManagerForRibbonDropDown
//
/// <summary>
/// Helper class for managing graph layouts.
/// </summary>
///
/// <remarks>
/// This class is meant for use in Excel applications that allow the user to
/// select a graph layout via a RibbonDropDown that has one item per available
/// layout.  It provides methods for populating the RibbonDropDown and for
/// creating a layout of the selected type.
///
/// <para>
/// Call <see cref="AddRibbonDropDownItems" /> during application
/// initialization.  When the user selects one of the RibbonDropDownItems added
/// by this method, the <see cref="LayoutManager.LayoutChanged" /> event fires.
/// In the event handler, call <see cref="LayoutManager.CreateLayout" /> to
/// create a layout of the selected type.
/// </para>
///
/// </remarks>
///
/// <seealso cref="LayoutManager" />
/// <seealso cref="LayoutManagerForMenu" />
/// <seealso cref="LayoutManagerForComboBox" />
//*****************************************************************************

public class LayoutManagerForRibbonDropDown : LayoutManager
{
    //*************************************************************************
    //  Constructor: LayoutManagerForRibbonDropDown()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="LayoutManagerForRibbonDropDown" /> class.
    /// </summary>
    //*************************************************************************

    public LayoutManagerForRibbonDropDown()
    {
        m_oRibbonDropDown = null;

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
            if (value == base.Layout)
            {
                return;
            }

            // If a RibbonDropDown has been populated, select the item
            // corresponding to the new layout type.

            if (m_oRibbonDropDown != null)
            {
                RibbonDropDownItemCollection oItems = m_oRibbonDropDown.Items;
                Int32 iItems = oItems.Count;
                Int32 i;

                for (i = 0; i < iItems; i++)
                {
                    RibbonDropDownItem oItem = oItems[i];

                    Debug.Assert(oItem.Tag is LayoutType);

                    if ( (LayoutType)oItem.Tag == value )
                    {
                        // Note that setting SelectedItemIndex does not fire a
                        // SelectionChange event.

                        m_oRibbonDropDown.SelectedItemIndex = i;
                        break;
                    }
                }

                Debug.Assert(i < iItems);
            }

            // Note: Settings SelectedItemIndex in a RibbonDropDown does NOT
            // cause the SelectionChanged event to fire.  Therefore, the
            // base.Layout assignment statement in
            // RibbonDropDown_SelectionChanged() doesn't execute, and the
            // assignment must be made here.

            base.Layout = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: AddRibbonDropDownItems()
    //
    /// <summary>
    /// Adds a set of items to a RibbonDropDown, one for each layout supported
    /// by this class.
    /// </summary>
    ///
    /// <param name="ribbonDropDown">
    /// RibbonDropDown to add items to.
    /// </param>
    ///
    /// <remarks>
    /// When the RibbonDropDown's selection is changed, the <see
    /// cref="Layout" /> property is changed and the <see
    /// cref="LayoutManager.LayoutChanged" /> event fires.
    /// </remarks>
    //*************************************************************************

    public void
    AddRibbonDropDownItems
    (
        RibbonDropDown ribbonDropDown
    )
    {
        Debug.Assert(ribbonDropDown != null);
        Debug.Assert(m_oRibbonDropDown == null);
        AssertValid();

        m_oRibbonDropDown = ribbonDropDown;

        RibbonDropDownItemCollection oItems = m_oRibbonDropDown.Items;
        Int32 iIndexToSelect = -1;

        // Add an item for each available layout.

        foreach ( LayoutInfo oLayoutInfo in AllLayouts.GetAllLayouts() )
        {
            if (oLayoutInfo != AllLayouts.LayoutGroupSeparator)
            {
                LayoutType eLayout = oLayoutInfo.Layout;

                if (eLayout == base.Layout)
                {
                    iIndexToSelect = oItems.Count;
                }

                RibbonDropDownItem oItem = new RibbonDropDownItem();
                oItem.Label = oLayoutInfo.Text;
                oItem.Tag = eLayout;

                oItems.Add(oItem);
            }
        }

        Debug.Assert(oItems.Count ==
            Enum.GetValues( typeof(LayoutType) ).Length);

        Debug.Assert(iIndexToSelect != -1);

        m_oRibbonDropDown.SelectedItemIndex = iIndexToSelect;

        m_oRibbonDropDown.SelectionChanged +=
            new System.EventHandler<RibbonControlEventArgs>(
                this.RibbonDropDown_SelectionChanged);
    }

    //*************************************************************************
    //  Method: RibbonDropDown_SelectionChanged()
    //
    /// <summary>
    /// Handles the SelectionChanged event on the m_oRibbonDropDown
    /// RibbonDropDown.
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
    RibbonDropDown_SelectionChanged
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        Debug.Assert(m_oRibbonDropDown != null);
        Debug.Assert(m_oRibbonDropDown.SelectedItem.Tag is LayoutType);

        // This may have been called because SelectedItemIndex was set in the
        // derived Layout property setter.  Avoid calling the derived setter
        // again by calling the base setter instead.

        base.Layout = (LayoutType)m_oRibbonDropDown.SelectedItem.Tag;
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

        // m_oRibbonDropDown
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// RibbonDropDown that contains one item for each available layout, or
    /// null if AddRibbonDropDownItems() hasn't been called yet.

    protected RibbonDropDown m_oRibbonDropDown;
}

}
