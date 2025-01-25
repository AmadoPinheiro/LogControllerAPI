using Microsoft.EntityFrameworkCore;
using LogTransformer.Core.Entities;
using System.Collections.Generic;

namespace LogTransformer.Infrastructure
{
    public class LogDbContext : DbContext
    {
        public LogDbContext(DbContextOptions<LogDbContext> options)
            : base(options)
        {
        }

        public DbSet<LogEntry> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LogEntry>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OriginalLog).IsRequired();
                entity.Property(e => e.TransformedLog).IsRequired(false);
            });
        }
    }
}
