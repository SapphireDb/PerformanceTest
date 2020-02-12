using System;
using System.ComponentModel.DataAnnotations;
using SapphireDb.Attributes;

namespace PerformanceTestServer.Data.Models
{
    public class Entry
    {
        public Entry()
        {
            CreatedOn = DateTime.UtcNow;
        }
        
        [Key]
        public int Id { get; set; }
        
        public DateTime CreatedOn { get; set; }
    }
}