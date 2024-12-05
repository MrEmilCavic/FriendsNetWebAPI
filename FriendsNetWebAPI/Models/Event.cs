using System.ComponentModel.DataAnnotations;

namespace FriendsNetWebAPI.Models
{
    public class Event
    {
        [Key]
        public int eventsID { get; set; }
        public string Title { get; set; }
        public DateTime eventTime { get; set; }
        public string Place { get; set; }
        public int createdBy { get; set; }
        public string? Description { get; set; }
        public string? banner { get; set; }
        public string? invited { get; set; }
        public string? accepted { get; set; }
        public string? rejected { get; set; }
    }
}