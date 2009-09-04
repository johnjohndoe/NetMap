using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace Microsoft.NodeXL.GraphDataProviders.Controls.LimitToNPeople
{
//*****************************************************************************
//  Class: LimitToNPeopleControl
//
/// <summary>
/// UserControl that contains a ComboBox for selecting a maximum number of
/// people to include.
/// </summary>
///
/// <remarks>
/// Use the <see cref="N" /> property to set and get the maximum number of
/// people to include.
///
/// <para>
/// This control uses the following keyboard shortcuts: M
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class LimitToNPeopleControl : UserControl
{
    //*************************************************************************
    //  Constructor: LimitToNPeopleControl()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="LimitToNPeopleControl" />
    /// class.
    /// </summary>
    //*************************************************************************

    public LimitToNPeopleControl()
    {
        InitializeComponent();

        cbxN.PopulateWithObjectsAndText(
            100, "100",
            200, "200",
            300, "300"
            );

        AssertValid();
    }

    //*************************************************************************
    //  Property: N
    //
    /// <summary>
    /// Gets or sets the maximum number of people to include.
    /// </summary>
    ///
    /// <value>
    /// The maximum number of people to include, as an Int32.  Must be 100, 
    /// 200, 300, or Int32.MaxValue for "no limit."  The default is
    /// Int32.MaxValue.
    /// </value>
    //*************************************************************************

    public Int32
    N
    {
        get
        {
            AssertValid();

            if (chkLimitToNPeople.Checked)
            {
                return ( (Int32)cbxN.SelectedValue );
            }

            return (Int32.MaxValue);
        }

        set
        {
            Boolean bLimitToNPeople = (value < Int32.MaxValue);

            if (bLimitToNPeople)
            {
                cbxN.SelectedValue = value;

                // Verify that the specified value is one of the available
                // ComboBox values.

                Debug.Assert(cbxN.SelectedValue != null);
            }

            chkLimitToNPeople.Checked = bLimitToNPeople;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: chkLimitToNPeople_CheckedChanged()
    //
    /// <summary>
    /// Handles the CheckedChanged event on the chkLimitToNPeople CheckBox.
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
    chkLimitToNPeople_CheckedChanged
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        cbxN.Enabled = chkLimitToNPeople.Checked;
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
