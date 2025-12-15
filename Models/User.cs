using System.ComponentModel.DataAnnotations;

namespace CafeManagementSystem.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string Role { get; set; } // Admin / Staff
    }
}
