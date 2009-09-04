
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

public class AutoFillUserSettings : ApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: AutoFillUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the AutoFillUserSettings class.
    /// </summary>
    //*************************************************************************

    public AutoFillUserSettings()
    {
        // (Do nothing.)

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
    //  Property: VertexPrimaryLabelSourceColumnName
    //
    /// <summary>
    /// Gets or sets the name of the column to use as a data source for the
    /// vertex primary label column.
    /// </summary>
    ///
    /// <value>
    /// The name of the column to use as a data source for the vertex primary
    /// label column.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    VertexPrimaryLabelSourceColumnName
    {
        get
        {
            AssertValid();

            return ( (String)this[VertexPrimaryLabelSourceColumnNameKey] );
        }

        set
        {
            this[VertexPrimaryLabelSourceColumnNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexPrimaryLabelFillColorSourceColumnName
    //
    /// <summary>
    /// Gets or sets the name of the column to use as a data source for the
    /// vertex primary label fill color column.
    /// </summary>
    ///
    /// <value>
    /// The name of the column to use as a data source for the vertex primary
    /// label fill color column.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    VertexPrimaryLabelFillColorSourceColumnName
    {
        get
        {
            AssertValid();

            return ( (String)this[
                VertexPrimaryLabelFillColorSourceColumnNameKey] );
        }

        set
        {
            this[VertexPrimaryLabelFillColorSourceColumnNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexPrimaryLabelFillColorDetails
    //
    /// <summary>
    /// Gets or sets the details for auto-filling the vertex primary label fill
    /// color column.
    /// </summary>
    ///
    /// <value>
    /// The details for auto-filling the vertex primary label fill color
    /// column.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute(
        "false\tfalse\t0\t10\tRed\tGreen\tfalse\tfalse") ]

    public ColorColumnAutoFillUserSettings
    VertexPrimaryLabelFillColorDetails
    {
        get
        {
            AssertValid();

            return ( (ColorColumnAutoFillUserSettings)
                this[VertexPrimaryLabelFillColorDetailsKey] );
        }

        set
        {
            this[VertexPrimaryLabelFillColorDetailsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexSecondaryLabelSourceColumnName
    //
    /// <summary>
    /// Gets or sets the name of the column to use as a data source for the
    /// vertex secondary label column.
    /// </summary>
    ///
    /// <value>
    /// The name of the column to use as a data source for the vertex secondary
    /// label column.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    VertexSecondaryLabelSourceColumnName
    {
        get
        {
            AssertValid();

            return ( (String)this[VertexSecondaryLabelSourceColumnNameKey] );
        }

        set
        {
            this[VertexSecondaryLabelSourceColumnNameKey] = value;

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
    //  Method: Copy()
    //
    /// <summary>
    /// Creates a deep copy of the object.
    /// </summary>
    ///
    /// <returns>
    /// A deep copy of the object.
    /// </returns>
    //*************************************************************************

    public AutoFillUserSettings
    Copy()
    {
        AssertValid();

        AutoFillUserSettings oCopy = new AutoFillUserSettings();

        oCopy.EdgeColorSourceColumnName = String.Copy(
            this.EdgeColorSourceColumnName);

        oCopy.EdgeColorDetails = this.EdgeColorDetails.Copy();

        oCopy.EdgeWidthSourceColumnName = String.Copy(
            this.EdgeWidthSourceColumnName);

        oCopy.EdgeWidthDetails = this.EdgeWidthDetails.Copy();

        oCopy.EdgeAlphaSourceColumnName = String.Copy(
            this.EdgeAlphaSourceColumnName);

        oCopy.EdgeAlphaDetails = this.EdgeAlphaDetails.Copy();

        oCopy.EdgeVisibilitySourceColumnName = String.Copy(
            this.EdgeVisibilitySourceColumnName);

        oCopy.EdgeVisibilityDetails = this.EdgeVisibilityDetails.Copy();

        oCopy.VertexColorSourceColumnName = String.Copy(
            this.VertexColorSourceColumnName);

        oCopy.VertexColorDetails = this.VertexColorDetails.Copy();

        oCopy.VertexShapeSourceColumnName = String.Copy(
            this.VertexShapeSourceColumnName);

        oCopy.VertexShapeDetails = this.VertexShapeDetails.Copy();

        oCopy.VertexRadiusSourceColumnName = String.Copy(
            this.VertexRadiusSourceColumnName);

        oCopy.VertexRadiusDetails = this.VertexRadiusDetails.Copy();

        oCopy.VertexAlphaSourceColumnName = String.Copy(
            this.VertexAlphaSourceColumnName);

        oCopy.VertexAlphaDetails = this.VertexAlphaDetails.Copy();

        oCopy.VertexPrimaryLabelSourceColumnName = String.Copy(
            this.VertexPrimaryLabelSourceColumnName);

        oCopy.VertexSecondaryLabelSourceColumnName = String.Copy(
            this.VertexSecondaryLabelSourceColumnName);

        oCopy.VertexToolTipSourceColumnName = String.Copy(
            this.VertexToolTipSourceColumnName);

        oCopy.VertexVisibilitySourceColumnName = String.Copy(
            this.VertexVisibilitySourceColumnName);

        oCopy.VertexVisibilityDetails = this.VertexVisibilityDetails.Copy();

        oCopy.VertexXSourceColumnName = String.Copy(
            this.VertexXSourceColumnName);

        oCopy.VertexXDetails = this.VertexXDetails.Copy();

        oCopy.VertexYSourceColumnName = String.Copy(
            this.VertexYSourceColumnName);

        oCopy.VertexYDetails = this.VertexYDetails.Copy();

        oCopy.VertexLayoutOrderSourceColumnName = String.Copy(
            this.VertexLayoutOrderSourceColumnName);

        oCopy.VertexLayoutOrderDetails = this.VertexLayoutOrderDetails.Copy();

        oCopy.VertexPolarRSourceColumnName = String.Copy(
            this.VertexPolarRSourceColumnName);

        oCopy.VertexPolarRDetails = this.VertexPolarRDetails.Copy();

        oCopy.VertexPolarAngleSourceColumnName = String.Copy(
            this.VertexPolarAngleSourceColumnName);

        oCopy.VertexPolarAngleDetails = this.VertexPolarAngleDetails.Copy();

        return (oCopy);
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

    /// Name of the settings key for the VertexPrimaryLabelSourceColumnName
    /// property.

    protected const String VertexPrimaryLabelSourceColumnNameKey =
        "VertexPrimaryLabelSourceColumnName";

    /// Name of the settings key for the
    /// VertexPrimaryLabelFillColorSourceColumnName property.

    protected const String VertexPrimaryLabelFillColorSourceColumnNameKey =
        "VertexPrimaryLabelFillColorSourceColumnName";

    /// Name of the settings key for the VertexPrimaryLabelFillColorDetailsKey
    /// property.

    protected const String VertexPrimaryLabelFillColorDetailsKey =
        "VertexPrimaryLabelFillColorDetails";

    /// Name of the settings key for the VertexSecondaryLabelSourceColumnName
    /// property.

    protected const String VertexSecondaryLabelSourceColumnNameKey =
        "VertexSecondaryLabelSourceColumnName";

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

    // (None.)
}

}
