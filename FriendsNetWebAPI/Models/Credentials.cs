using System.ComponentModel.DataAnnotations;

namespace FriendsNetWebAPI.Models
{
    public class Credentials
    {
        [Key]
        public int userID { get; set; }
        public string secret { get; set; }
        public string userIdentification { get; set; }
    }
}