
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: FileUtil
//
/// <summary>
/// Static utility methods for working with files.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class FileUtil
{
    //*************************************************************************
    //  Method: RemoveIllegalFileNameChars()
    //
    /// <summary>
    /// Removes illegal characters from a file name.
    /// </summary>
    ///
    /// <param name="fileName">
    /// File name that may contain illegal characters.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="fileName" /> with any illegal characters removed.
    /// </returns>
    //*************************************************************************

    public static String
    RemoveIllegalFileNameChars
    (
        String fileName
    )
    {
        Debug.Assert(fileName != null);

        // Use a regular expression to do the work.

        Regex oRegex = new Regex(
            "[" + Regex.Escape(
            new String( Path.GetInvalidFileNameChars() ) ) + "]"
            );

        return ( oRegex.Replace(fileName, String.Empty) );
    }

    //*************************************************************************
    //  Method: EncodeIllegalFileNameChars()
    //
    /// <summary>
    /// Encodes illegal characters in a file name.
    /// </summary>
    ///
    /// <param name="fileName">
    /// File name that may contain illegal characters.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="fileName" /> with any illegal characters hex-encoded.
    /// </returns>
    //*************************************************************************

    public static String
    EncodeIllegalFileNameChars
    (
        String fileName
    )
    {
        Debug.Assert(fileName != null);

        String sEncodedFileName = fileName;

        foreach (char cInvalidFileNameChar in Path.GetInvalidFileNameChars() )
        {
            sEncodedFileName = sEncodedFileName.Replace(
                cInvalidFileNameChar.ToString(),
                Uri.HexEscape(cInvalidFileNameChar) );
        }

        return (sEncodedFileName);
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    // Are these constants defined anywhere in the .NET Framework?  They were
    // taken from PathTooLongException.Message, which is this:
    //
    // "The specified path, file name, or both are too long. The fully
    // qualified file name must be less than 260 characters, and the directory
    // name must be less than 248 characters."
    //

    /// Maximum file name length.

    public const Int32 MaximumFileNameLength = 259;

    /// Maximum folder name length.

    public const Int32 MaximumFolderNameLength = 247;
}

}
