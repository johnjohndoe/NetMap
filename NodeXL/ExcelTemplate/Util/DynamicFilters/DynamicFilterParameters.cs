
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: DynamicFilterParameters
//
/// <summary>
/// Base class for storing dynamic filters parameters.
/// </summary>
///
/// <remarks>
/// A dynamic filter is a control that can be adjusted to selectively show and
/// hide edges and vertices in the graph in real time.  There is one control
/// for each filterable column in the workbook, and one instance of this class
/// for each such control.
///
/// <para>
/// Use the <see cref="DynamicFilterUtil" /> class to get collections of filter
/// parameters.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class DynamicFilterParameters : Object
{
    //*************************************************************************
    //  Constructor: DynamicFilterParameters()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="DynamicFilterParameters" /> class.
    /// </summary>
	///
    /// <param name="columnName">
    /// Name of the column that can be filtered on.
    /// </param>
    //*************************************************************************

    public DynamicFilterParameters
	(
		String columnName
	)
    {
		m_sColumnName = columnName;

		// AssertValid();
    }

    //*************************************************************************
    //  Property: ColumnName
    //
    /// <summary>
    /// Gets the name of the column that can be filtered on.
    /// </summary>
    ///
    /// <value>
    /// The name of the column that can be filtered on.
    /// </value>
    //*************************************************************************

    public String
    ColumnName
    {
        get
        {
            AssertValid();

            return (m_sColumnName);
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

    public virtual void
    AssertValid()
    {
		Debug.Assert( !String.IsNullOrEmpty(m_sColumnName) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Name of the column that can be filtered on.

	protected String m_sColumnName;
}

}
