using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WorkflowASPNet
{
    using System.Threading;

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TestService" in code, svc and config file together.
    public class TestService : ITestService
    {
        public int DoWork(int delay)
        {
            Thread.Sleep(delay);
            return delay;
        }
    }
}
