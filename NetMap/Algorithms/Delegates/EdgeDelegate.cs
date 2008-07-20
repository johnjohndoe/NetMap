
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.Algorithms
{
//*****************************************************************************
//  Delegate: EdgeDelegate
//
/// <summary>
/// Represents a method that returns a Boolean for a given edge.
/// </summary>
///
/// <param name="edge">
///	The edge to evaluate, as an <see cref="IEdge" />.
/// </param>
///
///	<returns>
///	A Boolean value whose meaning is defined by the class using the delegate.
///	</returns>
//*****************************************************************************

public delegate Boolean
EdgeDelegate
(
	IEdge edge
);

}
