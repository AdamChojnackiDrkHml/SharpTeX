using Microsoft.Extensions.Logging;

namespace SharpTeX.Logging;

public static class DefaultLogger
{
    private readonly static ILoggerFactory LoggerFactory;
    
    static DefaultLogger()
    {
        LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
    }
    
    public static ILogger<T> CreateLogger<T>() 
        => LoggerFactory.CreateLogger<T>();

    public static ILogger CreateLogger()
        => LoggerFactory.CreateLogger("SharpTeX");
}