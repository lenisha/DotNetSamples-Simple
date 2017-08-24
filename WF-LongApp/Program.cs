using System;
using System.Linq;
using System.Activities;
using System.Activities.Statements;
using System.Threading;

namespace WF_LongApp
{

	class Program
	{
		static void Main(string[] args)
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
	}
}
