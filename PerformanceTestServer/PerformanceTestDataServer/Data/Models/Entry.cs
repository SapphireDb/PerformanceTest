using System;
using System.ComponentModel.DataAnnotations;

namespace PerformanceTestDataServer.Data.Models
{
    public class Entry
    {
        [Key]
        public int Id { get; set; }

        public Guid ClientId { get; set; }
        
        public double AverageDiffFromServer { get; set; }

        public double AverageDiffFromClient { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}