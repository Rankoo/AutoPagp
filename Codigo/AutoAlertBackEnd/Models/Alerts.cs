using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AutoAlertBackEnd.Models
{
    public class Alerts : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ServiceId { get; set; }
        public DateTime ScheduledAt { get; set; }
        [MaxLength(50)]
        public string? Channel { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }
        
        public Services? Service { get; set; }

        [JsonIgnore]
        public ICollection<Notifications>? Notifications { get; set; }
    }
}
