using Auth.Domain.Contracts;
using Auth.Domain.Entities;
using CommanLib;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Service
{
    public  class TokenService(IOptions<JwtOption> options,ITokenRefreshRepository refreshRepository) : ITokenService
    {
        public async Task<RefreshToken> CreateRefreshTokenAsync(Guid userId)
        {
            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Token = GenerateRefreshToken(),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await refreshRepository.AddAsync(refreshToken);

            return refreshToken;

        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }

        }

        public string GenerateAccessToken(AppUser appUser, IList<string> roles)
        { 
          var jwt= options.Value;
            //create Claim 
            List<Claim> claims = [
                new (JwtRegisteredClaimNames.Name,appUser.UserName),
                new(JwtRegisteredClaimNames.PhoneNumber,appUser.PhoneNumber),
                new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString())

                ];


            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            //key and credentials
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey));

            var cred =new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims:claims,
                issuer: jwt.Issuer ,
                audience:jwt.Audience ,
                signingCredentials: cred,
                expires: DateTime.UtcNow.AddMinutes(60)

                );
            return  new JwtSecurityTokenHandler().WriteToken(token);

        }

        public async Task<RefreshToken?> GetByRefreshTokenAsync(string token)
        {

            return await refreshRepository.GetByTokenAsync(token);
        }

        public async Task RevokeRefreshTokenAsync(string token)
        {

            var refreshToken =
              await refreshRepository.GetByTokenAsync(token);

            if (refreshToken == null)
                return;

            refreshToken.RevokedAt = DateTime.UtcNow;

            await refreshRepository.UpdateAsync(refreshToken);

        }

        public async Task<bool> ValidateRefreshTokenAsync(string token)
        {

            var refreshToken =
        await refreshRepository.GetByTokenAsync(token);

            if (refreshToken == null)
                return false;

            if (refreshToken.IsRevoked)
                return false;

            if (refreshToken.ExpiresAt <= DateTime.UtcNow)
                return false;

            return true;

        }
    }
}
