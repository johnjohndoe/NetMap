
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Diagnostics;

namespace Microsoft.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterAccessToken
//
/// <summary>
/// Represents a Twitter OAuth access token.
/// </summary>
///
/// <remarks>
/// Call <see cref="Save" /> when an OAuth access token is obtained from
/// Twitter.  <see cref="Save" /> saves the token to a file in the user's
/// profile.  Call <see cref="TryLoad" /> to determine whether a token has been
/// obtained from Twitter.
///
/// <para>
/// (Confusingly, a Twitter "token" consists of both a token string and a
/// secret string.  The same word is used for both a single string and a pair
/// of strings.)
/// </para>
///
/// </remarks>
//*****************************************************************************

public class TwitterAccessToken : Object
{
    //*************************************************************************
    //  Constructor: TwitterAccessToken()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="TwitterAccessToken" />
    /// class.
    /// </summary>
    //*************************************************************************

    public TwitterAccessToken()
    {
        m_sToken = null;
        m_sSecret = null;
    }

    //*************************************************************************
    //  Method: Save()
    //
    /// <summary>
    /// Saves an access token to a file in the user's profile.
    /// </summary>
    ///
    /// <param name="token">
    /// Twitter access token.
    /// </param>
    ///
    /// <param name="secret">
    /// Twitter access token secret.
    /// </param>
    //*************************************************************************

    public void
    Save
    (
        String token,
        String secret
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(token) );
        Debug.Assert( !String.IsNullOrEmpty(secret) );
        AssertValid();

        m_sToken = token;
        m_sSecret = secret;

        String sAccessTokenFilePath = GetAccessTokenFilePath();

        Directory.CreateDirectory( Path.GetDirectoryName(
            sAccessTokenFilePath) );

        using ( StreamWriter oStreamWriter =
            new StreamWriter(sAccessTokenFilePath) )
        {
            oStreamWriter.Write(m_sToken + "\t" + m_sSecret);
        }
    }

    //*************************************************************************
    //  Method: TryLoad()
    //
    /// <summary>
    /// Attempts to load an access token from a file in the user's profile.
    /// </summary>
    ///
    /// <param name="token">
    /// Where the Twitter access token gets stored if true is returned.
    /// </param>
    ///
    /// <param name="secret">
    /// Where the Twitter access token secret gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the access token was loaded.
    /// </returns>
    ///
    /// <remarks>
    /// If the load is successful, the token is cached so that additional disk
    /// accesses are not required if this method is called again.
    /// </remarks>
    //*************************************************************************

    public Boolean
    TryLoad
    (
        out String token,
        out String secret
    )
    {
        AssertValid();

        String sAccessTokenFilePath = GetAccessTokenFilePath();

        if ( m_sToken == null && File.Exists(sAccessTokenFilePath) )
        {
            String sFileContents;

            using ( StreamReader oStreamReader =
                new StreamReader(sAccessTokenFilePath) )
            {
                sFileContents = oStreamReader.ReadToEnd();
            }

            String [] asFields = sFileContents.Split( new Char[]{'\t'} );

            Debug.Assert(asFields.Length == 2);
            m_sToken = asFields[0];
            m_sSecret = asFields[1];

            Debug.Assert( !String.IsNullOrEmpty(m_sToken) );
            Debug.Assert( !String.IsNullOrEmpty(m_sSecret) );
        }

        token = m_sToken;
        secret = m_sSecret;

        return (token != null);
    }

    //*************************************************************************
    //  Method: Exists()
    //
    /// <summary>
    /// Determines whether an access token exists.
    /// </summary>
    ///
    /// <returns>
    /// true if an access token exists.
    /// </returns>
    //*************************************************************************

    public static Boolean
    Exists()
    {
        String sToken, sSecret;

        return ( new TwitterAccessToken().TryLoad(out sToken, out sSecret) );
    }

    //*************************************************************************
    //  Method: Delete()
    //
    /// <summary>
    /// Deletes the access token if it exists.
    /// </summary>
    //*************************************************************************

    public static void
    Delete()
    {
        try
        {
            File.Delete( GetAccessTokenFilePath() );
        }
        catch (DirectoryNotFoundException)
        {
        }
    }

    //*************************************************************************
    //  Method: GetAccessTokenFilePath()
    //
    /// <summary>
    /// Gets the full path to the current user's Twitter access token file.
    /// </summary>
    ///
    /// <remarks>
    /// This is public only to make it accessible to unit tests.  It should not
    /// be used by other callers.
    /// </remarks>
    //*************************************************************************

    public static String
    GetAccessTokenFilePath()
    {
        String sFolder = Path.Combine(

            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),

            @"MicrosoftResearch\NodeXLExcel2007Template"
            );

        return ( Path.Combine(sFolder, FileName) );
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
        Debug.Assert( String.IsNullOrEmpty(m_sToken) ==
            String.IsNullOrEmpty(m_sSecret) );
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Name of the file where the user's access token is stored, without a
    /// path.

    protected const String FileName = "TwitterAccessToken.txt";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Twitter access token, or null if a token hasn't been obtained for the
    /// current user.

    protected String m_sToken;

    /// Twitter access token secret, or null if a token hasn't been obtained
    /// for the current user.

    protected String m_sSecret;
}

}
