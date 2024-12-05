using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
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
        private readonly FriendsNetContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(FriendsNetContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("signin")]
        public IActionResult SignIn([FromBody] SignIn model)
        {
            var user = _context.Credentials.SingleOrDefault(c => c.userIdentification == model.userIdentification);

            if (user == null || !SecretHasher.VerifySecret(model.Secret, user.secret))
            {
                return Unauthorized("We can't seem to find your e-mail or passwords don't match :(" + model.Secret + " XX " + user.secret);
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT_SECRET"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.userIdentification),
                    new Claim(ClaimTypes.Name, user.userID.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["JWT_ISSUER"],
                Audience = _configuration["JWET_AUDIENCE"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });

        }

    }
}