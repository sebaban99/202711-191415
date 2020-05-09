using System.ComponentModel.DataAnnotations;

namespace IMMRequest.WebApi
{
    public class LoginDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}