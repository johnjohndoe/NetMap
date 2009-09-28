using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterSearchNetworkGraphDataProvider
//
/// <summary>
/// Gets the network of people who have tweeted a specified search term.
/// </summary>
///
/// <remarks>
/// Call <see cref="GraphDataProviderBase.TryGetGraphData" /> to get GraphML
/// that describes the network of people who have tweeted a specified search
/// term.
/// </remarks>
//*****************************************************************************

public class TwitterSearchNetworkGraphDataProvider : GraphDataProviderBase
{
   //*************************************************************************
    //  Constructor: TwitterSearchNetworkGraphDataProvider()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterSearchNetworkGraphDataProvider" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterSearchNetworkGraphDataProvider()
    :
    base(GraphDataProviderName,
        "get the network of people whose tweets contain a specified word")
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

        return ( new TwitterGetSearchNetworkDialog() );
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
        "Twitter Search Network";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
