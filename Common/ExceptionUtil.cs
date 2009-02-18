
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: ExceptionUtil
//
/// <summary>
/// Exception utility methods.
/// </summary>
///
/// <remarks>
/// This class contains utility methods for dealing with exceptions.  All
/// methods are static.
/// </remarks>
//*****************************************************************************

public class ExceptionUtil
{
    //*************************************************************************
    //  Constructor: ExceptionUtil()
    //
    /// <summary>
    /// Do not use this constructor.
    /// </summary>
    ///
    /// <remarks>
    /// Do not use this constructor.  All ExceptionUtil methods are static.
    /// </remarks>
    //*************************************************************************

    private
    ExceptionUtil()
    {
        // (All methods are static.)
    }

    //*************************************************************************
    //  Method: GetMessageTrace()
    //
    /// <summary>
    /// Concatenates an exception's message with its inner exceptions.
    /// </summary>
    ///
    /// <param name="oException">
    /// <see cref="System.Exception" /> object to get a message trace for.
    /// </param>
    ///
    /// <returns>
    /// Sample:
    ///
    /// <para>
    /// [InvalidOperationException]: Invalid operation message
    /// [Exception]: Exception message
    /// [InvalidPrinterException]: Invalid printer message
    /// </para>
    ///
    /// </returns>
    ///
    /// <remarks>
    /// This method returns the specified exception's message, followed by a
    /// line break, followed by its inner exception's message (if there is
    /// one), followed by a line break, followed by its inner exception's
    /// message (if there is one), and so on.  Each message is preceded by the
    /// exception type in brackets.
    /// </remarks>
    //*************************************************************************

    public static String
    GetMessageTrace
    (
        Exception oException
    )
    {
        Debug.Assert(oException != null);

        StringBuilder oStringBuilder = new StringBuilder();

        while (oException != null)
        {
            if (oStringBuilder.Length != 0)
                oStringBuilder.Append(Environment.NewLine);

            oStringBuilder.Append( String.Format(
                "[{0}]: {1}",
                oException.GetType().Name,
                oException.Message
                ) );

            oException = oException.InnerException;
        }

        return ( oStringBuilder.ToString() );
    }
}

}
