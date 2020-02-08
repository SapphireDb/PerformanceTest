using Microsoft.EntityFrameworkCore;
using PerformanceTestServer.Data.Models;
using SapphireDb;

namespace PerformanceTestServer.Data
{
    public class Db : SapphireDbContext
    {
        public Db(DbContextOptions<Db> options, SapphireDatabaseNotifier notifier) : base(options, notifier)
        {
        }

        public DbSet<Entry> Entries { get; set; }
    }
}