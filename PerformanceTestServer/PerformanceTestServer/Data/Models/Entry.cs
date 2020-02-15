using System;
using System.ComponentModel.DataAnnotations;
using SapphireDb.Attributes;

namespace PerformanceTestServer.Data.Models
{
    public class Entry
    {
        [Key]
        public int Id { get; set; }

        public Guid ClientId { get; set; }
        
        public DateTime CreatedOn { get; set; }
    }
}