using System;
using System.ComponentModel.DataAnnotations;

namespace PerformanceTestServer.Data.Models
{
    public class BenchmarkEntry
    {
        [Key]
        public int Id { get; set; }
        
        public Guid ClientId { get; set; }
    }
}