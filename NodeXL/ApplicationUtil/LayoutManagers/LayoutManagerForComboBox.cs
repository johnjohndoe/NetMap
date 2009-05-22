
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.NodeXL.Layouts;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ApplicationUtil
{
//*****************************************************************************
//  Class: LayoutManagerForComboBox
//
/// <summary>
/// Helper class for managing graph layouts.
/// </summary>
///
/// <remarks>
/// This class is meant for use in applications that allow the user to select
/// a graph layout via a ComboBox that has one item per available layout.  It
/// provides methods for populating the ComboBox and for creating a layout of
/// the selected type.
///
/// <para>
/// Call <see cref="AddComboBoxItems" /> during application initialization.
/// When the user selects one of the combo box items added by this method, the
/// <see cref="LayoutManager.LayoutChanged" /> event fires.  In the event
/// handler, call <see cref="LayoutManager.CreateLayout" /> to create a layout
/// of the selected type.
/// </para>
///
/// </remarks>
///
/// <seealso cref="LayoutManager" />
/// <seealso cref="LayoutManagerForMenu" />
//*****************************************************************************

public class LayoutManagerForComboBox : LayoutManager
{
    //*************************************************************************
    //  Constructor: LayoutManagerForComboBox()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="LayoutManagerForComboBox" /> class.
    /// </summary>
    //*************************************************************************

    public LayoutManagerForComboBox()
    {
        m_oComboBox = null;

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

            // If a ComboBox has been populated, select the item corresponding
            // to the new layout type.

            if (m_oComboBox != null)
            {
                ComboBox.ObjectCollection oItems = m_oComboBox.Items;
                Int32 iItems = oItems.Count;
                Int32 i;

                for (i = 0; i < iItems; i++)
                {
                    Debug.Assert(oItems[i] is ObjectWithText);

                    Debug.Assert( ( (ObjectWithText)oItems[i] ).Object
                        is LayoutType);

                    if ( (LayoutType)( ( (ObjectWithText)oItems[i] ).Object)
                        == value )
                    {
                        // Note that setting SelectedIndex causes the
                        // SelectedIndexChanged event to fire, and that
                        // ComboBox_SelectedIndexChanged() sets base.Layout.

                        m_oComboBox.SelectedIndex = i;
                        break;
                    }
                }

                Debug.Assert(i < iItems);
            }
            else
            {
                base.Layout = value;
            }

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: AddComboBoxItems()
    //
    /// <summary>
    /// Adds a set of items to a ComboBox, one for each layout supported by
    /// this class.
    /// </summary>
    ///
    /// <param name="comboBox">
    /// ComboBox to add items to.
    /// </param>
    ///
    /// <remarks>
    /// When the ComboBox's selection is changed, the <see cref="Layout" />
    /// property is changed and the <see cref="LayoutManager.LayoutChanged" />
    /// event fires.
    /// </remarks>
    //*************************************************************************

    public void
    AddComboBoxItems
    (
        ComboBox comboBox
    )
    {
        const String MethodName = "AddComboBoxItems";

        this.ArgumentChecker.CheckArgumentNotNull(
            MethodName, "comboBox", comboBox);

        AssertValid();

        if (m_oComboBox != null)
        {
            this.ArgumentChecker.ThrowArgumentException(MethodName, "comboBox",
                "This method has already been called.  Don't call it twice."
                );
        }

        m_oComboBox = comboBox;

        // Add an ObjectWithText for each available layout.

        ComboBox.ObjectCollection oItems = m_oComboBox.Items;
        Int32 iIndexToSelect = -1;

        foreach ( LayoutInfo oLayoutInfo in AllLayouts.GetAllLayouts() )
        {
            if (oLayoutInfo != AllLayouts.LayoutGroupSeparator)
            {
                LayoutType eLayout = oLayoutInfo.Layout;

                if (eLayout == base.Layout)
                {
                    iIndexToSelect = oItems.Count;
                }

                oItems.Add( new ObjectWithText(eLayout, oLayoutInfo.Text) );
            }
        }

        Debug.Assert(oItems.Count ==
            Enum.GetValues( typeof(LayoutType) ).Length);

        Debug.Assert(iIndexToSelect != -1);

        m_oComboBox.SelectedIndex = iIndexToSelect;

        m_oComboBox.SelectedIndexChanged +=
            new System.EventHandler(this.ComboBox_SelectedIndexChanged);
    }

    //*************************************************************************
    //  Method: ComboBox_SelectedIndexChanged()
    //
    /// <summary>
    /// Handles the SelectedIndexChanged event on the m_oComboBox ComboBox.
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
    ComboBox_SelectedIndexChanged
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        Debug.Assert(m_oComboBox != null);
        Debug.Assert(m_oComboBox.SelectedItem is ObjectWithText);

        Debug.Assert( ( (ObjectWithText)m_oComboBox.SelectedItem ).Object is
            LayoutType );

        // This may have been called because SelectedIndex was set in the
        // derived Layout property setter.  Avoid calling the derived setter
        // again by calling the base setter instead.

        base.Layout = (LayoutType)
            ( (ObjectWithText)m_oComboBox.SelectedItem ).Object;
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

        // m_oComboBox
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// ComboBox that contains one item for each available layout, or null if
    /// AddComboBoxItems() hasn't been called yet.

    protected ComboBox m_oComboBox;
}

}
