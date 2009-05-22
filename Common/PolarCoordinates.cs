
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: SinglePolarCoordinates
//
/// <summary>
/// Represents a point specified in polar coordinates, using a Single for the
/// <see cref="PolarCoordinates{TR, TAngle}.R" /> and <see
/// cref="PolarCoordinates{TR, TAngle}.Angle" /> properties.
/// </summary>
///
/// <remarks>
/// An instance of this class specifies a point in polar coordinates using a
/// distance from the origin <see cref="PolarCoordinates{TR, TAngle}.R" />, and
/// an angle <see cref="PolarCoordinates{TR, TAngle}.Angle" />.  The units and
/// limits are defined by the application, not by this class.
/// </remarks>
//*****************************************************************************

public class SinglePolarCoordinates : PolarCoordinates<Single, Single>
{
    //*************************************************************************
    //  Constructor: SinglePolarCoordinates()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="SinglePolarCoordinates" />
    /// class using specified coordinates.
    /// </summary>
    ///
    /// <param name="r">
    /// The distance of the point from the origin.
    /// </param>
    ///
    /// <param name="angle">
    /// The angle of the point, in a counterclockwise rotation from the
    /// positive x-axis.
    /// </param>
    //*************************************************************************

    public SinglePolarCoordinates
    (
        Single r,
        Single angle
    )
    : base(r, angle)
    {
        // (Do nothing else.)

        AssertValid();
    }
}


//*****************************************************************************
//  Class: PolarCoordinates
//
/// <summary>
/// Represents a point specified in polar coordinates.
/// </summary>
///
/// <typeparam name="TR">
/// Type of the <see cref="R" /> property.
/// </typeparam>
///
/// <typeparam name="TAngle">
/// Type of the <see cref="Angle" /> property.
/// </typeparam>
///
/// <remarks>
/// An instance of this class specifies a point in polar coordinates using a
/// distance from the origin <see cref="R" />, and an angle <see
/// cref="Angle" />.  The coordinate types, units, and limits are defined by
/// the application, not by this class.
/// </remarks>
//*****************************************************************************

public class PolarCoordinates<TR, TAngle> : Object
{
    //*************************************************************************
    //  Constructor: PolarCoordinates()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
    /// cref="PolarCoordinates{TR, TAngle}" /> class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="PolarCoordinates{TR, TAngle}" /> class using default coordinates.
    /// </summary>
    ///
    /// <remarks>
    /// The coordinates are set to the default values for TR and TAngle.
    /// </remarks>
    //*************************************************************************

    public PolarCoordinates()
    : this( default(TR), default(TAngle) )
    {
        // (Do nothing else.)

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: PolarCoordinates()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="PolarCoordinates{TR, TAngle}" /> class using specified
    /// coordinates.
    /// </summary>
    ///
    /// <param name="r">
    /// The distance of the point from the origin.
    /// </param>
    ///
    /// <param name="angle">
    /// The angle of the point, in a counterclockwise rotation from the
    /// positive x-axis.
    /// </param>
    //*************************************************************************

    public PolarCoordinates
    (
        TR r,
        TAngle angle
    )
    {
        m_oR = r;
        m_oAngle = angle;

        AssertValid();
    }

    //*************************************************************************
    //  Property: R
    //
    /// <summary>
    /// Gets or sets the distance of the point from the origin.
    /// </summary>
    ///
    /// <value>
    /// The distance of the point from the origin.  The units and limits are
    /// defined by the application, not by this class.
    /// </value>
    //*************************************************************************

    public TR
    R
    {
        get
        {
            AssertValid();

            return (m_oR);
        }

        set
        {
            m_oR = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Angle
    //
    /// <summary>
    /// Gets or sets the angle of the point.
    /// </summary>
    ///
    /// <value>
    /// The angle of the point, in a counterclockwise rotation from the
    /// positive x-axis.  The units and limits are defined by the application,
    /// not by this class.
    /// </value>
    //*************************************************************************

    public TAngle
    Angle
    {
        get
        {
            AssertValid();

            return (m_oAngle);
        }

        set
        {
            m_oAngle = value;

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
        // m_oR
        // m_oAngle
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The distance of the point from the origin.

    protected TR m_oR;

    /// The angle of the point, in a counterclockwise rotation from the
    /// positive x-axis.

    protected TAngle m_oAngle;
}

}
