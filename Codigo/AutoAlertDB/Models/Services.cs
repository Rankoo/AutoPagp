using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutoAlertDB.Models
{
    public class Services : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid StoreId { get; set; }
    [Required, MaxLength(150)]
    public required string Name { get; set; }

    [MaxLength(100)]
    public string? Provider { get; set; }

    [MaxLength(100)]
    public string? AccountNumber { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal? Amount { get; set; }
    [MaxLength(50)]
    public string? Status { get; set; }
    public DateTime? LastCheck { get; set; }
    public bool AlertSent { get; set; }

        public Stores? Store { get; set; }
        public ICollection<Alerts>? Alerts { get; set; }
    }
}
