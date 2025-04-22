using mentalHealth.Models;
using Microsoft.EntityFrameworkCore;
using mentalHealth.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace mentalHealth.Data // Or MVC_Project.Data if using a Data folder
{
    public class AppDbContext : DbContext
    {
        // Constructor fix (add this)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Therapist> Therapists { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ProgressTracking> ProgressTrackings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Session>()
                .HasOne(s => s.Client)
                .WithMany(u => u.BookedSessions)
                .OnDelete(DeleteBehavior.Restrict); // Fixed: Now recognized

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Client)
                .WithMany(u => u.AuthoredReviews)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Session>()
    .HasOne(s => s.Client)
    .WithMany(u => u.BookedSessions)
    .HasForeignKey(s => s.ClientId)
    .OnDelete(DeleteBehavior.Restrict);

        }

    }
}