using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.GraphDataProviders.Flickr
{
//*****************************************************************************
//  Class: FlickrRelatedTagNetworkGraphDataProvider
//
/// <summary>
/// Gets the network of tags related to a specified tag.
/// </summary>
///
/// <remarks>
/// Call <see cref="GraphDataProviderBase.TryGetGraphData" /> to get GraphML
/// that describes a network of Flickr tags related to a specified tag.
/// </remarks>
//*****************************************************************************

public class FlickrRelatedTagNetworkGraphDataProvider : GraphDataProviderBase
{
   //*************************************************************************
    //  Constructor: FlickrRelatedTagNetworkGraphDataProvider()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="FlickrRelatedTagNetworkGraphDataProvider" /> class.
    /// </summary>
    //*************************************************************************

    public FlickrRelatedTagNetworkGraphDataProvider()
    :
    base(GraphDataProviderName,
        "get the Flickr tags that are related to a specified tag")
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

        return ( new FlickrGetRelatedTagNetworkDialog() );
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
        "Flickr Related Tags Network";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
