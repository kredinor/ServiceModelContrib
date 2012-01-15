namespace ServiceModelContrib.Tests.Mocks
{
    using System.ServiceModel;
    using System.Threading;

    public class MockService : IMockService
    {
        private readonly ILogger _logger;

        public MockService()
        {
        }

        public MockService(ILogger logger)
        {
            _logger = logger;
        }

        #region IMockService Members

        public void DoOperation(string message)
        {
            EnsureLoggerPresent();
            _logger.Log(message);
        }

        public string GetLastLogEntry()
        {
            EnsureLoggerPresent();
            return _logger.GetLastEntry();
        }

        public string FlowUserName()
        {
            if (Thread.CurrentPrincipal == null)
            {
                return string.Empty;
            }

            return Thread.CurrentPrincipal.Identity.Name;
        }

        public void OneWay()
        {
            // nop.
        }

        public void ShouldThrow()
        {
            throw new FaultException(new FaultReason("fuu"));
        }

        #endregion

        private void EnsureLoggerPresent()
        {
            if (_logger == null)
                throw new LoggerNotPresentException();
        }
    }
}