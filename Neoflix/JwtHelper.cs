using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Neoflix
{
    /// <summary>
    /// Helper class for working with JSON Web tokens.<br/>
    /// for more info on JSON Web tokens visit: https://jwt.io/.
    /// </summary>
    public static class JwtHelper
    {
        private static readonly SymmetricSecurityKey SigningKey;

        static JwtHelper()
        {
            var secret = Config.UnpackJwtConfig();

            var secretBytes = Encoding.UTF8.GetBytes(secret);
            SigningKey = new SymmetricSecurityKey(secretBytes);
        }

        /// <summary>
        /// Configure JwtBearerOptions to check only signed.
        /// </summary>
        /// <param name="opts"></param>
        public static void ConfigureJwt(JwtBearerOptions opts)
        {
            opts.RequireHttpsMetadata = false;
            opts.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = SigningKey,
                ValidateIssuerSigningKey = true,
                RequireSignedTokens = true,
                ValidateLifetime = false
            };
        }

        /// <summary>
        /// Create a JWT.
        /// </summary>
        /// <param name="dict">Claims in a dictionary.</param>
        /// <returns>encoded JWT</returns>
        public static string CreateToken(Dictionary<string, object> dict)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateJwtSecurityToken(new SecurityTokenDescriptor
            {
                Claims = dict,
                SigningCredentials = new SigningCredentials(SigningKey, "HS256")
            });
            return tokenHandler.WriteToken(token);
        }
    }
}