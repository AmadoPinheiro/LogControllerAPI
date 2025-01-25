using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace LogTransformer.Tests
{
    public class LogsControllerTests : IClassFixture<WebApplicationFactory<Api.Startup>>
    {
        private readonly HttpClient _client;

        public LogsControllerTests(WebApplicationFactory<Api.Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task AddLog_ShouldReturnOk()
        {
            var log = new
            {
                OriginalLog = "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2"
            };
            var content = new StringContent(JsonConvert.SerializeObject(log), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/logs", content);

            var responseMessage = response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.NotNull(responseMessage.Content);

        }

        [Fact]
        public async Task TransformLogWithOptions_ShouldReturnTransformedLogAndSaveToFile()
        {
            var response = await _client.GetAsync("/api/logs/transform/1?saveToFile=true");

            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<dynamic>(responseContent);

            Assert.NotNull(result);
            Assert.Contains("logs", result["filePath"].ToString());

            Assert.True(System.IO.File.Exists(result["filePath"].ToString()));
        }

        [Fact]
        public async Task AddLog_InvalidLog_ShouldReturnBadRequest()
        {
            var invalidLog = new { OriginalLog = "312|200|HIT" };
            var content = new StringContent(JsonConvert.SerializeObject(invalidLog), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/logs", content);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task TransformLogById_ShouldReturnTransformedLog()
        {
            var response = await _client.GetAsync("/api/logs/transform/1");

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<dynamic>(responseContent);

            var transformedLog = result["transformedLog"].ToString();

            Assert.Contains("#Version: 1.0", transformedLog);
            Assert.Contains("\"MINHA CDN\" GET", transformedLog);
        }
        [Fact]
        public async Task TransformLogs_MultipleLogs_ShouldReturnTransformedLogs()
        {
            var logs = new[]
            {
                "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2",
                "101|200|MISS|\"POST /myImages HTTP/1.1\"|319.4"
            };

            var content = new StringContent(JsonConvert.SerializeObject(logs), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/logs/transform", content);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<dynamic>(responseContent);

            var transformedLogs = result["transformedLogs"].ToString();

            Assert.Contains("#Version: 1.0", transformedLogs);
            Assert.Contains("\"MINHA CDN\" GET 200 /robots.txt", transformedLogs);
            Assert.Contains("\"MINHA CDN\" POST 200 /myImages", transformedLogs);
        }

        [Fact]
        public async Task TransformLogs_SingleLog_ShouldReturnTransformedLog()
        {
            var log = "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2";

            var content = new StringContent(JsonConvert.SerializeObject(new[] { log }), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/logs/transform", content);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<dynamic>(responseContent);

            var transformedLogs = result["transformedLogs"].ToString();

            Assert.Contains("#Version: 1.0", transformedLogs);
            Assert.Contains("\"MINHA CDN\" GET 200 /robots.txt", transformedLogs);
        }


    }
}
