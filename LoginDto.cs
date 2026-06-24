using System.ComponentModel.DataAnnotations;

namespace VisitorMVC.Models.DTOs
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
