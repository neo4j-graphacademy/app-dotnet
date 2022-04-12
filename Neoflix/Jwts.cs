using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Neoflix
{
    public class Jwts
    {
        private static readonly SymmetricSecurityKey SigningKey;
        private static readonly string Issuer;
        private static readonly string Audience;

        static Jwts()
        {
            var (secret, audience, issuer) = Config.UnpackJwtConfig();

            var secretBytes = Encoding.UTF8.GetBytes(secret);
            SigningKey = new SymmetricSecurityKey(secretBytes);
            Audience = audience;
            Issuer = issuer;
        }

        public static void ConfigureNeoflixJwt(JwtBearerOptions opts)
        {
            opts.RequireHttpsMetadata = false;
            opts.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidAudience = Audience,
                ValidateIssuer = false,
                ValidIssuer = Issuer,
                IssuerSigningKey = SigningKey,
                ValidateIssuerSigningKey = true,
                RequireSignedTokens = true,
                ValidateLifetime = false
            };
        }

        public static string CreateToken(Dictionary<string, object> dict)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateJwtSecurityToken(new SecurityTokenDescriptor
            {
                AdditionalHeaderClaims = dict,
                SigningCredentials = new SigningCredentials(SigningKey, "HS256")
            });
            return tokenHandler.WriteToken(token);
        }
    }
}