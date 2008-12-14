
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.Visualization
{
//*****************************************************************************
//  Enum: VertexDrawerPrecedence
//
/// <summary>
/// Specifies which vertex drawer should be used for a vertex.
/// </summary>
//*****************************************************************************

public enum
VertexDrawerPrecedence
{
	/// <summary>
	/// Draw the vertex as a shape.
	/// </summary>

	Shape,

	/// <summary>
	/// Draw the vertex as an image.
	/// </summary>

	Image,

	/// <summary>
	/// Draw the vertex as a primary label.
	/// </summary>

	PrimaryLabel,
}
}
