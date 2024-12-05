using System.ComponentModel.DataAnnotations;

namespace FriendsNetWebAPI.Models
{
    public class NewUser
    {
        [Key]
        public int UserID { get; set; }
        public string userIdentification { get; set; }
        public string secret { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}