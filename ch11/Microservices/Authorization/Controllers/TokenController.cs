using Authorization.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase // Defining the TokenController class which inherits from ControllerBase
    {
        private readonly ILogger<TokenController> _logger; // Declaring a private readonly field for the logger

        public TokenController(ILogger<TokenController> logger) // Constructor to initialize the logger
        {
            _logger = logger; // Assigning the logger
        }

        [HttpGet(Name = "GetToken")] // Defining an HTTP GET method with a name "GetToken"
        [AllowAnonymous] // Allowing anonymous access to this method
        public async Task<JWT> GetToken() // Asynchronous method to get a JWT token
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
    }
}
