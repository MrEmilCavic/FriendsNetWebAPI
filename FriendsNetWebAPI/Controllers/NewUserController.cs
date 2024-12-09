using FriendsNetWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace FriendsNetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewUserController : Controller
    {
        private readonly FriendsNetContext _context;

        public NewUserController(FriendsNetContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> NewUser([FromBody] NewUser model)
        {
            if (model == null)
            {
                return BadRequest("Oopsie! Bad request");
            }

            if (model.secret != model.ConfirmPassword)
            {
                return BadRequest("Ai caramba! The passwords don't match");
            }

            string hashedSecret = SecretHasher.HashSecret(model.secret);

            var user = new Credentials
            {
                userIdentification = model.userIdentification,
                secret = hashedSecret
            };

            _context.Credentials.Add(user);
            await _context.SaveChangesAsync();

            if (string.IsNullOrEmpty(user.userID.ToString())) 
                {
                throw new InvalidOperationException("user.userID ish empty D:");
            } else
            {
                Console.WriteLine(user.userID);
            }

            var newProfile = new Profiles
            {
                userID = user.userID,
                FirstName = "Bewildered",
                LastName = "Kanelsnegl"
            };

            _context.Profiles.Add(newProfile);
            await _context.SaveChangesAsync();

            var newInterestProfile = new InterestProfiles
            {
                UserID = user.userID
            };

            _context.InterestProfiles.Add(newInterestProfile);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Hooray! Welcome to the community!" });
        }
    }
}