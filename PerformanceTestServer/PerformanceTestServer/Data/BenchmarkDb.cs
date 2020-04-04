using Microsoft.EntityFrameworkCore;
using PerformanceTestServer.Data.Models;
using SapphireDb;

namespace PerformanceTestServer.Data
{
    public class BenchmarkDb : SapphireDbContext
    {
        public BenchmarkDb(DbContextOptions options, SapphireDatabaseNotifier notifier) : base(options, notifier)
        {
        }
        
        public DbSet<BenchmarkEntry> Entries { get; set; }
    }
}