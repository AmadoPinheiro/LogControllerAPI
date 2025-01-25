using System.Collections.Generic;
using System.Threading.Tasks;
using LogTransformer.Core.Entities;

namespace LogTransformer.Core.Interfaces
{
    public interface ILogRepository
    {
        Task AddLogAsync(LogEntry log);
        Task<LogEntry> GetLogByIdAsync(int id);
        Task<IEnumerable<LogEntry>> GetAllLogsAsync();
    }
}
