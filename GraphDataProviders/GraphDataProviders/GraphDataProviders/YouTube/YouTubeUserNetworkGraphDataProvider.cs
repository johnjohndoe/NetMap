

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.GraphDataProviders.YouTube
{
//*****************************************************************************
//  Class: YouTubeUserNetworkGraphDataProvider
//
/// <summary>
/// Gets a network of people related to a YouTube user.
/// </summary>
///
/// <remarks>
/// Call <see cref="GraphDataProviderBase.TryGetGraphData" /> to get GraphML
/// that describes a network of people related to a YouTube user.
/// </remarks>
//*****************************************************************************

public class YouTubeUserNetworkGraphDataProvider : GraphDataProviderBase
{
   //*************************************************************************
    //  Constructor: YouTubeUserNetworkGraphDataProvider()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="YouTubeUserNetworkGraphDataProvider" /> class.
    /// </summary>
    //*************************************************************************

    public YouTubeUserNetworkGraphDataProvider()
    :
    base(GraphDataProviderName,
        "get the network of people related to a YouTube user")
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

        return ( new YouTubeGetUserNetworkDialog() );
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
        "YouTube User's Network";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
