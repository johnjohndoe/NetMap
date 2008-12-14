
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Algorithms
{
//*****************************************************************************
//  Delegate: VertexDelegate
//
/// <summary>
/// Represents a method that returns a Boolean for a given vertex.
/// </summary>
///
/// <param name="vertex">
///	The vertex to evaluate, as an <see cref="IVertex" />.
/// </param>
///
///	<returns>
///	A Boolean value whose meaning is defined by the class using the delegate.
///	</returns>
//*****************************************************************************

public delegate Boolean
VertexDelegate
(
	IVertex vertex
);

}
