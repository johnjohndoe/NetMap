using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace Microsoft.NodeXL.GraphDataProviders
{
//*****************************************************************************
//  Class: LimitToNControl
//
/// <summary>
/// UserControl for selecting a maximum number of objects to include.
/// </summary>
///
/// <remarks>
/// Use the <see cref="N" /> property to set and get the maximum number of
/// objects to include.
///
/// <para>
/// By default, the "objects" are people.  If the control is being used to
/// select a maximum number of something besides people, set the <see
/// cref="ObjectName" /> property.
/// </para>
///
/// <para>
/// This control uses the following keyboard shortcuts: T
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class LimitToNControl : UserControl
{
    //*************************************************************************
    //  Constructor: LimitToNControl()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="LimitToNControl" /> class.
    /// </summary>
    //*************************************************************************

    public LimitToNControl()
    {
        InitializeComponent();

        cbxN.PopulateWithObjectsAndText(
            100, "100",
            200, "200",
            300, "300",
            400, "400",
            500, "500",
            1000, "1,000"
            );

        AssertValid();
    }

    //*************************************************************************
    //  Property: N
    //
    /// <summary>
    /// Gets or sets the maximum number of objects to include.
    /// </summary>
    ///
    /// <value>
    /// The maximum number of objects to include, as an Int32.  Must be 100, 
    /// 200, 300, 400, 500, 1000, or Int32.MaxValue for "no limit."  The
    /// default is Int32.MaxValue.
    /// </value>
    //*************************************************************************

    public Int32
    N
    {
        get
        {
            AssertValid();

            if (chkLimitToN.Checked)
            {
                return ( (Int32)cbxN.SelectedValue );
            }

            return (Int32.MaxValue);
        }

        set
        {
            Boolean bLimitToN = (value < Int32.MaxValue);

            chkLimitToN.Checked = bLimitToN;

            if (bLimitToN)
            {
                cbxN.SelectedValue = value;

                // Verify that the specified value is one of the available
                // ComboBox values.

                Debug.Assert(cbxN.SelectedValue != null);
            }

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ObjectName
    //
    /// <summary>
    /// Gets or sets the name of the objects being limited.
    /// </summary>
    ///
    /// <value>
    /// The name of the objects being limited.  The default is "people".
    /// </value>
    //*************************************************************************

    public String
    ObjectName
    {
        get
        {
            AssertValid();

            return (lblObjectName.Text);
        }

        set
        {
            lblObjectName.Text = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: chkLimitToN_CheckedChanged()
    //
    /// <summary>
    /// Handles the CheckedChanged event on the chkLimitToN CheckBox.
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
    chkLimitToN_CheckedChanged
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        cbxN.Enabled = chkLimitToN.Checked;
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
