
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: DeltaQMaxHeap
//
/// <summary>
/// Represents a max heap used by <see cref="ClusterCalculator" /> to keep
/// track of the maximum delta Q value in each community.
///
/// <para>
/// There is an element in the max heap for each community.  The key is the
/// Community and the value is the Community's maximum delta Q.
/// </para>
///
/// </summary>
//*****************************************************************************

public class DeltaQMaxHeap : BinaryHeap<Community, Single>
{
    //*************************************************************************
    //  Constructor: DeltaQMaxHeap()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="DeltaQMaxHeap" /> class
    /// with a specified initial capacity.
    /// </summary>
    ///
    /// <param name="initialCapacity">
    /// Initial capacity.  Must be non-negative.
    /// </param>
    //*************************************************************************

    public DeltaQMaxHeap
    (
        Int32 initialCapacity
    )
    : base( initialCapacity, new DeltaQComparer() )
    {
        // (Do nothing else.)

        AssertValid();
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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}


//*****************************************************************************
//  Class: DeltaQComparer
//
/// <summary>
/// Compares two delta Q values.
/// </summary>
//*****************************************************************************

public class DeltaQComparer : IComparer<Single>
{
    //*************************************************************************
    //  Constructor: DeltaQComparer()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="DeltaQComparer" /> class.
    /// </summary>
    //*************************************************************************

    public DeltaQComparer()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: Compare()
    //
    /// <summary>
    /// Compares two objects and returns a value indicating whether one is less
    /// than, equal to, or greater than the other.
    /// </summary>
    ///
    /// <param name="x">
    /// The first object to compare.
    /// </param>
    ///
    /// <param name="y">
    /// The second object to compare.
    /// </param>
    ///
    /// <returns>
    /// See the interface definition.
    /// </returns>
    //*************************************************************************

    public Int32
    Compare
    (
        Single x,
        Single y
    )
    {
        AssertValid();

        return ( x.CompareTo(y) );
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
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
