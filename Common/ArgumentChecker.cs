
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: ArgumentChecker
//
/// <summary>
/// Contains methods that check method arguments and property values.
/// </summary>
///
/// <remarks>
/// This can be used by a class that wants to check values passed to its
/// property setters and arguments passed to its methods.  The CheckXXX()
/// methods in this class throw an exception if a parameter value or method
/// argument is invalid.
///
/// <para>
/// This class is marked internal to avoid compiler error CS0433 ("The
/// type ... exists in both ...") when an executable references two or more
/// assemblies that use this class.
/// </para>
///
/// </remarks>
//*****************************************************************************

internal class ArgumentChecker : Object
{
    //*************************************************************************
    //  Constructor: ArgumentChecker()
    //
    /// <summary>
    /// Initializes a new instance of the ArgumentChecker class.
    /// </summary>
	///
	/// <param name="sOwnerClassName">
	/// Class name of the object that created this object.  The class name gets
	/// prepended to all exception messages.  Sample: "MyClass".
	/// </param>
    //*************************************************************************

    public ArgumentChecker
	(
		String sOwnerClassName
	)
    {
		m_sOwnerClassName = sOwnerClassName;

		AssertValid();
    }

	//*************************************************************************
	//	Method: CheckPropertyNotNull()
	//
	/// <summary>
	/// Throws an exception if a property value is null.
	/// </summary>
	///
	/// <param name="sPropertyName">
	/// Name of the property.
	/// </param>
	///
	/// <param name="oPropertyValue">
	/// Property value to check.
	/// </param>
	//*************************************************************************

	public void
	CheckPropertyNotNull
	(
		String sPropertyName,
		Object oPropertyValue
	)
	{
		AssertValid();
		Debug.Assert(sPropertyName != null);
		Debug.Assert(sPropertyName.Length > 0);

		if (oPropertyValue == null)
		{
			ThrowPropertyException(sPropertyName,

				"Can't be null."
				);
		}
	}

    //*************************************************************************
    //  Method: CheckPropertyNotEmpty()
    //
    /// <summary>
	/// Throws an exception if a string property value is null or has a length
	/// of zero.
    /// </summary>
	///
	/// <param name="sPropertyName">
	/// Name of the string property.
	/// </param>
	///
	/// <param name="sPropertyValue">
	/// Property value to check.
	/// </param>
    //*************************************************************************

    public void
    CheckPropertyNotEmpty
	(
		String sPropertyName,
		String sPropertyValue
	)
    {
		AssertValid();
		Debug.Assert(sPropertyName != null);
		Debug.Assert(sPropertyName.Length > 0);

		CheckPropertyNotNull(sPropertyName, sPropertyValue);

		if (sPropertyValue.Length == 0)
		{
			ThrowPropertyException(sPropertyName,

				"Must have a length greater than zero."
				);
		}
    }

    //*************************************************************************
    //  Method: CheckPropertyPositive()
    //
    /// <summary>
	/// Throws an exception if an Int32 property value is not positive.
    /// </summary>
	///
	/// <param name="sPropertyName">
	/// Name of the property.
	/// </param>
	///
	/// <param name="iPropertyValue">
	/// Property value to check.
	/// </param>
    //*************************************************************************

    public void
    CheckPropertyPositive
	(
		String sPropertyName,
		Int32 iPropertyValue
	)
    {
		AssertValid();
		Debug.Assert(sPropertyName != null);
		Debug.Assert(sPropertyName.Length > 0);

		if (iPropertyValue <= 0)
		{
			ThrowPropertyException(sPropertyName,

				"Must be greater than zero."
				);
		}
    }

    //*************************************************************************
    //  Method: CheckPropertyPositive()
    //
    /// <summary>
	/// Throws an exception if a Decimal property value is not positive.
    /// </summary>
	///
	/// <param name="sPropertyName">
	/// Name of the property.
	/// </param>
	///
	/// <param name="decPropertyValue">
	/// Property value to check.
	/// </param>
    //*************************************************************************

    public void
    CheckPropertyPositive
	(
		String sPropertyName,
		Decimal decPropertyValue
	)
    {
		AssertValid();
		Debug.Assert(sPropertyName != null);
		Debug.Assert(sPropertyName.Length > 0);

		if (decPropertyValue <= 0)
		{
			ThrowPropertyException(sPropertyName,

				"Must be greater than zero."
				);
		}
    }

    //*************************************************************************
    //  Method: CheckPropertyNotNegative()
    //
    /// <summary>
	/// Throws an exception if an Int32 property value is negative.
    /// </summary>
	///
	/// <param name="sPropertyName">
	/// Name of the property.
	/// </param>
	///
	/// <param name="iPropertyValue">
	/// Property value to check.
	/// </param>
    //*************************************************************************

    public void
    CheckPropertyNotNegative
	(
		String sPropertyName,
		Int32 iPropertyValue
	)
    {
		AssertValid();
		Debug.Assert(sPropertyName != null);
		Debug.Assert(sPropertyName.Length > 0);

		if (iPropertyValue < 0)
		{
			ThrowPropertyException(sPropertyName,

				"Must be greater than or equal to zero."
				);
		}
    }

    //*************************************************************************
    //  Method: CheckPropertyNotEqual()
    //
    /// <summary>
	/// Throws an exception if a string property is equal to a specified
	/// invalid value.
    /// </summary>
	///
	/// <param name="sPropertyName">
	/// Name of the property.
	/// </param>
	///
	/// <param name="sPropertyValue">
	/// Property value to check.
	/// </param>
	///
	/// <param name="sInvalidValue">
	/// Invalid value for <paramref name="sPropertyValue" />.  Can't be null.
	/// </param>
    //*************************************************************************

    public void
    CheckPropertyNotEqual
	(
		String sPropertyName,
		String sPropertyValue,
		String sInvalidValue
	)
    {
		AssertValid();
		Debug.Assert(sPropertyName != null);
		Debug.Assert(sPropertyName.Length > 0);
		Debug.Assert(sInvalidValue != null);

		if (sPropertyValue == sInvalidValue)
		{
			ThrowPropertyException(sPropertyName, String.Format(
			
				"Can't be {0}."
				,
				sInvalidValue
				) );
		}
    }

    //*************************************************************************
    //  Method: CheckPropertyNotEqual()
    //
    /// <summary>
	/// Throws an exception if an Int32 property is equal to a specified
	/// invalid value.
    /// </summary>
	///
	/// <param name="sPropertyName">
	/// Name of the property.
	/// </param>
	///
	/// <param name="iPropertyValue">
	/// Property value to check.
	/// </param>
	///
	/// <param name="iInvalidValue">
	/// Invalid value for <paramref name="iPropertyValue" />.
	/// </param>
    //*************************************************************************

    public void
    CheckPropertyNotEqual
	(
		String sPropertyName,
		Int32 iPropertyValue,
		Int32 iInvalidValue
	)
    {
		AssertValid();
		Debug.Assert(sPropertyName != null);
		Debug.Assert(sPropertyName.Length > 0);

		if (iPropertyValue == iInvalidValue)
		{
			ThrowPropertyException(sPropertyName, String.Format(
			
				"Can't be {0}."
				,
				iInvalidValue
				) );
		}
    }

	//*************************************************************************
	//	Method: CheckPropertyInRange
	//
	/// <summary>
	/// Throws an exception if an Int32 property value is not within a
	/// specified range.
	/// </summary>
	///
	/// <param name="sPropertyName">
	/// Name of the property.
	/// </param>
	///
	/// <param name="iPropertyValue">
	/// Property value to check.
	/// </param>
	///
	/// <param name="iMinimumValidValue">
	/// Minimum allowed value.
	/// </param>
	///
	/// <param name="iMaximumValidValue">
	/// Maximum allowed value.
	/// </param>
	//*************************************************************************

	public void
	CheckPropertyInRange
	(
		String sPropertyName,
		Int32 iPropertyValue,
		Int32 iMinimumValidValue,
		Int32 iMaximumValidValue
	)
	{
		AssertValid();
		Debug.Assert(sPropertyName != null);
		Debug.Assert(sPropertyName.Length > 0);
		Debug.Assert(iMaximumValidValue >= iMinimumValidValue);

		if (iPropertyValue < iMinimumValidValue ||
			iPropertyValue > iMaximumValidValue)
		{
			ThrowPropertyException(sPropertyName, String.Format(
				
				"Must be between {0} and {1}."
				,
				iMinimumValidValue, iMaximumValidValue
				) );
		}
	}

	//*************************************************************************
	//	Method: CheckPropertyInRange
	//
	/// <summary>
	/// Throws an exception if a Single property value is not within a
	/// specified range.
	/// </summary>
	///
	/// <param name="sPropertyName">
	/// Name of the property.
	/// </param>
	///
	/// <param name="fPropertyValue">
	/// Property value to check.
	/// </param>
	///
	/// <param name="fMinimumValidValue">
	/// Minimum allowed value.
	/// </param>
	///
	/// <param name="fMaximumValidValue">
	/// Maximum allowed value.
	/// </param>
	//*************************************************************************

	public void
	CheckPropertyInRange
	(
		String sPropertyName,
		Single fPropertyValue,
		Single fMinimumValidValue,
		Single fMaximumValidValue
	)
	{
		AssertValid();
		Debug.Assert(sPropertyName != null);
		Debug.Assert(sPropertyName.Length > 0);
		Debug.Assert(fMaximumValidValue >= fMinimumValidValue);

		if (fPropertyValue < fMinimumValidValue ||
			fPropertyValue > fMaximumValidValue)
		{
			ThrowPropertyException(sPropertyName, String.Format(
				
				"Must be between {0} and {1}."
				,
				fMinimumValidValue, fMaximumValidValue
				) );
		}
	}

	//*************************************************************************
	//	Method: CheckPropertyIsDefined
	//
	/// <summary>
	/// Throws an exception if a property value is not defined within a
	/// specified enumeration.
	/// </summary>
	///
	/// <param name="sPropertyName">
	/// Name of the property.
	/// </param>
	///
	/// <param name="oPropertyValue">
	/// Property value to check.
	/// </param>
	///
	/// <param name="oEnumType">
	/// Type of the enumeration that <paramref name="oPropertyValue" /> is
	/// supposed to be defined within.
	/// </param>
	//*************************************************************************

	public void
	CheckPropertyIsDefined
	(
		String sPropertyName,
		Object oPropertyValue,
		Type oEnumType
	)
	{
		AssertValid();
		Debug.Assert(sPropertyName != null);
		Debug.Assert(sPropertyName.Length > 0);

		if ( !Enum.IsDefined(oEnumType, oPropertyValue) )
		{
			ThrowPropertyException(sPropertyName, String.Format(
				
				"Must be a member of the {0} enumeration."
				,
				oEnumType.Name
				) );
		}
	}

	//*************************************************************************
	//	Method: CheckArgumentNotNull()
	//
	/// <summary>
	/// Throws an exception if a method argument is null.
	/// </summary>
	///
	/// <param name="sMethodName">
	/// Name of the method calling this method.
	/// </param>
	///
	/// <param name="sArgumentName">
	/// Name of the argument to check.
	/// </param>
	///
	/// <param name="oArgumentValue">
	/// Argument to check.
	/// </param>
	//*************************************************************************

	public void
	CheckArgumentNotNull
	(
		String sMethodName,
		String sArgumentName,
		Object oArgumentValue
	)
	{
		AssertValid();
		Debug.Assert(sMethodName != null);
		Debug.Assert(sMethodName.Length > 0);
		Debug.Assert(sArgumentName != null);
		Debug.Assert(sArgumentName.Length > 0);

		if (oArgumentValue == null)
		{
			Debug.Assert(false);

			throw new ArgumentNullException(

				sArgumentName,

				String.Format(
					"{0}.{1}: {2} argument can't be null.",
					m_sOwnerClassName, sMethodName, sArgumentName
					)
				);
		}
	}

    //*************************************************************************
    //  Method: CheckArgumentNotEmpty()
    //
    /// <summary>
	/// Throws an exception if a string method argument is null or has a length
	/// of zero.
    /// </summary>
	///
	/// <param name="sMethodName">
	/// Name of the method calling this method.
	/// </param>
	///
	/// <param name="sArgumentName">
	/// Name of the argument to check.
	/// </param>
	///
	/// <param name="sArgumentValue">
	/// Argument value to check.
	/// </param>
    //*************************************************************************

    public void
    CheckArgumentNotEmpty
	(
		String sMethodName,
		String sArgumentName,
		String sArgumentValue
	)
    {
		AssertValid();
		Debug.Assert(sMethodName != null);
		Debug.Assert(sMethodName.Length > 0);
		Debug.Assert(sArgumentName != null);
		Debug.Assert(sArgumentName.Length > 0);

		CheckArgumentNotNull(sMethodName, sArgumentName, sArgumentValue);

		if (sArgumentValue.Length == 0)
		{
			String sMessageDetails = String.Format(

				"{0} argument must have a length greater than zero."
				,
				sArgumentName
				);

			ThrowArgumentException(
				sMethodName, sArgumentName, sMessageDetails);
		}
    }

	//*************************************************************************
	//	Method: CheckArgumentPositive
	//
	/// <summary>
	/// Throws an exception if an Int32 method argument is not positive.
	/// </summary>
	///
	/// <param name="sMethodName">
	/// Name of the method calling this method.
	/// </param>
	///
	/// <param name="sArgumentName">
	/// Name of the argument to check.
	/// </param>
	///
	/// <param name="iArgumentValue">
	/// Argument to check.
	/// </param>
	//*************************************************************************

	public void
	CheckArgumentPositive
	(
		String sMethodName,
		String sArgumentName,
		Int32 iArgumentValue
	)
	{
		AssertValid();
		Debug.Assert(sMethodName != null);
		Debug.Assert(sMethodName.Length > 0);
		Debug.Assert(sArgumentName != null);
		Debug.Assert(sArgumentName.Length > 0);

		CheckArgumentPositive(sMethodName, sArgumentName,
			(Double)iArgumentValue);
	}

	//*************************************************************************
	//	Method: CheckArgumentPositive
	//
	/// <summary>
	/// Throws an exception if an Int64 method argument is not positive.
	/// </summary>
	///
	/// <param name="sMethodName">
	/// Name of the method calling this method.
	/// </param>
	///
	/// <param name="sArgumentName">
	/// Name of the argument to check.
	/// </param>
	///
	/// <param name="lArgumentValue">
	/// Argument to check.
	/// </param>
	//*************************************************************************

	public void
	CheckArgumentPositive
	(
		String sMethodName,
		String sArgumentName,
		Int64 lArgumentValue
	)
	{
		AssertValid();
		Debug.Assert(sMethodName != null);
		Debug.Assert(sMethodName.Length > 0);
		Debug.Assert(sArgumentName != null);
		Debug.Assert(sArgumentName.Length > 0);

		CheckArgumentPositive(sMethodName, sArgumentName,
			(Double)lArgumentValue);
	}

	//*************************************************************************
	//	Method: CheckArgumentPositive
	//
	/// <summary>
	/// Throws an exception if a Double method argument is not positive.
	/// </summary>
	///
	/// <param name="sMethodName">
	/// Name of the method calling this method.
	/// </param>
	///
	/// <param name="sArgumentName">
	/// Name of the argument to check.
	/// </param>
	///
	/// <param name="dArgumentValue">
	/// Argument to check.
	/// </param>
	//*************************************************************************

	public void
	CheckArgumentPositive
	(
		String sMethodName,
		String sArgumentName,
		Double dArgumentValue
	)
	{
		AssertValid();
		Debug.Assert(sMethodName != null);
		Debug.Assert(sMethodName.Length > 0);
		Debug.Assert(sArgumentName != null);
		Debug.Assert(sArgumentName.Length > 0);

		if (dArgumentValue <= 0)
		{
			String sMessageDetails = String.Format(

				"{0} argument must be greater than zero."
				,
				sArgumentName
				);

			ThrowArgumentException(
				sMethodName, sArgumentName, sMessageDetails);
		}
	}

	//*************************************************************************
	//	Method: CheckArgumentNotNegative
	//
	/// <summary>
	/// Throws an exception if an Int32 method argument is negative.
	/// </summary>
	///
	/// <param name="sMethodName">
	/// Name of the method calling this method.
	/// </param>
	///
	/// <param name="sArgumentName">
	/// Name of the argument to check.
	/// </param>
	///
	/// <param name="iArgumentValue">
	/// Argument to check.
	/// </param>
	//*************************************************************************

	public void
	CheckArgumentNotNegative
	(
		String sMethodName,
		String sArgumentName,
		Int32 iArgumentValue
	)
	{
		AssertValid();
		Debug.Assert(sMethodName != null);
		Debug.Assert(sMethodName.Length > 0);
		Debug.Assert(sArgumentName != null);
		Debug.Assert(sArgumentName.Length > 0);

		CheckArgumentNotNegative(
			sMethodName, sArgumentName, (Int64)iArgumentValue);
	}

	//*************************************************************************
	//	Method: CheckArgumentNotNegative
	//
	/// <summary>
	/// Throws an exception if an Int64 method argument is negative.
	/// </summary>
	///
	/// <param name="sMethodName">
	/// Name of the method calling this method.
	/// </param>
	///
	/// <param name="sArgumentName">
	/// Name of the argument to check.
	/// </param>
	///
	/// <param name="lArgumentValue">
	/// Argument to check.
	/// </param>
	//*************************************************************************

	public void
	CheckArgumentNotNegative
	(
		String sMethodName,
		String sArgumentName,
		Int64 lArgumentValue
	)
	{
		AssertValid();
		Debug.Assert(sMethodName != null);
		Debug.Assert(sMethodName.Length > 0);
		Debug.Assert(sArgumentName != null);
		Debug.Assert(sArgumentName.Length > 0);

		if (lArgumentValue < 0)
		{
			String sMessageDetails = String.Format(

				"{0} argument must be greater than or equal to zero."
				,
				sArgumentName
				);

			ThrowArgumentException(
				sMethodName, sArgumentName, sMessageDetails);
		}
	}

	//*************************************************************************
	//	Method: CheckArgumentIsDefined
	//
	/// <summary>
	/// Throws an exception if a method argument is not defined within a
	/// specified enumeration.
	/// </summary>
	///
	/// <param name="sMethodName">
	/// Name of the method calling this method.
	/// </param>
	///
	/// <param name="sArgumentName">
	/// Name of the argument to check.
	/// </param>
	///
	/// <param name="oArgumentValue">
	/// Argument value to check.
	/// </param>
	///
	/// <param name="oEnumType">
	/// Type of the enumeration that <paramref name="oArgumentValue" /> is
	/// supposed to be defined within.
	/// </param>
	//*************************************************************************

	public void
	CheckArgumentIsDefined
	(
		String sMethodName,
		String sArgumentName,
		Object oArgumentValue,
		Type oEnumType
	)
	{
		AssertValid();
		Debug.Assert(sMethodName != null);
		Debug.Assert(sMethodName.Length > 0);
		Debug.Assert(sArgumentName != null);
		Debug.Assert(sArgumentName.Length > 0);

		if ( !Enum.IsDefined(oEnumType, oArgumentValue) )
		{
			ThrowArgumentException(sMethodName, sArgumentName, String.Format(
				
				"Must be a member of the {0} enumeration."
				,
				oEnumType.Name
				) );
		}
	}

	//*************************************************************************
	//	Method: ThrowPropertyException
	//
	/// <overloads>
	/// Throws a new <see cref="ApplicationException" /> when the value passed
	/// to a property setter is invalid.
	/// </overloads>
	///
	/// <summary>
	/// Throws a new <see cref="ApplicationException" /> when the value passed
	/// to a property setter is invalid and there is an inner exception.
	/// </summary>
	///
	/// <param name="sPropertyName">
	/// Name of the property calling this method.
	/// </param>
	///
	/// <param name="sMessageDetails">
	/// Exception message details.  Gets appended to the class and property
	/// names.  See the example below.
	/// </param>
	///
	/// <param name="oInnerException">
	/// Inner exception, or null if there is none.
	/// </param>
	///
	/// <remarks>
	/// Call this method when the value passed to a property setter is invalid.
	/// It throws an exception whose message automatically includes the class
	/// and property names.
	/// </remarks>
	///
	/// <example>
	/// If the class name that was passed to the <see cref="ArgumentChecker" />
	/// constructor is "MyClass", <paramref name="sPropertyName" /> is
	/// "MyProperty", and <paramref name="sMessageDetails" /> is "The value
	/// can't be null.", then the new exception's message is
	/// "MyClass.MyProperty: The value can't be null."
	/// </example>
	//*************************************************************************

	public void
	ThrowPropertyException
	(
		String sPropertyName,
		String sMessageDetails,
		Exception oInnerException
	)
	{
		AssertValid();
		Debug.Assert(sPropertyName != null);
		Debug.Assert(sPropertyName.Length > 0);
		Debug.Assert(sMessageDetails != null);
		Debug.Assert(sMessageDetails.Length > 0);

		Debug.Assert(false);

		throw new ApplicationException( String.Format(

			"{0}.{1}: {2}"
			,
			m_sOwnerClassName,
			sPropertyName,
			sMessageDetails
			),

			oInnerException
			);
	}

	//*************************************************************************
	//	Method: ThrowPropertyException
	//
	/// <summary>
	/// Throws a new <see cref="ApplicationException" /> when the value passed
	/// to a property setter is invalid and there is no inner exception.
	/// </summary>
	///
	/// <param name="sPropertyName">
	/// Name of the property calling this method.
	/// </param>
	///
	/// <param name="sMessageDetails">
	/// Exception message details.  Gets appended to the class and property
	/// names.  See the example below.
	/// </param>
	///
	/// <remarks>
	/// Call this method when the value passed to a property setter is invalid.
	/// It throws an exception whose message automatically includes the class
	/// and property names.
	/// </remarks>
	///
	/// <example>
	/// If the class name that was passed to the <see cref="ArgumentChecker" />
	/// constructor is "MyClass", <paramref name="sPropertyName" /> is
	/// "MyProperty", and <paramref name="sMessageDetails" /> is "The value
	/// can't be null.", then the new exception's message is
	/// "MyClass.MyProperty: The value can't be null."
	/// </example>
	//*************************************************************************

	public void
	ThrowPropertyException
	(
		String sPropertyName,
		String sMessageDetails
	)
	{
		AssertValid();
		Debug.Assert(sPropertyName != null);
		Debug.Assert(sPropertyName.Length > 0);
		Debug.Assert(sMessageDetails != null);
		Debug.Assert(sMessageDetails.Length > 0);

		ThrowPropertyException(sPropertyName, sMessageDetails, null);
	}

	//*************************************************************************
	//	Method: ThrowArgumentException
	//
	/// <overloads>
	/// Throws a new <see cref="ArgumentException" /> when a method argument is
	/// invalid.
	/// </overloads>
	///
	/// <summary>
	/// Throws a new <see cref="ArgumentException" /> when a method argument is
	/// invalid and there is an inner exception.
	/// </summary>
	///
	/// <param name="sMethodName">
	/// Name of the method calling this method.
	/// </param>
	///
	/// <param name="sArgumentName">
	/// Name of the invalid argument.
	/// </param>
	///
	/// <param name="sMessageDetails">
	/// Exception message details.  Gets appended to the class and method
	/// names.  See the example below.
	/// </param>
	///
	/// <param name="oInnerException">
	/// Inner exception, or null if there is none.
	/// </param>
	///
	/// <remarks>
	/// Call this method when a method argument is invalid.  It throws an
	/// exception whose message automatically includes the class and method
	/// names.
	/// </remarks>
	///
	/// <example>
	/// If the class name that was passed to the <see cref="ArgumentChecker" />
	/// constructor is "MyClass", <paramref name="sMethodName" /> is
	/// "MyMethod", <paramref name="sArgumentName" /> is "TheArgument", and
	/// <paramref name="sMessageDetails" /> is "TheArgument can't be empty.",
	/// then the new exception's message is "MyClass.MyMethod: TheArgument
	/// can't be empty."
	/// </example>
	//*************************************************************************

	public void
	ThrowArgumentException
	(
		String sMethodName,
		String sArgumentName,
		String sMessageDetails,
		Exception oInnerException
	)
	{
		AssertValid();
		Debug.Assert(sMethodName != null);
		Debug.Assert(sMethodName.Length > 0);
		Debug.Assert(sArgumentName != null);
		Debug.Assert(sArgumentName.Length > 0);
		Debug.Assert(sMessageDetails != null);
		Debug.Assert(sMessageDetails.Length > 0);

		Debug.Assert(false);

		throw new ArgumentException( String.Format(

			"{0}.{1}: {2}"
			,
			m_sOwnerClassName,
			sMethodName,
			sMessageDetails
			),
			sArgumentName,
			oInnerException
			);
	}

	//*************************************************************************
	//	Method: ThrowArgumentException
	//
	/// <summary>
	/// Throws a new <see cref="ArgumentException" /> when a method argument is
	/// invalid and there is no inner exception.
	/// </summary>
	///
	/// <param name="sMethodName">
	/// Name of the method calling this method.
	/// </param>
	///
	/// <param name="sArgumentName">
	/// Name of the invalid argument.
	/// </param>
	///
	/// <param name="sMessageDetails">
	/// Exception message details.  Gets appended to the class and method
	/// names.  See the example below.
	/// </param>
	///
	/// <remarks>
	/// Call this method when a method argument is invalid.  It throws an
	/// exception whose message automatically includes the class and method
	/// names.
	/// </remarks>
	///
	/// <example>
	/// If the class name that was passed to the <see cref="ArgumentChecker" />
	/// constructor is "MyClass", <paramref name="sMethodName" /> is
	/// "MyMethod", <paramref name="sArgumentName" /> is "TheArgument", and
	/// <paramref name="sMessageDetails" /> is "TheArgument can't be empty.",
	/// then the new exception's message is "MyClass.MyMethod: TheArgument
	/// can't be empty."
	/// </example>
	//*************************************************************************

	public void
	ThrowArgumentException
	(
		String sMethodName,
		String sArgumentName,
		String sMessageDetails
	)
	{
		AssertValid();
		Debug.Assert(sMethodName != null);
		Debug.Assert(sMethodName.Length > 0);
		Debug.Assert(sArgumentName != null);
		Debug.Assert(sArgumentName.Length > 0);
		Debug.Assert(sMessageDetails != null);
		Debug.Assert(sMessageDetails.Length > 0);

		ThrowArgumentException(sMethodName, sArgumentName, sMessageDetails,
			null);
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
		Debug.Assert(m_sOwnerClassName != null);
		Debug.Assert(m_sOwnerClassName.Length > 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Class name of the object that created this object.

	protected String m_sOwnerClassName;
}

}
