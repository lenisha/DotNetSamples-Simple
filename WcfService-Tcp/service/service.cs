//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

using System;
using System.Net;
using System.ServiceModel;
using System.Net.Sockets;
using System.Linq;

namespace Microsoft.Samples.NetTcp
{
    // Define a service contract.
    [ServiceContract(Namespace="http://Microsoft.Samples.NetTcp")]
    public interface ICalculator
    {
        [OperationContract]
        double Add(double n1, double n2);
        [OperationContract]
        double Subtract(double n1, double n2);
        [OperationContract]
        double Multiply(double n1, double n2);
        [OperationContract]
        double Divide(double n1, double n2);
    }

	// Service class which implements the service contract.
	// Added code to write output to the console window
	[ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
	public class CalculatorService : ICalculator
    {
        public double Add(double n1, double n2)
        {
            double result = n1 + n2;
            Console.WriteLine("Received Add({0},{1})", n1, n2);
            Console.WriteLine("Return: {0}", result);
            return result;
        }

        public double Subtract(double n1, double n2)
        {
            double result = n1 - n2;
            Console.WriteLine("Received Subtract({0},{1})", n1, n2);
            Console.WriteLine("Return: {0}", result);
            return result;
        }

        public double Multiply(double n1, double n2)
        {
            double result = n1 * n2;
            Console.WriteLine("Received Multiply({0},{1})", n1, n2);
            Console.WriteLine("Return: {0}", result);
            return result;
        }

        public double Divide(double n1, double n2)
        {
            double result = n1 / n2;
            Console.WriteLine("Received Divide({0},{1})", n1, n2);
            Console.WriteLine("Return: {0}", result);
            return result;
        }

        // Host the service within this EXE console application.
        public static void Main()
        {
			// get PORT assigned to app from PCF env var
			var port = Environment.GetEnvironmentVariable("PORT");
			if (string.IsNullOrEmpty(port))
			{
				port = "9743";
			}
			var localip = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
			if (localip == null)
			{
				Console.Error.WriteLine("Couldn't find IP address of container");
				Environment.Exit(1);
			}
			// Create a ServiceHost for the CalculatorService type.
			using (ServiceHost serviceHost = new ServiceHost(typeof(CalculatorService), new Uri($"net.tcp://{localip}:{port}/servicemodelsamples/service")))
            {
                // Open the ServiceHost to create listeners and start listening for messages.
                serviceHost.Open();
				Console.WriteLine($"Server has started on {localip}:{port}.\r\nWaiting for a connection...");
                Console.WriteLine("Press <ENTER> to terminate service.");
                Console.WriteLine();
                Console.ReadLine();
            }
        }
    }
}
