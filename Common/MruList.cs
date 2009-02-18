
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: MruList
//
/// <summary>
/// Represents a most-recently-used list.
/// </summary>
///
/// <remarks>
/// The list contains objects of type Object.  An argument to the constructor
/// determines the maximum number of objects in the list.
///
/// <para>
/// Use the <see cref="Add" /> method to add an object to the top of the list.
/// Adding an object pushes the other objects down.  If the list is full,
/// adding an object causes the oldest object to drop off the bottom of the
/// list.
/// </para>
///
/// <para>
/// Use the <see cref="Array" /> property to get the objects in the list.
/// </para>
///
/// <para>
/// Use the <see cref="Remove" /> method to remove an object from the list.
/// </para>
///
/// <para>
/// This class is not serializable.  To serialize an MruList, derive a class
/// from MruList and implement a type-specific Array property.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class MruList : Object
{
    //*************************************************************************
    //  Constructor: MruList()
    //
    /// <summary>
    /// Initializes a new instance of the MruList class.
    /// </summary>
    ///
    /// <param name="iCapacity">
    /// Maximum number of objects in the list.
    /// </param>
    //*************************************************************************

    public MruList
    (
        Int32 iCapacity
    )
    {
        Initialize(iCapacity);
    }

    //*************************************************************************
    //  Constructor: MruList()
    //
    /// <summary>
    /// Initializes a new instance of the MruList class.
    /// </summary>
    ///
    /// <remarks>
    /// This paramaterless constructor is required by the XmlSerializer.
    /// </remarks>
    //*************************************************************************

    public MruList()
    {
        Initialize(10);
    }

    //*************************************************************************
    //  Property: Count
    //
    /// <summary>
    /// Gets the number of objects in the list.
    /// </summary>
    ///
    /// <value>
    /// The number of objects in the list.
    /// </value>
    //*************************************************************************

    public Int32 Count
    {
        get
        {
            AssertValid();

            return (m_oObjects.Count);
        }
    }

    //*************************************************************************
    //  Property: Capacity
    //
    /// <summary>
    /// Do not use this property.
    /// </summary>
    ///
    /// <value>
    /// The maximum number of objects in the list.
    /// </value>
    ///
    /// <remarks>
    /// This property exists only for the XmlSerializer.  Do not use it,
    /// because the class was not designed to have its capacity changed after
    /// an object is created.
    /// </remarks>
    //*************************************************************************

    public Int32 Capacity
    {
        get
        {
            AssertValid();

            return (m_iCapacity);
        }

        set
        {
            m_iCapacity = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Array
    //
    /// <summary>
    /// Gets the objects in the list as a new Object array.
    /// </summary>
    ///
    /// <value>
    /// An Object array containing the objects in the list.
    /// </value>
    ///
    /// <remarks>
    /// The newest object is at index 0.
    ///
    /// <para>
    /// Do not set this property.  The setter is for the XmlSerializer only.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    // This class is not serializable.  To serialize an MruList, derive a class
    // from MruList and implement a type-specific Array property.

    [System.Xml.Serialization.XmlIgnoreAttribute]

    public Object [] Array
    {
        get
        {
            return ( m_oObjects.ToArray() );
        }

        set
        {
            // Do not call this.  This is for the XmlSerializer only.

            m_oObjects = new ArrayList();
            m_oObjects.AddRange(value);
        }
    }

    //*************************************************************************
    //  Method: Add()
    //
    /// <summary>
    /// Adds an object to the top of the list.
    /// </summary>
    ///
    /// <param name="oObject">
    /// Object to add to the list.
    /// </param>
    ///
    /// <remarks>
    /// If <paramref name="oObject" /> already exists in the list, the object
    /// is not added again.  Instead, the existing entry is moved to the top of
    /// the list.
    /// </remarks>
    //*************************************************************************

    public void
    Add
    (
        Object oObject
    )
    {
        Debug.Assert(oObject != null);
        AssertValid();

        Int32 iIndex = m_oObjects.IndexOf(oObject);

        if (iIndex == -1)
        {
            // The object is not already in the list.

            if (m_oObjects.Count == m_iCapacity)
            {
                // The list is full.  Remove the oldest object, which is at
                // the bottom of the list.

                m_oObjects.RemoveAt(m_iCapacity - 1);
            }

            // Add the object to the top of the list.

            m_oObjects.Insert(0, oObject);
        }
        else
        {
            // The object is already in the list.  Move it to the top of the
            // list.

            m_oObjects.RemoveAt(iIndex);
            m_oObjects.Insert(0, oObject);
        }
    }

    //*************************************************************************
    //  Method: Remove()
    //
    /// <summary>
    /// Removex an object from the list.
    /// </summary>
    ///
    /// <param name="oObject">
    /// Object to remove from the list.
    /// </param>
    ///
    /// <remarks>
    /// If <paramref name="oObject" /> is not in the list, this method does
    /// nothing.
    /// </remarks>
    //*************************************************************************

    public void
    Remove
    (
        Object oObject
    )
    {
        Debug.Assert(oObject != null);
        AssertValid();

        m_oObjects.Remove(oObject);
    }

    //*************************************************************************
    //  Method: Contains()
    //
    /// <summary>
    /// Determines whether an object is in the list.
    /// </summary>
    ///
    /// <param name="oObject">
    /// Object to test.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="oObject" /> is in the list.
    /// </returns>
    //*************************************************************************

    public Boolean
    Contains
    (
        Object oObject
    )
    {
        Debug.Assert(oObject != null);
        AssertValid();

        return ( m_oObjects.Contains(oObject) );
    }

    //*************************************************************************
    //  Method: Initialize()
    //
    /// <summary>
    /// Initializes the object.
    /// </summary>
    ///
    /// <param name="iCapacity">
    /// Maximum number of objects in the list.
    /// </param>
    //*************************************************************************

    protected void
    Initialize
    (
        Int32 iCapacity
    )
    {
        Debug.Assert(iCapacity > 0);

        m_iCapacity = iCapacity;
        m_oObjects = new ArrayList(iCapacity);

        AssertValid();
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
        Debug.Assert(m_oObjects != null);
        Debug.Assert(m_iCapacity > 0);
        Debug.Assert(m_oObjects.Count <= m_iCapacity);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// List of objects.  The most recently added object is at index 0.

    protected ArrayList m_oObjects;

    /// Maximum number of objects in the list.

    protected Int32 m_iCapacity;
}

}
