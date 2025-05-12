using Authorization.JWT;
using Authorization.SampleDB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedMessages.BasicTypes;

namespace Authorization.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("Authorization")]
    public class AuthorizationController : ControllerBase // Defining the TokenController class which inherits from ControllerBase
    {
        private readonly ILogger<AuthorizationController> _logger; // Declaring a private readonly field for the logger

        public AuthorizationController(ILogger<AuthorizationController> logger) // Constructor to initialize the logger
        {
            _logger = logger; // Assigning the logger
        }

        private async Task<JWTData> GenerateTokenAsync()
        {
            var token = new JWTBuilder() // Creating a new JWTBuilder instance
                        .AddSecurityKey(JwtSecurityKey.Create()) // Adding a security key
                        .AddSubject("Chapter10.Subject") // Adding a subject
                        .AddIssuer("Chapter10.Issuer") // Adding an issuer
                        .AddAudience("Chapter10.Audience") // Adding an audience
                        .AddClaim("Chapter10.ClainType", "Chapter10.ClainValue") // Adding a claim
                        .AddExpiry(10) // Adding an expiry time of 10 minutes
                        .Build(); // Building the token

            return token; // Returning the generated token
        }

        [HttpPost("Login")] // Defining an HTTP GET method with a name "GetToken"
        [AllowAnonymous] // Allowing anonymous access to this method
        public async Task<IActionResult> Login([FromBody] LoginBasicInfoMessage login) // Asynchronous method to get a JWT token
        {
            if (login == null)
            {
                return BadRequest("Login data cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
            {
                return BadRequest("Email and Password are required.");
            }
            // Here you would typically validate the user's credentials against a database or other data source.
            var user = SampleDatabase.Instance.ValidateUser(login.Email, login.Password);
            if (user != null) 
            {
                var token = await GenerateTokenAsync();
                AuthorizedUserMessage authorizedUserMessage = new AuthorizedUserMessage(); // Creating a new instance of AuthorizedUserMessage
                authorizedUserMessage.UserId = user.Id; // Setting the UserId   
                authorizedUserMessage.DisplayName = user.DisplayName; 
                authorizedUserMessage.Token = token.Value;
                authorizedUserMessage.ValidTo = token.ValidTo;
                return Ok(authorizedUserMessage);
            }
            else
            {
                return Unauthorized("Invalid email or password."); // Return unauthorized if invalid
            }
        }

        [HttpPost("Renew")] // Defining an HTTP GET method with a name "GetToken"
        public async Task<IActionResult> Renew([FromBody] AuthorizedUserMessage authorizedUser) // Asynchronous method to get a JWT token
        {
            if (authorizedUser == null)
            {
                return BadRequest("Login data cannot be null.");
            }
            if (DateTime.Now > authorizedUser.ValidTo)
            {
                return Unauthorized("Token expired."); // Return unauthorized if invalid
            }

            var user = SampleDatabase.Instance.GetUser(authorizedUser.UserId);
            if (user != null)
            {
                var token = await GenerateTokenAsync();
                AuthorizedUserMessage authorizedUserMessage = new AuthorizedUserMessage(); // Creating a new instance of AuthorizedUserMessage
                authorizedUserMessage.UserId = user.Id; // Setting the UserId   
                authorizedUserMessage.DisplayName = user.DisplayName;
                authorizedUserMessage.Token = token.Value;
                authorizedUserMessage.ValidTo = token.ValidTo;

                return Ok(authorizedUserMessage);
            }
            else
            {
                return Unauthorized("Invalid user."); // Return unauthorized if invalid
            }
        }
    }
}
