using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using System.IO;

namespace ConsumeService.ServiceContracts
{
    [ServiceContract(Namespace = "IConsumeLoginService/JSONData")]
    public interface IConsumeLoginService
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "login/{username}/{password}")]
        string GetUsername(string username, string password);
    }
}
