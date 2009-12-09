using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.GraphDataProviders.Flickr
{
//*****************************************************************************
//  Class: FlickrApiKeyControl
//
/// <summary>
/// UserControl that gets a Flickr API key.
/// </summary>
///
/// <remarks>
/// Use the <see cref="ApiKey" /> property to set and get the API key.  Use
/// <see cref="Validate" /> to validate the API key.
///
/// <para>
/// This control uses the following keyboard shortcut: F
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class FlickrApiKeyControl : UserControl
{
    //*************************************************************************
    //  Constructor: FlickrApiKeyControl()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="FlickrApiKeyControl" />
    /// class.
    /// </summary>
    //*************************************************************************

    public FlickrApiKeyControl()
    {
        InitializeComponent();

        this.lnkRequestFlickrApiKey.FileName = RequestFlickrApiKeyUrl;

        AssertValid();
    }

    //*************************************************************************
    //  Property: ApiKey
    //
    /// <summary>
    /// Gets or sets the Flickr API key.
    /// </summary>
    ///
    /// <value>
    /// The Flickr API key, as a String.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    public String
    ApiKey
    {
        get
        {
            AssertValid();

            return ( txbApiKey.Text.Trim() );
        }

        set
        {
            txbApiKey.Text = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: Validate()
    //
    /// <summary>
    /// Validates the user's entries.
    /// </summary>
    ///
    /// <returns>
    /// true if validation passes.
    /// </returns>
    ///
    /// <remarks>
    /// If the user's entries are invalid, an error message is displayed and
    /// false is returned.  Otherwise, true is returned.
    /// </remarks>
    //*************************************************************************

    public new Boolean
    Validate()
    {
        AssertValid();

        String sApiKey;

        return ( FormUtil.ValidateRequiredTextBox(txbApiKey,
            "Enter a Flickr API key.", out sApiKey) );
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
    //  Protected constants
    //*************************************************************************

    /// Flickr Web page for requesting an API key.

    protected const String RequestFlickrApiKeyUrl =
        "http://www.flickr.com/services/api/misc.api_keys.html";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}
}
