using System.ComponentModel.DataAnnotations;
namespace FriendsNetWebAPI.Models
{
    public class EventDiscussion
    {
        [Key]
        public int eventsID { get; set; }
        public int userID { get; set; }
        public string discussiontxt { get; set; }

    }
}