namespace ServiceModelContrib.Tests.Mocks
{
    using System;
    using System.Diagnostics;

    public class MockLogger : ILogger
    {
        private string _lastMessage;

        #region ILogger Members

        public void Log(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException("message");

            _lastMessage = message;
            Trace.Write(message);
        }

        public string GetLastEntry()
        {
            return _lastMessage;
        }

        #endregion
    }
}