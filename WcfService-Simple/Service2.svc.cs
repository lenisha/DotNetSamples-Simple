using System;
using System.Collections;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;

namespace WcfService_Simple
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Service2
    {
        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        [OperationContract]
        [WebGet(UriTemplate = "/hello",ResponseFormat = WebMessageFormat.Json)]
        public String DoWork()
        {
            // Add your operation implementation here
            return "Hello rest:" + Thread.CurrentPrincipal.Identity.ToString();
        }


        // Add more operations here and mark them with 
        [OperationContract]
        [WebGet(UriTemplate = "/env", ResponseFormat = WebMessageFormat.Json)]
        public IDictionary GetEnv()
        {
            return Environment.GetEnvironmentVariables();
        }
    }
}
