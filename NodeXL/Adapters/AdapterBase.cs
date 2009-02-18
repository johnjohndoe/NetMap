
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Adapters
{
//*****************************************************************************
//  Class: AdapterBase
//
/// <summary>
/// Base class for most classes in the <see cref="Microsoft.NodeXL.Adapters" />
/// namespace.
/// </summary>
//*****************************************************************************

public class AdapterBase : NodeXLBase
{
    //*************************************************************************
    //  Constructor: AdapterBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="AdapterBase" />
    /// class.
    /// </summary>
    //*************************************************************************

    public AdapterBase()
    {
        // (Do nothing.)

        // AssertValid();
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
    ///
    /// <para>
    /// The <see cref="NodeXLBase" /> implementation of this property cannot be
    /// used because it's marked as internal.  The reason it's internal is
    /// explained in the comments for the <see cref="ArgumentChecker" /> class.
    /// </para>
    ///
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

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
        base.AssertValid();

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
