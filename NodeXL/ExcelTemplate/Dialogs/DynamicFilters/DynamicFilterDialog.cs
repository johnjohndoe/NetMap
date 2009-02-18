

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Reflection;
using System.Configuration;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.WinFormsControls;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: DynamicFilterDialog
//
/// <summary>
/// Displays a dynamic filter for each column in the edge and vertex worksheets
/// that can be filtered on.
/// </summary>
///
/// <remarks>
/// A dynamic filter is a control that can be adjusted to selectively show and
/// hide edges and vertices in the graph in real time.
///
/// <para>
/// This dialog shows a RangeTrackBar or DateTimeRangeTrackBar control for each
/// numeric or date/time column in the edge and vertex worksheets.  The
/// available range of the track bar is set to the range of numbers in the
/// column.  As the user changes the track bar's selected range, the edges or
/// vertices that fall outside the selected range are hidden in the TaskPane in
/// real time.
/// </para>
///
/// <para>
/// The filtering is implemented using an Excel formula this dialog writes to
/// a Dynamic Range column added to the edge table, and another formula written
/// to the vertex table.  The formula yields a Boolean that specifies whether
/// the edge or vertex should be shown in the graph.  The TaskPane reads these
/// columns and shows and hide the graph's edges and vertices based on the
/// Boolean values.
/// </para>
///
/// <para>
/// The Excel formula looks like this, in pseudocode:
/// </para>
///
/// <para>
/// if (column N of this row is between X and Y) then TRUE else FALSE
/// </para>
///
/// <para>
/// The X and Y values come from a hidden table that this dialog populates.
/// The hidden table has one row per dynamic filter, and the columns are the
/// filtered table name, the filtered column name, and the filter's current
/// selected range.  As the user change's a track bar control's selected range,
/// this dialog updates the appropriate row in the hidden table, tells Excel to
/// recalculate the Dynamic Filter column in the edge or vertex worksheet, and
/// fires a <see cref="DynamicFilterColumnsChanged" /> event telling the
/// TaskPane to read the columns.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class DynamicFilterDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: DynamicFilterDialog()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
    /// cref="DynamicFilterDialog" /> class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="DynamicFilterDialog" /> class with a workbook.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph contents.
    /// </param>
    //*************************************************************************

    public DynamicFilterDialog
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    : this()
    {
        Debug.Assert(workbook != null);

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oDynamicFilterDialogUserSettings =
            new DynamicFilterDialogUserSettings(this);

        m_oWorkbook = workbook;

        m_oExcelCalculationRestorer =
            new ExcelCalculationRestorer(workbook.Application);

        m_oExcelCalculationRestorer.TimerIntervalMs =
            CalculationRestorerTimerIntervalMs;

        m_bHandleRangeTrackBarEvents = false;

        m_oRangeTrackBarTimer = new Timer();
        m_oRangeTrackBarTimer.Interval = RangeTrackBarTimerIntervalMs;

        m_oRangeTrackBarTimer.Tick += new EventHandler(
            this.m_oRangeTrackBarTimer_Tick);

        m_oDynamicFilterSettings = null;
        m_oEdgeDynamicFilterColumnData = null;
        m_oVertexDynamicFilterColumnData = null;

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: DynamicFilterDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="DynamicFilterDialog" /> class for the Visual Studio
    /// designer.
    /// </summary>
    ///
    /// <remarks>
    /// Do not use this constructor.  It is for use by the Visual Studio
    /// designer only.
    /// </remarks>
    //*************************************************************************

    public DynamicFilterDialog()
    {
        InitializeComponent();

        // AssertValid();
    }

    //*************************************************************************
    //  Event: DynamicFilterColumnsChanged
    //
    /// <summary>
    /// Occurs when one or more dynamic filter columns change.
    /// </summary>
    //*************************************************************************

    public event DynamicFilterColumnsChangedEventHandler
        DynamicFilterColumnsChanged;


    //*************************************************************************
    //  Property: Application
    //
    /// <summary>
    /// Gets the Excel Applicaton object.
    /// </summary>
    ///
    /// <value>
    /// The Excel Application object.
    /// </value>
    //*************************************************************************

    protected Microsoft.Office.Interop.Excel.Application
    Application
    {
        get
        {
            AssertValid();

            return (m_oWorkbook.Application);
        }
    }

    //*************************************************************************
    //  Method: InitializeDynamicFilters()
    //
    /// <summary>
    /// Initializes the dialog's dynamic filter controls.
    /// </summary>
    //*************************************************************************

    protected void
    InitializeDynamicFilters()
    {
        AssertValid();

        m_bHandleRangeTrackBarEvents = false;

        m_oEdgeDynamicFilterColumnData =
            InitializeDynamicFiltersForOneTable(WorksheetNames.Edges,
                TableNames.Edges, grpEdgeFilters);

        m_oVertexDynamicFilterColumnData =
            InitializeDynamicFiltersForOneTable(WorksheetNames.Vertices,
                TableNames.Vertices, grpVertexFilters);

        grpVertexFilters.Top = grpEdgeFilters.Bottom + GroupBoxBottomMargin;

        grpEdgeFilters.Visible = grpVertexFilters.Visible = true;

        m_bHandleRangeTrackBarEvents = true;
    }

    //*************************************************************************
    //  Method: InitializeDynamicFiltersForOneTable()
    //
    /// <summary>
    /// Initializes the dialog's dynamic filter controls for one table.
    /// </summary>
    ///
    /// <param name="sWorksheetName">
    /// Name of the worksheet containing the table.
    /// </param>
    ///
    /// <param name="sTableName">
    /// Name of the table.
    /// </param>
    ///
    /// <param name="oGroupBox">
    /// GroupBox into which the dynamic filters should be placed.
    /// </param>
    ///
    /// <returns>
    /// Data range of the table column that contains the dynamic filter
    /// formulas.
    /// </returns>
    //*************************************************************************

    protected Microsoft.Office.Interop.Excel.Range
    InitializeDynamicFiltersForOneTable
    (
        String sWorksheetName,
        String sTableName,
        GroupBox oGroupBox
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sWorksheetName) );
        Debug.Assert( !String.IsNullOrEmpty(sTableName) );
        Debug.Assert(oGroupBox != null);
        AssertValid();

        Control.ControlCollection oChildControls = oGroupBox.Controls;
        Int32 iChildControls = oChildControls.Count;

        // Clear the GroupBox of all child controls except the first child,
        // which is a Label that says "no columns."

        for (Int32 i = iChildControls - 1; i > 0; i--)
        {
            oChildControls.RemoveAt(i);
        }

        // Get a collection of dynamic filter parameters, one for each column
        // in the table that can be filtered on.

        ICollection<DynamicFilterParameters> oTableParameters =
            DynamicFilterUtil.GetDynamicFilterParameters(
                m_oWorkbook, sWorksheetName, sTableName
                );

        // If there are no filterable columns, the "no columns" Label should be
        // made visible.  It is now the only child control.

        Debug.Assert(oChildControls.Count == 1);
        Debug.Assert(oChildControls[0] is Label);

        Label oNoFilterLabel = (Label)oChildControls[0];
        oNoFilterLabel.Visible = (oTableParameters.Count == 0);

        // Assuming that there are filterable columns, use the label's
        // coordinates as a starting point for dynamically added controls.

        Int32 iX = oNoFilterLabel.Location.X;
        Int32 iY = oNoFilterLabel.Location.Y;

        // An Excel formula in a new Dynamic Filter column will be used for
        // dynamic filtering.  Here is an example of what the final formula
        // will look like:
        //
        // = IF(AND([ColumnName1] >= $P$2, [ColumnName1] <= $Q$2,
        // [ColumnName2] >= Misc!$P$3, [ColumnName2] <= Misc!$Q$3), TRUE, FALSE)
        //
        // A StringBuilder is used to collect the conditions between the AND()
        // parentheses, the part that looks like this:
        //
        // [ColumnName1] >= Misc!$P$2, [ColumnName1] <= Misc!$Q$2,
        // [ColumnName2] >= Misc!$P$3, [ColumnName2] <= Misc!$Q$3

        StringBuilder oDynamicFilterConditions = new StringBuilder();

        Int32 iDynamicFilters = 0;

        foreach (DynamicFilterParameters oDynamicFilterParameters in
            oTableParameters)
        {
            switch (oDynamicFilterParameters.GetType().Name)
            {
                case "NumericFilterParameters":
                case "DateTimeFilterParameters":

                    // Add a label and a range track bar control to the
                    // GroupBox.

                    AddLabelToGroupBox(oDynamicFilterParameters, oGroupBox,
                        iX, ref iY);

                    Debug.Assert(oDynamicFilterParameters is
                        NumericFilterParameters);

                    AddRangeTrackBarToGroupBox(
                        (NumericFilterParameters)oDynamicFilterParameters,
                        sTableName, oGroupBox, oDynamicFilterConditions,
                        iX, ref iY);

                    break;

                default:

                    Debug.Assert(false);
                    break;
            }

            iDynamicFilters++;

            if (iDynamicFilters == MaximumDynamicFiltersPerTable)
            {
                break;
            }
        }

        oGroupBox.Height = iY;

        // Add a TRUE filter condition.  If the conditions are empty (which
        // happens when there are no filterable columns), this results in the
        // following final formula, which works fine:
        //
        // = IF(AND(TRUE), TRUE, FALSE)
        //
        // Otherwise, the added TRUE condition avoids an error caused by the
        // final comma in the final condition, like this:
        //
        // = IF(AND([ColumnName1] >= Misc!$P$2,
        // [ColumnName1] <= Misc!$Q$2, TRUE), TRUE, FALSE)

        oDynamicFilterConditions.Append("TRUE");

        String sDynamicFilterFormula = String.Format(

            "= IF(AND({0}), TRUE, FALSE)"
            ,
            oDynamicFilterConditions
            );

        // Add the dynamic filter formula to the table.

        return ( AddDynamicFilterFormulaToTable(sWorksheetName, sTableName,
            sDynamicFilterFormula) );
    }

    //*************************************************************************
    //  Method: ResetAllDynamicFilters()
    //
    /// <summary>
    /// Resets all the dynamic filters to their widest settings.
    /// </summary>
    //*************************************************************************

    protected void
    ResetAllDynamicFilters()
    {
        AssertValid();

        // Turn off automatic recalculation.

        SetManualCalculation();

        // Stop multiple IDynamicFilterRangeTrackBar.SelectedRangeChanged
        // events from being handled, which would be inefficient.  Instead,
        // process all the changes at once when done looping.

        m_bHandleRangeTrackBarEvents = false;

        // Loop through all child controls of both group boxes.

        foreach ( GroupBox oGroupBox in
            new GroupBox[] {grpEdgeFilters, grpVertexFilters} )
        {
            foreach (Control oControl in oGroupBox.Controls)
            {
                if (oControl is IDynamicFilterRangeTrackBar)
                {
                    // Set the selected range to the entire available range.

                    IDynamicFilterRangeTrackBar oDynamicFilterRangeTrackBar =
                        (IDynamicFilterRangeTrackBar)oControl;

                    oDynamicFilterRangeTrackBar.SetSelectedRange(
                        oDynamicFilterRangeTrackBar.AvailableMinimum,
                        oDynamicFilterRangeTrackBar.AvailableMaximum
                        );

                    // Update the persisted settings for this range track bar.
                    // This updates two cells in the dynamic filter settings
                    // table.

                    Debug.Assert(oControl.Tag is ColumnInfo);

                    ColumnInfo oColumnInfo = (ColumnInfo)oControl.Tag;

                    String sSelectedMinimumAddress, sSelectedMaximumAddress;

                    m_oDynamicFilterSettings.SetSettings(
                        oColumnInfo.TableName, oColumnInfo.ColumnName,
                        oDynamicFilterRangeTrackBar.SelectedMinimum,
                        oDynamicFilterRangeTrackBar.SelectedMaximum,
                        out sSelectedMinimumAddress,
                        out sSelectedMaximumAddress);
                }
            }
        }

        m_bHandleRangeTrackBarEvents = true;

        // Now recalculate the dynamic filter columns using the values just
        // written to the dynamic filter settings table.

        RecalculateDynamicFilterColumns(DynamicFilterColumns.AllTables);

        FireDynamicFilterColumnsChanged(DynamicFilterColumns.AllTables);
    }

    //*************************************************************************
    //  Method: AddLabelToGroupBox()
    //
    /// <summary>
    /// Adds a dynamic filter control Label to a GroupBox.
    /// </summary>
    ///
    /// <param name="oDynamicFilterParameters">
    /// Parameters for the dynamic filter control.
    /// </param>
    ///
    /// <param name="oGroupBox">
    /// GroupBox to which the Label should be added.
    /// </param>
    ///
    /// <param name="iX">
    /// x-coordinate of the Label.
    /// </param>
    ///
    /// <param name="iY">
    /// y-coordinate of the Label.  When this method returns, this is set to
    /// the y-coordinate to use for the dynamic filter control.
    /// </param>
    //*************************************************************************

    protected void
    AddLabelToGroupBox
    (
        DynamicFilterParameters oDynamicFilterParameters,
        GroupBox oGroupBox,
        Int32 iX,
        ref Int32 iY
    )
    {
        Debug.Assert(oGroupBox != null);
        Debug.Assert(oDynamicFilterParameters != null);
        AssertValid();

        Label oLabel = new Label();
        oLabel.Text = oDynamicFilterParameters.ColumnName + ":";
        oLabel.Location = new Point(iX, iY);
        oLabel.Margin = Padding.Empty;
        oLabel.Height = 15;
        oLabel.Width = oGroupBox.Width - (2 * iX);

        oLabel.Anchor =
            AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

        oLabel.AutoEllipsis = true;
        oGroupBox.Controls.Add(oLabel);

        iY += oLabel.Height + LabelBottomMargin;
    }

    //*************************************************************************
    //  Method: AddRangeTrackBarToGroupBox()
    //
    /// <summary>
    /// Adds a range track bar control to a GroupBox.
    /// </summary>
    ///
    /// <param name="oNumericFilterParameters">
    /// Parameters for the range track bar.
    /// </param>
    ///
    /// <param name="sTableName">
    /// Name of the table that corresponds to the group box.
    /// </param>
    ///
    /// <param name="oGroupBox">
    /// GroupBox to which the range track bar should be added.
    /// </param>
    ///
    /// <param name="oDynamicFilterConditions">
    /// The Excel conditions being built for filtering.
    /// </param>
    ///
    /// <param name="iX">
    /// x-coordinate of the range track bar.
    /// </param>
    ///
    /// <param name="iY">
    /// y-coordinate of the range track bar.  When this method returns, this is
    /// set to the y-coordinate to use for the next Label.
    /// </param>
    //*************************************************************************

    protected void
    AddRangeTrackBarToGroupBox
    (
        NumericFilterParameters oNumericFilterParameters,
        String sTableName,
        GroupBox oGroupBox,
        StringBuilder oDynamicFilterConditions,
        Int32 iX,
        ref Int32 iY
    )
    {
        Debug.Assert(oNumericFilterParameters != null);
        Debug.Assert( !String.IsNullOrEmpty(sTableName) );
        Debug.Assert(oGroupBox != null);
        Debug.Assert(oDynamicFilterConditions != null);
        AssertValid();

        String sColumnName = oNumericFilterParameters.ColumnName;

        // Get the ranges to use for the range track bar.

        Decimal decAvailableMinimum, decAvailableMaximum,
            decSelectedMinimum, decSelectedMaximum;

        String sSelectedMinimumAddress, sSelectedMaximumAddress;

        GetRangeTrackBarRanges(oNumericFilterParameters, sTableName,
            out decAvailableMinimum, out decAvailableMaximum,
            out decSelectedMinimum, out decSelectedMaximum,
            out sSelectedMinimumAddress, out sSelectedMaximumAddress);

        // Create a range track bar appropriate for the filter parameters.

        IDynamicFilterRangeTrackBar oDynamicFilterRangeTrackBar;

        if (oNumericFilterParameters is DateTimeFilterParameters)
        {
            oDynamicFilterRangeTrackBar =
                new DynamicFilterDateTimeRangeTrackBar();
        }
        else
        {
            oDynamicFilterRangeTrackBar = new DynamicFilterRangeTrackBar();
        }

        // Set the range track bar's ranges and custom properties.

        oDynamicFilterRangeTrackBar.SetAvailableRange(
            decAvailableMinimum, decAvailableMaximum);

        oDynamicFilterRangeTrackBar.SetSelectedRange(
            decSelectedMinimum, decSelectedMaximum);

        oDynamicFilterRangeTrackBar.SetCustomProperties(
            oNumericFilterParameters);

        // Position the range track bar.

        Debug.Assert(oDynamicFilterRangeTrackBar is Control);

        Control oControl = (Control)oDynamicFilterRangeTrackBar;

        oControl.Location = new Point(iX, iY);
        oControl.Width = oGroupBox.Width - (2 * iX);

        oControl.Anchor = AnchorStyles.Left | AnchorStyles.Top
            | AnchorStyles.Right;

        // Store the column information in the control's Tag.  This is needed
        // by the SelectedRangeChanged event handler.

        ColumnInfo oColumnInfo = new ColumnInfo();
        oColumnInfo.TableName = sTableName;
        oColumnInfo.ColumnName = sColumnName;
        oControl.Tag = oColumnInfo;

        // Make sure that a bunch of unwanted events during initialization are
        // avoided.

        Debug.Assert(!m_bHandleRangeTrackBarEvents);

        oDynamicFilterRangeTrackBar.SelectedRangeChanged +=
            new EventHandler(this.RangeTrackBar_SelectedRangeChanged);

        oGroupBox.Controls.Add(oControl);

        iY += oControl.Height + DynamicFilterControlBottomMargin;

        // Append a pair of conditions to the Excel conditions.  Sample
        // appended conditions:
        //
        // [ColumnName1] >= Misc!$P$2, [ColumnName1] <= Misc!$Q$2,

        oDynamicFilterConditions.AppendFormat(

            "[{0}] >= {1}!{2}, [{0}] <= {1}!{3},"
            ,
            sColumnName,
            WorksheetNames.Miscellaneous,
            sSelectedMinimumAddress,
            sSelectedMaximumAddress
            );
    }

    //*************************************************************************
    //  Method: AddDynamicFilterFormulaToTable()
    //
    /// <summary>
    /// Adds a dynamic filter formula to a table.
    /// </summary>
    ///
    /// <param name="sWorksheetName">
    /// Name of the worksheet containing the table.
    /// </param>
    ///
    /// <param name="sTableName">
    /// Name of the table.
    /// </param>
    ///
    /// <param name="sDynamicFilterFormula">
    /// The formula to add.
    /// </param>
    ///
    /// <returns>
    /// Data range of the table column that contains the dynamic filter
    /// formula.
    /// </returns>
    //*************************************************************************

    protected Microsoft.Office.Interop.Excel.Range
    AddDynamicFilterFormulaToTable
    (
        String sWorksheetName,
        String sTableName,
        String sDynamicFilterFormula
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sWorksheetName) );
        Debug.Assert( !String.IsNullOrEmpty(sTableName) );
        Debug.Assert( !String.IsNullOrEmpty(sDynamicFilterFormula) );
        AssertValid();

        // Get the table.

        Microsoft.Office.Interop.Excel.ListObject oTable;

        if ( !ExcelUtil.TryGetTable(m_oWorkbook, sWorksheetName, sTableName,
                out oTable) )
        {
            throw new WorkbookFormatException(
                "Can't find a required table."
                );
        }

        // Add a dynamic filter column to the table if it isn't already there.

        Microsoft.Office.Interop.Excel.Range oDynamicFilterColumnData =
            AddDynamicFilterTableColumn(oTable);

        // Check whether the entire table is empty.  (The default state of a
        // table in a new workbook is one empty row.)

        if ( !ExcelUtil.TableIsEmpty(oTable) )
        {
            // Set every cell to the filter formula.

            oDynamicFilterColumnData.set_Value(Missing.Value,
                sDynamicFilterFormula);
        }

        return (oDynamicFilterColumnData);
    }

    //*************************************************************************
    //  Method: GetRangeTrackBarRanges()
    //
    /// <summary>
    /// Gets the ranges to use for a range track bar.
    /// </summary>
    ///
    /// <param name="oNumericFilterParameters">
    /// Parameters for the range track bar.
    /// </param>
    ///
    /// <param name="sTableName">
    /// Name of the table that corresponds to the group box.
    /// </param>
    ///
    /// <param name="decAvailableMinimum">
    /// Where the minimum value in the available range gets stored.
    /// </param>
    ///
    /// <param name="decAvailableMaximum">
    /// Where the maximum value in the available range gets stored.
    /// </param>
    ///
    /// <param name="decSelectedMinimum">
    /// Where the minimum value in the selected range gets stored.
    /// </param>
    ///
    /// <param name="decSelectedMaximum">
    /// Where the maximum value in the selected range gets stored.
    /// </param>
    ///
    /// <param name="sSelectedMinimumAddress">
    /// Where the cell address of the minimum value in the selected range gets
    /// stored.
    /// </param>
    ///
    /// <param name="sSelectedMaximumAddress">
    /// Where the cell address of the maximum value in the selected range gets
    /// stored.
    /// </param>
    //*************************************************************************

    protected void
    GetRangeTrackBarRanges
    (
        NumericFilterParameters oNumericFilterParameters,
        String sTableName,
        out Decimal decAvailableMinimum,
        out Decimal decAvailableMaximum,
        out Decimal decSelectedMinimum,
        out Decimal decSelectedMaximum,
        out String sSelectedMinimumAddress,
        out String sSelectedMaximumAddress
    )
    {
        Debug.Assert(oNumericFilterParameters != null);
        Debug.Assert( !String.IsNullOrEmpty(sTableName) );
        AssertValid();

        String sColumnName = oNumericFilterParameters.ColumnName;

        decAvailableMinimum =
            (Decimal)oNumericFilterParameters.MinimumCellValue;

        decAvailableMaximum =
            (Decimal)oNumericFilterParameters.MaximumCellValue;

        // DynamicFilterUtil.GetDynamicFilterParameters() skips zero-width
        // ranges, so this should always be true.

        Debug.Assert(decAvailableMaximum > decAvailableMinimum);

        // If there are already saved selected range settings for this filter
        // and they don't exceed the actual cell values, use them.

        if (
            !m_oDynamicFilterSettings.TryGetSettings(sTableName, sColumnName,
                out decSelectedMinimum, out decSelectedMaximum,
                out sSelectedMinimumAddress, out sSelectedMaximumAddress)
            ||
            decSelectedMinimum < decAvailableMinimum
            ||
            decSelectedMinimum > decAvailableMaximum
            ||
            decSelectedMaximum < decAvailableMinimum
            ||
            decSelectedMaximum > decAvailableMaximum
            )
        {
            // There were no saved selected range settings, or they can't be
            // used.  Set the selected range to the entire available range,
            // then save the settings.

            decSelectedMinimum = decAvailableMinimum;
            decSelectedMaximum = decAvailableMaximum;

            m_oDynamicFilterSettings.SetSettings(sTableName, sColumnName,
                decSelectedMinimum, decSelectedMaximum,
                out sSelectedMinimumAddress, out sSelectedMaximumAddress);
        }

        Debug.Assert(decSelectedMaximum >= decSelectedMinimum);
        Debug.Assert( !String.IsNullOrEmpty(sSelectedMinimumAddress) );
        Debug.Assert( !String.IsNullOrEmpty(sSelectedMaximumAddress) );
    }

    //*************************************************************************
    //  Method: AddDynamicFilterTableColumn()
    //
    /// <summary>
    /// Adds a dynamic filter column to a table if it doesn't already exist.
    /// </summary>
    ///
    /// <param name="oTable">
    /// Table to add the column to.
    /// </param>
    ///
    /// <returns>
    /// A range containing the column's data.
    /// </returns>
    //*************************************************************************

    protected Microsoft.Office.Interop.Excel.Range
    AddDynamicFilterTableColumn
    (
        Microsoft.Office.Interop.Excel.ListObject oTable
    )
    {
        Debug.Assert(oTable != null);
        AssertValid();

        Microsoft.Office.Interop.Excel.ListColumn oDynamicFilterColumn;
        Microsoft.Office.Interop.Excel.Range oDynamicFilterColumnData;

        if (
            (
            !ExcelUtil.TryGetTableColumn(oTable,
                CommonTableColumnNames.DynamicFilter, out oDynamicFilterColumn)
            &&
            !ExcelUtil.TryAddTableColumn(oTable,
                CommonTableColumnNames.DynamicFilter,
                ExcelUtil.AutoColumnWidth, null, out oDynamicFilterColumn)
            )
            ||
            !ExcelUtil.TryGetTableColumnData(oTable,
                CommonTableColumnNames.DynamicFilter,
                out oDynamicFilterColumnData)
            )
        {
            throw new WorkbookFormatException(
                "Can't add a required column."
                );
        }

        return (oDynamicFilterColumnData);
    }

    //*************************************************************************
    //  Method: SetManualCalculation()
    //
    /// <summary>
    /// Turns off Excel's automatic recalculation.
    /// </summary>
    //*************************************************************************

    protected void
    SetManualCalculation()
    {
        AssertValid();

        this.Application.Calculation =
            Microsoft.Office.Interop.Excel.XlCalculation.xlCalculationManual;

        // Use a CalculationRestorer object to automatically restore the
        // original value of the Application.Calculation property after a
        // specified period.


        m_oExcelCalculationRestorer.StartRestoreTimer();
    }

    //*************************************************************************
    //  Method: RecalculateDynamicFilterColumns()
    //
    /// <summary>
    /// Recalculates specified dynamic filter columns.
    /// </summary>
    ///
    /// <param name="eDynamicFilterColumns">
    /// Indicates which dynamic filter columns to recalculate.
    /// </param>
    //*************************************************************************

    protected void
    RecalculateDynamicFilterColumns
    (
        DynamicFilterColumns eDynamicFilterColumns
    )
    {
        AssertValid();

        if ( (eDynamicFilterColumns & DynamicFilterColumns.EdgeTable) != 0 )
        {
            m_oEdgeDynamicFilterColumnData.CalculateRowMajorOrder();
        }

        if ( (eDynamicFilterColumns & DynamicFilterColumns.VertexTable) != 0 )
        {
            m_oVertexDynamicFilterColumnData.CalculateRowMajorOrder();
        }
    }

    //*************************************************************************
    //  Method: FireDynamicFilterColumnsChanged()
    //
    /// <summary>
    /// Fires the <see cref="DynamicFilterColumnsChanged" /> event if
    /// appropriate.
    /// </summary>
    ///
    /// <param name="eDynamicFilterColumns">
    /// Indicates which dynamic filter columns changed.
    /// </param>
    //*************************************************************************

    protected void
    FireDynamicFilterColumnsChanged
    (
        DynamicFilterColumns eDynamicFilterColumns
    )
    {
        AssertValid();

        DynamicFilterColumnsChangedEventHandler oEventHandler =
            this.DynamicFilterColumnsChanged;

        if (oEventHandler != null)
        {
            oEventHandler( this, new DynamicFilterColumnsChangedEventArgs(
                eDynamicFilterColumns) );
        }
    }

    //*************************************************************************
    //  Method: OnLoad()
    //
    /// <summary>
    /// Handles the Load event.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected override void
    OnLoad
    (
        EventArgs e
    )
    {
        AssertValid();

        base.OnLoad(e);

        this.Application.ScreenUpdating = false;

        try
        {
            // Get access to the dynamic filter settings, which are stored in
            // a hidden worksheet.

            m_oDynamicFilterSettings = new DynamicFilterSettings(m_oWorkbook);
            m_oDynamicFilterSettings.Open();

            InitializeDynamicFilters();
        }
        catch (Exception oException)
        {
            this.Application.ScreenUpdating = true;
            ErrorUtil.OnException(oException);

            this.Close();
            return;
        }
        finally
        {
            this.Application.ScreenUpdating = true;
        }
    }

    //*************************************************************************
    //  Method: OnSelectedRangeChanged()
    //
    /// <summary>
    /// Handles the SelectedRangeChanged event on all the range track bars.
    /// </summary>
    ///
    /// <param name="oDynamicFilterRangeTrackBar">
    /// Range track bar that fired the event.
    /// </param>
    //*************************************************************************

    protected void
    OnSelectedRangeChanged
    (
        IDynamicFilterRangeTrackBar oDynamicFilterRangeTrackBar
    )
    {
        Debug.Assert(oDynamicFilterRangeTrackBar != null);
        AssertValid();

        // Retrieve the column information for this range track bar.

        Debug.Assert(oDynamicFilterRangeTrackBar is Control);

        Control oControl = (Control)oDynamicFilterRangeTrackBar;

        Debug.Assert(oControl.Tag is ColumnInfo);

        ColumnInfo oColumnInfo = (ColumnInfo)oControl.Tag;

        // Turn off automatic recalculation.  The dynamic filter column in the
        // edge or vertex table needs to be recalculated as the RangeTrackBar
        // is manipulated, but to minimize delays nothing else should be
        // recalculated.

        SetManualCalculation();

        // Update the persisted settings for this RangeTrackBar.  This updates
        // two cells in the dynamic filter settings table.

        String sSelectedMinimumAddress, sSelectedMaximumAddress;

        m_oDynamicFilterSettings.SetSettings(oColumnInfo.TableName,
            oColumnInfo.ColumnName,
            oDynamicFilterRangeTrackBar.SelectedMinimum,
            oDynamicFilterRangeTrackBar.SelectedMaximum,
            out sSelectedMinimumAddress, out sSelectedMaximumAddress);

        // Now recalculate just the dynamic filter column.

        DynamicFilterColumns eDynamicFilterColumns = DynamicFilterColumns.None;

        if (oColumnInfo.TableName == TableNames.Edges)
        {
            eDynamicFilterColumns = DynamicFilterColumns.EdgeTable;
        }
        else if (oColumnInfo.TableName == TableNames.Vertices)
        {
            eDynamicFilterColumns = DynamicFilterColumns.VertexTable;
        }
        else
        {
            Debug.Assert(false);
        }

        RecalculateDynamicFilterColumns(eDynamicFilterColumns);

        FireDynamicFilterColumnsChanged(eDynamicFilterColumns);
    }

    //*************************************************************************
    //  Method: OnClosed()
    //
    /// <summary>
    /// Handles the Closed event.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected override void
    OnClosed
    (
        EventArgs e
    )
    {
        AssertValid();

        base.OnClosed(e);

        if (m_oRangeTrackBarTimer != null)
        {
            m_oRangeTrackBarTimer.Stop();
        }

        // Immediately restore the original value of the
        // Application.Calculation property.

        m_oExcelCalculationRestorer.Restore();
    }

    //*************************************************************************
    //  Method: RangeTrackBar_SelectedRangeChanged()
    //
    /// <summary>
    /// Handles the SelectedRangeChanged event on all the range track bars.
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
    RangeTrackBar_SelectedRangeChanged
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        if (!m_bHandleRangeTrackBarEvents)
        {
            return;
        }

        // It's possible to handle the selected range change immediately by
        // updating the appropriate dynamic range column and having the
        // TaskPane read the column and redraw the graph.  This takes a while,
        // however, and it slows down the responsivenees of the RangeTrackBar
        // control the user is changing.
        //
        // Instead, use a timer to delay the handling of the
        // SelectedRangeChanged event, and reset the timer as more such events
        // come in.  This allows the RangeTrackBar control to update more
        // quickly and postpones the redrawing of the graph until the user
        // pauses or stops changing the RangeTrackBar control.
        //
        // Store the range track bar in the timer's Tag for use by the Tick
        // event handler.

        Debug.Assert(sender is IDynamicFilterRangeTrackBar);

        m_oRangeTrackBarTimer.Stop();
        m_oRangeTrackBarTimer.Tag = (IDynamicFilterRangeTrackBar)sender;
        m_oRangeTrackBarTimer.Start();
    }

    //*************************************************************************
    //  Method: m_oRangeTrackBarTimer_Tick()
    //
    /// <summary>
    /// Handles the Tick event on the RangeTrackBar timer.
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
    m_oRangeTrackBarTimer_Tick
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        // See RangeTrackBar_SelectedRangeChanged() for details on how this
        // timer is used.

        m_oRangeTrackBarTimer.Stop();

        Debug.Assert(m_oRangeTrackBarTimer.Tag is IDynamicFilterRangeTrackBar);

        try
        {
            OnSelectedRangeChanged(
                (IDynamicFilterRangeTrackBar)m_oRangeTrackBarTimer.Tag );
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
        }
    }

    //*************************************************************************
    //  Method: tsbResetAllDynamicFilters_Click()
    //
    /// <summary>
    /// Handles the Click event on the tsbResetAllDynamicFilters
    /// ToolStripButton.
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
    tsbResetAllDynamicFilters_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        try
        {
            ResetAllDynamicFilters();
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
        }
    }

    //*************************************************************************
    //  Method: tsbReadWorkbook_Click()
    //
    /// <summary>
    /// Handles the Click event on the tsbReadWorkbook ToolStripButton.
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
    tsbReadWorkbook_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        try
        {
            InitializeDynamicFilters();
            RecalculateDynamicFilterColumns(DynamicFilterColumns.AllTables);
            FireDynamicFilterColumnsChanged(DynamicFilterColumns.AllTables);
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
        }
    }

    //*************************************************************************
    //  Method: tsbClose_Click()
    //
    /// <summary>
    /// Handles the Click event on the tsbClose ToolStripButton.
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
    tsbClose_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        this.Close();
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

        Debug.Assert(m_oDynamicFilterDialogUserSettings != null);
        Debug.Assert(m_oWorkbook != null);
        Debug.Assert(m_oExcelCalculationRestorer != null);
        // m_bHandleRangeTrackBarEvents
        Debug.Assert(m_oRangeTrackBarTimer != null);
        // m_oDynamicFilterSettings
        // m_oEdgeDynamicFilterColumnData
        // m_oVertexDynamicFilterColumnData
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Maximum number of dynamic filters to show for each table.  The absolute
    /// limit is set by the maximum number of conditions in an Excel AND()
    /// formula divided by 2, which is 255 divided by 2.  (The divide-by-2 is
    /// due to the fact that each dynamic filter adds two conditions to the
    /// AND() formula.)

    protected const Int32 MaximumDynamicFiltersPerTable = 20;

    /// Vertical margin between a dynamic filter label and its dynamic filter
    /// control.

    protected const Int32 LabelBottomMargin = 1;

    /// Vertical margin between a dynamic filter control and the next dynamic
    /// filter label.

    protected const Int32 DynamicFilterControlBottomMargin = 5;

    /// Vertical margin between the group boxes containing the dynamic filters.

    protected const Int32 GroupBoxBottomMargin = 10;

    /// Interval to use for the Application.Calculation restorer, in
    /// milliseconds.

    protected const Int32 CalculationRestorerTimerIntervalMs = 2000;

    /// Interval to use for the RangeTrackBar event timer, in milliseconds.

    protected const Int32 RangeTrackBarTimerIntervalMs = 50;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// User settings for this dialog.

    protected DynamicFilterDialogUserSettings
        m_oDynamicFilterDialogUserSettings;

    /// Workbook containing the graph contents.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

    /// Object that automatically restores the original value of the
    /// Application.Calculation property.

    protected ExcelCalculationRestorer m_oExcelCalculationRestorer;

    /// true if the RangeTrackBar.SelectedRangeChanged events should be
    /// handled.

    protected Boolean m_bHandleRangeTrackBarEvents;

    /// Timer used to fire the DynamicFilterColumnsChanged event.

    protected Timer m_oRangeTrackBarTimer;

    /// Persisted settings for the dynamic filters, or null if the settings
    /// haven't been obtained yet.

    protected DynamicFilterSettings m_oDynamicFilterSettings;

    /// Data range of the column on the edge worksheet that contains the
    /// dynamic filter formulas.

    protected Microsoft.Office.Interop.Excel.Range
        m_oEdgeDynamicFilterColumnData;

    /// Data range of the column on the vertex worksheet that contains the
    /// dynamic filter formulas.

    protected Microsoft.Office.Interop.Excel.Range
        m_oVertexDynamicFilterColumnData;


    //*************************************************************************
    //  Embedded class: ColumnInfo
    //
    /// <summary>
    /// Contains information about a column being filtered on.
    /// </summary>
    ///
    /// <remarks>
    /// When a RangeTrackBar is created, its Tag is set to one of these
    /// objects.  The information is needed by the RangeTrackBar's
    /// SelectedRangeChanged event handler.
    /// </remarks>
    //*************************************************************************

    protected class ColumnInfo
    {
        /// Name of the table containing the column being filtered on.

        public String TableName;

        /// Name of the column being filtered on.

        public String ColumnName;
    }
}

//*****************************************************************************
//  Enum: DynamicFilterColumns
//
/// <summary>
/// Specifies the dynamic filter column in the edge table, the vertex
/// table, or both.
/// </summary>
///
/// <remarks>
/// These values can be ORed together.
/// </remarks>
//*****************************************************************************

[System.FlagsAttribute]

public enum
DynamicFilterColumns
{
    /// <summary>
    /// Specifies no dynamic filter column.
    /// </summary>

    None = 0,

    /// <summary>
    /// Specifies the dynamic filter column in the edge table.
    /// </summary>

    EdgeTable = 1,

    /// <summary>
    /// Specifies the dynamic filter column in the vertex table.
    /// </summary>

    VertexTable = 2,

    /// <summary>
    /// Specifies the dynamic filter column in all tables.
    /// </summary>

    AllTables = EdgeTable | VertexTable,
}


//*****************************************************************************
//  Class: DynamicFilterDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="DynamicFilterDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("DynamicFilterDialog2") ]

public class DynamicFilterDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: DynamicFilterDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="DynamicFilterDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public DynamicFilterDialogUserSettings
    (
        Form oForm
    )
    : base (oForm, true)
    {
        Debug.Assert(oForm != null);

        // (Do nothing.)

        AssertValid();
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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}
}
