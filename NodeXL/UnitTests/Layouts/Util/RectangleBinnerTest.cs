
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.Layouts;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: RectangleBinnerTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="RectangleBinner" />
/// class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class RectangleBinnerTest : Object
{
    //*************************************************************************
    //  Constructor: RectangleBinnerTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="RectangleBinnerTest" /> class.
    /// </summary>
    //*************************************************************************

    public RectangleBinnerTest()
    {
        // (Do nothing.)
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
        // (Do nothing.)
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
        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: Test1()
    //
    /// <summary>
    /// Tests the TryGetNextBin() and TryGetRemainingRectangle() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    Test1()
    {
        // 2 rows of 3 bins each, space left over at top.

        RectangleBinner oRectangleBinner = new RectangleBinner(
            new Rectangle( new Point(1, 1), new Size(6, 5) ),
            2);

        Rectangle oReturnedBin;

        foreach (Rectangle oExpectedBin in new Rectangle[] {

            new Rectangle( new Point(1, 4), new Size(2, 2) ),
            new Rectangle( new Point(3, 4), new Size(2, 2) ),
            new Rectangle( new Point(5, 4), new Size(2, 2) ),

            new Rectangle( new Point(1, 2), new Size(2, 2) ),
            new Rectangle( new Point(3, 2), new Size(2, 2) ),
            new Rectangle( new Point(5, 2), new Size(2, 2) ),
            } )
        {
            Assert.IsTrue( oRectangleBinner.TryGetNextBin(out oReturnedBin) );
            Assert.AreEqual(oExpectedBin, oReturnedBin);
        }

        Assert.IsFalse( oRectangleBinner.TryGetNextBin(out oReturnedBin) );
        Assert.IsFalse( oRectangleBinner.TryGetNextBin(out oReturnedBin) );

        Rectangle oRemainingRectangle;

        Assert.IsTrue( oRectangleBinner.TryGetRemainingRectangle(
            out oRemainingRectangle) );

        Assert.AreEqual(new Rectangle( new Point(1, 1), new Size(6, 1) ),
            oRemainingRectangle);
    }

    //*************************************************************************
    //  Method: Test2()
    //
    /// <summary>
    /// Tests the TryGetNextBin() and TryGetRemainingRectangle() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    Test2()
    {
        // 2 rows of 3 bins each, no space left over at top.

        RectangleBinner oRectangleBinner = new RectangleBinner(
            new Rectangle( new Point(1, 2), new Size(6, 4) ),
            2);

        Rectangle oReturnedBin;

        foreach (Rectangle oExpectedBin in new Rectangle[] {

            new Rectangle( new Point(1, 4), new Size(2, 2) ),
            new Rectangle( new Point(3, 4), new Size(2, 2) ),
            new Rectangle( new Point(5, 4), new Size(2, 2) ),

            new Rectangle( new Point(1, 2), new Size(2, 2) ),
            new Rectangle( new Point(3, 2), new Size(2, 2) ),
            new Rectangle( new Point(5, 2), new Size(2, 2) ),
            } )
        {
            Assert.IsTrue( oRectangleBinner.TryGetNextBin(out oReturnedBin) );
            Assert.AreEqual(oExpectedBin, oReturnedBin);
        }

        Assert.IsFalse( oRectangleBinner.TryGetNextBin(out oReturnedBin) );

        Rectangle oRemainingRectangle;

        Assert.IsFalse( oRectangleBinner.TryGetRemainingRectangle(
            out oRemainingRectangle) );
    }

    //*************************************************************************
    //  Method: Test3()
    //
    /// <summary>
    /// Tests the TryGetNextBin() and TryGetRemainingRectangle() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    Test3()
    {
        // 2 rows of 3 bins each, space left over at top and on right.

        RectangleBinner oRectangleBinner = new RectangleBinner(
            new Rectangle( new Point(1, 1), new Size(7, 5) ),
            2);

        Rectangle oReturnedBin;

        foreach (Rectangle oExpectedBin in new Rectangle[] {

            new Rectangle( new Point(1, 4), new Size(2, 2) ),
            new Rectangle( new Point(3, 4), new Size(2, 2) ),
            new Rectangle( new Point(5, 4), new Size(2, 2) ),

            new Rectangle( new Point(1, 2), new Size(2, 2) ),
            new Rectangle( new Point(3, 2), new Size(2, 2) ),
            new Rectangle( new Point(5, 2), new Size(2, 2) ),
            } )
        {
            Assert.IsTrue( oRectangleBinner.TryGetNextBin(out oReturnedBin) );
            Assert.AreEqual(oExpectedBin, oReturnedBin);
        }

        Assert.IsFalse( oRectangleBinner.TryGetNextBin(out oReturnedBin) );

        Rectangle oRemainingRectangle;

        Assert.IsTrue( oRectangleBinner.TryGetRemainingRectangle(
            out oRemainingRectangle) );

        Assert.AreEqual(new Rectangle( new Point(1, 1), new Size(7, 1) ),
            oRemainingRectangle);
    }

    //*************************************************************************
    //  Method: Test4()
    //
    /// <summary>
    /// Tests the TryGetNextBin() and TryGetRemainingRectangle() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    Test4()
    {
        // Room for exactly one bin.

        RectangleBinner oRectangleBinner = new RectangleBinner(
            new Rectangle( new Point(10, 20), new Size(10, 10) ),
            10);

        Rectangle oReturnedBin;

        foreach (Rectangle oExpectedBin in new Rectangle[] {

            new Rectangle( new Point(10, 20), new Size(10, 10) ),
            } )
        {
            Assert.IsTrue( oRectangleBinner.TryGetNextBin(out oReturnedBin) );
            Assert.AreEqual(oExpectedBin, oReturnedBin);
        }

        Assert.IsFalse( oRectangleBinner.TryGetNextBin(out oReturnedBin) );

        Rectangle oRemainingRectangle;

        Assert.IsFalse( oRectangleBinner.TryGetRemainingRectangle(
            out oRemainingRectangle) );
    }

    //*************************************************************************
    //  Method: Test5()
    //
    /// <summary>
    /// Tests the TryGetNextBin() and TryGetRemainingRectangle() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    Test5()
    {
        // Too small for even one bin.

        RectangleBinner oRectangleBinner = new RectangleBinner(
            new Rectangle( new Point(10, 20), new Size(5, 5) ),
            6);

        Rectangle oReturnedBin;

        Assert.IsFalse( oRectangleBinner.TryGetNextBin(out oReturnedBin) );

        Rectangle oRemainingRectangle;

        Assert.IsTrue( oRectangleBinner.TryGetRemainingRectangle(
            out oRemainingRectangle) );

        Assert.AreEqual(new Rectangle( new Point(10, 20), new Size(5, 5) ),
            oRemainingRectangle);
    }

    //*************************************************************************
    //  Method: Test6()
    //
    /// <summary>
    /// Tests the TryGetNextBin() and TryGetRemainingRectangle() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    Test6()
    {
        // TryGetNextBin() never called.

        RectangleBinner oRectangleBinner = new RectangleBinner(
            new Rectangle( new Point(1, 1), new Size(6, 5) ),
            2);

        Rectangle oRemainingRectangle;

        Assert.IsTrue( oRectangleBinner.TryGetRemainingRectangle(
            out oRemainingRectangle) );

        Assert.AreEqual(new Rectangle( new Point(1, 1), new Size(6, 5) ),
            oRemainingRectangle);
    }

    //*************************************************************************
    //  Method: Test7()
    //
    /// <summary>
    /// Tests the TryGetNextBin() and TryGetRemainingRectangle() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    Test7()
    {
        // 2 rows of 3 bins each, space left over at top, but only 3 bins asked
        // for.

        RectangleBinner oRectangleBinner = new RectangleBinner(
            new Rectangle( new Point(1, 1), new Size(6, 5) ),
            2);

        Rectangle oReturnedBin;

        foreach (Rectangle oExpectedBin in new Rectangle[] {

            new Rectangle( new Point(1, 4), new Size(2, 2) ),
            new Rectangle( new Point(3, 4), new Size(2, 2) ),
            new Rectangle( new Point(5, 4), new Size(2, 2) ),
            } )
        {
            Assert.IsTrue( oRectangleBinner.TryGetNextBin(out oReturnedBin) );
            Assert.AreEqual(oExpectedBin, oReturnedBin);
        }

        Rectangle oRemainingRectangle;

        Assert.IsTrue( oRectangleBinner.TryGetRemainingRectangle(
            out oRemainingRectangle) );

        Assert.AreEqual(new Rectangle( new Point(1, 1), new Size(6, 3) ),
            oRemainingRectangle);
    }

    //*************************************************************************
    //  Method: Test8()
    //
    /// <summary>
    /// Tests the TryGetNextBin() and TryGetRemainingRectangle() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    Test8()
    {
        // 2 rows of 3 bins each, space left over at top, but only 4 bins asked
        // for.

        RectangleBinner oRectangleBinner = new RectangleBinner(
            new Rectangle( new Point(1, 1), new Size(6, 5) ),
            2);

        Rectangle oReturnedBin;

        foreach (Rectangle oExpectedBin in new Rectangle[] {

            new Rectangle( new Point(1, 4), new Size(2, 2) ),
            new Rectangle( new Point(3, 4), new Size(2, 2) ),
            new Rectangle( new Point(5, 4), new Size(2, 2) ),

            new Rectangle( new Point(1, 2), new Size(2, 2) ),
            } )
        {
            Assert.IsTrue( oRectangleBinner.TryGetNextBin(out oReturnedBin) );
            Assert.AreEqual(oExpectedBin, oReturnedBin);
        }

        Rectangle oRemainingRectangle;

        Assert.IsTrue( oRectangleBinner.TryGetRemainingRectangle(
            out oRemainingRectangle) );

        Assert.AreEqual(new Rectangle( new Point(1, 1), new Size(6, 1) ),
            oRemainingRectangle);
    }


    //*************************************************************************
    //  Method: Test9()
    //
    /// <summary>
    /// Tests the TryGetNextBin() and TryGetRemainingRectangle() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    Test9()
    {
        // 2 rows of 3 bins each, space left over at top, but only 1 bin asked
        // for.

        RectangleBinner oRectangleBinner = new RectangleBinner(
            new Rectangle( new Point(1, 1), new Size(6, 5) ),
            2);

        Rectangle oReturnedBin;

        foreach (Rectangle oExpectedBin in new Rectangle[] {

            new Rectangle( new Point(1, 4), new Size(2, 2) ),
            } )
        {
            Assert.IsTrue( oRectangleBinner.TryGetNextBin(out oReturnedBin) );
            Assert.AreEqual(oExpectedBin, oReturnedBin);
        }

        Rectangle oRemainingRectangle;

        Assert.IsTrue( oRectangleBinner.TryGetRemainingRectangle(
            out oRemainingRectangle) );

        Assert.AreEqual(new Rectangle( new Point(1, 1), new Size(6, 3) ),
            oRemainingRectangle);
    }

    //*************************************************************************
    //  Method: Test10()
    //
    /// <summary>
    /// Tests the TryGetNextBin() and TryGetRemainingRectangle() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    Test10()
    {
        // Weird case: 2 rows of 3 bins each, no space left over at top, top
        // row only partially filled.

        RectangleBinner oRectangleBinner = new RectangleBinner(
            new Rectangle( new Point(1, 2), new Size(6, 4) ),
            2);

        Rectangle oReturnedBin;

        foreach (Rectangle oExpectedBin in new Rectangle[] {

            new Rectangle( new Point(1, 4), new Size(2, 2) ),
            new Rectangle( new Point(3, 4), new Size(2, 2) ),
            new Rectangle( new Point(5, 4), new Size(2, 2) ),

            new Rectangle( new Point(1, 2), new Size(2, 2) ),
            } )
        {
            Assert.IsTrue( oRectangleBinner.TryGetNextBin(out oReturnedBin) );
            Assert.AreEqual(oExpectedBin, oReturnedBin);
        }

        Rectangle oRemainingRectangle;

        Assert.IsTrue( oRectangleBinner.TryGetRemainingRectangle(
            out oRemainingRectangle) );

        Assert.AreEqual(new Rectangle( new Point(3, 2), new Size(4, 2) ),
            oRemainingRectangle);
    }

    //*************************************************************************
    //  Method: Test11()
    //
    /// <summary>
    /// Tests the TryGetNextBin() and TryGetRemainingRectangle() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    Test11()
    {
        // Weird case: 2 rows of 3 bins each, no space left over at top, top
        // row only partially filled.

        RectangleBinner oRectangleBinner = new RectangleBinner(
            new Rectangle( new Point(1, 2), new Size(6, 4) ),
            2);

        Rectangle oReturnedBin;

        foreach (Rectangle oExpectedBin in new Rectangle[] {

            new Rectangle( new Point(1, 4), new Size(2, 2) ),
            new Rectangle( new Point(3, 4), new Size(2, 2) ),
            new Rectangle( new Point(5, 4), new Size(2, 2) ),

            new Rectangle( new Point(1, 2), new Size(2, 2) ),
            new Rectangle( new Point(3, 2), new Size(2, 2) ),
            } )
        {
            Assert.IsTrue( oRectangleBinner.TryGetNextBin(out oReturnedBin) );
            Assert.AreEqual(oExpectedBin, oReturnedBin);
        }

        Rectangle oRemainingRectangle;

        Assert.IsTrue( oRectangleBinner.TryGetRemainingRectangle(
            out oRemainingRectangle) );

        Assert.AreEqual(new Rectangle( new Point(5, 2), new Size(2, 2) ),
            oRemainingRectangle);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
