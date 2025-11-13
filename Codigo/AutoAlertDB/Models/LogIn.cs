using System.ComponentModel.DataAnnotations;

namespace AutoAlertDB.Models
{
    public class LogIn
    {
        [Required]
        public required string Email { get; set; } = string.Empty!;

        [Required]
        public required string Password { get; set; } = string.Empty!;
    }
}
