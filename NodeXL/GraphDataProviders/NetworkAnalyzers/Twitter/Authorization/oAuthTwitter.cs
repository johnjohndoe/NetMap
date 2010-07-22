using System;
using System.Web;
using System.Net;
using System.IO;
using System.Collections.Specialized;


//****************************************************************************
// Credits
//
// The original versions of this file and its companion file, oAuth.cs, were 
// written by Shannon Whitley.  They were downloaded from his blog in
// June 2010:
//
//     http://www.voiceoftech.com/swhitley/?p=856
//
// Shannon's code was referenced by the Twitter API Wiki as an example of an
// OAuth implementation for .NET.
//
// Many thanks to Shannon for making his implementation public.
//
// The changes made to the original file were as follows:
//
//     1. Added a pragma to turn off some benign warnings.
//
//     2. Changed the namespace.
//
//     3. Added more error handling.
//
//     4. Set WebRequest.UserAgent and Timeout.
//
//     5. Refactored oAuthWebRequest() to fit into NodeXL's request retry
//        scheme.
// 
//****************************************************************************


// XML comments for some public types and members are missing.  Turn off
// warnings for them.

#pragma warning disable 1591, 1573

namespace Microsoft.NodeXL.GraphDataProviders.Twitter
{
    public class oAuthTwitter : OAuthBase
    {
        public enum Method { GET, POST };
        public const string REQUEST_TOKEN = "http://twitter.com/oauth/request_token";
        public const string AUTHORIZE = "http://twitter.com/oauth/authorize";
        public const string ACCESS_TOKEN = "http://twitter.com/oauth/access_token";

        private string _consumerKey = "k3j8yuEkPGvbehgJJh526A";
        private string _consumerSecret = "jkn3ud93urfhfsdlk32j8sJE3kJeLejWWkJeejEjeh";
        private string _token = "";
        private string _tokenSecret = "";
        private string _verifier = "";

#region Properties
        public string ConsumerKey 
        {
            get
            {
                return _consumerKey; 
            } 
            set { _consumerKey = value; } 
        }
        
        public string ConsumerSecret { 
            get {
                return _consumerSecret; 
            } 
            set { _tokenSecret = value; } 
        }

        public string Token { get { return _token; } set { _token = value; } }
        public string TokenSecret { get { return _tokenSecret; } set { _tokenSecret = value; } }
        public string Verifier { get { return _verifier; } set { _verifier = value; } }

#endregion

        /// <summary>
        /// Get the link to Twitter's authorization page for this application.
        /// </summary>
        /// <returns>The url with a valid request token.</returns>
        public string AuthorizationLinkGet()
        {
            string ret = null;

            string response = oAuthWebRequest(Method.GET, REQUEST_TOKEN, String.Empty);
            if (response.Length > 0)
            {
                //response contains token and token secret.  We only need the token.
                NameValueCollection qs = HttpUtility.ParseQueryString(response);
                if (qs["oauth_token"] != null)
                {
                    ret = AUTHORIZE + "?oauth_token=" + qs["oauth_token"];
                }
            }
            else
            {
                throw new WebException(
                    "Can't get the link to the Twitter authorization page.");
            }

            return ret;
        }

        /// <summary>
        /// Exchange the request token for an access token.
        /// </summary>
        /// <param name="authToken">The oauth_token is supplied by Twitter's authorization page following the callback.</param>
        public void AccessTokenGet(string authToken, string verifier)
        {
            this.Token = authToken;
            this.Verifier = verifier;

            string response = oAuthWebRequest(Method.GET, ACCESS_TOKEN, String.Empty);

            if (response.Length > 0)
            {
                //Store the Token and Token Secret
                NameValueCollection qs = HttpUtility.ParseQueryString(response);
                if (qs["oauth_token"] != null)
                {
                    this.Token = qs["oauth_token"];
                }
                else
                {
                    throw new WebException(
                        "An access token wasn't sent by Twitter.");
                }

                if (qs["oauth_token_secret"] != null)
                {
                    this.TokenSecret = qs["oauth_token_secret"];
                }
                else
                {
                    throw new WebException(
                        "An access token secret wasn't sent by Twitter.");
                }

            }
            else
            {
                throw new WebException(
                    "Can't exchange the request token for an access token.");
            }
        }
        
        /// <summary>
        /// Constructs the URL and POST data to use for a web request using
        /// oAuth.
        /// </summary>
        /// <param name="method">GET or POST</param>
        /// <param name="url">The full url, including the querystring.</param>
        /// <param name="postData">Data to post (querystring format)</param>
        /// <param name="authorizedUrl">The url with authorization information added.</param>
        /// <param name="authorizedPostData">The POST data with authorization information added.</param>
        public void ConstructAuthWebRequest(Method method, string url, string postData, out String authorizedUrl, out String authorizedPostData)
        {
            string outUrl = "";
            string querystring = "";


            //Setup postData for signing.
            //Add the postData to the querystring.
            if (method == Method.POST)
            {
                if (postData.Length > 0)
                {
                    //Decode the parameters and re-encode using the oAuth UrlEncode method.
                    NameValueCollection qs = HttpUtility.ParseQueryString(postData);
                    postData = "";
                    foreach (string key in qs.AllKeys)
                    {
                        if (postData.Length > 0)
                        {
                            postData += "&";
                        }
                        qs[key] = HttpUtility.UrlDecode(qs[key]);
                        qs[key] = this.UrlEncode(qs[key]);
                        postData += key + "=" + qs[key];

                    }
                    if (url.IndexOf("?") > 0)
                    {
                        url += "&";
                    }
                    else
                    {
                        url += "?";
                    }
                    url += postData;
                }
            }

            Uri uri = new Uri(url);
            
            string nonce = this.GenerateNonce();
            string timeStamp = this.GenerateTimeStamp();

            //Generate Signature
            string sig = this.GenerateSignature(uri,
                this.ConsumerKey,
                this.ConsumerSecret,
                this.Token,
                this.TokenSecret,
                this.Verifier,
                method.ToString(),
                timeStamp,
                nonce,
                out outUrl,
                out querystring);

            querystring += "&oauth_signature=" + HttpUtility.UrlEncode(sig);

            //Convert the querystring to postData
            if (method == Method.POST)
            {
                postData = querystring;
                querystring = "";
            }

            if (querystring.Length > 0)
            {
                outUrl += "?";
            }

            authorizedUrl = outUrl +  querystring;
            authorizedPostData = postData;
        }

        /// <summary>
        /// Submit a web request using oAuth.
        /// </summary>
        /// <param name="method">GET or POST</param>
        /// <param name="url">The full url, including the querystring.</param>
        /// <param name="postData">Data to post (querystring format)</param>
        /// <returns>The web server response.</returns>
        public string oAuthWebRequest(Method method, string url, string postData)
        {
            String authorizedUrl, authorizedPostData;

            ConstructAuthWebRequest(method, url, postData, out authorizedUrl,
                out authorizedPostData);

            return ( WebRequest(method, authorizedUrl, authorizedPostData) );
        }


        /// <summary>
        /// Web Request Wrapper
        /// </summary>
        /// <param name="method">Http Method</param>
        /// <param name="url">Full url to the web resource</param>
        /// <param name="postData">Data to post in querystring format</param>
        /// <returns>The web server response.</returns>
        public string WebRequest(Method method, string url, string postData)
        {
            HttpWebRequest webRequest = null;
            StreamWriter requestWriter = null;
            string responseData = "";

            webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = method.ToString();
            webRequest.ServicePoint.Expect100Continue = false;
            webRequest.UserAgent = HttpNetworkAnalyzerBase.UserAgent;
            webRequest.Timeout = HttpNetworkAnalyzerBase.HttpWebRequestTimeoutMs;

            if (method == Method.POST)
            {
                webRequest.ContentType = "application/x-www-form-urlencoded";

                //POST the data.
                requestWriter = new StreamWriter(webRequest.GetRequestStream());
                try
                {
                    requestWriter.Write(postData);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    requestWriter.Close();
                    requestWriter = null;
                }
            }

            responseData = WebResponseGet(webRequest);

            webRequest = null;

            return responseData;

        }

        /// <summary>
        /// Process the web response.
        /// </summary>
        /// <param name="webRequest">The request object.</param>
        /// <returns>The response data.</returns>
        public string WebResponseGet(HttpWebRequest webRequest)
        {
            StreamReader responseReader = null;
            string responseData = "";

            try
            {
                responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            catch
            {
                throw;
            }
            finally
            {
                webRequest.GetResponse().GetResponseStream().Close();
                responseReader.Close();
                responseReader = null;
            }

            return responseData;
        }
    }
}
