using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.GraphDataProviders.Flickr
{
//*****************************************************************************
//  Class: FlickrRelatedTagsGraphDataProvider
//
/// <summary>
/// Gets the network of tags related to a specified tag.
/// </summary>
///
/// <remarks>
/// Call <see cref="GraphDataProviderBase.TryGetGraphData" /> to get GraphML
/// that describes the 1.5-degree network of Flickr tags related to a specified
/// tag.
/// </remarks>
//*****************************************************************************

public class FlickrRelatedTagsGraphDataProvider : GraphDataProviderBase
{
   //*************************************************************************
    //  Constructor: FlickrRelatedTagsGraphDataProvider()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="FlickrRelatedTagsGraphDataProvider" /> class.
    /// </summary>
    //*************************************************************************

    public FlickrRelatedTagsGraphDataProvider()
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

        return ( new FlickrGetRelatedTagsDialog() );
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
        "Network of Related Flickr Tags";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
