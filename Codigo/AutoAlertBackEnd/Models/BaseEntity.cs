using System;

namespace AutoAlertBackEnd.Models
{
    /// <summary>
    /// Base entity that provides timestamp properties for all models.
    /// </summary>
    public abstract class BaseEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
