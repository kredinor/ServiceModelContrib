namespace ServiceModelContrib.Tests.Mocks
{
    public interface ILogger
    {
        void Log(string message);

        string GetLastEntry();
    }
}