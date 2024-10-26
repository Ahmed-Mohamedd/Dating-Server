﻿using Api.Data.Entities;
using Api.Helpers;
using Api.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Services
{
    public class TokenProvider : ITokenProvider
    {
        private readonly JWT _jwt;
        public TokenProvider(IOptions<JWT> jwt)
        {
            _jwt = jwt.Value;
        }
        public async Task<string> CreateToken(AppUser user)
        {

            var claims = new List<Claim>
            {
                new Claim (JwtRegisteredClaimNames.NameId , user.Username),
                new Claim (JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString())
            };
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredintials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(double.Parse(_jwt.ExpiresOn.ToString())),
                signingCredentials: signingCredintials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}