
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
using Microsoft.NodeXL.Core;

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
/// This is a modeless dialog.  To show it, call its <see
/// cref="Form.Show(IWin32Window)" /> method.
///
/// <para>
/// A dynamic filter is a control that can be adjusted to selectively show and
/// hide edges and vertices in the graph in real time.
/// </para>
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
/// <para>
/// In addition to the <see cref="DynamicFilterColumnsChanged" /> event, the
/// TaskPane should handle the <see cref="FilteredAlphaChanged" /> event and
/// redraw the graph with the new filtered alpha value when the event fires.
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

        m_bHandleControlEvents = false;

        m_oChangeEventDelayTimer = new Timer();
        m_oChangeEventDelayTimer.Interval = ChangeEventTimerIntervalMs;

        m_oChangeEventDelayTimer.Tick += new EventHandler(
            this.m_oChangeEventDelayTimer_Tick);

        m_oDynamicFilterSettings = null;
        m_oEdgeDynamicFilterColumnData = null;
        m_oVertexDynamicFilterColumnData = null;

        nudFilteredAlpha.Minimum =
            (Decimal)AlphaConverter.MinimumAlphaWorkbook;

        nudFilteredAlpha.Maximum =
            (Decimal)AlphaConverter.MaximumAlphaWorkbook;

        nudFilteredAlpha.Value = (Decimal)
            ( new PerWorkbookSettings(m_oWorkbook) ).FilteredAlpha;

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
    //  Property: FilteredAlpha
    //
    /// <summary>
    /// Gets the alpha component to use for vertices and edges that are
    /// filtered.
    /// </summary>
    ///
    /// <value>
    /// The alpha value to use for vertices and edges that have a <see
    /// cref="Microsoft.NodeXL.Core.ReservedMetadataKeys.Visibility" /> value
    /// of <see cref="Microsoft.NodeXL.Core.VisibilityKeyValue.Filtered" />.
    /// The value is in workbook units, between
    /// <see cref="AlphaConverter.MinimumAlphaWorkbook" /> and <see
    /// cref="AlphaConverter.MaximumAlphaWorkbook" />.  The value must be
    /// converted using <see cref="AlphaConverter.WorkbookToGraph" /> before
    /// applying it to a graph.
    /// </value>
    ///
    /// <remarks>
    /// The TaskPane should read this property from its <see
    /// cref="FilteredAlphaChanged" /> event handler.
    /// </remarks>
    //*************************************************************************

    public Single
    FilteredAlpha
    {
        get
        {
            AssertValid();

            return ( (Single)nudFilteredAlpha.Value );
        }
    }

    //*************************************************************************
    //  Method: GetDynamicFilterRangeTrackBars()
    //
    /// <summary>
    /// Gets collection of <see cref="IDynamicFilterRangeTrackBar" /> objects,
    /// one for each filtered column in the workbook.
    /// </summary>
    ///
    /// <param name="edgeDynamicFilterRangeTrackBars">
    /// Where a collection of <see cref="IDynamicFilterRangeTrackBar" />
    /// objects gets stored, one for each filtered column in the edge
    /// worksheet.
    /// </param>
    ///
    /// <param name="vertexDynamicFilterRangeTrackBars">
    /// Where a collection of <see cref="IDynamicFilterRangeTrackBar" />
    /// objects gets stored, one for each filtered column in the vertex
    /// worksheet.
    /// </param>
    ///
    /// <remarks>
    /// If the user has selected a dynamic filter's entire available range,
    /// meaning that its column is not filtered on, that filter is not
    /// returned.
    /// </remarks>
    //*************************************************************************

    public void
    GetDynamicFilterRangeTrackBars
    (
        out ICollection<IDynamicFilterRangeTrackBar>
            edgeDynamicFilterRangeTrackBars,

        out ICollection<IDynamicFilterRangeTrackBar>
            vertexDynamicFilterRangeTrackBars
    )
    {
        AssertValid();

        LinkedList<IDynamicFilterRangeTrackBar> oEdgeLinkedList =
            new LinkedList<IDynamicFilterRangeTrackBar>();

        LinkedList<IDynamicFilterRangeTrackBar> oVertexLinkedList =
            new LinkedList<IDynamicFilterRangeTrackBar>();

        LinkedList<IDynamicFilterRangeTrackBar> oLinkedListToAddTo =
            oEdgeLinkedList;

        foreach ( GroupBox oGroupBox in
            new GroupBox[] {grpEdgeFilters, grpVertexFilters} )
        {
            foreach (Control oControl in oGroupBox.Controls)
            {
                if (oControl is IDynamicFilterRangeTrackBar)
                {
                    IDynamicFilterRangeTrackBar oDynamicFilterRangeTrackBar =
                        (IDynamicFilterRangeTrackBar)oControl;

                    if (!oDynamicFilterRangeTrackBar.AvailableRangeSelected)
                    {
                        oLinkedListToAddTo.AddLast(
                            oDynamicFilterRangeTrackBar);
                    }
                }
            }

            oLinkedListToAddTo = oVertexLinkedList;
        }

        edgeDynamicFilterRangeTrackBars = oEdgeLinkedList;
        vertexDynamicFilterRangeTrackBars = oVertexLinkedList;
    }

    //*************************************************************************
    //  Event: DynamicFilterColumnsChanged
    //
    /// <summary>
    /// Occurs when one or more dynamic filter columns change.
    /// </summary>
    ///
    /// <remarks>
    /// See the class topic for details on how this event should be handled.
    /// </remarks>
    //*************************************************************************

    public event DynamicFilterColumnsChangedEventHandler
        DynamicFilterColumnsChanged;


    //*************************************************************************
    //  Event: FilteredAlphaChanged
    //
    /// <summary>
    /// Occurs when the <see cref="FilteredAlpha" /> property changes.
    /// </summary>
    ///
    /// <remarks>
    /// See the class topic for details on how this event should be handled.
    /// </remarks>
    //*************************************************************************

    public event EventHandler FilteredAlphaChanged;


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

        m_bHandleControlEvents = false;
        this.UseWaitCursor = true;

        m_oEdgeDynamicFilterColumnData =
            InitializeDynamicFiltersForOneTable(WorksheetNames.Edges,
                TableNames.Edges, grpEdgeFilters);

        m_oVertexDynamicFilterColumnData =
            InitializeDynamicFiltersForOneTable(WorksheetNames.Vertices,
                TableNames.Vertices, grpVertexFilters);

        grpVertexFilters.Top = grpEdgeFilters.Bottom + GroupBoxBottomMargin;

        grpEdgeFilters.Visible = grpVertexFilters.Visible = true;

        this.UseWaitCursor = false;
        m_bHandleControlEvents = true;
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

                    Label oLabel = AddLabelToGroupBox(oDynamicFilterParameters,
                        oGroupBox, iX, iY);

                    PictureBox oHistogram = AddHistogramToGroupBox(
                        sWorksheetName, oDynamicFilterParameters, oGroupBox,
                        iX, ref iY);

                    Debug.Assert(oDynamicFilterParameters is
                        NumericFilterParameters);

                    IDynamicFilterRangeTrackBar oDynamicFilterRangeTrackBar =
                        AddRangeTrackBarToGroupBox(
                            (NumericFilterParameters)oDynamicFilterParameters,
                            sTableName, oGroupBox, oDynamicFilterConditions,
                            iX, ref iY);

                    // Adjust the location and dimension of the controls that
                    // depend on other controls.

                    Rectangle oInternalTrackBarBounds =
                        oDynamicFilterRangeTrackBar.InternalTrackBarBounds;

                    oHistogram.Width = oInternalTrackBarBounds.Width;

                    oHistogram.Left = oDynamicFilterRangeTrackBar.Left
                        + oInternalTrackBarBounds.Left;

                    oLabel.Width = oHistogram.Left - oLabel.Left
                        - LabelRightMargin;

                    oLabel.Height = oHistogram.Height;

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

        m_bHandleControlEvents = false;

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

                    String sSelectedMinimumAddress, sSelectedMaximumAddress;

                    m_oDynamicFilterSettings.SetSettings(
                        oDynamicFilterRangeTrackBar.TableName,
                        oDynamicFilterRangeTrackBar.ColumnName,
                        oDynamicFilterRangeTrackBar.SelectedMinimum,
                        oDynamicFilterRangeTrackBar.SelectedMaximum,
                        out sSelectedMinimumAddress,
                        out sSelectedMaximumAddress);
                }
            }
        }

        m_bHandleControlEvents = true;

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
    /// y-coordinate of the Label.  Note that this does NOT get increased by
    /// the height of the Label.  The histogram, which has the same Top
    /// coordinate as the Label, is taller than the Label, so
    /// AddHistogramImageToPictureBox() increases iY, not this method.
    /// </param>
    ///
    /// <returns>
    /// The new Label.
    /// </returns>
    //*************************************************************************

    protected Label
    AddLabelToGroupBox
    (
        DynamicFilterParameters oDynamicFilterParameters,
        GroupBox oGroupBox,
        Int32 iX,
        Int32 iY
    )
    {
        Debug.Assert(oGroupBox != null);
        Debug.Assert(oDynamicFilterParameters != null);
        AssertValid();

        Label oLabel = new Label();
        oLabel.Text = oDynamicFilterParameters.ColumnName + ":";
        oLabel.Location = new Point(iX, iY);
        oLabel.Margin = Padding.Empty;
        oLabel.AutoEllipsis = true;

        // The actual size of the Label will get set by the caller.

        oLabel.AutoSize = false;

        oGroupBox.Controls.Add(oLabel);

        return (oLabel);
    }

    //*************************************************************************
    //  Method: AddHistogramToGroupBox()
    //
    /// <summary>
    /// Adds a dynamic filter control histogram to a GroupBox.
    /// </summary>
    ///
    /// <param name="sWorksheetName">
    /// Name of the worksheet containing the column the histogram is for.
    /// </param>
    ///
    /// <param name="oDynamicFilterParameters">
    /// Parameters for the dynamic filter control.
    /// </param>
    ///
    /// <param name="oGroupBox">
    /// GroupBox to which the histogram should be added.
    /// </param>
    ///
    /// <param name="iX">
    /// x-coordinate of the histogram.
    /// </param>
    ///
    /// <param name="iY">
    /// y-coordinate of the histogram.  Gets increased by the height of the
    /// histogram.
    /// </param>
    ///
    /// <returns>
    /// The new PictureBox that displays the histogram.
    /// </returns>
    //*************************************************************************

    protected PictureBox
    AddHistogramToGroupBox
    (
        String sWorksheetName,
        DynamicFilterParameters oDynamicFilterParameters,
        GroupBox oGroupBox,
        Int32 iX,
        ref Int32 iY
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sWorksheetName) );
        Debug.Assert(oDynamicFilterParameters != null);
        Debug.Assert(oGroupBox != null);
        AssertValid();

        // The histogram is a PictureBox containing an image created by Excel.

        PictureBoxPlus oPictureBox = new PictureBoxPlus();
        oPictureBox.Location = new Point(iX, iY);
        oPictureBox.Margin = Padding.Empty;
        oPictureBox.Height = HistogramHeight;
        oPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

        oPictureBox.Anchor =
            AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

        oGroupBox.Controls.Add(oPictureBox);

        AddHistogramImageToPictureBox(sWorksheetName, oDynamicFilterParameters,
            oPictureBox);

        iY += oPictureBox.Height + HistogramBottomMargin;

        return (oPictureBox);
    }

    //*************************************************************************
    //  Method: AddHistogramImageToPictureBox()
    //
    /// <summary>
    /// Adds a dynamic filter control histogram image to a PictureBox.
    /// </summary>
    ///
    /// <param name="sWorksheetName">
    /// Name of the worksheet containing the column the histogram is for.
    /// </param>
    ///
    /// <param name="oDynamicFilterParameters">
    /// Parameters for the dynamic filter control.
    /// </param>
    ///
    /// <param name="oPictureBox">
    /// The PictureBox to which the histogram image should be added.
    /// </param>
    //*************************************************************************

    protected void
    AddHistogramImageToPictureBox
    (
        String sWorksheetName,
        DynamicFilterParameters oDynamicFilterParameters,
        PictureBoxPlus oPictureBox
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sWorksheetName) );
        Debug.Assert(oDynamicFilterParameters != null);
        Debug.Assert(oPictureBox != null);
        AssertValid();

        // This method uses the following technique to get Excel to generate
        // the histogram image.
        //
        // There is a hidden chart on the Miscellaneous worksheet that is
        // used for the histogram.  It gets its data from two columns in a
        // hidden table on the Overall Metrics worksheet that use Excel
        // formulas to calculate the frequency distribution of the values in an
        // Excel column, called the "source column."  The formulas use Excel's
        // INDIRECT() function to get the address of the source column from a
        // cell named NamedRange.DynamicFilterSourceColumnRange.
        //
        // In a new workbook, the frequency distribution columns initially
        // contain all #REF! errors, because the named range is empty.  This
        // method sets the named range to something like
        // "Vertices[ColumnName]", then forces Excel to recalculate the
        // frequency distribution columns.  This causes a histogram to appear
        // in the chart.  An image of the chart is then copied to the clipboard
        // and pasted into the PictureBox.

        Microsoft.Office.Interop.Excel.Worksheet oOverallMetricsWorksheet,
            oMiscellaneousWorksheet;

        Microsoft.Office.Interop.Excel.Range oDynamicFilterSourceColumnRange,
            oDynamicFilterForceCalculationRange;

        Microsoft.Office.Interop.Excel.Chart oDynamicFilterHistogram;

        if (
            ExcelUtil.TryGetWorksheet(m_oWorkbook,
                WorksheetNames.OverallMetrics, out oOverallMetricsWorksheet)
            &&
            ExcelUtil.TryGetNamedRange(oOverallMetricsWorksheet,
                NamedRangeNames.DynamicFilterSourceColumnRange,
                out oDynamicFilterSourceColumnRange)
            &&
            ExcelUtil.TryGetNamedRange(oOverallMetricsWorksheet,
                NamedRangeNames.DynamicFilterForceCalculationRange,
                out oDynamicFilterForceCalculationRange)
            &&
            ExcelUtil.TryGetWorksheet(m_oWorkbook,
                WorksheetNames.Miscellaneous, out oMiscellaneousWorksheet)
            &&
            ExcelUtil.TryGetChart(oMiscellaneousWorksheet,
                ChartNames.DynamicFilterHistogram, out oDynamicFilterHistogram)
            )
        {
            // Set the named range to the address of the source column.
            // Sample: "Vertices[Degree]".

            oDynamicFilterSourceColumnRange.set_Value(Missing.Value,
                String.Format(
                    "{0}[{1}]",
                    sWorksheetName,
                    oDynamicFilterParameters.ColumnName
                    )
                );

            // Excel's automatic calculation may be turned off, either by the
            // user or by code elsewhere in this dialog.  Make sure the
            // frequency distribution columns get calculated.

            oDynamicFilterForceCalculationRange.Calculate();

            // Make sure the chart is drawn immediately.

            oDynamicFilterHistogram.Refresh();

            // Tell Excel to copy the chart image to the clipboard.  (Although
            // the second argument to CopyPicture is xlBitmap, no bitmap gets
            // copied.  Instead, Excel uses an enhanced metafile.)

            oDynamicFilterHistogram.CopyPicture(
                Microsoft.Office.Interop.Excel.XlPictureAppearance.xlScreen,
                Microsoft.Office.Interop.Excel.XlCopyPictureFormat.xlBitmap,
                Microsoft.Office.Interop.Excel.XlPictureAppearance.xlScreen);

            oPictureBox.TryPasteEnhancedMetafile();
        }
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
    /// y-coordinate of the range track bar.  Gets increased by the height of
    /// the range track bar.
    /// </param>
    ///
    /// <returns>
    /// The new range track bar.
    /// </returns>
    //*************************************************************************

    protected IDynamicFilterRangeTrackBar
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
                new DynamicFilterDateTimeRangeTrackBar(
                    sTableName, sColumnName);
        }
        else
        {
            oDynamicFilterRangeTrackBar = new DynamicFilterRangeTrackBar(
                sTableName, sColumnName);
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

        // Make sure that a bunch of unwanted events during initialization are
        // avoided.

        Debug.Assert(!m_bHandleControlEvents);

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

        return (oDynamicFilterRangeTrackBar);
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

        if ( !ExcelUtil.VisibleTableRangeIsEmpty(oTable) )
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

        // Excel values are Doubles, but NumericRangeTrackBar values are
        // Decimal.  (That's because NumericRangeTrackBar uses a NumericUpDown
        // control, which can only handle Decimals.)  This can cause problems
        // with very small and very large numbers.
        //
        // For example, the available minimum number 1.54768660209645E-22
        // becomes 1.547687E-22 when converted to a Decimal, and because the
        // Decimal is greater than the Double, using that Decimal for the
        // available minimum would always filter out the edge or vertex row
        // that the 1.54768660209645E-22 came from.
        //
        // Fix this by rounding the available minimum down and the available
        // maximum up if necessary.

        Double dAvailableMinimum = oNumericFilterParameters.MinimumCellValue;
        decAvailableMinimum = (Decimal)dAvailableMinimum;

        if ( (Double)decAvailableMinimum > dAvailableMinimum)
        {
            decAvailableMinimum = Math.Floor(decAvailableMinimum);
        }

        Double dAvailableMaximum = oNumericFilterParameters.MaximumCellValue;
        decAvailableMaximum = (Decimal)dAvailableMaximum;

        if ( (Double)decAvailableMaximum < dAvailableMaximum)
        {
            decAvailableMaximum = Math.Ceiling(decAvailableMaximum);
        }

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
    //  Method: StartChangeEventDelayTimer()
    //
    /// <summary>
    /// Starts the timer used to delay the DynamicFilterColumnsChanged and
    /// FilteredAlphaChanged events.
    /// </summary>
    ///
    /// <param name="oChangedControl">
    /// The control that was just changed by the user.
    /// </param>
    //*************************************************************************

    protected void
    StartChangeEventDelayTimer
    (
        Object oChangedControl
    )
    {
        Debug.Assert(oChangedControl != null);
        AssertValid();

        m_oChangeEventDelayTimer.Stop();

        // Store the changed control in the timer's Tag for use by the Tick
        // event handler.

        m_oChangeEventDelayTimer.Tag = oChangedControl;

        m_oChangeEventDelayTimer.Start();
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
    //  Method: FireFilteredAlphaChanged()
    //
    /// <summary>
    /// Fires the <see cref="FilteredAlphaChanged" /> event if appropriate.
    /// </summary>
    //*************************************************************************

    protected void
    FireFilteredAlphaChanged()
    {
        AssertValid();

        EventUtil.FireEvent(this, this.FilteredAlphaChanged);
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

        // Turn off automatic recalculation.  The dynamic filter column in the
        // edge or vertex table needs to be recalculated as the RangeTrackBar
        // is manipulated, but to minimize delays nothing else should be
        // recalculated.

        SetManualCalculation();

        // Update the persisted settings for this RangeTrackBar.  This updates
        // two cells in the dynamic filter settings table.

        String sSelectedMinimumAddress, sSelectedMaximumAddress;

        m_oDynamicFilterSettings.SetSettings(
            oDynamicFilterRangeTrackBar.TableName,
            oDynamicFilterRangeTrackBar.ColumnName,
            oDynamicFilterRangeTrackBar.SelectedMinimum,
            oDynamicFilterRangeTrackBar.SelectedMaximum,
            out sSelectedMinimumAddress, out sSelectedMaximumAddress);

        // Now recalculate just the dynamic filter column.

        DynamicFilterColumns eDynamicFilterColumns = DynamicFilterColumns.None;

        if (oDynamicFilterRangeTrackBar.TableName == TableNames.Edges)
        {
            eDynamicFilterColumns = DynamicFilterColumns.EdgeTable;
        }
        else if (oDynamicFilterRangeTrackBar.TableName == TableNames.Vertices)
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

        if (m_oChangeEventDelayTimer != null)
        {
            m_oChangeEventDelayTimer.Stop();
        }

        // Immediately restore the original value of the
        // Application.Calculation property.

        m_oExcelCalculationRestorer.Restore();

        ( new PerWorkbookSettings(m_oWorkbook) ).FilteredAlpha =
            (Single)nudFilteredAlpha.Value;
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

        if (!m_bHandleControlEvents)
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

        Debug.Assert(sender is IDynamicFilterRangeTrackBar);

        StartChangeEventDelayTimer( (IDynamicFilterRangeTrackBar)sender );
    }

    //*************************************************************************
    //  Method: nudFilteredAlpha_ValueChanged()
    //
    /// <summary>
    /// Handles the ValueChanged event on the nudFilteredAlpha NumericUpdDown.
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
    nudFilteredAlpha_ValueChanged
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        if (!m_bHandleControlEvents)
        {
            return;
        }

        // See the comments in RangeTrackBar_SelectedRangeChanged() for details
        // on how the change event delay timer works.

        StartChangeEventDelayTimer(nudFilteredAlpha);
    }

    //*************************************************************************
    //  Method: m_oChangeEventDelayTimer_Tick()
    //
    /// <summary>
    /// Handles the Tick event on the change event delay timer.
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
    m_oChangeEventDelayTimer_Tick
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        // See RangeTrackBar_SelectedRangeChanged() for details on how this
        // timer is used.

        m_oChangeEventDelayTimer.Stop();

        Object oChangedControl = m_oChangeEventDelayTimer.Tag;

        try
        {
            if (oChangedControl is IDynamicFilterRangeTrackBar)
            {
                OnSelectedRangeChanged(
                    (IDynamicFilterRangeTrackBar)oChangedControl );
            }
            else if (oChangedControl is NumericUpDown)
            {
                FireFilteredAlphaChanged();
            }
            else
            {
                Debug.Assert(false);
            }
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
        }
    }

    //*************************************************************************
    //  Method: btnResetAllDynamicFilters_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnResetAllDynamicFilters Button.
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
    btnResetAllDynamicFilters_Click
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
    //  Method: btnReadWorkbook_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnReadWorkbook Button.
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
    btnReadWorkbook_Click
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
    //  Method: btnClose_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnClose Button.
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
    btnClose_Click
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
        // m_bHandleControlEvents
        Debug.Assert(m_oChangeEventDelayTimer != null);
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

    /// Margin between the right edge of a dynamic filter label and the left
    /// edge of the histogram.  (The label and histogram have the same Top
    /// coordinate.)

    protected const Int32 LabelRightMargin = 2;

    /// Height of a dynamic filter histogram.

    protected const Int32 HistogramHeight = 40;

    /// Vertical margin between the bottom of a dynamic filter histogram and
    /// the top of a dynamic filter range track bar control.

    protected const Int32 HistogramBottomMargin = 1;

    /// Vertical margin between the bottom of a dynamic filter range track bar
    /// control and the next dynamic filter label and histogram.

    protected const Int32 DynamicFilterControlBottomMargin = 20;

    /// Vertical margin between the group boxes containing the dynamic filters.

    protected const Int32 GroupBoxBottomMargin = 10;

    /// Interval to use for the Application.Calculation restorer, in
    /// milliseconds.

    protected const Int32 CalculationRestorerTimerIntervalMs = 2000;

    /// Interval to use for the change event delay timer, in milliseconds.

    protected const Int32 ChangeEventTimerIntervalMs = 50;


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

    /// true if the RangeTrackBar.SelectedRangeChanged and
    /// nudFilteredAlpha.ValueChanged events should be handled.

    protected Boolean m_bHandleControlEvents;

    /// Timer used to delay the DynamicFilterColumnsChanged and
    /// FilteredAlphaChanged events.

    protected Timer m_oChangeEventDelayTimer;

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

[ SettingsGroupNameAttribute("DynamicFilterDialog3") ]

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
