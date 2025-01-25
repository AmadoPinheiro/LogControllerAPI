using LogTransformer.Core.Validations;
using System;
using Xunit;

public class LogValidatorTests
{
    [Fact]
    public void IsValidLogFormat_ValidLog_ShouldReturnTrue()
    {
        var validLog = "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2";
        var result = LogValidator.IsValidLogFormat(validLog);

        Assert.True(result);
    }

    [Fact]
    public void IsValidLogFormat_InvalidLog_ShouldReturnFalse()
    {
        var invalidLog = "312|200|HIT";
        var result = LogValidator.IsValidLogFormat(invalidLog);

        Assert.False(result);
    }

    [Fact]
    public void EnsureValidLog_InvalidLog_ShouldThrowException()
    {
        var invalidLog = "312|200|HIT";

        Assert.Throws<ArgumentException>(() => LogValidator.EnsureValidLog(invalidLog));
    }

    [Fact]
    public void EnsureValidLog_ValidLog_ShouldNotThrowException()
    {
        var validLog = "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2";

        var exception = Record.Exception(() => LogValidator.EnsureValidLog(validLog));

        Assert.Null(exception);
    }
}
