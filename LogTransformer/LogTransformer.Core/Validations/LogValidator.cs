using System;

namespace LogTransformer.Core.Validations
{
    public static class LogValidator
    {
        public static bool IsValidLogFormat(string log)
        {
            if (string.IsNullOrEmpty(log)) return false;

            var logParts = log.Split('|');
            if (logParts.Length < 5) return false;

            if (!int.TryParse(logParts[0], out _)) return false; 
            if (!int.TryParse(logParts[1], out _)) return false; 
            if (!double.TryParse(logParts[4], out _)) return false;

            return true;
        }

        public static void EnsureValidLog(string log)
        {
            if (!IsValidLogFormat(log))
            {
                throw new ArgumentException("Formato do log inválido. O log deve conter pelo menos 5 campos separados por '|', e os campos devem ser válidos.");
            }
        }
    }
}
