using System.ComponentModel.DataAnnotations;

namespace Demo_API_Input_Validation.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required] // The username is mandatory and must be specified
        [StringLength(20, MinimumLength = 4)]
        [RegularExpression("^[a-zA-Z0-9_-]+$", ErrorMessage = "The username may only contain letters, numbers, underscores or hyphens.")]
        public required string Username { get; set; }

        [Required] // The email is mandatory and must be specified
        [EmailAddress] // The email must conform to the correct format of an email address
        public required string Email { get; set; }

        [Required] // The password hash is mandatory and must be specified
        public required string PasswordHash { get; set; }

        [Required] // The password salt is mandatory and must be specified
        public required string PasswordSalt { get; set; }
    }
}
