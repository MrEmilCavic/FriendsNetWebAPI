using System.ComponentModel.DataAnnotations;

namespace FriendsNetWebAPI.Models
{
    public class Interests
    {
        [Key]
        public int interestID { get; set; }
        public string Title { get; set; }
        public int categoriesID { get; set; }
        public string interestsImg { get; set; }
    }
}