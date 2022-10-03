using NLog;
using ILogger = MusicCollection.Common.Loggers.ILogger;

namespace MusicCollection.Common.Loggers.NLog;

public class NLogLogger : ILogger
{
    private NLogLogger(Logger logger)
    {
        this.logger = logger;
    }

    public void Info(string message, params object?[] args)
    {
        logger.Info(message, args);
    }

    public void Error(string message, params object?[] args)
    {
        logger.Error(message, args);
    }

    public void Error(Exception exception, string message, params object?[] args)
    {
        logger.Error(exception, message, args);
    }

    public static NLogLogger Build(string name)
    {
        var logger = LogManager.GetLogger(name);
        return new NLogLogger(logger);
    }

    private readonly Logger logger;
}