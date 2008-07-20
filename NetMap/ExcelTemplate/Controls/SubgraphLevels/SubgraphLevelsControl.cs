using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//	Class: SubgraphLevelsControl
//
/// <summary>
/// UserControl that contains a ComboBox for selecting the number of adjacent
/// vertex levels to include in a subgraph, along with a PictureBox that shows
/// a sample subgraph for the selected level.
/// </summary>
///
/// <remarks>
/// Use the <see cref="Levels" /> property to set and get the number of
/// adjacent vertex levels to include in a subgraph.
///
/// <para>
/// This control uses a set of sample subgraph images included in the assembly
/// as embedded resources.
/// </para>
///
/// <para>
/// This control uses the following keyboard shortcuts: L
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class SubgraphLevelsControl : UserControl
{
	//*************************************************************************
	//	Constructor: SubgraphLevelsControl()
	//
	/// <summary>
	///	Initializes a new instance of the <see cref="SubgraphLevelsControl" />
	/// class.
	/// </summary>
	//*************************************************************************

	public SubgraphLevelsControl()
	{
		InitializeComponent();

		cbxLevels.PopulateWithObjectsAndText(
            1.0M, "1.0",
            1.5M, "1.5",
            2.0M, "2.0",
            2.5M, "2.5",
            3.0M, "3.0",
            3.5M, "3.5",
            4.0M, "4.0",
            4.5M, "4.5"
            );
	}

    //*************************************************************************
    //  Property: Levels
    //
    /// <summary>
    /// Gets or sets the number of adjacent vertex levels to include in a
	/// subgraph.
    /// </summary>
    ///
    /// <value>
	/// The number of adjacent vertex levels to include in a subgraph, as a
	/// Decimal.  The default is 1.0M.
    /// </value>
    //*************************************************************************

    public Decimal
	Levels
    {
        get
        {
            AssertValid();

			return ( (Decimal)cbxLevels.SelectedValue );
        }

        set
        {
			cbxLevels.SelectedValue = value;

            AssertValid();
        }
    }

	//*************************************************************************
	//	Method: cbxLevels_SelectedIndexChanged()
	//
	/// <summary>
	///	Handles the SelectedIndexChanged event on the cbxLevels ComboBox.
	/// </summary>
	///
	/// <param name="sender">
	///	Standard event argument.
	/// </param>
	///
	/// <param name="e">
	/// Standard event argument.
	/// </param>
	//*************************************************************************

    private void
	cbxLevels_SelectedIndexChanged
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();

		// The sample images displayed in the picSampleSubgraph PictureBox are
		// stored as embedded resources.  The file names, without namespaces,
		// are "1.0.jpg", "1.5.jpg", etc.

		String sResourceName = String.Format(

			"Images.SampleSubgraphs.{0}.jpg"
			,
			cbxLevels.Text  // Sample text: "1.5"
			);

		picSampleSubgraph.Image = new Bitmap(this.GetType(), sResourceName);
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
