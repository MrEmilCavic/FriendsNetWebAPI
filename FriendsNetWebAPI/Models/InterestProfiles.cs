using System.ComponentModel.DataAnnotations;

namespace FriendsNetWebAPI.Models
{
    public class InterestProfiles
    {
        [Key]
        public int UserID { get; set; }
        public string? InterestProfile { get; set; }
    }
}
