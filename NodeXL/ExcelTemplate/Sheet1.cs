

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: Sheet1
//
/// <summary>
/// Represents the edge worksheet.
/// </summary>
//*****************************************************************************

public partial class Sheet1
{
    //*************************************************************************
    //  Event: EdgeSelectionChanged
    //
    /// <summary>
    /// Occurs when the selection state of the edge table changes.
    /// </summary>
    //*************************************************************************

    public event TableSelectionChangedEventHandler EdgeSelectionChanged;


    //*************************************************************************
    //  Method: Sheet1_Startup()
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
    Sheet1_Startup
    (
        object sender,
        System.EventArgs e
    )
    {
        // Create the object that does most of the work for this class.

        m_oSheets1And2Helper = new Sheets1And2Helper(this, this.Edges);

        m_oSheets1And2Helper.TableSelectionChanged +=
            new TableSelectionChangedEventHandler(
                m_oSheets1And2Helper_TableSelectionChanged);

        m_oSheets1And2Helper.Sheet_Startup(sender, e);

        Globals.ThisWorkbook.SetVisualAttribute2 +=
            new SetVisualAttributeEventHandler(
                this.ThisWorkbook_SetVisualAttribute2);

        AssertValid();
    }

    //*************************************************************************
    //  Method: m_oSheets1And2Helper_TableSelectionChanged()
    //
    /// <summary>
    /// Handles the TableSelectionChanged event on the m_oSheets1And2Helper
    /// object.
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
    m_oSheets1And2Helper_TableSelectionChanged
    (
        object sender,
        TableSelectionChangedEventArgs e
    )
    {
        AssertValid();

        FireEdgeSelectionChanged(e);
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
            !m_oSheets1And2Helper.TryGetSelectedRange(out oSelectedRange) )
        {
            return;
        }

        // See if the specified attribute is set by the helper class.

        m_oSheets1And2Helper.SetVisualAttribute(e, oSelectedRange,
            EdgeTableColumnNames.Color, CommonTableColumnNames.Alpha);

        if (e.VisualAttributeSet)
        {
            return;
        }

        if (e.VisualAttribute == VisualAttributes.EdgeWidth)
        {
            EdgeWidthDialog oEdgeWidthDialog = new EdgeWidthDialog();

            if (oEdgeWidthDialog.ShowDialog() == DialogResult.OK)
            {
                ExcelUtil.SetVisibleSelectedTableColumnData(
                    this.Edges.InnerObject, oSelectedRange,
                    EdgeTableColumnNames.Width, oEdgeWidthDialog.EdgeWidth);

                e.VisualAttributeSet = true;
            }
        }
        else if (e.VisualAttribute == VisualAttributes.EdgeVisibility)
        {
            Debug.Assert(e.AttributeValue is EdgeWorksheetReader.Visibility);

            ExcelUtil.SetVisibleSelectedTableColumnData(
                this.Edges.InnerObject, oSelectedRange,
                CommonTableColumnNames.Visibility,

                ( new EdgeVisibilityConverter() ).GraphToWorkbook(
                    (EdgeWorksheetReader.Visibility)e.AttributeValue)
                );

            e.VisualAttributeSet = true;
        }
    }

    //*************************************************************************
    //  Method: Sheet1_Shutdown()
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
    Sheet1_Shutdown
    (
        object sender,
        System.EventArgs e
    )
    {
        AssertValid();

        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: FireEdgeSelectionChanged()
    //
    /// <summary>
    /// Fires the <see cref="EdgeSelectionChanged" /> event if appropriate.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    FireEdgeSelectionChanged
    (
        TableSelectionChangedEventArgs e
    )
    {
        Debug.Assert(e != null);
        AssertValid();

        TableSelectionChangedEventHandler oEdgeSelectionChanged =
            this.EdgeSelectionChanged;

        if (oEdgeSelectionChanged != null)
        {
            try
            {
                oEdgeSelectionChanged(this, e);
            }
            catch (Exception oException)
            {
                // If exceptions aren't caught here, Excel consumes them
                // without indicating that anything is wrong.

                ErrorUtil.OnException(oException);
            }
        }
    }


    #region VSTO Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InternalStartup()
    {
        this.Startup += new System.EventHandler(Sheet1_Startup);
        this.Shutdown += new System.EventHandler(Sheet1_Shutdown);
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
        Debug.Assert(m_oSheets1And2Helper != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object that does most of the work for this class.

    private Sheets1And2Helper m_oSheets1And2Helper;
}

}
