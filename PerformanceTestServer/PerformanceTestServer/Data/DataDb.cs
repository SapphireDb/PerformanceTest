using Microsoft.EntityFrameworkCore;
using PerformanceTestServer.Data.Models;

namespace PerformanceTestServer.Data
{
    public class DataDb : DbContext
    {
        public DataDb(DbContextOptions<DataDb> options) : base(options)
        {
            
        }
        
        public DbSet<DataEntry> Entries { get; set; }
    }
}