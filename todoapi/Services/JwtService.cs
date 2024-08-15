using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using todoapi.Models;

namespace todoapi.Services
{
    public class JwtService
    {
        private string secureKey = "this is a super rare and very secure key";

        public string Generate(int id, int expirationTime) 
        {
            //Standart security data merge with security key
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey));
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(credentials);

            //The data we want to encode (leave jwt for 1 day)
            var payload = new JwtPayload(id.ToString(), null, null, null, DateTime.UtcNow.AddMinutes(expirationTime));
            var securityToken = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);

        }

        public JwtSecurityToken Verify(string jwt, bool validateLifeTime = true)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secureKey);
            tokenHandler.ValidateToken(jwt, new TokenValidationParameters
            {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = validateLifeTime
            }
            , out SecurityToken validatedToken);
           

           return (JwtSecurityToken) validatedToken;
        }


    }
}