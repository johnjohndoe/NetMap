
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Enum: TwitterAuthorizationStatus
//
/// <summary>
/// Specifies the user's Twitter authorization status.
/// </summary>
//*****************************************************************************

public enum
TwitterAuthorizationStatus
{
    /// <summary>
    /// The user doesn't have a Twitter account, so all requests to Twitter
    /// will be unauthenticated.
    /// </summary>

    NoTwitterAccount,

    /// <summary>
    /// The user has a Twitter account, but she has not yet authorized Twitter
    /// to use it.
    /// </summary>

    HasTwitterAccountNotAuthorized,

    /// <summary>
    /// The user has a Twitter account and she has authorized Twitter to use
    /// it.
    /// </summary>

    HasTwitterAccountAuthorized,
}

}
