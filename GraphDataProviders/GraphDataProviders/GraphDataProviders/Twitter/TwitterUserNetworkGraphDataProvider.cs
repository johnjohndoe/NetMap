

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterUserNetworkGraphDataProvider
//
/// <summary>
/// Gets the network of people followed by a Twitter user or people whom a
/// Twitter user follows.
/// </summary>
///
/// <remarks>
/// Call <see cref="GraphDataProviderBase.TryGetGraphData" /> to get GraphML
/// that describes the network of people followed by a Twitter user, or the
/// network of people following the user.
/// </remarks>
//*****************************************************************************

public class TwitterUserNetworkGraphDataProvider : GraphDataProviderBase
{
   //*************************************************************************
    //  Constructor: TwitterUserNetworkGraphDataProvider()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterUserNetworkGraphDataProvider" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterUserNetworkGraphDataProvider()
    :
    base(GraphDataProviderName,
        "get the network of people followed by a Twitter user or the network"
        + " of people following the user")
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: CreateDialog()
    //
    /// <summary>
    /// Creates a dialog for getting graph data.
    /// </summary>
    ///
    /// <returns>
    /// A dialog derived from GraphDataProviderDialogBase.
    /// </returns>
    //*************************************************************************

    protected override GraphDataProviderDialogBase
    CreateDialog()
    {
        AssertValid();

        return ( new TwitterGetUserNetworkDialog() );
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
    //  Public constants
    //*************************************************************************

    /// Value of the Name property.

    public const String GraphDataProviderName =
        "Twitter User's Network";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
