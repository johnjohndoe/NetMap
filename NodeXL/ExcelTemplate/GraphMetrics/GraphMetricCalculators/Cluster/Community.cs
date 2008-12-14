
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.NodeXL.Core;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: Community
//
/// <summary>
/// Represents a community used by <see cref="ClusterCalculator" />.
/// </summary>
//*****************************************************************************

// This class inherits from NodeXLBase to get the extended ToString()
// functionality.

public class Community : NodeXLBase
{
    //*************************************************************************
    //  Constructor: Community()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="Community" /> class.
    /// </summary>
    //*************************************************************************

    public Community()
    {
        m_iID = 0;
		m_oCommunityPairs = new SortedList<Int32, CommunityPair>();
		m_oCommunityPairWithMaximumDeltaQ = null;
		m_oVertices = new LinkedList<IVertex>();
		m_iDegree = 0;

		AssertValid();
    }

    //*************************************************************************
    //  Property: ID
    //
    /// <summary>
    /// Gets or sets a unique community ID.
    /// </summary>
    ///
    /// <value>
    /// A unique community ID, as an Int32.  The default is 0.
    /// </value>
    //*************************************************************************

    public Int32
    ID
    {
        get
        {
            AssertValid();

            return (m_iID);
        }

		set
		{
			m_iID = value;

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: CommunityPairs
    //
    /// <summary>
    /// Gets a sorted list of <see cref="CommunityPair" /> objects that this
	/// community contains.
    /// </summary>
    ///
    /// <value>
	/// A SortedList of <see cref="CommunityPair" /> objects.  The sort key is
	/// the ID of CommunityPair.<see cref="CommunityPair.Community2" /> and the
	/// value is the <see cref="CommunityPair" />.  The default is an empty
	/// SortedList.
    /// </value>
    //*************************************************************************

    public SortedList<Int32, CommunityPair>
    CommunityPairs
    {
        get
        {
            AssertValid();

            return (m_oCommunityPairs);
        }
    }

    //*************************************************************************
    //  Property: CommunityPairWithMaximumDeltaQ
    //
    /// <summary>
    /// Gets or sets the <see cref="CommunityPair" /> object that has the
	/// maximum delta Q value.
    /// </summary>
    ///
    /// <value>
    /// The <see cref="CommunityPair" /> object that has the maximum delta Q
	/// value in the <see cref="CommunityPairs" /> list.  The default is null.
    /// </value>
    //*************************************************************************

    public CommunityPair
    CommunityPairWithMaximumDeltaQ
    {
        get
        {
            AssertValid();

            return (m_oCommunityPairWithMaximumDeltaQ);
        }

		set
		{
            m_oCommunityPairWithMaximumDeltaQ = value;

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: MaximumDeltaQ
    //
    /// <summary>
    /// Gets the maximum delta Q value among all community pairs within <see
	/// cref="CommunityPairs" />.
    /// </summary>
    ///
    /// <value>
    /// The maximum delta Q value, as a Single.  The default is <see
	/// cref="DeltaQNotSet" />.
    /// </value>
    //*************************************************************************

    public Single
    MaximumDeltaQ
    {
        get
        {
            AssertValid();

            if (m_oCommunityPairWithMaximumDeltaQ == null)
			{
				return (DeltaQNotSet);
			}

			return (m_oCommunityPairWithMaximumDeltaQ.DeltaQ);
        }
    }

    //*************************************************************************
    //  Property: Vertices
    //
    /// <summary>
    /// Gets a linked list of the vertices that this community contains.
    /// </summary>
    ///
    /// <value>
	/// A LinkedList of <see cref="IVertex" /> objects.  The default is an
	/// empty LinkedList.
    /// </value>
    //*************************************************************************

    public LinkedList<IVertex>
    Vertices
    {
        get
        {
            AssertValid();

            return (m_oVertices);
        }
    }

    //*************************************************************************
    //  Property: Degree
    //
    /// <summary>
    /// Gets or sets the sum of the degrees for the vertices that this
	/// community contains.
    /// </summary>
    ///
    /// <value>
    /// The sum of the degrees for the community's vertices, as an Int32.  The
	/// default is zero.
    /// </value>
    //*************************************************************************

    public Int32
    Degree
    {
        get
        {
            AssertValid();

            return (m_iDegree);
        }

		set
		{
			m_iDegree = value;

			AssertValid();
		}
    }

    //*************************************************************************
    //  Method: InitializeDeltaQs()
    //
    /// <summary>
    /// Initializes the delta Q values for this community.
    /// </summary>
    ///
    /// <param name="communities">
    /// List of all communities.
    /// </param>
    ///
    /// <param name="edgesInGraph">
    /// Number of edges in the graph.
    /// </param>
	///
	/// <remarks>
	/// It's assumed that the community is in its initial state of having one
	/// vertex only.
	/// </remarks>
    //*************************************************************************

	public void
	InitializeDeltaQs
	(
		LinkedList<Community> communities,
		Int32 edgesInGraph
	)
	{
		Debug.Assert(communities != null);
		Debug.Assert(edgesInGraph > 0);
		AssertValid();

		Single fMaximumDeltaQ = Single.MinValue;
		m_oCommunityPairWithMaximumDeltaQ = null;

		// These calculations are based on equation 8 of "Finding Community
		// Structure in Very Large Networks," by Clauset, Newman and Moore.

		Debug.Assert(m_oVertices.Count == 1);

		Int32 iKi = m_iDegree;
		Single f2M = 2F * edgesInGraph;
		Single fOneOver2M = 1F / f2M;
		Single fOneOver2MSquared = 1F / (f2M * f2M);

		foreach (CommunityPair oCommunityPair in m_oCommunityPairs.Values)
		{
			Community oCommunity2 = oCommunityPair.Community2;

			Debug.Assert(oCommunity2.Vertices.Count == 1);

			Int32 iKj = oCommunity2.Degree;

			Single fDeltaQ =
				fOneOver2M - ( (Single)(iKi * iKj) * fOneOver2MSquared );

			oCommunityPair.DeltaQ = fDeltaQ;

			if (fDeltaQ > fMaximumDeltaQ)
			{
				fMaximumDeltaQ = fDeltaQ;
				m_oCommunityPairWithMaximumDeltaQ = oCommunityPair;
			}
		}
	}

    //*************************************************************************
    //  Method: OnMergedCommunities()
    //
    /// <summary>
	/// Gets called when two communities merge and one or both of them were
	/// connected to this community.
    /// </summary>
    ///
    /// <param name="mergedCommunity1">
	/// The first community that was merged.
    /// </param>
	///
    /// <param name="mergedCommunity2">
	/// The second community that was merged.
    /// </param>
	///
    /// <param name="newMergedCommunity">
	/// The new community that the communities were merged into.
    /// </param>
	///
    /// <param name="newCommunityPairDeltaQ">
	/// The new delta Q value for the community pair that connects this
	/// community to the merged community.
    /// </param>
	///
    /// <param name="deltaQMaxHeap">
	/// Max heap, used to keep track of the maximum delta Q value in each
	/// community.  There is an element in the max heap for each community.
	/// The key is the Community and the value is the Community's maximum
	/// delta Q.
    /// </param>
    //*************************************************************************

	public void
	OnMergedCommunities
	(
		Community mergedCommunity1,
		Community mergedCommunity2,
		Community newMergedCommunity,
		Single newCommunityPairDeltaQ,
		DeltaQMaxHeap deltaQMaxHeap
	)
	{
		Debug.Assert(mergedCommunity1 != null);
		Debug.Assert(mergedCommunity2 != null);
		Debug.Assert(newMergedCommunity != null);
		Debug.Assert(deltaQMaxHeap != null);
		AssertValid();

		// If only one of the two merged communities was connected to this
		// community, fPreviousCommunityPairDeltaQ is the delta Q for this
		// community's community pair for the merged community.  If both were
		// connected, fPreviousCommunityPairDeltaQ is the larger of the two
		// delta Q values.

		Single fPreviousCommunityPairDeltaQ = Single.MinValue;

		Int32 iMergedCommunity1ID = mergedCommunity1.ID;
		Int32 iMergedCommunity2ID = mergedCommunity2.ID;

		Debug.Assert(iMergedCommunity1ID != iMergedCommunity2ID);

		Int32 iSmallerMergedCommunityID =
			Math.Min(iMergedCommunity1ID, iMergedCommunity2ID);

		Int32 iLargerMergedCommunityID =
			Math.Max(iMergedCommunity1ID, iMergedCommunity2ID);

		// Delete the community pair or pairs that connect to one of the merged
		// communities.
		//
		// Go backwards through the community pairs so that they can be deleted
		// while looping.  (Don't use foreach, because you can't delete while
		// enumerating.)

		for (Int32 i = m_oCommunityPairs.Count - 1; i >= 0; i--)
		{
			Int32 iOtherCommunityID = m_oCommunityPairs.Keys[i];

			if (iOtherCommunityID > iLargerMergedCommunityID)
			{
				// We haven't yet reached the range of community pairs that
				// might connect to either merged community.

				continue;
			}

			if (iOtherCommunityID < iSmallerMergedCommunityID)
			{
				// We're beyond the range of community pairs that might connect
				// to either merged community.

				break;
			}

			CommunityPair oCommunityPair = m_oCommunityPairs.Values[i];

			if (iOtherCommunityID == iLargerMergedCommunityID)
			{
				// This community pair connects to the merged community with
				// the larger ID.

				fPreviousCommunityPairDeltaQ = oCommunityPair.DeltaQ;

				m_oCommunityPairs.RemoveAt(i);
			}
			else if (iOtherCommunityID == iSmallerMergedCommunityID)
			{
				// This community pair connects to the merged community with
				// the smaller ID.

				fPreviousCommunityPairDeltaQ = Math.Max(
					fPreviousCommunityPairDeltaQ, oCommunityPair.DeltaQ);

				m_oCommunityPairs.RemoveAt(i);

				// There is no reason to continue looking at community pairs.

				break;
			}
			else
			{
				// This community pair does not connect to either merged
				// community.

				continue;
			}
		}

		Debug.Assert(fPreviousCommunityPairDeltaQ != Single.MinValue);

		// Add a new community pair that connects to the new merged community.

		CommunityPair oNewCommunityPair = new CommunityPair();
		oNewCommunityPair.Community1 = this;
		oNewCommunityPair.Community2 = newMergedCommunity;
		oNewCommunityPair.DeltaQ = newCommunityPairDeltaQ;
        m_oCommunityPairs.Add(newMergedCommunity.ID, oNewCommunityPair);

		// Update m_oCommunityPairWithMaximumDeltaQ if necessary.  These rules
		// come from section 4.1 of "Finding Community Structure in Mega-scale
		// Social Networks," by Ken Wakita and Toshiyuki Tsurumi.

		Single fOldMaximumDeltaQ = this.MaximumDeltaQ;

		if (fPreviousCommunityPairDeltaQ < fOldMaximumDeltaQ)
		{
			// The deleted community pair (or pairs) was not the one with the
			// maximum delta Q.

			if (newCommunityPairDeltaQ <= fPreviousCommunityPairDeltaQ)
			{
				// The delta Q value for the new community pair is less than or
				// equal to the delta Q value of the deleted community pair (or
				// pairs).  Do nothing.
			}
			else
			{
				// The delta Q value for the new community pair is greater than
				// the delta Q value of the deleted community pair (or pairs).

				if (newCommunityPairDeltaQ > fOldMaximumDeltaQ)
				{
					// The new community pair is the one with the maximum
					// delta Q.

					m_oCommunityPairWithMaximumDeltaQ = oNewCommunityPair;
				}
			}
		}
		else
		{
			// The deleted community pair (or pairs) was the one with the
			// maximum delta Q.

			if (newCommunityPairDeltaQ >= fPreviousCommunityPairDeltaQ)
			{
				// The new community pair is the one with the maximum
				// delta Q.

				m_oCommunityPairWithMaximumDeltaQ = oNewCommunityPair;
			}
			else
			{
				// Worst case: All community pairs must be scanned.

				Single fNewMaximumDeltaQ = Single.MinValue;

				foreach (CommunityPair oCommunityPair in
					m_oCommunityPairs.Values)
				{
					if (oCommunityPair.DeltaQ > fNewMaximumDeltaQ)
					{
						m_oCommunityPairWithMaximumDeltaQ = oCommunityPair;
						fNewMaximumDeltaQ = oCommunityPair.DeltaQ;
					}
				}
			}
		}

		if (fOldMaximumDeltaQ != this.MaximumDeltaQ)
		{
			// Update the max heap.

			deltaQMaxHeap.UpdateValue(this, this.MaximumDeltaQ);
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

		return ( "ID = " + m_iID.ToString(ExcelTemplateForm.Int32Format) );
	}

	//*************************************************************************
	//	Method: GetHashCode()
	//
	/// <summary>
	/// Serves as a hash function for a particular type. 
	/// </summary>
	///
	/// <returns>
	/// A hash code for the current Object.
	/// </returns>
	//*************************************************************************

	public override Int32
	GetHashCode()
	{
		AssertValid();

		return (m_iID);
	}

	//*************************************************************************
	//	Method: AppendPropertiesToString()
	//
	/// <summary>
	/// Appends the derived class's public property values to a String.
	/// </summary>
	///
	/// <param name="oStringBuilder">
	/// Object to append to.
	/// </param>
	///
    /// <param name="iIndentationLevel">
	/// Current indentation level.  Level 0 is "no indentation."
    /// </param>
    ///
	/// <param name="sFormat">
	/// The format to use, either "G", "P", or "D".  See <see
	/// cref="NodeXLBase.ToString()" /> for details.
	/// </param>
	///
	/// <remarks>
	/// This method calls <see cref="ToStringUtil.AppendPropertyToString(
	/// StringBuilder, Int32, String, Object, Boolean)" /> for each of the
	/// derived class's public properties.  It is used in the implementation of
	/// <see cref="NodeXLBase.ToString()" />.
	/// </remarks>
	//*************************************************************************

	protected override void
	AppendPropertiesToString
	(
		StringBuilder oStringBuilder,
		Int32 iIndentationLevel,
		String sFormat
	)
	{
		AssertValid();
		Debug.Assert(oStringBuilder != null);
		Debug.Assert(iIndentationLevel >= 0);
		Debug.Assert( !String.IsNullOrEmpty(sFormat) );
		Debug.Assert(sFormat == "G" || sFormat == "P" || sFormat == "D");

		const String SingleFormat = "N3";

		oStringBuilder.AppendLine();

        base.AppendPropertiesToString(
			oStringBuilder, iIndentationLevel, sFormat);

        if (sFormat == "G")
        {
            return;
        }

		ToStringUtil.AppendPropertyToString(oStringBuilder, iIndentationLevel,
			"ID", m_iID);

		ToStringUtil.AppendPropertyToString( oStringBuilder, iIndentationLevel,
			"CommunityPairWithMaximumDeltaQ",

			(m_oCommunityPairWithMaximumDeltaQ == null) ?
				ToStringUtil.NullString
				:
				m_oCommunityPairWithMaximumDeltaQ.Community1.ID.ToString()
					+ ","
					+ m_oCommunityPairWithMaximumDeltaQ.Community2.ID.ToString()
			);

        Int32 iCommunityPairs = m_oCommunityPairs.Count;

        ToStringUtil.AppendIndentationToString(oStringBuilder,
			iIndentationLevel);

        oStringBuilder.Append( iCommunityPairs.ToString(
			ExcelTemplateForm.Int32Format) );

        oStringBuilder.Append(
			StringUtil.MakePlural(" community pair", iCommunityPairs) );

        if (sFormat == "D")
        {
            oStringBuilder.AppendLine();

            foreach (CommunityPair oCommunityPair in m_oCommunityPairs.Values)
            {
				oStringBuilder.AppendLine();

				ToStringUtil.AppendIndentationToString(oStringBuilder,
					iIndentationLevel + 1);

				oStringBuilder.AppendLine("CommunityPair");

				Community oCommunity1 = oCommunityPair.Community1;

				ToStringUtil.AppendPropertyToString( oStringBuilder,
					iIndentationLevel + 1,
					"Community1",
					oCommunity1 == null ? ToStringUtil.NullString :
					oCommunity1.ID.ToString() );

				Community oCommunity2 = oCommunityPair.Community2;

				ToStringUtil.AppendPropertyToString( oStringBuilder,
					iIndentationLevel + 1,
					"Community2",
					oCommunity2 == null ? ToStringUtil.NullString :
					oCommunity2.ID.ToString() );

				Single fDeltaQ = oCommunityPair.DeltaQ;

				ToStringUtil.AppendPropertyToString(oStringBuilder,
					iIndentationLevel + 1,
					"DeltaQ",
					( (fDeltaQ == Community.DeltaQNotSet) ?
						"DeltaQNotSet" : fDeltaQ.ToString(SingleFormat) )
					);
            }

			ToStringUtil.AppendVerticesToString(oStringBuilder,
				iIndentationLevel, sFormat, m_oVertices);
        }
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

		Debug.Assert(m_iID >= 0);
		Debug.Assert(m_oCommunityPairs != null);
		// m_oCommunityPairWithMaximumDeltaQ
		Debug.Assert(m_oVertices != null);
		Debug.Assert(m_iDegree >= 0);
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

	/// <summary>
	/// Value of <see cref="CommunityPair.DeltaQ" /> and <see
	/// cref="MaximumDeltaQ" /> when a delta Q hasn't been set yet.
	/// </summary>

	public static Single DeltaQNotSet = Single.MinValue;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Unique community ID.

	protected Int32 m_iID;

	/// SortedList of CommunityPair objects.  The sort key is the Community.ID
	/// of the CommunityPair's Community2, and the value is the CommunityPair.

	protected SortedList<Int32, CommunityPair> m_oCommunityPairs;

    /// The CommunityPair object that has the maximum delta Q value in the
	/// m_oCommunityPairs list, or null.

    protected CommunityPair m_oCommunityPairWithMaximumDeltaQ;

    /// Linked list of the vertices that this community contains.

    protected LinkedList<IVertex> m_oVertices;

    /// The sum of the degrees for the vertices in m_oVertices.

	protected Int32 m_iDegree;
}

}
