namespace LogTransformer.Api.Models
{

    public class LogResponseDto
    {
        public int Id { get; set; }
        public string OriginalLog { get; set; }
        public string TransformedLog { get; set; }
    }
}