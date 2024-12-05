using System.ComponentModel.DataAnnotations;

namespace FriendsNetWebAPI.Models
{
    public class Profiles
    {
        [Key]
        public int userID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Gender { get; set; }
        public string? Phonenumber { get; set; }
        public string? Email { get; set; }
        public string? Town { get; set; }
        public string? ZIP { get; set; }
        public string? Interestprofile { get; set; }
        public string? ProfileImg { get; set; }
        public string? Descriptiontxt { get; set; }
    }
}