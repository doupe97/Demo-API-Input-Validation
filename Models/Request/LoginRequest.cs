using System.ComponentModel.DataAnnotations;

namespace Demo_API_Input_Validation.Models.Request
{
    public class LoginRequest
    {
        [Required] // The username is mandatory and must be specified in the login request
        public required string Username { get; set; }

        [Required] // The password is mandatory and must be specified in the login request
        public required string Password { get; set; }
    }
}
