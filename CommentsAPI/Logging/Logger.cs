using CommentsAPI.Data;
using CommentsAPI.Models;

namespace CommentsAPI.Logging
{
    public class Logger : ILogger
    {
        private readonly string _categoryName;
        private readonly IServiceProvider _serviceProvider;

        public Logger(string categoryName, IServiceProvider serviceProvider)
        {
            _categoryName = categoryName;
            _serviceProvider = serviceProvider;
        }

        public IDisposable? BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public void Log<TState>(LogLevel logLevel, EventId eventId,
            TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var message = formatter(state, exception);

            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<CommentDBContext>();

            db.Logs.Add(new LogEntry
            {
                Timestamp = DateTime.UtcNow,
                Level = logLevel.ToString(),
                Message = message,
                Exception = exception?.ToString(),
                Endpoint = _categoryName
            });

            db.SaveChanges();
        }
    }
}
