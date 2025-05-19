using Demo_API_Input_Validation.Data;
using Demo_API_Input_Validation.Models;
using Demo_API_Input_Validation.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Demo_API_Input_Validation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(SqliteDbContext sqliteDbContext) : ControllerBase
    {
        // Database context for storing data in the local SQLite database
        private readonly SqliteDbContext _sqliteDbContext = sqliteDbContext;

        /// <summary>
        ///     API endpoint to register a user
        /// </summary>
        /// <param name="request">Represents the register request</param>
        /// <returns>Status code 200, if the user registration was successful, else status code 400 (bad request)</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // Check for duplicate usernames
            if (await _sqliteDbContext.Users.AnyAsync(u => u.Username == request.Username))
                return BadRequest("The username is already taken.");

            // Check for duplicate email addresses
            if (await _sqliteDbContext.Users.AnyAsync(u => u.Email == request.Email))
                return BadRequest("The email address is already registered.");

            var salt = GenerateSalt();
            var hashedPassword = HashPassword(request.Password, salt);

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = hashedPassword,
                PasswordSalt = salt
            };

            // Store user data in local SQLite database
            _sqliteDbContext.Users.Add(user);
            await _sqliteDbContext.SaveChangesAsync();

            return Ok("The user was successfully registered.");
        }

        /// <summary>
        ///     API endpoint for logging in a user
        /// </summary>
        /// <param name="request">Represents the login request</param>
        /// <returns>Status code 200, if the user login was successful, else status code 401 (unauthorized)</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Retrieve the user from the local SQLite database using the specified username
            var user = await _sqliteDbContext.Users.SingleOrDefaultAsync(u => u.Username == request.Username);

            // Verification of the password with the calculated password hash
            if (user == null || !VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
                // Return a generic error message
                // An attacker receives no information about whether the user, the password or both is / are incorrect
                return Unauthorized("The username or password is invalid.");

            return Ok("The login was successful.");
        }

        /// <summary>
        ///     Hashes the specified password for secure storage in the local SQLite database
        /// </summary>
        /// <param name="password">Represents the cleartext password</param>
        /// <param name="salt">Represents the salt for password hashing</param>
        /// <returns>A base64 string representation of the SHA512 hashed password</returns>
        private string HashPassword(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA512);
            var hash = pbkdf2.GetBytes(64);
            return Convert.ToBase64String(hash);
        }

        /// <summary>
        ///     Verifies the specified password based on the stored password hash
        /// </summary>
        /// <param name="input">Represents the cleartext password to verify against the stored hash</param>
        /// <param name="storedHash">Represents the stored password hash</param>
        /// <param name="salt">Represents the salt for hashing</param>
        /// <returns>True, if the password is correct, else false</returns>
        private bool VerifyPassword(string input, string storedHash, string salt)
        {
            return HashPassword(input, salt) == storedHash;
        }

        /// <summary>
        ///     Generates a salt for password hashing
        /// </summary>
        /// <returns>A base64 string representation of the generated salt</returns>
        private string GenerateSalt()
        {
            var salt = new byte[32];
            using var random = RandomNumberGenerator.Create();
            random.GetBytes(salt);
            return Convert.ToBase64String(salt);
        }
    }
}
