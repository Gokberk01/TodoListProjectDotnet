using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using todoapi.Models;
using todoapi.Repositories;
using todoapi.Shared;


namespace todoapi.Services
{
    public class TokenService : ITokenService
    {
        private readonly ITokenRepository _repository;

        public TokenService(ITokenRepository repository)
        {
            _repository = repository;
        }

        public async Task AddRefreshToken(RefreshToken refreshToken)
        {
            await _repository.AddToken(refreshToken);
        }

        public async Task<RefreshToken> GetRefreshToken(string token)
        {
            return await _repository.GetToken(token);
        }

        public async Task RevokeRefreshToken(string token)
        {
            var refreshToken = await _repository.GetToken(token);
            if(refreshToken.Revoked == null)
            {
                refreshToken.Revoked = DateTime.UtcNow;
                await _repository.UpdateRefreshToken(refreshToken);
            }
        }

        public async Task<bool> IsRefreshTokenValid(string token)
        {
            return await _repository.IsRefreshTokenValid(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

    }
}