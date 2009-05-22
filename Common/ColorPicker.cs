
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: ColorPicker
//
/// <summary>
/// Represents a user control that allows the user to pick a color.
/// </summary>
///
/// <remarks>
/// The user control consists of a panel that displays the current color and a
/// button that opens the standard ColorDialog for editing the color.
///
/// <para>
/// Use the <see cref="Color" /> property to set the initial color after the
/// user control is created.  If you are just displaying the color and don't
/// want the user to edit it, set <see cref="ShowButton" /> to false to hide
/// the button.  A <see cref="ColorChanged" /> event is fired when the user
/// edits the color.  Read the <see cref="Color" /> property to get the edited
/// color.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class ColorPicker : UserControl
{
    //*************************************************************************
    //  Constructor: ColorPicker()
    //
    /// <summary>
    /// Initializes a new instance of the ColorPicker class.
    /// </summary>
    //*************************************************************************

    public ColorPicker()
    {
        InitializeComponent();

        this.Color = Color.White;
    }

    //*************************************************************************
    //  Property: Color
    //
    /// <summary>
    /// Gets or sets the color being edited.
    /// </summary>
    ///
    /// <value>
    /// The color being edited.  The default is Color.White.
    /// </value>
    //*************************************************************************

    public Color Color
    {
        get
        {
            AssertValid();

            return (pnlColor.BackColor);
        }

        set
        {
            pnlColor.BackColor = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ShowButton
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the editing button should be
    /// visible.
    /// </summary>
    ///
    /// <value>
    /// true if the editing button should be visible.  Set this to false to
    /// display a read-only color.  The default is true.
    /// </value>
    //*************************************************************************

    public Boolean ShowButton
    {
        get
        {
            AssertValid();

            return (btnColor.Visible);
        }

        set
        {
            btnColor.Visible = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Event: ColorChanged
    //
    /// <summary>
    /// Occurs when the user edits the color.
    /// </summary>
    ///
    /// <remarks>
    /// Read the <see cref="Color" /> property to get the edited color.
    /// </remarks>
    //*************************************************************************

    public event EventHandler ColorChanged;


    //*************************************************************************
    //  Method: ChangeColor()
    //
    /// <summary>
    /// Gets a color from the user.
    /// </summary>
    //*************************************************************************

    protected void
    ChangeColor()
    {
        AssertValid();

        ColorDialog oColorDialog = new ColorDialog();
        oColorDialog.Color = this.Color;

        if (oColorDialog.ShowDialog() == DialogResult.OK)
        {
            Color oOldColor = this.Color;

            this.Color = oColorDialog.Color;

            if (this.Color != oOldColor && ColorChanged != null)
            {
                // Fire a ColorChanged event.

                ColorChanged( this, new EventArgs() );
            }
        }
    }

    //*************************************************************************
    //  Method: btnColor_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnColor button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnColor_Click
    (
        object sender,
        System.EventArgs e
    )
    {
        AssertValid();

        ChangeColor();
    }

    //*************************************************************************
    //  Method: pnlColor_MouseDown()
    //
    /// <summary>
    /// Handles the MouseDown event on the pnlColor Panel.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    pnlColor_MouseDown
    (
        object sender,
        MouseEventArgs e
    )
    {
        AssertValid();

        ChangeColor();
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
        // (Do nothing.)
    }


    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose( bool disposing )
    {
        if( disposing )
        {
            if(components != null)
            {
                components.Dispose();
            }
        }
        base.Dispose( disposing );
    }

    #region Component Designer generated code
    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.btnColor = new System.Windows.Forms.Button();
        this.pnlColor = new System.Windows.Forms.Panel();
        this.SuspendLayout();
        // 
        // btnColor
        // 
        this.btnColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnColor.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.btnColor.Location = new System.Drawing.Point(36, 4);
        this.btnColor.Name = "btnColor";
        this.btnColor.Size = new System.Drawing.Size(24, 24);
        this.btnColor.TabIndex = 3;
        this.btnColor.Text = "\u25bc";
        this.btnColor.Click += new System.EventHandler(this.btnColor_Click);
        // 
        // pnlColor
        // 
        this.pnlColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.pnlColor.Location = new System.Drawing.Point(4, 4);
        this.pnlColor.Name = "pnlColor";
        this.pnlColor.Size = new System.Drawing.Size(24, 24);
        this.pnlColor.TabIndex = 4;
        this.pnlColor.MouseDown += new MouseEventHandler(this.pnlColor_MouseDown);
        // 
        // ColorPicker
        // 
        this.Controls.Add(this.btnColor);
        this.Controls.Add(this.pnlColor);
        this.Name = "ColorPicker";
        this.Size = new System.Drawing.Size(64, 32);
        this.ResumeLayout(false);

    }
    #endregion

    private System.Windows.Forms.Button btnColor;
    private System.Windows.Forms.Panel pnlColor;


    //*************************************************************************
    //  Private member data
    //*************************************************************************

    private System.ComponentModel.Container components = null;
}

}
