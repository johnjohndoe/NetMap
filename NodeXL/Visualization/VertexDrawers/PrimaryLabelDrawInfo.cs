
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Diagnostics;

namespace Microsoft.NodeXL.Visualization
{
//*****************************************************************************
//  Class: PrimaryLabelDrawInfo
//
/// <summary>
/// The <see cref="PerVertexWithLabelDrawer" /> class uses an instance of this
/// class to store drawing information for a vertex's primary label in the
/// vertex's metadata.
/// </summary>
//*****************************************************************************

public class PrimaryLabelDrawInfo : Object
{
    //*************************************************************************
    //  Constructor: PrimaryLabelDrawInfo()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="PrimaryLabelDrawInfo" />
	/// class.
    /// </summary>
	///
    /// <param name="primaryLabel">
	/// The vertex's primary label.  Can be null or empty.
    /// </param>
	///
    /// <param name="textRectangle">
	/// The rectangle in which the primary label text should be drawn.
    /// </param>
	///
    /// <param name="outlineRectangle">
	/// The vertex rectangle to draw an outline around.
    /// </param>
    //*************************************************************************

    public PrimaryLabelDrawInfo
	(
		String primaryLabel,
		RectangleF textRectangle,
		Rectangle outlineRectangle
	)
    {
		m_sPrimaryLabel = primaryLabel;
		m_oTextRectangle = textRectangle;
		m_oOutlineRectangle = outlineRectangle;

		AssertValid();
    }

    //*************************************************************************
    //  Property: PrimaryLabel
    //
    /// <summary>
    /// Gets the vertex's primary label.
    /// </summary>
    ///
    /// <value>
	/// The vertex's primary label.  Can be null or empty.
    /// </value>
    //*************************************************************************

    public String
    PrimaryLabel
    {
        get
        {
            AssertValid();

            return (m_sPrimaryLabel);
        }
    }

    //*************************************************************************
    //  Property: TextRectangle
    //
    /// <summary>
	/// Gets the rectangle in which the primary label text should be drawn.
    /// </summary>
    ///
    /// <value>
	/// The rectangle in which the primary label text should be drawn.
    /// </value>
    //*************************************************************************

    public RectangleF
    TextRectangle
    {
        get
        {
            AssertValid();

            return (m_oTextRectangle);
        }
    }

    //*************************************************************************
    //  Property: OutlineRectangle
    //
    /// <summary>
	/// Gets the vertex rectangle to draw an outline around.
    /// </summary>
    ///
    /// <value>
	/// The vertex rectangle to draw an outline around.
    /// </value>
    //*************************************************************************

    public Rectangle
    OutlineRectangle
    {
        get
        {
            AssertValid();

            return (m_oOutlineRectangle);
        }
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
		// m_sPrimaryLabel
		// m_oTextRectangle
		// m_oOutlineRectangle
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// The vertex's primary label.  Can be null or empty.

	protected String m_sPrimaryLabel;

	/// The rectangle in which the primary label text should be drawn.

	protected RectangleF m_oTextRectangle;

	/// The vertex rectangle to draw an outline around.

	protected Rectangle m_oOutlineRectangle;
}

}
