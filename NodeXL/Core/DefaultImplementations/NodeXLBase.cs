
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Collections;
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
///
/// <remarks>
/// The derived class may want to override the virtual <see
/// cref="AppendPropertiesToString" /> method.
/// </remarks>
//*****************************************************************************

public class NodeXLBase : Object, IFormattableNodeXL
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
    //  Method: ToString()
    //
    /// <overloads>
    /// Formats the value of the current instance.
    /// </overloads>
    ///
    /// <summary>
    /// Formats the value of the current instance using the default format. 
    /// </summary>
    ///
    /// <returns>
    /// The formatted string.
    /// </returns>
    //*************************************************************************

    public override String
    ToString()
    {
        AssertValid();

        return ( ToString("G", null) );
    }

    //*************************************************************************
    //  Method: ToString()
    //
    /// <summary>
    /// Formats the value of the current instance using a specified format.
    /// </summary>
    ///
    /// <param name="format">
    /// The <see cref="String" /> specifying the format to use, or null or an
    /// empty string to use the default format.  See <see
    /// cref="ToString(String, IFormatProvider)" /> for the available
    /// formats.
    /// </param>
    ///
    /// <returns>
    /// The formatted string.
    /// </returns>
    //*************************************************************************

    public virtual String
    ToString
    (
        String format
    )
    {
        AssertValid();

        return ( ToString(format, null) );
    }

    //*************************************************************************
    //  Method: ToString()
    //
    /// <summary>
    /// Formats the value of the current instance using a specified format and
    /// format provider.
    /// </summary>
    ///
    /// <param name="format">
    /// The <see cref="String" /> specifying the format to use, or null or an
    /// empty string to use the default format.  The available formats are 
    /// listed in the remarks.
    /// </param>
    ///
    /// <param name="formatProvider">
    /// The <see cref="IFormatProvider" /> to use to format the value, or null
    /// to use the format information from the current locale setting of the
    /// operating system.   This is currently ignored.
    /// </param>
    ///
    /// <returns>
    /// The formatted string.
    /// </returns>
    ///
    /// <remarks>
    /// <paramref name="formatProvider" /> is currently ignored.
    ///
    /// <para>
    /// <paramref name="format" /> must be one of the following values:
    /// </para>
    ///
    /// <list type="table">
    ///
    /// <listheader>
    /// <term>Format</term>
    /// <term>Results</term>
    /// </listheader>
    ///
    /// <item>
    /// <term>G</term>
    /// <term>
    /// Default format, includes only an instance summary.
    /// </term>
    /// </item>
    ///
    /// <item>
    /// <term>P</term>
    /// <term>
    /// Includes all public properties, each on a separate line.  For
    /// collection properties, only the item count is included.
    /// </term>
    /// </item>
    ///
    /// <item>
    /// <term>D</term>
    /// <term>
    /// Includes all public properties, each on a separate line.  For
    /// collection properties, a summary of each item in the collection is
    /// included, each on a separate line.
    /// </term>
    /// </item>
    ///
    /// <item>
    /// <term>null</term>
    /// <term>
    /// Same as G.
    /// </term>
    /// </item>
    ///
    /// <item>
    /// <term>Empty string ("")</term>
    /// <term>
    /// Same as G.
    /// </term>
    /// </item>
    ///
    /// </list>
    ///
    /// </remarks>
    //*************************************************************************

    public virtual String
    ToString
    (
        String format,
        IFormatProvider formatProvider
    )
    {
        AssertValid();

        const String MethodName = "ToString";

        if ( String.IsNullOrEmpty(format) )
        {
            format = "G";
        }

        switch (format)
        {
            case "G":
            case "P":
            case "D":

                StringBuilder oStringBuilder = new StringBuilder();

                const Int32 IndentationLevel = 0;

                // Append the properties for the derived class.

                AppendPropertiesToString(
                    oStringBuilder, IndentationLevel, format);

                return ( oStringBuilder.ToString() );

            default:

                Debug.Assert(false);

                throw new FormatException( String.Format(

                    "{0}.{1}: Invalid format.  Available formats are G, P, and"
                    + " D."
                    ,
                    this.ClassName,
                    MethodName
                    ) );
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
    //  Method: AppendPropertiesToString()
    //
    /// <summary>
    /// Appends the derived class's public property values to a String.
    /// </summary>
    ///
    /// <param name="oStringBuilder">
    /// Object to append to.
    /// </param>
    ///
    /// <param name="iIndentationLevel">
    /// Current indentation level.  Level 0 is "no indentation."
    /// </param>
    ///
    /// <param name="sFormat">
    /// The format to use, either G", "P", or "D".  See <see
    /// cref="NodeXLBase.ToString()" /> for details.
    /// </param>
    ///
    /// <remarks>
    /// This method calls <see cref="ToStringUtil.AppendPropertyToString(
    /// StringBuilder, Int32, String, Object, Boolean)" /> for each of the
    /// derived class's public properties.  It is used in the implementation of
    /// <see cref="NodeXLBase.ToString()" />.
    /// </remarks>
    //*************************************************************************

    protected virtual void
    AppendPropertiesToString
    (
        StringBuilder oStringBuilder,
        Int32 iIndentationLevel,
        String sFormat
    )
    {
        AssertValid();
        Debug.Assert(oStringBuilder != null);
        Debug.Assert(iIndentationLevel >= 0);
        Debug.Assert( !String.IsNullOrEmpty(sFormat) );
        Debug.Assert(sFormat == "G" || sFormat == "P" || sFormat == "D");

        // Append the class name.

        ToStringUtil.AppendIndentationToString(oStringBuilder,
            iIndentationLevel);

        oStringBuilder.AppendLine(this.ClassName);
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
