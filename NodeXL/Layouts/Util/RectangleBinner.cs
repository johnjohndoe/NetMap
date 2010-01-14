
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Diagnostics;

namespace Microsoft.NodeXL.Layouts
{
//*****************************************************************************
//  Class: RectangleBinner
//
/// <summary>
/// Splits a parent rectangle into "bin" rectangles.
/// </summary>
///
/// <remarks>
/// A bin is a small square that fits into a parent rectangle.  This class
/// splits the parent into consecutive bins, running from left to right along
/// the bottom of the parent and then moving up a row.
///
/// <para>
/// Pass the parent rectangle to the constructor, along with the length of the
/// bin.  Call <see cref="TryGetNextBin" /> to get the next bin, and call <see
/// cref="TryGetRemainingRectangle" /> to get any space not occupied by the
/// returned bins.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class RectangleBinner : Object
{
    //*************************************************************************
    //  Constructor: RectangleBinner()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="RectangleBinner" /> class.
    /// </summary>
    ///
    /// <param name="parent">
    /// The parent rectangle to split into bins.
    /// </param>
    ///
    /// <param name="binLength">
    /// Length and width of each bin square.  Must be greater than zero.
    /// </param>
    //*************************************************************************

    public RectangleBinner
    (
        Rectangle parent,
        Int32 binLength
    )
    {
        m_oParent = parent;
        m_iBinLength = binLength;

        // Start one bin out of bounds, so that the Rectangle.Offset() call in
        // the first call to TryGetNextBin() results in the first in-bounds
        // bin.

        m_oLastBin = Rectangle.FromLTRB(
            m_oParent.Left - m_iBinLength,
            m_oParent.Bottom - m_iBinLength,
            m_oParent.Left,
            m_oParent.Bottom
            );

        m_bBinReturned = false;

        AssertValid();
    }

    //*************************************************************************
    //  Method: TryGetNextBin()
    //
    /// <summary>
    /// Attempts to get the next bin.
    /// </summary>
    ///
    /// <param name="nextBin">
    /// Where the bin gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if there is enough room left for another bin.
    /// </returns>
    //*************************************************************************

    public Boolean
    TryGetNextBin
    (
        out Rectangle nextBin
    )
    {
        AssertValid();

        nextBin = m_oLastBin;

        // Move the bin rectangle one column to the right.

        nextBin.Offset(m_iBinLength, 0);

        if ( !m_oParent.Contains(nextBin) )
        {
            // Move the bin rectangle one row up and back to the leftmost
            // column.

            nextBin = Rectangle.FromLTRB(
                m_oParent.Left,
                nextBin.Top - m_iBinLength,
                m_oParent.Left + m_iBinLength,
                nextBin.Bottom - m_iBinLength
                );

            if ( !m_oParent.Contains(nextBin) )
            {
                // There is no room for an additional bin rectangle.

                return (false);
            }
        }

        m_oLastBin = nextBin;
        m_bBinReturned = true;
        return (true);
    }

    //*************************************************************************
    //  Method: TryGetRemainingRectangle()
    //
    /// <summary>
    /// Attempts to get the remaining space not occupied by the bins that have
    /// been returned by <see cref="TryGetNextBin" />.
    /// </summary>
    ///
    /// <param name="remainingRectangle">
    /// Where the remaining rectangle gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if there is any room remaining.
    /// </returns>
    //*************************************************************************

    public Boolean
    TryGetRemainingRectangle
    (
        out Rectangle remainingRectangle
    )
    {
        AssertValid();

        if (!m_bBinReturned)
        {
            remainingRectangle = m_oParent;
        }
        else if (m_oLastBin.Top == m_oParent.Top)
        {
            // There is at least one bin in the top row, and there is no space
            // above them.  Return the space to the right of the last bin in
            // the top row.

            remainingRectangle = Rectangle.FromLTRB(
                m_oLastBin.Right,
                m_oParent.Top,
                m_oParent.Right,
                m_oLastBin.Bottom
                );
        }
        else
        {
            // Return the entire row above the last bin.

            remainingRectangle = Rectangle.FromLTRB(
                m_oParent.Left,
                m_oParent.Top,
                m_oParent.Right,
                m_oLastBin.Top
            );
        }

        // Note that Rectangle.Contains() returns true even if the rectangle
        // passed to it has zero height or width.

        return ( remainingRectangle.Width > 0 && remainingRectangle.Height >0
            && m_oParent.Contains(remainingRectangle) );
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
        // m_oParent
        Debug.Assert(m_iBinLength > 0);
        // m_oLastBin
        // m_bBinReturned
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Parent rectangle to split into bins.

    protected Rectangle m_oParent;

    /// Length and width of each bin square.

    protected Int32 m_iBinLength;

    /// The last bin returned by TryGetNextBin(), or a bin that is one position
    /// out of bounds if TryGetNextBin() hasn't been called yet.

    protected Rectangle m_oLastBin;

    /// true if at least one bin has been returned.

    protected Boolean m_bBinReturned;
}

}
