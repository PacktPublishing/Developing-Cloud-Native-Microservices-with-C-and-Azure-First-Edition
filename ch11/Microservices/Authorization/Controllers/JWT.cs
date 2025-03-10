using System.IdentityModel.Tokens.Jwt;

namespace Authorization.Controllers
{
    public class JWT
    {
        // Private field to store the JWT token
        private JwtSecurityToken token;

        // Internal constructor to initialize the JWT with a given token
        internal JWT(JwtSecurityToken token)
        {
            this.token = token;
        }

        // Property to get the expiration date and time of the token
        public DateTime ValidTo => token.ValidTo;

        // Property to get the string representation of the token
        public string Value => new JwtSecurityTokenHandler().WriteToken(this.token);
    }
}