
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Net;
using System.IO;
using System.Text;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.Common
{
//*****************************************************************************
//  Class: UserRegisterer
//
/// <summary>
/// Registers a user by sending his email address to a Web service.
/// </summary>
///
/// <remarks>
/// SQL Data Services (SDS) is used as the Web service and database store.
///
/// <para>
/// The code in this class is based on the "Creating an Entity Using REST (C#)"
/// sample in MSDN:
/// </para>
///
/// <para>
/// http://msdn.microsoft.com/en-us/library/cc512433.aspx
/// </para>
///
/// </remarks>
//*****************************************************************************

public class UserRegisterer : Object
{
    //*************************************************************************
    //  Constructor: UserRegisterer()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="UserRegisterer" /> class.
    /// </summary>
    //*************************************************************************

    public UserRegisterer()
    {
		m_iHttpWebRequestTimeoutMs = 10000;

		AssertValid();
    }

	//*************************************************************************
	//	Property: HttpWebRequestTimeoutMs
	//
	/// <summary>
	/// Gets or sets the timeout to use for Web requests.
	/// </summary>
	///
	/// <value>
	/// The timeout to use for Web requests, in milliseconds.  Must be greater
	/// than zero.  The default is 10,000 milliseconds.
	/// </value>
	//*************************************************************************

	public Int32
	HttpWebRequestTimeoutMs
	{
		get
		{
			AssertValid();

			return (m_iHttpWebRequestTimeoutMs);
		}

		set
		{
			m_iHttpWebRequestTimeoutMs = value;

			AssertValid();
		}
	}

    //*************************************************************************
    //  Method: RegisterUser()
    //
    /// <summary>
	/// Registers a user by sending his email address to a Web service.
    /// </summary>
	///
	/// <param name="emailAddress">
	/// The user's email address.
	/// </param>
	///
	/// <remarks>
	/// If the user can't be registered, a <see cref="RegisterUserException" />
	/// is thrown.
	/// </remarks>
    //*************************************************************************

    public void
    RegisterUser
	(
		String emailAddress
	)
    {
		Debug.Assert( !String.IsNullOrEmpty(emailAddress) );
		AssertValid();

		// Create an entity payload.

		const String EntityTemplate =

			"<Entity xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\""
			+ " xmlns:x=\"http://www.w3.org/2001/XMLSchema\""
			+ " xmlns:s=\"http://schemas.microsoft.com/sitka/2008/03/\">"

			+ "<s:Id>{0}</s:Id>"

			+ "<NodeXLVersion xsi:type='x:string'>{1}</NodeXLVersion>"

			+ "<RegistrationTimeUtc xsi:type='x:string'>{2}"
			+ "</RegistrationTimeUtc>"

			+ "</Entity>"
			;

		String sEntity = String.Format( EntityTemplate,
			emailAddress,
			AssemblyUtil2.GetFileVersion(),
			DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss")
			);

		UTF8Encoding oUtf8Encoding = new UTF8Encoding();

		Int32 iByteCount = oUtf8Encoding.GetByteCount(sEntity);

		try
		{
			// Create a request object.

			HttpWebRequest oHttpWebRequest =
				(HttpWebRequest)HttpWebRequest.Create(ContainerUrl);

			oHttpWebRequest.Credentials =
				new NetworkCredential(UserName, Password);

			oHttpWebRequest.Method = "POST";
			oHttpWebRequest.ContentLength = iByteCount;
			oHttpWebRequest.ContentType = "application/x-ssds+xml";
			oHttpWebRequest.Timeout = m_iHttpWebRequestTimeoutMs;

			// Send the request.

			using ( Stream oRequestStream =
				oHttpWebRequest.GetRequestStream() )
			{
				oRequestStream.Write(oUtf8Encoding.GetBytes(sEntity), 0,
					iByteCount);
			}

			// Get the response.

			using ( HttpWebResponse oHttpWebResponse =
				(HttpWebResponse)oHttpWebRequest.GetResponse() )
			{
				switch (oHttpWebResponse.StatusCode)
				{
					case HttpStatusCode.Created:  // HTTP 201.

						// Success.

						break;

					default:

						String sMessage = GetErrorMessage( String.Format(

							"The HTTP status code was {0}."
							,
							oHttpWebResponse.StatusCode
							) );

						throw new RegisterUserException(sMessage, null);
				}
			}
		}
		catch (RegisterUserException)
		{
			throw;
		}
		catch (Exception oException)
		{
			OnException(oException);
		}
    }

    //*************************************************************************
    //  Method: OnException()
    //
    /// <summary>
	/// Handles an exception thrown during registration.
    /// </summary>
	///
	/// <param name="oException">
	/// Exception that was thrown.
	/// </param>
    //*************************************************************************

	protected void
	OnException
	(
		Exception oException
	)
	{
		Debug.Assert(oException != null);
		AssertValid();

		String sMessage = null;

		const String TimeoutMessage =
			"The Web site used for registration couldn't be reached.  Either"
			+ " the site is temporarily unavailable, or you don't have an"
			+ " Internet connection."
			;

		if (oException is WebException)
		{
			WebException oWebException = (WebException)oException;

			if (oWebException.Response is HttpWebResponse)
			{
				HttpWebResponse oHttpWebResponse =
					(HttpWebResponse)oWebException.Response;

				switch (oHttpWebResponse.StatusCode)
				{
					case HttpStatusCode.RequestTimeout:  // HTTP 408.

						sMessage = TimeoutMessage;
						break;

					case HttpStatusCode.Conflict:  // HTTP 409.

						sMessage = "That email address is already registered.";
						break;

					default:

						break;
				}
			}
			else
			{
				switch (oWebException.Status)
				{
					case WebExceptionStatus.Timeout:

						sMessage = TimeoutMessage;
						break;

					default:

						break;
				}
			}
		}

		if (sMessage == null)
		{
			sMessage = GetErrorMessage(
				ExceptionUtil.GetMessageTrace(oException) );
		}

		throw new RegisterUserException(sMessage, oException);
	}

    //*************************************************************************
    //  Method: GetErrorMessage()
    //
    /// <summary>
	/// Returns a user-friendly error message when registration fails.
    /// </summary>
	///
	/// <param name="sErrorDetails">
	/// Error details.
	/// </param>
	///
	/// <returns>
	/// User-friendly error message.
	/// </returns>
    //*************************************************************************

	protected String
	GetErrorMessage
	(
		String sErrorDetails
	)
	{
		Debug.Assert( !String.IsNullOrEmpty(sErrorDetails) );
		AssertValid();

		return ( String.Format(

			"An unexpected probem occurred.  You could not be registered."
			+ "  Please try again later.\r\n\r\n"
			+ " Details:\r\n\r\n"
			+ "{0}"
			,
			sErrorDetails
			) );
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
		Debug.Assert(m_iHttpWebRequestTimeoutMs > 0);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	/// User name and password of the SDS account.

	protected const String UserName = "UserRegistration";
	///
	protected const String Password = "7m%DpH4#!2";

	/// URL of the SDS containter.  The SDS authority is "users2" and the
	/// container is "users".

	protected const String ContainerUrl =
		"https://users2.data.database.windows.net/v1/users";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// The timeout to use for Twitter Web requests, in milliseconds.

	protected Int32 m_iHttpWebRequestTimeoutMs;
}

}
