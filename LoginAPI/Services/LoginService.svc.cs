using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LoginAPI.Services
{
    using ServiceContracts;
    using LoginAPI.Common.Tools;
    using System.ServiceModel.Web;
    using System.Net;
    using System.Xml.Linq;

    public class LoginService : ILoginService
    {
        #region Public Methods

        /// <summary>
        /// GetUser Method
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Info GetUser(string username, string password)
        {
            if (CryptTool.Authenticate(WebOperationContext.Current.IncomingRequest))
            {
                return Login(username, password);
            }
            else
            {
                return new Info
                {
                    Status = "Unauthorized Request."
                };
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// AuthenticateLogin Method
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private string AuthenticateLogin(string username, string password)
        {
            XDocument doc = XDocument.Load(System.AppDomain.CurrentDomain.BaseDirectory + "Users.xml");

            try
            {
                var result = (from item in doc.Descendants("User")
                              where (string)item.Element("Username") == username && ((string)item.Element("Password")).Encode() == password
                              select (string)item.Element("Name").Value).Single().ToString();
                return result.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Login Method
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private Info Login(string username, string password)
        {
            string name = AuthenticateLogin(username, password);
            if (!string.IsNullOrEmpty(name))
            {
                return new Info
                {
                    Status = "OK",
                    Content = new User { Name = name, Username = username }
                };
            }
            else
            {
                return new Info
                {
                    Status = "Username or Password incorrect!"
                };
            }
        }

        #endregion

    }
}
