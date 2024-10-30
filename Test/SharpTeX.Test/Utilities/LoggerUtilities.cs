using Microsoft.Extensions.Logging;
using Moq;

namespace SharpTeX.Test.Utilities;

public static class LoggerUtilities
{
    public static void VerifyLog(Mock<ILogger> logger, LogLevel logLevel, string message, Times times)
    {
        logger.Verify(mock => mock.Log(
        logLevel,
        It.IsAny<EventId>(),
        It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(message)),
        It.IsAny<Exception>(),
        It.IsAny<Func<It.IsAnyType, Exception?, string>>()
        ),
        times);
    }
}