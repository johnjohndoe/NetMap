
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Common;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: ByMetadataVertexSorterTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="ByMetadataVertexSorter" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class ByMetadataVertexSorterTest : Object
{
    //*************************************************************************
    //  Constructor: ByMetadataVertexSorterTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ByMetadataVertexSorterTest" /> class.
    /// </summary>
    //*************************************************************************

    public ByMetadataVertexSorterTest()
    {
        m_oByMetadataVertexSorter = null;
    }

    //*************************************************************************
    //  Method: SetUp()
    //
    /// <summary>
    /// Gets run before each test.
    /// </summary>
    //*************************************************************************

    [TestInitializeAttribute]

    public void
    SetUp()
    {
        m_oByMetadataVertexSorter = new ByMetadataVertexSorter<Int32>(SortKey);
    }

    //*************************************************************************
    //  Method: TearDown()
    //
    /// <summary>
    /// Gets run after each test.
    /// </summary>
    //*************************************************************************

    [TestCleanupAttribute]

    public void
    TearDown()
    {
        m_oByMetadataVertexSorter = null;
    }

    //*************************************************************************
    //  Method: TestConstructor()
    //
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestConstructor()
    {
        Assert.AreEqual(SortKey, m_oByMetadataVertexSorter.SortKey);
        Assert.IsTrue(m_oByMetadataVertexSorter.SortAscending);
    }

    //*************************************************************************
    //  Method: TestConstructorBad()
    //
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestConstructorBad()
    {
        // Null sortKey.

        try
        {
            m_oByMetadataVertexSorter =
                new ByMetadataVertexSorter<Int32>(null);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core.ByMetadataVertexSorter`1[[System.Int32,"
                + " mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken"
                + "=b77a5c561934e089]].Constructor: "

                + "sortKey argument can't be null.\r\n"
                + "Parameter name: sortKey"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestConstructorBad2()
    //
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestConstructorBad2()
    {
        // Empty sortKey.

        try
        {
            m_oByMetadataVertexSorter =
                new ByMetadataVertexSorter<Int32>(String.Empty);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core.ByMetadataVertexSorter`1[[System.Int32,"
                + " mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken"
                + "=b77a5c561934e089]].Constructor: "

                + "sortKey argument must have a length greater than zero.\r\n"
                + "Parameter name: sortKey"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestSortKey()
    //
    /// <summary>
    /// Tests the SortKey property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSortKey()
    {
        const String SortKey2 = "jkrejkre";

        m_oByMetadataVertexSorter.SortKey = SortKey2;

        Assert.AreEqual(SortKey2, m_oByMetadataVertexSorter.SortKey);
    }

    //*************************************************************************
    //  Method: TestSortKeyBad()
    //
    /// <summary>
    /// Tests the SortKey property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestSortKeyBad()
    {
        // null SortKey.

        try
        {
            m_oByMetadataVertexSorter.SortKey = null;
        }
        catch (ApplicationException oApplicationException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core.ByMetadataVertexSorter`1[[System.Int32,"
                + " mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken"
                + "=b77a5c561934e089]].SortKey: "

                + "Can't be null."
                ,
                oApplicationException.Message
                );

            throw oApplicationException;
        }
    }

    //*************************************************************************
    //  Method: TestSortKeyBad2()
    //
    /// <summary>
    /// Tests the SortKey property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestSortKeyBad2()
    {
        // Empty SortKey.

        try
        {
            m_oByMetadataVertexSorter.SortKey = String.Empty;
        }
        catch (ApplicationException oApplicationException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core.ByMetadataVertexSorter`1[[System.Int32,"
                + " mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken"
                + "=b77a5c561934e089]].SortKey: "

                + "Must have a length greater than zero."
                ,
                oApplicationException.Message
                );

            throw oApplicationException;
        }
    }

    //*************************************************************************
    //  Method: TestSortAscending()
    //
    /// <summary>
    /// Tests the SortAscending property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSortAscending()
    {
        m_oByMetadataVertexSorter.SortAscending = false;

        Assert.IsFalse(m_oByMetadataVertexSorter.SortAscending);

        m_oByMetadataVertexSorter.SortAscending = true;

        Assert.IsTrue(m_oByMetadataVertexSorter.SortAscending);
    }

    //*************************************************************************
    //  Method: TestSort()
    //
    /// <summary>
    /// Tests the Sort() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSort()
    {
        // Ascending sort on Int32.

        const Int32 Vertices = 100;

        IGraph oGraph = new Graph();

        IVertex [] aoUnsortedVertices =
            TestGraphUtil.AddVertices(oGraph, Vertices);

        IVertexCollection oVertexCollection = oGraph.Vertices;

        Int32 i;

        for (i = 0; i < Vertices; i++)
        {
            aoUnsortedVertices[i].SetValue(SortKey, Vertices - i);
        }

        ICollection<IVertex> oSortedVertices =
            m_oByMetadataVertexSorter.Sort(oVertexCollection);

        Assert.AreEqual(Vertices, oSortedVertices.Count);

        i = 0;

        foreach (IVertex oSortedVertex in oSortedVertices)
        {
            Assert.AreEqual(
                i + 1,
                (Int32)oSortedVertex.GetRequiredValue(SortKey, typeof(Int32) )
                );

            i++;
        }
    }

    //*************************************************************************
    //  Method: TestSort2()
    //
    /// <summary>
    /// Tests the Sort() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSort2()
    {
        // Descending sort on Int32.

        const Int32 Vertices = 100;

        IGraph oGraph = new Graph();

        IVertex [] aoUnsortedVertices =
            TestGraphUtil.AddVertices(oGraph, Vertices);

        IVertexCollection oVertexCollection = oGraph.Vertices;

        Int32 i;

        for (i = 0; i < Vertices; i++)
        {
            aoUnsortedVertices[i].SetValue(SortKey, Vertices - i);
        }

        m_oByMetadataVertexSorter.SortAscending = false;

        ICollection<IVertex> oSortedVertices =
            m_oByMetadataVertexSorter.Sort(oVertexCollection);

        Assert.AreEqual(Vertices, oSortedVertices.Count);

        i = 0;

        foreach (IVertex oSortedVertex in oSortedVertices)
        {
            Assert.AreEqual(
                Vertices - i,
                (Int32)oSortedVertex.GetRequiredValue( SortKey, typeof(Int32) )
                );

            i++;
        }
    }

    //*************************************************************************
    //  Method: TestSort3()
    //
    /// <summary>
    /// Tests the Sort() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSort3()
    {
        // Ascending sort on Double.

        ByMetadataVertexSorter<Double> oByMetadataVertexSorter =
            new ByMetadataVertexSorter<Double>(SortKey);

        const Int32 Vertices = 100;

        IGraph oGraph = new Graph();

        IVertex [] aoUnsortedVertices =
            TestGraphUtil.AddVertices(oGraph, Vertices);

        IVertexCollection oVertexCollection = oGraph.Vertices;

        Int32 i;

        for (i = 0; i < Vertices; i++)
        {
            aoUnsortedVertices[i].SetValue( SortKey, (Double)(Vertices - i) );
        }

        ICollection<IVertex> oSortedVertices =
            oByMetadataVertexSorter.Sort(oVertexCollection);

        Assert.AreEqual(Vertices, oSortedVertices.Count);

        i = 0;

        foreach (IVertex oSortedVertex in oSortedVertices)
        {
            Assert.AreEqual(
                (Double)i + 1,

                (Double)oSortedVertex.GetRequiredValue(
                    SortKey, typeof(Double) )
                );

            i++;
        }
    }

    //*************************************************************************
    //  Method: TestSort4()
    //
    /// <summary>
    /// Tests the Sort() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSort4()
    {
        // Descending sort on Double.

        ByMetadataVertexSorter<Double> oByMetadataVertexSorter =
            new ByMetadataVertexSorter<Double>(SortKey);

        const Int32 Vertices = 100;

        IGraph oGraph = new Graph();

        IVertex [] aoUnsortedVertices =
            TestGraphUtil.AddVertices(oGraph, Vertices);

        IVertexCollection oVertexCollection = oGraph.Vertices;

        Int32 i;

        for (i = 0; i < Vertices; i++)
        {
            aoUnsortedVertices[i].SetValue( SortKey, (Double)(Vertices - i) );
        }

        oByMetadataVertexSorter.SortAscending = false;

        for (i = 0; i < Vertices; i++)
        {
            aoUnsortedVertices[i].SetValue( SortKey, (Double)(Vertices - i) );
        }

        ICollection<IVertex> oSortedVertices =
            oByMetadataVertexSorter.Sort(oVertexCollection);

        Assert.AreEqual(Vertices, oSortedVertices.Count);

        i = 0;

        foreach (IVertex oSortedVertex in oSortedVertices)
        {
            Assert.AreEqual(

                (Double)(Vertices - i),

                (Double)oSortedVertex.GetRequiredValue(
                    SortKey, typeof(Double) )
                );

            i++;
        }
    }

    //*************************************************************************
    //  Method: TestSortBad()
    //
    /// <summary>
    /// Tests the Sort(IVertexCollection) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestSortBad()
    {
        // null collection.

        try
        {
            IVertexCollection oVertexCollection = null;

            m_oByMetadataVertexSorter.Sort(oVertexCollection);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core.ByMetadataVertexSorter`1[[System.Int32,"
                + " mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken"
                + "=b77a5c561934e089]].Sort: "

                + "vertices argument can't be null.\r\n"
                + "Parameter name: vertices"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestSortBad2()
    //
    /// <summary>
    /// Tests the Sort(IVertexCollection) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestSortBad2()
    {
        // Missing key.

        try
        {
            IGraph oGraph = new Graph();

            oGraph.Vertices.Add();

            m_oByMetadataVertexSorter.Sort(oGraph.Vertices);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core.ByMetadataVertexSorter`1[[System.Int32,"
                + " mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken"
                + "=b77a5c561934e089]].Sort: "

                + "One of the vertices does not have the specified sort key,"
                + " or the key's value is the wrong type.\r\n"
                + "Parameter name: key"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestSortBad3()
    //
    /// <summary>
    /// Tests the Sort(IVertexCollection) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestSortBad3()
    {
        // Value is wrong type.

        try
        {
            IGraph oGraph = new Graph();

            IVertex oVertex = oGraph.Vertices.Add();

            oVertex.SetValue(SortKey, "String");

            m_oByMetadataVertexSorter.Sort(oGraph.Vertices);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core.ByMetadataVertexSorter`1[[System.Int32,"
                + " mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken"
                + "=b77a5c561934e089]].Sort: "

                + "One of the vertices does not have the specified sort key,"
                + " or the key's value is the wrong type.\r\n"
                + "Parameter name: key"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    protected const String SortKey = "abcdefg";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // Object being tested.

    protected ByMetadataVertexSorter<Int32> m_oByMetadataVertexSorter;
}

}
