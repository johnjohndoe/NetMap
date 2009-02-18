
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: VertexAndEdgeDrawerBase
//
/// <summary>
/// Base class for classes that draw vertices and edges.
/// </summary>
//*****************************************************************************

public class VertexAndEdgeDrawerBase : DrawerBase
{
    //*************************************************************************
    //  Constructor: VertexAndEdgeDrawerBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="VertexAndEdgeDrawerBase" /> class.
    /// </summary>
    //*************************************************************************

    public VertexAndEdgeDrawerBase()
    {
        m_bUseSelection = true;
        m_oColor = SystemColors.WindowTextColor;
        m_oSelectedColor = SystemColors.HighlightColor;
        m_btFilteredAlpha = 10;

        CreateDrawingObjects();

        // AssertValid();
    }

    //*************************************************************************
    //  Property: UseSelection
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the selected state of a vertex
    /// or edge should be used.
    /// </summary>
    ///
    /// <value>
    /// If true, a vertex or edge is drawn using either <see cref="Color" /> or
    /// <see cref="SelectedColor" />, depending on whether the vertex or edge
    /// has been marked as selected with the <see
    /// cref="ReservedMetadataKeys.IsSelected" /> key.  If false,
    /// <see cref="Color" /> is used regardless of whether the vertex or edge
    /// has been marked as selected.
    /// </value>
    //*************************************************************************

    public Boolean
    UseSelection
    {
        get
        {
            AssertValid();

            return (m_bUseSelection);
        }

        set
        {
            if (m_bUseSelection == value)
            {
                return;
            }

            m_bUseSelection = value;

            FireRedrawRequired();

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Color
    //
    /// <summary>
    /// Gets or sets the default color of unselected vertices or edges.
    /// </summary>
    ///
    /// <value>
    /// The default color of unselected vertices or edges, as a <see
    /// cref="Color" />.  The default value is <see
    /// cref="SystemColors.WindowTextColor" />.
    /// </value>
    ///
    /// <remarks>
    /// See <see cref="UseSelection" /> for details on selected vs. unselected
    /// vertices and edges.
    ///
    /// <para>
    /// The default color of an unselected vertex or edge can be overridden by
    /// setting the <see cref="ReservedMetadataKeys.PerColor" /> key on the
    /// vertex or edge.  The key's value can be of type System.Drawing.Color or
    /// System.Windows.Media.Color.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="SelectedColor" />
    //*************************************************************************

    public Color
    Color
    {
        get
        {
            AssertValid();

            return (m_oColor);
        }

        set
        {
            if (m_oColor == value)
            {
                return;
            }

            m_oColor = value;

            CreateDrawingObjects();

            FireRedrawRequired();

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SelectedColor
    //
    /// <summary>
    /// Gets or sets the color of selected vertices or edges.
    /// </summary>
    ///
    /// <value>
    /// The color of selected vertices or edges, as a <see cref="Color" />.
    /// The default value is <see cref="SystemColors.HighlightColor" />.
    /// </value>
    ///
    /// <remarks>
    /// See <see cref="UseSelection" /> for details on selected vs. unselected
    /// vertices or edges.
    ///
    /// <para>
    /// The color of selected vertices and edges cannot be overridden on a
    /// per-vertex or per-edge basis.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="Color" />
    //*************************************************************************

    public Color
    SelectedColor
    {
        get
        {
            AssertValid();

            return (m_oSelectedColor);
        }

        set
        {
            if (m_oSelectedColor == value)
            {
                return;
            }

            m_oSelectedColor = value;

            CreateDrawingObjects();

            FireRedrawRequired();

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: FilteredAlpha
    //
    /// <summary>
    /// Gets or sets the alpha value to use for vertices and edges that are
    /// filtered.
    /// </summary>
    ///
    /// <value>
    /// The alpha value to use for vertices and edges that have a <see
    /// cref="ReservedMetadataKeys.Visibility" /> value of <see
    /// cref="VisibilityKeyValue.Filtered" />.  Must be between 0 (invisible)
    /// and 255 (opaque).  The default value is 10.
    /// </value>
    //*************************************************************************

    public Byte
    FilteredAlpha
    {
        get
        {
            AssertValid();

            return (m_btFilteredAlpha);
        }

        set
        {
            const String PropertyName = "FilteredAlpha";

            if (m_btFilteredAlpha == value)
            {
                return;
            }

            this.ArgumentChecker.CheckPropertyInRange(PropertyName, value,
                0, 255);

            m_btFilteredAlpha = value;

            FireRedrawRequired();

            AssertValid();
        }
    }
    //*************************************************************************
    //  Method: GetVisibility()
    //
    /// <summary>
    /// Gets the visibility of a vertex or edge.
    /// </summary>
    ///
    /// <param name="oVertexOrEdge">
    /// The vertex or edge.
    /// </param>
    ///
    /// <returns>
    /// If the <see cref="ReservedMetadataKeys.Visibility" /> key is present on
    /// <paramref name="oVertexOrEdge" />, the key's value is returned as a
    /// <see cref="VisibilityKeyValue" />.  Otherwise, <see
    /// cref="VisibilityKeyValue.Visible" /> is returned.
    /// </returns>
    //*************************************************************************

    protected VisibilityKeyValue
    GetVisibility
    (
        IMetadataProvider oVertexOrEdge
    )
    {
        Debug.Assert(oVertexOrEdge != null);
        AssertValid();

        Object oVisibilityKeyValue;

        if ( oVertexOrEdge.TryGetValue(ReservedMetadataKeys.Visibility,
            typeof(VisibilityKeyValue), out oVisibilityKeyValue) )
        {
            return ( (VisibilityKeyValue)oVisibilityKeyValue );
        }

        return (VisibilityKeyValue.Visible);
    }

    //*************************************************************************
    //  Method: GetDrawAsSelected()
    //
    /// <summary>
    /// Gets a flag indicating whether a vertex or edge should be drawn as
    /// selected.
    /// </summary>
    ///
    /// <param name="oVertexOrEdge">
    /// The vertex or edge.
    /// </param>
    ///
    /// <returns>
    /// true if the edge or vertex should be drawn as selected.
    /// </returns>
    //*************************************************************************

    protected Boolean
    GetDrawAsSelected
    (
        IMetadataProvider oVertexOrEdge
    )
    {
        Debug.Assert(oVertexOrEdge != null);
        AssertValid();

        return ( m_bUseSelection &&
            oVertexOrEdge.ContainsKey(ReservedMetadataKeys.IsSelected) );
    }

    //*************************************************************************
    //  Method: GetColor()
    //
    /// <overloads>
    /// Gets the color of a vertex or edge.
    /// </overloads>
    ///
    /// <summary>
    /// Gets the color of a vertex or edge.
    /// </summary>
    ///
    /// <param name="oVertexOrEdge">
    /// The vertex or edge to get the color for.
    /// </param>
    ///
    /// <param name="eVisibility">
    /// The visibility of the vertex or edge.  This can be obtained with <see
    /// cref="GetVisibility" />.
    /// </param>
    ///
    /// <param name="bDrawAsSelected">
    /// true if <paramref name="oVertexOrEdge" /> should be drawn as selected.
    /// </param>
    ///
    /// <returns>
    /// The color of the vertex or edge.  This includes any per-vertex or
    /// per-edge alpha value specified on the vertex or edge, along with the
    /// visibility specified by <paramref name="eVisibility" />.
    /// </returns>
    //*************************************************************************

    protected Color
    GetColor
    (
        IMetadataProvider oVertexOrEdge,
        VisibilityKeyValue eVisibility,
        Boolean bDrawAsSelected
    )
    {
        Debug.Assert(oVertexOrEdge != null);
        AssertValid();

        if (bDrawAsSelected)
        {
            return (m_oSelectedColor);
        }

        return ( GetColor(oVertexOrEdge, eVisibility,
            ReservedMetadataKeys.PerColor, m_oColor, true) );
    }

    //*************************************************************************
    //  Method: GetColor()
    //
    /// <summary>
    /// Gets the color of a vertex or edge given a default color and a metadata
    /// key.
    /// </summary>
    ///
    /// <param name="oVertexOrEdge">
    /// The vertex or edge to get the color for.
    /// </param>
    ///
    /// <param name="eVisibility">
    /// The visibility of the vertex or edge.  This can be obtained with <see
    /// cref="GetVisibility" />.  Not used if <paramref name="bApplyAlpha" />
    /// is false.
    /// </param>
    ///
    /// <param name="sKey">
    /// The metadata key to check for a per-vertex or per-edge color.
    /// </param>
    ///
    /// <param name="oDefaultColor">
    /// The default color to use if <paramref name="sKey" /> isn't specified
    /// on the vertex or edge.
    /// </param>
    ///
    /// <param name="bApplyAlpha">
    /// If true, <paramref name="eVisibility" /> and any per-vertex or per-edge
    /// alpha value is applied to the color.
    /// </param>
    ///
    /// <returns>
    /// The color of the vertex or edge.  If <paramref name="bApplyAlpha" /> is
    /// true, this includes any per-vertex or per-edge alpha value specified on
    /// the vertex or edge, along with the visibility specified by <paramref
    /// name="eVisibility" />.
    /// </returns>
    //*************************************************************************

    protected Color
    GetColor
    (
        IMetadataProvider oVertexOrEdge,
        VisibilityKeyValue eVisibility,
        String sKey,
        Color oDefaultColor,
        Boolean bApplyAlpha
    )
    {
        Debug.Assert(oVertexOrEdge != null);
        Debug.Assert( !String.IsNullOrEmpty(sKey) );
        AssertValid();

		Byte btDefaultAlpha = oDefaultColor.A;

        // Start with the default color.

        Color oColor = oDefaultColor;

        // Check for a per-vertex or per-edge color.

        Color oPerColor;

        if ( TryGetColorValue(oVertexOrEdge, sKey, out oPerColor) )
        {
            oColor = oPerColor;
			oColor.A = btDefaultAlpha;
        }

        if (bApplyAlpha)
        {
            // Apply the vertex or edge's alpha.

            oColor.A = GetAlpha(oVertexOrEdge, eVisibility, oColor.A);
        }

        return (oColor);
    }

    //*************************************************************************
    //  Method: GetAlpha()
    //
    /// <summary>
    /// Get the alpha value to use for a vertex or edge.
    /// </summary>
    ///
    /// <param name="oVertexOrEdge">
    /// The vertex or edge to get the alpha value for.
    /// </param>
    ///
    /// <param name="eVisibility">
    /// The visibility of the vertex or edge.  This can be obtained with <see
    /// cref="GetVisibility" />.
    /// </param>
    ///
    /// <param name="btDefaultAlpha">
    /// The alpha value to return in the vertex or edge is visible and has no
    /// per-vertex or per-edge alpha.
    /// </param>
    ///
    /// <returns>
    /// The alpha value to use, between 0 (transparent) and 255 (opaque).
    /// </returns>
    //*************************************************************************

    protected Byte
    GetAlpha
    (
        IMetadataProvider oVertexOrEdge,
        VisibilityKeyValue eVisibility,
        Byte btDefaultAlpha
    )
    {
        Debug.Assert(oVertexOrEdge != null);
        AssertValid();

        if (eVisibility == VisibilityKeyValue.Filtered)
        {
            // The vertex or edge is filtered.

            return (m_btFilteredAlpha);
        }

        // Check for a per-vertex or per-edge alpha.

        Object oPerAlphaAsObject;

        if ( oVertexOrEdge.TryGetValue(ReservedMetadataKeys.PerAlpha,
            typeof(Byte), out oPerAlphaAsObject) )
        {
            Byte btPerAlpha = (Byte)oPerAlphaAsObject;

            // The following test isn't necessary (a Byte can't be less than 0
            // or greater than 255), but the PerAlpha value type has been
            // changed several times and this code is left here as a reminder
            // to check limits if the type changes again.

            if (btPerAlpha < 0 || btPerAlpha > 255)
            {
                Debug.Assert(oVertexOrEdge is IIdentityProvider);

                throw new FormatException( String.Format(

                    "{0}: The {1} with the ID {2} has an out-of-range"
                    + " {3} value.  Valid values are between 0 and 255."
                    ,
                    this.ClassName,
                    (oVertexOrEdge is IVertex) ? "vertex" : "edge",
                    ( (IIdentityProvider)oVertexOrEdge ).ID,
                    "ReservedMetadataKeys.PerAlpha"
                    ) );
            }

            return (btPerAlpha);
        }

        return (btDefaultAlpha);
    }

    //*************************************************************************
    //  Method: TryGetColorValue()
    //
    /// <summary>
    /// Attempts to get a color from a vertex or edge's metadata.
    /// </summary>
    ///
    /// <param name="oVertexOrEdge">
    /// The vertex or edge to get the color for.
    /// </param>
    ///
    /// <param name="sKey">
    /// The color's key.
    /// </param>
    ///
    /// <param name="oColor">
    /// Where the color gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the vertex or edge contains the specified color key.
    /// </returns>
    ///
    /// <remarks>
    /// The value of the specified key can be of type
    /// System.Windows.Media.Color or System.Drawing.Color.  If it is of type
    /// System.Drawing.Color, it gets converted to type
    /// System.Windows.Media.Color.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    TryGetColorValue
    (
        IMetadataProvider oVertexOrEdge,
        String sKey,
        out Color oColor
    )
    {
        Debug.Assert(oVertexOrEdge != null);
        AssertValid();

        oColor = Color.FromRgb(0, 0, 0);

        Object oColorAsObject;

        if ( !oVertexOrEdge.TryGetValue(sKey, out oColorAsObject) )
        {
            return (false);
        }

        if ( typeof(System.Windows.Media.Color).IsInstanceOfType(
            oColorAsObject) )
        {
            oColor = (System.Windows.Media.Color)oColorAsObject;
        }
        else if ( typeof(System.Drawing.Color).IsInstanceOfType(
            oColorAsObject) )
        {
            oColor = WpfGraphicsUtil.ColorToWpfColor(
                (System.Drawing.Color)oColorAsObject );
        }
        else
        {
            throw new InvalidOperationException( String.Format(

                "The vertex or edge value with the key \"{0}\" is of type"
                + " {1}.  The expected type is either"
                + " System.Windows.Media.Color or System.Drawing.Color."
                ,
                sKey,
                oColorAsObject.GetType().FullName
                ) );
        }

        return (true);
    }


    //*************************************************************************
    //  Method: CreateDrawingObjects()
    //
    /// <summary>
    /// Creates a set of drawing objects for use by the derived class.
    /// </summary>
    //*************************************************************************

    protected void
    CreateDrawingObjects()
    {
        // AssertValid();

        m_oDefaultBrush = CreateFrozenSolidColorBrush(m_oColor);
        m_oDefaultPen = CreateFrozenPen(m_oDefaultBrush, DefaultPenThickness);
    }

    //*************************************************************************
    //  Method: GetBrush()
    //
    /// <summary>
    /// Gets a SolidColorBrush to use to draw a vertex or edge.
    /// </summary>
    ///
    /// <param name="oColor">
    /// The vertex or edge color.
    /// </param>
    ///
    /// <returns>
    /// A SolidColorBrush to use to draw a vertex or edge.
    /// </returns>
    //*************************************************************************

    protected SolidColorBrush
    GetBrush
    (
        Color oColor
    )
    {
        AssertValid();

        if (oColor == m_oDefaultBrush.Color)
        {
            return (m_oDefaultBrush);
        }

        return ( CreateFrozenSolidColorBrush(oColor) );
    }

    //*************************************************************************
    //  Method: GetPen()
    //
    /// <summary>
    /// Gets a pen to use to draw a vertex or edge.
    /// </summary>
    ///
    /// <param name="oColor">
    /// The vertex or edge color.
    /// </param>
    ///
    /// <param name="dThickness">
    /// The pen thickness.
    /// </param>
    ///
    /// <returns>
    /// A pen to use to draw a vertex or edge.
    /// </returns>
    //*************************************************************************

    protected Pen
    GetPen
    (
        Color oColor,
        Double dThickness
    )
    {
        Debug.Assert(dThickness > 0);
        AssertValid();

        Debug.Assert(m_oDefaultPen.Brush is SolidColorBrush);

        if (oColor == ( (SolidColorBrush)m_oDefaultPen.Brush ).Color &&
            dThickness == m_oDefaultPen.Thickness)
        {
            return (m_oDefaultPen);
        }

        return ( CreateFrozenPen( GetBrush(oColor), dThickness) );
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
        base.AssertValid();

        // m_bUseSelection
        // m_oColor
        // m_oSelectedColor
        Debug.Assert(m_btFilteredAlpha >= 0);
        Debug.Assert(m_btFilteredAlpha <= 255);
        Debug.Assert(m_oDefaultBrush != null);
        Debug.Assert(m_oDefaultPen != null);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Default pen thickness.

    protected const Double DefaultPenThickness = 1;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// true if vertices or edges marked as selected should be drawn as
    /// selected, false to draw all vertices or edges as unselected.

    protected Boolean m_bUseSelection;

    /// Color of an unselected vertex or edge.

    protected Color m_oColor;

    /// Color of a selected vertex or edge.

    protected Color m_oSelectedColor;

    /// Alpha value to use for vertices and edges that are filtered.

    protected Byte m_btFilteredAlpha;

    /// Default brush to use.

    protected SolidColorBrush m_oDefaultBrush;

    /// Default pen to use.

    protected Pen m_oDefaultPen;
}

}
