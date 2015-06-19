using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Web;
using System.Collections.Specialized;

namespace LoginAPI.Common.Tools
{
    public static class CryptTool
    {
        //Encrypt strings
        /// <summary>
        /// Encode Method
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Encode(this string value)
        {
            var hash = System.Security.Cryptography.SHA1.Create();
            var encoder = new System.Text.ASCIIEncoding();
            var combined = encoder.GetBytes(value ?? "");
            return BitConverter.ToString(hash.ComputeHash(combined)).ToLower().Replace("-", "");
        }

        /// <summary>
        /// Authenticate Method
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool Authenticate(IncomingWebRequestContext context)
        {
            bool Authenticated = false;
            string normalizedUrl;
            string normalizedRequestParameters;
            //context.Headers
            NameValueCollection pa = context.UriTemplateMatch.QueryParameters;
            if (pa != null && pa["oauth_consumer_key"] != null)
            {
                // to get uri without oauth parameters
                string uri = context.UriTemplateMatch.RequestUri.OriginalString.Replace
                    (context.UriTemplateMatch.RequestUri.Query, "");
                string consumersecret = "SecretKey";
                OAuthBase oauth = new OAuthBase();
                string hash = oauth.GenerateSignature(
                    new Uri(uri),
                    pa["oauth_consumer_key"],
                    consumersecret,
                    null, // totken
                    null, //token secret
                    "GET",
                    pa["oauth_timestamp"],
                    pa["oauth_nonce"],
                    out normalizedUrl,
                    out normalizedRequestParameters
                    );
                Authenticated = pa["oauth_signature"] == hash;
            }
            return Authenticated;
        }
    }
}