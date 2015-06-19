using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;

namespace LoginAPI.ServiceContracts
{
    [ServiceContract(Namespace = "ILoginService/JSONData")]
    public interface ILoginService
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            //BodyStyle = WebMessageBodyStyle.Wrapped,
            BodyStyle=WebMessageBodyStyle.Bare,
            
            UriTemplate = "login/{username}/{password}")]
        Info GetUser(string username, string password);
    }

    [DataContract]
    public class User
    {
        /// <summary>
        /// Gets or Sets Username
        /// </summary>
        [DataMember]
        public string Username { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember]
        public string Name { get; set; }
    }

    [DataContract]
    public class Info
    {
        /// <summary>
        /// Gets or Sets Content
        /// </summary>
        [DataMember]
        public User Content { get; set; }

        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        [DataMember]
        public string Status { get; set; }
    }

}
