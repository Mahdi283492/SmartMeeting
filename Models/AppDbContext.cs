using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace SmartMeetingAPI.Models
{
  public class AppDbContext 
        : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

      
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<Attendee> Attendees { get; set; }
        public DbSet<Minutes> Minutes { get; set; }
        public DbSet<ActionItem> ActionItems { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Attendee>()
                .HasKey(a => new { a.MeetingID, a.UserID });

            modelBuilder.Entity<Attendee>()
                .HasOne(a => a.User)
                .WithMany(u => u.Attendees)
                .HasForeignKey(a => a.UserID)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Attendee>()
                .HasOne(a => a.Meeting)
                .WithMany(m => m.Attendees)
                .HasForeignKey(a => a.MeetingID);
            modelBuilder.Entity<ActionItem>()
.HasOne(a => a.User)
.WithMany(u => u.AssignedItems)
.HasForeignKey(a => a.AssignedTo)
.OnDelete(DeleteBehavior.Restrict);
        }

    }
}
