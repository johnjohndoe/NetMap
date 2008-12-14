
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: CommunityPair
//
/// <summary>
/// Represents a pair of communities used by <see cref="ClusterCalculator" />.
/// </summary>
//*****************************************************************************

public class CommunityPair : Object
{
    //*************************************************************************
    //  Constructor: CommunityPair()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="CommunityPair" /> class.
    /// </summary>
    //*************************************************************************

    public CommunityPair()
    {
		m_oCommunity1 = null;
		m_oCommunity2 = null;
		m_fDeltaQ = Community.DeltaQNotSet;

		AssertValid();
    }

    //*************************************************************************
    //  Property: Community1
    //
    /// <summary>
    /// Gets or sets the first community in the pair.
    /// </summary>
    ///
    /// <value>
    /// The first community in the pair, as a <see cref="Community" />.  The
	/// default is null.
    /// </value>
    //*************************************************************************

    public Community
    Community1
    {
        get
        {
            AssertValid();

            return (m_oCommunity1);
        }

        set
        {
			m_oCommunity1 = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Community2
    //
    /// <summary>
    /// Gets or sets the second community in the pair.
    /// </summary>
    ///
    /// <value>
    /// The second community in the pair, as a <see cref="Community" />.  The
	/// default is null.
    /// </value>
    //*************************************************************************

    public Community
    Community2
    {
        get
        {
            AssertValid();

            return (m_oCommunity2);
        }

        set
        {
			m_oCommunity2 = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: DeltaQ
    //
    /// <summary>
    /// Gets or sets the delta Q for this community pair.
    /// </summary>
    ///
    /// <value>
    /// The delta Q for this community pair, as a Single.  The default is <see
	/// cref="Community.DeltaQNotSet" />.
    /// </value>
    //*************************************************************************

    public Single
    DeltaQ
    {
        get
        {
            AssertValid();

            return (m_fDeltaQ);
        }

		set
		{
			m_fDeltaQ = value;

			AssertValid();
		}
    }

	//*************************************************************************
	//	Method: ToString()
	//
	/// <summary>
	/// Formats the value of the current instance using the default format. 
	/// </summary>
	///
	/// <returns>
	/// The formatted string.
	/// </returns>
	//*************************************************************************

	public override String
	ToString()
	{
		AssertValid();

		if (m_oCommunity1 == null || m_oCommunity2 == null)
		{
			return ( base.ToString() );
		}

		return ( String.Format(

			"{0}, {1}, DeltaQ = {2}"
			,
			m_oCommunity1.ToString(),
			m_oCommunity2.ToString(),

			(m_fDeltaQ == Community.DeltaQNotSet) ? "DeltaQNotSet" :
				m_fDeltaQ.ToString("N3")
			) );
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
		// m_oCommunity1
		// m_oCommunity2
		// m_fDeltaQ
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// The first community in the pair.

	protected Community m_oCommunity1;

	/// The second community in the pair.

	protected Community m_oCommunity2;

	/// Maximum delta Q value among all community pairs within
	/// m_oCommunityPairs.

	protected Single m_fDeltaQ;
}

}
