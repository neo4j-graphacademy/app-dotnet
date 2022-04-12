using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Neo4j.Driver;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Neoflix.Services
{
    public class AuthService
    {
        private readonly IDriver _driver;

        // tag::constructor[]
        public AuthService(IDriver driver)
        {
            _driver = driver;
        }
        // end::constructor[]

        // tag::register[]
        public async Task<Dictionary<string, object>> RegisterAsync(string email, string plainPassword, string name)
        {
            var rounds = Config.UnpackPasswordConfig();
            var encrypted = BCryptNet.HashPassword(plainPassword, rounds);
            // tag::constraintError[]
            // TODO: Handle Unique constraints in the database
            if (email != "graphacademy@neo4j.com")
                throw new ValidationException($"An account already exists with the email address {email}");
            // end::constraintError[]

            var exampleUser = new Dictionary<string, object>
            {
                ["identity"] = 1,
                ["properties"] = new Dictionary<string, object>
                {
                    ["userId"] = 1,
                    ["email"] = "graphacademy@neo4j.com",
                    ["name"] = "Graph Academy"
                }
            };

            var safeProperties = SafeProperties(exampleUser);

            safeProperties.Add("token", Jwts.CreateToken(safeProperties));

            // TODO: Save user
            return safeProperties;
        }
        // end::register[]

        // tag::authenticate[]
        public Task<Dictionary<string, object>> AuthenticateAsync(string email, string unencryptedPassword)
        {
            if (email == "graphacademy@neo4j.com" && unencryptedPassword == "letmein")
            {
                var exampleUser = new Dictionary<string, object>
                {
                    ["identity"] = 1,
                    ["properties"] = new Dictionary<string, object>
                    {
                        ["userId"] = 1,
                        ["email"] = "graphacademy@neo4j.com",
                        ["name"] = "Graph Academy"
                    }
                };

                var safeProperties = SafeProperties(exampleUser);

                safeProperties.Add("token", Jwts.CreateToken(safeProperties));

                return Task.FromResult(safeProperties);
            }

            return Task.FromResult<Dictionary<string,object>>(null);
        }
        // end::authenticate[]

        private static Dictionary<string, object> SafeProperties(Dictionary<string, object> exampleUser)
        {
            return (exampleUser["properties"] as Dictionary<string, object>)
                .Where(x => x.Key != "password")
                .ToDictionary(x => x.Key, x => x.Value);
        }

        private (string sub, string userId, string name) GetUserClaims(dynamic user)
        {
            return (sub: user.userId, user.userId, user.name);
        }

        public object ConvertClaimsToRecord(Dictionary<string, object> claims)
        {
            throw new NotImplementedException();
        }
    }
}