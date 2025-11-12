using System.ComponentModel.DataAnnotations;

namespace AutoAlertDB.Models
{
    public class LogIn
    {
        [Required]
        public required string UserName { get; set; } = string.Empty!;

        [Required]
        public required string Password { get; set; } = string.Empty!;
    }
}
