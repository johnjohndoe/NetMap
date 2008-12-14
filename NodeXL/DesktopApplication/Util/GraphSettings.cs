
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Configuration;
using System.Diagnostics;
using Microsoft.NodeXL.Visualization;

namespace Microsoft.NodeXL.DesktopApplication
{
//*****************************************************************************
//  Class: GraphSettings
//
/// <summary>
/// Stores the user's settings that determine how graphs are drawn.
/// </summary>
//*****************************************************************************

[ SettingsGroupNameAttribute("Graph") ]

public class GraphSettings : ApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: GraphSettings()
    //
    /// <summary>
    /// Initializes a new instance of the GraphSettings class.
    /// </summary>
    //*************************************************************************

    public GraphSettings()
    {
		// (Do nothing.)

		AssertValid();
    }

    //*************************************************************************
    //  Property: BackColor
    //
    /// <summary>
    /// Gets or sets the graph's background color.
    /// </summary>
    ///
    /// <value>
	/// The graph's background color, as a Color.  The default value is
	/// <see cref="SystemColors.Window" />.
    /// </value>
    //*************************************************************************

    public Color
	BackColor
    {
        get
        {
            AssertValid();

			return ( Color.FromArgb(this.BackColorAsInt32) );
        }

        set
        {
			this.BackColorAsInt32 = value.ToArgb();

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: BackColorAsInt32
    //
    /// <summary>
    /// Gets or sets the graph's background color.
    /// </summary>
    ///
    /// <value>
	/// The graph's background color, as an Int32.  The default value is <see
	/// cref="SystemColors.Window" /> converted to an Int32.
    /// </value>
	///
	/// <remarks>
	/// An Int32 is used instead of a Color because Color can't be serialized.
	/// The Int32 can be converted to and from a Color using Color.FromArgb()
	/// and Color.ToArgb().
	/// </remarks>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("-1") ]

    public Int32
	BackColorAsInt32
    {
        get
        {
            AssertValid();

			return ( (Int32)this[BackColorAsInt32Key] );
        }

        set
        {
            this[BackColorAsInt32Key] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Margin
    //
    /// <summary>
	///	Gets or sets the margin to subtract from each edge of the graph
	/// rectangle before laying out the graph.
    /// </summary>
    ///
    /// <value>
	/// The margin to subtract from each edge.  Must be greater than or equal
	/// to zero.  The default value is 6.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("6") ]

    public Int32
    Margin
    {
        get
		{
			AssertValid();

			return ( (Int32)this[MarginKey] );
		}

		set
		{
			Debug.Assert(value >= 0);

            this[MarginKey] = value;

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: EdgeWidth
    //
    /// <summary>
    /// Gets or sets the width of unselected edges.
    /// </summary>
    ///
    /// <value>
	/// The width of unselected edges, as an Int32.  Must be between
	/// EdgeDrawer.MinimumWidth and EdgeDrawer.MaximumWidth.  The default value
	/// is 1.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("1") ]

    public Int32
	EdgeWidth
    {
        get
        {
            AssertValid();

			return ( (Int32)this[EdgeWidthKey] );
        }

        set
        {
			Debug.Assert(value >= EdgeDrawer.MinimumWidth);
			Debug.Assert(value <= EdgeDrawer.MaximumWidth);

            this[EdgeWidthKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SelectedEdgeWidth
    //
    /// <summary>
    /// Gets or sets the width of selected edges.
    /// </summary>
    ///
    /// <value>
	/// The width of selected edges, as an Int32.  Must be between
	/// EdgeDrawer.MinimumWidth and EdgeDrawer.MaximumWidth.  The default value
	/// is 2.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("2") ]

    public Int32
	SelectedEdgeWidth
    {
        get
        {
            AssertValid();

			return ( (Int32)this[SelectedEdgeWidthKey] );
        }

        set
        {
			Debug.Assert(value >= EdgeDrawer.MinimumWidth);
			Debug.Assert(value <= EdgeDrawer.MaximumWidth);

            this[SelectedEdgeWidthKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeColor
    //
    /// <summary>
    /// Gets or sets the color of unselected edges.
    /// </summary>
    ///
    /// <value>
	/// The color of unselected edges, as a Color.  The default value is <see
	/// cref="SystemColors.WindowText" />.
    /// </value>
    //*************************************************************************

    public Color
	EdgeColor
    {
        get
        {
            AssertValid();

			return ( Color.FromArgb(this.EdgeColorAsInt32) );
        }

        set
        {
			this.EdgeColorAsInt32 = value.ToArgb();

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeColorAsInt32
    //
    /// <summary>
    /// Gets or sets the color of unselected edges.
    /// </summary>
    ///
    /// <value>
	/// The color of unselected edges, as an Int32.  The default value is <see
	/// cref="SystemColors.WindowText" /> converted to an Int32.
    /// </value>
	///
	/// <remarks>
	/// An Int32 is used instead of a Color because Color can't be serialized.
	/// The Int32 can be converted to and from a Color using Color.FromArgb()
	/// and Color.ToArgb().
	/// </remarks>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("-16777216") ]

    public Int32
	EdgeColorAsInt32
    {
        get
        {
            AssertValid();

			return ( (Int32)this[EdgeColorAsInt32Key] );
        }

        set
        {
            this[EdgeColorAsInt32Key] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SelectedEdgeColor
    //
    /// <summary>
    /// Gets or sets the color of selected edges.
    /// </summary>
    ///
    /// <value>
	/// The color of selected edges, as a Color.  The default value is <see
	/// cref="Color.Red" />.
    /// </value>
    //*************************************************************************

    public Color
	SelectedEdgeColor
    {
        get
        {
            AssertValid();

			return ( Color.FromArgb(this.SelectedEdgeColorAsInt32) );
        }

        set
        {
			this.SelectedEdgeColorAsInt32 = value.ToArgb();

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SelectedEdgeColorAsInt32
    //
    /// <summary>
    /// Gets or sets the color of selected edges.
    /// </summary>
    ///
    /// <value>
	/// The color of selected edges, as an Int32.  The default value is <see
	/// cref="Color.Red" /> converted to an Int32.
    /// </value>
	///
	/// <remarks>
	/// An Int32 is used instead of a Color because Color can't be serialized.
	/// The Int32 can be converted to and from a Color using Color.FromArgb()
	/// and Color.ToArgb().
	/// </remarks>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("-65536") ]

    public Int32
	SelectedEdgeColorAsInt32
    {
        get
        {
            AssertValid();

			return ( (Int32)this[SelectedEdgeColorAsInt32Key] );
        }

        set
        {
            this[SelectedEdgeColorAsInt32Key] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: RelativeArrowSize
    //
    /// <summary>
    /// Gets or sets the relative size of arrow heads on directed edges.
    /// </summary>
    ///
    /// <value>
	///	The relative size of arrow heads, as a <see cref="Single" />.  Must be
	/// between <see cref="EdgeDrawer.MinimumRelativeArrowSize" /> and <see
	/// cref="EdgeDrawer.MaximumRelativeArrowSize" />, inclusive.  The default
	/// value is 3.
    /// </value>
	///
	/// <remarks>
	/// The value is relative to <see cref="EdgeWidth" /> and <see
	/// cref="SelectedEdgeWidth" />.  If the width or selected width is
	/// increased, the arrow size on unselected or selected edges is increased
	/// proportionally.
	/// </remarks>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("3") ]

    public Single
    RelativeArrowSize
    {
        get
		{
			AssertValid();

			return ( (Single)this[RelativeArrowSizeKey] );
		}

		set
		{
			Debug.Assert(value >= EdgeDrawer.MinimumRelativeArrowSize);
			Debug.Assert(value <= EdgeDrawer.MaximumRelativeArrowSize);

            this[RelativeArrowSizeKey] = value;

            AssertValid();
		}
    }

    //*************************************************************************
    //  Property: VertexShape
    //
    /// <summary>
    /// Gets or sets the shape of the vertices.
    /// </summary>
    ///
    /// <value>
	///	The shape of the vertices, as a <see
	/// cref="VertexDrawer.VertexShape" />.  The default value is <see
	/// cref="VertexDrawer.VertexShape.Disk" />.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("Disk") ]

    public VertexDrawer.VertexShape
    VertexShape
    {
        get
		{
			AssertValid();

			return ( (VertexDrawer.VertexShape)this[VertexShapeKey] );
		}

		set
		{
            this[VertexShapeKey] = value;

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: VertexRadius
    //
    /// <summary>
    /// Gets or sets the radius of the vertices.
    /// </summary>
    ///
    /// <value>
	/// The radius of the vertices, as a Single.  Must be between
	/// VertexDrawer.MinimumRadius and VertexDrawer.MaximumRadius.  The default
	/// value is 3.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("3") ]

    public Single
	VertexRadius
    {
        get
        {
            AssertValid();

			return ( (Single)this[VertexRadiusKey] );
        }

        set
        {
			Debug.Assert(value >= VertexDrawer.MinimumRadius);
			Debug.Assert(value <= VertexDrawer.MaximumRadius);

            this[VertexRadiusKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexColor
    //
    /// <summary>
    /// Gets or sets the color of unselected vertices.
    /// </summary>
    ///
    /// <value>
	/// The color of unselected vertices, as a Color.  The default value is
	/// <see cref="SystemColors.WindowText" />.
    /// </value>
    //*************************************************************************

    public Color
	VertexColor
    {
        get
        {
            AssertValid();

			return ( Color.FromArgb(this.VertexColorAsInt32) );
        }

        set
        {
			this.VertexColorAsInt32 = value.ToArgb();

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexColorAsInt32
    //
    /// <summary>
    /// Gets or sets the color of unselected vertices.
    /// </summary>
    ///
    /// <value>
	/// The color of unselected vertices, as an Int32.  The default value is
	/// <see cref="SystemColors.WindowText" /> converted to an Int32.
    /// </value>
	///
	/// <remarks>
	/// An Int32 is used instead of a Color because Color can't be serialized.
	/// The Int32 can be converted to and from a Color using Color.FromArgb()
	/// and Color.ToArgb().
	/// </remarks>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("-16777216") ]

    public Int32
	VertexColorAsInt32
    {
        get
        {
            AssertValid();

			return ( (Int32)this[VertexColorAsInt32Key] );
        }

        set
        {
            this[VertexColorAsInt32Key] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SelectedVertexColor
    //
    /// <summary>
    /// Gets or sets the color of the selected vertex.
    /// </summary>
    ///
    /// <value>
	/// The color of the selected vertex, as a Color.  The default value is
	/// <see cref="Color.Red" />.
    /// </value>
    //*************************************************************************

    public Color
	SelectedVertexColor
    {
        get
        {
            AssertValid();

			return ( Color.FromArgb(this.SelectedVertexColorAsInt32) );
        }

        set
        {
			this.SelectedVertexColorAsInt32 = value.ToArgb();

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SelectedVertexColorAsInt32
    //
    /// <summary>
    /// Gets or sets the color of the selected vertex.
    /// </summary>
    ///
    /// <value>
	/// The color of the selected vertex, as an Int32.  The default value is
	/// <see cref="Color.Red" /> converted to an Int32.
    /// </value>
	///
	/// <remarks>
	/// An Int32 is used instead of a Color because Color can't be serialized.
	/// The Int32 can be converted to and from a Color using Color.FromArgb()
	/// and Color.ToArgb().
	/// </remarks>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("-65536") ]

    public Int32
	SelectedVertexColorAsInt32
    {
        get
        {
            AssertValid();

			return ( (Int32)this[SelectedVertexColorAsInt32Key] );
        }

        set
        {
            this[SelectedVertexColorAsInt32Key] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: TransferToNodeXLControl()
    //
    /// <summary>
    /// Transfers the settings to a <see cref="NodeXLControl" />.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
    /// Control to transfer the settings to.
    /// </param>
    //*************************************************************************

    public void
    TransferToNodeXLControl
    (
		NodeXLControl nodeXLControl
    )
    {
		Debug.Assert(nodeXLControl != null);
		AssertValid();

		const String MethodName = "TransferToNodeXLControl";

		IEdgeDrawer oEdgeDrawer1 = nodeXLControl.EdgeDrawer;

		if ( !(oEdgeDrawer1 is EdgeDrawer) )
		{
			throw new InvalidOperationException( String.Format(

				"{0}.{1}: NodeXLControl.EdgeDrawer was set to an edge drawer"
				+ " that is not supported."
				,
				this.ClassName,
				MethodName
				) );
		}

		IVertexDrawer oVertexDrawer1 = nodeXLControl.VertexDrawer;

		if ( !(oVertexDrawer1 is VertexDrawer) )
		{
			throw new InvalidOperationException( String.Format(

				"{0}.{1}: NodeXLControl.VertexDrawer was set to a vertex drawer"
				+ " that is not supported."
				,
				this.ClassName,
				MethodName
				) );
		}

		nodeXLControl.BackColor = this.BackColor;
		nodeXLControl.Layout.Margin = this.Margin;

		EdgeDrawer oEdgeDrawer = (EdgeDrawer)oEdgeDrawer1;
		VertexDrawer oVertexDrawer = (VertexDrawer)oVertexDrawer1;

		oEdgeDrawer.Width = this.EdgeWidth;
		oEdgeDrawer.SelectedWidth = this.SelectedEdgeWidth;

		oEdgeDrawer.Color = this.EdgeColor;
        oEdgeDrawer.SelectedColor = this.SelectedEdgeColor;

        oEdgeDrawer.RelativeArrowSize = this.RelativeArrowSize;

		oVertexDrawer.Shape = this.VertexShape;
		oVertexDrawer.Radius = this.VertexRadius;
		oVertexDrawer.Color = this.VertexColor;
		oVertexDrawer.SelectedColor = this.SelectedVertexColor;
    }

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
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    public void
    AssertValid()
    {
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	/// Name of the settings key for the BackColorAsInt32 property.

	protected const String BackColorAsInt32Key =
		"BackColorAsInt32";

	/// Name of the settings key for the Margin property.

	protected const String MarginKey =
		"Margin";

	/// Name of the settings key for the EdgeWidth property.

	protected const String EdgeWidthKey =
		"EdgeWidth";

	/// Name of the settings key for the SelectedEdgeWidth property.

	protected const String SelectedEdgeWidthKey =
		"SelectedEdgeWidth";

	/// Name of the settings key for the EdgeColorAsInt32 property.

	protected const String EdgeColorAsInt32Key =
		"EdgeColorAsInt32";

	/// Name of the settings key for the SelectedEdgeColorAsInt32 property.

	protected const String SelectedEdgeColorAsInt32Key =
		"SelectedEdgeColorAsInt32";

	/// Name of the settings key for the RelativeArrowSize property.

	protected const String RelativeArrowSizeKey =
		"RelativeArrowSize";

	/// Name of the settings key for the VertexShape property.

	protected const String VertexShapeKey =
		"VertexShape";

	/// Name of the settings key for the VertexRadius property.

	protected const String VertexRadiusKey =
		"VertexRadius";

	/// Name of the settings key for the VertexColorAsInt32 property.

	protected const String VertexColorAsInt32Key =
		"VertexColorAsInt32";

	/// Name of the settings key for the SelectedVertexColorAsInt32 property.

	protected const String SelectedVertexColorAsInt32Key =
		"SelectedVertexColorAsInt32";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
