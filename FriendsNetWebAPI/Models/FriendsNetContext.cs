using Microsoft.EntityFrameworkCore;
using FriendsNetWebAPI.Models;

namespace FriendsNetWebAPI.Models
{
    public class FriendsNetContext : DbContext
    {
        public FriendsNetContext(DbContextOptions<FriendsNetContext> options) : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<FriendsNetWebAPI.Models.Credentials> Credentials { get; set; } = default!;
        public DbSet<FriendsNetWebAPI.Models.EventDiscussion> EventDiscussion { get; set; } = default!;
        public DbSet<FriendsNetWebAPI.Models.Interests> Interests { get; set; } = default!;
        public DbSet<FriendsNetWebAPI.Models.Profiles> Profiles { get; set; } = default!;
    }
}