using System;
using System.ComponentModel.DataAnnotations;

namespace PerformanceTestDataServer.Data.Models
{
    public class Entry
    {
        [Key]
        public int Id { get; set; }

        public Guid ClientId { get; set; }
        
        public double AverageServerDiff { get; set; }

        public double AverageClientDiff { get; set; }

        public DateTime Time { get; set; }
    }
}