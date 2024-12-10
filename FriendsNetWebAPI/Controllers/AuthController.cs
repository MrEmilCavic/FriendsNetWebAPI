using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using FriendsNetWebAPI.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace FriendsNetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly TokenService _tokenService;
        private readonly FriendsNetContext _context;
        private readonly IConfiguration _configuration;
        private readonly JwtSecurityTokenHandler _jwtHandler;


        public AuthController(FriendsNetContext context, IConfiguration configuration, TokenService tokenService)
        {
            _context = context;
            _configuration = configuration;
            _tokenService = tokenService;
            _jwtHandler = new JwtSecurityTokenHandler();
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignIn model)
        {
            var user = _context.Credentials.SingleOrDefault(c => c.userIdentification == model.userIdentification);

            if (user == null || !SecretHasher.VerifySecret(model.Secret, user.secret))
            {
                return Unauthorized("We can't seem to find your e-mail or passwords don't match :(");
            }

            
            var key = Encoding.ASCII.GetBytes(_configuration["JWT_SECRET"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.userIdentification),
                    new Claim(ClaimTypes.Name, user.userID.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["JWT_ISSUER"],
                Audience = _configuration["JWET_AUDIENCE"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var refreshToken = await _tokenService.CreateRefreshTokenAsync(user.userID);

            return Ok(new 
            { 
                Token = tokenString,
                RefreshToken = refreshToken
            });

        }


        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshToken model)
        {
            var tokenEntity = await _context.RefreshToken
                .SingleOrDefaultAsync(t => t.Token == model.Token && t.Expiration > DateTime.UtcNow);

            if(tokenEntity == null)
            {
                return Unauthorized("Token invalid or expired :(");
            }

            var principal = GetPrincipalFromExpiredToken(model.Token);
            if(principal == null)
            {
                return Unauthorized("Access token not valid!");
            }

            var userIdentification = principal.FindFirst(ClaimTypes.Email)?.Value;

            var user = await _context.Credentials.SingleOrDefaultAsync(c => c.userIdentification == userIdentification);
            if(user == null)
            {
                return Unauthorized();
            }

            var key = Encoding.ASCII.GetBytes(_configuration["JWT_SECRET"]);
            var newTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.userIdentification),
                    new Claim(ClaimTypes.Name, user.userID.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["JWT_ISSUER"],
                Audience = _configuration["JWT_AUDIENCE"]
            };

            var newToken = _jwtHandler.CreateToken(newTokenDescriptor);
            var newTokenString = _jwtHandler.WriteToken(newToken);
            var newRefreshToken = await _tokenService.CreateRefreshTokenAsync(user.userID);

            return Ok(new
            {
                Token = newTokenString,
                RefreshToken = newRefreshToken
            });
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["JWT_SECRET"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };

            try
            {
                return tokenHandler.ValidateToken(token, validationParameters, out _);
            }
            catch
            {
                return null;
            }
        }

    }
}