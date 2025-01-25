using System;

namespace LogTransformer.Core.Entities
{
    public class LogEntry
    {
        public int Id { get; set; }
        public string OriginalLog { get; set; }
        public string TransformedLog { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
