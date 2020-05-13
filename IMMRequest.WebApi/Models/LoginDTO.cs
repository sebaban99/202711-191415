using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace IMMRequest.WebApi
{
    [ExcludeFromCodeCoverage]
    public class LoginDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}