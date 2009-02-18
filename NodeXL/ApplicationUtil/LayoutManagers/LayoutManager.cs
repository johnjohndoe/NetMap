
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.NodeXL.Layouts;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ApplicationUtil
{
//*****************************************************************************
//  Class: LayoutManager
//
/// <summary>
/// Helper class for managing graph layouts.
/// </summary>
///
/// <remarks>
/// This class defines a set of available layouts.  (A layout is a class that
/// implements <see cref="IAsyncLayout" />.  It provides a <see
/// cref="Layout" /> property for keeping track of the layout type currently in
/// use, a <see cref="LayoutChanged" /> event that is raised when the layout is
/// changed, and a <see cref="CreateLayout" /> method for creating a layout of
/// the current type.
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
    //  Method: CreateLayout()
    //
    /// <summary>
    /// Creates a layout of the current type.
    /// </summary>
    ///
    /// <returns>
    /// A layout of type <see cref="Layout" />.
    /// </returns>
    //*************************************************************************

    public IAsyncLayout
    CreateLayout()
    {
        AssertValid();

        switch (m_eLayout)
        {
            case LayoutType.Circle:

                return ( new CircleLayout() );

            case LayoutType.Spiral:

                return ( new SpiralLayout() );

            case LayoutType.SinusoidHorizontal:

                return ( new SinusoidHorizontalLayout() );

            case LayoutType.SinusoidVertical:

                return ( new SinusoidVerticalLayout() );

            case LayoutType.Grid:

                return ( new GridLayout() );

            case LayoutType.FruchtermanReingold:

                return ( new FruchtermanReingoldLayout() );

            case LayoutType.Random:

                return ( new RandomLayout() );

            case LayoutType.Sugiyama:

                return ( new SugiyamaLayout() );

            default:

                Debug.Assert(false);
                return (null);
        }
    }

    //*************************************************************************
    //  Event: LayoutChanged
    //
    /// <summary>
    /// Occurs when the <see cref="Layout" /> property changes.
    /// </summary>
    ///
    /// <seealso cref="Layout" />
    //*************************************************************************

    public event EventHandler LayoutChanged;


    //*************************************************************************
    //  Property: ClassName
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
    //  Property: ArgumentChecker
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
