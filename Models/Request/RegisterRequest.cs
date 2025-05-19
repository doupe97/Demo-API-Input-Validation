using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Demo_API_Input_Validation.Models.Request
{
    public class RegisterRequest : IValidatableObject
    {
        [Required] // The username is mandatory and must be specified in the register request
        [StringLength(20, MinimumLength = 4)] // The username must be between 4 and 20 characters long
        [RegularExpression("^[a-zA-Z0-9_-]+$", ErrorMessage = "The username may only contain letters, numbers, underscores or hyphens.")]
        public required string Username { get; set; }

        [Required] // The email is mandatory and must be specified in the register request
        [EmailAddress] // The email must be in the correct format of an email address
        public required string Email { get; set; }

        [Required] // The password is mandatory and must be specified in the register request
        [MinLength(14, ErrorMessage = "The password must be at least 14 characters long.")]
        public required string Password { get; set; }

        /// <summary>
        ///     Function for multi-factor password validation
        /// </summary>
        /// <param name="validationContext">Represents the validation context</param>
        /// <returns>A validation result</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Regex.IsMatch(Password, "[A-Z]"))
                yield return new ValidationResult("The password must contain a capital letter.", [nameof(Password)]);

            if (!Regex.IsMatch(Password, "[a-z]"))
                yield return new ValidationResult("The password must contain a lowercase letter.", [nameof(Password)]);

            if (!Regex.IsMatch(Password, "[0-9]"))
                yield return new ValidationResult("The password must contain a number.", [nameof(Password)]);

            if (!Regex.IsMatch(Password, "[\\W_]"))
                yield return new ValidationResult("The password must contain a special character.", [nameof(Password)]);
        }
    }
}
