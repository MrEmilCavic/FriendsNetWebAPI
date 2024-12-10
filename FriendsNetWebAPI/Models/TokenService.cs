using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;

namespace FriendsNetWebAPI.Models
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        private readonly FriendsNetContext _context;

        public TokenService(IConfiguration configuration, FriendsNetContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public string GenerateRefreshToken()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var tokenBytes = new byte[32];
                rng.GetBytes(tokenBytes);
                return Convert.ToBase64String(tokenBytes);
            }
        }

        public async Task<string> CreateRefreshTokenAsync(int userId) {
            var refreshToken = GenerateRefreshToken();
            var expiration = DateTime.UtcNow.AddHours(3);

            var tokenEntity = new RefreshToken
            {
                Token = refreshToken,
                Expiration = expiration,
                UserId = userId
            };

            try
            {
                _context.RefreshToken.Add(tokenEntity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("ERROR saving refresh token!", ex);
            }

            return refreshToken;
        }
    }
}
