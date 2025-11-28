using Xunit;
using Moq;
using ProjektInzynierski.Utils;

public class LoggerTests
{
    [Fact]
    public void getInstance_ShouldReturnSameInstance()
    {
        var logger1 = Logger.getInstance();
        var logger2 = Logger.getInstance();

        Assert.Same(logger1, logger2);
    }

    [Fact]
    public void Attach_ShouldAddObserver()
    {
        var logger = Logger.getInstance();
        var observer = new Mock<ILogObserver>();

        logger.Attach(observer.Object);

        logger.Log("Test");
        observer.Verify(o => o.Update(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void Detach_ShouldRemoveObserver()
    {
        var logger = Logger.getInstance();
        var observer = new Mock<ILogObserver>();
        logger.Attach(observer.Object);
        logger.Detach(observer.Object);

        logger.Log("Test");
        observer.Verify(o => o.Update(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void Log_ShouldIncreaseLogCount()
    {
        var logger = Logger.getInstance();
        int initialCount = logger.GetType().GetField("_logCount", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(logger) as int? ?? 0;

        logger.Log("Message");

        int newCount = logger.GetType().GetField("_logCount", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(logger) as int? ?? 0;
        Assert.Equal(initialCount + 1, newCount);
    }

    [Fact]
    public void Notify_ShouldCallAllObservers()
    {
        var logger = Logger.getInstance();
        var observer1 = new Mock<ILogObserver>();
        var observer2 = new Mock<ILogObserver>();

        logger.Attach(observer1.Object);
        logger.Attach(observer2.Object);

        logger.Notify("Hello");

        observer1.Verify(o => o.Update("Hello"), Times.Once);
        observer2.Verify(o => o.Update("Hello"), Times.Once);
    }
}
