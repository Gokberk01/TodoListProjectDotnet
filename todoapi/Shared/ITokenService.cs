using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todoapi.Models;

namespace todoapi.Shared
{
    public interface ITokenService
    {
        Task AddRefreshToken(RefreshToken refreshToken);
        Task<RefreshToken> GetRefreshToken(string token);
        Task RevokeRefreshToken(string token);
        Task<bool> IsRefreshTokenValid(string token);
        string GenerateRefreshToken();

    }
}