using System;
using System.Diagnostics;

namespace Microsoft.SocialNetworkLib
{
//*****************************************************************************
//  Enum: NetworkLevel
//
/// <summary>
/// Represents a level in a network.
/// </summary>
///
/// <remarks>
/// "Level" is another word for "degree," as in a 1.5-degree network.
/// "Degree" is avoided because it has an additional meaning in networks
/// (vertex degree) and could thus be confusing.
/// </remarks>
//*****************************************************************************

public enum
NetworkLevel
{
    /// <summary>
    /// 1-level network.
    /// </summary>

    One,

    /// <summary>
    /// 1.5-level network.
    /// </summary>

    OnePointFive,

    /// <summary>
    /// 2-level network.
    /// </summary>

    Two,

    /// <summary>
    /// 2.5-level network.
    /// </summary>

    TwoPointFive,

    /// <summary>
    /// 3-level network.
    /// </summary>

    Three,

    /// <summary>
    /// 3.5-level network.
    /// </summary>

    ThreePointFive,

    /// <summary>
    /// 4-level network.
    /// </summary>

    Four,

    /// <summary>
    /// 4.5-level network.
    /// </summary>

    FourPointFive,
}
}
