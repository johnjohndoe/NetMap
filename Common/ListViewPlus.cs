
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.Windows.Forms;
using System.Globalization;
using System.Text;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: ListViewPlus
//
/// <summary>
/// Represents a ListView with additional features.
/// </summary>
///
/// <remarks>
/// This ListView-derived object supports sorting by a column that contains
/// data of an arbitrary type.  To enable sorting, populate the
/// <see cref="ListView.Columns" /> collection, then call the
/// <see cref="EnableSorting" /> method.  You can then call the
/// <see cref="Sort" /> method to sort by a specified column.  Sorting also
/// occurs automatically when the user clicks a column header.
///
/// <para>
/// By default, right-clicking an item in the ListView shows a context menu
/// with a set of standard menu items supported by this base class.  If the
/// derived class needs a custom context menu, it should set
/// <see cref="UseStandardContextMenu" /> to false and override OnMouseUp().
/// The derived OnMouseUp() should create and show a custom context menu, then
/// call the base-class method.  If the custom context menu should contain the
/// standard menu items in addition to the custom items, <see
/// cref="AddStandardContextMenuItems" /> should be called to add those items
/// to the menu.
/// </para>
///
/// <para>
/// Leave the <see cref="ListView.Sorting" /> property set to its default
/// value of SortOrder.None.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class ListViewPlus : ListView, IComparer
{
    //*************************************************************************
    //  Constructor: ListViewPlus()
    //
    /// <summary>
    /// Initializes a new instance of the ListViewPlus class.
    /// </summary>
    //*************************************************************************

    public ListViewPlus()
    {
        m_aeColumnTypes = null;
        m_iSortColumn = NoColumn;
        m_bSortAscending = false;
        m_bUseStandardContextMenu = true;
        m_iColumnToCopy = NoColumn;
        m_oRightClickedItem = null;
    }

    //*************************************************************************
    //  Property: SelectedItem
    //
    /// <summary>
    /// Gets the selected item in a single-select ListView.
    /// </summary>
    ///
    /// <value>
    /// The selected <see cref="ListViewItem" />, or null if no item is
    /// selected.
    /// </value>
    ///
    /// <remarks>
    /// The <see cref="ListView.MultiSelect" /> property must be set to false.
    /// </remarks>
    //*************************************************************************

    [System.ComponentModel.BrowsableAttribute(false)]
    [System.ComponentModel.ReadOnlyAttribute(true)]

    public ListViewItem SelectedItem
    {
        get
        {
            Debug.Assert(!MultiSelect);

            ListView.SelectedListViewItemCollection oSelectedItems =
                SelectedItems;

            Debug.Assert(oSelectedItems.Count <= 1);

            if (oSelectedItems.Count == 0)
                return (null);

            return ( oSelectedItems[0] );
        }
    }

    //*************************************************************************
    //  Property: ColumnToCopy
    //
    /// <summary>
    /// Gets the index of the column for which a copy command should be
    /// included in context menus.
    /// </summary>
    ///
    /// <value>
    /// A zero-based column index, or NoColumn if a copy column command should
    /// not be included.  The default value is NoColumn.
    /// </value>
    ///
    /// <remarks>
    /// If this property is not NoColumn, the default context menu that gets
    /// displayed when the user right-clicks a ListViewItem includes a menu
    /// item for copying the specified column of the right-clicked item.  If
    /// this property is 0 and the first column has a column header named
    /// "E-Mail Address", for example, then a "Copy E-Mail Address" menu item
    /// is added to the context menu.
    /// </remarks>
    //*************************************************************************

    // Prevent the property from being set in design mode.

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.ReadOnly(true)]

    public Int32 ColumnToCopy
    {
        get
        {
            AssertValid();

            return (m_iColumnToCopy);
        }

        set
        {
            m_iColumnToCopy = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: UseStandardContextMenu
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the standard context menu should
    /// be used when the user right-clicks an item.
    /// </summary>
    ///
    /// <value>
    /// true if the standard context menu should be used.  The default value is
    /// true.
    /// </value>
    ///
    /// <remarks>
    /// See the comments at the top of this class for more details on context
    /// menus.
    /// </remarks>
    //*************************************************************************

    public Boolean UseStandardContextMenu
    {
        get
        {
            AssertValid();

            return (m_bUseStandardContextMenu);
        }

        set
        {
            m_bUseStandardContextMenu = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: DeslectAllItems()
    //
    /// <summary>
    /// Deselects all items in the ListView.
    /// </summary>
    //*************************************************************************

    public void
    DeselectAllItems()
    {
        foreach (ListViewItem oItem in this.Items)
            oItem.Selected = false;
    }

    //*************************************************************************
    //  Method: GetItemByTag()
    //
    /// <summary>
    /// Gets the ListViewItem that has the specified tag.
    /// </summary>
    ///
    /// <param name="oTag">
    /// Object to look for in the item tags.  Important: This can't be an
    /// Int32.
    /// </param>
    ///
    /// <returns>
    /// The ListViewItem with the specified tag.
    /// </returns>
    ///
    /// <summary>
    /// This method finds and returns the item that has a Tag property set to
    /// the object <paramref name="oTag" />.  If no item has that tag, an
    /// exception is thrown.
    ///
    /// <para>
    /// Use the other version of this method if the tag is an Int32.
    /// </para>
    ///
    /// </summary>
    //*************************************************************************

    public ListViewItem
    GetItemByTag
    (
        Object oTag
    )
    {
        Debug.Assert(oTag != null);
        Debug.Assert( !(oTag.GetType() == typeof(Int32) ) );

        foreach (ListViewItem oItem in this.Items)
            if (oItem.Tag == oTag)
                return (oItem);

        throw new Exception("ListViewPlus.GetItemByTag: No such tag.");
    }

    //*************************************************************************
    //  Method: GetItemByTag()
    //
    /// <summary>
    /// Gets the ListViewItem that has the specified tag.
    /// </summary>
    ///
    /// <param name="iTag">
    /// Int32 to look for in the item tags.
    /// </param>
    ///
    /// <returns>
    /// The ListViewItem with the specified Int32 tag.
    /// </returns>
    ///
    /// <summary>
    /// This method finds and returns the item that has a Tag property set to
    /// the Int32 <paramref name="iTag" />.  If no item has that tag, an
    /// exception is thrown.
    ///
    /// <para>
    /// Use the other version of this method if the tag is an object.
    /// </para>
    ///
    /// </summary>
    //*************************************************************************

    public ListViewItem
    GetItemByTag
    (
        Int32 iTag
    )
    {
        foreach (ListViewItem oItem in this.Items)
            if ( (Int32)oItem.Tag == iTag )
                return (oItem);

        throw new Exception("ListViewPlus.GetItemByTag: No such tag.");
    }

    //*************************************************************************
    //  Method: GetRightClickedItem()
    //
    /// <summary>
    /// Gets the ListViewItem the user right-clicked on.
    /// </summary>
    ///
    /// <param name="oMouseEventArgs">
    /// Standard event argument.
    /// </param>
    ///
    /// <returns>
    /// The ListViewItem the user right-clicked on, or null if he didn't use
    /// the right mouse button or clicked an area not containing an item.
    /// </returns>
    ///
    /// <summary>
    /// This is meant for use within a MouseUp event handler for the ListView.
    /// </summary>
    //*************************************************************************

    public ListViewItem
    GetRightClickedItem
    (
        MouseEventArgs oMouseEventArgs
    )
    {
        Debug.Assert(oMouseEventArgs != null);

        ListViewItem oListViewItem = null;

        if (oMouseEventArgs.Button == MouseButtons.Right)
            oListViewItem = GetItemAt(oMouseEventArgs.X, oMouseEventArgs.Y);

        return (oListViewItem);
    }

    //*************************************************************************
    //  Method: EnableSorting()
    //
    /// <summary>
    /// Enables sorting by a column that contains data of an arbitrary type.
    /// </summary>
    ///
    /// <param name="aoColumnTypes">
    /// One or more Type objects, one for each column in the ListView.  These
    /// indicate the type of data stored in each column.  Supported types are
    /// String, Int32, Single, Double, and DateTime.
    /// </param>
    ///
    /// <remarks>
    /// Sorting does not occur until <see cref="Sort" /> is called or the user
    /// clicks a column header.
    /// </remarks>
    ///
    /// <remarks>
    /// The <see cref="ListView.Columns" /> collection must already be
    /// populated before this method is called.
    /// </remarks>
    //*************************************************************************

    public void
    EnableSorting
    (
        params Type[] aoColumnTypes
    )
    {
        Debug.Assert(this.Sorting == SortOrder.None);
        Debug.Assert(aoColumnTypes.Length > 0);
        Debug.Assert(aoColumnTypes.Length == this.Columns.Count);

        m_iSortColumn = NoColumn;
        m_bSortAscending = true;

        // Convert the types to enumerated values that can be tested quickly
        // in the Compare() method.

        Int32 iColumns = aoColumnTypes.Length;
        m_aeColumnTypes = new ColumnType[iColumns];

        for (Int32 i = 0; i < iColumns; i++)
            m_aeColumnTypes[i] = TypeToColumnType( aoColumnTypes[i] );

        // Handle the ColumnClick event.

        this.ColumnClick += new ColumnClickEventHandler(OnColumnClick);

        AssertValid();
    }

    //*************************************************************************
    //  Method: Sort()
    //
    /// <summary>
    /// Sorts the items in the ListView.
    /// </summary>
    ///
    /// <param name="iColumn">
    /// Zero-based column index to sort on.
    /// </param>
    ///
    /// <param name="bSortAscending">
    /// true to sort in ascending order, false for descending.
    /// </param>
    //*************************************************************************

    public void
    Sort
    (
        Int32 iColumn,
        Boolean bSortAscending
    )
    {
        Debug.Assert(iColumn >= 0);
        Debug.Assert(iColumn < m_aeColumnTypes.Length);
        AssertValid();

        m_iSortColumn = iColumn;
        m_bSortAscending = bSortAscending;

        // Tell the base class to sort itself using this derived class as the
        // IComparer interface.

        this.ListViewItemSorter = null;
        this.ListViewItemSorter = this;
    }

    //*************************************************************************
    //  Method: AddStandardContextMenuItems()
    //
    /// <summary>
    /// Adds the context menu items supported by this base class.
    /// </summary>
    ///
    /// <param name="oContextMenu">
    /// Context menu to add items to.
    /// </param>
    ///
    /// <param name="bAddSeparator">
    /// If true, a separator is added to the menu before the standard menu
    /// items.
    /// </param>
    ///
    /// <remarks>
    /// See the comments at the top of this class for more details on context
    /// menus.
    /// </remarks>
    //*************************************************************************

    public void
    AddStandardContextMenuItems
    (
        ContextMenu oContextMenu,
        Boolean bAddSeparator
    )
    {
        Debug.Assert(oContextMenu != null);
        AssertValid();

        Menu.MenuItemCollection oMenuItems = oContextMenu.MenuItems;

        if (bAddSeparator)
            oMenuItems.Add( new MenuItem("-") );

        if (m_iColumnToCopy != NoColumn)
        {
            // Add an item for copying the specified column of the right-
            // clicked ListViewItem.

            oMenuItems.Add( new MenuItem(
                "Cop&y " + this.Columns[m_iColumnToCopy].Text,
                new EventHandler(CopyColumn_Click) ) );
        }

        // Add items for copying the right-clicked ListViewItem and the entire
        // contents of the ListView.

        oMenuItems.Add( new MenuItem("Copy Ro&w",
            new EventHandler(CopyRow_Click) ) );

        oMenuItems.Add( new MenuItem("C&opy All Rows",
            new EventHandler(CopyAll_Click) ) );
    }

    //*************************************************************************
    //  Method: BeginUpdate()
    //
    /// <summary>
    /// Prevents the control from drawing or sorting until the
    /// <see cref="EndUpdate" /> method is called.
    /// </summary>
    //*************************************************************************

    public new void
    BeginUpdate()
    {
        AssertValid();

        base.BeginUpdate();

        // Turn off sorting.

        this.ListViewItemSorter = null;
    }

    //*************************************************************************
    //  Method: EndUpdate()
    //
    /// <summary>
    /// Resumes drawing and sorting of the control after drawing and sorting is
    /// suspended by the <see cref="BeginUpdate" /> method.
    /// </summary>
    //*************************************************************************

    public new void
    EndUpdate()
    {
        AssertValid();

        // If the ListView was sorted before BeginUpdate() was called, sort it
        // again.

        if (m_iSortColumn != NoColumn)
            this.ListViewItemSorter = this;

        base.EndUpdate();
    }

    //*************************************************************************
    //  Method: OnColumnClick()
    //
    /// <summary>
    /// Handles the ColumnClick event on the ListView.
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
    OnColumnClick
    (
        object sender,
        ColumnClickEventArgs e
    )
    {
        AssertValid();

        Int32 iColumn = e.Column;

        Boolean bSortAscending;

        // If the clicked column was the one most recently sorted, sort it
        // again in the opposite order.

        if (iColumn == m_iSortColumn)
            bSortAscending = !m_bSortAscending;
        else
            bSortAscending = true;

        Sort(iColumn, bSortAscending);
    }

    //*************************************************************************
    //  Method: Compare()
    //
    /// <summary>
    /// Compares two ListViewItem objects and returns a value indicating
    /// whether one is less than, equal to or greater than the other.
    /// </summary>
    ///
    /// <param name="oObject1">
    /// First ListViewItem to compare.
    /// </param>
    ///
    /// <param name="oObject2">
    /// Second ListViewItem to compare.
    /// </param>
    ///
    /// <returns>
    /// See <see cref="IComparer.Compare" />.
    /// </returns>
    //*************************************************************************

    public Int32
    Compare
    (
        Object oObject1,
        Object oObject2
    )
    {
        Debug.Assert(oObject1 != null);
        Debug.Assert(oObject2 != null);
        Debug.Assert(m_iSortColumn >= 0);
        Debug.Assert(m_iSortColumn < this.Columns.Count);

        // Cast the objects to ListViewItem.

        ListViewItem oListViewItem1 = (ListViewItem)oObject1;
        ListViewItem oListViewItem2 = (ListViewItem)oObject2;

        // Get the column text for each item.

        String sColumnText1, sColumnText2;

        if (m_iSortColumn == 0)
        {
            sColumnText1 = oListViewItem1.Text;
            sColumnText2 = oListViewItem2.Text;
        }
        else
        {
            sColumnText1 = oListViewItem1.SubItems[m_iSortColumn].Text;
            sColumnText2 = oListViewItem2.SubItems[m_iSortColumn].Text;
        }

        if (!m_bSortAscending)
        {
            // Swap the text.

            String sTemp = sColumnText1;
            sColumnText1 = sColumnText2;
            sColumnText2 = sTemp;
        }

        ColumnType eColumnType = ColumnToColumnType(m_iSortColumn);

        if (sColumnText1.Length == 0 || sColumnText2.Length == 0)
        {
            // One or both column texts are empty.  Force a String compare,
            // regardless of the actual column type.  (This allows a numeric or
            // DateTime column to contain an empty string.)

            eColumnType = ColumnType.String;
        }

        // Compare the column text.

        NumberStyles eNumberStyles = NumberStyles.AllowThousands;

        switch (eColumnType)
        {
            case ColumnType.String:

                return ( String.Compare(sColumnText1, sColumnText2, true) );

            case ColumnType.Int32:

                return ( Int32.Parse(sColumnText1, eNumberStyles).CompareTo(
                    Int32.Parse(sColumnText2, eNumberStyles) ) );

            case ColumnType.Single:

                return ( Single.Parse(sColumnText1).CompareTo(
                    Single.Parse(sColumnText2) ) );

            case ColumnType.Double:

                return ( Double.Parse(sColumnText1).CompareTo(
                    Double.Parse(sColumnText2) ) );

            case ColumnType.DateTime:

                return ( DateTime.Parse(sColumnText1).CompareTo(
                    DateTime.Parse(sColumnText2) ) );

            default:
                Debug.Assert(false);
                return (0);
        }
    }

    //*************************************************************************
    //  Method: TypeToColumnType()
    //
    /// <summary>
    /// Converts a Type object to a ColumnType enumerated value.
    /// </summary>
    ///
    /// <param name="oType">
    /// The type to convert.
    /// </param>
    ///
    /// <returns>
    /// A ColumnType enumerated value.
    /// </returns>
    //*************************************************************************

    protected ColumnType
    TypeToColumnType
    (
        Type oType
    )
    {
        if ( oType == typeof(String) )
            return (ColumnType.String);

        if ( oType == typeof(Int32) )
            return (ColumnType.Int32);

        if ( oType == typeof(Single) )
            return (ColumnType.Single);

        if ( oType == typeof(Double) )
            return (ColumnType.Double);

        if ( oType == typeof(DateTime) )
            return(ColumnType.DateTime);

        Debug.Assert(false);
        return (ColumnType.String);
    }

    //*************************************************************************
    //  Method: ColumnToColumnType()
    //
    /// <summary>
    /// Gets the ColumnType for a specified column.
    /// </summary>
    ///
    /// <param name="iColumn">
    /// Zero-based column index.
    /// </param>
    ///
    /// <returns>
    /// A ColumnType enumerated value.
    /// </returns>
    //*************************************************************************

    protected ColumnType
    ColumnToColumnType
    (
        Int32 iColumn
    )
    {
        Debug.Assert(iColumn >= 0);
        Debug.Assert(iColumn < m_aeColumnTypes.Length);

        return (m_aeColumnTypes[iColumn] );
    }

    //*************************************************************************
    //  Method: OnMouseUp()
    //
    /// <summary>
    /// Handles the MouseUp event.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    ///
    /// <remarks>
    /// See the comments at the top of this class for details on context menus.
    /// </remarks>
    //*************************************************************************

    protected override void
    OnMouseUp
    (
        MouseEventArgs e
    )
    {
        // Subtle point: m_oRightClickedItem must be set even if
        // m_bUseStandardContextMenu is false.  That's because the derived
        // class may use a custom context menu and call
        // AddStandardContextMenuItems() to add the standard items.  The
        // standard item handlers in this base class use m_oRightClickedItem.

        m_oRightClickedItem = GetRightClickedItem(e);

        if (m_bUseStandardContextMenu)
        {
            // The standard context menu supported by this base class should be
            // used.  Was an item right-clicked?

            if (m_oRightClickedItem != null)
            {
                // Yes.  Create and show a context menu containing items
                // supported by this base class.

                ContextMenu oContextMenu = new ContextMenu();
                AddStandardContextMenuItems(oContextMenu, false);
                oContextMenu.Show( this, new System.Drawing.Point(e.X, e.Y) );
            }
        }

        // Let the base class call any registered delegates.  This may include
        // a MouseUp handler that creates and shows a custom context menu.

        base.OnMouseUp(e);
    }

    //*************************************************************************
    //  Method: CopyColumn_Click()
    //
    /// <summary>
    /// Gets called when the context menu item for copying a column is clicked.
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
    CopyColumn_Click
    (
        object sender,
        System.EventArgs e
    )
    {
        AssertValid();
        Debug.Assert(m_oRightClickedItem != null);
        Debug.Assert(m_iColumnToCopy >= 0);

        String sClipboardString;

        if (m_iColumnToCopy == 0)
        {
            sClipboardString = m_oRightClickedItem.Text;
        }
        else
        {
            sClipboardString =
                m_oRightClickedItem.SubItems[m_iColumnToCopy].Text;
        }

        Clipboard.SetDataObject(sClipboardString);

        m_oRightClickedItem = null;
    }

    //*************************************************************************
    //  Method: CopyRow_Click()
    //
    /// <summary>
    /// Gets called when the Copy Row menu item on the context menu is clicked.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    ///
    /// <remarks>
    /// The string copied to the clipboard uses tabs to delimit the columns of
    /// the right-clicked ListViewItem.  Sample: "Name\tAddress"
    /// </remarks>
    //*************************************************************************

    protected void
    CopyRow_Click
    (
        object sender,
        System.EventArgs e
    )
    {
        AssertValid();
        Debug.Assert(m_oRightClickedItem != null);

        Clipboard.SetDataObject( ItemToClipboardString(m_oRightClickedItem) );

        m_oRightClickedItem = null;
    }

    //*************************************************************************
    //  Method: CopyAll_Click()
    //
    /// <summary>
    /// Gets called when the Copy All menu item on the context menu is clicked.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    ///
    /// <remarks>
    /// The string copied to the clipboard contains one line for the column
    /// headers and one line for each ListViewItem.  The line for a
    /// ListViewItem uses tabs to delimit the item columns.  Sample:
    /// "Name\tAddress"
    /// </remarks>
    //*************************************************************************

    protected void
    CopyAll_Click
    (
        object sender,
        System.EventArgs e
    )
    {
        AssertValid();

        StringBuilder oStringBuilder = new StringBuilder(1000);

        // Add the column headers delimited by tabs.

        foreach (ColumnHeader oColumnHeader in this.Columns)
        {
            oStringBuilder.Append(oColumnHeader.Text);
            oStringBuilder.Append('\t');
        }

        // Remove the final tab.

        oStringBuilder.Remove(oStringBuilder.Length - 1, 1);

        oStringBuilder.Append(Environment.NewLine);

        // Add a line for each item, delimited by newlines.

        foreach (ListViewItem oListViewItem in this.Items)
        {
            ItemToClipboardString(oListViewItem, oStringBuilder);
            oStringBuilder.Append(Environment.NewLine);
        }

        Clipboard.SetDataObject( oStringBuilder.ToString() );

        m_oRightClickedItem = null;
    }

    //*************************************************************************
    //  Method: ItemToClipboardString()
    //
    /// <summary>
    /// Converts a ListViewItem to a string suitable for copying to the
    /// clipboard.
    /// </summary>
    ///
    /// <param name="oListViewItem">
    /// Item to get a clipboard string for.
    /// </param>
    ///
    /// <returns>
    /// A string suitable for copying to the clipboard.  The columns are
    /// delimited by tabs.  Sample: "Name\tAddress"
    /// </returns>
    //*************************************************************************

    protected String
    ItemToClipboardString
    (
        ListViewItem oListViewItem
    )
    {
        Debug.Assert(oListViewItem != null);

        StringBuilder oStringBuilder = new StringBuilder(100);
        ItemToClipboardString(oListViewItem, oStringBuilder);

        return ( oStringBuilder.ToString() );
    }

    //*************************************************************************
    //  Method: ItemToClipboardString()
    //
    /// <summary>
    /// Converts a ListViewItem to a string suitable for copying to the
    /// clipboard and appends the string to a StringBuilder.
    /// </summary>
    ///
    /// <param name="oListViewItem">
    /// Item to get a clipboard string for.
    /// </param>
    ///
    /// <param name="oStringBuilder">
    /// Object to append the string to.
    /// </param>
    ///
    /// <remarks>
    /// The appended string uses tabs to delimit the columns of the
    /// ListViewItem.  Sample: "Name\tAddress"
    /// </remarks>
    //*************************************************************************

    protected void
    ItemToClipboardString
    (
        ListViewItem oListViewItem,
        StringBuilder oStringBuilder
    )
    {
        Debug.Assert(oListViewItem != null);
        Debug.Assert(oStringBuilder != null);

        Int32 iColumns = this.Columns.Count;

        ListViewItem.ListViewSubItemCollection oSubItems =
            oListViewItem.SubItems;

        if (oSubItems.Count != iColumns)
        {
            throw new InvalidOperationException(
                "ListViewPlus.ItemToClipBoardString: An item does not have"
                + " enough subitems.  There should be one subitem for each"
                + " column in the ListView.   Empty strings can be used for"
                + " subitems, but missing subitems are not allowed."

                );
        }

        oStringBuilder.Append(oListViewItem.Text);

        for (Int32 i = 1; i < iColumns; i++)
        {
            oStringBuilder.Append('\t');
            oStringBuilder.Append(oListViewItem.SubItems[i].Text);
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

    public virtual void
    AssertValid()
    {
        Debug.Assert(this != null);
        Debug.Assert(this.Sorting == SortOrder.None);

        if (m_aeColumnTypes != null)
        {
            Debug.Assert(m_aeColumnTypes.Length > 0);
            Debug.Assert(m_aeColumnTypes.Length == this.Columns.Count);
        }

        Debug.Assert(m_iSortColumn == NoColumn ||
            m_iSortColumn < this.Columns.Count);

        Debug.Assert(m_iColumnToCopy == NoColumn ||
            m_iColumnToCopy < this.Columns.Count);
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// Used with column index variables to indicate "no column".

    public static Int32 NoColumn = -1;


    //*************************************************************************
    //  Protected enumerations
    //*************************************************************************

    /// Type of data in the column to sort.

    protected enum ColumnType
    {
        ///
        String,
        ///
        Int32,
        ///
        Single,
        ///
        Double,
        ///
        DateTime,
    }

    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Type of data in each column.

    protected ColumnType [] m_aeColumnTypes;

    /// Zero-based index of the column to sort, or NoColumn if sorting is
    /// disabled.

    protected Int32 m_iSortColumn;

    /// true to sort in ascending order, false for descending.

    protected Boolean m_bSortAscending;

    /// true if the default right-click context menu should be used.

    protected Boolean m_bUseStandardContextMenu;

    /// Zero-based index of the column for which a copy command should be
    /// included on context menus, or NoColumn.

    protected Int32 m_iColumnToCopy;

    /// Right-clicked item, or null.

    protected ListViewItem m_oRightClickedItem;
}

}
