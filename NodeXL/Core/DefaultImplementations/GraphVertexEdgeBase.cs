
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.Core
{
//*****************************************************************************
//  Class: GraphVertexEdgeBase
//
/// <summary>
/// Base class for the <see cref="Graph" />, <see cref="Vertex" />, and <see
/// cref="Edge" /> classes.
/// </summary>
///
/// <remarks>
/// This base class implements the <see cref="IIdentityProvider" /> and <see
/// cref="IMetadataProvider" /> interfaces.
/// </remarks>
//*****************************************************************************

public class GraphVertexEdgeBase :
    NodeXLBase, IIdentityProvider, IMetadataProvider
{
    //*************************************************************************
    //  Constructor: GraphVertexEdgeBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphVertexEdgeBase" />
    /// class.
    /// </summary>
    ///
    /// <param name="id">
    /// The object's ID.  Must be unique among all objects of the same type.
    /// </param>
    //*************************************************************************

    public GraphVertexEdgeBase
    (
        Int32 id
    )
    {
        m_sName = null;
        m_iID = id;
        m_oMetadataProvider = null;

        // AssertValid();
    }

    //*************************************************************************
    //  Property: Name
    //
    /// <summary>
    /// Gets the object's name.
    /// </summary>
    ///
    /// <value>
    /// The object's name, as a String.  Can be null.  The default value is
    /// null.
    /// </value>
    ///
    /// <remarks>
    /// The name is optional and is set by the application.
    ///
    /// <para>
    /// Names are case sensitive.  The names "Mary" and "mary" are different,
    /// for example.
    /// </para>
    ///
    /// <para>
    /// The core NodeXL system does not enforce uniqueness of names.  It is
    /// possible to have two items with the same name in the same collection.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="ID" />
    //*************************************************************************

    public String
    Name
    {
        get
        {
            AssertValid();

            return (m_sName);
        }

        set
        {
            m_sName = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ID
    //
    /// <summary>
    /// Gets the object's ID.
    /// </summary>
    ///
    /// <value>
    /// The object's ID, as an Int32.
    /// </value>
    ///
    /// <remarks>
    /// The ID is set when the object is created.  It must be unique among all
    /// objects of the same type for the duration of the process in which NodeXL
    /// is running.
    ///
    /// <para>
    /// If an object is cloned, the copy must have an ID different from that of
    /// the original.
    /// </para>
    ///
    /// <para>
    /// If the object is saved to persistent storage and then loaded from that
    /// storage, the loaded object must have an ID different from that of the
    /// original.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="Name" />
    //*************************************************************************

    public Int32
    ID
    {
        get
        {
            AssertValid();

            return (m_iID);
        }
    }

    //*************************************************************************
    //  Property: Tag
    //
    /// <summary>
    /// Gets or sets a single metadata object.
    /// </summary>
    ///
    /// <value>
    /// A single metadata object, as an <see cref="Object" />.  Can be null.
    /// The default value is null.
    /// </value>
    ///
    /// <remarks>
    /// If you want to store multiple metadata objects as key/value pairs, use
    /// <see cref="SetValue" /> instead.
    /// </remarks>
    ///
    /// <seealso cref="SetValue" />
    //*************************************************************************

    public Object
    Tag
    {
        get
        {
            AssertValid();

            if (m_oMetadataProvider == null)
            {
                return (null);
            }

            return (m_oMetadataProvider.Tag);
        }

        set
        {
            if (m_oMetadataProvider == null)
            {
                m_oMetadataProvider = new MetadataProvider();
            }

            m_oMetadataProvider.Tag = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: ContainsKey
    //
    /// <summary>
    /// Determines whether a metadata value with a specified key exists.
    /// </summary>
    ///
    /// <param name="key">
    /// The value's key.  Can't be null or empty, and can't start with a
    /// tilde (~).
    /// </param>
    ///
    /// <returns>
    /// true if there is a metadata value with the key <paramref name="key" />,
    /// false if not.
    /// </returns>
    ///
    /// <remarks>
    /// If true is returned, the metadata value can be retrieved with <see
    /// cref="GetValue(String)" /> or one of its variants.
    ///
    /// <para>
    /// Keys that start with a tilde (~) are reserved by the NodeXL system for
    /// internal use.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="GetValue(String)" />
    /// <seealso cref="TryGetValue(String, out Object)" />
    /// <seealso cref="GetRequiredValue(String, Type)" />
    /// <seealso cref="SetValue(String, Object)" />
    //*************************************************************************

    public Boolean
    ContainsKey
    (
        String key
    )
    {
        AssertValid();

        const String MethodName = "ContainsKey";

        CheckClientKey(MethodName, key);

        if (m_oMetadataProvider == null)
        {
            return (false);
        }

        return ( m_oMetadataProvider.ContainsKey(key) );
    }

    //*************************************************************************
    //  Method: RemoveKey
    //
    /// <summary>
    /// Removes a specified metadata key if it exists.
    /// </summary>
    ///
    /// <param name="key">
    /// The key to remove.  Can't be null or empty, and can't start with a
    /// tilde (~).
    /// </param>
    ///
    /// <returns>
    /// true if the metadata key <paramref name="key" /> was removed, false if
    /// there is no such key.
    ///
    /// <para>
    /// Keys that start with a tilde (~) are reserved by the NodeXL system for
    /// internal use.
    /// </para>
    ///
    /// </returns>
    //*************************************************************************

    public Boolean
    RemoveKey
    (
        String key
    )
    {
        AssertValid();

        const String MethodName = "RemoveKey";

        CheckClientKey(MethodName, key);

        if (m_oMetadataProvider == null)
        {
            return (false);
        }

        return ( m_oMetadataProvider.RemoveKey(key) );
    }

    //*************************************************************************
    //  Method: SetValue
    //
    /// <summary>
    /// Sets the metadata value associated with a specified key. 
    /// </summary>
    ///
    /// <param name="key">
    /// The value's key.  Can't be null or empty, and can't start with a
    /// tilde (~).  If the key already exists, its value gets overwritten.
    /// </param>
    ///
    /// <param name="value">
    /// The value to set.  Can be null.
    /// </param>
    ///
    /// <remarks>
    /// The application can store arbitrary metadata by adding key/value pairs
    /// via <see cref="SetValue(String, Object)" />.  The values can be
    /// retrieved with <see cref="GetValue(String)" /> or one of its variants.
    /// The keys are of type <see cref="String" /> and the values are of type
    /// <see cref="Object" />.
    ///
    /// <para>
    /// If you want to store just a single metadata object, use the <see
    /// cref="Tag" /> property instead.
    /// </para>
    ///
    /// <para>
    /// Keys that start with a tilde (~) are reserved by the NodeXL system for
    /// internal use.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="ContainsKey" />
    /// <seealso cref="TryGetValue(String, out Object)" />
    /// <seealso cref="GetRequiredValue(String, Type)" />
    /// <seealso cref="GetValue(String)" />
    /// <seealso cref="Tag" />
    //*************************************************************************

    public void
    SetValue
    (
        String key,
        Object value
    )
    {
        AssertValid();

        const String MethodName = "SetValue";

        CheckClientKey(MethodName, key);

        if (m_oMetadataProvider == null)
        {
            m_oMetadataProvider = new MetadataProvider();
        }

        m_oMetadataProvider.SetValue(key, value);
    }

    //*************************************************************************
    //  Method: GetRequiredValue()
    //
    /// <summary>
    /// Gets the metadata value associated with a specified key and checks
    /// the value type.  The value must exist.
    /// </summary>
    ///
    /// <param name="key">
    /// The value's key.  Can't be null or empty, and can't start with a
    /// tilde (~).
    /// </param>
    ///
    /// <param name="valueType">
    /// Expected type of the requested value.  Sample: typeof(String).
    /// </param>
    ///
    /// <returns>
    /// The metadata value associated with <paramref name="key" />, as an <see
    /// cref="Object" />.
    /// </returns>
    ///
    /// <remarks>
    /// The application can store arbitrary metadata by adding key/value pairs
    /// via <see cref="SetValue(String, Object)" />.  The values can be
    /// retrieved with <see cref="GetValue(String)" /> or one of its variants.
    /// The keys are of type <see cref="String" /> and the values are of type
    /// <see cref="Object" />.
    ///
    /// <para>
    /// Values can be null.  If <paramref name="key" /> exists and its value is
    /// null, null is returned.  If <paramref name="key" /> does not exist,
    /// an exception is thrown.
    /// </para>
    ///
    /// <para>
    /// <paramref name="valueType" /> is used for error checking.  If the type
    /// of the requested value is not <paramref name="valueType" />, an
    /// exception is thrown.  Note that the type of the returned value is <see
    /// cref="Object" />, and that you must cast the returned object to the
    /// specified type.
    /// </para>
    ///
    /// <para>
    /// If you want to store just a single metadata object, use the <see
    /// cref="Tag" /> property instead.
    /// </para>
    ///
    /// <para>
    /// Keys that start with a tilde (~) are reserved by the NodeXL system for
    /// internal use.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="ContainsKey" />
    /// <seealso cref="SetValue" />
    /// <seealso cref="TryGetValue(String, out Object)" />
    /// <seealso cref="GetValue(String)" />
    /// <seealso cref="Tag" />
    //*************************************************************************

    public Object
    GetRequiredValue
    (
        String key,
        Type valueType
    )
    {
        AssertValid();

        const String MethodName = "GetRequiredValue";

        Object oValue;

        if ( !GetValue(MethodName, key, true, valueType, out oValue) )
        {
            this.ArgumentChecker.ThrowArgumentException(

                MethodName, "key",

                String.Format(

                    "A value with the key \"{0}\" does not exist."
                    ,
                    key
                ) );
        }

        return (oValue);
    }

    //*************************************************************************
    //  Method: TryGetValue
    //
    /// <overloads>
    /// Attempts to get the metadata value associated with a specified key. 
    /// </overloads>
    ///
    /// <summary>
    /// Attempts to get the metadata value associated with a specified key and
    /// checks the value type.
    /// </summary>
    ///
    /// <param name="key">
    /// The value's key.  Can't be null or empty, and can't start with a
    /// tilde (~).
    /// </param>
    ///
    /// <param name="valueType">
    /// Expected type of the requested value.  Sample: typeof(String).
    /// </param>
    ///
    /// <param name="value">
    /// Where the metadata value associated with <paramref name="key" /> gets
    /// stored if true is returned, as an <see cref="Object" />.
    /// </param>
    ///
    /// <returns>
    /// true if the metadata value associated with <paramref name="key" />
    /// exists, or false if not.
    /// </returns>
    ///
    /// <remarks>
    /// The application can store arbitrary metadata by adding key/value pairs
    /// via <see cref="SetValue(String, Object)" />.  The values can be
    /// retrieved with <see cref="GetValue(String)" /> or one of its variants.
    /// The keys are of type <see cref="String" /> and the values are of type
    /// <see cref="Object" />.
    ///
    /// <para>
    /// Values can be null.  If <paramref name="key" /> exists and its value is
    /// null, null is stored at <paramref name="value" /> and true is returned.
    /// If <paramref name="key" /> does not exist, false is returned.
    /// </para>
    ///
    /// <para>
    /// <paramref name="valueType" /> is used for error checking.  If the type
    /// of the requested value is not <paramref name="valueType" />, an
    /// exception is thrown.  Note that the type of the value stored at
    /// <paramref name="value" /> is <see cref="Object" />, and that you must
    /// cast the object to the specified type.
    /// </para>
    ///
    /// <para>
    /// If you want to store just a single metadata object, use the <see
    /// cref="Tag" /> property instead.
    /// </para>
    ///
    /// <para>
    /// Keys that start with a tilde (~) are reserved by the NodeXL system for
    /// internal use.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="ContainsKey" />
    /// <seealso cref="SetValue" />
    /// <seealso cref="GetRequiredValue(String, Type)" />
    /// <seealso cref="GetValue(String)" />
    /// <seealso cref="Tag" />
    //*************************************************************************

    public Boolean
    TryGetValue
    (
        String key,
        Type valueType,
        out Object value
    )
    {
        AssertValid();

        const String MethodName = "TryGetValue";

        return ( GetValue(MethodName, key, true, valueType, out value) );
    }

    //*************************************************************************
    //  Method: TryGetValue
    //
    /// <summary>
    /// Attempts to get the metadata value associated with a specified key.
    /// </summary>
    ///
    /// <param name="key">
    /// The value's key.  Can't be null or empty, and can't start with a
    /// tilde (~).
    /// </param>
    ///
    /// <param name="value">
    /// Where the metadata value associated with <paramref name="key" /> gets
    /// stored if true is returned, as an <see cref="Object" />.
    /// </param>
    ///
    /// <returns>
    /// true if the metadata value associated with <paramref name="key" />
    /// exists, or false if not.
    /// </returns>
    ///
    /// <remarks>
    /// The application can store arbitrary metadata by adding key/value pairs
    /// via <see cref="SetValue(String, Object)" />.  The values can be
    /// retrieved with <see cref="GetValue(String)" /> or one of its variants.
    /// The keys are of type <see cref="String" /> and the values are of type
    /// <see cref="Object" />.
    ///
    /// <para>
    /// Values can be null.  If <paramref name="key" /> exists and its value is
    /// null, null is stored at <paramref name="value" /> and true is returned.
    /// If <paramref name="key" /> does not exist, false is returned.
    /// </para>
    ///
    /// <para>
    /// If you want to store just a single metadata object, use the <see
    /// cref="Tag" /> property instead.
    /// </para>
    ///
    /// <para>
    /// Keys that start with a tilde (~) are reserved by the NodeXL system for
    /// internal use.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="ContainsKey" />
    /// <seealso cref="SetValue" />
    /// <seealso cref="GetValue(String)" />
    /// <seealso cref="GetRequiredValue(String, Type)" />
    /// <seealso cref="Tag" />
    //*************************************************************************

    public Boolean
    TryGetValue
    (
        String key,
        out Object value
    )
    {
        AssertValid();

        const String MethodName = "TryGetValue";

        return ( GetValue(MethodName, key, false, null, out value) );
    }

    //*************************************************************************
    //  Method: GetValue
    //
    /// <overloads>
    /// Gets the metadata value associated with a specified key. 
    /// </overloads>
    ///
    /// <summary>
    /// Gets the metadata value associated with a specified key and checks
    /// the value type.
    /// </summary>
    ///
    /// <param name="key">
    /// The value's key.  Can't be null or empty, and can't start with a
    /// tilde (~).
    /// </param>
    ///
    /// <param name="valueType">
    /// Expected type of the requested value.  Sample: typeof(String).
    /// </param>
    ///
    /// <returns>
    /// The metadata value associated with <paramref name="key" />, as an <see
    /// cref="Object" />, or null if the key doesn't exist.
    /// </returns>
    ///
    /// <remarks>
    /// The application can store arbitrary metadata by adding key/value pairs
    /// via <see cref="SetValue(String, Object)" />.  The values can be
    /// retrieved with <see cref="GetValue(String)" /> or one of its variants.
    /// The keys are of type <see cref="String" /> and the values are of type
    /// <see cref="Object" />.
    ///
    /// <para>
    /// Values can be null.  If <paramref name="key" /> exists and its value is
    /// null, null is returned.  If <paramref name="key" /> does not exist,
    /// null is returned.  If you need to distinguish between these two cases,
    /// use <see cref="TryGetValue(String, Type, out Object)" /> instead.
    /// </para>
    ///
    /// <para>
    /// <paramref name="valueType" /> is used for error checking.  If the type
    /// of the requested value is not <paramref name="valueType" />, an
    /// exception is thrown.  Note that the type of the returned value is <see
    /// cref="Object" />, and that you must cast the returned object to the
    /// specified type.
    /// </para>
    ///
    /// <para>
    /// If you want to store just a single metadata object, use the <see
    /// cref="Tag" /> property instead.
    /// </para>
    ///
    /// <para>
    /// Keys that start with a tilde (~) are reserved by the NodeXL system for
    /// internal use.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="ContainsKey" />
    /// <seealso cref="SetValue" />
    /// <seealso cref="TryGetValue(String, out Object)" />
    /// <seealso cref="GetRequiredValue(String, Type)" />
    /// <seealso cref="Tag" />
    //*************************************************************************

    public Object
    GetValue
    (
        String key,
        Type valueType
    )
    {
        AssertValid();

        const String MethodName = "GetValue";

        Object oValue = null;

        if ( !GetValue(MethodName, key, true, valueType, out oValue) )
        {
            // The key does not exist for this instance.

            return (null);
        }

        return (oValue);
    }

    //*************************************************************************
    //  Method: GetValue
    //
    /// <summary>
    /// Gets the metadata value associated with a specified key. 
    /// </summary>
    ///
    /// <param name="key">
    /// The value's key.  Can't be null or empty, and can't start with a
    /// tilde (~).
    /// </param>
    ///
    /// <returns>
    /// The metadata value associated with <paramref name="key" />, as an <see
    /// cref="Object" />, or null if the key doesn't exist.
    /// </returns>
    ///
    /// <remarks>
    /// The application can store arbitrary metadata by adding key/value pairs
    /// via <see cref="SetValue(String, Object)" />.  The values can be
    /// retrieved with <see cref="GetValue(String)" /> or one of its variants.
    /// The keys are of type <see cref="String" /> and the values are of type
    /// <see cref="Object" />.
    ///
    /// <para>
    /// Values can be null.  If <paramref name="key" /> exists and its value is
    /// null, null is returned.  If <paramref name="key" /> does not exist,
    /// null is returned.  If you need to distinguish between these two cases,
    /// use <see cref="TryGetValue(String, out Object)" /> instead.
    /// </para>
    ///
    /// <para>
    /// If you want to store just a single metadata object, use the <see
    /// cref="Tag" /> property instead.
    /// </para>
    ///
    /// <para>
    /// Keys that start with a tilde (~) are reserved by the NodeXL system for
    /// internal use.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="ContainsKey" />
    /// <seealso cref="SetValue" />
    /// <seealso cref="TryGetValue(String, out Object)" />
    /// <seealso cref="GetRequiredValue(String, Type)" />
    /// <seealso cref="Tag" />
    //*************************************************************************

    public Object
    GetValue
    (
        String key
    )
    {
        AssertValid();

        const String MethodName = "GetValue";

        Object oValue = null;

        if ( !GetValue(MethodName, key, false, null, out oValue) )
        {
            // The key does not exist for this instance.

            return (null);
        }

        return (oValue);
    }

    //*************************************************************************
    //  Method: GetValue
    //
    /// <summary>
    /// Gets the metadata value associated with a specified key and checks
    /// the value type.  Distinguishes between a null value and a non-existent
    /// value.
    /// </summary>
    ///
    /// <param name="sMethodName">
    /// Name of the method calling this method.
    /// </param>
    ///
    /// <param name="sKey">
    /// The value's key.  Can't be null or empty.
    /// </param>
    ///
    /// <param name="bCheckValueType">
    /// true to check the type of the requested value.
    /// </param>
    ///
    /// <param name="oValueType">
    /// Expected type of the requested value.  Used only if <paramref
    /// name="bCheckValueType" /> is true.
    /// </param>
    ///
    /// <param name="oValue">
    /// Where the metadata value associated with <paramref name="sKey" /> gets
    /// stored if true is returned, as an <see cref="Object" />.
    /// </param>
    ///
    /// <para>
    /// Keys that start with a tilde (~) are reserved by the NodeXL system for
    /// internal use.
    /// </para>
    ///
    /// <returns>
    /// true if the metadata value associated with <paramref name="sKey" />
    /// exists, or false if not.
    /// </returns>
    //*************************************************************************

    protected Boolean
    GetValue
    (
        String sMethodName,
        String sKey,
        Boolean bCheckValueType,
        Type oValueType,
        out Object oValue
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sMethodName) );
        AssertValid();

        const String KeyArgumentName = "key";

        CheckClientKey(sMethodName, sKey);

        ArgumentChecker oArgumentChecker = this.ArgumentChecker;

        if (bCheckValueType)
        {
            oArgumentChecker.CheckArgumentNotNull(
                sMethodName, "valueType", oValueType);
        }

        oValue = null;

        if ( m_oMetadataProvider == null ||
            !m_oMetadataProvider.TryGetValue(sKey, out oValue) )
        {
            return (false);
        }

        if (bCheckValueType && oValue != null)
        {
            // Check the type of the value.

            if ( !oValueType.IsInstanceOfType(oValue) )
            {
                oArgumentChecker.ThrowArgumentException(

                    sMethodName, KeyArgumentName,

                    String.Format(

                        "The value with the key \"{0}\" is of type {1}.  The"
                        + " expected type is {2}."
                        ,
                        sKey,
                        oValue.GetType().FullName,
                        oValueType.FullName
                    ) );
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: CopyTo
    //
    /// <summary>
    /// Copies this base class's protected data to another object.
    /// </summary>
    ///
    /// <param name="oOtherObject">
    /// The object to copy to.  Must implement <see cref="IIdentityProvider" />
    /// and <see cref="IMetadataProvider" />.
    /// </param>
    ///
    /// <param name="bCopyMetadataValues">
    /// If true, the key/value pairs that were set with <see
    /// cref="IMetadataProvider.SetValue" /> are copied to <paramref
    /// name="oOtherObject" />.  (This is a shallow copy.  The objects pointed
    /// to by the original values are NOT cloned.)  If false, the key/value
    /// pairs are not copied.
    /// </param>
    ///
    /// <param name="bCopyTag">
    /// If true, the <see cref="IMetadataProvider.Tag" /> property on <paramref
    /// name="oOtherObject" /> is set to the same value as in this object.
    /// (This is a shallow copy.  The object pointed to by this object's <see
    /// cref="IMetadataProvider.Tag" /> is NOT cloned.)  If false, the <see
    /// cref="IMetadataProvider.Tag "/> property on <paramref
    /// name="oOtherObject" /> is left at its default value of null.
    /// </param>
    ///
    /// <remarks>
    /// This method can be used to assist in cloning a derived object.  It
    /// copies the object's <see cref="IIdentityProvider.Name" /> and
    /// optionally the object's key/value pairs and <see
    /// cref="IMetadataProvider.Tag" />.  The object's <see
    /// cref="IIdentityProvider.ID" /> is not copied.
    /// </remarks>
    //*************************************************************************

    protected void
    CopyTo
    (
        Object oOtherObject,
        Boolean bCopyMetadataValues,
        Boolean bCopyTag
    )
    {
        AssertValid();
        Debug.Assert(oOtherObject != null);
        Debug.Assert(oOtherObject is IIdentityProvider);
        Debug.Assert(oOtherObject is IMetadataProvider);

        IIdentityProvider oOtherIdentityProvider =
            oOtherObject as IIdentityProvider;

        IMetadataProvider oOtherMetadataProvider =
            oOtherObject as IMetadataProvider;

        oOtherIdentityProvider.Name = this.Name;

        if (m_oMetadataProvider != null)
        {
            m_oMetadataProvider.CopyTo(oOtherMetadataProvider,
                bCopyMetadataValues, bCopyTag);
        }
    }

    //*************************************************************************
    //  Method: CheckClientKey
    //
    /// <summary>
    /// Throws an exception if a key passed to <see cref="ContainsKey" />, <see
    /// cref="SetValue(String, Object)" />, or <see cref="GetValue(String)" />
    /// is invalid.
    /// </summary>
    ///
    /// <param name="sMethodName">
    /// Name of the method calling this method.
    /// </param>
    ///
    /// <param name="sKey">
    /// The key passed to <see cref="ContainsKey" />, <see
    /// cref="SetValue(String, Object)" />, or <see cref="GetValue(String)" />.
    /// </param>
    //*************************************************************************

    protected void
    CheckClientKey
    (
        String sMethodName,
        String sKey
    )
    {
        AssertValid();
        Debug.Assert( !String.IsNullOrEmpty(sMethodName) );

        const String KeyArgumentName = "key";

        ArgumentChecker oArgumentChecker = this.ArgumentChecker;

        // Can't be null or empty.

        oArgumentChecker.CheckArgumentNotEmpty(
            sMethodName, KeyArgumentName, sKey);

        // Note: Don't check for keys that start with a tilde.  Although all
        // such keys are reserved for internal use, the client may actually be
        // a NodeXL method and so they can't be prohibited.
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
    /// The format to use, either "G", "P", or "D".  See <see
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

    protected override void
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

        #if false

        Sample string for sFormat == "G":

            ID = 35

        Sample string for sFormat == "P":

            ID = 35\r\n
            Name = [null]\r\n
            Tag = "hello"\r\n
            Values = 2 key/value pairs\r\n\r\n

        Sample string for sFormat == "D":

            ID = 35\r\n
            Name = [null]\r\n
            Tag = "hello"\r\n
            Values = 2 key/value pairs\r\n
                Key = "abc", Value = "xxx"\r\n
                Key = "de", Value = 123\r\n

        #endif

        ToStringUtil.AppendPropertyToString(oStringBuilder, iIndentationLevel,
            "ID", m_iID.ToString(Int32Format), false);

        if (sFormat == "G")
        {
            return;
        }

        oStringBuilder.AppendLine();

        ToStringUtil.AppendPropertyToString(oStringBuilder, iIndentationLevel,
            "Name", m_sName);

        ToStringUtil.AppendPropertyToString(oStringBuilder, iIndentationLevel,
            "Tag", this.Tag);

        // Append the arbitrary key/value pairs.

        ToStringUtil.AppendPropertyToString(oStringBuilder, iIndentationLevel,
            "Values", String.Empty, false);

        if (m_oMetadataProvider == null)
        {
            oStringBuilder.AppendLine("0 key/value pairs");
        }
        else
        {
            m_oMetadataProvider.AppendToString(oStringBuilder,
                iIndentationLevel, sFormat);
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

        // m_sName
        // m_iID
        // m_oMetadataProvider
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object name.  Can be null or empty.

    protected String m_sName;

    /// Object ID.

    protected Int32 m_iID;

    /// Object that stores the Tag and key/value pairs, or null if no key/value
    /// pairs or the Tag have been set yet.  This is created "lazily" to reduce
    /// memory usage when metadata isn't used.

    protected MetadataProvider m_oMetadataProvider;
}
}
