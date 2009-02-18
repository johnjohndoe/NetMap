
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.ControlLib;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: ToolTipPanel
//
/// <summary>
/// Panel that displays a tooltip.
/// </summary>
///
/// <remarks>
/// This class can be used in conjunction with the ToolTipTracker class by a
/// Control object that displays various objects within its window and wants to
/// show a tooltip for each object.  (The ToolTip class in the FCL makes it
/// easy to show a single tooltip for an entire control, but it does not
/// support different tooltips for different parts of the control's window.
/// See the ToolTipTracker class for more details.)
///
/// <para>
/// To use this class, have the Control object create a child <see
/// cref="ToolTipPanel" /> object.  Call <see cref="ShowToolTip" /> to show a
/// tooltip.  Call <see cref="HideToolTip" /> to hide it.
/// </para>
///
/// <para>
/// <see cref="ToolTipPanel" /> is better than using a standard Label control.
/// A Label doesn't pay attention to embedded line breaks and therefore can't
/// display a multiline tooltip.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class ToolTipPanel : Panel
{
    //*************************************************************************
    //  Constructor: ToolTipPanel()
    //
    /// <summary>
    /// Initializes a new instance of the ToolTipPanel class.
    /// </summary>
    //*************************************************************************

    public ToolTipPanel()
    {
        m_sText = null;

        this.ForeColor = SystemColors.InfoText;
        this.Visible = false;

        // Prevent the panel from capturing the mouse, which would mess up the
        // mouse-handling code in the parent control and in ToolTipTracker.

        this.Enabled = false;

        AssertValid();
    }

    //*************************************************************************
    //  Method: ShowToolTip()
    //
    /// <summary>
    /// Shows a tooltip.
    /// </summary>
    ///
    /// <param name="sText">
    /// Tooltip text.  Can contain multiple lines separated by "\r\n".  Can be
    /// null.
    /// </param>
    ///
    /// <param name="oParentControl">
    /// Parent control that is displaying the tooltip.
    /// </param>
    ///
    /// <remarks>
    /// This method positions, sizes, and shows the tooltip panel.  Call <see
    /// cref="HideToolTip" /> to hide the tooltip.
    /// </remarks>
    //*************************************************************************

    public void
    ShowToolTip
    (
        String sText,
        Control oParentControl
    )
    {
        Debug.Assert(oParentControl != null);
        AssertValid();

        m_sText = sText;

        // Set the panel's rectangle and make it visible.

        Rectangle oPanelRectangle = ComputePanelRectangle(
            sText, oParentControl);

        this.Location = oPanelRectangle.Location;
        this.Size = oPanelRectangle.Size;
        this.Visible = true;
    }

    //*************************************************************************
    //  Method: HideToolTip()
    //
    /// <summary>
    /// Hides the tooltip.
    /// </summary>
    ///
    /// <remarks>
    /// This method hides the tooltip shown by <see cref="ShowToolTip" />.
    /// It's okay to call this method before <see cref="ShowToolTip" /> has
    /// been called.
    /// </remarks>
    //*************************************************************************

    public void
    HideToolTip()
    {
        AssertValid();

        this.Visible = false;
    }

    //*************************************************************************
    //  Method: ComputePanelRectangle()
    //
    /// <summary>
    /// Computes the rectangle where the panel should be displayed.
    /// </summary>
    ///
    /// <param name="sText">
    /// Tooltip text.  Can contain multiple lines separated by "\r\n".  Can be
    /// null.
    /// </param>
    ///
    /// <param name="oParentControl">
    /// Parent control that is displaying the tooltip.
    /// </param>
    ///
    /// <returns>
    /// The rectangle where the panel should be displayed.
    /// </returns>
    //*************************************************************************

    protected Rectangle
    ComputePanelRectangle
    (
        String sText,
        Control oParentControl
    )
    {
        Debug.Assert(oParentControl != null);
        AssertValid();

        // Get some information about the parent control and the cursor.

        Rectangle oParentClientRectangle = oParentControl.ClientRectangle;

        PointF oClientMousePosition =
            ControlUtil.GetClientMousePosition(oParentControl);

        Size oCursorSize = oParentControl.Cursor.Size;

        // The upper-left corner of the panel will be below the cursor.

        Int32 iX = (Int32)oClientMousePosition.X;
        Int32 iY = (Int32)oClientMousePosition.Y + oCursorSize.Height;

        // If the user is moving the mouse quickly, it's possible that the
        // client mouse position is outside the parent control's rectangle.
        // Adjust iX and iY if this is the case.

        Size oParentClientSize = oParentClientRectangle.Size;

        iX = Math.Max(iX, 0);
        iX = Math.Min(oParentClientSize.Width, iX);
        iY = Math.Max(iY, 0);
        iY = Math.Min(oParentClientSize.Height, iY);

        Graphics oGraphics = this.CreateGraphics();

        // Get the text dimensions.

        SizeF oTextSize = oGraphics.MeasureString(sText, this.Font);

        oGraphics.Dispose();
        oGraphics = null;

        // Create the tooltip rectangle.  Add a uniform margin around the text.

        Rectangle oPanelRectangle = new Rectangle(
            iX,
            iY,
            (Int32)Math.Ceiling(oTextSize.Width + (2 * InternalMargin) + 2),
            (Int32)Math.Ceiling(oTextSize.Height + (2 * InternalMargin) + 2)
            );

        // Allow for a margin between the tooltip rectangle and the rectangle
        // of the parent control.

        if (oPanelRectangle.Bottom + InternalMargin > oParentClientSize.Height)
        {
            // The rectangle extends over the bottom edge of the parent
            // control.  Move it up.

            oPanelRectangle.Offset(
                0,
                oParentClientSize.Height - oPanelRectangle.Bottom
                    - InternalMargin
                );
        }

        if (oPanelRectangle.Right + InternalMargin > oParentClientSize.Width)
        {
            // The rectangle extends over the right edge of the parent control.
            // Move it to the left.

            oPanelRectangle.Offset(
                oParentClientSize.Width - oPanelRectangle.Right
                    - InternalMargin,
                0
                );
        }

        // Clip the rectangle to the parent control.

        oPanelRectangle.Intersect(oParentClientRectangle);

        Debug.Assert(oPanelRectangle.Left >= 0);
        Debug.Assert(oPanelRectangle.Top >= 0);
        Debug.Assert(oPanelRectangle.Right <= oParentClientSize.Width);
        Debug.Assert(oPanelRectangle.Bottom <= oParentClientSize.Height);

        return (oPanelRectangle);
    }

    //*************************************************************************
    //  Method: OnPaint()
    //
    /// <summary>
    /// Paints the control.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected override void
    OnPaint
    (
        PaintEventArgs e
    )
    {
        Debug.Assert(e != null);
        AssertValid();

        Graphics oGraphics = e.Graphics;

        Brush oBrush = new SolidBrush(this.ForeColor);

        oGraphics.DrawString( m_sText, this.Font, oBrush,
            new Point(InternalMargin, InternalMargin) );

        oBrush.Dispose();
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
        // m_sText
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Margin between the tooltip text and the tooltip rectangle, and between
    /// the tooltip rectangle and the rectangle of the parent control.  (These
    /// two margins are arbitrarily set to the same value.)

    protected const Int32 InternalMargin = 1;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Tooltip text.  Can contain multiple lines separated by "\r\n".  Can be
    /// null.

    protected String m_sText;
}

}
