

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: ExcelCalculationRestorer
//
/// <summary>
/// Restores the Calculation property of an Excel Application object after a
/// specified period.
/// </summary>
///
/// <remarks>
/// This class can be used by callers that modify the Calculation property of
/// an Excel Application object but want the original property value
/// automatically restored after a specified time.
///
/// <para>
/// Create a <see cref="ExcelCalculationRestorer" /> object before the
/// Application.Calculation property is changed.  When the property is changed,
/// call <see cref="StartRestoreTimer" />.  After <see
/// cref="TimerIntervalMs" /> milliseconds, the original property value will be
/// automatically restored.
/// </para>
///
/// <para>
/// <see cref="StartRestoreTimer" /> can be called repeatedly to restart the
/// restore timer.
/// </para>
///
/// <para>
/// Optionally, call <see cref="Restore" /> to immediately restore the original
/// property value.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class ExcelCalculationRestorer : Object
{
    //*************************************************************************
    //  Constructor: ExcelCalculationRestorer()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ExcelCalculationRestorer" />
    /// class.
    /// </summary>
    ///
    /// <param name="application">
    /// Application object whose Calculation property needs to be recorded and
    /// later restored.
    /// </param>
    //*************************************************************************

    public ExcelCalculationRestorer
    (
        Microsoft.Office.Interop.Excel.Application application
    )
    {
        Debug.Assert(application != null);

        m_oApplication = application;
        m_eOriginalCalculationValue = m_oApplication.Calculation;

        m_oTimer = new System.Windows.Forms.Timer();
        m_oTimer.Tick += new EventHandler(this.m_oTimer_Tick);

        AssertValid();
    }

    //*************************************************************************
    //  Property: TimerIntervalMs
    //
    /// <summary>
    /// Gets or set the restore timer interval.
    /// </summary>
    ///
    /// <value>
    /// The number of milliseconds after <see cref="StartRestoreTimer" /> is
    /// called that the Application.Calculation property is restored to its
    /// original value.  The default is 100 milliseconds.
    /// </value>
    //*************************************************************************

    public Int32
    TimerIntervalMs
    {
        get
        {
            AssertValid();

            return (m_oTimer.Interval);
        }

        set
        {
            m_oTimer.Interval = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: StartRestoreTimer()
    //
    /// <summary>
    /// Starts the restore timer.
    /// </summary>
    ///
    /// <remarks>
    /// The original value of the Application.Calculation property will be
    /// restored <see cref="TimerIntervalMs" /> milliseconds after this method
    /// is called.
    ///
    /// <para>
    /// This method can be called repeatedly to restart the timer.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    StartRestoreTimer()
    {
        AssertValid();

        m_oTimer.Stop();
        m_oTimer.Start();
    }

    //*************************************************************************
    //  Method: Restore()
    //
    /// <summary>
    /// Immediately restores the original value of the Application.Calculation
    /// property.
    /// </summary>
    ///
    /// <remarks>
    /// If the restore timer has been started with a call to <see
    /// cref="StartRestoreTimer" />, the timer is stopped.
    /// </remarks>
    //*************************************************************************

    public void
    Restore()
    {
        AssertValid();

        m_oTimer.Stop();

        m_oApplication.Calculation = m_eOriginalCalculationValue;
    }

    //*************************************************************************
    //  Method: m_oTimer_Tick()
    //
    /// <summary>
    /// Handles the Tick event on the m_oTimer Timer.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    m_oTimer_Tick
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        Restore();
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
        Debug.Assert(m_oApplication != null);
        // m_eOriginalCalculationValue
        Debug.Assert(m_oTimer != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Application object to restore the Calcuation value on.

    Microsoft.Office.Interop.Excel.Application m_oApplication;

    /// Original Application.Calculation value.

    Microsoft.Office.Interop.Excel.XlCalculation m_eOriginalCalculationValue;

    /// Restore timer.

    protected System.Windows.Forms.Timer m_oTimer;
}
}
