
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: CollapseOrExpandGroupsEventArgs
//
/// <summary>
/// Provides event information for the <see
/// cref="ThisWorkbook.CollapseOrExpandGroups" /> event.
/// </summary>
//*****************************************************************************

public class CollapseOrExpandGroupsEventArgs : EventArgs
{
    //*************************************************************************
    //  Constructor: CollapseOrExpandGroupsEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="CollapseOrExpandGroupsEventArgs" /> class.
    /// </summary>
    ///
    /// <param name="collapse">
    /// true to collapse the groups, false to expand them.
    /// </param>
    ///
    /// <param name="groupNames">
    /// Collection of group names, one for each group that needs to be
    /// collapsed or expanded.
    /// </param>
    //*************************************************************************

    public CollapseOrExpandGroupsEventArgs
    (
        Boolean collapse,
        ICollection<String> groupNames
    )
    {
        m_bCollapse = collapse;
        m_oGroupNames = groupNames;

        AssertValid();
    }

    //*************************************************************************
    //  Property: GroupNames
    //
    /// <summary>
    /// Gets the names of the groups to collapse or expand.
    /// </summary>
    ///
    /// <value>
    /// Collection of group names, one for each group that needs to be
    /// collapsed or expanded.
    /// </value>
    //*************************************************************************

    public ICollection<String>
    GroupNames
    {
        get
        {
            AssertValid();

            return (m_oGroupNames);
        }
    }

    //*************************************************************************
    //  Property: Collapse
    //
    /// <summary>
    /// Gets a flag indicating whether the groups should be collapsed.
    /// </summary>
    ///
    /// <value>
    /// true to collapse the groups, false to expand them.
    /// </value>
    //*************************************************************************

    public Boolean
    Collapse
    {
        get
        {
            AssertValid();

            return (m_bCollapse);
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
        // m_bCollapse
        Debug.Assert(m_oGroupNames != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// true to collapse the groups, false to expand them.

    protected Boolean m_bCollapse;

    /// Collection of group names, one for each group that needs to be
    /// collapsed or expanded.

    protected ICollection<String> m_oGroupNames;
}


//*****************************************************************************
//  Delegate: CollapseOrExpandGroupsEventHandler
//
/// <summary>
/// Represents a method that will handle the <see
/// cref="ThisWorkbook.CollapseOrExpandGroups" /> event.
/// </summary>
///
/// <param name="sender">
/// The source of the event.
/// </param>
///
/// <param name="e">
/// A <see cref="CollapseOrExpandGroupsEventArgs" /> object that contains the
/// event data.
/// </param>
//*****************************************************************************

public delegate void
CollapseOrExpandGroupsEventHandler
(
    Object sender,
    CollapseOrExpandGroupsEventArgs e
);

}
