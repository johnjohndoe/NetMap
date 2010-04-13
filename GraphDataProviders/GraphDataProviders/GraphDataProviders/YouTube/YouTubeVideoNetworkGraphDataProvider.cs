

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.GraphDataProviders.YouTube
{
//*****************************************************************************
//  Class: YouTubeVideoNetworkGraphDataProvider
//
/// <summary>
/// Gets a network of related YouTube videos.
/// </summary>
///
/// <remarks>
/// Call <see cref="GraphDataProviderBase.TryGetGraphData" /> to get GraphML
/// that describes a network of related YouTube videos.
/// </remarks>
//*****************************************************************************

public class YouTubeVideoNetworkGraphDataProvider : GraphDataProviderBase
{
   //*************************************************************************
    //  Constructor: YouTubeVideoNetworkGraphDataProvider()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="YouTubeVideoNetworkGraphDataProvider" /> class.
    /// </summary>
    //*************************************************************************

    public YouTubeVideoNetworkGraphDataProvider()
    :
    base(GraphDataProviderName,
        "get a network of related YouTube videos.")
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

        return ( new YouTubeGetVideoNetworkDialog() );
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
        "YouTube Video Network";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
