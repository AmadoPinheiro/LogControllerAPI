using LogTransformer.Core.Entities;
using LogTransformer.Core.Interfaces;
using LogTransformer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading.Tasks;
using Xunit;

public class LogRepositoryTests
{
    private readonly Mock<ILogRepository> _logRepositoryMock;
    public LogRepositoryTests()
    {
        _logRepositoryMock = new Mock<ILogRepository>();
    }

    private LogDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LogDbContext>()
            .UseInMemoryDatabase(databaseName: "LogDb")
            .Options;

        return new LogDbContext(options);
    }

    [Fact]
    public async Task AddLogAsync_ShouldAddLog()
    {
        var log = new LogEntry { OriginalLog = "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2" };

        _logRepositoryMock.Setup(repo => repo.AddLogAsync(It.IsAny<LogEntry>()))
                          .Returns(Task.CompletedTask);

        await _logRepositoryMock.Object.AddLogAsync(log);

        _logRepositoryMock.Verify(repo => repo.AddLogAsync(It.IsAny<LogEntry>()), Times.Once);
    }

    [Fact]
    public async Task GetLogByIdAsync_ShouldReturnLog()
    {
        var log = new LogEntry { Id = 1, OriginalLog = "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2" };

        _logRepositoryMock.Setup(repo => repo.GetLogByIdAsync(1))
                          .ReturnsAsync(log);

        var result = await _logRepositoryMock.Object.GetLogByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(log.OriginalLog, result.OriginalLog);
    }
}
