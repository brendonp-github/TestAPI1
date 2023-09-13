using System.ComponentModel.DataAnnotations;

namespace TestAPI1.Data
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email is required dude!")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Password is required dude!")]
        public string Password { get; set; } = null!;
    }
}
