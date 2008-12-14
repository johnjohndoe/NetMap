
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphMetricUserSettings
//
/// <summary>
/// Stores the user's settings for calculating graph metrics.
/// </summary>
//*****************************************************************************

[ SettingsGroupNameAttribute("GraphMetricUserSettings") ]

public class GraphMetricUserSettings : ApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: GraphMetricUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the GraphMetricUserSettings class.
    /// </summary>
    //*************************************************************************

    public GraphMetricUserSettings()
    {
		// (Do nothing.)

		AssertValid();
    }

    //*************************************************************************
    //  Property: AtLeastOneMetricSelected
    //
    /// <summary>
    /// Gets a flag indicating whether at least one set of graph metrics should
	/// be calculated.
    /// </summary>
    ///
    /// <value>
	/// true if at least one set of graph metrics should be calculated.
    /// </value>
    //*************************************************************************

    public Boolean
	AtLeastOneMetricSelected
    {
        get
        {
            AssertValid();

			return(this.CalculateInDegree || this.CalculateOutDegree ||
				this.CalculateDegree || this.CalculateClusteringCoefficient ||
				this.CalculateBetweennessCentrality ||
				this.CalculateOverallMetrics
				);
        }
    }

    //*************************************************************************
    //  Property: CalculateInDegree
    //
    /// <summary>
    /// Gets or sets a flag specifying whether vertex in-degrees should be
	/// calculated.
    /// </summary>
    ///
    /// <value>
	/// true to calculate vertex in-degrees.  The default is true.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("true") ]

    public Boolean
	CalculateInDegree
    {
        get
        {
            AssertValid();

			return ( (Boolean)this[CalculateInDegreeKey] );
        }

        set
        {
			this[CalculateInDegreeKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: CalculateOutDegree
    //
    /// <summary>
    /// Gets or sets a flag specifying whether vertex out-degrees should be
	/// calculated.
    /// </summary>
    ///
    /// <value>
	/// true to calculate vertex out-degrees.  The default is true.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("true") ]

    public Boolean
	CalculateOutDegree
    {
        get
        {
            AssertValid();

			return ( (Boolean)this[CalculateOutDegreeKey] );
        }

        set
        {
			this[CalculateOutDegreeKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: CalculateDegree
    //
    /// <summary>
    /// Gets or sets a flag specifying whether vertex degrees should be
	/// calculated.
    /// </summary>
    ///
    /// <value>
	/// true to calculate vertex degrees.  The default is true.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("true") ]

    public Boolean
	CalculateDegree
    {
        get
        {
            AssertValid();

			return ( (Boolean)this[CalculateDegreeKey] );
        }

        set
        {
			this[CalculateDegreeKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: CalculateClusteringCoefficient
    //
    /// <summary>
    /// Gets or sets a flag specifying whether clustering coefficients should
	/// be calculated.
    /// </summary>
    ///
    /// <value>
	/// true to calculate clustering coefficients.  The default is true.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("true") ]

    public Boolean
	CalculateClusteringCoefficient
    {
        get
        {
            AssertValid();

			return ( (Boolean)this[CalculateClusteringCoefficientKey] );
        }

        set
        {
			this[CalculateClusteringCoefficientKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: CalculateBetweennessCentrality
    //
    /// <summary>
    /// Gets or sets a flag specifying whether betweenness centralities should
	/// be calculated.
    /// </summary>
    ///
    /// <value>
	/// true to calculate betweenness centralities.  The default is false.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("false") ]

    public Boolean
	CalculateBetweennessCentrality
    {
        get
        {
            AssertValid();

			return ( (Boolean)this[CalculateBetweennessCentralityKey] );
        }

        set
        {
			this[CalculateBetweennessCentralityKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: CalculateOverallMetrics
    //
    /// <summary>
    /// Gets or sets a flag specifying whether overall graph metrics should be
	/// calculated.
    /// </summary>
    ///
    /// <value>
	/// true to calculate overall graph metrics.  The default is true.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("true") ]

    public Boolean
	CalculateOverallMetrics
    {
        get
        {
            AssertValid();

			return ( (Boolean)this[CalculateOverallMetricsKey] );
        }

        set
        {
			this[CalculateOverallMetricsKey] = value;

            AssertValid();
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

    public void
    AssertValid()
    {
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	/// Name of the settings key for the CalculateInDegree property.

	protected const String CalculateInDegreeKey =
		"CalculateInDegree";

	/// Name of the settings key for the CalculateOutDegree property.

	protected const String CalculateOutDegreeKey =
		"CalculateOutDegree";

	/// Name of the settings key for the CalculateDegree property.

	protected const String CalculateDegreeKey =
		"CalculateDegree";

	/// Name of the settings key for the CalculateClusteringCoefficient
	/// property.

	protected const String CalculateClusteringCoefficientKey =
		"CalculateClusteringCoefficient";

	/// Name of the settings key for the CalculateBetweennessCentrality
	/// property.

	protected const String CalculateBetweennessCentralityKey =
		"CalculateBetweennessCentrality";

	/// Name of the settings key for the CalculateOverallMetrics property.

	protected const String CalculateOverallMetricsKey =
		"CalculateOverallMetrics";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
