
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Common;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: ByDelegateVertexSorterTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="ByDelegateVertexSorter" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class ByDelegateVertexSorterTest : Object
{
    //*************************************************************************
    //  Constructor: ByDelegateVertexSorterTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ByDelegateVertexSorterTest" /> class.
    /// </summary>
    //*************************************************************************

    public ByDelegateVertexSorterTest()
    {
        m_oByDelegateVertexSorter = null;
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
        m_oByDelegateVertexSorter = new ByDelegateVertexSorter();
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
        m_oByDelegateVertexSorter = null;
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
        Assert.IsNotNull(m_oByDelegateVertexSorter.VertexComparer);
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
        // Ascending sort using default comparer, which sorts by ID.

        const Int32 Vertices = 100;

        IGraph oGraph = new Graph();

        IVertex [] aoUnsortedVertices =
            TestGraphUtil.AddVertices(oGraph, Vertices);

        IVertexCollection oVertexCollection = oGraph.Vertices;

        ICollection<IVertex> oSortedVertices =
            m_oByDelegateVertexSorter.Sort(oVertexCollection);

        Assert.AreEqual(Vertices, oSortedVertices.Count);

        Int32 iPreviousID = -1;

        foreach (IVertex oSortedVertex in oSortedVertices)
        {
            Int32 iID = oSortedVertex.ID;
            
            if (iPreviousID != -1)
            {
                Assert.AreEqual(iPreviousID + 1, iID);
            }

            iPreviousID = iID;
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
        // Ascending sort on IVertex.Name.

        m_oByDelegateVertexSorter.VertexComparer = this.CompareVerticesByName;

        const Int32 Vertices = 100;

        IGraph oGraph = new Graph();

        IVertex [] aoUnsortedVertices =
            TestGraphUtil.AddVertices(oGraph, Vertices);

        IVertexCollection oVertexCollection = oGraph.Vertices;

        Int32 i;

        for (i = 0; i < Vertices; i++)
        {
            aoUnsortedVertices[i].Name = (Vertices - i).ToString("D3");
        }

        ICollection<IVertex> oSortedVertices =
            m_oByDelegateVertexSorter.Sort(oVertexCollection);

        Assert.AreEqual(Vertices, oSortedVertices.Count);

        i = 0;

        foreach (IVertex oSortedVertex in oSortedVertices)
        {
            Assert.AreEqual(

                (i + 1).ToString("D3"),
                oSortedVertex.Name
                );

            i++;
        }
    }

    //*************************************************************************
    //  Method: TestSortBad()
    //
    /// <summary>
    /// Tests the Sort() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestSortBad()
    {
        // null vertices.

        try
        {
            m_oByDelegateVertexSorter.Sort(null);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.Core.ByDelegateVertexSorter.Sort: vertices"
                + " argument can't be null.\r\n"
                + "Parameter name: vertices"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: CompareVerticesByName()
    //
    /// <summary>
    /// Compares two vertices by name.
    /// </summary>
    ///
    /// <param name="oVertex1">
    /// First vertex to compare.
    /// </param>
    ///
    /// <param name="oVertex2">
    /// Second vertex to compare.
    /// </param>
    ///
    /// <returns>
    /// See IComparable.Compare().
    /// </returns>
    //*************************************************************************

    protected Int32
    CompareVerticesByName
    (
        IVertex oVertex1,
        IVertex oVertex2
    )
    {
        Assert.IsNotNull(oVertex1.Name);
        Assert.IsNotNull(oVertex2.Name);

        return ( oVertex1.Name.CompareTo(oVertex2.Name) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // Object being tested.

    protected ByDelegateVertexSorter m_oByDelegateVertexSorter;
}

}
