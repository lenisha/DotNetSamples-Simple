namespace WorkflowASPNet
{
    using System;
    using System.Activities;
    using System.Collections.Generic;
    using System.Threading;
    using System.Web.UI;

    using WorkflowASPNet.Services;

    public partial class _Default : Page
    {
        #region Constants and Fields

        private static readonly Activity SayHelloDefinition = new SayHello();

        #endregion

        #region Methods

        protected void LinkButtonSayHello(object sender, EventArgs e)
        {
            var delay = Int32.Parse(this.txtDelay.Text);
            if (this.CheckBoxAsync.Checked)
            {
                this.InvokeWorkflowAsync(delay);
                this.InvokeServiceAsync(delay);
            }
            else
            {
                this.InvokeWorkflow(delay);
                this.InvokeService(delay);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void InvokeService(int delay)
        {
            this.Trace.Write(string.Format("Calling service on thread {0}", Thread.CurrentThread.ManagedThreadId));

            var proxy = new TestServiceClient();

            try
            {
                var result = proxy.DoWork(delay);

                proxy.Close();

                this.Trace.Write(string.Format("Completed calling service on thread {0} delay {1}", Thread.CurrentThread.ManagedThreadId, result));

                this.labelDelay.Text = result.ToString();
            }
            catch (Exception)
            {
                proxy.Abort();
                throw;
            }
        }

        private void InvokeServiceAsync(int delay)
        {
            this.Trace.Write(string.Format("Calling service on thread {0}", Thread.CurrentThread.ManagedThreadId));

            var proxy = new TestServiceClient();

            proxy.DoWorkCompleted += (o, args) =>
                {
                    if (!args.Cancelled)
                    {
                        this.Trace.Write(string.Format("Completed calling service on thread {0} delay {1}", Thread.CurrentThread.ManagedThreadId, args.Result));
                        this.labelDelay.Text = args.Result.ToString();
                    }
                };

            proxy.DoWorkAsync(delay);
        }

        private void InvokeWorkflow(int delay)
        {
            var input = new Dictionary<string, object> { { "Name", this.TextBoxName.Text }, { "Delay", delay } };

            this.Trace.Write(string.Format("Starting workflow on thread {0}", Thread.CurrentThread.ManagedThreadId));

            var output = WorkflowInvoker.Invoke(SayHelloDefinition, input);

            this.Trace.Write(string.Format("Completed workflow on thread {0}", Thread.CurrentThread.ManagedThreadId));

            this.labelGreeting.Text = output["Greeting"].ToString();
        }

        private void InvokeWorkflowAsync(int delay)
        {
            // Create the inputs  
            var input = new Dictionary<string, object> { { "Name", this.TextBoxName.Text }, { "Delay", delay } };

            var workflowApplication = new WorkflowApplication(SayHelloDefinition, input)
                {
                    // Tell the workflow runtime we are using the ASP.NET synchronization context  
                    SynchronizationContext = SynchronizationContext.Current,
                    Completed = args =>
                        {
                            this.Trace.Write(
                                string.Format("Completed workflow on thread {0}", Thread.CurrentThread.ManagedThreadId));

                            // Update page controls here  
                            this.labelGreeting.Text = args.Outputs["Greeting"].ToString();

                            // Tell ASP.NET we are finished  
                            SynchronizationContext.Current.OperationCompleted();
                        }
                };

            this.Trace.Write(string.Format("Starting workflow on thread {0}", Thread.CurrentThread.ManagedThreadId));

            // Tell ASP.NET we are starting an async operation  
            SynchronizationContext.Current.OperationStarted();

            workflowApplication.Run();
            // Don't try anything here - it will run before your workflow has completed  
        }

        #endregion
    }
}