
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ch10.Controllers
{
    internal class JWTBuilder // Defining the JWTBuilder class
    {
        private SecurityKey securityKey = null; // Private field to store the security key
        private string subject = ""; // Private field to store the subject
        private string issuer = ""; // Private field to store the issuer
        private string audience = ""; // Private field to store the audience
        private Dictionary<string, string> claims = new Dictionary<string, string>(); // Private field to store the claims
        private int expiryInMinutes = 5; // Private field to store the expiry time in minutes

        public JWTBuilder() // Constructor for JWTBuilder
        {
        }

        public JWTBuilder AddSecurityKey(SecurityKey securityKey) // Method to add security key
        {
            this.securityKey = securityKey; // Setting the security key
            return this; // Returning the current instance
        }

        public JWTBuilder AddSubject(string subject) // Method to add subject
        {
            this.subject = subject; // Setting the subject
            return this; // Returning the current instance
        }

        public JWTBuilder AddIssuer(string issuer) // Method to add issuer
        {
            this.issuer = issuer; // Setting the issuer
            return this; // Returning the current instance
        }

        public JWTBuilder AddAudience(string audience) // Method to add audience
        {
            this.audience = audience; // Setting the audience
            return this; // Returning the current instance
        }

        public JWTBuilder AddClaim(string type, string value) // Method to add a single claim
        {
            this.claims.Add(type, value); // Adding the claim to the dictionary
            return this; // Returning the current instance
        }

        public JWTBuilder AddClaims(Dictionary<string, string> claims) // Method to add multiple claims
        {
            this.claims.Union(claims); // Adding the claims to the dictionary
            return this; // Returning the current instance
        }

        public JWTBuilder AddExpiry(int expiryInMinutes) // Method to add expiry time
        {
            this.expiryInMinutes = expiryInMinutes; // Setting the expiry time
            return this; // Returning the current instance
        }

        public JWT  Build() // Method to build the JWT
        {
            var claims = new List<Claim> // Creating a list of claims
                {
                    new Claim(JwtRegisteredClaimNames.Sub,this.subject), // Adding the subject claim
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Adding a unique identifier claim
                }.Union(this.claims.Select(item => new Claim(item.Key, item.Value))); // Adding the additional claims

            var token = new JwtSecurityToken( // Creating the JWT token
                issuer: this.issuer, // Setting the issuer
                audience: this.audience, // Setting the audience
                claims: claims, // Setting the claims
                expires: DateTime.UtcNow.AddMinutes(expiryInMinutes), // Setting the expiry time
                signingCredentials: new SigningCredentials( // Setting the signing credentials
                                                   this.securityKey, // Using the security key
                                                   SecurityAlgorithms.HmacSha256) // Using the HMAC SHA256 algorithm
                );

            return new JWT(token); // Returning the JWT
        }
    }
}