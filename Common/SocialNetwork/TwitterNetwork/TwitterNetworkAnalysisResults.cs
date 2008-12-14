
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.SocialNetworkLib
{
//*****************************************************************************
//  Class: TwitterNetworkAnalysisResults
//
/// <summary>
/// Represents the results of a Twitter network analysis.
/// </summary>
///
/// <remarks>
/// An instance of this class is returned by <see
/// cref="TwitterNetworkAnalyzer.AnalyzeTwitterNetwork" /> and <see
/// cref="TwitterNetworkAnalyzer.AnalyzeTwitterNetworkAsync" />.
/// </remarks>
//*****************************************************************************

public class TwitterNetworkAnalysisResults : Object
{
    //*************************************************************************
    //  Constructor: TwitterNetworkAnalysisResults()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
	/// cref="TwitterNetworkAnalysisResults" /> class.
    /// </overloads>
	///
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="TwitterNetworkAnalysisResults" /> class with specified values.
    /// </summary>
	///
    /// <param name="participantPairs">
	/// An array of <see cref="TwitterParticipantPair" /> objects, one for each
	/// participant pair in the network.  Can be empty but not null.
    /// </param>
	///
    /// <param name="participants">
	/// A dictionary of participants, one for each participant in <paramref
	/// name="participantPairs" />.  The key is the participant's Twitter
	/// screen name and the value is the <see cref="TwitterParticipant" />
	/// object for the participant.  Can be empty but not null.
    /// </param>
    //*************************************************************************

    public TwitterNetworkAnalysisResults
	(
		TwitterParticipantPair [] participantPairs,
		Dictionary<String, TwitterParticipant> participants
	)
    {
		m_aoParticipantPairs = participantPairs;
		m_oParticipants = participants;

		AssertValid();
    }

    //*************************************************************************
    //  Constructor: TwitterNetworkAnalysisResults()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="TwitterNetworkAnalysisResults" /> class with default values.
    /// </summary>
	///
	/// <remarks>
	/// Do not use this constructor.  It is for XML serialization only.
	/// </remarks>
    //*************************************************************************

    public TwitterNetworkAnalysisResults()
	:
	this( new TwitterParticipantPair[0],
		new Dictionary<String, TwitterParticipant>() )
    {
		// (Do nothing else.)

		AssertValid();
    }

    //*************************************************************************
    //  Property: ParticipantPairs
    //
    /// <summary>
	/// Gets or sets an array of <see cref="TwitterParticipantPair" /> objects.
    /// </summary>
    ///
    /// <value>
	/// An array of <see cref="TwitterParticipantPair" /> objects, one for each
	/// participant pair in the network.  Can be empty but not null.
    /// </value>
    //*************************************************************************

    public TwitterParticipantPair []
    ParticipantPairs
    {
        get
        {
            AssertValid();

            return (m_aoParticipantPairs);
        }

        set
        {
			m_aoParticipantPairs = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Participants
    //
    /// <summary>
	/// Gets or sets a dictionary of participants.
    /// </summary>
    ///
    /// <value>
	/// A dictionary of participants, one for each participant in <paramref
	/// name="participantPairs" />.  The key is the participant's Twitter
	/// screen name and the value is the <see cref="TwitterParticipant" />
	/// object for the participant.  Can be empty but not null.
    /// </value>
    //*************************************************************************

    public Dictionary<String, TwitterParticipant>
    Participants
    {
        get
        {
            AssertValid();

            return (m_oParticipants);
        }

        set
        {
			m_oParticipants = value;

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
		Debug.Assert(m_aoParticipantPairs != null);
		Debug.Assert(m_oParticipants != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// An array of TwitterParticipantPair objects, one for each participant
	/// pair in the network.  Can be empty but not null.

	protected TwitterParticipantPair [] m_aoParticipantPairs;

	/// A dictionary of participants, one for each participant in
	/// m_aoParticipantPairs.  The key is the participant's Twitter screen name
	/// and the value is the TwitterParticipant object for the participant.
	/// Can be empty but not null.

	protected Dictionary<String, TwitterParticipant> m_oParticipants;
}

}
