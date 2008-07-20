
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Configuration;
using System.Diagnostics;
using Microsoft.NetMap.Visualization;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: GeneralUserSettings
//
/// <summary>
/// Stores the user's general settings.
/// </summary>
//*****************************************************************************

[ SettingsGroupNameAttribute("GeneralUserSettings") ]

public class GeneralUserSettings : ApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: GeneralUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the GeneralUserSettings class.
    /// </summary>
    //*************************************************************************

    public GeneralUserSettings()
    {
		// (Do nothing.)

		AssertValid();
    }

    //*************************************************************************
    //  Property: BackColor
    //
    /// <summary>
    /// Gets or sets the color of unselected vertices.
    /// </summary>
    ///
    /// <value>
	/// The color of unselected vertices, as a KnownColor.  The default value
	/// is Color.White.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("White") ]

    public KnownColor
	BackColor
    {
        get
        {
            AssertValid();

			return ( (KnownColor)this[BackColorKey] );
        }

        set
        {
			this[BackColorKey] = value;

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
	/// The width of unselected edges, as a Single.  Must be between
	/// EdgeWidthConverter.MinimumWidthWorkbook and
	/// EdgeWidthConverter.MaximumWidthConverter.  The default value
	/// is 1.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("1") ]

    public Single
	EdgeWidth
    {
        get
        {
            AssertValid();

			return ( (Single)this[EdgeWidthKey] );
        }

        set
        {
			Debug.Assert(value >= EdgeWidthConverter.MinimumWidthWorkbook);
			Debug.Assert(value <= EdgeWidthConverter.MaximumWidthWorkbook);

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
	/// The width of selected edges, as a Single.  Must be between
	/// EdgeWidthConverter.MinimumWidthWorkbook and
	/// EdgeWidthConverter.MaximumWidthWorkbook.  The default value is 2.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("2") ]

    public Single
	SelectedEdgeWidth
    {
        get
        {
            AssertValid();

			return ( (Single)this[SelectedEdgeWidthKey] );
        }

        set
        {
			Debug.Assert(value >= EdgeWidthConverter.MinimumWidthWorkbook);
			Debug.Assert(value <= EdgeWidthConverter.MaximumWidthWorkbook);

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
	/// The color of unselected edges, as a KnownColor.  The default value
	/// is Color.Black.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("Black") ]

    public KnownColor
	EdgeColor
    {
        get
        {
            AssertValid();

			return ( (KnownColor)this[EdgeColorKey] );
        }

        set
        {
			this[EdgeColorKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeAlpha
    //
    /// <summary>
    /// Gets or sets the alpha component of the color of unselected edges.
    /// </summary>
    ///
	/// <value>
    /// The alpha component of the color of unselected edges.  Must be between
	/// AlphaConverter.MinimumAlphaWorkbook and
	/// AlphaConverter.MaximumAlphaConverter.  The default value is 10.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("10") ]

    public Single
	EdgeAlpha
    {
        get
        {
            AssertValid();

			return ( (Single)this[EdgeAlphaKey] );
        }

        set
        {
			this[EdgeAlphaKey] = value;

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
	/// The color of selected edges, as a KnownColor.  The default value
	/// is Color.Red.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("Red") ]

    public KnownColor
	SelectedEdgeColor
    {
        get
        {
            AssertValid();

			return ( (KnownColor)this[SelectedEdgeColorKey] );
        }

        set
        {
			this[SelectedEdgeColorKey] = value;

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
	/// VertexRadiusConverter.MinimumRadiusWorkbook and
	/// VertexRadiusConverter.MaximumRadiusWorkbook.  The default value is 1.5.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("1.5") ]

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
			Debug.Assert(value >= VertexRadiusConverter.MinimumRadiusWorkbook);
			Debug.Assert(value <= VertexRadiusConverter.MaximumRadiusWorkbook);

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
	/// The color of unselected vertices, as a KnownColor.  The default value
	/// is Color.Black.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("Black") ]

    public KnownColor
	VertexColor
    {
        get
        {
            AssertValid();

			return ( (KnownColor)this[VertexColorKey] );
        }

        set
        {
			this[VertexColorKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexAlpha
    //
    /// <summary>
    /// Gets or sets the alpha component of the color of unselected vertices.
    /// </summary>
    ///
    /// <value>
    /// The alpha component of the color of unselected vertices.  Must be
	/// between AlphaConverter.MinimumAlphaWorkbook and
	/// AlphaConverter.MaximumAlphaConverter.  The default value is 10.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("10") ]

    public Single
	VertexAlpha
    {
        get
        {
            AssertValid();

			return ( (Single)this[VertexAlphaKey] );
        }

        set
        {
			this[VertexAlphaKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SelectedVertexColor
    //
    /// <summary>
    /// Gets or sets the color of selected vertices.
    /// </summary>
    ///
    /// <value>
	/// The color of selected vertices, as a KnownColor.  The default value
	/// is Color.Red.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("Red") ]

    public KnownColor
	SelectedVertexColor
    {
        get
        {
            AssertValid();

			return ( (KnownColor)this[SelectedVertexColorKey] );
        }

        set
        {
			this[SelectedVertexColorKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: AutoSelect
    //
    /// <summary>
	/// Gets or sets a flag that determines edge and vertex selection behavior.
    /// </summary>
    ///
    /// <value>
	/// If true, selecting a vertex in the graph or workbook automatically
	/// selects the vertex's incident edges, and selecting an edge in the
	/// workbook automatically selects the edge's adjacent vertices.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("true") ]

    public Boolean
    AutoSelect
    {
        get
		{
			AssertValid();

			return ( (Boolean)this[AutoSelectKey] );
		}

        set
		{
			this[AutoSelectKey] = value;

			AssertValid();
		}
    }

    //*************************************************************************
    //  Method: TransferToNetMapControl()
    //
    /// <summary>
    /// Transfers the settings to a <see cref="NetMapControl" />.
    /// </summary>
    ///
    /// <param name="netMapControl">
    /// Control to transfer the settings to.
    /// </param>
    //*************************************************************************

    public void
    TransferToNetMapControl
    (
		NetMapControl netMapControl
    )
    {
		Debug.Assert(netMapControl != null);
		AssertValid();

		const String MethodName = "TransferToNetMapControl";

		TransferToEdgeAndVertexDrawers(netMapControl.EdgeDrawer,
			netMapControl.VertexDrawer, MethodName);

		netMapControl.BackColor = Color.FromKnownColor(this.BackColor);

		netMapControl.Layout.Margin = this.Margin;

		netMapControl.MouseSelectionMode = this.AutoSelect ?
			MouseSelectionMode.SelectVertexAndIncidentEdges :
			MouseSelectionMode.SelectVertexOnly;
    }

    //*************************************************************************
    //  Method: TransferToGraphDrawer()
    //
    /// <summary>
    /// Transfers the settings to a <see cref="GraphDrawer" />.
    /// </summary>
    ///
    /// <param name="graphDrawer">
    /// Graph drawer to transfer the settings to.
    /// </param>
    //*************************************************************************

    public void
    TransferToGraphDrawer
    (
		GraphDrawer graphDrawer
    )
    {
		Debug.Assert(graphDrawer != null);
		AssertValid();

		const String MethodName = "TransferToGraphDrawer";

		TransferToEdgeAndVertexDrawers(graphDrawer.EdgeDrawer,
			graphDrawer.VertexDrawer, MethodName);

		graphDrawer.BackColor = Color.FromKnownColor(this.BackColor);

		graphDrawer.Layout.Margin = this.Margin;
    }

    //*************************************************************************
    //  Method: TransferToEdgeAndVertexDrawers()
    //
    /// <summary>
    /// Transfers the settings to an <see cref="EdgeDrawer" /> and <see
	/// cref="VertexDrawer" />.
    /// </summary>
    ///
    /// <param name="oEdgeDrawer">
    /// IEdgeDrawer to transfer the settings to.  If not an EdgeDrawer object,
	/// an exception is thrown.
    /// </param>
    ///
    /// <param name="oVertexDrawer">
    /// IVertexDrawer to transfer the settings to.  If not a VertexDrawer
	/// object, an exception is thrown.
    /// </param>
    ///
    /// <param name="sCallingMethodName">
    /// Name of the calling method.  Gets used in exception messages.
    /// </param>
    //*************************************************************************

    protected void
    TransferToEdgeAndVertexDrawers
    (
		IEdgeDrawer oEdgeDrawer,
		IVertexDrawer oVertexDrawer,
		String sCallingMethodName
    )
    {
		Debug.Assert(oEdgeDrawer != null);
		Debug.Assert(oVertexDrawer != null);
		Debug.Assert( !String.IsNullOrEmpty(sCallingMethodName) );
		AssertValid();

		if ( !(oEdgeDrawer is EdgeDrawer) )
		{
			throw new InvalidOperationException( String.Format(

				"{0}.{1}: The EdgeDrawer was set to an edge drawer that is not"
				+ " supported."
				,
				this.ClassName,
				sCallingMethodName
				) );
		}

		if ( !(oVertexDrawer is VertexDrawer) )
		{
			throw new InvalidOperationException( String.Format(

				"{0}.{1}: VertexDrawer was set to a vertex drawer that is not"
				+ " supported."
				,
				this.ClassName,
				sCallingMethodName
				) );
		}


		// Transfer settings to the EdgeDrawer.

		EdgeDrawer oEdgeDrawer2 = (EdgeDrawer)oEdgeDrawer;

		EdgeWidthConverter oEdgeWidthConverter = new EdgeWidthConverter();
		AlphaConverter oAlphaConverter = new AlphaConverter();

		oEdgeDrawer2.Width =
			oEdgeWidthConverter.WorkbookToGraph(this.EdgeWidth);

		oEdgeDrawer2.SelectedWidth = oEdgeWidthConverter.WorkbookToGraph(
			this.SelectedEdgeWidth);

		oEdgeDrawer2.Color = Color.FromArgb(
			oAlphaConverter.WorkbookToGraph(this.EdgeAlpha),
			Color.FromKnownColor(this.EdgeColor)
			);

		oEdgeDrawer2.SelectedColor =
			Color.FromKnownColor(this.SelectedEdgeColor);

        oEdgeDrawer2.RelativeArrowSize = this.RelativeArrowSize;


		// Transfer settings to the VertexDrawer.

		VertexDrawer oVertexDrawer2 = (VertexDrawer)oVertexDrawer;

		oVertexDrawer2.Shape = this.VertexShape;

		oVertexDrawer2.Radius = ( new VertexRadiusConverter() ).
			WorkbookToGraph(this.VertexRadius);

		oVertexDrawer2.Color = Color.FromArgb(
			oAlphaConverter.WorkbookToGraph(this.VertexAlpha),
			Color.FromKnownColor(this.VertexColor)
			);

		oVertexDrawer2.SelectedColor =
			Color.FromKnownColor(this.SelectedVertexColor);
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

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	/// Name of the settings key for the BackColor property.

	protected const String BackColorKey =
		"BackColor";

	/// Name of the settings key for the Margin property.

	protected const String MarginKey =
		"Margin";

	/// Name of the settings key for the EdgeColor property.

	protected const String EdgeColorKey =
		"EdgeColor";

	/// Name of the settings key for the EdgeAlpha property.

	protected const String EdgeAlphaKey =
		"EdgeAlpha";

	/// Name of the settings key for the SelectedEdgeColor property.

	protected const String SelectedEdgeColorKey =
		"SelectedEdgeColor";
	/// Name of the settings key for the EdgeWidth property.

	protected const String EdgeWidthKey =
		"EdgeWidth";

	/// Name of the settings key for the SelectedEdgeWidth property.

	protected const String SelectedEdgeWidthKey =
		"SelectedEdgeWidth";

	/// Name of the settings key for the RelativeArrowSize property.

	protected const String RelativeArrowSizeKey =
		"RelativeArrowSize";

	/// Name of the settings key for the VertexShape property.

	protected const String VertexShapeKey =
		"VertexShape";

	/// Name of the settings key for the VertexRadius property.

	protected const String VertexRadiusKey =
		"VertexRadius";

	/// Name of the settings key for the VertexColor property.

	protected const String VertexColorKey =
		"VertexColor";

	/// Name of the settings key for the VertexAlpha property.

	protected const String VertexAlphaKey =
		"VertexAlpha";

	/// Name of the settings key for the SelectedVertexColor property.

	protected const String SelectedVertexColorKey =
		"SelectedVertexColor";

	/// Name of the settings key for the AutoSelect property.

	protected const String AutoSelectKey =
		"AutoSelect";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
