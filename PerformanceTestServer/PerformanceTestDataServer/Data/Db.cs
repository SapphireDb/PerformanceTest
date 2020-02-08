using Microsoft.EntityFrameworkCore;
using PerformanceTestDataServer.Data.Models;

namespace PerformanceTestDataServer.Data
{
    public class Db : DbContext
    {
        public Db(DbContextOptions<Db> options) : base(options)
        {
            
        }

        public DbSet<Entry> Entries { get; set; }
    }
}