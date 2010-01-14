
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.ApplicationUtil;
using Microsoft.WpfGraphicsLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: PerWorkbookSettings
//
/// <summary>
/// Provides access to settings that are stored on a per-workbook basis.
/// </summary>
///
/// <remarks>
/// The settings are stored in a table in a hidden worksheet.
///
/// <para>
/// Call <see cref="ReadWorksheet" /> to read those settings that get stored
/// directly on an <see cref="IGraph" /> object.  Use other properties to get
/// and set other settings that are not stored on an <see cref="IGraph" />
/// object.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class PerWorkbookSettings : WorksheetReaderBase
{
    //*************************************************************************
    //  Constructor: PerWorkbookSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="PerWorkbookSettings" />
    /// class.
    /// </summary>
    ///
    /// <param name="workbook">
    /// The workbook containing the settings.
    /// </param>
    //*************************************************************************

    public PerWorkbookSettings
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        m_oWorkbook = workbook;
        m_oSettings = null;

        AssertValid();
    }

    //*************************************************************************
    //  Property: TemplateVersion
    //
    /// <summary>
    /// Gets the version number of the template the workbook is based on.
    /// </summary>
    ///
    /// <value>
    /// A template version number.
    /// </value>
    //*************************************************************************

    public Int32
    TemplateVersion
    {
        get
        {
            AssertValid();

            // The template version is stored in the workbook as a Double with
            // zero decimal places.  Sample: 51.

            Object oTemplateVersion;

            if ( TryGetValue(TemplateVersionSettingName, typeof(Double),
                out oTemplateVersion) )
            {
                return ( (Int32)(Double)oTemplateVersion );
            }

            return (DefaultTemplateVersion);
        }
    }

    //*************************************************************************
    //  Property: BackColor
    //
    /// <summary>
    /// Gets or sets the graph's background color.
    /// </summary>
    ///
    /// <value>
    /// The graph's background color, as a Nullable&lt;Color&gt;, or a Nullable
    /// with no value if a background color hasn't been specified.  The default
    /// value is a Nullable with no value.
    /// </value>
    //*************************************************************************

    public Nullable<Color>
    BackColor
    {
        get
        {
            AssertValid();

            // The color is stored in the workbook as a String created by
            // ColorConverter2.  Samples: "White", "100,100,100".

            Object oBackColorAsObject;

            if ( TryGetValue(BackColorSettingName, typeof(String),
                out oBackColorAsObject) )
            {
                Color oBackColor;

                if ( ( new ColorConverter2() ).TryWorkbookToGraph(
                    (String)oBackColorAsObject, out oBackColor) )
                {
                    return ( new Nullable<Color>(oBackColor) );
                }
            }

            return ( new Nullable<Color>() );
        }

        set
        {
            String sBackColor;

            if (value.HasValue)
            {
                // Preceed the color with an apostrophe to force Excel to treat
                // "100,100,100", for example, as text and not a number.

                sBackColor = "'" +
                    ( new ColorConverter2() ).GraphToWorkbook(value.Value);
            }
            else
            {
                sBackColor = String.Empty;
            }

            SetValue(BackColorSettingName, sBackColor);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: BackgroundImageUri
    //
    /// <summary>
    /// Gets or sets the URI string for the graph's background image.
    /// </summary>
    ///
    /// <value>
    /// An URI string for the graph's background image, as a String, or null if
    /// no background image has been specified.  The default value is null.
    /// </value>
    //*************************************************************************

    public String
    BackgroundImageUri
    {
        get
        {
            AssertValid();

            Object oBackgroundImageUri;

            if ( TryGetValue(BackgroundImageUriSettingName, typeof(String),
                out oBackgroundImageUri) )
            {
                return ( (String)oBackgroundImageUri );
            }

            return (null);
        }

        set
        {
            SetValue(BackgroundImageUriSettingName,
                (value == null) ? String.Empty : value);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: GraphDirectedness
    //
    /// <summary>
    /// Gets or sets the graph directedness of the workbook.
    /// </summary>
    ///
    /// <value>
    /// A GraphDirectedness value.
    /// </value>
    //*************************************************************************

    public GraphDirectedness
    GraphDirectedness
    {
        get
        {
            AssertValid();

            // The directedness is stored in the workbook as a String.  Sample:
            // "Undirected".

            Object oGraphDirectedness;

            if ( TryGetValue(GraphDirectednessSettingName, typeof(String),
                out oGraphDirectedness) )
            {
                try
                {
                    return ( (GraphDirectedness)Enum.Parse(
                        typeof(GraphDirectedness),
                        (String)oGraphDirectedness) );
                }
                catch (ArgumentException)
                {
                }
            }

            return (DefaultGraphDirectedness);
        }

        set
        {
            SetValue( GraphDirectednessSettingName, value.ToString() );

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: AutoLayoutOnOpen
    //
    /// <summary>
    /// Gets the layout to use when the workbook is opened to automatically lay
    /// out and show the graph.
    /// </summary>
    ///
    /// <value>
    /// The <see cref="LayoutType" /> to use when the workbook is opened, or
    /// null to not automatically lay out and show the graph.
    /// </value>
    ///
    /// <value>
    /// If this returns null, no special action should be taken when the
    /// workbook is opened.  If it is non-null, the layout should be set to the
    /// returned value and the workbook should be automatically read.
    /// </value>
    //*************************************************************************

    public Nullable<LayoutType>
    AutoLayoutOnOpen
    {
        get
        {
            AssertValid();

            // The LayoutType is stored in the workbook as a String.  Sample:
            // "Grid".

            Object oLayoutType;

            if ( TryGetValue(AutoLayoutOnOpenSettingName, typeof(String),
                out oLayoutType) )
            {
                try
                {
                    return ( new Nullable<LayoutType>(
                        (LayoutType)Enum.Parse(typeof(LayoutType),
                            (String)oLayoutType) ) );
                }
                catch (ArgumentException)
                {
                }
            }

            return ( new Nullable<LayoutType>() );
        }
    }

    //*************************************************************************
    //  Property: FilteredAlpha
    //
    /// <summary>
    /// Gets or sets the alpha component to use for vertices and edges that are
    /// filtered.
    /// </summary>
    ///
    /// <value>
    /// The alpha value to use for vertices and edges that have a <see
    /// cref="ReservedMetadataKeys.Visibility" /> value of <see
    /// cref="VisibilityKeyValue.Filtered" />.  Must be between
    /// AlphaConverter.MinimumAlphaWorkbook and
    /// AlphaConverter.MaximumAlphaConverter.  The default value is 0.0.
    /// </value>
    //*************************************************************************

    public Single
    FilteredAlpha
    {
        get
        {
            AssertValid();

            // The filtered alpha is stored in the workbook as a Double with
            // zero decimal places.  Sample: 30.

            Single fFilteredAlpha = DefaultFilteredAlpha;
            Object oFilteredAlpha;

            if ( TryGetValue(FilteredAlphaSettingName, typeof(Double),
                out oFilteredAlpha) )
            {
                fFilteredAlpha = (Single)(Double)oFilteredAlpha;

                if (fFilteredAlpha < AlphaConverter.MinimumAlphaWorkbook ||
                    fFilteredAlpha > AlphaConverter.MaximumAlphaWorkbook)
                {
                    fFilteredAlpha = DefaultFilteredAlpha;
                }
            }

            return (fFilteredAlpha);
        }

        set
        {
            SetValue(FilteredAlphaSettingName, value);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: AutoFillWorkbookResults
    //
    /// <summary>
    /// Gets or sets the results of running the application's autofill feature.
    /// </summary>
    ///
    /// <value>
    /// The results of running the application's autofill feature, as an <see
    /// cref="ExcelTemplate.AutoFillWorkbookResults" /> object.  If the feature
    /// hasn't been run on the workbook, all TryXX() methods on the returned
    /// object will return false.
    /// </value>
    ///
    /// <remarks>
    /// Because the autofill and "autofill with scheme" features are mutually
    /// exclusive, setting this property may clear any <see
    /// cref="ExcelTemplate.AutoFillWorkbookWithSchemeResults" /> object that
    /// was saved set by the <see cref="AutoFillWorkbookWithSchemeResults" />
    /// property.  (It does this only if one or more columns were actually
    /// autofilled.)
    /// </remarks>
    //*************************************************************************

    public AutoFillWorkbookResults
    AutoFillWorkbookResults
    {
        get
        {
            AssertValid();

            // The results are stored in the workbook as a String, using the
            // AutoFillWorkbookResults.ConvertToString() and
            // ConvertFromString() methods.

            Object oAutoFillWorkbookResults;

            if ( !TryGetValue(AutoFillWorkbookResultsSettingName,
                typeof(String), out oAutoFillWorkbookResults) )
            {
                // Return an object whose TryXX() methods will all return
                // false.

                return ( new AutoFillWorkbookResults() );
            }

            return ( AutoFillWorkbookResults.FromString(
                (String)oAutoFillWorkbookResults) );
        }

        set
        {
            SetValue( AutoFillWorkbookResultsSettingName,
                value.ConvertToString() );

            // Clear the scheme results if necessary.  (See the property
            // comments.)

            if (value.AutoFilledNonXYColumnCount > 0)
            {
                ClearAutoFillWorkbookWithSchemeResults();
            }

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: AutoFillWorkbookWithSchemeResults
    //
    /// <summary>
    /// Gets or sets the results of running the application's "autofill with
    /// scheme" feature.
    /// </summary>
    ///
    /// <value>
    /// The results of running the application's "autofill with scheme"
    /// feature, as an <see
    /// cref="ExcelTemplate.AutoFillWorkbookWithSchemeResults" /> object.  If
    /// the feature hasn't been run on the workbook, the <see
    /// cref="ExcelTemplate.AutoFillWorkbookWithSchemeResults.SchemeType" />
    /// property on the returned object will return <see
    /// cref="AutoFillSchemeType.None" />.
    /// </value>
    ///
    /// <remarks>
    /// Because the autofill and "autofill with scheme" features are mutually
    /// exclusive, setting this property clears any <see
    /// cref="ExcelTemplate.AutoFillWorkbookResults" /> object that was saved
    /// by the <see cref="AutoFillWorkbookResults" /> property.
    /// </remarks>
    //*************************************************************************

    public AutoFillWorkbookWithSchemeResults
    AutoFillWorkbookWithSchemeResults
    {
        get
        {
            AssertValid();

            // The results are stored in the workbook as a String, using the
            // AutoFillWorkbookWithSchemeResults.ConvertToString() and
            // ConvertFromString() methods.

            Object oAutoFillWorkbookWithSchemeResults;

            if ( !TryGetValue(AutoFillWorkbookWithSchemeResultsSettingName,
                typeof(String), out oAutoFillWorkbookWithSchemeResults) )
            {
                // Return an object whose Type property will return
                // SchemeType.None.

                return ( new AutoFillWorkbookWithSchemeResults() );
            }

            return ( AutoFillWorkbookWithSchemeResults.ConvertFromString(
                (String)oAutoFillWorkbookWithSchemeResults) );
        }

        set
        {
            SetValue( AutoFillWorkbookWithSchemeResultsSettingName,
                value.ConvertToString() );

            // Clear the autofill results if necessary.  (See the property
            // comments.)

            if (value.SchemeType != AutoFillSchemeType.None)
            {
                ClearAutoFillWorkbookResults();
            }

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: ReadWorksheet()
    //
    /// <summary>
    /// Reads per-workbook settings that are stored directly on an <see
    /// cref="IGraph" /> object.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <param name="readWorkbookContext">
    /// Provides access to objects needed for converting an Excel workbook to a
    /// NodeXL graph.
    /// </param>
    ///
    /// <param name="graph">
    /// Graph to add data to.
    /// </param>
    //*************************************************************************

    public void
    ReadWorksheet
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        ReadWorkbookContext readWorkbookContext,
        IGraph graph
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert(readWorkbookContext != null);
        Debug.Assert(graph != null);
        AssertValid();

        Nullable<Color> oBackColor = this.BackColor;

        if (oBackColor.HasValue)
        {
            graph.SetValue(ReservedMetadataKeys.GraphBackColor,
                oBackColor.Value);

            // (Note that if there is no per-workbook background color, the
            // GraphDrawer.BackColor property will be used instead.)
        }

        String sBackgroundImageUri = this.BackgroundImageUri;

        if ( !String.IsNullOrEmpty(sBackgroundImageUri) )
        {
            System.Windows.Media.ImageSource oImage = ( new WpfImageUtil() ).
                GetImageSynchronousIgnoreDpi(sBackgroundImageUri);

            graph.SetValue(ReservedMetadataKeys.GraphBackgroundImage, oImage);

            // (Note that if there is no per-workbook background image, no
            // background image will be drawn.)
        }
    }

    //*************************************************************************
    //  Method: GetColumnGroupVisibility()
    //
    /// <summary>
    /// Gets a flag specifying whether a column group should be shown.
    /// </summary>
    ///
    /// <param name="columnGroup">
    /// The column group to get the visibility for.
    /// </param>
    ///
    /// <returns>
    /// true to show the column group.
    /// </returns>
    //*************************************************************************

    public Boolean
    GetColumnGroupVisibility
    (
        ColumnGroup columnGroup
    )
    {
        AssertValid();

        Boolean bColumnGroupVisibility = false;
        Object oColumnGroupVisibility;

        if ( TryGetValue(GetColumnGroupSettingName(columnGroup),
            typeof(Boolean), out oColumnGroupVisibility) )
        {
            bColumnGroupVisibility = (Boolean)oColumnGroupVisibility;
        }

        return (bColumnGroupVisibility);
    }

    //*************************************************************************
    //  Method: SetColumnGroupVisibility()
    //
    /// <summary>
    /// Sets a flag specifying whether a column group should be shown.
    /// </summary>
    ///
    /// <param name="columnGroup">
    /// The column group to set the visibility for.
    /// </param>
    ///
    /// <param name="show">
    /// true if the column group should be shown.
    /// </param>
    //*************************************************************************

    public void
    SetColumnGroupVisibility
    (
        ColumnGroup columnGroup,
        Boolean show
    )
    {
        AssertValid();

        SetValue(GetColumnGroupSettingName(columnGroup), show);
    }

    //*************************************************************************
    //  Method: OnWorkbookTablesCleared()
    //
    /// <summary>
    /// Clears properties that are no longer valid after the workbook's tables
    /// have been cleared.
    /// </summary>
    //*************************************************************************

    public void
    OnWorkbookTablesCleared()
    {
        AssertValid();

        ClearAutoFillWorkbookResults();
        ClearAutoFillWorkbookWithSchemeResults();
    }

    //*************************************************************************
    //  Method: ClearAutoFillWorkbookResults()
    //
    /// <summary>
    /// Clears the results of running the application's autofill feature.
    /// </summary>
    //*************************************************************************

    public void
    ClearAutoFillWorkbookResults()
    {
        AssertValid();

        SetValue( AutoFillWorkbookResultsSettingName,
            ( new AutoFillWorkbookResults() ).ConvertToString() );
    }

    //*************************************************************************
    //  Method: ClearAutoFillWorkbookWithSchemeResults()
    //
    /// <summary>
    /// Clears the results of running the application's "autofill with scheme"
    /// feature.
    /// </summary>
    //*************************************************************************

    public void
    ClearAutoFillWorkbookWithSchemeResults()
    {
        AssertValid();

        SetValue( AutoFillWorkbookWithSchemeResultsSettingName,
            ( new AutoFillWorkbookWithSchemeResults() ).ConvertToString() );
    }

    //*************************************************************************
    //  Method: GetColumnGroupSettingName()
    //
    /// <summary>
    /// Gets the setting name to use for getting or setting a flag specifying
    /// whether a column group should be shown.
    /// </summary>
    ///
    /// <param name="columnGroup">
    /// The column group.
    /// </param>
    ///
    /// <returns>
    /// The setting name to use.
    /// </returns>
    //*************************************************************************

    protected String
    GetColumnGroupSettingName
    (
        ColumnGroup columnGroup
    )
    {
        AssertValid();

        // Sample setting name: "Show Vertex Graph Metrics"

        return ( "Show " + EnumUtil.SplitName(
            columnGroup.ToString(), EnumSplitStyle.AllWordsStartUpperCase) );
    }

    //*************************************************************************
    //  Method: SetValue()
    //
    /// <summary>
    /// Sets the value of a setting.
    /// </summary>
    ///
    /// <param name="settingName">
    /// The setting's name.
    /// </param>
    ///
    /// <param name="value">
    /// The value to set.  Can be null.
    /// </param>
    //*************************************************************************

    protected void
    SetValue
    (
        String settingName,
        Object value
    )
    {
        AssertValid();

        Dictionary<String, Object> oSettings = GetAllSettings();

        oSettings[settingName] = value;

        WriteAllSettings();
    }

    //*************************************************************************
    //  Method: TryGetValue()
    //
    /// <summary>
    /// Attempts to get the value of a specified setting.
    /// </summary>
    ///
    /// <param name="settingName">
    /// The setting's name.
    /// </param>
    ///
    /// <param name="valueType">
    /// Expected type of the requested value.  Sample: typeof(String).
    /// </param>
    ///
    /// <param name="value">
    /// Where the value gets stored if true is returned, as an <see
    /// cref="Object" />.
    /// </param>
    ///
    /// <returns>
    /// true if the specified value was found, or false if not.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetValue
    (
        String settingName,
        Type valueType,
        out Object value
    )
    {
        AssertValid();

        value = null;

        Dictionary<String, Object> oSettings = GetAllSettings();

        // Although the worksheet that contains the settings is hidden, the
        // user may have unhidden it and edited the settings.  Therefore, don't
        // throw an exception if the value type is incorrect.

        if (oSettings.TryGetValue(settingName, out value) && value != null &&
            value.GetType() == valueType)
        {
            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: GetAllSettings()
    //
    /// <summary>
    /// Gets all settings from the workbook.
    /// </summary>
    ///
    /// <returns>
    /// A dictionary of all settings.  The key is the setting name and the
    /// value is the setting value.
    /// </returns>
    ///
    /// <remarks>
    /// The settings are read once and then cached.
    ///
    /// <para>
    /// If the settings can't be read, the returned dictionary is empty.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected Dictionary<String, Object>
    GetAllSettings()
    {
        AssertValid();

        if (m_oSettings == null)
        {
            m_oSettings = new Dictionary<String, Object>();

            // Attempt to get the optional table and table columns that contain
            // the settings.

            ListObject oPerWorkbookSettingsTable;
            Range oNameColumnData, oValueColumnData;
            Object [,] aoNameColumnValues, aoValueColumnValues;

            if (
                TryGetPerWorkbookSettingsTable(out oPerWorkbookSettingsTable)
                &&
                ExcelUtil.TryGetTableColumnDataAndValues(
                    oPerWorkbookSettingsTable,
                    PerWorkbookSettingsTableColumnNames.Name,
                    out oNameColumnData, out aoNameColumnValues)
                &&
                ExcelUtil.TryGetTableColumnDataAndValues(
                    oPerWorkbookSettingsTable,
                    PerWorkbookSettingsTableColumnNames.Value,
                    out oValueColumnData, out aoValueColumnValues)
                )
            {
                Int32 iRows = oNameColumnData.Rows.Count;

                for (Int32 iRowOneBased = 1; iRowOneBased <= iRows;
                    iRowOneBased++)
                {
                    String sName;

                    if ( ExcelUtil.TryGetNonEmptyStringFromCell(
                        aoNameColumnValues, iRowOneBased, 1, out sName) )
                    {
                        m_oSettings[sName] =
                            aoValueColumnValues[iRowOneBased, 1];
                    }
                }
            }
        }

        return (m_oSettings);
    }

    //*************************************************************************
    //  Method: WriteAllSettings()
    //
    /// <summary>
    /// Writes all settings from the workbook.
    /// </summary>
    //*************************************************************************

    protected void
    WriteAllSettings()
    {
        AssertValid();
        Debug.Assert(m_oSettings != null);

        ListObject oPerWorkbookSettingsTable;

        if ( !TryGetPerWorkbookSettingsTable(out oPerWorkbookSettingsTable) )
        {
            return;
        }

        // Clear the table.

        ExcelUtil.ClearTable(oPerWorkbookSettingsTable);

        // Attempt to get the optional table columns that contain the settings.

        Range oNameColumnData, oValueColumnData;

        if (
            !ExcelUtil.TryGetTableColumnData(oPerWorkbookSettingsTable,
                PerWorkbookSettingsTableColumnNames.Name, out oNameColumnData)
            ||
            !ExcelUtil.TryGetTableColumnData(oPerWorkbookSettingsTable,
                PerWorkbookSettingsTableColumnNames.Value,
                out oValueColumnData)
            )
        {
            return;
        }

        // Copy the settings to arrays.

        Int32 iSettings = m_oSettings.Count;

        Object [,] aoNameColumnValues =
            ExcelUtil.GetSingleColumn2DArray(iSettings);

        Object [,] aoValueColumnValues =
            ExcelUtil.GetSingleColumn2DArray(iSettings);

        Int32 i = 1;

        foreach (KeyValuePair<String, Object> oKeyValuePair in m_oSettings)
        {
            aoNameColumnValues[i, 1] = oKeyValuePair.Key;
            aoValueColumnValues[i, 1] = oKeyValuePair.Value;
            i++;
        }

        // Write the arrays to the columns.

        ExcelUtil.SetRangeValues(oNameColumnData, aoNameColumnValues);
        ExcelUtil.SetRangeValues(oValueColumnData, aoValueColumnValues);
    }

    //*************************************************************************
    //  Method: TryGetPerWorkbookSettingsTable()
    //
    /// <summary>
    /// Attempts to get the per-workbook settings table.
    /// </summary>
    ///
    /// <param name="oPerWorkbookSettingsTable">
    /// Where the table gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetPerWorkbookSettingsTable
    (
        out ListObject oPerWorkbookSettingsTable
    )
    {
        AssertValid();

        return( ExcelUtil.TryGetTable(m_oWorkbook,
            WorksheetNames.Miscellaneous, TableNames.PerWorkbookSettings,
            out oPerWorkbookSettingsTable) );
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

        Debug.Assert(m_oWorkbook != null);
        // m_oSettings
    }


    //*************************************************************************
    //  Setting name constants
    //*************************************************************************

    /// Name of the TemplateVersion setting.

    protected const String TemplateVersionSettingName = "Template Version";

    /// Name of the BackColor setting.

    protected const String BackColorSettingName = "Background Color";

    /// Name of the BackgroundImageUri setting.

    protected const String BackgroundImageUriSettingName =
        "Background Image";

    /// Name of the GraphDirectedness setting.

    protected const String GraphDirectednessSettingName = "Graph Directedness";

    /// Name of the AutoLayoutOnOpen setting.

    protected const String AutoLayoutOnOpenSettingName =
        "Auto Layout on Open";

    /// Name of the FilteredAlpha setting.

    protected const String FilteredAlphaSettingName = "Filtered Alpha";

    /// Name of the AutoFillWorkbookResults setting.

    protected const String AutoFillWorkbookResultsSettingName =
        "Autofill Workbook Results";

    /// Name of the AutoFillWorkbookWithSchemeResults setting.

    protected const String AutoFillWorkbookWithSchemeResultsSettingName =
        "Autofill Workbook With Scheme Results";



    //*************************************************************************
    //  Default property value constants
    //*************************************************************************

    /// Default value of the TemplateVersion property.

    protected const Int32 DefaultTemplateVersion = 1;

    /// Default value of the GraphDirectedness property.

    protected const GraphDirectedness DefaultGraphDirectedness =
        GraphDirectedness.Undirected;

    /// Default value of the FilteredAlpha property.

    protected const Single DefaultFilteredAlpha = 0;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The workbook containing the settings.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

    /// Dictionary of settings, or null if the settings haven't been read from
    /// the workbook yet.  The key is the setting name and the value is the
    /// setting value.
    ///
    /// Do not use this directly.  Use GetAllSettings() and WriteAllSettings()
    /// instead.

    protected Dictionary<String, Object> m_oSettings;
}
}
