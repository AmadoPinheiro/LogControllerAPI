using System;
using System.Collections.Generic;
using System.Linq;
using LogTransformer.Core.Entities;

namespace LogTransformer.Core.Services
{
    public class LogTransformerService
    {
        public IEnumerable<string> TransformLogs(IEnumerable<LogEntry> logs)
        {
            return logs.Select(log => TransformLog(log.OriginalLog));
        }

        public string TransformLog(string originalLog)
        {
            var parts = originalLog.Split('|');
            if (parts.Length < 5)
            {
                return null;
            }

            var provider = "\"MINHA CDN\"";
            var methodAndPath = parts[3].Trim('"').Split(' ');
            var method = methodAndPath[0];
            var path = methodAndPath[1];

            return $"{provider} {method} {parts[1]} {path} {int.Parse(parts[4].Split('.')[0])} {parts[0]} {parts[2]}";
        }

        public string TransformLogWithMetadata(IEnumerable<string> logs)
        {
            var header = $"#Version: 1.0\n#Date: {DateTime.UtcNow:dd/MM/yyyy HH:mm:ss}\n" +
                         "#Fields: provider http-method status-code uri-path time-taken response-size cache-status";

            var transformedLogs = logs.Select(TransformLog);

            return $"{header}\n{string.Join("\n", transformedLogs)}";
        }

        public string TransformLogAgoraFormat(string log)
        {
            var parts = log.Split('|');
            if (parts.Length != 5) return null;

            var responseSize = int.Parse(parts[0]);
            var statusCode = int.Parse(parts[1]);
            var cacheStatus = ConvertCacheStatus(parts[2]);
            var requestParts = parts[3].Trim('"').Split(' '); // Exemplo: "GET /robots.txt HTTP/1.1"
            var timeTaken = (int)Math.Round(double.Parse(parts[4]));

            return $"\"MINHA CDN\" {requestParts[0]} {statusCode} {requestParts[1]} {timeTaken} {responseSize} {cacheStatus}";
        }

        public string AddHeaderToLogs(List<string> transformedLogs)
        {
            var header = "#Version: 1.0\n" +
                         $"#Date: {DateTime.UtcNow:dd/MM/yyyy HH:mm:ss}\n" +
                         "#Fields: provider http-method status-code uri-path time-taken response-size cache-status\n";
            return header + string.Join("\n", transformedLogs);
        }


        private string ConvertCacheStatus(string status)
        {
            switch (status)
            {
                case "HIT":
                    return "HIT";
                case "MISS":
                    return "MISS";
                case "INVALIDATE":
                    return "REFRESH_HIT";
                default:
                    return status;
            }
        }


        //public string AddHeaderToLogs(IEnumerable<string> logs)
        //{
        //    var header = $"#Version: 1.0\n#Date: {DateTime.UtcNow:dd/MM/yyyy HH:mm:ss}\n" +
        //                 "#Fields: provider http-method status-code uri-path time-taken response-size cache-status";
        //    return $"{header}\n{string.Join("\n", logs)}";
        //}


    }
}
