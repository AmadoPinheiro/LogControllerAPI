using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogTransformer.Core.Entities;
using LogTransformer.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LogTransformer.Infrastructure
{
    public class LogRepository : ILogRepository
    {
        private readonly LogDbContext _context;

        public LogRepository(LogDbContext context)
        {
            _context = context;
        }

        public async Task AddLogAsync(LogEntry log)
        {
            await _context.Logs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<LogEntry> GetLogByIdAsync(int id)
        {
            return await _context.Logs.FindAsync(id);
        }

        public async Task<IEnumerable<LogEntry>> GetAllLogsAsync()
        {
            return await _context.Logs.ToListAsync();
        }
    }
}
