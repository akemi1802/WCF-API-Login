using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ConsumeService.Services
{
    using ServiceContracts;
    using LoginAPI.Common.Tools;
    using System.Net;
    using System.IO;
    using System.Web.Script.Serialization;

    public class ConsumeLoginService : IConsumeLoginService
    {
        #region Public Methods

        /// <summary>
        /// GetUsername Method
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string GetUsername(string username, string password)
        {
            //keys to OAuthBase -> SecretKey is the authentication, in case need change don't forget to change on Authenticate method 
            string consumerKey = "keyconsumer";
            string consumerSecret = "SecretKey";

            string formatUri = string.Format("{0}/{1}/{2}", "http://localhost:54299/Services/LoginService.svc/login", username, password.Encode());
            
            var uri = new Uri(formatUri);

            string url, param;
            var oAuth = new OAuthBase();
            var nonce = oAuth.GenerateNonce();
            var timeStamp = oAuth.GenerateTimeStamp();
            var signature = oAuth.GenerateSignature(uri, consumerKey,
            consumerSecret, string.Empty, string.Empty, "GET", timeStamp, nonce,
            OAuthBase.SignatureTypes.HMACSHA1, out url, out param);

            WebResponse webrespon = (WebResponse)WebRequest.Create(
               string.Format("{0}?{1}&oauth_signature={2}", url, param, signature)).GetResponse();
            StreamReader stream = new StreamReader(webrespon.GetResponseStream());

            var jsonreturn = stream.ReadToEnd();

            //Serializing to extract "Name" key from the Json
            var serializer = new JavaScriptSerializer();
            dynamic item = serializer.Deserialize<object>(jsonreturn);

            var name = GetValue("Name", item);
            bool retorno = string.IsNullOrEmpty(name);

            return string.IsNullOrEmpty(name) ? jsonreturn : name;
        }

        #endregion

        #region Private Methods

        //Method to return specific value from Json
        /// <summary>
        /// GetValue Method
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        private string GetValue(string key, Dictionary<string, object> values)
        {
            foreach (var keyValue in values)
            {
                if (key.Equals(keyValue.Key, StringComparison.OrdinalIgnoreCase))
                {
                    return keyValue.Value as string;
                }
                var value = keyValue.Value as Dictionary<string, object>;
                if (value != null)
                {
                    var val = this.GetValue(key, value);
                    if (val != null)
                    {
                        return val;
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
