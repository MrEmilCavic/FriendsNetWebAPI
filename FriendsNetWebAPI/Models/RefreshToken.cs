using System.ComponentModel.DataAnnotations;

namespace FriendsNetWebAPI.Models
{
    public class RefreshToken
    {
        [Key]
        public int TokenId { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }

    }
}
