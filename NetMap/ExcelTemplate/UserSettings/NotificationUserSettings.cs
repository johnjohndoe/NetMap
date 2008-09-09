
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Diagnostics;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: NotificationUserSettings
//
/// <summary>
/// Stores the user's settings for notifications that can be disabled by the
/// user.
/// </summary>
//*****************************************************************************

[ SettingsGroupNameAttribute("NotificationUserSettings") ]

public class NotificationUserSettings : ApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: NotificationUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the NotificationUserSettings class.
    /// </summary>
    //*************************************************************************

    public NotificationUserSettings()
    {
		// (Do nothing.)

		AssertValid();
    }

    //*************************************************************************
    //  Property: GraphHasDuplicateEdge
    //
    /// <summary>
    /// Gets or sets a flag specifying whether the user should be warned that
	/// the graph has a duplicate edge before graph metrics are calculated.
    /// </summary>
    ///
    /// <value>
	/// true to warn the user.  The default is true.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("true") ]

    public Boolean
	GraphHasDuplicateEdge
    {
        get
        {
            AssertValid();

			return ( (Boolean)this[GraphHasDuplicateEdgeKey] );
        }

        set
        {
			this[GraphHasDuplicateEdgeKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: CreateClusters
    //
    /// <summary>
    /// Gets or sets a flag specifying whether the user should notified about
	/// how clusters are created.
    /// </summary>
    ///
    /// <value>
	/// true to notify the user.  The default is true.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("true") ]

    public Boolean
	CreateClusters
    {
        get
        {
            AssertValid();

			return ( (Boolean)this[CreateClustersKey] );
        }

        set
        {
			this[CreateClustersKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: EnableAllNotifications()
    //
    /// <summary>
	/// Enables all user notifications maintained by this class.
    /// </summary>
    //*************************************************************************

	public void
    EnableAllNotifications()
    {
		this.GraphHasDuplicateEdge = true;
		this.CreateClusters = true;
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

	/// Name of the settings key for the GraphHasDuplicateEdge property.

	protected const String GraphHasDuplicateEdgeKey =
		"GraphHasDuplicateEdge";

	/// Name of the settings key for the CreateClusters property.

	protected const String CreateClustersKey =
		"CreateClusters";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
