
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Text;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: MinimumValueListBox
//
/// <summary>
/// ListBox in the <see cref="GroupByVertexAttributeDialog" /> that stores the
/// minimum value for each group.
/// </summary>
///
/// <remarks>
/// The type of the minimum group values is determined by the ExcelColumnFormat
/// the user has selected in the parent dialog.  If he has selected <see
/// cref="ExcelColumnFormat.Time "/>, for example, the minimum group values are
/// DateTimes that should be formatted as times.
///
/// <para>
/// MinimumValueListBox handles all the storage and formatting details by
/// using one of a family of "FormattableValue" classes as the item type:
/// FormattableTime, for example.  These classes contain the actual value --
/// DateTime in the FormattableTime case -- and implement a ToString() method
/// that the base-class ListBox calls when it displays the items.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class MinimumValueListBox : ListBox
{
    //*************************************************************************
    //  Constructor: MinimumValueListBox()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="MinimumValueListBox" />
    /// class.
    /// </summary>
    //*************************************************************************

    public MinimumValueListBox()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: AddItem()
    //
    /// <summary>
    /// Adds an item to the ListBox.
    /// </summary>
    ///
    /// <typeparam name="TItem">
    /// The type of the items stored in the ListBox.  Sample:
    /// FormattableNumber.
    /// </typeparam>
    ///
    /// <typeparam name="TContainedValue">
    /// The type of the value contained within TItem.  Sample: Double.
    /// </typeparam>
    ///
    /// <param name="item">
    /// The item to add to the ListBox.
    /// </param>
    ///
    /// <remarks>
    /// The item is added in ascending sort order.
    ///
    /// <para>
    /// This is an O(n) operation, where n is the number of items already in
    /// the ListBox.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    AddItem<TItem, TContainedValue>
    (
        TItem item
    )
        where TItem : FormattableValue<TContainedValue>

        where TContainedValue : IEquatable<TContainedValue>,
            IComparable<TContainedValue>, IFormattable
    {
        AssertValid();

        ListBox.ObjectCollection oItems = this.Items;
        Int32 iItems = oItems.Count;
        Int32 iInsertionIndex;

        for (iInsertionIndex = 0; iInsertionIndex < iItems; iInsertionIndex++)
        {
            Debug.Assert(oItems[iInsertionIndex] is TItem);

            TItem oFormattableValue = (TItem)oItems[iInsertionIndex];

            Int32 iCompareTo = item.CompareTo(oFormattableValue);

            if (iCompareTo == 0)
            {
                // The ListBox contains an item with the same contained value.
                // Don't add another such item.

                return;
            }

            if (iCompareTo < 0)
            {
                break;
            }
        }

        this.ClearSelected();
        oItems.Insert(iInsertionIndex, item);
        this.SelectedIndex = iInsertionIndex;
    }

    //*************************************************************************
    //  Method: GetAllContainedValues()
    //
    /// <summary>
    /// Gets all the contained values in the ListBox
    /// </summary>
    ///
    /// <typeparam name="TItem">
    /// The type of the items stored in the ListBox.  Sample:
    /// FormattableNumber.
    /// </typeparam>
    ///
    /// <typeparam name="TContainedValue">
    /// The type of the value contained within TItem.  Sample: Double.
    /// </typeparam>
    ///
    /// <returns>
    /// All the contained values in the ListBox.  The values are sorted in
    /// ascending order.
    /// </returns>
    //*************************************************************************

    public IEnumerable<TContainedValue>
    GetAllContainedValues<TItem, TContainedValue>()

        where TItem : FormattableValue<TContainedValue>

        where TContainedValue : IEquatable<TContainedValue>,
            IComparable<TContainedValue>, IFormattable
    {
        AssertValid();

        foreach (Object oItem in this.Items)
        {
            Debug.Assert(oItem is TItem);

            yield return ( ( (TItem)oItem ).ContainedValue );
        }
    }

    //*************************************************************************
    //  Method: ItemsToCultureInvariantString()
    //
    /// <summary>
    /// Converts the ListBox's items to a culture-invariant string appropriate
    /// for storing in the user's settings.
    /// </summary>
    ///
    /// <typeparam name="TItem">
    /// The type of the items stored in the ListBox.  Sample:
    /// FormattableNumber.
    /// </typeparam>
    ///
    /// <typeparam name="TContainedValue">
    /// The type of the value contained within TItem.  Sample: Double.
    /// </typeparam>
    ///
    /// <returns>
    /// A culture-invariant string.
    /// </returns>
    ///
    /// <remarks>
    /// Use <see cref="CultureInvariantStringToItems" /> to populate the
    /// ListBox with the saved settings.
    /// </remarks>
    //*************************************************************************

    public String
    ItemsToCultureInvariantString<TItem, TContainedValue>()

        where TItem : FormattableValue<TContainedValue>

        where TContainedValue : IEquatable<TContainedValue>,
            IComparable<TContainedValue>, IFormattable
    {
        AssertValid();

        StringBuilder oStringBuilder = new StringBuilder();

        foreach (Object oItem in this.Items)
        {
            Debug.Assert( oItem is FormattableValue<TContainedValue> );

            FormattableValue<TContainedValue> oFormattableValue =
                ( FormattableValue<TContainedValue> )oItem;

            if (oStringBuilder.Length > 0)
            {
                oStringBuilder.Append('\t');
            }

            oStringBuilder.Append(
                oFormattableValue.ToCultureInvariantString() );
        }

        return ( oStringBuilder.ToString() );
    }

    //*************************************************************************
    //  Method: CultureInvariantStringToItems()
    //
    /// <summary>
    /// Populates the ListBox with a a culture-invariant string retrieved from
    /// the user's settings.
    /// </summary>
    ///
    /// <typeparam name="TItem">
    /// The type of the items stored in the ListBox.  Sample:
    /// FormattableNumber.
    /// </typeparam>
    ///
    /// <typeparam name="TContainedValue">
    /// The type of the value contained within TItem.  Sample: Double.
    /// </typeparam>
    ///
    /// <param name="cultureInvariantString">
    /// A culture-invariant string created by ToCultureInvariantString.
    /// </param>
    //*************************************************************************

    public void
    CultureInvariantStringToItems<TItem, TContainedValue>
    (
        String cultureInvariantString
    )
        where TItem : FormattableValue<TContainedValue>, new()

        where TContainedValue : IEquatable<TContainedValue>,
            IComparable<TContainedValue>, IFormattable
    {
        Debug.Assert(cultureInvariantString != null);
        AssertValid();

        this.Items.Clear();

        foreach ( String sField in cultureInvariantString.Split('\t') )
        {
            TItem oTItem = new TItem();

            if ( oTItem.TryParseCultureInvariantString(sField) )
            {
                AddItem<TItem, TContainedValue>(oTItem);
            }
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
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}


//*****************************************************************************
//  Class: FormattableValue
//
/// <summary>
/// Represents an object that contains a value.
/// </summary>
///
/// <typeparam name="TContainedValue">
/// The type of the contained value.
/// </typeparam>
//*****************************************************************************

public abstract class FormattableValue<TContainedValue> : Object

    where TContainedValue : IEquatable<TContainedValue>,
        IComparable<TContainedValue>, IFormattable
{
    //*************************************************************************
    //  Constructor: FormattableValue()
    //
    /// <summary>
    /// Initializes a new instance of the FormattableValue class.
    /// </summary>
    ///
    /// <param name="valueToContain">
    /// The contained value.
    /// </param>
    //*************************************************************************

    public FormattableValue
    (
        TContainedValue valueToContain
    )
    {
        m_oContainedValue = valueToContain;

        // AssertValid();
    }

    //*************************************************************************
    //  Property: ContainedValue
    //
    /// <summary>
    /// Gets or sets the contained value.
    /// </summary>
    ///
    /// <value>
    /// The contained value.
    /// </value>
    //*************************************************************************

    public TContainedValue
    ContainedValue
    {
        get
        {
            AssertValid();

            return (m_oContainedValue);
        }

        set
        {
            m_oContainedValue = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: CompareTo()
    //
    /// <summary>
    /// Compares this object to another object of this type.
    /// </summary>
    ///
    /// <param name="otherObject">
    /// The other object to compare this object to.
    /// </param>
    /// 
    /// <returns>
    /// See IComparable.CompareTo().
    /// </returns>
    //*************************************************************************

    public Int32
    CompareTo
    (
        Object otherObject
    )
    {
        Debug.Assert(otherObject != null);
        Debug.Assert( otherObject is FormattableValue<TContainedValue> );
        AssertValid();

        FormattableValue<TContainedValue> oOtherFormattableValue =
            ( FormattableValue<TContainedValue> )otherObject;

        return ( this.ContainedValue.CompareTo(
            oOtherFormattableValue.ContainedValue) );
    }

    //*************************************************************************
    //  Method: ToCultureInvariantString()
    //
    /// <summary>
    /// Converts the contained value to a culture-invariant string.
    /// </summary>
    ///
    /// <returns>
    /// The culture-invariant string representation of the contained value.
    /// </returns>
    ///
    /// <remarks>
    /// The returned string can be "round-tripped."
    /// </remarks>
    //*************************************************************************

    public String
    ToCultureInvariantString()
    {
        AssertValid();

        return ( this.ContainedValue.ToString(this.CultureInvariantFormat,
            CultureInfo.InvariantCulture) );
    }

    //*************************************************************************
    //  Method: TryParseCultureInvariantString()
    //
    /// <summary>
    /// Attempts to set the contained value by parsing a culture-invariant
    /// string.
    /// </summary>
    ///
    /// <param name="cultureInvariantString">
    /// A culture-invariant string created by ToCultureInvariantString.
    /// </param>
    ///
    /// <returns>
    /// true if the string was successfully parsed.
    /// </returns>
    //*************************************************************************

    public abstract Boolean
    TryParseCultureInvariantString
    (
        String cultureInvariantString
    );

    //*************************************************************************
    //  Property: CultureInvariantFormat
    //
    /// <summary>
    /// Gets a format string to use for culture-invariant, round-trip
    /// formatting.
    /// </summary>
    ///
    /// <value>
    /// A format string.
    /// </value>
    //*************************************************************************

    protected abstract String
    CultureInvariantFormat
    {
        get;
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
    //  Protected constants
    //*************************************************************************

    /// The format string to use for culture-invariant DateTime formatting.

    protected const String DateTimeCultureInvariantFormat = "O";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The contained value.

    protected TContainedValue m_oContainedValue;
}


//*****************************************************************************
//  Class: FormattableNumber
//
/// <summary>
/// Contains a number from an Excel column with the format
/// ExcelColumnFormat.Number.
/// </summary>
//*****************************************************************************

public class FormattableNumber : FormattableValue<Double>
{
    //*************************************************************************
    //  Constructor: FormattableNumber()
    //
    /// <overloads>
    /// Initializes a new instance of the FormattableNumber class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the FormattableNumber class using a
    /// default contained value.
    /// </summary>
    //*************************************************************************

    public FormattableNumber()
    : this(Double.MinValue)
    {
        // (Do nothing.)

        // AssertValid();
    }

    //*************************************************************************
    //  Constructor: FormattableNumber()
    //
    /// <summary>
    /// Initializes a new instance of the FormattableNumber class using a
    /// specified contained value.
    /// </summary>
    ///
    /// <param name="number">
    /// The contained value.
    /// </param>
    //*************************************************************************

    public FormattableNumber
    (
        Double number
    )
    : base(number)
    {
        // (Do nothing.)

        // AssertValid();
    }

    //*************************************************************************
    //  Method: ToString()
    //
    /// <summary>
    /// Converts the contained value to a string using the default format
    /// provider.
    /// </summary>
    ///
    /// <returns>
    /// The string representation of the contained value.
    /// </returns>
    //*************************************************************************

    public override String
    ToString()
    {
        AssertValid();

        return ( this.ContainedValue.ToString() );
    }

    //*************************************************************************
    //  Method: TryParseCultureInvariantString()
    //
    /// <summary>
    /// Attempts to set the contained value by parsing a culture-invariant
    /// string.
    /// </summary>
    ///
    /// <param name="cultureInvariantString">
    /// A culture-invariant string created by ToCultureInvariantString.
    /// </param>
    ///
    /// <returns>
    /// true if the string was successfully parsed.
    /// </returns>
    //*************************************************************************

    public override Boolean
    TryParseCultureInvariantString
    (
        String cultureInvariantString
    )
    {
        AssertValid();

        Double dDouble;

        if ( Double.TryParse(cultureInvariantString, NumberStyles.Any,
            CultureInfo.InvariantCulture, out dDouble) )
        {
            this.ContainedValue = dDouble;
            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Property: CultureInvariantFormat
    //
    /// <summary>
    /// Gets a format string to use for culture-invariant, round-trip
    /// formatting.
    /// </summary>
    ///
    /// <value>
    /// A format string.
    /// </value>
    //*************************************************************************

    protected override String
    CultureInvariantFormat
    {
        get
        {
            AssertValid();

            return ("R");
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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}


//*****************************************************************************
//  Class: FormattableDateTime
//
/// <summary>
/// Base class for classes that contain a DateTime from an Excel column.
/// </summary>
//*****************************************************************************

public abstract class FormattableDateTime : FormattableValue<DateTime>
{
    //*************************************************************************
    //  Constructor: FormattableDateTime()
    //
    /// <overloads>
    /// Initializes a new instance of the FormattableDateTime class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the FormattableDateTime class using a
    /// default contained value.
    /// </summary>
    //*************************************************************************

    public FormattableDateTime()
    : this(DateTime.MinValue)
    {
        // (Do nothing.)

        // AssertValid();
    }

    //*************************************************************************
    //  Constructor: FormattableDateTime()
    //
    /// <summary>
    /// Initializes a new instance of the FormattableDateTime class using a
    /// specified contained value.
    /// </summary>
    ///
    /// <param name="dateTime">
    /// The contained value.
    /// </param>
    //*************************************************************************

    public FormattableDateTime
    (
        DateTime dateTime
    )
    : base(dateTime)
    {
        // (Do nothing.)

        // AssertValid();
    }

    //*************************************************************************
    //  Method: ToString()
    //
    /// <summary>
    /// Converts the contained value to a string using the default format
    /// provider.
    /// </summary>
    ///
    /// <returns>
    /// The string representation of the contained value.
    /// </returns>
    //*************************************************************************

    public override String
    ToString()
    {
        AssertValid();

        String sFormat = SimpleDateTimeFormatUtil.GetFormatPattern(
            this.SimpleDateTimeFormat);

        return ( this.ContainedValue.ToString(sFormat, null) );
    }

    //*************************************************************************
    //  Property: CultureInvariantFormat
    //
    /// <summary>
    /// Gets a format string to use for culture-invariant, round-trip
    /// formatting.
    /// </summary>
    ///
    /// <value>
    /// A format string.
    /// </value>
    //*************************************************************************

    protected override String
    CultureInvariantFormat
    {
        get
        {
            AssertValid();

            return ("O");
        }
    }

    //*************************************************************************
    //  Property: SimpleDateTimeFormat
    //
    /// <summary>
    /// Gets the simple date/time format to use when formatting a string.
    /// </summary>
    ///
    /// <value>
    /// The simple date/time format to use, as a SimpleDateTimeFormat.
    /// </value>
    //*************************************************************************

    protected abstract SimpleDateTimeFormat
    SimpleDateTimeFormat
    {
        get;
    }

    //*************************************************************************
    //  Method: TryParseCultureInvariantString()
    //
    /// <summary>
    /// Attempts to set the contained value by parsing a culture-invariant
    /// string.
    /// </summary>
    ///
    /// <param name="cultureInvariantString">
    /// A culture-invariant string created by ToCultureInvariantString.
    /// </param>
    ///
    /// <returns>
    /// true if the string was successfully parsed.
    /// </returns>
    //*************************************************************************

    public override Boolean
    TryParseCultureInvariantString
    (
        String cultureInvariantString
    )
    {
        AssertValid();

        DateTime oDateTime;

        if ( DateTime.TryParse(cultureInvariantString,
            CultureInfo.InvariantCulture, DateTimeStyles.None, out oDateTime) )
        {
            this.ContainedValue = oDateTime;
            return (true);
        }

        return (false);
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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}


//*****************************************************************************
//  Class: FormattableDate
//
/// <summary>
/// Contains a DateTime from an Excel column with the format
/// ExcelColumnFormat.Date.
/// </summary>
//*****************************************************************************

public class FormattableDate : FormattableDateTime
{
    //*************************************************************************
    //  Constructor: FormattableDate()
    //
    /// <overloads>
    /// Initializes a new instance of the FormattableDate class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the FormattableDate class using a default
    /// contained value.
    /// </summary>
    //*************************************************************************

    public FormattableDate()
    : this(DateTime.MinValue)
    {
        // (Do nothing.)

        // AssertValid();
    }

    //*************************************************************************
    //  Constructor: FormattableDate()
    //
    /// <summary>
    /// Initializes a new instance of the FormattableDate class using a
    /// specified contained value.
    /// </summary>
    ///
    /// <param name="dateTime">
    /// The contained value.
    /// </param>
    //*************************************************************************

    public FormattableDate
    (
        DateTime dateTime
    )
    : base(dateTime)
    {
        // (Do nothing.)

        // AssertValid();
    }

    //*************************************************************************
    //  Property: SimpleDateTimeFormat
    //
    /// <summary>
    /// Gets the simple date/time format to use when formatting a string.
    /// </summary>
    ///
    /// <value>
    /// The simple date/time format to use, as a SimpleDateTimeFormat.
    /// </value>
    //*************************************************************************

    protected override SimpleDateTimeFormat
    SimpleDateTimeFormat
    {
        get
        {
            AssertValid();

            return (SimpleDateTimeFormat.Date);
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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}


//*****************************************************************************
//  Class: FormattableTime
//
/// <summary>
/// Contains a DateTime from an Excel column with the format
/// ExcelColumnFormat.Time.
/// </summary>
//*****************************************************************************

public class FormattableTime : FormattableDateTime
{
    //*************************************************************************
    //  Constructor: FormattableTime()
    //
    /// <overloads>
    /// Initializes a new instance of the FormattableTime class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the FormattableTime class using a default
    /// contained value.
    /// </summary>
    //*************************************************************************

    public FormattableTime()
    : this(DateTime.MinValue)
    {
        // (Do nothing.)

        // AssertValid();
    }

    //*************************************************************************
    //  Constructor: FormattableTime()
    //
    /// <summary>
    /// Initializes a new instance of the FormattableTime class using a
    /// specified contained value.
    /// </summary>
    ///
    /// <param name="dateTime">
    /// The contained value.
    /// </param>
    //*************************************************************************

    public FormattableTime
    (
        DateTime dateTime
    )
    : base(dateTime)
    {
        // (Do nothing.)

        // AssertValid();
    }

    //*************************************************************************
    //  Property: SimpleDateTimeFormat
    //
    /// <summary>
    /// Gets the simple date/time format to use when formatting a string.
    /// </summary>
    ///
    /// <value>
    /// The simple date/time format to use, as a SimpleDateTimeFormat.
    /// </value>
    //*************************************************************************

    protected override SimpleDateTimeFormat
    SimpleDateTimeFormat
    {
        get
        {
            AssertValid();

            return (SimpleDateTimeFormat.Time);
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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}


//*****************************************************************************
//  Class: FormattableDateAndTime
//
/// <summary>
/// Contains a DateTime from an Excel column with the format
/// ExcelColumnFormat.DateAndTime.
/// </summary>
//*****************************************************************************

public class FormattableDateAndTime : FormattableDateTime
{
    //*************************************************************************
    //  Constructor: FormattableDateAndTime()
    //
    /// <overloads>
    /// Initializes a new instance of the FormattableDateAndTime class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the FormattableDateAndTime class using a
    /// default contained value.
    /// </summary>
    //*************************************************************************

    public FormattableDateAndTime()
    : this(DateTime.MinValue)
    {
        // (Do nothing.)

        // AssertValid();
    }

    //*************************************************************************
    //  Constructor: FormattableDateAndTime()
    //
    /// <summary>
    /// Initializes a new instance of the FormattableDateAndTime class using a
    /// specified contained value.
    /// </summary>
    ///
    /// <param name="dateTime">
    /// The contained value.
    /// </param>
    //*************************************************************************

    public FormattableDateAndTime
    (
        DateTime dateTime
    )
    : base(dateTime)
    {
        // (Do nothing.)

        // AssertValid();
    }

    //*************************************************************************
    //  Property: SimpleDateTimeFormat
    //
    /// <summary>
    /// Gets the simple date/time format to use when formatting a string.
    /// </summary>
    ///
    /// <value>
    /// The simple date/time format to use, as a SimpleDateTimeFormat.
    /// </value>
    //*************************************************************************

    protected override SimpleDateTimeFormat
    SimpleDateTimeFormat
    {
        get
        {
            AssertValid();

            return (SimpleDateTimeFormat.DateAndTime);
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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
