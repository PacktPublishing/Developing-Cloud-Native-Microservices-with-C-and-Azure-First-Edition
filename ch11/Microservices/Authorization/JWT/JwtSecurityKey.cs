using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Authorization.JWT
{
    internal class JwtSecurityKey // Defining an internal class named JwtSecurityKey
    {
        internal static SymmetricSecurityKey Create() // Defining a static method named Create that returns a SymmetricSecurityKey
        {
            // Creating and returning a new SymmetricSecurityKey using a predefined security key string
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes("chapter10-security-key-256-bits-togo"));
        }
    }
}