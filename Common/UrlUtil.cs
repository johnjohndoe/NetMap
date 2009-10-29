
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Web;
using System.Text;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: UrlUtil
//
/// <summary>
/// Utility methods for working with URLs.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class UrlUtil : Object
{
    //*************************************************************************
    //  Method: EncodeUrlParameter()
    //
    /// <summary>
    /// Encodes an URL parameter by converting it to UTF-8 and then
    /// URL-encoding the UTF-8.
    /// </summary>
    ///
    /// <param name="urlParameter">
    /// The URL parameter to be encoded.  Can't be null.
    /// </param>
    ///
    /// <returns>
    /// The encoded parameter.
    /// </returns>
    //*************************************************************************

    public static String
    EncodeUrlParameter
    (
        String urlParameter
    )
    {
        Debug.Assert(urlParameter != null);

        return ( HttpUtility.UrlEncode( Encoding.UTF8.GetBytes(
            urlParameter.ToCharArray() ) ) );
    }
}

}
