using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
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

			IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
			WebHeaderCollection headers = request.Headers;
			
			X509Store store = new X509Store(StoreLocation.CurrentUser);
			store.Open(OpenFlags.ReadOnly);

			StringBuilder certs = new StringBuilder();
			var certificates = store.Certificates;
			foreach (var certificate in certificates)
			{
				var friendlyName = certificate.FriendlyName;
				var xname = certificate.GetName(); //obsolete
				certs.Append(xname + " ");
				try
				{
					if (certificate.HasPrivateKey)
						certs.Append(" private key size:" + certificate.PrivateKey.KeySize);
				}
				catch (Exception ex)
				{
					certs.Append("Exception: "+ex.Message);
				}
			}
			store.Close();

			// Add your operation implementation here
			return "Certificates:"+ certs.ToString();
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
