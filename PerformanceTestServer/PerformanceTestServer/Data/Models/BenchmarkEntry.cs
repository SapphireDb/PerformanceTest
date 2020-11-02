using System;
using System.ComponentModel.DataAnnotations;

namespace PerformanceTestServer.Data.Models
{
    public class BenchmarkEntry
    {
        public BenchmarkEntry()
        {
            Time = DateTime.UtcNow;
        }
        
        [Key]
        public int Id { get; set; }
        
        public DateTime Time { get; set; }
    }
}