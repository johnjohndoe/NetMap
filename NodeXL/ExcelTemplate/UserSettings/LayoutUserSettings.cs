
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Diagnostics;
using Microsoft.NodeXL.Layouts;
using Microsoft.NodeXL.ApplicationUtil;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: LayoutUserSettings
//
/// <summary>
/// Stores the user's settings for all the graph layouts used by the
/// application.
/// </summary>
//*****************************************************************************

[ SettingsGroupNameAttribute("LayoutUserSettings") ]

public class LayoutUserSettings : ApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: LayoutUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the LayoutUserSettings class.
    /// </summary>
    //*************************************************************************

    public LayoutUserSettings()
    {
        // (Do nothing.)

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

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("FruchtermanReingold") ]

    public LayoutType
    Layout
    {
        get
        {
            AssertValid();

            return ( (LayoutType)this[LayoutKey] );
        }

        set
        {
            this[LayoutKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Margin
    //
    /// <summary>
    /// Gets or sets the margin to subtract from each edge of the graph
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
            this[MarginKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: UseBinning
    //
    /// <summary>
    /// Gets or sets a flag indicating whether binning should be used when the
    /// graph is laid out.
    /// </summary>
    ///
    /// <value>
    /// true to use binning.  The default value is false.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("false") ]

    public Boolean
    UseBinning
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[UseBinningKey] );
        }

        set
        {
            this[UseBinningKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: MaximumVerticesPerBin
    //
    /// <summary>
    /// Gets or sets the maximum number of vertices a binned component can
    /// have.
    /// </summary>
    ///
    /// <value>
    /// The maximum number of vertices a binned component can have.  The
    /// default value is 3.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("3") ]

    public Int32
    MaximumVerticesPerBin
    {
        get
        {
            AssertValid();

            return ( (Int32)this[MaximumVerticesPerBinKey] );
        }

        set
        {
            this[MaximumVerticesPerBinKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: BinLength
    //
    /// <summary>
    /// Gets or sets the height and width of each bin, in graph rectangle
    /// units.
    /// </summary>
    ///
    /// <value>
    /// The height and width of each bin, in graph rectangle units.  The
    /// default value is 16.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("16") ]

    public Int32
    BinLength
    {
        get
        {
            AssertValid();

            return ( (Int32)this[BinLengthKey] );
        }

        set
        {
            this[BinLengthKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: FruchtermanReingoldC
    //
    /// <summary>
    /// Gets or sets the constant that determines the strength of the
    /// attractive and repulsive forces between the vertices when using the
    /// FruchtermanReingoldLayout.
    /// </summary>
    ///
    /// <value>
    /// The "C" constant in the "Modelling the forces" section of the
    /// Fruchterman-Reingold paper.  Must be greater than 0.  The default value
    /// is 3.0.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("3.0") ]

    public Single
    FruchtermanReingoldC
    {
        get
        {
            AssertValid();

            return ( (Single)this[FruchtermanReingoldCKey] );
        }

        set
        {
            this[FruchtermanReingoldCKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: FruchtermanReingoldIterations
    //
    /// <summary>
    /// Gets or sets the number of times to run the Fruchterman-Reingold
    /// algorithm when using the FruchtermanReingoldLayout.
    /// </summary>
    ///
    /// <value>
    /// The number of times to run the Fruchterman-Reingold algorithm when the
    /// graph is laid out, as an Int32.  Must be greater than zero.  The
    /// default value is 10.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("10") ]

    public Int32
    FruchtermanReingoldIterations
    {
        get
        {
            AssertValid();

            return ( (Int32)this[FruchtermanReingoldIterationsKey] );
        }

        set
        {
            this[FruchtermanReingoldIterationsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: TransferToLayout()
    //
    /// <summary>
    /// Transfers the settings to an <see cref="IAsyncLayout" /> object.
    /// </summary>
    ///
    /// <param name="asyncLayout">
    /// Layout to transfer the settings to.
    /// </param>
    //*************************************************************************

    public void
    TransferToLayout
    (
        IAsyncLayout asyncLayout
    )
    {
        Debug.Assert(asyncLayout != null);
        AssertValid();

        asyncLayout.Margin = this.Margin;
        asyncLayout.UseBinning = this.UseBinning;
        asyncLayout.MaximumVerticesPerBin = this.MaximumVerticesPerBin;
        asyncLayout.BinLength = this.BinLength;

        if (asyncLayout is FruchtermanReingoldLayout)
        {
            FruchtermanReingoldLayout oFruchtermanReingoldLayout =
                (FruchtermanReingoldLayout)asyncLayout;

            oFruchtermanReingoldLayout.C = this.FruchtermanReingoldC;

            oFruchtermanReingoldLayout.Iterations =
                this.FruchtermanReingoldIterations;
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

    /// Name of the settings key for the Layout property.

    protected const String LayoutKey = "Layout";

    /// Name of the settings key for the Margin property.

    protected const String MarginKey = "Margin";

    /// Name of the settings key for the UseBinning property.

    protected const String UseBinningKey = "UseBinning";

    /// Name of the settings key for the MaximumVerticesPerBin property.

    protected const String MaximumVerticesPerBinKey = "MaximumVerticesPerBin";

    /// Name of the settings key for the BinLength property.

    protected const String BinLengthKey = "BinLength";

    /// Name of the settings key for the FruchtermanReingoldC property.

    protected const String FruchtermanReingoldCKey = "FruchtermanReingoldC";

    /// Name of the settings key for the FruchtermanReingoldIterations property.

    protected const String FruchtermanReingoldIterationsKey =
        "FruchtermanReingoldIterations";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
