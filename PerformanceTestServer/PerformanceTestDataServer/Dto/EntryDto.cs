using System;
using System.ComponentModel.DataAnnotations;

namespace PerformanceTestDataServer.Dto
{
    public class EntryDto
    {
        public Guid ClientId { get; set; }
        
        public double AverageServerDiff { get; set; }

        public double AverageClientDiff { get; set; }

        public DateTime ReceivedOn { get; set; }
    }
}