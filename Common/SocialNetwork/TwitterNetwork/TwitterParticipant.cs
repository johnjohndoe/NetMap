
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.SocialNetworkLib
{
//*****************************************************************************
//  Class: TwitterParticipant
//
/// <summary>
/// Represents a participant in a Twitter social network.
/// </summary>
//*****************************************************************************

public class TwitterParticipant : Object
{
    //*************************************************************************
    //  Constructor: TwitterParticipant()
    //
    /// <overloads>
    /// Initializes a new instance of the <see cref="TwitterParticipant" />
    /// class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see cref="TwitterParticipant" />
    /// class with specified values.
    /// </summary>
    ///
    /// <param name="screenName">
    /// The participant's Twitter screen name, as a String.  Can't be null or
    /// empty.
    /// </param>
    ///
    /// <param name="twitterID">
    /// The participant's Twitter ID, or Int32.MinValue if the ID isn't
    /// available.
    /// </param>
    ///
    /// <param name="followerCount">
    /// The number of followers of the participant, or Int32.MinValue if the
    /// number isn't available.
    /// </param>
    ///
    /// <param name="latestStatus">
    /// The participant's latest status, or null or empty if the latest status
    /// isn't available.
    /// </param>
    ///
    /// <param name="latestStatusTime">
    /// The time of the participant's latest status, or DateTime.MinValue if
    /// the time isn't available.
    /// </param>
    //*************************************************************************

    public TwitterParticipant
    (
        String screenName,
        Int32 twitterID,
        Int32 followerCount,
        String latestStatus,
        DateTime latestStatusTime
    )
    {
        m_sScreenName = screenName;
        m_iTwitterID = twitterID;
        m_iFollowerCount = followerCount;
        m_sLatestStatus = latestStatus;
        m_oLatestStatusTime = latestStatusTime;

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: TwitterParticipant()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="TwitterParticipant" />
    /// class with a screen name.
    /// </summary>
    ///
    /// <param name="screenName">
    /// The participant's Twitter screen name, as a String.  Can't be null or
    /// empty.
    /// </param>
    //*************************************************************************

    public TwitterParticipant
    (
        String screenName
    )
    :
    this(screenName, Int32.MinValue, 0, null, DateTime.MinValue)
    {
        // (Do nothing else.)

        AssertValid();
    }


    //*************************************************************************
    //  Constructor: TwitterParticipant()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="TwitterParticipant" />
    /// class with default values.
    /// </summary>
    ///
    /// <remarks>
    /// Do not use this constructor.  It is for XML serialization only.
    /// </remarks>
    //*************************************************************************

    public TwitterParticipant()
    :
    this("Participant")
    {
        // (Do nothing else.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: ScreenName
    //
    /// <summary>
    /// Gets or sets the participant's Twitter screen name.
    /// </summary>
    ///
    /// <value>
    /// The participant's Twitter screen name, as a String.  Can't be null or
    /// empty.
    /// </value>
    //*************************************************************************

    public String
    ScreenName
    {
        get
        {
            AssertValid();

            return (m_sScreenName);
        }

        set
        {
            m_sScreenName = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: TwitterID
    //
    /// <summary>
    /// Gets or sets the participant's Twitter ID.
    /// </summary>
    ///
    /// <value>
    /// The participant's Twitter ID, as an Int32, or Int32.MinValue if the ID
    /// isn't available.
    /// </value>
    //*************************************************************************

    public Int32
    TwitterID
    {
        get
        {
            AssertValid();

            return (m_iTwitterID);
        }

        set
        {
            m_iTwitterID = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: FollowerCount
    //
    /// <summary>
    /// Gets or sets the number of followers of the participant.
    /// </summary>
    ///
    /// <value>
    /// The number of followers of the participant, as an Int32, or
    /// Int32.MinValue if the number isn't available.
    /// </value>
    ///
    /// <remarks>
    /// In Twitter, a follower of person A is a person who receives Twitter
    /// updates from person A.
    /// </remarks>
    //*************************************************************************

    public Int32
    FollowerCount
    {
        get
        {
            AssertValid();

            return (m_iFollowerCount);
        }

        set
        {
            m_iFollowerCount = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: LatestStatus
    //
    /// <summary>
    /// Gets or sets the participant's latest status.
    /// </summary>
    ///
    /// <value>
    /// The participant's latest status, as a String, or null or empty if the
    /// latest status isn't available.
    /// </value>
    ///
    /// <remarks>
    /// In Twitter, a participant's status is his latest posting, or "tweet."
    /// </remarks>
    //*************************************************************************

    public String
    LatestStatus
    {
        get
        {
            AssertValid();

            return (m_sLatestStatus);
        }

        set
        {
            m_sLatestStatus = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: LatestStatusTime
    //
    /// <summary>
    /// Gets or sets the time of the participant's latest status.
    /// </summary>
    ///
    /// <value>
    /// The time of the participant's latest status, as a DateTime, or
    /// DateTime.MinValue if the time isn't available.
    /// </value>
    ///
    /// <remarks>
    /// In Twitter, a participant's status is his latest posting, or "tweet."
    /// </remarks>
    //*************************************************************************

    public DateTime
    LatestStatusTime
    {
        get
        {
            AssertValid();

            return (m_oLatestStatusTime);
        }

        set
        {
            m_oLatestStatusTime = value;

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
        Debug.Assert( !String.IsNullOrEmpty(m_sScreenName) );
        // m_iTwitterID

        Debug.Assert(m_iFollowerCount == Int32.MinValue ||
            m_iFollowerCount >= 0);

        // m_sLatestStatus
        // m_oLatestStatusTime
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The participant's Twitter screen name.

    protected String m_sScreenName;

    /// The participant's Twitter ID, or Int32.MinValue if the ID isn't
    /// available.

    protected Int32 m_iTwitterID;

    /// The number of followers of the participant, or Int32.MinValue if the
    /// number isn't available.

    protected Int32 m_iFollowerCount;

    /// The participant's latest status, or null or empty if the latest status
    /// isn't available.

    protected String m_sLatestStatus;

    /// The time of the participant's latest status, or DateTime.MinValue if
    /// the time isn't available.

    protected DateTime m_oLatestStatusTime;
}

}
