
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.Core
{
//*****************************************************************************
//  Class: EventUtil
//
/// <summary>
/// Utility methods for dealing with events.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class EventUtil
{
    //*************************************************************************
    //  Method: FireEvent()
    //
    /// <summary>
    /// Fires an event with an <see cref="EventHandler" /> signature if
    /// appropriate.
    /// </summary>
    ///
    /// <param name="eventHandler">
    /// Event handler, or null if no clients have subscribed to the event.
    /// </param>
    ///
    /// <param name="sender">
    /// Sender of the event.
    /// </param>
    ///
    /// <remarks>
    /// If <paramref name="eventHandler" /> is not null, this method fires the
    /// event represented by <paramref name="eventHandler" />.  Otherwise, it
    /// does nothing.
    /// </remarks>
    //*************************************************************************

    public static void
    FireEvent
    (
        Object sender,
        EventHandler eventHandler
    )
    {
        Debug.Assert(sender != null);

        if (eventHandler != null)
        {
            eventHandler(sender, EventArgs.Empty);
        }
    }
}

}
