using LogTransformer.Core.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

public class LogTransformerServiceTests
{
    private readonly LogTransformerService _service;

    public LogTransformerServiceTests()
    {
        _service = new LogTransformerService();
    }

    [Fact]
    public void TransformLog_ShouldReturnTransformedLog()
    {
        var originalLog = "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2";
        var expectedLog = "\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT";

        var result = _service.TransformLog(originalLog);

        Assert.Equal(expectedLog, result);
    }

    [Fact]
    public async Task TransformLogFromUrl_ShouldReturnTransformedLog()
    {
        var logUrl = "https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt";

        using (var client = new HttpClient())
        {
            var logContent = await client.GetStringAsync(logUrl);

            var transformedLog = _service.TransformLog(logContent);

            Assert.Contains("\"MINHA CDN\"", transformedLog);
        }
    }
    [Fact]
    public void TransformLog_InvalidLog_ShouldReturnNull()
    {
        var invalidLog = "312|200|HIT";
        var result = _service.TransformLog(invalidLog);

        Assert.Null(result);
    }

    [Fact]
    public void TransformLog_ValidLog_ShouldIncludeAllFields()
    {
        var validLog = "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2";
        var result = _service.TransformLog(validLog);

        Assert.Contains("\"MINHA CDN\"", result);
        Assert.Contains("GET", result);
        Assert.Contains("/robots.txt", result);
        Assert.Contains("200", result);
    }





}
