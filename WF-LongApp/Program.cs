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

		static void StartSimpleWorkflowAsync()
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

		static WorkflowApplication StartWorkflowAsync()
		{
			Variable<string> name = new Variable<string>();

			Activity wf = new Sequence
			{
				Variables = { name },
				Activities =
				 {
					 new WriteLine
					 {
						 Text = "What is your name?"
					 },
					 new ReadLine
					 {
						 BookmarkName = "UserName",
						 Result = new OutArgument<string>(name)

					 },
					 new WriteLine
					 {
						 Text = new InArgument<string>((env) =>
							 ("Hello, " + name.Get(env)))
					 }
				 }
			};

			// Create a WorkflowApplication instance.
			WorkflowApplication wfApp = new WorkflowApplication(wf);

			// Workflow lifecycle events omitted except idle.
			AutoResetEvent idleEvent = new AutoResetEvent(false);

			wfApp.Idle = delegate (WorkflowApplicationIdleEventArgs e)
			{
				idleEvent.Set();
			};

			// Run the workflow.
			wfApp.Run();

			// Wait for the workflow to go idle before gathering
			// the user's input.
			idleEvent.WaitOne();

			return wfApp;

			

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

			WorkflowApplication wfApp = null;
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
					wfApp = StartWorkflowAsync();

					var res = Encoding.UTF8.GetBytes("HTTP/1.1 200 OK\r\n\r\n Workflow invoked\r\n");
					stream.Write(res, 0, res.Length);
					stream.Close();
				}
				else if (new Regex("^GET /resume").IsMatch(data))
				{
					// Gather the user's input and resume the bookmark.
					// Bookmark resumption only occurs when the workflow
					// is idle. If a call to ResumeBookmark is made and the workflow
					// is not idle, ResumeBookmark blocks until the workflow becomes
					// idle before resuming the bookmark.
					BookmarkResumptionResult result = wfApp.ResumeBookmark("UserName",
						"Pivotal");

					// Possible BookmarkResumptionResult values:
					// Success, NotFound, or NotReady
					Console.WriteLine("BookmarkResumptionResult: {0}", result);

					var res = Encoding.UTF8.GetBytes("HTTP/1.1 200 OK\r\n\r\n Workflow resumed\r\n");
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

	public sealed class ReadLine : NativeActivity<string>
	{
		[RequiredArgument]
		public InArgument<string> BookmarkName { get; set; }

		protected override void Execute(NativeActivityContext context)
		{
			// Create a Bookmark and wait for it to be resumed.
			context.CreateBookmark(BookmarkName.Get(context),
				new BookmarkCallback(OnResumeBookmark));
		}

		// NativeActivity derived activities that do asynchronous operations by calling 
		// one of the CreateBookmark overloads defined on System.Activities.NativeActivityContext 
		// must override the CanInduceIdle property and return true.
		protected override bool CanInduceIdle
		{
			get { return true; }
		}

		public void OnResumeBookmark(NativeActivityContext context, Bookmark bookmark, object obj)
		{
			// When the Bookmark is resumed, assign its value to
			// the Result argument.
			Result.Set(context, (string)obj);
		}
	}
}
