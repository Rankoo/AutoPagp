using System;
using System.ComponentModel.DataAnnotations;

namespace AutoAlertDB.Models
{
    public class Notifications : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AlertId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? SentAt { get; set; }

        [MaxLength(100)]
        public string? Result { get; set; }

        

        public Alerts? Alert { get; set; }
        public Users? User { get; set; }
    }
}
