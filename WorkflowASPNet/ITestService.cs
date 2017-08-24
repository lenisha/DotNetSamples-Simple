namespace WorkflowASPNet
{
    using System.ServiceModel;

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITestService" in both code and config file together.
    [ServiceContract]
    public interface ITestService
    {
        #region Public Methods

        [OperationContract]
        int DoWork(int delay);

        #endregion
    }
}