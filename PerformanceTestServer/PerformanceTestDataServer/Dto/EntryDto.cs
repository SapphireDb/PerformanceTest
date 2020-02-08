using System;
using System.ComponentModel.DataAnnotations;

namespace PerformanceTestDataServer.Dto
{
    public class EntryDto
    {
        public Guid ClientId { get; set; }
        
        public int DiffFromServer { get; set; }

        public int DiffFromClient { get; set; }

        public DateTime ReceivedOn { get; set; }
    }
}