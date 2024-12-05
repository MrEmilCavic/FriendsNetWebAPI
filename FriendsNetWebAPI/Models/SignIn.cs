using System.ComponentModel.DataAnnotations;

namespace FriendsNetWebAPI.Models
{
    public class SignIn
    {
        [EmailAddress]
        public string userIdentification { get; set; }
        public string Secret { get; set; }
    }
}