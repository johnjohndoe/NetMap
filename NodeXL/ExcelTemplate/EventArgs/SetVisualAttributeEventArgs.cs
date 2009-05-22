
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: SetVisualAttributeEventArgs
//
/// <summary>
/// Provides information for the <see
/// cref="ThisWorkbook.SetVisualAttribute2" /> event.
/// </summary>
///
/// <remarks>
/// The first event handler that sets the specified visual attribute should set
/// the <see cref="VisualAttributeSet" /> property to true, and other event
/// handlers should then ignore the event.
/// </remarks>
//*****************************************************************************

public class SetVisualAttributeEventArgs : EventArgs
{
    //*************************************************************************
    //  Constructor: SetVisualAttributeEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="SetVisualAttributeEventArgs" /> class.
    /// </summary>
    ///
    /// <param name="visualAttribute">
    /// Specifies the visual attribute to set.  Must be only one of the flags
    /// in the <see cref="VisualAttributes" /> enumeration; it cannot be an
    /// ORed combination.
    /// </param>
    ///
    /// <param name="attributeValue">
    /// The visual attribute value, or null if the value isn't known yet and
    /// must be obtained from the user.
    /// </param>
    //*************************************************************************

    public SetVisualAttributeEventArgs
    (
        VisualAttributes visualAttribute,
        Object attributeValue
    )
    {
        m_eVisualAttribute = visualAttribute;
        m_oAttributeValue = attributeValue;
        m_bVisualAttributeSet = false;

        AssertValid();
    }

    //*************************************************************************
    //  Property: VisualAttribute
    //
    /// <summary>
    /// Gets a flag indicating which columns changed.
    /// </summary>
    ///
    /// <value>
    /// Specifies the visual attribute to set.  This is only one of the flags
    /// in the <see cref="VisualAttributes" /> enumeration; it cannot be an
    /// ORed combination.
    /// </value>
    //*************************************************************************

    public VisualAttributes
    VisualAttribute
    {
        get
        {
            AssertValid();

            return (m_eVisualAttribute);
        }
    }

    //*************************************************************************
    //  Property: AttributeValue
    //
    /// <summary>
    /// Gets the visual attribute value.
    /// </summary>
    ///
    /// <value>
    /// The visual attribute value, or null if the value isn't known yet and
    /// must be obtained from the user.
    /// </value>
    //*************************************************************************

    public Object
    AttributeValue
    {
        get
        {
            AssertValid();

            return (m_oAttributeValue);
        }
    }

    //*************************************************************************
    //  Property: VisualAttributeSet
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the specified visual attribute
    /// has been set.
    /// </summary>
    ///
    /// <value>
    /// true if the specified visual attribute has been set.
    /// </value>
    ///
    /// <remarks>
    /// The first event handler that sets the specified visual attribute should
    /// set this property to true, and other event handlers should then ignore
    /// the event.
    /// </remarks>
    //*************************************************************************

    public Boolean
    VisualAttributeSet
    {
        get
        {
            AssertValid();

            return (m_bVisualAttributeSet);
        }

        set
        {
            m_bVisualAttributeSet = value;

            AssertValid();
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
        // m_eVisualAttribute
        // m_oAttributeValue;
        // m_bVisualAttributeSet
    }


    //*************************************************************************
    //  Protected member data
    //*************************************************************************

    /// One of the flags in the VisualAttributes enumeration.

    protected VisualAttributes m_eVisualAttribute;

    /// The visual attribute value, or null if the value isn't known yet and
    /// must be obtained from the user.

    protected Object m_oAttributeValue;

    /// true if the specified visual attribute has been set.

    protected Boolean m_bVisualAttributeSet;
}


//*****************************************************************************
//  Delegate: SetVisualAttributeEventHandler
//
/// <summary>
/// Represents a method that will handle the <see
/// cref="ThisWorkbook.SetVisualAttribute2" /> event.
/// </summary>
///
/// <param name="sender">
/// The source of the event.
/// </param>
///
/// <param name="e">
/// An <see cref="SetVisualAttributeEventArgs" /> object that contains the
/// event data.
/// </param>
//*****************************************************************************

public delegate void
SetVisualAttributeEventHandler
(
    Object sender,
    SetVisualAttributeEventArgs e
);

}
