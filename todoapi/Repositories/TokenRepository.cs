using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using todoapi.Data;
using todoapi.Models;
using todoapi.Shared;

namespace todoapi.Repositories
{
    public class TokenRepository :ITokenRepository
    {
        private readonly AppDbContext _Tokencontext;

        public TokenRepository(AppDbContext context)
        {
            _Tokencontext = context;
        }

        public async Task AddToken(RefreshToken refreshToken)
        {
            await _Tokencontext.RefreshTokens.AddAsync(refreshToken);
            await _Tokencontext.SaveChangesAsync();
        }

        public async Task<RefreshToken> GetToken(string token)
        {
            return await _Tokencontext.RefreshTokens.SingleOrDefaultAsync(t => t.Token == token);
        }

        public async Task UpdateRefreshToken(RefreshToken refreshToken)
        {
            _Tokencontext.RefreshTokens.Update(refreshToken);
            await _Tokencontext.SaveChangesAsync();
        }

        public async Task RevokeRefreshToken(string token)
        {
            var refreshToken = await GetToken(token);
            if (refreshToken != null)
            {
                refreshToken.Revoked = DateTime.UtcNow;
                await _Tokencontext.SaveChangesAsync();
            }
        }

        public async Task<bool> IsRefreshTokenValid(string token)
        {
            var refreshToken = await GetToken(token);
            return refreshToken != null && refreshToken.IsActive;
        }

    }
}