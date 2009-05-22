
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.SocialNetworkLib
{
//*****************************************************************************
//  Class: EmailParticipantPair
//
/// <summary>
/// Represents a pair of participants in an email social network.
/// </summary>
//*****************************************************************************

public class EmailParticipantPair : ParticipantPair
{
    //*************************************************************************
    //  Constructor: EmailParticipantPair()
    //
    /// <overloads>
    /// Initializes a new instance of the <see cref="EmailParticipantPair" />
    /// class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see cref="EmailParticipantPair" />
    /// class with specified values.
    /// </summary>
    ///
    /// <param name="participant1">
    /// The first participant, as an email address.  Can't be null or empty.
    /// </param>
    ///
    /// <param name="participant2">
    /// The second participant, as an email address.  Can't be null or empty.
    /// </param>
    ///
    /// <param name="edgeWeight">
    /// The strength of the tie between the participants.  Must be
    /// non-negative.
    /// </param>
    //*************************************************************************

    public EmailParticipantPair
    (
        String participant1,
        String participant2,
        Int32 edgeWeight
    )
    : base(participant1, participant2)
    {
        m_iEdgeWeight = edgeWeight;

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: EmailParticipantPair()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EmailParticipantPair" />
    /// class with default values.
    /// </summary>
    ///
    /// <remarks>
    /// Do not use this constructor.  It is for XML serialization only.
    /// </remarks>
    //*************************************************************************

    public EmailParticipantPair()
    :
    this("Participant1", "Participant2", 0)
    {
        // (Do nothing else.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: EdgeWeight
    //
    /// <summary>
    /// Gets or sets the strength of the tie between the participants.
    /// </summary>
    ///
    /// <value>
    /// The strength of the tie between the participants, as an Int32.  Must be
    /// non-negative.
    /// </value>
    //*************************************************************************

    public Int32
    EdgeWeight
    {
        get
        {
            AssertValid();

            return (m_iEdgeWeight);
        }

        set
        {
            m_iEdgeWeight = value;

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

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
        base.AssertValid();

        Debug.Assert(m_iEdgeWeight >= 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The strength of the tie between the participants.

    protected Int32 m_iEdgeWeight;
}

}
