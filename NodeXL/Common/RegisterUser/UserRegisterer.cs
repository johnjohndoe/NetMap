
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Win32;

namespace Microsoft.NodeXL.Common
{
//*****************************************************************************
//  Class: UserRegisterer
//
/// <summary>
/// Contains helper methods for registering a user.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class UserRegisterer
{
    //*************************************************************************
    //  Method: CanCreateRegistrationEmail()
    //
    /// <summary>
    /// Determines whether an email that will request registration can be
    /// created.
    /// </summary>
    ///
    /// <returns>
    /// true if an email can be created, false if no mailto protocol is
    /// configured.
    /// </returns>
    ///
    /// <remarks>
    /// If true is returned, <see cref="TryCreateRegistrationEmail" /> can be
    /// called.  Note, however, that <see cref="TryCreateRegistrationEmail" />
    /// can fail and return false if the mailto protocol is improperly
    /// configured, so the return value of <see
    /// cref="TryCreateRegistrationEmail" /> should always be checked.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    CanCreateRegistrationEmail()
    {
        return ( Registry.GetValue(MailToRegistryKeyToCheck, String.Empty,
            null) != null );
    }

    //*************************************************************************
    //  Method: TryCreateRegistrationEmail()
    //
    /// <summary>
    /// Attempts to create an email that will request registration.
    /// </summary>
    ///
    /// <returns>
    /// true if the email was created, false if no mailto protocol is
    /// configured.
    /// </returns>
    ///
    /// <remarks>
    /// This method attempts to use a configured mailto protocol to create an
    /// email.  If there is no mailto protocol, false is returned.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryCreateRegistrationEmail()
    {
        try
        {
            Process.Start( String.Format(
                "mailto:{0}?subject=Register"
                ,
                ProjectInformation.RegistrationEmailAddress
                ) );
        }
        catch (Win32Exception)
        {
            return (false);
        }

        return (true);
    }


    //*************************************************************************
    //  Private constants
    //*************************************************************************

    /// Name of the key to check to determine if a mailto protocol is
    /// configured.  If the key exists and has a non-null default value, then
    /// the mailto protocol is configured.

    private static String MailToRegistryKeyToCheck =
        @"HKEY_CLASSES_ROOT\mailto\shell\open\command";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
