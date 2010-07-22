
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AutoFillUserSettings
//
/// <summary>
/// Stores the user's settings for the application's AutoFill feature.
/// </summary>
/// 
/// <remarks>
/// The AutoFill feature automatically fills edge and vertex attribute columns
/// using values from user-specified source columns.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("AutoFillUserSettings3") ]

[ SettingsProviderAttribute(typeof(
    Microsoft.NodeXL.ExcelTemplate.AutoFillSettingsProvider) ) ]

public class AutoFillUserSettings : NodeXLApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: AutoFillUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the AutoFillUserSettings class.
    /// </summary>
    ///
    /// <param name="workbook">
    /// The workbook being autofilled.
    /// </param>
    //*************************************************************************

    public AutoFillUserSettings
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        Debug.Assert(workbook != null);

        m_oWorkbook = workbook;

        AssertValid();
    }

    //*************************************************************************
    //  Property: EdgeColorSourceColumnName
    //
    /// <summary>
    /// Gets or sets the name of the column to use as a data source for the
    /// edge color column.
    /// </summary>
    ///
    /// <value>
    /// The name of the column to use as a data source for the edge color
    /// column.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    EdgeColorSourceColumnName
    {
        get
        {
            AssertValid();

            return ( (String)this[EdgeColorSourceColumnNameKey] );
        }

        set
        {
            this[EdgeColorSourceColumnNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeColorDetails
    //
    /// <summary>
    /// Gets or sets the details for auto-filling the edge color column.
    /// </summary>
    ///
    /// <value>
    /// The details for auto-filling the edge color column.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute(
        "false\tfalse\t0\t10\tRed\tGreen\tfalse\tfalse") ]

    public ColorColumnAutoFillUserSettings
    EdgeColorDetails
    {
        get
        {
            AssertValid();

            return ( (ColorColumnAutoFillUserSettings)
                this[EdgeColorDetailsKey] );
        }

        set
        {
            this[EdgeColorDetailsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeWidthSourceColumnName
    //
    /// <summary>
    /// Gets or sets the name of the column to use as a data source for the
    /// edge width column.
    /// </summary>
    ///
    /// <value>
    /// The name of the column to use as a data source for the edge width
    /// column.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    EdgeWidthSourceColumnName
    {
        get
        {
            AssertValid();

            return ( (String)this[EdgeWidthSourceColumnNameKey] );
        }

        set
        {
            this[EdgeWidthSourceColumnNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeWidthDetails
    //
    /// <summary>
    /// Gets or sets the details for auto-filling the edge width column.
    /// </summary>
    ///
    /// <value>
    /// The details for auto-filling the edge width column.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute(
        "false\tfalse\t1\t10\t1\t10\tfalse\tfalse") ]

    public NumericRangeColumnAutoFillUserSettings
    EdgeWidthDetails
    {
        get
        {
            AssertValid();

            return ( (NumericRangeColumnAutoFillUserSettings)
                this[EdgeWidthDetailsKey] );
        }

        set
        {
            this[EdgeWidthDetailsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeAlphaSourceColumnName
    //
    /// <summary>
    /// Gets or sets the name of the column to use as a data source for the
    /// edge alpha column.
    /// </summary>
    ///
    /// <value>
    /// The name of the column to use as a data source for the edge color
    /// column.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    EdgeAlphaSourceColumnName
    {
        get
        {
            AssertValid();

            return ( (String)this[EdgeAlphaSourceColumnNameKey] );
        }

        set
        {
            this[EdgeAlphaSourceColumnNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeAlphaDetails
    //
    /// <summary>
    /// Gets or sets the details for auto-filling the edge alpha column.
    /// </summary>
    ///
    /// <value>
    /// The details for auto-filling the edge alpha column.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute(
        "false\tfalse\t0\t100\t10\t100\tfalse\tfalse") ]

    public NumericRangeColumnAutoFillUserSettings
    EdgeAlphaDetails
    {
        get
        {
            AssertValid();

            return ( (NumericRangeColumnAutoFillUserSettings)
                this[EdgeAlphaDetailsKey] );
        }

        set
        {
            this[EdgeAlphaDetailsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeVisibilitySourceColumnName
    //
    /// <summary>
    /// Gets or sets the name of the column to use as a data source for the
    /// edge visibility column.
    /// </summary>
    ///
    /// <value>
    /// The name of the column to use as a data source for the edge visibility
    /// column.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    EdgeVisibilitySourceColumnName
    {
        get
        {
            AssertValid();

            return ( (String)this[EdgeVisibilitySourceColumnNameKey] );
        }

        set
        {
            this[EdgeVisibilitySourceColumnNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeVisibilityDetails
    //
    /// <summary>
    /// Gets or sets the details for auto-filling the edge visibility column.
    /// </summary>
    ///
    /// <value>
    /// The details for auto-filling the edge visibility column.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("GreaterThan\t0\t1\t0") ]

    public NumericComparisonColumnAutoFillUserSettings
    EdgeVisibilityDetails
    {
        get
        {
            AssertValid();

            return ( (NumericComparisonColumnAutoFillUserSettings)
                this[EdgeVisibilityDetailsKey] );
        }

        set
        {
            this[EdgeVisibilityDetailsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeLabelSourceColumnName
    //
    /// <summary>
    /// Gets or sets the name of the column to use as a data source for the
    /// edge label column.
    /// </summary>
    ///
    /// <value>
    /// The name of the column to use as a data source for the edge label
    /// column.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    EdgeLabelSourceColumnName
    {
        get
        {
            AssertValid();

            return ( (String)this[EdgeLabelSourceColumnNameKey] );
        }

        set
        {
            this[EdgeLabelSourceColumnNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexColorSourceColumnName
    //
    /// <summary>
    /// Gets or sets the name of the column to use as a data source for the
    /// vertex color column.
    /// </summary>
    ///
    /// <value>
    /// The name of the column to use as a data source for the vertex color
    /// column.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    VertexColorSourceColumnName
    {
        get
        {
            AssertValid();

            return ( (String)this[VertexColorSourceColumnNameKey] );
        }

        set
        {
            this[VertexColorSourceColumnNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexColorDetails
    //
    /// <summary>
    /// Gets or sets the details for auto-filling the vertex color column.
    /// </summary>
    ///
    /// <value>
    /// The details for auto-filling the vertex color column.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute(
        "false\tfalse\t0\t10\tRed\tGreen\tfalse\tfalse") ]

    public ColorColumnAutoFillUserSettings
    VertexColorDetails
    {
        get
        {
            AssertValid();

            return ( (ColorColumnAutoFillUserSettings)
                this[VertexColorDetailsKey] );
        }

        set
        {
            this[VertexColorDetailsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexShapeSourceColumnName
    //
    /// <summary>
    /// Gets or sets the name of the column to use as a data source for the
    /// vertex shape column.
    /// </summary>
    ///
    /// <value>
    /// The name of the column to use as a data source for the vertex shape
    /// column.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    VertexShapeSourceColumnName
    {
        get
        {
            AssertValid();

            return ( (String)this[VertexShapeSourceColumnNameKey] );
        }

        set
        {
            this[VertexShapeSourceColumnNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexShapeDetails
    //
    /// <summary>
    /// Gets or sets the details for auto-filling the vertex shape column.
    /// </summary>
    ///
    /// <value>
    /// The details for auto-filling the vertex shape column.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("GreaterThan\t0\tSphere\t") ]

    public NumericComparisonColumnAutoFillUserSettings
    VertexShapeDetails
    {
        get
        {
            AssertValid();

            return ( (NumericComparisonColumnAutoFillUserSettings)
                this[VertexShapeDetailsKey] );
        }

        set
        {
            this[VertexShapeDetailsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexRadiusSourceColumnName
    //
    /// <summary>
    /// Gets or sets the name of the column to use as a data source for the
    /// vertex radius column.
    /// </summary>
    ///
    /// <value>
    /// The name of the column to use as a data source for the vertex radius
    /// column.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    VertexRadiusSourceColumnName
    {
        get
        {
            AssertValid();

            return ( (String)this[VertexRadiusSourceColumnNameKey] );
        }

        set
        {
            this[VertexRadiusSourceColumnNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexRadiusDetails
    //
    /// <summary>
    /// Gets or sets the details for auto-filling the vertex radius column.
    /// </summary>
    ///
    /// <value>
    /// The details for auto-filling the vertex radius column.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute(
        "false\tfalse\t1\t10\t1.5\t10\tfalse\tfalse") ]

    public NumericRangeColumnAutoFillUserSettings
    VertexRadiusDetails
    {
        get
        {
            AssertValid();

            return ( (NumericRangeColumnAutoFillUserSettings)
                this[VertexRadiusDetailsKey] );
        }

        set
        {
            this[VertexRadiusDetailsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexAlphaSourceColumnName
    //
    /// <summary>
    /// Gets or sets the name of the column to use as a data source for the
    /// vertex alpha column.
    /// </summary>
    ///
    /// <value>
    /// The name of the column to use as a data source for the vertex alpha
    /// column.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    VertexAlphaSourceColumnName
    {
        get
        {
            AssertValid();

            return ( (String)this[VertexAlphaSourceColumnNameKey] );
        }

        set
        {
            this[VertexAlphaSourceColumnNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexAlphaDetails
    //
    /// <summary>
    /// Gets or sets the details for auto-filling the vertex alpha column.
    /// </summary>
    ///
    /// <value>
    /// The details for auto-filling the vertex alpha column.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute(
        "false\tfalse\t0\t100\t10\t100\tfalse\tfalse") ]

    public NumericRangeColumnAutoFillUserSettings
    VertexAlphaDetails
    {
        get
        {
            AssertValid();

            return ( (NumericRangeColumnAutoFillUserSettings)
                this[VertexAlphaDetailsKey] );
        }

        set
        {
            this[VertexAlphaDetailsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexLabelSourceColumnName
    //
    /// <summary>
    /// Gets or sets the name of the column to use as a data source for the
    /// vertex label column.
    /// </summary>
    ///
    /// <value>
    /// The name of the column to use as a data source for the vertex label
    /// column.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    VertexLabelSourceColumnName
    {
        get
        {
            AssertValid();

            return ( (String)this[VertexLabelSourceColumnNameKey] );
        }

        set
        {
            this[VertexLabelSourceColumnNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexLabelFillColorSourceColumnName
    //
    /// <summary>
    /// Gets or sets the name of the column to use as a data source for the
    /// vertex label fill color column.
    /// </summary>
    ///
    /// <value>
    /// The name of the column to use as a data source for the vertex label
    /// fill color column.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    VertexLabelFillColorSourceColumnName
    {
        get
        {
            AssertValid();

            return ( (String)this[VertexLabelFillColorSourceColumnNameKey] );
        }

        set
        {
            this[VertexLabelFillColorSourceColumnNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexLabelFillColorDetails
    //
    /// <summary>
    /// Gets or sets the details for auto-filling the vertex label fill color
    /// column.
    /// </summary>
    ///
    /// <value>
    /// The details for auto-filling the vertex label fill color
    /// column.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute(
        "false\tfalse\t0\t10\tRed\tGreen\tfalse\tfalse") ]

    public ColorColumnAutoFillUserSettings
    VertexLabelFillColorDetails
    {
        get
        {
            AssertValid();

            return ( (ColorColumnAutoFillUserSettings)
                this[VertexLabelFillColorDetailsKey] );
        }

        set
        {
            this[VertexLabelFillColorDetailsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexToolTipSourceColumnName
    //
    /// <summary>
    /// Gets or sets the name of the column to use as a data source for the
    /// vertex tooltip column.
    /// </summary>
    ///
    /// <value>
    /// The name of the column to use as a data source for the vertex tooltip
    /// column.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    VertexToolTipSourceColumnName
    {
        get
        {
            AssertValid();

            return ( (String)this[VertexToolTipSourceColumnNameKey] );
        }

        set
        {
            this[VertexToolTipSourceColumnNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexVisibilitySourceColumnName
    //
    /// <summary>
    /// Gets or sets the name of the column to use as a data source for the
    /// vertex visibility column.
    /// </summary>
    ///
    /// <value>
    /// The name of the column to use as a data source for the vertex
    /// visibility column.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    VertexVisibilitySourceColumnName
    {
        get
        {
            AssertValid();

            return ( (String)this[VertexVisibilitySourceColumnNameKey] );
        }

        set
        {
            this[VertexVisibilitySourceColumnNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexVisibilityDetails
    //
    /// <summary>
    /// Gets or sets the details for auto-filling the vertex visibility column.
    /// </summary>
    ///
    /// <value>
    /// The details for auto-filling the vertex visibility column.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("GreaterThan\t0\t1\t0") ]

    public NumericComparisonColumnAutoFillUserSettings
    VertexVisibilityDetails
    {
        get
        {
            AssertValid();

            return ( (NumericComparisonColumnAutoFillUserSettings)
                this[VertexVisibilityDetailsKey] );
        }

        set
        {
            this[VertexVisibilityDetailsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexXSourceColumnName
    //
    /// <summary>
    /// Gets or sets the name of the column to use as a data source for the
    /// vertex X-coordinate column.
    /// </summary>
    ///
    /// <value>
    /// The name of the column to use as a data source for the vertex
    /// X-coordinate column.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    VertexXSourceColumnName
    {
        get
        {
            AssertValid();

            return ( (String)this[VertexXSourceColumnNameKey] );
        }

        set
        {
            this[VertexXSourceColumnNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexXDetails
    //
    /// <summary>
    /// Gets or sets the details for auto-filling the vertex x-coordinate
    /// column.
    /// </summary>
    ///
    /// <value>
    /// The details for auto-filling the vertex x-coordinate column.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute(
        "false\tfalse\t0\t0\t0\t9999\tfalse\tfalse") ]

    public NumericRangeColumnAutoFillUserSettings
    VertexXDetails
    {
        get
        {
            AssertValid();

            return ( (NumericRangeColumnAutoFillUserSettings)
                this[VertexXDetailsKey] );
        }

        set
        {
            this[VertexXDetailsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexYSourceColumnName
    //
    /// <summary>
    /// Gets or sets the name of the column to use as a data source for the
    /// vertex y-coordinate column.
    /// </summary>
    ///
    /// <value>
    /// The name of the column to use as a data source for the vertex
    /// y-coordinate column.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    VertexYSourceColumnName
    {
        get
        {
            AssertValid();

            return ( (String)this[VertexYSourceColumnNameKey] );
        }

        set
        {
            this[VertexYSourceColumnNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexYDetails
    //
    /// <summary>
    /// Gets or sets the details for auto-filling the vertex y-coordinate
    /// column.
    /// </summary>
    ///
    /// <value>
    /// The details for auto-filling the vertex y-coordinate column.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute(
        "false\tfalse\t0\t0\t0\t9999\tfalse\tfalse") ]

    public NumericRangeColumnAutoFillUserSettings
    VertexYDetails
    {
        get
        {
            AssertValid();

            return ( (NumericRangeColumnAutoFillUserSettings)
                this[VertexYDetailsKey] );
        }

        set
        {
            this[VertexYDetailsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexLayoutOrderSourceColumnName
    //
    /// <summary>
    /// Gets or sets the name of the column to use as a data source for the
    /// vertex layout order column.
    /// </summary>
    ///
    /// <value>
    /// The name of the column to use as a data source for the vertex
    /// layout order column.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    VertexLayoutOrderSourceColumnName
    {
        get
        {
            AssertValid();

            return ( (String)this[VertexLayoutOrderSourceColumnNameKey] );
        }

        set
        {
            this[VertexLayoutOrderSourceColumnNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexLayoutOrderDetails
    //
    /// <summary>
    /// Gets or sets the details for auto-filling the vertex layout order
    /// column.
    /// </summary>
    ///
    /// <value>
    /// The details for auto-filling the vertex layout order column.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute(
        "false\tfalse\t0\t0\t1\t9999\tfalse\tfalse") ]

    public NumericRangeColumnAutoFillUserSettings
    VertexLayoutOrderDetails
    {
        get
        {
            AssertValid();

            return ( (NumericRangeColumnAutoFillUserSettings)
                this[VertexLayoutOrderDetailsKey] );
        }

        set
        {
            this[VertexLayoutOrderDetailsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexPolarRSourceColumnName
    //
    /// <summary>
    /// Gets or sets the name of the column to use as a data source for the
    /// vertex polar R-coordinate column.
    /// </summary>
    ///
    /// <value>
    /// The name of the column to use as a data source for the vertex
    /// R-coordindate column.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    VertexPolarRSourceColumnName
    {
        get
        {
            AssertValid();

            return ( (String)this[VertexPolarRSourceColumnNameKey] );
        }

        set
        {
            this[VertexPolarRSourceColumnNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexPolarRDetails
    //
    /// <summary>
    /// Gets or sets the details for auto-filling the vertex polar R-coordinate
    /// column.
    /// </summary>
    ///
    /// <value>
    /// The details for auto-filling the vertex polar R-coordinate column.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute(
        "false\tfalse\t0\t0\t0\t1\tfalse\tfalse") ]

    public NumericRangeColumnAutoFillUserSettings
    VertexPolarRDetails
    {
        get
        {
            AssertValid();

            return ( (NumericRangeColumnAutoFillUserSettings)
                this[VertexPolarRDetailsKey] );
        }

        set
        {
            this[VertexPolarRDetailsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexPolarAngleSourceColumnName
    //
    /// <summary>
    /// Gets or sets the name of the column to use as a data source for the
    /// vertex polar angle-coordinate column.
    /// </summary>
    ///
    /// <value>
    /// The name of the column to use as a data source for the vertex
    /// angle-coordindate column.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    VertexPolarAngleSourceColumnName
    {
        get
        {
            AssertValid();

            return ( (String)this[VertexPolarAngleSourceColumnNameKey] );
        }

        set
        {
            this[VertexPolarAngleSourceColumnNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexPolarAngleDetails
    //
    /// <summary>
    /// Gets or sets the details for auto-filling the vertex polar
    /// angle-coordinate column.
    /// </summary>
    ///
    /// <value>
    /// The details for auto-filling the vertex polar angle-coordinate column.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute(
        "false\tfalse\t0\t0\t0\t359\tfalse\tfalse") ]

    public NumericRangeColumnAutoFillUserSettings
    VertexPolarAngleDetails
    {
        get
        {
            AssertValid();

            return ( (NumericRangeColumnAutoFillUserSettings)
                this[VertexPolarAngleDetailsKey] );
        }

        set
        {
            this[VertexPolarAngleDetailsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SettingsContext
    //
    /// <summary>
    /// Gets the application settings context associated with the settings
    /// group.
    /// </summary>
    ///
    /// <value>
    /// A SettingsContext associated with the settings group.
    /// </value>
    //*************************************************************************

    public override SettingsContext
    Context
    {
        get
        {
            AssertValid();

            // Make the PerWorkbookSettings object available to the
            // AutoFillSettingsProvider class.

            SettingsContext oContext = base.Context;

            oContext[PerWorkbookSettingsKeyName] =
                new PerWorkbookSettings(m_oWorkbook);

            return (oContext);
        }
    }

    //*************************************************************************
    //  Method: Reset()
    //
    /// <summary>
    /// Restores the persisted application settings values to their
    /// corresponding default properties.
    /// </summary>
    //*************************************************************************

    public new void
    Reset()
    {
        AssertValid();

        // Clear any per-workbook settings.

        ( new PerWorkbookSettings(m_oWorkbook) ).AutoFillWorkbookSettings =
            null;

        base.Reset();
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
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// Name of the key that gets added to the SettingsContext object.  The
    /// value is a PerWorkbookSettings object.

    public const String PerWorkbookSettingsKeyName = "PerWorkbookSettings";


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Name of the settings key for the EdgeColorSourceColumnName property.

    protected const String EdgeColorSourceColumnNameKey =
        "EdgeColorSourceColumnName";

    /// Name of the settings key for the EdgeColorDetailsKey property.

    protected const String EdgeColorDetailsKey =
        "EdgeColorDetails";

    /// Name of the settings key for the EdgeWidthSourceColumnName property.

    protected const String EdgeWidthSourceColumnNameKey =
        "EdgeWidthSourceColumnName";

    /// Name of the settings key for the EdgeWidthDetailsKey property.

    protected const String EdgeWidthDetailsKey =
        "EdgeWidthDetails";

    /// Name of the settings key for the EdgeAlphaSourceColumnName property.

    protected const String EdgeAlphaSourceColumnNameKey =
        "EdgeAlphaSourceColumnName";

    /// Name of the settings key for the EdgeAlphaDetailsKey property.

    protected const String EdgeAlphaDetailsKey =
        "EdgeAlphaDetails";

    /// Name of the settings key for the EdgeVisibilitySourceColumnName
    /// property.

    protected const String EdgeVisibilitySourceColumnNameKey =
        "EdgeVisibilitySourceColumnName";

    /// Name of the settings key for the EdgeVisibilityDetailsKey property.

    protected const String EdgeVisibilityDetailsKey =
        "EdgeVisibilityDetails";

    /// Name of the settings key for the EdgeLabelSourceColumnName property.

    protected const String EdgeLabelSourceColumnNameKey =
        "EdgeLabelSourceColumnName";

    /// Name of the settings key for the VertexColorSourceColumnName property.

    protected const String VertexColorSourceColumnNameKey =
        "VertexColorSourceColumnName";

    /// Name of the settings key for the VertexColorDetailsKey property.

    protected const String VertexColorDetailsKey =
        "VertexColorDetails";

    /// Name of the settings key for the VertexShapeSourceColumnName property.

    protected const String VertexShapeSourceColumnNameKey =
        "VertexShapeSourceColumnName";

    /// Name of the settings key for the VertexShapeDetailsKey property.

    protected const String VertexShapeDetailsKey =
        "VertexShapeDetails";

    /// Name of the settings key for the VertexRadiusSourceColumnName property.

    protected const String VertexRadiusSourceColumnNameKey =
        "VertexRadiusSourceColumnName";

    /// Name of the settings key for the VertexRadiusDetailsKey property.

    protected const String VertexRadiusDetailsKey =
        "VertexRadiusDetails";

    /// Name of the settings key for the VertexAlphaSourceColumnName
    /// property.

    protected const String VertexAlphaSourceColumnNameKey =
        "VertexAlphaSourceColumnName";

    /// Name of the settings key for the VertexAlphaDetailsKey property.

    protected const String VertexAlphaDetailsKey =
        "VertexAlphaDetails";

    /// Name of the settings key for the VertexLabelSourceColumnName property.

    protected const String VertexLabelSourceColumnNameKey =
        "VertexLabelSourceColumnName";

    /// Name of the settings key for the
    /// VertexLabelFillColorSourceColumnName property.

    protected const String VertexLabelFillColorSourceColumnNameKey =
        "VertexLabelFillColorSourceColumnName";

    /// Name of the settings key for the VertexLabelFillColorDetailsKey
    /// property.

    protected const String VertexLabelFillColorDetailsKey =
        "VertexLabelFillColorDetails";

    /// Name of the settings key for the VertexToolTipSourceColumnName
    /// property.

    protected const String VertexToolTipSourceColumnNameKey =
        "VertexToolTipSourceColumnName";

    /// Name of the settings key for the VertexVisibilitySourceColumnName
    /// property.

    protected const String VertexVisibilitySourceColumnNameKey =
        "VertexVisibilitySourceColumnName";

    /// Name of the settings key for the VertexVisibilityDetailsKey property.

    protected const String VertexVisibilityDetailsKey =
        "VertexVisibilityDetails";

    /// Name of the settings key for the VertexXSourceColumnName property.

    protected const String VertexXSourceColumnNameKey =
        "VertexXSourceColumnName";

    /// Name of the settings key for the VertexXDetailsKey property.

    protected const String VertexXDetailsKey =
        "VertexXDetails";

    /// Name of the settings key for the VertexYSourceColumnName property.

    protected const String VertexYSourceColumnNameKey =
        "VertexYSourceColumnName";

    /// Name of the settings key for the VertexYDetailsKey property.

    protected const String VertexYDetailsKey =
        "VertexYDetails";

    /// Name of the settings key for the VertexLayoutOrderSourceColumnName
    /// property.

    protected const String VertexLayoutOrderSourceColumnNameKey =
        "VertexLayoutOrderSourceColumnName";

    /// Name of the settings key for the VertexLayoutOrderDetailsKey property.

    protected const String VertexLayoutOrderDetailsKey =
        "VertexLayoutOrderDetails";

    /// Name of the settings key for the VertexPolarRSourceColumnName property.

    protected const String VertexPolarRSourceColumnNameKey =
        "VertexPolarRSourceColumnName";

    /// Name of the settings key for the VertexPolarRDetailsKey property.

    protected const String VertexPolarRDetailsKey =
        "VertexPolarRDetails";

    /// Name of the settings key for the VertexPolarAngleSourceColumnName
    /// property.

    protected const String VertexPolarAngleSourceColumnNameKey =
        "VertexPolarAngleSourceColumnName";

    /// Name of the settings key for the VertexPolarAngleDetailsKey property.

    protected const String VertexPolarAngleDetailsKey =
        "VertexPolarAngleDetails";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The workbook being autofilled.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;
}


//*****************************************************************************
//  Class: AutoFillSettingsProvider
//
/// <summary>
/// Settings provider for the AutoFillUserSettings class.
/// </summary>
/// 
/// <remarks>
/// Here is how the autofill user settings should behave:
///
/// <list type="number">
///
/// <item><description>
/// For a new workbook, the settings should be retrieved from the user's
/// settings file by LocalFileSettingsProvider.
/// </description></item>
///
/// <item><description>
/// When the user edits the settings, they should be stored in both the user's
/// settings file AND in the workbook.
/// </description></item>
///
/// <item><description>
/// When the autofill settings are needed again, they should be retrieved from
/// the workbook.
/// </description></item>
///
/// <item><description>
/// The net result is that the autofill user settings travel with the workbook.
/// </description></item>
///
/// </list>
///
/// <para>
/// This behavior is achieved by implementing this custom SettingsProvider for
/// the AutoFillUserSettings class.  It overrides the <see
/// cref="GetPropertyValues" /> and <see cref="SetPropertyValues" /> methods.
/// <see cref="SetPropertyValues" /> stores the values in both the user's
/// settings file and in the workbook's PerWorkbookSettings object, and <see
/// cref="GetPropertyValues" /> overrides the settings from the file with the
/// settings from the PerWorkbookSettings object, if such per-workbook settings
/// exist.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class AutoFillSettingsProvider : LocalFileSettingsProvider
{
    //*************************************************************************
    //  Method: SetPropertyValues()
    //
    /// <summary>
    /// Sets the values of the specified group of property settings.
    /// </summary>
    ///
    /// <param name="context">
    /// A SettingsContext describing the current application usage.
    /// </param>
    ///
    /// <param name="values">
    /// A SettingsPropertyValueCollection representing the group of property
    /// settings to set.
    /// </param>
    //*************************************************************************

    public override void
    SetPropertyValues
    (
        SettingsContext context,
        SettingsPropertyValueCollection values
    )
    {
        AssertValid();

        String [] asAutoFillWorkbookSettings = new String[values.Count * 2];

        // Join the name/value pairs into a single composite string and store
        // the composite in the PerWorkbook settings.

        Int32 i = 0;

        foreach (SettingsPropertyValue oSettingsPropertyValue in values)
        {
            asAutoFillWorkbookSettings[i + 0] = oSettingsPropertyValue.Name;

            asAutoFillWorkbookSettings[i + 1] =
                oSettingsPropertyValue.SerializedValue.ToString();

            i += 2;
        }

        ( GetPerWorkbookSettings(context) ).AutoFillWorkbookSettings =
            String.Join(PerWorkbookSettings.FieldSeparatorString,
                asAutoFillWorkbookSettings);

        // Let the base class store the settings in the user's settings file.

        base.SetPropertyValues(context, values);
    }

    //*************************************************************************
    //  Method: GetPropertyValues()
    //
    /// <summary>
    /// Returns the collection of setting property values for the specified
    /// application instance and settings property group.
    /// </summary>
    ///
    /// <param name="context">
    /// A SettingsContext describing the current application usage.
    /// </param>
    ///
    /// <param name="properties">
    /// A SettingsPropertyCollection containing the settings property group
    /// whose values are to be retrieved.
    /// </param>
    //*************************************************************************

    public override SettingsPropertyValueCollection
    GetPropertyValues
    (
        SettingsContext context,
        SettingsPropertyCollection properties
    )
    {
        // Let the base class get the settings from the user's settings file,
        // or from the default values.

        SettingsPropertyValueCollection oValues =
            base.GetPropertyValues(context, properties);

        // Has SetPropertyValues() stored settings in the PerWorkbookSettings
        // object?

        String sAutoFillWorkbookSettings =
            ( GetPerWorkbookSettings(context) ).AutoFillWorkbookSettings;

        if (sAutoFillWorkbookSettings != null)
        {
            // Yes.  Split the composite string into name/value pairs.

            String [] asNameValuePairs = sAutoFillWorkbookSettings.Split(
                PerWorkbookSettings.FieldSeparator);

            Int32 iNamesAndValues = asNameValuePairs.Length;

            // To allow for future revisions in the name/value pairs, this
            // method is forgiving of various unexpected conditions.

            if (iNamesAndValues % 2 == 0)
            {
                for (Int32 i = 0; i < iNamesAndValues; i += 2)
                {
                    String sName = asNameValuePairs[i + 0];
                    String sValue = asNameValuePairs[i + 1];

                    if ( !String.IsNullOrEmpty(sName) )
                    {
                        SettingsPropertyValue oSettingsPropertyValue =
                            oValues[sName];

                        if (oSettingsPropertyValue != null)
                        {
                            // Override the value retrieved by the base class.

                            oSettingsPropertyValue.SerializedValue =
                                sValue;
                        }
                    }
                }
            }
        }

        return (oValues);
    }

    //*************************************************************************
    //  Method: GetPerWorkbookSettings()
    //
    /// <summary>
    /// Gets the <see cref="PerWorkbookSettings" /> from the SettingsContext
    /// object.
    /// </summary>
    ///
    /// <param name="oContext">
    /// A SettingsContext describing the current application usage.
    /// </param>
    ///
    /// <returns>
    /// The PerWorkbookSettings object stored within <paramref
    /// name="context" />.
    /// </returns>
    //*************************************************************************

    protected PerWorkbookSettings
    GetPerWorkbookSettings
    (
        SettingsContext oContext
    )
    {
        Debug.Assert(oContext != null);

        // AutoFillUserSettings.SettingsContext stored the PerWorkbookSettings
        // object using a known key.

        Object oPerWorkbookSettingsAsObject =
            oContext[AutoFillUserSettings.PerWorkbookSettingsKeyName];

        Debug.Assert(oPerWorkbookSettingsAsObject != null);
        Debug.Assert(oPerWorkbookSettingsAsObject is PerWorkbookSettings);

        return ( (PerWorkbookSettings)oPerWorkbookSettingsAsObject );
    }


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
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
