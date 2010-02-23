
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.Core
{
//*****************************************************************************
//  Class: NodeXLBase
//
/// <summary>
/// Base class for most classes in the <see cref="Microsoft.NodeXL.Core" />
/// namespace.
/// </summary>
//*****************************************************************************

public class NodeXLBase : Object
{
    //*************************************************************************
    //  Constructor: NodeXLBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="NodeXLBase" /> class.
    /// </summary>
    //*************************************************************************

    public NodeXLBase()
    {
        // (Do nothing.)

        // AssertValid();
    }

    //*************************************************************************
    //  Property: ClassName
    //
    /// <summary>
    /// Gets the full name of the derived class.
    /// </summary>
    ///
    /// <value>
    /// The full name of the derived class, suitable for use in error messages.
    /// </value>
    //*************************************************************************

    public virtual String
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
        // (Do nothing.)
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// <summary>
    /// String format used for most Int32s.
    /// </summary>

    public static readonly String Int32Format = "N0";

    /// <summary>
    /// String format used for most Singles.
    /// </summary>

    public static readonly String SingleFormat = "N1";

    /// <summary>
    /// String format used for most Doubles.
    /// </summary>

    public static readonly String DoubleFormat = "N1";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
