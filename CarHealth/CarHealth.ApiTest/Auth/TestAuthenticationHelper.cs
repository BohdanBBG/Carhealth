using CarHealth.Api.Models.IdentityModels;
using Microsoft.IdentityModel.Tokens;
using NUlid;
using NUlid.Rng;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CarHealth.ApiTest.Auth
{
    public static class TestAuthenticationHelper
    {
        public const string Audience = "test-audience";
        public const string Authority = "test-authority";

        public static string GenerateAccessToken(User userEntity)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();

            var secret = Ulid.NewUlid(new CSUlidRng()).ToString();
            //var key = Encoding.ASCII.GetBytes(secret);
            var valueKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("mysupersecret_secretkey!123"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("sub", userEntity.Id),

                    // permissions claim
                    //new Claim(PermissionConstants.PackedPermissionClaimType, PermissionPackHelper.PackPermissionsIntoString(userEntity.Permissions))
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(valueKey, SecurityAlgorithms.HmacSha256Signature),
                //SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Audience = Audience,
                Issuer = Authority,
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtAccessToken = tokenHandler.WriteToken(token);
            return jwtAccessToken;
        }

        public static bool ValidateAccessToken(string jwtAccessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.ReadToken(jwtAccessToken) as JwtSecurityToken;
            bool isValid = token.Audiences.Contains(Audience) && token.Issuer == Authority && token.ValidTo >= DateTime.UtcNow;
            return isValid;
        }

        public static ClaimsIdentity GetIdentityFromAccessToken(string jwtAccessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.ReadToken(jwtAccessToken) as JwtSecurityToken;
            var identity = new ClaimsIdentity(
                new Claim[]
                {
                    new Claim("sub", token.Subject), // userId
                },
                authenticationType: TestAuthenticationDefaults.AuthenticationScheme
            );

            // add other claims from token including
            // permissions claim
            identity.AddClaims(token.Claims);

            return identity;
        }
    }
}
