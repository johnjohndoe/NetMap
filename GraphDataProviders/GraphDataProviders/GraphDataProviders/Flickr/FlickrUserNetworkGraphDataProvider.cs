using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.GraphDataProviders.Flickr
{
//*****************************************************************************
//  Class: FlickrUserNetworkGraphDataProvider
//
/// <summary>
/// Gets a network of people related to a Flickr user.
/// </summary>
///
/// <remarks>
/// Call <see cref="GraphDataProviderBase.TryGetGraphData" /> to get GraphML
/// that describes a network of people related to a Flickr user.
/// </remarks>
//*****************************************************************************

public class FlickrUserNetworkGraphDataProvider : GraphDataProviderBase
{
   //*************************************************************************
    //  Constructor: FlickrUserNetworkGraphDataProvider()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="FlickrUserNetworkGraphDataProvider" /> class.
    /// </summary>
    //*************************************************************************

    public FlickrUserNetworkGraphDataProvider()
    :
    base(GraphDataProviderName,
        "get the network of people related to a Flickr user")
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

        return ( new FlickrGetUserNetworkDialog() );
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
        "Flickr User's Network";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
