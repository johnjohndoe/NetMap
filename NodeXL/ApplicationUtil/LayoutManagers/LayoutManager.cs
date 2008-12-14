
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.NodeXL.Visualization;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ApplicationUtil
{
//*****************************************************************************
//  Class: LayoutManager
//
/// <summary>
/// Helper class for managing layouts used by the <see cref="NodeXLControl" />.
/// </summary>
///
/// <remarks>
/// This class defines a set of layouts available for use with the <see
/// cref="NodeXLControl" />.  It provides a <see cref="Layout" /> property for
/// keeping track of the layout currently in use, a <see
/// cref="LayoutChanged" /> event that is raised when the layout is changed,
/// and an <see cref="ApplyLayoutToNodeXLControl" /> method for setting the <see
/// cref="NodeXLControl.Layout" />, <see cref="NodeXLControl.VertexDrawer" />,
/// and <see cref="NodeXLControl.EdgeDrawer" /> properties on a <see
/// cref="NodeXLControl" /> with a set of objects that are designed to work
/// together.
///
/// <para>
/// Use the derived <see cref="LayoutManagerForMenu" /> class if your
/// application uses ToolStripMenuItems for selecting the current layout.
/// </para>
///
/// </remarks>
///
/// <seealso cref="LayoutManagerForMenu" />
//*****************************************************************************

public class LayoutManager : Object
{
    //*************************************************************************
    //  Constructor: LayoutManager()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="LayoutManager" /> class.
    /// </summary>
    //*************************************************************************

    public LayoutManager()
    {
		m_eLayout = LayoutType.FruchtermanReingold;

		AssertValid();
    }

    //*************************************************************************
    //  Property: Layout
    //
    /// <summary>
    /// Gets or sets the layout type to use.
    /// </summary>
    ///
    /// <value>
    /// The layout type to use, as a <see cref="LayoutType" />.  The default is
	/// <see cref="LayoutType.FruchtermanReingold" />.
    /// </value>
    //*************************************************************************

    public LayoutType
    Layout
    {
        get
        {
            AssertValid();

            return (m_eLayout);
        }

		set
		{
			this.ArgumentChecker.CheckPropertyIsDefined(
				"Layout", value, typeof(LayoutType) );

			if (value == m_eLayout)
			{
				return;
			}

			m_eLayout = value;

			EventHandler oLayoutChanged = this.LayoutChanged;

			if (oLayoutChanged != null)
			{
				oLayoutChanged(this, EventArgs.Empty);
			}

			AssertValid();
		}
    }

    //*************************************************************************
    //  Method: ApplyLayoutToNodeXLControl()
    //
    /// <summary>
	/// Sets the layout-related properties on a <see cref="NodeXLControl" />.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
	/// Control to set the layout-related properties on.
    /// </param>
	///
    /// <param name="margin">
	/// The margin to use for the layout.
    /// </param>
	///
	/// <remarks>
	/// This method sets the <see cref="NodeXLControl.Layout" />, <see
	/// cref="NodeXLControl.VertexDrawer" />, and <see
	/// cref="NodeXLControl.EdgeDrawer" /> properties on <paramref
	/// name="nodeXLControl" /> to a set of objects that are designed to work
	/// together to render a layout of type <see cref="Layout" />.
	///
	/// <para>
	/// Important: The call to this method must be surrounded by <see
	/// cref="NodeXLControl.BeginUpdate" /> and <see
	/// cref="NodeXLControl.EndUpdate()" /> to avoid unwanted graph redraws.
	/// </para>
	///
	/// </remarks>
    //*************************************************************************

    public void
    ApplyLayoutToNodeXLControl
	(
		NodeXLControl nodeXLControl,
		Int32 margin
	)
	{
		const String MethodName = "ApplyLayoutToNodeXLControl";

		this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "nodeXLControl", nodeXLControl);

		this.ArgumentChecker.CheckArgumentNotNegative(
			MethodName, "margin", margin);

		AssertValid();

		ILayout oLayout = null;
		IVertexDrawer oVertexDrawer = null;
		IEdgeDrawer oEdgeDrawer = null;

		Boolean bUseDefaultVertexDrawer = true;
		Boolean bUseDefaultEdgeDrawer = true;

		switch (m_eLayout)
		{
			case LayoutType.Circle:

				oLayout = new CircleLayout();
				break;

			case LayoutType.Spiral:

				oLayout = new SpiralLayout();
				break;

			case LayoutType.SinusoidHorizontal:

				oLayout = new SinusoidHorizontalLayout();
				break;

			case LayoutType.SinusoidVertical:

				oLayout = new SinusoidVerticalLayout();
				break;

			case LayoutType.Grid:

				oLayout = new GridLayout();
				break;

			case LayoutType.FruchtermanReingold:

				oLayout = new FruchtermanReingoldLayout();
				break;

			case LayoutType.Random:

				oLayout = new RandomLayout();
				break;

			case LayoutType.Sugiyama:

				oLayout = new SugiyamaLayout();

				bUseDefaultVertexDrawer = false;
				bUseDefaultEdgeDrawer = false;

				oVertexDrawer = new SugiyamaVertexDrawer();
				oEdgeDrawer = new SugiyamaEdgeDrawer();

				break;

			default:

				Debug.Assert(false);
				break;
		}

		if (bUseDefaultVertexDrawer)
		{
			oVertexDrawer = new PerVertexWithLabelDrawer();
		}

		if (bUseDefaultEdgeDrawer)
		{
			oEdgeDrawer = new PerEdgeWithLabelDrawer();
		}

		Debug.Assert(oLayout != null);
		Debug.Assert(oVertexDrawer != null);
		Debug.Assert(oEdgeDrawer != null);

		oLayout.Margin = margin;

		nodeXLControl.Layout = oLayout;
		nodeXLControl.VertexDrawer = oVertexDrawer;
		nodeXLControl.EdgeDrawer = oEdgeDrawer;
    }

	//*************************************************************************
	//	Event: LayoutChanged
	//
	/// <summary>
	///	Occurs when the <see cref="Layout" /> property changes.
	/// </summary>
	///
    /// <seealso cref="Layout" />
	//*************************************************************************

	public event EventHandler LayoutChanged;


	//*************************************************************************
	//	Property: ClassName
	//
	/// <summary>
	/// Gets the full name of the class.
	/// </summary>
	///
	/// <value>
	/// The full name of the class, suitable for use in error messages.
	/// </value>
	//*************************************************************************

	protected String
	ClassName
	{
		get
		{
			return (this.GetType().FullName);
		}
	}

	//*************************************************************************
	//	Property: ArgumentChecker
	//
	/// <summary>
	/// Gets a new initialized <see cref="ArgumentChecker" /> object.
	/// </summary>
	///
	/// <value>
	/// A new initialized <see cref="ArgumentChecker" /> object.
	/// </value>
	///
	/// <remarks>
	/// The returned object can be used to check the validity of property
	/// values and method parameters.
	/// </remarks>
	//*************************************************************************

	internal ArgumentChecker
	ArgumentChecker
	{
		get
		{
			return ( new ArgumentChecker(this.ClassName) );
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

    public virtual void
    AssertValid()
    {
		// m_eLayout
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Current layout type.

	protected LayoutType m_eLayout;
}

}
