using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedMessages.BasicTypes;
using Authorization.SampleDB;

namespace Authorization.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("User")]
    public class UserController : ControllerBase // Defining the UserController class which inherits from ControllerBase
    {
        private readonly ILogger<UserController> _logger; // Declaring a private readonly field for the logger

        public UserController(ILogger<UserController> logger) // Constructor to initialize the logger
        {
            _logger = logger; // Assigning the logger
        }

        [HttpPost("AddUser")]
        [AllowAnonymous]
        public async Task<IActionResult> AddUser([FromBody] UserBasicInfoMessage user)
        {
            if (user == null)
            {
                return BadRequest("User data cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest("Email and Password are required.");
            }

            bool isAdded = SampleDatabase.Instance.AddUser(user);

            if (isAdded)
            {
                return CreatedAtAction(nameof(AddUser), new { id = user.Id }, user); // HTTP 201 Created
            }
            else
            {
                return Conflict("Cannot add an user with an id already taken."); // HTTP 409 Conflict
            }
        }


        [HttpPost("UserProfile")]
        [AllowAnonymous]
        public async Task<IActionResult> UserProfile(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("User ID cannot be empty.");
            }
            if (SampleDatabase.Instance.GetUser(id) is UserBasicInfoMessage user)
            {
                return Ok(user); // HTTP 200 OK
            }
            else
            {
                return NotFound("User not found."); // HTTP 404 Not Found
            }
        }


        [HttpPost("ChangePassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword(Guid id, string oldPassword,  string newPassword)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("User ID cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                return BadRequest("New password cannot be empty.");
            }

            var user = SampleDatabase.Instance.GetUser(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (user.Password != oldPassword)
            {
                return Unauthorized("Old password is incorrect.");
            }

            user.Password = newPassword;
            bool isUpdated = SampleDatabase.Instance.UpdateUser(user);

            if (isUpdated)
            {
                return Ok($"Password updated successfully to {user.Password}."); // HTTP 200 OK
            }
            else
            {
                return StatusCode(500, "An error occurred while updating the password."); // HTTP 500 Internal Server Error
            }
        }

        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("User ID cannot be empty.");
            }

            var user = SampleDatabase.Instance.GetUser(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.Password = new Random(1234).Next().ToString();
            bool isUpdated = SampleDatabase.Instance.UpdateUser(user);

            if (isUpdated)
            {
                return Ok($"Password reset successfully. New Password {user.Password}"); // HTTP 200 OK
            }
            else
            {
                return StatusCode(500, "An error occurred while resetting the password."); // HTTP 500 Internal Server Error
            }
        }

    }
}
