using System;
using System.ComponentModel.DataAnnotations;
using SapphireDb.Attributes;

namespace PerformanceTestServer.Data.Models
{
    [CreateEvent(nameof(OnCreate))]
    [UpdateEvent(nameof(OnUpdate))]
    public class Entry
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        private void OnCreate()
        {
            CreatedOn = DateTime.UtcNow;
        }

        private void OnUpdate()
        {
            UpdatedOn = DateTime.UtcNow;
        }
    }
}