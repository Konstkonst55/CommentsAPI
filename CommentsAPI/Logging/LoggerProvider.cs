namespace CommentsAPI.Logging
{
    public class LoggerProvider : ILoggerProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public LoggerProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new Logger(categoryName, _serviceProvider);
        }

        public void Dispose() { }
    }
}
