using System;
using System.ComponentModel.DataAnnotations;

namespace PerformanceTestServer.Data.Models
{
    public class DataEntry
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }
        
        public double AverageDiff { get; set; }

        public DateTime Time { get; set; }
    }
}