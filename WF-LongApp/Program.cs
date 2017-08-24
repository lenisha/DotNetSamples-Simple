using System;
using System.Linq;
using System.Activities;
using System.Activities.Statements;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace WF_LongApp
{

	class Program
	{
		static void Main(string[] args)
		{
			
			StartListener();
			
		}

		static void StartWorkflowAsync()
		{
			Activity wf = new Workflow1();
			AutoResetEvent syncEvent = new AutoResetEvent(false);

			//Activity wf = new WriteLine
			//{
			//	Text = "Hello World."
			//};

			// Create the WorkflowApplication using the desired
			// workflow definition.
			WorkflowApplication wfApp = new WorkflowApplication(wf);

			// Handle the desired lifecycle events.
			wfApp.Completed = delegate (WorkflowApplicationCompletedEventArgs e)
			{
				syncEvent.Set();
			};

			// Start the workflow.
			wfApp.Run();

			// Wait for Completed to arrive and signal that
			// the workflow is complete.
			syncEvent.WaitOne();
		}

		static void StartListener()
		{
			// get PORT assigned to app from PCF env var
			var port = Environment.GetEnvironmentVariable("PORT");
			if (string.IsNullOrEmpty(port))
			{
				port = "11000";
			}
			var localip = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
			if (localip == null)
			{
				Console.Error.WriteLine("Couldn't find IP address of container");
				Environment.Exit(1);
			}

			TcpListener server = new TcpListener(localip, int.Parse(port));

			server.Start();
			Console.WriteLine($"Server has started on {localip}:{port}.\r\nWaiting for a connection...");

			// loop forever and handle any connections
			while (true)
			{
				TcpClient client = server.AcceptTcpClient();

				Console.WriteLine("A client connected.");

				NetworkStream stream = client.GetStream();

				while (!stream.DataAvailable) ;

				Byte[] bytes = new Byte[client.Available];

				stream.Read(bytes, 0, bytes.Length);

				//translate bytes of request to string
				String data = Encoding.UTF8.GetString(bytes);

				Console.WriteLine($"Request received {data}");
				if (new Regex("^GET /start").IsMatch(data))
				{
					Console.WriteLine("START request");
					StartWorkflowAsync();

					var res = Encoding.UTF8.GetBytes("HTTP/1.1 200 OK\r\n\r\n Workflow invoked\r\n");
					stream.Write(res, 0, res.Length);
					stream.Close();
				}
				else if (new Regex("^GET /").IsMatch(data))
				{
					Console.WriteLine("health request");
				

					var res = Encoding.UTF8.GetBytes("HTTP/1.1 200 OK\r\n\r\n");
					stream.Write(res, 0, res.Length);
					stream.Close();
				}
				else
				{
					Console.WriteLine("??? request");
				}
			}
		}

	}
}
