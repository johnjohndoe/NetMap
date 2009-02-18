
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.SocialNetworkLib
{
//*****************************************************************************
//  Class: ParticipantPair
//
/// <summary>
/// Represents a pair of participants in a social network.
/// </summary>
//*****************************************************************************

public class ParticipantPair : Object
{
    //*************************************************************************
    //  Constructor: ParticipantPair()
    //
    /// <overloads>
    /// Initializes a new instance of the <see cref="ParticipantPair" /> class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see cref="ParticipantPair" /> class
    /// with specified values.
    /// </summary>
    ///
    /// <param name="participant1">
    /// The first participant.  Can't be null or empty.
    /// </param>
    ///
    /// <param name="participant2">
    /// The second participant.  Can't be null or empty.
    /// </param>
    //*************************************************************************

    public ParticipantPair
    (
        String participant1,
        String participant2
    )
    {
        m_sParticipant1 = participant1;
        m_sParticipant2 = participant2;

        // AssertValid();
    }

    //*************************************************************************
    //  Constructor: ParticipantPair()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ParticipantPair" /> class
    /// with default values.
    /// </summary>
    ///
    /// <remarks>
    /// Do not use this constructor.  It is for XML serialization only.
    /// </remarks>
    //*************************************************************************

    public ParticipantPair()
    :
    this("Participant1", "Participant2")
    {
        // (Do nothing else.)

        // AssertValid();
    }

    //*************************************************************************
    //  Property: Participant1
    //
    /// <summary>
    /// Gets or sets the first participant.
    /// </summary>
    ///
    /// <value>
    /// The first participant, as a String.  Can't be null or empty.
    /// </value>
    //*************************************************************************

    public String
    Participant1
    {
        get
        {
            AssertValid();

            return (m_sParticipant1);
        }

        set
        {
            m_sParticipant1 = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Participant2
    //
    /// <summary>
    /// Gets or sets the second participant.
    /// </summary>
    ///
    /// <value>
    /// The second participant, as a String.  Can't be null or empty.
    /// </value>
    //*************************************************************************

    public String
    Participant2
    {
        get
        {
            AssertValid();

            return (m_sParticipant2);
        }

        set
        {
            m_sParticipant2 = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: ToString()
    //
    /// <summary>
    /// Returns a String that represents the current object. 
    /// </summary>
    ///
    /// <returns>
    /// A string representation of the object.
    /// </returns>
    //*************************************************************************

    public override String
    ToString()
    {
        AssertValid();

        return ( String.Format(

            "{0}\t{1}"
            ,
            m_sParticipant1,
            m_sParticipant2
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

    public virtual void
    AssertValid()
    {
        Debug.Assert( !String.IsNullOrEmpty(m_sParticipant1) );
        Debug.Assert( !String.IsNullOrEmpty(m_sParticipant2) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The first participant.

    protected String m_sParticipant1;

    /// The second participant.

    protected String m_sParticipant2;
}

}
