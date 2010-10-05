
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Visualization.Wpf;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: Sheet5
//
/// <summary>
/// Represents the group worksheet.
/// </summary>
//*****************************************************************************

public partial class Sheet5
{
    //*************************************************************************
    //  Property: SheetHelper
    //
    /// <summary>
    /// Gets the object that does most of the work for this class.
    /// </summary>
    ///
    /// <value>
    /// A SheetHelper object.
    /// </value>
    //*************************************************************************

    public SheetHelper
    SheetHelper
    {
        get
        {
            AssertValid();

            return (m_oSheetHelper);
        }
    }

    //*************************************************************************
    //  Method: GetSelectedGroupNames()
    //
    /// <summary>
    /// Gets the group names for the rows in the table that have at least one
    /// cell selected.
    /// </summary>
    ///
    /// <returns>
    /// A collection of group names.
    /// </returns>
    ///
    /// <remarks>
    /// This method activates the worksheet if it isn't already activated.
    /// </remarks>
    //*************************************************************************

    public ICollection<String>
    GetSelectedGroupNames()
    {
        AssertValid();

        return ( m_oSheetHelper.GetSelectedStringColumnValues(
            GroupTableColumnNames.Name ) );
    }

    //*************************************************************************
    //  Method: SelectGroups()
    //
    /// <summary>
    /// Selects a specified collection of groups.
    /// </summary>
    ///
    /// <param name="groupNames">
    /// Names of the groups to select.
    /// </param>
    ///
    /// <remarks>
    /// This method activates the worksheet if it isn't already activated.
    /// </remarks>
    //*************************************************************************

    public void
    SelectGroups
    (
        ICollection<String> groupNames
    )
    {
        Debug.Assert(groupNames != null);
        AssertValid();

        m_oSheetHelper.SelectTableRowsByColumnValues<String>(
            GroupTableColumnNames.Name, groupNames,
            ExcelUtil.TryGetNonEmptyStringFromCell
            );
    }

    //*************************************************************************
    //  Method: Sheet5_Startup()
    //
    /// <summary>
    /// Handles the Startup event on the worksheet.
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
    Sheet5_Startup
    (
        object sender,
        System.EventArgs e
    )
    {
        Globals.ThisWorkbook.SetVisualAttribute2 +=
            new SetVisualAttributeEventHandler(
                this.ThisWorkbook_SetVisualAttribute2);

        // Create the helper object.

        m_oSheetHelper = new SheetHelper(this, this.Groups);
    }

    //*************************************************************************
    //  Method: Sheet5_Shutdown()
    //
    /// <summary>
    /// Handles the Shutdown event on the worksheet.
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
    Sheet5_Shutdown
    (
        object sender,
        System.EventArgs e
    )
    {
        AssertValid();

        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: ThisWorkbook_SetVisualAttribute2()
    //
    /// <summary>
    /// Handles the SetVisualAttribute2 event on ThisWorkbook.
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
    ThisWorkbook_SetVisualAttribute2
    (
        Object sender,
        SetVisualAttributeEventArgs e
    )
    {
        Debug.Assert(e != null);
        AssertValid();

        Microsoft.Office.Interop.Excel.Range oSelectedRange;

        if ( e.VisualAttributeSet ||
            !m_oSheetHelper.TryGetSelectedRange(out oSelectedRange) )
        {
            return;
        }

        // See if the specified attribute is set by the helper class.

        m_oSheetHelper.SetVisualAttribute(e, oSelectedRange,
            GroupTableColumnNames.VertexColor, null);

        if (e.VisualAttributeSet)
        {
            return;
        }

        if (e.VisualAttribute == VisualAttributes.VertexShape)
        {
            Debug.Assert(e.AttributeValue is VertexShape);

            ExcelUtil.SetVisibleSelectedTableColumnData(
                this.Groups.InnerObject, oSelectedRange,
                GroupTableColumnNames.VertexShape,

                ( new VertexShapeConverter() ).GraphToWorkbook(
                    (VertexShape)e.AttributeValue)
                );

            e.VisualAttributeSet = true;
        }
    }


    #region VSTO Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InternalStartup()
    {
        this.Startup += new System.EventHandler(Sheet5_Startup);
        this.Shutdown += new System.EventHandler(Sheet5_Shutdown);
    }

    #endregion


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
        Debug.Assert(m_oSheetHelper != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Helper object.

    private SheetHelper m_oSheetHelper;
}
}
