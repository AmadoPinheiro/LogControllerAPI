using System.ComponentModel.DataAnnotations;

namespace LogTransformer.Api.Models
{
    public class LogEntryDto
    {
        [Required(ErrorMessage = "O log original é obrigatório.")]
        public string OriginalLog { get; set; }
    }
}
