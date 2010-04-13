
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.SocialNetworkLib;

namespace Microsoft.NodeXL.GraphDataProviders
{
//*****************************************************************************
//  Class: NetworkLevelControl
//
/// <summary>
/// UserControl that contains a ComboBox for selecting a <see
/// cref="NetworkLevel" /> value, along with a PictureBox that shows a sample
/// network for the selected level.
/// </summary>
///
/// <remarks>
/// Use the <see cref="Level" /> property to set and get the network level.
///
/// <para>
/// This control uses a set of sample graph images included in the assembly as
/// embedded resources.
/// </para>
///
/// <para>
/// This control uses the following keyboard shortcuts: L
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class NetworkLevelControl : UserControl
{
    //*************************************************************************
    //  Constructor: NetworkLevelControl()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="NetworkLevelControl" />
    /// class.
    /// </summary>
    //*************************************************************************

    public NetworkLevelControl()
    {
        InitializeComponent();

        cbxLevel.PopulateWithObjectsAndText(
            NetworkLevel.One, "1.0",
            NetworkLevel.OnePointFive, "1.5",
            NetworkLevel.Two, "2.0"
            );
    }

    //*************************************************************************
    //  Property: Level
    //
    /// <summary>
    /// Gets or sets the network level.
    /// </summary>
    ///
    /// <value>
    /// The network level, as a NetworkLevel.  Must be NetworkLevel.One,
    /// NetworkLevel.OnePointFive, or NetworkLevel.Two.  The default is
    /// NetworkLevel.One.
    /// </value>
    //*************************************************************************

    public NetworkLevel
    Level
    {
        get
        {
            AssertValid();

            return ( (NetworkLevel)cbxLevel.SelectedValue );
        }

        set
        {
            cbxLevel.SelectedValue = value;

            // Verify that the specified value is one of the available ComboBox
            // values.

            Debug.Assert(cbxLevel.SelectedValue != null);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: cbxLevel_SelectedIndexChanged()
    //
    /// <summary>
    /// Handles the SelectedIndexChanged event on the cbxLevel ComboBox.
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
    cbxLevel_SelectedIndexChanged
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        // The sample images displayed in the picLevel PictureBox are stored as
        // embedded resources.  The file names, without namespaces, are
        // "1.0.jpg", "1.5.jpg", etc.

        String sResourceName = String.Format(

            "Images.NetworkLevels.{0}.jpg"
            ,
            cbxLevel.Text  // Sample text: "1.5"
            );

        picLevel.Image = new Bitmap(this.GetType(), sResourceName);
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


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}
}
