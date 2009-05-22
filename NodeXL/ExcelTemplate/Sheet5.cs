
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.NodeXL.Visualization.Wpf;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: Sheet5
//
/// <summary>
/// Represents the cluster worksheet.
/// </summary>
//*****************************************************************************

public partial class Sheet5
{
    //*************************************************************************
    //  Event: ClusterSelectionChanged
    //
    /// <summary>
    /// Occurs when the selection state of the cluster table changes.
    /// </summary>
    //*************************************************************************

    public event TableSelectionChangedEventHandler ClusterSelectionChanged;


    //*************************************************************************
    //  Method: FireClusterSelectionChanged()
    //
    /// <summary>
    /// Fires the <see cref="ClusterSelectionChanged" /> event if appropriate.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    FireClusterSelectionChanged
    (
        TableSelectionChangedEventArgs e
    )
    {
        Debug.Assert(e != null);
        AssertValid();

        TableSelectionChangedEventHandler oClusterSelectionChanged =
            this.ClusterSelectionChanged;

        if (oClusterSelectionChanged != null)
        {
            try
            {
                oClusterSelectionChanged(this, e);
            }
            catch (Exception oException)
            {
                // If exceptions aren't caught here, Excel consumes them
                // without indicating that anything is wrong.

                ErrorUtil.OnException(oException);
            }
        }
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

        m_oSheetHelper = new SheetHelper(this, this.Clusters);

        m_oSheetHelper.TableSelectionChanged +=
            new TableSelectionChangedEventHandler(
                m_oSheetHelper_TableSelectionChanged);

        m_oSheetHelper.Sheet_Startup();
    }

    //*************************************************************************
    //  Method: m_oSheetHelper_TableSelectionChanged()
    //
    /// <summary>
    /// Handles the TableSelectionChanged event on the m_oSheetHelper object.
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
    m_oSheetHelper_TableSelectionChanged
    (
        object sender,
        TableSelectionChangedEventArgs e
    )
    {
        AssertValid();

        FireClusterSelectionChanged(e);
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
            ClusterTableColumnNames.VertexColor, null);

        if (e.VisualAttributeSet)
        {
            return;
        }

        if (e.VisualAttribute == VisualAttributes.VertexShape)
        {
            Debug.Assert(e.AttributeValue is VertexShape);

            ExcelUtil.SetVisibleSelectedTableColumnData(
                this.Clusters.InnerObject, oSelectedRange,
                ClusterTableColumnNames.VertexShape,

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
